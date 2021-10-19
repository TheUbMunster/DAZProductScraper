using System.Collections;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Jpeg;
//using System.Diagnostics;
using static DazQuickviewManager;

public static class ImageProcessor
{
#if DEBUG
   private static List<double> imageLoadingTimes = new List<double>();
   public static double ImageLoadingTime { get => imageLoadingTimes.Average(); }
   private static List<double> imageResizeTimes = new List<double>();
   public static double ImageResizeTime { get => imageResizeTimes.Average(); }
   private static List<double> imageDrawTimes = new List<double>();
   public static double ImageDrawTime { get => imageDrawTimes.Average(); }
   private static List<double> imageSaveTimes = new List<double>();
   public static double ImageSaveTime { get => imageSaveTimes.Average(); }
#endif

   /// <summary>
   /// Generates an image stitched together from the images in the imageUrls. The first image in the array must be the main image.
   /// </summary>
   /// <param name="imageUrls">The urls of the images to stitch together, the one at index 0 being the main image.</param>
   /// <param name="fileName">The name of the file to save the image to (WITHOUT the file extension).</param>
   /// <param name="maxNumberOfSmallImagesPerThumbnail">The max number of small images to put on a thumbnail.</param>
   /// <param name="maxNonMainImages">The number of images (not including the main image) that will be processed into thumbnails.</param>
   /// <param name="maxThumbnails">The max number of thumbnails that will be made from the imageUrls.</param>
   /// <returns>A task representing the process</returns>
   public static async Task GenerateImage(string[] imageUrls, string fileName, int maxNumberOfSmallImagesPerThumbnail = 16, int maxNonMainImages = 32, int maxThumbnails = 2)
   {
      #region Argument Checking
      if (imageUrls == null)
      {
         throw new ArgumentException("Make sure imageUrls isn't null");
      }
      else if (imageUrls.Length == 0)
      {
         return; //no image can be made with nothing.
      }
      else if (maxNumberOfSmallImagesPerThumbnail < 0 || maxNumberOfSmallImagesPerThumbnail > 16)
      {
         throw new ArgumentException("Make sure maxNumberOfSmallImagesPerThumbnail is between 0 and 16.");
      }
      else if (maxNonMainImages < 0)
      {
         throw new ArgumentException("Make sure maxNonMainImages is greater than or equal to 0.");
      }
      else if (maxThumbnails < 1)
      {
         return; //no thumbnails to make
      }
      #endregion

      try
      {
         #region local declarations
         const int ioConcurrency = 4;

         Configuration config = new Configuration(new PngConfigurationModule(), new JpegConfigurationModule());
         config.MaxDegreeOfParallelism = 1;
         config.SetGraphicsOptions(x =>
         {
            x.Antialias = false;
         });

         string mainImageUrl = imageUrls[0];
         int smallImageUncheckedCount = imageUrls.Length - 1;
         (int width, int height) resultDimensions = DazQuickviewManager.FetchConfig.GetResolution(DazQuickviewManager.fetchConfig.Resolution);

         List<Image<Rgb24>> miniImages = new List<Image<Rgb24>>();
         Image<Rgb24> mainImage = null;
         #endregion

         //only do main image
         if (maxNonMainImages == 0 || maxNumberOfSmallImagesPerThumbnail == 0) //edge case
         {
            WebClient wc = new WebClient();
            byte[] result = await wc.DownloadDataTaskAsync(mainImageUrl);
            wc.Dispose();
            if (result != null)
            {
               await GenerateImage(result, fileName);
            }
            return;
         }

         //load the image data from the website urls
         using (SemaphoreSlim semaphore = new SemaphoreSlim(ioConcurrency, ioConcurrency))
         {
            List<Task<byte[]>> dataFetches = new List<Task<byte[]>>();

            #region fetch binary
            for (int i = 0; i < imageUrls.Length; i++)
            {
               string url = imageUrls[i];
               await semaphore.WaitAsync();
               dataFetches.Add(Task.Factory.StartNew(() =>
               {
                  WebClient wc = null;
                  try
                  {
                     wc = new WebClient();
                     //return await wc.DownloadDataTaskAsync(imageUrls[i]); //this task thread leaves and (probably) another gets spun up to deal with the after await part.
                     byte[] result = wc.DownloadData(url);
                     //the data *should* be good.
                     return result;
                  }
                  catch
                  {
                     return null;
                  }
                  finally
                  {
                     semaphore.Release();
                     wc?.Dispose();
                  }
               }, programwideCancellation.Token));
            }
            #endregion

            await Task.WhenAll((IEnumerable<Task>)dataFetches); //cast as resultless task array so it doesn't try to aggregate the results. (could access cancelled results).
            if (programwideCancellation.IsCancellationRequested)
            {
               dataFetches.ForEach(x => x.Dispose());
               return;
            }

            //otherwise all of the tasks must have completed before a cancellation was requested. Since we got this far and we
            //have all the info we need for the images, even if it does get cancelled, just finish it up.

            #region Loading images
            for (int i = 0; i < dataFetches.Count; i++)
            {
               if (dataFetches[i].Result != null)
               {
#if DEBUG
                  System.Diagnostics.Stopwatch stl = new System.Diagnostics.Stopwatch();
                  stl.Start();
#endif

                  using (MemoryStream ms = new MemoryStream(dataFetches[i].Result))
                  {
                     ms.Seek(0, SeekOrigin.Begin);
                     Image<Rgb24> img;
                     try
                     {
                        img = await Image.LoadAsync<Rgb24>(config, ms);
                     }
                     catch
                     {
                        img = null; //bad formatting probably (errors during image download? not likely with modern error correction).
                     }
                     if (i != 0)
                     {
                        if (img != null && miniImages.Count < maxNonMainImages)
                        {
                           miniImages.Add(img);
                        }
                     }
                     else
                     {
                        mainImage = img;
                     }
                  }

#if DEBUG
                  stl.Stop();
                  imageLoadingTimes.Add(stl.ElapsedMilliseconds / 1000d);
#endif
               }
            }
            #endregion

            dataFetches.ForEach(x => x.Dispose());

            #region If none of the mini images were loaded, try to save just main image
            if (miniImages.Count == 0) //no small images loaded.
            {
               //apparently we have/don't want no small images.
               //no need to dispose smallImages because it's empty.
               if (mainImage != null)
               {
                  mainImage.Mutate(o =>
                  {
                     o.Resize(new Size((int)(resultDimensions.height * (10f / 13f)), resultDimensions.height));
                     o.Pad(resultDimensions.width, resultDimensions.height, Color.Black);
                  });
                  using (MemoryStream ms = new MemoryStream())
                  {
                     mainImage.Save(ms, new JpegEncoder() { Quality = DazQuickviewManager.fetchConfig.JpgQuality });
                     FileStream fs = File.Create(fetchConfig.SaveDirectory + "\\0-" + fileName + ".jpg");
                     ms.Seek(0, SeekOrigin.Begin);
                     ms.CopyTo(fs);
                     fs.Dispose();
                  }
                  mainImage.Dispose();
               }
               return; //if we just made an image we're done, else if no mini images, no main image, do nothing.
            }
            #endregion
         }

         #region Calculate mini image dimensions
         Func<int, int, int, int> Clamp = (v, min, max) =>
         {
            return v > max ? max : (v < min ? min : v);
         };
         int rowColCount = (int)Math.Ceiling(Math.Sqrt(Clamp(Math.Min(miniImages.Count, maxNumberOfSmallImagesPerThumbnail), 0, 16))); //should never be 0

         int totalWidthPX = resultDimensions.width;
         int unusedWidthPX = (int)(resultDimensions.height * ((16d / 9d) - (10d / 13d))); //int cast is analogous to floor(x)
         //calculate fit if height of mini images is maxed.
         //width is unusedWidthPX, height is resultDimensions.height
         //to max the height, set the width of the grid of images to the unusedWidth
         int mhMiniImageWidth = unusedWidthPX / rowColCount;
         int mhMiniImageHeight = (int)((double)mhMiniImageWidth * (13d / 10d));
         bool mhFit = mhMiniImageHeight * rowColCount <= resultDimensions.height; //does it fit?
         //calculate fit if width of mini images is maxed.
         //width is unusedWidthPX, height is resultDimensions.height
         //to max the width, set the height of the grid of images to the unusedHeight (height)
         int mwMiniImageHeight = resultDimensions.height / rowColCount;
         int mwMiniImageWidth = (int)((double)mwMiniImageHeight * (10d / 13d));
         bool mwFit = mwMiniImageWidth * rowColCount <= unusedWidthPX; //does it fit in the unused width?

         int miniImageWidth;
         int miniImageHeight;
         if (mhFit) //pick the correct dimensions. (one of the two should always fit).
         {
            miniImageWidth = mhMiniImageWidth;
            miniImageHeight = mhMiniImageHeight;
         }
         else if (mwFit)
         {
            miniImageWidth = mwMiniImageWidth;
            miniImageHeight = mwMiniImageHeight;
         }
         else
         {
            throw new Exception("This should never happen? Maybe an off by one error.");
         }
         #endregion

         #region resize the image
#if DEBUG
         System.Diagnostics.Stopwatch str = new System.Diagnostics.Stopwatch();
         str.Start();
#endif
         //size the big images up.
         mainImage?.Mutate(x =>
         {
            x.Resize((int)(resultDimensions.height * (10d / 13d)), resultDimensions.height, KnownResamplers.Triangle); //size to max;
         });
         //size the mini images down.
         miniImages.ForEach(x =>
         {
            x.Mutate(o =>
            {
               o.Resize(miniImageWidth, miniImageHeight, KnownResamplers.Triangle); //TODO: hit this loop with the 1-2 multithread semaphore combo?
            });
         });

#if DEBUG
         str.Stop();
         imageResizeTimes.Add(str.ElapsedMilliseconds / 1000d);
#endif
         #endregion

         //put the images in the result image.
         int rootLeftMainImage = (resultDimensions.width - (((int)(resultDimensions.height * (10d / 13d))) + (rowColCount * miniImageWidth))) / 2;
         int rootLeftMiniImages = (rootLeftMainImage + ((int)(resultDimensions.height * (10d / 13d))));

         List<Image<Rgb24>> resultImages = new List<Image<Rgb24>>();
         int numberOfImages = (int)Math.Ceiling((double)miniImages.Count / (double)maxNumberOfSmallImagesPerThumbnail);
         numberOfImages = maxThumbnails < numberOfImages ? maxThumbnails : numberOfImages;

         for (int i = 0; i < numberOfImages; i++)
         {
            Image<Rgb24> resultImage = new Image<Rgb24>(config, resultDimensions.width, resultDimensions.height, new Rgb24(0, 0, 0));
            resultImages.Add(resultImage);

            #region Draw images onto result image
#if DEBUG
            System.Diagnostics.Stopwatch std = new System.Diagnostics.Stopwatch();
            std.Start();
#endif

            resultImage.Mutate(o =>
            {
               //main image
               if (mainImage != null)
               {
                  o.DrawImage(mainImage, new Point(rootLeftMainImage, 0), 1f);
               }
               //sub images
               int max = (i + 1) * maxNumberOfSmallImagesPerThumbnail;
               max = max > miniImages.Count ? miniImages.Count : max;
               for (int si = i * maxNumberOfSmallImagesPerThumbnail; si < max; si++)
               {
                  int pi = si - (i * maxNumberOfSmallImagesPerThumbnail);
                  int x = pi % rowColCount;
                  int y = (pi - x) / rowColCount;
                  if (miniImages[si] != null)
                  {
                     o.DrawImage(miniImages[si], new Point(rootLeftMiniImages + (x * miniImageWidth), y * miniImageHeight), 1f);
                  }
               }
            });

#if DEBUG
            std.Stop();
            imageDrawTimes.Add(std.ElapsedMilliseconds / 1000d);
#endif
            #endregion
         }

         //DISPOSE THE IMAGES
         foreach (Image<Rgb24> i in miniImages)
         {
            i?.Dispose();
         }
         mainImage?.Dispose();

         #region save result images
#if DEBUG
         System.Diagnostics.Stopwatch svt = new System.Diagnostics.Stopwatch();
         svt.Start();
#endif
         for (int i = 0; i < resultImages.Count; i++)
         {
            using (MemoryStream ms = new MemoryStream())
            {
               resultImages[i].Save(ms, new JpegEncoder() { Quality = DazQuickviewManager.fetchConfig.JpgQuality });
               FileStream fs = File.Create(fetchConfig.SaveDirectory + $"\\{i}-" + fileName + ".jpg");
               ms.Seek(0, SeekOrigin.Begin);
               ms.CopyTo(fs);
               fs.Dispose();
            }
            resultImages[i].Dispose();
         }

#if DEBUG
         svt.Stop();
         imageSaveTimes.Add(svt.ElapsedMilliseconds / 1000d);
#endif
         #endregion
      }
      catch (Exception e)
      {
         Console.WriteLine("BADBADBAD\n\n" + e.GetType().ToString() + " : " + e.Message + "\n\n" + e.StackTrace);
      }
   }

   /// <summary>
   /// Generates the image from the Base64Encoding image embedded in the products library page. (Used when the product page images aren't avaliable).
   /// </summary>
   /// <param name="imageData">The bytes data of the image</param>
   /// <param name="fileName">The name of the file to save the image to (WITHOUT the file extension).</param>
   /// <returns>A task representing the process</returns>
   public static async Task GenerateImage(byte[] imageData, string fileName)
   {
      Configuration config = new Configuration(new PngConfigurationModule(), new JpegConfigurationModule());
      config.MaxDegreeOfParallelism = 1;
      config.SetGraphicsOptions(x =>
      {
         x.Antialias = false;
      });
      (int width, int height) resultDimensions = DazQuickviewManager.FetchConfig.GetResolution(DazQuickviewManager.fetchConfig.Resolution);
      await Task.Run(() =>
      {
         using (Image<Rgb24> image = Image.Load<Rgb24>(config, imageData))
         {
            image.Mutate(o =>
            {
               o.Resize(new Size((int)(resultDimensions.height * (10f / 13f)), resultDimensions.height));
               o.Pad(resultDimensions.width, resultDimensions.height, Color.Black);
            });

            using (MemoryStream ms = new MemoryStream())
            {
               image.Save(ms, new JpegEncoder() { Quality = DazQuickviewManager.fetchConfig.JpgQuality });
               using (FileStream fs = File.Create(fetchConfig.SaveDirectory + $"\\0-" + fileName + ".jpg"))
               {
                  ms.Seek(0, SeekOrigin.Begin);
                  ms.CopyTo(fs);
                  fs.Dispose();
               }
            }
         }
      }, programwideCancellation.Token);
   }
}
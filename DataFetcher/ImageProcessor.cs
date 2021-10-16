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
   /// <summary>
   /// Generates an image stitched together from the images in the imageUrls. The first image in the array must be the main image.
   /// </summary>
   /// <param name="imageUrls">The urls of the images to stitch together, the one at index 0 being the main image.</param>
   /// <param name="saveToFilePath">The absolute file path to save the image (including the file extension).</param>
   /// <param name="maxNumberOfSmallImagesPerThumbnail">The max number of small images to put on a thumbnail.</param>
   /// <param name="maxNonMainImages">The number of images (not including the main image) that will be processed into thumbnails.</param>
   /// <param name="maxThumbnails">The max number of thumbnails that will be made from the imageUrls.</param>
   /// <returns>A task representing the process</returns>
   public static async Task GenerateImage(string[] imageUrls, string saveToFilePath, int maxNumberOfSmallImagesPerThumbnail = 16, int maxNonMainImages = 32, int maxThumbnails = 2)
   {
      try
      {
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
         int smallImageCheckedCount = 0;

         List<Image<Rgb24>> images = new List<Image<Rgb24>>();
         //load the image data from the website urls
         using (SemaphoreSlim semaphore = new SemaphoreSlim(ioConcurrency, ioConcurrency))
         {
            List<Task<byte[]>> dataFetches = new List<Task<byte[]>>();
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
                  catch (Exception)
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
            await Task.WhenAll((IEnumerable<Task>)dataFetches); //cast as resultless task array so it doesn't try to aggregate the results. (could access cancelled results).
            if (programwideCancellation.IsCancellationRequested)
            {
               return;
            }
            //otherwise all of the tasks must have completed before a cancellation was requested. Since we got this far and we
            //have all the info we need for the images, even if it does get cancelled, just finish it up.
            bool mainImageSucessfullyLoaded = false;
            for (int i = 0; i < dataFetches.Count; i++)
            {
               if (dataFetches[i].Result != null)
               {
                  //System.Diagnostics.Stopwatch stl = new System.Diagnostics.Stopwatch();
                  //stl.Start();

                  using (MemoryStream ms = new MemoryStream(dataFetches[i].Result))
                  {
                     ms.Seek(0, SeekOrigin.Begin);
                     images.Add(await Image.LoadAsync<Rgb24>(config, ms));
                  }

                  //stl.Stop();
                  //Debug.Log("Loadtime: " + (stl.ElapsedMilliseconds / 1000d));
                  if (i != 0)
                  {
                     smallImageCheckedCount++; //only increment if it's not the main image.
                  }
                  else
                  {
                     mainImageSucessfullyLoaded = true;
                  }
               }
            }
            if (smallImageCheckedCount == 0 || maxNumberOfSmallImagesPerThumbnail == 0)
            {
               //apparently we have/don't want no small images.
               foreach (Image i in images)
               {
                  i.Dispose();
               }
               if (mainImageSucessfullyLoaded)
               {
                  await GenerateImage(dataFetches[0].Result, saveToFilePath);
                  return;
               }
            }
         }
         Func<int, int, int, int> Clamp = (v, min, max) => {
            return v > max ? max : (v < min ? min : v);
         };
         int rowColCount = (int)Math.Ceiling(Math.Sqrt(Clamp(Math.Min(smallImageCheckedCount, maxNumberOfSmallImagesPerThumbnail), 0, 16)));

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

         await Task.Factory.StartNew(() =>
         {
            //System.Diagnostics.Stopwatch str = new System.Diagnostics.Stopwatch();
            //str.Start();
            //size the big images up.
            images[0]?.Mutate(x =>
            {
               x.Resize(new Size((int)(resultDimensions.height * (10d / 13d)), resultDimensions.height)); //size to max;
            });
            //size the mini images down.
            for (int i = 1; i < images.Count; i++)
            {
               if (images[i] != null)
               {
                  images[i].Mutate(x =>
                  {
                     ResizeOptions ro = new ResizeOptions();
                     ro.Size = new Size(miniImageWidth, miniImageHeight);
                     ro.Sampler = KnownResamplers.Bicubic;
                     x.Resize(ro); //TODO: hit this loop with the 1-2 multithread semaphore combo?
                  });
               }
            }
            //str.Stop();
            //Debug.Log("Resizetime: " + (str.ElapsedMilliseconds / 1000d));
            //put the images in the result image.
            int rootLeftMainImage = (resultDimensions.width - (((int)(resultDimensions.height * (10d / 13d))) + (rowColCount * miniImageWidth))) / 2;
            int rootLeftMiniImages = (rootLeftMainImage + ((int)(resultDimensions.height * (10d / 13d))));
            Image<Rgb24> resultImage = new Image<Rgb24>(config, resultDimensions.width, resultDimensions.height, new Rgb24(0, 0, 0)); // break out into list and do multiples for many thumbnails.
            //System.Diagnostics.Stopwatch std = new System.Diagnostics.Stopwatch();
            //std.Start();
            resultImage.Mutate(o =>
            {
               //main image
               if (images[0] != null)
               {
                  o.DrawImage(images[0], new Point(rootLeftMainImage, 0), 1f);
               }
               //sub images
               for (int i = 1; i < smallImageCheckedCount + 1; i++)
               {
                  int x = (i - 1) % rowColCount;
                  int y = ((i - 1) - x) / rowColCount;
                  if (images[i] != null)
                  {
                     o.DrawImage(images[i], new Point(rootLeftMiniImages + (x * miniImageWidth), y * miniImageHeight), 1f);
                  }
               }
            });
            //std.Stop();
            //Debug.Log("Drawtime: " + (std.ElapsedMilliseconds / 1000d));
            //DISPOSE THE IMAGES
            foreach (Image<Rgb24> i in images)
            {
               i?.Dispose();
            }
            using (MemoryStream ms = new MemoryStream())
            {
               //System.Diagnostics.Stopwatch svt = new System.Diagnostics.Stopwatch();
               //svt.Start();
               resultImage.Save(ms, new JpegEncoder() { Quality = DazQuickviewManager.fetchConfig.JpgQuality });
               FileStream fs = File.Create(saveToFilePath);
               ms.Seek(0, SeekOrigin.Begin);
               ms.CopyTo(fs);
               fs.Dispose();
               //svt.Stop();
               //Debug.Log("Resizetime: " + (svt.ElapsedMilliseconds / 1000d));
            }
            //byte[] dat = MemoryMarshal.AsBytes(resultImage.GetPixelMemoryGroup().ToArray()[0].Span).ToArray();
            resultImage.Dispose();
            //resultImage.SaveAsJpeg(saveToFilePath, new JpegEncoder() { Quality = ImageScooper.fetchConfig.JpgQuality });
         });
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
   /// <param name="saveToFilePath">The absolute file path to save the image (including the file extension).</param>
   /// <returns>A task representing the process</returns>
   public static async Task GenerateImage(byte[] imageData, string saveToFilePath)
   {
      (int width, int height) resultDimensions = DazQuickviewManager.FetchConfig.GetResolution(DazQuickviewManager.fetchConfig.Resolution);
      Image<Rgb24> resultImage = new Image<Rgb24>(resultDimensions.width, resultDimensions.height, new Rgb24(0, 0, 0));
      Image<Rgb24> loadedImage = Image.Load<Rgb24>(imageData);
      await Task.Factory.StartNew(() =>
      {
         loadedImage.Mutate(o =>
         {
            o.Resize(new Size(resultDimensions.height * (int)(10f / 13f), resultDimensions.height));
         });
         resultImage.Mutate(o =>
         {
            o.DrawImage(loadedImage, new Point((int)((resultDimensions.width / 2f) - ((resultDimensions.height * (10f / 13f)) / 2f)), 0), 1f);
         });
         resultImage.SaveAsJpeg(saveToFilePath, new JpegEncoder() { Quality = DazQuickviewManager.fetchConfig.JpgQuality });
      }, programwideCancellation.Token);
   }
}
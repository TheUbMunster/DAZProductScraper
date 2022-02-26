//#define IMAGESHARP_IMAGE_PROCESSING


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;
#if IMAGESHARP_IMAGE_PROCESSING
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Jpeg;
#else
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
#endif
//using System.Diagnostics;
using static DAZScraperModel;

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

#if IMAGESHARP_IMAGE_PROCESSING
         Configuration config = new Configuration(new PngConfigurationModule(), new JpegConfigurationModule());
         config.MaxDegreeOfParallelism = 1;
         config.SetGraphicsOptions(x =>
         {
            x.Antialias = false;
         });
#endif

         int smallImageUncheckedCount = imageUrls.Length - 1;
         (int width, int height) resultDimensions = DAZScraperModel.Config.GetResolution(DAZScraperModel.fetchConfig.Resolution);

#if IMAGESHARP_IMAGE_PROCESSING
         List<Image<Rgb24>> miniImages = new List<Image<Rgb24>>();
         Image<Rgb24> mainImage = null;
#else
         List<Image> miniImages = new List<Image>();
         Image mainImage = null;
#endif
         #endregion

         //only do main image
         if (maxNonMainImages == 0 || maxNumberOfSmallImagesPerThumbnail == 0) //edge case
         {
            byte[] result = null;
            using (WebClient wc = new WebClient())
            {
               result = await wc.DownloadDataTaskAsync(imageUrls[0]);
            }
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
               dataFetches.Add(Task.Run(async () =>
               {
                  WebClient wc = null;
                  try
                  {
                     wc = new WebClient();
                     //return await wc.DownloadDataTaskAsync(imageUrls[i]); //this task thread leaves and (probably) another gets spun up to deal with the after await part.
                     byte[] result = (await wc.DownloadDataTaskAsync(url));
                     //the data *should* be good.
                     return result;
                  }
                  catch
                  {
                     return (byte[])null;
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
#if IMAGESHARP_IMAGE_PROCESSING
                  Image<Rgb24> img;
#else
                  Image img;
#endif
                  if (miniImages.Count >= maxNonMainImages)
                  {
                     break; // we have exceeded the number of images we would need, don't waste computing power generating the rest.
                  }
                  try
                  {
#if IMAGESHARP_IMAGE_PROCESSING
                     img = Image.Load<Rgb24>(config, dataFetches[i].Result);
#else
                     using (MemoryStream ms = new MemoryStream(dataFetches[i].Result))
                     {
                        img = Image.FromStream(ms);
                     }
#endif
                  }
                  catch
                  {
                     img = null;
                  }
                  if (i != 0)
                  {
                     if (img != null)
                     {
                        miniImages.Add(img);
                     }
                  }
                  else
                  {
                     mainImage = img;
                  }
#if DEBUG
                  stl.Stop();
                  imageLoadingTimes.Add(stl.ElapsedMilliseconds / 1000d);
#endif
               }
            }
            #endregion

            dataFetches.ForEach(x => x.Dispose());
            dataFetches = null;
         }

         //the main and sub images are now loaded.
         #region Resize individual images
         int maxSubWidth, maxSubHeight;
         Func<int, int, int, int> Clamp = (v, min, max) =>
         {
            return v > max ? max : (v < min ? min : v);
         };
         int rowColCount = (int)Math.Ceiling(Math.Sqrt(Clamp(Math.Min(miniImages.Count, maxNumberOfSmallImagesPerThumbnail), 0, 16))); //should never be 0
         maxSubWidth = maxSubHeight = (int)((double)resultDimensions.height / (double)rowColCount);
         #region Resize Main image
         //size the main images up.
         //#if DEBUG
         //         System.Diagnostics.Stopwatch strm = new System.Diagnostics.Stopwatch();
         //         strm.Start();
         //#endif
         //#if IMAGESHARP_IMAGE_PROCESSING
         //         mainImage?.Mutate(x =>
         //         {
         //            ResizeOptions resizeOpt = new ResizeOptions()
         //            {
         //               Mode = ResizeMode.Max,
         //               //Size = new Size(maxMainWidth, maxMainHeight),
         //               Size = new Size(int.MaxValue, resultDimensions.height),
         //               Sampler = KnownResamplers.Triangle
         //            };
         //            x.Resize(resizeOpt);
         //         });
         //#else
         //         if (mainImage != null)
         //         {
         //            int height = resultDimensions.height;
         //            int width = (int)Math.Floor((float)mainImage.Width * ((float)resultDimensions.height / (float)mainImage.Height));
         //            Rectangle destRect = new Rectangle(0, 0, width, height);

         //            Bitmap tempMainResize = new Bitmap(width, height);
         //            tempMainResize.SetResolution(mainImage.HorizontalResolution, mainImage.VerticalResolution);
         //            using (var graphics = Graphics.FromImage(tempMainResize))
         //            {
         //               graphics.CompositingMode = CompositingMode.SourceCopy;
         //               graphics.CompositingQuality = CompositingQuality.HighQuality;
         //               graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
         //               graphics.SmoothingMode = SmoothingMode.HighQuality;
         //               graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

         //               using (var wrapMode = new ImageAttributes())
         //               {
         //                  wrapMode.SetWrapMode(WrapMode.TileFlipXY);
         //                  graphics.DrawImage(mainImage, destRect, 0, 0, mainImage.Width, mainImage.Height, GraphicsUnit.Pixel, wrapMode);
         //               }
         //            }
         //            mainImage.Dispose();
         //            mainImage = tempMainResize;
         //         }
         //#endif
         //#if DEBUG
         //         strm.Stop();
         //         imageResizeTimes.Add(strm.ElapsedMilliseconds / 1000d);
         //#endif
         #endregion
         //resize the mini images.
#if IMAGESHARP_IMAGE_PROCESSING
         miniImages.ForEach(x =>
         {
#if DEBUG
            System.Diagnostics.Stopwatch str = new System.Diagnostics.Stopwatch();
            str.Start();
#endif
            ResizeOptions resizeOpt = new ResizeOptions()
            {
               Mode = ResizeMode.Max,
               Size = new Size(maxSubWidth, maxSubHeight),
               Sampler = KnownResamplers.Triangle
            };
            x.Mutate(o =>
            {
               o.Resize(resizeOpt);
            });
#if DEBUG
            str.Stop();
            imageResizeTimes.Add(str.ElapsedMilliseconds / 1000d);
#endif
         });
#else
         for (int i = 0; i < miniImages.Count; i++)
         {
#if DEBUG
            System.Diagnostics.Stopwatch str = new System.Diagnostics.Stopwatch();
            str.Start();
#endif
            float widthDivHeight = (float)miniImages[i].Width / (float)miniImages[i].Height;

            int width;
            int height;
            if (widthDivHeight < 1f)
            {
               //max out height (portrait)
               height = maxSubHeight;
               width = (int)((float)maxSubHeight * widthDivHeight);
            }
            else if (widthDivHeight > 1f)
            {
               //max out width (landscape)
               height = (int)((float)maxSubWidth / widthDivHeight);
               width = maxSubWidth;
            }
            else
            {
               //square
               width = maxSubWidth;
               height = maxSubHeight;
            }
            //int height = resultDimensions.height;
            //int width = (int)Math.Floor((float)subImage.Width * ((float)resultDimensions.height / (float)subImage.Height));
            Rectangle destRect = new Rectangle(0, 0, width, height);

            Bitmap tempSubResize = new Bitmap(width, height);
            //tempSubResize.SetResolution(miniImages[i].HorizontalResolution, miniImages[i].VerticalResolution);
            using (var graphics = Graphics.FromImage(tempSubResize))
            {
               graphics.CompositingMode = CompositingMode.SourceCopy;
               graphics.CompositingQuality = CompositingQuality.HighQuality;
               graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
               graphics.SmoothingMode = SmoothingMode.HighQuality;
               graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

               using (var wrapMode = new ImageAttributes())
               {
                  wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                  graphics.DrawImage(miniImages[i], destRect, 0, 0, miniImages[i].Width, miniImages[i].Height, GraphicsUnit.Pixel, wrapMode);
               }
            }
            miniImages[i].Dispose();
            miniImages[i] = tempSubResize;
#if DEBUG
            str.Stop();
            imageResizeTimes.Add(str.ElapsedMilliseconds / 1000d);
#endif
         }
#endif
         #endregion

         #region If none of the mini images were loaded, try to save just main image
         if (miniImages.Count == 0) //no small images loaded.
         {
            //apparently we have/don't want no small images.
            //no need to dispose smallImages because it's empty.
            if (mainImage != null)
            {
#if IMAGESHARP_IMAGE_PROCESSING
               ResizeOptions resizeOpt = new ResizeOptions()
               {
                  Mode = ResizeMode.Max,
                  Size = new Size(resultDimensions.width, resultDimensions.height),
                  Sampler = KnownResamplers.Triangle
               };
               mainImage.Mutate(o =>
               {
                  o.Resize(resizeOpt);
                  o.Pad(resultDimensions.width, resultDimensions.height, Color.Black);
               });
               using (MemoryStream ms = new MemoryStream())
               {
                  mainImage.Save(ms, new JpegEncoder() { Quality = DazQuickviewManager.fetchConfig.JpgQuality });
                  using (FileStream fs = File.Create(FetchConfig.GetLibrarySaveDirectory() + "\\" + fileName + "-0.jpg"))
                  {
                     ms.Seek(0, SeekOrigin.Begin);
                     ms.CopyTo(fs);
                     await fs.FlushAsync();
                  }
               }
               mainImage.Dispose();
#else

               float widthDivHeight = (float)mainImage.Width / (float)mainImage.Height;

               int width;
               int height;
               int x;
               int y;
               if (widthDivHeight < 1f)
               {
                  //max out height (portrait)
                  height = resultDimensions.height;
                  width = (int)((float)resultDimensions.height * widthDivHeight);
                  x = (int)((resultDimensions.width / 2f) - (width / 2f));
                  y = 0;
               }
               else if (widthDivHeight > 1f)
               {
                  //max out width (landscape)
                  height = (int)((float)resultDimensions.width / widthDivHeight);
                  width = resultDimensions.width;
                  x = 0;
                  y = (int)((resultDimensions.height / 2f) - (height / 2f));
               }
               else
               {
                  //square
                  width = resultDimensions.width;
                  height = resultDimensions.height;
                  x = y = 0;
               }
               
               Rectangle destRect = new Rectangle(x, y, width, height);

               Bitmap tempMainResize = new Bitmap(resultDimensions.width, resultDimensions.height);
               //tempMainResize.SetResolution(mainImage.HorizontalResolution, mainImage.VerticalResolution);
               using (var graphics = Graphics.FromImage(tempMainResize))
               {
                  graphics.CompositingMode = CompositingMode.SourceCopy;
                  graphics.CompositingQuality = CompositingQuality.HighQuality;
                  graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                  graphics.SmoothingMode = SmoothingMode.HighQuality;
                  graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                  //graphics.DrawImageUnscaled(mainImage, (int)((resultDimensions.width / 2f) - (mainImage.Width / 2f)), 0); not all are 13 by 10
                  using (var wrapMode = new ImageAttributes())
                  {
                     wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                     graphics.DrawImage(mainImage, destRect, 0, 0, mainImage.Width, mainImage.Height, GraphicsUnit.Pixel, wrapMode);
                  }
               }
               mainImage.Dispose();
               mainImage = tempMainResize;
               ImageCodecInfo codec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(cd => cd.FormatID == ImageFormat.Jpeg.Guid);
               EncoderParameters encParam = new EncoderParameters(1);
               encParam.Param[0] = new EncoderParameter(Encoder.Quality, DAZScraperModel.fetchConfig.JpgQuality);
               mainImage.Save(Config.GetLibrarySaveDirectory() + "\\" + fileName + "-0.jpg", codec, encParam);
#endif
            }
            return; //if we just made an image we're done, else if no mini images, no main image, do nothing.
         }
         #endregion

         //put the images in the result image.
         //int rootLeftMainImage = (int)(((double)resultDimensions.width - ((double)(rowColCount * maxSubWidth) + (double)maxMainWidth)) / 2d);
         int rootLeftMainImage = 0;
         //int rootLeftSubImages = rootLeftMainImage + maxMainWidth;
         int rootLeftSubImages = resultDimensions.width - (rowColCount * maxSubWidth);
         //int rootTopMainImage = (int)((resultDimensions.height - (mainImage?.Height) ?? 0) / 2d);
         int rootTopMainImage = 0;

#if IMAGESHARP_IMAGE_PROCESSING
         List<Image<Rgb24>> resultImages = new List<Image<Rgb24>>();
#else
         List<Image> resultImages = new List<Image>();
#endif
         int numberOfImages = (int)Math.Ceiling((double)miniImages.Count / (double)maxNumberOfSmallImagesPerThumbnail);
         numberOfImages = maxThumbnails < numberOfImages ? maxThumbnails : numberOfImages;

         for (int i = 0; i < numberOfImages; i++)
         {
#if IMAGESHARP_IMAGE_PROCESSING
            Image<Rgb24> resultImage = new Image<Rgb24>(config, resultDimensions.width, resultDimensions.height, new Rgb24(0, 0, 0));
#else
            Bitmap resultImage = new Bitmap(resultDimensions.width, resultDimensions.height);
#endif
            resultImages.Add(resultImage);

            #region Draw images onto result image
#if DEBUG
            System.Diagnostics.Stopwatch std = new System.Diagnostics.Stopwatch();
            std.Start();
#endif
#if IMAGESHARP_IMAGE_PROCESSING
            //main image
            if (mainImage != null)
            {
               ResizeOptions resizeOpt = new ResizeOptions()
               {
                  Mode = ResizeMode.Max,
                  Size = new Size((rootLeftSubImages - rootLeftMainImage), resultDimensions.height),
                  Sampler = KnownResamplers.Triangle
               };
               mainImage.Mutate(mo =>
               {
                  mo.Resize(resizeOpt);
               });
            }
            resultImage.Mutate(o =>
            {
               o.DrawImage(mainImage, new Point(rootLeftMainImage, rootTopMainImage), 1f);
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
                     int iw = (int)((maxSubWidth - miniImages[si].Width) / 2d);
                     int ih = (int)((maxSubHeight - miniImages[si].Height) / 2d);
                     Point pos = new Point(rootLeftSubImages + (x * maxSubWidth) + iw, (y * maxSubHeight) + ih);
                     //o.DrawImage(miniImages[si], new Point(rootLeftMiniImages + (x * miniMaxImageWidth), y * miniMaxImageHeight), 1f);
                     o.DrawImage(miniImages[si], pos, 1f);
                  }
               }
            });
#else
            using (var graphics = Graphics.FromImage(resultImage))
            {
               graphics.CompositingMode = CompositingMode.SourceCopy;
               graphics.CompositingQuality = CompositingQuality.HighQuality;
               graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
               graphics.SmoothingMode = SmoothingMode.HighQuality;
               graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

               if (mainImage != null)
               {
                  float widthDivHeight = (float)mainImage.Width / (float)mainImage.Height;

                  int width;
                  int height;
                  int x;
                  int y;
                  if (widthDivHeight < 1f)
                  {
                     //max out height (portrait)
                     height = resultDimensions.height;
                     width = (int)((float)resultDimensions.height * widthDivHeight);
                     x = (int)(((rootLeftSubImages - rootLeftMainImage) / 2f) - (width / 2f));
                     y = 0;
                  }
                  else if (widthDivHeight > 1f)
                  {
                     //max out width (landscape)
                     height = (int)((float)(rootLeftSubImages - rootLeftMainImage) / widthDivHeight);
                     width = (rootLeftSubImages - rootLeftMainImage);
                     x = 0;
                     y = (int)((resultDimensions.height / 2f) - (height / 2f));
                  }
                  else
                  {
                     //square
                     width = (rootLeftSubImages - rootLeftMainImage);
                     height = resultDimensions.height;
                     x = y = 0; //fix this: this shouldnt be 0???? not sure
                  }

                  Rectangle destRect = new Rectangle(x - x, y, width, height); //x - x is just 0, but it's representing removing the thin black edge at the left of the image.

                  //graphics.DrawImageUnscaled(mainImage, rootLeftMainImage, rootTopMainImage); //put it into a box.

                  using (var wrapMode = new ImageAttributes())
                  {
                     wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                     graphics.DrawImage(mainImage, destRect, 0, 0, mainImage.Width, mainImage.Height, GraphicsUnit.Pixel, wrapMode);
                  }
               }
               int max = (i + 1) * maxNumberOfSmallImagesPerThumbnail;
               max = max > miniImages.Count ? miniImages.Count : max;
               for (int si = i * maxNumberOfSmallImagesPerThumbnail; si < max; si++)
               {
                  int pi = si - (i * maxNumberOfSmallImagesPerThumbnail);
                  int x = pi % rowColCount;
                  int y = (pi - x) / rowColCount;
                  if (miniImages[si] != null)
                  {
                     int iw = (int)((maxSubWidth - miniImages[si].Width) / 2d);
                     int ih = (int)((maxSubHeight - miniImages[si].Height) / 2d);
                     Point pos = new Point(rootLeftSubImages + (x * maxSubWidth) + iw, (y * maxSubHeight) + ih);
                     //int wdd = miniImages[si].Width;
                     //int htt = miniImages[si].Height;
                     graphics.DrawImageUnscaled(miniImages[si], pos);
                  }
               }
            }
#endif
#if DEBUG
            std.Stop();
            imageDrawTimes.Add(std.ElapsedMilliseconds / 1000d);
#endif
            #endregion
         }

         //DISPOSE THE IMAGES
         foreach (var i in miniImages)
         {
            i?.Dispose();
         }
         mainImage?.Dispose();

         #region save result images
         for (int i = 0; i < resultImages.Count; i++)
         {
#if DEBUG
            System.Diagnostics.Stopwatch svt = new System.Diagnostics.Stopwatch();
            svt.Start();
#endif
#if IMAGESHARP_IMAGE_PROCESSING
            using (MemoryStream ms = new MemoryStream())
            {
               resultImages[i].Save(ms, new JpegEncoder() { Quality = DazQuickviewManager.fetchConfig.JpgQuality });
               using (FileStream fs = File.Create(FetchConfig.GetLibrarySaveDirectory() + $"\\{fileName}-{i}.jpg"))
               {
                  ms.Seek(0, SeekOrigin.Begin);
                  ms.CopyTo(fs);
                  await fs.FlushAsync();
               }
            }

#else
            ImageCodecInfo codec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(x => x.FormatID == ImageFormat.Jpeg.Guid);
            EncoderParameters encParam = new EncoderParameters(1);
            encParam.Param[0] = new EncoderParameter(Encoder.Quality, DAZScraperModel.fetchConfig.JpgQuality);
            resultImages[i].Save(Config.GetLibrarySaveDirectory() + "\\" + fileName + $"-{i}.jpg", codec, encParam);
#endif
            resultImages[i].Dispose();
#if DEBUG
            svt.Stop();
            imageSaveTimes.Add(svt.ElapsedMilliseconds / 1000d);
#endif
         }
         #endregion
      }
      catch (Exception e)
      {
         Console.WriteLine("A bad thing occurred in the ImageProcessor. \n\n" + e.GetType().ToString() + " : " + e.Message + "\n\n" + e.StackTrace);
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
#if IMAGESHARP_IMAGE_PROCESSING
      Configuration config = new Configuration(new PngConfigurationModule(), new JpegConfigurationModule());
      config.MaxDegreeOfParallelism = 1;
      config.SetGraphicsOptions(x =>
      {
         x.Antialias = false;
      });
      (int width, int height) resultDimensions = DazQuickviewManager.FetchConfig.GetResolution(DazQuickviewManager.fetchConfig.Resolution);
      using (Image<Rgb24> image = Image.Load<Rgb24>(config, imageData))
      using (MemoryStream ms = new MemoryStream())
      using (FileStream fs = File.Create(FetchConfig.GetLibrarySaveDirectory() + $"\\" + fileName + "-0.jpg"))
      {
         ResizeOptions resizeOpt = new ResizeOptions()
         {
            Mode = ResizeMode.Max,
            Size = new Size(resultDimensions.width, resultDimensions.height),
            Sampler = KnownResamplers.Triangle
         };
         image.Mutate(o =>
         {
            o.Resize(resizeOpt);
            o.Pad(resultDimensions.width, resultDimensions.height, Color.Black);
         });
         image.Save(ms, new JpegEncoder() { Quality = DazQuickviewManager.fetchConfig.JpgQuality });
         {
            ms.Seek(0, SeekOrigin.Begin);
            ms.CopyTo(fs);
            await fs.FlushAsync();
         }
      }
#else
      (int width, int height) resultDimensions = DAZScraperModel.Config.GetResolution(DAZScraperModel.fetchConfig.Resolution);
      using (MemoryStream ms = new MemoryStream(imageData))
      using (Image before = Image.FromStream(ms))
      {
         int height = resultDimensions.height;
         int width = (int)Math.Floor((float)before.Width * ((float)resultDimensions.height / (float)before.Height));
         Rectangle destRect = new Rectangle((int)((resultDimensions.width / 2f) - (width / 2f)), 0, width, height);

         Bitmap after = new Bitmap(resultDimensions.width, resultDimensions.height);
         //temp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
         using (var graphics = Graphics.FromImage(after))
         {
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var wrapMode = new ImageAttributes())
            {
               wrapMode.SetWrapMode(WrapMode.TileFlipXY);
               graphics.DrawImage(before, destRect, 0, 0, before.Width, before.Height, GraphicsUnit.Pixel, wrapMode);
            }
         }

         ImageCodecInfo codec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(x => x.FormatID == ImageFormat.Jpeg.Guid);
         EncoderParameters encParam = new EncoderParameters(1);
         encParam.Param[0] = new EncoderParameter(Encoder.Quality, DAZScraperModel.fetchConfig.JpgQuality);
         after.Save(Config.GetLibrarySaveDirectory() + "\\" + fileName + "-0.jpg", codec, encParam);
      }
#endif
   }
}
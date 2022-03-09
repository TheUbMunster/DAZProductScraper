using System.Collections;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Web;
using PuppeteerSharp;
using PuppeteerSharp.Input;
using System.Threading.Tasks;
using System.Threading;
using System;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

public static class DAZScraperModel
{
   //TODO:
   /*
    * Error handling (e.g. wrong username/password, no internet connection)
    * filesystem and generation tool
    * progress bars
    * console log for user
    * DISPOSE ALL OF THE TASKS?
    * 
    * {
    *    key image,
    *    individual images,
    *    description text, 
    *    url link
    * }
    * 
    * folder with just shortcuts to the key images
    * 
    * MAYBE DONE?:
    * Request limiting so we don't ddos things 
    */

   public class Config
   {
      static Config()
      {
         //assure data folders exist.
         MakeDirs();
      }

      public enum ThumbnailResolution
      {
         R480p = 0,
         R720p,
         R1080p,
         R1440p,
         R2160p
      }

      private const string RootSaveDirectory = "DAZProductScraper_Data";
      private const string LibrarySaveSubdirectory = "Data_Library";
      private const string SortingSaveSubdirectory = "Sorted_Library";

      public ThumbnailResolution Resolution { get; set; } = ThumbnailResolution.R1080p;
      public int JpgQuality { get; set; } = 75;
      //public int ImageSaveConcurrency { get; set; } = 1;
      public int ImageFetchConcurrency { get; set; } = 10;

      #region Static Helpers
      public static (int width, int height) GetResolution(ThumbnailResolution res)
      {
         switch (res)
         {
            case ThumbnailResolution.R480p:
               return (852, 480);
            case ThumbnailResolution.R720p:
               return (1280, 720);
            case ThumbnailResolution.R1080p:
               return (1920, 1080);
            case ThumbnailResolution.R1440p:
               return (2560, 1440);
            case ThumbnailResolution.R2160p:
               return (3840, 2160);
            default:
               throw new ArgumentException($"The value {res} is not a valid resolution. Make sure to only pass values defined in the enum.");
         }
      }

      public static void MakeDirs()
      {
         Directory.CreateDirectory(GetRootFolderPath());
         Directory.CreateDirectory(GetLibrarySaveDirectory());
         Directory.CreateDirectory(GetSortingSaveDirectory());
      }

      public static void ResetDirs()
      {
         Directory.Delete(GetRootFolderPath(), true);
         MakeDirs();
      }

      public static string GetRootFolderPath()
      {
         return Path.GetFullPath(RootSaveDirectory);
      }

      public static string GetLibrarySaveDirectory()
      {
         return Path.Combine(GetRootFolderPath(), LibrarySaveSubdirectory);
      }

      public static string GetSortingSaveDirectory()
      {
         return Path.Combine(GetRootFolderPath(), SortingSaveSubdirectory);
      }

      public static IEnumerable<string> GetSortingFoldersPaths()
      {
         return Directory.EnumerateDirectories(GetSortingSaveDirectory());
      }

      public static IEnumerable<string> GetSortingFolderNames()
      {
         return Directory.EnumerateDirectories(GetSortingSaveDirectory()).Select(x => x.Substring(x.LastIndexOf('\\') + 1));
      }

      public static string GetSortingFolderPathByName(string name)
      {
         return Path.Combine(GetSortingSaveDirectory(), name);
      }

      public static string GetSortingFolderNameByPath(string path)
      {
         return path.Substring(path.LastIndexOf('\\') + 1);
      }

      public static string GetSortingFolderTextByName(string name)
      {
         return GetSortingFolderPathByName(name) + ".txt";
      }

      public static void DeleteSortingFolder(string name)
      {
         string path = GetSortingFolderPathByName(name);
         if (Directory.Exists(path))
            Directory.Delete(path, true);
         string txt = GetSortingFolderTextByName(name);
         if (File.Exists(txt))
            File.Delete(txt);
      }
      #endregion
   }

   public class ProductInfoFetch
   {
      public string productID;
      public string productName;
      public string cleanedProductName;
      public string productDescription;
      public string productPageURL;
      public string[] imageUrls;
      public byte[] base64Image;

      public ProductInfoFetch(string ID)
      {
         productID = ID;
      }

      public async Task FetchData() //sometimes throws 404 exception?
      {
         WebClient wc = null;
         try
         {
            wc = new WebClient();
            string result = await wc.DownloadStringTaskAsync(productInfoApiUrl + productID);
            JObject json = JObject.Parse(result);
            productName = WebUtility.HtmlDecode(json["name"].ToString());
            cleanedProductName = Regex.Replace(Regex.Replace(productName, " +", "_"), "[^a-zA-Z0-9_-]+", string.Empty);
            productPageURL = domain + json["url"].ToString(); //url shouldn't need to be decoded?
            base64Image = Convert.FromBase64String(json["image"].ToString());
         }
         catch
         {
            //forseen errors:
            //wc.DownloadStringTaskAsync was passed null
            //wc.DownloadStringTaskAsync was passed a bad url
            //wc.DownloadStringTaskAsync had an error occur while downloading
            //JObject.Parse had a bad parse.
            //json["name"] or json["url"] were bad.
            //completionCallback?.Invoke(this);
         }
         finally
         {
            wc?.Dispose();
            wc = null;
         }
         if (productPageURL == null) //needed for next part.
         {
            return;
         }
         try
         {
            wc = new WebClient();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(await wc.DownloadStringTaskAsync(productPageURL));
            imageUrls = doc.DocumentNode.SelectNodes("//img[@class=\"zoomable\"]").Select(x => x.GetAttributeValue("data-fancybox-href", null))?.ToArray();
            productDescription = doc.DocumentNode.SelectSingleNode(@"//div[contains(concat(' ', normalize-space(@class), ' '), ' box-collateral ') and 
contains(concat(' ', normalize-space(@class), ' '), ' box-additional ')]")?.InnerText?.Trim() ?? string.Empty;
            productDescription = productDescription?.Substring(0, productDescription.LastIndexOf("\nNotes\n") + 1) ?? string.Empty;
            //if the above substring doesn't work, ignore the child of the above select single node with classes "data data_for_notes"

            productDescription = WebUtility.HtmlDecode(productDescription);
            productDescription = productName + "\n" + productPageURL + "\n\n" + productDescription;
            if (imageUrls != null)
            {
               base64Image = null; //we don't need this anymore. (however, maybe use this as a fallback?)
            }
         }
         catch
         {
            //probably a 404 because this particular product is one of daz's "daz studio win64 installer" product or something.
            //no images because this product doesn't have a regular product page.
            //completionCallback?.Invoke(this);
         }
         finally
         {
            wc?.Dispose();
            wc = null;
         }
         //completionCallback?.Invoke(this);
      }
   }

   //enum State
   //{
   //   Undefined = 0,
   //   LoadingBrowser,
   //   BrowserLoaded,
   //   LoadingLogin,
   //   LoginLoaded,
   //   LoggingIn,
   //   LoggedIn,
   //   LoadingProductPage,
   //   ProductPageLoaded,
   //   FetchingImages,
   //   ImagesFetched
   //}

   const string domain = "https://www.daz3d.com";
   const string productInfoApiUrl = domain + "/dazApi/slab/";
   const string userLoginUrl = domain + "/customer/account/login";
   const string userProductPageUrl = domain + "/downloader/customer/files";

   //public const int maxConcurrencyIO = 10;

   public static Config fetchConfig = new Config() { Resolution = Config.ThumbnailResolution.R2160p, JpgQuality = 80 };

   //private static State currentState;

   private static Browser browser;
   private static Page webpage;
   public static CancellationTokenSource programwideCancellation = new CancellationTokenSource();
   public static event Action<string, int, bool> LogInfo; //info, color, clearPrevious.

   public static string email, pass;

   public static void Initialize(Browser browser)
   {
      DAZScraperModel.browser = browser;
   }

   //public static async Task InitBrowser()
   //{
   //   await InitializeBrowser();
   //}

   public static async Task GoToLogin(bool newPage = true)
   {
      await NavigateToLogin(newPage);
   }

   public static async Task<HttpStatusCode> TryLogin(string email, string pass)
   {
      return await Login(email, pass);
   }

   public static async Task NavigateToProductsPage()
   {
      await GoToProductsPage();
   }

   public static async Task<List<string>> FetchIds()
   {
      return await GetProductsIds();
   }

   public static async Task Generate(List<string> ids, bool overwriteExisting = true)
   {
      await GenerateData(ids, overwriteExisting);
   }

   /// <summary>
   /// Starts up a browser instance and assigns the browser field to the new browser.
   /// </summary>
   /// <param name="completionCallback">Called when the browser has been started</param>
//   private static async Task InitializeBrowser()
//   {
//      BrowserFetcher browserFetch = Puppeteer.CreateBrowserFetcher(new BrowserFetcherOptions() { });
//      RevisionInfo revInfo = await browserFetch.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
//      browser = await Puppeteer.LaunchAsync(new LaunchOptions
//      {
//         Headless =
//#if DEBUG
//         false
//#else
//         true
//#endif
//         ,
//         ExecutablePath = revInfo.ExecutablePath,
//         Timeout = 5000,
//      });
//      LogInfo?.Invoke("Launched browser", 5, false);
//   }

   /// <summary>
   /// Adds a new page to the browser and sends it to the daz login page.
   /// </summary>
   /// <param name="completionCallback">Called when the browser is at the login page</param>
   private static async Task NavigateToLogin(bool newPage = true)
   {
      if (newPage)
      {
         webpage = await browser.NewPageAsync();
      }
#if DEBUG
      await webpage.SetViewportAsync(new ViewPortOptions() { Width = 1920, Height = 1080 });
#endif
      LogInfo?.Invoke("Going to login page", 5, false);
      await webpage.GoToAsync(userLoginUrl);
      LogInfo?.Invoke("At login page", 5, false);
   }

   /// <summary>
   /// Logs the user into the webpage.
   /// </summary>
   /// <param name="email">The email to submit</param>
   /// <param name="password">The password to submit</param>
   /// <param name="completionCallback">Called when the login is submitted</param>
   private static async Task<HttpStatusCode> Login(string email, string password)
   {
      ElementHandle form = await webpage.WaitForSelectorAsync("#login-form");
      string loginJs = @"function puppeteerLogin(form)
{
	var email = form.children[1].children[0].children[1];
	var pass = form.children[1].children[1].children[1];
	var button = form.children[1].children[3].children[0];
	email.value = """ + email + @""";
	pass.value = """ + password + @""";
}";
      await form.EvaluateFunctionAsync(loginJs);
      ElementHandle sendButton = await webpage.WaitForSelectorAsync("#send2");
      await sendButton.EvaluateFunctionAsync("(x) => x.click()");
      LogInfo?.Invoke("Attempting login", 5, false);
      Response resp = await webpage.WaitForNavigationAsync(new NavigationOptions() { Timeout = 5000 });
      //Response resp = await webpage.GoToAsync("https://www.daz3d.com/customer/account/loginPost/"); //internalservererror even if you login correctly
      //Console.WriteLine("OK?: " + redirResponse.Ok);
      return resp?.Status ?? ((HttpStatusCode)(-1)); //TODO: return int code for timeout, failed but not timeout (bad credentials presumably), and success.
   }

   /// <summary>
   /// Goes to the user's product page (after having logged in).
   /// </summary>
   /// <param name="completionCallback">Called when the webpage is directed to the product page</param>
   private static async Task GoToProductsPage()
   {
      LogInfo?.Invoke("Going to product page", 5, true);
      await webpage.GoToAsync(userProductPageUrl);
      LogInfo?.Invoke("At product page", 5, false);
   }

   /// <summary>
   /// Gets all of the product ids for this user and saves them in productIds
   /// </summary>
   /// <param name="completionCallback">Called when the ids are loaded</param>
   private static async Task<List<string>> GetProductsIds()
   {
      LogInfo?.Invoke("Getting product ids", 5, false);
      ElementHandle productList = await webpage.WaitForSelectorAsync("#product_list");
      string getProductIds = @"function getProductIds(root)
{
   let result = [];
   for (let i = 0; i < _products.items.length; i++)
   {
      result[i] = _products.items[i].product_id;
   }
   return result;
}";
      return new List<string>((await productList.EvaluateFunctionAsync(getProductIds)).Select(x =>
      {
         string s = x.ToString();
         int i = s.IndexOf('_');
         //some interactive license products have an id xxxxx_xxxxx where the former is the real id.
         return s.Substring(0, i == -1 ? s.Length : i);
      }).Distinct());
   }

   /// <summary>
   /// Generates the images and such based off the provided product ids.
   /// </summary>
   /// <param name="ids">The product ids to generate the data for</param>
   /// <param name="completionCallback">Called when all the data has been generated.</param>
   private static async Task GenerateData(List<string> ids, bool overwriteExisting = true)
   {
      LogInfo?.Invoke("Generating/fetching description and image data", 1, true);
      //to make this faster, sort it in a way and write a comparison for a binary search (on pid)
      List<string> existingFiles = new List<string>(Directory.EnumerateFiles(Config.GetLibrarySaveDirectory()));
      //ids = new List<string>
      //{
      //   //"81647",
      //   //"16280",
      //   //"80823",
      //   //"46841",
      //   //"82636",
      //   //"82662",
      //   //"56001",
      //};
      using (SemaphoreSlim semaphore = new SemaphoreSlim(fetchConfig.ImageFetchConcurrency, fetchConfig.ImageFetchConcurrency)) //switch this over to config.
      {
         List<Task> fetches = new List<Task>();
         const int amountPerSlam = 200;
         LogInfo?.Invoke($"Total number of products: {ids.Count}", 5, false);
         //change this so that amountPerSlam only cares about products that were actually fetched (only applies when updating db, not overwriting).
         for (int i = 0; i < ids.Count; i += amountPerSlam)
         {
            if (i != 0)
               LogInfo?.Invoke("Done taking a break, resuming now.", 5, false);
            for (int j = i; j < (i + amountPerSlam < ids.Count ? i + amountPerSlam : ids.Count); j++)
            {
               await semaphore.WaitAsync(); //10 at a time
               string id = ids[j]; //closeure not capturing the right index? (don't put this inside the startnew task it's bad).
               fetches.Add(Task.Run(async () =>
               {
                  IEnumerable<string> imageFileMatches = existingFiles.Where(x => Regex.IsMatch(x, $"_{id}-\\d+.jpg")); //this is slow (binary search w/ regex somehow?)
                  if (!imageFileMatches.Any() || overwriteExisting) //no images or overwrite
                  {
                     ProductInfoFetch fetch = new ProductInfoFetch(id);
                     await fetch.FetchData();
                     LogInfo?.Invoke("Starting one image...", 5, false);
                     if (overwriteExisting)
                     {
                        //we're overwriting (regardless if the files exist or not)
                        LogInfo?.Invoke($"going to write/overwrite for product {fetch.productID}", 5, false);
                     }
                     else
                     {
                        //there must have been no files
                        LogInfo?.Invoke($"going to write product {fetch.productID}", 5, false);
                     }
                     //delete all files currently there
                     foreach (string file in imageFileMatches)
                     {
                        File.Delete(file);
                     }
                     if (fetch.imageUrls != null)
                     {
                        //ssr = new Screenshotter.ScreenshotRequest(fetch.cleanedProductName, fetch.imageUrls);
                        await ImageProcessor.GenerateImage(fetch.imageUrls, fetch.cleanedProductName + $"_{fetch.productID}", 9, 100, 3);
                     }
                     else if (fetch.base64Image != null)
                     {
                        //ssr = new Screenshotter.ScreenshotRequest(fetch.cleanedProductName, fetch.base64Image);
                        await ImageProcessor.GenerateImage(fetch.base64Image, fetch.cleanedProductName + $"_{fetch.productID}");
                     }
                     else
                     {
                        LogInfo?.Invoke($"Couldn't find any image data for {fetch.productID + " : " + (string.IsNullOrWhiteSpace(fetch.cleanedProductName) ? "no product name found" : fetch.cleanedProductName)}", 5, false);
                     }
                     IEnumerable<string> textFileMatches = existingFiles.Where(x => Regex.IsMatch(x, $"_{id}.txt"));
                     if (textFileMatches.Count() > 1)
                     {
                        LogInfo?.Invoke($"Found more than one text file for product {id}", 5, false);
                     }
                     if (!textFileMatches.Any() || overwriteExisting)
                     {
                        foreach (string file in textFileMatches)
                        {
                           File.Delete(file);
                        }
                     }
                     File.WriteAllText(Config.GetLibrarySaveDirectory() + "\\" + fetch.cleanedProductName + $"_{id}.txt", fetch.productDescription);
                  }
                  //ssr.Calculate().Wait(); //1 at a time of the 10
                  //Screenshotter.AddScreenshotRequest(ssr);
                  semaphore.Release();
               }, programwideCancellation.Token));
            }
            if (i + amountPerSlam >= ids.Count)
            {
               break; //if the next i loop iteration would put us past the end then don't bother waiting 2 more minutes.
            }
            LogInfo?.Invoke("Giving the server a break... (We don't want to DDOS here)", 5, false);
            await Task.Delay(1000 * 90);
         }
         await Task.WhenAll(fetches);
         fetches.ForEach(x => x.Dispose());
      }
   }

   public static string AttemptCreateSortingFolder(string name, string @params, bool overwriteIfExists = false)
   {
      if (string.IsNullOrWhiteSpace(@params))
      {
         return "You must have at least one parameter";
      }
      if (Directory.Exists(Config.GetSortingFolderPathByName(name)) && !overwriteIfExists)
      {
         //deny, that name already exists.
         return "A sorting keyword folder already exists with that name";
      }
      Regex validityRegex = new Regex("-\"(.+?)(?<!\\\\)\"");
      IEnumerable<string> args = validityRegex.Matches(@params).Cast<Match>().Select(x => x.Value.Trim());
      bool valid = true;
      foreach (string s in args)
      {
         valid &= validityRegex.IsMatch(s);
      }
      if (!valid)
      {
         //reject because the string isn't right
         return "The provided parameters were not parseable";
      }
      StringBuilder pattern = new StringBuilder();
      foreach (string s in args)
      {
         pattern.Append($"({validityRegex.Match(s).Groups[1].Value})|");
      }
      pattern.Remove(pattern.Length - 1, 1); //remove last pipe
      Regex reg;
      try
      {
         reg = new Regex(pattern.ToString());
      }
      catch (ArgumentException e)
      {
         //reject because full regex is broken (this means some part of the regex isn't valid)
         //keep track of the regexes and what line they're on and report what line wasn't parseable
         return "The generated regular expression was not valid";
      }
      //foreach text file that matches the regex, add a shortcut to that pid's images
      HashSet<string> pidsToAdd = new HashSet<string>();
      Regex getPidFromFilename = new Regex(".+_(\\d+)\\.txt");
      foreach (string f in Directory.EnumerateFiles(Config.GetLibrarySaveDirectory(), "*.txt"))
      {
         if (reg.IsMatch(System.IO.File.ReadAllText(f)))
         {
            pidsToAdd.Add(getPidFromFilename.Match(f).Groups[1].Value);
         }
      }
      if (Directory.Exists(Config.GetSortingFolderPathByName(name)))
         Directory.Delete(Config.GetSortingFolderPathByName(name), true);
      Directory.CreateDirectory(Config.GetSortingFolderPathByName(name));
      Regex getImageFilename = new Regex(".+\\\\(.+_)(\\d+)(-\\d+)\\.jpg");
      foreach (string f in Directory.EnumerateFiles(Config.GetLibrarySaveDirectory(), "*.jpg")) //"*_[pid]-*.jpg"
      {
         Match m = getImageFilename.Match(f);
         if (pidsToAdd.Contains(m.Groups[2].Value))
         {
            //shortcut this image.
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut($"{Config.GetSortingFolderPathByName(name)}\\{m.Groups[1].Value + m.Groups[2].Value + m.Groups[3].Value}.lnk");

            shortcut.TargetPath = f;
            shortcut.Save();
         }
      }
      File.WriteAllText($"{Config.GetSortingFolderPathByName(name)}.txt", @params);
      return null;
   }

   public static void OnApplicationQuit()
   {
      webpage?.Dispose();
      browser?.Dispose();
      try
      {
         programwideCancellation.Cancel();
         programwideCancellation.Dispose();
      }
      catch
      {
         //guess we already cancelled.
      }
   }
}
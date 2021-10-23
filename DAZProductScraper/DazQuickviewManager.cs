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

public static class DazQuickviewManager
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

   public class FetchConfig
   {
      public enum ThumbnailResolution
      {
         R480p,
         R720p,
         R1080p,
         R1440p,
         R2160p
      }

      public ThumbnailResolution Resolution { get; set; } = ThumbnailResolution.R1080p;
      public int JpgQuality { get; set; } = 75;
      public string SaveDirectory { get; set; } = "DAZProductScraper_Data";

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

      public async Task FetchData()
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
contains(concat(' ', normalize-space(@class), ' '), ' box-additional ')]")?.InnerText?.Trim();
            //ignore the child of the above select single node with classes "data data_for_notes"

            productDescription = WebUtility.HtmlDecode(productDescription);
            if (imageUrls != null)
            {
               base64Image = null; //we don't need this anymore.
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

   enum State
   {
      Undefined = 0,
      LoadingBrowser,
      BrowserLoaded,
      LoadingLogin,
      LoginLoaded,
      LoggingIn,
      LoggedIn,
      LoadingProductPage,
      ProductPageLoaded,
      FetchingImages,
      ImagesFetched
   }

   const string domain = "https://www.daz3d.com";
   const string productInfoApiUrl = domain + "/dazApi/slab/";
   const string userLoginUrl = domain + "/customer/account/login";
   const string userProductPageUrl = domain + "/downloader/customer/files";

   public const int maxConcurrencyIO = 10;

   public static FetchConfig fetchConfig = new FetchConfig() { Resolution = FetchConfig.ThumbnailResolution.R2160p, JpgQuality = 80 };

   private static State currentState;

   private static Browser browser;
   private static Page webpage;
   public static CancellationTokenSource programwideCancellation = new CancellationTokenSource();

   public static string email, pass;

   public static void Start()
   {
      //Test();
      System.IO.Directory.CreateDirectory(fetchConfig.SaveDirectory);
      ChangeState(State.LoadingBrowser);
   }

   public static async void Test()
   {
      System.IO.Directory.CreateDirectory(fetchConfig.SaveDirectory);
      ProductInfoFetch pif = new ProductInfoFetch("49035");
      await pif.FetchData();
      await ImageProcessor.GenerateImage(pif.imageUrls, pif.cleanedProductName, 2, 32, 100);
   }

   public static void TryLogin()
   {
      if (currentState == State.LoginLoaded)
      {
         ChangeState(State.LoggingIn);
      }
   }

   private static void ChangeState(State newState)
   {
      currentState = newState;
      switch (newState)
      {
         case State.Undefined:
            throw new InvalidOperationException();
         case State.LoadingBrowser:
            InitializeBrowser(() => ChangeState(State.BrowserLoaded));
            break;
         case State.BrowserLoaded:
            ChangeState(State.LoadingLogin);
            break;
         case State.LoadingLogin:
            NavigateToLogin(() => ChangeState(State.LoginLoaded));
            break;
         case State.LoginLoaded:
            //waiting
            break;
         case State.LoggingIn:
            Login(email, pass, () => ChangeState(State.LoggedIn));
            break;
         case State.LoggedIn:
            ChangeState(State.LoadingProductPage);
            break;
         case State.LoadingProductPage:
            GoToProductsPage(() => ChangeState(State.ProductPageLoaded));
            break;
         case State.ProductPageLoaded:
            GetProductsIds((List<string> l) =>
            {
               Console.WriteLine("Product Id's fetched");
               GenerateData(l, () => Console.WriteLine("Done!!!"));
            });
            break;
         default:
            throw new InvalidOperationException();
      }
   }

   /// <summary>
   /// Starts up a browser instance and assigns the browser field to the new browser.
   /// </summary>
   /// <param name="completionCallback">Called when the browser has been started</param>
   private static async void InitializeBrowser(Action completionCallback)
   {
      BrowserFetcher browserFetch = Puppeteer.CreateBrowserFetcher(new BrowserFetcherOptions() { });
      RevisionInfo revInfo = await browserFetch.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
      Browser b = await Puppeteer.LaunchAsync(new LaunchOptions
      {
         Headless =
#if DEBUG
         true//false
#else
         true
#endif
         ,
         ExecutablePath = revInfo.ExecutablePath
      });
      browser = b;
      completionCallback?.Invoke();
   }

   /// <summary>
   /// Adds a new page to the browser and sends it to the daz login page.
   /// </summary>
   /// <param name="completionCallback">Called when the browser is at the login page</param>
   private static async void NavigateToLogin(Action completionCallback)
   {
      webpage = await browser.NewPageAsync();
#if DEBUG
      await webpage.SetViewportAsync(new ViewPortOptions() { Width = 1920, Height = 1080 });
#endif
      await webpage.GoToAsync(userLoginUrl);
      completionCallback?.Invoke();
   }

   /// <summary>
   /// Logs the user into the webpage.
   /// </summary>
   /// <param name="email">The email to submit</param>
   /// <param name="password">The password to submit</param>
   /// <param name="completionCallback">Called when the login is submitted</param>
   private static async void Login(string email, string password, Action completionCallback)
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
      await webpage.WaitForNavigationAsync();
      completionCallback?.Invoke();
   }

   /// <summary>
   /// Goes to the user's product page (after having logged in).
   /// </summary>
   /// <param name="completionCallback">Called when the webpage is directed to the product page</param>
   private static async void GoToProductsPage(Action completionCallback)
   {
      await webpage.GoToAsync(userProductPageUrl);
      completionCallback?.Invoke();
   }

   /// <summary>
   /// Gets all of the product ids for this user and saves them in productIds
   /// </summary>
   /// <param name="completionCallback">Called when the ids are loaded</param>
   private static async void GetProductsIds(Action<List<string>> completionCallback)
   {
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
      completionCallback?.Invoke(new List<string>((await productList.EvaluateFunctionAsync(getProductIds)).Select(x =>
      {
         string s = x.ToString();
         int i = s.IndexOf('_');
         //some interactive license products have an id xxxxx_xxxxx where the former is the real id.
         return s.Substring(0, i == -1 ? s.Length : i);
      }).Distinct()));
   }

   private static async void GenerateData(List<string> ids, Action completionCallback)
   {
      System.IO.Directory.CreateDirectory(fetchConfig.SaveDirectory);
      using (SemaphoreSlim semaphore = new SemaphoreSlim(maxConcurrencyIO, maxConcurrencyIO))
      {
         List<Task> fetches = new List<Task>();
         const int amountPerSlam = 200;
         Console.WriteLine("Total number of products: " + ids.Count);
         for (int i = 0; i < ids.Count; i += amountPerSlam)
         {
            for (int j = i; j < (i + amountPerSlam < ids.Count ? i + amountPerSlam : ids.Count); j++)
            {
               await semaphore.WaitAsync(); //10 at a time
               string id = ids[j]; //closeure not capturing the right index? (don't put this inside the startnew task it's bad).
               fetches.Add(Task.Factory.StartNew(async () =>
               {
                  ProductInfoFetch fetch = new ProductInfoFetch(id);
                  await fetch.FetchData();
                  //Screenshotter.ScreenshotRequest ssr;
                  Console.WriteLine("Starting one image...");
                  if (fetch.imageUrls != null)
                  {
                     //ssr = new Screenshotter.ScreenshotRequest(fetch.cleanedProductName, fetch.imageUrls);
                     await ImageProcessor.GenerateImage(fetch.imageUrls, fetch.cleanedProductName, 9, 100, 3);
                  }
                  else if (fetch.base64Image != null)
                  {
                     //ssr = new Screenshotter.ScreenshotRequest(fetch.cleanedProductName, fetch.base64Image);
                     await ImageProcessor.GenerateImage(fetch.base64Image, fetch.cleanedProductName);
                  }
                  else
                  {
                     Console.WriteLine($"Couldn't find any image data for {fetch.productID + " : " + fetch.cleanedProductName}");
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
            Console.WriteLine("Giving the server a break..."); // this might be an issue for when we have less than 2000 products anyways
            await Task.Delay(1000 * 90);
         }
         await Task.WhenAll(fetches);
         fetches.ForEach(x => x.Dispose());
      }
      completionCallback?.Invoke();
   }

   public static void OnApplicationQuit()
   {
      webpage?.Dispose();
      browser?.Dispose();
      programwideCancellation.Cancel();
      programwideCancellation.Dispose();
   }
}
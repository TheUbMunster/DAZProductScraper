using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace DAZProductScraper
{
   static class BrowserService
   {
      private static Browser browser = null;
      private static bool disposed = true;

      public static async Task<Browser> GetBrowser()
      {
         if (disposed)
         {
            await StartBrowser();
         }
         return browser;
      }

      private static async Task StartBrowser()
      {
         BrowserFetcher browserFetch = Puppeteer.CreateBrowserFetcher(new BrowserFetcherOptions() { });
         RevisionInfo revInfo = await browserFetch.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
         browser = await Puppeteer.LaunchAsync(new LaunchOptions
         {
            Headless =
#if DEBUG
            false
#else
            true
#endif
         ,
            ExecutablePath = revInfo.ExecutablePath,
            Timeout = 5000,
         });
         disposed = false;
         //LogInfo?.Invoke("Launched browser", 5, false);
      }

      public static void Dispose()
      {
         browser?.Dispose();
         disposed = true;
      }
   }
}
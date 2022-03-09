using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAZProductScraper
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         var browser = BrowserService.GetBrowser();
         Job job = new Job();
         job.AddProcess(System.Diagnostics.Process.GetCurrentProcess().Id);
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         browser.Wait();
         job.AddProcess(browser.Result.Process.Id);
         //Ctor of DAZScraperGUI should take the browser created here.
         try
         {
            DAZScraperModel.Initialize(browser.Result);
            Application.Run(new DAZScraperGUI()); //TRY CATCH FINALLY CLOSE THE BROWSER HERE????
         }
         catch (Exception e)
         {

         }
         finally
         {
            BrowserService.Dispose();
         }
      }
   }
}

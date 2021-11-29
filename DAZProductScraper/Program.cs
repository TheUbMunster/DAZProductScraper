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
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         //Ctor of DAZScraperGUI should take the browser created here.
         Application.Run(new DAZScraperGUI()); //TRY CATCH FINALLY CLOSE THE BROWSER HERE
      }
   }
}

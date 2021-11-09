using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace DAZProductScraper
{
   public partial class DAZScraperGUI : Form
   {
      /*
       * Add new products from Daz
       * Add sorting keyword folder
       * "Resort Library"
       * Open Root library in file explorer
       */
      public DAZScraperGUI()
      {
         InitializeComponent();
         Application.ApplicationExit += Application_ApplicationExit;
         DazQuickviewManager.Start();
      }

      private void DAZScraperGUI_Shown(object sender, EventArgs e)
      {
         Invoke(new Action(() =>
         {
            var pp = new LoginPopup();
            pp.ShowDialog(this); //pp.Show(this);
            pp.Focus();
         }));
      }

      private void Application_ApplicationExit(object sender, EventArgs e)
      {
         DazQuickviewManager.OnApplicationQuit();
      }

      private void openRootFolderButton_Click(object sender, EventArgs e)
      {
         string p = DazQuickviewManager.FetchConfig.GetRootFilePath();
         if (Directory.Exists(p))
         {
            ProcessStartInfo si = new ProcessStartInfo
            {
               Arguments = p,
               FileName = "explorer.exe"
            };
            Process.Start(si);
         }
         else
         {

         }
      }
   }
}

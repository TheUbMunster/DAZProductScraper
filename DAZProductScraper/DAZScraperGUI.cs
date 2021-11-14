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
      //settings
      /*
       * Delete a sorting key word folder
       * Delete all sorting key word folders
       * Remove images from sorting folders that don't meet word criteria
       * Delete everything
       * Delete and Rebuild image/description base
       */
      public DAZScraperGUI()
      {
         InitializeComponent();
         Application.ApplicationExit += Application_ApplicationExit;
         //FormClosed += (a, b) => Application_ApplicationExit(a, b);
      }

      private void DAZScraperGUI_Shown(object sender, EventArgs e)
      {
         Invoke(new Action(() =>
         {
            var pp = new LoginPopup();
            pp.onClickLogin += OnLoginSubmit;
            Task.Run(async () =>
            {
               await DazQuickviewManager.InitToLogin();
               pp.Invoke(new Action(() => pp.SetLoginButtonState(true)));
               pp.Invoke(new Action(() => pp.PrintInfoToConsoleBox("Browser loaded.", LoginPopup.ConsoleBoxColor.Green, true)));
            });
            pp.PrintInfoToConsoleBox("Loading the browser...", LoginPopup.ConsoleBoxColor.Grey, true);
            pp.ShowDialog(this); //pp.Show(this);
         }));
      }

      private void OnLoginSubmit(LoginPopup sender, string email, string pass)
      {
         Task.Run(async () =>
         {
            bool success = await DazQuickviewManager.TryLogin(email, pass);
            if (success) /*valid credentials*/
            {
               sender.onClickLogin -= OnLoginSubmit;
               Invoke(new Action(() =>
               {
                  sender.Close();
                  OnLoginSuccess();
               }));
            }
            else
            {
               sender.Invoke(new Action(() =>
               {
                  sender.PrintInfoToConsoleBox("Login failed after 5 seconds.", LoginPopup.ConsoleBoxColor.Red, true);
                  sender.SetLoginButtonState(true);
               }));
            }
         });
      }

      private void OnLoginSuccess()
      {

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

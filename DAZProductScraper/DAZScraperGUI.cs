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
         Directory.CreateDirectory(DAZScraperModel.FetchConfig.GetLibrarySaveDirectory());
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
               await DAZScraperModel.InitBrowser();
               await DAZScraperModel.GoToLogin();
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
            System.Net.HttpStatusCode code = await DAZScraperModel.TryLogin(email, pass);
            {
               LoginPopup.ConsoleBoxColor col;
               string codeType;
               if ((int)code >= 200 && (int)code <= 299)
               {
                  codeType = "Success";
                  col = LoginPopup.ConsoleBoxColor.Green;
               }
               else if ((int)code >= 300 && (int)code <= 399)
               {
                  codeType = "Redirect";
                  col = LoginPopup.ConsoleBoxColor.Yellow;
               }
               else if ((int)code >= 400 && (int)code <= 499)
               {
                  codeType = "Client error";
                  col = LoginPopup.ConsoleBoxColor.Red;
               }
               else if ((int)code >= 500 && (int)code <= 599)
               {
                  codeType = "Server error";
                  col = LoginPopup.ConsoleBoxColor.Red;
               }
               else
               {
                  codeType = "Unclassified code";
                  col = LoginPopup.ConsoleBoxColor.Grey;
               }
               sender.Invoke(new Action(() => sender.PrintInfoToConsoleBox($"HTTP code ({codeType}): {(int)code}", col, true)));
            }
            {
               bool success;
               string logMessage;
               switch (code)
               {
                  default:
                  case (System.Net.HttpStatusCode)(-1):
                     //null response.
                     throw new NotImplementedException();
                     break;
                  case System.Net.HttpStatusCode.BadRequest:
                  case System.Net.HttpStatusCode.InternalServerError:
                     //failure
                     logMessage = "Bad credentials, try again.";
                     success = false;
                     break;
                  case System.Net.HttpStatusCode.OK:
                     //success.
                     logMessage = "Login successful!";
                     success = true;
                     break;
               }

               if (success)
               {
                  sender.onClickLogin -= OnLoginSubmit;
                  Invoke(new Action(() =>
                  {
                     sender.Invoke(new Action(() => sender.PrintInfoToConsoleBox(logMessage, LoginPopup.ConsoleBoxColor.Green, false)));
                     sender.Close();
                     OnLoginSuccess();
                  }));
               }
               else
               {
                  sender.Invoke(new Action(() =>
                  {
                     sender.PrintInfoToConsoleBox(logMessage, LoginPopup.ConsoleBoxColor.Red, false);
                  }));
                  await DAZScraperModel.GoToLogin(false);
                  sender.Invoke(new Action(() =>
                  {
                     sender.SetLoginButtonState(true);
                  }));
               }
            }
         });
      }

      private async void OnLoginSuccess()
      {
         await DAZScraperModel.NavigateToProductsPage();
         await DAZScraperModel.Generate(await DAZScraperModel.FetchIds());
      }

      private void Application_ApplicationExit(object sender, EventArgs e)
      {
         DAZScraperModel.OnApplicationQuit();
      }

      private void openRootFolderButton_Click(object sender, EventArgs e)
      {
         string p = DAZScraperModel.FetchConfig.GetLibrarySaveDirectory();
         Directory.CreateDirectory(p);
         ProcessStartInfo si = new ProcessStartInfo
         {
            Arguments = p,
            FileName = "explorer.exe"
         };
         Process.Start(si);
      }
   }
}

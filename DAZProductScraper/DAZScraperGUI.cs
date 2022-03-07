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
      #region Typedefs
      public enum ConsoleBoxColor
      {
         Green = 1,
         Yellow,
         Red,
         Grey,
         Black
      }
      #endregion

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
            pp.PrintInfoToConsoleBox("Loading the browser...", LoginPopup.ConsoleBoxColor.Grey, true);
            Task.Run(async () =>
            {
               await DAZScraperModel.InitBrowser();
               await DAZScraperModel.GoToLogin();
               pp.Invoke(new Action(() => pp.SetLoginButtonState(true)));
               pp.Invoke(new Action(() => pp.PrintInfoToConsoleBox("Browser loaded.", LoginPopup.ConsoleBoxColor.Green, true)));
            });
            pp.ShowDialog(this); //pp.Show(this); //this has to come last because it freezes the parent control.
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

      StringBuilder debugSB = new StringBuilder();
      private bool firstPrintOccurred;
      private void PrintInfoToConsoleBox(string info, ConsoleBoxColor color, bool clearPrior = false)
      {
         Color c;
         switch (color)
         {
            case ConsoleBoxColor.Green:
               c = Color.Green;
               break;
            case ConsoleBoxColor.Yellow:
               c = Color.Yellow;
               break;
            case ConsoleBoxColor.Red:
               c = Color.Red;
               break;
            case ConsoleBoxColor.Grey:
               c = Color.Gray;
               break;
            default:
            case ConsoleBoxColor.Black:
               c = Color.Black;
               break;
         }
         //conditional call instanceOfSomething.?[boolean condition expression, default value if function not called]MemberFunction();
         if (clearPrior)
         {
            debugLogRTB.Clear();
         }
         else
         {
            firstPrintOccurred = true;
         }
         debugLogRTB.SelectionColor = c;
         string temp = (firstPrintOccurred ? "\n" : "") + info;
         debugLogRTB.AppendText(temp);
         debugSB.Append(temp);
         Console.Write(temp);
      }

      private async void OnLoginSuccess()
      {
         await DAZScraperModel.NavigateToProductsPage();
         //await DAZScraperModel.Generate(await DAZScraperModel.FetchIds());
      }

      private void Application_ApplicationExit(object sender, EventArgs e)
      {
         //dump log
         DateTime now = DateTime.UtcNow;
         File.WriteAllText($"debug_log_{now.ToShortDateString()}_{now.ToShortTimeString()}", debugSB.ToString());
         //close model
         DAZScraperModel.OnApplicationQuit();
      }

      private void LockManipulators()
      {
         rebuildDatabaseButton.Enabled = false;
         updateDatabseButton.Enabled = false;
         clearDatabaseButton.Enabled = false;
         createKeywordFolderButton.Enabled = false;
         editKeywordFolderButton.Enabled = false;
         refreshKeywordFoldersButton.Enabled = false;
         deleteKeywordFolderButton.Enabled = false;
      }

      private void UnlockManipulators()
      {
         rebuildDatabaseButton.Enabled = true;
         updateDatabseButton.Enabled = true;
         clearDatabaseButton.Enabled = true;
         createKeywordFolderButton.Enabled = true;
         editKeywordFolderButton.Enabled = true;
         //refreshKeywordFoldersButton.Enabled = true; //not yet implemented
         deleteKeywordFolderButton.Enabled = true;
      }

      private void openRootFolderButton_Click(object sender, EventArgs e)
      {
         ProcessStartInfo si = new ProcessStartInfo
         {
            Arguments = DAZScraperModel.Config.GetLibrarySaveDirectory(),
            FileName = "explorer.exe"
         };
         Process.Start(si);
      }

      private void rebuildDatabaseButton_Click(object sender, EventArgs e)
      {
         RebuildDatabase();
      }

      private async void RebuildDatabase()
      {
         LockManipulators();
         DAZScraperModel.Config.ResetDirs();
         await DAZScraperModel.Generate(await DAZScraperModel.FetchIds(), true);
         UnlockManipulators();
      }

      private void updateDatabseButton_Click(object sender, EventArgs e)
      {
         UpdateDatabase();
      }

      private async void UpdateDatabase()
      {
         LockManipulators();
         await DAZScraperModel.Generate(await DAZScraperModel.FetchIds(), false);
         UnlockManipulators();
      }

      private void clearDatabaseButton_Click(object sender, EventArgs e)
      {
         LockManipulators();
         if (MessageBox.Show(this, $"Are you SURE you want to delete the ENTIRE local database? (Images, description files, sorting folders etc.)",
               "Delete everything?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
         {
            DAZScraperModel.Config.ResetDirs();
         }
         UnlockManipulators();
      }

      private void openKeywordFolderButton_Click(object sender, EventArgs e)
      {
         LockManipulators();
         var pp = new OpenFolderPopup(DAZScraperModel.Config.GetSortingSaveDirectory());
         pp.ShowDialog();
         if (Directory.Exists(pp.OpenPath))
         {
            ProcessStartInfo si = new ProcessStartInfo
            {
               Arguments = pp.OpenPath,
               FileName = "explorer.exe"
            };
            Process.Start(si);
            pp.Dispose();
         }
         UnlockManipulators();
      }

      private void createKeywordFolderButton_Click(object sender, EventArgs e)
      {
         LockManipulators();
         var pp = new CreateFolderPopup(DAZScraperModel.Config.GetLibrarySaveDirectory(), DAZScraperModel.Config.GetSortingSaveDirectory());
         pp.ShowDialog();
         pp.Dispose();
         UnlockManipulators();
      }

      private void editKeywordFolderButton_Click(object sender, EventArgs e)
      {
         LockManipulators();
         var op = new OpenFolderPopup(DAZScraperModel.Config.GetSortingSaveDirectory(), OpenFolderPopup.OperationType.Edit);
         op.ShowDialog();
         op.Dispose();
         if (op.OpenPath != null)
         {
            var cp = new CreateFolderPopup(DAZScraperModel.Config.GetLibrarySaveDirectory(), DAZScraperModel.Config.GetSortingSaveDirectory(), op.OpenPath.Substring(op.OpenPath.LastIndexOf('\\') + 1));
            cp.ShowDialog();
            cp.Dispose();
         }
         UnlockManipulators();
      }

      private void deleteKeywordFolderButton_Click(object sender, EventArgs e)
      {
         LockManipulators();
         var op = new OpenFolderPopup(DAZScraperModel.Config.GetSortingSaveDirectory(), OpenFolderPopup.OperationType.Delete);
         op.ShowDialog();
         op.Dispose();
         if (op.OpenPath != null)
         {
            if (Directory.Exists(op.OpenPath))
               Directory.Delete(op.OpenPath, true);
            File.Delete(op.OpenPath + ".txt");
         }
         UnlockManipulators();
      }
   }
}

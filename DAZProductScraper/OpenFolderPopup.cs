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
   public partial class OpenFolderPopup : Form
   {
      string parentFolderPath;
      string toOpen = null;
      const string noFoldersMessage = "[You have no keyword sorting folders]";

      public OpenFolderPopup(string parentFolderPath)
      {
         this.parentFolderPath = parentFolderPath;
         AcceptButton = openFolderButton;
         CancelButton = cancelButton;
         InitializeComponent();
      }

      private void folderComboBox_OnDropDown(object sender, EventArgs e)
      {
         folderComboBox.Items.Clear(); //clear the old ones because they might have deleted some keywords
         folderComboBox.Items.AddRange(Directory.EnumerateDirectories(parentFolderPath).Select(x => x.Substring(x.LastIndexOf("\\") + 1)).ToArray());
         if (folderComboBox.Items.Count == 0)
         {
            folderComboBox.Items.Add(noFoldersMessage);
         } 
      }

      private void folderComboBox_SelectedIndexChanged(object sender, EventArgs e)
      {
         if ((string)folderComboBox.SelectedItem != noFoldersMessage)
         {
            toOpen = parentFolderPath + "\\" + folderComboBox.SelectedItem;
         }
      }

      private void cancelButton_Click(object sender, EventArgs e)
      {
         Close();
      }

      private void openFolderButton_Click(object sender, EventArgs e)
      {
         if (toOpen != null && Directory.Exists(toOpen))
         {
            ProcessStartInfo si = new ProcessStartInfo
            {
               Arguments = toOpen,
               FileName = "explorer.exe"
            };
            Process.Start(si);
            Close();
         }
      }
   }
}

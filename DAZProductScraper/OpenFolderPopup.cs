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
      public enum OperationType
      {
         Open,
         Edit,
         Delete
      }

      string parentFolderPath;
      string toOpen = null;
      OperationType opType;
      public string OpenPath { get => toOpen; }
      const string noFoldersMessage = "[You have no keyword sorting folders]";

      public OpenFolderPopup(string parentFolderPath, OperationType opType = OperationType.Open)
      {
         this.parentFolderPath = parentFolderPath;
         AcceptButton = openFolderButton;
         CancelButton = cancelButton;
         this.opType = opType;
         InitializeComponent();
         switch (opType)
         {
            case OperationType.Open:
               Text = "Open a Keyword Sorting Folder";
               openFolderButton.Text = "Open Folder";
               folderComboBox.Text = "Select a keyword folder to open...";
               break;
            case OperationType.Edit:
               Text = "Edit a Keyword Sorting Folder";
               openFolderButton.Text = "Edit Folder";
               folderComboBox.Text = "Select a keyword folder to edit...";
               break;
            case OperationType.Delete:
               Text = "Delete a Keyword Sorting Folder";
               openFolderButton.Text = "Delete Folder";
               folderComboBox.Text = "Select a keyword folder to delete...";
               break;
            default:
               throw new ArgumentException("only pass enum values");
               break;
         }
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
         toOpen = null;
         Close();
      }

      private void openFolderButton_Click(object sender, EventArgs e)
      {
         if (opType == OperationType.Delete && folderComboBox.SelectedItem != null)
         {
            if (MessageBox.Show(this, $"Are you SURE you want to delete the keyword sorting folder \"{folderComboBox.SelectedItem}\"", 
               "Delete a Keyword Sorting Folder", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
            {
               return;
            }
         }
         Close();
      }
   }
}

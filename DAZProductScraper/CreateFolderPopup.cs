﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using IWshRuntimeLibrary;

namespace DAZProductScraper
{
   public partial class CreateFolderPopup : Form
   {
      private bool overwrite = false;

      public CreateFolderPopup(string selectedSortFolder = null)
      {
         InitializeComponent();
         if (selectedSortFolder != null)
         {
            paramsTextBox.Text = System.IO.File.ReadAllText(DAZScraperModel.Config.GetSortingFolderTextByName(selectedSortFolder));
            nameTextBox.Text = selectedSortFolder;
            nameTextBox.Enabled = false;
            createFolderButton.Text = "Overwrite Folder";
            overwrite = true;
            Text = "Overwrite a Keyword Sorting Folder";
         }
      }

      private void cancelButton_Click(object sender, EventArgs e)
      {
         Close();
      }

      private void createFolderButton_Click(object sender, EventArgs e)
      {
         string errorMessage = DAZScraperModel.AttemptCreateSortingFolder(nameTextBox.Text.Trim(), paramsTextBox.Text, overwrite);//AttemptCreateFolder(nameTextBox.Text.Trim(), paramsTextBox.Text);
         if (errorMessage == null)
         {
            Close();
         }
         else
         {
            MessageBox.Show(this, "Error", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      //move to model
      //private bool AttemptCreateFolder(string name, string @params)
      //{
      //   if (string.IsNullOrWhiteSpace(@params))
      //   {
      //      MessageBox.Show(this, "Error", "You must have at least one parameter", MessageBoxButtons.OK, MessageBoxIcon.Error);
      //      return false;
      //   }
      //   if (Directory.Exists($"{sortPath}\\{name}") && enforceUniqueFilename)
      //   {
      //      //deny, that name already exists.
      //      MessageBox.Show(this, "Error", "A sorting keyword folder already exists with that name", MessageBoxButtons.OK, MessageBoxIcon.Error);
      //      return false;
      //   }
      //   Regex validityRegex = new Regex("-\"(.+?)(?<!\\\\)\"");
      //   IEnumerable<string> args = validityRegex.Matches(@params).Cast<Match>().Select(x => x.Value.Trim());
      //   bool valid = true;
      //   foreach (string s in args)
      //   {
      //      valid &= validityRegex.IsMatch(s);
      //   }
      //   if (!valid)
      //   {
      //      //reject because the string isn't right
      //      MessageBox.Show(this, "Error", "The provided parameters were not parseable", MessageBoxButtons.OK, MessageBoxIcon.Error);
      //      return false;
      //   }
      //   StringBuilder pattern = new StringBuilder();
      //   foreach (string s in args)
      //   {
      //      pattern.Append($"({validityRegex.Match(s).Groups[1].Value})|");
      //   }
      //   pattern.Remove(pattern.Length - 1, 1); //remove last pipe
      //   Regex reg;
      //   try
      //   {
      //      reg = new Regex(pattern.ToString());
      //   }
      //   catch (ArgumentException e)
      //   {
      //      //reject because full regex is broken (this means some part of the regex isn't valid)
      //      //keep track of the regexes and what line they're on and report what line wasn't parseable
      //      MessageBox.Show(this, "Error", "The generated regular expression was not valid", MessageBoxButtons.OK, MessageBoxIcon.Error);
      //      return false;
      //   }
      //   //foreach text file that matches the regex, add a shortcut to that pid's images
      //   HashSet<string> pidsToAdd = new HashSet<string>();
      //   Regex getPidFromFilename = new Regex(".+_(\\d+)\\.txt");
      //   foreach (string f in Directory.EnumerateFiles(libPath, "*.txt"))
      //   {
      //      if (reg.IsMatch(System.IO.File.ReadAllText(f)))
      //      {
      //         pidsToAdd.Add(getPidFromFilename.Match(f).Groups[1].Value);
      //      }
      //   }
      //   if (Directory.Exists($"{sortPath}\\{name}"))
      //      Directory.Delete($"{sortPath}\\{name}", true);
      //   Directory.CreateDirectory($"{sortPath}\\{name}");
      //   Regex getImageFilename = new Regex(".+\\\\(.+_)(\\d+)(-\\d+)\\.jpg");
      //   foreach (string f in Directory.EnumerateFiles(libPath, "*.jpg")) //"*_[pid]-*.jpg"
      //   {
      //      Match m = getImageFilename.Match(f);
      //      if (pidsToAdd.Contains(m.Groups[2].Value))
      //      {
      //         //shortcut this image.
      //         WshShell shell = new WshShell();
      //         IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut($"{sortPath}\\{name}\\{m.Groups[1].Value + m.Groups[2].Value + m.Groups[3].Value}.lnk");

      //         shortcut.TargetPath = f;
      //         shortcut.Save();
      //      }
      //   }
      //   System.IO.File.WriteAllText($"{sortPath}\\{name}.txt", @params);
      //   return true;
      //}
   }
}
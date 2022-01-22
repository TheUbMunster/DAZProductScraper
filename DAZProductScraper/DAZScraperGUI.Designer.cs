
namespace DAZProductScraper
{
   partial class DAZScraperGUI
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.openRootFolderButton = new System.Windows.Forms.Button();
         this.updateDatabaseFull = new System.Windows.Forms.Button();
         this.createKeywordFolder = new System.Windows.Forms.Button();
         this.updateKeywordFolder = new System.Windows.Forms.Button();
         this.deleteKeywordFolder = new System.Windows.Forms.Button();
         this.deleteAllKeywordFolders = new System.Windows.Forms.Button();
         this.openKeywordFolder = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // openRootFolderButton
         // 
         this.openRootFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.openRootFolderButton.Location = new System.Drawing.Point(12, 12);
         this.openRootFolderButton.Name = "openRootFolderButton";
         this.openRootFolderButton.Size = new System.Drawing.Size(260, 23);
         this.openRootFolderButton.TabIndex = 0;
         this.openRootFolderButton.Text = "Open Root Folder";
         this.openRootFolderButton.UseVisualStyleBackColor = true;
         this.openRootFolderButton.Click += new System.EventHandler(this.openRootFolderButton_Click);
         // 
         // updateDatabaseFull
         // 
         this.updateDatabaseFull.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.updateDatabaseFull.Location = new System.Drawing.Point(12, 42);
         this.updateDatabaseFull.Name = "updateDatabaseFull";
         this.updateDatabaseFull.Size = new System.Drawing.Size(260, 23);
         this.updateDatabaseFull.TabIndex = 1;
         this.updateDatabaseFull.Text = "Update Local Product Database";
         this.updateDatabaseFull.UseVisualStyleBackColor = true;
         // 
         // createKeywordFolder
         // 
         this.createKeywordFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.createKeywordFolder.Location = new System.Drawing.Point(12, 73);
         this.createKeywordFolder.Name = "createKeywordFolder";
         this.createKeywordFolder.Size = new System.Drawing.Size(260, 23);
         this.createKeywordFolder.TabIndex = 2;
         this.createKeywordFolder.Text = "Create Keyword Sorting Folder";
         this.createKeywordFolder.UseVisualStyleBackColor = true;
         // 
         // updateKeywordFolder
         // 
         this.updateKeywordFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.updateKeywordFolder.Location = new System.Drawing.Point(12, 131);
         this.updateKeywordFolder.Name = "updateKeywordFolder";
         this.updateKeywordFolder.Size = new System.Drawing.Size(260, 23);
         this.updateKeywordFolder.TabIndex = 3;
         this.updateKeywordFolder.Text = "Update a Keyword Sorting Folder";
         this.updateKeywordFolder.UseVisualStyleBackColor = true;
         // 
         // deleteKeywordFolder
         // 
         this.deleteKeywordFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.deleteKeywordFolder.Location = new System.Drawing.Point(12, 161);
         this.deleteKeywordFolder.Name = "deleteKeywordFolder";
         this.deleteKeywordFolder.Size = new System.Drawing.Size(260, 23);
         this.deleteKeywordFolder.TabIndex = 4;
         this.deleteKeywordFolder.Text = "Delete a Keyword Sorting Folder";
         this.deleteKeywordFolder.UseVisualStyleBackColor = true;
         // 
         // deleteAllKeywordFolders
         // 
         this.deleteAllKeywordFolders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.deleteAllKeywordFolders.Location = new System.Drawing.Point(13, 191);
         this.deleteAllKeywordFolders.Name = "deleteAllKeywordFolders";
         this.deleteAllKeywordFolders.Size = new System.Drawing.Size(259, 23);
         this.deleteAllKeywordFolders.TabIndex = 5;
         this.deleteAllKeywordFolders.Text = "Delete All Keyword Sorting Folders";
         this.deleteAllKeywordFolders.UseVisualStyleBackColor = true;
         // 
         // openKeywordFolder
         // 
         this.openKeywordFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.openKeywordFolder.Location = new System.Drawing.Point(12, 102);
         this.openKeywordFolder.Name = "openKeywordFolder";
         this.openKeywordFolder.Size = new System.Drawing.Size(260, 23);
         this.openKeywordFolder.TabIndex = 6;
         this.openKeywordFolder.Text = "Open a Keyword Sorting Folder";
         this.openKeywordFolder.UseVisualStyleBackColor = true;
         // 
         // DAZScraperGUI
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
         this.ClientSize = new System.Drawing.Size(284, 450);
         this.Controls.Add(this.openKeywordFolder);
         this.Controls.Add(this.deleteAllKeywordFolders);
         this.Controls.Add(this.deleteKeywordFolder);
         this.Controls.Add(this.updateKeywordFolder);
         this.Controls.Add(this.createKeywordFolder);
         this.Controls.Add(this.updateDatabaseFull);
         this.Controls.Add(this.openRootFolderButton);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.MaximizeBox = false;
         this.Name = "DAZScraperGUI";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "DAZ Product Scraper";
         this.Shown += new System.EventHandler(this.DAZScraperGUI_Shown);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button openRootFolderButton;
      private System.Windows.Forms.Button updateDatabaseFull;
      private System.Windows.Forms.Button createKeywordFolder;
      private System.Windows.Forms.Button updateKeywordFolder;
      private System.Windows.Forms.Button deleteKeywordFolder;
      private System.Windows.Forms.Button deleteAllKeywordFolders;
      private System.Windows.Forms.Button openKeywordFolder;
   }
}



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
         this.SuspendLayout();
         // 
         // openRootFolderButton
         // 
         this.openRootFolderButton.Location = new System.Drawing.Point(394, 172);
         this.openRootFolderButton.Name = "openRootFolderButton";
         this.openRootFolderButton.Size = new System.Drawing.Size(215, 23);
         this.openRootFolderButton.TabIndex = 0;
         this.openRootFolderButton.Text = "Open Root Folder";
         this.openRootFolderButton.UseVisualStyleBackColor = true;
         this.openRootFolderButton.Click += new System.EventHandler(this.openRootFolderButton_Click);
         // 
         // DAZScraperGUI
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(800, 450);
         this.Controls.Add(this.openRootFolderButton);
         this.Name = "DAZScraperGUI";
         this.Text = "DAZ Product Scraper";
         this.Shown += new System.EventHandler(this.DAZScraperGUI_Shown);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button openRootFolderButton;
   }
}


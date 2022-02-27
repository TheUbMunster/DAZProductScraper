
namespace DAZProductScraper
{
   partial class OpenFolderPopup
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
         this.folderComboBox = new System.Windows.Forms.ComboBox();
         this.cancelButton = new System.Windows.Forms.Button();
         this.openFolderButton = new System.Windows.Forms.Button();
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.tableLayoutPanel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // folderComboBox
         // 
         this.tableLayoutPanel1.SetColumnSpan(this.folderComboBox, 2);
         this.folderComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.folderComboBox.FormattingEnabled = true;
         this.folderComboBox.Location = new System.Drawing.Point(5, 5);
         this.folderComboBox.Name = "folderComboBox";
         this.folderComboBox.Size = new System.Drawing.Size(465, 21);
         this.folderComboBox.TabIndex = 1;
         this.folderComboBox.Text = "Select a keyword folder to open...";
         this.folderComboBox.DropDown += new System.EventHandler(this.folderComboBox_OnDropDown);
         this.folderComboBox.SelectedIndexChanged += new System.EventHandler(this.folderComboBox_SelectedIndexChanged);
         // 
         // cancelButton
         // 
         this.cancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
         this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
         this.cancelButton.Location = new System.Drawing.Point(5, 33);
         this.cancelButton.Name = "cancelButton";
         this.cancelButton.Size = new System.Drawing.Size(229, 45);
         this.cancelButton.TabIndex = 8;
         this.cancelButton.Text = "Cancel";
         this.cancelButton.UseVisualStyleBackColor = true;
         this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
         // 
         // openFolderButton
         // 
         this.openFolderButton.Dock = System.Windows.Forms.DockStyle.Fill;
         this.openFolderButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
         this.openFolderButton.Location = new System.Drawing.Point(240, 33);
         this.openFolderButton.Name = "openFolderButton";
         this.openFolderButton.Size = new System.Drawing.Size(230, 45);
         this.openFolderButton.TabIndex = 9;
         this.openFolderButton.Text = "Open Folder";
         this.openFolderButton.UseVisualStyleBackColor = true;
         this.openFolderButton.Click += new System.EventHandler(this.openFolderButton_Click);
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 2;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.Controls.Add(this.cancelButton, 0, 1);
         this.tableLayoutPanel1.Controls.Add(this.folderComboBox, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.openFolderButton, 1, 1);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(2);
         this.tableLayoutPanel1.RowCount = 2;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(475, 83);
         this.tableLayoutPanel1.TabIndex = 10;
         // 
         // OpenFolderPopup
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(475, 83);
         this.Controls.Add(this.tableLayoutPanel1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "OpenFolderPopup";
         this.ShowIcon = false;
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "Open a Keyword Sorting Folder";
         this.tableLayoutPanel1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.ComboBox folderComboBox;
      private System.Windows.Forms.Button cancelButton;
      private System.Windows.Forms.Button openFolderButton;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
   }
}
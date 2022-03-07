
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
         this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
         this.tableLayoutPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // folderComboBox
         // 
         this.tableLayoutPanel.SetColumnSpan(this.folderComboBox, 2);
         this.folderComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.folderComboBox.FormattingEnabled = true;
         this.folderComboBox.Location = new System.Drawing.Point(3, 3);
         this.folderComboBox.Name = "folderComboBox";
         this.folderComboBox.Size = new System.Drawing.Size(478, 21);
         this.folderComboBox.TabIndex = 1;
         this.folderComboBox.Text = "Select a keyword folder to open...";
         this.folderComboBox.DropDown += new System.EventHandler(this.folderComboBox_OnDropDown);
         this.folderComboBox.SelectedIndexChanged += new System.EventHandler(this.folderComboBox_SelectedIndexChanged);
         // 
         // cancelButton
         // 
         this.cancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
         this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
         this.cancelButton.Location = new System.Drawing.Point(3, 31);
         this.cancelButton.Name = "cancelButton";
         this.cancelButton.Size = new System.Drawing.Size(236, 60);
         this.cancelButton.TabIndex = 2;
         this.cancelButton.Text = "Cancel";
         this.cancelButton.UseVisualStyleBackColor = true;
         this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
         // 
         // openFolderButton
         // 
         this.openFolderButton.Dock = System.Windows.Forms.DockStyle.Fill;
         this.openFolderButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
         this.openFolderButton.Location = new System.Drawing.Point(245, 31);
         this.openFolderButton.Name = "openFolderButton";
         this.openFolderButton.Size = new System.Drawing.Size(236, 60);
         this.openFolderButton.TabIndex = 3;
         this.openFolderButton.Text = "Open Folder";
         this.openFolderButton.UseVisualStyleBackColor = true;
         this.openFolderButton.Click += new System.EventHandler(this.openFolderButton_Click);
         // 
         // tableLayoutPanel
         // 
         this.tableLayoutPanel.ColumnCount = 2;
         this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel.Controls.Add(this.folderComboBox, 0, 0);
         this.tableLayoutPanel.Controls.Add(this.openFolderButton, 1, 1);
         this.tableLayoutPanel.Controls.Add(this.cancelButton, 0, 1);
         this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel.Name = "tableLayoutPanel";
         this.tableLayoutPanel.RowCount = 2;
         this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
         this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel.Size = new System.Drawing.Size(484, 94);
         this.tableLayoutPanel.TabIndex = 0;
         // 
         // OpenFolderPopup
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(484, 94);
         this.Controls.Add(this.tableLayoutPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "OpenFolderPopup";
         this.ShowIcon = false;
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "Open a Keyword Sorting Folder";
         this.TopMost = true;
         this.tableLayoutPanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.ComboBox folderComboBox;
      private System.Windows.Forms.Button cancelButton;
      private System.Windows.Forms.Button openFolderButton;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
   }
}
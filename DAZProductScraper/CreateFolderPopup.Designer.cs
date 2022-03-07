
namespace DAZProductScraper
{
   partial class CreateFolderPopup
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
         this.paramsTextBox = new System.Windows.Forms.TextBox();
         this.createFolderButton = new System.Windows.Forms.Button();
         this.cancelButton = new System.Windows.Forms.Button();
         this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
         this.nameTextBox = new System.Windows.Forms.TextBox();
         this.nameLabel = new System.Windows.Forms.Label();
         this.tableLayoutPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // paramsTextBox
         // 
         this.tableLayoutPanel.SetColumnSpan(this.paramsTextBox, 2);
         this.paramsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.paramsTextBox.Location = new System.Drawing.Point(3, 29);
         this.paramsTextBox.Multiline = true;
         this.paramsTextBox.Name = "paramsTextBox";
         this.paramsTextBox.Size = new System.Drawing.Size(478, 49);
         this.paramsTextBox.TabIndex = 0;
         // 
         // createFolderButton
         // 
         this.createFolderButton.Dock = System.Windows.Forms.DockStyle.Fill;
         this.createFolderButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
         this.createFolderButton.Location = new System.Drawing.Point(245, 84);
         this.createFolderButton.Name = "createFolderButton";
         this.createFolderButton.Size = new System.Drawing.Size(236, 54);
         this.createFolderButton.TabIndex = 2;
         this.createFolderButton.Text = "Create Folder";
         this.createFolderButton.UseVisualStyleBackColor = true;
         this.createFolderButton.Click += new System.EventHandler(this.createFolderButton_Click);
         // 
         // cancelButton
         // 
         this.cancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
         this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
         this.cancelButton.Location = new System.Drawing.Point(3, 84);
         this.cancelButton.Name = "cancelButton";
         this.cancelButton.Size = new System.Drawing.Size(236, 54);
         this.cancelButton.TabIndex = 1;
         this.cancelButton.Text = "Cancel";
         this.cancelButton.UseVisualStyleBackColor = true;
         this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
         // 
         // tableLayoutPanel
         // 
         this.tableLayoutPanel.ColumnCount = 2;
         this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel.Controls.Add(this.cancelButton, 0, 2);
         this.tableLayoutPanel.Controls.Add(this.createFolderButton, 1, 2);
         this.tableLayoutPanel.Controls.Add(this.paramsTextBox, 0, 1);
         this.tableLayoutPanel.Controls.Add(this.nameTextBox, 1, 0);
         this.tableLayoutPanel.Controls.Add(this.nameLabel, 0, 0);
         this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel.Name = "tableLayoutPanel";
         this.tableLayoutPanel.RowCount = 3;
         this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
         this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
         this.tableLayoutPanel.Size = new System.Drawing.Size(484, 141);
         this.tableLayoutPanel.TabIndex = 3;
         // 
         // nameTextBox
         // 
         this.nameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.nameTextBox.Location = new System.Drawing.Point(245, 3);
         this.nameTextBox.Name = "nameTextBox";
         this.nameTextBox.Size = new System.Drawing.Size(236, 20);
         this.nameTextBox.TabIndex = 3;
         // 
         // nameLabel
         // 
         this.nameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F);
         this.nameLabel.Location = new System.Drawing.Point(3, 0);
         this.nameLabel.Name = "nameLabel";
         this.nameLabel.Size = new System.Drawing.Size(236, 26);
         this.nameLabel.TabIndex = 4;
         this.nameLabel.Text = "Sorting Folder Name:";
         this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // CreateFolderPopup
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(484, 141);
         this.Controls.Add(this.tableLayoutPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.Name = "CreateFolderPopup";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "Create a Keyword Sorting Folder";
         this.tableLayoutPanel.ResumeLayout(false);
         this.tableLayoutPanel.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TextBox paramsTextBox;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
      private System.Windows.Forms.Button cancelButton;
      private System.Windows.Forms.Button createFolderButton;
      private System.Windows.Forms.TextBox nameTextBox;
      private System.Windows.Forms.Label nameLabel;
   }
}
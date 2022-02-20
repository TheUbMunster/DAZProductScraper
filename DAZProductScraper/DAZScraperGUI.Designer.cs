
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
         this.databaseLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
         this.separatorLabel1 = new System.Windows.Forms.Label();
         this.databaseLabel = new System.Windows.Forms.Label();
         this.clearDatabaseButton = new System.Windows.Forms.Button();
         this.rebuildDatabaseButton = new System.Windows.Forms.Button();
         this.updateDatabseButton = new System.Windows.Forms.Button();
         this.keywordSortingLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
         this.separatorLabel2 = new System.Windows.Forms.Label();
         this.refreshKeywordFoldersButton = new System.Windows.Forms.Button();
         this.deleteKeywordFolderButton = new System.Windows.Forms.Button();
         this.keywordSortingLabel = new System.Windows.Forms.Label();
         this.createKeywordFolderButton = new System.Windows.Forms.Button();
         this.editKeywordFolderButton = new System.Windows.Forms.Button();
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.separatorLabel3 = new System.Windows.Forms.Label();
         this.folderLabels = new System.Windows.Forms.Label();
         this.openRootFolderButton = new System.Windows.Forms.Button();
         this.openKeywordFolderButton = new System.Windows.Forms.Button();
         this.helpButton = new System.Windows.Forms.Button();
         this.quitButton = new System.Windows.Forms.Button();
         this.debugLogRTB = new System.Windows.Forms.RichTextBox();
         this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
         this.databaseLayoutPanel.SuspendLayout();
         this.keywordSortingLayoutPanel.SuspendLayout();
         this.tableLayoutPanel1.SuspendLayout();
         this.tableLayoutPanel2.SuspendLayout();
         this.SuspendLayout();
         // 
         // databaseLayoutPanel
         // 
         this.databaseLayoutPanel.ColumnCount = 3;
         this.databaseLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
         this.databaseLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
         this.databaseLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
         this.databaseLayoutPanel.Controls.Add(this.separatorLabel1, 0, 2);
         this.databaseLayoutPanel.Controls.Add(this.databaseLabel, 0, 0);
         this.databaseLayoutPanel.Controls.Add(this.clearDatabaseButton, 2, 1);
         this.databaseLayoutPanel.Controls.Add(this.rebuildDatabaseButton, 0, 1);
         this.databaseLayoutPanel.Controls.Add(this.updateDatabseButton, 1, 1);
         this.databaseLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
         this.databaseLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
         this.databaseLayoutPanel.Location = new System.Drawing.Point(0, 0);
         this.databaseLayoutPanel.Name = "databaseLayoutPanel";
         this.databaseLayoutPanel.RowCount = 3;
         this.databaseLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.databaseLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.databaseLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
         this.databaseLayoutPanel.Size = new System.Drawing.Size(584, 70);
         this.databaseLayoutPanel.TabIndex = 0;
         // 
         // separatorLabel1
         // 
         this.separatorLabel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.databaseLayoutPanel.SetColumnSpan(this.separatorLabel1, 3);
         this.separatorLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.separatorLabel1.Location = new System.Drawing.Point(3, 68);
         this.separatorLabel1.Name = "separatorLabel1";
         this.separatorLabel1.Size = new System.Drawing.Size(578, 2);
         this.separatorLabel1.TabIndex = 0;
         // 
         // databaseLabel
         // 
         this.databaseLayoutPanel.SetColumnSpan(this.databaseLabel, 3);
         this.databaseLabel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.databaseLabel.Location = new System.Drawing.Point(3, 0);
         this.databaseLabel.Name = "databaseLabel";
         this.databaseLabel.Size = new System.Drawing.Size(578, 20);
         this.databaseLabel.TabIndex = 0;
         this.databaseLabel.Text = "Local Database Controls";
         this.databaseLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // clearDatabaseButton
         // 
         this.clearDatabaseButton.Dock = System.Windows.Forms.DockStyle.Fill;
         this.clearDatabaseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
         this.clearDatabaseButton.ForeColor = System.Drawing.Color.Red;
         this.clearDatabaseButton.Location = new System.Drawing.Point(391, 23);
         this.clearDatabaseButton.Name = "clearDatabaseButton";
         this.clearDatabaseButton.Size = new System.Drawing.Size(190, 42);
         this.clearDatabaseButton.TabIndex = 2;
         this.clearDatabaseButton.Text = "Clear Database";
         this.clearDatabaseButton.UseVisualStyleBackColor = true;
         // 
         // rebuildDatabaseButton
         // 
         this.rebuildDatabaseButton.Dock = System.Windows.Forms.DockStyle.Fill;
         this.rebuildDatabaseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.rebuildDatabaseButton.Location = new System.Drawing.Point(3, 23);
         this.rebuildDatabaseButton.Name = "rebuildDatabaseButton";
         this.rebuildDatabaseButton.Size = new System.Drawing.Size(188, 42);
         this.rebuildDatabaseButton.TabIndex = 0;
         this.rebuildDatabaseButton.Text = "Rebuild Database";
         this.rebuildDatabaseButton.UseVisualStyleBackColor = true;
         // 
         // updateDatabseButton
         // 
         this.updateDatabseButton.Dock = System.Windows.Forms.DockStyle.Fill;
         this.updateDatabseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
         this.updateDatabseButton.Location = new System.Drawing.Point(197, 23);
         this.updateDatabseButton.Name = "updateDatabseButton";
         this.updateDatabseButton.Size = new System.Drawing.Size(188, 42);
         this.updateDatabseButton.TabIndex = 1;
         this.updateDatabseButton.Text = "Update Database";
         this.updateDatabseButton.UseVisualStyleBackColor = true;
         // 
         // keywordSortingLayoutPanel
         // 
         this.keywordSortingLayoutPanel.ColumnCount = 2;
         this.keywordSortingLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.keywordSortingLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.keywordSortingLayoutPanel.Controls.Add(this.separatorLabel2, 0, 3);
         this.keywordSortingLayoutPanel.Controls.Add(this.refreshKeywordFoldersButton, 0, 2);
         this.keywordSortingLayoutPanel.Controls.Add(this.deleteKeywordFolderButton, 1, 2);
         this.keywordSortingLayoutPanel.Controls.Add(this.keywordSortingLabel, 0, 0);
         this.keywordSortingLayoutPanel.Controls.Add(this.createKeywordFolderButton, 0, 1);
         this.keywordSortingLayoutPanel.Controls.Add(this.editKeywordFolderButton, 1, 1);
         this.keywordSortingLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
         this.keywordSortingLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
         this.keywordSortingLayoutPanel.Location = new System.Drawing.Point(0, 70);
         this.keywordSortingLayoutPanel.Name = "keywordSortingLayoutPanel";
         this.keywordSortingLayoutPanel.RowCount = 4;
         this.keywordSortingLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.keywordSortingLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.keywordSortingLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.keywordSortingLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
         this.keywordSortingLayoutPanel.Size = new System.Drawing.Size(584, 119);
         this.keywordSortingLayoutPanel.TabIndex = 1;
         // 
         // separatorLabel2
         // 
         this.separatorLabel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.keywordSortingLayoutPanel.SetColumnSpan(this.separatorLabel2, 2);
         this.separatorLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.separatorLabel2.Location = new System.Drawing.Point(3, 116);
         this.separatorLabel2.Name = "separatorLabel2";
         this.separatorLabel2.Size = new System.Drawing.Size(578, 3);
         this.separatorLabel2.TabIndex = 0;
         // 
         // refreshKeywordFoldersButton
         // 
         this.refreshKeywordFoldersButton.Dock = System.Windows.Forms.DockStyle.Fill;
         this.refreshKeywordFoldersButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
         this.refreshKeywordFoldersButton.Location = new System.Drawing.Point(3, 71);
         this.refreshKeywordFoldersButton.Name = "refreshKeywordFoldersButton";
         this.refreshKeywordFoldersButton.Size = new System.Drawing.Size(286, 42);
         this.refreshKeywordFoldersButton.TabIndex = 4;
         this.refreshKeywordFoldersButton.Text = "Refresh All Keyword Folders";
         this.refreshKeywordFoldersButton.UseVisualStyleBackColor = true;
         // 
         // deleteKeywordFolderButton
         // 
         this.deleteKeywordFolderButton.Dock = System.Windows.Forms.DockStyle.Fill;
         this.deleteKeywordFolderButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
         this.deleteKeywordFolderButton.ForeColor = System.Drawing.Color.Red;
         this.deleteKeywordFolderButton.Location = new System.Drawing.Point(295, 71);
         this.deleteKeywordFolderButton.Name = "deleteKeywordFolderButton";
         this.deleteKeywordFolderButton.Size = new System.Drawing.Size(286, 42);
         this.deleteKeywordFolderButton.TabIndex = 6;
         this.deleteKeywordFolderButton.Text = "Delete Keyword Folder";
         this.deleteKeywordFolderButton.UseVisualStyleBackColor = true;
         // 
         // keywordSortingLabel
         // 
         this.keywordSortingLayoutPanel.SetColumnSpan(this.keywordSortingLabel, 2);
         this.keywordSortingLabel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.keywordSortingLabel.Location = new System.Drawing.Point(3, 0);
         this.keywordSortingLabel.Name = "keywordSortingLabel";
         this.keywordSortingLabel.Size = new System.Drawing.Size(578, 20);
         this.keywordSortingLabel.TabIndex = 0;
         this.keywordSortingLabel.Text = "Keyword Sorting Controls";
         this.keywordSortingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // createKeywordFolderButton
         // 
         this.createKeywordFolderButton.Dock = System.Windows.Forms.DockStyle.Fill;
         this.createKeywordFolderButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
         this.createKeywordFolderButton.Location = new System.Drawing.Point(3, 23);
         this.createKeywordFolderButton.Name = "createKeywordFolderButton";
         this.createKeywordFolderButton.Size = new System.Drawing.Size(286, 42);
         this.createKeywordFolderButton.TabIndex = 3;
         this.createKeywordFolderButton.Text = "Create Keyword Folder";
         this.createKeywordFolderButton.UseVisualStyleBackColor = true;
         // 
         // editKeywordFolderButton
         // 
         this.editKeywordFolderButton.Dock = System.Windows.Forms.DockStyle.Fill;
         this.editKeywordFolderButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
         this.editKeywordFolderButton.Location = new System.Drawing.Point(295, 23);
         this.editKeywordFolderButton.Name = "editKeywordFolderButton";
         this.editKeywordFolderButton.Size = new System.Drawing.Size(286, 42);
         this.editKeywordFolderButton.TabIndex = 5;
         this.editKeywordFolderButton.Text = "Edit Keyword Folder";
         this.editKeywordFolderButton.UseVisualStyleBackColor = true;
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 2;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.Controls.Add(this.separatorLabel3, 0, 2);
         this.tableLayoutPanel1.Controls.Add(this.folderLabels, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.openRootFolderButton, 0, 1);
         this.tableLayoutPanel1.Controls.Add(this.openKeywordFolderButton, 1, 1);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 189);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 3;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(584, 70);
         this.tableLayoutPanel1.TabIndex = 2;
         // 
         // separatorLabel3
         // 
         this.separatorLabel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.tableLayoutPanel1.SetColumnSpan(this.separatorLabel3, 2);
         this.separatorLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
         this.separatorLabel3.Location = new System.Drawing.Point(3, 68);
         this.separatorLabel3.Name = "separatorLabel3";
         this.separatorLabel3.Size = new System.Drawing.Size(578, 2);
         this.separatorLabel3.TabIndex = 0;
         // 
         // folderLabels
         // 
         this.tableLayoutPanel1.SetColumnSpan(this.folderLabels, 2);
         this.folderLabels.Dock = System.Windows.Forms.DockStyle.Fill;
         this.folderLabels.Location = new System.Drawing.Point(3, 0);
         this.folderLabels.Name = "folderLabels";
         this.folderLabels.Size = new System.Drawing.Size(578, 20);
         this.folderLabels.TabIndex = 0;
         this.folderLabels.Text = "Folder Controls";
         this.folderLabels.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // openRootFolderButton
         // 
         this.openRootFolderButton.Dock = System.Windows.Forms.DockStyle.Fill;
         this.openRootFolderButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
         this.openRootFolderButton.Location = new System.Drawing.Point(3, 23);
         this.openRootFolderButton.Name = "openRootFolderButton";
         this.openRootFolderButton.Size = new System.Drawing.Size(286, 42);
         this.openRootFolderButton.TabIndex = 7;
         this.openRootFolderButton.Text = "Open Root Folder";
         this.openRootFolderButton.UseVisualStyleBackColor = true;
         this.openRootFolderButton.Click += new System.EventHandler(this.openRootFolderButton_Click);
         // 
         // openKeywordFolderButton
         // 
         this.openKeywordFolderButton.Dock = System.Windows.Forms.DockStyle.Fill;
         this.openKeywordFolderButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
         this.openKeywordFolderButton.Location = new System.Drawing.Point(295, 23);
         this.openKeywordFolderButton.Name = "openKeywordFolderButton";
         this.openKeywordFolderButton.Size = new System.Drawing.Size(286, 42);
         this.openKeywordFolderButton.TabIndex = 8;
         this.openKeywordFolderButton.Text = "Open Keyword Folder";
         this.openKeywordFolderButton.UseVisualStyleBackColor = true;
         // 
         // helpButton
         // 
         this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.helpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
         this.helpButton.Location = new System.Drawing.Point(470, 3);
         this.helpButton.Name = "helpButton";
         this.helpButton.Size = new System.Drawing.Size(111, 45);
         this.helpButton.TabIndex = 9;
         this.helpButton.Text = "Help";
         this.helpButton.UseVisualStyleBackColor = true;
         // 
         // quitButton
         // 
         this.quitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.quitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
         this.quitButton.Location = new System.Drawing.Point(470, 54);
         this.quitButton.Name = "quitButton";
         this.quitButton.Size = new System.Drawing.Size(111, 45);
         this.quitButton.TabIndex = 10;
         this.quitButton.Text = "Quit";
         this.quitButton.UseVisualStyleBackColor = true;
         // 
         // debugLogRTB
         // 
         this.debugLogRTB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.debugLogRTB.Location = new System.Drawing.Point(3, 3);
         this.debugLogRTB.Name = "debugLogRTB";
         this.tableLayoutPanel2.SetRowSpan(this.debugLogRTB, 2);
         this.debugLogRTB.Size = new System.Drawing.Size(461, 96);
         this.debugLogRTB.TabIndex = 0;
         this.debugLogRTB.TabStop = false;
         this.debugLogRTB.Text = "";
         // 
         // tableLayoutPanel2
         // 
         this.tableLayoutPanel2.ColumnCount = 2;
         this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
         this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
         this.tableLayoutPanel2.Controls.Add(this.debugLogRTB, 0, 0);
         this.tableLayoutPanel2.Controls.Add(this.quitButton, 1, 1);
         this.tableLayoutPanel2.Controls.Add(this.helpButton, 1, 0);
         this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 259);
         this.tableLayoutPanel2.Name = "tableLayoutPanel2";
         this.tableLayoutPanel2.RowCount = 2;
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel2.Size = new System.Drawing.Size(584, 102);
         this.tableLayoutPanel2.TabIndex = 11;
         // 
         // DAZScraperGUI
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(584, 361);
         this.Controls.Add(this.tableLayoutPanel2);
         this.Controls.Add(this.tableLayoutPanel1);
         this.Controls.Add(this.keywordSortingLayoutPanel);
         this.Controls.Add(this.databaseLayoutPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.MaximizeBox = false;
         this.Name = "DAZScraperGUI";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "DAZ Product Scraper";
         this.Shown += new System.EventHandler(this.DAZScraperGUI_Shown);
         this.databaseLayoutPanel.ResumeLayout(false);
         this.keywordSortingLayoutPanel.ResumeLayout(false);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.tableLayoutPanel2.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion
      private System.Windows.Forms.TableLayoutPanel databaseLayoutPanel;
      private System.Windows.Forms.Button clearDatabaseButton;
      private System.Windows.Forms.Button rebuildDatabaseButton;
      private System.Windows.Forms.Button updateDatabseButton;
      private System.Windows.Forms.Label databaseLabel;
      private System.Windows.Forms.TableLayoutPanel keywordSortingLayoutPanel;
      private System.Windows.Forms.Label keywordSortingLabel;
      private System.Windows.Forms.Button createKeywordFolderButton;
      private System.Windows.Forms.Button editKeywordFolderButton;
      private System.Windows.Forms.Button refreshKeywordFoldersButton;
      private System.Windows.Forms.Button deleteKeywordFolderButton;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.Label folderLabels;
      private System.Windows.Forms.Button openRootFolderButton;
      private System.Windows.Forms.Button openKeywordFolderButton;
      private System.Windows.Forms.Label separatorLabel1;
      private System.Windows.Forms.Label separatorLabel2;
      private System.Windows.Forms.Label separatorLabel3;
      private System.Windows.Forms.Button helpButton;
      private System.Windows.Forms.Button quitButton;
      private System.Windows.Forms.RichTextBox debugLogRTB;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
   }
}


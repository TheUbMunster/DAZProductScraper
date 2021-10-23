
namespace DAZProductScraperGUI
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
         this.testButton = new System.Windows.Forms.Button();
         this.emailTextBox = new System.Windows.Forms.TextBox();
         this.passTextBox = new System.Windows.Forms.TextBox();
         this.emailLabel = new System.Windows.Forms.Label();
         this.passLabel = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // testButton
         // 
         this.testButton.Location = new System.Drawing.Point(12, 64);
         this.testButton.Name = "testButton";
         this.testButton.Size = new System.Drawing.Size(110, 23);
         this.testButton.TabIndex = 2;
         this.testButton.Text = "Login and Generate";
         this.testButton.UseVisualStyleBackColor = true;
         this.testButton.Click += new System.EventHandler(this.testButton_Click);
         // 
         // emailTextBox
         // 
         this.emailTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.emailTextBox.Location = new System.Drawing.Point(53, 12);
         this.emailTextBox.Name = "emailTextBox";
         this.emailTextBox.Size = new System.Drawing.Size(735, 20);
         this.emailTextBox.TabIndex = 0;
         // 
         // passTextBox
         // 
         this.passTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.passTextBox.Location = new System.Drawing.Point(53, 38);
         this.passTextBox.Name = "passTextBox";
         this.passTextBox.PasswordChar = '*';
         this.passTextBox.Size = new System.Drawing.Size(735, 20);
         this.passTextBox.TabIndex = 1;
         // 
         // emailLabel
         // 
         this.emailLabel.AutoSize = true;
         this.emailLabel.Location = new System.Drawing.Point(12, 15);
         this.emailLabel.Name = "emailLabel";
         this.emailLabel.Size = new System.Drawing.Size(35, 13);
         this.emailLabel.TabIndex = 3;
         this.emailLabel.Text = "Email:";
         // 
         // passLabel
         // 
         this.passLabel.AutoSize = true;
         this.passLabel.Location = new System.Drawing.Point(12, 41);
         this.passLabel.Name = "passLabel";
         this.passLabel.Size = new System.Drawing.Size(33, 13);
         this.passLabel.TabIndex = 4;
         this.passLabel.Text = "Pass:";
         // 
         // DAZScraperGUI
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(800, 450);
         this.Controls.Add(this.passLabel);
         this.Controls.Add(this.emailLabel);
         this.Controls.Add(this.passTextBox);
         this.Controls.Add(this.emailTextBox);
         this.Controls.Add(this.testButton);
         this.Name = "DAZScraperGUI";
         this.Text = "DAZ Product Scraper";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button testButton;
      private System.Windows.Forms.TextBox emailTextBox;
      private System.Windows.Forms.TextBox passTextBox;
      private System.Windows.Forms.Label emailLabel;
      private System.Windows.Forms.Label passLabel;
   }
}


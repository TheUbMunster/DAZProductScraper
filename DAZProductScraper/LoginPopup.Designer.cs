
namespace DAZProductScraper
{
   partial class LoginPopup
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
         this.emailLabel = new System.Windows.Forms.Label();
         this.passLabel = new System.Windows.Forms.Label();
         this.emailTextBox = new System.Windows.Forms.TextBox();
         this.passTextBox = new System.Windows.Forms.TextBox();
         this.retainLoginInfo = new System.Windows.Forms.CheckBox();
         this.loginButton = new System.Windows.Forms.Button();
         this.infoLogRT = new System.Windows.Forms.RichTextBox();
         this.revealPasswordButton = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // emailLabel
         // 
         this.emailLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
         this.emailLabel.AutoSize = true;
         this.emailLabel.Location = new System.Drawing.Point(12, 15);
         this.emailLabel.Name = "emailLabel";
         this.emailLabel.Size = new System.Drawing.Size(35, 13);
         this.emailLabel.TabIndex = 0;
         this.emailLabel.Text = "Email:";
         // 
         // passLabel
         // 
         this.passLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
         this.passLabel.AutoSize = true;
         this.passLabel.Location = new System.Drawing.Point(12, 42);
         this.passLabel.Name = "passLabel";
         this.passLabel.Size = new System.Drawing.Size(33, 13);
         this.passLabel.TabIndex = 0;
         this.passLabel.Text = "Pass:";
         // 
         // emailTextBox
         // 
         this.emailTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.emailTextBox.Location = new System.Drawing.Point(53, 12);
         this.emailTextBox.Name = "emailTextBox";
         this.emailTextBox.Size = new System.Drawing.Size(575, 20);
         this.emailTextBox.TabIndex = 0;
         // 
         // passTextBox
         // 
         this.passTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.passTextBox.Location = new System.Drawing.Point(53, 39);
         this.passTextBox.Name = "passTextBox";
         this.passTextBox.Size = new System.Drawing.Size(547, 20);
         this.passTextBox.TabIndex = 1;
         this.passTextBox.UseSystemPasswordChar = true;
         // 
         // retainLoginInfo
         // 
         this.retainLoginInfo.AutoSize = true;
         this.retainLoginInfo.Checked = true;
         this.retainLoginInfo.CheckState = System.Windows.Forms.CheckState.Checked;
         this.retainLoginInfo.Location = new System.Drawing.Point(12, 66);
         this.retainLoginInfo.Name = "retainLoginInfo";
         this.retainLoginInfo.Size = new System.Drawing.Size(293, 17);
         this.retainLoginInfo.TabIndex = 2;
         this.retainLoginInfo.Text = "Retain login information for the remainder of this session?";
         this.retainLoginInfo.UseVisualStyleBackColor = true;
         // 
         // loginButton
         // 
         this.loginButton.Location = new System.Drawing.Point(12, 86);
         this.loginButton.Name = "loginButton";
         this.loginButton.Size = new System.Drawing.Size(293, 23);
         this.loginButton.TabIndex = 3;
         this.loginButton.Text = "Login";
         this.loginButton.UseVisualStyleBackColor = true;
         // 
         // infoLogRT
         // 
         this.infoLogRT.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.infoLogRT.Location = new System.Drawing.Point(312, 66);
         this.infoLogRT.Name = "infoLogRT";
         this.infoLogRT.ReadOnly = true;
         this.infoLogRT.Size = new System.Drawing.Size(316, 43);
         this.infoLogRT.TabIndex = 0;
         this.infoLogRT.TabStop = false;
         this.infoLogRT.Text = "";
         // 
         // revealPasswordButton
         // 
         this.revealPasswordButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.revealPasswordButton.Location = new System.Drawing.Point(606, 38);
         this.revealPasswordButton.Name = "revealPasswordButton";
         this.revealPasswordButton.Size = new System.Drawing.Size(22, 22);
         this.revealPasswordButton.TabIndex = 0;
         this.revealPasswordButton.TabStop = false;
         this.revealPasswordButton.UseVisualStyleBackColor = true;
         this.revealPasswordButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.revealPasswordButton_MouseDown);
         this.revealPasswordButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.revealPasswordButton_MouseUp);
         // 
         // LoginPopup
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(640, 121);
         this.Controls.Add(this.revealPasswordButton);
         this.Controls.Add(this.infoLogRT);
         this.Controls.Add(this.loginButton);
         this.Controls.Add(this.retainLoginInfo);
         this.Controls.Add(this.passTextBox);
         this.Controls.Add(this.emailTextBox);
         this.Controls.Add(this.passLabel);
         this.Controls.Add(this.emailLabel);
         this.HelpButton = true;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "LoginPopup";
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "Login to DAZ";
         this.TopMost = true;
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Label emailLabel;
      private System.Windows.Forms.Label passLabel;
      private System.Windows.Forms.TextBox emailTextBox;
      private System.Windows.Forms.TextBox passTextBox;
      private System.Windows.Forms.CheckBox retainLoginInfo;
      private System.Windows.Forms.Button loginButton;
      private System.Windows.Forms.RichTextBox infoLogRT;
      private System.Windows.Forms.Button revealPasswordButton;
   }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAZProductScraper
{
   public partial class LoginPopup : Form
   {
      public LoginPopup()
      {
         InitializeComponent();
      }

      private void revealPasswordButton_MouseDown(object sender, MouseEventArgs e)
      {
         passTextBox.UseSystemPasswordChar = false;
      }

      private void revealPasswordButton_MouseUp(object sender, MouseEventArgs e)
      {
         passTextBox.UseSystemPasswordChar = true;
      }
   }
}

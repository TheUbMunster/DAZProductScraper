using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAZProductScraperGUI
{
   public partial class DAZScraperGUI : Form
   {
      public DAZScraperGUI()
      {
         InitializeComponent();
         this.AcceptButton = testButton;
      }

      private void testButton_Click(object sender, EventArgs e)
      {
         DazQuickviewManager.email = emailTextBox.Text;
         DazQuickviewManager.pass = passTextBox.Text;
         DazQuickviewManager.Test();
      }
   }
}

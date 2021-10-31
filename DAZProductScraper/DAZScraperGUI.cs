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
   public partial class DAZScraperGUI : Form
   {
      public DAZScraperGUI()
      {
         InitializeComponent();
         Application.ApplicationExit += Application_ApplicationExit;
         DazQuickviewManager.Start();
      }

      private void DAZScraperGUI_Shown(object sender, EventArgs e)
      {
         Invoke(new Action(() =>
         {
            var pp = new LoginPopup();
            pp.ShowDialog(this); //pp.Show(this);
            pp.Focus();
         }));
      }

      private void Application_ApplicationExit(object sender, EventArgs e)
      {
         DazQuickviewManager.OnApplicationQuit();
      }
   }
}

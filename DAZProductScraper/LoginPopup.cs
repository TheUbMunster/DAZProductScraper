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
      #region Typedefs
      public enum ConsoleBoxColor
      {
         Green = 1,
         Yellow,
         Red,
         Grey,
         Black
      }
      #endregion

      #region Properties
      public string Email
      {
         get => emailTextBox.Text.Trim();
      }

      public string Pass
      {
         get => passTextBox.Text;
      }
      #endregion

      #region Fields
      private bool firstPrintOccurred = false;
      public event Action<LoginPopup, string, string> onClickLogin;
      #endregion

      #region Ctors
      public LoginPopup()
      {
         InitializeComponent();
      }
      #endregion

      #region Utility
      /// <summary>
      /// Prints something to the info box, for sending messages to the user e.g. invalid password.
      /// </summary>
      /// <param name="info">The string to print</param>
      /// <param name="color">The color of the text</param>
      /// <param name="clearPrior">If true, clears the text already in the box before printing</param>
      public void PrintInfoToConsoleBox(string info, ConsoleBoxColor color, bool clearPrior = false)
      {
         Color c;
         switch (color)
         {
            case ConsoleBoxColor.Green:
               c = Color.Green;
               break;
            case ConsoleBoxColor.Yellow:
               c = Color.Yellow;
               break;
            case ConsoleBoxColor.Red:
               c = Color.Red;
               break;
            case ConsoleBoxColor.Grey:
               c = Color.Gray;
               break;
            default:
            case ConsoleBoxColor.Black:
               c = Color.Black;
               break;
         }
         //conditional call instanceOfSomething.?[boolean condition expression, default value if function not called]MemberFunction();
         if (clearPrior)
         {
            infoLogRT.Clear();
            firstPrintOccurred = false;
         }
         else
         {
            firstPrintOccurred = true;
         }
         infoLogRT.SelectionColor = c;
         infoLogRT.AppendText((firstPrintOccurred ? "\n" : "") + info);
      }

      public void SetLoginButtonState(bool state)
      {
         loginButton.Enabled = state;
      }
      #endregion

      #region Event Handlers
      private void revealPasswordButton_MouseDown(object sender, MouseEventArgs e)
      {
         passTextBox.UseSystemPasswordChar = false;
      }

      private void revealPasswordButton_MouseUp(object sender, MouseEventArgs e)
      {
         passTextBox.UseSystemPasswordChar = true;
      }

      private void loginButton_Click(object sender, EventArgs e)
      {
         SetLoginButtonState(false);
         onClickLogin?.Invoke(this, Email, Pass);
      }
      #endregion
   }
}

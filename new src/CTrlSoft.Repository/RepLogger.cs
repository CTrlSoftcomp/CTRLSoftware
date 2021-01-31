using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CTrlSoft.Repository
{
    public class RepLogger
    {
        public static string AppName = Application.ProductName;

        public static DialogResult ShowMessage(
            IWin32Window owner, 
            string message,
            MessageBoxIcon messageBoxIcon,
            MessageBoxButtons messageBoxButtons, 
            MessageBoxDefaultButton messageBoxDefaultButton)
        {
            return MessageBox.Show(owner, message, AppName, messageBoxButtons, messageBoxIcon, messageBoxDefaultButton);
        }

        public static DialogResult ShowMessage(
            IWin32Window owner,
            string message,
            MessageBoxIcon messageBoxIcon,
            MessageBoxButtons messageBoxButtons)
        {
            return MessageBox.Show(owner, message, AppName, messageBoxButtons, messageBoxIcon);
        }

        public static DialogResult ShowMessage(
            string message,
            MessageBoxIcon messageBoxIcon)
        {
            return MessageBox.Show(message, AppName, MessageBoxButtons.OK, messageBoxIcon);
        }
    }
}

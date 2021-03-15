using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.Windows.Forms;
using CTrlSoft.SApp.Constant;

namespace CTrlSoft.SApp.Repository
{
    class MessageBox
    {
        public static DialogResult Tampil(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return MessageBoxAdv.Show(text, caption, buttons, icon, defaultButton);
        }
        public static DialogResult Tampil(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return MessageBoxAdv.Show(owner, text, caption, buttons, icon, defaultButton);
        }
        public static DialogResult Tampil(string text, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return MessageBoxAdv.Show(text, Public.NamaApplikasi, buttons, icon, defaultButton);
        }
        public static DialogResult Tampil(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return MessageBoxAdv.Show(owner, text, Public.NamaApplikasi, buttons, icon, defaultButton);
        }
    }
}

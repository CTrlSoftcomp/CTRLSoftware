using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CTrlSoft.SApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDA4OTAxQDMxMzgyZTM0MmUzMEcwaWFkWCtuRm1FZWwxMGtraFlTNWxqVEFFTHJhK2cvMHByNFBMSjBvUGc9");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}

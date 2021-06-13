using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.UserSkins;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using System.Threading;
using System.Globalization;

namespace CtrlSoft.AppUpdate
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Change current culture
            CultureInfo culture;
            culture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            UserLookAndFeel.Default.SetSkinStyle("Office 2016 Dark");

            if (args == null || args.ToList().Count == 0)
            {
                List<string> list = new List<string>();
                list.Add(Ini.BacaIniPath(Environment.CurrentDirectory + "\\System.ini", "AppUpdate", "AppPath", ""));
                args = list.ToArray();
                Application.Run(new frmMain(args));
            }
            else
            {
                Application.Run(new frmMain(args));
            }
        }
    }
}

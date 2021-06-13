using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CtrlSoft.AppUpdate
{
    static class Ini
    {
        public static string appini = Environment.CurrentDirectory + @"\System\Setting.ini";
        #region API Calls

        [DllImport("kernel32", EntryPoint = "WritePrivateProfileStringW", CharSet = CharSet.Unicode)]
        private static extern int WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);

        [DllImport("kernel32", EntryPoint = "GetPrivateProfileStringW", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, string lpReturnedString, int nSize, string lpFileName);



        #endregion
        #region INIRead Overloads

        private static string INIRead(string INIPath, string SectionName, string KeyName)
        {
            return INIRead(INIPath, SectionName, KeyName, "");
        }

        private static string INIRead(string INIPath, string SectionName)
        {
            return INIRead(INIPath, SectionName, null, "");
        }

        private static string INIRead(string INIPath)
        {
            return INIRead(INIPath, null, null, "");
        }

        #endregion
        private static string INIRead(string INIPath, string SectionName, string KeyName, string DefaultValue)

        {
            string INIReadRet = "";
            int n;
            string sData = Strings.Space(1024);
            n = Ini.GetPrivateProfileString(SectionName, KeyName, DefaultValue, sData, sData.Length, INIPath);
            if (n > 0)
            {
                INIReadRet = sData.Substring(0, n);
            }
            else
            {
                INIReadRet = "";
            }

            return INIReadRet;
        }

        private static void INIWrite(string INIPath, string SectionName, string KeyName, string TheValue)
        {
            Ini.WritePrivateProfileString(SectionName, KeyName, TheValue, INIPath);
        }

        private static void INIDelete(string INIPath, string SectionName, string KeyName)
        {
            string arglpString = null;
            Ini.WritePrivateProfileString(SectionName, KeyName, arglpString, INIPath);
        }

        private static void INIDelete(string INIPath, string SectionName)
        {
            string arglpKeyName = null;
            string arglpString = null;
            Ini.WritePrivateProfileString(SectionName, arglpKeyName, arglpString, INIPath);
        }

        public static string BacaIni(string Section, string Kunci, string IsiDefault)
        {
            string Sisi = INIRead(appini, Section, Kunci, IsiDefault);
            return Sisi;
        }

        public static void TulisIni(string Section, string Kunci, string Datanya)
        {
            INIWrite(appini, Section, Kunci, Datanya);
        }

        public static string BacaIniPath(string Path, string Section, string Kunci, string IsiDefault)
        {
            string Sisi = INIRead(Path, Section, Kunci, IsiDefault);
            return Sisi;
        }

        public static void TulisIniPath(string Path, string Section, string Kunci, string Datanya)
        {
            INIWrite(Path, Section, Kunci, Datanya);
        }
    }
}

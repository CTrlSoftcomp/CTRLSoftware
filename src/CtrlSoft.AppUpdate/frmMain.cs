using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using DevExpress.XtraEditors;
using IWshRuntimeLibrary;
using Microsoft.Win32;

namespace CtrlSoft.AppUpdate
{
    public partial class frmMain : DevExpress.XtraEditors.XtraForm
    {
        private string UrlYoutube = "https://www.youtube.com/watch?v=p6qIGeBInoA";

        private string VideoId
        {
            get
            {
                var yMatch = new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)").Match(UrlYoutube);
                return (yMatch.Success ? yMatch.Groups[1].Value : String.Empty);
            }
        }

        private FTP ftpClient = new FTP();

        public frmMain(String[] args)
        {
            InitializeComponent();
            if (args.Length >= 1)
            {
                ftpClient.PathProgram = (string)args.GetValue(0);
            }
        }

        private void CreateStruktur()
        {
            if (System.IO.Directory.Exists(Environment.CurrentDirectory + "/Update/"))
            {
                System.IO.Directory.Delete(Environment.CurrentDirectory + "/Update/", true);
            }
            System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + "/Update/");
        }

        public ChromiumWebBrowser browser;
        public void InitBrowser(string URL)
        {
            Cef.Initialize(new CefSettings());
            browser = new ChromiumWebBrowser(URL);
            panelControl2.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {

            //Untuk Ngetest aja
            //ftpClient.PathProgram = @"E:\DEVELOPER\2021 - CTRLSoftware\src\CtrlSoft.App\bin\Debug";

            if (IsVC2015Installed())
            {
                InitBrowser($"https://youtube.com/v/{VideoId}?version=3");
            } else
            {
                if (XtraMessageBox.Show($"Mohon untuk menginstall <b>[Microsoft Visual C++ 2015 (x86)]</b>," + Environment.NewLine +
                                        $"Silahkan download <href=https://ctrlsoft.id/perquisite/VC_redist.x86.exe>link berikut.</href>", 
                                        "WARNING", 
                                        MessageBoxButtons.YesNo, 
                                        MessageBoxIcon.Warning, 
                                        DevExpress.Utils.DefaultBoolean.True) == DialogResult.Yes)
                {
                    Process.Start($"https://ctrlsoft.id/perquisite/VC_redist.x86.exe");
                    Application.Exit();
                    Environment.Exit(1);
                };
            }
            bool createShortcut = false;
            if (ftpClient.PathProgram.Equals(""))
            {
                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {
                    dialog.ShowNewFolderButton = true;
                    dialog.Description = "Pilih Folder CtrlSoft.App ditempatkan.";
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        ftpClient.PathProgram = dialog.SelectedPath;
                        createShortcut = true;
                    }
                }
                if (ftpClient.PathProgram.Equals(""))
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Pilih directory dahulu untuk menginstall CtrlSoft.App, Terima Kasih!");

                    Application.Exit();
                }
            }

            labelControl2.Text = "Path : " + ftpClient.PathProgram;
            if (ftpClient.PathProgram.Length >= 1 &&
                System.IO.Directory.Exists(ftpClient.PathProgram))
            {
                //webBrowser.Navigate($"https://youtube.com/v/{VideoId}");
                //webBrowser.Navigate(@"http://google.com");
                //webBrowser.Navigate("https://www.youtube.com/watch?v=p6qIGeBInoA");
                CreateStruktur();
                bool sukses = ftpClient.DownloadUpdate(progressBarControl1, lbFile);

                if (sukses)
                {
                    var matchingProcesses = new List<Process>();
                    foreach (var item in Process.GetProcesses())
                    {
                        if (item.ProcessName.Equals("CtrlSoft.App"))
                        {
                            if (String.Compare(item.MainModule.FileName, ftpClient.PathProgram + "\\CtrlSoft.App.exe", true) == 0)
                            {
                                matchingProcesses.Add(item);
                                break;
                            }
                        }
                        Application.DoEvents();
                    }
                    foreach (var item in matchingProcesses)
                    {
                        item.Kill();
                        Application.DoEvents();
                    }

                    System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(Environment.CurrentDirectory + "\\Update\\");
                    Int32 iPos = 0;
                    foreach (var item in directoryInfo.GetFiles())
                    {
                        iPos += 1;
                        progressBarControl1.EditValue = (double)((double) iPos / (double) directoryInfo.GetFiles().Length) * 100.0;
                        lbFile.Text = "File : " + item.Name;
                        Application.DoEvents();

                        System.IO.File.Copy(item.FullName, ftpClient.PathProgram + "\\" + item.Name, true);
                    }
                    iPos = 0;
                    foreach (var item in directoryInfo.GetDirectories())
                    {
                        iPos += 1;
                        progressBarControl1.EditValue = (double)((double) iPos / (double) directoryInfo.GetDirectories().Length) * 100.0;
                        Application.DoEvents();

                        //System.IO.Directory.Move(item.FullName, ftpClient.PathProgram + "\\" + item.Name);

                        if (!System.IO.Directory.Exists(ftpClient.PathProgram + "\\" + item.Name))
                        {
                            System.IO.Directory.CreateDirectory(ftpClient.PathProgram + "\\" + item.Name);
                        }
                        copy(item.FullName, ftpClient.PathProgram + "\\" + item.Name);
                    }
                }
            }

            if (System.IO.File.Exists(ftpClient.PathProgram + "\\CtrlSoft.App.exe"))
            {
                //using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(Environment.CurrentDirectory + "\\Update\\Update.Bat"))
                //{
                //    streamWriter.WriteLine("" + ftpClient.PathProgram + "\\CtrlSoft.App.exe" + "");
                //    streamWriter.Flush();
                //}

                if (createShortcut)
                {
                    CreateShortcut("CTrlSoft.App", ftpClient.PathProgram + "\\CtrlSoft.App.exe", ftpClient.PathProgram);

                    //Check MSSQLLocalDB
                    if (Environment.Is64BitOperatingSystem)
                    {
                        if (XtraMessageBox.Show($"Mohon untuk menginstall database engine <b>[MSSQLLocalDB 2016 (x64)]</b>," + Environment.NewLine +
                                                $"Silahkan download <href=https://ctrlsoft.id/perquisite/SqlLocalDB_2016_x64.msi>link berikut.</href>",
                                        "WARNING",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Warning,
                                        DevExpress.Utils.DefaultBoolean.True) == DialogResult.Yes)
                        {
                            Process.Start($"https://ctrlsoft.id/perquisite/SqlLocalDB_2016_x64.msi");
                        };
                    }
                    else
                    {
                        if (XtraMessageBox.Show($"Mohon untuk menginstall database engine <b>[MSSQLLocalDB 2016 (x86)]</b>," + Environment.NewLine +
                                                $"Silahkan download <href=https://ctrlsoft.id/perquisite/SqlLocalDB_2016_x86.msi>link berikut.</href>",
                                        "WARNING",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Warning,
                                        DevExpress.Utils.DefaultBoolean.True) == DialogResult.Yes)
                        {
                            Process.Start($"https://ctrlsoft.id/perquisite/SqlLocalDB_2016_x86.msi");
                        };
                    }
                }

                System.Diagnostics.ProcessStartInfo newP = new System.Diagnostics.ProcessStartInfo();
                newP.Verb = "Open";
                newP.WindowStyle = ProcessWindowStyle.Normal;
                newP.FileName = ftpClient.PathProgram + "\\CtrlSoft.App.exe";
                newP.UseShellExecute = true;
                newP.WorkingDirectory = ftpClient.PathProgram;

                Process.Start(newP);
            }

            Application.Exit();
        }

        private void CreateShortcut(string linkName, 
                                    string fileLocation,
                                    string workingDirectory)
        {
            try
            {
                object shDesktop = (object)"Desktop";
                WshShell shell = new WshShell();
                string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + "\\" + linkName + ".lnk";
                if (System.IO.File.Exists(shortcutAddress))
                {
                    System.IO.File.Delete(shortcutAddress);
                }
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
                shortcut.Description = "New shortcut for a CTrlSoft.App";
                shortcut.Hotkey = "Ctrl+Shift+N";
                shortcut.TargetPath = fileLocation;
                shortcut.WorkingDirectory = workingDirectory;
                shortcut.Save();

            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void copy(string dirSource, string dirDestination)
        {
            System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(dirSource);
            Int32 iPos = 0;
            foreach (var item in directoryInfo.GetFiles())
            {
                iPos += 1;
                progressBarControl1.EditValue = (double)((double) iPos / (double) directoryInfo.GetFiles().Length) * 100.0;
                lbFile.Text = "File : " + item.Name;
                Application.DoEvents();

                System.IO.File.Copy(item.FullName, dirDestination + "\\" + item.Name, true);
            }
            iPos = 0;
            foreach (var item in directoryInfo.GetDirectories())
            {
                iPos += 1;
                progressBarControl1.EditValue = (double)((double) iPos / (double) directoryInfo.GetDirectories().Length) * 100.0;
                Application.DoEvents();

                if (!System.IO.Directory.Exists(dirDestination + "\\" + item.Name))
                {
                    System.IO.Directory.CreateDirectory(dirDestination + "\\" + item.Name);
                }
                copy(item.FullName, dirDestination + "\\" + item.Name);
            }
        }

        private void frmMain_FormClossing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
            Environment.Exit(1);
        }

        public static bool IsVC2015Installed()
        {
            string dependenciesPath = @"SOFTWARE\Classes\Installer\Dependencies";

            using (RegistryKey dependencies = Registry.LocalMachine.OpenSubKey(dependenciesPath))
            {
                if (dependencies == null) return false;

                foreach (string subKeyName in dependencies.GetSubKeyNames().Where(n => !n.ToLower().Contains("dotnet") && !n.ToLower().Contains("microsoft")))
                {
                    using (RegistryKey subDir = Registry.LocalMachine.OpenSubKey(dependenciesPath + "\\" + subKeyName))
                    {
                        var value = subDir.GetValue("DisplayName")?.ToString() ?? null;
                        if (string.IsNullOrEmpty(value))
                        {
                            continue;
                        }
                        if (Environment.Is64BitOperatingSystem)
                        {
                            if (Regex.IsMatch(value, @"C\+\+ 2015.*\((x64|x86)\)"))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (Regex.IsMatch(value, @"C\+\+ 2015.*\(x86\)"))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}

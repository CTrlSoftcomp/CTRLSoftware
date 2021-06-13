using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CtrlSoft.AppUpdate
{
    public partial class frmMain : DevExpress.XtraEditors.XtraForm
    {
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

        private void frmMain_Shown(object sender, EventArgs e)
        {
            //Untuk Ngetest aja
            //ftpClient.PathProgram = @"E:\DEVELOPER\2021 - CTRLSoftware\src\CtrlSoft.App\bin\Debug";

            labelControl2.Text = "Path : " + ftpClient.PathProgram;
            if (ftpClient.PathProgram.Length >= 1 &&
                System.IO.Directory.Exists(ftpClient.PathProgram))
            {
                CreateStruktur();
                bool sukses = ftpClient.DownloadUpdate(progressBarControl1);

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

        private void copy(string dirSource, string dirDestination)
        {
            System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(dirSource);
            Int32 iPos = 0;
            foreach (var item in directoryInfo.GetFiles())
            {
                iPos += 1;
                progressBarControl1.EditValue = (double)((double) iPos / (double) directoryInfo.GetFiles().Length) * 100.0;
                Application.DoEvents();

                System.IO.File.Copy(item.FullName, dirDestination + "\\" + item.Name, true);
            }
            iPos = 0;
            foreach (var item in directoryInfo.GetDirectories())
            {
                iPos += 1;
                progressBarControl1.EditValue = (double)((double) iPos / (double) directoryInfo.GetDirectories().Length) * 100.0;
                Application.DoEvents();
                
                copy(item.FullName, dirDestination + "\\" + item.Name);
            }
        }
    }
}

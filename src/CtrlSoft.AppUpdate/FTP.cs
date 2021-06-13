using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace CtrlSoft.AppUpdate
{
    class FTP
    {
        public FTP()
        {
            this.PathProgram = "";
            this.FtpServer = "ftp://ftp.ctrlsoft.id";
            this.FtpUserID = "CTrlSoftcomp@licenseapi.ctrlsoft.id";
            this.FtpPassword = "Ell1t3s3rv";
            this.FtpPort = 21;
        }
        public string PathProgram { get; set; }
        public string FtpServer { get; set; }
        public string FtpUserID { get; set; }
        public string FtpPassword { get; set; }
        public int FtpPort { get; set; }

        public bool DownloadUpdate(DevExpress.XtraEditors.ProgressBarControl progress)
        {
            try
            {
                NetworkCredential credentials = new NetworkCredential(this.FtpUserID, this.FtpPassword);
                string url = FtpServer;
                return DownloadFtpDirectory(progress, url, credentials, Environment.CurrentDirectory + "/Update/");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Error : " + ex.Message);
                return false;
            }
        }

        private bool DownloadFtpDirectory(DevExpress.XtraEditors.ProgressBarControl progress,
            string url, 
            NetworkCredential credentials, 
            string localPath)
        {
            try
            {
                FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(url);
                listRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                listRequest.Credentials = credentials;

                List<string> lines = new List<string>();

                using (FtpWebResponse listResponse = (FtpWebResponse)listRequest.GetResponse())
                using (Stream listStream = listResponse.GetResponseStream())
                using (StreamReader listReader = new StreamReader(listStream))
                {
                    while (!listReader.EndOfStream)
                    {
                        string line = listReader.ReadLine();
                        string[] tokens = line.Split(new[] { ' ' }, 9, StringSplitOptions.RemoveEmptyEntries);
                        string name = tokens[8];

                        if (!(name.Equals(".") || name.Equals("..") || name.Equals(".ftpquota")))
                        {
                            lines.Add(line);
                        }
                        Application.DoEvents();
                    }
                }

                Int32 iPos = 0;
                foreach (string line in lines)
                {
                    iPos += 1;
                    progress.EditValue = (double) ((double) iPos / (double) lines.Count) * 100.0;
                    Application.DoEvents();

                    string[] tokens =
                        line.Split(new[] { ' ' }, 9, StringSplitOptions.RemoveEmptyEntries);
                    string name = tokens[8];
                    if (!(name.Equals(".") || name.Equals("..") || name.Equals(".ftpquota")))
                    {
                        string permissions = tokens[0];

                        string localFilePath = Path.Combine(localPath, name);
                        string fileUrl = url + "/" + name;

                        if (permissions[0] == 'd')
                        {
                            if (!Directory.Exists(localFilePath))
                            {
                                Directory.CreateDirectory(localFilePath);
                            }

                            DownloadFtpDirectory(progress, fileUrl + "/", credentials, localFilePath);
                        }
                        else
                        {
                            FtpWebRequest downloadRequest = (FtpWebRequest)WebRequest.Create(fileUrl);
                            downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                            downloadRequest.Credentials = credentials;

                            using (FtpWebResponse downloadResponse =
                                      (FtpWebResponse)downloadRequest.GetResponse())
                            using (Stream sourceStream = downloadResponse.GetResponseStream())
                            using (Stream targetStream = File.Create(localFilePath))
                            {
                                byte[] buffer = new byte[10240];
                                int read;
                                while ((read = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    targetStream.Write(buffer, 0, read);
                                    Application.DoEvents();
                                }
                            }
                        }
                    }
                    Application.DoEvents();
                }

                return true;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Error : " + ex.Message);
                return false;
            }
        }
    }
}

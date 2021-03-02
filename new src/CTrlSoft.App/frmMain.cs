using Syncfusion.WinForms.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CTrlSoft.Repository;

namespace CTrlSoft.App
{
    public partial class frmMain : SfForm
    {
        public frmMain()
        {
            InitializeComponent();

            //Default dari Syncfusion di Disable
            //this.Style.TitleBar.Height = 26;
            //this.Style.TitleBar.BackColor = Color.White;
            //this.Style.TitleBar.IconBackColor = Color.FromArgb(15, 161, 212);
            //this.BackColor = Color.White;
            //this.Style.TitleBar.ForeColor = ColorTranslator.FromHtml("#343434");
            //this.Style.TitleBar.CloseButtonForeColor = Color.DarkGray;
            //this.Style.TitleBar.MaximizeButtonForeColor = Color.DarkGray;
            //this.Style.TitleBar.MinimizeButtonForeColor = Color.DarkGray;
            //this.Style.TitleBar.HelpButtonForeColor = Color.DarkGray;
            //this.Style.TitleBar.IconHorizontalAlignment = HorizontalAlignment.Left;
            //this.Style.TitleBar.Font = this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.Style.TitleBar.TextHorizontalAlignment = HorizontalAlignment.Center;
            //this.Style.TitleBar.TextVerticalAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Center;


            initComponent();
            initHandler();
        }

        private static frmDashboard frmDashboard = new frmDashboard();
        private void initComponent()
        {
            
        }

        private void initHandler()
        {
            Shown += frmMain_Shown;
        }

        private void menuLoginOut()
        {
            if (Repository.Utils.UserLogin != null || Repository.Utils.UserLogin.id <= 0)
            {
                using (frmLogin frm = new frmLogin())
                {
                    try
                    {
                        if (frm.ShowDialog(this) == DialogResult.OK)
                        {

                        }
                    } catch (Exception ex)
                    {
                        RepLogger.ShowMessage(this, "ERR : " + ex.Message, MessageBoxIcon.Error, MessageBoxButtons.OK);
                    }
                }
            } else
            {

            }
        }

        private void frmMain_Shown(Object sender, EventArgs e)
        {
            menuLoginOut();
        }

        private void imageStreamer1_Click(object sender, EventArgs e)
        {

        }
    }
}

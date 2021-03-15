using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.Windows.Forms.Grid.Grouping;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms;
using CTrlSoft.Repository;
using CTrlSoft.SApp.Repository;
using MessageBox = CTrlSoft.SApp.Repository.MessageBox;

namespace CTrlSoft.SApp
{
   public partial class frmMain : RibbonForm
    {
        private static frmDashboard frmDashboard = new frmDashboard();
        public frmMain()
        {
            InitializeComponent();


            initComponent();
            initHandler();
        }

        private void initComponent()
        {

        }

        private void initHandler()
        {
            Shown += frmMain_Shown;
            toolStripButton1.Click += menu_action;
        }

        private void menuLoginOut()
        {
            if (Constant.Public.UserLogin != null || Constant.Public.UserLogin.id <= 0)
            {
                using (frmLogin frm = new frmLogin())
                {
                    try
                    {
                        if (frm.ShowDialog(this) == DialogResult.OK)
                        {
                            //Gak Ngapa-ngapain
                        }
                    }
                    catch (Exception ex)
                    {
                        RepLogger.ShowMessage(this, "ERR : " + ex.Message, MessageBoxIcon.Error, MessageBoxButtons.OK);
                    }
                }
            }
            else
            {

            }
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            frmDashboard.MdiParent = this;
            frmDashboard.Show();

            menuLoginOut();
        }

        private void menu_action(object sender, EventArgs e)
        {
            frmSatuan x = new frmSatuan();
            x.MdiParent = this;
            x.Show();
            x.Focus();
        }

        private void _formClossing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Tampil(this, 
                "Ingin keluar applikasi?" + Environment.NewLine + "Jika ingin keluar maka seluruh transaksi yang terpending akan hilang, ingin meneruskannya?",
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question, 
                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                frmDashboard.Dispose();
                e.Cancel = false;
            }
        }
    }
}

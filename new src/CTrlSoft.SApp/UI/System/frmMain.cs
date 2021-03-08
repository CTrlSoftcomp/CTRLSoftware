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

        private void frmMain_Shown(Object sender, EventArgs e)
        {
            frmDashboard.MdiParent = this;
            frmDashboard.Show();

            menuLoginOut();
        }
    }
}

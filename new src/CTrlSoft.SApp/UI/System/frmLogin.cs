using Syncfusion.WinForms.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CTrlSoft.Repository;
using CTrlSoft.BLL.System.User;

namespace CTrlSoft.SApp
{
    public partial class frmLogin : SfForm
    {
        private RepUser rep = new RepUser(Properties.Settings.Default.MyConn);
        public frmLogin()
        {
            InitializeComponent();

            this.Style.TitleBar.Height = 26;
            this.Style.TitleBar.BackColor = Color.White;
            this.Style.TitleBar.IconBackColor = Color.FromArgb(15, 161, 212);
            this.BackColor = Color.White;
            this.Style.TitleBar.ForeColor = ColorTranslator.FromHtml("#343434");
            this.Style.TitleBar.CloseButtonForeColor = Color.DarkGray;
            this.Style.TitleBar.MaximizeButtonForeColor = Color.DarkGray;
            this.Style.TitleBar.MinimizeButtonForeColor = Color.DarkGray;
            this.Style.TitleBar.HelpButtonForeColor = Color.DarkGray;
            this.Style.TitleBar.IconHorizontalAlignment = HorizontalAlignment.Left;
            this.Style.TitleBar.Font = this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Style.TitleBar.TextHorizontalAlignment = HorizontalAlignment.Center;
            this.Style.TitleBar.TextVerticalAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Center;
        }

        private void btnlogin_click(Object sender, EventArgs e)
        {
            //For Default SYSADM->SYSADM@12345
            if (txtUserID.Text.Length == 0)
            {
                RepLogger.ShowMessage(this, "User ID harus diisi!", MessageBoxIcon.Stop, MessageBoxButtons.OK);
                txtUserID.Focus();
            }
            else if (txtPassword.Text.Length < Repository.Utils.MinLenghtPassword)
            {
                RepLogger.ShowMessage(this, "Password minimal harus " + Repository.Utils.MinLenghtPassword + " digit!", MessageBoxIcon.Stop, MessageBoxButtons.OK);
                txtPassword.Focus();
            }
            else
            {
                var hasil = rep.GetLogin(txtUserID.Text, txtPassword.Text);
                if (hasil.JSONResult)
                {
                    Repository.Utils.UserLogin = (Models.Dto.MUser)hasil.JSONValue;
                    RepLogger.ShowMessage(this, "Selamat datang " + Repository.Utils.UserLogin.nama + ", selamat beraktifitas.", MessageBoxIcon.Information, MessageBoxButtons.OK);

                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    RepLogger.ShowMessage(this, hasil.JSONMessage, MessageBoxIcon.Warning, MessageBoxButtons.OK);
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void txtUserID_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    txtPassword.Focus();
                    break;
                default:
                    break;
            }
        }

        private void txtPassword_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    btnLogin.Focus();
                    break;
                default:
                    break;
            }
        }
    }
}

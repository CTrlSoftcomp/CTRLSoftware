
namespace CTrlSoft.App
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.sfSkinManager1 = new Syncfusion.WinForms.Controls.SfSkinManager(this.components);
            this.SuspendLayout();
            // 
            // sfSkinManager1
            // 
            this.sfSkinManager1.Component = this;
            this.sfSkinManager1.Controls = null;
            this.sfSkinManager1.ThemeName = "Office2016Colorful";
            this.sfSkinManager1.VisualTheme = Syncfusion.Windows.Forms.VisualTheme.Metro;
            // 
            // frmMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(1029, 526);
            this.Name = "frmMain";
            this.ShowIcon = false;
            this.Style.MdiChild.CloseButtonForeColor = System.Drawing.Color.Black;
            this.Style.MdiChild.CloseButtonPressedForeColor = System.Drawing.Color.Black;
            this.Style.MdiChild.HelpButtonForeColor = System.Drawing.Color.Black;
            this.Style.MdiChild.IconBackColor = System.Drawing.Color.Black;
            this.Style.MdiChild.IconHorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.Style.MdiChild.IconVerticalAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Center;
            this.Style.MdiChild.MaximizeButtonForeColor = System.Drawing.Color.Black;
            this.Style.MdiChild.MinimizeButtonForeColor = System.Drawing.Color.Black;
            this.Style.MdiChild.TitleBarBackColor = System.Drawing.Color.White;
            this.Style.TitleBar.BackColor = System.Drawing.Color.Black;
            this.Style.TitleBar.CloseButtonForeColor = System.Drawing.Color.White;
            this.Style.TitleBar.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Style.TitleBar.ForeColor = System.Drawing.Color.White;
            this.Style.TitleBar.Height = 75;
            this.Style.TitleBar.HelpButtonForeColor = System.Drawing.Color.White;
            this.Style.TitleBar.MaximizeButtonForeColor = System.Drawing.Color.White;
            this.Style.TitleBar.MinimizeButtonForeColor = System.Drawing.Color.White;
            this.Style.TitleBar.TextHorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.Text = "CTrl Software";
            this.ThemeName = "Office2016Colorful";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.WinForms.Controls.SfSkinManager sfSkinManager1;
    }
}
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports System.IO
Imports System.Diagnostics
Imports DevExpress.XtraEditors
Imports DevExpress.Skins
Imports DevExpress.XtraBars.Ribbon
Imports DevExpress.XtraBars.Ribbon.Gallery
Imports DevExpress.Utils.Drawing
Imports DevExpress.Utils
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.LookAndFeel
Imports CtrlSoft.Ini
Imports CtrlSoft.Utils
Imports DevExpress.XtraBars

Public Class frmMain
    Inherits DevExpress.XtraBars.Ribbon.RibbonForm
    Private DashBoard As New frmDashboard

    Public Sub New()
        InitializeComponent()
        InitSkinGallery()
        UserLookAndFeel.Default.SetSkinStyle(Ini.BacaIni("Application", "Skins", "Office 2010 Blue"))
    End Sub
#Region "SkinGallery"
    Private Sub InitSkinGallery()
        DevExpress.XtraBars.Helpers.SkinHelper.InitSkinGallery(rgbiSkins, True)
    End Sub
#End Region

    Private Sub frmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If XtraMessageBox.Show("Yakin anda ingin keluar dari aplikasi?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                e.Cancel = True
            Else
                Ini.TulisIni("Application", "Skins", defaultLookAndFeel1.LookAndFeel.SkinName)
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, "Admin Says", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub barSetting_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles barSetting.ItemClick
        Using frm As New frmSettingDB
            Try
                If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    StrKonSQL = "Data Source=" & BacaIni("DBConfig", "Server", "localhost") & _
                                ";initial Catalog=" & BacaIni("DBConfig", "Database", "dbpos") & _
                                ";User ID=" & BacaIni("DBConfig", "Username", "sa") & _
                                ";Password=" & BacaIni("DBConfig", "Password", "Sg1") & _
                                ";Connect Timeout=" & BacaIni("DBConfig", "Timeout", "15")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, "Admin Says", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Using dlg As New WaitDialogForm("Initialize component", NamaAplikasi)
            Try
                Me.Text = "CTRL Software - [Version : " & Application.ProductVersion.ToString & "]"
                'RibbonControl.SelectedPage = RibbonPage1

                Application.DoEvents()
                DashBoard.MdiParent = Me
                DashBoard.Show()
                DashBoard.Focus()

                DisableMenu()
                UserLogin = New Model.User
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, "Admin Says", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub DisableMenu()
        Try
            For Each frm In Me.MdiChildren
                If Not TypeOf frm Is frmDashboard Then
                    frm.Close()
                End If
            Next

            Dim pages As RibbonPageCollection = RibbonControl.Pages
            For a As Integer = 0 To pages.Count - 1
                Dim page As RibbonPage = pages(a)
                If NullToStr(page.Tag) = NamaAplikasi Then
                    Dim groups As DevExpress.XtraBars.Ribbon.RibbonPageGroupCollection = page.Groups
                    For Each group As RibbonPageGroup In groups
                        Dim itemLinks As RibbonPageGroupItemLinkCollection = group.ItemLinks
                        For i As Integer = 0 To itemLinks.Count - 1
                            Dim item As BarItem = TryCast(itemLinks(i).Item, BarItem)
                            Ribbon.Items.Remove(item)
                        Next i
                    Next group
                    page.Groups.Clear()
                End If
            Next a
            RibbonControl.Pages.Clear()

            barSetting.Enabled = True
            barLoginOut.Enabled = True
            barExit.Enabled = True
            RibbonPageCategory1.Visible = False
            barLoginOut.Caption = "Login"

            UserLogin = New Model.User()
            Timer1.Enabled = False

            barStaticUID.Caption = "User : (none)"
            barStaticJam.Caption = "Tanggal System : (none)"
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, "Admin Says", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub frmMain_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        barLoginOut.PerformClick()
    End Sub

    Private Sub barExit_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles barExit.ItemClick
        Me.Close()
        DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub barLoginOut_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles barLoginOut.ItemClick
        mnLoginOut(UserLogin.HasLogin)
    End Sub

    Private Sub mnLoginOut(ByVal IsLogin As Boolean)
        If IsLogin Then
            DisableMenu()
        Else
            Using frm As New frmLogin
                Try
                    If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        InitMenu()
                        barSetting.Enabled = True
                        barLoginOut.Enabled = True
                        barExit.Enabled = True
                    End If
                Catch ex As Exception
                    XtraMessageBox.Show(ex.Message, "Admin Says", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End If
    End Sub

    Private Sub InitMenu()
        Try
            'Dim pageCategori As New RibbonPageCategory With {.Name = NamaAplikasi, .Tag = .Name, .Visible = True}
            For Each Menu As Model.Menu In UserLogin.Menu
                Dim rbPage As New RibbonPage
                rbPage.Visible = Menu.Visible
                rbPage.Name = Menu.Name
                rbPage.Text = Menu.Caption
                rbPage.Tag = NamaAplikasi

                Dim rbPageGroup As New RibbonPageGroup
                rbPageGroup.Visible = Menu.Visible
                rbPageGroup.Name = Menu.Name
                rbPageGroup.Text = Menu.Caption
                rbPageGroup.Tag = NamaAplikasi

                For Each SubMenu As Model.SubMenu In Menu.SubMenu
                    Dim rbItem As New BarButtonItem
                    rbItem.Visibility = IIf(SubMenu.Visible, BarItemVisibility.Always, BarItemVisibility.Never)
                    rbItem.Name = SubMenu.Name
                    rbItem.Caption = SubMenu.Caption
                    rbItem.ImageIndex = Menu.NoID - 1
                    rbItem.RibbonStyle = IIf(SubMenu.BigMenu, RibbonItemStyles.Large, RibbonItemStyles.Default)
                    rbItem.Tag = NamaAplikasi

                    'RibbonControl.Items.Add(rbItem)
                    rbPageGroup.ItemLinks.Add(rbItem, SubMenu.BeginGroup)
                Next
                rbPage.Groups.Add(rbPageGroup)
                'pageCategori.Pages.Add(rbPage)
                RibbonControl.Pages.Add(rbPage)
            Next
            'RibbonControl.PageCategories.Add(pageCategori)
            RibbonPageCategory1.Visible = IIf(UserLogin.Supervisor, True, False)
            barLoginOut.Caption = "Logout"

            UserLogin.TanggalSystem = Repository.RepMenu.GetTimeServer()
            TimeZoneInformation.TimeZoneFunctionality.SetTime(System.TimeZone.CurrentTimeZone.ToLocalTime(UserLogin.TanggalSystem))
            barStaticUID.Caption = "User : [" & UserLogin.Nama & "]"
            barStaticJam.Caption = "Tanggal System : " & UserLogin.TanggalSystem.ToString("dd/MM/yyyy HH:mm")

            Timer1.Interval = 5000
            Timer1.Enabled = True
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, "Admin Says", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Dim SQL As String = ""
        Try
            UserLogin.TanggalSystem = Now()
            barStaticJam.Caption = "Tanggal System : " & UserLogin.TanggalSystem.ToString("dd/MM/yyyy HH:mm")
            Application.DoEvents()
        Catch ex As Exception

        End Try
    End Sub
End Class
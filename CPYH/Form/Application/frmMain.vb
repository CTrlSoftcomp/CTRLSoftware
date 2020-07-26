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

            'Dim pages As RibbonPageCollection = RibbonControl.Pages
            'For a As Integer = 0 To pages.Count - 1
            '    Dim page As RibbonPage = pages(a)
            '    If NullToStr(page.Tag) = NamaAplikasi Then
            '        Dim groups As DevExpress.XtraBars.Ribbon.RibbonPageGroupCollection = page.Groups
            '        For Each group As RibbonPageGroup In groups
            '            Dim itemLinks As RibbonPageGroupItemLinkCollection = group.ItemLinks
            '            For i As Integer = 0 To itemLinks.Count - 1
            '                Dim item As BarItem = TryCast(itemLinks(i).Item, BarItem)
            '                Ribbon.Items.Remove(item)
            '            Next i
            '        Next group
            '        page.Groups.Clear()
            '    End If
            'Next a
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
                    rbItem.ImageIndex = IIf(SubMenu.BigMenu, -1, Menu.NoID - 1)
                    rbItem.LargeImageIndex = IIf(SubMenu.BigMenu, Menu.NoID - 1, -1)
                    rbItem.RibbonStyle = IIf(SubMenu.BigMenu, RibbonItemStyles.Large, RibbonItemStyles.Default)
                    
                    AddHandler rbItem.ItemClick, AddressOf rbItem_ItemClick
                    rbItem.Tag = NamaAplikasi

                    rbPageGroup.ItemLinks.Add(rbItem, SubMenu.BeginGroup)
                    RibbonControl.Items.Add(rbItem)
                Next
                rbPage.Groups.Add(rbPageGroup)
                
                RibbonControl.Pages.Add(rbPage)
            Next
            'RibbonControl.PageCategories.Add(pageCategori)
            RibbonControl.Images = ImageCollectionSmall
            RibbonControl.LargeImages = ImageCollectionLarge

            RibbonPageCategory1.Visible = IIf(UserLogin.Supervisor, True, False)
            barLoginOut.Caption = "Logout"
            
            UserLogin.TanggalSystem = Repository.RepSQLServer.GetTimeServer()
            TimeZoneInformation.TimeZoneFunctionality.SetTime(System.TimeZone.CurrentTimeZone.ToLocalTime(UserLogin.TanggalSystem))
            UserLogin.TanggalSystem = Now()
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

    Private Sub barSettingPerusahaan_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles barSettingPerusahaan.ItemClick
        Using frm As New frmSettingPerusahaan
            Try
                If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then

                End If
            Catch ex As Exception

            End Try
        End Using
    End Sub

    Private Sub rbItem_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        Select Case NullToStr(e.Item.Name)
            Case "mnMutasiStok"
                Dim x As frmLaporanMutasiSaldoStok = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmLaporanMutasiSaldoStok Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmLaporanMutasiSaldoStok(e.Item.Name, e.Item.Caption, UserLogin.TanggalSystem)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnSaldoStok"
                Dim x As frmLaporanSaldoStok = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmLaporanSaldoStok Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmLaporanSaldoStok(e.Item.Name, e.Item.Caption, UserLogin.TanggalSystem)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnKartuStok"
                Dim x As frmLaporanKartuStok = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmLaporanKartuStok Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmLaporanKartuStok(e.Item.Caption, -1, -1, UserLogin.TanggalSystem.AddDays((-1 * UserLogin.TanggalSystem.Day) + 1), UserLogin.TanggalSystem)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnReturBeli"
                Dim x As frmDaftarTransaksi = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarTransaksi AndAlso _
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = modMain.FormName.DaftarReturPembelian.ToString Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarTransaksi(modMain.FormName.DaftarReturPembelian, _
                                            e.Item.Caption)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnJual"
                Dim x As frmDaftarTransaksi = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarTransaksi AndAlso _
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = modMain.FormName.DaftarPenjualan.ToString Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarTransaksi(modMain.FormName.DaftarPenjualan, _
                                            e.Item.Caption)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnReturJual"
                Dim x As frmDaftarTransaksi = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarTransaksi AndAlso _
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = modMain.FormName.DaftarReturPenjualan.ToString Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarTransaksi(modMain.FormName.DaftarReturPenjualan, _
                                            e.Item.Caption)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnMutasiGudang"
                Dim x As frmDaftarTransaksi = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarTransaksi AndAlso _
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = modMain.FormName.DaftarMutasiGudang.ToString Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarTransaksi(modMain.FormName.DaftarMutasiGudang, _
                                            e.Item.Caption)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnPenyesuaianMasuk"
                Dim x As frmDaftarTransaksi = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarTransaksi AndAlso _
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = modMain.FormName.DaftarPenyesuaian.ToString Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarTransaksi(modMain.FormName.DaftarPenyesuaian, _
                                            e.Item.Caption)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnPenyesuaianKeluar"
                Dim x As frmDaftarTransaksi = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarTransaksi AndAlso _
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = modMain.FormName.DaftarPemakaian.ToString Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarTransaksi(modMain.FormName.DaftarPemakaian, _
                                            e.Item.Caption)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnBeli"
                Dim x As frmDaftarTransaksi = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarTransaksi AndAlso _
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = modMain.FormName.DaftarPembelian.ToString Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarTransaksi(modMain.FormName.DaftarPembelian, _
                                            e.Item.Caption)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnPO"
                Dim x As frmDaftarTransaksi = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarTransaksi AndAlso _
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = modMain.FormName.DaftarPO.ToString Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarTransaksi(modMain.FormName.DaftarPO, _
                                            e.Item.Caption)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnBarang"
                Dim x As frmDaftarBarang = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarBarang Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarBarang(e.Item.Name, _
                                            e.Item.Caption)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnKontak"
                Dim x As frmDaftarKontak = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarKontak Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarKontak(e.Item.Name, _
                                            e.Item.Caption)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnKategori"
                Dim x As frmDaftarMaster = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarMaster AndAlso frm.Name = e.Item.Name Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarMaster(e.Item.Name, _
                                            e.Item.Caption, _
                                            "MKategori", _
                                            "SELECT MKategori.NoID, MKategori.Kode, MKategori.Nama, MKategori.IsActive Aktif, MParent.Kode + '-' + MParent.Nama KategoriUtama" & vbCrLf & _
                                            "FROM MKategori" & vbCrLf & _
                                            "LEFT JOIN MKategori MParent ON MParent.NoID=MKategori.IDParent ")
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnSatuan"
                Dim x As frmDaftarMaster = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarMaster AndAlso frm.Name = e.Item.Name Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarMaster(e.Item.Name, _
                                            e.Item.Caption, _
                                            "MSatuan", _
                                            "SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama, MSatuan.Konversi, MSatuan.IsActive Aktif " & vbCrLf & _
                                            "FROM MSatuan")
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnMerk"
                Dim x As frmDaftarMaster = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarMaster AndAlso frm.Name = e.Item.Name Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarMaster(e.Item.Name, _
                                            e.Item.Caption, _
                                            "MMerk", _
                                            "SELECT MMerk.NoID, MMerk.Kode, MMerk.Nama, MMerk.IsActive Aktif " & vbCrLf & _
                                            "FROM MMerk")
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnGudang"
                Dim x As frmDaftarMaster = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarMaster AndAlso frm.Name = e.Item.Name Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarMaster(e.Item.Name, _
                                            e.Item.Caption, _
                                            "MGudang", _
                                            "SELECT MGudang.NoID, MGudang.Kode, MGudang.Nama, MGudang.IsActive Aktif " & vbCrLf & _
                                            "FROM MGudang")
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case Else
                XtraMessageBox.Show("Menue Durong Onok Boss!!!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End Select
        Application.DoEvents()
    End Sub

    Private Sub barManagementRole_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles barManagementRole.ItemClick
        Dim x As frmDaftar = Nothing
        For Each frm In Me.MdiChildren
            If TypeOf frm Is frmDaftar AndAlso frm.Name = e.Item.Name Then
                x = frm
            End If
        Next
        If x Is Nothing Then
            x = New frmDaftar(e.Item.Name, _
                                    e.Item.Caption, _
                                    "MRole", _
                                    "SELECT MRole.NoID, MRole.Role, MRole.IsSupervisor SPV" & vbCrLf & _
                                    "FROM MRole")
            x.MdiParent = Me
        End If
        x.Show()
        x.Focus()
    End Sub

    Private Sub barManagementUser_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles barManagementUser.ItemClick
        Dim x As frmDaftar = Nothing
        For Each frm In Me.MdiChildren
            If TypeOf frm Is frmDaftar AndAlso frm.Name = e.Item.Name Then
                x = frm
            End If
        Next
        If x Is Nothing Then
            x = New frmDaftar(e.Item.Name, _
                                    e.Item.Caption, _
                                    "MUser", _
                                    "SELECT MUser.NoID, MUser.Kode, MUser.Nama, MRole.[Role], MRole.IsSupervisor SPV " & vbCrLf & _
                                    "FROM MUser" & vbCrLf & _
                                    "LEFT JOIN MRole ON MRole.NoID=MUser.IDRole")
            x.MdiParent = Me
        End If
        x.Show()
        x.Focus()
    End Sub

    Private Sub barEditReport_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles barEditReport.EditValueChanged
        IsEditReport = NullToBool(barEditReport.EditValue)
    End Sub

    Private Sub IsEditReport_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles barEditReport.ItemClick
        IsEditReport = NullToBool(barEditReport.EditValue)
    End Sub

    Private Sub barEditReport_ItemPress(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles barEditReport.ItemPress

    End Sub
End Class
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
Imports CtrlSoft.App.Ini
Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.App.Public
Imports DevExpress.XtraBars
Imports CtrlSoft.Dto.Model
Imports CtrlSoft.Dto.ViewModel
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Tile

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
            If XtraMessageBox.Show("Yakin anda ingin keluar dari aplikasi?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.No Then
                e.Cancel = True
            Else
                Ini.TulisIni("Application", "Skins", defaultLookAndFeel1.LookAndFeel.SkinName)
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, "Admin Says", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub barSetting_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        Using frm As New frmSettingDB
            Try
                If frm.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                    Dim DBSetting = [Public].DBSetting.List.Where(Function(m) m.Default = True).SingleOrDefault
                    If (DBSetting IsNot Nothing) Then
                        StrKonSQL = DBSetting.KoneksiString
                    Else
                        StrKonSQL = "Data Source=" & BacaIni("DBConfig", "Server", "localhost") &
                                    ";initial Catalog=" & BacaIni("DBConfig", "Database", "dbpos") &
                                    ";User ID=" & BacaIni("DBConfig", "Username", "sa") &
                                    ";Password=" & BacaIni("DBConfig", "Password", "Sg1") &
                                    ";Connect Timeout=" & BacaIni("DBConfig", "Timeout", "15")
                    End If
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
                UserLogin = New MUser
                RefreshDataSQLite()
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, "Admin Says", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub DisableMenu()
        Try
            For Each frm In Me.MdiChildren
                If Not TypeOf frm Is frmDashboard Then
                    frm.Dispose()
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

            bvDataManagement.Enabled = True
            bvLogin.Enabled = True
            bvExit.Enabled = True
            RibbonPageCategory1.Visible = False
            bvLogin.Caption = "Login"

            UserLogin = New MUser()
            Timer1.Enabled = False

            barStaticUID.Caption = "User : (none)"
            barStaticJam.Caption = "Tanggal System : (none)"
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, "Admin Says", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub frmMain_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        mnLoginOut(UserLogin.HasLogin)
    End Sub

    Private Sub barExit_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        Me.Close()
        DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub barLoginOut_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        mnLoginOut(UserLogin.HasLogin)
    End Sub

    Private Sub mnLoginOut(ByVal IsLogin As Boolean)
        If IsLogin Then
            DisableMenu()
        Else
            Using frm As New frmLogin
                Try
                    If frm.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                        InitMenu()
                        bvDataManagement.Enabled = False
                        bvLogin.Enabled = True
                        bvExit.Enabled = True
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
            For Each Menu As CtrlSoft.Dto.Model.Menu In UserLogin.Menu
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

                For Each SubMenu As CtrlSoft.Dto.Model.SubMenu In Menu.SubMenu
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
            bvLogin.Caption = "Logout"

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
                If frm.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then

                End If
            Catch ex As Exception

            End Try
        End Using
    End Sub

    Private Sub rbItem_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        Select Case NullToStr(e.Item.Name)
            Case "mnDaftarSAPC"
                Dim x As frmDaftarTransaksi = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarTransaksi AndAlso
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = modMain.FormName.DaftarSaldoAwalPiutang.ToString Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarTransaksi(modMain.FormName.DaftarSaldoAwalPiutang,
                                            e.Item.Caption)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnDaftarSAHS"
                Dim x As frmDaftarTransaksi = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarTransaksi AndAlso
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = modMain.FormName.DaftarSaldoAwalHutang.ToString Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarTransaksi(modMain.FormName.DaftarSaldoAwalHutang,
                                            e.Item.Caption)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnDaftarSA"
                Dim x As frmDaftarTransaksi = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarTransaksi AndAlso
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = modMain.FormName.DaftarSaldoAwalPersediaan.ToString Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarTransaksi(modMain.FormName.DaftarSaldoAwalPersediaan,
                                            e.Item.Caption)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnMutasiStok"
                Dim x As frmLaporanMutasiSaldoStok = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmLaporanMutasiSaldoStok Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmLaporanMutasiSaldoStok(e.Item.Name,
                                                      e.Item.Caption,
                                                      UserLogin.TanggalSystem.AddDays((UserLogin.TanggalSystem.Day * -1) + 1),
                                                      UserLogin.TanggalSystem)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnLapLabaRugi"
                Dim x As frmLaporanLabaKotor = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmLaporanLabaKotor Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmLaporanLabaKotor(e.Item.Name,
                                                e.Item.Caption,
                                                UserLogin.TanggalSystem.AddDays((UserLogin.TanggalSystem.Day * -1) + 1),
                                                UserLogin.TanggalSystem)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnLapJualLaku"
                Dim x As frmLaporanPenjualanPalingLaku = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmLaporanPenjualanPalingLaku Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmLaporanPenjualanPalingLaku(e.Item.Name,
                                                          e.Item.Caption,
                                                          UserLogin.TanggalSystem.AddDays((UserLogin.TanggalSystem.Day * -1) + 1),
                                                          UserLogin.TanggalSystem)
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
                    If TypeOf frm Is frmDaftarTransaksi AndAlso
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = modMain.FormName.DaftarReturPembelian.ToString Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarTransaksi(modMain.FormName.DaftarReturPembelian,
                                            e.Item.Caption)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnJual"
                Dim x As frmDaftarTransaksi = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarTransaksi AndAlso
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = modMain.FormName.DaftarPenjualan.ToString Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarTransaksi(modMain.FormName.DaftarPenjualan,
                                            e.Item.Caption)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnReturJual"
                Dim x As frmDaftarTransaksi = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarTransaksi AndAlso
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = modMain.FormName.DaftarReturPenjualan.ToString Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarTransaksi(modMain.FormName.DaftarReturPenjualan,
                                            e.Item.Caption)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnMutasiGudang"
                Dim x As frmDaftarTransaksi = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarTransaksi AndAlso
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = modMain.FormName.DaftarMutasiGudang.ToString Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarTransaksi(modMain.FormName.DaftarMutasiGudang,
                                            e.Item.Caption)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnStockOpname"
                Dim x As frmDaftarTransaksi = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarTransaksi AndAlso
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = modMain.FormName.DaftarStockOpname.ToString Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarTransaksi(modMain.FormName.DaftarStockOpname,
                                            e.Item.Caption)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnPenyesuaianMasuk"
                Dim x As frmDaftarTransaksi = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarTransaksi AndAlso
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = modMain.FormName.DaftarPenyesuaian.ToString Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarTransaksi(modMain.FormName.DaftarPenyesuaian,
                                            e.Item.Caption)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnPenyesuaianKeluar"
                Dim x As frmDaftarTransaksi = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarTransaksi AndAlso
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = modMain.FormName.DaftarPemakaian.ToString Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarTransaksi(modMain.FormName.DaftarPemakaian,
                                            e.Item.Caption)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnBeli"
                Dim x As frmDaftarTransaksi = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarTransaksi AndAlso
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = modMain.FormName.DaftarPembelian.ToString Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarTransaksi(modMain.FormName.DaftarPembelian,
                                            e.Item.Caption)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnPO"
                Dim x As frmDaftarTransaksi = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarTransaksi AndAlso
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = modMain.FormName.DaftarPO.ToString Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarTransaksi(modMain.FormName.DaftarPO,
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
                    x = New frmDaftarBarang(e.Item.Name,
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
                    x = New frmDaftarKontak(e.Item.Name,
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
                    x = New frmDaftarMaster(e.Item.Name,
                                            e.Item.Caption,
                                            "MKategori",
                                            "SELECT MKategori.NoID, MKategori.Kode, MKategori.Nama, MKategori.IsActive Aktif, MParent.Kode + '-' + MParent.Nama KategoriUtama" & vbCrLf &
                                            "FROM MKategori" & vbCrLf &
                                            "LEFT JOIN MKategori MParent ON MParent.NoID=MKategori.IDParent ",
                                            frmDaftarMaster.TypePrimary.BigInt)
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
                    x = New frmDaftarMaster(e.Item.Name,
                                            e.Item.Caption,
                                            "MSatuan",
                                            "SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama, MSatuan.Konversi, MSatuan.IsActive Aktif " & vbCrLf &
                                            "FROM MSatuan",
                                            frmDaftarMaster.TypePrimary.BigInt)
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
                    x = New frmDaftarMaster(e.Item.Name,
                                            e.Item.Caption,
                                            "MMerk",
                                            "SELECT MMerk.NoID, MMerk.Kode, MMerk.Nama, MMerk.IsActive Aktif " & vbCrLf &
                                            "FROM MMerk",
                                            frmDaftarMaster.TypePrimary.BigInt)
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
                    x = New frmDaftarMaster(e.Item.Name,
                                            e.Item.Caption,
                                            "MGudang",
                                            "SELECT MGudang.NoID, MGudang.Kode, MGudang.Nama, MGudang.IsActive Aktif " & vbCrLf &
                                            "FROM MGudang",
                                            frmDaftarMaster.TypePrimary.BigInt)
                    x.MdiParent = Me
                End If
                x.Show()
                x.Focus()
            Case "mnMaterial"
                Dim x As frmDaftarMaster = Nothing
                For Each frm In Me.MdiChildren
                    If TypeOf frm Is frmDaftarMaster AndAlso frm.Name = e.Item.Name Then
                        x = frm
                    End If
                Next
                If x Is Nothing Then
                    x = New frmDaftarMaster(e.Item.Name,
                                            e.Item.Caption,
                                            "MMaterial",
                                            "SELECT MMaterial.NoID," & vbCrLf &
                                            "MMaterial.Kode," & vbCrLf &
                                            "MMaterial.Nama," & vbCrLf &
                                            "MMaterial.Keterangan," & vbCrLf &
                                            "MMaterial.Konversi," & vbCrLf &
                                            "MMaterial.Qty," & vbCrLf &
                                            "MMaterial.HargaPokok," & vbCrLf &
                                            "MMaterial.Jumlah," & vbCrLf &
                                            "MMaterial.IsActive Aktif," & vbCrLf &
                                            "MBarang.Kode AS KodeBarang," & vbCrLf &
                                            "MBarang.Nama AS NamaBarang," & vbCrLf &
                                            "MSatuan.Kode AS Satuan," & vbCrLf &
                                            "MUser.Nama AS UserEntry" & vbCrLf &
                                            "FROM MMaterial(NOLOCK)" & vbCrLf &
                                            "LEFT JOIN MBarangD(NOLOCK) ON MBarangD.NoID = MMaterial.IDBarangD" & vbCrLf &
                                            "LEFT JOIN MBarang(NOLOCK) ON MBarang.NoID = MMaterial.IDBarang" & vbCrLf &
                                            "LEFT JOIN MSatuan(NOLOCK) ON MSatuan.NoID = MMaterial.IDSatuan" & vbCrLf &
                                            "LEFT JOIN MUser(NOLOCK) ON MUser.NoID = MMaterial.IDUser",
                                            frmDaftarMaster.TypePrimary.Guid)
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
            x = New frmDaftar(e.Item.Name,
                                    e.Item.Caption,
                                    "MRole",
                                    "SELECT MRole.NoID, MRole.Role, MRole.IsSupervisor SPV" & vbCrLf &
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
            x = New frmDaftar(e.Item.Name,
                                    e.Item.Caption,
                                    "MUser",
                                    "SELECT MUser.NoID, MUser.Kode, MUser.Nama, MRole.[Role], MRole.IsSupervisor SPV " & vbCrLf &
                                    "FROM MUser" & vbCrLf &
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

    Private Sub mnUpdate_ItemClick(sender As Object, e As ItemClickEventArgs) Handles mnUpdate.ItemClick
        Dim TempFolder As String = ""
        Try
            TempFolder = My.Computer.FileSystem.CombinePath(My.Computer.FileSystem.SpecialDirectories.Temp.ToString, My.Application.Info.ProductName.ToString)
            If (System.IO.Directory.Exists(TempFolder)) Then
                System.IO.Directory.Delete(TempFolder, True)
            End If

            System.IO.Directory.CreateDirectory(TempFolder)
            If System.IO.File.Exists(Application.StartupPath & "\System\CtrlSoft.AppUpdate.7z") Then
                System.IO.File.Copy(Application.StartupPath & "\System\CtrlSoft.AppUpdate.7z", TempFolder & "\CtrlSoft.AppUpdate.7z")
                Ini.TulisIniPath(TempFolder & "\System.ini", "AppUpdate", "AppPath", Application.StartupPath)
                cls7z.TheExtractor(TempFolder & "\CtrlSoft.AppUpdate.7z", TempFolder)

                If (System.IO.File.Exists(TempFolder + "\CtrlSoft.AppUpdate.exe")) Then
                    Dim newP As System.Diagnostics.ProcessStartInfo = New System.Diagnostics.ProcessStartInfo() With {
                        .Verb = "Open",
                        .WindowStyle = ProcessWindowStyle.Normal,
                        .FileName = TempFolder + "\CtrlSoft.AppUpdate.exe",
                        .WorkingDirectory = TempFolder,
                        .UseShellExecute = True
                        }
                    Process.Start(newP)
                Else
                    XtraMessageBox.Show("App Update belum terdistribusi! Hubungi Support Kami. Terima Kasih", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                XtraMessageBox.Show("App Update belum terdistribusi! Hubungi Support Kami. Terima Kasih", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

#Region "Data SQLite"
    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Private Data As New List(Of DBSettings.MDBSetting)

    Private Sub RefreshDataSQLite()
        Dim List = DBSetting.List.ToList
        Data = List.Where(Function(m) m.Default = False).ToList
        MDBSettingBindingSource.DataSource = Data
        GridControl1.DataSource = MDBSettingBindingSource
        GridControl1.RefreshDataSource()

        Dim Active = List.Where(Function(m) m.Default = True).SingleOrDefault()
        If (Active IsNot Nothing) Then
            lbCurrentDBId.Text = Active.Id
            lbCurrentDBName.Text = Active.Database
            lbCurrentDBServer.Text = Active.Server
        Else
            lbCurrentDBId.Text = "null"
            lbCurrentDBName.Text = "null"
            lbCurrentDBServer.Text = "null"
        End If
    End Sub

    Private Sub TileView1_DataSourceChanged(sender As Object, e As EventArgs) Handles TileView1.DataSourceChanged
        With TileView1
            If System.IO.File.Exists(CtrlSoft.App.Public.SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml") Then
                .RestoreLayoutFromXml(CtrlSoft.App.Public.SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml")
            End If
            For i As Integer = 0 To .Columns.Count - 1
                Select Case .Columns(i).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(i).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(i).DisplayFormat.FormatString = "n2"
                    Case "string"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        .Columns(i).DisplayFormat.FormatString = ""
                    Case "date"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                        .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                    Case "datetime"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                        .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
                    Case "byte[]"
                        reppicedit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
                        .Columns(i).OptionsColumn.AllowGroup = False
                        .Columns(i).OptionsColumn.AllowSort = False
                        .Columns(i).OptionsFilter.AllowFilter = False
                        .Columns(i).ColumnEdit = reppicedit
                    Case "boolean"
                        .Columns(i).ColumnEdit = repckedit
                End Select
            Next
        End With
    End Sub
#End Region

#Region "Menu Baru"
    Private Sub bvExit_ItemClick(sender As Object, e As BackstageViewItemEventArgs) Handles bvExit.ItemClick
        pmAppMain.HidePopup()
        backstageViewControl1.Ribbon.HideApplicationButtonContentControl()

        Me.Close()
        DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub bvLogin_ItemClick(sender As Object, e As BackstageViewItemEventArgs) Handles bvLogin.ItemClick
        pmAppMain.HidePopup()
        backstageViewControl1.Ribbon.HideApplicationButtonContentControl()

        mnLoginOut(UserLogin.HasLogin)
    End Sub

    Private Sub TileView1_ItemClick(sender As Object, e As TileViewItemClickEventArgs) Handles TileView1.ItemClick
        If (Not UserLogin.HasLogin) Then
            If (XtraMessageBox.Show("Ingin mengubah ke pengaturan Profile yang dipilih?", NamaAplikasi, MessageBoxButtons.YesNo) = DialogResult.Yes) Then
                Try
                    Dim Obj = MDBSettingBindingSource.Current

                    If Obj IsNot Nothing Then
                        TryCast(Obj, DBSettings.MDBSetting).Default = True
                        Obj = DBSetting.Save(Obj)
                        If Obj IsNot Nothing Then
                            StrKonSQL = Obj.KoneksiString

                            RefreshDataSQLite()
                        End If
                    End If
                Catch ex As Exception
                    XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End If
    End Sub

    Private Sub cmdNewDB_Click(sender As Object, e As EventArgs) Handles cmdNewDB.Click
        Using frm As New frmNewProfile
            Try
                If frm.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                    Dim DBSetting = [Public].DBSetting.List.Where(Function(m) m.Default = True).SingleOrDefault
                    If (DBSetting IsNot Nothing) Then
                        StrKonSQL = DBSetting.KoneksiString
                    Else
                        StrKonSQL = "Data Source=" & BacaIni("DBConfig", "Server", "localhost") &
                                    ";initial Catalog=" & BacaIni("DBConfig", "Database", "dbpos") &
                                    ";User ID=" & BacaIni("DBConfig", "Username", "sa") &
                                    ";Password=" & BacaIni("DBConfig", "Password", "Sg1") &
                                    ";Connect Timeout=" & BacaIni("DBConfig", "Timeout", "15")
                    End If
                    RefreshDataSQLite()
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, "Admin Says", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub cmdOpenDB_Click(sender As Object, e As EventArgs) Handles cmdOpenDB.Click
        Using frm As New frmOpenProfile
            Try
                If frm.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                    Dim DBSetting = [Public].DBSetting.List.Where(Function(m) m.Default = True).SingleOrDefault
                    If (DBSetting IsNot Nothing) Then
                        StrKonSQL = DBSetting.KoneksiString
                    Else
                        StrKonSQL = "Data Source=" & BacaIni("DBConfig", "Server", "localhost") &
                                    ";initial Catalog=" & BacaIni("DBConfig", "Database", "dbpos") &
                                    ";User ID=" & BacaIni("DBConfig", "Username", "sa") &
                                    ";Password=" & BacaIni("DBConfig", "Password", "Sg1") &
                                    ";Connect Timeout=" & BacaIni("DBConfig", "Timeout", "15")
                    End If
                    RefreshDataSQLite()
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, "Admin Says", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub cmdReindexDB_Click(sender As Object, e As EventArgs) Handles cmdReindexDB.Click

    End Sub

    Private Sub cmdBackupDB_Click(sender As Object, e As EventArgs) Handles cmdBackupDB.Click

    End Sub

    Private Sub cmdRestoreDB_Click(sender As Object, e As EventArgs) Handles cmdRestoreDB.Click

    End Sub

    Private Sub backstageViewControl1_Shown(sender As Object, e As EventArgs) Handles backstageViewControl1.Shown
        backstageViewControl1.SelectedTabIndex = 0
    End Sub

#End Region
End Class
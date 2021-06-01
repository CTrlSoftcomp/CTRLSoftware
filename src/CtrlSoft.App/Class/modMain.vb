Imports System.Globalization
Imports DevExpress.XtraEditors
Imports CtrlSoft.App.Ini
Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.App.Public
Imports System.Threading
Imports System.IO
Imports DevExpress.XtraReports.Security

Public Class modMain

    <STAThread()>
    Shared Sub Main()
        Try
            'Default System
            InitFolders()

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

            'DevExpress.UserSkins.OfficeSkins.Register()
            ScriptPermissionManager.GlobalInstance = New ScriptPermissionManager(ExecutionMode.Unrestricted)

            DevExpress.UserSkins.BonusSkins.Register()
            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            ' Change current culture
            Dim culture As CultureInfo
            culture = CultureInfo.CreateSpecificCulture("en-US")
            Thread.CurrentThread.CurrentCulture = culture
            Thread.CurrentThread.CurrentUICulture = culture

            'DevExpress.Utils.AppearanceObject.DefaultFont = New Font("Segoe UI", 11)
            'DevExpress.Utils.AppearanceObject.ControlAppearance.Font = New Font("Segoe UI", 11)
            'DevExpress.Utils.AppearanceObject.EmptyAppearance.Font = New Font("Segoe UI", 11)

            Application.Run(New frmMain())
        Catch ex As Exception
            XtraMessageBox.Show("System error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Shared Sub InitFolders()
        If Not Directory.Exists(GetAppPath() & "\System") Then
            Directory.CreateDirectory(GetAppPath() & "\System")
        End If
        If Not Directory.Exists(GetAppPath() & "\System\Layouts") Then
            Directory.CreateDirectory(GetAppPath() & "\System\Layouts")
        End If
        If Not Directory.Exists(GetAppPath() & "\Report") Then
            Directory.CreateDirectory(GetAppPath() & "\Report")
        End If
        If Not Directory.Exists(GetAppPath() & "\Report") Then
            Directory.CreateDirectory(GetAppPath() & "\Report")
        End If
    End Sub

    Public Enum FormName
        DaftarPO = 0
        DaftarPembelian = 1
        DaftarReturPembelian = 2
        DaftarPenjualan = 3
        DaftarReturPenjualan = 4
        DaftarMutasiGudang = 5
        DaftarPenyesuaian = 6
        DaftarPemakaian = 7
        DaftarStockOpname = 8
        DaftarSaldoAwalPersediaan = 9
        DaftarSaldoAwalHutang = 10
        DaftarSaldoAwalPiutang = 11
    End Enum

    Public Enum FormInternal
        MutasiGudang = 0
        Penyesuaian = 1
        Pemakaian = 2
    End Enum
End Class

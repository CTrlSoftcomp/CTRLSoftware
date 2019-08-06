Imports System.Globalization
Imports DevExpress.XtraEditors
Imports CtrlSoft.Ini
Imports CtrlSoft.Utils
Imports System.Threading
Imports System.IO

Public Class modMain

    <STAThread()> _
        Shared Sub Main()
        Try
            'Default System
            InitFolders()

            StrKonSQL = "Data Source=" & BacaIni("DBConfig", "Server", "localhost") & _
                        ";initial Catalog=" & BacaIni("DBConfig", "Database", "dbpos") & _
                        ";User ID=" & BacaIni("DBConfig", "Username", "sa") & _
                        ";Password=" & BacaIni("DBConfig", "Password", "Sg1") & _
                        ";Connect Timeout=" & BacaIni("DBConfig", "Timeout", "15")

            DevExpress.UserSkins.OfficeSkins.Register()
            DevExpress.UserSkins.BonusSkins.Register()
            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            ' Change current culture
            Dim culture As CultureInfo
            culture = CultureInfo.CreateSpecificCulture("en-US")
            Thread.CurrentThread.CurrentCulture = culture
            Thread.CurrentThread.CurrentUICulture = culture

            DevExpress.Utils.AppearanceObject.DefaultFont = New Font("Segoe UI", 10)
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
End Class

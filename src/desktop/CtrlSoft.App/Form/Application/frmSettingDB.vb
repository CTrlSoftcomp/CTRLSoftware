Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports System.Data
Imports System.Data.SqlClient
Imports CtrlSoft.App.Utils
Imports System.Data.Odbc

Public Class frmSettingDB

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        DxErrorProvider1.ClearErrors()
        If TextEdit1.Text = "" Then
            DxErrorProvider1.SetError(TextEdit1, "Server harus diisi!")
        End If
        If TextEdit2.Text = "" Then
            DxErrorProvider1.SetError(TextEdit2, "Database harus diisi!")
        End If
        If TextEdit3.Text = "" Then
            DxErrorProvider1.SetError(TextEdit3, "User ID harus diisi!")
        End If
        If TextEdit4.Text = "" Then
            DxErrorProvider1.SetError(TextEdit4, "Password harus diisi!")
        End If
        If TextEdit5.Text = "" OrElse NullToLong(TextEdit5.Value) <= 0 Then
            DxErrorProvider1.SetError(TextEdit5, "Timeout harus lebih dari 0!")
        End If
        If TextEdit6.Text = "" Then
            DxErrorProvider1.SetError(TextEdit6, "ODBC harus diisi!")
        End If
        If Not DxErrorProvider1.HasErrors Then
            Using cn As New SqlConnection("Data Source=" & TextEdit1.Text & _
                        ";initial Catalog=" & TextEdit2.Text & _
                        ";User ID=" & TextEdit3.Text & _
                        ";Password=" & TextEdit4.Text & _
                        ";Connect Timeout=" & NullToLong(TextEdit5.Value))
                Using cnODBC As New OdbcConnection
                    Try
                        cn.Open()

                        ''Create ODBC
                        'ODBC.CreateSystemDSN(TextEdit6.Text, TextEdit1.Text, TextEdit3.Text, TextEdit4.Text, TextEdit2.Text)
                        'Test ODBC
                        cnODBC.ConnectionString = "Dsn=" & TextEdit6.Text & ";uid=" & TextEdit3.Text & ";pwd=" & TextEdit4.Text
                        cnODBC.Open()

                        'Save Koneksi
                        Ini.TulisIni("DBConfig", "Server", TextEdit1.Text)
                        Ini.TulisIni("DBConfig", "Database", TextEdit2.Text)
                        Ini.TulisIni("DBConfig", "Username", TextEdit3.Text)
                        Ini.TulisIni("DBConfig", "Password", TextEdit4.Text)
                        Ini.TulisIni("DBConfig", "TimeOut", NullToLong(TextEdit5.Value))
                        Ini.TulisIni("DBConfig", "ODBC", TextEdit6.Text)

                        DialogResult = Windows.Forms.DialogResult.OK
                        Close()
                    Catch ex As Exception
                        XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End Try
                End Using
            End Using
        End If
        Windows.Forms.Cursor.Current = curentcursor
    End Sub

    Private Sub TextEdit6_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles TextEdit6.ButtonClick
        If e.Button.Index = 0 Then
            RunODBC()
        End If
    End Sub

    Private Sub frmSettingDB_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TextEdit1.Text = Ini.BacaIni("DBConfig", "Server", "localhost")
        TextEdit2.Text = Ini.BacaIni("DBConfig", "Database", "dbpos")
        TextEdit3.Text = Ini.BacaIni("DBConfig", "Username", "sa")
        TextEdit4.Text = Ini.BacaIni("DBConfig", "Password", "Sg1")
        TextEdit5.Value = Ini.BacaIni("DBConfig", "Timeout", "15")
        TextEdit6.Text = Ini.BacaIni("DBConfig", "ODBC", "DBPOS")
    End Sub

    Sub RunODBC()
        Shell("odbcad32.exe", AppWinStyle.NormalFocus, False)
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class
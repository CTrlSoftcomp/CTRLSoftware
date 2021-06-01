Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports System.Data
Imports System.Data.SqlClient
Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.App.Public
Imports System.Data.Odbc

Public Class frmSettingDBEntri
    Public DBSetting As DBSettings.MDBSetting

    Public Sub New(ByVal Id As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        DBSetting = [Public].DBSetting.Get(Id)
    End Sub

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
        If Not DxErrorProvider1.HasErrors Then
            Using cn As New SqlConnection("Data Source=" & TextEdit1.Text &
                        ";initial Catalog=" & TextEdit2.Text &
                        ";User ID=" & TextEdit3.Text &
                        ";Password=" & TextEdit4.Text &
                        ";Connect Timeout=" & NullToLong(TextEdit5.Value))
                Try
                    cn.Open()

                    'Save Koneksi
                    DBSetting.Server = TextEdit1.Text
                    DBSetting.Database = TextEdit2.Text
                    DBSetting.UserID = TextEdit3.Text
                    DBSetting.Password = TextEdit4.Text
                    DBSetting.Timeout = NullTolInt(TextEdit5.EditValue)
                    DBSetting.Default = CheckEdit1.Checked

                    DBSetting = [Public].DBSetting.Save(DBSetting)

                    DialogResult = Windows.Forms.DialogResult.OK
                    Close()
                Catch ex As Exception
                    XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Try
            End Using
        End If
        Windows.Forms.Cursor.Current = curentcursor
    End Sub

    Private Sub frmSettingDB_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If DBSetting IsNot Nothing Then
            txtID.Text = DBSetting.Id
            TextEdit1.Text = DBSetting.Server
            TextEdit2.Text = DBSetting.Database
            TextEdit3.Text = DBSetting.UserID
            TextEdit4.Text = DBSetting.Password
            TextEdit5.Value = DBSetting.Timeout
            CheckEdit1.Checked = DBSetting.Default
        Else
            DBSetting = New DBSettings.MDBSetting With {.Id = ""}
            txtID.Text = DBSetting.Id
            TextEdit1.Text = Ini.BacaIni("DBConfig", "Server", "localhost")
            TextEdit2.Text = Ini.BacaIni("DBConfig", "Database", "dbpos")
            TextEdit3.Text = Ini.BacaIni("DBConfig", "Username", "sa")
            TextEdit4.Text = Ini.BacaIni("DBConfig", "Password", "Sg1")
            TextEdit5.Value = Ini.BacaIni("DBConfig", "Timeout", "15")
            CheckEdit1.Checked = True
        End If
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class
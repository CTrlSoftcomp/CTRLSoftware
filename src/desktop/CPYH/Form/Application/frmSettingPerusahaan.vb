Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports System.Data
Imports System.Data.SqlClient
Imports CtrlSoft.Utils
Imports System.Data.Odbc

Public Class frmSettingPerusahaan

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        DxErrorProvider1.ClearErrors()
        If TextEdit1.Text = "" Then
            DxErrorProvider1.SetError(TextEdit1, "Nama Perusahaan Tidak Boleh Kosong!", DXErrorProvider.ErrorType.Information)
        End If
        If Not System.IO.Directory.Exists(TextEdit3.Text) Then
            DxErrorProvider1.SetError(TextEdit3, "Path Layouts tidak ditemukan!", DXErrorProvider.ErrorType.Information)
        End If
        If Not DxErrorProvider1.HasErrors Then
            Dim Obj As New Model.SettingPerusahaan With {.NamaPerusahaan = TextEdit1.Text, _
                                                         .AlamatPerusahaan = MemoEdit1.Text, _
                                                         .KotaPerusahaan = TextEdit2.Text, _
                                                         .PathLayouts = TextEdit3.Text}
            Obj = Repository.RepConfig.SetSettingPerusahaan(Obj)
            If Obj IsNot Nothing Then
                Utils.SettingPerusahaan = Obj
                DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub

    Private Sub TextEdit3_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles TextEdit3.ButtonClick
        Select Case e.Button.Index
            Case 0
                Using frm As New FolderBrowserDialog With {.ShowNewFolderButton = True, _
                                                           .SelectedPath = TextEdit3.Text, _
                                                           .Description = "Pilih Folder Untuk Layouts"}
                    Try
                        If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                            TextEdit3.Text = frm.SelectedPath
                        End If
                    Catch ex As Exception
                        XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End Try
                End Using
        End Select
    End Sub

    Private Sub frmSettingPerusahaan_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            TextEdit1.Text = Utils.SettingPerusahaan.NamaPerusahaan
            TextEdit2.Text = Utils.SettingPerusahaan.KotaPerusahaan
            TextEdit3.Text = Utils.SettingPerusahaan.PathLayouts
            MemoEdit1.Text = Utils.SettingPerusahaan.AlamatPerusahaan
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
End Class
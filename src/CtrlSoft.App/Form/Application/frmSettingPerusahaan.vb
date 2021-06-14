Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports System.Data
Imports System.Data.SqlClient
Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.App.Public
Imports System.Data.Odbc
Imports CtrlSoft.Dto.Model

Public Class frmSettingPerusahaan

    Private Sub TextEdit3_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles TextEdit3.ButtonClick
        Select Case e.Button.Index
            Case 0
                Using frm As New FolderBrowserDialog With {.ShowNewFolderButton = True,
                                                           .SelectedPath = TextEdit3.Text,
                                                           .Description = "Pilih Folder Untuk Layouts"}
                    Try
                        If frm.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
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
            TextEdit1.Text = [Public].SettingPerusahaan.NamaPerusahaan
            TextEdit2.Text = [Public].SettingPerusahaan.KotaPerusahaan
            TextEdit3.Text = [Public].SettingPerusahaan.PathLayouts
            MemoEdit1.Text = [Public].SettingPerusahaan.AlamatPerusahaan
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub mnSimpan_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSimpan.ItemClick
        DxErrorProvider1.ClearErrors()
        If TextEdit1.Text = "" Then
            DxErrorProvider1.SetError(TextEdit1, "Nama Perusahaan Tidak Boleh Kosong!", DXErrorProvider.ErrorType.Information)
        End If
        If Not System.IO.Directory.Exists(TextEdit3.Text) Then
            DxErrorProvider1.SetError(TextEdit3, "Path Layouts tidak ditemukan!", DXErrorProvider.ErrorType.Information)
        End If
        If Not DxErrorProvider1.HasErrors Then
            Dim Obj As New SettingPerusahaan With {.NamaPerusahaan = TextEdit1.Text,
                                                         .AlamatPerusahaan = MemoEdit1.Text,
                                                         .KotaPerusahaan = TextEdit2.Text,
                                                         .PathLayouts = TextEdit3.Text}
            Obj = Repository.RepConfig.SetSettingPerusahaan(Obj)
            If Obj IsNot Nothing Then
                [Public].SettingPerusahaan = Obj
                DialogResult = System.Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub

    Private Sub mnTutup_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnTutup.ItemClick
        DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class
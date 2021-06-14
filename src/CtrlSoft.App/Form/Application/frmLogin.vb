﻿Public Class frmLogin 

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        DxErrorProvider1.ClearErrors()
        If TextEdit1.Text = "" Then
            DxErrorProvider1.SetError(TextEdit1, "User ID Harus diisi!", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information)
        End If
        If TextEdit2.Text = "" Then
            DxErrorProvider1.SetError(TextEdit2, "Password Harus diisi!", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information)
        End If
        If Not DxErrorProvider1.HasErrors Then
            Dim User As CtrlSoft.Dto.Model.MUser = Repository.RepSQLServer.GetLogin(TextEdit1.Text, TextEdit2.Text)
            If User IsNot Nothing Then
                [Public].UserLogin = User
                [Public].SettingPerusahaan = Repository.RepConfig.GetSettingPerusahaan()
                DialogResult = System.Windows.Forms.DialogResult.OK
                Me.Close()
            Else
                DxErrorProvider1.SetError(TextEdit1, "User dan Password Salah!", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information)
            End If
        End If
    End Sub

    Private Sub frmLogin_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SimpleButton1.ImageList = frmMain.ICButtons
        SimpleButton2.ImageList = frmMain.ICButtons
    End Sub
End Class
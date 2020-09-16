Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports System.Data
Imports System.Data.SqlClient
Imports CtrlSoft.App.Utils
Imports System.Data.Odbc

Public Class frmDashboard

    Private Sub frmDashboard_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'e.Cancel = True
    End Sub

    Private Sub frmDashboard_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If System.IO.File.Exists(GetAppPath() & "\system\image\Background.jpg") Then
        '    PictureEdit1.Image = Image.FromFile(GetAppPath() & "\system\image\Background.jpg")
        'End If
    End Sub
End Class
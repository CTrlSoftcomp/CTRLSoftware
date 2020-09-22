Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.App.Public
Imports CtrlSoft.Dto.Model
Imports CtrlSoft.Dto.ViewModel

Namespace Repository
    Public Class RepConfig
        Public Shared Function GetSettingPerusahaan() As SettingPerusahaan
            Dim Hasil As SettingPerusahaan = Nothing
            Using dlg As New WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
                dlg.Show()
                dlg.Focus()
                Dim JSON = CtrlSoft.Repository.Repository.RepConfig.GetSettingPerusahaan(StrKonSQL)
                If JSON.JSONResult Then
                    Hasil = JSON.JSONValue
                Else
                    XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Hasil = New SettingPerusahaan
                End If
            End Using

            Return Hasil
        End Function
        Public Shared Function SetSettingPerusahaan(ByVal Obj As SettingPerusahaan) As SettingPerusahaan
            Dim Hasil As SettingPerusahaan = Nothing
            Using dlg As New WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
                dlg.Show()
                dlg.Focus()
                Dim JSON = CtrlSoft.Repository.Repository.RepConfig.SetSettingPerusahaan(StrKonSQL, Obj)
                If JSON.JSONResult Then
                    Hasil = JSON.JSONValue
                Else
                    XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Hasil = New SettingPerusahaan
                End If
            End Using

            Return Hasil
        End Function
    End Class
End Namespace

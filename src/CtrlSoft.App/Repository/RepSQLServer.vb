Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.App.Public
Imports CtrlSoft.Dto.Model

Namespace Repository
    Public Class RepSQLServer
        Public Shared Function GetKodeKontak(ByVal tipe As Integer) As String
            'All = 0
            'Customer = 1
            'Supplier = 2
            'Pegawai = 3
            Dim Hasil As String = ""
            Using dlg As New WaitDialogForm("Sedang mengambil data ...", NamaAplikasi)
                dlg.Show()
                dlg.Focus()
                Dim JSON = CtrlSoft.Repository.Repository.RepSQLServer.GetKodeKontak(StrKonSQL, tipe)
                If JSON.JSONResult Then
                    Hasil = JSON.JSONValue
                Else
                    XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Using
            Return Hasil
        End Function
        Public Shared Function GetLogin(ByVal Kode As String, ByVal Pwd As String) As CtrlSoft.Dto.Model.MUser
            Dim User As CtrlSoft.Dto.Model.MUser = Nothing
            Using dlg As New WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
                dlg.Show()
                dlg.Focus()
                Dim JSON = CtrlSoft.Repository.Repository.RepSQLServer.GetLogin(StrKonSQL, Kode, Pwd)
                If JSON.JSONResult Then
                    User = JSON.JSONValue
                Else
                    XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Using
            Return User
        End Function
        Public Shared Function GetOtorisasiSPV(ByVal Kode As String, ByVal Pwd As String) As CtrlSoft.Dto.Model.MUser
            Dim User As CtrlSoft.Dto.Model.MUser = Nothing
            Using dlg As New WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
                dlg.Show()
                dlg.Focus()
                Dim JSON = CtrlSoft.Repository.Repository.RepSQLServer.GetOtorisasiSPV(StrKonSQL, Kode, Pwd)
                If JSON.JSONResult Then
                    User = JSON.JSONValue
                Else
                    XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Using
            Return User
        End Function
        Public Shared Function GetTimeServer() As Date
            Dim Hasil As Date = Now
            Using dlg As New WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
                dlg.Show()
                dlg.Focus()
                Dim JSON = CtrlSoft.Repository.Repository.RepSQLServer.GetTimeServer(StrKonSQL)
                If JSON.JSONResult Then
                    Hasil = JSON.JSONValue
                Else
                    XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Using

            Return Hasil
        End Function
        Public Shared Function GetListKategori() As List(Of Core)
            Dim Hasil As New List(Of Core)
            Using dlg As New WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
                dlg.Show()
                dlg.Focus()
                Dim JSON = CtrlSoft.Repository.Repository.RepSQLServer.GetListKategori(StrKonSQL)
                If JSON.JSONResult Then
                    Hasil = JSON.JSONValue
                Else
                    XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Using

            Return Hasil
        End Function
        Public Shared Function GetListDepartemen() As List(Of Core)
            Dim Hasil As New List(Of Core)
            Using dlg As New WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
                dlg.Show()
                dlg.Focus()
                Dim JSON = CtrlSoft.Repository.Repository.RepSQLServer.GetListDepartemen(StrKonSQL)
                If JSON.JSONResult Then
                    Hasil = JSON.JSONValue
                Else
                    XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Using

            Return Hasil
        End Function
        Public Shared Function GetListSupplier() As List(Of Core)
            Dim Hasil As New List(Of Core)
            Using dlg As New WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
                dlg.Show()
                dlg.Focus()
                Dim JSON = CtrlSoft.Repository.Repository.RepSQLServer.GetListSupplier(StrKonSQL)
                If JSON.JSONResult Then
                    Hasil = JSON.JSONValue
                Else
                    XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Using

            Return Hasil
        End Function
        Public Shared Function GetListMerk() As List(Of Core)
            Dim Hasil As New List(Of Core)
            Using dlg As New WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
                dlg.Show()
                dlg.Focus()
                Dim JSON = CtrlSoft.Repository.Repository.RepSQLServer.GetListMerk(StrKonSQL)
                If JSON.JSONResult Then
                    Hasil = JSON.JSONValue
                Else
                    XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Using

            Return Hasil
        End Function
        Public Shared Function GetListGudang() As List(Of Core)
            Dim Hasil As New List(Of Core)
            Using dlg As New WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
                dlg.Show()
                dlg.Focus()
                Dim JSON = CtrlSoft.Repository.Repository.RepSQLServer.GetListGudang(StrKonSQL)
                If JSON.JSONResult Then
                    Hasil = JSON.JSONValue
                Else
                    XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Using

            Return Hasil
        End Function
        Public Shared Function GetBarangDetil(ByVal Barcode As String) As List(Of Core)
            Dim Hasil As New List(Of Core)
            Using dlg As New WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
                dlg.Show()
                dlg.Focus()
                Dim JSON = CtrlSoft.Repository.Repository.RepSQLServer.GetBarangDetil(StrKonSQL, Barcode)
                If JSON.JSONResult Then
                    Hasil = JSON.JSONValue
                Else
                    XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Using

            Return Hasil
        End Function
    End Class
End Namespace
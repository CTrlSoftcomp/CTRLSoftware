Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports System.IO
Imports System.Diagnostics
Imports DevExpress.XtraEditors
Imports DevExpress.Skins
Imports DevExpress.XtraBars.Ribbon
Imports DevExpress.XtraBars.Ribbon.Gallery
Imports DevExpress.Utils.Drawing
Imports DevExpress.Utils
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.LookAndFeel
Imports CtrlSoft.App.Ini
Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.App.Public
Imports CtrlSoft.App.CetakDX
Imports DevExpress.XtraBars
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo

Public Class frmHasilPosting
    Private IDTransaksi As Long
    Private IDJenisTransaksi As Long

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Public Sub New(ByVal IDTransaksi As Long, ByVal IDJenisTransaksi As Long)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.IDJenisTransaksi = IDJenisTransaksi
        Me.IDTransaksi = IDTransaksi
    End Sub

    Public Sub LoadData(ByVal IDTransaksi As Long, ByVal IDJenisTransaksi As Long)
        Try
            Me.IDJenisTransaksi = IDJenisTransaksi
            Me.IDTransaksi = IDTransaksi

            RefreshData(-1)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub frmHasilPosting_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            RefreshData(-1)
        Catch ex As Exception

        End Try
    End Sub

    Public Sub RefreshData(ByVal NoID As Long)
        Using dlg As New WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using ds As New DataSet
                            Try
                                dlg.Show()
                                dlg.Focus()
                                cn.Open()
                                com.Connection = cn
                                oDA.SelectCommand = com

                                com.CommandText = "EXEC spHasilPosting @IDTransaksi, @IDJenisTransaksi"
                                com.Parameters.Clear()
                                com.Parameters.Add(New SqlParameter("@IDTransaksi", SqlDbType.BigInt)).Value = IDTransaksi
                                com.Parameters.Add(New SqlParameter("@IDJenisTransaksi", SqlDbType.BigInt)).Value = IDJenisTransaksi
                                oDA.Fill(ds)

                                BindingSource1.DataSource = ds.Tables(0)
                                BindingSource2.DataSource = ds.Tables(1)

                                GridView1.ClearSelection()
                                GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), NoID.ToString("n0"))
                                GridView1.SelectRow(GridView1.FocusedRowHandle)

                                GridView2.ClearSelection()
                                GridView2.FocusedRowHandle = GridView2.LocateByDisplayText(0, GridView2.Columns("NoID"), NoID.ToString("n0"))
                                GridView2.SelectRow(GridView2.FocusedRowHandle)
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Private Sub mnSaveLayout_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayout.ItemClick
        Using frm As New frmOtorisasi
            Try
                If frm.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                    GridView1.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & GridView1.Name & ".xml")
                    GridView2.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & GridView2.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub GridView1_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataSourceChanged, GridView2.DataSourceChanged
        With sender
            If System.IO.File.Exists([Public].SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml") Then
                .RestoreLayoutFromXml([Public].SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml")
            End If
            For i As Integer = 0 To .Columns.Count - 1
                Select Case .Columns(i).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(i).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(i).DisplayFormat.FormatString = "n2"
                    Case "string"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        .Columns(i).DisplayFormat.FormatString = ""
                    Case "date"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                        .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                    Case "datetime"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                        .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
                    Case "byte[]"
                        reppicedit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
                        .Columns(i).OptionsColumn.AllowGroup = False
                        .Columns(i).OptionsColumn.AllowSort = False
                        .Columns(i).OptionsFilter.AllowFilter = False
                        .Columns(i).ColumnEdit = reppicedit
                    Case "boolean"
                        .Columns(i).ColumnEdit = repckedit
                End Select
            Next
        End With
    End Sub

    Private Sub mnRefresh_ItemClick(sender As Object, e As ItemClickEventArgs) Handles mnRefresh.ItemClick
        RefreshData(-1)
    End Sub

    Private Sub mnTutup_ItemClick(sender As Object, e As ItemClickEventArgs) Handles mnTutup.ItemClick
        Me.Close()
        DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub
End Class
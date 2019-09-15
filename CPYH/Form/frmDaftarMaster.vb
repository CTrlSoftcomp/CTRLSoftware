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
Imports CtrlSoft.Ini
Imports CtrlSoft.Utils
Imports CtrlSoft.CetakDX
Imports DevExpress.XtraBars
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors.Repository

Public Class frmDaftarMaster
    Private formName As String
    Private tableName As String
    Private ds As New DataSet

    Private SQL As String

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        RefreshData(-1)
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

                                If ckTdkAktif.Checked Then
                                    com.CommandText = SQL
                                Else
                                    com.CommandText = SQL & " WHERE [" & tableName & "].IsActive=1"
                                End If
                                oDA.Fill(ds, tableName)
                                BindingSource1.DataSource = ds.Tables(tableName)

                                GridView1.ClearSelection()
                                GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), NoID.ToString("n0"))
                                GridView1.SelectRow(GridView1.FocusedRowHandle)

                                Me.ds = ds
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Private Sub cmdCetak_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCetak.Click
        Dim NamaFile As String = ""
        NamaFile = Application.StartupPath & "\Report\Lap_" & tableName & ".repx"
        ViewXtraReport(Me.MdiParent, IIf(IsEditReport, action_.Edit, action_.Preview), NamaFile, "Laporan Master", "Lap_" & tableName & ".repx", Me.ds)
    End Sub

    Private Sub cmdHapus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHapus.Click
        If GridView1.RowCount >= 1 Then
            Select Case tableName
                Case "MKategori"
                    If XtraMessageBox.Show("Ingin menonaktifkan data Kategori " & NullToStr(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "Nama")) & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        HapusData(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
                    End If
                Case "MSatuan"
                    If XtraMessageBox.Show("Ingin menonaktifkan data Satuan " & NullToStr(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "Nama")) & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        HapusData(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
                    End If
                Case "MMerk"
                    If XtraMessageBox.Show("Ingin menonaktifkan data Merk " & NullToStr(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "Nama")) & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        HapusData(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
                    End If
                Case "MGudang"
                    If XtraMessageBox.Show("Ingin menonaktifkan data Gudang " & NullToStr(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "Nama")) & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        HapusData(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
                    End If
                Case Else
                    XtraMessageBox.Show("Durong isok Boss!!!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End Select
        End If
    End Sub

    Private Sub HapusData(ByVal NoID As Long)
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
                                com.Transaction = cn.BeginTransaction
                                oDA.SelectCommand = com

                                Select Case tableName
                                    Case "MKategori", "MSatuan", "MMerk", "MGudang"
                                        com.CommandText = "UPDATE " & tableName & " SET IsActive=0 WHERE NoID=" & NoID
                                        com.ExecuteNonQuery()
                                End Select

                                If com.Transaction IsNot Nothing Then
                                    com.Transaction.Commit()
                                    com.Transaction = Nothing
                                    cmdRefresh.PerformClick()
                                End If
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        Select Case tableName
            Case "MGudang"
                Using frm As New frmEntriGudang(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
                    If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        RefreshData(frm.NoID)
                    End If
                End Using
            Case "MMerk"
                Using frm As New frmEntriMerk(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
                    If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        RefreshData(frm.NoID)
                    End If
                End Using
            Case "MSatuan"
                Using frm As New frmEntriSatuan(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
                    If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        RefreshData(frm.NoID)
                    End If
                End Using
            Case "MKategori"
                Using frm As New frmEntriKategori(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
                    If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        RefreshData(frm.NoID)
                    End If
                End Using
            Case Else
                XtraMessageBox.Show("Durong isok Boss!!!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End Select
    End Sub

    Private Sub cmdBaru_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBaru.Click
        Select Case tableName
            Case "MGudang"
                Using frm As New frmEntriGudang(-1)
                    If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        RefreshData(frm.NoID)
                    End If
                End Using
            Case "MMerk"
                Using frm As New frmEntriMerk(-1)
                    If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        RefreshData(frm.NoID)
                    End If
                End Using
            Case "MSatuan"
                Using frm As New frmEntriSatuan(-1)
                    If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        RefreshData(frm.NoID)
                    End If
                End Using
            Case "MKategori"
                Using frm As New frmEntriKategori(-1)
                    If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        RefreshData(frm.NoID)
                    End If
                End Using
            Case Else
                XtraMessageBox.Show("Durong isok Boss!!!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End Select
    End Sub

    Public Sub New(ByVal formName As String, ByVal caption As String, ByVal tableName As String, ByVal SQL As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Text = caption
        Me.tableName = tableName
        Me.formName = formName
        Me.SQL = SQL
        Me.Tag = Me.formName
        Me.Name = Me.formName
    End Sub

    Private Sub GridView1_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataSourceChanged
        With GridView1
            If System.IO.File.Exists(FolderLayouts & Me.Name & .Name & ".xml") Then
                .RestoreLayoutFromXml(FolderLayouts & Me.Name & .Name & ".xml")
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

    Private Sub frmDaftarMaster_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try

        Catch ex As Exception

        End Try
    End Sub

    Private Sub frmDaftarMaster_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            cmdBaru.ImageList = frmMain.ICButtons
            cmdEdit.ImageList = frmMain.ICButtons
            cmdHapus.ImageList = frmMain.ICButtons
            cmdCetak.ImageList = frmMain.ICButtons
            cmdRefresh.ImageList = frmMain.ICButtons
            cmdTutup.ImageList = frmMain.ICButtons

            LabelControl1.Text = Me.Text
            RefreshData(-1)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub mnSaveLayout_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayout.ItemClick
        Using frm As New frmOtorisasi
            Try
                If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    GridView1.SaveLayoutToXml(FolderLayouts & Me.Name & GridView1.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub
End Class
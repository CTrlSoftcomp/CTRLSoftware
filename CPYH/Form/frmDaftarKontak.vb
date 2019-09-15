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

Public Class frmDaftarKontak
    Private formName As String
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

                                SQL = "SELECT [MAlamat].[NoID],[MAlamat].[Kode],[MAlamat].[Nama],[MAlamat].[NamaAlias],[MAlamat].[Alamat],[MAlamat].[Kota]" & vbCrLf & _
                                      ",[MAlamat].[HP],[MAlamat].[Telp],[MAlamat].[ContactPerson],[MAlamat].[LimitHutang],[MAlamat].[LimitPiutang]" & vbCrLf & _
                                      ",[MAlamat].[LimitNotaPiutang],[MAlamat].[LimitUmurPiutang],MTypeHarga.Caption TypeHarga,[MAlamat].[IsActive] Aktif" & vbCrLf & _
                                      ",[MAlamat].[IsSupplier] Supplier,[MAlamat].[IsPegawai] Pegawai,[MAlamat].[IsCustomer] Customer" & vbCrLf & _
                                      "FROM [dbo].[MAlamat]" & vbCrLf & _
                                      "LEFT JOIN MTypeHarga ON MTypeHarga.NoID=MAlamat.IDTypeHarga" & vbCrLf & _
                                      "WHERE 1=1 "
                                If Not ckTdkAktif.Checked Then
                                    SQL &= " AND MAlamat.IsActive=1"
                                End If

                                com.CommandText = SQL
                                oDA.Fill(ds, "MAlamat")
                                BindingSource1.DataSource = ds.Tables("MAlamat")

                                com.CommandText = SQL & " AND MAlamat.IsCustomer=1"
                                oDA.Fill(ds, "MCustomer")
                                BindingSource2.DataSource = ds.Tables("MCustomer")

                                com.CommandText = SQL & " AND MAlamat.IsSupplier=1"
                                oDA.Fill(ds, "MSupplier")
                                BindingSource3.DataSource = ds.Tables("MSupplier")

                                com.CommandText = SQL & " AND MAlamat.IsPegawai=1"
                                oDA.Fill(ds, "MPegawai")
                                BindingSource4.DataSource = ds.Tables("MPegawai")

                                GridView1.ClearSelection()
                                GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), NoID.ToString("n0"))
                                GridView1.SelectRow(GridView1.FocusedRowHandle)

                                GridView2.ClearSelection()
                                GridView2.FocusedRowHandle = GridView2.LocateByDisplayText(0, GridView2.Columns("NoID"), NoID.ToString("n0"))
                                GridView2.SelectRow(GridView1.FocusedRowHandle)

                                GridView3.ClearSelection()
                                GridView3.FocusedRowHandle = GridView3.LocateByDisplayText(0, GridView3.Columns("NoID"), NoID.ToString("n0"))
                                GridView3.SelectRow(GridView1.FocusedRowHandle)

                                GridView4.ClearSelection()
                                GridView4.FocusedRowHandle = GridView4.LocateByDisplayText(0, GridView4.Columns("NoID"), NoID.ToString("n0"))
                                GridView4.SelectRow(GridView4.FocusedRowHandle)

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
        Select Case XtraTabControl1.SelectedTabPageIndex
            Case 1
                NamaFile = Application.StartupPath & "\Report\Lap_MAlamatCustomer.repx"
                ViewXtraReport(Me.MdiParent, IIf(IsEditReport, action_.Edit, action_.Preview), NamaFile, "Laporan Master Customer", "Lap_MAlamatCustomer.repx", Me.ds)
            Case 2
                NamaFile = Application.StartupPath & "\Report\Lap_MAlamatSupplier.repx"
                ViewXtraReport(Me.MdiParent, IIf(IsEditReport, action_.Edit, action_.Preview), NamaFile, "Laporan Master Supplier", "Lap_MAlamatSupplier.repx", Me.ds)
            Case 3
                NamaFile = Application.StartupPath & "\Report\Lap_MAlamatPegawai.repx"
                ViewXtraReport(Me.MdiParent, IIf(IsEditReport, action_.Edit, action_.Preview), NamaFile, "Laporan Master Pegawai", "Lap_MAlamatPegawai.repx", Me.ds)
            Case Else
                NamaFile = Application.StartupPath & "\Report\Lap_MAlamatAll.repx"
                ViewXtraReport(Me.MdiParent, IIf(IsEditReport, action_.Edit, action_.Preview), NamaFile, "Laporan Master All", "Lap_MAlamatAll.repx", Me.ds)
        End Select
    End Sub

    Private Sub cmdHapus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHapus.Click
        Dim gridview As DevExpress.XtraGrid.Views.Grid.GridView = Nothing
        Select Case XtraTabControl1.SelectedTabPageIndex
            Case 1
                gridview = GridView2
            Case 2
                gridview = GridView3
            Case 3
                gridview = GridView4
            Case Else
                gridview = GridView1
        End Select
        If gridview.RowCount >= 1 Then
            If XtraMessageBox.Show("Ingin menonaktifkan data Kontak " & NullToStr(gridview.GetRowCellValue(gridview.FocusedRowHandle, "Nama")) & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                HapusData(NullToLong(gridview.GetRowCellValue(gridview.FocusedRowHandle, "NoID")))
            End If
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

                                com.CommandText = "UPDATE MAlamat SET IsActive=0 WHERE NoID=" & NoID
                                com.ExecuteNonQuery()

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
        Select Case XtraTabControl1.SelectedTabPageIndex
            Case 1
                Using frm As New frmEntriKontak(frmEntriKontak.tipeKontak.Customer, NullToLong(GridView2.GetRowCellValue(GridView2.FocusedRowHandle, "NoID")))
                    If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        RefreshData(frm.NoID)
                    End If
                End Using
            Case 2
                Using frm As New frmEntriKontak(frmEntriKontak.tipeKontak.Supplier, NullToLong(GridView3.GetRowCellValue(GridView3.FocusedRowHandle, "NoID")))
                    If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        RefreshData(frm.NoID)
                    End If
                End Using
            Case 3
                Using frm As New frmEntriKontak(frmEntriKontak.tipeKontak.Pegawai, NullToLong(GridView4.GetRowCellValue(GridView4.FocusedRowHandle, "NoID")))
                    If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        RefreshData(frm.NoID)
                    End If
                End Using
            Case Else
                Using frm As New frmEntriKontak(frmEntriKontak.tipeKontak.All, NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
                    If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        RefreshData(frm.NoID)
                    End If
                End Using
        End Select
    End Sub

    Private Sub cmdBaru_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBaru.Click
        Select Case XtraTabControl1.SelectedTabPageIndex
            Case 1
                Using frm As New frmEntriKontak(frmEntriKontak.tipeKontak.Customer, -1)
                    If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        RefreshData(frm.NoID)
                    End If
                End Using
            Case 2
                Using frm As New frmEntriKontak(frmEntriKontak.tipeKontak.Supplier, -1)
                    If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        RefreshData(frm.NoID)
                    End If
                End Using
            Case 3
                Using frm As New frmEntriKontak(frmEntriKontak.tipeKontak.Pegawai, -1)
                    If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        RefreshData(frm.NoID)
                    End If
                End Using
            Case Else
                Using frm As New frmEntriKontak(frmEntriKontak.tipeKontak.All, -1)
                    If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        RefreshData(frm.NoID)
                    End If
                End Using
        End Select
    End Sub

    Public Sub New(ByVal formName As String, ByVal caption As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Text = caption
        Me.formName = formName
        Me.SQL = SQL
        Me.Tag = Me.formName
        Me.Name = Me.formName
    End Sub

    Private Sub GridView1_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataSourceChanged, GridView2.DataSourceChanged, GridView3.DataSourceChanged, GridView4.DataSourceChanged
        With sender
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
                    GridView2.SaveLayoutToXml(FolderLayouts & Me.Name & GridView2.Name & ".xml")
                    GridView3.SaveLayoutToXml(FolderLayouts & Me.Name & GridView3.Name & ".xml")
                    GridView4.SaveLayoutToXml(FolderLayouts & Me.Name & GridView4.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub
End Class
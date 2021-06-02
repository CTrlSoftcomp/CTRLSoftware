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
Imports CtrlSoft.Repository.Repository.PostingData
Imports CtrlSoft.Repository.Repository.UnPostingData
Imports DevExpress.XtraBars
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports CtrlSoft.Dto.ViewModel

Public Class frmDaftarTransaksi
    Private formName As modMain.FormName
    Private ds As New DataSet

    Private SQL As String

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit
    Private WithEvents frmHasilPosting As New frmHasilPosting(-1, -1)

    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        mnTutup.PerformClick()
    End Sub

    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        mnRefresh.PerformClick()
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

                                Select Case formName
                                    Case modMain.FormName.DaftarSaldoAwalHutang
                                        SQL = "spDaftarSaldoAwalHutang @TglDari, @TglSampai"
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@TglDari", SqlDbType.Date)).Value = DateEdit1.EditValue
                                        com.Parameters.Add(New SqlParameter("@TglSampai", SqlDbType.Date)).Value = DateEdit2.EditValue
                                    Case modMain.FormName.DaftarSaldoAwalPiutang
                                        SQL = "spDaftarSaldoAwalPiutang @TglDari, @TglSampai"
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@TglDari", SqlDbType.Date)).Value = DateEdit1.EditValue
                                        com.Parameters.Add(New SqlParameter("@TglSampai", SqlDbType.Date)).Value = DateEdit2.EditValue
                                    Case modMain.FormName.DaftarSaldoAwalPersediaan
                                        SQL = "spDaftarSaldoAwalPersediaan @TglDari, @TglSampai"
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@TglDari", SqlDbType.Date)).Value = DateEdit1.EditValue
                                        com.Parameters.Add(New SqlParameter("@TglSampai", SqlDbType.Date)).Value = DateEdit2.EditValue
                                    Case modMain.FormName.DaftarStockOpname
                                        SQL = "spDaftarStockOpname @TglDari, @TglSampai"
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@TglDari", SqlDbType.Date)).Value = DateEdit1.EditValue
                                        com.Parameters.Add(New SqlParameter("@TglSampai", SqlDbType.Date)).Value = DateEdit2.EditValue
                                    Case modMain.FormName.DaftarPO
                                        SQL = "spDaftarPO @TglDari, @TglSampai"
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@TglDari", SqlDbType.Date)).Value = DateEdit1.EditValue
                                        com.Parameters.Add(New SqlParameter("@TglSampai", SqlDbType.Date)).Value = DateEdit2.EditValue
                                    Case modMain.FormName.DaftarPembelian
                                        SQL = "spDaftarBeli @TglDari, @TglSampai"
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@TglDari", SqlDbType.Date)).Value = DateEdit1.EditValue
                                        com.Parameters.Add(New SqlParameter("@TglSampai", SqlDbType.Date)).Value = DateEdit2.EditValue
                                    Case modMain.FormName.DaftarReturPembelian
                                        SQL = "spDaftarReturBeli @TglDari, @TglSampai"
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@TglDari", SqlDbType.Date)).Value = DateEdit1.EditValue
                                        com.Parameters.Add(New SqlParameter("@TglSampai", SqlDbType.Date)).Value = DateEdit2.EditValue
                                    Case modMain.FormName.DaftarPenjualan
                                        SQL = "spDaftarJual @TglDari, @TglSampai"
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@TglDari", SqlDbType.Date)).Value = DateEdit1.EditValue
                                        com.Parameters.Add(New SqlParameter("@TglSampai", SqlDbType.Date)).Value = DateEdit2.EditValue
                                    Case modMain.FormName.DaftarReturPenjualan
                                        SQL = "spDaftarReturJual @TglDari, @TglSampai"
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@TglDari", SqlDbType.Date)).Value = DateEdit1.EditValue
                                        com.Parameters.Add(New SqlParameter("@TglSampai", SqlDbType.Date)).Value = DateEdit2.EditValue
                                    Case modMain.FormName.DaftarMutasiGudang
                                        SQL = "spDaftarMutasiGudang @TglDari, @TglSampai"
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@TglDari", SqlDbType.Date)).Value = DateEdit1.EditValue
                                        com.Parameters.Add(New SqlParameter("@TglSampai", SqlDbType.Date)).Value = DateEdit2.EditValue
                                    Case modMain.FormName.DaftarPemakaian
                                        SQL = "spDaftarPemakaian @TglDari, @TglSampai"
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@TglDari", SqlDbType.Date)).Value = DateEdit1.EditValue
                                        com.Parameters.Add(New SqlParameter("@TglSampai", SqlDbType.Date)).Value = DateEdit2.EditValue
                                    Case modMain.FormName.DaftarPenyesuaian
                                        SQL = "spDaftarPenyesuaian @TglDari, @TglSampai"
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@TglDari", SqlDbType.Date)).Value = DateEdit1.EditValue
                                        com.Parameters.Add(New SqlParameter("@TglSampai", SqlDbType.Date)).Value = DateEdit2.EditValue
                                End Select

                                com.CommandText = SQL
                                oDA.Fill(ds, "MData")
                                BindingSource1.DataSource = ds.Tables("MData")

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
        mnPreview.PerformClick()
    End Sub

    Private Sub cmdHapus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHapus.Click
        mnHapus.PerformClick()
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

                                Select Case formName
                                    Case modMain.FormName.DaftarSaldoAwalHutang
                                        com.CommandText = "DELETE FROM MSaldoAwalHutang WHERE ISNULL(MSaldoAwalHutang.IsPosted, 0)=0 AND MSaldoAwalHutang.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                    Case modMain.FormName.DaftarSaldoAwalPiutang
                                        com.CommandText = "DELETE FROM MSaldoAwalPiutang WHERE ISNULL(MSaldoAwalPiutang.IsPosted, 0)=0 AND MSaldoAwalPiutang.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                    Case modMain.FormName.DaftarSaldoAwalPersediaan
                                        com.CommandText = "DELETE FROM MSaldoAwalPersediaan WHERE ISNULL(MSaldoAwalPersediaan.IsPosted, 0)=0 AND MSaldoAwalPersediaan.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                    Case modMain.FormName.DaftarPO
                                        com.CommandText = "DELETE MPOD FROM MPOD INNER JOIN MPO ON MPO.NoID=MPOD.IDHeader WHERE ISNULL(MPO.IsPosted, 0)=0 AND MPO.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE FROM MPO WHERE ISNULL(MPO.IsPosted, 0)=0 AND MPO.NoID=" & NoID
                                        com.ExecuteNonQuery()
                                    Case modMain.FormName.DaftarPembelian
                                        com.CommandText = "DELETE MBeliD FROM MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDHeader WHERE ISNULL(MBeli.IsPosted, 0)=0 AND MBeli.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE FROM MBeli WHERE ISNULL(MBeli.IsPosted, 0)=0 AND MBeli.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                    Case modMain.FormName.DaftarReturPembelian
                                        com.CommandText = "DELETE MReturBeliD FROM MReturBeliD INNER JOIN MReturBeli ON MReturBeli.NoID=MReturBeliD.IDHeader WHERE ISNULL(MReturBeli.IsPosted, 0)=0 AND MReturBeli.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE FROM MReturBeli WHERE ISNULL(MReturBeli.IsPosted, 0)=0 AND MReturBeli.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                    Case modMain.FormName.DaftarPenjualan
                                        com.CommandText = "DELETE MJualD FROM MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDHeader WHERE ISNULL(MJual.IsPosted, 0)=0 AND MJual.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE MJualDBayar FROM MJualDBayar INNER JOIN MJual ON MJual.NoID=MJualDBayar.IDHeader WHERE ISNULL(MJual.IsPosted, 0)=0 AND MJual.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE FROM MJual WHERE ISNULL(MJual.IsPosted, 0)=0 AND MJual.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                    Case modMain.FormName.DaftarReturPenjualan
                                        com.CommandText = "DELETE MReturJualD FROM MReturJualD INNER JOIN MReturJual ON MReturJual.NoID=MReturJualD.IDHeader WHERE ISNULL(MReturJual.IsPosted, 0)=0 AND MReturJual.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE MReturJualDBayar FROM MReturJualDBayar INNER JOIN MReturJual ON MReturJual.NoID=MReturJualDBayar.IDHeader WHERE ISNULL(MReturJual.IsPosted, 0)=0 AND MReturJual.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE FROM MReturJual WHERE ISNULL(MReturJual.IsPosted, 0)=0 AND MReturJual.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                    Case modMain.FormName.DaftarMutasiGudang
                                        com.CommandText = "DELETE MMutasiGudangD FROM MMutasiGudangD INNER JOIN MMutasiGudang ON MMutasiGudang.NoID=MMutasiGudangD.IDHeader WHERE ISNULL(MMutasiGudang.IsPosted, 0)=0 AND MMutasiGudang.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE FROM MMutasiGudang WHERE ISNULL(MMutasiGudang.IsPosted, 0)=0 AND MMutasiGudang.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                    Case modMain.FormName.DaftarPemakaian
                                        com.CommandText = "DELETE MPemakaianD FROM MPemakaianD INNER JOIN MPemakaian ON MPemakaian.NoID=MPemakaianD.IDHeader WHERE ISNULL(MPemakaian.IsPosted, 0)=0 AND MPemakaian.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE FROM MPemakaian WHERE ISNULL(MPemakaian.IsPosted, 0)=0 AND MPemakaian.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                    Case modMain.FormName.DaftarPenyesuaian
                                        com.CommandText = "DELETE MPenyesuaianD FROM MPenyesuaianD INNER JOIN MPenyesuaian ON MPenyesuaian.NoID=MPenyesuaianD.IDHeader WHERE ISNULL(MPenyesuaian.IsPosted, 0)=0 AND MPenyesuaian.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE FROM MPenyesuaian WHERE ISNULL(MPenyesuaian.IsPosted, 0)=0 AND MPenyesuaian.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                    Case modMain.FormName.DaftarStockOpname
                                        com.CommandText = "DELETE MStockOpnameD FROM MStockOpnameD INNER JOIN MStockOpname ON MStockOpname.NoID=MStockOpnameD.IDHeader WHERE ISNULL(MStockOpname.IsPosted, 0)=0 AND MStockOpname.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE FROM MStockOpname WHERE ISNULL(MStockOpname.IsPosted, 0)=0 AND MStockOpname.NoID=" & NoID
                                        com.ExecuteNonQuery()
                                End Select

                                If com.Transaction IsNot Nothing Then
                                    com.Transaction.Commit()
                                    com.Transaction = Nothing
                                    mnRefresh.PerformClick()
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
        mnEdit.PerformClick()
    End Sub

    Private Sub cmdBaru_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBaru.Click
        mnBaru.PerformClick()
    End Sub

    Public Sub New(ByVal formName As modMain.FormName, ByVal caption As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Text = caption
        Me.formName = formName
        Me.SQL = SQL
        Me.Tag = Me.formName
        Me.Name = Me.formName.ToString
    End Sub

    Private Sub GridView1_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataSourceChanged
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

    Private Sub GridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DoubleClick
        cmdEdit.PerformClick()
    End Sub

    Private Sub GridView1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridView1.MouseDown
        Dim View As GridView = CType(sender, GridView)
        If View Is Nothing Then Return
        ' obtaining hit info
        Dim hitInfo As GridHitInfo = View.CalcHitInfo(New System.Drawing.Point(e.X, e.Y))
        If (e.Button = Windows.Forms.MouseButtons.Right) And (hitInfo.InRow) And _
          (Not View.IsGroupRow(hitInfo.RowHandle)) Then
            PopupMenu1.ShowPopup(Control.MousePosition)
        End If
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
            DateEdit1.EditValue = Now.AddDays(-7)
            DateEdit2.EditValue = Now
            RefreshData(-1)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub mnSaveLayout_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayout.ItemClick
        Using frm As New frmOtorisasi
            Try
                If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    GridView1.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & GridView1.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub mnPosting_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPosting.ItemClick
        Using dlg As New WaitDialogForm("Sedang memosting data ...", NamaAplikasi)
            Dim MyCursor As Cursor = Windows.Forms.Cursor.Current
            Dim NoID As Long = -1
            Dim JSON As JSONResult
            Try
                Cursor = Cursors.WaitCursor
                dlg.Show()
                dlg.Focus()

                For Each iRow In GridView1.GetSelectedRows
                    Select Case formName
                        Case modMain.FormName.DaftarSaldoAwalPiutang
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = PostingSaldoAwalPiutang(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case modMain.FormName.DaftarSaldoAwalHutang
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = PostingSaldoAwalHutang(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case modMain.FormName.DaftarSaldoAwalPersediaan
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = PostingSaldoAwalPersediaan(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case modMain.FormName.DaftarPO
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = PostingPO(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case modMain.FormName.DaftarPembelian
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = PostingBeli(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case modMain.FormName.DaftarReturPembelian
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = PostingReturBeli(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case modMain.FormName.DaftarPenjualan
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = PostingJual(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case modMain.FormName.DaftarReturPenjualan
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = PostingReturJual(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case modMain.FormName.DaftarMutasiGudang
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = PostingMutasiGudang(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case modMain.FormName.DaftarPemakaian
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = PostingPemakaian(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case modMain.FormName.DaftarPenyesuaian
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = PostingPenyesuaian(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case modMain.FormName.DaftarStockOpname
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = PostingStockOpname(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case Else
                            Exit For
                    End Select
                    mnRefresh.PerformClick()
                Next
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                Cursor = MyCursor
            End Try
        End Using
    End Sub

    Private Sub mnUnposting_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnUnposting.ItemClick
        Using dlg As New WaitDialogForm("Sedang mengunposting data ...", NamaAplikasi)
            Dim MyCursor As Cursor = Windows.Forms.Cursor.Current
            Dim NoID As Long = -1
            Dim JSON As JSONResult
            Try
                Cursor = Cursors.WaitCursor
                dlg.Show()
                dlg.Focus()

                For Each iRow In GridView1.GetSelectedRows
                    Select Case formName
                        Case modMain.FormName.DaftarSaldoAwalHutang
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = UnPostingSaldoAwalHutang(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case modMain.FormName.DaftarSaldoAwalPiutang
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = UnPostingSaldoAwalPiutang(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case modMain.FormName.DaftarSaldoAwalPersediaan
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = UnPostingSaldoAwalPersediaan(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case modMain.FormName.DaftarPO
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = UnPostingPO(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case modMain.FormName.DaftarPembelian
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = UnPostingBeli(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case modMain.FormName.DaftarReturPembelian
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = UnPostingReturBeli(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case modMain.FormName.DaftarPenjualan
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = UnPostingJual(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case modMain.FormName.DaftarReturPenjualan
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = UnPostingReturJual(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case modMain.FormName.DaftarMutasiGudang
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = UnPostingMutasiGudang(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case modMain.FormName.DaftarPemakaian
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = UnPostingPemakaian(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case modMain.FormName.DaftarPenyesuaian
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = UnPostingPenyesuaian(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case modMain.FormName.DaftarStockOpname
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            JSON = UnPostingStockOpname(StrKonSQL, [Public].UserLogin, NoID)
                            If Not JSON.JSONResult Then
                                XtraMessageBox.Show(JSON.JSONMessage, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Case Else
                            Exit For
                    End Select
                    mnRefresh.PerformClick()
                Next
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                Cursor = MyCursor
            End Try
        End Using
    End Sub

    Private Sub mnHasilPosting_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHasilPosting.ItemClick
        Using dlg As New WaitDialogForm("Sedang mengambil data ...", NamaAplikasi)
            Dim MyCursor As Cursor = Windows.Forms.Cursor.Current
            Dim NoID As Long = -1
            Try
                Cursor = Cursors.WaitCursor
                dlg.Show()
                dlg.Focus()

                For Each iRow In GridView1.GetSelectedRows
                    Select Case formName
                        Case modMain.FormName.DaftarSaldoAwalPiutang
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            frmHasilPosting = New frmHasilPosting(NoID, 1002)
                            frmHasilPosting.StartPosition = FormStartPosition.CenterScreen
                            frmHasilPosting.Show()
                            frmHasilPosting.Focus()
                        Case modMain.FormName.DaftarSaldoAwalHutang
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            frmHasilPosting = New frmHasilPosting(NoID, 1001)
                            frmHasilPosting.StartPosition = FormStartPosition.CenterScreen
                            frmHasilPosting.Show()
                            frmHasilPosting.Focus()
                        Case modMain.FormName.DaftarSaldoAwalPersediaan
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            frmHasilPosting = New frmHasilPosting(NoID, 1)
                            frmHasilPosting.StartPosition = FormStartPosition.CenterScreen
                            frmHasilPosting.Show()
                            frmHasilPosting.Focus()
                        Case modMain.FormName.DaftarPembelian
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            frmHasilPosting = New frmHasilPosting(NoID, 2)
                            frmHasilPosting.StartPosition = FormStartPosition.CenterScreen
                            frmHasilPosting.Show()
                            frmHasilPosting.Focus()
                        Case modMain.FormName.DaftarReturPembelian
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            frmHasilPosting = New frmHasilPosting(NoID, 3)
                            frmHasilPosting.StartPosition = FormStartPosition.CenterScreen
                            frmHasilPosting.Show()
                            frmHasilPosting.Focus()
                        Case modMain.FormName.DaftarPenjualan
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            frmHasilPosting = New frmHasilPosting(NoID, 6)
                            frmHasilPosting.StartPosition = FormStartPosition.CenterScreen
                            frmHasilPosting.Show()
                            frmHasilPosting.Focus()
                        Case modMain.FormName.DaftarReturPenjualan
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            frmHasilPosting = New frmHasilPosting(NoID, 7)
                            frmHasilPosting.StartPosition = FormStartPosition.CenterScreen
                            frmHasilPosting.Show()
                            frmHasilPosting.Focus()
                        Case modMain.FormName.DaftarMutasiGudang
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            frmHasilPosting = New frmHasilPosting(NoID, 4)
                            frmHasilPosting.StartPosition = FormStartPosition.CenterScreen
                            frmHasilPosting.Show()
                            frmHasilPosting.Focus()
                        Case modMain.FormName.DaftarPemakaian
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            frmHasilPosting = New frmHasilPosting(NoID, 8)
                            frmHasilPosting.StartPosition = FormStartPosition.CenterScreen
                            frmHasilPosting.Show()
                            frmHasilPosting.Focus()
                        Case modMain.FormName.DaftarPenyesuaian
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            frmHasilPosting = New frmHasilPosting(NoID, 14)
                            frmHasilPosting.StartPosition = FormStartPosition.CenterScreen
                            frmHasilPosting.Show()
                            frmHasilPosting.Focus()
                        Case modMain.FormName.DaftarStockOpname
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            frmHasilPosting = New frmHasilPosting(NoID, 20)
                            frmHasilPosting.StartPosition = FormStartPosition.CenterScreen
                            frmHasilPosting.Show()
                            frmHasilPosting.Focus()
                        Case Else
                            Exit For
                    End Select
                Next
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                Cursor = MyCursor
            End Try
        End Using
    End Sub

    Private Sub mnBaru_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnBaru.ItemClick
        Select Case formName
            Case modMain.FormName.DaftarSaldoAwalPiutang
                Using x As New frmEntriSaldoAwalPiutang(-1)
                    Try
                        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                            RefreshData(x.NoID)
                        End If
                    Catch ex As Exception
                        XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End Using
            Case modMain.FormName.DaftarSaldoAwalHutang
                Using x As New frmEntriSaldoAwalHutang(-1)
                    Try
                        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                            RefreshData(x.NoID)
                        End If
                    Catch ex As Exception
                        XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End Using
            Case modMain.FormName.DaftarSaldoAwalPersediaan
                Using x As New frmEntriSaldoAwalPersediaan(-1)
                    Try
                        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                            RefreshData(x.NoID)
                        End If
                    Catch ex As Exception
                        XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End Using
            Case modMain.FormName.DaftarPO
                Dim x As New frmEntriPO(-1)
                x.MdiParent = Me.MdiParent
                x.Show()
                x.Focus()
            Case modMain.FormName.DaftarPembelian
                Dim x As New frmEntriBeli(-1)
                x.MdiParent = Me.MdiParent
                x.Show()
                x.Focus()
            Case modMain.FormName.DaftarReturPembelian
                Dim x As New frmEntriReturBeli(-1)
                x.MdiParent = Me.MdiParent
                x.Show()
                x.Focus()
            Case modMain.FormName.DaftarPenjualan
                Dim x As New frmEntriJual(-1)
                x.MdiParent = Me.MdiParent
                x.Show()
                x.Focus()
            Case modMain.FormName.DaftarReturPenjualan
                Dim x As New frmEntriReturJual(-1)
                x.MdiParent = Me.MdiParent
                x.Show()
                x.Focus()
            Case modMain.FormName.DaftarMutasiGudang
                Dim x As New frmEntriInternal(modMain.FormInternal.MutasiGudang, -1)
                x.MdiParent = Me.MdiParent
                x.Show()
                x.Focus()
            Case modMain.FormName.DaftarPemakaian
                Dim x As New frmEntriInternal(modMain.FormInternal.Pemakaian, -1)
                x.MdiParent = Me.MdiParent
                x.Show()
                x.Focus()
            Case modMain.FormName.DaftarPenyesuaian
                Dim x As New frmEntriInternal(modMain.FormInternal.Penyesuaian, -1)
                x.MdiParent = Me.MdiParent
                x.Show()
                x.Focus()
            Case modMain.FormName.DaftarStockOpname
                Dim x As New frmEntriStockOpname(-1)
                x.MdiParent = Me.MdiParent
                x.Show()
                x.Focus()
        End Select
    End Sub

    Private Sub mnEdit_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnEdit.ItemClick
        Select Case formName
            Case modMain.FormName.DaftarSaldoAwalPiutang
                Using x As New frmEntriSaldoAwalPiutang(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
                    Try
                        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                            RefreshData(x.NoID)
                        End If
                    Catch ex As Exception
                        XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End Using
            Case modMain.FormName.DaftarSaldoAwalHutang
                Using x As New frmEntriSaldoAwalHutang(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
                    Try
                        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                            RefreshData(x.NoID)
                        End If
                    Catch ex As Exception
                        XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End Using
            Case modMain.FormName.DaftarSaldoAwalPersediaan
                Using x As New frmEntriSaldoAwalPersediaan(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
                    Try
                        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                            RefreshData(x.NoID)
                        End If
                    Catch ex As Exception
                        XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End Using
            Case modMain.FormName.DaftarPO
                Dim x As New frmEntriPO(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
                x.MdiParent = Me.MdiParent
                x.Show()
                x.Focus()
            Case modMain.FormName.DaftarPembelian
                Dim x As New frmEntriBeli(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
                x.MdiParent = Me.MdiParent
                x.Show()
                x.Focus()
            Case modMain.FormName.DaftarReturPembelian
                Dim x As New frmEntriReturBeli(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
                x.MdiParent = Me.MdiParent
                x.Show()
                x.Focus()
            Case modMain.FormName.DaftarPenjualan
                Dim x As New frmEntriJual(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
                x.MdiParent = Me.MdiParent
                x.Show()
                x.Focus()
            Case modMain.FormName.DaftarReturPenjualan
                Dim x As New frmEntriReturJual(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
                x.MdiParent = Me.MdiParent
                x.Show()
                x.Focus()
            Case modMain.FormName.DaftarMutasiGudang
                Dim x As New frmEntriInternal(modMain.FormInternal.MutasiGudang, NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
                x.MdiParent = Me.MdiParent
                x.Show()
                x.Focus()
            Case modMain.FormName.DaftarPemakaian
                Dim x As New frmEntriInternal(modMain.FormInternal.Pemakaian, NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
                x.MdiParent = Me.MdiParent
                x.Show()
                x.Focus()
            Case modMain.FormName.DaftarPenyesuaian
                Dim x As New frmEntriInternal(modMain.FormInternal.Penyesuaian, NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
                x.MdiParent = Me.MdiParent
                x.Show()
                x.Focus()
            Case modMain.FormName.DaftarStockOpname
                Dim x As New frmEntriStockOpname(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
                x.MdiParent = Me.MdiParent
                x.Show()
                x.Focus()
        End Select
    End Sub

    Private Sub mnHapus_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHapus.ItemClick
        Dim gridview As DevExpress.XtraGrid.Views.Grid.GridView = Nothing
        gridview = GridView1
        If gridview.RowCount >= 1 Then
            Select Case formName
                Case modMain.FormName.DaftarSaldoAwalPiutang
                    If gridview.GetSelectedRows.Count >= 1 Then
                        If XtraMessageBox.Show("Ingin menghapus data saldo piutang yang dipilih?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                            For Each iRow As Integer In gridview.GetSelectedRows
                                HapusData(NullToLong(gridview.GetRowCellValue(iRow, "NoID")))
                            Next
                        End If
                    Else
                        If XtraMessageBox.Show("Ingin menghapus data saldo piutang " & NullToStr(gridview.GetRowCellValue(gridview.FocusedRowHandle, "Kode")) & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                            HapusData(NullToLong(gridview.GetRowCellValue(gridview.FocusedRowHandle, "NoID")))
                        End If
                    End If
                Case modMain.FormName.DaftarSaldoAwalHutang
                    If gridview.GetSelectedRows.Count >= 1 Then
                        If XtraMessageBox.Show("Ingin menghapus data saldo hutang yang dipilih?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                            For Each iRow As Integer In gridview.GetSelectedRows
                                HapusData(NullToLong(gridview.GetRowCellValue(iRow, "NoID")))
                            Next
                        End If
                    Else
                        If XtraMessageBox.Show("Ingin menghapus data saldo hutang " & NullToStr(gridview.GetRowCellValue(gridview.FocusedRowHandle, "Kode")) & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                            HapusData(NullToLong(gridview.GetRowCellValue(gridview.FocusedRowHandle, "NoID")))
                        End If
                    End If
                Case modMain.FormName.DaftarSaldoAwalPersediaan
                    If gridview.GetSelectedRows.Count >= 1 Then
                        If XtraMessageBox.Show("Ingin menghapus data saldo persediaan yang dipilih?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                            For Each iRow As Integer In gridview.GetSelectedRows
                                HapusData(NullToLong(gridview.GetRowCellValue(iRow, "NoID")))
                            Next
                        End If
                    Else
                        If XtraMessageBox.Show("Ingin menghapus data saldo persediaan " & NullToStr(gridview.GetRowCellValue(gridview.FocusedRowHandle, "Kode")) & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                            HapusData(NullToLong(gridview.GetRowCellValue(gridview.FocusedRowHandle, "NoID")))
                        End If
                    End If
                Case modMain.FormName.DaftarPO
                    If XtraMessageBox.Show("Ingin menghapus data pesanan " & NullToStr(gridview.GetRowCellValue(gridview.FocusedRowHandle, "Kode")) & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        HapusData(NullToLong(gridview.GetRowCellValue(gridview.FocusedRowHandle, "NoID")))
                    End If
                Case modMain.FormName.DaftarPembelian
                    If XtraMessageBox.Show("Ingin menghapus data pembelian " & NullToStr(gridview.GetRowCellValue(gridview.FocusedRowHandle, "Kode")) & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        HapusData(NullToLong(gridview.GetRowCellValue(gridview.FocusedRowHandle, "NoID")))
                    End If
                Case modMain.FormName.DaftarReturPembelian
                    If XtraMessageBox.Show("Ingin menghapus data retur pembelian " & NullToStr(gridview.GetRowCellValue(gridview.FocusedRowHandle, "Kode")) & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        HapusData(NullToLong(gridview.GetRowCellValue(gridview.FocusedRowHandle, "NoID")))
                    End If
                Case modMain.FormName.DaftarPenjualan
                    If XtraMessageBox.Show("Ingin menghapus data penjualan " & NullToStr(gridview.GetRowCellValue(gridview.FocusedRowHandle, "Kode")) & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        HapusData(NullToLong(gridview.GetRowCellValue(gridview.FocusedRowHandle, "NoID")))
                    End If
                Case modMain.FormName.DaftarMutasiGudang
                    If XtraMessageBox.Show("Ingin menghapus data mutasi gudang " & NullToStr(gridview.GetRowCellValue(gridview.FocusedRowHandle, "Kode")) & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        HapusData(NullToLong(gridview.GetRowCellValue(gridview.FocusedRowHandle, "NoID")))
                    End If
                Case modMain.FormName.DaftarPemakaian
                    If XtraMessageBox.Show("Ingin menghapus data pemakaian barang " & NullToStr(gridview.GetRowCellValue(gridview.FocusedRowHandle, "Kode")) & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        HapusData(NullToLong(gridview.GetRowCellValue(gridview.FocusedRowHandle, "NoID")))
                    End If
                Case modMain.FormName.DaftarPenyesuaian
                    If XtraMessageBox.Show("Ingin menghapus data penyesuaian barang " & NullToStr(gridview.GetRowCellValue(gridview.FocusedRowHandle, "Kode")) & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        HapusData(NullToLong(gridview.GetRowCellValue(gridview.FocusedRowHandle, "NoID")))
                    End If
                Case modMain.FormName.DaftarStockOpname
                    If XtraMessageBox.Show("Ingin menghapus data Stock Opname barang " & NullToStr(gridview.GetRowCellValue(gridview.FocusedRowHandle, "Kode")) & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        HapusData(NullToLong(gridview.GetRowCellValue(gridview.FocusedRowHandle, "NoID")))
                    End If
            End Select
        End If
    End Sub

    Private Sub mnPreview_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPreview.ItemClick
        Dim NamaFile As String = "", Judul As String = ""
        Using ds As New DataSet
            Using con As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Try
                            con.Open()
                            com.Connection = con
                            com.CommandTimeout = con.ConnectionTimeout
                            oDA.SelectCommand = com
                            Select Case formName
                                Case modMain.FormName.DaftarSaldoAwalPiutang
                                    For Each eRow As Integer In GridView1.GetSelectedRows
                                        PostingSaldoAwalPiutang(StrKonSQL, UserLogin, NullToLong(GridView1.GetRowCellValue(eRow, "NoID")))
                                    Next

                                    com.CommandText = "spFakturMSaldoAwalPiutang @TglDari, @TglSampai"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@TglDari", SqlDbType.Date)).Value = DateEdit1.EditValue
                                    com.Parameters.Add(New SqlParameter("@TglSampai", SqlDbType.Date)).Value = DateEdit2.EditValue
                                    oDA.Fill(ds, formName.ToString)
                                    NamaFile = "Faktur_MSaldoAwalPiutang.repx"
                                    Judul = "Bukti Saldo Awal Piutang"
                                    ViewXtraReport(Me.MdiParent, IIf(IsEditReport, ActionPrint.Edit, ActionPrint.Preview), Application.StartupPath & "\Report\" & NamaFile, Judul, NamaFile, ds)
                                Case modMain.FormName.DaftarSaldoAwalHutang
                                    For Each eRow As Integer In GridView1.GetSelectedRows
                                        PostingSaldoAwalHutang(StrKonSQL, UserLogin, NullToLong(GridView1.GetRowCellValue(eRow, "NoID")))
                                    Next

                                    com.CommandText = "spFakturMSaldoAwalHutang @TglDari, @TglSampai"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@TglDari", SqlDbType.Date)).Value = DateEdit1.EditValue
                                    com.Parameters.Add(New SqlParameter("@TglSampai", SqlDbType.Date)).Value = DateEdit2.EditValue
                                    oDA.Fill(ds, formName.ToString)
                                    NamaFile = "Faktur_MSaldoAwalHutang.repx"
                                    Judul = "Bukti Saldo Awal Hutang"
                                    ViewXtraReport(Me.MdiParent, IIf(IsEditReport, ActionPrint.Edit, ActionPrint.Preview), Application.StartupPath & "\Report\" & NamaFile, Judul, NamaFile, ds)
                                Case modMain.FormName.DaftarSaldoAwalPersediaan
                                    For Each eRow As Integer In GridView1.GetSelectedRows
                                        PostingSaldoAwalPersediaan(StrKonSQL, UserLogin, NullToLong(GridView1.GetRowCellValue(eRow, "NoID")))
                                    Next

                                    com.CommandText = "spFakturMSaldoAwalPersediaan @TglDari, @TglSampai"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@TglDari", SqlDbType.Date)).Value = DateEdit1.EditValue
                                    com.Parameters.Add(New SqlParameter("@TglSampai", SqlDbType.Date)).Value = DateEdit2.EditValue
                                    oDA.Fill(ds, formName.ToString)
                                    NamaFile = "Faktur_MSaldoAwalPersediaan.repx"
                                    Judul = "Bukti Saldo Awal Periode"
                                    ViewXtraReport(Me.MdiParent, IIf(IsEditReport, ActionPrint.Edit, ActionPrint.Preview), Application.StartupPath & "\Report\" & NamaFile, Judul, NamaFile, ds)
                                Case modMain.FormName.DaftarPO
                                    PostingPO(StrKonSQL, UserLogin, NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))

                                    com.CommandText = "spFakturMPO @NoID"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                                    oDA.Fill(ds, formName.ToString)
                                    NamaFile = "Faktur_MPO.repx"
                                    Judul = "Faktur Pesanan"
                                    ViewXtraReport(Me.MdiParent, IIf(IsEditReport, ActionPrint.Edit, ActionPrint.Preview), Application.StartupPath & "\Report\" & NamaFile, Judul, NamaFile, ds)
                                Case modMain.FormName.DaftarPembelian
                                    PostingBeli(StrKonSQL, UserLogin, NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))

                                    com.CommandText = "spFakturMBeli @NoID"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                                    oDA.Fill(ds, formName.ToString)
                                    NamaFile = "Faktur_MBeli.repx"
                                    Judul = "Faktur Pembelian"
                                    ViewXtraReport(Me.MdiParent, IIf(IsEditReport, ActionPrint.Edit, ActionPrint.Preview), Application.StartupPath & "\Report\" & NamaFile, Judul, NamaFile, ds)
                                Case modMain.FormName.DaftarReturPembelian
                                    PostingReturBeli(StrKonSQL, UserLogin, NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))

                                    com.CommandText = "spFakturMReturBeli @NoID"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                                    oDA.Fill(ds, formName.ToString)
                                    NamaFile = "Faktur_MReturBeli.repx"
                                    Judul = "Faktur Retur Pembelian"
                                    ViewXtraReport(Me.MdiParent, IIf(IsEditReport, ActionPrint.Edit, ActionPrint.Preview), Application.StartupPath & "\Report\" & NamaFile, Judul, NamaFile, ds)
                                Case modMain.FormName.DaftarPenjualan
                                    PostingJual(StrKonSQL, UserLogin, NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))

                                    com.CommandText = "spFakturMJual @NoID"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                                    oDA.Fill(ds, formName.ToString)
                                    If ds.Tables(formName.ToString & "2") IsNot Nothing AndAlso ds.Tables(formName.ToString) IsNot Nothing Then
                                        ds.Relations.Add(New DataRelation("IX_IDSales", ds.Tables(formName.ToString & "2").Columns("NoID"), ds.Tables(formName.ToString).Columns("IDHeader")))
                                    End If
                                    If ds.Tables(formName.ToString & "2") IsNot Nothing AndAlso ds.Tables(formName.ToString & "1") IsNot Nothing Then
                                        ds.Relations.Add(New DataRelation("IX_IDSalesDBayar", ds.Tables(formName.ToString & "2").Columns("NoID"), ds.Tables(formName.ToString & "1").Columns("IDHeader")))
                                    End If
                                    NamaFile = "Faktur_MJual.repx"
                                    Judul = "Faktur Penjualan"
                                    ViewXtraReport(Me.MdiParent, IIf(IsEditReport, ActionPrint.Edit, ActionPrint.Preview), Application.StartupPath & "\Report\" & NamaFile, Judul, NamaFile, ds)
                                Case modMain.FormName.DaftarReturPenjualan
                                    PostingReturJual(StrKonSQL, UserLogin, NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))

                                    com.CommandText = "spFakturMReturJual @NoID"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                                    oDA.Fill(ds, formName.ToString)
                                    NamaFile = "Faktur_MReturJual.repx"
                                    Judul = "Faktur Retur Penjualan"
                                    ViewXtraReport(Me.MdiParent, IIf(IsEditReport, ActionPrint.Edit, ActionPrint.Preview), Application.StartupPath & "\Report\" & NamaFile, Judul, NamaFile, ds)
                                Case modMain.FormName.DaftarMutasiGudang
                                    PostingMutasiGudang(StrKonSQL, UserLogin, NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))

                                    com.CommandText = "spFakturMMutasiGudang @NoID"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                                    oDA.Fill(ds, formName.ToString)
                                    NamaFile = "Faktur_MMutasiGudang.repx"
                                    Judul = "Faktur Mutasi Gudang"
                                    ViewXtraReport(Me.MdiParent, IIf(IsEditReport, ActionPrint.Edit, ActionPrint.Preview), Application.StartupPath & "\Report\" & NamaFile, Judul, NamaFile, ds)
                                Case modMain.FormName.DaftarPemakaian
                                    PostingPemakaian(StrKonSQL, UserLogin, NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))

                                    com.CommandText = "spFakturMPemakaian @NoID"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                                    oDA.Fill(ds, formName.ToString)
                                    NamaFile = "Faktur_MPemakaian.repx"
                                    Judul = "Faktur Pemakaian"
                                    ViewXtraReport(Me.MdiParent, IIf(IsEditReport, ActionPrint.Edit, ActionPrint.Preview), Application.StartupPath & "\Report\" & NamaFile, Judul, NamaFile, ds)
                                Case modMain.FormName.DaftarPenyesuaian
                                    PostingPenyesuaian(StrKonSQL, UserLogin, NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))

                                    com.CommandText = "spFakturMPenyesuaian @NoID"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                                    oDA.Fill(ds, formName.ToString)
                                    NamaFile = "Faktur_MPenyesuaian.repx"
                                    Judul = "Faktur Penyesuaian"
                                    ViewXtraReport(Me.MdiParent, IIf(IsEditReport, ActionPrint.Edit, ActionPrint.Preview), Application.StartupPath & "\Report\" & NamaFile, Judul, NamaFile, ds)
                                Case modMain.FormName.DaftarStockOpname
                                    PostingStockOpname(StrKonSQL, UserLogin, NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))

                                    com.CommandText = "spFakturMStockOpname @NoID"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                                    oDA.Fill(ds, formName.ToString)
                                    NamaFile = "Faktur_MStockOpname.repx"
                                    Judul = "Faktur Stock Opname"
                                    ViewXtraReport(Me.MdiParent, IIf(IsEditReport, ActionPrint.Edit, ActionPrint.Preview), Application.StartupPath & "\Report\" & NamaFile, Judul, NamaFile, ds)
                            End Select
                        Catch ex As Exception
                            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Private Sub mnRefresh_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        RefreshData(-1)
    End Sub

    Private Sub mnTutup_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnTutup.ItemClick
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class
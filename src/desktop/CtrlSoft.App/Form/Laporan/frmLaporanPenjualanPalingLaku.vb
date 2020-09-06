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
Imports CtrlSoft.App.Utils
Imports CtrlSoft.App.CetakDX
Imports DevExpress.XtraBars
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo

Public Class frmLaporanPenjualanPalingLaku
    Private formName As String
    Private ds As New DataSet

    Private SQL As String, TglSampai As Date, TglDari As Date

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        mnTutup.PerformClick()
    End Sub

    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        mnRefresh.PerformClick()
    End Sub

    'Private Function IsValidasi() As Boolean
    '    DxErrorProvider1.ClearErrors()
    '    If txtGudang.Text = "" Then
    '        DxErrorProvider1.SetError(txtGudang, "Gudang Harus dipilih!", DXErrorProvider.ErrorType.Critical)
    '    End If

    '    Return Not DxErrorProvider1.HasErrors
    'End Function

    Public Sub RefreshData(ByVal NoID As Long)
        'IsValidasi()
        Using dsGudang As New DataSet
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
                                    com.CommandTimeout = cn.ConnectionTimeout
                                    oDA.SelectCommand = com

                                    SQL = "DECLARE @TglDari AS DATE= '" & DateEdit1.DateTime.ToString("yyyy-MM-dd") & "';" & vbCrLf & _
                                            "DECLARE @TglSampai AS DATE= '" & DateEdit2.DateTime.ToString("yyyy-MM-dd") & "';" & vbCrLf & _
                                            "SELECT MJualD.NoID, " & vbCrLf & _
                                            "MJual.Kode, " & vbCrLf & _
                                            "MCustomer.Nama AS Customer, " & vbCrLf & _
                                            "MJual.Tanggal, " & vbCrLf & _
                                            "MBarangD.Barcode, " & vbCrLf & _
                                            "MBarang.Kode KodeBarang, " & vbCrLf & _
                                            "MBarang.Nama NamaBarang," & vbCrLf & _
                                            "MJualD.Qty*MJualD.Konversi AS QtyJual," & vbCrLf & _
                                            "CASE" & vbCrLf & _
                                            "WHEN ISNULL(MJualD.Qty, 0) * ISNULL(MJualD.Konversi, 0) = 0" & vbCrLf & _
                                            "THEN 0" & vbCrLf & _
                                            "ELSE MJualD.Jumlah / (ISNULL(MJualD.Qty, 0) * ISNULL(MJualD.Konversi, 0))" & vbCrLf & _
                                            "END AS HargaNetto, " & vbCrLf & _
                                            "MSupplier1.Nama AS Supplier1, " & vbCrLf & _
                                            "MSupplier2.Nama AS Supplier2, " & vbCrLf & _
                                            "MSupplier3.Nama AS Supplier3, " & vbCrLf & _
                                            "MPersediaan.Persediaan AS Modal, " & vbCrLf & _
                                            "MJualD.Jumlah, " & vbCrLf & _
                                            "ISNULL(MJualD.Jumlah, 0) - ISNULL(MPersediaan.Persediaan, 0) AS LabaKotor," & vbCrLf & _
                                            "CASE" & vbCrLf & _
                                            "WHEN ISNULL(MJualD.Qty, 0) * ISNULL(MJualD.Konversi, 0) = 0" & vbCrLf & _
                                            "THEN 0" & vbCrLf & _
                                            "ELSE ISNULL(MPersediaan.Persediaan, 0) / (ISNULL(MJualD.Qty, 0) * ISNULL(MJualD.Konversi, 0))" & vbCrLf & _
                                            "END AS HPP," & vbCrLf & _
                                            "CASE" & vbCrLf & _
                                            "WHEN ISNULL(MJualD.Jumlah, 0) = 0" & vbCrLf & _
                                            "THEN 0" & vbCrLf & _
                                            "ELSE(ISNULL(MJualD.Jumlah, 0) - ISNULL(MPersediaan.Persediaan, 0)) / ISNULL(MJualD.Jumlah, 0) * 100" & vbCrLf & _
                                            "END AS ProvitMargin," & vbCrLf & _
                                            "CASE" & vbCrLf & _
                                            "WHEN ISNULL(MPersediaan.Persediaan, 0) = 0" & vbCrLf & _
                                            "THEN 0" & vbCrLf & _
                                            "ELSE(ISNULL(MJualD.Jumlah, 0) - ISNULL(MPersediaan.Persediaan, 0)) / ISNULL(MPersediaan.Persediaan, 0) * 100" & vbCrLf & _
                                            "END AS ProsenUp" & vbCrLf & _
                                            "FROM MJualD(NOLOCK)" & vbCrLf & _
                                            "INNER JOIN MJual(NOLOCK) ON MJual.NoID = MJualD.IDHeader" & vbCrLf & _
                                            "INNER JOIN MAlamat MCustomer(NOLOCK) ON MJual.IDCustomer = MCustomer.NoID" & vbCrLf & _
                                            "LEFT JOIN MBarang(NOLOCK) ON MBarang.NoID = MJualD.IDBarang" & vbCrLf & _
                                            "LEFT JOIN MBarangD(NOLOCK) ON MBarangD.NoID = MJualD.IDBarangD" & vbCrLf & _
                                            "LEFT JOIN MKategori(NOLOCK) ON MKategori.NoID = MBarang.IDKategori" & vbCrLf & _
                                            "LEFT JOIN MAlamat MSupplier1(NOLOCK) ON MSupplier1.NoID = MBarang.IDSupplier1" & vbCrLf & _
                                            "LEFT JOIN MAlamat MSupplier2(NOLOCK) ON MSupplier2.NoID = MBarang.IDSupplier2" & vbCrLf & _
                                            "LEFT JOIN MAlamat MSupplier3(NOLOCK) ON MSupplier3.NoID = MBarang.IDSupplier3" & vbCrLf & _
                                            "LEFT JOIN" & vbCrLf & _
                                            "(" & vbCrLf & _
                                            "SELECT MKartuStok.IDTransaksiD, " & vbCrLf & _
                                            "MKartuStok.IDTransaksi, " & vbCrLf & _
                                            "SUM(MKartuStok.Kredit - MKartuStok.Debet) AS Persediaan" & vbCrLf & _
                                            "FROM MKartuStok(NOLOCK)" & vbCrLf & _
                                            "WHERE MKartuStok.IDJenisTransaksi = 6" & vbCrLf & _
                                            "AND CONVERT(DATE, MKartuStok.Tanggal) >= @TglDari" & vbCrLf & _
                                            "AND CONVERT(DATE, MKartuStok.Tanggal) <= @TglSampai" & vbCrLf & _
                                            "GROUP BY MKartuStok.IDTransaksiD, " & vbCrLf & _
                                            "MKartuStok.IDTransaksi" & vbCrLf & _
                                            ") AS MPersediaan ON MPersediaan.IDTransaksi = MJualD.IDHeader" & vbCrLf & _
                                            "AND MPersediaan.IDTransaksiD = MJualD.NoID" & vbCrLf & _
                                            "WHERE MJual.IsPosted = 1" & vbCrLf & _
                                            "AND CONVERT(DATE, MJual.Tanggal) >= @TglDari" & vbCrLf & _
                                            "AND CONVERT(DATE, MJual.Tanggal) <= @TglSampai"
                                    If Not ckTdkAktif.Checked Then
                                        SQL &= " AND CONVERT(BIT, CASE WHEN MBarang.IsActive=1 AND MBarangD.IsActive=1 THEN 1 ELSE 0 END)=1"
                                    End If
                                    If NullToLong(txtKategori.EditValue) >= 1 Then
                                        SQL &= " AND MBarang.IDKategori=" & NullToLong(txtKategori.EditValue)
                                    End If
                                    If NullToLong(txtSupplier.EditValue) >= 1 Then
                                        SQL &= " AND (MBarang.IDSupplier1=" & NullToLong(txtSupplier.EditValue) & " OR MBarang.IDSupplier2=" & NullToLong(txtSupplier.EditValue) & " OR MBarang.IDSupplier3=" & NullToLong(txtSupplier.EditValue) & ")"
                                    End If
                                    If NullToLong(txtMerk.EditValue) >= 1 Then
                                        SQL &= " AND MBarang.IDMerk=" & NullToLong(txtMerk.EditValue)
                                    End If
                                    If NullToLong(txtGudang.EditValue) >= 1 Then
                                        SQL &= " AND MJual.IDGudang=" & NullToLong(txtGudang.EditValue)
                                    End If

                                    com.CommandText = SQL
                                    oDA.Fill(ds, "MBarang")
                                    BindingSource1.DataSource = ds.Tables("MBarang")

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
        End Using
    End Sub

    Private Sub cmdCetak_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCetak.Click
        mnPreview.PerformClick()
    End Sub

    Public Sub New(ByVal formName As String, ByVal caption As String, ByVal TglDari As Date, ByVal TglSampai As Date)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Text = caption
        Me.formName = formName
        Me.SQL = SQL
        Me.Tag = Me.formName
        Me.Name = Me.formName
        Me.TglDari = TglDari
        Me.TglSampai = TglSampai
    End Sub

    Private Sub GridView1_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataSourceChanged
        With sender
            If System.IO.File.Exists(Utils.SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml") Then
                .RestoreLayoutFromXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml")
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
            txtKategori.Properties.DataSource = Repository.RepSQLServer.GetListKategori
            txtKategori.Properties.DisplayMember = "Nama"
            txtKategori.Properties.ValueMember = "NoID"
            txtKategori.EditValue = -1

            txtSupplier.Properties.DataSource = Repository.RepSQLServer.GetListSupplier
            txtSupplier.Properties.DisplayMember = "Nama"
            txtSupplier.Properties.ValueMember = "NoID"
            txtSupplier.EditValue = -1

            txtMerk.Properties.DataSource = Repository.RepSQLServer.GetListMerk
            txtMerk.Properties.DisplayMember = "Nama"
            txtMerk.Properties.ValueMember = "NoID"
            txtMerk.EditValue = -1

            txtGudang.Properties.DataSource = Repository.RepSQLServer.GetListGudang
            txtGudang.Properties.DisplayMember = "Nama"
            txtGudang.Properties.ValueMember = "NoID"
            txtGudang.EditValue = -1

            DateEdit1.DateTime = TglDari
            DateEdit2.DateTime = TglSampai

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
                    GridView1.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & GridView1.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub GridView1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridView1.MouseDown
        'Dim View As GridView = CType(sender, GridView)
        'If View Is Nothing Then Return
        '' obtaining hit info
        'Dim hitInfo As GridHitInfo = View.CalcHitInfo(New System.Drawing.Point(e.X, e.Y))
        'If (e.Button = Windows.Forms.MouseButtons.Right) And (hitInfo.InRow) And _
        '  (Not View.IsGroupRow(hitInfo.RowHandle)) Then
        '    PopupMenu1.ShowPopup(Control.MousePosition)
        'End If
    End Sub

    Private WithEvents frmLogHistoryPerubahanHarga As New frmLogPerubahanHarga(-1)
    Private Sub mnHistoryHarga_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHistoryHarga.ItemClick
        Using dlg As New WaitDialogForm("Sedang mengambil data ...", NamaAplikasi)
            Dim MyCursor As Cursor = Windows.Forms.Cursor.Current
            Dim NoID As Long = -1
            Try
                Cursor = Cursors.WaitCursor
                dlg.Show()
                dlg.Focus()

                NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                frmLogHistoryPerubahanHarga = New frmLogPerubahanHarga(NoID)
                frmLogHistoryPerubahanHarga.StartPosition = FormStartPosition.CenterScreen
                frmLogHistoryPerubahanHarga.Show()
                frmLogHistoryPerubahanHarga.Focus()
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                Cursor = MyCursor
            End Try
        End Using
    End Sub

    Private Sub GridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DoubleClick

    End Sub

    Private Sub mnHitungUlangSaldo_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHitungUlangSaldo.ItemClick
        Using dlg As New WaitDialogForm("Sedang merubah data ...", NamaAplikasi)
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

                                com.CommandText = "DELETE T FROM TSaldoStok T INNER JOIN MBarang d ON d.NoID = T.IDBarang"
                                com.ExecuteNonQuery()
                                com.CommandText = "INSERT INTO TSaldoStok (" & vbCrLf & _
                                                  "IDBarang" & vbCrLf & _
                                                  ",IDGudang" & vbCrLf & _
                                                  ",SaldoAkhir" & vbCrLf & _
                                                  ")" & vbCrLf & _
                                                  "Select d.NoID" & vbCrLf & _
                                                  ",MSaldoStok.IDGudang" & vbCrLf & _
                                                  ",ISNULL(MSaldoStok.Saldo, 0)" & vbCrLf & _
                                                  "FROM MBarang d" & vbCrLf & _
                                                  "RIGHT JOIN (" & vbCrLf & _
                                                  "Select IDBarang" & vbCrLf & _
                                                  ",IDGudang" & vbCrLf & _
                                                  ",SUM(Konversi * (QtyMasuk - QtyKeluar)) AS Saldo" & vbCrLf & _
                                                  "FROM MKartuStok(NOLOCK)" & vbCrLf & _
                                                  "GROUP BY IDBarang" & vbCrLf & _
                                                  ",IDGudang" & vbCrLf & _
                                                  ") MSaldoStok ON MSaldoStok.IDBarang = d.NoID"
                                com.ExecuteNonQuery()

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

    Private Sub mnTutup_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnTutup.ItemClick
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub mnRefresh_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        RefreshData(-1)
    End Sub

    Private Sub mnPreview_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPreview.ItemClick
        Dim NamaFile As String = ""
        NamaFile = Application.StartupPath & "\Report\Laporan_LabaKotorPenjualan.repx"
        Dim CalculateFields As New List(Of Model.CetakDX.CalculateFields)
        CalculateFields.Add(New Model.CetakDX.CalculateFields With {.Name = "FilterGudang", _
                                                                    .Type = Model.CetakDX.CalculateFields.iType.String, _
                                                                    .Value = IIf(NullToLong(txtGudang.EditValue) >= 1, txtGudang.Text, "ALL")})
        CalculateFields.Add(New Model.CetakDX.CalculateFields With {.Name = "TglDari", _
                                                                    .Type = Model.CetakDX.CalculateFields.iType.VariantDateTime, _
                                                                    .Value = DateEdit1.DateTime.ToString("yyyy-MM-dd HH:mm:ss")})
        CalculateFields.Add(New Model.CetakDX.CalculateFields With {.Name = "TglSampai", _
                                                                    .Type = Model.CetakDX.CalculateFields.iType.VariantDateTime, _
                                                                    .Value = DateEdit2.DateTime.ToString("yyyy-MM-dd HH:mm:ss")})
        CalculateFields.Add(New Model.CetakDX.CalculateFields With {.Name = "FilterKategori", _
                                                                    .Type = Model.CetakDX.CalculateFields.iType.String, _
                                                                    .Value = IIf(NullToLong(txtKategori.EditValue) >= 1, txtKategori.Text, "ALL")})
        CalculateFields.Add(New Model.CetakDX.CalculateFields With {.Name = "FilterSupplier", _
                                                                    .Type = Model.CetakDX.CalculateFields.iType.String, _
                                                                    .Value = IIf(NullToLong(txtSupplier.EditValue) >= 1, txtSupplier.Text, "ALL")})
        CalculateFields.Add(New Model.CetakDX.CalculateFields With {.Name = "FilterMerk", _
                                                                    .Type = Model.CetakDX.CalculateFields.iType.String, _
                                                                    .Value = IIf(NullToLong(txtMerk.EditValue) >= 1, txtMerk.Text, "ALL")})
        ViewXtraReport(Me.MdiParent, IIf(IsEditReport, ActionPrint.Edit, ActionPrint.Preview), NamaFile, "Laporan Laba Kotor Penjualan", "Laporan_LabaKotorPenjualan.repx", Me.ds, , CalculateFields)

    End Sub
End Class
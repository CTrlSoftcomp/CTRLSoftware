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

Public Class frmLaporanSaldoStok
    Private formName As String
    Private ds As New DataSet

    Private SQL As String, TglSampai As Date

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

    Public Sub RefreshData(ByVal NoID As Long)
        Dim NamaFieldGudang As String = "", TotalFieldGudang As String = ""
        Dim NamaNilaiGudang As String = "", TotalNilaiGudang As String = ""
        Dim TmpNamaFieldGudang As String = "", TmpTotalNilaiGudang As String = ""
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
                                    oDA.SelectCommand = com

                                    com.CommandText = "SELECT * FROM MGudang WHERE IsActive=1"
                                    oDA.Fill(dsGudang, "MGudang")

                                    NamaFieldGudang = ""
                                    TotalFieldGudang = ""
                                    NamaNilaiGudang = ""
                                    TotalNilaiGudang = ""
                                    For Each iRow As DataRow In dsGudang.Tables("MGudang").Rows
                                        NamaFieldGudang &= IIf(NamaFieldGudang = "", "", ", ") & "[" & iRow.Item("Kode") & "]"
                                        TotalFieldGudang &= IIf(TotalFieldGudang = "", "", "+") & "ISNULL([" & iRow.Item("Kode") & "], 0.0)"
                                        TmpNamaFieldGudang &= IIf(TmpNamaFieldGudang = "", "", ", ") & "ISNULL(SUM([" & iRow.Item("Kode") & "]), 0.0) AS [" & iRow.Item("Kode") & "]"

                                        NamaNilaiGudang &= IIf(NamaNilaiGudang = "", "", ", ") & "[Nilai " & iRow.Item("Kode") & "]"
                                        TotalNilaiGudang &= IIf(TotalNilaiGudang = "", "", "+") & "ISNULL([Nilai " & iRow.Item("Kode") & "], 0.0)"
                                        TmpTotalNilaiGudang &= IIf(TmpTotalNilaiGudang = "", "", ", ") & "ISNULL(SUM([Nilai " & iRow.Item("Kode") & "]), 0.0) AS [Nilai " & iRow.Item("Kode") & "]"
                                    Next

                                    SQL = "SELECT DISTINCT MBarang.DefaultBarcode Barcode, MBarang.NoID, MBarang.Kode, MBarang.Nama, MBarang.Alias, MBarang.Keterangan, " & vbCrLf & _
                                            "CONVERT(BIT, CASE WHEN MBarang.IsActive=1 AND MBarangD.IsActive=1 THEN 1 ELSE 0 END) Aktif, " & vbCrLf & _
                                            "MBarang.IsiCtn, MBarang.HargaBeliPcs, " & vbCrLf & _
                                            "MSupplier1.Nama Supplier, MSupplier2.Nama Supplier2, MSupplier3.Nama Supplier3, MTypePajak.TypePajak, MMerk.Nama AS [Merk], MKategori.Nama AS Kategori, " & vbCrLf & _
                                            "MHistoryBeli.PembelianTerakhir, DATEDIFF(DAY, MHistoryBeli.PembelianTerakhir, '" & DateEdit1.DateTime.ToString("yyyy-MM-dd") & "') AS UmurBarang " & vbCrLf & _
                                            IIf(NamaFieldGudang = "", "", ", " & NamaFieldGudang.Replace("[", "TSaldoStok.[")) & " " & IIf(TotalFieldGudang = "", "", ", " & TotalFieldGudang.Replace("[", "TSaldoStok.[") & " AS TotalQty") & " " & vbCrLf & _
                                            IIf(NamaNilaiGudang = "", "", ", " & NamaNilaiGudang.Replace("[", "TSaldoStok.[")) & " " & IIf(TotalNilaiGudang = "", "", ", " & TotalNilaiGudang.Replace("[", "TSaldoStok.[") & " AS NilaiPersediaan") & " " & vbCrLf & _
                                            "FROM MBarang (NOLOCK)" & vbCrLf & _
                                            "INNER JOIN MBarangD (NOLOCK) ON MBarang.NoID=MBarangD.IDBarang" & vbCrLf & _
                                            "LEFT JOIN MKategori (NOLOCK) ON MKategori.NoID=MBarang.IDKategori" & vbCrLf & _
                                            "LEFT JOIN MMerk (NOLOCK) ON MMerk.NoID=MBarang.IDMerk" & vbCrLf & _
                                            "LEFT JOIN MTypePajak (NOLOCK) ON MTypePajak.NoID=MBarang.IDTypePajak" & vbCrLf & _
                                            "LEFT JOIN MAlamat MSupplier1 (NOLOCK) ON MSupplier1.NoID=MBarang.IDSupplier1" & vbCrLf & _
                                            "LEFT JOIN MAlamat MSupplier2 (NOLOCK) ON MSupplier2.NoID=MBarang.IDSupplier2" & vbCrLf & _
                                            "LEFT JOIN MAlamat MSupplier3 (NOLOCK) ON MSupplier3.NoID=MBarang.IDSupplier3" & vbCrLf & _
                                            "LEFT JOIN (SELECT MAX(MBeli.Tanggal) AS PembelianTerakhir, MBeliD.IDBarang" & vbCrLf & _
                                            "FROM MBeli(NOLOCK)" & vbCrLf & _
                                            "INNER JOIN MBeliD(NOLOCK) ON MBeli.NoID=MBeliD.IDHeader" & vbCrLf & _
                                            "WHERE CONVERT(DATE, MBeli.Tanggal)<='" & DateEdit1.DateTime.ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                                            "GROUP BY MBeliD.IDBarang) AS MHistoryBeli ON MHistoryBeli.IDBarang=MBarang.NoID" & vbCrLf & _
                                            IIf(NamaFieldGudang = "" AndAlso TotalFieldGudang = "", "", "LEFT JOIN (" & vbCrLf & _
                                            "SELECT IDBarang, " & TmpNamaFieldGudang & ", " & TmpTotalNilaiGudang & " FROM (SELECT * FROM (" & vbCrLf & _
                                            "SELECT A.IDBarang, B.Kode, 'Nilai ' + B.Kode AS Nilai, SUM(A.Konversi*(A.QtyMasuk-A.QtyKeluar)) AS SaldoAkhir, SUM(A.Debet-A.Kredit) AS NilaiAkhir" & vbCrLf & _
                                            "FROM MKartuStok (NOLOCK) A" & vbCrLf & _
                                            "INNER JOIN MGudang (NOLOCK) B ON B.NoID=A.IDGudang" & vbCrLf & _
                                            "WHERE CONVERT(DATE, A.Tanggal)<='" & DateEdit1.DateTime.ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                                            "GROUP BY A.IDBarang, B.Kode) T PIVOT (SUM(SaldoAkhir) FOR Kode IN (" & NamaFieldGudang & ")) AS PVT PIVOT (SUM(NilaiAkhir) FOR Nilai IN (" & NamaNilaiGudang & ")) AS PVT2) AS TmpSource GROUP BY IDBarang) TSaldoStok ON TSaldoStok.IDBarang=MBarang.NoID") & vbCrLf & _
                                            "WHERE 1=1 "
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
        mnCetak.PerformClick()
    End Sub

    Public Sub New(ByVal formName As String, ByVal caption As String, ByVal TglSampai As Date)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Text = caption
        Me.formName = formName
        Me.SQL = SQL
        Me.Tag = Me.formName
        Me.Name = Me.formName
        Me.TglSampai = TglSampai
    End Sub

    Private Sub GridView1_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataSourceChanged, _
    gvKategori.DataSourceChanged, gvSupplier.DataSourceChanged, gvMerk.DataSourceChanged
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

    Private Sub frmDaftarMaster_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            DateEdit1.DateTime = TglSampai

            cmdCetak.ImageList = frmMain.ICButtons
            cmdRefresh.ImageList = frmMain.ICButtons
            cmdTutup.ImageList = frmMain.ICButtons

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

            LabelControl1.Text = Me.Text
            RefreshData(-1)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub mnSaveLayout_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayout.ItemClick
        Using frm As New frmOtorisasi
            Try
                If frm.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                    GridView1.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & GridView1.Name & ".xml")
                    gvKategori.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & gvKategori.Name & ".xml")
                    gvSupplier.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & gvSupplier.Name & ".xml")
                    gvMerk.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & gvMerk.Name & ".xml")
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
        Dim View As GridView = CType(sender, GridView)
        If View Is Nothing Then Return
        ' obtaining hit info
        Dim hitInfo As GridHitInfo = View.CalcHitInfo(New System.Drawing.Point(e.X, e.Y))
        If (e.Button = System.Windows.Forms.MouseButtons.Right) And (hitInfo.InRow) And
          (Not View.IsGroupRow(hitInfo.RowHandle)) Then
            PopupMenu1.ShowPopup(Control.MousePosition)
        End If
    End Sub

    Private WithEvents frmLogHistoryPerubahanHarga As New frmLogPerubahanHarga(-1)
    Private Sub mnHistoryHarga_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHistoryHarga.ItemClick
        Using dlg As New WaitDialogForm("Sedang mengambil data ...", NamaAplikasi)
            Dim MyCursor As Cursor = System.Windows.Forms.Cursor.Current
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
                                com.CommandText = "INSERT INTO TSaldoStok (" & vbCrLf &
                                                  "IDBarang" & vbCrLf &
                                                  ",IDGudang" & vbCrLf &
                                                  ",SaldoAkhir" & vbCrLf &
                                                  ")" & vbCrLf &
                                                  "Select d.NoID" & vbCrLf &
                                                  ",MSaldoStok.IDGudang" & vbCrLf &
                                                  ",ISNULL(MSaldoStok.Saldo, 0)" & vbCrLf &
                                                  "FROM MBarang d" & vbCrLf &
                                                  "RIGHT JOIN (" & vbCrLf &
                                                  "Select IDBarang" & vbCrLf &
                                                  ",IDGudang" & vbCrLf &
                                                  ",SUM(Konversi * (QtyMasuk - QtyKeluar)) AS Saldo" & vbCrLf &
                                                  "FROM MKartuStok(NOLOCK)" & vbCrLf &
                                                  "GROUP BY IDBarang" & vbCrLf &
                                                  ",IDGudang" & vbCrLf &
                                                  ") MSaldoStok ON MSaldoStok.IDBarang = d.NoID"
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

    Private Sub mnTutup_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnTutup.ItemClick
        DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub mnRefresh_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        RefreshData(-1)
    End Sub

    Private Sub mnCetak_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnCetak.ItemClick
        Dim NamaFile As String = ""
        NamaFile = Application.StartupPath & "\Report\Laporan_SaldoStok.repx"
        Dim CalculateFields As New List(Of Model.CetakDX.CalculateFields)
        CalculateFields.Add(New Model.CetakDX.CalculateFields With {.Name = "TglSampai", _
                                                                    .Type = Model.CetakDX.CalculateFields.iType.VariantDateTime, _
                                                                    .Value = DateEdit1.DateTime.ToString("yyyy-MM-dd HH:mm:ss")})
        CalculateFields.Add(New Model.CetakDX.CalculateFields With {.Name = "FilterKategori", _
                                                                    .Type = Model.CetakDX.CalculateFields.iType.String, _
                                                                    .Value = IIf(NullToLong(txtKategori.EditValue) >= 1, txtKategori.Text, "ALL")})
        CalculateFields.Add(New Model.CetakDX.CalculateFields With {.Name = "FilterSupplier", _
                                                                    .Type = Model.CetakDX.CalculateFields.iType.String, _
                                                                    .Value = IIf(NullToLong(txtSupplier.EditValue) >= 1, txtSupplier.Text, "ALL")})
        CalculateFields.Add(New Model.CetakDX.CalculateFields With {.Name = "FilterMerk", _
                                                                    .Type = Model.CetakDX.CalculateFields.iType.String, _
                                                                    .Value = IIf(NullToLong(txtMerk.EditValue) >= 1, txtMerk.Text, "ALL")})
        ViewXtraReport(Me.MdiParent, IIf(IsEditReport, ActionPrint.Edit, ActionPrint.Preview), NamaFile, "Laporan Saldo Stok", "Laporan_SaldoStok.repx", Me.ds, , CalculateFields)

    End Sub
End Class
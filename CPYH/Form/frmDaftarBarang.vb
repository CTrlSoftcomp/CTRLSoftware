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
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo

Public Class frmDaftarBarang
    Private formName As String
    Private ds As New DataSet

    Private SQL As String

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
                                    For Each iRow As DataRow In dsGudang.Tables("MGudang").Rows
                                        NamaFieldGudang &= IIf(NamaFieldGudang = "", "", ", ") & "[" & iRow.Item("Kode") & "]"
                                        TotalFieldGudang &= IIf(TotalFieldGudang = "", "", "+") & "ISNULL([" & iRow.Item("Kode") & "], 0.0)"
                                    Next

                                    SQL = "SELECT MBarangD.Barcode, MBarangD.Konversi, MBarang.NoID, MBarangD.NoID IDBarangD, MBarang.Kode, MBarang.Nama, MBarang.Alias, MBarang.Keterangan, " & vbCrLf & _
                                            "CONVERT(BIT, CASE WHEN MBarang.IsActive=1 AND MBarangD.IsActive=1 THEN 1 ELSE 0 END) Aktif, MSatuan.Kode AS Satuan, " & vbCrLf & _
                                            "MBarang.IsiCtn, MBarang.HargaBeli, MBarang.DiscProsen1, MBarang.DiscProsen2, MBarang.DiscProsen3, MBarang.DiscProsen4, MBarang.DiscProsen5, MBarang.DiscRp, MBarang.HargaBeliPcs, " & vbCrLf & _
                                            "MBarangD.ProsenUpA MarginRetail, MBarangD.HargaJualA HargaRetail, MBarangD.ProsenUpB MarginGrosir, MBarangD.HargaJualB HargaGrosir," & vbCrLf & _
                                            "MSupplier1.Nama Supplier, MSupplier2.Nama Supplier2, MSupplier3.Nama Supplier3, MTypePajak.TypePajak, MMerk.Nama AS [Merk], MKategori.Nama AS Kategori " & IIf(NamaFieldGudang = "", "", ", " & NamaFieldGudang.Replace("[", "TSaldoStok.[")) & " " & IIf(TotalFieldGudang = "", "", ", " & TotalFieldGudang.Replace("[", "TSaldoStok.[") & " AS TotalQty") & " " & vbCrLf & _
                                            "FROM MBarang (NOLOCK)" & vbCrLf & _
                                            "INNER JOIN MBarangD (NOLOCK) ON MBarang.NoID=MBarangD.IDBarang" & vbCrLf & _
                                            "LEFT JOIN MKategori (NOLOCK) ON MKategori.NoID=MBarang.IDKategori" & vbCrLf & _
                                            "LEFT JOIN MMerk (NOLOCK) ON MMerk.NoID=MBarang.IDMerk" & vbCrLf & _
                                            "LEFT JOIN MTypePajak (NOLOCK) ON MTypePajak.NoID=MBarang.IDTypePajak" & vbCrLf & _
                                            "LEFT JOIN MSatuan (NOLOCK) ON MSatuan.NoID=MBarangD.IDSatuan" & vbCrLf & _
                                            "LEFT JOIN MAlamat MSupplier1 (NOLOCK) ON MSupplier1.NoID=MBarang.IDSupplier1" & vbCrLf & _
                                            "LEFT JOIN MAlamat MSupplier2 (NOLOCK) ON MSupplier2.NoID=MBarang.IDSupplier2" & vbCrLf & _
                                            "LEFT JOIN MAlamat MSupplier3 (NOLOCK) ON MSupplier3.NoID=MBarang.IDSupplier3" & vbCrLf & _
                                            IIf(NamaFieldGudang = "" AndAlso TotalFieldGudang = "", "", "LEFT JOIN (" & vbCrLf & _
                                            "SELECT * FROM (" & vbCrLf & _
                                            "SELECT A.IDBarang, B.Kode, A.SaldoAkhir" & vbCrLf & _
                                            "FROM TSaldoStok (NOLOCK) A" & vbCrLf & _
                                            "INNER JOIN MGudang (NOLOCK) B ON B.NoID=A.IDGudang) T PIVOT (SUM(SaldoAkhir) FOR Kode IN (" & NamaFieldGudang & ")) AS PVT) TSaldoStok ON TSaldoStok.IDBarang=MBarang.NoID") & vbCrLf & _
                                            "WHERE 1=1 "
                                    If Not ckTdkAktif.Checked Then
                                        SQL &= " AND CONVERT(BIT, CASE WHEN MBarang.IsActive=1 AND MBarangD.IsActive=1 THEN 1 ELSE 0 END)=1"
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

                                com.CommandText = "UPDATE MBarang SET IsActive=0 WHERE NoID=" & NoID
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

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        mnEdit.PerformClick()
    End Sub

    Private Sub cmdBaru_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBaru.Click
        mnBaru.PerformClick()
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
        Dim View As GridView = CType(sender, GridView)
        If View Is Nothing Then Return
        ' obtaining hit info
        Dim hitInfo As GridHitInfo = View.CalcHitInfo(New System.Drawing.Point(e.X, e.Y))
        If (e.Button = Windows.Forms.MouseButtons.Right) And (hitInfo.InRow) And _
          (Not View.IsGroupRow(hitInfo.RowHandle)) Then
            PopupMenu1.ShowPopup(Control.MousePosition)
        End If
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
        cmdEdit.PerformClick()
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

    Private Sub mnBaru_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnBaru.ItemClick
        Using frm As New frmEntriBarang(-1)
            If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                RefreshData(frm.NoID)
            End If
        End Using
    End Sub

    Private Sub mnEdit_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnEdit.ItemClick
        Using frm As New frmEntriBarang(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
            If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                RefreshData(frm.NoID)
            End If
        End Using
    End Sub

    Private Sub mnHapus_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHapus.ItemClick
        Dim gridview As DevExpress.XtraGrid.Views.Grid.GridView = Nothing
        gridview = GridView1
        If gridview.RowCount >= 1 Then
            If XtraMessageBox.Show("Ingin menonaktifkan data Barang " & NullToStr(gridview.GetRowCellValue(gridview.FocusedRowHandle, "Nama")) & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                HapusData(NullToLong(gridview.GetRowCellValue(gridview.FocusedRowHandle, "NoID")))
            End If
        End If
    End Sub

    Private Sub mnPreview_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPreview.ItemClick
        Dim NamaFile As String = ""
        NamaFile = Application.StartupPath & "\Report\Lap_MBarangAll.repx"
        ViewXtraReport(Me.MdiParent, IIf(IsEditReport, ActionPrint.Edit, ActionPrint.Preview), NamaFile, "Laporan Master Barang", "Lap_MBarangAll.repx", Me.ds)
    End Sub

    Private Sub mnRefresh_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        RefreshData(-1)
    End Sub

    Private Sub mnTutup_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnTutup.ItemClick
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class
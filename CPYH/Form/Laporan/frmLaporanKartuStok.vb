Imports System.Data.SqlClient
Imports System.Data
Imports DevExpress.Utils
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports CtrlSoft.Ini
Imports CtrlSoft.Utils
Imports CtrlSoft.CetakDX

Public Class frmLaporanKartuStok
    Private IDBarang As Long, IDGudang As Long
    Private PeriodeDari As Date, PeriodeSampai As Date

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Private ds As New DataSet

#Region "Init Data"
    Private Sub InitLoadLookUp()
        Using cn As New SqlConnection(StrKonSQL)
            Using com As New SqlCommand
                Using oDA As New SqlDataAdapter
                    Using ds As New DataSet
                        Try
                            cn.Open()
                            com.Connection = cn
                            com.CommandTimeout = cn.ConnectionTimeout
                            oDA.SelectCommand = com

                            com.CommandText = "SELECT NoID, Kode + '-' + Nama AS Gudang FROM MGudang WHERE IsActive=1"
                            oDA.Fill(ds, "MGudang")
                            txtGudang.Properties.DataSource = ds.Tables("MGudang")
                            txtGudang.Properties.ValueMember = "NoID"
                            txtGudang.Properties.DisplayMember = "Gudang"
                            txtGudang.Properties.SelectAllItemCaption = "Pilih Semua"

                            com.CommandText = "SELECT MBarang.NoID, MBarang.Kode, MBarangD.Barcode, MBarang.Nama + ISNULL(' (' + MSatuan.Kode + ')', '') AS Nama FROM MBarang (NOLOCK) INNER JOIN MBarangD (NOLOCK) ON MBarang.NoID=MBarangD.IDBarang LEFT JOIN MSatuan (NOLOCK) ON MSatuan.NoID=MBarangD.IDSatuan"
                            oDA.Fill(ds, "MBarang")
                            txtBarcode.Properties.DataSource = ds.Tables("MBarang")
                            txtBarcode.Properties.ValueMember = "NoID"
                            txtBarcode.Properties.DisplayMember = "Nama"
                        Catch ex As Exception
                            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Private Sub InitLoadData(Optional ByVal NoID As Long = -1)
        Using cn As New SqlConnection(StrKonSQL)
            Using com As New SqlCommand
                Using oDA As New SqlDataAdapter
                    Try
                        cn.Open()
                        com.Connection = cn
                        com.CommandTimeout = cn.ConnectionTimeout
                        oDA.SelectCommand = com

                        com.CommandText = "spLapKartuStok @TglDari, @TglSampai, @IDBarang, @IDGudang"
                        com.Parameters.Clear()
                        com.Parameters.Add(New SqlParameter("@TglDari", SqlDbType.Date)).Value = NullToDate(DateEdit1.EditValue)
                        com.Parameters.Add(New SqlParameter("@TglSampai", SqlDbType.Date)).Value = NullToDate(DateEdit2.EditValue)
                        com.Parameters.Add(New SqlParameter("@IDBarang", SqlDbType.BigInt)).Value = NullToLong(txtBarcode.EditValue)
                        com.Parameters.Add(New SqlParameter("@IDGudang", SqlDbType.VarChar)).Value = txtGudang.EditValue.ToString.Replace(" ", "")

                        If ds.Tables("") IsNot Nothing Then
                            ds.Tables("").Clear()
                            ds.Tables("").Columns.Clear()
                        End If
                        oDA.Fill(ds, "MData")
                        BindingSource1.DataSource = ds.Tables("MData")

                        GridView1.ClearSelection()
                        GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), NoID.ToString("n0"))
                        GridView1.SelectRow(GridView1.FocusedRowHandle)

                        If ds.Tables("MData").Rows.Count >= 1 Then
                            lbSaldoAwal.Text = NullToDbl(ds.Tables("MData").Rows(0).Item("SaldoAwal")).ToString("n2")
                            lbNilaiAwal.Text = NullToDbl(ds.Tables("MData").Rows(0).Item("NilaiAwal")).ToString("n2")
                            lbSaldoAkhir.Text = NullToDbl(ds.Tables("MData").Rows(ds.Tables("MData").Rows.Count - 1).Item("SaldoAkhir")).ToString("n2")
                            lbNilaiAkhir.Text = NullToDbl(ds.Tables("MData").Rows(ds.Tables("MData").Rows.Count - 1).Item("NilaiAkhir")).ToString("n2")
                        Else
                            lbSaldoAwal.Text = "0.00"
                            lbNilaiAwal.Text = "0.00"
                            lbSaldoAkhir.Text = "0.00"
                            lbNilaiAkhir.Text = "0.00"
                        End If
                    Catch ex As Exception
                        XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End Using
            End Using
        End Using
    End Sub
#End Region

    Public Sub New(ByVal Caption As String, _
                   ByVal IDBarang As Long, _
                   ByVal IDGudang As Long, _
                   ByVal PeriodeDari As Date, _
                   ByVal PeriodeSampai As Date)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Text = Caption
        Me.IDBarang = IDBarang
        Me.IDGudang = IDGudang
        Me.PeriodeDari = PeriodeDari
        Me.PeriodeSampai = PeriodeSampai
    End Sub

    Private Sub frmLaporanKartuStok_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Using dlg As New DevExpress.Utils.WaitDialogForm("Creating Component ...", NamaAplikasi)
            Try
                InitLoadLookUp()
                LabelControl1.Text = Me.Text
                DateEdit1.EditValue = PeriodeDari
                DateEdit2.EditValue = PeriodeSampai
                txtBarcode.EditValue = IDBarang
                If IDGudang <= 0 Then
                    txtGudang.SelectAll()
                Else
                    txtGudang.EditValue = IDGudang.ToString
                End If
                InitLoadData()
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub gv_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataSourceChanged, _
       gvBarcode.DataSourceChanged
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

    Private Sub mnSaveLayouts_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayouts.ItemClick
        Using frm As New frmOtorisasi
            Try
                If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    gvBarcode.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & gvBarcode.Name & ".xml")
                    GridView1.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & GridView1.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub mnPreview_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPreview.ItemClick
        Dim NamaFile As String = "", Judul As String = "", CalculateFields As String = ""
        Try
            InitLoadData()
            NamaFile = "Laporan_Kartu_Stok.repx"
            Judul = "Laporan Kartu Stok"
            CalculateFields = "TglDari=DATETIME=#" & DateEdit1.DateTime.ToString("yyyy-MM-dd") & "#|TglSampai=DATETIME=#" & DateEdit2.DateTime.ToString("yyyy-MM-dd") & "#|FilterBarang=STRING='" & FixApostropi(txtBarcode.Text) & "'|FilterGudang=STRING='" & FixApostropi(txtGudang.Text) & "'"
            ViewXtraReport(Me.MdiParent, IIf(IsEditReport, action_.Edit, action_.Preview), Application.StartupPath & "\Report\" & NamaFile, Judul, NamaFile, ds, , CalculateFields)
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub mnRefresh_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        InitLoadData()
    End Sub

    Private Sub mnTutup_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnTutup.ItemClick
        Me.Close()
        DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
End Class
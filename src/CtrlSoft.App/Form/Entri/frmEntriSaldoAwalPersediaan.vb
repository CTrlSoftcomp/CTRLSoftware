Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.App.Public
Imports DevExpress.XtraEditors.Repository
Imports CtrlSoft.Dto.Model

Public Class frmEntriSaldoAwalPersediaan
    Public NoID As Long = -1
    Private pStatus As [Public].pStatusForm

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Public Sub New(ByVal NoID As Long)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.NoID = NoID

        AddHandler txtQty.LostFocus, AddressOf txtEdit_EditValueChanged
        AddHandler txtHargaBeli.LostFocus, AddressOf txtEdit_EditValueChanged
    End Sub

    Private Sub frmEntriRole_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            LoadData(NoID)
            With LayoutControl1
                If System.IO.File.Exists([Public].SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml") Then
                    .RestoreLayoutFromXml([Public].SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml")
                End If
            End With
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Windows.Forms.Cursor.Current = curentcursor
    End Sub

    Private Sub LoadData(ByVal NoID As Long)
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

                                com.CommandText = "SELECT NoID, Kode + '-' + Nama AS Gudang FROM MGudang WHERE IsActive=1"
                                oDA.Fill(ds, "MGudang")
                                txtGudang.Properties.DataSource = ds.Tables("MGudang")
                                txtGudang.Properties.ValueMember = "NoID"
                                txtGudang.Properties.DisplayMember = "Gudang"

                                com.CommandText = [Public].Dataset.SQLLookUpBarcode
                                oDA.Fill(ds, "MBarangD")
                                txtBarcode.Properties.DataSource = ds.Tables("MBarangD")
                                txtBarcode.Properties.DisplayMember = "Barcode"
                                txtBarcode.Properties.ValueMember = "NoID"

                                com.CommandText = "SELECT * FROM MSaldoAwalPersediaan WHERE NoID=" & NoID
                                oDA.Fill(ds, "MSaldoAwalPersediaan")

                                If ds.Tables("MSaldoAwalPersediaan").Rows.Count >= 1 Then
                                    Dim iRow As DataRow = ds.Tables("MSaldoAwalPersediaan").Rows(0)
                                    If NullToBool(iRow.Item("IsPosted")) Then
                                        pStatus = pStatusForm.Posted
                                    Else
                                        pStatus = pStatusForm.Edit
                                    End If
                                    Me.NoID = NullToLong(iRow.Item("NoID"))
                                    txtKode.EditValue = NullToStr(iRow.Item("Kode"))
                                    txtTanggal.EditValue = NullToDate(iRow.Item("Tanggal"))
                                    txtGudang.EditValue = NullToLong(iRow.Item("IDGudang"))
                                    txtBarcode.EditValue = NullToLong(iRow.Item("IDBarangD"))
                                    IDBarang = NullToLong(iRow.Item("IDBarang"))
                                    txtSatuan.EditValue = NullToLong(iRow.Item("IDSatuan"))
                                    txtKonversi.EditValue = NullToDbl(iRow.Item("Konversi"))
                                    txtQty.EditValue = NullToDbl(iRow.Item("Qty"))

                                    txtHargaBeli.EditValue = NullToDbl(iRow.Item("HargaPokok"))
                                    txtCatatan.Text = NullToStr(iRow.Item("Keterangan"))
                                Else
                                    pStatus = pStatusForm.Baru
                                    Me.NoID = -1
                                    txtKode.EditValue = "AUTO"
                                    txtTanggal.EditValue = [Public].UserLogin.TanggalSystem
                                    txtGudang.EditValue = -1
                                    IDBarang = -1
                                    txtBarcode.EditValue = -1
                                    txtSatuan.EditValue = -1
                                    txtKonversi.EditValue = 1
                                    txtQty.EditValue = 0

                                    txtHargaBeli.EditValue = 0
                                    txtCatatan.Text = ""
                                End If
                                HitungJumlah()

                                txtBarcode.Focus()
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Private HPPPcs As Double = 0.0
    Private HargaBruto As Double = 0.0
    Private TotalBruto As Double = 0.0
    Private IDBarang As Long = -1

    Private Sub HitungJumlah()
        Try
            HargaBruto = Bulatkan(txtHargaBeli.EditValue, 2)
            TotalBruto = Bulatkan(HargaBruto * txtQty.EditValue, 2)
            txtJumlah.EditValue = TotalBruto
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LayoutControl1_DefaultLayoutLoaded(ByVal sender As Object, ByVal e As System.EventArgs) Handles LayoutControl1.DefaultLayoutLoaded
        With LayoutControl1
            If System.IO.File.Exists([Public].SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml") Then
                .RestoreLayoutFromXml([Public].SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml")
            End If
        End With
    End Sub

    Private Sub mnSaveLayout_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayout.ItemClick
        Using frm As New frmOtorisasi
            Try
                If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    LayoutControl1.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & LayoutControl1.Name & ".xml")
                    gvGudang.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & gvGudang.Name & ".xml")
                    gvBarcode.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & gvBarcode.Name & ".xml")
                    gvSatuan.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & gvSatuan.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub txtEdit_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        HitungJumlah()
    End Sub

    Private Sub gv_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSatuan.DataSourceChanged, gvBarcode.DataSourceChanged, gvGudang.DataSourceChanged
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

    Private Sub txtBarcode_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarcode.EditValueChanged
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

                                
                                com.CommandText = "SELECT MSatuan.NoID, MSatuan.Kode Satuan, MSatuan.Konversi FROM MSatuan(NOLOCK) INNER JOIN MBarangD(NOLOCK) ON MSatuan.NoID=MBarangD.IDSatuan WHERE MBarangD.IDBarang=" & IDBarang & " AND MBarangD.IsActive=1"
                                oDA.Fill(ds, "MSatuan")
                                txtSatuan.Properties.DataSource = ds.Tables("MSatuan")
                                txtSatuan.Properties.DisplayMember = "Satuan"
                                txtSatuan.Properties.ValueMember = "NoID"

                                com.CommandText = "DECLARE @IDBarang AS BIGINT = " & IDBarang & ", @Tanggal AS DATETIME = '" & NullToDate(txtTanggal.EditValue).ToString("yyyy-MM-dd HH:mm:ss") & "';" & vbCrLf & _
                                                  "SELECT TOP 1 MKartuStok.HPP" & vbCrLf & _
                                                  "FROM MKartuStok(NOLOCK)" & vbCrLf & _
                                                  "LEFT JOIN MJenisTransaksi(NOLOCK) ON MKartuStok.IDJenisTransaksi=MJenisTransaksi.ID" & vbCrLf & _
                                                  "WHERE MKartuStok.IDBarang=@IDBarang AND MKartuStok.Tanggal<=@Tanggal ORDER BY MKartuStok.Tanggal DESC, MJenisTransaksi.NoUrut DESC"


                                com.CommandText = "DECLARE @IDBarang AS BIGINT = " & IDBarang & ", @Tanggal AS DATETIME = '" & NullToDate(txtTanggal.EditValue).ToString("yyyy-MM-dd HH:mm:ss") & "';" & vbCrLf & _
                                                  "SELECT MBarangD.IDBarang, MBarang.Kode, MBarang.Nama + ISNULL(' (' + MSatuan.Kode + ')', '') AS Nama, MBarangD.IDSatuan, MBarang.HargaBeli, MBarang.DiscProsen1, MBarang.DiscProsen2, MBarang.DiscProsen3, MBarang.DiscProsen4, MBarang.DiscProsen5, MBarang.DiscRp, " & vbCrLf & _
                                                  "ISNULL((SELECT TOP 1 MKartuStok.HPP" & vbCrLf & _
                                                  "FROM MKartuStok(NOLOCK)" & vbCrLf & _
                                                  "LEFT JOIN MJenisTransaksi(NOLOCK) ON MKartuStok.IDJenisTransaksi=MJenisTransaksi.ID" & vbCrLf & _
                                                  "WHERE MKartuStok.IDBarang=@IDBarang AND MKartuStok.Tanggal<=@Tanggal ORDER BY MKartuStok.Tanggal DESC, MJenisTransaksi.NoUrut DESC), 0) AS HPP" & vbCrLf & _
                                                  "FROM MBarang INNER JOIN MBarangD ON MBarang.NoID=MBarangD.IDBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarangD.IDSatuan WHERE MBarangD.NoID=" & NullToLong(txtBarcode.EditValue)
                                oDA.Fill(ds, "MBarang")
                                If ds.Tables("MBarang").Rows.Count >= 1 Then
                                    Dim iRow As DataRow = ds.Tables("MBarang").Rows(0)
                                    IDBarang = NullToLong(iRow.Item("IDBarang"))
                                    txtKodeBarang.EditValue = NullToStr(iRow.Item("Kode"))
                                    txtNamaBarang.EditValue = NullToStr(iRow.Item("Nama"))
                                    txtSatuan.EditValue = NullToLong(iRow.Item("IDSatuan"))

                                    HPPPcs = NullToDbl(iRow.Item("HPP"))
                                    txtHargaBeli.EditValue = Bulatkan(HPPPcs * NullToDbl(txtKonversi.EditValue), 2)
                                Else
                                    IDBarang = -1
                                    txtKodeBarang.EditValue = ""
                                    txtNamaBarang.EditValue = ""
                                    txtSatuan.EditValue = -1

                                    HPPPcs = 0.0
                                    txtHargaBeli.EditValue = Bulatkan(HPPPcs * NullToDbl(txtKonversi.EditValue), 2)
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

    Private Sub txtSatuan_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSatuan.EditValueChanged
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

                                com.CommandText = "SELECT MSatuan.NoID, MSatuan.Kode Satuan, MSatuan.Konversi FROM MSatuan(NOLOCK) INNER JOIN MBarangD(NOLOCK) ON MSatuan.NoID=MBarangD.IDSatuan WHERE MBarangD.NoID=" & NullToLong(txtBarcode.EditValue)
                                oDA.Fill(ds, "MSatuan")
                                If ds.Tables("MSatuan").Rows.Count >= 1 Then
                                    Dim iRow As DataRow = ds.Tables("MSatuan").Rows(0)
                                    txtKonversi.EditValue = NullToDbl(iRow.Item("Konversi"))
                                Else
                                    txtKonversi.EditValue = 1
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

    Private Sub SimpanData(ByVal Tutup As Boolean)
        DxErrorProvider1.ClearErrors()
        If pStatus = pStatusForm.Posted Then
            DxErrorProvider1.SetError(txtKode, "Data telah diposting harus diisi!")
        End If
        If txtKode.Text = "" Then
            DxErrorProvider1.SetError(txtKode, "Kode harus diisi!")
        End If
        If txtTanggal.Text = "" Then
            DxErrorProvider1.SetError(txtKode, "Tanggal harus diisi!")
        End If
        If txtBarcode.Text = "" Then
            DxErrorProvider1.SetError(txtBarcode, "Barcode harus diisi!")
        End If
        If txtSatuan.Text = "" Then
            DxErrorProvider1.SetError(txtSatuan, "Satuan harus diisi!")
        End If
        If txtKonversi.EditValue <= 0 Then
            DxErrorProvider1.SetError(txtKonversi, "Konversi salah!")
        End If
        If txtJumlah.EditValue < 0.0 Then
            DxErrorProvider1.SetError(txtHargaBeli, "Nilai Saldo Awal salah!")
        End If

        If Not DxErrorProvider1.HasErrors Then
            Using dlg As New WaitDialogForm("Sedang menyimpan data ...", NamaAplikasi)
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

                                    'com.CommandText = "SELECT COUNT(NoID) FROM MBarangD WHERE Barcode='" & FixApostropi(txtBarcode.Text) & "' AND NoID<>" & Me.NoID
                                    'If NullToLong(com.ExecuteScalar()) >= 1 Then
                                    '    DxErrorProvider1.SetError(txtBarcode, "Barcode sudah dipakai!")
                                    'End If

                                    If Not DxErrorProvider1.HasErrors Then
                                        If pStatus = pStatusForm.Edit OrElse pStatus = pStatusForm.TempInsert Then
                                            com.CommandText = "SELECT IsPosted FROM MSaldoAwalPersediaan(NOLOCK) WHERE NoID>=1 AND NoID=" & NoID
                                            If NullToBool(com.ExecuteScalar()) Then
                                                DxErrorProvider1.SetError(txtKode, "Data telah diposting harus diisi!")
                                            End If
                                        End If
                                        If Not DxErrorProvider1.HasErrors Then

                                            If pStatus = pStatusForm.Edit OrElse pStatus = pStatusForm.TempInsert Then
                                                com.CommandText = "SELECT COUNT(Kode) FROM MSaldoAwalPersediaan WHERE Kode=@Kode AND NoID<>@NoID"
                                                com.Parameters.Clear()
                                                com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NoID
                                                com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = txtKode.Text
                                                If NullToLong(com.ExecuteScalar()) >= 1 Then
                                                    XtraMessageBox.Show("Nota telah sudah ada!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                                    Exit Try
                                                End If
                                            End If

                                            com.CommandText = "[dbo].[spSimpanMSaldoAwalPersediaan] " & vbCrLf & _
                                                              "@NoID " & vbCrLf & _
                                                              ",@Kode " & vbCrLf & _
                                                              ",@Tanggal " & vbCrLf & _
                                                              ",@Keterangan " & vbCrLf & _
                                                              ",@IDGudang " & vbCrLf & _
                                                              ",@IDBarangD " & vbCrLf & _
                                                              ",@IDBarang " & vbCrLf & _
                                                              ",@IDSatuan " & vbCrLf & _
                                                              ",@Qty " & vbCrLf & _
                                                              ",@Konversi " & vbCrLf & _
                                                              ",@HargaPokok " & vbCrLf & _
                                                              ",@IsPosted " & vbCrLf & _
                                                              ",@TglPosted " & vbCrLf & _
                                                              ",@IDUserPosted " & vbCrLf & _
                                                              ",@IDUserEntry " & vbCrLf & _
                                                              ",@IDUserEdit"


                                            com.Parameters.Clear()
                                            com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NoID
                                            com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = txtKode.Text
                                            com.Parameters.Add(New SqlParameter("@Tanggal", SqlDbType.DateTime)).Value = txtTanggal.EditValue
                                            com.Parameters.Add(New SqlParameter("@Keterangan", SqlDbType.VarChar)).Value = txtCatatan.Text
                                            com.Parameters.Add(New SqlParameter("@IDGudang", SqlDbType.Int)).Value = NullTolInt(txtGudang.EditValue)
                                            com.Parameters.Add(New SqlParameter("@IDBarangD", SqlDbType.Int)).Value = NullToDbl(txtBarcode.EditValue)
                                            com.Parameters.Add(New SqlParameter("@IDSatuan", SqlDbType.Int)).Value = NullToDbl(txtSatuan.EditValue)
                                            com.Parameters.Add(New SqlParameter("@IDBarang", SqlDbType.Int)).Value = NullToDbl(IDBarang)

                                            com.Parameters.Add(New SqlParameter("@IsPosted", SqlDbType.Bit)).Value = False
                                            com.Parameters.Add(New SqlParameter("@TglPosted", SqlDbType.DateTime)).Value = System.DBNull.Value
                                            com.Parameters.Add(New SqlParameter("@IDUserPosted", SqlDbType.Int)).Value = System.DBNull.Value
                                            com.Parameters.Add(New SqlParameter("@IDUserEntry", SqlDbType.Int)).Value = UserLogin.NoID
                                            com.Parameters.Add(New SqlParameter("@IDUserEdit", SqlDbType.Int)).Value = UserLogin.NoID

                                            com.Parameters.Add(New SqlParameter("@Qty", SqlDbType.Float)).Value = Bulatkan(NullToDbl(txtQty.EditValue), 3)
                                            com.Parameters.Add(New SqlParameter("@HargaPokok", SqlDbType.Float)).Value = Bulatkan(NullToDbl(txtHargaBeli.EditValue), 2)
                                            com.Parameters.Add(New SqlParameter("@Konversi", SqlDbType.Float)).Value = NullToDbl(txtKonversi.EditValue)
                                            com.Parameters.Add(New SqlParameter("@Jumlah", SqlDbType.Float)).Value = Bulatkan(NullToDbl(txtJumlah.EditValue), 2)
                                            com.ExecuteNonQuery()

                                            com.Transaction.Commit()

                                            If Tutup Then
                                                DialogResult = Windows.Forms.DialogResult.OK
                                                Me.Close()
                                            Else
                                                NoID = -1
                                                LoadData(NoID)
                                            End If
                                        End If
                                    End If
                                Catch ex As Exception
                                    XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                End Try
                            End Using
                        End Using
                    End Using
                End Using
            End Using
        End If
    End Sub

    Private Sub mnTutup_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnTutup.ItemClick
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub txtTanggal_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTanggal.EditValueChanged
        Try
            If pStatus = pStatusForm.Baru Then
                txtKode.Text = "AUTO" 'Repository.RepKode.GetNewKode("MJual", "Kode", "PO-" & NullToDate(txtTanggal.EditValue).ToString("yyMM") & "-", "", Repository.RepKode.Format.A00000)
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub mnSimpanBaru_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSimpanBaru.ItemClick
        SimpanData(False)
    End Sub

    Private Sub mnSimpanTutup_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSimpanTutup.ItemClick
        SimpanData(True)
    End Sub
End Class
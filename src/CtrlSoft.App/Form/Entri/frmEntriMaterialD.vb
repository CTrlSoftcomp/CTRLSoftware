Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.App.Public
Imports DevExpress.XtraEditors.Repository
Imports Dapper
Imports DevExpress.XtraTab.ViewInfo

Public Class frmEntriMaterialD
    Private pStatus As pStatusForm
    Public data As New CtrlSoft.Dto.Model.MMaterialD
    Public header As New CtrlSoft.Dto.Model.MMaterial

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Public Sub New(ByVal data As CtrlSoft.Dto.Model.MMaterialD,
                   ByVal header As CtrlSoft.Dto.Model.MMaterial)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.data = data
        Me.header = header
    End Sub

    Public Overridable Sub frmEntriRole_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim curentcursor As Cursor = System.Windows.Forms.Cursor.Current
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            LoadData(data.NoID)
            With LayoutControl1
                If System.IO.File.Exists([Public].SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml") Then
                    .RestoreLayoutFromXml([Public].SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml")
                End If
            End With
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        System.Windows.Forms.Cursor.Current = curentcursor
    End Sub

    Public Overridable Sub LoadData(ByVal NoID As Guid)
        Dim Args As New Dictionary(Of String, Object)
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

                                com.CommandText = "SELECT NoID, Kode FROM MSatuan WHERE IsActive=1"
                                oDA.Fill(ds, "MSatuan")
                                txtSatuan.Properties.DataSource = ds.Tables("MSatuan")
                                txtSatuan.Properties.ValueMember = "NoID"
                                txtSatuan.Properties.DisplayMember = "Kode"
                                txtSatuan.EditValue = -1

                                com.CommandText = "SELECT MBarangD.NoID, " & vbCrLf &
                                                  "MBarangD.Barcode, " & vbCrLf &
                                                  "MBarang.Kode AS KodeBarang, " & vbCrLf &
                                                  "MBarang.Nama NamaBarang, " & vbCrLf &
                                                  "MSatuan.Kode AS Satuan, " & vbCrLf &
                                                  "MBarangD.Konversi " & vbCrLf &
                                                  "FROM MBarangD(NOLOCK) " & vbCrLf &
                                                  "INNER JOIN MBarang(NOLOCK) ON MBarang.NoID = MBarangD.IDBarang " & vbCrLf &
                                                  "LEFT JOIN MSatuan(NOLOCK) ON MSatuan.NoID = MBarangD.IDSatuan " & vbCrLf &
                                                  "WHERE MBarangD.IsActive = 1 " & vbCrLf &
                                                  "AND MBarang.IsActive = 1"
                                oDA.Fill(ds, "MBarangD")
                                txtBarcode.Properties.DataSource = ds.Tables("MBarangD")
                                txtBarcode.Properties.ValueMember = "NoID"
                                txtBarcode.Properties.DisplayMember = "Barcode"
                                txtBarcode.EditValue = -1

                                If (data IsNot Nothing AndAlso data.IDBarangD >= 1) Then
                                    pStatus = pStatusForm.Edit
                                    txtBarcode.EditValue = data.IDBarangD
                                    txtQty.EditValue = data.Qty
                                    txtKonversi.EditValue = data.Konversi
                                    txtHargaPokok.EditValue = data.HargaPokok
                                    txtJumlah.EditValue = data.Jumlah
                                Else
                                    pStatus = pStatusForm.Baru
                                    data.NoID = System.Guid.NewGuid()
                                    txtBarcode.EditValue = -1
                                    txtQty.EditValue = 0
                                    txtKonversi.EditValue = 0.0
                                    txtHargaPokok.EditValue = 0.0
                                    txtJumlah.EditValue = 0.0
                                End If
                                com.Transaction.Commit()
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
        End Using
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
                If frm.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                    LayoutControl1.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & LayoutControl1.Name & ".xml")

                    gvBarcode.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & gvBarcode.Name & ".xml")
                    gvSatuan.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & gvSatuan.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Public Sub HitungMaterial()
        Try
            txtJumlah.EditValue = Bulatkan(txtQty.EditValue * txtHargaPokok.EditValue, 2)
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub mnSimpan_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSimpan.ItemClick
        SimpanData()
    End Sub

    Public Overridable Sub SimpanData()
        DxErrorProvider1.ClearErrors()
        HitungMaterial()

        If txtBarcode.EditValue <= 0 OrElse txtKodeBarang.Text = "" OrElse txtSatuan.Text = "" OrElse txtKonversi.EditValue <= 0 Then
            DxErrorProvider1.SetError(txtBarcode, "Barang harus diisi!")
        End If
        If txtQty.EditValue <= 0 Then
            DxErrorProvider1.SetError(txtQty, "Qty Barang harus diisi!")
        End If
        Dim exists = header.MMaterialDs.Where(Function(m) m.NoID <> data.NoID AndAlso m.IDBarang = NullToLong(txtBarcode.EditValue)).SingleOrDefault
        If (exists IsNot Nothing) Then
            DxErrorProvider1.SetError(txtBarcode, "Barang yang dipilih sudah ada!")
        End If

        If Not DxErrorProvider1.HasErrors Then
            Using dlg As New WaitDialogForm("Sedang menyimpan data ...", NamaAplikasi)
                Try
                    dlg.Show()
                    dlg.Focus()

                    With data
                        .KodeBarang = txtKodeBarang.Text
                        .NamaBarang = txtNamaBarang.Text
                        .Barcode = txtBarcode.Text
                        .Satuan = txtSatuan.Text
                        .IDBarangD = NullToLong(txtBarcode.EditValue)
                        .IDBarang = NullToLong(txtBarcode.Tag)
                        .IDSatuan = NullToLong(txtSatuan.EditValue)
                        .Qty = txtQty.EditValue
                        .Konversi = txtKonversi.EditValue
                        .HargaPokok = txtHargaPokok.EditValue
                        .Jumlah = txtJumlah.EditValue
                        .IDMaterial = data.IDMaterial
                    End With
                    DialogResult = System.Windows.Forms.DialogResult.OK
                    Me.Close()
                Catch ex As Exception
                    XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End If
    End Sub

    Private Sub mnTutup_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnTutup.ItemClick
        DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub gvBahan_DataSourceChanged(sender As Object, e As EventArgs) Handles gvBarcode.DataSourceChanged, gvSatuan.DataSourceChanged
        With sender
            If System.IO.File.Exists(CtrlSoft.App.Public.SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml") Then
                .RestoreLayoutFromXml(CtrlSoft.App.Public.SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml")
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

    Private Sub txtQty_LostFocus(sender As Object, e As EventArgs) Handles txtQty.LostFocus, txtHargaPokok.LostFocus
        HitungMaterial()
    End Sub

    Private Sub txtBarcode_EditValueChanged(sender As Object, e As EventArgs) Handles txtBarcode.EditValueChanged
        RubahBarcode()
    End Sub

    Private Sub RubahBarcode()
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

                                com.CommandText = "SELECT MBarangD.*, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MSatuan.Kode AS Satuan" & vbCrLf &
                                                  "FROM MBarangD(NOLOCK)" & vbCrLf &
                                                  "LEFT JOIN MBarang(NOLOCK) ON MBarang.NoID=MBarangD.IDBarang" & vbCrLf &
                                                  "LEFT JOIN MSatuan(NOLOCK) ON MBarang.NoID=MBarangD.IDSatuan" & vbCrLf &
                                                  "WHERE MBarangD.NoID=@NoID"
                                com.Parameters.Clear()
                                com.Parameters.AddWithValue("@NoID", NullToLong(txtBarcode.EditValue))
                                oDA.Fill(ds, "MBarangD")
                                If (ds.Tables("MBarangD").Rows.Count >= 1) Then
                                    txtBarcode.Tag = NullToLong(ds.Tables("MBarangD").Rows(0).Item("IDBarang"))
                                    txtKodeBarang.Text = NullToStr(ds.Tables("MBarangD").Rows(0).Item("KodeBarang"))
                                    txtNamaBarang.Text = NullToStr(ds.Tables("MBarangD").Rows(0).Item("NamaBarang"))
                                    txtSatuan.EditValue = NullToLong(ds.Tables("MBarangD").Rows(0).Item("IDSatuan"))
                                    txtKonversi.EditValue = NullTolInt(ds.Tables("MBarangD").Rows(0).Item("Konversi"))
                                Else
                                    txtBarcode.Tag = -1
                                    txtKodeBarang.Text = ""
                                    txtNamaBarang.Text = ""
                                    txtSatuan.EditValue = -1
                                    txtKonversi.EditValue = 0
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

    Private Sub mnRefresh_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        mnRefreshClick()
    End Sub

    Public Overridable Sub mnRefreshClick()
        LoadData(data.NoID)
    End Sub
End Class
﻿Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.App.Public
Imports DevExpress.XtraEditors.Repository
Imports CtrlSoft.Dto.Model

Public Class frmEntriBeliD
    Public NoID As Long = -1, IDBarangD As Long = -1
    Public IDHeader As Long = -1
    Private frmPemanggil As frmEntriBeli = Nothing
    Private pStatus As [Public].pStatusForm
    Private TypePajak As [Public].TypePajak

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Public Sub New(ByVal formPemanggil As frmEntriBeli,
                   ByVal NoID As Long,
                   ByVal IDHeader As Long,
                   ByVal TypePajak As [Public].TypePajak,
                   ByVal IDBarangD As Long)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.NoID = NoID
        Me.IDHeader = IDHeader
        Me.TypePajak = TypePajak
        Me.frmPemanggil = formPemanggil
        Me.IDBarangD = IDBarangD

        AddHandler txtQty.LostFocus, AddressOf txtEdit_EditValueChanged
        AddHandler txtDiscProsen1.LostFocus, AddressOf txtEdit_EditValueChanged
        AddHandler txtDiscProsen2.LostFocus, AddressOf txtEdit_EditValueChanged
        AddHandler txtDiscProsen3.LostFocus, AddressOf txtEdit_EditValueChanged
        AddHandler txtDiscProsen4.LostFocus, AddressOf txtEdit_EditValueChanged
        AddHandler txtDiscProsen5.LostFocus, AddressOf txtEdit_EditValueChanged
        AddHandler txtDiscRp.LostFocus, AddressOf txtEdit_EditValueChanged
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

                                com.CommandText = "SELECT MPOD.NoID, MPO.Kode FROM MPO INNER JOIN MPOD ON MPOD.IDHeader=MPO.NoID WHERE " & vbCrLf & _
                                                  IIf(frmPemanggil.txtPO.Text <> "", " MPOD.IDHeader=" & NullToLong(frmPemanggil.txtPO.EditValue), "MPO.NoID=-1")
                                oDA.Fill(ds, "MPOD")
                                txtPO.Properties.DataSource = ds.Tables("MPOD")
                                txtPO.Properties.DisplayMember = "Kode"
                                txtPO.Properties.ValueMember = "NoID"

                                com.CommandText = [Public].Dataset.SQLLookUpBarcode
                                oDA.Fill(ds, "MBarangD")
                                txtBarcode.Properties.DataSource = ds.Tables("MBarangD")
                                txtBarcode.Properties.DisplayMember = "Barcode"
                                txtBarcode.Properties.ValueMember = "NoID"

                                com.CommandText = "SELECT * FROM MBeliD WHERE NoID=" & NoID
                                oDA.Fill(ds, "MBeliD")

                                If ds.Tables("MBeliD").Rows.Count >= 1 Then
                                    Dim iRow As DataRow = ds.Tables("MBeliD").Rows(0)
                                    pStatus = pStatusForm.Edit
                                    Me.NoID = NullToLong(iRow.Item("NoID"))
                                    Me.IDHeader = NullToLong(iRow.Item("IDHeader"))
                                    txtBarcode.EditValue = NullToLong(iRow.Item("IDBarangD"))
                                    IDBarang = NullToLong(iRow.Item("IDBarang"))
                                    txtPO.EditValue = NullToLong(iRow.Item("IDPOD"))
                                    txtSatuan.EditValue = NullToLong(iRow.Item("IDSatuan"))
                                    txtKonversi.EditValue = NullToDbl(iRow.Item("Konversi"))
                                    txtQty.EditValue = NullToDbl(iRow.Item("Qty"))

                                    txtHargaBeli.EditValue = NullToDbl(iRow.Item("Harga"))
                                    txtDiscProsen1.EditValue = NullToDbl(iRow.Item("DiscProsen1"))
                                    txtDiscProsen2.EditValue = NullToDbl(iRow.Item("DiscProsen2"))
                                    txtDiscProsen3.EditValue = NullToDbl(iRow.Item("DiscProsen3"))
                                    txtDiscProsen4.EditValue = NullToDbl(iRow.Item("DiscProsen4"))
                                    txtDiscProsen5.EditValue = NullToDbl(iRow.Item("DiscProsen5"))
                                    txtDiscRp.EditValue = NullToDbl(iRow.Item("DiscRp"))
                                Else
                                    pStatus = pStatusForm.Baru
                                    Me.NoID = -1
                                    IDBarang = -1
                                    txtPO.EditValue = -1
                                    txtSatuan.EditValue = -1
                                    txtKonversi.EditValue = 1
                                    txtBarcode.EditValue = IDBarangD
                                    txtQty.EditValue = 0
                                    If NullToStr(txtBarcode.Text) <> "" OrElse IDBarangD >= 1 Then
                                        LayoutControlItem16.Control.Focus()
                                        txtQty.Focus()
                                    End If
                                End If
                                HitungJumlah()
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Private HargaPcs As Double = 0.0
    Private DPP As Double = 0.0
    Private PPN As Double = 0.0
    Private HargaBruto As Double = 0.0
    Private TotalBruto As Double = 0.0
    Private IDBarang As Long = -1

    Private Sub HitungJumlah()
        Try
            HargaBruto = Bulatkan((txtHargaBeli.EditValue * (1 - txtDiscProsen1.EditValue / 100) * (1 - txtDiscProsen2.EditValue / 100) * (1 - txtDiscProsen3.EditValue / 100) * (1 - txtDiscProsen4.EditValue / 100) * (1 - txtDiscProsen5.EditValue / 100)) - txtDiscRp.EditValue, 2)
            TotalBruto = Bulatkan(HargaBruto * txtQty.EditValue, 2)
            HargaPcs = HargaBruto / txtKonversi.EditValue
            If TypePajak = [Public].TypePajak.NonPajak Then
                DPP = 0.0
                PPN = 0.0
                txtJumlah.EditValue = TotalBruto
            ElseIf TypePajak = [Public].TypePajak.Include Then
                DPP = Bulatkan((HargaBruto * txtQty.EditValue) / 1.1, 0)
                PPN = Bulatkan(DPP * 0.1, 0)
                txtJumlah.EditValue = Bulatkan(DPP + PPN, 0)
            Else
                DPP = Bulatkan(TotalBruto, 0)
                PPN = Bulatkan(DPP * 0.1, 0)
                txtJumlah.EditValue = Bulatkan(DPP + PPN, 0)
            End If
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

    Private Sub gv_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSatuan.DataSourceChanged, gvBarcode.DataSourceChanged
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

                                com.CommandText = "DECLARE @IDBarang AS BIGINT = -1, @IDBarangD AS BIGINT = " & NullToLong(txtBarcode.EditValue) & ";" & vbCrLf & _
                                                  "SELECT @IDBarang = MBarangD.IDBarang FROM MBarangD WHERE MBarangD.NoID=@IDBarangD;" & vbCrLf & _
                                                  "SELECT X.NoID, MSatuan.Kode Satuan, X.Konversi " & vbCrLf & _
                                                  "FROM (" & vbCrLf & _
                                                  "SELECT MBarangD.IDSatuan NoID, MBarangD.Konversi " & vbCrLf & _
                                                  "FROM MBarangD" & vbCrLf & _
                                                  "INNER JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang" & vbCrLf & _
                                                  "WHERE MBarangD.IDBarang=@IDBarang AND MBarang.IsActive=1 AND MBarangD.IsActive=1" & vbCrLf & _
                                                  "UNION ALL" & vbCrLf & _
                                                  "SELECT MBarang.IDSatuanBeli, MBarang.IsiCtn" & vbCrLf & _
                                                  "FROM MBarang" & vbCrLf & _
                                                  "WHERE MBarang.NoID=@IDBarang AND MBarang.IsActive=1) AS X" & vbCrLf & _
                                                  "INNER JOIN MSatuan ON MSatuan.NoID=X.NoID" & vbCrLf & _
                                                  "GROUP BY X.NoID, X.Konversi, MSatuan.Kode"
                                oDA.Fill(ds, "MSatuan")
                                txtSatuan.Properties.DataSource = ds.Tables("MSatuan")
                                txtSatuan.Properties.DisplayMember = "Satuan"
                                txtSatuan.Properties.ValueMember = "NoID"

                                com.CommandText = "SELECT MBarangD.IDBarang, MBarang.Kode, MBarang.Nama + ISNULL(' (' + MSatuan.Kode + ')', '') AS Nama, MBarangD.IDSatuan, MBarang.HargaBeli, MBarang.DiscProsen1, MBarang.DiscProsen2, MBarang.DiscProsen3, MBarang.DiscProsen4, MBarang.DiscProsen5, MBarang.DiscRp FROM MBarang INNER JOIN MBarangD ON MBarang.NoID=MBarangD.IDBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarangD.IDSatuan WHERE MBarangD.NoID=" & NullToLong(txtBarcode.EditValue)
                                oDA.Fill(ds, "MBarang")
                                If ds.Tables("MBarang").Rows.Count >= 1 Then
                                    Dim iRow As DataRow = ds.Tables("MBarang").Rows(0)
                                    IDBarang = NullToLong(iRow.Item("IDBarang"))
                                    txtKodeBarang.EditValue = NullToStr(iRow.Item("Kode"))
                                    txtNamaBarang.EditValue = NullToStr(iRow.Item("Nama"))
                                    txtSatuan.EditValue = NullToLong(iRow.Item("IDSatuan"))

                                    txtHargaBeli.EditValue = NullToDbl(iRow.Item("HargaBeli"))
                                    txtDiscProsen1.EditValue = NullToDbl(iRow.Item("DiscProsen1"))
                                    txtDiscProsen2.EditValue = NullToDbl(iRow.Item("DiscProsen2"))
                                    txtDiscProsen3.EditValue = NullToDbl(iRow.Item("DiscProsen3"))
                                    txtDiscProsen4.EditValue = NullToDbl(iRow.Item("DiscProsen4"))
                                    txtDiscProsen5.EditValue = NullToDbl(iRow.Item("DiscProsen5"))
                                    txtDiscRp.EditValue = NullToDbl(iRow.Item("DiscRp"))

                                    If (frmPemanggil.txtPO.Text <> "") Then
                                        com.CommandText = "SELECT TOP 1 NoID FROM MPOD WHERE IDHeader=" & NullToLong(frmPemanggil.txtPO.EditValue)
                                        txtPO.EditValue = NullToLong(com.ExecuteScalar())
                                    Else
                                        txtPO.EditValue = -1
                                    End If
                                Else
                                    IDBarang = -1
                                    txtKodeBarang.EditValue = ""
                                    txtNamaBarang.EditValue = ""
                                    txtSatuan.EditValue = -1

                                    txtHargaBeli.EditValue = 0.0
                                    txtDiscProsen1.EditValue = 0.0
                                    txtDiscProsen2.EditValue = 0.0
                                    txtDiscProsen3.EditValue = 0.0
                                    txtDiscProsen4.EditValue = 0.0
                                    txtDiscProsen5.EditValue = 0.0
                                    txtDiscRp.EditValue = 0.0
                                    txtPO.EditValue = -1
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

                                com.CommandText = "DECLARE @IDBarang AS BIGINT = -1, @IDBarangD AS BIGINT = " & NullToLong(txtBarcode.EditValue) & ";" & vbCrLf & _
                                                  "SELECT @IDBarang = MBarangD.IDBarang FROM MBarangD WHERE MBarangD.NoID=@IDBarangD;" & vbCrLf & _
                                                  "SELECT X.NoID, MSatuan.Kode Satuan, X.Konversi " & vbCrLf & _
                                                  "FROM (" & vbCrLf & _
                                                  "SELECT MBarangD.IDSatuan NoID, MBarangD.Konversi " & vbCrLf & _
                                                  "FROM MBarangD" & vbCrLf & _
                                                  "INNER JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang" & vbCrLf & _
                                                  "WHERE MBarangD.IDBarang=@IDBarang AND MBarang.IsActive=1 AND MBarangD.IsActive=1" & vbCrLf & _
                                                  "UNION ALL" & vbCrLf & _
                                                  "SELECT MBarang.IDSatuanBeli, MBarang.IsiCtn" & vbCrLf & _
                                                  "FROM MBarang" & vbCrLf & _
                                                  "WHERE MBarang.NoID=@IDBarang AND MBarang.IsActive=1) AS X" & vbCrLf & _
                                                  "INNER JOIN MSatuan ON MSatuan.NoID=X.NoID" & vbCrLf & _
                                                  "GROUP BY X.NoID, X.Konversi, MSatuan.Kode" & vbCrLf & _
                                                  "HAVING X.NoID=" & NullToLong(txtSatuan.EditValue)
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

    Private Sub BarButtonItem1_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSimpan.ItemClick
        DxErrorProvider1.ClearErrors()
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
            DxErrorProvider1.SetError(txtHargaBeli, "Nilai Pembelian salah!")
        End If
        If frmPemanggil.txtPO.Text <> "" AndAlso txtPO.Text = "" Then
            If XtraMessageBox.Show("Barang tidak ditemukan di nota PO. Ingin Melanjutkan Penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                DxErrorProvider1.SetError(txtBarcode, "Barang tidak ditemukan di Item PO!")
            End If
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
                                        If pStatus = pStatusForm.Baru Then
                                            com.CommandText = "SELECT MAX(NoID) FROM MBeliD"
                                            NoID = NullToLong(com.ExecuteScalar()) + 1

                                            com.CommandText = "INSERT INTO [dbo].[MBeliD] ([NoID],[IDHeader],[IDBarangD],[IDBarang],[IDSatuan],[Konversi],[Qty]" & vbCrLf &
                                                              ",[Harga],[DiscProsen1],[DiscProsen2],[DiscProsen3],[DiscProsen4],[DiscProsen5],[DiscRp]" & vbCrLf &
                                                              ",[DiscNotaProsen],[DiscNotaRp],[JumlahBruto],[DPP],[PPN],[Jumlah]) VALUES (" & vbCrLf &
                                                              "@NoID,@IDHeader,@IDBarangD,@IDBarang,@IDSatuan,@Konversi,@Qty" & vbCrLf &
                                                              ",@Harga,@DiscProsen1,@DiscProsen2,@DiscProsen3,@DiscProsen4,@DiscProsen5,@DiscRp" & vbCrLf &
                                                              ",@DiscNotaProsen,@DiscNotaRp,@JumlahBruto,@DPP,@PPN,@Jumlah)"
                                        Else
                                            com.CommandText = "UPDATE [dbo].[MBeliD] SET [IDBarangD]=@IDBarangD,[IDBarang]=@IDBarang,[IDSatuan]=@IDSatuan,[Konversi]=@Konversi,[Qty]=@Qty" & vbCrLf &
                                                              ",[Harga]=@Harga,[DiscProsen1]=@DiscProsen1,[DiscProsen2]=@DiscProsen2,[DiscProsen3]=@DiscProsen3,[DiscProsen4]=@DiscProsen4,[DiscProsen5]=@DiscProsen5,[DiscRp]=@DiscRp" & vbCrLf &
                                                              ",[DiscNotaProsen]=@DiscNotaProsen,[DiscNotaRp]=@DiscNotaRp,[JumlahBruto]=@JumlahBruto,[DPP]=@DPP,[PPN]=@PPN,[Jumlah]=@Jumlah WHERE NoID=@NoID"
                                        End If
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NoID
                                        com.Parameters.Add(New SqlParameter("@IDHeader", SqlDbType.BigInt)).Value = IDHeader
                                        com.Parameters.Add(New SqlParameter("@IDBarangD", SqlDbType.BigInt)).Value = NullToLong(txtBarcode.EditValue)
                                        com.Parameters.Add(New SqlParameter("@IDBarang", SqlDbType.BigInt)).Value = IDBarang
                                        com.Parameters.Add(New SqlParameter("@IDSatuan", SqlDbType.Int)).Value = NullTolInt(txtSatuan.EditValue)
                                        com.Parameters.Add(New SqlParameter("@Konversi", SqlDbType.Int)).Value = NullToDbl(txtKonversi.EditValue)
                                        com.Parameters.Add(New SqlParameter("@Qty", SqlDbType.Float)).Value = Bulatkan(NullToDbl(txtQty.EditValue), 3)
                                        com.Parameters.Add(New SqlParameter("@Harga", SqlDbType.Float)).Value = Bulatkan(NullToDbl(txtHargaBeli.EditValue), 2)
                                        com.Parameters.Add(New SqlParameter("@DiscProsen1", SqlDbType.Float)).Value = Bulatkan(NullToDbl(txtDiscProsen1.EditValue), 2)
                                        com.Parameters.Add(New SqlParameter("@DiscProsen2", SqlDbType.Float)).Value = Bulatkan(NullToDbl(txtDiscProsen2.EditValue), 2)
                                        com.Parameters.Add(New SqlParameter("@DiscProsen3", SqlDbType.Float)).Value = Bulatkan(NullToDbl(txtDiscProsen3.EditValue), 2)
                                        com.Parameters.Add(New SqlParameter("@DiscProsen4", SqlDbType.Float)).Value = Bulatkan(NullToDbl(txtDiscProsen4.EditValue), 2)
                                        com.Parameters.Add(New SqlParameter("@DiscProsen5", SqlDbType.Float)).Value = Bulatkan(NullToDbl(txtDiscProsen5.EditValue), 2)
                                        com.Parameters.Add(New SqlParameter("@DiscRp", SqlDbType.Float)).Value = Bulatkan(NullToDbl(txtDiscRp.EditValue), 2)
                                        com.Parameters.Add(New SqlParameter("@DiscNotaProsen", SqlDbType.Float)).Value = 0.0
                                        com.Parameters.Add(New SqlParameter("@DiscNotaRp", SqlDbType.Float)).Value = 0.0
                                        com.Parameters.Add(New SqlParameter("@JumlahBruto", SqlDbType.Float)).Value = Bulatkan(TotalBruto, 2)
                                        com.Parameters.Add(New SqlParameter("@DPP", SqlDbType.Float)).Value = Bulatkan(DPP, 2)
                                        com.Parameters.Add(New SqlParameter("@PPN", SqlDbType.Float)).Value = Bulatkan(PPN, 2)
                                        com.Parameters.Add(New SqlParameter("@Jumlah", SqlDbType.Float)).Value = Bulatkan(NullToDbl(txtJumlah.EditValue), 2)
                                        com.ExecuteNonQuery()

                                        com.Parameters.Clear()

                                        com.CommandText = "UPDATE MBeliD SET " & vbCrLf &
                                                          "DiscNotaProsen=ROUND(@DiscNotaProsen, 2)" & vbCrLf &
                                                          ",DiscNotaRp=ROUND((@DiscNotaRp/(MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))) * 100.0, 2)" & vbCrLf &
                                                          "FROM MBeli " & vbCrLf &
                                                          "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID" & vbCrLf &
                                                          "WHERE MBeliD.IDHeader=" & NoID
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@DiscNotaProsen", SqlDbType.Float)).Value = NullToDbl(frmPemanggil.txtDiscProsen.EditValue)
                                        com.Parameters.Add(New SqlParameter("@DiscNotaRp", SqlDbType.Float)).Value = NullToDbl(frmPemanggil.txtDiscRp.EditValue)
                                        com.ExecuteNonQuery()

                                        com.CommandText = "UPDATE MBeliD SET DiscNotaRp=MBeli.DiscNotaRp-(ISNULL(MBeliD2.DiscNotaRp, 0)-ISNULL(MBeliD.DiscNotaRp, 0))" & vbCrLf &
                                                          "FROM MBeliD " & vbCrLf &
                                                          "INNER JOIN (SELECT IDHeader, MAX(NoID) AS NoID, SUM(DiscNotaRp) AS DiscNotaRp FROM MBeliD GROUP BY IDHeader) AS MBeliD2 ON MBeliD2.NoID=MBeliD.NoID" & vbCrLf &
                                                          "INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDHeader" & vbCrLf &
                                                          "WHERE MBeliD.IDHeader=" & NoID
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@DiscNotaProsen", SqlDbType.Float)).Value = NullToDbl(frmPemanggil.txtDiscProsen.EditValue)
                                        com.Parameters.Add(New SqlParameter("@DiscNotaRp", SqlDbType.Float)).Value = NullToDbl(frmPemanggil.txtDiscRp.EditValue)
                                        com.ExecuteNonQuery()

                                        com.CommandText = "UPDATE MBeliD SET " & vbCrLf &
                                                          "PPN=ROUND(CASE WHEN MBeli.IDTypePajak=0 THEN 0 WHEN MBeli.IDTypePajak=1 THEN ROUND((MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))-MBeliD.DiscNotaRp, 2)/1.1 ELSE ROUND((MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))-MBeliD.DiscNotaRp, 2)/1.0 END * 0.1, 0), " & vbCrLf &
                                                          "DPP=ROUND(CASE WHEN MBeli.IDTypePajak=0 THEN 0 WHEN MBeli.IDTypePajak=1 THEN ROUND((MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))-MBeliD.DiscNotaRp, 2)/1.1 ELSE ROUND((MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))-MBeliD.DiscNotaRp, 2)/1.0 END, 0) " & vbCrLf &
                                                          "FROM MBeli " & vbCrLf &
                                                          "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf &
                                                          "WHERE MBeliD.IDHeader=" & NoID
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@DiscNotaProsen", SqlDbType.Float)).Value = NullToDbl(frmPemanggil.txtDiscProsen.EditValue)
                                        com.Parameters.Add(New SqlParameter("@DiscNotaRp", SqlDbType.Float)).Value = NullToDbl(frmPemanggil.txtDiscRp.EditValue)
                                        com.ExecuteNonQuery()

                                        com.CommandText = "UPDATE MBeliD SET " & vbCrLf &
                                                          "PPN=ISNULL(MBeli.PPN, 0)-(ISNULL(Detil.PPN, 0)-ISNULL(MBeliD.PPN, 0)), " & vbCrLf &
                                                          "DPP=ISNULL(MBeli.DPP, 0)-(ISNULL(Detil.DPP, 0)-ISNULL(MBeliD.DPP, 0)) " & vbCrLf &
                                                          "FROM MBeli " & vbCrLf &
                                                          "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID" & vbCrLf &
                                                          "INNER JOIN (SELECT IDHeader, SUM(DPP) AS DPP, SUM(PPN) AS PPN, MAX(NoID) AS NoID FROM MBeliD GROUP BY IDHeader) AS Detil ON Detil.IDHeader=MBeli.NoID AND Detil.NoID=MBeliD.NoID" & vbCrLf &
                                                          "WHERE MBeliD.IDHeader=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "UPDATE MBeliD SET " & vbCrLf &
                                                          "Jumlah=CASE WHEN MBeli.IDTypePajak=0 THEN ROUND((MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))-MBeliD.DiscNotaRp, 2) ELSE MBeliD.DPP+MBeliD.PPN END " & vbCrLf &
                                                          "FROM MBeli " & vbCrLf &
                                                          "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf &
                                                          "WHERE MBeliD.IDHeader=" & NoID
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@DiscNotaProsen", SqlDbType.Float)).Value = NullToDbl(frmPemanggil.txtDiscProsen.EditValue)
                                        com.Parameters.Add(New SqlParameter("@DiscNotaRp", SqlDbType.Float)).Value = NullToDbl(frmPemanggil.txtDiscRp.EditValue)
                                        com.ExecuteNonQuery()

                                        com.Transaction.Commit()

                                        DialogResult = Windows.Forms.DialogResult.OK
                                        Me.Close()
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
End Class
Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.App.Public
Imports DevExpress.XtraEditors.Repository
Imports Dapper
Imports DevExpress.XtraTab.ViewInfo

Public Class frmEntriMaterial
    Private pStatus As pStatusForm
    Public data As New CtrlSoft.Dto.Model.MMaterial

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Public Sub New(ByVal NoID As Guid)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.data.NoID = NoID
    End Sub

    Private Sub frmEntriRole_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

    Public Sub LoadData(ByVal NoID As Guid)
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

                                Args.Add("@NoID", NoID)
                                data = cn.Query(Of CtrlSoft.Dto.Model.MMaterial)("SELECT * FROM MMaterial(NOLOCK) WHERE NoID=@NoID", New DynamicParameters(Args)).SingleOrDefault

                                If (data IsNot Nothing) Then
                                    pStatus = pStatusForm.Edit
                                    txtKode.Text = data.Kode
                                    txtNama.Text = data.Nama
                                    txtKeterangan.Text = data.Keterangan
                                    txtBarcode.EditValue = data.IDBarangD
                                    txtQty.EditValue = data.Qty
                                    txtKonversi.EditValue = data.Konversi
                                    txtHargaPokok.EditValue = data.HargaPokok
                                    txtJumlah.EditValue = data.Jumlah
                                    ckAktif.Checked = data.IsActive
                                Else
                                    pStatus = pStatusForm.Baru
                                    data = New Dto.Model.MMaterial()
                                    ckAktif.Checked = True
                                    txtKode.Text = Repository.RepSQLServer.GetKodeMaterial()
                                    txtNama.Text = ""
                                    txtKeterangan.Text = ""
                                    txtBarcode.EditValue = -1
                                    txtQty.EditValue = 0
                                    txtKonversi.EditValue = 0.0
                                    txtHargaPokok.EditValue = 0.0
                                    txtJumlah.EditValue = 0.0
                                    ckAktif.Checked = True
                                End If

                                data.MMaterialDs = cn.Query(Of CtrlSoft.Dto.Model.MMaterialD)("SELECT MMaterialD.*," & vbCrLf &
                                                                                              "MBarangD.Barcode, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MSatuan.Kode AS Satuan" & vbCrLf &
                                                                                              "FROM MMaterialD(NOLOCK)" & vbCrLf &
                                                                                              "INNER JOIN MBarangD(NOLOCK) ON MBarangD.NoID=MMaterialD.IDBarangD" & vbCrLf &
                                                                                              "INNER JOIN MBarang(NOLOCK) ON MBarang.NoID=MMaterialD.IDBarang" & vbCrLf &
                                                                                              "INNER JOIN MSatuan(NOLOCK) ON MSatuan.NoID=MMaterialD.IDSatuan" & vbCrLf &
                                                                                              "WHERE MMaterialD.IDMaterial=@NoID", New DynamicParameters(Args)).ToList
                                data.MMaterialDBiayas = cn.Query(Of CtrlSoft.Dto.Model.MMaterialDBiaya)("SELECT MMaterialDBiaya.*," & vbCrLf &
                                                                                              "MAkun.Nama AS Akun" & vbCrLf &
                                                                                              "FROM MMaterialDBiaya(NOLOCK)" & vbCrLf &
                                                                                              "INNER JOIN MAkun(NOLOCK) ON MAKun.ID=MMaterialDBiaya.IDAkun" & vbCrLf &
                                                                                              "WHERE MMaterialDBiaya.IDMaterial=@NoID", New DynamicParameters(Args)).ToList
                                data.MMaterialDSisas = cn.Query(Of CtrlSoft.Dto.Model.MMaterialDSisa)("SELECT MMaterialDSisa.*," & vbCrLf &
                                                                                              "MBarangD.Barcode, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MSatuan.Kode AS Satuan" & vbCrLf &
                                                                                              "FROM MMaterialDSisa(NOLOCK)" & vbCrLf &
                                                                                              "INNER JOIN MBarangD(NOLOCK) ON MBarangD.NoID=MMaterialDSisa.IDBarangD" & vbCrLf &
                                                                                              "INNER JOIN MBarang(NOLOCK) ON MBarang.NoID=MMaterialDSisa.IDBarang" & vbCrLf &
                                                                                              "INNER JOIN MSatuan(NOLOCK) ON MSatuan.NoID=MMaterialDSisa.IDSatuan" & vbCrLf &
                                                                                              "WHERE MMaterialDSisa.IDMaterial=@NoID", New DynamicParameters(Args)).ToList
                                MMaterialDBindingSource.DataSource = data.MMaterialDs
                                GCBahan.DataSource = MMaterialDBindingSource
                                MMaterialDBiayaBindingSource.DataSource = data.MMaterialDBiayas
                                MMaterialDSisaBindingSource.DataSource = data.MMaterialDSisas
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

                    gvBahan.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & gvBahan.Name & ".xml")
                    gvBarcode.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & gvBarcode.Name & ".xml")
                    gvBiaya.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & gvBiaya.Name & ".xml")
                    gvSatuan.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & gvSatuan.Name & ".xml")
                    gvSisa.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & gvSisa.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub HitungMaterial()
        Try
            Dim Bahan As Decimal = data.MMaterialDs.ToList().Sum(Function(m) m.Jumlah)
            Dim Biaya As Decimal = data.MMaterialDBiayas.ToList().Sum(Function(m) m.Jumlah)
            Dim Sisa As Decimal = data.MMaterialDSisas.ToList().Sum(Function(m) m.Jumlah)

            DxErrorProvider1.ClearErrors()
            If NullToDbl(txtQty.EditValue) > 0 Then
                txtHargaPokok.EditValue = Bulatkan((Bahan + Biaya - Sisa) / txtQty.EditValue, 2)
                txtJumlah.EditValue = Bulatkan(Bahan + Biaya - Sisa, 2)
            Else
                txtHargaPokok.EditValue = 0.0
                txtJumlah.EditValue = 0.0
                DxErrorProvider1.SetError(txtQty, "Qty Hasil harus diisi dengan benar!")
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub mnSimpan_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSimpan.ItemClick
        DxErrorProvider1.ClearErrors()
        HitungMaterial()

        If txtKode.Text = "" Then
            DxErrorProvider1.SetError(txtKode, "Kode harus diisi!")
        End If
        If txtNama.Text = "" Then
            DxErrorProvider1.SetError(txtNama, "Nama harus diisi!")
        End If
        If txtBarcode.Text = "" OrElse txtKodeBarang.Text = "" OrElse txtNamaBarang.Text = "" Then
            DxErrorProvider1.SetError(txtBarcode, "Barang harus diisi!")
        End If
        If txtQty.EditValue <= 0 Then
            DxErrorProvider1.SetError(txtQty, "Qty Barang harus diisi!")
        End If
        If data.MMaterialDs.Count = 0 Then
            DxErrorProvider1.SetError(GCBahan, "Barang Penyusun harus diisi!")
        End If
        If data.MMaterialDs.Sum(Function(m) m.Jumlah) + data.MMaterialDBiayas.Sum(Function(m) m.Jumlah) + data.MMaterialDSisas.Sum(Function(m) m.Jumlah) <> txtJumlah.EditValue Then
            DxErrorProvider1.SetError(GCBahan, "Rumus masih salah! Coba cek nilai bahan baku berseta hasil.")
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

                                    com.CommandText = "SELECT COUNT(NoID) FROM MMaterial WHERE Kode=@Kode And NoID<>@NoID"
                                    com.Parameters.Clear()
                                    com.Parameters.AddWithValue("@Kode", txtKode.Text)
                                    com.Parameters.AddWithValue("@NoID", data.NoID)
                                    If NullToLong(com.ExecuteScalar()) >= 1 Then
                                        DxErrorProvider1.SetError(txtKode, "Kode sudah dipakai.", DXErrorProvider.ErrorType.Warning)
                                    End If

                                    If Not DxErrorProvider1.HasErrors Then
                                        With data
                                            .Kode = txtKode.Text
                                            .Nama = txtNama.Text
                                            .IDBarangD = NullToLong(txtBarcode.EditValue)
                                            .IDBarang = NullToLong(txtBarcode.Tag)
                                            .IDSatuan = NullToLong(txtSatuan.EditValue)
                                            .IDUser = UserLogin.NoID
                                            .Qty = txtQty.EditValue
                                            .Konversi = txtKonversi.EditValue
                                            .Keterangan = txtKeterangan.Text
                                            .HargaPokok = txtHargaPokok.EditValue
                                            .Jumlah = txtJumlah.EditValue
                                            .IsActive = ckAktif.Checked
                                        End With

                                        If pStatus = pStatusForm.Baru Then
                                            data.NoID = System.Guid.NewGuid
                                            com.CommandText = "INSERT INTO [dbo].[MMaterial] ([NoID],[Kode],[Nama],[Keterangan],[IsActive],[IDBarangD],[IDBarang],[IDSatuan],[Konversi],[Qty],[HargaPokok],[Jumlah],[IDUser],[TanggalUpdate])" & vbCrLf &
                                                              "VALUES (@NoID,@Kode,@Nama,@Keterangan,@IsActive,@IDBarangD,@IDBarang,@IDSatuan,@Konversi,@Qty,@HargaPokok,@Jumlah,@IDUser,GETDATE())"
                                        Else
                                            com.CommandText = "UPDATE [dbo].[MMaterial] SET [Kode]=@Kode,[Nama]=@Nama,[Keterangan]=@Keterangan,[IsActive]=@IsActive,[IDBarangD]=@IDBarangD,[IDBarang]=@IDBarang,[IDSatuan]=@IDSatuan," & vbCrLf &
                                                              "[Konversi]=@Konversi,[Qty]=@Qty,[HargaPokok]=@HargaPokok,[Jumlah]=@Jumlah,[IDUser]=@IDUser,[TanggalUpdate]=GETDATE()" & vbCrLf &
                                                              "WHERE NoID=@NoID"
                                        End If
                                        com.Parameters.Clear()
                                        com.Parameters.AddWithValue("@NoID", data.NoID)
                                        com.Parameters.AddWithValue("@Kode", data.Kode)
                                        com.Parameters.AddWithValue("@Nama", data.Nama)
                                        com.Parameters.AddWithValue("@Keterangan", data.Keterangan)
                                        com.Parameters.AddWithValue("@IsActive", data.IsActive)
                                        com.Parameters.AddWithValue("@IDBarangD", data.IDBarangD)
                                        com.Parameters.AddWithValue("@IDBarang", data.IDBarang)
                                        com.Parameters.AddWithValue("@IDSatuan", data.IDSatuan)
                                        com.Parameters.AddWithValue("@Konversi", data.Konversi)
                                        com.Parameters.AddWithValue("@Qty", data.Qty)
                                        com.Parameters.AddWithValue("@HargaPokok", data.HargaPokok)
                                        com.Parameters.AddWithValue("@Jumlah", data.Jumlah)
                                        com.Parameters.AddWithValue("@IDUser", data.IDUser)
                                        com.ExecuteNonQuery()

                                        If (data.MMaterialDs.Count >= 1) Then
                                            com.CommandText = "DELETE FROM MMaterialD WHERE [IDMaterial]=@IDMaterial AND [NoID] NOT IN (" & String.Join(", ", data.MMaterialDs.Select(Function(m) "'" & m.NoID.ToString() & "'").ToArray()) & ")"
                                            com.Parameters.Clear()
                                            com.Parameters.AddWithValue("@IDMaterial", data.NoID)
                                            com.ExecuteNonQuery()
                                        End If

                                        If (data.MMaterialDBiayas.Count >= 1) Then
                                            com.CommandText = "DELETE FROM MMaterialDBiaya WHERE [IDMaterial]=@IDMaterial AND [NoID] NOT IN (" & String.Join(", ", data.MMaterialDBiayas.Select(Function(m) "'" & m.NoID.ToString() & "'").ToArray()) & ")"
                                            com.Parameters.Clear()
                                            com.Parameters.AddWithValue("@IDMaterial", data.NoID)
                                            com.ExecuteNonQuery()
                                        End If

                                        If (data.MMaterialDSisas.Count >= 1) Then
                                            com.CommandText = "DELETE FROM MMaterialDSisa WHERE [IDMaterial]=@IDMaterial AND [NoID] NOT IN (" & String.Join(", ", data.MMaterialDSisas.Select(Function(m) "'" & m.NoID.ToString() & "'").ToArray()) & ")"
                                            com.Parameters.Clear()
                                            com.Parameters.AddWithValue("@IDMaterial", data.NoID)
                                            com.ExecuteNonQuery()
                                        End If

                                        For Each item In data.MMaterialDs
                                            com.CommandText = "IF EXISTS (SELECT 1 FROM dbo.MMaterialD WHERE NoID = @NoID)" & vbCrLf &
                                                              "BEGIN" & vbCrLf &
                                                              "UPDATE MMaterialD SET [IDMaterial]=@IDMaterial, [IDBarangD]=@IDBarangD, [IDBarang]=@IDBarang, [IDSatuan]=@IDSatuan, [Konversi]=@Konversi, [Qty]=@Qty, [HargaPokok]=@HargaPokok, [Jumlah]=@Jumlah WHERE NoID=@NoID" & vbCrLf &
                                                              "END" & vbCrLf &
                                                              "ELSE" & vbCrLf &
                                                              "BEGIN" & vbCrLf &
                                                              "INSERT MMaterialD ([NoID], [IDMaterial], [IDBarangD], [IDBarang], [IDSatuan], [Konversi], [Qty], [HargaPokok], [Jumlah]) VALUES (@NoID, @IDMaterial, @IDBarangD, @IDBarang, @IDSatuan, @Konversi, @Qty, @HargaPokok, @Jumlah)" & vbCrLf &
                                                              "END"
                                            com.Parameters.Clear()
                                            com.Parameters.AddWithValue("@NoID", item.NoID)
                                            com.Parameters.AddWithValue("@IDMaterial", data.NoID)
                                            com.Parameters.AddWithValue("@IDBarangD", item.IDBarangD)
                                            com.Parameters.AddWithValue("@IDBarang", item.IDBarang)
                                            com.Parameters.AddWithValue("@IDSatuan", item.IDSatuan)
                                            com.Parameters.AddWithValue("@Konversi", item.Konversi)
                                            com.Parameters.AddWithValue("@Qty", item.Qty)
                                            com.Parameters.AddWithValue("@HargaPokok", item.HargaPokok)
                                            com.Parameters.AddWithValue("@Jumlah", item.Jumlah)
                                            com.ExecuteNonQuery()
                                        Next

                                        For Each item In data.MMaterialDBiayas
                                            com.CommandText = "IF EXISTS (SELECT 1 FROM dbo.MMaterialDBiaya WHERE NoID = @NoID)" & vbCrLf &
                                                              "BEGIN" & vbCrLf &
                                                              "UPDATE MMaterialDBiaya SET [IDMaterial]=@IDMaterial, [IDAkun]=@IDAkun, [Jumlah]=@Jumlah, [Keterangan]=@Keterangan WHERE NoID=@NoID" & vbCrLf &
                                                              "END" & vbCrLf &
                                                              "ELSE" & vbCrLf &
                                                              "BEGIN" & vbCrLf &
                                                              "INSERT MMaterialDBiaya ([NoID], [IDMaterial], [IDAkun], [Jumlah], [Keterangan]) VALUES (@NoID, @IDMaterial, @IDAkun, @Jumlah, @Keterangan)" & vbCrLf &
                                                              "END"
                                            com.Parameters.Clear()
                                            com.Parameters.AddWithValue("@NoID", item.NoID)
                                            com.Parameters.AddWithValue("@IDMaterial", data.NoID)
                                            com.Parameters.AddWithValue("@IDAkun", item.IDAkun)
                                            com.Parameters.AddWithValue("@Jumlah", item.Jumlah)
                                            com.Parameters.AddWithValue("@Keterangan", item.Keterangan)
                                            com.ExecuteNonQuery()
                                        Next

                                        For Each item In data.MMaterialDSisas
                                            com.CommandText = "IF EXISTS (SELECT 1 FROM dbo.MMaterialDSisa WHERE NoID = @NoID)" & vbCrLf &
                                                              "BEGIN" & vbCrLf &
                                                              "UPDATE MMaterialDSisa SET [IDMaterial]=@IDMaterial, [IDBarangD]=@IDBarangD, [IDBarang]=@IDBarang, [IDSatuan]=@IDSatuan, [Konversi]=@Konversi, [Qty]=@Qty, [HargaPokok]=@HargaPokok, [Jumlah]=@Jumlah WHERE NoID=@NoID" & vbCrLf &
                                                              "END" & vbCrLf &
                                                              "ELSE" & vbCrLf &
                                                              "BEGIN" & vbCrLf &
                                                              "INSERT MMaterialDSisa ([NoID], [IDMaterial], [IDBarangD], [IDBarang], [IDSatuan], [Konversi], [Qty], [HargaPokok], [Jumlah]) VALUES (@NoID, @IDMaterial, @IDBarangD, @IDBarang, @IDSatuan, @Konversi, @Qty, @HargaPokok, @Jumlah)" & vbCrLf &
                                                              "END"
                                            com.Parameters.Clear()
                                            com.Parameters.AddWithValue("@NoID", item.NoID)
                                            com.Parameters.AddWithValue("@IDMaterial", data.NoID)
                                            com.Parameters.AddWithValue("@IDBarangD", item.IDBarangD)
                                            com.Parameters.AddWithValue("@IDBarang", item.IDBarang)
                                            com.Parameters.AddWithValue("@IDSatuan", item.IDSatuan)
                                            com.Parameters.AddWithValue("@Konversi", item.Konversi)
                                            com.Parameters.AddWithValue("@Qty", item.Qty)
                                            com.Parameters.AddWithValue("@HargaPokok", item.HargaPokok)
                                            com.Parameters.AddWithValue("@Jumlah", item.Jumlah)
                                            com.ExecuteNonQuery()
                                        Next
                                        com.Transaction.Commit()

                                        DialogResult = System.Windows.Forms.DialogResult.OK
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
        DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub gvBahan_DataSourceChanged(sender As Object, e As EventArgs) Handles gvBahan.DataSourceChanged, gvBarcode.DataSourceChanged, gvBiaya.DataSourceChanged, gvSatuan.DataSourceChanged, gvSisa.DataSourceChanged
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
        LoadData(data.NoID)
    End Sub

    Private Sub TabControl_CustomHeaderButtonClick(sender As Object, e As CustomHeaderButtonEventArgs) Handles TabControl.CustomHeaderButtonClick
        Select Case e.Button.Index
            Case 0
                Select Case TabControl.SelectedTabPageIndex
                    Case 0
                        Using frm As New frmEntriMaterialD(New Dto.Model.MMaterialD With {.IDMaterial = data.NoID}, data)
                            Try
                                If (frm.ShowDialog(Me) = DialogResult.OK) Then
                                    data.MMaterialDs.Add(frm.data)

                                    MMaterialDBindingSource.DataSource = data.MMaterialDs
                                    GCBahan.DataSource = MMaterialDBindingSource
                                    GCBahan.RefreshDataSource()

                                    gvBahan.ClearSelection()
                                    gvBahan.FocusedRowHandle = gvBahan.LocateByDisplayText(0, gvBahan.Columns("NoID"), frm.data.NoID.ToString)
                                    gvBahan.SelectRow(gvBahan.FocusedRowHandle)

                                    HitungMaterial()
                                End If
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    Case 1
                        Using frm As New frmEntriMaterialDBiaya(New Dto.Model.MMaterialDBiaya With {.IDMaterial = data.NoID}, data)
                            Try
                                If (frm.ShowDialog(Me) = DialogResult.OK) Then
                                    data.MMaterialDBiayas.Add(frm.data)

                                    MMaterialDBiayaBindingSource.DataSource = data.MMaterialDBiayas
                                    GCBiaya.DataSource = MMaterialDBiayaBindingSource
                                    GCBiaya.RefreshDataSource()

                                    gvBiaya.ClearSelection()
                                    gvBiaya.FocusedRowHandle = gvBiaya.LocateByDisplayText(0, gvBiaya.Columns("NoID"), frm.data.NoID.ToString)
                                    gvBiaya.SelectRow(gvBiaya.FocusedRowHandle)

                                    HitungMaterial()
                                End If
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    Case 2
                        Using frm As New FrmEntriMaterialDSisa(New Dto.Model.MMaterialDSisa With {.IDMaterial = data.NoID}, data)
                            Try
                                If (frm.ShowDialog(Me) = DialogResult.OK) Then
                                    data.MMaterialDSisas.Add(frm.dataSisa)

                                    MMaterialDSisaBindingSource.DataSource = data.MMaterialDSisas
                                    GCSisa.DataSource = MMaterialDSisaBindingSource
                                    GCSisa.RefreshDataSource()

                                    gvSisa.ClearSelection()
                                    gvSisa.FocusedRowHandle = gvSisa.LocateByDisplayText(0, gvSisa.Columns("NoID"), frm.dataSisa.NoID.ToString)
                                    gvSisa.SelectRow(gvSisa.FocusedRowHandle)

                                    HitungMaterial()
                                End If
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                End Select
            Case 1 'Edit Data
                Select Case TabControl.SelectedTabPageIndex
                    Case 0
                        Dim OldData = MMaterialDBindingSource.Current()
                        Using frm As New frmEntriMaterialD(OldData, data)
                            Try
                                If (frm.ShowDialog(Me) = DialogResult.OK) Then
                                    data.MMaterialDs.Remove(OldData)
                                    data.MMaterialDs.Add(frm.data)

                                    MMaterialDBindingSource.DataSource = data.MMaterialDs
                                    GCBahan.DataSource = MMaterialDBindingSource
                                    GCBahan.RefreshDataSource()

                                    gvBahan.ClearSelection()
                                    gvBahan.FocusedRowHandle = gvBahan.LocateByDisplayText(0, gvBahan.Columns("NoID"), frm.data.NoID.ToString)
                                    gvBahan.SelectRow(gvBahan.FocusedRowHandle)

                                    HitungMaterial()
                                End If
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    Case 1
                        Dim OldData = MMaterialDBiayaBindingSource.Current()
                        Using frm As New frmEntriMaterialDBiaya(OldData, data)
                            Try
                                If (frm.ShowDialog(Me) = DialogResult.OK) Then
                                    data.MMaterialDBiayas.Remove(OldData)
                                    data.MMaterialDBiayas.Add(frm.data)

                                    MMaterialDBiayaBindingSource.DataSource = data.MMaterialDBiayas
                                    GCBiaya.DataSource = MMaterialDBiayaBindingSource
                                    GCBiaya.RefreshDataSource()

                                    gvBiaya.ClearSelection()
                                    gvBiaya.FocusedRowHandle = gvBiaya.LocateByDisplayText(0, gvBiaya.Columns("NoID"), frm.data.NoID.ToString)
                                    gvBiaya.SelectRow(gvBiaya.FocusedRowHandle)

                                    HitungMaterial()
                                End If
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    Case 2
                        Dim OldData = MMaterialDSisaBindingSource.Current()
                        Using frm As New FrmEntriMaterialDSisa(OldData, data)
                            Try
                                If (frm.ShowDialog(Me) = DialogResult.OK) Then
                                    data.MMaterialDSisas.Remove(OldData)
                                    data.MMaterialDSisas.Add(frm.dataSisa)

                                    MMaterialDSisaBindingSource.DataSource = data.MMaterialDSisas
                                    GCSisa.DataSource = MMaterialDSisaBindingSource
                                    GCSisa.RefreshDataSource()

                                    gvSisa.ClearSelection()
                                    gvSisa.FocusedRowHandle = gvSisa.LocateByDisplayText(0, gvSisa.Columns("NoID"), frm.dataSisa.NoID.ToString)
                                    gvSisa.SelectRow(gvSisa.FocusedRowHandle)

                                    HitungMaterial()
                                End If
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                End Select
            Case 2 'Hapus Data
                Select Case TabControl.SelectedTabPageIndex
                    Case 0
                        For Each i In gvBahan.GetSelectedRows
                            data.MMaterialDs.Remove(data.MMaterialDs.Where(Function(m) m.NoID = gvBahan.GetRowCellValue(i, "NoID")).SingleOrDefault)
                            Application.DoEvents()
                        Next
                        MMaterialDBindingSource.DataSource = data.MMaterialDs
                        GCBahan.DataSource = MMaterialDBindingSource
                        GCBahan.RefreshDataSource()
                        HitungMaterial()
                    Case 1
                        For Each i In gvBiaya.GetSelectedRows
                            data.MMaterialDBiayas.Remove(data.MMaterialDBiayas.Where(Function(m) m.NoID = gvBiaya.GetRowCellValue(i, "NoID")).SingleOrDefault)
                            Application.DoEvents()
                        Next
                        MMaterialDBiayaBindingSource.DataSource = data.MMaterialDBiayas
                        GCBiaya.DataSource = MMaterialDBiayaBindingSource
                        GCBiaya.RefreshDataSource()
                        HitungMaterial()
                    Case 2
                        For Each i In gvSisa.GetSelectedRows
                            data.MMaterialDSisas.Remove(data.MMaterialDSisas.Where(Function(m) m.NoID = gvSisa.GetRowCellValue(i, "NoID")).SingleOrDefault)
                            Application.DoEvents()
                        Next
                        MMaterialDSisaBindingSource.DataSource = data.MMaterialDSisas
                        GCSisa.DataSource = MMaterialDSisaBindingSource
                        GCSisa.RefreshDataSource()
                        HitungMaterial()
                End Select
        End Select
    End Sub
End Class
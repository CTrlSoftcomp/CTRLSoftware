Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports CtrlSoft.Utils
Imports DevExpress.XtraEditors.Repository

Public Class frmEntriBarang
    Public NoID As Long = -1
    Private pStatus As pStatusForm

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSimpan.Click
        DxErrorProvider1.ClearErrors()
        If txtKategori.Text = "" Then
            DxErrorProvider1.SetError(txtKategori, "Kategori harus diisi!")
        End If
        If txtKode.Text = "" Then
            DxErrorProvider1.SetError(txtKode, "Kode harus diisi!")
        End If
        If txtNama.Text = "" Then
            DxErrorProvider1.SetError(txtNama, "Nama harus diisi!")
        End If
        If txtBarcode.Text = "" Then
            DxErrorProvider1.SetError(txtBarcode, "Barcode harus diisi!")
        End If
        If txtSatuanBeli.Text = "" Then
            DxErrorProvider1.SetError(txtSatuanBeli, "Satuan beli harus diisi!")
        End If
        If txtTypePajak.Text = "" Then
            DxErrorProvider1.SetError(txtTypePajak, "Type Pajak harus diisi!")
        End If
        If txtSatuanJual.Text = "" Then
            DxErrorProvider1.SetError(txtSatuanJual, "Satuan jual harus diisi!")
        End If
        If txtIsiCtn.EditValue <= 0 Then
            DxErrorProvider1.SetError(txtIsiCtn, "Isi Ctn salah!")
        End If
        If txtHargaJualA.EditValue < txtModal.EditValue Then
            DxErrorProvider1.SetError(txtHargaJualA, "Harga Jual Retail dibawah modal!")
        End If
        If txtHargaJualB.EditValue <> 0 AndAlso txtHargaJualB.EditValue < txtModal.EditValue Then
            DxErrorProvider1.SetError(txtHargaJualB, "Harga Jual Grosir dibawah modal!")
        End If
        If txtSupplier1.Text = "" AndAlso txtSupplier2.Text = "" AndAlso txtSupplier3.Text = "" Then
            DxErrorProvider1.SetError(txtSupplier1, "Supplier harus diisi!")
        End If

        Dim IDBarangD As Long = -1
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

                                    com.CommandText = "SELECT COUNT(NoID) FROM MBarang WHERE DefaultBarcode='" & FixApostropi(txtBarcode.Text) & "' AND NoID<>" & Me.NoID
                                    If NullToLong(com.ExecuteScalar()) >= 1 Then
                                        DxErrorProvider1.SetError(txtBarcode, "Barcode sudah dipakai!")
                                    End If

                                    com.CommandText = "SELECT COUNT(NoID) FROM MBarang WHERE Kode='" & FixApostropi(txtKode.Text) & "' AND NoID<>" & Me.NoID
                                    If NullToLong(com.ExecuteScalar()) >= 1 Then
                                        DxErrorProvider1.SetError(txtKode, "Kode sudah dipakai!")
                                    End If

                                    com.CommandText = "SELECT COUNT(NoID) FROM MBarang WHERE Nama='" & FixApostropi(txtKode.Text) & "' AND NoID<>" & Me.NoID
                                    If NullToLong(com.ExecuteScalar()) >= 1 Then
                                        DxErrorProvider1.SetError(txtNama, "Nama sudah dipakai!")
                                    End If

                                    If Not DxErrorProvider1.HasErrors Then
                                        If pStatus = pStatusForm.Baru Then
                                            com.CommandText = "SELECT MAX(NoID) FROM MBarang"
                                            NoID = NullToLong(com.ExecuteScalar()) + 1

                                            com.CommandText = "INSERT INTO [dbo].[MBarang]" & vbCrLf & _
                                                              "([IDUser],[TanggalUpdate],[IDKategori],[NoID],[Kode],[Nama],[DefaultBarcode],[Alias]" & vbCrLf & _
                                                              ",[Keterangan],[IsActive],[IDTypePajak],[IDSupplier1]" & vbCrLf & _
                                                              ",[IDSupplier2],[IDSupplier3],[HargaBeli],[IDSatuanBeli]" & vbCrLf & _
                                                              ",[IsiCtn],[HargaBeliPcsBruto],[DiscProsen1],[DiscProsen2],[DiscProsen3]" & vbCrLf & _
                                                              ",[DiscProsen4],[DiscProsen5],[DiscRp],[HargaBeliPcs],[IDSatuan],[ProsenUpA]" & vbCrLf & _
                                                              ",[HargaJualA],[ProsenUpB],[HargaJualB]) VALUES" & vbCrLf & _
                                                              "(@IDUser,GETDATE(),@IDKategori,@NoID,@Kode,@Nama,@DefaultBarcode,@Alias" & vbCrLf & _
                                                              ",@Keterangan,@IsActive,@IDTypePajak,@IDSupplier1" & vbCrLf & _
                                                              ",@IDSupplier2,@IDSupplier3,@HargaBeli,@IDSatuanBeli" & vbCrLf & _
                                                              ",@IsiCtn,@HargaBeliPcsBruto,@DiscProsen1,@DiscProsen2,@DiscProsen3" & vbCrLf & _
                                                              ",@DiscProsen4,@DiscProsen5,@DiscRp,@HargaBeliPcs,@IDSatuan,@ProsenUpA" & vbCrLf & _
                                                              ",@HargaJualA,@ProsenUpB,@HargaJualB)"
                                        Else
                                            com.CommandText = "UPDATE [dbo].[MBarang] SET " & vbCrLf & _
                                                              "[IDUser]=@IDUser,[TanggalUpdate]=GETDATE(),[IDKategori]=@IDKategori,[Kode]=@Kode,[Nama]=@Nama,[DefaultBarcode]=@DefaultBarcode,[Alias]=@Alias" & vbCrLf & _
                                                              ",[Keterangan]=@Keterangan,[IsActive]=@IsActive,[IDTypePajak]=@IDTypePajak,[IDSupplier1]=@IDSupplier1" & vbCrLf & _
                                                              ",[IDSupplier2]=@IDSupplier2,[IDSupplier3]=@IDSupplier3,[HargaBeli]=@HargaBeli,[IDSatuanBeli]=@IDSatuanBeli" & vbCrLf & _
                                                              ",[IsiCtn]=@IsiCtn,[HargaBeliPcsBruto]=@HargaBeliPcsBruto,[DiscProsen1]=@DiscProsen1,[DiscProsen2]=@DiscProsen2,[DiscProsen3]=@DiscProsen3" & vbCrLf & _
                                                              ",[DiscProsen4]=@DiscProsen4,[DiscProsen5]=@DiscProsen5,[DiscRp]=@DiscRp,[HargaBeliPcs]=@HargaBeliPcs,[IDSatuan]=@IDSatuan,[ProsenUpA]=@ProsenUpA" & vbCrLf & _
                                                              ",[HargaJualA]=@HargaJualA,[ProsenUpB]=@ProsenUpB,[HargaJualB]=@HargaJualB WHERE NoID=@NoID"
                                        End If
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@IDUser", SqlDbType.Int)).Value = Utils.UserLogin.NoID
                                        com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NoID
                                        com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = txtKode.Text
                                        com.Parameters.Add(New SqlParameter("@Alias", SqlDbType.VarChar)).Value = txtAlias.Text
                                        com.Parameters.Add(New SqlParameter("@Nama", SqlDbType.VarChar)).Value = txtNama.Text
                                        com.Parameters.Add(New SqlParameter("@IDKategori", SqlDbType.Int)).Value = NullToLong(txtKategori.EditValue)
                                        com.Parameters.Add(New SqlParameter("@IsActive", SqlDbType.Bit)).Value = ckAktif.Checked
                                        com.Parameters.Add(New SqlParameter("@DefaultBarcode", SqlDbType.VarChar)).Value = txtBarcode.Text
                                        com.Parameters.Add(New SqlParameter("@Keterangan", SqlDbType.VarChar)).Value = txtKeterangan.Text
                                        com.Parameters.Add(New SqlParameter("@IDTypePajak", SqlDbType.Int)).Value = txtTypePajak.EditValue
                                        com.Parameters.Add(New SqlParameter("@IDSupplier1", SqlDbType.Int)).Value = txtSupplier1.EditValue
                                        com.Parameters.Add(New SqlParameter("@IDSupplier2", SqlDbType.Int)).Value = txtSupplier2.EditValue
                                        com.Parameters.Add(New SqlParameter("@IDSupplier3", SqlDbType.Int)).Value = txtSupplier3.EditValue
                                        com.Parameters.Add(New SqlParameter("@HargaBeli", SqlDbType.Money)).Value = txtHargaBeli.EditValue
                                        com.Parameters.Add(New SqlParameter("@IDSatuanBeli", SqlDbType.Int)).Value = txtSatuanBeli.EditValue
                                        com.Parameters.Add(New SqlParameter("@IsiCtn", SqlDbType.Int)).Value = txtIsiCtn.EditValue
                                        com.Parameters.Add(New SqlParameter("@HargaBeliPcsBruto", SqlDbType.Money)).Value = Bulatkan(txtHargaBeli.EditValue / txtIsiCtn.EditValue, 2)
                                        com.Parameters.Add(New SqlParameter("@DiscProsen1", SqlDbType.Money)).Value = Bulatkan(txtDiscProsen1.EditValue, 2)
                                        com.Parameters.Add(New SqlParameter("@DiscProsen2", SqlDbType.Money)).Value = Bulatkan(txtDiscProsen2.EditValue, 2)
                                        com.Parameters.Add(New SqlParameter("@DiscProsen3", SqlDbType.Money)).Value = Bulatkan(txtDiscProsen3.EditValue, 2)
                                        com.Parameters.Add(New SqlParameter("@DiscProsen4", SqlDbType.Money)).Value = Bulatkan(txtDiscProsen4.EditValue, 2)
                                        com.Parameters.Add(New SqlParameter("@DiscProsen5", SqlDbType.Money)).Value = Bulatkan(txtDiscProsen5.EditValue, 2)
                                        com.Parameters.Add(New SqlParameter("@DiscRp", SqlDbType.Money)).Value = Bulatkan(txtDiscRp.EditValue, 2)
                                        com.Parameters.Add(New SqlParameter("@HargaBeliPcs", SqlDbType.Money)).Value = Bulatkan(txtModal.EditValue, 2)
                                        com.Parameters.Add(New SqlParameter("@IDSatuan", SqlDbType.Int)).Value = txtSatuanJual.EditValue
                                        com.Parameters.Add(New SqlParameter("@ProsenUpA", SqlDbType.Float)).Value = Bulatkan(txtProsenUpA.EditValue, 2)
                                        com.Parameters.Add(New SqlParameter("@ProsenUpB", SqlDbType.Float)).Value = Bulatkan(txtProsenUpB.EditValue, 2)
                                        com.Parameters.Add(New SqlParameter("@HargaJualA", SqlDbType.Money)).Value = Bulatkan(txtHargaJualA.EditValue, 2)
                                        com.Parameters.Add(New SqlParameter("@HargaJualB", SqlDbType.Money)).Value = Bulatkan(txtHargaJualB.EditValue, 2)
                                        com.ExecuteNonQuery()

                                        If pStatus = pStatusForm.Baru Then
                                            com.CommandText = "SELECT MAX(NoID) FROM MBarangD"
                                            com.Parameters.Clear()
                                            IDBarangD = NullToLong(com.ExecuteScalar()) + 1
                                            com.CommandText = "INSERT INTO [dbo].[MBarangD] ([NoID],[IDBarang],[IDSatuan],[Konversi],[Barcode],[IsDefault],[IsActive],[ProsenUpA],[HargaJualA],[ProsenUpB],[HargaJualB])" & vbCrLf & _
                                                              "SELECT " & IDBarangD & " [NoID],1 [IDBarang],[IDSatuan],1 [Konversi],MBarang.DefaultBarcode [Barcode],1 [IsDefault],[IsActive],[ProsenUpA],[HargaJualA],[ProsenUpB],[HargaJualB]" & vbCrLf & _
                                                              "FROM MBarang" & vbCrLf & _
                                                              "WHERE MBarang.NoID=" & Me.NoID
                                            com.ExecuteNonQuery()
                                            com.Transaction.Commit()

                                            pStatus = pStatusForm.Edit
                                            cmdBaru.Enabled = True
                                            cmdEdit.Enabled = True
                                            cmdHapus.Enabled = True

                                            RefreshDetil(IDBarangD)
                                        Else
                                            com.CommandText = "UPDATE [dbo].[MBarangD] SET IDSatuan=MBarang.IDSatuan, Konversi=1, ProsenUpA=MBarang.ProsenUpA, HargaJualA=MBarang.HargaJualA, ProsenUpB=MBarang.ProsenUpB, HargaJualB=MBarang.HargaJualB" & vbCrLf & _
                                                              "FROM MBarangD INNER JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang" & vbCrLf & _
                                                              "WHERE MBarangD.IsDefault=1 AND MBarang.NoID=" & Me.NoID
                                            com.ExecuteNonQuery()

                                            com.CommandText = "UPDATE [dbo].[MBarangD] SET ProsenUpA=ROUND(((MBarangD.HargaJualA-(MBarang.HargaBeliPcs*MBarangD.Konversi))/(MBarang.HargaBeliPcs*MBarangD.Konversi))*100, 2), ProsenUpB=ROUND(((MBarangD.HargaJualB-(MBarang.HargaBeliPcs*MBarangD.Konversi))/(MBarang.HargaBeliPcs*MBarangD.Konversi))*100, 2)" & vbCrLf & _
                                                              "FROM MBarangD INNER JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang" & vbCrLf & _
                                                              "WHERE ISNULL(MBarangD.IsDefault,0)=1 AND MBarang.NoID=" & Me.NoID
                                            com.ExecuteNonQuery()
                                            com.Transaction.Commit()

                                            DialogResult = Windows.Forms.DialogResult.OK
                                            Me.Close()
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

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Sub New(ByVal NoID As Long)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.NoID = NoID
    End Sub

    Private Sub frmEntriRole_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            cmdSimpan.ImageList = frmMain.ICButtons
            cmdSimpan.ImageIndex = 8
            cmdTutup.ImageList = frmMain.ICButtons
            cmdTutup.ImageIndex = 5
            cmdBaru.ImageList = frmMain.ICButtons
            cmdEdit.ImageList = frmMain.ICButtons
            cmdHapus.ImageList = frmMain.ICButtons
            cmdRefresh.ImageList = frmMain.ICButtons

            LoadData(NoID)
            With LayoutControl1
                If System.IO.File.Exists(Utils.SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml") Then
                    .RestoreLayoutFromXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml")
                End If
            End With
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Windows.Forms.Cursor.Current = curentcursor
    End Sub

    Public Sub LoadData(ByVal NoID As Long)
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
                                txtSatuanBeli.Properties.DataSource = ds.Tables("MSatuan")
                                txtSatuanBeli.Properties.ValueMember = "NoID"
                                txtSatuanBeli.Properties.DisplayMember = "Kode"
                                txtSatuanBeli.EditValue = -1

                                txtSatuanJual.Properties.DataSource = ds.Tables("MSatuan")
                                txtSatuanJual.Properties.ValueMember = "NoID"
                                txtSatuanJual.Properties.DisplayMember = "Kode"
                                txtSatuanJual.EditValue = -1

                                com.CommandText = "SELECT NoID, TypePajak FROM MTypePajak"
                                oDA.Fill(ds, "MtypePajak")
                                txtTypePajak.Properties.DataSource = ds.Tables("MtypePajak")
                                txtTypePajak.Properties.ValueMember = "NoID"
                                txtTypePajak.Properties.DisplayMember = "TypePajak"
                                txtTypePajak.EditValue = 0

                                com.CommandText = "SELECT NoID, Kode, Nama FROM MKategori WHERE IsActive=1 AND NoID NOT IN (SELECT IDParent FROM MKategori)"
                                oDA.Fill(ds, "MKategori")
                                txtKategori.Properties.DataSource = ds.Tables("MKategori")
                                txtKategori.Properties.ValueMember = "NoID"
                                txtKategori.Properties.DisplayMember = "Nama"
                                txtKategori.EditValue = -1

                                com.CommandText = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1 AND IsSupplier=1"
                                oDA.Fill(ds, "MSupplier")
                                txtSupplier1.Properties.DataSource = ds.Tables("MSupplier")
                                txtSupplier1.Properties.ValueMember = "NoID"
                                txtSupplier1.Properties.DisplayMember = "Nama"
                                txtSupplier1.EditValue = -1

                                txtSupplier2.Properties.DataSource = ds.Tables("MSupplier")
                                txtSupplier2.Properties.ValueMember = "NoID"
                                txtSupplier2.Properties.DisplayMember = "Nama"
                                txtSupplier2.EditValue = -1

                                txtSupplier3.Properties.DataSource = ds.Tables("MSupplier")
                                txtSupplier3.Properties.ValueMember = "NoID"
                                txtSupplier3.Properties.DisplayMember = "Nama"
                                txtSupplier3.EditValue = -1

                                com.CommandText = "SELECT * FROM MBarang WHERE NoID=" & NoID
                                oDA.Fill(ds, "MBarang")

                                If ds.Tables("MBarang").Rows.Count >= 1 Then
                                    pStatus = pStatusForm.Edit
                                    Me.NoID = NullToLong(ds.Tables("MBarang").Rows(0).Item("NoID"))
                                    txtKode.Text = NullToStr(ds.Tables("MBarang").Rows(0).Item("Kode"))
                                    txtNama.Text = NullToStr(ds.Tables("MBarang").Rows(0).Item("Nama"))
                                    txtAlias.Text = NullToStr(ds.Tables("MBarang").Rows(0).Item("Alias"))
                                    txtBarcode.Text = NullToStr(ds.Tables("MBarang").Rows(0).Item("DefaultBarcode"))
                                    txtKategori.EditValue = NullTolInt(ds.Tables("MBarang").Rows(0).Item("IDKategori"))
                                    txtKeterangan.EditValue = NullToStr(ds.Tables("MBarang").Rows(0).Item("Keterangan"))
                                    txtTypePajak.EditValue = NullTolInt(ds.Tables("MBarang").Rows(0).Item("IDTypePajak"))
                                    ckAktif.Checked = NullToBool(ds.Tables("MBarang").Rows(0).Item("IsActive"))
                                    txtSupplier1.EditValue = NullTolInt(ds.Tables("MBarang").Rows(0).Item("IDSupplier1"))
                                    txtSupplier2.EditValue = NullTolInt(ds.Tables("MBarang").Rows(0).Item("IDSupplier2"))
                                    txtSupplier3.EditValue = NullTolInt(ds.Tables("MBarang").Rows(0).Item("IDSupplier3"))

                                    txtSatuanBeli.EditValue = NullTolInt(ds.Tables("MBarang").Rows(0).Item("IDSatuanBeli"))
                                    txtSatuanJual.EditValue = NullTolInt(ds.Tables("MBarang").Rows(0).Item("IDSatuan"))

                                    txtHargaBeli.EditValue = NullToDbl(ds.Tables("MBarang").Rows(0).Item("HargaBeli"))
                                    txtIsiCtn.EditValue = NullTolInt(ds.Tables("MBarang").Rows(0).Item("IsiCtn"))

                                    txtDiscProsen1.EditValue = NullToDbl(ds.Tables("MBarang").Rows(0).Item("DiscProsen1"))
                                    txtDiscProsen2.EditValue = NullToDbl(ds.Tables("MBarang").Rows(0).Item("DiscProsen2"))
                                    txtDiscProsen3.EditValue = NullToDbl(ds.Tables("MBarang").Rows(0).Item("DiscProsen3"))
                                    txtDiscProsen4.EditValue = NullToDbl(ds.Tables("MBarang").Rows(0).Item("DiscProsen4"))
                                    txtDiscProsen5.EditValue = NullToDbl(ds.Tables("MBarang").Rows(0).Item("DiscProsen5"))
                                    txtDiscRp.EditValue = NullToDbl(ds.Tables("MBarang").Rows(0).Item("DiscRp"))

                                    txtModal.EditValue = Utils.Bulatkan(IIf(txtTypePajak.EditValue = 2, 1.1, 1) * Utils.Bulatkan(Utils.Bulatkan(txtHargaBeli.EditValue / txtIsiCtn.EditValue, 2) * (1 - txtDiscProsen1.EditValue / 100) * (1 - txtDiscProsen2.EditValue / 100) * (1 - txtDiscProsen3.EditValue / 100) * (1 - txtDiscProsen4.EditValue / 100) * (1 - txtDiscProsen5.EditValue / 100) - txtDiscRp.EditValue, 2), 2)
                                    txtHargaJualA.EditValue = Utils.Bulatkan(txtModal.EditValue * (1 + txtProsenUpA.EditValue / 100), 0)
                                    txtHargaJualB.EditValue = Utils.Bulatkan(txtModal.EditValue * (1 + txtProsenUpB.EditValue / 100), 0)

                                    txtProsenUpA.EditValue = NullTolInt(ds.Tables("MBarang").Rows(0).Item("ProsenUpA"))
                                    txtProsenUpB.EditValue = NullTolInt(ds.Tables("MBarang").Rows(0).Item("ProsenUpB"))
                                    txtHargaJualA.EditValue = NullTolInt(ds.Tables("MBarang").Rows(0).Item("HargaJualA"))
                                    txtHargaJualB.EditValue = NullTolInt(ds.Tables("MBarang").Rows(0).Item("HargaJualB"))
                                Else
                                    pStatus = pStatusForm.Baru
                                    txtIsiCtn.EditValue = 1
                                    txtDiscProsen1.EditValue = 0.0
                                    txtDiscProsen2.EditValue = 0.0
                                    txtDiscProsen3.EditValue = 0.0
                                    txtDiscProsen4.EditValue = 0.0
                                    txtDiscProsen5.EditValue = 0.0
                                    txtProsenUpA.EditValue = -100.0
                                    txtProsenUpB.EditValue = -100.0

                                    txtModal.EditValue = Utils.Bulatkan(IIf(txtTypePajak.EditValue = 2, 1.1, 1) * Utils.Bulatkan(Utils.Bulatkan(txtHargaBeli.EditValue / txtIsiCtn.EditValue, 2) * (1 - txtDiscProsen1.EditValue / 100) * (1 - txtDiscProsen2.EditValue / 100) * (1 - txtDiscProsen3.EditValue / 100) * (1 - txtDiscProsen4.EditValue / 100) * (1 - txtDiscProsen5.EditValue / 100) - txtDiscRp.EditValue, 2), 2)
                                    txtHargaJualA.EditValue = Utils.Bulatkan(txtModal.EditValue * (1 + txtProsenUpA.EditValue / 100), 0)
                                    txtHargaJualB.EditValue = Utils.Bulatkan(txtModal.EditValue * (1 + txtProsenUpB.EditValue / 100), 0)

                                    Me.NoID = -1

                                    cmdBaru.Enabled = False
                                    cmdEdit.Enabled = False
                                    cmdHapus.Enabled = False
                                End If
                                RefreshDetil(-1)
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
    Public Sub RefreshDetil(ByVal IDBarangD As Long)
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

                                com.CommandText = "SELECT MBarangD.*, MSatuan.Kode Satuan, MBarangD.ProsenUpA [MarginRetail], MBarangD.HargaJualA HargaRetail, MBarangD.ProsenUpB [MarginGrosir], MBarangD.HargaJualB HargaGrosir " & vbCrLf & _
                                                  "FROM MBarangD " & vbCrLf & _
                                                  "LEFT JOIN MSatuan ON MSatuan.NoID=MBarangD.IDSatuan " & vbCrLf & _
                                                  "WHERE MBarangD.IDBarang=" & NoID
                                If Not ckTampilkanSemua.Checked Then
                                    com.CommandText &= " AND MBarangD.IsActive=1"
                                End If
                                oDA.Fill(ds, "MBarangD")
                                GridControl1.DataSource = ds.Tables("MBarangD")

                                GridView1.ClearSelection()
                                GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), IDBarangD.ToString("n0"))
                                GridView1.SelectRow(GridView1.FocusedRowHandle)

                                For i As Integer = 0 To GridView1.RowCount - 1
                                    If NullToBool(GridView1.GetRowCellValue(i, "IsDefault")) Then
                                        GridView1.SetRowCellValue(i, "ProsenUpA", txtProsenUpA.EditValue)
                                        GridView1.SetRowCellValue(i, "MarginRetail", txtProsenUpA.EditValue)
                                        GridView1.SetRowCellValue(i, "ProsenUpB", txtProsenUpB.EditValue)
                                        GridView1.SetRowCellValue(i, "MarginGrosir", txtProsenUpB.EditValue)

                                        GridView1.SetRowCellValue(i, "HargaJualA", txtHargaJualA.EditValue)
                                        GridView1.SetRowCellValue(i, "HargaRetail", txtHargaJualA.EditValue)
                                        GridView1.SetRowCellValue(i, "HargaJualB", txtHargaJualB.EditValue)
                                        GridView1.SetRowCellValue(i, "HargaGrosir", txtHargaJualB.EditValue)
                                    Else
                                        If NullToDbl(txtModal.EditValue) = 0 Then
                                            GridView1.SetRowCellValue(i, "ProsenUpA", -100.0)
                                            GridView1.SetRowCellValue(i, "MarginRetail", -100.0)
                                            GridView1.SetRowCellValue(i, "ProsenUpB", -100.0)
                                            GridView1.SetRowCellValue(i, "MarginGrosir", -100.0)
                                        Else
                                            GridView1.SetRowCellValue(i, "ProsenUpA", Utils.Bulatkan(((NullToDbl(GridView1.GetRowCellValue(i, "HargaJualA")) - (txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi")))) / (txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi")))) * 100, 2))
                                            GridView1.SetRowCellValue(i, "MarginRetail", Utils.Bulatkan(((NullToDbl(GridView1.GetRowCellValue(i, "HargaRetail")) - (txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi")))) / (txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi")))) * 100, 2))
                                            GridView1.SetRowCellValue(i, "ProsenUpB", Utils.Bulatkan(((NullToDbl(GridView1.GetRowCellValue(i, "HargaJualB")) - (txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi")))) / (txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi")))) * 100, 2))
                                            GridView1.SetRowCellValue(i, "MarginGrosir", Utils.Bulatkan(((NullToDbl(GridView1.GetRowCellValue(i, "HargaGrosir")) - (txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi")))) / (txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi")))) * 100, 2))
                                        End If
                                    End If
                                Next

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
            If System.IO.File.Exists(Utils.SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml") Then
                .RestoreLayoutFromXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml")
            End If
        End With
    End Sub

    Private Sub mnSaveLayout_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayout.ItemClick
        Using frm As New frmOtorisasi
            Try
                If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    LayoutControl1.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & LayoutControl1.Name & ".xml")
                    gvKategori.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & gvKategori.Name & ".xml")
                    gvSatuanBeli.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & gvSatuanBeli.Name & ".xml")
                    gvSatuanJual.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & gvSatuanJual.Name & ".xml")
                    gvSupplier1.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & gvSupplier1.Name & ".xml")
                    gvSupplier2.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & gvSupplier2.Name & ".xml")
                    gvSupplier3.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & gvSupplier3.Name & ".xml")
                    gvTypePajak.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & gvTypePajak.Name & ".xml")
                    GridView1.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & GridView1.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub gv_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataSourceChanged, gvKategori.DataSourceChanged, gvSatuanBeli.DataSourceChanged, _
    gvSatuanJual.DataSourceChanged, gvSupplier1.DataSourceChanged, _
    gvSupplier1.DataSourceChanged, gvSupplier2.DataSourceChanged, gvTypePajak.DataSourceChanged
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

    Private Sub txtHargaBeli_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtHargaBeli.EditValueChanged

    End Sub

    Private Sub txtHargaBeli_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHargaBeli.LostFocus, txtIsiCtn.LostFocus, txtDiscProsen1.LostFocus, _
    txtDiscProsen2.LostFocus, txtDiscProsen3.LostFocus, txtDiscProsen4.LostFocus, txtDiscProsen5.LostFocus, _
    txtDiscRp.LostFocus, txtModal.LostFocus, txtTypePajak.LostFocus
        Try
            txtModal.EditValue = Utils.Bulatkan(IIf(txtTypePajak.EditValue = 2, 1.1, 1) * Utils.Bulatkan(Utils.Bulatkan(txtHargaBeli.EditValue / txtIsiCtn.EditValue, 2) * (1 - txtDiscProsen1.EditValue / 100) * (1 - txtDiscProsen2.EditValue / 100) * (1 - txtDiscProsen3.EditValue / 100) * (1 - txtDiscProsen4.EditValue / 100) * (1 - txtDiscProsen5.EditValue / 100) - txtDiscRp.EditValue, 2), 2)
            txtHargaJualA.EditValue = Utils.Bulatkan(txtModal.EditValue * (1 + txtProsenUpA.EditValue / 100), 0)
            txtHargaJualB.EditValue = Utils.Bulatkan(txtModal.EditValue * (1 + txtProsenUpB.EditValue / 100), 0)
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtProsenUpA_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtProsenUpA.LostFocus
        Try
            txtHargaJualA.EditValue = Utils.Bulatkan(txtModal.EditValue * (1 + txtProsenUpA.EditValue / 100), 0)
            For i As Integer = 0 To GridView1.RowCount - 1
                If NullToBool(GridView1.GetRowCellValue(i, "IsDefault")) Then
                    GridView1.SetRowCellValue(i, "ProsenUpA", txtProsenUpA.EditValue)
                    GridView1.SetRowCellValue(i, "MarginRetail", txtProsenUpA.EditValue)

                    GridView1.SetRowCellValue(i, "HargaJualA", txtHargaJualA.EditValue)
                    GridView1.SetRowCellValue(i, "HargaRetail", txtHargaJualA.EditValue)
                Else
                    If NullToDbl(txtModal.EditValue) = 0 Then
                        GridView1.SetRowCellValue(i, "ProsenUpA", -100.0)
                        GridView1.SetRowCellValue(i, "MarginRetail", -100.0)
                    Else
                        GridView1.SetRowCellValue(i, "ProsenUpA", Utils.Bulatkan((NullToDbl(GridView1.GetRowCellValue(i, "HargaJualA")) - txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi"))) / txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi")) * 100, 2))
                        GridView1.SetRowCellValue(i, "MarginRetail", Utils.Bulatkan((NullToDbl(GridView1.GetRowCellValue(i, "HargaRetail")) - txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi"))) / txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi")) * 100, 2))
                    End If
                End If
            Next
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtProsenUpB_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtProsenUpB.LostFocus
        Try
            txtHargaJualB.EditValue = Utils.Bulatkan(txtModal.EditValue * (1 + txtProsenUpB.EditValue / 100), 0)
            For i As Integer = 0 To GridView1.RowCount - 1
                If NullToBool(GridView1.GetRowCellValue(i, "IsDefault")) Then
                    GridView1.SetRowCellValue(i, "ProsenUpB", txtProsenUpB.EditValue)
                    GridView1.SetRowCellValue(i, "MarginGrosir", txtProsenUpB.EditValue)

                    GridView1.SetRowCellValue(i, "HargaJualB", txtHargaJualB.EditValue)
                    GridView1.SetRowCellValue(i, "HargaGrosir", txtHargaJualB.EditValue)
                Else
                    If NullToDbl(txtModal.EditValue) = 0 Then
                        GridView1.SetRowCellValue(i, "ProsenUpB", -100.0)
                        GridView1.SetRowCellValue(i, "MarginGrosir", -100.0)
                    Else
                        GridView1.SetRowCellValue(i, "ProsenUpB", Utils.Bulatkan((NullToDbl(GridView1.GetRowCellValue(i, "HargaJualB")) - txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi"))) / txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi")) * 100, 2))
                        GridView1.SetRowCellValue(i, "MarginGrosir", Utils.Bulatkan((NullToDbl(GridView1.GetRowCellValue(i, "HargaGrosir")) - txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi"))) / txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi")) * 100, 2))
                    End If
                End If
            Next
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtHargaJualA_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHargaJualA.EditValueChanged
        Try
            If NullToDbl(txtModal.EditValue) = 0 Then
                txtProsenUpA.EditValue = -100.0
            Else
                txtProsenUpA.EditValue = Utils.Bulatkan((txtHargaJualA.EditValue - txtModal.EditValue) / txtModal.EditValue * 100, 2)
            End If
            For i As Integer = 0 To GridView1.RowCount - 1
                If NullToBool(GridView1.GetRowCellValue(i, "IsDefault")) Then
                    GridView1.SetRowCellValue(i, "ProsenUpA", txtProsenUpA.EditValue)
                    GridView1.SetRowCellValue(i, "MarginRetail", txtProsenUpA.EditValue)

                    GridView1.SetRowCellValue(i, "HargaJualA", txtHargaJualA.EditValue)
                    GridView1.SetRowCellValue(i, "HargaRetail", txtHargaJualA.EditValue)
                Else
                    If NullToDbl(txtModal.EditValue) = 0 Then
                        GridView1.SetRowCellValue(i, "ProsenUpA", -100.0)
                        GridView1.SetRowCellValue(i, "MarginRetail", -100.0)
                    Else
                        GridView1.SetRowCellValue(i, "ProsenUpA", Utils.Bulatkan((NullToDbl(GridView1.GetRowCellValue(i, "HargaJualA")) - txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi"))) / txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi")) * 100, 2))
                        GridView1.SetRowCellValue(i, "MarginRetail", Utils.Bulatkan((NullToDbl(GridView1.GetRowCellValue(i, "HargaRetail")) - txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi"))) / txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi")) * 100, 2))
                    End If
                End If
            Next
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtHargaJualA_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHargaJualA.LostFocus
        Try
            If NullToDbl(txtModal.EditValue) = 0 Then
                txtProsenUpA.EditValue = -100.0
            Else
                txtProsenUpA.EditValue = Utils.Bulatkan((txtHargaJualA.EditValue - txtModal.EditValue) / txtModal.EditValue * 100, 2)
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtHargaJualB_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHargaJualB.EditValueChanged
        Try
            If NullToDbl(txtModal.EditValue) = 0 Then
                txtProsenUpB.EditValue = -100.0
            Else
                txtProsenUpB.EditValue = Utils.Bulatkan((txtHargaJualB.EditValue - txtModal.EditValue) / txtModal.EditValue * 100, 2)
            End If
            For i As Integer = 0 To GridView1.RowCount - 1
                If NullToBool(GridView1.GetRowCellValue(i, "IsDefault")) Then
                    GridView1.SetRowCellValue(i, "ProsenUpB", txtProsenUpB.EditValue)
                    GridView1.SetRowCellValue(i, "MarginGrosir", txtProsenUpB.EditValue)

                    GridView1.SetRowCellValue(i, "HargaJualB", txtHargaJualB.EditValue)
                    GridView1.SetRowCellValue(i, "HargaGrosir", txtHargaJualB.EditValue)
                Else
                    If NullToDbl(txtModal.EditValue) = 0 Then
                        GridView1.SetRowCellValue(i, "ProsenUpB", -100.0)
                        GridView1.SetRowCellValue(i, "MarginGrosir", -100.0)
                    Else
                        GridView1.SetRowCellValue(i, "ProsenUpB", Utils.Bulatkan((NullToDbl(GridView1.GetRowCellValue(i, "HargaJualB")) - txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi"))) / txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi")) * 100, 2))
                        GridView1.SetRowCellValue(i, "MarginGrosir", Utils.Bulatkan((NullToDbl(GridView1.GetRowCellValue(i, "HargaGrosir")) - txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi"))) / txtModal.EditValue * NullToDbl(GridView1.GetRowCellValue(i, "Konversi")) * 100, 2))
                    End If
                End If
            Next
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtHargaJualB_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHargaJualB.LostFocus
        Try
            If NullToDbl(txtModal.EditValue) = 0 Then
                txtProsenUpB.EditValue = -100.0
            Else
                txtProsenUpB.EditValue = Utils.Bulatkan((txtHargaJualB.EditValue - txtModal.EditValue) / txtModal.EditValue * 100, 2)
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        RefreshDetil(-1)
    End Sub

    Private Sub cmdHapus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHapus.Click
        Dim gridview As DevExpress.XtraGrid.Views.Grid.GridView = Nothing
        gridview = GridView1
        If gridview.RowCount >= 1 Then
            If XtraMessageBox.Show("Ingin menonaktifkan data barcode " & NullToStr(gridview.GetRowCellValue(gridview.FocusedRowHandle, "Barcode")) & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
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

                                com.CommandText = "UPDATE MBarangD SET IsActive=0 WHERE NoID=" & NoID
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
        If pStatus = pStatusForm.Edit Then
            Using frm As New frmEntriBarangD(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")), Me.NoID, txtModal.EditValue)
                If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    RefreshDetil(frm.NoID)
                End If
            End Using
        End If
    End Sub

    Private Sub cmdBaru_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBaru.Click
        If pStatus = pStatusForm.Edit Then
            Using frm As New frmEntriBarangD(-1, Me.NoID, txtModal.EditValue)
                If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    RefreshDetil(frm.NoID)
                End If
            End Using
        End If
    End Sub

    Private Sub txtKategori_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKategori.EditValueChanged
        If pStatus = pStatusForm.Baru AndAlso txtKategori.Text <> "" Then
            Dim Kategori As String = "", NoUrut As Integer
            Using dlg As New WaitDialogForm("Sedang mengambil data ...", NamaAplikasi)
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

                                    Kategori = NullToLong(txtKategori.EditValue).ToString("000")

                                    com.CommandText = "SELECT MAX(CAST(CASE WHEN ISNUMERIC(SUBSTRING(Kode, 4, 4))=1 THEN SUBSTRING(Kode, 4, 4) ELSE 0 END AS INT)) AS Kode FROM MBarang WHERE LEFT(Kode, 3)='" & Kategori & "'"
                                    NoUrut = NullToLong(com.ExecuteScalar()) + 1

                                    txtKode.Text = Kategori & NoUrut.ToString("0000")

                                    com.CommandText = "SELECT MAX(NoID) FROM MBarangD"
                                    txtBarcode.Text = "8" & (NullToLong(com.ExecuteScalar()) + 1).ToString("000000")
                                    txtBarcode.Text = Utils.EAN8_Checksum(txtBarcode.EditValue)
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

    Private Sub GridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DoubleClick
        cmdEdit.PerformClick()
    End Sub
End Class
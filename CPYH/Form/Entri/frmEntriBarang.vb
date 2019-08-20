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
        If txtSupplier1.Text = "" AndAlso txtSupplier2.Text = "" AndAlso txtSupplier3.Text = "" Then
            DxErrorProvider1.SetError(txtSupplier1, "Supplier harus diisi!")
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

                                    If pStatus = pStatusForm.Baru Then
                                        com.CommandText = "SELECT MAX(NoID) FROM MKategori"
                                        NoID = NullToLong(com.ExecuteScalar()) + 1

                                        com.CommandText = "INSERT INTO MKategori (NoID, Kode, Nama, IDParent, IsActive) VALUES (@NoID, @Kode, @Nama, @IDParent, @IsActive)"
                                    Else
                                        com.CommandText = "UPDATE MKategori SET Kode=@Kode, Nama=@Nama, IDParent=@IDParent, IsActive=@IsActive WHERE NoID=@NoID"
                                    End If
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NoID
                                    com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = txtKode.Text
                                    com.Parameters.Add(New SqlParameter("@Nama", SqlDbType.VarChar)).Value = txtNama.Text
                                    com.Parameters.Add(New SqlParameter("@IDParent", SqlDbType.Int)).Value = NullToLong(txtKategori.EditValue)
                                    com.Parameters.Add(New SqlParameter("@IsActive", SqlDbType.Bit)).Value = ckAktif.Checked
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()

                                    DialogResult = Windows.Forms.DialogResult.OK
                                    Me.Close()
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
                If System.IO.File.Exists(FolderLayouts & Me.Name & .Name & ".xml") Then
                    .RestoreLayoutFromXml(FolderLayouts & Me.Name & .Name & ".xml")
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
                                    txtSatuanJual.EditValue = NullTolInt(ds.Tables("MBarang").Rows(0).Item("IDSatuanJual"))
                                    
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
            If System.IO.File.Exists(FolderLayouts & Me.Name & .Name & ".xml") Then
                .RestoreLayoutFromXml(FolderLayouts & Me.Name & .Name & ".xml")
            End If
        End With
    End Sub

    Private Sub mnSaveLayout_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayout.ItemClick
        Using frm As New frmOtorisasi
            Try
                If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    LayoutControl1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                    gvKategori.SaveLayoutToXml(FolderLayouts & Me.Name & gvKategori.Name & ".xml")
                    gvSatuanBeli.SaveLayoutToXml(FolderLayouts & Me.Name & gvSatuanBeli.Name & ".xml")
                    gvSatuanJual.SaveLayoutToXml(FolderLayouts & Me.Name & gvSatuanJual.Name & ".xml")
                    gvSupplier1.SaveLayoutToXml(FolderLayouts & Me.Name & gvSupplier1.Name & ".xml")
                    gvSupplier2.SaveLayoutToXml(FolderLayouts & Me.Name & gvSupplier2.Name & ".xml")
                    gvSupplier3.SaveLayoutToXml(FolderLayouts & Me.Name & gvSupplier3.Name & ".xml")
                    gvTypePajak.SaveLayoutToXml(FolderLayouts & Me.Name & gvTypePajak.Name & ".xml")
                    GridView1.SaveLayoutToXml(FolderLayouts & Me.Name & GridView1.Name & ".xml")
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
            If System.IO.File.Exists(FolderLayouts & Me.Name & .Name & ".xml") Then
                .RestoreLayoutFromXml(FolderLayouts & Me.Name & .Name & ".xml")
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
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtProsenUpB_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtProsenUpB.LostFocus
        Try
            txtHargaJualB.EditValue = Utils.Bulatkan(txtModal.EditValue * (1 + txtProsenUpB.EditValue / 100), 0)
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
End Class
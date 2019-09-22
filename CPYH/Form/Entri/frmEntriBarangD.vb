Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports CtrlSoft.Utils
Imports DevExpress.XtraEditors.Repository

Public Class frmEntriBarangD
    Public NoID As Long = -1
    Private IDBarang As Long = -1
    Private pStatus As pStatusForm

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        DxErrorProvider1.ClearErrors()
        If txtBarcode.Text = "" Then
            DxErrorProvider1.SetError(txtBarcode, "Barcode harus diisi!")
        End If
        If txtSatuan.Text = "" Then
            DxErrorProvider1.SetError(txtSatuan, "Satuan harus diisi!")
        End If
        If txtKonversi.EditValue <= 0 Then
            DxErrorProvider1.SetError(txtKonversi, "Isi Ctn salah!")
        End If
        If txtHargaJualA.EditValue < txtModal.EditValue * txtKonversi.EditValue Then
            DxErrorProvider1.SetError(txtHargaJualA, "Harga Jual Retail dibawah modal!")
        End If
        If txtHargaJualB.EditValue <> 0 AndAlso txtHargaJualB.EditValue < txtModal.EditValue Then
            DxErrorProvider1.SetError(txtHargaJualB, "Harga Jual Grosir dibawah modal!")
        End If
        If txtKonversi.EditValue <> 1 AndAlso ckIsDefault.Checked Then
            DxErrorProvider1.SetError(txtKonversi, "Jika dijadikan default maka konversi harus 1!")
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

                                    com.CommandText = "SELECT COUNT(NoID) FROM MBarangD WHERE Barcode='" & FixApostropi(txtBarcode.Text) & "' AND NoID<>" & Me.NoID
                                    If NullToLong(com.ExecuteScalar()) >= 1 Then
                                        DxErrorProvider1.SetError(txtBarcode, "Barcode sudah dipakai!")
                                    End If

                                    If Not DxErrorProvider1.HasErrors Then
                                        com.CommandText = "UPDATE MBarang SET IDUser=@IDUser, TanggalUpdate=GETDATE() WHERE NoID=@IDBarang"
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@IDBarang", SqlDbType.BigInt)).Value = IDBarang
                                        com.Parameters.Add(New SqlParameter("@IDUser", SqlDbType.Int)).Value = Utils.UserLogin.NoID
                                        com.ExecuteNonQuery()

                                        If pStatus = pStatusForm.Baru Then
                                            com.CommandText = "SELECT MAX(NoID) FROM MBarangD"
                                            NoID = NullToLong(com.ExecuteScalar()) + 1

                                            com.CommandText = "INSERT INTO MBarangD (NoID, Barcode, IDBarang, IDSatuan, Konversi, ProsenUpA, ProsenUpB, HargaJualA, HargaJualB, IsActive, IsDefault) VALUES (@IDUser, GETDATE(), @NoID, @Barcode, @IDBarang, @IDSatuan, @Konversi, @ProsenUpA, @ProsenUpB, @HargaJualA, @HargaJualB, @IsActive, @IsDefault)"
                                        Else
                                            com.CommandText = "UPDATE MBarangD SET Barcode=@Barcode, IDBarang=@IDBarang, IDSatuan=@IDSatuan, Konversi=@Konversi, ProsenUpA=@ProsenUpA, ProsenUpB=@ProsenUpB, HargaJualA=@HargaJualA, HargaJualB=@HargaJualB, IsActive=@IsActive, IsDefault=@IsDefault WHERE NoID=@NoID"
                                        End If
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NoID
                                        com.Parameters.Add(New SqlParameter("@Barcode", SqlDbType.VarChar)).Value = txtBarcode.Text
                                        com.Parameters.Add(New SqlParameter("@IDBarang", SqlDbType.BigInt)).Value = IDBarang
                                        com.Parameters.Add(New SqlParameter("@IDSatuan", SqlDbType.Int)).Value = NullTolInt(txtSatuan.EditValue)
                                        com.Parameters.Add(New SqlParameter("@Konversi", SqlDbType.Int)).Value = NullToDbl(txtKonversi.EditValue)
                                        com.Parameters.Add(New SqlParameter("@ProsenUpA", SqlDbType.Float)).Value = Bulatkan(txtProsenUpA.EditValue, 2)
                                        com.Parameters.Add(New SqlParameter("@ProsenUpB", SqlDbType.Float)).Value = Bulatkan(txtProsenUpB.EditValue, 2)
                                        com.Parameters.Add(New SqlParameter("@HargaJualA", SqlDbType.Float)).Value = Bulatkan(txtHargaJualA.EditValue, 2)
                                        com.Parameters.Add(New SqlParameter("@HargaJualB", SqlDbType.Float)).Value = Bulatkan(txtHargaJualB.EditValue, 2)
                                        com.Parameters.Add(New SqlParameter("@IsActive", SqlDbType.Bit)).Value = ckAktif.Checked
                                        com.Parameters.Add(New SqlParameter("@IsDefault", SqlDbType.Bit)).Value = ckIsDefault.Checked
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

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Sub New(ByVal NoID As Long, ByVal IDBarang As Long, ByVal ModalPcs As Double)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.NoID = NoID
        Me.IDBarang = IDBarang
        Me.txtModal.EditValue = ModalPcs
    End Sub

    Private Sub frmEntriRole_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            SimpleButton1.ImageList = frmMain.ICButtons
            SimpleButton1.ImageIndex = 8
            SimpleButton2.ImageList = frmMain.ICButtons
            SimpleButton2.ImageIndex = 5

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
                                txtSatuan.Properties.DataSource = ds.Tables("MSatuan")
                                txtSatuan.Properties.DisplayMember = "Kode"
                                txtSatuan.Properties.ValueMember = "NoID"

                                com.CommandText = "SELECT * FROM MBarangD WHERE NoID=" & NoID
                                oDA.Fill(ds, "MBarangD")

                                If ds.Tables("MBarangD").Rows.Count >= 1 Then
                                    pStatus = pStatusForm.Edit
                                    Me.NoID = NullToLong(ds.Tables("MBarangD").Rows(0).Item("NoID"))
                                    txtBarcode.Text = NullToStr(ds.Tables("MBarangD").Rows(0).Item("Barcode"))
                                    txtSatuan.EditValue = NullToLong(ds.Tables("MBarangD").Rows(0).Item("IDSatuan"))
                                    txtKonversi.EditValue = NullToDbl(ds.Tables("MBarangD").Rows(0).Item("Konversi"))
                                    txtProsenUpA.EditValue = NullToDbl(ds.Tables("MBarangD").Rows(0).Item("ProsenUpA"))
                                    txtProsenUpB.EditValue = NullToDbl(ds.Tables("MBarangD").Rows(0).Item("ProsenUpB"))
                                    txtHargaJualA.EditValue = NullToDbl(ds.Tables("MBarangD").Rows(0).Item("HargaJualA"))
                                    txtHargaJualB.EditValue = NullToDbl(ds.Tables("MBarangD").Rows(0).Item("HargaJualB"))

                                    txtHargaJualA.EditValue = Utils.Bulatkan(txtModal.EditValue * txtKonversi.EditValue * (1 + txtProsenUpA.EditValue / 100), 0)
                                    txtHargaJualB.EditValue = Utils.Bulatkan(txtModal.EditValue * txtKonversi.EditValue * (1 + txtProsenUpB.EditValue / 100), 0)

                                    ckAktif.Checked = NullToBool(ds.Tables("MBarangD").Rows(0).Item("IsActive"))
                                    ckIsDefault.Checked = NullToBool(ds.Tables("MBarangD").Rows(0).Item("IsDefault"))
                                Else
                                    pStatus = pStatusForm.Baru
                                    Me.NoID = -1
                                    txtKonversi.EditValue = 1
                                    txtProsenUpA.EditValue = -100.0
                                    txtProsenUpB.EditValue = -100.0
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
                    gvSatuan.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & gvSatuan.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub txtIsiCtn_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKonversi.EditValueChanged
        ckIsDefault.Checked = IIf(txtKonversi.EditValue = 1, True, False)
    End Sub

    Private Sub txtIsiCtn_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtKonversi.LostFocus
        ckIsDefault.Checked = IIf(txtKonversi.EditValue = 1, True, False)
    End Sub

    Private Sub txtProsenUpA_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtProsenUpA.LostFocus
        Try
            txtHargaJualA.EditValue = Utils.Bulatkan(txtModal.EditValue * txtKonversi.EditValue * (1 + txtProsenUpA.EditValue / 100), 0)
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtProsenUpB_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtProsenUpB.LostFocus
        Try
            txtHargaJualB.EditValue = Utils.Bulatkan(txtModal.EditValue * txtKonversi.EditValue * (1 + txtProsenUpB.EditValue / 100), 0)
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtHargaJualA_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtHargaJualA.EditValueChanged
        Try
            If NullToDbl(txtModal.EditValue) = 0 Then
                txtProsenUpA.EditValue = -100.0
            Else
                txtProsenUpA.EditValue = Utils.Bulatkan((txtHargaJualA.EditValue - txtModal.EditValue * txtKonversi.EditValue) / txtModal.EditValue * txtKonversi.EditValue * 100, 2)
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub gvSatuan_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSatuan.DataSourceChanged
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

    Private Sub txtHargaJualA_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHargaJualA.LostFocus
        Try
            If NullToDbl(txtModal.EditValue) = 0 Then
                txtProsenUpA.EditValue = -100.0
            Else
                txtProsenUpA.EditValue = Utils.Bulatkan((txtHargaJualA.EditValue - txtModal.EditValue * txtKonversi.EditValue) / txtModal.EditValue * txtKonversi.EditValue * 100, 2)
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtHargaJualB_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtHargaJualB.EditValueChanged
        Try
            If NullToDbl(txtModal.EditValue) = 0 Then
                txtProsenUpB.EditValue = -100.0
            Else
                txtProsenUpB.EditValue = Utils.Bulatkan((txtHargaJualB.EditValue - txtModal.EditValue * txtKonversi.EditValue) / txtModal.EditValue * txtKonversi.EditValue * 100, 2)
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
                txtProsenUpB.EditValue = Utils.Bulatkan((txtHargaJualB.EditValue - txtModal.EditValue * txtKonversi.EditValue) / txtModal.EditValue * txtKonversi.EditValue * 100, 2)
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
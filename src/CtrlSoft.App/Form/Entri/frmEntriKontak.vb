Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.App.Public
Imports DevExpress.XtraEditors.Repository

Public Class frmEntriKontak
    Public NoID As Long = -1
    Private pStatus As pStatusForm

    Public Enum tipeKontak
        All = 0
        Customer = 1
        Supplier = 2
        Pegawai = 3
    End Enum
    Private tipe As tipeKontak

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        DxErrorProvider1.ClearErrors()
        If txtKode.Text = "" Then
            DxErrorProvider1.SetError(txtKode, "Kode harus diisi!")
        End If
        If txtNama.Text = "" Then
            DxErrorProvider1.SetError(txtNama, "Nama harus diisi!")
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

                                    com.CommandText = "SELECT COUNT(NoID) FROM MAlamat WHERE Kode=@Kode AND NoID<>@NoID"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = txtKode.Text
                                    com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NoID
                                    If NullToLong(com.ExecuteScalar()) >= 1 Then
                                        DxErrorProvider1.SetError(txtKode, "Kode sudah dipakai.", DXErrorProvider.ErrorType.Warning)
                                    End If

                                    If Not DxErrorProvider1.HasErrors Then
                                        com.Parameters.Clear()
                                        If pStatus = pStatusForm.Baru Then
                                            com.CommandText = "SELECT MAX(NoID) FROM MAlamat"
                                            NoID = NullToLong(com.ExecuteScalar()) + 1

                                            com.CommandText = "INSERT INTO [dbo].[MAlamat] ([NoID],[Kode],[Nama],[NamaAlias],[Alamat],[Kota],[HP],[Telp],[ContactPerson],[LimitHutang],[LimitPiutang],[LimitNotaPiutang],[LimitUmurPiutang],[IDTypeHarga],[IsActive],[IsSupplier],[IsPegawai],[IsCustomer]) " & vbCrLf & _
                                                              "VALUES (@NoID,@Kode,@Nama,@NamaAlias,@Alamat,@Kota,@HP,@Telp,@ContactPerson,@LimitHutang,@LimitPiutang,@LimitNotaPiutang,@LimitUmurPiutang,@IDTypeHarga,@IsActive,@IsSupplier,@IsPegawai,@IsCustomer)"
                                        Else
                                            com.CommandText = "UPDATE [dbo].[MAlamat] SET [Kode]=@Kode,[Nama]=@Nama,[NamaAlias]=@NamaAlias,[Alamat]=@Alamat,[Kota]=@Kota," & vbCrLf & _
                                                              "[HP]=@HP,[Telp]=@Telp,[ContactPerson]=@ContactPerson,[LimitHutang]=@LimitHutang,[LimitPiutang]=@LimitPiutang," & vbCrLf & _
                                                              "[LimitNotaPiutang]=@LimitNotaPiutang,[LimitUmurPiutang]=@LimitUmurPiutang,[IDTypeHarga]=@IDTypeHarga,[IsActive]=@IsActive," & vbCrLf & _
                                                              "[IsSupplier]=@IsSupplier,[IsPegawai]=@IsPegawai,[IsCustomer]=@IsCustomer" & vbCrLf & _
                                                              "WHERE NoID=@NoID"
                                        End If
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NoID
                                        com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = txtKode.Text
                                        com.Parameters.Add(New SqlParameter("@Nama", SqlDbType.VarChar)).Value = txtNama.Text
                                        com.Parameters.Add(New SqlParameter("@NamaAlias", SqlDbType.VarChar)).Value = txtNamaAlias.Text
                                        com.Parameters.Add(New SqlParameter("@Alamat", SqlDbType.VarChar)).Value = txtAlamat.Text
                                        com.Parameters.Add(New SqlParameter("@Kota", SqlDbType.VarChar)).Value = txtKota.Text
                                        com.Parameters.Add(New SqlParameter("@HP", SqlDbType.VarChar)).Value = txtHP.Text
                                        com.Parameters.Add(New SqlParameter("@Telp", SqlDbType.VarChar)).Value = txtTelp.Text
                                        com.Parameters.Add(New SqlParameter("@ContactPerson", SqlDbType.VarChar)).Value = txtKontakPerson.Text
                                        com.Parameters.Add(New SqlParameter("@LimitHutang", SqlDbType.Decimal)).Value = txtLimitHutang.EditValue
                                        com.Parameters.Add(New SqlParameter("@LimitPiutang", SqlDbType.Decimal)).Value = txtLimitPiutang.EditValue
                                        com.Parameters.Add(New SqlParameter("@LimitNotaPiutang", SqlDbType.Decimal)).Value = txtLimitNota.EditValue
                                        com.Parameters.Add(New SqlParameter("@LimitUmurPiutang", SqlDbType.Decimal)).Value = txtLimitUmur.EditValue
                                        com.Parameters.Add(New SqlParameter("@IDTypeHarga", SqlDbType.Int)).Value = NullToLong(txtTypeHarga.EditValue)
                                        com.Parameters.Add(New SqlParameter("@IsActive", SqlDbType.Bit)).Value = ckAktif.Checked
                                        com.Parameters.Add(New SqlParameter("@IsSupplier", SqlDbType.Bit)).Value = ckSupplier.Checked
                                        com.Parameters.Add(New SqlParameter("@IsPegawai", SqlDbType.Bit)).Value = ckPegawai.Checked
                                        com.Parameters.Add(New SqlParameter("@IsCustomer", SqlDbType.Bit)).Value = ckCustomer.Checked
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

    Public Sub New(ByVal tipe As tipeKontak, ByVal NoID As Long)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.NoID = NoID
        Me.tipe = tipe
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
                If System.IO.File.Exists([Public].SettingPerusahaan.PathLayouts & Me.Name & .Name & tipe.ToString & ".xml") Then
                    .RestoreLayoutFromXml([Public].SettingPerusahaan.PathLayouts & Me.Name & .Name & tipe.ToString & ".xml")
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

                                com.CommandText = "SELECT NoID, Caption FROM MTypeHarga"
                                oDA.Fill(ds, "MTypeHarga")
                                txtTypeHarga.Properties.DataSource = ds.Tables("MTypeHarga")
                                txtTypeHarga.Properties.ValueMember = "NoID"
                                txtTypeHarga.Properties.DisplayMember = "Caption"
                                txtTypeHarga.EditValue = -1

                                com.CommandText = "SELECT * FROM MAlamat WHERE NoID=" & NoID
                                oDA.Fill(ds, "MAlamat")

                                If ds.Tables("MAlamat").Rows.Count >= 1 Then
                                    pStatus = pStatusForm.Edit
                                    Me.NoID = NullToLong(ds.Tables("MAlamat").Rows(0).Item("NoID"))
                                    txtKode.Text = NullToStr(ds.Tables("MAlamat").Rows(0).Item("Kode"))
                                    txtNama.Text = NullToStr(ds.Tables("MAlamat").Rows(0).Item("Nama"))
                                    txtNamaAlias.Text = NullToStr(ds.Tables("MAlamat").Rows(0).Item("NamaAlias"))
                                    txtAlamat.Text = NullToStr(ds.Tables("MAlamat").Rows(0).Item("Alamat"))
                                    txtHP.Text = NullToStr(ds.Tables("MAlamat").Rows(0).Item("HP"))
                                    txtTelp.Text = NullToStr(ds.Tables("MAlamat").Rows(0).Item("Telp"))
                                    txtKontakPerson.Text = NullToStr(ds.Tables("MAlamat").Rows(0).Item("ContactPerson"))
                                    txtKota.Text = NullToStr(ds.Tables("MAlamat").Rows(0).Item("Kota"))
                                    txtLimitHutang.Text = NullToDbl(ds.Tables("MAlamat").Rows(0).Item("LimitHutang"))
                                    txtLimitPiutang.Text = NullToDbl(ds.Tables("MAlamat").Rows(0).Item("LimitPiutang"))
                                    txtLimitNota.Text = NullToDbl(ds.Tables("MAlamat").Rows(0).Item("LimitNotaPiutang"))
                                    txtLimitUmur.Text = NullToDbl(ds.Tables("MAlamat").Rows(0).Item("LimitUmurPiutang"))
                                    txtTypeHarga.EditValue = NullTolInt(ds.Tables("MAlamat").Rows(0).Item("IDTypeHarga"))
                                    ckAktif.Checked = NullToBool(ds.Tables("MAlamat").Rows(0).Item("IsActive"))
                                    ckCustomer.Checked = NullToBool(ds.Tables("MAlamat").Rows(0).Item("IsCustomer"))
                                    ckPegawai.Checked = NullToBool(ds.Tables("MAlamat").Rows(0).Item("IsPegawai"))
                                    ckSupplier.Checked = NullToBool(ds.Tables("MAlamat").Rows(0).Item("IsSupplier"))
                                Else
                                    pStatus = pStatusForm.Baru
                                    Me.NoID = -1

                                    ckCustomer.Checked = IIf(tipe = tipeKontak.Customer, True, False)
                                    ckPegawai.Checked = IIf(tipe = tipeKontak.Pegawai, True, False)
                                    ckSupplier.Checked = IIf(tipe = tipeKontak.Supplier, True, False)
                                    txtKode.Text = Repository.RepSQLServer.GetKodeKontak(tipe)
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
            If System.IO.File.Exists([Public].SettingPerusahaan.PathLayouts & Me.Name & .Name & tipe.ToString & ".xml") Then
                .RestoreLayoutFromXml([Public].SettingPerusahaan.PathLayouts & Me.Name & .Name & tipe.ToString & ".xml")
            End If
        End With
    End Sub

    Private Sub mnSaveLayout_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayout.ItemClick
        Using frm As New frmOtorisasi
            Try
                If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    LayoutControl1.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & LayoutControl1.Name & tipe.ToString & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub
End Class
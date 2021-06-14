Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.App.Public
Imports DevExpress.XtraEditors.Repository
Imports CtrlSoft.Dto.Model

Public Class frmEntriSaldoAwalHutang
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
    End Sub

    Private Sub frmEntriRole_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim curentcursor As Cursor = System.Windows.Forms.Cursor.Current
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
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
        System.Windows.Forms.Cursor.Current = curentcursor
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

                                Dim IDAlamatLama As Long = 0
                                If NoID >= 1 Then
                                    com.CommandText = "SELECT IDSupplier FROM MSaldoAwalHutang(NOLOCK) WHERE NoID=" & NoID
                                    IDAlamatLama = NullToLong(com.ExecuteScalar())
                                End If
                                com.CommandText = [Public].Dataset.SQLLookUpAlamat & IIf(NoID >= 1, " OR MAlamat.NoID=" & IDAlamatLama, "")
                                oDA.Fill(ds, "MAlamat")
                                txtKontak.Properties.DataSource = ds.Tables("MAlamat")
                                txtKontak.Properties.ValueMember = "NoID"
                                txtKontak.Properties.DisplayMember = "Kode"

                                com.CommandText = "SELECT * FROM MSaldoAwalHutang WHERE NoID=" & NoID
                                oDA.Fill(ds, "MSaldoAwalHutang")

                                If ds.Tables("MSaldoAwalHutang").Rows.Count >= 1 Then
                                    Dim iRow As DataRow = ds.Tables("MSaldoAwalHutang").Rows(0)
                                    If NullToBool(iRow.Item("IsPosted")) Then
                                        pStatus = pStatusForm.Posted
                                    Else
                                        pStatus = pStatusForm.Edit
                                    End If
                                    Me.NoID = NullToLong(iRow.Item("NoID"))
                                    txtKode.EditValue = NullToStr(iRow.Item("Kode"))
                                    txtKodeReff.EditValue = NullToStr(iRow.Item("KodeReff"))
                                    txtTanggal.EditValue = NullToDate(iRow.Item("Tanggal"))
                                    txtJatuhTempo.EditValue = NullToDate(iRow.Item("JatuhTempo"))
                                    txtJT.EditValue = DateDiff(DateInterval.Day, NullToDate(iRow.Item("Tanggal")), NullToDate(iRow.Item("JatuhTempo")))
                                    txtKontak.EditValue = NullToLong(iRow.Item("IDSupplier"))
                                    txtJumlah.EditValue = NullToDbl(iRow.Item("Jumlah"))
                                    txtCatatan.Text = NullToStr(iRow.Item("Keterangan"))
                                Else
                                    pStatus = pStatusForm.Baru
                                    Me.NoID = -1
                                    txtKode.EditValue = "AUTO"
                                    txtKodeReff.EditValue = ""
                                    txtTanggal.EditValue = [Public].UserLogin.TanggalSystem
                                    txtJatuhTempo.EditValue = [Public].UserLogin.TanggalSystem
                                    txtJT.EditValue = 0
                                    txtKontak.EditValue = -1
                                    txtJumlah.EditValue = 0.0
                                    txtCatatan.Text = ""
                                End If

                                txtKontak.Focus()
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
                    gvKontak.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & gvKontak.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub gv_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvKontak.DataSourceChanged
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

    Private Sub txtKontak_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKontak.EditValueChanged
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

                                com.CommandText = "SELECT * FROM MAlamat(NOLOCK) WHERE NoID=" & NullToLong(txtKontak.EditValue)
                                oDA.Fill(ds, "MAlamat")
                                If ds.Tables("MAlamat").Rows.Count >= 1 Then
                                    Dim iRow As DataRow = ds.Tables("MAlamat").Rows(0)
                                    txtNamaAlamat.EditValue = NullToStr(iRow.Item("Nama"))
                                    txtAlamat.EditValue = NullToStr(iRow.Item("Alamat"))
                                Else
                                    txtNamaAlamat.EditValue = ""
                                    txtAlamat.EditValue = ""
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
            DxErrorProvider1.SetError(txtTanggal, "Tanggal harus diisi!")
        End If
        If txtJatuhTempo.Text = "" Then
            DxErrorProvider1.SetError(txtJatuhTempo, "Jatuh Tempo harus diisi!")
        End If
        If NullToLong(txtJT.EditValue) < 0 Then
            DxErrorProvider1.SetError(txtJatuhTempo, "Jatuh Tempo yang anda masukkan salah!")
        End If
        If txtJumlah.EditValue = 0.0 Then
            DxErrorProvider1.SetError(txtJumlah, "Nilai Saldo Awal salah!")
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

                                    If Not DxErrorProvider1.HasErrors Then
                                        If pStatus = pStatusForm.Edit OrElse pStatus = pStatusForm.TempInsert Then
                                            com.CommandText = "SELECT IsPosted FROM MSaldoAwalHutang(NOLOCK) WHERE NoID>=1 AND NoID=" & NoID
                                            If NullToBool(com.ExecuteScalar()) Then
                                                DxErrorProvider1.SetError(txtKode, "Data telah diposting harus diisi!")
                                            End If
                                        End If
                                        If Not DxErrorProvider1.HasErrors Then

                                            If pStatus = pStatusForm.Edit OrElse pStatus = pStatusForm.TempInsert Then
                                                com.CommandText = "SELECT COUNT(Kode) FROM MSaldoAwalHutang WHERE Kode=@Kode AND NoID<>@NoID"
                                                com.Parameters.Clear()
                                                com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NoID
                                                com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = txtKode.Text
                                                If NullToLong(com.ExecuteScalar()) >= 1 Then
                                                    XtraMessageBox.Show("Nota telah sudah ada!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                                    Exit Try
                                                End If
                                            End If

                                            com.CommandText = "[dbo].[spSimpanMSaldoAwalHutang] " & vbCrLf &
                                                              "@NoID " & vbCrLf &
                                                              ",@Kode " & vbCrLf &
                                                              ",@KodeReff " & vbCrLf &
                                                              ",@Tanggal " & vbCrLf &
                                                              ",@JatuhTempo " & vbCrLf &
                                                              ",@Jumlah " & vbCrLf &
                                                              ",@Keterangan " & vbCrLf &
                                                              ",@IDSupplier " & vbCrLf &
                                                              ",@IsPosted " & vbCrLf &
                                                              ",@TglPosted " & vbCrLf &
                                                              ",@IDUserPosted " & vbCrLf &
                                                              ",@IDUserEntry " & vbCrLf &
                                                              ",@IDUserEdit"


                                            com.Parameters.Clear()
                                            com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NoID
                                            com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = txtKode.Text
                                            com.Parameters.Add(New SqlParameter("@KodeReff", SqlDbType.VarChar)).Value = txtKodeReff.Text
                                            com.Parameters.Add(New SqlParameter("@Tanggal", SqlDbType.Date)).Value = txtTanggal.EditValue
                                            com.Parameters.Add(New SqlParameter("@JatuhTempo", SqlDbType.Date)).Value = txtJatuhTempo.EditValue
                                            com.Parameters.Add(New SqlParameter("@Jumlah", SqlDbType.Money)).Value = Bulatkan(NullToDbl(txtJumlah.EditValue), 2)
                                            com.Parameters.Add(New SqlParameter("@Keterangan", SqlDbType.VarChar)).Value = txtCatatan.Text
                                            com.Parameters.Add(New SqlParameter("@IDSupplier", SqlDbType.Int)).Value = NullTolInt(txtKontak.EditValue)

                                            com.Parameters.Add(New SqlParameter("@IsPosted", SqlDbType.Bit)).Value = False
                                            com.Parameters.Add(New SqlParameter("@TglPosted", SqlDbType.DateTime)).Value = System.DBNull.Value
                                            com.Parameters.Add(New SqlParameter("@IDUserPosted", SqlDbType.Int)).Value = System.DBNull.Value
                                            com.Parameters.Add(New SqlParameter("@IDUserEntry", SqlDbType.Int)).Value = UserLogin.NoID
                                            com.Parameters.Add(New SqlParameter("@IDUserEdit", SqlDbType.Int)).Value = UserLogin.NoID
                                            com.ExecuteNonQuery()

                                            com.Transaction.Commit()

                                            If Tutup Then
                                                DialogResult = System.Windows.Forms.DialogResult.OK
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
        DialogResult = System.Windows.Forms.DialogResult.Cancel
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

    Private Sub txtTanggal_LostFocus(sender As Object, e As EventArgs) Handles txtTanggal.LostFocus
        txtJatuhTempo.EditValue = NullToDate(txtTanggal.EditValue).AddDays(NullToLong(txtJT.EditValue))
    End Sub

    Private Sub txtJT_LostFocus(sender As Object, e As EventArgs) Handles txtJT.LostFocus
        txtJatuhTempo.EditValue = NullToDate(txtTanggal.EditValue).AddDays(NullToLong(txtJT.EditValue))
    End Sub

    Private Sub txtJatuhTempo_LostFocus(sender As Object, e As EventArgs) Handles txtJatuhTempo.LostFocus
        txtJT.EditValue = DateDiff(DateInterval.Day, NullToDate(txtTanggal.EditValue), NullToDate(txtJatuhTempo.EditValue))
    End Sub
End Class
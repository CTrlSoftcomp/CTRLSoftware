Imports CtrlSoft.App.Utils
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors
Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.Utils

Public Class frmEntriInternal
    Private NoID As Long = -1
    Private pStatus As pStatusForm
    Private pForm As modMain.FormInternal

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Private Property NamaForm() As String
        Get
            Select Case pForm
                Case modMain.FormInternal.Pemakaian
                    Return "Pemakaian"
                Case modMain.FormInternal.Penyesuaian
                    Return "Penyesuaian"
                Case modMain.FormInternal.MutasiGudang
                    Return "MutasiGudang"
                Case Else
                    Return ""
            End Select
        End Get
        Set(ByVal value As String)

        End Set
    End Property

    Private Property TitleForm() As String
        Get
            Select Case pForm
                Case modMain.FormInternal.Pemakaian
                    Return "Entri Pemakaian Barang"
                Case modMain.FormInternal.Penyesuaian
                    Return "Entri Penyesuaian Barang"
                Case modMain.FormInternal.MutasiGudang
                    Return "Entri Mutasi Gudang"
                Case Else
                    Return ""
            End Select
        End Get
        Set(ByVal value As String)

        End Set
    End Property

    Private Sub GridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DoubleClick
        mnEdit.PerformClick()
    End Sub

    Public Sub New(ByVal pForm As modMain.FormInternal, ByVal NoID As Long)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.NoID = NoID
        Me.pForm = pForm
        Me.Name = "frmEntri" & pForm
        Me.Text = TitleForm
    End Sub

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

                            com.CommandText = "SELECT NoID, Kode, Nama FROM MGudang WHERE IsActive=1"
                            oDA.Fill(ds, "MGudang")
                            txtGudangAsal.Properties.DataSource = ds.Tables("MGudang")
                            txtGudangAsal.Properties.ValueMember = "NoID"
                            txtGudangAsal.Properties.DisplayMember = "Kode"

                            If pForm = modMain.FormInternal.MutasiGudang Then
                                txtGudangTujuan.Properties.DataSource = ds.Tables("MGudang")
                                txtGudangTujuan.Properties.ValueMember = "NoID"
                                txtGudangTujuan.Properties.DisplayMember = "Kode"
                            Else
                                txtGudangTujuan.Properties.DataSource = Nothing
                                txtGudangTujuan.Properties.ValueMember = "NoID"
                                txtGudangTujuan.Properties.DisplayMember = "Kode"
                            End If
                        Catch ex As Exception
                            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End Using
                End Using
            End Using
        End Using
    End Sub
    Private Sub LoadData()
        Using cn As New SqlConnection(StrKonSQL)
            Using com As New SqlCommand
                Using oDA As New SqlDataAdapter
                    Using ds As New DataSet
                        Try
                            cn.Open()
                            com.Connection = cn
                            com.CommandTimeout = cn.ConnectionTimeout
                            oDA.SelectCommand = com

                            com.CommandText = "SELECT TOP 1 * FROM M" & NamaForm & " WHERE NoID=" & NoID
                            oDA.Fill(ds, "M" & NamaForm & "")

                            If ds.Tables("M" & NamaForm & "").Rows.Count >= 1 Then
                                Dim iRow As DataRow = ds.Tables("M" & NamaForm & "").Rows(0)
                                If NullToBool(iRow.Item("IsPosted")) Then
                                    pStatus = pStatusForm.Posted
                                Else
                                    pStatus = pStatusForm.Edit
                                End If
                                If pForm = modMain.FormInternal.MutasiGudang Then
                                    txtGudangAsal.EditValue = NullToLong(iRow.Item("IDGudangAsal"))
                                    txtGudangTujuan.EditValue = NullToLong(iRow.Item("IDGudangTujuan"))
                                Else
                                    txtGudangAsal.EditValue = NullToLong(iRow.Item("IDGudang"))
                                    txtGudangTujuan.EditValue = -1
                                End If
                                txtTanggal.EditValue = NullToDate(iRow.Item("Tanggal"))
                                txtKode.Text = NullToStr(iRow.Item("Kode"))
                                txtNoReff.Text = NullToStr(iRow.Item("NoReff"))
                                txtCatatan.Text = NullToStr(iRow.Item("Catatan"))
                            Else
                                txtGudangAsal.EditValue = -1
                                txtGudangTujuan.EditValue = -1
                                txtTanggal.EditValue = UserLogin.TanggalSystem
                                txtNoReff.Text = ""
                                txtCatatan.Text = ""
                                pStatus = pStatusForm.Baru
                            End If
                        Catch ex As Exception
                            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End Using
                End Using
            End Using
        End Using
    End Sub
    Private Sub RefreshDetil(Optional ByVal IDDetil As Long = -1)
        Using dlg As New DevExpress.Utils.WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
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

                                com.CommandText = "SELECT M" & NamaForm & "D.*, MSatuan.Kode AS Satuan, MBarangD.Barcode, MBarang.Kode KodeBarang, MBarang.Nama NamaBarang" & vbCrLf & _
                                                  "FROM M" & NamaForm & "D" & vbCrLf & _
                                                  "LEFT JOIN MBarang ON MBarang.NoID=M" & NamaForm & "D.IDBarang" & vbCrLf & _
                                                  "LEFT JOIN MBarangD ON MBarangD.NoID=M" & NamaForm & "D.IDBarangD" & vbCrLf & _
                                                  "LEFT JOIN MSatuan ON MSatuan.NoID=M" & NamaForm & "D.IDSatuan" & vbCrLf & _
                                                  "WHERE M" & NamaForm & "D.IDHeader=" & NoID
                                oDA.Fill(ds, "M" & NamaForm & "D")
                                GridControl1.DataSource = ds.Tables("M" & NamaForm & "D")
                                GridView1.RefreshData()

                                GridView1.ClearSelection()
                                GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), IDDetil.ToString("n0"))
                                GridView1.SelectRow(GridView1.FocusedRowHandle)

                                If ds.Tables("M" & NamaForm & "D").Rows.Count >= 1 Then
                                    txtGudangAsal.Enabled = False
                                    txtTanggal.Enabled = False
                                Else
                                    txtGudangAsal.Enabled = True
                                    txtTanggal.Enabled = True
                                End If

                                If pStatus = pStatusForm.Posted Then
                                    mnBaru.Enabled = False
                                    mnEdit.Enabled = False
                                    mnHapus.Enabled = False
                                    mnSimpan.Enabled = False
                                Else
                                    mnBaru.Enabled = True
                                    mnEdit.Enabled = True
                                    mnHapus.Enabled = True
                                    mnSimpan.Enabled = True
                                End If

                                com.CommandText = "SELECT SUM(M" & NamaForm & "D.Jumlah) Jumlah" & vbCrLf & _
                                                  "FROM M" & NamaForm & "D INNER JOIN M" & NamaForm & " ON M" & NamaForm & ".NoID=M" & NamaForm & "D.IDHeader" & vbCrLf & _
                                                  "WHERE M" & NamaForm & ".NoID=" & NoID & vbCrLf & _
                                                  "GROUP BY M" & NamaForm & ".NoID"
                                oDA.Fill(ds, "MHitung")
                                If ds.Tables("MHitung").Rows.Count >= 1 Then
                                    txtTotal.EditValue = NullToDbl(ds.Tables("MHitung").Rows(0).Item("Jumlah"))
                                Else
                                    txtTotal.EditValue = 0
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
    Private Function SimpanData() As Boolean
        Dim Hasil As Boolean = False
        If IsValidasi() Then
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using ds As New DataSet
                            Try
                                cn.Open()
                                com.Connection = cn
                                com.CommandTimeout = cn.ConnectionTimeout
                                com.Transaction = cn.BeginTransaction

                                oDA.SelectCommand = com

                                If pStatus = pStatusForm.Baru Then
                                    NoID = -1
                                ElseIf pStatus = pStatusForm.Edit Then
                                    com.CommandText = "SELECT IsPosted FROM M" & NamaForm & " WHERE NoID=" & NoID
                                    If NullToBool(com.ExecuteScalar()) Then
                                        XtraMessageBox.Show("Nota telah diposting!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                        Exit Try
                                    End If
                                ElseIf pStatus = pStatusForm.Posted Then
                                    XtraMessageBox.Show("Nota telah diposting!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    Exit Try
                                End If

                                com.CommandText = "SELECT SUM(M" & NamaForm & "D.Qty*M" & NamaForm & "D.Konversi) AS Qty, M" & NamaForm & "D.IDBarang, MBarang.Kode, MBarang.Nama" & vbCrLf & _
                                                  "FROM M" & NamaForm & "D" & vbCrLf & _
                                                  "INNER JOIN M" & NamaForm & " ON M" & NamaForm & ".NoID=M" & NamaForm & "D.IDHeader" & vbCrLf & _
                                                  "LEFT JOIN MBarang ON MBarang.NoID=M" & NamaForm & "D.IDBarang" & vbCrLf & _
                                                  "WHERE M" & NamaForm & ".NoID=" & NoID & vbCrLf & _
                                                  "GROUP BY M" & NamaForm & "D.IDBarang, MBarang.Kode, MBarang.Nama"
                                If ds.Tables("M" & NamaForm & "D") IsNot Nothing Then
                                    ds.Tables("M" & NamaForm & "D").Clear()
                                    ds.Tables("M" & NamaForm & "D").Columns.Clear()
                                End If
                                oDA.Fill(ds, "M" & NamaForm & "D")

                                If pForm = modMain.FormInternal.Penyesuaian Then
                                Else
                                    For Each iRow As DataRow In ds.Tables("M" & NamaForm & "D").Rows
                                        com.CommandText = "EXEC spCekSaldoStok " & NullToLong(iRow.Item("IDBarang")) & ", " & NullToLong(txtGudangAsal.EditValue) & ", '" & txtTanggal.DateTime.ToString("yyyy-MM-dd HH:mm:ss") & "'"
                                        If NullToDbl(com.ExecuteScalar()) < NullToDbl(iRow.Item("Qty")) Then
                                            DxErrorProvider1.SetError(txtKode, "Saldo Stok Tidak Cukup!")
                                        End If
                                    Next
                                End If
                                
                                com.CommandText = "SELECT COUNT(Kode) FROM M" & NamaForm & " WHERE Kode=@Kode AND NoID<>@NoID"
                                com.Parameters.Clear()
                                com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NoID
                                com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = txtKode.Text
                                If NullToLong(com.ExecuteScalar()) >= 1 Then
                                    XtraMessageBox.Show("Nota telah sudah ada!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    Exit Try
                                End If

                                If Not DxErrorProvider1.HasErrors Then
                                    If pForm = modMain.FormInternal.MutasiGudang Then
                                        com.CommandText = "EXEC [dbo].[spSimpanM" & NamaForm & "] @NoID,@Kode,@NoReff,@Tanggal,@Catatan,@Total,@IsPosted,@TglPosted,@IDUserPosted,@IDUserEntry,@IDUserEdit,@IDGudangAsal,@IDGudangTujuan"
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NoID
                                        com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = txtKode.Text
                                        com.Parameters.Add(New SqlParameter("@NoReff", SqlDbType.VarChar)).Value = txtNoReff.Text
                                        com.Parameters.Add(New SqlParameter("@Tanggal", SqlDbType.DateTime)).Value = NullToDate(txtTanggal.EditValue)
                                        com.Parameters.Add(New SqlParameter("@Catatan", SqlDbType.VarChar)).Value = txtCatatan.Text
                                        com.Parameters.Add(New SqlParameter("@Total", SqlDbType.Money)).Value = NullToDbl(txtTotal.EditValue)
                                        com.Parameters.Add(New SqlParameter("@IsPosted", SqlDbType.Bit)).Value = False
                                        com.Parameters.Add(New SqlParameter("@TglPosted", SqlDbType.DateTime)).Value = System.DBNull.Value
                                        com.Parameters.Add(New SqlParameter("@IDUserPosted", SqlDbType.Int)).Value = System.DBNull.Value
                                        com.Parameters.Add(New SqlParameter("@IDUserEntry", SqlDbType.Int)).Value = UserLogin.NoID
                                        com.Parameters.Add(New SqlParameter("@IDUserEdit", SqlDbType.Int)).Value = IIf(pStatus = pStatusForm.Edit, UserLogin.NoID, -1)
                                        com.Parameters.Add(New SqlParameter("@IDGudangAsal", SqlDbType.Int)).Value = NullToLong(txtGudangAsal.EditValue)
                                        com.Parameters.Add(New SqlParameter("@IDGudangTujuan", SqlDbType.Int)).Value = NullToLong(txtGudangTujuan.EditValue)
                                    Else
                                        com.CommandText = "EXEC [dbo].[spSimpanM" & NamaForm & "] @NoID,@Kode,@NoReff,@Tanggal,@Catatan,@Total,@IsPosted,@TglPosted,@IDUserPosted,@IDUserEntry,@IDUserEdit,@IDGudangAsal"
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NoID
                                        com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = txtKode.Text
                                        com.Parameters.Add(New SqlParameter("@NoReff", SqlDbType.VarChar)).Value = txtNoReff.Text
                                        com.Parameters.Add(New SqlParameter("@Tanggal", SqlDbType.DateTime)).Value = NullToDate(txtTanggal.EditValue)
                                        com.Parameters.Add(New SqlParameter("@Catatan", SqlDbType.VarChar)).Value = txtCatatan.Text
                                        com.Parameters.Add(New SqlParameter("@Total", SqlDbType.Money)).Value = NullToDbl(txtTotal.EditValue)
                                        com.Parameters.Add(New SqlParameter("@IsPosted", SqlDbType.Bit)).Value = False
                                        com.Parameters.Add(New SqlParameter("@TglPosted", SqlDbType.DateTime)).Value = System.DBNull.Value
                                        com.Parameters.Add(New SqlParameter("@IDUserPosted", SqlDbType.Int)).Value = System.DBNull.Value
                                        com.Parameters.Add(New SqlParameter("@IDUserEntry", SqlDbType.Int)).Value = UserLogin.NoID
                                        com.Parameters.Add(New SqlParameter("@IDUserEdit", SqlDbType.Int)).Value = IIf(pStatus = pStatusForm.Edit, UserLogin.NoID, -1)
                                        com.Parameters.Add(New SqlParameter("@IDGudangAsal", SqlDbType.Int)).Value = NullToLong(txtGudangAsal.EditValue)
                                    End If
                                    
                                    NoID = NullToLong(com.ExecuteScalar())
                                    com.Parameters.Clear()

                                    com.CommandText = "UPDATE M" & NamaForm & " SET Total=ISNULL(M" & NamaForm & "D.Jumlah, 0)" & vbCrLf & _
                                                      "FROM M" & NamaForm & " " & vbCrLf & _
                                                      "INNER JOIN (SELECT IDHeader, SUM(Jumlah) AS Jumlah FROM M" & NamaForm & "D GROUP BY IDHeader) AS M" & NamaForm & "D ON M" & NamaForm & "D.IDHeader=M" & NamaForm & ".NoID " & vbCrLf & _
                                                      "WHERE M" & NamaForm & ".NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    If com.Transaction IsNot Nothing Then
                                        com.Transaction.Commit()
                                    End If
                                    If NoID >= 1 Then
                                        If pStatus = pStatusForm.Baru Then
                                            LoadData()
                                        End If
                                        Hasil = True
                                    End If
                                End If
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
        End If

        Return Hasil
    End Function

    Private Function CekDetil() As Boolean
        Dim Hasil As Boolean = False
        Using cn As New SqlConnection(StrKonSQL)
            Using com As New SqlCommand
                Using oDA As New SqlDataAdapter
                    Using ds As New DataSet
                        Try
                            cn.Open()
                            com.Connection = cn
                            com.CommandTimeout = cn.ConnectionTimeout

                            oDA.SelectCommand = com

                            com.CommandText = "SELECT IsPosted FROM M" & NamaForm & "(NOLOCK) WHERE NoID=" & NoID
                            If NullToBool(com.ExecuteScalar()) Then
                                XtraMessageBox.Show("Nota telah diposting!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                Exit Try
                            End If

                            com.CommandText = "SELECT COUNT(NoID) FROM M" & NamaForm & "D(NOLOCK) WHERE IDHeader=" & NoID
                            Hasil = IIf(NullToLong(com.ExecuteScalar()) >= 1, True, False)
                        Catch ex As Exception
                            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End Using
                End Using
            End Using
        End Using

        Return Hasil
    End Function

    Private Function HapusDetil(ByVal IDDetil As Long) As Boolean
        Dim Hasil As Boolean = False
        Using cn As New SqlConnection(StrKonSQL)
            Using com As New SqlCommand
                Using oDA As New SqlDataAdapter
                    Using ds As New DataSet
                        Try
                            cn.Open()
                            com.Connection = cn
                            com.CommandTimeout = cn.ConnectionTimeout
                            com.Transaction = cn.BeginTransaction

                            oDA.SelectCommand = com

                            com.CommandText = "SELECT IsPosted FROM M" & NamaForm & " WHERE NoID=" & NoID
                            If NullToBool(com.ExecuteScalar()) Then
                                XtraMessageBox.Show("Nota telah diposting!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                Exit Try
                            End If

                            com.CommandText = "DELETE FROM M" & NamaForm & "D WHERE NoID=" & IDDetil
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE M" & NamaForm & " SET Total=ISNULL(M" & NamaForm & "D.Jumlah, 0)" & vbCrLf & _
                                              "FROM M" & NamaForm & " " & vbCrLf & _
                                              "INNER JOIN (SELECT IDHeader, SUM(Jumlah) AS Jumlah FROM M" & NamaForm & "D GROUP BY IDHeader) AS M" & NamaForm & "D ON M" & NamaForm & "D.IDHeader=M" & NamaForm & ".NoID " & vbCrLf & _
                                              "WHERE M" & NamaForm & ".NoID=" & NoID
                            com.ExecuteNonQuery()

                            If com.Transaction IsNot Nothing Then
                                com.Transaction.Commit()
                            End If
                            Hasil = True
                        Catch ex As Exception
                            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End Using
                End Using
            End Using
        End Using

        Return Hasil
    End Function
#End Region

    Private Sub frmEntriPO_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Using dlg As New DevExpress.Utils.WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
            Try
                dlg.Show()
                dlg.Focus()
                InitLoadLookUp()
                LoadData()
                RefreshDetil()
                If System.IO.File.Exists(Utils.SettingPerusahaan.PathLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
                    LayoutControl1.RestoreLayoutFromXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & LayoutControl1.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub txtSupplier_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGudangAsal.EditValueChanged, txtGudangTujuan.EditValueChanged

    End Sub

    Private Sub txtTanggal_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTanggal.EditValueChanged
        Try
            If pStatus = pStatusForm.Baru Then
                txtKode.Text = "AUTO" 'Repository.RepKode.GetNewKode("M"& NamaForm &"", "Kode", "PO-" & NullToDate(txtTanggal.EditValue).ToString("yyMM") & "-", "", Repository.RepKode.Format.A00000)
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub gv_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataSourceChanged, _
    gvGudangAsal.DataSourceChanged, gvGudangTujuan.DataSourceChanged
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

    Private Sub mnRefresh_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        RefreshDetil()
    End Sub

    Private Sub mnTutup_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnTutup.ItemClick
        Me.Close()
        DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub mnHapus_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHapus.ItemClick
        If (pStatus = pStatusForm.Edit OrElse pStatus = pStatusForm.TempInsert) AndAlso GridView1.RowCount >= 1 Then
            If HapusDetil(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))) Then
                RefreshDetil()
            End If
        End If
    End Sub

    Private Sub mnEdit_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnEdit.ItemClick
        If (pStatus = pStatusForm.Edit OrElse pStatus = pStatusForm.TempInsert) AndAlso GridView1.RowCount >= 1 Then
            Using frm As New frmEntriInternalD(Me.pForm, Me, NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")), NoID)
                Try
                    If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        RefreshDetil(frm.NoID)
                    End If
                Catch ex As Exception
                    XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End If
    End Sub

    Private Sub mnBaru_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnBaru.ItemClick
        If pStatusChange() Then
            Using frm As New frmEntriInternalD(Me.pForm, Me, -1, NoID)
                Try
                    If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        RefreshDetil(frm.NoID)
                    End If
                Catch ex As Exception
                    XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End If
    End Sub

    Private Function pStatusChange() As Boolean
        If pStatus = pStatusForm.Baru Then
            Return SimpanData()
        Else
            Return True
        End If
    End Function

    Private Sub mnSaveLayouts_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayouts.ItemClick
        Using frm As New frmOtorisasi
            Try
                If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    LayoutControl1.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & LayoutControl1.Name & ".xml")
                    gvGudangAsal.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & gvGudangAsal.Name & ".xml")
                    gvGudangTujuan.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & gvGudangTujuan.Name & ".xml")
                    GridView1.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & GridView1.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub mnSimpan_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSimpan.ItemClick
        If SimpanData() Then
            If pStatus = pStatusForm.Baru Then
                pStatus = pStatusForm.Edit
                LoadData()
            ElseIf pStatus = pStatusForm.Edit OrElse pStatus = pStatusForm.TempInsert Then
                Dim x As frmDaftarTransaksi = Nothing
                For Each frm In Me.MdiParent.MdiChildren
                    If TypeOf frm Is frmDaftarTransaksi AndAlso _
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = IIf(pForm = modMain.FormInternal.MutasiGudang, modMain.FormName.DaftarMutasiGudang.ToString, _
                                                                         IIf(pForm = modMain.FormInternal.Pemakaian, modMain.FormName.DaftarPemakaian, modMain.FormName.DaftarPenyesuaian)).ToString Then
                        x = frm
                    End If
                Next
                If x IsNot Nothing Then
                    x.RefreshData(NoID)
                    x.Show()
                    x.Focus()
                End If
                DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub

    Private Function IsValidasi() As Boolean
        DxErrorProvider1.ClearErrors()
        If txtGudangAsal.Text = "" Then
            DxErrorProvider1.SetError(txtGudangAsal, "Supplier harus dipilih!", DXErrorProvider.ErrorType.Critical)
        End If
        If txtGudangAsal.Text = "" Then
            DxErrorProvider1.SetError(txtGudangAsal, "Gudang Asal harus dipilih!", DXErrorProvider.ErrorType.Critical)
        End If
        If pForm = modMain.FormInternal.MutasiGudang Then
            If txtGudangTujuan.Text = "" Then
                DxErrorProvider1.SetError(txtGudangTujuan, "Gudang Tujuan harus dipilih!", DXErrorProvider.ErrorType.Critical)
            End If
            If txtGudangAsal.EditValue = txtGudangTujuan.EditValue Then
                DxErrorProvider1.SetError(txtGudangTujuan, "Gudang Asal dan Tujuan harus beda!", DXErrorProvider.ErrorType.Critical)
            End If
        End If
        'If txtTotal.EditValue < 0 Then
        '    DxErrorProvider1.SetError(txtTotal, "Total PeM" & NamaForm & "an salah!", DXErrorProvider.ErrorType.Critical)
        'End If
        If Not pStatus = pStatusForm.Baru AndAlso Not CekDetil() Then
            DxErrorProvider1.SetError(txtKode, "Item masih kosong!", DXErrorProvider.ErrorType.Critical)
        End If

        Return Not DxErrorProvider1.HasErrors
    End Function
End Class
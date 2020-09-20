Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.App.Public
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors
Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports CtrlSoft.App.CetakDX

Public Class frmEntriStockOpname
    Private NoID As Long = -1
    Private pStatus As pStatusForm

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit
    Dim repCalcEdit As New RepositoryItemCalcEdit

    Private dsEntri As New DataSet
    Private bsEntri As New BindingSource

    Private Property NamaForm() As String
        Get
            Return "StockOpname"
        End Get
        Set(ByVal value As String)

        End Set
    End Property

    Private Property TitleForm() As String
        Get
            Return "Entri Stock Opname"
        End Get
        Set(ByVal value As String)

        End Set
    End Property

    Public Sub New(ByVal NoID As Long)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.NoID = NoID
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

                            txtKategori.Properties.DataSource = Repository.RepSQLServer.GetListKategori
                            txtKategori.Properties.DisplayMember = "Nama"
                            txtKategori.Properties.ValueMember = "NoID"
                            txtKategori.EditValue = -1

                            txtSupplier.Properties.DataSource = Repository.RepSQLServer.GetListSupplier
                            txtSupplier.Properties.DisplayMember = "Nama"
                            txtSupplier.Properties.ValueMember = "NoID"
                            txtSupplier.EditValue = -1

                            txtMerk.Properties.DataSource = Repository.RepSQLServer.GetListMerk
                            txtMerk.Properties.DisplayMember = "Nama"
                            txtMerk.Properties.ValueMember = "NoID"
                            txtMerk.EditValue = -1
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
                                txtGudangAsal.EditValue = NullToLong(iRow.Item("IDGudang"))
                                txtTanggal.EditValue = NullToDate(iRow.Item("Tanggal"))
                                txtKode.Text = NullToStr(iRow.Item("Kode"))
                                txtNoReff.Text = NullToStr(iRow.Item("NoReff"))
                                txtCatatan.Text = NullToStr(iRow.Item("Catatan"))
                            Else
                                txtGudangAsal.EditValue = -1
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
        Dim IsGenerateUlang As Boolean = True
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

                                If RowHasChanged AndAlso _
                                XtraMessageBox.Show("Ingin menggenerate ulang data yang telah diinput?" & vbCrLf & _
                                                    "Entrian Qty Fisik akan dinolkan.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2) = _
                                                    Windows.Forms.DialogResult.No Then
                                    IsGenerateUlang = False
                                End If

                                If IsGenerateUlang Then
                                    com.CommandText = "DECLARE @IDGudang AS BIGINT= " & NullToLong(txtGudangAsal.EditValue) & ";" & vbCrLf & _
                                                  "DECLARE @Tanggal AS DATETIME= '" & txtTanggal.DateTime.ToString("yyyy-MM-dd HH:mm:ss") & "';" & vbCrLf & _
                                                  "DECLARE @IDHeader AS BIGINT= " & NoID & ";" & vbCrLf & _
                                                  "SELECT MBarang.NoID, " & vbCrLf & _
                                                  "MBarang.DefaultBarcode AS Barcode, " & vbCrLf & _
                                                  "MBarang.Kode AS KodeBarang, " & vbCrLf & _
                                                  "MBarang.Nama NamaBarang, " & vbCrLf & _
                                                  "MBarang.HargaBeliPcs," & vbCrLf & _
                                                  "MKategori.Kode + '-' + MKategori.Nama AS Kategori," & vbCrLf & _
                                                  "MMerk.Kode + '-' + MMerk.Nama AS [Merk]," & vbCrLf & _
                                                  "MSupplier1.Kode + '-' + MSupplier1.Nama AS Supplier1," & vbCrLf & _
                                                  "MSupplier2.Kode + '-' + MSupplier2.Nama AS Supplier2," & vbCrLf & _
                                                  "MSupplier3.Kode + '-' + MSupplier3.Nama AS Supplier3," & vbCrLf & _
                                                  "ISNULL(MSaldo.Saldo, 0) AS QtyKomputer, " & vbCrLf & _
                                                  "ISNULL(MStockOpnameD.QtyFisik, ISNULL(MSaldo.Saldo, 0)) AS QtyFisik, " & vbCrLf & _
                                                  "ISNULL(MSaldo.Saldo, 0) - ISNULL(MStockOpnameD.QtyFisik, ISNULL(MSaldo.Saldo, 0)) AS QtySelisih," & vbCrLf & _
                                                  "MBarang.HargaBeliPcs*(ISNULL(MSaldo.Saldo, 0) - ISNULL(MStockOpnameD.QtyFisik, ISNULL(MSaldo.Saldo, 0))) AS Jumlah" & vbCrLf & _
                                                  "FROM MBarang(NOLOCK)" & vbCrLf & _
                                                  "LEFT JOIN MKategori(NOLOCK) ON MKategori.NoID=MBarang.IDKategori" & vbCrLf & _
                                                  "LEFT JOIN MMerk(NOLOCK) ON MMerk.NoID=MBarang.IDMerk" & vbCrLf & _
                                                  "LEFT JOIN MAlamat(NOLOCK) MSupplier1 ON MSupplier1.NoID=MBarang.IDSupplier1" & vbCrLf & _
                                                  "LEFT JOIN MAlamat(NOLOCK) MSupplier2 ON MSupplier2.NoID=MBarang.IDSupplier2" & vbCrLf & _
                                                  "LEFT JOIN MAlamat(NOLOCK) MSupplier3 ON MSupplier3.NoID=MBarang.IDSupplier3" & vbCrLf & _
                                                  "LEFT JOIN" & vbCrLf & _
                                                  "(" & vbCrLf & _
                                                  "SELECT MKartuStok.IDBarang, " & vbCrLf & _
                                                  "SUM(MKartuStok.Konversi * (MKartuStok.QtyMasuk - MKartuStok.QtyKeluar)) AS Saldo" & vbCrLf & _
                                                  "FROM MKartuStok(NOLOCK)" & vbCrLf & _
                                                  "WHERE MKartuStok.IDGudang = @IDGudang" & vbCrLf & _
                                                  "AND MKartuStok.Tanggal <= @Tanggal" & vbCrLf & _
                                                  "GROUP BY MKartuStok.IDBarang" & vbCrLf & _
                                                  ") AS MSaldo ON MSaldo.IDBarang = MBarang.NoID" & vbCrLf & _
                                                  "LEFT JOIN" & vbCrLf & _
                                                  "(" & vbCrLf & _
                                                  "SELECT MStockOpnameD.IDBarang, " & vbCrLf & _
                                                  "SUM(MStockOpnameD.QtyFisik) AS QtyFisik" & vbCrLf & _
                                                  "FROM MStockOpnameD(NOLOCK)" & vbCrLf & _
                                                  "WHERE MStockOpnameD.IDHeader = @IDHeader" & vbCrLf & _
                                                  "GROUP BY MStockOpnameD.IDBarang" & vbCrLf & _
                                                  ") AS MStockOpnameD ON MStockOpnameD.IDBarang = MBarang.NoID" & vbCrLf & _
                                                  "WHERE (MBarang.IsActive=1 OR ISNULL(MSaldo.Saldo, 0)<>0 OR ISNULL(MStockOpnameD.QtyFisik, 0)<>0)"
                                    If NoID <= 0 Then
                                        com.CommandText &= vbCrLf & " AND MBarang.NoID=-100"
                                    End If
                                    If NullToLong(txtKategori.EditValue) >= 1 Then
                                        com.CommandText &= vbCrLf & " AND MBarang.IDKategori=" & NullToLong(txtKategori.EditValue)
                                    End If
                                    If NullToLong(txtSupplier.EditValue) >= 1 Then
                                        com.CommandText &= vbCrLf & " AND (MBarang.IDSupplier1=" & NullToLong(txtKategori.EditValue) & " OR MBarang.IDSupplier2=" & NullToLong(txtKategori.EditValue) & " OR MBarang.IDSupplier3=" & NullToLong(txtKategori.EditValue) & ")"
                                    End If
                                    If NullToLong(txtMerk.EditValue) >= 1 Then
                                        com.CommandText &= vbCrLf & " AND MBarang.IDMerk=" & NullToLong(txtMerk.EditValue)
                                    End If

                                    dsEntri.Dispose()
                                    dsEntri = New DataSet

                                    oDA.Fill(dsEntri, "Detil")
                                    bsEntri.Dispose()
                                    bsEntri = New BindingSource
                                    bsEntri.DataSource = dsEntri.Tables("Detil")

                                    GridControl1.DataSource = bsEntri
                                    GridView1.RefreshData()

                                    GridView1.ClearSelection()
                                    GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), IDDetil.ToString("n0"))
                                    GridView1.SelectRow(GridView1.FocusedRowHandle)

                                    If dsEntri.Tables("Detil").Rows.Count >= 1 Then
                                        txtGudangAsal.Enabled = False
                                        txtTanggal.Enabled = False
                                    Else
                                        txtGudangAsal.Enabled = True
                                        txtTanggal.Enabled = True
                                    End If

                                    If pStatus = pStatusForm.Posted Then
                                        mnGenerate.Enabled = False
                                        cmdGenerate.Enabled = False
                                        mnSimpan.Enabled = False
                                        GridView1.OptionsBehavior.Editable = False
                                        mnPreview.Enabled = False
                                    Else
                                        mnGenerate.Enabled = True
                                        cmdGenerate.Enabled = True
                                        mnSimpan.Enabled = True
                                        GridView1.OptionsBehavior.Editable = True
                                        mnPreview.Enabled = True
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

                                com.CommandText = "SELECT COUNT(Kode) FROM M" & NamaForm & " WHERE Kode=@Kode AND NoID<>@NoID"
                                com.Parameters.Clear()
                                com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NoID
                                com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = txtKode.Text
                                If NullToLong(com.ExecuteScalar()) >= 1 Then
                                    XtraMessageBox.Show("Nota telah sudah ada!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    Exit Try
                                End If

                                If Not DxErrorProvider1.HasErrors Then
                                    If pStatus = pStatusForm.Edit OrElse pStatus = pStatusForm.TempInsert Then
                                        'Simpan Detil
                                        com.CommandText = "DELETE FROM M" & NamaForm & "D WHERE IDHeader=" & NoID
                                        com.ExecuteNonQuery()

                                        Dim IDDetil As Long = -1
                                        com.CommandText = "SELECT MAX(NoID) FROM M" & NamaForm & "D(NOLOCK)"
                                        IDDetil = NullToLong(com.ExecuteScalar())
                                        For Each iRow As DataRow In dsEntri.Tables("Detil").Rows
                                            If Not (NullToDbl(iRow.Item("QtyKomputer")) = 0 AndAlso NullToDbl(iRow.Item("QtyFisik")) = 0) Then
                                                com.CommandText = "INSERT INTO M" & NamaForm & "D(NoID, IDHeader, IDBarangD, IDBarang, IDSatuan, Konversi, QtyKomputer, QtyFisik, Qty, HPP, Jumlah, Keterangan)" & vbCrLf & _
                                                                  "SELECT ISNULL(@NoID, 0) + 1 NoID, @IDHeader IDHeader, MAX(MBarangD.NoID) IDBarangD, @IDBarang, MBarangD.IDSatuan, 1 Konversi, @QtyKomputer, @QtyFisik, @QtySelisih, @HPP, @Jumlah, @Keterangan" & vbCrLf & _
                                                                  "FROM MBarang(NOLOCK)" & vbCrLf & _
                                                                  "INNER JOIN MBarangD(NOLOCK) ON MBarang.NoID=MBarangD.IDBarang" & vbCrLf & _
                                                                  "WHERE MBarang.NoID=@IDBarang AND MBarangD.Konversi=1" & vbCrLf & _
                                                                  "GROUP BY MBarangD.IDSatuan"
                                                com.Parameters.Clear()
                                                IDDetil += 1
                                                com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = IDDetil
                                                com.Parameters.Add(New SqlParameter("@IDHeader", SqlDbType.BigInt)).Value = NoID
                                                com.Parameters.Add(New SqlParameter("@IDBarang", SqlDbType.BigInt)).Value = NullToLong(iRow.Item("NoID"))
                                                com.Parameters.Add(New SqlParameter("@QtyKomputer", SqlDbType.Decimal)).Value = NullToDbl(iRow.Item("QtyKomputer"))
                                                com.Parameters.Add(New SqlParameter("@QtyFisik", SqlDbType.Decimal)).Value = NullToDbl(iRow.Item("QtyFisik"))
                                                com.Parameters.Add(New SqlParameter("@QtySelisih", SqlDbType.Decimal)).Value = NullToDbl(iRow.Item("QtySelisih"))
                                                com.Parameters.Add(New SqlParameter("@HPP", SqlDbType.Money)).Value = NullToDbl(iRow.Item("HargaBeliPcs"))
                                                com.Parameters.Add(New SqlParameter("@Jumlah", SqlDbType.Money)).Value = NullToDbl(iRow.Item("Jumlah"))
                                                com.Parameters.Add(New SqlParameter("@Keterangan", SqlDbType.VarChar)).Value = ""
                                                com.ExecuteNonQuery()
                                            End If
                                            Application.DoEvents()
                                        Next
                                    End If

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
#End Region

    Private Sub frmEntriPO_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Using dlg As New DevExpress.Utils.WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
            Try
                dlg.Show()
                dlg.Focus()
                InitLoadLookUp()
                LoadData()
                RefreshDetil()
                If System.IO.File.Exists([Public].SettingPerusahaan.PathLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
                    LayoutControl1.RestoreLayoutFromXml([Public].SettingPerusahaan.PathLayouts & Me.Name & LayoutControl1.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub txtSupplier_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGudangAsal.EditValueChanged

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

    Private IsHitung As Boolean = False, RowHasChanged As Boolean = False
    Private Sub GridView1_CellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        If Not IsHitung Then
            IsHitung = True
            Try
                Select Case e.Column.FieldName.ToLower
                    Case "QtyFisik".ToLower
                        bsEntri.Current.Item("QtySelisih") = NullToDbl(bsEntri.Current.Item("QtyKomputer")) - NullToDbl(bsEntri.Current.Item("QtyFisik"))
                        bsEntri.Current.Item("Jumlah") = Bulatkan(NullToDbl(bsEntri.Current.Item("HargaBeliPcs")) * NullToDbl(bsEntri.Current.Item("QtySelisih")), 2)
                        RowHasChanged = True
                End Select
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            IsHitung = False
        End If
    End Sub

    Private Sub gv_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataSourceChanged, _
    gvGudangAsal.DataSourceChanged
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

                If .Columns(i).FieldName.ToString.ToLower = "QtyFisik".ToLower Then
                    repCalcEdit.Mask.MaskType = Mask.MaskType.Numeric
                    repCalcEdit.Mask.EditMask = "n0"
                    repCalcEdit.Mask.UseMaskAsDisplayFormat = True
                    .Columns(i).columnEdit = repCalcEdit
                End If
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

    Private Sub mnSaveLayouts_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayouts.ItemClick
        Using frm As New frmOtorisasi
            Try
                If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    LayoutControl1.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & LayoutControl1.Name & ".xml")
                    gvGudangAsal.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & gvGudangAsal.Name & ".xml")
                    GridView1.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & GridView1.Name & ".xml")
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
                    TryCast(frm, frmDaftarTransaksi).Name = modMain.FormName.DaftarStockOpname.ToString Then
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
        'If txtTotal.EditValue < 0 Then
        '    DxErrorProvider1.SetError(txtTotal, "Total PeM" & NamaForm & "an salah!", DXErrorProvider.ErrorType.Critical)
        'End If
        'If Not pStatus = pStatusForm.Baru AndAlso Not CekDetil() Then
        '    DxErrorProvider1.SetError(txtKode, "Item masih kosong!", DXErrorProvider.ErrorType.Critical)
        'End If

        Return Not DxErrorProvider1.HasErrors
    End Function

    Private Sub cmdGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGenerate.Click
        mnGenerate.PerformClick()
    End Sub

    Private Sub mnGenerate_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnGenerate.ItemClick
        If pStatus = pStatusForm.Baru Then
            If SimpanData() Then
                mnRefresh.PerformClick()
            End If
        Else
            mnRefresh.PerformClick()
        End If
    End Sub

    Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
        Select Case e.FocusedColumn.FieldName.ToLower
            Case "QtyFisik".ToLower
                e.FocusedColumn.OptionsColumn.AllowEdit = True
            Case Else
                e.FocusedColumn.OptionsColumn.AllowEdit = False
        End Select
    End Sub

    Private Sub GridView1_RowCellStyle(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs) Handles GridView1.RowCellStyle
        Select Case e.Column.FieldName.ToLower
            Case "QtyFisik".ToLower
                e.Column.AppearanceCell.BackColor = Color.YellowGreen
                e.Column.AppearanceCell.BackColor2 = Color.YellowGreen
        End Select
    End Sub

    Private Sub mnPreview_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPreview.ItemClick
        If pStatus = pStatusForm.Edit OrElse pStatus = pStatusForm.TempInsert Then
            Dim NamaFile As String = ""
            NamaFile = Application.StartupPath & "\Report\FormInput_StockOpname.repx"
            Dim CalculateFields As New List(Of Model.CetakDX.CalculateFields)
            CalculateFields.Add(New Model.CetakDX.CalculateFields With {.Name = "Gudang", _
                                                                        .Type = Model.CetakDX.CalculateFields.iType.String, _
                                                                        .Value = txtGudangAsal.Text})
            CalculateFields.Add(New Model.CetakDX.CalculateFields With {.Name = "Tanggal", _
                                                                        .Type = Model.CetakDX.CalculateFields.iType.VariantDateTime, _
                                                                        .Value = txtTanggal.DateTime.ToString("yyyy-MM-dd HH:mm:ss")})
            CalculateFields.Add(New Model.CetakDX.CalculateFields With {.Name = "FilterKategori", _
                                                                        .Type = Model.CetakDX.CalculateFields.iType.String, _
                                                                        .Value = IIf(NullToLong(txtKategori.EditValue) >= 1, txtKategori.Text, "ALL")})
            CalculateFields.Add(New Model.CetakDX.CalculateFields With {.Name = "FilterSupplier", _
                                                                        .Type = Model.CetakDX.CalculateFields.iType.String, _
                                                                        .Value = IIf(NullToLong(txtSupplier.EditValue) >= 1, txtSupplier.Text, "ALL")})
            CalculateFields.Add(New Model.CetakDX.CalculateFields With {.Name = "FilterMerk", _
                                                                        .Type = Model.CetakDX.CalculateFields.iType.String, _
                                                                        .Value = IIf(NullToLong(txtMerk.EditValue) >= 1, txtMerk.Text, "ALL")})
            ViewXtraReport(Me.MdiParent, IIf(IsEditReport, ActionPrint.Edit, ActionPrint.Preview), NamaFile, "Form Input Stock Opname", "FormInput_StockOpname.repx", Me.dsEntri, , CalculateFields)
        Else
            XtraMessageBox.Show("Posisi Preview masih tertutup, Klik Generate atau Unposting data Terlebih dahulu!", NamaAplikasi, MessageBoxButtons.OK)
        End If
    End Sub
End Class
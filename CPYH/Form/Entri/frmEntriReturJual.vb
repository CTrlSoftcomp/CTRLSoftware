Imports CtrlSoft.Utils
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors
Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.Utils

Public Class frmEntriReturJual
    Private NoID As Long = -1
    Private pStatus As pStatusForm
    Private IDTypePajak As Integer = -1

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Private Sub GridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DoubleClick
        mnEdit.PerformClick()
    End Sub

    Public Sub New(ByVal NoID As Long)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.NoID = NoID
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

                            com.CommandText = "SELECT NoID, Kode, Nama, Alamat FROM MAlamat WHERE IsActive=1 AND IsCustomer=1"
                            oDA.Fill(ds, "MCustomer")
                            txtCustomer.Properties.DataSource = ds.Tables("MCustomer")
                            txtCustomer.Properties.ValueMember = "NoID"
                            txtCustomer.Properties.DisplayMember = "Kode"

                            com.CommandText = "SELECT NoID, Kode + '-' + Nama AS Gudang FROM MGudang WHERE IsActive=1"
                            oDA.Fill(ds, "MGudang")
                            txtGudang.Properties.DataSource = ds.Tables("MGudang")
                            txtGudang.Properties.ValueMember = "NoID"
                            txtGudang.Properties.DisplayMember = "Gudang"

                            com.CommandText = "SELECT NoID, TypePajak FROM MTypePajak"
                            oDA.Fill(ds, "MTypePajak")
                            txtTypePajak.Properties.DataSource = ds.Tables("MTypePajak")
                            txtTypePajak.Properties.ValueMember = "NoID"
                            txtTypePajak.Properties.DisplayMember = "TypePajak"

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

                            com.CommandText = "SELECT TOP 1 * FROM MReturJual WHERE NoID=" & NoID
                            oDA.Fill(ds, "MReturJual")

                            If ds.Tables("MReturJual").Rows.Count >= 1 Then
                                Dim iRow As DataRow = ds.Tables("MReturJual").Rows(0)
                                If NullToBool(iRow.Item("IsPosted")) Then
                                    pStatus = pStatusForm.Posted
                                Else
                                    pStatus = pStatusForm.Edit
                                End If
                                txtGudang.EditValue = NullToLong(iRow.Item("IDGudang"))
                                txtTanggal.EditValue = NullToDate(iRow.Item("Tanggal"))
                                txtKode.Text = NullToStr(iRow.Item("Kode"))
                                txtJatuhTempo.EditValue = NullToDate(iRow.Item("JatuhTempo"))
                                txtCustomer.EditValue = -1
                                txtCustomer.EditValue = NullToLong(iRow.Item("IDCustomer"))
                                txtTypePajak.EditValue = NullToLong(iRow.Item("IDTypePajak"))
                                IDTypePajak = txtTypePajak.EditValue
                                txtNoReff.Text = NullToStr(iRow.Item("NoReff"))
                                txtCatatan.Text = NullToStr(iRow.Item("Catatan"))
                            Else
                                txtGudang.EditValue = -1
                                txtTanggal.EditValue = Now
                                txtCustomer.EditValue = -1
                                txtTypePajak.EditValue = 1
                                IDTypePajak = txtTypePajak.EditValue
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

                                com.CommandText = "SELECT MReturJualD.*, MSatuan.Kode AS Satuan, MBarangD.Barcode, MBarang.Kode KodeBarang, MBarang.Nama NamaBarang" & vbCrLf & _
                                                  "FROM MReturJualD" & vbCrLf & _
                                                  "LEFT JOIN MBarang ON MBarang.NoID=MReturJualD.IDBarang" & vbCrLf & _
                                                  "LEFT JOIN MBarangD ON MBarangD.NoID=MReturJualD.IDBarangD" & vbCrLf & _
                                                  "LEFT JOIN MSatuan ON MSatuan.NoID=MReturJualD.IDSatuan" & vbCrLf & _
                                                  "WHERE MReturJualD.IDHeader=" & NoID
                                oDA.Fill(ds, "MReturJualD")
                                GridControl1.DataSource = ds.Tables("MReturJualD")
                                GridView1.RefreshData()

                                GridView1.ClearSelection()
                                GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), IDDetil.ToString("n0"))
                                GridView1.SelectRow(GridView1.FocusedRowHandle)

                                If ds.Tables("MReturJualD").Rows.Count >= 1 Then
                                    txtCustomer.Enabled = False
                                    txtTanggal.Enabled = False
                                Else
                                    txtCustomer.Enabled = True
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

                                com.CommandText = "SELECT SUM(MReturJualD.JumlahBruto) JumlahBruto, SUM(MReturJualD.Jumlah) Jumlah, SUM(MReturJualD.DPP) DPP, SUM(MReturJualD.PPN) PPN" & vbCrLf & _
                                                  "FROM MReturJualD INNER JOIN MReturJual ON MReturJual.NoID=MReturJualD.IDHeader" & vbCrLf & _
                                                  "WHERE MReturJual.NoID=" & NoID & vbCrLf & _
                                                  "GROUP BY MReturJual.NoID, MReturJual.IDTypePajak"
                                oDA.Fill(ds, "MHitung")
                                If ds.Tables("MHitung").Rows.Count >= 1 Then
                                    txtSubtotal.EditValue = NullToDbl(ds.Tables("MHitung").Rows(0).Item("JumlahBruto"))
                                    txtDPP.EditValue = NullToDbl(ds.Tables("MHitung").Rows(0).Item("DPP"))
                                    txtPPN.EditValue = NullToDbl(ds.Tables("MHitung").Rows(0).Item("PPN"))
                                    txtTotal.EditValue = NullToDbl(ds.Tables("MHitung").Rows(0).Item("Jumlah"))
                                Else
                                    txtSubtotal.EditValue = 0
                                    txtDPP.EditValue = 0
                                    txtPPN.EditValue = 0
                                    txtTotal.EditValue = 0
                                End If

                                com.CommandText = "SELECT SUM(MReturJualDBayar.ChargeRp) ChargeRp, SUM(MReturJualDBayar.Total) TotalBayar" & vbCrLf & _
                                                  "FROM MReturJualDBayar INNER JOIN MReturJual ON MReturJual.NoID=MReturJualDBayar.IDHeader" & vbCrLf & _
                                                  "WHERE MReturJual.NoID=" & NoID & vbCrLf & _
                                                  "GROUP BY MReturJualDBayar.NoID"
                                oDA.Fill(ds, "MBayar")
                                If ds.Tables("MBayar").Rows.Count >= 1 Then
                                    TextEdit1.EditValue = NullToDbl(ds.Tables("MBayar").Rows(0).Item("TotalBayar"))
                                    TextEdit2.EditValue = Bulatkan(txtTotal.EditValue + NullToDbl(ds.Tables("MBayar").Rows(0).Item("ChargeRp")) - NullToDbl(ds.Tables("MBayar").Rows(0).Item("TotalBayar")), 2)
                                Else
                                    TextEdit1.EditValue = 0.0
                                    TextEdit2.EditValue = Bulatkan(txtTotal.EditValue, 2)
                                End If

                                com.CommandText = "SELECT A.NoID, B.Nama Pembayaran, A.Total, A.Nominal, A.ChargeProsen [Charge (%)], A.ChargeRp [Charge (Rp)], A.NoRekening, A.AtasNama" & vbCrLf & _
                                                  "FROM MReturJualDBayar A" & vbCrLf & _
                                                  "LEFT JOIN MJenisPembayaran B ON B.NoID=A.IDJenisPembayaran" & vbCrLf & _
                                                  "WHERE A.IDHeader=" & NoID & vbCrLf & _
                                                  "ORDER BY A.NoID"
                                oDA.Fill(ds, "MReturJualDBayar")
                                GridControl2.DataSource = ds.Tables("MReturJualDBayar")
                                GridView2.RefreshData()
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Private ListPembayaran As New List(Of Model.Pembayaran)

    Private Function SimpanData(ByVal MunculkanPembayaran As Boolean) As Boolean
        Dim Hasil As Boolean = False
        Dim frm As frmEntriReturJualDBayar = Nothing
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
                                    com.CommandText = "SELECT IsPosted FROM MReturJual WHERE NoID=" & NoID
                                    If NullToBool(com.ExecuteScalar()) Then
                                        XtraMessageBox.Show("Nota telah diposting!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                        Exit Try
                                    End If
                                ElseIf pStatus = pStatusForm.Posted Then
                                    XtraMessageBox.Show("Nota telah diposting!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    Exit Try
                                End If

                                com.CommandText = "SELECT SUM(MReturJualD.Qty*MReturJualD.Konversi) AS QtyRetur, MReturJualD.IDBarang, MBarang.Kode, MBarang.Nama" & vbCrLf & _
                                                  "FROM MReturJualD" & vbCrLf & _
                                                  "INNER JOIN MReturJual ON MReturJual.NoID=MReturJualD.IDHeader" & vbCrLf & _
                                                  "LEFT JOIN MBarang ON MBarang.NoID=MReturJualD.IDBarang" & vbCrLf & _
                                                  "WHERE MReturJual.NoID=" & NoID & vbCrLf & _
                                                  "GROUP BY MReturJualD.IDBarang, MBarang.Kode, MBarang.Nama"
                                If ds.Tables("MReturJualD") IsNot Nothing Then
                                    ds.Tables("MReturJualD").Clear()
                                    ds.Tables("MReturJualD").Columns.Clear()
                                End If
                                oDA.Fill(ds, "MReturJualD")

                                'For Each iRow As DataRow In ds.Tables("MReturJualD").Rows
                                '    com.CommandText = "EXEC spCekSaldoStok " & NullToLong(iRow.Item("IDBarang")) & ", " & NullToLong(txtGudang.EditValue) & ", '" & txtTanggal.DateTime.ToString("yyyy-MM-dd HH:mm:ss") & "'"
                                '    If NullToDbl(com.ExecuteScalar()) < NullToDbl(iRow.Item("QtyRetur")) Then
                                '        DxErrorProvider1.SetError(txtKode, "Saldo Stok Tidak Cukup!")
                                '    End If
                                'Next

                                com.CommandText = "SELECT COUNT(Kode) FROM MReturJual WHERE Kode=@Kode AND NoID<>@NoID"
                                com.Parameters.Clear()
                                com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NoID
                                com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = txtKode.Text
                                If NullToLong(com.ExecuteScalar()) >= 1 Then
                                    XtraMessageBox.Show("Nota telah sudah ada!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    Exit Try
                                End If

                                ListPembayaran = New List(Of Model.Pembayaran)
                                If Not pStatus = pStatusForm.Baru Then
                                    com.CommandText = "SELECT MReturJualDBayar.*, MJenisPembayaran.IsBank FROM MReturJualDBayar LEFT JOIN MJenisPembayaran ON MJenisPembayaran.NoID=MReturJualDBayar.IDJenisPembayaran WHERE MReturJualDBayar.IDHeader=" & NoID & " ORDER BY MReturJualDBayar.NoID"
                                    oDA.Fill(ds, "MReturJualDBayar")
                                    For Each iRow As DataRow In ds.Tables("MReturJualDBayar").Rows
                                        ListPembayaran.Add(New Model.Pembayaran With {.NoID = NullToLong(iRow.Item("NoID")), _
                                                                                      .IDJenisPembayaran = NullToLong(iRow.Item("IDJenisPembayaran")), _
                                                                                      .AtasNama = NullToStr(iRow.Item("AtasNama")), _
                                                                                      .NoRekening = NullToStr(iRow.Item("NoRekening")), _
                                                                                      .IsBank = NullToBool(iRow.Item("IsBank")), _
                                                                                      .ChargeProsen = NullToDbl(iRow.Item("ChargeProsen")), _
                                                                                      .ChargeRp = NullToDbl(iRow.Item("ChargeRp")), _
                                                                                      .Total = NullToDbl(iRow.Item("Total")), _
                                                                                      .Nominal = NullToDbl(iRow.Item("Nominal"))})
                                    Next
                                    If MunculkanPembayaran Then
                                        frm = New frmEntriReturJualDBayar(txtCustomer.EditValue, txtTotal.EditValue, ListPembayaran)
                                        If frm.ShowDialog(Me) <> Windows.Forms.DialogResult.OK Then
                                            DxErrorProvider1.SetError(txtKode, "Pembayaran dibatalkan!")
                                        Else
                                            ListPembayaran = frm.iList
                                        End If
                                    End If
                                End If

                                If Not DxErrorProvider1.HasErrors Then
                                    com.CommandText = "EXEC [dbo].[spSimpanMReturJual] @NoID,@Kode,@NoReff,@IDCustomer,@Tanggal,@JatuhTempo,@Catatan,@IDTypePajak,@Subtotal," & vbCrLf & _
                                                  "@DiscNotaProsen,@DiscNotaRp,@TotalBruto,@DPP,@PPN,@Total,@Sisa,@IsPosted,@TglPosted,@IDUserPosted,@IDUserEntry,@IDUserEdit,@IDGudang"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NoID
                                    com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = txtKode.Text
                                    com.Parameters.Add(New SqlParameter("@NoReff", SqlDbType.VarChar)).Value = txtNoReff.Text
                                    com.Parameters.Add(New SqlParameter("@IDCustomer", SqlDbType.Int)).Value = NullTolInt(txtCustomer.EditValue)
                                    com.Parameters.Add(New SqlParameter("@Tanggal", SqlDbType.DateTime)).Value = NullToDate(txtTanggal.EditValue)
                                    com.Parameters.Add(New SqlParameter("@JatuhTempo", SqlDbType.Date)).Value = NullToDate(txtJatuhTempo.EditValue)
                                    com.Parameters.Add(New SqlParameter("@Catatan", SqlDbType.VarChar)).Value = txtCatatan.Text
                                    com.Parameters.Add(New SqlParameter("@IDTypePajak", SqlDbType.SmallInt)).Value = NullTolInt(txtTypePajak.EditValue)
                                    com.Parameters.Add(New SqlParameter("@Subtotal", SqlDbType.Money)).Value = NullToDbl(txtSubtotal.EditValue)
                                    com.Parameters.Add(New SqlParameter("@DiscNotaProsen", SqlDbType.Float)).Value = 0.0
                                    com.Parameters.Add(New SqlParameter("@DiscNotaRp", SqlDbType.Float)).Value = 0.0
                                    com.Parameters.Add(New SqlParameter("@TotalBruto", SqlDbType.Money)).Value = NullToDbl(txtSubtotal.EditValue)
                                    com.Parameters.Add(New SqlParameter("@DPP", SqlDbType.Money)).Value = NullToDbl(txtDPP.EditValue)
                                    com.Parameters.Add(New SqlParameter("@PPN", SqlDbType.Money)).Value = NullToDbl(txtPPN.EditValue)
                                    com.Parameters.Add(New SqlParameter("@Total", SqlDbType.Money)).Value = NullToDbl(txtTotal.EditValue)
                                    com.Parameters.Add(New SqlParameter("@Sisa", SqlDbType.Money)).Value = (NullToDbl(txtTotal.EditValue) + NullToDbl(ListPembayaran.Sum(Function(m) m.ChargeRp))) - NullToDbl(ListPembayaran.Sum(Function(m) m.Total))
                                    com.Parameters.Add(New SqlParameter("@IsPosted", SqlDbType.Bit)).Value = False
                                    com.Parameters.Add(New SqlParameter("@TglPosted", SqlDbType.DateTime)).Value = System.DBNull.Value
                                    com.Parameters.Add(New SqlParameter("@IDUserPosted", SqlDbType.Int)).Value = System.DBNull.Value
                                    com.Parameters.Add(New SqlParameter("@IDUserEntry", SqlDbType.Int)).Value = UserLogin.NoID
                                    com.Parameters.Add(New SqlParameter("@IDUserEdit", SqlDbType.Int)).Value = IIf(pStatus = pStatusForm.Edit, UserLogin.NoID, -1)
                                    com.Parameters.Add(New SqlParameter("@IDGudang", SqlDbType.Int)).Value = NullToLong(txtGudang.EditValue)

                                    NoID = NullToLong(com.ExecuteScalar())
                                    com.Parameters.Clear()

                                    com.CommandText = "UPDATE MReturJual SET Subtotal=ISNULL(MReturJualD.JumlahBruto, 0), TotalBruto=ISNULL(MReturJualD.JumlahBruto, 0), " & vbCrLf & _
                                                      "DPP=ROUND(CASE WHEN MReturJual.IDTypePajak=0 THEN 0 WHEN MReturJual.IDTypePajak=2 THEN ISNULL(MReturJualD.JumlahBruto, 0)/1.0 ELSE ISNULL(MReturJualD.JumlahBruto, 0)/1.1 END, 0) " & vbCrLf & _
                                                      "FROM MReturJual " & vbCrLf & _
                                                      "INNER JOIN (SELECT IDHeader, SUM(JumlahBruto) AS JumlahBruto FROM MReturJualD GROUP BY IDHeader) AS MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID " & vbCrLf & _
                                                      "WHERE MReturJual.NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "UPDATE MReturJual SET " & vbCrLf & _
                                                      "PPN=ROUND((CASE WHEN MReturJual.IDTypePajak=0 THEN 0 WHEN MReturJual.IDTypePajak=1 THEN ISNULL(MReturJualD.JumlahBruto, 0)-ISNULL(MReturJualD.DPP, 0) ELSE 0.1*ISNULL(MReturJualD.JumlahBruto, 0) END), 0) " & vbCrLf & _
                                                      "FROM MReturJual " & vbCrLf & _
                                                      "INNER JOIN (SELECT IDHeader, SUM(JumlahBruto) AS JumlahBruto, SUM(DPP) AS DPP, SUM(PPN) AS PPN FROM MReturJualD GROUP BY IDHeader) AS MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID " & vbCrLf & _
                                                      "WHERE MReturJualD.IDHeader=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "UPDATE MReturJualD SET " & vbCrLf & _
                                                      "PPN=ROUND((CASE WHEN MReturJual.IDTypePajak=0 THEN 0 WHEN MReturJual.IDTypePajak=1 THEN ISNULL(MReturJualD.JumlahBruto, 0)-ISNULL(MReturJualD.DPP, 0) ELSE 0.1*ISNULL(MReturJualD.JumlahBruto, 0) END), 0) " & vbCrLf & _
                                                      "FROM MReturJual " & vbCrLf & _
                                                      "INNER JOIN MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID " & vbCrLf & _
                                                      "WHERE MReturJualD.IDHeader=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "UPDATE MReturJualD SET " & vbCrLf & _
                                                      "DPP=ROUND(CASE WHEN MReturJual.IDTypePajak=0 THEN 0 WHEN MReturJual.IDTypePajak=2 THEN ISNULL(MReturJualD.JumlahBruto, 0)/1.0 ELSE ISNULL(MReturJualD.JumlahBruto, 0)/1.1 END, 0) " & vbCrLf & _
                                                      "FROM MReturJual " & vbCrLf & _
                                                      "INNER JOIN MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID " & vbCrLf & _
                                                      "WHERE MReturJual.NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "UPDATE MReturJualD SET " & vbCrLf & _
                                                      "PPN=ROUND((CASE WHEN MReturJual.IDTypePajak=0 THEN 0 WHEN MReturJual.IDTypePajak=1 THEN ISNULL(MReturJualD.JumlahBruto, 0)-ISNULL(MReturJualD.DPP, 0) ELSE 0.1*ISNULL(MReturJualD.JumlahBruto, 0) END), 0) " & vbCrLf & _
                                                      "FROM MReturJual " & vbCrLf & _
                                                      "INNER JOIN MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID " & vbCrLf & _
                                                      "WHERE MReturJualD.IDHeader=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "UPDATE MReturJualD SET " & vbCrLf & _
                                                      "PPN=ISNULL(MReturJual.PPN, 0)-(ISNULL(Detil.PPN, 0)-ISNULL(MReturJualD.PPN, 0)), " & vbCrLf & _
                                                      "DPP=ISNULL(MReturJual.DPP, 0)-(ISNULL(Detil.DPP, 0)-ISNULL(MReturJualD.DPP, 0)) " & vbCrLf & _
                                                      "FROM MReturJual " & vbCrLf & _
                                                      "INNER JOIN MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID" & vbCrLf & _
                                                      "INNER JOIN (SELECT IDHeader, SUM(DPP) AS DPP, SUM(PPN) AS PPN, MAX(NoID) AS NoID FROM MReturJualD GROUP BY IDHeader) AS Detil ON Detil.IDHeader=MReturJual.NoID AND Detil.NoID=MReturJualD.NoID" & vbCrLf & _
                                                      "WHERE MReturJualD.IDHeader=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "UPDATE MReturJualD SET " & vbCrLf & _
                                                      "Jumlah=CASE WHEN MReturJual.IDTypePajak=0 THEN MReturJualD.JumlahBruto ELSE MReturJualD.DPP+MReturJualD.PPN END " & vbCrLf & _
                                                      "FROM MReturJual " & vbCrLf & _
                                                      "INNER JOIN MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID " & vbCrLf & _
                                                      "WHERE MReturJualD.IDHeader=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM MReturJualDBayar WHERE IDHeader=" & NoID
                                    com.ExecuteNonQuery()
                                    For Each bayar In ListPembayaran
                                        com.CommandText = "INSERT INTO [dbo].[MReturJualDBayar] ([IDHeader],[IDJenisPembayaran],[AtasNama],[NoRekening],[Nominal],[ChargeProsen],[ChargeRp],[Total]) VALUES (@IDHeader,@IDJenisPembayaran,@AtasNama,@NoRekening,@Nominal,@ChargeProsen,@ChargeRp,@Total)"
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@IDHeader", SqlDbType.BigInt)).Value = NoID
                                        com.Parameters.Add(New SqlParameter("@IDJenisPembayaran", SqlDbType.Int)).Value = bayar.IDJenisPembayaran
                                        com.Parameters.Add(New SqlParameter("@AtasNama", SqlDbType.VarChar)).Value = NullToStr(bayar.AtasNama)
                                        com.Parameters.Add(New SqlParameter("@NoRekening", SqlDbType.VarChar)).Value = NullToStr(bayar.NoRekening)
                                        com.Parameters.Add(New SqlParameter("@Nominal", SqlDbType.Money)).Value = bayar.Nominal
                                        com.Parameters.Add(New SqlParameter("@ChargeProsen", SqlDbType.Float)).Value = bayar.ChargeProsen
                                        com.Parameters.Add(New SqlParameter("@ChargeRp", SqlDbType.Money)).Value = bayar.ChargeRp
                                        com.Parameters.Add(New SqlParameter("@Total", SqlDbType.Money)).Value = bayar.Total
                                        com.ExecuteNonQuery()
                                    Next

                                    com.CommandText = "UPDATE MReturJual SET Subtotal=ISNULL(MReturJualD.JumlahBruto, 0), TotalBruto=ISNULL(MReturJualD.JumlahBruto, 0), Total=ISNULL(MReturJualD.Jumlah, 0), " & vbCrLf & _
                                                      "Bayar=ISNULL(MReturJualDBayar.TotalBayar, 0), Sisa=CASE WHEN ISNULL(MReturJualD.Jumlah, 0)+ISNULL(MReturJualDBayar.ChargeRp, 0)-ISNULL(MReturJualDBayar.TotalBayar, 0)<0 THEN 0 ELSE ISNULL(MReturJualD.Jumlah, 0)+ISNULL(MReturJualDBayar.ChargeRp, 0)-ISNULL(MReturJualDBayar.TotalBayar, 0) END" & vbCrLf & _
                                                      "FROM MReturJual " & vbCrLf & _
                                                      "LEFT JOIN (SELECT IDHeader, SUM(JumlahBruto) AS JumlahBruto, SUM(Jumlah) AS Jumlah FROM MReturJualD GROUP BY IDHeader) AS MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID " & vbCrLf & _
                                                      "LEFT JOIN (SELECT IDHeader, SUM(Total) AS TotalBayar, SUM(ChargeRp) AS ChargeRp FROM MReturJualDBayar GROUP BY IDHeader) AS MReturJualDBayar ON MReturJualDBayar.IDHeader=MReturJual.NoID " & vbCrLf & _
                                                      "WHERE MReturJual.NoID=" & NoID
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
                            Finally
                                If frm IsNot Nothing Then
                                    frm.Dispose()
                                End If
                            End Try
                        End Using
                    End Using
                End Using
            End Using
        End If
        Return Hasil
    End Function
    Private Function UpdateTypePajak() As Boolean
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

                            com.CommandText = "SELECT IsPosted FROM MReturJual WHERE NoID=" & NoID
                            If NullToBool(com.ExecuteScalar()) Then
                                XtraMessageBox.Show("Nota telah diposting!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                Exit Try
                            End If

                            com.CommandText = "UPDATE MReturJual SET IDTypePajak=" & NullToLong(txtTypePajak.EditValue) & " WHERE NoID=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MReturJual SET Subtotal=ISNULL(MReturJualD.JumlahBruto, 0), TotalBruto=ISNULL(MReturJualD.JumlahBruto, 0), " & vbCrLf & _
                                              "DPP=ROUND(CASE WHEN MReturJual.IDTypePajak=0 THEN 0 WHEN MReturJual.IDTypePajak=2 THEN ISNULL(MReturJualD.JumlahBruto, 0)/1.0 ELSE ISNULL(MReturJualD.JumlahBruto, 0)/1.1 END, 0) " & vbCrLf & _
                                              "FROM MReturJual " & vbCrLf & _
                                              "INNER JOIN (SELECT IDHeader, SUM(JumlahBruto) AS JumlahBruto FROM MReturJualD GROUP BY IDHeader) AS MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID " & vbCrLf & _
                                              "WHERE MReturJual.NoID=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MReturJual SET " & vbCrLf & _
                                              "PPN=ROUND((CASE WHEN MReturJual.IDTypePajak=0 THEN 0 WHEN MReturJual.IDTypePajak=1 THEN ISNULL(MReturJualD.JumlahBruto, 0)-ISNULL(MReturJualD.DPP, 0) ELSE 0.1*ISNULL(MReturJualD.JumlahBruto, 0) END), 0) " & vbCrLf & _
                                              "FROM MReturJual " & vbCrLf & _
                                              "INNER JOIN (SELECT IDHeader, SUM(JumlahBruto) AS JumlahBruto, SUM(DPP) AS DPP, SUM(PPN) AS PPN FROM MReturJualD GROUP BY IDHeader) AS MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID " & vbCrLf & _
                                              "WHERE MReturJualD.IDHeader=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MReturJualD SET " & vbCrLf & _
                                              "PPN=ROUND((CASE WHEN MReturJual.IDTypePajak=0 THEN 0 WHEN MReturJual.IDTypePajak=1 THEN ISNULL(MReturJualD.JumlahBruto, 0)-ISNULL(MReturJualD.DPP, 0) ELSE 0.1*ISNULL(MReturJualD.JumlahBruto, 0) END), 0) " & vbCrLf & _
                                              "FROM MReturJual " & vbCrLf & _
                                              "INNER JOIN MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID " & vbCrLf & _
                                              "WHERE MReturJualD.IDHeader=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MReturJualD SET " & vbCrLf & _
                                              "DPP=ROUND(CASE WHEN MReturJual.IDTypePajak=0 THEN 0 WHEN MReturJual.IDTypePajak=2 THEN ISNULL(MReturJualD.JumlahBruto, 0)/1.0 ELSE ISNULL(MReturJualD.JumlahBruto, 0)/1.1 END, 0) " & vbCrLf & _
                                              "FROM MReturJual " & vbCrLf & _
                                              "INNER JOIN MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID " & vbCrLf & _
                                              "WHERE MReturJual.NoID=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MReturJualD SET " & vbCrLf & _
                                              "PPN=ROUND((CASE WHEN MReturJual.IDTypePajak=0 THEN 0 WHEN MReturJual.IDTypePajak=1 THEN ISNULL(MReturJualD.JumlahBruto, 0)-ISNULL(MReturJualD.DPP, 0) ELSE 0.1*ISNULL(MReturJualD.JumlahBruto, 0) END), 0) " & vbCrLf & _
                                              "FROM MReturJual " & vbCrLf & _
                                              "INNER JOIN MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID " & vbCrLf & _
                                              "WHERE MReturJualD.IDHeader=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MReturJualD SET " & vbCrLf & _
                                              "PPN=ISNULL(MReturJual.PPN, 0)-(ISNULL(Detil.PPN, 0)-ISNULL(MReturJualD.PPN, 0)), " & vbCrLf & _
                                              "DPP=ISNULL(MReturJual.DPP, 0)-(ISNULL(Detil.DPP, 0)-ISNULL(MReturJualD.DPP, 0)) " & vbCrLf & _
                                              "FROM MReturJual " & vbCrLf & _
                                              "INNER JOIN MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID" & vbCrLf & _
                                              "INNER JOIN (SELECT IDHeader, SUM(DPP) AS DPP, SUM(PPN) AS PPN, MAX(NoID) AS NoID FROM MReturJualD GROUP BY IDHeader) AS Detil ON Detil.IDHeader=MReturJual.NoID AND Detil.NoID=MReturJualD.NoID" & vbCrLf & _
                                              "WHERE MReturJualD.IDHeader=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MReturJualD SET " & vbCrLf & _
                                              "Jumlah=CASE WHEN MReturJual.IDTypePajak=0 THEN MReturJualD.JumlahBruto ELSE MReturJualD.DPP+MReturJualD.PPN END " & vbCrLf & _
                                              "FROM MReturJual " & vbCrLf & _
                                              "INNER JOIN MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID " & vbCrLf & _
                                              "WHERE MReturJualD.IDHeader=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MReturJual SET Subtotal=ISNULL(MReturJualD.JumlahBruto, 0), TotalBruto=ISNULL(MReturJualD.JumlahBruto, 0), Total=ISNULL(MReturJualD.Jumlah, 0), " & vbCrLf & _
                                              "Bayar=ISNULL(MReturJualDBayar.TotalBayar, 0), Sisa=ISNULL(MReturJualD.Jumlah, 0)+ISNULL(MReturJualDBayar.ChargeRp, 0)-ISNULL(MReturJualDBayar.TotalBayar, 0)" & vbCrLf & _
                                              "FROM MReturJual " & vbCrLf & _
                                              "LEFT JOIN (SELECT IDHeader, SUM(JumlahBruto) AS JumlahBruto, SUM(Jumlah) AS Jumlah FROM MReturJualD GROUP BY IDHeader) AS MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID " & vbCrLf & _
                                              "LEFT JOIN (SELECT IDHeader, SUM(Total) AS TotalBayar, SUM(ChargeRp) AS ChargeRp FROM MReturJualDBayar GROUP BY IDHeader) AS MReturJualDBayar ON MReturJualDBayar.IDHeader=MReturJual.NoID " & vbCrLf & _
                                              "WHERE MReturJual.NoID=" & NoID
                            com.ExecuteNonQuery()

                            If com.Transaction IsNot Nothing Then
                                com.Transaction.Commit()
                            End If
                            Hasil = True
                            RefreshDetil()
                            IDTypePajak = NullTolInt(txtTypePajak.EditValue)
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

                            com.CommandText = "SELECT IsPosted FROM MReturJual WHERE NoID=" & NoID
                            If NullToBool(com.ExecuteScalar()) Then
                                XtraMessageBox.Show("Nota telah diposting!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                Exit Try
                            End If

                            com.CommandText = "DELETE FROM MReturJualD WHERE NoID=" & IDDetil
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MReturJual SET IDTypePajak=" & NullToLong(txtTypePajak.EditValue) & " WHERE NoID=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MReturJual SET Subtotal=ISNULL(MReturJualD.JumlahBruto, 0), TotalBruto=ISNULL(MReturJualD.JumlahBruto, 0), " & vbCrLf & _
                                              "DPP=ROUND(CASE WHEN MReturJual.IDTypePajak=0 THEN 0 WHEN MReturJual.IDTypePajak=2 THEN ISNULL(MReturJualD.JumlahBruto, 0)/1.0 ELSE ISNULL(MReturJualD.JumlahBruto, 0)/1.1 END, 0) " & vbCrLf & _
                                              "FROM MReturJual " & vbCrLf & _
                                              "INNER JOIN (SELECT IDHeader, SUM(JumlahBruto) AS JumlahBruto FROM MReturJualD GROUP BY IDHeader) AS MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID " & vbCrLf & _
                                              "WHERE MReturJual.NoID=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MReturJual SET " & vbCrLf & _
                                              "PPN=ROUND((CASE WHEN MReturJual.IDTypePajak=0 THEN 0 WHEN MReturJual.IDTypePajak=1 THEN ISNULL(MReturJualD.JumlahBruto, 0)-ISNULL(MReturJualD.DPP, 0) ELSE 0.1*ISNULL(MReturJualD.JumlahBruto, 0) END), 0) " & vbCrLf & _
                                              "FROM MReturJual " & vbCrLf & _
                                              "INNER JOIN (SELECT IDHeader, SUM(JumlahBruto) AS JumlahBruto, SUM(DPP) AS DPP, SUM(PPN) AS PPN FROM MReturJualD GROUP BY IDHeader) AS MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID " & vbCrLf & _
                                              "WHERE MReturJualD.IDHeader=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MReturJualD SET " & vbCrLf & _
                                              "PPN=ROUND((CASE WHEN MReturJual.IDTypePajak=0 THEN 0 WHEN MReturJual.IDTypePajak=1 THEN ISNULL(MReturJualD.JumlahBruto, 0)-ISNULL(MReturJualD.DPP, 0) ELSE 0.1*ISNULL(MReturJualD.JumlahBruto, 0) END), 0) " & vbCrLf & _
                                              "FROM MReturJual " & vbCrLf & _
                                              "INNER JOIN MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID " & vbCrLf & _
                                              "WHERE MReturJualD.IDHeader=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MReturJualD SET " & vbCrLf & _
                                              "DPP=ROUND(CASE WHEN MReturJual.IDTypePajak=0 THEN 0 WHEN MReturJual.IDTypePajak=2 THEN ISNULL(MReturJualD.JumlahBruto, 0)/1.0 ELSE ISNULL(MReturJualD.JumlahBruto, 0)/1.1 END, 0) " & vbCrLf & _
                                              "FROM MReturJual " & vbCrLf & _
                                              "INNER JOIN MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID " & vbCrLf & _
                                              "WHERE MReturJual.NoID=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MReturJualD SET " & vbCrLf & _
                                              "PPN=ROUND((CASE WHEN MReturJual.IDTypePajak=0 THEN 0 WHEN MReturJual.IDTypePajak=1 THEN ISNULL(MReturJualD.JumlahBruto, 0)-ISNULL(MReturJualD.DPP, 0) ELSE 0.1*ISNULL(MReturJualD.JumlahBruto, 0) END), 0) " & vbCrLf & _
                                              "FROM MReturJual " & vbCrLf & _
                                              "INNER JOIN MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID " & vbCrLf & _
                                              "WHERE MReturJualD.IDHeader=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MReturJualD SET " & vbCrLf & _
                                              "PPN=ISNULL(MReturJual.PPN, 0)-(ISNULL(Detil.PPN, 0)-ISNULL(MReturJualD.PPN, 0)), " & vbCrLf & _
                                              "DPP=ISNULL(MReturJual.DPP, 0)-(ISNULL(Detil.DPP, 0)-ISNULL(MReturJualD.DPP, 0)) " & vbCrLf & _
                                              "FROM MReturJual " & vbCrLf & _
                                              "INNER JOIN MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID" & vbCrLf & _
                                              "INNER JOIN (SELECT IDHeader, SUM(DPP) AS DPP, SUM(PPN) AS PPN, MAX(NoID) AS NoID FROM MReturJualD GROUP BY IDHeader) AS Detil ON Detil.IDHeader=MReturJual.NoID AND Detil.NoID=MReturJualD.NoID" & vbCrLf & _
                                              "WHERE MReturJualD.IDHeader=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MReturJualD SET " & vbCrLf & _
                                              "Jumlah=CASE WHEN MReturJual.IDTypePajak=0 THEN MReturJualD.JumlahBruto ELSE MReturJualD.DPP+MReturJualD.PPN END " & vbCrLf & _
                                              "FROM MReturJual " & vbCrLf & _
                                              "INNER JOIN MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID " & vbCrLf & _
                                              "WHERE MReturJualD.IDHeader=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MReturJual SET Subtotal=ISNULL(MReturJualD.JumlahBruto, 0), TotalBruto=ISNULL(MReturJualD.JumlahBruto, 0), Total=ISNULL(MReturJualD.Jumlah, 0), " & vbCrLf & _
                                              "Bayar=ISNULL(MReturJualDBayar.TotalBayar, 0), Sisa=ISNULL(MReturJualD.Jumlah, 0)+ISNULL(MReturJualDBayar.ChargeRp, 0)-ISNULL(MReturJualDBayar.TotalBayar, 0)" & vbCrLf & _
                                              "FROM MReturJual " & vbCrLf & _
                                              "LEFT JOIN (SELECT IDHeader, SUM(JumlahBruto) AS JumlahBruto, SUM(Jumlah) AS Jumlah FROM MReturJualD GROUP BY IDHeader) AS MReturJualD ON MReturJualD.IDHeader=MReturJual.NoID " & vbCrLf & _
                                              "LEFT JOIN (SELECT IDHeader, SUM(Total) AS TotalBayar, SUM(ChargeRp) AS ChargeRp FROM MReturJualDBayar GROUP BY IDHeader) AS MReturJualDBayar ON MReturJualDBayar.IDHeader=MReturJual.NoID " & vbCrLf & _
                                              "WHERE MReturJual.NoID=" & NoID
                            com.ExecuteNonQuery()

                            If com.Transaction IsNot Nothing Then
                                com.Transaction.Commit()
                            End If
                            Hasil = True
                            IDTypePajak = NullTolInt(txtTypePajak.EditValue)
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

    Private Sub txtCustomer_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCustomer.EditValueChanged
        Using cn As New SqlConnection(StrKonSQL)
            Using com As New SqlCommand
                Using oDA As New SqlDataAdapter
                    Using ds As New DataSet
                        Try
                            cn.Open()
                            com.Connection = cn
                            com.CommandTimeout = cn.ConnectionTimeout
                            oDA.SelectCommand = com

                            com.CommandText = "SELECT NoID, Kode, Nama, Alamat FROM MAlamat WHERE NoID=" & NullToLong(txtCustomer.EditValue)
                            oDA.Fill(ds, "MCustomer")
                            If ds.Tables("MCustomer").Rows.Count >= 1 Then
                                txtNamaCustomer.Text = NullToStr(ds.Tables("MCustomer").Rows(0).Item("Nama"))
                                txtAlamatCustomer.Text = NullToStr(ds.Tables("MCustomer").Rows(0).Item("Alamat"))
                            Else
                                txtNamaCustomer.Text = ""
                                txtAlamatCustomer.Text = ""
                            End If
                        Catch ex As Exception
                            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Private Sub txtTanggal_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTanggal.EditValueChanged
        Try
            If pStatus = pStatusForm.Baru Then
                txtKode.Text = "AUTO" 'Repository.RepKode.GetNewKode("MReturJual", "Kode", "PO-" & NullToDate(txtTanggal.EditValue).ToString("yyMM") & "-", "", Repository.RepKode.Format.A00000)
                txtJatuhTempo.EditValue = DateAdd(DateInterval.Day, 30, txtTanggal.EditValue)
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub gv_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataSourceChanged, gvCustomer.DataSourceChanged, gvTypePajak.DataSourceChanged, GridView2.DataSourceChanged
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
            Using frm As New frmEntriReturJualD(Me, NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")), NoID, txtTypePajak.EditValue)
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
        If IIf(pStatus = pStatusForm.Baru, SimpanData(False), True) = True Then
            Using frm As New frmEntriReturJualD(Me, -1, NoID, txtTypePajak.EditValue)
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

    Private Sub mnSaveLayouts_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayouts.ItemClick
        Using frm As New frmOtorisasi
            Try
                If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    LayoutControl1.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & LayoutControl1.Name & ".xml")
                    gvCustomer.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & gvCustomer.Name & ".xml")
                    gvTypePajak.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & gvTypePajak.Name & ".xml")
                    GridView1.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & GridView1.Name & ".xml")
                    GridView2.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & GridView2.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub mnSimpan_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSimpan.ItemClick
        If SimpanData(True) Then
            If pStatus = pStatusForm.Baru Then
                pStatus = pStatusForm.Edit
                LoadData()
            ElseIf pStatus = pStatusForm.Edit OrElse pStatus = pStatusForm.TempInsert Then
                Dim x As frmDaftarTransaksi = Nothing
                For Each frm In Me.MdiParent.MdiChildren
                    If TypeOf frm Is frmDaftarTransaksi AndAlso _
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = modMain.FormName.DaftarReturPenjualan.ToString Then
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
        If txtTypePajak.Text = "" Then
            DxErrorProvider1.SetError(txtTypePajak, "Type Pajak harus dipilih!", DXErrorProvider.ErrorType.Critical)
        End If
        If txtCustomer.Text = "" Then
            DxErrorProvider1.SetError(txtCustomer, "Customer harus dipilih!", DXErrorProvider.ErrorType.Critical)
        End If
        If txtGudang.Text = "" Then
            DxErrorProvider1.SetError(txtGudang, "Gudang harus dipilih!", DXErrorProvider.ErrorType.Critical)
        End If
        If DateDiff(DateInterval.Day, txtTanggal.EditValue, txtJatuhTempo.EditValue) < 0 Then
            DxErrorProvider1.SetError(txtJatuhTempo, "Jatuh Tempo PemReturJualan salah!", DXErrorProvider.ErrorType.Critical)
        End If
        If txtTotal.EditValue < 0 Then
            DxErrorProvider1.SetError(txtTotal, "Total PemReturJualan salah!", DXErrorProvider.ErrorType.Critical)
        End If
        Return Not DxErrorProvider1.HasErrors
    End Function

    Private Sub txtTypePajak_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTypePajak.EditValueChanged

    End Sub

    Private Sub txtTypePajak_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTypePajak.LostFocus
        If (pStatus = pStatusForm.Edit OrElse pStatus = pStatusForm.TempInsert) AndAlso IDTypePajak <> NullTolInt(txtTypePajak.EditValue) Then
            UpdateTypePajak()
        End If
    End Sub
End Class
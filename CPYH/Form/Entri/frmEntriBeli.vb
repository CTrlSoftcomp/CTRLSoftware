Imports CtrlSoft.Utils
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors
Imports System.Data
Imports System.Data.SqlClient

Public Class frmEntriBeli
    Private NoID As Long = -1
    Private pStatus As pStatusForm
    Private IDTypePajak As Integer = -1

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

                            com.CommandText = "SELECT NoID, Kode, Nama, Alamat FROM MAlamat WHERE IsActive=1 AND IsSupplier=1"
                            oDA.Fill(ds, "MSupplier")
                            txtSupplier.Properties.DataSource = ds.Tables("MSupplier")
                            txtSupplier.Properties.ValueMember = "NoID"
                            txtSupplier.Properties.DisplayMember = "Kode"

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

                            com.CommandText = "SELECT TOP 1 * FROM MBeli WHERE NoID=" & NoID
                            oDA.Fill(ds, "MBeli")

                            If ds.Tables("MBeli").Rows.Count >= 1 Then
                                Dim iRow As DataRow = ds.Tables("MBeli").Rows(0)
                                If NullToBool(iRow.Item("IsPosted")) Then
                                    pStatus = pStatusForm.Posted
                                Else
                                    pStatus = pStatusForm.Edit
                                End If
                                txtTanggal.EditValue = NullToDate(iRow.Item("Tanggal"))
                                txtKode.Text = NullToStr(iRow.Item("Kode"))
                                txtExpired.EditValue = NullToDate(iRow.Item("Expired"))
                                txtSupplier.EditValue = -1
                                txtSupplier.EditValue = NullToLong(iRow.Item("IDSupplier"))
                                txtTypePajak.EditValue = NullToLong(iRow.Item("IDTypePajak"))
                                IDTypePajak = txtTypePajak.EditValue
                                txtNoReff.Text = NullToStr(iRow.Item("NoReff"))
                                txtCatatan.Text = NullToStr(iRow.Item("Catatan"))
                            Else
                                txtTanggal.EditValue = Now
                                txtSupplier.EditValue = -1
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

                                com.CommandText = "SELECT MBeliD.*, MSatuan.Kode AS Satuan, MBarangD.Barcode, MBarang.Kode KodeBarang, MBarang.Nama NamaBarang" & vbCrLf & _
                                                  "FROM MBeliD" & vbCrLf & _
                                                  "LEFT JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang" & vbCrLf & _
                                                  "LEFT JOIN MBarangD ON MBarangD.NoID=MBeliD.IDBarangD" & vbCrLf & _
                                                  "LEFT JOIN MSatuan ON MSatuan.NoID=MBeliD.IDSatuan" & vbCrLf & _
                                                  "WHERE MBeliD.IDHeader=" & NoID
                                oDA.Fill(ds, "MBeliD")
                                GridControl1.DataSource = ds.Tables("MBeliD")
                                GridView1.RefreshData()

                                GridView1.ClearSelection()
                                GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), IDDetil.ToString("n0"))
                                GridView1.SelectRow(GridView1.FocusedRowHandle)

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

                                com.CommandText = "SELECT SUM(MBeliD.JumlahBruto) JumlahBruto, SUM(MBeliD.Jumlah) Jumlah, SUM(MBeliD.DPP) DPP, SUM(MBeliD.PPN) PPN" & vbCrLf & _
                                                  "FROM MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDHeader" & vbCrLf & _
                                                  "WHERE MBeli.NoID=" & NoID & vbCrLf & _
                                                  "GROUP BY MBeli.NoID, MBeli.IDTypePajak"
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
                                    com.CommandText = "SELECT IsPosted FROM MBeli WHERE NoID=" & NoID
                                    If NullToBool(com.ExecuteScalar()) Then
                                        XtraMessageBox.Show("Nota telah diposting!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                        Exit Try
                                    End If
                                ElseIf pStatus = pStatusForm.Posted Then
                                    XtraMessageBox.Show("Nota telah diposting!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    Exit Try
                                End If

                                com.CommandText = "SELECT COUNT(Kode) FROM MBeli WHERE Kode=@Kode AND NoID<>@NoID"
                                com.Parameters.Clear()
                                com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NoID
                                com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = txtKode.Text
                                If NullToLong(com.ExecuteScalar()) >= 1 Then
                                    XtraMessageBox.Show("Nota telah sudah ada!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    Exit Try
                                End If

                                com.CommandText = "EXEC [dbo].[spSimpanMBeli] @NoID,@Kode,@NoReff,@IDSupplier,@Tanggal,@Expired,@Catatan,@IDTypePajak,@Subtotal," & vbCrLf & _
                                                  "@DiscNotaProsen,@DiscNotaRp,@TotalBruto,@DPP,@PPN,@Total,@IsPosted,@TglPosted,@IDUserPosted,@IDUserEntry,@IDUserEdit,@IsExpired"
                                com.Parameters.Clear()
                                com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NoID
                                com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = txtKode.Text
                                com.Parameters.Add(New SqlParameter("@NoReff", SqlDbType.VarChar)).Value = txtNoReff.Text
                                com.Parameters.Add(New SqlParameter("@IDSupplier", SqlDbType.Int)).Value = NullTolInt(txtSupplier.EditValue)
                                com.Parameters.Add(New SqlParameter("@Tanggal", SqlDbType.DateTime)).Value = NullToDate(txtTanggal.EditValue)
                                com.Parameters.Add(New SqlParameter("@Expired", SqlDbType.Date)).Value = NullToDate(txtExpired.EditValue)
                                com.Parameters.Add(New SqlParameter("@Catatan", SqlDbType.VarChar)).Value = txtCatatan.Text
                                com.Parameters.Add(New SqlParameter("@IDTypePajak", SqlDbType.SmallInt)).Value = NullTolInt(txtTypePajak.EditValue)
                                com.Parameters.Add(New SqlParameter("@Subtotal", SqlDbType.Money)).Value = NullToDbl(txtSubtotal.EditValue)
                                com.Parameters.Add(New SqlParameter("@DiscNotaProsen", SqlDbType.Float)).Value = 0.0
                                com.Parameters.Add(New SqlParameter("@DiscNotaRp", SqlDbType.Float)).Value = 0.0
                                com.Parameters.Add(New SqlParameter("@TotalBruto", SqlDbType.Money)).Value = NullToDbl(txtSubtotal.EditValue)
                                com.Parameters.Add(New SqlParameter("@DPP", SqlDbType.Money)).Value = NullToDbl(txtDPP.EditValue)
                                com.Parameters.Add(New SqlParameter("@PPN", SqlDbType.Money)).Value = NullToDbl(txtPPN.EditValue)
                                com.Parameters.Add(New SqlParameter("@Total", SqlDbType.Money)).Value = NullToDbl(txtTotal.EditValue)
                                com.Parameters.Add(New SqlParameter("@IsPosted", SqlDbType.Bit)).Value = False
                                com.Parameters.Add(New SqlParameter("@TglPosted", SqlDbType.DateTime)).Value = System.DBNull.Value
                                com.Parameters.Add(New SqlParameter("@IDUserPosted", SqlDbType.Int)).Value = System.DBNull.Value
                                com.Parameters.Add(New SqlParameter("@IDUserEntry", SqlDbType.Int)).Value = UserLogin.NoID
                                com.Parameters.Add(New SqlParameter("@IDUserEdit", SqlDbType.Int)).Value = IIf(pStatus = pStatusForm.Edit, UserLogin.NoID, -1)
                                com.Parameters.Add(New SqlParameter("@IsExpired", SqlDbType.Bit)).Value = False
                                NoID = NullToLong(com.ExecuteScalar())
                                com.Parameters.Clear()

                                com.CommandText = "UPDATE MBeli SET Subtotal=ISNULL(MBeliD.JumlahBruto, 0), TotalBruto=ISNULL(MBeliD.JumlahBruto, 0), " & vbCrLf & _
                                                  "PPN=ROUND((CASE WHEN MBeli.IDTypePajak=0 THEN 0 ELSE 0.1 END)*ISNULL(MBeliD.JumlahBruto, 0), 0), " & vbCrLf & _
                                                  "DPP=ROUND(CASE WHEN MBeli.IDTypePajak=0 THEN 0 WHEN MBeli.IDTypePajak=1 THEN ISNULL(MBeliD.JumlahBruto, 0)/1.0 ELSE ISNULL(MBeliD.JumlahBruto, 0)/1.1 END, 0) " & vbCrLf & _
                                                  "FROM MBeli " & vbCrLf & _
                                                  "INNER JOIN (SELECT IDHeader, SUM(JumlahBruto) AS JumlahBruto FROM MBeliD GROUP BY IDHeader) AS MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                                  "WHERE MBeli.NoID=" & NoID
                                com.ExecuteNonQuery()

                                com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                                  "PPN=ROUND((CASE WHEN MBeli.IDTypePajak=0 THEN 0 ELSE 0.1 END)*ISNULL(MBeliD.JumlahBruto, 0), 0), " & vbCrLf & _
                                                  "DPP=ROUND(CASE WHEN MBeli.IDTypePajak=0 THEN 0 WHEN MBeli.IDTypePajak=1 THEN ISNULL(MBeliD.JumlahBruto, 0)/1.0 ELSE ISNULL(MBeliD.JumlahBruto, 0)/1.1 END, 0) " & vbCrLf & _
                                                  "FROM MBeli " & vbCrLf & _
                                                  "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                                  "WHERE MBeliD.IDHeader=" & NoID
                                com.ExecuteNonQuery()

                                com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                                  "PPN=ISNULL(MBeli.PPN, 0)-(ISNULL(Detil.PPN, 0)-ISNULL(MBeliD.PPN, 0)), " & vbCrLf & _
                                                  "DPP=ISNULL(MBeli.DPP, 0)-(ISNULL(Detil.DPP, 0)-ISNULL(MBeliD.DPP, 0)) " & vbCrLf & _
                                                  "FROM MBeli " & vbCrLf & _
                                                  "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID" & vbCrLf & _
                                                  "INNER JOIN (SELECT IDHeader, SUM(DPP) AS DPP, SUM(PPN) AS PPN, MAX(NoID) AS NoID FROM MBeliD GROUP BY IDHeader) AS Detil ON Detil.IDHeader=MBeli.NoID AND Detil.NoID=MBeliD.NoID" & vbCrLf & _
                                                  "WHERE MBeliD.IDHeader=" & NoID
                                com.ExecuteNonQuery()

                                com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                                  "Jumlah=CASE WHEN MBeli.IDTypePajak=0 THEN MBeliD.JumlahBruto ELSE MBeliD.DPP+MBeliD.PPN END " & vbCrLf & _
                                                  "FROM MBeli " & vbCrLf & _
                                                  "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                                  "WHERE MBeliD.IDHeader=" & NoID
                                com.ExecuteNonQuery()

                                com.CommandText = "UPDATE MBeli SET Subtotal=ISNULL(MBeliD.JumlahBruto, 0), TotalBruto=ISNULL(MBeliD.JumlahBruto, 0), Total=ISNULL(MBeliD.Jumlah, 0)" & vbCrLf & _
                                                  "FROM MBeli " & vbCrLf & _
                                                  "INNER JOIN (SELECT IDHeader, SUM(JumlahBruto) AS JumlahBruto, SUM(Jumlah) AS Jumlah FROM MBeliD GROUP BY IDHeader) AS MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                                  "WHERE MBeli.NoID=" & NoID
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

                            com.CommandText = "SELECT IsPosted FROM MBeli WHERE NoID=" & NoID
                            If NullToBool(com.ExecuteScalar()) Then
                                XtraMessageBox.Show("Nota telah diposting!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                Exit Try
                            End If

                            com.CommandText = "UPDATE MBeli SET IDTypePajak=" & NullToLong(txtTypePajak.EditValue) & " WHERE NoID=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeli SET Subtotal=ISNULL(MBeliD.JumlahBruto, 0), TotalBruto=ISNULL(MBeliD.JumlahBruto, 0), " & vbCrLf & _
                                                  "PPN=ROUND((CASE WHEN MBeli.IDTypePajak=0 THEN 0 ELSE 0.1 END)*ISNULL(MBeliD.JumlahBruto, 0), 0), " & vbCrLf & _
                                                  "DPP=ROUND(CASE WHEN MBeli.IDTypePajak=0 THEN 0 WHEN MBeli.IDTypePajak=1 THEN ISNULL(MBeliD.JumlahBruto, 0)/1.0 ELSE ISNULL(MBeliD.JumlahBruto, 0)/1.1 END, 0) " & vbCrLf & _
                                                  "FROM MBeli " & vbCrLf & _
                                                  "INNER JOIN (SELECT IDHeader, SUM(JumlahBruto) AS JumlahBruto FROM MBeliD GROUP BY IDHeader) AS MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                                  "WHERE MBeli.NoID=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                              "PPN=ROUND((CASE WHEN MBeli.IDTypePajak=0 THEN 0 ELSE 0.1 END)*ISNULL(MBeliD.JumlahBruto, 0), 0), " & vbCrLf & _
                                              "DPP=ROUND(CASE WHEN MBeli.IDTypePajak=0 THEN 0 WHEN MBeli.IDTypePajak=1 THEN ISNULL(MBeliD.JumlahBruto, 0)/1.0 ELSE ISNULL(MBeliD.JumlahBruto, 0)/1.1 END, 0) " & vbCrLf & _
                                              "FROM MBeli " & vbCrLf & _
                                              "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                              "WHERE MBeliD.IDHeader=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                              "PPN=ISNULL(MBeli.PPN, 0)-(ISNULL(Detil.PPN, 0)-ISNULL(MBeliD.PPN, 0)), " & vbCrLf & _
                                              "DPP=ISNULL(MBeli.DPP, 0)-(ISNULL(Detil.DPP, 0)-ISNULL(MBeliD.DPP, 0)) " & vbCrLf & _
                                              "FROM MBeli " & vbCrLf & _
                                              "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID" & vbCrLf & _
                                              "INNER JOIN (SELECT IDHeader, SUM(DPP) AS DPP, SUM(PPN) AS PPN, MAX(NoID) AS NoID FROM MBeliD GROUP BY IDHeader) AS Detil ON Detil.IDHeader=MBeli.NoID AND Detil.NoID=MBeliD.NoID" & vbCrLf & _
                                              "WHERE MBeliD.IDHeader=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                              "Jumlah=CASE WHEN MBeli.IDTypePajak=0 THEN MBeliD.JumlahBruto ELSE MBeliD.DPP+MBeliD.PPN END " & vbCrLf & _
                                              "FROM MBeli " & vbCrLf & _
                                              "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                              "WHERE MBeliD.IDHeader=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeli SET Subtotal=ISNULL(MBeliD.JumlahBruto, 0), TotalBruto=ISNULL(MBeliD.JumlahBruto, 0), Total=ISNULL(MBeliD.Jumlah, 0)" & vbCrLf & _
                                              "FROM MBeli " & vbCrLf & _
                                              "INNER JOIN (SELECT IDHeader, SUM(JumlahBruto) AS JumlahBruto, SUM(Jumlah) AS Jumlah FROM MBeliD GROUP BY IDHeader) AS MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                              "WHERE MBeli.NoID=" & NoID
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

                            com.CommandText = "SELECT IsPosted FROM MBeli WHERE NoID=" & NoID
                            If NullToBool(com.ExecuteScalar()) Then
                                XtraMessageBox.Show("Nota telah diposting!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                Exit Try
                            End If

                            com.CommandText = "DELETE FROM MBeliD WHERE NoID=" & IDDetil
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeli SET IDTypePajak=" & NullToLong(txtTypePajak.EditValue) & " WHERE NoID=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeli SET Subtotal=ISNULL(MBeliD.JumlahBruto, 0), TotalBruto=ISNULL(MBeliD.JumlahBruto, 0), " & vbCrLf & _
                                              "PPN=ROUND((CASE WHEN MBeli.IDTypePajak=0 THEN 0 ELSE 0.1 END)*ISNULL(MBeliD.JumlahBruto, 0), 0), " & vbCrLf & _
                                              "DPP=ROUND(CASE WHEN MBeli.IDTypePajak=0 THEN 0 WHEN MBeli.IDTypePajak=1 THEN ISNULL(MBeliD.JumlahBruto, 0)/1.0 ELSE ISNULL(MBeliD.JumlahBruto, 0)/1.1 END, 0) " & vbCrLf & _
                                              "FROM MBeli " & vbCrLf & _
                                              "INNER JOIN (SELECT IDHeader, SUM(JumlahBruto) AS JumlahBruto FROM MBeliD GROUP BY IDHeader) AS MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                              "WHERE MBeli.NoID=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                              "PPN=ROUND((CASE WHEN MBeli.IDTypePajak=0 THEN 0 ELSE 0.1 END)*ISNULL(MBeliD.JumlahBruto, 0), 0), " & vbCrLf & _
                                              "DPP=ROUND(CASE WHEN MBeli.IDTypePajak=0 THEN 0 WHEN MBeli.IDTypePajak=1 THEN ISNULL(MBeliD.JumlahBruto, 0)/1.0 ELSE ISNULL(MBeliD.JumlahBruto, 0)/1.1 END, 0) " & vbCrLf & _
                                              "FROM MBeli " & vbCrLf & _
                                              "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                              "WHERE MBeliD.IDHeader=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                              "PPN=ISNULL(MBeli.PPN, 0)-(ISNULL(Detil.PPN, 0)-ISNULL(MBeliD.PPN, 0)), " & vbCrLf & _
                                              "DPP=ISNULL(MBeli.DPP, 0)-(ISNULL(Detil.DPP, 0)-ISNULL(MBeliD.DPP, 0)) " & vbCrLf & _
                                              "FROM MBeli " & vbCrLf & _
                                              "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID" & vbCrLf & _
                                              "INNER JOIN (SELECT IDHeader, SUM(DPP) AS DPP, SUM(PPN) AS PPN, MAX(NoID) AS NoID FROM MBeliD GROUP BY IDHeader) AS Detil ON Detil.IDHeader=MBeli.NoID AND Detil.NoID=MBeliD.NoID" & vbCrLf & _
                                              "WHERE MBeliD.IDHeader=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                              "Jumlah=CASE WHEN MBeli.IDTypePajak=0 THEN MBeliD.JumlahBruto ELSE MBeliD.DPP+MBeliD.PPN END " & vbCrLf & _
                                              "FROM MBeli " & vbCrLf & _
                                              "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                              "WHERE MBeliD.IDHeader=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeli SET Subtotal=ISNULL(MBeliD.JumlahBruto, 0), TotalBruto=ISNULL(MBeliD.JumlahBruto, 0), Total=ISNULL(MBeliD.Jumlah, 0)" & vbCrLf & _
                                              "FROM MBeli " & vbCrLf & _
                                              "INNER JOIN (SELECT IDHeader, SUM(JumlahBruto) AS JumlahBruto, SUM(Jumlah) AS Jumlah FROM MBeliD GROUP BY IDHeader) AS MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                              "WHERE MBeli.NoID=" & NoID
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
                If System.IO.File.Exists(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
                    LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub txtSupplier_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSupplier.EditValueChanged
        Using cn As New SqlConnection(StrKonSQL)
            Using com As New SqlCommand
                Using oDA As New SqlDataAdapter
                    Using ds As New DataSet
                        Try
                            cn.Open()
                            com.Connection = cn
                            com.CommandTimeout = cn.ConnectionTimeout
                            oDA.SelectCommand = com

                            com.CommandText = "SELECT NoID, Kode, Nama, Alamat FROM MAlamat WHERE NoID=" & NullToLong(txtSupplier.EditValue)
                            oDA.Fill(ds, "MSupplier")
                            If ds.Tables("MSupplier").Rows.Count >= 1 Then
                                txtNamaSupplier.Text = NullToStr(ds.Tables("MSupplier").Rows(0).Item("Nama"))
                                txtAlamatSupplier.Text = NullToStr(ds.Tables("MSupplier").Rows(0).Item("Alamat"))
                            Else
                                txtNamaSupplier.Text = ""
                                txtAlamatSupplier.Text = ""
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
                txtKode.Text = "AUTO" 'Repository.RepKode.GetNewKode("MBeli", "Kode", "PO-" & NullToDate(txtTanggal.EditValue).ToString("yyMM") & "-", "", Repository.RepKode.Format.A00000)
                txtExpired.EditValue = DateAdd(DateInterval.Day, 30, txtTanggal.EditValue)
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub gv_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataSourceChanged, gvSupplier.DataSourceChanged, gvTypePajak.DataSourceChanged
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
            Using frm As New frmEntriBeliD(Me, NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")), NoID, txtTypePajak.EditValue)
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
        If IIf(pStatus = pStatusForm.Baru, SimpanData(), True) = True Then
            Using frm As New frmEntriBeliD(Me, -1, NoID, txtTypePajak.EditValue)
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
                    LayoutControl1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                    gvSupplier.SaveLayoutToXml(FolderLayouts & Me.Name & gvSupplier.Name & ".xml")
                    gvTypePajak.SaveLayoutToXml(FolderLayouts & Me.Name & gvTypePajak.Name & ".xml")
                    GridView1.SaveLayoutToXml(FolderLayouts & Me.Name & GridView1.Name & ".xml")
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
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = modMain.FormName.DaftarPO.ToString Then
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
        If txtSupplier.Text = "" Then
            DxErrorProvider1.SetError(txtSupplier, "Supplier harus dipilih!", DXErrorProvider.ErrorType.Critical)
        End If
        If DateDiff(DateInterval.Day, txtTanggal.EditValue, txtExpired.EditValue) < 0 Then
            DxErrorProvider1.SetError(txtExpired, "Expired Pesanan salah!", DXErrorProvider.ErrorType.Critical)
        End If
        If txtTotal.EditValue < 0 Then
            DxErrorProvider1.SetError(txtTotal, "Total Peanan salah!", DXErrorProvider.ErrorType.Critical)
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
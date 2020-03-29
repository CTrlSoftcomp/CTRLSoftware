Imports CtrlSoft.Utils
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors
Imports System.Data
Imports System.Data.SqlClient

Namespace Repository
    Public Class UnPostingData
        Public Shared Function UnPostingPO(ByVal NoID As Long) As Boolean
            Dim Hasil As Boolean = False
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using ds As New DataSet
                            Try
                                cn.Open()
                                com.Connection = cn
                                com.CommandTimeout = cn.ConnectionTimeout
                                com.Transaction = com.Connection.BeginTransaction
                                oDA.SelectCommand = com

                                com.CommandText = "SELECT COUNT(MBeliD.NoID) FROM MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDHeader INNER JOIN MPOD ON MPOD.NoID=MBeliD.IDPOD WHERE MPOD.IDHeader=" & NoID
                                If NullToLong(com.ExecuteScalar()) = 0 Then
                                    com.CommandText = "UPDATE MPO SET IsPosted=0, TglPosted=NULL, IDUserPosted=NULL WHERE NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
                                    Hasil = True
                                End If
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return Hasil
        End Function
        Public Shared Function UnPostingBeli(ByVal NoID As Long) As Boolean
            Dim Hasil As Boolean = False
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using ds As New DataSet
                            Try
                                cn.Open()
                                com.Connection = cn
                                com.CommandTimeout = cn.ConnectionTimeout
                                com.Transaction = com.Connection.BeginTransaction
                                oDA.SelectCommand = com

                                com.CommandText = "SELECT MBeliD.*, MBeli.Total, MBeli.Tanggal, MBeli.Kode AS KdTransaksi, MBeli.IDSupplier, MBeli.DPP, MBeli.PPN, MAlamat.LimitHutang" & vbCrLf & _
                                                  "FROM MBeliD" & vbCrLf & _
                                                  "INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDHeader" & vbCrLf & _
                                                  "INNER JOIN MAlamat ON MAlamat.NoID=MBeli.IDSupplier" & vbCrLf & _
                                                  "WHERE ISNULL(MBeli.Total,0)>0 AND ISNULL(MBeli.IsPosted,0)=1 AND MBeli.NoID=" & NoID
                                oDA.Fill(ds, "MBeli")
                                If ds.Tables("MBeli").Rows.Count >= 1 Then
                                    com.CommandText = "SELECT COUNT(MReturBeliD.NoID) FROM MReturBeliD INNER JOIN MReturBeli ON MReturBeli.NoID=MReturBeliD.IDHeader INNER JOIN MBeliD ON MBeliD.NoID=MReturBeliD.IDBeliD WHERE MBeliD.IDHeader=" & NoID
                                    If NullToLong(com.ExecuteScalar()) <> 0 Then
                                        XtraMessageBox.Show("Pembelian sudah pernah diretur!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                        Return False
                                    End If
                                    com.CommandText = "SELECT COUNT(NoID) FROM MHutangPiutang WHERE ReffNoTransaksi='" & FixApostropi(ds.Tables("MBeli").Rows(0).Item("KdTransaksi")) & "'"
                                    If NullToLong(com.ExecuteScalar()) <> 0 Then
                                        XtraMessageBox.Show("Pembelian sudah dibayar!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                        Return False
                                    End If

                                    com.CommandText = "UPDATE MBeli SET IsPosted=0, TglPosted=NULL, IDUserPosted=NULL WHERE ISNULL(IsPosted, 0)=1 AND NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi=2 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MHutangPiutang] WHERE IDJenisTransaksi=2 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
                                    Hasil = True
                                End If
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return Hasil
        End Function
        Public Shared Function UnPostingReturBeli(ByVal NoID As Long) As Boolean
            Dim Hasil As Boolean = False
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using ds As New DataSet
                            Try
                                cn.Open()
                                com.Connection = cn
                                com.CommandTimeout = cn.ConnectionTimeout
                                com.Transaction = com.Connection.BeginTransaction
                                oDA.SelectCommand = com

                                com.CommandText = "SELECT MReturBeliD.*, MReturBeli.Total, MReturBeli.Tanggal, MReturBeli.Kode AS KdTransaksi, MReturBeli.IDSupplier, MReturBeli.DPP, MReturBeli.PPN, MAlamat.LimitHutang" & vbCrLf & _
                                                  "FROM MReturBeliD" & vbCrLf & _
                                                  "INNER JOIN MReturBeli ON MReturBeli.NoID=MReturBeliD.IDHeader" & vbCrLf & _
                                                  "INNER JOIN MAlamat ON MAlamat.NoID=MReturBeli.IDSupplier" & vbCrLf & _
                                                  "WHERE ISNULL(MReturBeli.Total,0)>0 AND ISNULL(MReturBeli.IsPosted,0)=1 AND MReturBeli.NoID=" & NoID
                                oDA.Fill(ds, "MReturBeli")
                                If ds.Tables("MReturBeli").Rows.Count >= 1 Then
                                    com.CommandText = "SELECT COUNT(NoID) FROM MHutangPiutang WHERE ReffNoTransaksi='" & FixApostropi(ds.Tables("MReturBeli").Rows(0).Item("KdTransaksi")) & "'"
                                    If NullToLong(com.ExecuteScalar()) <> 0 Then
                                        XtraMessageBox.Show("Returan Barang sudah dibayar!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                        Return False
                                    End If

                                    com.CommandText = "UPDATE MReturBeli SET IsPosted=0, TglPosted=NULL, IDUserPosted=NULL WHERE ISNULL(IsPosted, 0)=1 AND NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi=3 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MHutangPiutang] WHERE IDJenisTransaksi=3 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
                                    Hasil = True
                                End If
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return Hasil
        End Function
        Public Shared Function UnPostingJual(ByVal NoID As Long) As Boolean
            Dim Hasil As Boolean = False
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using ds As New DataSet
                            Try
                                cn.Open()
                                com.Connection = cn
                                com.CommandTimeout = cn.ConnectionTimeout
                                com.Transaction = com.Connection.BeginTransaction
                                oDA.SelectCommand = com

                                com.CommandText = "SELECT MJualD.*, MJual.Total, MJual.Tanggal, MJual.Kode AS KdTransaksi, MJual.IDCustomer, MJual.DPP, MJual.PPN, MAlamat.LimitHutang" & vbCrLf & _
                                                  "FROM MJualD" & vbCrLf & _
                                                  "INNER JOIN MJual ON MJual.NoID=MJualD.IDHeader" & vbCrLf & _
                                                  "INNER JOIN MAlamat ON MAlamat.NoID=MJual.IDCustomer" & vbCrLf & _
                                                  "WHERE ISNULL(MJual.Total,0)>0 AND ISNULL(MJual.IsPosted,0)=1 AND MJual.NoID=" & NoID
                                oDA.Fill(ds, "MJual")
                                If ds.Tables("MJual").Rows.Count >= 1 Then
                                    com.CommandText = "SELECT COUNT(NoID) FROM MHutangPiutang WHERE ReffNoTransaksi='" & FixApostropi(ds.Tables("MJual").Rows(0).Item("KdTransaksi")) & "'"
                                    If NullToLong(com.ExecuteScalar()) <> 0 Then
                                        XtraMessageBox.Show("Penjualan Barang sudah dibayar!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                        Return False
                                    End If

                                    com.CommandText = "UPDATE MJual SET IsPosted=0, TglPosted=NULL, IDUserPosted=NULL WHERE ISNULL(IsPosted, 0)=1 AND NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi=6 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MHutangPiutang] WHERE IDJenisTransaksi=6 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
                                    Hasil = True
                                End If
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return Hasil
        End Function
        Public Shared Function UnPostingReturJual(ByVal NoID As Long) As Boolean
            Dim Hasil As Boolean = False
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using ds As New DataSet
                            Try
                                cn.Open()
                                com.Connection = cn
                                com.CommandTimeout = cn.ConnectionTimeout
                                com.Transaction = com.Connection.BeginTransaction
                                oDA.SelectCommand = com

                                com.CommandText = "SELECT MReturJualD.*, MReturJual.Total, MReturJual.Tanggal, MReturJual.Kode AS KdTransaksi, MReturJual.IDCustomer, MReturJual.DPP, MReturJual.PPN, MAlamat.LimitHutang" & vbCrLf & _
                                                  "FROM MReturJualD" & vbCrLf & _
                                                  "INNER JOIN MReturJual ON MReturJual.NoID=MReturJualD.IDHeader" & vbCrLf & _
                                                  "INNER JOIN MAlamat ON MAlamat.NoID=MReturJual.IDCustomer" & vbCrLf & _
                                                  "WHERE ISNULL(MReturJual.Total,0)>0 AND ISNULL(MReturJual.IsPosted,0)=1 AND MReturJual.NoID=" & NoID
                                oDA.Fill(ds, "MReturJual")
                                If ds.Tables("MReturJual").Rows.Count >= 1 Then
                                    com.CommandText = "SELECT COUNT(NoID) FROM MHutangPiutang WHERE ReffNoTransaksi='" & FixApostropi(ds.Tables("MReturJual").Rows(0).Item("KdTransaksi")) & "'"
                                    If NullToLong(com.ExecuteScalar()) <> 0 Then
                                        XtraMessageBox.Show("PenReturJualan Barang sudah dibayar!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                        Return False
                                    End If

                                    com.CommandText = "UPDATE MReturJual SET IsPosted=0, TglPosted=NULL, IDUserPosted=NULL WHERE ISNULL(IsPosted, 0)=1 AND NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi=7 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MHutangPiutang] WHERE IDJenisTransaksi=7 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
                                    Hasil = True
                                End If
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return Hasil
        End Function
        Public Shared Function UnPostingMutasiGudang(ByVal NoID As Long) As Boolean
            Dim Hasil As Boolean = False
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using ds As New DataSet
                            Try
                                cn.Open()
                                com.Connection = cn
                                com.CommandTimeout = cn.ConnectionTimeout
                                com.Transaction = com.Connection.BeginTransaction
                                oDA.SelectCommand = com

                                com.CommandText = "SELECT MMutasiGudangD.*, MMutasiGudang.Total, MMutasiGudang.Tanggal, MMutasiGudang.Kode AS KdTransaksi" & vbCrLf & _
                                                  "FROM MMutasiGudangD" & vbCrLf & _
                                                  "INNER JOIN MMutasiGudang ON MMutasiGudang.NoID=MMutasiGudangD.IDHeader" & vbCrLf & _
                                                  "WHERE ISNULL(MMutasiGudang.IsPosted,0)=1 AND MMutasiGudang.NoID=" & NoID
                                oDA.Fill(ds, "MMutasiGudang")
                                If ds.Tables("MMutasiGudang").Rows.Count >= 1 Then
                                    com.CommandText = "UPDATE MMutasiGudang SET IsPosted=0, TglPosted=NULL, IDUserPosted=NULL WHERE ISNULL(IsPosted, 0)=1 AND NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi IN (4, 5) AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
                                    Hasil = True
                                End If
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return Hasil
        End Function
        Public Shared Function UnPostingPemakaian(ByVal NoID As Long) As Boolean
            Dim Hasil As Boolean = False
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using ds As New DataSet
                            Try
                                cn.Open()
                                com.Connection = cn
                                com.CommandTimeout = cn.ConnectionTimeout
                                com.Transaction = com.Connection.BeginTransaction
                                oDA.SelectCommand = com

                                com.CommandText = "SELECT MPemakaianD.*, MPemakaian.Total, MPemakaian.Tanggal, MPemakaian.Kode AS KdTransaksi" & vbCrLf & _
                                                  "FROM MPemakaianD" & vbCrLf & _
                                                  "INNER JOIN MPemakaian ON MPemakaian.NoID=MPemakaianD.IDHeader" & vbCrLf & _
                                                  "WHERE ISNULL(MPemakaian.IsPosted,0)=1 AND MPemakaian.NoID=" & NoID
                                oDA.Fill(ds, "MPemakaian")
                                If ds.Tables("MPemakaian").Rows.Count >= 1 Then
                                    com.CommandText = "UPDATE MPemakaian SET IsPosted=0, TglPosted=NULL, IDUserPosted=NULL WHERE ISNULL(IsPosted, 0)=1 AND NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi = 8 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
                                    Hasil = True
                                End If
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return Hasil
        End Function
        Public Shared Function UnPostingPenyesuaian(ByVal NoID As Long) As Boolean
            Dim Hasil As Boolean = False
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using ds As New DataSet
                            Try
                                cn.Open()
                                com.Connection = cn
                                com.CommandTimeout = cn.ConnectionTimeout
                                com.Transaction = com.Connection.BeginTransaction
                                oDA.SelectCommand = com

                                com.CommandText = "SELECT MPenyesuaianD.*, MPenyesuaian.Total, MPenyesuaian.Tanggal, MPenyesuaian.Kode AS KdTransaksi" & vbCrLf & _
                                                  "FROM MPenyesuaianD" & vbCrLf & _
                                                  "INNER JOIN MPenyesuaian ON MPenyesuaian.NoID=MPenyesuaianD.IDHeader" & vbCrLf & _
                                                  "WHERE ISNULL(MPenyesuaian.IsPosted,0)=1 AND MPenyesuaian.NoID=" & NoID
                                oDA.Fill(ds, "MPenyesuaian")
                                If ds.Tables("MPenyesuaian").Rows.Count >= 1 Then
                                    com.CommandText = "UPDATE MPenyesuaian SET IsPosted=0, TglPosted=NULL, IDUserPosted=NULL WHERE ISNULL(IsPosted, 0)=1 AND NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi = 14 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
                                    Hasil = True
                                End If
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return Hasil
        End Function
    End Class
End Namespace

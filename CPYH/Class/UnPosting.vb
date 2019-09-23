﻿Imports CtrlSoft.Utils
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
    End Class
End Namespace
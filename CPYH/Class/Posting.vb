Imports CtrlSoft.Utils
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors
Imports System.Data
Imports System.Data.SqlClient

Namespace Repository
    Public Class PostingData
        Public Shared Function PostingPO(ByVal NoID As Long) As Boolean
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

                                com.CommandText = "UPDATE MPO SET IsPosted=1, TglPosted=GETDATE(), IDUserPosted=" & UserLogin.NoID & " WHERE ISNULL(IsPosted, 0)=0 AND NoID=" & NoID
                                If NullToLong(com.ExecuteNonQuery()) >= 1 Then
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
        Public Shared Function PostingBeli(ByVal NoID As Long) As Boolean
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
                                                  "WHERE ISNULL(MBeli.Total,0)>0 AND ISNULL(MBeli.IsPosted,0)=0 AND MBeli.NoID=" & NoID
                                oDA.Fill(ds, "MBeli")
                                If ds.Tables("MBeli").Rows.Count >= 1 Then
                                    Dim Limit As Double = 0.0
                                    com.CommandText = "SELECT SUM(Kredit-Debet) AS Saldo FROM MHutangPiutang WHERE CONVERT(DATE, Tanggal)<=CONVERT(DATE, '" & NullToDate(ds.Tables("MBeli").Rows(0).Item("Tanggal")).ToString("yyyy-MM-dd") & "') AND IDAlamat=" & NullToLong(ds.Tables("MBeli").Rows(0).Item("IDSupplier"))
                                    Limit = NullToDbl(ds.Tables("MBeli").Rows(0).Item("LimitHutang")) - NullToDbl(com.ExecuteScalar()) - NullToDbl(ds.Tables("MBeli").Rows(0).Item("Total"))
                                    If Limit < 0 Then
                                        XtraMessageBox.Show("Limit Hutang tidak mencukupi! Setting dulu di master Supplier.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                        Return False
                                    End If

                                    com.CommandText = "UPDATE MBeli SET IsPosted=1, TglPosted=GETDATE(), IDUserPosted=" & UserLogin.NoID & " WHERE ISNULL(IsPosted, 0)=0 AND NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi=2 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "INSERT INTO [dbo].[MKartuStok] (" & vbCrLf & _
                                                      "[IDBarang],[IDBarangD],[Varian],[IDJenisTransaksi],[IDTransaksi],[IDTransaksiD],[IDGudang]" & vbCrLf & _
                                                      ",[IDSatuan],[Konversi],[QtyMasuk],[QtyKeluar],[Debet],[Kredit],[HargaBeli],[HPP],[SaldoAkhir]" & vbCrLf & _
                                                      ",[NilaiAkhir])" & vbCrLf & _
                                                      "SELECT MBeliD.[IDBarang],MBeliD.[IDBarangD],''[Varian],2 [IDJenisTransaksi],MBeli.NoID [IDTransaksi],MBeliD.NoID [IDTransaksiD],MBeli.IDGudang [IDGudang]" & vbCrLf & _
                                                      ",MBeliD.IDSatuan [IDSatuan],MBeliD.Konversi [Konversi],MBeliD.Qty [QtyMasuk],0 [QtyKeluar],0 [Debet],0 [Kredit],0 [HargaBeli],0 [HPP],0 [SaldoAkhir],0 [NilaiAkhir]" & vbCrLf & _
                                                      "FROM MBeliD" & vbCrLf & _
                                                      "INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDHeader" & vbCrLf & _
                                                      "WHERE MBeli.NoID=" & NullToLong(NoID)
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MHutangPiutang] WHERE IDJenisTransaksi=2 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "INSERT INTO [dbo].[MHutangPiutang] (" & vbCrLf & _
                                                      "[IDAlamat],[IDTransaksi],[IDJenisTransaksi],[NoTransaksi],[NoUrut],[Tanggal],[JatuhTempo],[Debet]" & vbCrLf & _
                                                      ",[Kredit],[Saldo],[ReffNoTransaksi],[ReffNoUrut])" & vbCrLf & _
                                                      "SELECT MBeli.IDSupplier, MBeli.NoID, 2 [IDJenisTransaksi],MBeli.Kode [NoTransaksi],1 [NoUrut],MBeli.[Tanggal],MBeli.[JatuhTempo],0 [Debet]" & vbCrLf & _
                                                      ",MBeli.Total [Kredit],MBeli.Total [Saldo],'' [ReffNoTransaksi],-1 [ReffNoUrut]" & vbCrLf & _
                                                      "FROM MBeli" & vbCrLf & _
                                                      "INNER JOIN MAlamat ON MAlamat.NoID=MBeli.IDSupplier" & vbCrLf & _
                                                      "WHERE MBeli.NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "UPDATE [dbo].[MBarang]" & vbCrLf & _
                                                      "SET [IDUser]=MBeli.IDUserPosted" & vbCrLf & _
                                                      ",[TanggalUpdate]=MBeli.TglPosted" & vbCrLf & _
                                                      ",[Keterangan] = 'Update Harga Beli Dari Pembelian ' + MBeli.Kode" & vbCrLf & _
                                                      ",[IDTypePajak] = MBeli.IDTypePajak" & vbCrLf & _
                                                      ",[IDSupplier3] = MBeli.IDSupplier" & vbCrLf & _
                                                      ",[HargaBeli] = MBeliD.Harga" & vbCrLf & _
                                                      ",[IDSatuanBeli] = MBeliD.IDSatuan" & vbCrLf & _
                                                      ",[IsiCtn] = MBeliD.Konversi" & vbCrLf & _
                                                      ",[HargaBeliPcsBruto] = ROUND(MBeliD.Harga/MBeliD.Konversi, 2)" & vbCrLf & _
                                                      ",[DiscProsen1] = MBeliD.DiscProsen1" & vbCrLf & _
                                                      ",[DiscProsen2] = MBeliD.DiscProsen2" & vbCrLf & _
                                                      ",[DiscProsen3] = MBeliD.DiscProsen3" & vbCrLf & _
                                                      ",[DiscProsen4] = MBeliD.DiscProsen4" & vbCrLf & _
                                                      ",[DiscProsen5] = MBeliD.DiscProsen5" & vbCrLf & _
                                                      ",[DiscRp] = MBeliD.DiscRp" & vbCrLf & _
                                                      ",[HargaBeliPcs] = ROUND(MBeliD.Jumlah/(MBeliD.Qty*MBeliD.Konversi), 2)" & vbCrLf & _
                                                      ",[ProsenUpA] = ROUND((MBarang.HargaJualA-ROUND(MBeliD.Jumlah/(MBeliD.Qty*MBeliD.Konversi), 2))/ROUND(MBeliD.Jumlah/(MBeliD.Qty*MBeliD.Konversi), 2)*100, 2)" & vbCrLf & _
                                                      ",[ProsenUpB] = ROUND((MBarang.HargaJualB-ROUND(MBeliD.Jumlah/(MBeliD.Qty*MBeliD.Konversi), 2))/ROUND(MBeliD.Jumlah/(MBeliD.Qty*MBeliD.Konversi), 2)*100, 2)" & vbCrLf & _
                                                      "FROM MBarang" & vbCrLf & _
                                                      "INNER JOIN MBeliD ON MBeliD.IDBarang=MBarang.NoID" & vbCrLf & _
                                                      "INNER JOIN MBeli ON MBeliD.IDHeader=MBeli.NoID" & vbCrLf & _
                                                      "WHERE ISNULL(MBeliD.Jumlah, 0)<>0 AND MBeli.NoID=" & NoID
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

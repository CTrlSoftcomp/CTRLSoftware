Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.Repository.Repository.AppLog
Imports CtrlSoft.Dto.ViewModel
Imports CtrlSoft.Dto.Model
Imports System.Data
Imports System.Data.SqlClient

Namespace Repository
    Public Class PostingData
        Public Shared Function PostingPO(ByVal StrKonSQL As String,
                                         ByVal UserLogin As MUser,
                                         ByVal NoID As Long) As JSONResult
            Dim JSON As New JSONResult With {.JSONMessage = "", .JSONResult = True, .JSONRows = 0, .JSONValue = Nothing}
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
                                    With JSON
                                        .JSONResult = True
                                        .JSONMessage = "Data PO NoID : " & NoID & " Berhasil diPosting"
                                        .JSONRows = 1
                                        .JSONValue = Nothing
                                    End With
                                Else
                                    With JSON
                                        .JSONResult = False
                                        .JSONMessage = "Data PO NoID : " & NoID & " Tidak ditemukan"
                                        .JSONRows = 0
                                        .JSONValue = Nothing
                                    End With
                                End If
                            Catch ex As Exception
                                With JSON
                                    .JSONResult = False
                                    .JSONMessage = ex.Message
                                    .JSONRows = 0
                                    .JSONValue = Nothing
                                End With
                            Finally
                                InsertLog(StrKonSQL, New CtrlSoft.Dto.ViewModel.AppLog With {
                                          .AppName = My.Application.Info.AssemblyName,
                                          .Date = Now,
                                          .Event = "PostingPO",
                                          .User = UserLogin.Nama,
                                          .JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)},
                                      JSON)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return JSON
        End Function
        Public Shared Function PostingBeli(ByVal StrKonSQL As String,
                                           ByVal UserLogin As MUser,
                                           ByVal NoID As Long) As JSONResult
            Dim JSON As New JSONResult With {.JSONMessage = "", .JSONResult = True, .JSONRows = 0, .JSONValue = Nothing}
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
                                                  "WHERE ISNULL(MBeli.IsPosted,0)=0 AND MBeli.NoID=" & NoID
                                oDA.Fill(ds, "MBeli")
                                If ds.Tables("MBeli").Rows.Count >= 1 Then
                                    Dim Limit As Double = 0.0
                                    com.CommandText = "SELECT SUM(Kredit-Debet) AS Saldo FROM MHutangPiutang WHERE CONVERT(DATE, Tanggal)<=CONVERT(DATE, '" & NullToDate(ds.Tables("MBeli").Rows(0).Item("Tanggal")).ToString("yyyy-MM-dd") & "') AND IDAlamat=" & NullToLong(ds.Tables("MBeli").Rows(0).Item("IDSupplier"))
                                    Limit = NullToDbl(ds.Tables("MBeli").Rows(0).Item("LimitHutang")) - NullToDbl(com.ExecuteScalar()) - NullToDbl(ds.Tables("MBeli").Rows(0).Item("Total"))
                                    If Limit < 0 Then
                                        With JSON
                                            .JSONResult = False
                                            .JSONMessage = "Limit Hutang tidak mencukupi! Setting dulu di master Supplier."
                                            .JSONRows = 0
                                            .JSONValue = Nothing
                                        End With
                                    End If

                                    If JSON.JSONResult Then
                                        com.CommandText = "UPDATE MBeli SET IsPosted=1, TglPosted=GETDATE(), IDUserPosted=" & UserLogin.NoID & " WHERE ISNULL(IsPosted, 0)=0 AND NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi=2 AND IDTransaksi=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "INSERT INTO [dbo].[MKartuStok] (" & vbCrLf & _
                                                          "[Tanggal],[IDBarang],[IDBarangD],[Varian],[IDJenisTransaksi],[IDTransaksi],[IDTransaksiD],[IDGudang]" & vbCrLf & _
                                                          ",[IDSatuan],[Konversi],[QtyMasuk],[QtyKeluar],[Debet],[Kredit],[HargaBeli],[HPP],[SaldoAkhir]" & vbCrLf & _
                                                          ",[NilaiAkhir])" & vbCrLf & _
                                                          "SELECT MBeli.[Tanggal],MBeliD.[IDBarang],MBeliD.[IDBarangD],''[Varian],2 [IDJenisTransaksi],MBeli.NoID [IDTransaksi],MBeliD.NoID [IDTransaksiD],MBeli.IDGudang [IDGudang]" & vbCrLf & _
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
                                                          "WHERE (MBeliD.Qty*MBeliD.Konversi)<>0 AND ISNULL(MBeliD.Jumlah, 0)<>0 AND MBeli.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        com.Transaction.Commit()
                                        With JSON
                                            .JSONResult = True
                                            .JSONMessage = "Data Pembelian NoID : " & NoID & " Berhasil diPosting"
                                            .JSONRows = 1
                                            .JSONValue = Nothing
                                        End With
                                    End If
                                Else
                                    With JSON
                                        .JSONResult = False
                                        .JSONMessage = "Data Pembelian NoID : " & NoID & " Tidak ditemukan"
                                        .JSONRows = 0
                                        .JSONValue = Nothing
                                    End With
                                End If
                            Catch ex As Exception
                                With JSON
                                    .JSONResult = False
                                    .JSONMessage = ex.Message
                                    .JSONRows = 0
                                    .JSONValue = Nothing
                                End With
                            Finally
                                InsertLog(StrKonSQL, New CtrlSoft.Dto.ViewModel.AppLog With {
                                          .AppName = My.Application.Info.AssemblyName,
                                          .Date = Now,
                                          .Event = "PostingBeli",
                                          .User = UserLogin.Nama,
                                          .JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)},
                                      JSON)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return JSON
        End Function
        Public Shared Function PostingReturBeli(ByVal StrKonSQL As String,
                                                ByVal UserLogin As MUser,
                                                ByVal NoID As Long) As JSONResult
            Dim JSON As New JSONResult With {.JSONMessage = "", .JSONResult = True, .JSONRows = 0, .JSONValue = Nothing}
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
                                                  "WHERE ISNULL(MReturBeli.Total,0)>0 AND ISNULL(MReturBeli.IsPosted,0)=0 AND MReturBeli.NoID=" & NoID
                                oDA.Fill(ds, "MReturBeli")
                                If ds.Tables("MReturBeli").Rows.Count >= 1 Then
                                    com.CommandText = "UPDATE MReturBeli SET IsPosted=1, TglPosted=GETDATE(), IDUserPosted=" & UserLogin.NoID & " WHERE ISNULL(IsPosted, 0)=0 AND NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi=3 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "INSERT INTO [dbo].[MKartuStok] (" & vbCrLf & _
                                                      "[Tanggal],[IDBarang],[IDBarangD],[Varian],[IDJenisTransaksi],[IDTransaksi],[IDTransaksiD],[IDGudang]" & vbCrLf & _
                                                      ",[IDSatuan],[Konversi],[QtyMasuk],[QtyKeluar],[Debet],[Kredit],[HargaBeli],[HPP],[SaldoAkhir]" & vbCrLf & _
                                                      ",[NilaiAkhir])" & vbCrLf & _
                                                      "SELECT MReturBeli.[Tanggal],MReturBeliD.[IDBarang],MReturBeliD.[IDBarangD],''[Varian],3 [IDJenisTransaksi],MReturBeli.NoID [IDTransaksi],MReturBeliD.NoID [IDTransaksiD],MReturBeli.IDGudang [IDGudang]" & vbCrLf & _
                                                      ",MReturBeliD.IDSatuan [IDSatuan],MReturBeliD.Konversi [Konversi],0 [QtyMasuk],MReturBeliD.Qty [QtyKeluar],0 [Debet],0 [Kredit],0 [HargaBeli],0 [HPP],0 [SaldoAkhir],0 [NilaiAkhir]" & vbCrLf & _
                                                      "FROM MReturBeliD" & vbCrLf & _
                                                      "INNER JOIN MReturBeli ON MReturBeli.NoID=MReturBeliD.IDHeader" & vbCrLf & _
                                                      "WHERE MReturBeli.NoID=" & NullToLong(NoID)
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MHutangPiutang] WHERE IDJenisTransaksi=3 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "INSERT INTO [dbo].[MHutangPiutang] (" & vbCrLf & _
                                                      "[IDAlamat],[IDTransaksi],[IDJenisTransaksi],[NoTransaksi],[NoUrut],[Tanggal],[JatuhTempo],[Debet]" & vbCrLf & _
                                                      ",[Kredit],[Saldo],[ReffNoTransaksi],[ReffNoUrut])" & vbCrLf & _
                                                      "SELECT MReturBeli.IDSupplier, MReturBeli.NoID, 3 [IDJenisTransaksi],MReturBeli.Kode [NoTransaksi],1 [NoUrut],MReturBeli.[Tanggal],MReturBeli.[JatuhTempo],MReturBeli.Total [Debet]" & vbCrLf & _
                                                      ",0 [Kredit],MReturBeli.Total [Saldo],'' [ReffNoTransaksi],-1 [ReffNoUrut]" & vbCrLf & _
                                                      "FROM MReturBeli" & vbCrLf & _
                                                      "INNER JOIN MAlamat ON MAlamat.NoID=MReturBeli.IDSupplier" & vbCrLf & _
                                                      "WHERE MReturBeli.NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
                                    With JSON
                                        .JSONResult = True
                                        .JSONMessage = "Data Retur Pembelian NoID : " & NoID & " Berhasil diPosting"
                                        .JSONRows = 1
                                        .JSONValue = Nothing
                                    End With
                                Else
                                    With JSON
                                        .JSONResult = False
                                        .JSONMessage = "Data Retur Pembelian NoID : " & NoID & " Tidak ditemukan"
                                        .JSONRows = 0
                                        .JSONValue = Nothing
                                    End With
                                End If
                            Catch ex As Exception
                                With JSON
                                    .JSONResult = False
                                    .JSONMessage = ex.Message
                                    .JSONRows = 0
                                    .JSONValue = Nothing
                                End With
                            Finally
                                InsertLog(StrKonSQL, New CtrlSoft.Dto.ViewModel.AppLog With {
                                          .AppName = My.Application.Info.AssemblyName,
                                          .Date = Now,
                                          .Event = "PostingReturBeli",
                                          .User = UserLogin.Nama,
                                          .JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)},
                                      JSON)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return JSON
        End Function
        Public Shared Function PostingJual(ByVal StrKonSQL As String,
                                           ByVal UserLogin As MUser,
                                           ByVal NoID As Long) As JSONResult
            Dim JSON As New JSONResult With {.JSONMessage = "", .JSONResult = True, .JSONRows = 0, .JSONValue = Nothing}
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

                                com.CommandText = "SELECT MJualD.*, MJual.Total, CASE WHEN MJual.Total-ISNULL(MJualDBayar.Jumlah, 0)<0 THEN 0 ELSE MJual.Total-ISNULL(MJualDBayar.Jumlah, 0) END AS Sisa, MJual.Sisa Sisa1, MJual.Tanggal, MJual.Kode AS KdTransaksi, MJual.IDCustomer, MJual.DPP, MJual.PPN, MAlamat.LimitPiutang, MAlamat.LimitNotaPiutang, MAlamat.LimitUmurPiutang" & vbCrLf & _
                                                  "FROM MJualD" & vbCrLf & _
                                                  "INNER JOIN MJual ON MJual.NoID=MJualD.IDHeader" & vbCrLf & _
                                                  "INNER JOIN MAlamat ON MAlamat.NoID=MJual.IDCustomer" & vbCrLf & _
                                                  "LEFT JOIN (SELECT IDHeader, SUM(Nominal) AS Jumlah FROM MJualDBayar GROUP BY IDHeader) AS MJualDBayar ON MJualDBayar.IDHeader=MJual.NoID" & vbCrLf & _
                                                  "WHERE ISNULL(MJual.Total,0)>0 AND ISNULL(MJual.IsPosted,0)=0 AND MJual.NoID=" & NoID
                                oDA.Fill(ds, "MJual")
                                If ds.Tables("MJual").Rows.Count >= 1 Then
                                    If NullToDbl(ds.Tables("MJual").Rows(0).Item("Sisa")) <> NullToDbl(ds.Tables("MJual").Rows(0).Item("Sisa1")) Then
                                        With JSON
                                            .JSONResult = False
                                            .JSONMessage = "Lakukan Pembayaran dengan benar dahulu."
                                            .JSONRows = 0
                                            .JSONValue = Nothing
                                        End With
                                    Else
                                        com.CommandText = "SELECT A.IDAlamat" & vbCrLf & _
                                                          ",SUM((A.Debet - A.Kredit) - ISNULL(B.Pelunasan, 0)) AS Saldo" & vbCrLf & _
                                                          ",COUNT(A.NoTransaksi) AS JmlNota" & vbCrLf & _
                                                          ",DATEDIFF(DAY, MAX(A.Tanggal), GETDATE()) TglTerlamaPiutang" & vbCrLf & _
                                                          "FROM MHutangPiutang A(NOLOCK)" & vbCrLf & _
                                                          "LEFT JOIN (" & vbCrLf & _
                                                          "SELECT ReffNoTransaksi" & vbCrLf & _
                                                          ",ReffNoUrut" & vbCrLf & _
                                                          ",SUM(Kredit - Debet) AS Pelunasan" & vbCrLf & _
                                                          "FROM MHutangPiutang(NOLOCK)" & vbCrLf & _
                                                          "GROUP BY ReffNoTransaksi" & vbCrLf & _
                                                          ",ReffNoUrut" & vbCrLf & _
                                                          ") AS B ON B.ReffNoTransaksi = A.NoTransaksi" & vbCrLf & _
                                                          "AND B.ReffNoUrut = A.NoUrut" & vbCrLf & _
                                                          "WHERE ISNULL(A.ReffNoTransaksi, '')='' AND ISNULL(A.ReffNoUrut, 0)<=0 AND (A.Debet - A.Kredit) - ISNULL(B.Pelunasan, 0) <> 0.0" & vbCrLf & _
                                                          "GROUP BY A.IDAlamat" & vbCrLf & _
                                                          "HAVING A.IDAlamat = " & NullToLong(ds.Tables("MJual").Rows(0).Item("IDCustomer"))
                                        oDA.Fill(ds, "MPiutang")

                                        If ds.Tables("MPiutang").Rows.Count >= 1 Then
                                            If JSON.JSONResult AndAlso NullToDbl(ds.Tables("MJual").Rows(0).Item("LimitPiutang")) - NullToDbl(ds.Tables("MPiutang").Rows(0).Item("Saldo")) - NullToDbl(ds.Tables("MJual").Rows(0).Item("Sisa")) < 0 Then
                                                With JSON
                                                    .JSONResult = False
                                                    .JSONMessage = "Limit Piutang tidak mencukupi! Setting dulu di master Customer."
                                                    .JSONRows = 0
                                                    .JSONValue = Nothing
                                                End With
                                            End If
                                            If JSON.JSONResult AndAlso NullToDbl(ds.Tables("MJual").Rows(0).Item("LimitNotaPiutang")) - NullToDbl(ds.Tables("MPiutang").Rows(0).Item("JmlNota")) - IIf(NullToDbl(ds.Tables("MJual").Rows(0).Item("Sisa")) > 0, 1, 0) < 0 Then
                                                With JSON
                                                    .JSONResult = False
                                                    .JSONMessage = "Limit Nota Piutang tidak mencukupi! Setting dulu di master Customer."
                                                    .JSONRows = 0
                                                    .JSONValue = Nothing
                                                End With
                                            End If
                                            If JSON.JSONResult AndAlso NullToDbl(ds.Tables("MJual").Rows(0).Item("LimitUmurPiutang")) - NullToDbl(ds.Tables("MPiutang").Rows(0).Item("TglTerlamaPiutang")) < 0 Then
                                                With JSON
                                                    .JSONResult = False
                                                    .JSONMessage = "Limit Umur Nota Piutang tidak mencukupi! Setting dulu di master Customer."
                                                    .JSONRows = 0
                                                    .JSONValue = Nothing
                                                End With
                                            End If
                                        End If

                                        If JSON.JSONResult Then
                                            com.CommandText = "UPDATE MJual SET IsPosted=1, TglPosted=GETDATE(), IDUserPosted=" & UserLogin.NoID & " WHERE ISNULL(IsPosted, 0)=0 AND NoID=" & NoID
                                            com.ExecuteNonQuery()

                                            com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi=6 AND IDTransaksi=" & NoID
                                            com.ExecuteNonQuery()

                                            com.CommandText = "INSERT INTO [dbo].[MKartuStok] (" & vbCrLf & _
                                                              "[Tanggal],[IDBarang],[IDBarangD],[Varian],[IDJenisTransaksi],[IDTransaksi],[IDTransaksiD],[IDGudang]" & vbCrLf & _
                                                              ",[IDSatuan],[Konversi],[QtyMasuk],[QtyKeluar],[Debet],[Kredit],[HargaBeli],[HPP],[SaldoAkhir]" & vbCrLf & _
                                                              ",[NilaiAkhir])" & vbCrLf & _
                                                              "SELECT MJual.[Tanggal],MJualD.[IDBarang],MJualD.[IDBarangD],''[Varian],6 [IDJenisTransaksi],MJual.NoID [IDTransaksi],MJualD.NoID [IDTransaksiD],MJual.IDGudang [IDGudang]" & vbCrLf & _
                                                              ",MJualD.IDSatuan [IDSatuan],MJualD.Konversi [Konversi],0 [QtyMasuk],MJualD.Qty [QtyKeluar],0 [Debet],0 [Kredit],0 [HargaBeli],0 [HPP],0 [SaldoAkhir],0 [NilaiAkhir]" & vbCrLf & _
                                                              "FROM MJualD" & vbCrLf & _
                                                              "INNER JOIN MJual ON MJual.NoID=MJualD.IDHeader" & vbCrLf & _
                                                              "WHERE MJual.NoID=" & NullToLong(NoID)
                                            com.ExecuteNonQuery()

                                            com.CommandText = "DELETE FROM [dbo].[MHutangPiutang] WHERE IDJenisTransaksi=6 AND IDTransaksi=" & NoID
                                            com.ExecuteNonQuery()

                                            com.CommandText = "INSERT INTO [dbo].[MHutangPiutang] (" & vbCrLf & _
                                                              "[IDAlamat],[IDTransaksi],[IDJenisTransaksi],[NoTransaksi],[NoUrut],[Tanggal],[JatuhTempo],[Debet]" & vbCrLf & _
                                                              ",[Kredit],[Saldo],[ReffNoTransaksi],[ReffNoUrut])" & vbCrLf & _
                                                              "SELECT MJual.IDCustomer, MJual.NoID, 6 [IDJenisTransaksi],MJual.Kode [NoTransaksi],1 [NoUrut],MJual.[Tanggal],MJual.[JatuhTempo],MJual.Sisa [Debet]" & vbCrLf & _
                                                              ",0 [Kredit],MJual.Sisa [Saldo],'' [ReffNoTransaksi],-1 [ReffNoUrut]" & vbCrLf & _
                                                              "FROM MJual" & vbCrLf & _
                                                              "INNER JOIN MAlamat ON MAlamat.NoID=MJual.IDCustomer" & vbCrLf & _
                                                              "WHERE MJual.Sisa > 0 AND MJual.NoID=" & NoID
                                            com.ExecuteNonQuery()

                                            com.Transaction.Commit()
                                            With JSON
                                                .JSONResult = True
                                                .JSONMessage = "Data Penjualan NoID : " & NoID & " berhasil diposting"
                                                .JSONRows = 1
                                                .JSONValue = Nothing
                                            End With
                                        End If
                                    End If
                                Else
                                    With JSON
                                        .JSONResult = False
                                        .JSONMessage = "Data Penjualan NoID : " & NoID & " Tidak ditemukan"
                                        .JSONRows = 0
                                        .JSONValue = Nothing
                                    End With
                                End If
                            Catch ex As Exception
                                With JSON
                                    .JSONResult = False
                                    .JSONMessage = ex.Message
                                    .JSONRows = 0
                                    .JSONValue = Nothing
                                End With
                            Finally
                                InsertLog(StrKonSQL, New CtrlSoft.Dto.ViewModel.AppLog With {
                                          .AppName = My.Application.Info.AssemblyName,
                                          .Date = Now,
                                          .Event = "PostingJual",
                                          .User = UserLogin.Nama,
                                          .JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)},
                                      JSON)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return JSON
        End Function
        Public Shared Function PostingReturJual(ByVal StrKonSQL As String,
                                                ByVal UserLogin As MUser,
                                                ByVal NoID As Long) As JSONResult
            Dim JSON As New JSONResult With {.JSONMessage = "", .JSONResult = True, .JSONRows = 0, .JSONValue = Nothing}
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

                                com.CommandText = "SELECT MReturJualD.*, MReturJual.Total, CASE WHEN MReturJual.Total-ISNULL(MReturJualDBayar.Jumlah, 0)<0 THEN 0 ELSE MReturJual.Total-ISNULL(MReturJualDBayar.Jumlah, 0) END AS Sisa, MReturJual.Sisa Sisa1, MReturJual.Tanggal, MReturJual.Kode AS KdTransaksi, MReturJual.IDCustomer, MReturJual.DPP, MReturJual.PPN, MAlamat.LimitPiutang, MAlamat.LimitNotaPiutang, MAlamat.LimitUmurPiutang" & vbCrLf & _
                                                  "FROM MReturJualD" & vbCrLf & _
                                                  "INNER JOIN MReturJual ON MReturJual.NoID=MReturJualD.IDHeader" & vbCrLf & _
                                                  "INNER JOIN MAlamat ON MAlamat.NoID=MReturJual.IDCustomer" & vbCrLf & _
                                                  "LEFT JOIN (SELECT IDHeader, SUM(Nominal) AS Jumlah FROM MReturJualDBayar GROUP BY IDHeader) AS MReturJualDBayar ON MReturJualDBayar.IDHeader=MReturJual.NoID" & vbCrLf & _
                                                  "WHERE ISNULL(MReturJual.Total,0)>0 AND ISNULL(MReturJual.IsPosted,0)=0 AND MReturJual.NoID=" & NoID
                                oDA.Fill(ds, "MReturJual")
                                If ds.Tables("MReturJual").Rows.Count >= 1 Then
                                    If NullToDbl(ds.Tables("MReturJual").Rows(0).Item("Sisa")) <> NullToDbl(ds.Tables("MReturJual").Rows(0).Item("Sisa1")) Then
                                        With JSON
                                            .JSONResult = False
                                            .JSONMessage = "Lakukan Pembayaran dengan benar dahulu."
                                            .JSONRows = 0
                                            .JSONValue = Nothing
                                        End With
                                    Else
                                        'com.CommandText = "SELECT A.IDAlamat" & vbCrLf & _
                                        '                  ",SUM((A.Debet - A.Kredit) - ISNULL(B.Pelunasan, 0)) AS Saldo" & vbCrLf & _
                                        '                  ",COUNT(A.NoTransaksi) AS JmlNota" & vbCrLf & _
                                        '                  ",DATEDIFF(DAY, MAX(A.Tanggal), GETDATE()) TglTerlamaPiutang" & vbCrLf & _
                                        '                  "FROM MHutangPiutang A(NOLOCK)" & vbCrLf & _
                                        '                  "LEFT JOIN (" & vbCrLf & _
                                        '                  "SELECT ReffNoTransaksi" & vbCrLf & _
                                        '                  ",ReffNoUrut" & vbCrLf & _
                                        '                  ",SUM(Kredit - Debet) AS Pelunasan" & vbCrLf & _
                                        '                  "FROM MHutangPiutang(NOLOCK)" & vbCrLf & _
                                        '                  "GROUP BY ReffNoTransaksi" & vbCrLf & _
                                        '                  ",ReffNoUrut" & vbCrLf & _
                                        '                  ") AS B ON B.ReffNoTransaksi = A.NoTransaksi" & vbCrLf & _
                                        '                  "AND B.ReffNoUrut = A.NoUrut" & vbCrLf & _
                                        '                  "WHERE ISNULL(A.ReffNoTransaksi, '')='' AND ISNULL(A.ReffNoUrut, 0)<=0 AND (A.Debet - A.Kredit) - ISNULL(B.Pelunasan, 0) <> 0.0" & vbCrLf & _
                                        '                  "GROUP BY A.IDAlamat" & vbCrLf & _
                                        '                  "HAVING A.IDAlamat = " & NullToLong(ds.Tables("MReturJual").Rows(0).Item("IDCustomer"))
                                        'oDA.Fill(ds, "MPiutang")

                                        'If ds.Tables("MPiutang").Rows.Count >= 1 Then
                                        '    If NullToDbl(ds.Tables("MReturJual").Rows(0).Item("LimitPiutang")) - NullToDbl(ds.Tables("MPiutang").Rows(0).Item("Saldo")) - NullToDbl(ds.Tables("MReturJual").Rows(0).Item("Sisa")) < 0 Then
                                        '        XtraMessageBox.Show("Limit Piutang tidak mencukupi! Setting dulu di master Customer.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                        '        Return False
                                        '    End If
                                        '    If NullToDbl(ds.Tables("MReturJual").Rows(0).Item("LimitNotaPiutang")) - NullToDbl(ds.Tables("MPiutang").Rows(0).Item("JmlNota")) - IIf(NullToDbl(ds.Tables("MReturJual").Rows(0).Item("Sisa")) > 0, 1, 0) < 0 Then
                                        '        XtraMessageBox.Show("Limit Nota Piutang tidak mencukupi! Setting dulu di master Customer.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                        '        Return False
                                        '    End If
                                        '    If NullToDbl(ds.Tables("MReturJual").Rows(0).Item("LimitUmurPiutang")) - NullToDbl(ds.Tables("MPiutang").Rows(0).Item("TglTerlamaPiutang")) < 0 Then
                                        '        XtraMessageBox.Show("Limit Umur Nota Piutang tidak mencukupi! Setting dulu di master Customer.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                        '        Return False
                                        '    End If
                                        'End If

                                        com.CommandText = "UPDATE MReturJual SET IsPosted=1, TglPosted=GETDATE(), IDUserPosted=" & UserLogin.NoID & " WHERE ISNULL(IsPosted, 0)=0 AND NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi=7 AND IDTransaksi=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "INSERT INTO [dbo].[MKartuStok] (" & vbCrLf & _
                                                          "[Tanggal],[IDBarang],[IDBarangD],[Varian],[IDJenisTransaksi],[IDTransaksi],[IDTransaksiD],[IDGudang]" & vbCrLf & _
                                                          ",[IDSatuan],[Konversi],[QtyMasuk],[QtyKeluar],[Debet],[Kredit],[HargaBeli],[HPP],[SaldoAkhir]" & vbCrLf & _
                                                          ",[NilaiAkhir])" & vbCrLf & _
                                                          "SELECT MReturJual.[Tanggal],MReturJualD.[IDBarang],MReturJualD.[IDBarangD],''[Varian],7 [IDJenisTransaksi],MReturJual.NoID [IDTransaksi],MReturJualD.NoID [IDTransaksiD],MReturJual.IDGudang [IDGudang]" & vbCrLf & _
                                                          ",MReturJualD.IDSatuan [IDSatuan],MReturJualD.Konversi [Konversi],MReturJualD.Qty [QtyMasuk],0 [QtyKeluar],0 [Debet],0 [Kredit],0 [HargaBeli],0 [HPP],0 [SaldoAkhir],0 [NilaiAkhir]" & vbCrLf & _
                                                          "FROM MReturJualD" & vbCrLf & _
                                                          "INNER JOIN MReturJual ON MReturJual.NoID=MReturJualD.IDHeader" & vbCrLf & _
                                                          "WHERE MReturJual.NoID=" & NullToLong(NoID)
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE FROM [dbo].[MHutangPiutang] WHERE IDJenisTransaksi=7 AND IDTransaksi=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "INSERT INTO [dbo].[MHutangPiutang] (" & vbCrLf & _
                                                          "[IDAlamat],[IDTransaksi],[IDJenisTransaksi],[NoTransaksi],[NoUrut],[Tanggal],[JatuhTempo],[Debet]" & vbCrLf & _
                                                          ",[Kredit],[Saldo],[ReffNoTransaksi],[ReffNoUrut])" & vbCrLf & _
                                                          "SELECT MReturJual.IDCustomer, MReturJual.NoID, 7 [IDJenisTransaksi],MReturJual.Kode [NoTransaksi],1 [NoUrut],MReturJual.[Tanggal],MReturJual.[JatuhTempo],0 [Debet]" & vbCrLf & _
                                                          ",MReturJual.Sisa [Kredit],MReturJual.Sisa [Saldo],'' [ReffNoTransaksi],-1 [ReffNoUrut]" & vbCrLf & _
                                                          "FROM MReturJual" & vbCrLf & _
                                                          "INNER JOIN MAlamat ON MAlamat.NoID=MReturJual.IDCustomer" & vbCrLf & _
                                                          "WHERE MReturJual.Sisa > 0 AND MReturJual.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        com.Transaction.Commit()
                                        With JSON
                                            .JSONResult = True
                                            .JSONMessage = "Data Retur Penjualan NoID : " & NoID & " berhasil diposting"
                                            .JSONRows = 1
                                            .JSONValue = Nothing
                                        End With
                                    End If
                                Else
                                    With JSON
                                        .JSONResult = False
                                        .JSONMessage = "Data Retur Penjualan NoID : " & NoID & " Tidak ditemukan"
                                        .JSONRows = 0
                                        .JSONValue = Nothing
                                    End With
                                End If
                            Catch ex As Exception
                                With JSON
                                    .JSONResult = False
                                    .JSONMessage = ex.Message
                                    .JSONRows = 0
                                    .JSONValue = Nothing
                                End With
                            Finally
                                InsertLog(StrKonSQL, New CtrlSoft.Dto.ViewModel.AppLog With {
                                          .AppName = My.Application.Info.AssemblyName,
                                          .Date = Now,
                                          .Event = "PostingReturJual",
                                          .User = UserLogin.Nama,
                                          .JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)},
                                      JSON)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return JSON
        End Function
        Public Shared Function PostingMutasiGudang(ByVal StrKonSQL As String,
                                                   ByVal UserLogin As MUser,
                                                   ByVal NoID As Long) As JSONResult
            Dim JSON As New JSONResult With {.JSONMessage = "", .JSONResult = True, .JSONRows = 0, .JSONValue = Nothing}
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

                                com.CommandText = "SELECT MMutasiGudangD.*, MMutasiGudang.Total" & vbCrLf & _
                                                  "FROM MMutasiGudangD" & vbCrLf & _
                                                  "INNER JOIN MMutasiGudang ON MMutasiGudang.NoID=MMutasiGudangD.IDHeader" & vbCrLf & _
                                                  "INNER JOIN MGudang MGudangAsal ON MGudangAsal.NoID=MMutasiGudang.IDGudangAsal" & vbCrLf & _
                                                  "INNER JOIN MGudang MGudangTujuan ON MGudangTujuan.NoID=MMutasiGudang.IDGudangTujuan" & vbCrLf & _
                                                  "WHERE ISNULL(MMutasiGudang.IsPosted,0)=0 AND MMutasiGudang.NoID=" & NoID
                                oDA.Fill(ds, "MMutasiGudang")
                                If ds.Tables("MMutasiGudang").Rows.Count >= 1 Then
                                    com.CommandText = "UPDATE MMutasiGudang SET IsPosted=1, TglPosted=GETDATE(), IDUserPosted=" & UserLogin.NoID & " WHERE ISNULL(IsPosted, 0)=0 AND NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi IN (4,5) AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "INSERT INTO [dbo].[MKartuStok] (" & vbCrLf & _
                                                      "[Tanggal],[IDBarang],[IDBarangD],[Varian],[IDJenisTransaksi],[IDTransaksi],[IDTransaksiD],[IDGudang]" & vbCrLf & _
                                                      ",[IDSatuan],[Konversi],[QtyMasuk],[QtyKeluar],[Debet],[Kredit],[HargaBeli],[HPP],[SaldoAkhir]" & vbCrLf & _
                                                      ",[NilaiAkhir])" & vbCrLf & _
                                                      "SELECT MMutasiGudang.[Tanggal],MMutasiGudangD.[IDBarang],MMutasiGudangD.[IDBarangD],''[Varian],4 [IDJenisTransaksi],MMutasiGudang.NoID [IDTransaksi],MMutasiGudangD.NoID [IDTransaksiD],MMutasiGudang.IDGudangAsal [IDGudang]" & vbCrLf & _
                                                      ",MMutasiGudangD.IDSatuan [IDSatuan],MMutasiGudangD.Konversi [Konversi],0 [QtyMasuk],MMutasiGudangD.Qty [QtyKeluar],0 [Debet],0 [Kredit],0 [HargaBeli],MMutasiGudangD.HPP [HPP],0 [SaldoAkhir],0 [NilaiAkhir]" & vbCrLf & _
                                                      "FROM MMutasiGudangD" & vbCrLf & _
                                                      "INNER JOIN MMutasiGudang ON MMutasiGudang.NoID=MMutasiGudangD.IDHeader" & vbCrLf & _
                                                      "WHERE MMutasiGudang.NoID=" & NullToLong(NoID)
                                    com.ExecuteNonQuery()

                                    com.CommandText = "INSERT INTO [dbo].[MKartuStok] (" & vbCrLf & _
                                                      "[Tanggal],[IDBarang],[IDBarangD],[Varian],[IDJenisTransaksi],[IDTransaksi],[IDTransaksiD],[IDGudang]" & vbCrLf & _
                                                      ",[IDSatuan],[Konversi],[QtyMasuk],[QtyKeluar],[Debet],[Kredit],[HargaBeli],[HPP],[SaldoAkhir]" & vbCrLf & _
                                                      ",[NilaiAkhir])" & vbCrLf & _
                                                      "SELECT MMutasiGudang.[Tanggal],MMutasiGudangD.[IDBarang],MMutasiGudangD.[IDBarangD],''[Varian],5 [IDJenisTransaksi],MMutasiGudang.NoID [IDTransaksi],MMutasiGudangD.NoID [IDTransaksiD],MMutasiGudang.IDGudangTujuan [IDGudang]" & vbCrLf & _
                                                      ",MMutasiGudangD.IDSatuan [IDSatuan],MMutasiGudangD.Konversi [Konversi],MMutasiGudangD.Qty [QtyMasuk],0 [QtyKeluar],0 [Debet],0 [Kredit],0 [HargaBeli],MMutasiGudangD.HPP [HPP],0 [SaldoAkhir],0 [NilaiAkhir]" & vbCrLf & _
                                                      "FROM MMutasiGudangD" & vbCrLf & _
                                                      "INNER JOIN MMutasiGudang ON MMutasiGudang.NoID=MMutasiGudangD.IDHeader" & vbCrLf & _
                                                      "WHERE MMutasiGudang.NoID=" & NullToLong(NoID)
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
                                    With JSON
                                        .JSONResult = True
                                        .JSONMessage = "Data Mutasi Gudang NoID : " & NoID & " berhasil diposting"
                                        .JSONRows = 1
                                        .JSONValue = Nothing
                                    End With
                                Else
                                    With JSON
                                        .JSONResult = False
                                        .JSONMessage = "Data Mutasi Gudang NoID : " & NoID & " Tidak ditemukan"
                                        .JSONRows = 0
                                        .JSONValue = Nothing
                                    End With
                                End If
                            Catch ex As Exception
                                With JSON
                                    .JSONResult = False
                                    .JSONMessage = ex.Message
                                    .JSONRows = 0
                                    .JSONValue = Nothing
                                End With
                            Finally
                                InsertLog(StrKonSQL, New CtrlSoft.Dto.ViewModel.AppLog With {
                                          .AppName = My.Application.Info.AssemblyName,
                                          .Date = Now,
                                          .Event = "PostingMutasiGudang",
                                          .User = UserLogin.Nama,
                                          .JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)},
                                      JSON)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return JSON
        End Function
        Public Shared Function PostingPemakaian(ByVal StrKonSQL As String,
                                                ByVal UserLogin As MUser,
                                                ByVal NoID As Long) As JSONResult
            Dim JSON As New JSONResult With {.JSONMessage = "", .JSONResult = True, .JSONRows = 0, .JSONValue = Nothing}
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

                                com.CommandText = "SELECT MPemakaianD.*, MPemakaian.Total" & vbCrLf & _
                                                  "FROM MPemakaianD" & vbCrLf & _
                                                  "INNER JOIN MPemakaian ON MPemakaian.NoID=MPemakaianD.IDHeader" & vbCrLf & _
                                                  "WHERE ISNULL(MPemakaian.IsPosted,0)=0 AND MPemakaian.NoID=" & NoID
                                oDA.Fill(ds, "MPemakaian")
                                If ds.Tables("MPemakaian").Rows.Count >= 1 Then
                                    com.CommandText = "UPDATE MPemakaian SET IsPosted=1, TglPosted=GETDATE(), IDUserPosted=" & UserLogin.NoID & " WHERE ISNULL(IsPosted, 0)=0 AND NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi = 8 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "INSERT INTO [dbo].[MKartuStok] (" & vbCrLf & _
                                                      "[Tanggal],[IDBarang],[IDBarangD],[Varian],[IDJenisTransaksi],[IDTransaksi],[IDTransaksiD],[IDGudang]" & vbCrLf & _
                                                      ",[IDSatuan],[Konversi],[QtyMasuk],[QtyKeluar],[Debet],[Kredit],[HargaBeli],[HPP],[SaldoAkhir]" & vbCrLf & _
                                                      ",[NilaiAkhir])" & vbCrLf & _
                                                      "SELECT MPemakaian.[Tanggal],MPemakaianD.[IDBarang],MPemakaianD.[IDBarangD],''[Varian],8 [IDJenisTransaksi],MPemakaian.NoID [IDTransaksi],MPemakaianD.NoID [IDTransaksiD],MPemakaian.IDGudang [IDGudang]" & vbCrLf & _
                                                      ",MPemakaianD.IDSatuan [IDSatuan],MPemakaianD.Konversi [Konversi],0 [QtyMasuk],MPemakaianD.Qty [QtyKeluar],0 [Debet],0 [Kredit],0 [HargaBeli],MPemakaianD.HPP [HPP],0 [SaldoAkhir],0 [NilaiAkhir]" & vbCrLf & _
                                                      "FROM MPemakaianD" & vbCrLf & _
                                                      "INNER JOIN MPemakaian ON MPemakaian.NoID=MPemakaianD.IDHeader" & vbCrLf & _
                                                      "WHERE MPemakaian.NoID=" & NullToLong(NoID)
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
                                    With JSON
                                        .JSONResult = True
                                        .JSONMessage = "Data Pemakaian NoID : " & NoID & " berhasil diposting"
                                        .JSONRows = 1
                                        .JSONValue = Nothing
                                    End With
                                Else
                                    With JSON
                                        .JSONResult = False
                                        .JSONMessage = "Data Pemakaian NoID : " & NoID & " Tidak ditemukan"
                                        .JSONRows = 0
                                        .JSONValue = Nothing
                                    End With
                                End If
                            Catch ex As Exception
                                With JSON
                                    .JSONResult = False
                                    .JSONMessage = ex.Message
                                    .JSONRows = 0
                                    .JSONValue = Nothing
                                End With
                            Finally
                                InsertLog(StrKonSQL, New CtrlSoft.Dto.ViewModel.AppLog With {
                                          .AppName = My.Application.Info.AssemblyName,
                                          .Date = Now,
                                          .Event = "PostingPemakaian",
                                          .User = UserLogin.Nama,
                                          .JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)},
                                      JSON)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return JSON
        End Function
        Public Shared Function PostingPenyesuaian(ByVal StrKonSQL As String,
                                                  ByVal UserLogin As MUser,
                                                  ByVal NoID As Long) As JSONResult
            Dim JSON As New JSONResult With {.JSONMessage = "", .JSONResult = True, .JSONRows = 0, .JSONValue = Nothing}
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

                                com.CommandText = "SELECT MPenyesuaianD.*, MPenyesuaian.Total" & vbCrLf & _
                                                  "FROM MPenyesuaianD" & vbCrLf & _
                                                  "INNER JOIN MPenyesuaian ON MPenyesuaian.NoID=MPenyesuaianD.IDHeader" & vbCrLf & _
                                                  "WHERE ISNULL(MPenyesuaian.IsPosted,0)=0 AND MPenyesuaian.NoID=" & NoID
                                oDA.Fill(ds, "MPenyesuaian")
                                If ds.Tables("MPenyesuaian").Rows.Count >= 1 Then
                                    com.CommandText = "UPDATE MPenyesuaian SET IsPosted=1, TglPosted=GETDATE(), IDUserPosted=" & UserLogin.NoID & " WHERE ISNULL(IsPosted, 0)=0 AND NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi = 14 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "INSERT INTO [dbo].[MKartuStok] (" & vbCrLf & _
                                                      "[Tanggal],[IDBarang],[IDBarangD],[Varian],[IDJenisTransaksi],[IDTransaksi],[IDTransaksiD],[IDGudang]" & vbCrLf & _
                                                      ",[IDSatuan],[Konversi],[QtyMasuk],[QtyKeluar],[Debet],[Kredit],[HargaBeli],[HPP],[SaldoAkhir]" & vbCrLf & _
                                                      ",[NilaiAkhir])" & vbCrLf & _
                                                      "SELECT MPenyesuaian.[Tanggal],MPenyesuaianD.[IDBarang],MPenyesuaianD.[IDBarangD],''[Varian],14 [IDJenisTransaksi],MPenyesuaian.NoID [IDTransaksi],MPenyesuaianD.NoID [IDTransaksiD],MPenyesuaian.IDGudang [IDGudang]" & vbCrLf & _
                                                      ",MPenyesuaianD.IDSatuan [IDSatuan],MPenyesuaianD.Konversi [Konversi],MPenyesuaianD.Qty [QtyMasuk],0 [QtyKeluar],MPenyesuaianD.Jumlah [Debet],0 [Kredit],0 [HargaBeli],MPenyesuaianD.HPP [HPP],0 [SaldoAkhir],0 [NilaiAkhir]" & vbCrLf & _
                                                      "FROM MPenyesuaianD" & vbCrLf & _
                                                      "INNER JOIN MPenyesuaian ON MPenyesuaian.NoID=MPenyesuaianD.IDHeader" & vbCrLf & _
                                                      "WHERE MPenyesuaian.NoID=" & NullToLong(NoID)
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
                                    With JSON
                                        .JSONResult = True
                                        .JSONMessage = "Data Penyesuaian NoID : " & NoID & " berhasil diposting"
                                        .JSONRows = 1
                                        .JSONValue = Nothing
                                    End With
                                Else
                                    With JSON
                                        .JSONResult = False
                                        .JSONMessage = "Data Penyesuaian NoID : " & NoID & " Tidak ditemukan"
                                        .JSONRows = 0
                                        .JSONValue = Nothing
                                    End With
                                End If
                            Catch ex As Exception
                                With JSON
                                    .JSONResult = False
                                    .JSONMessage = ex.Message
                                    .JSONRows = 0
                                    .JSONValue = Nothing
                                End With
                            Finally
                                InsertLog(StrKonSQL, New CtrlSoft.Dto.ViewModel.AppLog With {
                                          .AppName = My.Application.Info.AssemblyName,
                                          .Date = Now,
                                          .Event = "PostingPenyesuaian",
                                          .User = UserLogin.Nama,
                                          .JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)},
                                      JSON)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return JSON
        End Function
        Public Shared Function PostingStockOpname(ByVal StrKonSQL As String,
                                                  ByVal UserLogin As MUser,
                                                  ByVal NoID As Long) As JSONResult
            Dim JSON As New JSONResult With {.JSONMessage = "", .JSONResult = True, .JSONRows = 0, .JSONValue = Nothing}
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

                                com.CommandText = "SELECT MStockOpnameD.*, MStockOpname.Total, MStockOpname.Tanggal, MStockOpname.IDGudang" & vbCrLf & _
                                                  "FROM MStockOpnameD" & vbCrLf & _
                                                  "INNER JOIN MStockOpname ON MStockOpname.NoID=MStockOpnameD.IDHeader" & vbCrLf & _
                                                  "WHERE ISNULL(MStockOpname.IsPosted,0)=0 AND MStockOpname.NoID=" & NoID
                                oDA.Fill(ds, "MStockOpname")
                                If ds.Tables("MStockOpname").Rows.Count >= 1 Then
                                    com.CommandText = "UPDATE MStockOpname SET IsPosted=1, TglPosted=GETDATE(), IDUserPosted=" & UserLogin.NoID & " WHERE ISNULL(IsPosted, 0)=0 AND NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi = 20 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    'Hitung Selisih, Jumlah, dan Total
                                    com.CommandText = "UPDATE MStockOpnameD SET QtyKomputer=ISNULL(MSaldo.Saldo, 0), Qty=ISNULL(MSaldo.Saldo, 0)-ISNULL(MStockOpnameD.QtyFisik, 0), Jumlah=ROUND(ISNULL(MStockOpnameD.HPP, 0)*(ISNULL(MSaldo.Saldo, 0)-ISNULL(MStockOpnameD.QtyFisik, 0)), 2)" & vbCrLf & _
                                                      "FROM MStockOpnameD" & vbCrLf & _
                                                      "INNER JOIN MStockOpname ON MStockOpname.NoID=MStockOpnameD.IDHeader" & vbCrLf & _
                                                      "LEFT JOIN (SELECT MKartuStok.IDBarang, SUM(MKartuStok.Konversi*(MKartuStok.QtyMasuk-MKartuStok.QtyKeluar)) AS Saldo FROM MKartuStok(NOLOCK) WHERE MKartuStok.IDGudang=@IDGudang AND MKartuStok.Tanggal<=@Tanggal GROUP BY MKartuStok.IDBarang) MSaldo ON MSaldo.IDBarang=MStockOpnameD.IDBarang" & vbCrLf & _
                                                      "WHERE MStockOpname.NoID=@NoID"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NoID
                                    com.Parameters.Add(New SqlParameter("@IDGudang", SqlDbType.Int)).Value = NullToLong(ds.Tables("MStockOpname").Rows(0).Item("IDGudang"))
                                    com.Parameters.Add(New SqlParameter("@Tanggal", SqlDbType.DateTime)).Value = NullToDate(ds.Tables("MStockOpname").Rows(0).Item("Tanggal"))
                                    com.ExecuteNonQuery()

                                    com.CommandText = "UPDATE MStockOpname SET Total=ISNULL(MStockOpnameD.Jumlah, 0)" & vbCrLf & _
                                                      "FROM MStockOpname " & vbCrLf & _
                                                      "INNER JOIN (SELECT IDHeader, SUM(Jumlah) AS Jumlah FROM MStockOpnameD GROUP BY IDHeader) AS MStockOpnameD ON MStockOpnameD.IDHeader=MStockOpname.NoID " & vbCrLf & _
                                                      "WHERE MStockOpname.NoID=" & NoID
                                    com.Parameters.Clear()
                                    com.ExecuteNonQuery()

                                    com.CommandText = "INSERT INTO [dbo].[MKartuStok] (" & vbCrLf & _
                                                      "[Tanggal],[IDBarang],[IDBarangD],[Varian],[IDJenisTransaksi],[IDTransaksi],[IDTransaksiD],[IDGudang]" & vbCrLf & _
                                                      ",[IDSatuan],[Konversi],[QtyMasuk],[QtyKeluar],[Debet],[Kredit],[HargaBeli],[HPP],[SaldoAkhir]" & vbCrLf & _
                                                      ",[NilaiAkhir])" & vbCrLf & _
                                                      "SELECT MStockOpname.[Tanggal],MStockOpnameD.[IDBarang],MStockOpnameD.[IDBarangD],''[Varian],20 [IDJenisTransaksi],MStockOpname.NoID [IDTransaksi],MStockOpnameD.NoID [IDTransaksiD],MStockOpname.IDGudang [IDGudang]" & vbCrLf & _
                                                      ",MStockOpnameD.IDSatuan [IDSatuan],MStockOpnameD.Konversi [Konversi],MStockOpnameD.Qty [QtyMasuk],0 [QtyKeluar],MStockOpnameD.Jumlah [Debet],0 [Kredit],0 [HargaBeli],MStockOpnameD.HPP [HPP],0 [SaldoAkhir],0 [NilaiAkhir]" & vbCrLf & _
                                                      "FROM MStockOpnameD" & vbCrLf & _
                                                      "INNER JOIN MStockOpname ON MStockOpname.NoID=MStockOpnameD.IDHeader" & vbCrLf & _
                                                      "WHERE MStockOpname.NoID=" & NullToLong(NoID)
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
                                    With JSON
                                        .JSONResult = True
                                        .JSONMessage = "Data Stock Opname NoID : " & NoID & " berhasil diposting"
                                        .JSONRows = 1
                                        .JSONValue = Nothing
                                    End With
                                Else
                                    With JSON
                                        .JSONResult = False
                                        .JSONMessage = "Data Stock Opname NoID : " & NoID & " Tidak ditemukan"
                                        .JSONRows = 0
                                        .JSONValue = Nothing
                                    End With
                                End If
                            Catch ex As Exception
                                With JSON
                                    .JSONResult = False
                                    .JSONMessage = ex.Message
                                    .JSONRows = 0
                                    .JSONValue = Nothing
                                End With
                            Finally
                                InsertLog(StrKonSQL, New CtrlSoft.Dto.ViewModel.AppLog With {
                                          .AppName = My.Application.Info.AssemblyName,
                                          .Date = Now,
                                          .Event = "PostingStockOpname",
                                          .User = UserLogin.Nama,
                                          .JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)},
                                      JSON)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return JSON
        End Function
        Public Shared Function PostingSaldoAwalPersediaan(ByVal StrKonSQL As String,
                                                          ByVal UserLogin As MUser,
                                                          ByVal NoID As Long) As JSONResult
            Dim JSON As New JSONResult With {.JSONMessage = "", .JSONResult = True, .JSONRows = 0, .JSONValue = Nothing}
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

                                com.CommandText = "SELECT MSaldoAwalPersediaan.*, MSaldoAwalPersediaan.Jumlah AS Total, MSaldoAwalPersediaan.Tanggal, MSaldoAwalPersediaan.IDGudang" & vbCrLf &
                                                  "FROM MSaldoAwalPersediaan" & vbCrLf &
                                                  "WHERE ISNULL(MSaldoAwalPersediaan.IsPosted,0)=0 AND MSaldoAwalPersediaan.NoID=" & NoID
                                oDA.Fill(ds, "MSaldoAwalPersediaan")
                                If ds.Tables("MSaldoAwalPersediaan").Rows.Count >= 1 Then
                                    com.CommandText = "UPDATE MSaldoAwalPersediaan SET IsPosted=1, TglPosted=GETDATE(), IDUserPosted=" & UserLogin.NoID & " WHERE ISNULL(IsPosted, 0)=0 AND NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi = 1 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    'Hitung Selisih, Jumlah, dan Total
                                    com.CommandText = "UPDATE MSaldoAwalPersediaan SET Jumlah=ROUND(ISNULL(MSaldoAwalPersediaan.HargaPokok, 0)*ISNULL(MSaldoAwalPersediaan.Qty, 0), 2)" & vbCrLf &
                                                      "FROM MSaldoAwalPersediaan " & vbCrLf &
                                                      "WHERE MSaldoAwalPersediaan.NoID=" & NoID
                                    com.Parameters.Clear()
                                    com.ExecuteNonQuery()

                                    com.CommandText = "INSERT INTO [dbo].[MKartuStok] (" & vbCrLf &
                                                      "[Tanggal],[IDBarang],[IDBarangD],[Varian],[IDJenisTransaksi],[IDTransaksi],[IDTransaksiD],[IDGudang]" & vbCrLf &
                                                      ",[IDSatuan],[Konversi],[QtyMasuk],[QtyKeluar],[Debet],[Kredit],[HargaBeli],[HPP],[SaldoAkhir]" & vbCrLf &
                                                      ",[NilaiAkhir])" & vbCrLf &
                                                      "SELECT MSaldoAwalPersediaan.[Tanggal],MSaldoAwalPersediaan.[IDBarang],MSaldoAwalPersediaan.[IDBarangD],''[Varian],1 [IDJenisTransaksi],MSaldoAwalPersediaan.NoID [IDTransaksi],MSaldoAwalPersediaan.NoID [IDTransaksiD],MSaldoAwalPersediaan.IDGudang [IDGudang]" & vbCrLf &
                                                      ",MSaldoAwalPersediaan.IDSatuan [IDSatuan],MSaldoAwalPersediaan.Konversi [Konversi],MSaldoAwalPersediaan.Qty [QtyMasuk],0 [QtyKeluar],MSaldoAwalPersediaan.Jumlah [Debet],0 [Kredit],0 [HargaBeli],MSaldoAwalPersediaan.HargaPokok [HPP],0 [SaldoAkhir],0 [NilaiAkhir]" & vbCrLf &
                                                      "FROM MSaldoAwalPersediaan" & vbCrLf &
                                                      "WHERE MSaldoAwalPersediaan.NoID=" & NullToLong(NoID)
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
                                    With JSON
                                        .JSONResult = True
                                        .JSONMessage = "Data Saldo Awal Persediaan NoID : " & NoID & " berhasil diposting"
                                        .JSONRows = 1
                                        .JSONValue = Nothing
                                    End With
                                Else
                                    With JSON
                                        .JSONResult = False
                                        .JSONMessage = "Data Saldo Awal Persediaan NoID : " & NoID & " Tidak ditemukan"
                                        .JSONRows = 0
                                        .JSONValue = Nothing
                                    End With
                                End If
                            Catch ex As Exception
                                With JSON
                                    .JSONResult = False
                                    .JSONMessage = ex.Message
                                    .JSONRows = 0
                                    .JSONValue = Nothing
                                End With
                            Finally
                                InsertLog(StrKonSQL, New CtrlSoft.Dto.ViewModel.AppLog With {
                                          .AppName = My.Application.Info.AssemblyName,
                                          .Date = Now,
                                          .Event = "PostingSaldoAwalPersediaan",
                                          .User = UserLogin.Nama,
                                          .JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)},
                                      JSON)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return JSON
        End Function
        Public Shared Function PostingSaldoAwalHutang(ByVal StrKonSQL As String,
                                                      ByVal UserLogin As MUser,
                                                      ByVal NoID As Long) As JSONResult
            Dim JSON As New JSONResult With {.JSONMessage = "", .JSONResult = True, .JSONRows = 0, .JSONValue = Nothing}
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

                                com.CommandText = "SELECT MSaldoAwalHutang.*, MSaldoAwalHutang.Jumlah AS Total, MSaldoAwalHutang.Tanggal " & vbCrLf &
                                                  "FROM MSaldoAwalHutang" & vbCrLf &
                                                  "WHERE ISNULL(MSaldoAwalHutang.IsPosted,0)=0 AND MSaldoAwalHutang.NoID=" & NoID
                                oDA.Fill(ds, "MSaldoAwalHutang")
                                If ds.Tables("MSaldoAwalHutang").Rows.Count >= 1 Then
                                    com.CommandText = "UPDATE MSaldoAwalHutang SET IsPosted=1, TglPosted=GETDATE(), IDUserPosted=" & UserLogin.NoID & " WHERE ISNULL(IsPosted, 0)=0 AND NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi = 1001 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MHutangPiutang] WHERE IDJenisTransaksi = 1001 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "INSERT INTO [dbo].[MHutangPiutang] (" & vbCrLf &
                                                      "[IDAlamat],[IDTransaksi],[IDJenisTransaksi],[NoTransaksi],[NoUrut],[Tanggal],[JatuhTempo],[Debet]" & vbCrLf &
                                                      ",[Kredit],[Saldo],[ReffNoTransaksi],[ReffNoUrut])" & vbCrLf &
                                                      "SELECT MSaldoAwalHutang.IDSupplier, MSaldoAwalHutang.NoID, 1001 [IDJenisTransaksi],MSaldoAwalHutang.Kode [NoTransaksi],1 [NoUrut],MSaldoAwalHutang.[Tanggal],MSaldoAwalHutang.[JatuhTempo],CASE WHEN ISNULL(MSaldoAwalHutang.Jumlah, 0)<0 THEN ABS(MSaldoAwalHutang.Jumlah) ELSE 0 END [Debet]" & vbCrLf &
                                                      ",CASE WHEN ISNULL(MSaldoAwalHutang.Jumlah, 0)>0 THEN ABS(MSaldoAwalHutang.Jumlah) ELSE 0 END [Kredit],-1*ISNULL(MSaldoAwalHutang.Jumlah, 0) [Saldo],'' [ReffNoTransaksi],-1 [ReffNoUrut]" & vbCrLf &
                                                      "FROM MSaldoAwalHutang" & vbCrLf &
                                                      "INNER JOIN MAlamat ON MAlamat.NoID=MSaldoAwalHutang.IDSupplier" & vbCrLf &
                                                      "WHERE MSaldoAwalHutang.NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
                                    With JSON
                                        .JSONResult = True
                                        .JSONMessage = "Data Saldo Awal Hutang NoID : " & NoID & " berhasil diposting"
                                        .JSONRows = 1
                                        .JSONValue = Nothing
                                    End With
                                Else
                                    With JSON
                                        .JSONResult = False
                                        .JSONMessage = "Data Saldo Awal Hutang NoID : " & NoID & " Tidak ditemukan"
                                        .JSONRows = 0
                                        .JSONValue = Nothing
                                    End With
                                End If
                            Catch ex As Exception
                                With JSON
                                    .JSONResult = False
                                    .JSONMessage = ex.Message
                                    .JSONRows = 0
                                    .JSONValue = Nothing
                                End With
                            Finally
                                InsertLog(StrKonSQL, New CtrlSoft.Dto.ViewModel.AppLog With {
                                          .AppName = My.Application.Info.AssemblyName,
                                          .Date = Now,
                                          .Event = "PostingSaldoAwalHutang",
                                          .User = UserLogin.Nama,
                                          .JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)},
                                      JSON)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return JSON
        End Function
        Public Shared Function PostingSaldoAwalPiutang(ByVal StrKonSQL As String,
                                                      ByVal UserLogin As MUser,
                                                      ByVal NoID As Long) As JSONResult
            Dim JSON As New JSONResult With {.JSONMessage = "", .JSONResult = True, .JSONRows = 0, .JSONValue = Nothing}
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

                                com.CommandText = "SELECT MSaldoAwalPiutang.*, MSaldoAwalPiutang.Jumlah AS Total, MSaldoAwalPiutang.Tanggal " & vbCrLf &
                                                  "FROM MSaldoAwalPiutang" & vbCrLf &
                                                  "WHERE ISNULL(MSaldoAwalPiutang.IsPosted,0)=0 AND MSaldoAwalPiutang.NoID=" & NoID
                                oDA.Fill(ds, "MSaldoAwalPiutang")
                                If ds.Tables("MSaldoAwalPiutang").Rows.Count >= 1 Then
                                    com.CommandText = "UPDATE MSaldoAwalPiutang SET IsPosted=1, TglPosted=GETDATE(), IDUserPosted=" & UserLogin.NoID & " WHERE ISNULL(IsPosted, 0)=0 AND NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi = 1002 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MPiutangPiutang] WHERE IDJenisTransaksi = 1002 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "INSERT INTO [dbo].[MPiutangPiutang] (" & vbCrLf &
                                                      "[IDAlamat],[IDTransaksi],[IDJenisTransaksi],[NoTransaksi],[NoUrut],[Tanggal],[JatuhTempo],[Debet]" & vbCrLf &
                                                      ",[Kredit],[Saldo],[ReffNoTransaksi],[ReffNoUrut])" & vbCrLf &
                                                      "SELECT MSaldoAwalPiutang.IDCustomer, MSaldoAwalPiutang.NoID, 1002 [IDJenisTransaksi],MSaldoAwalPiutang.Kode [NoTransaksi],1 [NoUrut],MSaldoAwalPiutang.[Tanggal],MSaldoAwalPiutang.[JatuhTempo],CASE WHEN ISNULL(MSaldoAwalPiutang.Jumlah, 0)>0 THEN ABS(MSaldoAwalPiutang.Jumlah) ELSE 0 END [Debet]" & vbCrLf &
                                                      ",CASE WHEN ISNULL(MSaldoAwalPiutang.Jumlah, 0)<0 THEN ABS(MSaldoAwalPiutang.Jumlah) ELSE 0 END [Kredit],ISNULL(MSaldoAwalPiutang.Jumlah, 0) [Saldo],'' [ReffNoTransaksi],-1 [ReffNoUrut]" & vbCrLf &
                                                      "FROM MSaldoAwalPiutang" & vbCrLf &
                                                      "INNER JOIN MAlamat ON MAlamat.NoID=MSaldoAwalPiutang.IDCustomer" & vbCrLf &
                                                      "WHERE MSaldoAwalPiutang.NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
                                    With JSON
                                        .JSONResult = True
                                        .JSONMessage = "Data Saldo Awal Piutang NoID : " & NoID & " berhasil diposting"
                                        .JSONRows = 1
                                        .JSONValue = Nothing
                                    End With
                                Else
                                    With JSON
                                        .JSONResult = False
                                        .JSONMessage = "Data Saldo Awal Piutang NoID : " & NoID & " Tidak ditemukan"
                                        .JSONRows = 0
                                        .JSONValue = Nothing
                                    End With
                                End If
                            Catch ex As Exception
                                With JSON
                                    .JSONResult = False
                                    .JSONMessage = ex.Message
                                    .JSONRows = 0
                                    .JSONValue = Nothing
                                End With
                            Finally
                                InsertLog(StrKonSQL, New CtrlSoft.Dto.ViewModel.AppLog With {
                                          .AppName = My.Application.Info.AssemblyName,
                                          .Date = Now,
                                          .Event = "PostingSaldoAwalPiutang",
                                          .User = UserLogin.Nama,
                                          .JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)},
                                      JSON)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return JSON
        End Function
    End Class
End Namespace

Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.Repository.Repository.AppLog
Imports CtrlSoft.Dto.ViewModel
Imports CtrlSoft.Dto.Model
Imports System.Data
Imports System.Data.SqlClient

Namespace Repository
    Public Class UnPostingData
        Public Shared Function UnPostingPO(ByVal StrKonSQL As String,
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

                                com.CommandText = "SELECT COUNT(MBeliD.NoID) FROM MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDHeader INNER JOIN MPOD ON MPOD.NoID=MBeliD.IDPOD WHERE MPOD.IDHeader=" & NoID
                                If NullToLong(com.ExecuteScalar()) = 0 Then
                                    com.CommandText = "UPDATE MPO SET IsPosted=0, TglPosted=NULL, IDUserPosted=NULL WHERE NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
                                    With JSON
                                        .JSONResult = True
                                        .JSONMessage = "Data PO NoID : " & NoID & " Berhasil diUnPosting"
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
                                          .Event = "UnPostingPO",
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
        Public Shared Function UnPostingBeli(ByVal StrKonSQL As String,
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
                                                  "WHERE ISNULL(MBeli.Total,0)>0 AND ISNULL(MBeli.IsPosted,0)=1 AND MBeli.NoID=" & NoID
                                oDA.Fill(ds, "MBeli")
                                If ds.Tables("MBeli").Rows.Count >= 1 Then
                                    com.CommandText = "SELECT COUNT(MReturBeliD.NoID) FROM MReturBeliD INNER JOIN MReturBeli ON MReturBeli.NoID=MReturBeliD.IDHeader INNER JOIN MBeliD ON MBeliD.NoID=MReturBeliD.IDBeliD WHERE MBeliD.IDHeader=" & NoID
                                    If JSON.JSONResult AndAlso NullToLong(com.ExecuteScalar()) <> 0 Then
                                        With JSON
                                            .JSONResult = False
                                            .JSONMessage = "Pembelian sudah pernah diretur!"
                                            .JSONRows = 0
                                            .JSONValue = Nothing
                                        End With
                                    End If
                                    com.CommandText = "SELECT COUNT(NoID) FROM MHutangPiutang WHERE ReffNoTransaksi='" & FixApostropi(ds.Tables("MBeli").Rows(0).Item("KdTransaksi")) & "'"
                                    If JSON.JSONResult AndAlso NullToLong(com.ExecuteScalar()) <> 0 Then
                                        With JSON
                                            .JSONResult = False
                                            .JSONMessage = "Pembelian sudah dibayar!"
                                            .JSONRows = 0
                                            .JSONValue = Nothing
                                        End With
                                    End If

                                    If JSON.JSONResult Then
                                        com.CommandText = "UPDATE MBeli SET IsPosted=0, TglPosted=NULL, IDUserPosted=NULL WHERE ISNULL(IsPosted, 0)=1 AND NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi=2 AND IDTransaksi=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE FROM [dbo].[MHutangPiutang] WHERE IDJenisTransaksi=2 AND IDTransaksi=" & NoID
                                        com.ExecuteNonQuery()

                                        com.Transaction.Commit()
                                        With JSON
                                            .JSONResult = True
                                            .JSONMessage = "Data Pembelian NoID : " & NoID & " Berhasil diUnPosting"
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
                                          .Event = "UnPostingBeli",
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
        Public Shared Function UnPostingReturBeli(ByVal StrKonSQL As String,
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
                                                  "WHERE ISNULL(MReturBeli.Total,0)>0 AND ISNULL(MReturBeli.IsPosted,0)=1 AND MReturBeli.NoID=" & NoID
                                oDA.Fill(ds, "MReturBeli")
                                If ds.Tables("MReturBeli").Rows.Count >= 1 Then
                                    com.CommandText = "SELECT COUNT(NoID) FROM MHutangPiutang WHERE ReffNoTransaksi='" & FixApostropi(ds.Tables("MReturBeli").Rows(0).Item("KdTransaksi")) & "'"
                                    If JSON.JSONResult AndAlso NullToLong(com.ExecuteScalar()) <> 0 Then
                                        With JSON
                                            .JSONResult = False
                                            .JSONMessage = "Returan sudah dibayar!"
                                            .JSONRows = 0
                                            .JSONValue = Nothing
                                        End With
                                    End If

                                    If JSON.JSONResult Then
                                        com.CommandText = "UPDATE MReturBeli SET IsPosted=0, TglPosted=NULL, IDUserPosted=NULL WHERE ISNULL(IsPosted, 0)=1 AND NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi=3 AND IDTransaksi=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE FROM [dbo].[MHutangPiutang] WHERE IDJenisTransaksi=3 AND IDTransaksi=" & NoID
                                        com.ExecuteNonQuery()

                                        com.Transaction.Commit()
                                        With JSON
                                            .JSONResult = True
                                            .JSONMessage = "Data Retur Pembelian NoID : " & NoID & " Berhasil diUnPosting"
                                            .JSONRows = 1
                                            .JSONValue = Nothing
                                        End With
                                    End If
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
                                          .Event = "UnPostingReturBeli",
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
        Public Shared Function UnPostingJual(ByVal StrKonSQL As String,
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

                                com.CommandText = "SELECT MJualD.*, MJual.Total, MJual.Tanggal, MJual.Kode AS KdTransaksi, MJual.IDCustomer, MJual.DPP, MJual.PPN, MAlamat.LimitHutang" & vbCrLf & _
                                                  "FROM MJualD" & vbCrLf & _
                                                  "INNER JOIN MJual ON MJual.NoID=MJualD.IDHeader" & vbCrLf & _
                                                  "INNER JOIN MAlamat ON MAlamat.NoID=MJual.IDCustomer" & vbCrLf & _
                                                  "WHERE ISNULL(MJual.Total,0)>0 AND ISNULL(MJual.IsPosted,0)=1 AND MJual.NoID=" & NoID
                                oDA.Fill(ds, "MJual")
                                If ds.Tables("MJual").Rows.Count >= 1 Then
                                    com.CommandText = "SELECT COUNT(NoID) FROM MHutangPiutang WHERE ReffNoTransaksi='" & FixApostropi(ds.Tables("MJual").Rows(0).Item("KdTransaksi")) & "'"
                                    If JSON.JSONResult AndAlso NullToLong(com.ExecuteScalar()) <> 0 Then
                                        With JSON
                                            .JSONResult = False
                                            .JSONMessage = "Penjualan sudah dibayar!"
                                            .JSONRows = 0
                                            .JSONValue = Nothing
                                        End With
                                    End If

                                    If JSON.JSONResult Then
                                        com.CommandText = "UPDATE MJual SET IsPosted=0, TglPosted=NULL, IDUserPosted=NULL WHERE ISNULL(IsPosted, 0)=1 AND NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi=6 AND IDTransaksi=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE FROM [dbo].[MHutangPiutang] WHERE IDJenisTransaksi=6 AND IDTransaksi=" & NoID
                                        com.ExecuteNonQuery()

                                        com.Transaction.Commit()
                                        With JSON
                                            .JSONResult = True
                                            .JSONMessage = "Data Penjualan NoID : " & NoID & " Berhasil diUnPosting"
                                            .JSONRows = 1
                                            .JSONValue = Nothing
                                        End With
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
                                          .Event = "UnPostingJual",
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
        Public Shared Function UnPostingReturJual(ByVal StrKonSQL As String,
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

                                com.CommandText = "SELECT MReturJualD.*, MReturJual.Total, MReturJual.Tanggal, MReturJual.Kode AS KdTransaksi, MReturJual.IDCustomer, MReturJual.DPP, MReturJual.PPN, MAlamat.LimitHutang" & vbCrLf & _
                                                  "FROM MReturJualD" & vbCrLf & _
                                                  "INNER JOIN MReturJual ON MReturJual.NoID=MReturJualD.IDHeader" & vbCrLf & _
                                                  "INNER JOIN MAlamat ON MAlamat.NoID=MReturJual.IDCustomer" & vbCrLf & _
                                                  "WHERE ISNULL(MReturJual.Total,0)>0 AND ISNULL(MReturJual.IsPosted,0)=1 AND MReturJual.NoID=" & NoID
                                oDA.Fill(ds, "MReturJual")
                                If ds.Tables("MReturJual").Rows.Count >= 1 Then
                                    com.CommandText = "SELECT COUNT(NoID) FROM MHutangPiutang WHERE ReffNoTransaksi='" & FixApostropi(ds.Tables("MReturJual").Rows(0).Item("KdTransaksi")) & "'"
                                    If JSON.JSONResult AndAlso NullToLong(com.ExecuteScalar()) <> 0 Then
                                        With JSON
                                            .JSONResult = False
                                            .JSONMessage = "Retur Penjualan sudah dibayar!"
                                            .JSONRows = 0
                                            .JSONValue = Nothing
                                        End With
                                    End If

                                    If JSON.JSONResult Then
                                        com.CommandText = "UPDATE MReturJual SET IsPosted=0, TglPosted=NULL, IDUserPosted=NULL WHERE ISNULL(IsPosted, 0)=1 AND NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi=7 AND IDTransaksi=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE FROM [dbo].[MHutangPiutang] WHERE IDJenisTransaksi=7 AND IDTransaksi=" & NoID
                                        com.ExecuteNonQuery()

                                        com.Transaction.Commit()
                                        With JSON
                                            .JSONResult = True
                                            .JSONMessage = "Data Retur Penjualan NoID : " & NoID & " Berhasil diUnPosting"
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
                                          .Event = "UnPostingReturJual",
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
        Public Shared Function UnPostingMutasiGudang(ByVal StrKonSQL As String,
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
                                    With JSON
                                        .JSONResult = True
                                        .JSONMessage = "Data Mutasi Gudang NoID : " & NoID & " Berhasil diUnPosting"
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
                                          .Event = "UnPostingMutasiGudang",
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
        Public Shared Function UnPostingPemakaian(ByVal StrKonSQL As String,
                                                  ByVal UserLogin As MUser,
                                                  ByVal NoID As Long) As JSONResult
            Dim JSON As New JSONResult With {.JSONMessage = "", .JSONResult = True, .JSONRows = 0, .JSONValue = Nothing}
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
                                    With JSON
                                        .JSONResult = True
                                        .JSONMessage = "Data Pemakaian NoID : " & NoID & " Berhasil diUnPosting"
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
                                          .Event = "UnPostingPemakaian",
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
        Public Shared Function UnPostingPenyesuaian(ByVal StrKonSQL As String,
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
                                    With JSON
                                        .JSONResult = True
                                        .JSONMessage = "Data Penyesuaian NoID : " & NoID & " Berhasil diUnPosting"
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
                                          .Event = "UnPostingPenyesuaian",
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
        Public Shared Function UnPostingStockOpname(ByVal StrKonSQL As String,
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

                                com.CommandText = "SELECT MStockOpnameD.*, MStockOpname.Total, MStockOpname.Tanggal, MStockOpname.Kode AS KdTransaksi" & vbCrLf & _
                                                  "FROM MStockOpnameD" & vbCrLf & _
                                                  "INNER JOIN MStockOpname ON MStockOpname.NoID=MStockOpnameD.IDHeader" & vbCrLf & _
                                                  "WHERE ISNULL(MStockOpname.IsPosted,0)=1 AND MStockOpname.NoID=" & NoID
                                oDA.Fill(ds, "MStockOpname")
                                If ds.Tables("MStockOpname").Rows.Count >= 1 Then
                                    com.CommandText = "UPDATE MStockOpname SET IsPosted=0, TglPosted=NULL, IDUserPosted=NULL WHERE ISNULL(IsPosted, 0)=1 AND NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi = 20 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
                                    With JSON
                                        .JSONResult = True
                                        .JSONMessage = "Data Stock Opname NoID : " & NoID & " Berhasil diUnPosting"
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
                                          .Event = "UnPostingStockOpname",
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
        Public Shared Function UnPostingSaldoAwalPersediaan(ByVal StrKonSQL As String,
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

                                com.CommandText = "SELECT MSaldoAwalPersediaan.*, MSaldoAwalPersediaan.Total, MSaldoAwalPersediaan.Tanggal, MSaldoAwalPersediaan.Kode AS KdTransaksi" & vbCrLf &
                                                  "FROM MSaldoAwalPersediaan" & vbCrLf &
                                                  "WHERE ISNULL(MSaldoAwalPersediaan.IsPosted,0)=1 AND MSaldoAwalPersediaan.NoID=" & NoID
                                oDA.Fill(ds, "MSaldoAwalPersediaan")
                                If ds.Tables("MSaldoAwalPersediaan").Rows.Count >= 1 Then
                                    com.CommandText = "UPDATE MSaldoAwalPersediaan SET IsPosted=0, TglPosted=NULL, IDUserPosted=NULL WHERE ISNULL(IsPosted, 0)=1 AND NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi = 1 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
                                    With JSON
                                        .JSONResult = True
                                        .JSONMessage = "Data Saldo Awal Persediaan NoID : " & NoID & " Berhasil diUnPosting"
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
                                          .Event = "UnPostingSaldoAwalPersediaan",
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
        Public Shared Function UnPostingSaldoAwalHutang(ByVal StrKonSQL As String,
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

                                com.CommandText = "SELECT MSaldoAwalHutang.*, MSaldoAwalHutang.Total, MSaldoAwalHutang.Tanggal, MSaldoAwalHutang.Kode AS KdTransaksi" & vbCrLf &
                                                  "FROM MSaldoAwalHutang" & vbCrLf &
                                                  "WHERE ISNULL(MSaldoAwalHutang.IsPosted,0)=1 AND MSaldoAwalHutang.NoID=" & NoID
                                oDA.Fill(ds, "MSaldoAwalHutang")
                                If ds.Tables("MSaldoAwalHutang").Rows.Count >= 1 Then
                                    com.CommandText = "UPDATE MSaldoAwalHutang SET IsPosted=0, TglPosted=NULL, IDUserPosted=NULL WHERE ISNULL(IsPosted, 0)=1 AND NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi = 1001 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MHutangPiutang] WHERE IDJenisTransaksi = 1001 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
                                    With JSON
                                        .JSONResult = True
                                        .JSONMessage = "Data Saldo Awal Hutang NoID : " & NoID & " Berhasil diUnPosting"
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
                                          .Event = "UnPostingSaldoAwalHutang",
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
        Public Shared Function UnPostingSaldoAwalPiutang(ByVal StrKonSQL As String,
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

                                com.CommandText = "SELECT MSaldoAwalPiutang.*, MSaldoAwalPiutang.Total, MSaldoAwalPiutang.Tanggal, MSaldoAwalPiutang.Kode AS KdTransaksi" & vbCrLf &
                                                  "FROM MSaldoAwalPiutang" & vbCrLf &
                                                  "WHERE ISNULL(MSaldoAwalPiutang.IsPosted,0)=1 AND MSaldoAwalPiutang.NoID=" & NoID
                                oDA.Fill(ds, "MSaldoAwalPiutang")
                                If ds.Tables("MSaldoAwalPiutang").Rows.Count >= 1 Then
                                    com.CommandText = "UPDATE MSaldoAwalPiutang SET IsPosted=0, TglPosted=NULL, IDUserPosted=NULL WHERE ISNULL(IsPosted, 0)=1 AND NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MKartuStok] WHERE IDJenisTransaksi = 1002 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "DELETE FROM [dbo].[MPiutangPiutang] WHERE IDJenisTransaksi = 1002 AND IDTransaksi=" & NoID
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
                                    With JSON
                                        .JSONResult = True
                                        .JSONMessage = "Data Saldo Awal Piutang NoID : " & NoID & " Berhasil diUnPosting"
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
                                          .Event = "UnPostingSaldoAwalPiutang",
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

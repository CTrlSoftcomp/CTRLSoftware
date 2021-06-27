Imports System.Data
Imports System.Data.SqlClient
Imports CtrlSoft.Dto

Namespace Repository
    Public Class RepUpdateDB
        Private Shared Function ExecUpdateDB(ByVal StrKonSQL As String, ByVal ListUpdate As List(Of Model.UpdateDB)) As ViewModel.JSONResult
            Dim JSON As New ViewModel.JSONResult With {.JSONResult = False, .JSONMessage = "Data Tidak ditemukan", .JSONRows = 0, .JSONValue = Nothing}
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Try
                        cn.Open()
                        com.Connection = cn
                        com.CommandTimeout = cn.ConnectionTimeout

                        For Each Obj In ListUpdate
                            Try
                                com.Transaction = cn.BeginTransaction
                                com.CommandText = "SELECT COUNT(*) FROM TAppDB(NOLOCK) WHERE [DBVersion]=@DBVersion"
                                com.Parameters.Clear()
                                com.Parameters.Add(New SqlParameter("@DBVersion", SqlDbType.VarChar)).Value = Obj.DBVersion
                                com.Parameters.Add(New SqlParameter("@Tanggal", SqlDbType.DateTime)).Value = Obj.TglUpdate
                                'com.Parameters.Add(New SqlParameter("@SQL", SqlDbType.VarChar)).Value = Obj.SQL
                                If com.ExecuteScalar() = 0 Then
                                    com.CommandText = "INSERT INTO TAppDB([DBVersion], [Tanggal]) VALUES (@DBVersion, @Tanggal)"
                                    com.ExecuteNonQuery()

                                    For Each Query In Obj.ListSQL
                                        com.CommandText = Query
                                        com.Parameters.Clear()
                                        com.ExecuteNonQuery()
                                    Next

                                    If com.Transaction IsNot Nothing Then
                                        com.Transaction.Commit()
                                    End If
                                Else
                                    If com.Transaction IsNot Nothing Then
                                        com.Transaction.Rollback()
                                    End If
                                End If
                            Catch ex As Exception
                                If com.Transaction IsNot Nothing Then
                                    com.Transaction.Rollback()
                                End If
                                With JSON
                                    .JSONMessage = "Error Update " & Obj.DBVersion & vbCrLf & "Reason : " & ex.Message
                                    .JSONResult = False
                                    .JSONRows = 0
                                    .JSONValue = Nothing
                                End With
                            End Try
                        Next
                    Catch ex As Exception
                        With JSON
                            .JSONMessage = "Ada Kesalahan :" & ex.Message
                            .JSONResult = False
                            .JSONRows = 0
                            .JSONValue = Nothing
                        End With
                    End Try
                End Using
            End Using

            Return JSON
        End Function

        Public Shared Function UpdateDB(ByVal StrKonSQL As String) As ViewModel.JSONResult
            Dim iList As New List(Of Model.UpdateDB)
            iList.AddRange(ListUpdate_July_2020)
            iList.AddRange(ListUpdate_September_2020)
            iList.AddRange(ListUpdate_Juni_2021)

            Return ExecUpdateDB(StrKonSQL, iList)
        End Function

#Region "Struktur"
        Private Shared Function ListUpdate_July_2020() As List(Of Model.UpdateDB)
            Dim Hasil As New List(Of Model.UpdateDB)
            Dim Obj As New Model.UpdateDB
            Dim SQL As String = ""

            SQL = "ALTER TABLE MPO ALTER COLUMN Tanggal DATETIME NOT NULL;" & vbCrLf & _
                  "ALTER TABLE MJual ALTER COLUMN Tanggal DATETIME NOT NULL;" & vbCrLf & _
                  "ALTER TABLE MBeli ALTER COLUMN Tanggal DATETIME NOT NULL;" & vbCrLf & _
                  "ALTER TABLE MReturBeli ALTER COLUMN Tanggal DATETIME NOT NULL;" & vbCrLf & _
                  "ALTER TABLE MReturJual ALTER COLUMN Tanggal DATETIME NOT NULL;" & vbCrLf & _
                  "ALTER TABLE MPenyesuaian ALTER COLUMN Tanggal DATETIME NOT NULL;" & vbCrLf & _
                  "ALTER TABLE MPemakaian ALTER COLUMN Tanggal DATETIME NOT NULL;" & vbCrLf & _
                  "ALTER TABLE MMutasiGudang ALTER COLUMN Tanggal DATETIME NOT NULL;"
            Obj = New Model.UpdateDB With {.DBVersion = "TglUpdate_200726", .TglUpdate = CDate("2020-07-26"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE TABLE [dbo].[MStockOpname](" & vbCrLf & _
                "[NoID] [bigint] NOT NULL," & vbCrLf & _
                "[Kode] [varchar](50) NULL," & vbCrLf & _
                "[NoReff] [varchar](50) NULL," & vbCrLf & _
                "[IDGudang] [int] NULL," & vbCrLf & _
                "[Tanggal] [datetime] NOT NULL," & vbCrLf & _
                "[Catatan] [varchar](250) NULL," & vbCrLf & _
                "[Total] [money] NULL," & vbCrLf & _
                "[IsPosted] [bit] NULL," & vbCrLf & _
                "[TglPosted] [datetime] NULL," & vbCrLf & _
                "[IDUserPosted] [int] NULL," & vbCrLf & _
                "[IDUserEntry] [int] NULL," & vbCrLf & _
                "[IDUserEdit] [int] NULL," & vbCrLf & _
                "CONSTRAINT [PK_MStockOpname] PRIMARY KEY CLUSTERED " & vbCrLf & _
                "(" & vbCrLf & _
                "[NoID] ASC" & vbCrLf & _
                ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" & vbCrLf & _
                ") ON [PRIMARY];" & vbCrLf & _
                "CREATE TABLE [dbo].[MStockOpnameD](" & vbCrLf & _
                "[NoID] [bigint] NOT NULL," & vbCrLf & _
                "[IDHeader] [bigint] NULL," & vbCrLf & _
                "[IDBarangD] [bigint] NULL," & vbCrLf & _
                "[IDBarang] [bigint] NULL," & vbCrLf & _
                "[IDSatuan] [int] NULL," & vbCrLf & _
                "[Konversi] [numeric](18, 0) NULL," & vbCrLf & _
                "[QtyKomputer] [numeric](18, 3) NULL," & vbCrLf & _
                "[QtyFisik] [numeric](18, 3) NULL," & vbCrLf & _
                "[Qty] [numeric](18, 3) NULL," & vbCrLf & _
                "[HPP] [money] NULL," & vbCrLf & _
                "[Jumlah] [money] NULL," & vbCrLf & _
                "[Keterangan] [varchar](150) NULL," & vbCrLf & _
                "CONSTRAINT [PK_MStockOpnameD] PRIMARY KEY CLUSTERED " & vbCrLf & _
                "(" & vbCrLf & _
                "[NoID] ASC" & vbCrLf & _
                ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" & vbCrLf & _
                ") ON [PRIMARY]"
            Obj = New Model.UpdateDB With {.DBVersion = "MStockOpname_200726", .TglUpdate = CDate("2020-07-26"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "-- =============================================" & vbCrLf & _
                    "-- Author: <Author,,Name>" & vbCrLf & _
                    "-- Create date: <Create Date,,>" & vbCrLf & _
                    "-- Description: <Description,,>" & vbCrLf & _
                    "-- =============================================" & vbCrLf & _
                    "CREATE PROCEDURE [dbo].[spDaftarStockOpname](@TglDari DATETIME = '2019-01-01', @TglSampai DATETIME = '2019-01-31')" & vbCrLf & _
                    "AS " & vbCrLf & _
                    "BEGIN" & vbCrLf & _
                    "SET NOCOUNT ON;" & vbCrLf & _
                    "SELECT MStockOpname.*, UserEntry.Nama UserEntry, UserEdit.Nama UserEdit, UserPosted.Nama UserPosted, MGudang.Kode Gudang" & vbCrLf & _
                    "FROM MStockOpname (NOLOCK)" & vbCrLf & _
                    "LEFT JOIN MGudang (NOLOCK) ON MStockOpname.IDGudang=MGudang.NoID" & vbCrLf & _
                    "LEFT JOIN MUser UserEntry (NOLOCK) ON MStockOpname.IDUserEntry=UserEntry.NoID" & vbCrLf & _
                    "LEFT JOIN MUser UserEdit (NOLOCK) ON MStockOpname.IDUserEdit=UserEdit.NoID" & vbCrLf & _
                    "LEFT JOIN MUser UserPosted (NOLOCK) ON MStockOpname.IDUserPosted=UserPosted.NoID" & vbCrLf & _
                    "WHERE CONVERT(DATE, MStockOpname.Tanggal) >= CONVERT(DATE, @TglDari) AND CONVERT(DATE, MStockOpname.Tanggal) <= CONVERT(DATE, @TglSampai)" & vbCrLf & _
                    "ORDER BY MStockOpname.Tanggal, MStockOpname.NoID" & vbCrLf & _
                    "END"
            Obj = New Model.UpdateDB With {.DBVersion = "spDaftarStockOpname_200726", .TglUpdate = CDate("2020-07-26"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "-- =============================================" & vbCrLf & _
                    "-- Author:  <Author,,Name>" & vbCrLf & _
                    "-- Create date: <Create Date,,>" & vbCrLf & _
                    "-- Description: <Description,,>" & vbCrLf & _
                    "-- =============================================" & vbCrLf & _
                    "CREATE PROCEDURE [dbo].[spFakturMStockOpname](@NoID BIGINT = -1)" & vbCrLf & _
                    "AS" & vbCrLf & _
                    "BEGIN" & vbCrLf & _
                    "SET NOCOUNT ON;" & vbCrLf & _
                    "SELECT MStockOpnameD.*, " & vbCrLf & _
                    "MStockOpname.Kode, " & vbCrLf & _
                    "UserEntry.Nama AS UserEntry, " & vbCrLf & _
                    "UserEdit.Nama AS UserEdit, " & vbCrLf & _
                    "UserPosted.Nama AS UserPosted, " & vbCrLf & _
                    "MGudang.Kode AS Gudang, " & vbCrLf & _
                    "MStockOpname.NoReff, " & vbCrLf & _
                    "MStockOpname.Tanggal, " & vbCrLf & _
                    "MStockOpname.Catatan, " & vbCrLf & _
                    "MStockOpname.Total, " & vbCrLf & _
                    "MStockOpname.IsPosted, " & vbCrLf & _
                    "MStockOpname.TglPosted, " & vbCrLf & _
                    "MStockOpnameD.IDHeader, " & vbCrLf & _
                    "MBarang.Kode AS KodeBarang, " & vbCrLf & _
                    "MBarang.Nama AS NamaBarang, " & vbCrLf & _
                    "MBarangD.Barcode, " & vbCrLf & _
                    "MSatuan.Kode AS Satuan" & vbCrLf & _
                    "FROM MStockOpname" & vbCrLf & _
                    "INNER JOIN MStockOpnameD ON MStockOpnameD.IDHeader = MStockOpname.NoID" & vbCrLf & _
                    "LEFT JOIN MBarangD ON MStockOpnameD.IDBarangD = MBarangD.NoID" & vbCrLf & _
                    "LEFT JOIN MSatuan ON MStockOpnameD.IDSatuan = MSatuan.NoID" & vbCrLf & _
                    "LEFT JOIN MBarang ON MStockOpnameD.IDBarang = MBarang.NoID" & vbCrLf & _
                    "LEFT JOIN MGudang ON MGudang.NoID = MStockOpname.IDGudang" & vbCrLf & _
                    "LEFT JOIN MUser AS UserPosted ON UserPosted.NoID = MStockOpname.IDUserPosted" & vbCrLf & _
                    "LEFT JOIN MUser AS UserEntry ON MStockOpname.IDUserEntry = UserEntry.NoID" & vbCrLf & _
                    "LEFT JOIN MUser AS UserEdit ON MStockOpname.IDUserEdit = UserEdit.NoID" & vbCrLf & _
                    "WHERE MStockOpname.IsPosted = 1" & vbCrLf & _
                    "AND MStockOpname.NoID = @NoID;" & vbCrLf & _
                    "END"
            Obj = New Model.UpdateDB With {.DBVersion = "spFakturMStockOpname_200726", .TglUpdate = CDate("2020-07-26"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE PROCEDURE [dbo].[spSimpanMStockOpname]" & vbCrLf & _
                    "(@NoID           BIGINT, " & vbCrLf & _
                    "@Kode           VARCHAR(255), " & vbCrLf & _
                    "@NoReff         VARCHAR(255), " & vbCrLf & _
                    "@Tanggal        DATETIME, " & vbCrLf & _
                    "@Catatan        VARCHAR(MAX), " & vbCrLf & _
                    "@Total          MONEY, " & vbCrLf & _
                    "@IsPosted       BIT, " & vbCrLf & _
                    "@TglPosted      DATETIME, " & vbCrLf & _
                    "@IDUserPosted   INT, " & vbCrLf & _
                    "@IDUserEntry    INT, " & vbCrLf & _
                    "@IDUserEdit     INT, " & vbCrLf & _
                    "@IDGudang       INT" & vbCrLf & _
                    ")" & vbCrLf & _
                    "AS" & vbCrLf & _
                    "BEGIN" & vbCrLf & _
                    "DECLARE @NoUrut AS INT= 0;" & vbCrLf & _
                    "IF(@NoID <= 0)" & vbCrLf & _
                    "BEGIN" & vbCrLf & _
                    "SELECT @NoID = MAX(NoID)" & vbCrLf & _
                    "FROM MStockOpname;" & vbCrLf & _
                    "SET @NoID = ISNULL(@NoID, 0) + 1;" & vbCrLf & _
                    "SET @Kode = 'STO/' + SUBSTRING(CONVERT(VARCHAR(100), @Tanggal, 112), 3, 4) + '/';" & vbCrLf & _
                    "SELECT @NoUrut = MAX(CASE" & vbCrLf & _
                    "    WHEN ISNUMERIC(RIGHT(Kode, 5)) = 1" & vbCrLf & _
                    "    THEN RIGHT(Kode, 5)" & vbCrLf & _
                    "    ELSE 0" & vbCrLf & _
                    "END)" & vbCrLf & _
                    "FROM MStockOpname" & vbCrLf & _
                    "WHERE LEFT(Kode, LEN(@Kode)) = @Kode;" & vbCrLf & _
                    "SET @NoUrut = ISNULL(@NoUrut, 0) + 1;" & vbCrLf & _
                    "SET @Kode = @Kode + LEFT('00000', 5 - LEN(CONVERT(VARCHAR(10), @NoUrut))) + CONVERT(VARCHAR(10), @NoUrut);" & vbCrLf & _
                    "INSERT INTO [MStockOpname]" & vbCrLf & _
                    "(NoID, " & vbCrLf & _
                    "Kode, " & vbCrLf & _
                    "NoReff, " & vbCrLf & _
                    "Tanggal,  " & vbCrLf & _
                    "Catatan, " & vbCrLf & _
                    "Total, " & vbCrLf & _
                    "IsPosted, " & vbCrLf & _
                    "TglPosted, " & vbCrLf & _
                    "IDUserPosted, " & vbCrLf & _
                    "IDUserEntry, " & vbCrLf & _
                    "IDUserEdit, " & vbCrLf & _
                    "IDGudang" & vbCrLf & _
                    ")" & vbCrLf & _
                    "SELECT @NoID, " & vbCrLf & _
                    "@Kode, " & vbCrLf & _
                    "@NoReff, " & vbCrLf & _
                    "@Tanggal, " & vbCrLf & _
                    "@Catatan, " & vbCrLf & _
                    "@Total, " & vbCrLf & _
                    "@IsPosted, " & vbCrLf & _
                    "@TglPosted, " & vbCrLf & _
                    "@IDUserPosted, " & vbCrLf & _
                    "@IDUserEntry, " & vbCrLf & _
                    "@IDUserEdit, " & vbCrLf & _
                    "@IDGudang;" & vbCrLf & _
                    "END;" & vbCrLf & _
                    "ELSE" & vbCrLf & _
                    "BEGIN" & vbCrLf & _
                    "UPDATE [MStockOpname]" & vbCrLf & _
                    "SET " & vbCrLf & _
                    "NoID = @NoID, " & vbCrLf & _
                    "Kode = @Kode, " & vbCrLf & _
                    "NoReff = @NoReff, " & vbCrLf & _
                    "Tanggal = @Tanggal, " & vbCrLf & _
                    "Catatan = @Catatan, " & vbCrLf & _
                    "Total = @Total, " & vbCrLf & _
                    "IsPosted = @IsPosted, " & vbCrLf & _
                    "TglPosted = @TglPosted, " & vbCrLf & _
                    "IDUserPosted = @IDUserPosted, " & vbCrLf & _
                    "IDUserEntry = @IDUserEntry, " & vbCrLf & _
                    "IDUserEdit = @IDUserEdit, " & vbCrLf & _
                    "IDGudang = @IDGudang" & vbCrLf & _
                    "WHERE NoID = @NoID;" & vbCrLf & _
                    "END;" & vbCrLf & _
                    "SELECT @NoID AS NoID;" & vbCrLf & _
                    "END"
            Obj = New Model.UpdateDB With {.DBVersion = "spSimpanMStockOpname_200726", .TglUpdate = CDate("2020-07-26"), .SQL = SQL}
            Hasil.Add(Obj)

            Return Hasil
        End Function

        Private Shared Function ListUpdate_September_2020() As List(Of Model.UpdateDB)
            Dim Hasil As New List(Of Model.UpdateDB)
            Dim Obj As New Model.UpdateDB
            Dim SQL As String = ""
            Dim ListSQL As List(Of String) = Nothing

            SQL = "CREATE TABLE [dbo].[MJenisTransaksiD](" & vbCrLf &
                  "[NoID] [int] NOT NULL," & vbCrLf &
                  "[IDJenisTransaksi] [int] NOT NULL," & vbCrLf &
                  "[Kode] [varchar](50) NULL," & vbCrLf &
                  "[Nama] [varchar](50) NULL," & vbCrLf &
                  "CONSTRAINT [PK_MJenisTransaksiD] PRIMARY KEY CLUSTERED" & vbCrLf &
                  "(" & vbCrLf &
                  "[NoID] Asc" & vbCrLf &
                  ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" & vbCrLf &
                  ") ON [PRIMARY]"
            Obj = New Model.UpdateDB With {.DBVersion = "MJenisTransaksiD_200906", .TglUpdate = CDate("2020-09-06"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE TABLE [dbo].[MPOS](" & vbCrLf &
                  "[NoID] [int] NOT NULL," & vbCrLf &
                  "[Kode] [varchar](50) NULL," & vbCrLf &
                  "[Nama] [varchar](50) NULL," & vbCrLf &
                  "[IsActive] [bit] NULL," & vbCrLf &
                  "[IDJenisPenjualan] [int] NULL," & vbCrLf &
                  "[LastConnected] [datetime] NULL," & vbCrLf &
                  "[LastTransaction] [datetime] NULL," & vbCrLf &
                  "CONSTRAINT [PK_MPOS] PRIMARY KEY CLUSTERED " & vbCrLf &
                  "(" & vbCrLf &
                  "[NoID] Asc" & vbCrLf &
                  ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" & vbCrLf &
                  ") ON [PRIMARY]"
            Obj = New Model.UpdateDB With {.DBVersion = "MPOS_200906", .TglUpdate = CDate("2020-09-06"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE TABLE [dbo].[MReset](" & vbCrLf &
                "[NoID] [bigint] NOT NULL," & vbCrLf &
                "[Kode] [varchar](50) NULL," & vbCrLf &
                "[IDKasir] [int] NULL," & vbCrLf &
                "[Tanggal] [date] NULL," & vbCrLf &
                "[Shift] [smallint] NULL," & vbCrLf &
                "[IDPOS] [int] NULL," & vbCrLf &
                "[JumlahNota] [int] NULL," & vbCrLf &
                "[Subtotal] [money] NULL," & vbCrLf &
                "[Tunai] [money] NULL," & vbCrLf &
                "[SetorTunai] [money] NULL," & vbCrLf &
                "[IsPosted] [bit] NULL," & vbCrLf &
                "[TglPosted] [datetime] NULL," & vbCrLf &
                "[IDUserPosted] [int] NULL," & vbCrLf &
                "[IDUserEntry] [int] NULL," & vbCrLf &
                "[IDUserEdit] [int] NULL," & vbCrLf &
                "CONSTRAINT [PK_MReset] PRIMARY KEY CLUSTERED " & vbCrLf &
                "(" & vbCrLf &
                "[NoID] ASC" & vbCrLf &
                ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" & vbCrLf &
                ") ON [PRIMARY]"
            Obj = New Model.UpdateDB With {.DBVersion = "MReset_200906", .TglUpdate = CDate("2020-09-06"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE TABLE [dbo].[MResetD](" & vbCrLf &
                "[IDReset] [bigint] NOT NULL," & vbCrLf &
                "[IDJenisPembayaran] [int] NOT NULL," & vbCrLf &
                "[JumlahNota] [int] NULL," & vbCrLf &
                "[Subtotal] [money] NULL," & vbCrLf &
                "CONSTRAINT [PK_MResetD] PRIMARY KEY CLUSTERED " & vbCrLf &
                "(" & vbCrLf &
                "[IDReset] ASC," & vbCrLf &
                "[IDJenisPembayaran] ASC" & vbCrLf &
                ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" & vbCrLf &
                ") ON [PRIMARY]"
            Obj = New Model.UpdateDB With {.DBVersion = "MResetD_200906", .TglUpdate = CDate("2020-09-06"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "UPDATE MMenu SET [Caption]='Keuangan', IsActive=1 WHERE NoID=6;" & vbCrLf &
                  "UPDATE MMenu SET [Caption]='Pelunasan Hutang dan Kas Keluar', IsActive=1 WHERE NoID=601;" & vbCrLf &
                  "UPDATE MMenu SET [Caption]='Pelunasan Piutang dan Kas Masuk', IsActive=1 WHERE NoID=602;"
            Obj = New Model.UpdateDB With {.DBVersion = "spKasBank_2000916", .TglUpdate = CDate("2020-09-16"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "UPDATE MMenu SET IsActive=1 WHERE NoID=2;" & vbCrLf &
                  "INSERT INTO MMenu (NoID, IDParent, NoUrut, [Name], Caption, IsBig, IsBeginGroup, IsActive) VALUES" & vbCrLf &
                  "(202, 2, 2, 'mnDaftarSAHS', 'Daftar Saldo Awal Hutang', 1, 1, 1)," & vbCrLf &
                  "(203, 2, 3, 'mnDaftarSAPC', 'Daftar Saldo Awal Piutang', 1, 0, 1);"
            Obj = New Model.UpdateDB With {.DBVersion = "mnSaldoAwal_2000916", .TglUpdate = CDate("2020-09-16"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE TABLE [dbo].[MSaldoAwalPersediaan](" & vbCrLf &
                  "[NoID] [int] NOT NULL," & vbCrLf &
                  "[IDGudang] [int] NOT NULL," & vbCrLf &
                  "[IDBarangD] [bigint] NOT NULL," & vbCrLf &
                  "[IDBarang] [bigint] NOT NULL," & vbCrLf &
                  "[Kode] [varchar](15) NOT NULL," & vbCrLf &
                  "[Tanggal] [datetime] NOT NULL," & vbCrLf &
                  "[Qty] [numeric](18, 3) NOT NULL," & vbCrLf &
                  "[IDSatuan] [int] NOT NULL," & vbCrLf &
                  "[Konversi] [numeric](18, 0) NOT NULL," & vbCrLf &
                  "[HargaPokok] [money] NOT NULL," & vbCrLf &
                  "[Jumlah] [money] NOT NULL," & vbCrLf &
                  "[IDUserEntry] [int] NOT NULL," & vbCrLf &
                  "[TglEntry] [datetime] NOT NULL," & vbCrLf &
                  "[IDUserEdit] [int] NULL," & vbCrLf &
                  "[TglEdit] [datetime] NULL," & vbCrLf &
                  "[IsPosted] [bit] NULL," & vbCrLf &
                  "[IDUserPosted] [int] NULL," & vbCrLf &
                  "[TglPosted] [datetime] NULL," & vbCrLf &
                  "[Keterangan] [varchar](250) NULL," & vbCrLf &
                  "CONSTRAINT [PK_MSaldoAwalPersediaan] PRIMARY KEY CLUSTERED" & vbCrLf &
                  "(" & vbCrLf &
                  "[NoID] Asc" & vbCrLf &
                  ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]," & vbCrLf &
                  "CONSTRAINT [IX_Kode] UNIQUE NONCLUSTERED" & vbCrLf &
                  "(" & vbCrLf &
                  "[Kode] Asc" & vbCrLf &
                  ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" & vbCrLf &
                  ") ON [PRIMARY]"
            Obj = New Model.UpdateDB With {.DBVersion = "MSaldoAwalPersediaan_2000916", .TglUpdate = CDate("2020-09-16"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "-- =============================================" & vbCrLf &
                    "-- Author: <Author,,Name>" & vbCrLf &
                    "-- Create date: <Create Date,,>" & vbCrLf &
                    "-- Description: <Description,,>" & vbCrLf &
                    "-- =============================================" & vbCrLf &
                    "CREATE PROCEDURE [dbo].[spSaldoAwalPersediaan](@TglDari DATETIME = '2019-01-01', @TglSampai DATETIME = '2019-01-31')" & vbCrLf &
                    "AS " & vbCrLf &
                    "BEGIN" & vbCrLf &
                    "SET NOCOUNT ON;" & vbCrLf &
                    "SELECT MSaldoAwalPersediaan.*, MBarang.Kode KodeBarang, MBarang.Nama NamaBarang, MBarangD.Barcode, UserEntry.Nama UserEntry, UserEdit.Nama UserEdit, UserPosted.Nama UserPosted, MGudang.Kode Gudang" & vbCrLf &
                    "FROM MSaldoAwalPersediaan (NOLOCK)" & vbCrLf &
                    "LEFT JOIN MGudang (NOLOCK) ON MSaldoAwalPersediaan.IDGudang=MGudang.NoID" & vbCrLf &
                    "LEFT JOIN MBarang (NOLOCK) ON MSaldoAwalPersediaan.IDBarang=MBarang.NoID" & vbCrLf &
                    "LEFT JOIN MBarangD (NOLOCK) ON MSaldoAwalPersediaan.IDBarangD=MBarangD.NoID" & vbCrLf &
                    "LEFT JOIN MUser UserEntry (NOLOCK) ON MSaldoAwalPersediaan.IDUserEntry=UserEntry.NoID" & vbCrLf &
                    "LEFT JOIN MUser UserEdit (NOLOCK) ON MSaldoAwalPersediaan.IDUserEdit=UserEdit.NoID" & vbCrLf &
                    "LEFT JOIN MUser UserPosted (NOLOCK) ON MSaldoAwalPersediaan.IDUserPosted=UserPosted.NoID" & vbCrLf &
                    "WHERE CONVERT(DATE, MSaldoAwalPersediaan.Tanggal) >= CONVERT(DATE, @TglDari) AND CONVERT(DATE, MSaldoAwalPersediaan.Tanggal) <= CONVERT(DATE, @TglSampai)" & vbCrLf &
                    "ORDER BY MSaldoAwalPersediaan.Tanggal, MSaldoAwalPersediaan.NoID" & vbCrLf &
                    "END"
            Obj = New Model.UpdateDB With {.DBVersion = "spMSaldoAwalPersediaan_200916", .TglUpdate = CDate("2020-09-16"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "-- =============================================" & vbCrLf &
                    "-- Author: <Author,,Name>" & vbCrLf &
                    "-- Create date: <Create Date,,>" & vbCrLf &
                    "-- Description: <Description,,>" & vbCrLf &
                    "-- =============================================" & vbCrLf &
                    "CREATE PROCEDURE [dbo].[spFakturMSaldoAwalPersediaan](@TglDari DATETIME = '2019-01-01', @TglSampai DATETIME = '2019-01-31')" & vbCrLf &
                    "AS " & vbCrLf &
                    "BEGIN" & vbCrLf &
                    "SET NOCOUNT ON;" & vbCrLf &
                    "SELECT MSaldoAwalPersediaan.*, MBarang.Kode KodeBarang, MBarang.Nama NamaBarang, MBarangD.Barcode, UserEntry.Nama UserEntry, UserEdit.Nama UserEdit, UserPosted.Nama UserPosted, MGudang.Kode Gudang" & vbCrLf &
                    "FROM MSaldoAwalPersediaan (NOLOCK)" & vbCrLf &
                    "LEFT JOIN MGudang (NOLOCK) ON MSaldoAwalPersediaan.IDGudang=MGudang.NoID" & vbCrLf &
                    "LEFT JOIN MBarang (NOLOCK) ON MSaldoAwalPersediaan.IDBarang=MBarang.NoID" & vbCrLf &
                    "LEFT JOIN MBarangD (NOLOCK) ON MSaldoAwalPersediaan.IDBarangD=MBarangD.NoID" & vbCrLf &
                    "LEFT JOIN MUser UserEntry (NOLOCK) ON MSaldoAwalPersediaan.IDUserEntry=UserEntry.NoID" & vbCrLf &
                    "LEFT JOIN MUser UserEdit (NOLOCK) ON MSaldoAwalPersediaan.IDUserEdit=UserEdit.NoID" & vbCrLf &
                    "LEFT JOIN MUser UserPosted (NOLOCK) ON MSaldoAwalPersediaan.IDUserPosted=UserPosted.NoID" & vbCrLf &
                    "WHERE MSaldoAwalPersediaan.IsPosted=1 AND CONVERT(DATE, MSaldoAwalPersediaan.Tanggal) >= CONVERT(DATE, @TglDari) AND CONVERT(DATE, MSaldoAwalPersediaan.Tanggal) <= CONVERT(DATE, @TglSampai)" & vbCrLf &
                    "ORDER BY MSaldoAwalPersediaan.Tanggal, MSaldoAwalPersediaan.NoID" & vbCrLf &
                    "END"
            Obj = New Model.UpdateDB With {.DBVersion = "spFakturMSaldoAwalPersediaan_200916", .TglUpdate = CDate("2020-09-16"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE PROCEDURE [dbo].[spSimpanMSaldoAwalPersediaan]" & vbCrLf &
                    "(@NoID         BIGINT, " & vbCrLf &
                    "@Kode         VARCHAR(255), " & vbCrLf &
                    "@Tanggal      DATETIME, " & vbCrLf &
                    "@Keterangan   VARCHAR(MAX), " & vbCrLf &
                    "@IDGudang     INT, " & vbCrLf &
                    "@IDBarangD    INT, " & vbCrLf &
                    "@IDBarang     INT, " & vbCrLf &
                    "@IDSatuan     INT, " & vbCrLf &
                    "@Qty          NUMERIC(18, 3), " & vbCrLf &
                    "@Konversi     INT, " & vbCrLf &
                    "@HargaPokok   MONEY, " & vbCrLf &
                    "@IsPosted     BIT, " & vbCrLf &
                    "@TglPosted    DATETIME, " & vbCrLf &
                    "@IDUserPosted INT, " & vbCrLf &
                    "@IDUserEntry  INT, " & vbCrLf &
                    "@IDUserEdit   INT" & vbCrLf &
                    ")" & vbCrLf &
                    "AS" & vbCrLf &
                    "BEGIN" & vbCrLf &
                    "DECLARE @NoUrut AS INT= 0;" & vbCrLf &
                    "IF(@NoID <= 0)" & vbCrLf &
                    "BEGIN" & vbCrLf &
                    "SELECT @NoID = MAX(NoID)" & vbCrLf &
                    "FROM MSaldoAwalPersediaan;" & vbCrLf &
                    "SET @NoID = ISNULL(@NoID, 0) + 1;" & vbCrLf &
                    "SET @Kode = 'SAP/' + SUBSTRING(CONVERT(VARCHAR(100), @Tanggal, 112), 3, 4) + '/';" & vbCrLf &
                    "SELECT @NoUrut = MAX(CASE" & vbCrLf &
                    "WHEN ISNUMERIC(RIGHT(Kode, 5)) = 1" & vbCrLf &
                    "THEN RIGHT(Kode, 5)" & vbCrLf &
                    "ELSE 0" & vbCrLf &
                    "END)" & vbCrLf &
                    "FROM MSaldoAwalPersediaan" & vbCrLf &
                    "WHERE LEFT(Kode, LEN(@Kode)) = @Kode;" & vbCrLf &
                    "SET @NoUrut = ISNULL(@NoUrut, 0) + 1;" & vbCrLf &
                    "SET @Kode = @Kode + LEFT('00000', 5 - LEN(CONVERT(VARCHAR(10), @NoUrut))) + CONVERT(VARCHAR(10), @NoUrut);" & vbCrLf &
                    "INSERT INTO [dbo].[MSaldoAwalPersediaan]" & vbCrLf &
                    "([NoID], " & vbCrLf &
                    "[IDGudang], " & vbCrLf &
                    "[IDBarangD], " & vbCrLf &
                    "[IDBarang], " & vbCrLf &
                    "[Kode], " & vbCrLf &
                    "[Tanggal], " & vbCrLf &
                    "[Qty], " & vbCrLf &
                    "[IDSatuan], " & vbCrLf &
                    "[Konversi], " & vbCrLf &
                    "[HargaPokok], " & vbCrLf &
                    "[Jumlah], " & vbCrLf &
                    "[IDUserEntry], " & vbCrLf &
                    "[TglEntry], " & vbCrLf &
                    "[IDUserEdit], " & vbCrLf &
                    "[TglEdit], " & vbCrLf &
                    "[IsPosted], " & vbCrLf &
                    "[IDUserPosted], " & vbCrLf &
                    "[TglPosted], " & vbCrLf &
                    "[Keterangan]" & vbCrLf &
                    ")" & vbCrLf &
                    "SELECT @NoID, " & vbCrLf &
                    "@IDGudang, " & vbCrLf &
                    "@IDBarangD, " & vbCrLf &
                    "@IDBarang, " & vbCrLf &
                    "@Kode, " & vbCrLf &
                    "@Tanggal, " & vbCrLf &
                    "@Qty, " & vbCrLf &
                    "@IDSatuan, " & vbCrLf &
                    "@Konversi, " & vbCrLf &
                    "@HargaPokok, " & vbCrLf &
                    "ROUND(@HargaPokok * @Qty, 2), " & vbCrLf &
                    "@IDUserEntry, " & vbCrLf &
                    "GETDATE(), " & vbCrLf &
                    "NULL, " & vbCrLf &
                    "NULL, " & vbCrLf &
                    "0, " & vbCrLf &
                    "NULL, " & vbCrLf &
                    "NULL, " & vbCrLf &
                    "@Keterangan;" & vbCrLf &
                    "END;" & vbCrLf &
                    "ELSE" & vbCrLf &
                    "BEGIN" & vbCrLf &
                    "UPDATE [dbo].[MSaldoAwalPersediaan]" & vbCrLf &
                    "SET " & vbCrLf &
                    "[IDGudang] = @IDGudang, " & vbCrLf &
                    "[IDBarangD] = @IDBarangD, " & vbCrLf &
                    "[IDBarang] = @IDBarang, " & vbCrLf &
                    "[Kode] = @Kode, " & vbCrLf &
                    "[Tanggal] = @Tanggal, " & vbCrLf &
                    "[Qty] = @Qty, " & vbCrLf &
                    "[IDSatuan] = @IDSatuan, " & vbCrLf &
                    "[Konversi] = @Konversi, " & vbCrLf &
                    "[HargaPokok] = @HargaPokok, " & vbCrLf &
                    "[Jumlah] = ROUND(@HargaPokok * @Qty, 2), " & vbCrLf &
                    "[IDUserEdit] = @IDUserEdit, " & vbCrLf &
                    "[TglEdit] = GETDATE(), " & vbCrLf &
                    "[IsPosted] = @IsPosted, " & vbCrLf &
                    "[IDUserPosted] = CASE" & vbCrLf &
                    "WHEN ISNULL(@IsPosted, 0) = 1" & vbCrLf &
                    "THEN @IDUserPosted" & vbCrLf &
                    "ELSE NULL" & vbCrLf &
                    "END, " & vbCrLf &
                    "[TglPosted] = CASE" & vbCrLf &
                    "WHEN ISNULL(@IsPosted, 0) = 1" & vbCrLf &
                    "THEN GETDATE()" & vbCrLf &
                    "ELSE NULL" & vbCrLf &
                    "END, " & vbCrLf &
                    "[Keterangan] = @Keterangan" & vbCrLf &
                    "WHERE [NoID] = @NoID;" & vbCrLf &
                    "END;" & vbCrLf &
                    "SELECT @NoID AS NoID;" & vbCrLf &
                    "END;"
            Obj = New Model.UpdateDB With {.DBVersion = "spSimpanMSaldoAwalPersediaan_200916", .TglUpdate = CDate("2020-09-16"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "INSERT INTO MJenisTransaksi (ID, Kode, Nama, Keterangan, IsAuto, TERURUT, IDServerAsal, NoUrut) VALUES" & vbCrLf &
                  "(1001, 'SAH', 'Saldo Awal Hutang', 'Saldo Awal Hutang', 1, 0, NULL, 1)," & vbCrLf &
                  "(1002, 'SAP', 'Saldo Awal Piutang', 'Saldo Awal Piutang', 1, 0, NULL, 1)"
            Obj = New Model.UpdateDB With {.DBVersion = "JenisTransaksi_200916", .TglUpdate = CDate("2020-09-16"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE TABLE [dbo].[MSaldoAwalPiutang](" & vbCrLf &
                  "[NoID] [int] NOT NULL," & vbCrLf &
                  "[IDCustomer] [int] NULL," & vbCrLf &
                  "[Kode] [varchar](50) NULL," & vbCrLf &
                  "[Tanggal] [date] NULL," & vbCrLf &
                  "[JatuhTempo] [date] NULL," & vbCrLf &
                  "[KodeReff] [varchar](50) NULL," & vbCrLf &
                  "[Jumlah] [money] NULL," & vbCrLf &
                  "[IDUserEntry] [int] NULL," & vbCrLf &
                  "[TglEntry] [datetime] NULL," & vbCrLf &
                  "[IDUserEdit] [int] NULL," & vbCrLf &
                  "[TglEdit] [datetime] NULL," & vbCrLf &
                  "[IsPosted] [bit] NULL," & vbCrLf &
                  "[IDUserPosted] [int] NULL," & vbCrLf &
                  "[TglPosted] [datetime] NULL," & vbCrLf &
                  "[Keterangan] [varchar](max) NULL," & vbCrLf &
                  "CONSTRAINT [PK_MSaldoAwalPiutang] PRIMARY KEY CLUSTERED " & vbCrLf &
                  "(" & vbCrLf &
                  "[NoID] ASC" & vbCrLf &
                  ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" & vbCrLf &
                  ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"
            Obj = New Model.UpdateDB With {.DBVersion = "MSaldoAwalPiutang_200922", .TglUpdate = CDate("2020-09-22"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE TABLE [dbo].[MSaldoAwalHutang](" & vbCrLf &
                  "[NoID] [int] NOT NULL," & vbCrLf &
                  "[IDSupplier] [int] NULL," & vbCrLf &
                  "[Kode] [varchar](50) NULL," & vbCrLf &
                  "[Tanggal] [date] NULL," & vbCrLf &
                  "[JatuhTempo] [date] NULL," & vbCrLf &
                  "[KodeReff] [varchar](50) NULL," & vbCrLf &
                  "[Jumlah] [money] NULL," & vbCrLf &
                  "[IDUserEntry] [int] NULL," & vbCrLf &
                  "[TglEntry] [datetime] NULL," & vbCrLf &
                  "[IDUserEdit] [int] NULL," & vbCrLf &
                  "[TglEdit] [datetime] NULL," & vbCrLf &
                  "[IsPosted] [bit] NULL," & vbCrLf &
                  "[IDUserPosted] [int] NULL," & vbCrLf &
                  "[TglPosted] [datetime] NULL," & vbCrLf &
                  "[Keterangan] [varchar](max) NULL," & vbCrLf &
                  "CONSTRAINT [PK_MSaldoAwalHutang] PRIMARY KEY CLUSTERED " & vbCrLf &
                  "(" & vbCrLf &
                  "[NoID] ASC" & vbCrLf &
                  ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" & vbCrLf &
                  ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"
            Obj = New Model.UpdateDB With {.DBVersion = "MSaldoAwalHutang_200922", .TglUpdate = CDate("2020-09-22"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "-- =============================================" & vbCrLf &
                  "-- Author: <Author,,Name>" & vbCrLf &
                  "-- Create date: <Create Date,,>" & vbCrLf &
                  "-- Description: <Description,,>" & vbCrLf &
                  "-- =============================================" & vbCrLf &
                  "CREATE PROCEDURE [dbo].[spFakturMSaldoAwalHutang](@TglDari DATETIME = '2019-01-01', @TglSampai DATETIME = '2019-01-31')" & vbCrLf &
                  "AS " & vbCrLf &
                  "BEGIN" & vbCrLf &
                  "SET NOCOUNT ON;" & vbCrLf &
                  "SELECT MSaldoAwalHutang.*, MAlamat.Kode KodeSupplier, MAlamat.Nama NamaSupplier, UserEntry.Nama UserEntry, UserEdit.Nama UserEdit, UserPosted.Nama UserPosted" & vbCrLf &
                  "FROM MSaldoAwalHutang (NOLOCK)" & vbCrLf &
                  "LEFT JOIN MAlamat (NOLOCK) ON MSaldoAwalHutang.IDSupplier=MAlamat.NoID" & vbCrLf &
                  "LEFT JOIN MUser UserEntry (NOLOCK) ON MSaldoAwalHutang.IDUserEntry=UserEntry.NoID" & vbCrLf &
                  "LEFT JOIN MUser UserEdit (NOLOCK) ON MSaldoAwalHutang.IDUserEdit=UserEdit.NoID" & vbCrLf &
                  "LEFT JOIN MUser UserPosted (NOLOCK) ON MSaldoAwalHutang.IDUserPosted=UserPosted.NoID" & vbCrLf &
                  "WHERE MSaldoAwalHutang.IsPosted=1 AND CONVERT(DATE, MSaldoAwalHutang.Tanggal) >= CONVERT(DATE, @TglDari) AND CONVERT(DATE, MSaldoAwalHutang.Tanggal) <= CONVERT(DATE, @TglSampai)" & vbCrLf &
                  "ORDER BY MSaldoAwalHutang.Tanggal, MSaldoAwalHutang.NoID" & vbCrLf &
                  "END"
            Obj = New Model.UpdateDB With {.DBVersion = "spFakturMSaldoAwalHutang_200922", .TglUpdate = CDate("2020-09-22"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "-- =============================================" & vbCrLf &
                  "-- Author: <Author,,Name>" & vbCrLf &
                  "-- Create date: <Create Date,,>" & vbCrLf &
                  "-- Description: <Description,,>" & vbCrLf &
                  "-- =============================================" & vbCrLf &
                  "CREATE PROCEDURE [dbo].[spFakturMSaldoAwalPiutang](@TglDari DATETIME = '2019-01-01', @TglSampai DATETIME = '2019-01-31')" & vbCrLf &
                  "AS " & vbCrLf &
                  "BEGIN" & vbCrLf &
                  "SET NOCOUNT ON;" & vbCrLf &
                  "SELECT MSaldoAwalPiutang.*, MAlamat.Kode KodeCustomer, MAlamat.Nama NamaCustomer, UserEntry.Nama UserEntry, UserEdit.Nama UserEdit, UserPosted.Nama UserPosted" & vbCrLf &
                  "FROM MSaldoAwalPiutang (NOLOCK)" & vbCrLf &
                  "LEFT JOIN MAlamat (NOLOCK) ON MSaldoAwalPiutang.IDCustomer=MAlamat.NoID" & vbCrLf &
                  "LEFT JOIN MUser UserEntry (NOLOCK) ON MSaldoAwalPiutang.IDUserEntry=UserEntry.NoID" & vbCrLf &
                  "LEFT JOIN MUser UserEdit (NOLOCK) ON MSaldoAwalPiutang.IDUserEdit=UserEdit.NoID" & vbCrLf &
                  "LEFT JOIN MUser UserPosted (NOLOCK) ON MSaldoAwalPiutang.IDUserPosted=UserPosted.NoID" & vbCrLf &
                  "WHERE MSaldoAwalPiutang.IsPosted=1 AND CONVERT(DATE, MSaldoAwalPiutang.Tanggal) >= CONVERT(DATE, @TglDari) AND CONVERT(DATE, MSaldoAwalPiutang.Tanggal) <= CONVERT(DATE, @TglSampai)" & vbCrLf &
                  "ORDER BY MSaldoAwalPiutang.Tanggal, MSaldoAwalPiutang.NoID" & vbCrLf &
                  "END"
            Obj = New Model.UpdateDB With {.DBVersion = "spFakturMSaldoAwalPiutang_200922", .TglUpdate = CDate("2020-09-22"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE PROCEDURE [dbo].[spSimpanMSaldoAwalHutang](" & vbCrLf &
                "@NoID         BIGINT, " & vbCrLf &
                "@Kode         VARCHAR(255), " & vbCrLf &
                "@KodeReff     VARCHAR(255), " & vbCrLf &
                "@Tanggal      DATETIME, " & vbCrLf &
                "@JatuhTempo   DATETIME, " & vbCrLf &
                "@Jumlah       MONEY, " & vbCrLf &
                "@Keterangan   VARCHAR(MAX), " & vbCrLf &
                "@IDSupplier   INT, " & vbCrLf &
                "@IsPosted     BIT, " & vbCrLf &
                "@TglPosted    DATETIME, " & vbCrLf &
                "@IDUserPosted INT, " & vbCrLf &
                "@IDUserEntry  INT, " & vbCrLf &
                "@IDUserEdit   INT)" & vbCrLf &
                "AS" & vbCrLf &
                "BEGIN" & vbCrLf &
                "DECLARE @NoUrut AS INT= 0;" & vbCrLf &
                "IF(@NoID <= 0)" & vbCrLf &
                "BEGIN" & vbCrLf &
                "SELECT @NoID = MAX(NoID)" & vbCrLf &
                "FROM MSaldoAwalHutang;" & vbCrLf &
                "SET @NoID = ISNULL(@NoID, 0) + 1;" & vbCrLf &
                "SET @Kode = 'SAH/' + SUBSTRING(CONVERT(VARCHAR(100), @Tanggal, 112), 3, 4) + '/';" & vbCrLf &
                "SELECT @NoUrut = MAX(CASE" & vbCrLf &
                "WHEN ISNUMERIC(RIGHT(Kode, 5)) = 1" & vbCrLf &
                "THEN RIGHT(Kode, 5)" & vbCrLf &
                "ELSE 0" & vbCrLf &
                "END)" & vbCrLf &
                "FROM MSaldoAwalHutang" & vbCrLf &
                "WHERE LEFT(Kode, LEN(@Kode)) = @Kode;" & vbCrLf &
                "SET @NoUrut = ISNULL(@NoUrut, 0) + 1;" & vbCrLf &
                "SET @Kode = @Kode + LEFT('00000', 5 - LEN(CONVERT(VARCHAR(10), @NoUrut))) + CONVERT(VARCHAR(10), @NoUrut);" & vbCrLf &
                "INSERT INTO [dbo].[MSaldoAwalHutang]" & vbCrLf &
                "([NoID], " & vbCrLf &
                "[IDSupplier]," & vbCrLf &
                "[Kode], " & vbCrLf &
                "[Tanggal],  " & vbCrLf &
                "[JatuhTempo]," & vbCrLf &
                "[KodeReff]," & vbCrLf &
                "[Jumlah], " & vbCrLf &
                "[IDUserEntry], " & vbCrLf &
                "[TglEntry], " & vbCrLf &
                "[IDUserEdit], " & vbCrLf &
                "[TglEdit], " & vbCrLf &
                "[IsPosted], " & vbCrLf &
                "[IDUserPosted], " & vbCrLf &
                "[TglPosted], " & vbCrLf &
                "[Keterangan]" & vbCrLf &
                ")" & vbCrLf &
                "SELECT @NoID, " & vbCrLf &
                "@IDSupplier, " & vbCrLf &
                "@Kode, " & vbCrLf &
                "@Tanggal, " & vbCrLf &
                "@JatuhTempo, " & vbCrLf &
                "@KodeReff," & vbCrLf &
                "ROUND(@Jumlah, 2)," & vbCrLf &
                "@IDUserEntry, " & vbCrLf &
                "GETDATE(), " & vbCrLf &
                "NULL, " & vbCrLf &
                "NULL, " & vbCrLf &
                "0, " & vbCrLf &
                "NULL, " & vbCrLf &
                "NULL, " & vbCrLf &
                "@Keterangan;" & vbCrLf &
                "END;" & vbCrLf &
                "ELSE" & vbCrLf &
                "BEGIN" & vbCrLf &
                "UPDATE [dbo].[MSaldoAwalHutang]" & vbCrLf &
                "SET " & vbCrLf &
                "[IDSupplier] = @IDSupplier, " & vbCrLf &
                "[Kode] = @Kode, " & vbCrLf &
                "[KodeReff] = @KodeReff, " & vbCrLf &
                "[Tanggal] = @Tanggal, " & vbCrLf &
                "[JatuhTempo] = @JatuhTempo, " & vbCrLf &
                "[Jumlah] = ROUND(@Jumlah, 2), " & vbCrLf &
                "[IDUserEdit] = @IDUserEdit, " & vbCrLf &
                "[TglEdit] = GETDATE(), " & vbCrLf &
                "[IsPosted] = @IsPosted, " & vbCrLf &
                "[IDUserPosted] = CASE" & vbCrLf &
                "WHEN ISNULL(@IsPosted, 0) = 1" & vbCrLf &
                "THEN @IDUserPosted" & vbCrLf &
                "ELSE NULL" & vbCrLf &
                "END, " & vbCrLf &
                "[TglPosted] = CASE" & vbCrLf &
                "WHEN ISNULL(@IsPosted, 0) = 1" & vbCrLf &
                "THEN GETDATE()" & vbCrLf &
                "ELSE NULL" & vbCrLf &
                "END, " & vbCrLf &
                "[Keterangan] = @Keterangan" & vbCrLf &
                "WHERE [NoID] = @NoID;" & vbCrLf &
                "END;" & vbCrLf &
                "SELECT @NoID AS NoID;" & vbCrLf &
                "END;"
            Obj = New Model.UpdateDB With {.DBVersion = "spSimpanMSaldoAwalHutang_200922", .TglUpdate = CDate("2020-09-22"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE PROCEDURE [dbo].[spSimpanMSaldoAwalPiutang](" & vbCrLf &
                "@NoID         BIGINT, " & vbCrLf &
                "@Kode         VARCHAR(255), " & vbCrLf &
                "@KodeReff     VARCHAR(255), " & vbCrLf &
                "@Tanggal      DATETIME, " & vbCrLf &
                "@JatuhTempo   DATETIME, " & vbCrLf &
                "@Jumlah       MONEY, " & vbCrLf &
                "@Keterangan   VARCHAR(MAX), " & vbCrLf &
                "@IDCustomer   INT, " & vbCrLf &
                "@IsPosted     BIT, " & vbCrLf &
                "@TglPosted    DATETIME, " & vbCrLf &
                "@IDUserPosted INT, " & vbCrLf &
                "@IDUserEntry  INT, " & vbCrLf &
                "@IDUserEdit   INT)" & vbCrLf &
                "AS" & vbCrLf &
                "BEGIN" & vbCrLf &
                "DECLARE @NoUrut AS INT= 0;" & vbCrLf &
                "IF(@NoID <= 0)" & vbCrLf &
                "BEGIN" & vbCrLf &
                "SELECT @NoID = MAX(NoID)" & vbCrLf &
                "FROM MSaldoAwalPiutang;" & vbCrLf &
                "SET @NoID = ISNULL(@NoID, 0) + 1;" & vbCrLf &
                "SET @Kode = 'SAH/' + SUBSTRING(CONVERT(VARCHAR(100), @Tanggal, 112), 3, 4) + '/';" & vbCrLf &
                "SELECT @NoUrut = MAX(CASE" & vbCrLf &
                "WHEN ISNUMERIC(RIGHT(Kode, 5)) = 1" & vbCrLf &
                "THEN RIGHT(Kode, 5)" & vbCrLf &
                "ELSE 0" & vbCrLf &
                "END)" & vbCrLf &
                "FROM MSaldoAwalPiutang" & vbCrLf &
                "WHERE LEFT(Kode, LEN(@Kode)) = @Kode;" & vbCrLf &
                "SET @NoUrut = ISNULL(@NoUrut, 0) + 1;" & vbCrLf &
                "SET @Kode = @Kode + LEFT('00000', 5 - LEN(CONVERT(VARCHAR(10), @NoUrut))) + CONVERT(VARCHAR(10), @NoUrut);" & vbCrLf &
                "INSERT INTO [dbo].[MSaldoAwalPiutang]" & vbCrLf &
                "([NoID], " & vbCrLf &
                "[IDCustomer]," & vbCrLf &
                "[Kode], " & vbCrLf &
                "[Tanggal],  " & vbCrLf &
                "[JatuhTempo]," & vbCrLf &
                "[KodeReff]," & vbCrLf &
                "[Jumlah], " & vbCrLf &
                "[IDUserEntry], " & vbCrLf &
                "[TglEntry], " & vbCrLf &
                "[IDUserEdit], " & vbCrLf &
                "[TglEdit], " & vbCrLf &
                "[IsPosted], " & vbCrLf &
                "[IDUserPosted], " & vbCrLf &
                "[TglPosted], " & vbCrLf &
                "[Keterangan]" & vbCrLf &
                ")" & vbCrLf &
                "SELECT @NoID, " & vbCrLf &
                "@IDCustomer, " & vbCrLf &
                "@Kode, " & vbCrLf &
                "@Tanggal, " & vbCrLf &
                "@JatuhTempo, " & vbCrLf &
                "@KodeReff," & vbCrLf &
                "ROUND(@Jumlah, 2)," & vbCrLf &
                "@IDUserEntry, " & vbCrLf &
                "GETDATE(), " & vbCrLf &
                "NULL, " & vbCrLf &
                "NULL, " & vbCrLf &
                "0, " & vbCrLf &
                "NULL, " & vbCrLf &
                "NULL, " & vbCrLf &
                "@Keterangan;" & vbCrLf &
                "END;" & vbCrLf &
                "ELSE" & vbCrLf &
                "BEGIN" & vbCrLf &
                "UPDATE [dbo].[MSaldoAwalPiutang]" & vbCrLf &
                "SET " & vbCrLf &
                "[IDCustomer] = @IDCustomer, " & vbCrLf &
                "[Kode] = @Kode, " & vbCrLf &
                "[KodeReff] = @KodeReff, " & vbCrLf &
                "[Tanggal] = @Tanggal, " & vbCrLf &
                "[JatuhTempo] = @JatuhTempo, " & vbCrLf &
                "[Jumlah] = ROUND(@Jumlah, 2), " & vbCrLf &
                "[IDUserEdit] = @IDUserEdit, " & vbCrLf &
                "[TglEdit] = GETDATE(), " & vbCrLf &
                "[IsPosted] = @IsPosted, " & vbCrLf &
                "[IDUserPosted] = CASE" & vbCrLf &
                "WHEN ISNULL(@IsPosted, 0) = 1" & vbCrLf &
                "THEN @IDUserPosted" & vbCrLf &
                "ELSE NULL" & vbCrLf &
                "END, " & vbCrLf &
                "[TglPosted] = CASE" & vbCrLf &
                "WHEN ISNULL(@IsPosted, 0) = 1" & vbCrLf &
                "THEN GETDATE()" & vbCrLf &
                "ELSE NULL" & vbCrLf &
                "END, " & vbCrLf &
                "[Keterangan] = @Keterangan" & vbCrLf &
                "WHERE [NoID] = @NoID;" & vbCrLf &
                "END;" & vbCrLf &
                "SELECT @NoID AS NoID;" & vbCrLf &
                "END;"
            Obj = New Model.UpdateDB With {.DBVersion = "spSimpanMSaldoAwalPiutang_200922", .TglUpdate = CDate("2020-09-22"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "DROP PROCEDURE [dbo].[spSaldoAwalPersediaan];"
            Obj = New Model.UpdateDB With {.DBVersion = "spSaldoAwalPersediaan_200922", .TglUpdate = CDate("2020-09-22"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "-- =============================================" & vbCrLf &
                  "-- Author: <Author,,Name>" & vbCrLf &
                  "-- Create date: <Create Date,,>" & vbCrLf &
                  "-- Description: <Description,,>" & vbCrLf &
                  "-- =============================================" & vbCrLf &
                  "CREATE PROCEDURE [dbo].[spDaftarSaldoAwalPersediaan](@TglDari DATETIME = '2019-01-01', @TglSampai DATETIME = '2019-01-31')" & vbCrLf &
                  "AS " & vbCrLf &
                  "BEGIN" & vbCrLf &
                  "SET NOCOUNT ON;" & vbCrLf &
                  "SELECT MSaldoAwalPersediaan.*, MBarang.Kode KodeBarang, MBarang.Nama NamaBarang, MBarangD.Barcode, UserEntry.Nama UserEntry, UserEdit.Nama UserEdit, UserPosted.Nama UserPosted, MGudang.Kode Gudang" & vbCrLf &
                  "FROM MSaldoAwalPersediaan (NOLOCK)" & vbCrLf &
                  "LEFT JOIN MGudang (NOLOCK) ON MSaldoAwalPersediaan.IDGudang=MGudang.NoID" & vbCrLf &
                  "LEFT JOIN MBarang (NOLOCK) ON MSaldoAwalPersediaan.IDBarang=MBarang.NoID" & vbCrLf &
                  "LEFT JOIN MBarangD (NOLOCK) ON MSaldoAwalPersediaan.IDBarangD=MBarangD.NoID" & vbCrLf &
                  "LEFT JOIN MUser UserEntry (NOLOCK) ON MSaldoAwalPersediaan.IDUserEntry=UserEntry.NoID" & vbCrLf &
                  "LEFT JOIN MUser UserEdit (NOLOCK) ON MSaldoAwalPersediaan.IDUserEdit=UserEdit.NoID" & vbCrLf &
                  "LEFT JOIN MUser UserPosted (NOLOCK) ON MSaldoAwalPersediaan.IDUserPosted=UserPosted.NoID" & vbCrLf &
                  "WHERE CONVERT(DATE, MSaldoAwalPersediaan.Tanggal) >= CONVERT(DATE, @TglDari) AND CONVERT(DATE, MSaldoAwalPersediaan.Tanggal) <= CONVERT(DATE, @TglSampai)" & vbCrLf &
                  "ORDER BY MSaldoAwalPersediaan.Tanggal, MSaldoAwalPersediaan.NoID" & vbCrLf &
                  "END;"
            Obj = New Model.UpdateDB With {.DBVersion = "spDaftarSaldoAwalPersediaan_200922", .TglUpdate = CDate("2020-09-22"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "-- =============================================" & vbCrLf &
                "-- Author: <Author,,Name>" & vbCrLf &
                "-- Create date: <Create Date,,>" & vbCrLf &
                "-- Description: <Description,,>" & vbCrLf &
                "-- =============================================" & vbCrLf &
                "CREATE PROCEDURE [dbo].[spDaftarSaldoAwalHutang](@TglDari DATETIME = '2019-01-01', @TglSampai DATETIME = '2019-01-31')" & vbCrLf &
                "AS " & vbCrLf &
                "BEGIN" & vbCrLf &
                "SET NOCOUNT ON;" & vbCrLf &
                "SELECT MSaldoAwalHutang.*, MAlamat.Kode KodeSupplier, MAlamat.Nama NamaSupplier, MAlamat.Alamat AlamatSupplier, UserEntry.Nama UserEntry, UserEdit.Nama UserEdit, UserPosted.Nama UserPosted" & vbCrLf &
                "FROM MSaldoAwalHutang (NOLOCK)" & vbCrLf &
                "LEFT JOIN MAlamat (NOLOCK) ON MSaldoAwalHutang.IDSupplier=MAlamat.NoID" & vbCrLf &
                "LEFT JOIN MUser UserEntry (NOLOCK) ON MSaldoAwalHutang.IDUserEntry=UserEntry.NoID" & vbCrLf &
                "LEFT JOIN MUser UserEdit (NOLOCK) ON MSaldoAwalHutang.IDUserEdit=UserEdit.NoID" & vbCrLf &
                "LEFT JOIN MUser UserPosted (NOLOCK) ON MSaldoAwalHutang.IDUserPosted=UserPosted.NoID" & vbCrLf &
                "WHERE CONVERT(DATE, MSaldoAwalHutang.Tanggal) >= CONVERT(DATE, @TglDari) AND CONVERT(DATE, MSaldoAwalHutang.Tanggal) <= CONVERT(DATE, @TglSampai)" & vbCrLf &
                "ORDER BY MSaldoAwalHutang.Tanggal, MSaldoAwalHutang.NoID" & vbCrLf &
                "END"
            Obj = New Model.UpdateDB With {.DBVersion = "spDaftarSaldoAwalHutang_200922", .TglUpdate = CDate("2020-09-22"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "-- =============================================" & vbCrLf &
                "-- Author: <Author,,Name>" & vbCrLf &
                "-- Create date: <Create Date,,>" & vbCrLf &
                "-- Description: <Description,,>" & vbCrLf &
                "-- =============================================" & vbCrLf &
                "CREATE PROCEDURE [dbo].[spDaftarSaldoAwalPiutang](@TglDari DATETIME = '2019-01-01', @TglSampai DATETIME = '2019-01-31')" & vbCrLf &
                "AS " & vbCrLf &
                "BEGIN" & vbCrLf &
                "SET NOCOUNT ON;" & vbCrLf &
                "SELECT MSaldoAwalPiutang.*, MAlamat.Kode KodeCustomer, MAlamat.Nama NamaCustomer, MAlamat.Alamat AlamatCustomer, UserEntry.Nama UserEntry, UserEdit.Nama UserEdit, UserPosted.Nama UserPosted" & vbCrLf &
                "FROM MSaldoAwalPiutang (NOLOCK)" & vbCrLf &
                "LEFT JOIN MAlamat (NOLOCK) ON MSaldoAwalPiutang.IDCustomer=MAlamat.NoID" & vbCrLf &
                "LEFT JOIN MUser UserEntry (NOLOCK) ON MSaldoAwalPiutang.IDUserEntry=UserEntry.NoID" & vbCrLf &
                "LEFT JOIN MUser UserEdit (NOLOCK) ON MSaldoAwalPiutang.IDUserEdit=UserEdit.NoID" & vbCrLf &
                "LEFT JOIN MUser UserPosted (NOLOCK) ON MSaldoAwalPiutang.IDUserPosted=UserPosted.NoID" & vbCrLf &
                "WHERE CONVERT(DATE, MSaldoAwalPiutang.Tanggal) >= CONVERT(DATE, @TglDari) AND CONVERT(DATE, MSaldoAwalPiutang.Tanggal) <= CONVERT(DATE, @TglSampai)" & vbCrLf &
                "ORDER BY MSaldoAwalPiutang.Tanggal, MSaldoAwalPiutang.NoID" & vbCrLf &
                "END"
            Obj = New Model.UpdateDB With {.DBVersion = "spDaftarSaldoAwalPiutang_200922", .TglUpdate = CDate("2020-09-22"), .SQL = SQL}
            Hasil.Add(Obj)

            ListSQL = New List(Of String)
            ListSQL.Add("CREATE TABLE MKasIN" & vbCrLf &
                    "(NoID        INT NOT NULL, " & vbCrLf &
                    "Tanggal      DATETIME, " & vbCrLf &
                    "IDKontak     INT, " & vbCrLf &
                    "Kode         VARCHAR(50), " & vbCrLf &
                    "KodeReff     VARCHAR(50), " & vbCrLf &
                    "Keterangan   VARCHAR(MAX), " & vbCrLf &
                    "Total        MONEY, " & vbCrLf &
                    "IDUserEntry  INT," & vbCrLf &
                    "TglEntry     DATETIME," & vbCrLf &
                    "IDUserEdit   INT," & vbCrLf &
                    "TglEdit      DATETIME," & vbCrLf &
                    "IsPosted     BIT," & vbCrLf &
                    "IDUserPosted INT," & vbCrLf &
                    "TglPosted    DATETIME," & vbCrLf &
                    "CONSTRAINT [PK_MKasIN] PRIMARY KEY CLUSTERED([NoID] ASC)" & vbCrLf &
                    "WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], " & vbCrLf &
                    ")" & vbCrLf &
                    "ON [PRIMARY];")
            ListSQL.Add("CREATE UNIQUE NONCLUSTERED INDEX [IX_Kode] ON [dbo].[MKasIN]" & vbCrLf &
                        "([Kode] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]")
            ListSQL.Add("CREATE TABLE [dbo].[MKasIND](" & vbCrLf &
                    "[IDKasIN] [int] NOT NULL," & vbCrLf &
                    "[NoUrut] [int] NOT NULL," & vbCrLf &
                    "[ReffNoTransaksi] [varchar](50) NULL," & vbCrLf &
                    "[ReffNoUrut] [smallint] NULL," & vbCrLf &
                    "[Debet] [money] NULL," & vbCrLf &
                    "[Kredit] [money] NULL," & vbCrLf &
                    "[Keterangan] [varchar](max) NULL," & vbCrLf &
                    "CONSTRAINT [PK_MKasIND] PRIMARY KEY CLUSTERED " & vbCrLf &
                    "(" & vbCrLf &
                    "[IDKasIN] ASC," & vbCrLf &
                    "[NoUrut] ASC" & vbCrLf &
                    ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" & vbCrLf &
                    ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];")
            ListSQL.Add("CREATE TABLE [dbo].[MKasINDBayar](" & vbCrLf &
                    "[IDKasIN] [int] NOT NULL," & vbCrLf &
                    "[IDJenisPembayaran] [int] NOT NULL," & vbCrLf &
                    "[AtasNama] [varchar](50) NULL," & vbCrLf &
                    "[NoRekening] [varchar](150) NULL," & vbCrLf &
                    "[Nominal] [money] NOT NULL," & vbCrLf &
                    "[ChargeProsen] [numeric](18, 3) NULL," & vbCrLf &
                    "[ChargeRp] [money] NULL," & vbCrLf &
                    "[Total] [money] NULL," & vbCrLf &
                    "CONSTRAINT [PK_MKasINDBayar] PRIMARY KEY CLUSTERED " & vbCrLf &
                    "(" & vbCrLf &
                    "[IDKasIN] ASC," & vbCrLf &
                    "[IDJenisPembayaran] ASC" & vbCrLf &
                    ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" & vbCrLf &
                    ") ON [PRIMARY];")
            ListSQL.Add("ALTER TABLE [dbo].[MKasINDBayar] ADD  CONSTRAINT [DF_MKasINDBayar_Nominal]  DEFAULT ((0)) FOR [Nominal];")
            ListSQL.Add("ALTER TABLE [dbo].[MKasINDBayar] ADD  CONSTRAINT [DF_MKasINDBayar_ChargeProsen]  DEFAULT ((0)) FOR [ChargeProsen];")
            ListSQL.Add("ALTER TABLE [dbo].[MKasINDBayar] ADD  CONSTRAINT [DF_MKasINDBayar_ChargeRp]  DEFAULT ((0)) FOR [ChargeRp];")
            ListSQL.Add("ALTER TABLE [dbo].[MKasINDBayar] ADD  CONSTRAINT [DF_MKasINDBayar_Total]  DEFAULT ((0)) FOR [Total];")
            Obj = New Model.UpdateDB With {.DBVersion = "MKasIN_200922", .TglUpdate = CDate("2020-09-22"), .SQL = "", .ListSQL = ListSQL}
            Hasil.Add(Obj)

            ListSQL = New List(Of String)
            ListSQL.Add("CREATE TABLE MKasOut" & vbCrLf &
                    "(NoID        INT NOT NULL, " & vbCrLf &
                    "Tanggal      DATETIME, " & vbCrLf &
                    "IDKontak     INT, " & vbCrLf &
                    "Kode         VARCHAR(50), " & vbCrLf &
                    "KodeReff     VARCHAR(50), " & vbCrLf &
                    "Keterangan   VARCHAR(MAX), " & vbCrLf &
                    "Total        MONEY, " & vbCrLf &
                    "IDUserEntry  INT," & vbCrLf &
                    "TglEntry     DATETIME," & vbCrLf &
                    "IDUserEdit   INT," & vbCrLf &
                    "TglEdit      DATETIME," & vbCrLf &
                    "IsPosted     BIT," & vbCrLf &
                    "IDUserPosted INT," & vbCrLf &
                    "TglPosted    DATETIME," & vbCrLf &
                    "CONSTRAINT [PK_MKasOut] PRIMARY KEY CLUSTERED([NoID] ASC)" & vbCrLf &
                    "WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], " & vbCrLf &
                    ")" & vbCrLf &
                    "ON [PRIMARY];")
            ListSQL.Add("CREATE UNIQUE NONCLUSTERED INDEX [IX_Kode] ON [dbo].[MKasOut]" & vbCrLf &
                        "([Kode] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]")
            ListSQL.Add("CREATE TABLE [dbo].[MKasOutD](" & vbCrLf &
                    "[IDKasOut] [int] NOT NULL," & vbCrLf &
                    "[NoUrut] [int] NOT NULL," & vbCrLf &
                    "[ReffNoTransaksi] [varchar](50) NULL," & vbCrLf &
                    "[ReffNoUrut] [smallint] NULL," & vbCrLf &
                    "[Debet] [money] NULL," & vbCrLf &
                    "[Kredit] [money] NULL," & vbCrLf &
                    "[Keterangan] [varchar](max) NULL," & vbCrLf &
                    "CONSTRAINT [PK_MKasOutD] PRIMARY KEY CLUSTERED " & vbCrLf &
                    "(" & vbCrLf &
                    "[IDKasOut] ASC," & vbCrLf &
                    "[NoUrut] ASC" & vbCrLf &
                    ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" & vbCrLf &
                    ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];")
            ListSQL.Add("CREATE TABLE [dbo].[MKasOutDBayar](" & vbCrLf &
                    "[IDKasOut] [int] NOT NULL," & vbCrLf &
                    "[IDJenisPembayaran] [int] NOT NULL," & vbCrLf &
                    "[AtasNama] [varchar](50) NULL," & vbCrLf &
                    "[NoRekening] [varchar](150) NULL," & vbCrLf &
                    "[Nominal] [money] NOT NULL," & vbCrLf &
                    "[ChargeProsen] [numeric](18, 3) NULL," & vbCrLf &
                    "[ChargeRp] [money] NULL," & vbCrLf &
                    "[Total] [money] NULL," & vbCrLf &
                    "CONSTRAINT [PK_MKasOutDBayar] PRIMARY KEY CLUSTERED " & vbCrLf &
                    "(" & vbCrLf &
                    "[IDKasOut] ASC," & vbCrLf &
                    "[IDJenisPembayaran] ASC" & vbCrLf &
                    ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" & vbCrLf &
                    ") ON [PRIMARY];")
            ListSQL.Add("ALTER TABLE [dbo].[MKasOutDBayar] ADD  CONSTRAINT [DF_MKasOutDBayar_Nominal]  DEFAULT ((0)) FOR [Nominal];")
            ListSQL.Add("ALTER TABLE [dbo].[MKasOutDBayar] ADD  CONSTRAINT [DF_MKasOutDBayar_ChargeProsen]  DEFAULT ((0)) FOR [ChargeProsen];")
            ListSQL.Add("ALTER TABLE [dbo].[MKasOutDBayar] ADD  CONSTRAINT [DF_MKasOutDBayar_ChargeRp]  DEFAULT ((0)) FOR [ChargeRp];")
            ListSQL.Add("ALTER TABLE [dbo].[MKasOutDBayar] ADD  CONSTRAINT [DF_MKasOutDBayar_Total]  DEFAULT ((0)) FOR [Total];")
            Obj = New Model.UpdateDB With {.DBVersion = "MKasOut_200922", .TglUpdate = CDate("2020-09-22"), .SQL = "", .ListSQL = ListSQL}
            Hasil.Add(Obj)

            Return Hasil
        End Function

        Private Shared Function ListUpdate_Juni_2021() As List(Of Model.UpdateDB)
            Dim Hasil As New List(Of Model.UpdateDB)
            Dim Obj As New Model.UpdateDB
            Dim SQL As String = ""
            Dim ListSQL As List(Of String) = Nothing

            SQL = "CREATE TABLE dbo.MMaterial (" & vbCrLf &
                  "NoID uniqueidentifier Not NULL," & vbCrLf &
                  "Kode varchar(50) NOT NULL," & vbCrLf &
                  "Nama varchar(150) NOT NULL," & vbCrLf &
                  "Keterangan varchar(150) NOT NULL," & vbCrLf &
                  "IsActive bit NOT NULL CONSTRAINT DF_MMaterial_IsActive DEFAULT ((0))," & vbCrLf &
                  "IDBarangD bigint NOT NULL," & vbCrLf &
                  "IDBarang bigint NOT NULL," & vbCrLf &
                  "IDSatuan int NOT NULL," & vbCrLf &
                  "Konversi numeric NOT NULL," & vbCrLf &
                  "Qty numeric(18, 3) NOT NULL," & vbCrLf &
                  "HargaPokok money NOT NULL," & vbCrLf &
                  "Jumlah money NOT NULL," & vbCrLf &
                  "IDUser int NOT NULL," & vbCrLf &
                  "TanggalUpdate datetime NOT NULL," & vbCrLf &
                  "CONSTRAINT PK_MMaterial PRIMARY KEY (NoID)" & vbCrLf &
                  ");" & vbCrLf &
                  "CREATE UNIQUE INDEX IX_Kode" & vbCrLf &
                  "ON dbo.MMaterial (Kode)"
            Obj = New Model.UpdateDB With {.DBVersion = "MMaterial_210623", .TglUpdate = CDate("2021-06-23"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE TABLE dbo.MWOAssembly (" & vbCrLf &
                  "NoID uniqueidentifier NOT NULL," & vbCrLf &
                  "Kode varchar(50) NOT NULL," & vbCrLf &
                  "Tanggal datetime NOT NULL," & vbCrLf &
                  "TanggalWO datetime NOT NULL," & vbCrLf &
                  "IDMaterial uniqueidentifier NOT NULL," & vbCrLf &
                  "IDPegawai bigint NOT NULL," & vbCrLf &
                  "IDGudang int NOT NULL," & vbCrLf &
                  "Catatan varchar(150) NOT NULL," & vbCrLf &
                  "IDBarangD bigint NOT NULL," & vbCrLf &
                  "IDBarang bigint NOT NULL," & vbCrLf &
                  "IDSatuan int NOT NULL," & vbCrLf &
                  "Konversi numeric NOT NULL," & vbCrLf &
                  "Qty numeric(18, 3) NOT NULL," & vbCrLf &
                  "HargaPokok money NOT NULL," & vbCrLf &
                  "Jumlah money NOT NULL," & vbCrLf &
                  "IsPosted bit NULL," & vbCrLf &
                  "TglPosted datetime NULL," & vbCrLf &
                  "IDUserPosted int NULL," & vbCrLf &
                  "IDUserEntry int NULL," & vbCrLf &
                  "IDUserEdit int NULL," & vbCrLf &
                  "CONSTRAINT PK_MWOAssembly PRIMARY KEY (NoID)," & vbCrLf &
                  "CONSTRAINT FK_MWOAssembly_MMaterial FOREIGN KEY (IDMaterial) REFERENCES dbo.MMaterial (NoID) ON UPDATE CASCADE" & vbCrLf &
                  ");" & vbCrLf &
                  "CREATE UNIQUE INDEX IX_Kode" & vbCrLf &
                  "ON dbo.MWOAssembly (Kode)"
            Obj = New Model.UpdateDB With {.DBVersion = "MWOAssembly_210623", .TglUpdate = CDate("2021-06-23"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE TABLE dbo.MAssembly (" & vbCrLf &
                  "NoID bigint NOT NULL," & vbCrLf &
                  "Kode varchar(50) NOT NULL," & vbCrLf &
                  "Tanggal datetime NOT NULL," & vbCrLf &
                  "IDWOAssembly uniqueidentifier NULL," & vbCrLf &
                  "IDMaterial uniqueidentifier NOT NULL," & vbCrLf &
                  "IDPegawai bigint NOT NULL," & vbCrLf &
                  "IDGudang int NOT NULL," & vbCrLf &
                  "Catatan varchar(150) NOT NULL," & vbCrLf &
                  "IDBarangD bigint NOT NULL," & vbCrLf &
                  "IDBarang bigint NOT NULL," & vbCrLf &
                  "IDSatuan int NOT NULL," & vbCrLf &
                  "Konversi numeric NOT NULL," & vbCrLf &
                  "Qty numeric(18, 3) NOT NULL," & vbCrLf &
                  "HargaPokok money NOT NULL," & vbCrLf &
                  "Jumlah money NOT NULL," & vbCrLf &
                  "IsPosted bit NULL," & vbCrLf &
                  "TglPosted datetime NULL," & vbCrLf &
                  "IDUserPosted int NULL," & vbCrLf &
                  "IDUserEntry int NULL," & vbCrLf &
                  "IDUserEdit int NULL," & vbCrLf &
                  "CONSTRAINT PK_MAssembly PRIMARY KEY (NoID)," & vbCrLf &
                  "CONSTRAINT FK_MAssembly_MMaterial FOREIGN KEY (IDMaterial) REFERENCES dbo.MMaterial (NoID)," & vbCrLf &
                  "CONSTRAINT FK_MAssembly_MWOAssembly FOREIGN KEY (IDWOAssembly) REFERENCES dbo.MWOAssembly (NoID)" & vbCrLf &
                  ");" & vbCrLf &
                  "CREATE UNIQUE INDEX IX_Kode" & vbCrLf &
                  "ON dbo.MAssembly (Kode)"
            Obj = New Model.UpdateDB With {.DBVersion = "MAssembly_210623", .TglUpdate = CDate("2021-06-23"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE TABLE dbo.MMaterialDSisa (" & vbCrLf &
                  "NoID uniqueidentifier NOT NULL," & vbCrLf &
                  "IDMaterial uniqueidentifier NOT NULL," & vbCrLf &
                  "IDBarangD bigint NOT NULL," & vbCrLf &
                  "IDBarang bigint NOT NULL," & vbCrLf &
                  "IDSatuan int NOT NULL," & vbCrLf &
                  "Konversi numeric NOT NULL," & vbCrLf &
                  "Qty numeric(18, 3) NOT NULL," & vbCrLf &
                  "HargaPokok money NOT NULL," & vbCrLf &
                  "Jumlah money NOT NULL," & vbCrLf &
                  "CONSTRAINT PK_MMaterialDSisa PRIMARY KEY (NoID)," & vbCrLf &
                  "CONSTRAINT FK_MMaterialDSisa_MMaterial FOREIGN KEY (IDMaterial) REFERENCES dbo.MMaterial (NoID) ON DELETE CASCADE ON UPDATE CASCADE" & vbCrLf &
                  ");" & vbCrLf &
                  "CREATE UNIQUE INDEX IX_IDMaterial" & vbCrLf &
                  "ON dbo.MMaterialDSisa (IDMaterial, IDBarangD)"
            Obj = New Model.UpdateDB With {.DBVersion = "MMaterialDSisa_210623", .TglUpdate = CDate("2021-06-23"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE TABLE dbo.MAssemblyDSisa (" & vbCrLf &
                  "NoID bigint NOT NULL," & vbCrLf &
                  "IDHeader bigint NOT NULL," & vbCrLf &
                  "IDMaterialDSisa uniqueidentifier NOT NULL," & vbCrLf &
                  "Tanggal datetime NOT NULL," & vbCrLf &
                  "IDBarangD bigint NOT NULL," & vbCrLf &
                  "IDBarang bigint NOT NULL," & vbCrLf &
                  "IDSatuan int NOT NULL," & vbCrLf &
                  "Konversi numeric NOT NULL," & vbCrLf &
                  "Qty numeric(18, 3) NOT NULL," & vbCrLf &
                  "HargaPokok money NOT NULL," & vbCrLf &
                  "Jumlah money NOT NULL," & vbCrLf &
                  "IsPosted bit NULL," & vbCrLf &
                  "TglPosted datetime NULL," & vbCrLf &
                  "IDUserPosted int NULL," & vbCrLf &
                  "IDUserEntry int NULL," & vbCrLf &
                  "IDUserEdit int NULL," & vbCrLf &
                  "CONSTRAINT PK_MAssemblyDSisa PRIMARY KEY (NoID, IDHeader, IDMaterialDSisa)," & vbCrLf &
                  "CONSTRAINT FK_MAssemblyDSisa_MAssemblyDSisa FOREIGN KEY (IDHeader) REFERENCES dbo.MAssembly (NoID) ON UPDATE CASCADE," & vbCrLf &
                  "CONSTRAINT FK_MAssemblyDSisa_MMaterialDSisa FOREIGN KEY (IDMaterialDSisa) REFERENCES dbo.MMaterialDSisa (NoID) ON UPDATE CASCADE" & vbCrLf &
                  ")"
            Obj = New Model.UpdateDB With {.DBVersion = "MAssemblyDSisa_210623", .TglUpdate = CDate("2021-06-23"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE TABLE dbo.MMaterialDBiaya (" & vbCrLf &
                  "NoID uniqueidentifier NOT NULL," & vbCrLf &
                  "IDMaterial uniqueidentifier NOT NULL," & vbCrLf &
                  "IDAkun UNIQUEIDENTIFIER NOT NULL," & vbCrLf &
                  "Jumlah money NOT NULL," & vbCrLf &
                  "Keterangan varchar(150) NULL," & vbCrLf &
                  "CONSTRAINT PK_MMaterialDBiaya PRIMARY KEY (NoID)," & vbCrLf &
                  "CONSTRAINT FK_MMaterialDBiaya_MMaterial FOREIGN KEY (IDMaterial) REFERENCES dbo.MMaterial (NoID) ON DELETE CASCADE ON UPDATE CASCADE" & vbCrLf &
                  ")"
            Obj = New Model.UpdateDB With {.DBVersion = "MMaterialDBiaya_210623", .TglUpdate = CDate("2021-06-23"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE TABLE dbo.MAssemblyDBiaya (" & vbCrLf &
                  "NoID bigint NOT NULL," & vbCrLf &
                  "IDHeader bigint NOT NULL," & vbCrLf &
                  "IDMaterialDBiaya uniqueidentifier NOT NULL," & vbCrLf &
                  "Tanggal datetime NOT NULL," & vbCrLf &
                  "IDAkun UNIQUEIDENTIFIER NOT NULL," & vbCrLf &
                  "Jumlah money NOT NULL," & vbCrLf &
                  "Keterangan varchar(150) NULL," & vbCrLf &
                  "IsPosted bit NULL," & vbCrLf &
                  "TglPosted datetime NULL," & vbCrLf &
                  "IDUserPosted int NULL," & vbCrLf &
                  "IDUserEntry int NULL," & vbCrLf &
                  "IDUserEdit int NULL," & vbCrLf &
                  "CONSTRAINT PK_MAssemblyDBiaya PRIMARY KEY (NoID, IDHeader, IDMaterialDBiaya)," & vbCrLf &
                  "CONSTRAINT FK_MAssemblyDBiaya_MAssembly FOREIGN KEY (IDHeader) REFERENCES dbo.MAssembly (NoID) ON UPDATE CASCADE," & vbCrLf &
                  "CONSTRAINT FK_MAssemblyDBiaya_MMaterialDBiaya FOREIGN KEY (IDMaterialDBiaya) REFERENCES dbo.MMaterialDBiaya (NoID) ON UPDATE CASCADE" & vbCrLf &
                  ")"
            Obj = New Model.UpdateDB With {.DBVersion = "MAssemblyDBiaya_210623", .TglUpdate = CDate("2021-06-23"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE TABLE dbo.MMaterialD (" & vbCrLf &
                  "NoID uniqueidentifier NOT NULL," & vbCrLf &
                  "IDMaterial uniqueidentifier NOT NULL," & vbCrLf &
                  "IDBarangD bigint NOT NULL," & vbCrLf &
                  "IDBarang bigint NOT NULL," & vbCrLf &
                  "IDSatuan int NOT NULL," & vbCrLf &
                  "Konversi numeric NOT NULL," & vbCrLf &
                  "Qty numeric(18, 3) NOT NULL," & vbCrLf &
                  "HargaPokok money NOT NULL," & vbCrLf &
                  "Jumlah money NOT NULL," & vbCrLf &
                  "CONSTRAINT PK_MMaterialD PRIMARY KEY (NoID)," & vbCrLf &
                  "CONSTRAINT FK_MMaterialD_MMaterial FOREIGN KEY (IDMaterial) REFERENCES dbo.MMaterial (NoID) ON DELETE CASCADE ON UPDATE CASCADE" & vbCrLf &
                  ");" & vbCrLf &
                  "CREATE UNIQUE INDEX IX_IDMaterial" & vbCrLf &
                  "ON dbo.MMaterialD (IDBarangD, IDMaterial)"
            Obj = New Model.UpdateDB With {.DBVersion = "MMaterialD_210623", .TglUpdate = CDate("2021-06-23"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE TABLE dbo.MAssemblyD (" & vbCrLf &
                  "NoID bigint NOT NULL," & vbCrLf &
                  "IDHeader bigint NOT NULL," & vbCrLf &
                  "IDMaterialD uniqueidentifier NOT NULL," & vbCrLf &
                  "IDBarangD bigint NOT NULL," & vbCrLf &
                  "Tanggal datetime NOT NULL," & vbCrLf &
                  "IDBarang bigint NOT NULL," & vbCrLf &
                  "IDSatuan int NOT NULL," & vbCrLf &
                  "Konversi numeric NOT NULL," & vbCrLf &
                  "Qty numeric(18, 3) NOT NULL," & vbCrLf &
                  "HargaPokok money NOT NULL," & vbCrLf &
                  "Jumlah money NOT NULL," & vbCrLf &
                  "IsPosted bit NULL," & vbCrLf &
                  "TglPosted datetime NULL," & vbCrLf &
                  "IDUserPosted int NULL," & vbCrLf &
                  "IDUserEntry int NULL," & vbCrLf &
                  "IDUserEdit int NULL," & vbCrLf &
                  "CONSTRAINT PK_MAssemblyD PRIMARY KEY (NoID, IDHeader, IDMaterialD)," & vbCrLf &
                  "CONSTRAINT FK_MAssemblyD_MAssembly FOREIGN KEY (IDHeader) REFERENCES dbo.MAssembly (NoID) ON UPDATE CASCADE," & vbCrLf &
                  "CONSTRAINT FK_MAssemblyD_MMaterialD FOREIGN KEY (IDMaterialD) REFERENCES dbo.MMaterialD (NoID) ON UPDATE CASCADE" & vbCrLf &
                  ")"
            Obj = New Model.UpdateDB With {.DBVersion = "MAssemblyD_210623", .TglUpdate = CDate("2021-06-23"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE dbo.spFakturMJual @NoID BIGINT = -1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT dbo.MJualD.NoID, dbo.MJualD.Konversi, dbo.MJualD.Qty, dbo.MJualD.Harga, dbo.MJualD.DiscProsen1, dbo.MJualD.DiscProsen2, dbo.MJualD.DiscProsen3, dbo.MJualD.DiscProsen4, dbo.MJualD.DiscProsen5, dbo.MJualD.DiscRp, 
		dbo.MJualD.JumlahBruto, dbo.MJualD.DPP, dbo.MJualD.PPN, dbo.MJualD.Jumlah, dbo.MJual.Kode, UserEntry.Nama AS UserEntry, UserEdit.Nama AS UserEdit, UserPosted.Nama AS UserPosted, dbo.MAlamat.Kode AS KodeCustomer, 
		dbo.MAlamat.Nama AS NamaCustomer, dbo.MTypePajak.TypePajak, dbo.MJual.NoReff, dbo.MJual.Tanggal, dbo.MJual.Catatan, dbo.MJual.Subtotal, dbo.MJual.DiscNotaProsen, dbo.MJual.DiscNotaRp, 
		dbo.MJual.TotalBruto, dbo.MJual.DPP AS DPPHeader, dbo.MJual.PPN AS PPNHeader, dbo.MJual.Total, dbo.MJual.IsPosted, dbo.MJual.TglPosted, dbo.MJual.JatuhTempo, dbo.MJualD.IDHeader, dbo.MBarang.Kode AS KodeBarang, 
		dbo.MBarang.Nama AS NamaBarang, dbo.MBarangD.Barcode, dbo.MSatuan.Kode AS Satuan, dbo.MJual.Sisa, dbo.MJual.Bayar
	FROM ((dbo.MJual 
		INNER JOIN dbo.MJualD ON dbo.MJualD.IDHeader = dbo.MJual.NoID) 
		LEFT JOIN dbo.MBarangD ON dbo.MJualD.IDBarangD = dbo.MBarangD.NoID) 
		LEFT JOIN dbo.MSatuan ON dbo.MJualD.IDSatuan = dbo.MSatuan.NoID
		LEFT JOIN dbo.MBarang ON dbo.MJualD.IDBarang = dbo.MBarang.NoID 
		LEFT JOIN dbo.MTypePajak ON dbo.MJual.IDTypePajak = dbo.MTypePajak.NoID 
		LEFT JOIN dbo.MAlamat ON dbo.MAlamat.NoID = dbo.MJual.IDCustomer 
		LEFT JOIN dbo.MUser AS UserPosted ON UserPosted.NoID = dbo.MJual.IDUserPosted
		LEFT JOIN dbo.MUser AS UserEntry ON dbo.MJual.IDUserEntry = UserEntry.NoID
		LEFT JOIN dbo.MUser AS UserEdit ON dbo.MJual.IDUserEdit = UserEdit.NoID
	WHERE dbo.MJual.IsPosted = 1 AND dbo.MJual.NoID = @NoID

	SELECT MJualDBayar.*, MJenisPembayaran.Kode KodePembayaran, MJenisPembayaran.Nama NamaPembayaran 
	FROM MJualDBayar(NOLOCK)
	LEFT JOIN MJenisPembayaran(NOLOCK) ON MJualDBayar.IDJenisPembayaran=MJenisPembayaran.NoID
	WHERE MJualDBayar.IDHeader=@NoID
	ORDER BY MJualDBayar.NoID
END"
            Obj = New Model.UpdateDB With {.DBVersion = "spFakturMJual_210623", .TglUpdate = CDate("2021-06-23"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE TABLE [dbo].[MKlasAkun](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[NoUrut] [int] NULL,
	[Klasifikasi] [varchar](150) NULL,
	[Debet] [bit] NULL,
	[Neraca] [bit] NULL,
 CONSTRAINT [PK_MKlasAkun] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];" & vbCrLf &
"ALTER TABLE [dbo].[MKlasAkun] ADD  CONSTRAINT [DF_MKlasAkun_ID]  DEFAULT (newid()) FOR [ID]"
            Obj = New Model.UpdateDB With {.DBVersion = "MKlasAkun_210627", .TglUpdate = CDate("2021-06-27"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE TABLE [dbo].[MAkun](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[IDKlasAkun] [uniqueidentifier] NOT NULL,
	[IDParent] [uniqueidentifier] NULL,
	[IDDepartemen] [int] NULL,
	[Kode] [varchar](50) NULL,
	[Nama] [varchar](100) NULL,
	[Keterangan] [varchar](255) NULL,
	[IDType] [int] NULL,
	[IsDebet] [bit] NULL,
	[IsKasBank] [bit] NULL,
	[NoRekening] [varchar](50) NULL,
	[AtasNamaRekening] [varchar](255) NULL,
	[IDTypeBank] [int] NULL,
	[IsNeraca] [bit] NULL,
 CONSTRAINT [PK_MAkun] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];" & vbCrLf &
            "ALTER TABLE [dbo].[MAkun] ADD  CONSTRAINT [DF_MAkun_ID]  DEFAULT (newid()) FOR [ID];" & vbCrLf &
            "ALTER TABLE [dbo].[MAkun] ADD  CONSTRAINT [DF_MAkun_IDDepartemen]  DEFAULT ((1)) FOR [IDDepartemen];" & vbCrLf &
            "ALTER TABLE [dbo].[MAkun] ADD  CONSTRAINT [DF_MAkun_IDType]  DEFAULT ((0)) FOR [IDType];" & vbCrLf &
            "ALTER TABLE [dbo].[MAkun] ADD  CONSTRAINT [DF_MAkun_IsDebet]  DEFAULT ((0)) FOR [IsDebet];" & vbCrLf &
            "ALTER TABLE [dbo].[MAkun] ADD  CONSTRAINT [DF_MAkun_IsKasBank]  DEFAULT ((0)) FOR [IsKasBank];" & vbCrLf &
            "ALTER TABLE [dbo].[MAkun] ADD  CONSTRAINT [DF_MAkun_IDTypeBank]  DEFAULT ((0)) FOR [IDTypeBank];" & vbCrLf &
            "ALTER TABLE [dbo].[MAkun] ADD  CONSTRAINT [DF_MAkun_IsNeraca]  DEFAULT ((0)) FOR [IsNeraca];" & vbCrLf &
            "ALTER TABLE [dbo].[MAkun]  WITH CHECK ADD  CONSTRAINT [FK_MAkun_MKlasAkun] FOREIGN KEY([IDKlasAkun])
             REFERENCES [dbo].[MKlasAkun] ([ID]);" & vbCrLf &
            "ALTER TABLE [dbo].[MAkun] CHECK CONSTRAINT [FK_MAkun_MKlasAkun];"
            Obj = New Model.UpdateDB With {.DBVersion = "MAkun_210627", .TglUpdate = CDate("2021-06-27"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "INSERT dbo.MKlasAkun(ID, NoUrut, Klasifikasi, Debet, Neraca) VALUES ('6c43cf12-8c53-4057-be4a-01844f3a85a5', 17, N'Aktiva Lainnya', 1, 1);
INSERT dbo.MKlasAkun(ID, NoUrut, Klasifikasi, Debet, Neraca) VALUES ('a937c0d2-33bc-4771-a8c9-074a9ee42051', 21, N'Akun Hutang', 0, 1);
INSERT dbo.MKlasAkun(ID, NoUrut, Klasifikasi, Debet, Neraca) VALUES ('c8778972-3568-486c-a098-1256e1e4b7d9', 14, N'Aktiva Lancar Lainnya', 1, 1);
INSERT dbo.MKlasAkun(ID, NoUrut, Klasifikasi, Debet, Neraca) VALUES ('490fa648-4575-4700-844f-1b9a7d4872ff', 13, N'Persediaan', 1, 1);
INSERT dbo.MKlasAkun(ID, NoUrut, Klasifikasi, Debet, Neraca) VALUES ('ba07e7fc-38ee-43ef-95cd-3edd1fc1db88', 16, N'Depresiasi & Amortisasi', 1, 1);
INSERT dbo.MKlasAkun(ID, NoUrut, Klasifikasi, Debet, Neraca) VALUES ('97620fd7-9bb5-4a4c-add0-5595a28335c1', 22, N'Kewajiban Lancar Lainnya', 0, 1);
INSERT dbo.MKlasAkun(ID, NoUrut, Klasifikasi, Debet, Neraca) VALUES ('07be4329-0f11-4bf4-ac4d-7987055e6d53', 24, N'Ekuitas', 0, 1);
INSERT dbo.MKlasAkun(ID, NoUrut, Klasifikasi, Debet, Neraca) VALUES ('6287c827-c1e8-4edd-980c-837e37ba9a78', 29, N'Beban Lainnya', 0, 0);
INSERT dbo.MKlasAkun(ID, NoUrut, Klasifikasi, Debet, Neraca) VALUES ('130bb3a6-74d1-40aa-be1f-97c2e8566581', 28, N'Pendapatan Lainnya', 0, 0);
INSERT dbo.MKlasAkun(ID, NoUrut, Klasifikasi, Debet, Neraca) VALUES ('fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', 27, N'Beban', 0, 0);
INSERT dbo.MKlasAkun(ID, NoUrut, Klasifikasi, Debet, Neraca) VALUES ('704384af-a23e-4b85-83db-af24866c9642', 11, N'Kas & Bank', 1, 1);
INSERT dbo.MKlasAkun(ID, NoUrut, Klasifikasi, Debet, Neraca) VALUES ('4d92ca55-798b-4d74-9aef-bd9216b1979d', 23, N'Kewajiban Jangka Panjang', 0, 1);
INSERT dbo.MKlasAkun(ID, NoUrut, Klasifikasi, Debet, Neraca) VALUES ('202ed6ce-8281-4ce3-856c-be8c8ea5121a', 25, N'Pendapatan', 0, 0);
INSERT dbo.MKlasAkun(ID, NoUrut, Klasifikasi, Debet, Neraca) VALUES ('b5375610-13d0-4e17-981a-bf5e18dccf4d', 26, N'Harga Pokok Penjualan', 0, 0);
INSERT dbo.MKlasAkun(ID, NoUrut, Klasifikasi, Debet, Neraca) VALUES ('21b07be0-954d-43f3-9d58-cd7ac615878f', 15, N'Aktiva Tetap', 1, 1);
INSERT dbo.MKlasAkun(ID, NoUrut, Klasifikasi, Debet, Neraca) VALUES ('62fadc5c-923f-4c96-af9a-cddf8f5e6013', 12, N'Akun Piutang', 1, 1);"
            Obj = New Model.UpdateDB With {.DBVersion = "Isi_MKlasAkun_210627", .TglUpdate = CDate("2021-06-27"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('da63901f-4f8f-492e-8229-0451e5e56b26', 'c8778972-3568-486c-a098-1256e1e4b7d9', NULL, 1, N'1-10401', N'Aset Lancar Lainnya', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('a3e29934-eca9-497b-8750-05ab3b542ff8', '97620fd7-9bb5-4a4c-add0-5595a28335c1', NULL, 1, N'2-20202', N'Hutang Deviden', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('25df4334-1885-4f52-91e8-05fbc740c572', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', NULL, 1, N'6-60200', N'Donasi', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('dda98832-2ebf-4574-98f1-0730e05ee34f', 'b5375610-13d0-4e17-981a-bf5e18dccf4d', NULL, 1, N'5-50400', N'Biaya Impor', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('ac2df772-1acb-4fde-9815-07447b19b97f', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '25df4334-1885-4f52-91e8-05fbc740c572', 1, N'6-60207', N'Iuran & Langganan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('21179d11-2753-47a6-992b-08ddb1105763', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '25df4334-1885-4f52-91e8-05fbc740c572', 1, N'6-60211', N'Sarana Kantor', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('b72520dd-c1fb-49b6-9bae-0b72d3c2a923', 'ba07e7fc-38ee-43ef-95cd-3edd1fc1db88', NULL, 1, N'1-10755', N'Akumulasi Penyusutan - Peralatan Kantor', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('089327ad-d64d-4fd8-99bf-0b7b45a1448a', 'a937c0d2-33bc-4771-a8c9-074a9ee42051', NULL, 1, N'2-20101', N'Hutang Belum Ditagih', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('4f20a441-c7a4-4ebc-95f5-0c88512d01e8', '97620fd7-9bb5-4a4c-add0-5595a28335c1', NULL, 1, N'2-20203', N'Pendapatan Diterima Di Muka', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('4388f4c3-181e-4313-9221-0d1de67505da', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '774d02a9-08de-4ef4-81af-65e488ebabb4', 1, N'6-60109', N'Pesangon', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('e57f55d1-7d69-43c6-ad86-0e2638505f28', 'c8778972-3568-486c-a098-1256e1e4b7d9', NULL, 1, N'1-10400', N'Dana Belum Disetor', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('e5023df0-3c3a-40ae-9749-10d3aa6c17c4', '21b07be0-954d-43f3-9d58-cd7ac615878f', NULL, 1, N'1-10705', N'Aset Tetap - Perlengkapan Kantor', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('9f59eefa-f832-4ed7-89e9-10d9f9f71fc9', 'b5375610-13d0-4e17-981a-bf5e18dccf4d', NULL, 1, N'5-50500', N'Biaya Produksi', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('ca9a1606-d01f-41be-85fe-1172b25215ba', '704384af-a23e-4b85-83db-af24866c9642', NULL, 1, N'1-10003', N'Giro', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('fd43a9ae-6c5f-4f7c-9213-15bdea6629c0', '21b07be0-954d-43f3-9d58-cd7ac615878f', NULL, 1, N'1-10702', N'Aset Tetap - Building Improvements', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('01f726aa-402c-46b4-89f6-188a9a4eeee7', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '8d562995-0ff1-41c9-bf30-74de69c59d32', 1, N'6-60501', N'Penyusutan - Perbaikan Bangunan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('ff12b484-829f-474b-b273-1a5eb1482452', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '8d562995-0ff1-41c9-bf30-74de69c59d32', 1, N'6-60502', N'Penyusutan - Kendaraan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('d7699b67-898d-4ae2-8680-1d37fe0cbdb9', '202ed6ce-8281-4ce3-856c-be8c8ea5121a', NULL, 1, N'4-40200', N'Retur Penjualan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('ee06320a-f649-4034-bd45-1e823532a7d5', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '25df4334-1885-4f52-91e8-05fbc740c572', 1, N'6-60214', N'Pajak dan Perizinan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('ab9040af-37eb-4882-b1f3-225496264b18', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '774d02a9-08de-4ef4-81af-65e488ebabb4', 1, N'6-60108', N'Insentif', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('936d3dc8-8535-4d3b-a048-2464cddda38a', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '25df4334-1885-4f52-91e8-05fbc740c572', 1, N'6-60209', N'Legal & Profesional', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('28f304e9-c883-4a91-9dcf-251d15ee4697', 'b5375610-13d0-4e17-981a-bf5e18dccf4d', NULL, 1, N'5-50200', N'Retur Pembelian', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('9c789175-81de-4e69-8a5a-2643b152097e', '6287c827-c1e8-4edd-980c-837e37ba9a78', NULL, 1, N'9-90000', N'Beban Pajak - Kini', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('02da01ea-f002-44ad-a6fa-26739d01a539', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '774d02a9-08de-4ef4-81af-65e488ebabb4', 1, N'6-60105', N'Pengobatan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('f76b9ff6-9844-4465-a0cf-28f7945d71cd', '202ed6ce-8281-4ce3-856c-be8c8ea5121a', NULL, 1, N'4-40201', N'Pendapatan Belum Ditagih', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('a75a7aa1-0642-4eb6-a5ce-2c2310547e9c', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '25df4334-1885-4f52-91e8-05fbc740c572', 1, N'6-60203', N'Perbaikan & Pemeliharaan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('2acc5cc9-e5f6-4fcf-b548-2eb0023aac0c', 'ba07e7fc-38ee-43ef-95cd-3edd1fc1db88', NULL, 1, N'1-10754', N'Akumulasi Penyusutan - Mesin & Peralatan', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('4f822882-771b-4e22-a64a-359275d70a6e', 'a937c0d2-33bc-4771-a8c9-074a9ee42051', NULL, 1, N'2-20100', N'Hutang Usaha', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('3ce1cc28-25ca-4a41-9d93-36debe37c1f7', '6287c827-c1e8-4edd-980c-837e37ba9a78', NULL, 1, N'8-80000', N'Beban Bunga', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('28d8fc7f-78cd-40aa-8521-3a0c9dc5f6ce', '6287c827-c1e8-4edd-980c-837e37ba9a78', NULL, 1, N'9-90001', N'Beban Pajak - Tangguhan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('c9948b26-9bb4-46b7-b544-3acde5a7097d', '07be4329-0f11-4bf4-ac4d-7987055e6d53', NULL, 1, N'3-30001', N'Tambahan Modal Disetor', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('dcc8b1c1-0c5d-492e-945a-3b6cfa8fd314', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '25df4334-1885-4f52-91e8-05fbc740c572', 1, N'6-60202', N'Bensin, Tol dan Parkir - Umum', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('854549d1-7fd0-44ab-891a-3bca3f007c72', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', 'c304caac-1241-4bd8-831a-cf9c45aae993', 1, N'6-60305', N'Pemborong', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('d7eeb87a-ee43-46f0-94c7-3d7de72c8efa', 'b5375610-13d0-4e17-981a-bf5e18dccf4d', NULL, 1, N'5-50300', N'Pengiriman & Pengangkutan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('d4bc4af3-0883-4a56-998f-3dac2de73b68', 'ba07e7fc-38ee-43ef-95cd-3edd1fc1db88', NULL, 1, N'1-10753', N'Akumulasi penyusutan - Kendaraan', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('84979a78-1613-487e-8b21-3ec8994c99cd', '97620fd7-9bb5-4a4c-add0-5595a28335c1', NULL, 1, N'2-20201', N'Hutang Gaji', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('4be0efa9-1222-4594-bfd5-4377e6719014', 'c8778972-3568-486c-a098-1256e1e4b7d9', NULL, 1, N'1-10500', N'PPN Masukan', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('59237b99-fef4-43db-ad37-43c89378b945', '6c43cf12-8c53-4057-be4a-01844f3a85a5', NULL, 1, N'1-10800', N'Investasi', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('79e58562-17f6-4c4f-978a-45311aa20e87', '07be4329-0f11-4bf4-ac4d-7987055e6d53', NULL, 1, N'3-30000', N'Modal Saham', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('846a3aa3-2585-4725-ad81-45488a207125', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', 'c96ee599-08fb-4483-a437-76d3d04aaf92', 1, N'6-60402', N'Biaya Sewa - Operasional', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('237a0e9b-72f7-4ac0-934a-45921fc39cf7', '6c43cf12-8c53-4057-be4a-01844f3a85a5', NULL, 1, N'1-10800', N'Investasi', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('927a58b8-2124-4e9f-8fbd-47097d8d60fd', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '774d02a9-08de-4ef4-81af-65e488ebabb4', 1, N'6-60104', N'Lembur', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('ff8d8709-f8ad-44e7-8bcf-47a477083c67', 'b5375610-13d0-4e17-981a-bf5e18dccf4d', NULL, 1, N'5-50100', N'Diskon Pembelian', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('f87b7304-c629-4cac-9186-47ccb74844fe', '97620fd7-9bb5-4a4c-add0-5595a28335c1', NULL, 1, N'2-20599', N'Hutang Pajak Lainnya', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('1a2edf38-eedd-4236-8700-4919c1f026cb', '21b07be0-954d-43f3-9d58-cd7ac615878f', NULL, 1, N'1-10702', N'Aset Tetap - Building Improvements', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('42b5436b-ffac-4793-95c4-49672f9d1e09', '6287c827-c1e8-4edd-980c-837e37ba9a78', NULL, 1, N'8-80002', N'(Laba)/Rugi Pelepasan Aset Tetap', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('0b691c3c-92f8-4acf-ab7a-49d189b482f1', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '774d02a9-08de-4ef4-81af-65e488ebabb4', 1, N'6-60102', N'Upah', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('21c3f93f-1c60-4465-be44-4bae050d9b9e', 'ba07e7fc-38ee-43ef-95cd-3edd1fc1db88', NULL, 1, N'1-10753', N'Akumulasi penyusutan - Kendaraan', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('7d7eba6b-8c4b-42e7-91d0-4cc6c56b8c96', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '774d02a9-08de-4ef4-81af-65e488ebabb4', 1, N'6-60106', N'THR & Bonus', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('9b0fc8af-b4ba-4a04-b672-4e766edfe0ae', 'c8778972-3568-486c-a098-1256e1e4b7d9', NULL, 1, N'1-10502', N'Pajak Dibayar Di Muka - PPh 23', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('1e3a6686-48d8-4789-bbbd-4fc58b4ccebc', '97620fd7-9bb5-4a4c-add0-5595a28335c1', NULL, 1, N'2-20301', N'Sarana Kantor Terhutang', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('5dc375b8-874c-4438-85cb-52e2e0f6d90d', '97620fd7-9bb5-4a4c-add0-5595a28335c1', NULL, 1, N'2-20200', N'Hutang Lain Lain', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('df5989c8-4f0e-4365-b1cb-5936cdc12b22', '97620fd7-9bb5-4a4c-add0-5595a28335c1', NULL, 1, N'2-20503', N'Hutang Pajak - PPh 23', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('e6203032-6b25-49c3-b593-59d59e62b0f7', '202ed6ce-8281-4ce3-856c-be8c8ea5121a', NULL, 1, N'4-40000', N'Pendapatan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('9f1fdb5d-29c4-489e-b932-5a6acdeaf6d9', '4d92ca55-798b-4d74-9aef-bd9216b1979d', NULL, 1, N'2-20700', N'Kewajiban Manfaat Karyawan', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('35d5c968-cbcb-474d-a1c7-5a90b00663a9', 'ba07e7fc-38ee-43ef-95cd-3edd1fc1db88', NULL, 1, N'1-10751', N'Akumulasi Penyusutan - Bangunan', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('31cc30f1-0fec-446e-ba56-5b00c15ffe97', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', 'c96ee599-08fb-4483-a437-76d3d04aaf92', 1, N'6-60401', N'Biaya Sewa - Kendaraan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('1c7beb41-bdda-4a3b-be01-5e8d190799e4', '21b07be0-954d-43f3-9d58-cd7ac615878f', NULL, 1, N'1-10701', N'Aset Tetap - Bangunan', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('9d60c1f1-3504-405b-90f8-5eed69248d72', 'ba07e7fc-38ee-43ef-95cd-3edd1fc1db88', NULL, 1, N'1-10755', N'Akumulasi Penyusutan - Peralatan Kantor', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('bd57ae76-5efc-4986-85fb-629146034009', '97620fd7-9bb5-4a4c-add0-5595a28335c1', NULL, 1, N'2-20500', N'PPN Keluaran', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('00f0bc10-497a-4874-879f-64989775aecb', '97620fd7-9bb5-4a4c-add0-5595a28335c1', NULL, 1, N'2-20600', N'Hutang dari Pemegang Saham', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('14f53cb3-1878-4863-9a79-64bee46d0f0d', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '774d02a9-08de-4ef4-81af-65e488ebabb4', 1, N'6-60103', N'Makanan & Transportasi', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('a9908981-3e79-4ab2-a922-64ca661bdc1e', '07be4329-0f11-4bf4-ac4d-7987055e6d53', NULL, 1, N'3-30300', N'Pendapatan Komprehensif Lainnya', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('774d02a9-08de-4ef4-81af-65e488ebabb4', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', NULL, 1, N'6-60100', N'Biaya Umum & Administratif', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('14a8a00f-4fc0-40c1-ac8a-66100b86b300', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '25df4334-1885-4f52-91e8-05fbc740c572', 1, N'6-60208', N'Asuransi', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('1a422b75-f952-4fed-b98a-672e235b3235', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '8d562995-0ff1-41c9-bf30-74de69c59d32', 1, N'6-60504', N'Penyusutan - Peralatan Kantor', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('6b76f945-8249-4f5f-9c92-677437b0bf25', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', 'a2795c8d-1466-4647-9a2e-bb19f89ad2f4', 1, N'6-60006', N'Marketing Lainnya', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('1c97e32c-ac8a-4628-a1cb-68e2e88fd41d', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '25df4334-1885-4f52-91e8-05fbc740c572', 1, N'6-60217', N'Listrik', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('59a2b5b1-1a90-4b20-8410-68f04e39e23b', '704384af-a23e-4b85-83db-af24866c9642', NULL, 1, N'1-10002', N'Rekening Bank', N'', 0, 1, 1, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('5b64870b-f929-4831-ac77-6aaacff8c538', '21b07be0-954d-43f3-9d58-cd7ac615878f', NULL, 1, N'1-10706', N'Aset Tetap - Aset Sewa Guna Usaha', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('48270dd4-efeb-433f-825e-6c2a3596b695', 'c8778972-3568-486c-a098-1256e1e4b7d9', NULL, 1, N'1-10501', N'Pajak Dibayar Di Muka - PPh 22', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('7574f328-3294-464c-83e9-6d3a37ebb664', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '25df4334-1885-4f52-91e8-05fbc740c572', 1, N'6-60201', N'Hiburan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('7c2b9341-efb9-4dcf-a16b-7451b5ff98e4', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', 'a2795c8d-1466-4647-9a2e-bb19f89ad2f4', 1, N'6-60002', N'Komisi & Fee', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('8d562995-0ff1-41c9-bf30-74de69c59d32', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', NULL, 1, N'6-60500', N'Penyusutan - Bangunan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('c96ee599-08fb-4483-a437-76d3d04aaf92', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', NULL, 1, N'6-60400', N'Biaya Sewa - Bangunan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('7cfdba48-5497-4e7d-bf97-775b53a4a515', '97620fd7-9bb5-4a4c-add0-5595a28335c1', NULL, 1, N'2-20502', N'Hutang Pajak - PPh 22', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('3690aff5-6bac-42d6-8296-7a041e4bce88', '97620fd7-9bb5-4a4c-add0-5595a28335c1', NULL, 1, N'2-20400', N'Hutang Bank', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('3125d7c7-9a2d-49a6-a155-7c7d3f752b05', 'ba07e7fc-38ee-43ef-95cd-3edd1fc1db88', NULL, 1, N'1-10756', N'Akumulasi Penyusutan - Aset Sewa Guna Usaha', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('6d5bdca0-8e40-441e-8aec-7ce1e9dbaa3c', 'c8778972-3568-486c-a098-1256e1e4b7d9', NULL, 1, N'1-10300', N'Piutang Lainnya', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('c5196ec8-6869-4cba-9694-7e56799dd6f8', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '25df4334-1885-4f52-91e8-05fbc740c572', 1, N'6-60218', N'Air', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('e9a5147c-a689-4374-aadd-821032290658', '62fadc5c-923f-4c96-af9a-cddf8f5e6013', NULL, 1, N'1-10101', N'Piutang Belum Ditagih', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('ff8f2e42-724a-41b7-8eb8-84997cf76a5c', '97620fd7-9bb5-4a4c-add0-5595a28335c1', NULL, 1, N'2-20399', N'Biaya Terhutang Lainnya', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('68999d8b-65de-4fa5-b014-85f47ffd2aae', '130bb3a6-74d1-40aa-be1f-97c2e8566581', 'b073ad71-5b07-434e-8e38-b35f431943a3', 1, N'7-70001', N'Pendapatan Bunga - Deposito', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('8a3c3db7-9594-443c-a18a-864c72c69c11', '97620fd7-9bb5-4a4c-add0-5595a28335c1', NULL, 1, N'2-20302', N'Bunga Terhutang', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('8181c7eb-7363-46f5-b184-8935a3eeac92', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '25df4334-1885-4f52-91e8-05fbc740c572', 1, N'6-60219', N'IPL', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('ed220c86-1783-46a9-acf1-8a0d35d04dec', '130bb3a6-74d1-40aa-be1f-97c2e8566581', 'b073ad71-5b07-434e-8e38-b35f431943a3', 1, N'7-70002', N'Pendapatan Komisi - Barang Konsinyasi', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('8c97f51d-f204-4137-9705-8ba1b0048585', '21b07be0-954d-43f3-9d58-cd7ac615878f', NULL, 1, N'1-10703', N'Aset Tetap - Kendaraan', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('229c1d33-15a7-4866-83aa-8dd87117f494', 'ba07e7fc-38ee-43ef-95cd-3edd1fc1db88', NULL, 1, N'1-10757', N'Akumulasi Amortisasi', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('8e5ec2d4-01e3-45da-a8ad-8f23adeb09ee', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', 'c304caac-1241-4bd8-831a-cf9c45aae993', 1, N'6-60301', N'Alat Tulis Kantor & Printing', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('cd5fb184-b03b-435c-b286-91e9c3f0a157', '21b07be0-954d-43f3-9d58-cd7ac615878f', NULL, 1, N'1-10706', N'Aset Tetap - Aset Sewa Guna Usaha', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('0434caa7-5836-43f2-8f84-96a05e7f4a93', '21b07be0-954d-43f3-9d58-cd7ac615878f', NULL, 1, N'1-10700', N'Aset Tetap - Tanah', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('964f0374-1581-4cc6-9cff-97e076e7df54', '97620fd7-9bb5-4a4c-add0-5595a28335c1', NULL, 1, N'2-20601', N'Kewajiban Lancar Lainnya', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('8814cf58-227f-46ce-9d66-99345153ca5e', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', 'c96ee599-08fb-4483-a437-76d3d04aaf92', 1, N'6-60403', N'Biaya Sewa - Lain - lain', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('6e667acb-c831-4896-a90b-9949e4f04090', 'c8778972-3568-486c-a098-1256e1e4b7d9', NULL, 1, N'1-10503', N'Pajak Dibayar Di Muka - PPh 25', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('ea443d59-dbab-4e19-8aca-9a1dc5235664', '6287c827-c1e8-4edd-980c-837e37ba9a78', NULL, 1, N'8-80999', N'Beban Lain - lain', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('b0c503b2-3c07-4bdf-a7bb-9c736ca5946a', '21b07be0-954d-43f3-9d58-cd7ac615878f', NULL, 1, N'1-10703', N'Aset Tetap - Kendaraan', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('58d44576-43bf-4822-9fcb-a2b0a76c2e45', '97620fd7-9bb5-4a4c-add0-5595a28335c1', NULL, 1, N'2-20504', N'Hutang Pajak - PPh 29', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('82e75ef8-8083-4444-9003-a3fbb343e08e', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '25df4334-1885-4f52-91e8-05fbc740c572', 1, N'6-60215', N'Denda', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('488ce967-d8b3-4b73-9061-a452ce0ccf25', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '25df4334-1885-4f52-91e8-05fbc740c572', 1, N'6-60210', N'Beban Manfaat Karyawan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('0b2114de-9229-48bf-a1e7-a68cfb63a1dd', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '774d02a9-08de-4ef4-81af-65e488ebabb4', 1, N'6-60110', N'Manfaat dan Tunjangan Lain', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('bf586c51-90ba-4267-9c5d-a74145d00a5b', 'c8778972-3568-486c-a098-1256e1e4b7d9', NULL, 1, N'1-10403', N'Uang Muka', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('06d5be27-98e8-405f-8796-a9124eeecbd1', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', 'a2795c8d-1466-4647-9a2e-bb19f89ad2f4', 1, N'6-60004', N'Perjalanan Dinas - Penjualan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('43c1350c-a6b1-4188-b75f-a944d569d507', '62fadc5c-923f-4c96-af9a-cddf8f5e6013', NULL, 1, N'1-10100', N'Piutang Usaha', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('1169848a-cfdd-488b-ae9c-ad05817be49c', '6287c827-c1e8-4edd-980c-837e37ba9a78', NULL, 1, N'8-80100', N'Penyesuaian Persediaan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('640d1e04-cc72-433f-b441-ad401624b61a', '07be4329-0f11-4bf4-ac4d-7987055e6d53', NULL, 1, N'3-30999', N'Ekuitas Saldo Awal', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('9dcb2d8a-1537-4712-ad8a-ae0881f2025d', 'ba07e7fc-38ee-43ef-95cd-3edd1fc1db88', NULL, 1, N'1-10756', N'Akumulasi Penyusutan - Aset Sewa Guna Usaha', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('75fba00b-f0d6-4426-9ebc-ae482a061bdd', 'ba07e7fc-38ee-43ef-95cd-3edd1fc1db88', NULL, 1, N'1-10754', N'Akumulasi Penyusutan - Mesin & Peralatan', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('b063f3d9-5930-4a3a-af2c-afcf3341f24b', '21b07be0-954d-43f3-9d58-cd7ac615878f', NULL, 1, N'1-10701', N'Aset Tetap - Bangunan', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('cbbbb197-f28d-489f-828e-b24dde6e2800', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', 'a2795c8d-1466-4647-9a2e-bb19f89ad2f4', 1, N'6-60001', N'Iklan & Promosi', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('b073ad71-5b07-434e-8e38-b35f431943a3', '130bb3a6-74d1-40aa-be1f-97c2e8566581', NULL, 1, N'7-70000', N'Pendapatan Bunga - Bank', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('1893452c-5920-40b9-83fa-b408eb93a522', 'a937c0d2-33bc-4771-a8c9-074a9ee42051', NULL, 1, N'2-20100', N'Hutang Usaha', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('15fd549b-c281-42af-81a0-b5c2b4e248dd', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '8d562995-0ff1-41c9-bf30-74de69c59d32', 1, N'6-60599', N'Penyusutan - Aset Sewa Guna Usaha', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('0b194c75-9f1d-4826-843b-ba256fb50724', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '25df4334-1885-4f52-91e8-05fbc740c572', 1, N'6-60205', N'Makanan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('6637a538-ef69-4e68-b43b-bb1597e656df', 'c8778972-3568-486c-a098-1256e1e4b7d9', NULL, 1, N'1-10502', N'Pajak Dibayar Di Muka - PPh 23', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('a2795c8d-1466-4647-9a2e-bb19f89ad2f4', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', NULL, 1, N'6-60000', N'Biaya Penjualan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('d0423df4-9d8b-4f54-887a-bc6194f3727f', '21b07be0-954d-43f3-9d58-cd7ac615878f', NULL, 1, N'1-10704', N'Aset Tetap - Mesin & Peralatan', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('bbeae96f-acfb-4e0e-ad07-bd623a4a2cce', '490fa648-4575-4700-844f-1b9a7d4872ff', NULL, 1, N'1-10200', N'Persediaan Barang', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('e1c354a4-47a9-4234-9a70-bfcfb6fe69f9', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '25df4334-1885-4f52-91e8-05fbc740c572', 1, N'6-60212', N'Pelatihan & Pengembangan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('71df9baa-9eaa-4396-bbbc-c0055bf5678a', '21b07be0-954d-43f3-9d58-cd7ac615878f', NULL, 1, N'1-10707', N'Aset Tak Berwujud', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('b5ba2f0b-e14b-48d2-9582-c35c060547e9', '21b07be0-954d-43f3-9d58-cd7ac615878f', NULL, 1, N'1-10700', N'Aset Tetap - Tanah', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('db09726b-c318-491c-a867-c60702f08677', 'a937c0d2-33bc-4771-a8c9-074a9ee42051', NULL, 1, N'2-20101', N'Hutang Belum Ditagih', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('223f4845-741b-4f0c-9a9f-c90726da7734', '62fadc5c-923f-4c96-af9a-cddf8f5e6013', NULL, 1, N'1-10102', N'Cadangan Kerugian Piutang', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('41ee2500-8acf-45fd-8569-c931339a6b3f', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '25df4334-1885-4f52-91e8-05fbc740c572', 1, N'6-60220', N'Langganan Software', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('09505736-2255-4711-a80c-ca358ba4e951', '97620fd7-9bb5-4a4c-add0-5595a28335c1', NULL, 1, N'2-20501', N'Hutang Pajak - PPh 21', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('58b9f332-bcba-4c83-851c-cbc5ae31703d', 'ba07e7fc-38ee-43ef-95cd-3edd1fc1db88', NULL, 1, N'1-10752', N'Akumulasi Penyusutan - Building Improvements', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('8b4d6066-b138-49ad-91f8-cd8b288d5326', 'c8778972-3568-486c-a098-1256e1e4b7d9', NULL, 1, N'1-10301', N'Piutang Karyawan', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('4f002808-26e5-4ee7-bb7b-cf63767b97b6', '202ed6ce-8281-4ce3-856c-be8c8ea5121a', NULL, 1, N'4-40100', N'Diskon Penjualan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('c304caac-1241-4bd8-831a-cf9c45aae993', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', NULL, 1, N'6-60300', N'Beban Kantor', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('7615c798-1a38-476c-8318-d01f23ef4d5b', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', 'c304caac-1241-4bd8-831a-cf9c45aae993', 1, N'6-60304', N'Supplies dan Material', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('8e211ee8-3a50-467a-927e-d287cd27abad', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', 'c304caac-1241-4bd8-831a-cf9c45aae993', 1, N'6-60303', N'Keamanan dan Kebersihan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('ebb6dfb7-3aa3-47dd-953c-d43b90372ab5', '130bb3a6-74d1-40aa-be1f-97c2e8566581', 'b073ad71-5b07-434e-8e38-b35f431943a3', 1, N'7-70099', N'Pendapatan Lain - lain', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('1390566f-770b-4ac1-aa6f-d57eda184861', 'ba07e7fc-38ee-43ef-95cd-3edd1fc1db88', NULL, 1, N'1-10757', N'Akumulasi Amortisasi', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('7fff481c-7e17-4db1-8d21-d6d9b514e919', '21b07be0-954d-43f3-9d58-cd7ac615878f', NULL, 1, N'1-10705', N'Aset Tetap - Perlengkapan Kantor', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('78f260c0-37f0-4d01-8dbe-d755ddf4e7fc', '21b07be0-954d-43f3-9d58-cd7ac615878f', NULL, 1, N'1-10707', N'Aset Tak Berwujud', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('a773b24e-427e-4845-bb35-d8f1215fac3e', '6287c827-c1e8-4edd-980c-837e37ba9a78', NULL, 1, N'8-80001', N'Provisi', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('0c5c5c07-887c-4162-8579-da0586db74c8', '07be4329-0f11-4bf4-ac4d-7987055e6d53', NULL, 1, N'3-30200', N'Deviden', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('19b7f167-8512-4674-8156-daf8550ca53d', '21b07be0-954d-43f3-9d58-cd7ac615878f', NULL, 1, N'1-10704', N'Aset Tetap - Mesin & Peralatan', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('3863b79d-b1cb-4d87-bf6b-dd67fafe4569', 'c8778972-3568-486c-a098-1256e1e4b7d9', NULL, 1, N'1-10501', N'Pajak Dibayar Di Muka - PPh 22', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('75c817df-dac1-4009-bef7-e21f52f7ce7e', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '774d02a9-08de-4ef4-81af-65e488ebabb4', 1, N'6-60101', N'Gaji', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('29a24a46-5185-4ef6-91f1-e23515a5beb8', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '25df4334-1885-4f52-91e8-05fbc740c572', 1, N'6-60216', N'Pengeluaran Barang Rusak', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('ebd2ad44-1b33-43aa-9099-e2ba0540d9e7', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', 'a2795c8d-1466-4647-9a2e-bb19f89ad2f4', 1, N'6-60005', N'Komunikasi - Penjualan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('cc91b040-6ca9-436d-88e6-e406eba5722a', '130bb3a6-74d1-40aa-be1f-97c2e8566581', 'b073ad71-5b07-434e-8e38-b35f431943a3', 1, N'7-70003', N'Pembulatan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('1af90cdf-7b04-4265-9256-e6ee75cfdcd1', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', 'a2795c8d-1466-4647-9a2e-bb19f89ad2f4', 1, N'6-60003', N'Bensin, Tol dan Parkir - Penjualan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('522f1812-dc92-4368-8b2b-e7415859fe83', '07be4329-0f11-4bf4-ac4d-7987055e6d53', NULL, 1, N'3-30100', N'Laba Ditahan', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('b640c103-aea6-4c32-960d-e74c5626fb15', '97620fd7-9bb5-4a4c-add0-5595a28335c1', NULL, 1, N'2-20205', N'Hutang Konsinyasi', N'', 0, 0, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('f9e40e0b-084a-4921-bb33-e79df2f915ac', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '25df4334-1885-4f52-91e8-05fbc740c572', 1, N'6-60206', N'Komunikasi - Umum', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('e9c6eb60-4d31-4e5b-ba50-e7edc1ddaca9', 'b5375610-13d0-4e17-981a-bf5e18dccf4d', NULL, 1, N'5-50000', N'Beban Pokok Pendapatan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('feb7be77-2e0f-4ae3-a7f8-ea4a30546229', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '25df4334-1885-4f52-91e8-05fbc740c572', 1, N'6-60204', N'Perjalanan Dinas - Umum', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('58fddbfa-80aa-491d-9479-eb2ab2705dc8', 'ba07e7fc-38ee-43ef-95cd-3edd1fc1db88', NULL, 1, N'1-10752', N'Akumulasi Penyusutan - Building Improvements', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('c9c61be2-ccbe-456b-b436-eb5dfd36d77a', 'c8778972-3568-486c-a098-1256e1e4b7d9', NULL, 1, N'1-10503', N'Pajak Dibayar Di Muka - PPh 25', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('afd4e902-4672-4462-9d6e-efc0bb29dbec', 'c8778972-3568-486c-a098-1256e1e4b7d9', NULL, 1, N'1-10402', N'Biaya Dibayar Di Muka', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('6f998173-ad0d-4130-aaba-f17a1aa58836', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '8d562995-0ff1-41c9-bf30-74de69c59d32', 1, N'6-60503', N'Penyusutan - Mesin & Peralatan', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('c5bc6fad-ed8b-437b-99c9-f2c58ff8d7f2', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '25df4334-1885-4f52-91e8-05fbc740c572', 1, N'6-60213', N'Beban Piutang Tak Tertagih', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('760218ed-75e9-4c22-8e0c-f2d7e38ea70e', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', 'c304caac-1241-4bd8-831a-cf9c45aae993', 1, N'6-60302', N'Bea Materai', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('c662f675-61d7-439d-8fb5-f42ea4a49c9a', 'fafa4f6b-e6e1-40b9-8c37-9a332d73e4a1', '774d02a9-08de-4ef4-81af-65e488ebabb4', 1, N'6-60107', N'Jamsostek', N'', 0, 0, NULL, NULL, NULL, 0, 0);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('892488f5-b59e-4763-a3ec-f7cfe158c075', 'ba07e7fc-38ee-43ef-95cd-3edd1fc1db88', NULL, 1, N'1-10751', N'Akumulasi Penyusutan - Bangunan', N'', 0, 1, NULL, NULL, NULL, 0, 1);
INSERT dbo.MAkun(ID, IDKlasAkun, IDParent, IDDepartemen, Kode, Nama, Keterangan, IDType, IsDebet, IsKasBank, NoRekening, AtasNamaRekening, IDTypeBank, IsNeraca) VALUES ('9709b796-5588-4425-b302-fd7e89d14839', '704384af-a23e-4b85-83db-af24866c9642', NULL, 1, N'1-10001', N'Kas', N'', 0, 1, 1, NULL, NULL, 0, 1);"
            Obj = New Model.UpdateDB With {.DBVersion = "Isi_MAkun_210627", .TglUpdate = CDate("2021-06-27"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "TRUNCATE TABLE dbo.MMenu;" & vbCrLf &
                  "INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (1, -1, 1, N'mnMaster', N'Master', 0, 0, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (2, -1, 2, N'mnSaldoAwal', N'Saldo Awal', 0, 0, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (3, -1, 3, N'mnPembelian', N'Pembelian', 0, 0, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (4, -1, 4, N'mnPenjualan', N'Penjualan', 0, 0, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (5, -1, 5, N'mnInternal', N'Internal', 0, 0, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (6, -1, 7, N'mnKasBank', N'Keuangan', 0, 0, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (7, -1, 8, N'mnAccounting', N'Akuntansi', 0, 0, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (8, -1, 9, N'mnLaporan', N'Laporan', 0, 0, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (9, -1, 6, N'mnPabrikasi', N'Pabrikasi', 0, 0, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (101, 1, 1, N'mnKategori', N'Data Kategori', 0, 0, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (102, 1, 2, N'mnSatuan', N'Data Satuan', 0, 0, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (103, 1, 3, N'mnMerk', N'Data Merk', 0, 0, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (104, 1, 4, N'mnBarang', N'Daftar Barang', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (105, 1, 5, N'mnKontak', N'Data All Kontak (Supplier, Customer, dan Karyawan)', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (106, 1, 6, N'mnGudang', N'Data Gudang', 0, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (107, 1, 7, N'mnEDC', N'Data EDC', 0, 0, 0);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (201, 2, 1, N'mnDaftarSA', N'Daftar Saldo Awal Persediaan', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (202, 2, 2, N'mnDaftarSAHS', N'Daftar Saldo Awal Hutang', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (203, 2, 3, N'mnDaftarSAPC', N'Daftar Saldo Awal Piutang', 1, 0, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (301, 3, 1, N'mnPO', N'Daftar Pesanan', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (302, 3, 2, N'mnBeli', N'Daftar Pembelian', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (303, 3, 3, N'mnReturBeli', N'Daftar Retur Pembelian', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (304, 3, 4, N'mnPPNMasukan', N'Daftar PPN Masukan', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (401, 4, 1, N'mnJual', N'Daftar Penjualan', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (402, 4, 2, N'mnReturJual', N'Daftar Retur Penjualan', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (403, 4, 3, N'mnPPNKeluaran', N'Daftar PPN Keluaran', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (501, 5, 1, N'mnPenyesuaianMasuk', N'Daftar Penyesuaian Masuk', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (502, 5, 2, N'mnPenyesuaianKeluar', N'Daftar Penyesuaian Keluar', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (503, 5, 3, N'mnStockOpname', N'Stock Opname', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (504, 5, 4, N'mnMutasiGudang', N'Daftar Mutasi Gudang', 1, 0, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (601, 6, 1, N'mnDaftarKasOut', N'Pelunasan Hutang dan Kas Keluar', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (602, 6, 2, N'mnDaftarKasIn', N'Pelunasan Piutang dan Kas Masuk', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (603, 6, 3, N'mnBiaya', N'Daftar Biaya', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (701, 7, 1, N'mnAcc', N'Daftar Accounting', 1, 1, 0);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (702, 7, 0, N'mnDaftarAkun', N'Daftar Akun', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (801, 8, 1, N'mnKartuStok', N'Laporan Kartu Stok', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (802, 8, 2, N'mnSaldoStok', N'Laporan Saldo Stok', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (803, 8, 3, N'mnMutasiStok', N'Laporan Mutasi Stok', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (804, 8, 4, N'mnLapJual', N'Laporan Penjualan', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (805, 8, 5, N'mnLapBeli', N'Laporan Pembelian', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (806, 8, 6, N'mnLapAgingHutang', N'Laporan Aging Hutang', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (807, 8, 7, N'mnLapAgingPiutang', N'Laporan Aging Piutang', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (808, 8, 8, N'mnLapLabaRugi', N'Laporan Laba Rugi (Kotor)', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (809, 8, 9, N'mnLapJualLaku', N'Laporan Penjualan Paling Laku', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (901, 9, 1, N'mnMaterial', N'Daftar Material / Formula', 1, 0, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (902, 9, 2, N'mnWOAssembly', N'Work Order', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (903, 9, 3, N'mnAssemblyOut', N'Daftar Pengeluaran Bahan', 1, 1, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (904, 9, 4, N'mnAssemblyBiaya', N'Daftar Pencatatan Biaya', 1, 0, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (905, 9, 5, N'mnAssemblyHasil', N'Daftar Hasil Produksi', 1, 0, 1);
INSERT dbo.MMenu(NoID, IDParent, NoUrut, Name, Caption, IsBig, IsBeginGroup, IsActive) VALUES (906, 9, 6, N'mnAssembly', N'Daftar Produksi (Langsung)', 1, 1, 1);"
            Obj = New Model.UpdateDB With {.DBVersion = "Update_MMenu_210627", .TglUpdate = CDate("2021-06-27"), .SQL = SQL}
            Hasil.Add(Obj)

            Return Hasil
        End Function
#End Region
#Region "Menu"
        Private Enum ParentMenu
            Master = 1
            SaldoAwal = 2
            Pembelian = 3
            Penjualan = 4
            Internal = 5
            KasBank = 6
            Keuangan = 7
            Laporan = 8
            Pabrikasi = 9
        End Enum

        Public Shared Function ListMenu_20210623() As List(Of Model.MMenu)

        End Function
#End Region
    End Class
End Namespace
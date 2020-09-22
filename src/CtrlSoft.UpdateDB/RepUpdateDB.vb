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

                                    com.CommandText = Obj.SQL
                                    com.Parameters.Clear()
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
                                Else
                                    com.Transaction.Rollback()
                                End If
                            Catch ex As Exception
                                com.Transaction.Rollback()
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

            SQL = "CREATE TABLE [dbo].[MJenisTransaksiD](" & vbCrLf & _
                  "[NoID] [int] NOT NULL," & vbCrLf & _
                  "[IDJenisTransaksi] [int] NOT NULL," & vbCrLf & _
                  "[Kode] [varchar](50) NULL," & vbCrLf & _
                  "[Nama] [varchar](50) NULL," & vbCrLf & _
                  "CONSTRAINT [PK_MJenisTransaksiD] PRIMARY KEY CLUSTERED" & vbCrLf & _
                  "(" & vbCrLf & _
                  "[NoID] Asc" & vbCrLf & _
                  ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" & vbCrLf & _
                  ") ON [PRIMARY]"
            Obj = New Model.UpdateDB With {.DBVersion = "MJenisTransaksiD_200906", .TglUpdate = CDate("2020-09-06"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE TABLE [dbo].[MPOS](" & vbCrLf & _
                  "[NoID] [int] NOT NULL," & vbCrLf & _
                  "[Kode] [varchar](50) NULL," & vbCrLf & _
                  "[Nama] [varchar](50) NULL," & vbCrLf & _
                  "[IsActive] [bit] NULL," & vbCrLf & _
                  "[IDJenisPenjualan] [int] NULL," & vbCrLf & _
                  "[LastConnected] [datetime] NULL," & vbCrLf & _
                  "[LastTransaction] [datetime] NULL," & vbCrLf & _
                  "CONSTRAINT [PK_MPOS] PRIMARY KEY CLUSTERED " & vbCrLf & _
                  "(" & vbCrLf & _
                  "[NoID] Asc" & vbCrLf & _
                  ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" & vbCrLf & _
                  ") ON [PRIMARY]"
            Obj = New Model.UpdateDB With {.DBVersion = "MPOS_200906", .TglUpdate = CDate("2020-09-06"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE TABLE [dbo].[MReset](" & vbCrLf & _
                "[NoID] [bigint] NOT NULL," & vbCrLf & _
                "[Kode] [varchar](50) NULL," & vbCrLf & _
                "[IDKasir] [int] NULL," & vbCrLf & _
                "[Tanggal] [date] NULL," & vbCrLf & _
                "[Shift] [smallint] NULL," & vbCrLf & _
                "[IDPOS] [int] NULL," & vbCrLf & _
                "[JumlahNota] [int] NULL," & vbCrLf & _
                "[Subtotal] [money] NULL," & vbCrLf & _
                "[Tunai] [money] NULL," & vbCrLf & _
                "[SetorTunai] [money] NULL," & vbCrLf & _
                "[IsPosted] [bit] NULL," & vbCrLf & _
                "[TglPosted] [datetime] NULL," & vbCrLf & _
                "[IDUserPosted] [int] NULL," & vbCrLf & _
                "[IDUserEntry] [int] NULL," & vbCrLf & _
                "[IDUserEdit] [int] NULL," & vbCrLf & _
                "CONSTRAINT [PK_MReset] PRIMARY KEY CLUSTERED " & vbCrLf & _
                "(" & vbCrLf & _
                "[NoID] ASC" & vbCrLf & _
                ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" & vbCrLf & _
                ") ON [PRIMARY]"
            Obj = New Model.UpdateDB With {.DBVersion = "MReset_200906", .TglUpdate = CDate("2020-09-06"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE TABLE [dbo].[MResetD](" & vbCrLf & _
                "[IDReset] [bigint] NOT NULL," & vbCrLf & _
                "[IDJenisPembayaran] [int] NOT NULL," & vbCrLf & _
                "[JumlahNota] [int] NULL," & vbCrLf & _
                "[Subtotal] [money] NULL," & vbCrLf & _
                "CONSTRAINT [PK_MResetD] PRIMARY KEY CLUSTERED " & vbCrLf & _
                "(" & vbCrLf & _
                "[IDReset] ASC," & vbCrLf & _
                "[IDJenisPembayaran] ASC" & vbCrLf & _
                ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" & vbCrLf & _
                ") ON [PRIMARY]"
            Obj = New Model.UpdateDB With {.DBVersion = "MResetD_200906", .TglUpdate = CDate("2020-09-06"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "UPDATE MMenu SET [Caption]='Keuangan', IsActive=1 WHERE NoID=6;" & vbCrLf & _
                  "UPDATE MMenu SET [Caption]='Pelunasan Hutang dan Kas Keluar', IsActive=1 WHERE NoID=601;" & vbCrLf & _
                  "UPDATE MMenu SET [Caption]='Pelunasan Piutang dan Kas Masuk', IsActive=1 WHERE NoID=602;"
            Obj = New Model.UpdateDB With {.DBVersion = "spKasBank_2000916", .TglUpdate = CDate("2020-09-16"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "UPDATE MMenu SET IsActive=1 WHERE NoID=2;" & vbCrLf & _
                  "INSERT INTO MMenu (NoID, IDParent, NoUrut, [Name], Caption, IsBig, IsBeginGroup, IsActive) VALUES" & vbCrLf & _
                  "(202, 2, 2, 'mnDaftarSAHS', 'Daftar Saldo Awal Hutang', 1, 1, 1)," & vbCrLf & _
                  "(203, 2, 3, 'mnDaftarSAPC', 'Daftar Saldo Awal Piutang', 1, 0, 1);"
            Obj = New Model.UpdateDB With {.DBVersion = "mnSaldoAwal_2000916", .TglUpdate = CDate("2020-09-16"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE TABLE [dbo].[MSaldoAwalPersediaan](" & vbCrLf & _
                  "[NoID] [int] NOT NULL," & vbCrLf & _
                  "[IDGudang] [int] NOT NULL," & vbCrLf & _
                  "[IDBarangD] [bigint] NOT NULL," & vbCrLf & _
                  "[IDBarang] [bigint] NOT NULL," & vbCrLf & _
                  "[Kode] [varchar](15) NOT NULL," & vbCrLf & _
                  "[Tanggal] [datetime] NOT NULL," & vbCrLf & _
                  "[Qty] [numeric](18, 3) NOT NULL," & vbCrLf & _
                  "[IDSatuan] [int] NOT NULL," & vbCrLf & _
                  "[Konversi] [numeric](18, 0) NOT NULL," & vbCrLf & _
                  "[HargaPokok] [money] NOT NULL," & vbCrLf & _
                  "[Jumlah] [money] NOT NULL," & vbCrLf & _
                  "[IDUserEntry] [int] NOT NULL," & vbCrLf & _
                  "[TglEntry] [datetime] NOT NULL," & vbCrLf & _
                  "[IDUserEdit] [int] NULL," & vbCrLf & _
                  "[TglEdit] [datetime] NULL," & vbCrLf & _
                  "[IsPosted] [bit] NULL," & vbCrLf & _
                  "[IDUserPosted] [int] NULL," & vbCrLf & _
                  "[TglPosted] [datetime] NULL," & vbCrLf & _
                  "[Keterangan] [varchar](250) NULL," & vbCrLf & _
                  "CONSTRAINT [PK_MSaldoAwalPersediaan] PRIMARY KEY CLUSTERED" & vbCrLf & _
                  "(" & vbCrLf & _
                  "[NoID] Asc" & vbCrLf & _
                  ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]," & vbCrLf & _
                  "CONSTRAINT [IX_Kode] UNIQUE NONCLUSTERED" & vbCrLf & _
                  "(" & vbCrLf & _
                  "[Kode] Asc" & vbCrLf & _
                  ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" & vbCrLf & _
                  ") ON [PRIMARY]"
            Obj = New Model.UpdateDB With {.DBVersion = "MSaldoAwalPersediaan_2000916", .TglUpdate = CDate("2020-09-16"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "-- =============================================" & vbCrLf & _
                    "-- Author: <Author,,Name>" & vbCrLf & _
                    "-- Create date: <Create Date,,>" & vbCrLf & _
                    "-- Description: <Description,,>" & vbCrLf & _
                    "-- =============================================" & vbCrLf & _
                    "CREATE PROCEDURE [dbo].[spSaldoAwalPersediaan](@TglDari DATETIME = '2019-01-01', @TglSampai DATETIME = '2019-01-31')" & vbCrLf & _
                    "AS " & vbCrLf & _
                    "BEGIN" & vbCrLf & _
                    "SET NOCOUNT ON;" & vbCrLf & _
                    "SELECT MSaldoAwalPersediaan.*, MBarang.Kode KodeBarang, MBarang.Nama NamaBarang, MBarangD.Barcode, UserEntry.Nama UserEntry, UserEdit.Nama UserEdit, UserPosted.Nama UserPosted, MGudang.Kode Gudang" & vbCrLf & _
                    "FROM MSaldoAwalPersediaan (NOLOCK)" & vbCrLf & _
                    "LEFT JOIN MGudang (NOLOCK) ON MSaldoAwalPersediaan.IDGudang=MGudang.NoID" & vbCrLf & _
                    "LEFT JOIN MBarang (NOLOCK) ON MSaldoAwalPersediaan.IDBarang=MBarang.NoID" & vbCrLf & _
                    "LEFT JOIN MBarangD (NOLOCK) ON MSaldoAwalPersediaan.IDBarangD=MBarangD.NoID" & vbCrLf & _
                    "LEFT JOIN MUser UserEntry (NOLOCK) ON MSaldoAwalPersediaan.IDUserEntry=UserEntry.NoID" & vbCrLf & _
                    "LEFT JOIN MUser UserEdit (NOLOCK) ON MSaldoAwalPersediaan.IDUserEdit=UserEdit.NoID" & vbCrLf & _
                    "LEFT JOIN MUser UserPosted (NOLOCK) ON MSaldoAwalPersediaan.IDUserPosted=UserPosted.NoID" & vbCrLf & _
                    "WHERE CONVERT(DATE, MSaldoAwalPersediaan.Tanggal) >= CONVERT(DATE, @TglDari) AND CONVERT(DATE, MSaldoAwalPersediaan.Tanggal) <= CONVERT(DATE, @TglSampai)" & vbCrLf & _
                    "ORDER BY MSaldoAwalPersediaan.Tanggal, MSaldoAwalPersediaan.NoID" & vbCrLf & _
                    "END"
            Obj = New Model.UpdateDB With {.DBVersion = "spMSaldoAwalPersediaan_200916", .TglUpdate = CDate("2020-09-16"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "-- =============================================" & vbCrLf & _
                    "-- Author: <Author,,Name>" & vbCrLf & _
                    "-- Create date: <Create Date,,>" & vbCrLf & _
                    "-- Description: <Description,,>" & vbCrLf & _
                    "-- =============================================" & vbCrLf & _
                    "CREATE PROCEDURE [dbo].[spFakturMSaldoAwalPersediaan](@TglDari DATETIME = '2019-01-01', @TglSampai DATETIME = '2019-01-31')" & vbCrLf & _
                    "AS " & vbCrLf & _
                    "BEGIN" & vbCrLf & _
                    "SET NOCOUNT ON;" & vbCrLf & _
                    "SELECT MSaldoAwalPersediaan.*, MBarang.Kode KodeBarang, MBarang.Nama NamaBarang, MBarangD.Barcode, UserEntry.Nama UserEntry, UserEdit.Nama UserEdit, UserPosted.Nama UserPosted, MGudang.Kode Gudang" & vbCrLf & _
                    "FROM MSaldoAwalPersediaan (NOLOCK)" & vbCrLf & _
                    "LEFT JOIN MGudang (NOLOCK) ON MSaldoAwalPersediaan.IDGudang=MGudang.NoID" & vbCrLf & _
                    "LEFT JOIN MBarang (NOLOCK) ON MSaldoAwalPersediaan.IDBarang=MBarang.NoID" & vbCrLf & _
                    "LEFT JOIN MBarangD (NOLOCK) ON MSaldoAwalPersediaan.IDBarangD=MBarangD.NoID" & vbCrLf & _
                    "LEFT JOIN MUser UserEntry (NOLOCK) ON MSaldoAwalPersediaan.IDUserEntry=UserEntry.NoID" & vbCrLf & _
                    "LEFT JOIN MUser UserEdit (NOLOCK) ON MSaldoAwalPersediaan.IDUserEdit=UserEdit.NoID" & vbCrLf & _
                    "LEFT JOIN MUser UserPosted (NOLOCK) ON MSaldoAwalPersediaan.IDUserPosted=UserPosted.NoID" & vbCrLf & _
                    "WHERE MSaldoAwalPersediaan.IsPosted=1 AND CONVERT(DATE, MSaldoAwalPersediaan.Tanggal) >= CONVERT(DATE, @TglDari) AND CONVERT(DATE, MSaldoAwalPersediaan.Tanggal) <= CONVERT(DATE, @TglSampai)" & vbCrLf & _
                    "ORDER BY MSaldoAwalPersediaan.Tanggal, MSaldoAwalPersediaan.NoID" & vbCrLf & _
                    "END"
            Obj = New Model.UpdateDB With {.DBVersion = "spFakturMSaldoAwalPersediaan_200916", .TglUpdate = CDate("2020-09-16"), .SQL = SQL}
            Hasil.Add(Obj)

            SQL = "CREATE PROCEDURE [dbo].[spSimpanMSaldoAwalPersediaan]" & vbCrLf & _
                    "(@NoID         BIGINT, " & vbCrLf & _
                    "@Kode         VARCHAR(255), " & vbCrLf & _
                    "@Tanggal      DATETIME, " & vbCrLf & _
                    "@Keterangan   VARCHAR(MAX), " & vbCrLf & _
                    "@IDGudang     INT, " & vbCrLf & _
                    "@IDBarangD    INT, " & vbCrLf & _
                    "@IDBarang     INT, " & vbCrLf & _
                    "@IDSatuan     INT, " & vbCrLf & _
                    "@Qty          NUMERIC(18, 3), " & vbCrLf & _
                    "@Konversi     INT, " & vbCrLf & _
                    "@HargaPokok   MONEY, " & vbCrLf & _
                    "@IsPosted     BIT, " & vbCrLf & _
                    "@TglPosted    DATETIME, " & vbCrLf & _
                    "@IDUserPosted INT, " & vbCrLf & _
                    "@IDUserEntry  INT, " & vbCrLf & _
                    "@IDUserEdit   INT" & vbCrLf & _
                    ")" & vbCrLf & _
                    "AS" & vbCrLf & _
                    "BEGIN" & vbCrLf & _
                    "DECLARE @NoUrut AS INT= 0;" & vbCrLf & _
                    "IF(@NoID <= 0)" & vbCrLf & _
                    "BEGIN" & vbCrLf & _
                    "SELECT @NoID = MAX(NoID)" & vbCrLf & _
                    "FROM MSaldoAwalPersediaan;" & vbCrLf & _
                    "SET @NoID = ISNULL(@NoID, 0) + 1;" & vbCrLf & _
                    "SET @Kode = 'SAP/' + SUBSTRING(CONVERT(VARCHAR(100), @Tanggal, 112), 3, 4) + '/';" & vbCrLf & _
                    "SELECT @NoUrut = MAX(CASE" & vbCrLf & _
                    "WHEN ISNUMERIC(RIGHT(Kode, 5)) = 1" & vbCrLf & _
                    "THEN RIGHT(Kode, 5)" & vbCrLf & _
                    "ELSE 0" & vbCrLf & _
                    "END)" & vbCrLf & _
                    "FROM MSaldoAwalPersediaan" & vbCrLf & _
                    "WHERE LEFT(Kode, LEN(@Kode)) = @Kode;" & vbCrLf & _
                    "SET @NoUrut = ISNULL(@NoUrut, 0) + 1;" & vbCrLf & _
                    "SET @Kode = @Kode + LEFT('00000', 5 - LEN(CONVERT(VARCHAR(10), @NoUrut))) + CONVERT(VARCHAR(10), @NoUrut);" & vbCrLf & _
                    "INSERT INTO [dbo].[MSaldoAwalPersediaan]" & vbCrLf & _
                    "([NoID], " & vbCrLf & _
                    "[IDGudang], " & vbCrLf & _
                    "[IDBarangD], " & vbCrLf & _
                    "[IDBarang], " & vbCrLf & _
                    "[Kode], " & vbCrLf & _
                    "[Tanggal], " & vbCrLf & _
                    "[Qty], " & vbCrLf & _
                    "[IDSatuan], " & vbCrLf & _
                    "[Konversi], " & vbCrLf & _
                    "[HargaPokok], " & vbCrLf & _
                    "[Jumlah], " & vbCrLf & _
                    "[IDUserEntry], " & vbCrLf & _
                    "[TglEntry], " & vbCrLf & _
                    "[IDUserEdit], " & vbCrLf & _
                    "[TglEdit], " & vbCrLf & _
                    "[IsPosted], " & vbCrLf & _
                    "[IDUserPosted], " & vbCrLf & _
                    "[TglPosted], " & vbCrLf & _
                    "[Keterangan]" & vbCrLf & _
                    ")" & vbCrLf & _
                    "SELECT @NoID, " & vbCrLf & _
                    "@IDGudang, " & vbCrLf & _
                    "@IDBarangD, " & vbCrLf & _
                    "@IDBarang, " & vbCrLf & _
                    "@Kode, " & vbCrLf & _
                    "@Tanggal, " & vbCrLf & _
                    "@Qty, " & vbCrLf & _
                    "@IDSatuan, " & vbCrLf & _
                    "@Konversi, " & vbCrLf & _
                    "@HargaPokok, " & vbCrLf & _
                    "ROUND(@HargaPokok * @Qty, 2), " & vbCrLf & _
                    "@IDUserEntry, " & vbCrLf & _
                    "GETDATE(), " & vbCrLf & _
                    "NULL, " & vbCrLf & _
                    "NULL, " & vbCrLf & _
                    "0, " & vbCrLf & _
                    "NULL, " & vbCrLf & _
                    "NULL, " & vbCrLf & _
                    "@Keterangan;" & vbCrLf & _
                    "END;" & vbCrLf & _
                    "ELSE" & vbCrLf & _
                    "BEGIN" & vbCrLf & _
                    "UPDATE [dbo].[MSaldoAwalPersediaan]" & vbCrLf & _
                    "SET " & vbCrLf & _
                    "[IDGudang] = @IDGudang, " & vbCrLf & _
                    "[IDBarangD] = @IDBarangD, " & vbCrLf & _
                    "[IDBarang] = @IDBarang, " & vbCrLf & _
                    "[Kode] = @Kode, " & vbCrLf & _
                    "[Tanggal] = @Tanggal, " & vbCrLf & _
                    "[Qty] = @Qty, " & vbCrLf & _
                    "[IDSatuan] = @IDSatuan, " & vbCrLf & _
                    "[Konversi] = @Konversi, " & vbCrLf & _
                    "[HargaPokok] = @HargaPokok, " & vbCrLf & _
                    "[Jumlah] = ROUND(@HargaPokok * @Qty, 2), " & vbCrLf & _
                    "[IDUserEdit] = @IDUserEdit, " & vbCrLf & _
                    "[TglEdit] = GETDATE(), " & vbCrLf & _
                    "[IsPosted] = @IsPosted, " & vbCrLf & _
                    "[IDUserPosted] = CASE" & vbCrLf & _
                    "WHEN ISNULL(@IsPosted, 0) = 1" & vbCrLf & _
                    "THEN @IDUserPosted" & vbCrLf & _
                    "ELSE NULL" & vbCrLf & _
                    "END, " & vbCrLf & _
                    "[TglPosted] = CASE" & vbCrLf & _
                    "WHEN ISNULL(@IsPosted, 0) = 1" & vbCrLf & _
                    "THEN GETDATE()" & vbCrLf & _
                    "ELSE NULL" & vbCrLf & _
                    "END, " & vbCrLf & _
                    "[Keterangan] = @Keterangan" & vbCrLf & _
                    "WHERE [NoID] = @NoID;" & vbCrLf & _
                    "END;" & vbCrLf & _
                    "SELECT @NoID AS NoID;" & vbCrLf & _
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

            SQL = "DROP PROCEDURE [dbo].[spSaldoAwalPersediaan];" & vbCrLf &
                  "-- =============================================" & vbCrLf &
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

            Return Hasil
        End Function
#End Region
    End Class
End Namespace
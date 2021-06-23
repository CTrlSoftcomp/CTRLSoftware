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
                  "IDAkun bigint NOT NULL," & vbCrLf &
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
                  "IDAkun bigint NOT NULL," & vbCrLf &
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

            Return Hasil
        End Function
#End Region
    End Class
End Namespace
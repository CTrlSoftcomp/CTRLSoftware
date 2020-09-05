'Imports System.Data.SqlClient
'Imports System.Data

Public Class DBVersion
    '    Public Shared Sub UpdateDB()
    '        Using cn As New SqlConnection(Utils.StrKonSQL)
    '            Using com As New SqlCommand
    '                Try
    '                    cn.Open()
    '                    com.Connection = cn
    '                    com.CommandTimeout = cn.ConnectionTimeout

    '                    TambahkanKolom202001(cn, com)
    '                Catch ex As Exception

    '                End Try
    '            End Using
    '        End Using
    '    End Sub

    '#Region "Tambahkan Kolom"
    '    'Private Shared Sub TambahkanKolom202001(ByVal cn As SqlConnection, ByVal com As SqlCommand)
    '    '    Dim DBVersion As String = ""
    '    '    Try
    '    '        com.CommandText = "CREATE TABLE TAppDB (NoID INT IDENTITY(1,1) PRIMARY KEY, Tanggal DATETIME, DBVersion VARCHAR(100))"
    '    '        com.ExecuteNonQuery()
    '    '    Catch ex As Exception

    '    '    End Try

    '    '    Try
    '    '        DBVersion = "MReturJual"
    '    '        com.CommandText = "SELECT COUNT(NoID) FROM TAppDB WHERE DBVersion='" & Utils.FixApostropi(DBVersion) & "'"
    '    '        If Utils.NullToLong(com.ExecuteScalar()) = 0 Then
    '    '            com.Transaction = cn.BeginTransaction

    '    '            com.CommandText = "INSERT INTO TAppDB (Tanggal, DBVersion) VALUES (GETDATE(), '" & Utils.FixApostropi(DBVersion) & "')"
    '    '            com.ExecuteNonQuery()

    '    '            com.CommandText = "CREATE TABLE [dbo].[MReturJual](" & vbCrLf & _
    '    '                                "[NoID] [bigint] NOT NULL," & vbCrLf & _
    '    '                                "[Kode] [varchar](50) NULL," & vbCrLf & _
    '    '                                "[NoReff] [varchar](50) NULL," & vbCrLf & _
    '    '                                "[IDCustomer] [int] NULL," & vbCrLf & _
    '    '                                "[IDGudang] [int] NULL," & vbCrLf & _
    '    '                                "[Tanggal] [date] NULL," & vbCrLf & _
    '    '                                "[JatuhTempo] [date] NULL," & vbCrLf & _
    '    '                                "[Catatan] [varchar](250) NULL," & vbCrLf & _
    '    '                                "[IDTypePajak] [smallint] NULL," & vbCrLf & _
    '    '                                "[Subtotal] [money] NULL," & vbCrLf & _
    '    '                                "[DiscNotaProsen] [numeric](18, 2) NULL," & vbCrLf & _
    '    '                                "[DiscNotaRp] [money] NULL," & vbCrLf & _
    '    '                                "[TotalBruto] [money] NULL," & vbCrLf & _
    '    '                                "[DPP] [money] NULL," & vbCrLf & _
    '    '                                "[PPN] [money] NULL," & vbCrLf & _
    '    '                                "[Total] [money] NULL," & vbCrLf & _
    '    '                                "[Bayar] [money] NULL," & vbCrLf & _
    '    '                                "[Sisa] [money] NULL," & vbCrLf & _
    '    '                                "[IsPosted] [bit] NULL," & vbCrLf & _
    '    '                                "[TglPosted] [datetime] NULL," & vbCrLf & _
    '    '                                "[IDUserPosted] [int] NULL," & vbCrLf & _
    '    '                                "[IDUserEntry] [int] NULL," & vbCrLf & _
    '    '                                "[IDUserEdit] [int] NULL," & vbCrLf & _
    '    '                                "CONSTRAINT [PK_MReturJual] PRIMARY KEY CLUSTERED " & vbCrLf & _
    '    '                                "(" & vbCrLf & _
    '    '                                "[NoID] ASC" & vbCrLf & _
    '    '                                ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" & vbCrLf & _
    '    '                                ") ON [PRIMARY]"
    '    '            com.ExecuteNonQuery()

    '    '            com.CommandText = "CREATE TABLE [dbo].[MReturJualD](" & vbCrLf & _
    '    '                                "[NoID] [bigint] NOT NULL," & vbCrLf & _
    '    '                                "[IDHeader] [bigint] NULL," & vbCrLf & _
    '    '                                "[IDBarangD] [bigint] NULL," & vbCrLf & _
    '    '                                "[IDBarang] [bigint] NULL," & vbCrLf & _
    '    '                                "[IDSatuan] [int] NULL," & vbCrLf & _
    '    '                                "[Konversi] [numeric](18, 0) NULL," & vbCrLf & _
    '    '                                "[Qty] [numeric](18, 3) NULL," & vbCrLf & _
    '    '                                "[Harga] [money] NULL," & vbCrLf & _
    '    '                                "[DiscProsen1] [numeric](18, 2) NULL," & vbCrLf & _
    '    '                                "[DiscProsen2] [numeric](18, 2) NULL," & vbCrLf & _
    '    '                                "[DiscProsen3] [numeric](18, 2) NULL," & vbCrLf & _
    '    '                                "[DiscProsen4] [numeric](18, 2) NULL," & vbCrLf & _
    '    '                                "[DiscProsen5] [numeric](18, 2) NULL," & vbCrLf & _
    '    '                                "[DiscRp] [money] NULL," & vbCrLf & _
    '    '                                "[DiscNotaProsen] [numeric](18, 2) NULL," & vbCrLf & _
    '    '                                "[DiscNotaRp] [money] NULL," & vbCrLf & _
    '    '                                "[JumlahBruto] [money] NULL," & vbCrLf & _
    '    '                                "[DPP] [money] NULL," & vbCrLf & _
    '    '                                "[PPN] [money] NULL," & vbCrLf & _
    '    '                                "[Jumlah] [money] NULL," & vbCrLf & _
    '    '                                "CONSTRAINT [PK_MReturJualD] PRIMARY KEY CLUSTERED " & vbCrLf & _
    '    '                                "(" & vbCrLf & _
    '    '                                "[NoID] ASC" & vbCrLf & _
    '    '                                ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" & vbCrLf & _
    '    '                                ") ON [PRIMARY]"
    '    '            com.ExecuteNonQuery()

    '    '            com.CommandText = "CREATE TABLE [dbo].[MReturJualDBayar](" & vbCrLf & _
    '    '                            "[NoID] [bigint] IDENTITY(1,1) NOT NULL," & vbCrLf & _
    '    '                            "[IDHeader] [bigint] NOT NULL," & vbCrLf & _
    '    '                            "[IDJenisPembayaran] [int] NOT NULL," & vbCrLf & _
    '    '                            "[AtasNama] [varchar](50) NULL," & vbCrLf & _
    '    '                            "[NoRekening] [varchar](150) NULL," & vbCrLf & _
    '    '                            "[Nominal] [money] Default(0) NOT NULL," & vbCrLf & _
    '    '                            "[ChargeProsen] [numeric](18, 3) Default(0) NULL," & vbCrLf & _
    '    '                            "[ChargeRp] [money] Default(0) NULL," & vbCrLf & _
    '    '                            "[Total] [money] Default(0) NULL," & vbCrLf & _
    '    '                            "CONSTRAINT [PK_MReturJualDBayar] PRIMARY KEY CLUSTERED " & vbCrLf & _
    '    '                            "(" & vbCrLf & _
    '    '                            "[NoID] ASC" & vbCrLf & _
    '    '                            ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" & vbCrLf & _
    '    '                            ") ON [PRIMARY]"
    '    '            com.ExecuteNonQuery()

    '    '            com.CommandText = "CREATE PROCEDURE [dbo].[spDaftarReturJual] @TglDari DATETIME = '2019-01-01', @TglSampai DATETIME = '2019-01-31'" & vbCrLf & _
    '    '                            "AS" & vbCrLf & _
    '    '                            "BEGIN" & vbCrLf & _
    '    '                            "-- SET NOCOUNT ON added to prevent extra result sets from" & vbCrLf & _
    '    '                            "-- interfering with SELECT statements." & vbCrLf & _
    '    '                            "SET NOCOUNT ON;" & vbCrLf & _
    '    '                            "-- Insert statements for procedure here" & vbCrLf & _
    '    '                            "SELECT MReturJual.*, UserEntry.Nama UserEntry, UserEdit.Nama UserEdit, UserPosted.Nama UserPosted, MAlamat.Kode KodeCustomer, MAlamat.Nama NamaCustomer, MTypePajak.TypePajak, MGudang.Kode AS Gudang" & vbCrLf & _
    '    '                            "FROM MReturJual (NOLOCK)" & vbCrLf & _
    '    '                            "LEFT JOIN MTypePajak (NOLOCK) ON MReturJual.IDTypePajak=MTypePajak.NoID" & vbCrLf & _
    '    '                            "LEFT JOIN MAlamat (NOLOCK) ON MReturJual.IDCustomer=MAlamat.NoID" & vbCrLf & _
    '    '                            "LEFT JOIN MGudang (NOLOCK) ON MReturJual.IDGudang=MGudang.NoID" & vbCrLf & _
    '    '                            "LEFT JOIN MUser UserEntry (NOLOCK) ON MReturJual.IDUserEntry=UserEntry.NoID" & vbCrLf & _
    '    '                            "LEFT JOIN MUser UserEdit (NOLOCK) ON MReturJual.IDUserEdit=UserEdit.NoID" & vbCrLf & _
    '    '                            "LEFT JOIN MUser UserPosted (NOLOCK) ON MReturJual.IDUserPosted=UserPosted.NoID" & vbCrLf & _
    '    '                            "WHERE CONVERT(DATE, MReturJual.Tanggal) >= CONVERT(DATE, @TglDari) AND CONVERT(DATE, MReturJual.Tanggal) <= CONVERT(DATE, @TglSampai)" & vbCrLf & _
    '    '                            "ORDER BY MReturJual.Tanggal, MReturJual.NoID" & vbCrLf & _
    '    '                            "END"
    '    '            com.ExecuteNonQuery()

    '    '            com.CommandText = "CREATE PROCEDURE [dbo].[spSimpanMReturJual] (" & vbCrLf & _
    '    '                                "@NoID BIGINT" & vbCrLf & _
    '    '                                ",@Kode VARCHAR(255)" & vbCrLf & _
    '    '                                ",@NoReff VARCHAR(255)" & vbCrLf & _
    '    '                                ",@IDCustomer INT" & vbCrLf & _
    '    '                                ",@Tanggal DATETIME" & vbCrLf & _
    '    '                                ",@JatuhTempo DATE" & vbCrLf & _
    '    '                                ",@Catatan VARCHAR(MAX)" & vbCrLf & _
    '    '                                ",@IDTypePajak SMALLINT" & vbCrLf & _
    '    '                                ",@Subtotal MONEY" & vbCrLf & _
    '    '                                ",@DiscNotaProsen NUMERIC(18, 2)" & vbCrLf & _
    '    '                                ",@DiscNotaRp MONEY" & vbCrLf & _
    '    '                                ",@TotalBruto MONEY" & vbCrLf & _
    '    '                                ",@DPP MONEY" & vbCrLf & _
    '    '                                ",@PPN MONEY" & vbCrLf & _
    '    '                                ",@Total MONEY" & vbCrLf & _
    '    '                                ",@Sisa MONEY" & vbCrLf & _
    '    '                                ",@IsPosted BIT" & vbCrLf & _
    '    '                                ",@TglPosted DATETIME" & vbCrLf & _
    '    '                                ",@IDUserPosted INT" & vbCrLf & _
    '    '                                ",@IDUserEntry INT" & vbCrLf & _
    '    '                                ",@IDUserEdit INT" & vbCrLf & _
    '    '                                ",@IDGudang INT" & vbCrLf & _
    '    '                                ")" & vbCrLf & _
    '    '                                "AS" & vbCrLf & _
    '    '                                "BEGIN" & vbCrLf & _
    '    '                                "DECLARE @NoUrut AS INT = 0" & vbCrLf & _
    '    '                                "" & vbCrLf & _
    '    '                                "IF (@NoID <= 0)" & vbCrLf & _
    '    '                                "BEGIN" & vbCrLf & _
    '    '                                "SELECT @NoID = MAX(NoID)" & vbCrLf & _
    '    '                                "FROM MReturJual" & vbCrLf & _
    '    '                                "" & vbCrLf & _
    '    '                                "SET @NoID = ISNULL(@NoID, 0) + 1" & vbCrLf & _
    '    '                                "SET @Kode = 'RJL/' + SUBSTRING(CONVERT(VARCHAR(100), @Tanggal, 112), 3, 4) + '/'" & vbCrLf & _
    '    '                                "" & vbCrLf & _
    '    '                                "SELECT @NoUrut = MAX(CASE " & vbCrLf & _
    '    '                                "WHEN ISNUMERIC(RIGHT(Kode, 5)) = 1" & vbCrLf & _
    '    '                                "THEN RIGHT(Kode, 5)" & vbCrLf & _
    '    '                                "ELSE 0" & vbCrLf & _
    '    '                                "END)" & vbCrLf & _
    '    '                                "FROM MReturJual" & vbCrLf & _
    '    '                                "WHERE LEFT(Kode, LEN(@Kode)) = @Kode" & vbCrLf & _
    '    '                                "" & vbCrLf & _
    '    '                                "SET @NoUrut = ISNULL(@NoUrut, 0) + 1" & vbCrLf & _
    '    '                                "SET @Kode = @Kode + LEFT('00000', 5 - LEN(CONVERT(VARCHAR(10), @NoUrut))) + CONVERT(VARCHAR(10), @NoUrut)" & vbCrLf & _
    '    '                                "" & vbCrLf & _
    '    '                                "INSERT INTO [MReturJual] (" & vbCrLf & _
    '    '                                "NoID" & vbCrLf & _
    '    '                                ",Kode" & vbCrLf & _
    '    '                                ",NoReff" & vbCrLf & _
    '    '                                ",IDCustomer" & vbCrLf & _
    '    '                                ",Tanggal" & vbCrLf & _
    '    '                                ",JatuhTempo" & vbCrLf & _
    '    '                                ",Catatan" & vbCrLf & _
    '    '                                ",IDTypePajak" & vbCrLf & _
    '    '                                ",Subtotal" & vbCrLf & _
    '    '                                ",DiscNotaProsen" & vbCrLf & _
    '    '                                ",DiscNotaRp" & vbCrLf & _
    '    '                                ",TotalBruto" & vbCrLf & _
    '    '                                ",DPP" & vbCrLf & _
    '    '                                ",PPN" & vbCrLf & _
    '    '                                ",Total" & vbCrLf & _
    '    '                                ",Bayar" & vbCrLf & _
    '    '                                ",Sisa" & vbCrLf & _
    '    '                                ",IsPosted" & vbCrLf & _
    '    '                                ",TglPosted" & vbCrLf & _
    '    '                                ",IDUserPosted" & vbCrLf & _
    '    '                                ",IDUserEntry" & vbCrLf & _
    '    '                                ",IDUserEdit" & vbCrLf & _
    '    '                                ",IDGudang" & vbCrLf & _
    '    '                                ")" & vbCrLf & _
    '    '                                "SELECT @NoID" & vbCrLf & _
    '    '                                ",@Kode" & vbCrLf & _
    '    '                                ",@NoReff" & vbCrLf & _
    '    '                                ",@IDCustomer" & vbCrLf & _
    '    '                                ",@Tanggal" & vbCrLf & _
    '    '                                ",@JatuhTempo" & vbCrLf & _
    '    '                                ",@Catatan" & vbCrLf & _
    '    '                                ",@IDTypePajak" & vbCrLf & _
    '    '                                ",@Subtotal" & vbCrLf & _
    '    '                                ",@DiscNotaProsen" & vbCrLf & _
    '    '                                ",@DiscNotaRp" & vbCrLf & _
    '    '                                ",@TotalBruto" & vbCrLf & _
    '    '                                ",@DPP" & vbCrLf & _
    '    '                                ",@PPN" & vbCrLf & _
    '    '                                ",@Total" & vbCrLf & _
    '    '                                ",@Total-@Sisa" & vbCrLf & _
    '    '                                ",@Sisa" & vbCrLf & _
    '    '                                ",@IsPosted" & vbCrLf & _
    '    '                                ",@TglPosted" & vbCrLf & _
    '    '                                ",@IDUserPosted" & vbCrLf & _
    '    '                                ",@IDUserEntry" & vbCrLf & _
    '    '                                ",@IDUserEdit" & vbCrLf & _
    '    '                                ",@IDGudang" & vbCrLf & _
    '    '                                "END" & vbCrLf & _
    '    '                                "ELSE" & vbCrLf & _
    '    '                                "BEGIN" & vbCrLf & _
    '    '                                "UPDATE [MReturJual]" & vbCrLf & _
    '    '                                "SET NoID = @NoID" & vbCrLf & _
    '    '                                ",Kode = @Kode" & vbCrLf & _
    '    '                                ",NoReff = @NoReff" & vbCrLf & _
    '    '                                ",IDCustomer = @IDCustomer" & vbCrLf & _
    '    '                                ",Tanggal = @Tanggal" & vbCrLf & _
    '    '                                ",JatuhTempo = @JatuhTempo" & vbCrLf & _
    '    '                                ",Catatan = @Catatan" & vbCrLf & _
    '    '                                ",IDTypePajak = @IDTypePajak" & vbCrLf & _
    '    '                                ",Subtotal = @Subtotal" & vbCrLf & _
    '    '                                ",DiscNotaProsen = @DiscNotaProsen" & vbCrLf & _
    '    '                                ",DiscNotaRp = @DiscNotaRp" & vbCrLf & _
    '    '                                ",TotalBruto = @TotalBruto" & vbCrLf & _
    '    '                                ",DPP = @DPP" & vbCrLf & _
    '    '                                ",PPN = @PPN" & vbCrLf & _
    '    '                                ",Total = @Total" & vbCrLf & _
    '    '                                ",Bayar = @Total-@Sisa" & vbCrLf & _
    '    '                                ",Sisa = @Sisa" & vbCrLf & _
    '    '                                ",IsPosted = @IsPosted" & vbCrLf & _
    '    '                                ",TglPosted = @TglPosted" & vbCrLf & _
    '    '                                ",IDUserPosted = @IDUserPosted" & vbCrLf & _
    '    '                                ",IDUserEntry = @IDUserEntry" & vbCrLf & _
    '    '                                ",IDUserEdit = @IDUserEdit" & vbCrLf & _
    '    '                                ",IDGudang = @IDGudang" & vbCrLf & _
    '    '                                "WHERE NoID = @NoID" & vbCrLf & _
    '    '                                "END" & vbCrLf & _
    '    '                                "SELECT @NoID AS NoID" & vbCrLf & _
    '    '                                "END"
    '    '            com.ExecuteNonQuery()


    '    '            com.CommandText = "CREATE PROCEDURE [dbo].[spFakturMReturJual] @NoID BIGINT = -1" & vbCrLf & _
    '    '                            "AS" & vbCrLf & _
    '    '                            "BEGIN" & vbCrLf & _
    '    '                            "SET NOCOUNT ON;" & vbCrLf & _
    '    '                            "SELECT dbo.MReturJualD.NoID, dbo.MReturJualD.Konversi, dbo.MReturJualD.Qty, dbo.MReturJualD.Harga, dbo.MReturJualD.DiscProsen1, dbo.MReturJualD.DiscProsen2, dbo.MReturJualD.DiscProsen3, dbo.MReturJualD.DiscProsen4, dbo.MReturJualD.DiscProsen5, dbo.MReturJualD.DiscRp, " & vbCrLf & _
    '    '                            "dbo.MReturJualD.JumlahBruto, dbo.MReturJualD.DPP, dbo.MReturJualD.PPN, dbo.MReturJualD.Jumlah, dbo.MReturJual.Kode, UserEntry.Nama AS UserEntry, UserEdit.Nama AS UserEdit, UserPosted.Nama AS UserPosted, dbo.MAlamat.Kode AS KodeCustomer, " & vbCrLf & _
    '    '                            "dbo.MAlamat.Nama AS NamaCustomer, dbo.MTypePajak.TypePajak, dbo.MReturJual.NoReff, dbo.MReturJual.Tanggal, dbo.MReturJual.Catatan, dbo.MReturJual.Subtotal, dbo.MReturJual.DiscNotaProsen, dbo.MReturJual.DiscNotaRp, " & vbCrLf & _
    '    '                            "dbo.MReturJual.TotalBruto, dbo.MReturJual.DPP AS DPPHeader, dbo.MReturJual.PPN AS PPNHeader, dbo.MReturJual.Total, dbo.MReturJual.IsPosted, dbo.MReturJual.TglPosted, dbo.MReturJual.JatuhTempo, dbo.MReturJualD.IDHeader, dbo.MBarang.Kode AS KodeBarang, " & vbCrLf & _
    '    '                            "dbo.MBarang.Nama AS NamaBarang, dbo.MBarangD.Barcode, dbo.MSatuan.Kode AS Satuan, dbo.MReturJual.Sisa, dbo.MReturJual.Bayar" & vbCrLf & _
    '    '                            "FROM ((dbo.MReturJual " & vbCrLf & _
    '    '                            "INNER JOIN dbo.MReturJualD ON dbo.MReturJualD.IDHeader = dbo.MReturJual.NoID) " & vbCrLf & _
    '    '                            "LEFT JOIN dbo.MBarangD ON dbo.MReturJualD.IDBarangD = dbo.MBarangD.NoID) " & vbCrLf & _
    '    '                            "LEFT JOIN dbo.MSatuan ON dbo.MReturJualD.IDSatuan = dbo.MSatuan.NoID" & vbCrLf & _
    '    '                            "LEFT JOIN dbo.MBarang ON dbo.MReturJualD.IDBarang = dbo.MBarang.NoID " & vbCrLf & _
    '    '                            "LEFT JOIN dbo.MTypePajak ON dbo.MReturJual.IDTypePajak = dbo.MTypePajak.NoID " & vbCrLf & _
    '    '                            "LEFT JOIN dbo.MAlamat ON dbo.MAlamat.NoID = dbo.MReturJual.IDCustomer " & vbCrLf & _
    '    '                            "LEFT JOIN dbo.MUser AS UserPosted ON UserPosted.NoID = dbo.MReturJual.IDUserPosted" & vbCrLf & _
    '    '                            "LEFT JOIN dbo.MUser AS UserEntry ON dbo.MReturJual.IDUserEntry = UserEntry.NoID" & vbCrLf & _
    '    '                            "LEFT JOIN dbo.MUser AS UserEdit ON dbo.MReturJual.IDUserEdit = UserEdit.NoID" & vbCrLf & _
    '    '                            "WHERE dbo.MReturJual.IsPosted = 1 AND dbo.MReturJual.NoID = @NoID" & vbCrLf & _
    '    '                            "END"
    '    '            com.ExecuteNonQuery()
    '    '            com.Transaction.Commit()
    '    '        End If
    '    '    Catch ex As Exception

    '    '    Finally
    '    '        cn.Close()
    '    '        cn.Open()
    '    '        com.Connection = cn
    '    '    End Try
    '    'End Sub
    '#End Region
End Class

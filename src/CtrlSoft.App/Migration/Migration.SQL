USE [master];
GO

/****** Object:  Database [DBPOS]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE DATABASE [DBPOS] ON PRIMARY
(
                                   NAME = N'DBPOS', 
                                   FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\DBPOS_data.mdf', 
                                   SIZE = 5440 KB, 
                                   MAXSIZE = UNLIMITED, 
                                   FILEGROWTH = 1024 KB
) LOG ON
(
                                   NAME = N'DBPOS_log', 
                                   FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\DBPOS_log.ldf', 
                                   SIZE = 11136 KB, 
                                   MAXSIZE = 2048 GB, 
                                   FILEGROWTH = 10%
);
GO
ALTER DATABASE [DBPOS] SET COMPATIBILITY_LEVEL = 100;
GO
IF(1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
    BEGIN
        EXEC [DBPOS].[dbo].[sp_fulltext_database] 
             @action = 'enable';
    END;
GO
ALTER DATABASE [DBPOS] SET ANSI_NULL_DEFAULT OFF; 
GO
ALTER DATABASE [DBPOS] SET ANSI_NULLS OFF; 
GO
ALTER DATABASE [DBPOS] SET ANSI_PADDING OFF; 
GO
ALTER DATABASE [DBPOS] SET ANSI_WARNINGS OFF; 
GO
ALTER DATABASE [DBPOS] SET ARITHABORT OFF; 
GO
ALTER DATABASE [DBPOS] SET AUTO_CLOSE OFF; 
GO
ALTER DATABASE [DBPOS] SET AUTO_SHRINK OFF; 
GO
ALTER DATABASE [DBPOS] SET AUTO_UPDATE_STATISTICS ON; 
GO
ALTER DATABASE [DBPOS] SET CURSOR_CLOSE_ON_COMMIT OFF; 
GO
ALTER DATABASE [DBPOS] SET CURSOR_DEFAULT GLOBAL; 
GO
ALTER DATABASE [DBPOS] SET CONCAT_NULL_YIELDS_NULL OFF; 
GO
ALTER DATABASE [DBPOS] SET NUMERIC_ROUNDABORT OFF; 
GO
ALTER DATABASE [DBPOS] SET QUOTED_IDENTIFIER OFF; 
GO
ALTER DATABASE [DBPOS] SET RECURSIVE_TRIGGERS OFF; 
GO
ALTER DATABASE [DBPOS] SET DISABLE_BROKER; 
GO
ALTER DATABASE [DBPOS] SET AUTO_UPDATE_STATISTICS_ASYNC OFF; 
GO
ALTER DATABASE [DBPOS] SET DATE_CORRELATION_OPTIMIZATION OFF; 
GO
ALTER DATABASE [DBPOS] SET TRUSTWORTHY OFF; 
GO
ALTER DATABASE [DBPOS] SET ALLOW_SNAPSHOT_ISOLATION OFF; 
GO
ALTER DATABASE [DBPOS] SET PARAMETERIZATION SIMPLE; 
GO
ALTER DATABASE [DBPOS] SET READ_COMMITTED_SNAPSHOT OFF; 
GO
ALTER DATABASE [DBPOS] SET HONOR_BROKER_PRIORITY OFF; 
GO
ALTER DATABASE [DBPOS] SET RECOVERY FULL; 
GO
ALTER DATABASE [DBPOS] SET MULTI_USER; 
GO
ALTER DATABASE [DBPOS] SET PAGE_VERIFY CHECKSUM;  
GO
ALTER DATABASE [DBPOS] SET DB_CHAINING OFF; 
GO
EXEC sys.sp_db_vardecimal_storage_format 
     N'DBPOS', 
     N'ON';
GO
USE [DBPOS];
GO

/****** Object:  UserDefinedFunction [dbo].[fnBulatkanNew]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE FUNCTION [dbo].[fnBulatkanNew]
(@AngkaAsal AS       MONEY    = 0.0, 
 @NilaiPembulatan AS MONEY    = 0.0, 
 @TypeBulat AS       SMALLINT = 0 --0:Keatas, 1:Kebawah, 2:SetengahKeatas
)
RETURNS MONEY
AS
     BEGIN
         DECLARE @Hasiltmp AS MONEY= 0.0;
         DECLARE @Hasil AS MONEY= 0.0;
         SET @Hasiltmp = CONVERT(FLOAT, @AngkaAsal);
         IF(@NilaiPembulatan = 0.0)
             SET @Hasil = @AngkaAsal;
             ELSE
             IF(@NilaiPembulatan <= 0.01)
                 BEGIN
                     --Return Math.Round(AngkaAsal, 2)
                     SET @Hasiltmp = (ROUND(@AngkaAsal, 2) * 100);
                     IF(@Hasiltmp % 5.0 <> 0.0) --'ada sisa maka tambah 5
                         BEGIN
                             SET @Hasiltmp = 5.0 * (FLOOR(@Hasiltmp / 5.0)) + 5.0;
                         END;
                     SET @Hasil = @Hasiltmp / 100;
                 END;
                 ELSE
                 IF(@NilaiPembulatan <= 0.1) --= 1 DIGIT belakang koma
                     BEGIN
                         SET @Hasil = ROUND(@AngkaAsal, 1);
                     END;
                     ELSE
                     IF(@NilaiPembulatan > 0)
                         BEGIN
                             IF(@TypeBulat = 0)
                                 BEGIN
                                     SET @Hasil = (FLOOR(@Hasiltmp / @NilaiPembulatan)) * @NilaiPembulatan + CASE
                                                                                                                 WHEN @Hasiltmp % @NilaiPembulatan > 0.0
                                                                                                                 THEN @NilaiPembulatan
                                                                                                                 ELSE 0
                                                                                                             END;
                                 END;
                                 ELSE
                                 IF(@TypeBulat = 1)
                                     BEGIN
                                         SET @Hasil = FLOOR(@Hasiltmp / @NilaiPembulatan) * @NilaiPembulatan;
                                     END;
                                     ELSE
                                     IF(@AngkaAsal % @NilaiPembulatan) > (@NilaiPembulatan / 2.0)
                                         SET @Hasil = FLOOR(@Hasiltmp / @NilaiPembulatan) * @NilaiPembulatan + @NilaiPembulatan;
                                         ELSE
                                         SET @Hasil = FLOOR(@Hasiltmp / @NilaiPembulatan) * @NilaiPembulatan;
                         END;
                         ELSE
                         BEGIN
                             SET @Hasil = ROUND(@AngkaAsal, 0);
                         END;
         RETURN @Hasil;
     END;
GO

/****** Object:  UserDefinedFunction [dbo].[sfn_ean_chkdigit]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE FUNCTION [dbo].[sfn_ean_chkdigit]
(@barcode VARCHAR(20)
)
RETURNS CHAR(1)
AS
     BEGIN
         DECLARE @chk_digit INT, @chk INT;
         DECLARE @num TABLE(num INT);
         IF LEN(@barcode) NOT IN(7, 12)
             BEGIN
                 RETURN NULL;
             END;
         INSERT INTO @num
                SELECT 1
                UNION ALL
                SELECT 2
                UNION ALL
                SELECT 3
                UNION ALL
                SELECT 4
                UNION ALL
                SELECT 5
                UNION ALL
                SELECT 6
                UNION ALL
                SELECT 7
                UNION ALL
                SELECT 8
                UNION ALL
                SELECT 9
                UNION ALL
                SELECT 10
                UNION ALL
                SELECT 11
                UNION ALL
                SELECT 12;
         SELECT @chk_digit = SUM(CONVERT(INT, SUBSTRING(@barcode, LEN(@barcode) - num + 1, 1)) * CASE
                                                                                                     WHEN num % 2 = 1
                                                                                                     THEN 3
                                                                                                     ELSE 1
                                                                                                 END)
         FROM @num
         WHERE num <= LEN(@barcode);
         SELECT @chk_digit = (10 - (@chk_digit % 10)) % 10;
         RETURN CHAR(ASCII('0') + @chk_digit);
     END;
GO

/****** Object:  Table [dbo].[IMP_BRG]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[IMP_BRG]
([NO]          [INT] NOT NULL, 
 [NAMA BARANG] [NVARCHAR](255) NULL, 
 [MERECK]      [NVARCHAR](255) NULL, 
 [UKURAN]      [NVARCHAR](255) NULL, 
 [NETTO]       [FLOAT] NULL, 
 [HARGA JUAL]  [FLOAT] NULL, 
 [STOK AWAL]   [FLOAT] NULL, 
 [SAAT INI]    [FLOAT] NULL, 
 [MARGIN]      [FLOAT] NULL, 
 [NILAI STOCK] [FLOAT] NULL, 
 CONSTRAINT [PK_IMP_BRG] PRIMARY KEY CLUSTERED([NO] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[IMP_BRG2]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[IMP_BRG2]
([NO]          [INT] IDENTITY(1, 1) NOT NULL, 
 [KATEGORI]    [NVARCHAR](255) NULL, 
 [MERK]        [NVARCHAR](255) NULL, 
 [NAMA]        [NVARCHAR](255) NULL, 
 [KODEWARNA]   [NVARCHAR](255) NULL, 
 [WARNA]       [NVARCHAR](255) NULL, 
 [UKURAN]      [NVARCHAR](255) NULL, 
 [NAMABARANG]  [NVARCHAR](255) NULL, 
 [SATUAN]      [NVARCHAR](255) NULL, 
 [HARGABELI]   [FLOAT] NULL, 
 [HARGAJUAL]   [FLOAT] NULL, 
 [SATUANBESAR] [NVARCHAR](255) NULL, 
 [ISIBESAR]    [FLOAT] NULL, 
 [HARGABESAR]  [FLOAT] NULL, 
 CONSTRAINT [PK_IMP_BRG2] PRIMARY KEY CLUSTERED([NO] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MAlamat]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MAlamat]
([NoID]             [INT] NOT NULL, 
 [Kode]             [VARCHAR](50) NULL, 
 [Nama]             [VARCHAR](50) NULL, 
 [NamaAlias]        [VARCHAR](50) NULL, 
 [Alamat]           [VARCHAR](250) NULL, 
 [Kota]             [VARCHAR](25) NULL, 
 [HP]               [VARCHAR](15) NULL, 
 [Telp]             [VARCHAR](15) NULL, 
 [ContactPerson]    [VARCHAR](50) NULL, 
 [LimitHutang]      [MONEY] NULL, 
 [LimitPiutang]     [MONEY] NULL, 
 [LimitNotaPiutang] [INT] NULL, 
 [LimitUmurPiutang] [INT] NULL, 
 [IDTypeHarga]      [INT] NULL, 
 [IsActive]         [BIT] NULL, 
 [IsSupplier]       [BIT] NULL, 
 [IsPegawai]        [BIT] NULL, 
 [IsCustomer]       [BIT] NULL, 
 CONSTRAINT [PK_MAlamat] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MAssembly]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MAssembly]
([NoID]         [BIGINT] NOT NULL, 
 [Kode]         [VARCHAR](50) NOT NULL, 
 [Tanggal]      [DATETIME] NOT NULL, 
 [IDWOAssembly] [UNIQUEIDENTIFIER] NULL, 
 [IDMaterial]   [UNIQUEIDENTIFIER] NOT NULL, 
 [IDPegawai]    [BIGINT] NOT NULL, 
 [IDGudang]     [INT] NOT NULL, 
 [Catatan]      [VARCHAR](150) NOT NULL, 
 [IDBarangD]    [BIGINT] NOT NULL, 
 [IDBarang]     [BIGINT] NOT NULL, 
 [IDSatuan]     [INT] NOT NULL, 
 [Konversi]     [NUMERIC](18, 0) NOT NULL, 
 [Qty]          [NUMERIC](18, 3) NOT NULL, 
 [HargaPokok]   [MONEY] NOT NULL, 
 [Jumlah]       [MONEY] NOT NULL, 
 [IsPosted]     [BIT] NULL, 
 [TglPosted]    [DATETIME] NULL, 
 [IDUserPosted] [INT] NULL, 
 [IDUserEntry]  [INT] NULL, 
 [IDUserEdit]   [INT] NULL, 
 CONSTRAINT [PK_MAssembly] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MAssemblyD]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MAssemblyD]
([NoID]         [BIGINT] NOT NULL, 
 [IDHeader]     [BIGINT] NOT NULL, 
 [IDMaterialD]  [UNIQUEIDENTIFIER] NOT NULL, 
 [IDBarangD]    [BIGINT] NOT NULL, 
 [Tanggal]      [DATETIME] NOT NULL, 
 [IDBarang]     [BIGINT] NOT NULL, 
 [IDSatuan]     [INT] NOT NULL, 
 [Konversi]     [NUMERIC](18, 0) NOT NULL, 
 [Qty]          [NUMERIC](18, 3) NOT NULL, 
 [HargaPokok]   [MONEY] NOT NULL, 
 [Jumlah]       [MONEY] NOT NULL, 
 [IsPosted]     [BIT] NULL, 
 [TglPosted]    [DATETIME] NULL, 
 [IDUserPosted] [INT] NULL, 
 [IDUserEntry]  [INT] NULL, 
 [IDUserEdit]   [INT] NULL, 
 CONSTRAINT [PK_MAssemblyD] PRIMARY KEY CLUSTERED([NoID] ASC, [IDHeader] ASC, [IDMaterialD] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MAssemblyDBiaya]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MAssemblyDBiaya]
([NoID]             [BIGINT] NOT NULL, 
 [IDHeader]         [BIGINT] NOT NULL, 
 [IDMaterialDBiaya] [UNIQUEIDENTIFIER] NOT NULL, 
 [Tanggal]          [DATETIME] NOT NULL, 
 [IDAkun]           [UNIQUEIDENTIFIER] NOT NULL, 
 [Jumlah]           [MONEY] NOT NULL, 
 [Keterangan]       [VARCHAR](150) NULL, 
 [IsPosted]         [BIT] NULL, 
 [TglPosted]        [DATETIME] NULL, 
 [IDUserPosted]     [INT] NULL, 
 [IDUserEntry]      [INT] NULL, 
 [IDUserEdit]       [INT] NULL, 
 CONSTRAINT [PK_MAssemblyDBiaya] PRIMARY KEY CLUSTERED([NoID] ASC, [IDHeader] ASC, [IDMaterialDBiaya] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MAssemblyDSisa]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MAssemblyDSisa]
([NoID]            [BIGINT] NOT NULL, 
 [IDHeader]        [BIGINT] NOT NULL, 
 [IDMaterialDSisa] [UNIQUEIDENTIFIER] NOT NULL, 
 [Tanggal]         [DATETIME] NOT NULL, 
 [IDBarangD]       [BIGINT] NOT NULL, 
 [IDBarang]        [BIGINT] NOT NULL, 
 [IDSatuan]        [INT] NOT NULL, 
 [Konversi]        [NUMERIC](18, 0) NOT NULL, 
 [Qty]             [NUMERIC](18, 3) NOT NULL, 
 [HargaPokok]      [MONEY] NOT NULL, 
 [Jumlah]          [MONEY] NOT NULL, 
 [IsPosted]        [BIT] NULL, 
 [TglPosted]       [DATETIME] NULL, 
 [IDUserPosted]    [INT] NULL, 
 [IDUserEntry]     [INT] NULL, 
 [IDUserEdit]      [INT] NULL, 
 CONSTRAINT [PK_MAssemblyDSisa] PRIMARY KEY CLUSTERED([NoID] ASC, [IDHeader] ASC, [IDMaterialDSisa] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MBarang]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MBarang]
([NoID]              [INT] NOT NULL, 
 [Kode]              [VARCHAR](15) NULL, 
 [Nama]              [VARCHAR](50) NULL, 
 [IDKategori]        [INT] NULL, 
 [DefaultBarcode]    [VARCHAR](25) NULL, 
 [Alias]             [VARCHAR](50) NULL, 
 [Keterangan]        [VARCHAR](50) NULL, 
 [IsActive]          [BIT] NULL, 
 [IDTypePajak]       [SMALLINT] NULL, 
 [IDSupplier1]       [INT] NULL, 
 [IDSupplier2]       [INT] NULL, 
 [IDSupplier3]       [INT] NULL, 
 [HargaBeli]         [MONEY] NULL, 
 [IDSatuanBeli]      [INT] NULL, 
 [IsiCtn]            [INT] NULL, 
 [HargaBeliPcsBruto] [MONEY] NULL, 
 [DiscProsen1]       [NUMERIC](18, 2) NULL, 
 [DiscProsen2]       [NUMERIC](18, 2) NULL, 
 [DiscProsen3]       [NUMERIC](18, 2) NULL, 
 [DiscProsen4]       [NUMERIC](18, 2) NULL, 
 [DiscProsen5]       [NUMERIC](18, 2) NULL, 
 [DiscRp]            [MONEY] NULL, 
 [HargaBeliPcs]      [MONEY] NULL, 
 [IDSatuan]          [INT] NULL, 
 [ProsenUpA]         [NUMERIC](18, 2) NULL, 
 [HargaJualA]        [MONEY] NULL, 
 [ProsenUpB]         [NUMERIC](18, 2) NULL, 
 [HargaJualB]        [MONEY] NULL, 
 [IDUser]            [INT] NULL, 
 [TanggalUpdate]     [DATETIME] NULL, 
 [IDMerk]            [INT] NULL, 
 [HargaJualA1]       [MONEY] NULL, 
 [HargaJualB1]       [MONEY] NULL, 
 CONSTRAINT [PK_MBarang] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MBarangD]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MBarangD]
([NoID]       [INT] NOT NULL, 
 [IDBarang]   [INT] NULL, 
 [IDSatuan]   [INT] NULL, 
 [Konversi]   [INT] NULL, 
 [Barcode]    [VARCHAR](25) NULL, 
 [IsDefault]  [BIT] NULL, 
 [IsActive]   [BIT] NULL, 
 [ProsenUpA]  [NUMERIC](18, 2) NULL, 
 [HargaJualA] [MONEY] NULL, 
 [ProsenUpB]  [NUMERIC](18, 2) NULL, 
 [HargaJualB] [MONEY] NULL, 
 CONSTRAINT [PK_MBarangD] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MBeli]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MBeli]
([NoID]           [BIGINT] NOT NULL, 
 [Kode]           [VARCHAR](50) NULL, 
 [NoReff]         [VARCHAR](50) NULL, 
 [IDSupplier]     [INT] NULL, 
 [IDPO]           [BIGINT] NULL, 
 [IDGudang]       [INT] NULL, 
 [Tanggal]        [DATETIME] NOT NULL, 
 [JatuhTempo]     [DATE] NULL, 
 [Catatan]        [VARCHAR](250) NULL, 
 [IDTypePajak]    [SMALLINT] NULL, 
 [Subtotal]       [MONEY] NULL, 
 [DiscNotaProsen] [NUMERIC](18, 2) NULL, 
 [DiscNotaRp]     [MONEY] NULL, 
 [TotalBruto]     [MONEY] NULL, 
 [DPP]            [MONEY] NULL, 
 [PPN]            [MONEY] NULL, 
 [Total]          [MONEY] NULL, 
 [IsPosted]       [BIT] NULL, 
 [TglPosted]      [DATETIME] NULL, 
 [IDUserPosted]   [INT] NULL, 
 [IDUserEntry]    [INT] NULL, 
 [IDUserEdit]     [INT] NULL, 
 CONSTRAINT [PK_MBeli] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MBeliD]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MBeliD]
([NoID]           [BIGINT] NOT NULL, 
 [IDHeader]       [BIGINT] NULL, 
 [IDPOD]          [BIGINT] NULL, 
 [IDBarangD]      [BIGINT] NULL, 
 [IDBarang]       [BIGINT] NULL, 
 [IDSatuan]       [INT] NULL, 
 [Konversi]       [NUMERIC](18, 0) NULL, 
 [Qty]            [NUMERIC](18, 3) NULL, 
 [Harga]          [MONEY] NULL, 
 [DiscProsen1]    [NUMERIC](18, 2) NULL, 
 [DiscProsen2]    [NUMERIC](18, 2) NULL, 
 [DiscProsen3]    [NUMERIC](18, 2) NULL, 
 [DiscProsen4]    [NUMERIC](18, 2) NULL, 
 [DiscProsen5]    [NUMERIC](18, 2) NULL, 
 [DiscRp]         [MONEY] NULL, 
 [DiscNotaProsen] [NUMERIC](18, 2) NULL, 
 [DiscNotaRp]     [MONEY] NULL, 
 [JumlahBruto]    [MONEY] NULL, 
 [DPP]            [MONEY] NULL, 
 [PPN]            [MONEY] NULL, 
 [Jumlah]         [MONEY] NULL, 
 CONSTRAINT [PK_MBeliD] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MGudang]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MGudang]
([NoID]     [INT] NOT NULL, 
 [Kode]     [VARCHAR](50) NULL, 
 [Nama]     [VARCHAR](50) NULL, 
 [IsActive] [BIT] NULL, 
 CONSTRAINT [PK_MGudang] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MHutangPiutang]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MHutangPiutang]
([NoID]             [BIGINT] IDENTITY(1, 1) NOT NULL, 
 [IDAlamat]         [INT] NOT NULL, 
 [IDTransaksi]      [BIGINT] NOT NULL, 
 [IDJenisTransaksi] [INT] NOT NULL, 
 [NoTransaksi]      [VARCHAR](50) NOT NULL, 
 [NoUrut]           [SMALLINT] NOT NULL, 
 [Tanggal]          [DATE] NULL, 
 [JatuhTempo]       [DATE] NULL, 
 [Debet]            [MONEY] NULL, 
 [Kredit]           [MONEY] NULL, 
 [Saldo]            [MONEY] NULL, 
 [ReffNoTransaksi]  [VARCHAR](50) NULL, 
 [ReffNoUrut]       [SMALLINT] NULL, 
 CONSTRAINT [PK_MHutangPiutang] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MJenisPembayaran]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MJenisPembayaran]
([NoID]         [INT] NOT NULL, 
 [Kode]         [VARCHAR](50) NULL, 
 [Nama]         [VARCHAR](50) NULL, 
 [IsBank]       [BIT] NULL, 
 [ChargeProsen] [NUMERIC](18, 2) NULL, 
 CONSTRAINT [PK_MJenisPembayaran] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MJenisTransaksi]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MJenisTransaksi]
([ID]           [INT] NOT NULL, 
 [Kode]         [NVARCHAR](3) NULL, 
 [Nama]         [NVARCHAR](25) NULL, 
 [Keterangan]   [NVARCHAR](50) NULL, 
 [IsAuto]       [BIT] NULL, 
 [TERURUT]      [INT] NULL, 
 [IDServerAsal] [INT] NULL, 
 [NoUrut]       [INT] NULL, 
 CONSTRAINT [PK_MJenisTransaksi] PRIMARY KEY CLUSTERED([ID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MJenisTransaksiD]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MJenisTransaksiD]
([NoID]             [INT] NOT NULL, 
 [IDJenisTransaksi] [INT] NOT NULL, 
 [Kode]             [VARCHAR](50) NULL, 
 [Nama]             [VARCHAR](50) NULL, 
 CONSTRAINT [PK_MJenisTransaksiD] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MJual]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MJual]
([NoID]           [BIGINT] NOT NULL, 
 [Kode]           [VARCHAR](50) NULL, 
 [NoReff]         [VARCHAR](50) NULL, 
 [IDCustomer]     [INT] NULL, 
 [IDGudang]       [INT] NULL, 
 [Tanggal]        [DATETIME] NOT NULL, 
 [JatuhTempo]     [DATE] NULL, 
 [Catatan]        [VARCHAR](250) NULL, 
 [IDTypePajak]    [SMALLINT] NULL, 
 [Subtotal]       [MONEY] NULL, 
 [DiscNotaProsen] [NUMERIC](18, 2) NULL, 
 [DiscNotaRp]     [MONEY] NULL, 
 [TotalBruto]     [MONEY] NULL, 
 [DPP]            [MONEY] NULL, 
 [PPN]            [MONEY] NULL, 
 [Total]          [MONEY] NULL, 
 [Bayar]          [MONEY] NULL, 
 [Sisa]           [MONEY] NULL, 
 [IsPosted]       [BIT] NULL, 
 [TglPosted]      [DATETIME] NULL, 
 [IDUserPosted]   [INT] NULL, 
 [IDUserEntry]    [INT] NULL, 
 [IDUserEdit]     [INT] NULL, 
 CONSTRAINT [PK_MJual] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MJualD]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MJualD]
([NoID]           [BIGINT] NOT NULL, 
 [IDHeader]       [BIGINT] NULL, 
 [IDBarangD]      [BIGINT] NULL, 
 [IDBarang]       [BIGINT] NULL, 
 [IDSatuan]       [INT] NULL, 
 [Konversi]       [NUMERIC](18, 0) NULL, 
 [Qty]            [NUMERIC](18, 3) NULL, 
 [Harga]          [MONEY] NULL, 
 [DiscProsen1]    [NUMERIC](18, 2) NULL, 
 [DiscProsen2]    [NUMERIC](18, 2) NULL, 
 [DiscProsen3]    [NUMERIC](18, 2) NULL, 
 [DiscProsen4]    [NUMERIC](18, 2) NULL, 
 [DiscProsen5]    [NUMERIC](18, 2) NULL, 
 [DiscRp]         [MONEY] NULL, 
 [DiscNotaProsen] [NUMERIC](18, 2) NULL, 
 [DiscNotaRp]     [MONEY] NULL, 
 [JumlahBruto]    [MONEY] NULL, 
 [DPP]            [MONEY] NULL, 
 [PPN]            [MONEY] NULL, 
 [Jumlah]         [MONEY] NULL, 
 CONSTRAINT [PK_MJualD] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MJualDBayar]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MJualDBayar]
([NoID]              [BIGINT] IDENTITY(1, 1) NOT NULL, 
 [IDHeader]          [BIGINT] NOT NULL, 
 [IDJenisPembayaran] [INT] NOT NULL, 
 [AtasNama]          [VARCHAR](50) NULL, 
 [NoRekening]        [VARCHAR](150) NULL, 
 [Nominal]           [MONEY] NOT NULL, 
 [ChargeProsen]      [NUMERIC](18, 3) NULL, 
 [ChargeRp]          [MONEY] NULL, 
 [Total]             [MONEY] NULL, 
 CONSTRAINT [PK_MJualDBayar] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MKartuStok]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MKartuStok]
([NoID]             [BIGINT] IDENTITY(1, 1) NOT NULL, 
 [IDBarang]         [BIGINT] NOT NULL, 
 [IDBarangD]        [BIGINT] NOT NULL, 
 [Varian]           [VARCHAR](50) NOT NULL, 
 [IDJenisTransaksi] [INT] NOT NULL, 
 [IDTransaksi]      [BIGINT] NOT NULL, 
 [IDTransaksiD]     [BIGINT] NOT NULL, 
 [IDGudang]         [INT] NOT NULL, 
 [Tanggal]          [DATETIME] NULL, 
 [IDSatuan]         [INT] NULL, 
 [Konversi]         [NUMERIC](18, 0) NULL, 
 [QtyMasuk]         [NUMERIC](18, 3) NULL, 
 [QtyKeluar]        [NUMERIC](18, 3) NULL, 
 [Debet]            [MONEY] NULL, 
 [Kredit]           [MONEY] NULL, 
 [HargaBeli]        [MONEY] NULL, 
 [HPP]              [MONEY] NULL, 
 [SaldoAkhir]       [NUMERIC](18, 3) NULL, 
 [NilaiAkhir]       [MONEY] NULL, 
 CONSTRAINT [PK_MKartuStok] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MKartuStokOnHand]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MKartuStokOnHand]
([NoID]             [BIGINT] IDENTITY(1, 1) NOT NULL, 
 [IDBarang]         [BIGINT] NOT NULL, 
 [IDBarangD]        [BIGINT] NOT NULL, 
 [Varian]           [VARCHAR](50) NOT NULL, 
 [IDJenisTransaksi] [INT] NOT NULL, 
 [IDTransaksi]      [BIGINT] NOT NULL, 
 [IDTransaksiD]     [BIGINT] NOT NULL, 
 [IDGudang]         [INT] NOT NULL, 
 [IDSatuan]         [INT] NULL, 
 [Konversi]         [NUMERIC](18, 0) NULL, 
 [QtyMasuk]         [NUMERIC](18, 3) NULL, 
 [QtyKeluar]        [NUMERIC](18, 3) NULL, 
 [Debet]            [MONEY] NULL, 
 [Kredit]           [MONEY] NULL, 
 [HargaBeli]        [MONEY] NULL, 
 [HPP]              [MONEY] NULL, 
 [SaldoAkhir]       [NUMERIC](18, 3) NULL, 
 [NilaiAkhir]       [MONEY] NULL, 
 CONSTRAINT [PK_MKartuStokOnHand] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MKasIN]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MKasIN]
([NoID]         [INT] NOT NULL, 
 [Tanggal]      [DATETIME] NULL, 
 [IDKontak]     [INT] NULL, 
 [Kode]         [VARCHAR](50) NULL, 
 [KodeReff]     [VARCHAR](50) NULL, 
 [Keterangan]   [VARCHAR](MAX) NULL, 
 [Total]        [MONEY] NULL, 
 [IDUserEntry]  [INT] NULL, 
 [TglEntry]     [DATETIME] NULL, 
 [IDUserEdit]   [INT] NULL, 
 [TglEdit]      [DATETIME] NULL, 
 [IsPosted]     [BIT] NULL, 
 [IDUserPosted] [INT] NULL, 
 [TglPosted]    [DATETIME] NULL, 
 CONSTRAINT [PK_MKasIN] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MKasIND]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MKasIND]
([IDKasIN]         [INT] NOT NULL, 
 [NoUrut]          [INT] NOT NULL, 
 [ReffNoTransaksi] [VARCHAR](50) NULL, 
 [ReffNoUrut]      [SMALLINT] NULL, 
 [Debet]           [MONEY] NULL, 
 [Kredit]          [MONEY] NULL, 
 [Keterangan]      [VARCHAR](MAX) NULL, 
 CONSTRAINT [PK_MKasIND] PRIMARY KEY CLUSTERED([IDKasIN] ASC, [NoUrut] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MKasINDBayar]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MKasINDBayar]
([IDKasIN]           [INT] NOT NULL, 
 [IDJenisPembayaran] [INT] NOT NULL, 
 [AtasNama]          [VARCHAR](50) NULL, 
 [NoRekening]        [VARCHAR](150) NULL, 
 [Nominal]           [MONEY] NOT NULL, 
 [ChargeProsen]      [NUMERIC](18, 3) NULL, 
 [ChargeRp]          [MONEY] NULL, 
 [Total]             [MONEY] NULL, 
 CONSTRAINT [PK_MKasINDBayar] PRIMARY KEY CLUSTERED([IDKasIN] ASC, [IDJenisPembayaran] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MKasOut]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MKasOut]
([NoID]         [INT] NOT NULL, 
 [Tanggal]      [DATETIME] NULL, 
 [IDKontak]     [INT] NULL, 
 [Kode]         [VARCHAR](50) NULL, 
 [KodeReff]     [VARCHAR](50) NULL, 
 [Keterangan]   [VARCHAR](MAX) NULL, 
 [Total]        [MONEY] NULL, 
 [IDUserEntry]  [INT] NULL, 
 [TglEntry]     [DATETIME] NULL, 
 [IDUserEdit]   [INT] NULL, 
 [TglEdit]      [DATETIME] NULL, 
 [IsPosted]     [BIT] NULL, 
 [IDUserPosted] [INT] NULL, 
 [TglPosted]    [DATETIME] NULL, 
 CONSTRAINT [PK_MKasOut] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MKasOutD]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MKasOutD]
([IDKasOut]        [INT] NOT NULL, 
 [NoUrut]          [INT] NOT NULL, 
 [ReffNoTransaksi] [VARCHAR](50) NULL, 
 [ReffNoUrut]      [SMALLINT] NULL, 
 [Debet]           [MONEY] NULL, 
 [Kredit]          [MONEY] NULL, 
 [Keterangan]      [VARCHAR](MAX) NULL, 
 CONSTRAINT [PK_MKasOutD] PRIMARY KEY CLUSTERED([IDKasOut] ASC, [NoUrut] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MKasOutDBayar]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MKasOutDBayar]
([IDKasOut]          [INT] NOT NULL, 
 [IDJenisPembayaran] [INT] NOT NULL, 
 [AtasNama]          [VARCHAR](50) NULL, 
 [NoRekening]        [VARCHAR](150) NULL, 
 [Nominal]           [MONEY] NOT NULL, 
 [ChargeProsen]      [NUMERIC](18, 3) NULL, 
 [ChargeRp]          [MONEY] NULL, 
 [Total]             [MONEY] NULL, 
 CONSTRAINT [PK_MKasOutDBayar] PRIMARY KEY CLUSTERED([IDKasOut] ASC, [IDJenisPembayaran] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MKategori]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MKategori]
([NoID]     [INT] NOT NULL, 
 [Kode]     [VARCHAR](50) NULL, 
 [Nama]     [VARCHAR](50) NULL, 
 [IDParent] [INT] NULL, 
 [IsActive] [BIT] NULL, 
 CONSTRAINT [PK_MKategori] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MMaterial]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MMaterial]
([NoID]          [UNIQUEIDENTIFIER] NOT NULL, 
 [Kode]          [VARCHAR](50) NOT NULL, 
 [Nama]          [VARCHAR](150) NOT NULL, 
 [Keterangan]    [VARCHAR](150) NOT NULL, 
 [IsActive]      [BIT] NOT NULL, 
 [IDBarangD]     [BIGINT] NOT NULL, 
 [IDBarang]      [BIGINT] NOT NULL, 
 [IDSatuan]      [INT] NOT NULL, 
 [Konversi]      [NUMERIC](18, 0) NOT NULL, 
 [Qty]           [NUMERIC](18, 3) NOT NULL, 
 [HargaPokok]    [MONEY] NOT NULL, 
 [Jumlah]        [MONEY] NOT NULL, 
 [IDUser]        [INT] NOT NULL, 
 [TanggalUpdate] [DATETIME] NOT NULL, 
 CONSTRAINT [PK_MMaterial] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MMaterialD]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MMaterialD]
([NoID]       [UNIQUEIDENTIFIER] NOT NULL, 
 [IDMaterial] [UNIQUEIDENTIFIER] NOT NULL, 
 [IDBarangD]  [BIGINT] NOT NULL, 
 [IDBarang]   [BIGINT] NOT NULL, 
 [IDSatuan]   [INT] NOT NULL, 
 [Konversi]   [NUMERIC](18, 0) NOT NULL, 
 [Qty]        [NUMERIC](18, 3) NOT NULL, 
 [HargaPokok] [MONEY] NOT NULL, 
 [Jumlah]     [MONEY] NOT NULL, 
 CONSTRAINT [PK_MMaterialD] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MMaterialDBiaya]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MMaterialDBiaya]
([NoID]       [UNIQUEIDENTIFIER] NOT NULL, 
 [IDMaterial] [UNIQUEIDENTIFIER] NOT NULL, 
 [IDAkun]     [UNIQUEIDENTIFIER] NOT NULL, 
 [Jumlah]     [MONEY] NOT NULL, 
 [Keterangan] [VARCHAR](150) NULL, 
 CONSTRAINT [PK_MMaterialDBiaya] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MMaterialDSisa]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MMaterialDSisa]
([NoID]       [UNIQUEIDENTIFIER] NOT NULL, 
 [IDMaterial] [UNIQUEIDENTIFIER] NOT NULL, 
 [IDBarangD]  [BIGINT] NOT NULL, 
 [IDBarang]   [BIGINT] NOT NULL, 
 [IDSatuan]   [INT] NOT NULL, 
 [Konversi]   [NUMERIC](18, 0) NOT NULL, 
 [Qty]        [NUMERIC](18, 3) NOT NULL, 
 [HargaPokok] [MONEY] NOT NULL, 
 [Jumlah]     [MONEY] NOT NULL, 
 CONSTRAINT [PK_MMaterialDSisa] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MMenu]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MMenu]
([NoID]         [INT] NOT NULL, 
 [IDParent]     [INT] NULL, 
 [NoUrut]       [INT] NULL, 
 [Name]         [VARCHAR](50) NULL, 
 [Caption]      [VARCHAR](50) NULL, 
 [IsBig]        [BIT] NULL, 
 [IsBeginGroup] [BIT] NULL, 
 [IsActive]     [BIT] NULL, 
 CONSTRAINT [PK_MMenu] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MMerk]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MMerk]
([NoID]     [INT] NOT NULL, 
 [Kode]     [VARCHAR](50) NULL, 
 [Nama]     [VARCHAR](50) NULL, 
 [IsActive] [BIT] NULL, 
 CONSTRAINT [PK_MMerk] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MMutasiGudang]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MMutasiGudang]
([NoID]           [BIGINT] NOT NULL, 
 [Kode]           [VARCHAR](50) NULL, 
 [NoReff]         [VARCHAR](50) NULL, 
 [IDGudangAsal]   [INT] NULL, 
 [IDGudangTujuan] [INT] NULL, 
 [Tanggal]        [DATETIME] NOT NULL, 
 [Catatan]        [VARCHAR](250) NULL, 
 [Total]          [MONEY] NULL, 
 [IsPosted]       [BIT] NULL, 
 [TglPosted]      [DATETIME] NULL, 
 [IDUserPosted]   [INT] NULL, 
 [IDUserEntry]    [INT] NULL, 
 [IDUserEdit]     [INT] NULL, 
 CONSTRAINT [PK_MMutasiGudang] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MMutasiGudangD]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MMutasiGudangD]
([NoID]       [BIGINT] NOT NULL, 
 [IDHeader]   [BIGINT] NULL, 
 [IDBarangD]  [BIGINT] NULL, 
 [IDBarang]   [BIGINT] NULL, 
 [IDSatuan]   [INT] NULL, 
 [Konversi]   [NUMERIC](18, 0) NULL, 
 [Qty]        [NUMERIC](18, 3) NULL, 
 [HPP]        [MONEY] NULL, 
 [Jumlah]     [MONEY] NULL, 
 [Keterangan] [VARCHAR](150) NULL, 
 CONSTRAINT [PK_MMutasiGudangD] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MPemakaian]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MPemakaian]
([NoID]         [BIGINT] NOT NULL, 
 [Kode]         [VARCHAR](50) NULL, 
 [NoReff]       [VARCHAR](50) NULL, 
 [IDGudang]     [INT] NULL, 
 [Tanggal]      [DATETIME] NOT NULL, 
 [Catatan]      [VARCHAR](250) NULL, 
 [Total]        [MONEY] NULL, 
 [IsPosted]     [BIT] NULL, 
 [TglPosted]    [DATETIME] NULL, 
 [IDUserPosted] [INT] NULL, 
 [IDUserEntry]  [INT] NULL, 
 [IDUserEdit]   [INT] NULL, 
 CONSTRAINT [PK_MPemakaian] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MPemakaianD]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MPemakaianD]
([NoID]       [BIGINT] NOT NULL, 
 [IDHeader]   [BIGINT] NULL, 
 [IDBarangD]  [BIGINT] NULL, 
 [IDBarang]   [BIGINT] NULL, 
 [IDSatuan]   [INT] NULL, 
 [Konversi]   [NUMERIC](18, 0) NULL, 
 [Qty]        [NUMERIC](18, 3) NULL, 
 [HPP]        [MONEY] NULL, 
 [Jumlah]     [MONEY] NULL, 
 [Keterangan] [VARCHAR](150) NULL, 
 CONSTRAINT [PK_MPemakaianD] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MPenyesuaian]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MPenyesuaian]
([NoID]         [BIGINT] NOT NULL, 
 [Kode]         [VARCHAR](50) NULL, 
 [NoReff]       [VARCHAR](50) NULL, 
 [IDGudang]     [INT] NULL, 
 [Tanggal]      [DATETIME] NOT NULL, 
 [Catatan]      [VARCHAR](250) NULL, 
 [Total]        [MONEY] NULL, 
 [IsPosted]     [BIT] NULL, 
 [TglPosted]    [DATETIME] NULL, 
 [IDUserPosted] [INT] NULL, 
 [IDUserEntry]  [INT] NULL, 
 [IDUserEdit]   [INT] NULL, 
 CONSTRAINT [PK_MPenyesuaian] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MPenyesuaianD]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MPenyesuaianD]
([NoID]       [BIGINT] NOT NULL, 
 [IDHeader]   [BIGINT] NULL, 
 [IDBarangD]  [BIGINT] NULL, 
 [IDBarang]   [BIGINT] NULL, 
 [IDSatuan]   [INT] NULL, 
 [Konversi]   [NUMERIC](18, 0) NULL, 
 [Qty]        [NUMERIC](18, 3) NULL, 
 [HPP]        [MONEY] NULL, 
 [Jumlah]     [MONEY] NULL, 
 [Keterangan] [VARCHAR](150) NULL, 
 CONSTRAINT [PK_MPenyesuaianD] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MPO]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MPO]
([NoID]           [BIGINT] NOT NULL, 
 [Kode]           [VARCHAR](50) NULL, 
 [NoReff]         [VARCHAR](50) NULL, 
 [IDSupplier]     [INT] NULL, 
 [Tanggal]        [DATETIME] NOT NULL, 
 [Expired]        [DATE] NULL, 
 [Catatan]        [VARCHAR](250) NULL, 
 [IDTypePajak]    [SMALLINT] NULL, 
 [Subtotal]       [MONEY] NULL, 
 [DiscNotaProsen] [NUMERIC](18, 2) NULL, 
 [DiscNotaRp]     [MONEY] NULL, 
 [TotalBruto]     [MONEY] NULL, 
 [DPP]            [MONEY] NULL, 
 [PPN]            [MONEY] NULL, 
 [Total]          [MONEY] NULL, 
 [IsPosted]       [BIT] NULL, 
 [TglPosted]      [DATETIME] NULL, 
 [IDUserPosted]   [INT] NULL, 
 [IDUserEntry]    [INT] NULL, 
 [IDUserEdit]     [INT] NULL, 
 [IsExpired]      [BIT] NULL, 
 CONSTRAINT [PK_MPO] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MPOD]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MPOD]
([NoID]           [BIGINT] NOT NULL, 
 [IDHeader]       [BIGINT] NULL, 
 [IDBarangD]      [BIGINT] NULL, 
 [IDBarang]       [BIGINT] NULL, 
 [IDSatuan]       [INT] NULL, 
 [Konversi]       [NUMERIC](18, 0) NULL, 
 [Qty]            [NUMERIC](18, 3) NULL, 
 [Harga]          [MONEY] NULL, 
 [DiscProsen1]    [NUMERIC](18, 2) NULL, 
 [DiscProsen2]    [NUMERIC](18, 2) NULL, 
 [DiscProsen3]    [NUMERIC](18, 2) NULL, 
 [DiscProsen4]    [NUMERIC](18, 2) NULL, 
 [DiscProsen5]    [NUMERIC](18, 2) NULL, 
 [DiscRp]         [MONEY] NULL, 
 [DiscNotaProsen] [NUMERIC](18, 2) NULL, 
 [DiscNotaRp]     [MONEY] NULL, 
 [JumlahBruto]    [MONEY] NULL, 
 [DPP]            [MONEY] NULL, 
 [PPN]            [MONEY] NULL, 
 [Jumlah]         [MONEY] NULL, 
 CONSTRAINT [PK_MPOD] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MPOS]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MPOS]
([NoID]             [INT] NOT NULL, 
 [Kode]             [VARCHAR](50) NULL, 
 [Nama]             [VARCHAR](50) NULL, 
 [IsActive]         [BIT] NULL, 
 [IDJenisPenjualan] [INT] NULL, 
 [LastConnected]    [DATETIME] NULL, 
 [LastTransaction]  [DATETIME] NULL, 
 CONSTRAINT [PK_MPOS] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MReset]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MReset]
([NoID]         [BIGINT] NOT NULL, 
 [Kode]         [VARCHAR](50) NULL, 
 [IDKasir]      [INT] NULL, 
 [Tanggal]      [DATE] NULL, 
 [Shift]        [SMALLINT] NULL, 
 [IDPOS]        [INT] NULL, 
 [JumlahNota]   [INT] NULL, 
 [Subtotal]     [MONEY] NULL, 
 [Tunai]        [MONEY] NULL, 
 [SetorTunai]   [MONEY] NULL, 
 [IsPosted]     [BIT] NULL, 
 [TglPosted]    [DATETIME] NULL, 
 [IDUserPosted] [INT] NULL, 
 [IDUserEntry]  [INT] NULL, 
 [IDUserEdit]   [INT] NULL, 
 CONSTRAINT [PK_MReset] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MResetD]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MResetD]
([IDReset]           [BIGINT] NOT NULL, 
 [IDJenisPembayaran] [INT] NOT NULL, 
 [JumlahNota]        [INT] NULL, 
 [Subtotal]          [MONEY] NULL, 
 CONSTRAINT [PK_MResetD] PRIMARY KEY CLUSTERED([IDReset] ASC, [IDJenisPembayaran] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MReturBeli]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MReturBeli]
([NoID]           [BIGINT] NOT NULL, 
 [Kode]           [VARCHAR](50) NULL, 
 [NoReff]         [VARCHAR](50) NULL, 
 [IDSupplier]     [INT] NULL, 
 [IDGudang]       [INT] NULL, 
 [Tanggal]        [DATETIME] NOT NULL, 
 [JatuhTempo]     [DATE] NULL, 
 [Catatan]        [VARCHAR](250) NULL, 
 [IDTypePajak]    [SMALLINT] NULL, 
 [Subtotal]       [MONEY] NULL, 
 [DiscNotaProsen] [NUMERIC](18, 2) NULL, 
 [DiscNotaRp]     [MONEY] NULL, 
 [TotalBruto]     [MONEY] NULL, 
 [DPP]            [MONEY] NULL, 
 [PPN]            [MONEY] NULL, 
 [Total]          [MONEY] NULL, 
 [IsPosted]       [BIT] NULL, 
 [TglPosted]      [DATETIME] NULL, 
 [IDUserPosted]   [INT] NULL, 
 [IDUserEntry]    [INT] NULL, 
 [IDUserEdit]     [INT] NULL, 
 CONSTRAINT [PK_MReturBeli] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MReturBeliD]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MReturBeliD]
([NoID]           [BIGINT] NOT NULL, 
 [IDHeader]       [BIGINT] NULL, 
 [IDBeliD]        [BIGINT] NULL, 
 [IDBarangD]      [BIGINT] NULL, 
 [IDBarang]       [BIGINT] NULL, 
 [IDSatuan]       [INT] NULL, 
 [Konversi]       [NUMERIC](18, 0) NULL, 
 [Qty]            [NUMERIC](18, 3) NULL, 
 [Harga]          [MONEY] NULL, 
 [DiscProsen1]    [NUMERIC](18, 2) NULL, 
 [DiscProsen2]    [NUMERIC](18, 2) NULL, 
 [DiscProsen3]    [NUMERIC](18, 2) NULL, 
 [DiscProsen4]    [NUMERIC](18, 2) NULL, 
 [DiscProsen5]    [NUMERIC](18, 2) NULL, 
 [DiscRp]         [MONEY] NULL, 
 [DiscNotaProsen] [NUMERIC](18, 2) NULL, 
 [DiscNotaRp]     [MONEY] NULL, 
 [JumlahBruto]    [MONEY] NULL, 
 [DPP]            [MONEY] NULL, 
 [PPN]            [MONEY] NULL, 
 [Jumlah]         [MONEY] NULL, 
 CONSTRAINT [PK_MReturBeliD] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MReturJual]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MReturJual]
([NoID]           [BIGINT] NOT NULL, 
 [Kode]           [VARCHAR](50) NULL, 
 [NoReff]         [VARCHAR](50) NULL, 
 [IDCustomer]     [INT] NULL, 
 [IDGudang]       [INT] NULL, 
 [Tanggal]        [DATETIME] NOT NULL, 
 [JatuhTempo]     [DATE] NULL, 
 [Catatan]        [VARCHAR](250) NULL, 
 [IDTypePajak]    [SMALLINT] NULL, 
 [Subtotal]       [MONEY] NULL, 
 [DiscNotaProsen] [NUMERIC](18, 2) NULL, 
 [DiscNotaRp]     [MONEY] NULL, 
 [TotalBruto]     [MONEY] NULL, 
 [DPP]            [MONEY] NULL, 
 [PPN]            [MONEY] NULL, 
 [Total]          [MONEY] NULL, 
 [Bayar]          [MONEY] NULL, 
 [Sisa]           [MONEY] NULL, 
 [IsPosted]       [BIT] NULL, 
 [TglPosted]      [DATETIME] NULL, 
 [IDUserPosted]   [INT] NULL, 
 [IDUserEntry]    [INT] NULL, 
 [IDUserEdit]     [INT] NULL, 
 CONSTRAINT [PK_MReturJual] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MReturJualD]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MReturJualD]
([NoID]           [BIGINT] NOT NULL, 
 [IDHeader]       [BIGINT] NULL, 
 [IDBarangD]      [BIGINT] NULL, 
 [IDBarang]       [BIGINT] NULL, 
 [IDSatuan]       [INT] NULL, 
 [Konversi]       [NUMERIC](18, 0) NULL, 
 [Qty]            [NUMERIC](18, 3) NULL, 
 [Harga]          [MONEY] NULL, 
 [DiscProsen1]    [NUMERIC](18, 2) NULL, 
 [DiscProsen2]    [NUMERIC](18, 2) NULL, 
 [DiscProsen3]    [NUMERIC](18, 2) NULL, 
 [DiscProsen4]    [NUMERIC](18, 2) NULL, 
 [DiscProsen5]    [NUMERIC](18, 2) NULL, 
 [DiscRp]         [MONEY] NULL, 
 [DiscNotaProsen] [NUMERIC](18, 2) NULL, 
 [DiscNotaRp]     [MONEY] NULL, 
 [JumlahBruto]    [MONEY] NULL, 
 [DPP]            [MONEY] NULL, 
 [PPN]            [MONEY] NULL, 
 [Jumlah]         [MONEY] NULL, 
 CONSTRAINT [PK_MReturJualD] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MReturJualDBayar]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MReturJualDBayar]
([NoID]              [BIGINT] IDENTITY(1, 1) NOT NULL, 
 [IDHeader]          [BIGINT] NOT NULL, 
 [IDJenisPembayaran] [INT] NOT NULL, 
 [AtasNama]          [VARCHAR](50) NULL, 
 [NoRekening]        [VARCHAR](150) NULL, 
 [Nominal]           [MONEY] NOT NULL, 
 [ChargeProsen]      [NUMERIC](18, 3) NULL, 
 [ChargeRp]          [MONEY] NULL, 
 [Total]             [MONEY] NULL, 
 CONSTRAINT [PK_MReturJualDBayar] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MRole]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MRole]
([NoID]         [INT] NOT NULL, 
 [Role]         [VARCHAR](50) NULL, 
 [IsSupervisor] [BIT] NULL, 
 CONSTRAINT [PK_MRole] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MRoleD]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MRoleD]
([NoID]     [INT] IDENTITY(1, 1) NOT NULL, 
 [IDRole]   [INT] NULL, 
 [IDMenu]   [INT] NULL, 
 [IsActive] [BIT] NULL, 
 CONSTRAINT [PK_MRoleD] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MSaldoAwalHutang]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MSaldoAwalHutang]
([NoID]         [INT] NOT NULL, 
 [IDSupplier]   [INT] NULL, 
 [Kode]         [VARCHAR](50) NULL, 
 [Tanggal]      [DATE] NULL, 
 [JatuhTempo]   [DATE] NULL, 
 [KodeReff]     [VARCHAR](50) NULL, 
 [Jumlah]       [MONEY] NULL, 
 [IDUserEntry]  [INT] NULL, 
 [TglEntry]     [DATETIME] NULL, 
 [IDUserEdit]   [INT] NULL, 
 [TglEdit]      [DATETIME] NULL, 
 [IsPosted]     [BIT] NULL, 
 [IDUserPosted] [INT] NULL, 
 [TglPosted]    [DATETIME] NULL, 
 [Keterangan]   [VARCHAR](MAX) NULL, 
 CONSTRAINT [PK_MSaldoAwalHutang] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MSaldoAwalPersediaan]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MSaldoAwalPersediaan]
([NoID]         [INT] NOT NULL, 
 [IDGudang]     [INT] NOT NULL, 
 [IDBarangD]    [BIGINT] NOT NULL, 
 [IDBarang]     [BIGINT] NOT NULL, 
 [Kode]         [VARCHAR](15) NOT NULL, 
 [Tanggal]      [DATETIME] NOT NULL, 
 [Qty]          [NUMERIC](18, 3) NOT NULL, 
 [IDSatuan]     [INT] NOT NULL, 
 [Konversi]     [NUMERIC](18, 0) NOT NULL, 
 [HargaPokok]   [MONEY] NOT NULL, 
 [Jumlah]       [MONEY] NOT NULL, 
 [IDUserEntry]  [INT] NOT NULL, 
 [TglEntry]     [DATETIME] NOT NULL, 
 [IDUserEdit]   [INT] NULL, 
 [TglEdit]      [DATETIME] NULL, 
 [IsPosted]     [BIT] NULL, 
 [IDUserPosted] [INT] NULL, 
 [TglPosted]    [DATETIME] NULL, 
 [Keterangan]   [VARCHAR](250) NULL, 
 CONSTRAINT [PK_MSaldoAwalPersediaan] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], 
 CONSTRAINT [IX_Kode] UNIQUE NONCLUSTERED([Kode] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MSaldoAwalPiutang]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MSaldoAwalPiutang]
([NoID]         [INT] NOT NULL, 
 [IDCustomer]   [INT] NULL, 
 [Kode]         [VARCHAR](50) NULL, 
 [Tanggal]      [DATE] NULL, 
 [JatuhTempo]   [DATE] NULL, 
 [KodeReff]     [VARCHAR](50) NULL, 
 [Jumlah]       [MONEY] NULL, 
 [IDUserEntry]  [INT] NULL, 
 [TglEntry]     [DATETIME] NULL, 
 [IDUserEdit]   [INT] NULL, 
 [TglEdit]      [DATETIME] NULL, 
 [IsPosted]     [BIT] NULL, 
 [IDUserPosted] [INT] NULL, 
 [TglPosted]    [DATETIME] NULL, 
 [Keterangan]   [VARCHAR](MAX) NULL, 
 CONSTRAINT [PK_MSaldoAwalPiutang] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MSatuan]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MSatuan]
([NoID]     [INT] NOT NULL, 
 [Kode]     [VARCHAR](50) NULL, 
 [Nama]     [VARCHAR](50) NULL, 
 [Konversi] [INT] NULL, 
 [IsActive] [BIT] NULL, 
 CONSTRAINT [PK_MSatuan] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MSetting]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MSetting]
([KeySettings] [VARCHAR](255) NOT NULL, 
 [KeyValue]    [VARCHAR](MAX) NULL, 
 PRIMARY KEY CLUSTERED([KeySettings] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MStockOpname]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MStockOpname]
([NoID]         [BIGINT] NOT NULL, 
 [Kode]         [VARCHAR](50) NULL, 
 [NoReff]       [VARCHAR](50) NULL, 
 [IDGudang]     [INT] NULL, 
 [Tanggal]      [DATETIME] NOT NULL, 
 [Catatan]      [VARCHAR](250) NULL, 
 [Total]        [MONEY] NULL, 
 [IsPosted]     [BIT] NULL, 
 [TglPosted]    [DATETIME] NULL, 
 [IDUserPosted] [INT] NULL, 
 [IDUserEntry]  [INT] NULL, 
 [IDUserEdit]   [INT] NULL, 
 CONSTRAINT [PK_MStockOpname] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MStockOpnameD]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MStockOpnameD]
([NoID]        [BIGINT] NOT NULL, 
 [IDHeader]    [BIGINT] NULL, 
 [IDBarangD]   [BIGINT] NULL, 
 [IDBarang]    [BIGINT] NULL, 
 [IDSatuan]    [INT] NULL, 
 [Konversi]    [NUMERIC](18, 0) NULL, 
 [QtyKomputer] [NUMERIC](18, 3) NULL, 
 [QtyFisik]    [NUMERIC](18, 3) NULL, 
 [Qty]         [NUMERIC](18, 3) NULL, 
 [HPP]         [MONEY] NULL, 
 [Jumlah]      [MONEY] NULL, 
 [Keterangan]  [VARCHAR](150) NULL, 
 CONSTRAINT [PK_MStockOpnameD] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MTypeHarga]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MTypeHarga]
([NoID]      [INT] NOT NULL, 
 [Caption]   [VARCHAR](50) NULL, 
 [FieldName] [VARCHAR](50) NULL, 
 CONSTRAINT [PK_MTypeHarga] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MTypePajak]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MTypePajak]
([NoID]      [INT] NOT NULL, 
 [TypePajak] [VARCHAR](50) NULL, 
 CONSTRAINT [PK_MTypePajak] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MUser]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MUser]
([NoID]     [INT] NOT NULL, 
 [Kode]     [VARCHAR](50) NULL, 
 [Nama]     [VARCHAR](50) NULL, 
 [Pwd]      [VARCHAR](50) NULL, 
 [IDAlamat] [INT] NULL, 
 [IDRole]   [INT] NULL, 
 CONSTRAINT [PK_MUser] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[MWOAssembly]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[MWOAssembly]
([NoID]         [UNIQUEIDENTIFIER] NOT NULL, 
 [Kode]         [VARCHAR](50) NOT NULL, 
 [Tanggal]      [DATETIME] NOT NULL, 
 [TanggalWO]    [DATETIME] NOT NULL, 
 [IDMaterial]   [UNIQUEIDENTIFIER] NOT NULL, 
 [IDPegawai]    [BIGINT] NOT NULL, 
 [IDGudang]     [INT] NOT NULL, 
 [Catatan]      [VARCHAR](150) NOT NULL, 
 [IDBarangD]    [BIGINT] NOT NULL, 
 [IDBarang]     [BIGINT] NOT NULL, 
 [IDSatuan]     [INT] NOT NULL, 
 [Konversi]     [NUMERIC](18, 0) NOT NULL, 
 [Qty]          [NUMERIC](18, 3) NOT NULL, 
 [HargaPokok]   [MONEY] NOT NULL, 
 [Jumlah]       [MONEY] NOT NULL, 
 [IsPosted]     [BIT] NULL, 
 [TglPosted]    [DATETIME] NULL, 
 [IDUserPosted] [INT] NULL, 
 [IDUserEntry]  [INT] NULL, 
 [IDUserEdit]   [INT] NULL, 
 CONSTRAINT [PK_MWOAssembly] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[TAppDB]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[TAppDB]
([NoID]      [INT] IDENTITY(1, 1) NOT NULL, 
 [Tanggal]   [DATETIME] NULL, 
 [DBVersion] [VARCHAR](100) NULL, 
 PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[TLogPerubahanHargaBeli]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[TLogPerubahanHargaBeli]
([NoID]             [BIGINT] IDENTITY(1, 1) NOT NULL, 
 [IDBarang]         [BIGINT] NULL, 
 [Keterangan]       [VARCHAR](255) NULL, 
 [Tanggal]          [DATETIME] NULL, 
 [HargaBeliPcsLama] [MONEY] NULL, 
 [HargaBeliPcsBaru] [MONEY] NULL, 
 [IDUser]           [INT] NULL, 
 [TanggalUpdate]    [DATETIME] NULL, 
 CONSTRAINT [PK_TLogPerubahanHargaBeli] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[TLogPerubahanHargaJual]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[TLogPerubahanHargaJual]
([NoID]             [BIGINT] IDENTITY(1, 1) NOT NULL, 
 [IDBarangD]        [BIGINT] NULL, 
 [Keterangan]       [VARCHAR](255) NULL, 
 [Tanggal]          [DATETIME] NULL, 
 [HargaBeliPcsLama] [MONEY] NULL, 
 [HargaBeliPcsBaru] [MONEY] NULL, 
 [ProsenUpALama]    [NUMERIC](18, 2) NULL, 
 [ProsenUpABaru]    [NUMERIC](18, 2) NULL, 
 [HargaJualALama]   [MONEY] NULL, 
 [HargaJualABaru]   [MONEY] NULL, 
 [ProsenUpBLama]    [NUMERIC](18, 2) NULL, 
 [ProsenUpBBaru]    [NUMERIC](18, 2) NULL, 
 [HargaJualBLama]   [MONEY] NULL, 
 [HargaJualBBaru]   [MONEY] NULL, 
 [IDUser]           [INT] NULL, 
 [TanggalUpdate]    [DATETIME] NULL, 
 CONSTRAINT [PK_TLogPerubahanHargaJual] PRIMARY KEY CLUSTERED([NoID] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  Table [dbo].[TSaldoStok]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[TSaldoStok]
([IDBarang]   [BIGINT] NOT NULL, 
 [IDGudang]   [INT] NOT NULL, 
 [SaldoAkhir] [NUMERIC](18, 3) NULL, 
 CONSTRAINT [PK_TSaldoStok] PRIMARY KEY CLUSTERED([IDBarang] ASC, [IDGudang] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO

/****** Object:  UserDefinedFunction [dbo].[fnSplitString]    Script Date: 6/23/2021 11:20:17 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE FUNCTION [dbo].[fnSplitString]
(@List  NVARCHAR(MAX), 
 @Delim VARCHAR(255)
)
RETURNS TABLE
AS
     RETURN
(
    SELECT [Value]
    FROM
    (
        SELECT [Value] = LTRIM(RTRIM(SUBSTRING(@List, [Number], CHARINDEX(@Delim, @List + @Delim, [Number]) - [Number])))
        FROM
        (
            SELECT Number = ROW_NUMBER() OVER(
                   ORDER BY name)
            FROM sys.all_objects
        ) AS x
        WHERE Number <= LEN(@List)
              AND SUBSTRING(@Delim + @List, [Number], LEN(@Delim)) = @Delim
    ) AS y
);
GO
SET ANSI_PADDING ON;
GO

/****** Object:  Index [IX_Kode]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE UNIQUE NONCLUSTERED INDEX [IX_Kode] ON [dbo].[MAssembly]
([Kode] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO
SET ANSI_PADDING ON;
GO

/****** Object:  Index [IX_Barcode]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE UNIQUE NONCLUSTERED INDEX [IX_Barcode] ON [dbo].[MBarang]
([DefaultBarcode] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO
SET ANSI_PADDING ON;
GO

/****** Object:  Index [IX_Kode]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE UNIQUE NONCLUSTERED INDEX [IX_Kode] ON [dbo].[MBarang]
([Kode] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO
SET ANSI_PADDING ON;
GO

/****** Object:  Index [IX_Barcode]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE UNIQUE NONCLUSTERED INDEX [IX_Barcode] ON [dbo].[MBarangD]
([Barcode] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO
SET ANSI_PADDING ON;
GO

/****** Object:  Index [IX_MGudang]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE UNIQUE NONCLUSTERED INDEX [IX_MGudang] ON [dbo].[MGudang]
([Kode] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO
SET ANSI_PADDING ON;
GO

/****** Object:  Index [IX_IDBarang]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE NONCLUSTERED INDEX [IX_IDBarang] ON [dbo].[MKartuStok]
([IDBarang] ASC, [IDBarangD] ASC, [Varian] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO

/****** Object:  Index [IX_IDGudang]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE NONCLUSTERED INDEX [IX_IDGudang] ON [dbo].[MKartuStok]
([IDGudang] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO

/****** Object:  Index [IX_IDTransaksi]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE NONCLUSTERED INDEX [IX_IDTransaksi] ON [dbo].[MKartuStok]
([IDJenisTransaksi] ASC, [IDTransaksi] ASC, [IDTransaksiD] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO
SET ANSI_PADDING ON;
GO

/****** Object:  Index [IX_Barang]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE NONCLUSTERED INDEX [IX_Barang] ON [dbo].[MKartuStokOnHand]
([IDBarang] ASC, [IDBarangD] ASC, [Varian] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO

/****** Object:  Index [IX_Transaksi]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE NONCLUSTERED INDEX [IX_Transaksi] ON [dbo].[MKartuStokOnHand]
([IDJenisTransaksi] ASC, [IDTransaksi] ASC, [IDTransaksiD] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO
SET ANSI_PADDING ON;
GO

/****** Object:  Index [IX_Kode]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE UNIQUE NONCLUSTERED INDEX [IX_Kode] ON [dbo].[MKasIN]
([Kode] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO
SET ANSI_PADDING ON;
GO

/****** Object:  Index [IX_Kode]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE UNIQUE NONCLUSTERED INDEX [IX_Kode] ON [dbo].[MKasOut]
([Kode] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO
SET ANSI_PADDING ON;
GO

/****** Object:  Index [IX_MKategori]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE UNIQUE NONCLUSTERED INDEX [IX_MKategori] ON [dbo].[MKategori]
([Kode] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO
SET ANSI_PADDING ON;
GO

/****** Object:  Index [IX_Kode]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE UNIQUE NONCLUSTERED INDEX [IX_Kode] ON [dbo].[MMaterial]
([Kode] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO

/****** Object:  Index [IX_IDMaterial]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE UNIQUE NONCLUSTERED INDEX [IX_IDMaterial] ON [dbo].[MMaterialD]
([IDBarangD] ASC, [IDMaterial] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO

/****** Object:  Index [IX_IDMaterial]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE UNIQUE NONCLUSTERED INDEX [IX_IDMaterial] ON [dbo].[MMaterialDSisa]
([IDMaterial] ASC, [IDBarangD] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO
SET ANSI_PADDING ON;
GO

/****** Object:  Index [IX_MMenu]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE UNIQUE NONCLUSTERED INDEX [IX_MMenu] ON [dbo].[MMenu]
([Name] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO
SET ANSI_PADDING ON;
GO

/****** Object:  Index [IX_MMerk]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE UNIQUE NONCLUSTERED INDEX [IX_MMerk] ON [dbo].[MMerk]
([Kode] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO
SET ANSI_PADDING ON;
GO

/****** Object:  Index [IX_MPO]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE UNIQUE NONCLUSTERED INDEX [IX_MPO] ON [dbo].[MPO]
([Kode] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO

/****** Object:  Index [IX_MPOD]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE NONCLUSTERED INDEX [IX_MPOD] ON [dbo].[MPOD]
([IDHeader] ASC, [IDBarang] ASC, [IDBarangD] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO
SET ANSI_PADDING ON;
GO

/****** Object:  Index [IX_MRole]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE UNIQUE NONCLUSTERED INDEX [IX_MRole] ON [dbo].[MRole]
([Role] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO

/****** Object:  Index [IX_MRoleD]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE UNIQUE NONCLUSTERED INDEX [IX_MRoleD] ON [dbo].[MRoleD]
([IDMenu] ASC, [IDRole] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO
SET ANSI_PADDING ON;
GO

/****** Object:  Index [IX_MSatuan]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE UNIQUE NONCLUSTERED INDEX [IX_MSatuan] ON [dbo].[MSatuan]
([Kode] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO
SET ANSI_PADDING ON;
GO

/****** Object:  Index [IX_MUser]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE UNIQUE NONCLUSTERED INDEX [IX_MUser] ON [dbo].[MUser]
([Kode] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO
SET ANSI_PADDING ON;
GO

/****** Object:  Index [IX_Kode]    Script Date: 6/23/2021 11:20:17 AM ******/

CREATE UNIQUE NONCLUSTERED INDEX [IX_Kode] ON [dbo].[MWOAssembly]
([Kode] ASC
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO
ALTER TABLE [dbo].[IMP_BRG]
ADD CONSTRAINT [DF_IMP_BRG_NETTO] DEFAULT((0)) FOR [NETTO];
GO
ALTER TABLE [dbo].[IMP_BRG]
ADD CONSTRAINT [DF_IMP_BRG_HARGA JUAL] DEFAULT((0)) FOR [HARGA JUAL];
GO
ALTER TABLE [dbo].[IMP_BRG]
ADD CONSTRAINT [DF_IMP_BRG_STOK AWAL] DEFAULT((0)) FOR [STOK AWAL];
GO
ALTER TABLE [dbo].[IMP_BRG]
ADD CONSTRAINT [DF_IMP_BRG_SAAT INI] DEFAULT((0)) FOR [SAAT INI];
GO
ALTER TABLE [dbo].[IMP_BRG]
ADD CONSTRAINT [DF_IMP_BRG_MARGIN] DEFAULT((0)) FOR [MARGIN];
GO
ALTER TABLE [dbo].[IMP_BRG]
ADD CONSTRAINT [DF_IMP_BRG_NILAI STOCK] DEFAULT((0)) FOR [NILAI STOCK];
GO
ALTER TABLE [dbo].[MBarangD]
ADD CONSTRAINT [DF_MBarangD_IsDefault] DEFAULT((1)) FOR [IsDefault];
GO
ALTER TABLE [dbo].[MBarangD]
ADD CONSTRAINT [DF_MBarangD_IsActive] DEFAULT((1)) FOR [IsActive];
GO
ALTER TABLE [dbo].[MJualDBayar]
ADD CONSTRAINT [DF_MJualDBayar_Nominal] DEFAULT((0)) FOR [Nominal];
GO
ALTER TABLE [dbo].[MJualDBayar]
ADD CONSTRAINT [DF_MJualDBayar_ChargeProsen] DEFAULT((0)) FOR [ChargeProsen];
GO
ALTER TABLE [dbo].[MJualDBayar]
ADD CONSTRAINT [DF_MJualDBayar_ChargeRp] DEFAULT((0)) FOR [ChargeRp];
GO
ALTER TABLE [dbo].[MJualDBayar]
ADD CONSTRAINT [DF_MJualDBayar_Total] DEFAULT((0)) FOR [Total];
GO
ALTER TABLE [dbo].[MKartuStok]
ADD CONSTRAINT [DF_MKartuStok_IDGudang] DEFAULT((1)) FOR [IDGudang];
GO
ALTER TABLE [dbo].[MKasINDBayar]
ADD CONSTRAINT [DF_MKasINDBayar_Nominal] DEFAULT((0)) FOR [Nominal];
GO
ALTER TABLE [dbo].[MKasINDBayar]
ADD CONSTRAINT [DF_MKasINDBayar_ChargeProsen] DEFAULT((0)) FOR [ChargeProsen];
GO
ALTER TABLE [dbo].[MKasINDBayar]
ADD CONSTRAINT [DF_MKasINDBayar_ChargeRp] DEFAULT((0)) FOR [ChargeRp];
GO
ALTER TABLE [dbo].[MKasINDBayar]
ADD CONSTRAINT [DF_MKasINDBayar_Total] DEFAULT((0)) FOR [Total];
GO
ALTER TABLE [dbo].[MKasOutDBayar]
ADD CONSTRAINT [DF_MKasOutDBayar_Nominal] DEFAULT((0)) FOR [Nominal];
GO
ALTER TABLE [dbo].[MKasOutDBayar]
ADD CONSTRAINT [DF_MKasOutDBayar_ChargeProsen] DEFAULT((0)) FOR [ChargeProsen];
GO
ALTER TABLE [dbo].[MKasOutDBayar]
ADD CONSTRAINT [DF_MKasOutDBayar_ChargeRp] DEFAULT((0)) FOR [ChargeRp];
GO
ALTER TABLE [dbo].[MKasOutDBayar]
ADD CONSTRAINT [DF_MKasOutDBayar_Total] DEFAULT((0)) FOR [Total];
GO
ALTER TABLE [dbo].[MMaterial]
ADD CONSTRAINT [DF_MMaterial_IsActive] DEFAULT((0)) FOR [IsActive];
GO
ALTER TABLE [dbo].[MMenu]
ADD CONSTRAINT [DF_MMenu_NoUrut] DEFAULT((0)) FOR [NoUrut];
GO
ALTER TABLE [dbo].[MMenu]
ADD CONSTRAINT [DF_MMenu_IsBig] DEFAULT((0)) FOR [IsBig];
GO
ALTER TABLE [dbo].[MMenu]
ADD CONSTRAINT [DF_MMenu_IsBeginGroup] DEFAULT((0)) FOR [IsBeginGroup];
GO
ALTER TABLE [dbo].[MMenu]
ADD CONSTRAINT [DF_MMenu_IsActive] DEFAULT((0)) FOR [IsActive];
GO
ALTER TABLE [dbo].[MReturJualDBayar]
ADD DEFAULT((0)) FOR [Nominal];
GO
ALTER TABLE [dbo].[MReturJualDBayar]
ADD DEFAULT((0)) FOR [ChargeProsen];
GO
ALTER TABLE [dbo].[MReturJualDBayar]
ADD DEFAULT((0)) FOR [ChargeRp];
GO
ALTER TABLE [dbo].[MReturJualDBayar]
ADD DEFAULT((0)) FOR [Total];
GO
ALTER TABLE [dbo].[MAssembly]
WITH CHECK
ADD CONSTRAINT [FK_MAssembly_MMaterial] FOREIGN KEY([IDMaterial]) REFERENCES [dbo].[MMaterial]([NoID]);
GO
ALTER TABLE [dbo].[MAssembly] CHECK CONSTRAINT [FK_MAssembly_MMaterial];
GO
ALTER TABLE [dbo].[MAssembly]
WITH CHECK
ADD CONSTRAINT [FK_MAssembly_MWOAssembly] FOREIGN KEY([IDWOAssembly]) REFERENCES [dbo].[MWOAssembly]([NoID]);
GO
ALTER TABLE [dbo].[MAssembly] CHECK CONSTRAINT [FK_MAssembly_MWOAssembly];
GO
ALTER TABLE [dbo].[MAssemblyD]
WITH CHECK
ADD CONSTRAINT [FK_MAssemblyD_MAssembly] FOREIGN KEY([IDHeader]) REFERENCES [dbo].[MAssembly]([NoID]) ON UPDATE CASCADE;
GO
ALTER TABLE [dbo].[MAssemblyD] CHECK CONSTRAINT [FK_MAssemblyD_MAssembly];
GO
ALTER TABLE [dbo].[MAssemblyD]
WITH CHECK
ADD CONSTRAINT [FK_MAssemblyD_MMaterialD] FOREIGN KEY([IDMaterialD]) REFERENCES [dbo].[MMaterialD]([NoID]) ON UPDATE CASCADE;
GO
ALTER TABLE [dbo].[MAssemblyD] CHECK CONSTRAINT [FK_MAssemblyD_MMaterialD];
GO
ALTER TABLE [dbo].[MAssemblyDBiaya]
WITH CHECK
ADD CONSTRAINT [FK_MAssemblyDBiaya_MAssembly] FOREIGN KEY([IDHeader]) REFERENCES [dbo].[MAssembly]([NoID]) ON UPDATE CASCADE;
GO
ALTER TABLE [dbo].[MAssemblyDBiaya] CHECK CONSTRAINT [FK_MAssemblyDBiaya_MAssembly];
GO
ALTER TABLE [dbo].[MAssemblyDBiaya]
WITH CHECK
ADD CONSTRAINT [FK_MAssemblyDBiaya_MMaterialDBiaya] FOREIGN KEY([IDMaterialDBiaya]) REFERENCES [dbo].[MMaterialDBiaya]([NoID]) ON UPDATE CASCADE;
GO
ALTER TABLE [dbo].[MAssemblyDBiaya] CHECK CONSTRAINT [FK_MAssemblyDBiaya_MMaterialDBiaya];
GO
ALTER TABLE [dbo].[MAssemblyDSisa]
WITH CHECK
ADD CONSTRAINT [FK_MAssemblyDSisa_MAssemblyDSisa] FOREIGN KEY([IDHeader]) REFERENCES [dbo].[MAssembly]([NoID]) ON UPDATE CASCADE;
GO
ALTER TABLE [dbo].[MAssemblyDSisa] CHECK CONSTRAINT [FK_MAssemblyDSisa_MAssemblyDSisa];
GO
ALTER TABLE [dbo].[MAssemblyDSisa]
WITH CHECK
ADD CONSTRAINT [FK_MAssemblyDSisa_MMaterialDSisa] FOREIGN KEY([IDMaterialDSisa]) REFERENCES [dbo].[MMaterialDSisa]([NoID]) ON UPDATE CASCADE;
GO
ALTER TABLE [dbo].[MAssemblyDSisa] CHECK CONSTRAINT [FK_MAssemblyDSisa_MMaterialDSisa];
GO
ALTER TABLE [dbo].[MMaterialD]
WITH CHECK
ADD CONSTRAINT [FK_MMaterialD_MMaterial] FOREIGN KEY([IDMaterial]) REFERENCES [dbo].[MMaterial]([NoID]) ON UPDATE CASCADE ON DELETE CASCADE;
GO
ALTER TABLE [dbo].[MMaterialD] CHECK CONSTRAINT [FK_MMaterialD_MMaterial];
GO
ALTER TABLE [dbo].[MMaterialDBiaya]
WITH CHECK
ADD CONSTRAINT [FK_MMaterialDBiaya_MMaterial] FOREIGN KEY([IDMaterial]) REFERENCES [dbo].[MMaterial]([NoID]) ON UPDATE CASCADE ON DELETE CASCADE;
GO
ALTER TABLE [dbo].[MMaterialDBiaya] CHECK CONSTRAINT [FK_MMaterialDBiaya_MMaterial];
GO
ALTER TABLE [dbo].[MMaterialDSisa]
WITH CHECK
ADD CONSTRAINT [FK_MMaterialDSisa_MMaterial] FOREIGN KEY([IDMaterial]) REFERENCES [dbo].[MMaterial]([NoID]) ON UPDATE CASCADE ON DELETE CASCADE;
GO
ALTER TABLE [dbo].[MMaterialDSisa] CHECK CONSTRAINT [FK_MMaterialDSisa_MMaterial];
GO
ALTER TABLE [dbo].[MWOAssembly]
WITH CHECK
ADD CONSTRAINT [FK_MWOAssembly_MMaterial] FOREIGN KEY([IDMaterial]) REFERENCES [dbo].[MMaterial]([NoID]) ON UPDATE CASCADE;
GO
ALTER TABLE [dbo].[MWOAssembly] CHECK CONSTRAINT [FK_MWOAssembly_MMaterial];
GO

/****** Object:  StoredProcedure [dbo].[procRepairHPPPerBarang]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[procRepairHPPPerBarang]
(@IDBarang AS      INT  = 0, 
 @TanggalDari AS   DATE = '2018-01-01', 
 @TanggalSampai AS DATE = '2019-01-01', 
 @IDUser AS        INT  = 9
)
AS
    BEGIN
        SET NOCOUNT ON;
        SET DEADLOCK_PRIORITY NORMAL;
        DECLARE @rCount INT= 0;
        DECLARE @DefdigitDecimal AS INT= 2;
        DECLARE @ispkp AS BIT= 0;
        DECLARE @Temp AS TABLE
        (NoID             INT IDENTITY(1, 1) PRIMARY KEY, 
         IDKartuStok      BIGINT, 
         IDJenisTransaksi INT, 
         Tanggal          DATETIME, 
         NilaiAwal        NUMERIC(18, 2), 
         Debet            NUMERIC(18, 2), 
         Kredit           NUMERIC(18, 2), 
         NilaiAkhir       NUMERIC(18, 2), 
         SaldoAwal        NUMERIC(18, 3), 
         QtyMasuk         NUMERIC(18, 3), 
         QtyKeluar        NUMERIC(18, 3), 
         SaldoAkhir       NUMERIC(18, 3), 
         HargaBeli        NUMERIC(18, 2), 
         HPP              NUMERIC(18, 2), 
         JumlahBeli       NUMERIC(18, 2)
        );
        INSERT INTO @Temp
        (IDKartuStok, 
         Tanggal, 
         IDJenisTransaksi, 
         NilaiAwal, 
         Debet, 
         Kredit, 
         NilaiAkhir, 
         SaldoAwal, 
         QtyMasuk, 
         QtyKeluar, 
         SaldoAkhir, 
         HPP, 
         HargaBeli
        )
               SELECT MKartuStok.NoID, 
                      MKartuStok.Tanggal, 
                      MKartuStok.IDJenisTransaksi, 
                      0, 
                      ISNULL(MKartuStok.Debet, 0), 
                      ISNULL(MKartuStok.Kredit, 0), 
                      0, 
                      0, 
                      ISNULL(MKartuStok.QtyMasuk * MKartuStok.Konversi, 0), 
                      ISNULL(MKartuStok.QtyKeluar * MKartuStok.Konversi, 0), 
                      0, 
                      ISNULL(MKartuStok.HPP, 0), 
                      ISNULL(MKartuStok.HargaBeli, 0)
               FROM MKartuStok(NOLOCK)
                    LEFT JOIN MJenisTransaksi ON MKartuStok.IDJenisTransaksi = MJenisTransaksi.ID
               WHERE MKartuStok.IDBarang = @IDBarang
                     AND MKartuStok.Tanggal >= CONVERT(DATE, @TanggalDari)
                     AND MKartuStok.Tanggal < DATEADD(DAY, 1, CONVERT(DATE, @Tanggalsampai))
               ORDER BY MKartuStok.Tanggal, 
                        MJenisTransaksi.NoUrut, 
                        MKartuStok.HargaBeli DESC, 
                        MKartuStok.QtyMasuk DESC, 
                        MKartuStok.QtyKeluar, 
                        MKartuStok.NoID;
        DECLARE @HPP AS NUMERIC(18, 2)= 0, @HargaBeli AS NUMERIC(18, 2)= 0, @NilaiAkhir AS NUMERIC(18, 2)= 0, @SaldoAkhir AS NUMERIC(18, 3)= 0, @SaldoAwal AS NUMERIC(18, 3), @NilaiAwal AS NUMERIC(18, 3);
        DECLARE @IDJenisTransaksi AS INT= 0, @QtyIn NUMERIC(18, 2)= 0, @QtyOut NUMERIC(18, 2)= 0, @Tanggal DATETIME;
        DECLARE @i AS INT, @iMax AS INT;
        SELECT @i = 1, 
               @iMax = MAX(NoID)
        FROM @Temp T;
        WHILE(@i <= @iMax)
            BEGIN
                IF(@i <= 1)
                    BEGIN
                        --HargaBeli Ambil Dari
                        SELECT TOP 1 @HargaBeli = ISNULL(HargaBeli, 0)
                        FROM MKartuStok(NOLOCK)
                             LEFT JOIN MJenisTransaksi(NOLOCK) ON MKartuStok.IDJenisTransaksi = MJenisTransaksi.ID
                        WHERE CONVERT(DATE, Tanggal) < CONVERT(DATE, @TanggalDari)
                              AND IDBarang = @IDBarang
                              AND (IDJenisTransaksi = 2
                                   OR IDJenisTransaksi = 1
                                   OR IDJenisTransaksi = 14
                                   OR (IDJenisTransaksi = 19
                                       AND QtyMasuk > 0)
                                   OR (IDJenisTransaksi = 20
                                       AND QtyMasuk - QtyKeluar > 0))
                        ORDER BY MKartuStok.Tanggal DESC, 
                                 mjenistransaksi.nourut DESC, 
                                 MKartuStok.NoID;
                        IF(@HargaBeli = 0)
                            BEGIN
                                --Kalau Harga Beli Masih 0 Ambil Pembelian Setelahnya.
                                SELECT TOP 1 @HargaBeli = ISNULL(HargaBeli, 0)
                                FROM MKartuStok(NOLOCK)
                                     LEFT JOIN MJenisTransaksi ON MKartuStok.IDJenisTransaksi = MJenisTransaksi.ID
                                WHERE CONVERT(DATE, Tanggal) > CONVERT(DATE, @TanggalDari)
                                      AND IDBarang = @IDBarang
                                      AND (IDJenisTransaksi = 2)
                                ORDER BY MKartuStok.Tanggal ASC;
                                IF(@HargaBeli = 0)
                                    BEGIN
                                        SELECT @HargaBeli = ROUND(HargaBeliPcs, @DefdigitDecimal)
                                        FROM MBarang(NOLOCK)
                                        WHERE MBarang.NoID = @IDBarang;
                                    END;
                            END;
                        SELECT @SaldoAwal = SUM((MKartuStok.QtyMasuk - MKartuStok.QtyKeluar) * MKartuStok.Konversi), 
                               @NilaiAwal = SUM(ISNULL(MKartuStok.Debet, 0) - ISNULL(MKartuStok.Kredit, 0))
                        FROM MKartuStok(NOLOCK)
                             INNER JOIN MBarang(NOLOCK) ON MBarang.NoID = MKartuStok.IDBarang
                        WHERE CONVERT(DATE, MKartuStok.Tanggal) < CONVERT(DATE, @TanggalDari)
                              AND MBarang.NoID = @IDBarang;
                        SELECT TOP 1 @HPP = ISNULL(HPP, 0)
                        FROM MKartuStok(NOLOCK)
                             LEFT JOIN MJenisTransaksi ON MKartuStok.IDJenisTransaksi = MJenisTransaksi.ID
                        WHERE CONVERT(DATE, Tanggal) < CONVERT(DATE, @TanggalDari)
                              AND IDBarang = @IDBarang
                        ORDER BY MKartuStok.Tanggal DESC, 
                                 mjenistransaksi.nourut DESC, 
                                 MKartuStok.NoID;
                    END;
                --PRINT CONVERT(VARCHAR, @i) + ', Cek Data Saldo Awal ' + CONVERT(VARCHAR, ISNULL(@SaldoAwal, 0));
                UPDATE T
                  SET 
                      SaldoAwal = ISNULL(@SaldoAwal, 0), 
                      NilaiAwal = ISNULL(@NilaiAwal, 0), 
                      SaldoAkhir = ISNULL(@SaldoAwal, 0) + T.QtyMasuk - T.QtyKeluar, 
                      NilaiAkhir = ISNULL(@NilaiAkhir, 0), 
                      HPP = @HPP
                FROM @Temp T
                WHERE T.NoID = @i;
                SELECT @SaldoAwal = T.SaldoAkhir, 
                       @SaldoAkhir = ISNULL(@SaldoAkhir, 0), 
                       @NilaiAwal = T.NilaiAkhir, 
                       @NilaiAkhir = ISNULL(T.NilaiAkhir, 0), 
                       @IDJenisTransaksi = T.IDJenisTransaksi, 
                       @QtyIn = ISNULL(QtyMasuk, 0), 
                       @QtyOut = ISNULL(QtyKeluar, 0), 
                       @Tanggal = T.Tanggal
                FROM @Temp T
                WHERE T.NoID = @i;
                SET @NilaiAkhir = ISNULL(@NilaiAkhir, 0);
                SET @SaldoAkhir = ISNULL(@SaldoAkhir, 0);
                SET @HargaBeli = ISNULL(@HargaBeli, 0);
                SET @NilaiAwal = ISNULL(@NilaiAwal, 0);
                SET @SaldoAwal = ISNULL(@SaldoAwal, 0);

                --Yang In diambilkan dari Transaksi
                IF(@IDJenisTransaksi = 1)
                    BEGIN
                        --               --HargaDef Harga Per Satuan.
                        --               --UPDATE T
                        --               --SET SaldoAwal = @SaldoAkhir + T.QtyKeluar - T.QtyMasuk
                        --               --	,SaldoAkhir = @SaldoAkhir
                        --               --	,Debet = ROUND(ISNULL(MSaldoAwalPersediaanD.Jumlah, 0), @DefdigitDecimal)
                        --               --	,Kredit = 0
                        --               --	,HargaBeli = ROUND(MSaldoAwalPersediaanD.HargaPokok, @DefdigitDecimal)
                        --               --	,JumlahBeli = ROUND(MSaldoAwalPersediaanD.HargaPokok * MSaldoAwalPersediaanD.Qty * MSaldoAwalPersediaanD.Konversi, @DefdigitDecimal)
                        --               --	,NilaiAwal = @NilaiAkhir
                        --               --	,NilaiAkhir = @NilaiAkhir + ROUND(ISNULL(MSaldoAwalPersediaanD.Jumlah, 0), @DefdigitDecimal)
                        --               --FROM @Temp T
                        --               --INNER JOIN MKartuStok ON T.IDKartuStok = MKartuStok.NoID
                        --               --INNER JOIN (
                        --               --	MSaldoAwalPersediaan INNER JOIN MSaldoAwalPersediaanD ON MSaldoAwalPersediaan.NoID = MSaldoAwalPersediaanD.IDHeader
                        --               --	) ON MSaldoAwalPersediaan.NoID = MKartuStok.IDTransaksi
                        --               --	AND MSaldoAwalPersediaanD.NoID = MKartuStok.IDTransaksiD
                        --               --	AND MKartuStok.IDJenisTransaksi = 1
                        --               --WHERE T.NoID = @i
                        --               --Hitung HPP
                        --               UPDATE T
                        --                 SET 
                        --                     Debet = MSaldoAwalPersediaanD.Jumlah,
                        --Kredit = 0,
                        --HargaBeli = ROUND(MSaldoAwalPersediaanD.Jumlah / (ISNULL(T.QtyMasuk, 0) - ISNULL(T.QtyKeluar, 0)), @DefdigitDecimal),
                        --JumlahBeli = MSaldoAwalPersediaanD.Jumlah,
                        --NilaiAkhir = @NilaiAwal + MSaldoAwalPersediaanD.Jumlah
                        --               FROM @Temp T
                        --                    INNER JOIN MKartuStok(NOLOCK) ON MKartuStok.NoID = T.IDKartuStok
                        --                    INNER JOIN MSaldoAwalPersediaanD(NOLOCK) ON MSaldoAwalPersediaanD.NoID = MKartuStok.IDTransaksiD
                        --                                                                AND MSaldoAwalPersediaanD.IDHeader = MKartuStok.IDTransaksi
                        --                                                                AND MKartuStok.IDJenisTransaksi = 1
                        --               WHERE T.NoID = @i;
                        SELECT @HPP = ROUND(CASE
                                                WHEN T.SaldoAkhir = 0
                                                THEN 0
                                                ELSE T.NilaiAkhir / T.SaldoAkhir
                                            END, @DefdigitDecimal), 
                               @HargaBeli = ISNULL(HargaBeli, 0)
                        FROM @Temp T
                        WHERE T.NoID = @i;
                    END;
                    ELSE
                    IF(@IDJenisTransaksi = 2)
                        BEGIN
                            --HargaDef Harga Per Satuan.
                            UPDATE T
                              SET 
                                  Debet = ROUND(MBeliD.Jumlah, @DefdigitDecimal), 
                                  Kredit = 0, 
                                  HargaBeli = CASE
                                                  WHEN(MKartuStok.QtyMasuk * MKartuStok.Konversi) = 0
                                                  THEN @HargaBeli
                                                  ELSE ROUND((ROUND(MBeliD.Jumlah, @DefdigitDecimal)) / (MKartuStok.QtyMasuk * MKartuStok.Konversi), @DefdigitDecimal)
                                              END, 
                                  JumlahBeli = ROUND(MBeliD.Jumlah, @DefdigitDecimal), 
                                  NilaiAkhir = @NilaiAwal + ROUND(MBeliD.Jumlah, @DefdigitDecimal)
                            FROM MKartuStok(NOLOCK)
                                 INNER JOIN @Temp T ON T.IDKartuStok = MKartuStok.NoID
                                 INNER JOIN(MBeliD
                                            INNER JOIN MBeli ON MBeli.NoID = MBeliD.IDHeader) ON MBeli.NoID = MKartuStok.IDTransaksi
                                                                                                 AND MBeliD.NoID = MKartuStok.IDTransaksiD
                                                                                                 AND MKartuStok.IDJenisTransaksi = 2
                            WHERE T.NoID = @i;

                            --Hitung HPP
                            SELECT @HPP = ROUND(CASE
                                                    WHEN T.SaldoAkhir = 0
                                                    THEN 0
                                                    ELSE T.NilaiAkhir / T.SaldoAkhir
                                                END, @DefdigitDecimal), 
                                   @HargaBeli = ISNULL(HargaBeli, 0), 
                                   @SaldoAkhir = T.SaldoAkhir, 
                                   @NilaiAkhir = T.NilaiAkhir
                            FROM @Temp T
                            WHERE T.NoID = @i;
                        END;
                        ELSE
                        IF(@IDJenisTransaksi = 3)
                            BEGIN
                                UPDATE T
                                  SET 
                                      Debet = 0, 
                                      Kredit = CASE
                                                   WHEN ROUND(ISNULL(MReturBeliD.Jumlah, 0), @DefdigitDecimal) > @NilaiAwal
                                                   THEN @NilaiAwal
                                                   ELSE ROUND(ISNULL(MReturBeliD.Jumlah, 0), @DefdigitDecimal)
                                               END, 
                                      HargaBeli = CASE
                                                      WHEN(MKartuStok.QtyKeluar * MKartuStok.Konversi) = 0
                                                      THEN @HargaBeli
                                                      ELSE ROUND((ROUND(ISNULL(MReturBeliD.Jumlah, 0), @DefdigitDecimal)) / (MKartuStok.QtyKeluar * MKartuStok.Konversi), @DefdigitDecimal)
                                                  END, 
                                      JumlahBeli = -1 * ROUND(ISNULL(MReturBeliD.Jumlah, 0), @DefdigitDecimal), 
                                      NilaiAkhir = @NilaiAwal - CASE
                                                                    WHEN ROUND(ISNULL(MReturBeliD.Jumlah, 0), @DefdigitDecimal) > @NilaiAwal
                                                                    THEN @NilaiAwal
                                                                    ELSE ROUND(ISNULL(MReturBeliD.Jumlah, 0), @DefdigitDecimal)
                                                                END
                                FROM MKartuStok(NOLOCK)
                                     INNER JOIN @Temp T ON T.IDKartuStok = MKartuStok.NoID
                                     INNER JOIN(MReturBeliD
                                                INNER JOIN MReturBeli ON MReturBeli.NoID = MReturBeliD.IDHeader) ON MReturBeli.NoID = MKartuStok.IDTransaksi
                                                                                                                    AND MReturBeliD.NoID = MKartuStok.IDTransaksiD
                                                                                                                    AND MKartuStok.IDJenisTransaksi = 3
                                WHERE T.NoID = @i;

                                --Hitung HPP
                                SELECT @HPP = ROUND(CASE
                                                        WHEN T.SaldoAkhir = 0
                                                        THEN 0
                                                        ELSE T.NilaiAkhir / T.SaldoAkhir
                                                    END, @DefdigitDecimal), 
                                       @HargaBeli = ISNULL(HargaBeli, 0), 
                                       @SaldoAkhir = T.SaldoAkhir, 
                                       @NilaiAkhir = T.NilaiAkhir
                                FROM @Temp T
                                WHERE T.NoID = @i;
                            END;
                            ELSE
                            IF(@IDJenisTransaksi = 14)
                                BEGIN
                                    SELECT Debet = ROUND(ISNULL(MPenyesuaianD.Jumlah, 0), 2), 
                                           Kredit = 0, 
                                           HargaBeli = CASE
                                                           WHEN(ISNULL(T.QtyMasuk, 0) - ISNULL(T.QtyKeluar, 0)) = 0
                                                           THEN @HargaBeli
                                                           ELSE ROUND(ROUND(ISNULL(MPenyesuaianD.Jumlah, 0), 2) / (ISNULL(T.QtyMasuk, 0) - ISNULL(T.QtyKeluar, 0)), @DefdigitDecimal)
                                                       END, 
                                           JumlahBeli = ROUND(ISNULL(MPenyesuaianD.Jumlah, 0), 2), 
                                           NilaiAkhir = @NilaiAwal - ROUND(ISNULL(MPenyesuaianD.Jumlah, 0), 2)
                                    FROM @Temp T
                                         INNER JOIN MKartuStok(NOLOCK) ON MKartuStok.NoID = T.IDKartuStok
                                         INNER JOIN MPenyesuaianD(NOLOCK) ON MPenyesuaianD.NoID = MKartuStok.IDTransaksiD
                                                                             AND MPenyesuaianD.IDHeader = MKartuStok.IDTransaksi
                                                                             AND MKartuStok.IDJenisTransaksi = 14
                                    WHERE T.NoID = @i;

                                    --Hitung HPP
                                    SELECT @HPP = ROUND(CASE
                                                            WHEN T.SaldoAkhir = 0
                                                            THEN 0
                                                            ELSE T.NilaiAkhir / T.SaldoAkhir
                                                        END, @DefdigitDecimal), 
                                           @HargaBeli = ISNULL(HargaBeli, 0), 
                                           @SaldoAkhir = T.SaldoAkhir, 
                                           @NilaiAkhir = T.NilaiAkhir
                                    FROM @Temp T
                                    WHERE T.NoID = @i;
                                END;
                                ELSE
                                IF(@IDJenisTransaksi = 19
                                   AND (ISNULL(@QtyIn, 0) - ISNULL(@QtyOut, 0)) > 0) --Hasil Produksi
                                    BEGIN
                                        --UPDATE T
                                        --SET SaldoAwal = @SaldoAkhir + T.QtyKeluar - T.QtyMasuk
                                        --	,SaldoAkhir = @SaldoAkhir
                                        --	,Debet = ROUND(MAssemblyD.HargaPokok * MAssemblyD.Qty * MAssemblyD.Konversi, @DefdigitDecimal)
                                        --	,Kredit = 0
                                        --	,HargaBeli = ROUND(MAssemblyD.HargaPokok, @DefdigitDecimal)
                                        --	,JumlahBeli = ROUND(MAssemblyD.Jumlah, @DefdigitDecimal)
                                        --	,NilaiAwal = @NilaiAkhir
                                        --	,NilaiAkhir = @NilaiAkhir + ROUND(MAssemblyD.HargaPokok * MAssemblyD.Qty * MAssemblyD.Konversi, @DefdigitDecimal)
                                        --FROM MKartuStok(NOLOCK)
                                        --INNER JOIN @Temp T ON T.IDKartuStok = MKartuStok.NoID
                                        --INNER JOIN (
                                        --	MAssembly INNER JOIN MAssemblyD ON MAssemblyD.IDHeader = MAssembly.NoID
                                        --	) ON MKartuStok.IDTransaksi = MAssembly.NoID
                                        --	AND MKartuStok.IDTransaksiD = MAssemblyD.NoID
                                        --	AND MKartuStok.IDJenisTransaksi = 19
                                        --WHERE T.NoID = @i
                                        --Hitung HPP
                                        --Hitung HPP
                                        SELECT @HPP = ROUND(CASE
                                                                WHEN T.SaldoAkhir = 0
                                                                THEN 0
                                                                ELSE T.NilaiAkhir / T.SaldoAkhir
                                                            END, @DefdigitDecimal), 
                                               @HargaBeli = ISNULL(HargaBeli, 0), 
                                               @SaldoAkhir = T.SaldoAkhir, 
                                               @NilaiAkhir = T.NilaiAkhir
                                        FROM @Temp T
                                        WHERE T.NoID = @i;
                                    END;
                                    ELSE
                                    IF(@IDJenisTransaksi = 20
                                       AND (ISNULL(@QtyIn, 0) - ISNULL(@QtyOut, 0)) > 0)
                                        BEGIN
                                            --KALAU SUDAH ADA TRANSAKSI SEBELUM DAN <0 QTYHPP AMBIL DARI PEMBELIAN SEBELUM
                                            --Kalau Saldo Awalnya sudah <=0
                                            IF @SaldoAkhir = 0
                                               AND (@SaldoAkhir - (ISNULL(@QtyIn, 0) - ISNULL(@QtyOut, 0)) <= 0)
                                                BEGIN
                                                    --Tugas SO Pas Saldo Akhir 0 Ngenolin Nilai Akhir jga.
                                                    --UPDATE T
                                                    --SET SaldoAwal = @SaldoAkhir + T.QtyKeluar - T.QtyMasuk
                                                    --	,SaldoAkhir = @SaldoAkhir
                                                    --	,Debet = ABS(@NilaiAkhir)
                                                    --	,--ROUND(ROUND(@HPP, @DefdigitDecimal)*(ISNULL(@QtyIn, 0)-ISNULL(@QtyOut, 0)), @DefdigitDecimal), 
                                                    --	Kredit = 0
                                                    --	,HargaBeli = @HPP
                                                    --	,JumlahBeli = ROUND(ROUND(MStokOpnameOnline.HargaBeli, @DefdigitDecimal) * (ISNULL(@QtyIn, 0) - ISNULL(@QtyOut, 0)), @DefdigitDecimal)
                                                    --	,NilaiAwal = @NilaiAkhir
                                                    --	,NilaiAkhir = @NilaiAkhir - ABS(@NilaiAkhir) --@NilaiAkhir+ROUND(ROUND(@HPP, @DefdigitDecimal)*(ISNULL(@QtyIn, 0)-ISNULL(@QtyOut, 0)), @DefdigitDecimal)
                                                    --FROM MKartuStok(NOLOCK)
                                                    --INNER JOIN @Temp T ON T.IDKartuStok = MKartuStok.NoID
                                                    --INNER JOIN MStokOpnameOnline(NOLOCK) ON MKartuStok.IDTransaksi = MStokOpnameOnline.NoID
                                                    --	AND MKartuStok.IDJenisTransaksi = 20
                                                    --INNER JOIN MBarang ON MStokOpnameOnline.IDBarang = MBarang.NoID
                                                    --WHERE T.NoID = @i
                                                    --Hitung HPP
                                                    SELECT @HPP = ROUND(CASE
                                                                            WHEN T.SaldoAkhir = 0
                                                                            THEN 0
                                                                            ELSE T.NilaiAkhir / T.SaldoAkhir
                                                                        END, @DefdigitDecimal), 
                                                           @HargaBeli = ISNULL(HargaBeli, 0), 
                                                           @SaldoAkhir = T.SaldoAkhir, 
                                                           @NilaiAkhir = T.NilaiAkhir
                                                    FROM @Temp T
                                                    WHERE T.NoID = @i;
                                                END;
                                                ELSE
                                                BEGIN
                                                    --UPDATE T
                                                    --SET SaldoAwal = @SaldoAkhir + T.QtyKeluar - T.QtyMasuk
                                                    --	,SaldoAkhir = @SaldoAkhir
                                                    --	,Debet = ROUND(ROUND(@HargaBeli, @DefdigitDecimal) * (ISNULL(@QtyIn, 0) - ISNULL(@QtyOut, 0)), @DefdigitDecimal)
                                                    --	,
                                                    --	--Debet=ROUND(ROUND(Case When IsNull(MStokOpnameOnline.HargaBeli,0)=0 Then MBarang.HargaBeliPcs Else MStokOpnameOnline.HargaBeli End, @DefdigitDecimal)*(ISNULL(@QtyIn, 0)-ISNULL(@QtyOut, 0)), @DefdigitDecimal), 
                                                    --	Kredit = 0
                                                    --	,HargaBeli = @HargaBeli
                                                    --	,--ROUND(Case When IsNull(MStokOpnameOnline.HargaBeli,0)=0 Then MBarang.HargaBeliPcs Else MStokOpnameOnline.HargaBeli End, 2), 
                                                    --	JumlahBeli = ROUND(ROUND(@HargaBeli, @DefdigitDecimal) * (ISNULL(@QtyIn, 0) - ISNULL(@QtyOut, 0)), @DefdigitDecimal)
                                                    --	,NilaiAwal = @NilaiAkhir
                                                    --	,NilaiAkhir = @NilaiAkhir + ROUND(ROUND(@HargaBeli, @DefdigitDecimal) * (ISNULL(@QtyIn, 0) - ISNULL(@QtyOut, 0)), @DefdigitDecimal)
                                                    --FROM MKartuStok(NOLOCK)
                                                    --INNER JOIN @Temp T ON T.IDKartuStok = MKartuStok.NoID
                                                    --INNER JOIN MStokOpnameOnline(NOLOCK) ON MKartuStok.IDTransaksi = MStokOpnameOnline.NoID
                                                    --	AND MKartuStok.IDJenisTransaksi = 20
                                                    --INNER JOIN MBarang ON MStokOpnameOnline.IDBarang = MBarang.NoID
                                                    --WHERE T.NoID = @i
                                                    --Hitung HPP
                                                    SELECT @HPP = ROUND(CASE
                                                                            WHEN T.SaldoAkhir = 0
                                                                            THEN 0
                                                                            ELSE T.NilaiAkhir / T.SaldoAkhir
                                                                        END, @DefdigitDecimal), 
                                                           @HargaBeli = ISNULL(HargaBeli, 0), 
                                                           @SaldoAkhir = T.SaldoAkhir, 
                                                           @NilaiAkhir = T.NilaiAkhir
                                                    FROM @Temp T
                                                    WHERE T.NoID = @i;
                                                END;
                                        END
                                                    --Selain Transaksi Masuk, pakai Qty Mines Atau Tidak;
                                        ELSE
                                        IF(@SaldoAkhir < 0
                                           AND ISNULL(@QtyIn, 0) - ISNULL(@QtyOut, 0) < 0)
                                            BEGIN
                                                UPDATE T
                                                  SET 
                                                      Debet = ROUND(ISNULL(@HPP, 0) * T.QtyMasuk, @DefdigitDecimal), 
                                                      Kredit = ROUND(ISNULL(@HPP, 0) * T.QtyKeluar, @DefdigitDecimal), 
                                                      HargaBeli = @HargaBeli, 
                                                      JumlahBeli = ROUND(@HargaBeli * (ISNULL(@QtyIn, 0) - ISNULL(@QtyOut, 0)), @DefdigitDecimal), 
                                                      NilaiAkhir = @NilaiAkhir + ROUND(ISNULL(@HPP, 0) * T.QtyMasuk, @DefdigitDecimal) - ROUND(ISNULL(@HPP, 0) * T.QtyKeluar, @DefdigitDecimal)
                                                FROM MKartuStok(NOLOCK)
                                                     INNER JOIN @Temp T ON T.IDKartuStok = MKartuStok.NoID
                                                WHERE T.NoID = @i;

                                                --Hitung HPP
                                                SELECT @HPP = @HPP, 
                                                       @HargaBeli = ISNULL(HargaBeli, 0), 
                                                       @SaldoAkhir = T.SaldoAkhir, 
                                                       @NilaiAkhir = T.NilaiAkhir
                                                FROM @Temp T
                                                WHERE T.NoID = @i;
                                            END;
                                            ELSE
                                            IF(@SaldoAkhir = 0
                                               AND ISNULL(@QtyIn, 0) - ISNULL(@QtyOut, 0) < 0)
                                                BEGIN
                                                    --Jupuk Nilai Persediaan Sakdurunge
                                                    UPDATE T
                                                      SET 
                                                          Debet = 0, 
                                                          Kredit = @NilaiAwal, 
                                                          HargaBeli = @HargaBeli, 
                                                          JumlahBeli = ROUND(@HargaBeli * (ISNULL(@QtyIn, 0) - ISNULL(@QtyOut, 0)), @DefdigitDecimal), 
                                                          NilaiAkhir = @NilaiAwal - @NilaiAwal
                                                    FROM MKartuStok(NOLOCK)
                                                         INNER JOIN @Temp T ON T.IDKartuStok = MKartuStok.NoID
                                                    WHERE T.NoID = @i;

                                                    --Hitung HPP
                                                    SELECT @HPP = @HPP, 
                                                           @HargaBeli = ISNULL(HargaBeli, 0), 
                                                           @SaldoAkhir = T.SaldoAkhir, 
                                                           @NilaiAkhir = T.NilaiAkhir
                                                    FROM @Temp T
                                                    WHERE T.NoID = @i;
                                                END;
                                                ELSE
                                                BEGIN
                                                    --Selain Case diatas
                                                    UPDATE T
                                                      SET 
                                                          Debet = ROUND(@HPP * T.QtyMasuk, @DefdigitDecimal), 
                                                          Kredit = ROUND(@HPP * T.QtyKeluar, @DefdigitDecimal), 
                                                          HargaBeli = @HargaBeli, 
                                                          JumlahBeli = ROUND(@HargaBeli * (ISNULL(@QtyIn, 0) - ISNULL(@QtyOut, 0)), @DefdigitDecimal), 
                                                          NilaiAkhir = @NilaiAkhir + ROUND(@HPP * T.QtyMasuk, @DefdigitDecimal) - ROUND(@HPP * T.QtyKeluar, @DefdigitDecimal)
                                                    FROM MKartuStok(NOLOCK)
                                                         INNER JOIN @Temp T ON T.IDKartuStok = MKartuStok.NoID
                                                    WHERE T.NoID = @i;

                                                    --Hitung HPP
                                                    SELECT @HPP = @HPP, 
                                                           @HargaBeli = ISNULL(HargaBeli, 0), 
                                                           @SaldoAkhir = T.SaldoAkhir, 
                                                           @NilaiAkhir = T.NilaiAkhir
                                                    FROM @Temp T
                                                    WHERE T.NoID = @i;
                                                END;
                -- Final Di Setting Juga
                UPDATE T
                  SET 
                      SaldoAwal = ISNULL(@SaldoAwal, 0), 
                      NilaiAwal = ISNULL(@NilaiAwal, 0), 
                      SaldoAkhir = ISNULL(@SaldoAwal, 0) + T.QtyMasuk - T.QtyKeluar, 
                      NilaiAkhir = ISNULL(@NilaiAkhir, 0), 
                      HPP = @HPP
                FROM @Temp T
                WHERE T.NoID = @i;
                SET @i = @i + 1;
            END;
        UPDATE MKartuStok
          SET 
              Debet = T.Debet, 
              Kredit = T.Kredit, 
              SaldoAkhir = T.SaldoAkhir, 
              HPP = T.HPP, 
              HargaBeli = T.HargaBeli
        FROM MKartuStok
             INNER JOIN @Temp T ON T.IDKartuStok = MKartuStok.NoID;
        --SELECT *
        --FROM @Temp;
        DELETE FROM @Temp;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spCekSaldoStok]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[spCekSaldoStok]
-- Add the parameters for the stored procedure here
@IDBarang AS BIGINT, 
@IDGudang AS INT, 
@Tanggal AS  DATETIME
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        -- Insert statements for procedure here
        SELECT SUM((QtyMasuk - QtyKeluar) * Konversi) AS Saldo
        FROM MKartuStok(NOLOCK)
        WHERE IDBarang = @IDBarang
              AND IDGudang = @IDGudang
              AND Tanggal <= @Tanggal;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spDaftarBeli]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spDaftarBeli] @TglDari   DATETIME = '2019-01-01', 
                                      @TglSampai DATETIME = '2019-01-31'
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        -- Insert statements for procedure here
        SELECT MBeli.*, 
               UserEntry.Nama UserEntry, 
               UserEdit.Nama UserEdit, 
               UserPosted.Nama UserPosted, 
               MAlamat.Kode KodeSupplier, 
               MAlamat.Nama NamaSupplier, 
               MTypePajak.TypePajak, 
               MPO.Kode KodePO, 
               MGudang.Kode AS Gudang
        FROM MBeli(NOLOCK)
             LEFT JOIN MTypePajak(NOLOCK) ON MBeli.IDTypePajak = MTypePajak.NoID
             LEFT JOIN MAlamat(NOLOCK) ON MBeli.IDSupplier = MAlamat.NoID
             LEFT JOIN MGudang(NOLOCK) ON MBeli.IDGudang = MGudang.NoID
             LEFT JOIN MUser UserEntry(NOLOCK) ON MBeli.IDUserEntry = UserEntry.NoID
             LEFT JOIN MUser UserEdit(NOLOCK) ON MBeli.IDUserEdit = UserEdit.NoID
             LEFT JOIN MUser UserPosted(NOLOCK) ON MBeli.IDUserPosted = UserPosted.NoID
             LEFT JOIN MPO(NOLOCK) ON MBeli.IDPO = MPO.NoID
        WHERE CONVERT(DATE, MBeli.Tanggal) >= CONVERT(DATE, @TglDari)
              AND CONVERT(DATE, MBeli.Tanggal) <= CONVERT(DATE, @TglSampai)
        ORDER BY MBeli.Tanggal, 
                 MBeli.NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spDaftarJual]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spDaftarJual] @TglDari   DATETIME = '2019-01-01', 
                                      @TglSampai DATETIME = '2019-01-31'
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        -- Insert statements for procedure here
        SELECT MJual.*, 
               UserEntry.Nama UserEntry, 
               UserEdit.Nama UserEdit, 
               UserPosted.Nama UserPosted, 
               MAlamat.Kode KodeCustomer, 
               MAlamat.Nama NamaCustomer, 
               MTypePajak.TypePajak, 
               MGudang.Kode AS Gudang
        FROM MJual(NOLOCK)
             LEFT JOIN MTypePajak(NOLOCK) ON MJual.IDTypePajak = MTypePajak.NoID
             LEFT JOIN MAlamat(NOLOCK) ON MJual.IDCustomer = MAlamat.NoID
             LEFT JOIN MGudang(NOLOCK) ON MJual.IDGudang = MGudang.NoID
             LEFT JOIN MUser UserEntry(NOLOCK) ON MJual.IDUserEntry = UserEntry.NoID
             LEFT JOIN MUser UserEdit(NOLOCK) ON MJual.IDUserEdit = UserEdit.NoID
             LEFT JOIN MUser UserPosted(NOLOCK) ON MJual.IDUserPosted = UserPosted.NoID
        WHERE CONVERT(DATE, MJual.Tanggal) >= CONVERT(DATE, @TglDari)
              AND CONVERT(DATE, MJual.Tanggal) <= CONVERT(DATE, @TglSampai)
        ORDER BY MJual.Tanggal, 
                 MJual.NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spDaftarMutasiGudang]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spDaftarMutasiGudang] @TglDari   DATETIME = '2019-01-01', 
                                              @TglSampai DATETIME = '2019-01-31'
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        -- Insert statements for procedure here
        SELECT MMutasiGudang.*, 
               UserEntry.Nama UserEntry, 
               UserEdit.Nama UserEdit, 
               UserPosted.Nama UserPosted, 
               MGudangAsal.Kode GudangAsal, 
               MGudangTujuan.Kode GudangTujuan
        FROM MMutasiGudang(NOLOCK)
             LEFT JOIN MGudang MGudangAsal(NOLOCK) ON MMutasiGudang.IDGudangAsal = MGudangAsal.NoID
             LEFT JOIN MGudang MGudangTujuan(NOLOCK) ON MMutasiGudang.IDGudangTujuan = MGudangTujuan.NoID
             LEFT JOIN MUser UserEntry(NOLOCK) ON MMutasiGudang.IDUserEntry = UserEntry.NoID
             LEFT JOIN MUser UserEdit(NOLOCK) ON MMutasiGudang.IDUserEdit = UserEdit.NoID
             LEFT JOIN MUser UserPosted(NOLOCK) ON MMutasiGudang.IDUserPosted = UserPosted.NoID
        WHERE CONVERT(DATE, MMutasiGudang.Tanggal) >= CONVERT(DATE, @TglDari)
              AND CONVERT(DATE, MMutasiGudang.Tanggal) <= CONVERT(DATE, @TglSampai)
        ORDER BY MMutasiGudang.Tanggal, 
                 MMutasiGudang.NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spDaftarPemakaian]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spDaftarPemakaian] @TglDari   DATETIME = '2019-01-01', 
                                           @TglSampai DATETIME = '2019-01-31'
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        -- Insert statements for procedure here
        SELECT MPemakaian.*, 
               UserEntry.Nama UserEntry, 
               UserEdit.Nama UserEdit, 
               UserPosted.Nama UserPosted, 
               MGudang.Kode Gudang
        FROM MPemakaian(NOLOCK)
             LEFT JOIN MGudang(NOLOCK) ON MPemakaian.IDGudang = MGudang.NoID
             LEFT JOIN MUser UserEntry(NOLOCK) ON MPemakaian.IDUserEntry = UserEntry.NoID
             LEFT JOIN MUser UserEdit(NOLOCK) ON MPemakaian.IDUserEdit = UserEdit.NoID
             LEFT JOIN MUser UserPosted(NOLOCK) ON MPemakaian.IDUserPosted = UserPosted.NoID
        WHERE CONVERT(DATE, MPemakaian.Tanggal) >= CONVERT(DATE, @TglDari)
              AND CONVERT(DATE, MPemakaian.Tanggal) <= CONVERT(DATE, @TglSampai)
        ORDER BY MPemakaian.Tanggal, 
                 MPemakaian.NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spDaftarPenyesuaian]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spDaftarPenyesuaian] @TglDari   DATETIME = '2019-01-01', 
                                             @TglSampai DATETIME = '2019-01-31'
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        -- Insert statements for procedure here
        SELECT MPenyesuaian.*, 
               UserEntry.Nama UserEntry, 
               UserEdit.Nama UserEdit, 
               UserPosted.Nama UserPosted, 
               MGudang.Kode Gudang
        FROM MPenyesuaian(NOLOCK)
             LEFT JOIN MGudang(NOLOCK) ON MPenyesuaian.IDGudang = MGudang.NoID
             LEFT JOIN MUser UserEntry(NOLOCK) ON MPenyesuaian.IDUserEntry = UserEntry.NoID
             LEFT JOIN MUser UserEdit(NOLOCK) ON MPenyesuaian.IDUserEdit = UserEdit.NoID
             LEFT JOIN MUser UserPosted(NOLOCK) ON MPenyesuaian.IDUserPosted = UserPosted.NoID
        WHERE CONVERT(DATE, MPenyesuaian.Tanggal) >= CONVERT(DATE, @TglDari)
              AND CONVERT(DATE, MPenyesuaian.Tanggal) <= CONVERT(DATE, @TglSampai)
        ORDER BY MPenyesuaian.Tanggal, 
                 MPenyesuaian.NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spDaftarPO]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spDaftarPO] @TglDari   DATETIME = '2019-01-01', 
                                    @TglSampai DATETIME = '2019-01-31'
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        -- Insert statements for procedure here
        SELECT MPO.*, 
               UserEntry.Nama UserEntry, 
               UserEdit.Nama UserEdit, 
               UserPosted.Nama UserPosted, 
               MAlamat.Kode KodeSupplier, 
               MAlamat.Nama NamaSupplier, 
               MTypePajak.TypePajak
        FROM MPO(NOLOCK)
             LEFT JOIN MTypePajak(NOLOCK) ON MPO.IDTypePajak = MTypePajak.NoID
             LEFT JOIN MAlamat(NOLOCK) ON MPO.IDSupplier = MAlamat.NoID
             LEFT JOIN MUser UserEntry(NOLOCK) ON MPO.IDUserEntry = UserEntry.NoID
             LEFT JOIN MUser UserEdit(NOLOCK) ON MPO.IDUserEdit = UserEdit.NoID
             LEFT JOIN MUser UserPosted(NOLOCK) ON MPO.IDUserPosted = UserPosted.NoID
        WHERE CONVERT(DATE, MPO.Tanggal) >= CONVERT(DATE, @TglDari)
              AND CONVERT(DATE, MPO.Tanggal) <= CONVERT(DATE, @TglSampai)
        ORDER BY MPO.Tanggal, 
                 MPO.NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spDaftarReturBeli]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spDaftarReturBeli] @TglDari   DATETIME = '2019-01-01', 
                                           @TglSampai DATETIME = '2019-01-31'
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        -- Insert statements for procedure here
        SELECT MReturBeli.*, 
               UserEntry.Nama UserEntry, 
               UserEdit.Nama UserEdit, 
               UserPosted.Nama UserPosted, 
               MAlamat.Kode KodeSupplier, 
               MAlamat.Nama NamaSupplier, 
               MTypePajak.TypePajak, 
               MGudang.Kode AS Gudang
        FROM MReturBeli(NOLOCK)
             LEFT JOIN MTypePajak(NOLOCK) ON MReturBeli.IDTypePajak = MTypePajak.NoID
             LEFT JOIN MAlamat(NOLOCK) ON MReturBeli.IDSupplier = MAlamat.NoID
             LEFT JOIN MGudang(NOLOCK) ON MReturBeli.IDGudang = MGudang.NoID
             LEFT JOIN MUser UserEntry(NOLOCK) ON MReturBeli.IDUserEntry = UserEntry.NoID
             LEFT JOIN MUser UserEdit(NOLOCK) ON MReturBeli.IDUserEdit = UserEdit.NoID
             LEFT JOIN MUser UserPosted(NOLOCK) ON MReturBeli.IDUserPosted = UserPosted.NoID
        WHERE CONVERT(DATE, MReturBeli.Tanggal) >= CONVERT(DATE, @TglDari)
              AND CONVERT(DATE, MReturBeli.Tanggal) <= CONVERT(DATE, @TglSampai)
        ORDER BY MReturBeli.Tanggal, 
                 MReturBeli.NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spDaftarReturJual]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[spDaftarReturJual] @TglDari   DATETIME = '2019-01-01', 
                                           @TglSampai DATETIME = '2019-01-31'
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;
        -- Insert statements for procedure here
        SELECT MReturJual.*, 
               UserEntry.Nama UserEntry, 
               UserEdit.Nama UserEdit, 
               UserPosted.Nama UserPosted, 
               MAlamat.Kode KodeCustomer, 
               MAlamat.Nama NamaCustomer, 
               MTypePajak.TypePajak, 
               MGudang.Kode AS Gudang
        FROM MReturJual(NOLOCK)
             LEFT JOIN MTypePajak(NOLOCK) ON MReturJual.IDTypePajak = MTypePajak.NoID
             LEFT JOIN MAlamat(NOLOCK) ON MReturJual.IDCustomer = MAlamat.NoID
             LEFT JOIN MGudang(NOLOCK) ON MReturJual.IDGudang = MGudang.NoID
             LEFT JOIN MUser UserEntry(NOLOCK) ON MReturJual.IDUserEntry = UserEntry.NoID
             LEFT JOIN MUser UserEdit(NOLOCK) ON MReturJual.IDUserEdit = UserEdit.NoID
             LEFT JOIN MUser UserPosted(NOLOCK) ON MReturJual.IDUserPosted = UserPosted.NoID
        WHERE CONVERT(DATE, MReturJual.Tanggal) >= CONVERT(DATE, @TglDari)
              AND CONVERT(DATE, MReturJual.Tanggal) <= CONVERT(DATE, @TglSampai)
        ORDER BY MReturJual.Tanggal, 
                 MReturJual.NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spDaftarSaldoAwalHutang]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Author: <Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spDaftarSaldoAwalHutang]
(@TglDari   DATETIME = '2019-01-01', 
 @TglSampai DATETIME = '2019-01-31'
)
AS
    BEGIN
        SET NOCOUNT ON;
        SELECT MSaldoAwalHutang.*, 
               MAlamat.Kode KodeSupplier, 
               MAlamat.Nama NamaSupplier, 
               MAlamat.Alamat AlamatSupplier, 
               UserEntry.Nama UserEntry, 
               UserEdit.Nama UserEdit, 
               UserPosted.Nama UserPosted
        FROM MSaldoAwalHutang(NOLOCK)
             LEFT JOIN MAlamat(NOLOCK) ON MSaldoAwalHutang.IDSupplier = MAlamat.NoID
             LEFT JOIN MUser UserEntry(NOLOCK) ON MSaldoAwalHutang.IDUserEntry = UserEntry.NoID
             LEFT JOIN MUser UserEdit(NOLOCK) ON MSaldoAwalHutang.IDUserEdit = UserEdit.NoID
             LEFT JOIN MUser UserPosted(NOLOCK) ON MSaldoAwalHutang.IDUserPosted = UserPosted.NoID
        WHERE CONVERT(DATE, MSaldoAwalHutang.Tanggal) >= CONVERT(DATE, @TglDari)
              AND CONVERT(DATE, MSaldoAwalHutang.Tanggal) <= CONVERT(DATE, @TglSampai)
        ORDER BY MSaldoAwalHutang.Tanggal, 
                 MSaldoAwalHutang.NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spDaftarSaldoAwalPersediaan]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Author: <Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spDaftarSaldoAwalPersediaan]
(@TglDari   DATETIME = '2019-01-01', 
 @TglSampai DATETIME = '2019-01-31'
)
AS
    BEGIN
        SET NOCOUNT ON;
        SELECT MSaldoAwalPersediaan.*, 
               MBarang.Kode KodeBarang, 
               MBarang.Nama NamaBarang, 
               MBarangD.Barcode, 
               UserEntry.Nama UserEntry, 
               UserEdit.Nama UserEdit, 
               UserPosted.Nama UserPosted, 
               MGudang.Kode Gudang
        FROM MSaldoAwalPersediaan(NOLOCK)
             LEFT JOIN MGudang(NOLOCK) ON MSaldoAwalPersediaan.IDGudang = MGudang.NoID
             LEFT JOIN MBarang(NOLOCK) ON MSaldoAwalPersediaan.IDBarang = MBarang.NoID
             LEFT JOIN MBarangD(NOLOCK) ON MSaldoAwalPersediaan.IDBarangD = MBarangD.NoID
             LEFT JOIN MUser UserEntry(NOLOCK) ON MSaldoAwalPersediaan.IDUserEntry = UserEntry.NoID
             LEFT JOIN MUser UserEdit(NOLOCK) ON MSaldoAwalPersediaan.IDUserEdit = UserEdit.NoID
             LEFT JOIN MUser UserPosted(NOLOCK) ON MSaldoAwalPersediaan.IDUserPosted = UserPosted.NoID
        WHERE CONVERT(DATE, MSaldoAwalPersediaan.Tanggal) >= CONVERT(DATE, @TglDari)
              AND CONVERT(DATE, MSaldoAwalPersediaan.Tanggal) <= CONVERT(DATE, @TglSampai)
        ORDER BY MSaldoAwalPersediaan.Tanggal, 
                 MSaldoAwalPersediaan.NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spDaftarSaldoAwalPiutang]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Author: <Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spDaftarSaldoAwalPiutang]
(@TglDari   DATETIME = '2019-01-01', 
 @TglSampai DATETIME = '2019-01-31'
)
AS
    BEGIN
        SET NOCOUNT ON;
        SELECT MSaldoAwalPiutang.*, 
               MAlamat.Kode KodeCustomer, 
               MAlamat.Nama NamaCustomer, 
               MAlamat.Alamat AlamatCustomer, 
               UserEntry.Nama UserEntry, 
               UserEdit.Nama UserEdit, 
               UserPosted.Nama UserPosted
        FROM MSaldoAwalPiutang(NOLOCK)
             LEFT JOIN MAlamat(NOLOCK) ON MSaldoAwalPiutang.IDCustomer = MAlamat.NoID
             LEFT JOIN MUser UserEntry(NOLOCK) ON MSaldoAwalPiutang.IDUserEntry = UserEntry.NoID
             LEFT JOIN MUser UserEdit(NOLOCK) ON MSaldoAwalPiutang.IDUserEdit = UserEdit.NoID
             LEFT JOIN MUser UserPosted(NOLOCK) ON MSaldoAwalPiutang.IDUserPosted = UserPosted.NoID
        WHERE CONVERT(DATE, MSaldoAwalPiutang.Tanggal) >= CONVERT(DATE, @TglDari)
              AND CONVERT(DATE, MSaldoAwalPiutang.Tanggal) <= CONVERT(DATE, @TglSampai)
        ORDER BY MSaldoAwalPiutang.Tanggal, 
                 MSaldoAwalPiutang.NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spDaftarStockOpname]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Author: <Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spDaftarStockOpname]
(@TglDari   DATETIME = '2019-01-01', 
 @TglSampai DATETIME = '2019-01-31'
)
AS
    BEGIN
        SET NOCOUNT ON;
        SELECT MStockOpname.*, 
               UserEntry.Nama UserEntry, 
               UserEdit.Nama UserEdit, 
               UserPosted.Nama UserPosted, 
               MGudang.Kode Gudang
        FROM MStockOpname(NOLOCK)
             LEFT JOIN MGudang(NOLOCK) ON MStockOpname.IDGudang = MGudang.NoID
             LEFT JOIN MUser UserEntry(NOLOCK) ON MStockOpname.IDUserEntry = UserEntry.NoID
             LEFT JOIN MUser UserEdit(NOLOCK) ON MStockOpname.IDUserEdit = UserEdit.NoID
             LEFT JOIN MUser UserPosted(NOLOCK) ON MStockOpname.IDUserPosted = UserPosted.NoID
        WHERE CONVERT(DATE, MStockOpname.Tanggal) >= CONVERT(DATE, @TglDari)
              AND CONVERT(DATE, MStockOpname.Tanggal) <= CONVERT(DATE, @TglSampai)
        ORDER BY MStockOpname.Tanggal, 
                 MStockOpname.NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spFakturMBeli]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spFakturMBeli] @NoID BIGINT = -1
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        -- Insert statements for procedure here
        SELECT dbo.MBeliD.NoID, 
               dbo.MBeliD.Konversi, 
               dbo.MBeliD.Qty, 
               dbo.MBeliD.Harga, 
               dbo.MBeliD.DiscProsen1, 
               dbo.MBeliD.DiscProsen2, 
               dbo.MBeliD.DiscProsen3, 
               dbo.MBeliD.DiscProsen4, 
               dbo.MBeliD.DiscProsen5, 
               dbo.MBeliD.DiscRp, 
               dbo.MBeliD.JumlahBruto, 
               dbo.MBeliD.DPP, 
               dbo.MBeliD.PPN, 
               dbo.MBeliD.Jumlah, 
               dbo.MBeli.Kode, 
               UserEntry.Nama AS UserEntry, 
               UserEdit.Nama AS UserEdit, 
               UserPosted.Nama AS UserPosted, 
               dbo.MAlamat.Kode AS KodeSupplier, 
               dbo.MAlamat.Nama AS NamaSupplier, 
               dbo.MTypePajak.TypePajak, 
               dbo.MBeli.NoReff, 
               dbo.MBeli.Tanggal, 
               dbo.MBeli.Catatan, 
               dbo.MBeli.Subtotal, 
               dbo.MBeli.DiscNotaProsen, 
               dbo.MBeli.DiscNotaRp, 
               dbo.MBeli.TotalBruto, 
               dbo.MBeli.DPP AS DPPHeader, 
               dbo.MBeli.PPN AS PPNHeader, 
               dbo.MBeli.Total, 
               dbo.MBeli.IsPosted, 
               dbo.MBeli.TglPosted, 
               dbo.MBeli.JatuhTempo, 
               dbo.MBeliD.IDHeader, 
               dbo.MBarang.Kode AS KodeBarang, 
               dbo.MBarang.Nama AS NamaBarang, 
               dbo.MBarangD.Barcode, 
               dbo.MSatuan.Kode AS Satuan, 
               MPO.Kode AS NoPO
        FROM((dbo.MBeli
              INNER JOIN dbo.MBeliD ON dbo.MBeliD.IDHeader = dbo.MBeli.NoID)
              LEFT JOIN dbo.MBarangD ON dbo.MBeliD.IDBarangD = dbo.MBarangD.NoID)
              LEFT JOIN dbo.MSatuan ON dbo.MBeliD.IDSatuan = dbo.MSatuan.NoID
              LEFT JOIN dbo.MBarang ON dbo.MBeliD.IDBarang = dbo.MBarang.NoID
              LEFT JOIN dbo.MTypePajak ON dbo.MBeli.IDTypePajak = dbo.MTypePajak.NoID
              LEFT JOIN dbo.MAlamat ON dbo.MAlamat.NoID = dbo.MBeli.IDSupplier
              LEFT JOIN dbo.MUser AS UserPosted ON UserPosted.NoID = dbo.MBeli.IDUserPosted
              LEFT JOIN dbo.MUser AS UserEntry ON dbo.MBeli.IDUserEntry = UserEntry.NoID
              LEFT JOIN dbo.MUser AS UserEdit ON dbo.MBeli.IDUserEdit = UserEdit.NoID
              LEFT JOIN dbo.MPOD ON dbo.MBeliD.IDPOD = dbo.MPOD.NoID
              LEFT JOIN dbo.MPO ON dbo.MPOD.IDHeader = dbo.MPO.NoID
        WHERE dbo.MBeli.IsPosted = 1
              AND dbo.MBeli.NoID = @NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spFakturMJual]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spFakturMJual] @NoID BIGINT = -1
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        -- Insert statements for procedure here
        SELECT dbo.MJualD.NoID, 
               dbo.MJualD.Konversi, 
               dbo.MJualD.Qty, 
               dbo.MJualD.Harga, 
               dbo.MJualD.DiscProsen1, 
               dbo.MJualD.DiscProsen2, 
               dbo.MJualD.DiscProsen3, 
               dbo.MJualD.DiscProsen4, 
               dbo.MJualD.DiscProsen5, 
               dbo.MJualD.DiscRp, 
               dbo.MJualD.JumlahBruto, 
               dbo.MJualD.DPP, 
               dbo.MJualD.PPN, 
               dbo.MJualD.Jumlah, 
               dbo.MJual.Kode, 
               UserEntry.Nama AS UserEntry, 
               UserEdit.Nama AS UserEdit, 
               UserPosted.Nama AS UserPosted, 
               dbo.MAlamat.Kode AS KodeCustomer, 
               dbo.MAlamat.Nama AS NamaCustomer, 
               dbo.MTypePajak.TypePajak, 
               dbo.MJual.NoReff, 
               dbo.MJual.Tanggal, 
               dbo.MJual.Catatan, 
               dbo.MJual.Subtotal, 
               dbo.MJual.DiscNotaProsen, 
               dbo.MJual.DiscNotaRp, 
               dbo.MJual.TotalBruto, 
               dbo.MJual.DPP AS DPPHeader, 
               dbo.MJual.PPN AS PPNHeader, 
               dbo.MJual.Total, 
               dbo.MJual.IsPosted, 
               dbo.MJual.TglPosted, 
               dbo.MJual.JatuhTempo, 
               dbo.MJualD.IDHeader, 
               dbo.MBarang.Kode AS KodeBarang, 
               dbo.MBarang.Nama AS NamaBarang, 
               dbo.MBarangD.Barcode, 
               dbo.MSatuan.Kode AS Satuan, 
               dbo.MJual.Sisa, 
               dbo.MJual.Bayar
        FROM((dbo.MJual
              INNER JOIN dbo.MJualD ON dbo.MJualD.IDHeader = dbo.MJual.NoID)
              LEFT JOIN dbo.MBarangD ON dbo.MJualD.IDBarangD = dbo.MBarangD.NoID)
              LEFT JOIN dbo.MSatuan ON dbo.MJualD.IDSatuan = dbo.MSatuan.NoID
              LEFT JOIN dbo.MBarang ON dbo.MJualD.IDBarang = dbo.MBarang.NoID
              LEFT JOIN dbo.MTypePajak ON dbo.MJual.IDTypePajak = dbo.MTypePajak.NoID
              LEFT JOIN dbo.MAlamat ON dbo.MAlamat.NoID = dbo.MJual.IDCustomer
              LEFT JOIN dbo.MUser AS UserPosted ON UserPosted.NoID = dbo.MJual.IDUserPosted
              LEFT JOIN dbo.MUser AS UserEntry ON dbo.MJual.IDUserEntry = UserEntry.NoID
              LEFT JOIN dbo.MUser AS UserEdit ON dbo.MJual.IDUserEdit = UserEdit.NoID
        WHERE dbo.MJual.IsPosted = 1
              AND dbo.MJual.NoID = @NoID;
        SELECT MJualDBayar.*, 
               MJenisPembayaran.Kode KodePembayaran, 
               MJenisPembayaran.Nama NamaPembayaran
        FROM MJualDBayar(NOLOCK)
             LEFT JOIN MJenisPembayaran(NOLOCK) ON MJualDBayar.IDJenisPembayaran = MJenisPembayaran.NoID
        WHERE MJualDBayar.IDHeader = @NoID
        ORDER BY MJualDBayar.NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spFakturMMutasiGudang]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spFakturMMutasiGudang] @NoID BIGINT = -1
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        -- Insert statements for procedure here
        SELECT MMutasiGudangD.*, 
               MMutasiGudang.Kode, 
               UserEntry.Nama AS UserEntry, 
               UserEdit.Nama AS UserEdit, 
               UserPosted.Nama AS UserPosted, 
               MGudangAsal.Kode AS GudangAsal, 
               MGudangTujuan.Kode AS GudangTujuan, 
               MMutasiGudang.NoReff, 
               MMutasiGudang.Tanggal, 
               MMutasiGudang.Catatan, 
               MMutasiGudang.Total, 
               MMutasiGudang.IsPosted, 
               MMutasiGudang.TglPosted, 
               MMutasiGudangD.IDHeader, 
               MBarang.Kode AS KodeBarang, 
               MBarang.Nama AS NamaBarang, 
               MBarangD.Barcode, 
               MSatuan.Kode AS Satuan
        FROM MMutasiGudang
             INNER JOIN MMutasiGudangD ON MMutasiGudangD.IDHeader = MMutasiGudang.NoID
             LEFT JOIN MBarangD ON MMutasiGudangD.IDBarangD = MBarangD.NoID
             LEFT JOIN MSatuan ON MMutasiGudangD.IDSatuan = MSatuan.NoID
             LEFT JOIN MBarang ON MMutasiGudangD.IDBarang = MBarang.NoID
             LEFT JOIN MGudang MGudangAsal ON MGudangAsal.NoID = MMutasiGudang.IDGudangAsal
             LEFT JOIN MGudang MGudangTujuan ON MGudangTujuan.NoID = MMutasiGudang.IDGudangTujuan
             LEFT JOIN MUser AS UserPosted ON UserPosted.NoID = MMutasiGudang.IDUserPosted
             LEFT JOIN MUser AS UserEntry ON MMutasiGudang.IDUserEntry = UserEntry.NoID
             LEFT JOIN MUser AS UserEdit ON MMutasiGudang.IDUserEdit = UserEdit.NoID
        WHERE MMutasiGudang.IsPosted = 1
              AND MMutasiGudang.NoID = @NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spFakturMPemakaian]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spFakturMPemakaian] @NoID BIGINT = -1
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        -- Insert statements for procedure here
        SELECT MPemakaianD.*, 
               MPemakaian.Kode, 
               UserEntry.Nama AS UserEntry, 
               UserEdit.Nama AS UserEdit, 
               UserPosted.Nama AS UserPosted, 
               MGudang.Kode AS Gudang, 
               MPemakaian.NoReff, 
               MPemakaian.Tanggal, 
               MPemakaian.Catatan, 
               MPemakaian.Total, 
               MPemakaian.IsPosted, 
               MPemakaian.TglPosted, 
               MPemakaianD.IDHeader, 
               MBarang.Kode AS KodeBarang, 
               MBarang.Nama AS NamaBarang, 
               MBarangD.Barcode, 
               MSatuan.Kode AS Satuan
        FROM MPemakaian
             INNER JOIN MPemakaianD ON MPemakaianD.IDHeader = MPemakaian.NoID
             LEFT JOIN MBarangD ON MPemakaianD.IDBarangD = MBarangD.NoID
             LEFT JOIN MSatuan ON MPemakaianD.IDSatuan = MSatuan.NoID
             LEFT JOIN MBarang ON MPemakaianD.IDBarang = MBarang.NoID
             LEFT JOIN MGudang ON MGudang.NoID = MPemakaian.IDGudang
             LEFT JOIN MUser AS UserPosted ON UserPosted.NoID = MPemakaian.IDUserPosted
             LEFT JOIN MUser AS UserEntry ON MPemakaian.IDUserEntry = UserEntry.NoID
             LEFT JOIN MUser AS UserEdit ON MPemakaian.IDUserEdit = UserEdit.NoID
        WHERE MPemakaian.IsPosted = 1
              AND MPemakaian.NoID = @NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spFakturMPenyesuaian]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spFakturMPenyesuaian] @NoID BIGINT = -1
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        -- Insert statements for procedure here
        SELECT MPenyesuaianD.*, 
               MPenyesuaian.Kode, 
               UserEntry.Nama AS UserEntry, 
               UserEdit.Nama AS UserEdit, 
               UserPosted.Nama AS UserPosted, 
               MGudang.Kode AS Gudang, 
               MPenyesuaian.NoReff, 
               MPenyesuaian.Tanggal, 
               MPenyesuaian.Catatan, 
               MPenyesuaian.Total, 
               MPenyesuaian.IsPosted, 
               MPenyesuaian.TglPosted, 
               MPenyesuaianD.IDHeader, 
               MBarang.Kode AS KodeBarang, 
               MBarang.Nama AS NamaBarang, 
               MBarangD.Barcode, 
               MSatuan.Kode AS Satuan
        FROM MPenyesuaian
             INNER JOIN MPenyesuaianD ON MPenyesuaianD.IDHeader = MPenyesuaian.NoID
             LEFT JOIN MBarangD ON MPenyesuaianD.IDBarangD = MBarangD.NoID
             LEFT JOIN MSatuan ON MPenyesuaianD.IDSatuan = MSatuan.NoID
             LEFT JOIN MBarang ON MPenyesuaianD.IDBarang = MBarang.NoID
             LEFT JOIN MGudang ON MGudang.NoID = MPenyesuaian.IDGudang
             LEFT JOIN MUser AS UserPosted ON UserPosted.NoID = MPenyesuaian.IDUserPosted
             LEFT JOIN MUser AS UserEntry ON MPenyesuaian.IDUserEntry = UserEntry.NoID
             LEFT JOIN MUser AS UserEdit ON MPenyesuaian.IDUserEdit = UserEdit.NoID
        WHERE MPenyesuaian.IsPosted = 1
              AND MPenyesuaian.NoID = @NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spFakturMPO]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spFakturMPO] @NoID BIGINT = -1
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        -- Insert statements for procedure here
        SELECT dbo.MPOD.NoID, 
               dbo.MPOD.Konversi, 
               dbo.MPOD.Qty, 
               dbo.MPOD.Harga, 
               dbo.MPOD.DiscProsen1, 
               dbo.MPOD.DiscProsen2, 
               dbo.MPOD.DiscProsen3, 
               dbo.MPOD.DiscProsen4, 
               dbo.MPOD.DiscProsen5, 
               dbo.MPOD.DiscRp, 
               dbo.MPOD.JumlahBruto, 
               dbo.MPOD.DPP, 
               dbo.MPOD.PPN, 
               dbo.MPOD.Jumlah, 
               dbo.MPO.Kode, 
               UserEntry.Nama AS UserEntry, 
               UserEdit.Nama AS UserEdit, 
               UserPosted.Nama AS UserPosted, 
               dbo.MAlamat.Kode AS KodeSupplier, 
               dbo.MAlamat.Nama AS NamaSupplier, 
               dbo.MTypePajak.TypePajak, 
               dbo.MPO.NoReff, 
               dbo.MPO.Tanggal, 
               dbo.MPO.Expired, 
               dbo.MPO.Catatan, 
               dbo.MPO.Subtotal, 
               dbo.MPO.DiscNotaProsen, 
               dbo.MPO.DiscNotaRp, 
               dbo.MPO.TotalBruto, 
               dbo.MPO.DPP AS DPPHeader, 
               dbo.MPO.PPN AS PPNHeader, 
               dbo.MPO.Total, 
               dbo.MPO.IsPosted, 
               dbo.MPO.TglPosted, 
               dbo.MPO.IsExpired, 
               dbo.MPOD.IDHeader, 
               dbo.MBarang.Kode AS KodeBarang, 
               dbo.MBarang.Nama AS NamaBarang, 
               dbo.MBarangD.Barcode, 
               dbo.MSatuan.Kode AS Satuan
        FROM((dbo.MPO
              INNER JOIN dbo.MPOD ON dbo.MPOD.IDHeader = dbo.MPO.NoID)
              LEFT JOIN dbo.MBarangD ON dbo.MPOD.IDBarangD = dbo.MBarangD.NoID)
              LEFT JOIN dbo.MSatuan ON dbo.MPOD.IDSatuan = dbo.MSatuan.NoID
              LEFT JOIN dbo.MBarang ON dbo.MPOD.IDBarang = dbo.MBarang.NoID
              LEFT JOIN dbo.MTypePajak ON dbo.MPO.IDTypePajak = dbo.MTypePajak.NoID
              LEFT JOIN dbo.MAlamat ON dbo.MAlamat.NoID = dbo.MPO.IDSupplier
              LEFT JOIN dbo.MUser AS UserPosted ON UserPosted.NoID = dbo.MPO.IDUserPosted
              LEFT JOIN dbo.MUser AS UserEntry ON dbo.MPO.IDUserEntry = UserEntry.NoID
              LEFT JOIN dbo.MUser AS UserEdit ON dbo.MPO.IDUserEdit = UserEdit.NoID
        WHERE dbo.MPO.IsPosted = 1
              AND dbo.MPO.NoID = @NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spFakturMReturBeli]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spFakturMReturBeli] @NoID BIGINT = -1
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        -- Insert statements for procedure here
        SELECT dbo.MReturBeliD.NoID, 
               dbo.MReturBeliD.Konversi, 
               dbo.MReturBeliD.Qty, 
               dbo.MReturBeliD.Harga, 
               dbo.MReturBeliD.DiscProsen1, 
               dbo.MReturBeliD.DiscProsen2, 
               dbo.MReturBeliD.DiscProsen3, 
               dbo.MReturBeliD.DiscProsen4, 
               dbo.MReturBeliD.DiscProsen5, 
               dbo.MReturBeliD.DiscRp, 
               dbo.MReturBeliD.JumlahBruto, 
               dbo.MReturBeliD.DPP, 
               dbo.MReturBeliD.PPN, 
               dbo.MReturBeliD.Jumlah, 
               dbo.MReturBeli.Kode, 
               UserEntry.Nama AS UserEntry, 
               UserEdit.Nama AS UserEdit, 
               UserPosted.Nama AS UserPosted, 
               dbo.MAlamat.Kode AS KodeSupplier, 
               dbo.MAlamat.Nama AS NamaSupplier, 
               dbo.MTypePajak.TypePajak, 
               dbo.MReturBeli.NoReff, 
               dbo.MReturBeli.Tanggal, 
               dbo.MReturBeli.Catatan, 
               dbo.MReturBeli.Subtotal, 
               dbo.MReturBeli.DiscNotaProsen, 
               dbo.MReturBeli.DiscNotaRp, 
               dbo.MReturBeli.TotalBruto, 
               dbo.MReturBeli.DPP AS DPPHeader, 
               dbo.MReturBeli.PPN AS PPNHeader, 
               dbo.MReturBeli.Total, 
               dbo.MReturBeli.IsPosted, 
               dbo.MReturBeli.TglPosted, 
               dbo.MReturBeli.JatuhTempo, 
               dbo.MReturBeliD.IDHeader, 
               dbo.MBarang.Kode AS KodeBarang, 
               dbo.MBarang.Nama AS NamaBarang, 
               dbo.MBarangD.Barcode, 
               dbo.MSatuan.Kode AS Satuan, 
               MBeli.Kode AS NoBeli
        FROM((dbo.MReturBeli
              INNER JOIN dbo.MReturBeliD ON dbo.MReturBeliD.IDHeader = dbo.MReturBeli.NoID)
              LEFT JOIN dbo.MBarangD ON dbo.MReturBeliD.IDBarangD = dbo.MBarangD.NoID)
              LEFT JOIN dbo.MSatuan ON dbo.MReturBeliD.IDSatuan = dbo.MSatuan.NoID
              LEFT JOIN dbo.MBarang ON dbo.MReturBeliD.IDBarang = dbo.MBarang.NoID
              LEFT JOIN dbo.MTypePajak ON dbo.MReturBeli.IDTypePajak = dbo.MTypePajak.NoID
              LEFT JOIN dbo.MAlamat ON dbo.MAlamat.NoID = dbo.MReturBeli.IDSupplier
              LEFT JOIN dbo.MUser AS UserPosted ON UserPosted.NoID = dbo.MReturBeli.IDUserPosted
              LEFT JOIN dbo.MUser AS UserEntry ON dbo.MReturBeli.IDUserEntry = UserEntry.NoID
              LEFT JOIN dbo.MUser AS UserEdit ON dbo.MReturBeli.IDUserEdit = UserEdit.NoID
              LEFT JOIN dbo.MBeliD ON dbo.MReturBeliD.IDBeliD = dbo.MBeliD.NoID
              LEFT JOIN dbo.MBeli ON dbo.MBeliD.IDHeader = dbo.MBeli.NoID
        WHERE dbo.MReturBeli.IsPosted = 1
              AND dbo.MReturBeli.NoID = @NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spFakturMReturJual]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[spFakturMReturJual] @NoID BIGINT = -1
AS
    BEGIN
        SET NOCOUNT ON;
        SELECT dbo.MReturJualD.NoID, 
               dbo.MReturJualD.Konversi, 
               dbo.MReturJualD.Qty, 
               dbo.MReturJualD.Harga, 
               dbo.MReturJualD.DiscProsen1, 
               dbo.MReturJualD.DiscProsen2, 
               dbo.MReturJualD.DiscProsen3, 
               dbo.MReturJualD.DiscProsen4, 
               dbo.MReturJualD.DiscProsen5, 
               dbo.MReturJualD.DiscRp, 
               dbo.MReturJualD.JumlahBruto, 
               dbo.MReturJualD.DPP, 
               dbo.MReturJualD.PPN, 
               dbo.MReturJualD.Jumlah, 
               dbo.MReturJual.Kode, 
               UserEntry.Nama AS UserEntry, 
               UserEdit.Nama AS UserEdit, 
               UserPosted.Nama AS UserPosted, 
               dbo.MAlamat.Kode AS KodeCustomer, 
               dbo.MAlamat.Nama AS NamaCustomer, 
               dbo.MTypePajak.TypePajak, 
               dbo.MReturJual.NoReff, 
               dbo.MReturJual.Tanggal, 
               dbo.MReturJual.Catatan, 
               dbo.MReturJual.Subtotal, 
               dbo.MReturJual.DiscNotaProsen, 
               dbo.MReturJual.DiscNotaRp, 
               dbo.MReturJual.TotalBruto, 
               dbo.MReturJual.DPP AS DPPHeader, 
               dbo.MReturJual.PPN AS PPNHeader, 
               dbo.MReturJual.Total, 
               dbo.MReturJual.IsPosted, 
               dbo.MReturJual.TglPosted, 
               dbo.MReturJual.JatuhTempo, 
               dbo.MReturJualD.IDHeader, 
               dbo.MBarang.Kode AS KodeBarang, 
               dbo.MBarang.Nama AS NamaBarang, 
               dbo.MBarangD.Barcode, 
               dbo.MSatuan.Kode AS Satuan, 
               dbo.MReturJual.Sisa, 
               dbo.MReturJual.Bayar
        FROM((dbo.MReturJual
              INNER JOIN dbo.MReturJualD ON dbo.MReturJualD.IDHeader = dbo.MReturJual.NoID)
              LEFT JOIN dbo.MBarangD ON dbo.MReturJualD.IDBarangD = dbo.MBarangD.NoID)
              LEFT JOIN dbo.MSatuan ON dbo.MReturJualD.IDSatuan = dbo.MSatuan.NoID
              LEFT JOIN dbo.MBarang ON dbo.MReturJualD.IDBarang = dbo.MBarang.NoID
              LEFT JOIN dbo.MTypePajak ON dbo.MReturJual.IDTypePajak = dbo.MTypePajak.NoID
              LEFT JOIN dbo.MAlamat ON dbo.MAlamat.NoID = dbo.MReturJual.IDCustomer
              LEFT JOIN dbo.MUser AS UserPosted ON UserPosted.NoID = dbo.MReturJual.IDUserPosted
              LEFT JOIN dbo.MUser AS UserEntry ON dbo.MReturJual.IDUserEntry = UserEntry.NoID
              LEFT JOIN dbo.MUser AS UserEdit ON dbo.MReturJual.IDUserEdit = UserEdit.NoID
        WHERE dbo.MReturJual.IsPosted = 1
              AND dbo.MReturJual.NoID = @NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spFakturMSaldoAwalHutang]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Author: <Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spFakturMSaldoAwalHutang]
(@TglDari   DATETIME = '2019-01-01', 
 @TglSampai DATETIME = '2019-01-31'
)
AS
    BEGIN
        SET NOCOUNT ON;
        SELECT MSaldoAwalHutang.*, 
               MAlamat.Kode KodeSupplier, 
               MAlamat.Nama NamaSupplier, 
               UserEntry.Nama UserEntry, 
               UserEdit.Nama UserEdit, 
               UserPosted.Nama UserPosted
        FROM MSaldoAwalHutang(NOLOCK)
             LEFT JOIN MAlamat(NOLOCK) ON MSaldoAwalHutang.IDSupplier = MAlamat.NoID
             LEFT JOIN MUser UserEntry(NOLOCK) ON MSaldoAwalHutang.IDUserEntry = UserEntry.NoID
             LEFT JOIN MUser UserEdit(NOLOCK) ON MSaldoAwalHutang.IDUserEdit = UserEdit.NoID
             LEFT JOIN MUser UserPosted(NOLOCK) ON MSaldoAwalHutang.IDUserPosted = UserPosted.NoID
        WHERE MSaldoAwalHutang.IsPosted = 1
              AND CONVERT(DATE, MSaldoAwalHutang.Tanggal) >= CONVERT(DATE, @TglDari)
              AND CONVERT(DATE, MSaldoAwalHutang.Tanggal) <= CONVERT(DATE, @TglSampai)
        ORDER BY MSaldoAwalHutang.Tanggal, 
                 MSaldoAwalHutang.NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spFakturMSaldoAwalPersediaan]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Author: <Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spFakturMSaldoAwalPersediaan]
(@TglDari   DATETIME = '2019-01-01', 
 @TglSampai DATETIME = '2019-01-31'
)
AS
    BEGIN
        SET NOCOUNT ON;
        SELECT MSaldoAwalPersediaan.*, 
               MBarang.Kode KodeBarang, 
               MBarang.Nama NamaBarang, 
               MBarangD.Barcode, 
               UserEntry.Nama UserEntry, 
               UserEdit.Nama UserEdit, 
               UserPosted.Nama UserPosted, 
               MGudang.Kode Gudang
        FROM MSaldoAwalPersediaan(NOLOCK)
             LEFT JOIN MGudang(NOLOCK) ON MSaldoAwalPersediaan.IDGudang = MGudang.NoID
             LEFT JOIN MBarang(NOLOCK) ON MSaldoAwalPersediaan.IDBarang = MBarang.NoID
             LEFT JOIN MBarangD(NOLOCK) ON MSaldoAwalPersediaan.IDBarangD = MBarangD.NoID
             LEFT JOIN MUser UserEntry(NOLOCK) ON MSaldoAwalPersediaan.IDUserEntry = UserEntry.NoID
             LEFT JOIN MUser UserEdit(NOLOCK) ON MSaldoAwalPersediaan.IDUserEdit = UserEdit.NoID
             LEFT JOIN MUser UserPosted(NOLOCK) ON MSaldoAwalPersediaan.IDUserPosted = UserPosted.NoID
        WHERE MSaldoAwalPersediaan.IsPosted = 1
              AND CONVERT(DATE, MSaldoAwalPersediaan.Tanggal) >= CONVERT(DATE, @TglDari)
              AND CONVERT(DATE, MSaldoAwalPersediaan.Tanggal) <= CONVERT(DATE, @TglSampai)
        ORDER BY MSaldoAwalPersediaan.Tanggal, 
                 MSaldoAwalPersediaan.NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spFakturMSaldoAwalPiutang]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Author: <Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spFakturMSaldoAwalPiutang]
(@TglDari   DATETIME = '2019-01-01', 
 @TglSampai DATETIME = '2019-01-31'
)
AS
    BEGIN
        SET NOCOUNT ON;
        SELECT MSaldoAwalPiutang.*, 
               MAlamat.Kode KodeCustomer, 
               MAlamat.Nama NamaCustomer, 
               UserEntry.Nama UserEntry, 
               UserEdit.Nama UserEdit, 
               UserPosted.Nama UserPosted
        FROM MSaldoAwalPiutang(NOLOCK)
             LEFT JOIN MAlamat(NOLOCK) ON MSaldoAwalPiutang.IDCustomer = MAlamat.NoID
             LEFT JOIN MUser UserEntry(NOLOCK) ON MSaldoAwalPiutang.IDUserEntry = UserEntry.NoID
             LEFT JOIN MUser UserEdit(NOLOCK) ON MSaldoAwalPiutang.IDUserEdit = UserEdit.NoID
             LEFT JOIN MUser UserPosted(NOLOCK) ON MSaldoAwalPiutang.IDUserPosted = UserPosted.NoID
        WHERE MSaldoAwalPiutang.IsPosted = 1
              AND CONVERT(DATE, MSaldoAwalPiutang.Tanggal) >= CONVERT(DATE, @TglDari)
              AND CONVERT(DATE, MSaldoAwalPiutang.Tanggal) <= CONVERT(DATE, @TglSampai)
        ORDER BY MSaldoAwalPiutang.Tanggal, 
                 MSaldoAwalPiutang.NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spFakturMStockOpname]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Author:  <Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spFakturMStockOpname](@NoID BIGINT = -1)
AS
    BEGIN
        SET NOCOUNT ON;
        SELECT MStockOpnameD.*, 
               MStockOpname.Kode, 
               UserEntry.Nama AS UserEntry, 
               UserEdit.Nama AS UserEdit, 
               UserPosted.Nama AS UserPosted, 
               MGudang.Kode AS Gudang, 
               MStockOpname.NoReff, 
               MStockOpname.Tanggal, 
               MStockOpname.Catatan, 
               MStockOpname.Total, 
               MStockOpname.IsPosted, 
               MStockOpname.TglPosted, 
               MStockOpnameD.IDHeader, 
               MBarang.Kode AS KodeBarang, 
               MBarang.Nama AS NamaBarang, 
               MBarangD.Barcode, 
               MSatuan.Kode AS Satuan
        FROM MStockOpname
             INNER JOIN MStockOpnameD ON MStockOpnameD.IDHeader = MStockOpname.NoID
             LEFT JOIN MBarangD ON MStockOpnameD.IDBarangD = MBarangD.NoID
             LEFT JOIN MSatuan ON MStockOpnameD.IDSatuan = MSatuan.NoID
             LEFT JOIN MBarang ON MStockOpnameD.IDBarang = MBarang.NoID
             LEFT JOIN MGudang ON MGudang.NoID = MStockOpname.IDGudang
             LEFT JOIN MUser AS UserPosted ON UserPosted.NoID = MStockOpname.IDUserPosted
             LEFT JOIN MUser AS UserEntry ON MStockOpname.IDUserEntry = UserEntry.NoID
             LEFT JOIN MUser AS UserEdit ON MStockOpname.IDUserEdit = UserEdit.NoID
        WHERE MStockOpname.IsPosted = 1
              AND MStockOpname.NoID = @NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spGenerateMenu]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[spGenerateMenu](@IDRole INT = -1)
AS
     IF(@IDRole >= 1)
         BEGIN
             DELETE [dbo].[MRoleD]
             FROM [dbo].[MRoleD]
                  LEFT JOIN MMenu ON MMenu.NoID = MRoleD.IDMenu
             WHERE MRoleD.IDRole = @IDRole
                   AND MMenu.NoID IS NULL
                   OR ISNULL(MMenu.IsActive, 0) = 0;
             INSERT INTO [dbo].[MRoleD]
             ([IDRole], 
              [IDMenu], 
              [IsActive]
             )
                    SELECT @IDRole [IDRole], 
                           MMenu.NoID [IDMenu], 
                           MMenu.[IsActive]
                    FROM MMenu
                    WHERE IsActive = 1
                          AND NoID NOT IN
                    (
                        SELECT MRoleD.IDMenu
                        FROM MRoleD
                        WHERE IDRole = @IDRole
                    );
         END;
         ELSE
         BEGIN
             DELETE [dbo].[MRoleD]
             FROM [dbo].[MRoleD]
                  LEFT JOIN MMenu ON MMenu.NoID = MRoleD.IDMenu
             WHERE MMenu.NoID IS NULL
                   OR ISNULL(MMenu.IsActive, 0) = 0;
             DECLARE @Temp AS TABLE
             (PKID   INT
              PRIMARY KEY IDENTITY(1, 1), 
              IDRole INT
             );
             INSERT INTO @Temp(IDRole)
                    SELECT NoID
                    FROM MRole;
             DECLARE @i AS INT, @iMax INT;
             SELECT @i = 1, 
                    @iMax = MAX(PKID)
             FROM @Temp;
             WHILE(@i <= @iMax)
                 BEGIN
                     SELECT @IDRole = T.IDRole
                     FROM @Temp T
                     WHERE T.PKID = @i;
                     INSERT INTO [dbo].[MRoleD]
                     ([IDRole], 
                      [IDMenu], 
                      [IsActive]
                     )
                            SELECT @IDRole [IDRole], 
                                   MMenu.NoID [IDMenu], 
                                   MMenu.[IsActive]
                            FROM MMenu
                            WHERE IsActive = 1
                                  AND NoID NOT IN
                            (
                                SELECT MRoleD.IDMenu
                                FROM MRoleD
                                WHERE IDRole = @IDRole
                            );
                     SET @i = @i + 1;
                 END;
         END;
GO

/****** Object:  StoredProcedure [dbo].[spHasilPosting]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spHasilPosting]
-- Add the parameters for the stored procedure here
@IDTransaksi AS      BIGINT = -1, 
@IDJenisTransaksi AS INT    = -1
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        -- Insert statements for procedure here
        DECLARE @Temp AS TABLE
        (IDJenisTransaksi INT
         PRIMARY KEY
        );
        IF(@IDJenisTransaksi = 4
           OR @IDJenisTransaksi = 5)
            BEGIN
                INSERT INTO @Temp(IDJenisTransaksi)
                       SELECT 4 IDJenis
                       UNION ALL
                       SELECT 5 IDJenis;
            END;
            ELSE
            BEGIN
                INSERT INTO @Temp(IDJenisTransaksi)
                       SELECT @IDJenisTransaksi IDJenis;
            END;
        SELECT MKartuStok.*, 
               MBarang.Kode KodeBarang, 
               MBarang.Nama AS NamaBarang, 
               MSatuan.Kode AS Satuan, 
               MGudang.Nama AS Gudang, 
               MKartuStok.QtyMasuk * MKartuStok.Konversi AS [Qty Masuk (Pcs)], 
               MKartuStok.QtyKeluar * MKartuStok.Konversi AS [Qty Keluar (Pcs)]
        FROM MKartuStok(NOLOCK)
             INNER JOIN @Temp T ON T.IDJenisTransaksi = MKartuStok.IDJenisTransaksi
             LEFT JOIN MBarang(NOLOCK) ON MBarang.NoID = MKartuStok.IDBarang
             LEFT JOIN MBarangD(NOLOCK) ON MBarangD.NoID = MKartuStok.IDBarangD
             LEFT JOIN MSatuan(NOLOCK) ON MSatuan.NoID = MKartuStok.IDSatuan
             LEFT JOIN MGudang(NOLOCK) ON MGudang.NoID = MKartuStok.IDGudang
        WHERE MKartuStok.IDTransaksi = @IDTransaksi;
        SELECT MHutangPiutang.*, 
               MAlamat.Kode KodeKontak, 
               MAlamat.Nama AS NamaKontak
        FROM MHutangPiutang(NOLOCK)
             INNER JOIN @Temp T ON T.IDJenisTransaksi = MHutangPiutang.IDJenisTransaksi
             LEFT JOIN MAlamat(NOLOCK) ON MAlamat.NoID = MHutangPiutang.IDAlamat
        WHERE MHutangPiutang.IDTransaksi = @IDTransaksi;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spLapKartuStok]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spLapKartuStok]
-- Add the parameters for the stored procedure here
@TglDari AS   DATE, 
@TglSampai AS DATE, 
@IDBarang AS  BIGINT, 
@IDGudang AS  VARCHAR(250)
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        -- Insert statements for procedure here
        DECLARE @Temp AS TABLE
        (PKID             INT IDENTITY(1, 1) PRIMARY KEY, 
         IDJenisTransaksi INT, 
         IDTransaksi      BIGINT, 
         IDKartuStok      INT, 
         SaldoAwal        FLOAT, 
         SaldoAkhir       FLOAT, 
         NilaiAwal        FLOAT, 
         NilaiAkhir       FLOAT
        );
        DECLARE @i AS INT, @iMax AS INT, @SaldoAwal FLOAT, @SaldoAkhir FLOAT, @NilaiAwal FLOAT, @NilaiAkhir FLOAT;
        DECLARE @QtyMasuk FLOAT, @QtyKeluar FLOAT, @Debet FLOAT, @Kredit FLOAT;
        INSERT INTO @Temp
        (IDJenisTransaksi, 
         IDTransaksi, 
         IDKartuStok, 
         SaldoAwal, 
         SaldoAkhir, 
         NilaiAwal, 
         NilaiAkhir
        )
               SELECT 1, 
                      -999, 
                      -999, 
                      SUM((QtyMasuk - QtyKeluar) * Konversi), 
                      SUM((QtyMasuk - QtyKeluar) * Konversi), 
                      SUM(Debet - Kredit), 
                      SUM(Debet - Kredit)
               FROM MKartuStok(NOLOCK)
               WHERE MKartuStok.IDBarang = @IDBarang
                     AND IDGudang IN
               (
                   SELECT *
                   FROM dbo.fnSplitString(REPLACE(@IDGudang, ' ', ''), ',')
               )
               AND CONVERT(DATE, MKartuStok.Tanggal) < CONVERT(DATE, @TglDari);
        INSERT INTO @Temp
        (IDJenisTransaksi, 
         IDTransaksi, 
         IDKartuStok, 
         SaldoAwal, 
         SaldoAkhir, 
         NilaiAwal, 
         NilaiAkhir
        )
               SELECT MKartuStok.IDJenisTransaksi, 
                      MKartuStok.IDTransaksi, 
                      MKartuStok.NoID, 
                      0, 
                      0, 
                      0, 
                      0
               FROM MKartuStok(NOLOCK)
               WHERE MKartuStok.IDBarang = @IDBarang
                     AND IDGudang IN
               (
                   SELECT *
                   FROM dbo.fnSplitString(REPLACE(@IDGudang, ' ', ''), ',')
               )
               AND CONVERT(DATE, MKartuStok.Tanggal) >= CONVERT(DATE, @TglDari)
               AND CONVERT(DATE, MKartuStok.Tanggal) <= CONVERT(DATE, @TglSampai)
               ORDER BY MKartuStok.Tanggal;
        SELECT @i = 1, 
               @iMax = MAX(PKID)
        FROM @Temp T;
        WHILE(@i <= @iMax)
            BEGIN
                SELECT @SaldoAwal = CASE
                                        WHEN @i = 1
                                        THEN ISNULL(T.SaldoAwal, 0)
                                        ELSE @SaldoAwal
                                    END,
                       --@SaldoAkhir = T.SaldoAkhir,
                       @NilaiAwal = CASE
                                        WHEN @i = 1
                                        THEN ISNULL(T.NilaiAwal, 0)
                                        ELSE @NilaiAwal
                                    END,
                       --@NilaiAkhir = T.NilaiAkhir,
                       @QtyMasuk = B.QtyMasuk * B.Konversi, 
                       @QtyKeluar = B.QtyKeluar * B.Konversi, 
                       @Debet = B.Debet, 
                       @Kredit = B.Kredit
                FROM @Temp T
                     LEFT JOIN MKartuStok(NOLOCK) B ON T.IDKartuStok = B.NoID
                WHERE T.PKID = @i;
                SET @SaldoAkhir = ISNULL(@SaldoAwal, 0) + ISNULL(@QtyMasuk, 0) - ISNULL(@QtyKeluar, 0);
                SET @NilaiAkhir = ISNULL(@NilaiAwal, 0) + ISNULL(@Debet, 0) - ISNULL(@Kredit, 0);
                UPDATE T
                  SET 
                      SaldoAkhir = @SaldoAkhir, 
                      NilaiAkhir = @NilaiAkhir, 
                      SaldoAwal = @SaldoAwal, 
                      NilaiAwal = @NilaiAwal
                FROM @Temp T
                WHERE T.PKID = @i;
                SET @SaldoAwal = @SaldoAkhir;
                SET @NilaiAwal = @NilaiAkhir;
                SET @i = @i + 1;
            END;
        SELECT K.[NoID], 
               K.[IDBarang], 
               K.[IDBarangD], 
               K.[Varian], 
               K.[IDJenisTransaksi], 
               K.[IDTransaksi], 
               K.[IDTransaksiD], 
               K.[IDGudang], 
               K.[Tanggal], 
               T.SaldoAwal, 
               K.[QtyMasuk] * K.Konversi AS QtyMasuk, 
               K.[QtyKeluar] * K.Konversi AS QtyKeluar, 
               T.SaldoAkhir, 
               T.NilaiAwal, 
               K.[Debet], 
               K.[Kredit], 
               T.NilaiAkhir, 
               K.[HargaBeli], 
               K.[HPP], 
               B.Barcode, 
               A.Kode AS KodeBarang, 
               A.Nama AS NamaBarang, 
               C.Kode AS Satuan, 
               D.Nama AS JenisTransaksi, 
               E.Nama AS Gudang
        FROM @Temp T
             LEFT JOIN MKartuStok K(NOLOCK) ON K.NoID = T.IDKartuStok
             LEFT JOIN MBarang A(NOLOCK) ON A.NoID = K.IDBarang
             LEFT JOIN MBarangD B(NOLOCK) ON B.NoID = K.IDBarangD
             LEFT JOIN MSatuan C(NOLOCK) ON C.NoID = K.IDSatuan
             LEFT JOIN MJenisTransaksi D(NOLOCK) ON D.ID = T.IDJenisTransaksi
             LEFT JOIN MGudang E(NOLOCK) ON E.NoID = K.IDGudang
        ORDER BY K.Tanggal;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spLogPerubahanHarga]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spLogPerubahanHarga] 
-- Add the parameters for the stored procedure here
@IDBarang AS BIGINT = -1
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        -- Insert statements for procedure here
        SELECT TLogPerubahanHargaJual.*, 
               MBarang.Kode KodeBarang, 
               MBarang.Nama AS NamaBarang, 
               MUser.Nama AS UserEdit, 
               MBarangD.Barcode, 
               MSatuan.Kode Satuan
        FROM TLogPerubahanHargaJual(NOLOCK)
             LEFT JOIN MBarangD(NOLOCK) ON MBarangD.NoID = TLogPerubahanHargaJual.IDBarangD
             LEFT JOIN MBarang(NOLOCK) ON MBarang.NoID = MBarangD.IDBarang
             LEFT JOIN MUser(NOLOCK) ON MUser.NoID = TLogPerubahanHargaJual.IDUser
             LEFT JOIN MSatuan(NOLOCK) ON MSatuan.NoID = MBarangD.IDSatuan
        WHERE MBarangD.IDBarang = @IDBarang
        ORDER BY TLogPerubahanHargaJual.Tanggal DESC;
        SELECT TLogPerubahanHargaBeli.*, 
               MBarang.Kode KodeBarang, 
               MBarang.Nama AS NamaBarang, 
               MUser.Nama AS UserEdit
        FROM TLogPerubahanHargaBeli(NOLOCK)
             LEFT JOIN MBarang(NOLOCK) ON MBarang.NoID = TLogPerubahanHargaBeli.IDBarang
             LEFT JOIN MUser(NOLOCK) ON MUser.NoID = TLogPerubahanHargaBeli.IDUser
        WHERE TLogPerubahanHargaBeli.IDBarang = @IDBarang
        ORDER BY TLogPerubahanHargaBeli.Tanggal DESC;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spSimpanMBeli]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[spSimpanMBeli]
(@NoID           BIGINT, 
 @Kode           VARCHAR(255), 
 @NoReff         VARCHAR(255), 
 @IDSupplier     INT, 
 @Tanggal        DATETIME, 
 @JatuhTempo     DATE, 
 @Catatan        VARCHAR(MAX), 
 @IDTypePajak    SMALLINT, 
 @Subtotal       MONEY, 
 @DiscNotaProsen NUMERIC(18, 2), 
 @DiscNotaRp     MONEY, 
 @TotalBruto     MONEY, 
 @DPP            MONEY, 
 @PPN            MONEY, 
 @Total          MONEY, 
 @IsPosted       BIT, 
 @TglPosted      DATETIME, 
 @IDUserPosted   INT, 
 @IDUserEntry    INT, 
 @IDUserEdit     INT, 
 @IDPO           INT, 
 @IDGudang       INT
)
AS
    BEGIN
        DECLARE @NoUrut AS INT= 0;
        IF(@NoID <= 0)
            BEGIN
                SELECT @NoID = MAX(NoID)
                FROM MBeli;
                SET @NoID = ISNULL(@NoID, 0) + 1;
                SET @Kode = 'BL/' + SUBSTRING(CONVERT(VARCHAR(100), @Tanggal, 112), 3, 4) + '/';
                SELECT @NoUrut = MAX(CASE
                                         WHEN ISNUMERIC(RIGHT(Kode, 5)) = 1
                                         THEN RIGHT(Kode, 5)
                                         ELSE 0
                                     END)
                FROM MBeli
                WHERE LEFT(Kode, LEN(@Kode)) = @Kode;
                SET @NoUrut = ISNULL(@NoUrut, 0) + 1;
                SET @Kode = @Kode + LEFT('00000', 5 - LEN(CONVERT(VARCHAR(10), @NoUrut))) + CONVERT(VARCHAR(10), @NoUrut);
                INSERT INTO [MBeli]
                (NoID, 
                 Kode, 
                 NoReff, 
                 IDSupplier, 
                 Tanggal, 
                 JatuhTempo, 
                 Catatan, 
                 IDTypePajak, 
                 Subtotal, 
                 DiscNotaProsen, 
                 DiscNotaRp, 
                 TotalBruto, 
                 DPP, 
                 PPN, 
                 Total, 
                 IsPosted, 
                 TglPosted, 
                 IDUserPosted, 
                 IDUserEntry, 
                 IDUserEdit, 
                 IDPO, 
                 IDGudang
                )
                       SELECT @NoID, 
                              @Kode, 
                              @NoReff, 
                              @IDSupplier, 
                              @Tanggal, 
                              @JatuhTempo, 
                              @Catatan, 
                              @IDTypePajak, 
                              @Subtotal, 
                              @DiscNotaProsen, 
                              @DiscNotaRp, 
                              @TotalBruto, 
                              @DPP, 
                              @PPN, 
                              @Total, 
                              @IsPosted, 
                              @TglPosted, 
                              @IDUserPosted, 
                              @IDUserEntry, 
                              @IDUserEdit, 
                              @IDPO, 
                              @IDGudang;
            END;
            ELSE
            BEGIN
                UPDATE [MBeli]
                  SET 
                      NoID = @NoID, 
                      Kode = @Kode, 
                      NoReff = @NoReff, 
                      IDSupplier = @IDSupplier, 
                      Tanggal = @Tanggal, 
                      JatuhTempo = @JatuhTempo, 
                      Catatan = @Catatan, 
                      IDTypePajak = @IDTypePajak, 
                      Subtotal = @Subtotal, 
                      DiscNotaProsen = @DiscNotaProsen, 
                      DiscNotaRp = @DiscNotaRp, 
                      TotalBruto = @TotalBruto, 
                      DPP = @DPP, 
                      PPN = @PPN, 
                      Total = @Total, 
                      IsPosted = @IsPosted, 
                      TglPosted = @TglPosted, 
                      IDUserPosted = @IDUserPosted, 
                      IDUserEntry = @IDUserEntry, 
                      IDUserEdit = @IDUserEdit, 
                      IDPO = @IDPO, 
                      IDGudang = @IDGudang
                WHERE NoID = @NoID;
            END;
        SELECT @NoID AS NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spSimpanMJual]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[spSimpanMJual]
(@NoID           BIGINT, 
 @Kode           VARCHAR(255), 
 @NoReff         VARCHAR(255), 
 @IDCustomer     INT, 
 @Tanggal        DATETIME, 
 @JatuhTempo     DATE, 
 @Catatan        VARCHAR(MAX), 
 @IDTypePajak    SMALLINT, 
 @Subtotal       MONEY, 
 @DiscNotaProsen NUMERIC(18, 2), 
 @DiscNotaRp     MONEY, 
 @TotalBruto     MONEY, 
 @DPP            MONEY, 
 @PPN            MONEY, 
 @Total          MONEY, 
 @Sisa           MONEY, 
 @IsPosted       BIT, 
 @TglPosted      DATETIME, 
 @IDUserPosted   INT, 
 @IDUserEntry    INT, 
 @IDUserEdit     INT, 
 @IDGudang       INT
)
AS
    BEGIN
        DECLARE @NoUrut AS INT= 0;
        IF(@NoID <= 0)
            BEGIN
                SELECT @NoID = MAX(NoID)
                FROM MJual;
                SET @NoID = ISNULL(@NoID, 0) + 1;
                SET @Kode = 'JL/' + SUBSTRING(CONVERT(VARCHAR(100), @Tanggal, 112), 3, 4) + '/';
                SELECT @NoUrut = MAX(CASE
                                         WHEN ISNUMERIC(RIGHT(Kode, 5)) = 1
                                         THEN RIGHT(Kode, 5)
                                         ELSE 0
                                     END)
                FROM MJual
                WHERE LEFT(Kode, LEN(@Kode)) = @Kode;
                SET @NoUrut = ISNULL(@NoUrut, 0) + 1;
                SET @Kode = @Kode + LEFT('00000', 5 - LEN(CONVERT(VARCHAR(10), @NoUrut))) + CONVERT(VARCHAR(10), @NoUrut);
                INSERT INTO [MJual]
                (NoID, 
                 Kode, 
                 NoReff, 
                 IDCustomer, 
                 Tanggal, 
                 JatuhTempo, 
                 Catatan, 
                 IDTypePajak, 
                 Subtotal, 
                 DiscNotaProsen, 
                 DiscNotaRp, 
                 TotalBruto, 
                 DPP, 
                 PPN, 
                 Total, 
                 Bayar, 
                 Sisa, 
                 IsPosted, 
                 TglPosted, 
                 IDUserPosted, 
                 IDUserEntry, 
                 IDUserEdit, 
                 IDGudang
                )
                       SELECT @NoID, 
                              @Kode, 
                              @NoReff, 
                              @IDCustomer, 
                              @Tanggal, 
                              @JatuhTempo, 
                              @Catatan, 
                              @IDTypePajak, 
                              @Subtotal, 
                              @DiscNotaProsen, 
                              @DiscNotaRp, 
                              @TotalBruto, 
                              @DPP, 
                              @PPN, 
                              @Total, 
                              @Total - @Sisa, 
                              @Sisa, 
                              @IsPosted, 
                              @TglPosted, 
                              @IDUserPosted, 
                              @IDUserEntry, 
                              @IDUserEdit, 
                              @IDGudang;
            END;
            ELSE
            BEGIN
                UPDATE [MJual]
                  SET 
                      NoID = @NoID, 
                      Kode = @Kode, 
                      NoReff = @NoReff, 
                      IDCustomer = @IDCustomer, 
                      Tanggal = @Tanggal, 
                      JatuhTempo = @JatuhTempo, 
                      Catatan = @Catatan, 
                      IDTypePajak = @IDTypePajak, 
                      Subtotal = @Subtotal, 
                      DiscNotaProsen = @DiscNotaProsen, 
                      DiscNotaRp = @DiscNotaRp, 
                      TotalBruto = @TotalBruto, 
                      DPP = @DPP, 
                      PPN = @PPN, 
                      Total = @Total, 
                      Bayar = @Total - @Sisa, 
                      Sisa = @Sisa, 
                      IsPosted = @IsPosted, 
                      TglPosted = @TglPosted, 
                      IDUserPosted = @IDUserPosted, 
                      IDUserEntry = @IDUserEntry, 
                      IDUserEdit = @IDUserEdit, 
                      IDGudang = @IDGudang
                WHERE NoID = @NoID;
            END;
        SELECT @NoID AS NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spSimpanMMutasiGudang]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[spSimpanMMutasiGudang]
(@NoID           BIGINT, 
 @Kode           VARCHAR(255), 
 @NoReff         VARCHAR(255), 
 @Tanggal        DATETIME, 
 @Catatan        VARCHAR(MAX), 
 @Total          MONEY, 
 @IsPosted       BIT, 
 @TglPosted      DATETIME, 
 @IDUserPosted   INT, 
 @IDUserEntry    INT, 
 @IDUserEdit     INT, 
 @IDGudangAsal   INT, 
 @IDGudangTujuan INT
)
AS
    BEGIN
        DECLARE @NoUrut AS INT= 0;
        IF(@NoID <= 0)
            BEGIN
                SELECT @NoID = MAX(NoID)
                FROM MMutasiGudang;
                SET @NoID = ISNULL(@NoID, 0) + 1;
                SET @Kode = 'MTS/' + SUBSTRING(CONVERT(VARCHAR(100), @Tanggal, 112), 3, 4) + '/';
                SELECT @NoUrut = MAX(CASE
                                         WHEN ISNUMERIC(RIGHT(Kode, 5)) = 1
                                         THEN RIGHT(Kode, 5)
                                         ELSE 0
                                     END)
                FROM MMutasiGudang
                WHERE LEFT(Kode, LEN(@Kode)) = @Kode;
                SET @NoUrut = ISNULL(@NoUrut, 0) + 1;
                SET @Kode = @Kode + LEFT('00000', 5 - LEN(CONVERT(VARCHAR(10), @NoUrut))) + CONVERT(VARCHAR(10), @NoUrut);
                INSERT INTO [MMutasiGudang]
                (NoID, 
                 Kode, 
                 NoReff, 
                 Tanggal, 
                 Catatan, 
                 Total, 
                 IsPosted, 
                 TglPosted, 
                 IDUserPosted, 
                 IDUserEntry, 
                 IDUserEdit, 
                 IDGudangAsal, 
                 IDGudangTujuan
                )
                       SELECT @NoID, 
                              @Kode, 
                              @NoReff, 
                              @Tanggal, 
                              @Catatan, 
                              @Total, 
                              @IsPosted, 
                              @TglPosted, 
                              @IDUserPosted, 
                              @IDUserEntry, 
                              @IDUserEdit, 
                              @IDGudangAsal, 
                              @IDGudangTujuan;
            END;
            ELSE
            BEGIN
                UPDATE [MMutasiGudang]
                  SET 
                      NoID = @NoID, 
                      Kode = @Kode, 
                      NoReff = @NoReff, 
                      Tanggal = @Tanggal, 
                      Catatan = @Catatan, 
                      Total = @Total, 
                      IsPosted = @IsPosted, 
                      TglPosted = @TglPosted, 
                      IDUserPosted = @IDUserPosted, 
                      IDUserEntry = @IDUserEntry, 
                      IDUserEdit = @IDUserEdit, 
                      IDGudangAsal = @IDGudangAsal, 
                      IDGudangTujuan = @IDGudangTujuan
                WHERE NoID = @NoID;
            END;
        SELECT @NoID AS NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spSimpanMPemakaian]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[spSimpanMPemakaian]
(@NoID         BIGINT, 
 @Kode         VARCHAR(255), 
 @NoReff       VARCHAR(255), 
 @Tanggal      DATETIME, 
 @Catatan      VARCHAR(MAX), 
 @Total        MONEY, 
 @IsPosted     BIT, 
 @TglPosted    DATETIME, 
 @IDUserPosted INT, 
 @IDUserEntry  INT, 
 @IDUserEdit   INT, 
 @IDGudang     INT
)
AS
    BEGIN
        DECLARE @NoUrut AS INT= 0;
        IF(@NoID <= 0)
            BEGIN
                SELECT @NoID = MAX(NoID)
                FROM MPemakaian;
                SET @NoID = ISNULL(@NoID, 0) + 1;
                SET @Kode = 'SBK/' + SUBSTRING(CONVERT(VARCHAR(100), @Tanggal, 112), 3, 4) + '/';
                SELECT @NoUrut = MAX(CASE
                                         WHEN ISNUMERIC(RIGHT(Kode, 5)) = 1
                                         THEN RIGHT(Kode, 5)
                                         ELSE 0
                                     END)
                FROM MPemakaian
                WHERE LEFT(Kode, LEN(@Kode)) = @Kode;
                SET @NoUrut = ISNULL(@NoUrut, 0) + 1;
                SET @Kode = @Kode + LEFT('00000', 5 - LEN(CONVERT(VARCHAR(10), @NoUrut))) + CONVERT(VARCHAR(10), @NoUrut);
                INSERT INTO [MPemakaian]
                (NoID, 
                 Kode, 
                 NoReff, 
                 Tanggal, 
                 Catatan, 
                 Total, 
                 IsPosted, 
                 TglPosted, 
                 IDUserPosted, 
                 IDUserEntry, 
                 IDUserEdit, 
                 IDGudang
                )
                       SELECT @NoID, 
                              @Kode, 
                              @NoReff, 
                              @Tanggal, 
                              @Catatan, 
                              @Total, 
                              @IsPosted, 
                              @TglPosted, 
                              @IDUserPosted, 
                              @IDUserEntry, 
                              @IDUserEdit, 
                              @IDGudang;
            END;
            ELSE
            BEGIN
                UPDATE [MPemakaian]
                  SET 
                      NoID = @NoID, 
                      Kode = @Kode, 
                      NoReff = @NoReff, 
                      Tanggal = @Tanggal, 
                      Catatan = @Catatan, 
                      Total = @Total, 
                      IsPosted = @IsPosted, 
                      TglPosted = @TglPosted, 
                      IDUserPosted = @IDUserPosted, 
                      IDUserEntry = @IDUserEntry, 
                      IDUserEdit = @IDUserEdit, 
                      IDGudang = @IDGudang
                WHERE NoID = @NoID;
            END;
        SELECT @NoID AS NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spSimpanMPenyesuaian]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[spSimpanMPenyesuaian]
(@NoID         BIGINT, 
 @Kode         VARCHAR(255), 
 @NoReff       VARCHAR(255), 
 @Tanggal      DATETIME, 
 @Catatan      VARCHAR(MAX), 
 @Total        MONEY, 
 @IsPosted     BIT, 
 @TglPosted    DATETIME, 
 @IDUserPosted INT, 
 @IDUserEntry  INT, 
 @IDUserEdit   INT, 
 @IDGudang     INT
)
AS
    BEGIN
        DECLARE @NoUrut AS INT= 0;
        IF(@NoID <= 0)
            BEGIN
                SELECT @NoID = MAX(NoID)
                FROM MPenyesuaian;
                SET @NoID = ISNULL(@NoID, 0) + 1;
                SET @Kode = 'SBM/' + SUBSTRING(CONVERT(VARCHAR(100), @Tanggal, 112), 3, 4) + '/';
                SELECT @NoUrut = MAX(CASE
                                         WHEN ISNUMERIC(RIGHT(Kode, 5)) = 1
                                         THEN RIGHT(Kode, 5)
                                         ELSE 0
                                     END)
                FROM MPenyesuaian
                WHERE LEFT(Kode, LEN(@Kode)) = @Kode;
                SET @NoUrut = ISNULL(@NoUrut, 0) + 1;
                SET @Kode = @Kode + LEFT('00000', 5 - LEN(CONVERT(VARCHAR(10), @NoUrut))) + CONVERT(VARCHAR(10), @NoUrut);
                INSERT INTO [MPenyesuaian]
                (NoID, 
                 Kode, 
                 NoReff, 
                 Tanggal, 
                 Catatan, 
                 Total, 
                 IsPosted, 
                 TglPosted, 
                 IDUserPosted, 
                 IDUserEntry, 
                 IDUserEdit, 
                 IDGudang
                )
                       SELECT @NoID, 
                              @Kode, 
                              @NoReff, 
                              @Tanggal, 
                              @Catatan, 
                              @Total, 
                              @IsPosted, 
                              @TglPosted, 
                              @IDUserPosted, 
                              @IDUserEntry, 
                              @IDUserEdit, 
                              @IDGudang;
            END;
            ELSE
            BEGIN
                UPDATE [MPenyesuaian]
                  SET 
                      NoID = @NoID, 
                      Kode = @Kode, 
                      NoReff = @NoReff, 
                      Tanggal = @Tanggal, 
                      Catatan = @Catatan, 
                      Total = @Total, 
                      IsPosted = @IsPosted, 
                      TglPosted = @TglPosted, 
                      IDUserPosted = @IDUserPosted, 
                      IDUserEntry = @IDUserEntry, 
                      IDUserEdit = @IDUserEdit, 
                      IDGudang = @IDGudang
                WHERE NoID = @NoID;
            END;
        SELECT @NoID AS NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spSimpanMPO]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[spSimpanMPO]
(@NoID           BIGINT, 
 @Kode           VARCHAR(255), 
 @NoReff         VARCHAR(255), 
 @IDSupplier     INT, 
 @Tanggal        DATETIME, 
 @Expired        DATE, 
 @Catatan        VARCHAR(MAX), 
 @IDTypePajak    SMALLINT, 
 @Subtotal       MONEY, 
 @DiscNotaProsen NUMERIC(18, 2), 
 @DiscNotaRp     MONEY, 
 @TotalBruto     MONEY, 
 @DPP            MONEY, 
 @PPN            MONEY, 
 @Total          MONEY, 
 @IsPosted       BIT, 
 @TglPosted      DATETIME, 
 @IDUserPosted   INT, 
 @IDUserEntry    INT, 
 @IDUserEdit     INT, 
 @IsExpired      BIT
)
AS
    BEGIN
        DECLARE @NoUrut AS INT= 0;
        IF(@NoID <= 0)
            BEGIN
                SELECT @NoID = MAX(NoID)
                FROM MPO;
                SET @NoID = ISNULL(@NoID, 0) + 1;
                SET @Kode = 'PO/' + SUBSTRING(CONVERT(VARCHAR(100), @Tanggal, 112), 3, 4) + '/';
                SELECT @NoUrut = MAX(CASE
                                         WHEN ISNUMERIC(RIGHT(Kode, 5)) = 1
                                         THEN RIGHT(Kode, 5)
                                         ELSE 0
                                     END)
                FROM MPO
                WHERE LEFT(Kode, LEN(@Kode)) = @Kode;
                SET @NoUrut = ISNULL(@NoUrut, 0) + 1;
                SET @Kode = @Kode + LEFT('00000', 5 - LEN(CONVERT(VARCHAR(10), @NoUrut))) + CONVERT(VARCHAR(10), @NoUrut);
                INSERT INTO [MPO]
                (NoID, 
                 Kode, 
                 NoReff, 
                 IDSupplier, 
                 Tanggal, 
                 Expired, 
                 Catatan, 
                 IDTypePajak, 
                 Subtotal, 
                 DiscNotaProsen, 
                 DiscNotaRp, 
                 TotalBruto, 
                 DPP, 
                 PPN, 
                 Total, 
                 IsPosted, 
                 TglPosted, 
                 IDUserPosted, 
                 IDUserEntry, 
                 IDUserEdit, 
                 IsExpired
                )
                       SELECT @NoID, 
                              @Kode, 
                              @NoReff, 
                              @IDSupplier, 
                              @Tanggal, 
                              @Expired, 
                              @Catatan, 
                              @IDTypePajak, 
                              @Subtotal, 
                              @DiscNotaProsen, 
                              @DiscNotaRp, 
                              @TotalBruto, 
                              @DPP, 
                              @PPN, 
                              @Total, 
                              @IsPosted, 
                              @TglPosted, 
                              @IDUserPosted, 
                              @IDUserEntry, 
                              @IDUserEdit, 
                              @IsExpired;
            END;
            ELSE
            BEGIN
                UPDATE [MPO]
                  SET 
                      NoID = @NoID, 
                      Kode = @Kode, 
                      NoReff = @NoReff, 
                      IDSupplier = @IDSupplier, 
                      Tanggal = @Tanggal, 
                      Expired = @Expired, 
                      Catatan = @Catatan, 
                      IDTypePajak = @IDTypePajak, 
                      Subtotal = @Subtotal, 
                      DiscNotaProsen = @DiscNotaProsen, 
                      DiscNotaRp = @DiscNotaRp, 
                      TotalBruto = @TotalBruto, 
                      DPP = @DPP, 
                      PPN = @PPN, 
                      Total = @Total, 
                      IsPosted = @IsPosted, 
                      TglPosted = @TglPosted, 
                      IDUserPosted = @IDUserPosted, 
                      IDUserEntry = @IDUserEntry, 
                      IDUserEdit = @IDUserEdit, 
                      IsExpired = @IsExpired
                WHERE NoID = @NoID;
            END;
        SELECT @NoID AS NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spSimpanMReturBeli]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[spSimpanMReturBeli]
(@NoID           BIGINT, 
 @Kode           VARCHAR(255), 
 @NoReff         VARCHAR(255), 
 @IDSupplier     INT, 
 @Tanggal        DATETIME, 
 @JatuhTempo     DATE, 
 @Catatan        VARCHAR(MAX), 
 @IDTypePajak    SMALLINT, 
 @Subtotal       MONEY, 
 @DiscNotaProsen NUMERIC(18, 2), 
 @DiscNotaRp     MONEY, 
 @TotalBruto     MONEY, 
 @DPP            MONEY, 
 @PPN            MONEY, 
 @Total          MONEY, 
 @IsPosted       BIT, 
 @TglPosted      DATETIME, 
 @IDUserPosted   INT, 
 @IDUserEntry    INT, 
 @IDUserEdit     INT, 
 @IDGudang       INT
)
AS
    BEGIN
        DECLARE @NoUrut AS INT= 0;
        IF(@NoID <= 0)
            BEGIN
                SELECT @NoID = MAX(NoID)
                FROM MReturBeli;
                SET @NoID = ISNULL(@NoID, 0) + 1;
                SET @Kode = 'RBL/' + SUBSTRING(CONVERT(VARCHAR(100), @Tanggal, 112), 3, 4) + '/';
                SELECT @NoUrut = MAX(CASE
                                         WHEN ISNUMERIC(RIGHT(Kode, 5)) = 1
                                         THEN RIGHT(Kode, 5)
                                         ELSE 0
                                     END)
                FROM MReturBeli
                WHERE LEFT(Kode, LEN(@Kode)) = @Kode;
                SET @NoUrut = ISNULL(@NoUrut, 0) + 1;
                SET @Kode = @Kode + LEFT('00000', 5 - LEN(CONVERT(VARCHAR(10), @NoUrut))) + CONVERT(VARCHAR(10), @NoUrut);
                INSERT INTO [MReturBeli]
                (NoID, 
                 Kode, 
                 NoReff, 
                 IDSupplier, 
                 Tanggal, 
                 JatuhTempo, 
                 Catatan, 
                 IDTypePajak, 
                 Subtotal, 
                 DiscNotaProsen, 
                 DiscNotaRp, 
                 TotalBruto, 
                 DPP, 
                 PPN, 
                 Total, 
                 IsPosted, 
                 TglPosted, 
                 IDUserPosted, 
                 IDUserEntry, 
                 IDUserEdit, 
                 IDGudang
                )
                       SELECT @NoID, 
                              @Kode, 
                              @NoReff, 
                              @IDSupplier, 
                              @Tanggal, 
                              @JatuhTempo, 
                              @Catatan, 
                              @IDTypePajak, 
                              @Subtotal, 
                              @DiscNotaProsen, 
                              @DiscNotaRp, 
                              @TotalBruto, 
                              @DPP, 
                              @PPN, 
                              @Total, 
                              @IsPosted, 
                              @TglPosted, 
                              @IDUserPosted, 
                              @IDUserEntry, 
                              @IDUserEdit, 
                              @IDGudang;
            END;
            ELSE
            BEGIN
                UPDATE [MReturBeli]
                  SET 
                      NoID = @NoID, 
                      Kode = @Kode, 
                      NoReff = @NoReff, 
                      IDSupplier = @IDSupplier, 
                      Tanggal = @Tanggal, 
                      JatuhTempo = @JatuhTempo, 
                      Catatan = @Catatan, 
                      IDTypePajak = @IDTypePajak, 
                      Subtotal = @Subtotal, 
                      DiscNotaProsen = @DiscNotaProsen, 
                      DiscNotaRp = @DiscNotaRp, 
                      TotalBruto = @TotalBruto, 
                      DPP = @DPP, 
                      PPN = @PPN, 
                      Total = @Total, 
                      IsPosted = @IsPosted, 
                      TglPosted = @TglPosted, 
                      IDUserPosted = @IDUserPosted, 
                      IDUserEntry = @IDUserEntry, 
                      IDUserEdit = @IDUserEdit, 
                      IDGudang = @IDGudang
                WHERE NoID = @NoID;
            END;
        SELECT @NoID AS NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spSimpanMReturJual]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[spSimpanMReturJual]
(@NoID           BIGINT, 
 @Kode           VARCHAR(255), 
 @NoReff         VARCHAR(255), 
 @IDCustomer     INT, 
 @Tanggal        DATETIME, 
 @JatuhTempo     DATE, 
 @Catatan        VARCHAR(MAX), 
 @IDTypePajak    SMALLINT, 
 @Subtotal       MONEY, 
 @DiscNotaProsen NUMERIC(18, 2), 
 @DiscNotaRp     MONEY, 
 @TotalBruto     MONEY, 
 @DPP            MONEY, 
 @PPN            MONEY, 
 @Total          MONEY, 
 @Sisa           MONEY, 
 @IsPosted       BIT, 
 @TglPosted      DATETIME, 
 @IDUserPosted   INT, 
 @IDUserEntry    INT, 
 @IDUserEdit     INT, 
 @IDGudang       INT
)
AS
    BEGIN
        DECLARE @NoUrut AS INT= 0;
        IF(@NoID <= 0)
            BEGIN
                SELECT @NoID = MAX(NoID)
                FROM MReturJual;
                SET @NoID = ISNULL(@NoID, 0) + 1;
                SET @Kode = 'RJL/' + SUBSTRING(CONVERT(VARCHAR(100), @Tanggal, 112), 3, 4) + '/';
                SELECT @NoUrut = MAX(CASE
                                         WHEN ISNUMERIC(RIGHT(Kode, 5)) = 1
                                         THEN RIGHT(Kode, 5)
                                         ELSE 0
                                     END)
                FROM MReturJual
                WHERE LEFT(Kode, LEN(@Kode)) = @Kode;
                SET @NoUrut = ISNULL(@NoUrut, 0) + 1;
                SET @Kode = @Kode + LEFT('00000', 5 - LEN(CONVERT(VARCHAR(10), @NoUrut))) + CONVERT(VARCHAR(10), @NoUrut);
                INSERT INTO [MReturJual]
                (NoID, 
                 Kode, 
                 NoReff, 
                 IDCustomer, 
                 Tanggal, 
                 JatuhTempo, 
                 Catatan, 
                 IDTypePajak, 
                 Subtotal, 
                 DiscNotaProsen, 
                 DiscNotaRp, 
                 TotalBruto, 
                 DPP, 
                 PPN, 
                 Total, 
                 Bayar, 
                 Sisa, 
                 IsPosted, 
                 TglPosted, 
                 IDUserPosted, 
                 IDUserEntry, 
                 IDUserEdit, 
                 IDGudang
                )
                       SELECT @NoID, 
                              @Kode, 
                              @NoReff, 
                              @IDCustomer, 
                              @Tanggal, 
                              @JatuhTempo, 
                              @Catatan, 
                              @IDTypePajak, 
                              @Subtotal, 
                              @DiscNotaProsen, 
                              @DiscNotaRp, 
                              @TotalBruto, 
                              @DPP, 
                              @PPN, 
                              @Total, 
                              @Total - @Sisa, 
                              @Sisa, 
                              @IsPosted, 
                              @TglPosted, 
                              @IDUserPosted, 
                              @IDUserEntry, 
                              @IDUserEdit, 
                              @IDGudang;
            END;
            ELSE
            BEGIN
                UPDATE [MReturJual]
                  SET 
                      NoID = @NoID, 
                      Kode = @Kode, 
                      NoReff = @NoReff, 
                      IDCustomer = @IDCustomer, 
                      Tanggal = @Tanggal, 
                      JatuhTempo = @JatuhTempo, 
                      Catatan = @Catatan, 
                      IDTypePajak = @IDTypePajak, 
                      Subtotal = @Subtotal, 
                      DiscNotaProsen = @DiscNotaProsen, 
                      DiscNotaRp = @DiscNotaRp, 
                      TotalBruto = @TotalBruto, 
                      DPP = @DPP, 
                      PPN = @PPN, 
                      Total = @Total, 
                      Bayar = @Total - @Sisa, 
                      Sisa = @Sisa, 
                      IsPosted = @IsPosted, 
                      TglPosted = @TglPosted, 
                      IDUserPosted = @IDUserPosted, 
                      IDUserEntry = @IDUserEntry, 
                      IDUserEdit = @IDUserEdit, 
                      IDGudang = @IDGudang
                WHERE NoID = @NoID;
            END;
        SELECT @NoID AS NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spSimpanMSaldoAwalHutang]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[spSimpanMSaldoAwalHutang]
(@NoID         BIGINT, 
 @Kode         VARCHAR(255), 
 @KodeReff     VARCHAR(255), 
 @Tanggal      DATETIME, 
 @JatuhTempo   DATETIME, 
 @Jumlah       MONEY, 
 @Keterangan   VARCHAR(MAX), 
 @IDSupplier   INT, 
 @IsPosted     BIT, 
 @TglPosted    DATETIME, 
 @IDUserPosted INT, 
 @IDUserEntry  INT, 
 @IDUserEdit   INT
)
AS
    BEGIN
        DECLARE @NoUrut AS INT= 0;
        IF(@NoID <= 0)
            BEGIN
                SELECT @NoID = MAX(NoID)
                FROM MSaldoAwalHutang;
                SET @NoID = ISNULL(@NoID, 0) + 1;
                SET @Kode = 'SAH/' + SUBSTRING(CONVERT(VARCHAR(100), @Tanggal, 112), 3, 4) + '/';
                SELECT @NoUrut = MAX(CASE
                                         WHEN ISNUMERIC(RIGHT(Kode, 5)) = 1
                                         THEN RIGHT(Kode, 5)
                                         ELSE 0
                                     END)
                FROM MSaldoAwalHutang
                WHERE LEFT(Kode, LEN(@Kode)) = @Kode;
                SET @NoUrut = ISNULL(@NoUrut, 0) + 1;
                SET @Kode = @Kode + LEFT('00000', 5 - LEN(CONVERT(VARCHAR(10), @NoUrut))) + CONVERT(VARCHAR(10), @NoUrut);
                INSERT INTO [dbo].[MSaldoAwalHutang]
                ([NoID], 
                 [IDSupplier], 
                 [Kode], 
                 [Tanggal], 
                 [JatuhTempo], 
                 [KodeReff], 
                 [Jumlah], 
                 [IDUserEntry], 
                 [TglEntry], 
                 [IDUserEdit], 
                 [TglEdit], 
                 [IsPosted], 
                 [IDUserPosted], 
                 [TglPosted], 
                 [Keterangan]
                )
                       SELECT @NoID, 
                              @IDSupplier, 
                              @Kode, 
                              @Tanggal, 
                              @JatuhTempo, 
                              @KodeReff, 
                              ROUND(@Jumlah, 2), 
                              @IDUserEntry, 
                              GETDATE(), 
                              NULL, 
                              NULL, 
                              0, 
                              NULL, 
                              NULL, 
                              @Keterangan;
            END;
            ELSE
            BEGIN
                UPDATE [dbo].[MSaldoAwalHutang]
                  SET 
                      [IDSupplier] = @IDSupplier, 
                      [Kode] = @Kode, 
                      [KodeReff] = @KodeReff, 
                      [Tanggal] = @Tanggal, 
                      [JatuhTempo] = @JatuhTempo, 
                      [Jumlah] = ROUND(@Jumlah, 2), 
                      [IDUserEdit] = @IDUserEdit, 
                      [TglEdit] = GETDATE(), 
                      [IsPosted] = @IsPosted, 
                      [IDUserPosted] = CASE
                                           WHEN ISNULL(@IsPosted, 0) = 1
                                           THEN @IDUserPosted
                                           ELSE NULL
                                       END, 
                      [TglPosted] = CASE
                                        WHEN ISNULL(@IsPosted, 0) = 1
                                        THEN GETDATE()
                                        ELSE NULL
                                    END, 
                      [Keterangan] = @Keterangan
                WHERE [NoID] = @NoID;
            END;
        SELECT @NoID AS NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spSimpanMSaldoAwalPersediaan]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[spSimpanMSaldoAwalPersediaan]
(@NoID         BIGINT, 
 @Kode         VARCHAR(255), 
 @Tanggal      DATETIME, 
 @Keterangan   VARCHAR(MAX), 
 @IDGudang     INT, 
 @IDBarangD    INT, 
 @IDBarang     INT, 
 @IDSatuan     INT, 
 @Qty          NUMERIC(18, 3), 
 @Konversi     INT, 
 @HargaPokok   MONEY, 
 @IsPosted     BIT, 
 @TglPosted    DATETIME, 
 @IDUserPosted INT, 
 @IDUserEntry  INT, 
 @IDUserEdit   INT
)
AS
    BEGIN
        DECLARE @NoUrut AS INT= 0;
        IF(@NoID <= 0)
            BEGIN
                SELECT @NoID = MAX(NoID)
                FROM MSaldoAwalPersediaan;
                SET @NoID = ISNULL(@NoID, 0) + 1;
                SET @Kode = 'SAP/' + SUBSTRING(CONVERT(VARCHAR(100), @Tanggal, 112), 3, 4) + '/';
                SELECT @NoUrut = MAX(CASE
                                         WHEN ISNUMERIC(RIGHT(Kode, 5)) = 1
                                         THEN RIGHT(Kode, 5)
                                         ELSE 0
                                     END)
                FROM MSaldoAwalPersediaan
                WHERE LEFT(Kode, LEN(@Kode)) = @Kode;
                SET @NoUrut = ISNULL(@NoUrut, 0) + 1;
                SET @Kode = @Kode + LEFT('00000', 5 - LEN(CONVERT(VARCHAR(10), @NoUrut))) + CONVERT(VARCHAR(10), @NoUrut);
                INSERT INTO [dbo].[MSaldoAwalPersediaan]
                ([NoID], 
                 [IDGudang], 
                 [IDBarangD], 
                 [IDBarang], 
                 [Kode], 
                 [Tanggal], 
                 [Qty], 
                 [IDSatuan], 
                 [Konversi], 
                 [HargaPokok], 
                 [Jumlah], 
                 [IDUserEntry], 
                 [TglEntry], 
                 [IDUserEdit], 
                 [TglEdit], 
                 [IsPosted], 
                 [IDUserPosted], 
                 [TglPosted], 
                 [Keterangan]
                )
                       SELECT @NoID, 
                              @IDGudang, 
                              @IDBarangD, 
                              @IDBarang, 
                              @Kode, 
                              @Tanggal, 
                              @Qty, 
                              @IDSatuan, 
                              @Konversi, 
                              @HargaPokok, 
                              ROUND(@HargaPokok * @Qty, 2), 
                              @IDUserEntry, 
                              GETDATE(), 
                              NULL, 
                              NULL, 
                              0, 
                              NULL, 
                              NULL, 
                              @Keterangan;
            END;
            ELSE
            BEGIN
                UPDATE [dbo].[MSaldoAwalPersediaan]
                  SET 
                      [IDGudang] = @IDGudang, 
                      [IDBarangD] = @IDBarangD, 
                      [IDBarang] = @IDBarang, 
                      [Kode] = @Kode, 
                      [Tanggal] = @Tanggal, 
                      [Qty] = @Qty, 
                      [IDSatuan] = @IDSatuan, 
                      [Konversi] = @Konversi, 
                      [HargaPokok] = @HargaPokok, 
                      [Jumlah] = ROUND(@HargaPokok * @Qty, 2), 
                      [IDUserEdit] = @IDUserEdit, 
                      [TglEdit] = GETDATE(), 
                      [IsPosted] = @IsPosted, 
                      [IDUserPosted] = CASE
                                           WHEN ISNULL(@IsPosted, 0) = 1
                                           THEN @IDUserPosted
                                           ELSE NULL
                                       END, 
                      [TglPosted] = CASE
                                        WHEN ISNULL(@IsPosted, 0) = 1
                                        THEN GETDATE()
                                        ELSE NULL
                                    END, 
                      [Keterangan] = @Keterangan
                WHERE [NoID] = @NoID;
            END;
        SELECT @NoID AS NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spSimpanMSaldoAwalPiutang]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[spSimpanMSaldoAwalPiutang]
(@NoID         BIGINT, 
 @Kode         VARCHAR(255), 
 @KodeReff     VARCHAR(255), 
 @Tanggal      DATETIME, 
 @JatuhTempo   DATETIME, 
 @Jumlah       MONEY, 
 @Keterangan   VARCHAR(MAX), 
 @IDCustomer   INT, 
 @IsPosted     BIT, 
 @TglPosted    DATETIME, 
 @IDUserPosted INT, 
 @IDUserEntry  INT, 
 @IDUserEdit   INT
)
AS
    BEGIN
        DECLARE @NoUrut AS INT= 0;
        IF(@NoID <= 0)
            BEGIN
                SELECT @NoID = MAX(NoID)
                FROM MSaldoAwalPiutang;
                SET @NoID = ISNULL(@NoID, 0) + 1;
                SET @Kode = 'SAH/' + SUBSTRING(CONVERT(VARCHAR(100), @Tanggal, 112), 3, 4) + '/';
                SELECT @NoUrut = MAX(CASE
                                         WHEN ISNUMERIC(RIGHT(Kode, 5)) = 1
                                         THEN RIGHT(Kode, 5)
                                         ELSE 0
                                     END)
                FROM MSaldoAwalPiutang
                WHERE LEFT(Kode, LEN(@Kode)) = @Kode;
                SET @NoUrut = ISNULL(@NoUrut, 0) + 1;
                SET @Kode = @Kode + LEFT('00000', 5 - LEN(CONVERT(VARCHAR(10), @NoUrut))) + CONVERT(VARCHAR(10), @NoUrut);
                INSERT INTO [dbo].[MSaldoAwalPiutang]
                ([NoID], 
                 [IDCustomer], 
                 [Kode], 
                 [Tanggal], 
                 [JatuhTempo], 
                 [KodeReff], 
                 [Jumlah], 
                 [IDUserEntry], 
                 [TglEntry], 
                 [IDUserEdit], 
                 [TglEdit], 
                 [IsPosted], 
                 [IDUserPosted], 
                 [TglPosted], 
                 [Keterangan]
                )
                       SELECT @NoID, 
                              @IDCustomer, 
                              @Kode, 
                              @Tanggal, 
                              @JatuhTempo, 
                              @KodeReff, 
                              ROUND(@Jumlah, 2), 
                              @IDUserEntry, 
                              GETDATE(), 
                              NULL, 
                              NULL, 
                              0, 
                              NULL, 
                              NULL, 
                              @Keterangan;
            END;
            ELSE
            BEGIN
                UPDATE [dbo].[MSaldoAwalPiutang]
                  SET 
                      [IDCustomer] = @IDCustomer, 
                      [Kode] = @Kode, 
                      [KodeReff] = @KodeReff, 
                      [Tanggal] = @Tanggal, 
                      [JatuhTempo] = @JatuhTempo, 
                      [Jumlah] = ROUND(@Jumlah, 2), 
                      [IDUserEdit] = @IDUserEdit, 
                      [TglEdit] = GETDATE(), 
                      [IsPosted] = @IsPosted, 
                      [IDUserPosted] = CASE
                                           WHEN ISNULL(@IsPosted, 0) = 1
                                           THEN @IDUserPosted
                                           ELSE NULL
                                       END, 
                      [TglPosted] = CASE
                                        WHEN ISNULL(@IsPosted, 0) = 1
                                        THEN GETDATE()
                                        ELSE NULL
                                    END, 
                      [Keterangan] = @Keterangan
                WHERE [NoID] = @NoID;
            END;
        SELECT @NoID AS NoID;
    END;
GO

/****** Object:  StoredProcedure [dbo].[spSimpanMStockOpname]    Script Date: 6/23/2021 11:20:18 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[spSimpanMStockOpname]
(@NoID         BIGINT, 
 @Kode         VARCHAR(255), 
 @NoReff       VARCHAR(255), 
 @Tanggal      DATETIME, 
 @Catatan      VARCHAR(MAX), 
 @Total        MONEY, 
 @IsPosted     BIT, 
 @TglPosted    DATETIME, 
 @IDUserPosted INT, 
 @IDUserEntry  INT, 
 @IDUserEdit   INT, 
 @IDGudang     INT
)
AS
    BEGIN
        DECLARE @NoUrut AS INT= 0;
        IF(@NoID <= 0)
            BEGIN
                SELECT @NoID = MAX(NoID)
                FROM MStockOpname;
                SET @NoID = ISNULL(@NoID, 0) + 1;
                SET @Kode = 'STO/' + SUBSTRING(CONVERT(VARCHAR(100), @Tanggal, 112), 3, 4) + '/';
                SELECT @NoUrut = MAX(CASE
                                         WHEN ISNUMERIC(RIGHT(Kode, 5)) = 1
                                         THEN RIGHT(Kode, 5)
                                         ELSE 0
                                     END)
                FROM MStockOpname
                WHERE LEFT(Kode, LEN(@Kode)) = @Kode;
                SET @NoUrut = ISNULL(@NoUrut, 0) + 1;
                SET @Kode = @Kode + LEFT('00000', 5 - LEN(CONVERT(VARCHAR(10), @NoUrut))) + CONVERT(VARCHAR(10), @NoUrut);
                INSERT INTO [MStockOpname]
                (NoID, 
                 Kode, 
                 NoReff, 
                 Tanggal, 
                 Catatan, 
                 Total, 
                 IsPosted, 
                 TglPosted, 
                 IDUserPosted, 
                 IDUserEntry, 
                 IDUserEdit, 
                 IDGudang
                )
                       SELECT @NoID, 
                              @Kode, 
                              @NoReff, 
                              @Tanggal, 
                              @Catatan, 
                              @Total, 
                              @IsPosted, 
                              @TglPosted, 
                              @IDUserPosted, 
                              @IDUserEntry, 
                              @IDUserEdit, 
                              @IDGudang;
            END;
            ELSE
            BEGIN
                UPDATE [MStockOpname]
                  SET 
                      NoID = @NoID, 
                      Kode = @Kode, 
                      NoReff = @NoReff, 
                      Tanggal = @Tanggal, 
                      Catatan = @Catatan, 
                      Total = @Total, 
                      IsPosted = @IsPosted, 
                      TglPosted = @TglPosted, 
                      IDUserPosted = @IDUserPosted, 
                      IDUserEntry = @IDUserEntry, 
                      IDUserEdit = @IDUserEdit, 
                      IDGudang = @IDGudang
                WHERE NoID = @NoID;
            END;
        SELECT @NoID AS NoID;
    END;
GO
CREATE TABLE [dbo].[MKlasAkun](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[NoUrut] [int] NULL,
	[Klasifikasi] [varchar](150) NULL,
	[Debet] [bit] NULL,
	[Neraca] [bit] NULL,
 CONSTRAINT [PK_MKlasAkun] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[MKlasAkun] ADD  CONSTRAINT [DF_MKlasAkun_ID]  DEFAULT (newid()) FOR [ID]
GO
CREATE TABLE [dbo].[MAkun](
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
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[MAkun] ADD  CONSTRAINT [DF_MAkun_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[MAkun] ADD  CONSTRAINT [DF_MAkun_IDDepartemen]  DEFAULT ((1)) FOR [IDDepartemen]
GO
ALTER TABLE [dbo].[MAkun] ADD  CONSTRAINT [DF_MAkun_IDType]  DEFAULT ((0)) FOR [IDType]
GO
ALTER TABLE [dbo].[MAkun] ADD  CONSTRAINT [DF_MAkun_IsDebet]  DEFAULT ((0)) FOR [IsDebet]
GO
ALTER TABLE [dbo].[MAkun] ADD  CONSTRAINT [DF_MAkun_IsKasBank]  DEFAULT ((0)) FOR [IsKasBank]
GO
ALTER TABLE [dbo].[MAkun] ADD  CONSTRAINT [DF_MAkun_IDTypeBank]  DEFAULT ((0)) FOR [IDTypeBank]
GO
ALTER TABLE [dbo].[MAkun] ADD  CONSTRAINT [DF_MAkun_IsNeraca]  DEFAULT ((0)) FOR [IsNeraca]
GO
ALTER TABLE [dbo].[MAkun]  WITH CHECK ADD  CONSTRAINT [FK_MAkun_MKlasAkun] FOREIGN KEY([IDKlasAkun])
REFERENCES [dbo].[MKlasAkun] ([ID])
GO
ALTER TABLE [dbo].[MAkun] CHECK CONSTRAINT [FK_MAkun_MKlasAkun]
GO
USE [master];
GO
ALTER DATABASE [DBPOS] SET READ_WRITE; 
GO
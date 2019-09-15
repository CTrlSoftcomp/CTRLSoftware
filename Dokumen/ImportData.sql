UPDATE IMP_BRG SET UKURAN=UPPER(REPLACE(UKURAN, ' ', ''))

TRUNCATE TABLE MMerk
TRUNCATE TABLE MKategori
TRUNCATE TABLE MSatuan


INSERT INTO MSatuan (NoID, Kode, Nama, Konversi, IsActive) VALUES (1, 'PCS', 'PCS', 1, 1)
INSERT INTO MSatuan (NoID, Kode, Nama, Konversi, IsActive)
SELECT 1+ROW_NUMBER() OVER(ORDER BY UKURAN) AS NoID, UKURAN, UKURAN, 1, 1
FROM IMP_BRG 
WHERE NOT UKURAN IS NULL
GROUP BY UKURAN

INSERT INTO MKategori (NoID, Kode, Nama, IDParent, IsActive) VALUES (1, '000', 'UNCATEGORIES', -1, 1)

INSERT INTO MMerk (NoID, Kode, Nama, IsActive)
SELECT ROW_NUMBER() OVER(ORDER BY MERECK) AS NoID, MERECK, MERECK, 1
FROM IMP_BRG 
WHERE NOT MERECK IS NULL
GROUP BY MERECK

TRUNCATE TABLE MBarang
TRUNCATE TABLE MBarangD

INSERT INTO [dbo].[MBarang]
           ([NoID]
           ,[Kode]
           ,[Nama]
           ,[IDKategori]
           ,[DefaultBarcode]
           ,[Alias]
           ,[Keterangan]
           ,[IsActive]
           ,[IDTypePajak]
           ,[IDSupplier1]
           ,[IDSupplier2]
           ,[IDSupplier3]
           ,[HargaBeli]
           ,[IDSatuanBeli]
           ,[IsiCtn]
           ,[HargaBeliPcsBruto]
           ,[DiscProsen1]
           ,[DiscProsen2]
           ,[DiscProsen3]
           ,[DiscProsen4]
           ,[DiscProsen5]
           ,[DiscRp]
           ,[HargaBeliPcs]
           ,[IDSatuan]
           ,[ProsenUpA]
           ,[HargaJualA]
           ,[ProsenUpB]
           ,[HargaJualB])
SELECT ROW_NUMBER() OVER(ORDER BY [NO]) [NoID]
           ,'000'+ LEFT('0000', 4-LEN(CONVERT(VARCHAR(10), ROW_NUMBER() OVER(ORDER BY [NO])))) + CONVERT(VARCHAR(10), ROW_NUMBER() OVER(ORDER BY [NO])) [Kode]
           ,UPPER(ISNULL([NAMA BARANG], '') + ' ' + ISNULL(UKURAN, '')) NamaBarang
           ,1 [IDKategori]
           ,'000'+ LEFT('0000', 4-LEN(CONVERT(VARCHAR(10), ROW_NUMBER() OVER(ORDER BY [NO])))) + CONVERT(VARCHAR(10), ROW_NUMBER() OVER(ORDER BY [NO])) + 
		   ISNULL(dbo.sfn_ean_chkdigit('000'+ LEFT('0000', 4-LEN(CONVERT(VARCHAR(10), ROW_NUMBER() OVER(ORDER BY [NO])))) + CONVERT(VARCHAR(10), ROW_NUMBER() OVER(ORDER BY [NO]))), '') [DefaultBarcode]
           ,'' [Alias]
           ,'' [Keterangan]
           ,1 [IsActive]
           ,0 [IDTypePajak]
           ,0 [IDSupplier1]
           ,0 [IDSupplier2]
           ,0 [IDSupplier3]
           ,ISNULL(NETTO, 0) [HargaBeli]
           ,ISNULL(MSatuan.NoID, 1) [IDSatuanBeli]
           ,1 [IsiCtn]
           ,ISNULL(NETTO, 0) [HargaBeliPcsBruto]
           ,0 [DiscProsen1]
           ,0 [DiscProsen2]
           ,0 [DiscProsen3]
           ,0 [DiscProsen4]
           ,0 [DiscProsen5]
           ,0 [DiscRp]
           ,ISNULL(NETTO, 0) [HargaBeliPcs]
           ,ISNULL(MSatuan.NoID, 1) [IDSatuan]
           ,CASE WHEN ISNULL(NETTO, 0)=0 THEN 0 ELSE ROUND((ISNULL([HARGA JUAL], 0)-ISNULL(NETTO, 0))/ISNULL(NETTO, 0)*100, 2) END [ProsenUpA]
           ,ISNULL([HARGA JUAL], 0) [HargaJualA]
           ,-100.0 [ProsenUpB]
           ,0 [HargaJualB]
FROM IMP_BRG
LEFT JOIN MSatuan ON MSatuan.Kode=IMP_BRG.UKURAN
INSERT INTO [dbo].[MBarangD]
           ([NoID]
           ,[IDBarang]
           ,[IDSatuan]
           ,[Konversi]
           ,[Barcode]
           ,[IsDefault]
           ,[IsActive]
           ,[ProsenUpA]
           ,[HargaJualA]
           ,[ProsenUpB]
           ,[HargaJualB])
SELECT [NoID]
           ,NoID [IDBarang]
           ,[IDSatuan]
           ,1 [Konversi]
           ,DefaultBarcode [Barcode]
           ,1 [IsDefault]
           ,[IsActive]
           ,[ProsenUpA]
           ,[HargaJualA]
           ,[ProsenUpB]
           ,[HargaJualB]
FROM MBarang
UPDATE IMP_BRG SET UKURAN=UPPER(REPLACE(UKURAN, ' ', ''))

TRUNCATE TABLE MMerk
TRUNCATE TABLE MKategori
TRUNCATE TABLE MSatuan


--INSERT INTO MSatuan (NoID, Kode, Nama, Konversi, IsActive) VALUES (1, 'PCS', 'PCS', 1, 1)
INSERT INTO MSatuan (NoID, Kode, Nama, Konversi, IsActive)
SELECT ROW_NUMBER() OVER(ORDER BY SATUAN) AS NoID, SATUAN, SATUAN, Konversi, IsActive
FROM (
SELECT SATUAN, 1 Konversi, 1 IsActive
FROM IMP_BRG2 
WHERE NOT SATUAN IS NULL
GROUP BY SATUAN
UNION ALL
SELECT SATUANBESAR, 1, 1
FROM IMP_BRG2 
WHERE NOT SATUANBESAR IS NULL
GROUP BY SATUANBESAR) AS X 
GROUP BY SATUAN, Konversi, IsActive

INSERT INTO MKategori (NoID, Kode, Nama, IDParent, IsActive) VALUES (1, '000', 'UNCATEGORIES', -1, 1)
INSERT INTO MKategori (NoID, Kode, Nama, IDParent, IsActive)
SELECT 1+ROW_NUMBER() OVER(ORDER BY KATEGORI) NoID, KATEGORI Kode, KATEGORI Nama, -1 IDParent, 1 IsActive
FROM IMP_BRG2
WHERE NOT KATEGORI IS NULL
GROUP BY KATEGORI

INSERT INTO MMerk (NoID, Kode, Nama, IsActive) VALUES (1, '000', 'UNMERK', 1)
INSERT INTO MMerk (NoID, Kode, Nama, IsActive)
SELECT 1+ROW_NUMBER() OVER(ORDER BY MERK) AS NoID, MERK, MERK, 1
FROM IMP_BRG2
WHERE NOT MERK IS NULL
GROUP BY MERK

TRUNCATE TABLE MBarang
TRUNCATE TABLE MBarangD
TRUNCATE TABLE TLogPerubahanHargaBeli
TRUNCATE TABLE TLogPerubahanHargaJual

INSERT INTO [dbo].[MBarang]
           ([NoID]
           ,[Kode]
           ,[Nama]
           ,[IDKategori]
		   ,[IDMerk]
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
           ,[HargaJualB]
		   ,HargaJualA1
		   ,HargaJualB1)
SELECT ROW_NUMBER() OVER(ORDER BY [NO]) [NoID]
           ,'000'+ LEFT('0000', 4-LEN(CONVERT(VARCHAR(10), ROW_NUMBER() OVER(ORDER BY [NO])))) + CONVERT(VARCHAR(10), ROW_NUMBER() OVER(ORDER BY [NO])) [Kode]
           ,UPPER(ISNULL([NAMABARANG], '')) NamaBarang
           ,MKategori.NoID [IDKategori]
           ,MMerk.NoID [IDMerk]
           ,'000'+ LEFT('0000', 4-LEN(CONVERT(VARCHAR(10), ROW_NUMBER() OVER(ORDER BY [NO])))) + CONVERT(VARCHAR(10), ROW_NUMBER() OVER(ORDER BY [NO])) + 
		   ISNULL(dbo.sfn_ean_chkdigit('000'+ LEFT('0000', 4-LEN(CONVERT(VARCHAR(10), ROW_NUMBER() OVER(ORDER BY [NO])))) + CONVERT(VARCHAR(10), ROW_NUMBER() OVER(ORDER BY [NO]))), '') [DefaultBarcode]
           ,'' [Alias]
           ,'' [Keterangan]
           ,1 [IsActive]
           ,0 [IDTypePajak]
           ,3 [IDSupplier1]
           ,0 [IDSupplier2]
           ,0 [IDSupplier3]
           ,ISNULL(HARGABELI, 0)*CASE WHEN ISNULL(ISIBESAR, 0)>0 THEN ISIBESAR ELSE 1 END [HargaBeli]
           ,CASE WHEN ISNULL(ISIBESAR, 0)>0 THEN ISNULL(SATUANBESAR.NoID, 0) ELSE ISNULL(MSatuan.NoID, 0) END [IDSatuanBeli]
           ,CASE WHEN ISNULL(ISIBESAR, 0)>0 THEN ISIBESAR ELSE 1 END [IsiCtn]
           ,ISNULL(HARGABELI, 0)*CASE WHEN ISNULL(ISIBESAR, 0)>0 THEN ISIBESAR ELSE 1 END [HargaBeliPcsBruto]
           ,0 [DiscProsen1]
           ,0 [DiscProsen2]
           ,0 [DiscProsen3]
           ,0 [DiscProsen4]
           ,0 [DiscProsen5]
           ,0 [DiscRp]
           ,ISNULL(HARGABELI, 0) [HargaBeliPcs]
           ,ISNULL(MSatuan.NoID, 1) [IDSatuan]
           ,CASE WHEN ISNULL(HARGABELI, 0)=0 THEN 0 ELSE ROUND((ISNULL([HARGAJUAL], 0)-ISNULL(HARGABELI, 0))/ISNULL(HARGABELI, 0)*100, 2) END [ProsenUpA]
           ,ISNULL([HARGAJUAL], 0) [HargaJualA]
           ,-100.0 [ProsenUpB]
           ,0 [HargaJualB]
		   ,CASE WHEN ISNULL(ISIBESAR, 0)>0 THEN HARGABESAR ELSE 0 END [HARGAJUALA1]
           ,0 [HARGAJUALA1]
FROM IMP_BRG2
LEFT JOIN MSatuan ON MSatuan.Kode=IMP_BRG2.SATUAN
LEFT JOIN MSatuan SATUANBESAR ON SATUANBESAR.Kode=IMP_BRG2.SATUANBESAR
LEFT JOIN MKategori ON MKategori.Kode=IMP_BRG2.KATEGORI
LEFT JOIN MMerk ON MMerk.Kode=IMP_BRG2.MERK

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

DECLARE @NoID AS BIGINT = -1
SELECT @NoID=MAX(NoID) FROM MBarangD
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
SELECT ISNULL(@NoID, 0) + ROW_NUMBER() OVER(ORDER BY NoID) [NoID]
           ,NoID [IDBarang]
           ,[IDSatuanBeli]
           ,IsiCtn [Konversi]
           ,'K' + DefaultBarcode [Barcode]
           ,0 [IsDefault]
           ,[IsActive]
           ,CASE WHEN ISNULL(HargaBeliPcs, 0)*IsiCtn=0 THEN 0 ELSE ROUND((((ISNULL(HargaJualA1, 0)*ISNULL(IsiCtn, 0))-(ISNULL(HargaBeliPcs, 0)*IsiCtn))/(ISNULL(HargaBeliPcs, 0)*IsiCtn))*100, 2) END AS[ProsenUpA]
           ,[HargaJualA1]
           ,-100 [ProsenUpB]
           ,[HargaJualB1]
FROM MBarang
WHERE ISNULL(IsiCtn, 0)>1
/*
PostgreSQL Backup
Database: dbctrl/public
Backup Time: 2021-01-31 14:37:37
*/

DROP TABLE IF EXISTS "public"."__EFMigrationsHistory";
DROP TABLE IF EXISTS "public"."makun";
DROP TABLE IF EXISTS "public"."mjenistransaksi";
DROP TABLE IF EXISTS "public"."mjenistransaksid";
DROP TABLE IF EXISTS "public"."mjurnal";
DROP TABLE IF EXISTS "public"."mjurnalumum";
DROP TABLE IF EXISTS "public"."mjurnalumumd";
DROP TABLE IF EXISTS "public"."mkasin";
DROP TABLE IF EXISTS "public"."mkasind";
DROP TABLE IF EXISTS "public"."mkasindbayar";
DROP TABLE IF EXISTS "public"."mkasout";
DROP TABLE IF EXISTS "public"."mkasoutd";
DROP TABLE IF EXISTS "public"."mkasoutdbayar";
DROP TABLE IF EXISTS "public"."mkontak";
DROP TABLE IF EXISTS "public"."mrole";
DROP TABLE IF EXISTS "public"."muser";
CREATE TABLE "__EFMigrationsHistory" (
  "MigrationId" varchar(150) COLLATE "pg_catalog"."default" NOT NULL,
  "ProductVersion" varchar(32) COLLATE "pg_catalog"."default" NOT NULL
)
;
ALTER TABLE "__EFMigrationsHistory" OWNER TO "postgres";
CREATE TABLE "makun" (
  "id" int4 NOT NULL,
  "idparent" int4 NOT NULL DEFAULT '-1'::integer,
  "iddepartemen" int2 NOT NULL DEFAULT 1,
  "kode" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "nama" varchar(100) COLLATE "pg_catalog"."default" NOT NULL,
  "keterangan" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "idtype" int2,
  "isdebet" bit(1),
  "iskasbank" bit(1),
  "norekening" varchar(50) COLLATE "pg_catalog"."default",
  "atasnamarekening" varchar(255) COLLATE "pg_catalog"."default",
  "idtypebank" int2,
  "isneraca" bit(1)
)
;
ALTER TABLE "makun" OWNER TO "postgres";
CREATE TABLE "mjenistransaksi" (
  "id" int2 NOT NULL,
  "nourut" int2 NOT NULL,
  "kode" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "nama" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "keterangan" varchar(255) COLLATE "pg_catalog"."default" NOT NULL
)
;
ALTER TABLE "mjenistransaksi" OWNER TO "postgres";
CREATE TABLE "mjenistransaksid" (
  "nourut" int2 NOT NULL,
  "idjenistransaksi" int2 NOT NULL,
  "jenis" varchar(255) COLLATE "pg_catalog"."default",
  "prefix" varchar(255) COLLATE "pg_catalog"."default",
  "sufix" varchar(255) COLLATE "pg_catalog"."default",
  "digit" int2,
  "format" varchar(255) COLLATE "pg_catalog"."default"
)
;
ALTER TABLE "mjenistransaksid" OWNER TO "postgres";
CREATE TABLE "mjurnal" (
  "idtransaksi" int4 NOT NULL,
  "idjenistransaksid" int2 NOT NULL,
  "idpasangan" int2 NOT NULL,
  "idakun" int4 NOT NULL,
  "iddepartemen" int2 NOT NULL,
  "kode" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "tanggal" date NOT NULL,
  "debet" money,
  "credit" money,
  "idkurs" int2,
  "kurs" money,
  "debeta" money,
  "credita" money,
  "keterangan" varchar(255) COLLATE "pg_catalog"."default"
)
;
ALTER TABLE "mjurnal" OWNER TO "postgres";
CREATE TABLE "mjurnalumum" (
  "id" int4 NOT NULL,
  "kode" varchar(50) COLLATE "pg_catalog"."default",
  "tanggal" date,
  "keterangan" varchar(255) COLLATE "pg_catalog"."default",
  "iduserentri" int2,
  "tglentri" date,
  "iduseredit" int2,
  "tgledit" date,
  "isposted" bit(1),
  "tglposted" date
)
;
ALTER TABLE "mjurnalumum" OWNER TO "postgres";
CREATE TABLE "mjurnalumumd" (
  "id" int2 NOT NULL,
  "idjurnalumum" int4 NOT NULL,
  "idakun" int2,
  "iddepartemen" int2,
  "debet" money,
  "credit" money,
  "idkurs" int2,
  "kurs" money,
  "debeta" money,
  "credita" money,
  "keterangan" varchar(255) COLLATE "pg_catalog"."default"
)
;
ALTER TABLE "mjurnalumumd" OWNER TO "postgres";
CREATE TABLE "mkasin" (
  "id" int4 NOT NULL,
  "kode" varchar(50) COLLATE "pg_catalog"."default",
  "tanggal" date,
  "keterangan" varchar(255) COLLATE "pg_catalog"."default",
  "idakun" int4 NOT NULL,
  "iduserentri" int2,
  "tglentri" date,
  "iduseredit" int2,
  "tgledit" date,
  "isposted" bit(1),
  "tglposted" date
)
;
ALTER TABLE "mkasin" OWNER TO "postgres";
CREATE TABLE "mkasind" (
  "id" int2 NOT NULL,
  "idkasin" int4 NOT NULL,
  "idakun" int2,
  "iddepartemen" int2,
  "debet" money,
  "credit" money,
  "idkurs" int2,
  "kurs" money,
  "debeta" money,
  "credita" money,
  "keterangan" varchar(255) COLLATE "pg_catalog"."default"
)
;
ALTER TABLE "mkasind" OWNER TO "postgres";
CREATE TABLE "mkasindbayar" (
  "id" int2 NOT NULL,
  "idkasin" int4 NOT NULL,
  "idakun" int4 NOT NULL,
  "idkontak" int4 NOT NULL,
  "norekening" varchar(255) COLLATE "pg_catalog"."default",
  "bank" varchar(255) COLLATE "pg_catalog"."default",
  "atasnamarekening" varchar(255) COLLATE "pg_catalog"."default",
  "tgltransfer" date,
  "nobukti" varchar(255) COLLATE "pg_catalog"."default",
  "tgljatuhtempo" date,
  "nominal" money
)
;
ALTER TABLE "mkasindbayar" OWNER TO "postgres";
CREATE TABLE "mkasout" (
  "id" int4 NOT NULL,
  "kode" varchar(50) COLLATE "pg_catalog"."default",
  "tanggal" date,
  "keterangan" varchar(255) COLLATE "pg_catalog"."default",
  "idakun" int4 NOT NULL,
  "iduserentri" int2,
  "tglentri" date,
  "iduseredit" int2,
  "tgledit" date,
  "isposted" bit(1),
  "tglposted" date
)
;
ALTER TABLE "mkasout" OWNER TO "postgres";
CREATE TABLE "mkasoutd" (
  "id" int2 NOT NULL,
  "idkasout" int4 NOT NULL,
  "idakun" int2,
  "iddepartemen" int2,
  "debet" money,
  "credit" money,
  "idkurs" int2,
  "kurs" money,
  "debeta" money,
  "credita" money,
  "keterangan" varchar(255) COLLATE "pg_catalog"."default"
)
;
ALTER TABLE "mkasoutd" OWNER TO "postgres";
CREATE TABLE "mkasoutdbayar" (
  "id" int2 NOT NULL,
  "idkasout" int4 NOT NULL,
  "idakun" int4 NOT NULL,
  "idkontak" int4 NOT NULL,
  "norekening" varchar(255) COLLATE "pg_catalog"."default",
  "bank" varchar(255) COLLATE "pg_catalog"."default",
  "atasnamarekening" varchar(255) COLLATE "pg_catalog"."default",
  "tgltransfer" date,
  "nobukti" varchar(255) COLLATE "pg_catalog"."default",
  "tgljatuhtempo" date,
  "nominal" money
)
;
ALTER TABLE "mkasoutdbayar" OWNER TO "postgres";
CREATE TABLE "mkontak" (
  "id" int4 NOT NULL,
  "kode" varchar(50) COLLATE "pg_catalog"."default",
  "nama" varchar(255) COLLATE "pg_catalog"."default",
  "alamat1" varchar(255) COLLATE "pg_catalog"."default",
  "alamat2" varchar(255) COLLATE "pg_catalog"."default",
  "alamat3" varchar(255) COLLATE "pg_catalog"."default",
  "hp" varchar(50) COLLATE "pg_catalog"."default",
  "telpon" varchar(50) COLLATE "pg_catalog"."default",
  "iswhatsapp" bit(1),
  "norekening" varchar(255) COLLATE "pg_catalog"."default",
  "bank" varchar(255) COLLATE "pg_catalog"."default",
  "atasnamarekening" varchar(255) COLLATE "pg_catalog"."default"
)
;
ALTER TABLE "mkontak" OWNER TO "postgres";
CREATE TABLE "mrole" (
  "id" int4 NOT NULL,
  "role" varchar(255) COLLATE "pg_catalog"."default",
  "issupervisor" bit(1)
)
;
ALTER TABLE "mrole" OWNER TO "postgres";
CREATE TABLE "muser" (
  "id" int4 NOT NULL,
  "userid" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "pwd" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "nama" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "idkontak" int4 NOT NULL,
  "idrole" int2 NOT NULL
)
;
ALTER TABLE "muser" OWNER TO "postgres";
BEGIN;
LOCK TABLE "public"."__EFMigrationsHistory" IN SHARE MODE;
DELETE FROM "public"."__EFMigrationsHistory";
COMMIT;
BEGIN;
LOCK TABLE "public"."makun" IN SHARE MODE;
DELETE FROM "public"."makun";
INSERT INTO "public"."makun" ("id","idparent","iddepartemen","kode","nama","keterangan","idtype","isdebet","iskasbank","norekening","atasnamarekening","idtypebank","isneraca") VALUES (1, -1, 1, '1', 'AKTIVA', '', 0, '1', '0', NULL, NULL, 0, '1'),(2, -1, 1, '2', 'MODAL & HUTANG', '', 0, '0', '0', NULL, NULL, 0, '1'),(3, -1, 1, '3', 'MODAL', '', 0, '0', '0', NULL, NULL, 0, '1'),(4, -1, 1, '4', 'PENDAPATAN', '', 0, '0', '0', NULL, NULL, 0, '0'),(5, -1, 1, '5', 'HARGA POKOK PENJUALAN', '', 0, '1', '0', NULL, NULL, 0, '0'),(6, -1, 1, '6', 'BIAYA OPERASIONAL', '', 0, '1', '0', NULL, NULL, 0, '0'),(7, -1, 1, '7', 'PENDAPATAN LAIN-LAIN', '', 0, '0', '0', NULL, NULL, 0, '0'),(11, 1, 1, '11', 'Kas dan Setara Kas', '', 0, '1', '0', NULL, NULL, 0, '1'),(12, 1, 1, '12', 'Piutang', '', 0, '1', '0', NULL, NULL, 0, '1'),(13, 1, 1, '13', 'Persediaan', '', 0, '1', '0', NULL, NULL, 0, '1'),(14, 1, 1, '14', 'Pajak', '', 0, '1', '0', NULL, NULL, 0, '1'),(15, 1, 1, '15', 'Aset Lancar Lainnya', '', 0, '1', '0', NULL, NULL, 0, '1'),(16, 1, 1, '21', 'Aset Tetap', '', 0, '1', '0', NULL, NULL, 0, '1'),(17, 1, 1, '22', 'Aset Tidak Lancar Lainnya', '', 0, '1', '0', NULL, NULL, 0, '1'),(18, 2, 1, '31', 'Kewajiban Lancar', '', 0, '0', '0', NULL, NULL, 0, '1'),(19, 2, 1, '32', 'Hutang Pajak', '', 0, '0', '0', NULL, NULL, 0, '1'),(20, 3, 1, '33', 'Ekuitas', '', 0, '0', '0', NULL, NULL, 0, '1'),(21, 4, 1, '41', 'Penjualan Bersih', '', 0, '0', '0', NULL, NULL, 0, '0'),(22, 5, 1, '42', 'Beban Pokok Penjualan', '', 0, '1', '0', NULL, NULL, 0, '0'),(23, 6, 1, '51', 'Beban Usaha', '', 0, '1', '0', NULL, NULL, 0, '0'),(24, 7, 1, '61', 'Pendapatan Usaha Lain-Lain', '', 0, '0', '0', NULL, NULL, 0, '0'),(25, 6, 1, '53', 'Beban Usaha Lain-Lain', '', 0, '1', '0', NULL, NULL, 0, '0'),(26, 6, 1, '54', 'Beban Pajak Penghasilan Badan', '', 0, '1', '0', NULL, NULL, 0, '0'),(27, 6, 1, '52', 'Beban Usaha Umum & Administrasi', '', 0, '1', '0', NULL, NULL, 0, '0'),(28, 11, 1, '11.1', 'KAS', '', 0, '1', '0', NULL, NULL, 0, '1'),(29, 11, 1, '11.2', 'BANK', '', 0, '1', '0', NULL, NULL, 0, '1'),(1201, 28, 1, '11.101', 'KAS BESAR / SETORAN KASIR', '', 0, '1', '1', NULL, NULL, 1, '1'),(1202, 28, 1, '11.102', 'KAS KECIL KLAPAN', '', 0, '1', '1', NULL, NULL, 1, '1'),(1203, 29, 1, '11.201', 'BCA 125.067.0075', '', 0, '1', '1', NULL, NULL, 3, '1'),(1204, 29, 1, '11.202', 'BCA 125.370015.1', '', 0, '1', '1', NULL, NULL, 3, '1'),(1205, 11, 1, '1.105', 'MANDIRI', '', 0, '1', '1', NULL, NULL, 3, '1'),(1206, 11, 1, '1.106', 'BRI', '', 0, '1', '1', NULL, NULL, 3, '1'),(1207, 11, 1, '1.107', 'BNI', '', 0, '1', '1', NULL, NULL, 3, '1'),(1301, 12, 1, '1.201', 'PIUTANG DAGANG', '', 2, '1', '0', 'NULL', 'NULL', 0, '1'),(1302, 12, 1, '1.202', 'PIUTANG SEWA GONDOLA', '', 2, '1', '0', 'NULL', 'NULL', 0, '1'),(1303, 12, 1, '1.203', 'PIUTANG GIRO MUNDUR', '', 0, '1', '0', 'NULL', 'NULL', 0, '1'),(1304, 12, 1, '1.204', 'PIUTANG KARYAWAN', '', 2, '1', '1', NULL, NULL, 2, '1'),(1305, 12, 1, '1.205', 'PIUTANG CABANG', '', 2, '1', '0', 'NULL', 'NULL', 0, '1'),(1306, 12, 1, '1.206', 'PIUTANG SEWA RAK', '', 2, '1', '0', 'NULL', 'NULL', 0, '1'),(1307, 12, 1, '1.207', 'KAS KECIL', '', 0, '1', '1', NULL, NULL, 1, '1'),(1308, 12, 1, '1.208', 'PIUTANG ANTAR CABANG', '', 0, '1', '1', 'NULL', 'NULL', 1, '1'),(1401, 13, 1, '1.301', 'PERSEDIAAN BARANG DAGANGAN', '', 1, '1', '0', 'NULL', 'NULL', 0, '1'),(1402, 13, 1, '1.302', 'UANG MUKA PEMBELIAN', '', 2, '1', '0', 'NULL', 'NULL', 0, '1'),(1500, 14, 1, '1.4', 'UANG MUKA PAJAK', '', 0, '1', '0', 'NULL', 'NULL', 0, '1'),(1501, 14, 1, '1.401', 'PPN MASUKAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '1'),(1502, 14, 1, '1.402', 'PPN MASUKAN YBF', '', 0, '1', '0', 'NULL', 'NULL', 0, '1'),(1503, 14, 1, '1.403', 'PPH PASAL 22', '', 0, '1', '0', 'NULL', 'NULL', 0, '1'),(1504, 14, 1, '1.404', 'PPH PASAL 23', '', 0, '1', '0', 'NULL', 'NULL', 0, '1'),(1505, 14, 1, '1.405', 'PPH PASAL 25', '', 0, '1', '0', 'NULL', 'NULL', 0, '1'),(1601, 15, 1, '1.501', 'ASET LANCAR LAINNYA', '', 0, '1', '0', 'NULL', 'NULL', 0, '1'),(2201, 16, 1, '2.101', 'TANAH', '', 0, '1', '0', 'NULL', 'NULL', 0, '1'),(2202, 16, 1, '2.102', 'BANGUNAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '1'),(2203, 16, 1, '2.103', 'AKUMULASI PENYUSUTAN BANGUNAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '1'),(2204, 16, 1, '2.104', 'INVENTARIS KANTOR', '', 0, '1', '0', 'NULL', 'NULL', 0, '1'),(2205, 16, 1, '2.105', 'AKUMULASI PENYUSUTAN INVENTARIS KANTOR', '', 0, '1', '0', 'NULL', 'NULL', 0, '1'),(2206, 16, 1, '2.106', 'INVENTARIS KENDARAAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '1'),(2207, 16, 1, '2.107', 'AKUMULASI PENYUSUTAN INVENTARIS KENDARAAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '1'),(2208, 16, 1, '2.108', 'INSTALASI LISTRIK/GENSET', '', 0, '1', '0', 'NULL', 'NULL', 0, '1'),(2209, 16, 1, '2.109', 'AKUMULASI PENYUSUTAN INSTALASI LISTRIK/GENSET', '', 0, '1', '0', 'NULL', 'NULL', 0, '1'),(3201, 18, 1, '3.101', 'HUTANG DAGANG', '', 2, '0', '0', 'NULL', 'NULL', 0, '1'),(3202, 18, 1, '3.102', 'HUTANG GIRO MUNDUR', '', 0, '0', '0', 'NULL', 'NULL', 0, '1'),(3203, 18, 1, '3.103', 'UANG MUKA PENJUALAN', '', 2, '0', '0', 'NULL', 'NULL', 0, '1'),(3204, 18, 1, '3.104', 'HUTANG ASSET', '', 2, '0', '0', 'NULL', 'NULL', 0, '1'),(3205, 18, 1, '3.105', 'HUTANG MLH PKP', '', 0, '0', '0', 'NULL', 'NULL', 0, '1'),(3206, 18, 1, '3.106', 'HUTANG MLH NON PKP', '', 0, '0', '0', 'NULL', 'NULL', 0, '1'),(3300, 19, 1, '3.2', 'HUTANG PAJAK', '', 0, '0', '0', 'NULL', 'NULL', 0, '1'),(3301, 19, 1, '3.201', 'PPN KELUARAN', '', 0, '0', '0', 'NULL', 'NULL', 0, '1'),(3302, 19, 1, '3.202', 'HUTANG PPN DN', '', 0, '0', '0', 'NULL', 'NULL', 0, '1'),(3303, 19, 1, '3.203', 'HUTANG PPN MEMBANGUN SENDIRI', '', 0, '0', '0', 'NULL', 'NULL', 0, '1'),(3304, 19, 1, '3.204', 'HUTANG PPH PASAL 4 AYAT 2', '', 0, '0', '0', 'NULL', 'NULL', 0, '1'),(3305, 19, 1, '3.205', 'HUTANG PPH PASAL 21', '', 0, '0', '0', 'NULL', 'NULL', 0, '1'),(3306, 19, 1, '3.206', 'HUTANG PPH PASAL 25', '', 0, '0', '0', 'NULL', 'NULL', 0, '1'),(3307, 19, 1, '3.207', 'HUTANG PPH PASAL 29', '', 0, '0', '0', 'NULL', 'NULL', 0, '1'),(3401, 20, 1, '3.301', 'MODAL DISETOR', '', 0, '0', '0', 'NULL', 'NULL', 0, '1'),(3402, 20, 1, '3.302', 'LABA DITAHAN', '', 0, '0', '0', 'NULL', 'NULL', 0, '1'),(3403, 20, 1, '3.303', 'LABA PERIODE BERJALAN', '', 0, '0', '0', 'NULL', 'NULL', 0, '1'),(3404, 20, 1, '3.304', 'PRIVE', '', 0, '0', '0', 'NULL', 'NULL', 0, '1'),(4201, 21, 1, '4.101', 'PENJUALAN', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(4202, 21, 1, '4.102', 'PENDAPATAN LAIN LAIN', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(4203, 21, 1, '4.103', 'RETUR PENJUALAN', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(4301, 22, 1, '4.201', 'BEBAN POKOK PENJUALAN (HPP)', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(4302, 22, 1, '4.202', 'PEMBELIAN BARANG DAGANGAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(4303, 22, 1, '4.203', 'DISKON PEMBELIAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(4304, 22, 1, '4.204', 'RETUR PEMBELIAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5201, 23, 1, '5.101', 'BIAYA GAJI BAGIAN PENJUALAN & GUDANG', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5202, 23, 1, '5.102', 'BIAYA MAKAN/KOMSUMSI', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5203, 23, 1, '5.103', 'BIAYA BEROBAT', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5204, 23, 1, '5.104', 'BIAYA SERAGAM KARYAWAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5206, 23, 1, '5.106', 'KOMISI SALES/DRIVER/HELPER', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5207, 23, 1, '5.107', 'BIAYA PERLENGKAPAN GUDANG', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5208, 23, 1, '5.108', 'BIAYA AKSESORIS DISPLAY', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5209, 23, 1, '5.109', 'BIAYA / PENDAPATAN BARCODE', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5210, 23, 1, '5.11', 'BIAYA PROMOSI/IKLAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5211, 23, 1, '5.111', 'BIAYA BONUS PENJUALAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5212, 23, 1, '5.112', 'BIAYA PENJUALAN LAIN-LAIN', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5213, 23, 1, '5.113', 'BIAYA BENSIN/SOLAR/OLIE KENDARAAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5214, 23, 1, '5.114', 'PARKIR/TOL/RESTRIBUSI', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5215, 23, 1, '5.115', 'BIAYA TRANSPORTASI', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5216, 23, 1, '5.116', 'BIAYA PENGURUSAN PERIJINAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5217, 23, 1, '5.117', 'BIAYA ASURANSI', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5218, 23, 1, '5.118', 'BIAYA SEWA TANAH DAN BANGUNAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5219, 23, 1, '5.119', 'BIAYA PEMELIHARAAN BANGUNAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5220, 23, 1, '5.12', 'BIAYA PEMELIHARAAN KENDARAAN KANTOR', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5221, 23, 1, '5.121', 'BIAYA PEMELIHARAAN INVENTARIS KANTOR', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5222, 23, 1, '5.122', 'BEBAN SERVIS & PERBAIKAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5223, 23, 1, '5.123', 'BIAYA SURAT-SURAT KENDARAAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5224, 23, 1, '5.124', 'BIAYA PENYUSUTAN BANGUNAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5225, 23, 1, '5.125', 'BIAYA PENYUSUTAN KENDARAAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5226, 23, 1, '5.126', 'BIAYA PENYST. INV.KANTOR', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5227, 23, 1, '5.127', 'BIAYA PERLENGKAPAN LISTRIK', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5300, 27, 1, '5.2', 'BIAYA PULSA', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5301, 27, 1, '5.201', 'BIAYA GAJI STAF', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5302, 27, 1, '5.202', 'BIAYA THR', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5303, 27, 1, '5.203', 'UANG MAKAN STAFF', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5304, 27, 1, '5.204', 'BIAYA DIREKSI', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5305, 27, 1, '5.205', 'BIAYA ALAT-ALAT TULIS', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5306, 27, 1, '5.206', 'BIAYA CETAK/FOTO COPY', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5307, 27, 1, '5.207', 'BIAYA PERALATAN KANTOR', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5308, 27, 1, '5.208', 'BIAYA PLN / SOLAR GENSET', '', 0, '1', '1', NULL, NULL, 2, '0'),(5309, 27, 1, '5.209', 'BIAYA PDAM', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5310, 27, 1, '5.21', 'BIAYA SARANA KANTOR', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5311, 27, 1, '5.211', 'BIAYA TELEPON & INTERNET', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5312, 27, 1, '5.212', 'XXBENDA-BENDA POS', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5313, 27, 1, '5.213', 'PEMBULATAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5314, 27, 1, '5.214', 'MANAGEMENT FEE & CONSULTANT', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5315, 27, 1, '5.215', 'BIAYA PAJAK', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5316, 27, 1, '5.216', 'PAJAK BUMI DAN BANGUNAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5317, 27, 1, '5.217', 'IURAN/SUMBANGAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5318, 27, 1, '5.218', 'BIAYA MATERAI', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5319, 27, 1, '5.219', 'BIAYA TRANSFER', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5320, 23, 1, '5.22', 'BIAYA EDC CHARGE', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5321, 23, 1, '5.221', 'BIAYA ONGKIR', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5322, 23, 1, '5.222', 'DISKON KHUSUS KOPERASI', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5323, 27, 1, '5.223', 'BIAYA P3K', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5324, 27, 1, '5.224', 'BIAYA PERLENGKAPAN KASIR', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5325, 23, 1, '5.225', 'BPJS KESEHATAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5326, 23, 1, '5.226', 'BPJS TENAGAKERJA', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5400, 25, 1, '5.3', 'MONTHLY CHARGE', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5401, 25, 1, '5.301', 'BIAYA ADMINISTRASI BANK', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5402, 25, 1, '5.302', 'BEBAN USAHA LAINNYA', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5403, 25, 1, '5.303', 'BEBAN ARISAN', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5405, 25, 1, '5.305', 'SUMBANGAN INTERN', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5406, 25, 1, '5.306', 'SUMBANGAN EXTERN', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5407, 27, 1, '5.307', 'MAINTENANCE KOMPUTER', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(5408, 23, 1, '5.308', 'BIAYA PENYUSUTAN INS LISTRIK/GENSET', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(6201, 24, 1, '6.101', 'PENDAPATAN SEWA GONDOLA', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6202, 24, 1, '6.102', 'PENDAPATAN TARGET PENJUALAN', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6203, 24, 1, '6.103', 'PENDAPATAN GRAND OPENING', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6204, 24, 1, '6.104', 'PENDAPATAN LISTING FEE', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6205, 24, 1, '6.105', 'PENDAPATAN USAHA LAINNYA', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6206, 24, 1, '6.106', 'PENDAPATAN JASA GIRO', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6207, 24, 1, '6.107', 'PENDAPATAN PARKIR', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6208, 24, 1, '6.108', 'PENDAPATAN SEWA STAND', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6209, 24, 1, '6.109', 'PENDAPATAN TAS/KRESEK', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6210, 24, 1, '6.11', 'PENDAPATAN KARDUS', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6211, 24, 1, '6.111', 'PENDAPATAN TAS/KRESEK', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6212, 24, 1, '6.112', 'PENDAPATAN DENDA', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6213, 24, 1, '6.113', 'PENDAPATAN SEWA RUMAH', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6214, 24, 1, '6.114', 'PENDAPATAN SELISIH RESTOK', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6215, 24, 1, '6.115', 'ONGKOS KU', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6216, 24, 1, '6.116', 'DISCON PRO', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6217, 24, 1, '6.117', 'KURANG POT PPN', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6218, 24, 1, '6.118', 'VISIBILITY', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6219, 23, 1, '6.119', 'BIAYA EXPEDISI', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(6220, 24, 1, '6.12', 'PENDAPATAN SELISIH VOUCHER (AWAL - AWAL)', '', 0, '0', '1', NULL, NULL, 2, '0'),(6221, 24, 1, '6.121', 'INSENTIVE', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6222, 24, 1, '6.122', 'PENDAPATAN SELISIH RETUR', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6223, 24, 1, '6.123', 'SELISIH DISKON', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6224, 24, 1, '6.124', 'SELISIH PENJUALAN', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6225, 24, 1, '6.125', 'BAGI HASIL ', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6227, 24, 1, '6.127', 'PROMO', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6228, 24, 1, '6.128', 'PENDAPATAN VENDOR', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6229, 24, 1, '6.129', 'PENDAPATAN PLN', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6230, 24, 1, '6.13', 'PENDAPATAN SELISIH SALES', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6231, 24, 1, '6.131', 'ONGKOS BONGKAR', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6232, 24, 1, '6.132', 'PENDAPATAN  PROGRAM', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6233, 24, 1, '6.133', 'PENDAPATAN BUNGA', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6234, 24, 1, '6.134', 'PENDAPATAN SEWA RAK', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6235, 24, 1, '6.135', 'PENDAPATAN SELISIH RETUR', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6236, 24, 1, '6.136', 'PENDAPATAN SELISIH MDR', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6237, 24, 1, '6.137', 'PENDAPATAN SELISIH GESEK EDC', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6238, 24, 1, '6.138', 'PENDAPATAN DOBEL GESEK', '', 0, '0', '0', 'NULL', 'NULL', 0, '0'),(6320, 26, 1, '6.22', 'PAJAK BUNGA', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(6321, 23, 1, '6.221', 'BIAYA OPERASIONAL TAMU', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(6322, 23, 1, '6.222', 'BIAYA PACKING - REPACK - SELISIH', '', 0, '1', '0', 'NULL', 'NULL', 0, '0'),(53204, 25, 1, '53.104', 'xxx', '', 0, '1', '0', NULL, NULL, 0, '0'),(53205, 11, 1, '1.108', 'KAS KASIR A', '', 0, '1', '1', NULL, NULL, 1, '1'),(53206, 11, 1, '1.109', 'KAS KASIR B', '', 0, '1', '1', 'NULL', 'NULL', 1, '1'),(53207, 11, 1, '1.11', 'KAS KASIR C', '', 0, '1', '1', 'NULL', 'NULL', 1, '1'),(53208, 11, 1, '1.111', 'KAS KASIR D', '', 0, '1', '1', 'NULL', 'NULL', 1, '1'),(53209, 11, 1, '1.112', 'KAS KASIR E', '', 0, '1', '1', 'NULL', 'NULL', 1, '1'),(53210, 11, 1, '1.113', 'KAS KASIR F', '', 0, '1', '1', 'NULL', 'NULL', 1, '1'),(53211, 11, 1, '1.114', 'KAS KASIR J', '', 0, '1', '1', 'NULL', 'NULL', 1, '1'),(53212, 11, 1, '1.115', 'KAS KASIR K', '', 0, '1', '1', 'NULL', 'NULL', 1, '1'),(53213, 11, 1, '1.116', 'KAS KASIR L', '', 0, '1', '1', 'NULL', 'NULL', 1, '1'),(53214, 24, 1, '6.139', 'PENDAPATAN PEMBULATAN SELISIH PEMBELIAN', '', 0, '0', '1', NULL, NULL, 2, '0'),(53215, 24, 1, '6.14', 'PENDAPATAN BIAYA TRANSFER BEDA BANK', '', 0, '0', '1', NULL, NULL, 2, '0'),(53216, 11, 1, '1.151', 'KAS KECIL / PIUTANG SUM', '', 0, '1', '1', NULL, NULL, 1, '1'),(53217, 11, 1, '1.152', 'KAS KECIL / PIUTANG RIKA', '', 0, '1', '1', NULL, NULL, 1, '1'),(53218, 11, 1, '1.153', 'KAS KECIL / PIUTANG EKY', '', 0, '1', '1', NULL, NULL, 1, '1'),(53219, 11, 1, '1.154', 'KAS KECIL / PIUTANG OLIF', '', 0, '1', '1', NULL, NULL, 1, '1'),(53220, 11, 1, '1.155', 'KAS KECIL / PIUTANG RATNA', '', 0, '1', '1', NULL, NULL, 1, '1'),(53221, 11, 1, '1.156', 'KAS KECIL / PIUTANG VANDA', '', 0, '1', '1', NULL, NULL, 1, '1'),(53222, 11, 1, '1.157', 'KAS KECIL / PIUTANG YIN', '', 0, '1', '1', NULL, NULL, 1, '1'),(53223, 11, 1, '1.158', 'KAS KECIL / PIUTANG YOHANA (ADM GD)', '', 0, '1', '1', NULL, NULL, 1, '1'),(53224, 11, 1, '1.159', 'KAS KECIL / PIUTANG DIAH (ADM GD)', '', 0, '1', '1', 'NULL', 'NULL', 1, '1'),(53225, 11, 1, '1.16', 'KAS KECIL / PIUTANG RISKA (RAYA)', '', 0, '1', '1', 'NULL', 'NULL', 1, '1'),(53226, 11, 1, '1.121', 'KAS KECIL RAYA', '', 0, '1', '1', 'NULL', 'NULL', 1, '1'),(53227, 11, 1, '1.122', 'KAS KECIL RESTO', '', 0, '1', '1', 'NULL', 'NULL', 1, '1'),(53228, 12, 1, '1.209', 'PIUTANG VOUCHER SUPPLIER', '', 2, '1', '0', 'NULL', 'NULL', 0, '1'),(53229, 11, 1, '1.13', 'PENDAPATAN VOUCHER SUPPLIER', '', 0, '1', '1', NULL, NULL, 1, '1'),(53230, 13, 1, '1.303', 'REPACKING ULANG', '', 0, '1', '1', NULL, NULL, 2, '1'),(53231, 24, 1, '6.141', 'PENDAPATAN BONUS PEMBELIAN', '', 0, '0', '1', NULL, NULL, 2, '0'),(53232, 11, 1, '1.131', 'KAS KOPEGTEL', '', 0, '1', '1', NULL, NULL, 1, '1'),(53233, 11, 1, '1.132', 'KAS SAMALIANDA', '', 0, '1', '1', NULL, NULL, 1, '1'),(53234, 18, 1, '3.107', 'UANG MUKA KOPEGTEL', '', 0, '0', '1', NULL, NULL, 1, '1'),(53235, 18, 1, '3.108', 'UANG MUKA SAMALIANDA', '', 0, '0', '1', NULL, NULL, 1, '1'),(53236, 13, 1, '1.304', 'STOK OPNAME BELUM DIBEBANKAN', '', 0, '1', '0', NULL, NULL, 0, '1'),(53237, 11, 1, '1.123', 'CIMB NIAGA', '', 0, '1', '1', NULL, NULL, 3, '1'),(53238, 27, 1, '5.128', 'BIAYA KRESEK', '', 0, '1', '1', NULL, NULL, 2, '0'),(53239, 27, 1, '5.129', 'BIAYA AIR GALON', '', 0, '1', '1', NULL, NULL, 2, '0'),(53240, 27, 1, '5.13', 'BIAYA MAKANAN KUCING', '', 0, '1', '1', NULL, NULL, 2, '0'),(53241, 24, 1, '6.142', 'PENDAPATAN PEMBETULAN BARCODE / STOK', '', 0, '0', '0', NULL, NULL, 0, '0'),(53242, 27, 1, '5.131', 'LAKBAN / ISOLASI', '', 0, '1', '1', NULL, NULL, 2, '0'),(53243, 27, 1, '5.135', 'BIAYA KEPERLUAN OB', '', 0, '1', '1', NULL, NULL, 2, '0'),(53244, 23, 1, '6.224', 'PITA PRINTER', '', 0, '1', '1', NULL, NULL, 2, '0'),(53245, 23, 1, '6.223', 'BIAYA RABAT / KOMISI', '', 0, '1', '0', NULL, NULL, 0, '0'),(53246, 27, 1, '5.132', 'PLASTIK', '', 0, '1', '0', NULL, NULL, 0, '0'),(53247, 13, 1, '1.305', 'SELISIH PERSEDIAAN RETUR', '', 0, '1', '0', NULL, NULL, 0, '1'),(53248, 27, 1, '5.133', 'BAYAR LISTRIK', '', 0, '1', '0', NULL, NULL, 0, '0'),(53249, 27, 1, '5.136', 'LAP MOBIL / SERBET ', '', 0, '1', '0', NULL, NULL, 0, '0'),(53250, 27, 1, '5.137', 'BIAYA KARDUS', '', 0, '1', '0', NULL, NULL, 0, '0'),(53251, 27, 1, '5.138', 'SABUN ', '', 0, '1', '0', NULL, NULL, 0, '0'),(53252, 28, 1, '11.103', 'KAS BANK / SETORAN KASIR', '', 0, '1', '1', NULL, NULL, 1, '1'),(53253, 28, 1, '11.104', 'KAS KECIL RAYA', '', 0, '1', '1', NULL, NULL, 1, '1'),(5205, 23, 1, '5.105', 'ONGKOS KANVAS', '', 0, '1', '0', '', 'NULL', 0, '0');
COMMIT;
BEGIN;
LOCK TABLE "public"."mjenistransaksi" IN SHARE MODE;
DELETE FROM "public"."mjenistransaksi";
INSERT INTO "public"."mjenistransaksi" ("id","nourut","kode","nama","keterangan") VALUES (101, 101, 'Jurnal Umum', 'Jurnal Umum', 'Jurnal Umum'),(102, 102, 'Kas/Bank Masuk', 'Kas/Bank Masuk', 'Kas/Bank Masuk'),(103, 103, 'Kas/Bank Keluar', 'Kas/Bank Keluar', 'Kas/Bank Keluar');
COMMIT;
BEGIN;
LOCK TABLE "public"."mjenistransaksid" IN SHARE MODE;
DELETE FROM "public"."mjenistransaksid";
INSERT INTO "public"."mjenistransaksid" ("nourut","idjenistransaksi","jenis","prefix","sufix","digit","format") VALUES (10101, 101, 'Jurnal Umum', 'JU', NULL, 5, '[YYYY][MM]/[00000]'),(10201, 102, 'Kas Masuk', 'KM', NULL, 5, '[YYYY][MM]/[00000]'),(10202, 102, 'Bank Masuk', 'BM', NULL, 5, '[YYYY][MM]/[00000]'),(10301, 103, 'Kas Keluar', 'KK', NULL, 5, '[YYYY][MM]/[00000]'),(10302, 103, 'Bank Keluar', 'BK', NULL, 5, '[YYYY][MM]/[00000]');
COMMIT;
BEGIN;
LOCK TABLE "public"."mjurnal" IN SHARE MODE;
DELETE FROM "public"."mjurnal";
COMMIT;
BEGIN;
LOCK TABLE "public"."mjurnalumum" IN SHARE MODE;
DELETE FROM "public"."mjurnalumum";
COMMIT;
BEGIN;
LOCK TABLE "public"."mjurnalumumd" IN SHARE MODE;
DELETE FROM "public"."mjurnalumumd";
COMMIT;
BEGIN;
LOCK TABLE "public"."mkasin" IN SHARE MODE;
DELETE FROM "public"."mkasin";
COMMIT;
BEGIN;
LOCK TABLE "public"."mkasind" IN SHARE MODE;
DELETE FROM "public"."mkasind";
COMMIT;
BEGIN;
LOCK TABLE "public"."mkasindbayar" IN SHARE MODE;
DELETE FROM "public"."mkasindbayar";
COMMIT;
BEGIN;
LOCK TABLE "public"."mkasout" IN SHARE MODE;
DELETE FROM "public"."mkasout";
COMMIT;
BEGIN;
LOCK TABLE "public"."mkasoutd" IN SHARE MODE;
DELETE FROM "public"."mkasoutd";
COMMIT;
BEGIN;
LOCK TABLE "public"."mkasoutdbayar" IN SHARE MODE;
DELETE FROM "public"."mkasoutdbayar";
COMMIT;
BEGIN;
LOCK TABLE "public"."mkontak" IN SHARE MODE;
DELETE FROM "public"."mkontak";
INSERT INTO "public"."mkontak" ("id","kode","nama","alamat1","alamat2","alamat3","hp","telpon","iswhatsapp","norekening","bank","atasnamarekening") VALUES (1, 'SYSADM', 'SYSADM', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
COMMIT;
BEGIN;
LOCK TABLE "public"."mrole" IN SHARE MODE;
DELETE FROM "public"."mrole";
INSERT INTO "public"."mrole" ("id","role","issupervisor") VALUES (1, 'SYSADM', '1'),(2, 'ADM', '1'),(3, 'USER', '0');
COMMIT;
BEGIN;
LOCK TABLE "public"."muser" IN SHARE MODE;
DELETE FROM "public"."muser";
INSERT INTO "public"."muser" ("id","userid","pwd","nama","idkontak","idrole") VALUES (1, 'SYSADM', '80333F920AF5D4BA09970A81D43E2411', 'SYSADM', 1, 1);
COMMIT;
ALTER TABLE "__EFMigrationsHistory" ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");
ALTER TABLE "makun" ADD CONSTRAINT "makun_pkey" PRIMARY KEY ("id");
CREATE INDEX "makun_idxidparent" ON "makun" USING btree (
  "idparent" "pg_catalog"."int4_ops" ASC NULLS LAST
);
CREATE UNIQUE INDEX "makun_idxkode" ON "makun" USING btree (
  "kode" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
ALTER TABLE "mjenistransaksi" ADD CONSTRAINT "mjenistransaksi_pkey" PRIMARY KEY ("id");
CREATE UNIQUE INDEX "mjenistransaksi_idxkode" ON "mjenistransaksi" USING btree (
  "kode" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
CREATE UNIQUE INDEX "mjenistransaksi_idxnourut" ON "mjenistransaksi" USING btree (
  "nourut" "pg_catalog"."int2_ops" ASC NULLS LAST
);
ALTER TABLE "mjenistransaksid" ADD CONSTRAINT "mjenistransaksid_pkey" PRIMARY KEY ("nourut", "idjenistransaksi");
ALTER TABLE "mjurnal" ADD CONSTRAINT "mjurnal_pkey" PRIMARY KEY ("idtransaksi", "idjenistransaksid", "idpasangan", "idakun", "iddepartemen");
ALTER TABLE "mjurnalumum" ADD CONSTRAINT "mjurnalumum_pkey" PRIMARY KEY ("id");
CREATE UNIQUE INDEX "idx_mjurnalumum_kode" ON "mjurnalumum" USING btree (
  "kode" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
ALTER TABLE "mjurnalumumd" ADD CONSTRAINT "mjurnalumumd_pkey" PRIMARY KEY ("id", "idjurnalumum");
ALTER TABLE "mkasin" ADD CONSTRAINT "mkasin_pkey" PRIMARY KEY ("id");
CREATE UNIQUE INDEX "idx_mkasin_kode" ON "mkasin" USING btree (
  "kode" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
ALTER TABLE "mkasind" ADD CONSTRAINT "mkasind_pkey" PRIMARY KEY ("id", "idkasin");
ALTER TABLE "mkasindbayar" ADD CONSTRAINT "mkasindbayar_pkey" PRIMARY KEY ("id", "idkasin");
ALTER TABLE "mkasout" ADD CONSTRAINT "mkasout_pkey" PRIMARY KEY ("id");
CREATE UNIQUE INDEX "idx_mkasout_kode" ON "mkasout" USING btree (
  "kode" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
ALTER TABLE "mkasoutd" ADD CONSTRAINT "mkasoutd_pkey" PRIMARY KEY ("id", "idkasout");
ALTER TABLE "mkasoutdbayar" ADD CONSTRAINT "mkasoutdbayar_pkey" PRIMARY KEY ("id", "idkasout");
ALTER TABLE "mkontak" ADD CONSTRAINT "mkontak_pkey" PRIMARY KEY ("id");
ALTER TABLE "mrole" ADD CONSTRAINT "mrole_pkey" PRIMARY KEY ("id");
ALTER TABLE "muser" ADD CONSTRAINT "muser_pkey" PRIMARY KEY ("id");
CREATE UNIQUE INDEX "idx_muser_userid" ON "muser" USING btree (
  "userid" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
ALTER TABLE "mjurnalumumd" ADD CONSTRAINT "fk_mjurnalumum_idakun" FOREIGN KEY ("idakun") REFERENCES "public"."makun" ("id") ON DELETE RESTRICT ON UPDATE CASCADE;
ALTER TABLE "mjurnalumumd" ADD CONSTRAINT "fk_mjurnalumumd_idjurnalumum" FOREIGN KEY ("idjurnalumum") REFERENCES "public"."mjurnalumum" ("id") ON DELETE RESTRICT ON UPDATE CASCADE;
ALTER TABLE "mkasin" ADD CONSTRAINT "fk_mkasin_idakun" FOREIGN KEY ("idakun") REFERENCES "public"."makun" ("id") ON DELETE RESTRICT ON UPDATE CASCADE;
ALTER TABLE "mkasind" ADD CONSTRAINT "fk_mkasind_idakun" FOREIGN KEY ("idakun") REFERENCES "public"."makun" ("id") ON DELETE RESTRICT ON UPDATE CASCADE;
ALTER TABLE "mkasind" ADD CONSTRAINT "fk_mkasind_idkasin" FOREIGN KEY ("idkasin") REFERENCES "public"."mkasin" ("id") ON DELETE RESTRICT ON UPDATE CASCADE;
ALTER TABLE "mkasindbayar" ADD CONSTRAINT "fk_mkasindbayar_idakun" FOREIGN KEY ("idakun") REFERENCES "public"."makun" ("id") ON DELETE RESTRICT ON UPDATE CASCADE;
ALTER TABLE "mkasindbayar" ADD CONSTRAINT "fk_mkasindbayar_idkasin" FOREIGN KEY ("idkasin") REFERENCES "public"."mkasin" ("id") ON DELETE RESTRICT ON UPDATE CASCADE;
ALTER TABLE "mkasindbayar" ADD CONSTRAINT "fk_mkasindbayar_idkontak" FOREIGN KEY ("idkontak") REFERENCES "public"."mkontak" ("id") ON DELETE RESTRICT ON UPDATE CASCADE;
ALTER TABLE "mkasout" ADD CONSTRAINT "fk_mkasout_idakun" FOREIGN KEY ("idakun") REFERENCES "public"."makun" ("id") ON DELETE RESTRICT ON UPDATE CASCADE;
ALTER TABLE "mkasoutd" ADD CONSTRAINT "fk_mkasoutd_idakun" FOREIGN KEY ("idakun") REFERENCES "public"."makun" ("id") ON DELETE RESTRICT ON UPDATE CASCADE;
ALTER TABLE "mkasoutd" ADD CONSTRAINT "fk_mkasoutd_idkasout" FOREIGN KEY ("idkasout") REFERENCES "public"."mkasout" ("id") ON DELETE RESTRICT ON UPDATE CASCADE;
ALTER TABLE "mkasoutdbayar" ADD CONSTRAINT "fk_mkasoutdbayar_idakun" FOREIGN KEY ("idakun") REFERENCES "public"."makun" ("id") ON DELETE RESTRICT ON UPDATE CASCADE;
ALTER TABLE "mkasoutdbayar" ADD CONSTRAINT "fk_mkasoutdbayar_idkasout" FOREIGN KEY ("idkasout") REFERENCES "public"."mkasout" ("id") ON DELETE RESTRICT ON UPDATE CASCADE;
ALTER TABLE "mkasoutdbayar" ADD CONSTRAINT "fk_mkasoutdbayar_idkontak" FOREIGN KEY ("idkontak") REFERENCES "public"."mkontak" ("id") ON DELETE RESTRICT ON UPDATE CASCADE;
ALTER TABLE "muser" ADD CONSTRAINT "fk_muser_idkontak" FOREIGN KEY ("idkontak") REFERENCES "public"."mkontak" ("id") ON DELETE RESTRICT ON UPDATE CASCADE;
ALTER TABLE "muser" ADD CONSTRAINT "fk_muser_idrole" FOREIGN KEY ("idrole") REFERENCES "public"."mrole" ("id") ON DELETE RESTRICT ON UPDATE CASCADE;

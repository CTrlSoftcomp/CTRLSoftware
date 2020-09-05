Imports System.Data
Imports System.Data.SqlClient
Imports CtrlSoft.Utils
Imports DevExpress.XtraEditors

Namespace Repository
    Public Class RepUpdateDB
        Private Shared Sub ExecUpdateDB(ByVal ListUpdate As List(Of Model.UpdateDB))
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
                                If NullToLong(com.ExecuteScalar()) = 0 Then
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
                                XtraMessageBox.Show("Error Update " & Obj.DBVersion & vbCrLf & "Reason : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                                com.Transaction.Rollback()
                            End Try
                        Next
                    Catch ex As Exception
                        XtraMessageBox.Show("Ada Kesalahan :" & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End Try
                End Using
            End Using
        End Sub

        Public Shared Sub UpdateDB()
            Dim iList As New List(Of Model.UpdateDB)
            iList.AddRange(ListUpdate_July_2020)

            ExecUpdateDB(iList)
        End Sub

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
#End Region
    End Class
End Namespace

Namespace Model
    Public Class UpdateDB
        Private _DBVersion As String
        Private _TglUpdate As Date
        Private _SQL As String

        Public Property DBVersion() As String
            Get
                Return _DBVersion
            End Get
            Set(ByVal value As String)
                _DBVersion = value
            End Set
        End Property

        Public Property TglUpdate() As Date
            Get
                Return _TglUpdate
            End Get
            Set(ByVal value As Date)
                _TglUpdate = value
            End Set
        End Property

        Public Property SQL() As String
            Get
                Return _SQL
            End Get
            Set(ByVal value As String)
                _SQL = value
            End Set
        End Property

        Public Sub New()
            Me.TglUpdate = Now()
            Me.SQL = ""
            Me.DBVersion = ""
        End Sub
    End Class
End Namespace
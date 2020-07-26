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
                                com.Parameters.Add(New SqlParameter("@SQL", SqlDbType.VarChar)).Value = Obj.SQL
                                If NullToLong(com.ExecuteScalar()) = 0 Then
                                    com.CommandText = "INSERT INTO TAppDB([DBVersion], [Tanggal]) VALUES (@DBVersion, @Tanggal)"
                                    com.ExecuteNonQuery()

                                    com.CommandText = Obj.SQL
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
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
Imports System.Data.SQLite
Imports Dapper
Imports System.IO
Imports System.Linq

Namespace Cache
    Public Class RepCache
        Implements ICache

        Private NamaFileDB As String = Environment.CurrentDirectory & "\System\Cache.db"
        Private StrKonSQL As String = "Data Source=" & NamaFileDB & ";Version=3;New=True;Compress=True;"

        Public Sub New()
            SeederDatabase()
        End Sub

        Public Sub SeederDatabase()
            Dim cn As New SQLiteConnection(StrKonSQL)
            Dim com As New SQLiteCommand
            Dim IsAdaDB As Boolean = False
            Try
                If Not File.Exists(NamaFileDB) Then
                    IsAdaDB = False
                Else
                    IsAdaDB = True
                End If

                cn.Open()
                com.Connection = cn
                com.CommandTimeout = cn.ConnectionTimeout

                If Not IsAdaDB Then
                    com.CommandText = "CREATE TABLE cache (" & vbCrLf &
                                      "[Type] TEXT Not NULL," & vbCrLf &
                                      "[Id] TEXT NOT NULL," & vbCrLf &
                                      "[Count] NUMBER," & vbCrLf &
                                      "PRIMARY KEY ([Type], [Id])" & vbCrLf &
                                      ")"
                    com.ExecuteNonQuery()

                    com.CommandText = "create table _dbmigration ([DBVersion] TEXT PRIMARY KEY)"
                    com.ExecuteNonQuery()

                    com.CommandText = "INSERT INTO _dbMigration (DBVersion) VALUES ('1.0')"
                    com.ExecuteNonQuery()
                End If
            Catch ex As Exception
                MessageBox.Show("Error : " & ex.Message, Environment.MachineName)
            Finally
                If com IsNot Nothing Then
                    com.Dispose()
                End If
                If cn IsNot Nothing Then
                    cn.Dispose()
                End If
            End Try
        End Sub

        Public Function Cache(Type As String, Id As String) As MCache Implements ICache.Cache
            Dim Obj As New MCache
            Dim Args As New Dictionary(Of String, Object)
            Using db As New SQLiteConnection(StrKonSQL)
                Try
                    db.Open()
                    Args.Add("@Type", Type)
                    Args.Add("@Id", Id)
                    Obj = db.Query(Of MCache)("SELECT [Type], [Id], [Count] FROM Cache WHERE [Type]=@Type AND [Id]=@Id", New DynamicParameters(Args)).SingleOrDefault()
                Catch ex As Exception
                    MessageBox.Show("Error : " & ex.Message, Environment.MachineName)
                End Try
            End Using
            Return Obj
        End Function

        Public Function Save(Type As String, Id As String) As MCache Implements ICache.Save
            Dim Obj As New MCache
            Dim Args As New Dictionary(Of String, Object)
            Using db As New SQLiteConnection(StrKonSQL)
                Try
                    db.Open()

                    Args.Add("@Type", Type)
                    Args.Add("@Id", Id)
                    Obj = db.Query(Of MCache)("SELECT [Type], [Id], [Count] FROM Cache WHERE [Type]=@Type AND [Id]=@Id", New DynamicParameters(Args)).SingleOrDefault()
                    If Obj IsNot Nothing Then
                        db.Execute("UPDATE Cache SET [Count]=[Count]+1 WHERE [Type]=@Type AND [Id]=@Id", New DynamicParameters(Args))
                    Else
                        db.Execute("INSERT INTO Cache ([Type], [Id], [Count]) VALUES (@Type, @Id, 1)", New DynamicParameters(Args))
                    End If
                Catch ex As Exception
                    MessageBox.Show("Error : " & ex.Message, Environment.MachineName)
                End Try
            End Using
            Return Obj
        End Function

        Public Function Delete(Type As String, Id As String) As MCache Implements ICache.Delete
            Dim Obj As New MCache
            Dim Args As New Dictionary(Of String, Object)
            Using db As New SQLiteConnection(StrKonSQL)
                Try
                    db.Open()

                    Args.Add("@Type", Type)
                    Args.Add("@Id", Id)
                    Obj = db.Query(Of MCache)("SELECT [Type], [Id], [Count] FROM Cache WHERE [Type]=@Type AND [Id]=@Id", New DynamicParameters(Args)).SingleOrDefault()
                    If Obj IsNot Nothing Then
                        db.Execute("DELETE FROM Cache WHERE [Type]=@Type AND [Id]=@Id", New DynamicParameters(Args))
                    End If
                Catch ex As Exception
                    MessageBox.Show("Error : " & ex.Message, Environment.MachineName)
                End Try
            End Using
            Return Obj
        End Function

        Public Function List(Type As String, Filter As String) As List(Of MCache) Implements ICache.List
            Dim Obj As New List(Of MCache)
            Dim Args As New Dictionary(Of String, Object)
            Using db As New SQLiteConnection(StrKonSQL)
                Try
                    db.Open()

                    Args.Add("@Type", Type)
                    Args.Add("@Filter", "%" & Filter & "%")
                    Obj = db.Query(Of MCache)("SELECT [Type], [Id], [Count] FROM Cache WHERE [Type]=@Type AND [Id] LIKE @Filter", New DynamicParameters(Args)).ToList()
                Catch ex As Exception
                    MessageBox.Show("Error : " & ex.Message, Environment.MachineName)
                End Try
            End Using
            Return Obj
        End Function
    End Class
End Namespace

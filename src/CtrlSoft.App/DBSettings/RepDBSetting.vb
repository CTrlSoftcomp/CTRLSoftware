Imports System.Data.SQLite
Imports Dapper
Imports System.IO
Imports System.Linq
Imports CtrlSoft.App.Ini

Namespace DBSettings
    Public Class RepDBSetting
        Implements IDBSetting

        Public Shared NamaFileDB As String = Environment.CurrentDirectory & "\System\DBSetting.db"
        Private StrKonSQL As String = "Data Source=" & NamaFileDB & ";Version=3;New=True;Compress=True;"

        Public Sub New()
            SeederDatabase(False)
        End Sub

        Public Sub SeederDatabase(ByVal CreateDB As Boolean)
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
                    com.CommandText = "CREATE TABLE DBSetting (" & vbCrLf &
                                      "[Id] TEXT Not NULL," & vbCrLf &
                                      "[Server] TEXT NOT NULL," & vbCrLf &
                                      "[Database] TEXT NOT NULL," & vbCrLf &
                                      "[UserID] TEXT NOT NULL," & vbCrLf &
                                      "[Password] TEXT NOT NULL," & vbCrLf &
                                      "[Timeout] NUMBER NOT NULL," & vbCrLf &
                                      "[Default] BIT NOT NULL," & vbCrLf &
                                      "PRIMARY KEY ([Id])" & vbCrLf &
                                      ")"
                    com.ExecuteNonQuery()

                    com.CommandText = "create table _dbmigration ([DBVersion] TEXT PRIMARY KEY)"
                    com.ExecuteNonQuery()

                    com.CommandText = "INSERT INTO _dbMigration (DBVersion) VALUES ('1.0')"
                    com.ExecuteNonQuery()

                    If CreateDB Then
                        com.CommandText = "INSERT INTO DBSetting ([Id], [Server], [Database], [UserID], [Password], [Timeout], [Default]) VALUES (@Id, @Server, @Database, @UserID, @Password, @Timeout, 1)"
                        com.Parameters.Clear()
                        com.Parameters.Add(New SQLiteParameter("@Id", "DEFAULT"))
                        com.Parameters.Add(New SQLiteParameter("@Server", BacaIni("DBConfig", "Server", "(localdb)\MSSQLLocalDB")))
                        com.Parameters.Add(New SQLiteParameter("@Database", BacaIni("DBConfig", "Database", "dbpos")))
                        com.Parameters.Add(New SQLiteParameter("@UserID", BacaIni("DBConfig", "Username", "sa")))
                        com.Parameters.Add(New SQLiteParameter("@Password", BacaIni("DBConfig", "Password", "1234123412")))
                        com.Parameters.Add(New SQLiteParameter("@Timeout", BacaIni("DBConfig", "Timeout", "15")))
                        com.ExecuteNonQuery()
                    End If
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

        Public Function [Get](Id As String) As MDBSetting Implements IDBSetting.Get
            Dim Obj As New MDBSetting
            Dim Args As New Dictionary(Of String, Object)
            Using db As New SQLiteConnection(StrKonSQL)
                Try
                    db.Open()
                    Args.Add("@Id", Id)
                    Obj = db.Query(Of MDBSetting)("SELECT * FROM DBSetting WHERE [Id]=@Id", New DynamicParameters(Args)).SingleOrDefault()
                Catch ex As Exception
                    MessageBox.Show("Error : " & ex.Message, Environment.MachineName)
                End Try
            End Using
            Return Obj
        End Function

        Public Function Save(Model As MDBSetting) As MDBSetting Implements IDBSetting.Save
            Dim Obj As New MDBSetting
            Dim Args As New Dictionary(Of String, Object)
            Using db As New SQLiteConnection(StrKonSQL)
                Try
                    db.Open()

                    If IsDBNull(Model.Id) OrElse Model.Id.Length = 0 Then
                        Model.Id = Guid.NewGuid().ToString()
                    End If
                    Args.Add("@Id", Model.Id)
                    Args.Add("@Server", Model.Server)
                    Args.Add("@Database", Model.Database)
                    Args.Add("@UserID", Model.UserID)
                    Args.Add("@Password", Model.Password)
                    Args.Add("@TimeOut", Model.Timeout)
                    Args.Add("@Default", Model.Default)

                    If Model.Default Then
                        db.Execute("UPDATE DBSetting SET [Default]=0", Nothing)
                    End If

                    Obj = db.Query(Of MDBSetting)("SELECT * FROM DBSetting WHERE [Id]=@Id", New DynamicParameters(Args)).SingleOrDefault()
                    If Obj IsNot Nothing Then
                        db.Execute("UPDATE DBSetting SET [Server]=@Server, [Database]=@Database, [UserID]=@UserID, [Password]=@Password, [Timeout]=@Timeout, [Default]=@Default WHERE [Id]=@Id", New DynamicParameters(Args))
                    Else
                        db.Execute("INSERT INTO DBSetting ([Id], [Server], [Database], [UserID], [Password], [Timeout], [Default]) VALUES (@Id, @Server, @Database, @UserID, @Password, @Timeout, @Default)", New DynamicParameters(Args))
                    End If

                    Dim CheckDefault = db.Query(Of MDBSetting)("SELECT * FROM DBSetting WHERE [Default]=1", Nothing).ToList()
                    If CheckDefault.Count = 0 Then
                        db.Execute("UPDATE DBSetting SET [Server]=@Server, [Database]=@Database, [UserID]=@UserID, [Password]=@Password, [Timeout]=@Timeout, [Default]=1 WHERE [Id]=@Id", New DynamicParameters(Args))
                    End If
                Catch ex As Exception
                    MessageBox.Show("Error : " & ex.Message, Environment.MachineName)
                End Try
            End Using
            Return Obj
        End Function

        Public Function Delete(Id As String) As MDBSetting Implements IDBSetting.Delete
            Dim Obj As New MDBSetting
            Dim Args As New Dictionary(Of String, Object)
            Using db As New SQLiteConnection(StrKonSQL)
                Try
                    db.Open()

                    Args.Add("@Id", Id)
                    Obj = db.Query(Of MDBSetting)("SELECT * FROM DBSetting WHERE [Id]=@Id", New DynamicParameters(Args)).SingleOrDefault()
                    If Obj IsNot Nothing Then
                        db.Execute("DELETE FROM DBSetting WHERE [Id]=@Id", New DynamicParameters(Args))
                    End If
                Catch ex As Exception
                    MessageBox.Show("Error : " & ex.Message, Environment.MachineName)
                End Try
            End Using
            Return Obj
        End Function

        Public Function List() As List(Of MDBSetting) Implements IDBSetting.List
            Dim Obj As New List(Of MDBSetting)
            Dim Args As New Dictionary(Of String, Object)
            Using db As New SQLiteConnection(StrKonSQL)
                Try
                    db.Open()

                    Obj = db.Query(Of MDBSetting)("SELECT * FROM DBSetting", New DynamicParameters(Args)).ToList().OrderByDescending(Function(m) m.Default).OrderBy(Function(m) m.Server).ToList()
                Catch ex As Exception
                    MessageBox.Show("Error : " & ex.Message, Environment.MachineName)
                End Try
            End Using
            Return Obj
        End Function
    End Class
End Namespace

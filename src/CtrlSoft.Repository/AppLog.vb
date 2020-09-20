Imports System.Data.SqlClient
Imports System.Data
Imports CtrlSoft.Dto.ViewModel

Namespace Repository
    Public Class AppLog
        Public Shared Sub InsertLog(ByVal StrKonSQL As String,
                                    ByVal [AppLog] As CtrlSoft.Dto.ViewModel.AppLog,
                                    ByVal [JSON] As JSONResult)
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Try
                        cn.Open()
                        com.Connection = cn
                        com.CommandTimeout = cn.ConnectionTimeout
                        com.Transaction = cn.BeginTransaction

                        com.CommandText = "INSERT INTO [DBCtrlLog].dbo.MAppLog ([AppName],[Date],[Event],[User],[JSON]) VALUES (@AppName,@Date,@Event,@User,@JSON)"
                        com.Parameters.Clear()
                        com.Parameters.Add(New SqlParameter("@AppName", System.Data.SqlDbType.VarChar)).Value = AppLog.AppName
                        com.Parameters.Add(New SqlParameter("@Date", System.Data.SqlDbType.DateTime)).Value = AppLog.Date
                        com.Parameters.Add(New SqlParameter("@Event", System.Data.SqlDbType.VarChar)).Value = AppLog.Event
                        com.Parameters.Add(New SqlParameter("@User", System.Data.SqlDbType.VarChar)).Value = AppLog.User
                        com.Parameters.Add(New SqlParameter("@JSON", System.Data.SqlDbType.VarChar)).Value = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)
                        com.ExecuteNonQuery()

                        com.Transaction.Commit()
                    Catch ex As Exception
                        InsertLocalLog(AppLog, JSON)
                    End Try
                End Using
            End Using
        End Sub

        Private Shared Sub InsertLocalLog(ByVal [AppLog] As CtrlSoft.Dto.ViewModel.AppLog,
                                          ByVal [JSON] As JSONResult)
            Dim FolderLog As String = Environment.CurrentDirectory & "\AppLog\"
            Dim FileLog As String = FolderLog & Now.ToString("yyMMdd") & ".log"
            Dim Append As Boolean = False
            Try
                If Not System.IO.Directory.Exists(FolderLog) Then
                    System.IO.Directory.CreateDirectory(FolderLog)
                End If
                Append = System.IO.File.Exists(FileLog)
                Using myStream As New System.IO.StreamWriter(FileLog, Append)
                    AppLog.JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)
                    myStream.WriteLine("[" & Now.ToString("yy-MM-dd HH:mm:ss") & "] - Failed Create Log on [DBCtrlLog].dbo.MAppLog - " & Newtonsoft.Json.JsonConvert.SerializeObject(AppLog))
                    myStream.Flush()
                    myStream.Close()
                End Using
            Catch ex As Exception

            End Try
        End Sub
    End Class
End Namespace

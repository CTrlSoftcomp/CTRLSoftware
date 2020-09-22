Imports System.Data
Imports System.Data.SqlClient
Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.Dto.Model
Imports CtrlSoft.Dto.ViewModel

Namespace Repository
    Public Class RepConfig
        Public Shared Function GetSettingPerusahaan(ByVal StrKonSQL As String) As JSONResult
            Dim JSON As New JSONResult
            Dim Hasil As SettingPerusahaan = Nothing
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using ds As New DataSet
                            Try
                                cn.Open()
                                com.Connection = cn
                                oDA.SelectCommand = com

                                com.CommandText = "SELECT * FROM MSetting"
                                oDA.Fill(ds, "MSetting")

                                Hasil = New SettingPerusahaan
                                For Each iRow As DataRow In ds.Tables("MSetting").Rows
                                    Select Case iRow.Item("KeySettings").ToString.ToLower
                                        Case "NamaPerusahaan".ToLower
                                            Hasil.NamaPerusahaan = iRow.Item("KeyValue")
                                        Case "AlamatPerusahaan".ToLower
                                            Hasil.AlamatPerusahaan = iRow.Item("KeyValue")
                                        Case "KotaPerusahaan".ToLower
                                            Hasil.KotaPerusahaan = iRow.Item("KeyValue")
                                        Case "PathLayouts".ToLower
                                            Hasil.PathLayouts = iRow.Item("KeyValue")
                                    End Select
                                Next
                                With JSON
                                    .JSONMessage = "Data Ditemukan"
                                    .JSONResult = True
                                    .JSONRows = 1
                                    .JSONValue = Hasil
                                End With
                            Catch ex As Exception
                                With JSON
                                    .JSONMessage = ex.Message
                                    .JSONResult = False
                                End With
                            End Try
                        End Using
                    End Using
                End Using
            End Using

            Return JSON
        End Function
        Public Shared Function SetSettingPerusahaan(ByVal StrKonSQL As String, ByVal Obj As SettingPerusahaan) As JSONResult
            Dim JSON As New JSONResult
            Dim Hasil As SettingPerusahaan = Nothing
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using ds As New DataSet
                            Try
                                cn.Open()
                                com.Connection = cn
                                oDA.SelectCommand = com
                                com.Transaction = com.Connection.BeginTransaction

                                com.CommandText = "IF EXISTS (SELECT KeySettings FROM MSetting WHERE KeySettings=@KeySettings)" & vbCrLf &
                                                      "UPDATE MSetting SET KeyValue=@KeyValue WHERE KeySettings=@KeySettings" & vbCrLf &
                                                      "ELSE" & vbCrLf &
                                                      "INSERT INTO MSetting (KeySettings, KeyValue) VALUES (@KeySettings, @KeyValue)"
                                com.Parameters.Clear()
                                com.Parameters.Add(New SqlParameter("@KeySettings", SqlDbType.VarChar)).Value = "NamaPerusahaan"
                                com.Parameters.Add(New SqlParameter("@KeyValue", SqlDbType.VarChar)).Value = Obj.NamaPerusahaan
                                com.ExecuteNonQuery()

                                com.CommandText = "IF EXISTS (SELECT KeySettings FROM MSetting WHERE KeySettings=@KeySettings)" & vbCrLf &
                                                      "UPDATE MSetting SET KeyValue=@KeyValue WHERE KeySettings=@KeySettings" & vbCrLf &
                                                      "ELSE" & vbCrLf &
                                                      "INSERT INTO MSetting (KeySettings, KeyValue) VALUES (@KeySettings, @KeyValue)"
                                com.Parameters.Clear()
                                com.Parameters.Add(New SqlParameter("@KeySettings", SqlDbType.VarChar)).Value = "KotaPerusahaan"
                                com.Parameters.Add(New SqlParameter("@KeyValue", SqlDbType.VarChar)).Value = Obj.KotaPerusahaan
                                com.ExecuteNonQuery()

                                com.CommandText = "IF EXISTS (SELECT KeySettings FROM MSetting WHERE KeySettings=@KeySettings)" & vbCrLf &
                                                      "UPDATE MSetting SET KeyValue=@KeyValue WHERE KeySettings=@KeySettings" & vbCrLf &
                                                      "ELSE" & vbCrLf &
                                                      "INSERT INTO MSetting (KeySettings, KeyValue) VALUES (@KeySettings, @KeyValue)"
                                com.Parameters.Clear()
                                com.Parameters.Add(New SqlParameter("@KeySettings", SqlDbType.VarChar)).Value = "AlamatPerusahaan"
                                com.Parameters.Add(New SqlParameter("@KeyValue", SqlDbType.VarChar)).Value = Obj.AlamatPerusahaan
                                com.ExecuteNonQuery()

                                com.CommandText = "IF EXISTS (SELECT KeySettings FROM MSetting WHERE KeySettings=@KeySettings)" & vbCrLf &
                                                      "UPDATE MSetting SET KeyValue=@KeyValue WHERE KeySettings=@KeySettings" & vbCrLf &
                                                      "ELSE" & vbCrLf &
                                                      "INSERT INTO MSetting (KeySettings, KeyValue) VALUES (@KeySettings, @KeyValue)"
                                com.Parameters.Clear()
                                com.Parameters.Add(New SqlParameter("@KeySettings", SqlDbType.VarChar)).Value = "PathLayouts"
                                com.Parameters.Add(New SqlParameter("@KeyValue", SqlDbType.VarChar)).Value = Obj.PathLayouts
                                com.ExecuteNonQuery()

                                com.Transaction.Commit()

                                Hasil = Obj
                                With JSON
                                    .JSONMessage = "Data Ditemukan"
                                    .JSONResult = True
                                    .JSONRows = 1
                                    .JSONValue = Hasil
                                End With
                            Catch ex As Exception
                                With JSON
                                    .JSONMessage = ex.Message
                                    .JSONResult = False
                                End With
                                Hasil = Nothing
                            End Try
                        End Using
                    End Using
                End Using
            End Using

            Return JSON
        End Function
    End Class
End Namespace

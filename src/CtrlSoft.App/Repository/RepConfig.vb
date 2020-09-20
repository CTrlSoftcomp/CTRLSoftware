Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.App.Public

Namespace Repository
    Public Class RepConfig
        Public Shared Function GetSettingPerusahaan() As Model.SettingPerusahaan
            Dim Hasil As Model.SettingPerusahaan = Nothing
            Using dlg As New WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
                Using cn As New SqlConnection(StrKonSQL)
                    Using com As New SqlCommand
                        Using oDA As New SqlDataAdapter
                            Using ds As New DataSet
                                Try
                                    dlg.Show()
                                    dlg.Focus()
                                    cn.Open()
                                    com.Connection = cn
                                    oDA.SelectCommand = com

                                    com.CommandText = "SELECT * FROM MSetting"
                                    oDA.Fill(ds, "MSetting")

                                    Hasil = New Model.SettingPerusahaan
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
                                Catch ex As Exception
                                    XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    Hasil = New Model.SettingPerusahaan
                                End Try
                            End Using
                        End Using
                    End Using
                End Using
            End Using

            Return Hasil
        End Function
        Public Shared Function SetSettingPerusahaan(ByVal Obj As Model.SettingPerusahaan) As Model.SettingPerusahaan
            Dim Hasil As Model.SettingPerusahaan = Nothing
            Using dlg As New WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
                Using cn As New SqlConnection(StrKonSQL)
                    Using com As New SqlCommand
                        Using oDA As New SqlDataAdapter
                            Using ds As New DataSet
                                Try
                                    dlg.Show()
                                    dlg.Focus()
                                    cn.Open()
                                    com.Connection = cn
                                    oDA.SelectCommand = com
                                    com.Transaction = com.Connection.BeginTransaction

                                    com.CommandText = "IF EXISTS (SELECT KeySettings FROM MSetting WHERE KeySettings=@KeySettings)" & vbCrLf & _
                                                      "UPDATE MSetting SET KeyValue=@KeyValue WHERE KeySettings=@KeySettings" & vbCrLf & _
                                                      "ELSE" & vbCrLf & _
                                                      "INSERT INTO MSetting (KeySettings, KeyValue) VALUES (@KeySettings, @KeyValue)"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@KeySettings", SqlDbType.VarChar)).Value = "NamaPerusahaan"
                                    com.Parameters.Add(New SqlParameter("@KeyValue", SqlDbType.VarChar)).Value = SettingPerusahaan.NamaPerusahaan
                                    com.ExecuteNonQuery()

                                    com.CommandText = "IF EXISTS (SELECT KeySettings FROM MSetting WHERE KeySettings=@KeySettings)" & vbCrLf & _
                                                      "UPDATE MSetting SET KeyValue=@KeyValue WHERE KeySettings=@KeySettings" & vbCrLf & _
                                                      "ELSE" & vbCrLf & _
                                                      "INSERT INTO MSetting (KeySettings, KeyValue) VALUES (@KeySettings, @KeyValue)"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@KeySettings", SqlDbType.VarChar)).Value = "KotaPerusahaan"
                                    com.Parameters.Add(New SqlParameter("@KeyValue", SqlDbType.VarChar)).Value = SettingPerusahaan.KotaPerusahaan
                                    com.ExecuteNonQuery()

                                    com.CommandText = "IF EXISTS (SELECT KeySettings FROM MSetting WHERE KeySettings=@KeySettings)" & vbCrLf & _
                                                      "UPDATE MSetting SET KeyValue=@KeyValue WHERE KeySettings=@KeySettings" & vbCrLf & _
                                                      "ELSE" & vbCrLf & _
                                                      "INSERT INTO MSetting (KeySettings, KeyValue) VALUES (@KeySettings, @KeyValue)"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@KeySettings", SqlDbType.VarChar)).Value = "AlamatPerusahaan"
                                    com.Parameters.Add(New SqlParameter("@KeyValue", SqlDbType.VarChar)).Value = SettingPerusahaan.AlamatPerusahaan
                                    com.ExecuteNonQuery()

                                    com.CommandText = "IF EXISTS (SELECT KeySettings FROM MSetting WHERE KeySettings=@KeySettings)" & vbCrLf & _
                                                      "UPDATE MSetting SET KeyValue=@KeyValue WHERE KeySettings=@KeySettings" & vbCrLf & _
                                                      "ELSE" & vbCrLf & _
                                                      "INSERT INTO MSetting (KeySettings, KeyValue) VALUES (@KeySettings, @KeyValue)"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@KeySettings", SqlDbType.VarChar)).Value = "PathLayouts"
                                    com.Parameters.Add(New SqlParameter("@KeyValue", SqlDbType.VarChar)).Value = SettingPerusahaan.PathLayouts
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()

                                    Hasil = Obj
                                Catch ex As Exception
                                    XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    Hasil = Nothing
                                End Try
                            End Using
                        End Using
                    End Using
                End Using
            End Using

            Return Hasil
        End Function
    End Class
End Namespace

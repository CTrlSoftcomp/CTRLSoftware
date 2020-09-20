Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.App.Public

Namespace Repository
    Public Class RepKode
        Public Enum [Format]
            A0000 = 0
            A00000 = 1
        End Enum

        Public Shared Function GetNewKode(ByVal NamaTabel As String, ByVal Kolom As String, ByVal Prefix As String, ByVal Sufix As String, ByVal Format As Format) As String
            Dim Hasil As String = ""
            Dim NoUrut As Long = -1
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using ds As New DataSet
                            Try
                                cn.Open()
                                com.Connection = cn
                                com.CommandTimeout = cn.ConnectionTimeout
                                oDA.SelectCommand = com

                                com.CommandText = "SELECT CONVERT(BIGINT, MAX(CASE WHEN ISNUMERIC(SUBSTRING([" & Kolom & "], LEN('" & Prefix & "')+1, LEN('" & Format.ToString.Replace("A", "") & "')))=1 THEN " & vbCrLf & _
                                                  "SUBSTRING([" & Kolom & "], LEN('" & Prefix & "')+1, LEN('" & Format.ToString.Replace("A", "") & "')) ELSE '0' END)) AS MaxKode" & vbCrLf & _
                                                  "FROM [" & NamaTabel & "] " & vbCrLf & _
                                                  "WHERE SUBSTRING([" & Kolom & "], 1, LEN('" & Prefix & "'))='" & Prefix & "' AND RIGHT([" & Kolom & "], LEN('" & Sufix & "'))='" & Sufix & "'"
                                oDA.Fill(ds, NamaTabel)
                                If ds.Tables(NamaTabel).Rows.Count >= 1 Then
                                    NoUrut = NullToLong(ds.Tables(NamaTabel).Rows(0).Item("MaxKode")) + 1
                                Else
                                    NoUrut = 1
                                End If

                                Hasil = Prefix & Microsoft.VisualBasic.Strings.Format(NoUrut, Format.ToString.Replace("A", "")) & Sufix
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    End Using
                End Using
            End Using

            Return Hasil
        End Function
    End Class
End Namespace

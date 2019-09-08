Imports CtrlSoft.Utils
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors
Imports System.Data
Imports System.Data.SqlClient

Namespace Repository
    Public Class UnPostingData
        Public Shared Function UnPostingPO(ByVal NoID As Long) As Boolean
            Dim Hasil As Boolean = False
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using ds As New DataSet
                            Try
                                cn.Open()
                                com.Connection = cn
                                com.CommandTimeout = cn.ConnectionTimeout
                                com.Transaction = com.Connection.BeginTransaction
                                oDA.SelectCommand = com

                                com.CommandText = "SELECT COUNT(MBeliD.NoID) FROM MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli INNER JOIN MPOD ON MPOD.NoID=MBeliD.IDPOD WHERE MPOD.IDHeader=" & NoID
                                If NullToLong(com.ExecuteScalar()) = 0 Then
                                    com.CommandText = "UPDATE IsPosted=0, TglPosted=NULL, IDUserPosted=NULL WHERE NoID=" & NoID
                                    com.ExecuteNonQuery()

                                    com.Transaction.Commit()
                                    Hasil = True
                                End If
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

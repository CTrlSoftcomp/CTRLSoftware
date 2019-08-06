Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports CtrlSoft.Utils

Namespace Repository
    Public Class RepMenu
        Public Shared Function GetLogin(ByVal Kode As String, ByVal Pwd As String) As Model.User
            Dim User As Model.User = Nothing
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

                                    com.CommandText = "SELECT TOP 1 MUser.*, MRole.Role, MRole.IsSupervisor FROM MUser INNER JOIN MRole ON MRole.NoID=MUser.IDRole WHERE MUser.Kode=@Kode AND MUser.Pwd=@Pwd"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = Kode
                                    com.Parameters.Add(New SqlParameter("@Pwd", SqlDbType.VarChar)).Value = Pwd

                                    oDA.Fill(ds, "MUser")
                                    For Each iRow As DataRow In ds.Tables("MUser").Rows
                                        User = New Model.User With {.NoID = NullToLong(iRow.Item("NoID")), _
                                                                     .Nama = NullToStr(iRow.Item("Nama")), _
                                                                     .Kode = NullToStr(iRow.Item("Kode")), _
                                                                     .Role = NullToStr(iRow.Item("Role")), _
                                                                     .Supervisor = NullToBool(iRow.Item("IsSupervisor"))}
                                        com.CommandText = "SELECT dbo.MMenu.NoID, dbo.MMenu.IDParent, dbo.MMenu.NoUrut, dbo.MMenu.Name, dbo.MMenu.Caption, dbo.MMenu.IsBig, dbo.MMenu.IsBeginGroup, CONVERT(BIT, CASE WHEN dbo.MMenu.IsActive=1 AND dbo.MRoleD.IsActive=1 THEN 1 ELSE 0 END) IsActive" & vbCrLf & _
                                                          " FROM dbo.MUser " & vbCrLf & _
                                                          " INNER JOIN dbo.MRoleD ON dbo.MUser.IDRole = dbo.MRoleD.IDRole " & vbCrLf & _
                                                          " RIGHT JOIN dbo.MMenu ON dbo.MRoleD.IDMenu = dbo.MMenu.NoID " & vbCrLf & _
                                                          " WHERE dbo.MMenu.IDParent=-1 AND CONVERT(BIT, CASE WHEN dbo.MMenu.IsActive=1 AND dbo.MRoleD.IsActive=1 THEN 1 ELSE 0 END)=1 AND dbo.MUser.NoID=@IDUser " & vbCrLf & _
                                                          " ORDER BY dbo.MMenu.NoUrut"
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@IDUser", SqlDbType.BigInt)).Value = User.NoID
                                        If Not ds.Tables("MUserD") Is Nothing Then
                                            ds.Tables("MUserD").Clear()
                                            ds.Tables("MUserD").Columns.Clear()
                                        End If
                                        oDA.Fill(ds, "MUserD")
                                        For Each aRow As DataRow In ds.Tables("MUserD").Rows
                                            Dim Menu As New Model.Menu With {.NoID = NullToLong(aRow.Item("NoID")), _
                                                                               .Name = NullToStr(aRow.Item("Name")), _
                                                                               .Caption = NullToStr(aRow.Item("Caption")), _
                                                                               .Visible = NullToBool(aRow.Item("IsActive"))}
                                            com.CommandText = "SELECT dbo.MMenu.NoID, dbo.MMenu.IDParent, dbo.MMenu.NoUrut, dbo.MMenu.Name, dbo.MMenu.Caption, dbo.MMenu.IsBig, dbo.MMenu.IsBeginGroup, CONVERT(BIT, CASE WHEN dbo.MMenu.IsActive=1 AND dbo.MRoleD.IsActive=1 THEN 1 ELSE 0 END) IsActive" & vbCrLf & _
                                                          " FROM dbo.MUser " & vbCrLf & _
                                                          " INNER JOIN dbo.MRoleD ON dbo.MUser.IDRole = dbo.MRoleD.IDRole " & vbCrLf & _
                                                          " RIGHT JOIN dbo.MMenu ON dbo.MRoleD.IDMenu = dbo.MMenu.NoID " & vbCrLf & _
                                                          " WHERE dbo.MMenu.IDParent=@IDParent AND CONVERT(BIT, CASE WHEN dbo.MMenu.IsActive=1 AND dbo.MRoleD.IsActive=1 THEN 1 ELSE 0 END)=1 AND dbo.MUser.NoID=@IDUser " & vbCrLf & _
                                                          " ORDER BY dbo.MMenu.NoUrut"
                                            com.Parameters.Clear()
                                            com.Parameters.Add(New SqlParameter("@IDUser", SqlDbType.BigInt)).Value = User.NoID
                                            com.Parameters.Add(New SqlParameter("@IDParent", SqlDbType.BigInt)).Value = Menu.NoID
                                            If Not ds.Tables("MUserDD") Is Nothing Then
                                                ds.Tables("MUserDD").Clear()
                                                ds.Tables("MUserDD").Columns.Clear()
                                            End If
                                            oDA.Fill(ds, "MUserDD")
                                            For Each bRow As DataRow In ds.Tables("MUserDD").Rows
                                                Menu.SubMenu.Add(New Model.SubMenu With {.NoID = NullToLong(bRow.Item("NoID")), _
                                                                                         .Name = NullToStr(bRow.Item("Name")), _
                                                                                         .Caption = NullToStr(bRow.Item("Caption")), _
                                                                                         .BigMenu = NullToBool(bRow.Item("IsBig")), _
                                                                                         .BeginGroup = NullToBool(bRow.Item("IsBeginGroup")), _
                                                                                         .Visible = NullToBool(bRow.Item("IsActive"))})
                                            Next
                                            User.Menu.Add(Menu)

                                            Application.DoEvents()
                                        Next
                                        Application.DoEvents()
                                    Next
                                Catch ex As Exception
                                    XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    User = Nothing
                                End Try
                            End Using
                        End Using
                    End Using
                End Using
            End Using
            Return User
        End Function
        Public Shared Function GetOtorisasiSPV(ByVal Kode As String, ByVal Pwd As String) As Model.User
            Dim User As Model.User = Nothing
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

                                    com.CommandText = "SELECT TOP 1 MUser.*, MRole.Role, MRole.IsSupervisor FROM MUser INNER JOIN MRole ON MRole.NoID=MUser.IDRole WHERE MRole.IsSupervisor=1 AND MUser.Kode=@Kode AND MUser.Pwd=@Pwd"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = Kode
                                    com.Parameters.Add(New SqlParameter("@Pwd", SqlDbType.VarChar)).Value = Pwd

                                    oDA.Fill(ds, "MUser")
                                    For Each iRow As DataRow In ds.Tables("MUser").Rows
                                        User = New Model.User With {.NoID = NullToLong(iRow.Item("NoID")), _
                                                                     .Nama = NullToStr(iRow.Item("Nama")), _
                                                                     .Kode = NullToStr(iRow.Item("Kode")), _
                                                                     .Role = NullToStr(iRow.Item("Role")), _
                                                                     .Supervisor = NullToBool(iRow.Item("IsSupervisor"))}
                                        com.CommandText = "SELECT dbo.MMenu.NoID, dbo.MMenu.IDParent, dbo.MMenu.NoUrut, dbo.MMenu.Name, dbo.MMenu.Caption, dbo.MMenu.IsBig, dbo.MMenu.IsBeginGroup, CONVERT(BIT, CASE WHEN dbo.MMenu.IsActive=1 AND dbo.MRoleD.IsActive=1 THEN 1 ELSE 0 END) IsActive" & vbCrLf & _
                                                          " FROM dbo.MUser " & vbCrLf & _
                                                          " INNER JOIN dbo.MRoleD ON dbo.MUser.IDRole = dbo.MRoleD.IDRole " & vbCrLf & _
                                                          " RIGHT JOIN dbo.MMenu ON dbo.MRoleD.IDMenu = dbo.MMenu.NoID " & vbCrLf & _
                                                          " WHERE dbo.MMenu.IDParent=-1 AND CONVERT(BIT, CASE WHEN dbo.MMenu.IsActive=1 AND dbo.MRoleD.IsActive=1 THEN 1 ELSE 0 END)=1 AND dbo.MUser.NoID=@IDUser " & vbCrLf & _
                                                          " ORDER BY dbo.MMenu.NoUrut"
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@IDUser", SqlDbType.BigInt)).Value = User.NoID
                                        If Not ds.Tables("MUserD") Is Nothing Then
                                            ds.Tables("MUserD").Clear()
                                            ds.Tables("MUserD").Columns.Clear()
                                        End If
                                        oDA.Fill(ds, "MUserD")
                                        For Each aRow As DataRow In ds.Tables("MUserD").Rows
                                            Dim Menu As New Model.Menu With {.NoID = NullToLong(aRow.Item("NoID")), _
                                                                               .Name = NullToStr(aRow.Item("Name")), _
                                                                               .Caption = NullToStr(aRow.Item("Caption")), _
                                                                               .Visible = NullToBool(aRow.Item("IsActive"))}
                                            com.CommandText = "SELECT dbo.MMenu.NoID, dbo.MMenu.IDParent, dbo.MMenu.NoUrut, dbo.MMenu.Name, dbo.MMenu.Caption, dbo.MMenu.IsBig, dbo.MMenu.IsBeginGroup, CONVERT(BIT, CASE WHEN dbo.MMenu.IsActive=1 AND dbo.MRoleD.IsActive=1 THEN 1 ELSE 0 END) IsActive" & vbCrLf & _
                                                          " FROM dbo.MUser " & vbCrLf & _
                                                          " INNER JOIN dbo.MRoleD ON dbo.MUser.IDRole = dbo.MRoleD.IDRole " & vbCrLf & _
                                                          " RIGHT JOIN dbo.MMenu ON dbo.MRoleD.IDMenu = dbo.MMenu.NoID " & vbCrLf & _
                                                          " WHERE dbo.MMenu.IDParent=@IDParent AND CONVERT(BIT, CASE WHEN dbo.MMenu.IsActive=1 AND dbo.MRoleD.IsActive=1 THEN 1 ELSE 0 END)=1 AND dbo.MUser.NoID=@IDUser " & vbCrLf & _
                                                          " ORDER BY dbo.MMenu.NoUrut"
                                            com.Parameters.Clear()
                                            com.Parameters.Add(New SqlParameter("@IDUser", SqlDbType.BigInt)).Value = User.NoID
                                            com.Parameters.Add(New SqlParameter("@IDParent", SqlDbType.BigInt)).Value = Menu.NoID
                                            If Not ds.Tables("MUserDD") Is Nothing Then
                                                ds.Tables("MUserDD").Clear()
                                                ds.Tables("MUserDD").Columns.Clear()
                                            End If
                                            oDA.Fill(ds, "MUserDD")
                                            For Each bRow As DataRow In ds.Tables("MUserDD").Rows
                                                Menu.SubMenu.Add(New Model.SubMenu With {.NoID = NullToLong(bRow.Item("NoID")), _
                                                                                         .Name = NullToStr(bRow.Item("Name")), _
                                                                                         .Caption = NullToStr(bRow.Item("Caption")), _
                                                                                         .BigMenu = NullToBool(bRow.Item("IsBig")), _
                                                                                         .BeginGroup = NullToBool(bRow.Item("IsBeginGroup")), _
                                                                                         .Visible = NullToBool(bRow.Item("IsActive"))})
                                            Next
                                            User.Menu.Add(Menu)

                                            Application.DoEvents()
                                        Next
                                        Application.DoEvents()
                                    Next
                                Catch ex As Exception
                                    XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    User = Nothing
                                End Try
                            End Using
                        End Using
                    End Using
                End Using
            End Using
            Return User
        End Function
        Public Shared Function GetTimeServer() As Date
            Dim Hasil As Date = Now
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

                                    com.CommandText = "DECLARE @TZ SMALLINT; " & vbCrLf & _
                                                 "SELECT @TZ=DATEPART(TZ, SYSDATETIMEOFFSET()); " & vbCrLf & _
                                                 "SELECT DATEADD(HOUR, -1*@TZ/60, GETDATE()) AS Tanggal"
                                    Hasil = com.ExecuteScalar()
                                Catch ex As Exception
                                    XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                End Try
                            End Using
                        End Using
                    End Using
                End Using
            End Using
        End Function
    End Class
End Namespace
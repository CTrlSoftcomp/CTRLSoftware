Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports CtrlSoft.Utils

Namespace Repository
    Public Class RepSQLServer
        Public Shared Function GetKodeKontak(ByVal tipe As Integer) As String
            'All = 0
            'Customer = 1
            'Supplier = 2
            'Pegawai = 3
            Dim Hasil As String = "", NoUrut As Long = -1
            Using dlg As New WaitDialogForm("Sedang mengambil data ...", NamaAplikasi)
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

                                    com.CommandText = "SELECT CONVERT(BIGINT, MAX(RIGHT(Kode, 3))) FROM MAlamat WHERE ISNUMERIC(RIGHT(Kode, 3))=1 AND SUBSTRING(Kode, 1, LEN(@Kode))=@Kode"
                                    Select Case tipe
                                        Case 1
                                            Hasil = "CUST-"
                                        Case 2
                                            Hasil = "SUP-"
                                        Case 3
                                            Hasil = "KRY-"
                                        Case Else
                                            Hasil = "KRY-"
                                    End Select
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = Hasil
                                    NoUrut = NullToLong(com.ExecuteScalar) + 1
                                    Hasil &= NoUrut.ToString("00000")

                                    Application.DoEvents()
                                Catch ex As Exception
                                    XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    Hasil = ""
                                End Try
                            End Using
                        End Using
                    End Using
                End Using
            End Using
            Return Hasil
        End Function
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

                                    'Proses Update Struktur
                                    Repository.RepUpdateDB.UpdateDB()

                                    com.CommandText = "SELECT TOP 1 MUser.*, MRole.Role, MRole.IsSupervisor FROM MUser INNER JOIN MRole ON MRole.NoID=MUser.IDRole WHERE MUser.Kode=@Kode AND MUser.Pwd=@Pwd"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = Kode
                                    com.Parameters.Add(New SqlParameter("@Pwd", SqlDbType.VarChar)).Value = Pwd

                                    oDA.Fill(ds, "MUser")
                                    For Each iRow As DataRow In ds.Tables("MUser").Rows
                                        User = New Model.User With {.TanggalSystem = Now(), _
                                                                    .NoID = NullToLong(iRow.Item("NoID")), _
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
                                    Hasil = Now()
                                End Try
                            End Using
                        End Using
                    End Using
                End Using
            End Using

            Return Hasil
        End Function
        Public Shared Function GetListKategori() As List(Of Model.Core)
            Dim Hasil As New List(Of Model.Core)
            Using dlg As New WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
                Using cn As New SqlConnection(StrKonSQL)
                    Using com As New SqlCommand
                        Using oDA As New SqlDataAdapter
                            Using dt As New DataTable
                                Try
                                    dlg.Show()
                                    dlg.Focus()
                                    cn.Open()
                                    com.Connection = cn
                                    oDA.SelectCommand = com

                                    com.CommandText = "SELECT * FROM MKategori(NOLOCK) WHERE IsActive=1 AND NoID NOT IN (SELECT IDParent FROM MKategori(NOLOCK))"
                                    oDA.Fill(dt)

                                    For Each iRow As DataRow In dt.Rows
                                        Hasil.Add(New Model.Core With {.NoID = NullToLong(iRow.Item("NoID")), _
                                                                       .Kode = NullToStr(iRow.Item("Kode")), _
                                                                       .Nama = NullToStr(iRow.Item("Nama")) & "-" & NullToStr(iRow.Item("Nama"))})
                                    Next
                                Catch ex As Exception
                                    XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    Hasil.Clear()
                                End Try
                            End Using
                        End Using
                    End Using
                End Using
            End Using

            Return Hasil
        End Function
        Public Shared Function GetListDepartemen() As List(Of Model.Core)
            Dim Hasil As New List(Of Model.Core)
            Using dlg As New WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
                Using cn As New SqlConnection(StrKonSQL)
                    Using com As New SqlCommand
                        Using oDA As New SqlDataAdapter
                            Using dt As New DataTable
                                Try
                                    dlg.Show()
                                    dlg.Focus()
                                    cn.Open()
                                    com.Connection = cn
                                    oDA.SelectCommand = com

                                    com.CommandText = "SELECT * FROM MKategori(NOLOCK) WHERE IsActive=1 AND NoID IN (SELECT IDParent FROM MKategori(NOLOCK))"
                                    oDA.Fill(dt)

                                    For Each iRow As DataRow In dt.Rows
                                        Hasil.Add(New Model.Core With {.NoID = NullToLong(iRow.Item("NoID")), _
                                                                       .Kode = NullToStr(iRow.Item("Kode")), _
                                                                       .Nama = NullToStr(iRow.Item("Nama")) & "-" & NullToStr(iRow.Item("Nama"))})
                                    Next
                                Catch ex As Exception
                                    XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    Hasil.Clear()
                                End Try
                            End Using
                        End Using
                    End Using
                End Using
            End Using

            Return Hasil
        End Function
        Public Shared Function GetListSupplier() As List(Of Model.Core)
            Dim Hasil As New List(Of Model.Core)
            Using dlg As New WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
                Using cn As New SqlConnection(StrKonSQL)
                    Using com As New SqlCommand
                        Using oDA As New SqlDataAdapter
                            Using dt As New DataTable
                                Try
                                    dlg.Show()
                                    dlg.Focus()
                                    cn.Open()
                                    com.Connection = cn
                                    oDA.SelectCommand = com

                                    com.CommandText = "SELECT NoID, Kode, Nama FROM MAlamat(NOLOCK) WHERE IsActive=1 AND IsSupplier=1"
                                    oDA.Fill(dt)

                                    For Each iRow As DataRow In dt.Rows
                                        Hasil.Add(New Model.Core With {.NoID = NullToLong(iRow.Item("NoID")), _
                                                                       .Kode = NullToStr(iRow.Item("Kode")), _
                                                                       .Nama = NullToStr(iRow.Item("Nama")) & "-" & NullToStr(iRow.Item("Nama"))})
                                    Next
                                Catch ex As Exception
                                    XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    Hasil.Clear()
                                End Try
                            End Using
                        End Using
                    End Using
                End Using
            End Using

            Return Hasil
        End Function
        Public Shared Function GetListMerk() As List(Of Model.Core)
            Dim Hasil As New List(Of Model.Core)
            Using dlg As New WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
                Using cn As New SqlConnection(StrKonSQL)
                    Using com As New SqlCommand
                        Using oDA As New SqlDataAdapter
                            Using dt As New DataTable
                                Try
                                    dlg.Show()
                                    dlg.Focus()
                                    cn.Open()
                                    com.Connection = cn
                                    oDA.SelectCommand = com

                                    com.CommandText = "SELECT NoID, Kode, Nama FROM MMerk(NOLOCK) WHERE IsActive=1"
                                    oDA.Fill(dt)

                                    For Each iRow As DataRow In dt.Rows
                                        Hasil.Add(New Model.Core With {.NoID = NullToLong(iRow.Item("NoID")), _
                                                                       .Kode = NullToStr(iRow.Item("Kode")), _
                                                                       .Nama = NullToStr(iRow.Item("Nama")) & "-" & NullToStr(iRow.Item("Nama"))})
                                    Next
                                Catch ex As Exception
                                    XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    Hasil.Clear()
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
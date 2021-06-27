Imports System.Data
Imports System.Data.SqlClient
Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.Repository.Repository.AppLog
Imports CtrlSoft.Dto.Model
Imports CtrlSoft.Dto.ViewModel

Namespace Repository
    Public Class RepSQLServer
        Public Shared Function GetKodeMaterial(ByVal StrKonSQL As String) As JSONResult
            Dim JSON As New JSONResult
            Dim Hasil As String = "", NoUrut As Long = -1
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using ds As New System.Data.DataSet
                            Try
                                cn.Open()
                                com.Connection = cn
                                oDA.SelectCommand = com

                                com.CommandText = "SELECT CONVERT(BIGINT, MAX(RIGHT(Kode, 3))) FROM MMaterial WHERE ISNUMERIC(RIGHT(Kode, 3))=1 AND SUBSTRING(Kode, 1, LEN(@Kode))=@Kode"
                                Hasil = "RAW-"
                                com.Parameters.Clear()
                                com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = Hasil
                                NoUrut = NullToLong(com.ExecuteScalar) + 1
                                Hasil &= NoUrut.ToString("00000")
                                With JSON
                                    .JSONMessage = "Berhasil"
                                    .JSONResult = True
                                    .JSONValue = Hasil
                                End With
                            Catch ex As Exception
                                With JSON
                                    .JSONMessage = ex.Message
                                    .JSONResult = False
                                End With
                                InsertLog(StrKonSQL, New CtrlSoft.Dto.ViewModel.AppLog With {
                                          .AppName = My.Application.Info.AssemblyName,
                                          .Date = Now,
                                          .Event = "GetKodeMaterial",
                                          .User = "Unknow",
                                          .JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)},
                                      JSON)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return JSON
        End Function
        Public Shared Function GetKodeKontak(ByVal StrKonSQL As String,
                                             ByVal tipe As Integer) As JSONResult
            'All = 0
            'Customer = 1
            'Supplier = 2
            'Pegawai = 3
            Dim JSON As New JSONResult
            Dim Hasil As String = "", NoUrut As Long = -1
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using ds As New System.Data.DataSet
                            Try
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

                                With JSON
                                    .JSONMessage = "Berhasil"
                                    .JSONResult = True
                                    .JSONValue = Hasil
                                End With
                            Catch ex As Exception
                                With JSON
                                    .JSONMessage = ex.Message
                                    .JSONResult = False
                                End With
                                InsertLog(StrKonSQL, New CtrlSoft.Dto.ViewModel.AppLog With {
                                          .AppName = My.Application.Info.AssemblyName,
                                          .Date = Now,
                                          .Event = "GetKodeKontak",
                                          .User = "Unknow",
                                          .JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)},
                                      JSON)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return JSON
        End Function
        Public Shared Function GetLogin(ByVal StrKonSQL As String,
                                        ByVal Kode As String,
                                        ByVal Pwd As String) As JSONResult
            Dim JSON As New JSONResult
            Dim User As CtrlSoft.Dto.Model.MUser = Nothing
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using ds As New System.Data.DataSet
                            Try
                                cn.Open()
                                com.Connection = cn
                                oDA.SelectCommand = com

                                'Proses Update Struktur
                                CtrlSoft.UpdateDB.Repository.RepUpdateDB.UpdateDB(StrKonSQL)

                                com.CommandText = "SELECT TOP 1 MUser.*, MRole.Role, MRole.IsSupervisor FROM MUser INNER JOIN MRole ON MRole.NoID=MUser.IDRole WHERE MUser.Kode=@Kode AND MUser.Pwd=@Pwd"
                                com.Parameters.Clear()
                                com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = Kode
                                com.Parameters.Add(New SqlParameter("@Pwd", SqlDbType.VarChar)).Value = Pwd

                                oDA.Fill(ds, "MUser")
                                For Each iRow As DataRow In ds.Tables("MUser").Rows
                                    User = New MUser With {.TanggalSystem = Now(),
                                                                    .NoID = NullToLong(iRow.Item("NoID")),
                                                                    .Nama = NullToStr(iRow.Item("Nama")),
                                                                    .Kode = NullToStr(iRow.Item("Kode")),
                                                                    .Role = NullToStr(iRow.Item("Role")),
                                                                    .Supervisor = NullToBool(iRow.Item("IsSupervisor"))}
                                    com.CommandText = "SELECT dbo.MMenu.NoID, dbo.MMenu.IDParent, dbo.MMenu.NoUrut, dbo.MMenu.Name, dbo.MMenu.Caption, dbo.MMenu.IsBig, dbo.MMenu.IsBeginGroup, CONVERT(BIT, CASE WHEN dbo.MMenu.IsActive=1 AND dbo.MRoleD.IsActive=1 THEN 1 ELSE 0 END) IsActive" & vbCrLf &
                                                          " FROM dbo.MUser " & vbCrLf &
                                                          " INNER JOIN dbo.MRoleD ON dbo.MUser.IDRole = dbo.MRoleD.IDRole " & vbCrLf &
                                                          " RIGHT JOIN dbo.MMenu ON dbo.MRoleD.IDMenu = dbo.MMenu.NoID " & vbCrLf &
                                                          " WHERE dbo.MMenu.IDParent=-1 AND CONVERT(BIT, CASE WHEN dbo.MMenu.IsActive=1 AND dbo.MRoleD.IsActive=1 THEN 1 ELSE 0 END)=1 AND dbo.MUser.NoID=@IDUser " & vbCrLf &
                                                          " ORDER BY dbo.MMenu.NoUrut"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@IDUser", SqlDbType.BigInt)).Value = User.NoID
                                    If Not ds.Tables("MUserD") Is Nothing Then
                                        ds.Tables("MUserD").Clear()
                                        ds.Tables("MUserD").Columns.Clear()
                                    End If
                                    oDA.Fill(ds, "MUserD")
                                    For Each aRow As DataRow In ds.Tables("MUserD").Rows
                                        Dim Menu As New Menu With {.NoID = NullToLong(aRow.Item("NoID")),
                                                                       .Name = NullToStr(aRow.Item("Name")),
                                                                       .Caption = NullToStr(aRow.Item("Caption")),
                                                                       .Visible = NullToBool(aRow.Item("IsActive"))}
                                        com.CommandText = "SELECT dbo.MMenu.NoID, dbo.MMenu.IDParent, dbo.MMenu.NoUrut, dbo.MMenu.Name, dbo.MMenu.Caption, dbo.MMenu.IsBig, dbo.MMenu.IsBeginGroup, CONVERT(BIT, CASE WHEN dbo.MMenu.IsActive=1 AND dbo.MRoleD.IsActive=1 THEN 1 ELSE 0 END) IsActive" & vbCrLf &
                                                          " FROM dbo.MUser " & vbCrLf &
                                                          " INNER JOIN dbo.MRoleD ON dbo.MUser.IDRole = dbo.MRoleD.IDRole " & vbCrLf &
                                                          " RIGHT JOIN dbo.MMenu ON dbo.MRoleD.IDMenu = dbo.MMenu.NoID " & vbCrLf &
                                                          " WHERE dbo.MMenu.IDParent=@IDParent AND CONVERT(BIT, CASE WHEN dbo.MMenu.IsActive=1 AND dbo.MRoleD.IsActive=1 THEN 1 ELSE 0 END)=1 AND dbo.MUser.NoID=@IDUser " & vbCrLf &
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
                                            Menu.SubMenu.Add(New SubMenu With {.NoID = NullToLong(bRow.Item("NoID")),
                                                                                         .Name = NullToStr(bRow.Item("Name")),
                                                                                         .Caption = NullToStr(bRow.Item("Caption")),
                                                                                         .BigMenu = NullToBool(bRow.Item("IsBig")),
                                                                                         .BeginGroup = NullToBool(bRow.Item("IsBeginGroup")),
                                                                                         .Visible = NullToBool(bRow.Item("IsActive"))})
                                        Next
                                        User.Menu.Add(Menu)
                                    Next

                                    With JSON
                                        .JSONMessage = "Data ditemukan"
                                        .JSONResult = True
                                        .JSONRows = 1
                                        .JSONValue = User
                                    End With
                                Next
                            Catch ex As Exception
                                With JSON
                                    .JSONMessage = ex.Message
                                    .JSONResult = False
                                End With
                                InsertLog(StrKonSQL, New CtrlSoft.Dto.ViewModel.AppLog With {
                                          .AppName = My.Application.Info.AssemblyName,
                                          .Date = Now,
                                          .Event = "GetLogin",
                                          .User = "Unknow",
                                          .JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)},
                                      JSON)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return JSON
        End Function
        Public Shared Function GetOtorisasiSPV(ByVal StrKonSQL As String,
                                               ByVal Kode As String,
                                               ByVal Pwd As String) As JSONResult
            Dim JSON As New JSONResult
            Dim User As CtrlSoft.Dto.Model.MUser = Nothing
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using ds As New System.Data.DataSet
                            Try
                                cn.Open()
                                com.Connection = cn
                                oDA.SelectCommand = com

                                com.CommandText = "SELECT TOP 1 MUser.*, MRole.Role, MRole.IsSupervisor FROM MUser INNER JOIN MRole ON MRole.NoID=MUser.IDRole WHERE MRole.IsSupervisor=1 AND MUser.Kode=@Kode AND MUser.Pwd=@Pwd"
                                com.Parameters.Clear()
                                com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = Kode
                                com.Parameters.Add(New SqlParameter("@Pwd", SqlDbType.VarChar)).Value = Pwd

                                oDA.Fill(ds, "MUser")
                                For Each iRow As DataRow In ds.Tables("MUser").Rows
                                    User = New MUser With {.NoID = NullToLong(iRow.Item("NoID")),
                                                                     .Nama = NullToStr(iRow.Item("Nama")),
                                                                     .Kode = NullToStr(iRow.Item("Kode")),
                                                                     .Role = NullToStr(iRow.Item("Role")),
                                                                     .Supervisor = NullToBool(iRow.Item("IsSupervisor"))}
                                    com.CommandText = "SELECT dbo.MMenu.NoID, dbo.MMenu.IDParent, dbo.MMenu.NoUrut, dbo.MMenu.Name, dbo.MMenu.Caption, dbo.MMenu.IsBig, dbo.MMenu.IsBeginGroup, CONVERT(BIT, CASE WHEN dbo.MMenu.IsActive=1 AND dbo.MRoleD.IsActive=1 THEN 1 ELSE 0 END) IsActive" & vbCrLf &
                                                          " FROM dbo.MUser " & vbCrLf &
                                                          " INNER JOIN dbo.MRoleD ON dbo.MUser.IDRole = dbo.MRoleD.IDRole " & vbCrLf &
                                                          " RIGHT JOIN dbo.MMenu ON dbo.MRoleD.IDMenu = dbo.MMenu.NoID " & vbCrLf &
                                                          " WHERE dbo.MMenu.IDParent=-1 AND CONVERT(BIT, CASE WHEN dbo.MMenu.IsActive=1 AND dbo.MRoleD.IsActive=1 THEN 1 ELSE 0 END)=1 AND dbo.MUser.NoID=@IDUser " & vbCrLf &
                                                          " ORDER BY dbo.MMenu.NoUrut"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@IDUser", SqlDbType.BigInt)).Value = User.NoID
                                    If Not ds.Tables("MUserD") Is Nothing Then
                                        ds.Tables("MUserD").Clear()
                                        ds.Tables("MUserD").Columns.Clear()
                                    End If
                                    oDA.Fill(ds, "MUserD")
                                    For Each aRow As DataRow In ds.Tables("MUserD").Rows
                                        Dim Menu As New Menu With {.NoID = NullToLong(aRow.Item("NoID")),
                                                                       .Name = NullToStr(aRow.Item("Name")),
                                                                       .Caption = NullToStr(aRow.Item("Caption")),
                                                                       .Visible = NullToBool(aRow.Item("IsActive"))}
                                        com.CommandText = "SELECT dbo.MMenu.NoID, dbo.MMenu.IDParent, dbo.MMenu.NoUrut, dbo.MMenu.Name, dbo.MMenu.Caption, dbo.MMenu.IsBig, dbo.MMenu.IsBeginGroup, CONVERT(BIT, CASE WHEN dbo.MMenu.IsActive=1 AND dbo.MRoleD.IsActive=1 THEN 1 ELSE 0 END) IsActive" & vbCrLf &
                                                          " FROM dbo.MUser " & vbCrLf &
                                                          " INNER JOIN dbo.MRoleD ON dbo.MUser.IDRole = dbo.MRoleD.IDRole " & vbCrLf &
                                                          " RIGHT JOIN dbo.MMenu ON dbo.MRoleD.IDMenu = dbo.MMenu.NoID " & vbCrLf &
                                                          " WHERE dbo.MMenu.IDParent=@IDParent AND CONVERT(BIT, CASE WHEN dbo.MMenu.IsActive=1 AND dbo.MRoleD.IsActive=1 THEN 1 ELSE 0 END)=1 AND dbo.MUser.NoID=@IDUser " & vbCrLf &
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
                                            Menu.SubMenu.Add(New SubMenu With {.NoID = NullToLong(bRow.Item("NoID")),
                                                                                         .Name = NullToStr(bRow.Item("Name")),
                                                                                         .Caption = NullToStr(bRow.Item("Caption")),
                                                                                         .BigMenu = NullToBool(bRow.Item("IsBig")),
                                                                                         .BeginGroup = NullToBool(bRow.Item("IsBeginGroup")),
                                                                                         .Visible = NullToBool(bRow.Item("IsActive"))})
                                        Next
                                        User.Menu.Add(Menu)
                                    Next
                                    With JSON
                                        .JSONMessage = "Data ditemukan"
                                        .JSONResult = True
                                        .JSONRows = 1
                                        .JSONValue = User
                                    End With
                                Next
                            Catch ex As Exception
                                With JSON
                                    .JSONMessage = ex.Message
                                    .JSONResult = False
                                End With
                                InsertLog(StrKonSQL, New CtrlSoft.Dto.ViewModel.AppLog With {
                                          .AppName = My.Application.Info.AssemblyName,
                                          .Date = Now,
                                          .Event = "GetOtorisasiSPV",
                                          .User = "Unknow",
                                          .JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)},
                                      JSON)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
            Return JSON
        End Function
        Public Shared Function GetTimeServer(ByVal StrKonSQL As String) As JSONResult
            Dim Hasil As Date = Now
            Dim JSON As New JSONResult
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using ds As New System.Data.DataSet
                            Try
                                cn.Open()
                                com.Connection = cn
                                oDA.SelectCommand = com

                                com.CommandText = "DECLARE @TZ SMALLINT; " & vbCrLf &
                                                 "SELECT @TZ=DATEPART(TZ, SYSDATETIMEOFFSET()); " & vbCrLf &
                                                 "SELECT DATEADD(HOUR, -1*@TZ/60, GETDATE()) AS Tanggal"
                                Hasil = com.ExecuteScalar()
                                With JSON
                                    .JSONMessage = "Data ditemukan"
                                    .JSONResult = True
                                    .JSONRows = 1
                                    .JSONValue = Hasil
                                End With
                            Catch ex As Exception
                                With JSON
                                    .JSONMessage = ex.Message
                                    .JSONResult = False
                                End With
                                InsertLog(StrKonSQL, New CtrlSoft.Dto.ViewModel.AppLog With {
                                          .AppName = My.Application.Info.AssemblyName,
                                          .Date = Now,
                                          .Event = "GetTimeServer",
                                          .User = "Unknow",
                                          .JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)},
                                      JSON)
                            End Try
                        End Using
                    End Using
                End Using
            End Using

            Return JSON
        End Function
        Public Shared Function GetListKategori(ByVal StrKonSQL As String) As JSONResult
            Dim JSON As New JSONResult
            Dim Hasil As New List(Of Core)
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using dt As New DataTable
                            Try
                                cn.Open()
                                com.Connection = cn
                                oDA.SelectCommand = com

                                com.CommandText = "SELECT * FROM MKategori(NOLOCK) WHERE IsActive=1 AND NoID NOT IN (SELECT IDParent FROM MKategori(NOLOCK))"
                                oDA.Fill(dt)

                                For Each iRow As DataRow In dt.Rows
                                    Hasil.Add(New Core With {.NoID = NullToLong(iRow.Item("NoID")),
                                                                       .Kode = NullToStr(iRow.Item("Kode")),
                                                                       .Nama = NullToStr(iRow.Item("Kode")) & "-" & NullToStr(iRow.Item("Nama"))})
                                Next
                                With JSON
                                    .JSONMessage = "Data ditemukan"
                                    .JSONResult = True
                                    .JSONRows = 1
                                    .JSONValue = Hasil
                                End With
                            Catch ex As Exception
                                With JSON
                                    .JSONMessage = ex.Message
                                    .JSONResult = False
                                End With
                                InsertLog(StrKonSQL, New CtrlSoft.Dto.ViewModel.AppLog With {
                                          .AppName = My.Application.Info.AssemblyName,
                                          .Date = Now,
                                          .Event = "GetListKategori",
                                          .User = "Unknow",
                                          .JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)},
                                      JSON)
                                Hasil.Clear()
                            End Try
                        End Using
                    End Using
                End Using
            End Using

            Return JSON
        End Function
        Public Shared Function GetListDepartemen(ByVal StrKonSQL As String) As JSONResult
            Dim JSON As New JSONResult
            Dim Hasil As New List(Of Core)
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using dt As New DataTable
                            Try
                                cn.Open()
                                com.Connection = cn
                                oDA.SelectCommand = com

                                com.CommandText = "SELECT * FROM MKategori(NOLOCK) WHERE IsActive=1 AND NoID IN (SELECT IDParent FROM MKategori(NOLOCK))"
                                oDA.Fill(dt)

                                For Each iRow As DataRow In dt.Rows
                                    Hasil.Add(New Core With {.NoID = NullToLong(iRow.Item("NoID")),
                                                                       .Kode = NullToStr(iRow.Item("Kode")),
                                                                       .Nama = NullToStr(iRow.Item("Kode")) & "-" & NullToStr(iRow.Item("Nama"))})
                                Next
                                With JSON
                                    .JSONMessage = "Data ditemukan"
                                    .JSONResult = True
                                    .JSONRows = 1
                                    .JSONValue = Hasil
                                End With
                            Catch ex As Exception
                                With JSON
                                    .JSONMessage = ex.Message
                                    .JSONResult = False
                                End With
                                InsertLog(StrKonSQL, New CtrlSoft.Dto.ViewModel.AppLog With {
                                          .AppName = My.Application.Info.AssemblyName,
                                          .Date = Now,
                                          .Event = "GetListDepartemen",
                                          .User = "Unknow",
                                          .JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)},
                                      JSON)
                                Hasil.Clear()
                            End Try
                        End Using
                    End Using
                End Using
            End Using

            Return JSON
        End Function
        Public Shared Function GetListSupplier(ByVal StrKonSQL As String) As JSONResult
            Dim JSON As New JSONResult
            Dim Hasil As New List(Of Core)
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using dt As New DataTable
                            Try
                                cn.Open()
                                com.Connection = cn
                                oDA.SelectCommand = com

                                com.CommandText = "SELECT NoID, Kode, Nama FROM MAlamat(NOLOCK) WHERE IsActive=1 AND IsSupplier=1"
                                oDA.Fill(dt)

                                For Each iRow As DataRow In dt.Rows
                                    Hasil.Add(New Core With {.NoID = NullToLong(iRow.Item("NoID")),
                                                                       .Kode = NullToStr(iRow.Item("Kode")),
                                                                       .Nama = NullToStr(iRow.Item("Kode")) & "-" & NullToStr(iRow.Item("Nama"))})
                                Next
                                With JSON
                                    .JSONMessage = "Data ditemukan"
                                    .JSONResult = True
                                    .JSONRows = 1
                                    .JSONValue = Hasil
                                End With
                            Catch ex As Exception
                                With JSON
                                    .JSONMessage = ex.Message
                                    .JSONResult = False
                                End With
                                InsertLog(StrKonSQL, New CtrlSoft.Dto.ViewModel.AppLog With {
                                          .AppName = My.Application.Info.AssemblyName,
                                          .Date = Now,
                                          .Event = "GetListSupplier",
                                          .User = "Unknow",
                                          .JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)},
                                      JSON)
                                Hasil.Clear()
                            End Try
                        End Using
                    End Using
                End Using
            End Using

            Return JSON
        End Function
        Public Shared Function GetListMerk(ByVal StrKonSQL As String) As JSONResult
            Dim JSON As New JSONResult
            Dim Hasil As New List(Of Core)
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using dt As New DataTable
                            Try
                                cn.Open()
                                com.Connection = cn
                                oDA.SelectCommand = com

                                com.CommandText = "SELECT NoID, Kode, Nama FROM MMerk(NOLOCK) WHERE IsActive=1"
                                oDA.Fill(dt)

                                For Each iRow As DataRow In dt.Rows
                                    Hasil.Add(New Core With {.NoID = NullToLong(iRow.Item("NoID")),
                                                                       .Kode = NullToStr(iRow.Item("Kode")),
                                                                       .Nama = NullToStr(iRow.Item("Kode")) & "-" & NullToStr(iRow.Item("Nama"))})
                                Next
                                With JSON
                                    .JSONMessage = "Data ditemukan"
                                    .JSONResult = True
                                    .JSONRows = 1
                                    .JSONValue = Hasil
                                End With
                            Catch ex As Exception
                                With JSON
                                    .JSONMessage = ex.Message
                                    .JSONResult = False
                                End With
                                InsertLog(StrKonSQL, New CtrlSoft.Dto.ViewModel.AppLog With {
                                          .AppName = My.Application.Info.AssemblyName,
                                          .Date = Now,
                                          .Event = "GetListMerk",
                                          .User = "Unknow",
                                          .JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)},
                                      JSON)
                                Hasil.Clear()
                            End Try
                        End Using
                    End Using
                End Using
            End Using

            Return JSON
        End Function
        Public Shared Function GetListGudang(ByVal StrKonSQL As String) As JSONResult
            Dim JSON As New JSONResult
            Dim Hasil As New List(Of Core)
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using dt As New DataTable
                            Try
                                cn.Open()
                                com.Connection = cn
                                oDA.SelectCommand = com

                                com.CommandText = "SELECT NoID, Kode, Nama FROM MGudang(NOLOCK) WHERE IsActive=1"
                                oDA.Fill(dt)

                                For Each iRow As DataRow In dt.Rows
                                    Hasil.Add(New Core With {.NoID = NullToLong(iRow.Item("NoID")),
                                                                       .Kode = NullToStr(iRow.Item("Kode")),
                                                                       .Nama = NullToStr(iRow.Item("Kode")) & "-" & NullToStr(iRow.Item("Nama"))})
                                Next
                                With JSON
                                    .JSONMessage = "Data ditemukan"
                                    .JSONResult = True
                                    .JSONRows = 1
                                    .JSONValue = Hasil
                                End With
                            Catch ex As Exception
                                With JSON
                                    .JSONMessage = ex.Message
                                    .JSONResult = False
                                End With
                                InsertLog(StrKonSQL, New CtrlSoft.Dto.ViewModel.AppLog With {
                                          .AppName = My.Application.Info.AssemblyName,
                                          .Date = Now,
                                          .Event = "GetListGudang",
                                          .User = "Unknow",
                                          .JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)},
                                      JSON)
                                Hasil.Clear()
                            End Try
                        End Using
                    End Using
                End Using
            End Using

            Return JSON
        End Function
        Public Shared Function GetBarangDetil(ByVal StrKonSQL As String, ByVal Barcode As String) As JSONResult
            Dim JSON As New JSONResult
            Dim Hasil As New List(Of Core)
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using dt As New DataTable
                            Try
                                cn.Open()
                                com.Connection = cn
                                oDA.SelectCommand = com

                                com.CommandText = "SELECT B.NoID, B.Barcode Kode, A.Nama AS Nama FROM MBarang(NOLOCK) A INNER JOIN MBarangD(NOLOCK) B ON A.NoID=B.IDBarang WHERE A.IsActive=1 AND B.IsActive=1" & vbCrLf &
                                                      " AND (B.Barcode=@Barcode OR A.Kode=@Barcode) ORDER BY B.Konversi, A.Kode, B.Barcode"
                                com.Parameters.Clear()
                                com.Parameters.Add(New SqlParameter("@Barcode", SqlDbType.VarChar)).Value = Barcode
                                oDA.Fill(dt)

                                For Each iRow As DataRow In dt.Rows
                                    Hasil.Add(New Core With {.NoID = NullToLong(iRow.Item("NoID")),
                                                                 .Kode = NullToStr(iRow.Item("Kode")),
                                                                 .Nama = NullToStr(iRow.Item("Nama")) & "-" & NullToStr(iRow.Item("Nama"))})
                                Next
                                With JSON
                                    .JSONMessage = "Data ditemukan"
                                    .JSONResult = True
                                    .JSONRows = 1
                                    .JSONValue = Hasil
                                End With
                            Catch ex As Exception
                                With JSON
                                    .JSONMessage = ex.Message
                                    .JSONResult = False
                                End With
                                InsertLog(StrKonSQL, New CtrlSoft.Dto.ViewModel.AppLog With {
                                          .AppName = My.Application.Info.AssemblyName,
                                          .Date = Now,
                                          .Event = "GetBarangDetil",
                                          .User = "Unknow",
                                          .JSON = Newtonsoft.Json.JsonConvert.SerializeObject(JSON)},
                                      JSON)
                            End Try
                        End Using
                    End Using
                End Using
            End Using

            Return JSON
        End Function
    End Class
End Namespace
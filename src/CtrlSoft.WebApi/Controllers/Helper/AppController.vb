Imports System.Net
Imports System.Web.Http
Imports CtrlSoft.Dto.ViewModel
Imports CtrlSoft.Dto.Model
Imports System.Security.Claims

Namespace Controllers.Helper
    <RoutePrefix("api/app")>
    Public Class AppController
        Inherits ApiController

        ' Get api/App
        <HttpGet, Route("getlogin")>
        Public Function GetLogin(ByVal UserID As String,
                                 ByVal Password As String) As JSONResult
            Dim identity = CType(User.Identity, ClaimsIdentity)
            If Not (identity Is Nothing OrElse IsDBNull(identity)) Then
                Return CtrlSoft.Repository.Repository.RepSQLServer.GetLogin(My.Settings.MyConn, UserID, Password)
            Else
                Return New JSONResult With {.JSONMessage = "IDentity is null!", .JSONRows = 0, .JSONResult = False, .JSONValue = Nothing}
            End If
        End Function
    End Class
End Namespace
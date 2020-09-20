Namespace ViewModel
    Public Class AppLog
        Public Property [NoID] As Long
        Public Property [AppName] As String
        Public Property [Date] As Date
        Public Property [Event] As String
        Public Property [User] As String
        Public Property [JSON] As String

        Public Sub New()
            Me.Date = Now
        End Sub
    End Class
End Namespace

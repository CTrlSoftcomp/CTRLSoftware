Namespace DBSettings
    Public Class MDBSetting
        Public Property Id As String
        Public Property Server As String
        Public Property Database As String
        Public Property UserID As String
        Public Property Password As String
        Public Property Timeout As Integer
        Public Property [Default] As Boolean
        Public Property [Group] As String
            Get
                Return IIf([Default], "Koneksi Aktif", "Koneksi Lainnya")
            End Get
            Set(value As String)

            End Set
        End Property
        Public Function KoneksiString() As String
            If (Server.Equals("(localdb)\MSSQLLocalDB")) Then
                Return "Server=" & Server & ";Initial Catalog=" & Database & ";Asynchronous Processing=True;Connect Timeout=" & Timeout & ";Application Name=" & Id.ToString() & ";"
            Else
                Return "Server=" & Server & ";Initial Catalog=" & Database & ";User Id=" & UserID & ";Password=" & Password & ";Asynchronous Processing=True;Connect Timeout=" & Timeout & ";Application Name=" & Id.ToString() & ";"
            End If
        End Function
    End Class
    Public Class DBName
        Public Property DBName As String
    End Class
End Namespace
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
            Return "Server=" & Server & ";Initial Catalog=" & Database & ";User Id=" & UserID & ";Password=" & Password & ";Asynchronous Processing=True;Connect Timeout=" & TimeOut & ";Application Name=" & Id.ToString() & ";"
        End Function
    End Class
    Public Class DBName
        Public Property DBName As String
    End Class
End Namespace
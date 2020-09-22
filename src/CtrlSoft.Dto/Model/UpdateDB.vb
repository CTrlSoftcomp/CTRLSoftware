Namespace Model
    Public Class UpdateDB
        Private _DBVersion As String
        Private _TglUpdate As Date
        Private _SQL As String
        Private _ListSQL As List(Of String)

        Public Property DBVersion() As String
            Get
                Return _DBVersion
            End Get
            Set(ByVal value As String)
                _DBVersion = value
            End Set
        End Property

        Public Property TglUpdate() As Date
            Get
                Return _TglUpdate
            End Get
            Set(ByVal value As Date)
                _TglUpdate = value
            End Set
        End Property

        Public Property SQL() As String
            Get
                Return _SQL
            End Get
            Set(ByVal value As String)
                _SQL = value
            End Set
        End Property

        Public Property ListSQL As List(Of String)
            Get
                If SQL <> "" Then
                    _ListSQL.Add(SQL)
                End If
                Return _ListSQL
            End Get
            Set(value As List(Of String))
                _ListSQL = value
            End Set
        End Property

        Public Sub New()
            Me.TglUpdate = Now()
            Me.SQL = ""
            Me.DBVersion = ""
            Me.ListSQL = New List(Of String)
        End Sub
    End Class
End Namespace
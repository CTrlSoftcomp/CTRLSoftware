Namespace Cache
    Public Interface ICache
        Function Cache(Type As String, Id As String) As MCache
        Function Save(Type As String, Id As String) As MCache
        Function Delete(Type As String, Id As String) As MCache
        Function List(Type As String, Filter As String) As List(Of MCache)
    End Interface
End Namespace

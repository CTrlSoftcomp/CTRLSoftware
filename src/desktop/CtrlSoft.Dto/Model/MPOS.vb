Namespace Model
    Public Class MPOS
        Public Property NoID As Long
        Public Property Kode As String
        Public Property Nama As String
        Public Property JenisPenjualan As MJenisPenjualan
        Public Property IP As String
    End Class

    Public Class MJenisPenjualan
        Public Property NoID As Long
        Public Property Kode As String
        Public Property Nama As String
    End Class
End Namespace

Namespace Model
    Public Class Barang
        Private IDBarangD_ As Long
        Private IDBarang_ As Long
        Private Kode_ As String
        Private Nama_ As String

        Public Property IDBarang() As Long
            Get
                Return IDBarang_
            End Get
            Set(ByVal value As Long)
                IDBarang_ = value
            End Set
        End Property

        Public Property IDBarangD() As Long
            Get
                Return IDBarangD_
            End Get
            Set(ByVal value As Long)
                IDBarangD_ = value
            End Set
        End Property

        Public Property Kode() As String
            Get
                Return Kode_
            End Get
            Set(ByVal value As String)
                Kode_ = value
            End Set
        End Property

        Public Property Nama() As String
            Get
                Return Nama_
            End Get
            Set(ByVal value As String)
                Nama_ = value
            End Set
        End Property
    End Class

    Public Class PurchaseOrder
        Private NoUrut_ As Integer
        Private IDHeader_ As Long
        Private Barang_ As Model.Barang

    End Class
End Namespace

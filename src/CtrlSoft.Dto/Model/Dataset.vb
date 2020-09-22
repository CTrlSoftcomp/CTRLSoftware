Namespace Model
    Public Class DatasetLookUp
        Public Property SQLLookUpBarcode() As String
            Get
                Return "SELECT MBarangD.NoID, MBarangD.Barcode, MBarang.Kode, MBarang.Nama + ISNULL(' (' + MSatuan.Kode + ')', '') AS Nama, MBarangD.Konversi AS Isi" & vbCrLf & _
                       "FROM MBarang" & vbCrLf & _
                       "INNER JOIN MBarangD ON MBarangD.IDBarang=MBarang.NoID" & vbCrLf & _
                       "LEFT JOIN MSatuan ON MBarangD.IDSatuan=MSatuan.NoID" & vbCrLf & _
                       "WHERE MBarangD.IsActive=1 AND MBarang.IsActive=1"
            End Get
            Set(ByVal value As String)

            End Set
        End Property

        Public Property SQLLookUpBarang() As String
            Get
                Return "SELECT MBarang.NoID, MBarang.Kode, MBarang.Nama Nama" & vbCrLf &
                       "FROM MBarang" & vbCrLf &
                       "WHERE MBarang.IsActive=1"
            End Get
            Set(ByVal value As String)

            End Set
        End Property

        Public Property SQLLookUpAlamat() As String
            Get
                Return "SELECT NoID, Kode, Nama, Alamat FROM MAlamat(NOLOCK) WHERE IsActive=1"
            End Get
            Set(value As String)

            End Set
        End Property
    End Class
End Namespace

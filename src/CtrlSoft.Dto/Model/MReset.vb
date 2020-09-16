Namespace Model
    Public Class MReset
        Public Property NoID As Long
        Public Property Kode As String
        Public Property Tanggal As Date
        Public Property Shift As Integer
        Public Property Kasir As MUser
        Public Property POS As MPOS
        Public Property JumlahNota As Integer
        Public Property Subtotal As Double
        Public Property Tunai As Double
        Public Property SetorTunai As Double
        Public Property IsPosted As Boolean
        Public Property TglPosted As Date
        Public Property UserPosted As MUser
        Public Property UserEntry As MUser
        Public Property UserEdit As MUser
        Public Property Detil As New List(Of MResetD)

        Public Sub New()
            Me.Kasir = New MUser
            Me.POS = New MPOS
            Me.UserEntry = New MUser
            Me.UserPosted = New MUser
            Me.UserEdit = New MUser
            Me.Detil = New List(Of MResetD)
        End Sub
    End Class

    Public Class MResetD
        Public Property IDReset As Long
        Public Property JenisPembayaran As MJenisPembayaran
        Public Property JumlahNota As Integer
        Public Property Subtotal As Double

        Public Sub New()
            Me.JenisPembayaran = New MJenisPembayaran
        End Sub
    End Class

    Public Class MJenisPembayaran
        Public Property NoID As Long
        Public Property Kode As String
        Public Property Nama As String
        Public Property IsBank As Boolean
        Public Property ChargeProsen As Double
    End Class
End Namespace

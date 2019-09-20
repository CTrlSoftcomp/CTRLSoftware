Namespace Model
    Public Class Pembayaran
        Private _NoID As Long
        Private _IDJenisPembayaran As Integer
        Private _IsBank As Boolean
        Private _AtasNama As String
        Private _NoRekening As String
        Private _Nominal As Double
        Private _ChargeProsen As Double
        Private _ChargeRp As Double
        Private _Total As Double

        Public Property NoID() As Long
            Get
                Return _NoID
            End Get
            Set(ByVal value As Long)
                _NoID = value
            End Set
        End Property

        Public Property IDJenisPembayaran() As Integer
            Get
                Return _IDJenisPembayaran
            End Get
            Set(ByVal value As Integer)
                _IDJenisPembayaran = value
            End Set
        End Property

        Public Property AtasNama() As String
            Get
                Return _AtasNama
            End Get
            Set(ByVal value As String)
                _AtasNama = value
            End Set
        End Property

        Public Property NoRekening() As String
            Get
                Return _NoRekening
            End Get
            Set(ByVal value As String)
                _NoRekening = value
            End Set
        End Property

        Public Property Nominal() As Double
            Get
                Return _Nominal
            End Get
            Set(ByVal value As Double)
                _Nominal = value
            End Set
        End Property

        Public Property ChargeProsen() As Double
            Get
                Return _ChargeProsen
            End Get
            Set(ByVal value As Double)
                _ChargeProsen = value
            End Set
        End Property

        Public Property ChargeRp() As Double
            Get
                Return _ChargeRp
            End Get
            Set(ByVal value As Double)
                _ChargeRp = value
            End Set
        End Property

        Public Property Total() As Double
            Get
                Return _Total
            End Get
            Set(ByVal value As Double)
                _Total = value
            End Set
        End Property

        Public Property IsBank() As Boolean
            Get
                Return _IsBank
            End Get
            Set(ByVal value As Boolean)
                _IsBank = value
            End Set
        End Property

        Public Sub New()
            Total = 0.0
            ChargeRp = 0.0
            ChargeProsen = 0.0
            IDJenisPembayaran = 1
            Nominal = 0.0
            IsBank = False
        End Sub
    End Class
End Namespace

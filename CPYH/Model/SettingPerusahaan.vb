Namespace Model
    Public Class SettingPerusahaan
        Private NamaPerusahaan_ As String
        Private AlamatPerusahaan_ As String
        Private KotaPerusahaan_ As String
        Private PathLayouts_ As String

        Public Property NamaPerusahaan() As String
            Get
                Return NamaPerusahaan_
            End Get
            Set(ByVal value As String)
                NamaPerusahaan_ = value
            End Set
        End Property

        Public Property AlamatPerusahaan() As String
            Get
                Return AlamatPerusahaan_
            End Get
            Set(ByVal value As String)
                AlamatPerusahaan_ = value
            End Set
        End Property

        Public Property KotaPerusahaan() As String
            Get
                Return KotaPerusahaan_
            End Get
            Set(ByVal value As String)
                KotaPerusahaan_ = value
            End Set
        End Property

        Public Property PathLayouts() As String
            Get
                If System.IO.Directory.Exists(PathLayouts_) Then
                    Return PathLayouts_
                Else
                    Return Application.StartupPath & "\System\Layouts\"
                End If
            End Get
            Set(ByVal value As String)
                PathLayouts_ = value
            End Set
        End Property

        Public Sub New()
            NamaPerusahaan = "Nama Perusahaan anda"
            AlamatPerusahaan = "Jl. ABCDEFGHIJ No 1"
            KotaPerusahaan = "Surabaya"
            PathLayouts = Application.StartupPath & "\System\Layouts\"
        End Sub
    End Class
End Namespace
Namespace Model
    Public Class MUser
        Private NoID_ As Long = -1
        Private Kode_ As String = ""
        Private Nama_ As String = ""
        Private Role_ As String = ""
        Private TanggalSystem_ As Date = Now
        Private Menu_ As List(Of Menu) = New List(Of Menu)
        Private Supervisor_ As Boolean = False

        Public Property NoID() As Long
            Get
                Return NoID_
            End Get
            Set(ByVal value As Long)
                NoID_ = value
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

        Public Property Role() As String
            Get
                Return Role_
            End Get
            Set(ByVal value As String)
                Role_ = value
            End Set
        End Property

        Public Property HasLogin() As Boolean
            Get
                If NoID >= 1 Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)

            End Set
        End Property

        Public Property TanggalSystem() As Date
            Get
                Return Now()
            End Get
            Set(ByVal value As Date)
                TanggalSystem_ = value
            End Set
        End Property

        Public Property Menu() As List(Of Menu)
            Get
                Return Menu_
            End Get
            Set(ByVal value As List(Of Menu))
                Menu_ = value
            End Set
        End Property

        Public Property Supervisor() As Boolean
            Get
                Return Supervisor_
            End Get
            Set(ByVal value As Boolean)
                Supervisor_ = value
            End Set
        End Property
    End Class

    Public Class SubMenu
        Private NoID_ As Long
        Private Name_ As String
        Private Caption_ As String
        Private Visible_ As Boolean
        Private BeginGroup_ As Boolean
        Private BigMenu_ As Boolean

        Public Property NoID() As Long
            Get
                Return NoID_
            End Get
            Set(ByVal value As Long)
                NoID_ = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return Name_
            End Get
            Set(ByVal value As String)
                Name_ = value
            End Set
        End Property

        Public Property Caption() As String
            Get
                Return Caption_
            End Get
            Set(ByVal value As String)
                Caption_ = value
            End Set
        End Property

        Public Property Visible() As Boolean
            Get
                Return Visible_
            End Get
            Set(ByVal value As Boolean)
                Visible_ = value
            End Set
        End Property

        Public Property BeginGroup() As Boolean
            Get
                Return BeginGroup_
            End Get
            Set(ByVal value As Boolean)
                BeginGroup_ = value
            End Set
        End Property

        Public Property BigMenu() As Boolean
            Get
                Return BigMenu_
            End Get
            Set(ByVal value As Boolean)
                BigMenu_ = value
            End Set
        End Property
    End Class

    Public Class Menu
        Private NoID_ As Long
        Private Name_ As String
        Private Caption_ As String
        Private Visible_ As Boolean
        Private SubMenu_ As List(Of SubMenu) = New List(Of SubMenu)

        Public Property NoID() As Long
            Get
                Return NoID_
            End Get
            Set(ByVal value As Long)
                NoID_ = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return Name_
            End Get
            Set(ByVal value As String)
                Name_ = value
            End Set
        End Property

        Public Property Caption() As String
            Get
                Return Caption_
            End Get
            Set(ByVal value As String)
                Caption_ = value
            End Set
        End Property

        Public Property Visible() As Boolean
            Get
                Return Visible_
            End Get
            Set(ByVal value As Boolean)
                Visible_ = value
            End Set
        End Property

        Public Property SubMenu() As List(Of SubMenu)
            Get
                Return SubMenu_
            End Get
            Set(ByVal value As List(Of SubMenu))
                SubMenu_ = value
            End Set
        End Property
    End Class
End Namespace

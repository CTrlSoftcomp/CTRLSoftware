'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated from a template.
'
'     Manual changes to this file may cause unexpected behavior in your application.
'     Manual changes to this file will be overwritten if the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Collections.Generic

Namespace Model
    Public Class MMaterial
        Public Sub New()
            Me.NoID = System.Guid.NewGuid
            Me.MMaterialDs = New HashSet(Of MMaterialD)
            Me.MMaterialDBiayas = New HashSet(Of MMaterialDBiaya)
            Me.MMaterialDSisas = New HashSet(Of MMaterialDSisa)
        End Sub

        Public Property NoID As System.Guid
        Public Property Kode As String
        Public Property Nama As String
        Public Property Keterangan As String
        Public Property IsActive As Boolean
        Public Property IDBarangD As Long
        Public Property IDBarang As Long
        Public Property IDSatuan As Integer
        Public Property Konversi As Decimal
        Public Property Qty As Decimal
        Public Property HargaPokok As Decimal
        Public Property Jumlah As Decimal
            Get
                Return System.Math.Round(Qty * HargaPokok, 2)
            End Get
            Set(value As Decimal)

            End Set
        End Property
        Public Property IDUser As Integer
        Public Property TanggalUpdate As Date

        Public Overridable Property MMaterialDs As ICollection(Of MMaterialD) = New HashSet(Of MMaterialD)
        Public Overridable Property MMaterialDBiayas As ICollection(Of MMaterialDBiaya) = New HashSet(Of MMaterialDBiaya)
        Public Overridable Property MMaterialDSisas As ICollection(Of MMaterialDSisa) = New HashSet(Of MMaterialDSisa)
    End Class
End Namespace
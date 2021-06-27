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
    Public Class MAssembly
        Public Sub New()
            Me.IDWOAssembly = System.Guid.NewGuid
            Me.IDMaterial = System.Guid.NewGuid
            Me.MAssemblyDs = New HashSet(Of MAssemblyD)
            Me.MAssemblyDBiayas = New HashSet(Of MAssemblyDBiaya)
            Me.MAssemblyDSisas = New HashSet(Of MAssemblyDSisa)
        End Sub

        Public Property NoID As Long
        Public Property Kode As String
        Public Property Tanggal As Date
        Public Property IDWOAssembly As Nullable(Of System.Guid)
        Public Property IDMaterial As System.Guid
        Public Property IDPegawai As Long
        Public Property IDGudang As Integer
        Public Property Catatan As String
        Public Property IDBarangD As Long
        Public Property IDBarang As Long
        Public Property IDSatuan As Integer
        Public Property Konversi As Decimal
        Public Property Qty As Decimal
        Public Property HargaPokok As Decimal
        Public Property Jumlah As Decimal
        Public Property IsPosted As Nullable(Of Boolean)
        Public Property TglPosted As Nullable(Of Date)
        Public Property IDUserPosted As Nullable(Of Integer)
        Public Property IDUserEntry As Nullable(Of Integer)
        Public Property IDUserEdit As Nullable(Of Integer)

        Public Overridable Property MAssemblyDs As ICollection(Of MAssemblyD) = New HashSet(Of MAssemblyD)
        Public Overridable Property MAssemblyDBiayas As ICollection(Of MAssemblyDBiaya) = New HashSet(Of MAssemblyDBiaya)
        Public Overridable Property MAssemblyDSisas As ICollection(Of MAssemblyDSisa) = New HashSet(Of MAssemblyDSisa)
    End Class
End Namespace
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

Partial Public Class MMaterialD
    Public Property NoID As System.Guid
    Public Property IDMaterial As System.Guid
    Public Property IDBarangD As Long
    Public Property IDBarang As Long
    Public Property IDSatuan As Integer
    Public Property Konversi As Decimal
    Public Property Qty As Decimal
    Public Property HargaPokok As Decimal
    Public Property Jumlah As Decimal

    Public Overridable Property MAssemblyDs As ICollection(Of MAssemblyD) = New HashSet(Of MAssemblyD)
    Public Overridable Property MMaterial As MMaterial

End Class

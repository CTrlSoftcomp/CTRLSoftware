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

Partial Public Class MAssemblyDSisa
    Public Property NoID As Long
    Public Property IDHeader As Long
    Public Property IDMaterialDSisa As System.Guid
    Public Property Tanggal As Date
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

    Public Overridable Property MAssembly As MAssembly
    Public Overridable Property MMaterialDSisa As MMaterialDSisa

End Class

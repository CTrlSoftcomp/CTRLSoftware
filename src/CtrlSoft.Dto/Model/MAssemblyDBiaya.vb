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
    Public Class MAssemblyDBiaya
        Public Property NoID As Long
        Public Property IDHeader As Long
        Public Property IDMaterialDBiaya As System.Guid
        Public Property Tanggal As Date
        Public Property IDAkun As System.Guid
        Public Property Jumlah As Decimal
        Public Property Keterangan As String
        Public Property IsPosted As Nullable(Of Boolean)
        Public Property TglPosted As Nullable(Of Date)
        Public Property IDUserPosted As Nullable(Of Integer)
        Public Property IDUserEntry As Nullable(Of Integer)
        Public Property IDUserEdit As Nullable(Of Integer)
    End Class
End Namespace
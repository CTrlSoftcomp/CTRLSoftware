Imports CtrlSoft.Dto.Model

Public Class [Public]
    Public Shared NamaAplikasi As String = Application.ProductName.ToString
    Public Shared StrKonSQL As String = ""
    Public Shared UserLogin As New MUser
    Public Shared SettingPerusahaan As New SettingPerusahaan
    Public Shared UserOtorisasi As New MUser
    Public Shared IsEditReport As Boolean = False
    Public Shared Dataset As New DatasetLookUp

    Public Enum pStatusForm
        Baru = 0
        Edit = 1
        TempInsert = 2
        Posted = 3
    End Enum

    Public Enum TypePajak
        NonPajak = 0
        Include = 1
        Exclude = 2
    End Enum
End Class

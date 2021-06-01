Namespace DBSettings
    Public Interface IDBSetting
        Function [Get](Id As String) As MDBSetting
        Function Save(Model As MDBSetting) As MDBSetting
        Function Delete(Id As String) As MDBSetting
        Function List() As List(Of MDBSetting)
    End Interface
End Namespace

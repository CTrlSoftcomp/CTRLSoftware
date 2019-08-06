Imports CtrlSoft.Utils

Class ODBC
    Private Declare Function SQLConfigDataSource Lib "ODBCCP32.DLL" (ByVal hwndParent As Integer, ByVal ByValfRequest As Integer, ByVal lpszDriver As String, ByVal lpszAttributes As String) As Integer
    Private Declare Function SQLInstallerError Lib "ODBCCP32.DLL" (ByVal iError As Integer, ByRef pfErrorCode As Integer, ByVal lpszErrorMsg As System.Text.StringBuilder, ByVal cbErrorMsgMax As Integer, ByRef pcbErrorMsg As Integer) As Integer

    Private Const ODBC_ADD_DSN As Short = 1 ' Add data source
    Private Const ODBC_ADD_SYS_DSN As Short = 4
    Private Const vbAPINull As Integer = 0 ' NULL Pointer

    Public Shared Sub CreateUserDSN()

        Dim intRet As Integer
        Dim Driver As String
        Dim Attributes As String

        'Set the driver to SQL Server because it is most common.
        Driver = "SQL Server"
        'Set the attributes delimited by null.
        'See driver documentation for a complete
        'list of supported attributes.
        Attributes = "SERVER=(local)" & Chr(0)
        Attributes = Attributes & "DESCRIPTION=Local DSN" & Chr(0)
        Attributes = Attributes & "DSN=Local DSN" & Chr(0)
        Attributes = Attributes & "DATABASE=Northwind" & Chr(0)
        'Unsupported by SQL Server
        'Attributes = Attributes & "Uid=" & Chr(0) & "pwd=" & Chr(0)
        'To show dialog, use Form1.Hwnd instead of vbAPINull.
        intRet = SQLConfigDataSource(vbAPINull, ODBC_ADD_DSN, Driver, Attributes)
        If intRet <> 0 Then
            MsgBox("DSN Created")
        Else
            Dim nErrorCode As Integer
            Dim strError As New System.Text.StringBuilder(255)
            Dim nErrorLen As Integer
            intRet = SQLInstallerError(1, nErrorCode, strError, 255, nErrorLen)
            MsgBox("Create Failed - " & Left$(strError.ToString, nErrorLen))
        End If

    End Sub

    Public Shared Sub CreateSystemDSN(ByVal DSNName As String, ByVal ServerName As String, ByVal UserID As String, ByVal Password As String, ByVal DatabaseName As String)

        Dim ReturnValue As Integer
        Dim Driver As String
        Dim Attributes As String

        'Dim UserID As String = BacaIni("odbcconfig", "Username", "sa")
        'Dim Password As String = BacaIni("odbcconfig", "Password", "sahaysstem")
        'Dim ServerName As String = BacaIni("DBConfig", "Server", "CityToys")
        'Dim DatabaseName As String = BacaIni("odbcconfig", "Database", "DBCityToys")
        'Dim DSNName As String = BacaIni("odbcconfig", "Server", "CityToys")
        'Set the driver to SQL Server because it is most common.
        Driver = "SQL Server"
        'Set the attributes delimited by null.
        'See driver documentation for a complete
        'list of supported attributes.
        Attributes = "SERVER=" & ServerName & Chr(0)
        Attributes = Attributes & "DESCRIPTION=ODBC " & NamaAplikasi & Chr(0)
        Attributes = Attributes & "DSN=" & DSNName & Chr(0)
        Attributes = Attributes & "DATABASE=" & DatabaseName & Chr(0)
        'To show dialog, use Form1.Hwnd instead of vbAPINull.
        ReturnValue = SQLConfigDataSource(vbAPINull, ODBC_ADD_SYS_DSN, Driver, Attributes)
        If ReturnValue <> 0 Then
            'MsgBox("DSN Created")
        Else
            MsgBox("Create Failed")
        End If
    End Sub
End Class

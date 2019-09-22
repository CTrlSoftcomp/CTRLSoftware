Imports DevExpress.XtraEditors
Imports System.Net
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid

Public Class Utils
    Public Shared Parser As New modParser
    Public Shared NamaAplikasi As String = Application.ProductName.ToString
    Public Shared StrKonSQL As String = ""
    Public Shared UserLogin As New Model.User
    Public Shared SettingPerusahaan As New Model.SettingPerusahaan
    Public Shared UserOtorisasi As New Model.User
    Public Shared IsEditReport As Boolean = False

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

    Public Shared Function BuangSpasi(ByVal x) As String
        Dim i As Integer, Str As String = ""
        If IsDBNull(x) Then
            Return ""
        Else
            Str = ""
            For i = 1 To Len(x)
                If Mid(x, i, 1) <> " " Then
                    Str = Str & Mid(x, i, 1)
                End If
            Next
            Return Str
        End If
    End Function
    Public Shared Sub hextoarray(ByVal inphex As String, ByRef outarray() As Byte)
        ReDim outarray(0 To Len(inphex) / 2)
        Dim i As Integer
        For i = 1 To Len(inphex) Step 2
            outarray(((i + 1) / 2) - 1) = Val("&H" + Mid$(inphex, i, 2))
            'Debug.Print Val("&H" + Mid$(inphex, i, 2))
        Next i
    End Sub
    Public Shared Function EAN8_Checksum(ByVal EAN8_Barcode As String) As String
        'http://www.barcodeisland.com/ean8.phtml

        Dim ChecksumCalculation As Integer
        ChecksumCalculation = 0
        Dim Position As Integer
        Position = 1
        Dim i As Integer
        For i = Len(EAN8_Barcode) - 1 To 0 Step -1
            If Position Mod 2 = 1 Then
                'odd position
                ChecksumCalculation = ChecksumCalculation + CLng(Mid(EAN8_Barcode, i + 1, 1)) * 3
            Else
                'even position
                ChecksumCalculation = ChecksumCalculation + CLng(Mid(EAN8_Barcode, i + 1, 1)) * 1
            End If
            Position = Position + 1
        Next

        Dim Checksum As Integer
        Checksum = (10 - (ChecksumCalculation Mod 10)) Mod 10
        If Checksum = 10 Then
            Checksum = 0
        End If
        If EAN8_Barcode.Length >= 7 Then
            Return EAN8_Barcode.Substring(0, 7) & Format$(Checksum, "0")
        Else
            Return EAN8_Barcode.ToString() & Format$(Checksum, "0")
        End If
    End Function
    Public Shared Function Append_EAN13_Checksum(ByVal RawString As String) As String
        Dim Position As Integer
        Dim Checksum As Integer

        Checksum = 0
        For Position = 2 To 12 Step 2
            Checksum = Checksum + Val(Mid$(RawString, Position, 1))
        Next Position
        Checksum = Checksum * 3
        For Position = 1 To 11 Step 2
            Checksum = Checksum + Val(Mid$(RawString, Position, 1))
        Next Position
        Checksum = Checksum Mod 10
        Checksum = 10 - Checksum
        If Checksum = 10 Then
            Checksum = 0
        End If
        If RawString.Length >= 12 Then
            Return RawString.Substring(0, 12) & Format$(Checksum, "0")
        Else
            Return RawString.ToString() & Format$(Checksum, "0")
        End If
    End Function
    Public Shared Function GetIPAddress() As String
        Dim IP1 As String = ""
        'Dim localEntry As Net.IPHostEntry = Dns.GetHostEntry(System.Net.Dns.GetHostName)
        'For Each address As Net.IPAddress In localEntry.AddressList
        '    If IP1 = "" Then
        '        IP1 = IP1 & address.ToString
        '    Else
        '        IP1 = address.IsIPv6SiteLocal.ToString & ":" & IP1
        '    End If
        'Next

        Dim IP As System.Net.IPAddress
        With Dns.GetHostByName(Dns.GetHostName()) 'GetHostByName
            IP = New System.Net.IPAddress(.AddressList(0).Address) 'System.Net.IPAddress(.AddressList(0).Address)

            IP1 = IP.ToString
        End With
        Return IP1
    End Function
    'And to implement It you would need to call it like this,you can do whatever you want
    'with it as its very flexible
    Public Shared Sub MYIPandHOST()
        Dim host As String = System.Net.Dns.GetHostName
        XtraMessageBox.Show("Name of the System is: " & host)
        XtraMessageBox.Show("Your IP address is: " & GetIPAddress())
    End Sub
    Public Shared Sub BukaFile(ByVal nmfile As String)
        Try
            Dim p As New System.Diagnostics.ProcessStartInfo
            p.Verb = "Open"
            p.WindowStyle = ProcessWindowStyle.Normal
            p.FileName = nmfile
            p.UseShellExecute = True
            System.Diagnostics.Process.Start(p)
        Catch ex As Exception
            XtraMessageBox.Show("Ada Kesalahan :" & vbCrLf & ex.Message & vbCrLf & "File : " & nmfile, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
    Public Shared Sub PrintFile(ByVal nmfile As String)
        Try
            Dim p As New System.Diagnostics.ProcessStartInfo
            p.Verb = "Print"
            p.WindowStyle = ProcessWindowStyle.Normal
            p.FileName = nmfile
            p.UseShellExecute = True
            System.Diagnostics.Process.Start(p)
        Catch ex As Exception
            XtraMessageBox.Show("Ada Kesalahan :" & vbCrLf & ex.Message & vbCrLf & "File : " & nmfile, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
    Public Shared Function ComTerbilang(ByVal Angka As Double) As String
        Dim x As String = ""
        Dim Bilang As New Terbilang
        Try
            x = NullToStr(Bilang.UFLTerbilang(Angka))
        Catch ex As Exception
            XtraMessageBox.Show("Ada Kesalahan :" & vbCrLf & ex.Message & vbCrLf & Angka, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
        Return x
    End Function
    Public Shared Function NullToDbl(ByVal Value As Object) As Double
        If IsDBNull(Value) Then
            Return 0.0
        Else
            If IsNumeric(Value) Then
                Return Value
            Else
                Return 0.0
            End If

        End If
    End Function
    Public Shared Function NullToBool(ByVal Value As Object) As Boolean
        If IsDBNull(Value) Then
            Return False
        ElseIf Value Is Nothing Then
            Return False
        ElseIf Value.ToString = "" Then
            Return False
        Else
            Return CBool(Value)
        End If
    End Function
    Public Shared Function NullToStr(ByVal Value As Object) As String
        If IsDBNull(Value) Then
            Return ""
        ElseIf Value Is Nothing Then
            Return ""
        Else
            Return Value
        End If
    End Function
    Public Shared Function AppendBackSlash(ByVal str As String) As String
        If Right(str, 1) = "\" Then
            Return str
        Else
            Return str & "\"
        End If
    End Function
    Public Shared Function NullToLong(ByVal Value As Object) As Long
        If IsDBNull(Value) Then
            Return 0
        Else
            If IsNumeric(Value) Then
                Return Value
            Else
                Return 0
            End If
        End If
    End Function
    Public Shared Function NullTolInt(ByVal Value As Object) As Integer
        If IsDBNull(Value) Then
            Return 0
        Else
            If IsNumeric(Value) Then
                Return Value
            Else
                Return 0
            End If
        End If
    End Function
    Public Shared Function NullToDate(ByVal X As Object) As Date
        If TypeOf X Is Date OrElse IsDate(X) Then
            Return CDate(X)
        Else
            Return CDate("1/1/1900")
        End If
    End Function
    Public Shared Function NullToDateMDB(ByVal X As Object) As String
        If TypeOf X Is Date Then
            Return "#" & Format(CDate(X), "MM/dd/yyyy") & "#"
        Else
            Return "NULL"
        End If
    End Function
    Public Shared Sub SetGridView(ByRef GridControl1 As DevExpress.XtraGrid.GridControl)
        'Set Format Gridview Here
        Dim repChekEdit As New RepositoryItemCheckEdit
        Try
            For i As Integer = 0 To GridControl1.ViewCollection.Count - 1
                Dim view As DevExpress.XtraGrid.Views.Base.ColumnView
                If i = 0 Then
                    view = CType(GridControl1.DefaultView, Views.Base.ColumnView)
                Else
                    view = CType(GridControl1.ViewCollection(i), Views.Base.ColumnView)
                End If
                For x As Integer = 0 To view.Columns.Count - 1
                    Select Case view.Columns(i).ColumnType.Name.ToLower
                        Case "int32", "int64", "int"
                            view.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            view.Columns(i).DisplayFormat.FormatString = "n2"
                        Case "decimal", "single", "money", "double"
                            view.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            view.Columns(i).DisplayFormat.FormatString = "n2"
                        Case "string"
                            view.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                            view.Columns(i).DisplayFormat.FormatString = ""
                        Case "date"
                            view.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            view.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                        Case "datetime"
                            view.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            view.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
                        Case "boolean"
                            view.Columns(i).ColumnEdit = repChekEdit
                    End Select
                Next
            Next
        Catch ex As Exception

        End Try
    End Sub
    Public Shared Function Bulatkan(ByVal x As Double, ByVal Koma As Integer) As Double
        If Koma >= 0 Then
            Bulatkan = System.Math.Round(x, CInt(Koma))
            If System.Math.Round(x - Bulatkan, CInt(Koma + 5)) >= 0.5 / (10 ^ Koma) Then Bulatkan = Bulatkan + 1 / (10 ^ Koma)
        Else
            Bulatkan = x
        End If
    End Function
    Public Shared Function Evaluate(ByVal kalimat As String) As Double
        Dim DecSep As String
        Dim Nfi As System.Globalization.NumberFormatInfo = System.Globalization.CultureInfo.InstalledUICulture.NumberFormat
        DecSep = Nfi.NumberDecimalSeparator
        kalimat = kalimat.Replace(".", DecSep).Replace(",", DecSep)
        Parser.Function = kalimat
        Parser.BuildFunctionTree()
        Return Parser.Result
    End Function
    Public Shared Function FixApostropi(ByVal obj As Object) As String
        Dim x As String = ""
        Try
            x = obj.ToString.Replace("'", "''")
        Catch ex As Exception
            x = ""
        End Try
        Return x
    End Function
    Public Shared Function FixKoma(ByVal obj As Object) As String
        Dim x As String = ""
        Try
            x = obj.ToString.Replace(",", ".")
        Catch ex As Exception
            x = ""
        End Try
        Return x
    End Function
    Public Shared Function EncryptText(ByVal strText As String, ByVal strPwd As String) As String
        Dim i As Integer, c As Integer
        Dim strBuff As String = Nothing

#If Not CASE_SENSITIVE_PASSWORD Then

        'Convert password to upper case
        'if not case-sensitive
        strPwd = UCase$(strPwd)

#End If

        'Encrypt string
        If CBool(Len(strPwd)) Then
            For i = 1 To Len(strText)
                c = Asc(Mid$(strText, i, 1))
                c = c + Asc(Mid$(strPwd, (i Mod Len(strPwd)) + 1, 1))
                strBuff = strBuff & Chr(c And &HFF)
            Next i
        Else
            strBuff = strText
        End If
        Return strBuff
    End Function
    Public Shared Function DecryptText(ByVal strText As String, ByVal strPwd As String) As String
        Dim i As Integer, c As Integer
        Dim strBuff As String = Nothing

#If Not CASE_SENSITIVE_PASSWORD Then

        'Convert password to upper case
        'if not case-sensitive
        strPwd = UCase$(strPwd)

#End If

        'Decrypt string
        If CBool(Len(strPwd)) Then
            For i = 1 To Len(strText)
                c = Asc(Mid$(strText, i, 1))
                c = c - Asc(Mid$(strPwd, (i Mod Len(strPwd)) + 1, 1))
                strBuff = strBuff & Chr(c And &HFF)
            Next i
        Else
            strBuff = strText
        End If
        Return strBuff
    End Function
    Public Shared Function GetAppPath() As String

        'Dim asm As [Assembly] = [Assembly].GetExecutingAssembly()

        'Return System.IO.Path.GetDirectoryName(asm.GetName().CodeBase)
        Return Application.StartupPath
    End Function
End Class

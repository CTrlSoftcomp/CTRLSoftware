Public Class Terbilang

    Public UFPrefixFunctions As Boolean

    'Persist the "words" across all calls to this class
    Private m_19AndUnder(19) As String
    'UPGRADE_WARNING: Lower bound of array m_Tens was changed from 2 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
    Private m_Tens(9) As String
    Private m_Hundred As String
    'UPGRADE_WARNING: Lower bound of array m_Groups was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
    Private m_Groups(10) As String
    Private m_Dollar As String
    Private m_Dollars As String
    Private m_NoCents As String
    Private m_Cent As String
    Private m_Cents As String
    Private m_Hyphen As String
    Private m_And As String
    Private m_InvalidAmount As String

    'UPGRADE_NOTE: Class_Initialize was upgraded to Class_Initialize_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Private Sub Class_Initialize_Renamed()
        UFPrefixFunctions = False
        'Initialize all the "words"

        m_19AndUnder(0) = "Zero"
        m_19AndUnder(1) = "One"
        m_19AndUnder(2) = "Two"
        m_19AndUnder(3) = "Three"
        m_19AndUnder(4) = "Four"
        m_19AndUnder(5) = "Five"
        m_19AndUnder(6) = "Six"
        m_19AndUnder(7) = "Seven"
        m_19AndUnder(8) = "Eight"
        m_19AndUnder(9) = "Nine"
        m_19AndUnder(10) = "Ten"
        m_19AndUnder(11) = "Eleven"
        m_19AndUnder(12) = "Twelve"
        m_19AndUnder(13) = "Thirteen"
        m_19AndUnder(14) = "Fourteen"
        m_19AndUnder(15) = "Fifteen"
        m_19AndUnder(16) = "Sixteen"
        m_19AndUnder(17) = "Seventeen"
        m_19AndUnder(18) = "Eighteen"
        m_19AndUnder(19) = "Nineteen"

        m_Tens(2) = "Twenty"
        m_Tens(3) = "Thirty"
        m_Tens(4) = "Forty"
        m_Tens(5) = "Fifty"
        m_Tens(6) = "Sixty"
        m_Tens(7) = "Seventy"
        m_Tens(8) = "Eighty"
        m_Tens(9) = "Ninety"

        m_Hundred = "Hundred"

        m_Groups(1) = ""
        m_Groups(2) = "Thousand"
        m_Groups(3) = "Million"
        m_Groups(4) = "Billion"
        m_Groups(5) = "Trillion"
        m_Groups(6) = "Quadrillion"
        m_Groups(7) = "Quintillion"
        m_Groups(8) = "Sextillion"
        m_Groups(9) = "Septillion"
        m_Groups(10) = "Octillion"

        m_Dollar = "" 'Dollar"
        m_Dollars = "" 'Dollars"

        m_NoCents = "No Cents"
        'm_Cent & m_Cents could both be changed to "/100"
        m_Cent = " Cent"
        m_Cents = " Cents"

        'Used for #s like: 23 = "Twenty-Three"
        m_Hyphen = "-"

        'Used between dollars & cents: "One Dollar and 12 Cents"
        m_And = " and "

        m_InvalidAmount = "Invalid Dollar Amount."
    End Sub

    Public Function MonetaryToWords(ByRef Value As Object) As String
        Dim decValue As Object
        Dim sValue As String
        Dim iDecimal As Short
        Dim sCents As String
        Dim sDollars As String
        Dim CentOnly As String
        On Error GoTo ER

        'Convert input into a Decimal value (up to 28 digits)
        'UPGRADE_WARNING: Couldn't resolve default property of object Value. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object decValue. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        decValue = CDec(Value)
        'UPGRADE_WARNING: Couldn't resolve default property of object decValue. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If decValue < 0 Then GoTo ER

        'Convert the Decimal value back into a string.  This eliminates
        '  any format characters such as "$" or ",".
        'UPGRADE_WARNING: Couldn't resolve default property of object decValue. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sValue = CStr(decValue)

        'Find the decimal point & extract the dollars from the cents
        iDecimal = InStr(1, sValue, ".")
        If iDecimal = 0 Then
            sDollars = sValue
            sCents = "00"
        Else
            'Extract decimal value
            sCents = Mid(sValue, iDecimal + 1)
            If Len(sCents) > 2 Then GoTo ER

            'Extract dollars
            sDollars = Left(sValue, iDecimal - 1)

            'Fill-out decimal places to two digits
            sCents = Left(sCents & "00", 2)
        End If

        'At this point,
        '  sDollars = the whole dollar value (0.. approx 79 Octillion)
        '  sCents   = the cents (00..99)

        System.Diagnostics.Debug.Assert(Len(sCents) = 2, "")
        System.Diagnostics.Debug.Assert(Len(sDollars) > 0, "")
        System.Diagnostics.Debug.Assert(Len(sDollars) < 31, "")
        CentOnly = sCents
        Select Case sCents

            Case "00"
                sCents = m_NoCents

            Case "01"
                sCents = m_Cent

            Case Else
                sCents = m_Cents
        End Select

        MonetaryToWords = DollarsToWords(sDollars) & m_And & DollarsToWords(CentOnly) & " " & sCents

        Exit Function

ER:

        MonetaryToWords = m_InvalidAmount
    End Function

    Private Function DollarsToWords(ByRef sDollars As String) As String
        Dim sWords As String
        Dim decValue As Object
        Dim sRemaining As String
        Dim s3Digits As String
        Dim iGroup As Short
        Dim i100s As Short
        Dim i10s As Short
        Dim i1s As Short
        Dim i99OrLess As Short
        Dim sWork As String

        'We had better be passing a valid number
        System.Diagnostics.Debug.Assert(IsNumeric(sDollars), "")

        'Check for special cases.  This also serves to validate the value
        'UPGRADE_WARNING: Couldn't resolve default property of object decValue. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        decValue = CDec(sDollars)
        Select Case decValue
            Case 0
                'UPGRADE_WARNING: Couldn't resolve default property of object decValue. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                DollarsToWords = m_19AndUnder(decValue) & m_Dollars
                Exit Function

            Case 1
                'UPGRADE_WARNING: Couldn't resolve default property of object decValue. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                DollarsToWords = m_19AndUnder(decValue) & m_Dollar
                Exit Function

        End Select

        'There should be no insignificant zeroes, "punctuation" or decimals
        'UPGRADE_WARNING: Couldn't resolve default property of object decValue. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        System.Diagnostics.Debug.Assert(sDollars = CStr(decValue), "")

        iGroup = 0
        sRemaining = sDollars
        sWords = ""

        'Extract each group of three digits, convert to words and prefix to result
        While Len(sRemaining) > 0
            iGroup = iGroup + 1

            'Extract next group of three digits
            If Len(sRemaining) > 3 Then
                s3Digits = Right(sRemaining, 3)
                sRemaining = Left(sRemaining, Len(sRemaining) - 3)
            Else
                'Fill-out group to three digits
                s3Digits = Right("00" & sRemaining, 3)
                sRemaining = ""
            End If

            System.Diagnostics.Debug.Assert(Len(s3Digits) = 3, "")

            If s3Digits <> "000" Then
                i100s = CShort(Left(s3Digits, 1))
                i10s = CShort(Mid(s3Digits, 2, 1))
                i1s = CShort(Right(s3Digits, 1))
                i99OrLess = (i10s * 10) + i1s
                sWork = " " & m_Groups(iGroup)

                Select Case True
                    'Do we have 20..99?
                    Case i10s > 1
                        System.Diagnostics.Debug.Assert(i10s <= 9, "")

                        If i1s > 0 Then
                            System.Diagnostics.Debug.Assert(i1s <= 9, "")

                            sWork = m_Tens(i10s) & m_Hyphen & m_19AndUnder(i1s) & sWork
                        Else
                            sWork = m_Tens(i10s) & sWork
                        End If

                        'Do we have 01..19?
                    Case i99OrLess > 0
                        System.Diagnostics.Debug.Assert(i99OrLess <= 99, "")

                        sWork = m_19AndUnder(i99OrLess) & sWork

                    Case Else
                        'If we're here, it's because there are no tens or ones
                        System.Diagnostics.Debug.Assert(i99OrLess = 0, "")
                        System.Diagnostics.Debug.Assert(i10s = 0, "")
                        System.Diagnostics.Debug.Assert(i1s = 0, "")
                        System.Diagnostics.Debug.Assert(Right(s3Digits, 2) = "00", "")

                        'If there's no tens or ones, there better be hundreds
                        System.Diagnostics.Debug.Assert(i100s > 0, "")

                End Select

                If i100s > 0 Then
                    System.Diagnostics.Debug.Assert(i100s <= 9, "")

                    sWork = m_19AndUnder(i100s) & " " & m_Hundred & " " & sWork
                End If

                System.Diagnostics.Debug.Assert(Len(Trim(sWork)) > 0, "")

                sWords = sWork & " " & sWords
            End If
        End While

        DollarsToWords = Trim(sWords) & m_Dollars
    End Function


    '    Public Function UFLTerbilang(ByVal nilai As Long) As String
    '     Dim bilangan() As String
    '     UFLbilangan = Split(",satu,dua,tiga,empat,lima,enam,tujuh,delapan,sembilan,sepuluh,sebelas", ",")
    '      If (nilai < 12) Then
    '        UFLTerbilang = " " + UFLbilangan(nilai)
    '      ElseIf (nilai < 20) Then
    '        UFLTerbilang = UFLTerbilang(nilai - 10) + " belas"
    '      ElseIf (nilai < 100) Then
    '        UFLTerbilang = UFLTerbilang(CLng((nilai \ 10))) + " puluh" + UFLTerbilang(nilai Mod 10)
    '      ElseIf (nilai < 200) Then
    '        UFLTerbilang = " seratus" + UFLTerbilang(nilai - 100)
    '      ElseIf (nilai < 1000) Then
    '        UFLTerbilang = UFLTerbilang(CLng(nilai \ 100)) + " ratus" + UFLTerbilang(nilai Mod 100)
    '      ElseIf (nilai < 2000) Then
    '        UFLTerbilang = " seribu" + UFLTerbilang(nilai - 1000)
    '      ElseIf (nilai < 1000000) Then
    '        UFLTerbilang = UFLTerbilang(CLng(nilai \ 1000)) + " ribu" + UFLTerbilang(nilai Mod 1000)
    '      ElseIf (nilai < 1000000000) Then
    '        UFLTerbilang = UFLTerbilang(CLng(nilai \ 1000000)) + " juta" + UFLTerbilang(nilai Mod 1000000)
    '      ElseIf (nilai < 1000000000000#) Then
    '        UFLTerbilang = UFLTerbilang(CLng(nilai \ 1000000000)) + " milyar" + UFLTerbilang(nilai Mod 1000000000)
    '      ElseIf (nilai < 1E+15) Then
    '        UFLTerbilang = UFLTerbilang(CLng((nilai \ 1000000000000#))) + " trilyun" + UFLTerbilang(nilai Mod 1000000000000#)
    '      Else
    '        UFLTerbilang = ""
    '      End If
    '    End Function

    'Rekursi error pakai \ terbatas 2 milyard
    ' Public Function UFLTerbilang(ByVal nilai As Int64) As String
    '     Dim bilangan() As String
    '     UFLbilangan = Split(",satu,dua,tiga,empat,lima,enam,tujuh,delapan,sembilan,sepuluh,sebelas", ",")
    '      If (nilai < 12) Then
    '        UFLTerbilang = " " + UFLbilangan(nilai)
    '      ElseIf (nilai < 20) Then
    '        UFLTerbilang = UFLTerbilang(nilai - 10) + " belas"
    '      ElseIf (nilai < 100) Then
    '        UFLTerbilang = UFLTerbilang(CLng((nilai \ 10))) + " puluh" + UFLTerbilang(nilai Mod 10)
    '      ElseIf (nilai < 200) Then
    '        UFLTerbilang = " seratus" + UFLTerbilang(nilai - 100)
    '      ElseIf (nilai < 1000) Then
    '        UFLTerbilang = UFLTerbilang(CLng(nilai \ 100)) + " ratus" + UFLTerbilang(nilai Mod 100)
    '      ElseIf (nilai < 2000) Then
    '        UFLTerbilang = " seribu" + UFLTerbilang(nilai - 1000)
    '      ElseIf (nilai < 1000000) Then
    '        UFLTerbilang = UFLTerbilang(CLng(nilai \ 1000)) + " ribu" + UFLTerbilang(nilai Mod 1000)
    '      ElseIf (nilai < 1000000000) Then
    '        UFLTerbilang = UFLTerbilang(CLng(nilai \ 1000000)) + " juta" + UFLTerbilang(nilai Mod 1000000)
    '      ElseIf (nilai < 1000000000000#) Then
    '        UFLTerbilang = UFLTerbilang(CLng(nilai \ 1000000000)) + " milyar" + UFLTerbilang(nilai Mod 1000000000)
    '      ElseIf (nilai < 1E+15) Then
    '        UFLTerbilang = UFLTerbilang(CLng((nilai \ 1000000000000#))) + " trilyun" + UFLTerbilang(nilai Mod 1000000000000#)
    '      Else
    '        UFLTerbilang = ""
    '      End If
    '    End Function

    Public Function UFLTerbilang(ByVal x As Double) As String
        'Dim Terbilang As Object
        Dim tampung As Double
        Dim teks As String
        Dim bagian As String
        Dim i As Short
        Dim tanda As Boolean

        Dim letak(5) As Object
        'UPGRADE_WARNING: Couldn't resolve default property of object letak(1). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        letak(1) = "ribu "
        'UPGRADE_WARNING: Couldn't resolve default property of object letak(2). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        letak(2) = "juta "
        'UPGRADE_WARNING: Couldn't resolve default property of object letak(3). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        letak(3) = "milyar "
        'UPGRADE_WARNING: Couldn't resolve default property of object letak(4). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        letak(4) = "trilyun "

        If (x = 0) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object Terbilang. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UFLTerbilang = "nol"
            Exit Function
        End If

        If (x < 2000) Then
            tanda = True
        End If

        teks = ""

        If (x >= 1.0E+15) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object Terbilang. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UFLTerbilang = "Nilai terlalu besar"
            Exit Function
        End If

        For i = 4 To 1 Step -1
            tampung = Int(x / (10 ^ (3 * i)))
            If (tampung > 0) Then
                bagian = ratusan(tampung, tanda)
                'UPGRADE_WARNING: Couldn't resolve default property of object letak(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                teks = teks & bagian & letak(i)
            End If
            x = x - tampung * (10 ^ (3 * i))
        Next

        teks = teks & ratusan(x, False)
        UFLTerbilang = teks
    End Function

    Function ratusan(ByVal y As Double, ByVal flag As Boolean) As String
        Dim tmp As Double
        Dim bilang As String
        Dim bag As String
        Dim j As Short

        Dim angka(9) As Object
        'UPGRADE_WARNING: Couldn't resolve default property of object angka(1). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        angka(1) = "se"
        'UPGRADE_WARNING: Couldn't resolve default property of object angka(2). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        angka(2) = "dua "
        'UPGRADE_WARNING: Couldn't resolve default property of object angka(3). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        angka(3) = "tiga "
        'UPGRADE_WARNING: Couldn't resolve default property of object angka(4). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        angka(4) = "empat "
        'UPGRADE_WARNING: Couldn't resolve default property of object angka(5). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        angka(5) = "lima "
        'UPGRADE_WARNING: Couldn't resolve default property of object angka(6). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        angka(6) = "enam "
        'UPGRADE_WARNING: Couldn't resolve default property of object angka(7). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        angka(7) = "tujuh "
        'UPGRADE_WARNING: Couldn't resolve default property of object angka(8). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        angka(8) = "delapan "
        'UPGRADE_WARNING: Couldn't resolve default property of object angka(9). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        angka(9) = "sembilan "

        Dim posisi(2) As Object
        'UPGRADE_WARNING: Couldn't resolve default property of object posisi(1). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        posisi(1) = "puluh "
        'UPGRADE_WARNING: Couldn't resolve default property of object posisi(2). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        posisi(2) = "ratus "

        bilang = ""
        For j = 2 To 1 Step -1
            tmp = Int(y / (10 ^ j))
            If (tmp > 0) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object angka(tmp). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                bag = angka(tmp)
                If (j = 1 And tmp = 1) Then
                    y = y - tmp * 10 ^ j
                    If (y >= 1) Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object posisi(j). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        posisi(j) = "belas "
                    Else
                        'UPGRADE_WARNING: Couldn't resolve default property of object angka(y). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        angka(y) = "se"
                    End If
                    'UPGRADE_WARNING: Couldn't resolve default property of object posisi(j). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object angka(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    bilang = bilang & angka(y) & posisi(j)
                    ratusan = bilang
                    Exit Function
                Else
                    'UPGRADE_WARNING: Couldn't resolve default property of object posisi(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    bilang = bilang & bag & posisi(j)
                End If
            End If
            y = y - tmp * 10 ^ j
        Next

        If (flag = False) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object angka(1). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            angka(1) = "satu "
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object angka(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        bilang = bilang & angka(y)
        ratusan = bilang
    End Function


    Public Function Terbilangkoma(ByVal nilai As Double) As String
        Terbilangkoma = UFLTerbilang(Int(nilai)) & " koma " & UFLTerbilang(System.Math.Round(nilai - Int(nilai), 2) * 100)
    End Function
    Public Function TerbilangSen(ByVal nilai As Double) As String
        TerbilangSen = UFLTerbilang(Int(nilai)) & " rupiah " & UFLTerbilang(System.Math.Round(nilai - Int(nilai), 2) * 100) & " sen"
    End Function

    Public Function TerbilangkomaEnglish(ByVal nilai As Double) As String
        'TerbilangkomaEnglish = MonetaryToWords(Int(nilai)) & " koma " & UFLTerbilang(Round(nilai - Int(nilai), 2) * 100)
        TerbilangkomaEnglish = MonetaryToWords(nilai)
    End Function
    Public Function TerbilangSenEnglish(ByVal nilai As Double) As String
        '/TerbilangSenEnglish = MonetaryToWords(Int(nilai)) & " rupiah " & UFLTerbilang(Round(nilai - Int(nilai), 2) * 100) & " sen"
        TerbilangSenEnglish = DollarsToWords(Format(nilai, "######.##"))
    End Function
End Class

Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraReports
Imports DevExpress.XtraEditors
Imports System.Data
Imports DevExpress.Utils
Imports System.IO
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.Native
Imports DevExpress.XtraReports.UserDesigner
Imports DevExpress.XtraPrinting.Native
Imports DevExpress.XtraPrinting.Preview
Imports DevExpress.XtraPrinting.Control
Imports DevExpress.XtraEditors.Repository
Imports System.Collections.Generic
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraReports.Extensions
Imports CtrlSoft.Utils

Public Class CetakDX
    Public Enum action_
        Edit = 0
        Preview = 1
        Print = 2
    End Enum
    Public Shared Function TextFileSave(ByVal strData As String, ByVal FullPath As String, Optional ByVal ErrInfo As String = "") As Boolean
        Dim Contents As String = ""
        Dim bAns As Boolean = False
        Dim objReader As StreamWriter
        Dim InfoFile As FileInfo = Nothing
        Try
            InfoFile = New FileInfo(FullPath)
            If Not InfoFile.Directory.Exists Then
                InfoFile.Directory.Create()
            End If
            objReader = New StreamWriter(FullPath)
            objReader.Write(strData)
            objReader.Close()
            bAns = True
        Catch Ex As Exception
            ErrInfo = Ex.Message
        End Try
        Return bAns
    End Function
    Public Shared Function TextFileGet(ByVal FullPath As String, Optional ByRef ErrInfo As String = "") As String
        Dim strContents As String = ""
        Dim objReader As StreamReader
        Try
            objReader = New StreamReader(FullPath)
            strContents = objReader.ReadToEnd()
            objReader.Close()
        Catch Ex As Exception
            ErrInfo = Ex.Message
        End Try
        Return strContents
    End Function
    Public Shared Function ViewXtraReport(ByVal frmParent As XtraForm, ByVal Action As action_, ByVal sReportName As String, ByVal Judul As String, ByVal RptName As String, ByVal DS As DataSet, Optional ByVal UkuranKertas As String = "", Optional ByVal CalculateFields As String = "", Optional ByVal ParameterField As String = "", Optional ByVal FilterString As String = "") As Boolean
        Dim Hasil As Boolean = False
        Dim XtraReport As DevExpress.XtraReports.UI.XtraReport = Nothing
        Dim dlg As WaitDialogForm = Nothing
        Dim Parameter() As String = Nothing
        Dim ValueField() As String = Nothing
        ' Create a new Security Permission which denies any File IO operations.
        Dim permission As New ScriptSecurityPermission("System.Security.Permissions.FileIOPermission")
        Try
            dlg = New WaitDialogForm("Sedang diproses...", "Mohon Tunggu Sebentar.")
            dlg.Show()
            dlg.TopMost = False
            If System.IO.File.Exists(sReportName) Then
                XtraReport = New DevExpress.XtraReports.UI.XtraReport
                XtraReport.LoadLayout(sReportName)
                If Not DS Is Nothing Then
                    If Not Directory.Exists(Application.StartupPath & "\Report\XCD") Then
                        System.IO.Directory.CreateDirectory(Application.StartupPath & "\Report\XCD")
                    End If
                    DS.WriteXmlSchema(Application.StartupPath & "\Report\XCD\" & Replace(RptName.ToUpper, ".REPX", "") & ".xsd")
                    XtraReport.DataSource = DS
                End If
                XtraReport.DataSourceSchema = Application.StartupPath & "\Report\XCD\" & Replace(RptName.ToUpper, ".REPX", "") & ".xsd"
                'XtraReport.PrinterName = ""
                If UkuranKertas <> "" Then
                    XtraReport.PaperName = UkuranKertas
                End If
                For i As Integer = 0 To XtraReport.CalculatedFields.Count - 1
                    Select Case XtraReport.CalculatedFields(i).Name.ToUpper
                        Case "NamaPerusahaan".ToUpper
                            XtraReport.CalculatedFields(i).Expression = "'" & FixApostropi(SettingPerusaan.NamaPerusahaan) & "'"
                        Case "AlamatPerusahaan".ToUpper
                            XtraReport.CalculatedFields(i).Expression = "'" & FixApostropi(SettingPerusaan.AlamatPerusahaan) & "'"
                        Case "KotaPerusahaan".ToUpper
                            XtraReport.CalculatedFields(i).Expression = "'" & FixApostropi(SettingPerusaan.KotaPerusahaan) & "'"
                        Case Else 'Selain Settingan Default
                            If CalculateFields <> "" Then
                                Parameter = CalculateFields.Split("|")
                                For x As Integer = 0 To Parameter.Length - 1
                                    ValueField = Parameter(x).Split("=")
                                    Select Case ValueField(1).ToUpper
                                        Case "Bit".ToUpper, "Boolean".ToUpper
                                            If XtraReport.CalculatedFields(i).Name.ToUpper = NullToStr(ValueField(0)).ToUpper Then
                                                XtraReport.CalculatedFields(i).Expression = CBool(ValueField(2)).ToString
                                            End If
                                        Case "Date".ToUpper, "Time".ToUpper, "Datetime".ToUpper
                                            If XtraReport.CalculatedFields(i).Name.ToUpper = NullToStr(ValueField(0)).ToUpper Then
                                                XtraReport.CalculatedFields(i).Expression = CDate(ValueField(2)).ToString("#yyyy-MM-dd#")
                                            End If
                                        Case "Int".ToUpper, "Integer".ToUpper, "Single".ToUpper, "Long".ToUpper
                                            If XtraReport.CalculatedFields(i).Name.ToUpper = NullToStr(ValueField(0)).ToUpper Then
                                                XtraReport.CalculatedFields(i).Expression = FixKoma(CLng(ValueField(2)))
                                            End If
                                        Case "Double".ToUpper, "Numeric".ToUpper, "Decimal".ToUpper
                                            If XtraReport.CalculatedFields(i).Name.ToUpper = NullToStr(ValueField(0)).ToUpper Then
                                                XtraReport.CalculatedFields(i).Expression = FixKoma(CDbl(ValueField(2)))
                                            End If
                                        Case "Money".ToUpper, "Currency".ToUpper
                                            If XtraReport.CalculatedFields(i).Name.ToUpper = NullToStr(ValueField(0)).ToUpper Then
                                                XtraReport.CalculatedFields(i).Expression = FixKoma(CDbl(ValueField(2)))
                                            End If
                                        Case Else
                                            If XtraReport.CalculatedFields(i).Name.ToUpper = NullToStr(ValueField(0)).ToUpper Then
                                                XtraReport.CalculatedFields(i).Expression = NullToStr(ValueField(2))
                                            End If
                                    End Select
                                Next
                            End If
                    End Select
                Next
                If ParameterField <> "" Then
                    Parameter = ParameterField.Split("|")
                    For i As Integer = 0 To Parameter.Length - 1
                        ValueField = Parameter(i).Split("=")
                        Select Case ValueField(1).ToUpper
                            Case "Bit".ToUpper, "Boolean".ToUpper
                                XtraReport.Parameters(ValueField(0)).Value = CBool(ValueField(2))
                            Case "Date".ToUpper, "Time".ToUpper, "Datetime".ToUpper
                                XtraReport.Parameters(ValueField(0)).Value = CDate(ValueField(2))
                            Case "Int".ToUpper, "Integer".ToUpper, "Single".ToUpper, "Long".ToUpper
                                XtraReport.Parameters(ValueField(0)).Value = CLng(ValueField(2))
                            Case "Double".ToUpper, "Numeric".ToUpper, "Decimal".ToUpper
                                XtraReport.Parameters(ValueField(0)).Value = CDbl(ValueField(2))
                            Case "Money".ToUpper, "Currency".ToUpper
                                XtraReport.Parameters(ValueField(0)).Value = CDbl(ValueField(2))
                            Case Else
                                XtraReport.Parameters(ValueField(0)).Value = CStr(ValueField(2))
                        End Select
                    Next
                    XtraReport.RequestParameters = False
                End If
                XtraReport.ScriptLanguage = ScriptLanguage.VisualBasic
                permission.Deny = True
                If FilterString <> "" Then
                    XtraReport.FilterString = FilterString
                End If

                ' Add this permission to a report's list of permissions for scripts.
                XtraReport.ScriptSecurityPermissions.Add(permission)
                'For Each i In XtraReport.ScriptSecurityPermissions
                '    XtraReport.ScriptSecurityPermissions.Item(i).Deny = True
                'Next

                'XtraReport.ScriptsSource.ToString()
                XtraReport.Name = RptName
                XtraReport.PrintingSystem.SetCommandVisibility(PrintingSystemCommand.ClosePreview, DevExpress.XtraPrinting.CommandVisibility.None)
                XtraReport.DisplayName = RptName
                'XtraReport.CreateDocument(True)

                If Action = action_.Edit Then
                    XtraReport.ShowDesignerDialog()
                ElseIf Action = action_.Preview Then
                    XtraReport.ShowPreviewDialog()
                ElseIf Action = action_.Print Then
                    XtraReport.PrintDialog()
                End If
            Else
                If XtraMessageBox.Show("File tidak ditemukan, lakukan mode design ?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    XtraReport = New DevExpress.XtraReports.UI.XtraReport
                    If Not DS Is Nothing Then
                        If Not Directory.Exists(Application.StartupPath & "\Report\XCD") Then
                            System.IO.Directory.CreateDirectory(Application.StartupPath & "\Report\XCD")
                        End If
                        DS.WriteXmlSchema(Application.StartupPath & "\Report\XCD\" & Replace(RptName.ToUpper, ".REPX", "") & ".xsd")
                        XtraReport.DataSource = DS
                    End If
                    XtraReport.DataSourceSchema = Application.StartupPath & "\Report\XCD\" & Replace(RptName.ToUpper, ".REPX", "") & ".xsd"
                    XtraReport.ReportUnit = ReportUnit.TenthsOfAMillimeter
                    XtraReport.PaperKind = System.Drawing.Printing.PaperKind.Custom
                    XtraReport.PrinterName = ""
                    XtraReport.PaperName = UkuranKertas
                    XtraReport.Name = Replace(RptName.ToUpper, ".REPX", "")
                    XtraReport.DisplayName = RptName
                    XtraReport.ShowDesignerDialog()
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            dlg.Close()
            dlg.Dispose()
            If Not DS Is Nothing AndAlso Action = action_.Print Then
                DS.Dispose()
            End If
            If Not XtraReport Is Nothing AndAlso Action = action_.Print Then
                XtraReport.Dispose()
            End If
        End Try
        Return Hasil
    End Function
End Class
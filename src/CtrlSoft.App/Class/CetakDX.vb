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
Imports CtrlSoft.App.Utils

Public Class CetakDX
    Public Enum ActionPrint
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
    Public Shared Function ViewXtraReport(ByVal frmParent As XtraForm, _
                                          ByVal Action As ActionPrint, _
                                          ByVal sReportName As String, _
                                          ByVal Judul As String, _
                                          ByVal RptName As String, _
                                          ByVal DS As DataSet, _
                                          Optional ByVal UkuranKertas As String = "", _
                                          Optional ByVal CalculateFields As List(Of Model.CetakDX.CalculateFields) = Nothing, _
                                          Optional ByVal ParameterField As List(Of Model.CetakDX.CalculateFields) = Nothing, _
                                          Optional ByVal FilterString As String = "") As Boolean
        Dim Hasil As Boolean = False
        Dim XtraReport As DevExpress.XtraReports.UI.XtraReport = Nothing
        Dim dlg As WaitDialogForm = Nothing
        Dim Parameter() As String = Nothing
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

                'Calculate Fields
                For i As Integer = 0 To XtraReport.CalculatedFields.Count - 1
                    Select Case XtraReport.CalculatedFields(i).Name.ToUpper
                        Case "NamaPerusahaan".ToUpper
                            XtraReport.CalculatedFields(i).Expression = "'" & FixApostropi(Utils.SettingPerusahaan.NamaPerusahaan) & "'"
                        Case "AlamatPerusahaan".ToUpper
                            XtraReport.CalculatedFields(i).Expression = "'" & FixApostropi(Utils.SettingPerusahaan.AlamatPerusahaan) & "'"
                        Case "KotaPerusahaan".ToUpper
                            XtraReport.CalculatedFields(i).Expression = "'" & FixApostropi(Utils.SettingPerusahaan.KotaPerusahaan) & "'"
                        Case Else 'Selain Settingan Default
                            If CalculateFields IsNot Nothing Then
                                For Each item As Model.CetakDX.CalculateFields In CalculateFields
                                    Select Case item.Type
                                        Case Model.CetakDX.CalculateFields.iType.VariantBit
                                            If XtraReport.CalculatedFields(i).Name.ToUpper = NullToStr(item.Name).ToUpper Then
                                                XtraReport.CalculatedFields(i).Expression = CBool(item.Value).ToString
                                            End If
                                        Case Model.CetakDX.CalculateFields.iType.VariantDateTime
                                            If XtraReport.CalculatedFields(i).Name.ToUpper = NullToStr(item.Name).ToUpper Then
                                                XtraReport.CalculatedFields(i).Expression = CDate(item.Value).ToString("#yyyy-MM-dd HH:mm:ss#")
                                            End If
                                        Case Model.CetakDX.CalculateFields.iType.VariantInt
                                            If XtraReport.CalculatedFields(i).Name.ToUpper = NullToStr(item.Name).ToUpper Then
                                                XtraReport.CalculatedFields(i).Expression = FixKoma(CLng(item.Value))
                                            End If
                                        Case Model.CetakDX.CalculateFields.iType.VariantDouble
                                            If XtraReport.CalculatedFields(i).Name.ToUpper = NullToStr(item.Name).ToUpper Then
                                                XtraReport.CalculatedFields(i).Expression = FixKoma(CDbl(item.Value))
                                            End If
                                        Case Model.CetakDX.CalculateFields.iType.String
                                            If XtraReport.CalculatedFields(i).Name.ToUpper = NullToStr(item.Name).ToUpper Then
                                                XtraReport.CalculatedFields(i).Expression = "'" & FixApostropi(NullToStr(item.Value)) & "'"
                                            End If
                                        Case Else
                                            If XtraReport.CalculatedFields(i).Name.ToUpper = NullToStr(item.Name).ToUpper Then
                                                XtraReport.CalculatedFields(i).Expression = NullToStr(item.Value)
                                            End If
                                    End Select
                                Next
                            End If
                    End Select
                Next

                'Parameter Fields
                If ParameterField IsNot Nothing Then
                    For Each item As Model.CetakDX.CalculateFields In ParameterField
                        Select Case item.Type
                            Case Model.CetakDX.CalculateFields.iType.VariantBit
                                XtraReport.Parameters(item.Name).Value = CBool(item.Value).ToString
                            Case Model.CetakDX.CalculateFields.iType.VariantDateTime
                                XtraReport.Parameters(item.Name).Value = CDate(item.Value).ToString("#yyyy-MM-dd HH:mm:ss#")
                            Case Model.CetakDX.CalculateFields.iType.VariantInt
                                XtraReport.Parameters(item.Name).Value = FixKoma(CLng(item.Value))
                            Case Model.CetakDX.CalculateFields.iType.VariantDouble
                                XtraReport.Parameters(item.Name).Value = FixKoma(CDbl(item.Value))
                            Case Model.CetakDX.CalculateFields.iType.String
                                XtraReport.Parameters(item.Name).Value = "'" & FixApostropi(NullToStr(item.Value)) & "'"
                            Case Else
                                XtraReport.Parameters(item.Name).Value = NullToStr(item.Value)
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

                If Action = ActionPrint.Edit Then
                    XtraReport.ShowDesignerDialog()
                ElseIf Action = ActionPrint.Preview Then
                    XtraReport.ShowPreviewDialog()
                ElseIf Action = ActionPrint.Print Then
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
            If Not DS Is Nothing AndAlso Action = ActionPrint.Print Then
                DS.Dispose()
            End If
            If Not XtraReport Is Nothing AndAlso Action = ActionPrint.Print Then
                XtraReport.Dispose()
            End If
        End Try
        Return Hasil
    End Function
End Class

Namespace Model.CetakDX
    Public Class CalculateFields
        Private _Name As String
        Private _Type As iType
        Private _Value As Object

        Public Enum iType
            [String] = 0
            [VariantInt] = 1
            [VariantDateTime] = 2
            [VariantBit] = 3
            [VariantDouble] = 4
        End Enum

        Public Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
            End Set
        End Property

        Public Property Type() As iType
            Get
                Return _Type
            End Get
            Set(ByVal value As iType)
                _Type = value
            End Set
        End Property

        Public Property Value() As Object
            Get
                Return _Value
            End Get
            Set(ByVal value As Object)
                _Value = value
            End Set
        End Property
    End Class
End Namespace
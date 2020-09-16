Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports CtrlSoft.App.Utils
Imports DevExpress.XtraEditors.Repository

Public Class frmEntriRole
    Public NoID As Long = -1
    Private pStatus As pStatusForm

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        DxErrorProvider1.ClearErrors()
        If TextEdit1.Text = "" Then
            DxErrorProvider1.SetError(TextEdit1, "Role harus diisi!")
        End If

        If Not DxErrorProvider1.HasErrors Then
            Using dlg As New WaitDialogForm("Sedang menyimpan data ...", NamaAplikasi)
                Using cn As New SqlConnection(StrKonSQL)
                    Using com As New SqlCommand
                        Using oDA As New SqlDataAdapter
                            Using ds As New DataSet
                                Try
                                    dlg.Show()
                                    dlg.Focus()
                                    cn.Open()
                                    com.Connection = cn
                                    com.Transaction = cn.BeginTransaction
                                    oDA.SelectCommand = com

                                    If pStatus = pStatusForm.Baru Then
                                        com.CommandText = "SELECT MAX(NoID) FROM MRole"
                                        NoID = NullToLong(com.ExecuteScalar()) + 1

                                        com.CommandText = "INSERT INTO MRole (NoID, [Role], IsSupervisor) VALUES (" & NoID & ", '" & FixApostropi(TextEdit1.Text) & "', " & IIf(CheckEdit1.Checked, 1, 0) & ")"
                                        com.ExecuteNonQuery()
                                    Else
                                        com.CommandText = "UPDATE MRole SET [Role]='" & FixApostropi(TextEdit1.Text) & "', IsSupervisor=" & IIf(CheckEdit1.Checked, 1, 0) & " WHERE NoID=" & NoID
                                        com.ExecuteNonQuery()
                                    End If

                                    com.CommandText = "DELETE FROM MRoleD WHERE IDRole=" & NoID
                                    com.ExecuteNonQuery()

                                    For Each row In BindingSource1.List
                                        com.CommandText = "INSERT INTO [dbo].[MRoleD] ([IDRole],[IDMenu],[IsActive]) VALUES (@IDRole,@IDMenu,@IsActive)"
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@IDRole", SqlDbType.BigInt)).Value = NoID
                                        com.Parameters.Add(New SqlParameter("@IDMenu", SqlDbType.Int)).Value = NullTolInt(row.Item("NoID"))
                                        com.Parameters.Add(New SqlParameter("@IsActive", SqlDbType.Bit)).Value = NullToBool(row.Item("Aktif"))
                                        com.ExecuteNonQuery()
                                    Next
                                    com.Transaction.Commit()

                                    DialogResult = Windows.Forms.DialogResult.OK
                                    Me.Close()
                                Catch ex As Exception
                                    XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                End Try
                            End Using
                        End Using
                    End Using
                End Using
            End Using
        End If
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Sub New(ByVal NoID As Long)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.NoID = NoID
    End Sub

    Private Sub frmEntriRole_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            SimpleButton1.ImageList = frmMain.ICButtons
            SimpleButton1.ImageIndex = 8
            SimpleButton2.ImageList = frmMain.ICButtons
            SimpleButton2.ImageIndex = 5

            LoadData(NoID)
            With LayoutControl1
                If System.IO.File.Exists(Utils.SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml") Then
                    .RestoreLayoutFromXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml")
                End If
            End With
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Windows.Forms.Cursor.Current = curentcursor
    End Sub

    Public Sub LoadData(ByVal NoID As Long)
        Using dlg As New WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using ds As New DataSet
                            Try
                                dlg.Show()
                                dlg.Focus()
                                cn.Open()
                                com.Connection = cn
                                oDA.SelectCommand = com

                                com.CommandText = "SELECT * FROM MRole WHERE NoID=" & NoID
                                oDA.Fill(ds, "MRole")

                                If ds.Tables("MRole").Rows.Count >= 1 Then
                                    pStatus = pStatusForm.Edit
                                    Me.NoID = NullToLong(ds.Tables("MRole").Rows(0).Item("NoID"))
                                    TextEdit1.Text = NullToStr(ds.Tables("MRole").Rows(0).Item("Role"))
                                    CheckEdit1.Checked = NullToBool(ds.Tables("MRole").Rows(0).Item("IsSupervisor"))

                                    If TextEdit1.Text = "ALL" OrElse TextEdit1.Text = "SU" Then
                                        SimpleButton1.Enabled = False
                                    Else
                                        SimpleButton1.Enabled = True
                                    End If

                                    com.CommandText = "EXEC [dbo].[spGenerateMenu] " & NoID
                                    com.ExecuteNonQuery()

                                    com.CommandText = "SELECT MMenu.NoID, MParent.Caption GroupMenu, MMenu.Caption, MMenu.NoUrut, MRoleD.IsActive Aktif" & vbCrLf & _
                                                      "FROM MRoleD" & vbCrLf & _
                                                      "INNER JOIN MMenu ON MMenu.NoID=MRoleD.IDMenu" & vbCrLf & _
                                                      "LEFT JOIN MMenu MParent ON MParent.NoID=MMenu.IDParent" & vbCrLf & _
                                                      "WHERE MMenu.IsActive=1 AND MParent.IsActive=1 AND MRoleD.IDRole=" & NoID & vbCrLf & _
                                                      "ORDER BY MMenu.NoID, MParent.Caption"
                                Else
                                    pStatus = pStatusForm.Baru
                                    Me.NoID = -1
                                    TextEdit1.Text = ""
                                    CheckEdit1.Checked = False

                                    com.CommandText = "SELECT MMenu.NoID, MParent.Caption GroupMenu, MMenu.Caption, MMenu.NoUrut, CONVERT(BIT, 0) Aktif" & vbCrLf & _
                                                      "FROM MMenu" & vbCrLf & _
                                                      "LEFT JOIN MMenu MParent ON MParent.NoID=MMenu.IDParent" & vbCrLf & _
                                                      "WHERE MMenu.IsActive=1 AND MParent.IsActive=1" & vbCrLf & _
                                                      "ORDER BY MMenu.NoID, MParent.Caption"
                                End If
                                oDA.Fill(ds, "MMenu")
                                BindingSource1.DataSource = ds.Tables("MMenu")
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Private Sub GridView1_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataSourceChanged
        With GridView1
            If System.IO.File.Exists(Utils.SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml") Then
                .RestoreLayoutFromXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml")
            End If
            For i As Integer = 0 To .Columns.Count - 1
                Select Case .Columns(i).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(i).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(i).DisplayFormat.FormatString = "n2"
                    Case "string"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        .Columns(i).DisplayFormat.FormatString = ""
                    Case "date"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                        .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                    Case "datetime"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                        .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
                    Case "byte[]"
                        reppicedit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
                        .Columns(i).OptionsColumn.AllowGroup = False
                        .Columns(i).OptionsColumn.AllowSort = False
                        .Columns(i).OptionsFilter.AllowFilter = False
                        .Columns(i).ColumnEdit = reppicedit
                    Case "boolean"
                        .Columns(i).ColumnEdit = repckedit
                End Select
            Next
        End With
    End Sub

    Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
        Try
            If e.FocusedColumn.FieldName.ToString = "Aktif" Then
                GridView1.OptionsBehavior.Editable = True
            Else
                GridView1.OptionsBehavior.Editable = False
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub GridView1_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
        Try
            If GridView1.FocusedColumn.FieldName.ToString = "Aktif" Then
                GridView1.OptionsBehavior.Editable = True
            Else
                GridView1.OptionsBehavior.Editable = False
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LayoutControl1_DefaultLayoutLoaded(ByVal sender As Object, ByVal e As System.EventArgs) Handles LayoutControl1.DefaultLayoutLoaded
        With LayoutControl1
            If System.IO.File.Exists(Utils.SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml") Then
                .RestoreLayoutFromXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml")
            End If
        End With
    End Sub

    Private Sub mnSaveLayout_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayout.ItemClick
        Using frm As New frmOtorisasi
            Try
                If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    GridView1.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & GridView1.Name & ".xml")
                    LayoutControl1.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & LayoutControl1.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub
End Class
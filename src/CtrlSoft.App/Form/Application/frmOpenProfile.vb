﻿Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports System.Data
Imports System.Data.SqlClient
Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.App.Public
Imports System.Data.Odbc
Imports Dapper
Imports DevExpress.XtraEditors.Controls

Public Class frmOpenProfile
    Public DBSetting As DBSettings.MDBSetting

    Private ListDB As New List(Of DBSettings.DBName)

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        DBSetting = [Public].DBSetting.Get("")

        Dim str As String() = {"(localdb)\MSSQLLocalDB", "(local)", "127.0.0.1", "localhost", "."}
        Dim collection As AutoCompleteStringCollection = New AutoCompleteStringCollection()
        collection.AddRange(str)
        txtServer.MaskBox.AutoCompleteCustomSource = collection
        txtServer.MaskBox.AutoCompleteSource = AutoCompleteSource.CustomSource
        txtServer.MaskBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend
    End Sub

    Private Sub frmSettingDB_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If DBSetting IsNot Nothing Then
            txtID.Text = DBSetting.Id
            txtServer.Text = DBSetting.Server
            txtDatabase.Text = DBSetting.Database
            txtUserID.Text = DBSetting.UserID
            txtPassword.Text = DBSetting.Password
            txtTimeout.Value = DBSetting.Timeout
            ckDefault.Checked = DBSetting.Default
        Else
            DBSetting = New DBSettings.MDBSetting With {.Id = ""}
            txtID.Text = DBSetting.Id
            txtServer.Text = "(localdb)\MSSQLLocalDB"
            txtDatabase.Text = ""
            txtUserID.Text = "sa"
            txtPassword.Text = "1234123412"
            txtTimeout.Value = 15
            ckDefault.Checked = True
        End If
        LookUpDB(txtServer.Text, txtUserID.Text, txtPassword.Text)
    End Sub

    Private Sub LookUpDB(ByVal Server As String, ByVal UserID As String, ByVal Pwd As String)
        Dim ListDB As New List(Of DBSettings.DBName)
        'Using dlg As New WaitDialogForm("Sedang menghubungkan ke database", NamaAplikasi)
        Using cn As New SqlConnection(IIf(Not Server.Equals("(localdb)\MSSQLLocalDB"),
                                              "Server=" & Server & ";Database=master;User Id=" & UserID & ";Password=" & Pwd & ";Timeout=3;",
                                              "Server=" & Server & ";Database=master;Integrated Security=true;Timeout=3;"))
            Using ds As New DataSet
                Using com As New SqlCommand
                    Try
                        'dlg.Show()

                        If (Server.Equals("(localdb)\MSSQLLocalDB")) Then
                            txtUserID.Properties.ReadOnly = True
                            txtPassword.Properties.ReadOnly = True
                        Else
                            txtUserID.Properties.ReadOnly = False
                            txtPassword.Properties.ReadOnly = False
                        End If

                        cn.Open()
                        com.Connection = cn
                        com.CommandTimeout = cn.ConnectionTimeout

                        ListDB = cn.Query(Of DBSettings.DBName)("SELECT T.[name] DBName FROM [sysdatabases] T WHERE T.[name] NOT IN ('master', 'tempdb', 'model', 'msdb', 'ReportServer', 'ReportServerTempDB')", Nothing)

                        Me.ListDB.Clear()
                        txtDatabase.Properties.Items.Clear()

                        For Each db In ListDB
                            com.CommandText = "SELECT COUNT([name]) AS X FROM [" & db.DBName & "].sys.tables WHERE name = 'TAppDB' OR name = 'MKartuStokOnHand'"
                            If NullToLong(com.ExecuteScalar()) >= 2 Then
                                'DBnya CtrlSoft
                                Me.ListDB.Add(db)
                            End If
                        Next

                        Dim temp As New List(Of String)
                        temp = (From x In Me.ListDB.ToList()
                                Select x.DBName).ToList()
                        txtDatabase.Properties.Items.AddRange(temp)
                    Catch ex As Exception
                        'XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End Try
                End Using
            End Using
        End Using
        'End Using
    End Sub

    Private Sub TextEdit1_LostFocus(sender As Object, e As EventArgs) Handles txtUserID.LostFocus, txtPassword.LostFocus
        LookUpDB(txtServer.Text, txtUserID.Text, txtPassword.Text)
    End Sub

    Private Sub mnSimpan_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSimpan.ItemClick
        Dim curentcursor As Cursor = System.Windows.Forms.Cursor.Current
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
        DxErrorProvider1.ClearErrors()
        If txtServer.Text = "" Then
            DxErrorProvider1.SetError(txtServer, "Server harus diisi!")
        End If
        If txtDatabase.Text = "" Then
            DxErrorProvider1.SetError(txtDatabase, "Database harus diisi!")
        End If
        If txtUserID.Text = "" Then
            DxErrorProvider1.SetError(txtUserID, "User ID harus diisi!")
        End If
        If txtPassword.Text = "" Then
            DxErrorProvider1.SetError(txtPassword, "Password harus diisi!")
        End If
        If txtTimeout.Text = "" OrElse NullToLong(txtTimeout.Value) <= 0 Then
            DxErrorProvider1.SetError(txtTimeout, "Timeout harus lebih dari 0!")
        End If

        If Not DxErrorProvider1.HasErrors Then
            Using cn As New SqlConnection(IIf(Not txtServer.Text.Equals("(localdb)\MSSQLLocalDB"),
                                              "Server=" & txtServer.Text & ";Database=" & txtDatabase.Text & ";User Id=" & txtUserID.Text & ";Password=" & txtPassword.Text & ";Timeout=3;",
                                              "Server=" & txtServer.Text & ";Database=" & txtDatabase.Text & ";Integrated Security=true;Timeout=3;"))
                Try
                    cn.Open()

                    'Save Koneksi
                    DBSetting.Server = txtServer.Text
                    DBSetting.Database = txtDatabase.Text
                    DBSetting.UserID = txtUserID.Text
                    DBSetting.Password = txtPassword.Text
                    DBSetting.Timeout = NullTolInt(txtTimeout.EditValue)
                    DBSetting.Default = ckDefault.Checked

                    DBSetting = [Public].DBSetting.Save(DBSetting)

                    DialogResult = System.Windows.Forms.DialogResult.OK
                    Close()
                Catch ex As Exception
                    XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Try
            End Using
        End If
        System.Windows.Forms.Cursor.Current = curentcursor
    End Sub

    Private Sub mnTutup_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnTutup.ItemClick
        DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class
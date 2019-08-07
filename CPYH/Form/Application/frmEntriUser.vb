Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports CtrlSoft.Utils
Imports DevExpress.XtraEditors.Repository

Public Class frmEntriUser
    Public NoID As Long = -1
    Private pStatus As pStatusForm

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        DxErrorProvider1.ClearErrors()
        If txtUserID.Text = "" Then
            DxErrorProvider1.SetError(txtUserID, "User ID harus diisi!")
        End If
        If txtPassword.Text = "" Then
            DxErrorProvider1.SetError(txtPassword, "Password harus diisi!")
        End If
        If txtNama.Text = "" Then
            DxErrorProvider1.SetError(txtNama, "Nama harus diisi!")
        End If
        If NullToLong(txtRole.EditValue) <= 0 Then
            DxErrorProvider1.SetError(txtRole, "Role User harus diisi!")
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
                                        com.CommandText = "SELECT MAX(NoID) FROM MUser"
                                        NoID = NullToLong(com.ExecuteScalar()) + 1

                                        com.CommandText = "INSERT INTO MUser (NoID, Kode, Nama, Pwd, IDRole, IDAlamat) VALUES (@NoID, @Kode, @Nama, @Pwd, @IDRole, @IDAlamat)"
                                    Else
                                        com.CommandText = "UPDATE MUser SET Kode=@Kode, Nama=@Nama, Pwd=@Pwd, IDRole=@IDRole, IDAlamat=@IDAlamat WHERE NoID=@NoID"
                                    End If
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NoID
                                    com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = txtUserID.Text
                                    com.Parameters.Add(New SqlParameter("@Nama", SqlDbType.VarChar)).Value = txtNama.Text
                                    com.Parameters.Add(New SqlParameter("@Pwd", SqlDbType.VarChar)).Value = txtPassword.Text
                                    com.Parameters.Add(New SqlParameter("@IDRole", SqlDbType.Int)).Value = txtRole.EditValue
                                    com.Parameters.Add(New SqlParameter("@IDAlamat", SqlDbType.Int)).Value = -1
                                    com.ExecuteNonQuery()

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
            LoadData(NoID)
            With LayoutControl1
                If System.IO.File.Exists(FolderLayouts & Me.Name & .Name & ".xml") Then
                    .RestoreLayoutFromXml(FolderLayouts & Me.Name & .Name & ".xml")
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
                                com.Transaction = cn.BeginTransaction
                                oDA.SelectCommand = com

                                com.CommandText = "SELECT NoID, [Role] FROM MRole"
                                oDA.Fill(ds, "MRole")
                                txtRole.Properties.DataSource = ds.Tables("MRole")
                                txtRole.Properties.ValueMember = "NoID"
                                txtRole.Properties.DisplayMember = "Role"
                                txtRole.EditValue = -1

                                com.CommandText = "SELECT * FROM MUser WHERE NoID=" & NoID
                                oDA.Fill(ds, "MUser")

                                If ds.Tables("MUser").Rows.Count >= 1 Then
                                    pStatus = pStatusForm.Edit
                                    Me.NoID = NullToLong(ds.Tables("MUser").Rows(0).Item("NoID"))
                                    txtUserID.Text = NullToStr(ds.Tables("MUser").Rows(0).Item("Kode"))
                                    txtNama.Text = NullToStr(ds.Tables("MUser").Rows(0).Item("Nama"))
                                    txtPassword.Text = NullToStr(ds.Tables("MUser").Rows(0).Item("Pwd"))
                                    txtRole.EditValue = NullTolInt(ds.Tables("MUser").Rows(0).Item("IDRole"))

                                    If txtUserID.Text = "ADM" OrElse txtUserID.Text = "SU" Then
                                        SimpleButton1.Enabled = False
                                    Else
                                        SimpleButton1.Enabled = True
                                    End If
                                Else
                                    pStatus = pStatusForm.Baru
                                    Me.NoID = -1
                                End If
                                com.Transaction.Commit()
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
        End Using
    End Sub
    Private Sub LayoutControl1_DefaultLayoutLoaded(ByVal sender As Object, ByVal e As System.EventArgs) Handles LayoutControl1.DefaultLayoutLoaded
        With LayoutControl1
            If System.IO.File.Exists(FolderLayouts & Me.Name & .Name & ".xml") Then
                .RestoreLayoutFromXml(FolderLayouts & Me.Name & .Name & ".xml")
            End If
        End With
    End Sub

    Private Sub mnSaveLayout_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayout.ItemClick
        Using frm As New frmOtorisasi
            Try
                If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    LayoutControl1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub
End Class
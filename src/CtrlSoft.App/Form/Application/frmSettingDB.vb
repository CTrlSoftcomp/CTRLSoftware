Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Tile
Imports CtrlSoft.App.Public

Public Class frmSettingDB
    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Private Data As New List(Of DBSettings.MDBSetting)

    Private Sub mnRefresh_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        Data = DBSetting.List.ToList()
        MDBSettingBindingSource.DataSource = Data
        GridControl1.DataSource = MDBSettingBindingSource
        GridControl1.RefreshDataSource()
    End Sub

    Private Sub frmSettingDB_Load(sender As Object, e As EventArgs) Handles Me.Load
        mnRefresh.PerformClick()
    End Sub

    Private Sub TileView1_DataSourceChanged(sender As Object, e As EventArgs) Handles TileView1.DataSourceChanged
        With TileView1
            If System.IO.File.Exists(SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml") Then
                .RestoreLayoutFromXml(SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml")
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

    Private Sub mnBaru_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnBaru.ItemClick
        Using frm As New frmSettingDBEntri("")
            Try
                If frm.ShowDialog(Me) = DialogResult.OK Then
                    mnRefresh.PerformClick()

                    'TileView1.ClearSelection()
                    'TileView1.FocusedRowHandle = TileView1.LocateByDisplayText(0, colId, frm.DBSetting.Id)
                    'TileView1.SelectRow(TileView1.FocusedRowHandle)
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub mnEdit_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnEdit.ItemClick
        Dim Obj As DBSettings.MDBSetting = MDBSettingBindingSource.Current
        If Obj IsNot Nothing Then
            Using frm As New frmSettingDBEntri(Obj.Id)
                Try
                    If frm.ShowDialog(Me) = DialogResult.OK Then
                        mnRefresh.PerformClick()

                        'TileView1.ClearSelection()
                        'TileView1.FocusedRowHandle = TileView1.LocateByDisplayText(0, colId, frm.DBSetting.Id)
                        'TileView1.SelectRow(TileView1.FocusedRowHandle)
                    End If
                Catch ex As Exception
                    XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End If
    End Sub

    Private Sub mnHapus_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHapus.ItemClick
        Try
            If DBSetting.List.Count > 1 Then
                Dim Obj As DBSettings.MDBSetting = MDBSettingBindingSource.Current

                If Obj IsNot Nothing AndAlso Not Obj.Id.Equals("DEFAULT") Then
                    Obj = DBSetting.Delete(Obj.Id)
                    If Obj IsNot Nothing Then
                        mnRefresh.PerformClick()
                    End If
                Else
                    XtraMessageBox.Show("Setting koneksi utama tidak dapat dihapus.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                XtraMessageBox.Show("Setting koneksi hanya yang utama.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub mnTutup_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnTutup.ItemClick
        DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub TileView1_ItemDoubleClick(sender As Object, e As TileViewItemClickEventArgs) Handles TileView1.ItemDoubleClick
        mnEdit.PerformClick()
    End Sub

    Private Sub mnPilih_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPilih.ItemClick
        Try
            Dim Obj = MDBSettingBindingSource.Current

            If Obj IsNot Nothing Then
                TryCast(Obj, DBSettings.MDBSetting).Default = True
                Obj = DBSetting.Save(Obj)
                If Obj IsNot Nothing Then
                    DialogResult = DialogResult.OK
                    Me.Close()
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
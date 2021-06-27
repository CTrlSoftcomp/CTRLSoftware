Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.App.Public
Imports DevExpress.XtraEditors.Repository
Imports Dapper
Imports DevExpress.XtraTab.ViewInfo

Public Class frmEntriMaterialDBiaya
    Private pStatus As pStatusForm
    Public data As New CtrlSoft.Dto.Model.MMaterialDBiaya
    Public header As New CtrlSoft.Dto.Model.MMaterial

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Public Sub New(ByVal data As CtrlSoft.Dto.Model.MMaterialDBiaya,
                   ByVal header As CtrlSoft.Dto.Model.MMaterial)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.data = data
        Me.header = header
    End Sub

    Private Sub frmEntriRole_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim curentcursor As Cursor = System.Windows.Forms.Cursor.Current
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            LoadData(data.NoID)
            With LayoutControl1
                If System.IO.File.Exists([Public].SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml") Then
                    .RestoreLayoutFromXml([Public].SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml")
                End If
            End With
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        System.Windows.Forms.Cursor.Current = curentcursor
    End Sub

    Public Overridable Sub LoadData(ByVal NoID As Guid)
        Dim Args As New Dictionary(Of String, Object)
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

                                com.CommandText = "SELECT MAkun.ID, MAkun.Kode, MAkun.Nama, MKlasAkun.Klasifikasi " & vbCrLf &
                                                  "FROM MAkun(NOLOCK) " & vbCrLf &
                                                  "LEFT JOIN MKlasAkun(NOLOCK) ON MAkun.IDKlasAkun = MKlasAkun.ID " & vbCrLf &
                                                  "WHERE MKlasAkun.ID IN ('FAFA4F6B-E6E1-40B9-8C37-9A332D73E4A1') AND MAkun.ID NOT IN (SELECT IDParent FROM MAkun WHERE IDParent IS NOT NULL) " & vbCrLf &
                                                  "ORDER BY MKlasAkun.NoUrut, MAkun.Kode"
                                oDA.Fill(ds, "MAkun")
                                txtAkun.Properties.DataSource = ds.Tables("MAkun")
                                txtAkun.Properties.ValueMember = "ID"
                                txtAkun.Properties.DisplayMember = "Nama"
                                txtAkun.EditValue = -1

                                If (data IsNot Nothing AndAlso Not IsDBNull(data.IDAkun)) Then
                                    pStatus = pStatusForm.Edit
                                    txtAkun.EditValue = data.IDAkun
                                    txtNilai.EditValue = data.Jumlah
                                    txtKeterangan.Text = data.Keterangan
                                Else
                                    pStatus = pStatusForm.Baru
                                    data.NoID = System.Guid.NewGuid()
                                    txtAkun.EditValue = ""
                                    txtNilai.EditValue = 0.0
                                    txtKeterangan.Text = ""
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
            If System.IO.File.Exists([Public].SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml") Then
                .RestoreLayoutFromXml([Public].SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml")
            End If
        End With
    End Sub

    Private Sub mnSaveLayout_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayout.ItemClick
        Using frm As New frmOtorisasi
            Try
                If frm.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                    LayoutControl1.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & LayoutControl1.Name & ".xml")

                    gvAkun.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & gvAkun.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub mnSimpan_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSimpan.ItemClick
        SimpanData()
    End Sub

    Public Overridable Sub SimpanData()
        DxErrorProvider1.ClearErrors()
        If txtAkun.Text = "" Then
            DxErrorProvider1.SetError(txtAkun, "Akun harus diisi!")
        End If
        If txtNilai.EditValue <= 0 Then
            DxErrorProvider1.SetError(txtNilai, "Nilai Barang harus diisi!")
        End If

        If Not DxErrorProvider1.HasErrors Then
            Using dlg As New WaitDialogForm("Sedang menyimpan data ...", NamaAplikasi)
                Try
                    dlg.Show()
                    dlg.Focus()

                    With data
                        .IDAkun = txtAkun.EditValue
                        .Akun = txtAkun.Text
                        .Keterangan = txtKeterangan.Text
                        .Jumlah = txtNilai.EditValue
                        .IDMaterial = data.IDMaterial
                    End With
                    DialogResult = System.Windows.Forms.DialogResult.OK
                    Me.Close()
                Catch ex As Exception
                    XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End If
    End Sub

    Private Sub mnTutup_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnTutup.ItemClick
        DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub gvBahan_DataSourceChanged(sender As Object, e As EventArgs) Handles gvAkun.DataSourceChanged
        With sender
            If System.IO.File.Exists(CtrlSoft.App.Public.SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml") Then
                .RestoreLayoutFromXml(CtrlSoft.App.Public.SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml")
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

    Private Sub mnRefresh_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        mnRefreshClick()
    End Sub

    Public Overridable Sub mnRefreshClick()
        LoadData(data.NoID)
    End Sub
End Class
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports System.IO
Imports System.Diagnostics
Imports DevExpress.XtraEditors
Imports DevExpress.Skins
Imports DevExpress.XtraBars.Ribbon
Imports DevExpress.XtraBars.Ribbon.Gallery
Imports DevExpress.Utils.Drawing
Imports DevExpress.Utils
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.LookAndFeel
Imports CtrlSoft.Ini
Imports CtrlSoft.Utils
Imports CtrlSoft.CetakDX
Imports DevExpress.XtraBars
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo

Public Class frmDaftarTransaksi
    Private formName As modMain.FormName
    Private ds As New DataSet

    Private SQL As String

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit
    Private WithEvents frmHasilPosting As New frmHasilPosting(-1, -1)

    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        RefreshData(-1)
    End Sub

    Public Sub RefreshData(ByVal NoID As Long)
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

                                Select Case formName
                                    Case modMain.FormName.DaftarPO
                                        SQL = "spDaftarPO @TglDari, @TglSampai"
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@TglDari", SqlDbType.Date)).Value = DateEdit1.EditValue
                                        com.Parameters.Add(New SqlParameter("@TglSampai", SqlDbType.Date)).Value = DateEdit2.EditValue
                                    Case modMain.FormName.DaftarPembelian
                                        SQL = "spDaftarBeli @TglDari, @TglSampai"
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@TglDari", SqlDbType.Date)).Value = DateEdit1.EditValue
                                        com.Parameters.Add(New SqlParameter("@TglSampai", SqlDbType.Date)).Value = DateEdit2.EditValue
                                End Select

                                com.CommandText = SQL
                                oDA.Fill(ds, "MData")
                                BindingSource1.DataSource = ds.Tables("MData")

                                GridView1.ClearSelection()
                                GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), NoID.ToString("n0"))
                                GridView1.SelectRow(GridView1.FocusedRowHandle)

                                Me.ds = ds
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Private Sub cmdCetak_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCetak.Click
        Dim NamaFile As String = "", Judul As String = ""
        Using ds As New DataSet
            Using con As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Try
                            con.Open()
                            com.Connection = con
                            com.CommandTimeout = con.ConnectionTimeout
                            oDA.SelectCommand = com
                            Select Case formName
                                Case modMain.FormName.DaftarPO
                                    Repository.PostingData.PostingPO(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))

                                    com.CommandText = "spFakturMPO @NoID"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                                    oDA.Fill(ds, formName.ToString)
                                    NamaFile = "Faktur_MPO.repx"
                                    Judul = "Faktur Pesanan"
                                    ViewXtraReport(Me.MdiParent, IIf(IsEditReport, action_.Edit, action_.Preview), Application.StartupPath & "\Report\" & NamaFile, Judul, NamaFile, ds)
                                Case modMain.FormName.DaftarPembelian
                                    Repository.PostingData.PostingBeli(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))

                                    com.CommandText = "spFakturMBeli @NoID"
                                    com.Parameters.Clear()
                                    com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                                    oDA.Fill(ds, formName.ToString)
                                    NamaFile = "Faktur_MBeli.repx"
                                    Judul = "Faktur Pembelian"
                                    ViewXtraReport(Me.MdiParent, IIf(IsEditReport, action_.Edit, action_.Preview), Application.StartupPath & "\Report\" & NamaFile, Judul, NamaFile, ds)
                            End Select
                        Catch ex As Exception
                            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Private Sub cmdHapus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHapus.Click
        Dim gridview As DevExpress.XtraGrid.Views.Grid.GridView = Nothing
        gridview = GridView1
        If gridview.RowCount >= 1 Then
            Select Case formName
                Case modMain.FormName.DaftarPO
                    If XtraMessageBox.Show("Ingin menghapus data pesanan " & NullToStr(gridview.GetRowCellValue(gridview.FocusedRowHandle, "Kode")) & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        HapusData(NullToLong(gridview.GetRowCellValue(gridview.FocusedRowHandle, "NoID")))
                    End If
                Case modMain.FormName.DaftarPembelian
                    If XtraMessageBox.Show("Ingin menghapus data pembelian " & NullToStr(gridview.GetRowCellValue(gridview.FocusedRowHandle, "Kode")) & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        HapusData(NullToLong(gridview.GetRowCellValue(gridview.FocusedRowHandle, "NoID")))
                    End If
            End Select
        End If
    End Sub

    Private Sub HapusData(ByVal NoID As Long)
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

                                Select Case formName
                                    Case modMain.FormName.DaftarPO
                                        com.CommandText = "DELETE MPOD FROM MPOD INNER JOIN MPO ON MPO.NoID=MPOD.IDPO WHERE ISNULL(MPO.IsPosted, 0)=0 AND MPO.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE FROM MPO WHERE ISNULL(MPO.IsPosted, 0)=0 AND MPO.NoID=" & NoID
                                        com.ExecuteNonQuery()
                                    Case modMain.FormName.DaftarPembelian
                                        com.CommandText = "DELETE MBeliD FROM MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli WHERE ISNULL(MBeli.IsPosted, 0)=0 AND MBeli.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "DELETE FROM MBeli WHERE ISNULL(MBeli.IsPosted, 0)=0 AND MBeli.NoID=" & NoID
                                        com.ExecuteNonQuery()
                                End Select

                                If com.Transaction IsNot Nothing Then
                                    com.Transaction.Commit()
                                    com.Transaction = Nothing
                                    cmdRefresh.PerformClick()
                                End If
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        Select Case formName
            Case modMain.FormName.DaftarPO
                Dim x As New frmEntriPO(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
                x.MdiParent = Me.MdiParent
                x.Show()
                x.Focus()
            Case modMain.FormName.DaftarPembelian
                Dim x As New frmEntriBeli(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
                x.MdiParent = Me.MdiParent
                x.Show()
                x.Focus()
        End Select
    End Sub

    Private Sub cmdBaru_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBaru.Click
        Select Case formName
            Case modMain.FormName.DaftarPO
                Dim x As New frmEntriPO(-1)
                x.MdiParent = Me.MdiParent
                x.Show()
                x.Focus()
            Case modMain.FormName.DaftarPembelian
                Dim x As New frmEntriBeli(-1)
                x.MdiParent = Me.MdiParent
                x.Show()
                x.Focus()
        End Select
    End Sub

    Public Sub New(ByVal formName As modMain.FormName, ByVal caption As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Text = caption
        Me.formName = formName
        Me.SQL = SQL
        Me.Tag = Me.formName
        Me.Name = Me.formName.ToString
    End Sub

    Private Sub GridView1_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataSourceChanged
        With sender
            If System.IO.File.Exists(FolderLayouts & Me.Name & .Name & ".xml") Then
                .RestoreLayoutFromXml(FolderLayouts & Me.Name & .Name & ".xml")
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

    Private Sub GridView1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridView1.MouseDown
        Dim View As GridView = CType(sender, GridView)
        If View Is Nothing Then Return
        ' obtaining hit info
        Dim hitInfo As GridHitInfo = View.CalcHitInfo(New System.Drawing.Point(e.X, e.Y))
        If (e.Button = Windows.Forms.MouseButtons.Right) And (hitInfo.InRow) And _
          (Not View.IsGroupRow(hitInfo.RowHandle)) Then
            PopupMenu1.ShowPopup(Control.MousePosition)
        End If
    End Sub

    Private Sub frmDaftarMaster_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try

        Catch ex As Exception

        End Try
    End Sub

    Private Sub frmDaftarMaster_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            cmdBaru.ImageList = frmMain.ICButtons
            cmdEdit.ImageList = frmMain.ICButtons
            cmdHapus.ImageList = frmMain.ICButtons
            cmdCetak.ImageList = frmMain.ICButtons
            cmdRefresh.ImageList = frmMain.ICButtons
            cmdTutup.ImageList = frmMain.ICButtons

            LabelControl1.Text = Me.Text
            DateEdit1.EditValue = Now.AddDays(-7)
            DateEdit2.EditValue = Now
            RefreshData(-1)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub mnSaveLayout_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayout.ItemClick
        Using frm As New frmOtorisasi
            Try
                If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    GridView1.SaveLayoutToXml(FolderLayouts & Me.Name & GridView1.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub mnPosting_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPosting.ItemClick
        Using dlg As New WaitDialogForm("Sedang memosting data ...", NamaAplikasi)
            Dim MyCursor As Cursor = Windows.Forms.Cursor.Current
            Dim NoID As Long = -1
            Try
                Cursor = Cursors.WaitCursor
                dlg.Show()
                dlg.Focus()

                For Each iRow In GridView1.GetSelectedRows
                    Select Case formName
                        Case modMain.FormName.DaftarPO
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            If Not Repository.PostingData.PostingPO(NoID) Then
                                Exit For
                            End If
                        Case modMain.FormName.DaftarPembelian
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            If Not Repository.PostingData.PostingBeli(NoID) Then
                                Exit For
                            End If
                        Case Else
                            Exit For
                    End Select
                    cmdRefresh.PerformClick()
                Next
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                Cursor = MyCursor
            End Try
        End Using
    End Sub

    Private Sub mnUnposting_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnUnposting.ItemClick
        Using dlg As New WaitDialogForm("Sedang mengunposting data ...", NamaAplikasi)
            Dim MyCursor As Cursor = Windows.Forms.Cursor.Current
            Dim NoID As Long = -1
            Try
                Cursor = Cursors.WaitCursor
                dlg.Show()
                dlg.Focus()

                For Each iRow In GridView1.GetSelectedRows
                    Select Case formName
                        Case modMain.FormName.DaftarPO
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            If Not Repository.UnPostingData.UnPostingPO(NoID) Then
                                Exit For
                            End If
                        Case modMain.FormName.DaftarPembelian
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            If Not Repository.UnPostingData.UnPostingBeli(NoID) Then
                                Exit For
                            End If
                        Case Else
                            Exit For
                    End Select
                    cmdRefresh.PerformClick()
                Next
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                Cursor = MyCursor
            End Try
        End Using
    End Sub

    Private Sub mnHasilPosting_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHasilPosting.ItemClick
        Using dlg As New WaitDialogForm("Sedang mengambil data ...", NamaAplikasi)
            Dim MyCursor As Cursor = Windows.Forms.Cursor.Current
            Dim NoID As Long = -1
            Try
                Cursor = Cursors.WaitCursor
                dlg.Show()
                dlg.Focus()

                For Each iRow In GridView1.GetSelectedRows
                    Select Case formName
                        Case modMain.FormName.DaftarPembelian
                            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                            frmHasilPosting = New frmHasilPosting(NoID, 2)
                            frmHasilPosting.StartPosition = FormStartPosition.CenterScreen
                            frmHasilPosting.Show()
                            frmHasilPosting.Focus()
                        Case Else
                            Exit For
                    End Select
                Next
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                Cursor = MyCursor
            End Try
        End Using
    End Sub
End Class
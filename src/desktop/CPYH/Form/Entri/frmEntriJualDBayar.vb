Imports CtrlSoft.Utils
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors
Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Grid

Public Class frmEntriJualDBayar
    Public iList As New List(Of Model.Pembayaran)
    Private Total As Double
    Private IDCustomer As Long

    Private Sub mnTambah_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnTambah.ItemClick
        Try
            For Each obj In iList
                If obj.IDJenisPembayaran <= 0 Then
                    GridView1.ClearSelection()
                    GridView1.FocusedRowHandle = iList.IndexOf(obj)
                    GridView1.SelectRow(GridView1.FocusedRowHandle)
                    Exit Try
                End If
            Next
            iList.Add(New Model.Pembayaran With {.IDJenisPembayaran = 1, .IsBank = False, .Nominal = 0.0, .ChargeProsen = 0.0, .ChargeRp = 0.0, .Total = 0.0})
            GridControl1.RefreshDataSource()
            GridView1.ClearSelection()
            GridView1.FocusedRowHandle = GridView1.RowCount - 1
            GridView1.SelectRow(GridView1.FocusedRowHandle)
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function GetJenisPembayaran(ByVal NoID As Long) As Model.Pembayaran
        Dim Obj As New Model.Pembayaran
        Using cn As New SqlConnection(StrKonSQL)
            Using com As New SqlCommand
                Using oDA As New SqlDataAdapter
                    Using ds As New DataSet
                        Try
                            cn.Open()
                            com.Connection = cn
                            com.CommandTimeout = cn.ConnectionTimeout
                            com.Transaction = cn.BeginTransaction

                            oDA.SelectCommand = com

                            com.CommandText = "SELECT * FROM MJenisPembayaran WHERE NoID=" & NoID
                            oDA.Fill(ds, "MJenisPembayaran")
                            For Each iRow As DataRow In ds.Tables("MJenisPembayaran").Rows
                                Obj = New Model.Pembayaran With {.NoID = NullToLong(iRow.Item("NoID")), _
                                                                 .IDJenisPembayaran = NullToLong(iRow.Item("NoID")), _
                                                                 .AtasNama = "", _
                                                                 .NoRekening = "", _
                                                                 .IsBank = NullToBool(iRow.Item("IsBank")), _
                                                                 .ChargeProsen = NullToDbl(iRow.Item("ChargeProsen")), _
                                                                 .ChargeRp = 0.0, _
                                                                 .Total = 0.0, _
                                                                 .Nominal = 0.0}
                            Next
                        Catch ex As Exception
                            Obj = New Model.Pembayaran
                        End Try
                    End Using
                End Using
            End Using
        End Using
        Return Obj
    End Function

    Private IsLagiEdit As Boolean = False
    Private Sub GridView1_CellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        Dim pembayaran As Model.Pembayaran
        Try
            If Not IsLagiEdit Then
                Select Case GridView1.FocusedColumn.FieldName.ToLower
                    Case "IDJenisPembayaran".ToLower
                        IsLagiEdit = True
                        pembayaran = GetJenisPembayaran(NullToLong(e.Value))
                        GridView1.SetRowCellValue(GridView1.FocusedRowHandle, "IsBank", pembayaran.IsBank)
                        GridView1.SetRowCellValue(GridView1.FocusedRowHandle, "ChargeProsen", pembayaran.ChargeProsen)
                        GridView1.SetRowCellValue(GridView1.FocusedRowHandle, "ChargeRp", Bulatkan(NullToDbl(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "Nominal")) * (pembayaran.ChargeProsen / 100), 2))
                        GridView1.SetRowCellValue(GridView1.FocusedRowHandle, "Total", Bulatkan(NullToDbl(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "Nominal")) * (1 + pembayaran.ChargeProsen / 100), 2))

                        'GridView1.Columns("NoRekening").OptionsColumn.AllowEdit = NullToBool(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "IsBank"))
                        'GridView1.Columns("NoRekening").OptionsColumn.AllowFocus = NullToBool(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "IsBank"))
                        'GridView1.Columns("AtasNama").OptionsColumn.AllowEdit = NullToBool(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "IsBank"))
                        'GridView1.Columns("AtasNama").OptionsColumn.AllowFocus = NullToBool(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "IsBank"))
                        IsLagiEdit = False
                    Case "Nominal".ToLower
                        IsLagiEdit = True
                        GridView1.SetRowCellValue(GridView1.FocusedRowHandle, "ChargeRp", Bulatkan(NullToDbl(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "Nominal")) * (NullToDbl(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "ChargeProsen")) / 100), 2))
                        GridView1.SetRowCellValue(GridView1.FocusedRowHandle, "Total", Bulatkan(NullToDbl(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "Nominal")) * (1 + NullToDbl(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "ChargeProsen")) / 100), 2))
                        IsLagiEdit = False
                End Select
            End If
        Catch ex As Exception

        Finally
            HitungJumlah()
        End Try
    End Sub

    Private Sub GV1_RowCellStyle(ByVal sender As Object, ByVal e As RowCellStyleEventArgs) Handles GridView1.RowCellStyle
        Try
            Dim view As GridView = TryCast(sender, GridView)
            Dim IsBank As Boolean = NullToBool(view.GetRowCellValue(e.RowHandle, "IsBank"))
            Select Case e.Column.FieldName.ToLower
                Case "AtasNama".ToLower, "NoRekening".ToLower
                    If Not IsBank Then
                        e.Appearance.BackColor = Color.Silver
                    End If
            End Select
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub InitLoadLookUpPO()
        Using cn As New SqlConnection(StrKonSQL)
            Using com As New SqlCommand
                Using oDA As New SqlDataAdapter
                    Using ds As New DataSet
                        Try
                            cn.Open()
                            com.Connection = cn
                            com.CommandTimeout = cn.ConnectionTimeout
                            oDA.SelectCommand = com

                            com.CommandText = "SELECT NoID, Nama AS Pembayaran FROM MJenisPembayaran"
                            oDA.Fill(ds, "MJenisPembayaran")
                            riSearchLookUpEdit.DataSource = ds.Tables("MJenisPembayaran")
                            riSearchLookUpEdit.ValueMember = "NoID"
                            riSearchLookUpEdit.DisplayMember = "Pembayaran"
                        Catch ex As Exception
                            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Private Sub RefreshData()
        Try
            PembayaranBindingSource.DataSource = iList
            GridControl1.RefreshDataSource()
            HitungJumlah()
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub mnHapus_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHapus.ItemClick
        Try
            GridView1.DeleteRow(GridView1.FocusedRowHandle)
            PembayaranBindingSource.DataSource = iList
            GridControl1.RefreshDataSource()
            HitungJumlah()
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function IsValidasi() As Boolean
        DxErrorProvider1.ClearErrors()
        If NullToDbl(Subtotal) < 0.0 Then
            DxErrorProvider1.SetError(txtTotalBayar, "Penjualan anda salah input!", DXErrorProvider.ErrorType.Critical)
        End If

        'Cek Limit Piutang
        Using cn As New SqlConnection(StrKonSQL)
            Using com As New SqlCommand
                Using oDA As New SqlDataAdapter
                    Using ds As New DataSet
                        Try
                            cn.Open()
                            com.Connection = cn
                            com.CommandTimeout = cn.ConnectionTimeout
                            oDA.SelectCommand = com

                            com.CommandText = "SELECT * FROM MAlamat WHERE NoID=" & IDCustomer
                            oDA.Fill(ds, "MAlamat")

                            If ds.Tables("MAlamat").Rows.Count >= 1 Then
                                com.CommandText = "SELECT A.IDAlamat" & vbCrLf & _
                                                  ",SUM((A.Debet - A.Kredit) - ISNULL(B.Pelunasan, 0)) AS Saldo" & vbCrLf & _
                                                  ",COUNT(A.NoTransaksi) AS JmlNota" & vbCrLf & _
                                                  ",DATEDIFF(DAY, MAX(A.Tanggal), GETDATE()) TglTerlamaPiutang" & vbCrLf & _
                                                  "FROM MHutangPiutang A(NOLOCK)" & vbCrLf & _
                                                  "LEFT JOIN (" & vbCrLf & _
                                                  "SELECT ReffNoTransaksi" & vbCrLf & _
                                                  ",ReffNoUrut" & vbCrLf & _
                                                  ",SUM(Kredit - Debet) AS Pelunasan" & vbCrLf & _
                                                  "FROM MHutangPiutang(NOLOCK)" & vbCrLf & _
                                                  "GROUP BY ReffNoTransaksi" & vbCrLf & _
                                                  ",ReffNoUrut" & vbCrLf & _
                                                  ") AS B ON B.ReffNoTransaksi = A.NoTransaksi" & vbCrLf & _
                                                  "AND B.ReffNoUrut = A.NoUrut" & vbCrLf & _
                                                  "WHERE ISNULL(A.ReffNoTransaksi, '')='' AND ISNULL(A.ReffNoUrut, 0)<=0 AND (A.Debet - A.Kredit) - ISNULL(B.Pelunasan, 0) <> 0.0" & vbCrLf & _
                                                  "GROUP BY A.IDAlamat" & vbCrLf & _
                                                  "HAVING A.IDAlamat = " & IDCustomer
                                oDA.Fill(ds, "MPiutang")

                                If ds.Tables("MPiutang").Rows.Count >= 1 Then
                                    If NullToDbl(ds.Tables("MAlamat").Rows(0).Item("LimitPiutang")) - NullToDbl(ds.Tables("MPiutang").Rows(0).Item("Saldo")) - IIf(Sisa <= 0, Sisa * -1, 0) < 0 Then
                                        DxErrorProvider1.SetError(txtTotalBayar, "Limit Piutang Customer Tidak Mencukupi.", DXErrorProvider.ErrorType.Critical)
                                    End If
                                    If NullToDbl(ds.Tables("MAlamat").Rows(0).Item("LimitNotaPiutang")) - NullToDbl(ds.Tables("MPiutang").Rows(0).Item("JmlNota")) - IIf(Sisa <= 0, 1, 0) < 0 Then
                                        DxErrorProvider1.SetError(txtTotalBayar, "Limit Jml Nota Piutang Customer melebihi batas.", DXErrorProvider.ErrorType.Critical)
                                    End If
                                    If NullToDbl(ds.Tables("MAlamat").Rows(0).Item("LimitUmurPiutang")) - NullToDbl(ds.Tables("MPiutang").Rows(0).Item("TglTerlamaPiutang")) < 0 Then
                                        DxErrorProvider1.SetError(txtTotalBayar, "Customer ini memiliki Piutang yang sudah Jatuh Tempo dan Melebihi Setting Limit Umur Piutang Terlama.", DXErrorProvider.ErrorType.Critical)
                                    End If
                                End If
                            Else
                                DxErrorProvider1.SetError(txtTotalBayar, "Customer harus diisi.", DXErrorProvider.ErrorType.Warning)
                            End If
                        Catch ex As Exception
                            DxErrorProvider1.SetError(txtTotalBayar, "Error : " & ex.Message, DXErrorProvider.ErrorType.Critical)
                        End Try
                    End Using
                End Using
            End Using
        End Using

        Return Not DxErrorProvider1.HasErrors
    End Function

    Private Sub mnSimpan_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSimpan.ItemClick
        If IsValidasi() Then
            DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub mnBatal_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnBatal.ItemClick
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Sub New(ByVal IDCustomer As Long, ByVal Total As Double, ByVal iList As List(Of Model.Pembayaran))

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Total = Total
        Me.iList = iList
        Me.IDCustomer = IDCustomer
    End Sub

    Private Subtotal, ChargeRp, TotalBayar, Sisa As Double

    Private Sub HitungJumlah()
        Try
            Subtotal = Me.Total
            ChargeRp = iList.Sum(Function(m) m.ChargeRp)
            TotalBayar = iList.Sum(Function(m) m.Total)
            Sisa = TotalBayar - (Subtotal + ChargeRp)
            txtSubtotal.EditValue = Subtotal
            txtCharge.EditValue = ChargeRp
            txtTotalBayar.EditValue = TotalBayar
            txtKembali.EditValue = Sisa
        Catch ex As Exception

        End Try
    End Sub

    Private Sub frmEntriJualDBayar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InitLoadLookUpPO()
        RefreshData()
    End Sub

    Private Sub GridView1_ValidateRow(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs) Handles GridView1.ValidateRow
        'e.
    End Sub

    Private Sub GridView1_ValidatingEditor(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs) Handles GridView1.ValidatingEditor
        
    End Sub

    Private Sub mnSaveLayout_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayout.ItemClick
        Using frm As New frmOtorisasi
            Try
                If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    GridView1.SaveLayoutToXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & GridView1.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub gv_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataSourceChanged
        With sender
            If System.IO.File.Exists(Utils.SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml") Then
                .RestoreLayoutFromXml(Utils.SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml")
            End If
        End With
    End Sub

    Private Sub GridView1_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
        Select Case GridView1.FocusedColumn.FieldName.ToLower
            Case "Nominal".ToLower, "IDJenisPembayaran".ToLower
                'GridView1.Columns(GridView1.FocusedColumn.FieldName).OptionsColumn.AllowEdit = True
                'GridView1.Columns(GridView1.FocusedColumn.FieldName).OptionsColumn.AllowFocus = True
                GridView1.OptionsBehavior.Editable = True
            Case "AtasNama".ToLower, "NoRekening".ToLower
                'GridView1.Columns(GridView1.FocusedColumn.FieldName).OptionsColumn.AllowEdit = NullToBool(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "IsBank"))
                'GridView1.Columns(GridView1.FocusedColumn.FieldName).OptionsColumn.AllowFocus = NullToBool(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "IsBank"))
                GridView1.OptionsBehavior.Editable = NullToBool(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "IsBank"))
            Case Else
                GridView1.OptionsBehavior.Editable = False
        End Select
    End Sub

    Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
        Select Case e.FocusedColumn.FieldName.ToLower
            Case "Nominal".ToLower, "IDJenisPembayaran".ToLower
                'GridView1.Columns(GridView1.FocusedColumn.FieldName).OptionsColumn.AllowEdit = True
                'GridView1.Columns(GridView1.FocusedColumn.FieldName).OptionsColumn.AllowFocus = True
                GridView1.OptionsBehavior.Editable = True
            Case "AtasNama".ToLower, "NoRekening".ToLower
                'GridView1.Columns(GridView1.FocusedColumn.FieldName).OptionsColumn.AllowEdit = NullToBool(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "IsBank"))
                'GridView1.Columns(GridView1.FocusedColumn.FieldName).OptionsColumn.AllowFocus = NullToBool(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "IsBank"))
                GridView1.OptionsBehavior.Editable = NullToBool(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "IsBank"))
            Case Else
                GridView1.OptionsBehavior.Editable = False
        End Select
    End Sub
End Class
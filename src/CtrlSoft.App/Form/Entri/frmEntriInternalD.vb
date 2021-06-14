Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.App.Public
Imports DevExpress.XtraEditors.Repository

Public Class frmEntriInternalD
    Public NoID As Long = -1
    Public IDHeader As Long = -1
    Private frmPemanggil As frmEntriInternal = Nothing
    Private pStatus As [Public].pStatusForm
    Private pForm As modMain.FormInternal

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Private Property NamaForm() As String
        Get
            Select Case pForm
                Case modMain.FormInternal.Pemakaian
                    Return "Pemakaian"
                Case modMain.FormInternal.Penyesuaian
                    Return "Penyesuaian"
                Case modMain.FormInternal.MutasiGudang
                    Return "MutasiGudang"
                Case Else
                    Return ""
            End Select
        End Get
        Set(ByVal value As String)

        End Set
    End Property

    Private Property TitleForm() As String
        Get
            Select Case pForm
                Case modMain.FormInternal.Pemakaian
                    Return "Entri Item Pemakaian Barang"
                Case modMain.FormInternal.Penyesuaian
                    Return "Entri Item Penyesuaian Barang"
                Case modMain.FormInternal.MutasiGudang
                    Return "Entri Item Mutasi Gudang"
                Case Else
                    Return ""
            End Select
        End Get
        Set(ByVal value As String)

        End Set
    End Property

    Public Sub New(ByVal pForm As modMain.FormInternal, ByVal formPemanggil As frmEntriInternal, ByVal NoID As Long, ByVal IDHeader As Long)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.NoID = NoID
        Me.IDHeader = IDHeader
        Me.frmPemanggil = formPemanggil
        Me.pForm = pForm
        Me.Name = "frmEntri" & pForm & "D"
        Me.Text = TitleForm

        AddHandler txtQty.LostFocus, AddressOf txtEdit_EditValueChanged
        AddHandler txtHPP.LostFocus, AddressOf txtEdit_EditValueChanged
    End Sub

    Private Sub frmEntriRole_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim curentcursor As Cursor = System.Windows.Forms.Cursor.Current
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            LoadData(NoID)
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

    Private Sub LoadData(ByVal NoID As Long)
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

                                com.CommandText = [Public].Dataset.SQLLookUpBarcode
                                oDA.Fill(ds, "MBarangD")
                                txtBarcode.Properties.DataSource = ds.Tables("MBarangD")
                                txtBarcode.Properties.DisplayMember = "Barcode"
                                txtBarcode.Properties.ValueMember = "NoID"

                                com.CommandText = "SELECT * FROM M" & NamaForm & "D WHERE NoID=" & NoID
                                oDA.Fill(ds, "M" & NamaForm & "D")

                                If ds.Tables("M" & NamaForm & "D").Rows.Count >= 1 Then
                                    Dim iRow As DataRow = ds.Tables("M" & NamaForm & "D").Rows(0)
                                    pStatus = pStatusForm.Edit
                                    Me.NoID = NullToLong(iRow.Item("NoID"))
                                    Me.IDHeader = NullToLong(iRow.Item("IDHeader"))
                                    txtBarcode.EditValue = NullToLong(iRow.Item("IDBarangD"))
                                    IDBarang = NullToLong(iRow.Item("IDBarang"))
                                    txtSatuan.EditValue = NullToLong(iRow.Item("IDSatuan"))
                                    txtKonversi.EditValue = NullToDbl(iRow.Item("Konversi"))
                                    txtQty.EditValue = NullToDbl(iRow.Item("Qty"))

                                    txtHPP.EditValue = NullToDbl(iRow.Item("HPP"))
                                    txtKeterangan.Text = NullToStr(iRow.Item("Keterangan"))
                                Else
                                    pStatus = pStatusForm.Baru
                                    Me.NoID = -1
                                    IDBarang = -1
                                    txtBarcode.EditValue = -1
                                    txtSatuan.EditValue = -1
                                    txtKonversi.EditValue = 1
                                    txtQty.EditValue = 0
                                    txtKeterangan.Text = ""
                                End If
                                HitungJumlah()
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Private HargaPcs As Double = 0.0
    Private DPP As Double = 0.0
    Private PPN As Double = 0.0
    Private HargaBruto As Double = 0.0
    Private TotalBruto As Double = 0.0
    Private IDBarang As Long = -1

    Private Sub HitungJumlah()
        Try
            HargaBruto = Bulatkan(txtHPP.EditValue * txtKonversi.EditValue, 2)
            TotalBruto = Bulatkan(HargaBruto * txtQty.EditValue, 2)
            HargaPcs = HargaBruto / txtKonversi.EditValue
            txtJumlah.EditValue = TotalBruto
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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
                    gvSatuan.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & gvSatuan.Name & ".xml")
                    gvBarcode.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & gvBarcode.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub txtEdit_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        HitungJumlah()
    End Sub

    Private Sub gv_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSatuan.DataSourceChanged, gvBarcode.DataSourceChanged
        With sender
            If System.IO.File.Exists([Public].SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml") Then
                .RestoreLayoutFromXml([Public].SettingPerusahaan.PathLayouts & Me.Name & .Name & ".xml")
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

    Private Sub txtBarcode_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarcode.EditValueChanged
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

                                com.CommandText = "DECLARE @IDBarang AS BIGINT = -1, @IDBarangD AS BIGINT = " & NullToLong(txtBarcode.EditValue) & ";" & vbCrLf &
                                                  "SELECT @IDBarang = MBarangD.IDBarang FROM MBarangD WHERE MBarangD.NoID=@IDBarangD;" & vbCrLf &
                                                  "SELECT X.NoID, MSatuan.Kode Satuan, X.Konversi " & vbCrLf &
                                                  "FROM (" & vbCrLf &
                                                  "SELECT MBarangD.IDSatuan NoID, MBarangD.Konversi " & vbCrLf &
                                                  "FROM MBarangD" & vbCrLf &
                                                  "INNER JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang" & vbCrLf &
                                                  "WHERE MBarangD.IDBarang=@IDBarang AND MBarang.IsActive=1 AND MBarangD.IsActive=1" & vbCrLf &
                                                  "UNION ALL" & vbCrLf &
                                                  "SELECT MBarang.IDSatuanBeli, MBarang.IsiCtn" & vbCrLf &
                                                  "FROM MBarang" & vbCrLf &
                                                  "WHERE MBarang.NoID=@IDBarang AND MBarang.IsActive=1) AS X" & vbCrLf &
                                                  "INNER JOIN MSatuan ON MSatuan.NoID=X.NoID" & vbCrLf &
                                                  "GROUP BY X.NoID, X.Konversi, MSatuan.Kode"
                                oDA.Fill(ds, "MSatuan")
                                txtSatuan.Properties.DataSource = ds.Tables("MSatuan")
                                txtSatuan.Properties.DisplayMember = "Satuan"
                                txtSatuan.Properties.ValueMember = "NoID"

                                com.CommandText = "SELECT MBarangD.IDBarang, MBarang.Kode, MBarang.Nama, MBarangD.IDSatuan, MBarang.HargaBeliPcs AS HargaBeli FROM MBarang INNER JOIN MBarangD ON MBarang.NoID=MBarangD.IDBarang WHERE MBarangD.NoID=" & NullToLong(txtBarcode.EditValue)
                                oDA.Fill(ds, "MBarang")
                                If ds.Tables("MBarang").Rows.Count >= 1 Then
                                    Dim iRow As DataRow = ds.Tables("MBarang").Rows(0)
                                    IDBarang = NullToLong(iRow.Item("IDBarang"))
                                    txtKodeBarang.EditValue = NullToStr(iRow.Item("Kode"))
                                    txtNamaBarang.EditValue = NullToStr(iRow.Item("Nama"))
                                    txtSatuan.EditValue = NullToLong(iRow.Item("IDSatuan"))

                                    txtHPP.EditValue = NullToDbl(iRow.Item("HargaBeli"))
                                Else
                                    IDBarang = -1
                                    txtKodeBarang.EditValue = ""
                                    txtNamaBarang.EditValue = ""
                                    txtSatuan.EditValue = -1

                                    txtHPP.EditValue = 0.0
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

    Private Sub txtSatuan_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSatuan.EditValueChanged
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

                                com.CommandText = "DECLARE @IDBarang AS BIGINT = -1, @IDBarangD AS BIGINT = " & NullToLong(txtBarcode.EditValue) & ";" & vbCrLf &
                                                  "SELECT @IDBarang = MBarangD.IDBarang FROM MBarangD WHERE MBarangD.NoID=@IDBarangD;" & vbCrLf &
                                                  "SELECT X.NoID, MSatuan.Kode Satuan, X.Konversi " & vbCrLf &
                                                  "FROM (" & vbCrLf &
                                                  "SELECT MBarangD.IDSatuan NoID, MBarangD.Konversi " & vbCrLf &
                                                  "FROM MBarangD" & vbCrLf &
                                                  "INNER JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang" & vbCrLf &
                                                  "WHERE MBarangD.IDBarang=@IDBarang AND MBarang.IsActive=1 AND MBarangD.IsActive=1" & vbCrLf &
                                                  "UNION ALL" & vbCrLf &
                                                  "SELECT MBarang.IDSatuanBeli, MBarang.IsiCtn" & vbCrLf &
                                                  "FROM MBarang" & vbCrLf &
                                                  "WHERE MBarang.NoID=@IDBarang AND MBarang.IsActive=1) AS X" & vbCrLf &
                                                  "INNER JOIN MSatuan ON MSatuan.NoID=X.NoID" & vbCrLf &
                                                  "GROUP BY X.NoID, X.Konversi, MSatuan.Kode" & vbCrLf &
                                                  "HAVING X.NoID=" & NullToLong(txtSatuan.EditValue)
                                oDA.Fill(ds, "MSatuan")
                                If ds.Tables("MSatuan").Rows.Count >= 1 Then
                                    Dim iRow As DataRow = ds.Tables("MSatuan").Rows(0)
                                    txtKonversi.EditValue = NullToDbl(iRow.Item("Konversi"))
                                Else
                                    txtKonversi.EditValue = 1
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

    Private Sub mnSimpan_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSimpan.ItemClick
        DxErrorProvider1.ClearErrors()
        If txtBarcode.Text = "" Then
            DxErrorProvider1.SetError(txtBarcode, "Barcode harus diisi!")
        End If
        If txtSatuan.Text = "" Then
            DxErrorProvider1.SetError(txtSatuan, "Satuan harus diisi!")
        End If
        If txtKonversi.EditValue <= 0 Then
            DxErrorProvider1.SetError(txtKonversi, "Konversi salah!")
        End If
        If txtJumlah.EditValue < 0.0 Then
            DxErrorProvider1.SetError(txtHPP, "Nilai HPP salah!")
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

                                    If pForm = modMain.FormInternal.Penyesuaian Then
                                    Else
                                        com.CommandText = "EXEC spCekSaldoStok " & NullToLong(IDBarang) & ", " & NullToLong(frmPemanggil.txtGudangAsal.EditValue) & ", '" & frmPemanggil.txtTanggal.DateTime.ToString("yyyy-MM-dd HH:mm:ss") & "'"
                                        If NullToDbl(com.ExecuteScalar()) < (txtQty.EditValue * txtKonversi.EditValue) Then
                                            DxErrorProvider1.SetError(txtQty, "Saldo Stok Tidak Cukup!")
                                        End If
                                    End If

                                    If Not DxErrorProvider1.HasErrors Then
                                        If pStatus = pStatusForm.Baru Then
                                            com.CommandText = "SELECT MAX(NoID) FROM M" & NamaForm & "D"
                                            NoID = NullToLong(com.ExecuteScalar()) + 1

                                            com.CommandText = "INSERT INTO [dbo].[M" & NamaForm & "D] ([NoID],[IDHeader],[IDBarangD],[IDBarang],[IDSatuan],[Konversi],[Qty]" & vbCrLf &
                                                              ",[HPP],[Jumlah],[Keterangan]) VALUES (" & vbCrLf &
                                                              "@NoID,@IDHeader,@IDBarangD,@IDBarang,@IDSatuan,@Konversi,@Qty" & vbCrLf &
                                                              ",@HPP,@Jumlah,@Keterangan)"
                                        Else
                                            com.CommandText = "UPDATE [dbo].[M" & NamaForm & "D] SET [IDBarangD]=@IDBarangD,[IDBarang]=@IDBarang,[IDSatuan]=@IDSatuan,[Konversi]=@Konversi,[Qty]=@Qty" & vbCrLf &
                                                              ",[HPP]=@HPP,[Jumlah]=@Jumlah,[Keterangan]=@Keterangan WHERE NoID=@NoID"
                                        End If
                                        com.Parameters.Clear()
                                        com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NoID
                                        com.Parameters.Add(New SqlParameter("@IDHeader", SqlDbType.BigInt)).Value = IDHeader
                                        com.Parameters.Add(New SqlParameter("@IDBarangD", SqlDbType.BigInt)).Value = NullToLong(txtBarcode.EditValue)
                                        com.Parameters.Add(New SqlParameter("@IDBarang", SqlDbType.BigInt)).Value = IDBarang
                                        com.Parameters.Add(New SqlParameter("@IDSatuan", SqlDbType.Int)).Value = NullTolInt(txtSatuan.EditValue)
                                        com.Parameters.Add(New SqlParameter("@Konversi", SqlDbType.Int)).Value = NullToDbl(txtKonversi.EditValue)
                                        com.Parameters.Add(New SqlParameter("@Qty", SqlDbType.Float)).Value = Bulatkan(NullToDbl(txtQty.EditValue), 3)
                                        com.Parameters.Add(New SqlParameter("@HPP", SqlDbType.Float)).Value = Bulatkan(NullToDbl(txtHPP.EditValue), 2)
                                        com.Parameters.Add(New SqlParameter("@Jumlah", SqlDbType.Float)).Value = Bulatkan(NullToDbl(txtJumlah.EditValue), 2)
                                        com.Parameters.Add(New SqlParameter("@Keterangan", SqlDbType.VarChar)).Value = NullToStr(txtKeterangan.Text)
                                        com.ExecuteNonQuery()

                                        com.Parameters.Clear()

                                        com.CommandText = "UPDATE M" & NamaForm & " SET Total=ISNULL(M" & NamaForm & "D.Jumlah, 0) " & vbCrLf &
                                                          "FROM M" & NamaForm & " " & vbCrLf &
                                                          "INNER JOIN (SELECT IDHeader, SUM(Jumlah) AS Jumlah FROM M" & NamaForm & "D GROUP BY IDHeader) AS M" & NamaForm & "D ON M" & NamaForm & "D.IDHeader=M" & NamaForm & ".NoID " & vbCrLf &
                                                          "WHERE M" & NamaForm & ".NoID=" & IDHeader
                                        com.ExecuteNonQuery()

                                        com.Transaction.Commit()

                                        DialogResult = System.Windows.Forms.DialogResult.OK
                                        Me.Close()
                                    End If
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

    Private Sub mnTutup_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnTutup.ItemClick
        DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class
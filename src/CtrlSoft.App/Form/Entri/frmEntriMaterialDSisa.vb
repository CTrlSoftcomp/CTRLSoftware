Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.App.Public
Imports DevExpress.XtraEditors.Repository
Imports Dapper
Imports DevExpress.XtraTab.ViewInfo

Public Class frmEntriMaterialDSisa
    Inherits frmEntriMaterialD
    Private pStatus As pStatusForm
    Public dataSisa As New CtrlSoft.Dto.Model.MMaterialDSisa

    Public Sub New(ByVal data As CtrlSoft.Dto.Model.MMaterialDSisa,
                   ByVal header As CtrlSoft.Dto.Model.MMaterial)

        ' This call is required by the Windows Form Designer.
        MyBase.New(Nothing, header)
        Me.Name = "FrmEntriMaterialDSisa"
        Me.Text = "Entri Sisa Material / Formula"
        Me.LayoutControlGroup2.Text = "Informasi Barang Sisa Hasil Produksi"
        ' Add any initialization after the InitializeComponent() call.
        Me.dataSisa = data
        Me.header = header
    End Sub

    Public Overrides Sub frmEntriRole_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim curentcursor As Cursor = System.Windows.Forms.Cursor.Current
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            LoadData(dataSisa.NoID)
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

    Public Overrides Sub LoadData(ByVal NoID As Guid)
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

                                com.CommandText = "SELECT NoID, Kode FROM MSatuan WHERE IsActive=1"
                                oDA.Fill(ds, "MSatuan")
                                txtSatuan.Properties.DataSource = ds.Tables("MSatuan")
                                txtSatuan.Properties.ValueMember = "NoID"
                                txtSatuan.Properties.DisplayMember = "Kode"
                                txtSatuan.EditValue = -1

                                com.CommandText = "SELECT MBarangD.NoID, " & vbCrLf &
                                                  "MBarangD.Barcode, " & vbCrLf &
                                                  "MBarang.Kode AS KodeBarang, " & vbCrLf &
                                                  "MBarang.Nama NamaBarang, " & vbCrLf &
                                                  "MSatuan.Kode AS Satuan, " & vbCrLf &
                                                  "MBarangD.Konversi " & vbCrLf &
                                                  "FROM MBarangD(NOLOCK) " & vbCrLf &
                                                  "INNER JOIN MBarang(NOLOCK) ON MBarang.NoID = MBarangD.IDBarang " & vbCrLf &
                                                  "LEFT JOIN MSatuan(NOLOCK) ON MSatuan.NoID = MBarangD.IDSatuan " & vbCrLf &
                                                  "WHERE MBarangD.IsActive = 1 " & vbCrLf &
                                                  "AND MBarang.IsActive = 1"
                                oDA.Fill(ds, "MBarangD")
                                txtBarcode.Properties.DataSource = ds.Tables("MBarangD")
                                txtBarcode.Properties.ValueMember = "NoID"
                                txtBarcode.Properties.DisplayMember = "Barcode"
                                txtBarcode.EditValue = -1

                                If (dataSisa IsNot Nothing AndAlso dataSisa.IDBarangD >= 1) Then
                                    pStatus = pStatusForm.Edit
                                    txtBarcode.EditValue = dataSisa.IDBarangD
                                    txtQty.EditValue = dataSisa.Qty
                                    txtKonversi.EditValue = dataSisa.Konversi
                                    txtHargaPokok.EditValue = dataSisa.HargaPokok
                                    txtJumlah.EditValue = dataSisa.Jumlah
                                Else
                                    pStatus = pStatusForm.Baru
                                    dataSisa.NoID = System.Guid.NewGuid()
                                    txtBarcode.EditValue = -1
                                    txtQty.EditValue = 0
                                    txtKonversi.EditValue = 0.0
                                    txtHargaPokok.EditValue = 0.0
                                    txtJumlah.EditValue = 0.0
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

    Public Overrides Sub SimpanData()
        DxErrorProvider1.ClearErrors()
        HitungMaterial()

        If txtBarcode.EditValue <= 0 OrElse txtKodeBarang.Text = "" OrElse txtSatuan.Text = "" OrElse txtKonversi.EditValue <= 0 Then
            DxErrorProvider1.SetError(txtBarcode, "Barang harus diisi!")
        End If
        If txtQty.EditValue <= 0 Then
            DxErrorProvider1.SetError(txtQty, "Qty Barang harus diisi!")
        End If
        Dim exists = header.MMaterialDSisas.Where(Function(m) m.NoID <> dataSisa.NoID AndAlso m.IDBarang = NullToLong(txtBarcode.EditValue)).SingleOrDefault
        If (exists IsNot Nothing) Then
            DxErrorProvider1.SetError(txtBarcode, "Barang yang dipilih sudah ada!")
        End If

        If Not DxErrorProvider1.HasErrors Then
            Using dlg As New WaitDialogForm("Sedang menyimpan data ...", NamaAplikasi)
                Try
                    dlg.Show()
                    dlg.Focus()

                    With dataSisa
                        .KodeBarang = txtKodeBarang.Text
                        .NamaBarang = txtNamaBarang.Text
                        .Barcode = txtBarcode.Text
                        .Satuan = txtSatuan.Text
                        .IDBarangD = NullToLong(txtBarcode.EditValue)
                        .IDBarang = NullToLong(txtBarcode.Tag)
                        .IDSatuan = NullToLong(txtSatuan.EditValue)
                        .Qty = txtQty.EditValue
                        .Konversi = txtKonversi.EditValue
                        .HargaPokok = txtHargaPokok.EditValue
                        .Jumlah = txtJumlah.EditValue
                        .IDMaterial = dataSisa.IDMaterial
                    End With
                    DialogResult = System.Windows.Forms.DialogResult.OK
                    Me.Close()
                Catch ex As Exception
                    XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End If
    End Sub

    Public Overrides Sub mnRefreshClick()
        LoadData(dataSisa.NoID)
    End Sub
End Class

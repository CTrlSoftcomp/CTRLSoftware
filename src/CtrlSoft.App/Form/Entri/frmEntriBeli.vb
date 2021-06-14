﻿Imports CtrlSoft.Repository.Utils
Imports CtrlSoft.App.Public
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors
Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.Utils

Public Class frmEntriBeli
    Private NoID As Long = -1
    Private pStatus As pStatusForm
    Private IDTypePajak As Integer = -1

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Public Sub New(ByVal NoID As Long)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.NoID = NoID
    End Sub

#Region "Init Data"
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

                            com.CommandText = "DECLARE @IDSupplier AS INT =" & NullToLong(txtSupplier.EditValue) & ", @Tanggal AS DATETIME='" & NullToDate(txtTanggal.EditValue).ToString("yyyy-MM-dd HH:mm:ss") & "', @IDBeli AS BIGINT=" & NoID & ";" & vbCrLf & _
                                              "SELECT MPO.NoID, MPO.Kode, MPO.Tanggal" & vbCrLf & _
                                              "FROM MPO" & vbCrLf & _
                                              "INNER JOIN MPOD ON MPO.NoID=MPOD.IDHeader" & vbCrLf & _
                                              "LEFT JOIN (SELECT MBeli.IDPO, COUNT(MBeli.IDPO) AS Jml " & vbCrLf & _
                                              "FROM MBeli WHERE MBeli.NoID<>@IDBeli GROUP BY MBeli.IDPO) MBeli ON MBeli.IDPO=MPO.NoID" & vbCrLf & _
                                              "WHERE MPO.IsPosted=1 AND ISNULL(MPO.IsExpired, 0)=0 AND CONVERT(DATE, MPO.Expired)>=CONVERT(DATE, @Tanggal) AND ISNULL(MBeli.Jml, 0)=0 AND MPO.IDSupplier=@IDSupplier AND CONVERT(DATE, MPO.Tanggal)<=CONVERT(DATE, @Tanggal)" & vbCrLf & _
                                              "GROUP BY MPO.NoID, MPO.Kode, MPO.Tanggal"
                            oDA.Fill(ds, "MPO")
                            txtPO.Properties.DataSource = ds.Tables("MPO")
                            txtPO.Properties.ValueMember = "NoID"
                            txtPO.Properties.DisplayMember = "Kode"
                        Catch ex As Exception
                            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End Using
                End Using
            End Using
        End Using
    End Sub
    Private Sub InitLoadDataPO()
        Dim IDPOD As Long = -1
        If IIf(pStatus = pStatusForm.Baru, SimpanData(), True) = True AndAlso NullToStr(txtPO.Text) <> "" Then
            Using dlg As New WaitDialogForm("Sedang meload data!", NamaAplikasi)
                Using cn As New SqlConnection(StrKonSQL)
                    Using com As New SqlCommand
                        Using oDA As New SqlDataAdapter
                            Using ds As New DataSet
                                Try
                                    dlg.Show()
                                    dlg.Focus()

                                    cn.Open()
                                    com.Connection = cn
                                    com.CommandTimeout = cn.ConnectionTimeout
                                    oDA.SelectCommand = com
                                    com.Transaction = cn.BeginTransaction

                                    com.CommandText = "SELECT MAX(NoID) FROM MBeliD"
                                    IDPOD = NullToLong(com.ExecuteScalar()) + 1

                                    com.CommandText = "DECLARE @IDPO AS BIGINT=" & NullToLong(txtPO.EditValue) & ", @IDBeli AS BIGINT=" & NoID & ", @IDBeliD AS BIGINT=" & IDPOD & ";" & vbCrLf & _
                                                      "INSERT INTO [dbo].[MBeliD]" & vbCrLf & _
                                                      "([NoID],[IDHeader],[IDPOD],[IDBarangD],[IDBarang],[IDSatuan],[Konversi],[Qty],[Harga]" & vbCrLf & _
                                                      ",[DiscProsen1],[DiscProsen2],[DiscProsen3],[DiscProsen4],[DiscProsen5],[DiscRp]" & vbCrLf & _
                                                      ",[DiscNotaProsen],[DiscNotaRp],[JumlahBruto],[DPP],[PPN],[Jumlah])" & vbCrLf & _
                                                      "SELECT @IDBeliD + ROW_NUMBER() OVER(ORDER BY NoID) [NoID],@IDBeli [IDHeader],NoID [IDPOD],[IDBarangD],[IDBarang],[IDSatuan],[Konversi],[Qty],[Harga]" & vbCrLf & _
                                                      ",[DiscProsen1],[DiscProsen2],[DiscProsen3],[DiscProsen4],[DiscProsen5],[DiscRp]" & vbCrLf & _
                                                      ",[DiscNotaProsen],[DiscNotaRp],[JumlahBruto],[DPP],[PPN],[Jumlah]" & vbCrLf & _
                                                      "FROM MPOD" & vbCrLf & _
                                                      "WHERE MPOD.IDHeader=@IDPO"
                                    If NullToLong(com.ExecuteNonQuery()) >= 1 Then
                                        com.CommandText = "SELECT MPO.IDTypePajak FROM MPO WHERE NoID=" & NullToLong(txtPO.EditValue)
                                        txtTypePajak.EditValue = NullToLong(com.ExecuteScalar())
                                        IDTypePajak = txtTypePajak.EditValue

                                        com.CommandText = "UPDATE MBeli SET IDTypePajak=" & NullToLong(txtTypePajak.EditValue) & " WHERE NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "UPDATE MBeli SET Subtotal=ISNULL(MBeliD.JumlahBruto, 0), TotalBruto=ISNULL(MBeliD.JumlahBruto, 0), " & vbCrLf & _
                                                              "PPN=ROUND((CASE WHEN MBeli.IDTypePajak=0 THEN 0 ELSE 0.1 END)*ISNULL(MBeliD.JumlahBruto, 0), 0), " & vbCrLf & _
                                                              "DPP=ROUND(CASE WHEN MBeli.IDTypePajak=0 THEN 0 WHEN MBeli.IDTypePajak=1 THEN ISNULL(MBeliD.JumlahBruto, 0)/1.0 ELSE ISNULL(MBeliD.JumlahBruto, 0)/1.1 END, 0) " & vbCrLf & _
                                                              "FROM MBeli " & vbCrLf & _
                                                              "INNER JOIN (SELECT IDHeader, SUM(JumlahBruto) AS JumlahBruto FROM MBeliD GROUP BY IDHeader) AS MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                                              "WHERE MBeli.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                                          "PPN=ROUND((CASE WHEN MBeli.IDTypePajak=0 THEN 0 ELSE 0.1 END)*ISNULL(MBeliD.JumlahBruto, 0), 0), " & vbCrLf & _
                                                          "DPP=ROUND(CASE WHEN MBeli.IDTypePajak=0 THEN 0 WHEN MBeli.IDTypePajak=1 THEN ISNULL(MBeliD.JumlahBruto, 0)/1.0 ELSE ISNULL(MBeliD.JumlahBruto, 0)/1.1 END, 0) " & vbCrLf & _
                                                          "FROM MBeli " & vbCrLf & _
                                                          "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                                          "WHERE MBeliD.IDHeader=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                                          "PPN=ISNULL(MBeli.PPN, 0)-(ISNULL(Detil.PPN, 0)-ISNULL(MBeliD.PPN, 0)), " & vbCrLf & _
                                                          "DPP=ISNULL(MBeli.DPP, 0)-(ISNULL(Detil.DPP, 0)-ISNULL(MBeliD.DPP, 0)) " & vbCrLf & _
                                                          "FROM MBeli " & vbCrLf & _
                                                          "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID" & vbCrLf & _
                                                          "INNER JOIN (SELECT IDHeader, SUM(DPP) AS DPP, SUM(PPN) AS PPN, MAX(NoID) AS NoID FROM MBeliD GROUP BY IDHeader) AS Detil ON Detil.IDHeader=MBeli.NoID AND Detil.NoID=MBeliD.NoID" & vbCrLf & _
                                                          "WHERE MBeliD.IDHeader=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                                          "Jumlah=CASE WHEN MBeli.IDTypePajak=0 THEN MBeliD.JumlahBruto ELSE MBeliD.DPP+MBeliD.PPN END " & vbCrLf & _
                                                          "FROM MBeli " & vbCrLf & _
                                                          "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                                          "WHERE MBeliD.IDHeader=" & NoID
                                        com.ExecuteNonQuery()

                                        com.CommandText = "UPDATE MBeli SET Subtotal=ISNULL(MBeliD.JumlahBruto, 0), TotalBruto=ISNULL(MBeliD.JumlahBruto, 0), Total=ISNULL(MBeliD.Jumlah, 0)" & vbCrLf & _
                                                          "FROM MBeli " & vbCrLf & _
                                                          "INNER JOIN (SELECT IDHeader, SUM(JumlahBruto) AS JumlahBruto, SUM(Jumlah) AS Jumlah FROM MBeliD GROUP BY IDHeader) AS MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                                          "WHERE MBeli.NoID=" & NoID
                                        com.ExecuteNonQuery()

                                        If com.Transaction IsNot Nothing Then
                                            com.Transaction.Commit()
                                        End If
                                        com.Transaction = Nothing

                                        RefreshDetil()
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
    Private Sub InitLoadLookUp()
        Using cn As New SqlConnection(StrKonSQL)
            Using com As New SqlCommand
                Using oDA As New SqlDataAdapter
                    Using ds As New DataSet
                        Try
                            cn.Open()
                            com.Connection = cn
                            com.CommandTimeout = cn.ConnectionTimeout
                            oDA.SelectCommand = com

                            com.CommandText = "SELECT NoID, Kode, Nama, Alamat FROM MAlamat WHERE IsActive=1 AND IsSupplier=1"
                            oDA.Fill(ds, "MSupplier")
                            txtSupplier.Properties.DataSource = ds.Tables("MSupplier")
                            txtSupplier.Properties.ValueMember = "NoID"
                            txtSupplier.Properties.DisplayMember = "Kode"

                            com.CommandText = "SELECT NoID, Kode + '-' + Nama AS Gudang FROM MGudang WHERE IsActive=1"
                            oDA.Fill(ds, "MGudang")
                            txtGudang.Properties.DataSource = ds.Tables("MGudang")
                            txtGudang.Properties.ValueMember = "NoID"
                            txtGudang.Properties.DisplayMember = "Gudang"

                            com.CommandText = "SELECT NoID, TypePajak FROM MTypePajak"
                            oDA.Fill(ds, "MTypePajak")
                            txtTypePajak.Properties.DataSource = ds.Tables("MTypePajak")
                            txtTypePajak.Properties.ValueMember = "NoID"
                            txtTypePajak.Properties.DisplayMember = "TypePajak"

                        Catch ex As Exception
                            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End Using
                End Using
            End Using
        End Using
    End Sub
    Private Sub LoadData()
        Using cn As New SqlConnection(StrKonSQL)
            Using com As New SqlCommand
                Using oDA As New SqlDataAdapter
                    Using ds As New DataSet
                        Try
                            cn.Open()
                            com.Connection = cn
                            com.CommandTimeout = cn.ConnectionTimeout
                            oDA.SelectCommand = com

                            com.CommandText = "SELECT TOP 1 * FROM MBeli WHERE NoID=" & NoID
                            oDA.Fill(ds, "MBeli")

                            If ds.Tables("MBeli").Rows.Count >= 1 Then
                                Dim iRow As DataRow = ds.Tables("MBeli").Rows(0)
                                If NullToBool(iRow.Item("IsPosted")) Then
                                    pStatus = pStatusForm.Posted
                                Else
                                    pStatus = pStatusForm.Edit
                                End If
                                txtGudang.EditValue = NullToLong(iRow.Item("IDGudang"))
                                txtTanggal.EditValue = NullToDate(iRow.Item("Tanggal"))
                                txtKode.Text = NullToStr(iRow.Item("Kode"))
                                txtJatuhTempo.EditValue = NullToDate(iRow.Item("JatuhTempo"))
                                txtSupplier.EditValue = -1
                                txtSupplier.EditValue = NullToLong(iRow.Item("IDSupplier"))
                                InitLoadLookUpPO()
                                txtPO.EditValue = NullToLong(iRow.Item("IDPO"))
                                txtTypePajak.EditValue = NullToLong(iRow.Item("IDTypePajak"))
                                IDTypePajak = txtTypePajak.EditValue
                                txtNoReff.Text = NullToStr(iRow.Item("NoReff"))
                                txtCatatan.Text = NullToStr(iRow.Item("Catatan"))
                                txtDiscProsen.EditValue = NullToDbl(iRow.Item("DiscNotaProsen"))
                                txtDiscRp.EditValue = NullToDbl(iRow.Item("DiscNotaRp"))
                            Else
                                txtGudang.EditValue = -1
                                txtTanggal.EditValue = Now
                                txtSupplier.EditValue = -1
                                InitLoadLookUpPO()
                                txtPO.EditValue = -1
                                txtTypePajak.EditValue = 1
                                IDTypePajak = txtTypePajak.EditValue
                                txtDiscProsen.EditValue = 0.0
                                txtDiscRp.EditValue = 0.0
                                pStatus = pStatusForm.Baru
                            End If
                        Catch ex As Exception
                            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End Using
                End Using
            End Using
        End Using
    End Sub
    Private Sub RefreshDetil(Optional ByVal IDDetil As Long = -1)
        Using dlg As New DevExpress.Utils.WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
            Using cn As New SqlConnection(StrKonSQL)
                Using com As New SqlCommand
                    Using oDA As New SqlDataAdapter
                        Using ds As New DataSet
                            Try
                                dlg.Show()
                                dlg.Focus()

                                cn.Open()
                                com.Connection = cn
                                com.CommandTimeout = cn.ConnectionTimeout
                                oDA.SelectCommand = com

                                com.CommandText = "SELECT MBeliD.*, MPOD.Qty*MPOD.Konversi AS QtyOrder, MSatuan.Kode AS Satuan, MBarangD.Barcode, MBarang.Kode KodeBarang, MBarang.Nama NamaBarang" & vbCrLf &
                                                  "FROM MBeliD" & vbCrLf &
                                                  "LEFT JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang" & vbCrLf &
                                                  "LEFT JOIN MBarangD ON MBarangD.NoID=MBeliD.IDBarangD" & vbCrLf &
                                                  "LEFT JOIN MSatuan ON MSatuan.NoID=MBeliD.IDSatuan" & vbCrLf &
                                                  "LEFT JOIN MPOD ON MPOD.NoID=MBeliD.IDPOD" & vbCrLf &
                                                  "WHERE MBeliD.IDHeader=" & NoID
                                oDA.Fill(ds, "MBeliD")
                                GridControl1.DataSource = ds.Tables("MBeliD")
                                GridView1.RefreshData()

                                GridView1.ClearSelection()
                                GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), IDDetil.ToString("n0"))
                                GridView1.SelectRow(GridView1.FocusedRowHandle)

                                If ds.Tables("MBeliD").Rows.Count >= 1 Then
                                    txtPO.Enabled = False
                                    txtSupplier.Enabled = False
                                    txtTanggal.Enabled = False
                                Else
                                    txtPO.Enabled = True
                                    txtSupplier.Enabled = True
                                    txtTanggal.Enabled = True
                                End If

                                If pStatus = pStatusForm.Posted Then
                                    mnBaru.Enabled = False
                                    mnEdit.Enabled = False
                                    mnHapus.Enabled = False
                                    mnSimpan.Enabled = False

                                    txtDiscProsen.Properties.ReadOnly = True
                                    txtDiscRp.Properties.ReadOnly = True
                                Else
                                    mnBaru.Enabled = True
                                    mnEdit.Enabled = True
                                    mnHapus.Enabled = True
                                    mnSimpan.Enabled = True

                                    txtDiscProsen.Properties.ReadOnly = False
                                    txtDiscRp.Properties.ReadOnly = False
                                End If

                                com.CommandText = "SELECT SUM(MBeliD.JumlahBruto) JumlahBruto, SUM(MBeliD.Jumlah) Jumlah, SUM(MBeliD.DPP) DPP, SUM(MBeliD.PPN) PPN" & vbCrLf &
                                                  "FROM MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDHeader" & vbCrLf &
                                                  "WHERE MBeli.NoID=" & NoID & vbCrLf &
                                                  "GROUP BY MBeli.NoID, MBeli.IDTypePajak"
                                oDA.Fill(ds, "MHitung")
                                If ds.Tables("MHitung").Rows.Count >= 1 Then
                                    txtSubtotal.EditValue = NullToDbl(ds.Tables("MHitung").Rows(0).Item("JumlahBruto"))
                                    txtDPP.EditValue = NullToDbl(ds.Tables("MHitung").Rows(0).Item("DPP"))
                                    txtPPN.EditValue = NullToDbl(ds.Tables("MHitung").Rows(0).Item("PPN"))
                                    txtTotal.EditValue = NullToDbl(ds.Tables("MHitung").Rows(0).Item("Jumlah"))
                                Else
                                    txtSubtotal.EditValue = 0
                                    txtDPP.EditValue = 0
                                    txtPPN.EditValue = 0
                                    txtTotal.EditValue = 0
                                End If
                                HitungJumlah()
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Finally
                                mnFastEntri.PerformClick()
                            End Try
                        End Using
                    End Using
                End Using
            End Using
        End Using
    End Sub
    Private Sub HitungJumlah()
        Using cn As New SqlConnection(StrKonSQL)
            Using com As New SqlCommand
                Try
                    If pStatus <> pStatusForm.Posted Then
                        txtBruto.EditValue = Bulatkan((txtSubtotal.EditValue * (1 - (txtDiscProsen.EditValue / 100))) - txtDiscRp.EditValue, 2)
                        If txtTypePajak.EditValue = [Public].TypePajak.NonPajak Then
                            txtDPP.EditValue = 0.0
                            txtPPN.EditValue = 0.0
                            txtTotal.EditValue = txtBruto.EditValue
                        ElseIf txtTypePajak.EditValue = [Public].TypePajak.Include Then
                            txtDPP.EditValue = Bulatkan(txtBruto.EditValue / 1.1, 0)
                            txtPPN.EditValue = Bulatkan(txtDPP.EditValue * 0.1, 0)
                            txtTotal.EditValue = Bulatkan(txtDPP.EditValue + txtPPN.EditValue, 0)
                        Else
                            txtDPP.EditValue = Bulatkan(txtBruto.EditValue, 0)
                            txtPPN.EditValue = Bulatkan(txtDPP.EditValue * 0.1, 0)
                            txtTotal.EditValue = Bulatkan(txtDPP.EditValue + txtPPN.EditValue, 0)
                        End If

                        cn.Open()
                        com.Connection = cn

                        com.CommandText = "SELECT IsPosted FROM MBeli WHERE NoID=" & NoID
                        If Not NullToBool(com.ExecuteScalar()) Then
                            com.Transaction = cn.BeginTransaction

                            com.CommandText = "UPDATE MBeli SET IDTypePajak=" & NullToLong(txtTypePajak.EditValue) & " WHERE NoID=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeli SET Subtotal=ISNULL(MBeliD.JumlahBruto, 0), TotalBruto=ISNULL(MBeliD.JumlahBruto, 0), " & vbCrLf & _
                                                  "PPN=ROUND((CASE WHEN MBeli.IDTypePajak=0 THEN 0 ELSE 0.1 END)*ISNULL(MBeliD.JumlahBruto, 0), 0), " & vbCrLf & _
                                                  "DPP=ROUND(CASE WHEN MBeli.IDTypePajak=0 THEN 0 WHEN MBeli.IDTypePajak=1 THEN ISNULL(MBeliD.JumlahBruto, 0)/1.0 ELSE ISNULL(MBeliD.JumlahBruto, 0)/1.1 END, 0) " & vbCrLf & _
                                                  "FROM MBeli " & vbCrLf & _
                                                  "INNER JOIN (SELECT IDHeader, SUM(JumlahBruto) AS JumlahBruto FROM MBeliD GROUP BY IDHeader) AS MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                                  "WHERE MBeli.NoID=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                              "PPN=ROUND((CASE WHEN MBeli.IDTypePajak=0 THEN 0 ELSE 0.1 END)*ISNULL(MBeliD.JumlahBruto, 0), 0), " & vbCrLf & _
                                              "DPP=ROUND(CASE WHEN MBeli.IDTypePajak=0 THEN 0 WHEN MBeli.IDTypePajak=1 THEN ISNULL(MBeliD.JumlahBruto, 0)/1.0 ELSE ISNULL(MBeliD.JumlahBruto, 0)/1.1 END, 0) " & vbCrLf & _
                                              "FROM MBeli " & vbCrLf & _
                                              "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                              "WHERE MBeliD.IDHeader=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                              "PPN=ISNULL(MBeli.PPN, 0)-(ISNULL(Detil.PPN, 0)-ISNULL(MBeliD.PPN, 0)), " & vbCrLf & _
                                              "DPP=ISNULL(MBeli.DPP, 0)-(ISNULL(Detil.DPP, 0)-ISNULL(MBeliD.DPP, 0)) " & vbCrLf & _
                                              "FROM MBeli " & vbCrLf & _
                                              "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID" & vbCrLf & _
                                              "INNER JOIN (SELECT IDHeader, SUM(DPP) AS DPP, SUM(PPN) AS PPN, MAX(NoID) AS NoID FROM MBeliD GROUP BY IDHeader) AS Detil ON Detil.IDHeader=MBeli.NoID AND Detil.NoID=MBeliD.NoID" & vbCrLf & _
                                              "WHERE MBeliD.IDHeader=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                              "Jumlah=CASE WHEN MBeli.IDTypePajak=0 THEN MBeliD.JumlahBruto ELSE MBeliD.DPP+MBeliD.PPN END " & vbCrLf & _
                                              "FROM MBeli " & vbCrLf & _
                                              "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                              "WHERE MBeliD.IDHeader=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeli SET Subtotal=ISNULL(MBeliD.JumlahBruto, 0), TotalBruto=ISNULL(" & FixKoma(txtBruto.EditValue) & ", 0), Total=ISNULL(" & FixKoma(txtTotal.EditValue) & ", 0)" & vbCrLf & _
                                              "FROM MBeli " & vbCrLf & _
                                              "INNER JOIN (SELECT IDHeader, SUM(JumlahBruto) AS JumlahBruto, SUM(Jumlah) AS Jumlah FROM MBeliD GROUP BY IDHeader) AS MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                              "WHERE MBeli.NoID=" & NoID
                            com.ExecuteNonQuery()

                            If com.Transaction IsNot Nothing Then
                                com.Transaction.Commit()
                            End If
                            com.Transaction = Nothing
                        End If
                    End If
                Catch ex As Exception
                    XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End Using
    End Sub
    Private Function SimpanData() As Boolean
        Dim Hasil As Boolean = False
        If IsValidasi() Then
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

                                If pStatus = pStatusForm.Baru Then
                                    NoID = -1
                                ElseIf pStatus = pStatusForm.Edit Then
                                    com.CommandText = "SELECT IsPosted FROM MBeli WHERE NoID=" & NoID
                                    If NullToBool(com.ExecuteScalar()) Then
                                        XtraMessageBox.Show("Nota telah diposting!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                        Exit Try
                                    End If
                                ElseIf pStatus = pStatusForm.Posted Then
                                    XtraMessageBox.Show("Nota telah diposting!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    Exit Try
                                End If

                                com.CommandText = "SELECT COUNT(Kode) FROM MBeli WHERE Kode=@Kode AND NoID<>@NoID"
                                com.Parameters.Clear()
                                com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NoID
                                com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = txtKode.Text
                                If NullToLong(com.ExecuteScalar()) >= 1 Then
                                    XtraMessageBox.Show("Nota telah sudah ada!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    Exit Try
                                End If

                                com.CommandText = "EXEC [dbo].[spSimpanMBeli] @NoID,@Kode,@NoReff,@IDSupplier,@Tanggal,@JatuhTempo,@Catatan,@IDTypePajak,@Subtotal," & vbCrLf & _
                                                  "@DiscNotaProsen,@DiscNotaRp,@TotalBruto,@DPP,@PPN,@Total,@IsPosted,@TglPosted,@IDUserPosted,@IDUserEntry,@IDUserEdit,@IDPO,@IDGudang"
                                com.Parameters.Clear()
                                com.Parameters.Add(New SqlParameter("@NoID", SqlDbType.BigInt)).Value = NoID
                                com.Parameters.Add(New SqlParameter("@Kode", SqlDbType.VarChar)).Value = txtKode.Text
                                com.Parameters.Add(New SqlParameter("@NoReff", SqlDbType.VarChar)).Value = txtNoReff.Text
                                com.Parameters.Add(New SqlParameter("@IDSupplier", SqlDbType.Int)).Value = NullTolInt(txtSupplier.EditValue)
                                com.Parameters.Add(New SqlParameter("@Tanggal", SqlDbType.DateTime)).Value = NullToDate(txtTanggal.EditValue)
                                com.Parameters.Add(New SqlParameter("@JatuhTempo", SqlDbType.Date)).Value = NullToDate(txtJatuhTempo.EditValue)
                                com.Parameters.Add(New SqlParameter("@Catatan", SqlDbType.VarChar)).Value = txtCatatan.Text
                                com.Parameters.Add(New SqlParameter("@IDTypePajak", SqlDbType.SmallInt)).Value = NullTolInt(txtTypePajak.EditValue)
                                com.Parameters.Add(New SqlParameter("@Subtotal", SqlDbType.Money)).Value = NullToDbl(txtSubtotal.EditValue)
                                com.Parameters.Add(New SqlParameter("@DiscNotaProsen", SqlDbType.Float)).Value = NullToDbl(txtDiscProsen.EditValue)
                                com.Parameters.Add(New SqlParameter("@DiscNotaRp", SqlDbType.Float)).Value = NullToDbl(txtDiscRp.EditValue)
                                com.Parameters.Add(New SqlParameter("@TotalBruto", SqlDbType.Money)).Value = NullToDbl(txtBruto.EditValue)
                                com.Parameters.Add(New SqlParameter("@DPP", SqlDbType.Money)).Value = NullToDbl(txtDPP.EditValue)
                                com.Parameters.Add(New SqlParameter("@PPN", SqlDbType.Money)).Value = NullToDbl(txtPPN.EditValue)
                                com.Parameters.Add(New SqlParameter("@Total", SqlDbType.Money)).Value = NullToDbl(txtTotal.EditValue)
                                com.Parameters.Add(New SqlParameter("@IsPosted", SqlDbType.Bit)).Value = False
                                com.Parameters.Add(New SqlParameter("@TglPosted", SqlDbType.DateTime)).Value = System.DBNull.Value
                                com.Parameters.Add(New SqlParameter("@IDUserPosted", SqlDbType.Int)).Value = System.DBNull.Value
                                com.Parameters.Add(New SqlParameter("@IDUserEntry", SqlDbType.Int)).Value = UserLogin.NoID
                                com.Parameters.Add(New SqlParameter("@IDUserEdit", SqlDbType.Int)).Value = IIf(pStatus = pStatusForm.Edit, UserLogin.NoID, -1)
                                com.Parameters.Add(New SqlParameter("@IDPO", SqlDbType.BigInt)).Value = NullToLong(txtPO.EditValue)
                                com.Parameters.Add(New SqlParameter("@IDGudang", SqlDbType.Int)).Value = NullToLong(txtGudang.EditValue)

                                NoID = NullToLong(com.ExecuteScalar())
                                com.Parameters.Clear()

                                com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                                  "DiscNotaProsen=ROUND(@DiscNotaProsen, 2)" & vbCrLf & _
                                                  ",DiscNotaRp=ROUND((@DiscNotaRp/(MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))) * 100.0, 2)" & vbCrLf & _
                                                  "FROM MBeli " & vbCrLf & _
                                                  "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID" & vbCrLf & _
                                                  "WHERE MBeliD.IDHeader=" & NoID
                                com.Parameters.Clear()
                                com.Parameters.Add(New SqlParameter("@DiscNotaProsen", SqlDbType.Float)).Value = NullToDbl(txtDiscProsen.EditValue)
                                com.Parameters.Add(New SqlParameter("@DiscNotaRp", SqlDbType.Float)).Value = NullToDbl(txtDiscRp.EditValue)
                                com.ExecuteNonQuery()

                                com.CommandText = "UPDATE MBeliD SET DiscNotaRp=MBeli.DiscNotaRp-(ISNULL(MBeliD2.DiscNotaRp, 0)-ISNULL(MBeliD.DiscNotaRp, 0))" & vbCrLf & _
                                                  "FROM MBeliD " & vbCrLf & _
                                                  "INNER JOIN (SELECT IDHeader, MAX(NoID) AS NoID, SUM(DiscNotaRp) AS DiscNotaRp FROM MBeliD GROUP BY IDHeader) AS MBeliD2 ON MBeliD2.NoID=MBeliD.NoID" & vbCrLf & _
                                                  "INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDHeader" & vbCrLf & _
                                                  "WHERE MBeliD.IDHeader=" & NoID
                                com.Parameters.Clear()
                                com.Parameters.Add(New SqlParameter("@DiscNotaProsen", SqlDbType.Float)).Value = NullToDbl(txtDiscProsen.EditValue)
                                com.Parameters.Add(New SqlParameter("@DiscNotaRp", SqlDbType.Float)).Value = NullToDbl(txtDiscRp.EditValue)
                                com.ExecuteNonQuery()

                                com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                                  "PPN=ROUND(CASE WHEN MBeli.IDTypePajak=0 THEN 0 WHEN MBeli.IDTypePajak=1 THEN ROUND((MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))-MBeliD.DiscNotaRp, 2)/1.1 ELSE ROUND((MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))-MBeliD.DiscNotaRp, 2)/1.0 END * 0.1, 0), " & vbCrLf & _
                                                  "DPP=ROUND(CASE WHEN MBeli.IDTypePajak=0 THEN 0 WHEN MBeli.IDTypePajak=1 THEN ROUND((MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))-MBeliD.DiscNotaRp, 2)/1.1 ELSE ROUND((MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))-MBeliD.DiscNotaRp, 2)/1.0 END, 0) " & vbCrLf & _
                                                  "FROM MBeli " & vbCrLf & _
                                                  "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                                  "WHERE MBeliD.IDHeader=" & NoID
                                com.Parameters.Clear()
                                com.Parameters.Add(New SqlParameter("@DiscNotaProsen", SqlDbType.Float)).Value = NullToDbl(txtDiscProsen.EditValue)
                                com.Parameters.Add(New SqlParameter("@DiscNotaRp", SqlDbType.Float)).Value = NullToDbl(txtDiscRp.EditValue)
                                com.ExecuteNonQuery()

                                com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                                  "PPN=ISNULL(MBeli.PPN, 0)-(ISNULL(Detil.PPN, 0)-ISNULL(MBeliD.PPN, 0)), " & vbCrLf & _
                                                  "DPP=ISNULL(MBeli.DPP, 0)-(ISNULL(Detil.DPP, 0)-ISNULL(MBeliD.DPP, 0)) " & vbCrLf & _
                                                  "FROM MBeli " & vbCrLf & _
                                                  "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID" & vbCrLf & _
                                                  "INNER JOIN (SELECT IDHeader, SUM(DPP) AS DPP, SUM(PPN) AS PPN, MAX(NoID) AS NoID FROM MBeliD GROUP BY IDHeader) AS Detil ON Detil.IDHeader=MBeli.NoID AND Detil.NoID=MBeliD.NoID" & vbCrLf & _
                                                  "WHERE MBeliD.IDHeader=" & NoID
                                com.ExecuteNonQuery()

                                com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                                  "Jumlah=CASE WHEN MBeli.IDTypePajak=0 THEN ROUND((MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))-MBeliD.DiscNotaRp, 2) ELSE MBeliD.DPP+MBeliD.PPN END " & vbCrLf & _
                                                  "FROM MBeli " & vbCrLf & _
                                                  "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                                  "WHERE MBeliD.IDHeader=" & NoID
                                com.Parameters.Clear()
                                com.Parameters.Add(New SqlParameter("@DiscNotaProsen", SqlDbType.Float)).Value = NullToDbl(txtDiscProsen.EditValue)
                                com.Parameters.Add(New SqlParameter("@DiscNotaRp", SqlDbType.Float)).Value = NullToDbl(txtDiscRp.EditValue)
                                com.ExecuteNonQuery()

                                If com.Transaction IsNot Nothing Then
                                    com.Transaction.Commit()
                                End If
                                If NoID >= 1 Then
                                    If pStatus = pStatusForm.Baru Then
                                        LoadData()
                                    End If
                                    Hasil = True
                                End If
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                    End Using
                End Using
            End Using
        End If

        Return Hasil
    End Function
    Private Function UpdateTypePajak() As Boolean
        Dim Hasil As Boolean = False
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

                            com.CommandText = "SELECT IsPosted FROM MBeli WHERE NoID=" & NoID
                            If NullToBool(com.ExecuteScalar()) Then
                                XtraMessageBox.Show("Nota telah diposting!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                Exit Try
                            End If

                            com.CommandText = "UPDATE MBeli SET IDTypePajak=" & NullToLong(txtTypePajak.EditValue) & " WHERE NoID=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                              "DiscNotaProsen=ROUND(@DiscNotaProsen, 2)" & vbCrLf & _
                                              ",DiscNotaRp=ROUND((@DiscNotaRp/(MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))) * 100.0, 2)" & vbCrLf & _
                                              "FROM MBeli " & vbCrLf & _
                                              "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID" & vbCrLf & _
                                              "WHERE MBeliD.IDHeader=" & NoID
                            com.Parameters.Clear()
                            com.Parameters.Add(New SqlParameter("@DiscNotaProsen", SqlDbType.Float)).Value = NullToDbl(txtDiscProsen.EditValue)
                            com.Parameters.Add(New SqlParameter("@DiscNotaRp", SqlDbType.Float)).Value = NullToDbl(txtDiscRp.EditValue)
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeliD SET DiscNotaRp=MBeli.DiscNotaRp-(ISNULL(MBeliD2.DiscNotaRp, 0)-ISNULL(MBeliD.DiscNotaRp, 0))" & vbCrLf & _
                                              "FROM MBeliD " & vbCrLf & _
                                              "INNER JOIN (SELECT IDHeader, MAX(NoID) AS NoID, SUM(DiscNotaRp) AS DiscNotaRp FROM MBeliD GROUP BY IDHeader) AS MBeliD2 ON MBeliD2.NoID=MBeliD.NoID" & vbCrLf & _
                                              "INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDHeader" & vbCrLf & _
                                              "WHERE MBeliD.IDHeader=" & NoID
                            com.Parameters.Clear()
                            com.Parameters.Add(New SqlParameter("@DiscNotaProsen", SqlDbType.Float)).Value = NullToDbl(txtDiscProsen.EditValue)
                            com.Parameters.Add(New SqlParameter("@DiscNotaRp", SqlDbType.Float)).Value = NullToDbl(txtDiscRp.EditValue)
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                              "PPN=ROUND(CASE WHEN MBeli.IDTypePajak=0 THEN 0 WHEN MBeli.IDTypePajak=1 THEN ROUND((MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))-MBeliD.DiscNotaRp, 2)/1.1 ELSE ROUND((MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))-MBeliD.DiscNotaRp, 2)/1.0 END * 0.1, 0), " & vbCrLf & _
                                              "DPP=ROUND(CASE WHEN MBeli.IDTypePajak=0 THEN 0 WHEN MBeli.IDTypePajak=1 THEN ROUND((MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))-MBeliD.DiscNotaRp, 2)/1.1 ELSE ROUND((MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))-MBeliD.DiscNotaRp, 2)/1.0 END, 0) " & vbCrLf & _
                                              "FROM MBeli " & vbCrLf & _
                                              "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                              "WHERE MBeliD.IDHeader=" & NoID
                            com.Parameters.Clear()
                            com.Parameters.Add(New SqlParameter("@DiscNotaProsen", SqlDbType.Float)).Value = NullToDbl(txtDiscProsen.EditValue)
                            com.Parameters.Add(New SqlParameter("@DiscNotaRp", SqlDbType.Float)).Value = NullToDbl(txtDiscRp.EditValue)
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                              "PPN=ISNULL(MBeli.PPN, 0)-(ISNULL(Detil.PPN, 0)-ISNULL(MBeliD.PPN, 0)), " & vbCrLf & _
                                              "DPP=ISNULL(MBeli.DPP, 0)-(ISNULL(Detil.DPP, 0)-ISNULL(MBeliD.DPP, 0)) " & vbCrLf & _
                                              "FROM MBeli " & vbCrLf & _
                                              "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID" & vbCrLf & _
                                              "INNER JOIN (SELECT IDHeader, SUM(DPP) AS DPP, SUM(PPN) AS PPN, MAX(NoID) AS NoID FROM MBeliD GROUP BY IDHeader) AS Detil ON Detil.IDHeader=MBeli.NoID AND Detil.NoID=MBeliD.NoID" & vbCrLf & _
                                              "WHERE MBeliD.IDHeader=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                              "Jumlah=CASE WHEN MBeli.IDTypePajak=0 THEN ROUND((MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))-MBeliD.DiscNotaRp, 2) ELSE MBeliD.DPP+MBeliD.PPN END " & vbCrLf & _
                                              "FROM MBeli " & vbCrLf & _
                                              "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                              "WHERE MBeliD.IDHeader=" & NoID
                            com.Parameters.Clear()
                            com.Parameters.Add(New SqlParameter("@DiscNotaProsen", SqlDbType.Float)).Value = NullToDbl(txtDiscProsen.EditValue)
                            com.Parameters.Add(New SqlParameter("@DiscNotaRp", SqlDbType.Float)).Value = NullToDbl(txtDiscRp.EditValue)
                            com.ExecuteNonQuery()

                            If com.Transaction IsNot Nothing Then
                                com.Transaction.Commit()
                            End If
                            Hasil = True
                            RefreshDetil()
                            IDTypePajak = NullTolInt(txtTypePajak.EditValue)
                        Catch ex As Exception
                            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End Using
                End Using
            End Using
        End Using

        Return Hasil
    End Function
    Private Function HapusDetil(ByVal IDDetil As Long) As Boolean
        Dim Hasil As Boolean = False
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

                            com.CommandText = "SELECT IsPosted FROM MBeli WHERE NoID=" & NoID
                            If NullToBool(com.ExecuteScalar()) Then
                                XtraMessageBox.Show("Nota telah diposting!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                Exit Try
                            End If

                            com.CommandText = "DELETE FROM MBeliD WHERE NoID=" & IDDetil
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeli SET IDTypePajak=" & NullToLong(txtTypePajak.EditValue) & " WHERE NoID=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                              "DiscNotaProsen=ROUND(@DiscNotaProsen, 2)" & vbCrLf & _
                                              ",DiscNotaRp=ROUND((@DiscNotaRp/(MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))) * 100.0, 2)" & vbCrLf & _
                                              "FROM MBeli " & vbCrLf & _
                                              "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID" & vbCrLf & _
                                              "WHERE MBeliD.IDHeader=" & NoID
                            com.Parameters.Clear()
                            com.Parameters.Add(New SqlParameter("@DiscNotaProsen", SqlDbType.Float)).Value = NullToDbl(txtDiscProsen.EditValue)
                            com.Parameters.Add(New SqlParameter("@DiscNotaRp", SqlDbType.Float)).Value = NullToDbl(txtDiscRp.EditValue)
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeliD SET DiscNotaRp=MBeli.DiscNotaRp-(ISNULL(MBeliD2.DiscNotaRp, 0)-ISNULL(MBeliD.DiscNotaRp, 0))" & vbCrLf & _
                                              "FROM MBeliD " & vbCrLf & _
                                              "INNER JOIN (SELECT IDHeader, MAX(NoID) AS NoID, SUM(DiscNotaRp) AS DiscNotaRp FROM MBeliD GROUP BY IDHeader) AS MBeliD2 ON MBeliD2.NoID=MBeliD.NoID" & vbCrLf & _
                                              "INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDHeader" & vbCrLf & _
                                              "WHERE MBeliD.IDHeader=" & NoID
                            com.Parameters.Clear()
                            com.Parameters.Add(New SqlParameter("@DiscNotaProsen", SqlDbType.Float)).Value = NullToDbl(txtDiscProsen.EditValue)
                            com.Parameters.Add(New SqlParameter("@DiscNotaRp", SqlDbType.Float)).Value = NullToDbl(txtDiscRp.EditValue)
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                              "PPN=ROUND(CASE WHEN MBeli.IDTypePajak=0 THEN 0 WHEN MBeli.IDTypePajak=1 THEN ROUND((MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))-MBeliD.DiscNotaRp, 2)/1.1 ELSE ROUND((MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))-MBeliD.DiscNotaRp, 2)/1.0 END * 0.1, 0), " & vbCrLf & _
                                              "DPP=ROUND(CASE WHEN MBeli.IDTypePajak=0 THEN 0 WHEN MBeli.IDTypePajak=1 THEN ROUND((MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))-MBeliD.DiscNotaRp, 2)/1.1 ELSE ROUND((MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))-MBeliD.DiscNotaRp, 2)/1.0 END, 0) " & vbCrLf & _
                                              "FROM MBeli " & vbCrLf & _
                                              "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                              "WHERE MBeliD.IDHeader=" & NoID
                            com.Parameters.Clear()
                            com.Parameters.Add(New SqlParameter("@DiscNotaProsen", SqlDbType.Float)).Value = NullToDbl(txtDiscProsen.EditValue)
                            com.Parameters.Add(New SqlParameter("@DiscNotaRp", SqlDbType.Float)).Value = NullToDbl(txtDiscRp.EditValue)
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                              "PPN=ISNULL(MBeli.PPN, 0)-(ISNULL(Detil.PPN, 0)-ISNULL(MBeliD.PPN, 0)), " & vbCrLf & _
                                              "DPP=ISNULL(MBeli.DPP, 0)-(ISNULL(Detil.DPP, 0)-ISNULL(MBeliD.DPP, 0)) " & vbCrLf & _
                                              "FROM MBeli " & vbCrLf & _
                                              "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID" & vbCrLf & _
                                              "INNER JOIN (SELECT IDHeader, SUM(DPP) AS DPP, SUM(PPN) AS PPN, MAX(NoID) AS NoID FROM MBeliD GROUP BY IDHeader) AS Detil ON Detil.IDHeader=MBeli.NoID AND Detil.NoID=MBeliD.NoID" & vbCrLf & _
                                              "WHERE MBeliD.IDHeader=" & NoID
                            com.ExecuteNonQuery()

                            com.CommandText = "UPDATE MBeliD SET " & vbCrLf & _
                                              "Jumlah=CASE WHEN MBeli.IDTypePajak=0 THEN ROUND((MBeliD.JumlahBruto * (1.0-(@DiscNotaProsen/100.0)))-MBeliD.DiscNotaRp, 2) ELSE MBeliD.DPP+MBeliD.PPN END " & vbCrLf & _
                                              "FROM MBeli " & vbCrLf & _
                                              "INNER JOIN MBeliD ON MBeliD.IDHeader=MBeli.NoID " & vbCrLf & _
                                              "WHERE MBeliD.IDHeader=" & NoID
                            com.Parameters.Clear()
                            com.Parameters.Add(New SqlParameter("@DiscNotaProsen", SqlDbType.Float)).Value = NullToDbl(txtDiscProsen.EditValue)
                            com.Parameters.Add(New SqlParameter("@DiscNotaRp", SqlDbType.Float)).Value = NullToDbl(txtDiscRp.EditValue)
                            com.ExecuteNonQuery()

                            If com.Transaction IsNot Nothing Then
                                com.Transaction.Commit()
                            End If
                            Hasil = True
                            IDTypePajak = NullTolInt(txtTypePajak.EditValue)
                        Catch ex As Exception
                            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End Using
                End Using
            End Using
        End Using

        Return Hasil
    End Function
#End Region

    Private Sub frmEntriPO_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Using dlg As New DevExpress.Utils.WaitDialogForm("Sedang merefresh data ...", NamaAplikasi)
            Try
                dlg.Show()
                dlg.Focus()
                InitLoadLookUp()
                LoadData()
                RefreshDetil()
                If System.IO.File.Exists([Public].SettingPerusahaan.PathLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
                    LayoutControl1.RestoreLayoutFromXml([Public].SettingPerusahaan.PathLayouts & Me.Name & LayoutControl1.Name & ".xml")
                End If

                AddHandler txtSubtotal.LostFocus, AddressOf txt_LostFocus
                AddHandler txtDiscProsen.LostFocus, AddressOf txt_LostFocus
                AddHandler txtDiscRp.LostFocus, AddressOf txt_LostFocus
                AddHandler txtDPP.LostFocus, AddressOf txt_LostFocus
                AddHandler txtPPN.LostFocus, AddressOf txt_LostFocus
                AddHandler txtTotal.LostFocus, AddressOf txt_LostFocus
                AddHandler txtBruto.LostFocus, AddressOf txt_LostFocus
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub



    Private Sub txtSupplier_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSupplier.EditValueChanged
        Using cn As New SqlConnection(StrKonSQL)
            Using com As New SqlCommand
                Using oDA As New SqlDataAdapter
                    Using ds As New DataSet
                        Try
                            cn.Open()
                            com.Connection = cn
                            com.CommandTimeout = cn.ConnectionTimeout
                            oDA.SelectCommand = com

                            com.CommandText = "SELECT NoID, Kode, Nama, Alamat FROM MAlamat WHERE NoID=" & NullToLong(txtSupplier.EditValue)
                            oDA.Fill(ds, "MSupplier")
                            If ds.Tables("MSupplier").Rows.Count >= 1 Then
                                txtNamaSupplier.Text = NullToStr(ds.Tables("MSupplier").Rows(0).Item("Nama"))
                                txtAlamatSupplier.Text = NullToStr(ds.Tables("MSupplier").Rows(0).Item("Alamat"))
                            Else
                                txtNamaSupplier.Text = ""
                                txtAlamatSupplier.Text = ""
                            End If
                            InitLoadLookUpPO()
                        Catch ex As Exception
                            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Private Sub txtTanggal_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTanggal.EditValueChanged
        Try
            If pStatus = pStatusForm.Baru Then
                txtKode.Text = "AUTO" 'Repository.RepKode.GetNewKode("MBeli", "Kode", "PO-" & NullToDate(txtTanggal.EditValue).ToString("yyMM") & "-", "", Repository.RepKode.Format.A00000)
                txtJatuhTempo.EditValue = DateAdd(DateInterval.Day, 30, txtTanggal.EditValue)
                InitLoadLookUpPO()
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub gv_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataSourceChanged, gvSupplier.DataSourceChanged, gvTypePajak.DataSourceChanged, gvPO.DataSourceChanged, gvGudang.DataSourceChanged
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

    Private Sub mnRefresh_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        RefreshDetil()
    End Sub

    Private Sub mnTutup_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnTutup.ItemClick
        Me.Close()
        DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub mnHapus_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHapus.ItemClick
        If (pStatus = pStatusForm.Edit OrElse pStatus = pStatusForm.TempInsert) AndAlso GridView1.RowCount >= 1 Then
            If HapusDetil(NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))) Then
                RefreshDetil()
            End If
        End If
    End Sub

    Private Sub mnEdit_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnEdit.ItemClick
        If (pStatus = pStatusForm.Edit OrElse pStatus = pStatusForm.TempInsert) AndAlso GridView1.RowCount >= 1 Then
            Using frm As New frmEntriBeliD(Me, NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")), NoID, txtTypePajak.EditValue, -1)
                Try
                    If frm.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                        RefreshDetil(frm.NoID)
                    End If
                Catch ex As Exception
                    XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End If
    End Sub

    Private Sub mnBaru_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnBaru.ItemClick
        If pStatusChange() Then
            Using frm As New frmEntriBeliD(Me, -1, NoID, txtTypePajak.EditValue, -1)
                Try
                    If frm.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                        RefreshDetil(frm.NoID)
                    End If
                Catch ex As Exception
                    XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End If
    End Sub

    Private Function pStatusChange() As Boolean
        If pStatus = pStatusForm.Baru Then
            Return SimpanData()
        Else
            Return True
        End If
    End Function

    Private Sub mnSaveLayouts_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayouts.ItemClick
        Using frm As New frmOtorisasi
            Try
                If frm.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                    LayoutControl1.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & LayoutControl1.Name & ".xml")
                    gvSupplier.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & gvSupplier.Name & ".xml")
                    gvTypePajak.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & gvTypePajak.Name & ".xml")
                    gvPO.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & gvPO.Name & ".xml")
                    gvGudang.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & gvGudang.Name & ".xml")
                    GridView1.SaveLayoutToXml([Public].SettingPerusahaan.PathLayouts & Me.Name & GridView1.Name & ".xml")
                End If
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub mnSimpan_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSimpan.ItemClick
        If SimpanData() Then
            If pStatus = pStatusForm.Baru Then
                pStatus = pStatusForm.Edit
                LoadData()
            ElseIf pStatus = pStatusForm.Edit OrElse pStatus = pStatusForm.TempInsert Then
                Dim x As frmDaftarTransaksi = Nothing
                For Each frm In Me.MdiParent.MdiChildren
                    If TypeOf frm Is frmDaftarTransaksi AndAlso
                    TryCast(frm, frmDaftarTransaksi).Name.ToString = modMain.FormName.DaftarPembelian.ToString Then
                        x = frm
                    End If
                Next
                If x IsNot Nothing Then
                    x.RefreshData(NoID)
                    x.Show()
                    x.Focus()
                End If
                DialogResult = System.Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub

    Private Function IsValidasi() As Boolean
        DxErrorProvider1.ClearErrors()
        If txtTypePajak.Text = "" Then
            DxErrorProvider1.SetError(txtTypePajak, "Type Pajak harus dipilih!", DXErrorProvider.ErrorType.Critical)
        End If
        If txtSupplier.Text = "" Then
            DxErrorProvider1.SetError(txtSupplier, "Supplier harus dipilih!", DXErrorProvider.ErrorType.Critical)
        End If
        If txtGudang.Text = "" Then
            DxErrorProvider1.SetError(txtGudang, "Gudang harus dipilih!", DXErrorProvider.ErrorType.Critical)
        End If
        If DateDiff(DateInterval.Day, txtTanggal.EditValue, txtJatuhTempo.EditValue) < 0 Then
            DxErrorProvider1.SetError(txtJatuhTempo, "Jatuh Tempo Pembelian salah!", DXErrorProvider.ErrorType.Critical)
        End If
        If txtTotal.EditValue < 0 Then
            DxErrorProvider1.SetError(txtTotal, "Total Pembelian salah!", DXErrorProvider.ErrorType.Critical)
        End If
        Return Not DxErrorProvider1.HasErrors
    End Function

    Private Sub txtTypePajak_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTypePajak.EditValueChanged

    End Sub

    Private Sub txtTypePajak_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTypePajak.LostFocus
        If (pStatus = pStatusForm.Edit OrElse pStatus = pStatusForm.TempInsert) AndAlso IDTypePajak <> NullTolInt(txtTypePajak.EditValue) Then
            UpdateTypePajak()
        End If
    End Sub

    Private Sub txtPO_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtPO.ButtonClick
        Select Case e.Button.Index
            Case 0

            Case 1
                InitLoadDataPO()
            Case 2
                InitLoadLookUpPO()
        End Select
    End Sub

    Private Sub GridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DoubleClick
        mnEdit.PerformClick()
    End Sub

    Private Sub txt_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        HitungJumlah()
    End Sub

    Private Sub mnFastEntri_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnFastEntri.ItemClick
        txtFastEntri.Focus()
    End Sub

    Private Sub txtFastEntri_KeyDown(sender As Object, e As KeyEventArgs) Handles txtFastEntri.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter
                If txtFastEntri.Text <> "" AndAlso IIf(pStatus = pStatusForm.Baru, SimpanData(), True) = True Then
                    Dim iList = Repository.RepSQLServer.GetBarangDetil(txtFastEntri.Text)
                    If iList.Count >= 1 Then
                        Using frm As New frmEntriBeliD(Me, -1, NoID, txtTypePajak.EditValue, iList(0).NoID)
                            Try
                                If frm.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                                    RefreshDetil(frm.NoID)
                                End If
                            Catch ex As Exception
                                XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End Try
                        End Using
                        txtFastEntri.Text = ""
                    Else
                        XtraMessageBox.Show("Data tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK)
                    End If
                    mnFastEntri.PerformClick()
                End If
        End Select
    End Sub
End Class
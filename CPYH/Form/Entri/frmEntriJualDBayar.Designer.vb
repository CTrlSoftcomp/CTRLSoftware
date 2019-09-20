<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEntriJualDBayar
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.Bar2 = New DevExpress.XtraBars.Bar
        Me.mnTambah = New DevExpress.XtraBars.BarButtonItem
        Me.mnHapus = New DevExpress.XtraBars.BarButtonItem
        Me.mnSimpan = New DevExpress.XtraBars.BarButtonItem
        Me.mnBatal = New DevExpress.XtraBars.BarButtonItem
        Me.Bar3 = New DevExpress.XtraBars.Bar
        Me.BarStaticItem4 = New DevExpress.XtraBars.BarStaticItem
        Me.BarStaticItem3 = New DevExpress.XtraBars.BarStaticItem
        Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem
        Me.BarStaticItem2 = New DevExpress.XtraBars.BarStaticItem
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl
        Me.mnSaveLayout = New DevExpress.XtraBars.BarButtonItem
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl
        Me.PembayaranBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colNoID = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIDJenisTransaksi = New DevExpress.XtraGrid.Columns.GridColumn
        Me.riSearchLookUpEdit = New DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit
        Me.RepositoryItemSearchLookUpEdit1View = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colIsBank = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colAtasNama = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colNoRekening = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colNominal = New DevExpress.XtraGrid.Columns.GridColumn
        Me.riCalcEdit = New DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit
        Me.colChargeProsen = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colChargeRp = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colTotal = New DevExpress.XtraGrid.Columns.GridColumn
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl
        Me.lbKembali = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl8 = New DevExpress.XtraEditors.LabelControl
        Me.lbTotalBayar = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl6 = New DevExpress.XtraEditors.LabelControl
        Me.lbCharge = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl
        Me.lbSubtotal = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl
        Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer
        Me.LineShape1 = New Microsoft.VisualBasic.PowerPacks.LineShape
        Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PembayaranBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.riSearchLookUpEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemSearchLookUpEdit1View, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.riCalcEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BarManager1
        '
        Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar2, Me.Bar3})
        Me.BarManager1.DockControls.Add(Me.barDockControlTop)
        Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.BarManager1.DockControls.Add(Me.barDockControlRight)
        Me.BarManager1.Form = Me
        Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarStaticItem2, Me.BarStaticItem3, Me.BarStaticItem1, Me.mnBatal, Me.mnHapus, Me.mnSimpan, Me.mnTambah, Me.BarStaticItem4, Me.mnSaveLayout})
        Me.BarManager1.MainMenu = Me.Bar2
        Me.BarManager1.MaxItemId = 9
        Me.BarManager1.StatusBar = Me.Bar3
        '
        'Bar2
        '
        Me.Bar2.BarName = "Main menu"
        Me.Bar2.DockCol = 0
        Me.Bar2.DockRow = 0
        Me.Bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.Bar2.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.mnTambah), New DevExpress.XtraBars.LinkPersistInfo(Me.mnHapus), New DevExpress.XtraBars.LinkPersistInfo(Me.mnSimpan, True), New DevExpress.XtraBars.LinkPersistInfo(Me.mnBatal)})
        Me.Bar2.OptionsBar.MultiLine = True
        Me.Bar2.OptionsBar.UseWholeRow = True
        Me.Bar2.Text = "Main menu"
        '
        'mnTambah
        '
        Me.mnTambah.Caption = "&Tambah"
        Me.mnTambah.Id = 6
        Me.mnTambah.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F1)
        Me.mnTambah.Name = "mnTambah"
        '
        'mnHapus
        '
        Me.mnHapus.Caption = "&Hapus"
        Me.mnHapus.Id = 4
        Me.mnHapus.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F4)
        Me.mnHapus.Name = "mnHapus"
        '
        'mnSimpan
        '
        Me.mnSimpan.Caption = "&Simpan Pembayaran"
        Me.mnSimpan.Id = 2
        Me.mnSimpan.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F6)
        Me.mnSimpan.Name = "mnSimpan"
        '
        'mnBatal
        '
        Me.mnBatal.Caption = "&Batal"
        Me.mnBatal.Id = 3
        Me.mnBatal.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F3)
        Me.mnBatal.Name = "mnBatal"
        '
        'Bar3
        '
        Me.Bar3.BarName = "Status bar"
        Me.Bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
        Me.Bar3.DockCol = 0
        Me.Bar3.DockRow = 0
        Me.Bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
        Me.Bar3.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarStaticItem4), New DevExpress.XtraBars.LinkPersistInfo(Me.BarStaticItem3), New DevExpress.XtraBars.LinkPersistInfo(Me.BarStaticItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.BarStaticItem2)})
        Me.Bar3.OptionsBar.AllowQuickCustomization = False
        Me.Bar3.OptionsBar.DrawDragBorder = False
        Me.Bar3.OptionsBar.UseWholeRow = True
        Me.Bar3.Text = "Status bar"
        '
        'BarStaticItem4
        '
        Me.BarStaticItem4.Caption = "F1 - Tambah"
        Me.BarStaticItem4.Id = 7
        Me.BarStaticItem4.Name = "BarStaticItem4"
        Me.BarStaticItem4.TextAlignment = System.Drawing.StringAlignment.Near
        '
        'BarStaticItem3
        '
        Me.BarStaticItem3.Caption = "F4 - Hapus"
        Me.BarStaticItem3.Id = 5
        Me.BarStaticItem3.Name = "BarStaticItem3"
        Me.BarStaticItem3.TextAlignment = System.Drawing.StringAlignment.Near
        '
        'BarStaticItem1
        '
        Me.BarStaticItem1.Caption = "F6 - Simpan Pembayaran"
        Me.BarStaticItem1.Id = 0
        Me.BarStaticItem1.Name = "BarStaticItem1"
        Me.BarStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near
        '
        'BarStaticItem2
        '
        Me.BarStaticItem2.Caption = "F3 - Batal"
        Me.BarStaticItem2.Id = 1
        Me.BarStaticItem2.Name = "BarStaticItem2"
        Me.BarStaticItem2.TextAlignment = System.Drawing.StringAlignment.Near
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Size = New System.Drawing.Size(819, 22)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 436)
        Me.barDockControlBottom.Size = New System.Drawing.Size(819, 26)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 22)
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 414)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(819, 22)
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 414)
        '
        'mnSaveLayout
        '
        Me.mnSaveLayout.Caption = "Save Layout"
        Me.mnSaveLayout.Id = 8
        Me.mnSaveLayout.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F10)
        Me.mnSaveLayout.Name = "mnSaveLayout"
        '
        'GridControl1
        '
        Me.GridControl1.DataSource = Me.PembayaranBindingSource
        Me.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridControl1.Location = New System.Drawing.Point(0, 165)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.riCalcEdit, Me.riSearchLookUpEdit})
        Me.GridControl1.Size = New System.Drawing.Size(819, 271)
        Me.GridControl1.TabIndex = 7
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'PembayaranBindingSource
        '
        Me.PembayaranBindingSource.DataSource = GetType(CtrlSoft.Model.Pembayaran)
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colNoID, Me.colIDJenisTransaksi, Me.colIsBank, Me.colAtasNama, Me.colNoRekening, Me.colNominal, Me.colChargeProsen, Me.colChargeRp, Me.colTotal})
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsCustomization.AllowColumnMoving = False
        Me.GridView1.OptionsCustomization.AllowFilter = False
        Me.GridView1.OptionsCustomization.AllowGroup = False
        Me.GridView1.OptionsCustomization.AllowSort = False
        Me.GridView1.OptionsFilter.AllowColumnMRUFilterList = False
        Me.GridView1.OptionsFilter.AllowFilterEditor = False
        Me.GridView1.OptionsFilter.AllowMRUFilterList = False
        Me.GridView1.OptionsFind.AllowFindPanel = False
        Me.GridView1.OptionsNavigation.EnterMoveNextColumn = True
        Me.GridView1.OptionsView.ColumnAutoWidth = False
        Me.GridView1.OptionsView.ShowFooter = True
        Me.GridView1.OptionsView.ShowGroupPanel = False
        '
        'colNoID
        '
        Me.colNoID.DisplayFormat.FormatString = "n0"
        Me.colNoID.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.colNoID.FieldName = "NoID"
        Me.colNoID.Name = "colNoID"
        Me.colNoID.OptionsColumn.AllowEdit = False
        Me.colNoID.OptionsColumn.AllowFocus = False
        '
        'colIDJenisTransaksi
        '
        Me.colIDJenisTransaksi.Caption = "Jenis Pembayaran"
        Me.colIDJenisTransaksi.ColumnEdit = Me.riSearchLookUpEdit
        Me.colIDJenisTransaksi.FieldName = "IDJenisPembayaran"
        Me.colIDJenisTransaksi.Name = "colIDJenisTransaksi"
        Me.colIDJenisTransaksi.Visible = True
        Me.colIDJenisTransaksi.VisibleIndex = 0
        Me.colIDJenisTransaksi.Width = 96
        '
        'riSearchLookUpEdit
        '
        Me.riSearchLookUpEdit.AutoHeight = False
        Me.riSearchLookUpEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.riSearchLookUpEdit.Name = "riSearchLookUpEdit"
        Me.riSearchLookUpEdit.View = Me.RepositoryItemSearchLookUpEdit1View
        '
        'RepositoryItemSearchLookUpEdit1View
        '
        Me.RepositoryItemSearchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.RepositoryItemSearchLookUpEdit1View.Name = "RepositoryItemSearchLookUpEdit1View"
        Me.RepositoryItemSearchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.RepositoryItemSearchLookUpEdit1View.OptionsView.ShowGroupPanel = False
        '
        'colIsBank
        '
        Me.colIsBank.Caption = "Bank"
        Me.colIsBank.FieldName = "IsBank"
        Me.colIsBank.Name = "colIsBank"
        '
        'colAtasNama
        '
        Me.colAtasNama.FieldName = "AtasNama"
        Me.colAtasNama.Name = "colAtasNama"
        Me.colAtasNama.Visible = True
        Me.colAtasNama.VisibleIndex = 1
        '
        'colNoRekening
        '
        Me.colNoRekening.FieldName = "NoRekening"
        Me.colNoRekening.Name = "colNoRekening"
        Me.colNoRekening.Visible = True
        Me.colNoRekening.VisibleIndex = 2
        '
        'colNominal
        '
        Me.colNominal.ColumnEdit = Me.riCalcEdit
        Me.colNominal.DisplayFormat.FormatString = "n2"
        Me.colNominal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.colNominal.FieldName = "Nominal"
        Me.colNominal.Name = "colNominal"
        Me.colNominal.SummaryItem.DisplayFormat = "{0:n2}"
        Me.colNominal.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.colNominal.Visible = True
        Me.colNominal.VisibleIndex = 3
        '
        'riCalcEdit
        '
        Me.riCalcEdit.Appearance.Options.UseTextOptions = True
        Me.riCalcEdit.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.riCalcEdit.AutoHeight = False
        Me.riCalcEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.riCalcEdit.EditFormat.FormatString = "n2"
        Me.riCalcEdit.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.riCalcEdit.Mask.EditMask = "n2"
        Me.riCalcEdit.Mask.UseMaskAsDisplayFormat = True
        Me.riCalcEdit.Name = "riCalcEdit"
        '
        'colChargeProsen
        '
        Me.colChargeProsen.AppearanceCell.BackColor = System.Drawing.Color.Silver
        Me.colChargeProsen.AppearanceCell.Options.UseBackColor = True
        Me.colChargeProsen.DisplayFormat.FormatString = "n2"
        Me.colChargeProsen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.colChargeProsen.FieldName = "ChargeProsen"
        Me.colChargeProsen.Name = "colChargeProsen"
        Me.colChargeProsen.OptionsColumn.AllowEdit = False
        Me.colChargeProsen.OptionsColumn.AllowFocus = False
        Me.colChargeProsen.Visible = True
        Me.colChargeProsen.VisibleIndex = 4
        Me.colChargeProsen.Width = 80
        '
        'colChargeRp
        '
        Me.colChargeRp.AppearanceCell.BackColor = System.Drawing.Color.Silver
        Me.colChargeRp.AppearanceCell.Options.UseBackColor = True
        Me.colChargeRp.DisplayFormat.FormatString = "n2"
        Me.colChargeRp.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.colChargeRp.FieldName = "ChargeRp"
        Me.colChargeRp.Name = "colChargeRp"
        Me.colChargeRp.OptionsColumn.AllowEdit = False
        Me.colChargeRp.OptionsColumn.AllowFocus = False
        Me.colChargeRp.SummaryItem.DisplayFormat = "{0:n2}"
        Me.colChargeRp.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.colChargeRp.Visible = True
        Me.colChargeRp.VisibleIndex = 5
        '
        'colTotal
        '
        Me.colTotal.AppearanceCell.BackColor = System.Drawing.Color.Silver
        Me.colTotal.AppearanceCell.Options.UseBackColor = True
        Me.colTotal.DisplayFormat.FormatString = "n2"
        Me.colTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.colTotal.FieldName = "Total"
        Me.colTotal.Name = "colTotal"
        Me.colTotal.OptionsColumn.AllowEdit = False
        Me.colTotal.OptionsColumn.AllowFocus = False
        Me.colTotal.SummaryItem.DisplayFormat = "{0:n2}"
        Me.colTotal.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.colTotal.Visible = True
        Me.colTotal.VisibleIndex = 6
        '
        'PanelControl1
        '
        Me.PanelControl1.Controls.Add(Me.lbKembali)
        Me.PanelControl1.Controls.Add(Me.LabelControl8)
        Me.PanelControl1.Controls.Add(Me.lbTotalBayar)
        Me.PanelControl1.Controls.Add(Me.LabelControl6)
        Me.PanelControl1.Controls.Add(Me.lbCharge)
        Me.PanelControl1.Controls.Add(Me.LabelControl4)
        Me.PanelControl1.Controls.Add(Me.lbSubtotal)
        Me.PanelControl1.Controls.Add(Me.LabelControl1)
        Me.PanelControl1.Controls.Add(Me.ShapeContainer1)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelControl1.Location = New System.Drawing.Point(0, 22)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(819, 143)
        Me.PanelControl1.TabIndex = 12
        '
        'lbKembali
        '
        Me.lbKembali.Appearance.Font = New System.Drawing.Font("Segoe UI Semibold", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbKembali.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.lbKembali.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.lbKembali.Location = New System.Drawing.Point(612, 112)
        Me.lbKembali.Name = "lbKembali"
        Me.lbKembali.Size = New System.Drawing.Size(195, 25)
        Me.lbKembali.TabIndex = 7
        Me.lbKembali.Text = "999.999.999,00"
        '
        'LabelControl8
        '
        Me.LabelControl8.Appearance.Font = New System.Drawing.Font("Segoe UI Semibold", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl8.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.LabelControl8.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.LabelControl8.Location = New System.Drawing.Point(291, 112)
        Me.LabelControl8.Name = "LabelControl8"
        Me.LabelControl8.Size = New System.Drawing.Size(315, 25)
        Me.LabelControl8.TabIndex = 6
        Me.LabelControl8.Text = "Kembalian / Kurang Bayar"
        '
        'lbTotalBayar
        '
        Me.lbTotalBayar.Appearance.Font = New System.Drawing.Font("Segoe UI Semibold", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbTotalBayar.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.lbTotalBayar.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.lbTotalBayar.Location = New System.Drawing.Point(612, 68)
        Me.lbTotalBayar.Name = "lbTotalBayar"
        Me.lbTotalBayar.Size = New System.Drawing.Size(195, 25)
        Me.lbTotalBayar.TabIndex = 5
        Me.lbTotalBayar.Text = "999.999.999,00"
        '
        'LabelControl6
        '
        Me.LabelControl6.Appearance.Font = New System.Drawing.Font("Segoe UI Semibold", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl6.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.LabelControl6.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.LabelControl6.Location = New System.Drawing.Point(468, 68)
        Me.LabelControl6.Name = "LabelControl6"
        Me.LabelControl6.Size = New System.Drawing.Size(138, 25)
        Me.LabelControl6.TabIndex = 4
        Me.LabelControl6.Text = "Total Bayar"
        '
        'lbCharge
        '
        Me.lbCharge.Appearance.Font = New System.Drawing.Font("Segoe UI Semibold", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbCharge.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.lbCharge.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.lbCharge.Location = New System.Drawing.Point(612, 37)
        Me.lbCharge.Name = "lbCharge"
        Me.lbCharge.Size = New System.Drawing.Size(195, 25)
        Me.lbCharge.TabIndex = 3
        Me.lbCharge.Text = "999.999.999,00"
        '
        'LabelControl4
        '
        Me.LabelControl4.Appearance.Font = New System.Drawing.Font("Segoe UI Semibold", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl4.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.LabelControl4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.LabelControl4.Location = New System.Drawing.Point(468, 37)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Size = New System.Drawing.Size(138, 25)
        Me.LabelControl4.TabIndex = 2
        Me.LabelControl4.Text = "Charge"
        '
        'lbSubtotal
        '
        Me.lbSubtotal.Appearance.Font = New System.Drawing.Font("Segoe UI Semibold", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbSubtotal.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.lbSubtotal.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.lbSubtotal.Location = New System.Drawing.Point(612, 6)
        Me.lbSubtotal.Name = "lbSubtotal"
        Me.lbSubtotal.Size = New System.Drawing.Size(195, 25)
        Me.lbSubtotal.TabIndex = 1
        Me.lbSubtotal.Text = "999.999.999,00"
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Segoe UI Semibold", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.LabelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.LabelControl1.Location = New System.Drawing.Point(468, 6)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(138, 25)
        Me.LabelControl1.TabIndex = 0
        Me.LabelControl1.Text = "Subtotal"
        '
        'ShapeContainer1
        '
        Me.ShapeContainer1.Location = New System.Drawing.Point(2, 2)
        Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.ShapeContainer1.Name = "ShapeContainer1"
        Me.ShapeContainer1.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {Me.LineShape1})
        Me.ShapeContainer1.Size = New System.Drawing.Size(815, 139)
        Me.ShapeContainer1.TabIndex = 8
        Me.ShapeContainer1.TabStop = False
        '
        'LineShape1
        '
        Me.LineShape1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LineShape1.BorderWidth = 2
        Me.LineShape1.Name = "LineShape1"
        Me.LineShape1.X1 = 504
        Me.LineShape1.X2 = 812
        Me.LineShape1.Y1 = 97
        Me.LineShape1.Y2 = 97
        '
        'DxErrorProvider1
        '
        Me.DxErrorProvider1.ContainerControl = Me
        '
        'frmEntriJualDBayar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(819, 462)
        Me.Controls.Add(Me.GridControl1)
        Me.Controls.Add(Me.PanelControl1)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Name = "frmEntriJualDBayar"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Input Pembayaran Penjualan"
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PembayaranBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.riSearchLookUpEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemSearchLookUpEdit1View, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.riCalcEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
    Friend WithEvents Bar2 As DevExpress.XtraBars.Bar
    Friend WithEvents mnSimpan As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnBatal As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
    Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents BarStaticItem2 As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents PembayaranBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents riSearchLookUpEdit As DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit
    Friend WithEvents RepositoryItemSearchLookUpEdit1View As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents riCalcEdit As DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit
    Friend WithEvents BarStaticItem3 As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents mnHapus As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnTambah As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarStaticItem4 As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lbTotalBayar As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl6 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lbCharge As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lbSubtotal As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lbKembali As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl8 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
    Friend WithEvents LineShape1 As Microsoft.VisualBasic.PowerPacks.LineShape
    Friend WithEvents colNoID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIDJenisTransaksi As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colAtasNama As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colNoRekening As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colNominal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colChargeProsen As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colChargeRp As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTotal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIsBank As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents mnSaveLayout As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
End Class

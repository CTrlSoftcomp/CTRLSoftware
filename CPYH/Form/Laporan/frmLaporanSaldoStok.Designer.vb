<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLaporanSaldoStok
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
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl
        Me.cmdCetak = New DevExpress.XtraEditors.SimpleButton
        Me.cmdRefresh = New DevExpress.XtraEditors.SimpleButton
        Me.cmdTutup = New DevExpress.XtraEditors.SimpleButton
        Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl
        Me.LabelControl5 = New DevExpress.XtraEditors.LabelControl
        Me.txtMerk = New DevExpress.XtraEditors.SearchLookUpEdit
        Me.gvMerk = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl
        Me.txtSupplier = New DevExpress.XtraEditors.SearchLookUpEdit
        Me.gvSupplier = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl
        Me.txtKategori = New DevExpress.XtraEditors.SearchLookUpEdit
        Me.gvKategori = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl
        Me.DateEdit1 = New DevExpress.XtraEditors.DateEdit
        Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.Bar2 = New DevExpress.XtraBars.Bar
        Me.BarSubItem1 = New DevExpress.XtraBars.BarSubItem
        Me.mnSaveLayout = New DevExpress.XtraBars.BarButtonItem
        Me.Bar4 = New DevExpress.XtraBars.Bar
        Me.mnCetak = New DevExpress.XtraBars.BarButtonItem
        Me.mnRefresh = New DevExpress.XtraBars.BarButtonItem
        Me.mnTutup = New DevExpress.XtraBars.BarButtonItem
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl
        Me.mnHistoryHarga = New DevExpress.XtraBars.BarButtonItem
        Me.mnHitungUlangSaldo = New DevExpress.XtraBars.BarButtonItem
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl
        Me.ckTdkAktif = New DevExpress.XtraEditors.CheckEdit
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl
        Me.BindingSource1 = New System.Windows.Forms.BindingSource(Me.components)
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.PopupMenu1 = New DevExpress.XtraBars.PopupMenu(Me.components)
        Me.Bar1 = New DevExpress.XtraBars.Bar
        Me.Bar3 = New DevExpress.XtraBars.Bar
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl2.SuspendLayout()
        CType(Me.txtMerk.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvMerk, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSupplier.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvSupplier, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKategori.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvKategori, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEdit1.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ckTdkAktif.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PopupMenu1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelControl1
        '
        Me.PanelControl1.Controls.Add(Me.cmdCetak)
        Me.PanelControl1.Controls.Add(Me.cmdRefresh)
        Me.PanelControl1.Controls.Add(Me.cmdTutup)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelControl1.Location = New System.Drawing.Point(0, 507)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(983, 51)
        Me.PanelControl1.TabIndex = 0
        Me.PanelControl1.Visible = False
        '
        'cmdCetak
        '
        Me.cmdCetak.Appearance.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCetak.Appearance.Options.UseFont = True
        Me.cmdCetak.ImageIndex = 6
        Me.cmdCetak.Location = New System.Drawing.Point(12, 5)
        Me.cmdCetak.Name = "cmdCetak"
        Me.cmdCetak.Size = New System.Drawing.Size(120, 38)
        Me.cmdCetak.TabIndex = 3
        Me.cmdCetak.Text = "&Cetak"
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdRefresh.Appearance.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRefresh.Appearance.Options.UseFont = True
        Me.cmdRefresh.ImageIndex = 3
        Me.cmdRefresh.Location = New System.Drawing.Point(725, 5)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.Size = New System.Drawing.Size(120, 38)
        Me.cmdRefresh.TabIndex = 4
        Me.cmdRefresh.Text = "&Refresh"
        '
        'cmdTutup
        '
        Me.cmdTutup.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdTutup.Appearance.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTutup.Appearance.Options.UseFont = True
        Me.cmdTutup.ImageIndex = 5
        Me.cmdTutup.Location = New System.Drawing.Point(851, 5)
        Me.cmdTutup.Name = "cmdTutup"
        Me.cmdTutup.Size = New System.Drawing.Size(120, 38)
        Me.cmdTutup.TabIndex = 5
        Me.cmdTutup.Text = "&Tutup"
        '
        'PanelControl2
        '
        Me.PanelControl2.Controls.Add(Me.LabelControl5)
        Me.PanelControl2.Controls.Add(Me.txtMerk)
        Me.PanelControl2.Controls.Add(Me.LabelControl3)
        Me.PanelControl2.Controls.Add(Me.txtSupplier)
        Me.PanelControl2.Controls.Add(Me.LabelControl4)
        Me.PanelControl2.Controls.Add(Me.txtKategori)
        Me.PanelControl2.Controls.Add(Me.LabelControl2)
        Me.PanelControl2.Controls.Add(Me.DateEdit1)
        Me.PanelControl2.Controls.Add(Me.LabelControl1)
        Me.PanelControl2.Controls.Add(Me.ckTdkAktif)
        Me.PanelControl2.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelControl2.Location = New System.Drawing.Point(0, 51)
        Me.PanelControl2.Name = "PanelControl2"
        Me.PanelControl2.Size = New System.Drawing.Size(983, 94)
        Me.PanelControl2.TabIndex = 1
        '
        'LabelControl5
        '
        Me.LabelControl5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelControl5.Location = New System.Drawing.Point(399, 15)
        Me.LabelControl5.Name = "LabelControl5"
        Me.LabelControl5.Size = New System.Drawing.Size(23, 13)
        Me.LabelControl5.TabIndex = 11
        Me.LabelControl5.Text = "Merk"
        '
        'txtMerk
        '
        Me.txtMerk.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMerk.EnterMoveNextControl = True
        Me.txtMerk.Location = New System.Drawing.Point(428, 12)
        Me.txtMerk.Name = "txtMerk"
        Me.txtMerk.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtMerk.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMerk.Properties.NullText = ""
        Me.txtMerk.Properties.View = Me.gvMerk
        Me.txtMerk.Size = New System.Drawing.Size(226, 20)
        Me.txtMerk.TabIndex = 12
        '
        'gvMerk
        '
        Me.gvMerk.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.gvMerk.Name = "gvMerk"
        Me.gvMerk.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.gvMerk.OptionsView.ShowGroupPanel = False
        '
        'LabelControl3
        '
        Me.LabelControl3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelControl3.Location = New System.Drawing.Point(701, 67)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(38, 13)
        Me.LabelControl3.TabIndex = 9
        Me.LabelControl3.Text = "Supplier"
        '
        'txtSupplier
        '
        Me.txtSupplier.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSupplier.EnterMoveNextControl = True
        Me.txtSupplier.Location = New System.Drawing.Point(745, 64)
        Me.txtSupplier.Name = "txtSupplier"
        Me.txtSupplier.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtSupplier.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSupplier.Properties.NullText = ""
        Me.txtSupplier.Properties.View = Me.gvSupplier
        Me.txtSupplier.Size = New System.Drawing.Size(226, 20)
        Me.txtSupplier.TabIndex = 10
        '
        'gvSupplier
        '
        Me.gvSupplier.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.gvSupplier.Name = "gvSupplier"
        Me.gvSupplier.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.gvSupplier.OptionsView.ShowGroupPanel = False
        '
        'LabelControl4
        '
        Me.LabelControl4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelControl4.Location = New System.Drawing.Point(699, 41)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Size = New System.Drawing.Size(40, 13)
        Me.LabelControl4.TabIndex = 7
        Me.LabelControl4.Text = "Kategori"
        '
        'txtKategori
        '
        Me.txtKategori.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtKategori.EnterMoveNextControl = True
        Me.txtKategori.Location = New System.Drawing.Point(745, 38)
        Me.txtKategori.Name = "txtKategori"
        Me.txtKategori.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtKategori.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtKategori.Properties.NullText = ""
        Me.txtKategori.Properties.View = Me.gvKategori
        Me.txtKategori.Size = New System.Drawing.Size(226, 20)
        Me.txtKategori.TabIndex = 8
        '
        'gvKategori
        '
        Me.gvKategori.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.gvKategori.Name = "gvKategori"
        Me.gvKategori.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.gvKategori.OptionsView.ShowGroupPanel = False
        '
        'LabelControl2
        '
        Me.LabelControl2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelControl2.Location = New System.Drawing.Point(688, 15)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(51, 13)
        Me.LabelControl2.TabIndex = 3
        Me.LabelControl2.Text = "Sampai Tgl"
        '
        'DateEdit1
        '
        Me.DateEdit1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DateEdit1.EditValue = New Date(2019, 8, 24, 21, 31, 53, 120)
        Me.DateEdit1.EnterMoveNextControl = True
        Me.DateEdit1.Location = New System.Drawing.Point(745, 12)
        Me.DateEdit1.MenuManager = Me.BarManager1
        Me.DateEdit1.Name = "DateEdit1"
        Me.DateEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DateEdit1.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.DateEdit1.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret
        Me.DateEdit1.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.DateEdit1.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.DateEdit1.Size = New System.Drawing.Size(100, 20)
        Me.DateEdit1.TabIndex = 4
        '
        'BarManager1
        '
        Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar2, Me.Bar4})
        Me.BarManager1.DockControls.Add(Me.barDockControlTop)
        Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.BarManager1.DockControls.Add(Me.barDockControlRight)
        Me.BarManager1.Form = Me
        Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarSubItem1, Me.mnSaveLayout, Me.mnHistoryHarga, Me.mnHitungUlangSaldo, Me.mnCetak, Me.mnRefresh, Me.mnTutup})
        Me.BarManager1.MainMenu = Me.Bar2
        Me.BarManager1.MaxItemId = 7
        '
        'Bar2
        '
        Me.Bar2.BarName = "Main menu"
        Me.Bar2.DockCol = 0
        Me.Bar2.DockRow = 0
        Me.Bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.Bar2.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarSubItem1)})
        Me.Bar2.OptionsBar.MultiLine = True
        Me.Bar2.OptionsBar.UseWholeRow = True
        Me.Bar2.Text = "Main menu"
        Me.Bar2.Visible = False
        '
        'BarSubItem1
        '
        Me.BarSubItem1.Caption = "File"
        Me.BarSubItem1.Id = 0
        Me.BarSubItem1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.mnSaveLayout)})
        Me.BarSubItem1.Name = "BarSubItem1"
        '
        'mnSaveLayout
        '
        Me.mnSaveLayout.Caption = "Save Layout"
        Me.mnSaveLayout.Id = 1
        Me.mnSaveLayout.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F10))
        Me.mnSaveLayout.Name = "mnSaveLayout"
        '
        'Bar4
        '
        Me.Bar4.BarName = "Custom 3"
        Me.Bar4.DockCol = 0
        Me.Bar4.DockRow = 1
        Me.Bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.Bar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.mnCetak), New DevExpress.XtraBars.LinkPersistInfo(Me.mnRefresh, True), New DevExpress.XtraBars.LinkPersistInfo(Me.mnTutup, True)})
        Me.Bar4.Text = "Custom 3"
        '
        'mnCetak
        '
        Me.mnCetak.Caption = "&Cetak (F8)"
        Me.mnCetak.Id = 4
        Me.mnCetak.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F8)
        Me.mnCetak.Name = "mnCetak"
        '
        'mnRefresh
        '
        Me.mnRefresh.Caption = "&Refresh (F5)"
        Me.mnRefresh.Id = 5
        Me.mnRefresh.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5)
        Me.mnRefresh.Name = "mnRefresh"
        '
        'mnTutup
        '
        Me.mnTutup.Caption = "&Tutup (F3)"
        Me.mnTutup.Id = 6
        Me.mnTutup.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F3)
        Me.mnTutup.Name = "mnTutup"
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Size = New System.Drawing.Size(983, 51)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 558)
        Me.barDockControlBottom.Size = New System.Drawing.Size(983, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 51)
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 507)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(983, 51)
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 507)
        '
        'mnHistoryHarga
        '
        Me.mnHistoryHarga.Caption = "History Harga"
        Me.mnHistoryHarga.Id = 2
        Me.mnHistoryHarga.Name = "mnHistoryHarga"
        '
        'mnHitungUlangSaldo
        '
        Me.mnHitungUlangSaldo.Caption = "Hitung Ulang Saldo"
        Me.mnHitungUlangSaldo.Id = 3
        Me.mnHitungUlangSaldo.Name = "mnHitungUlangSaldo"
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl1.Location = New System.Drawing.Point(12, 15)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(175, 25)
        Me.LabelControl1.TabIndex = 1
        Me.LabelControl1.Text = "Laporan Saldo Stok"
        '
        'ckTdkAktif
        '
        Me.ckTdkAktif.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ckTdkAktif.Location = New System.Drawing.Point(851, 12)
        Me.ckTdkAktif.Name = "ckTdkAktif"
        Me.ckTdkAktif.Properties.Caption = "Tampilkan Tdk Aktif"
        Me.ckTdkAktif.Size = New System.Drawing.Size(120, 19)
        Me.ckTdkAktif.TabIndex = 0
        '
        'GridControl1
        '
        Me.GridControl1.DataSource = Me.BindingSource1
        Me.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridControl1.Location = New System.Drawing.Point(0, 145)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.Size = New System.Drawing.Size(983, 362)
        Me.GridControl1.TabIndex = 2
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsBehavior.Editable = False
        Me.GridView1.OptionsNavigation.EnterMoveNextColumn = True
        Me.GridView1.OptionsSelection.MultiSelect = True
        Me.GridView1.OptionsView.ColumnAutoWidth = False
        Me.GridView1.OptionsView.ShowAutoFilterRow = True
        Me.GridView1.OptionsView.ShowFooter = True
        Me.GridView1.OptionsView.ShowGroupPanel = False
        '
        'PopupMenu1
        '
        Me.PopupMenu1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.mnHistoryHarga), New DevExpress.XtraBars.LinkPersistInfo(Me.mnHitungUlangSaldo)})
        Me.PopupMenu1.Manager = Me.BarManager1
        Me.PopupMenu1.Name = "PopupMenu1"
        '
        'Bar1
        '
        Me.Bar1.BarName = "Tools"
        Me.Bar1.DockCol = 0
        Me.Bar1.DockRow = 1
        Me.Bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.Bar1.Text = "Tools"
        '
        'Bar3
        '
        Me.Bar3.BarName = "Tools"
        Me.Bar3.DockCol = 0
        Me.Bar3.DockRow = 1
        Me.Bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.Bar3.Text = "Tools"
        '
        'frmLaporanSaldoStok
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(983, 558)
        Me.Controls.Add(Me.GridControl1)
        Me.Controls.Add(Me.PanelControl2)
        Me.Controls.Add(Me.PanelControl1)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Name = "frmLaporanSaldoStok"
        Me.Text = "Laporan Saldo Stok"
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl2.ResumeLayout(False)
        Me.PanelControl2.PerformLayout()
        CType(Me.txtMerk.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvMerk, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSupplier.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvSupplier, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKategori.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvKategori, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEdit1.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ckTdkAktif.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSource1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PopupMenu1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents cmdRefresh As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdTutup As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdCetak As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents ckTdkAktif As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents BindingSource1 As System.Windows.Forms.BindingSource
    Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
    Friend WithEvents Bar2 As DevExpress.XtraBars.Bar
    Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents BarSubItem1 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents mnSaveLayout As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents PopupMenu1 As DevExpress.XtraBars.PopupMenu
    Friend WithEvents mnHistoryHarga As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnHitungUlangSaldo As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents DateEdit1 As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtKategori As DevExpress.XtraEditors.SearchLookUpEdit
    Friend WithEvents gvKategori As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtSupplier As DevExpress.XtraEditors.SearchLookUpEdit
    Friend WithEvents gvSupplier As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LabelControl5 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtMerk As DevExpress.XtraEditors.SearchLookUpEdit
    Friend WithEvents gvMerk As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
    Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
    Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
    Friend WithEvents mnCetak As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnRefresh As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnTutup As DevExpress.XtraBars.BarButtonItem
End Class

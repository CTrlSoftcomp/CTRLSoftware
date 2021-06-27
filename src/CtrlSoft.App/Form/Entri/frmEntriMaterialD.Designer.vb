<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmEntriMaterialD
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.txtJumlah = New DevExpress.XtraEditors.TextEdit()
        Me.txtHargaPokok = New DevExpress.XtraEditors.TextEdit()
        Me.txtKonversi = New DevExpress.XtraEditors.TextEdit()
        Me.txtSatuan = New DevExpress.XtraEditors.SearchLookUpEdit()
        Me.gvSatuan = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.txtNamaBarang = New DevExpress.XtraEditors.TextEdit()
        Me.txtKodeBarang = New DevExpress.XtraEditors.TextEdit()
        Me.txtBarcode = New DevExpress.XtraEditors.SearchLookUpEdit()
        Me.gvBarcode = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.txtQty = New DevExpress.XtraEditors.TextEdit()
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlGroup2 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem16 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem10 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem11 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem13 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem14 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem15 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
        Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.Bar2 = New DevExpress.XtraBars.Bar()
        Me.BarSubItem1 = New DevExpress.XtraBars.BarSubItem()
        Me.mnSaveLayout = New DevExpress.XtraBars.BarButtonItem()
        Me.Bar1 = New DevExpress.XtraBars.Bar()
        Me.mnSimpan = New DevExpress.XtraBars.BarButtonItem()
        Me.mnTutup = New DevExpress.XtraBars.BarButtonItem()
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
        Me.mnRefresh = New DevExpress.XtraBars.BarButtonItem()
        Me.mnBaru = New DevExpress.XtraBars.BarButtonItem()
        Me.mnEdit = New DevExpress.XtraBars.BarButtonItem()
        Me.mnHapus = New DevExpress.XtraBars.BarButtonItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.txtJumlah.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtHargaPokok.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKonversi.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSatuan.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvSatuan, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtNamaBarang.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKodeBarang.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtBarcode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvBarcode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtQty.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem16, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem13, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem14, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem15, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.txtJumlah)
        Me.LayoutControl1.Controls.Add(Me.txtHargaPokok)
        Me.LayoutControl1.Controls.Add(Me.txtKonversi)
        Me.LayoutControl1.Controls.Add(Me.txtSatuan)
        Me.LayoutControl1.Controls.Add(Me.txtNamaBarang)
        Me.LayoutControl1.Controls.Add(Me.txtKodeBarang)
        Me.LayoutControl1.Controls.Add(Me.txtBarcode)
        Me.LayoutControl1.Controls.Add(Me.txtQty)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 51)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = New System.Drawing.Rectangle(941, 332, 442, 350)
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        Me.LayoutControl1.Size = New System.Drawing.Size(335, 291)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'txtJumlah
        '
        Me.txtJumlah.EditValue = New Decimal(New Integer() {0, 0, 0, 65536})
        Me.txtJumlah.EnterMoveNextControl = True
        Me.txtJumlah.Location = New System.Drawing.Point(91, 210)
        Me.txtJumlah.Name = "txtJumlah"
        Me.txtJumlah.Properties.Appearance.Options.UseTextOptions = True
        Me.txtJumlah.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.txtJumlah.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtJumlah.Properties.Mask.EditMask = "n2"
        Me.txtJumlah.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtJumlah.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtJumlah.Properties.ReadOnly = True
        Me.txtJumlah.Size = New System.Drawing.Size(220, 20)
        Me.txtJumlah.StyleController = Me.LayoutControl1
        Me.txtJumlah.TabIndex = 20
        '
        'txtHargaPokok
        '
        Me.txtHargaPokok.EditValue = New Decimal(New Integer() {0, 0, 0, 65536})
        Me.txtHargaPokok.EnterMoveNextControl = True
        Me.txtHargaPokok.Location = New System.Drawing.Point(91, 186)
        Me.txtHargaPokok.Name = "txtHargaPokok"
        Me.txtHargaPokok.Properties.Appearance.Options.UseTextOptions = True
        Me.txtHargaPokok.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.txtHargaPokok.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtHargaPokok.Properties.Mask.EditMask = "n2"
        Me.txtHargaPokok.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtHargaPokok.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtHargaPokok.Size = New System.Drawing.Size(220, 20)
        Me.txtHargaPokok.StyleController = Me.LayoutControl1
        Me.txtHargaPokok.TabIndex = 19
        '
        'txtKonversi
        '
        Me.txtKonversi.EditValue = New Decimal(New Integer() {0, 0, 0, 65536})
        Me.txtKonversi.EnterMoveNextControl = True
        Me.txtKonversi.Location = New System.Drawing.Point(91, 138)
        Me.txtKonversi.Name = "txtKonversi"
        Me.txtKonversi.Properties.Appearance.Options.UseTextOptions = True
        Me.txtKonversi.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.txtKonversi.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtKonversi.Properties.Mask.EditMask = "n0"
        Me.txtKonversi.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtKonversi.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtKonversi.Properties.ReadOnly = True
        Me.txtKonversi.Size = New System.Drawing.Size(220, 20)
        Me.txtKonversi.StyleController = Me.LayoutControl1
        Me.txtKonversi.TabIndex = 18
        '
        'txtSatuan
        '
        Me.txtSatuan.Location = New System.Drawing.Point(91, 114)
        Me.txtSatuan.Name = "txtSatuan"
        Me.txtSatuan.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtSatuan.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSatuan.Properties.NullText = ""
        Me.txtSatuan.Properties.ReadOnly = True
        Me.txtSatuan.Properties.View = Me.gvSatuan
        Me.txtSatuan.Size = New System.Drawing.Size(220, 20)
        Me.txtSatuan.StyleController = Me.LayoutControl1
        Me.txtSatuan.TabIndex = 17
        '
        'gvSatuan
        '
        Me.gvSatuan.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.gvSatuan.Name = "gvSatuan"
        Me.gvSatuan.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.gvSatuan.OptionsView.ShowGroupPanel = False
        '
        'txtNamaBarang
        '
        Me.txtNamaBarang.EnterMoveNextControl = True
        Me.txtNamaBarang.Location = New System.Drawing.Point(91, 90)
        Me.txtNamaBarang.Name = "txtNamaBarang"
        Me.txtNamaBarang.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNamaBarang.Properties.ReadOnly = True
        Me.txtNamaBarang.Size = New System.Drawing.Size(220, 20)
        Me.txtNamaBarang.StyleController = Me.LayoutControl1
        Me.txtNamaBarang.TabIndex = 16
        '
        'txtKodeBarang
        '
        Me.txtKodeBarang.EnterMoveNextControl = True
        Me.txtKodeBarang.Location = New System.Drawing.Point(91, 66)
        Me.txtKodeBarang.Name = "txtKodeBarang"
        Me.txtKodeBarang.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtKodeBarang.Properties.ReadOnly = True
        Me.txtKodeBarang.Size = New System.Drawing.Size(220, 20)
        Me.txtKodeBarang.StyleController = Me.LayoutControl1
        Me.txtKodeBarang.TabIndex = 15
        '
        'txtBarcode
        '
        Me.txtBarcode.Location = New System.Drawing.Point(91, 42)
        Me.txtBarcode.Name = "txtBarcode"
        Me.txtBarcode.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtBarcode.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtBarcode.Properties.NullText = ""
        Me.txtBarcode.Properties.View = Me.gvBarcode
        Me.txtBarcode.Size = New System.Drawing.Size(220, 20)
        Me.txtBarcode.StyleController = Me.LayoutControl1
        Me.txtBarcode.TabIndex = 14
        '
        'gvBarcode
        '
        Me.gvBarcode.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.gvBarcode.Name = "gvBarcode"
        Me.gvBarcode.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.gvBarcode.OptionsView.ShowGroupPanel = False
        '
        'txtQty
        '
        Me.txtQty.EditValue = New Decimal(New Integer() {0, 0, 0, 65536})
        Me.txtQty.EnterMoveNextControl = True
        Me.txtQty.Location = New System.Drawing.Point(91, 162)
        Me.txtQty.Name = "txtQty"
        Me.txtQty.Properties.Appearance.Options.UseTextOptions = True
        Me.txtQty.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.txtQty.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtQty.Properties.Mask.EditMask = "n0"
        Me.txtQty.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtQty.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtQty.Size = New System.Drawing.Size(220, 20)
        Me.txtQty.StyleController = Me.LayoutControl1
        Me.txtQty.TabIndex = 12
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.CustomizationFormText = "Root"
        Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup1.GroupBordersVisible = False
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup2})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "Root"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(335, 291)
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlGroup2
        '
        Me.LayoutControlGroup2.CustomizationFormText = "Informasi Personal"
        Me.LayoutControlGroup2.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem16, Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem10, Me.LayoutControlItem11, Me.LayoutControlItem13, Me.LayoutControlItem14, Me.LayoutControlItem15})
        Me.LayoutControlGroup2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup2.Name = "LayoutControlGroup2"
        Me.LayoutControlGroup2.Size = New System.Drawing.Size(315, 271)
        Me.LayoutControlGroup2.Text = "Informasi Barang Hasil / Jadi"
        '
        'LayoutControlItem16
        '
        Me.LayoutControlItem16.Control = Me.txtQty
        Me.LayoutControlItem16.CustomizationFormText = "Limit Hutang"
        Me.LayoutControlItem16.Location = New System.Drawing.Point(0, 120)
        Me.LayoutControlItem16.Name = "LayoutControlItem16"
        Me.LayoutControlItem16.Size = New System.Drawing.Size(291, 24)
        Me.LayoutControlItem16.Text = "Qty"
        Me.LayoutControlItem16.TextSize = New System.Drawing.Size(64, 13)
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.txtBarcode
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(291, 24)
        Me.LayoutControlItem1.Text = "Barcode"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(64, 13)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.txtKodeBarang
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 24)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(291, 24)
        Me.LayoutControlItem2.Text = "Kode Barang"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(64, 13)
        '
        'LayoutControlItem10
        '
        Me.LayoutControlItem10.Control = Me.txtNamaBarang
        Me.LayoutControlItem10.Location = New System.Drawing.Point(0, 48)
        Me.LayoutControlItem10.Name = "LayoutControlItem10"
        Me.LayoutControlItem10.Size = New System.Drawing.Size(291, 24)
        Me.LayoutControlItem10.Text = "Nama Barang"
        Me.LayoutControlItem10.TextSize = New System.Drawing.Size(64, 13)
        '
        'LayoutControlItem11
        '
        Me.LayoutControlItem11.Control = Me.txtSatuan
        Me.LayoutControlItem11.Location = New System.Drawing.Point(0, 72)
        Me.LayoutControlItem11.Name = "LayoutControlItem11"
        Me.LayoutControlItem11.Size = New System.Drawing.Size(291, 24)
        Me.LayoutControlItem11.Text = "Satuan"
        Me.LayoutControlItem11.TextSize = New System.Drawing.Size(64, 13)
        '
        'LayoutControlItem13
        '
        Me.LayoutControlItem13.Control = Me.txtKonversi
        Me.LayoutControlItem13.Location = New System.Drawing.Point(0, 96)
        Me.LayoutControlItem13.Name = "LayoutControlItem13"
        Me.LayoutControlItem13.Size = New System.Drawing.Size(291, 24)
        Me.LayoutControlItem13.Text = "Konversi"
        Me.LayoutControlItem13.TextSize = New System.Drawing.Size(64, 13)
        '
        'LayoutControlItem14
        '
        Me.LayoutControlItem14.Control = Me.txtHargaPokok
        Me.LayoutControlItem14.Location = New System.Drawing.Point(0, 144)
        Me.LayoutControlItem14.Name = "LayoutControlItem14"
        Me.LayoutControlItem14.Size = New System.Drawing.Size(291, 24)
        Me.LayoutControlItem14.Text = "Harga Pokok"
        Me.LayoutControlItem14.TextSize = New System.Drawing.Size(64, 13)
        '
        'LayoutControlItem15
        '
        Me.LayoutControlItem15.Control = Me.txtJumlah
        Me.LayoutControlItem15.Location = New System.Drawing.Point(0, 168)
        Me.LayoutControlItem15.Name = "LayoutControlItem15"
        Me.LayoutControlItem15.Size = New System.Drawing.Size(291, 61)
        Me.LayoutControlItem15.Text = "Nilai"
        Me.LayoutControlItem15.TextSize = New System.Drawing.Size(64, 13)
        '
        'DxErrorProvider1
        '
        Me.DxErrorProvider1.ContainerControl = Me
        '
        'BarManager1
        '
        Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar2, Me.Bar1})
        Me.BarManager1.DockControls.Add(Me.barDockControlTop)
        Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.BarManager1.DockControls.Add(Me.barDockControlRight)
        Me.BarManager1.Form = Me
        Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarSubItem1, Me.mnSaveLayout, Me.mnSimpan, Me.mnTutup, Me.mnRefresh, Me.mnBaru, Me.mnEdit, Me.mnHapus})
        Me.BarManager1.MainMenu = Me.Bar2
        Me.BarManager1.MaxItemId = 8
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
        Me.mnSaveLayout.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F10)
        Me.mnSaveLayout.Name = "mnSaveLayout"
        '
        'Bar1
        '
        Me.Bar1.BarName = "Custom 3"
        Me.Bar1.DockCol = 0
        Me.Bar1.DockRow = 1
        Me.Bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.Bar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.mnSimpan, True), New DevExpress.XtraBars.LinkPersistInfo(Me.mnTutup)})
        Me.Bar1.Text = "Custom 3"
        '
        'mnSimpan
        '
        Me.mnSimpan.Caption = "&Simpan (F6)"
        Me.mnSimpan.Id = 2
        Me.mnSimpan.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F6)
        Me.mnSimpan.Name = "mnSimpan"
        '
        'mnTutup
        '
        Me.mnTutup.Caption = "&Tutup (F3)"
        Me.mnTutup.Id = 3
        Me.mnTutup.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F3)
        Me.mnTutup.Name = "mnTutup"
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Manager = Me.BarManager1
        Me.barDockControlTop.Size = New System.Drawing.Size(335, 51)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 342)
        Me.barDockControlBottom.Manager = Me.BarManager1
        Me.barDockControlBottom.Size = New System.Drawing.Size(335, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 51)
        Me.barDockControlLeft.Manager = Me.BarManager1
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 291)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(335, 51)
        Me.barDockControlRight.Manager = Me.BarManager1
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 291)
        '
        'mnRefresh
        '
        Me.mnRefresh.Caption = "&Reset (F5)"
        Me.mnRefresh.Id = 4
        Me.mnRefresh.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5)
        Me.mnRefresh.Name = "mnRefresh"
        '
        'mnBaru
        '
        Me.mnBaru.Caption = "&Baru (F1)"
        Me.mnBaru.Id = 5
        Me.mnBaru.Name = "mnBaru"
        Me.mnBaru.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        '
        'mnEdit
        '
        Me.mnEdit.Caption = "&Edit (F2)"
        Me.mnEdit.Id = 6
        Me.mnEdit.Name = "mnEdit"
        Me.mnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        '
        'mnHapus
        '
        Me.mnHapus.Caption = "&Hapus (F4)"
        Me.mnHapus.Id = 7
        Me.mnHapus.Name = "mnHapus"
        Me.mnHapus.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        '
        'frmEntriMaterialD
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(335, 342)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Name = "frmEntriMaterialD"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Entri Bahan Material / Formula"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.txtJumlah.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtHargaPokok.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKonversi.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSatuan.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvSatuan, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtNamaBarang.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKodeBarang.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtBarcode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvBarcode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtQty.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem16, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem13, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem14, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem15, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
    Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
    Friend WithEvents Bar2 As DevExpress.XtraBars.Bar
    Friend WithEvents BarSubItem1 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents mnSaveLayout As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents txtQty As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem16 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
    Friend WithEvents mnSimpan As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnTutup As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents txtJumlah As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtHargaPokok As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtKonversi As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtSatuan As DevExpress.XtraEditors.SearchLookUpEdit
    Friend WithEvents gvSatuan As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents txtNamaBarang As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtKodeBarang As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtBarcode As DevExpress.XtraEditors.SearchLookUpEdit
    Friend WithEvents gvBarcode As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem10 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem11 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem13 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem14 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem15 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents mnRefresh As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnBaru As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnEdit As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnHapus As DevExpress.XtraBars.BarButtonItem
End Class

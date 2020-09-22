<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmEntriSaldoAwalHutang
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
        Me.txtCatatan = New DevExpress.XtraEditors.MemoEdit()
        Me.txtTanggal = New DevExpress.XtraEditors.DateEdit()
        Me.txtKode = New DevExpress.XtraEditors.TextEdit()
        Me.txtKontak = New DevExpress.XtraEditors.SearchLookUpEdit()
        Me.gvKontak = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.txtAlamat = New DevExpress.XtraEditors.TextEdit()
        Me.txtNamaAlamat = New DevExpress.XtraEditors.TextEdit()
        Me.txtJumlah = New DevExpress.XtraEditors.TextEdit()
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem14 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem15 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
        Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.Bar2 = New DevExpress.XtraBars.Bar()
        Me.BarSubItem1 = New DevExpress.XtraBars.BarSubItem()
        Me.mnSaveLayout = New DevExpress.XtraBars.BarButtonItem()
        Me.Bar1 = New DevExpress.XtraBars.Bar()
        Me.mnSimpanBaru = New DevExpress.XtraBars.BarButtonItem()
        Me.mnSimpanTutup = New DevExpress.XtraBars.BarButtonItem()
        Me.mnTutup = New DevExpress.XtraBars.BarButtonItem()
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
        Me.txtJatuhTempo = New DevExpress.XtraEditors.DateEdit()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.txtJT = New DevExpress.XtraEditors.TextEdit()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.txtKodeReff = New DevExpress.XtraEditors.TextEdit()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.txtCatatan.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTanggal.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTanggal.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKontak.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvKontak, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtAlamat.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtNamaAlamat.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtJumlah.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem14, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem15, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtJatuhTempo.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtJatuhTempo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtJT.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKodeReff.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.txtKodeReff)
        Me.LayoutControl1.Controls.Add(Me.txtJT)
        Me.LayoutControl1.Controls.Add(Me.txtJatuhTempo)
        Me.LayoutControl1.Controls.Add(Me.txtCatatan)
        Me.LayoutControl1.Controls.Add(Me.txtTanggal)
        Me.LayoutControl1.Controls.Add(Me.txtKode)
        Me.LayoutControl1.Controls.Add(Me.txtKontak)
        Me.LayoutControl1.Controls.Add(Me.txtAlamat)
        Me.LayoutControl1.Controls.Add(Me.txtNamaAlamat)
        Me.LayoutControl1.Controls.Add(Me.txtJumlah)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 51)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = New System.Drawing.Rectangle(736, 236, 250, 350)
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        Me.LayoutControl1.Size = New System.Drawing.Size(413, 355)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'txtCatatan
        '
        Me.txtCatatan.Location = New System.Drawing.Point(12, 220)
        Me.txtCatatan.Name = "txtCatatan"
        Me.txtCatatan.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCatatan.Size = New System.Drawing.Size(389, 123)
        Me.txtCatatan.StyleController = Me.LayoutControl1
        Me.txtCatatan.TabIndex = 14
        '
        'txtTanggal
        '
        Me.txtTanggal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTanggal.EditValue = New Date(2019, 8, 24, 21, 31, 53, 120)
        Me.txtTanggal.EnterMoveNextControl = True
        Me.txtTanggal.Location = New System.Drawing.Point(77, 36)
        Me.txtTanggal.Name = "txtTanggal"
        Me.txtTanggal.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtTanggal.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.txtTanggal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret
        Me.txtTanggal.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtTanggal.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.txtTanggal.Size = New System.Drawing.Size(127, 20)
        Me.txtTanggal.StyleController = Me.LayoutControl1
        Me.txtTanggal.TabIndex = 5
        '
        'txtKode
        '
        Me.txtKode.EnterMoveNextControl = True
        Me.txtKode.Location = New System.Drawing.Point(77, 12)
        Me.txtKode.Name = "txtKode"
        Me.txtKode.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtKode.Size = New System.Drawing.Size(324, 20)
        Me.txtKode.StyleController = Me.LayoutControl1
        Me.txtKode.TabIndex = 11
        '
        'txtKontak
        '
        Me.txtKontak.EnterMoveNextControl = True
        Me.txtKontak.Location = New System.Drawing.Point(77, 108)
        Me.txtKontak.Name = "txtKontak"
        Me.txtKontak.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtKontak.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtKontak.Properties.NullText = ""
        Me.txtKontak.Properties.View = Me.gvKontak
        Me.txtKontak.Size = New System.Drawing.Size(324, 20)
        Me.txtKontak.StyleController = Me.LayoutControl1
        Me.txtKontak.TabIndex = 16
        '
        'gvKontak
        '
        Me.gvKontak.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.gvKontak.Name = "gvKontak"
        Me.gvKontak.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.gvKontak.OptionsView.ShowGroupPanel = False
        '
        'txtAlamat
        '
        Me.txtAlamat.EnterMoveNextControl = True
        Me.txtAlamat.Location = New System.Drawing.Point(77, 156)
        Me.txtAlamat.Name = "txtAlamat"
        Me.txtAlamat.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAlamat.Properties.ReadOnly = True
        Me.txtAlamat.Size = New System.Drawing.Size(324, 20)
        Me.txtAlamat.StyleController = Me.LayoutControl1
        Me.txtAlamat.TabIndex = 12
        '
        'txtNamaAlamat
        '
        Me.txtNamaAlamat.EnterMoveNextControl = True
        Me.txtNamaAlamat.Location = New System.Drawing.Point(77, 132)
        Me.txtNamaAlamat.Name = "txtNamaAlamat"
        Me.txtNamaAlamat.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNamaAlamat.Properties.ReadOnly = True
        Me.txtNamaAlamat.Size = New System.Drawing.Size(324, 20)
        Me.txtNamaAlamat.StyleController = Me.LayoutControl1
        Me.txtNamaAlamat.TabIndex = 12
        '
        'txtJumlah
        '
        Me.txtJumlah.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtJumlah.EnterMoveNextControl = True
        Me.txtJumlah.Location = New System.Drawing.Point(77, 180)
        Me.txtJumlah.Name = "txtJumlah"
        Me.txtJumlah.Properties.Appearance.Options.UseTextOptions = True
        Me.txtJumlah.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.txtJumlah.Properties.Mask.EditMask = "n2"
        Me.txtJumlah.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtJumlah.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtJumlah.Size = New System.Drawing.Size(324, 20)
        Me.txtJumlah.StyleController = Me.LayoutControl1
        Me.txtJumlah.TabIndex = 17
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.CustomizationFormText = "Root"
        Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup1.GroupBordersVisible = False
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem5, Me.LayoutControlItem14, Me.LayoutControlItem15, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem1, Me.LayoutControlItem7, Me.LayoutControlItem4, Me.LayoutControlItem6, Me.LayoutControlItem8})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "Root"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(413, 355)
        Me.LayoutControlGroup1.Text = "Root"
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.txtNamaAlamat
        Me.LayoutControlItem5.CustomizationFormText = "Kode Barang"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 120)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(393, 24)
        Me.LayoutControlItem5.Text = "Nama"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(62, 13)
        '
        'LayoutControlItem14
        '
        Me.LayoutControlItem14.Control = Me.txtAlamat
        Me.LayoutControlItem14.CustomizationFormText = "Nama Barang"
        Me.LayoutControlItem14.Location = New System.Drawing.Point(0, 144)
        Me.LayoutControlItem14.Name = "LayoutControlItem14"
        Me.LayoutControlItem14.Size = New System.Drawing.Size(393, 24)
        Me.LayoutControlItem14.Text = "Alamat"
        Me.LayoutControlItem14.TextSize = New System.Drawing.Size(62, 13)
        '
        'LayoutControlItem15
        '
        Me.LayoutControlItem15.Control = Me.txtJumlah
        Me.LayoutControlItem15.CustomizationFormText = "Jumlah"
        Me.LayoutControlItem15.Location = New System.Drawing.Point(0, 168)
        Me.LayoutControlItem15.Name = "LayoutControlItem15"
        Me.LayoutControlItem15.Size = New System.Drawing.Size(393, 24)
        Me.LayoutControlItem15.Text = "Jumlah"
        Me.LayoutControlItem15.TextSize = New System.Drawing.Size(62, 13)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.txtKode
        Me.LayoutControlItem2.CustomizationFormText = "Kode"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(393, 24)
        Me.LayoutControlItem2.Text = "Kode"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(62, 13)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.txtTanggal
        Me.LayoutControlItem3.CustomizationFormText = "Tanggal"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 24)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(196, 24)
        Me.LayoutControlItem3.Text = "Tanggal"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(62, 13)
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.txtKontak
        Me.LayoutControlItem1.CustomizationFormText = "Gudang"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 96)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(393, 24)
        Me.LayoutControlItem1.Text = "Kontak"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(62, 13)
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.txtCatatan
        Me.LayoutControlItem7.CustomizationFormText = "Keterangan"
        Me.LayoutControlItem7.Location = New System.Drawing.Point(0, 192)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(393, 143)
        Me.LayoutControlItem7.Text = "Keterangan"
        Me.LayoutControlItem7.TextLocation = DevExpress.Utils.Locations.Top
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(62, 13)
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
        Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarSubItem1, Me.mnSaveLayout, Me.mnSimpanBaru, Me.mnSimpanTutup, Me.mnTutup})
        Me.BarManager1.MainMenu = Me.Bar2
        Me.BarManager1.MaxItemId = 5
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
        Me.Bar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.mnSimpanBaru), New DevExpress.XtraBars.LinkPersistInfo(Me.mnSimpanTutup, True), New DevExpress.XtraBars.LinkPersistInfo(Me.mnTutup, True)})
        Me.Bar1.Text = "Custom 3"
        '
        'mnSimpanBaru
        '
        Me.mnSimpanBaru.Caption = "&Simpan + Entri Baru (F6)"
        Me.mnSimpanBaru.Id = 2
        Me.mnSimpanBaru.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F6)
        Me.mnSimpanBaru.Name = "mnSimpanBaru"
        '
        'mnSimpanTutup
        '
        Me.mnSimpanTutup.Caption = "S&impan + Tutup (F2)"
        Me.mnSimpanTutup.Id = 3
        Me.mnSimpanTutup.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F2)
        Me.mnSimpanTutup.Name = "mnSimpanTutup"
        '
        'mnTutup
        '
        Me.mnTutup.Caption = "&Tutup (F3)"
        Me.mnTutup.Id = 4
        Me.mnTutup.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F3)
        Me.mnTutup.Name = "mnTutup"
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Size = New System.Drawing.Size(413, 51)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 406)
        Me.barDockControlBottom.Size = New System.Drawing.Size(413, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 51)
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 355)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(413, 51)
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 355)
        '
        'txtJatuhTempo
        '
        Me.txtJatuhTempo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtJatuhTempo.EditValue = New Date(2019, 8, 24, 21, 31, 53, 120)
        Me.txtJatuhTempo.EnterMoveNextControl = True
        Me.txtJatuhTempo.Location = New System.Drawing.Point(77, 60)
        Me.txtJatuhTempo.Name = "txtJatuhTempo"
        Me.txtJatuhTempo.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtJatuhTempo.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.txtJatuhTempo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret
        Me.txtJatuhTempo.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtJatuhTempo.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.txtJatuhTempo.Size = New System.Drawing.Size(324, 20)
        Me.txtJatuhTempo.StyleController = Me.LayoutControl1
        Me.txtJatuhTempo.TabIndex = 6
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.txtJatuhTempo
        Me.LayoutControlItem4.CustomizationFormText = "Jatuh Tempo"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 48)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(393, 24)
        Me.LayoutControlItem4.Text = "Jatuh Tempo"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(62, 13)
        '
        'txtJT
        '
        Me.txtJT.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtJT.EnterMoveNextControl = True
        Me.txtJT.Location = New System.Drawing.Point(273, 36)
        Me.txtJT.Name = "txtJT"
        Me.txtJT.Properties.Appearance.Options.UseTextOptions = True
        Me.txtJT.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.txtJT.Properties.Mask.EditMask = "n0"
        Me.txtJT.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtJT.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtJT.Size = New System.Drawing.Size(128, 20)
        Me.txtJT.StyleController = Me.LayoutControl1
        Me.txtJT.TabIndex = 18
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.txtJT
        Me.LayoutControlItem6.CustomizationFormText = "JT Hari"
        Me.LayoutControlItem6.Location = New System.Drawing.Point(196, 24)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(197, 24)
        Me.LayoutControlItem6.Text = "JT Hari"
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(62, 13)
        '
        'txtKodeReff
        '
        Me.txtKodeReff.EnterMoveNextControl = True
        Me.txtKodeReff.Location = New System.Drawing.Point(77, 84)
        Me.txtKodeReff.Name = "txtKodeReff"
        Me.txtKodeReff.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtKodeReff.Size = New System.Drawing.Size(324, 20)
        Me.txtKodeReff.StyleController = Me.LayoutControl1
        Me.txtKodeReff.TabIndex = 12
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.Control = Me.txtKodeReff
        Me.LayoutControlItem8.CustomizationFormText = "No Reff"
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 72)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(393, 24)
        Me.LayoutControlItem8.Text = "No Reff"
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(62, 13)
        '
        'frmEntriSaldoAwalHutang
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(413, 406)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Name = "frmEntriSaldoAwalHutang"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Entri Item Saldo Awal Hutang"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.txtCatatan.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTanggal.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTanggal.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKontak.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvKontak, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtAlamat.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtNamaAlamat.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtJumlah.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem14, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem15, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtJatuhTempo.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtJatuhTempo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtJT.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKodeReff.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

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
    Friend WithEvents txtAlamat As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtNamaAlamat As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem14 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem15 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtJumlah As DevExpress.XtraEditors.TextEdit
    Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
    Friend WithEvents mnSimpanBaru As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnSimpanTutup As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnTutup As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents txtKontak As DevExpress.XtraEditors.SearchLookUpEdit
    Friend WithEvents gvKontak As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtKode As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtTanggal As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtCatatan As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtJatuhTempo As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtJT As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtKodeReff As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
End Class

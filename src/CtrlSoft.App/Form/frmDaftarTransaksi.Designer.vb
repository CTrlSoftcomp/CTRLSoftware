﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDaftarTransaksi
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
        Me.components = New System.ComponentModel.Container()
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        Me.cmdCetak = New DevExpress.XtraEditors.SimpleButton()
        Me.cmdHapus = New DevExpress.XtraEditors.SimpleButton()
        Me.cmdEdit = New DevExpress.XtraEditors.SimpleButton()
        Me.cmdBaru = New DevExpress.XtraEditors.SimpleButton()
        Me.cmdRefresh = New DevExpress.XtraEditors.SimpleButton()
        Me.cmdTutup = New DevExpress.XtraEditors.SimpleButton()
        Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl()
        Me.DateEdit2 = New DevExpress.XtraEditors.DateEdit()
        Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.Bar2 = New DevExpress.XtraBars.Bar()
        Me.BarSubItem1 = New DevExpress.XtraBars.BarSubItem()
        Me.mnSaveLayout = New DevExpress.XtraBars.BarButtonItem()
        Me.Bar1 = New DevExpress.XtraBars.Bar()
        Me.mnBaru = New DevExpress.XtraBars.BarButtonItem()
        Me.mnEdit = New DevExpress.XtraBars.BarButtonItem()
        Me.mnHapus = New DevExpress.XtraBars.BarButtonItem()
        Me.mnPreview = New DevExpress.XtraBars.BarButtonItem()
        Me.mnRefresh = New DevExpress.XtraBars.BarButtonItem()
        Me.mnTutup = New DevExpress.XtraBars.BarButtonItem()
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
        Me.mnPosting = New DevExpress.XtraBars.BarButtonItem()
        Me.mnUnposting = New DevExpress.XtraBars.BarButtonItem()
        Me.mnHasilPosting = New DevExpress.XtraBars.BarButtonItem()
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        Me.DateEdit1 = New DevExpress.XtraEditors.DateEdit()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.BindingSource1 = New System.Windows.Forms.BindingSource(Me.components)
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.PopupMenu1 = New DevExpress.XtraBars.PopupMenu(Me.components)
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl2.SuspendLayout()
        CType(Me.DateEdit2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEdit2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEdit1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PopupMenu1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelControl1
        '
        Me.PanelControl1.Controls.Add(Me.cmdCetak)
        Me.PanelControl1.Controls.Add(Me.cmdHapus)
        Me.PanelControl1.Controls.Add(Me.cmdEdit)
        Me.PanelControl1.Controls.Add(Me.cmdBaru)
        Me.PanelControl1.Controls.Add(Me.cmdRefresh)
        Me.PanelControl1.Controls.Add(Me.cmdTutup)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelControl1.Location = New System.Drawing.Point(0, 507)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(1069, 51)
        Me.PanelControl1.TabIndex = 0
        Me.PanelControl1.Visible = False
        '
        'cmdCetak
        '
        Me.cmdCetak.Appearance.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCetak.Appearance.Options.UseFont = True
        Me.cmdCetak.ImageOptions.ImageIndex = 6
        Me.cmdCetak.Location = New System.Drawing.Point(390, 6)
        Me.cmdCetak.Name = "cmdCetak"
        Me.cmdCetak.Size = New System.Drawing.Size(120, 38)
        Me.cmdCetak.TabIndex = 3
        Me.cmdCetak.Text = "&Cetak"
        '
        'cmdHapus
        '
        Me.cmdHapus.Appearance.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHapus.Appearance.Options.UseFont = True
        Me.cmdHapus.ImageOptions.ImageIndex = 2
        Me.cmdHapus.Location = New System.Drawing.Point(264, 6)
        Me.cmdHapus.Name = "cmdHapus"
        Me.cmdHapus.Size = New System.Drawing.Size(120, 38)
        Me.cmdHapus.TabIndex = 2
        Me.cmdHapus.Text = "&Hapus"
        '
        'cmdEdit
        '
        Me.cmdEdit.Appearance.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.Appearance.Options.UseFont = True
        Me.cmdEdit.ImageOptions.ImageIndex = 1
        Me.cmdEdit.Location = New System.Drawing.Point(138, 6)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.Size = New System.Drawing.Size(120, 38)
        Me.cmdEdit.TabIndex = 1
        Me.cmdEdit.Text = "&Edit"
        '
        'cmdBaru
        '
        Me.cmdBaru.Appearance.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBaru.Appearance.Options.UseFont = True
        Me.cmdBaru.ImageOptions.ImageIndex = 0
        Me.cmdBaru.Location = New System.Drawing.Point(12, 6)
        Me.cmdBaru.Name = "cmdBaru"
        Me.cmdBaru.Size = New System.Drawing.Size(120, 38)
        Me.cmdBaru.TabIndex = 0
        Me.cmdBaru.Text = "&Baru"
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdRefresh.Appearance.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRefresh.Appearance.Options.UseFont = True
        Me.cmdRefresh.ImageOptions.ImageIndex = 3
        Me.cmdRefresh.Location = New System.Drawing.Point(811, 6)
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
        Me.cmdTutup.ImageOptions.ImageIndex = 5
        Me.cmdTutup.Location = New System.Drawing.Point(937, 6)
        Me.cmdTutup.Name = "cmdTutup"
        Me.cmdTutup.Size = New System.Drawing.Size(120, 38)
        Me.cmdTutup.TabIndex = 5
        Me.cmdTutup.Text = "&Tutup"
        '
        'PanelControl2
        '
        Me.PanelControl2.Controls.Add(Me.DateEdit2)
        Me.PanelControl2.Controls.Add(Me.LabelControl3)
        Me.PanelControl2.Controls.Add(Me.LabelControl2)
        Me.PanelControl2.Controls.Add(Me.DateEdit1)
        Me.PanelControl2.Controls.Add(Me.LabelControl1)
        Me.PanelControl2.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelControl2.Location = New System.Drawing.Point(0, 51)
        Me.PanelControl2.Name = "PanelControl2"
        Me.PanelControl2.Size = New System.Drawing.Size(1069, 40)
        Me.PanelControl2.TabIndex = 1
        '
        'DateEdit2
        '
        Me.DateEdit2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DateEdit2.EditValue = New Date(2019, 8, 24, 21, 31, 53, 120)
        Me.DateEdit2.EnterMoveNextControl = True
        Me.DateEdit2.Location = New System.Drawing.Point(957, 15)
        Me.DateEdit2.MenuManager = Me.BarManager1
        Me.DateEdit2.Name = "DateEdit2"
        Me.DateEdit2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DateEdit2.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.DateEdit2.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.DateEdit2.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret
        Me.DateEdit2.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.DateEdit2.Size = New System.Drawing.Size(100, 20)
        Me.DateEdit2.TabIndex = 5
        '
        'BarManager1
        '
        Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar2, Me.Bar1})
        Me.BarManager1.DockControls.Add(Me.barDockControlTop)
        Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.BarManager1.DockControls.Add(Me.barDockControlRight)
        Me.BarManager1.Form = Me
        Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarSubItem1, Me.mnSaveLayout, Me.mnPosting, Me.mnUnposting, Me.mnHasilPosting, Me.mnBaru, Me.mnEdit, Me.mnHapus, Me.mnPreview, Me.mnRefresh, Me.mnTutup})
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
        Me.mnSaveLayout.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F10))
        Me.mnSaveLayout.Name = "mnSaveLayout"
        '
        'Bar1
        '
        Me.Bar1.BarName = "Custom 3"
        Me.Bar1.DockCol = 0
        Me.Bar1.DockRow = 1
        Me.Bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.Bar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.mnBaru), New DevExpress.XtraBars.LinkPersistInfo(Me.mnEdit), New DevExpress.XtraBars.LinkPersistInfo(Me.mnHapus), New DevExpress.XtraBars.LinkPersistInfo(Me.mnPreview, True), New DevExpress.XtraBars.LinkPersistInfo(Me.mnRefresh, True), New DevExpress.XtraBars.LinkPersistInfo(Me.mnTutup, True)})
        Me.Bar1.Text = "Custom 3"
        '
        'mnBaru
        '
        Me.mnBaru.Caption = "&Baru (F1)"
        Me.mnBaru.Id = 2
        Me.mnBaru.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F1)
        Me.mnBaru.Name = "mnBaru"
        '
        'mnEdit
        '
        Me.mnEdit.Caption = "&Edit (F2)"
        Me.mnEdit.Id = 3
        Me.mnEdit.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F2)
        Me.mnEdit.Name = "mnEdit"
        '
        'mnHapus
        '
        Me.mnHapus.Caption = "&Hapus (F4)"
        Me.mnHapus.Id = 4
        Me.mnHapus.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F4)
        Me.mnHapus.Name = "mnHapus"
        '
        'mnPreview
        '
        Me.mnPreview.Caption = "&Preview (F8)"
        Me.mnPreview.Id = 5
        Me.mnPreview.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F8)
        Me.mnPreview.Name = "mnPreview"
        '
        'mnRefresh
        '
        Me.mnRefresh.Caption = "&Refresh (F5)"
        Me.mnRefresh.Id = 6
        Me.mnRefresh.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5)
        Me.mnRefresh.Name = "mnRefresh"
        '
        'mnTutup
        '
        Me.mnTutup.Caption = "&Tutup (F3)"
        Me.mnTutup.Id = 7
        Me.mnTutup.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F3)
        Me.mnTutup.Name = "mnTutup"
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Manager = Me.BarManager1
        Me.barDockControlTop.Size = New System.Drawing.Size(1069, 51)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 558)
        Me.barDockControlBottom.Manager = Me.BarManager1
        Me.barDockControlBottom.Size = New System.Drawing.Size(1069, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 51)
        Me.barDockControlLeft.Manager = Me.BarManager1
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 507)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(1069, 51)
        Me.barDockControlRight.Manager = Me.BarManager1
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 507)
        '
        'mnPosting
        '
        Me.mnPosting.Caption = "&Posting"
        Me.mnPosting.Id = 2
        Me.mnPosting.Name = "mnPosting"
        '
        'mnUnposting
        '
        Me.mnUnposting.Caption = "&Unposting"
        Me.mnUnposting.Id = 3
        Me.mnUnposting.Name = "mnUnposting"
        '
        'mnHasilPosting
        '
        Me.mnHasilPosting.Caption = "&Hasil Posting"
        Me.mnHasilPosting.Id = 4
        Me.mnHasilPosting.Name = "mnHasilPosting"
        '
        'LabelControl3
        '
        Me.LabelControl3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelControl3.Location = New System.Drawing.Point(936, 19)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(15, 13)
        Me.LabelControl3.TabIndex = 4
        Me.LabelControl3.Text = "s/d"
        '
        'LabelControl2
        '
        Me.LabelControl2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelControl2.Location = New System.Drawing.Point(772, 19)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(36, 13)
        Me.LabelControl2.TabIndex = 3
        Me.LabelControl2.Text = "Periode"
        '
        'DateEdit1
        '
        Me.DateEdit1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DateEdit1.EditValue = New Date(2019, 8, 24, 21, 31, 53, 120)
        Me.DateEdit1.EnterMoveNextControl = True
        Me.DateEdit1.Location = New System.Drawing.Point(830, 15)
        Me.DateEdit1.MenuManager = Me.BarManager1
        Me.DateEdit1.Name = "DateEdit1"
        Me.DateEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DateEdit1.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.DateEdit1.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.DateEdit1.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret
        Me.DateEdit1.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.DateEdit1.Size = New System.Drawing.Size(100, 20)
        Me.DateEdit1.TabIndex = 2
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl1.Appearance.Options.UseFont = True
        Me.LabelControl1.Location = New System.Drawing.Point(12, 9)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(56, 25)
        Me.LabelControl1.TabIndex = 1
        Me.LabelControl1.Text = "Daftar"
        '
        'GridControl1
        '
        Me.GridControl1.DataSource = Me.BindingSource1
        Me.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridControl1.Location = New System.Drawing.Point(0, 91)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.Size = New System.Drawing.Size(1069, 416)
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
        Me.PopupMenu1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.mnPosting), New DevExpress.XtraBars.LinkPersistInfo(Me.mnUnposting), New DevExpress.XtraBars.LinkPersistInfo(Me.mnHasilPosting, True)})
        Me.PopupMenu1.Manager = Me.BarManager1
        Me.PopupMenu1.Name = "PopupMenu1"
        '
        'frmDaftarTransaksi
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1069, 558)
        Me.Controls.Add(Me.GridControl1)
        Me.Controls.Add(Me.PanelControl2)
        Me.Controls.Add(Me.PanelControl1)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Name = "frmDaftarTransaksi"
        Me.Text = "Daftar Transaksi"
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl2.ResumeLayout(False)
        Me.PanelControl2.PerformLayout()
        CType(Me.DateEdit2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEdit2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEdit1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSource1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PopupMenu1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents cmdRefresh As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdTutup As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdHapus As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdEdit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdBaru As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdCetak As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
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
    Friend WithEvents DateEdit2 As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents DateEdit1 As DevExpress.XtraEditors.DateEdit
    Friend WithEvents mnPosting As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnUnposting As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnHasilPosting As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents PopupMenu1 As DevExpress.XtraBars.PopupMenu
    Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
    Friend WithEvents mnBaru As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnEdit As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnHapus As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnPreview As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnRefresh As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnTutup As DevExpress.XtraBars.BarButtonItem
End Class

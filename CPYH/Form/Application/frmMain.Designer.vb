<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits DevExpress.XtraBars.Ribbon.RibbonForm

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.RibbonControl = New DevExpress.XtraBars.Ribbon.RibbonControl
        Me.ApplicationMenu1 = New DevExpress.XtraBars.Ribbon.ApplicationMenu(Me.components)
        Me.barSetting = New DevExpress.XtraBars.BarButtonItem
        Me.barLoginOut = New DevExpress.XtraBars.BarButtonItem
        Me.barExit = New DevExpress.XtraBars.BarButtonItem
        Me.ImageCollectionSmall = New DevExpress.Utils.ImageCollection(Me.components)
        Me.rgbiSkins = New DevExpress.XtraBars.RibbonGalleryBarItem
        Me.barStaticUID = New DevExpress.XtraBars.BarStaticItem
        Me.barStaticJam = New DevExpress.XtraBars.BarStaticItem
        Me.barSettingPerusahaan = New DevExpress.XtraBars.BarButtonItem
        Me.barEditReport = New DevExpress.XtraBars.BarEditItem
        Me.RepositoryItemCheckEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        Me.barManagementUser = New DevExpress.XtraBars.BarButtonItem
        Me.barManagementRole = New DevExpress.XtraBars.BarButtonItem
        Me.ImageCollectionLarge = New DevExpress.Utils.ImageCollection(Me.components)
        Me.RibbonPageCategory1 = New DevExpress.XtraBars.Ribbon.RibbonPageCategory
        Me.RibbonPage6 = New DevExpress.XtraBars.Ribbon.RibbonPage
        Me.RibbonPageGroup2 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup
        Me.RibbonPageGroup5 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup
        Me.RibbonStatusBar = New DevExpress.XtraBars.Ribbon.RibbonStatusBar
        Me.XtraTabbedMdiManager1 = New DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(Me.components)
        Me.defaultLookAndFeel1 = New DevExpress.LookAndFeel.DefaultLookAndFeel(Me.components)
        Me.BarButtonItem1 = New DevExpress.XtraBars.BarButtonItem
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.ICButtons = New DevExpress.Utils.ImageCollection(Me.components)
        CType(Me.RibbonControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ApplicationMenu1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ImageCollectionSmall, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ImageCollectionLarge, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.XtraTabbedMdiManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ICButtons, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RibbonControl
        '
        Me.RibbonControl.ApplicationButtonDropDownControl = Me.ApplicationMenu1
        Me.RibbonControl.ApplicationButtonText = Nothing
        '
        '
        '
        Me.RibbonControl.ExpandCollapseItem.Id = 0
        Me.RibbonControl.ExpandCollapseItem.Name = ""
        Me.RibbonControl.Images = Me.ImageCollectionSmall
        Me.RibbonControl.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.RibbonControl.ExpandCollapseItem, Me.rgbiSkins, Me.barStaticUID, Me.barStaticJam, Me.barLoginOut, Me.barSetting, Me.barExit, Me.barSettingPerusahaan, Me.barEditReport, Me.barManagementUser, Me.barManagementRole})
        Me.RibbonControl.LargeImages = Me.ImageCollectionLarge
        Me.RibbonControl.Location = New System.Drawing.Point(0, 0)
        Me.RibbonControl.MaxItemId = 21
        Me.RibbonControl.Name = "RibbonControl"
        Me.RibbonControl.PageCategories.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageCategory() {Me.RibbonPageCategory1})
        Me.RibbonControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemCheckEdit1})
        Me.RibbonControl.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2010
        Me.RibbonControl.SelectedPage = Me.RibbonPage6
        Me.RibbonControl.Size = New System.Drawing.Size(1151, 145)
        Me.RibbonControl.StatusBar = Me.RibbonStatusBar
        '
        'ApplicationMenu1
        '
        Me.ApplicationMenu1.ItemLinks.Add(Me.barSetting)
        Me.ApplicationMenu1.ItemLinks.Add(Me.barLoginOut, True)
        Me.ApplicationMenu1.ItemLinks.Add(Me.barExit, True)
        Me.ApplicationMenu1.Name = "ApplicationMenu1"
        Me.ApplicationMenu1.Ribbon = Me.RibbonControl
        '
        'barSetting
        '
        Me.barSetting.Caption = "Setting Koneksi"
        Me.barSetting.Id = 14
        Me.barSetting.ImageIndex = 11
        Me.barSetting.LargeImageIndex = 11
        Me.barSetting.Name = "barSetting"
        '
        'barLoginOut
        '
        Me.barLoginOut.Caption = "Login"
        Me.barLoginOut.Id = 13
        Me.barLoginOut.ImageIndex = 12
        Me.barLoginOut.LargeImageIndex = 12
        Me.barLoginOut.Name = "barLoginOut"
        '
        'barExit
        '
        Me.barExit.Caption = "Keluar"
        Me.barExit.Id = 15
        Me.barExit.ImageIndex = 13
        Me.barExit.LargeImageIndex = 13
        Me.barExit.Name = "barExit"
        '
        'ImageCollectionSmall
        '
        Me.ImageCollectionSmall.ImageStream = CType(resources.GetObject("ImageCollectionSmall.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.ImageCollectionSmall.Images.SetKeyName(0, "product.png")
        Me.ImageCollectionSmall.Images.SetKeyName(1, "coinstack.png")
        Me.ImageCollectionSmall.Images.SetKeyName(2, "dispatch.png")
        Me.ImageCollectionSmall.Images.SetKeyName(3, "market_segmentation2.png")
        Me.ImageCollectionSmall.Images.SetKeyName(4, "alliance2.png")
        Me.ImageCollectionSmall.Images.SetKeyName(5, "currency_dollar_sign2.png")
        Me.ImageCollectionSmall.Images.SetKeyName(6, "balance.png")
        Me.ImageCollectionSmall.Images.SetKeyName(7, "stats_bar_chart.png")
        Me.ImageCollectionSmall.Images.SetKeyName(8, "home.png")
        Me.ImageCollectionSmall.Images.SetKeyName(9, "group.png")
        Me.ImageCollectionSmall.Images.SetKeyName(10, "administrator.png")
        Me.ImageCollectionSmall.Images.SetKeyName(11, "connect_to_database.png")
        Me.ImageCollectionSmall.Images.SetKeyName(12, "key.png")
        Me.ImageCollectionSmall.Images.SetKeyName(13, "close.png")
        '
        'rgbiSkins
        '
        Me.rgbiSkins.Caption = "Skins"
        Me.rgbiSkins.Id = 1
        Me.rgbiSkins.Name = "rgbiSkins"
        '
        'barStaticUID
        '
        Me.barStaticUID.Caption = "User : (none)"
        Me.barStaticUID.Id = 2
        Me.barStaticUID.Name = "barStaticUID"
        Me.barStaticUID.TextAlignment = System.Drawing.StringAlignment.Near
        '
        'barStaticJam
        '
        Me.barStaticJam.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right
        Me.barStaticJam.Caption = "Tanggal System : (none)"
        Me.barStaticJam.Id = 3
        Me.barStaticJam.Name = "barStaticJam"
        Me.barStaticJam.TextAlignment = System.Drawing.StringAlignment.Near
        '
        'barSettingPerusahaan
        '
        Me.barSettingPerusahaan.Caption = "Setting Perusahaan"
        Me.barSettingPerusahaan.Id = 16
        Me.barSettingPerusahaan.ImageIndex = 8
        Me.barSettingPerusahaan.LargeImageIndex = 8
        Me.barSettingPerusahaan.Name = "barSettingPerusahaan"
        Me.barSettingPerusahaan.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large
        '
        'barEditReport
        '
        Me.barEditReport.Caption = "Edit Report"
        Me.barEditReport.Edit = Me.RepositoryItemCheckEdit1
        Me.barEditReport.Id = 18
        Me.barEditReport.Name = "barEditReport"
        '
        'RepositoryItemCheckEdit1
        '
        Me.RepositoryItemCheckEdit1.AutoHeight = False
        Me.RepositoryItemCheckEdit1.Name = "RepositoryItemCheckEdit1"
        '
        'barManagementUser
        '
        Me.barManagementUser.Caption = "Management User"
        Me.barManagementUser.Id = 19
        Me.barManagementUser.ImageIndex = 10
        Me.barManagementUser.Name = "barManagementUser"
        '
        'barManagementRole
        '
        Me.barManagementRole.Caption = "Role User"
        Me.barManagementRole.Id = 20
        Me.barManagementRole.ImageIndex = 9
        Me.barManagementRole.Name = "barManagementRole"
        '
        'ImageCollectionLarge
        '
        Me.ImageCollectionLarge.ImageSize = New System.Drawing.Size(32, 32)
        Me.ImageCollectionLarge.ImageStream = CType(resources.GetObject("ImageCollectionLarge.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.ImageCollectionLarge.Images.SetKeyName(0, "product.png")
        Me.ImageCollectionLarge.Images.SetKeyName(1, "coinstack.png")
        Me.ImageCollectionLarge.Images.SetKeyName(2, "dispatch.png")
        Me.ImageCollectionLarge.Images.SetKeyName(3, "market_segmentation.png")
        Me.ImageCollectionLarge.Images.SetKeyName(4, "alliance.png")
        Me.ImageCollectionLarge.Images.SetKeyName(5, "currency_dollar_sign.png")
        Me.ImageCollectionLarge.Images.SetKeyName(6, "balance.png")
        Me.ImageCollectionLarge.Images.SetKeyName(7, "stats_column_chart.png")
        Me.ImageCollectionLarge.Images.SetKeyName(8, "home.png")
        Me.ImageCollectionLarge.Images.SetKeyName(9, "group.png")
        Me.ImageCollectionLarge.Images.SetKeyName(10, "administrator.png")
        Me.ImageCollectionLarge.Images.SetKeyName(11, "connect_to_database.png")
        Me.ImageCollectionLarge.Images.SetKeyName(12, "key.png")
        Me.ImageCollectionLarge.Images.SetKeyName(13, "close.png")
        '
        'RibbonPageCategory1
        '
        Me.RibbonPageCategory1.Color = System.Drawing.Color.Empty
        Me.RibbonPageCategory1.Name = "RibbonPageCategory1"
        Me.RibbonPageCategory1.Pages.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPage() {Me.RibbonPage6})
        Me.RibbonPageCategory1.Text = "Tools"
        Me.RibbonPageCategory1.Visible = False
        '
        'RibbonPage6
        '
        Me.RibbonPage6.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.RibbonPageGroup2, Me.RibbonPageGroup5})
        Me.RibbonPage6.Name = "RibbonPage6"
        Me.RibbonPage6.Text = "Setting"
        Me.RibbonPage6.Visible = False
        '
        'RibbonPageGroup2
        '
        Me.RibbonPageGroup2.ItemLinks.Add(Me.rgbiSkins)
        Me.RibbonPageGroup2.Name = "RibbonPageGroup2"
        Me.RibbonPageGroup2.Text = "Skins"
        '
        'RibbonPageGroup5
        '
        Me.RibbonPageGroup5.ItemLinks.Add(Me.barSettingPerusahaan)
        Me.RibbonPageGroup5.ItemLinks.Add(Me.barManagementUser, True)
        Me.RibbonPageGroup5.ItemLinks.Add(Me.barManagementRole)
        Me.RibbonPageGroup5.ItemLinks.Add(Me.barEditReport, True)
        Me.RibbonPageGroup5.Name = "RibbonPageGroup5"
        Me.RibbonPageGroup5.Text = "Setting Applikasi"
        '
        'RibbonStatusBar
        '
        Me.RibbonStatusBar.ItemLinks.Add(Me.barStaticUID)
        Me.RibbonStatusBar.ItemLinks.Add(Me.barStaticJam)
        Me.RibbonStatusBar.Location = New System.Drawing.Point(0, 498)
        Me.RibbonStatusBar.Name = "RibbonStatusBar"
        Me.RibbonStatusBar.Ribbon = Me.RibbonControl
        Me.RibbonStatusBar.Size = New System.Drawing.Size(1151, 31)
        '
        'XtraTabbedMdiManager1
        '
        Me.XtraTabbedMdiManager1.MdiParent = Me
        '
        'BarButtonItem1
        '
        Me.BarButtonItem1.Caption = "Data Satuan"
        Me.BarButtonItem1.Id = 4
        Me.BarButtonItem1.Name = "BarButtonItem1"
        '
        'Timer1
        '
        Me.Timer1.Interval = 5000
        '
        'ICButtons
        '
        Me.ICButtons.ImageSize = New System.Drawing.Size(24, 24)
        Me.ICButtons.ImageStream = CType(resources.GetObject("ICButtons.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.ICButtons.Images.SetKeyName(0, "add.png")
        Me.ICButtons.Images.SetKeyName(1, "pencil.png")
        Me.ICButtons.Images.SetKeyName(2, "trash_can.png")
        Me.ICButtons.Images.SetKeyName(3, "reload.png")
        Me.ICButtons.Images.SetKeyName(4, "checkmark.png")
        Me.ICButtons.Images.SetKeyName(5, "close.png")
        Me.ICButtons.Images.SetKeyName(6, "print.png")
        Me.ICButtons.Images.SetKeyName(7, "lock_open.png")
        Me.ICButtons.Images.SetKeyName(8, "diskette.png")
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1151, 529)
        Me.Controls.Add(Me.RibbonStatusBar)
        Me.Controls.Add(Me.RibbonControl)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.Name = "frmMain"
        Me.Ribbon = Me.RibbonControl
        Me.StatusBar = Me.RibbonStatusBar
        Me.Text = "CTRL Software"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.RibbonControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ApplicationMenu1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ImageCollectionSmall, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ImageCollectionLarge, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.XtraTabbedMdiManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ICButtons, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RibbonControl As DevExpress.XtraBars.Ribbon.RibbonControl
    Friend WithEvents RibbonStatusBar As DevExpress.XtraBars.Ribbon.RibbonStatusBar
    Friend WithEvents XtraTabbedMdiManager1 As DevExpress.XtraTabbedMdi.XtraTabbedMdiManager
    Friend WithEvents defaultLookAndFeel1 As DevExpress.LookAndFeel.DefaultLookAndFeel
    Friend WithEvents RibbonPageCategory1 As DevExpress.XtraBars.Ribbon.RibbonPageCategory
    Friend WithEvents rgbiSkins As DevExpress.XtraBars.RibbonGalleryBarItem
    Friend WithEvents barStaticUID As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents RibbonPage6 As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents RibbonPageGroup2 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents barStaticJam As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents BarButtonItem1 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents ApplicationMenu1 As DevExpress.XtraBars.Ribbon.ApplicationMenu
    Friend WithEvents barLoginOut As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents barSetting As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents barExit As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents barSettingPerusahaan As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents barEditReport As DevExpress.XtraBars.BarEditItem
    Friend WithEvents RepositoryItemCheckEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents RibbonPageGroup5 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents ImageCollectionLarge As DevExpress.Utils.ImageCollection
    Friend WithEvents ImageCollectionSmall As DevExpress.Utils.ImageCollection
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents barManagementUser As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents barManagementRole As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents ICButtons As DevExpress.Utils.ImageCollection


End Class

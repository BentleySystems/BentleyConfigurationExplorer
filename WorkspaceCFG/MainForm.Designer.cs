using System;
using System.Diagnostics;
using System.Drawing;

namespace WorkspaceCFG
{
    [System.ComponentModel.DesignerCategory("Form")]
    public partial class MainForm : System.Windows.Forms.Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is not null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            MenuStrip = new System.Windows.Forms.MenuStrip();
            FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            NewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            OpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            CloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            ImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            MsdebugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            ErrorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            VariableListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToTextFileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            CompiledCFGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            WorkspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToTextFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToSummaryFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            FileListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToTextToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            ToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            VariableExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            FileHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            FullHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            PathCheckerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            VariableGeneratorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            VariableValidaterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            VariableLibraryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            CompareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            IconCreatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            DebugCompareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            DebugLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            OptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ReloadWorkspacesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ResetWindowLocationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ApplicationManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            AutoloadWorkspaceSelectorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            AutoloadWorkspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ViewMenu = new System.Windows.Forms.ToolStripMenuItem();
            StatusBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            WindowsMenu = new System.Windows.Forms.ToolStripMenuItem();
            CascadeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            TileVerticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            TileHorizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            CloseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ArrangeIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ResetLocationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            KeepInMainWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            HelpMenu = new System.Windows.Forms.ToolStripMenuItem();
            ContentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            CopyToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            StatusStrip = new System.Windows.Forms.StatusStrip();
            ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ToolTip = new System.Windows.Forms.ToolTip(components);
            OpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            SaveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            QuickAccessPanel = new System.Windows.Forms.FlowLayoutPanel();
            ProcessButton = new System.Windows.Forms.Button();
            ReloadButton = new System.Windows.Forms.Button();
            LoadButton = new System.Windows.Forms.Button();
            VariableExplorerButton = new System.Windows.Forms.Button();
            FileHistoryButton = new System.Windows.Forms.Button();
            EventHistoryButton = new System.Windows.Forms.Button();
            SearchButton = new System.Windows.Forms.Button();
            VariableValidatorButton = new System.Windows.Forms.Button();
            PathValidatorButton = new System.Windows.Forms.Button();
            MenuStrip.SuspendLayout();
            StatusStrip.SuspendLayout();
            QuickAccessPanel.SuspendLayout();
            SuspendLayout();
            // 
            // MenuStrip
            // 
            MenuStrip.ImageScalingSize = new Size(24, 24);
            MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { FileMenu, ToolsToolStripMenuItem, OptionsToolStripMenuItem, ViewMenu, WindowsMenu, HelpMenu });
            MenuStrip.MdiWindowListItem = WindowsMenu;
            MenuStrip.Name = "MenuStrip";
            MenuStrip.Padding = new System.Windows.Forms.Padding(9, 2, 0, 2);
            MenuStrip.Size = new Size(1429, 36);
            MenuStrip.TabIndex = 5;
            MenuStrip.Text = "MenuStrip";
            // 
            // FileMenu
            // 
            FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { NewToolStripMenuItem, OpenToolStripMenuItem, CloseToolStripMenuItem, ToolStripSeparator3, ImportToolStripMenuItem, ToolStripMenuItem2, SaveToolStripMenuItem, ToolStripSeparator4, ExitToolStripMenuItem });
            FileMenu.ImageTransparentColor = SystemColors.ActiveBorder;
            FileMenu.Name = "FileMenu";
            FileMenu.Size = new Size(16, 32);
            // 
            // NewToolStripMenuItem
            // 
            NewToolStripMenuItem.Image = (Image)resources.GetObject("NewToolStripMenuItem.Image");
            NewToolStripMenuItem.ImageTransparentColor = Color.Black;
            NewToolStripMenuItem.Name = "NewToolStripMenuItem";
            NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N;
            NewToolStripMenuItem.Size = new Size(167, 34);
            NewToolStripMenuItem.Click += ShowNewForm;
            // 
            // OpenToolStripMenuItem
            // 
            OpenToolStripMenuItem.Image = (Image)resources.GetObject("OpenToolStripMenuItem.Image");
            OpenToolStripMenuItem.ImageTransparentColor = Color.Black;
            OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
            OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O;
            OpenToolStripMenuItem.Size = new Size(167, 34);
            OpenToolStripMenuItem.Click += OpenFile;
            // 
            // CloseToolStripMenuItem
            // 
            CloseToolStripMenuItem.Name = "CloseToolStripMenuItem";
            CloseToolStripMenuItem.Size = new Size(167, 34);
            CloseToolStripMenuItem.Click += CloseToolStripMenuItem_Click;
            // 
            // ToolStripSeparator3
            // 
            ToolStripSeparator3.Name = "ToolStripSeparator3";
            ToolStripSeparator3.Size = new Size(164, 6);
            // 
            // ImportToolStripMenuItem
            // 
            ImportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { MsdebugToolStripMenuItem });
            ImportToolStripMenuItem.Name = "ImportToolStripMenuItem";
            ImportToolStripMenuItem.Size = new Size(167, 34);
            // 
            // MsdebugToolStripMenuItem
            // 
            MsdebugToolStripMenuItem.Name = "MsdebugToolStripMenuItem";
            MsdebugToolStripMenuItem.Size = new Size(102, 34);
            MsdebugToolStripMenuItem.Click += MsdebugToolStripMenuItem_Click;
            // 
            // ToolStripMenuItem2
            // 
            ToolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ErrorsToolStripMenuItem, VariableListToolStripMenuItem, WorkspaceToolStripMenuItem, FileListToolStripMenuItem });
            ToolStripMenuItem2.Name = "ToolStripMenuItem2";
            ToolStripMenuItem2.Size = new Size(167, 34);
            // 
            // ErrorsToolStripMenuItem
            // 
            ErrorsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ToTextToolStripMenuItem });
            ErrorsToolStripMenuItem.Name = "ErrorsToolStripMenuItem";
            ErrorsToolStripMenuItem.Size = new Size(102, 34);
            // 
            // ToTextToolStripMenuItem
            // 
            ToTextToolStripMenuItem.Name = "ToTextToolStripMenuItem";
            ToTextToolStripMenuItem.Size = new Size(102, 34);
            ToTextToolStripMenuItem.Click += ToTextToolStripMenuItem_Click;
            // 
            // VariableListToolStripMenuItem
            // 
            VariableListToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ToTextFileToolStripMenuItem1, CompiledCFGToolStripMenuItem });
            VariableListToolStripMenuItem.Name = "VariableListToolStripMenuItem";
            VariableListToolStripMenuItem.Size = new Size(102, 34);
            // 
            // ToTextFileToolStripMenuItem1
            // 
            ToTextFileToolStripMenuItem1.Name = "ToTextFileToolStripMenuItem1";
            ToTextFileToolStripMenuItem1.Size = new Size(102, 34);
            ToTextFileToolStripMenuItem1.Click += ToTextFileToolStripMenuItem1_Click;
            // 
            // CompiledCFGToolStripMenuItem
            // 
            CompiledCFGToolStripMenuItem.Name = "CompiledCFGToolStripMenuItem";
            CompiledCFGToolStripMenuItem.Size = new Size(102, 34);
            CompiledCFGToolStripMenuItem.Click += CompiledCFGToolStripMenuItem_Click;
            // 
            // WorkspaceToolStripMenuItem
            // 
            WorkspaceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ToTextFileToolStripMenuItem, ToSummaryFileToolStripMenuItem });
            WorkspaceToolStripMenuItem.Name = "WorkspaceToolStripMenuItem";
            WorkspaceToolStripMenuItem.Size = new Size(102, 34);
            // 
            // ToTextFileToolStripMenuItem
            // 
            ToTextFileToolStripMenuItem.Name = "ToTextFileToolStripMenuItem";
            ToTextFileToolStripMenuItem.Size = new Size(102, 34);
            ToTextFileToolStripMenuItem.Click += ToTextFileToolStripMenuItem_Click;
            // 
            // ToSummaryFileToolStripMenuItem
            // 
            ToSummaryFileToolStripMenuItem.Name = "ToSummaryFileToolStripMenuItem";
            ToSummaryFileToolStripMenuItem.Size = new Size(102, 34);
            ToSummaryFileToolStripMenuItem.Click += ToSummaryFileToolStripMenuItem_Click;
            // 
            // FileListToolStripMenuItem
            // 
            FileListToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ToTextToolStripMenuItem1, ToExcelToolStripMenuItem });
            FileListToolStripMenuItem.Name = "FileListToolStripMenuItem";
            FileListToolStripMenuItem.Size = new Size(102, 34);
            // 
            // ToTextToolStripMenuItem1
            // 
            ToTextToolStripMenuItem1.Name = "ToTextToolStripMenuItem1";
            ToTextToolStripMenuItem1.Size = new Size(102, 34);
            ToTextToolStripMenuItem1.Click += ToTextToolStripMenuItem1_Click;
            // 
            // ToExcelToolStripMenuItem
            // 
            ToExcelToolStripMenuItem.Name = "ToExcelToolStripMenuItem";
            ToExcelToolStripMenuItem.Size = new Size(102, 34);
            ToExcelToolStripMenuItem.Click += ToExcelToolStripMenuItem_Click;
            // 
            // SaveToolStripMenuItem
            // 
            SaveToolStripMenuItem.Image = (Image)resources.GetObject("SaveToolStripMenuItem.Image");
            SaveToolStripMenuItem.ImageTransparentColor = Color.Black;
            SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            SaveToolStripMenuItem.Size = new Size(167, 34);
            SaveToolStripMenuItem.Click += SaveToolStripMenuItem_Click;
            // 
            // ToolStripSeparator4
            // 
            ToolStripSeparator4.Name = "ToolStripSeparator4";
            ToolStripSeparator4.Size = new Size(164, 6);
            // 
            // ExitToolStripMenuItem
            // 
            ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            ExitToolStripMenuItem.Size = new Size(167, 34);
            ExitToolStripMenuItem.Click += ExitToolsStripMenuItem_Click;
            // 
            // ToolsToolStripMenuItem
            // 
            ToolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { VariableExplorerToolStripMenuItem, FileHistoryToolStripMenuItem, FullHistoryToolStripMenuItem, PathCheckerToolStripMenuItem, ToolStripMenuItem1, VariableGeneratorToolStripMenuItem, VariableValidaterToolStripMenuItem, VariableLibraryToolStripMenuItem, ToolStripSeparator5, CompareToolStripMenuItem, IconCreatorToolStripMenuItem, ToolStripSeparator6, DebugCompareToolStripMenuItem, DebugLineToolStripMenuItem, ToolStripSeparator7, ApplicationManagerToolStripMenuItem });
            ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem";
            ToolsToolStripMenuItem.Size = new Size(16, 32);
            // 
            // VariableExplorerToolStripMenuItem
            // 
            VariableExplorerToolStripMenuItem.Name = "VariableExplorerToolStripMenuItem";
            VariableExplorerToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E;
            VariableExplorerToolStripMenuItem.Size = new Size(162, 34);
            VariableExplorerToolStripMenuItem.Click += VariableExplorerToolStripMenuItem_Click;
            // 
            // FileHistoryToolStripMenuItem
            // 
            FileHistoryToolStripMenuItem.Name = "FileHistoryToolStripMenuItem";
            FileHistoryToolStripMenuItem.Size = new Size(162, 34);
            FileHistoryToolStripMenuItem.Click += FileExplorerToolStripMenuItem_Click;
            // 
            // FullHistoryToolStripMenuItem
            // 
            FullHistoryToolStripMenuItem.Name = "FullHistoryToolStripMenuItem";
            FullHistoryToolStripMenuItem.Size = new Size(162, 34);
            FullHistoryToolStripMenuItem.Click += FullHistoryToolStripMenuItem_Click;
            // 
            // PathCheckerToolStripMenuItem
            // 
            PathCheckerToolStripMenuItem.Name = "PathCheckerToolStripMenuItem";
            PathCheckerToolStripMenuItem.Size = new Size(162, 34);
            PathCheckerToolStripMenuItem.Click += PathCheckerToolStripMenuItem_Click;
            // 
            // ToolStripMenuItem1
            // 
            ToolStripMenuItem1.Name = "ToolStripMenuItem1";
            ToolStripMenuItem1.Size = new Size(162, 34);
            ToolStripMenuItem1.Tag = "";
            ToolStripMenuItem1.Click += ToolStripMenuItem1_Click;
            // 
            // VariableGeneratorToolStripMenuItem
            // 
            VariableGeneratorToolStripMenuItem.Name = "VariableGeneratorToolStripMenuItem";
            VariableGeneratorToolStripMenuItem.Size = new Size(162, 34);
            VariableGeneratorToolStripMenuItem.Click += VariableGeneratorToolStripMenuItem_Click;
            // 
            // VariableValidaterToolStripMenuItem
            // 
            VariableValidaterToolStripMenuItem.Name = "VariableValidaterToolStripMenuItem";
            VariableValidaterToolStripMenuItem.Size = new Size(162, 34);
            VariableValidaterToolStripMenuItem.Click += VariableValidatorToolStripMenuItem_Click;
            // 
            // VariableLibraryToolStripMenuItem
            // 
            VariableLibraryToolStripMenuItem.Name = "VariableLibraryToolStripMenuItem";
            VariableLibraryToolStripMenuItem.Size = new Size(162, 34);
            VariableLibraryToolStripMenuItem.Click += VariableLibraryToolStripMenuItem_Click;
            // 
            // ToolStripSeparator5
            // 
            ToolStripSeparator5.Name = "ToolStripSeparator5";
            ToolStripSeparator5.Size = new Size(159, 6);
            // 
            // CompareToolStripMenuItem
            // 
            CompareToolStripMenuItem.Name = "CompareToolStripMenuItem";
            CompareToolStripMenuItem.Size = new Size(162, 34);
            CompareToolStripMenuItem.Click += CompareToolStripMenuItem_Click;
            // 
            // IconCreatorToolStripMenuItem
            // 
            IconCreatorToolStripMenuItem.Name = "IconCreatorToolStripMenuItem";
            IconCreatorToolStripMenuItem.Size = new Size(162, 34);
            IconCreatorToolStripMenuItem.Click += IconCreatorToolStripMenuItem_Click;
            // 
            // ToolStripSeparator6
            // 
            ToolStripSeparator6.Name = "ToolStripSeparator6";
            ToolStripSeparator6.Size = new Size(159, 6);
            // 
            // DebugCompareToolStripMenuItem
            // 
            DebugCompareToolStripMenuItem.Name = "DebugCompareToolStripMenuItem";
            DebugCompareToolStripMenuItem.Size = new Size(162, 34);
            DebugCompareToolStripMenuItem.Click += DebugCompareButton_Click;
            // 
            // DebugLineToolStripMenuItem
            // 
            DebugLineToolStripMenuItem.Name = "DebugLineToolStripMenuItem";
            DebugLineToolStripMenuItem.Size = new Size(162, 34);
            DebugLineToolStripMenuItem.Click += DebugLineButton_Click;
            // 
            // ToolStripSeparator7
            // 
            ToolStripSeparator7.Name = "ToolStripSeparator7";
            ToolStripSeparator7.Size = new Size(159, 6);
            // 
            // ApplicationManagerToolStripMenuItem
            // 
            ApplicationManagerToolStripMenuItem.Name = "ApplicationManagerToolStripMenuItem";
            ApplicationManagerToolStripMenuItem.Size = new Size(162, 34);
            ApplicationManagerToolStripMenuItem.Click += ApplicationManagerToolStripMenuItem_Click;
            // 
            // OptionsToolStripMenuItem
            // 
            OptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ReloadWorkspacesToolStripMenuItem, ResetWindowLocationsToolStripMenuItem, AutoloadWorkspaceSelectorToolStripMenuItem, AutoloadWorkspaceToolStripMenuItem });
            OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem";
            OptionsToolStripMenuItem.Size = new Size(16, 32);
            // 
            // ReloadWorkspacesToolStripMenuItem
            // 
            ReloadWorkspacesToolStripMenuItem.Name = "ReloadWorkspacesToolStripMenuItem";
            ReloadWorkspacesToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            ReloadWorkspacesToolStripMenuItem.Size = new Size(133, 34);
            ReloadWorkspacesToolStripMenuItem.Click += ReloadWorkspacesToolStripMenuItem_Click;
            // 
            // ResetWindowLocationsToolStripMenuItem
            // 
            ResetWindowLocationsToolStripMenuItem.Name = "ResetWindowLocationsToolStripMenuItem";
            ResetWindowLocationsToolStripMenuItem.Size = new Size(133, 34);
            ResetWindowLocationsToolStripMenuItem.Click += ResetWindowLocationsToolStripMenuItem_Click;
            // 
            // AutoloadWorkspaceSelectorToolStripMenuItem
            // 
            AutoloadWorkspaceSelectorToolStripMenuItem.CheckOnClick = true;
            AutoloadWorkspaceSelectorToolStripMenuItem.Name = "AutoloadWorkspaceSelectorToolStripMenuItem";
            AutoloadWorkspaceSelectorToolStripMenuItem.Size = new Size(133, 34);
            AutoloadWorkspaceSelectorToolStripMenuItem.Click += AutoloadWorkspaceSelectorToolStripMenuItem_Click;
            // 
            // AutoloadWorkspaceToolStripMenuItem
            // 
            AutoloadWorkspaceToolStripMenuItem.CheckOnClick = true;
            AutoloadWorkspaceToolStripMenuItem.Name = "AutoloadWorkspaceToolStripMenuItem";
            AutoloadWorkspaceToolStripMenuItem.Size = new Size(133, 34);
            AutoloadWorkspaceToolStripMenuItem.Click += AutoloadWorkspaceToolStripMenuItem_Click;
            // 
            // ViewMenu
            // 
            ViewMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { StatusBarToolStripMenuItem });
            ViewMenu.Name = "ViewMenu";
            ViewMenu.Size = new Size(16, 32);
            // 
            // StatusBarToolStripMenuItem
            // 
            StatusBarToolStripMenuItem.Checked = true;
            StatusBarToolStripMenuItem.CheckOnClick = true;
            StatusBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            StatusBarToolStripMenuItem.Name = "StatusBarToolStripMenuItem";
            StatusBarToolStripMenuItem.Size = new Size(102, 34);
            StatusBarToolStripMenuItem.Click += StatusBarToolStripMenuItem_Click;
            // 
            // WindowsMenu
            // 
            WindowsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { CascadeToolStripMenuItem, TileVerticalToolStripMenuItem, TileHorizontalToolStripMenuItem, CloseAllToolStripMenuItem, ArrangeIconsToolStripMenuItem, ResetLocationsToolStripMenuItem, KeepInMainWindowToolStripMenuItem });
            WindowsMenu.Name = "WindowsMenu";
            WindowsMenu.Size = new Size(16, 32);
            // 
            // CascadeToolStripMenuItem
            // 
            CascadeToolStripMenuItem.Name = "CascadeToolStripMenuItem";
            CascadeToolStripMenuItem.Size = new Size(102, 34);
            CascadeToolStripMenuItem.Click += CascadeToolStripMenuItem_Click;
            // 
            // TileVerticalToolStripMenuItem
            // 
            TileVerticalToolStripMenuItem.Name = "TileVerticalToolStripMenuItem";
            TileVerticalToolStripMenuItem.Size = new Size(102, 34);
            TileVerticalToolStripMenuItem.Click += TileVerticalToolStripMenuItem_Click;
            // 
            // TileHorizontalToolStripMenuItem
            // 
            TileHorizontalToolStripMenuItem.Name = "TileHorizontalToolStripMenuItem";
            TileHorizontalToolStripMenuItem.Size = new Size(102, 34);
            TileHorizontalToolStripMenuItem.Click += TileHorizontalToolStripMenuItem_Click;
            // 
            // CloseAllToolStripMenuItem
            // 
            CloseAllToolStripMenuItem.Name = "CloseAllToolStripMenuItem";
            CloseAllToolStripMenuItem.Size = new Size(102, 34);
            CloseAllToolStripMenuItem.Click += CloseAllToolStripMenuItem_Click;
            // 
            // ArrangeIconsToolStripMenuItem
            // 
            ArrangeIconsToolStripMenuItem.Name = "ArrangeIconsToolStripMenuItem";
            ArrangeIconsToolStripMenuItem.Size = new Size(102, 34);
            ArrangeIconsToolStripMenuItem.Click += ArrangeIconsToolStripMenuItem_Click;
            // 
            // ResetLocationsToolStripMenuItem
            // 
            ResetLocationsToolStripMenuItem.Name = "ResetLocationsToolStripMenuItem";
            ResetLocationsToolStripMenuItem.Size = new Size(102, 34);
            ResetLocationsToolStripMenuItem.Click += ResetLocationsToolStripMenuItem_Click;
            // 
            // KeepInMainWindowToolStripMenuItem
            // 
            KeepInMainWindowToolStripMenuItem.CheckOnClick = true;
            KeepInMainWindowToolStripMenuItem.Name = "KeepInMainWindowToolStripMenuItem";
            KeepInMainWindowToolStripMenuItem.Size = new Size(102, 34);
            KeepInMainWindowToolStripMenuItem.CheckStateChanged += KeepInMainWindowToolStripMenuItem_CheckStateChanged;
            // 
            // HelpMenu
            // 
            HelpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ContentsToolStripMenuItem, ToolStripSeparator8, AboutToolStripMenuItem });
            HelpMenu.Name = "HelpMenu";
            HelpMenu.Size = new Size(16, 32);
            // 
            // ContentsToolStripMenuItem
            // 
            ContentsToolStripMenuItem.Name = "ContentsToolStripMenuItem";
            ContentsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F1;
            ContentsToolStripMenuItem.Size = new Size(172, 34);
            ContentsToolStripMenuItem.Click += ContentsToolStripMenuItem_Click;
            // 
            // ToolStripSeparator8
            // 
            ToolStripSeparator8.Name = "ToolStripSeparator8";
            ToolStripSeparator8.Size = new Size(169, 6);
            // 
            // AboutToolStripMenuItem
            // 
            AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            AboutToolStripMenuItem.Size = new Size(172, 34);
            AboutToolStripMenuItem.Click += AboutToolStripMenuItem_Click;
            // 
            // CopyToClipboardToolStripMenuItem
            // 
            CopyToClipboardToolStripMenuItem.Name = "CopyToClipboardToolStripMenuItem";
            CopyToClipboardToolStripMenuItem.Size = new Size(171, 22);
            CopyToClipboardToolStripMenuItem.Click += CopyToClipboardToolStripMenuItem_Click;
            // 
            // ViewToolStripMenuItem
            // 
            ViewToolStripMenuItem.Name = "ViewToolStripMenuItem";
            ViewToolStripMenuItem.Size = new Size(171, 22);
            ViewToolStripMenuItem.Click += ViewToolStripMenuItem_Click;
            // 
            // StatusStrip
            // 
            StatusStrip.ImageScalingSize = new Size(24, 24);
            StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripStatusLabel });
            StatusStrip.Name = "StatusStrip";
            StatusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 21, 0);
            StatusStrip.Size = new Size(1429, 22);
            StatusStrip.TabIndex = 7;
            StatusStrip.Text = "StatusStrip";
            // 
            // ToolStripStatusLabel
            // 
            ToolStripStatusLabel.BackColor = SystemColors.Control;
            ToolStripStatusLabel.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            ToolStripStatusLabel.Name = "ToolStripStatusLabel";
            ToolStripStatusLabel.Size = new Size(0, 0);
            ToolStripStatusLabel.Spring = true;
            ToolStripStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // OpenFileDialog1
            // 
            OpenFileDialog1.DefaultExt = "*.txt";
            OpenFileDialog1.FileName = "OpenFileDialog1";
            // 
            // QuickAccessPanel
            // 
            QuickAccessPanel.Controls.Add(ProcessButton);
            QuickAccessPanel.Controls.Add(ReloadButton);
            QuickAccessPanel.Controls.Add(LoadButton);
            QuickAccessPanel.Controls.Add(VariableExplorerButton);
            QuickAccessPanel.Controls.Add(FileHistoryButton);
            QuickAccessPanel.Controls.Add(EventHistoryButton);
            QuickAccessPanel.Controls.Add(SearchButton);
            QuickAccessPanel.Controls.Add(VariableValidatorButton);
            QuickAccessPanel.Controls.Add(PathValidatorButton);
            QuickAccessPanel.AutoSize = true;
            QuickAccessPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            QuickAccessPanel.Dock = System.Windows.Forms.DockStyle.Top;
            QuickAccessPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            QuickAccessPanel.Name = "QuickAccessPanel";
            QuickAccessPanel.Padding = new System.Windows.Forms.Padding(4);
            QuickAccessPanel.TabIndex = 8;
            QuickAccessPanel.WrapContents = false;
            // 
            // ProcessButton
            // 
            ProcessButton.AutoSize = true;
            ProcessButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ProcessButton.Image = (Image)resources.GetObject("ProcessToolStripButton.Image");
            ProcessButton.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            ProcessButton.MaximumSize = new Size(0, 112);
            ProcessButton.MinimumSize = new Size(70, 112);
            ProcessButton.Name = "ProcessButton";
            ProcessButton.Padding = new System.Windows.Forms.Padding(6, 10, 6, 6);
            ProcessButton.TextAlign = ContentAlignment.BottomCenter;
            ProcessButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            ProcessButton.UseVisualStyleBackColor = true;
            ProcessButton.Click += ShowNewForm;
            // 
            // ReloadButton
            // 
            ReloadButton.AutoSize = true;
            ReloadButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ReloadButton.Image = (Image)resources.GetObject("ReloadToolStripButton.Image");
            ReloadButton.Margin = new System.Windows.Forms.Padding(3);
            ReloadButton.MaximumSize = new Size(0, 112);
            ReloadButton.MinimumSize = new Size(70, 112);
            ReloadButton.Name = "ReloadButton";
            ReloadButton.Padding = new System.Windows.Forms.Padding(6, 10, 6, 6);
            ReloadButton.TextAlign = ContentAlignment.BottomCenter;
            ReloadButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            ReloadButton.UseVisualStyleBackColor = true;
            ReloadButton.Click += ReloadWorkspacesToolStripMenuItem_Click;
            // 
            // LoadButton
            // 
            LoadButton.AutoSize = true;
            LoadButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            LoadButton.Image = (Image)resources.GetObject("LoadToolStripButton.Image");
            LoadButton.Margin = new System.Windows.Forms.Padding(3, 3, 9, 3);
            LoadButton.MaximumSize = new Size(0, 112);
            LoadButton.MinimumSize = new Size(70, 112);
            LoadButton.Name = "LoadButton";
            LoadButton.Padding = new System.Windows.Forms.Padding(6, 10, 6, 6);
            LoadButton.TextAlign = ContentAlignment.BottomCenter;
            LoadButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            LoadButton.UseVisualStyleBackColor = true;
            LoadButton.Click += OpenFile;
            // 
            // VariableExplorerButton
            // 
            VariableExplorerButton.AutoSize = true;
            VariableExplorerButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            VariableExplorerButton.Image = (Image)resources.GetObject("VariableExplorerToolStripButton.Image");
            VariableExplorerButton.Margin = new System.Windows.Forms.Padding(3);
            VariableExplorerButton.MaximumSize = new Size(0, 112);
            VariableExplorerButton.MinimumSize = new Size(70, 112);
            VariableExplorerButton.Name = "VariableExplorerButton";
            VariableExplorerButton.Padding = new System.Windows.Forms.Padding(6, 10, 6, 6);
            VariableExplorerButton.TextAlign = ContentAlignment.BottomCenter;
            VariableExplorerButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            VariableExplorerButton.UseVisualStyleBackColor = true;
            VariableExplorerButton.Click += VariableExplorerToolStripMenuItem_Click;
            // 
            // FileHistoryButton
            // 
            FileHistoryButton.AutoSize = true;
            FileHistoryButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            FileHistoryButton.Image = (Image)resources.GetObject("FileHistoryToolStripButton.Image");
            FileHistoryButton.Margin = new System.Windows.Forms.Padding(3);
            FileHistoryButton.MaximumSize = new Size(0, 112);
            FileHistoryButton.MinimumSize = new Size(70, 112);
            FileHistoryButton.Name = "FileHistoryButton";
            FileHistoryButton.Padding = new System.Windows.Forms.Padding(6, 10, 6, 6);
            FileHistoryButton.TextAlign = ContentAlignment.BottomCenter;
            FileHistoryButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            FileHistoryButton.UseVisualStyleBackColor = true;
            FileHistoryButton.Click += FileExplorerToolStripMenuItem_Click;
            // 
            // EventHistoryButton
            // 
            EventHistoryButton.AutoSize = true;
            EventHistoryButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            EventHistoryButton.Image = (Image)resources.GetObject("EventHistoryToolStripButton.Image");
            EventHistoryButton.Margin = new System.Windows.Forms.Padding(3, 3, 9, 3);
            EventHistoryButton.MaximumSize = new Size(0, 112);
            EventHistoryButton.MinimumSize = new Size(70, 112);
            EventHistoryButton.Name = "EventHistoryButton";
            EventHistoryButton.Padding = new System.Windows.Forms.Padding(6, 10, 6, 6);
            EventHistoryButton.TextAlign = ContentAlignment.BottomCenter;
            EventHistoryButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            EventHistoryButton.UseVisualStyleBackColor = true;
            EventHistoryButton.Click += FullHistoryToolStripMenuItem_Click;
            // 
            // SearchButton
            // 
            SearchButton.AutoSize = true;
            SearchButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            SearchButton.Image = (Image)resources.GetObject("SearchToolStripButton.Image");
            SearchButton.Margin = new System.Windows.Forms.Padding(3);
            SearchButton.MaximumSize = new Size(0, 112);
            SearchButton.MinimumSize = new Size(70, 112);
            SearchButton.Name = "SearchButton";
            SearchButton.Padding = new System.Windows.Forms.Padding(6, 10, 6, 6);
            SearchButton.TextAlign = ContentAlignment.BottomCenter;
            SearchButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            SearchButton.UseVisualStyleBackColor = true;
            SearchButton.Click += ToolStripMenuItem1_Click;
            // 
            // VariableValidatorButton
            // 
            VariableValidatorButton.AutoSize = true;
            VariableValidatorButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            VariableValidatorButton.Image = (Image)resources.GetObject("VariableValidatorToolStripButton.Image");
            VariableValidatorButton.Margin = new System.Windows.Forms.Padding(9, 3, 3, 3);
            VariableValidatorButton.MaximumSize = new Size(0, 112);
            VariableValidatorButton.MinimumSize = new Size(70, 112);
            VariableValidatorButton.Name = "VariableValidatorButton";
            VariableValidatorButton.Padding = new System.Windows.Forms.Padding(6, 10, 6, 6);
            VariableValidatorButton.TextAlign = ContentAlignment.BottomCenter;
            VariableValidatorButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            VariableValidatorButton.UseVisualStyleBackColor = true;
            VariableValidatorButton.Click += VariableValidatorToolStripMenuItem_Click;
            // 
            // PathValidatorButton
            // 
            PathValidatorButton.AutoSize = true;
            PathValidatorButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            PathValidatorButton.Image = (Image)resources.GetObject("PathValidatorToolStripButton.Image");
            PathValidatorButton.Margin = new System.Windows.Forms.Padding(3);
            PathValidatorButton.MaximumSize = new Size(0, 112);
            PathValidatorButton.MinimumSize = new Size(70, 112);
            PathValidatorButton.Name = "PathValidatorButton";
            PathValidatorButton.Padding = new System.Windows.Forms.Padding(6, 10, 6, 6);
            PathValidatorButton.TextAlign = ContentAlignment.BottomCenter;
            PathValidatorButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            PathValidatorButton.UseVisualStyleBackColor = true;
            PathValidatorButton.Click += PathCheckerToolStripMenuItem_Click;
            // 
            // MainForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            BackColor = SystemColors.ControlLightLight;
            ClientSize = new Size(1429, 1074);
            Controls.Add(QuickAccessPanel);
            Controls.Add(MenuStrip);
            Controls.Add(StatusStrip);
            IsMdiContainer = true;
            KeyPreview = true;
            MainMenuStrip = MenuStrip;
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            MinimumSize = new Size(514, 630);
            Name = "MainForm";
            ShowIcon = false;
            FormClosing += MainForm_FormClosing;
            MdiChildActivate += WindowClosed;
            Shown += MainForm_Shown;
            DragDrop += MainForm_DragDrop;
            DragOver += MainForm_DragOver;
            KeyDown += WatchF5_KeyDown;
            MenuStrip.ResumeLayout(false);
            MenuStrip.PerformLayout();
            StatusStrip.ResumeLayout(false);
            StatusStrip.PerformLayout();
            QuickAccessPanel.ResumeLayout(false);
            QuickAccessPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }
        internal System.Windows.Forms.ToolStripMenuItem ContentsToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem HelpMenu;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator8;
        internal System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ArrangeIconsToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem CloseAllToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem WindowsMenu;
        internal System.Windows.Forms.ToolStripMenuItem CascadeToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem TileVerticalToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem TileHorizontalToolStripMenuItem;
        internal System.Windows.Forms.ToolTip ToolTip;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel;
        internal System.Windows.Forms.StatusStrip StatusStrip;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator4;
        internal System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem NewToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem FileMenu;
        internal System.Windows.Forms.ToolStripMenuItem OpenToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator3;
        internal System.Windows.Forms.ToolStripMenuItem SaveToolStripMenuItem;
        internal System.Windows.Forms.MenuStrip MenuStrip;
        internal System.Windows.Forms.ToolStripMenuItem ViewMenu;
        internal System.Windows.Forms.ToolStripMenuItem StatusBarToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ToolsToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem VariableExplorerToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem FileHistoryToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem FullHistoryToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripMenuItem VariableGeneratorToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem VariableLibraryToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator5;
        internal System.Windows.Forms.ToolStripMenuItem CompareToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem IconCreatorToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem2;
        internal System.Windows.Forms.ToolStripMenuItem ErrorsToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ToTextToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem VariableListToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ToTextFileToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripMenuItem CompiledCFGToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem WorkspaceToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ToTextFileToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem FileListToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ToTextToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripMenuItem ToExcelToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem CloseToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem CopyToClipboardToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ViewToolStripMenuItem;
        internal System.Windows.Forms.OpenFileDialog OpenFileDialog1;
        internal System.Windows.Forms.SaveFileDialog SaveFileDialog1;
        internal System.Windows.Forms.ToolStripMenuItem ResetLocationsToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem OptionsToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ReloadWorkspacesToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ResetWindowLocationsToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem KeepInMainWindowToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ApplicationManagerToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem PathCheckerToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ImportToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem MsdebugToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem VariableValidaterToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AutoloadWorkspaceSelectorToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AutoloadWorkspaceToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator6;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator7;
        internal System.Windows.Forms.ToolStripMenuItem DebugCompareToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem DebugLineToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ToSummaryFileToolStripMenuItem;
        internal System.Windows.Forms.FlowLayoutPanel QuickAccessPanel;
        internal System.Windows.Forms.Button ProcessButton;
        internal System.Windows.Forms.Button ReloadButton;
        internal System.Windows.Forms.Button LoadButton;
        internal System.Windows.Forms.Button VariableExplorerButton;
        internal System.Windows.Forms.Button FileHistoryButton;
        internal System.Windows.Forms.Button EventHistoryButton;
        internal System.Windows.Forms.Button SearchButton;
        internal System.Windows.Forms.Button VariableValidatorButton;
        internal System.Windows.Forms.Button PathValidatorButton;

    }
}

// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Windows.Forms;
using Bentley.ConfigurationExplorer.AboutApp;
using WorkspaceCFG.My;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{

    public partial class MainForm
    {

        public CFGConfiguration Configuration;
        public string IconFile;

        private VariableExplorer _variableExplorer;
        private FileHistory _fileHistory;
        private EventHistory _eventHistory;

        public MainForm()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            LocalizeControls();
            LoadSettings();
            SetCfgButtonEnable();
            SetWindowButtonEnable();
        }

        private void LoadSettings()
        {
            KeepInMainWindowToolStripMenuItem.Checked = MySettings.Default.GroupWindows;
            AutoloadWorkspaceSelectorToolStripMenuItem.Checked = MySettings.Default.AutoloadWorkspaceSelector;
            AutoloadWorkspaceToolStripMenuItem.Checked = MySettings.Default.AutoloadLastWorkspace;
        }

        private void LocalizeControls()
        {
            Text = CEResource.TXT_ConfigurationExplorer;

            FileMenu.Text = CEResource.TXT_File;
            NewToolStripMenuItem.Text = CEResource.TXT_NewWorkspace;
            OpenToolStripMenuItem.Text = CEResource.TXT_OpenWorkspace;
            CloseToolStripMenuItem.Text = CEResource.TXT_CloseWorkspace;
            ImportToolStripMenuItem.Text = CEResource.TXT_Import;
            MsdebugToolStripMenuItem.Text = CEResource.TXT_msdebug;
            ToolStripMenuItem2.Text = CEResource.TXT_Export;
            ErrorsToolStripMenuItem.Text = CEResource.TXT_ErrorsTitle;
            ToTextToolStripMenuItem.Text = CEResource.TXT_Errors_ToTextFile;
            VariableListToolStripMenuItem.Text = CEResource.TXT_VariableList;
            ToTextFileToolStripMenuItem1.Text = CEResource.TXT_VariableList_ToTextFile;
            CompiledCFGToolStripMenuItem.Text = CEResource.TXT_CompiledCFG;
            WorkspaceToolStripMenuItem.Text = CEResource.TXT_Configuration;
            ToTextFileToolStripMenuItem.Text = CEResource.TXT_Configuration_ToTextFile;
            FileListToolStripMenuItem.Text = CEResource.TXT_FileList;
            ToTextToolStripMenuItem1.Text = CEResource.TXT_ToText;
            ToExcelToolStripMenuItem.Text = CEResource.TXT_ToExcel;
            SaveToolStripMenuItem.Text = CEResource.TXT_SaveWorkspace;
            ExitToolStripMenuItem.Text = CEResource.TXT_Exit;
            ToolsToolStripMenuItem.Text = CEResource.TXT_Tools;
            VariableExplorerToolStripMenuItem.Text = CEResource.TXT_VariableExplorer;
            FileHistoryToolStripMenuItem.Text = CEResource.TXT_FileHistoryMenu;
            FullHistoryToolStripMenuItem.Text = CEResource.TXT_ConfigurationHistory;
            PathCheckerToolStripMenuItem.Text = CEResource.TXT_PathChecker;
            ToolStripMenuItem1.Text = CEResource.TXT_Search;
            VariableGeneratorToolStripMenuItem.Text = CEResource.TXT_VariableGenerator;
            VariableValidaterToolStripMenuItem.Text = CEResource.TXT_VariableValidater;
            VariableLibraryToolStripMenuItem.Text = CEResource.TXT_VariableLibrary;
            CompareToolStripMenuItem.Text = CEResource.TXT_CompareWorkspaces;
            IconCreatorToolStripMenuItem.Text = CEResource.TXT_ShortcutCreator;
            DebugCompareToolStripMenuItem.Text = "Compare to Export";
            DebugLineToolStripMenuItem.Text = "Parsing Debugger";
            OptionsToolStripMenuItem.Text = CEResource.TXT_Options;
            ReloadWorkspacesToolStripMenuItem.Text = CEResource.TXT_ReloadWorkspaces;
            ResetWindowLocationsToolStripMenuItem.Text = CEResource.TXT_ResetWindowLocations;
            ApplicationManagerToolStripMenuItem.Text = CEResource.TXT_ApplicationManager;
            ViewMenu.Text = CEResource.TXT_View;
            StatusBarToolStripMenuItem.Text = CEResource.TXT_StatusBar;
            WindowsMenu.Text = CEResource.TXT_Windows;
            CascadeToolStripMenuItem.Text = CEResource.TXT_Cascade;
            TileVerticalToolStripMenuItem.Text = CEResource.TXT_TileVertical;
            TileHorizontalToolStripMenuItem.Text = CEResource.TXT_TileHorizontal;
            CloseAllToolStripMenuItem.Text = CEResource.TXT_CloseAll;
            ArrangeIconsToolStripMenuItem.Text = CEResource.TXT_ArrangeIcons;
            ResetLocationsToolStripMenuItem.Text = CEResource.TXT_ResetLocations;
            KeepInMainWindowToolStripMenuItem.Text = CEResource.TXT_GroupInMainWindow;
            HelpMenu.Text = CEResource.TXT_Help;
            ContentsToolStripMenuItem.Text = CEResource.TXT_Contents;
            AboutToolStripMenuItem.Text = CEResource.TXT_AboutMenu;
            CopyToClipboardToolStripMenuItem.Text = CEResource.TXT_CopyToClipboard;
            ViewToolStripMenuItem.Text = CEResource.TXT_InstallAppsView;
            // ToolStripStatusLabel.Text = CEResource.TXT_Status_Ready
            AutoloadWorkspaceSelectorToolStripMenuItem.Text = CEResource.TXT_AutoloadSelectorWorkspace;
            AutoloadWorkspaceToolStripMenuItem.Text = CEResource.TXT_AutoloadWorkspace;

            ProcessButton.Text = "Process\r\nConfiguration";
            ReloadButton.Text = "Reload\r\nConfiguration";
            LoadButton.Text = "Load\r\nConfiguration";
            VariableExplorerButton.Text = VariableExplorerToolStripMenuItem.Text;
            FileHistoryButton.Text = FileHistoryToolStripMenuItem.Text;
            EventHistoryButton.Text = FullHistoryToolStripMenuItem.Text;
            SearchButton.Text = ToolStripMenuItem1.Text;
            VariableValidatorButton.Text = VariableValidaterToolStripMenuItem.Text;
            PathValidatorButton.Text = PathCheckerToolStripMenuItem.Text;
        }

        private void CloseAllForms()
        {
            // Close all child forms of the parent.
            foreach (Form ChildForm in MdiChildren)
            {
                ChildForm.Close();
                ChildForm.Dispose();
            }
        }

        private void SetCfgButtonEnable()
        {
            bool buttonEnabled = Configuration is not null;

            ReloadWorkspacesToolStripMenuItem.Enabled = buttonEnabled;
            VariableExplorerToolStripMenuItem.Enabled = buttonEnabled;
            FileHistoryToolStripMenuItem.Enabled = buttonEnabled;
            FullHistoryToolStripMenuItem.Enabled = buttonEnabled;
            ToolStripMenuItem2.Enabled = buttonEnabled; // Export
            PathCheckerToolStripMenuItem.Enabled = buttonEnabled;
            VariableGeneratorToolStripMenuItem.Enabled = buttonEnabled;
            VariableValidaterToolStripMenuItem.Enabled = buttonEnabled;
            SaveToolStripMenuItem.Enabled = buttonEnabled;
            CloseToolStripMenuItem.Enabled = buttonEnabled;
            ToolStripMenuItem1.Enabled = buttonEnabled; // Search
            DebugCompareToolStripMenuItem.Enabled = buttonEnabled;
            DebugLineToolStripMenuItem.Enabled = true; // TODO: may want to add a setting for this

            ReloadButton.Enabled = buttonEnabled;
            VariableExplorerButton.Enabled = buttonEnabled;
            FileHistoryButton.Enabled = buttonEnabled;
            EventHistoryButton.Enabled = buttonEnabled;
            SearchButton.Enabled = buttonEnabled;
            VariableValidatorButton.Enabled = buttonEnabled;
            PathValidatorButton.Enabled = buttonEnabled;
        }

        private void SetWindowButtonEnable()
        {
            bool buttonEnabled = 0 < MdiChildren.Length;

            CloseAllToolStripMenuItem.Enabled = buttonEnabled;
            ResetLocationsToolStripMenuItem.Enabled = buttonEnabled;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Prompts the user for a new configuration and processes that configuration
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void GetNewConfiguration(ref CFGEnums.Application app)
        {
            ConfigurationSelectorDialog configurationSelectorDialog;
            bool showform = !(!string.IsNullOrEmpty(app.Name) && !string.IsNullOrEmpty(app.Workspace) && !string.IsNullOrEmpty(app.Workset) && !string.IsNullOrEmpty(app.Role));

            if (showform)
            {
                configurationSelectorDialog = new ConfigurationSelectorDialog(ref app);

                if (configurationSelectorDialog.ShowDialog() == DialogResult.OK)
                {
                    Configuration = configurationSelectorDialog.GetConfiguration();
                }
                else
                {
                    return;
                }
            }
            else
            {
                Configuration = Utilities.GetConfiguration(ref app, ref app.Workspace, ref app.Workset, ref app.Role);
            }

            SetCfgButtonEnable();
            InterfaceControler.CloseConfigurations();

            if (Configuration is null)
                return;

            {
                ref var withBlock = ref Configuration;
                withBlock.ShowStatus = true;
                withBlock.ResetStatus();

                InterfaceControler.ConfigurationIsProcessing = true; // controls status echo updates

                withBlock.ProcessConfiguration();

                InterfaceControler.ConfigurationIsProcessing = false; // controls status echo updates
            }

            SetCaption();

            Configuration.IsFromFile = false;
            InterfaceControler.SetConfiguration(ref Configuration, CFGEnums.ConfigurationType.Main);
            InterfaceControler.RefreshForms();
        }

        public void LoadConfigurationFile(string path = "")
        {
            if (string.IsNullOrEmpty(path))
            {
                {
                    var withBlock = OpenFileDialog1;
                    withBlock.Title = CEResource.TXT_TitleOpenWorkSpaceFromFile;
                    withBlock.InitialDirectory = Directory.Exists(MySettings.Default.LastWorkspaceDirectory)
                        ? MySettings.Default.LastWorkspaceDirectory
                        : CEResource.TXT_CDrive;
                    withBlock.Filter = CEResource.TXT_ExtOpenWorkSpaceFilter;
                    withBlock.FileName = CEResource.TXT_ExtWrk;
                    withBlock.RestoreDirectory = true;
                    if (withBlock.ShowDialog() == DialogResult.Cancel)
                        return;

                    path = withBlock.FileName;
                    MySettings.Default.LastWorkspaceDirectory = Path.GetDirectoryName(path) ?? CEResource.TXT_CDrive;
                    withBlock.Dispose();
                }
            }

            CloseAllForms();
            InterfaceControler.CloseConfigurations();
            bool argsilent = false;
            WorkspaceExport.LoadConfiguration(ref path, ref Configuration, silent: ref argsilent);

            if (Configuration is not null)
            {
                Configuration.IsFromFile = true;
                InterfaceControler.SetConfiguration(ref Configuration, CFGEnums.ConfigurationType.Main);
            }

            StatusStrip.Text = CEResource.TXT_ConfigurationOpened;

            SetCaption();

            // Close all child forms of the parent.
            foreach (Form ChildForm in MdiChildren)
                ChildForm.Close();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Opens a workspace from file and builds the status form.
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void OpenFile(object sender, EventArgs e)
        {
            LoadConfigurationFile();
            SetCfgButtonEnable();

            ReloadWorkspacesToolStripMenuItem.Enabled = false;

            if (Configuration is null)
                return;

            InterfaceControler.ShowStatus();
           
            Configuration.UpdateStatus();

            var count = default(int);
            foreach (CFGFile cfile in Configuration.CFGFiles)
                count += cfile.Lines.Count;

            InterfaceControler.StatusDepth = 0;
            InterfaceControler.StatusFile = "Done";
            InterfaceControler.StatusClickable = true;

            InterfaceControler.StatusForm.Workspace = Configuration;
            // set the counter here, as st_linecount currently represent line count of cesettings.upf file only
            InterfaceControler.StatusLineCount = count;
            InterfaceControler.UpdateStatus();
            InterfaceControler.ClearEcho();
        }

        public void SetCaption()
        {
            Text = CEResource.TXT_TitleConfigurationExplorer;
            if (Configuration is not null)
            {
                Text += " - " + Configuration.Description2;
            }
        }

        public void SetGroupWindows(bool grouped)
        {
            MySettings.Default.GroupWindows = grouped;
            MySettings.Default.Save();

            InterfaceControler.GroupWindowsInMainForm = grouped;
            InterfaceControler.Set_Forms_MDIParent(ref grouped);
        }

        private void SetMainFormBackgroundColor()
        {

            // Loop through the form's MdiClient controls.
            foreach (Control ctl in Controls)
            {
                if (ctl is MdiClient)
                {
                    // Set the control's BackColor to match this form's.
                    ctl.BackColor = BackColor;
                }
            }
        }

        // TODO: which version should be used?
        private void ShowAboutDialog()
        {
            // Dim productName = $"{CEResource.TXT_ProductName}{Environment.NewLine}{CEResource.TXT_Version}: {Assembly.GetExecutingAssembly.GetName.Version}"
            // Dim details = $"{CEResource.TXT_Copyright}{Environment.NewLine}{Environment.NewLine}{CEResource.TXT_CopyrightNotice}"
            // RadMessageBox.Show(productName, CEResource.TXT_ProductName, MessageBoxButtons.OK, details)

            var newAboutForm = new AboutForm(Utilities.GetVersionNumber());
            newAboutForm.ShowDialog();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Handles the 'new' button.
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void ShowNewForm(object sender, EventArgs e)
        {
            CFGEnums.Application argapp = default;
            GetNewConfiguration(ref argapp);
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("FileDrop"))
            {
                CFGEnums.Application application;
                string[] files = (string[])e.Data.GetData("FileDrop", true);
                string key;

                foreach (string @file in files)
                {
                    application = Utilities.GetApplicationFromLNK(@file);
                    // create key from app name and exe path
                    key = application.Name + application.EXEPath;
                    if (!string.IsNullOrEmpty(application.Name))
                    {
                        GetNewConfiguration(ref application);
                        return;
                    }
                }
            }
        }

        private void MainForm_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] theFiles = (string[])e.Data.GetData(DataFormats.FileDrop, true);
                foreach (string theFile in theFiles)
                {
                    if (theFile.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase))
                    {
                        e.Effect = DragDropEffects.Copy;
                        return;
                    }
                }
            }
            e.Effect = DragDropEffects.None;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MySettings.Default.Save();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            bool dialogShown = false;

            if (!string.IsNullOrEmpty((IconFile ?? "").Trim()))
            {

                CFGEnums.Application app;
                app = Utilities.GetApplicationFromLNK(IconFile);
                if (!string.IsNullOrEmpty(app.Name))
                {
                    GetNewConfiguration(ref app);
                    dialogShown = true;
                }
                IconFile = "";
            }
            SetMainFormBackgroundColor();

            // Load process configuration dialog at startup, if not already shown
            if (!dialogShown && MySettings.Default.AutoloadWorkspaceSelector == true)
            {
                CFGEnums.Application argapp = default;
                GetNewConfiguration(ref argapp);
            }
        }
        #region Control Events

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StatusStrip.Visible = StatusBarToolStripMenuItem.Checked;
        }

        private void AutoloadWorkspaceSelectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MySettings.Default.AutoloadWorkspaceSelector = AutoloadWorkspaceSelectorToolStripMenuItem.Checked;
        }

        private void AutoloadWorkspaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MySettings.Default.AutoloadLastWorkspace = AutoloadWorkspaceToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Close all child forms of the parent.
            foreach (Form ChildForm in MdiChildren)
                ChildForm.Close();
            SetWindowButtonEnable();
        }

        private void VariableExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open Variable Explorer from the main form

            if (Configuration is null)
                return;
            _variableExplorer = new VariableExplorer(CFGEnums.ConfigurationType.Main);

            if (InterfaceControler.GroupWindowsInMainForm)
                _variableExplorer.MdiParent = this;
            _variableExplorer.Show();
            SetWindowButtonEnable();
        }

        private void FileExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open File History from the main form

            if (Configuration is null)
                return;
            _fileHistory = new FileHistory(CFGEnums.ConfigurationType.Main);

            if (InterfaceControler.GroupWindowsInMainForm)
                _fileHistory.MdiParent = this;
            _fileHistory.Show();
            SetWindowButtonEnable();
        }

        private void FullHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open Workspace History from the main form

            if (Configuration is null)
                return;
            _eventHistory = new EventHistory(CFGEnums.ConfigurationType.Main);

            {
                ref var withBlock = ref _eventHistory;
                withBlock.IgnoreBuild = true;
                withBlock.CheckBox1.Checked = true;
                withBlock.CheckBox2.Checked = true;
                withBlock.CheckBox3.Checked = true;
                withBlock.CheckBox4.Checked = true;
                withBlock.CheckBox4.Checked = true;
                withBlock.ComboBox1.Text = "";
                withBlock.IgnoreBuild = false;
                if (InterfaceControler.GroupWindowsInMainForm)
                    withBlock.MdiParent = this;
                withBlock.Show();
            }
            SetWindowButtonEnable();
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Open Search from the main form

            var srch = new Search(CFGEnums.ConfigurationType.Main);

            if (InterfaceControler.GroupWindowsInMainForm)
                srch.MdiParent = this;
            srch.Show();
            SetWindowButtonEnable();
        }

        private void VariableGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open Variable Generator from the main form
            if (Configuration is null)
                return;

            var vargen = new VariableGenerator(CFGEnums.ConfigurationType.Main);

            if (InterfaceControler.GroupWindowsInMainForm)
                vargen.MdiParent = this;
            vargen.Show();
            SetWindowButtonEnable();
        }

        private void VariableLibraryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VariableLibrary variableLibrary;

            if (Configuration is null)
            {
                variableLibrary = new VariableLibrary(CFGEnums.ConfigurationType.Unknown);
            }
            else
            {
                variableLibrary = new VariableLibrary(CFGEnums.ConfigurationType.Main);
            }


            if (InterfaceControler.GroupWindowsInMainForm)
                variableLibrary.MdiParent = this;
            variableLibrary.Show();
            SetWindowButtonEnable();
        }

        private void CompareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ConfigurationCompare();



            if (InterfaceControler.GroupWindowsInMainForm)
                form.MdiParent = this;

            form.Show();
            SetWindowButtonEnable();
        }

        private void IconCreatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ShortcutCreator();
            form.ShowDialog();

            SetWindowButtonEnable();
        }

        private void ViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new InstalledApps();
            form.ShowDialog();
            form.Dispose();

            SetWindowButtonEnable();
        }

        private void CopyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Utilities.GetBentleyApps());
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAboutDialog();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path;

            if (Configuration is null)
                return;

            {
                var withBlock = SaveFileDialog1;
                withBlock.Title = CEResource.TXT_MsgSaveWorkspaceToFile;
                withBlock.InitialDirectory = CEResource.TXT_CDrive;
                withBlock.Filter = CEResource.TXT_ExtOpenWorkSpaceFilter;
                withBlock.FileName = Configuration.ApplicationData.Name + "-" + Configuration.ApplicationData.Workspace + "-" + Configuration.ApplicationData.Workset + ".bcf";
                withBlock.RestoreDirectory = true;
                if (withBlock.ShowDialog() == DialogResult.Cancel)
                    return;

                path = withBlock.FileName;
                withBlock.Dispose();
            }

            bool argsilent = false;
            WorkspaceExport.SaveWorkspace(ref path, ref Configuration, silent: ref argsilent);

            StatusStrip.Text = CEResource.TXT_MsgWorkspaceSaved;
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Close the current workspace

            Configuration = null;
            InterfaceControler.CloseConfigurations();

            CloseAllForms();

            // Clear out the title
            SetCaption();

            SetCfgButtonEnable();
        }

        private void ToTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkspaceExport.SaveWorkspaceToTextFile(ref Configuration);
        }

        private void ToTextToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            WorkspaceExport.SaveWorkspaceToTextFile(ref Configuration);
        }

        private void ToTextFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            WorkspaceExport.SaveWorkspaceVariables(ref Configuration);
        }

        private void CompiledCFGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkspaceExport.SaveCompiledWorkspace(ref Configuration);
        }

        private void ToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utilities.ExportFileList(ref Configuration);
        }

        private void ToTextFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkspaceExport.SaveWorkspaceToTextFile(ref Configuration);
        }

        private void ToSummaryFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExcelUtilities.ExportFileList2(ref Configuration);
        }

        private void ResetLocationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InterfaceControler.ResetFormLocations();
        }

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveToolStripMenuItem_Click(null, null);
        }

        private void OpenCEsettingsCFGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainType.CESettings.OpenSettingsFile();
        }

        private void ReloadWorkspacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InterfaceControler.RefreshOpenForms();
        }

        private void ReloadSettingsConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StatusStrip.Text = CEResource.TXT_MsgSettingsReloaded;
        }

        private void ResetWindowLocationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InterfaceControler.ResetFormLocations();
        }

        private void WatchF5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                InterfaceControler.RefreshOpenForms();
            }
        }

        private void KeepInMainWindowToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            SetGroupWindows(KeepInMainWindowToolStripMenuItem.Checked);
        }

        private void ApplicationManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new AppWizard();

            if (InterfaceControler.GroupWindowsInMainForm)
                form.MdiParent = this;

            form.Show();
        }

        private void PathCheckerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open Path Checker from the main form
            if (Configuration is null)
                return;

            var pathchk = new PathChecker(CFGEnums.ConfigurationType.Main);

            if (InterfaceControler.GroupWindowsInMainForm)
                pathchk.MdiParent = this;
            pathchk.Show();
            SetWindowButtonEnable();
        }

        private void DebugCompareButton_Click(object sender, EventArgs e)
        {
            string path;

            string intialTmpPath;

            var rootVar = Configuration.GetVariable("_ROOTDIR");
            var appVar = Configuration.GetVariable("_USTN_PRODUCT_DIRNAME");
            string root = "";
            string app = "";

            if (rootVar is not null)
                root = rootVar.FinalExpansion;
            if (appVar is not null)
                app = appVar.FinalExpansion;

            intialTmpPath = UtilitiesPath.GetSystemApplicationDir("%TEMP%", app, root);
            if (string.IsNullOrEmpty(intialTmpPath))
            {
                intialTmpPath = CEResource.TXT_CDrive;
            }

            {
                var withBlock = OpenFileDialog1;
                withBlock.Title = CEResource.TXT_MsgOpenMsdebugFile;
                withBlock.InitialDirectory = intialTmpPath;
                withBlock.Filter = CEResource.TXT_OpenMsdebugFileFilter;
                withBlock.FileName = CEResource.TXT_LabelVersionHistoryTxt;
                withBlock.RestoreDirectory = true;
                if (withBlock.ShowDialog() == DialogResult.Cancel)
                    return;

                path = withBlock.FileName;
                withBlock.Dispose();
            }

            var debugFile = new DebugFile();
            debugFile.LoadFile(path);
            debugFile.Compare(Configuration);
        }

        private void DebugLineButton_Click(object sender, EventArgs e)
        {
            var debugForm = new DebugForm();
            debugForm.Show();
        }

        private void MsdebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path;

            {
                string intialTmpPath;

                var rootVar = Configuration.GetVariable("_ROOTDIR");
                var appVar = Configuration.GetVariable("_USTN_PRODUCT_DIRNAME");
                string root = "";
                string app = "";

                if (rootVar is not null)
                    root = rootVar.FinalExpansion;
                if (appVar is not null)
                    app = appVar.FinalExpansion;

                intialTmpPath = UtilitiesPath.GetSystemApplicationDir("%TEMP%", app, root);
                if (string.IsNullOrEmpty(intialTmpPath))
                {
                    intialTmpPath= CEResource.TXT_CDrive;
                }


                var withBlock = OpenFileDialog1;
                withBlock.Title = CEResource.TXT_MsgOpenMsdebugFile;
                withBlock.InitialDirectory = intialTmpPath;
                withBlock.Filter = CEResource.TXT_OpenMsdebugFileFilter;
                withBlock.FileName = CEResource.TXT_LabelVersionHistoryTxt;
                withBlock.RestoreDirectory = true;
                if (withBlock.ShowDialog() == DialogResult.Cancel)
                    return;

                path = withBlock.FileName;
                withBlock.Dispose();
            }

            CloseAllForms();
            InterfaceControler.CloseConfigurations();

            Configuration = DebugScan.LoadMsDebug(ref path);

            if (Configuration is not null)
            {
                Configuration.IsFromFile = true;
                InterfaceControler.SetConfiguration(ref Configuration, CFGEnums.ConfigurationType.Main);
            }

            StatusStrip.Text = CEResource.TXT_LabelWorkspaceOpened;

            SetCaption();

            // Close all child forms of the parent.
            foreach (Form ChildForm in MdiChildren)
                ChildForm.Close();

            InterfaceControler.ShowStatus();
            CFGFile argfile = null;
            Configuration.UpdateStatus(file: argfile);

            InterfaceControler.StatusDepth = 0;
            InterfaceControler.StatusFile = CEResource.TXT_MsgDone;
            InterfaceControler.StatusClickable = true;
            InterfaceControler.StatusForm.Workspace = Configuration;
            InterfaceControler.UpdateStatus();
            InterfaceControler.ClearEcho();
        }

        private void VariableValidatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open Variable Generator from the main form
            if (Configuration is null)
                return;

            var varval = new VariableValidater(CFGEnums.ConfigurationType.Main);

            if (InterfaceControler.GroupWindowsInMainForm)
                varval.MdiParent = this;
            varval.Show();
            SetWindowButtonEnable();
        }

        private void ContentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utilities.OpenHelpPDF();
        }

        private void HelpToolStripButton_Click(object sender, EventArgs e)
        {
            Utilities.OpenHelpPDF();
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            SetWindowButtonEnable();
        }

        #endregion

    }
}
// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WorkspaceCFG.My;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{

    public partial class FileHistory
    {

        // ---------------------------------------------------------------------------------------
        // Private Members
        // ---------------+---------------+---------------+---------------+---------------+-------
        private int _modifiedHours;
        private CFGConfiguration _workspace;
        private CFGFile _currentFile;
        private CFGEnums.ConfigurationType _workspaceType;
        private int _currentline;
        private bool _buildingMode;
        private int _selectionStart;
        private string _selectedFilepath;
        private TreeNode _lastNode;

        // ---------------------------------------------------------------------------------------
        // @description: Initializes members
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static bool NeedsRefreshed = false;

        public FileHistory(CFGEnums.ConfigurationType inwrkType)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            _buildingMode = true;
            _workspaceType = inwrkType;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Clear out collections and arrays
        // ---------------+---------------+---------------+---------------+---------------+-------
        ~FileHistory()
        {
            _workspace = null;
            _workspaceType = default;
            _currentFile = null;
            LineNumberPictureBox.Dispose();
            LineDescriptionRTB.Dispose();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Form Load event; reads settings and fills it with data.
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void FileHistory_Load(object sender, EventArgs e)
        {
            object argform = this;
            InterfaceControler.AddForm(ref argform);
            Location = MySettings.Default.FH_Location;
            WindowState = MySettings.Default.FH_WindowState;
            if (WindowState == System.Windows.Forms.FormWindowState.Normal)
            {
                Size = MySettings.Default.FH_Size;
                PerformLayout();
            }
            _modifiedHours = MainType.CESettings.HighlightModifiedInterval;
            RefreshForm();
            _selectionStart = 0;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Form close event: saves settings
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void FileHistory_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            PromtSaveChanges();
            object argform = this;
            InterfaceControler.RemoveForm(ref argform);
            MySettings.Default.FH_Location = Location;
            MySettings.Default.FH_WindowState = WindowState;
            // Settings.VC_Split3_Dist = Me.SplitContainer3.SplitterDistance
            // Me.SplitContainer3.SplitterDistance = Settings.VC_Split3_Dist
            if (WindowState == System.Windows.Forms.FormWindowState.Normal)
                MySettings.Default.FH_Size = Size;
            MySettings.Default.Save();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Fills the form with data
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void RefreshForm()
        {
            _buildingMode = true;

            InterfaceControler.GetConfiguration(ref _workspace, _workspaceType);
            if (_workspace is null)
                return;

            // Set form caption
            Text = CEResource.TXT_TitleFileHistory + _workspace.Description2;

            hsModifedAgo.Value = (int)Math.Round(_modifiedHours * 0.4d);

            BuildTree();

            _buildingMode = false;

            ConfigFileRTB.SelectionStart = _selectionStart;
            UpdateLineNumber();
            UpdateSaveSatus();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Updates the node colors on the tree
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void UpdateTreeColor()
        {
            System.Windows.Forms.TreeNode[] nodes;

            foreach (CFGFile cfgFile in _workspace.CFGFiles)
            {
                nodes = TreeView1.Nodes.Find(cfgFile.UID.ToString(), true);
                if (nodes is not null && nodes.Length > 0)
                    SetNodeColor(ref nodes[0], cfgFile);
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Builds the file tree
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void BuildTree()
        {
            System.Windows.Forms.TreeNode node;
            System.Windows.Forms.TreeNode nodes;

            if (_workspace is null)
                return;

            {
                var withBlock = TreeView1;
                withBlock.BeginUpdate();
                withBlock.Nodes.Clear();
                foreach (CFGFile cfgfile in _workspace.CFGFiles)
                {
                    if (cfgfile.Depth == 0)
                    {
                        //node = withBlock.Nodes.Add(cfgfile.UID.ToString(), cfgfile.Name + " - " + cfgfile.UID  );
                        node = withBlock.Nodes.Add(cfgfile.UID.ToString(), cfgfile.Name);
                        if (CultureInfo.CurrentCulture.CompareInfo.Compare(cfgfile.FilePath ?? "", _selectedFilepath ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                            _lastNode = node;
                        foreach (CFGFile child in cfgfile.ChildFiles)
                        {
                            //nodes = node.Nodes.Add(child.UID.ToString(), child.Name + " - " + child.UID);
                            nodes = node.Nodes.Add(child.UID.ToString(),  child.Name);
                            if (CultureInfo.CurrentCulture.CompareInfo.Compare(cfgfile.FilePath ?? "", _selectedFilepath ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                                _lastNode = nodes;
                            Add_Child_Node(child, ref nodes);
                        }
                    }
                }

                UpdateTreeColor();

                // Select a node in the tree
                if (!string.IsNullOrEmpty(_selectedFilepath) && _lastNode is not null)
                {
                    withBlock.SelectedNode = _lastNode;
                }
                else if (withBlock.Nodes.Count > 0)
                {
                    if (withBlock.Nodes.Count > 1)
                    {
                        withBlock.SelectedNode = withBlock.Nodes[1];
                    }
                    else
                    {
                        withBlock.SelectedNode = withBlock.Nodes[0];
                    }
                }
                withBlock.ExpandAll();
                withBlock.EndUpdate();
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Add child nodes to the file tree
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void Add_Child_Node(CFGFile cfile, ref System.Windows.Forms.TreeNode node)
        {
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(cfile.FilePath ?? "", _selectedFilepath ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                _lastNode = node;
            foreach (CFGFile child in cfile.ChildFiles)
            {
                //var argnode = node.Nodes.Add(child.UID.ToString(), child.Name + " - " + child.UID);
                var argnode = node.Nodes.Add(child.UID.ToString(), child.Name);
                Add_Child_Node(child, ref argnode);
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Sets the color on a node in the tree based on its modified date
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void SetNodeColor(ref System.Windows.Forms.TreeNode node, CFGFile cfile)
        {
            if (node is null)
                return;

            // Replace DateAndTime.DateDiff(DateInterval.Hour, ...) with (DateTime.Now - cfile.DateLastModified).TotalHours
            if ((DateTime.Now - cfile.DateLastModified).TotalHours < _modifiedHours)
            {
                if (node.ForeColor != Color.Blue)
                {
                    node.ForeColor = Color.Blue;
                    node.NodeFont = new Font(node.TreeView.Font, FontStyle.Bold);
                    node.Text = (node.Text ?? "").Trim() + "      ";
                }
            }
            else if (node.ForeColor != Color.Black)
            {
                node.ForeColor = Color.Black;
                node.NodeFont = new Font(node.TreeView.Font, FontStyle.Regular);
            }

            if (ConfigFileRTB.NeedsToBeSaved)
            {
                if (CultureInfo.CurrentCulture.CompareInfo.Compare(cfile.FilePath ?? "", _currentFile.FilePath ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                {
                    node.ForeColor = Color.DarkOrange;
                    node.NodeFont = new Font(node.TreeView.Font, FontStyle.Bold);
                    node.Text = (node.Text ?? "").Trim() + "      ";
                }
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Fills the form with data about the current select file
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void FillForm(int uid)
        {
            if (_workspace is null)
                return;

            _buildingMode = true;

            CFGFile cfile = null;
            int t;

            // Find the file with the given ID
            var loopTo = _workspace.CFGFiles.Count - 1;
            for (t = 0; t <= loopTo; t++)
            {
                if (((dynamic)_workspace.CFGFiles[t])?.UID == uid)

                {

                    cfile = (CFGFile)_workspace.CFGFiles[t];
                    break;

                }
            }

                if (cfile is null)
                    return;

                _currentFile = cfile;

                {
                    ref var withBlock = ref cfile;
                    var argll = LinkLabel1;
                    BuildLinkLabel(argll, withBlock.FilePath);
                    LinkLabel1 = argll;

                    Label13.Text = Utilities.GetLevelExtendedName(withBlock.StartLevel);
                    if (withBlock.ParentFile is not null)
                    {
                        var argll1 = LinkLabel4;
                        BuildLinkLabel(argll1, withBlock.ParentFile.FilePath);
                        LinkLabel4 = argll1;
                    }
                    else
                    {
                        var argll2 = LinkLabel4;
                        string argmsg = CEResource.TXT_MsgNoParentFile;
                        BuildLinkLabel(argll2, argmsg);
                        LinkLabel4 = argll2;
                    }
                    if (NeedsRefreshed)
                    {
                        Label7.Text = "*";
                    }
                    else
                    {
                        Label7.Text = withBlock.DateLastModified.ToString();
                    }

                    FillLineCount();
                    FillErrorWarningCount();
                    ConfigFileRTB.ReadOnly = false;

                    if (_workspace.IsFromFile)
                    {
                        ConfigFileRTB.LoadCFGFile(ref cfile);
                        ConfigFileRTB.ReadOnly = true;
                        Text = CEResource.TXT_TitleFileHistoryReadOnly + _workspace.Description2;
                    }
                    else
                    {
                        ConfigFileRTB.LoadCFGFile(ref cfile.FilePath, ref cfile);
                    }

                    _currentline = 1;
                    LineNumberPictureBox.Invalidate();
                }

                ConfigFileRTB.NeedsToBeSaved = false;

                UpdateSaveSatus();

                _selectedFilepath = cfile.FilePath;

                _buildingMode = false;

        }

        private string GetFullPathByName(int uid)
        {
            if (_workspace is null)
                return "";

            CFGFile cfile = null;
            int t;

            // Find the file with the given ID
            var loopTo = _workspace.CFGFiles.Count - 1;
            for (t = 0; t <= loopTo; t++)
            {
                if (((dynamic)_workspace.CFGFiles[t])?.UID == uid)

                {

                    cfile = (CFGFile)_workspace.CFGFiles[t];
                    break;

                }
            }
            return cfile.FilePath;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Truncates a linklabel based on its size
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void BuildLinkLabel(LinkLabel ll, string msg)
        {
            if (ll == null || string.IsNullOrEmpty(msg))
                return;

            int labelWidth = ll.Width;

            ll.Text = msg;

            // Estimate average character width and check if text fits
            double avgCharWidth = 7.0;
            if ((labelWidth / (double)msg.Length) > avgCharWidth)
                return;

            // Calculate how many characters can fit
            int maxVisibleChars = (int)Math.Round(labelWidth / avgCharWidth) - 3;
            if (msg.Length <= maxVisibleChars)
                return;

            // Truncate and prepend ellipsis
            string truncated = msg.Substring(msg.Length - maxVisibleChars);
            ll.Text = "..." + truncated.Substring(3);
        }


        // ---------------------------------------------------------------------------------------
        // @description: Counts the number of usable lines in a cfg file, sets the label on the form
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void FillLineCount()
        {
            int count = 0;

            if (_currentFile is null)
                return;

            if (NeedsRefreshed)
            {
                Label9.Text = "*";
                return;
            }

            foreach (CFGLine line in _currentFile.Lines)
            {
                if (line.IsComment == false && !string.IsNullOrEmpty(line.Text))
                    count += 1;
            }

            Label9.Text = count + " out of " + _currentFile.Lines.Count;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Counts the number of errors in a cfg file, sets the labels on the form
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void FillErrorWarningCount()
        {
            int ecount = 0;
            int wcount = 0;

            if (NeedsRefreshed)
            {
                LinkLabel2.Text = "*";
                LinkLabel3.Text = "*";
                return;
            }

            foreach (CFGEvent eve in _workspace.CFGEvents)
            {
                if (eve.ParentFile is not null && eve.ParentFile.UID == _currentFile.UID)
                {
                    if (eve.EventType == CFGEnums.CFGEventType.cfgError || eve.EventType == CFGEnums.CFGEventType.cfgCritical)
                        ecount += 1;
                    if (eve.EventType == CFGEnums.CFGEventType.cfgWarning)
                        wcount += 1;
                }
            }

            LinkLabel2.Text = ecount.ToString();
            LinkLabel3.Text = wcount.ToString();
        }





        // ---------------------------------------------------------------------------------------
        // @description: Handles a file being selected in the tree
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (int.TryParse(TreeView1.SelectedNode.Name, out int nodeId))
            {
                FillForm(nodeId);

                // --- Add this block to update the CFGTree ---
                // Find the open CFGTree form
                var cfgTreeForm = Application.OpenForms
                    .Cast<Form>()
                    .FirstOrDefault(f => f is CFGTree) as CFGTree;

                if (cfgTreeForm != null && _currentFile != null)
                {
                    // Highlight or select the file in CFGTree
                    cfgTreeForm.HighlightFiles(new List<string> { _currentFile.FilePath });
                }
                // -------------------------------------------
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Shows error events for the selected file
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void LinkLabel2_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            if (_workspace is null)
                return;

            var EH = new EventHistory(_workspaceType);
            EH.IgnoreBuild = true;
            EH.CheckBox1.Checked = true;
            EH.CheckBox2.Checked = false;
            EH.CheckBox3.Checked = false;
            EH.CheckBox4.Checked = false;
            EH.CheckBox4.Checked = false;
            EH.ComboBox1.Text = _currentFile.FilePath;
            EH.IgnoreBuild = false;
            if (InterfaceControler.GroupWindowsInMainForm)
                EH.MdiParent = MdiParent;
            EH.Show();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Shows warning events for the selected file
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void LinkLabel3_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            if (_workspace is null)
                return;

            var EH = new EventHistory(_workspaceType);
            EH.IgnoreBuild = true;
            EH.CheckBox1.Checked = false;
            EH.CheckBox2.Checked = true;
            EH.CheckBox3.Checked = false;
            EH.CheckBox4.Checked = false;
            EH.CheckBox4.Checked = false;
            EH.ComboBox1.Text = _currentFile.FilePath;
            EH.IgnoreBuild = false;
            if (InterfaceControler.GroupWindowsInMainForm)
                EH.MdiParent = MdiParent;
            EH.Show();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Opens the selected file in the cfg viewer
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void LinkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            if (_workspace is null)
                return;
            if (_currentFile is null)
                return;

            Utilities.OpenInEditor(ref _currentFile.FilePath);
        }

        // ---------------------------------------------------------------------------------------
        // @description: Opens the selected file's parent file in the cfg viewer
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void LinkLabel4_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            if (_workspace is null)
                return;
            if (_currentFile is null)
                return;
            if (_currentFile.ParentFile is null)
                return;

            Utilities.OpenInEditor(ref _currentFile.ParentFile.FilePath);
        }

        // ---------------------------------------------------------------------------------------
        // @description: Opens the selected file in the cfg viewer
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void OpenInViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileViewer fview;

            if (_workspace is null)
                return;
            if (_currentFile is null)
                return;

            fview = new FileViewer(CFGEnums.ConfigurationType.Main, ref _currentFile.FilePath);
            fview.CheckBox1.Checked = true;
            if (InterfaceControler.GroupWindowsInMainForm)
                fview.MdiParent = MdiParent;
            fview.Show();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Opens the selected file in the default editor
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void OpenInEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentFile is null)
                return;
            if (File.Exists(_currentFile.FilePath) == false)
            {

                string strMsg = string.Format(CEResource.TXT_MsgFileNotFound, _currentFile.FilePath);

                System.Windows.Forms.MessageBox.Show(strMsg, CEResource.TXT_TitleFileNotFound);
                return;
            }

            Utilities.OpenInEditor(ref _currentFile.FilePath);
        }
        // ---------------------------------------------------------------------------------------
        // @description: Opens the event viewer for the selected file
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void ShowEventsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_workspace is null)
                return;

            var EH = new EventHistory(_workspaceType);
            EH.IgnoreBuild = true;
            EH.CheckBox1.Checked = true;
            EH.CheckBox2.Checked = true;
            EH.CheckBox3.Checked = true;
            EH.CheckBox4.Checked = true;
            EH.CheckBox4.Checked = true;
            EH.ComboBox1.Text = _currentFile.FilePath;
            EH.IgnoreBuild = false;
            if (InterfaceControler.GroupWindowsInMainForm)
                EH.MdiParent = MdiParent;
            EH.Show();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Rescans the workspace and refreshes all forms when F5 is pressed
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void WatchF5_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.F5)
            {
                InterfaceControler.RefreshOpenForms();
                Focus();
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Exports the variable list to excel
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void ToExcelToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string path;

            if (_workspace is null)
                return;

            {
                var withBlock = SaveFileDialog1;
                withBlock.Title = CEResource.TXT_MsgSaveVariableListToExcel;
                withBlock.InitialDirectory = CEResource.TXT_CDrive;
                withBlock.Filter = CEResource.TXT_ExtSaveWorkSpaceFilter;
                withBlock.FileName = _workspace.ApplicationData.Name + "-" + _workspace.ApplicationData.Workspace + "-" + _workspace.ApplicationData.Workset + ".xls";
                withBlock.RestoreDirectory = true;
                if (withBlock.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    return;

                path = withBlock.FileName;
                withBlock.Dispose();
            }

            System.Windows.Forms.MessageBox.Show(CEResource.TXT_MsgFileExportComplete, CEResource.TXT_MsgDone);
        }

        // ---------------------------------------------------------------------------------------
        // @description: Opens every file in the editor
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void OpenAllInEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_workspace is null)
                return;

            System.Windows.Forms.DialogResult result;
            string strMsg = string.Format(CEResource.TXT_MsgOpenAllConfFiles, _workspace.CFGFiles.Count);

            result = System.Windows.Forms.MessageBox.Show(strMsg, CEResource.TXT_MsgOpenAllInEditor, System.Windows.Forms.MessageBoxButtons.OKCancel);

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                foreach (CFGFile cfile in _workspace.CFGFiles)
                    Utilities.OpenInEditor(ref cfile.FilePath, false);
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Handles the modified date scroll bar, updates the tree view coloring
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void HScrollBar1_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            // Replace Conversion.Int with Math.Floor
            _modifiedHours = (int)Math.Round(Math.Floor(hsModifedAgo.Value * 168.0 / 50.0));

            if (_modifiedHours < 1)
                _modifiedHours = 1;

            if (_modifiedHours == 1)
            {
                lblModifiedAgo.Text = CEResource.TXT_MsgHighliteFilesModifiedInThLastHour;
            }
            else
            {
                lblModifiedAgo.Text = string.Format(CEResource.TXT_MsgHighliteFilesModifiedInThLastHour, _modifiedHours);
            }

            UpdateTreeColor();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Export the tree view to an image file
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void ToImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ContextMenuStrip1.Close();
            TreeView1.Refresh();
            object argfrm = TreeView1;
            Utilities.SaveControlImage(ref argfrm);
            TreeView1 = (System.Windows.Forms.TreeView)argfrm;

            // TODO: replace the old version
            // Utilities.SaveBitmapToFile(Utilities.GetControlScreenShot(TreeView1))
        }

        // ---------------------------------------------------------------------------------------
        // @description: Opens the selected files include event in a cfg viewer
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void OpenIncludeEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentFile is null)
                return;
            if (_currentFile.ParentFile is null)
                return;

            int pid;

            pid = _currentFile.ParentFile.UID;

            foreach (CFGEvent eve in _workspace.CFGEvents)
            {
                if (eve.EventType == CFGEnums.CFGEventType.cfgInclude)
                {
                    if (eve.ParentFile is not null)
                    {
                        if (eve.ParentFile.UID == pid)
                        {
                            // Replace Strings.InStr with .Contains
                            if (!string.IsNullOrEmpty(eve.Description) && eve.Description.IndexOf(_currentFile.Name, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                Utilities.OpenInEditor(ref _currentFile.ParentFile.FilePath, false, eve.ParentLine.LineNumber);
                                return;
                            }
                        }
                    }
                }
            }
        }
        // ---------------------------------------------------------------------------------------
        // @description: Exports the file list to a textfile
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void ToTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utilities.ExportFileList(ref _workspace);
        }

        // ---------------------------------------------------------------------------------------
        // @description: Exports the file list to an Excel file
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void ToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utilities.ExportFileList(ref _workspace, "excel");
        }

        // ---------------------------------------------------------------------------------------
        // @description: Forces a node in the tree view to be selected on a right mouse click
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void TreeView1_NodeMouseClick(object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e)
        {
            TreeView1.SelectedNode = e.Node;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Opens explorer to the selected file's containing folder.
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void OpenContainingFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utilities.OpenContainingFolder(ref _currentFile.FilePath);
        }


        private void ConfigFileRTB_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            long ln;
            ln = ConfigFileRTB.GetLineFromCharIndex(ConfigFileRTB.SelectionStart) + 1;
            _selectionStart = ConfigFileRTB.SelectionStart;

            if (ln != _currentline)
            {
                _currentline = (int)ln;
                LineNumberPictureBox.Invalidate();
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Handles the mouse click event on the cfg file Richtextbox
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void RichTextBox1_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            UpdateLineNumber();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Handles the mouse double click event on the cfg file Richtextbox
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void RichTextBox1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            string variablename;
            CFGVariable @var;

            var argRTB = ConfigFileRTB;
            variablename = GetVariableFromRTB(argRTB, e.X, e.Y, true);
            ConfigFileRTB = argRTB;

            @var = _workspace.GetVariable(variablename);
            if (@var is not null)
            {
                var vdos = new VariableDossier(CFGEnums.ConfigurationType.Main, ref variablename);
                vdos.Show();
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Returns a variable name from richtext box based on the mouse location
        // ---------------+---------------+---------------+---------------+---------------+-------
        private string GetVariableFromRTB(CFGRichTextBox RTB, int x, int y, bool expand_selection = false)
        {
            int ps;
            int maxp;
            int minp;
            string variablename;

            ps = RTB.GetCharIndexFromPosition(new Point(x, y));

            maxp = ps;
            minp = ps;

            bool localIsValidChar() { var tmp = RTB.Text; char argc = tmp[maxp]; var ret = IsValidChar(ref argc); return ret; }

            while (maxp < RTB.TextLength - 1 && localIsValidChar())
                maxp += 1;

            bool localIsValidChar1() { var tmp1 = RTB.Text; char argc1 = tmp1[minp]; var ret = IsValidChar(ref argc1); return ret; }

            while (minp >= 0 && localIsValidChar1())
                minp -= 1;
            if (maxp - minp - 1 <= 0)
                return "";

            // Replace Strings.Mid and Strings.Trim with Substring and Trim
            variablename = (RTB.Text.Substring(minp + 1, maxp - minp - 1)).Trim();

            // Replace Strings.Replace and Strings.Trim with Replace and Trim
            if (string.IsNullOrEmpty(variablename.Replace("\t", " ").Trim()))
                return "";
            if (minp + 1 >= RTB.TextLength)
                return "";
            if (expand_selection)
            {
                RTB.SelectionStart = minp + 1;
                RTB.SelectionLength = maxp - minp - 1;
            }

            return variablename;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Returns true if c is an allowable character in a variable name
        // ---------------+---------------+---------------+---------------+---------------+-------
        private bool IsValidChar(ref char c)
        {
            return char.IsDigit(c) ||
                (c >= 'A' && c <= 'Z') ||
                (c >= 'a' && c <= 'z') ||
                c == '_';
        }

        private void UpdateSaveSatus()
        {
            TreeView1.HideSelection = ConfigFileRTB.NeedsToBeSaved;
            TreeView1.Enabled = ConfigFileRTB.NeedsToBeSaved;
            UpdateTreeColor();
            TreeView1.Enabled = !ConfigFileRTB.NeedsToBeSaved;
            Label5.Text = CEResource.TXT_MsgConfigurationFileSaveChanges;
            btnSave.Enabled = ConfigFileRTB.NeedsToBeSaved;
            btnCancel.Enabled = ConfigFileRTB.NeedsToBeSaved;

            if (ConfigFileRTB.NeedsToBeSaved)
            {
                Label5.Text = CEResource.TXT_MsgConfigurationFileSaveChanges;
            }
            else
            {
                Label5.Text = CEResource.TXT_MsgConfigurationFile;
            }
        }

        private void UpdateLineNumber()
        {
            _currentline = ConfigFileRTB.GetLineFromCharIndex(ConfigFileRTB.SelectionStart) + 1;
            _selectionStart = ConfigFileRTB.SelectionStart;
            BuildDesription(ref _currentline);
            LineNumberPictureBox.Invalidate();
        }

        public void PromtSaveChanges()
        {
            if (!ConfigFileRTB.NeedsToBeSaved)
                return;
            System.Windows.Forms.DialogResult result;

            result = System.Windows.Forms.MessageBox.Show(string.Format(CEResource.TXT_MsgSaveChangesTo, _currentFile.Name), CEResource.TXT_MsgSaveChanges, System.Windows.Forms.MessageBoxButtons.YesNo);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                FileWatcher.SetEnable(false);
                ConfigFileRTB.SaveCFGFile();
                FileWatcher.SetEnable(true);
            }
            else
            {
                ConfigFileRTB.NeedsToBeSaved = false;
            }

            UpdateSaveSatus();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Forces the cfg richtextbox background color to stay white even when set to readonly
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void RichTextBox1_ReadOnlyChanged(object sender, EventArgs e)
        {
            ConfigFileRTB.BackColor = Color.White;
        }

        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Utilities.OpenInEditor(ref _currentFile.FilePath, line: _currentline);
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Utilities.OpenContainingFolder(ref _currentFile.FilePath);
        }

        // ---------------------------------------------------------------------------------------
        // @description: Redraws the current file linklabel when its size is changed
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void LinkLabel1_SizeChanged(object sender, EventArgs e)
        {
            if (_currentFile is null)
                return;

            var argll = LinkLabel1;
            BuildLinkLabel(argll, _currentFile.FilePath);
            LinkLabel1 = argll;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Redraws the parent file linklabel when its size is changed
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void LinkLabel4_SizeChanged(object sender, EventArgs e)
        {
            if (_currentFile is null)
                return;

            if (_currentFile.ParentFile is not null)
            {
                var argll = LinkLabel4;
                BuildLinkLabel(argll, _currentFile.ParentFile.FilePath);
                LinkLabel4 = argll;
            }
            else
            {
                var argll1 = LinkLabel4;
                string argmsg = CEResource.TXT_MsgNoParentFile;
                BuildLinkLabel(argll1, argmsg);
                LinkLabel4 = argll1;
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Draws line numbers next to the cfg file richtextbox
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void DrawRichTextBoxLineNumbers(ref Graphics g)
        {
            {
                var withBlock = ConfigFileRTB;
                float font_height;
                font_height = withBlock.GetPositionFromCharIndex(withBlock.GetFirstCharIndexFromLine(2)).Y - withBlock.GetPositionFromCharIndex(withBlock.GetFirstCharIndexFromLine(1)).Y;
                if (font_height == 0f)
                    return;

                // Get the first line index and location
                int first_index;
                int first_line;
                int first_line_y;
                first_index = withBlock.GetCharIndexFromPosition(new Point(0, (int)Math.Round(g.VisibleClipBounds.Y + font_height / 3f)));
                first_line = withBlock.GetLineFromCharIndex(first_index);
                first_line_y = withBlock.GetPositionFromCharIndex(first_index).Y;

                // Print on the PictureBox the visible line numbers of the RichTextBox
                g.Clear(DefaultBackColor);
                int i = first_line;
                var y = default(float);
                while (y < g.VisibleClipBounds.Y + g.VisibleClipBounds.Height && i <= withBlock.Lines.Length)
                {
                    y = first_line_y + 2 + font_height * (i - first_line - 1);
                    if (i == _currentline)
                    {
                        g.DrawString(i.ToString(), new Font(withBlock.Font, FontStyle.Bold), Brushes.DarkRed, LineNumberPictureBox.Width - g.MeasureString(i.ToString(), withBlock.Font).Width, y);
                        g.DrawRectangle(Pens.Black, 0f, y, LineNumberPictureBox.Width - 2, font_height);
                    }
                    else
                    {
                        g.DrawString(i.ToString(), withBlock.Font, Brushes.DarkBlue, LineNumberPictureBox.Width - g.MeasureString(i.ToString(), withBlock.Font).Width, y);
                    }

                    i += 1;
                }

            }
        }

        private void R_Resize(object sender, EventArgs e)
        {
            LineNumberPictureBox.Invalidate();
        }

        private void R_VScroll(object sender, EventArgs e)
        {
            // LineNumberPictureBox.Invalidate()
        }

        private void P_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            var argg = e.Graphics;
            DrawRichTextBoxLineNumbers(ref argg);
        }

        // ---------------------------------------------------------------------------------------
        // @description: Opens a link in the line description richtextbox
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void RichTextBox2_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
        {
            string argFilename = e.LinkText;
            Utilities.OpenInEditor(ref argFilename);
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigFileRTB.Copy();
        }

        private void ToggleCommentBlockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigFileRTB.ToggleCommentBlock();
            ConfigFileRTB.NeedsToBeSaved = true;
            UpdateSaveSatus();
            NeedsRefreshed = true;
        }

        private void ConfigFileRTB_TextChanged(object sender, EventArgs e)
        {
            if (_buildingMode)
                return;

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(ConfigFileRTB.UndoActionName, "Unknown", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0 & !ConfigFileRTB.NeedsToBeSaved)
            {
                ConfigFileRTB.NeedsToBeSaved = true;
                UpdateSaveSatus();
                NeedsRefreshed = true;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            FileWatcher.SetEnable(false);
            ConfigFileRTB.SaveCFGFile();

            UpdateSaveSatus();
            FileWatcher.SetEnable(true);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            InterfaceControler.RefreshOpenForms();
            Focus();
        }



        private void Button3_Click(object sender, EventArgs e)
        {
            if (!ConfigFileRTB.NeedsToBeSaved)
                return;
            if (_currentFile is not null)
            {
                ConfigFileRTB.NeedsToBeSaved = false;
                ConfigFileRTB.LoadCFGFile(ref _currentFile.FilePath, ref _currentFile);
                UpdateSaveSatus();
            }
        }

        private void ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (_currentFile is null)
                return;

            if (NeedsRefreshed)
            {
                System.Windows.Forms.MessageBox.Show(CEResource.TXT_MsgWorkspacMustBeRescanned, CEResource.TXT_TitleAutoFormat, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }

            if (_workspace is null)
                return;
            if (_workspace.IsFromFile)
                return;

            ConfigFileRTB.FormatStyle(ref _currentFile);

            UpdateSaveSatus();
        }

        private void ContextMenuStrip3_Opening(object sender, CancelEventArgs e)
        {
            var x = new VariableGenerator(_workspaceType);
            string st;

            st = ConfigFileRTB.SelectedText;
            ToolStripSeparator2.Visible = false;
            ToolStripSeparator3.Visible = false;
            CI1.Visible = false;
            CI2.Visible = false;
            CI3.Visible = false;
            CI4.Visible = false;
            CI5.Visible = false;
            CI6.Visible = false;

            if (_workspace is not null && _workspace.IsFromFile)
                return;

            if (st.Length > 0)
            {
                CopyExpandedToolStripMenuItem.Visible = true;
                ExpandToolStripMenuItem.Visible = true;
                ToolStripSeparator3.Visible = true;
            }
            else
            {
                CopyExpandedToolStripMenuItem.Visible = false;
                ExpandToolStripMenuItem.Visible = false;
                ToolStripSeparator3.Visible = false;
            }

            if (st.Length < 6 || st.Length > 254)
                return;

            // Replace Strings.Replace and Constants.vbNullString with Replace and ""
            st = st.Replace("\n", "");

            x.TextBox1.Text = st;
            x.NumericUpDown2.Value = 5m;
            x.GenerateSolutions();

            if (x.dgSolutions.Rows.Count >= 1)
            {
                ToolStripSeparator2.Visible = true;
                CI1.Visible = true;
                CI1.Text = x.dgSolutions.Rows[0].Cells[0].Value?.ToString();
            }

            if (x.dgSolutions.Rows.Count >= 2)
            {
                CI2.Visible = true;
                CI2.Text = x.dgSolutions.Rows[1].Cells[0].Value?.ToString();
            }

            if (x.dgSolutions.Rows.Count >= 3)
            {
                CI3.Visible = true;
                CI3.Text = x.dgSolutions.Rows[2].Cells[0].Value?.ToString();
            }

            if (x.dgSolutions.Rows.Count >= 4)
            {
                CI4.Visible = true;
                CI4.Text = x.dgSolutions.Rows[3].Cells[0].Value?.ToString();
            }

            if (x.dgSolutions.Rows.Count >= 5)
            {
                CI5.Visible = true;
                CI5.Text = x.dgSolutions.Rows[4].Cells[0].Value?.ToString();
            }

            if (x.dgSolutions.Rows.Count >= 6)
            {
                CI6.Visible = true;
                CI6.Text = x.dgSolutions.Rows[5].Cells[0].Value?.ToString();
            }

            x.Dispose();
        }

        private void CI1_Click(object sender, EventArgs e)
        {
            ConfigFileRTB.SelectedText = CI1.Text;
            ConfigFileRTB.NeedsToBeSaved = true;
            UpdateSaveSatus();
            NeedsRefreshed = true;
        }

        private void CI2_Click(object sender, EventArgs e)
        {
            ConfigFileRTB.SelectedText = CI2.Text;
            ConfigFileRTB.NeedsToBeSaved = true;
            UpdateSaveSatus();
            NeedsRefreshed = true;
        }

        private void CI3_Click(object sender, EventArgs e)
        {
            ConfigFileRTB.SelectedText = CI3.Text;
            ConfigFileRTB.NeedsToBeSaved = true;
            UpdateSaveSatus();
            NeedsRefreshed = true;
        }

        private void CI4_Click(object sender, EventArgs e)
        {
            ConfigFileRTB.SelectedText = CI4.Text;
            ConfigFileRTB.NeedsToBeSaved = true;
            UpdateSaveSatus();
            NeedsRefreshed = true;
        }

        private void CI5_Click(object sender, EventArgs e)
        {
            ConfigFileRTB.SelectedText = CI5.Text;
            ConfigFileRTB.NeedsToBeSaved = true;
            UpdateSaveSatus();
            NeedsRefreshed = true;
        }

        private void CI6_Click(object sender, EventArgs e)
        {
            ConfigFileRTB.SelectedText = CI6.Text;
            ConfigFileRTB.NeedsToBeSaved = true;
            UpdateSaveSatus();
            NeedsRefreshed = true;
        }

        private void ExpandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_workspace is not null && _workspace.IsFromFile)
                return;

            string st = ConfigFileRTB.SelectedText;

            // Replace Strings.Replace with Replace
            string expansionValue = MacroParser.ParseLine(_workspace, 6, st.Replace("{", "(").Replace("}", ")"), true);

            ConfigFileRTB.SelectedText = expansionValue;
            ConfigFileRTB.NeedsToBeSaved = true;
            UpdateSaveSatus();
            NeedsRefreshed = true;
        }

        private void CopyExpandedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string st = ConfigFileRTB.SelectedText;

            // Replace Strings.Replace with Replace
            string expansionValue = MacroParser.ParseLine(_workspace, 6, st.Replace("{", "(").Replace("}", ")"), true);

            // Replace Strings.Replace with Replace
            System.Windows.Forms.Clipboard.SetText(expansionValue.Replace("/", @"\"));
        }

        private void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            OpenInEditorToolStripMenuItem.Enabled = File.Exists(_currentFile.FilePath);
        }

        // ---------------------------------------------------------------------------------------
        // @description: Builds a description of what happened on a line in a cfg file
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void BuildDesription(ref int ln)
        {
            if (_currentFile is null)
                return;
            if (_workspace is null)
                return;
            CFGFile temp_file;

            Label1.Text = CEResource.TXT_LabelLineDescriptionLine + ln;

            if (NeedsRefreshed)
            {
                LineDescriptionRTB.Text = CEResource.TXT_MsgWorkspaceMustBeRefreshed;
                return;
            }

            CFGLine cline;
            if (_currentFile.Lines.Count >= ln)
            {
                cline = _currentFile.Lines[ln - 1];
                if (cline is not null && cline.IsComment)
                {
                    LineDescriptionRTB.Text = CEResource.TXT_LabelCommentLine;
                    return;
                }
            }
            string msg = "";

            foreach (CFGEvent eve in _workspace.CFGEvents)
            {
                temp_file = null;
                if (eve.ParentFile is null)
                {
                    if (eve.ParentLine is not null)
                    {
                        temp_file = eve.ParentLine.ParentFile;
                    }
                }
                else
                {
                    temp_file = eve.ParentFile;
                }

                if (temp_file is not null)
                {

                    if (temp_file.UID == _currentFile.UID)
                    {
                        if (eve.ParentLine is not null)
                        {
                            if (ln == eve.ParentLine.LineNumber)
                            {
                                if (string.IsNullOrEmpty(msg))
                                {
                                    msg = eve.UID + ": " + GetDescription(eve);
                                }
                                else
                                {
                                    msg = msg + '\n' + eve.UID + ": " + GetDescription(eve);
                                }
                            }
                        }
                    }
                }
            }

            LineDescriptionRTB.Text = msg;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Returns a more verbose description of an event
        // ---------------+---------------+---------------+---------------+---------------+-------
        private string GetDescription(CFGEvent eve)
        {
            switch (eve.EventType)
            {
                case CFGEnums.CFGEventType.cfgVarCreated:
                    return string.Format(CEResource.TXT_MsgVariableCreatedAtLevel, eve.Variable.Name, eve.Level);

                case CFGEnums.CFGEventType.cfgVardef:
                    string formattedValue = eve.VarValue?.Replace(";", "\n\t") ?? string.Empty;
                    return $"{eve.Description}\n\t{formattedValue}";

                case CFGEnums.CFGEventType.cfgMessage:
                    if (eve.Description != null &&
                        eve.Description.IndexOf("referenced", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return string.Format(CEResource.TXT_MsgVariableReferenced, eve.VarName);
                    }
                    return eve.Description ?? string.Empty;

                default:
                    return eve.Description ?? string.Empty;
            }
        }

        private void btnOpenTreeViewer_Click(object sender, EventArgs e)
        {
            // Check if a CFGTree form is already open
            var openTree = Application.OpenForms
                .OfType<CFGTree>()
                .FirstOrDefault();

            CFGTree treeViewerForm;
            if (openTree != null)
            {
                treeViewerForm = openTree;
                treeViewerForm.BringToFront();
            }
            else
            {
                treeViewerForm = new CFGTree(_workspace, _workspaceType);
                treeViewerForm.AfterNodeSelect += CfgTree_AfterSelect;
                treeViewerForm.Show();
            }

            // If cbShowAllNode is true, highlight all file paths in the tree
            //if (cbShowAllNode != null && cbShowAllNode.Checked)

            {
                var allPaths = new List<string>();
                foreach (TreeNode node in TreeView1.Nodes)
                {
                    CollectAllNodePaths(node, allPaths);
                }
                treeViewerForm.HighlightFiles(allPaths);
            }
        }

        // Helper method to collect all file paths from TreeView1
        private void CollectAllNodePaths(TreeNode node, List<string> paths)
        {
            string fullPath;
        
            if (node.FullPath is not null && !string.IsNullOrEmpty(node.FullPath))
            {
                int nodeID = Convert.ToInt32(node.Name);
                fullPath = GetFullPathByName(nodeID);
                paths.Add(fullPath);
            }
            foreach (TreeNode child in node.Nodes)
            {
                CollectAllNodePaths(child, paths);
            }
        }

        // Add this method to your FileHistory class to fix the CS0103 error:
        private void CfgTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Use the correct type for NodeData
            if (e.Node.Tag is WorkspaceCFG.CFGTree.NodeData data)
            {
                // Find the open CFGTreeGrid form (fix the missing type error by adding a using or fully qualifying)
                var treeGridForm = Application.OpenForms
                    .Cast<Form>()
                    .FirstOrDefault(f => f.GetType().Name == "CFGTreeGrid");

                if (treeGridForm != null)
                {
                    // Use reflection to call HighlightFiles if type is not directly verfügbar
                    var method = treeGridForm.GetType().GetMethod("HighlightFiles");
                    if (method != null)
                    {
                        method.Invoke(treeGridForm, new object[] { new List<string> { data.FullPath } });
                    }
                }
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }

}



// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{

    public partial class PathChecker
    {

        private struct PathCheckResult
        {
            public string VariableName;
            public string PathType;
            public string Path;
            public string Status;
            public bool FolderExists;
            public bool FileExists;
            public bool IsFile;
        }

        private CFGConfiguration _workspace;
        private CFGEnums.ConfigurationType _workspaceType;
        private List<PathCheckResult> _pathCollection;
        private DataTable _dataTable;

        public PathChecker(CFGEnums.ConfigurationType inwrktype)
        {
            // This call is required by the designer.
            InitializeComponent();

            // Gets a workspace instance
            object argform = this;
            InterfaceControler.AddForm(ref argform);
            _workspaceType = inwrktype;

            RefreshForm();
        }

        public void RefreshForm()
        {
            InterfaceControler.GetConfiguration(ref _workspace, _workspaceType);
            if (_workspace is null)
                return;
            Text = CEResource.TXT_TitlePathChecker + _workspace.Description2;
            BuildList();
        }

        private void PathChecker_FormClosing(object sender, FormClosingEventArgs e)
        {
            object argform = this;
            InterfaceControler.RemoveForm(ref argform);
        }

        private void WatchF5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                InterfaceControler.RefreshOpenForms();
            }
        }

        private void BuildList()
        {
            if (_workspace is null)
                return;

            _pathCollection = new List<PathCheckResult>();

            foreach (CFGVariable @var in _workspace.Variables.Values.ToList())
                ScanVariable(@var);

            BuildTable();
        }

        private void BuildTable()
        {
            DataRow dataRow;

            if (_workspace is null)
                return;
            if (_pathCollection is null)
                return;

            // Build filtered datatable here
            _dataTable = new DataTable();

            _dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCVariable));
            _dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCStatus));
            _dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCPath));

            _dataTable.BeginLoadData();

            foreach (PathCheckResult result in _pathCollection)
            {
                if (string.IsNullOrEmpty(TextBox1.Text) ||
                    result.VariableName.IndexOf(TextBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    dataRow = _dataTable.NewRow();
                    dataRow[0] = result.VariableName;
                    dataRow[1] = result.Status;
                    dataRow[2] = result.Path;
                    _dataTable.Rows.Add(dataRow);
                }
            }

            _dataTable.EndLoadData();

            DataGridView1.DataSource = _dataTable;
            DataGridView1.Sort(DataGridView1.Columns[0], System.ComponentModel.ListSortDirection.Descending);
            DataGridView1.Refresh();
            Label6.Text = DataGridView1.Rows.Count.ToString() + CEResource.TXT_LabelPaths;
        }

        private void ScanVariable(CFGVariable variable)
        {
            string[] fval;
            string[] exp;
            int t;
            int y;

            if (variable is null)
                return;
            if (variable.ParentConfiguration is null)
                return;

            fval = (variable.Value ?? "").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            int loopTo = fval.Length - 1;
            for (y = 0; y <= loopTo; y++)
            {

                if ((fval[y]?.Trim().Length ?? 0) > 1)
                {
                    string expansionValue = MacroParser.ParseLine(variable.ParentConfiguration, 6, fval[y], true);

                    exp = (expansionValue ?? "").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    int loopTo1 = exp.Length - 1;
                    for (t = 0; t <= loopTo1; t++)
                        CheckPath(ref exp[t], ref variable);

                }
            }
        }

        private void CheckPath(ref string path, ref CFGVariable variable)
        {
            if (!UtilitiesPath.IsPath(ref path))
                return;

            var pchk = new PathCheckResult();

            pchk.Path = path;
            pchk.VariableName = variable.Name;

            pchk.IsFile = UtilitiesPath.IsFile(ref path);

            if (pchk.IsFile && string.IsNullOrEmpty(UtilitiesPath.ExtFromPath(ref path)))
            {
                pchk.IsFile = false;
                path += @"\";
            }

            pchk.FolderExists = UtilitiesPath.FolderExists(UtilitiesPath.GetDirectoryName(ref path));
            pchk.FileExists = pchk.IsFile && File.Exists(path);

            if (pchk.FolderExists && (pchk.Path?.Contains("*") ?? false))
                pchk.FileExists = UtilitiesPath.WildCardFilesExist(ref path);

            pchk.Status = CEResource.TXT_OK;

            if (pchk.IsFile & !pchk.FileExists)
            {
                pchk.Status = CEResource.TXT_MsgMissingFile;
            }

            if (!pchk.FolderExists)
            {
                pchk.Status = CEResource.TXT_MsgMissingFolder;
            }

            _pathCollection.Add(pchk);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            BuildList();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Open the first available folder
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void OpenFirstAvaliableFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_workspace is null)
                return;
            if (DataGridView1.Rows.Count == 0)
                return;

            string path;
            long length;
            path = DataGridView1.SelectedRows[0].Cells[2].Value?.ToString();

            if (UtilitiesPath.IsPath(ref path))
            {
                path = UtilitiesPath.GetDirectoryName(ref path);
                length = path.Length;
                while (true)
                {
                    if (UtilitiesPath.FolderExists(path))
                    {
                        Utilities.OpenContainingFolder(ref path);
                        return;
                    }
                    path = Utilities.ParentDevDir(path);
                    if (string.IsNullOrEmpty(path) || path.Length == length)
                        return;
                    length = path.Length;
                }
            }
        }

        private void OpenContainingFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_workspace is null)
                return;
            if (DataGridView1.Rows.Count == 0)
                return;

            string path;

            path = DataGridView1.SelectedRows[0].Cells[2].Value?.ToString();

            if (UtilitiesPath.IsPath(ref path))
            {
                string argfilename = UtilitiesPath.GetDirectoryName(ref path);
                Utilities.OpenContainingFolder(ref argfilename);
            }
        }

        private void OpenVariableInDossieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_workspace is null)
                return;
            if (DataGridView1.Rows.Count == 0)
                return;

            string varname;
            CFGVariable @var;

            varname = DataGridView1.SelectedRows[0].Cells[0].Value?.ToString();

            @var = _workspace.GetVariable(varname);
            if (@var is null)
                return;

            var vdos = new VariableDossier(_workspaceType, ref varname);
            if (InterfaceControler.GroupWindowsInMainForm)
                vdos.MdiParent = MdiParent;
            vdos.Show();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            BuildTable();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            TextBox1.Clear();
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if (e.Value is not null && CultureInfo.CurrentCulture.CompareInfo.Compare(e.Value.ToString() ?? "", CEResource.TXT_OK ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
                {
                    e.CellStyle.ForeColor = Color.Red;
                }
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
                treeViewerForm.Show();
            }

            if (_pathCollection is not null)
            {
                var filePaths = _pathCollection
                    .Where(result => result.IsFile && result.FileExists)
                    .Select(result => result.Path)
                    .Distinct()
                    .ToList();

                treeViewerForm.HighlightFiles(filePaths);
            }
        }
    }
}
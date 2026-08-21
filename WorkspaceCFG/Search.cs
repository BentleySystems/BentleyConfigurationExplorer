// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Globalization;
using WorkspaceCFG.My;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{
    public partial class Search
    {

        private CFGConfiguration _workspace;
        private CFGEnums.ConfigurationType _workspaceType;

        public Search(CFGEnums.ConfigurationType inwrktype)
        {
            // This call is required by the Windows Form Designer.
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
            Text = CEResource.TXT_TitleSearch + _workspace.Description2;
            BuildSearch();
        }

        private void AddRow(ref DataTable dataTable, string rtype, string desc, string filename, string linenumber, string filepath)
        {
            var dataRow = dataTable.NewRow();

            dataRow[0] = rtype;
            dataRow[3] = (desc ?? "").Replace("\t", " ");
            dataRow[1] = filename;
            dataRow[2] = linenumber;
            dataRow[4] = filepath;

            dataTable.Rows.Add(dataRow);
        }

        private void Search_Library(ref DataTable dt, ref string ss)
        {
            string searchText = ss.ToUpperInvariant();

            VarDB.EnsureLoaded();
            foreach (CFGEnums.VarInfo dbe in VarDB.VariableDB)
            {
                string name = dbe.Name?.ToUpperInvariant() ?? string.Empty;
                string desc = dbe.LongDesc?.ToUpperInvariant() ?? string.Empty;

                if (name.Contains(searchText) || desc.Contains(searchText))
                {
                    string matchType = CEResource.TXT_LabelVARIABLELIBRARY;
                    string lineNumber = "-";
                    AddRow(ref dt, matchType, dbe.LongDesc, dbe.Name, lineNumber, dbe.Name);
                }
            }
        }

        private void BuildSearch()
        {
            var dataTable = new DataTable();
            if (string.IsNullOrWhiteSpace(TextBox1.Text) || _workspace == null)
                return;

            string searchPattern = "*" + TextBox1.Text.ToUpperInvariant() + "*";

            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCMatchType));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCFile));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCLine));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCDescrption));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCData1));

            dataTable.BeginLoadData();

            // Search config files
            foreach (CFGFile cfgFile in _workspace.CFGFiles)
            {
                if (!string.IsNullOrEmpty(TextBox1.Text) && cfgFile.FilePath != null &&
                    cfgFile.FilePath.IndexOf(TextBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    AddRow(ref dataTable, CEResource.TXT_CONFIGFILENAME, cfgFile.FilePath, cfgFile.Name, "-", cfgFile.FilePath);
                }

                foreach (var cfgLine in cfgFile.Lines)
                {
                    if (cfgLine.Text != null)
                    {
                        string lineText = cfgLine.Text.ToUpperInvariant();
                        string commentText = cfgLine.InlineComment?.ToUpperInvariant() ?? "";

                        if (lineText.Contains(TextBox1.Text.ToUpperInvariant()))
                        {
                            string type = cfgLine.IsComment ? CEResource.TXT_LabelCOMMENT : CEResource.TXT_CONFIGTEXT;
                            string desc = cfgLine.Text + " " + cfgLine.InlineComment;
                            string lineNumber = cfgLine.LineNumber.ToString();

                            AddRow(ref dataTable, type, desc, cfgLine.ParentFile.Name, lineNumber, cfgFile.FilePath);
                        }
                        else if (!string.IsNullOrEmpty(commentText) && commentText.Contains(TextBox1.Text.ToUpperInvariant()))
                        {
                            string desc = cfgLine.Text + " " + cfgLine.InlineComment;
                            string lineNumber = cfgLine.LineNumber.ToString();

                            AddRow(ref dataTable, CEResource.TXT_LabelCOMMENT, desc, cfgLine.ParentFile.Name, lineNumber, cfgLine.ParentFile.FilePath);
                        }
                    }
                }
            }

            // Search variables
            foreach (CFGVariable cfgVariable in _workspace.Variables.Values)
            {
                string searchText = TextBox1.Text.ToUpperInvariant();

                if (cfgVariable.Name.ToUpperInvariant().Contains(searchText))
                {
                    AddRow(ref dataTable, CEResource.TXT_LabelVARIABLENAME, cfgVariable.Name, cfgVariable.Name, "-", string.Empty);
                }

                if (cfgVariable.FinalExpansion.ToUpperInvariant().Contains(searchText))
                {
                    AddRow(ref dataTable, CEResource.TXT_LabelVARIABLEEXPANSION, cfgVariable.FinalExpansion, cfgVariable.Name, "-", string.Empty);
                }
                else if (!string.IsNullOrEmpty(cfgVariable.Value) && cfgVariable.Value.ToUpperInvariant().Contains(searchText))
                {
                    AddRow(ref dataTable, CEResource.TXT_LabelVARIABLEVALUE, cfgVariable.Value, cfgVariable.Name, "-", string.Empty);
                }
            }

            // Search variable library
            if (CheckBox4.Checked)
            {
                Search_Library(ref dataTable, ref searchPattern);
            }

            Label6.Text = dataTable.Rows.Count == 1
                ? CEResource.TXT_MsgSingleResult
                : dataTable.Rows.Count + CEResource.TXT_MsgMultipleResults;

            dataTable.EndLoadData();

            DataGridView1.DataSource = dataTable;
            DataGridView1.Refresh();
            DataGridView1.Columns[4].Visible = false;
            DataGridView1.Sort(DataGridView1.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
        }

        private void Search_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            MySettings.Default.SRCH_Location = Location;
            MySettings.Default.SRCH_WindowState = WindowState;
            if (WindowState == System.Windows.Forms.FormWindowState.Normal)
                MySettings.Default.SRCH_Size = Size;
            MySettings.Default.Save();
            object argform = this;
            InterfaceControler.RemoveForm(ref argform);
        }

        private void Search_Load(object sender, EventArgs e)
        {
            Location = MySettings.Default.SRCH_Location;
            WindowState = MySettings.Default.SRCH_WindowState;
            if (WindowState == System.Windows.Forms.FormWindowState.Normal)
                Size = MySettings.Default.SRCH_Size;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            BuildSearch();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            TextBox1.Text = "";
            Clear_Datagrid();
            Label6.Text = CEResource.TXT_NoResults;
        }

        private void Clear_Datagrid()
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCMatchType));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCDescrption));

            dataTable.EndLoadData();

            DataGridView1.DataSource = dataTable;
            DataGridView1.Refresh();
        }

        private void WatchF5_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.F5)
            {
                InterfaceControler.RefreshOpenForms();
            }
        }

        private void TextBox1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == 13)
                BuildSearch();
        }

        private void DataGridView1_CellDoubleClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            string mtype;
            string test;

            if (_workspace is null)
                return;
            if (DataGridView1.SelectedRows.Count == 0)
                return;

            {
                var withBlock = DataGridView1;
                mtype = withBlock.SelectedRows[0].Cells[0].Value?.ToString();

                if (CultureInfo.CurrentCulture.CompareInfo.Compare(mtype ?? "", CEResource.TXT_LabelVARIABLENAME ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 | CultureInfo.CurrentCulture.CompareInfo.Compare(mtype ?? "", CEResource.TXT_LabelVARIABLEVALUE ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 | CultureInfo.CurrentCulture.CompareInfo.Compare(mtype ?? "", CEResource.TXT_LabelVARIABLEEXPANSION ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                {
                    string argvariableName = withBlock.SelectedRows[0].Cells[1].Value?.ToString();
                    var vdos = new VariableDossier(_workspaceType, ref argvariableName);
                    withBlock.SelectedRows[0].Cells[1].Value = argvariableName;
                    if (InterfaceControler.GroupWindowsInMainForm)
                        vdos.MdiParent = MdiParent;
                    vdos.Show();
                }

                if (CultureInfo.CurrentCulture.CompareInfo.Compare(mtype ?? "", CEResource.TXT_LabelCOMMENT ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 | CultureInfo.CurrentCulture.CompareInfo.Compare(mtype ?? "", CEResource.TXT_CONFIGTEXT ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                {
                    test = withBlock.SelectedRows[0].Cells[2].Value?.ToString();
                    string argFilename = withBlock.SelectedRows[0].Cells[4].Value?.ToString();
                    if (int.TryParse(test, out int lineNumber))
                        Utilities.OpenInEditor(ref argFilename, true, lineNumber);
                    else
                        Utilities.OpenInEditor(ref argFilename, true, 0);
                    withBlock.SelectedRows[0].Cells[4].Value = argFilename;
                }

                if (CultureInfo.CurrentCulture.CompareInfo.Compare(mtype ?? "", CEResource.TXT_CONFIGFILENAME ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                {
                    string argFilename1 = withBlock.SelectedRows[0].Cells[4].Value?.ToString();
                    Utilities.OpenInEditor(ref argFilename1);
                    withBlock.SelectedRows[0].Cells[4].Value = argFilename1;
                }
            }
        }

        private void CheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            BuildSearch();
        }

        private void ExportToExcelToolStripMenuItem_Click(object sender, EventArgs e)
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

            string argSheetName = CEResource.TXT_LabelSearchResults;
            ExcelExporter.DataGridExport(DataGridView1, ref argSheetName, path);
            System.Windows.Forms.MessageBox.Show(CEResource.TXT_MsgFileExportComplete, CEResource.TXT_MsgDone);
        }

        ~Search()
        {
            _workspace = null;
            _workspaceType = default;
        }

    }
}
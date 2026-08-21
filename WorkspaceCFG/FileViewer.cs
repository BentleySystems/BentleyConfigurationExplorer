// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using WorkspaceCFG.My;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{
    public partial class FileViewer
    {

        private CFGConfiguration _workspace;
        private CFGEnums.ConfigurationType _workspaceType;
        private string _configFile;
        private int _gotoRow;
        private string _selectedVarName;

        public FileViewer(CFGEnums.ConfigurationType inwrktype, ref string cfgfilepath, int row = -1)
        {
            InitializeComponent();

            object argform = this;
            InterfaceControler.AddForm(ref argform);
            _workspaceType = inwrktype;
            _configFile = cfgfilepath;
            _gotoRow = row;

            RefreshForm();
        }

        public void RefreshForm()
        {
            InterfaceControler.GetConfiguration(ref _workspace, _workspaceType);
            if (_workspace is null)
                return;

            Text = CEResource.TXT_TitleFileViewer + _workspace.Description2;

            Build_Combo_Box();

            if (!string.IsNullOrEmpty(_configFile))
                Build_File_Grid();
        }

        private void Build_Combo_Box()
        {
            if (_workspace is null)
                return;

            ComboBox1.Items.Clear();

            foreach (CFGFile cfgFile in _workspace.CFGFiles)
                ComboBox1.Items.Add(cfgFile.FilePath);
        }

        private void Build_File_Grid()
        {
            int lineCount;
            CFGFile cfgFile;
            DataRow dataRow;
            var dataTable = new DataTable();
            int t;

            dataTable.Columns.Add(new DataColumn(CEResource.TXT_LabelLine));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_LabelFile));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_LabelDescription));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_LabelColor));

            if (_workspace is null)
                return;
            cfgFile = _workspace.GetFile(_configFile);
            if (cfgFile is null)
                return;

            lineCount = cfgFile.Lines.Last().LineNumber;

            if (lineCount <= 0)
                return;

            var loopTo = lineCount;
            for (t = 0; t <= loopTo; t++)
            {
                dataRow = dataTable.NewRow();
                dataRow[0] = t + 1;
                dataTable.Rows.Add(dataRow);
            }

            // Add text
            foreach (var cfgLine in cfgFile.Lines)
            {
                dataTable.Rows[cfgLine.LineNumber - 1][1] = cfgLine.Text.Replace("\t", " ") + " " + cfgLine.InlineComment;
                if (cfgLine.IsComment)
                    dataTable.Rows[cfgLine.LineNumber - 1][3] = CEResource.TXT_LabelCOMMENT;
            }

            if (CheckBox1.Checked)
                BuildDescriptionColumn(dataTable, cfgFile);

            DataGridView1.DataSource = dataTable;
            DataGridView1.Columns[2].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            DataGridView1.Columns[2].DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            DataGridView1.Columns[3].Visible = false;
            DataGridView1.Refresh();

            ComboBox1.Text = cfgFile.FilePath;

            Text = "File Viewer - " + cfgFile.Name + " - " + _workspace.Description2;
        }

        private void BuildDescriptionColumn(DataTable dataTable, CFGFile cfile)
        {
            if (cfile == null || _workspace == null)
                return;

            foreach (CFGEvent eve in _workspace.CFGEvents)
            {
                CFGFile cfgFile = eve.ParentFile ?? eve.ParentLine?.ParentFile;

                if (cfgFile == null || cfgFile.UID != cfile.UID || eve.ParentLine == null)
                    continue;

                int lineIndex = eve.ParentLine.LineNumber - 1;

                if (dataTable.Rows[lineIndex][2] == DBNull.Value || dataTable.Rows[lineIndex][2] == null)
                {
                    dataTable.Rows[lineIndex][2] = $"{eve.UID}: {Get_Description(eve)}";
                }
                else
                {
                    string existingText = dataTable.Rows[lineIndex][2].ToString();
                    dataTable.Rows[lineIndex][2] = $"{existingText}\n{eve.UID}: {eve.Description}";
                }
            }
        }

        private void FileViewer_Activated(object sender, EventArgs e)
        {
            if (_gotoRow > 0)
            {
                if (DataGridView1.Rows.Count >= _gotoRow)
                {
                    DataGridView1.CurrentCell = DataGridView1.Rows[_gotoRow].Cells[0];
                }
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _configFile = ComboBox1.Text;
            Build_File_Grid();
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            Build_File_Grid();
        }

        private void Export_To_File()
        {
            string path;
            CFGFile cfile;
            if (_workspace is null)
                return;
            cfile = _workspace.GetFile(_configFile);
            if (cfile is null)
                return;

            {
                var withBlock = SaveFileDialog1;
                withBlock.Title = CEResource.TXT_MsgExportWorkspaceDetailsToTextFile;
                withBlock.InitialDirectory = CEResource.TXT_CDrive;
                withBlock.Filter = CEResource.TXT_ExtExportWorkSpaceFilterCfg;
                if (cfile.Name != null && cfile.Name.IndexOf(CEResource.TXT_ExtPcf, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    withBlock.Filter = CEResource.TXT_ExtExportWorkSpaceFilterPcf;
                }
                else if (cfile.Name != null && cfile.Name.IndexOf(CEResource.TXT_ExtCcf, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    withBlock.Filter = CEResource.TXT_ExtExportWorkSpaceFilterCcf;
                }
                else if (cfile.Name != null && cfile.Name.IndexOf(CEResource.TXT_ExtDat, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    withBlock.Filter = CEResource.TXT_ExtExportWorkSpaceFilterDat;
                }

                withBlock.FileName = cfile.Name;
                withBlock.RestoreDirectory = true;
                if (withBlock.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    return;

                path = withBlock.FileName;
                withBlock.Dispose();
            }

            using (var streamWriter = new StreamWriter(path))
            {
                foreach (var line in cfile.Lines)
                    streamWriter.WriteLine(
                        new string('\t', line.Indent) +
                        line.Text.Replace(@"\", "/") +
                        "  " +
                        line.InlineComment
                    );
            }
        }

        private void ExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Export_To_File();
        }

        private void OpenInEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utilities.OpenInEditor(ref _configFile);
        }

        private void OpenInEditorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int rn;
            if (DataGridView1.SelectedCells.Count > 0)
            {
                rn = DataGridView1.SelectedCells[0].RowIndex + 1;
            }
            else
            {
                rn = 0;
            }
            Utilities.OpenInEditor(ref _configFile, true, rn);
        }

        private void ExportToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Export_To_File();
        }

        private string Get_Description(CFGEvent eve)
        {
            if (eve.EventType == CFGEnums.CFGEventType.cfgVarCreated)
                return string.Format(CEResource.TXT_MsgVariableCreated, eve.Variable.Name);
            return eve.Description;
        }

        private void FileViewer_Click(object sender, EventArgs e)
        {
            DataGridView1.Rows[4].Selected = true;
            DataGridView1.CurrentCell = DataGridView1.Rows[0].Cells[0];
            DataGridView1.CurrentCell = DataGridView1.Rows[4].Cells[0];
        }

        private void OpenContainingFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_workspace is null)
                return;
            if (string.IsNullOrEmpty(_configFile))
                return;
            Process.Start("explorer.exe", NoFilenameFromPath(ref _configFile));
        }

        private string NoFilenameFromPath(ref string path)
        {
            return path.Substring(0, path.Length - FilenameFromPath(ref path).Length);
        }

        private string FilenameFromPath(ref string path)
        {
            int pos = -1;
            int t;

            var loopTo = path.Length - 1;
            for (t = 0; t <= loopTo; t++)
            {
                if (path[t].ToString() == @"\")
                    pos = t;
            }

            if (pos == -1)
                return path;
            return path.Substring(pos + 1);
        }

        private void FileViewer_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            object argform = this;
            InterfaceControler.RemoveForm(ref argform);
            MySettings.Default.FV_Location = Location;
            MySettings.Default.FV_WindowState = WindowState;
            if (WindowState == System.Windows.Forms.FormWindowState.Normal)
                MySettings.Default.FV_Size = Size;
            MySettings.Default.Save();
        }

        private void WatchF5_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.F5)
            {
                InterfaceControler.RefreshOpenForms();
            }
        }

        private void FileViewer_Load(object sender, EventArgs e)
        {
            Location = MySettings.Default.FV_Location;
            WindowState = MySettings.Default.FV_WindowState;
            if (WindowState == System.Windows.Forms.FormWindowState.Normal)
                Size = MySettings.Default.FV_Size;
        }

        private void DataGridView1_DataBindingComplete(object sender, System.Windows.Forms.DataGridViewBindingCompleteEventArgs e)
        {
            int t;
            {
                var withBlock = DataGridView1;
                var loopTo = withBlock.Rows.Count - 2;
                for (t = 0; t <= loopTo; t++)
                {
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(withBlock.Rows[t].Cells[3].Value.ToString() ?? "", CEResource.TXT_LabelCOMMENT ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                    {
                        withBlock.Rows[t].DefaultCellStyle.ForeColor = Color.Green;
                    }
                }
            }
        }

        private void ContextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _selectedVarName = string.Empty;
            OpenVariableInDossierToolStripMenuItem.Enabled = false;

            if (_workspace == null || DataGridView1.SelectedRows.Count == 0)
                return;

            int lineno;
            try
            {
                object cellValue = DataGridView1.SelectedRows[0].Cells[0].Value;
                if (cellValue == null || !int.TryParse(cellValue.ToString(), out lineno))
                    return;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                return;
            }

            _selectedVarName = _workspace.GetVariableName(ref _configFile, lineno);
            OpenVariableInDossierToolStripMenuItem.Enabled = !string.IsNullOrEmpty(_selectedVarName);
        }

        private void OpenVariableInDossierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_workspace is null)
                return;

            if (!string.IsNullOrEmpty(_selectedVarName))
            {
                var x = new VariableDossier(_workspaceType, ref _selectedVarName);
                if (InterfaceControler.GroupWindowsInMainForm)
                    x.MdiParent = MdiParent;
                x.Show();
            }
        }

        ~FileViewer()
        {
            _workspace = null;
            _workspaceType = default;
        }

    }
}
// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using WorkspaceCFG.My;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{

    public partial class EventHistory
    {

        public bool IgnoreBuild;

        private CFGConfiguration _workspace;
        private CFGEnums.ConfigurationType _workspaceType;
        private CFGFile _currentfile;

        public EventHistory(CFGEnums.ConfigurationType inwrktype)
        {
            IgnoreBuild = true;
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

            Text = CEResource.TXT_TitleEventHistory + _workspace.Description2;

            // Fill the combo box with the names of each file processed
            BuildComboBox();
        }

        // Fill the combo box with all the files processed in this workspace

        private void BuildComboBox()
        {

            if (_workspace is null)
                return;

            ComboBox1.Items.Clear();
            ComboBox1.Items.Add("ALL");
            foreach (CFGFile cfgFile in _workspace.CFGFiles)
                ComboBox1.Items.Add(cfgFile.FilePath);
            ComboBox1.Items.Add("ALL");
        }

        // Fills the datagrid with events based on the filters

        private void BuildEventGrid()
        {
            bool display;
            string uid;
            DataRow dataRow;
            var dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCID));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCDescrption));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCVariable));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCLevel));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCFile));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCLine));

            if (IgnoreBuild)
                return;

            dataTable.BeginLoadData();
            foreach (CFGEvent cfgEvent in _workspace.CFGEvents)
            {
                display = true;
                if (cfgEvent.EventType == CFGEnums.CFGEventType.cfgError && CheckBox1.Checked == false)
                    display = false;
                if (cfgEvent.EventType == CFGEnums.CFGEventType.cfgWarning && CheckBox2.Checked == false)
                    display = false;
                if (cfgEvent.EventType == CFGEnums.CFGEventType.cfgInclude && CheckBox3.Checked == false)
                    display = false;
                if (cfgEvent.EventType != CFGEnums.CFGEventType.cfgError && cfgEvent.EventType != CFGEnums.CFGEventType.cfgWarning && cfgEvent.EventType != CFGEnums.CFGEventType.cfgInclude && CheckBox4.Checked == false)
                {
                    display = false;
                }

                if (cfgEvent.ParentFile is not null)
                {
                    if (string.IsNullOrEmpty(ComboBox1.Text) | CultureInfo.CurrentCulture.CompareInfo.Compare(ComboBox1.Text, "ALL", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 | CultureInfo.CurrentCulture.CompareInfo.Compare(ComboBox1.Text ?? "", cfgEvent.ParentFile.FilePath ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                    {
                    }
                    // TODO: this is the same as display = display
                    // display = display And True
                    else
                    {
                        display = false;
                    }
                }
                else if (string.IsNullOrEmpty(ComboBox1.Text) | CultureInfo.CurrentCulture.CompareInfo.Compare(ComboBox1.Text, "ALL", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                {
                    // TODO: this is the same as display = display
                    // display = display And True
                }

                if (display)
                {
                    dataRow = dataTable.NewRow();
                    uid = "0000" + cfgEvent.UID.ToString();
                    dataRow[0] = uid.Substring(uid.Length - 4);
                    dataRow[1] = cfgEvent.Description;

                    if (cfgEvent.EventType == CFGEnums.CFGEventType.cfgFinishInclude)
                    {
                        dataRow[1] = dataRow[1].ToString() + " " + cfgEvent.ParentFile.Name;
                    }

                    dataRow[2] = cfgEvent.VarName;

                    dataRow[3] = " ";
                    if (cfgEvent.Level > -999)
                        dataRow[3] = Utilities.GetLevelExtendedName(cfgEvent.Level);

                    if (cfgEvent.ParentLine is not null)
                        dataRow[5] = cfgEvent.ParentLine.LineNumber;
                    if (cfgEvent.ParentFile is not null)
                        dataRow[4] = cfgEvent.ParentFile.Name;
                    dataTable.Rows.Add(dataRow);
                }

            }
            dataTable.EndLoadData();

            DataGridView1.SuspendLayout();
            try
            {
                DataGridView1.DataSource = dataTable;
                // Use DisplayedCells instead of AllCells: AllCells/Fill measure every
                // row in the entire table on every rebind, which is O(n) per column
                // and becomes extremely slow with large event histories.
                // DisplayedCells only measures the rows currently visible.
                DataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                DataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                DataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                DataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                DataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                DataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
            finally
            {
                DataGridView1.ResumeLayout();
            }
            DataGridView1.Refresh();
        }

        private void OpenSelectedRowInFileViewer()
        {
            if (_workspace == null)
                return;

            var withBlock = DataGridView1;
            if (withBlock.SelectedRows.Count == 0)
                return;

            if (!int.TryParse(withBlock.SelectedRows[0].Cells[0].Value?.ToString(), out int eid))
                return;

            if (_workspace.CFGEvents[eid] is CFGEvent cfgEvent &&
                cfgEvent.ParentFile is CFGFile cfgFile &&
                cfgEvent.ParentLine != null)
            {
                var fileViewer = new FileViewer(
                    CFGEnums.ConfigurationType.Main,
                    ref cfgFile.FilePath,
                    cfgEvent.ParentLine.LineNumber - 1
                );

                fileViewer.CheckBox1.Checked = true;

                if (InterfaceControler.GroupWindowsInMainForm)
                    fileViewer.MdiParent = MdiParent;

                fileViewer.Show();
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            BuildEventGrid();
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            BuildEventGrid();
        }

        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            BuildEventGrid();
        }

        private void CheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            BuildEventGrid();
        }

        private void EventHistory_FormClosed(object sender, FormClosedEventArgs e)
        {
            object argform = this;
            InterfaceControler.RemoveForm(ref argform);
        }

        private void EventHistory_Shown(object sender, EventArgs e)
        {
            IgnoreBuild = false;
            BuildEventGrid();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuildEventGrid();
        }

        private void GoToLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSelectedRowInFileViewer();
        }

        private void OpenFileInViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_workspace == null)
                return;

            var withBlock = DataGridView1;
            if (withBlock.SelectedRows.Count == 0)
                return;

            if (!int.TryParse(withBlock.SelectedRows[0].Cells[0].Value?.ToString(), out int eid))
                return;

            if (_workspace.CFGEvents[eid] is CFGEvent eve &&
                eve.ParentFile is CFGFile cfile &&
                eve.ParentLine != null)
            {
                var fview = new FileViewer(CFGEnums.ConfigurationType.Main, ref cfile.FilePath);
                fview.CheckBox1.Checked = true;

                if (InterfaceControler.GroupWindowsInMainForm)
                    fview.MdiParent = MdiParent;

                fview.Show();
            }
        }

        private void WatchF5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                InterfaceControler.RefreshOpenForms();
            }
        }

        private void EventHistory_FormClosing(object sender, FormClosingEventArgs e)
        {
            MySettings.Default.EH_Location = Location;
            MySettings.Default.EH_WindowState = WindowState;
            if (WindowState == FormWindowState.Normal)
                MySettings.Default.EH_Size = Size;
            object argform = this;
            InterfaceControler.RemoveForm(ref argform);
            MySettings.Default.Save();
        }

        private void EventHistory_Load(object sender, EventArgs e)
        {
            Location = MySettings.Default.EH_Location;
            WindowState = MySettings.Default.EH_WindowState;
            if (WindowState == FormWindowState.Normal)
                Size = MySettings.Default.EH_Size;
        }

        private void ToExcelToolStripMenuItem_Click(object sender, EventArgs e)
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
                if (withBlock.ShowDialog() == DialogResult.Cancel)
                    return;

                path = withBlock.FileName;
                withBlock.Dispose();
            }

            string argSheetName = CEResource.TXT_TitleEventHistory;
            ExcelExporter.DataGridExport(DataGridView1, ref argSheetName, path);
            MessageBox.Show(CEResource.TXT_MsgFileExportComplete, CEResource.TXT_MsgDone);
        }

        private void OpenVariableInDossierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_workspace == null || DataGridView1.SelectedRows.Count == 0)
                return;

            try
            {
                // Safely parse the event ID from the selected row
                object cellValue = DataGridView1.SelectedRows[0].Cells[0].Value;
                if (cellValue == null || !int.TryParse(cellValue.ToString(), out int eid))
                    return;

                // Ensure the event ID is within bounds
                if (eid < 0 || eid >= _workspace.CFGEvents.Count)
                    return;

                CFGEvent eve = _workspace.CFGEvents[eid] as CFGEvent;

                if (eve?.Variable?.Name != null)
                {
                    var dossier = new VariableDossier(_workspaceType, ref eve.Variable.Name);

                    if (InterfaceControler.GroupWindowsInMainForm)
                        dossier.MdiParent = MdiParent;

                    dossier.Show();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
        }

        private void ContextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_workspace == null)
                return;

            OpenVariableInDossierToolStripMenuItem.Enabled = false;

            var withBlock = DataGridView1;
            if (withBlock.SelectedRows.Count == 0)
                return;

            try
            {
                if (!int.TryParse(withBlock.SelectedRows[0].Cells[0].Value?.ToString(), out int eid))
                    return;

                if (eid <= _workspace.CFGEvents.Count &&
                    _workspace.CFGEvents[eid] is CFGEvent eve &&
                    eve.Variable != null)
                {
                    OpenVariableInDossierToolStripMenuItem.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
        }

        private void OpenFileInEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_workspace == null)
                return;

            var withBlock = DataGridView1;
            if (withBlock.SelectedRows.Count == 0)
                return;

            if (!int.TryParse(withBlock.SelectedRows[0].Cells[0].Value?.ToString(), out int eid))
                return;

            if (_workspace.CFGEvents[eid] is CFGEvent eve && eve.ParentFile is CFGFile cfile)
            {
                if (eve.ParentLine != null)
                {
                    Utilities.OpenInEditor(ref cfile.FilePath, line: eve.ParentLine.LineNumber);
                }
                else
                {
                    Utilities.OpenInEditor(ref cfile.FilePath);
                }
            }
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OpenSelectedRowInFileViewer();
        }

        ~EventHistory()
        {
            _workspace = null;
            _workspaceType = default;
            _currentfile = null;
        }

    }
}
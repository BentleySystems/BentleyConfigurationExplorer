// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using WorkspaceCFG.My;
using System.Collections.Generic;

namespace WorkspaceCFG
{

    public partial class VariableDossier
    {

        private CFGEnums.ConfigurationType _workspaceType;
        private CFGVariable _variable;
        private string _variableName;

        public VariableDossier(CFGEnums.ConfigurationType inwrktype, ref string variableName)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            _workspaceType = inwrktype;
            _variableName = variableName;

            RefreshForm();
        }

        ~VariableDossier()
        {
            _workspaceType = default;
        }

        private void GetChildrenList(ref CFGVariable @var, ref List<SubVariable> childlist)
        {
            // Find all children of this variable

            if (@var.GetChildren() is null)
                return;

            foreach (SubVariable cvar in @var.GetChildren())
            {
                if (!childlist.Exists(sv => sv.Variable.Name == cvar.Variable.Name))
                {
                    childlist.Add(cvar);
                    GetChildrenList(ref cvar.Variable, ref childlist);
                }
            }
        }

        private void GetParentList(ref CFGVariable @var, ref List<SubVariable> parentlist)
        {
            // Find all parents of this variable

            if (@var.GetParents() is null)
                return;

            foreach (SubVariable cvar in @var.GetParents())
            {
                if (!parentlist.Exists(sv => sv.Variable.Name == cvar.Variable.Name))
                {
                    parentlist.Add(cvar);
                    GetParentList(ref cvar.Variable, ref parentlist);
                }
            }
        }

        public void RefreshForm()
        {
            CFGConfiguration workspace = null;
            int t;

            InterfaceControler.GetConfiguration(ref workspace, _workspaceType);
            if (workspace is null)
                return;

            _variable = workspace.GetVariable(_variableName);

            if (_variable is null)
                return;

            Text = "Variable Dossier - " + workspace.Description2;

            TextBox1.Text = _variable.Name;

            Label9.Text = Utilities.GetLevelExtendedName(_variable.Level);
            if (_variable.IsDefined)
            {
                Label10.Text = "DEFINED";
            }
            else
            {
                Label10.Text = "UNDEFINED";
            }

            if (_variable.IsLocked)
            {
                Label10.Text = Label10.Text + " , LOCKED";
            }
            else
            {
                Label10.Text = Label10.Text + " , UNLOCKED";
            }

            BuildValueGrid();
            BuildHistoryGrid();
            BuildChildGrid();
            BuildParentGrid();

            RichTextBox1.Text = (Utilities.GetVariableLongDescription(_variable.Name) ?? "").Trim();
            RichTextBox1.Text = "";
            ;
           
            /* Cannot convert OnErrorResumeNextStatementSyntax, CONVERSION ERROR: Conversion for OnErrorResumeNextStatement not implemented, please report this issue in 'On Error Resume Next' at character 2086


                        Input:

                                On Error Resume Next

                         */
            for (t = 0; t <= 6; t++)
                RichTextBox1.Text = RichTextBox1.Text + "Level " + t + ":  " + _variable.Values[t] + '\n';
            RichTextBox1.Text = RichTextBox1.Text + "Final Expansion:  " + _variable.FinalExpansion + '\n';

            Label1.Text = Utilities.GetVariableCategory(_variable.Name);
            Label12.Text = Utilities.GetVariableApplication(_variable.Name);
        }

        private void BuildValueGrid()
        {
            string[] exp;
            string[] eexp;
            int t;
            int rcount;
            DataRow dataRow;
            var dataTable = new DataTable();
            long i;

            if (_variable is null)
                return;
            if (_variable.ParentConfiguration is null)
                return;

            dataTable.Columns.Add(new DataColumn("Value"));
            dataTable.Columns.Add(new DataColumn("Expansion"));

            exp = (_variable.Value ?? "").Split(';');
            rcount = exp.Length - 1;

            var loopTo = rcount;
            for (t = 0; t <= loopTo; t++)
            {
                string expansionValue = MacroParser.ParseLine(_variable.ParentConfiguration, 6, exp[t], true);

                eexp = (expansionValue ?? "").Split(';');
                var loopTo1 = eexp.Length - 1;
                for (i = 0L; i <= loopTo1; i++)
                {
                    dataRow = dataTable.NewRow();
                    if (i == 0L)
                    {
                        dataRow[0] = exp[t];
                    }
                    else
                    {
                        dataRow[0] = " ";
                    }

                    dataRow[1] = eexp[(int)i];
                    dataTable.Rows.Add(dataRow);
                }
            }
            DataGridView1.DataSource = dataTable;
            DataGridView1.Refresh();
        }

        private void BuildHistoryGrid()
        {
            // Fills the history grid by finding events that refer to this variable

            DataRow dataRow;
            var dataTable = new DataTable();
            string uid;

            dataTable.Columns.Add(new DataColumn("ID"));
            dataTable.Columns.Add(new DataColumn("Event"));
            dataTable.Columns.Add(new DataColumn("File"));
            dataTable.Columns.Add(new DataColumn("Line"));
            dataTable.Columns.Add(new DataColumn("Level"));
            dataTable.Columns.Add(new DataColumn("Value"));
            dataTable.Columns.Add(new DataColumn("Expansion"));

            if (_variable is null)
                return;
            if (_variable.ParentConfiguration is null)
                return;
            dataTable.BeginLoadData();
            foreach (CFGEvent ev in _variable.ParentConfiguration.CFGEvents)
            {
                if (ev.Variable is not null && CultureInfo.CurrentCulture.CompareInfo.Compare(ev.Variable.Name ?? "", _variable.Name ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                {
                    dataRow = dataTable.NewRow();
                    uid = "0000" + ev.UID.ToString();
                    dataRow[0] = uid.Substring(uid.Length - 4);
                    dataRow[1] = ev.Description;
                    if (ev.ParentFile is not null)
                        dataRow[2] = ev.ParentFile.FilePath;
                    if (ev.ParentLine is not null)
                        dataRow[3] = ev.ParentLine.LineNumber;
                    dataRow[4] = " ";
                    if (ev.Level > -999)
                        dataRow[4] = Utilities.GetLevelExtendedName(ev.Level);
                    dataRow[5] = ev.VarValue;
                    dataRow[6] = ev.VarExpansion;
                    dataTable.Rows.Add(dataRow);
                }
            }

            // Add a FINAL VALUE row
            dataRow = dataTable.NewRow();
            dataRow[0] = "-";
            dataRow[1] = "FINAL VALUE";
            dataRow[2] = "-";
            dataRow[3] = "-";
            dataRow[4] = Utilities.GetLevelExtendedName(_variable.Level);
            dataRow[5] = _variable.Value;
            dataRow[6] = _variable.FinalExpansion;
            dataTable.Rows.Add(dataRow);

            dataTable.EndLoadData();

            {
                var withBlock = DataGridView2;
                withBlock.DataSource = dataTable;
                withBlock.Refresh();
            }
        }

        private void BuildChildGrid()
        {
            // Fills the children grid

            var children = new List<SubVariable>();
            DataRow dataRow;
            var dataTable = new DataTable();

            GetChildrenList(ref _variable, ref children);

            dataTable.Columns.Add(new DataColumn("Children"));

            if (children.Count == 0)
            {
                dataRow = dataTable.NewRow();
                dataRow[0] = "No child variables";
                dataTable.Rows.Add(dataRow);
            }
            else
            {
                foreach (SubVariable cvar in children)
                {
                    dataRow = dataTable.NewRow();
                    dataRow[0] = cvar.Variable.Name;
                    dataTable.Rows.Add(dataRow);
                }
            }

            DataGridView4.DataSource = dataTable;
            DataGridView4.Refresh();
            DataGridView4.Columns[0].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        }

        private void BuildParentGrid()
        {
            // Fills the parent grid

            var parents = new List<SubVariable>();
            DataRow dataRow;
            var dataTable = new DataTable();

            GetParentList(ref _variable, ref parents);

            dataTable.Columns.Add(new DataColumn("Parents"));

            if (parents.Count == 0)
            {
                dataRow = dataTable.NewRow();
                dataRow[0] = "No parent variables";
                dataTable.Rows.Add(dataRow);
            }
            else
            {
                foreach (SubVariable cvar in parents)
                {
                    dataRow = dataTable.NewRow();
                    dataRow[0] = cvar.Variable.Name;
                    dataTable.Rows.Add(dataRow);
                }
            }

            DataGridView3.DataSource = dataTable;
            DataGridView3.Refresh();
            DataGridView3.Columns[0].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        }

        private void DataGridView4_CellContentDoubleClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            VariableDossier variableDossier;
            CFGVariable variable;
            string variableName;

            if (_variable is null)
                return;

            if (_variable.ParentConfiguration is null)
                return;

            {
                var withBlock = DataGridView4;
                if (withBlock.SelectedCells.Count == 0)
                    return;
                variableName = Convert.ToString(withBlock.SelectedCells[0].Value);
                if (string.IsNullOrEmpty(variableName))
                    return;
                variable = _variable.ParentConfiguration.GetVariable(variableName);
                if (CultureInfo.CurrentCulture.CompareInfo.Compare(variableName, "No child variables", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                    return;
                if (_variable is not null)
                {
                    variableDossier = new VariableDossier(_workspaceType, ref variableName);
                    if (InterfaceControler.GroupWindowsInMainForm)
                        variableDossier.MdiParent = MdiParent;
                    variableDossier.Show();
                }

            }
        }

        private void DataGridView3_CellContentDoubleClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            VariableDossier variableDossier;
            CFGVariable variable;
            string variableName;

            if (_variable is null)
                return;

            if (_variable.ParentConfiguration is null)
                return;

            {
                var withBlock = DataGridView3;
                if (withBlock.SelectedCells.Count == 0)
                    return;
                variableName = Convert.ToString(withBlock.SelectedCells[0].Value);
                if (string.IsNullOrEmpty(variableName))
                    return;
                if (CultureInfo.CurrentCulture.CompareInfo.Compare(variableName, "No parent variables", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                    return;
                variable = _variable.ParentConfiguration.GetVariable(variableName);
                if (_variable is not null)
                {
                    variableDossier = new VariableDossier(_workspaceType, ref variableName);
                    if (InterfaceControler.GroupWindowsInMainForm)
                        variableDossier.MdiParent = MdiParent;
                    variableDossier.Show();
                }

            }
        }

        private void GoToLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_variable == null)
                return;

            var workspace = _variable.ParentConfiguration;
            if (workspace == null)
                return;

            var withBlock = DataGridView2;
            if (withBlock.SelectedRows.Count == 0)
                return;

            var selectedValue = withBlock.SelectedRows[0].Cells[0].Value?.ToString();
            if (string.Equals(selectedValue, "-", StringComparison.OrdinalIgnoreCase))
                return;

            if (!long.TryParse(selectedValue, out long eid))
                return;

            if (eid < 0 || eid >= workspace.CFGEvents.Count)
                return;

            var cfgEvent = workspace.CFGEvents[(int)eid] as CFGEvent;
            if (cfgEvent == null || cfgEvent.ParentFile == null || cfgEvent.ParentLine == null)
                return;

            var cfgFile = cfgEvent.ParentFile;
            var fileViewer = new FileViewer(_workspaceType, ref cfgFile.FilePath, cfgEvent.ParentLine.LineNumber - 1)
            {
                MdiParent = InterfaceControler.GroupWindowsInMainForm ? MdiParent : null
            };

            fileViewer.CheckBox1.Checked = true;
            fileViewer.Show();
        }

        private void OpenFileInViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_variable == null)
                return;

            var workspace = _variable.ParentConfiguration;
            if (workspace == null)
                return;

            var withBlock = DataGridView2;
            if (withBlock.SelectedRows.Count == 0)
                return;

            var selectedValue = withBlock.SelectedRows[0].Cells[0].Value?.ToString();
            if (string.Equals(selectedValue, "-", StringComparison.OrdinalIgnoreCase))
                return;

            if (!long.TryParse(selectedValue, out long eid))
                return;

            if (eid < 0 || eid >= workspace.CFGEvents.Count)
                return;

            var cfgEvent = workspace.CFGEvents[(int)eid] as CFGEvent;
            if (cfgEvent?.ParentFile == null || cfgEvent.ParentLine == null)
                return;

            var cfgFile = cfgEvent.ParentFile;
            var fileViewer = new FileViewer(_workspaceType, ref cfgFile.FilePath)
            {
                MdiParent = InterfaceControler.GroupWindowsInMainForm ? MdiParent : null
            };

            fileViewer.CheckBox1.Checked = true;
            fileViewer.Show();
        }

        private void VariableReport_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            object argform = this;
            InterfaceControler.RemoveForm(ref argform);
            MySettings.Default.VR_Location = Location;
            MySettings.Default.VR_WindowState = WindowState;
            if (WindowState == System.Windows.Forms.FormWindowState.Normal)
                MySettings.Default.VR_Size = Size;
            MySettings.Default.Save();
        }

        private void VariableReport_Shown(object sender, EventArgs e)
        {
            object argform = this;
            InterfaceControler.AddForm(ref argform);
        }

        private void WatchF5_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.F5)
            {
                InterfaceControler.RefreshOpenForms();
            }
        }

        private void VariableReport_Load(object sender, EventArgs e)
        {
            Location = new Point(MySettings.Default.VR_Location.X + 20, MySettings.Default.VR_Location.Y + 28);
            MySettings.Default.VR_Location = Location;
            MySettings.Default.Save();
            WindowState = MySettings.Default.VR_WindowState;
            if (WindowState == System.Windows.Forms.FormWindowState.Normal)
            {
                // VR_Size is a shared setting used by several dialogs, so its persisted value
                // may be smaller than this dialog needs to show its grids comfortably. Prefer
                // whichever is taller/wider between the saved size and this dialog's own
                // preferred starting size, without permanently restricting how small the user
                // can later resize it (that is governed by MinimumSize).
                var preferredSize = new Size(1010, 1050);
                var savedSize = MySettings.Default.VR_Size;
                Size = new Size(Math.Max(savedSize.Width, preferredSize.Width), Math.Max(savedSize.Height, preferredSize.Height));
            }

            // Force an explicit layout pass after programmatically resizing the form, since
            // the TableLayoutPanel rows don't always re-cascade bounds to their children
            // when Size is assigned directly rather than via a user resize.
            PerformLayout();
        }

        private void ToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path;

            if (_variable.ParentConfiguration is null)
                return;

            {
                var withBlock = SaveFileDialog1;
                withBlock.Title = "Save Variable List to Excel";
                withBlock.InitialDirectory = @"C:\";
                withBlock.Filter = "Excel File (*.xls)|*.xls|Excel File (*.xls)|*.xls";
                withBlock.FileName = _variable.ParentConfiguration.ApplicationData.Name + "-" + _variable.ParentConfiguration.ApplicationData.Workspace + "-" + _variable.ParentConfiguration.ApplicationData.Workset + ".xls";
                withBlock.RestoreDirectory = true;
                if (withBlock.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    return;

                path = withBlock.FileName;
                withBlock.Dispose();
            }

            string argSheetName = "Variable History";
            ExcelExporter.DataGridExport(DataGridView2, ref argSheetName, path);
            System.Windows.Forms.MessageBox.Show("File export complete.", "Done");
        }

        private void OpenFileInEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_variable == null)
                return;

            var workspace = _variable.ParentConfiguration;
            if (workspace == null)
                return;

            var withBlock = DataGridView2;
            if (withBlock.SelectedRows.Count == 0)
                return;

            var selectedValue = withBlock.SelectedRows[0].Cells[0].Value?.ToString();
            if (string.Equals(selectedValue, "-", StringComparison.OrdinalIgnoreCase))
                return;

            if (!long.TryParse(selectedValue, out long eid))
                return;

            if (eid < 0 || eid >= workspace.CFGEvents.Count)
                return;

            var cfgEvent = workspace.CFGEvents[(int)eid] as CFGEvent;
            if (cfgEvent?.ParentFile == null)
                return;

            var cfgFile = cfgEvent.ParentFile;

            if (cfgEvent.ParentLine != null)
            {
                Utilities.OpenInEditor(ref cfgFile.FilePath, line: cfgEvent.ParentLine.LineNumber);
            }
            else
            {
                Utilities.OpenInEditor(ref cfgFile.FilePath);
            }
        }

        private void ToolStripMenuItem7_Click(object sender, EventArgs e)
        {
            // Open the selected variable locations list
            Locations locations;
            string value;

            {
                var withBlock = DataGridView1;
                if (withBlock.SelectedCells.Count == 0)
                    return;
                value = Convert.ToString(withBlock.SelectedCells[0].OwningRow.Cells[0].Value);
                if (string.IsNullOrEmpty(value))
                    return;
                locations = new Locations(_workspaceType, ref _variableName, ref value);
                if (InterfaceControler.GroupWindowsInMainForm)
                    locations.MdiParent = MdiParent;
                locations.Show();
            }
        }

        private void ViewLocationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open the selected variable locations list
            Locations locations;
            string value;

            {
                var withBlock = DataGridView2;
                if (withBlock.SelectedRows.Count == 0)
                    return;

                value = withBlock.SelectedRows[0].Cells[5].Value.ToString();
                if (string.IsNullOrEmpty(value))
                    return;
                locations = new Locations(_workspaceType, ref _variableName, ref value);
                if (InterfaceControler.GroupWindowsInMainForm)
                    locations.MdiParent = MdiParent;
                locations.Show();
            }
        }

        private void ViewDefinitionInEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open the selected variable locations list

            string value;
            long owningRowIndex;

            if (_variable is null)
                return;

            try
            {

                {
                    var withBlock = DataGridView1;
                    if (withBlock.SelectedCells.Count == 0)
                        return;
                    owningRowIndex = withBlock.SelectedCells[0].OwningRow.Index;
                    while (owningRowIndex >= 0L && string.IsNullOrEmpty((Convert.ToString(withBlock.Rows[(int)owningRowIndex].Cells[0].Value) ?? "").Trim()))
                        owningRowIndex -= 1L;
                    value = Convert.ToString((withBlock.Rows[(int)owningRowIndex].Cells[0].Value));
                    value = value.ToLower();

                    if (string.IsNullOrEmpty(value))
                        return;
                }

                foreach (CFGEvent ev in _variable.ParentConfiguration.CFGEvents)
                {
                    if (ev.Variable != null &&
                        string.Equals(ev.Variable.Name ?? "", _variable.Name ?? "", StringComparison.CurrentCultureIgnoreCase) &&
                        !string.IsNullOrEmpty(ev.VarValue))
                    {
                        string evValueLower = ev.VarValue.ToLowerInvariant();
                        string valueLower = value.ToLowerInvariant();

                        if (evValueLower.Contains(valueLower) || valueLower.Contains(evValueLower))
                        {
                            Utilities.OpenInEditor(ref ev.ParentFile.FilePath, line: ev.ParentLine.LineNumber);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
        }

    }
}
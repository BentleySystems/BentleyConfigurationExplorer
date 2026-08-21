// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using WorkspaceCFG.My;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{
    // ---------------------------------------------------------------------------------------
    // 
    // Variable Explorer Form
    // 
    // 
    // ---------------------------------------------------------------------------------------

    public partial class VariableExplorer
    {

        // ---------------------------------------------------------------------------------------
        // Private Members
        // ---------------+---------------+---------------+---------------+---------------+-------
        private CFGConfiguration _workspace;
        private CFGEnums.ConfigurationType _workspaceType;
        private int _gotoRow;                 // Stores the current row index
        private int _gotoCell;                // Stores the current cell index
        private bool _suppressSelectionChanged;

        // ---------------------------------------------------------------------------------------
        // @description: Initializes members
        // ---------------+---------------+---------------+---------------+---------------+-------
        public VariableExplorer(CFGEnums.ConfigurationType workspaceType)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            _workspaceType = workspaceType;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Form Load event reads settings and fills it with data.
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void VariableExplorer_Load(object sender, EventArgs e)
        {
            // Fill Form with data
            RefreshForm();

            // Load form location, state, and size
            Location = MySettings.Default.VE_Location;
            WindowState = MySettings.Default.VE_WindowState;
            if (WindowState == System.Windows.Forms.FormWindowState.Normal)
                Size = MySettings.Default.VE_Size;

            // Remember splitter. Clamp to the container's valid range so a value saved under a
            // different DPI/monitor/window size can never exceed the panel bounds (which would
            // throw or collapse a panel to zero height).
            int minDist = SplitContainer1.Panel1MinSize;
            int maxDist = Math.Max(minDist, SplitContainer1.Height - SplitContainer1.Panel2MinSize - SplitContainer1.SplitterWidth);
            if (MySettings.Default.VE_Split1_Dist > 0)
                SplitContainer1.SplitterDistance = Math.Min(Math.Max(MySettings.Default.VE_Split1_Dist, minDist), maxDist);

            // Force an explicit layout pass after programmatically resizing the form: the
            // nested SplitContainer1 -> SplitContainer2 hierarchy doesn't always re-cascade
            // anchor-based bounds to its grandchildren when Size is assigned directly (as
            // opposed to a user-driven resize), which can leave the innermost anchored
            // controls (RichTextBox1/2/3) stuck at their design-time bounds.
            PerformLayout();

            // Load column view settings for the dgvariables datagridview
            {
                var withBlock = MySettings.Default;
                CatagoryToolStripMenuItem.Checked = withBlock.VE_C1;
                ApplicationToolStripMenuItem.Checked = withBlock.VE_C2;
                LevelToolStripMenuItem.Checked = withBlock.VE_C3;
                LengthToolStripMenuItem.Checked = withBlock.VE_C4;
                ValueToolStripMenuItem.Checked = withBlock.VE_C5;
                ExpansionToolStripMenuItem.Checked = withBlock.VE_C6;
                ShowMultilineVariablesToolStripMenuItem.Checked = withBlock.VE_MULTILINE;
            }

            // Initialize connection to Interface Controller
            object argform = this;
            InterfaceControler.AddForm(ref argform);
        }

        // ---------------------------------------------------------------------------------------
        // @description: Fills the form with data
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void RefreshForm()
        {
            // Get a reference to the workspace object
            InterfaceControler.GetConfiguration(ref _workspace, _workspaceType);
            if (_workspace is null)
                return;

            // Set form caption
            Text = CEResource.TXT_TitleVariableExplorer + _workspace.Description2;

            // Save the index to the selected row and column
            if (dgVariables.SelectedCells.Count > 0)
            {
                _gotoRow = dgVariables.SelectedCells[0].OwningRow.Index;
                _gotoCell = dgVariables.SelectedCells[0].ColumnIndex;
            }

            // Load the variable grid with data
            BuildVariableGrid();

            // Reselect the row and column that was selected before the refresh
            if (_gotoRow != 0 && _gotoRow < dgVariables.Rows.Count && dgVariables.Rows[_gotoRow].Cells[_gotoCell].Visible)
            {
                dgVariables.CurrentCell = dgVariables.Rows[_gotoRow].Cells[_gotoCell];
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Builds the variable datagridview
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void BuildVariableGrid()
        {
            DataTable dataTable;
            DataRow dataRow;
            bool usevar;
            string nameFilter;
            string expansionFilter;

            dataTable = new DataTable();

            VarDB.EnsureLoaded();
            var variableInfoByName = new Dictionary<string, CFGEnums.VarInfo>(StringComparer.Ordinal);
            foreach (CFGEnums.VarInfo variableInfo in VarDB.VariableDB)
            {
                if (variableInfo.Name is not null && !variableInfoByName.ContainsKey(variableInfo.Name))
                    variableInfoByName.Add(variableInfo.Name, variableInfo);
            }

            // Add columns
            {
                var withBlock = dataTable.Columns;
                withBlock.Add(new DataColumn(CEResource.TXT_DCVariable));          // 0
                withBlock.Add(new DataColumn(CEResource.TXT_DCCategory));          // 1
                withBlock.Add(new DataColumn(CEResource.TXT_DCApplication));       // 2
                withBlock.Add(new DataColumn(CEResource.TXT_DCLevel));             // 3
                withBlock.Add(new DataColumn(CEResource.TXT_DCLength));            // 4
                withBlock.Add(new DataColumn(CEResource.TXT_DCValue));             // 5
                withBlock.Add(new DataColumn(CEResource.TXT_DCExpansion));         // 6
            }

            // Remove wildcards from the filter setup
            nameFilter = (tbNameFilter.Text ?? "").Trim();
            expansionFilter = (tbExpantionFilter.Text ?? "").Trim();

            dataTable.BeginLoadData();
            foreach (CFGVariable variable in _workspace.Variables.Values)
            {
                usevar = true;

                // Name filter
                if (!string.IsNullOrWhiteSpace(nameFilter))
                {
                    if (string.IsNullOrWhiteSpace(variable.Name) ||
                        variable.Name.IndexOf(nameFilter, StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        usevar = false;
                    }
                }

                // Expansion filter
                if (!string.IsNullOrWhiteSpace(expansionFilter))
                {
                    // Exclude if expansion is null/empty/whitespace or does not match filter
                    if (string.IsNullOrWhiteSpace(variable.FinalExpansion) ||
                        variable.FinalExpansion.IndexOf(expansionFilter, StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        usevar = false;
                    }
                }

                if (usevar)
                {
                    dataRow = dataTable.NewRow();
                    variableInfoByName.TryGetValue(variable.Name, out CFGEnums.VarInfo variableInfo);
                    dataRow[0] = variable.Name;
                    dataRow[1] = variableInfo.Category ?? "";
                    dataRow[2] = variableInfo.App ?? "";
                    dataRow[3] = Utilities.GetLevelExtendedName(variable.Level);
                    dataRow[4] = variable.FinalExpansion?.Length ?? 0;

                    if (ShowMultilineVariablesToolStripMenuItem.Checked)
                    {
                        dataRow[5] = variable.Value?.Replace(";", "\n");
                        dataRow[6] = variable.FinalExpansion?.Replace(";", "\n");
                    }
                    else
                    {
                        dataRow[5] = variable.Value;
                        dataRow[6] = variable.FinalExpansion;
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }


            dataTable.EndLoadData();

            // Sort the data before binding so the grid only creates its final row order once.
            dataTable.DefaultView.Sort = $"[{CEResource.TXT_DCVariable.Replace("]", "]]", StringComparison.Ordinal)}] ASC";

            // Set the row count caption
            Label6.Text = CEResource.TXT_SingleVariable;
            if (dataTable.Rows.Count > 1)
                Label6.Text = dataTable.Rows.Count + CEResource.TXT_MultipleVariables;

            // Assign the datatable to the datagridview
            _suppressSelectionChanged = true;
            try
            {
                dgVariables.DataSource = dataTable;

                // Load the gridview columns display settings
                UpdateDGColumnViews();
            }
            finally
            {
                _suppressSelectionChanged = false;
            }

            {
                var withBlock1 = dgVariables;
                withBlock1.Columns[5].DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                withBlock1.Columns[6].DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                withBlock1.AutoResizeColumn(0, System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells);
                withBlock1.AutoResizeColumn(1, System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells);
                withBlock1.AutoResizeColumn(2, System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells);
                withBlock1.AutoResizeColumn(3, System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells);
                withBlock1.AutoResizeColumn(4, System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells);
                withBlock1.Columns[5].MinimumWidth = 250;
                withBlock1.Columns[5].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                withBlock1.Columns[6].MinimumWidth = 250;
                withBlock1.Columns[6].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            }

            DgVariables_SelectionChanged(dgVariables, EventArgs.Empty);
        }

        // ---------------------------------------------------------------------------------------
        // @description: Opens a select variable in the dossier
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void OpenVariable()
        {
            // Open the selected variable dossier

            string varname;
            CFGVariable variable;

            if (_suppressSelectionChanged || _workspace is null)
                return;

            if (dgVariables.SelectedCells.Count == 0)
                return;
            varname = Convert.ToString(dgVariables.SelectedCells[0].OwningRow.Cells[0].Value);
            variable = _workspace.GetVariable(varname);
            if (variable is null)
                return;

            var vdos = new VariableDossier(_workspaceType, ref varname);
            //vdos.MdiParent = MdiParent;
            vdos.Show();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Handles the name filter change event
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void TbNameFilter_TextChanged(object sender, EventArgs e)
        {
            BuildVariableGrid();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Handles the expansion filter change event
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void TbExpantionFilter_TextChanged(object sender, EventArgs e)
        {
            BuildVariableGrid();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Handles the name filter clear button event
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void Button1_Click(object sender, EventArgs e)
        {
            tbNameFilter.Text = "";
        }

        // ---------------------------------------------------------------------------------------
        // @description: Handles the expansion filter clear button event
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void Button2_Click(object sender, EventArgs e)
        {
            tbExpantionFilter.Text = "";
        }

        // ---------------------------------------------------------------------------------------
        // @description: Openes the selected variable when a cell is double clicked
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void DataGridView1_CellContentDoubleClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            OpenVariable();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Openes the selected variable when a cell is the enter key is pressed
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void DgVariables_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                OpenVariable();
                e.Handled = true;
                return;
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Jumps to a variable when a key is pressed.
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void DgVariables_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == 13)
            {
                e.Handled = true;
                return;
            }

            string key;
            key = Convert.ToString(char.ToUpper(e.KeyChar, System.Globalization.CultureInfo.CurrentCulture));
            foreach (System.Windows.Forms.DataGridViewRow row in dgVariables.Rows)
            {
                //if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(row.Cells[0].Value(0), key, true)))
                if (row.Cells[0].Value?.ToString() == key?.ToString())
                {
                    dgVariables.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Fills value, expansion, and description textboxes when a variable is selected
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void DgVariables_SelectionChanged(object sender, EventArgs e)
        {
            string[] exp;
            string varname;
            CFGVariable variable;

            if (_workspace is null)
                return;
            if (dgVariables.SelectedCells.Count == 0)
                return;

            // Get selected variable name
            varname = Convert.ToString(dgVariables.SelectedCells[0].OwningRow.Cells[0].Value);

            // Get a reference to the variable
            variable = _workspace.GetVariable(varname);
            if (variable is null)
                return;

            // Fill the value richtextbox
            exp = (variable.Value ?? "").Split(';');
            RichTextBox2.Text = string.Join('\n', exp);

            // Fill the expansion richtextbox
            exp = (variable.FinalExpansion ?? "").Split(';');
            RichTextBox1.Text = string.Join('\n', exp);

            // Fill description
            RichTextBox3.Text = (Utilities.GetVariableLongDescription(varname) ?? "").Trim();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Form closing event: saves settings, clears out members
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void VariableExplorer_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            // Save form settings
            {
                var withBlock = MySettings.Default;
                withBlock.VE_C1 = CatagoryToolStripMenuItem.Checked;
                withBlock.VE_C2 = ApplicationToolStripMenuItem.Checked;
                withBlock.VE_C3 = LevelToolStripMenuItem.Checked;
                withBlock.VE_C4 = LengthToolStripMenuItem.Checked;
                withBlock.VE_C5 = ValueToolStripMenuItem.Checked;
                withBlock.VE_C6 = ExpansionToolStripMenuItem.Checked;
                withBlock.VE_MULTILINE = ShowMultilineVariablesToolStripMenuItem.Checked;
                withBlock.VE_Location = Location;
                withBlock.VE_WindowState = WindowState;
                withBlock.VE_Split1_Dist = SplitContainer1.SplitterDistance;
                if (WindowState == System.Windows.Forms.FormWindowState.Normal)
                    withBlock.VE_Size = Size;
                withBlock.Save();
            }

            // Remove this form instance from the interface controller
            object argform = this;
            InterfaceControler.RemoveForm(ref argform);

            // Cleanup
            _workspaceType = default;
            _workspace = null;
            ((dynamic)dgVariables.DataSource).Dispose();
            dgVariables.DataSource = null;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Rescans the workspace and refreshes all forms when F5 is pressed
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void WatchF5_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.F5)
            {
                InterfaceControler.RefreshOpenForms();
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Saves the variable datagrid to excel
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void ToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_workspace is null)
                return;

            string path;

            var saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Title = CEResource.TXT_MsgSaveVariableListToExcel;
            saveFileDialog.InitialDirectory = CEResource.TXT_CDrive;
            saveFileDialog.Filter = CEResource.TXT_ExtSaveWorkSpaceFilter;
            saveFileDialog.FileName = _workspace.ApplicationData.Name + "-" + _workspace.ApplicationData.Workspace + "-" + _workspace.ApplicationData.Workset + ".xls";
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            path = saveFileDialog.FileName;
            saveFileDialog.Dispose();

            string argSheetName = CEResource.TXT_VariableExplorerTitle;
            ExcelExporter.DataGridExport(dgVariables, ref argSheetName, path);
            System.Windows.Forms.MessageBox.Show(CEResource.TXT_MsgFileExportComplete, CEResource.TXT_MsgDone);
        }

        // ---------------------------------------------------------------------------------------
        // @description: Opens a variable in the dossier when selected in the context menu
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void OpenVariableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenVariable();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Open the selected variable locations list
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void ViewLocationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open the selected variable locations list
            Locations locations;
            string varname;
            CFGVariable variable;

            if (_workspace is null)
                return;

            if (dgVariables.SelectedCells.Count == 0)
                return;
            varname = Convert.ToString(dgVariables.SelectedCells[0].OwningRow.Cells[0].Value);
            variable = _workspace.GetVariable(varname);
            if (variable is null)
                return;
            string argvalue = null;
            locations = new Locations(_workspaceType, ref varname, value: ref argvalue);
            if (InterfaceControler.GroupWindowsInMainForm)
                locations.MdiParent = MdiParent;

            locations.Show();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Copy the selected cell to the clipboard
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgVariables.SelectedCells.Count == 0)
                return;

            try
            {
                System.Windows.Forms.Clipboard.SetText(Convert.ToString(dgVariables.SelectedCells[0].Value));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Opens a context menu based on the location of the cursor
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void DgVariables_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Location.Y <= 18)
            {
                dgVariables.ContextMenuStrip = cmColumnChooser;          // Opens the column chooser context menu
            }
            else
            {
                dgVariables.ContextMenuStrip = cmVariableCommands;
            }        // Opens the variable context menu
        }

        // ---------------------------------------------------------------------------------------
        // @description: Hide or show columns in the datagridview.
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void CatagoryToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (dgVariables.Columns.Count >= 1)
                dgVariables.Columns[1].Visible = CatagoryToolStripMenuItem.Checked;
        }

        private void ApplicationToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (dgVariables.Columns.Count >= 2)
                dgVariables.Columns[2].Visible = ApplicationToolStripMenuItem.Checked;
        }

        private void LevelToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (dgVariables.Columns.Count >= 3)
                dgVariables.Columns[3].Visible = LevelToolStripMenuItem.Checked;
        }

        private void LengthToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (dgVariables.Columns.Count >= 4)
                dgVariables.Columns[4].Visible = LengthToolStripMenuItem.Checked;
        }

        private void ValueToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (dgVariables.Columns.Count >= 5)
                dgVariables.Columns[5].Visible = ValueToolStripMenuItem.Checked;
        }

        private void ExpansionToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (dgVariables.Columns.Count >= 6)
                dgVariables.Columns[6].Visible = ExpansionToolStripMenuItem.Checked;
        }

        private void ShowMultilineVariablesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            RefreshForm();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Updates the datagridview column visibility.
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void UpdateDGColumnViews()
        {
            dgVariables.Columns[1].Visible = CatagoryToolStripMenuItem.Checked;
            dgVariables.Columns[2].Visible = ApplicationToolStripMenuItem.Checked;
            dgVariables.Columns[3].Visible = LevelToolStripMenuItem.Checked;
            dgVariables.Columns[4].Visible = LengthToolStripMenuItem.Checked;
            dgVariables.Columns[5].Visible = ValueToolStripMenuItem.Checked;
            dgVariables.Columns[6].Visible = ExpansionToolStripMenuItem.Checked;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Overrides the tool tip value for the datagrid to display the lengths of
        // each of the variable's expansions.
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void DgVariables_CellToolTipTextNeeded(object sender, System.Windows.Forms.DataGridViewCellToolTipTextNeededEventArgs e)
        {
            // Shows the length of each expansion in a variable
            string[] mis;
            CFGVariable variable;

            if (_workspace is null)
                return;
            if (e.RowIndex <= -1)
                return;

            if (e.ColumnIndex == 4)
            {
                variable = _workspace.GetVariable(Convert.ToString(dgVariables.Rows[e.RowIndex].Cells[0].Value));
                if (variable is not null)
                {
                    e.ToolTipText = "";
                    mis = (variable.FinalExpansion ?? "").Split(';');
                    foreach (string vl in mis)
                        e.ToolTipText = e.ToolTipText + vl.Length.ToString() + '\n';
                    if (e.ToolTipText.Length > 0)
                        e.ToolTipText = e.ToolTipText.Substring(0, e.ToolTipText.Length - 1);
                }
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Selects a cell in the datagridview on a mouse right click event.
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void DgVariables_CellMouseDown(object sender, System.Windows.Forms.DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
                dgVariables.CurrentCell = dgVariables.Rows[e.RowIndex].Cells[e.ColumnIndex];
        }

        private void ToTextFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkspaceExport.SaveWorkspaceVariables(ref _workspace);
        }

    }
}
// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{
    // ---------------------------------------------------------------------------------------
    // 
    // Variable Compare Form - Compares two workspaces for differences
    // 
    // 
    // ---------------------------------------------------------------------------------------
    public partial class VariableCompare
    {

        // ---------------------------------------------------------------------------------------
        // Private Members
        // ---------------+---------------+---------------+---------------+---------------+-------
        private CFGConfiguration _workspace1;             // Reference to the first workspace to be compared
        private CFGConfiguration _workspace2;             // Reference to the second workspace to be compared
        private bool _ignoreUpdate;
        private int _gotoRow;
        private ArrayList _variableList;

        public VariableCompare()
        {
            // This call is required by the Windows Form Designer.
            _ignoreUpdate = true;
            InitializeComponent();

            _ignoreUpdate = false;

            object argform = this;
            InterfaceControler.AddForm(ref argform);

            RefreshForm();
        }

        ~VariableCompare()
        {
            _workspace1 = null;
            _workspace2 = null;
            _variableList = null;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Fills the form with data
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void RefreshForm()
        {
            InterfaceControler.GetConfiguration(ref _workspace1, CFGEnums.ConfigurationType.Compare1);
            InterfaceControler.GetConfiguration(ref _workspace2, CFGEnums.ConfigurationType.Compare2);
            if (_workspace1 is null || _workspace2 is null)
                return;

            // Save selection
            if (DataGridView1.SelectedRows.Count > 0)
            {
                _gotoRow = DataGridView1.SelectedRows[0].Index;
            }

            // Initialize variable list
            BuildVarList();

            _ignoreUpdate = false;

            // Build grid views
            Build_Grid_Views();

            LBwrk1Desc.Text = _workspace1.Description2;
            LBwrk2Desc.Text = _workspace2.Description2;

            // Recall selection
            if (_gotoRow != 0)
            {
                if (_gotoRow < DataGridView1.Rows.Count)
                {
                    DataGridView1.CurrentCell = DataGridView1.Rows[_gotoRow].Cells[0];
                    DataGridView2.CurrentCell = DataGridView2.Rows[_gotoRow].Cells[0];
                }
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Apply color to the datagrid cells
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void ApplyColor(ref DataGridView dataGridView)
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                var cellValue = dataGridView.Rows[i].Cells[2].Value?.ToString()?.ToUpperInvariant();

                if (cellValue == "RED")
                {
                    dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.DarkRed;
                    dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                }
                else if (cellValue == "MISSING")
                {
                    dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.LightGray;
                }
                else if (cellValue == "ORANGE")
                {
                    dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.DarkOrange;
                }
                else if (cellValue == "BLUE")
                {
                    dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;
                }
            }
        }

        private void Build_Grid_Views()
        {
            if (_ignoreUpdate)
                return;
            BuildDataTables();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Compares the workspaces and builds the datatables for the gridviews
        // ---------------+---------------+---------------+---------------+---------------+--
        private void BuildDataTables()
        {
            DataTable dataTable1;
            DataTable dataTable2;
            DataRow dataRow1;
            DataRow dataRow2;
            CFGVariable variable1;
            CFGVariable variable2;
            string nfilter;
            bool showrow;
            bool needsCompare;

            dataTable1 = new DataTable();
            dataTable2 = new DataTable();

            // Build Columns
            dataTable1.Columns.Add(new DataColumn(CEResource.TXT_DCVariable));
            dataTable1.Columns.Add(new DataColumn(CEResource.TXT_DCExpansion));
            dataTable1.Columns.Add(new DataColumn(CEResource.TXT_DCResult));
            dataTable2.Columns.Add(new DataColumn(CEResource.TXT_DCVariable));
            dataTable2.Columns.Add(new DataColumn(CEResource.TXT_DCExpansion));
            dataTable2.Columns.Add(new DataColumn(CEResource.TXT_DCResult));

            nfilter = (TextBox1.Text ?? "").Trim().ToUpper();

            // Insert Values
            foreach (string name in _variableList)
            {
                showrow = true;
                if (!string.IsNullOrEmpty(TextBox1.Text))
                {
                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(nfilter) &&
                        !name.ToUpper().Contains(nfilter))
                        showrow = false;
                }

                if (showrow)
                {
                    needsCompare = true;

                    variable1 = _workspace1.GetVariable(name);
                    variable2 = _workspace2.GetVariable(name);

                    // Fill DG 1
                    dataRow1 = dataTable1.NewRow();
                    dataRow1[0] = name;
                    if (variable1 is not null)
                    {
                        if (CheckBox3.Checked)
                        {
                            dataRow1[1] = SetProgramFilesNuetral(ref variable1.FinalExpansion);
                        }
                        else
                        {
                            dataRow1[1] = variable1.FinalExpansion;
                        }
                        dataRow1[2] = "";
                    }
                    else
                    {
                        dataRow1[1] = "";
                        dataRow1[2] = "MISSING";
                        needsCompare = false;
                    }


                    // Fill DG 2
                    dataRow2 = dataTable2.NewRow();
                    dataRow2[0] = name;
                    if (variable2 is not null)
                    {
                        if (CheckBox3.Checked)
                        {
                            dataRow2[1] = SetProgramFilesNuetral(ref variable2.FinalExpansion);
                        }
                        else
                        {
                            dataRow2[1] = variable2.FinalExpansion;
                        }
                        dataRow2[2] = "";
                        if (dataRow1[2]?.ToString().Equals("MISSING", StringComparison.OrdinalIgnoreCase) == true)
                        {
                            dataRow2[2] = "BLUE";
                        }
                    }
                    else
                    {
                        dataRow2[1] = "";
                        dataRow2[2] = "MISSING";
                        dataRow1[2] = "BLUE";
                        needsCompare = false;
                    }

                    // Run Compare
                    showrow = false;
                    if (needsCompare)
                    {
                        if (CB_EXP_DIF.Checked)
                        {
                            if (CheckBox3.Checked)
                            {
                                if (CultureInfo.CurrentCulture.CompareInfo.Compare(SetProgramFilesNuetral(ref variable1.FinalExpansion) ?? "", SetProgramFilesNuetral(ref variable2.FinalExpansion) ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
                                {
                                    dataRow1[2] = "RED";
                                    dataRow2[2] = "RED";
                                    showrow = true;
                                }
                            }
                            else if (CultureInfo.CurrentCulture.CompareInfo.Compare(variable1.FinalExpansion ?? "", variable2.FinalExpansion ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
                            {
                                dataRow1[2] = "RED";
                                dataRow2[2] = "RED";
                                showrow = true;
                            }
                        }
                        if (CB_VAL_DIFF.Checked)
                        {
                            if (CheckBox3.Checked)
                            {
                                if (CultureInfo.CurrentCulture.CompareInfo.Compare(SetProgramFilesNuetral(ref variable1.Value) ?? "", SetProgramFilesNuetral(ref variable2.Value) ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
                                {
                                    dataRow1[2] = "RED";
                                    dataRow2[2] = "RED";
                                    showrow = true;
                                }
                            }
                            else if (CultureInfo.CurrentCulture.CompareInfo.Compare(variable1.Value ?? "", variable2.Value ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
                            {
                                dataRow1[2] = "RED";
                                dataRow2[2] = "RED";
                                showrow = true;
                            }
                        }
                        if (CheckBox2.Checked)
                        {
                            if (variable1.IsDefined == false)
                            {
                                dataRow1[2] = "ORANGE";
                                showrow = true;
                            }
                            if (variable2.IsDefined == false)
                            {
                                dataRow2[2] = "ORANGE";
                                showrow = true;
                            }
                        }
                    }

                    if (CheckBox1.Checked)
                    {
                        if (showrow | needsCompare == false)
                        {
                            dataTable1.Rows.Add(dataRow1);
                            dataTable2.Rows.Add(dataRow2);
                        }
                    }
                    else
                    {
                        dataTable1.Rows.Add(dataRow1);
                        dataTable2.Rows.Add(dataRow2);
                    }

                }
            }

            // Fill Grids with data
            DataGridView1.DataSource = dataTable1;
            DataGridView1.Refresh();
            DataGridView2.DataSource = dataTable2;
            DataGridView2.Refresh();

            DataGridView1.Columns[2].Visible = false;
            DataGridView2.Columns[2].Visible = false;

            // Sort and resize columns
            DataGridView1.Columns[1].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            DataGridView2.Columns[1].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Find all variable names in both workspaces
        // ---------------+---------------+---------------+---------------+---------------+--
        private void BuildVarList()
        {
            if (_workspace1 is null | _workspace2 is null)
                return;

            if (_variableList is null)
                _variableList = new ArrayList();

            var xcol = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _variableList.Clear();

            CFGVariable @var;

            foreach (CFGVariable currentVar in _workspace1.Variables.Values)
            {
                @var = currentVar;
                if (!string.IsNullOrEmpty(@var.Name))
                    xcol.Add(@var.Name);
            }

            foreach (CFGVariable currentVar1 in _workspace2.Variables.Values)
            {
                @var = currentVar1;
                if (!string.IsNullOrEmpty(@var.Name))
                    xcol.Add(@var.Name);
            }

            foreach (string name in xcol)
            {
                if (!string.IsNullOrEmpty(name))
                    _variableList.Add(name);
            }

            _variableList.Sort();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Build value descriptions for the selected variable
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void BuildValueBoxes()
        {
            CFGVariable variable;
            string[] exp;
            int t;
            string varname;
            bool needsCompare;
            string value;

            if (_ignoreUpdate)
                return;
            if (_workspace1 is null | _workspace2 is null)
                return;

            RichTextBox1.Text = "";
            RichTextBox2.Text = "";
            RichTextBox3.Text = "";
            RichTextBox4.Text = "";

            needsCompare = true;
            {
                var withBlock = DataGridView1;
                if (DataGridView1.SelectedRows.Count == 0)
                    return;
                if (withBlock.SelectedRows[0].IsNewRow)
                    return;

                varname = Convert.ToString(withBlock.SelectedRows[0].Cells[0].Value);
                if (string.IsNullOrEmpty(varname))
                    return;

                variable = _workspace1.GetVariable(varname);
                if (variable is not null)
                {
                    if (CheckBox3.Checked)
                    {
                        exp = (SetProgramFilesNuetral(ref variable.Value) ?? "").Split(';');
                    }
                    else
                    {
                        exp = (variable.Value ?? "").Split(';');
                    }
                    value = "";
                    var loopTo = exp.Length - 1;
                    for (t = 0; t <= loopTo; t++)
                        value = value + exp[t] + '\n';
                    RichTextBox2.Text = value;
                    if (CheckBox3.Checked)
                    {
                        exp = (SetProgramFilesNuetral(ref variable.FinalExpansion) ?? "").Split(';');
                    }
                    else
                    {
                        exp = (variable.FinalExpansion ?? "").Split(';');
                    }
                    value = "";
                    var loopTo1 = exp.Length - 1;
                    for (t = 0; t <= loopTo1; t++)
                    {
                        if (string.IsNullOrEmpty((exp[t] ?? "").Trim()))
                        {
                            value = value + "[NULL]" + '\n';
                        }
                        else
                        {
                            value = value + exp[t] + '\n';
                        }
                    }
                    RichTextBox1.Text = value;
                }
                else
                {
                    needsCompare = false;
                }

            }

            {
                var withBlock1 = DataGridView2;
                if (string.IsNullOrEmpty(varname))
                    return;
                variable = _workspace2.GetVariable(varname);
                if (variable is not null)
                {
                    if (CheckBox3.Checked)
                    {
                        exp = (SetProgramFilesNuetral(ref variable.Value) ?? "").Split(';');
                    }
                    else
                    {
                        exp = (variable.Value ?? "").Split(';');
                    }
                    value = "";
                    var loopTo2 = exp.Length - 1;
                    for (t = 0; t <= loopTo2; t++)
                        value = value + exp[t] + '\n';
                    RichTextBox3.Text = value;
                    if (CheckBox3.Checked)
                    {
                        exp = (SetProgramFilesNuetral(ref variable.FinalExpansion) ?? "").Split(';');
                    }
                    else
                    {
                        exp = (variable.FinalExpansion ?? "").Split(';');
                    }
                    value = "";
                    var loopTo3 = exp.Length - 1;
                    for (t = 0; t <= loopTo3; t++)
                    {
                        if (string.IsNullOrEmpty((exp[t] ?? "").Trim()))
                        {
                            value = value + "[NULL]" + '\n';
                        }
                        else
                        {
                            value = value + exp[t] + '\n';
                        }
                    }
                    RichTextBox4.Text = value;
                }
                else
                {
                    needsCompare = false;
                }

            }

            // Get description
            RichTextBox5.Text = (Utilities.GetVariableLongDescription(varname) ?? "").Trim();

            if (needsCompare)
                ColorText();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Apply color to the textboxes
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void ColorText()
        {
            int t;
            int max;
            var nfont = new Font(RichTextBox1.Font, FontStyle.Bold);
            string tb1;
            string tb2;

            {
                var withBlock = RichTextBox3;
                if (CultureInfo.CurrentCulture.CompareInfo.Compare(RichTextBox2.Text ?? "", withBlock.Text ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
                {
                    if (RichTextBox2.Text.Length < withBlock.Text.Length)
                    {
                        max = RichTextBox2.Text.Length - 1;
                    }
                    else
                    {
                        max = withBlock.Text.Length - 1;
                    }

                    tb1 = RichTextBox2.Text;
                    tb2 = RichTextBox3.Text;
                    var loopTo = max;
                    for (t = 0; t <= loopTo; t++)
                    {
                        if (tb2[t] != tb1[t])
                        {
                            withBlock.SelectionStart = t;
                            withBlock.SelectionLength = withBlock.Text.Length - t;
                            withBlock.SelectionColor = Color.DarkRed;
                            withBlock.SelectionFont = nfont;
                            break;
                        }
                    }

                }
            }

            {
                var withBlock1 = RichTextBox4;
                if (CultureInfo.CurrentCulture.CompareInfo.Compare(RichTextBox1.Text ?? "", withBlock1.Text ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
                {
                    if (RichTextBox1.Text.Length < withBlock1.Text.Length)
                    {
                        max = RichTextBox1.Text.Length - 1;
                    }
                    else
                    {
                        max = withBlock1.Text.Length - 1;
                    }

                    tb1 = RichTextBox1.Text;
                    tb2 = RichTextBox4.Text;
                    var loopTo1 = max;
                    for (t = 0; t <= loopTo1; t++)
                    {
                        if (tb2[t] != tb1[t])
                        {
                            withBlock1.SelectionStart = t;
                            withBlock1.SelectionLength = withBlock1.Text.Length - t;
                            withBlock1.SelectionColor = Color.DarkRed;
                            withBlock1.SelectionFont = nfont;
                            break;
                        }
                    }

                }
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Set path to neutral program files location
        // ---------------+---------------+---------------+---------------+---------------+-------
        private string SetProgramFilesNuetral(ref string filepath)
        {
            string val;

            val = (filepath ?? "").Replace("/", @"\");
            val = val.Replace(@"c:\program files\", "[PROGRAM_FILES]");
            val = val.Replace(@"c:\program files (x86)\", "[PROGRAM_FILES]");

            return val;
        }

        private void DataGridView1_DataBindingComplete(object sender, System.Windows.Forms.DataGridViewBindingCompleteEventArgs e)
        {
            var argdataGridView = DataGridView1;
            ApplyColor(ref argdataGridView);
            DataGridView1 = argdataGridView;
        }

        private void DataGridView2_DataBindingComplete(object sender, System.Windows.Forms.DataGridViewBindingCompleteEventArgs e)
        {
            var argdataGridView = DataGridView2;
            ApplyColor(ref argdataGridView);
            DataGridView2 = argdataGridView;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Force each datagrid to align when scrolling
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void DataGridView1_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            if (_ignoreUpdate)
                return;
            if (DataGridView1.FirstDisplayedScrollingRowIndex == DataGridView2.FirstDisplayedScrollingRowIndex)
                return;
            DataGridView2.FirstDisplayedScrollingRowIndex = DataGridView1.FirstDisplayedScrollingRowIndex;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Force each datagrid to align when scrolling
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void DataGridView2_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            if (_ignoreUpdate)
                return;
            if (DataGridView1.FirstDisplayedScrollingRowIndex == DataGridView2.FirstDisplayedScrollingRowIndex)
                return;
            DataGridView1.FirstDisplayedScrollingRowIndex = DataGridView2.FirstDisplayedScrollingRowIndex;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Build descriptions for a selected variable
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (_ignoreUpdate)
                return;
            if (DataGridView2.Rows.Count < 1)
                return;
            if (DataGridView1.SelectedRows.Count < 1)
                return;
            if (DataGridView2.SelectedRows.Count < 1)
                return;
            if (DataGridView2.SelectedRows[0].Index == DataGridView1.SelectedRows[0].Index)
                return;
            // console.writeline("DG View 1 selection changed")
            DataGridView2.ClearSelection();
            DataGridView2.Rows[DataGridView1.SelectedRows[0].Index].Selected = true;
            BuildValueBoxes();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Build descriptions for a selected variable
        // ---------------+---------------+---------------+---------------+---------------+--
        private void DataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (_ignoreUpdate)
                return;
            if (DataGridView1.Rows.Count < 1)
                return;
            if (DataGridView1.SelectedRows.Count < 1)
                return;
            if (DataGridView2.SelectedRows.Count < 1)
                return;
            if (DataGridView2.SelectedRows[0].Index == DataGridView1.SelectedRows[0].Index)
                return;
            // console.writeline("DG View 2 selection changed")
            DataGridView1.ClearSelection();
            DataGridView1.Rows[DataGridView2.SelectedRows[0].Index].Selected = true;
            BuildValueBoxes();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Save form settings when closing
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void VariableCompare_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            object argform = this;
            InterfaceControler.RemoveForm(ref argform);
            // Settings.VC_Location = Me.Location
            // Settings.VC_WindowState = Me.WindowState
            // If Me.WindowState = System.Windows.Forms.FormWindowState.Normal Then Settings.VC_Size = Me.Size
            // Settings.VC_Split1_Dist = Me.SplitContainer1.SplitterDistance
            // Settings.VC_Split2_Dist = Me.SplitContainer2.SplitterDistance
            // Settings.VC_Split3_Dist = Me.SplitContainer3.SplitterDistance
            // Settings.Save()
        }

        // ---------------------------------------------------------------------------------------
        // @description: For the left and right panels to be the same size
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void VariableCompare_Resize(object sender, EventArgs e)
        {
            if (_ignoreUpdate)
                return;
            // console.writeline("VariableCompare_Resize")
            DataGridView1.AutoResizeColumns();
            DataGridView2.AutoResizeColumns();
            if (WindowState == System.Windows.Forms.FormWindowState.Maximized)
            {
                SplitContainer1.SplitterDistance = (int)Math.Round(SplitContainer1.Width / 2d);
            }
        }

        private void VariableCompare_Shown(object sender, EventArgs e)
        {
            if (_ignoreUpdate)
                return;
            if (_workspace1 is null | _workspace2 is null)
                Close();
            DataGridView1.AutoResizeColumns();
            DataGridView2.AutoResizeColumns();
        }

        private void CheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (_ignoreUpdate)
                return;
            Build_Grid_Views();
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (_ignoreUpdate)
                return;
            Build_Grid_Views();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (_ignoreUpdate)
                return;
            Build_Grid_Views();
        }

        private void SplitContainer1_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
        {
            if (_ignoreUpdate)
                return;
            if (SplitContainer2.SplitterDistance != SplitContainer1.SplitterDistance)
            {
                SplitContainer2.SplitterDistance = SplitContainer1.SplitterDistance;
            }
        }

        private void SplitContainer2_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
        {
            if (_ignoreUpdate)
                return;
            if (SplitContainer2.SplitterDistance != SplitContainer1.SplitterDistance)
            {
                SplitContainer1.SplitterDistance = SplitContainer2.SplitterDistance;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            TextBox1.Text = "";
        }

        private void CheckBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (_ignoreUpdate)
                return;
            Build_Grid_Views();
        }

        private void DataGridView1_CellContentDoubleClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            CFGVariable variable;
            string varname;

            if (_workspace1 is null)
                return;

            {
                var withBlock = DataGridView1;
                if (withBlock.SelectedRows.Count == 0)
                    return;
                varname = Convert.ToString(withBlock.SelectedRows[0].Cells[0].Value);
                if (string.IsNullOrEmpty(varname))
                    return;
                variable = _workspace1.GetVariable(varname);
                if (variable is not null)
                {
                    var vdos = new VariableDossier(CFGEnums.ConfigurationType.Compare1, ref varname);
                    if (InterfaceControler.GroupWindowsInMainForm)
                        vdos.MdiParent = MdiParent;
                    vdos.Show();
                }

            }
        }

        private void DataGridView2_CellContentDoubleClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            CFGVariable variable;
            string varname;

            if (_workspace2 is null)
                return;

            {
                var withBlock = DataGridView2;
                if (withBlock.SelectedRows.Count == 0)
                    return;
                varname = Convert.ToString(withBlock.SelectedRows[0].Cells[0].Value);
                if (string.IsNullOrEmpty(varname))
                    return;
                variable = _workspace2.GetVariable(varname);
                if (variable is not null)
                {
                    var vdos = new VariableDossier(CFGEnums.ConfigurationType.Compare1, ref varname);
                    if (InterfaceControler.GroupWindowsInMainForm)
                        vdos.MdiParent = MdiParent;
                    vdos.Show();
                }
            }
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (_ignoreUpdate)
                return;
            Build_Grid_Views();
        }

        private void WatchF5_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.F5)
            {
                InterfaceControler.RefreshOpenForms();
            }
        }

        private void ToExcelFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path;

            if (_workspace1 is null)
                return;

            {
                var withBlock = SaveFileDialog1;
                withBlock.Title = CEResource.TXT_MsgSaveVariableListToExcel;
                withBlock.InitialDirectory = CEResource.TXT_CDrive;
                withBlock.Filter = CEResource.TXT_ExtSaveWorkSpaceFilter;
                withBlock.FileName = _workspace1.ApplicationData.Name + "-" + _workspace1.ApplicationData.Workspace + "-" + _workspace1.ApplicationData.Workset + ".xls";
                withBlock.RestoreDirectory = true;
                if (withBlock.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    return;

                path = withBlock.FileName;
                withBlock.Dispose();
            }

            if (string.IsNullOrEmpty(path))
                return;

            string argSheetName = CEResource.TXT_VariableCompare;
            ExcelExporter.DataGridExport(DataGridView1, DataGridView2, ref argSheetName, path);
            System.Windows.Forms.MessageBox.Show(CEResource.TXT_MsgFileExportComplete, CEResource.TXT_MsgDone);
        }

        private void VariableCompare_Load(object sender, EventArgs e)
        {
            // Me.Location = Settings.VC_Location
            // Me.WindowState = Settings.VC_WindowState
            // If Me.WindowState = System.Windows.Forms.FormWindowState.Normal Then Me.Size = Settings.VC_Size
            // Me.SplitContainer1.SplitterDistance = Settings.VC_Split1_Dist
            // Me.SplitContainer2.SplitterDistance = Settings.VC_Split2_Dist
            // Me.SplitContainer3.SplitterDistance = Settings.VC_Split3_Dist
        }

        private void OpenVariableExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var VE = new VariableExplorer(CFGEnums.ConfigurationType.Compare1);
            if (InterfaceControler.GroupWindowsInMainForm)
                VE.MdiParent = MdiParent;
            VE.Show();
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var VE = new VariableExplorer(CFGEnums.ConfigurationType.Compare2);
            if (InterfaceControler.GroupWindowsInMainForm)
                VE.MdiParent = MdiParent;
            VE.Show();
        }

        private void OpenFileHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var FH = new FileHistory(CFGEnums.ConfigurationType.Compare1);
            if (InterfaceControler.GroupWindowsInMainForm)
                FH.MdiParent = MdiParent;
            FH.Show();
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var FH = new FileHistory(CFGEnums.ConfigurationType.Compare2);
            if (InterfaceControler.GroupWindowsInMainForm)
                FH.MdiParent = MdiParent;
            FH.Show();
        }

        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            var EH = new EventHistory(CFGEnums.ConfigurationType.Compare2);
            if (InterfaceControler.GroupWindowsInMainForm)
                EH.MdiParent = MdiParent;
            EH.Show();
        }

        private void OpenWorkspaceHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var EH = new EventHistory(CFGEnums.ConfigurationType.Compare1);
            if (InterfaceControler.GroupWindowsInMainForm)
                EH.MdiParent = MdiParent;
            EH.Show();
        }

        private void DataGridView1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            string key;
            key = char.ToLower(e.KeyChar).ToString();
            int t;
            var loopTo = DataGridView1.Rows.Count - 1;
            for (t = 0; t <= loopTo; t++)
            {
                if (DataGridView1.Rows[t].Cells[0].Value?.ToString() == key?.ToString())
                {
                    DataGridView1.CurrentCell = DataGridView1.Rows[t].Cells[0];
                    DataGridView2.CurrentCell = DataGridView2.Rows[t].Cells[0];
                    return;
                }
            }
        }

        private void DataGridView2_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            string key;
            key = char.ToLower(e.KeyChar).ToString();
            int t;
            var loopTo = DataGridView2.Rows.Count - 1;
            for (t = 0; t <= loopTo; t++)
            {
                if (DataGridView1.Rows[t].Cells[0].Value?.ToString() == key?.ToString())
                {
                    DataGridView1.CurrentCell = DataGridView1.Rows[t].Cells[0];
                    DataGridView2.CurrentCell = DataGridView2.Rows[t].Cells[0];
                    return;
                }
            }
        }

        private void ToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            ToExcelFileToolStripMenuItem_Click(sender, e);
        }

        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (_ignoreUpdate)
                return;
            Build_Grid_Views();
        }

    }
}
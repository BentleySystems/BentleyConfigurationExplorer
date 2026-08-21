// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using WorkspaceCFG.My;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{
    public partial class VariableLibrary
    {

        private CFGEnums.ConfigurationType _workspaceType;
        private CFGConfiguration _workspace;

        ~VariableLibrary()
        {
            _workspaceType = default;
            _workspace = null;
        }

        public VariableLibrary(CFGEnums.ConfigurationType inwrkType)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            _workspaceType = inwrkType;

            // Fill Form with data
            RefreshForm();
        }

        public void RefreshForm()
        {
            InterfaceControler.GetConfiguration(ref _workspace, _workspaceType);

            if (_workspace is null)
            {
                CheckBox1.Enabled = false;
                CheckBox1.Checked = false;
            }
            else
            {
                Text = CEResource.TXT_TitleVariableLibrary + _workspace.Description2;
            }

            Fill_Datagrid();
        }

        private void Fill_Datagrid()
        {
            var dataTable = new DataTable();
            int count = 0;

            VarDB.EnsureLoaded();
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCVariable));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCCategory));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCApplication));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCStatus));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCLongDescription));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCColor));

            foreach (var kvp in VarDB.VariableDB)
            {
                var entry = kvp;
                string name = entry.Name?.ToUpper() ?? string.Empty;
                string desc = entry.LongDesc ?? string.Empty;

                bool use = true;

                if (!string.IsNullOrEmpty(TextBox1.Text))
                {
                    use = name.Contains(TextBox1.Text);
                }

                if (!string.IsNullOrEmpty(TextBox2.Text))
                {
                    use &= desc.Contains(TextBox2.Text);
                }

                bool inwrk = false;

                if (_workspace != null)
                {
                    var @var = _workspace.GetVariable(name);
                    if (@var == null)
                    {
                        use &= true;
                    }
                    else
                    {
                        if (CheckBox1.Checked)
                            use = false;

                        inwrk = true;
                    }
                }

                if (use)
                {
                    var dataRow = dataTable.NewRow();
                    dataRow[0] = name;
                    dataRow[1] = entry.Category;
                    dataRow[2] = entry.App;
                    dataRow[3] = entry.Status;
                    dataRow[4] = entry.LongDesc;
                    dataRow[5] = inwrk ? "BLUE" : "";

                    dataTable.Rows.Add(dataRow);
                    count++;
                }
            }

            if (!CheckBox1.Checked)
                Get_Missing_Variables(ref dataTable, ref count);

            Label6.Text = count == 1
                ? CEResource.TXT_SingleVariable
                : count + CEResource.TXT_MultipleVariables;

            DataGridView1.DataSource = dataTable;
            DataGridView1.Columns[5].Visible = false;
            DataGridView1.Sort(DataGridView1.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
            DataGridView1.Refresh();
        }

        private void Get_Missing_Variables(ref DataTable dt, ref int count)
        {
            if (_workspace is null)
                return;
            DataRow dataRow;
            bool use;

            try
            {
                foreach (CFGVariable @var in _workspace.Variables.Values)
                {
                    use = true;
                    if (!string.IsNullOrEmpty(TextBox1.Text))
                    {
                        use = @var.Name != null && TextBox1.Text != null &&
                              @var.Name.IndexOf(TextBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0;
                    }

                    if (use)
                    {
                        if (!VarDB.VariableDB.Any(entry => entry.Name == @var.Name))
                        {
                            dataRow = dt.NewRow();
                            dataRow[0] = @var.Name;
                            dataRow[3] = "MISSING";
                            dataRow[4] = "Variable not found in library";
                            dataRow[5] = "RED";
                            count += 1;
                            dt.Rows.Add(dataRow);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            RefreshForm();
        }

        private void VariableLibrary_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            object argform = this;
            InterfaceControler.RemoveForm(ref argform);
            MySettings.Default.VL_Location = Location;
            MySettings.Default.VL_WindowState = WindowState;
            if (WindowState == System.Windows.Forms.FormWindowState.Normal)
                MySettings.Default.VL_Size = Size;
            MySettings.Default.Save();
        }

        private void VariableLibrary_Shown(object sender, EventArgs e)
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

        private void Button1_Click(object sender, EventArgs e)
        {
            TextBox1.Text = "";
            RefreshForm();
        }

        private void VariableLibrary_Load(object sender, EventArgs e)
        {
            Location = MySettings.Default.VL_Location;
            WindowState = MySettings.Default.VL_WindowState;
            if (WindowState == System.Windows.Forms.FormWindowState.Normal)
                Size = MySettings.Default.VL_Size;
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            Fill_Datagrid();
        }

        private void DataGridView1_DataBindingComplete(object sender, System.Windows.Forms.DataGridViewBindingCompleteEventArgs e)
        {
            int t;

            if (DataGridView1.Rows.Count == 0)
                return;

            {
                var withBlock = DataGridView1;
                var loopTo = withBlock.Rows.Count - 1;
                for (t = 0; t <= loopTo; t++)
                {
                    var cellValue = withBlock.Rows[t].Cells[5].Value?.ToString();
                    if (string.Equals(cellValue, "BLUE", StringComparison.OrdinalIgnoreCase))
                        withBlock.Rows[t].DefaultCellStyle.ForeColor = Color.Blue;
                    if (string.Equals(cellValue, "RED", StringComparison.OrdinalIgnoreCase))
                        withBlock.Rows[t].DefaultCellStyle.ForeColor = Color.Red;
                }
            }
        }

        private void ExportToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path;

            {
                var withBlock = SaveFileDialog1;
                withBlock.Title = CEResource.TXT_MsgSaveVariableListToExcel;
                withBlock.InitialDirectory = CEResource.TXT_CDrive;
                withBlock.Filter = CEResource.TXT_ExtSaveWorkSpaceFilter;
                if (_workspace is not null)
                {
                    withBlock.FileName = _workspace.ApplicationData.Name + "-" + _workspace.ApplicationData.Workspace + "-" + _workspace.ApplicationData.Workset + CEResource.TXT_ExtXls;
                }
                else
                {
                    withBlock.FileName = CEResource.TXT_LibraryXls;
                }

                withBlock.RestoreDirectory = true;
                if (withBlock.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    return;

                path = withBlock.FileName;
                withBlock.Dispose();
            }

            string argSheetName = CEResource.TXT_EventHistory;
            ExcelExporter.DataGridExport(DataGridView1, ref argSheetName, path);
            System.Windows.Forms.MessageBox.Show(CEResource.TXT_MsgFileExportComplete, CEResource.TXT_MsgDone);
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            string name;

            {
                var withBlock = DataGridView1;

                if (withBlock.SelectedCells.Count == 0)
                    return;
                name = withBlock.SelectedCells[0].OwningRow.Cells[0].Value?.ToString();

                RichTextBox1.Text = Utilities.GetVariableComment(name);
                RichTextBox3.Text = Utilities.GetVariableLongDescription(name);

            }
        }

        private void DataGridView1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            string key = char.ToLower(e.KeyChar).ToString();

            int t;
            var loopTo = DataGridView1.Rows.Count - 1;
            for (t = 0; t <= loopTo; t++)
            {
                //TODO: Check this.
                //if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(DataGridView1.Rows[t].Cells[0].Value(0).ToLower, key, true)))
                var cellValue = DataGridView1.Rows[t].Cells[0].Value?.ToString();

                if (string.Equals(cellValue, key, StringComparison.OrdinalIgnoreCase))
                {
                    DataGridView1.CurrentCell = DataGridView1.Rows[t].Cells[0];
                    return;
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            TextBox2.Text = "";
            RefreshForm();
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            Fill_Datagrid();
        }

        private void DataGridView1_CellContentClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
        }
    }
}
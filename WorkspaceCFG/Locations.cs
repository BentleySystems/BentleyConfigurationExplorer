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
using System.Runtime.InteropServices;
using WorkspaceCFG.My;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{

    public partial class Locations
    {

        private CFGEnums.ConfigurationType _workspaceType;
        private CFGVariable _variable;
        private string _variableName;
        private string _variableValue;

        ~Locations()
        {
            _workspaceType = default;
            _variable = null;
            _variableName = null;
        }

        public Locations(CFGEnums.ConfigurationType inwrktype, ref string varname, [Optional, DefaultParameterValue("")] ref string value)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            _workspaceType = inwrktype;
            _variableName = varname;
            _variableValue = value;

            RefreshForm();
        }

        public void RefreshForm()
        {
            CFGConfiguration wrkspc = null;

            InterfaceControler.GetConfiguration(ref wrkspc, _workspaceType);
            if (wrkspc is null)
                return;

            _variable = wrkspc.GetVariable(_variableName);

            if (_variable is null)
                return;

            Text = CEResource.TXT_LabelLocations + _variableName + "  " + _variableValue;

            BuildGrid();
        }

        private void BuildGrid()
        {
            string[] fval;
            string[] exp;
            int t;
            int y;
            var dataTable = new DataTable();
            // Dim cfgExpansion As CFGExpansion

            if (_variable is null)
                return;
            if (_variable.ParentConfiguration is null)
                return;

            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCValue));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCLocation));

            if (string.IsNullOrEmpty(_variableValue))
            {
                fval = (_variable.Value ?? "").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                fval = (_variableValue ?? "").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }

            int loopTo = fval.Length - 1;
            for (y = 0; y <= loopTo; y++)
            {

                if ((fval[y]?.Trim().Length ?? 0) > 1)
                {
                    string expansionValue = MacroParser.ParseLine(_variable.ParentConfiguration, 6, fval[y], true);

                    exp = (expansionValue ?? "").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    int loopTo1 = exp.Length - 1;
                    for (t = 0; t <= loopTo1; t++)
                        Get_Locations(ref dataTable, ref exp[t], ref fval[y]);
                }
            }

            t = 0;
            if (!string.IsNullOrEmpty(TextBox1.Text))
            {
                while (t < dataTable.Rows.Count)
                {
                    if (!dataTable.Rows[t][1].ToString().ToUpper().Contains(TextBox1.Text.ToUpper()))
                    {
                        dataTable.Rows.Remove(dataTable.Rows[t]);
                    }
                    else
                    {
                        t += 1;
                    }
                }
            }

            if (dataTable.Rows.Count == 1)
            {
                Label6.Text = CEResource.TXT_SingleLocation;
            }
            else
            {
                Label6.Text = string.Format(CEResource.TXT_MultipleLocations, dataTable.Rows.Count);
            }

            dgLocations.DataSource = dataTable;
            dgLocations.Refresh();
            dgLocations.Columns[0].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dgLocations.Columns[1].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        }

        private void Get_Locations(ref DataTable dt, ref string location, ref string value)
        {
            string path;
            string filename;
            DirectoryInfo di;
            FileInfo[] fils;
            DataRow dataRow;

            path = UtilitiesPath.GetDirectoryName(ref location);
            filename = UtilitiesPath.GetFileName(location);

            // If the location is not a path then exit sub
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(
                    (location ?? "").ToLowerInvariant(),
                    (filename ?? "").ToLowerInvariant(),
                    CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                return;
            if (string.IsNullOrEmpty(path))
                return;
            if (UtilitiesPath.FolderExists(path) == false)
                return;

            // Expand folders into files
            if (string.IsNullOrEmpty(filename))
            {
                dataRow = dt.NewRow();
                dataRow[0] = value;
                dataRow[1] = path;
                dt.Rows.Add(dataRow);
                if (CheckBox1.Checked)
                {
                    filename = "*.*";
                }
                else
                {
                    return;
                }
            }

            // Include statements can have wildcards
            try
            {
                if ((filename ?? "").Contains("*"))
                {

                    di = new DirectoryInfo(path);
                    fils = di.GetFiles(filename);
                    foreach (var fiNext in fils)
                    {
                        dataRow = dt.NewRow();
                        dataRow[0] = value;
                        dataRow[1] = fiNext.FullName;
                        dt.Rows.Add(dataRow);
                    }
                }
                else
                {
                    dataRow = dt.NewRow();
                    dataRow[0] = value;
                    dataRow[1] = path + filename;
                    dt.Rows.Add(dataRow);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
        }

        private void Locations_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            object argform = this;
            InterfaceControler.RemoveForm(ref argform);
            MySettings.Default.Loc_Location = Location;
            MySettings.Default.Loc_WindowState = WindowState;
            if (WindowState == System.Windows.Forms.FormWindowState.Normal)
                MySettings.Default.Loc_Size = Size;
            MySettings.Default.Save();
        }

        private void Locations_Load(object sender, EventArgs e)
        {
            Location = new Point(MySettings.Default.Loc_Location.X + 20, MySettings.Default.Loc_Location.Y + 28);
            MySettings.Default.VR_Location = Location;
            MySettings.Default.Save();
            WindowState = MySettings.Default.VR_WindowState;
            if (WindowState == System.Windows.Forms.FormWindowState.Normal)
                Size = MySettings.Default.Loc_Size;
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            BuildGrid();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            TextBox1.Text = "";
        }

        private void OpenContainingFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                var withBlock = dgLocations;
                if (withBlock.SelectedRows.Count == 0)
                    return;
                string localGetDirectoryName() { string argpath = withBlock.SelectedRows[0].Cells[1].Value?.ToString(); var ret = UtilitiesPath.GetDirectoryName(ref argpath); withBlock.SelectedRows[0].Cells[1].Value = argpath; return ret; }

                string argfilename = localGetDirectoryName();
                Utilities.OpenContainingFolder(ref argfilename);
            }
        }

        private void CopyPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string output = "";

            {
                var withBlock = dgLocations;
                foreach (System.Windows.Forms.DataGridViewRow rw in withBlock.SelectedRows)
                    output = output + rw.Cells[1].Value.ToString() + '\n';
            }

            System.Windows.Forms.Clipboard.SetText(output);
        }

        private void CopyFileNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string output = "";

            {
                var withBlock = dgLocations;
                foreach (System.Windows.Forms.DataGridViewRow rw in withBlock.SelectedRows)
                {
                    string localGetFileName() { string argpath1 = rw.Cells[1].Value.ToString(); var ret = UtilitiesPath.GetFileName(argpath1); return ret; }

                    output = output + localGetFileName() + '\n';
                }
            }

            System.Windows.Forms.Clipboard.SetText(output);
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            RefreshForm();
        }

        private void dgLocations_CellContentClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
        }

    }
}
// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{

    public partial class VariableValidater
    {

        private CFGConfiguration _workspace;
        private CFGEnums.ConfigurationType _workspaceType;

        public VariableValidater(CFGEnums.ConfigurationType workspaceType)
        {
            // This call is required by the designer.
            InitializeComponent();

            // Gets a workspace instance
            object argform = this;
            InterfaceControler.AddForm(ref argform);
            _workspaceType = workspaceType;

            RefreshForm();
        }

        public void RefreshForm()
        {
            InterfaceControler.GetConfiguration(ref _workspace, _workspaceType);
            if (_workspace is null)
                return;
            Text = CEResource.TXT_LabelVariableValidater + _workspace.Description2;
            BuildRuleList();
        }

        private void VariableValidator_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            object argform = this;
            InterfaceControler.RemoveForm(ref argform);
        }

        private void WatchF5_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.F5)
            {
                InterfaceControler.RefreshOpenForms();
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuildList();
        }

        private void BuildRuleList()
        {
            string path;
            DirectoryInfo directoryInfo;
            FileInfo[] fileInfoList;

            path = Utilities.GetExeFolder() + @"Rules\";

            try
            {
                directoryInfo = new DirectoryInfo(path);
                fileInfoList = directoryInfo.GetFiles("*.txt");
                foreach (FileInfo fileInfo in fileInfoList)
                    ComboBox1.Items.Add(fileInfo.Name.Replace(".txt", ""));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
        }

        private void BuildList()
        {
            if (_workspace is null)
                return;
            if (string.IsNullOrEmpty(ComboBox1.Text))
                return;

            var dataTable = new DataTable();
            DataRow dataRow;

            var variableValidater = new CFGVariableValidater(ref _workspace);
            string argvalidationrule = ComboBox1.Text;
            variableValidater.ProcessValidationRule(argvalidationrule);
            ComboBox1.Text = argvalidationrule;

            // Build filtered datatable here
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCVariable));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCStatus));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCRule));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCValue));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCRuleFile));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCLineNumber));

            dataTable.BeginLoadData();

            foreach (CFGEnums.ValidationResult result in variableValidater.ResultCol)
            {
                if (CheckBox1.Checked == false || result.alert)
                {
                    dataRow = dataTable.NewRow();
                    dataRow[0] = result.variablename;

                    if (result.alert)
                    {
                        dataRow[1] = "FAILED";
                    }
                    else
                    {
                        dataRow[1] = "PASSED";
                    }

                    dataRow[2] = (result.opt + " " + result.attr).Replace("_", " ") + " " + result.inval;
                    dataRow[3] = result.varval;
                    dataRow[4] = result.rulename;
                    dataRow[5] = result.linenumber;

                    dataTable.Rows.Add(dataRow);
                }
            }

            dataTable.EndLoadData();

            DataGridView1.DataSource = dataTable;
            DataGridView1.Sort(DataGridView1.Columns[0], System.ComponentModel.ListSortDirection.Descending);
            DataGridView1.Refresh();

            Label6.Text = DataGridView1.Rows.Count.ToString() + CEResource.TXT_MultipleVariables;
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            BuildList();
        }

        private void ToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path;

            if (_workspace is null)
                return;

            var SaveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            SaveFileDialog1.Title = CEResource.TXT_MsgSaveVariableListToExcel;
            SaveFileDialog1.InitialDirectory = CEResource.TXT_CDrive;
            SaveFileDialog1.Filter = CEResource.TXT_ExtSaveWorkSpaceFilter;
            SaveFileDialog1.FileName = _workspace.ApplicationData.Name + "-" + ComboBox1.Text + CEResource.TXT_ExtXls;
            SaveFileDialog1.RestoreDirectory = true;
            if (SaveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            path = SaveFileDialog1.FileName;
            SaveFileDialog1.Dispose();

            string argSheetName = CEResource.TXT_VariableValidater;
            ExcelExporter.DataGridExport(DataGridView1, ref argSheetName, path);
            System.Windows.Forms.MessageBox.Show(CEResource.TXT_MsgFileExportComplete, CEResource.TXT_MsgDone);
        }

    }
}
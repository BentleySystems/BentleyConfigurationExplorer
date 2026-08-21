// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using System.Windows.Forms;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{

    public partial class AppWizard
    {

        //private Collection _tempAppList;
        private Dictionary<string, CFGEnums.Application> _tempAppList = new Dictionary<string, CFGEnums.Application>();
        private bool _enableUpdate;
        private bool _isDirty;

        // These are ENUMS for the rows in data tables
        private const int APP_NAME = 0;
        private const int APP_VERSION = 1;
        private const int APP_EXE_PATH = 2;
        private const int APP_STARTUPCFG = 3;
        private const int APP_CONFIGURATIONROOT = 4;
        private const int APP_CMD_ARGS = 5;
        private const int APP_VARSDBNAMES = 6;
        private const int APP_PREDEFCFGNAMES = 7;
        private const int APP_RULES = 8;
        private const int APP_LASTWORKSPACESELECTED = 9;
        private const int APP_LASTWORKSETSELECTED = 10;
        private const int APP_LASTROLESELECTED = 11;

        public AppWizard()
        {
            _enableUpdate = false;
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            CopyAppList();
            RefreshForm();
        }

        private void AppWizard_Shown(object sender, EventArgs e)
        {
            cmdApply.Enabled = false;

            // Add this form instance from the interface controller
            object argform = this;
            InterfaceControler.AddForm(ref argform);

            _enableUpdate = true;
        }

        private void AppWizard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isDirty)
            {
                var response = MessageBox.Show("Save changes?", "Data Check", MessageBoxButtons.YesNoCancel);

                switch (response)
                {
                    case DialogResult.Yes:
                        {
                            SaveAppList();
                            break;
                        }
                    case DialogResult.No:
                        {
                            break;
                        }
                    // 
                    case DialogResult.Cancel:
                        {
                            e.Cancel = true;
                            return;
                        }
                }
            }

            // Remove this form instance from the interface controller
            object argform = this;
            InterfaceControler.RemoveForm(ref argform);
        }

        public void AddAppToTempAppList(CFGEnums.Application application)
        {


            string key = $"{application.Name}|{application.EXEPath}"; // Use a delimiter to avoid collisions
            if (!string.IsNullOrEmpty(application.Name) && !string.IsNullOrEmpty(application.EXEPath))
            {
                _tempAppList[key] = application;
            }

        }

        private bool DidAppListChange()
        {
            if (_tempAppList.Count != MainType.CESettings.Applications.Count)
                return true;

            foreach (var application in MainType.CESettings.Applications)
            {
                string key = $"{application.Name}|{application.EXEPath}";

                if (_tempAppList.TryGetValue(key, out var existingApp))
                {
                    if (!application.Equals(existingApp))
                        return true;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        public void RefreshForm()
        {
            BuildAppList();
        }


        private void CopyAppList()
        {
            _tempAppList = new Dictionary<string, CFGEnums.Application>();

            foreach (CFGEnums.Application application in MainType.CESettings.Applications)
            {
                string key = $"{application.Name}|{application.EXEPath}";

                // Overwrite if key already exists
                _tempAppList[key] = application;
            }
        }


        private void BuildAppList()
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCName));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_InstalledProductVersion));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCExePath));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCStartUpPath));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCConfigurationRoot));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCCommandlineArguments));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCVarsDBNames));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCPredefinedCfgName));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCRules));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCLastWorkspaceSeleted));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCLastWorksetSeleted));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCLastRoleSeleted));

            dataTable.BeginLoadData();

            foreach (var application in _tempAppList.Values)
            {
                var dataRow = dataTable.NewRow();
                dataRow[APP_NAME] = application.Name;
                dataRow[APP_VERSION] = application.Version;
                dataRow[APP_EXE_PATH] = application.EXEPath;
                dataRow[APP_STARTUPCFG] = application.StartupCfgPath;
                dataRow[APP_CONFIGURATIONROOT] = application.ConfigurationRoot;
                dataRow[APP_CMD_ARGS] = application.CommandOptions;
                dataRow[APP_PREDEFCFGNAMES] = application.PredefinedCfgNames;
                dataRow[APP_RULES] = application.Rules;
                dataRow[APP_LASTWORKSPACESELECTED] = application.LastSelectedWorkspace;
                dataRow[APP_LASTWORKSETSELECTED] = application.LastSelectedWorkset;
                dataRow[APP_LASTROLESELECTED] = application.LastSelectedRole;

                dataTable.Rows.Add(dataRow);
            }

            dataTable.EndLoadData();

            dgApplicationList.DataSource = dataTable;
            dgApplicationList.Sort(dgApplicationList.Columns[APP_NAME], System.ComponentModel.ListSortDirection.Ascending);
            dgApplicationList.Columns[APP_LASTWORKSPACESELECTED].Visible = false;
            dgApplicationList.Columns[APP_LASTWORKSETSELECTED].Visible = false;
            dgApplicationList.Columns[APP_LASTROLESELECTED].Visible = false;
            dgApplicationList.Refresh();
        }

        private void SaveAppList()
        {
            MainType.CESettings.Applications = new List<CFGEnums.Application>();

            foreach (var application in _tempAppList.Values)
            {
                MainType.CESettings.Applications.Add(application);
            }

            System.Threading.Thread.Sleep(200); // Optional: consider replacing with async delay if needed

            MainType.CESettings.Save();

            Application.DoEvents(); // Use cautiously; consider alternatives in modern async code
        }

        private void DgApplicationList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!_enableUpdate)
                return;

            _tempAppList = new Dictionary<string, CFGEnums.Application>();

            foreach (DataGridViewRow row in dgApplicationList.Rows)
            {
                if (row.IsNewRow) continue;

                var app = new CFGEnums.Application();

                app.Name = row.Cells[APP_NAME].Value?.ToString();
                app.Version = row.Cells[APP_VERSION].Value?.ToString();
                app.EXEPath = row.Cells[APP_EXE_PATH].Value?.ToString();
                app.StartupCfgPath = row.Cells[APP_STARTUPCFG].Value?.ToString();
                app.CommandOptions = row.Cells[APP_CMD_ARGS].Value?.ToString();
                app.ConfigurationRoot = row.Cells[APP_CONFIGURATIONROOT].Value?.ToString();
                app.PredefinedCfgNames = row.Cells[APP_PREDEFCFGNAMES].Value?.ToString();
                app.VariableDBNames = row.Cells[APP_VARSDBNAMES].Value?.ToString();
                app.Rules = row.Cells[APP_RULES].Value?.ToString();
                app.LastSelectedWorkspace = row.Cells[APP_LASTWORKSPACESELECTED].Value?.ToString();
                app.LastSelectedWorkset = row.Cells[APP_LASTWORKSETSELECTED].Value?.ToString();
                app.LastSelectedRole = row.Cells[APP_LASTROLESELECTED].Value?.ToString();

                if (!string.IsNullOrEmpty(app.Name) && !string.IsNullOrEmpty(app.EXEPath))
                {
                    string key = $"{app.Name}|{app.EXEPath}";
                    _tempAppList[key] = app;
                }
            }

            _isDirty = true;
            cmdSave.Enabled = true;
            cmdApply.Enabled = true;
        }

        private void DgApplicationList_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (_enableUpdate == true)
            {
                cmdSave.Enabled = true;
                cmdApply.Enabled = true;
                _isDirty = true;
            }
        }

        private void DgApplicationList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (_enableUpdate == true)
            {
                cmdSave.Enabled = true;
                cmdApply.Enabled = true;
                _isDirty = true;
            }
        }

        private void DgApplicationList_DragDrop(object sender, DragEventArgs e)
        {
            _enableUpdate = false;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, true);

                foreach (string file in files)
                {
                    CFGEnums.Application app = Utilities.GetApplicationFromLNK(file);
                    string key = $"{app.Name}|{app.EXEPath}";

                    if (!string.IsNullOrEmpty(app.Name) && !_tempAppList.ContainsKey(key))
                    {
                        _tempAppList[key] = app;
                        cmdSave.Enabled = true;
                        cmdApply.Enabled = true;
                    }
                }

                RefreshForm();
            }

            _enableUpdate = true;
        }

        private void DgApplicationList_DragOver(object sender, DragEventArgs e)
        {
            _enableUpdate = false;

            if (e.Data.GetDataPresent("FileDrop"))
            {
                string[] theFiles = (string[])e.Data.GetData("FileDrop", true);
                foreach (string theFile in theFiles)
                {
                    if (theFile != null && theFile.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase))
                    {
                        e.Effect = DragDropEffects.Copy;
                        return;
                    }
                }
            }

            e.Effect = DragDropEffects.None;
            _enableUpdate = true;
        }

        private void DgApplicationList_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (!_enableUpdate)
                return;

            string appName = e.Row.Cells[APP_NAME].Value?.ToString();
            string exePath = e.Row.Cells[APP_EXE_PATH].Value?.ToString();

            // Validate app name and exe path
            if (string.IsNullOrEmpty(appName) || string.IsNullOrEmpty(exePath))
                return;

            // Create key from app name and exe path
            string key = $"{appName}|{exePath}";

            // Check for existence in dictionary
            if (!_tempAppList.ContainsKey(key))
                return;

            // Remove from dictionary and enable save/apply
            _tempAppList.Remove(key);
            cmdSave.Enabled = true;
            cmdApply.Enabled = true;
        }

        private async void CmdApply_Click(object sender, EventArgs e)
        {
            MainType.CESettings.Applications = new List<CFGEnums.Application>();

            foreach (var app in _tempAppList.Values)
            {
                MainType.CESettings.Applications.Add(app);
            }

            await Task.Delay(200); // Non-blocking delay

            cmdApply.Enabled = false;
            _isDirty = false;
        }


        // Add app
        private void CmdCancel_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            Close();
        }

        private void CmdSave_Click(object sender, EventArgs e)
        {
            SaveAppList();
            Close();
        }

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            var dialogInstalledApp = new InstalledApplication();
            var argappWizard = this;
            dialogInstalledApp.LoadInstalledApps(ref argappWizard);
            dialogInstalledApp.ShowDialog();
        }

        // Remove app
        private void ToolStripButton2_Click(object sender, EventArgs e)
        {
            // If no rows are selected, exit
            if (dgApplicationList.SelectedRows == null || dgApplicationList.SelectedRows.Count == 0)
                return;

            foreach (DataGridViewRow row in dgApplicationList.SelectedRows)
            {
                // Skip blank or new rows
                if (row.IsNewRow ||
                    row.Cells[APP_NAME].Value is DBNull ||
                    row.Cells[APP_EXE_PATH].Value is DBNull)
                    continue;

                string appName = row.Cells[APP_NAME].Value?.ToString();
                string exePath = row.Cells[APP_EXE_PATH].Value?.ToString();

                if (string.IsNullOrEmpty(appName) || string.IsNullOrEmpty(exePath))
                    continue;

                string key = $"{appName}|{exePath}";

                // Remove from grid
                dgApplicationList.Rows.Remove(row);

                // Remove from dictionary
                if (_tempAppList.ContainsKey(key))
                {
                    _tempAppList.Remove(key);
                    cmdSave.Enabled = true;
                    cmdApply.Enabled = true;
                }
            }

            dgApplicationList.Sort(dgApplicationList.Columns[APP_NAME], System.ComponentModel.ListSortDirection.Ascending);
            dgApplicationList.Refresh();
        }


        }
    }
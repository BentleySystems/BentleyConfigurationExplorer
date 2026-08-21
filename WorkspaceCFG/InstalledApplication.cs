// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices; // Add this at the top with other using statements
using System.Windows.Forms;
using Microsoft.Win32;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{

    public partial class InstalledApplication
    {

        private AppWizard _appWizard;
        private string[,] _appNameFileVersionMap = new string[3, 501];
        private int _appNameFileVersionCount;
        private int _ignoreAppsCount = 0;
        private string[] _ignoreAppsList = new string[501];

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

        public InstalledApplication()
        {
            // This call is required by the designer.
            InitializeComponent();
        }

        public void SearchShortcutsInFolder(ref DataTable dt, DirectoryInfo oParentDirectoryInfo, ref int indexno, ref string[] UniqueArrayOfInstalledApplicationName, ref string[] UniqueArrayOfInstalledApplicationExe)
        {

            foreach (var lnkFile in oParentDirectoryInfo.GetFiles("*.lnk"))
            {
                try
                {
                    // Create WScript.Shell COM object
                    Type shellType = Type.GetTypeFromProgID("WScript.Shell");
                    dynamic WshShell = Activator.CreateInstance(shellType);
                    var theShortcut = WshShell.CreateShortcut(Path.Combine(lnkFile.DirectoryName, lnkFile.Name));

                    if (!string.IsNullOrWhiteSpace(Convert.ToString(theShortcut.TargetPath)))
                    {
                        string vertPath = string.Empty;
                        // First check if the shortcut contain path to configuration
                        string arguments = theShortcut.Arguments?.ToString();
                        string[] argTokens = (arguments ?? "").Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string token in argTokens)
                        {
                            if (token.Trim().ToLowerInvariant().StartsWith("wc"))
                            {
                                // Remove the first two characters ("wc") and all quotes
                                vertPath = token.Length > 2 ? token.Substring(2).Replace("\"", "") : "";
                                // Remove -wcFilePath part from arguments list
                                arguments = arguments.Replace("-" + token, "");
                                break;
                            }
                        }
                        // If no configuration file is in the input argument, set a default one
                        if (string.IsNullOrEmpty(vertPath))
                        {
                            string targetPath = theShortcut.TargetPath?.ToString();
                            string targetDirectory = string.IsNullOrEmpty(targetPath)
                                ? string.Empty
                                : Path.GetDirectoryName(targetPath) ?? string.Empty;
                            vertPath = Path.Combine(targetDirectory, "config", "mslocal.cfg");
                        }

                        vertPath = vertPath.Trim();
                        if (File.Exists(vertPath))
                        {

                            string lnkName = lnkFile.Name;
                            int lastDot = lnkName.LastIndexOf('.');
                            string NewAppName = lastDot > 0 ? lnkName.Substring(0, lastDot) : lnkName;
                            NewAppName = NewAppName.Trim();
                            theShortcut.TargetPath = theShortcut.TargetPath.Trim();

                            bool Result = false;

                            int p;

                            var loopTo = indexno - 1;
                            for (p = 0; p <= loopTo; p++)
                            {
                                bool namesMatch = string.Compare(
                                    UniqueArrayOfInstalledApplicationName[p] ?? "",
                                    NewAppName ?? "",
                                    CultureInfo.CurrentCulture,
                                    CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth
                                    ) == 0;

                                bool pathsMatch = string.Equals(
                                    Convert.ToString(UniqueArrayOfInstalledApplicationExe[p]),
                                    Convert.ToString(theShortcut.TargetPath),
                                    StringComparison.OrdinalIgnoreCase
                                );

                                if (namesMatch && pathsMatch)
                                {
                                    // Your logic here
                                }
                                {
                                    Result = true;
                                    p = indexno;
                                }
                            }

                            int i;

                            string exePath = theShortcut.TargetPath?.ToString();
                            exePath = exePath.Replace("/", @"\");
                            exePath = UtilitiesPath.GetFileName(exePath);
                            // skip if not an exe file
                            if (Result == false && !exePath.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                            {
                                Result = true;
                            }

                            // skip if in ignore list
                            if (Result == false)
                            {
                                var loopTo1 = _ignoreAppsCount;
                                for (i = 0; i <= loopTo1; i++)
                                {
                                    // MessageBox.Show("In checking : " & ignoreAppsList(i))
                                    if (exePath.Equals(_ignoreAppsList[i], StringComparison.OrdinalIgnoreCase))
                                    {
                                        Result = true;
                                        break;
                                    }
                                }
                            }

                            if (Result == false)
                            {
                                if (!Result)
                                {
                                    var grid = _appWizard.dgApplicationList;

                                    if (grid != null && grid.Rows.Count > 0)
                                    {
                                        foreach (DataGridViewRow row in grid.Rows)
                                        {
                                            var nameCell = row.Cells[APP_NAME].Value;
                                            var exePathCell = row.Cells[APP_EXE_PATH].Value;

                                            if (nameCell == DBNull.Value || exePathCell == DBNull.Value)
                                            {
                                                Result = false;
                                            }
                                            else
                                            {
                                                string name = Convert.ToString(nameCell);
                                                exePath = Convert.ToString(exePathCell);

                                                if (string.Equals(name, NewAppName, StringComparison.OrdinalIgnoreCase) &&
                                                    string.Equals(exePath, theShortcut.TargetPath, StringComparison.OrdinalIgnoreCase))
                                                {
                                                    Result = true;
                                                    break; // Optional: exit early if match is found
                                                }
                                            }
                                        }
                                    }
                                }

                                if (Result == false)
                                {

                                    UniqueArrayOfInstalledApplicationName[indexno] = NewAppName;
                                    UniqueArrayOfInstalledApplicationExe[indexno] = theShortcut.TargetPath?.ToString();

                                    indexno += 1;

                                    DataRow wrow;
                                    wrow = dt.NewRow();

                                    wrow[APP_NAME] = UniqueArrayOfInstalledApplicationName[indexno - 1];
                                    wrow[APP_EXE_PATH] = UniqueArrayOfInstalledApplicationExe[indexno - 1];
                                    wrow[APP_VERSION] = string.Empty;
                                    wrow[APP_STARTUPCFG] = vertPath;
                                    wrow[APP_CMD_ARGS] = arguments;
                                    dt.Rows.Add(wrow);

                                }
                            }
                        }
                    }
                }
                // End If
                catch (Exception ex)
                {
                    Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                }

            }
            foreach (var oFolder in oParentDirectoryInfo.GetDirectories())
                SearchShortcutsInFolder(ref dt, oFolder, ref indexno, ref UniqueArrayOfInstalledApplicationName, ref UniqueArrayOfInstalledApplicationExe);
        }

        public void LoadInstalledApps(ref AppWizard appWizard)
        {
            _appWizard = appWizard;

            var dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn(CEResource.TXT_InstalledProductName));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_InstalledProductVersion));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_EXEPath));
            dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCStartUpPath));

            FillInstalledProductsList(ref dataTable, ref _appNameFileVersionMap, ref _appNameFileVersionCount);

            {
                var withBlock = DataGridView1;
                withBlock.DataSource = dataTable;
                // .Columns(APP_EXE_PATH).Visible = False
                withBlock.Columns[APP_STARTUPCFG].Visible = false;
                // .Columns(APP_CMD_ARGS).Visible = False

                withBlock.Sort(withBlock.Columns[APP_NAME], System.ComponentModel.ListSortDirection.Ascending);
                withBlock.ReadOnly = true;
                withBlock.Refresh();
            }
        }

        public void FillInstalledProductsList(ref DataTable dataTable, ref string[,] AppNameFileVersionMap, ref int AppNameFileVersionCount)
        {
            var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            var powerProductsKey = baseKey.OpenSubKey(@"SOFTWARE\\Bentley\\Installed_Products");

            if (powerProductsKey is null)
                return;

            foreach (string keyName in powerProductsKey.GetSubKeyNames())
            {
                object productKey = powerProductsKey.OpenSubKey(keyName);
                if (productKey is null)
                {
                    continue;
                }

                var version = ((dynamic)productKey).GetValue("Version");
                if (version is null)
                {
                    continue;
                }

                var configurationLocation = ((dynamic)productKey).GetValue("ConfigurationPath");
                if (configurationLocation is null)
                {
                    continue;
                }

                object indexno = 0;
                object productVersion = new Version(version.ToString());
                if (((dynamic)productVersion).Major >= 10 && !string.IsNullOrEmpty(configurationLocation))
                {
                    dynamic key = productKey;

                    string applicationDisplayName = key.GetValue("DisplayProductName")?.ToString();
                    string applicationPath = key.GetValue("ApplicationPath")?.ToString();
                    string programPath = key.GetValue("ProgramPath")?.ToString() ?? string.Empty;
                    string configPath = Path.Combine(programPath, @"config\mslocal.cfg");

                    int index = Convert.ToInt32(indexno);

                    AppNameFileVersionMap[0, index] = applicationDisplayName;
                    AppNameFileVersionMap[1, index] = applicationPath;
                    AppNameFileVersionMap[2, index] = version?.ToString();

                    DataRow dataRow = dataTable.NewRow();
                    dataRow[APP_NAME] = applicationDisplayName;
                    dataRow[APP_VERSION] = version;
                    dataRow[APP_EXE_PATH] = applicationPath;
                    dataRow[APP_STARTUPCFG] = configPath;

                    dataTable.Rows.Add(dataRow);
                }


            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            // If the dataGridView selected rows are null
            if (DataGridView1.SelectedRows is null || DataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            DataTable dt;

            {
                var withBlock = _appWizard.dgApplicationList;
                dt = (DataTable)withBlock.DataSource;
            }

            DataRow wrow;

            foreach (DataGridViewRow dataGridViewRow in DataGridView1.SelectedRows)
            {
                wrow = dt.NewRow();

                if (string.IsNullOrEmpty(Convert.ToString(dataGridViewRow.Cells[APP_NAME].Value)))
                    break;

                wrow[APP_NAME] = dataGridViewRow.Cells[APP_NAME].Value;
                wrow[APP_VERSION] = dataGridViewRow.Cells[APP_VERSION].Value;
                wrow[APP_EXE_PATH] = dataGridViewRow.Cells[APP_EXE_PATH].Value;
                wrow[APP_STARTUPCFG] = dataGridViewRow.Cells[APP_STARTUPCFG].Value;

                CFGEnums.Application vert;
                vert = new CFGEnums.Application()
                {
                    Name = wrow[APP_NAME]?.ToString(),
                    EXEPath = wrow[APP_EXE_PATH]?.ToString(),
                    StartupCfgPath = wrow[APP_STARTUPCFG]?.ToString()
                };

                vert.Version = wrow[APP_VERSION]?.ToString();

                _appWizard.AddAppToTempAppList(vert);

                dt.Rows.Add(wrow);

                DataGridView1.AllowUserToDeleteRows = true;
                DataGridView1.Rows.Remove(DataGridView1.Rows[dataGridViewRow.Index]);
                DataGridView1.Refresh();
                DataGridView1.AllowUserToDeleteRows = false;

            }

            {
                var withBlock1 = _appWizard.dgApplicationList;
                withBlock1.DataSource = dt;
                withBlock1.Sort(withBlock1.Columns[APP_NAME], System.ComponentModel.ListSortDirection.Ascending);
                withBlock1.Refresh();
            }

            Application.DoEvents();
            Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            Close();
        }

        private void ToolStripButtonCopyToClipboard_Click(object sender, EventArgs e)
        {
            if (DataGridView1.SelectedRows is null || DataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            string ClipBoardText = "";
            var ClipBoardData = new string[201];
            int cnt = 1;

            foreach (DataGridViewRow dataGridViewRow in DataGridView1.SelectedRows)
            {
                if (string.IsNullOrEmpty(Convert.ToString(dataGridViewRow.Cells[0].Value)))
                    break;

                ClipBoardData[cnt] = dataGridViewRow.Cells[APP_NAME].Value.ToString() + " " +
                                     dataGridViewRow.Cells[APP_VERSION].Value.ToString() + " " +
                                     dataGridViewRow.Cells[APP_EXE_PATH].Value.ToString() + " " +
                                     dataGridViewRow.Cells[APP_STARTUPCFG].Value.ToString() + " " +
                                     dataGridViewRow.Cells[APP_CMD_ARGS].Value.ToString() +
                                     dataGridViewRow.Cells[APP_VERSION].Value.ToString() + Environment.NewLine;
                cnt += 1;
            }

            for (int p = cnt; p >= 1; p -= 1)
                ClipBoardText += ClipBoardData[p];

            Clipboard.SetText(ClipBoardText);
        }
    }
}
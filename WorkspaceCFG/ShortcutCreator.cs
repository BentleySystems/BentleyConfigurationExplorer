// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
// Replacing IWshRuntimeLibrary WScript.Shell object
// Imports IWshRuntimeLibrary
using System.Windows.Forms;
using Microsoft.Win32;
using WorkspaceCFG.My;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{

    public partial class ShortcutCreator
    {

        private ConfigurationSelector _configurationSelector;

        private const string WC_PARAMETER = "-wc";

        public ShortcutCreator()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            LocalizeControls();
            BuildUstationList();

            _configurationSelector = new ConfigurationSelector();
            var argappComboBox = cmb_APP1;
            ComboBox argworkspaceComboBox = null;
            ComboBox argworksetComboBox = null;
            ComboBox argroleComboBox = null;
            _configurationSelector.InitializeCombos(ref argappComboBox, ref argworkspaceComboBox, ref argworksetComboBox, ref argroleComboBox);
            cmb_APP1 = argappComboBox;
            cmb_APP1.Items.Add(CEResource.TXT_LabelSelectApplication);
            cmb_APP1.Text = CEResource.TXT_LabelSelectApplication;
        }

        private void BuildUstationList()
        {
            var rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Bentley\MicroStation");
            RegistryKey sk;
            string[] skname;
            string path;

            try
            {
                skname = rk.GetSubKeyNames();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                return;
            }

            for (int counter = 0, loopTo = skname.Length - 1; counter <= loopTo; counter++)
            {
                sk = rk.OpenSubKey(skname[counter]);
                path = sk.GetValue(CEResource.TXT_LabelPathName)?.ToString();

                if (!string.IsNullOrEmpty(path))
                {
                    tbTargetApp.Text = path;
                    break;
                }
            }
        }

        private void CreateCommandPreview()
        {
            if (!string.IsNullOrEmpty(tbTargetApp.Text))
            {
                tbCommandPreview.Text = "\"" + tbTargetApp.Text + "\"";
            }

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(WC_PARAMETER, tbConfigFile.Text ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
            {
                tbCommandPreview.Text = tbCommandPreview.Text + " \"" + tbConfigFile.Text + "\"";
            }

            if (!string.IsNullOrEmpty(tbAddCmdArgs.Text))
            {
                tbCommandPreview.Text = tbCommandPreview.Text + " \"" + tbAddCmdArgs.Text + "\"";
            }
        }

        private bool CreateShortcut(string shortcutName, ref string args, string creationDir, string targetFullpath, string workingDir, string iconFile, int iconNumber)
        {
            try
            {
                // Use Activator.CreateInstance for WScript.Shell COM object
                Type shellType = Type.GetTypeFromProgID("WScript.Shell");
                dynamic WshShell = Activator.CreateInstance(shellType);
                var shortCut = WshShell.CreateShortcut(Path.Combine(creationDir, shortcutName + ".lnk"));
                shortCut.TargetPath = targetFullpath;
                shortCut.WindowStyle = 1;
                shortCut.Description = shortcutName;
                shortCut.WorkingDirectory = workingDir;
                shortCut.IconLocation = iconFile + ", " + iconNumber;
                shortCut.Arguments = args;
                shortCut.Save();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                return false;
            }
        }

        private string FilenameFromPath(ref string path)
        {
            int pos = -1;
            int t;

            var loopTo = path.Length - 1;
            for (t = 0; t <= loopTo; t++)
            {
                if (path[t] == '\\')
                    pos = t;
            }

            if (pos == -1)
                return path;
            // Equivalent to VB's Mid(path, pos + 2)
            return path.Substring(pos + 1);
        }

        private string NoFilenameFromPath(ref string path)
        {
            // Equivalent to VB's Mid(path, 1, path.Length - FilenameFromPath(ref path).Length)
            return path.Substring(0, path.Length - FilenameFromPath(ref path).Length);
        }

        private void LocalizeControls()
        {
            Text = CEResource.TXT_ShortcutCreatorTitle;
            Label3.Text = CEResource.TXT_ApplicationShortcutDescription;
            Label8.Text = CEResource.TXT_ShortcutName;
            Label10.Text = CEResource.TXT_TargetApplicationLocation;
            Label4.Text = CEResource.TXT_WCConfigurationFile;
            Label11.Text = CEResource.TXT_AdditionalCommandlineArguments;
            Label2.Text = CEResource.TXT_ShortCutCommandPreview;
            Label9.Text = CEResource.TXT_ShortcutOutputFolder;
            tbOutPutFolder.Text = CEResource.TXT_CDrive;

            btnCreate.Text = CEResource.TXT_Create;
            btnCancel.Text = CEResource.TXT_Cancel;
        }

        private void MakeShortcut(ref string name, ref string config, ref string args, ref CFGEnums.Application vert)
        {
            string arguments;
            string exe_path;

            // Get the exe path from text box
            exe_path = tbTargetApp.Text;

            arguments = config + " " + tbAddCmdArgs.Text + " " + args;
            CreateShortcut(name, ref arguments, tbOutPutFolder.Text, exe_path, NoFilenameFromPath(ref exe_path), vert.Iconfile, vert.IconNumber);
        }

        private void UpdateEnabled()
        {
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(cmb_APP1.Text ?? "", CEResource.TXT_LabelSelectApplication ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                tbConfigFile.Enabled = false;
                btnBrowseCFG.Enabled = false;
                tbShortcutName.Text = "";
                tbTargetApp.Text = "";
                tbConfigFile.Text = WC_PARAMETER;
                tbAddCmdArgs.Text = "";
                tbCommandPreview.Text = "";
            }
            else
            {
                btnBrowseCFG.Enabled = true;
                tbConfigFile.Enabled = true;
            }
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            CFGEnums.Application app;
            string config;
            string args = "";

            if (string.IsNullOrEmpty(tbOutPutFolder.Text.Trim()))
            {
                return;
            }
            if (string.IsNullOrEmpty(tbTargetApp.Text.Trim()))
            {
                return;
            }

            config = WC_PARAMETER + "\"" + tbConfigFile.Text.Substring(WC_PARAMETER.Length) + "\"";
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(cmb_APP1.Text ?? "", CEResource.TXT_LabelSelectApplication ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                foreach (var currentApp in MainType.CESettings.Applications)
                {
                    app = currentApp;
                    args = "";
                    MakeShortcut(ref app.Name, ref config, ref args, ref app);
                }
            }
            else
            {
                string exePath = string.Empty;
                CFGEnums.ShortApplication sv = (CFGEnums.ShortApplication)cmb_APP1.SelectedItem;
                if (sv is not null)
                    exePath = sv.ExePath;
                app = MainType.CESettings.GetApplication(cmb_APP1.Text.Trim(), exePath);
                if (string.IsNullOrEmpty(app.Name))
                    return;
                string argname = tbShortcutName.Text;
                MakeShortcut(ref argname, ref config, ref args, ref app);
                tbShortcutName.Text = argname;
            }

            MessageBox.Show(CEResource.TXT_MsgExportComplete, CEResource.TXT_MsgIconCreator, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnBrowseCFG_Click(object sender, EventArgs e)
        {
            {
                var withBlock = OpenFileDialog1;
                withBlock.Title = CEResource.TXT_MsgFindConfigurationFile;
                withBlock.InitialDirectory = tbConfigFile.Text.Substring(WC_PARAMETER.Length);
                withBlock.Filter = CEResource.TXT_ExtExportWorkSpaceFilterCfg;
                withBlock.FileName = CEResource.TXT_ExtCfg;
                withBlock.RestoreDirectory = true;
                if (withBlock.ShowDialog() == DialogResult.Cancel)
                    return;

                tbConfigFile.Text = WC_PARAMETER + withBlock.FileName;
                withBlock.Dispose();
            }
        }

        private void BtnBrowseTargetApp_Click(object sender, EventArgs e)
        {
            {
                var withBlock = OpenFileDialog1;
                withBlock.Title = CEResource.TXT_MsgFindUstationExe;
                withBlock.InitialDirectory = tbTargetApp.Text;
                withBlock.Filter = CEResource.TXT_ExtFilterExe;
                withBlock.FileName = CEResource.TXT_ExtExe;
                withBlock.RestoreDirectory = true;
                if (withBlock.ShowDialog() == DialogResult.Cancel)
                    return;

                tbTargetApp.Text = withBlock.FileName;
                withBlock.Dispose();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnBrowseOutPut_Click(object sender, EventArgs e)
        {
            {
                var withBlock = FolderBrowserDialog1;
                withBlock.RootFolder = Environment.SpecialFolder.Desktop;
                withBlock.SelectedPath = tbOutPutFolder.Text;
                withBlock.Description = CEResource.TXT_MsgSelectTheShortcutOutputDirectory;
                if (withBlock.ShowDialog() == DialogResult.OK)
                    tbOutPutFolder.Text = withBlock.SelectedPath;
            }
        }

        private void Cmb_APP1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(cmb_APP1.Text ?? "", CEResource.TXT_LabelSelectApplication ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                UpdateEnabled();
            }
            else
            {
                CFGEnums.Application vert;
                string exePath = string.Empty;
                CFGEnums.ShortApplication sv = (CFGEnums.ShortApplication)cmb_APP1.SelectedItem;
                if (sv is not null)
                    exePath = sv.ExePath;
                vert = MainType.CESettings.GetApplication(cmb_APP1.Text.Trim(), exePath);
                tbShortcutName.Text = vert.Name;
                tbTargetApp.Text = vert.EXEPath;
                if (!string.IsNullOrEmpty(vert.CommandOptions))
                {
                    // Split CommandOptions based on spaces
                    string[] words = vert.CommandOptions.Split(new char[] { ' ' });
                    foreach (var word in words)
                    {
                        if (word.StartsWith(WC_PARAMETER))
                        {
                            tbConfigFile.Text = word;
                        }
                        else
                        {
                            tbAddCmdArgs.Text = word;
                        }
                    }
                }
                else
                {
                    tbConfigFile.Text = WC_PARAMETER;
                    tbAddCmdArgs.Text = "";
                }
                tbConfigFile.Enabled = true;
                btnBrowseCFG.Enabled = true;
                CreateCommandPreview();
            }
        }

        private void IconCreator_FormClosing(object sender, FormClosingEventArgs e)
        {
            MySettings.Default.Shortcut_Out = tbOutPutFolder.Text;
            MySettings.Default.Shortcut_Args = tbAddCmdArgs.Text;
            MySettings.Default.Save();
        }

        private void IconCreator_Shown(object sender, EventArgs e)
        {
            tbOutPutFolder.Text = MySettings.Default.Shortcut_Out;
            tbAddCmdArgs.Text = MySettings.Default.Shortcut_Args;
        }

        private void TbConfigFile_TextChanged(object sender, EventArgs e)
        {
            string configFile;
            configFile = tbConfigFile.Text;
            // User editing resulted in removal of any/all -wc character string,
            // handle to add it back
            if (!configFile.StartsWith(WC_PARAMETER))
            {
                if (configFile.Length < WC_PARAMETER.Length)
                {
                    tbConfigFile.Text = WC_PARAMETER;
                }
                else
                {
                    int index;
                    int subStrIndex = 0;
                    var loopTo = WC_PARAMETER.Length - 1;
                    for (index = 0; index <= loopTo; index++)
                    {
                        if (WC_PARAMETER[index] == configFile[subStrIndex])
                        {
                            subStrIndex += 1;
                        }
                    }
                    // prefix -wc with existing string
                    tbConfigFile.Text = WC_PARAMETER + configFile.Substring(subStrIndex);
                }
                // set cursor location to end of -wc
                tbConfigFile.SelectionStart = WC_PARAMETER.Length;
            }
            CreateCommandPreview();
        }

        private void TbTargetApp_TextChanged(object sender, EventArgs e)
        {
            CreateCommandPreview();
        }

        private void TbAddCmdArgs_TextChanged(object sender, EventArgs e)
        {
            CreateCommandPreview();
        }
    }
}
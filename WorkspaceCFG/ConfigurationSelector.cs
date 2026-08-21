// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WorkspaceCFG.My.Resources;
using System.Collections.Generic;

namespace WorkspaceCFG
{
    public class ConfigurationSelector
    {
        // ---------------------------------------------------------------------------------------
        // Private Members
        // ---------------+---------------+---------------+---------------+---------------+-------
        private string _USTN_WORKSPACESROOT;
        private string _lastApplication;

        // ---------------------------------------------------------------------------------------
        // Public Members
        // ---------------+---------------+---------------+---------------+---------------+-------
        public bool Initalized;
        public CFGEnums.Application InputApplication;

        // ---------------------------------------------------------------------------------------
        // @description: Overloaded new sub, initializes InputApplication
        // ---------------+---------------+---------------+---------------+---------------+-------
        public ConfigurationSelector()
        {
            // Required
        }

        public ConfigurationSelector(CFGEnums.Application application)
        {
            InputApplication = application;
            MainType.CESettings.LastAppLoaded = InputApplication.Name;
            _lastApplication = MainType.CESettings.LastAppLoaded;
        }

        // Helper for VB string/constant compatibility
        private static class VBCompat
        {
            public static string Replace(string input, string oldValue, string newValue)
                => input?.Replace(oldValue, newValue) ?? string.Empty;
            public static string vbNullString => string.Empty;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Clears out the combo boxes and fills vert_cmb with a list of applications
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void InitializeCombos(ref ComboBox appComboBox, ref ComboBox workspaceComboBox, ref ComboBox worksetComboBox, ref ComboBox roleComboBox)
        {
            // Clear out user combobox
            if (workspaceComboBox is not null)
            {
                workspaceComboBox.Items.Clear();
                workspaceComboBox.Text = !string.IsNullOrEmpty(InputApplication.Name) ? InputApplication.Workspace : string.Empty;
            }

            // Clear out project combobox
            if (worksetComboBox is not null)
            {
                worksetComboBox.Items.Clear();
                worksetComboBox.Text = !string.IsNullOrEmpty(InputApplication.Name) ? InputApplication.Workset : string.Empty;
            }

            if (roleComboBox is not null)
            {
                roleComboBox.Items.Clear();
                roleComboBox.Text = !string.IsNullOrEmpty(InputApplication.Name) ? InputApplication.Role : string.Empty;
            }


            // Fill vert_cmb with a list of applications
            FillApplicationsCombo(ref appComboBox);
        }

        // ---------------------------------------------------------------------------------------
        // @description: Fill vert_cmb with a list of applications found in CESettings.Applicationss
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void FillApplicationsCombo(ref ComboBox ApplicationComboBox)
        {
            // Clear out existing values
            ApplicationComboBox.Items.Clear();

            // If InputVertical.name already exists in CESettings.Verticals then replace it with this instance
            // This allows the user to drag and drop a shortcut that may already exist and have the new values override the existing
            if (!string.IsNullOrEmpty(InputApplication.Name))
            {
                if (MainType.CESettings.Applications.Contains(InputApplication))
                {
                    MainType.CESettings.Applications.Remove(InputApplication);
                }
                MainType.CESettings.Applications.Add(InputApplication);
            }

            foreach (CFGEnums.Application Application in MainType.CESettings.Applications)
                ApplicationComboBox.Items.Add(new CFGEnums.ShortApplication(CalculateNameForVertName(ApplicationComboBox, Application.Name), Application.EXEPath));

            ApplicationComboBox.DisplayMember = "Name";
            ApplicationComboBox.ValueMember = "ExePath";

            // Set the selected vertical in the combobox

            if (!string.IsNullOrEmpty(InputApplication.Name))
                ApplicationComboBox.Text = InputApplication.Name;
        }

        private string CalculateNameForVertName(ComboBox ApplicationComboBox, string name)
        {
            // If a combo box has multiple entries with same text, with AutoComplete functionality On,
            // it always select the first one found in the combo, even if user click on the some other entry of that value
            // Add space at the end to mark a different entry
            // This space needs to be later removed while processing for vertical
            string vertName = name;
            foreach (CFGEnums.ShortApplication sv in ApplicationComboBox.Items)
            {
                if (CultureInfo.CurrentCulture.CompareInfo.Compare(sv.Name ?? "", name ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                {
                    vertName += " ";
                }
            }
            return vertName;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Fill workspace_cmb with a list of workspace .cfg files found in path
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void FillWorkspaces(ref ComboBox workspaceComboBox, string path)
        {
            DirectoryInfo di;

            // Clear out existing values
            workspaceComboBox.Items.Clear();
            workspaceComboBox.Items.Add(CEResource.TXT_NoWorkspace);

            // Warn the user that the user configs couldn't be found
            // If Trim(path) = vbNullString Then
            // System.Windows.Forms.MessageBox.Show(CEResource.TXT_ErrFindingWorkspacesConfigFolder, CEResource.TXT_TitleError)
            // Exit Sub
            // End If

            try
            {
                if (!UtilitiesPath.FolderExists(path))
                {
                    MessageBox.Show(string.Format(CEResource.TXT_ErrWorkspacesFolderPointsToInvalidLocation, path), CEResource.TXT_TitleError);
                    return;
                }

                di = new DirectoryInfo(path);
                foreach (FileInfo fiNext in di.GetFiles("*.cfg"))
                    workspaceComboBox.Items.Add(VBCompat.Replace(fiNext.Name, ".cfg", VBCompat.vbNullString));

                if (workspaceComboBox.Items.Count == 0)
                {
                    MessageBox.Show(string.Format(CEResource.TXT_ErrNoUcfFilesFound, path), CEResource.TXT_TitleError);
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }

            // Set the selected workspace in the combobox
            if (!string.IsNullOrEmpty(InputApplication.Name))
            {
                workspaceComboBox.Text = InputApplication.Workspace;
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Fill workset_cmb with a list of .dgnws filenames found in path
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void FillWorksets(ref ComboBox worksetComboBox, string path, bool worksetLocked, ref bool ready)
        {
            DirectoryInfo di;

            ready = true;

            var worksets = new ArrayList();
            // Clear out existing values
            worksetComboBox.Items.Clear();

            try
            {
                if (UtilitiesPath.FolderExists(path) == false)
                {
                    MessageBox.Show(string.Format(CEResource.TXT_ErrWorkSetsFolderPointsToInvalidLocation, path), CEResource.TXT_TitleError);
                    return;
                }

                di = new DirectoryInfo(path);

                foreach (FileInfo fiNext in di.GetFiles("*.cfg"))
                    worksets.Add(VBCompat.Replace(fiNext.Name, ".cfg", VBCompat.vbNullString));

                worksets.Sort();

                worksetComboBox.Items.Add(CEResource.TXT_NoWorkset);
                foreach (string wset in worksets)
                    worksetComboBox.Items.Add(wset);

                if (worksetComboBox.Items.Count == 0)
                {
                    MessageBox.Show(string.Format(CEResource.TXT_ErrNoPcfFilesFound, path), CEResource.TXT_TitleError);
                    ready = false;

                    worksetComboBox.Items.Clear();
                    worksetComboBox.Text = "";
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }

            worksetComboBox.Text = "";

            // Set the selected workset in the combobox
            if (!string.IsNullOrEmpty(InputApplication.Name))
                worksetComboBox.Text = InputApplication.Workset;

            if (worksetComboBox.Items.Count > 1 && !worksetLocked)
            {
                if (string.IsNullOrEmpty(worksetComboBox.Text))
                {
                    worksetComboBox.Text = worksetComboBox.Items[1].ToString();
                }
            }
            else if (worksetComboBox.Items.Count > 0)        // select no project, if there is no other valid project
            {
                worksetComboBox.Text = worksetComboBox.Items[0].ToString();
            }

            Utilities.ValidateSelectedWorkset(ref worksetComboBox, worksetLocked);
        }

        // ---------------------------------------------------------------------------------------
        // @description: Fill workset_cmb with a list of .dgnws filenames found in path
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void FillRoles(ref ComboBox rolesComboBox, string path, bool roleLocked, ref bool ready)
        {
            DirectoryInfo di;

            ready = true;

            var roles = new ArrayList();
            // Clear out existing values
            rolesComboBox.Items.Clear();

            try
            {
                if (UtilitiesPath.FolderExists(path) == false)
                {
                    MessageBox.Show(string.Format(CEResource.TXT_ErrRolesFolderPointsToInvalidLocation, path), CEResource.TXT_TitleError);
                    return;
                }

                di = new DirectoryInfo(path);

                foreach (FileInfo fiNext in di.GetFiles("*.cfg"))
                    roles.Add(VBCompat.Replace(fiNext.Name, ".cfg", VBCompat.vbNullString));

                roles.Sort();

                rolesComboBox.Items.Add(CEResource.TXT_NoRole);
                foreach (string role in roles)
                    rolesComboBox.Items.Add(role);

                if (rolesComboBox.Items.Count == 0)
                {
                    MessageBox.Show(string.Format(CEResource.TXT_ErrNoPcfFilesFound, path), CEResource.TXT_TitleError);
                    ready = false;

                    rolesComboBox.Items.Clear();
                    rolesComboBox.Text = "";
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }

            rolesComboBox.Text = "";

            // Set the selected project in the combobox
            if (!string.IsNullOrEmpty(InputApplication.Name))
                rolesComboBox.Text = InputApplication.Workset;

            if (rolesComboBox.Items.Count > 1 && !roleLocked)
            {
                if (string.IsNullOrEmpty(rolesComboBox.Text))
                {
                    rolesComboBox.Text = rolesComboBox.Items[1].ToString();
                }
            }
            else if (rolesComboBox.Items.Count > 0)        // select no project, if there is no other valid project
            {
                rolesComboBox.Text = rolesComboBox.Items[0].ToString();
            }

            Utilities.ValidateSelectedRole(ref rolesComboBox, roleLocked);
        }

        // ---------------------------------------------------------------------------------------
        // @description: Fills the workspace combobox when an application has been chosen
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void ApplicationSelected(ref string applicationName, ref string exePath, ref ComboBox workspacesComboBox, ref ComboBox roleComboBox)
        {
            Application.DoEvents();

            CFGConfiguration configuration;
            CFGEnums.Application app;
            CFGVariable variable;

            roleComboBox.Enabled = false;
            roleComboBox.Text = "";

            _USTN_WORKSPACESROOT = VBCompat.vbNullString;

            // Find vert from comboselection
            app = MainType.CESettings.GetApplication(applicationName.Trim(), exePath);
            // TODO: review

            if (string.IsNullOrEmpty(app.Name))
                return;
            // Main.CESettings.ValidateVertSettingsFiles(vert.Name)


            string argworkspace_str = VBCompat.vbNullString;
            string argworkset_str = VBCompat.vbNullString;
            string argrole_str = VBCompat.vbNullString;
            configuration = Utilities.GetConfiguration(ref app.Name, ref app.EXEPath, ref argworkspace_str, ref argworkset_str, ref argrole_str);

            MainType.CESettings.LastAppLoaded = "";

            // If _lastVertical Is Nothing OrElse _lastVertical <> applicationName Then
            if (MainType.CESettings.LastAppLoaded is null || string.IsNullOrEmpty(MainType.CESettings.LastAppLoaded))
            {
                // If the state actually changed we can safely exit the sub since this sub is called again.
                if (string.Equals(_lastApplication, applicationName, StringComparison.CurrentCultureIgnoreCase))
                {
                    MainType.CESettings.LastAppLoaded = applicationName;
                    return;
                }

                MainType.CESettings.LastAppLoaded = applicationName;
            }

            // Process the whole workspace
            // configuration.ProcessConfiguration()
            configuration.ProcessWatchForLevel(3, app.StartupCfgPath);


            variable = configuration.GetVariable("_USTN_WORKSPACESROOT");
            if (variable is null)
                return;

            // Process the workspace until the variable is found
            _USTN_WORKSPACESROOT = variable.Expand();

            FillWorkspaces(ref workspacesComboBox, _USTN_WORKSPACESROOT);
        }

        // ---------------------------------------------------------------------------------------
        // @description: Fills the workspace combobox when an application has been chosen
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void ApplicationSelected2(ref ComboBox appComboBox, ref ComboBox workspaceComboBox, ComboBox roleComboBox)
        {

            string exePath = string.Empty;
            CFGEnums.ShortApplication sv = (CFGEnums.ShortApplication)appComboBox.SelectedItem;

            if (sv is not null)
                exePath = sv.ExePath;
            string argapplicationName = appComboBox.Text.Trim();
            ApplicationSelected(ref argapplicationName, ref exePath, ref workspaceComboBox, ref roleComboBox);
        }

        // ---------------------------------------------------------------------------------------
        // @description: Fills the workset combobox when a workspace has been chosen
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void WorkspaceSelected(ref ComboBox applicationComboBox, ref ComboBox workspaceComboBox, ref ComboBox worksetComboBox, ref ComboBox roleComboBox, [Optional, DefaultParameterValue(false)] ref bool ready)
        {

            Application.DoEvents();

            string path;
            CFGConfiguration configuration;
            CFGEnums.Application app;
            CFGVariable variable;
            roleComboBox.Enabled = false;
            roleComboBox.Text = "";

            // If _USTN_WORKSPACESROOT = vbNullString Then Exit Sub
            if (string.IsNullOrEmpty(workspaceComboBox.Text))
                return;

            // Find vert from combo selection
            string exePath = string.Empty;
            CFGEnums.ShortApplication sv = (CFGEnums.ShortApplication)applicationComboBox.SelectedItem;
            if (sv is not null)
                exePath = sv.ExePath;
            app = MainType.CESettings.GetApplication(applicationComboBox.Text.Trim(), exePath);
            if (string.IsNullOrEmpty(app.Name))
                return;

            string argworkspace_str = VBCompat.vbNullString;
            string argworkset_str = VBCompat.vbNullString;
            string argrole_str = VBCompat.vbNullString;
            configuration = Utilities.GetConfiguration(ref app.Name, ref app.EXEPath, ref argworkspace_str, ref argworkset_str, ref argrole_str);

            // Fix for Problem 1: Use ContainsKey
            if (!configuration.Variables.ContainsKey("_USTN_WORKSPACENAME"))
            {
                variable = new CFGVariable(configuration)
                {
                    Name = "_USTN_WORKSPACENAME",
                    Level = -1,
                    NeedsSpecialExpansion = false,
                    IsLocked = false
                };
                variable.SetValue(workspaceComboBox.Text);
                variable.SetFinalExpansion();
                // Fix for Problem 2 & 3: Add with key and value
                configuration.Variables.Add(variable.Name, variable);
            }
            configuration.ApplicationData.Workspace = workspaceComboBox.Text;
            path = _USTN_WORKSPACESROOT + workspaceComboBox.Text + ".cfg";

            // Process the workspace until worksets root is found
            // configuration.ProcessWatchForFile(path, app.StartupCfgPath)
            // configuration.ProcessWatchForFile("Workset", app.StartupCfgPath)
            configuration.ProcessWatchForLevel(4, app.StartupCfgPath);


            variable = configuration.GetVariable("_USTN_WORKSETSROOT");

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(workspaceComboBox.Text ?? "", CEResource.TXT_NoWorkspace ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
            {
                FillWorksets(ref worksetComboBox, variable.Expand(), variable.IsLocked, ref ready);
            }
            else
            {
                worksetComboBox.Items.Clear();
                worksetComboBox.Items.Add(CEResource.TXT_NoWorkset);
                worksetComboBox.Text = worksetComboBox.Items[0].ToString();
            }

            // Process the workspace until roles root is found
            variable = configuration.GetVariable("_USTN_ROLESDIR");

            // If *.cfg exists in _USTN_ROLESDIR, enable Roles pick list
            bool localWildCardFilesExist() { string arglocation = Path.Combine(variable.Value, "*.cfg"); var ret = UtilitiesPath.WildCardFilesExist(ref arglocation); return ret; }

            if (localWildCardFilesExist() == true)
            {
                // If workspaceComboBox.Text <> CEResource.TXT_NoWorkspace Then
                roleComboBox.Enabled = true;
                FillRoles(ref roleComboBox, variable.Expand(), variable.IsLocked, ref ready);
                // Else
                // roleComboBox.Items.Clear()
                // roleComboBox.Items.Add(CEResource.TXT_NoWorkset)
                // roleComboBox.Text = roleComboBox.Items(0).ToString()
                // End If
            }
        }

        public void WorksetSelected(ref ComboBox applicationComboBox, ref ComboBox workspaceComboBox, ref ComboBox worksetComboBox)
        {

            Application.DoEvents();
            string path;
            CFGConfiguration wrk;
            CFGEnums.Application app;
            CFGVariable variable;
            if (string.IsNullOrEmpty(_USTN_WORKSPACESROOT))
                return;
            if (string.IsNullOrEmpty(workspaceComboBox.Text))
                return;

            // Find vert from combo selection
            string exePath = VBCompat.vbNullString;
            CFGEnums.ShortApplication sv = (CFGEnums.ShortApplication)applicationComboBox.SelectedItem;
            if (sv is not null)
                exePath = sv.ExePath;
            app = MainType.CESettings.GetApplication(applicationComboBox.Text.Trim(), exePath);
            if (string.IsNullOrEmpty(app.Name))
                return;

            string argworkspace_str = VBCompat.vbNullString;
            string argworkset_str = VBCompat.vbNullString;
            string argrole_str = VBCompat.vbNullString;
            wrk = Utilities.GetConfiguration(ref app.Name, ref app.EXEPath, ref argworkspace_str, ref argworkset_str, ref argrole_str);
            wrk.ApplicationData.Workset = "$(_USTN_WORKSETROOT)" + worksetComboBox.Text + ".cfg";

            // Fix Problem 4: Use ContainsKey
            if (wrk.Variables.ContainsKey("_USTN_WORKSETCFG"))
            {
                wrk.Variables.Remove("_USTN_WORKSETCFG");
            }

            variable = new CFGVariable(wrk)
            {
                Name = "_USTN_WORKSETCFG",
                Level = -1,
                NeedsSpecialExpansion = true,
                IsLocked = false
            };
            path = "$(_USTN_WORKSETROOT)" + worksetComboBox.Text + ".cfg";
            variable.SetValue(path);
            variable.SetFinalExpansion();
            wrk.Variables.Add(variable.Name, variable);
        }

        public void RoleSelected(ref ComboBox applicationComboBox, ref ComboBox workspaceComboBox, ref ComboBox roleComboBox)
        {

            Application.DoEvents();
            CFGConfiguration wrk;
            CFGEnums.Application app;
            CFGVariable variable;
            if (string.IsNullOrEmpty(_USTN_WORKSPACESROOT))
                return;
            if (string.IsNullOrEmpty(workspaceComboBox.Text))
                return;

            // Find vert from combo selection
            string exePath = VBCompat.vbNullString;
            CFGEnums.ShortApplication sv = (CFGEnums.ShortApplication)applicationComboBox.SelectedItem;
            if (sv is not null)
                exePath = sv.ExePath;
            app = MainType.CESettings.GetApplication(applicationComboBox.Text.Trim(), exePath);
            if (string.IsNullOrEmpty(app.Name))
                return;

            string argworkspace_str = VBCompat.vbNullString;
            string argworkset_str = VBCompat.vbNullString;
            string argrole_str = VBCompat.vbNullString;
            wrk = Utilities.GetConfiguration(ref app.Name, ref app.EXEPath, ref argworkspace_str, ref argworkset_str, ref argrole_str);
            wrk.ApplicationData.Role = roleComboBox.Text + ".cfg";

            variable = new CFGVariable(wrk)
            {
                Name = "_USTN_ROLE_NAME",
                Level = -1,
                NeedsSpecialExpansion = true,
                IsLocked = false
            };
            variable.SetValue(roleComboBox.Text);
            variable.SetFinalExpansion();
            //variable.IsLocked = true;
            wrk.Variables.Add(variable.Name, variable);
        }
    }
}
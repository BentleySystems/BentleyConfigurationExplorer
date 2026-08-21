// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using WorkspaceCFG.My;

namespace WorkspaceCFG
{

    public static class InterfaceControler
    {

        // ---------------------------------------------------------------------------------------
        // Public Members
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static CFGConfiguration MainConfiguration;
        public static CFGConfiguration CompareConfiguration1;
        public static CFGConfiguration CompareConfiguration2;
        public static MainForm MainForm;
        public static Status StatusForm;
        public static bool GroupWindowsInMainForm;

        // Status variables for main form
        public static string StatusFile;
        public static int StatusDepth;
        public static int StatusVariables;
        public static int StatusFiles;
        public static int StatusEvents;
        public static int StatusWarnings;
        public static int StatusErrors;
        public static bool StatusClickable;
        public static int StatusLineCount;

        public static bool ConfigurationIsProcessing;
        // ---------------------------------------------------------------------------------------
        // Private Members
        // ---------------+---------------+---------------+---------------+---------------+-------
        private readonly static ArrayList FrmCol = new ArrayList();          // Collection of open forms

        public static void ShowMainForm()
        {
            // Opt into true per-monitor v2 DPI awareness at runtime. This must happen before
            // EnableVisualStyles()/any Form is created. Previously this was declared in
            // app.manifest, but the modern Windows Forms guidance (WFO0003) is to configure it
            // via this API (or the 'ApplicationHighDpiMode' MSBuild property, which only takes
            // effect automatically through the SDK-generated ApplicationConfiguration.Initialize()
            // entry point -- not used here since this app has its own manual Main()).
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm = new MainForm();
            ProcessCommandLine();

            Application.Run(MainForm);
        }

        // ---------------------------------------------------------------------------------------
        // @description: Process CommandLine Arguments sent to Configuration Explorer
        // Accepts workspace files or link files.
        // ---------------+---------------+---------------+---------------+---------------+-------
        private static void ProcessCommandLine()
        {
            try
            {
                string[] args = Environment.GetCommandLineArgs();

                if (args.Length > 1)
                {
                    string filepath = args[1].Replace("\"", "");

                    if (File.Exists(filepath))
                    {
                        if (filepath.EndsWith(".bcf", StringComparison.OrdinalIgnoreCase))
                        {
                            LoadWorkspaceFile(filepath);
                        }

                        if (filepath.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase))
                        {
                            MainForm.IconFile = filepath;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] InterfaceController.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Shows the status form inside the MainMDI form
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void ShowStatus()
        {
            if (StatusForm is not null)
                StatusForm.Close();

            StatusForm = new Status()
            {
                MdiParent = MainForm,
                StartPosition = FormStartPosition.Manual,
                Location = new Point(10, 10),
                Dock = DockStyle.Fill
            };

            StatusForm.Show();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Hides the status form inside the MainMDI form
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void HideStatus()
        {
            if (StatusForm is not null)
                StatusForm.Close();

            Application.DoEvents();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Updates the status form inside the MainMDI form
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void UpdateStatus()
        {
            if (MainForm is null || StatusForm is null)
                return;

            StatusForm.lblCurrentFile.Text = StatusFile;
            StatusForm.lblDepth.Text = StatusDepth.ToString();
            StatusForm.lblVariables.Text = StatusVariables.ToString();
            StatusForm.lblFiles.Text = StatusFiles.ToString();
            StatusForm.lblEvents.Text = StatusEvents.ToString();
            StatusForm.lblWarnings.Text = StatusWarnings.ToString();
            StatusForm.lblErrors.Text = StatusErrors.ToString();
            StatusForm.lblLineCount.Text = StatusLineCount.ToString();
            StatusForm.ClickEnabled = StatusClickable;

            Application.DoEvents();
        }

        public static void ClearEcho()
        {
            if (MainForm is null || StatusForm is null)
                return;

            StatusForm.ListBoxEcho.Items.Clear();

            Application.DoEvents();
        }

        public static void Echo(string message)
        {
            if (MainForm is null || StatusForm is null)
                return;
            if (ConfigurationIsProcessing == false)
                return;

            StatusForm.ListBoxEcho.Items.Add(message);

            Application.DoEvents();
        }
        // ---------------------------------------------------------------------------------------
        // @description: Handles Rescanning the workspace and refreshing all open forms
        // ---------------+---------------+---------------+---------------+---------------+-------
        private static void RescanWorkspaces()
        {
            HideStatus();

            RescanWorkspace(ref CompareConfiguration1, true);
            RescanWorkspace(ref CompareConfiguration2, true);
            RescanWorkspace(ref MainConfiguration, false);

            if (MainForm is not null)
                MainForm.SetCaption();
            if (MainConfiguration is not null && StatusForm is not null)
                StatusForm.Workspace = MainConfiguration;

            FileHistory.NeedsRefreshed = false;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Handles Rescanning a single workspace
        // ---------------+---------------+---------------+---------------+---------------+-------
        private static void RescanWorkspace(ref CFGConfiguration configuration, bool hidewhendone = true)
        {
            if (configuration is null)
                return;

            var newWorkspace = new CFGConfiguration();

            newWorkspace.ApplicationData.CommandOptions = configuration.ApplicationData.CommandOptions;
            newWorkspace.ApplicationData.Workspace = configuration.ApplicationData.Workspace;
            newWorkspace.ApplicationData.Workset = configuration.ApplicationData.Workset;
            newWorkspace.ApplicationData.Role = configuration.ApplicationData.Role;
            newWorkspace.ApplicationData.Name = configuration.ApplicationData.Name;
            newWorkspace.ApplicationData.StartupCfgPath = configuration.ApplicationData.StartupCfgPath;
            newWorkspace.ApplicationData.EXEPath = configuration.ApplicationData.EXEPath;
            newWorkspace.ApplicationData.Rules = configuration.ApplicationData.Rules;
            newWorkspace.ResetStatus();
            newWorkspace.ShowStatus = true;
            newWorkspace.CloseStatusWhenDone = hidewhendone;
            newWorkspace.ApplicationData.PredefinedPath = configuration.ApplicationData.PredefinedPath;
            newWorkspace.ProcessConfiguration();

            configuration = newWorkspace;

            Application.DoEvents();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Sets the midparent value for all forms. (Group Windows in main form or not)
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void Set_Forms_MDIParent(ref bool enabled)
        {
            foreach (Form form in FrmCol)
            {
                if (enabled)
                {
                    form.MdiParent = MainForm;
                }
                else
                {
                    form.MdiParent = null;
                }
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Forces all form locations to 40,40 on the screen. Fixes issue with windows getting lost
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void ResetFormLocations()
        {
            var defaultPoint = new Point(40, 40);

            foreach (Form form in FrmCol)
                form.Location = defaultPoint;

            {
                var withBlock = MySettings.Default;
                withBlock.EH_Location = defaultPoint;
                withBlock.VC_Location = defaultPoint;
                withBlock.VE_Location = defaultPoint;
                withBlock.VL_Location = defaultPoint;
                withBlock.VR_Location = defaultPoint;
                withBlock.GC_Location = defaultPoint;
                withBlock.FV_Location = defaultPoint;
                withBlock.ST_Location = defaultPoint;
                withBlock.PS_Location = defaultPoint;
                withBlock.FH_Location = defaultPoint;

                withBlock.Save();
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Rescans and workspace and calls the Refresh function in all open forms.
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void RefreshOpenForms()
        {

            foreach (object form in FrmCol)
            {
                if (form is FileHistory fh)
                    fh.PromtSaveChanges();
                // Add other types as needed
            }

            RescanWorkspaces();

            foreach (object form in FrmCol)
                ((dynamic)form).RefreshForm();

            Application.DoEvents();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Calls the Refresh function in all open forms. (Without rescanning)
        // This is used if a new workspace is scanned and there are open windows.
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void RefreshForms()
        {
            foreach (object form in FrmCol)
                ((dynamic)form).RefreshForm();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Activates  Window.
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void ActivateDesiredWindow(string indexStr)
        {
            int index = int.Parse(indexStr);
            Form form = (Form)FrmCol[index];
            form.Activate();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Adds a form to the form collection
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void AddForm(ref object form)
        {
            FrmCol.Add(form);

            Application.DoEvents();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Removes a form from the form collection
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void RemoveForm(ref object form)
        {
            try
            {
                FrmCol.Remove(form);
                //TODO: Check this
                //form?.dispose();
             
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] InterfaceController.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }

            Application.DoEvents();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Returns a reference to a workspace object based on the workspace type
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void GetConfiguration(ref CFGConfiguration configuration, CFGEnums.ConfigurationType configurationType)
        {
            switch (configurationType)
            {

                case CFGEnums.ConfigurationType.Main:
                    {
                        configuration = MainConfiguration;
                        break;
                    }
                case CFGEnums.ConfigurationType.Compare1:
                    {
                        configuration = CompareConfiguration1;
                        break;
                    }
                case CFGEnums.ConfigurationType.Compare2:
                    {
                        configuration = CompareConfiguration2;
                        break;
                    }

                default:
                    {
                        configuration = null;
                        break;
                    }
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Sets the current workspace based on type
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void SetConfiguration(ref CFGConfiguration configuration, CFGEnums.ConfigurationType workspaceType)
        {
            switch (workspaceType)
            {
                case CFGEnums.ConfigurationType.Main:
                    {
                        MainConfiguration = configuration;
                        if (StatusForm is not null)
                            StatusForm.Workspace = configuration;
                        break;
                    }
                case CFGEnums.ConfigurationType.Compare1:
                    {
                        CompareConfiguration1 = configuration;
                        break;
                    }
                case CFGEnums.ConfigurationType.Compare2:
                    {
                        CompareConfiguration2 = configuration;
                        break;
                    }

                default:
                    {
                        configuration = null;
                        break;
                    }
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Loads a workspace from file into the main form
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void LoadWorkspaceFile(string filepath)
        {
            if (MainForm is not null)
                MainForm.LoadConfigurationFile(filepath);
        }

        // ---------------------------------------------------------------------------------------
        // @description: Clears all existing workspaces
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void CloseConfigurations()
        {
            MainConfiguration = null;
            CompareConfiguration1 = null;
            CompareConfiguration2 = null;

            Application.DoEvents();
        }

    }
}
// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using WorkspaceCFG.My;

namespace WorkspaceCFG
{

    public partial class ConfigurationSelection
    {
        private bool _ready;

        // ---------------------------------------------------------------------------------------
        // Public Members
        // ---------------+---------------+---------------+---------------+---------------+-------
        public string CurrentApplication;
        public string CurrentWorkspace;
        public string CurrentWorkset;
        public string CurrentRole;
        public string CurrentApplicationPath;

        public bool UpdateEnabled;       // Disables or enables the combobox changed events
        public bool Cancel;
        public ConfigurationSelector Selector;

        // TODO: handle settings

        // Runtime-only flag; not meant to be persisted by the Windows Forms designer.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Ready
        {
            get
            {
                return _ready;
            }
            set
            {
                _ready = value;

                var ev = new ReadyChangedEvent() { Ready = _ready };
                ReadyChanged?.Invoke(this, ev);
            }
        }

        public event ReadyChangedEventHandler ReadyChanged;

        public delegate void ReadyChangedEventHandler(object sender, ReadyChangedEvent e);

        public ConfigurationSelection()
        {
            // This call is required by the designer.
            InitializeComponent();

            Cancel = true;
            Selector = new ConfigurationSelector();
        }

        public void LoadSettings()
        {
            Application.DoEvents();
            cmbApplication.Text = MySettings.Default.ChosenApplication;
            cmbWorkpace.Text = MySettings.Default.ChosenWorkspace;
            cmbWorkset.Text = MySettings.Default.ChosenWorkset;
            CurrentApplicationPath = MySettings.Default.ChosenApplicationPath;
        }

        public void SaveSettings()
        {
            MySettings.Default.ChosenApplication = cmbApplication.Text;
            MySettings.Default.ChosenWorkspace = cmbWorkpace.Text;
            MySettings.Default.ChosenWorkset = cmbWorkset.Text;

            CFGEnums.ShortApplication sv = (CFGEnums.ShortApplication)cmbApplication.SelectedItem;
            if (sv is not null)
            {
                MySettings.Default.ChosenApplicationPath = sv.ExePath;
            }
            // Main.CESettings.Save()
            MySettings.Default.Save();
        }


        // ---------------------------------------------------------------------------------------
        // @description: Returns a workspace object that's ready to be processed
        // ---------------+---------------+---------------+---------------+---------------+-------
        public CFGConfiguration GetConfiguration()
        {
            var argapplication_cmb = cmbApplication;
            var argworkspace_cmb = cmbWorkpace;
            var argworkset_cmb = cmbWorkset;
            var argrole_cmb = cmbRole;
            return Utilities.GetConfiguration(ref argapplication_cmb, ref argworkspace_cmb, ref argworkset_cmb, ref argrole_cmb);
        }

        public void UpdateApplications()
        {
            InterfaceControler.MainForm.Cursor = Cursors.WaitCursor;

            // If an input application was given then fill the form with its data

            {
                ref var withBlock = ref Selector.InputApplication;
                if (!string.IsNullOrEmpty(withBlock.Name))
                {
                    UpdateEnabled = false;
                    cmbApplication.Text = withBlock.Name;
                    cmbWorkpace.Text = withBlock.Workspace;
                    cmbWorkset.Text = withBlock.Workset;
                    cmbRole.Text = withBlock.Role;
                    UpdateEnabled = true;
                }
            }

            var tempvert = MainType.CESettings.GetApplication(cmbApplication.Text.Trim(), CurrentApplicationPath);
            if (string.IsNullOrEmpty(tempvert.Name))
            {
                UpdateEnabled = false;
                cmbApplication.Text = string.Empty;
                cmbWorkpace.Text = string.Empty;
                cmbWorkset.Text = string.Empty;
                cmbRole.Text = string.Empty;
                UpdateEnabled = true;
            }

            CurrentApplication = cmbApplication.Text;
            CurrentWorkspace = cmbWorkpace.Text;
            CurrentWorkset = cmbWorkset.Text;
            CurrentRole = cmbRole.Text;

            cmbApplication.Items.Clear();
            if (string.IsNullOrEmpty(cmbApplication.Text))
            {
                var argappComboBox = cmbApplication;
                var argworkspaceComboBox = cmbWorkpace;
                var argworksetComboBox = cmbWorkset;
                var argroleComboBox = cmbRole;
                Selector.InitializeCombos(ref argappComboBox, ref argworkspaceComboBox, ref argworksetComboBox, ref argroleComboBox);
                cmbApplication = argappComboBox;
                cmbWorkpace = argworkspaceComboBox;
                cmbWorkset = argworksetComboBox;
                cmbRole = argroleComboBox;
            }
            else
            {
                var argApplicationComboBox = cmbApplication;
                Selector.FillApplicationsCombo(ref argApplicationComboBox);
                cmbApplication = argApplicationComboBox;
                string argapplicationName = cmbApplication.Text;
                var argworkspacesComboBox = cmbWorkpace;
                var argroleComboBox1 = cmbRole;
                Selector.ApplicationSelected(ref argapplicationName, ref CurrentApplicationPath, ref argworkspacesComboBox, ref argroleComboBox1);
                cmbApplication.Text = argapplicationName;
                cmbWorkpace = argworkspacesComboBox;
                cmbRole = argroleComboBox1;
                ValidateSelectedWorkspace();
                var argapplicationComboBox = cmbApplication;
                var argworkspaceComboBox1 = cmbWorkpace;
                var argworksetComboBox1 = cmbWorkset;
                var argroleComboBox2 = cmbRole;
                bool argready = Ready;
                Selector.WorkspaceSelected(ref argapplicationComboBox, ref argworkspaceComboBox1, ref argworksetComboBox1, ref argroleComboBox2, ref argready);
                cmbApplication = argapplicationComboBox;
                cmbWorkpace = argworkspaceComboBox1;
                cmbWorkset = argworksetComboBox1;
                cmbRole = argroleComboBox2;
                Ready = argready;
                CurrentWorkset = cmbWorkset.Text;
            }

            InterfaceControler.MainForm.Cursor = Cursors.Default;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Update combo boxes based on the selected application
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void UpdateSelection()
        {
            InterfaceControler.MainForm.Cursor = Cursors.WaitCursor;

            var argappComboBox = cmbApplication;
            var argworkspaceComboBox = cmbWorkpace;
            Selector.ApplicationSelected2(ref argappComboBox, ref argworkspaceComboBox, cmbRole);
            cmbApplication = argappComboBox;
            cmbWorkpace = argworkspaceComboBox;
            CurrentApplication = cmbApplication.Text;
            CurrentApplicationPath = cmbApplication.SelectedValue?.ToString();

            if (cmbWorkpace.Items.Count > 0)
            {
                cmbWorkpace.Text = cmbWorkpace.Items[0].ToString();
            }

            UpdateWorkspace();
        }

        private void UpdateWorkspace()
        {
            InterfaceControler.MainForm.Cursor = Cursors.WaitCursor;

            var argapplicationComboBox = cmbApplication;
            var argworkspaceComboBox = cmbWorkpace;
            var argworksetComboBox = cmbWorkset;
            var argroleComboBox = cmbRole;
            bool argready = Ready;
            Selector.WorkspaceSelected(ref argapplicationComboBox, ref argworkspaceComboBox, ref argworksetComboBox, ref argroleComboBox, ref argready);
            cmbApplication = argapplicationComboBox;
            cmbWorkpace = argworkspaceComboBox;
            cmbWorkset = argworksetComboBox;
            cmbRole = argroleComboBox;
            Ready = argready;
            CurrentWorkspace = cmbWorkpace.Text;
            CurrentWorkset = cmbWorkset.Text;

            InterfaceControler.MainForm.Cursor = Cursors.Default;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Validate if the workspace set in the cmbWorkspace.Text is still valid
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void ValidateSelectedWorkspace()
        {
            foreach (string workspace in cmbWorkpace.Items)
            {
                if (CultureInfo.CurrentCulture.CompareInfo.Compare(cmbWorkpace.Text ?? "", workspace ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                    return;
            }

            // Select first item if one exists
            if (cmbWorkpace.Items.Count > 0)
            {
                CurrentWorkspace = cmbWorkpace.Items[0]?.ToString();
            }
            else
            {
                CurrentWorkspace = string.Empty;
            }

            cmbWorkpace.Text = CurrentWorkspace;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Handles a application being selected in the combobox
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void CmbApplication_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(CurrentApplication ?? "", cmbApplication.Text ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
            {
                UpdateSelection();
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description:  Handles a workspace being selected in the combobox
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void CmbWorkspace_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(CurrentWorkspace ?? "", cmbWorkpace.Text ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
            {
                UpdateWorkspace();
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description:  Handles a workset being selected in the combobox
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void CmbWorkset_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(cmbWorkset.Text ?? "", CurrentWorkset ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
            {
                CurrentWorkset = cmbWorkset.Text;
                var argapplicationComboBox = cmbApplication;
                var argworkspaceComboBox = cmbWorkpace;
                var argworksetComboBox = cmbWorkset;
                Selector.WorksetSelected(ref argapplicationComboBox, ref argworkspaceComboBox, ref argworksetComboBox);
                cmbApplication = argapplicationComboBox;
                cmbWorkpace = argworkspaceComboBox;
                cmbWorkset = argworksetComboBox;
            }
        }

        private void CmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(cmbRole.Text ?? "", CurrentRole ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
            {
                CurrentRole = cmbRole.Text;
                var argapplicationComboBox = cmbApplication;
                var argworkspaceComboBox = cmbWorkpace;
                var argroleComboBox = cmbRole;
                Selector.RoleSelected(ref argapplicationComboBox, ref argworkspaceComboBox, ref argroleComboBox);
                cmbApplication = argapplicationComboBox;
                cmbWorkpace = argworkspaceComboBox;
                cmbRole = argroleComboBox;
            }
        }
    }

    public class ReadyChangedEvent : EventArgs
    {
        public bool Ready;
    }
}
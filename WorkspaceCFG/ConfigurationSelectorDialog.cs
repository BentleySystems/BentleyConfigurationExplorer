// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Windows.Forms;
using WorkspaceCFG.My;

namespace WorkspaceCFG
{

    public partial class ConfigurationSelectorDialog
    {

        // ---------------------------------------------------------------------------------------
        // Public Members
        // ---------------+---------------+---------------+---------------+---------------+-------
        // Public Cancel As Boolean
        public ConfigurationSelector Selector;

        // ---------------------------------------------------------------------------------------
        // @description: Overloaded new sub
        // ---------------+---------------+---------------+---------------+---------------+-------
        public ConfigurationSelectorDialog()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
        }

        public ConfigurationSelectorDialog(ref CFGEnums.Application application)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            ConfigurationSelection1.Selector = new ConfigurationSelector(application); // TOD: KM is this needed?
            ConfigurationSelection1.UpdateEnabled = true;
        }

        public CFGConfiguration GetConfiguration()
        {
            return ConfigurationSelection1.GetConfiguration();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Handles form shown event. Loads settings. Initializes combo boxes.
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void ConfigurationSelector_Shown(object sender, EventArgs e)
        {
            if (MySettings.Default.AutoloadLastWorkspace)
            {
                ConfigurationSelection1.LoadSettings();
            }

            ConfigurationSelection1.UpdateApplications();
        }

        // ---------------------------------------------------------------------------------------
        // @description: OK Button: Closes the form
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void BtnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            Close();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Cancel Button: Closes the form
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            var appWizard = new AppWizard();

            if (InterfaceControler.GroupWindowsInMainForm)
            {
                appWizard.MdiParent = (Form)Parent;
            }

            appWizard.ShowDialog();

            // Refresh the list of apps
            ConfigurationSelection1.UpdateApplications();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Handles form closing event. Saves Settings
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void ConfigurationSelector_FormClosing(object sender, FormClosingEventArgs e)
        {
            MySettings.Default.PS_StartPos = StartPosition;
            MySettings.Default.PS_Location = Location;
            MySettings.Default.Save();
            MainType.CESettings.Save();
            ConfigurationSelection1.SaveSettings();
        }

        private void ConfigurationSelection1_ReadyChanged(object sender, ReadyChangedEvent e)
        {
            btnOK.Enabled = e.Ready;
        }

        private void ConfigurationSelectorDialog_Load(object sender, EventArgs e)
        {

        }
    }
}
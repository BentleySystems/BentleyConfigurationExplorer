// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Windows.Forms;
using WorkspaceCFG.My;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{

    public partial class ConfigurationCompare
    {

        private CFGConfiguration _configuration1;
        private CFGConfiguration _configuration2;
        private bool _ignoreUpdate;

        public ConfigurationCompare()
        {
            // This call is required by the designer.
            InitializeComponent();

            // Me.Icon= Utilities.GetApplicationIcon() 'TODO KM can set icon from app if needed
            _ignoreUpdate = true;
        }

        private bool AreWorkspacesSelected()
        {
            if (RadioButtonConfiguration1.Checked)
            {
                if (string.IsNullOrEmpty(ConfigurationSelection1.CurrentApplication))
                {
                    MessageBox.Show(CEResource.TXT_MsgWorkSpace1NeedsVertSelection, CEResource.TXT_TitleError);
                    return false;
                }
            }

            if (RadioButtonConfiguration2.Checked)
            {
                if (string.IsNullOrEmpty(ConfigurationSelection2.CurrentApplication))
                {
                    MessageBox.Show(CEResource.TXT_MsgWorkSpace2NeedsVertSelection1, CEResource.TXT_TitleError);
                    return false;
                }
            }

            return true;
        }

        private bool AreWorkspacesValid()
        {
            if (RadioButtonConfiguration1.Checked)
            {
                _configuration1 = ConfigurationSelection1.GetConfiguration();
                Cursor = Cursors.WaitCursor;

                _configuration1?.ProcessConfiguration();

                Cursor = Cursors.Default;
                if (_configuration1 is not null)
                    _configuration1.IsFromFile = false;
            }
            else
            {
                string argpath = TextBoxFile1.Text;
                bool argsilent = false;
                WorkspaceExport.LoadConfiguration(ref argpath, ref _configuration1, silent: ref argsilent);
                TextBoxFile1.Text = argpath;
                if (_configuration1 is not null)
                    _configuration1.IsFromFile = true;
            }

            if (_configuration1 is null)
            {
                MessageBox.Show(CEResource.TXT_MsgProblemReadingWorkSpace1, CEResource.TXT_TitleError);
                return false;
            }

            if (RadioButtonConfiguration2.Checked)
            {
                _configuration2 = ConfigurationSelection2.GetConfiguration();
                Cursor = Cursors.WaitCursor;

                _configuration2?.ProcessConfiguration();

                Cursor = Cursors.Default;
                if (_configuration2 is not null)
                    _configuration2.IsFromFile = false;
            }
            else
            {
                string argpath1 = TextBoxFile2.Text;
                bool argsilent1 = false;
                WorkspaceExport.LoadConfiguration(ref argpath1, ref _configuration2, silent: ref argsilent1);
                TextBoxFile2.Text = argpath1;
                if (_configuration2 is not null)
                    _configuration2.IsFromFile = true;
            }

            if (_configuration2 is null)
            {
                MessageBox.Show(CEResource.TXT_MsgProblemReadingWorkSpace2, CEResource.TXT_TitleError);
                return false;
            }

            return true;
        }

        private void CompareConfiguration()
        {
            if (!AreWorkspacesSelected())
                return;
            if (!AreWorkspacesValid())
                return;

            InterfaceControler.SetConfiguration(ref _configuration1, CFGEnums.ConfigurationType.Compare1);
            InterfaceControler.SetConfiguration(ref _configuration2, CFGEnums.ConfigurationType.Compare2);

            var variableCompare = new VariableCompare();
            if (InterfaceControler.GroupWindowsInMainForm)
                variableCompare.MdiParent = MdiParent;
            variableCompare.Show();

            MySettings.Default.Save();
            Close();
        }

        private void GetFilename(ref TextBox textBox)
        {
            var OpenFileDialog1 = new OpenFileDialog();

            OpenFileDialog1.Title = CEResource.TXT_TitleOpenWorkSpaceFromFile;
            OpenFileDialog1.InitialDirectory = !string.IsNullOrEmpty(textBox.Text) ? textBox.Text : CEResource.TXT_CDrive;
            OpenFileDialog1.Filter = CEResource.TXT_ExtOpenWorkSpaceFilter;
            OpenFileDialog1.FileName = CEResource.TXT_ExtWrk;
            OpenFileDialog1.RestoreDirectory = true;

            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox.Text = OpenFileDialog1.FileName;
            }
        }

        public CFGConfiguration GetConfiguration1()
        {
            if (RadioButtonConfiguration1.Checked == true)
            {
                return ConfigurationSelection1.GetConfiguration();
            }
            else
            {
                CFGConfiguration wrkspc = null;
                string argpath = TextBoxFile1.Text;
                bool argsilent = false;
                WorkspaceExport.LoadConfiguration(ref argpath, ref wrkspc, silent: ref argsilent);
                TextBoxFile1.Text = argpath;
                return wrkspc;
            }
        }

        public CFGConfiguration GetConfiguration2()
        {
            if (RadioButtonConfiguration2.Checked == true)
            {
                return ConfigurationSelection2.GetConfiguration();
            }
            else
            {
                CFGConfiguration wrkspc = null;
                string argpath = TextBoxFile2.Text;
                bool argsilent = false;
                WorkspaceExport.LoadConfiguration(ref argpath, ref wrkspc, silent: ref argsilent);
                TextBoxFile2.Text = argpath;

                return wrkspc;
            }
        }

        private void UpdateControls()
        {
            if (_ignoreUpdate)
                return;

            ConfigurationSelection1.Enabled = RadioButtonConfiguration1.Checked;
            TextBoxFile1.Enabled = RadioButtonFile1.Checked;
            ButtonBrowse1.Enabled = RadioButtonFile1.Checked;
            ConfigurationSelection2.Enabled = RadioButtonConfiguration2.Checked;
            TextBoxFile2.Enabled = RadioButtonFile2.Checked;
            ButtonBrowse2.Enabled = RadioButtonFile2.Checked;
        }

        private void RadioButtonWorkspace1_CheckedChanged(object sender, EventArgs e)
        {
            if (_ignoreUpdate)
                return;

            UpdateControls();
        }

        private void RadioButtonWorkspace2_CheckedChanged(object sender, EventArgs e)
        {
            if (_ignoreUpdate)
                return;

            UpdateControls();
        }

        private void ButtonBrowse1_Click(object sender, EventArgs e)
        {
            var argtextBox = TextBoxFile1;
            GetFilename(ref argtextBox);
            TextBoxFile1 = argtextBox;
        }

        private void ButtonBrowse2_Click(object sender, EventArgs e)
        {
            var argtextBox = TextBoxFile2;
            GetFilename(ref argtextBox);
            TextBoxFile2 = argtextBox;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            CompareConfiguration();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ConfigurationCompare_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Remove this form instance from the interface controller
            object argform = this;
            InterfaceControler.RemoveForm(ref argform);
        }

        private void ConfigurationCompare_Load(object sender, EventArgs e)
        {
            // Gets a workspace instance
            object argform = this;
            InterfaceControler.AddForm(ref argform);
        }

        private void ConfigurationCompare_Shown(object sender, EventArgs e)
        {
            _ignoreUpdate = false;

            ButtonOK.Enabled = false;
            UpdateControls();

            ConfigurationSelection1.UpdateApplications();
            ConfigurationSelection2.UpdateApplications();

            ButtonOK.Enabled = true;
        }

    }
}
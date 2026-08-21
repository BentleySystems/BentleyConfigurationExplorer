// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;

namespace WorkspaceCFG
{
    public partial class Status
    {

        public CFGConfiguration Workspace;
        public bool ClickEnabled;

        ~Status()
        {
            Workspace = null;
        }

        public Status()
        {
            InitializeComponent();
            lblLineCount = _lblLineCount;
            lblDepth = _lblDepth;
            lblVariables = _lblVariables;
            lblFiles = _lblFiles;
            lblEvents = _lblEvents;
            lblCurrentFile = _lblCurrentFile;
            lblWarnings = _lblWarnings;
            lblErrors = _lblErrors;
            _lblLineCount.Name = "lblLineCount";
            _lblDepth.Name = "lblDepth";
            _lblVariables.Name = "lblVariables";
            _lblFiles.Name = "lblFiles";
            _lblEvents.Name = "lblEvents";
            _lblCurrentFile.Name = "lblCurrentFile";
            _lblWarnings.Name = "lblWarnings";
            _lblErrors.Name = "lblErrors";
        }

        private void Link_Variables_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            SendToBack();

            if (ClickEnabled == false)
                return;
            if (Workspace is null)
                return;

            var variableExplorer = new VariableExplorer(CFGEnums.ConfigurationType.Main);
            if (InterfaceControler.GroupWindowsInMainForm)
                variableExplorer.MdiParent = MdiParent;
            variableExplorer.Show();
        }

        private void Link_Files_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            SendToBack();

            if (Workspace is null)
                return;
            if (ClickEnabled == false)
                return;

            var Filex = new FileHistory(CFGEnums.ConfigurationType.Main);
            if (InterfaceControler.GroupWindowsInMainForm)
                Filex.MdiParent = MdiParent;
            Filex.Show();
        }

        private void Link_Events_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            SendToBack();

            if (Workspace is null)
                return;
            if (ClickEnabled == false)
                return;

            var eventHistory = new EventHistory(CFGEnums.ConfigurationType.Main);
            eventHistory.IgnoreBuild = true;
            eventHistory.CheckBox1.Checked = true;
            eventHistory.CheckBox2.Checked = true;
            eventHistory.CheckBox3.Checked = true;
            eventHistory.CheckBox4.Checked = true;
            eventHistory.CheckBox4.Checked = true;
            eventHistory.ComboBox1.Text = "";
            eventHistory.IgnoreBuild = false;
            if (InterfaceControler.GroupWindowsInMainForm)
                eventHistory.MdiParent = MdiParent;
            eventHistory.Show();
        }

        private void Link_Warnings_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            SendToBack();

            if (Workspace is null)
                return;
            if (ClickEnabled == false)
                return;

            var eventHistory = new EventHistory(CFGEnums.ConfigurationType.Main);
            eventHistory.IgnoreBuild = true;
            eventHistory.CheckBox1.Checked = false;
            eventHistory.CheckBox2.Checked = true;
            eventHistory.CheckBox3.Checked = false;
            eventHistory.CheckBox4.Checked = false;
            eventHistory.CheckBox4.Checked = false;
            eventHistory.ComboBox1.Text = "";
            eventHistory.IgnoreBuild = false;
            if (InterfaceControler.GroupWindowsInMainForm)
                eventHistory.MdiParent = MdiParent;
            eventHistory.Show();
        }

        private void Link_Errors_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            SendToBack();
            if (Workspace is null)
                return;

            if (ClickEnabled == false)
                return;
            var eventHistory = new EventHistory(CFGEnums.ConfigurationType.Main);
            eventHistory.IgnoreBuild = true;
            eventHistory.CheckBox1.Checked = true;
            eventHistory.CheckBox2.Checked = false;
            eventHistory.CheckBox3.Checked = false;
            eventHistory.CheckBox4.Checked = false;
            eventHistory.CheckBox4.Checked = false;
            eventHistory.ComboBox1.Text = "";
            eventHistory.IgnoreBuild = false;
            if (InterfaceControler.GroupWindowsInMainForm)
                eventHistory.MdiParent = MdiParent;
            eventHistory.Show();
        }

        private void Status_GotFocus(object sender, EventArgs e)
        {
            SendToBack();
        }

        private void Status_LostFocus(object sender, EventArgs e)
        {
            SendToBack();
        }

    }
}
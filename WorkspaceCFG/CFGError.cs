// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Windows.Forms;

namespace WorkspaceCFG
{

    public partial class CFGError
    {

        public string FilePath;
        public int LineNumber;

        public CFGError()
        {
            InitializeComponent();
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Utilities.OpenInEditor(ref FilePath, line: LineNumber);
            Close();
        }

    }
}
// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Windows.Forms;
using Bentley.AboutApp.Common;

namespace Bentley.ConfigurationExplorer.AboutApp
{
    /*====================================================================================**/
    /// <summary></summary>
    /*==============+===============+===============+===============+===============+======*/
    public partial class LegalInformation : Form
    {
        public LegalInformation()
        {
            InitializeComponent();
        }

        /*------------------------------------------------------------------------------------**/
        /// <summary></summary>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        private void buttonOk_Click(object sender,EventArgs e)
        {
            this.Close();
        }

        /*------------------------------------------------------------------------------------**/
        /// <summary></summary>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        private void LegalInformation_FormClosing(object sender, FormClosingEventArgs e)
        {
            CommonUtil.IsLegalNoticeOpen = false;
        }

        /*------------------------------------------------------------------------------------**/
        /// <summary>To open the clicked link present in richTextBox</summary>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }
    }
}
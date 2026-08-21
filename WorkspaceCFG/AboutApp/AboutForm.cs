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
    public partial class AboutForm : Form
    {

        private readonly String _versionString;

        /*------------------------------------------------------------------------------------**/
        /// <summary></summary>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        public AboutForm()
        {
            InitializeComponent ();
        }

        public AboutForm(String versionStr)
        {
            _versionString= versionStr;
            InitializeComponent();
        }

        /*------------------------------------------------------------------------------------**/
        /// <summary></summary>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        private void AboutForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CommonUtil.IsAboutOpen = false;
        }

        /*------------------------------------------------------------------------------------**/
        /// <summary></summary>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        private void About_Load(object sender, EventArgs e)
        {
            lblVersion.Text = _versionString;
        }

        /*------------------------------------------------------------------------------------**/
        /// <summary></summary>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        private void ButtonLegal_Click(object sender, EventArgs e)
        {
            if (!CommonUtil.IsLegalNoticeOpen)
            {
                CommonUtil.IsLegalNoticeOpen = true;
                new LegalInformation().Show ();
            }
        }

        /*------------------------------------------------------------------------------------**/
        /// <summary></summary>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        private void ButtonOK_Click(object sender, EventArgs e)
        {
            this.Close ();
        }
    }
}
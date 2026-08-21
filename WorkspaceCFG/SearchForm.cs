// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;

namespace WorkspaceCFG
{
    public partial class SearchForm
    {
        public string SearchCriteria;

        public SearchForm()
        {
            InitializeComponent();
        }

        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            ReturnCriteria();
        }

        private void TextBoxSearch_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                ReturnCriteria();
            }
        }

        private void ReturnCriteria()
        {
            SearchCriteria = TextBoxSearch.Text;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
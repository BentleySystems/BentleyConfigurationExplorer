// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using Microsoft.Win32;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{

    public partial class InstalledApps
    {

        public InstalledApps()
        {
            // This call is required by the designer.
            InitializeComponent();
        }

        private void InstalledApps_Load(object sender, EventArgs e)
        {
            {
                var withBlock = DataGridView4;
                withBlock.Columns.Add(CEResource.TXT_LabelProduct, CEResource.TXT_LabelProduct);
                withBlock.Columns.Add(CEResource.TXT_LabelVersion, CEResource.TXT_LabelVersion);
            }

            BuildBentleyApps();
        }

        // TODO: somewhat overlap: GetBentleyApps.GetBentleyApps and Utilities.GetListOfInstalledProductsWithVersionNumbers and InstalledApplication.BuildBentleyApps and FillInstalledProductsList
        public void BuildBentleyApps()
        {
            string uninstallKey;
            RegistryKey key;
            RegistryKey subkey;
            string[] subkeyNames;
            string name;
            string publisher;
            string version;
            string urlInfoAbout;

            uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

            key = Registry.LocalMachine.OpenSubKey(uninstallKey);
            subkeyNames = key.GetSubKeyNames();

            int crow = 0;

            for (int counter = 0, loopTo = subkeyNames.Length - 1; counter <= loopTo; counter++)
            {
                subkey = key.OpenSubKey(subkeyNames[counter]);
                {
                    var withBlock = DataGridView4;
                    name = subkey.GetValue("DisplayName")?.ToString();
                    version = subkey.GetValue("DisplayVersion")?.ToString();
                    publisher = subkey.GetValue("Publisher")?.ToString();
                    urlInfoAbout = subkey.GetValue("URLInfoAbout")?.ToString();

                    if (publisher.IndexOf("bentley", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        name.IndexOf("bentley", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        name.IndexOf("microstation", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        name.IndexOf("projectwise", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        urlInfoAbout.IndexOf("bentley", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        withBlock.Rows.Add(1);
                        withBlock.Rows[crow].Cells[0].Value = name;
                        withBlock.Rows[crow].Cells[1].Value = version;
                        crow += 1;
                    }
                }
            }

            // TODO: FIXME any use of IS64Bit() should be checked or removed (see SetProgramFiles for an example)
            if (Utilities.Is64Bit())
            {
                uninstallKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
                key = Registry.LocalMachine.OpenSubKey(uninstallKey);
                subkeyNames = key.GetSubKeyNames();
                for (int counter = 0, loopTo1 = subkeyNames.Length - 1; counter <= loopTo1; counter++)
                {
                    subkey = key.OpenSubKey(subkeyNames[counter]);
                    {
                        var withBlock1 = DataGridView4;
                        name = subkey.GetValue("DisplayName")?.ToString();
                        version = subkey.GetValue("DisplayVersion")?.ToString();
                        publisher = subkey.GetValue("Publisher")?.ToString();
                        urlInfoAbout = subkey.GetValue("URLInfoAbout")?.ToString();

                        if (publisher.IndexOf("bentley", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            name.IndexOf("bentley", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            name.IndexOf("microstation", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            name.IndexOf("projectwise", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            urlInfoAbout.IndexOf("bentley", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            withBlock1.Rows.Add(1);
                            withBlock1.Rows[crow].Cells[0].Value = name;
                            withBlock1.Rows[crow].Cells[1].Value = version;
                            crow += 1;
                        }
                    }
                }
            }

            DataGridView4.AutoResizeColumns();
            DataGridView4.Sort(DataGridView4.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
        }

    }
}
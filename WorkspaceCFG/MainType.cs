// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Threading;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using System.Diagnostics;

namespace WorkspaceCFG
{

    static class MainType
    {

        public static CESettings CESettings = new CESettings();

        [STAThread]
        public static void Main()
        {
            // Load application settings
            CESettings = CESettings.Load();
            
            // Show main application window
            InterfaceControler.ShowMainForm();
        }
    }
}
// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace WorkspaceCFG
{

    [Serializable]
    public class CESettings
    {

        public List<CFGEnums.Application> Applications = new List<CFGEnums.Application>();
        public int HighlightModifiedInterval = 12;        // Number of hours to look for modifications in a file (File History)
        public string LastAppLoaded = "";
        private static string GetFileFullPath()
        {
            string localAppDataDir = Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%");

            if (localAppDataDir.Equals("%LOCALAPPDATA%"))
            {
                localAppDataDir = Environment.ExpandEnvironmentVariables("%APPDATA%");
            }

            string dirPath = Path.Combine(localAppDataDir, "Bentley", "ConfigurationExplorer");

            try
            {
                if (!Directory.Exists(dirPath))
                {
                    // try to create the directory
                    Directory.CreateDirectory(dirPath);
                }
            }
            catch
            {
                throw;
            }

            object fileFullPath = Path.Combine(dirPath, "CESETTINGS.xml");

            return fileFullPath?.ToString();
        }

        public static CESettings Load()
        {
            string fileFullPath = GetFileFullPath();

            if (!File.Exists(fileFullPath))
            {
                XmlSerialization.SerializeToFile(new CESettings(), fileFullPath);
            }

            return XmlSerialization.DeserializeFromFile<CESettings>(fileFullPath);
        }

        // Find Application by name and exe path

        public CFGEnums.Application GetApplication(string name, string exePath)
        {
            foreach (var app in Applications)
            {
                if (CultureInfo.CurrentCulture.CompareInfo.Compare(app.Name ?? "", name ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 && CultureInfo.CurrentCulture.CompareInfo.Compare(app.EXEPath ?? "", exePath ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                    return app;
            }

            return default;
        }

        public void OpenSettingsFile()
        {
            string argFilename = GetFileFullPath();
            Utilities.OpenInEditor(ref argFilename, true);
        }

        public void Save()
        {
            // updates with last app
            XmlSerialization.SerializeToFile(this, GetFileFullPath());
        }

    }
}
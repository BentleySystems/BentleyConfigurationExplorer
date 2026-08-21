// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using static System.Environment;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace WorkspaceCFG
{

    static class UtilitiesPath
    {

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns the text file encoding type ( ASCII or Unicode )
        // 
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static System.Text.Encoding CheckEncoding(ref string filepath)
        {
            System.Text.Encoding enc;

            using (var @file = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {

                if (@file.CanSeek)
                {
                    byte[] bom = new byte[4]; // Get the byte-order mark, if there is one
                    int bytesRead = @file.ReadAtLeast(bom, 2, throwOnEndOfStream: false);

                    if (bytesRead >= 3 && bom[0] == 0xEF && bom[1] == 0xBB && bom[2] == 0xBF ||
                        bytesRead >= 2 && bom[0] == 0xFF && bom[1] == 0xFE ||
                        bytesRead >= 2 && bom[0] == 0xFE && bom[1] == 0xFF ||
                        bytesRead == 4 && bom[0] == 0 && bom[1] == 0 && bom[2] == 0xFE && bom[3] == 0xFF) // ucs-4
                    {
                        enc = System.Text.Encoding.Unicode;
                    }
                    else
                    {
                        enc = System.Text.Encoding.ASCII;
                    }
                }
                else
                {
                    enc = System.Text.Encoding.ASCII;
                }
            }

            return enc;
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns the file extension of the input string.
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static string ExtFromPath(ref string path)
        {
            if (path is null)
                return null;

            int lastDot = path.LastIndexOf('.');
            if (lastDot == -1 || lastDot == path.Length - 1)
                return "";
            return path.Substring(lastDot);
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns true if a folder exists
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static bool FolderExists(string FolderPath)
        {
            try
            {
                var f = new DirectoryInfo(FolderPath);
                return f.Exists;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] UtilitiesPath.{MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                return false;
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns a path without the filename.
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static string GetDirectoryName(ref string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return "";
            string filename = GetFileName(path);
            return path.Substring(0, path.Length - filename.Length);
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns the filename from a path
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static string GetFileName(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return "";

            int pos = -1;

            for (int t = 0; t < path.Length; t++)
            {
                if (path[t] == '\\' || path[t] == '/')
                    pos = t; // Added check for '/' as well.
            }

            if (pos == -1)
                return path;

            return path.Substring(pos + 1);
        }


        // ---------------------------------------------------------------------------------------
        // @description: 'Returns the NOExt of the input string which is the file and path without the file extension.
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static string GetPathWithoutExtension(ref string path)
        {
            int lastDot = path.LastIndexOf('.');
            if (lastDot > 0)
            {
                return path.Substring(0, lastDot);
            }
            return path;
        }

        // In each subfolder of that path, there is a DirectoryExplanation.txt file. In that file is the path to where this app is installed.
        // We want to search that text file for the installation path of this app, and the folder that contains the correct text file is the correct
        // sub directory.

        public static string GetSystemApplicationDir(string EnvVariable,string appname, string rootdir)
        {
            char[] trimchars = new char[] { '\\', '/' };

            rootdir = rootdir.TrimEnd(trimchars);

            // May be a better way to do this? Not sure. Need to consult the USTN way.
            // Dim AppDataDir As String = System.Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%")
            //string ProgDataDir = ExpandEnvironmentVariables("%PROGRAMDATA%");
            string AppDataDir = ExpandEnvironmentVariables(EnvVariable);

            // If AppDataDir.Equals("%LOCALAPPDATA%") Then
            // AppDataDir = System.Environment.ExpandEnvironmentVariables("%APPDATA%")
            // AppDataDir = System.Environment.ExpandEnvironmentVariables("%PROGRAMDATA%")
            // End If

            // AppDataDir = AppDataDir & "\Bentley\" & appname & "\"
            //ProgDataDir = ProgDataDir + @"\Bentley\" + appname + @"\";
            AppDataDir = Path.Combine(AppDataDir, "Bentley", appname);

            try
            {
                foreach (var d in Directory.GetDirectories(AppDataDir))
                {
                    foreach (var f in Directory.GetFiles(d, "DirectoryExplanation.txt"))
                    {
                        string text = File.ReadAllText(f);
                        int index = text.IndexOf(rootdir);
                        if (index >= 0)
                            return Path.Join(d, Path.DirectorySeparatorChar.ToString());
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] UtilitiesPath.{MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                // Pass
            }

            return "";
        }
        public static string GetPrefsDir(string appname, string rootdir)
        {
            char[] trimchars = new char[] { '\\', '/' };

            rootdir = rootdir.TrimEnd(trimchars);

            // May be a better way to do this? Not sure. Need to consult the USTN way.
            // Dim AppDataDir As String = System.Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%")
            //string ProgDataDir = ExpandEnvironmentVariables("%PROGRAMDATA%");
            string AppDataDir = ExpandEnvironmentVariables("%LOCALAPPDATA%");

            // If AppDataDir.Equals("%LOCALAPPDATA%") Then
            // AppDataDir = System.Environment.ExpandEnvironmentVariables("%APPDATA%")
            // AppDataDir = System.Environment.ExpandEnvironmentVariables("%PROGRAMDATA%")
            // End If

            // AppDataDir = AppDataDir & "\Bentley\" & appname & "\"
            //ProgDataDir = ProgDataDir + @"\Bentley\" + appname + @"\";
            AppDataDir = Path.Combine(AppDataDir, "Bentley", appname);

            try
            {
                foreach (var d in Directory.GetDirectories(AppDataDir))
                {
                    foreach (var f in Directory.GetFiles(d, "DirectoryExplanation.txt"))
                    {
                        string text = File.ReadAllText(f);
                        int index = text.IndexOf(rootdir);
                        if (index >= 0)
                            return Path.Join(d, Path.DirectorySeparatorChar.ToString());
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] UtilitiesPath.{MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                // Pass
            }

            return "";
        }

        // Get the path to %LOCALAPPDATA%
        public static string GetUserAppDataPath()
        {
            return GetFolderPath(SpecialFolder.LocalApplicationData);
        }

        public static string GetUserRoamingAppDataPath()
        {
            return GetFolderPath(SpecialFolder.ApplicationData);
        }

        public static string GetUserSubDirectory(string basePath, string app, string searchPattern, string fallback)
        {
            string directoryPath = Path.Combine(basePath, "Bentley", app);

            // Get all subdirectories
            DirectoryInfo[] subdirectories = new DirectoryInfo(directoryPath).GetDirectories(searchPattern);

            // Filter and sort subdirectories by version number
            var highestVersionSubdirectory = subdirectories.Where(d => Utilities.IsVersionNumber(d.Name)).OrderByDescending(d => Utilities.ParseVersionNumber(d.Name)).FirstOrDefault();



            // Check if a subdirectory was found
            if (highestVersionSubdirectory is not null)
            {
                return Path.Join(highestVersionSubdirectory.FullName, Path.DirectorySeparatorChar.ToString());
            }
            else
            {
                return Path.Join(directoryPath, fallback, Path.DirectorySeparatorChar.ToString());
            }
        }

        public static string GetUserTempPath()
        {
            return Path.GetTempPath();
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns true if the input string appears to be a path
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static bool IsPath(ref string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 3)
                return false;

            // Check for drive letter path (e.g., "C:\")
            if (char.IsLetter(value[0]) && value[1] == ':' && (value[2] == '\\' || value[2] == '/'))
                return true;

            // Check for UNC path (e.g., "\\server\share")
            if (value.StartsWith(@"\\", StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns true if the input string appears to be a file
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static bool IsFile(ref string value)
        {
            return IsPath(ref value) && !IsFolder(ref value) && value.Contains(".");
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns true if the input string appears to be a folder
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static bool IsFolder(ref string value)
        {
            if (IsPath(ref value))
            {
                string dirName = GetDirectoryName(ref value).Trim().ToLowerInvariant();
                string valTrim = (value ?? "").Trim().ToLowerInvariant();
                if (CultureInfo.CurrentCulture.CompareInfo.Compare(dirName, valTrim, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                    return true;
            }
            return false;
        }

        public static bool IsValidFileNameOrPath(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            foreach (char badChar in Path.GetInvalidPathChars())
            {
                if (name.IndexOf(badChar) >= 0)
                    return false;
            }

            return true;
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns true if a file exists. The filepath can have a *
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static bool WildCardFilesExist(ref string location)
        {
            string path = GetDirectoryName(ref location);
            string filename = GetFileName(location);

            try
            {
                if (Directory.Exists(path))
                {
                    return new DirectoryInfo(path).GetFiles(filename).Length > 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] UtilitiesPath.{MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }

            return false;
        }

        public static bool CESetup(ref string CEPredefinedCfgsDir, ref string CEVariableCSVsDir)
        {
            // CESetup = False
            string programdataDir = ExpandEnvironmentVariables("%PROGRAMDATA%");

            string dirPath = Path.Combine(programdataDir, "Bentley", "ConfigurationExplorer");
            CEPredefinedCfgsDir = Path.Combine(dirPath, "PredefinedCfgs");
            CEVariableCSVsDir = Path.Combine(dirPath, "VariableCSVs");

            try
            {
                Directory.CreateDirectory(dirPath);
                Directory.CreateDirectory(CEPredefinedCfgsDir);
                Directory.CreateDirectory(CEVariableCSVsDir);
                return true;
            }
            catch (Exception)
            {
                // File/Folder creation failed, show message to user and exit
                string msg;
                msg = string.Format(Utilities.GetResourceString("TXT_ErrCesettingsFailedToCreateCfg"), dirPath);
                object unused = MessageBox.Show(msg, Utilities.GetResourceString("TXT_MsgFileCreateFailed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                Exit(0);
            }
            return false;
        }
    }
}
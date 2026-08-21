// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Microsoft.Win32;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{

    static class Utilities
    {

        [DllImport("GDI32.DLL", CharSet = CharSet.Auto)]
        public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        public static ResourceManager resmgr = new ResourceManager("WorkspaceCFG.CEResource", Assembly.GetExecutingAssembly());

        public static Icon GetApplicationIcon()
        {
            string exePath = Application.ExecutablePath;
            return Icon.ExtractAssociatedIcon(exePath);
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns the location of 'Configuration Explorer.exe'
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static string GetExePath()
        {
            return Environment.ProcessPath ?? Application.ExecutablePath;
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns the folder location of 'Configuration Explorer.exe'
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static string GetExeFolder()
        {
            return AppContext.BaseDirectory;
        }

        public static string GetResourceString(string resourceName)
        {
            string GetResourceStringRet = default;
            GetResourceStringRet = resmgr.GetString(resourceName, CultureInfo.CurrentCulture);
            return GetResourceStringRet;
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns true if the input string appears boolean value; 0 1 true false
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static bool IsBoolean(ref string value)
        {
            string tmp = (value ?? "").Trim().ToLowerInvariant();
            return CultureInfo.CurrentCulture.CompareInfo.Compare(tmp, "0", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 ||
                   CultureInfo.CurrentCulture.CompareInfo.Compare(tmp, "1", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 ||
                   CultureInfo.CurrentCulture.CompareInfo.Compare(tmp, "true", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 ||
                   CultureInfo.CurrentCulture.CompareInfo.Compare(tmp, "false", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0;
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns the apparent value type of a string (boolean, file, folder)
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static string ValueType(string value)
        {
            if (UtilitiesPath.IsPath(ref value))
            {
                string dirName = (UtilitiesPath.GetDirectoryName(ref value) ?? "").Trim().ToLowerInvariant();
                string valTrim = (value ?? "").Trim().ToLowerInvariant();
                if (CultureInfo.CurrentCulture.CompareInfo.Compare(dirName, valTrim, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                {
                    return "folder";
                }
                else
                {
                    return "file";
                }
            }
            if (IsBoolean(ref value))
                return "boolean";
            return "";
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns the parentdevdir of a string
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static string ParentDevDir(string path)
        {
            string[] mis;
            string temp;
            string test;
            int t;
            test = UtilitiesPath.GetDirectoryName(ref path);

            if (!path.EndsWith(@"\"))
                path += @"\";

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(path ?? "", test ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
                return "";

            if (path.Length >= 2 && path.Substring(0, 2) == @"\\")
            {
                path = "//" + path.Substring(2);
            }

            mis = (path ?? "").Split(new[] { '\\' }, StringSplitOptions.None);

            temp = "";
            if (mis.Length > 2)
            {
                var loopTo = mis.Length - 2;
                for (t = 0; t <= loopTo; t++)
                {
                    if (!string.IsNullOrEmpty(mis[t]?.Trim()))
                    {
                        temp = temp + mis[t] + @"\";
                    }
                }
            }

            return temp.Replace("//", @"\\");
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns a variable's long description from the variable database
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static string GetVariableLongDescription(string Varname)
        {
            VarDB.EnsureLoaded();
            // These are LINQ or lamda expressions
            // v is similar to the i in a for loop for the current VBEntry being checked
            // => is kinda like WHERE
            if (VarDB.VariableDB.Any(v => v.Name == Varname)) //check if any have name = Varname
                return VarDB.VariableDB.Find(v => v.Name == Varname).LongDesc; //find where name = Varname and return LongDesc
            return "";
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns a variable's category from the variable database
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static string GetVariableCategory(string Varname)
        {
            VarDB.EnsureLoaded();
            if (VarDB.VariableDB.Any(v => v.Name == Varname)) //check if any have name = Varname
                return VarDB.VariableDB.Find(v => v.Name == Varname).Category;
            return "";
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns a variable's comment from the variable database
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static string GetVariableComment(string Varname)
        {
            VarDB.EnsureLoaded();
            if (VarDB.VariableDB.Any(v => v.Name == Varname)) //check if any have name = Varname
                return VarDB.VariableDB.Find(v => v.Name == Varname).Comment;
            return "";
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns a variable's application from the variable database
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static string GetVariableApplication(string Varname)
        {
            VarDB.EnsureLoaded();
            if (VarDB.VariableDB.Any(v => v.Name == Varname)) //check if any have name = Varname
                return VarDB.VariableDB.Find(v => v.Name == Varname).App;
            return "";
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Opens windows explorer to the containing folder of a file path
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void OpenContainingFolder(ref string filename)
        {
            try
            {
                Process.Start("explorer.exe", UtilitiesPath.GetDirectoryName(ref filename));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] Utilities.{MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Opens a file in notepad++ or else the system's default text editor
        // 'Setting warn = true will warn the user if the file isn't found
        // 'Line will attempt to open the file at a specific line number
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void OpenInEditor(ref string Filename, bool warn = true, int line = 0)
        {
            try
            {
                if (File.Exists(Filename) == false)
                {
                    if (warn)
                        MessageBox.Show(string.Format(CEResource.TXT_MsgFileNotFound, Filename), CEResource.TXT_MsgFileNotFound);
                    return;
                }

                try
                {
                    Process.Start("notepad++.exe", $"-n{line.ToString().Trim()} \"{Filename}\"");
                    System.Threading.Thread.Sleep(200);
                }
                catch
                {
                    Process.Start(Filename);
                    System.Threading.Thread.Sleep(200);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] Utilities.{MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns the level name given its number
        // ---------------+---------------+---------------+---------------+---------------+------

        // CONNECT Version
        public static string GetLevelExtendedName(int level)
        {
            switch (level)
            {
                case -2:
                    {
                        return "-2 Predefined";
                    }
                case -1:
                    {
                        return "-1 Windows System Environment";
                    }
                case 0:
                    {
                        return "0 System";
                    }
                case 1:
                    {
                        return "1 Application";
                    }
                case 2:
                    {
                        return "2 Organization";
                    }
                case 3:
                    {
                        return "3 WorkSpace";
                    }
                case 4:
                    {
                        return "4 WorkSet";
                    }
                case 5:
                    {
                        return "5 Role";
                    }
                case 6:
                    {
                        return "6 User";
                    }

                default:
                    {
                        return "Unknown";
                    }
            }
        }

        public static string GetLevelName(int level)
        {
            switch (level)
            {
                case -2:
                    {
                        return "Predefined";
                    }
                case -1:
                    {
                        return "Windows System Environment";
                    }
                case 0:
                    {
                        return "System";
                    }
                case 1:
                    {
                        return "Application";
                    }
                case 2:
                    {
                        return "Organization";
                    }
                case 3:
                    {
                        return "WorkSpace";
                    }
                case 4:
                    {
                        return "WorkSet";
                    }
                case 5:
                    {
                        return "Role";
                    }
                case 6:
                    {
                        return "User";
                    }

                default:
                    {
                        return "Unknown";
                    }
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns a level number given its name
        // ---------------+---------------+---------------+---------------+---------------+------

        // CONNECT Version
        public static int GetLevelNumber(string name)
        {
            string lname = (name ?? "").ToLowerInvariant();
            switch (lname)
            {
                case var @case when CultureInfo.CurrentCulture.CompareInfo.Compare(@case, "system", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                    return 0;
                case var case1 when CultureInfo.CurrentCulture.CompareInfo.Compare(case1, "application", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                    return 1;
                case var case2 when CultureInfo.CurrentCulture.CompareInfo.Compare(case2, "organization", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                    return 2;
                case var case3 when CultureInfo.CurrentCulture.CompareInfo.Compare(case3, "workspace", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                    return 3;
                case var case4 when CultureInfo.CurrentCulture.CompareInfo.Compare(case4, "workset", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                    return 4;
                case var case5 when CultureInfo.CurrentCulture.CompareInfo.Compare(case5, "role", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                    return 5;
                case var case6 when CultureInfo.CurrentCulture.CompareInfo.Compare(case6, "user", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                    return 6;
                default:
                    return -1;
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns a bitmap object of a control
        // 
        // ---------------+---------------+---------------+---------------+---------------+-------
        private static Bitmap GetControlImage(object frm)
        {
            // Get this form's Graphics object.
            Graphics me_gr = (Graphics)((dynamic)frm).CreateGraphics();

            // See how big to make the result bitmap.
            int wid, hgt;

            wid = (int)((dynamic)frm).ClientSize.Width;
            hgt = (int)((dynamic)frm).ClientSize.Height;


            // Make a Bitmap to hold the image.
            var bm = new Bitmap(wid, hgt, me_gr);
            Graphics bm_gr;
            bm_gr = Graphics.FromImage(bm);
            var bm_hdc = bm_gr.GetHdc();

            // Get the form's hDC. We must do this after
            // creating the new Bitmap, which uses me_gr.
            IntPtr me_hdc;
            me_hdc = me_gr.GetHdc();

            // BitBlt the form's image onto the Bitmap.
            BitBlt(bm_hdc, 0, 0, wid, hgt, me_hdc, 0, 0, 13369376);

            // Clean up.
            bm_gr.ReleaseHdc(bm_hdc);
            me_gr.ReleaseHdc(me_hdc);

            // Return the result.
            return bm;
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Saves a picture of a form control to file
        // 
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void SaveControlImage(ref object frm)
        {
            Bitmap bm;
            string path;
            var savefiledialog = new System.Windows.Forms.SaveFileDialog();
            var dp = new System.Drawing.Imaging.EncoderParameters(1);
            System.Drawing.Imaging.ImageCodecInfo[] arrayICI = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            System.Drawing.Imaging.ImageCodecInfo jpegICI = null;
            int x;
            var loopTo = arrayICI.Length - 1;
            for (x = 0; x <= loopTo; x++)
            {
                if (arrayICI[x].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[x];
                    break;
                }
            }

            dp.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);

            bm = GetControlImage(frm);

            savefiledialog.Title = CEResource.TXT_MsgSaveVariableListToImage;
            savefiledialog.InitialDirectory = CEResource.TXT_CDrive;
            savefiledialog.Filter = CEResource.TXT_FileFilterJPEG;
            savefiledialog.FileName = CEResource.TXT_UntitledJpg;
            savefiledialog.RestoreDirectory = true;
            if (savefiledialog.ShowDialog() == DialogResult.Cancel)
                return;

            path = savefiledialog.FileName;
            savefiledialog.Dispose();

            bm.Save(path, jpegICI, dp);

            MessageBox.Show(CEResource.TXT_MsgFileExportComplete, CEResource.TXT_MsgDone);

            bm.Dispose();
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Saves the list of cfg files to a text file in three different layouts
        // 
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void ExportFileList(ref CFGConfiguration workspace, string ftype = "text", bool full = true)
        {
            if (workspace is null)
                return;

            CFGFile cfile;
            var savefiledialog = new System.Windows.Forms.SaveFileDialog();
            string path;

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(ftype, "text", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                savefiledialog.Title = CEResource.TXT_ExportVariableListToTextFile;
                savefiledialog.InitialDirectory = CEResource.TXT_CDrive;
                savefiledialog.Filter = CEResource.TXT_FileFilterTxt;
                savefiledialog.FileName = workspace.ApplicationData.Name + CEResource.TXT_FilelistTxt;
                savefiledialog.RestoreDirectory = true;
                if (savefiledialog.ShowDialog() == DialogResult.Cancel)
                    return;

                path = savefiledialog.FileName;
                savefiledialog.Dispose();
            }
            else
            {
                savefiledialog.Title = CEResource.TXT_ExportVariableListToTextFile;
                savefiledialog.InitialDirectory = CEResource.TXT_CDrive;
                savefiledialog.Filter = CEResource.TXT_FileFilterCsv;
                savefiledialog.FileName = workspace.ApplicationData.Name + CEResource.TXT_FilelistCsv;
                savefiledialog.RestoreDirectory = true;
                if (savefiledialog.ShowDialog() == DialogResult.Cancel)
                    return;

                path = savefiledialog.FileName;
                savefiledialog.Dispose();
            }

            using var streamWriter = new StreamWriter(path);
            if (full)
            {
                if (CultureInfo.CurrentCulture.CompareInfo.Compare(ftype, "text", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                {
                    foreach (CFGFile currentCfile in workspace.CFGFiles)
                    {
                        cfile = currentCfile;
                        streamWriter.WriteLine(new string('\t', cfile.Depth) + cfile.Name);
                    }
                }
                else
                {
                    foreach (CFGFile currentCfile1 in workspace.CFGFiles)
                    {
                        cfile = currentCfile1;
                        streamWriter.WriteLine(new string(',', cfile.Depth) + cfile.Name);
                    }
                }

                streamWriter.WriteLine();
                streamWriter.WriteLine();
            }

            foreach (CFGFile currentCfile2 in workspace.CFGFiles)
            {
                cfile = currentCfile2;
                streamWriter.WriteLine(cfile.Name);
            }

            if (full)
            {
                streamWriter.WriteLine();
                streamWriter.WriteLine();

                foreach (CFGFile currentCfile3 in workspace.CFGFiles)
                {
                    cfile = currentCfile3;
                    streamWriter.WriteLine(cfile.FilePath);
                }
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns the first line of a text file
        // 
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static string GetVersionString(string path)
        {
            string version;

            try
            {
                if (File.Exists(path) == false)
                    return "";

                using (var reader = new StreamReader(path))
                {
                    version = (reader.ReadLine() ?? "").Trim().ToLowerInvariant();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] Utilities.{MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                version = "";
            }

            return version;
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns a cleanup up variable value, removes extra semicolons and fixes backslash/forwardslash
        // 
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static string CleanupMultiVariableValue(string input)
        {
            // Removes unneeded semicolons from a variable value
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var parts = input.Split(';');
            var cleanedParts = new List<string>();

            foreach (var part in parts)
            {
                var trimmed = part.Trim();

                if (trimmed.Length > 2)
                {
                    var lower = trimmed.ToLower();
                    if (!lower.StartsWith("file:") && !lower.StartsWith("pw:"))
                    {
                        // Replace double backslashes with single backslash starting from index 2
                        trimmed = trimmed.Substring(0, 2) + trimmed.Substring(2).Replace(@"\\", @"\");
                    }
                }

                if (!string.IsNullOrEmpty(trimmed))
                {
                    cleanedParts.Add(trimmed);
                }
            }

            var result = string.Join(";", cleanedParts);

            // Remove leading semicolon if present
            if (result.StartsWith(";"))
                result = result.Substring(1);

            return result;
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Builds a Vertical from an LNK file
        // 
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static CFGEnums.Application GetApplicationFromLNK(string filepath)
        {
            CFGEnums.Application vert;
            Regex regCFGPATH;

            string[] mis;
            string code;
            MatchCollection mtchs;
            string arguments;

            if (!filepath.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase))
                return default;
            if (!File.Exists(filepath))
                return default;

            vert = new CFGEnums.Application();

            var theShortcut = new ShortcutInfo(filepath);

            regCFGPATH = new Regex("(?i:-wc)(?<CFGPATH>\"[^\"]*\")");

            // don't process if not an exe file
            if (!theShortcut.TargetPath.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                return vert;

            vert.Name = Path.GetFileNameWithoutExtension(theShortcut.ShortCutPath);
            vert.EXEPath = theShortcut.TargetPath;

            arguments = theShortcut.Arguments;

            mtchs = regCFGPATH.Matches(arguments);
            foreach (Match mtch in mtchs)
            {
                var groupValue = mtch.Groups["CFGPATH"].Value;
                if (!string.IsNullOrEmpty(groupValue.Trim()))
                {
                    vert.StartupCfgPath = groupValue.Replace("\"", "").Trim();
                    var midTmp = new string(' ', groupValue.Length + 3);
                    int startIndex = mtch.Groups["CFGPATH"].Index - 2;
                    if (startIndex >= 0 && startIndex + midTmp.Length <= arguments.Length)
                    {
                        arguments = arguments.Substring(0, startIndex) + midTmp + arguments.Substring(startIndex + midTmp.Length);
                    }
                }
            }

            mis = (arguments ?? "").Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in mis)
            {
                if (item.Length > 2)
                {
                    code = item.Substring(0, 2).ToLowerInvariant();
                    switch (code ?? "")
                    {
                        case var @case when CultureInfo.CurrentCulture.CompareInfo.Compare(@case, "wc", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                            vert.StartupCfgPath = item.Substring(2).Replace("\"", "");
                            break;
                        case var case1 when CultureInfo.CurrentCulture.CompareInfo.Compare(case1, "wu", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                            vert.Workspace = item.Substring(2).Replace("\"", "");
                            break;
                        case var case2 when CultureInfo.CurrentCulture.CompareInfo.Compare(case2, "wp", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0:
                            vert.Workset = item.Substring(2).Replace("\"", "");
                            break;
                        default:
                            if (!string.IsNullOrEmpty(item.Trim()))
                                vert.CommandOptions = vert.CommandOptions + "-" + item;
                            break;
                    }
                }
            }

            mis = (theShortcut.IconLocation ?? "").Split(new[] { ',' }, StringSplitOptions.None);
            if (mis.Length > 0)
                vert.Iconfile = mis[0];
            if (mis.Length > 1 && int.TryParse(mis[1], out int iconNum))
                vert.IconNumber = iconNum;

            // We have decided not to use Predefined CFG files because these files are hardcoded and all variables in these files are reassigned in ms*.cfg files
            // Guess which predefined file to use
            // If LCase(vert.Name) Like "*v8i*" OrElse LCase(vert.EXEPath) Like "*v8i*" OrElse LCase(vert.Iconfile) Like "*v8i*" OrElse LCase(vert.Path) Like "*v8i*" Then
            // vert.PredefinedPath = "PREDEFINED V8I.cfg"
            // Else
            // vert.PredefinedPath = "PREDEFINED XM.cfg"
            // If IS64Bit() Then vert.PredefinedPath = "x64 " & vert.PredefinedPath
            // End If

            // If no configuration path is specified then attempt to use mslocal.cfg
            if (string.IsNullOrEmpty((vert.StartupCfgPath ?? "").Trim()) && !string.IsNullOrEmpty(vert.EXEPath))
            {
                vert.StartupCfgPath = UtilitiesPath.GetDirectoryName(ref vert.EXEPath) + @"config\mslocal.cfg";
            }
            var AppNameFileVersionMap = new string[3, 501];
            var AppNameFileVersionCount = default(int);
            GetListOfInstalledProductsWithVersionNumbers(ref AppNameFileVersionMap, ref AppNameFileVersionCount);
            for (int x = 0, loopTo = AppNameFileVersionCount; x <= loopTo; x++)
            {
                if (CultureInfo.CurrentCulture.CompareInfo.Compare(vert.EXEPath ?? "", AppNameFileVersionMap[1, x] ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                {
                    vert.Version = AppNameFileVersionMap[2, x];
                    if (!string.IsNullOrEmpty(vert.Version))
                    {
                        int dotIdx = vert.Version.IndexOf(".");
                        int majorversion = dotIdx > 0 ? int.Parse(vert.Version.Substring(0, dotIdx)) : 0;
                    }
                    break;
                }
            }

            return vert;
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Returns true if running under a 64bit operating system
        // 
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static bool Is64Bit()
        {
            return Environment.Is64BitOperatingSystem;
        }

        public static void OpenHelpPDF()
        {
            string filename;

            // filename = GetExeFolder() & "Help\Guide.pdf"
            filename = GetExeFolder() + @"Documentation\BentleyConfigurationExplorerHelp.pdf";

            if (File.Exists(filename) == false)
                return;

            try
            {
                Process.Start(filename);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] Utilities.{MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
        }

        public static void OpenSystemInfo()
        {
            Process.Start("msinfo32.exe");
        }

        // ---------------------------------------------------------------------------------------
        // @description: 'Overloaded function returns a ready to process workspace object based on various inputs
        // 
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static CFGConfiguration GetConfiguration(ref CFGEnums.Application application, ref string workspace_str, ref string workset_str, ref string role_str)
        {
            var SelectedConfiguration = new CFGConfiguration();

            string wrArg = string.Empty;
            GetWRCommandLineArgument(ref wrArg);
            if (!string.IsNullOrEmpty(wrArg) && !string.IsNullOrEmpty(application.Name))
            {
                application.CommandOptions += " " + wrArg;
            }

            // Populate configuration data
            var appData = SelectedConfiguration.ApplicationData;
            appData.Workspace = workspace_str;
            appData.Workset = workset_str;
            appData.Role = role_str;
            appData.Name = application.Name;
            appData.StartupCfgPath = application.StartupCfgPath;
            appData.Version = application.Version;
            appData.PredefinedPath = application.PredefinedPath;
            appData.CommandOptions = application.CommandOptions;
            appData.ConfigurationRoot = application.ConfigurationRoot;
            appData.PredefinedCfgNames = application.PredefinedCfgNames;
            appData.VariableDBNames = application.VariableDBNames;
            appData.EXEPath = application.EXEPath;
            appData.Rules = application.Rules;

            SelectedConfiguration.ApplicationData = appData;

            return SelectedConfiguration;
        }

        // Combo Box
        public static CFGConfiguration GetConfiguration(ref ComboBox application_cmb, ref ComboBox workspace_cmb, ref ComboBox workset_cmb, ref ComboBox role_cmb)
        {

            CFGEnums.Application app;

            string exePath = string.Empty;
            CFGEnums.ShortApplication sv = (CFGEnums.ShortApplication)application_cmb.SelectedItem;
            if (sv is not null)
                exePath = sv.ExePath;

            app = MainType.CESettings.GetApplication(application_cmb.Text.Trim(), exePath);
            if (string.IsNullOrEmpty(app.Name))
                return null;

            string configurationArg = string.Empty;
            GetWRCommandLineArgument(ref configurationArg);
            if (!string.IsNullOrEmpty(configurationArg))
            {
                app.CommandOptions += " " + configurationArg;
            }

            string argworkspace_str = workspace_cmb.Text;
            string argworkset_str = workset_cmb.Text;
            string argrole_str = role_cmb.Text;
            return GetConfiguration(ref app, ref argworkspace_str, ref argworkset_str, ref argrole_str);
            //workspace_cmb.Text = argworkspace_str;
            //workset_cmb.Text = argworkset_str;
            //role_cmb.Text = argrole_str;
        }

        public static CFGConfiguration GetConfiguration(ref string application_str, ref string exePath, ref string workspace_str, ref string workset_str, ref string role_str)
        {
            CFGEnums.Application app;

            app = MainType.CESettings.GetApplication(application_str, exePath);
            if (string.IsNullOrEmpty(app.Name))
                return null;

            string wrArg = string.Empty;
            GetWRCommandLineArgument(ref wrArg);
            if (!string.IsNullOrEmpty(wrArg))
            {
                app.CommandOptions += " " + wrArg;
            }

            return GetConfiguration(ref app, ref workspace_str, ref workset_str, ref role_str);
        }

        // TODO: somewhat overlap: GetBentleyApps.GetBentleyApps and Utilities.GetListOfInstalledProductsWithVersionNumbers and InstalledApplication.BuildBentleyApps
        // ---------------------------------------------------------------------------------------
        // @description: Returns a list of all Bentley products installed on the machine
        // 
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static string GetBentleyApps()
        {
            string uninstallkey;
            RegistryKey rk;
            RegistryKey sk;
            string[] skname;
            string name;
            string publisher;
            string version;
            string result;

            uninstallkey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

            rk = Registry.LocalMachine.OpenSubKey(uninstallkey);
            skname = rk.GetSubKeyNames();
            result = "";
            for (int counter = 0, loopTo = skname.Length - 1; counter <= loopTo; counter++)
            {
                sk = rk.OpenSubKey(skname[counter]);

                name = sk.GetValue("DisplayName")?.ToString();
                version = sk.GetValue("DisplayVersion")?.ToString();
                publisher = sk.GetValue("Publisher")?.ToString();
                if ((!string.IsNullOrEmpty(publisher) && publisher.IndexOf("Bentley", StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrEmpty(name) && (name.IndexOf("Bentley", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                                     name.IndexOf("MicroStation", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                                     name.IndexOf("ProjectWise", StringComparison.OrdinalIgnoreCase) >= 0)))
                {
                    result = result + name + '\t' + version + '\n';
                }
            }

            // TODO: FIXME any use of IS64Bit() should be checked or removed (see SetProgramFiles for an example)
            if (Is64Bit())
            {
                uninstallkey = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

                rk = Registry.LocalMachine.OpenSubKey(uninstallkey);
                skname = rk.GetSubKeyNames();
                for (int counter = 0, loopTo1 = skname.Length - 1; counter <= loopTo1; counter++)
                {
                    sk = rk.OpenSubKey(skname[counter]);

                    name = sk.GetValue("DisplayName")?.ToString();
                    version = sk.GetValue("DisplayVersion")?.ToString();
                    publisher = sk.GetValue("Publisher")?.ToString();
                    if ((!string.IsNullOrEmpty(publisher) && publisher.IndexOf("Bentley", StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrEmpty(name) && (name.IndexOf("Bentley", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                                         name.IndexOf("MicroStation", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                                         name.IndexOf("ProjectWise", StringComparison.OrdinalIgnoreCase) >= 0)))
                    {
                        result = result + name + '\t' + version + '\n';
                    }
                }
            }

            return result;
        }

        public static int GetApplicationIndex(ref ComboBox vert_cmb, ref string name, ref string exePath)
        {
            int index;
            CFGEnums.ShortApplication sv;
            var loopTo = vert_cmb.Items.Count - 1;
            for (index = 0; index <= loopTo; index += 1)
            {
                sv = (CFGEnums.ShortApplication)vert_cmb.Items[index];
                if (CultureInfo.CurrentCulture.CompareInfo.Compare(sv.Name ?? "", name ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 && CultureInfo.CurrentCulture.CompareInfo.Compare(sv.ExePath ?? "", exePath ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                    return index;
            }
            return -1;
        }

        // ---------------------------------------------------------------------------------------
        // @description: split a string using regex
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static string[] SplitString(string expression, string delimiter, char qualifier, bool ignoreCase)
        {
            string statement = string.Format(
                "{0}(?=(?:[^{1}]*{1}[^{1}]*{1})*(?![^{1}]*{1}))",
                Regex.Escape(delimiter),
                Regex.Escape(qualifier.ToString())
);

            var options = RegexOptions.Compiled | RegexOptions.Multiline;
            if (ignoreCase)
            {
                options = options | RegexOptions.IgnoreCase;
            }

            var regexExp = new Regex(statement, options);
            return regexExp.Split(expression);
        }

        // ---------------------------------------------------------------------------------------
        // @description: Extracts workspace path argument from BCE arguments list
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void GetWRCommandLineArgument(ref string wrArg)
        {
            string[] args = Environment.GetCommandLineArgs();
            foreach (string cmd in args)
            {
                if (cmd.Length > 5)
                {
                    string lowerTrimmed = cmd.Trim().ToLowerInvariant();
                    if (lowerTrimmed.StartsWith("-wr", StringComparison.Ordinal))
                    {
                        wrArg = cmd;
                        return;
                    }
                }
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Validate if the user set as the cmbUser.Text is still valid
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void ValidateSelectedWorkset(ref ComboBox cmbWorksets, bool worksetLocked)
        {
            foreach (var item in cmbWorksets.Items)
            {
                string workset = item?.ToString() ?? string.Empty;
                string selectedText = cmbWorksets.Text ?? string.Empty;

                if (string.Equals(selectedText, workset, CompareOptionsToString()))
                {
                    if (!string.Equals(workset, "No Workset", CompareOptionsToString()) || worksetLocked)
                    {
                        return;
                    }
                }
            }

            string currentWorkset = string.Empty;

            if (cmbWorksets.Items.Count > 1)
            {
                currentWorkset = cmbWorksets.Items[1]?.ToString() ?? string.Empty;
            }
            else if (cmbWorksets.Items.Count > 0)
            {
                currentWorkset = cmbWorksets.Items[0]?.ToString() ?? string.Empty;
            }

            cmbWorksets.Text = currentWorkset;
        }

        private static StringComparison CompareOptionsToString()
        {
            // You can adjust this if you want to mimic IgnoreKanaType and IgnoreWidth more closely
            return StringComparison.CurrentCultureIgnoreCase;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Validate if the Role set as the cmbRole.Text is still valid
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void ValidateSelectedRole(ref ComboBox cmbRoles, bool roleLocked)
        {
            foreach (var item in cmbRoles.Items)
            {
                string role = item?.ToString() ?? string.Empty;
                string selectedText = cmbRoles.Text ?? string.Empty;

                if (string.Equals(selectedText, role, StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!string.Equals(role, "No Role", StringComparison.CurrentCultureIgnoreCase) || roleLocked)
                    {
                        return;
                    }
                }
            }

            string currentRole = string.Empty;

            if (cmbRoles.Items.Count > 1)
            {
                currentRole = cmbRoles.Items[1]?.ToString() ?? string.Empty;
            }
            else if (cmbRoles.Items.Count > 0)
            {
                currentRole = cmbRoles.Items[0]?.ToString() ?? string.Empty;
            }

            cmbRoles.Text = currentRole;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Returns a Bentley Configuration Explorer Version Number
        // 
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static string GetVersionNumber()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;

            return $"{version.Major}.{version.Minor}.{version.Build}"; // was Return "1.0"
        }

        // TODO: somewhat overlaps: GetBentleyApps.GetBentleyApps and Utilities.GetListOfInstalledProductsWithVersionNumbers and InstalledApplication.BuildBentleyApps
        public static void GetListOfInstalledProductsWithVersionNumbers(ref string[,] appNameFileVersionMap, ref int appNameFileVersionCount)
        {
            string[] registryPaths = { @"SOFTWARE\Wow6432Node\Bentley", @"SOFTWARE\Bentley" };
            int index = 0;

            foreach (string basePath in registryPaths)
            {
                using (RegistryKey baseKey = Registry.LocalMachine.OpenSubKey(basePath))
                {
                    if (baseKey == null) continue;

                    foreach (string subKeyName in baseKey.GetSubKeyNames())
                    {
                        string subKeyPath = Path.Combine(basePath, subKeyName);
                        using (RegistryKey subKey = Registry.LocalMachine.OpenSubKey(subKeyPath))
                        {
                            if (subKey == null) continue;

                            foreach (string subAppKeyName in subKey.GetSubKeyNames())
                            {
                                string fullKeyPath = $@"HKEY_LOCAL_MACHINE\{subKeyPath}\{subAppKeyName}";
                                string appPath = Registry.GetValue(fullKeyPath, "ApplicationPath", null)?.ToString()
                                                 ?? Registry.GetValue(fullKeyPath, "DisplayIcon", "")?.ToString();

                                if (string.IsNullOrEmpty(appPath)) continue;

                                string extension = Path.GetExtension(appPath);
                                if (!string.Equals(extension, ".exe", StringComparison.OrdinalIgnoreCase)) continue;

                                string displayName = Registry.GetValue(fullKeyPath, "DisplayProductName", null)?.ToString()
                                                     ?? Registry.GetValue(fullKeyPath, "ProductName", "")?.ToString();

                                string displayVersion = Registry.GetValue(fullKeyPath, "DisplayVersion", "")?.ToString();
                                string version = Registry.GetValue(fullKeyPath, "Version", "")?.ToString();

                                appNameFileVersionMap[0, index] = displayName ?? "Unknown";
                                appNameFileVersionMap[1, index] = appPath;

                                appNameFileVersionMap[2, index] = !string.IsNullOrEmpty(displayVersion)
                                    ? displayVersion
                                    : !string.IsNullOrEmpty(version)
                                        ? version
                                        : "No Version Found";

                                appNameFileVersionCount++;
                                index++;
                            }
                        }
                    }
                }
            }
        }

        // Check if string is in a version number format of digits and dots
        public static bool IsVersionNumber(string name)
        {
            return name.All(c => char.IsDigit(c) || c == '.');
        }

        public static Version ParseVersionNumber(string name)
        {
            string[] parts = name.Split('.');
            int[] versionParts = parts.Select(s => int.Parse(s)).ToArray();

            return new Version(versionParts[0], versionParts[1], versionParts[2]);
        }

        /*
        public static Bitmap GetControlScreenShot(Control control)
        {
            var controlImage = new Bitmap(control.Width, control.Height);

            using (var g = Graphics.FromImage(controlImage))
            {
                g.CopyFromScreen(control.PointToScreen(new Point(0, 0)), new Point(0, 0), new Size(control.Width, control.Height));
            }

            return controlImage;
        }
        */

        public static void SaveBitmapToFile(ref Bitmap bitmap)
        {
            string path;
            var savefiledialog = new System.Windows.Forms.SaveFileDialog();

            savefiledialog.Title = CEResource.TXT_MsgSaveFileTreeToImage;
            savefiledialog.InitialDirectory = CEResource.TXT_CDrive;
            savefiledialog.Filter = CEResource.TXT_FileFilterPNG;
            savefiledialog.FileName = CEResource.TXT_UntitledPng;
            savefiledialog.RestoreDirectory = true;
            if (savefiledialog.ShowDialog() == DialogResult.Cancel)
                return;

            path = savefiledialog.FileName;
            savefiledialog.Dispose();

            bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Png);
        }

    }
}
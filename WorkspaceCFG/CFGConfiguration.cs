// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{

    [Serializable()]
    [JsonObject(MemberSerialization.Fields)]
    public class CFGConfiguration
    {

        // ---------------------------------------------------------------------------------------
        // Private Members
        // ---------------+---------------+---------------+---------------+---------------+-------
        private int _passCount;

        // ---------------------------------------------------------------------------------------
        // Public Members
        // ---------------+---------------+---------------+---------------+---------------+-------
        public CFGEnums.Application ApplicationData;             // Collection of data needed to process a specific workspace
        //public Collection Variables;          // Collection of variable objects created during processing
        public Dictionary<string, CFGVariable> Variables = new Dictionary<string, CFGVariable>(StringComparer.OrdinalIgnoreCase);
        public ArrayList CFGFiles;            // Arraylist of CFG_File objects created during processing
        public ArrayList CFGEvents;           // Arraylist of CFGEvents created during processing
        public DateTime DateProcessed;            // Date and time the workspace was processed
        public string Description;            // Name + User + Project
        public string Description2;           // Name + User + Project + DateProcessed
        public int ErrorCount;            // Number of errors found
        public int WarningCount;          // Number of warnings found
        public bool AbortProcessing;       // Flag to abort processing if a critical error is found
        public bool ShowStatus;            // Show the status on the main form during processing
        public string WatchVariable;          // Setup a watch to return when a variable is defined
        public string WatchForFile;           // Setup a watch to return when a file is processed
        public int WatchLevel;
        public bool IsFromFile;            // Was this workspace object opened from a file
        public bool CloseStatusWhenDone;   // Flag to hide the status window when finished
        public bool DisableEventRecording; // Flag to disable events from being recorded
        public int WatchForLevel;

        public string CEPredefinedCfgsDir;
        public string CEVariableCSVsDir;
        public string CEVariableXMLsDir;

        // ---------------------------------------------------------------------------------------
        // @description: Initializes members
        // ---------------+---------------+---------------+---------------+---------------+-------
        public CFGConfiguration()
        {
            // Initalize variables
            ApplicationData = new CFGEnums.Application();
            DisableEventRecording = false;
            CFGFiles = new ArrayList();
            CFGEvents = new ArrayList();
            AbortProcessing = false;
            WatchVariable = "";
            WatchLevel = 7;
            ResetStatus();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Clear out collections and arrays
        // ---------------+---------------+---------------+---------------+---------------+-------
        ~CFGConfiguration()
        {
            Variables.Clear();
            Variables = null;
            CFGFiles.Clear();
            CFGFiles = null;
            CFGEvents.Clear();
            CFGEvents = null;
        }

        public void DefineVariable(string name, string value, int level, bool needsExpansion, bool isLocked, string eventDescription)
        {
            var argwrk = this;
            var variable = new CFGVariable(argwrk) { Name = name };

            variable.SetValue(value);
            variable.Level = level;
            variable.NeedsSpecialExpansion = needsExpansion;
            variable.IsLocked = isLocked;
            variable.SetFinalExpansion();

            if (Variables.ContainsKey(name))
                Variables.Remove(name);

            Variables.Add(variable.Name,variable);

            if (!string.IsNullOrEmpty(eventDescription))
            {
                var argetype = CFGEnums.CFGEventType.cfgVardef;
                string argdescription = Utilities.GetResourceString(eventDescription);
                CFGLine argline = null;
                CFGFile argfile = null;
                AddEvent(ref argetype, ref argdescription, argline, argfile, variable, variable.Name);
            }

        }

        private void RedefineVariable(string name, string value, int level, bool needsExpansion, bool isLocked, string eventDescription)
        {
            if (Variables.ContainsKey(name))
                Variables.Remove(name);

            DefineVariable(name, value, level, needsExpansion, isLocked, eventDescription);
        }

        public void ReplaceVariablePlaceholders()
        {
            //Debug.WriteLine($"CFGConfiguration: public void ReplaceVariablePlaceholders()");
            foreach (CFGVariable variable in Variables.Values)
                ReplaceVariablePlaceholders(variable);
        }

        public void ReplaceVariablePlaceholders(CFGVariable variable)
        {
            //Debug.WriteLine($"CFGConfiguration: public void ReplaceVariablePlaceholders(CFGVariable <" + variable.Name + ">)");
            if (variable is not null)
            {
                int iteration = 0;
                while (iteration < 2 && variable.Value is not null && variable.Value.Contains("$("))
                {
                    iteration += 1;
                    foreach (CFGVariable parent in Variables.Values)
                    {
                        if (variable.Value is not null && variable.Value.Contains("$(" + parent.Name + ")"))
                        {
                            if (parent.IsDefined)
                            {
                                // TODO: testing
                                variable.SetValue(variable.Value.Replace("$(" + parent.Name + ")", parent.Value));
                            }
                            else
                            {
                                variable.SetValue(variable.Value.Replace("$(" + parent.Name + ")", ""));
                            }
                            variable.FinalExpansion = variable.Value;
                        }
                    }
                }
            }
            //Debug.WriteLine($"CFGConfiguration: public void ReplaceVariablePlaceholders(CFGVariable variable) return: variable.FinalExansion <" + variable.FinalExpansion + ">");
        }

        public string ReplaceVariablePlaceholders(string expression)
        {
            if (expression is not null)
            {
                int iteration = 0;

                while (iteration < 2 && expression.Contains("$("))
                {
                    iteration++;

                    foreach (CFGVariable parent in Variables.Values)
                    {
                        string placeholder = "$(" + parent.Name + ")";
                        if (expression.Contains(placeholder))
                        {
                            expression = expression.Replace(placeholder, parent.Value);
                        }
                    }
                }
            }

            return expression;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Creates variables defined in the commandline
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void SetCommandLineVariables()
        {
            // Create MS_CONFIG variable as MicroStation would
            DefineVariable("MS_CONFIG", ApplicationData.StartupCfgPath, -2, false, true, "TXT_MsgVariableDefinedInCommandline");

            if (!string.IsNullOrEmpty(ApplicationData.ConfigurationRoot))
            {
                DefineVariable("_USTN_CONFIGURATION", ApplicationData.ConfigurationRoot, -2, false, true, "TXT_MsgVariableDefinedInCommandline");
            }

            if (string.IsNullOrEmpty(ApplicationData.CommandOptions))
                return;

            // Split command line arguments
            string[] arguments = Utilities.SplitString(ApplicationData.CommandOptions, " ", '"', true);

            // Trim arguments
            for (int i = 0; i < arguments.Length; i++)
            {
                arguments[i] = arguments[i].Trim();
            }

            // Process each argument
            foreach (string argument in arguments)
            {
                if (argument.Length > 5)
                {
                    string lowerArg = argument.ToLowerInvariant();

                    if (lowerArg.StartsWith("-ws"))
                    {
                        SetCommandLineVariables(argument);
                    }
                    else if (lowerArg.StartsWith("-wr"))
                    {
                        string value = argument.Substring(3).Trim().Trim('"');
                        if (!value.EndsWith("\\"))
                            value += "\\";

                        SetCommandLineVariables($"-ws_USTN_WORKSPACEROOT={value}");
                    }
                }
            }

            // Regex to extract -wr"somepath"
            var regCFGPATH = new Regex(@"(?i:-wr)(?<CFGPATH>""[^""]*"")");
            var matches = regCFGPATH.Matches(ApplicationData.CommandOptions);

            foreach (Match match in matches)
            {
                var cfgPathGroup = match.Groups["CFGPATH"];
                string cfgPath = cfgPathGroup.Value.Replace("\"", "").Trim();

                if (!string.IsNullOrEmpty(cfgPath))
                {
                    SetCommandLineVariables($"-ws_USTN_WORKSPACEROOT={cfgPath}");
                }
            }
        }


        // ---------------------------------------------------------------------------------------
        // @description: Creates a single variable defined in the commandline
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void SetCommandLineVariables(string argument)
        {
            if (string.IsNullOrEmpty(argument) || argument.Length < 4)
                return;

            argument = argument.Substring(3); // VB's Mid(argument, 4) is 1-based; C# is 0-based

            int eq = argument.IndexOf('=');
            if (eq < 1)
                return;

            //string name = argument.Substring(0, eq).Trim().ToUpperInvariant();
            string name = argument.Substring(0, eq).Trim();
            if (string.IsNullOrEmpty(name))
                return;

            string value = argument.Substring(eq + 1).Trim();

            if (!Variables.ContainsKey(name))
            {
                DefineVariable(name, value, -2, false, true, "TXT_MsgVariableDefinedInCommandline");
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Creates User and Project variables
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void SetSelectionVars()
        {
            if (!string.IsNullOrEmpty(ApplicationData.Workspace))
            {
                DefineVariable("_USTN_USERNAME", "Personal", 0, false, false, "TXT_MsgVariableDefinedFromUserSelection");
            }

            // Set _USTN_WORKSPACENAME
            if (!string.IsNullOrEmpty(ApplicationData.Workspace))
            {
                // TODO: maybe move into new RedefineVariable
                if (Variables.ContainsKey("_USTN_WORKSPACENAME"))
                    Variables.Remove("_USTN_WORKSPACENAME");
                if (CultureInfo.CurrentCulture.CompareInfo.Compare(ApplicationData.Workspace, "No Workspace", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
                    DefineVariable("_USTN_WORKSPACENAME", ApplicationData.Workspace, 1, false, false, "");
            }

            // Set _USTN_WORKSETNAME
            if (!string.IsNullOrEmpty(ApplicationData.Workset))
            {
                // TODO: maybe move into new RedefineVariable
                if (Variables.ContainsKey("_USTN_WORKSETNAME"))
                    Variables.Remove("_USTN_WORKSETNAME");
                if (CultureInfo.CurrentCulture.CompareInfo.Compare(ApplicationData.Workset, "No Workset", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
                    RedefineVariable("_USTN_WORKSETNAME", ApplicationData.Workset, 1, false, false, "");
            }

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(ApplicationData.Workset, "No Workset", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
            {
                // Set _USTN_WORKSETROOT to default
                // RedefineVariable("_USTN_WORKSETROOT", "$(_USTN_WORKSETSROOT)$(_USTN_WORKSETNAME)/", 0, False, False, "")

                // Set _USTN_WORKSETCFG to default
                // RedefineVariable("_USTN_WORKSETCFG", "$(_USTN_WORKSETSROOT)$(_USTN_WORKSETNAME).cfg", 6, False, False, "")
            }

            // Set _USTN_WORKSETNAME
            if (!string.IsNullOrEmpty(ApplicationData.Role))
            {
                // TODO: maybe move into new RedefineVariable
                if (Variables.ContainsKey("_USTN_ROLE_NAME"))
                    Variables.Remove("_USTN_ROLE_NAME");
                if (CultureInfo.CurrentCulture.CompareInfo.Compare(ApplicationData.Workset, "No Workset", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
                    RedefineVariable("_USTN_ROLE_NAME", ApplicationData.Role, 1, false, false, "");
            }
        }

        private void Set_USTN_LOCALE()
        {
            // Get locale variables
            var cultureInfo = CultureInfo.CurrentCulture;
            var regionInfo = new RegionInfo(cultureInfo.LCID);

            if (!string.IsNullOrEmpty(ApplicationData.EXEPath))
            {
                // Define _USTN_LOCALE_LANGUAGE
                RedefineVariable("_USTN_LOCALE_LANGUAGE", cultureInfo.TwoLetterISOLanguageName, -1, false, true, "");

                // Define _USTN_LOCALE_COUNTRY
                RedefineVariable("_USTN_LOCALE_COUNTRY", regionInfo.TwoLetterISORegionName, -1, false, true, "");
            }
        }

        private void Set_ROOTDIR()
        {
            if (!string.IsNullOrEmpty(ApplicationData.EXEPath))
            {
                string rootDir = UtilitiesPath.GetDirectoryName(ref ApplicationData.EXEPath);
                RedefineVariable(
                    "_ROOTDIR",
                    rootDir,
                    -1,
                    false,
                    true,
                    "TXT_MsgVariableCreatedFromExeLocation"
                );
            }
        }

        private void Set_WORKDIR()
        {
            if (!string.IsNullOrEmpty(ApplicationData.EXEPath))
            {
                RedefineVariable("_WORKDIR", @"C:\WINDOWS\system32\", -1, false, true, "TXT_MsgVariableCreatedFromExeLocation");
            }
        }

        private void Set_PRODUCT()
        {
            string exePath = ApplicationData.EXEPath;

            if (!string.IsNullOrEmpty(exePath))
            {
                string dirLocation = Path.GetDirectoryName(exePath);
                if (string.IsNullOrEmpty(dirLocation))
                    return;
                Type classType = null;
                System.Reflection.Assembly productAssembly;

                try
                {
                    // Load the assembly from the specified path.
                    productAssembly = System.Reflection.Assembly.LoadFile(
                        Path.Combine(dirLocation, "Bentley.PowerPlatform.FeatureAspects.dll"));
                    classType = productAssembly.GetType("Bentley.PowerPlatform.Resx");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name} 1 : {ex.Message}");
                    // This happens when we are trying to load a 32-bit dll.
                }

                if (classType is null)
                {
                    return;
                }

                SetProductInfo(ref classType);
            }
        }

        public void SetProductInfo(ref Type classType)
        {
            var msv = new CFGEnums.MSVersion();
            object property_value;

            // TODO: This is not returning the proper variable.
            // productConfigVarName - > _USTN_PRODUCT_CONFIGVARNAME
            property_value = GetPropertyInfoValue(ref classType, "ProductNames_ConfigVarName");
            if (property_value is null)
                property_value = "";
            msv.productConfigVarName = property_value.ToString();
            RedefineVariable("_USTN_PRODUCT_CONFIGVARNAME", property_value.ToString(), -1, false, true, "TXT_MsgVariableCreatedFromProductConfigVarName");

            // productDirectoryName - > _USTN_PRODUCT_DIRNAME
            property_value = GetPropertyInfoValue(ref classType, "ProductNames_DirectoryName");
            if (property_value is null)
                property_value = "";
            msv.productDirectoryName = property_value.ToString();
            RedefineVariable("_USTN_PRODUCT_DIRNAME", property_value.ToString(), -1, false, true, "TXT_MsgVariableCreatedFromProductDirectoryName");

            // productFileBaseName - > _USTN_PRODUCT_FILEBASENAME
            property_value = GetPropertyInfoValue(ref classType, "ProductNames_FileBaseName");
            if (property_value is null)
                property_value = "";
            msv.productFileBaseName = property_value.ToString();
            RedefineVariable("_USTN_PRODUCT_FILEBASENAME", property_value.ToString(), -1, false, true, "TXT_MsgVariableCreatedFromProductFileBaseName");


            // productFullMarketingName -> _USTN_PRODUCT_FILETYPENAME
            property_value = GetPropertyInfoValue(ref classType, "ProductNames_FileTypeName");
            if (property_value is null)
                property_value = "";
            msv.productFileTypeName = property_value.ToString();
            RedefineVariable("_USTN_PRODUCT_FILETYPENAME", property_value.ToString(), -1, false, true, "TXT_MsgVariableCreatedFromProductFileTypeName");

            // productFullMarketingName -> _USTN_PRODUCT_FULLMARKETINGNAME
            property_value = GetPropertyInfoValue(ref classType, "ProductNames_FullMarketingName");
            if (property_value is null)
                property_value = "";
            msv.productFullMarketingName = property_value.ToString();
            RedefineVariable("_USTN_PRODUCT_FULLMARKETINGNAME", property_value.ToString(), -1, false, true, "TXT_MsgVariableCreatedFromProductFullMarketingName");

            // productGroupName -> _USTN_PRODUCT_ONE_GROUPNAME
            property_value = GetPropertyInfoValue(ref classType, "ProductNames_GroupName");
            if (property_value is null)
                property_value = "";
            msv.productGroupName = property_value.ToString();
            RedefineVariable("_USTN_PRODUCT_ONE_GROUPNAME", property_value.ToString(), -1, false, true, "TXT_MsgVariableCreatedFromProductGroupName");

            // productProductName -> _USTN_PRODUCT_NAME
            property_value = GetPropertyInfoValue(ref classType, "ProductNames_ProductName");
            if (property_value is null)
                property_value = "";
            msv.productProductName = property_value.ToString();
            RedefineVariable("_USTN_PRODUCT_NAME", property_value.ToString(), -1, false, true, "TXT_MsgVariableCreatedFromName");

            // productShortName -> _USTN_PRODUCT_SHORTNAME, _USTN_PRODUCT_HELPFILENAME, _USTN_HELPNAMESPACE
            property_value = GetPropertyInfoValue(ref classType, "ProductNames_ProductShortName");
            if (property_value is null)
                property_value = "";
            msv.productProductShortName = property_value.ToString();
            RedefineVariable("_USTN_PRODUCT_SHORTNAME", property_value.ToString(), -1, false, true, "TXT_MsgVariableCreatedFromProductShortName");
            RedefineVariable("_USTN_PRODUCT_HELPFILENAME", property_value.ToString(), -1, false, true, "TXT_MsgVariableCreatedProductShortName");
            RedefineVariable("_USTN_HELPNAMESPACE", property_value.ToString(), -1, false, true, "TXT_MsgVariableCreatedFromProductShortName");
            RedefineVariable("_ENGINENAME", property_value.ToString(), -1, false, true, "TXT_MsgVariableCreatedFromProductShortName");

            // productShortMarketingName -> _USTN_PRODUCT_SHORTMARKETINGNAME
            property_value = GetPropertyInfoValue(ref classType, "ProductNames_ShortMarketingName");
            if (property_value is null)
                property_value = "";
            msv.productShortMarketingName = property_value.ToString();
            RedefineVariable("_USTN_PRODUCT_SHORTMARKETINGNAME", property_value.ToString(), -1, false, true, "TXT_MsgVariableCreatedFromProductShortMarketingName");

            // productSubGroupName -> _USTN_PRODUCT_ONE_SUBGROUPNAME
            property_value = GetPropertyInfoValue(ref classType, "ProductNames_SubGroupName");
            if (property_value is null)
                property_value = "";
            msv.productSubGroupName = property_value.ToString();
            RedefineVariable("_USTN_PRODUCT_ONE_SUBGROUPNAME", property_value.ToString(), -1, false, true, "TXT_MsgVariableCreatedFromProductSubGroupName");

            // productTitlebarName -> _USTN_PRODUCT_TITLEBARNAME
            property_value = GetPropertyInfoValue(ref classType, "ProductNames_TitleBarName");
            if (property_value is null)
                property_value = "";
            msv.productTitlebarName = property_value.ToString();
            RedefineVariable("_USTN_PRODUCT_TITLEBARNAME", property_value.ToString(), -1, false, true, "TXT_MsgVariableCreatedFromProductTitleBarName");

            // productCopyrightYears -> _USTN_PRODUCT_COPYRIGHTYEARS
            property_value = GetPropertyInfoValue(ref classType, "CopyrightYears");
            if (property_value is null)
                property_value = "";
            msv.productCopyrightYears = property_value.ToString();
            RedefineVariable("_USTN_PRODUCT_COPYRIGHTYEARS", property_value.ToString(), -1, false, true, "TXT_MsgVariableCreatedFromProductCopyRightYears");

            // version -> _USTN_PRODUCT_VERSION_GENERATION, _USTN_PRODUCT_VERSION_MAJOR, _USTN_PRODUCT_VERSION_MINOR, _USTN_PRODUCT_VERSION_BUILD
            property_value = GetPropertyInfoValue(ref classType, "ProductVersion");
            if (property_value is null)
                property_value = ApplicationData.Version;
            msv.version = property_value.ToString();
            //string[] version = Strings.Split(property_value.ToString(), ".", Compare: CompareMethod.Text);
            string[] version = property_value.ToString().Split('.');
            RedefineVariable("_USTN_PRODUCT_VERSION_GENERATION", version[0], -1, false, true, "TXT_MsgVariableCreatedFromProduct_Version");
            RedefineVariable("_USTN_PRODUCT_VERSION_MAJOR", version[1], -1, false, true, "TXT_MsgVariableCreatedFromProduct_Version");
            RedefineVariable("_USTN_PRODUCT_VERSION_MINOR", version[2], -1, false, true, "TXT_MsgVariableCreatedFromProduct_Version");
            RedefineVariable("_USTN_PRODUCT_VERSION_BUILD", version[3], -1, false, true, "TXT_MsgVariableCreatedFromProduct_Version");

            // _VERSION_10_0, _VERSION_8_11


            string valueStr = property_value?.ToString() ?? string.Empty;

            if (valueStr.StartsWith("10.", StringComparison.OrdinalIgnoreCase))
            {
                RedefineVariable("_VERSION_10_0", "", -1, false, true, "TXT_MsgVariableCreatedFromProduct_Version");
            }
            else
            {
                RedefineVariable("_VERSION_10_0", "", -1, false, true, "TXT_MsgVariableCreatedFromProduct_Version");
            }

            // _MICROSTATION
            if (ApplicationData.Name.Contains("MicroStation"))
            {
                RedefineVariable("_MICROSTATION", 1.ToString(), -1, false, true, "");
            }
        }

        public object GetPropertyInfoValue(ref Type classType, string name)
        {
            object property_value = null;

            if (classType != null)
            {
                var propertyInfo = classType.GetProperty(name);
                var getMethodInfo = propertyInfo.GetGetMethod();
                property_value = getMethodInfo.Invoke(null, null);
            }

            return property_value;
        }

        private void Set_USTN_LocalUserTempPathOld()
        {
            var rootVar = GetVariable("_ROOTDIR");
            var appVar = GetVariable("_USTN_PRODUCT_DIRNAME");
            string root;
            string app = "";

            if (rootVar is not null)
                root = rootVar.FinalExpansion; // TODO: root is set but not used
            if (appVar is not null)
                app = appVar.FinalExpansion;

            CFGVariable variableTmp;

            variableTmp = GetVariable("MS_TMP");

            if (variableTmp is null)
            {
                DefineVariable("MS_TMP", UtilitiesPath.GetUserSubDirectory(UtilitiesPath.GetUserTempPath(), app, "10.*", "10.0.0"), -1, false, false, "TXT_MsgVariableCreatedFromLocalUserTempPath");
                variableTmp = GetVariable("MS_TMP");
            }

            int argclevel = 6;
            RedefineVariable("_USTN_LOCALUSERTEMPPATH", variableTmp.GetValue(clevel: ref argclevel), -1, false, true, "TXT_MsgVariableCreatedFromLocalUserTempPath");
        }

        // The directory that serves as a common repository for application-specific data that is used by the current, non-roaming user
        private void Set_USTN_LocalUserTempPath()
        {
            var rootVar = GetVariable("_ROOTDIR");
            var appVar = GetVariable("_USTN_PRODUCT_DIRNAME");
            string root = "";
            string app = "";
            string path;

            if (rootVar is not null)
                root = rootVar.FinalExpansion;
            if (appVar is not null)
                app = appVar.FinalExpansion;

            if (Variables.ContainsKey("_USTN_LocalUserTempPath"))
            {
                return;
            }
            else
            {
                path = UtilitiesPath.GetSystemApplicationDir("%TEMP%", app, root);
                if (string.IsNullOrEmpty(path))
                    return;
                DefineVariable("_USTN_LocalUserTempPath", path, 0, false, true, "TXT_MsgVariableCreatedFromLocalUserTempPath");
            }
        }

        // The directory that serves as a common repository for application-specific data that is used by the current, non-roaming user
        private void Set_USTN_LocalUserAppDataPath()
        {
            var rootVar = GetVariable("_ROOTDIR");
            var appVar = GetVariable("_USTN_PRODUCT_DIRNAME");
            string root = "";
            string app = "";
            string path;

            if (rootVar is not null)
                root = rootVar.FinalExpansion;
            if (appVar is not null)
                app = appVar.FinalExpansion;

            if (Variables.ContainsKey("_USTN_LocalUserAppDataPath"))
            {
                return;
            }
            else
            {
                path = UtilitiesPath.GetSystemApplicationDir("%LOCALAPPDATA%",app, root);
                if (string.IsNullOrEmpty(path))
                    return;
                DefineVariable("_USTN_LocalUserAppDataPath", path, 0, false, true, "TXT_MsgVariableCreatedFromLocalUserTempPath");
            }
        }

        public string GetInstallPathFromDirectoryExplanation(string path)
        {
            string installFolder = "";
            try
            {
                string directoryFile = path + "DirectoryExplanation.txt";

                using (var streamReader = new StreamReader(directoryFile))
                {
                    string line;

                    while (!streamReader.EndOfStream && string.IsNullOrEmpty(installFolder))
                    {
                        //line = Strings.Trim(streamReader.ReadLine());
                        line = streamReader.ReadLine()?.Trim();
                        if (Directory.Exists(line))
                        {
                            installFolder = line;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }

            return installFolder;
        }

        // The directory that serves as a common repository for application-specific data for the current roaming user.
        private void Set_USTN_UserAppDataPath()
        {
            var appVar = GetVariable("_USTN_PRODUCT_DIRNAME");
            string app = "";

            if (appVar is not null)
                app = appVar.FinalExpansion;

            string path = Path.Join(
                UtilitiesPath.GetUserRoamingAppDataPath(),
                "Bentley",
                app,
                Path.DirectorySeparatorChar.ToString());
            DefineVariable("_USTN_UserAppDataPath", path, 0, false, true, "h");
        }

        // The directory that serves as a common repository for application-specific data that is used by all users.
       /*
        private void Set_USTN_CommonAppDataPath1()
        {
            var appVar = GetVariable("_USTN_PRODUCT_DIRNAME");
            string app = "";

            if (appVar is not null)
                app = appVar.FinalExpansion;

            string programDataDir = Environment.ExpandEnvironmentVariables("%PROGRAMDATA%");
            RedefineVariable("_USTN_CommonAppDataPath", UtilitiesPath.GetUserSubDirectory(programDataDir, app, "10.*", "10.0.0"), -1, false, true, "");
        }
        */
        private void Set_USTN_CommonAppDataPath()
        {
            var rootVar = GetVariable("_ROOTDIR");
            var appVar = GetVariable("_USTN_PRODUCT_DIRNAME");
            string root = "";
            string app = "";
            string path;

            if (rootVar is not null)
                root = rootVar.FinalExpansion;
            if (appVar is not null)
                app = appVar.FinalExpansion;

            if (Variables.ContainsKey("_USTN_LocalUserAppDataPath"))
            {
                return;
            }
            else
            {
                path = UtilitiesPath.GetSystemApplicationDir("%PROGRAMDATA%", app, root);
                if (string.IsNullOrEmpty(path))
                    return;
                DefineVariable("_USTN_LocalUserAppDataPath", path, 0, false, true, "TXT_MsgVariableCreatedFromLocalUserTempPath");
            }
        }



        private void Set_USTN_USERCFG()
        {
            RedefineVariable("_USTN_USERCFG", "$(_USTN_LocalUserAppDataPath)/prefs/Personal.ucf", -1, true, true, "TXT_MsgVariableCreatedFromLocalUserTempPath");
        }

        // ---------------------------------------------------------------------------------------
        // @description: Process this workspace and abort when a specific file is reached.
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void ProcessWatchForFile(string filepath, string cfgpath)
        {
            DisableEventRecording = true;

            SetVariables();

            try
            {
                // Enable the watch
                WatchForFile = filepath;

                CESetup();

                ProcessFilesInDirectory(ApplicationData.PredefinedCfgNames, CEPredefinedCfgsDir);

                if (!Variables.ContainsKey("_USTN_CONFIGURATION"))
                {
                    string msdirfile;

                    msdirfile = GetVariable("_ROOTDIR").FinalExpansion + @"config\msdir.cfg";
                    CFGFile argParentFile = null;
                    ProcessFile(msdirfile, argParentFile);
                    CFGVariable installedConfiguration;
                    installedConfiguration = GetVariable("_USTN_INSTALLED_CONFIGURATION");
                    DefineVariable("_USTN_CONFIGURATION", installedConfiguration.Value, 0, true, true, "TXT_MsgVariableDefinedInCommandline");
                }

                string _ustn_Enginename, _ustn_Product_GroupName, _ustn_Product_SubgroupName;

                _ustn_Enginename = GetVariable("_ENGINENAME").FinalExpansion;
                _ustn_Product_GroupName = GetVariable("_USTN_PRODUCT_ONE_GROUPNAME").FinalExpansion;
                _ustn_Product_SubgroupName = GetVariable("_USTN_PRODUCT_ONE_SUBGROUPNAME").FinalExpansion;

                VarDB.GetVariableDBCSV(CEVariableCSVsDir);
                // VarDB.GetVariableDBXML(CEVariableXMLsDir)


                CFGFile argParentFile1 = null;
                ProcessFile(cfgpath, argParentFile1);

                // Replace variable placeholders (e.g $(ABC)) with current values
                ReplaceVariablePlaceholders();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                return;
            }
        }

        public void ProcessWatchForLevel(int watchLevel, string cfgpath)
        {
            DisableEventRecording = true;

            SetVariables();

            try
            {
                // Enable the watch
                WatchForLevel = watchLevel;

                CESetup();

                ProcessFilesInDirectory(ApplicationData.PredefinedCfgNames, CEPredefinedCfgsDir);

                if (!Variables.ContainsKey("_USTN_CONFIGURATION"))
                {
                    string msdirfile;
                    msdirfile = GetVariable("_ROOTDIR").FinalExpansion + @"config\msdir.cfg";
                    CFGFile argParentFile = null;
                    ProcessFile(msdirfile, argParentFile);
                    CFGVariable installedConfiguration;
                    installedConfiguration = GetVariable("_USTN_INSTALLED_CONFIGURATION");
                    DefineVariable("_USTN_CONFIGURATION", installedConfiguration.Value, 0, true, true, "TXT_MsgVariableDefinedInCommandline");
                }

                string _ustn_Enginename, _ustn_Product_GroupName, _ustn_Product_SubgroupName;

                _ustn_Enginename = GetVariable("_ENGINENAME").FinalExpansion;
                _ustn_Product_GroupName = GetVariable("_USTN_PRODUCT_ONE_GROUPNAME").FinalExpansion;
                _ustn_Product_SubgroupName = GetVariable("_USTN_PRODUCT_ONE_SUBGROUPNAME").FinalExpansion;

                VarDB.GetVariableDBCSV(CEVariableCSVsDir);
                // VarDB.GetVariableDBXML(CEVariableXMLsDir)

                CFGFile argParentFile1 = null;
                ProcessFile(cfgpath, argParentFile1);

                // Replace variable placeholders (e.g $(ABC)) with current values
                ReplaceVariablePlaceholders();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                return;
            }
        }

        private void SetVariables()
        {
            SetCommandLineVariables();
            SetBuiltinVariables();

            Set_ROOTDIR();
            Set_WORKDIR();
            Set_PRODUCT();

            Set_USTN_LocalUserAppDataPath();
            Set_USTN_LocalUserTempPath();
            Set_USTN_UserAppDataPath();
            Set_USTN_CommonAppDataPath();
            Set_USTN_USERCFG();
            Set_USTN_LOCALE();
        }

        private void SetBuiltinVariables()
        {
            RedefineVariable("_winNT", "", -1, false, true, "");
            RedefineVariable("_intelNT", "", -1, false, true, "");
            RedefineVariable("_PLATFORMNAME", "intelnt", -1, false, true, "");
        }

        // ---------------------------------------------------------------------------------------
        // @description: Begins processing a series of configuration files
        // Now takes in the CONNECT checkbox so that SetUserProjectVars can know if this is a
        // CONNECT application or not.
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void ProcessConfiguration()
        {

            FileWatcher.ClearWatches();
            FileWatcher.SetEnable(false);
            // Record the date/time the workspace was processed
            DateProcessed = DateTime.Now;

            // Build descriptions
            Description = ApplicationData.Name + " - " + ApplicationData.Workspace + " - " + ApplicationData.Workset;

            // Clear out some statitics
            ResetStatus();

            // Open the status form to display events as the workspace loads
            if (ShowStatus)
            {
                InterfaceControler.StatusClickable = false;
                InterfaceControler.ShowStatus();
                this.UpdateStatus();
            }

            SetVariables();

            try
            {
                // Set the user and project
                SetSelectionVars();

                // Process the predefined cfg file
                // TODO: Change to single path interation

                CESetup();

                VarDB.GetVariableDBCSV(CEVariableCSVsDir);
                // VarDB.GetVariableDBXML(CEVariableXMLsDir)

                ProcessFilesInDirectory(ApplicationData.PredefinedCfgNames, CEPredefinedCfgsDir);

                if (!Variables.ContainsKey("_USTN_CONFIGURATION"))
                {
                    string msdirfile;
                    msdirfile = GetVariable("_ROOTDIR").FinalExpansion + @"config\msdir.cfg";
                    CFGFile argParentFile = null;
                    ProcessFile(msdirfile, argParentFile);
                    CFGVariable installedConfiguration;
                    installedConfiguration = GetVariable("_USTN_INSTALLED_CONFIGURATION");
                    DefineVariable("_USTN_CONFIGURATION", installedConfiguration.Value, 0, true, true, "TXT_MsgVariableDefinedInCommandline");
                }

                string _ustn_Enginename, _ustn_Product_GroupName, _ustn_Product_SubgroupName;

                _ustn_Enginename = GetVariable("_ENGINENAME").FinalExpansion;
                _ustn_Product_GroupName = GetVariable("_USTN_PRODUCT_ONE_GROUPNAME").FinalExpansion;
                _ustn_Product_SubgroupName = GetVariable("_USTN_PRODUCT_ONE_SUBGROUPNAME").FinalExpansion;

                VarDB.GetVariableDBCSV(CEVariableCSVsDir);
                // VarDB.GetVariableDBXML(CEVariableXMLsDir)

                // Process the main config file
                CFGFile argParentFile1 = null;
                ProcessFile(ApplicationData.StartupCfgPath, argParentFile1, 0);
                WatchLevel = 7;

                // TODO: testing
                // Replace variable placeholders (e.g $(ABC)) with current values
                ReplaceVariablePlaceholders();

                // Post Processing
                // ---------------------------------------------------------------------------
                // Assigns a CFG_Variable reference to events that were created before their variable existed, but exist now
                SetEventVariables();

                // Expand each variable for quick reference later
                SetVariableExpansions();

                // Check for inconsistent variable values
                CheckVariableValueTypes();

                // Run Validation Rules
                RunValidationRules();

                // _USTN_CACHECFG is a predefined variable, but is set at the user level and should be locked after set
                if (Variables.ContainsKey("_USTN_CACHECFG") == true)
                {
                    ((dynamic)Variables["_USTN_CACHECFG"]).IsLocked = true;
                }

                // Show that the workspace is done processing
                if (ShowStatus)
                {

                    InterfaceControler.StatusClickable = true;

                    if (AbortProcessing)
                    {
                        this.UpdateStatus();
                        InterfaceControler.StatusFile = " Critical Error";
                        InterfaceControler.UpdateStatus();
                    }
                    else
                    {
                        this.UpdateStatus();
                        InterfaceControler.StatusDepth = 0;
                        InterfaceControler.StatusFile = "Done";
                        InterfaceControler.UpdateStatus();
                    }

                    if (CloseStatusWhenDone)
                        InterfaceControler.HideStatus();
                }

                foreach (CFGFile cfile in CFGFiles)
                    FileWatcher.AddWatch(ref cfile.FilePath);

                FileWatcher.SetEnable(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name} 3 : {ex.Message}");
                return;
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Begins processing a configuration file
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void ProcessFile(string path, CFGFile ParentFile, int level = -999)
        {
            // Create a new cfg file object
            CFGFile cFile;

            // Skip Aborted or .ucf files
            if (AbortProcessing || path.ToUpperInvariant().Contains(".UCF"))
            {
                string message = "NOTE: Skipping .ucf file: " + path;
                InterfaceControler.Echo(message);
                return;
            }
                    

            // Add processing event to the event list
            if (ParentFile is null)
            {
                var argetype = CFGEnums.CFGEventType.cfgInclude;
                string argdescription = string.Format(CEResource.TXT_MsgStartProcessing, path);
                CFGLine argline = null;
                CFGFile argfile = null;
                CFGVariable argvariable = null;
                string argvarname = "";
                this.AddEvent(ref argetype, ref argdescription, line: argline, file: argfile, variable: argvariable, varname: argvarname);
            }

            // Removed to allow recursive processing of .cfg files.  V8i did not allow recurcive files, but CONNECT does.
            // Check to see if the file has been processed already
            // If HasFileBeenProcessed(path) Then
            // AddEvent(CFGEventType.cfgMessage, String.Format(CEResource.TXT_MsgIncludeIgnoredAlreadyProcessed, path))
            // Exit Sub
            // End If

            // Check to see if file exists
            if (File.Exists(path) == false)
            {
                var argetype1 = CFGEnums.CFGEventType.cfgCritical;
                string argdescription1 = string.Format(CEResource.TXT_MsgIncludeIgnoredFileNotFound, path);
                CFGLine argline1 = null;
                CFGFile argfile1 = null;
                CFGVariable argvariable1 = null;
                string argvarname1 = "";
                this.AddEvent(ref argetype1, ref argdescription1, line: argline1, file: argfile1, variable: argvariable1, varname: argvarname1);
                return;
            }

            if (ParentFile is not null)
            {
                // Create a new CFG_FILE and
                var argwrkspc = this;
                cFile = new CFGFile(ref argwrkspc)
                {
                    CurrentLevel = ParentFile.CurrentLevel,
                    ParentFile = ParentFile,
                    Depth = ParentFile.Depth + 1
                };
                ParentFile.ChildFiles.Add(cFile);
            }
            else
            {
                // Create a new CFG_FILE and
                var argwrkspc1 = this;
                cFile = new CFGFile(ref argwrkspc1)
                {
                    ParentFile = ParentFile,
                    Depth = 0
                };

            }

            // Set the path
            cFile.FilePath = path;

            // Inherit the level value
            cFile.CurrentLevel = -1;
            if (ParentFile is not null)
                cFile.CurrentLevel = ParentFile.CurrentLevel;
            if (level != -999)
                cFile.CurrentLevel = level;

            // Add it to CFGFiles list
            CFGFiles.Add(cFile);

            // Process the file
            cFile.ProcessFile();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Returns true if CFGFile contains a file with a specific path
        // ---------------+---------------+---------------+---------------+---------------+-------
        public bool HasFileBeenProcessed(ref string path)
        {
            // Check to see if this file has already been processed
            foreach (CFGFile cfile in CFGFiles)
            {
                //if (CultureInfo.CurrentCulture.CompareInfo.Compare(Strings.LCase(cfile.FilePath) ?? "", Strings.LCase(path) ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                if (string.Equals(cfile.FilePath, path, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }

            return false;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Adds a new event to the event list CFGEvents
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void AddEvent(ref CFGEnums.CFGEventType etype, ref string description, [Optional] CFGLine line, [Optional] CFGFile @file, [Optional] CFGVariable variable, [Optional, DefaultParameterValue("")] string varname)
        {
            CFGEvent newevent;

            // Watch for File Event

            if (etype == CFGEnums.CFGEventType.cfgLevelChange)
            {
                //AbortProcessing = CultureInfo.CurrentCulture.CompareInfo.Compare("MSCONFIG.CFG", Strings.UCase(@file.Name), CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 && WatchForLevel == @file.CurrentLevel && WatchForLevel != 0;

                AbortProcessing =
                    string.Equals("MSCONFIG.CFG", @file.Name, StringComparison.CurrentCultureIgnoreCase) &&
                    WatchForLevel == @file.CurrentLevel &&
                    WatchForLevel != 0;
                if (AbortProcessing)
                {
                    WatchForLevel = 7;
                    return;
                }
            }

            if (etype == CFGEnums.CFGEventType.cfgFinishInclude)
            {
                //AbortProcessing = !string.IsNullOrEmpty(WatchForFile) && CultureInfo.CurrentCulture.CompareInfo.Compare(Strings.UCase(WatchForFile) ?? "", Strings.UCase(@file.FilePath) ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0;

                AbortProcessing =
                    !string.IsNullOrEmpty(WatchForFile) &&
                    string.Equals(WatchForFile, @file.FilePath, StringComparison.CurrentCultureIgnoreCase);
                return;
            }

            if (etype == CFGEnums.CFGEventType.cfgVardef)
            {
                if (variable is not null)
                {
                    //if (!string.IsNullOrEmpty(WatchVariable) && CultureInfo.CurrentCulture.CompareInfo.Compare(Strings.UCase(WatchVariable) ?? "", Strings.UCase(variable.Name) ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                    if (!string.IsNullOrEmpty(WatchVariable) &&
                        string.Equals(WatchVariable, variable.Name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        AbortProcessing = true;
                        return;
                    }
                }
            }

            if (!DisableEventRecording)
            {

                newevent = new CFGEvent();

                // Count errors and warnings to display on status form
                switch (etype)
                {
                    case CFGEnums.CFGEventType.cfgWarning:
                        {
                            WarningCount += 1;
                            if (line is not null)
                                line.HasWarnings = true;
                            break;
                        }
                    case CFGEnums.CFGEventType.cfgError:
                        {
                            ErrorCount += 1;
                            break;
                        }
                }

                // Assign values to event
                {
                    ref var withBlock = ref newevent;
                    withBlock.EventType = etype;
                    withBlock.Description = description;
                    withBlock.ParentLine = line;
                    withBlock.ParentFile = @file;
                    withBlock.Variable = variable;

                    if (variable is not null)
                    {
                        if (etype == CFGEnums.CFGEventType.cfgVardef)
                        {
                            withBlock.VarValue = variable.Value;
                            withBlock.VarExpansion = variable.Expand(6);
                        }
                        withBlock.VarName = variable.Name;
                        withBlock.Level = variable.Level;
                    }
                    else
                    {
                        withBlock.VarName = varname;
                    }

                }

                CFGEvents.Add(newevent);
            }

            // If the event is a critical error then abort processing
            if (etype == CFGEnums.CFGEventType.cfgCritical)
            {

                string message;
                newevent = new CFGEvent();
                AbortProcessing = true;
                var argetype = CFGEnums.CFGEventType.cfgAbort;
                string argdescription = CEResource.TXT_MsgCriticalErrorProcessAborted;
                CFGVariable argvariable = null;
                string argvarname = "";
                AddEvent(ref argetype, ref argdescription, line, @file, variable: argvariable, varname: argvarname);

                // If ShowStatus Then
                var cfgerror = new CFGError();

                if (@file is not null)
                {
                    cfgerror.FilePath = @file.FilePath;
                    cfgerror.LinkLabel1.Visible = true;
                    if (line is not null)
                    {
                        message = string.Format(CEResource.TXT_MsgCriticalErrorRaisedOnLine, @file.Name, line.LineNumber);
                        message = message + newevent.Description + " ";
                        cfgerror.LinkLabel1.Text = message;
                        cfgerror.Label1.Text = line.Text;

                        cfgerror.LineNumber = line.LineNumber;
                    }
                    else
                    {
                        message = string.Format(CEResource.TXT_MsgCriticalErrorRaisedOnLine, @file.Name, "unknown");
                        message = message + newevent.Description + " ";
                        cfgerror.LinkLabel1.Text = message;
                        if (newevent.ParentLine is not null)
                        {
                            cfgerror.Label1.Text = newevent.ParentLine.Text;
                        }
                        cfgerror.LineNumber = 0;
                    }
                }
                else
                {
                    message = CEResource.TXT_MsgCriticalErrorRaisedOnLine;
                    message = message + newevent.Description + " ";
                    cfgerror.LinkLabel1.Text = message;
                }

                cfgerror.ShowDialog();

            }

            // Update the statitics on the status form
            if (ShowStatus && CFGEvents.Count % 3 == 0)
            {
                this.UpdateStatus();
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Updates the status information in the status window
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void UpdateStatus([Optional] CFGFile @file)
        {
            try
            {
                if (CFGFiles?.Count > 0)
                {
                    var targetFile = @file ?? CFGFiles[CFGFiles.Count - 1] as CFGFile;
                    if (targetFile != null)
                    {
                        InterfaceControler.StatusFile = targetFile.Name;
                        InterfaceControler.StatusDepth = targetFile.Depth;
                    }
                }

                InterfaceControler.StatusVariables = Variables?.Count ?? 0;
                InterfaceControler.StatusFiles = CFGFiles?.Count ?? 0;
                InterfaceControler.StatusEvents = CFGEvents?.Count ?? 0;
                InterfaceControler.StatusWarnings = WarningCount;
                InterfaceControler.StatusErrors = ErrorCount;
                InterfaceControler.UpdateStatus();
                InterfaceControler.StatusLineCount = CFGLine.LineCount;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] {GetType().Name}.{nameof(UpdateStatus)}: {ex.Message}");
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Returns a reference to a variable in the workspace
        // ---------------+---------------+---------------+---------------+---------------+-------
        public CFGVariable GetVariable(string name)
        {
            // Returns Nothing if the variable is not found
            //Debug.WriteLine($"CFGConfiguration: GetVariable(" + name + ")");
            if (string.IsNullOrEmpty(name))
                return null;
            //String NameUpper = name.ToUpper();
            //name = name.ToLowerInvariant();
            //name = name.ToUpperInvariant();

            if (Variables.ContainsKey(name) == true)
            {
                //Debug.Write($"CFGConfiguration: GetVariable return:" + (CFGVariable)Variables[name]);
                return (CFGVariable)Variables[name];
            }
            //Debug.Write($"CFGConfiguration: GetVariable return:Null");
            return null;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Returns a reference to a CFG_File in the workspace
        // ---------------+---------------+---------------+---------------+---------------+-------
        public CFGFile GetFile(string path)
        {
            // Returns Nothing if the file is not found

            foreach (CFGFile cfile in CFGFiles)
            {
                //if (CultureInfo.CurrentCulture.CompareInfo.Compare(Strings.UCase(cfile.FilePath) ?? "", Strings.UCase(path) ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                if (string.Equals(cfile.FilePath, path, StringComparison.CurrentCultureIgnoreCase))
                    return cfile;
            }

            return null;
        }



        // ---------------------------------------------------------------------------------------
        // @description: Returns the reference to a variable defined on lineno in Filename.
        // ---------------+---------------+---------------+---------------+---------------+-------
        public string GetVariableName(ref string Filename, int lineno)
        {
            // Returns the name of a variable defined on lineno in Filename. Returns nothing if nothing is found on that line.

            CFGFile cfile;
            CFGLine cline;
            cfile = GetFile(Filename);

            if (cfile is null || cfile.Lines.Count < lineno)
                return "";

            cline = cfile.GetLine(lineno);
            if (cline is null)
                return "";

            foreach (CFGEvent eve in CFGEvents)
            {
                if (eve.ParentLine is not null && eve.ParentLine.Equals(cline) && eve.Variable is not null)
                    return eve.Variable.Name;
            }

            return "";
        }

        public bool IsVariableDefined(string name)
        {
            return GetVariable(name) is not null;

        }

        // ---------------------------------------------------------------------------------------
        // @description: Post processing; Run Validation Rules
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void RunValidationRules()
        {
            if (string.IsNullOrWhiteSpace(ApplicationData.Rules))
                return;

            var arginwrkspc = this;
            var VR = new CFGVariableValidater(ref arginwrkspc);
            string[] mis = ApplicationData.Rules.Split(';');

            foreach (string rule in mis)
                VR.ProcessValidationRule(rule);

            VR.CreateEventsFromValidationResults();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Deletes all tokens from each line to save space when saving the workspace to a file
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void ClearTokens()
        {
            foreach (CFGFile @file in CFGFiles)
                @file.ClearTokens();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Clear out some statitics
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void ResetStatus()
        {
            CFGLine.LineCount = 0;
            CFGFile.NextUID = 0;
            ErrorCount = 0;
            WarningCount = 0;
            CFGEvent.NextUID = 0;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Post processing: finds a variable reference for each event that needs one
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void SetEventVariables()
        {
            foreach (CFGEvent ev in CFGEvents)
            {
                if (!string.IsNullOrEmpty(ev.VarName) & ev.Variable is null)
                {
                    ev.Variable = GetVariable(ev.VarName);
                }

                if (ev.ParentFile is null & ev.ParentLine is not null)
                {
                    ev.ParentFile = ev.ParentLine.ParentFile;
                }
            }
        }
        // ---------------------------------------------------------------------------------------
        // @description: Post processing; Expand each variable
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void SetVariableExpansions()
        {
            //foreach (CFGVariable variable in Variables)
            //variable.SetFinalExpansion();

            // Snapshot: SetFinalExpansion() -> Expand() -> MacroParser.ParseLine() can define new
            // variables on this same dictionary (e.g. undefined $(VAR) references), which would
            // otherwise throw "Collection was modified" while enumerating it directly.
            foreach (var kvp in Variables.ToList())
            {
                string key = kvp.Key;
                CFGVariable variable = kvp.Value;
                variable.SetFinalExpansion();
            }


        }

        // ---------------------------------------------------------------------------------------
        // @description: Post processing; Check for potential value type mismatches in variable values
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void CheckVariableValueTypes()
        {
            foreach (CFGVariable @var in Variables.Values)
            {
                string[] mis = @var.FinalExpansion.Split(new[] { ';' }, StringSplitOptions.None);
                string tmp1 = string.Empty;
                string tmp2;

                if (mis.Length > 1)
                {
                    foreach (string st in mis)
                    {
                        string valueType = Utilities.ValueType(st);

                        if (string.IsNullOrEmpty(tmp1))
                        {
                            tmp1 = valueType;
                        }
                        else
                        {
                            tmp2 = valueType;

                            if (!string.IsNullOrEmpty(tmp2) &&
                                !string.Equals(tmp1, tmp2, StringComparison.CurrentCultureIgnoreCase))
                            {
                                var eventType = CFGEnums.CFGEventType.cfgWarning;
                                string description = string.Format(CEResource.TXT_MsgVariableContainsInconsistancy, tmp1, tmp2);
                                CFGLine line = null;
                                CFGFile file = null;

                                AddEvent(ref eventType, ref description, line, file, @var, @var.Name);
                                break;
                            }
                        }
                    }
                }

                // TODO: this needs work
                foreach (string st in mis)
                {
                    string fileName = UtilitiesPath.GetFileName(st).ToLowerInvariant();

                    if ((!UtilitiesPath.IsValidFileNameOrPath(fileName)) && (fileName!=""))
                    {
                        var eventType = CFGEnums.CFGEventType.cfgWarning;
                        string description = CEResource.TXT_MsgPossibleInvalidFileExtension;
                        CFGLine line = null;
                        CFGFile file = null;

                        AddEvent(ref eventType, ref description, line, file, @var, @var.Name);
                    }
                }
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Returns a reference to a CFG_File in the workspace
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void ProcessFilesInDirectory(string fileListRaw, string dirpath)
        {
            if (!string.IsNullOrWhiteSpace(fileListRaw))
            {
                string[] files = fileListRaw.Split(';');

                foreach (string fileName in files)
                {
                    string cfgFile = Path.Combine(dirpath, fileName + ".cfg");
                    CFGFile argParentFile = null;
                    ProcessFile(cfgFile, argParentFile);
                }
            }
        }

        public void CESetup()
        {

            string programdataDir = Environment.ExpandEnvironmentVariables("%PROGRAMDATA%");

            string CEProgamDataPath = Path.Combine(programdataDir, "Bentley", "ConfigurationExplorer");
            CEPredefinedCfgsDir = Path.Combine(CEProgamDataPath, "PredefinedCfgs");
            CEVariableCSVsDir = Path.Combine(CEProgamDataPath, "VariableCSVs");
            CEVariableXMLsDir = Path.Combine(CEProgamDataPath, "VariableXMLs");

            try
            {
                if (!Directory.Exists(CEProgamDataPath))
                {
                    // try to create the directory
                    object unused1 = Directory.CreateDirectory(CEProgamDataPath);
                }
                if (!Directory.Exists(CEPredefinedCfgsDir))
                {
                    object unused2 = Directory.CreateDirectory(CEPredefinedCfgsDir);
                }
                if (!Directory.Exists(CEVariableCSVsDir))
                {
                    object unused3 = Directory.CreateDirectory(CEVariableCSVsDir);
                }
                if (!Directory.Exists(CEVariableXMLsDir))
                {
                    object unused3 = Directory.CreateDirectory(CEVariableXMLsDir);
                }
            }
            catch (Exception)
            {
                // File/Folder creation failed, show message to user and exit
                string msg;
                msg = string.Format(Utilities.GetResourceString("TXT_ErrCesettingsFailedToCreateCfg"), CEProgamDataPath);
                object unused = MessageBox.Show(msg, Utilities.GetResourceString("TXT_MsgFileCreateFailed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }



    }
}
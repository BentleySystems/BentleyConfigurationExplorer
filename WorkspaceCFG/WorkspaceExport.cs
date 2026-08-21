// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{

    public static class WorkspaceExport
    {

        // Settings used to persist/restore a full CFGConfiguration object graph to/from disk.
        // TypeNameHandling.Auto preserves the concrete runtime types stored in the CFGFiles/CFGEvents
        // ArrayLists (BinaryFormatter used to do this implicitly); PreserveReferencesHandling.All
        // preserves any circular references (e.g. events pointing back at their parent file/line).
        private static readonly JsonSerializerSettings WorkspaceJsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            Formatting = Formatting.None
        };

        // ---------------------------------------------------------------------------------------
        // @description: Saves a workspace instance to a file
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void SaveWorkspace(ref string path, ref CFGConfiguration workspace, [Optional, DefaultParameterValue(false)] ref bool silent)
        {
            try
            {
                workspace.ClearTokens();

                string json = JsonConvert.SerializeObject(workspace, WorkspaceJsonSettings);
                File.WriteAllText(path, json, new UTF8Encoding(false));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] WorkspaceExport.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                if (!silent)
                {
                    System.Windows.Forms.MessageBox.Show(CEResource.TXT_ProblemSavingWorkspace1, CEResource.TXT_TitleError);
                }
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Loads a workspace instance from a file
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void LoadConfiguration(ref string path, ref CFGConfiguration workspace, [Optional, DefaultParameterValue(false)] ref bool silent)
        {
            if (File.Exists(path) == false)
                return;

            try
            {
                string json = File.ReadAllText(path);
                workspace = JsonConvert.DeserializeObject<CFGConfiguration>(json, WorkspaceJsonSettings);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] WorkspaceExport.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                if (!silent)
                    System.Windows.Forms.MessageBox.Show(CEResource.TXT_ProblemOpeningWorkspace, CEResource.TXT_TitleError);
            }
        }


        public static void SaveWorkspaceToJson(ref string path, ref CFGConfiguration workspace, [Optional, DefaultParameterValue(false)] ref bool silent)
        {
            if (workspace is null)
                return;

            try
            {
                workspace.ClearTokens();

                using (var streamWriter = new StreamWriter(path, false, new UTF8Encoding(false)))
                {
                    WriteWorkspaceJson(streamWriter, workspace);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] WorkspaceExport.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                if (!silent)
                {
                    System.Windows.Forms.MessageBox.Show(CEResource.TXT_ProblemSavingWorkspace1, CEResource.TXT_TitleError);
                }
            }
        }

        public static bool ConvertWorkspaceFileToJson(ref string sourcePath, ref string jsonPath, [Optional, DefaultParameterValue(false)] ref bool silent)
        {
            CFGConfiguration workspace = null;

            LoadConfiguration(ref sourcePath, ref workspace, ref silent);
            if (workspace is null)
                return false;

            SaveWorkspaceToJson(ref jsonPath, ref workspace, ref silent);
            return File.Exists(jsonPath);
        }

        // ---------------------------------------------------------------------------------------
        // @description: Saves a workspace to a text file. Similar to a msdebug file
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void SaveWorkspaceToTextFile(ref CFGConfiguration workspace)
        {
            if (workspace is null)
                return;
            string path;
            string temp;

            var saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Title = CEResource.TXT_ExportWorkspaceDetailsToTextFile;
            saveFileDialog.InitialDirectory = CEResource.TXT_CDrive;
            saveFileDialog.Filter = CEResource.TXT_FileFilterTxt;
            saveFileDialog.FileName = workspace.ApplicationData.Name + "-" + workspace.ApplicationData.Workspace + "-" + workspace.ApplicationData.Workset + CEResource.TXT_ExtTxt;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            path = saveFileDialog.FileName;

            using (var streamWriter = new StreamWriter(path))
            {
                streamWriter.WriteLine(CFGLine.LineCount + " usable lines found ");
                streamWriter.WriteLine(workspace.Variables.Count + " variables defined");
                streamWriter.WriteLine(workspace.CFGFiles.Count + " files processed");
                streamWriter.WriteLine(workspace.CFGEvents.Count + " total events.");
                streamWriter.WriteLine("-------------------------");

                foreach (CFGEvent cfgEvent in workspace.CFGEvents)
                {
                    if (cfgEvent.ParentLine is not null)
                    {
                        streamWriter.WriteLine(cfgEvent.UID + 1 + ": " + cfgEvent.Description + " : " + cfgEvent.ParentFile.Name + " Line: " + cfgEvent.ParentLine.LineNumber);
                    }
                    else
                    {
                        streamWriter.WriteLine(cfgEvent.UID + 1 + ": " + cfgEvent.Description);
                    }
                }

                streamWriter.WriteLine("-------------------------");

                foreach (CFGVariable variable in workspace.Variables.Values.ToList())
                {
                    temp = variable.Expand();
                    if (string.IsNullOrEmpty(temp))
                        temp = "[NULL]";
                    streamWriter.WriteLine(variable.Name.ToUpperInvariant() + new string(' ', Math.Max(0, 60 - variable.Name.Length)) + "=" + '\t' + temp);
                }

            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Creates a "compiled" configuration file with all the final variable values
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void SaveCompiledWorkspace(ref CFGConfiguration workspace)
        {
            if (workspace is null)
                return;

            string temp;
            int t;
            string path;
            CFGVariable variable;

            var savefiledialog = new System.Windows.Forms.SaveFileDialog();
            savefiledialog.Title = CEResource.TXT_ExportWorkspaceDetailsToTextFile;
            savefiledialog.InitialDirectory = CEResource.TXT_CDrive;
            savefiledialog.Filter = CEResource.TXT_ExtExportWorkSpaceFilterCfg;
            savefiledialog.FileName = workspace.ApplicationData.Name + "-" + workspace.ApplicationData.Workspace + "-" + workspace.ApplicationData.Workset + CEResource.TXT_ExtCfg;
            savefiledialog.RestoreDirectory = true;
            if (savefiledialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            path = savefiledialog.FileName;

            using (var streamWriter = new StreamWriter(path))
            {
                streamWriter.WriteLine("#" + '\n' + "#" + '\n' + "#");
                streamWriter.WriteLine("# Compiled workspace:");
                streamWriter.WriteLine("# Date: " + workspace.DateProcessed.ToString());
                streamWriter.WriteLine("# Application : " + workspace.ApplicationData.Name);
                streamWriter.WriteLine("# User : " + workspace.ApplicationData.Workspace);
                streamWriter.WriteLine("# Project : " + workspace.ApplicationData.Workset);
                streamWriter.WriteLine("#" + '\n' + "#" + '\n' + "#");

                for (t = -2; t <= 4; t++)
                {

                    // Output level change
                    if (t >= 0)
                    {
                        streamWriter.WriteLine("%level " + t);
                    }

                    // Output variables
                    foreach (CFGVariable currentVariable in workspace.Variables.Values)
                    {
                        variable = currentVariable;
                        if (variable.IsDefined && CultureInfo.CurrentCulture.CompareInfo.Compare(variable.Name, "MS_CONFIG", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0 && variable.Level != -1 && variable.Level == t)
                        {
                            temp = (Utilities.CleanupMultiVariableValue(variable.FinalExpansion) ?? "").Replace(@"\", "/");
                            streamWriter.WriteLine(variable.Name.ToUpperInvariant() + new string(' ', Math.Max(0, 60 - variable.Name.Length)) + "=" + '\t' + temp);
                        }
                    }
                }

                // Output locks
                foreach (CFGVariable currentVariable1 in workspace.Variables.Values)
                {
                    variable = currentVariable1;
                    if (variable.IsDefined && variable.IsLocked && CultureInfo.CurrentCulture.CompareInfo.Compare(variable.Name, "MS_CONFIG", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0 && variable.Level != -1)
                    {
                        streamWriter.WriteLine("%lock " + variable.Name);
                    }
                }
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Creates a "compiled" configuration file with all the final variable values
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static void SaveWorkspaceVariables(ref CFGConfiguration workspace)
        {
            if (workspace is null)
                return;

            string path;

            var savefiledialog = new System.Windows.Forms.SaveFileDialog();
            savefiledialog.Title = CEResource.TXT_ExportVariableListToTextFile;
            savefiledialog.InitialDirectory = CEResource.TXT_CDrive;
            savefiledialog.Filter = CEResource.TXT_FileFilterTxt;
            savefiledialog.FileName = workspace.ApplicationData.Name + "-" + workspace.ApplicationData.Workspace + "-" + workspace.ApplicationData.Workset + CEResource.TXT_ExtTxt;
            savefiledialog.RestoreDirectory = true;
            if (savefiledialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            path = savefiledialog.FileName.ToString();

            using (var streamWriter = new StreamWriter(path))
            {
                foreach (CFGVariable variable in workspace.Variables.Values)
                    streamWriter.WriteLine(variable.Name.ToUpperInvariant());
            }
        }

        private static void WriteWorkspaceJson(StreamWriter streamWriter, CFGConfiguration workspace)
        {
            streamWriter.WriteLine("{");
            WriteStringProperty(streamWriter, "schemaVersion", "1.0", 2);
            WriteStringProperty(streamWriter, "format", "BCE.WorkspaceAnalysis", 2);
            WriteStringProperty(streamWriter, "generatedAt", DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture), 2);
            WriteStringProperty(streamWriter, "dateProcessed", workspace.DateProcessed.ToString("o", CultureInfo.InvariantCulture), 2);
            WriteStringProperty(streamWriter, "description", workspace.Description, 2);
            WriteStringProperty(streamWriter, "description2", workspace.Description2, 2);

            streamWriter.WriteLine("  \"summary\": {");
            WriteNumberProperty(streamWriter, "variableCount", workspace.Variables.Count, 4);
            WriteNumberProperty(streamWriter, "fileCount", workspace.CFGFiles.Count, 4);
            WriteNumberProperty(streamWriter, "eventCount", workspace.CFGEvents.Count, 4);
            WriteNumberProperty(streamWriter, "errorCount", workspace.ErrorCount, 4);
            WriteNumberProperty(streamWriter, "warningCount", workspace.WarningCount, 4, false);
            streamWriter.WriteLine("  },");

            WriteApplicationJson(streamWriter, workspace.ApplicationData);
            WriteFilesJson(streamWriter, workspace);
            WriteVariablesJson(streamWriter, workspace);
            WriteEventsJson(streamWriter, workspace);
            streamWriter.WriteLine("}");
        }

        private static void WriteApplicationJson(StreamWriter streamWriter, CFGEnums.Application application)
        {
            streamWriter.WriteLine("  \"workspace\": {");
            WriteStringProperty(streamWriter, "name", application.Name, 4);
            WriteStringProperty(streamWriter, "path", application.Path, 4);
            WriteStringProperty(streamWriter, "startupCfgPath", application.StartupCfgPath, 4);
            WriteStringProperty(streamWriter, "commandOptions", application.CommandOptions, 4);
            WriteStringProperty(streamWriter, "workspace", application.Workspace, 4);
            WriteStringProperty(streamWriter, "workset", application.Workset, 4);
            WriteStringProperty(streamWriter, "role", application.Role, 4);
            WriteStringProperty(streamWriter, "version", application.Version, 4);
            WriteStringProperty(streamWriter, "configurationRoot", application.ConfigurationRoot, 4, false);
            streamWriter.WriteLine("  },");
        }

        private static void WriteFilesJson(StreamWriter streamWriter, CFGConfiguration workspace)
        {
            streamWriter.WriteLine("  \"files\": [");
            for (int index = 0; index < workspace.CFGFiles.Count; index++)
            {
                var cfgFile = workspace.CFGFiles[index] as CFGFile;
                streamWriter.WriteLine("    {");
                WriteNumberProperty(streamWriter, "uid", cfgFile?.UID ?? -1, 6);
                WriteStringProperty(streamWriter, "name", cfgFile?.Name, 6);
                WriteStringProperty(streamWriter, "path", cfgFile?.FilePath, 6);
                WriteNumberProperty(streamWriter, "depth", cfgFile?.Depth ?? 0, 6);
                WriteNumberProperty(streamWriter, "startLevel", cfgFile?.StartLevel ?? -999, 6);
                WriteNumberProperty(streamWriter, "currentLevel", cfgFile?.CurrentLevel ?? -999, 6);
                WriteStringProperty(streamWriter, "dateLastModified", cfgFile is null ? null : cfgFile.DateLastModified.ToString("o", CultureInfo.InvariantCulture), 6, false);
                streamWriter.Write("    }");
                if (index < workspace.CFGFiles.Count - 1)
                    streamWriter.Write(",");
                streamWriter.WriteLine();
            }
            streamWriter.WriteLine("  ],");
        }

        private static void WriteVariablesJson(StreamWriter streamWriter, CFGConfiguration workspace)
        {
            var variables = workspace.Variables.Values.OrderBy(variable => variable.Name, StringComparer.OrdinalIgnoreCase).ToList();

            streamWriter.WriteLine("  \"variables\": [");
            for (int index = 0; index < variables.Count; index++)
            {
                CFGVariable variable = variables[index];
                string finalExpansion = variable.FinalExpansion ?? variable.Expand();

                streamWriter.WriteLine("    {");
                WriteStringProperty(streamWriter, "name", variable.Name, 6);
                WriteNumberProperty(streamWriter, "level", variable.Level, 6);
                WriteStringProperty(streamWriter, "levelName", GetLevelName(variable.Level), 6);
                WriteBoolProperty(streamWriter, "isDefined", variable.IsDefined, 6);
                WriteBoolProperty(streamWriter, "isLocked", variable.IsLocked, 6);
                WriteBoolProperty(streamWriter, "needsSpecialExpansion", variable.NeedsSpecialExpansion, 6);
                WriteStringProperty(streamWriter, "value", variable.Value, 6);
                WriteStringProperty(streamWriter, "finalExpansion", finalExpansion, 6);
                streamWriter.WriteLine("      \"valuesByLevel\": {");
                for (int level = 0; level < variable.Values.Length; level++)
                {
                    WriteStringProperty(streamWriter, level.ToString(CultureInfo.InvariantCulture), variable.Values[level], 8, level < variable.Values.Length - 1);
                }
                streamWriter.WriteLine("      }");
                streamWriter.Write("    }");
                if (index < variables.Count - 1)
                    streamWriter.Write(",");
                streamWriter.WriteLine();
            }
            streamWriter.WriteLine("  ],");
        }

        private static void WriteEventsJson(StreamWriter streamWriter, CFGConfiguration workspace)
        {
            streamWriter.WriteLine("  \"events\": [");
            for (int index = 0; index < workspace.CFGEvents.Count; index++)
            {
                var cfgEvent = workspace.CFGEvents[index] as CFGEvent;
                streamWriter.WriteLine("    {");
                WriteNumberProperty(streamWriter, "uid", cfgEvent?.UID ?? -1, 6);
                WriteStringProperty(streamWriter, "eventType", cfgEvent?.EventType.ToString(), 6);
                WriteStringProperty(streamWriter, "description", cfgEvent?.Description, 6);
                WriteStringProperty(streamWriter, "variable", cfgEvent?.VarName, 6);
                WriteStringProperty(streamWriter, "value", cfgEvent?.VarValue, 6);
                WriteStringProperty(streamWriter, "expansion", cfgEvent?.VarExpansion, 6);
                WriteNumberProperty(streamWriter, "level", cfgEvent?.Level ?? -999, 6);
                WriteStringProperty(streamWriter, "levelName", cfgEvent is null ? null : GetLevelName(cfgEvent.Level), 6);
                WriteStringProperty(streamWriter, "file", cfgEvent?.ParentFile?.FilePath, 6);
                WriteNumberProperty(streamWriter, "line", cfgEvent?.ParentLine?.LineNumber ?? 0, 6);
                WriteStringProperty(streamWriter, "lineText", cfgEvent?.ParentLine?.Text, 6, false);
                streamWriter.Write("    }");
                if (index < workspace.CFGEvents.Count - 1)
                    streamWriter.Write(",");
                streamWriter.WriteLine();
            }
            streamWriter.WriteLine("  ]");
        }

        private static string GetLevelName(int level)
        {
            return level > -999 ? Utilities.GetLevelExtendedName(level) : string.Empty;
        }

        private static void WriteStringProperty(StreamWriter streamWriter, string name, string value, int indent, bool comma = true)
        {
            streamWriter.Write(new string(' ', indent));
            streamWriter.Write('"');
            streamWriter.Write(EscapeJson(name));
            streamWriter.Write("\": ");
            if (value is null)
            {
                streamWriter.Write("null");
            }
            else
            {
                streamWriter.Write('"');
                streamWriter.Write(EscapeJson(value));
                streamWriter.Write('"');
            }
            if (comma)
                streamWriter.Write(',');
            streamWriter.WriteLine();
        }

        private static void WriteNumberProperty(StreamWriter streamWriter, string name, int value, int indent, bool comma = true)
        {
            streamWriter.Write(new string(' ', indent));
            streamWriter.Write('"');
            streamWriter.Write(EscapeJson(name));
            streamWriter.Write("\": ");
            streamWriter.Write(value.ToString(CultureInfo.InvariantCulture));
            if (comma)
                streamWriter.Write(',');
            streamWriter.WriteLine();
        }

        private static void WriteBoolProperty(StreamWriter streamWriter, string name, bool value, int indent, bool comma = true)
        {
            streamWriter.Write(new string(' ', indent));
            streamWriter.Write('"');
            streamWriter.Write(EscapeJson(name));
            streamWriter.Write("\": ");
            streamWriter.Write(value ? "true" : "false");
            if (comma)
                streamWriter.Write(',');
            streamWriter.WriteLine();
        }

        private static string EscapeJson(string value)
        {
            if (value is null)
                return string.Empty;

            var builder = new StringBuilder(value.Length + 16);
            foreach (char ch in value)
            {
                switch (ch)
                {
                    case '\\':
                        builder.Append("\\\\");
                        break;
                    case '"':
                        builder.Append("\\\"");
                        break;
                    case '\b':
                        builder.Append("\\b");
                        break;
                    case '\f':
                        builder.Append("\\f");
                        break;
                    case '\n':
                        builder.Append("\\n");
                        break;
                    case '\r':
                        builder.Append("\\r");
                        break;
                    case '\t':
                        builder.Append("\\t");
                        break;
                    default:
                        if (char.IsControl(ch))
                        {
                            builder.Append("\\u");
                            builder.Append(((int)ch).ToString("x4", CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            builder.Append(ch);
                        }
                        break;
                }
            }

            return builder.ToString();
        }

    }
}
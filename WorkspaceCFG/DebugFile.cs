// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace WorkspaceCFG
{

    public class DebugFile
    {
        // FIX: Use List<DebugVariable> instead of Collection
        public List<DebugVariable> Variables = new List<DebugVariable>();
        public string Path;

        private readonly Regex lineRegex = new Regex("(?<VARIABLE>[^:]*):(?<LEVEL>[^=]*)=(?<VALUE>.*)", RegexOptions.Singleline | RegexOptions.Compiled);

        public DebugFile()
        {
        }

        public void Compare(CFGConfiguration workspace)
        {
            if (workspace is null)
                return;

            string filepath = Path.Replace(".txt", "_comparison.txt");
            using (var streamWriter = new StreamWriter(filepath))
            {

                streamWriter.WriteLine("Variables found by the product but not BCE");
                streamWriter.WriteLine("-------------------------------------------");
                streamWriter.WriteLine(string.Format("  {0,-45}{1,-20}{2,-30}{3,-45}", "Variable Name", "Level", "Note", "Value"));
                streamWriter.WriteLine(string.Format("  {0,-45}{1,-20}{2,-30}{3,-45}", "--------------------------", "----------", "----------", "--------------------------"));
                foreach (DebugVariable prodVar in Variables)
                {
                    var bceVar = workspace.GetVariable(prodVar.Name);
                    if (bceVar is null)
                    {
                        streamWriter.WriteLine(string.Format("  {0,-45}{1,-20}{2,-30}{3,-45}", prodVar.Name, prodVar.Level, prodVar.Note, prodVar.RawValue));
                    }
                }
                streamWriter.WriteLine("");

                streamWriter.WriteLine("Variables found by BCE but not the product:");
                streamWriter.WriteLine("-------------------------------------------");
                foreach (CFGVariable bceVar in workspace.Variables.Values)
                {
                    // FIX: Use Any() to check if the variable name exists in Variables
                    if (!Variables.Exists(v => v.Name == bceVar.Name))
                    {
                        streamWriter.WriteLine("  " + bceVar.Name);
                    }
                }
                streamWriter.WriteLine("");

                streamWriter.WriteLine("Variable values that differ:");
                streamWriter.WriteLine("-------------------------------------------");
                foreach (DebugVariable prodVar in Variables)
                {
                    var bceVar = workspace.GetVariable(prodVar.Name);
                    if (bceVar is not null)
                    {
                        if (CultureInfo.CurrentCulture.CompareInfo.Compare(prodVar.Value ?? "", bceVar.FinalExpansion ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
                        {
                            bceVar.Expand();
                            streamWriter.WriteLine("  " + prodVar.Name);
                            streamWriter.WriteLine("    [prod] " + prodVar.Value);
                            streamWriter.WriteLine("    [bce ] " + bceVar.FinalExpansion);
                        }
                    }
                }
                streamWriter.WriteLine("");

                streamWriter.WriteLine("Variable lock values that differ:");
                streamWriter.WriteLine("-------------------------------------------");
                foreach (DebugVariable prodVar in Variables)
                {
                    var bceVar = workspace.GetVariable(prodVar.Name);
                    if (bceVar is not null)
                    {
                        if (prodVar.Locked != bceVar.IsLocked)
                        {
                            streamWriter.WriteLine("  " + prodVar.Name);
                            streamWriter.WriteLine("    [prod] " + prodVar.Locked.ToYesNoString());
                            streamWriter.WriteLine("    [bce ] " + bceVar.IsLocked.ToYesNoString());
                        }
                    }
                }

            }

            Utilities.OpenInEditor(ref filepath);

        }

        public void LoadFile(string filepath)
        {
            Path = filepath;

            try
            {
                using (var reader = new StreamReader(filepath))
                {
                    string line;
                    Match match;
                    bool locked;

                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();

                        // TODO: grabbing lock status directly until added to regex
                        if (line.Contains("<Locked>"))
                        {
                            line = line.Replace("<Locked>", "");
                            locked = true;
                        }
                        else
                        {
                            locked = false;
                        }

                        line = line.Trim();

                        match = lineRegex.Match(line);
                        if (match.Success)
                        {
                            var debugVar = new DebugVariable(match.Groups[1].Value.ToString().Trim(), match.Groups[2].Value.ToString().Trim(), match.Groups[3].Value.ToString().Trim(), locked);
                            Variables.Add(debugVar);
                        }
                        else
                        {
                            // FIX: Use Variables.Count - 1 for last element in List
                            if (Variables.Count > 0)
                            {
                                DebugVariable lastVar = Variables[Variables.Count - 1];
                                if (lastVar is not null)
                                {
                                    lastVar.Value += line;
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
        }

    }

    public class DebugVariable
    {

        public string Name;
        public string Level;
        public string Value;
        public string RawValue;
        public bool Locked;

        public string Note;
        public bool Ignore;

        public DebugVariable(string name, string level, string value, bool locked)
        {
            Name = name;
            Level = level;
            Value = value;
            RawValue = value;
            Locked = locked;

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(Value, "(null)", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                Value = "";
            }

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(Name, "_DGNDIR", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                Note = "Current File Directory";
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(Name, "_DGNFILE", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                Note = "Current File Name";
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(Name, "_EMBEDFILE", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                Note = "Embeded File Name";
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(Name, "_PACKAGEFILE", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                Note = "Package File Name";
        }

    }
}
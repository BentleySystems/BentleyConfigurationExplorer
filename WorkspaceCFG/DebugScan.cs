// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace WorkspaceCFG
{

    static class DebugScan
    {
        private readonly static Regex varlineRegx = new Regex(@"\((?<LEVEL>predfined|system|application|project|site|user)\):(?<VARNAME>[^=]*)=(?<VALUE>.* )\[(?<EXPANSION>.*)]", RegexOptions.Singleline | RegexOptions.Compiled);
        private readonly static Regex VarDefRegx = new Regex("(?<VARIABLE>[^:]*):(?<OPERATOR>[^=]*)=(?<VALUE>.*)", RegexOptions.Singleline | RegexOptions.Compiled);
        private readonly static Regex CFGStartRegx = new Regex(@"Processing macro file \[(?<CFGFILE>[^\]]*)", RegexOptions.Singleline | RegexOptions.Compiled);
        private readonly static Regex CFGEndRegx = new Regex(@"End of macro file \[(?<CFGFILE>[^\]]*)", RegexOptions.Singleline | RegexOptions.Compiled);

        // Helper for VB string compatibility
        private static class VBCompat
        {
            public static string Trim(string s) => s?.Trim() ?? string.Empty;
            public static string Replace(string s, string oldValue, string newValue) => s?.Replace(oldValue, newValue) ?? string.Empty;
        }

        public static CFGConfiguration LoadMsDebug(ref string path)
        {
            int mode = 1;
            var cfgWorkspace = new CFGConfiguration();

            {
                ref var withBlock = ref cfgWorkspace;
                withBlock.Description = UtilitiesPath.GetFileName(path);
                withBlock.Description2 = withBlock.Description;
                withBlock.IsFromFile = true;
                withBlock.DateProcessed = DateTime.Now;
            }

            try
            {
                using (var reader = new StreamReader(path))
                {
                    string line;
                    CFGVariable cfgVariable;
                    CFGFile cfgFile;
                    CFGFile currentfile = null;
                    Match match;

                    while (!reader.EndOfStream)
                    {
                        // FIX: Use VBCompat.Trim and VBCompat.Replace instead of Strings.* and remove CompareMethod
                        line = VBCompat.Trim(VBCompat.Replace(reader.ReadLine(), "/", @"\"));

                        if (line.IndexOf("Configuration Variable Summary", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            mode = 2;
                        }

                        if (mode == 1)
                        {
                            // Look for new cfg files being processed
                            match = CFGStartRegx.Match(line);
                            if (match.Success)
                            {
                                cfgFile = new CFGFile(ref cfgWorkspace);
                                if (currentfile is null)
                                {
                                    currentfile = cfgFile;
                                    cfgFile.Depth = 0;
                                }
                                else
                                {
                                    currentfile.ChildFiles.Add(cfgFile);
                                    cfgFile.Depth = currentfile.Depth + 1;
                                }

                                cfgFile.ParentFile = currentfile;
                                cfgFile.FilePath = VBCompat.Trim(match.Groups[1].Value.ToString());
                                string argpath = UtilitiesPath.GetFileName(cfgFile.FilePath);
                                cfgFile.Name = UtilitiesPath.GetPathWithoutExtension(ref argpath);
                                currentfile = cfgFile;
                                cfgWorkspace.CFGFiles.Add(cfgFile);
                            }

                            // Look for config files to end
                            match = CFGEndRegx.Match(line);
                            if (match.Success)
                            {
                                if (currentfile is not null)
                                {
                                    currentfile = currentfile.ParentFile;
                                }
                            }
                        }

                        // Read the variable summary
                        if (mode == 2)
                        {
                            match = VarDefRegx.Match(line);

                            if (match.Success)
                            {
                                cfgVariable = new CFGVariable(cfgWorkspace)
                                {
                                    IsDefined = true,
                                    Name = VBCompat.Trim(match.Groups[1].Value),
                                    Level = Utilities.GetLevelNumber(VBCompat.Trim(match.Groups[2].Value)),
                                    Value = VBCompat.Trim(match.Groups[3].Value)
                                };
                                cfgVariable.Value = VBCompat.Replace(cfgVariable.Value, "/", @"\");

                                if (cfgVariable.Value.IndexOf("(null)", StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    cfgVariable.Value = string.Empty;
                                }

                                if (cfgVariable.Value.IndexOf("<locked>", StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    cfgVariable.IsLocked = true;
                                    cfgVariable.Value = Regex.Replace(cfgVariable.Value, "<locked>", "", RegexOptions.IgnoreCase).Trim();
                                }

                                if (cfgVariable.Level > 0)
                                {
                                    cfgVariable.Values[cfgVariable.Level] = cfgVariable.Value;
                                }
                                else
                                {
                                    cfgVariable.Values[0] = cfgVariable.Value;
                                }

                                cfgVariable.SetFinalExpansion();
                                cfgWorkspace.Variables.Add(cfgVariable.Name, cfgVariable);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] DebugScan.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                // TODO: do something with the error
            }

            return cfgWorkspace;
        }

    }
}
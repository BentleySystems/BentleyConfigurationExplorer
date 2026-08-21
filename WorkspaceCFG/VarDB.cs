// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
namespace WorkspaceCFG
{


    [Serializable]
    public class VarDB
    {

        public static List<CFGEnums.VarInfo> VariableDB;
        public static List<CFGEnums.VarInfo> VariableDBXML;

        // ---------------------------------------------------------------------------------------
        // @description: Lazily loads VariableDB from the standard ProgramData location if it
        // hasn't been populated yet. Needed because VariableDB is normally populated as a side
        // effect of ProcessConfiguration(), which doesn't run when loading a saved .bcf file
        // directly - without this, any Utilities.GetVariable* lookup throws ArgumentNullException.
        // ---------------------------------------------------------------------------------------
        public static void EnsureLoaded()
        {
            if (VariableDB is not null)
                return;

            string programdataDir = Environment.ExpandEnvironmentVariables("%PROGRAMDATA%");
            string CEVariableCSVsDir = Path.Combine(programdataDir, "Bentley", "ConfigurationExplorer", "VariableCSVs");

            try
            {
                GetVariableDBCSV(CEVariableCSVsDir);
            }
            catch
            {
                VariableDB ??= new List<CFGEnums.VarInfo>();
            }
        }

        public static void GetVariableDBCSV(string CEVariableCSVsDir)
        {

            try
            {
                if (!Directory.Exists(CEVariableCSVsDir))
                {
                    // try to create the directory
                    Directory.CreateDirectory(CEVariableCSVsDir);
                }
            }
            catch
            {
                throw;
            }

            VariableDB = new List<CFGEnums.VarInfo>();

            string VariableDBCSVFile = Path.Combine(CEVariableCSVsDir, "variables.csv");

            if (!File.Exists(VariableDBCSVFile))
            {
                // try to create the file
                string ExeDir = Utilities.GetExeFolder();
                string SeedVariblesFile = Path.Combine(ExeDir, "Config", "variables.csv");
                //System.Windows.Forms.MessageBox.Show("Bentley Configuration Explorer Install Dir:" + ExeDir);
                File.Copy(SeedVariblesFile, VariableDBCSVFile, true);
            }

            Load_VarDBCSV(VariableDBCSVFile);


        }

        public static void Load_VarDBCSV(string VariableDBCSVFile)
        {

            CFGEnums.VarInfo entry;
            string[] fields;
            string readline;

            try
            {
                StreamReader oRead;
                oRead = File.OpenText(VariableDBCSVFile);
                string unused1 = (oRead.ReadLine() ?? "").Trim();

                while (!oRead.EndOfStream)
                {
                    readline = (oRead.ReadLine() ?? "").Trim();
                    fields = (readline ?? "").Split(',');
                    if (fields.Length > 0)
                    {
                        entry = new CFGEnums.VarInfo();
                        if (fields.Length >= 1)
                        {
                            entry.Name = fields[0];
                        }
                        else
                        {
                            Console.WriteLine();
                        }
                        if (fields.Length >= 2)
                            entry.Type = fields[1];
                        if (fields.Length >= 3)
                            entry.ShortDesc = fields[2];
                        if (fields.Length >= 4)
                            entry.LongDesc = fields[3];
                        if (fields.Length >= 5)
                            entry.Restart = fields[4];
                        if (fields.Length >= 6)
                            entry.Project = fields[5];
                        if (fields.Length >= 7)
                            entry.Version = fields[6];
                        if (fields.Length >= 8)
                            entry.Category = fields[7];
                        if (fields.Length >= 9)
                            entry.App = fields[8];
                        if (fields.Length >= 10)
                            entry.Comment = fields[9];
                        if (fields.Length >= 11)
                            entry.Status = fields[10];
                        
                        if (!VariableDB.Contains(entry))
                            VariableDB.Add(entry);
                    }
                }
            }
            catch
            {

            }
            VarDB.Save();
        }
        public static void GetVariableDBXML(string CEVariableXMLsDir)
        {
            try
            {
                if (!Directory.Exists(CEVariableXMLsDir))
                {
                    // try to create the directory
                    Directory.CreateDirectory(CEVariableXMLsDir);
                }
            }
            catch
            {
                throw;
            }
            VariableDBXML = new List< CFGEnums.VarInfo>();

            var VariableDBXMLx = new VarDB();

            string VariableDBXMLFile = Path.Combine(CEVariableXMLsDir, "variables.xml");

            if (!File.Exists(VariableDBXMLFile))
            {
                // try to create the file
                string ExeDir = Utilities.GetExeFolder();
                string SeedVariblesFile = Path.Combine(ExeDir, "Config", "variables.xml");
                // MsgBox("Bentley Configuration Explorer Install Dir:" & ExeDir)
                File.Copy(SeedVariblesFile, VariableDBXMLFile, true);
            }


            // If Not VariableDB.Contains(DBEntry.Name) Then VariableDB.Add(Entry, Entry.Name)
            // VariableDBXMLx = LoadVarDBXML(VariableDBXMLFile)


        }


        private static string GetFileFullPath()
        {
            string ProgramDataDir = Environment.ExpandEnvironmentVariables("%PROGRAMDATA%");

            if (ProgramDataDir.Equals("%PROGRAMDATA%"))
            {
                ProgramDataDir = Environment.ExpandEnvironmentVariables("%PROGRAMDATA%");
            }

            string fileFullPath = Path.Combine(
                ProgramDataDir,
                "Bentley",
                "ConfigurationExplorer",
                "VariableXMLs",
                "Variables.xml");

            return fileFullPath;
        }

        private static CFGEnums.VarInfo LoadVarDBXML(string fileFullPath)
        {

            if (!File.Exists(fileFullPath))
            {
                XmlSerialization.SerializeToFile(new CFGEnums.VarInfo(), fileFullPath);
            }

            return XmlSerialization.DeserializeFromFile<CFGEnums.VarInfo>(fileFullPath);


        }

        public static Dictionary<string, CFGEnums.VarInfo> Load()
        {
            string fileFullPath = GetFileFullPath();

            if (!File.Exists(fileFullPath))
            {
                XmlSerialization.SerializeToFile(new CESettings(), fileFullPath);
            }

            return XmlSerialization.DeserializeFromFile<Dictionary<string, CFGEnums.VarInfo>>(fileFullPath);
        }

        public static void Save()
        {
            // updates with last app
            XmlSerialization.SerializeToFile(VariableDB, GetFileFullPath());
        }


    }
}
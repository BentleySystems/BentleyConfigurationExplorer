// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace WorkspaceCFG
{
    public class CFGVariableValidater
    {
        private static readonly Regex _valRegx = new Regex(@"(?i:(?<VARNAME>.*)(?<OPT1>SHOULD_NOT|SHOULD|MUST_NOT|MUST)[\s]*(?<OPT2>[^\s]*)[\s]*(?<VL>.*))", RegexOptions.Singleline | RegexOptions.Compiled);
        private readonly CFGConfiguration _configuration;

        public List<CFGEnums.ValidationResult> ResultCol;

        public CFGVariableValidater(ref CFGConfiguration inwrkspc)
        {
            ResultCol = new List<CFGEnums.ValidationResult>();
            _configuration = inwrkspc;
        }

        public void ProcessValidationRule(string validationrule)
        {
            string argfilepath = Utilities.GetExeFolder() + @"Rules\" + validationrule + ".txt";
            ProcessValidationFile(argfilepath, validationrule);
        }

        private void ProcessValidationFile(string filepath, string validationrule)
        {
            if (_configuration is null)
                return;

            try
            {
                if (!File.Exists(filepath))
                    return;

                using (var reader = new StreamReader(filepath))
                {
                    string line;
                    int lineNumber = 0;
                    Match match;
                    CFGEnums.ValidationResult result;

                    while (!reader.EndOfStream)
                    {
                        lineNumber++;
                        line = reader.ReadLine()?.Replace("\t", " ").Trim();
                        if (!string.IsNullOrEmpty(line) && !line.StartsWith("#"))
                        {
                            match = _valRegx.Match(line);
                            if (match.Success)
                            {
                                result = new CFGEnums.ValidationResult()
                                {
                                    variablename = match.Groups["VARNAME"].Value.Trim().ToUpperInvariant(),
                                    opt = match.Groups["OPT1"].Value.Trim().ToUpperInvariant(),
                                    attr = match.Groups["OPT2"].Value.Trim().ToUpperInvariant(),
                                    inval = match.Groups["VL"].Value.Trim(),
                                    rulename = validationrule,
                                    linenumber = lineNumber.ToString()
                                };
                                ProcessRule(ref result);
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

        private void ProcessRule(ref CFGEnums.ValidationResult result)
        {
            string[] mis;
            CFGVariable cfgVariable;
            CFGEnums.ValidationResult tempResult;

            cfgVariable = _configuration.GetVariable(result.variablename);

            result.negation = result.opt.EndsWith("_NOT", StringComparison.OrdinalIgnoreCase);
            result.iswarning = result.opt.StartsWith("SHOULD", StringComparison.OrdinalIgnoreCase);
            result.iserror = !result.iswarning;

            if (cfgVariable is null)
            {
                if (string.Equals(result.attr, "BE_DEFINED", StringComparison.OrdinalIgnoreCase))
                {
                    result.alert = !result.negation;
                    ResultCol.Add(result);
                }
                return;
            }

            result.varval = cfgVariable.FinalExpansion;
            switch (result.attr ?? "")
            {
                case var attr when string.Equals(attr, "BE_DEFINED", StringComparison.OrdinalIgnoreCase):
                    ProcessBE_DEFINED(ref cfgVariable, ref result);
                    return;
                case var attr when string.Equals(attr, "BE_LOCKED", StringComparison.OrdinalIgnoreCase):
                    ProcessBE_LOCKED(ref cfgVariable, ref result);
                    return;
                case var attr when string.Equals(attr, "HAVE_ONE_VALUE", StringComparison.OrdinalIgnoreCase):
                    ProcessHAS_ONE_VALUE(ref cfgVariable, ref result);
                    return;
                case var attr when string.Equals(attr, "BE_LIKE", StringComparison.OrdinalIgnoreCase) ||
                                   string.Equals(attr, "BE_NOT_LIKE", StringComparison.OrdinalIgnoreCase):
                    ProcessBE_LIKE(ref cfgVariable, ref result);
                    return;
                case var attr when string.Equals(attr, "BE_BLANK", StringComparison.OrdinalIgnoreCase):
                    ProcessBE_BLANK(ref cfgVariable, ref result);
                    return;
            }

            mis = cfgVariable.FinalExpansion.Split(';');

            foreach (string value in mis)
            {
                tempResult = (CFGEnums.ValidationResult)result.Clone();
                tempResult.varval = value;

                ProcessFolderFileAttributes(ref tempResult, value);

                switch (result.attr ?? "")
                {
                    case var attr when string.Equals(attr, "BE_FOLDER", StringComparison.OrdinalIgnoreCase):
                        ProcessBE_FOLDER(ref tempResult);
                        break;
                    case var attr when string.Equals(attr, "BE_FILE", StringComparison.OrdinalIgnoreCase):
                        ProcessBE_FILE(ref tempResult);
                        break;
                    case var attr when string.Equals(attr, "BE_FILE_OR_FOLDER", StringComparison.OrdinalIgnoreCase):
                        ProcessBE_FILE_OR_FOLDER(ref tempResult);
                        break;
                    case var attr when string.Equals(attr, "BE_NUMBER", StringComparison.OrdinalIgnoreCase):
                        ProcessBE_NUMBER(ref tempResult, value);
                        break;
                    case var attr when string.Equals(attr, "HAVE_FOLDER_EXIST", StringComparison.OrdinalIgnoreCase):
                        ProcessHAVE_FOLDER_EXIST(ref tempResult);
                        break;
                    case var attr when string.Equals(attr, "HAVE_FILE_EXIST", StringComparison.OrdinalIgnoreCase):
                        ProcessHAVE_FILE_EXIST(ref tempResult);
                        break;
                    case var attr when string.Equals(attr, "HAVE_FILE_OR_FOLDER_EXIST", StringComparison.OrdinalIgnoreCase):
                        ProcessHAVE_FILE_OR_FOLDER_EXIST(ref tempResult);
                        break;
                    case var attr when string.Equals(attr, "HAVE_WILDCARD_CHARACTER", StringComparison.OrdinalIgnoreCase):
                        ProcessHAVE_WILDCARD_CHARACTER(ref tempResult, value);
                        break;
                    case var attr when string.Equals(attr, "BE_EQUAL_TO", StringComparison.OrdinalIgnoreCase) ||
                                       string.Equals(attr, "BE_NOT_EQUAL_TO", StringComparison.OrdinalIgnoreCase):
                        ProcessBE_EQUAL_TO(ref tempResult, value);
                        break;
                }
            }
        }

        private void ProcessFolderFileAttributes(ref CFGEnums.ValidationResult result, string value)
        {
            result.filefoldertest = true;

            if (UtilitiesPath.IsPath(ref value))
            {
                result.isFolder = UtilitiesPath.IsFolder(ref value);
                result.folderExists = UtilitiesPath.FolderExists(UtilitiesPath.GetDirectoryName(ref value));
                result.isFile = UtilitiesPath.IsFile(ref value);
                result.fileExists = result.isFile && File.Exists(value);
                if (result.folderExists && value.Contains("*"))
                    result.fileExists = UtilitiesPath.WildCardFilesExist(ref value);
            }
            else
            {
                result.isFile = false;
                result.isFolder = false;
                result.fileExists = false;
                result.folderExists = false;
            }
        }

        private void ProcessBE_DEFINED(ref CFGVariable @var, ref CFGEnums.ValidationResult result)
        {
            result.alert = !(@var.IsDefined ^ result.negation);
            ResultCol.Add(result);
        }

        private void ProcessBE_LOCKED(ref CFGVariable @var, ref CFGEnums.ValidationResult result)
        {
            result.alert = !(@var.IsLocked ^ result.negation);
            ResultCol.Add(result);
        }

        private void ProcessHAS_ONE_VALUE(ref CFGVariable @var, ref CFGEnums.ValidationResult result)
        {
            result.alert = !(@var.NumberOfValues() == 1 ^ result.negation);
            ResultCol.Add(result);
        }

        private void ProcessBE_LIKE(ref CFGVariable @var, ref CFGEnums.ValidationResult result)
        {
            var alert = false;
            string[] mis;

            mis = result.inval.Split('|');

            foreach (string value in mis)
            {
                bool containsValue = @var.FinalExpansion.IndexOf(value.Trim(), StringComparison.OrdinalIgnoreCase) >= 0;
                alert |= !(containsValue ^ result.negation);
            }

            if (string.Equals(result.attr, "BE_NOT_LIKE", StringComparison.OrdinalIgnoreCase))
                alert = !alert;

            result.alert = alert;

            ResultCol.Add(result);
        }

        private void ProcessBE_BLANK(ref CFGVariable @var, ref CFGEnums.ValidationResult result)
        {
            result.alert = !(string.IsNullOrEmpty(@var.FinalExpansion) ^ result.negation);
            ResultCol.Add(result);
        }

        private void ProcessBE_FOLDER(ref CFGEnums.ValidationResult result)
        {
            result.alert = !(result.isFolder & !result.isFile ^ result.negation);
            ResultCol.Add(result);
        }

        private void ProcessBE_FILE(ref CFGEnums.ValidationResult result)
        {
            result.alert = !(result.isFile ^ result.negation);
            ResultCol.Add(result);
        }

        private void ProcessBE_FILE_OR_FOLDER(ref CFGEnums.ValidationResult result)
        {
            result.alert = !((result.isFile | result.isFolder) ^ result.negation);
            ResultCol.Add(result);
        }

        private void ProcessBE_NUMBER(ref CFGEnums.ValidationResult result, string value)
        {
            // Remove Strings.Trim and Conversion.Val, use double.TryParse
            double numericValue;
            bool isNumber = double.TryParse(value.Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out numericValue);
            bool isZero = string.Equals(value.Trim(), "0", StringComparison.OrdinalIgnoreCase);
            result.alert = !((isZero || (isNumber && Math.Abs(numericValue) > 0d)) ^ result.negation);
            ResultCol.Add(result);
        }

        private void ProcessBE_EQUAL_TO(ref CFGEnums.ValidationResult result, string value)
        {
            bool alert;
            string[] mis;
            bool passed;

            mis = result.inval.Split('|');

            passed = false;
            foreach (string nvalue in mis)
            {
                if (string.Equals(nvalue.Trim().ToLowerInvariant(), "(null)", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(value))
                {
                    passed = true;
                    break;
                }
                else if (string.Compare(nvalue, value, true, CultureInfo.CurrentCulture) == 0)
                {
                    passed = true;
                    break;
                }
            }

            alert = passed ^ !result.negation;

            if (string.Equals(result.attr, "BE_NOT_EQUAL", StringComparison.OrdinalIgnoreCase))
                alert = !alert;

            result.alert = alert;

            ResultCol.Add(result);
        }

        private void ProcessHAVE_FOLDER_EXIST(ref CFGEnums.ValidationResult result)
        {
            result.alert = !(result.folderExists & !result.isFile ^ result.negation);
            ResultCol.Add(result);
        }

        private void ProcessHAVE_FILE_EXIST(ref CFGEnums.ValidationResult result)
        {
            result.alert = !(result.fileExists ^ result.negation);
            ResultCol.Add(result);
        }

        private void ProcessHAVE_WILDCARD_CHARACTER(ref CFGEnums.ValidationResult result, string value)
        {
            result.alert = !(value.Contains("*") ^ result.negation);
            ResultCol.Add(result);
        }

        private void ProcessHAVE_FILE_OR_FOLDER_EXIST(ref CFGEnums.ValidationResult result)
        {
            result.alert = !((result.isFile & result.fileExists | result.isFolder & result.folderExists) ^ result.negation);
            ResultCol.Add(result);
        }

        public void CreateEventsFromValidationResults()
        {
            if (_configuration is null)
                return;

            CFGEnums.CFGEventType eventtype;
            string description;

            foreach (CFGEnums.ValidationResult result in ResultCol)
            {
                if (result.alert)
                {
                    if (result.iserror)
                    {
                        eventtype = CFGEnums.CFGEventType.cfgError;
                    }
                    else
                    {
                        eventtype = CFGEnums.CFGEventType.cfgWarning;
                    }

                    description = "Variable Validation Alert: " + result.variablename + (" " + result.opt + " " + result.attr).Replace("_", " ") + " " + result.inval;
                    CFGLine argline = null;
                    CFGFile argfile = null;
                    var argvariable = _configuration.GetVariable(result.variablename);
                    _configuration.AddEvent(ref eventtype, ref description, argline, argfile, argvariable, result.variablename);
                }
            }
        }
    }
}
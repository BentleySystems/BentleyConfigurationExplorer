// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using WorkspaceCFG.My.Resources;
using static WorkspaceCFG.CFGEnums;
using static WorkspaceCFG.CFGFile;

namespace WorkspaceCFG
{

    public interface ILogger
    {
        void LogLine(string message);
    }

    public class MacroParser
    {
        private readonly ILogger _logger;
        private readonly CFGFile _file;
        private readonly CFGConfiguration _workspace;
        private readonly IEnumerator<CFGLine> _lines;

        private IEnumerator<CFGToken> _tokens;
        private int _depth;                       // Tracks recursion depth
        private List<string> _parentList;
        private CFGLine _currentLine;                 // current line being processed
        private bool _variableNeedsExpansion;
        private bool _currentVarRefUsed;
        private CFGToken _lastOperator;
        private bool _inVar = false;
        private Stack _ifStack;
        private bool _inIf;
        public bool debugLineOutput = false;

        // Replacement for Microsoft.VisualBasic.Conversion.Val: lenient parse of the leading
        // numeric portion of a string, returning 0 if no valid number is found (matches VB Val() semantics).
        private static double VBVal(string s)
        {
            if (string.IsNullOrEmpty(s))
                return 0d;

            int i = 0;
            int len = s.Length;
            while (i < len && char.IsWhiteSpace(s[i]))
                i++;

            int start = i;
            if (i < len && (s[i] == '+' || s[i] == '-'))
                i++;

            int digitsStart = i;
            while (i < len && char.IsDigit(s[i]))
                i++;

            bool sawDigits = i > digitsStart;

            if (i < len && s[i] == '.')
            {
                i++;
                int fracStart = i;
                while (i < len && char.IsDigit(s[i]))
                    i++;
                sawDigits = sawDigits || i > fracStart;
            }

            if (sawDigits && i < len && (s[i] == 'e' || s[i] == 'E'))
            {
                int expMark = i;
                int j = i + 1;
                if (j < len && (s[j] == '+' || s[j] == '-'))
                    j++;
                int expDigitsStart = j;
                while (j < len && char.IsDigit(s[j]))
                    j++;
                if (j > expDigitsStart)
                    i = j;
                else
                    i = expMark;
            }

            if (!sawDigits)
                return 0d;

            string numStr = s.Substring(start, i - start);
            return double.TryParse(numStr, NumberStyles.Float, CultureInfo.InvariantCulture, out var result) ? result : 0d;
        }

        // Replacement for Microsoft.VisualBasic.Conversion.Int: truncates toward negative infinity.
        private static double VBInt(double d) => Math.Floor(d);

        // Replacements for Microsoft.VisualBasic.Strings members used throughout this parser.
        private static string VBUCase(string s) => s?.ToUpper(CultureInfo.CurrentCulture);

        private static string VBLCase(string s) => s?.ToLower(CultureInfo.CurrentCulture);

        // 1-based start, to end of string (matches VB Mid$(s, start)).
        private static string VBMid(string s, int start)
        {
            if (string.IsNullOrEmpty(s) || start > s.Length)
                return "";
            int zeroStart = Math.Max(start - 1, 0);
            return s.Substring(zeroStart);
        }

        // 1-based start with explicit length, clamped to the string bounds (matches VB Mid$(s, start, length)).
        private static string VBMid(string s, int start, int length)
        {
            if (string.IsNullOrEmpty(s) || start > s.Length || length <= 0)
                return "";
            int zeroStart = Math.Max(start - 1, 0);
            int maxLen = s.Length - zeroStart;
            int len = Math.Min(length, maxLen);
            return s.Substring(zeroStart, len);
        }

        // Case-insensitive literal replace (matches VB Strings.Replace with Compare:=CompareMethod.Text).
        private static string VBReplace(string s, string find, string replacement)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(find))
                return s;
            return System.Text.RegularExpressions.Regex.Replace(s, System.Text.RegularExpressions.Regex.Escape(find),
                replacement.Replace("$", "$$"), System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }

        // Matches VB Strings.Split with Compare:=CompareMethod.Text.
        private static string[] VBSplit(string s, string delimiter)
        {
            if (s is null)
                return new string[] { "" };
            return s.Split(new[] { delimiter }, StringSplitOptions.None);
        }

        // Matches VB Strings.InStr with Compare:=CompareMethod.Text (1-based index, 0 if not found).
        private static int VBInStr(string s, string substr)
        {
            if (string.IsNullOrEmpty(s))
                return 0;
            int idx = s.IndexOf(substr, StringComparison.OrdinalIgnoreCase);
            return idx < 0 ? 0 : idx + 1;
        }

        // Replacement for Microsoft.VisualBasic.Information.IsNumeric.
        private static bool VBIsNumeric(object value)
        {
            if (value is null)
                return false;
            string s = value as string ?? Convert.ToString(value, CultureInfo.CurrentCulture);
            if (string.IsNullOrWhiteSpace(s))
                return false;
            return double.TryParse(s.Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out _);
        }

        // Replacement for Microsoft.VisualBasic.Information.UBound (1D arrays).
        private static int VBUBound(Array arr) => arr.Length - 1;

        // Replacement for Microsoft.VisualBasic.FileSystem.Dir: returns the name of the first file/directory
        // matching the given path pattern (which may include wildcards), or "" if nothing matches.
        private static string VBDir(string pathPattern)
        {
            try
            {
                string dir = Path.GetDirectoryName(pathPattern);
                string pattern = Path.GetFileName(pathPattern);
                if (string.IsNullOrEmpty(dir))
                    dir = ".";
                if (!Directory.Exists(dir))
                    return "";
                var entries = Directory.GetFileSystemEntries(dir, pattern);
                return entries.Length > 0 ? Path.GetFileName(entries[0]) : "";
            }
            catch
            {
                return "";
            }
        }

        public int tempLevel;
        public object PerformFinalExpansion = false;

        public object DEBUG_LOGGING = true;
        public object DEBUG_LOGGING_TO_OUTPUT = false;
        public object DEBUG_LOGGING_VERBOSE = false;            // flag to output open and close brackets and parentesis - needs a better name



        public static string ParseLine(CFGConfiguration parentWorkspace, int currentLevel, string value, bool argOnly)
        {
            //Debug.WriteLine($"MacroParser: public static string ParseLine(CFGConfiguration parentWorkspace, int currentLevel, string <" + value + ">, bool argOnly)");
            var @file = new CFGFile(ref parentWorkspace) { CurrentLevel = currentLevel };
            var parser = new MacroParser(null, ref @file)
            {
                PerformFinalExpansion = true,            // TODO: temporary
                DEBUG_LOGGING = false
            };

            return parser.ParseLine(ref value, argOnly);
        }

        public MacroParser(ILogger logger, ref CFGFile @file)
        {
            _logger = logger;
            _file = @file;

            _ifStack = new Stack();
            _workspace = @file.ParentConfiguration;
            _lines = _file.Lines.GetEnumerator();

            _depth = 1;

            _parentList = new List<string>();
        }

        public void ParseFile()
        {
            while (NextLine())
            {
                _depth = 0;
                _parentList = new List<string>();
                // TODO: _variableNeedsExpansion = _lines.Current.IsVarDef

                if (_workspace.AbortProcessing)
                {
                    return;
                }

                if (!_lines.Current.IsComment)
                {
                    _lines.Current.Indent = _ifStack.Count; // Calculate the auto indent value
                }

                if (!_lines.Current.HasErrors && !_lines.Current.IsComment)
                {
                    var argline = _lines.Current;
                    ParseLine(ref argline);
                }
            }
        }

        public string ParseLine(ref string lineToParse, bool ArgOnly = false)
        {
            //Debug.WriteLine($"MacroParser: public string ParseLine(ref string <" + lineToParse+ ">, bool ArgOnly = false)");
            string expression;

            var line = new CFGLine();
            if (debugLineOutput && line.Text is not null && !string.IsNullOrEmpty(line.Text))
            {
                Debug.WriteLine("line ParceLine1: " + line.Text);
            }

            line.Text = lineToParse;
            line.TokenizeLine(ArgOnly);


            if (!ArgOnly)
            {
                _file.ParseTokenPhrase(line.Tokens, 0, line.Tokens.Count - 1);
            }
            SetTokenEnumerator(line);
            expression = ParseLine(ref line);

            if (!string.IsNullOrEmpty(expression))
            {
                expression = expression.Replace(@"\\", @"\");
            }
            //Debug.WriteLine($"MacroParser: public string ParseLine(ref CFGLine line) return: <" +expression + ">");
            return expression;
        }

        public string ParseLine(ref CFGLine line)
        {
            //Debug.WriteLine($"MacroParser: public string ParseLine(ref CFGLine <" + line.ToString +">)");
            CFGToken variableToken = null;
            CFGToken assignmentToken = null;
            _lastOperator = null;

            if (debugLineOutput && line.Text is not null && !string.IsNullOrEmpty(line.Text))
            {
                Debug.WriteLine("line ParceLine2: " + line.Text);
            }

            // Stop on errors
            // If line.HasErrors OrElse _workspace.AbortProcessing OrElse line.Tokens.Count = 0 OrElse line.Text = vbNullString Then Exit Function
            if (line.HasErrors || line.Tokens.Count == 0 || string.IsNullOrEmpty(line.Text))
            {
                return "";
            }

            // ParseTokenPhrase(line.Tokens, 0, line.Tokens.Count - 1)
            // If _tokens Is Nothing Then
            SetTokenEnumerator(line);
            // End If

            Get_ParentList(ref line);
            _file.ExpandVariableReferences(ref line);
            if (line.IsVarDef)
            {
                _file.ConvertFunctionsToStrings(ref line);
            }

            _currentLine = line;

            // Ignore the current line if it falls within a failed IF statement
            // But allow else, endif, and elif statements to always process
            bool ProcessNext;
            ProcessNext = ProcessNextLine();
            if (!ProcessNext)
            {
                if (line.IsIfStructure)
                {

                    if (line.Tokens[0].TokenType == CFGEnums.TT.ttIF)
                    {
                        if (_ifStack.Count > 0)
                            _ifStack.Push(IfStackStates.FalseState);
                        var argetype = CFGEnums.CFGEventType.cfgMessage;
                        string argdescription = Utilities.GetResourceString("TXT_ErrLineIgnoredDueToIFStructure");
                        CFGVariable argvariable = null;
                        string argvarname = "";
                        _workspace.AddEvent(ref argetype, ref argdescription, line, this._file, argvariable, varname: argvarname);
                        return "";
                    }

                    // Determine if the ELIF should be processed
                    if (line.Tokens[0].TokenType == CFGEnums.TT.ttELIF && !Process_Next_ELIF())
                    {
                        var argetype1 = CFGEnums.CFGEventType.cfgMessage;
                        string argdescription1 = Utilities.GetResourceString("TXT_ErrLineIgnoredDueToIFStructure");
                        CFGVariable argvariable1 = null;
                        string argvarname1 = "";
                        _workspace.AddEvent(ref argetype1, ref argdescription1, line, this._file, argvariable1, varname: argvarname1);
                        if (_currentLine.Indent > 0)
                            _currentLine.Indent -= 1;
                        return "";
                    }
                }
                else
                {
                    if (line.Tokens[0].TokenType == CFGEnums.TT.ttVarName)
                    {
                        var argetype2 = CFGEnums.CFGEventType.cfgVardef;
                        string argdescription2 = Utilities.GetResourceString("TXT_ErrNotProcessedDueToIFStructure");
                        CFGVariable argvariable2 = null;
                        _workspace.AddEvent(ref argetype2, ref argdescription2, line, this._file, argvariable2, line.Tokens[0].Value);
                    }
                    else
                    {
                        var argetype3 = CFGEnums.CFGEventType.cfgMessage;
                        string argdescription3 = Utilities.GetResourceString("TXT_ErrLineIgnoredDueToIFStructure");
                        CFGVariable argvariable3 = null;
                        string argvarname2 = "";
                        _workspace.AddEvent(ref argetype3, ref argdescription3, line, this._file, argvariable3, varname: argvarname2);
                    }

                    return "";

                }
            }
            else if (line.IsIfStructure)
            {
                if (line.Tokens[0].TokenType == CFGEnums.TT.ttELIF && !Process_Next_ELIF())
                {
                    var argetype4 = CFGEnums.CFGEventType.cfgMessage;
                    string argdescription4 = Utilities.GetResourceString("TXT_ErrLineIgnoredDueToIFStructure");
                    CFGVariable argvariable4 = null;
                    string argvarname3 = "";
                    _workspace.AddEvent(ref argetype4, ref argdescription4, line, this._file, argvariable4, varname: argvarname3);
                    if (_currentLine.Indent > 0)
                        _currentLine.Indent -= 1;

                    return "";
                }

            }

            // Get line
            LogLine("Line:");
            LogLine($"    {line.Text}{Environment.NewLine}");

            // _file.Get_ParentList(line)
            // _file.ExpandVariableReferences(line)

            // If ProcessNextLine() = False Then
            // If line.IsIfStructure = False Then Exit Function
            // If line.Tokens(0).TokenType = TT.ttIF Then Exit Function
            // If line.Tokens(0).TokenType = TT.ttELIF Then
            // If Process_Next_ELIF() = False Then Exit Function
            // End If
            // End If

            // Output tokens
            DebugTokens();

            // Flow and variable directives
            switch (_tokens.Current.TokenType)
            {
                case CFGEnums.TT.ttINCLUDE:
                    {
                        Process_INCLUDE();
                        break;
                    }
                case CFGEnums.TT.ttIF:
                    {
                        Process_IF();
                        break;
                    }
                case CFGEnums.TT.ttELSE:
                    {
                        Process_ELSE();
                        break;
                    }
                case CFGEnums.TT.ttELIF:
                    {
                        Process_ELIF();
                        break;
                    }
                case CFGEnums.TT.ttENDIF:
                    {
                        Process_ENDIF();
                        break;
                    }
                case CFGEnums.TT.ttECHO:
                    {
                        Process_ECHO();
                        break;
                    }
                case CFGEnums.TT.ttERROR:
                    {
                        Process_ERROR();
                        break;
                    }
                case CFGEnums.TT.ttLOCK:
                    {
                        Process_LOCK();
                        break;
                    }
                case CFGEnums.TT.ttUNDEF:
                    {
                        Process_UNDEF();
                        break;
                    }
                case CFGEnums.TT.ttLEVEL:
                    {
                        Process_LEVEL();
                        break;
                    }
                case CFGEnums.TT.ttVarName:
                    {
                        // Get variable and assignment operator
                        variableToken = _tokens.Current;
                        _tokens.MoveNext();
                        if (_tokens.Current is not null)
                        {
                            // Get assignment 
                            switch (_tokens.Current.TokenType)
                            {
                                case CFGEnums.TT.ttEQUATE:
                                case CFGEnums.TT.ttGREATERTHAN:
                                case CFGEnums.TT.ttLESSTHAN:
                                case CFGEnums.TT.ttCOLON:
                                case CFGEnums.TT.ttADD:
                                    {
                                        assignmentToken = _tokens.Current;
                                        _tokens.MoveNext();
                                        break;
                                    }
                            }
                        }

                        break;
                    }
            }

            LogLine($"Parse text:");
            string expression = ParseExpression(false);


            // TODO: variable creation
            if (variableToken is not null)
            {
                // TODO: temporary string correction
                if (assignmentToken is not null)
                {
                    expression = expression.Replace(@"\\", @"\");

                    if (expression.ToLower().Contains("file:") == false && expression.ToLower().Contains("pw:") == false)
                    {
                        expression = expression.Replace("/", @"\");
                    }
                    //Debug.Write($"MacroProcessor: ParseLine(ref CFGLine line: expression: " + expression);
                    Process_Variable_Def(ref variableToken, ref assignmentToken, expression);
                }
            }

            LogLine($"{Environment.NewLine}Expression value:");
            LogLine($"    {expression}");
            LogLine("--------------------------------------------------------------------------------");

            foreach (string varname in _parentList)
            {
                var argetype5 = CFGEnums.CFGEventType.cfgMessage;
                string argdescription5 = CEResource.TXT_VariableReferenced;
                var argvariable5 = _workspace.GetVariable(varname);
                _workspace.AddEvent(ref argetype5, ref argdescription5, line, this._file, argvariable5, varname);
            }
            //Debug.WriteLine($"MacroProcessor: public string ParseLine(ref CFGLine line) return: " + expression);
            return expression;
        }

        // TODO: express could return string, bool or numeric values
        private string ParseExpression(bool inFunction)
        {
            //Debug.Write($"MacroProcessor: private string ParseExpression(bool " + inFunction.ToString() + ")");

            string text = string.Empty;
            string result = string.Empty;

            _depth += 1;

            while (_tokens.Current is not null)
            {
                DebugExpression();

                // Process current token
                switch (_tokens.Current.TokenType)
                {
                    case CFGEnums.TT.ttOPENPARENTHESIS:
                        {
                            text += Process_OPENPARENTHESIS(inFunction);
                            break;
                        }
                    case CFGEnums.TT.ttCLOSEPARENTHESIS:
                        {
                            _tokens.MoveNext();
                            return text;
                        }
                    case CFGEnums.TT.ttOPENBRACKET:
                        {
                            text += Process_OPENBRACKET();
                            break;
                        }
                    case CFGEnums.TT.ttCLOSEBRACKET:
                        {
                            text += Process_CLOSEBRACKET();
                            break;
                        }
                    case CFGEnums.TT.ttFUNCOPEN:
                        {
                            _tokens.MoveNext();
                            text += ParseExpression(inFunction);
                            break;
                        }
                    case CFGEnums.TT.ttFUNCOPENabs:
                        {
                            _tokens.MoveNext();
                            text += ParseExpression(inFunction);
                            break;
                        }
                    case CFGEnums.TT.ttFUNCCLOSE:
                    case CFGEnums.TT.ttFUNCCLOSEabs:
                        {
                            break;
                        }
                    case CFGEnums.TT.ttCOMMA:
                        {
                            text += Process_COMMA(inFunction);
                            break;
                        }
                    case CFGEnums.TT.ttVarValue:
                        {
                            text += Process_VarValue(inFunction);
                            break;
                        }
                    case CFGEnums.TT.ttVarValueCurrent:
                        {
                            text += Process_VarValueCurrent();
                            break;
                        }
                    case CFGEnums.TT.ttString:
                        {
                            text += Process_String(inFunction);
                            break;
                        }
                    case CFGEnums.TT.ttStringWithParenthesis:
                        {
                            text += Process_StringWithParenthesis(inFunction);
                            break;
                        }
                    case CFGEnums.TT.ttStringWithBrackets:
                        {
                            text += Process_StringWithBrackets(inFunction);
                            break;
                        }
                    case CFGEnums.TT.ttStringWithQuotes:
                        {
                            text += Process_StringWithQuotes(inFunction);
                            break;
                        }
                    // Case TT.ttEQUATE, TT.ttGREATERTHAN, TT.ttLESSTHAN, TT.ttCOLON, TT.ttAPPEND : Process_Variable_Def(token) handled in ParseLine
                    case CFGEnums.TT.ttImpliedAppend:
                        {
                            _tokens.MoveNext();
                            break;
                        }
                    case CFGEnums.TT.ttIF:
                        {
                            Process_IF(); // handled in ParseLine
                            break;
                        }
                    case CFGEnums.TT.ttENDIF:
                        {
                            Process_ENDIF();
                            break;
                        }
                    // Return String.Empty
                    case CFGEnums.TT.ttELIF:
                        {
                            Process_ELIF(); // handled In ParseLine
                            break;
                        }
                    case CFGEnums.TT.ttELSE:
                        {
                            Process_ELSE(); // handled In ParseLine 
                            break;
                        }
                    case CFGEnums.TT.ttCONCAT:
                        {
                            text += Process_CONCAT();
                            break;
                        }
                    case CFGEnums.TT.ttBUILD:
                        {
                            text += Process_BUILD();
                            break;
                        }
                    case CFGEnums.TT.ttLASTDIRPIECE:
                        {
                            text += Process_LASTDIRPIECE(ParseArgument(true));
                            break;
                        }
                    case CFGEnums.TT.ttFIRSTDIRPIECE:
                        {
                            text += Process_FIRSTDIRPIECE(ParseArgument(true));
                            break;
                        }
                    case CFGEnums.TT.ttDEV:
                        {
                            text += Process_DEV(ParseArgument(true));
                            break;
                        }
                    case CFGEnums.TT.ttDEVDIR:
                        {
                            text += Process_DEVDIR(ParseArgument(true));
                            break;
                        }
                    case CFGEnums.TT.ttDIR:
                        {
                            text += Process_DIR(ParseArgument(true));
                            break;
                        }
                    case CFGEnums.TT.ttEXISTS:
                        {
                            //text = Conversions.ToString(Process_EXISTS());
                            text = Process_EXISTS()?.ToString();
                            break;
                        }
                    case CFGEnums.TT.ttFEATURE:
                        {
                            Process_FEATURE();
                            break;
                        }
                    // Case TT.ttLEVEL handled in ParseLine 
                    case CFGEnums.TT.ttLevelNo:
                        {
                            return text;
                        }
                    case CFGEnums.TT.ttLOCK:
                        {
                            Process_LOCK();
                            break;
                        }
                    // Case TT.ttINCLUDE handled in ParseLine
                    case CFGEnums.TT.ttDEFINED:
                        {
                            text = Process_DEFINED();
                            break;
                        }
                    case CFGEnums.TT.ttEXT:
                        {
                            text += Process_EXT(ParseArgument(true));
                            break;
                        }
                    case CFGEnums.TT.ttFILENAME:
                        {
                            text += Process_FILENAME(ParseArgument(true));
                            break;
                        }
                    case CFGEnums.TT.ttNOEXT:
                        {
                            text += Process_NOEXT(ParseArgument(true));
                            break;
                        }
                    case CFGEnums.TT.ttUNDEF:
                        {
                            Process_UNDEF();
                            break;
                        }
                    case CFGEnums.TT.ttPARENTDIR:
                        {
                            text += Process_PARENTDIR(ParseArgument(true));
                            break;
                        }
                    case CFGEnums.TT.ttPARENTDEVDIR:
                        {
                            text += Process_PARENTDEVDIR(ParseArgument(true));
                            break;
                        }
                    case CFGEnums.TT.ttBACKTRACK:
                        {
                            text += Process_BACKTRACK(ParseArgument(true));
                            break;
                        }
                    case CFGEnums.TT.ttREGISTRYREAD:
                        {
                            text += Process_REGISTRYREAD(ParseArgument(true));
                            break;
                        }
                    case CFGEnums.TT.ttBASENAME:
                        {
                            text += Process_BASENAME(ParseArgument(true));
                            break;
                        }
                    case CFGEnums.TT.ttDOLLAR:
                        {
                            text += Process_DOLLAR(inFunction);
                            break;
                        }
                    case CFGEnums.TT.ttAND:
                        {
                            text = Process_AND(inFunction, text);
                            break;
                        }
                    case CFGEnums.TT.ttOR:
                        {
                            text = Process_OR(inFunction, text);
                            break;
                        }
                    case CFGEnums.TT.ttNOT:
                        {
                            text = Process_NOT(inFunction);
                            break;
                        }
                    case CFGEnums.TT.ttNOTEQUALS:
                        {
                            text = Process_NOTEQUALS(inFunction, text);
                            break;
                        }
                    case CFGEnums.TT.ttEQUALS:
                        {
                            text = Process_EQUALS(inFunction, text);
                            break;
                        }
                    // Case TT.ttERROR handled in ParseLine
                    // Case TT.ttECHO handled in ParseLine
                    case CFGEnums.TT.ttFIRST:
                        {
                            text += Process_FIRST(ParseArgument(true));
                            break;
                        }
                    case CFGEnums.TT.ttGREATERTHAN:
                        {
                            text = Process_GT(inFunction, text); // converted from ttGREATERTHAN in CFGLine.ConvertFunctionChars()
                            break;
                        }
                    case CFGEnums.TT.ttADD:
                        {
                            text = Process_ADD(inFunction, text); // converted from ttAPPEND in CFGLine.ConvertFunctionChars()
                            break;
                        }
                    case CFGEnums.TT.ttGREATEROREQUAL:
                        {
                            text = Process_GREATERTHANOREQUAL(inFunction, text);
                            break;
                        }
                    case CFGEnums.TT.ttLESSTHAN:
                        {
                            text = Process_LT(inFunction, text);    // converted from ttLESSTHAN in CFGLine.ConvertFunctionChars()
                            break;
                        }
                    case CFGEnums.TT.ttLESSTOREQUAL:
                        {
                            text = Process_LESSTHANOREQUAL(inFunction, text);
                            break;
                        }
                    case CFGEnums.TT.ttBITAND:
                        {
                            text = Process_BITAND(inFunction, text);
                            break;
                        }
                    case CFGEnums.TT.ttBITOR:
                        {
                            text = Process_BITOR(inFunction, text);
                            break;
                        }
                    case CFGEnums.TT.ttBITXOR:
                        {
                            text = Process_BITXOR(inFunction, text);
                            break;
                        }
                    case CFGEnums.TT.ttMODULUS:
                        {
                            text = Process_MODULUS(inFunction, text);
                            break;
                        }
                    case CFGEnums.TT.ttMULTIPLY:
                        {
                            text = Process_MULTIPLY(inFunction, text); // TODO: these don't work yet. Sample tests for True == 16
                            break;
                        }
                    case CFGEnums.TT.ttSHIFTLEFT:
                        {
                            text = Process_SHIFTLEFT(inFunction, text);
                            break;
                        }
                    case var @case when @case == CFGEnums.TT.ttSHIFTRIGHT:
                        {
                            text = Process_SHIFTRIGHT(inFunction, text);
                            break;
                        }
                    case CFGEnums.TT.ttSUBTRACT:
                        {
                            text = Process_SUBTRACT(inFunction, text);
                            break;
                        }
                    case CFGEnums.TT.ttDIVIDE:
                        {
                            text = Process_DIVIDE(inFunction, text); // TODO: this is never hit as / is replaced with \ at some point and becomes a ttString
                            break;
                        }

                    default:
                        {
                            Debug.WriteLine($"    [ERROR in ParseExpression] Unexpected token: {_tokens.Current.TokenType}");
                            return $"[ERROR] Unexpected token: {_tokens.Current.TokenType}";
                        }
                }

                if (!string.IsNullOrEmpty(text) && text is not null)
                {
                    result = text;
                }
                else
                {
                    // TODO: fixing if processing at the expense of operator precendence for now.
                    LogLine($"{new string(' ', (_depth + 1) * 4)}** Empty result and possible return needed for operator precedence in ParseExpression **");
                    // LogLine($"{New String(" ", (_depth + 1) * 4)}Yielded to previous operator")
                    return result;
                }

            }

            return result;
        }

        private string ParseArgument(bool inFunction)
        {
            string padding = new string(' ', _depth * 4);

            //(_tokens.Current.TokenType != T)
            if (inFunction)
            {
                _tokens.MoveNext(); // move past function
                                    // TODO: check ( vs { or something else
                _tokens.MoveNext(); // move past ( }

            }
            string val = ParseExpression(inFunction);

            LogLine($"{padding}= {val}");

            return val;
        }

        private void Process_Variable_Def(ref CFGToken var_token, ref CFGToken op_token, string New_Value)
        {
            CFGVariable variable;
            bool Existed;
            //Debug.WriteLine($"MacroProcessor: Process_Variable_Def(ref CFGToken var_token, ref CFGToken op_token, string New_Value)");

            // Get a reference to the variable if it exists
            variable = _workspace.GetVariable(var_token.Value);

            // Create a new variable if it doesn't exist
            Existed = true;
            if (variable is null)
            {
                Existed = false;
                variable = new CFGVariable(this._workspace)
                {
                    Name = VBUCase(var_token.Value),
                    Level = _file.CurrentLevel
                };
                _workspace.Variables.Add(variable.Name, variable);
                var argetype = CFGEnums.CFGEventType.cfgVarCreated;
                string argdescription = string.Format(CEResource.TXT_MsgVariableCreated, variable.Name);
                var argline = _lines.Current;
                _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, variable, var_token.Value);
            //}
            //else
            //{
                // variable already exist, set the isdefined flag to True to allow variable.SetValue to change the value to new value
                //variable.IsDefined = true;
            }

            // Raise error about modifying a locked variable
            if (variable.IsLocked)
            {
                var argetype1 = CFGEnums.CFGEventType.cfgWarning;
                string argdescription1 = CEResource.TXT_MsgAttemptToModifyLockedVariable;
                var argline1 = _lines.Current;
                string argvarname = "";
                _workspace.AddEvent(ref argetype1, ref argdescription1, argline1, this._file, variable, varname: argvarname);
                return;
            }

            // Check to see if the new value appears to be a hardcoded path
            Check_Variable_Hardcode(ref New_Value, ref variable);

            // Validate value as a possible network path
            ValidateValueAsUNCPath(ref New_Value);

            // Assign the new value to the variable based on the operator used
            switch (op_token.TokenType)
            {
                case CFGEnums.TT.ttEQUATE:
                    {
                        if (CultureInfo.CurrentCulture.CompareInfo.Compare(VBUCase(variable.GetValue(ref _file.CurrentLevel)) ?? "", VBUCase(New_Value) ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 && !string.IsNullOrEmpty(New_Value))
                        {
                            var argetype2 = CFGEnums.CFGEventType.cfgWarning;
                            string argdescription2 = CEResource.TXT_MsgDuplicateVariableAssignment;
                            var argline2 = _lines.Current;
                            string argvarname1 = "";
                            _workspace.AddEvent(ref argetype2, ref argdescription2, argline2, this._file, variable, varname: argvarname1);
                        }

                        variable.SetValue(New_Value, _file.CurrentLevel);
                        variable.Level = _file.CurrentLevel;
                        variable.IsDefined = true;
                        bool argClearParents = true;
                        variable.AppendParents(ref _parentList, ref argClearParents);
                        variable.NeedsSpecialExpansion = _variableNeedsExpansion; // TODO: need to check if this gets set
                        var argetype3 = CFGEnums.CFGEventType.cfgVardef;
                        string argdescription3 = CEResource.TXT_MsgVariableValueChanged;
                        var argline3 = _lines.Current;
                        string argvarname2 = "";
                        _workspace.AddEvent(ref argetype3, ref argdescription3, argline3, this._file, variable, varname: argvarname2);
                        break;
                    }

                case CFGEnums.TT.ttCOLON:
                    {
                        // If variable.Value IsNot Nothing AndAlso Not File.Exists(New_Value) AndAlso Not Directory.Exists(New_Value) Then
                        if (Existed && variable.Value is not null && variable.IsDefined)
                        {
                            var argetype4 = CFGEnums.CFGEventType.cfgMessage;
                            string argdescription4 = CEResource.TXT_MsgVariableAssignmentIgnored;
                            var argline4 = _lines.Current;
                            string argvarname3 = "";
                            _workspace.AddEvent(ref argetype4, ref argdescription4, argline4, this._file, variable, varname: argvarname3);
                        }
                        else
                        {
                            variable.SetValue(New_Value, _file.CurrentLevel);
                            variable.Level = _file.CurrentLevel;
                            variable.IsDefined = true;
                            bool argClearParents1 = false;
                            variable.AppendParents(ref _parentList, ref argClearParents1);
                            variable.NeedsSpecialExpansion = _variableNeedsExpansion; // TODO: need to check if this gets set
                            var argetype5 = CFGEnums.CFGEventType.cfgVardef;
                            string argdescription5 = CEResource.TXT_MsgVariableValueChanged;
                            var argline5 = _lines.Current;
                            string argvarname4 = "";
                            _workspace.AddEvent(ref argetype5, ref argdescription5, argline5, this._file, variable, varname: argvarname4);
                        }

                        break;
                    }
                case CFGEnums.TT.ttGREATERTHAN:
                    {
                        if (!string.IsNullOrEmpty(variable.GetValue(ref _file.CurrentLevel)))
                        {
                            variable.SetValue(variable.GetValue(ref _file.CurrentLevel) + ";" + New_Value, _file.CurrentLevel);
                        }
                        else
                        {
                            variable.SetValue(New_Value, _file.CurrentLevel);
                        }
                        variable.Level = _file.CurrentLevel;
                        variable.IsDefined = true;
                        bool argClearParents2 = false;
                        variable.AppendParents(ref _parentList, ref argClearParents2);
                        variable.NeedsSpecialExpansion = _variableNeedsExpansion | variable.NeedsSpecialExpansion; // TODO: need to check if this gets set
                        var argetype6 = CFGEnums.CFGEventType.cfgVardef;
                        string argdescription6 = CEResource.TXT_MsgVariableValueChanged + " >  ";
                        var argline6 = _lines.Current;
                        string argvarname5 = "";
                        _workspace.AddEvent(ref argetype6, ref argdescription6, argline6, this._file, variable, varname: argvarname5);
                        break;
                    }
                case CFGEnums.TT.ttLESSTHAN:
                    {
                        if (!string.IsNullOrEmpty(variable.GetValue(ref _file.CurrentLevel)))
                        {
                            variable.SetValue(New_Value + ";" + variable.GetValue(ref _file.CurrentLevel), _file.CurrentLevel);
                        }
                        else
                        {
                            variable.SetValue(New_Value, _file.CurrentLevel);
                        }
                        variable.Level = _file.CurrentLevel;
                        variable.IsDefined = true;
                        bool argClearParents3 = false;
                        variable.AppendParents(ref _parentList, ref argClearParents3);
                            variable.NeedsSpecialExpansion = _variableNeedsExpansion | variable.NeedsSpecialExpansion; // TODO: need to check if this gets set
                        var argetype7 = CFGEnums.CFGEventType.cfgVardef;
                        string argdescription7 = CEResource.TXT_MsgVariableValueChanged + " <  ";
                        var argline7 = _lines.Current;
                        string argvarname6 = "";
                        _workspace.AddEvent(ref argetype7, ref argdescription7, argline7, this._file, variable, varname: argvarname6);
                        break;
                    }
                case CFGEnums.TT.ttADD:
                    {
                        variable.SetValue(variable.GetValue(ref _file.CurrentLevel) + " " + New_Value, _file.CurrentLevel);
                        variable.Level = _file.CurrentLevel;
                        variable.IsDefined = true;
                        bool argClearParents4 = false;
                        variable.AppendParents(ref _parentList, ref argClearParents4);
                        variable.NeedsSpecialExpansion = _variableNeedsExpansion | variable.NeedsSpecialExpansion; // TODO: need to check if this gets set
                        var argetype8 = CFGEnums.CFGEventType.cfgVardef;
                        string argdescription8 = CEResource.TXT_MsgVariableValueChanged + " +  ";
                        var argline8 = _lines.Current;
                        string argvarname7 = "";
                        _workspace.AddEvent(ref argetype8, ref argdescription8, argline8, this._file, variable, varname: argvarname7);
                        break;
                    }
            }
        }

        private void Check_Variable_Hardcode(ref string value, ref CFGVariable variable)
        {
            //Debug.Write($"Check_Variable_Hardcode(ref string " + value + ", ref CFGVariable " + variable.Name +")");
            if (value is null || variable is null)
                return;

            // Add a warning to the event list if the value appears to be a hardcoded path
            var found = default(bool);
            if (_currentVarRefUsed)
                return;
            if (value.Length < 3)
                return;
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(VBMid(value, 1, 2), @"\\", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                found = true;
            if (char.IsLetter(value[0]) && value[1].ToString() == ": ")
                found = true;
            if (found)
            {
                var argetype = CFGEnums.CFGEventType.cfgWarning;
                string argdescription = CEResource.TXT_MsgPossibleHardcodedLocation + value;
                var argline = _lines.Current;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, variable, varname: argvarname);
            }
        }

        private void ValidateValueAsUNCPath(ref string value)
        {
            //Debug.Write($"MacorParser: ValidateValueAsUNCPath(ref string value)");
            // Check if this is a network path starting with \\\, then verify a trailing slash exist at the end of variable
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(VBMid(value, 1, 3), @"\\\", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
                return;

            // Verify the path is a valid directory over network
            if (!Directory.Exists(VBMid(value, 2)))
                return;

            if (value.EndsWith(@"\") | value.EndsWith("/"))
                return;

            value += @"\";
        }

        public void SetTokenEnumerator(CFGLine line)
        {
            _tokens = line.Tokens.GetEnumerator();
            _tokens.MoveNext();
        }

        private bool NextLine()
        {
            if (_lines.MoveNext())
            {
                SetTokenEnumerator(_lines.Current);

                return true;
            }

            return false;
        }

        // TODO: there is an existing token to string function that's similar
        private string Get_Path(bool inFunction)
        {
            CFGToken token;
            string path;
            CFGVariable variable;

            if (_tokens.Current is null)
            {
                return "~error";
            }

            token = _tokens.Current;

            if (token.TokenType == CFGEnums.TT.ttVarValue || token.TokenType == CFGEnums.TT.ttVarValueCurrent || token.TokenType == CFGEnums.TT.ttString || token.TokenType == CFGEnums.TT.ttStringWithParenthesis || token.TokenType == CFGEnums.TT.ttStringWithBrackets)
            {
                // Get variable
                variable = _workspace.GetVariable(token.Value);

                if (variable is null)
                {
                    if (token.TokenType == CFGEnums.TT.ttVarValue || token.TokenType == CFGEnums.TT.ttVarValueCurrent)
                    {
                        _workspace.DefineVariable(token.Value, "", 0, false, false, null);
                        variable = _workspace.GetVariable(token.Value);
                        variable.IsDefined = false;
                    }
                }

                // Value is in ${}
                if (token.TokenType == CFGEnums.TT.ttStringWithBrackets)
                {
                    _currentVarRefUsed = true;
                }

                if (variable is not null)
                {
                    if (!_parentList.Contains(variable.Name))
                        _parentList.Add(variable.Name);
                    if (variable.IsDefined)
                    {
                        // path = variable.Expand(_file.CurrentLevel)
                        path = variable.Expand();
                    }
                    else if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(token.Value)))
                    {
                        path = Environment.GetEnvironmentVariable(token.Value);
                    }
                    else
                    {
                        var argetype = CFGEnums.CFGEventType.cfgError;
                        string argdescription = CEResource.TXT_ErrUndefinedVariablesShouldNotBeUsedInThisContext;
                        var argline = _lines.Current;
                        CFGVariable argvariable = null;
                        _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, varname: token.Value, variable: argvariable);
                        return "";
                    }
                }
                else if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(token.Value)))
                {
                    path = Environment.GetEnvironmentVariable(token.Value);
                }
                else
                {
                    path = token.Value;
                }
            }
            else
            {
                return "~Error";
            }

            return path;
        }



        public void Get_ParentList(ref CFGLine Line)
        {
            // Builds a list of parent variables referenced in this line
            string varref;

            _parentList = new List<string>();

            if (Line.IsVarDef)
            {
                varref = VBLCase(Line.Tokens[0].Value);
                foreach (CFGToken token in Line.Tokens)
                {
                    if (token.TokenType == CFGEnums.TT.ttVarValue || token.TokenType == CFGEnums.TT.ttVarValueCurrent)
                    {
                        if (CultureInfo.CurrentCulture.CompareInfo.Compare(VBLCase(token.Value) ?? "", varref ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                        {
                            var argetype = CFGEnums.CFGEventType.cfgAbort;
                            string argdescription = CEResource.TXT_MsgVariableReferencesItself;
                            CFGVariable argvariable = null;
                            string argvarname = "";
                            _workspace.AddEvent(ref argetype, ref argdescription, Line, this._file, variable: argvariable, varname: argvarname);
                            return;
                        }
                        if (!_parentList.Contains(token.Value))
                            _parentList.Add(token.Value);
                    }

                }
            }
        }

        private string GetValueFromString(CFGToken token, bool check_varname = false, bool check_quotes = false)
        {
            // Returns the value from a string token - if its a variable reference then it finds its value

            CFGVariable variable;

            if (token is null)
                return "~Error";

            if (token.TokenType != CFGEnums.TT.ttString && token.TokenType != CFGEnums.TT.ttStringWithParenthesis && token.TokenType != CFGEnums.TT.ttStringWithQuotes && token.TokenType != CFGEnums.TT.ttStringWithBrackets)
            {
                return "~Error";
            }

            if (token.TokenType == CFGEnums.TT.ttStringWithQuotes)
                return VBReplace(token.Value, "\"", "");

            if (check_varname && (token.TokenType == CFGEnums.TT.ttString || token.TokenType == CFGEnums.TT.ttStringWithParenthesis))
            {
                if (token.OriginalType != CFGEnums.TT.ttVarValue)
                {
                    variable = _workspace.GetVariable(token.Value);
                    if (variable is not null)
                    {
                        if (!_parentList.Contains(variable.Name))
                            _parentList.Add(variable.Name);
                        return variable.Expand(_file.CurrentLevel);
                    }
                }
            }

            if (check_quotes && token.OriginalType == CFGEnums.TT.ttString)
            {
                if (!VBIsNumeric(token.Value))
                {
                    var argetype = CFGEnums.CFGEventType.cfgCritical;
                    string argdescription = CEResource.TXT_ErrMissingQuotesAroundText;
                    var argline = _lines.Current;
                    CFGVariable argvariable = null;
                    string argvarname = "";
                    _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, variable: argvariable, varname: argvarname);
                    return "~error";
                }
            }

            if (token.TokenType == CFGEnums.TT.ttString)
                return token.Value;

            if (token.TokenType == CFGEnums.TT.ttStringWithParenthesis || token.TokenType == CFGEnums.TT.ttStringWithBrackets)
            {
                variable = _workspace.GetVariable(token.Value);

                if (variable is not null)
                {
                    if (!_parentList.Contains(variable.Name))
                        _parentList.Add(variable.Name);
                    variable.Expand(_file.CurrentLevel);
                }
                else
                {
                    return "";
                }
            }

            return "~error";
        }

        private void DebugExpression()
        {
            //Debug.Write($"MacroProcessor: private void DebugExpression()");
            if (_tokens.Current is not null)
            {
                switch (_tokens.Current.TokenType)
                {
                    case CFGEnums.TT.ttOPENPARENTHESIS:
                        {
                            break;
                        }
                    case CFGEnums.TT.ttCLOSEPARENTHESIS:
                        {
                            _depth -= 1;
                            if (Convert.ToBoolean(DEBUG_LOGGING_VERBOSE))
                            {
                                LogLine($"{new string(' ', _depth * 4)}{_tokens.Current.TokenType}    {_tokens.Current.Value}");
                            }

                            break;
                        }

                    default:
                        {
                            LogLine($"{new string(' ', _depth * 4)}{_tokens.Current.TokenType}    {_tokens.Current.Value}");
                            break;
                        }
                }
            }
        }

        private void DebugTokens()
        {
            CFGToken token;

            LogLine("Token output:");

            while (_tokens.Current is not null)
            {
                token = _tokens.Current;
                LogLine($"    [{token.Precedence}] {token.TokenType,-25}{token.Value}");

                _tokens.MoveNext();
            }

            LogLine();

            // Reset enumerator
            _tokens.Reset();
            _tokens.MoveNext();
        }

        // Public Sub IndexTokens(Tokens As List(Of CFGToken), StartToken As Integer, EndToken As Integer)
        // Dim i As Integer
        // Dim parStackIndex As New Stack
        // Dim parFunctionStack As New Stack
        // Dim functionCloseIndex As Integer = 0
        // Dim absCloseIndex As Integer = 0
        // Dim TokenInFunction As Boolean = False

        // For i = StartToken To EndToken
        // Tokens(i).TokenIndex = i
        // Tokens(i).ValueWorking = Tokens(i).Value
        // Tokens(i).TokenTypeWorking = Tokens(i).TokenType
        // Tokens(i).TokenGroupWorking = Tokens(i).TokenGroup
        // If Tokens(i).TokenType = TT.ttOPENPARENTHESIS Then
        // parStackIndex.Push(i)
        // ElseIf Tokens(i).TokenType = TT.ttCLOSEPARENTHESIS Then
        // Tokens(i).EncapluationMatch = parStackIndex.Peek()
        // Tokens(parStackIndex.Peek()).EncapluationMatch = i
        // parStackIndex.Pop()
        // End If
        // Next

        // For i = StartToken To EndToken
        // If Tokens(i).TokenGroup = TokenGroup.tgFunction Then
        // If Tokens(i + 1).TokenType = TT.ttOPENPARENTHESIS Then
        // 'Indetify Matching Close Parthesis.
        // If Tokens(i + 1).EncapluationMatch >= functionCloseIndex Then
        // functionCloseIndex = Tokens(i + 1).EncapluationMatch
        // End If
        // TokenInFunction = True
        // Else 'Set token to String since it is not a function
        // Tokens(i).TokenType = TT.ttString
        // Tokens(i).TokenGroup = TokenGroup.tgOperand
        // End If
        // ElseIf Tokens(i).TokenType = TT.ttStringWithBrackets Or Tokens(i).TokenType = TT.ttStringWithParenthesis Then
        // Tokens(i).TokenType = TT.ttString
        // Tokens(i).TokenGroup = TokenGroup.tgOperand
        // End If

        // If Tokens(i).TokenIndex >= functionCloseIndex Then
        // functionCloseIndex = 0
        // TokenInFunction = False
        // End If
        // Tokens(i).IndexFunctionClose = functionCloseIndex
        // Tokens(i).InFunction = TokenInFunction
        // Next
        // End Sub

        // Public Function EvaluateTokens(Tokens As List(Of CFGToken), StartToken As Integer) As String
        // 'Dim eTokens As List(Of CFGToken)
        // Dim i As Integer

        // Do While i < Tokens.Count
        // If Tokens(i).TokenType = TT.ttOPENPARENTHESIS Then
        // EvaluateTokens(Tokens, i)
        // ElseIf Tokens(i).TokenType = TT.ttCLOSEPARENTHESIS Then
        // EvaluateTokens(Tokens, i)
        // ElseIf Tokens(i).TokenType = TT.ttString Then
        // EvaluateTokens = Tokens(i).Value
        // ElseIf Tokens(i).TokenGroup = TokenGroup.tgFunction Then
        // ProcessFunction(Tokens, i)
        // ElseIf Tokens(i).TokenGroup = TokenGroup.tgBooleanFunc Then



        // End If

        // Loop
        // Return ""
        // End Function

        public string ProcessFunction(List<CFGToken> Tokens, int StartToken)
        {
            int i;
            i = StartToken;
            if (Tokens[i].TokenType == CFGEnums.TT.ttCONCAT | Tokens[i].TokenType == CFGEnums.TT.ttBUILD)
            {

                return "";
            }
            else if (Tokens[i].TokenType == CFGEnums.TT.ttDOLLAR)
            {
                return "";
            }
            else
            {

                return "";
            }

        }

        // Public Function ProcessBoolean(Tokens As List(Of CFGToken), StartToken As Integer) As String
        // Dim i As Integer
        // i = StartToken
        // If Tokens(i).TokenType = TT.ttAND OrElse Tokens(i).TokenType = TT.ttOR OrElse Tokens(i).TokenType = TT Then


        // ElseIf Tokens(i).TokenType = TT.ttDOLLAR Then

        // Else


        // End If

        // End Function

        private void LogLine(string message = "")
        {
            if (Convert.ToBoolean(DEBUG_LOGGING_TO_OUTPUT))
            {
                Debug.WriteLine(message);
            }

            if (Convert.ToBoolean(DEBUG_LOGGING))
            {
                _logger?.LogLine(message);
            }
        }


        private bool ProcessNextLine()
        {
            return _ifStack.Count == 0 || Equals(_ifStack.Peek(), IfStackStates.TrueState);
        }

        private bool Process_Next_ELIF()
        {
            var currentState = _ifStack.Peek();

            if (Equals(currentState, IfStackStates.TrueState))
            {
                _ifStack.Pop();
                _ifStack.Push(IfStackStates.FalseState);
                return false;
            }

            if (Equals(currentState, IfStackStates.StartState))
            {
                return true;
            }

            // currentState == IfStackStates.FalseState
            return false;
        }

        #region Token Processing

        private string Process_OPENPARENTHESIS(bool inFunction)
        {
            string val;

            _tokens.MoveNext();

            val = ParseExpression(inFunction);

            return val;
        }

        private string Process_OPENBRACKET()
        {
            string val = "{";

            _tokens.MoveNext();

            return val;
        }

        private string Process_CLOSEBRACKET()
        {
            string val = "";

            if (_inVar == false)
            {
                val = "}";
            }

            _tokens.MoveNext();

            return val;
        }

        private string Process_COMMA(bool inFunction)
        {
            string val = string.Empty;

            if (!inFunction)
            {
                val = ",";
            }

            _tokens.MoveNext();

            return val;
        }

        private string Process_VarValue(bool inFunction)
        {
            string val;

            _variableNeedsExpansion = inFunction;

            var variable = _workspace.GetVariable(_tokens.Current.Value);

            if (variable is not null && variable.IsDefined)
            {
                val = Get_Path(inFunction);
            }
            else if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(_tokens.Current.Value)))
            {
                val = Environment.GetEnvironmentVariable(_tokens.Current.Value);
            }
            else
            {
                if (variable is null)
                {
                    if (_tokens.Current.TokenType == CFGEnums.TT.ttVarValue || _tokens.Current.TokenType == CFGEnums.TT.ttVarValueCurrent)
                    {
                        _workspace.DefineVariable(_tokens.Current.Value, "", 0, false, false, null);
                        variable = _workspace.GetVariable(_tokens.Current.Value);
                        variable.IsDefined = false;
                    }
                }

                val = "$(" + _tokens.Current.Value + ")";
                _variableNeedsExpansion = true;
            }

            _tokens.MoveNext();

            return val;
        }

        private string Process_VarValueCurrent()
        {
            string val = Get_Path(true);

            _tokens.MoveNext();

            return val;
        }

        private string Process_String(bool inFunction)
        {
            string val;
            var variable = _workspace.GetVariable(_tokens.Current.Value);

            if (inFunction || Convert.ToBoolean(PerformFinalExpansion))
            {
                if (!string.IsNullOrEmpty(_tokens.Current.Value) && _tokens.Current.Value.IndexOf("role2", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    string tempVal;
                    tempVal = variable != null
                        ? variable.Value
                        : _workspace.ReplaceVariablePlaceholders(_tokens.Current.Value);
                    if (tempVal=="2")
                    { 
                        Debugger.Break();
                    }
                }
                val = variable != null
                    ? variable.Value
                    : _workspace.ReplaceVariablePlaceholders(_tokens.Current.Value);
            }
            else
            {
                val = _tokens.Current.Value;
            }

            _tokens.MoveNext();
            return val;
        }

        private string Process_StringWithParenthesis(bool inFunction)
        {
            string val;

            if (inFunction || _inIf)
            {
                val = Get_Path(inFunction); // inside a function or If block
            }
            else if (_inVar)
            {
                val = "$(" + _tokens.Current.Value + ")";
            }
            else
            {
                // TODO: strings with parenthesis get confused with variable names in parentheses
                val = $"({_tokens.Current.Value})";
            } // not inside a function or If block 

            _tokens.MoveNext();

            return val;
        }

        private string Process_StringWithBrackets(bool inFunction)
        {
            string val = Get_Path(inFunction);

            _tokens.MoveNext();

            return val;
        }

        private string Process_StringWithQuotes(bool inFunction)
        {
            string val;

            if (inFunction == true)
            {
                string DQ;
                DQ = "\"";
                val = VBReplace(_tokens.Current.Value, DQ, "");
            }
            else
            {
                val = $"\"{_tokens.Current.Value}\"";

            }
            _tokens.MoveNext();

            return val;
        }

        private void Process_LEVEL()
        {
            // Processes the %level directive

            _tokens.MoveNext();

            var leveltoken = _tokens.Current;
            int new_val;
            object values = new[] { "system", "application", "organization", "workspace", "workset", "role", "user" };

            // Check operand stack for at least 1 token
            if (_tokens.Current is null)
            {
                var argetype = CFGEnums.CFGEventType.cfgError;
                string argdescription = Utilities.GetResourceString("TXT_MsgUnableToProcessLineLevel");
                CFGVariable argvariable = null;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, _currentLine, this._file, variable: argvariable, varname: argvarname);    // Event
                _currentLine.HasErrors = true;
                return;
            }

            if (leveltoken.TokenType != CFGEnums.TT.ttString)
            {
                var argetype1 = CFGEnums.CFGEventType.cfgError;
                string argdescription1 = Utilities.GetResourceString("TXT_MsgUnableToProcessLineLevel");
                CFGVariable argvariable1 = null;
                string argvarname1 = "";
                _workspace.AddEvent(ref argetype1, ref argdescription1, _currentLine, this._file, variable: argvariable1, varname: argvarname1);    // Event
                _currentLine.HasErrors = true;
                return;
            }

            new_val = (int)Math.Round(VBVal(leveltoken.Value));

            if (char.IsDigit(leveltoken.Value[0]) && new_val >= 0 && new_val <= 6 && leveltoken.Value.Length == 1)
            {
                _file.CurrentLevel = new_val;
                var argetype2 = CFGEnums.CFGEventType.cfgLevelChange;
                string argdescription2 = Utilities.GetResourceString("TXT_MsgLevelChangedTo") + _file.CurrentLevel;
                CFGVariable argvariable2 = null;
                string argvarname2 = "";
                _workspace.AddEvent(ref argetype2, ref argdescription2, _currentLine, this._file, variable: argvariable2, varname: argvarname2);    // Event
            }
            else if (Array.IndexOf((Array)values, leveltoken.Value.ToLower()) != -1)
            {
                new_val = Array.IndexOf((Array)values, leveltoken.Value.ToLower());
                _file.CurrentLevel = new_val;
                var argetype4 = CFGEnums.CFGEventType.cfgLevelChange;
                string argdescription4 = Utilities.GetResourceString("TXT_MsgLevelChangedTo") + _file.CurrentLevel;
                CFGVariable argvariable4 = null;
                string argvarname4 = "";
                _workspace.AddEvent(ref argetype4, ref argdescription4, _currentLine, this._file, variable: argvariable4, varname: argvarname4);    // Event
            }
            else
            {
                var argetype3 = CFGEnums.CFGEventType.cfgError;
                string argdescription3 = Utilities.GetResourceString("TXT_MsgInvalidLevelAssignment") + leveltoken.Value;
                CFGVariable argvariable3 = null;
                string argvarname3 = "";
                _workspace.AddEvent(ref argetype3, ref argdescription3, _currentLine, this._file, variable: argvariable3, varname: argvarname3);    // Event
                _currentLine.HasErrors = true;
                return;
            }

            tempLevel = _file.CurrentLevel;

            if (tempLevel == 1)
            {
                tempLevel = 1;
            }
            else if (tempLevel == 2)
            {
                tempLevel = 2;
            }
            else if (tempLevel == 3)
            {
                tempLevel = 3;
            }
            else if (tempLevel == 4)
            {
                tempLevel = 4;
            }
            else if (tempLevel == 5)
            {
                tempLevel = 5;
            }
            else if (tempLevel == 6)
            {
                tempLevel = 6;
            }
        }

        private void Process_ECHO()
        {
            // Processes the %echo directive

            string message;

            _tokens.MoveNext();

            if (_tokens.Current is null)
            {
                message = CEResource.TXT_EchoStatement;
            }
            else
            {
                message = CEResource.TXT_EchoStatement + ": " + ParseExpression(false);
            }

            InterfaceControler.Echo(message);
            LogLine(message);

            var argetype = CFGEnums.CFGEventType.cfgMessage;
            var argline = _lines.Current;
            CFGVariable argvariable = null;
            string argvarname = "";
            _workspace.AddEvent(ref argetype, ref message, argline, this._file, variable: argvariable, varname: argvarname);
            // _lines.Current().HasErrors = False
        }

        private void Process_ERROR()
        {
            // Processes the %error directive

            string message;

            _tokens.MoveNext();

            if (_tokens.Current is null)
            {
                message = CEResource.TXT_ErrRaised;
            }
            else
            {
                message = CEResource.TXT_ErrRaised + ": " + ParseExpression(false);
            }

            InterfaceControler.Echo(message);
            LogLine(message);

            var argetype = CFGEnums.CFGEventType.cfgCritical;
            var argline = _lines.Current;
            CFGVariable argvariable = null;
            string argvarname = "";
            _workspace.AddEvent(ref argetype, ref message, argline, this._file, variable: argvariable, varname: argvarname);
            _lines.Current.HasErrors = true;
        }

        // TODO: check for errors and add BCE events
        // TODO: replace ParseExpression with ParseBooleanExpression
        private string Process_IF(string forcedValue = "")
        {
            // Processes the %if directive

            _inIf = true;

            // Push start state in ifstack
            if (_ifStack.Count == 0 || Equals(_ifStack.Peek(), IfStackStates.TrueState))
            {
                _ifStack.Push(IfStackStates.StartState);
            }

            // If _lastOperator IsNot Nothing AndAlso _lastOperator.Precedence > _tokens.Current.Precedence Then
            // _lastOperator = _tokens.Current
            // Return String.Empty
            // End If
            // _lastOperator = _tokens.Current

            _tokens.MoveNext();

            string val;
            if (!string.IsNullOrEmpty(forcedValue))
            {
                val = forcedValue;
            }
            else
            {
                val = ParseExpression(true);
            }

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(val, "0", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                val = "False";

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(val, "False", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
                val = "True";

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(val, "True", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0 && CultureInfo.CurrentCulture.CompareInfo.Compare(val, "False", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
            {
                var argetype = CFGEnums.CFGEventType.cfgCritical;
                string argdescription = Utilities.GetResourceString("TXT_MsgUnableToProcessLineIf");
                CFGVariable argvariable = null;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, _currentLine, this._file, variable: argvariable, varname: argvarname);    // Event
                _currentLine.HasErrors = true;
                return "";
            }

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(val, "True", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                _ifStack.Pop();
                _ifStack.Push(IfStackStates.TrueState);
            }

            var argetype1 = CFGEnums.CFGEventType.cfgMessage;
            string argdescription1 = string.Format(Utilities.GetResourceString("TXT_MsgIfStructureIsSet"), ProcessNextLine());
            CFGVariable argvariable1 = null;
            string argvarname1 = "";
            _workspace.AddEvent(ref argetype1, ref argdescription1, _currentLine, this._file, argvariable1, varname: argvarname1);

            return val;
        }

        // TODO: check for errors and add BCE events
        private string Process_ELIF(string forcedValue = "")
        {
            // Processes the %elif directive

            // Processes the %elif directive
            // Check if the state is valid to process ELIF
            // It should be in start/initial state for elif to get processed
            if (!Equals(_ifStack.Peek(), IfStackStates.StartState))
            {
                var argetype = CFGEnums.CFGEventType.cfgCritical;
                string argdescription = Utilities.GetResourceString("TXT_MsgUnableToProcessLineElif");
                CFGVariable argvariable = null;
                string argvarname = "";

                _workspace.AddEvent(ref argetype, ref argdescription, _currentLine, this._file, variable: argvariable, varname: argvarname);
                _currentLine.HasErrors = true;
                return "";
            }

            // If _lastOperator IsNot Nothing AndAlso _lastOperator.Precedence > _tokens.Current.Precedence Then
            // _lastOperator = _tokens.Current
            // Return String.Empty
            // End If
            // _lastOperator = _tokens.Current

            _tokens.MoveNext();

            string val;
            if (!string.IsNullOrEmpty(forcedValue))
            {
                val = forcedValue;
            }
            else
            {
                val = ParseExpression(true);
            }

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(val, "0", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                val = "False";

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(val, "False", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
                val = "True";

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(val, "True", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0 && CultureInfo.CurrentCulture.CompareInfo.Compare(val, "False", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
            {
                var argetype1 = CFGEnums.CFGEventType.cfgCritical;
                string argdescription1 = Utilities.GetResourceString("TXT_MsgUnableToProcessLineIf");
                CFGVariable argvariable1 = null;
                string argvarname1 = "";
                _workspace.AddEvent(ref argetype1, ref argdescription1, _currentLine, this._file, variable: argvariable1, varname: argvarname1);    // Event
                _currentLine.HasErrors = true;
                return "";
            }


            if (CultureInfo.CurrentCulture.CompareInfo.Compare(val, "True", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                // While NextLine() AndAlso _tokens.Current() IsNot Nothing AndAlso _tokens.Current().TokenType <> TT.ttELIF AndAlso _tokens.Current().TokenType <> TT.ttELSE AndAlso _tokens.Current().TokenType <> TT.ttENDIF
                // ParseLine(_lines.Current())
                _ifStack.Pop();
                _ifStack.Push(IfStackStates.TrueState);
            }

            return val;
        }

        // TODO: check for errors and add BCE events
        private void Process_ELSE()
        {
            // Processes the %else directive

            int currentState = Convert.ToInt32(_ifStack.Peek());
            _ifStack.Pop();

            if (currentState == (int)IfStackStates.StartState)
            {
                _ifStack.Push(IfStackStates.TrueState);
            }
            else // FalseState or TrueState
            {
                _ifStack.Push(IfStackStates.FalseState);
            }

            _tokens.MoveNext();
        }

        private void Process_ENDIF()
        {

            // Processes the %endif directive

            if (_ifStack.Count == 0)
            {
                var argetype = CFGEnums.CFGEventType.cfgCritical;
                string argdescription = Utilities.GetResourceString("TXT_MsgUnableToProcessThisIfBlock");
                CFGVariable argvariable = null;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, _currentLine, this._file, variable: argvariable, varname: argvarname);
                _currentLine.HasErrors = true;
                return;
            }

            _ifStack.Pop();

            var argetype1 = CFGEnums.CFGEventType.cfgMessage;
            string argdescription1 = string.Format(Utilities.GetResourceString("TXT_MsgIfStructureIsSet"), ProcessNextLine());
            CFGVariable argvariable1 = null;
            string argvarname1 = "";
            _workspace.AddEvent(ref argetype1, ref argdescription1, _currentLine, this._file, argvariable1, varname: argvarname1);
            if (_currentLine.Indent > 0)
                _currentLine.Indent -= 1;

            _tokens.MoveNext();

            _inIf = false;
        }
        private void Process_INCLUDE()
        {
            // Processes the %include directive

            string path;
            string value;
            string filename;
            var files = new ArrayList();
            DirectoryInfo di;
            FileInfo[] fils;
            var has_level = default(bool);
            var level = default(int);
            CFGToken leveltoken;

            _tokens.MoveNext(); // move past exists token
            if (_tokens.Current.TokenType == CFGEnums.TT.ttOPENPARENTHESIS)
            {
                _tokens.MoveNext(); // move past ( token
            }

            if (_tokens.Current is null)
            {
                var argetype = CFGEnums.CFGEventType.cfgError;
                string argdescription = CEResource.TXT_MsgUnableToProcessLineINCLUDE;
                var argline = _lines.Current;
                CFGVariable argvariable = null;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, variable: argvariable, varname: argvarname);
                _lines.Current.HasErrors = true;
                return;
            }

            value = ParseExpression(true);

            path = UtilitiesPath.GetDirectoryName(ref value);
            filename = UtilitiesPath.GetFileName(value);

            // '%include statements can have levels i.e.  %include test.cfg level 4
            if (_tokens.Current is not null)
            {
                if (_tokens.Current.TokenType == CFGEnums.TT.ttLevelNo)
                {
                    has_level = true;
                    leveltoken = _tokens.Current;
                    level = (int)Math.Round(VBVal(leveltoken.Value)); // TODO: what if level is non-numeric like Role?

                    _tokens.MoveNext();
                }
            }

            // If token.TokenType <> TT.ttString AndAlso token.TokenType <> TT.ttStringWithParenthesis AndAlso token.TokenType <> TT.ttStringWithQuotes Then
            // _workspace.AddEvent(CFGEventType.cfgError, CEResource.TXT_MsgUnableToProcessLineINCLUDEBraces, _lines.Current(), _file)
            // _lines.Current().HasErrors = True
            // Exit Sub
            // End If

            // 'If the value is only a filename then derive the full path from this cfg_file
            // If value = "*.cfg" AndAlso path = "" AndAlso has_level AndAlso _operandStack.Count > 0 AndAlso _operandStack.Peek().TokenType <> TT.ttLevelNo Then
            // token = _operandStack.Pop()
            // value = token.Value & "\" & value
            // value = Replace(value, "\\", "\")
            // path = UtilitiesPath.GetDirectoryName(value)
            // filename = UtilitiesPath.GetFileName(value)
            // ElseIf value = "\*.cfg" AndAlso path = "\" AndAlso has_level AndAlso _operandStack.Count > 0 AndAlso _operandStack.Peek().TokenType <> TT.ttLevelNo Then
            // token = _operandStack.Pop()
            // value = token.Value & value
            // value = Replace(value, "\\", "\")
            // path = UtilitiesPath.GetDirectoryName(value)
            // filename = UtilitiesPath.GetFileName(value)
            // End If

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(VBLCase(value) ?? "", VBLCase(filename) ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                path = Path.Join(
                    UtilitiesPath.GetDirectoryName(ref _file.FilePath),
                    Path.DirectorySeparatorChar.ToString());
                if (path.Length > 2)
                {
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(VBMid(path, 1, 2), @"\\", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                    {
                        path = @"\" + VBReplace(path, @"\\", @"\");
                    }
                    else
                    {
                        path = VBReplace(path, @"\\", @"\");
                    }
                }
            }

            if (string.IsNullOrEmpty(filename))
                return;

            value = path + filename;

            try
            {
                if (string.IsNullOrEmpty(VBDir(value)))
                {
                    var argetype1 = CFGEnums.CFGEventType.cfgWarning;
                    string argdescription1 = CEResource.TXT_MsgNoFilesToINCLUDE;
                    var argline1 = _lines.Current;
                    CFGVariable argvariable1 = null;
                    string argvarname1 = "";
                    _workspace.AddEvent(ref argetype1, ref argdescription1, argline1, this._file, variable: argvariable1, varname: argvarname1);
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                var argetype2 = CFGEnums.CFGEventType.cfgError;
                string argdescription2 = CEResource.TXT_ErrCannotResolvePathInclude + value;
                var argline2 = _lines.Current;
                CFGVariable argvariable2 = null;
                string argvarname2 = "";
                _workspace.AddEvent(ref argetype2, ref argdescription2, argline2, this._file, variable: argvariable2, varname: argvarname2);
                return;
            }

            // Include statements can have wildcards
            if (VBInStr(filename, "*") > 0)
            {
                di = new DirectoryInfo(path);
                fils = di.GetFiles(filename);

                // Fixed file sort order to match Windows.
                Array.Sort(fils, new FileInfoComparer());

                foreach (var fiNext in fils)
                    files.Add(path + fiNext.Name);
            }
            else
            {
                files.Add(value);
            }

            // Include each file
            foreach (string currentPath in files)
            {
                path = currentPath;
                if (_workspace.AbortProcessing)
                    return;
                if (has_level)
                {
                    var argetype3 = CFGEnums.CFGEventType.cfgInclude;
                    string argdescription3 = _file.Name + CEResource.TXT_MsgIsIncluding + UtilitiesPath.GetFileName(path) + CEResource.TXT_MsgIsIncluding + level;
                    var argline3 = _lines.Current;
                    CFGVariable argvariable3 = null;
                    string argvarname3 = "";
                    _workspace.AddEvent(ref argetype3, ref argdescription3, argline3, this._file, variable: argvariable3, varname: argvarname3);

                    _workspace.ProcessFile(path, this._file, level);
                }
                else
                {
                    var argetype4 = CFGEnums.CFGEventType.cfgInclude;
                    string argdescription4 = _file.Name + CEResource.TXT_MsgWithLevel + UtilitiesPath.GetFileName(path);
                    var argline4 = _lines.Current;
                    CFGVariable argvariable4 = null;
                    string argvarname4 = "";
                    _workspace.AddEvent(ref argetype4, ref argdescription4, argline4, this._file, variable: argvariable4, varname: argvarname4);
                    _workspace.ProcessFile(path,this._file);
                }
            }
        }

        private string Process_BASENAME(string path)
        {
            // Processes the basename() function

            string argpath = UtilitiesPath.GetFileName(path);
            string val = UtilitiesPath.GetPathWithoutExtension(ref argpath);
            return val;
        }

        private string Process_DOLLAR(bool inFunction)
        {
            string val;

            _inVar = true;
            _tokens.MoveNext(); // move past $
            if (inFunction || _tokens.Current.TokenType == CFGEnums.TT.ttOPENBRACKET)
            {
                if (_tokens.Current.TokenType == CFGEnums.TT.ttOPENBRACKET || _tokens.Current.TokenType == CFGEnums.TT.ttOPENPARENTHESIS)
                {
                    _tokens.MoveNext(); // move past ( or {
                }

                val = ParseExpression(true);
            }
            else
            {
                if (_tokens.Current.TokenType == CFGEnums.TT.ttOPENBRACKET || _tokens.Current.TokenType == CFGEnums.TT.ttOPENPARENTHESIS)
                {
                    _tokens.MoveNext(); // move past ( or {
                }

                val = ParseExpression(false);
            }

            _inVar = false;

            return val;
        }

        private string Process_CONCAT()
        {
            // Processes the concat() function

            string val = string.Empty;

            _tokens.MoveNext(); // move past function
            _tokens.MoveNext(); // move past (

            val += ParseExpression(true);

            return val;
        }

        private string Process_BUILD()
        {
            // Processes the build() function

            string val = string.Empty;

            _tokens.MoveNext(); // move past function
            _tokens.MoveNext(); // move past (

            val += ParseExpression(true);

            return val;
        }

        private string Process_DEVDIR(string path)
        {
            // Processes the devdir() function

            string val;
            int t;
            string[] mis;

            path = UtilitiesPath.GetDirectoryName(ref path);

            path = VBReplace(path, "/", @"\");
            mis = VBSplit(path, @"\");

            val = "";
            if (VBUBound(mis) > 1)
            {
                var loopTo = VBUBound(mis);
                for (t = 0; t <= loopTo; t++)
                {
                    if (!string.IsNullOrEmpty(mis[t]))
                    {
                        val = val + mis[t] + @"\";
                    }
                }
            }

            return val;
        }

        private string Process_DEV(string path)
        {
            // Processes the dev() function

            string val = VBMid(path, 1, VBInStr(path, ":"));
            return val;
        }

        private string Process_DIR(string path)
        {
            // Processes the dir() function

            string val;
            int t;
            string[] mis;

            path = UtilitiesPath.GetDirectoryName(ref path);

            path = VBReplace(path, "/", @"\");
            mis = VBSplit(path, @"\");

            val = "";
            if (VBUBound(mis) > 1)
            {
                var loopTo = VBUBound(mis);
                for (t = 0; t <= loopTo; t++)
                {
                    if (t == 0)
                    {
                        if (VBInStr(mis[0], ":") > 0)
                        {
                            val = @"\";
                        }
                        else
                        {
                            val = mis[0] + @"\";
                        }
                    }
                    else if (!string.IsNullOrEmpty(mis[t]))
                    {
                        val = val + mis[t] + @"\";
                    }
                }
            }

            return val;
        }

        private string Process_EXT(string path)
        {
            // Processes the ext() function

            string val = UtilitiesPath.ExtFromPath(ref path);
            return val;
        }

        private string Process_FILENAME(string path)
        {
            // Processes the filename() function

            string val = UtilitiesPath.GetFileName(path);
            return val;
        }

        private string Process_FIRST(string path)
        {
            // Processes the first() function

            string[] mis = VBSplit(path, ";");

            return mis[0];
        }

        private string Process_FIRSTDIRPIECE(string path)
        {
            // Processes the firstdirpiece() function

            string val = string.Empty;
            string[] mis;

            path = UtilitiesPath.GetDirectoryName(ref path);

            if (!string.IsNullOrEmpty(path))
            {
                path = VBReplace(path, "/", @"\");
                mis = VBSplit(path, @"\");

                val = mis[1];
            }

            return val;
        }

        private string Process_LASTDIRPIECE(string path)
        {
            // Processes the lastdirpiece() function

            if (path is null)
                return string.Empty;

            string val;
            string[] mis;

            path = UtilitiesPath.GetDirectoryName(ref path);
            path = VBReplace(path, "/", @"\");

            if (path is null)
                return string.Empty;

            mis = VBSplit(path, @"\");
            val = mis[VBUBound(mis) - 1];

            return val;
        }

        private string Process_NOEXT(string path)
        {
            // Processes the noext() function

            path = VBReplace(path, "/", @"\");
            string val = UtilitiesPath.GetPathWithoutExtension(ref path);
            return val;
        }

        private string Process_PARENTDEVDIR(string path)
        {
            // Processes the registryread() function

            string val = string.Empty;
            string[] mis;
            int t;

            path = UtilitiesPath.GetDirectoryName(ref path);
            path = VBReplace(path, "/", @"\");
            mis = VBSplit(path, @"\");

            if (VBUBound(mis) > 1)
            {
                var loopTo = VBUBound(mis) - 2;
                for (t = 0; t <= loopTo; t++)
                {
                    if (!string.IsNullOrEmpty(mis[t]))
                    {
                        val = val + mis[t] + @"\";
                    }
                }
            }

            return val;
        }

        private string Process_BACKTRACK(string path)
        {
            return Process_PARENTDEVDIR(path);
        }

        private string Process_PARENTDIR(string path)
        {
            // Processes the parentdir() function

            string val = string.Empty;
            string[] mis;
            int t;

            path = UtilitiesPath.GetDirectoryName(ref path);
            path = VBReplace(path, "/", @"\");
            mis = VBSplit(path, @"\");

            if (VBUBound(mis) > 1)
            {
                var loopTo = VBUBound(mis) - 2;
                for (t = 0; t <= loopTo; t++)
                {
                    if (t == 0)
                    {
                        if (VBInStr(mis[0], ":") > 0)
                        {
                            val = @"\";
                        }
                        else
                        {
                            val = mis[0] + @"\";
                        }
                    }
                    else if (!string.IsNullOrEmpty(mis[t]))
                    {
                        val = val + mis[t] + @"\";

                    }
                }
            }

            return val;
        }

        private string Process_REGISTRYREAD(string path)
        {
            // Processes the registryread() function

            // Remove all double quotes from the path
            string val = path.Replace("\"", "");
            int valuePosition = val.LastIndexOf('\\');

            if (valuePosition > 0)
            {
                string regPath = val.Substring(0, valuePosition);
                string regValue = val.Substring(valuePosition + 1);

                object readKey = Registry.GetValue(regPath, regValue, null);
                if (readKey != null)
                {
                    val = readKey.ToString() + @"\";
                }
            }

            return val;
        }

        private void Process_LOCK()
        {
            // Processes the %lock directive

            CFGToken token;
            CFGVariable Variable;

            _tokens.MoveNext();  // move past %lock token

            if (_tokens.Current is null)
            {
                var argetype = CFGEnums.CFGEventType.cfgError;
                string argdescription = CEResource.TXT_MsgUnableToProcessLineLock;
                var argline = _lines.Current;
                CFGVariable argvariable = null;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, variable: argvariable, varname: argvarname);
                _lines.Current.HasErrors = true;
                return;
            }

            token = _tokens.Current;

            if (token.TokenType != CFGEnums.TT.ttString)
            {
                var argetype1 = CFGEnums.CFGEventType.cfgError;
                string argdescription1 = CEResource.TXT_MsgUnableToProcessLineLock;
                var argline1 = _lines.Current;
                CFGVariable argvariable1 = null;
                string argvarname1 = "";
                _workspace.AddEvent(ref argetype1, ref argdescription1, argline1, this._file, variable: argvariable1, varname: argvarname1);
                _lines.Current.HasErrors = true;
                return;
            }

            // Get a reference to the variable if it exists
            Variable = _workspace.GetVariable(token.Value);

            if (Variable is null)
            {
                var argetype2 = CFGEnums.CFGEventType.cfgWarning;
                string argdescription2 = CEResource.TXT_MsgAttemptToLockAnUndefinedVariable;
                var argline2 = _lines.Current;
                CFGVariable argvariable2 = null;
                _workspace.AddEvent(ref argetype2, ref argdescription2, argline2, this._file, varname:token.Value, variable: argvariable2);
                return;
            }

            if (Variable.IsDefined == false)
            {
                var argetype3 = CFGEnums.CFGEventType.cfgWarning;
                string argdescription3 = CEResource.TXT_MsgAttemptToLockAnUndefinedVariable;
                var argline3 = _lines.Current;
                _workspace.AddEvent(ref argetype3, ref argdescription3, argline3, this._file, Variable, token.Value);
                return;
            }

            Variable.IsLocked = true;

            var argetype4 = CFGEnums.CFGEventType.cfgVarlocked;
            string argdescription4 = CEResource.TXT_MsgVariableLocked;
            var argline4 = _lines.Current;
            _workspace.AddEvent(ref argetype4, ref argdescription4, argline4, this._file, Variable, token.Value);
        }

        // Note: aligned to old CFGFile version
        private void Process_UNDEF()
        {
            // Processes the %undef directive

            CFGToken token;
            string value;
            CFGVariable Variable;

            _tokens.MoveNext();  // move past %undef token

            if (_tokens.Current is null)
            {
                var argetype = CFGEnums.CFGEventType.cfgError;
                string argdescription = CEResource.TXT_MsgUnableToProcessLineINCLUDE;
                var argline = _lines.Current;
                CFGVariable argvariable = null;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, variable: argvariable, varname: argvarname);
                _lines.Current.HasErrors = true;
                return;
            }

            token = _tokens.Current;
            value = ParseArgument(false);

                            // TODO: should the token type be checked?
            // If token.TokenType <> TT.ttString AndAlso token.TokenType <> TT.ttStringWithParenthesis AndAlso token.TokenType <> TT.ttStringWithQuotes Then
            // ParentWorkspace.AddEvent(CFGEventType.cfgError, CEResource.TXT_MsgUnableToProcessLineUndef, _currentLine, Me)
            // _currentLine.HasErrors = True
            // Exit Sub
            // End If

            // If token.OriginalType <> TT.ttString Then
            // ParentWorkspace.AddEvent(CFGEventType.cfgCritical, CEResource.TXT_MsgInvalidUndefStatement, _currentLine, Me)
            // _currentLine.HasErrors = True
            // Exit Sub
            // End If

            // Get a reference to the variable if it exists
            Variable = _workspace.GetVariable(token.Value);

            // Determine if the variable is defined
            if (Variable is not null)
            {

                if (Variable.IsLocked == false)
                {
                    Variable.Undefine();
                    var argetype1 = CFGEnums.CFGEventType.cfgVarundef;
                    string argdescription1 = CEResource.TXT_MsgVariableUndefined;
                    var argline1 = _lines.Current;
                    string argvarname1 = "";
                    _workspace.AddEvent(ref argetype1, ref argdescription1, argline1, this._file, Variable, varname: argvarname1);
                }
                else
                {
                    var argetype2 = CFGEnums.CFGEventType.cfgWarning;
                    string argdescription2 = CEResource.TXT_MsgAttemptToUndefineLockedVariable;
                    var argline2 = _lines.Current;
                    string argvarname2 = "";
                    _workspace.AddEvent(ref argetype2, ref argdescription2, argline2,  this._file, Variable, varname: argvarname2);
                    return;
                }
            }
            else
            {
                var argetype3 = CFGEnums.CFGEventType.cfgWarning;
                string argdescription3 = CEResource.TXT_MsgAttemptToUndefineVariableThatDoesNotExist;
                var argline3 = _lines.Current;
                CFGVariable argvariable1 = null;
                _workspace.AddEvent(ref argetype3, ref argdescription3, argline3, this._file, varname: token.Value, variable: argvariable1);
                return;
            }
        }

        // Note: aligned to old CFGFile version
        private string Process_DEFINED()
        {
            // Processes the defined() function

            _tokens.MoveNext(); // move past defined token
            if (_tokens.Current.TokenType == CFGEnums.TT.ttOPENPARENTHESIS)
            {
                _tokens.MoveNext(); // move past ( token
            }

            CFGVariable Variable;
            string name;
            bool IsDefined;



            if (_tokens.Current is null)
            {
                var argetype = CFGEnums.CFGEventType.cfgError;
                string argdescription = CEResource.TXT_MsgUnableToProcessLineDefined;
                var argline = _lines.Current;
                CFGVariable argvariable = null;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, variable: argvariable, varname: argvarname);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }

            name = _tokens.Current.Value;

            // Get a reference to the variable if it exists
            Variable = _workspace.GetVariable(name);

            // Determine if the variable is defined
            if (Variable is null)
            {
                IsDefined = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(name));
            }
            else
            {
                IsDefined = Variable.IsDefined;
            }

            // Original
            // Dim v2 As String = _workspace.IsVariableDefined(_tokens.Current.Value)

            _tokens.MoveNext(); // move past )

            string temptext;
            temptext = ParseExpression(true);
            if (!string.IsNullOrEmpty(temptext))
            {
                IsDefined = Convert.ToBoolean(temptext);
            }
            var argetype1 = CFGEnums.CFGEventType.cfgMessage;
            string argdescription1 = string.Format(CEResource.TXT_MsgCheckingVariableExistance, name) + IsDefined;
            var argline1 = _lines.Current;
            _workspace.AddEvent(ref argetype1, ref argdescription1, argline1, this._file, Variable, name);

            return IsDefined.ToString();
        }

        private object Process_EXISTS()
        {
            // Processes the exists() function


            _tokens.MoveNext(); // move past exists token
            if (_tokens.Current.TokenType == TT.ttOPENPARENTHESIS)
            {
                _tokens.MoveNext(); // move past ( token
            }

            string tokenValue = _tokens.Current.Value;
            string value = ParseExpression(true);
            value = value.Trim();

            if (string.IsNullOrEmpty(value))
            {
                _tokens.MoveNext(); // move past )
                return false;
            }

            string dirPath = Path.GetDirectoryName(value);
            string fileNameOnly = Path.GetFileName(value);
            string dirRoot = Directory.GetDirectoryRoot(value);
            string dirPartialPath = dirPath.Substring(dirRoot.Length);
            string directorySearchPath = Path.Combine(dirRoot, dirPartialPath);

            string[] dirs;

            try
            {
                if (!Directory.Exists(directorySearchPath))
                {
                    _tokens.MoveNext(); // move past )
                    return false;
                }

                string searchRoot = Path.GetDirectoryName(directorySearchPath);
                string searchPattern = Path.GetFileName(directorySearchPath);
                dirs = Directory.GetDirectories(searchRoot, searchPattern);
            }
            catch (DirectoryNotFoundException)
            {
                _tokens.MoveNext(); // move past )
                return false;
            }
            catch (ArgumentException)
            {
                _tokens.MoveNext(); // move past )
                return false;
            }
            catch
            {
                _tokens.MoveNext(); // move past )
                return false;
            }

            bool dirCheck = false;
            string lastChr = value.Substring(value.Length - 1);

            // Dim Test As Boolean = True
            try
            {
                if (fileNameOnly == "")
                {
                    dirCheck = true;
                }

                if (dirs.Length == 0)
                {
                    _tokens.MoveNext(); // move past )
                    return false;
                }
                else if (dirs.Length > 0 && dirCheck)
                {
                    _tokens.MoveNext(); // move past )
                    return true;
                }
                else if (dirs.Length > 0 && !dirCheck)
                {
                    foreach (string dir in dirs)
                    {
                        string[] files = Directory.GetFiles(dir, fileNameOnly);
                        if (files.Length > 0)
                        {
                            _tokens.MoveNext(); // move past )
                            return true;
                        }
                    }

                    _tokens.MoveNext(); // move past )
                    return false;
                }
                else
                {
                    _tokens.MoveNext(); // move past )
                    return false;
                }
            }
            catch
            {
                _tokens.MoveNext(); // move past )
                return false;
            }
        }

        private void Process_FEATURE()
        {
            // Process feature() function

            Process_IF("True");
        }

        private string Process_AND(bool inFunction, string v1)
        {
            // Process the && boolean function

            if (_lastOperator is not null && _lastOperator.Precedence > _tokens.Current.Precedence)
            {
                _lastOperator = _tokens.Current;
                return string.Empty;
            }
            _lastOperator = _tokens.Current;

            _tokens.MoveNext(); // move past && token

            string v2 = ParseExpression(inFunction);
            bool result;

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "0", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0 & CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "False", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
            {
                v1 = "True";
            }

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(v2, "0", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0 & CultureInfo.CurrentCulture.CompareInfo.Compare(v2, "False", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
            {
                v2 = "True";
            }

            result = CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "True", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 && CultureInfo.CurrentCulture.CompareInfo.Compare(v2, "True", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0;

            return result.ToString();
        }

        private string Process_OR(bool inFunction, string v1)
        {
            // Process the || boolean function

            if (_lastOperator is not null && _lastOperator.Precedence > _tokens.Current.Precedence)
            {
                _lastOperator = _tokens.Current;
                return string.Empty;
            }
            _lastOperator = _tokens.Current;

            _tokens.MoveNext(); // move past || token

            string v2 = ParseExpression(inFunction);
            bool result;

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "0", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0 & CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "False", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
            {
                v1 = "True";
            }
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(v2, "0", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0 & CultureInfo.CurrentCulture.CompareInfo.Compare(v2, "False", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
            {
                v2 = "True";
            }

            result = CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "True", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 || CultureInfo.CurrentCulture.CompareInfo.Compare(v2, "True", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0;

            return result.ToString();
        }

        private string Process_NOT(bool inFunction)
        {
            // Process the ! boolean function

            if (_lastOperator is not null && _lastOperator.Precedence > _tokens.Current.Precedence)
            {
                _lastOperator = _tokens.Current;
                return string.Empty;
            }
            _lastOperator = _tokens.Current;

            _tokens.MoveNext(); // move past ! token

            string v1 = ParseExpression(inFunction);
            bool result;

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "0", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0 & CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "False", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
            {
                v1 = "True";
            }

            result = !(CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "True", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0);

            return result.ToString();
        }

        #region Aligned to old CFGFile

        // Note: aligned to old CFGFile version
        private string Process_NOTEQUALS(bool inFunction, string v1)
        {
            // Process the != operator

            if (_lastOperator is not null && _lastOperator.Precedence > _tokens.Current.Precedence)
            {
                _lastOperator = _tokens.Current;
                return string.Empty;
            }
            _lastOperator = _tokens.Current;

            _tokens.MoveNext(); // move past != token

            string v2 = ParseExpression(inFunction);
            bool result;

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "~error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 || CultureInfo.CurrentCulture.CompareInfo.Compare(v2, "~error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                var argetype = CFGEnums.CFGEventType.cfgError;
                string argdescription = CEResource.TXT_MsgUnableToProcessLineNotEqual;
                var argline = _lines.Current;
                CFGVariable argvariable = null;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, variable: argvariable, varname: argvarname);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }

            if (!(VBIsNumeric(v1) && VBIsNumeric(v2)))
            {
                // Compare as strings
                result = CultureInfo.CurrentCulture.CompareInfo.Compare(v1.ToString() ?? "", v2.ToString() ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0;
            }
            else
            {
                // Compare as numbers
                result = VBVal(v1) != VBVal(v2);
            }

            return result.ToString();
        }

        // Note: aligned to old CFGFile version, except for setting v1 and v2
        private string Process_EQUALS(bool inFunction, string v1)
        {
            // Process the == operator

            if (_lastOperator is not null && _lastOperator.Precedence > _tokens.Current.Precedence)
            {
                _lastOperator = _tokens.Current;
                return string.Empty;
            }
            _lastOperator = _tokens.Current;

            _tokens.MoveNext(); // move past == token

            // CFGFile version:
            // v1 = Get_Value_From_String(True, True)
            // v2 = Get_Value_From_String(True, True)

            string v2 = ParseExpression(inFunction);
            bool result;

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "~error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 || CultureInfo.CurrentCulture.CompareInfo.Compare(v2, "~error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                var argetype = CFGEnums.CFGEventType.cfgError;
                string argdescription = CEResource.TXT_MsgUnableToProcessLineEqual;
                var argline = _lines.Current;
                CFGVariable argvariable = null;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, variable: argvariable, varname: argvarname);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }

            if (!(VBIsNumeric(v1) && VBIsNumeric(v2)))
            {
                // Treat as strings

                if (string.Compare(v1, v2, false) == 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(VBLCase(v1) ?? "", VBLCase(v2) ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                    {
                        var argetype1 = CFGEnums.CFGEventType.cfgWarning;
                        string argdescription1 = CEResource.TXT_MsgComparisonFailedDueToCaseSensitivity + v1 + " == " + v2;
                        var argline1 = _lines.Current;
                        CFGVariable argvariable1 = null;
                        string argvarname1 = "";
                        _workspace.AddEvent(ref argetype1, ref argdescription1, argline1, this._file, variable: argvariable1, varname: argvarname1);
                    }
                }
            }

            else
            {
                // Treat as numbers
                result = VBVal(v1) == VBVal(v2);
            }

            return result.ToString();
        }

        // Note: aligned to old CFGFile version
        private string Process_GT(bool inFunction, string v1)
        {
            // Process the > operator

            if (_lastOperator is not null && _lastOperator.Precedence > _tokens.Current.Precedence)
            {
                _lastOperator = _tokens.Current;
                return string.Empty;
            }
            _lastOperator = _tokens.Current;

            _tokens.MoveNext(); // move past > token

            string v2 = ParseExpression(inFunction);
            bool result;

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "~Error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 || CultureInfo.CurrentCulture.CompareInfo.Compare(v2, "~Error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                var argetype = CFGEnums.CFGEventType.cfgError;
                string argdescription = CEResource.TXT_MsgUnableToProcessLineGreaterThan;
                var argline = _lines.Current;
                CFGVariable argvariable = null;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, variable: argvariable, varname: argvarname);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }

            if (!(VBIsNumeric(v1) && VBIsNumeric(v2)))
            {
                // treat as strings
                result = string.Compare(v1, v2, StringComparison.OrdinalIgnoreCase) > 0;
            }
            else
            {
                // treat as numbers
                result = VBVal(v1) > VBVal(v2);
            }

            return result.ToString();
        }

        // Note: aligned to old CFGFile version
        private string Process_GREATERTHANOREQUAL(bool inFunction, string v1)
        {
            // Process the >= operator

            if (_lastOperator is not null && _lastOperator.Precedence > _tokens.Current.Precedence)
            {
                _lastOperator = _tokens.Current;
                return string.Empty;
            }

            _lastOperator = _tokens.Current;

            _tokens.MoveNext(); // move past >= token

            string v2 = ParseExpression(inFunction);
            bool result;

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "~error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 || CultureInfo.CurrentCulture.CompareInfo.Compare(v2, "~error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                var argetype = CFGEnums.CFGEventType.cfgError;
                string argdescription = CEResource.TXT_MsgUnableToProcessLineGreaterThanEqual;
                var argline = _lines.Current;
                CFGVariable argvariable = null;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, variable: argvariable, varname: argvarname);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }

            if (!(VBIsNumeric(v1) && VBIsNumeric(v2)))
            {
                // treat as strings
                result = string.Compare(v1, v2, StringComparison.OrdinalIgnoreCase) >= 0;

            }
            else
            {
                // treat as numbers
                result = VBVal(v1) >= VBVal(v2);
            }

            return result.ToString();
        }

        // Note: aligned to old CFGFile version
        private string Process_LT(bool inFunction, string v1)
        {
            // Process the < operator

            if (_lastOperator is not null && _lastOperator.Precedence > _tokens.Current.Precedence)
            {
                _lastOperator = _tokens.Current;
                return string.Empty;
            }
            _lastOperator = _tokens.Current;

            _tokens.MoveNext(); // move past < token

            string v2 = ParseExpression(inFunction);
            bool result;

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "~Error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 || CultureInfo.CurrentCulture.CompareInfo.Compare(v2, "~Error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                var argetype = CFGEnums.CFGEventType.cfgError;
                string argdescription = CEResource.TXT_MsgUnableToProcessLineLessThan;
                var argline = _lines.Current;
                CFGVariable argvariable = null;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, variable: argvariable, varname: argvarname);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }

            if (!(VBIsNumeric(v1) && VBIsNumeric(v2)))
            {
                // treat as strings
                result = string.Compare(v1, v2, StringComparison.OrdinalIgnoreCase) < 0;
            }
            else
            {
                // treat as numbers
                result = VBVal(v1) < VBVal(v2);
            }

            return result.ToString();
        }

        // Note: aligned to old CFGFile version
        private string Process_LESSTHANOREQUAL(bool inFunction, string v1)
        {
            // Process the <= operator

            if (_lastOperator is not null && _lastOperator.Precedence > _tokens.Current.Precedence)
            {
                _lastOperator = _tokens.Current;
                return string.Empty;
            }
            _lastOperator = _tokens.Current;

            _tokens.MoveNext(); // move past <= token

            string v2 = ParseExpression(inFunction);
            bool result;

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "~error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 || CultureInfo.CurrentCulture.CompareInfo.Compare(v2, "~error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                var argetype = CFGEnums.CFGEventType.cfgError;
                string argdescription = CEResource.TXT_MsgUnableToProcessLineLessThanEqual;
                var argline = _lines.Current;
                CFGVariable argvariable = null;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, variable: argvariable, varname: argvarname);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }

            if (!(VBIsNumeric(v1) && VBIsNumeric(v2)))
            {
                // treat as strings
                result = string.Compare(v1, v2, StringComparison.OrdinalIgnoreCase) <= 0;
            }
            else
            {
                // treat as numbers
                result = VBVal(v1) <= VBVal(v2);
            }

            return result.ToString();
        }

        // Note: aligned to old CFGFile version
        private string Process_ADD(bool inFunction, string v1)
        {
            // Process the + operator

            if (_lastOperator is not null && _lastOperator.Precedence > _tokens.Current.Precedence)
            {
                _lastOperator = _tokens.Current;
                return string.Empty;
            }
            _lastOperator = _tokens.Current;


            _tokens.MoveNext(); // move past * token

            string v2 = ParseExpression(inFunction);
            string result;

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "~Error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 || CultureInfo.CurrentCulture.CompareInfo.Compare(v2, "~Error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                var argetype = CFGEnums.CFGEventType.cfgError;
                string argdescription = CEResource.TXT_MsgUnableToProcessLineMATHADD;
                var argline = _lines.Current;
                CFGVariable argvariable = null;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, variable: argvariable, varname: argvarname);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }

            if (!(VBIsNumeric(v1) && VBIsNumeric(v2)))
            {
                // treat them as strings
                result = v1 + v2;
            }
            else
            {
                // Treat as numbers
                result = VBInt(VBVal(v1) + VBVal(v2)).ToString().Trim();
            }

            return result;
        }

        // Note: aligned to old CFGFile version
        private string Process_SUBTRACT(bool inFunction, string v1)
        {
            // Process the - operator

            if (_lastOperator is not null && _lastOperator.Precedence > _tokens.Current.Precedence)
            {
                _lastOperator = _tokens.Current;
                return string.Empty;
            }
            _lastOperator = _tokens.Current;

            _tokens.MoveNext(); // move past * token

            string v2 = ParseExpression(inFunction);
            string result;

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "~error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 || CultureInfo.CurrentCulture.CompareInfo.Compare(v2, "~error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                var argetype = CFGEnums.CFGEventType.cfgError;
                string argdescription = CEResource.TXT_MsgUnableToProcessLineMATHSUBTRACT;
                var argline = _lines.Current;
                CFGVariable argvariable = null;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, variable: argvariable, varname: argvarname);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }

            if (!(VBIsNumeric(v1) && VBIsNumeric(v2)))
            {
                var argetype1 = CFGEnums.CFGEventType.cfgError;
                string argdescription1 = CEResource.TXT_ErrOperandTypeErrorWithSubraction;
                var argline1 = _lines.Current;
                CFGVariable argvariable1 = null;
                string argvarname1 = "";
                _workspace.AddEvent(ref argetype1, ref argdescription1, argline1, this._file, variable: argvariable1, varname: argvarname1);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }
            else
            {
                // Treat as numbers
                result = VBInt(VBVal(v1) - VBVal(v2)).ToString().Trim();
            }

            return result;
        }

        // Note: aligned to old CFGFile version
        private string Process_BITAND(bool inFunction, string v1)
        {
            // Process the + operator

            if (_lastOperator is not null && _lastOperator.Precedence > _tokens.Current.Precedence)
            {
                _lastOperator = _tokens.Current;
                return string.Empty;
            }
            _lastOperator = _tokens.Current;

            _tokens.MoveNext(); // move past * token

            string v2 = ParseExpression(inFunction);
            string result;

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "~error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 || CultureInfo.CurrentCulture.CompareInfo.Compare(v2, "~error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                var argetype = CFGEnums.CFGEventType.cfgError;
                string argdescription = CEResource.TXT_MsgUnableToProcessLineMATHAND;
                var argline = _lines.Current;
                CFGVariable argvariable = null;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, variable: argvariable, varname: argvarname);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }

            if (!(VBIsNumeric(v1) && VBIsNumeric(v2)))
            {
                var argetype1 = CFGEnums.CFGEventType.cfgError;
                string argdescription1 = CEResource.TXT_ErrOperandTypeErrorWithSubraction;
                var argline1 = _lines.Current;
                CFGVariable argvariable1 = null;
                string argvarname1 = "";
                _workspace.AddEvent(ref argetype1, ref argdescription1, argline1, this._file, variable: argvariable1, varname: argvarname1);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }
            // Treat as numbers
            else if (VBVal(v2) != 0d)
            {
                result = ((long)Math.Round(Math.Floor(Convert.ToDouble(v1))) &
                    (long)Math.Floor(Convert.ToDouble(v2)))
                    .ToString().Trim();
            }
            else
            {
                var argetype2 = CFGEnums.CFGEventType.cfgError;
                string argdescription2 = CEResource.TXT_ErrModulusByZero;
                var argline2 = _lines.Current;
                CFGVariable argvariable2 = null;
                string argvarname2 = "";
                _workspace.AddEvent(ref argetype2, ref argdescription2, argline2, this._file, variable: argvariable2, varname: argvarname2);
                _lines.Current.HasErrors = true;
                return string.Empty;

            }

            return result;
        }

        // Note: aligned to old CFGFile version
        private string Process_BITOR(bool inFunction, string v1)
        {
            // Process the | operator

            if (_lastOperator is not null && _lastOperator.Precedence > _tokens.Current.Precedence)
            {
                _lastOperator = _tokens.Current;
                return string.Empty;
            }
            _lastOperator = _tokens.Current;

            _tokens.MoveNext(); // move past * token

            string v2 = ParseExpression(inFunction);
            string result;

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "~Error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 || CultureInfo.CurrentCulture.CompareInfo.Compare(v2, "~Error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                var argetype = CFGEnums.CFGEventType.cfgError;
                string argdescription = CEResource.TXT_MsgUnableToProcessLineMATHOR;
                var argline = _lines.Current;
                CFGVariable argvariable = null;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, variable: argvariable, varname: argvarname);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }

            if (!(VBIsNumeric(v1) && VBIsNumeric(v2)))
            {
                var argetype1 = CFGEnums.CFGEventType.cfgError;
                string argdescription1 = CEResource.TXT_ErrOperandTypeErrorWithSubraction;
                var argline1 = _lines.Current;
                CFGVariable argvariable1 = null;
                string argvarname1 = "";
                _workspace.AddEvent(ref argetype1, ref argdescription1, argline1, this._file, variable: argvariable1, varname: argvarname1);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }
            // Treat as numbers
            else if (VBVal(v2) != 0d)
            {
                result = ((long)Math.Round(Math.Floor(Convert.ToDouble(v1))) |
                    (long)Math.Floor(Convert.ToDouble(v2)))
                    .ToString().Trim();
            }
            else
            {
                var argetype2 = CFGEnums.CFGEventType.cfgError;
                string argdescription2 = CEResource.TXT_ErrModulusByZero;
                var argline2 = _lines.Current;
                CFGVariable argvariable2 = null;
                string argvarname2 = "";
                _workspace.AddEvent(ref argetype2, ref argdescription2, argline2, this._file, variable: argvariable2, varname: argvarname2);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }

            return result;
        }

        // Note: aligned to old CFGFile version
        private string Process_BITXOR(bool inFunction, string v1)
        {
            // Process the ^ operator

            if (_lastOperator is not null && _lastOperator.Precedence > _tokens.Current.Precedence)
            {
                _lastOperator = _tokens.Current;
                return string.Empty;
            }
            _lastOperator = _tokens.Current;

            _tokens.MoveNext(); // move past * token

            string v2 = ParseExpression(inFunction);
            string result;

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "~error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 || CultureInfo.CurrentCulture.CompareInfo.Compare(v2, "~error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                var argetype = CFGEnums.CFGEventType.cfgError;
                string argdescription = CEResource.TXT_MsgUnableToProcessLineMATHXOR;
                var argline = _lines.Current;
                CFGVariable argvariable = null;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, variable: argvariable, varname: argvarname);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }

            if (!(VBIsNumeric(v1) && VBIsNumeric(v2)))
            {
                var argetype1 = CFGEnums.CFGEventType.cfgError;
                string argdescription1 = CEResource.TXT_ErrOperandTypeErrorWithSubraction;
                var argline1 = _lines.Current;
                CFGVariable argvariable1 = null;
                string argvarname1 = "";
                _workspace.AddEvent(ref argetype1, ref argdescription1, argline1, this._file, variable: argvariable1, varname: argvarname1);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }
            // Treat as numbers
            else if (VBVal(v2) != 0d)
            {
                result = ((long)Math.Round(Math.Floor(Convert.ToDouble(v1))) |
                    (long)Math.Floor(Convert.ToDouble(v2)))
                    .ToString().Trim();
            }
            else
            {
                var argetype2 = CFGEnums.CFGEventType.cfgError;
                string argdescription2 = CEResource.TXT_ErrModulusByZero;
                var argline2 = _lines.Current;
                CFGVariable argvariable2 = null;
                string argvarname2 = "";
                _workspace.AddEvent(ref argetype2, ref argdescription2, argline2, this._file, variable: argvariable2, varname: argvarname2);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }

            return result;
        }

        // Note: aligned to old CFGFile version
        private string Process_MODULUS(bool inFunction, string v1)
        {
            // Process the + operator

            if (_lastOperator is not null && _lastOperator.Precedence > _tokens.Current.Precedence)
            {
                _lastOperator = _tokens.Current;
                return string.Empty;
            }
            _lastOperator = _tokens.Current;

            if (_lastOperator is not null && _lastOperator.Precedence > _tokens.Current.Precedence)
            {
                _lastOperator = _tokens.Current;
                return string.Empty;
            }
            _lastOperator = _tokens.Current;

            _tokens.MoveNext(); // move past * token

            string v2 = ParseExpression(inFunction);
            string result;

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "~Error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 || CultureInfo.CurrentCulture.CompareInfo.Compare(v2, "~Error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                var argetype = CFGEnums.CFGEventType.cfgError;
                string argdescription = CEResource.TXT_MsgUnableToProcessLineMATHMODULUS;
                var argline = _lines.Current;
                CFGVariable argvariable = null;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, variable: argvariable, varname: argvarname);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }

            if (!(VBIsNumeric(v1) && VBIsNumeric(v2)))
            {
                var argetype1 = CFGEnums.CFGEventType.cfgError;
                string argdescription1 = CEResource.TXT_ErrOperandTypeErrorWithMODULUS;
                var argline1 = _lines.Current;
                CFGVariable argvariable1 = null;
                string argvarname1 = "";
                _workspace.AddEvent(ref argetype1, ref argdescription1, argline1, this._file, variable: argvariable1, varname: argvarname1);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }
            // Treat as numbers
            else if (VBVal(v2) != 0d)
            {
                result = (int.Parse(v1) % int.Parse(v2)).ToString().Trim();
            }
            else
            {
                var argetype2 = CFGEnums.CFGEventType.cfgError;
                string argdescription2 = CEResource.TXT_ErrModulusByZero;
                var argline2 = _lines.Current;
                CFGVariable argvariable2 = null;
                string argvarname2 = "";
                _workspace.AddEvent(ref argetype2, ref argdescription2, argline2, this._file, variable: argvariable2, varname: argvarname2);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }

            return result;
        }

        // Note: aligned to old CFGFile version
        private string Process_MULTIPLY(bool inFunction, string v1)
        {
            // Process the * operator

            if (_lastOperator is not null && _lastOperator.Precedence > _tokens.Current.Precedence)
            {
                _lastOperator = _tokens.Current;
                return string.Empty;
            }
            _lastOperator = _tokens.Current;

            _tokens.MoveNext(); // move past * token

            if (_lastOperator is not null && _lastOperator.Precedence > _tokens.Current.Precedence)
            {
                _lastOperator = _tokens.Current;
                return string.Empty;
            }
            _lastOperator = _tokens.Current;

            string v2 = ParseExpression(inFunction);
            string result;

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "~error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 || CultureInfo.CurrentCulture.CompareInfo.Compare(v2, "~error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                var argetype = CFGEnums.CFGEventType.cfgError;
                string argdescription = CEResource.TXT_MsgUnableToProcessLineMATHMULIPLY;
                var argline = _lines.Current;
                CFGVariable argvariable = null;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, variable: argvariable, varname: argvarname);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }

            if (!(VBIsNumeric(v1) && VBIsNumeric(v2)))
            {
                var argetype1 = CFGEnums.CFGEventType.cfgError;
                string argdescription1 = CEResource.TXT_ErrOperandTypeErrorWithMULTIPLY;
                var argline1 = _lines.Current;
                CFGVariable argvariable1 = null;
                string argvarname1 = "";
                _workspace.AddEvent(ref argetype1, ref argdescription1, argline1, this._file, variable: argvariable1, varname: argvarname1);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }
            else
            {
                // Treat as numbers
                result = (VBInt(VBVal(v1)) * VBInt(VBVal(v2))).ToString().Trim();
            } // TODO: should this be int?

            return result;
        }

        // Note: aligned to old CFGFile version
        private string Process_SHIFTLEFT(bool inFunction, string v1)
        {
            // Process the << operator

            if (_lastOperator is not null && _lastOperator.Precedence > _tokens.Current.Precedence)
            {
                _lastOperator = _tokens.Current;
                return string.Empty;
            }
            _lastOperator = _tokens.Current;

            _tokens.MoveNext(); // move past << token

            string v2 = ParseExpression(inFunction);
            bool result;

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "~Error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 || CultureInfo.CurrentCulture.CompareInfo.Compare(v2, "~Error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                var argetype = CFGEnums.CFGEventType.cfgError;
                string argdescription = "Unable To process line (math) SHIFTLEFT ";
                var argline = _lines.Current;
                CFGVariable argvariable = null;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, variable: argvariable, varname: argvarname);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }

            if (!(VBIsNumeric(v1) && VBIsNumeric(v2)))
            {
                var argetype1 = CFGEnums.CFGEventType.cfgError;
                string argdescription1 = CEResource.TXT_ErrOperandTypeErrorWithSubraction;
                var argline1 = _lines.Current;
                CFGVariable argvariable1 = null;
                string argvarname1 = "";
                _workspace.AddEvent(ref argetype1, ref argdescription1, argline1, this._file, variable: argvariable1, varname: argvarname1);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }
            // Treat as numbers
            else if (Convert.ToDouble(v1) != 0)
            {
                int num1 = Convert.ToInt32(Convert.ToDouble(v1));
                int num2 = Convert.ToInt32(Convert.ToDouble(v2));

                long shifted = (long)num1 << num2;
                result = shifted != 0;
            }
            else
            {
                var argetype2 = CFGEnums.CFGEventType.cfgError;
                string argdescription2 = CEResource.TXT_ErrModulusByZero;
                var argline2 = _lines.Current;
                CFGVariable argvariable2 = null;
                string argvarname2 = "";
                _workspace.AddEvent(ref argetype2, ref argdescription2, argline2, this._file, variable: argvariable2, varname: argvarname2);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }

            return result.ToString();
        }

        // Note: aligned to old CFGFile version
        private string Process_SHIFTRIGHT(bool inFunction, string v1)
        {
            // Process the >> operator

            if (_lastOperator is not null && _lastOperator.Precedence > _tokens.Current.Precedence)
            {
                _lastOperator = _tokens.Current;
                return string.Empty;
            }
            _lastOperator = _tokens.Current;

            _tokens.MoveNext(); // move past >> token

            string v2 = ParseExpression(inFunction);
            bool result;

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "~Error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 || CultureInfo.CurrentCulture.CompareInfo.Compare(v2, "~Error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                var argetype = CFGEnums.CFGEventType.cfgError;
                string argdescription = CEResource.TXT_MsgUnableToProcessLineMATHSHIFTRIGHT;
                var argline = _lines.Current;
                CFGVariable argvariable = null;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, variable: argvariable, varname: argvarname);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }

            if (!(VBIsNumeric(v1) && VBIsNumeric(v2)))
            {
                var argetype1 = CFGEnums.CFGEventType.cfgError;
                string argdescription1 = CEResource.TXT_ErrOperandTypeErrorWithSubraction;
                var argline1 = _lines.Current;
                CFGVariable argvariable1 = null;
                string argvarname1 = "";
                _workspace.AddEvent(ref argetype1, ref argdescription1, argline1, this._file, variable: argvariable1, varname: argvarname1);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }
            // Treat as numbers
            else if (Convert.ToDouble(v1) != 0)
            {
                int num1 = Convert.ToInt32(Convert.ToDouble(v1));
                int num2 = Convert.ToInt32(Convert.ToDouble(v2));

                long shifted = (long)num1 >> num2;
                result = shifted != 0;
            }

            else
            {
                var argetype2 = CFGEnums.CFGEventType.cfgError;
                string argdescription2 = CEResource.TXT_ErrModulusByZero;
                var argline2 = _lines.Current;
                CFGVariable argvariable2 = null;
                string argvarname2 = "";
                _workspace.AddEvent(ref argetype2, ref argdescription2, argline2, this._file, variable: argvariable2, varname: argvarname2);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }

            return result.ToString();
        }

        // Note: aligned to old CFGFile version
        private string Process_DIVIDE(bool inFunction, string v1)
        {
            // Process the / operator

            if (_lastOperator is not null && _lastOperator.Precedence > _tokens.Current.Precedence)
            {
                _lastOperator = _tokens.Current;
                return string.Empty;
            }
            _lastOperator = _tokens.Current;

            _tokens.MoveNext(); // move past / token

            string v2 = ParseExpression(inFunction);
            string result;

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(v1, "~error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 || CultureInfo.CurrentCulture.CompareInfo.Compare(v2, "~error", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                var argetype = CFGEnums.CFGEventType.cfgError;
                string argdescription = CEResource.TXT_MsgUnableToProcessLineMATHDIVIDE;
                var argline = _lines.Current;
                CFGVariable argvariable = null;
                string argvarname = "";
                _workspace.AddEvent(ref argetype, ref argdescription, argline, this._file, variable: argvariable, varname: argvarname);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }

            if (!(VBIsNumeric(v1) && VBIsNumeric(v2)))
            {
                var argetype1 = CFGEnums.CFGEventType.cfgError;
                string argdescription1 = CEResource.TXT_ErrOperandTypeErrorWithDIVIDE;
                var argline1 = _lines.Current;
                CFGVariable argvariable1 = null;
                string argvarname1 = "";
                _workspace.AddEvent(ref argetype1, ref argdescription1, argline1, this._file, variable: argvariable1, varname: argvarname1);
                _lines.Current.HasErrors = true;
                return string.Empty;
            }
            // Treat as numbers
            else if (VBInt(VBVal(v1)) != 0d)
            {
                result = VBInt(VBInt(VBVal(v1)) / VBInt(VBVal(v2))).ToString().Trim(); // TODO: should this be int?
            }
            else
            {
                var argetype2 = CFGEnums.CFGEventType.cfgError;
                string argdescription2 = CEResource.TXT_ErrDivideByZero;
                var argline2 = _lines.Current;
                CFGVariable argvariable2 = null;
                string argvarname2 = "";
                _workspace.AddEvent(ref argetype2, ref argdescription2, argline2, this._file, variable: argvariable2, varname: argvarname2);
                _lines.Current.HasErrors = true;
                return string.Empty;

            }

            return result;
        }

        #endregion

        #endregion

    }
}
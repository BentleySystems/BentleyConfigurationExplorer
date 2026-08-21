// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections;

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{


    [Serializable()]
    [JsonObject(MemberSerialization.Fields)]
    public class CFGLine
    {

        // ---------------------------------------------------------------------------------------
        // Private Members
        // ---------------+---------------+---------------+---------------+---------------+-------
        private static int _lineCounter = 0;
        private static readonly Dictionary<CFGEnums.TT, string> _tokenValues = InitTokenValues();
        private static Regex _funcDefRegx = new Regex(@"[ ]*(?<FUNCTION>[ ]*(?<NAME>\$|defined|exists|parentdevdir|parentdir|devdir|dir|dev|basename|filename|noext|ext|first|concat|build|registryread|lastdirpiece|firstdirpiece)[ ]*)[({]{1}?", RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex _btrackDefRegx = new Regex(@"[ ]*(?<BACKTRACK>[ ]*(?<NAME>\.\.\\))", RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex _preProcRegx = new Regex("(?<PREPROC>[%][ ]*(?<PROCTYPE>iffeature|ifndef|ifdef|if|elif|else|endif|lock|undef|error|echo|level|include))(?<ARGUMENT>.*)", RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex _varRefRegx = new Regex(@"(?<USEVAR>\((?<VARIABLE>[^])}({]*)\))", RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex _spclCharRegx = new Regex(@"(?<SPCLCHAR>[ ]*(?<CHAR>\|\||&&|\$\(|\$\{|!=|==|[!,])[ ]*)", RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex _varDefRegx = new Regex("(?<VARIABLE>[^:=<>+]*)(?<OPERATOR>[:=<>+]?)(?<ARGUMENT>.*)", RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex _varValRegx = new Regex(@"(?<USEVAR>\$\([\s]*(?<VARIABLE>[a-zA-Z0-9_\-\$\*\.\,\@\!\?\\\/\;]*)[\s]*\))", RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex _varValAbsRegx = new Regex(@"(?<USEVAR>\$\{[\s]*(?<VARIABLE>[a-zA-Z0-9_\-\$\*\.\,\@\!\?\\\/\;]*)[\s]*\})", RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex _stringParensRegx = new Regex(@"\([\s]*(?<USEVARSTRING>(?<STRING>[a-zA-Z0-9_\-\$\*\.\,\@\!\?\\\/\;]*))[\s]*\)", RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex _stringBracketsRegx = new Regex(@"\{[\s]*(?<USEVARSTRING>(?<STRING>[a-zA-Z0-9_\-\$\*\.\,\@\!\?\\\/\;]*))[\s]*\}", RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex _preProcCharRegx = new Regex(@"(?<PREPROCCHAR>[ ]*(?<CHAR>\|\||&&|\||<<|>>|<=|>=|-|\+|&|<|>|%|\*|\^|!=|==|[!,])[ ]*)", RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex _parenthesisRegx = new Regex("(?<PARENTHESIS>[{()}])", RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex _stringQuote = new Regex("(?<QUOTE>" + '"' + "[^#" + '"' + "]*" + '"' + ")", RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex _stringRegx = new Regex("[#]?(?<STRING>[^#]*)[#]?", RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex _includeLevel = new Regex("(?<LEVEL>[ ]*level(?<LEVELNO>.*))", RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex _varTagRegx = new Regex(@"(?<TAG>\[[\s]*[a-zA-Z0-9_\-\$\*\.\,\@\!\?\\\/\;]*[\s]*\])", RegexOptions.Singleline | RegexOptions.Compiled);

        private bool _reportEvents;
        private int _mathCharCount;
        private bool _argumentOnly;

        // ---------------------------------------------------------------------------------------
        // Public Members
        // ---------------+---------------+---------------+---------------+---------------+-------
        public string Text;                                               // Text of the line
        public int LineNumber;                                        // Line number relative to cfgfile
        public List<CFGToken> Tokens;
        public CFGFile ParentFile;                                        // Reference to the parent file
        public bool IsVarDef;                                          // Is this line a variable definition?
        public bool IsPreproc;                                         // Is this line a preprocessor directive?
        public bool IsIfStructure;                                     // Is this line an %if %endif %elif or %else
        public bool IsComment;                                         // Is this line a comment
        public bool IsIF;                                              // Is this line an %if statement
        public int Indent;                                            // Measure of the if structure depth (used to reformat the text)
        public string InlineComment;                                      // Does this line have a comment
        public bool HasErrors;                                         // Any errors found
        public bool HasFunction;
        public bool HasWarnings;                                       // Any warnings found
        public string VarName;                                            // Variable name (if one exists)
        public string VarArg;

        public bool debugLineOutput = false;

        public CFGLine()
        {
            _lineCounter += 1;
            HasErrors = false;
            Tokens = new List<CFGToken>();
        }

        ~CFGLine()
        {
            Text = null;
            Tokens.Clear();
            Tokens = null;
            ParentFile = null;
        }

        public static int LineCount
        {
            get
            {
                return _lineCounter;
            }
            set
            {
                _lineCounter = value;
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Tokenizes a single line from a cfg file
        // ---------------+---------------+---------------+---------------+---------------+-------
        public List<CFGToken> TokenizeLine(bool ArgOnly = false)
        {
            _argumentOnly = ArgOnly;
            _reportEvents = !ArgOnly;                          // Don't report events when processing an argument

            if (_reportEvents && ParentFile is null)
            {
                HasErrors = true;
                return null;
            }

            // Cleanup the line before processing
            RemoveComments();

            if (string.IsNullOrEmpty(Text))
                return null;

            CheckPreprocTabs();
            Text = Text.Replace("\t", " ").Trim();  // Clear tab characters
            FixSlashes();

            if (string.IsNullOrEmpty(Text))
            {
                IsComment = true;
                return null;
            }

            if (debugLineOutput && Text is not null && !string.IsNullOrEmpty(Text))
            {
                Debug.WriteLine("line CFGLine: " + Text);
            }

            // Begin processing the line
            if (ArgOnly == false)
            {
                if (Text.StartsWith("%"))
                {
                    // It's a pre-processor directive
                    IsVarDef = false;
                    IsPreproc = true;
                    Text = "%" + Text.Substring(1).Trim(); // Clear spaces between % and directive
                    ParsePreproc();
                }
                else
                {
                    // It's a variable definition
                    if (Text.StartsWith("+"))
                    {
                        Text = Text.Substring(1).Trim(); // Plus signs are ignored at the beginning of a line
                    }
                    IsVarDef = true;
                    IsPreproc = false;
                    ParseVardef();
                }
            }
            else
            {
                // ParseLine Argument
                ParseArgument(Text, 0);
            }

            // Run clean up procedures on tokens in preparation for processing
            Tokens.Sort();

            if (IsIF && _mathCharCount > 0)
            {
                ConvertFunctionChars();
            }

            CombineTokens();
            ConvertParenthesis();
            InsertArgumentCountTokens();
            Convert_SPCLCHARS();
            InsertImpliedTokens();
            // CheckFunctionCallDefs()
            SetPvals();                   // sets token precedence
            SetTypes();                   // sets token original type

            // Can this line modify if else structure of parent file?
            if (Tokens.Count > 0)
            {
                {
                    var withBlock = Tokens[0];
                    IsIfStructure = withBlock.TokenType == CFGEnums.TT.ttIF || withBlock.TokenType == CFGEnums.TT.ttENDIF || withBlock.TokenType == CFGEnums.TT.ttELSE || withBlock.TokenType == CFGEnums.TT.ttELIF;
                }
            }

            // If no tokens were found then the line couldn't be parsed
            if (Tokens.Count == 0)
            {
                Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name} Line not parsed");
                if (_reportEvents)
                {
                    var argetype = CFGEnums.CFGEventType.cfgError;
                    string argdescription = CEResource.TXT_ErrCannotParseThisLine;
                    var argline = this;
                    CFGVariable argvariable = null;
                    string argvarname = "";
                    ParentFile.ParentConfiguration.AddEvent(ref argetype, ref argdescription, argline, ParentFile, variable: argvariable, varname: argvarname);
                } // Event
                HasErrors = true;
            }

            return Tokens;
        }

        // ---------------------------------------------------------------------------------------
        // @description:    ParseLine a variable definition line i.e.  myvar = c:\temp\
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void ParseVardef()
        {
            Match mtch;
            bool warnonce = false;

            HasErrors = true;
            IsIF = false;

            mtch = _varDefRegx.Match(Text);

            {
                var withBlock = mtch.Groups["VARIABLE"];

                // No variable name exists
                if (string.IsNullOrEmpty(withBlock.Value) && _reportEvents)
                {
                    Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name} No variable name exists");
                    var argetype = CFGEnums.CFGEventType.cfgError;
                    string argdescription = CEResource.TXT_ErrZeroLengthVariableName;
                    var argline = this;
                    CFGVariable argvariable = null;
                    string argvarname = "";
                    ParentFile.ParentConfiguration.AddEvent(ref argetype, ref argdescription, argline, ParentFile, variable: argvariable, varname: argvarname); // Event
                    return;
                }

                // Variable name is too short
                if (withBlock.Value.Length < 1)
                {
                    Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name} Variable name is too short");
                    var argetype1 = CFGEnums.CFGEventType.cfgError;
                    string argdescription1 = CEResource.TXT_ErrNameShouldBeMinTwoChar;
                    var argline1 = this;
                    CFGVariable argvariable1 = null;
                    string argvarname1 = "";
                    ParentFile.ParentConfiguration.AddEvent(ref argetype1, ref argdescription1, argline1, ParentFile, variable: argvariable1, varname: argvarname1);    // Event
                    return;
                }

                // Variable names can't have these characters
                // Technically they can but they cause errors later on
                foreach (var c in withBlock.Value.Trim())
                {
                    if (c == '[' || c == '{' || c == '}' || c == '(' || c == ')')
                    {
                        Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                            "Variable names can't have these characters" + " File:" + this.ParentFile.Name + "LineNum:" + this.LineNumber.ToString() + "Text: " + this.Text);

                        var argetype2 = CFGEnums.CFGEventType.cfgError;
                        string argdescription2 = CEResource.TXT_ErrBadCharacterInVariableName;
                        var argline2 = this;
                        CFGVariable argvariable2 = null;
                        string argvarname2 = "";

                        ParentFile.ParentConfiguration.AddEvent(ref argetype2, ref argdescription2, argline2, ParentFile, variable: argvariable2, varname: argvarname2);
                        HasErrors = true;
                        return;
                    }
                    else if (!warnonce && !char.IsLetterOrDigit(c) && c != '_')
                    {
                        Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name} Variable names shouldn't contain non-standard characters");

                        var argetype3 = CFGEnums.CFGEventType.cfgWarning;
                        string argdescription3 = CEResource.TXT_ErrVarNameShouldNotContainNonStandardChars;
                        var argline3 = this;
                        CFGVariable argvariable3 = null;
                        string argvarname3 = "";

                        ParentFile.ParentConfiguration.AddEvent(ref argetype3, ref argdescription3, argline3, ParentFile, variable: argvariable3, varname: argvarname3);
                        warnonce = true;
                    }

                }

                // Add variable token
 
                AddToken(CFGEnums.TT.ttVarName,
                    CFGEnums.TokenGroup.tgOperand, 
                    withBlock.Value, 
                    withBlock.Index);
                VarName = withBlock.Value.Trim();
            }

            {
                var withBlock1 = mtch.Groups["OPERATOR"];
                // No operator found
                if (withBlock1.Success == false | string.IsNullOrEmpty(withBlock1.Value))
                {
                    Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name} No Operator found");
                    var argetype4 = CFGEnums.CFGEventType.cfgCritical;
                    string argdescription4 = CEResource.TXT_ErrMissingAssignmentOperator;
                    var argline4 = this;
                    CFGVariable argvariable4 = null;
                    string argvarname4 = "";
                    ParentFile.ParentConfiguration.AddEvent(ref argetype4, ref argdescription4, argline4, ParentFile, variable: argvariable4, varname: argvarname4);    // Event
                    return;
                }

                // Add operator token
                AddToken(StringToTokenType(withBlock1.Value), CFGEnums.TokenGroup.tgAssignment, withBlock1.Value, withBlock1.Index);
            }

            // ParseLine argument
            {
                var withBlock2 = mtch.Groups["ARGUMENT"];
                VarArg = withBlock2.Value.Trim();
                ParseArgument(VarArg, withBlock2.Index);
            }

            HasErrors = false;
        }

        // ---------------------------------------------------------------------------------------
        // @description:    ParseLine a preprocessor directive line i.e. %if
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void ParsePreproc()
        {
            Match mtch;
            string arg;

            mtch = _preProcRegx.Match(Text);

            HasErrors = true;
            IsIF = false;

            {
                var withBlock = mtch.Groups["PROCTYPE"];
                // No variable name exists
                if (string.IsNullOrEmpty(withBlock.Value))
                {
                    Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name} No variable name exists");
                    var argetype = CFGEnums.CFGEventType.cfgError;
                    string argdescription = CEResource.TXT_ErrUnknownDirectiveName;
                    var argline = this;
                    CFGVariable argvariable = null;
                    string argvarname = "";
                    ParentFile.ParentConfiguration.AddEvent(ref argetype, ref argdescription, argline, ParentFile, variable: argvariable, varname: argvarname);    // Event
                    return;
                }

                // Add preproc token
                AddToken(StringToTokenType(withBlock.Value), 
                    CFGEnums.TokenGroup.tgPreProc, 
                    withBlock.Value, 
                    withBlock.Index);
            }

            // ParseLine argument
            {
                var withBlock1 = mtch.Groups["ARGUMENT"];
                arg = withBlock1.Value.Trim();

                // Include statements can have a Level # at the end which has to be specifically detected
                if (Tokens[Tokens.Count - 1].TokenType == CFGEnums.TT.ttINCLUDE)
                    ParseIncludeLevel(ref arg, withBlock1.Index);

                ParseArgument(arg, withBlock1.Index);
            }

            HasErrors = false;
        }

        // ---------------------------------------------------------------------------------------
        // @description:    Uses regular expressions to tokenize the argument portion of a line
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void ParseArgument(string arg, int offset)
        {
            Match mtch;
            MatchCollection mtchs;
            string value;
            Group withBlock = null;
            //var withBlock;

            if (string.IsNullOrWhiteSpace(arg))
                return;

            arg = "#" + arg;

            // Find variable definitions $(variable)
            mtchs = _varValRegx.Matches(arg);
            foreach (Match currentMtch in mtchs)
            {
                mtch = currentMtch;
                {
                    withBlock = mtch.Groups["USEVAR"];
                    if (!string.IsNullOrEmpty(withBlock.Value?.Trim()))
                    {
                        AddToken(CFGEnums.TT.ttVarValue, 
                            CFGEnums.TokenGroup.tgOperand, 
                            mtch.Groups["VARIABLE"].Value, 
                            offset + withBlock.Index);
                        //var midTmp = Strings.Replace(Strings.Space(withBlock.Length), " ", "#", Compare: CompareMethod.Text);
                        //StringType.MidStmtStr(ref arg, withBlock.Index + 1, midTmp.Length, midTmp);
                        arg = ReplaceSubstringWithRepeatedChar(arg, withBlock.Index, withBlock.Length, '#');
                    }
                }
            }

            // Find variable definitions ${variable}
            mtchs = _varValAbsRegx.Matches(arg);
            foreach (Match currentMtch in mtchs)
            {
                mtch = currentMtch;
                {
                    withBlock = mtch.Groups["USEVAR"];
                    if (!string.IsNullOrEmpty(withBlock.Value?.Trim()))
                    {
                        AddToken(CFGEnums.TT.ttVarValueCurrent, 
                            CFGEnums.TokenGroup.tgOperand, 
                            mtch.Groups["VARIABLE"].Value, 
                            offset + withBlock.Index);
                        //var midTmp = Strings.Replace(Strings.Space(withBlock.Length), " ", "#", Compare: CompareMethod.Text);
                        //StringType.MidStmtStr(ref arg, withBlock.Index + 1, midTmp.Length, midTmp);
                        arg = ReplaceSubstringWithRepeatedChar(arg, withBlock.Index, withBlock.Length, '#');
                    }
                }
            }

            // Find function definitions
            mtchs = _funcDefRegx.Matches(arg);
            foreach (Match currentMtch in mtchs)
            {
                mtch = currentMtch;
                {
                    withBlock = mtch.Groups["FUNCTION"];
                    if (!string.IsNullOrEmpty(withBlock.Value?.Trim()))
                    {
                        AddToken(StringToTokenType(mtch.Groups["NAME"].Value), 
                            CFGEnums.TokenGroup.tgFunction, 
                            mtch.Groups["NAME"].Value, 
                            offset + withBlock.Index);
                        //var midTmp = Strings.Replace(Strings.Space(withBlock.Length), " ", "#", Compare: CompareMethod.Text);
                        //StringType.MidStmtStr(ref arg, withBlock.Index + 1, midTmp.Length, midTmp);
                        arg = ReplaceSubstringWithRepeatedChar(arg, withBlock.Index, withBlock.Length, '#');
                        HasFunction = true;
                    }
                }
            }

            // Find function definitions
            mtchs = _btrackDefRegx.Matches(arg);
            foreach (Match currentMtch in mtchs)
            {
                mtch = currentMtch;
                {
                    withBlock = mtch.Groups["BACKTRACK"];
                    if (!string.IsNullOrEmpty(withBlock.Value?.Trim()))
                    {
                        AddToken(StringToTokenType(mtch.Groups["NAME"].Value), 
                            CFGEnums.TokenGroup.tgFunction, 
                            mtch.Groups["NAME"].Value, 
                            offset + withBlock.Index);
                        //var midTmp = Strings.Replace(Strings.Space(withBlock.Length), " ", "#", Compare: CompareMethod.Text);
                        //StringType.MidStmtStr(ref arg, withBlock.Index + 1, midTmp.Length, midTmp);
						arg = ReplaceSubstringWithRepeatedChar(arg, withBlock.Index, withBlock.Length, '#');
                        //HasFunction = true;
                    }
                }
            }

            // TODO: testing removal of stringInParenthesis
            // 'Find strings in parentheses that can be potential variables
            mtchs = _stringParensRegx.Matches(arg);
            foreach (Match currentMtch in mtchs)
            {
                mtch = currentMtch;
                {
                    withBlock = mtch.Groups["USEVARSTRING"];
                    if (!string.IsNullOrEmpty(withBlock.Value?.Trim()))
                    {
                        AddToken(CFGEnums.TT.ttStringWithParenthesis, 
                            CFGEnums.TokenGroup.tgOperand, 
                            mtch.Groups["STRING"].Value, 
                            offset + withBlock.Index);
                        //var midTmp = Strings.Replace(Strings.Space(withBlock.Length), " ", "#", Compare: CompareMethod.Text);
                        //StringType.MidStmtStr(ref arg, withBlock.Index + 1, midTmp.Length, midTmp);
                        arg = ReplaceSubstringWithRepeatedChar(arg, withBlock.Index, withBlock.Length, '#');
                    }
                }
            }

            // Find strings in parentheses that can be potential variables
            mtchs = _stringBracketsRegx.Matches(arg);
            foreach (Match currentMtch in mtchs)
            {
                mtch = currentMtch;
                {
                    withBlock = mtch.Groups["USEVARSTRING"];
                    if (!string.IsNullOrEmpty(withBlock.Value?.Trim()))
                    {
                        AddToken(CFGEnums.TT.ttStringWithBrackets, 
                            CFGEnums.TokenGroup.tgOperand, 
                            mtch.Groups["STRING"].Value, 
                            offset + withBlock.Index);
                        //var midTmp = Strings.Replace(Strings.Space(withBlock.Length), " ", "#", Compare: CompareMethod.Text);
                        //StringType.MidStmtStr(ref arg, withBlock.Index + 1, midTmp.Length, midTmp);
                        arg = ReplaceSubstringWithRepeatedChar(arg, withBlock.Index, withBlock.Length, '#');
                    }
                }
            }

            // Find strings within quotes "text"
            mtchs = _stringQuote.Matches(arg);
            foreach (Match currentMtch in mtchs)
            {
                mtch = currentMtch;
                {
                    withBlock = mtch.Groups["QUOTE"];
                    if (!string.IsNullOrEmpty(withBlock.Value))
                    {
                        value = withBlock.Value?.Replace("\"", string.Empty);

                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            value = value.Trim();
                        }

                        //var argTokenType = CFGEnums.TT.ttStringWithQuotes;
                        //var argTokenGroup = CFGEnums.TokenGroup.tgOperand;
                        AddToken(CFGEnums.TT.ttStringWithQuotes, 
                            CFGEnums.TokenGroup.tgOperand, 
                            value, 
                            offset + withBlock.Index); // TODO: testing

                        //var midTmp = Strings.Replace(Strings.Space(withBlock.Length), " ", "#", Compare: CompareMethod.Text);
                        //StringType.MidStmtStr(ref arg, withBlock.Index + 1, midTmp.Length, midTmp);
                        arg = ReplaceSubstringWithRepeatedChar(arg, withBlock.Index, withBlock.Length, '#');
                    }
                }
            }

            // Find special preproc chars for if statements (math)
            _mathCharCount = 0;
            if (IsPreproc)
            {
                if (Tokens.Count > 0)
                {
                    if (Tokens[0].TokenType == CFGEnums.TT.ttIF || Tokens[0].TokenType == CFGEnums.TT.ttELIF)
                    {
                        IsIF = true;
                        mtchs = _preProcCharRegx.Matches(arg);
                        foreach (Match currentMtch in mtchs)
                        {
                            mtch = currentMtch;
                            {
                                withBlock = mtch.Groups["PREPROCCHAR"];
                                if (!string.IsNullOrWhiteSpace(withBlock.Value))
                                {
                                    string trimmedValue = withBlock.Value.Trim();
                                    var argTokenType = StringToTokenType(trimmedValue);
                                    string argvalue5 = trimmedValue;

                                    AddToken(argTokenType,
                                             CFGEnums.TokenGroup.tgBooleanFunc,
                                             trimmedValue,
                                             offset + withBlock.Index);

                                    arg = ReplaceSubstringWithRepeatedChar(arg, withBlock.Index, withBlock.Length, '#');
                                    _mathCharCount += 1;
                                }
                            }
                        }
                    }
                }
            }

            mtchs = _spclCharRegx.Matches(arg);
            foreach (Match currentMtch in mtchs)
            {
                mtch = currentMtch;
                {
                    withBlock = mtch.Groups["SPCLCHAR"];
                    if (!string.IsNullOrWhiteSpace(withBlock.Value))
                    {
                        string trimmedValue = withBlock.Value.Trim();

                        AddToken(
                            StringToTokenType(trimmedValue),
                            CFGEnums.TokenGroup.tgBooleanFunc,
                            trimmedValue,
                            offset + withBlock.Index
                        );

                        arg = ReplaceSubstringWithRepeatedChar(arg, withBlock.Index, withBlock.Length, '#');
                    }
                }
            }

            // Find parenthesis
            mtchs = _parenthesisRegx.Matches(arg);
            foreach (Match currentMtch in mtchs)
            {
                mtch = currentMtch;
                {
                    withBlock = mtch.Groups["PARENTHESIS"];
                    if (!string.IsNullOrEmpty(withBlock.Value?.Trim()))
                    {
                        AddToken(StringToTokenType(withBlock.Value?.Trim()), 
                            CFGEnums.TokenGroup.tgGroupingChar, 
                            withBlock.Value?.Trim(), 
                            offset + withBlock.Index);
                        //var midTmp = Strings.Replace(Strings.Space(withBlock.Length), " ", "#", Compare: CompareMethod.Text);
                        //StringType.MidStmtStr(ref arg, withBlock.Index + 1, midTmp.Length, midTmp);
                        arg = ReplaceSubstringWithRepeatedChar(arg, withBlock.Index, withBlock.Length, '#');
                    }
                }
            }

            // Find remaining strings
            mtchs = _stringRegx.Matches(arg);
            foreach (Match currentMtch in mtchs)
            {
                mtch = currentMtch;
                {
                    withBlock = mtch.Groups["STRING"];
                    if (!string.IsNullOrEmpty(withBlock.Value))
                    {
                        AddToken(CFGEnums.TT.ttString, 
                            CFGEnums.TokenGroup.tgOperand, 
                            withBlock.Value, 
                            offset + withBlock.Index);
                        //var midTmp = Strings.Replace(Strings.Space(withBlock.Length), " ", "#", Compare: CompareMethod.Text);
                        //StringType.MidStmtStr(ref arg, withBlock.Index + 1, midTmp.Length, midTmp);
                        arg = ReplaceSubstringWithRepeatedChar(arg, withBlock.Index, withBlock.Length, '#');
                    }
                }
            }
        }


        public static string ReplaceSubstringWithRepeatedChar(string original, int startIndex, int length, char replacementChar)
        {
            if (string.IsNullOrEmpty(original) || startIndex < 0 || startIndex + length > original.Length)
                return original;

            string replacement = new string(replacementChar, length);
            string returnstring= original.Substring(0, startIndex) + replacement + original.Substring(startIndex + length);
            return returnstring;
        }


        // ---------------------------------------------------------------------------------------
        // @description:     The include directive can have a level assignment, this routine finds that assignment and creates a token
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void ParseIncludeLevel(ref string arg, int index)
        {
            string value;
            object str = new[] { "system", "application", "organization", "workspace", "workset", "role", "user" };
            Match mtch;

            mtch = _includeLevel.Match(arg.ToLowerInvariant());

            // No level is specified - return to normal parsing
            if (string.IsNullOrEmpty(mtch.Groups["LEVEL"].Value))
            {
                return;
            }

            {
                var withBlock = mtch.Groups["LEVELNO"];
                // add preproc token
                value = withBlock.Value?.Trim();
                if (CultureInfo.CurrentCulture.CompareInfo.Compare(value, "1", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 || CultureInfo.CurrentCulture.CompareInfo.Compare(value, "2", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 || CultureInfo.CurrentCulture.CompareInfo.Compare(value, "3", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 || CultureInfo.CurrentCulture.CompareInfo.Compare(value, "4", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 || CultureInfo.CurrentCulture.CompareInfo.Compare(value, "5", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 || CultureInfo.CurrentCulture.CompareInfo.Compare(value, "6", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 || CultureInfo.CurrentCulture.CompareInfo.Compare(value, "0", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                {
;
                    AddToken(CFGEnums.TT.ttLevelNo, CFGEnums.TokenGroup.tgOperand, withBlock.Value, withBlock.Index + 1);
                }
                else if (Array.IndexOf((Array)str, value) != -1)
                {
                    AddToken(CFGEnums.TT.ttLevelNo, CFGEnums.TokenGroup.tgOperand, " " + Utilities.GetLevelNumber(value).ToString(), withBlock.Index + 1);
                }
                else
                {
                    Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}");
                    if (_reportEvents)
                    {
                        var argetype = CFGEnums.CFGEventType.cfgError;
                        string argdescription = CEResource.TXT_ErrLevelSettingOnIncludeStatement;
                        var argline = this;
                        CFGVariable argvariable = null;
                        string argvarname = "";
                        ParentFile.ParentConfiguration.AddEvent(ref argetype, ref argdescription, argline, ParentFile, variable: argvariable, varname: argvarname);
                    }
                } // Event

                arg = arg.Substring(0, mtch.Groups["LEVEL"].Index);
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description:     Removes inline comments from Text, stores the comment in InlineComment
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void RemoveComments()
        {
            int loc = Text.IndexOf('#');
            int tagloc = Text.IndexOf(@"[");
            if ((loc >= 0)||(tagloc >= 0))
            {
                if (loc < 0)
                    loc = int.MaxValue;
                if (tagloc < 0)
                    tagloc = int.MaxValue;
                loc = Math.Min(loc, tagloc);

                string beforeComment = Text.Substring(0, loc);
                string trimmed = beforeComment.Replace("\t", " ").Trim();

                InlineComment = string.IsNullOrEmpty(trimmed) ? Text : Text.Substring(loc);
                Text = beforeComment;
            }
        }


        // ---------------------------------------------------------------------------------------
        // @description:     Deals with backslashes, converts them to forward slashes
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void FixSlashes()
        {
            string lowerText = Text.ToLowerInvariant();

            // TODO: commented out - unsure why just yet
            // If -1 = txt.IndexOf("registryread") AndAlso Text Like "*\*" Then
            // If _reportEvents Then ParentFile.ParentWorkspace.AddEvent(CFGEventType.cfgWarning, CEResource.TXT_ErrWrongKindOfSlash, Me, ParentFile) 'Event

            // 'Mimics the behavior of MicroStation where backslashes are removed from the end of a line
            // If Text(Text.Length - 1) = "\" Then
            // Text = Text.Substring(0, Text.Length - 1)
            // If _reportEvents Then ParentFile.ParentWorkspace.AddEvent(CFGEventType.cfgWarning, CEResource.TXT_WarningBackSlashRemoved, Me, ParentFile) 'Event
            // End If
            // End If

            // Change all forward slashes to backslashes - this is for display and processing
            if (!lowerText.Contains("file:") && !lowerText.Contains("pw:"))
            {
                Text = Text.Replace("/", @"\");
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description:     Creates a new token object and appends it to Tokens
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void AddToken(CFGEnums.TT TokenType, CFGEnums.TokenGroup TokenGroup, string value, int pos)
        {
            var token = new CFGToken()
            {
                TokenType = TokenType,
                Value = value
            };

            // TO DO: Some string value may want spaces???
            if (TokenType != CFGEnums.TT.ttString && !string.IsNullOrWhiteSpace(token.Value))
                token.Value = token.Value.Trim();

            token.Position = pos;

            switch (value)
            {
                case "(":
                    token.TokenType = CFGEnums.TT.ttOPENPARENTHESIS;
                    token.TokenGroup = CFGEnums.TokenGroup.tgGroupingChar;
                    break;
                case ")":
                    token.TokenType = CFGEnums.TT.ttCLOSEPARENTHESIS;
                    token.TokenGroup = CFGEnums.TokenGroup.tgGroupingChar;
                    break;
                case "{":
                    token.TokenType = CFGEnums.TT.ttOPENBRACKET;
                    token.TokenGroup = CFGEnums.TokenGroup.tgGroupingChar;
                    break;
                case "}":
                    token.TokenType = CFGEnums.TT.ttCLOSEBRACKET;
                    token.TokenGroup = CFGEnums.TokenGroup.tgGroupingChar;
                    break;
                default:
                    token.TokenGroup = TokenGroup;
                    break;
            }

            string upperValue = value.ToUpperInvariant();

            if (upperValue == "IFDEF")
            {
                AddToken(CFGEnums.TT.ttIF, CFGEnums.TokenGroup.tgPreProc, "if", pos);
                AddToken(CFGEnums.TT.ttDEFINED, CFGEnums.TokenGroup.tgFunction, "defined", pos + 1);
            }
            else if (upperValue == "IFNDEF")
            {
                AddToken(CFGEnums.TT.ttIF, CFGEnums.TokenGroup.tgPreProc, "if", pos);
                AddToken(CFGEnums.TT.ttNOT, CFGEnums.TokenGroup.tgBooleanFunc, "!", pos + 1);
                AddToken(CFGEnums.TT.ttDEFINED, CFGEnums.TokenGroup.tgFunction, "defined", pos + 2);
            }
            else if (upperValue == "IFFEATURE")
            {
                AddToken(CFGEnums.TT.ttIF, CFGEnums.TokenGroup.tgPreProc, "if", pos);
                AddToken(CFGEnums.TT.ttFEATURE, CFGEnums.TokenGroup.tgFunction, "feature", pos + 1);
            }
            else if (TokenType == CFGEnums.TT.ttBACKTRACK)
            {
                Add_BackTrackToken(pos);
            }
            else
            {
                Tokens.Add(token);
            }


            token.OriginalType = token.TokenType;
        }

        // ---------------------------------------------------------------------------------------
        // @description:     Inserts a new token at a specific location in Tokens
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void Insert_Token(CFGEnums.TT TokenType, CFGEnums.TokenGroup TokenGroup, string value, int pos, bool trimValue = true)
        {
            var token = new CFGToken
            {
                TokenType = TokenType,
                Value = value?.Trim(),
                Position = pos,
                TokenGroup = TokenGroup
            };
            if (trimValue)
            {
                token.Value = value?.Trim();
            }
            else
            {
                token.Value = value;
            }
            Tokens.Insert(pos, token);
        }

        private void Add_BackTrackToken(int pos)
        {
            var tokenClose = new CFGToken()
            {
                TokenType = CFGEnums.TT.ttCLOSEPARENTHESIS,
                Value = ")",
                Position = pos,
                TokenGroup = CFGEnums.TokenGroup.tgGroupingChar
            };

            Tokens.Add(tokenClose);

            int FirstArgPosition = this.Tokens[2].Position;
            int FirstArgPositionRelative= this.Tokens[2].PositionRelative;

            var tokenOpen = new CFGToken()
            {
                TokenType = CFGEnums.TT.ttOPENPARENTHESIS,
                Value = "(",
                Position = FirstArgPosition,
                PositionRelative= FirstArgPositionRelative-1,
                TokenGroup = CFGEnums.TokenGroup.tgGroupingChar
            };
            Tokens.Insert(2, tokenOpen);

            var tokenBT = new CFGToken()
            {
                TokenType = CFGEnums.TT.ttBACKTRACK,
                Value = "..\\",
                Position = FirstArgPosition,
                PositionRelative = FirstArgPositionRelative - 2,
                TokenGroup = CFGEnums.TokenGroup.tgFunction
            };
            Tokens.Insert(2, tokenBT);

            Tokens.Sort();
        }
        // ---------------------------------------------------------------------------------------
        // @description:     Combines groups of tokens into (string) or {string} tokens
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void CombineTokens()
        {
            var t = default(int);

            if (Tokens.Count < 3)
                return;

            while (t < Tokens.Count - 2)
            {
                if (Tokens[t].TokenType == CFGEnums.TT.ttOPENPARENTHESIS &&
                    Tokens[t + 1].TokenType == CFGEnums.TT.ttString &&
                    Tokens[t + 2].TokenType == CFGEnums.TT.ttCLOSEPARENTHESIS)
                {
                    Tokens.RemoveAt(t);       // Removes OPENPARENTHESIS
                    Tokens.RemoveAt(t);       // Now the original t+1 (ttString) is at index t, so remove it too

                    Tokens[t].TokenType = CFGEnums.TT.ttStringWithParenthesis;
                    Tokens[t].Value = Tokens[t].Value?.Trim();
                }

                t += 1;
            }

            if (Tokens.Count < 3)
                return;

            t = 0;
            while (t < Tokens.Count - 2)
            {
                if (Tokens[t].TokenType == CFGEnums.TT.ttOPENBRACKET &&
                    Tokens[t + 1].TokenType == CFGEnums.TT.ttString &&
                    Tokens[t + 2].TokenType == CFGEnums.TT.ttCLOSEBRACKET)
                {
                    Tokens.RemoveAt(t);       // Removes OPENBRACKET
                    Tokens.RemoveAt(t);       // Now the original t+1 (ttString) is at index t, so remove it too

                    Tokens[t].TokenType = CFGEnums.TT.ttStringWithBrackets;
                    Tokens[t].Value = Tokens[t].Value?.Trim();
                }

                t += 1;
            }

            // TODO: testing Convert
            if (Tokens.Count < 3)
                return;

            t = 0;
            while (t < Tokens.Count - 1)
            {
                if (t + 3 < Tokens.Count &&
                    Tokens[t].TokenType == CFGEnums.TT.ttDOLLAR &&
                    Tokens[t + 1].TokenType == CFGEnums.TT.ttOPENPARENTHESIS &&
                    Tokens[t + 2].TokenType == CFGEnums.TT.ttStringWithParenthesis &&
                    Tokens[t + 3].TokenType == CFGEnums.TT.ttCLOSEPARENTHESIS)
                {
                    Tokens[t + 2].TokenType = CFGEnums.TT.ttVarValue;
                    Tokens[t + 2].Value = Tokens[t + 2].Value?.Trim();
                }
                else if (t + 1 < Tokens.Count &&
                         Tokens[t].TokenType == CFGEnums.TT.ttDOLLAR &&
                         Tokens[t + 1].TokenType == CFGEnums.TT.ttStringWithParenthesis)
                {
                    Tokens[t + 1].TokenType = CFGEnums.TT.ttVarValue;
                    Tokens[t + 1].Value = Tokens[t + 1].Value?.Trim();

                    Tokens.Insert(t + 1, new CFGToken
                    {
                        TokenType = CFGEnums.TT.ttOPENPARENTHESIS,
                        Position = t + 1,
                        TokenGroup = CFGEnums.TokenGroup.tgGroupingChar,
                        Value = "("
                    });

                    Tokens.Insert(t + 3, new CFGToken
                    {
                        TokenType = CFGEnums.TT.ttCLOSEPARENTHESIS,
                        Position = t + 3,
                        TokenGroup = CFGEnums.TokenGroup.tgGroupingChar,
                        Value = ")"
                    });
                }

                // Optional: Handle ttStringWithParenthesis or ttStringWithBrackets if needed
                // if ((Tokens[t].TokenType == CFGEnums.TT.ttStringWithParenthesis ||
                //      Tokens[t].TokenType == CFGEnums.TT.ttStringWithBrackets) && IsIF)
                // {
                //     Tokens[t].TokenType = CFGEnums.TT.ttVarValue;
                // }

                t++;
            }

            if (Tokens.Count < 3)
                return;

            t = 0;
            while (t < Tokens.Count - 3)
            {
                if (Tokens[t].TokenType == CFGEnums.TT.ttDOLLAR &&
                    Tokens[t + 1].TokenType == CFGEnums.TT.ttOPENBRACKET &&
                    Tokens[t + 2].TokenType == CFGEnums.TT.ttStringWithBrackets &&
                    Tokens[t + 3].TokenType == CFGEnums.TT.ttCLOSEBRACKET)
                {
                    Tokens[t + 2].TokenType = CFGEnums.TT.ttVarValue;
                    Tokens[t + 2].Value = Tokens[t + 2].Value?.Trim();
                }

                t++;
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description:     Sets the operator precedence values for each token based on its TokenType value
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void SetPvals()
        {
            foreach (CFGToken t in Tokens)
                t.SetPrecedence();
        }

        // ---------------------------------------------------------------------------------------
        // @description:     Checks for a $( or a ${ before a function call
        // Inserts a function close token to wrap all tokens within a function
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void CheckFunctionCallDefs()
        {
            // Checks for a $( or a ${ before a function call
            // Inserts a function close token to wrap all tokens within a function

            int t;

            var loopTo = Tokens.Count - 1;
            for (t = 0; t <= loopTo; t++)
            {
                {
                    var withBlock = Tokens[t];
                    if (withBlock.TokenType == CFGEnums.TT.ttFUNCOPEN || withBlock.TokenType == CFGEnums.TT.ttFUNCOPENabs)
                    {
                        t = FindFunctionEnd(t);
                        if (t == -1)
                        {
                            HasErrors = true;
                            return;
                        }
                        // Insert function close token
                        if (withBlock.TokenType == CFGEnums.TT.ttFUNCOPEN)
                        {
                            Insert_Token(CFGEnums.TT.ttFUNCCLOSE, 
                                CFGEnums.TokenGroup.tgFunction, 
                                ")", 
                                t + 1);
                        }
                        if (withBlock.TokenType == CFGEnums.TT.ttFUNCOPENabs)
                        {
                            Insert_Token(CFGEnums.TT.ttFUNCCLOSEabs, 
                                CFGEnums.TokenGroup.tgFunction, 
                                "}", 
                                t + 1);
                        }
                    }
                    else if (withBlock.TokenGroup == CFGEnums.TokenGroup.tgFunction)
                    {
                            // TODO: does every function need to get checked for?
                        if (withBlock.TokenType != CFGEnums.TT.ttFIRSTDIRPIECE && 
                            withBlock.TokenType != CFGEnums.TT.ttLASTDIRPIECE && 
                            withBlock.TokenType != CFGEnums.TT.ttPARENTDIR && 
                            withBlock.TokenType != CFGEnums.TT.ttPARENTDEVDIR &&
                            withBlock.TokenType != CFGEnums.TT.ttBACKTRACK &&
                            withBlock.TokenType != CFGEnums.TT.ttDEFINED && 
                            withBlock.TokenType != CFGEnums.TT.ttEXISTS && 
                            withBlock.TokenType != CFGEnums.TT.ttFEATURE && 
                            withBlock.TokenType != CFGEnums.TT.ttFUNCCLOSE && 
                            withBlock.TokenType != CFGEnums.TT.ttFUNCCLOSEabs)
                        {
                            Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}");
                            // If _reportEvents Then ParentFile.ParentWorkspace.AddEvent(CFGEventType.cfgError, CEResource.TXT_ErrFunctionCallNotProcessed, Me, ParentFile) 'Event
                            // HasErrors = True
                            // Exit Sub
                        }
                    }

                }
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description:    Checks for a specific error where there's a tab character between a % and the preproc directive
        // MicroStation's parser can't handle this situation.
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void CheckPreprocTabs()
        {
            int t;

            if (Text?[0].ToString() != "%")
                return;

            var loopTo = Text.Length - 1;
            for (t = 1; t <= loopTo; t++)
            {
                if (Text[t].ToString() != " " && Text[t] != '\t')
                    return;

                if (Text[t] == '\t')
                {
                    Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}");
                    if (_reportEvents)
                    {
                        var argetype = CFGEnums.CFGEventType.cfgCritical;
                        string argdescription = CEResource.TXT_ErrTabCharBetween;
                        var argline = this;
                        CFGVariable argvariable = null;
                        string argvarname = "";
                        ParentFile.ParentConfiguration.AddEvent(ref argetype, ref argdescription, argline, ParentFile, variable: argvariable, varname: argvarname);
                    } // Event

                    HasErrors = true;
                    return;
                }
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description:     Checks for a special case where a space inside an 'exists' statement can cause issues
        // example   %if exists("C:\my folder\test.txt") throws a critical error
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void CheckExistsSpaceIssue()
        {
            if (!IsPreproc)
                return;

            int t = 0;

            while (t < Tokens.Count - 1)
            {
                if (Tokens[t].TokenType == CFGEnums.TT.ttEXISTS)
                {
                    t++;
                    while (t < Tokens.Count)
                    {
                        var tokenType = Tokens[t].TokenType;

                        if (tokenType == CFGEnums.TT.ttFUNCCLOSE || tokenType == CFGEnums.TT.ttFUNCCLOSEabs)
                            break;

                        if (tokenType == CFGEnums.TT.ttString ||
                            tokenType == CFGEnums.TT.ttStringWithQuotes ||
                            tokenType == CFGEnums.TT.ttStringWithBrackets ||
                            tokenType == CFGEnums.TT.ttStringWithParenthesis)
                        {
                            if (Tokens[t].Value?.Trim().Contains(" ") == true)
                            {
                                Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}");

                                if (_reportEvents)
                                {
                                    var eventType = CFGEnums.CFGEventType.cfgCritical;
                                    string description = CEResource.TXT_ErrNoSpaceInPath;
                                    CFGVariable variable = null;
                                    string varName = "";

                                    ParentFile.ParentConfiguration.AddEvent(
                                        ref eventType,
                                        ref description,
                                        this,
                                        ParentFile,
                                        variable: variable,
                                        varname: varName
                                    );
                                }

                                HasErrors = true;
                                return;
                            }
                        }

                        t++;
                    }
                }

                t++;
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description:    Convert parenthesis and brackets to text if they are surrounded by strings
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void ConvertParenthesis()
        {
            // TODO: testing
            // Dim t As Integer

            // While t < (Tokens.Count - 2)
            // If Tokens(t).TokenType = TT.ttString Then
            // If Tokens(t + 1).TokenType = TT.ttOPENPARENTHESIS OrElse Tokens(t + 1).TokenType = TT.ttCLOSEPARENTHESIS _
            // OrElse Tokens(t + 1).TokenType = TT.ttOPENBRACKET OrElse Tokens(t + 1).TokenType = TT.ttCLOSEBRACKET Then
            // If Tokens(t + 2).TokenType = TT.ttString Then
            // Tokens(t + 1).TokenType = TT.ttString
            // Tokens(t + 1).TokenGroup = TokenGroup.tgOperand
            // End If
            // End If
            // End If
            // t += 1
            // End While
        }

        // ---------------------------------------------------------------------------------------
        // @description:      Convert comma tokens back to strings if they are not used inside of build() or concat()
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void Convert_SPCLCHARS()
        {
            var t = default(int);
            while (t < Tokens.Count - 1)
            {
                if (Tokens[t].TokenType == CFGEnums.TT.ttCOMMA)
                {
                    Tokens[t].TokenType = CFGEnums.TT.ttString;
                    Tokens[t].TokenGroup = CFGEnums.TokenGroup.tgOperand;
                }
                t += 1;
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description:      Stores the token's original type
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void SetTypes()
        {
            foreach (CFGToken token in Tokens)
                token.OriginalType = token.TokenType;
        }

        // ---------------------------------------------------------------------------------------
        // @description:     Removes math operators from within functions

        // ---------------+---------------+---------------+---------------+---------------+-------
        private void ConvertFunctionChars()
        {
            int t = 0;
            int y;
            int fe;
            int insidef = 0;
            bool insideq = false;

            while (t < Tokens.Count)
            {

                if (Tokens[t].TokenType == CFGEnums.TT.ttFUNCOPEN || Tokens[t].TokenType == CFGEnums.TT.ttFUNCOPENabs)
                    insidef += 1;
                if (Tokens[t].TokenType == CFGEnums.TT.ttFUNCCLOSE || Tokens[t].TokenType == CFGEnums.TT.ttFUNCCLOSEabs)
                    insidef -= 1;
                if (Tokens[t].TokenType == CFGEnums.TT.ttQUOTE)
                    insideq = !insideq;

                if (insidef != 0 && !insideq)
                {
                    if (Tokens[t].TokenGroup == CFGEnums.TokenGroup.tgBooleanFunc)
                    {
                        if (Tokens[t].TokenType != CFGEnums.TT.ttCOMMA)
                        {
                            Tokens[t].TokenType = CFGEnums.TT.ttString;
                            Tokens[t].TokenGroup = CFGEnums.TokenGroup.tgOperand;
                        }
                    }
                }
                t += 1;
            }

            t = 0;
            while (t < Tokens.Count)
            {
                if (Tokens[t].TokenType == CFGEnums.TT.ttDEFINED || Tokens[t].TokenType == CFGEnums.TT.ttEXISTS)
                {

                    fe = FindFunctionEnd(t);
                    if (fe > 0)
                    {
                        var loopTo = fe;
                        for (y = t; y <= loopTo; y++)
                        {
                            if (Tokens[y].TokenGroup == CFGEnums.TokenGroup.tgBooleanFunc)
                            {
                                if (Tokens[y].TokenType != CFGEnums.TT.ttCOMMA)
                                {
                                    Tokens[y].TokenType = CFGEnums.TT.ttString;
                                    Tokens[y].TokenGroup = CFGEnums.TokenGroup.tgOperand;
                                }
                            }
                        }
                        t = fe - 1;
                    }

                }
                t += 1;
            }

            t = 0;
            // While t < Tokens.Count
            // With Tokens(t)
            // If .TokenType = TT.ttGREATERTHAN Then .TokenType = TT.ttGT
            // If .TokenType = TT.ttLESSTHAN Then .TokenType = TT.ttLT
            // If .TokenType = TT.ttADD Then .TokenType = TT.ttADD
            // End With
            // t += 1
            // End While
        }

        // ---------------------------------------------------------------------------------------
        // @description:     Inserts an append token
        // There's an implied append between specific tokens
        // For example: $(variable)\folder\  = $(variable) + \folder\
        //         // refactored by Copilot
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void InsertImpliedTokens()
        {
            if (Tokens.Count < 2)
                return;

            for (int t = 0; t < Tokens.Count - 1; t++)
            {
                var current = Tokens[t];
                var next = Tokens[t + 1];

                // Operand followed by Operand/Function/OpenParenthesis (excluding LevelNo)
                if (current.TokenGroup == CFGEnums.TokenGroup.tgOperand &&
                    (next.TokenGroup == CFGEnums.TokenGroup.tgOperand ||
                     next.TokenGroup == CFGEnums.TokenGroup.tgFunction ||
                     next.TokenType == CFGEnums.TT.ttOPENPARENTHESIS) &&
                    next.TokenType != CFGEnums.TT.ttLevelNo)
                {
                    Tokens.Insert(t + 1, CreateImpliedAppendToken());
                    t++; // Skip over inserted token
                    continue;
                }
            }
        }

        private CFGToken CreateImpliedAppendToken()
        {
            return new CFGToken
            {
                TokenType = CFGEnums.TT.ttImpliedAppend,
                TokenGroup = CFGEnums.TokenGroup.tgImpliedAppend,
                Value = "@"
            };
        }

        private CFGToken CloneToken(CFGToken token)
        {
            return new CFGToken
            {
                TokenType = token.TokenType,
                TokenGroup = token.TokenGroup,
                Value = token.Value
            };
        }

        // ---------------------------------------------------------------------------------------
        // @description:     Concat and Build have a variable number of arguments
        // Inserts a token that to the function how many arguments it will get
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void InsertArgumentCountTokens()
        {
            var t = default(int);
            int fe;
            bool brackets;

            if (Tokens.Count < 3)
                return;
            while (t < Tokens.Count - 1)
            {
                {
                    var withBlock = Tokens[t];
                    if (withBlock.TokenType == CFGEnums.TT.ttBUILD | withBlock.TokenType == CFGEnums.TT.ttCONCAT)
                    {
                        fe = FindFunctionEnd(t);
                        if (Tokens[t + 1].TokenType == CFGEnums.TT.ttOPENBRACKET)
                        {
                            brackets = true;
                        }
                        else
                        {
                            brackets = false;
                        }
                        if (fe > t && fe <= Tokens.Count - 1)
                        {
                            InsertArgumentCountTokens(t, fe, brackets, withBlock.TokenType == CFGEnums.TT.ttBUILD);
                        }
                    }
                }
                t += 1;
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description:  Inserts an argument count token
        // Wraps each argument in parentheses to force operator precedence
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void InsertArgumentCountTokens(int p1, int p2, bool brackets = false, bool isBuild = false)
        {
            int t;
            int pcount = 1;
            CFGEnums.TT op;
            string opc;
            CFGEnums.TT cl;
            string clc;

            if (brackets)
            {
                op = CFGEnums.TT.ttOPENBRACKET;
                opc = "{";
                cl = CFGEnums.TT.ttCLOSEBRACKET;
                clc = "}";
            }
            else
            {
                op = CFGEnums.TT.ttOPENPARENTHESIS;
                opc = "(";
                cl = CFGEnums.TT.ttCLOSEPARENTHESIS;
                clc = ")";
            }

            var argTokenGroup = CFGEnums.TokenGroup.tgGroupingChar;
            Insert_Token(op, argTokenGroup, opc, p1 + 2);
            p2 += 1;

            t = p1;
            while (t <= p2)
            {
                {
                    var withBlock = Tokens[t];
                    if (withBlock.TokenType == CFGEnums.TT.ttCOMMA)
                    {
                        if (t < Tokens.Count - 1)
                        {
                            if (Tokens[t + 1].TokenType != CFGEnums.TT.ttCOMMA)
                                pcount += 1;
                        }
                        Tokens.RemoveAt(t);
                        p2 -= 1;
                        Insert_Token(cl, 
                            CFGEnums.TokenGroup.tgGroupingChar,
                            clc, 
                            t);
                        p2 += 1;
                        if (isBuild)
                        {
                            CFGEnums.TokenGroup argTokenGroup2 = default;
                            string argvalue = " ";
                            Insert_Token(CFGEnums.TT.ttString, argTokenGroup2, argvalue, t + 1, false);
                            p2 += 1;
                        }
                        Insert_Token(op, 
                            CFGEnums.TokenGroup.tgGroupingChar, 
                            opc, 
                            t + 1);
                        p2 += 1;
                    }
                }
                t += 1;
            }

            Insert_Token(cl, 
                CFGEnums.TokenGroup.tgGroupingChar, 
                clc, 
                p2 + 1);
        }

        // ---------------------------------------------------------------------------------------
        // @description:  Returns the index of the closing parenthesis or bracket of a function
        // start = the index of the function token within Tokens
        // Returns -1 if not found
        // ---------------+---------------+---------------+---------------+---------------+-------
        private int FindFunctionEnd(int start)
        {
            int pcount;
            int t;
            t = start + 1;

            if (t < Tokens.Count)
            {
                if (Tokens[t].TokenType == CFGEnums.TT.ttStringWithParenthesis || Tokens[t].TokenType == CFGEnums.TT.ttStringWithBrackets)
                {
                    return t;
                }
                if (Tokens[t].TokenType != CFGEnums.TT.ttOPENPARENTHESIS && Tokens[t].TokenType != CFGEnums.TT.ttOPENBRACKET)
                {
                    return -1;
                }
            }

            pcount = 1;
            t += 1;

            while (t < Tokens.Count)
            {
                if (Tokens[t].TokenType == CFGEnums.TT.ttOPENPARENTHESIS || Tokens[t].TokenType == CFGEnums.TT.ttOPENBRACKET)
                    pcount += 1;
                if (Tokens[t].TokenType == CFGEnums.TT.ttCLOSEPARENTHESIS || Tokens[t].TokenType == CFGEnums.TT.ttCLOSEBRACKET)
                    pcount -= 1;
                if (pcount == 0)
                    return t;
                t += 1;
            }

            return -1;   // Imbalanced parentheses
        }

        // ---------------------------------------------------------------------------------------
        // @description: Converts (){}" ! = != to strings if no function was found in the token list
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void ConvertMisreadItems()
        {
            // Depending on the context these characters could be operators or simply strings
            // For example:
            // %if !exists("c:\test.txt")      ! is a boolean operator
            // variable = Hello World!         ! is part of a string
            // 
            // This function converts misread characters back to strings

            if (IsPreproc)
                return;

            // Look for a function token
            foreach (CFGToken token in Tokens)
            {
                if (token.TokenType == CFGEnums.TT.ttFUNCOPEN || token.TokenType == CFGEnums.TT.ttFUNCOPENabs)
                {
                    return;
                }
            }

            foreach (CFGToken token in Tokens)
            {
                if (token.TokenGroup == CFGEnums.TokenGroup.tgGroupingChar || token.TokenGroup == CFGEnums.TokenGroup.tgBooleanFunc)
                {
                    token.TokenGroup = CFGEnums.TokenGroup.tgOperand;
                    token.TokenType = CFGEnums.TT.ttString;
                }
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Removes the function wrapper tokens
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void RemoveFunctionWrappers()
        {
            int t = 0;
            while (t < Tokens.Count)
            {
                if (Tokens[t].TokenType == CFGEnums.TT.ttFUNCOPEN || Tokens[t].TokenType == CFGEnums.TT.ttFUNCOPENabs || Tokens[t].TokenType == CFGEnums.TT.ttFUNCCLOSE || Tokens[t].TokenType == CFGEnums.TT.ttFUNCCLOSEabs)
                {
                    Tokens.RemoveAt(t);
                }
                else
                {
                    t += 1;
                }
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Creates a lookup table for tokens
        // ---------------+---------------+---------------+---------------+---------------+-------
        private static Dictionary<CFGEnums.TT, string> InitTokenValues()
        {
            return new Dictionary<CFGEnums.TT, string>
    {
        { CFGEnums.TT.ttIF, "IF" },
        { CFGEnums.TT.ttELIF, "ELIF" },
        { CFGEnums.TT.ttIFNDEF, "IFNDEF" },
        { CFGEnums.TT.ttIFDEF, "IFDEF" },
        { CFGEnums.TT.ttIFFEATURE, "IFFEATURE" },
        { CFGEnums.TT.ttENDIF, "ENDIF" },
        { CFGEnums.TT.ttELSE, "ELSE" },
        { CFGEnums.TT.ttLOCK, "LOCK" },
        { CFGEnums.TT.ttUNDEF, "UNDEF" },
        { CFGEnums.TT.ttERROR, "ERROR" },
        { CFGEnums.TT.ttECHO, "ECHO" },
        { CFGEnums.TT.ttREGISTRYREAD, "REGISTRYREAD" },
        { CFGEnums.TT.ttDEFINED, "DEFINED" },
        { CFGEnums.TT.ttEXISTS, "EXISTS" },
        { CFGEnums.TT.ttINCLUDE, "INCLUDE" },
        { CFGEnums.TT.ttFEATURE, "FEATURE" },
        { CFGEnums.TT.ttCONCAT, "CONCAT" },
        { CFGEnums.TT.ttDEV, "DEV" },
        { CFGEnums.TT.ttDIR, "DIR" },
        { CFGEnums.TT.ttDEVDIR, "DEVDIR" },
        { CFGEnums.TT.ttPARENTDIR, "PARENTDIR" },
        { CFGEnums.TT.ttPARENTDEVDIR, "PARENTDEVDIR" },
        { CFGEnums.TT.ttBACKTRACK, @"..\" },
        { CFGEnums.TT.ttFIRSTDIRPIECE, "FIRSTDIRPIECE" },
        { CFGEnums.TT.ttLASTDIRPIECE, "LASTDIRPIECE" },
        { CFGEnums.TT.ttBASENAME, "BASENAME" },
        { CFGEnums.TT.ttFILENAME, "FILENAME" },
        { CFGEnums.TT.ttEXT, "EXT" },
        { CFGEnums.TT.ttNOEXT, "NOEXT" },
        { CFGEnums.TT.ttFIRST, "FIRST" },
        { CFGEnums.TT.ttBUILD, "BUILD" },
        { CFGEnums.TT.ttLEVEL, "LEVEL" },
        { CFGEnums.TT.ttCOLON, ":" },
        { CFGEnums.TT.ttEQUATE, "=" },
        { CFGEnums.TT.ttGREATERTHAN, ">" },
        { CFGEnums.TT.ttLESSTHAN, "<" },
        { CFGEnums.TT.ttADD, "+" },
        { CFGEnums.TT.ttEQUALS, "==" },
        { CFGEnums.TT.ttNOT, "!" },
        { CFGEnums.TT.ttOPENPARENTHESIS, "(" },
        { CFGEnums.TT.ttCLOSEPARENTHESIS, ")" },
        { CFGEnums.TT.ttOPENBRACKET, "{" },
        { CFGEnums.TT.ttCLOSEBRACKET, "}" },
        { CFGEnums.TT.ttQUOTE, "\"" },
        { CFGEnums.TT.ttAND, "&&" },
        { CFGEnums.TT.ttOR, "||" },
        { CFGEnums.TT.ttFUNCOPEN, "$(" },
        { CFGEnums.TT.ttFUNCOPENabs, "${" },
        { CFGEnums.TT.ttNOTEQUALS, "!=" },
        { CFGEnums.TT.ttCOMMA, "," },
        { CFGEnums.TT.ttMODULUS, "%" },
        { CFGEnums.TT.ttMULTIPLY, "*" },
        { CFGEnums.TT.ttGREATEROREQUAL, ">=" },
        { CFGEnums.TT.ttLESSTOREQUAL, "<=" },
        { CFGEnums.TT.ttBITAND, "&" },
        { CFGEnums.TT.ttBITOR, "|" },
        { CFGEnums.TT.ttBITXOR, "^" },
        { CFGEnums.TT.ttDIVIDE, "/" },
        { CFGEnums.TT.ttSUBTRACT, "-" },
        { CFGEnums.TT.ttSHIFTLEFT, "<<" },
        { CFGEnums.TT.ttSHIFTRIGHT, ">>" },
        { CFGEnums.TT.ttDOLLAR, "$" }
            };
        }

        // ---------------------------------------------------------------------------------------
        // @description: Uses the lookup table to match a string with its token
        // ---------------+---------------+---------------+---------------+---------------+-------
        private CFGEnums.TT StringToTokenType(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return 0;

            string upperName = name.ToUpperInvariant();

            foreach (var kvp in _tokenValues)
            {
                if (kvp.Value.Equals(upperName, StringComparison.OrdinalIgnoreCase))
                    return kvp.Key;
            }

            return 0; // Or consider throwing an exception or returning a default token
        }

    }
}
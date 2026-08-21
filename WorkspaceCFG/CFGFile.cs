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
using Newtonsoft.Json;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{

    [Serializable()]
    [JsonObject(MemberSerialization.Fields)]
    public class CFGFile
    {

        public enum IfStackStates
        {
            StartState,
            TrueState,
            FalseState
        }

        // ---------------------------------------------------------------------------------------
        // Private Members
        // ---------------+---------------+---------------+---------------+---------------+-------
        [JsonIgnore] // Transient parse-time state; System.Collections.Stack is not compatible with PreserveReferencesHandling
        private Stack _ifStack;                           // Keeps track of if else structure (Has State of IfStackStates)
        //private Collection _parentList;                   // Keeps track of the current variable's parent variables
        [JsonIgnore] // Transient parse-time state, not part of the saved configuration
        private Dictionary<string, string> _parentList = new Dictionary<string, string>();
        [JsonIgnore] // Transient parse-time state, not part of the saved configuration
        private CFGLine _currentLine;                     // _currentLine being processed
        [JsonIgnore] // Transient parse-time state, not part of the saved configuration
        private bool _variableNeedsExpansion;          // Optimizes processing if the variable doesn't have to be expanded
        [JsonIgnore] // Transient parse-time state, not part of the saved configuration
        private bool _currentVarRefUsed;               // Current line has a variable reference

        // ---------------------------------------------------------------------------------------
        // Public Members
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static int NextUID = 0;
        public int UID;                               // Unique ID
        public int Depth;                             // Include depth
        public string FilePath;                           // Full name and path
        public string Name;                               // Name of the file
        public List<CFGLine> Lines;                    // Holds the data for each line in the file
        public CFGConfiguration ParentConfiguration;              // Reference to the parent workspace
        public int CurrentLevel;                      // Current variable level
        public CFGFile ParentFile;                        // Reference to the file that 'included' this file
        public ArrayList ChildFiles;   // Array of Child Files
        public DateTime DateLastModified;                     // Last modified date
        public int StartLevel;                        // Starting Level
        public bool debugLineOutput = false;
        public bool debugFileOutput = false;

        // ---------------------------------------------------------------------------------------
        // @description: Initializes members
        // ---------------+---------------+---------------+---------------+---------------+-------
        public CFGFile(ref CFGConfiguration wrkspc)
        {
            UID = NextUID;
            NextUID += 1;                  // Grab a unique ID and increment the counter
            ParentConfiguration = wrkspc;
            _ifStack = new Stack();
            ChildFiles = new ArrayList();
            //_parentList = new Collection();
            _parentList = new Dictionary<string, string>();
            Lines = new List<CFGLine>();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Member cleanup
        // ---------------+---------------+---------------+---------------+---------------+-------
        ~CFGFile()
        {
            ParentConfiguration = null;
            ChildFiles.Clear();
            ChildFiles = null;
            ParentFile = null;
            _ifStack = null;
            _parentList.Clear();
            _parentList = null;
            _currentLine = null;
            Lines.Clear();
            Lines = null;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Processes the configuration file defined in FilePath
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void ProcessFile()
        {
            // Remember the level at the time this file was included
            StartLevel = CurrentLevel;

            // Remove double backslashes that crop up from bad variable definitions
            string FilePathDrive = FilePath.Substring(0, 2);
            string FilePathDev= FilePath.Substring(2);
            FilePathDev = FilePathDev.Replace(@"\\\\", @"\\");

            FilePath = FilePathDrive + FilePathDev;

            DateLastModified = File.GetLastWriteTime(FilePath);
            Name = UtilitiesPath.GetFileName(FilePath);

            CheckFileEncoding();
            TokenizeFile();
            Verify_IF_Structure();

            // Do not process lines in cache.ucf.
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(Path.GetFileName(FilePath), "cache.ucf", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0)
            {
                var argfile = this;
                var parser = new MacroParser(null, ref argfile) { DEBUG_LOGGING = false };
                parser.ParseFile();
            }

            // Add event to event list
            if (ParentConfiguration.AbortProcessing)
                return;
            var argetype = CFGEnums.CFGEventType.cfgFinishInclude;
            string argdescription = CEResource.TXT_FinishedProcessingFile;
            var argfile1 = this;
            CFGVariable argvariable = null;
            string argvarname = "";
            ParentConfiguration.AddEvent(ref argetype, ref argdescription, _currentLine, argfile1, variable: argvariable, varname: argvarname);

            ClearTokens();
        }

        // ---------------------------------------------------------------------------------------
        // @description: Verifies the current file uses ASCII encoding
        // ---------------+---------------+---------------+---------------+---------------+-------
        private void CheckFileEncoding()
        {
            var encoding = UtilitiesPath.CheckEncoding(ref FilePath);
            string encodingName = encoding.ToString();

            bool isAscii = encodingName.IndexOf("ASCII", StringComparison.OrdinalIgnoreCase) >= 0;
            bool isUnicode = encodingName.IndexOf("Unicode", StringComparison.OrdinalIgnoreCase) >= 0;

            if (!isAscii && !isUnicode)
            {
                var eventType = CFGEnums.CFGEventType.cfgCritical;
                string description = string.Format(CEResource.TXT_ErrWrongTextFileEncodingType, encodingName);

                CFGLine line = null;
                CFGVariable variable = null;
                string variableName = "";

                ParentConfiguration.AddEvent(
                    ref eventType,
                    ref description,
                    line,
                    this,
                    variable: variable,
                    varname: variableName
                );
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Converts the file into a collection tokenized CFGLine objects
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void TokenizeFile()
        {
            try
            {
                using var streamReader = new StreamReader(FilePath);

                if (debugFileOutput)
                {
                    Debug.WriteLine($"File Path: {FilePath}");
                    //debugLineOutput = FilePath.IndexOf("msdir.cfg", StringComparison.OrdinalIgnoreCase) != -1;
                }

                int lineNumber = 0;

                while (!streamReader.EndOfStream)
                {
                    if (ParentConfiguration.AbortProcessing)
                        return;

                    string line = streamReader.ReadLine()?.Trim() ?? string.Empty;
                    lineNumber++;

                    bool isComment = !string.IsNullOrEmpty(line) &&
                                     (line.StartsWith("#") || line.StartsWith("["));

                    var cfgLine = new CFGLine
                    {
                        Text = line,
                        LineNumber = lineNumber,
                        ParentFile = this,
                        IsComment = isComment
                    };

                    //if (!isComment)
                    //{
                        if (debugLineOutput && !string.IsNullOrEmpty(cfgLine.Text))
                        {
                            Debug.WriteLine($"line CFGLine: {cfgLine.Text}");
                        }

                        cfgLine.TokenizeLine();
                    //}

                    Lines.Add(cfgLine);
                    ParseTokenPhrase(cfgLine.Tokens, 0, cfgLine.Tokens.Count - 1);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                // TODO: Handle the error appropriately
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Converts variable reference tokens into string tokens
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void ExpandVariableReferences(ref CFGLine Line)
        {
            CFGVariable variable;

            // No need to expand variable reference if the line doesn't have to be processed
            if (ProcessNextLine() == false)
            {
                if (Line.IsIfStructure == false)
                    return;
                if (Line.Tokens[0].TokenType == CFGEnums.TT.ttIF)
                    return;
                if (Line.Tokens[0].TokenType == CFGEnums.TT.ttELIF)
                {
                    if (Process_Next_ELIF() == false)
                        return;
                }
            }

            _variableNeedsExpansion = false;
            _currentVarRefUsed = false;

            foreach (CFGToken token in Line.Tokens)
            {
                if (token.TokenType == CFGEnums.TT.ttVarValue & Line.IsPreproc || token.TokenType == CFGEnums.TT.ttVarValueCurrent)
                {
                    _currentVarRefUsed = true;
                    variable = ParentConfiguration.GetVariable(token.Value);

                    if (!_parentList.ContainsKey(token.Value))
                        _parentList.Add(token.Value, token.Value);
                    if (variable is not null)
                    {
                        // .Value = variable.Expand(CurrentLevel)
                        token.Value = variable.Expand();
                    }
                    else if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(token.Value)))
                    {
                        // TODO: FIXME any use of IS64Bit() should be checked or removed (see SetProgramFiles for an example)
                        if (string.Equals(token.Value, "commonprogramfiles", StringComparison.OrdinalIgnoreCase) && Utilities.Is64Bit())
                        {
                            string envValue = Environment.GetEnvironmentVariable(token.Value);
                            if (!string.IsNullOrEmpty(envValue))
                            {
                                token.Value = envValue.Replace(@"Program Files\", @"Program Files (x86)\");
                            }
                        }
                        else
                        {
                            token.Value = Environment.GetEnvironmentVariable(token.Value);
                        }
                    }
                    else
                    {
                        // Raise error about the use of an undefined variable
                        var argetype = CFGEnums.CFGEventType.cfgWarning;
                        string argdescription = CEResource.TXT_MsgUndefinedVariablesShouldNotBeUsedInThisContext;
                        var argfile = this;
                        CFGVariable argvariable = null;
                        string argvarname = "";
                        ParentConfiguration.AddEvent(ref argetype, ref argdescription, Line, argfile, variable: argvariable, varname: argvarname);
                        token.Value = string.Empty;
                    }
                    token.TokenType = CFGEnums.TT.ttString;
                }

                if (Line.IsVarDef)
                {
                    if (token.TokenType == CFGEnums.TT.ttVarValue)
                    {
                        token.Value = "$(" + token.Value + ")";
                        token.TokenType = CFGEnums.TT.ttString;
                        _variableNeedsExpansion = true;
                    }
                }
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Converts function tokens into string tokens
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void ConvertFunctionsToStrings(ref CFGLine Line)
        {
            // If the functions have to be stored in the variable then change the function tokens into simple string tokens
            int t = 0;

            while (t < Line.Tokens.Count)
            {
                if (Line.Tokens[t].TokenType == CFGEnums.TT.ttFUNCOPEN)
                {
                    ConvertFunctionsToStrings(ref Line, t);
                    _variableNeedsExpansion = true;
                }
                else
                {
                    t += 1;
                }
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Converts function tokens into string tokens
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void ConvertFunctionsToStrings(ref CFGLine Line, int pos)
        {
            // If this line is a variable definition line, then functions and variables are stored as strings for later expansion

            int t;
            string newval = string.Empty;

            t = pos;
            {
                ref var withBlock = ref Line;
                while (t < withBlock.Tokens.Count)
                {
                    if (withBlock.Tokens[t].TokenType == CFGEnums.TT.ttFUNCCLOSE)
                    {
                        {
                            var withBlock1 = withBlock.Tokens[pos];
                            withBlock1.TokenType = CFGEnums.TT.ttString;
                            withBlock1.TokenGroup = CFGEnums.TokenGroup.tgOperand;
                            withBlock1.Value = "$" + newval;
                        }
                        return;
                    }
                    else
                    {
                        if (withBlock.Tokens[t].TokenType != CFGEnums.TT.ttFUNCOPEN)
                        {
                            var tmp = withBlock.Tokens;
                            var argtoken = tmp[t];
                            newval += String_From_Token(ref argtoken);
                            tmp[t] = argtoken;
                        }
                        withBlock.Tokens.RemoveAt(t);
                    }
                }
            }

            var argetype = CFGEnums.CFGEventType.cfgError;
            string argdescription = CEResource.TXT_MsgImbalancedParenthesisConvertFunctionToString;
            var argfile = this;
            CFGVariable argvariable = null;
            string argvarname = "";
            ParentConfiguration.AddEvent(ref argetype, ref argdescription, Line, argfile, argvariable, varname: argvarname);
            Line.HasErrors = true;
        }

        private string String_From_Token(ref CFGToken token)
        {
            if (token.TokenType == CFGEnums.TT.ttStringWithParenthesis)
                return "(" + token.Value + ")";
            if (token.TokenType == CFGEnums.TT.ttStringWithQuotes)
                return '"' + token.Value + '"';
            if (token.TokenType == CFGEnums.TT.ttStringWithBrackets)
                return "{" + token.Value + "}";
            return token.Value;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Determines if the next line should be processed or ignored based on the IF ELSE ENDIF Structure
        // ---------------+---------------+---------------+---------------+---------------+-------
        private bool ProcessNextLine()
        {
            // If the IF stack is empty or the top state is TrueState, continue processing

            if (_ifStack.Count == 0 || (IfStackStates)_ifStack.Peek() == IfStackStates.TrueState)
            {
                return true;
            }

            // Start state or false state,
            return false;
        }

        private bool Process_Next_ELIF()
        {
            // Peek at the current state on the IF stack
            var currentState = _ifStack.Peek();

            switch (currentState)
            {
                case IfStackStates.TrueState:
                    // Previous IF/ELSE IF was true; skip this ELIF
                    _ifStack.Pop();
                    _ifStack.Push(IfStackStates.FalseState);
                    return false;

                case IfStackStates.StartState:
                    // First ELIF after IF block; allow processing
                    return true;

                case IfStackStates.FalseState:
                default:
                    // Previous condition was false; skip this ELIF
                    return false;
            }
        }

        public bool Verify_IF_Structure()
        {
            // Checks the IF ELSE structure of the file - easier to do this before processing

            var IFStack = new Stack();

            foreach (CFGLine line in Lines)
            {
                // Dim x As Integer = line.Tokens(0).TokenType
                // Dim locCFGLine As CFGLine = line.Tokens(0)
                // Dim locEnumTT As CFGEnums.TT = locCFGLine.Tokens
                if (line.Tokens.Count > 0)
                {
                    if (line.Tokens[0].TokenType == CFGEnums.TT.ttIF)
                    {
                        IFStack.Push(0);
                    }
                    else if (line.Tokens[0].TokenType == CFGEnums.TT.ttELSE)
                    {
                        if (IFStack.Count == 0)
                        {
                            var argetype = CFGEnums.CFGEventType.cfgCritical;
                            string argdescription = CEResource.TXT_MsgInvalidIfElseStructureElseWithoutIf;
                            var argfile = this;
                            CFGVariable argvariable = null;
                            string argvarname = "";
                            ParentConfiguration.AddEvent(ref argetype, ref argdescription, line, argfile, variable: argvariable, varname: argvarname);
                            return false;
                        }
                        else if (IFStack.Peek() is int value && value == 1)
                        {
                            var argetype1 = CFGEnums.CFGEventType.cfgCritical;
                            string argdescription1 = CEResource.TXT_MsgInvalidIfElseStructureTooManyElse;
                            var argfile1 = this;
                            CFGVariable argvariable1 = null;
                            string argvarname1 = "";
                            ParentConfiguration.AddEvent(ref argetype1, ref argdescription1, line, argfile1, variable: argvariable1, varname: argvarname1);
                            return false;
                        }
                        else
                        {
                            IFStack.Pop();
                            IFStack.Push(1);
                        }
                    }
                    else if (line.Tokens[0].TokenType == CFGEnums.TT.ttELIF)
                    {
                        if (IFStack.Count == 0)
                        {
                            var argetype2 = CFGEnums.CFGEventType.cfgCritical;
                            string argdescription2 = CEResource.TXT_MsgInvalidIfElseStructureElifWithoutAnIf;
                            var argfile2 = this;
                            CFGVariable argvariable2 = null;
                            string argvarname2 = "";
                            ParentConfiguration.AddEvent(ref argetype2, ref argdescription2, line, argfile2, variable: argvariable2, varname: argvarname2);
                            return false;
                        }
                        else if (IFStack.Peek() is int value && value == 1)
                        {
                            var argetype3 = CFGEnums.CFGEventType.cfgCritical;
                            string argdescription3 = CEResource.TXT_MsgInvalidIfElseStructureElifWithinElse;
                            var argfile3 = this;
                            CFGVariable argvariable3 = null;
                            string argvarname3 = "";
                            ParentConfiguration.AddEvent(ref argetype3, ref argdescription3, line, argfile3, variable: argvariable3, varname: argvarname3);
                            return false;
                        }
                        else
                        {
                            IFStack.Pop();
                            IFStack.Push(2);
                        }
                    }
                    else if (line.Tokens[0].TokenType == CFGEnums.TT.ttENDIF)
                    {
                        if (IFStack.Count == 0)
                        {
                            var argetype4 = CFGEnums.CFGEventType.cfgCritical;
                            string argdescription4 = CEResource.TXT_MsgInvalidIfElseStructureEndifWithoutIf;
                            var argfile4 = this;
                            CFGVariable argvariable4 = null;
                            string argvarname4 = "";
                            ParentConfiguration.AddEvent(ref argetype4, ref argdescription4, line, argfile4, variable: argvariable4, varname: argvarname4);
                            return false;
                        }
                        else
                        {
                            IFStack.Pop();
                        }
                    }
                }
            }

            if (IFStack.Count > 0)
            {
                var argetype5 = CFGEnums.CFGEventType.cfgCritical;
                string argdescription5 = CEResource.TXT_MsgInvalidIfElseStructureMissingEndif;
                CFGLine argline = null;
                var argfile5 = this;
                CFGVariable argvariable5 = null;
                string argvarname5 = "";
                ParentConfiguration.AddEvent(ref argetype5, ref argdescription5, argline, argfile5, variable: argvariable5, varname: argvarname5);
                return false;
            }

            // If else structure is valid
            return true;
        }

        public void ParseTokenPhrase(List<CFGToken> Tokens, int StartToken, int EndToken)
        {
            int i;
            var parStackIndex = new Stack();

            var loopTo = EndToken;
            for (i = StartToken; i <= loopTo; i++)
            {
                if (Tokens[i].TokenType == CFGEnums.TT.ttOPENPARENTHESIS)
                {
                    parStackIndex.Push(i);
                }
                else if (Tokens[i].TokenType == CFGEnums.TT.ttCLOSEPARENTHESIS)
                {
                    int index = Convert.ToInt32(parStackIndex.Peek());
                    Tokens[i].EncapluationMatch = index;
                    Tokens[index].EncapluationMatch = i;
                    parStackIndex.Pop();
                    break;
                }
            }
        }

        public void ClearTokens()
        {
            foreach (CFGLine line in Lines)
                line.Tokens.Clear();
        }

        public CFGLine GetLine(int lineno)
        {
            foreach (CFGLine cline in Lines)
            {
                if (cline.LineNumber == lineno)
                    return cline;
            }

            return null;
        }

    }
}
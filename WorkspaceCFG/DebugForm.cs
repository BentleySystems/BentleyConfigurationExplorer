// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Linq;


namespace WorkspaceCFG
{

    public partial class DebugForm : ILogger
    {

        public DebugForm()
        {
            InitializeComponent();
            InitializeDefaults();
        }

        private void InitializeDefaults()
        {
            string text = "_USTN_WORKSETCFG = C:/Example/WorkSpaces/ExampleWorkSet.cfg" + Environment.NewLine + "MS_RFDIR = $(_USTN_WORKSETCFG)" + Environment.NewLine + @"TEST = C:\Example\output\(filename(devdir($(_USTN_WORKSETCFG))(new)(ext($(_USTN_WORKSETCFG)))))";

            TextBoxLine.Text = text;
            TextBoxDebug.Text = string.Empty;
        }

        private void ButtonParse_Click(object sender, EventArgs e)
        {
            TextBoxDebug.Text = string.Empty;
            ParseLines();
        }

        private void ParseLines()
        {
            // Setup file and workspace
            var argwrkspc = new CFGConfiguration();
            var File = new CFGFile(ref argwrkspc);

            int lineNumber = 1;

            var parser = new MacroParser(this, ref File) { DEBUG_LOGGING = true };

            foreach (string textLine in TextBoxLine.Lines)
            {
                // Create new line object
                var Line = new CFGLine();
                {
                    ref var withBlock = ref Line;
                    withBlock.Text = textLine;
                    withBlock.LineNumber = lineNumber;
                    withBlock.ParentFile = File;
                    withBlock.IsComment = false;
                    withBlock.TokenizeLine();
                }
                // parser.IndexTokens(Line.Tokens, 0, Line.Tokens.Count - 1)
                // Debug.Write("Line: " & Line.Text)
                parser.ParseLine(ref Line);
                File.Lines.Add(Line);
                lineNumber += 1;
            }

            // Parse
            // Dim parser As New MacroParser(Me, File) With {
            // .DEBUG_LOGGING = True
            // }
            // parser.ParseFile()

            File.ParentConfiguration.ReplaceVariablePlaceholders();

            // Output variable values
            LogLine("");
            LogLine("Final variables:");
            foreach (CFGVariable variable in File.ParentConfiguration.Variables.Values.ToList())
            {
                if (variable is not null)
                {
                    variable.Value = variable.Expand(variable.Level);
                    LogLine($"    {variable.Name}  = {variable.Value}");
                }
            }

        }

        public void LogLine(string message)
        {
            TextBoxDebug.AppendText(message + Environment.NewLine);
        }


    }
}
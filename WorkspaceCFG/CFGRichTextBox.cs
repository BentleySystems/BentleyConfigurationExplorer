// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WorkspaceCFG
{

    public class CFGRichTextBox : RichTextBox
    {
        public CFGRichTextBox()
        {
            KeyUp += CFGRichTextBox_KeyPress;
        }

        [DllImport("user32", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        [DllImport("user32")]
        private static extern int LockWindowUpdate(int hWnd);

        public bool disableupdate;

        // ---------------------------------------------------------------------------------------
        // @description: Private shared members
        // ---------------+---------------+---------------+---------------+---------------+-------
        // TODO: do these match those in CFGLine
        private static readonly Regex PreProcRegx = new Regex("(?<PREPROC>[%][ ]*(?<PROCTYPE>ifndef|ifdef|if|elif|else|endif|lock|undef|error|echo|level|include))(?<ARGUMENT>.*)", RegexOptions.Compiled);
        private static readonly Regex VarDefRegx = new Regex("(?<VARIABLE>[^:=<>+]*)(?<OPERATOR>[:=<>+]?)(?<ARGUMENT>.*)", RegexOptions.Compiled);
        private static readonly Regex FuncDefRegx = new Regex("[# ,{(!](?<FUNCTION>[ ]*(?<NAME>defined|exists|parentdevdir|parentdir|devdir|dir|dev|basename|filename|noext|ext|first|concat|build|registryread)[ ]*)[({]{1}?", RegexOptions.Compiled);
        private static readonly Regex VarValRegx = new Regex(@"(?<USEVAR>\$\((?<VARIABLE>[^])}#]*)\))", RegexOptions.Compiled);
        private static readonly Regex VarvalAbsRegx = new Regex(@"(?<USEVAR>\$\{(?<VARIABLE>[^])}#]*)\})", RegexOptions.Compiled);
        private static readonly Regex PreProcCharRegx = new Regex(@"(?<PREPROCCHAR>[ ]*(?<CHAR>\|\||&&|\||<<|>>|<=|>=|\+|&|<|>|%|\*|\^|!=|==|[!,])[ ]*)", RegexOptions.Compiled);
        private string CurrentFilePath;
        private string _searchCriteria;
        // ---------------------------------------------------------------------------------------
        // @description: Public members
        // ---------------+---------------+---------------+---------------+---------------+-------
        public bool NeedsToBeSaved;

        // ---------------------------------------------------------------------------------------
        // @description: Loads a CFG file into the RTbox
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void LoadCFGFile(ref string filepath, [Optional] ref CFGFile cfile)
        {
            // Set the tab spacing
            int t;
            var na = new int[32];
            for (t = 0; t <= 31; t++)
                na[t] = (t + 1) * 27;
            SelectionTabs = na;
            AcceptsTab = true;

            // Lock the update
            LockWindowUpdate(Handle.ToInt32());

            // Load the file into the RTbox
            try
            {
                if (File.Exists(filepath))
                {
                    LoadFile(filepath, RichTextBoxStreamType.PlainText);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                NeedsToBeSaved = false;
                return;
            }

            ColorAllLines();

            if (cfile is not null)
                ColorErrorLine(ref cfile);

            SelectionStart = 0;
            SelectionLength = 0;
            ClearUndo();

            // Unlock the update
            LockWindowUpdate(0);

            CurrentFilePath = filepath;
            NeedsToBeSaved = false;
        }

        public void LoadCFGFile(ref CFGFile cfile)
        {
            // Set the tab spacing to mimic default notpad++
            int t;
            var na = new int[32];
            for (t = 0; t <= 31; t++)
                na[t] = (t + 1) * 27;
            SelectionTabs = na;
            AcceptsTab = true;

            // Lock the window update
            LockWindowUpdate(Handle.ToInt32());

            // Load the file into the RTbox
            Clear();
            ClearUndo();

            foreach (CFGLine cline in cfile.Lines)
            {
                if (cline is not null && !string.IsNullOrEmpty(cline.Text))
                {
                    AppendText(cline.Text);
                }

                AppendText(Environment.NewLine);
            }

            ColorAllLines();

            ColorErrorLine(ref cfile);

            SelectionStart = 0;
            SelectionLength = 0;
            ClearUndo();

            // Unlock the update
            LockWindowUpdate(0);

            CurrentFilePath = cfile.FilePath;
            NeedsToBeSaved = false;
        }

        public void SaveCFGFile()
        {
            // Load the file into the RTbox
            try
            {
                if (File.Exists(CurrentFilePath))
                {
                    SaveFile(CurrentFilePath, RichTextBoxStreamType.PlainText);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  [ERROR] {GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                NeedsToBeSaved = false;
                return;
            }

            NeedsToBeSaved = false;
        }
        // ---------------------------------------------------------------------------------------
        // @description: Colors each line
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void ColorAllLines()
        {
            int t;

            // Color Each Line
            var loopTo = Lines.Length - 1;
            for (t = 0; t <= loopTo; t++)
            {
                bool argdynamic = false;
                ColorLineNumber(t, dynamic: ref argdynamic);
            }
        }

        // ---------------------------------------------------------------------------------------
        // @description: Colors a single line
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void ColorLineNumber(int LineIndex, [Optional, DefaultParameterValue(false)] ref bool dynamic)
        {
            string ln;
            int cl;
            long currentpos;

            if (dynamic)
                LockWindowUpdate(Handle.ToInt32());

            currentpos = SelectionStart;
            int firstchr = GetCharFromLineIndex(LineIndex);

            Match mtch;
            MatchCollection mtchs;

            ln = Lines[LineIndex].Replace("\t", " ");

            if (dynamic)
            {
                SelectionStart = firstchr;
                SelectionLength = Lines[LineIndex].Length;
                SelectionColor = Color.Black;
            }

            if (string.IsNullOrWhiteSpace(ln))
            {
                if (dynamic)
                    LockWindowUpdate(0);
                return;
            }

            mtch = PreProcRegx.Match(ln);

            {
                var withBlock = mtch.Groups["PREPROC"];
                if (!string.IsNullOrEmpty(withBlock.Value))
                {
                    SelectionStart = firstchr + withBlock.Index;
                    SelectionLength = withBlock.Value.Length;
                    SelectionColor = Color.Blue;
                }
            }

            // Find variable definitions $(variable)
            mtchs = VarValRegx.Matches(ln);
            foreach (Match currentMtch in mtchs)
            {
                mtch = currentMtch;
                {
                    var withBlock1 = mtch.Groups["USEVAR"];
                    if (!string.IsNullOrWhiteSpace(withBlock1.Value))
                    {
                        SelectionStart = firstchr + withBlock1.Index;
                        SelectionLength = withBlock1.Value.Length;
                        SelectionColor = Color.DarkGray;
                    }
                }
            }

            // Find variable definitions ${variable}
            mtchs = VarvalAbsRegx.Matches(ln);
            foreach (Match currentMtch1 in mtchs)
            {
                mtch = currentMtch1;
                {
                    var withBlock2 = mtch.Groups["USEVAR"];
                    if (!string.IsNullOrWhiteSpace(withBlock2.Value))
                    {
                        SelectionStart = firstchr + withBlock2.Index;
                        SelectionLength = withBlock2.Value.Length;
                        SelectionColor = Color.DarkGoldenrod;
                    }
                }
            }

            mtchs = PreProcCharRegx.Matches(ln);
            foreach (Match currentMtch2 in mtchs)
            {
                mtch = currentMtch2;
                {
                    var withBlock3 = mtch.Groups["PREPROCCHAR"];
                    if (!string.IsNullOrWhiteSpace(withBlock3.Value))
                    {
                        SelectionStart = firstchr + withBlock3.Index;
                        SelectionLength = withBlock3.Value.Length;
                        SelectionColor = Color.Blue;
                    }
                }
            }

            // Find function definitions
            mtchs = FuncDefRegx.Matches(ln);
            foreach (Match currentMtch3 in mtchs)
            {
                mtch = currentMtch3;
                {
                    var withBlock4 = mtch.Groups["FUNCTION"];
                    if (!string.IsNullOrWhiteSpace(withBlock4.Value))
                    {
                        SelectionStart = firstchr + withBlock4.Index;
                        SelectionLength = withBlock4.Value.Length;
                        SelectionColor = Color.Blue;
                        // SelectionFont = BoldFont
                    }
                }
            }

            // Comments
            cl = ln.IndexOf("#", StringComparison.Ordinal);
            if (cl >= 0)
            {
                SelectionStart = firstchr + cl;
                SelectionLength = ln.Length - cl;
                SelectionColor = Color.Green;
            }

            // Not Processed
            if (string.Compare(ln.Substring(0, 1), "*", CultureInfo.CurrentCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                SelectionStart = firstchr;
                SelectionLength = ln.Length;
                SelectionColor = Color.DarkOliveGreen;
            }

            SelectionStart = (int)currentpos;
            SelectionLength = 0;
            SelectionColor = Color.Black;

            if (dynamic)
                LockWindowUpdate(0);
        }

        // ---------------------------------------------------------------------------------------
        // @description: Color error line
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void ColorErrorLine(ref CFGFile cfile)
        {
            int currentpos = SelectionStart;

            // Prevent UI flickering during updates
            LockWindowUpdate(Handle.ToInt32());

            foreach (CFGLine cln in cfile.Lines)
            {
                if (cln.HasErrors || cln.HasWarnings)
                {
                    int firstChar = GetCharFromLineIndex(cln.LineNumber - 1);
                    string lineText = Lines[cln.LineNumber - 1].Replace("\t", " ");

                    SelectionStart = firstChar;
                    SelectionLength = lineText.Length;
                    SelectionBackColor = cln.HasErrors ? Color.OrangeRed : Color.LightPink;
                }
            }

            // Restore cursor and selection
            SelectionStart = currentpos;
            SelectionLength = 0;
            SelectionColor = Color.Black;

            LockWindowUpdate(0);
        }

        // ---------------------------------------------------------------------------------------
        // @description: Returns the first character index in a given line
        // ---------------+---------------+---------------+---------------+---------------+-------
        public int GetCharFromLineIndex(int LineIndex)
        {
            return SendMessage(Handle, 187, LineIndex, 0);
        }

        private void CFGRichTextBox_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.Control || e.KeyCode == Keys.ControlKey)
            {
                switch (e.KeyCode)
                {
                    case Keys.F:
                        {
                            ShowSearchForm();
                            break;
                        }
                    case Keys.R:
                        {
                            Redo();
                            break;
                        }
                    case Keys.C:
                        {
                            Copy();
                            break;
                        }
                    case Keys.X:
                        {
                            Cut();
                            break;
                        }
                    case Keys.V:
                        {
                            Paste();
                            break;
                        }
                    case Keys.Z:
                        {
                            if (CultureInfo.CurrentCulture.CompareInfo.Compare(UndoActionName, "Unknown", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                            {
                                while (CultureInfo.CurrentCulture.CompareInfo.Compare(UndoActionName, "Unknown", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                                    Undo();
                            }
                            Undo();
                            break;
                        }
                }
                return;
            }

            if (e.KeyCode == Keys.F3)
                FindNext();

            if (e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.Alt)
                return;
            if (e.KeyCode == Keys.End)
                return;
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                return;
            if (e.KeyCode == Keys.PageDown || e.KeyCode == Keys.PageDown)
                return;

            bool argdynamic = true;
            ColorLineNumber(GetLineFromCharIndex(SelectionStart), ref argdynamic);
        }

        public void ToggleCommentBlock()
        {
            long strline;
            long endline;
            long t;
            long mode;
            long index;
            long cl;

            strline = GetLineFromCharIndex(SelectionStart);
            endline = GetLineFromCharIndex(SelectionStart + SelectionLength);
            mode = 0L;

            var loopTo = endline;
            for (t = strline; t <= loopTo; t++)
            {
                if (!string.IsNullOrWhiteSpace(Lines[(int)t]))
                {

                    if (mode == 0L)
                    {
                        var tmp = Lines;
                        string arglstr = tmp[(int)strline];
                        if (GetIsCommentLine(ref arglstr) >= 0L)
                        {
                            mode = 1L;
                        }
                        else
                        {
                            mode = 2L;
                        }
                    }
                    index = GetFirstCharIndexFromLine((int)t);

                    if (mode == 1L)
                    {
                        var tmp1 = Lines;
                        string arglstr1 = tmp1[(int)t];
                        cl = GetIsCommentLine(ref arglstr1);
                        if (cl >= 0L)
                        {
                            SelectionStart = (int)(index + cl);
                            SelectionLength = 1;
                            SelectedText = "";
                        }
                    }

                    if (mode == 2L)
                    {
                        SelectionStart = (int)index;
                        SelectionLength = 0;
                        SelectedText = "#";
                    }

                    bool argdynamic = true;
                    ColorLineNumber((int)t, ref argdynamic);
                }
            }
        }

        private long GetIsCommentLine(ref string lstr)
        {
            long t;
            if (lstr.Length > 0)
            {
                var loopTo = (long)lstr.Length;
                for (t = 0L; t <= loopTo; t++)
                {
                    if (lstr[(int)t].ToString() != " " & lstr[(int)t] != '\t')
                    {
                        if (lstr[(int)t].ToString() == "#")
                        {
                            return t;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                }
            }

            return -1;
        }


        public void FormatStyle(ref CFGFile cfile)
        {
            string cl;                        // current text
            int current_indent;
            var varindent = default(int);
            bool first;
            string temp;

            // Lock the update
            LockWindowUpdate(Handle.ToInt32());

            // Clear existing text
            Clear();
            current_indent = 0;
            first = false;

            foreach (CFGLine line in cfile.Lines)
            {
                cl = line.Text;

                if (!line.IsComment)
                {
                    if (current_indent != line.Indent || first == false)
                    {
                        varindent = GetVariableIndent(line,cfile);
                        first = true;
                    }

                    current_indent = line.Indent;
                }

                if (line.IsVarDef)
                    cl = SetVariableIndent(line, varindent);

                if (line.IsComment)
                {
                    temp = cl + line.InlineComment;
                    if (temp.Length > 0)
                    {
                        if (current_indent > 0 || (temp.Length > 1 && IsAcceptableChar(temp[1])))
                        {
                            AppendText(new string('\t', current_indent + 1) + cl + line.InlineComment + Environment.NewLine);
                        }
                        else
                        {
                            AppendText(cl + line.InlineComment + '\n');
                        }
                    }
                    else
                    {
                        AppendText("\n");
                    }
                }
                else
                {
                    AppendText(new string('\t', line.Indent + 1) + cl.Replace("\\", "/") + "  " + line.InlineComment + Environment.NewLine);
                }
            }

            ColorAllLines();

            NeedsToBeSaved = true;

            ClearUndo();

            // Unlock the update
            LockWindowUpdate(0);
        }

        private int GetVariableIndent(CFGLine line, CFGFile cfile)
        {
            int ci;
            int cl;
            CFGLine lref;
            int result;
            int x;

            cl = line.LineNumber - 1;
            ci = line.Indent;
            result = 0;

            while (cl < cfile.Lines.Count)
            {
                lref = cfile.Lines[cl];
                if (lref.Indent != ci)
                    return result;
                if (lref.IsVarDef)
                {
                    x = (int)Math.Round(Math.Ceiling(lref.VarName.Length / 4d));
                    if (x > result)
                        result = x;
                }
                cl += 1;
            }

            return result;
        }

        private string SetVariableIndent(CFGLine line, int indent)
        {
            string text = line.Text;
            int oppos = text.Length;

            // Find the earliest position of any of the target characters
            foreach (var ch in new[] { '=', '+', '<', '>', ':' })
            {
                int index = text.IndexOf(ch);
                if (index > 0 && index < oppos)
                    oppos = index;
            }

            //string varName = line.VarName.ToUpperInvariant();
            string varName = line.VarName;
            int tabCount = (int)Math.Round(indent - Math.Floor(line.VarName.Length / 4d) + 2d);

            if (indent == 0)
            {
                tabCount = 1;
                if (line.VarName.Length % 4 == 0)
                    tabCount += 1;
            }
            else if (tabCount < 1)
            {
                tabCount = 1;
                if (line.VarName.Length % 4 == 0)
                    tabCount += 1;
            }

            string separator = oppos < text.Length ? text[oppos].ToString() : string.Empty;
            string remainder = oppos + 1 < text.Length ? text.Substring(oppos + 1).Trim() : string.Empty;

            return varName + new string('\t', tabCount) + separator + '\t' + remainder;
        }

        private bool IsAcceptableChar(char x)
        {
            return char.IsLetterOrDigit(x) | x.ToString() == "_";
        }

        private void ShowSearchForm()
        {
            object form = new SearchForm();

            // Did the user click Save?
            if (Convert.ToBoolean((((dynamic)form).ShowDialog() == DialogResult.OK, DialogResult.OK, true)))
            {
                _searchCriteria = ((dynamic)form).SearchCriteria?.ToString();
                SelectionStart = 0;
                FindNext();
            }
        }

        public void FindNext()
        {
            int result = Find(_searchCriteria, SelectionStart + 1, RichTextBoxFinds.NoHighlight);

            if (result > 0)
            {
                SelectionStart = result;
            }
            else
            {
                SelectionStart = 0;
            }

            SelectionLength = _searchCriteria.Length;
        }

    }
}
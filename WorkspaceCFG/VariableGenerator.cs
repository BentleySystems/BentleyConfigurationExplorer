// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections; // <-- Required for ArrayList
using System.Data;
using System.Globalization;
using System.Runtime.InteropServices;
using WorkspaceCFG.My;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{
    // ---------------------------------------------------------------------------------------
    // 
    // Variable Generator
    // 
    // Creates expressions for variable definitions.
    // 
    // 
    // ---------------------------------------------------------------------------------------

    public partial class VariableGenerator
    {

        public struct Candidate
        {
            // Candidate variable
            public string Name;
            public int StartPos;
            public int EndPos;
            public int Backtrack;
            public string Value;
            public int Score;
        }

        private CFGEnums.ConfigurationType _workspaceType;
        private CFGConfiguration _workspace;
        private string _targetPath;
        private ArrayList _candidates;
        private DataTable _dataTable;

        public VariableGenerator(CFGEnums.ConfigurationType workspaceType)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            _workspaceType = workspaceType;
            _targetPath = "";
            RefreshForm();
        }

        public void RefreshForm()
        {
            // Fills the form with data from the wrkspc object
            // Can be called from the Interface Controller

            InterfaceControler.GetConfiguration(ref _workspace, _workspaceType);
            if (_workspace is null)
                return;

            Text = CEResource.TXT_TitleVariableGenerator + _workspace.Description2;
        }

        private void VariableGenerator_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            object argform = this;
            InterfaceControler.RemoveForm(ref argform);
            MySettings.Default.VG_Location = Location;
            MySettings.Default.VG_WindowState = WindowState;
            if (WindowState == System.Windows.Forms.FormWindowState.Normal)
                MySettings.Default.VG_Size = Size;
            MySettings.Default.Save();
        }

        private void VariableGenerator_Shown(object sender, EventArgs e)
        {
            // Initialize connection to Interface Controller
            object argform = this;
            InterfaceControler.AddForm(ref argform);
        }

        private void WatchF5_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.F5)
            {
                InterfaceControler.RefreshOpenForms();
            }
        }

        private void VariableGenerator_Load(object sender, EventArgs e)
        {
            Location = MySettings.Default.VG_Location;
            WindowState = MySettings.Default.VG_WindowState;
            if (WindowState == System.Windows.Forms.FormWindowState.Normal)
                Size = MySettings.Default.VG_Size;
        }

        public void GenerateSolutions()
        {
            // Generate Suggestions

            if (string.IsNullOrEmpty((_targetPath ?? "").Trim()))
                return;

            MakeCandidateList();

            var solution = new ArrayList();

            _dataTable = new DataTable();
            _dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCExpression));
            _dataTable.Columns.Add(new DataColumn(CEResource.TXT_DCScore));

            _dataTable.BeginLoadData();

            BuildVariables(ref solution, 0);

            _dataTable.EndLoadData();

            if (_dataTable.Rows.Count == 1)
            {
                Label6.Text = CEResource.TXT_MsgSingleResult;
            }
            else
            {
                Label6.Text = _dataTable.Rows.Count + CEResource.TXT_MsgMultipleResults;
            }

            dgSolutions.DataSource = _dataTable;
            dgSolutions.Sort(dgSolutions.Columns[1], System.ComponentModel.ListSortDirection.Descending);
            dgSolutions.Columns[1].Visible = false;
            // dgSolutions.Refresh()
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            GenerateSolutions();
        }

        private void MakeCandidateList()
        {
            if (_workspace is null)
                return;

            string temp;
            bool found;
            int backtrack;

            temp = "";
            _candidates = new ArrayList();

            foreach (CFGVariable @var in _workspace.Variables.Values)
            {
                if (!string.IsNullOrEmpty((@var.FinalExpansion ?? "").Trim()))
                {
                    if (@var.NumberOfValues() == 1)
                    {
                        found = false;
                        backtrack = 0;
                        if (UtilitiesPath.IsPath(ref @var.FinalExpansion))
                        {
                            int argbacktrack = 0;
                            found = CheckCandidate(ref @var.FinalExpansion, ref @var.Name, ref argbacktrack);
                            if (!found)
                                temp = Utilities.ParentDevDir(@var.FinalExpansion);
                            while (!string.IsNullOrEmpty(temp) & found == false & backtrack < NumericUpDown1.Value)
                            {
                                backtrack += 1;
                                found = CheckCandidate(ref temp, ref @var.Name, ref backtrack);
                                temp = Utilities.ParentDevDir(temp);
                            }
                        }
                        else
                        {
                            int argbacktrack1 = 0;
                            CheckCandidate(ref @var.FinalExpansion, ref @var.Name, ref argbacktrack1);
                        }
                    }
                }
            }
        }

        private bool CheckCandidate(ref string value, ref string varname, [Optional, DefaultParameterValue(0)] ref int backtrack)
        {
            int startpos;
            Candidate candidate;

            if (string.IsNullOrEmpty(value))
                return false;
            if (value.Length < NumericUpDown2.Value)
                return false;

            startpos = (_targetPath ?? "").IndexOf(value, StringComparison.CurrentCultureIgnoreCase) + 1;

            if (startpos == 0)
                return false;

            while (startpos < _targetPath.Length & startpos > 0)
            {
                candidate = new Candidate()
                {
                    Backtrack = backtrack,
                    StartPos = startpos - 1,
                    EndPos = startpos + value.Length - 1,
                    Name = "$(" + varname + ")",
                    Value = value,
                    Score = -75 * backtrack
                };

                if (startpos == 1)
                    candidate.Score += 2;

                _candidates.Add(candidate);
                startpos = (_targetPath ?? "").IndexOf(value, startpos, StringComparison.CurrentCultureIgnoreCase) + 1;

                if (string.IsNullOrEmpty(value))
                    return true;
            }

            return true;
        }

        private void BuildVariables(ref ArrayList solution, int pos = 0)
        {
            if (_dataTable.Rows.Count >= 5000)
                return; // Limit Solutions to 5000

            int t;
            int v;
            Candidate candidate;
            string result;
            var score = default(int);

            if (pos >= _targetPath.Length)
            {
                // report solution
                result = "";
                var loopTo = solution.Count;
                for (t = 1; t <= loopTo; t++)
                {
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(solution[t].GetType().ToString(), "System.Char", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                    {
                        result = Convert.ToString(result + solution[t]);
                        score -= 2;
                    }
                    else
                    {
                        score += Convert.ToInt32(((dynamic)solution[t]).Score);
                        result += BuildParentDevDirExpression(((dynamic)solution[t]).Name?.ToString(), Convert.ToInt32(((dynamic)solution[t]).Backtrack));

                    }
                }
                AddResult(ref result, score);
                return;
            }

            // Attempt the next character
            solution.Add(_targetPath[pos]);
            BuildVariables(ref solution, pos + 1);
            solution.RemoveAt(solution.Count - 1);

            // Attempt to find a candidate variable
            var loopTo2 = _candidates.Count;
            for (v = 1; v <= loopTo2; v++)
            {
                candidate = (Candidate)_candidates[v - 1];
                if (candidate.StartPos == pos)
                {
                    solution.Add(candidate);
                    BuildVariables(ref solution, pos + candidate.Value.Length);
                    solution.RemoveAt(solution.Count - 1);
                }
            }
        }

        private static string BuildParentDevDirExpression(string variableExpression, int backtrack)
        {
            string result = variableExpression ?? string.Empty;

            for (int index = 0; index < backtrack; index++)
            {
                result = "parentdevdir(" + result + ")";
            }

            return result;
        }

        private void AddResult(ref string value, int score)
        {
            DataRow dataRow;
            string sc;

            dataRow = _dataTable.NewRow();

            dataRow[0] = (value ?? "").Replace(@"\", "/");
            sc = "0000000" + ((1000 + score).ToString().Trim());
            dataRow[1] = sc.Substring(sc.Length - 7);

            _dataTable.Rows.Add(dataRow);
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            _targetPath = (TextBox1.Text ?? "").Replace("/", @"\").Trim();

            _targetPath = MacroParser.ParseLine(_workspace, 6, (_targetPath ?? "").Replace("{", "(").Replace("}", ")"), true);
            if (string.IsNullOrEmpty(_targetPath))
                _targetPath = (TextBox1.Text ?? "").Replace("/", @"\").Trim();

            Label3.Text = _targetPath;
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgSolutions.RowCount == 0)
                return;
            if (dgSolutions.SelectedCells.Count == 0)
                return;

            int t;
            string temp;
            temp = "";

            var withBlock = dgSolutions;
            if (withBlock.SelectedCells.Count == 1)
            {
                temp = Convert.ToString(withBlock.SelectedCells[0].Value)?.Replace(@"\", "/");
            }
            else
            {
                var loopTo = withBlock.Rows.Count - 1;
                for (t = 0; t <= loopTo; t++)
                {
                    if (withBlock.Rows[t].Cells[0].Selected)
                    {
                        temp = temp + (Convert.ToString(withBlock.Rows[t].Cells[0].Value)?.Replace(@"\", "/")) + '\n';
                    }
                }
            }

            System.Windows.Forms.Clipboard.SetText(temp);
        }

        private void CopyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dgSolutions.RowCount == 0)
                return;
            if (dgSolutions.SelectedCells.Count == 0)
                return;

            int t;
            string temp;
            temp = "";

            var withBlock = dgSolutions;
            if (withBlock.SelectedCells.Count == 1)
            {
                temp = Convert.ToString(withBlock.SelectedCells[0].Value)?.Replace("/", @"\");
            }
            else
            {
                var loopTo = withBlock.Rows.Count - 1;
                for (t = 0; t <= loopTo; t++)
                {
                    if (withBlock.Rows[t].Cells[0].Selected)
                    {
                        temp = temp + (Convert.ToString(withBlock.Rows[t].Cells[0].Value)?.Replace("/", @"\")) + '\n';
                    }
                }
            }
            System.Windows.Forms.Clipboard.SetText(temp);
        }
    }
}
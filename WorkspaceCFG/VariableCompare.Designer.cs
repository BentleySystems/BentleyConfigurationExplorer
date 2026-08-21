using System;
using System.Diagnostics;
using System.Drawing;

namespace WorkspaceCFG
{
    public partial class VariableCompare : System.Windows.Forms.Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is not null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            RichTextBox2 = new System.Windows.Forms.RichTextBox();
            ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            OpenVariableExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            OpenVariableExplorerToolStripMenuItem.Click += new EventHandler(OpenVariableExplorerToolStripMenuItem_Click);
            OpenFileHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            OpenFileHistoryToolStripMenuItem.Click += new EventHandler(OpenFileHistoryToolStripMenuItem_Click);
            OpenWorkspaceHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            OpenWorkspaceHistoryToolStripMenuItem.Click += new EventHandler(OpenWorkspaceHistoryToolStripMenuItem_Click);
            ExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToExcelFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToExcelFileToolStripMenuItem.Click += new EventHandler(ToExcelFileToolStripMenuItem_Click);
            RichTextBox1 = new System.Windows.Forms.RichTextBox();
            TextBox1 = new System.Windows.Forms.TextBox();
            TextBox1.TextChanged += new EventHandler(TextBox1_TextChanged);
            Label4 = new System.Windows.Forms.Label();
            Label1 = new System.Windows.Forms.Label();
            Label2 = new System.Windows.Forms.Label();
            Button1 = new System.Windows.Forms.Button();
            Button1.Click += new EventHandler(Button1_Click);
            DataGridView1 = new System.Windows.Forms.DataGridView();
            DataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(DataGridView1_DataBindingComplete);
            DataGridView1.Scroll += new System.Windows.Forms.ScrollEventHandler(DataGridView1_Scroll);
            DataGridView1.SelectionChanged += new EventHandler(DataGridView1_SelectionChanged);
            DataGridView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(DataGridView1_CellContentDoubleClick);
            DataGridView1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(DataGridView1_KeyPress);
            LBwrk1Desc = new System.Windows.Forms.Label();
            SplitContainer1 = new System.Windows.Forms.SplitContainer();
            SplitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(SplitContainer1_SplitterMoved);
            DataGridView2 = new System.Windows.Forms.DataGridView();
            DataGridView2.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(DataGridView2_DataBindingComplete);
            DataGridView2.Scroll += new System.Windows.Forms.ScrollEventHandler(DataGridView2_Scroll);
            DataGridView2.SelectionChanged += new EventHandler(DataGridView2_SelectionChanged);
            DataGridView2.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(DataGridView2_CellContentDoubleClick);
            DataGridView2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(DataGridView2_KeyPress);
            ContextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(components);
            ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem1.Click += new EventHandler(ToolStripMenuItem1_Click);
            ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem2.Click += new EventHandler(ToolStripMenuItem2_Click);
            ToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem3.Click += new EventHandler(ToolStripMenuItem3_Click);
            ToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem6.Click += new EventHandler(ToolStripMenuItem6_Click);
            LBwrk2Desc = new System.Windows.Forms.Label();
            RichTextBox4 = new System.Windows.Forms.RichTextBox();
            Label6 = new System.Windows.Forms.Label();
            Label8 = new System.Windows.Forms.Label();
            RichTextBox3 = new System.Windows.Forms.RichTextBox();
            CB_VAL_DIFF = new System.Windows.Forms.CheckBox();
            CB_VAL_DIFF.CheckedChanged += new EventHandler(CheckBox4_CheckedChanged);
            CB_EXP_DIF = new System.Windows.Forms.CheckBox();
            CB_EXP_DIF.CheckedChanged += new EventHandler(CheckBox1_CheckedChanged);
            SplitContainer2 = new System.Windows.Forms.SplitContainer();
            SplitContainer2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(SplitContainer2_SplitterMoved);
            SplitContainer4 = new System.Windows.Forms.SplitContainer();
            SplitContainer5 = new System.Windows.Forms.SplitContainer();
            SplitContainer3 = new System.Windows.Forms.SplitContainer();
            CheckBox1 = new System.Windows.Forms.CheckBox();
            CheckBox1.CheckedChanged += new EventHandler(CheckBox1_CheckedChanged_1);
            CheckBox2 = new System.Windows.Forms.CheckBox();
            CheckBox2.CheckedChanged += new EventHandler(CheckBox2_CheckedChanged);
            SaveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            RichTextBox5 = new System.Windows.Forms.RichTextBox();
            CheckBox3 = new System.Windows.Forms.CheckBox();
            CheckBox3.CheckedChanged += new EventHandler(CheckBox3_CheckedChanged);
            ContextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SplitContainer1).BeginInit();
            SplitContainer1.Panel1.SuspendLayout();
            SplitContainer1.Panel2.SuspendLayout();
            SplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DataGridView2).BeginInit();
            ContextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SplitContainer2).BeginInit();
            SplitContainer2.Panel1.SuspendLayout();
            SplitContainer2.Panel2.SuspendLayout();
            SplitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SplitContainer4).BeginInit();
            SplitContainer4.Panel1.SuspendLayout();
            SplitContainer4.Panel2.SuspendLayout();
            SplitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SplitContainer5).BeginInit();
            SplitContainer5.Panel1.SuspendLayout();
            SplitContainer5.Panel2.SuspendLayout();
            SplitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SplitContainer3).BeginInit();
            SplitContainer3.Panel1.SuspendLayout();
            SplitContainer3.Panel2.SuspendLayout();
            SplitContainer3.SuspendLayout();
            SuspendLayout();
            // 
            // RichTextBox2
            // 
            RichTextBox2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            RichTextBox2.ContextMenuStrip = ContextMenuStrip1;
            RichTextBox2.DetectUrls = false;
            RichTextBox2.Location = new Point(5, 19);
            RichTextBox2.Name = "RichTextBox2";
            RichTextBox2.ReadOnly = true;
            RichTextBox2.Size = new Size(439, 103);
            RichTextBox2.TabIndex = 23;
            RichTextBox2.Text = "";
            RichTextBox2.WordWrap = false;
            // 
            // ContextMenuStrip1
            // 
            ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { OpenVariableExplorerToolStripMenuItem, OpenFileHistoryToolStripMenuItem, OpenWorkspaceHistoryToolStripMenuItem, ExportToolStripMenuItem });
            ContextMenuStrip1.Name = "ContextMenuStrip1";
            ContextMenuStrip1.Size = new Size(206, 92);
            // 
            // OpenVariableExplorerToolStripMenuItem
            // 
            OpenVariableExplorerToolStripMenuItem.Name = "OpenVariableExplorerToolStripMenuItem";
            OpenVariableExplorerToolStripMenuItem.Size = new Size(205, 22);
            OpenVariableExplorerToolStripMenuItem.Text = "Open Variable Explorer";
            // 
            // OpenFileHistoryToolStripMenuItem
            // 
            OpenFileHistoryToolStripMenuItem.Name = "OpenFileHistoryToolStripMenuItem";
            OpenFileHistoryToolStripMenuItem.Size = new Size(205, 22);
            OpenFileHistoryToolStripMenuItem.Text = "Open File History";
            // 
            // OpenWorkspaceHistoryToolStripMenuItem
            // 
            OpenWorkspaceHistoryToolStripMenuItem.Name = "OpenWorkspaceHistoryToolStripMenuItem";
            OpenWorkspaceHistoryToolStripMenuItem.Size = new Size(205, 22);
            OpenWorkspaceHistoryToolStripMenuItem.Text = "Open Workspace History";
            // 
            // ExportToolStripMenuItem
            // 
            ExportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ToExcelFileToolStripMenuItem });
            ExportToolStripMenuItem.Name = "ExportToolStripMenuItem";
            ExportToolStripMenuItem.Size = new Size(205, 22);
            ExportToolStripMenuItem.Text = "Export";
            // 
            // ToExcelFileToolStripMenuItem
            // 
            ToExcelFileToolStripMenuItem.Name = "ToExcelFileToolStripMenuItem";
            ToExcelFileToolStripMenuItem.Size = new Size(137, 22);
            ToExcelFileToolStripMenuItem.Text = "To Excel File";
            // 
            // RichTextBox1
            // 
            RichTextBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            RichTextBox1.ContextMenuStrip = ContextMenuStrip1;
            RichTextBox1.DetectUrls = false;
            RichTextBox1.Location = new Point(6, 19);
            RichTextBox1.Name = "RichTextBox1";
            RichTextBox1.ReadOnly = true;
            RichTextBox1.Size = new Size(438, 95);
            RichTextBox1.TabIndex = 21;
            RichTextBox1.Text = "";
            RichTextBox1.WordWrap = false;
            // 
            // TextBox1
            // 
            TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            TextBox1.Location = new Point(6, 24);
            TextBox1.Name = "TextBox1";
            TextBox1.Size = new Size(204, 22);
            TextBox1.TabIndex = 22;
            // 
            // Label4
            // 
            Label4.AutoSize = true;
            Label4.BackColor = Color.Transparent;
            Label4.Font = new Font("Segoe UI", 8.0f);
            Label4.Location = new Point(3, 8);
            Label4.Name = "Label4";
            Label4.Size = new Size(68, 13);
            Label4.TabIndex = 29;
            Label4.Text = "Name Filter:";
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.BackColor = Color.Transparent;
            Label1.Font = new Font("Segoe UI", 8.0f);
            Label1.Location = new Point(3, 0);
            Label1.Name = "Label1";
            Label1.Size = new Size(63, 13);
            Label1.TabIndex = 26;
            Label1.Text = "Expansion:";
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.BackColor = Color.Transparent;
            Label2.Font = new Font("Segoe UI", 8.0f);
            Label2.Location = new Point(5, 0);
            Label2.Name = "Label2";
            Label2.Size = new Size(38, 13);
            Label2.TabIndex = 25;
            Label2.Text = "Value:";
            // 
            // Button1
            // 
            Button1.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            Button1.Location = new Point(216, 24);
            Button1.Name = "Button1";
            Button1.Size = new Size(46, 20);
            Button1.TabIndex = 24;
            Button1.Text = "Clear";
            Button1.UseVisualStyleBackColor = true;
            // 
            // DataGridView1
            // 
            DataGridView1.AllowUserToAddRows = false;
            DataGridView1.AllowUserToDeleteRows = false;
            DataGridView1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            DataGridView1.BackgroundColor = Color.White;
            DataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            DataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridView1.ContextMenuStrip = ContextMenuStrip1;
            DataGridView1.Cursor = System.Windows.Forms.Cursors.Default;
            DataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            DataGridView1.GridColor = Color.LightSteelBlue;
            DataGridView1.Location = new Point(9, 27);
            DataGridView1.MultiSelect = false;
            DataGridView1.Name = "DataGridView1";
            DataGridView1.RowHeadersVisible = false;
            DataGridView1.RowTemplate.DefaultCellStyle.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            DataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            DataGridView1.RowTemplate.Height = 22;
            DataGridView1.RowTemplate.ReadOnly = true;
            DataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            DataGridView1.ShowRowErrors = false;
            DataGridView1.Size = new Size(428, 207);
            DataGridView1.TabIndex = 20;
            // 
            // LBwrk1Desc
            // 
            LBwrk1Desc.AutoSize = true;
            LBwrk1Desc.BackColor = Color.Transparent;
            LBwrk1Desc.Font = new Font("Segoe UI", 8.0f);
            LBwrk1Desc.Location = new Point(9, 8);
            LBwrk1Desc.Name = "LBwrk1Desc";
            LBwrk1Desc.Size = new Size(156, 13);
            LBwrk1Desc.TabIndex = 36;
            LBwrk1Desc.Text = "Workspace Description Here:";
            // 
            // SplitContainer1
            // 
            SplitContainer1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            SplitContainer1.BackColor = SystemColors.Control;
            SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            SplitContainer1.Cursor = System.Windows.Forms.Cursors.SizeWE;
            SplitContainer1.Location = new Point(3, 3);
            SplitContainer1.Name = "SplitContainer1";
            // 
            // SplitContainer1.Panel1
            // 
            SplitContainer1.Panel1.Controls.Add(DataGridView1);
            SplitContainer1.Panel1.Controls.Add(LBwrk1Desc);
            // 
            // SplitContainer1.Panel2
            // 
            SplitContainer1.Panel2.Controls.Add(DataGridView2);
            SplitContainer1.Panel2.Controls.Add(LBwrk2Desc);
            SplitContainer1.Size = new Size(971, 242);
            SplitContainer1.SplitterDistance = 447;
            SplitContainer1.TabIndex = 37;
            // 
            // DataGridView2
            // 
            DataGridView2.AllowUserToAddRows = false;
            DataGridView2.AllowUserToDeleteRows = false;
            DataGridView2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            DataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            DataGridView2.BackgroundColor = Color.White;
            DataGridView2.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            DataGridView2.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridView2.ContextMenuStrip = ContextMenuStrip2;
            DataGridView2.Cursor = System.Windows.Forms.Cursors.Default;
            DataGridView2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            DataGridView2.GridColor = Color.LightSteelBlue;
            DataGridView2.Location = new Point(5, 27);
            DataGridView2.MultiSelect = false;
            DataGridView2.Name = "DataGridView2";
            DataGridView2.RowHeadersVisible = false;
            DataGridView2.RowTemplate.DefaultCellStyle.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            DataGridView2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            DataGridView2.RowTemplate.Height = 22;
            DataGridView2.RowTemplate.ReadOnly = true;
            DataGridView2.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            DataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            DataGridView2.ShowRowErrors = false;
            DataGridView2.Size = new Size(511, 207);
            DataGridView2.TabIndex = 37;
            // 
            // ContextMenuStrip2
            // 
            ContextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripMenuItem1, ToolStripMenuItem2, ToolStripMenuItem3, ToolStripMenuItem4 });
            ContextMenuStrip2.Name = "ContextMenuStrip1";
            ContextMenuStrip2.Size = new Size(206, 92);
            // 
            // ToolStripMenuItem1
            // 
            ToolStripMenuItem1.Name = "ToolStripMenuItem1";
            ToolStripMenuItem1.Size = new Size(205, 22);
            ToolStripMenuItem1.Text = "Open Variable Explorer";
            // 
            // ToolStripMenuItem2
            // 
            ToolStripMenuItem2.Name = "ToolStripMenuItem2";
            ToolStripMenuItem2.Size = new Size(205, 22);
            ToolStripMenuItem2.Text = "Open File History";
            // 
            // ToolStripMenuItem3
            // 
            ToolStripMenuItem3.Name = "ToolStripMenuItem3";
            ToolStripMenuItem3.Size = new Size(205, 22);
            ToolStripMenuItem3.Text = "Open Workspace History";
            // 
            // ToolStripMenuItem4
            // 
            ToolStripMenuItem4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripMenuItem6 });
            ToolStripMenuItem4.Name = "ToolStripMenuItem4";
            ToolStripMenuItem4.Size = new Size(205, 22);
            ToolStripMenuItem4.Text = "Export";
            // 
            // ToolStripMenuItem6
            // 
            ToolStripMenuItem6.Name = "ToolStripMenuItem6";
            ToolStripMenuItem6.Size = new Size(137, 22);
            ToolStripMenuItem6.Text = "To Excel File";
            // 
            // LBwrk2Desc
            // 
            LBwrk2Desc.AutoSize = true;
            LBwrk2Desc.BackColor = Color.Transparent;
            LBwrk2Desc.Font = new Font("Segoe UI", 8.0f);
            LBwrk2Desc.Location = new Point(3, 8);
            LBwrk2Desc.Name = "LBwrk2Desc";
            LBwrk2Desc.Size = new Size(156, 13);
            LBwrk2Desc.TabIndex = 42;
            LBwrk2Desc.Text = "Workspace Description Here:";
            // 
            // RichTextBox4
            // 
            RichTextBox4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            RichTextBox4.ContextMenuStrip = ContextMenuStrip2;
            RichTextBox4.DetectUrls = false;
            RichTextBox4.Location = new Point(8, 20);
            RichTextBox4.Name = "RichTextBox4";
            RichTextBox4.ReadOnly = true;
            RichTextBox4.Size = new Size(511, 94);
            RichTextBox4.TabIndex = 38;
            RichTextBox4.Text = "";
            RichTextBox4.WordWrap = false;
            // 
            // Label6
            // 
            Label6.AutoSize = true;
            Label6.BackColor = Color.Transparent;
            Label6.Font = new Font("Segoe UI", 8.0f);
            Label6.Location = new Point(7, 0);
            Label6.Name = "Label6";
            Label6.Size = new Size(38, 13);
            Label6.TabIndex = 40;
            Label6.Text = "Value:";
            // 
            // Label8
            // 
            Label8.AutoSize = true;
            Label8.BackColor = Color.Transparent;
            Label8.Font = new Font("Segoe UI", 8.0f);
            Label8.Location = new Point(6, 1);
            Label8.Name = "Label8";
            Label8.Size = new Size(63, 13);
            Label8.TabIndex = 41;
            Label8.Text = "Expansion:";
            // 
            // RichTextBox3
            // 
            RichTextBox3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            RichTextBox3.ContextMenuStrip = ContextMenuStrip2;
            RichTextBox3.DetectUrls = false;
            RichTextBox3.Location = new Point(8, 19);
            RichTextBox3.Name = "RichTextBox3";
            RichTextBox3.ReadOnly = true;
            RichTextBox3.Size = new Size(511, 104);
            RichTextBox3.TabIndex = 39;
            RichTextBox3.Text = "";
            RichTextBox3.WordWrap = false;
            // 
            // CB_VAL_DIFF
            // 
            CB_VAL_DIFF.AutoSize = true;
            CB_VAL_DIFF.BackColor = SystemColors.Control;
            CB_VAL_DIFF.Checked = true;
            CB_VAL_DIFF.CheckState = System.Windows.Forms.CheckState.Checked;
            CB_VAL_DIFF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CB_VAL_DIFF.Font = new Font("Segoe UI", 8.0f);
            CB_VAL_DIFF.Location = new Point(277, 8);
            CB_VAL_DIFF.Name = "CB_VAL_DIFF";
            CB_VAL_DIFF.Size = new Size(200, 17);
            CB_VAL_DIFF.TabIndex = 48;
            CB_VAL_DIFF.Text = "Detect Variances in Variable Values";
            CB_VAL_DIFF.UseVisualStyleBackColor = false;
            // 
            // CB_EXP_DIF
            // 
            CB_EXP_DIF.AutoSize = true;
            CB_EXP_DIF.Checked = true;
            CB_EXP_DIF.CheckState = System.Windows.Forms.CheckState.Checked;
            CB_EXP_DIF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CB_EXP_DIF.Font = new Font("Segoe UI", 8.0f);
            CB_EXP_DIF.Location = new Point(277, 26);
            CB_EXP_DIF.Name = "CB_EXP_DIF";
            CB_EXP_DIF.Size = new Size(225, 17);
            CB_EXP_DIF.TabIndex = 49;
            CB_EXP_DIF.Text = "Detect Variances in Variable Expansions";
            CB_EXP_DIF.UseVisualStyleBackColor = true;
            // 
            // SplitContainer2
            // 
            SplitContainer2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            SplitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            SplitContainer2.Location = new Point(0, -1);
            SplitContainer2.Name = "SplitContainer2";
            // 
            // SplitContainer2.Panel1
            // 
            SplitContainer2.Panel1.Controls.Add(SplitContainer4);
            // 
            // SplitContainer2.Panel2
            // 
            SplitContainer2.Panel2.Controls.Add(SplitContainer5);
            SplitContainer2.Size = new Size(974, 253);
            SplitContainer2.SplitterDistance = 449;
            SplitContainer2.TabIndex = 50;
            // 
            // SplitContainer4
            // 
            SplitContainer4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            SplitContainer4.IsSplitterFixed = true;
            SplitContainer4.Location = new Point(-2, 3);
            SplitContainer4.Name = "SplitContainer4";
            SplitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer4.Panel1
            // 
            SplitContainer4.Panel1.Controls.Add(RichTextBox2);
            SplitContainer4.Panel1.Controls.Add(Label2);
            // 
            // SplitContainer4.Panel2
            // 
            SplitContainer4.Panel2.Controls.Add(RichTextBox1);
            SplitContainer4.Panel2.Controls.Add(Label1);
            SplitContainer4.Size = new Size(451, 243);
            SplitContainer4.SplitterDistance = 125;
            SplitContainer4.TabIndex = 27;
            // 
            // SplitContainer5
            // 
            SplitContainer5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            SplitContainer5.IsSplitterFixed = true;
            SplitContainer5.Location = new Point(-5, 3);
            SplitContainer5.Name = "SplitContainer5";
            SplitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer5.Panel1
            // 
            SplitContainer5.Panel1.Controls.Add(RichTextBox3);
            SplitContainer5.Panel1.Controls.Add(Label6);
            // 
            // SplitContainer5.Panel2
            // 
            SplitContainer5.Panel2.Controls.Add(RichTextBox4);
            SplitContainer5.Panel2.Controls.Add(Label8);
            SplitContainer5.Size = new Size(524, 248);
            SplitContainer5.SplitterDistance = 125;
            SplitContainer5.TabIndex = 42;
            // 
            // SplitContainer3
            // 
            SplitContainer3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            SplitContainer3.Location = new Point(6, 50);
            SplitContainer3.Name = "SplitContainer3";
            SplitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer3.Panel1
            // 
            SplitContainer3.Panel1.Controls.Add(SplitContainer1);
            // 
            // SplitContainer3.Panel2
            // 
            SplitContainer3.Panel2.Controls.Add(SplitContainer2);
            SplitContainer3.Size = new Size(974, 500);
            SplitContainer3.SplitterDistance = 247;
            SplitContainer3.TabIndex = 51;
            // 
            // CheckBox1
            // 
            CheckBox1.AutoSize = true;
            CheckBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CheckBox1.Font = new Font("Segoe UI", 8.0f);
            CheckBox1.Location = new Point(551, 8);
            CheckBox1.Name = "CheckBox1";
            CheckBox1.Size = new Size(148, 17);
            CheckBox1.TabIndex = 52;
            CheckBox1.Text = "Hide Matching Variables";
            CheckBox1.UseVisualStyleBackColor = true;
            // 
            // CheckBox2
            // 
            CheckBox2.AutoSize = true;
            CheckBox2.Checked = true;
            CheckBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            CheckBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CheckBox2.Font = new Font("Segoe UI", 8.0f);
            CheckBox2.Location = new Point(551, 25);
            CheckBox2.Name = "CheckBox2";
            CheckBox2.Size = new Size(179, 17);
            CheckBox2.TabIndex = 53;
            CheckBox2.Text = "Highlight Undefined Variables";
            CheckBox2.UseVisualStyleBackColor = true;
            // 
            // RichTextBox5
            // 
            RichTextBox5.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            RichTextBox5.DetectUrls = false;
            RichTextBox5.Location = new Point(6, 556);
            RichTextBox5.Name = "RichTextBox5";
            RichTextBox5.ReadOnly = true;
            RichTextBox5.Size = new Size(974, 102);
            RichTextBox5.TabIndex = 42;
            RichTextBox5.Text = "";
            RichTextBox5.WordWrap = false;
            // 
            // CheckBox3
            // 
            CheckBox3.AutoSize = true;
            CheckBox3.Checked = true;
            CheckBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            CheckBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CheckBox3.Font = new Font("Segoe UI", 8.0f);
            CheckBox3.Location = new Point(751, 8);
            CheckBox3.Name = "CheckBox3";
            CheckBox3.Size = new Size(119, 17);
            CheckBox3.TabIndex = 54;
            CheckBox3.Text = "Hide Program Files";
            CheckBox3.UseVisualStyleBackColor = true;
            // 
            // VariableCompare
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(984, 666);
            Controls.Add(CheckBox3);
            Controls.Add(RichTextBox5);
            Controls.Add(SplitContainer3);
            Controls.Add(TextBox1);
            Controls.Add(CheckBox2);
            Controls.Add(CheckBox1);
            Controls.Add(Label4);
            Controls.Add(CB_EXP_DIF);
            Controls.Add(CB_VAL_DIFF);
            Controls.Add(Button1);
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 8.0f);
            KeyPreview = true;
            MinimumSize = new Size(800, 605);
            Name = "VariableCompare";
            ShowIcon = false;
            Text = "Workspace Variable Compare";
            ContextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)DataGridView1).EndInit();
            SplitContainer1.Panel1.ResumeLayout(false);
            SplitContainer1.Panel1.PerformLayout();
            SplitContainer1.Panel2.ResumeLayout(false);
            SplitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)SplitContainer1).EndInit();
            SplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)DataGridView2).EndInit();
            ContextMenuStrip2.ResumeLayout(false);
            SplitContainer2.Panel1.ResumeLayout(false);
            SplitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SplitContainer2).EndInit();
            SplitContainer2.ResumeLayout(false);
            SplitContainer4.Panel1.ResumeLayout(false);
            SplitContainer4.Panel1.PerformLayout();
            SplitContainer4.Panel2.ResumeLayout(false);
            SplitContainer4.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)SplitContainer4).EndInit();
            SplitContainer4.ResumeLayout(false);
            SplitContainer5.Panel1.ResumeLayout(false);
            SplitContainer5.Panel1.PerformLayout();
            SplitContainer5.Panel2.ResumeLayout(false);
            SplitContainer5.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)SplitContainer5).EndInit();
            SplitContainer5.ResumeLayout(false);
            SplitContainer3.Panel1.ResumeLayout(false);
            SplitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SplitContainer3).EndInit();
            SplitContainer3.ResumeLayout(false);
            FormClosing += new System.Windows.Forms.FormClosingEventHandler(VariableCompare_FormClosing);
            Resize += new EventHandler(VariableCompare_Resize);
            Shown += new EventHandler(VariableCompare_Shown);
            Load += new EventHandler(VariableCompare_Load);
            KeyDown += new System.Windows.Forms.KeyEventHandler(WatchF5_KeyDown);
            ResumeLayout(false);
            PerformLayout();

        }
        internal System.Windows.Forms.RichTextBox RichTextBox2;
        internal System.Windows.Forms.RichTextBox RichTextBox1;
        internal System.Windows.Forms.TextBox TextBox1;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Button Button1;
        internal System.Windows.Forms.DataGridView DataGridView1;
        internal System.Windows.Forms.Label LBwrk1Desc;
        internal System.Windows.Forms.SplitContainer SplitContainer1;
        internal System.Windows.Forms.DataGridView DataGridView2;
        internal System.Windows.Forms.Label LBwrk2Desc;
        internal System.Windows.Forms.RichTextBox RichTextBox4;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.RichTextBox RichTextBox3;
        internal System.Windows.Forms.CheckBox CB_VAL_DIFF;
        internal System.Windows.Forms.CheckBox CB_EXP_DIF;
        internal System.Windows.Forms.SplitContainer SplitContainer2;
        internal System.Windows.Forms.SplitContainer SplitContainer3;
        internal System.Windows.Forms.SplitContainer SplitContainer4;
        internal System.Windows.Forms.SplitContainer SplitContainer5;
        internal System.Windows.Forms.CheckBox CheckBox1;
        internal System.Windows.Forms.ContextMenuStrip ContextMenuStrip1;
        internal System.Windows.Forms.ToolStripMenuItem ExportToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ToExcelFileToolStripMenuItem;
        internal System.Windows.Forms.CheckBox CheckBox2;
        internal System.Windows.Forms.SaveFileDialog SaveFileDialog1;
        internal System.Windows.Forms.RichTextBox RichTextBox5;
        internal System.Windows.Forms.ToolStripMenuItem OpenVariableExplorerToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem OpenFileHistoryToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem OpenWorkspaceHistoryToolStripMenuItem;
        internal System.Windows.Forms.ContextMenuStrip ContextMenuStrip2;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem2;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem3;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem4;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem6;
        internal System.Windows.Forms.CheckBox CheckBox3;
    }
}
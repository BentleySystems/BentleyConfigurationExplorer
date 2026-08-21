using System;
using System.Diagnostics;
using System.Drawing;

namespace WorkspaceCFG
{
    public partial class VariableExplorer : System.Windows.Forms.Form
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
            dgVariables = new System.Windows.Forms.DataGridView();
            cmVariableCommands = new System.Windows.Forms.ContextMenuStrip(components);
            ExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToTextFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            OpenVariableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ViewLocationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            CopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            RichTextBox1 = new System.Windows.Forms.RichTextBox();
            tbNameFilter = new System.Windows.Forms.TextBox();
            RichTextBox2 = new System.Windows.Forms.RichTextBox();
            Label2 = new System.Windows.Forms.Label();
            Label1 = new System.Windows.Forms.Label();
            RichTextBox3 = new System.Windows.Forms.RichTextBox();
            Label3 = new System.Windows.Forms.Label();
            tbExpantionFilter = new System.Windows.Forms.TextBox();
            Label4 = new System.Windows.Forms.Label();
            Label5 = new System.Windows.Forms.Label();
            SplitContainer1 = new System.Windows.Forms.SplitContainer();
            DetailsPanel = new System.Windows.Forms.TableLayoutPanel();
            Label6 = new System.Windows.Forms.Label();
            cmColumnChooser = new System.Windows.Forms.ContextMenuStrip(components);
            ShowMultilineVariablesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            CatagoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            LevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            LengthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ExpansionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            Button1 = new System.Windows.Forms.Button();
            Button2 = new System.Windows.Forms.Button();
            MainLayout = new System.Windows.Forms.TableLayoutPanel();
            TopFilterPanel = new System.Windows.Forms.TableLayoutPanel();
            NameFilterFlow = new System.Windows.Forms.FlowLayoutPanel();
            ExpansionFilterFlow = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)dgVariables).BeginInit();
            cmVariableCommands.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SplitContainer1).BeginInit();
            SplitContainer1.Panel1.SuspendLayout();
            SplitContainer1.Panel2.SuspendLayout();
            SplitContainer1.SuspendLayout();
            DetailsPanel.SuspendLayout();
            cmColumnChooser.SuspendLayout();
            MainLayout.SuspendLayout();
            TopFilterPanel.SuspendLayout();
            NameFilterFlow.SuspendLayout();
            ExpansionFilterFlow.SuspendLayout();
            SuspendLayout();
            // 
            // dgVariables
            // 
            dgVariables.AllowUserToAddRows = false;
            dgVariables.AllowUserToDeleteRows = false;
            dgVariables.AllowUserToOrderColumns = true;
            dgVariables.AllowUserToResizeRows = false;
            dgVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            dgVariables.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dgVariables.BackgroundColor = Color.White;
            dgVariables.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dgVariables.CausesValidation = false;
            dgVariables.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dgVariables.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dgVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgVariables.ContextMenuStrip = cmVariableCommands;
            dgVariables.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            dgVariables.GridColor = Color.LightSteelBlue;
            dgVariables.MultiSelect = false;
            dgVariables.Name = "dgVariables";
            dgVariables.ReadOnly = true;
            dgVariables.RowHeadersVisible = false;
            dgVariables.RowHeadersWidth = 51;
            dgVariables.RowTemplate.DefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgVariables.RowTemplate.Height = 22;
            dgVariables.RowTemplate.ReadOnly = true;
            dgVariables.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            dgVariables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            dgVariables.ShowCellErrors = false;
            dgVariables.ShowEditingIcon = false;
            dgVariables.ShowRowErrors = false;
            dgVariables.TabIndex = 1;
            dgVariables.CellContentDoubleClick += DataGridView1_CellContentDoubleClick;
            dgVariables.CellMouseDown += DgVariables_CellMouseDown;
            dgVariables.CellToolTipTextNeeded += DgVariables_CellToolTipTextNeeded;
            dgVariables.SelectionChanged += DgVariables_SelectionChanged;
            dgVariables.KeyDown += DgVariables_KeyDown;
            dgVariables.KeyPress += DgVariables_KeyPress;
            dgVariables.MouseDown += DgVariables_MouseDown;
            // 
            // cmVariableCommands
            // 
            cmVariableCommands.Font = new Font("Segoe UI", 8F);
            cmVariableCommands.ImageScalingSize = new Size(20, 20);
            cmVariableCommands.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ExportToolStripMenuItem, OpenVariableToolStripMenuItem, ViewLocationsToolStripMenuItem, CopyToolStripMenuItem });
            cmVariableCommands.Name = "ContextMenuStrip1";
            cmVariableCommands.Size = new Size(185, 116);
            // 
            // ExportToolStripMenuItem
            // 
            ExportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ToExcelToolStripMenuItem, ToTextFileToolStripMenuItem });
            ExportToolStripMenuItem.Name = "ExportToolStripMenuItem";
            ExportToolStripMenuItem.Size = new Size(184, 28);
            ExportToolStripMenuItem.Text = "Export";
            // 
            // ToExcelToolStripMenuItem
            // 
            ToExcelToolStripMenuItem.Name = "ToExcelToolStripMenuItem";
            ToExcelToolStripMenuItem.Size = new Size(183, 34);
            ToExcelToolStripMenuItem.Text = "To Excel";
            ToExcelToolStripMenuItem.Click += ToExcelToolStripMenuItem_Click;
            // 
            // ToTextFileToolStripMenuItem
            // 
            ToTextFileToolStripMenuItem.Name = "ToTextFileToolStripMenuItem";
            ToTextFileToolStripMenuItem.Size = new Size(183, 34);
            ToTextFileToolStripMenuItem.Text = "To Text File";
            ToTextFileToolStripMenuItem.Click += ToTextFileToolStripMenuItem_Click;
            // 
            // OpenVariableToolStripMenuItem
            // 
            OpenVariableToolStripMenuItem.Name = "OpenVariableToolStripMenuItem";
            OpenVariableToolStripMenuItem.Size = new Size(184, 28);
            OpenVariableToolStripMenuItem.Text = "Open Variable";
            OpenVariableToolStripMenuItem.Click += OpenVariableToolStripMenuItem_Click;
            // 
            // ViewLocationsToolStripMenuItem
            // 
            ViewLocationsToolStripMenuItem.Name = "ViewLocationsToolStripMenuItem";
            ViewLocationsToolStripMenuItem.Size = new Size(184, 28);
            ViewLocationsToolStripMenuItem.Text = "View Locations";
            ViewLocationsToolStripMenuItem.Click += ViewLocationsToolStripMenuItem_Click;
            // 
            // CopyToolStripMenuItem
            // 
            CopyToolStripMenuItem.Name = "CopyToolStripMenuItem";
            CopyToolStripMenuItem.Size = new Size(184, 28);
            CopyToolStripMenuItem.Text = "Copy";
            CopyToolStripMenuItem.Click += CopyToolStripMenuItem_Click;
            // 
            // RichTextBox1
            // 
            RichTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            RichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            RichTextBox1.DetectUrls = false;
            RichTextBox1.Font = new Font("Segoe UI", 8F);
            RichTextBox1.Name = "RichTextBox1";
            RichTextBox1.ReadOnly = true;
            RichTextBox1.Size = new Size(774, 84);
            RichTextBox1.TabIndex = 3;
            RichTextBox1.Text = "";
            RichTextBox1.WordWrap = false;
            // 
            // tbNameFilter
            // 
            tbNameFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tbNameFilter.Font = new Font("Segoe UI", 8F);
            tbNameFilter.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            tbNameFilter.Name = "tbNameFilter";
            tbNameFilter.Size = new Size(225, 29);
            tbNameFilter.TabIndex = 4;
            tbNameFilter.TextChanged += TbNameFilter_TextChanged;
            // 
            // RichTextBox2
            // 
            RichTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            RichTextBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            RichTextBox2.DetectUrls = false;
            RichTextBox2.Font = new Font("Segoe UI", 8F);
            RichTextBox2.Name = "RichTextBox2";
            RichTextBox2.ReadOnly = true;
            RichTextBox2.Size = new Size(774, 84);
            RichTextBox2.TabIndex = 10;
            RichTextBox2.Text = "";
            RichTextBox2.WordWrap = false;
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.BackColor = Color.Transparent;
            Label2.Font = new Font("Segoe UI", 8F);
            Label2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            Label2.Name = "Label2";
            Label2.Size = new Size(108, 21);
            Label2.TabIndex = 12;
            Label2.Text = "Current Value:";
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.BackColor = Color.Transparent;
            Label1.Font = new Font("Segoe UI", 8F);
            Label1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            Label1.Name = "Label1";
            Label1.Size = new Size(140, 21);
            Label1.TabIndex = 13;
            Label1.Text = "Current Expansion:";
            // 
            // RichTextBox3
            // 
            RichTextBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            RichTextBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            RichTextBox3.Font = new Font("Segoe UI", 8F);
            RichTextBox3.Name = "RichTextBox3";
            RichTextBox3.ReadOnly = true;
            RichTextBox3.Size = new Size(1034, 84);
            RichTextBox3.TabIndex = 14;
            RichTextBox3.Text = "";
            // 
            // Label3
            // 
            Label3.AutoSize = true;
            Label3.BackColor = Color.Transparent;
            Label3.Font = new Font("Segoe UI", 8F);
            Label3.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            Label3.Name = "Label3";
            Label3.Size = new Size(92, 21);
            Label3.TabIndex = 15;
            Label3.Text = "Description:";
            // 
            // tbExpantionFilter
            // 
            tbExpantionFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tbExpantionFilter.Font = new Font("Segoe UI", 8F);
            tbExpantionFilter.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            tbExpantionFilter.Name = "tbExpantionFilter";
            tbExpantionFilter.Size = new Size(229, 29);
            tbExpantionFilter.TabIndex = 16;
            tbExpantionFilter.TextChanged += TbExpantionFilter_TextChanged;
            // 
            // Label4
            // 
            Label4.AutoSize = true;
            Label4.BackColor = Color.Transparent;
            Label4.Font = new Font("Segoe UI", 8F);
            Label4.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            Label4.Name = "Label4";
            Label4.Size = new Size(94, 21);
            Label4.TabIndex = 18;
            Label4.Text = "Name Filter:";
            // 
            // Label5
            // 
            Label5.AutoSize = true;
            Label5.BackColor = Color.Transparent;
            Label5.Font = new Font("Segoe UI", 8F);
            Label5.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            Label5.Name = "Label5";
            Label5.Size = new Size(122, 21);
            Label5.TabIndex = 19;
            Label5.Text = "Expansion Filter:";
            // 
            // SplitContainer1
            // 
            SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            SplitContainer1.Name = "SplitContainer1";
            SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer1.Panel1
            // 
            SplitContainer1.Panel1.Controls.Add(dgVariables);
            // 
            // SplitContainer1.Panel2
            // 
            SplitContainer1.Panel2.Controls.Add(DetailsPanel);
            SplitContainer1.Size = new Size(1300, 943);
            SplitContainer1.SplitterDistance = 292;
            SplitContainer1.TabIndex = 20;
            // 
            // DetailsPanel
            // 
            DetailsPanel.ColumnCount = 1;
            DetailsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            DetailsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            DetailsPanel.RowCount = 6;
            DetailsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            DetailsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            DetailsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            DetailsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            DetailsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            DetailsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            DetailsPanel.Controls.Add(Label1, 0, 0);
            DetailsPanel.Controls.Add(RichTextBox1, 0, 1);
            DetailsPanel.Controls.Add(Label2, 0, 2);
            DetailsPanel.Controls.Add(RichTextBox2, 0, 3);
            DetailsPanel.Controls.Add(Label3, 0, 4);
            DetailsPanel.Controls.Add(RichTextBox3, 0, 5);
            DetailsPanel.Name = "DetailsPanel";
            DetailsPanel.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            DetailsPanel.Size = new Size(1034, 649);
            DetailsPanel.TabIndex = 21;
            // 
            // Label6
            // 
            Label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            Label6.AutoSize = true;
            Label6.BackColor = Color.Transparent;
            Label6.Dock = System.Windows.Forms.DockStyle.Right;
            Label6.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Label6.Name = "Label6";
            Label6.Size = new Size(103, 29);
            Label6.TabIndex = 21;
            Label6.Text = "Variables";
            Label6.TextAlign = ContentAlignment.BottomRight;
            // 
            // cmColumnChooser
            // 
            cmColumnChooser.Font = new Font("Segoe UI", 8F);
            cmColumnChooser.ImageScalingSize = new Size(20, 20);
            cmColumnChooser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ShowMultilineVariablesToolStripMenuItem, CatagoryToolStripMenuItem, ApplicationToolStripMenuItem, LevelToolStripMenuItem, LengthToolStripMenuItem, ValueToolStripMenuItem, ExpansionToolStripMenuItem });
            cmColumnChooser.Name = "ContextMenuStrip1";
            cmColumnChooser.Size = new Size(252, 228);
            // 
            // ShowMultilineVariablesToolStripMenuItem
            // 
            ShowMultilineVariablesToolStripMenuItem.CheckOnClick = true;
            ShowMultilineVariablesToolStripMenuItem.Name = "ShowMultilineVariablesToolStripMenuItem";
            ShowMultilineVariablesToolStripMenuItem.Size = new Size(251, 32);
            ShowMultilineVariablesToolStripMenuItem.Text = "Show Multiline Variables";
            ShowMultilineVariablesToolStripMenuItem.CheckedChanged += ShowMultilineVariablesToolStripMenuItem_CheckedChanged;
            // 
            // CatagoryToolStripMenuItem
            // 
            CatagoryToolStripMenuItem.Checked = true;
            CatagoryToolStripMenuItem.CheckOnClick = true;
            CatagoryToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            CatagoryToolStripMenuItem.Name = "CatagoryToolStripMenuItem";
            CatagoryToolStripMenuItem.Size = new Size(251, 32);
            CatagoryToolStripMenuItem.Text = "Category";
            CatagoryToolStripMenuItem.CheckedChanged += CatagoryToolStripMenuItem_CheckedChanged;
            // 
            // ApplicationToolStripMenuItem
            // 
            ApplicationToolStripMenuItem.Checked = true;
            ApplicationToolStripMenuItem.CheckOnClick = true;
            ApplicationToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            ApplicationToolStripMenuItem.Name = "ApplicationToolStripMenuItem";
            ApplicationToolStripMenuItem.Size = new Size(251, 32);
            ApplicationToolStripMenuItem.Text = "Application";
            ApplicationToolStripMenuItem.CheckedChanged += ApplicationToolStripMenuItem_CheckedChanged;
            // 
            // LevelToolStripMenuItem
            // 
            LevelToolStripMenuItem.Checked = true;
            LevelToolStripMenuItem.CheckOnClick = true;
            LevelToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            LevelToolStripMenuItem.Name = "LevelToolStripMenuItem";
            LevelToolStripMenuItem.Size = new Size(251, 32);
            LevelToolStripMenuItem.Text = "Level";
            LevelToolStripMenuItem.CheckedChanged += LevelToolStripMenuItem_CheckedChanged;
            // 
            // LengthToolStripMenuItem
            // 
            LengthToolStripMenuItem.Checked = true;
            LengthToolStripMenuItem.CheckOnClick = true;
            LengthToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            LengthToolStripMenuItem.Name = "LengthToolStripMenuItem";
            LengthToolStripMenuItem.Size = new Size(251, 32);
            LengthToolStripMenuItem.Text = "Length";
            LengthToolStripMenuItem.CheckedChanged += LengthToolStripMenuItem_CheckedChanged;
            // 
            // ValueToolStripMenuItem
            // 
            ValueToolStripMenuItem.Checked = true;
            ValueToolStripMenuItem.CheckOnClick = true;
            ValueToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            ValueToolStripMenuItem.Name = "ValueToolStripMenuItem";
            ValueToolStripMenuItem.Size = new Size(251, 32);
            ValueToolStripMenuItem.Text = "Value";
            ValueToolStripMenuItem.CheckedChanged += ValueToolStripMenuItem_CheckedChanged;
            // 
            // ExpansionToolStripMenuItem
            // 
            ExpansionToolStripMenuItem.Checked = true;
            ExpansionToolStripMenuItem.CheckOnClick = true;
            ExpansionToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            ExpansionToolStripMenuItem.Name = "ExpansionToolStripMenuItem";
            ExpansionToolStripMenuItem.Size = new Size(251, 32);
            ExpansionToolStripMenuItem.Text = "Expansion:";
            ExpansionToolStripMenuItem.CheckedChanged += ExpansionToolStripMenuItem_CheckedChanged;
            // 
            // Button1
            // 
            Button1.AutoSize = true;
            Button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Button1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            Button1.MinimumSize = new Size(75, 29);
            Button1.Name = "Button1";
            Button1.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            Button1.TabIndex = 22;
            Button1.Text = "Clear";
            Button1.UseVisualStyleBackColor = true;
            Button1.Click += Button1_Click;
            // 
            // Button2
            // 
            Button2.AutoSize = true;
            Button2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Button2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            Button2.MinimumSize = new Size(75, 29);
            Button2.Name = "Button2";
            Button2.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            Button2.TabIndex = 23;
            Button2.Text = "Clear";
            Button2.UseVisualStyleBackColor = true;
            Button2.Click += Button2_Click;
            // 
            // NameFilterFlow
            // 
            NameFilterFlow.AutoSize = true;
            NameFilterFlow.Controls.Add(tbNameFilter);
            NameFilterFlow.Controls.Add(Button1);
            NameFilterFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            NameFilterFlow.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            NameFilterFlow.Margin = new System.Windows.Forms.Padding(0);
            NameFilterFlow.Name = "NameFilterFlow";
            NameFilterFlow.Size = new Size(303, 29);
            NameFilterFlow.TabIndex = 24;
            NameFilterFlow.WrapContents = false;
            // 
            // ExpansionFilterFlow
            // 
            ExpansionFilterFlow.AutoSize = true;
            ExpansionFilterFlow.Controls.Add(tbExpantionFilter);
            ExpansionFilterFlow.Controls.Add(Button2);
            ExpansionFilterFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            ExpansionFilterFlow.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            ExpansionFilterFlow.Margin = new System.Windows.Forms.Padding(0);
            ExpansionFilterFlow.Name = "ExpansionFilterFlow";
            ExpansionFilterFlow.Size = new Size(307, 29);
            ExpansionFilterFlow.TabIndex = 25;
            ExpansionFilterFlow.WrapContents = false;
            // 
            // TopFilterPanel
            // 
            TopFilterPanel.AutoSize = true;
            TopFilterPanel.ColumnCount = 3;
            TopFilterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            TopFilterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            TopFilterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            TopFilterPanel.Controls.Add(Label4, 0, 0);
            TopFilterPanel.Controls.Add(Label5, 1, 0);
            TopFilterPanel.Controls.Add(NameFilterFlow, 0, 1);
            TopFilterPanel.Controls.Add(ExpansionFilterFlow, 1, 1);
            TopFilterPanel.Controls.Add(Label6, 2, 1);
            TopFilterPanel.Dock = System.Windows.Forms.DockStyle.Top;
            TopFilterPanel.Name = "TopFilterPanel";
            TopFilterPanel.Padding = new System.Windows.Forms.Padding(12, 9, 12, 3);
            TopFilterPanel.RowCount = 2;
            TopFilterPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            TopFilterPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            TopFilterPanel.Size = new Size(1324, 71);
            TopFilterPanel.TabIndex = 26;
            // 
            // MainLayout
            // 
            MainLayout.ColumnCount = 1;
            MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            MainLayout.Controls.Add(TopFilterPanel, 0, 0);
            MainLayout.Controls.Add(SplitContainer1, 0, 1);
            MainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            MainLayout.Name = "MainLayout";
            MainLayout.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            MainLayout.RowCount = 2;
            MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            MainLayout.Size = new Size(1324, 1014);
            MainLayout.TabIndex = 27;
            // 
            // VariableExplorer
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(1721, 1318);
            Controls.Add(MainLayout);
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 8F);
            KeyPreview = true;
            MinimumSize = new Size(1040, 780);
            Name = "VariableExplorer";
            ShowIcon = false;
            Text = "Variable Explorer";
            FormClosing += VariableExplorer_FormClosing;
            Load += VariableExplorer_Load;
            KeyDown += WatchF5_KeyDown;
            ((System.ComponentModel.ISupportInitialize)dgVariables).EndInit();
            cmVariableCommands.ResumeLayout(false);
            SplitContainer1.Panel1.ResumeLayout(false);
            SplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SplitContainer1).EndInit();
            SplitContainer1.ResumeLayout(false);
            DetailsPanel.ResumeLayout(false);
            DetailsPanel.PerformLayout();
            cmColumnChooser.ResumeLayout(false);
            MainLayout.ResumeLayout(false);
            MainLayout.PerformLayout();
            TopFilterPanel.ResumeLayout(false);
            TopFilterPanel.PerformLayout();
            NameFilterFlow.ResumeLayout(false);
            NameFilterFlow.PerformLayout();
            ExpansionFilterFlow.ResumeLayout(false);
            ExpansionFilterFlow.PerformLayout();
            ResumeLayout(false);

        }
        internal System.Windows.Forms.DataGridView dgVariables;
        internal System.Windows.Forms.RichTextBox RichTextBox1;
        internal System.Windows.Forms.TextBox tbNameFilter;
        internal System.Windows.Forms.RichTextBox RichTextBox2;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.RichTextBox RichTextBox3;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.TextBox tbExpantionFilter;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.SplitContainer SplitContainer1;
        internal System.Windows.Forms.TableLayoutPanel DetailsPanel;
        internal System.Windows.Forms.ContextMenuStrip cmVariableCommands;
        internal System.Windows.Forms.ToolStripMenuItem ExportToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ToExcelToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ToTextFileToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem OpenVariableToolStripMenuItem;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.ToolStripMenuItem ViewLocationsToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem CopyToolStripMenuItem;
        internal System.Windows.Forms.ContextMenuStrip cmColumnChooser;
        internal System.Windows.Forms.ToolStripMenuItem LevelToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem LengthToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem CatagoryToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ValueToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ExpansionToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ApplicationToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ShowMultilineVariablesToolStripMenuItem;
        internal System.Windows.Forms.Button Button1;
        internal System.Windows.Forms.Button Button2;
        internal System.Windows.Forms.TableLayoutPanel MainLayout;
        internal System.Windows.Forms.TableLayoutPanel TopFilterPanel;
        internal System.Windows.Forms.FlowLayoutPanel NameFilterFlow;
        internal System.Windows.Forms.FlowLayoutPanel ExpansionFilterFlow;
    }
}
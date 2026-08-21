using System;
using System.Diagnostics;
using System.Drawing;

namespace WorkspaceCFG
{
    public partial class VariableGenerator : System.Windows.Forms.Form
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
            dgSolutions = new System.Windows.Forms.DataGridView();
            ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            CopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            CopyToolStripMenuItem.Click += new EventHandler(CopyToolStripMenuItem_Click);
            CopyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            CopyToolStripMenuItem1.Click += new EventHandler(CopyToolStripMenuItem1_Click);
            Button1 = new System.Windows.Forms.Button();
            Button1.Click += new EventHandler(Button1_Click);
            TextBox1 = new System.Windows.Forms.TextBox();
            TextBox1.TextChanged += new EventHandler(TextBox1_TextChanged);
            Label4 = new System.Windows.Forms.Label();
            Label5 = new System.Windows.Forms.Label();
            Label6 = new System.Windows.Forms.Label();
            NumericUpDown1 = new System.Windows.Forms.NumericUpDown();
            Label1 = new System.Windows.Forms.Label();
            Label2 = new System.Windows.Forms.Label();
            NumericUpDown2 = new System.Windows.Forms.NumericUpDown();
            Label3 = new System.Windows.Forms.Label();
            RootLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            TitleRowPanel = new System.Windows.Forms.TableLayoutPanel();
            GenerateRowPanel = new System.Windows.Forms.TableLayoutPanel();
            BacktrackRowPanel = new System.Windows.Forms.TableLayoutPanel();
            MinLengthRowPanel = new System.Windows.Forms.TableLayoutPanel();
            ResultsRowPanel = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)dgSolutions).BeginInit();
            ContextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown2).BeginInit();
            RootLayoutPanel.SuspendLayout();
            TitleRowPanel.SuspendLayout();
            GenerateRowPanel.SuspendLayout();
            BacktrackRowPanel.SuspendLayout();
            MinLengthRowPanel.SuspendLayout();
            ResultsRowPanel.SuspendLayout();
            SuspendLayout();
            // 
            // dgSolutions
            // 
            RootLayoutPanel.SetRow(dgSolutions, 5);
            dgSolutions.AllowUserToAddRows = false;
            dgSolutions.AllowUserToDeleteRows = false;
            dgSolutions.AllowUserToOrderColumns = true;
            dgSolutions.Dock = System.Windows.Forms.DockStyle.Fill;
            dgSolutions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgSolutions.BackgroundColor = Color.White;
            dgSolutions.CausesValidation = false;
            dgSolutions.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dgSolutions.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dgSolutions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgSolutions.ContextMenuStrip = ContextMenuStrip1;
            dgSolutions.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            dgSolutions.GridColor = Color.LightSteelBlue;
            dgSolutions.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            dgSolutions.Name = "dgSolutions";
            dgSolutions.RowHeadersVisible = false;
            dgSolutions.RowTemplate.DefaultCellStyle.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgSolutions.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dgSolutions.RowTemplate.Height = 22;
            dgSolutions.RowTemplate.ReadOnly = true;
            dgSolutions.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            dgSolutions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            dgSolutions.ShowRowErrors = false;
            dgSolutions.TabIndex = 4;
            dgSolutions.VirtualMode = true;
            // 
            // ContextMenuStrip1
            // 
            ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { CopyToolStripMenuItem, CopyToolStripMenuItem1 });
            ContextMenuStrip1.Name = "ContextMenuStrip1";
            ContextMenuStrip1.Size = new Size(111, 48);
            // 
            // CopyToolStripMenuItem
            // 
            CopyToolStripMenuItem.Name = "CopyToolStripMenuItem";
            CopyToolStripMenuItem.Size = new Size(110, 22);
            CopyToolStripMenuItem.Text = "Copy /";
            // 
            // CopyToolStripMenuItem1
            // 
            CopyToolStripMenuItem1.Name = "CopyToolStripMenuItem1";
            CopyToolStripMenuItem1.Size = new Size(110, 22);
            CopyToolStripMenuItem1.Text = @"Copy \";
            // 
            // Button1
            // 
            GenerateRowPanel.SetColumn(Button1, 1);
            Button1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            Button1.Font = new Font("Segoe UI", 8.0f);
            Button1.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            Button1.Name = "Button1";
            Button1.Size = new Size(91, 22);
            Button1.TabIndex = 5;
            Button1.Text = "Generate";
            Button1.UseVisualStyleBackColor = true;
            // 
            // TextBox1
            // 
            GenerateRowPanel.SetColumn(TextBox1, 0);
            TextBox1.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            TextBox1.Margin = new System.Windows.Forms.Padding(0);
            TextBox1.Name = "TextBox1";
            TextBox1.TabIndex = 22;
            // 
            // Label4
            // 
            TitleRowPanel.SetColumn(Label4, 0);
            Label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            Label4.AutoSize = true;
            Label4.BackColor = Color.Transparent;
            Label4.Font = new Font("Segoe UI", 8.0f, FontStyle.Bold, GraphicsUnit.Point, 0);
            Label4.Name = "Label4";
            Label4.TabIndex = 24;
            Label4.Text = "Desired Value:";
            // 
            // Label5
            // 
            ResultsRowPanel.SetColumn(Label5, 0);
            Label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            Label5.AutoSize = true;
            Label5.BackColor = Color.Transparent;
            Label5.Font = new Font("Segoe UI", 8.0f);
            Label5.Name = "Label5";
            Label5.TabIndex = 44;
            Label5.Text = "Results";
            // 
            // Label6
            // 
            ResultsRowPanel.SetColumn(Label6, 1);
            Label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            Label6.BackColor = Color.Transparent;
            Label6.Dock = System.Windows.Forms.DockStyle.Fill;
            Label6.Font = new Font("Segoe UI", 8.0f);
            Label6.Name = "Label6";
            Label6.TabIndex = 45;
            Label6.Text = "Results";
            Label6.TextAlign = ContentAlignment.BottomRight;
            // 
            // NumericUpDown1
            // 
            BacktrackRowPanel.SetColumn(NumericUpDown1, 1);
            NumericUpDown1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            NumericUpDown1.Name = "NumericUpDown1";
            NumericUpDown1.Size = new Size(42, 22);
            NumericUpDown1.TabIndex = 46;
            // 
            // Label1
            // 
            BacktrackRowPanel.SetColumn(Label1, 0);
            Label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            Label1.AutoSize = true;
            Label1.BackColor = Color.Transparent;
            Label1.Font = new Font("Segoe UI", 8.0f);
            Label1.Name = "Label1";
            Label1.TabIndex = 47;
            Label1.Text = "Max Path Backtrack:";
            // 
            // Label2
            // 
            MinLengthRowPanel.SetColumn(Label2, 0);
            Label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            Label2.AutoSize = true;
            Label2.BackColor = Color.Transparent;
            Label2.Font = new Font("Segoe UI", 8.0f);
            Label2.Name = "Label2";
            Label2.TabIndex = 49;
            Label2.Text = "Min Variable Length:";
            // 
            // NumericUpDown2
            // 
            MinLengthRowPanel.SetColumn(NumericUpDown2, 1);
            NumericUpDown2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            NumericUpDown2.Name = "NumericUpDown2";
            NumericUpDown2.Size = new Size(42, 22);
            NumericUpDown2.TabIndex = 48;
            NumericUpDown2.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // Label3
            // 
            TitleRowPanel.SetColumn(Label3, 1);
            Label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            Label3.AutoSize = true;
            Label3.BackColor = Color.Transparent;
            Label3.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            Label3.Name = "Label3";
            Label3.TabIndex = 50;
            Label3.Text = "-";
            // 
            // TitleRowPanel
            // 
            TitleRowPanel.AutoSize = true;
            TitleRowPanel.ColumnCount = 2;
            TitleRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            TitleRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            TitleRowPanel.Controls.Add(Label4);
            TitleRowPanel.Controls.Add(Label3);
            TitleRowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            TitleRowPanel.Margin = new System.Windows.Forms.Padding(0);
            TitleRowPanel.Name = "TitleRowPanel";
            TitleRowPanel.RowCount = 1;
            TitleRowPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            TitleRowPanel.TabIndex = 51;
            // 
            // GenerateRowPanel
            // 
            RootLayoutPanel.SetRow(GenerateRowPanel, 1);
            GenerateRowPanel.AutoSize = true;
            GenerateRowPanel.ColumnCount = 2;
            GenerateRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
            GenerateRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            GenerateRowPanel.Controls.Add(TextBox1);
            GenerateRowPanel.Controls.Add(Button1);
            GenerateRowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            GenerateRowPanel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            GenerateRowPanel.Name = "GenerateRowPanel";
            GenerateRowPanel.RowCount = 1;
            GenerateRowPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            GenerateRowPanel.TabIndex = 52;
            // 
            // BacktrackRowPanel
            // 
            RootLayoutPanel.SetRow(BacktrackRowPanel, 2);
            BacktrackRowPanel.AutoSize = true;
            BacktrackRowPanel.ColumnCount = 2;
            BacktrackRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            BacktrackRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            BacktrackRowPanel.Controls.Add(Label1);
            BacktrackRowPanel.Controls.Add(NumericUpDown1);
            BacktrackRowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            BacktrackRowPanel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            BacktrackRowPanel.Name = "BacktrackRowPanel";
            BacktrackRowPanel.RowCount = 1;
            BacktrackRowPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            BacktrackRowPanel.TabIndex = 53;
            // 
            // MinLengthRowPanel
            // 
            RootLayoutPanel.SetRow(MinLengthRowPanel, 3);
            MinLengthRowPanel.AutoSize = true;
            MinLengthRowPanel.ColumnCount = 2;
            MinLengthRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            MinLengthRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            MinLengthRowPanel.Controls.Add(Label2);
            MinLengthRowPanel.Controls.Add(NumericUpDown2);
            MinLengthRowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            MinLengthRowPanel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            MinLengthRowPanel.Name = "MinLengthRowPanel";
            MinLengthRowPanel.RowCount = 1;
            MinLengthRowPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            MinLengthRowPanel.TabIndex = 54;
            // 
            // ResultsRowPanel
            // 
            RootLayoutPanel.SetRow(ResultsRowPanel, 4);
            ResultsRowPanel.ColumnCount = 2;
            ResultsRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            ResultsRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
            ResultsRowPanel.Controls.Add(Label5);
            ResultsRowPanel.Controls.Add(Label6);
            ResultsRowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            ResultsRowPanel.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            ResultsRowPanel.Name = "ResultsRowPanel";
            ResultsRowPanel.RowCount = 1;
            ResultsRowPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            ResultsRowPanel.TabIndex = 55;
            // 
            // RootLayoutPanel
            // 
            RootLayoutPanel.ColumnCount = 1;
            RootLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
            RootLayoutPanel.Controls.Add(TitleRowPanel, 0, 0);
            RootLayoutPanel.Controls.Add(GenerateRowPanel, 0, 1);
            RootLayoutPanel.Controls.Add(BacktrackRowPanel, 0, 2);
            RootLayoutPanel.Controls.Add(MinLengthRowPanel, 0, 3);
            RootLayoutPanel.Controls.Add(ResultsRowPanel, 0, 4);
            RootLayoutPanel.Controls.Add(dgSolutions, 0, 5);
            RootLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            RootLayoutPanel.Name = "RootLayoutPanel";
            RootLayoutPanel.Padding = new System.Windows.Forms.Padding(10, 9, 10, 12);
            RootLayoutPanel.RowCount = 6;
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
            RootLayoutPanel.TabIndex = 56;
            // 
            // VariableGenerator
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(784, 468);
            Controls.Add(RootLayoutPanel);
            Font = new Font("Segoe UI", 8.0f);
            KeyPreview = true;
            MinimumSize = new Size(800, 400);
            Name = "VariableGenerator";
            ShowIcon = false;
            Text = "Variable Generator";
            ((System.ComponentModel.ISupportInitialize)dgSolutions).EndInit();
            ContextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)NumericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown2).EndInit();
            RootLayoutPanel.ResumeLayout(false);
            TitleRowPanel.ResumeLayout(false);
            TitleRowPanel.PerformLayout();
            GenerateRowPanel.ResumeLayout(false);
            GenerateRowPanel.PerformLayout();
            BacktrackRowPanel.ResumeLayout(false);
            BacktrackRowPanel.PerformLayout();
            MinLengthRowPanel.ResumeLayout(false);
            MinLengthRowPanel.PerformLayout();
            ResultsRowPanel.ResumeLayout(false);
            ResultsRowPanel.PerformLayout();
            FormClosing += new System.Windows.Forms.FormClosingEventHandler(VariableGenerator_FormClosing);
            Shown += new EventHandler(VariableGenerator_Shown);
            Load += new EventHandler(VariableGenerator_Load);
            KeyDown += new System.Windows.Forms.KeyEventHandler(WatchF5_KeyDown);
            ResumeLayout(false);

        }
        internal System.Windows.Forms.DataGridView dgSolutions;
        internal System.Windows.Forms.Button Button1;
        internal System.Windows.Forms.TextBox TextBox1;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.NumericUpDown NumericUpDown1;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.NumericUpDown NumericUpDown2;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.ContextMenuStrip ContextMenuStrip1;
        internal System.Windows.Forms.ToolStripMenuItem CopyToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem CopyToolStripMenuItem1;
        internal System.Windows.Forms.TableLayoutPanel RootLayoutPanel;
        internal System.Windows.Forms.TableLayoutPanel TitleRowPanel;
        internal System.Windows.Forms.TableLayoutPanel GenerateRowPanel;
        internal System.Windows.Forms.TableLayoutPanel BacktrackRowPanel;
        internal System.Windows.Forms.TableLayoutPanel MinLengthRowPanel;
        internal System.Windows.Forms.TableLayoutPanel ResultsRowPanel;
    }
}
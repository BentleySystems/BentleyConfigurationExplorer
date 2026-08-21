using System;
using System.Diagnostics;
using System.Drawing;

namespace WorkspaceCFG
{
    public partial class Search : System.Windows.Forms.Form
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
            DataGridView1 = new System.Windows.Forms.DataGridView();
            DataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(DataGridView1_CellDoubleClick);
            ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            ExportToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ExportToExcelToolStripMenuItem.Click += new EventHandler(ExportToExcelToolStripMenuItem_Click);
            TextBox1 = new System.Windows.Forms.TextBox();
            TextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(TextBox1_KeyPress);
            Label4 = new System.Windows.Forms.Label();
            Label6 = new System.Windows.Forms.Label();
            Button2 = new System.Windows.Forms.Button();
            Button2.Click += new EventHandler(Button2_Click);
            Button1 = new System.Windows.Forms.Button();
            Button1.Click += new EventHandler(Button1_Click);
            CheckBox4 = new System.Windows.Forms.CheckBox();
            CheckBox4.CheckedChanged += new EventHandler(CheckBox4_CheckedChanged);
            SaveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            RootLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            HeaderRowPanel = new System.Windows.Forms.TableLayoutPanel();
            HeaderControlsFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)DataGridView1).BeginInit();
            ContextMenuStrip1.SuspendLayout();
            RootLayoutPanel.SuspendLayout();
            HeaderRowPanel.SuspendLayout();
            HeaderControlsFlowPanel.SuspendLayout();
            SuspendLayout();
            // 
            // DataGridView1
            // 
            DataGridView1.AllowUserToAddRows = false;
            DataGridView1.AllowUserToDeleteRows = false;
            DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            DataGridView1.BackgroundColor = Color.White;
            DataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            DataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridView1.ContextMenuStrip = ContextMenuStrip1;
            DataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            DataGridView1.GridColor = Color.LightSteelBlue;
            DataGridView1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            DataGridView1.Name = "DataGridView1";
            DataGridView1.RowHeadersVisible = false;
            DataGridView1.RowTemplate.DefaultCellStyle.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            DataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            DataGridView1.RowTemplate.Height = 22;
            DataGridView1.RowTemplate.ReadOnly = true;
            DataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            DataGridView1.ShowRowErrors = false;
            DataGridView1.TabIndex = 43;
            // 
            // ContextMenuStrip1
            // 
            ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ExportToExcelToolStripMenuItem });
            ContextMenuStrip1.Name = "ContextMenuStrip1";
            ContextMenuStrip1.Size = new Size(153, 26);
            // 
            // ExportToExcelToolStripMenuItem
            // 
            ExportToExcelToolStripMenuItem.Name = "ExportToExcelToolStripMenuItem";
            ExportToExcelToolStripMenuItem.Size = new Size(152, 22);
            ExportToExcelToolStripMenuItem.Text = "Export to Excel";
            // 
            // TextBox1
            // 
            TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            TextBox1.Font = new Font("Segoe UI", 8.0f);
            TextBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            TextBox1.Name = "TextBox1";
            TextBox1.Size = new Size(254, 22);
            TextBox1.TabIndex = 46;
            // 
            // Label4
            // 
            Label4.AutoSize = true;
            Label4.BackColor = Color.Transparent;
            Label4.Font = new Font("Segoe UI", 8.0f);
            Label4.Margin = new System.Windows.Forms.Padding(0, 6, 4, 0);
            Label4.Name = "Label4";
            Label4.TabIndex = 48;
            Label4.Text = "Search:";
            // 
            // Label6
            // 
            Label6.Dock = System.Windows.Forms.DockStyle.Fill;
            Label6.BackColor = Color.Transparent;
            Label6.Font = new Font("Segoe UI", 8.0f);
            Label6.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            Label6.Name = "Label6";
            Label6.TabIndex = 45;
            Label6.Text = "Results";
            Label6.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Button2
            // 
            Button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            Button2.Font = new Font("Segoe UI", 8.0f);
            Button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Button2.Name = "Button2";
            Button2.Size = new Size(72, 22);
            Button2.TabIndex = 49;
            Button2.Text = "Find It";
            Button2.UseVisualStyleBackColor = true;
            // 
            // Button1
            // 
            Button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            Button1.Font = new Font("Segoe UI", 8.0f);
            Button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Button1.Name = "Button1";
            Button1.Size = new Size(91, 22);
            Button1.TabIndex = 50;
            Button1.Text = "Clear Results";
            Button1.UseVisualStyleBackColor = true;
            // 
            // CheckBox4
            // 
            CheckBox4.AutoSize = true;
            CheckBox4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CheckBox4.Font = new Font("Segoe UI", 8.0f);
            CheckBox4.Margin = new System.Windows.Forms.Padding(4, 8, 4, 5);
            CheckBox4.Name = "CheckBox4";
            CheckBox4.Size = new Size(203, 17);
            CheckBox4.TabIndex = 51;
            CheckBox4.Text = "Include results from variable library";
            CheckBox4.UseVisualStyleBackColor = true;
            // 
            // RootLayoutPanel
            // 
            RootLayoutPanel.ColumnCount = 1;
            RootLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            RootLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            RootLayoutPanel.Padding = new System.Windows.Forms.Padding(12, 8, 12, 11);
            RootLayoutPanel.RowCount = 2;
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            RootLayoutPanel.Controls.Add(HeaderRowPanel, 0, 0);
            RootLayoutPanel.Controls.Add(DataGridView1, 0, 1);
            RootLayoutPanel.Name = "RootLayoutPanel";
            // 
            // HeaderRowPanel
            // 
            HeaderRowPanel.AutoSize = true;
            HeaderRowPanel.ColumnCount = 2;
            HeaderRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            HeaderRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            HeaderRowPanel.Controls.Add(HeaderControlsFlowPanel, 0, 0);
            HeaderRowPanel.Controls.Add(Label6, 1, 0);
            HeaderRowPanel.Dock = System.Windows.Forms.DockStyle.Top;
            HeaderRowPanel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            HeaderRowPanel.Name = "HeaderRowPanel";
            HeaderRowPanel.RowCount = 1;
            HeaderRowPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // HeaderControlsFlowPanel
            // 
            HeaderControlsFlowPanel.AutoSize = true;
            HeaderControlsFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            HeaderControlsFlowPanel.WrapContents = false;
            HeaderControlsFlowPanel.Margin = new System.Windows.Forms.Padding(0);
            HeaderControlsFlowPanel.Controls.Add(Label4);
            HeaderControlsFlowPanel.Controls.Add(TextBox1);
            HeaderControlsFlowPanel.Controls.Add(Button2);
            HeaderControlsFlowPanel.Controls.Add(Button1);
            HeaderControlsFlowPanel.Controls.Add(CheckBox4);
            HeaderControlsFlowPanel.Name = "HeaderControlsFlowPanel";
            // 
            // Search
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(784, 361);
            Controls.Add(RootLayoutPanel);
            Font = new Font("Segoe UI", 8.0f);
            KeyPreview = true;
            MinimumSize = new Size(800, 200);
            Name = "Search";
            ShowIcon = false;
            Text = "Search";
            ((System.ComponentModel.ISupportInitialize)DataGridView1).EndInit();
            ContextMenuStrip1.ResumeLayout(false);
            FormClosing += new System.Windows.Forms.FormClosingEventHandler(Search_FormClosing);
            Load += new EventHandler(Search_Load);
            KeyDown += new System.Windows.Forms.KeyEventHandler(WatchF5_KeyDown);
            RootLayoutPanel.ResumeLayout(false);
            RootLayoutPanel.PerformLayout();
            HeaderRowPanel.ResumeLayout(false);
            HeaderRowPanel.PerformLayout();
            HeaderControlsFlowPanel.ResumeLayout(false);
            HeaderControlsFlowPanel.PerformLayout();
            ResumeLayout(false);

        }
        internal System.Windows.Forms.DataGridView DataGridView1;
        internal System.Windows.Forms.TextBox TextBox1;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Button Button2;
        internal System.Windows.Forms.Button Button1;
        internal System.Windows.Forms.CheckBox CheckBox4;
        internal System.Windows.Forms.ContextMenuStrip ContextMenuStrip1;
        internal System.Windows.Forms.ToolStripMenuItem ExportToExcelToolStripMenuItem;
        internal System.Windows.Forms.SaveFileDialog SaveFileDialog1;
        private System.Windows.Forms.TableLayoutPanel RootLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel HeaderRowPanel;
        private System.Windows.Forms.FlowLayoutPanel HeaderControlsFlowPanel;
    }
}
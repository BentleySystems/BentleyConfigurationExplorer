using System;
using System.Diagnostics;
using System.Drawing;

namespace WorkspaceCFG
{
    public partial class VariableValidater : System.Windows.Forms.Form
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
            Label6 = new System.Windows.Forms.Label();
            DataGridView1 = new System.Windows.Forms.DataGridView();
            ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            ExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToExcelToolStripMenuItem.Click += new EventHandler(ToExcelToolStripMenuItem_Click);
            ComboBox1 = new System.Windows.Forms.ComboBox();
            ComboBox1.SelectedIndexChanged += new EventHandler(ComboBox1_SelectedIndexChanged);
            Label1 = new System.Windows.Forms.Label();
            CheckBox1 = new System.Windows.Forms.CheckBox();
            CheckBox1.CheckedChanged += new EventHandler(CheckBox1_CheckedChanged);
            RootLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            HeaderLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)DataGridView1).BeginInit();
            ContextMenuStrip1.SuspendLayout();
            RootLayoutPanel.SuspendLayout();
            HeaderLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // Label6
            // 
            HeaderLayoutPanel.SetColumn(Label6, 3);
            Label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            Label6.AutoSize = true;
            Label6.BackColor = Color.Transparent;
            Label6.Font = new Font("Segoe UI", 8.0f);
            Label6.Name = "Label6";
            Label6.TabIndex = 59;
            Label6.Text = "Results";
            Label6.TextAlign = ContentAlignment.MiddleRight;
            // 
            // DataGridView1
            // 
            RootLayoutPanel.SetRow(DataGridView1, 1);
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
            DataGridView1.Margin = new System.Windows.Forms.Padding(0);
            DataGridView1.Name = "DataGridView1";
            DataGridView1.RowHeadersVisible = false;
            DataGridView1.RowTemplate.DefaultCellStyle.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            DataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            DataGridView1.RowTemplate.Height = 22;
            DataGridView1.RowTemplate.ReadOnly = true;
            DataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            DataGridView1.ShowRowErrors = false;
            DataGridView1.TabIndex = 57;
            // 
            // ContextMenuStrip1
            // 
            ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ExportToolStripMenuItem });
            ContextMenuStrip1.Name = "ContextMenuStrip1";
            ContextMenuStrip1.Size = new Size(109, 26);
            // 
            // ExportToolStripMenuItem
            // 
            ExportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ToExcelToolStripMenuItem });
            ExportToolStripMenuItem.Name = "ExportToolStripMenuItem";
            ExportToolStripMenuItem.Size = new Size(108, 22);
            ExportToolStripMenuItem.Text = "Export";
            // 
            // ToExcelToolStripMenuItem
            // 
            ToExcelToolStripMenuItem.Name = "ToExcelToolStripMenuItem";
            ToExcelToolStripMenuItem.Size = new Size(116, 22);
            ToExcelToolStripMenuItem.Text = "To Excel";
            // 
            // ComboBox1
            // 
            HeaderLayoutPanel.SetColumn(ComboBox1, 1);
            ComboBox1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            ComboBox1.FormattingEnabled = true;
            ComboBox1.MaxDropDownItems = 60;
            ComboBox1.Name = "ComboBox1";
            ComboBox1.Size = new Size(330, 21);
            ComboBox1.TabIndex = 63;
            // 
            // Label1
            // 
            HeaderLayoutPanel.SetColumn(Label1, 0);
            Label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            Label1.AutoSize = true;
            Label1.BackColor = Color.Transparent;
            Label1.Font = new Font("Segoe UI", 8.0f);
            Label1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            Label1.Name = "Label1";
            Label1.TabIndex = 64;
            Label1.Text = "Rule:";
            // 
            // CheckBox1
            // 
            HeaderLayoutPanel.SetColumn(CheckBox1, 2);
            CheckBox1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            CheckBox1.AutoSize = true;
            CheckBox1.Checked = true;
            CheckBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            CheckBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CheckBox1.Font = new Font("Segoe UI", 8.0f);
            CheckBox1.Name = "CheckBox1";
            CheckBox1.TabIndex = 65;
            CheckBox1.Text = "Hide results that have passed validation";
            CheckBox1.UseVisualStyleBackColor = true;
            // 
            // HeaderLayoutPanel
            // 
            HeaderLayoutPanel.AutoSize = true;
            HeaderLayoutPanel.ColumnCount = 4;
            HeaderLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            HeaderLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            HeaderLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            HeaderLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
            HeaderLayoutPanel.Controls.Add(Label1);
            HeaderLayoutPanel.Controls.Add(ComboBox1);
            HeaderLayoutPanel.Controls.Add(CheckBox1);
            HeaderLayoutPanel.Controls.Add(Label6);
            HeaderLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            HeaderLayoutPanel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 9);
            HeaderLayoutPanel.Name = "HeaderLayoutPanel";
            HeaderLayoutPanel.RowCount = 1;
            HeaderLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            HeaderLayoutPanel.TabIndex = 66;
            // 
            // RootLayoutPanel
            // 
            RootLayoutPanel.ColumnCount = 1;
            RootLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
            RootLayoutPanel.Controls.Add(HeaderLayoutPanel, 0, 0);
            RootLayoutPanel.Controls.Add(DataGridView1, 0, 1);
            RootLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            RootLayoutPanel.Name = "RootLayoutPanel";
            RootLayoutPanel.Padding = new System.Windows.Forms.Padding(12, 9, 12, 12);
            RootLayoutPanel.RowCount = 2;
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
            RootLayoutPanel.TabIndex = 67;
            // 
            // VariableValidater
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(784, 361);
            Controls.Add(RootLayoutPanel);
            Font = new Font("Segoe UI", 8.0f);
            KeyPreview = true;
            MinimumSize = new Size(800, 200);
            Name = "VariableValidater";
            ShowIcon = false;
            Text = "Variable Validater";
            ((System.ComponentModel.ISupportInitialize)DataGridView1).EndInit();
            ContextMenuStrip1.ResumeLayout(false);
            RootLayoutPanel.ResumeLayout(false);
            HeaderLayoutPanel.ResumeLayout(false);
            HeaderLayoutPanel.PerformLayout();
            FormClosing += new System.Windows.Forms.FormClosingEventHandler(VariableValidator_FormClosing);
            KeyDown += new System.Windows.Forms.KeyEventHandler(WatchF5_KeyDown);
            ResumeLayout(false);

        }
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.DataGridView DataGridView1;
        internal System.Windows.Forms.ComboBox ComboBox1;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.CheckBox CheckBox1;
        internal System.Windows.Forms.ContextMenuStrip ContextMenuStrip1;
        internal System.Windows.Forms.ToolStripMenuItem ExportToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ToExcelToolStripMenuItem;
        internal System.Windows.Forms.TableLayoutPanel RootLayoutPanel;
        internal System.Windows.Forms.TableLayoutPanel HeaderLayoutPanel;
    }
}
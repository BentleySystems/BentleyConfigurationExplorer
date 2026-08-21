using System;
using System.Diagnostics;
using System.Drawing;

namespace WorkspaceCFG
{
    public partial class VariableLibrary : System.Windows.Forms.Form
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
            DataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(DataGridView1_DataBindingComplete);
            DataGridView1.SelectionChanged += new EventHandler(DataGridView1_SelectionChanged);
            DataGridView1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(DataGridView1_KeyPress);
            DataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(DataGridView1_CellContentClick);
            ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            ExportToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ExportToExcelToolStripMenuItem.Click += new EventHandler(ExportToExcelToolStripMenuItem_Click);
            CheckBox1 = new System.Windows.Forms.CheckBox();
            CheckBox1.CheckedChanged += new EventHandler(CheckBox1_CheckedChanged);
            Label6 = new System.Windows.Forms.Label();
            TextBox1 = new System.Windows.Forms.TextBox();
            TextBox1.TextChanged += new EventHandler(TextBox1_TextChanged);
            Label4 = new System.Windows.Forms.Label();
            Button1 = new System.Windows.Forms.Button();
            Button1.Click += new EventHandler(Button1_Click);
            RichTextBox3 = new System.Windows.Forms.RichTextBox();
            Label3 = new System.Windows.Forms.Label();
            RichTextBox1 = new System.Windows.Forms.RichTextBox();
            Label1 = new System.Windows.Forms.Label();
            SaveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            TextBox2 = new System.Windows.Forms.TextBox();
            TextBox2.TextChanged += new EventHandler(TextBox2_TextChanged);
            Label2 = new System.Windows.Forms.Label();
            Button2 = new System.Windows.Forms.Button();
            Button2.Click += new EventHandler(Button2_Click);
            RootLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            FilterRowPanel = new System.Windows.Forms.FlowLayoutPanel();
            GridHeaderRowPanel = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)DataGridView1).BeginInit();
            ContextMenuStrip1.SuspendLayout();
            RootLayoutPanel.SuspendLayout();
            FilterRowPanel.SuspendLayout();
            GridHeaderRowPanel.SuspendLayout();
            SuspendLayout();
            // 
            // DataGridView1
            // 
            RootLayoutPanel.SetRow(DataGridView1, 2);
            DataGridView1.AllowUserToAddRows = false;
            DataGridView1.AllowUserToDeleteRows = false;
            DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
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
            DataGridView1.RowHeadersWidth = 51;
            DataGridView1.RowTemplate.DefaultCellStyle.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            DataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            DataGridView1.RowTemplate.Height = 22;
            DataGridView1.RowTemplate.ReadOnly = true;
            DataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            DataGridView1.ShowRowErrors = false;
            DataGridView1.TabIndex = 37;
            // 
            // ContextMenuStrip1
            // 
            ContextMenuStrip1.ImageScalingSize = new Size(20, 20);
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
            // CheckBox1
            // 
            CheckBox1.AutoSize = true;
            CheckBox1.FlatAppearance.BorderSize = 0;
            CheckBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CheckBox1.Font = new Font("Segoe UI", 8.0f);
            CheckBox1.Margin = new System.Windows.Forms.Padding(24, 6, 3, 3);
            CheckBox1.Name = "CheckBox1";
            CheckBox1.TabIndex = 38;
            CheckBox1.Text = "Hide Variables in use by current workspace";
            CheckBox1.UseVisualStyleBackColor = true;
            // 
            // Label6
            // 
            GridHeaderRowPanel.SetColumn(Label6, 1);
            Label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            Label6.BackColor = Color.Transparent;
            Label6.Dock = System.Windows.Forms.DockStyle.Fill;
            Label6.Font = new Font("Segoe UI", 8.0f);
            Label6.Name = "Label6";
            Label6.TabIndex = 39;
            Label6.Text = "Variables";
            Label6.TextAlign = ContentAlignment.BottomRight;
            // 
            // TextBox1
            // 
            FilterRowPanel.SetFlowBreak(TextBox1, false);
            TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            TextBox1.Font = new Font("Segoe UI", 8.0f);
            TextBox1.Margin = new System.Windows.Forms.Padding(3, 16, 3, 3);
            TextBox1.Name = "TextBox1";
            TextBox1.Size = new Size(204, 22);
            TextBox1.TabIndex = 40;
            // 
            // Label4
            // 
            Label4.AutoSize = true;
            Label4.BackColor = Color.Transparent;
            Label4.Font = new Font("Segoe UI", 8.0f);
            Label4.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            Label4.Name = "Label4";
            Label4.TabIndex = 42;
            Label4.Text = "Name Filter:";
            // 
            // Button1
            // 
            Button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            Button1.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            Button1.Margin = new System.Windows.Forms.Padding(3, 13, 24, 3);
            Button1.Name = "Button1";
            Button1.Size = new Size(37, 20);
            Button1.TabIndex = 41;
            Button1.Text = "Clear";
            Button1.UseVisualStyleBackColor = true;
            // 
            // RichTextBox3
            // 
            RootLayoutPanel.SetRow(RichTextBox3, 4);
            RichTextBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            RichTextBox3.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            RichTextBox3.Name = "RichTextBox3";
            RichTextBox3.ReadOnly = true;
            RichTextBox3.Size = new Size(760, 100);
            RichTextBox3.TabIndex = 43;
            RichTextBox3.Text = "";
            // 
            // Label3
            // 
            RootLayoutPanel.SetRow(Label3, 3);
            Label3.AutoSize = true;
            Label3.BackColor = Color.Transparent;
            Label3.Font = new Font("Segoe UI", 8.0f);
            Label3.Margin = new System.Windows.Forms.Padding(4, 6, 3, 0);
            Label3.Name = "Label3";
            Label3.TabIndex = 44;
            Label3.Text = "Description:";
            // 
            // RichTextBox1
            // 
            RootLayoutPanel.SetRow(RichTextBox1, 6);
            RichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            RichTextBox1.Margin = new System.Windows.Forms.Padding(0);
            RichTextBox1.Name = "RichTextBox1";
            RichTextBox1.ReadOnly = true;
            RichTextBox1.Size = new Size(760, 100);
            RichTextBox1.TabIndex = 45;
            RichTextBox1.Text = "";
            // 
            // Label1
            // 
            RootLayoutPanel.SetRow(Label1, 5);
            Label1.AutoSize = true;
            Label1.BackColor = Color.Transparent;
            Label1.Font = new Font("Segoe UI", 8.0f);
            Label1.Margin = new System.Windows.Forms.Padding(4, 6, 3, 0);
            Label1.Name = "Label1";
            Label1.TabIndex = 46;
            Label1.Text = "Comment:";
            // 
            // TextBox2
            // 
            TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            TextBox2.Font = new Font("Segoe UI", 8.0f);
            TextBox2.Margin = new System.Windows.Forms.Padding(24, 16, 3, 3);
            TextBox2.Name = "TextBox2";
            TextBox2.Size = new Size(204, 22);
            TextBox2.TabIndex = 47;
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.BackColor = Color.Transparent;
            Label2.Font = new Font("Segoe UI", 8.0f);
            Label2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            Label2.Name = "Label2";
            Label2.TabIndex = 49;
            Label2.Text = "Description Filter:";
            // 
            // Button2
            // 
            Button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            Button2.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            Button2.Margin = new System.Windows.Forms.Padding(3, 13, 3, 3);
            Button2.Name = "Button2";
            Button2.Size = new Size(37, 20);
            Button2.TabIndex = 48;
            Button2.Text = "Clear";
            Button2.UseVisualStyleBackColor = true;
            // 
            // GridHeaderRowPanel
            // 
            RootLayoutPanel.SetRow(GridHeaderRowPanel, 1);
            GridHeaderRowPanel.AutoSize = true;
            GridHeaderRowPanel.ColumnCount = 2;
            GridHeaderRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            GridHeaderRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
            GridHeaderRowPanel.Controls.Add(Label6, 1, 0);
            GridHeaderRowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            GridHeaderRowPanel.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            GridHeaderRowPanel.Name = "GridHeaderRowPanel";
            GridHeaderRowPanel.RowCount = 1;
            GridHeaderRowPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            GridHeaderRowPanel.TabIndex = 50;
            // 
            // FilterRowPanel
            // 
            RootLayoutPanel.SetRow(FilterRowPanel, 0);
            FilterRowPanel.AutoSize = true;
            FilterRowPanel.Controls.Add(Label4);
            FilterRowPanel.Controls.Add(TextBox1);
            FilterRowPanel.Controls.Add(Button1);
            FilterRowPanel.Controls.Add(Label2);
            FilterRowPanel.Controls.Add(TextBox2);
            FilterRowPanel.Controls.Add(Button2);
            FilterRowPanel.Controls.Add(CheckBox1);
            FilterRowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            FilterRowPanel.Margin = new System.Windows.Forms.Padding(0);
            FilterRowPanel.Name = "FilterRowPanel";
            FilterRowPanel.TabIndex = 51;
            FilterRowPanel.WrapContents = false;
            // 
            // RootLayoutPanel
            // 
            RootLayoutPanel.ColumnCount = 1;
            RootLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
            RootLayoutPanel.Controls.Add(FilterRowPanel, 0, 0);
            RootLayoutPanel.Controls.Add(GridHeaderRowPanel, 0, 1);
            RootLayoutPanel.Controls.Add(DataGridView1, 0, 2);
            RootLayoutPanel.Controls.Add(Label3, 0, 3);
            RootLayoutPanel.Controls.Add(RichTextBox3, 0, 4);
            RootLayoutPanel.Controls.Add(Label1, 0, 5);
            RootLayoutPanel.Controls.Add(RichTextBox1, 0, 6);
            RootLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            RootLayoutPanel.Name = "RootLayoutPanel";
            RootLayoutPanel.Padding = new System.Windows.Forms.Padding(12, 9, 12, 12);
            RootLayoutPanel.RowCount = 7;
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100f));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100f));
            RootLayoutPanel.TabIndex = 52;
            // 
            // VariableLibrary
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(784, 567);
            Controls.Add(RootLayoutPanel);
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 8.0f);
            KeyPreview = true;
            MinimumSize = new Size(800, 606);
            Name = "VariableLibrary";
            ShowIcon = false;
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            Text = "Variable Library";
            ((System.ComponentModel.ISupportInitialize)DataGridView1).EndInit();
            ContextMenuStrip1.ResumeLayout(false);
            RootLayoutPanel.ResumeLayout(false);
            RootLayoutPanel.PerformLayout();
            FilterRowPanel.ResumeLayout(false);
            FilterRowPanel.PerformLayout();
            GridHeaderRowPanel.ResumeLayout(false);
            GridHeaderRowPanel.PerformLayout();
            FormClosing += new System.Windows.Forms.FormClosingEventHandler(VariableLibrary_FormClosing);
            Shown += new EventHandler(VariableLibrary_Shown);
            Load += new EventHandler(VariableLibrary_Load);
            KeyDown += new System.Windows.Forms.KeyEventHandler(WatchF5_KeyDown);
            ResumeLayout(false);

        }
        internal System.Windows.Forms.DataGridView DataGridView1;
        internal System.Windows.Forms.CheckBox CheckBox1;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.TextBox TextBox1;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Button Button1;
        internal System.Windows.Forms.RichTextBox RichTextBox3;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.RichTextBox RichTextBox1;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.ContextMenuStrip ContextMenuStrip1;
        internal System.Windows.Forms.ToolStripMenuItem ExportToExcelToolStripMenuItem;
        internal System.Windows.Forms.SaveFileDialog SaveFileDialog1;
        internal System.Windows.Forms.TextBox TextBox2;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Button Button2;
        internal System.Windows.Forms.TableLayoutPanel RootLayoutPanel;
        internal System.Windows.Forms.FlowLayoutPanel FilterRowPanel;
        internal System.Windows.Forms.TableLayoutPanel GridHeaderRowPanel;
    }
}
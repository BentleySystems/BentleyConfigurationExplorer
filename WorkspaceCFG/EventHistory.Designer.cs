using System;
using System.Diagnostics;
using System.Drawing;

namespace WorkspaceCFG
{
    public partial class EventHistory : System.Windows.Forms.Form
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
            ContextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(ContextMenuStrip1_Opening);
            GoToLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            GoToLineToolStripMenuItem.Click += new EventHandler(GoToLineToolStripMenuItem_Click);
            OpenVariableInDossierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            OpenVariableInDossierToolStripMenuItem.Click += new EventHandler(OpenVariableInDossierToolStripMenuItem_Click);
            OpenFileInEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            OpenFileInEditorToolStripMenuItem.Click += new EventHandler(OpenFileInEditorToolStripMenuItem_Click);
            OpenFileInViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            OpenFileInViewerToolStripMenuItem.Click += new EventHandler(OpenFileInViewerToolStripMenuItem_Click);
            ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            ExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToExcelToolStripMenuItem.Click += new EventHandler(ToExcelToolStripMenuItem_Click);
            ToTextFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            CheckBox1 = new System.Windows.Forms.CheckBox();
            CheckBox1.CheckedChanged += new EventHandler(CheckBox1_CheckedChanged);
            CheckBox2 = new System.Windows.Forms.CheckBox();
            CheckBox2.CheckedChanged += new EventHandler(CheckBox2_CheckedChanged);
            CheckBox3 = new System.Windows.Forms.CheckBox();
            CheckBox3.CheckedChanged += new EventHandler(CheckBox3_CheckedChanged);
            ComboBox1 = new System.Windows.Forms.ComboBox();
            ComboBox1.SelectedIndexChanged += new EventHandler(ComboBox1_SelectedIndexChanged);
            Label1 = new System.Windows.Forms.Label();
            CheckBox4 = new System.Windows.Forms.CheckBox();
            CheckBox4.CheckedChanged += new EventHandler(CheckBox4_CheckedChanged);
            SaveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            MainLayout = new System.Windows.Forms.TableLayoutPanel();
            CheckBoxFlow = new System.Windows.Forms.FlowLayoutPanel();
            FilterFlow = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)DataGridView1).BeginInit();
            ContextMenuStrip1.SuspendLayout();
            MainLayout.SuspendLayout();
            CheckBoxFlow.SuspendLayout();
            FilterFlow.SuspendLayout();
            SuspendLayout();
            // 
            // DataGridView1
            // 
            DataGridView1.AllowUserToDeleteRows = false;
            DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;

            DataGridView1.BackgroundColor = Color.White;
            DataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            DataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            DataGridView1.ColumnHeadersDefaultCellStyle.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridView1.ContextMenuStrip = ContextMenuStrip1;
            DataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            DataGridView1.GridColor = Color.LightSteelBlue;
            DataGridView1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            DataGridView1.Name = "DataGridView1";
            DataGridView1.RowHeadersVisible = false;
            DataGridView1.RowTemplate.DefaultCellStyle.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            DataGridView1.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(0, 2, 0, 3);
            DataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            DataGridView1.RowTemplate.Height = 22;
            DataGridView1.RowTemplate.ReadOnly = true;
            DataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            DataGridView1.ShowRowErrors = false;
            DataGridView1.TabIndex = 36;
            // 
            // ContextMenuStrip1
            // 
            ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { GoToLineToolStripMenuItem, OpenVariableInDossierToolStripMenuItem, OpenFileInEditorToolStripMenuItem, OpenFileInViewerToolStripMenuItem, ToolStripSeparator1, ExportToolStripMenuItem });
            ContextMenuStrip1.Name = "ContextMenuStrip1";
            ContextMenuStrip1.Size = new Size(202, 120);
            // 
            // GoToLineToolStripMenuItem
            // 
            GoToLineToolStripMenuItem.Name = "GoToLineToolStripMenuItem";
            GoToLineToolStripMenuItem.Size = new Size(201, 22);
            GoToLineToolStripMenuItem.Text = "Go To Line in Viewer";
            // 
            // OpenVariableInDossierToolStripMenuItem
            // 
            OpenVariableInDossierToolStripMenuItem.Name = "OpenVariableInDossierToolStripMenuItem";
            OpenVariableInDossierToolStripMenuItem.Size = new Size(201, 22);
            OpenVariableInDossierToolStripMenuItem.Text = "Open Variable in Dossier";
            // 
            // OpenFileInEditorToolStripMenuItem
            // 
            OpenFileInEditorToolStripMenuItem.Name = "OpenFileInEditorToolStripMenuItem";
            OpenFileInEditorToolStripMenuItem.Size = new Size(201, 22);
            OpenFileInEditorToolStripMenuItem.Text = "Open File in Editor";
            // 
            // OpenFileInViewerToolStripMenuItem
            // 
            OpenFileInViewerToolStripMenuItem.Name = "OpenFileInViewerToolStripMenuItem";
            OpenFileInViewerToolStripMenuItem.Size = new Size(201, 22);
            OpenFileInViewerToolStripMenuItem.Text = "Open File in Viewer";
            // 
            // ToolStripSeparator1
            // 
            ToolStripSeparator1.Name = "ToolStripSeparator1";
            ToolStripSeparator1.Size = new Size(198, 6);
            // 
            // ExportToolStripMenuItem
            // 
            ExportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ToExcelToolStripMenuItem, ToTextFileToolStripMenuItem });
            ExportToolStripMenuItem.Name = "ExportToolStripMenuItem";
            ExportToolStripMenuItem.Size = new Size(201, 22);
            ExportToolStripMenuItem.Text = "Export";
            // 
            // ToExcelToolStripMenuItem
            // 
            ToExcelToolStripMenuItem.Name = "ToExcelToolStripMenuItem";
            ToExcelToolStripMenuItem.Size = new Size(131, 22);
            ToExcelToolStripMenuItem.Text = "To Excel";
            // 
            // ToTextFileToolStripMenuItem
            // 
            ToTextFileToolStripMenuItem.Name = "ToTextFileToolStripMenuItem";
            ToTextFileToolStripMenuItem.Size = new Size(131, 22);
            ToTextFileToolStripMenuItem.Text = "To Text File";
            // 
            // CheckBox1
            // 
            CheckBox1.AutoSize = true;
            CheckBox1.FlatAppearance.BorderSize = 0;
            CheckBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CheckBox1.Font = new Font("Segoe UI", 8.0f);
            CheckBox1.Margin = new System.Windows.Forms.Padding(0, 3, 12, 3);
            CheckBox1.Name = "CheckBox1";
            CheckBox1.Size = new Size(85, 17);
            CheckBox1.TabIndex = 37;
            CheckBox1.Text = "Show Errors";
            CheckBox1.UseVisualStyleBackColor = true;
            // 
            // CheckBox2
            // 
            CheckBox2.AutoSize = true;
            CheckBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CheckBox2.Font = new Font("Segoe UI", 8.0f);
            CheckBox2.Margin = new System.Windows.Forms.Padding(0, 3, 12, 3);
            CheckBox2.Name = "CheckBox2";
            CheckBox2.Size = new Size(105, 17);
            CheckBox2.TabIndex = 38;
            CheckBox2.Text = "Show Warnings";
            CheckBox2.UseVisualStyleBackColor = true;
            // 
            // CheckBox3
            // 
            CheckBox3.AutoSize = true;
            CheckBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CheckBox3.Font = new Font("Segoe UI", 8.0f);
            CheckBox3.Margin = new System.Windows.Forms.Padding(0, 3, 12, 3);
            CheckBox3.Name = "CheckBox3";
            CheckBox3.Size = new Size(98, 17);
            CheckBox3.TabIndex = 39;
            CheckBox3.Text = "Show Includes";
            CheckBox3.UseVisualStyleBackColor = true;
            // 
            // ComboBox1
            // 
            ComboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            ComboBox1.FormattingEnabled = true;
            ComboBox1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            ComboBox1.MaxDropDownItems = 60;
            ComboBox1.Name = "ComboBox1";
            ComboBox1.Size = new Size(760, 21);
            ComboBox1.TabIndex = 40;
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            Label1.Font = new Font("Segoe UI", 8.0f);
            Label1.Margin = new System.Windows.Forms.Padding(0, 6, 6, 3);
            Label1.Name = "Label1";
            Label1.Size = new Size(71, 13);
            Label1.TabIndex = 41;
            Label1.Text = "Filter By File:";
            // 
            // CheckBox4
            // 
            CheckBox4.AutoSize = true;
            CheckBox4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CheckBox4.Font = new Font("Segoe UI", 8.0f);
            CheckBox4.Margin = new System.Windows.Forms.Padding(0, 3, 12, 3);
            CheckBox4.Name = "CheckBox4";
            CheckBox4.Size = new Size(90, 17);
            CheckBox4.TabIndex = 47;
            CheckBox4.Text = "Show Others";
            CheckBox4.UseVisualStyleBackColor = true;
            // 
            // CheckBoxFlow
            // 
            CheckBoxFlow.AutoSize = true;
            CheckBoxFlow.Controls.Add(CheckBox1);
            CheckBoxFlow.Controls.Add(CheckBox2);
            CheckBoxFlow.Controls.Add(CheckBox3);
            CheckBoxFlow.Controls.Add(CheckBox4);
            CheckBoxFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            CheckBoxFlow.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            CheckBoxFlow.Name = "CheckBoxFlow";
            CheckBoxFlow.Size = new Size(760, 23);
            CheckBoxFlow.TabIndex = 48;
            CheckBoxFlow.WrapContents = false;
            // 
            // FilterFlow
            // 
            FilterFlow.AutoSize = true;
            FilterFlow.ColumnCount = 2;
            FilterFlow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            FilterFlow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            FilterFlow.Controls.Add(Label1, 0, 0);
            FilterFlow.Controls.Add(ComboBox1, 1, 0);
            FilterFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            FilterFlow.Name = "FilterFlow";
            FilterFlow.RowCount = 1;
            FilterFlow.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            FilterFlow.Size = new Size(760, 27);
            FilterFlow.TabIndex = 49;
            // 
            // MainLayout
            // 
            MainLayout.ColumnCount = 1;
            MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            MainLayout.Controls.Add(CheckBoxFlow, 0, 0);
            MainLayout.Controls.Add(FilterFlow, 0, 1);
            MainLayout.Controls.Add(DataGridView1, 0, 2);
            MainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            MainLayout.Name = "MainLayout";
            MainLayout.Padding = new System.Windows.Forms.Padding(9);
            MainLayout.RowCount = 3;
            MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            MainLayout.TabIndex = 50;
            // 
            // EventHistory
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(784, 361);
            Controls.Add(MainLayout);
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 8.0f);
            KeyPreview = true;
            MinimumSize = new Size(600, 200);
            Name = "EventHistory";
            ShowIcon = false;
            Text = "Event History";
            ((System.ComponentModel.ISupportInitialize)DataGridView1).EndInit();
            ContextMenuStrip1.ResumeLayout(false);
            MainLayout.ResumeLayout(false);
            MainLayout.PerformLayout();
            CheckBoxFlow.ResumeLayout(false);
            CheckBoxFlow.PerformLayout();
            FilterFlow.ResumeLayout(false);
            FilterFlow.PerformLayout();
            FormClosed += new System.Windows.Forms.FormClosedEventHandler(EventHistory_FormClosed);
            Shown += new EventHandler(EventHistory_Shown);
            FormClosing += new System.Windows.Forms.FormClosingEventHandler(EventHistory_FormClosing);
            Load += new EventHandler(EventHistory_Load);
            KeyDown += new System.Windows.Forms.KeyEventHandler(WatchF5_KeyDown);
            ResumeLayout(false);

        }
        internal System.Windows.Forms.TableLayoutPanel MainLayout;
        internal System.Windows.Forms.FlowLayoutPanel CheckBoxFlow;
        internal System.Windows.Forms.TableLayoutPanel FilterFlow;
        internal System.Windows.Forms.DataGridView DataGridView1;
        internal System.Windows.Forms.CheckBox CheckBox1;
        internal System.Windows.Forms.CheckBox CheckBox2;
        internal System.Windows.Forms.CheckBox CheckBox3;
        internal System.Windows.Forms.ComboBox ComboBox1;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.ContextMenuStrip ContextMenuStrip1;
        internal System.Windows.Forms.ToolStripMenuItem ExportToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ToTextFileToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ToExcelToolStripMenuItem;
        internal System.Windows.Forms.CheckBox CheckBox4;
        internal System.Windows.Forms.ToolStripMenuItem GoToLineToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem OpenFileInEditorToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem OpenFileInViewerToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
        internal System.Windows.Forms.SaveFileDialog SaveFileDialog1;
        internal System.Windows.Forms.ToolStripMenuItem OpenVariableInDossierToolStripMenuItem;
    }
}
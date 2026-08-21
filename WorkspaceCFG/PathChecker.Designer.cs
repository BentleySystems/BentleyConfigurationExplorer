using System;
using System.Diagnostics;
using System.Drawing;

namespace WorkspaceCFG
{
    public partial class PathChecker : System.Windows.Forms.Form
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.DataGridView1 = new System.Windows.Forms.DataGridView();
            this.ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.OpenContainingFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFirstAvaliableFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenVariableInDossieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Button1 = new System.Windows.Forms.Button();
            this.Label6 = new System.Windows.Forms.Label();
            this.Button2 = new System.Windows.Forms.Button();
            this.btnOpenTreeViewer = new System.Windows.Forms.Button();
            this.RootLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.FilterRowPanel = new System.Windows.Forms.TableLayoutPanel();
            this.FilterControlsFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.FooterRowPanel = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView1)).BeginInit();
            this.ContextMenuStrip1.SuspendLayout();
            this.RootLayoutPanel.SuspendLayout();
            this.FilterRowPanel.SuspendLayout();
            this.FilterControlsFlowPanel.SuspendLayout();
            this.FooterRowPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TextBox1
            // 
            this.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.Size = new System.Drawing.Size(470, 26);
            this.TextBox1.TabIndex = 53;
            this.TextBox1.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.BackColor = System.Drawing.Color.Transparent;
            this.Label4.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.Margin = new System.Windows.Forms.Padding(0, 6, 4, 0);
            this.Label4.Name = "Label4";
            this.Label4.TabIndex = 54;
            this.Label4.Text = "Filter:";
            // 
            // DataGridView1
            // 
            this.DataGridView1.AllowUserToAddRows = false;
            this.DataGridView1.AllowUserToDeleteRows = false;
            this.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.DataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.DataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView1.ContextMenuStrip = this.ContextMenuStrip1;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DataGridView1.DefaultCellStyle = dataGridViewCellStyle5;
            this.DataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.DataGridView1.GridColor = System.Drawing.Color.LightSteelBlue;
            this.DataGridView1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.DataGridView1.Name = "DataGridView1";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.DataGridView1.RowHeadersVisible = false;
            this.DataGridView1.RowHeadersWidth = 62;
            this.DataGridView1.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DataGridView1.RowTemplate.Height = 22;
            this.DataGridView1.RowTemplate.ReadOnly = true;
            this.DataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridView1.ShowRowErrors = false;
            this.DataGridView1.TabIndex = 52;
            this.DataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataGridView1_CellFormatting);
            // 
            // ContextMenuStrip1
            // 
            this.ContextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenContainingFolderToolStripMenuItem,
            this.OpenFirstAvaliableFolderToolStripMenuItem,
            this.OpenVariableInDossieToolStripMenuItem});
            this.ContextMenuStrip1.Name = "ContextMenuStrip1";
            this.ContextMenuStrip1.Size = new System.Drawing.Size(298, 100);
            // 
            // OpenContainingFolderToolStripMenuItem
            // 
            this.OpenContainingFolderToolStripMenuItem.Name = "OpenContainingFolderToolStripMenuItem";
            this.OpenContainingFolderToolStripMenuItem.Size = new System.Drawing.Size(297, 32);
            this.OpenContainingFolderToolStripMenuItem.Text = "Open Containing Folder";
            this.OpenContainingFolderToolStripMenuItem.Click += new System.EventHandler(this.OpenContainingFolderToolStripMenuItem_Click);
            // 
            // OpenFirstAvaliableFolderToolStripMenuItem
            // 
            this.OpenFirstAvaliableFolderToolStripMenuItem.Name = "OpenFirstAvaliableFolderToolStripMenuItem";
            this.OpenFirstAvaliableFolderToolStripMenuItem.Size = new System.Drawing.Size(297, 32);
            this.OpenFirstAvaliableFolderToolStripMenuItem.Text = "Open First Available Folder";
            this.OpenFirstAvaliableFolderToolStripMenuItem.Click += new System.EventHandler(this.OpenFirstAvaliableFolderToolStripMenuItem_Click);
            // 
            // OpenVariableInDossieToolStripMenuItem
            // 
            this.OpenVariableInDossieToolStripMenuItem.Name = "OpenVariableInDossieToolStripMenuItem";
            this.OpenVariableInDossieToolStripMenuItem.Size = new System.Drawing.Size(297, 32);
            this.OpenVariableInDossieToolStripMenuItem.Text = "Open Variable in Dossier";
            this.OpenVariableInDossieToolStripMenuItem.Click += new System.EventHandler(this.OpenVariableInDossieToolStripMenuItem_Click);
            // 
            // Button1
            // 
            this.Button1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Button1.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button1.Margin = new System.Windows.Forms.Padding(4, 5, 0, 5);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(132, 43);
            this.Button1.TabIndex = 55;
            this.Button1.Text = "Rescan";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Label6
            // 
            this.Label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Label6.AutoSize = true;
            this.Label6.BackColor = System.Drawing.Color.Transparent;
            this.Label6.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.Margin = new System.Windows.Forms.Padding(4, 0, 0, 6);
            this.Label6.Name = "Label6";
            this.Label6.TabIndex = 56;
            this.Label6.Text = "Results";
            this.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Button2
            // 
            this.Button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Button2.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(64, 26);
            this.Button2.TabIndex = 57;
            this.Button2.Text = "Clear";
            this.Button2.UseVisualStyleBackColor = true;
            this.Button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // btnOpenTreeViewer
            // 
            this.btnOpenTreeViewer.Margin = new System.Windows.Forms.Padding(4, 5, 0, 5);
            this.btnOpenTreeViewer.Name = "btnOpenTreeViewer";
            this.btnOpenTreeViewer.Size = new System.Drawing.Size(266, 26);
            this.btnOpenTreeViewer.TabIndex = 58;
            this.btnOpenTreeViewer.Text = "Load Tree View";
            this.btnOpenTreeViewer.UseVisualStyleBackColor = true;
            this.btnOpenTreeViewer.Click += new System.EventHandler(this.btnOpenTreeViewer_Click);
            // 
            // RootLayoutPanel
            // 
            this.RootLayoutPanel.ColumnCount = 1;
            this.RootLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RootLayoutPanel.Padding = new System.Windows.Forms.Padding(18, 11, 18, 11);
            this.RootLayoutPanel.RowCount = 3;
            this.RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.RootLayoutPanel.Controls.Add(this.FilterRowPanel, 0, 0);
            this.RootLayoutPanel.Controls.Add(this.DataGridView1, 0, 1);
            this.RootLayoutPanel.Controls.Add(this.FooterRowPanel, 0, 2);
            this.RootLayoutPanel.Name = "RootLayoutPanel";
            // 
            // FilterRowPanel
            // 
            this.FilterRowPanel.AutoSize = true;
            this.FilterRowPanel.ColumnCount = 2;
            this.FilterRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.FilterRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.FilterRowPanel.Controls.Add(this.FilterControlsFlowPanel, 0, 0);
            this.FilterRowPanel.Controls.Add(this.Label6, 1, 0);
            this.FilterRowPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.FilterRowPanel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.FilterRowPanel.Name = "FilterRowPanel";
            this.FilterRowPanel.RowCount = 1;
            this.FilterRowPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // FilterControlsFlowPanel
            // 
            this.FilterControlsFlowPanel.AutoSize = true;
            this.FilterControlsFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.FilterControlsFlowPanel.WrapContents = false;
            this.FilterControlsFlowPanel.Margin = new System.Windows.Forms.Padding(0);
            this.FilterControlsFlowPanel.Controls.Add(this.Label4);
            this.FilterControlsFlowPanel.Controls.Add(this.TextBox1);
            this.FilterControlsFlowPanel.Controls.Add(this.Button2);
            this.FilterControlsFlowPanel.Controls.Add(this.btnOpenTreeViewer);
            this.FilterControlsFlowPanel.Name = "FilterControlsFlowPanel";
            // 
            // FooterRowPanel
            // 
            this.FooterRowPanel.AutoSize = true;
            this.FooterRowPanel.ColumnCount = 2;
            this.FooterRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.FooterRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.FooterRowPanel.Controls.Add(this.Button1, 1, 0);
            this.FooterRowPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.FooterRowPanel.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.FooterRowPanel.Name = "FooterRowPanel";
            this.FooterRowPanel.RowCount = 1;
            this.FooterRowPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // PathChecker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 555);
            this.Controls.Add(this.RootLayoutPanel);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(889, 278);
            this.Name = "PathChecker";
            this.ShowIcon = false;
            this.Text = "Path Checker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PathChecker_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WatchF5_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView1)).EndInit();
            this.ContextMenuStrip1.ResumeLayout(false);
            this.RootLayoutPanel.ResumeLayout(false);
            this.RootLayoutPanel.PerformLayout();
            this.FilterRowPanel.ResumeLayout(false);
            this.FilterRowPanel.PerformLayout();
            this.FilterControlsFlowPanel.ResumeLayout(false);
            this.FilterControlsFlowPanel.PerformLayout();
            this.FooterRowPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        internal System.Windows.Forms.TextBox TextBox1;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.DataGridView DataGridView1;
        internal System.Windows.Forms.Button Button1;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.ContextMenuStrip ContextMenuStrip1;
        internal System.Windows.Forms.ToolStripMenuItem OpenContainingFolderToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem OpenFirstAvaliableFolderToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem OpenVariableInDossieToolStripMenuItem;
        internal System.Windows.Forms.Button Button2;
        private System.Windows.Forms.Button btnOpenTreeViewer;
        private System.Windows.Forms.TableLayoutPanel RootLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel FilterRowPanel;
        private System.Windows.Forms.FlowLayoutPanel FilterControlsFlowPanel;
        private System.Windows.Forms.TableLayoutPanel FooterRowPanel;
    }
}
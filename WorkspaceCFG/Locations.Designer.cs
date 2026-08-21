using System;
using System.Diagnostics;
using System.Drawing;

namespace WorkspaceCFG
{
    public partial class Locations : System.Windows.Forms.Form
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
            dgLocations = new System.Windows.Forms.DataGridView();
            dgLocations.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgLocations_CellContentClick);
            ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            CopyPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            CopyPathToolStripMenuItem.Click += new EventHandler(CopyPathToolStripMenuItem_Click);
            CopyFileNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            CopyFileNameToolStripMenuItem.Click += new EventHandler(CopyFileNameToolStripMenuItem_Click);
            OpenContainingFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            OpenContainingFolderToolStripMenuItem.Click += new EventHandler(OpenContainingFolderToolStripMenuItem_Click);
            TextBox1 = new System.Windows.Forms.TextBox();
            TextBox1.TextChanged += new EventHandler(TextBox1_TextChanged);
            Label4 = new System.Windows.Forms.Label();
            Button1 = new System.Windows.Forms.Button();
            Button1.Click += new EventHandler(Button1_Click);
            Label6 = new System.Windows.Forms.Label();
            CheckBox1 = new System.Windows.Forms.CheckBox();
            CheckBox1.CheckedChanged += new EventHandler(CheckBox1_CheckedChanged);
            RootLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            FilterRowPanel = new System.Windows.Forms.TableLayoutPanel();
            FilterControlsFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)dgLocations).BeginInit();
            ContextMenuStrip1.SuspendLayout();
            RootLayoutPanel.SuspendLayout();
            FilterRowPanel.SuspendLayout();
            FilterControlsFlowPanel.SuspendLayout();
            SuspendLayout();
            // 
            // dgLocations
            // 
            dgLocations.AllowUserToAddRows = false;
            dgLocations.AllowUserToDeleteRows = false;
            dgLocations.AllowUserToOrderColumns = true;
            dgLocations.Dock = System.Windows.Forms.DockStyle.Fill;
            dgLocations.BackgroundColor = Color.White;
            dgLocations.CausesValidation = false;
            dgLocations.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dgLocations.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dgLocations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgLocations.ContextMenuStrip = ContextMenuStrip1;
            dgLocations.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            dgLocations.GridColor = Color.LightSteelBlue;
            dgLocations.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            dgLocations.Name = "dgLocations";
            dgLocations.RowHeadersVisible = false;
            dgLocations.RowTemplate.DefaultCellStyle.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgLocations.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dgLocations.RowTemplate.Height = 22;
            dgLocations.RowTemplate.ReadOnly = true;
            dgLocations.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            dgLocations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgLocations.ShowRowErrors = false;
            dgLocations.TabIndex = 3;
            dgLocations.VirtualMode = true;
            // 
            // ContextMenuStrip1
            // 
            ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { CopyPathToolStripMenuItem, CopyFileNameToolStripMenuItem, OpenContainingFolderToolStripMenuItem });
            ContextMenuStrip1.Name = "ContextMenuStrip1";
            ContextMenuStrip1.Size = new Size(202, 70);
            // 
            // CopyPathToolStripMenuItem
            // 
            CopyPathToolStripMenuItem.Name = "CopyPathToolStripMenuItem";
            CopyPathToolStripMenuItem.Size = new Size(201, 22);
            CopyPathToolStripMenuItem.Text = "Copy Path";
            // 
            // CopyFileNameToolStripMenuItem
            // 
            CopyFileNameToolStripMenuItem.Name = "CopyFileNameToolStripMenuItem";
            CopyFileNameToolStripMenuItem.Size = new Size(201, 22);
            CopyFileNameToolStripMenuItem.Text = "Copy File Name";
            // 
            // OpenContainingFolderToolStripMenuItem
            // 
            OpenContainingFolderToolStripMenuItem.Name = "OpenContainingFolderToolStripMenuItem";
            OpenContainingFolderToolStripMenuItem.Size = new Size(201, 22);
            OpenContainingFolderToolStripMenuItem.Text = "Open Containing Folder";
            // 
            // TextBox1
            // 
            TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            TextBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            TextBox1.Name = "TextBox1";
            TextBox1.Size = new Size(211, 22);
            TextBox1.TabIndex = 19;
            // 
            // Label4
            // 
            Label4.AutoSize = true;
            Label4.BackColor = Color.Transparent;
            Label4.Font = new Font("Segoe UI", 8.0f);
            Label4.Margin = new System.Windows.Forms.Padding(0, 6, 4, 0);
            Label4.Name = "Label4";
            Label4.TabIndex = 21;
            Label4.Text = "Filter:";
            // 
            // Button1
            // 
            Button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            Button1.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            Button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Button1.Name = "Button1";
            Button1.Size = new Size(47, 20);
            Button1.TabIndex = 20;
            Button1.Text = "Clear";
            Button1.UseVisualStyleBackColor = true;
            // 
            // Label6
            // 
            Label6.BackColor = Color.Transparent;
            Label6.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            Label6.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            Label6.Name = "Label6";
            Label6.Dock = System.Windows.Forms.DockStyle.Fill;
            Label6.TabIndex = 49;
            Label6.Text = "Locations";
            Label6.TextAlign = ContentAlignment.MiddleRight;
            // 
            // CheckBox1
            // 
            CheckBox1.AutoSize = true;
            CheckBox1.Checked = true;
            CheckBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            CheckBox1.FlatAppearance.BorderSize = 0;
            CheckBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CheckBox1.Font = new Font("Segoe UI", 8.0f);
            CheckBox1.Margin = new System.Windows.Forms.Padding(12, 8, 4, 0);
            CheckBox1.Name = "CheckBox1";
            CheckBox1.Size = new Size(176, 17);
            CheckBox1.TabIndex = 50;
            CheckBox1.Text = "Expand Folders to list all files.";
            CheckBox1.UseVisualStyleBackColor = true;
            // 
            // FilterControlsFlowPanel
            // 
            FilterControlsFlowPanel.AutoSize = true;
            FilterControlsFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            FilterControlsFlowPanel.WrapContents = false;
            FilterControlsFlowPanel.Margin = new System.Windows.Forms.Padding(0);
            FilterControlsFlowPanel.Controls.Add(Label4);
            FilterControlsFlowPanel.Controls.Add(TextBox1);
            FilterControlsFlowPanel.Controls.Add(Button1);
            FilterControlsFlowPanel.Controls.Add(CheckBox1);
            FilterControlsFlowPanel.Name = "FilterControlsFlowPanel";
            // 
            // FilterRowPanel
            // 
            FilterRowPanel.AutoSize = true;
            FilterRowPanel.ColumnCount = 2;
            FilterRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            FilterRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            FilterRowPanel.Controls.Add(FilterControlsFlowPanel, 0, 0);
            FilterRowPanel.Controls.Add(Label6, 1, 0);
            FilterRowPanel.Dock = System.Windows.Forms.DockStyle.Top;
            FilterRowPanel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            FilterRowPanel.Name = "FilterRowPanel";
            FilterRowPanel.RowCount = 1;
            FilterRowPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // RootLayoutPanel
            // 
            RootLayoutPanel.ColumnCount = 1;
            RootLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            RootLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            RootLayoutPanel.Padding = new System.Windows.Forms.Padding(12, 9, 12, 9);
            RootLayoutPanel.RowCount = 2;
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            RootLayoutPanel.Controls.Add(FilterRowPanel, 0, 0);
            RootLayoutPanel.Controls.Add(dgLocations, 0, 1);
            RootLayoutPanel.Name = "RootLayoutPanel";
            // 
            // Locations
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(784, 361);
            Controls.Add(RootLayoutPanel);
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 8.0f);
            KeyPreview = true;
            MinimumSize = new Size(600, 200);
            Name = "Locations";
            ShowIcon = false;
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            Text = "Locations";
            ((System.ComponentModel.ISupportInitialize)dgLocations).EndInit();
            ContextMenuStrip1.ResumeLayout(false);
            RootLayoutPanel.ResumeLayout(false);
            RootLayoutPanel.PerformLayout();
            FilterRowPanel.ResumeLayout(false);
            FilterRowPanel.PerformLayout();
            FilterControlsFlowPanel.ResumeLayout(false);
            FilterControlsFlowPanel.PerformLayout();
            FormClosing += new System.Windows.Forms.FormClosingEventHandler(Locations_FormClosing);
            Load += new EventHandler(Locations_Load);
            ResumeLayout(false);
            PerformLayout();

        }
        internal System.Windows.Forms.DataGridView dgLocations;
        internal System.Windows.Forms.TextBox TextBox1;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Button Button1;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.ContextMenuStrip ContextMenuStrip1;
        internal System.Windows.Forms.ToolStripMenuItem CopyPathToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem CopyFileNameToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem OpenContainingFolderToolStripMenuItem;
        internal System.Windows.Forms.CheckBox CheckBox1;
        internal System.Windows.Forms.TableLayoutPanel RootLayoutPanel;
        internal System.Windows.Forms.TableLayoutPanel FilterRowPanel;
        internal System.Windows.Forms.FlowLayoutPanel FilterControlsFlowPanel;
    }
}
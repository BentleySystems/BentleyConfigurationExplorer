using System;
using System.Diagnostics;
using System.Drawing;

namespace WorkspaceCFG
{
    public partial class FileViewer : System.Windows.Forms.Form
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
            ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            ContextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(ContextMenuStrip1_Opening);
            OpenContainingFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            OpenContainingFolderToolStripMenuItem.Click += new EventHandler(OpenContainingFolderToolStripMenuItem_Click);
            ExportToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ExportToFileToolStripMenuItem.Click += new EventHandler(ExportToFileToolStripMenuItem_Click);
            OpenInEditorToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            OpenInEditorToolStripMenuItem1.Click += new EventHandler(OpenInEditorToolStripMenuItem1_Click);
            OpenVariableInDossierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            OpenVariableInDossierToolStripMenuItem.Click += new EventHandler(OpenVariableInDossierToolStripMenuItem_Click);
            MenuStrip1 = new System.Windows.Forms.MenuStrip();
            FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ExportToolStripMenuItem.Click += new EventHandler(ExportToolStripMenuItem_Click);
            OpenInEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            OpenInEditorToolStripMenuItem.Click += new EventHandler(OpenInEditorToolStripMenuItem_Click);
            ToolbarPanel = new System.Windows.Forms.TableLayoutPanel();
            ComboBox1 = new System.Windows.Forms.ComboBox();
            ComboBox1.SelectedIndexChanged += new EventHandler(ComboBox1_SelectedIndexChanged);
            CheckBox1 = new System.Windows.Forms.CheckBox();
            CheckBox1.CheckedChanged += new EventHandler(CheckBox1_CheckedChanged);
            RootLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            SaveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)DataGridView1).BeginInit();
            ContextMenuStrip1.SuspendLayout();
            MenuStrip1.SuspendLayout();
            ToolbarPanel.SuspendLayout();
            RootLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // DataGridView1
            // 
            DataGridView1.AllowUserToDeleteRows = false;
            DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            DataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            DataGridView1.BackgroundColor = Color.White;
            DataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            DataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridView1.ContextMenuStrip = ContextMenuStrip1;
            DataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            DataGridView1.GridColor = Color.LightSteelBlue;
            DataGridView1.Margin = new System.Windows.Forms.Padding(12, 8, 12, 12);
            DataGridView1.Name = "DataGridView1";
            DataGridView1.RowHeadersVisible = false;
            DataGridView1.RowTemplate.DefaultCellStyle.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            DataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            DataGridView1.RowTemplate.Height = 22;
            DataGridView1.RowTemplate.ReadOnly = true;
            DataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            DataGridView1.ShowRowErrors = false;
            DataGridView1.TabIndex = 35;
            // 
            // ContextMenuStrip1
            // 
            ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { OpenContainingFolderToolStripMenuItem, ExportToFileToolStripMenuItem, OpenInEditorToolStripMenuItem1, OpenVariableInDossierToolStripMenuItem });
            ContextMenuStrip1.Name = "ContextMenuStrip1";
            ContextMenuStrip1.Size = new Size(202, 92);
            // 
            // OpenContainingFolderToolStripMenuItem
            // 
            OpenContainingFolderToolStripMenuItem.Name = "OpenContainingFolderToolStripMenuItem";
            OpenContainingFolderToolStripMenuItem.Size = new Size(201, 22);
            OpenContainingFolderToolStripMenuItem.Text = "Open Containing Folder";
            // 
            // ExportToFileToolStripMenuItem
            // 
            ExportToFileToolStripMenuItem.Name = "ExportToFileToolStripMenuItem";
            ExportToFileToolStripMenuItem.Size = new Size(201, 22);
            ExportToFileToolStripMenuItem.Text = "Export to file";
            // 
            // OpenInEditorToolStripMenuItem1
            // 
            OpenInEditorToolStripMenuItem1.Name = "OpenInEditorToolStripMenuItem1";
            OpenInEditorToolStripMenuItem1.Size = new Size(201, 22);
            OpenInEditorToolStripMenuItem1.Text = "Open in Editor";
            // 
            // OpenVariableInDossierToolStripMenuItem
            // 
            OpenVariableInDossierToolStripMenuItem.Name = "OpenVariableInDossierToolStripMenuItem";
            OpenVariableInDossierToolStripMenuItem.Size = new Size(201, 22);
            OpenVariableInDossierToolStripMenuItem.Text = "Open Variable in Dossier";
            // 
            // MenuStrip1
            // 
            MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { FileToolStripMenuItem });
            MenuStrip1.Name = "MenuStrip1";
            MenuStrip1.TabIndex = 36;
            MenuStrip1.Text = "MenuStrip1";
            // 
            // FileToolStripMenuItem
            // 
            FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ExportToolStripMenuItem, OpenInEditorToolStripMenuItem });
            FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            FileToolStripMenuItem.Size = new Size(40, 20);
            FileToolStripMenuItem.Text = "File:";
            // 
            // ExportToolStripMenuItem
            // 
            ExportToolStripMenuItem.Name = "ExportToolStripMenuItem";
            ExportToolStripMenuItem.Size = new Size(150, 22);
            ExportToolStripMenuItem.Text = "Export";
            // 
            // OpenInEditorToolStripMenuItem
            // 
            OpenInEditorToolStripMenuItem.Name = "OpenInEditorToolStripMenuItem";
            OpenInEditorToolStripMenuItem.Size = new Size(150, 22);
            OpenInEditorToolStripMenuItem.Text = "Open in Editor";
            // 
            // ToolbarPanel
            // 
            ToolbarPanel.AutoSize = true;
            ToolbarPanel.ColumnCount = 2;
            ToolbarPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            ToolbarPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ToolbarPanel.Controls.Add(CheckBox1, 0, 0);
            ToolbarPanel.Controls.Add(ComboBox1, 1, 0);
            ToolbarPanel.Dock = System.Windows.Forms.DockStyle.Top;
            ToolbarPanel.Margin = new System.Windows.Forms.Padding(12, 8, 12, 8);
            ToolbarPanel.Name = "ToolbarPanel";
            ToolbarPanel.RowCount = 1;
            ToolbarPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // ComboBox1
            // 
            ComboBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ComboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            ComboBox1.FormattingEnabled = true;
            ComboBox1.Margin = new System.Windows.Forms.Padding(9, 0, 0, 0);
            ComboBox1.MaxDropDownItems = 60;
            ComboBox1.Name = "ComboBox1";
            ComboBox1.TabIndex = 37;
            // 
            // CheckBox1
            // 
            CheckBox1.AutoSize = true;
            CheckBox1.Font = new Font("Segoe UI", 8.0f);
            CheckBox1.Margin = new System.Windows.Forms.Padding(0);
            CheckBox1.Name = "CheckBox1";
            CheckBox1.TabIndex = 38;
            CheckBox1.Text = "Show Event Descriptions";
            CheckBox1.UseVisualStyleBackColor = true;
            // 
            // RootLayoutPanel
            // 
            RootLayoutPanel.ColumnCount = 1;
            RootLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            RootLayoutPanel.Controls.Add(ToolbarPanel, 0, 0);
            RootLayoutPanel.Controls.Add(DataGridView1, 0, 1);
            RootLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            RootLayoutPanel.Name = "RootLayoutPanel";
            RootLayoutPanel.RowCount = 2;
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            // 
            // FileViewer
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(784, 361);
            Controls.Add(RootLayoutPanel);
            Controls.Add(MenuStrip1);
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 8.0f);
            KeyPreview = true;
            MainMenuStrip = MenuStrip1;
            MinimumSize = new Size(600, 200);
            Name = "FileViewer";
            ShowIcon = false;
            Text = "FileViewer";
            ((System.ComponentModel.ISupportInitialize)DataGridView1).EndInit();
            ContextMenuStrip1.ResumeLayout(false);
            MenuStrip1.ResumeLayout(false);
            MenuStrip1.PerformLayout();
            ToolbarPanel.ResumeLayout(false);
            ToolbarPanel.PerformLayout();
            RootLayoutPanel.ResumeLayout(false);
            Activated += new EventHandler(FileViewer_Activated);
            Click += new EventHandler(FileViewer_Click);
            FormClosing += new System.Windows.Forms.FormClosingEventHandler(FileViewer_FormClosing);
            Load += new EventHandler(FileViewer_Load);
            KeyDown += new System.Windows.Forms.KeyEventHandler(WatchF5_KeyDown);
            ResumeLayout(false);
            PerformLayout();

        }
        internal System.Windows.Forms.DataGridView DataGridView1;
        internal System.Windows.Forms.MenuStrip MenuStrip1;
        internal System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ExportToolStripMenuItem;
        internal System.Windows.Forms.ComboBox ComboBox1;
        internal System.Windows.Forms.ToolStripMenuItem OpenInEditorToolStripMenuItem;
        internal System.Windows.Forms.CheckBox CheckBox1;
        internal System.Windows.Forms.SaveFileDialog SaveFileDialog1;
        internal System.Windows.Forms.ContextMenuStrip ContextMenuStrip1;
        internal System.Windows.Forms.ToolStripMenuItem OpenInEditorToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripMenuItem ExportToFileToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem OpenContainingFolderToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem OpenVariableInDossierToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel ToolbarPanel;
        private System.Windows.Forms.TableLayoutPanel RootLayoutPanel;
    }
}
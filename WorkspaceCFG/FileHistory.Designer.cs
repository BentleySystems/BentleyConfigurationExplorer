using System;
using System.Diagnostics;
using System.Drawing;

namespace WorkspaceCFG
{
    public partial class FileHistory : System.Windows.Forms.Form
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
            this.ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.OpenInEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenContainingFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenAllInEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenIncludeEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowEventsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label9 = new System.Windows.Forms.Label();
            this.Label11 = new System.Windows.Forms.Label();
            this.LinkLabel1 = new System.Windows.Forms.LinkLabel();
            this.LinkLabel2 = new System.Windows.Forms.LinkLabel();
            this.LinkLabel3 = new System.Windows.Forms.LinkLabel();
            this.Label7 = new System.Windows.Forms.Label();
            this.gbCFGStats = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.LinkLabel4 = new System.Windows.Forms.LinkLabel();
            this.Label13 = new System.Windows.Forms.Label();
            this.Label12 = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.gbModifedAgo = new System.Windows.Forms.GroupBox();
            this.hsModifedAgo = new System.Windows.Forms.HScrollBar();
            this.lblModifiedAgo = new System.Windows.Forms.Label();
            this.TreeView1 = new System.Windows.Forms.TreeView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRescan = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnOpenTreeViewer = new System.Windows.Forms.Button();
            this.LineNumberPictureBox = new System.Windows.Forms.PictureBox();
            this.ConfigFileRTB = new WorkspaceCFG.CFGRichTextBox();
            this.ContextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.CopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyExpandedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleCommentBlockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ExpandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.CI1 = new System.Windows.Forms.ToolStripMenuItem();
            this.CI2 = new System.Windows.Forms.ToolStripMenuItem();
            this.CI3 = new System.Windows.Forms.ToolStripMenuItem();
            this.CI4 = new System.Windows.Forms.ToolStripMenuItem();
            this.CI5 = new System.Windows.Forms.ToolStripMenuItem();
            this.CI6 = new System.Windows.Forms.ToolStripMenuItem();
            this.Label5 = new System.Windows.Forms.Label();
            this.LineDescriptionRTB = new System.Windows.Forms.RichTextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.SaveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.gbConfiguration = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SplitContainer1 = new System.Windows.Forms.SplitContainer();
            this.LeftPanel = new System.Windows.Forms.TableLayoutPanel();
            this.RightPanel = new System.Windows.Forms.TableLayoutPanel();
            this.HeaderFlow = new System.Windows.Forms.FlowLayoutPanel();
            this.EditorPanel = new System.Windows.Forms.TableLayoutPanel();
            this.StatsGrid = new System.Windows.Forms.TableLayoutPanel();
            this.ContextMenuStrip1.SuspendLayout();
            this.gbCFGStats.SuspendLayout();
            this.gbModifedAgo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LineNumberPictureBox)).BeginInit();
            this.ContextMenuStrip3.SuspendLayout();
            this.gbConfiguration.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).BeginInit();
            this.SplitContainer1.Panel1.SuspendLayout();
            this.SplitContainer1.Panel2.SuspendLayout();
            this.SplitContainer1.SuspendLayout();
            this.LeftPanel.SuspendLayout();
            this.RightPanel.SuspendLayout();
            this.HeaderFlow.SuspendLayout();
            this.EditorPanel.SuspendLayout();
            this.StatsGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // ContextMenuStrip1
            // 
            this.ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenInEditorToolStripMenuItem,
            this.OpenContainingFolderToolStripMenuItem,
            this.OpenAllInEditorToolStripMenuItem,
            this.OpenIncludeEventToolStripMenuItem,
            this.ShowEventsToolStripMenuItem,
            this.ExportToolStripMenuItem});
            this.ContextMenuStrip1.Name = "ContextMenuStrip1";
            this.ContextMenuStrip1.Size = new System.Drawing.Size(202, 136);
            this.ContextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip1_Opening);
            // 
            // OpenInEditorToolStripMenuItem
            // 
            this.OpenInEditorToolStripMenuItem.Name = "OpenInEditorToolStripMenuItem";
            this.OpenInEditorToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.OpenInEditorToolStripMenuItem.Text = "Open in Editor";
            this.OpenInEditorToolStripMenuItem.Click += new System.EventHandler(this.OpenInEditorToolStripMenuItem_Click);
            // 
            // OpenContainingFolderToolStripMenuItem
            // 
            this.OpenContainingFolderToolStripMenuItem.Name = "OpenContainingFolderToolStripMenuItem";
            this.OpenContainingFolderToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.OpenContainingFolderToolStripMenuItem.Text = "Open Containing Folder";
            this.OpenContainingFolderToolStripMenuItem.Click += new System.EventHandler(this.OpenContainingFolderToolStripMenuItem_Click);
            // 
            // OpenAllInEditorToolStripMenuItem
            // 
            this.OpenAllInEditorToolStripMenuItem.Name = "OpenAllInEditorToolStripMenuItem";
            this.OpenAllInEditorToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.OpenAllInEditorToolStripMenuItem.Text = "Open All in Editor";
            this.OpenAllInEditorToolStripMenuItem.Click += new System.EventHandler(this.OpenAllInEditorToolStripMenuItem_Click);
            // 
            // OpenIncludeEventToolStripMenuItem
            // 
            this.OpenIncludeEventToolStripMenuItem.Name = "OpenIncludeEventToolStripMenuItem";
            this.OpenIncludeEventToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.OpenIncludeEventToolStripMenuItem.Text = "Open Include Event";
            this.OpenIncludeEventToolStripMenuItem.Click += new System.EventHandler(this.OpenIncludeEventToolStripMenuItem_Click);
            // 
            // ShowEventsToolStripMenuItem
            // 
            this.ShowEventsToolStripMenuItem.Name = "ShowEventsToolStripMenuItem";
            this.ShowEventsToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.ShowEventsToolStripMenuItem.Text = "Show Events";
            this.ShowEventsToolStripMenuItem.Click += new System.EventHandler(this.ShowEventsToolStripMenuItem_Click);
            // 
            // ExportToolStripMenuItem
            // 
            this.ExportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToExcelToolStripMenuItem,
            this.ToTextToolStripMenuItem,
            this.ToImageToolStripMenuItem});
            this.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem";
            this.ExportToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.ExportToolStripMenuItem.Text = "Export";
            // 
            // ToExcelToolStripMenuItem
            // 
            this.ToExcelToolStripMenuItem.Name = "ToExcelToolStripMenuItem";
            this.ToExcelToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.ToExcelToolStripMenuItem.Text = "To Excel";
            this.ToExcelToolStripMenuItem.Click += new System.EventHandler(this.ToExcelToolStripMenuItem_Click);
            // 
            // ToTextToolStripMenuItem
            // 
            this.ToTextToolStripMenuItem.Name = "ToTextToolStripMenuItem";
            this.ToTextToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.ToTextToolStripMenuItem.Text = "To Text";
            this.ToTextToolStripMenuItem.Click += new System.EventHandler(this.ToTextToolStripMenuItem_Click);
            // 
            // ToImageToolStripMenuItem
            // 
            this.ToImageToolStripMenuItem.Name = "ToImageToolStripMenuItem";
            this.ToImageToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.ToImageToolStripMenuItem.Text = "To Image";
            this.ToImageToolStripMenuItem.Click += new System.EventHandler(this.ToImageToolStripMenuItem_Click);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Label2.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(127, 13);
            this.Label2.TabIndex = 41;
            this.Label2.Text = "File:";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.BackColor = System.Drawing.Color.Transparent;
            this.Label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Label3.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(127, 13);
            this.Label3.TabIndex = 42;
            this.Label3.Text = "Usable Lines:";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.BackColor = System.Drawing.Color.Transparent;
            this.Label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Label4.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(127, 13);
            this.Label4.TabIndex = 44;
            this.Label4.Text = "Warnings:";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.BackColor = System.Drawing.Color.Transparent;
            this.Label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Label6.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(127, 13);
            this.Label6.TabIndex = 43;
            this.Label6.Text = "Errors:";
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.BackColor = System.Drawing.Color.Transparent;
            this.Label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Label9.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label9.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(881, 15);
            this.Label9.TabIndex = 46;
            this.Label9.Text = "100";
            // 
            // Label11
            // 
            this.Label11.AutoSize = true;
            this.Label11.BackColor = System.Drawing.Color.Transparent;
            this.Label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Label11.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label11.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(127, 13);
            this.Label11.TabIndex = 49;
            this.Label11.Text = "Last Modified Date:";
            // 
            // LinkLabel1
            // 
            this.LinkLabel1.AutoSize = true;
            this.LinkLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LinkLabel1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.LinkLabel1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.LinkLabel1.Name = "LinkLabel1";
            this.LinkLabel1.Size = new System.Drawing.Size(879, 17);
            this.LinkLabel1.TabIndex = 50;
            this.LinkLabel1.TabStop = true;
            this.LinkLabel1.Text = "LinkLabel1";
            this.LinkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked);
            this.LinkLabel1.SizeChanged += new System.EventHandler(this.LinkLabel1_SizeChanged);
            // 
            // LinkLabel2
            // 
            this.LinkLabel2.AutoSize = true;
            this.LinkLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LinkLabel2.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinkLabel2.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.LinkLabel2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.LinkLabel2.Name = "LinkLabel2";
            this.LinkLabel2.Size = new System.Drawing.Size(879, 15);
            this.LinkLabel2.TabIndex = 51;
            this.LinkLabel2.TabStop = true;
            this.LinkLabel2.Text = "LinkLabel2";
            this.LinkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel2_LinkClicked);
            // 
            // LinkLabel3
            // 
            this.LinkLabel3.AutoSize = true;
            this.LinkLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LinkLabel3.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinkLabel3.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.LinkLabel3.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.LinkLabel3.Name = "LinkLabel3";
            this.LinkLabel3.Size = new System.Drawing.Size(879, 15);
            this.LinkLabel3.TabIndex = 52;
            this.LinkLabel3.TabStop = true;
            this.LinkLabel3.Text = "LinkLabel3";
            this.LinkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel3_LinkClicked);
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.BackColor = System.Drawing.Color.Transparent;
            this.Label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Label7.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label7.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(879, 18);
            this.Label7.TabIndex = 53;
            this.Label7.Text = "6/25/2009 4:31 PM";
            // 
            // gbCFGStats
            // 
            this.gbCFGStats.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.gbCFGStats.Controls.Add(this.StatsGrid);
            this.gbCFGStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbCFGStats.Name = "gbCFGStats";
            this.gbCFGStats.Size = new System.Drawing.Size(1019, 210);
            this.gbCFGStats.TabIndex = 54;
            this.gbCFGStats.TabStop = false;
            // 
            // StatsGrid
            // 
            this.StatsGrid.ColumnCount = 2;
            this.StatsGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.StatsGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.StatsGrid.Controls.Add(this.label10, 0, 0);
            this.StatsGrid.Controls.Add(this.Label2, 0, 1);
            this.StatsGrid.Controls.Add(this.LinkLabel1, 1, 1);
            this.StatsGrid.Controls.Add(this.Label8, 0, 2);
            this.StatsGrid.Controls.Add(this.LinkLabel4, 1, 2);
            this.StatsGrid.Controls.Add(this.Label12, 0, 3);
            this.StatsGrid.Controls.Add(this.Label13, 1, 3);
            this.StatsGrid.Controls.Add(this.Label3, 0, 4);
            this.StatsGrid.Controls.Add(this.Label9, 1, 4);
            this.StatsGrid.Controls.Add(this.Label6, 0, 5);
            this.StatsGrid.Controls.Add(this.LinkLabel2, 1, 5);
            this.StatsGrid.Controls.Add(this.Label4, 0, 6);
            this.StatsGrid.Controls.Add(this.LinkLabel3, 1, 6);
            this.StatsGrid.Controls.Add(this.Label11, 0, 7);
            this.StatsGrid.Controls.Add(this.Label7, 1, 7);
            this.StatsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StatsGrid.Name = "StatsGrid";
            this.StatsGrid.Padding = new System.Windows.Forms.Padding(3);
            this.StatsGrid.RowCount = 8;
            this.StatsGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.StatsGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.StatsGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.StatsGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.StatsGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.StatsGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.StatsGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.StatsGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.StatsGrid.Size = new System.Drawing.Size(1019, 210);
            this.StatsGrid.TabIndex = 60;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label10.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(270, 27);
            this.label10.TabIndex = 59;
            this.label10.Text = "File Statistics:";
            this.StatsGrid.SetColumnSpan(this.label10, 2);
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // LinkLabel4
            // 
            this.LinkLabel4.AutoSize = true;
            this.LinkLabel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LinkLabel4.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinkLabel4.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.LinkLabel4.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.LinkLabel4.Name = "LinkLabel4";
            this.LinkLabel4.Size = new System.Drawing.Size(879, 17);
            this.LinkLabel4.TabIndex = 58;
            this.LinkLabel4.TabStop = true;
            this.LinkLabel4.Text = "LinkLabel4";
            this.LinkLabel4.SizeChanged += new System.EventHandler(this.LinkLabel4_SizeChanged);
            // 
            // Label13
            // 
            this.Label13.AutoSize = true;
            this.Label13.BackColor = System.Drawing.Color.Transparent;
            this.Label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Label13.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label13.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(879, 14);
            this.Label13.TabIndex = 57;
            this.Label13.Text = "LEVEL";
            // 
            // Label12
            // 
            this.Label12.AutoSize = true;
            this.Label12.BackColor = System.Drawing.Color.Transparent;
            this.Label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Label12.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label12.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(127, 13);
            this.Label12.TabIndex = 56;
            this.Label12.Text = "Starting Level:";
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.BackColor = System.Drawing.Color.Transparent;
            this.Label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Label8.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(127, 13);
            this.Label8.TabIndex = 54;
            this.Label8.Text = "Parent File:";
            // 
            // gbModifedAgo
            // 
            this.gbModifedAgo.Controls.Add(this.hsModifedAgo);
            this.gbModifedAgo.Controls.Add(this.lblModifiedAgo);
            this.gbModifedAgo.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbModifedAgo.Name = "gbModifedAgo";
            this.gbModifedAgo.Size = new System.Drawing.Size(344, 64);
            this.gbModifedAgo.TabIndex = 1;
            this.gbModifedAgo.TabStop = false;
            // 
            // hsModifedAgo
            // 
            this.hsModifedAgo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hsModifedAgo.Maximum = 200;
            this.hsModifedAgo.Name = "hsModifedAgo";
            this.hsModifedAgo.Size = new System.Drawing.Size(344, 18);
            this.hsModifedAgo.TabIndex = 56;
            this.hsModifedAgo.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HScrollBar1_Scroll);
            // 
            // lblModifiedAgo
            // 
            this.lblModifiedAgo.BackColor = System.Drawing.Color.Transparent;
            this.lblModifiedAgo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblModifiedAgo.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblModifiedAgo.Name = "lblModifiedAgo";
            this.lblModifiedAgo.Size = new System.Drawing.Size(350, 32);
            this.lblModifiedAgo.TabIndex = 56;
            this.lblModifiedAgo.Text = "Highlite files modified in the last 12 hours:";
            // 
            // TreeView1
            // 
            this.TreeView1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.TreeView1.CausesValidation = false;
            this.TreeView1.ContextMenuStrip = this.ContextMenuStrip1;
            this.TreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeView1.HideSelection = false;
            this.TreeView1.Name = "TreeView1";
            this.TreeView1.Size = new System.Drawing.Size(341, 918);
            this.TreeView1.TabIndex = 0;
            this.TreeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView1_AfterSelect);
            this.TreeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView1_NodeMouseClick);
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(9, 3, 3, 3);
            this.btnCancel.MinimumSize = new System.Drawing.Size(85, 38);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.btnCancel.TabIndex = 61;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.Button3_Click);
            // 
            // btnRescan
            // 
            this.btnRescan.AutoSize = true;
            this.btnRescan.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRescan.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRescan.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.btnRescan.MinimumSize = new System.Drawing.Size(151, 38);
            this.btnRescan.Name = "btnRescan";
            this.btnRescan.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.btnRescan.TabIndex = 60;
            this.btnRescan.Text = "Rescan Workspace";
            this.btnRescan.UseVisualStyleBackColor = true;
            this.btnRescan.Click += new System.EventHandler(this.Button2_Click);
            // 
            // btnSave
            // 
            this.btnSave.AutoSize = true;
            this.btnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.btnSave.MinimumSize = new System.Drawing.Size(95, 38);
            this.btnSave.Name = "btnSave";
            this.btnSave.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.btnSave.TabIndex = 59;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.Button1_Click);
            // 
            // btnOpenTreeViewer
            // 
            this.btnOpenTreeViewer.AutoSize = true;
            this.btnOpenTreeViewer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOpenTreeViewer.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnOpenTreeViewer.Margin = new System.Windows.Forms.Padding(9, 3, 3, 3);
            this.btnOpenTreeViewer.MinimumSize = new System.Drawing.Size(151, 38);
            this.btnOpenTreeViewer.Name = "btnOpenTreeViewer";
            this.btnOpenTreeViewer.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.btnOpenTreeViewer.TabIndex = 62;
            this.btnOpenTreeViewer.Text = "CFG Tree";
            this.btnOpenTreeViewer.UseVisualStyleBackColor = true;
            this.btnOpenTreeViewer.Click += new System.EventHandler(this.btnOpenTreeViewer_Click);
            // 
            // LineNumberPictureBox
            // 
            this.LineNumberPictureBox.BackColor = System.Drawing.Color.White;
            this.LineNumberPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LineNumberPictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.LineNumberPictureBox.Name = "LineNumberPictureBox";
            this.LineNumberPictureBox.Size = new System.Drawing.Size(54, 504);
            this.LineNumberPictureBox.TabIndex = 43;
            this.LineNumberPictureBox.TabStop = false;
            this.LineNumberPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.P_Paint);
            // 
            // ConfigFileRTB
            // 
            this.ConfigFileRTB.AcceptsTab = true;
            this.ConfigFileRTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ConfigFileRTB.ContextMenuStrip = this.ContextMenuStrip3;
            this.ConfigFileRTB.DetectUrls = false;
            this.ConfigFileRTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConfigFileRTB.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.ConfigFileRTB.HideSelection = false;
            this.ConfigFileRTB.Name = "ConfigFileRTB";
            this.ConfigFileRTB.ShortcutsEnabled = false;
            this.ConfigFileRTB.Size = new System.Drawing.Size(957, 504);
            this.ConfigFileRTB.TabIndex = 42;
            this.ConfigFileRTB.Text = "";
            this.ConfigFileRTB.WordWrap = false;
            this.ConfigFileRTB.VScroll += new System.EventHandler(this.R_VScroll);
            this.ConfigFileRTB.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RichTextBox1_MouseClick);
            this.ConfigFileRTB.ReadOnlyChanged += new System.EventHandler(this.RichTextBox1_ReadOnlyChanged);
            this.ConfigFileRTB.TextChanged += new System.EventHandler(this.ConfigFileRTB_TextChanged);
            this.ConfigFileRTB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ConfigFileRTB_KeyUp);
            this.ConfigFileRTB.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.RichTextBox1_MouseDoubleClick);
            this.ConfigFileRTB.Resize += new System.EventHandler(this.R_Resize);
            // 
            // ContextMenuStrip3
            // 
            this.ContextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem3,
            this.ToolStripMenuItem1,
            this.ToolStripSeparator1,
            this.CopyToolStripMenuItem,
            this.CopyExpandedToolStripMenuItem,
            this.PasteToolStripMenuItem,
            this.ToggleCommentBlockToolStripMenuItem,
            this.ToolStripMenuItem4,
            this.ToolStripSeparator2,
            this.ExpandToolStripMenuItem,
            this.ToolStripSeparator3,
            this.CI1,
            this.CI2,
            this.CI3,
            this.CI4,
            this.CI5,
            this.CI6});
            this.ContextMenuStrip3.Name = "ContextMenuStrip1";
            this.ContextMenuStrip3.Size = new System.Drawing.Size(202, 330);
            this.ContextMenuStrip3.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip3_Opening);
            // 
            // ToolStripMenuItem3
            // 
            this.ToolStripMenuItem3.Name = "ToolStripMenuItem3";
            this.ToolStripMenuItem3.Size = new System.Drawing.Size(201, 22);
            this.ToolStripMenuItem3.Text = "Open in Editor";
            this.ToolStripMenuItem3.Click += new System.EventHandler(this.ToolStripMenuItem3_Click);
            // 
            // ToolStripMenuItem1
            // 
            this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
            this.ToolStripMenuItem1.Size = new System.Drawing.Size(201, 22);
            this.ToolStripMenuItem1.Text = "Open Containing Folder";
            this.ToolStripMenuItem1.Click += new System.EventHandler(this.ToolStripMenuItem1_Click);
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(198, 6);
            // 
            // CopyToolStripMenuItem
            // 
            this.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem";
            this.CopyToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.CopyToolStripMenuItem.Text = "Copy";
            this.CopyToolStripMenuItem.Click += new System.EventHandler(this.CopyToolStripMenuItem_Click);
            // 
            // CopyExpandedToolStripMenuItem
            // 
            this.CopyExpandedToolStripMenuItem.Name = "CopyExpandedToolStripMenuItem";
            this.CopyExpandedToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.CopyExpandedToolStripMenuItem.Text = "Copy Expanded";
            this.CopyExpandedToolStripMenuItem.Click += new System.EventHandler(this.CopyExpandedToolStripMenuItem_Click);
            // 
            // PasteToolStripMenuItem
            // 
            this.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem";
            this.PasteToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.PasteToolStripMenuItem.Text = "Paste";
            this.PasteToolStripMenuItem.Click += new System.EventHandler(this.PasteToolStripMenuItem_Click);
            // 
            // ToggleCommentBlockToolStripMenuItem
            // 
            this.ToggleCommentBlockToolStripMenuItem.Name = "ToggleCommentBlockToolStripMenuItem";
            this.ToggleCommentBlockToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.ToggleCommentBlockToolStripMenuItem.Text = "Toggle Comment Block";
            this.ToggleCommentBlockToolStripMenuItem.Click += new System.EventHandler(this.ToggleCommentBlockToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem4
            // 
            this.ToolStripMenuItem4.Name = "ToolStripMenuItem4";
            this.ToolStripMenuItem4.Size = new System.Drawing.Size(201, 22);
            this.ToolStripMenuItem4.Text = "Auto Format";
            this.ToolStripMenuItem4.Click += new System.EventHandler(this.ToolStripMenuItem4_Click);
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(198, 6);
            // 
            // ExpandToolStripMenuItem
            // 
            this.ExpandToolStripMenuItem.Name = "ExpandToolStripMenuItem";
            this.ExpandToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.ExpandToolStripMenuItem.Text = "Expand";
            this.ExpandToolStripMenuItem.Click += new System.EventHandler(this.ExpandToolStripMenuItem_Click);
            // 
            // ToolStripSeparator3
            // 
            this.ToolStripSeparator3.Name = "ToolStripSeparator3";
            this.ToolStripSeparator3.Size = new System.Drawing.Size(198, 6);
            // 
            // CI1
            // 
            this.CI1.Name = "CI1";
            this.CI1.Size = new System.Drawing.Size(201, 22);
            this.CI1.Text = "Item 1";
            this.CI1.Click += new System.EventHandler(this.CI1_Click);
            // 
            // CI2
            // 
            this.CI2.Name = "CI2";
            this.CI2.Size = new System.Drawing.Size(201, 22);
            this.CI2.Text = "Item 2";
            this.CI2.Click += new System.EventHandler(this.CI2_Click);
            // 
            // CI3
            // 
            this.CI3.Name = "CI3";
            this.CI3.Size = new System.Drawing.Size(201, 22);
            this.CI3.Text = "Item 3";
            this.CI3.Click += new System.EventHandler(this.CI3_Click);
            // 
            // CI4
            // 
            this.CI4.Name = "CI4";
            this.CI4.Size = new System.Drawing.Size(201, 22);
            this.CI4.Text = "Item 4";
            this.CI4.Click += new System.EventHandler(this.CI4_Click);
            // 
            // CI5
            // 
            this.CI5.Name = "CI5";
            this.CI5.Size = new System.Drawing.Size(201, 22);
            this.CI5.Text = "Item 5";
            this.CI5.Click += new System.EventHandler(this.CI5_Click);
            // 
            // CI6
            // 
            this.CI6.Name = "CI6";
            this.CI6.Size = new System.Drawing.Size(201, 22);
            this.CI6.Text = "Item 6";
            this.CI6.Click += new System.EventHandler(this.CI6_Click);
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.BackColor = System.Drawing.Color.Transparent;
            this.Label5.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.Label5.Margin = new System.Windows.Forms.Padding(3, 12, 9, 3);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(239, 19);
            this.Label5.TabIndex = 41;
            this.Label5.Text = "Configuration File:";
            // 
            // LineDescriptionRTB
            // 
            this.LineDescriptionRTB.BackColor = System.Drawing.SystemColors.Info;
            this.LineDescriptionRTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LineDescriptionRTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LineDescriptionRTB.ForeColor = System.Drawing.Color.Black;
            this.LineDescriptionRTB.HideSelection = false;
            this.LineDescriptionRTB.Margin = new System.Windows.Forms.Padding(5);
            this.LineDescriptionRTB.Name = "LineDescriptionRTB";
            this.LineDescriptionRTB.Size = new System.Drawing.Size(1019, 164);
            this.LineDescriptionRTB.TabIndex = 58;
            this.LineDescriptionRTB.Text = "Events";
            this.LineDescriptionRTB.WordWrap = false;
            this.LineDescriptionRTB.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.RichTextBox2_LinkClicked);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Label1.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(142, 27);
            this.Label1.TabIndex = 39;
            this.Label1.Text = "Line Description:";
            // 
            // gbConfiguration
            // 
            this.gbConfiguration.Controls.Add(this.HeaderFlow);
            this.gbConfiguration.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbConfiguration.Name = "gbConfiguration";
            this.gbConfiguration.Size = new System.Drawing.Size(1016, 60);
            this.gbConfiguration.TabIndex = 0;
            this.gbConfiguration.TabStop = false;
            // 
            // HeaderFlow
            // 
            this.HeaderFlow.AutoSize = true;
            this.HeaderFlow.Controls.Add(this.Label5);
            this.HeaderFlow.Controls.Add(this.btnOpenTreeViewer);
            this.HeaderFlow.Controls.Add(this.btnRescan);
            this.HeaderFlow.Controls.Add(this.btnSave);
            this.HeaderFlow.Controls.Add(this.btnCancel);
            this.HeaderFlow.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeaderFlow.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.HeaderFlow.Name = "HeaderFlow";
            this.HeaderFlow.Size = new System.Drawing.Size(1016, 44);
            this.HeaderFlow.TabIndex = 1;
            this.HeaderFlow.WrapContents = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1019, 46);
            this.groupBox1.TabIndex = 63;
            this.groupBox1.TabStop = false;
            // 
            // SplitContainer1
            // 
            this.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer1.Name = "SplitContainer1";
            // 
            // SplitContainer1.Panel1
            // 
            this.SplitContainer1.Panel1.Controls.Add(this.LeftPanel);
            // 
            // SplitContainer1.Panel2
            // 
            this.SplitContainer1.Panel2.Controls.Add(this.RightPanel);
            this.SplitContainer1.Size = new System.Drawing.Size(1372, 1021);
            this.SplitContainer1.SplitterDistance = 350;
            this.SplitContainer1.TabIndex = 64;
            // 
            // LeftPanel
            // 
            this.LeftPanel.ColumnCount = 1;
            this.LeftPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LeftPanel.Controls.Add(this.gbModifedAgo, 0, 0);
            this.LeftPanel.Controls.Add(this.TreeView1, 0, 1);
            this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Padding = new System.Windows.Forms.Padding(3);
            this.LeftPanel.RowCount = 2;
            this.LeftPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.LeftPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LeftPanel.Size = new System.Drawing.Size(350, 1021);
            this.LeftPanel.TabIndex = 0;
            // 
            // RightPanel
            // 
            this.RightPanel.ColumnCount = 1;
            this.RightPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RightPanel.Controls.Add(this.gbCFGStats, 0, 0);
            this.RightPanel.Controls.Add(this.gbConfiguration, 0, 1);
            this.RightPanel.Controls.Add(this.EditorPanel, 0, 2);
            this.RightPanel.Controls.Add(this.groupBox1, 0, 3);
            this.RightPanel.Controls.Add(this.LineDescriptionRTB, 0, 4);
            this.RightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Padding = new System.Windows.Forms.Padding(3);
            this.RightPanel.RowCount = 5;
            this.RightPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.RightPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.RightPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.RightPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.RightPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.RightPanel.Size = new System.Drawing.Size(1018, 1021);
            this.RightPanel.TabIndex = 0;
            // 
            // EditorPanel
            // 
            this.EditorPanel.ColumnCount = 2;
            this.EditorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.EditorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.EditorPanel.Controls.Add(this.LineNumberPictureBox, 0, 0);
            this.EditorPanel.Controls.Add(this.ConfigFileRTB, 1, 0);
            this.EditorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditorPanel.Name = "EditorPanel";
            this.EditorPanel.RowCount = 1;
            this.EditorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.EditorPanel.Size = new System.Drawing.Size(1018, 561);
            this.EditorPanel.TabIndex = 1;
            // 
            // FileHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1378, 1027);
            this.Controls.Add(this.SplitContainer1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(900, 1030);
            this.Name = "FileHistory";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.ShowIcon = false;
            this.Text = "File History";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FileHistory_FormClosing);
            this.Load += new System.EventHandler(this.FileHistory_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WatchF5_KeyDown);
            this.ContextMenuStrip1.ResumeLayout(false);
            this.gbCFGStats.ResumeLayout(false);
            this.gbModifedAgo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LineNumberPictureBox)).EndInit();
            this.ContextMenuStrip3.ResumeLayout(false);
            this.gbConfiguration.ResumeLayout(false);
            this.gbConfiguration.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.SplitContainer1.Panel1.ResumeLayout(false);
            this.SplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).EndInit();
            this.SplitContainer1.ResumeLayout(false);
            this.LeftPanel.ResumeLayout(false);
            this.LeftPanel.PerformLayout();
            this.RightPanel.ResumeLayout(false);
            this.RightPanel.PerformLayout();
            this.HeaderFlow.ResumeLayout(false);
            this.HeaderFlow.PerformLayout();
            this.EditorPanel.ResumeLayout(false);
            this.StatsGrid.ResumeLayout(false);
            this.StatsGrid.PerformLayout();
            this.ResumeLayout(false);

        }
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.Label Label11;
        internal System.Windows.Forms.LinkLabel LinkLabel1;
        internal System.Windows.Forms.LinkLabel LinkLabel2;
        internal System.Windows.Forms.LinkLabel LinkLabel3;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.GroupBox gbCFGStats;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.ContextMenuStrip ContextMenuStrip1;
        internal System.Windows.Forms.ToolStripMenuItem OpenContainingFolderToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem OpenInEditorToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem OpenIncludeEventToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ShowEventsToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ExportToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ToExcelToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ToTextToolStripMenuItem;
        internal System.Windows.Forms.SaveFileDialog SaveFileDialog1;
        internal System.Windows.Forms.ToolStripMenuItem OpenAllInEditorToolStripMenuItem;
        internal System.Windows.Forms.GroupBox gbModifedAgo;
        internal System.Windows.Forms.Label lblModifiedAgo;
        internal System.Windows.Forms.HScrollBar hsModifedAgo;
        internal System.Windows.Forms.ToolStripMenuItem ToImageToolStripMenuItem;
        internal System.Windows.Forms.TreeView TreeView1;
        internal System.Windows.Forms.Label Label13;
        internal System.Windows.Forms.Label Label12;
        internal System.Windows.Forms.Label Label5;
        internal CFGRichTextBox ConfigFileRTB;
        internal System.Windows.Forms.RichTextBox LineDescriptionRTB;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.ContextMenuStrip ContextMenuStrip3;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem3;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1;
        internal System.Windows.Forms.LinkLabel LinkLabel4;
        internal System.Windows.Forms.PictureBox LineNumberPictureBox;
        internal System.Windows.Forms.ToolStripMenuItem CopyToolStripMenuItem;
        internal System.Windows.Forms.Button btnRescan;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.Button btnOpenTreeViewer;
        internal System.Windows.Forms.ToolStripMenuItem ToggleCommentBlockToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem4;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
        internal System.Windows.Forms.ToolStripMenuItem CI1;
        internal System.Windows.Forms.ToolStripMenuItem CI2;
        internal System.Windows.Forms.ToolStripMenuItem CI3;
        internal System.Windows.Forms.ToolStripMenuItem CI4;
        internal System.Windows.Forms.ToolStripMenuItem CI5;
        internal System.Windows.Forms.ToolStripMenuItem CI6;
        internal System.Windows.Forms.ToolStripMenuItem ExpandToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator3;
        internal System.Windows.Forms.ToolStripMenuItem CopyExpandedToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem PasteToolStripMenuItem;

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Implement paste functionality as needed
        }

        private System.Windows.Forms.GroupBox gbConfiguration;
        private System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.Label label10;
        internal System.Windows.Forms.SplitContainer SplitContainer1;
        internal System.Windows.Forms.TableLayoutPanel LeftPanel;
        internal System.Windows.Forms.TableLayoutPanel RightPanel;
        internal System.Windows.Forms.FlowLayoutPanel HeaderFlow;
        internal System.Windows.Forms.TableLayoutPanel EditorPanel;
        internal System.Windows.Forms.TableLayoutPanel StatsGrid;
    }
}
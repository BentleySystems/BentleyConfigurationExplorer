using System;
using System.Diagnostics;
using System.Drawing;

namespace WorkspaceCFG
{
    public partial class VariableDossier : System.Windows.Forms.Form
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
            this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.HeaderTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.InfoTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Label8 = new System.Windows.Forms.Label();
            this.Label9 = new System.Windows.Forms.Label();
            this.Label11 = new System.Windows.Forms.Label();
            this.Label10 = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label13 = new System.Windows.Forms.Label();
            this.Label12 = new System.Windows.Forms.Label();
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.HeaderSeparator = new System.Windows.Forms.Label();
            this.CurrentValueGroupBox = new System.Windows.Forms.GroupBox();
            this.DataGridView1 = new System.Windows.Forms.DataGridView();
            this.ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewDefinitionInEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HistoryGroupBox = new System.Windows.Forms.GroupBox();
            this.DataGridView2 = new System.Windows.Forms.DataGridView();
            this.ContextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.GoToLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFileInEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFileInViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewLocationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToTextFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RelationsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ParentsGroupBox = new System.Windows.Forms.GroupBox();
            this.DataGridView3 = new System.Windows.Forms.DataGridView();
            this.ChildrenGroupBox = new System.Windows.Forms.GroupBox();
            this.DataGridView4 = new System.Windows.Forms.DataGridView();
            this.RichTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SaveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.MainTableLayoutPanel.SuspendLayout();
            this.HeaderTableLayoutPanel.SuspendLayout();
            this.InfoTableLayoutPanel.SuspendLayout();
            this.CurrentValueGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView1)).BeginInit();
            this.ContextMenuStrip1.SuspendLayout();
            this.HistoryGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView2)).BeginInit();
            this.ContextMenuStrip2.SuspendLayout();
            this.RelationsTableLayoutPanel.SuspendLayout();
            this.ParentsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView3)).BeginInit();
            this.ChildrenGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView4)).BeginInit();
            this.SuspendLayout();
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.ColumnCount = 1;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Controls.Add(this.HeaderTableLayoutPanel, 0, 0);
            this.MainTableLayoutPanel.Controls.Add(this.CurrentValueGroupBox, 0, 1);
            this.MainTableLayoutPanel.Controls.Add(this.HistoryGroupBox, 0, 2);
            this.MainTableLayoutPanel.Controls.Add(this.RelationsTableLayoutPanel, 0, 3);
            this.MainTableLayoutPanel.Controls.Add(this.RichTextBox1, 0, 4);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.Padding = new System.Windows.Forms.Padding(10);
            this.MainTableLayoutPanel.RowCount = 5;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 105F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 175F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 145F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(1010, 1050);
            this.MainTableLayoutPanel.TabIndex = 0;
            // 
            // HeaderTableLayoutPanel
            // 
            this.HeaderTableLayoutPanel.ColumnCount = 2;
            this.HeaderTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.HeaderTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.HeaderTableLayoutPanel.Controls.Add(this.InfoTableLayoutPanel, 0, 0);
            this.HeaderTableLayoutPanel.Controls.Add(this.TextBox1, 1, 0);
            this.HeaderTableLayoutPanel.Controls.Add(this.HeaderSeparator, 0, 1);
            this.HeaderTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeaderTableLayoutPanel.Name = "HeaderTableLayoutPanel";
            this.HeaderTableLayoutPanel.RowCount = 2;
            this.HeaderTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.HeaderTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.HeaderTableLayoutPanel.SetColumnSpan(this.HeaderSeparator, 2);
            this.HeaderTableLayoutPanel.Size = new System.Drawing.Size(977, 70);
            this.HeaderTableLayoutPanel.TabIndex = 0;
            // 
            // InfoTableLayoutPanel
            // 
            this.InfoTableLayoutPanel.AutoSize = true;
            this.InfoTableLayoutPanel.ColumnCount = 2;
            this.InfoTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.InfoTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.InfoTableLayoutPanel.Controls.Add(this.Label8, 0, 0);
            this.InfoTableLayoutPanel.Controls.Add(this.Label9, 1, 0);
            this.InfoTableLayoutPanel.Controls.Add(this.Label11, 0, 1);
            this.InfoTableLayoutPanel.Controls.Add(this.Label10, 1, 1);
            this.InfoTableLayoutPanel.Controls.Add(this.Label7, 0, 2);
            this.InfoTableLayoutPanel.Controls.Add(this.Label1, 1, 2);
            this.InfoTableLayoutPanel.Controls.Add(this.Label13, 0, 3);
            this.InfoTableLayoutPanel.Controls.Add(this.Label12, 1, 3);
            this.InfoTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.InfoTableLayoutPanel.Name = "InfoTableLayoutPanel";
            this.InfoTableLayoutPanel.RowCount = 4;
            this.InfoTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.InfoTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.InfoTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.InfoTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.InfoTableLayoutPanel.Size = new System.Drawing.Size(191, 84);
            this.InfoTableLayoutPanel.TabIndex = 0;
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(54, 21);
            this.Label8.TabIndex = 0;
            this.Label8.Text = "Level:";
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(39, 21);
            this.Label9.TabIndex = 1;
            this.Label9.Text = "SITE";
            // 
            // Label11
            // 
            this.Label11.AutoSize = true;
            this.Label11.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(61, 21);
            this.Label11.TabIndex = 2;
            this.Label11.Text = "Status:";
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(72, 21);
            this.Label10.TabIndex = 3;
            this.Label10.Text = "DEFINED";
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(84, 21);
            this.Label7.TabIndex = 4;
            this.Label7.Text = "Category:";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(73, 21);
            this.Label1.TabIndex = 5;
            this.Label1.Text = "Category";
            // 
            // Label13
            // 
            this.Label13.AutoSize = true;
            this.Label13.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(103, 21);
            this.Label13.TabIndex = 6;
            this.Label13.Text = "Application:";
            // 
            // Label12
            // 
            this.Label12.AutoSize = true;
            this.Label12.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(88, 21);
            this.Label12.TabIndex = 7;
            this.Label12.Text = "Application";
            // 
            // TextBox1
            // 
            this.TextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.TextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.TextBox1.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.ReadOnly = true;
            this.TextBox1.Size = new System.Drawing.Size(783, 22);
            this.TextBox1.TabIndex = 1;
            this.TextBox1.Text = "_VARIABLE NAME_";
            this.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox1.WordWrap = false;
            // 
            // HeaderSeparator
            // 
            this.HeaderSeparator.BackColor = System.Drawing.SystemColors.ControlDark;
            this.HeaderSeparator.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeaderSeparator.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.HeaderSeparator.Name = "HeaderSeparator";
            this.HeaderSeparator.Size = new System.Drawing.Size(977, 1);
            this.HeaderSeparator.TabIndex = 2;
            // 
            // CurrentValueGroupBox
            // 
            this.CurrentValueGroupBox.Controls.Add(this.DataGridView1);
            this.CurrentValueGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CurrentValueGroupBox.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.CurrentValueGroupBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.CurrentValueGroupBox.Name = "CurrentValueGroupBox";
            this.CurrentValueGroupBox.Size = new System.Drawing.Size(977, 155);
            this.CurrentValueGroupBox.TabIndex = 1;
            this.CurrentValueGroupBox.TabStop = false;
            this.CurrentValueGroupBox.Text = "Current Value:";
            // 
            // DataGridView1
            // 
            this.DataGridView1.AllowUserToAddRows = false;
            this.DataGridView1.AllowUserToDeleteRows = false;
            this.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.DataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.DataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView1.ContextMenuStrip = this.ContextMenuStrip1;
            this.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.DataGridView1.GridColor = System.Drawing.Color.LightSteelBlue;
            this.DataGridView1.Name = "DataGridView1";
            this.DataGridView1.RowHeadersVisible = false;
            this.DataGridView1.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DataGridView1.RowTemplate.Height = 22;
            this.DataGridView1.RowTemplate.ReadOnly = true;
            this.DataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.DataGridView1.ShowRowErrors = false;
            this.DataGridView1.Size = new System.Drawing.Size(971, 130);
            this.DataGridView1.TabIndex = 0;
            // 
            // ContextMenuStrip1
            // 
            this.ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem7,
            this.ViewDefinitionInEditorToolStripMenuItem});
            this.ContextMenuStrip1.Name = "ContextMenuStrip1";
            this.ContextMenuStrip1.Size = new System.Drawing.Size(275, 64);
            // 
            // ToolStripMenuItem7
            // 
            this.ToolStripMenuItem7.Name = "ToolStripMenuItem7";
            this.ToolStripMenuItem7.Size = new System.Drawing.Size(274, 30);
            this.ToolStripMenuItem7.Text = "View Locations";
            this.ToolStripMenuItem7.Click += new System.EventHandler(this.ToolStripMenuItem7_Click);
            // 
            // ViewDefinitionInEditorToolStripMenuItem
            // 
            this.ViewDefinitionInEditorToolStripMenuItem.Name = "ViewDefinitionInEditorToolStripMenuItem";
            this.ViewDefinitionInEditorToolStripMenuItem.Size = new System.Drawing.Size(274, 30);
            this.ViewDefinitionInEditorToolStripMenuItem.Text = "View Definition in Editor";
            this.ViewDefinitionInEditorToolStripMenuItem.Click += new System.EventHandler(this.ViewDefinitionInEditorToolStripMenuItem_Click);
            // 
            // HistoryGroupBox
            // 
            this.HistoryGroupBox.Controls.Add(this.DataGridView2);
            this.HistoryGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HistoryGroupBox.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.HistoryGroupBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.HistoryGroupBox.Name = "HistoryGroupBox";
            this.HistoryGroupBox.Size = new System.Drawing.Size(977, 313);
            this.HistoryGroupBox.TabIndex = 2;
            this.HistoryGroupBox.TabStop = false;
            this.HistoryGroupBox.Text = "History:";
            // 
            // DataGridView2
            // 
            this.DataGridView2.AllowUserToAddRows = false;
            this.DataGridView2.AllowUserToDeleteRows = false;
            this.DataGridView2.AllowUserToOrderColumns = true;
            this.DataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DataGridView2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DataGridView2.BackgroundColor = System.Drawing.Color.White;
            this.DataGridView2.CausesValidation = false;
            this.DataGridView2.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.DataGridView2.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView2.ContextMenuStrip = this.ContextMenuStrip2;
            this.DataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridView2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.DataGridView2.GridColor = System.Drawing.Color.LightSteelBlue;
            this.DataGridView2.Name = "DataGridView2";
            this.DataGridView2.RowHeadersVisible = false;
            this.DataGridView2.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataGridView2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DataGridView2.RowTemplate.Height = 22;
            this.DataGridView2.RowTemplate.ReadOnly = true;
            this.DataGridView2.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridView2.ShowRowErrors = false;
            this.DataGridView2.Size = new System.Drawing.Size(971, 288);
            this.DataGridView2.StandardTab = true;
            this.DataGridView2.TabIndex = 0;
            this.DataGridView2.VirtualMode = true;
            // 
            // ContextMenuStrip2
            // 
            this.ContextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GoToLineToolStripMenuItem,
            this.OpenFileInEditorToolStripMenuItem,
            this.OpenFileInViewerToolStripMenuItem,
            this.ViewLocationsToolStripMenuItem,
            this.ToolStripSeparator1,
            this.ExportToolStripMenuItem});
            this.ContextMenuStrip2.Name = "ContextMenuStrip1";
            this.ContextMenuStrip2.Size = new System.Drawing.Size(243, 160);
            // 
            // GoToLineToolStripMenuItem
            // 
            this.GoToLineToolStripMenuItem.Name = "GoToLineToolStripMenuItem";
            this.GoToLineToolStripMenuItem.Size = new System.Drawing.Size(242, 30);
            this.GoToLineToolStripMenuItem.Text = "Go To Line in Viewer";
            this.GoToLineToolStripMenuItem.Click += new System.EventHandler(this.GoToLineToolStripMenuItem_Click);
            // 
            // OpenFileInEditorToolStripMenuItem
            // 
            this.OpenFileInEditorToolStripMenuItem.Name = "OpenFileInEditorToolStripMenuItem";
            this.OpenFileInEditorToolStripMenuItem.Size = new System.Drawing.Size(242, 30);
            this.OpenFileInEditorToolStripMenuItem.Text = "Open File in Editor";
            this.OpenFileInEditorToolStripMenuItem.Click += new System.EventHandler(this.OpenFileInEditorToolStripMenuItem_Click);
            // 
            // OpenFileInViewerToolStripMenuItem
            // 
            this.OpenFileInViewerToolStripMenuItem.Name = "OpenFileInViewerToolStripMenuItem";
            this.OpenFileInViewerToolStripMenuItem.Size = new System.Drawing.Size(242, 30);
            this.OpenFileInViewerToolStripMenuItem.Text = "Open File in Viewer";
            this.OpenFileInViewerToolStripMenuItem.Click += new System.EventHandler(this.OpenFileInViewerToolStripMenuItem_Click);
            // 
            // ViewLocationsToolStripMenuItem
            // 
            this.ViewLocationsToolStripMenuItem.Name = "ViewLocationsToolStripMenuItem";
            this.ViewLocationsToolStripMenuItem.Size = new System.Drawing.Size(242, 30);
            this.ViewLocationsToolStripMenuItem.Text = "View Locations";
            this.ViewLocationsToolStripMenuItem.Click += new System.EventHandler(this.ViewLocationsToolStripMenuItem_Click);
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(239, 6);
            // 
            // ExportToolStripMenuItem
            // 
            this.ExportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToExcelToolStripMenuItem,
            this.ToTextFileToolStripMenuItem});
            this.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem";
            this.ExportToolStripMenuItem.Size = new System.Drawing.Size(242, 30);
            this.ExportToolStripMenuItem.Text = "Export ";
            // 
            // ToExcelToolStripMenuItem
            // 
            this.ToExcelToolStripMenuItem.Name = "ToExcelToolStripMenuItem";
            this.ToExcelToolStripMenuItem.Size = new System.Drawing.Size(168, 30);
            this.ToExcelToolStripMenuItem.Text = "To Excel";
            this.ToExcelToolStripMenuItem.Click += new System.EventHandler(this.ToExcelToolStripMenuItem_Click);
            // 
            // ToTextFileToolStripMenuItem
            // 
            this.ToTextFileToolStripMenuItem.Name = "ToTextFileToolStripMenuItem";
            this.ToTextFileToolStripMenuItem.Size = new System.Drawing.Size(168, 30);
            this.ToTextFileToolStripMenuItem.Text = "To Text File";
            // 
            // RelationsTableLayoutPanel
            // 
            this.RelationsTableLayoutPanel.ColumnCount = 2;
            this.RelationsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.RelationsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.RelationsTableLayoutPanel.Controls.Add(this.ParentsGroupBox, 0, 0);
            this.RelationsTableLayoutPanel.Controls.Add(this.ChildrenGroupBox, 1, 0);
            this.RelationsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RelationsTableLayoutPanel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.RelationsTableLayoutPanel.Name = "RelationsTableLayoutPanel";
            this.RelationsTableLayoutPanel.RowCount = 1;
            this.RelationsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RelationsTableLayoutPanel.Size = new System.Drawing.Size(977, 353);
            this.RelationsTableLayoutPanel.TabIndex = 3;
            // 
            // ParentsGroupBox
            // 
            this.ParentsGroupBox.Controls.Add(this.DataGridView3);
            this.ParentsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ParentsGroupBox.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.ParentsGroupBox.Name = "ParentsGroupBox";
            this.ParentsGroupBox.Size = new System.Drawing.Size(482, 347);
            this.ParentsGroupBox.TabIndex = 0;
            this.ParentsGroupBox.TabStop = false;
            this.ParentsGroupBox.Text = "Parents:";
            // 
            // DataGridView3
            // 
            this.DataGridView3.AllowUserToAddRows = false;
            this.DataGridView3.AllowUserToDeleteRows = false;
            this.DataGridView3.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DataGridView3.BackgroundColor = System.Drawing.Color.White;
            this.DataGridView3.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.DataGridView3.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.DataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridView3.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.DataGridView3.GridColor = System.Drawing.Color.LightSteelBlue;
            this.DataGridView3.Name = "DataGridView3";
            this.DataGridView3.RowHeadersVisible = false;
            this.DataGridView3.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataGridView3.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DataGridView3.RowTemplate.Height = 22;
            this.DataGridView3.RowTemplate.ReadOnly = true;
            this.DataGridView3.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DataGridView3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridView3.ShowRowErrors = false;
            this.DataGridView3.Size = new System.Drawing.Size(476, 322);
            this.DataGridView3.TabIndex = 0;
            this.DataGridView3.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView3_CellContentDoubleClick);
            // 
            // ChildrenGroupBox
            // 
            this.ChildrenGroupBox.Controls.Add(this.DataGridView4);
            this.ChildrenGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChildrenGroupBox.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.ChildrenGroupBox.Name = "ChildrenGroupBox";
            this.ChildrenGroupBox.Size = new System.Drawing.Size(483, 347);
            this.ChildrenGroupBox.TabIndex = 1;
            this.ChildrenGroupBox.TabStop = false;
            this.ChildrenGroupBox.Text = "Children:";
            // 
            // DataGridView4
            // 
            this.DataGridView4.AllowUserToAddRows = false;
            this.DataGridView4.AllowUserToDeleteRows = false;
            this.DataGridView4.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DataGridView4.BackgroundColor = System.Drawing.Color.White;
            this.DataGridView4.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.DataGridView4.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.DataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridView4.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.DataGridView4.GridColor = System.Drawing.Color.LightSteelBlue;
            this.DataGridView4.Name = "DataGridView4";
            this.DataGridView4.RowHeadersVisible = false;
            this.DataGridView4.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataGridView4.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DataGridView4.RowTemplate.Height = 22;
            this.DataGridView4.RowTemplate.ReadOnly = true;
            this.DataGridView4.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DataGridView4.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridView4.ShowRowErrors = false;
            this.DataGridView4.Size = new System.Drawing.Size(477, 322);
            this.DataGridView4.TabIndex = 0;
            this.DataGridView4.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView4_CellContentDoubleClick);
            // 
            // RichTextBox1
            // 
            this.RichTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.RichTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RichTextBox1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.RichTextBox1.Name = "RichTextBox1";
            this.RichTextBox1.Size = new System.Drawing.Size(977, 132);
            this.RichTextBox1.TabIndex = 4;
            this.RichTextBox1.Text = "";
            this.RichTextBox1.WordWrap = false;
            // 
            // VariableDossier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1010, 1050);
            this.Controls.Add(this.MainTableLayoutPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1010, 800);
            this.Name = "VariableDossier";
            this.ShowIcon = false;
            this.Text = "Variable Dossier";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VariableReport_FormClosing);
            this.Load += new System.EventHandler(this.VariableReport_Load);
            this.Shown += new System.EventHandler(this.VariableReport_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WatchF5_KeyDown);
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.HeaderTableLayoutPanel.ResumeLayout(false);
            this.HeaderTableLayoutPanel.PerformLayout();
            this.InfoTableLayoutPanel.ResumeLayout(false);
            this.InfoTableLayoutPanel.PerformLayout();
            this.CurrentValueGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView1)).EndInit();
            this.ContextMenuStrip1.ResumeLayout(false);
            this.HistoryGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView2)).EndInit();
            this.ContextMenuStrip2.ResumeLayout(false);
            this.RelationsTableLayoutPanel.ResumeLayout(false);
            this.ParentsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView3)).EndInit();
            this.ChildrenGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView4)).EndInit();
            this.ResumeLayout(false);

        }
        internal System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        internal System.Windows.Forms.TableLayoutPanel HeaderTableLayoutPanel;
        internal System.Windows.Forms.TableLayoutPanel InfoTableLayoutPanel;
        internal System.Windows.Forms.Label HeaderSeparator;
        internal System.Windows.Forms.GroupBox CurrentValueGroupBox;
        internal System.Windows.Forms.DataGridView DataGridView1;
        internal System.Windows.Forms.GroupBox HistoryGroupBox;
        internal System.Windows.Forms.RichTextBox RichTextBox1;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.Label Label10;
        internal System.Windows.Forms.Label Label11;
        internal System.Windows.Forms.DataGridView DataGridView2;
        internal System.Windows.Forms.TableLayoutPanel RelationsTableLayoutPanel;
        internal System.Windows.Forms.GroupBox ParentsGroupBox;
        internal System.Windows.Forms.GroupBox ChildrenGroupBox;
        internal System.Windows.Forms.DataGridView DataGridView4;
        internal System.Windows.Forms.DataGridView DataGridView3;
        internal System.Windows.Forms.ContextMenuStrip ContextMenuStrip2;
        internal System.Windows.Forms.ToolStripMenuItem GoToLineToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem OpenFileInEditorToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem OpenFileInViewerToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
        internal System.Windows.Forms.ToolStripMenuItem ExportToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ToTextFileToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ToExcelToolStripMenuItem;
        internal System.Windows.Forms.TextBox TextBox1;
        internal System.Windows.Forms.SaveFileDialog SaveFileDialog1;
        internal System.Windows.Forms.Label Label12;
        internal System.Windows.Forms.Label Label13;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.ContextMenuStrip ContextMenuStrip1;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem7;
        internal System.Windows.Forms.ToolStripMenuItem ViewLocationsToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ViewDefinitionInEditorToolStripMenuItem;
    }
}

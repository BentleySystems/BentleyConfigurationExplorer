using System;
using System.Diagnostics;
using System.Drawing;

namespace WorkspaceCFG
{
    public partial class AppWizard : System.Windows.Forms.Form
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(AppWizard));
            Label1 = new System.Windows.Forms.Label();
            cmdSave = new System.Windows.Forms.Button();
            cmdSave.Click += new EventHandler(CmdSave_Click);
            cmdApply = new System.Windows.Forms.Button();
            cmdApply.Click += new EventHandler(CmdApply_Click);
            dgApplicationList = new System.Windows.Forms.DataGridView();
            dgApplicationList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(DgApplicationList_CellValueChanged);
            dgApplicationList.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(DgApplicationList_RowsRemoved);
            dgApplicationList.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(DgApplicationList_RowsAdded);
            dgApplicationList.DragDrop += new System.Windows.Forms.DragEventHandler(DgApplicationList_DragDrop);
            dgApplicationList.DragOver += new System.Windows.Forms.DragEventHandler(DgApplicationList_DragOver);
            dgApplicationList.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(DgApplicationList_UserDeletingRow);
            cmdAddApp = new System.Windows.Forms.Button();
            cmdAddApp.Click += new EventHandler(ToolStripButton1_Click);
            cmdRemoveApp = new System.Windows.Forms.Button();
            cmdRemoveApp.Click += new EventHandler(ToolStripButton2_Click);
            PictureTip = new System.Windows.Forms.PictureBox();
            LabelTip = new System.Windows.Forms.Label();
            cmdCancel = new System.Windows.Forms.Button();
            cmdCancel.Click += new EventHandler(CmdCancel_Click);
            RootLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            ContentLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            HeaderRowPanel = new System.Windows.Forms.TableLayoutPanel();
            ButtonHeaderFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            FooterRowPanel = new System.Windows.Forms.TableLayoutPanel();
            TipFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            ButtonFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)dgApplicationList).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PictureTip).BeginInit();
            RootLayoutPanel.SuspendLayout();
            ContentLayoutPanel.SuspendLayout();
            HeaderRowPanel.SuspendLayout();
            ButtonHeaderFlowPanel.SuspendLayout();
            FooterRowPanel.SuspendLayout();
            TipFlowPanel.SuspendLayout();
            ButtonFlowPanel.SuspendLayout();
            SuspendLayout();
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Font = new Font("Segoe UI", 8.0f);
            Label1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            Label1.Name = "Label1";
            Label1.TabIndex = 42;
            Label1.Text = "Application List:";
            // 
            // cmdSave
            // 
            cmdSave.Font = new Font("Segoe UI", 8.0f);
            cmdSave.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            cmdSave.Name = "cmdSave";
            cmdSave.Size = new Size(66, 27);
            cmdSave.TabIndex = 45;
            cmdSave.Text = "OK";
            // 
            // cmdApply
            // 
            cmdApply.Font = new Font("Segoe UI", 8.0f);
            cmdApply.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            cmdApply.Name = "cmdApply";
            cmdApply.Size = new Size(66, 27);
            cmdApply.TabIndex = 48;
            cmdApply.Text = "Apply";
            cmdApply.Visible = false;
            // 
            // dgApplicationList
            // 
            dgApplicationList.AllowDrop = true;
            dgApplicationList.AllowUserToOrderColumns = true;
            dgApplicationList.AllowUserToResizeRows = false;
            dgApplicationList.Dock = System.Windows.Forms.DockStyle.Fill;
            dgApplicationList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgApplicationList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dgApplicationList.BackgroundColor = SystemColors.Control;
            dgApplicationList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dgApplicationList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgApplicationList.GridColor = SystemColors.Control;
            dgApplicationList.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            dgApplicationList.Name = "dgApplicationList";
            dgApplicationList.RowHeadersWidth = 30;
            dgApplicationList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgApplicationList.TabIndex = 46;
            // 
            // cmdAddApp
            // 
            cmdAddApp.Font = new Font("Segoe UI", 8.0f);
            cmdAddApp.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            cmdAddApp.Name = "cmdAddApp";
            cmdAddApp.Size = new Size(75, 27);
            cmdAddApp.TabIndex = 49;
            cmdAddApp.Text = "Add";
            cmdAddApp.UseVisualStyleBackColor = true;
            // 
            // cmdRemoveApp
            // 
            cmdRemoveApp.Font = new Font("Segoe UI", 8.0f);
            cmdRemoveApp.Margin = new System.Windows.Forms.Padding(0);
            cmdRemoveApp.Name = "cmdRemoveApp";
            cmdRemoveApp.Size = new Size(75, 27);
            cmdRemoveApp.TabIndex = 50;
            cmdRemoveApp.Text = "Remove";
            cmdRemoveApp.UseVisualStyleBackColor = true;
            // 
            // PictureTip
            // 
            PictureTip.Image = (Image)resources.GetObject("PictureTip.Image");
            PictureTip.InitialImage = null;
            PictureTip.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            PictureTip.Name = "PictureTip";
            PictureTip.Size = new Size(16, 16);
            PictureTip.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            PictureTip.TabIndex = 51;
            PictureTip.TabStop = false;
            // 
            // LabelTip
            // 
            LabelTip.AutoSize = true;
            LabelTip.Font = new Font("Segoe UI", 8.0f);
            LabelTip.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            LabelTip.Name = "LabelTip";
            LabelTip.TabIndex = 50;
            LabelTip.Text = "Tip : Add applications by dragging and dropping application shortcuts";
            // 
            // cmdCancel
            // 
            cmdCancel.Font = new Font("Segoe UI", 8.0f);
            cmdCancel.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            cmdCancel.Name = "cmdCancel";
            cmdCancel.Size = new Size(66, 27);
            cmdCancel.TabIndex = 52;
            cmdCancel.Text = "Cancel";
            cmdCancel.UseVisualStyleBackColor = true;
            // 
            // RootLayoutPanel
            // 
            RootLayoutPanel.ColumnCount = 1;
            RootLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            RootLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            RootLayoutPanel.RowCount = 1;
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            RootLayoutPanel.Controls.Add(ContentLayoutPanel, 0, 0);
            RootLayoutPanel.Name = "RootLayoutPanel";
            // 
            // ContentLayoutPanel
            // 
            ContentLayoutPanel.ColumnCount = 1;
            ContentLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ContentLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            ContentLayoutPanel.Padding = new System.Windows.Forms.Padding(12, 11, 12, 11);
            ContentLayoutPanel.RowCount = 4;
            ContentLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            ContentLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            ContentLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ContentLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            ContentLayoutPanel.Controls.Add(Label1, 0, 0);
            ContentLayoutPanel.Controls.Add(HeaderRowPanel, 0, 1);
            ContentLayoutPanel.Controls.Add(dgApplicationList, 0, 2);
            ContentLayoutPanel.Controls.Add(FooterRowPanel, 0, 3);
            ContentLayoutPanel.Name = "ContentLayoutPanel";
            // 
            // HeaderRowPanel
            // 
            HeaderRowPanel.AutoSize = true;
            HeaderRowPanel.ColumnCount = 1;
            HeaderRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            HeaderRowPanel.Controls.Add(ButtonHeaderFlowPanel, 0, 0);
            HeaderRowPanel.Dock = System.Windows.Forms.DockStyle.Top;
            HeaderRowPanel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            HeaderRowPanel.Name = "HeaderRowPanel";
            HeaderRowPanel.RowCount = 1;
            HeaderRowPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // ButtonHeaderFlowPanel
            // 
            ButtonHeaderFlowPanel.AutoSize = true;
            ButtonHeaderFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            ButtonHeaderFlowPanel.WrapContents = false;
            ButtonHeaderFlowPanel.Margin = new System.Windows.Forms.Padding(0);
            ButtonHeaderFlowPanel.Controls.Add(cmdAddApp);
            ButtonHeaderFlowPanel.Controls.Add(cmdRemoveApp);
            ButtonHeaderFlowPanel.Name = "ButtonHeaderFlowPanel";
            // 
            // FooterRowPanel
            // 
            FooterRowPanel.AutoSize = true;
            FooterRowPanel.ColumnCount = 2;
            FooterRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            FooterRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            FooterRowPanel.Controls.Add(TipFlowPanel, 0, 0);
            FooterRowPanel.Controls.Add(ButtonFlowPanel, 1, 0);
            FooterRowPanel.Dock = System.Windows.Forms.DockStyle.Top;
            FooterRowPanel.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            FooterRowPanel.Name = "FooterRowPanel";
            FooterRowPanel.RowCount = 1;
            FooterRowPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // TipFlowPanel
            // 
            TipFlowPanel.AutoSize = true;
            TipFlowPanel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            TipFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            TipFlowPanel.WrapContents = false;
            TipFlowPanel.Margin = new System.Windows.Forms.Padding(0);
            TipFlowPanel.Controls.Add(PictureTip);
            TipFlowPanel.Controls.Add(LabelTip);
            TipFlowPanel.Name = "TipFlowPanel";
            // 
            // ButtonFlowPanel
            // 
            ButtonFlowPanel.AutoSize = true;
            ButtonFlowPanel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            ButtonFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            ButtonFlowPanel.WrapContents = false;
            ButtonFlowPanel.Margin = new System.Windows.Forms.Padding(0);
            ButtonFlowPanel.Controls.Add(cmdApply);
            ButtonFlowPanel.Controls.Add(cmdSave);
            ButtonFlowPanel.Controls.Add(cmdCancel);
            ButtonFlowPanel.Name = "ButtonFlowPanel";
            // 
            // AppWizard
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(784, 381);
            Controls.Add(RootLayoutPanel);
            Font = new Font("Segoe UI", 8.0f);
            MinimumSize = new Size(600, 220);
            Name = "AppWizard";
            ShowIcon = false;
            Text = "Application Manager";
            ((System.ComponentModel.ISupportInitialize)dgApplicationList).EndInit();
            ((System.ComponentModel.ISupportInitialize)PictureTip).EndInit();
            Shown += new EventHandler(AppWizard_Shown);
            FormClosing += new System.Windows.Forms.FormClosingEventHandler(AppWizard_FormClosing);
            RootLayoutPanel.ResumeLayout(false);
            ContentLayoutPanel.ResumeLayout(false);
            ContentLayoutPanel.PerformLayout();
            HeaderRowPanel.ResumeLayout(false);
            HeaderRowPanel.PerformLayout();
            ButtonHeaderFlowPanel.ResumeLayout(false);
            ButtonHeaderFlowPanel.PerformLayout();
            FooterRowPanel.ResumeLayout(false);
            FooterRowPanel.PerformLayout();
            TipFlowPanel.ResumeLayout(false);
            TipFlowPanel.PerformLayout();
            ButtonFlowPanel.ResumeLayout(false);
            ButtonFlowPanel.PerformLayout();
            ResumeLayout(false);

        }
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Button cmdSave;
        internal System.Windows.Forms.Button cmdApply;
        internal System.Windows.Forms.DataGridView dgApplicationList;
        internal System.Windows.Forms.Button cmdAddApp;
        internal System.Windows.Forms.Button cmdRemoveApp;
        internal System.Windows.Forms.PictureBox PictureTip;
        internal System.Windows.Forms.Label LabelTip;
        internal System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.TableLayoutPanel RootLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel ContentLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel HeaderRowPanel;
        private System.Windows.Forms.FlowLayoutPanel ButtonHeaderFlowPanel;
        private System.Windows.Forms.TableLayoutPanel FooterRowPanel;
        private System.Windows.Forms.FlowLayoutPanel TipFlowPanel;
        private System.Windows.Forms.FlowLayoutPanel ButtonFlowPanel;
    }
}
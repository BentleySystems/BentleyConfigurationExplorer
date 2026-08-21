namespace WorkspaceCFG
{
    partial class CFGTree
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lblNote = new System.Windows.Forms.Label();
            this.cfgRichTextBox1 = new WorkspaceCFG.CFGRichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.NotePanel = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.NotePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Name = "treeView1";
            this.treeView1.Scrollable = true;
            this.treeView1.Size = new System.Drawing.Size(1464, 402);
            this.treeView1.TabIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNote.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.lblNote.Name = "lblNote";
            this.lblNote.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.lblNote.Size = new System.Drawing.Size(82, 20);
            this.lblNote.TabIndex = 3;
            this.lblNote.Text = "Note Data";
            // 
            // cfgRichTextBox1
            // 
            this.cfgRichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cfgRichTextBox1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.cfgRichTextBox1.Name = "cfgRichTextBox1";
            this.cfgRichTextBox1.Size = new System.Drawing.Size(1464, 351);
            this.cfgRichTextBox1.TabIndex = 4;
            this.cfgRichTextBox1.Text = "";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.NotePanel);
            this.splitContainer1.Size = new System.Drawing.Size(1515, 917);
            this.splitContainer1.SplitterDistance = 458;
            this.splitContainer1.TabIndex = 5;
            // 
            // NotePanel
            // 
            this.NotePanel.ColumnCount = 1;
            this.NotePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.NotePanel.Controls.Add(this.lblNote, 0, 0);
            this.NotePanel.Controls.Add(this.cfgRichTextBox1, 0, 1);
            this.NotePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NotePanel.Name = "NotePanel";
            this.NotePanel.RowCount = 2;
            this.NotePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.NotePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.NotePanel.TabIndex = 6;
            // 
            // CFGTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1515, 917);
            this.Controls.Add(this.splitContainer1);
            this.Name = "CFGTree";
            this.Text = "CFGTree";
            this.Load += new System.EventHandler(this.CFGTree_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.NotePanel.ResumeLayout(false);
            this.NotePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label lblNote;
        private CFGRichTextBox cfgRichTextBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel NotePanel;
    }
}
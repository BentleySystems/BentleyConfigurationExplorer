using System;
using System.Diagnostics;
using System.Drawing;

namespace WorkspaceCFG
{
    public partial class DebugForm : System.Windows.Forms.Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugForm));
            this.Label5 = new System.Windows.Forms.Label();
            this.SplitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Panel1LayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.TextBoxLine = new System.Windows.Forms.TextBox();
            this.ButtonParse = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Panel2LayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Label1 = new System.Windows.Forms.Label();
            this.TextBoxDebug = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).BeginInit();
            this.SplitContainer1.Panel1.SuspendLayout();
            this.SplitContainer1.Panel2.SuspendLayout();
            this.SplitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.Panel1LayoutPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.Panel2LayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label5.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(67, 13);
            this.Label5.TabIndex = 11;
            this.Label5.Text = "Lines to test:";
            // 
            // SplitContainer1
            // 
            this.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SplitContainer1.Name = "SplitContainer1";
            this.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer1.Panel1
            // 
            this.SplitContainer1.Panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.SplitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // SplitContainer1.Panel2
            // 
            this.SplitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.SplitContainer1.Panel2.Controls.Add(this.panel2);
            this.SplitContainer1.Size = new System.Drawing.Size(884, 575);
            this.SplitContainer1.SplitterDistance = 287;
            this.SplitContainer1.SplitterWidth = 6;
            this.SplitContainer1.TabIndex = 15;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.Panel1LayoutPanel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(884, 287);
            this.panel1.TabIndex = 15;
            // 
            // Panel1LayoutPanel
            // 
            this.Panel1LayoutPanel.ColumnCount = 1;
            this.Panel1LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Panel1LayoutPanel.Controls.Add(this.Label5, 0, 0);
            this.Panel1LayoutPanel.Controls.Add(this.TextBoxLine, 0, 1);
            this.Panel1LayoutPanel.Controls.Add(this.ButtonParse, 0, 2);
            this.Panel1LayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel1LayoutPanel.Name = "Panel1LayoutPanel";
            this.Panel1LayoutPanel.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.Panel1LayoutPanel.RowCount = 3;
            this.Panel1LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.Panel1LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Panel1LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // TextBoxLine
            // 
            this.TextBoxLine.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxLine.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxLine.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxLine.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.TextBoxLine.Multiline = true;
            this.TextBoxLine.Name = "TextBoxLine";
            this.TextBoxLine.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextBoxLine.TabIndex = 18;
            this.TextBoxLine.WordWrap = false;
            // 
            // ButtonParse
            // 
            this.ButtonParse.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ButtonParse.AutoSize = true;
            this.ButtonParse.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ButtonParse.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonParse.MinimumSize = new System.Drawing.Size(112, 35);
            this.ButtonParse.Name = "ButtonParse";
            this.ButtonParse.Padding = new System.Windows.Forms.Padding(16, 5, 16, 4);
            this.ButtonParse.TabIndex = 15;
            this.ButtonParse.Text = "Parse";
            this.ButtonParse.UseVisualStyleBackColor = true;
            this.ButtonParse.Click += new System.EventHandler(this.ButtonParse_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Window;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.Panel2LayoutPanel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(884, 282);
            this.panel2.TabIndex = 13;
            // 
            // Panel2LayoutPanel
            // 
            this.Panel2LayoutPanel.ColumnCount = 1;
            this.Panel2LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Panel2LayoutPanel.Controls.Add(this.Label1, 0, 0);
            this.Panel2LayoutPanel.Controls.Add(this.TextBoxDebug, 0, 1);
            this.Panel2LayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel2LayoutPanel.Name = "Panel2LayoutPanel";
            this.Panel2LayoutPanel.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.Panel2LayoutPanel.RowCount = 2;
            this.Panel2LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.Panel2LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(75, 13);
            this.Label1.TabIndex = 12;
            this.Label1.Text = "Debug output:";
            // 
            // TextBoxDebug
            // 
            this.TextBoxDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxDebug.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxDebug.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.TextBoxDebug.Margin = new System.Windows.Forms.Padding(0);
            this.TextBoxDebug.Multiline = true;
            this.TextBoxDebug.Name = "TextBoxDebug";
            this.TextBoxDebug.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextBoxDebug.TabIndex = 1;
            this.TextBoxDebug.WordWrap = false;
            // 
            // DebugForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 780);
            this.Controls.Add(this.SplitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "DebugForm";
            this.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.Text = "Parsing Debugger";
            this.SplitContainer1.Panel1.ResumeLayout(false);
            this.SplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).EndInit();
            this.SplitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.Panel1LayoutPanel.ResumeLayout(false);
            this.Panel1LayoutPanel.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.Panel2LayoutPanel.ResumeLayout(false);
            this.Panel2LayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.SplitContainer SplitContainer1;
        internal System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel Panel1LayoutPanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel Panel2LayoutPanel;
        internal System.Windows.Forms.TextBox TextBoxLine;
        internal System.Windows.Forms.Button ButtonParse;
        internal System.Windows.Forms.TextBox TextBoxDebug;
    }
}
using System;
using System.Diagnostics;
using System.Drawing;

namespace WorkspaceCFG
{
    public partial class CFGError : System.Windows.Forms.Form
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
            RootLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            LinkLabel1 = new System.Windows.Forms.LinkLabel();
            LinkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(LinkLabel1_LinkClicked);
            Label1 = new System.Windows.Forms.Label();
            ButtonRowPanel = new System.Windows.Forms.FlowLayoutPanel();
            OK_Button = new System.Windows.Forms.Button();
            OK_Button.Click += new EventHandler(OK_Button_Click);
            RootLayoutPanel.SuspendLayout();
            ButtonRowPanel.SuspendLayout();
            SuspendLayout();
            // 
            // RootLayoutPanel
            // 
            RootLayoutPanel.AutoSize = true;
            RootLayoutPanel.ColumnCount = 1;
            RootLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            RootLayoutPanel.Controls.Add(LinkLabel1, 0, 0);
            RootLayoutPanel.Controls.Add(Label1, 0, 1);
            RootLayoutPanel.Controls.Add(ButtonRowPanel, 0, 2);
            RootLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            RootLayoutPanel.Name = "RootLayoutPanel";
            RootLayoutPanel.Padding = new System.Windows.Forms.Padding(12);
            RootLayoutPanel.RowCount = 3;
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // LinkLabel1
            // 
            LinkLabel1.AutoSize = true;
            LinkLabel1.Dock = System.Windows.Forms.DockStyle.Top;
            LinkLabel1.Font = new Font("Segoe UI", 8.0f);
            LinkLabel1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            LinkLabel1.Name = "LinkLabel1";
            LinkLabel1.TabIndex = 1;
            LinkLabel1.TabStop = true;
            LinkLabel1.Text = "There was a critical error raised in <FILE> on line <LINE>";
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Dock = System.Windows.Forms.DockStyle.Top;
            Label1.Font = new Font("Segoe UI", 8.0f);
            Label1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            Label1.Name = "Label1";
            Label1.TabIndex = 2;
            Label1.Text = "Line data";
            // 
            // ButtonRowPanel
            // 
            ButtonRowPanel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            ButtonRowPanel.AutoSize = true;
            ButtonRowPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            ButtonRowPanel.Controls.Add(OK_Button);
            ButtonRowPanel.Margin = new System.Windows.Forms.Padding(0);
            ButtonRowPanel.Name = "ButtonRowPanel";
            // 
            // OK_Button
            // 
            OK_Button.Font = new Font("Segoe UI", 8.0f);
            OK_Button.Margin = new System.Windows.Forms.Padding(0);
            OK_Button.Name = "OK_Button";
            OK_Button.Size = new Size(66, 25);
            OK_Button.TabIndex = 0;
            OK_Button.Text = "OK";
            // 
            // CFGError
            // 
            AcceptButton = OK_Button;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(424, 100);
            Controls.Add(RootLayoutPanel);
            Font = new Font("Segoe UI", 8.0f);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CFGError";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Critical Error";
            RootLayoutPanel.ResumeLayout(false);
            ButtonRowPanel.ResumeLayout(false);
            ResumeLayout(false);

        }
        internal System.Windows.Forms.TableLayoutPanel RootLayoutPanel;
        internal System.Windows.Forms.FlowLayoutPanel ButtonRowPanel;
        internal System.Windows.Forms.Button OK_Button;
        internal System.Windows.Forms.LinkLabel LinkLabel1;
        internal System.Windows.Forms.Label Label1;

    }
}
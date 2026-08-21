using System;
using System.Diagnostics;
using System.Drawing;

namespace WorkspaceCFG
{
    public partial class ConfigurationSelectorDialog : System.Windows.Forms.Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationSelectorDialog));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.OpenAppMgrButton = new System.Windows.Forms.Button();
            this.ConfigurationSelection1 = new WorkspaceCFG.ConfigurationSelection();
            this.RootLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.HeaderRowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.FooterRowPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.RootLayoutPanel.SuspendLayout();
            this.HeaderRowPanel.SuspendLayout();
            this.FooterRowPanel.SuspendLayout();
            this.ButtonFlowPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.AutoSize = true;
            this.btnOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOK.Enabled = false;
            this.btnOK.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnOK.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Padding = new System.Windows.Forms.Padding(16, 5, 16, 4);
            this.btnOK.MinimumSize = new System.Drawing.Size(80, 29);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "Process";
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(16, 5, 16, 4);
            this.btnCancel.MinimumSize = new System.Drawing.Size(67, 29);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // OpenAppMgrButton
            // 
            this.OpenAppMgrButton.AutoSize = true;
            this.OpenAppMgrButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.OpenAppMgrButton.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.OpenAppMgrButton.Image = ((System.Drawing.Image)(resources.GetObject("OpenAppMgrToolStripButton.Image")));
            this.OpenAppMgrButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OpenAppMgrButton.Margin = new System.Windows.Forms.Padding(0);
            this.OpenAppMgrButton.Name = "OpenAppMgrButton";
            this.OpenAppMgrButton.Padding = new System.Windows.Forms.Padding(8, 4, 10, 4);
            this.OpenAppMgrButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OpenAppMgrButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.OpenAppMgrButton.Text = "Open Application Manager";
            this.OpenAppMgrButton.UseVisualStyleBackColor = true;
            this.OpenAppMgrButton.Click += new System.EventHandler(this.ToolStripButton1_Click);
            // 
            // ConfigurationSelection1
            // 
            this.ConfigurationSelection1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConfigurationSelection1.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.ConfigurationSelection1.Name = "ConfigurationSelection1";
            this.ConfigurationSelection1.Ready = false;
            this.ConfigurationSelection1.TabIndex = 12;
            this.ConfigurationSelection1.ReadyChanged += new WorkspaceCFG.ConfigurationSelection.ReadyChangedEventHandler(this.ConfigurationSelection1_ReadyChanged);
            // 
            // RootLayoutPanel
            // 
            this.RootLayoutPanel.ColumnCount = 1;
            this.RootLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RootLayoutPanel.Padding = new System.Windows.Forms.Padding(12, 8, 12, 11);
            this.RootLayoutPanel.RowCount = 3;
            this.RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.RootLayoutPanel.Controls.Add(this.HeaderRowPanel, 0, 0);
            this.RootLayoutPanel.Controls.Add(this.ConfigurationSelection1, 0, 1);
            this.RootLayoutPanel.Controls.Add(this.FooterRowPanel, 0, 2);
            this.RootLayoutPanel.Name = "RootLayoutPanel";
            // 
            // HeaderRowPanel
            // 
            this.HeaderRowPanel.AutoSize = true;
            this.HeaderRowPanel.Controls.Add(this.OpenAppMgrButton);
            this.HeaderRowPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeaderRowPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.HeaderRowPanel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.HeaderRowPanel.Name = "HeaderRowPanel";
            this.HeaderRowPanel.WrapContents = false;
            // 
            // FooterRowPanel
            // 
            this.FooterRowPanel.AutoSize = true;
            this.FooterRowPanel.ColumnCount = 2;
            this.FooterRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.FooterRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.FooterRowPanel.Controls.Add(this.ButtonFlowPanel, 1, 0);
            this.FooterRowPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.FooterRowPanel.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.FooterRowPanel.Name = "FooterRowPanel";
            this.FooterRowPanel.RowCount = 1;
            this.FooterRowPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // ButtonFlowPanel
            // 
            this.ButtonFlowPanel.AutoSize = true;
            this.ButtonFlowPanel.Controls.Add(this.btnOK);
            this.ButtonFlowPanel.Controls.Add(this.btnCancel);
            this.ButtonFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.ButtonFlowPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonFlowPanel.Name = "ButtonFlowPanel";
            this.ButtonFlowPanel.WrapContents = false;
            // 
            // ConfigurationSelectorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 388);
            this.Controls.Add(this.RootLayoutPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(285, 255);
            this.Name = "ConfigurationSelectorDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Process Configuration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigurationSelector_FormClosing);
            this.Load += new System.EventHandler(this.ConfigurationSelectorDialog_Load);
            this.Shown += new System.EventHandler(this.ConfigurationSelector_Shown);
            this.RootLayoutPanel.ResumeLayout(false);
            this.RootLayoutPanel.PerformLayout();
            this.HeaderRowPanel.ResumeLayout(false);
            this.HeaderRowPanel.PerformLayout();
            this.FooterRowPanel.ResumeLayout(false);
            this.FooterRowPanel.PerformLayout();
            this.ButtonFlowPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal System.Windows.Forms.Button btnOK;
        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.Button OpenAppMgrButton;
        internal ConfigurationSelection ConfigurationSelection1;
        internal System.Windows.Forms.TableLayoutPanel RootLayoutPanel;
        internal System.Windows.Forms.FlowLayoutPanel HeaderRowPanel;
        internal System.Windows.Forms.TableLayoutPanel FooterRowPanel;
        internal System.Windows.Forms.FlowLayoutPanel ButtonFlowPanel;
    }
}
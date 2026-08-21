using System;
using System.Diagnostics;
using System.Drawing;

namespace WorkspaceCFG
{
    public partial class InstalledApplication : System.Windows.Forms.Form
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(InstalledApplication));
            DataGridView1 = new System.Windows.Forms.DataGridView();
            ButtonCopyToClipboard = new System.Windows.Forms.Button();
            ButtonCopyToClipboard.Click += new EventHandler(ToolStripButtonCopyToClipboard_Click);
            AddButton = new System.Windows.Forms.Button();
            AddButton.Click += new EventHandler(AddButton_Click);
            Button2 = new System.Windows.Forms.Button();
            Button2.Click += new EventHandler(Button2_Click);
            LabelTip = new System.Windows.Forms.Label();
            PictureBox1 = new System.Windows.Forms.PictureBox();
            RootLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            FooterPanel = new System.Windows.Forms.TableLayoutPanel();
            TipFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            ButtonFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)DataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PictureBox1).BeginInit();
            RootLayoutPanel.SuspendLayout();
            FooterPanel.SuspendLayout();
            TipFlowPanel.SuspendLayout();
            ButtonFlowPanel.SuspendLayout();
            SuspendLayout();
            // 
            // DataGridView1
            // 
            DataGridView1.AllowUserToAddRows = false;
            DataGridView1.AllowUserToDeleteRows = false;
            DataGridView1.AllowUserToResizeRows = false;
            DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridView1.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);
            DataGridView1.Name = "DataGridView1";
            DataGridView1.RowHeadersWidth = 51;
            DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            DataGridView1.TabIndex = 0;
            // 
            // ButtonCopyToClipboard
            // 
            ButtonCopyToClipboard.AutoSize = true;
            ButtonCopyToClipboard.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ButtonCopyToClipboard.Font = new Font("Segoe UI", 8.0f);
            ButtonCopyToClipboard.Margin = new System.Windows.Forms.Padding(0);
            ButtonCopyToClipboard.MinimumSize = new Size(125, 27);
            ButtonCopyToClipboard.Name = "ButtonCopyToClipboard";
            ButtonCopyToClipboard.Padding = new System.Windows.Forms.Padding(8, 2, 8, 2);
            ButtonCopyToClipboard.TabIndex = 1;
            ButtonCopyToClipboard.Text = "Copy to Clipboard";
            ButtonCopyToClipboard.UseVisualStyleBackColor = true;
            // 
            // AddButton
            // 
            AddButton.AutoSize = true;
            AddButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            AddButton.Font = new Font("Segoe UI", 8.0f);
            AddButton.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            AddButton.MinimumSize = new Size(75, 30);
            AddButton.Name = "AddButton";
            AddButton.Padding = new System.Windows.Forms.Padding(8, 2, 8, 2);
            AddButton.TabIndex = 2;
            AddButton.Text = "Add";
            AddButton.UseVisualStyleBackColor = true;
            // 
            // Button2
            // 
            Button2.AutoSize = true;
            Button2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Button2.Font = new Font("Segoe UI", 8.0f);
            Button2.Margin = new System.Windows.Forms.Padding(0);
            Button2.MinimumSize = new Size(75, 31);
            Button2.Name = "Button2";
            Button2.Padding = new System.Windows.Forms.Padding(8, 2, 8, 2);
            Button2.TabIndex = 3;
            Button2.Text = "Cancel";
            Button2.UseVisualStyleBackColor = true;
            // 
            // LabelTip
            // 
            LabelTip.AutoSize = true;
            LabelTip.Font = new Font("Segoe UI", 8.0f);
            LabelTip.Margin = new System.Windows.Forms.Padding(4, 4, 0, 0);
            LabelTip.Name = "LabelTip";
            LabelTip.TabIndex = 5;
            LabelTip.Text = "Tip : To select mulitiple applications, hold down the Ctrl key while selecting.";
            // 
            // PictureBox1
            // 
            PictureBox1.Image = (Image)resources.GetObject("PictureBox1.Image");
            PictureBox1.InitialImage = null;
            PictureBox1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            PictureBox1.Name = "PictureBox1";
            PictureBox1.Size = new Size(18, 22);
            PictureBox1.TabIndex = 6;
            PictureBox1.TabStop = false;
            // 
            // RootLayoutPanel
            // 
            RootLayoutPanel.ColumnCount = 1;
            RootLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            RootLayoutPanel.Controls.Add(ButtonCopyToClipboard, 0, 0);
            RootLayoutPanel.Controls.Add(DataGridView1, 0, 1);
            RootLayoutPanel.Controls.Add(FooterPanel, 0, 2);
            RootLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            RootLayoutPanel.Name = "RootLayoutPanel";
            RootLayoutPanel.Padding = new System.Windows.Forms.Padding(12, 8, 12, 10);
            RootLayoutPanel.RowCount = 3;
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // FooterPanel
            // 
            FooterPanel.AutoSize = true;
            FooterPanel.ColumnCount = 2;
            FooterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            FooterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            FooterPanel.Controls.Add(TipFlowPanel, 0, 0);
            FooterPanel.Controls.Add(ButtonFlowPanel, 1, 0);
            FooterPanel.Dock = System.Windows.Forms.DockStyle.Top;
            FooterPanel.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            FooterPanel.Name = "FooterPanel";
            FooterPanel.RowCount = 1;
            FooterPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // TipFlowPanel
            // 
            TipFlowPanel.AutoSize = true;
            TipFlowPanel.Controls.Add(PictureBox1);
            TipFlowPanel.Controls.Add(LabelTip);
            TipFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            TipFlowPanel.Margin = new System.Windows.Forms.Padding(0);
            TipFlowPanel.Name = "TipFlowPanel";
            TipFlowPanel.WrapContents = false;
            // 
            // ButtonFlowPanel
            // 
            ButtonFlowPanel.AutoSize = true;
            ButtonFlowPanel.Controls.Add(AddButton);
            ButtonFlowPanel.Controls.Add(Button2);
            ButtonFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            ButtonFlowPanel.Margin = new System.Windows.Forms.Padding(0);
            ButtonFlowPanel.Name = "ButtonFlowPanel";
            ButtonFlowPanel.WrapContents = false;
            // 
            // InstalledApplication
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(784, 361);
            Controls.Add(RootLayoutPanel);
            DoubleBuffered = true;
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(650, 200);
            Name = "InstalledApplication";
            ShowIcon = false;
            Text = "Installed Bentley Applications";
            ((System.ComponentModel.ISupportInitialize)DataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)PictureBox1).EndInit();
            RootLayoutPanel.ResumeLayout(false);
            RootLayoutPanel.PerformLayout();
            FooterPanel.ResumeLayout(false);
            FooterPanel.PerformLayout();
            TipFlowPanel.ResumeLayout(false);
            TipFlowPanel.PerformLayout();
            ButtonFlowPanel.ResumeLayout(false);
            ButtonFlowPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }
        internal System.Windows.Forms.DataGridView DataGridView1;
        internal System.Windows.Forms.Button ButtonCopyToClipboard;
        internal System.Windows.Forms.Button AddButton;
        internal System.Windows.Forms.Button Button2;
        internal System.Windows.Forms.Label LabelTip;
        internal System.Windows.Forms.PictureBox PictureBox1;
        private System.Windows.Forms.TableLayoutPanel RootLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel FooterPanel;
        private System.Windows.Forms.FlowLayoutPanel TipFlowPanel;
        private System.Windows.Forms.FlowLayoutPanel ButtonFlowPanel;
    }
}
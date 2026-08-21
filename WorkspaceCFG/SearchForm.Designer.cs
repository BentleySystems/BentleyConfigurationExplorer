using System;
using System.Diagnostics;
using System.Drawing;

namespace WorkspaceCFG
{
    public partial class SearchForm : System.Windows.Forms.Form
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
            TextBoxSearch = new System.Windows.Forms.TextBox();
            TextBoxSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(TextBoxSearch_KeyDown);
            ButtonSearch = new System.Windows.Forms.Button();
            ButtonSearch.Click += new EventHandler(ButtonSearch_Click);
            RootLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            ButtonRowPanel = new System.Windows.Forms.TableLayoutPanel();
            RootLayoutPanel.SuspendLayout();
            ButtonRowPanel.SuspendLayout();
            SuspendLayout();
            // 
            // TextBoxSearch
            // 
            TextBoxSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            TextBoxSearch.Font = new Font("Segoe UI", 8.0f);
            TextBoxSearch.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            TextBoxSearch.Name = "TextBoxSearch";
            TextBoxSearch.TabIndex = 0;
            // 
            // ButtonSearch
            // 
            ButtonSearch.Anchor = System.Windows.Forms.AnchorStyles.Right;
            ButtonSearch.Font = new Font("Segoe UI", 8.0f);
            ButtonSearch.Margin = new System.Windows.Forms.Padding(0);
            ButtonSearch.Name = "ButtonSearch";
            ButtonSearch.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            ButtonSearch.Size = new Size(75, 23);
            ButtonSearch.TabIndex = 1;
            ButtonSearch.Text = "Search";
            ButtonSearch.UseVisualStyleBackColor = true;
            // 
            // RootLayoutPanel
            // 
            RootLayoutPanel.ColumnCount = 1;
            RootLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            RootLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            RootLayoutPanel.Padding = new System.Windows.Forms.Padding(12);
            RootLayoutPanel.RowCount = 2;
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.Controls.Add(TextBoxSearch, 0, 0);
            RootLayoutPanel.Controls.Add(ButtonRowPanel, 0, 1);
            RootLayoutPanel.Name = "RootLayoutPanel";
            // 
            // ButtonRowPanel
            // 
            ButtonRowPanel.AutoSize = true;
            ButtonRowPanel.ColumnCount = 2;
            ButtonRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ButtonRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            ButtonRowPanel.Controls.Add(ButtonSearch, 1, 0);
            ButtonRowPanel.Dock = System.Windows.Forms.DockStyle.Top;
            ButtonRowPanel.Name = "ButtonRowPanel";
            ButtonRowPanel.RowCount = 1;
            ButtonRowPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // SearchForm
            // 
            AutoScaleDimensions = new SizeF(8.0f, 16.0f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(361, 78);
            Controls.Add(RootLayoutPanel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            Name = "SearchForm";
            ShowIcon = false;
            Text = "Search";
            RootLayoutPanel.ResumeLayout(false);
            RootLayoutPanel.PerformLayout();
            ButtonRowPanel.ResumeLayout(false);
            ButtonRowPanel.PerformLayout();
            ResumeLayout(false);

        }

        internal System.Windows.Forms.TextBox TextBoxSearch;
        internal System.Windows.Forms.Button ButtonSearch;
        private System.Windows.Forms.TableLayoutPanel RootLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel ButtonRowPanel;
    }
}
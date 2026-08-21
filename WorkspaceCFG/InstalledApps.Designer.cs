using System;
using System.Diagnostics;
using System.Drawing;

namespace WorkspaceCFG
{
    public partial class InstalledApps : System.Windows.Forms.Form
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
            DataGridView4 = new System.Windows.Forms.DataGridView();
            RootLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)DataGridView4).BeginInit();
            RootLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // DataGridView4
            // 
            DataGridView4.AllowUserToDeleteRows = false;
            DataGridView4.Dock = System.Windows.Forms.DockStyle.Fill;
            DataGridView4.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            DataGridView4.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            DataGridView4.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            DataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridView4.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            DataGridView4.GridColor = Color.LightSteelBlue;
            DataGridView4.Margin = new System.Windows.Forms.Padding(0);
            DataGridView4.Name = "DataGridView4";
            DataGridView4.RowHeadersVisible = false;
            DataGridView4.RowHeadersWidth = 51;
            DataGridView4.RowTemplate.DefaultCellStyle.Font = new Font("Courier New", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            DataGridView4.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            DataGridView4.RowTemplate.Height = 22;
            DataGridView4.RowTemplate.ReadOnly = true;
            DataGridView4.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            DataGridView4.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            DataGridView4.ShowRowErrors = false;
            DataGridView4.TabIndex = 39;
            // 
            // RootLayoutPanel
            // 
            RootLayoutPanel.ColumnCount = 1;
            RootLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            RootLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            RootLayoutPanel.Padding = new System.Windows.Forms.Padding(12);
            RootLayoutPanel.RowCount = 1;
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            RootLayoutPanel.Controls.Add(DataGridView4, 0, 0);
            RootLayoutPanel.Name = "RootLayoutPanel";
            // 
            // InstalledApps
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(784, 361);
            Controls.Add(RootLayoutPanel);
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 8.0f);
            MinimumSize = new Size(600, 200);
            Name = "InstalledApps";
            ShowIcon = false;
            Text = "Installed Bentley Applications";
            ((System.ComponentModel.ISupportInitialize)DataGridView4).EndInit();
            RootLayoutPanel.ResumeLayout(false);
            Load += new EventHandler(InstalledApps_Load);
            ResumeLayout(false);

        }
        internal System.Windows.Forms.DataGridView DataGridView4;
        private System.Windows.Forms.TableLayoutPanel RootLayoutPanel;
    }
}
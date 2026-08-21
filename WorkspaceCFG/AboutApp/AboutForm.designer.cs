//using Bentley.AboutApp.Resource;

namespace Bentley.ConfigurationExplorer.AboutApp
    {
    partial class AboutForm
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
                components.Dispose ();
                }

            base.Dispose (disposing);
            }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
            {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBoxLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.infoLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelAppName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonRowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonLegal = new System.Windows.Forms.Button();
            this.rootLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1.SuspendLayout();
            this.groupBoxLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.infoLayoutPanel.SuspendLayout();
            this.buttonRowPanel.SuspendLayout();
            this.rootLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.groupBoxLayoutPanel);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBoxLayoutPanel
            // 
            this.groupBoxLayoutPanel.AutoSize = true;
            this.groupBoxLayoutPanel.ColumnCount = 2;
            this.groupBoxLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.groupBoxLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.groupBoxLayoutPanel.Controls.Add(this.pictureBox1, 0, 0);
            this.groupBoxLayoutPanel.Controls.Add(this.infoLayoutPanel, 1, 0);
            this.groupBoxLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxLayoutPanel.Name = "groupBoxLayoutPanel";
            this.groupBoxLayoutPanel.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.groupBoxLayoutPanel.RowCount = 1;
            this.groupBoxLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = global::Bentley.ConfigurationExplorer.AboutApp.Properties.Resources.bentleyb_64;
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(56, 56);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // infoLayoutPanel
            // 
            this.infoLayoutPanel.AutoSize = true;
            this.infoLayoutPanel.ColumnCount = 2;
            this.infoLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.infoLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.infoLayoutPanel.Controls.Add(this.labelAppName, 0, 0);
            this.infoLayoutPanel.Controls.Add(this.label1, 0, 1);
            this.infoLayoutPanel.Controls.Add(this.lblVersion, 1, 1);
            this.infoLayoutPanel.Controls.Add(this.label2, 0, 2);
            this.infoLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoLayoutPanel.Name = "infoLayoutPanel";
            this.infoLayoutPanel.RowCount = 3;
            this.infoLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.infoLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.infoLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // labelAppName
            // 
            this.infoLayoutPanel.SetColumnSpan(this.labelAppName, 2);
            this.labelAppName.AutoSize = true;
            this.labelAppName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelAppName.Margin = new System.Windows.Forms.Padding(3, 0, 3, 6);
            this.labelAppName.Name = "labelAppName";
            this.labelAppName.Text = "Bentley Configuration Explorer";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 4);
            this.label1.Name = "label1";
            this.label1.Text = "Version";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblVersion.Margin = new System.Windows.Forms.Padding(3, 0, 3, 4);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Text = "XX";
            // 
            // label2
            // 
            this.infoLayoutPanel.SetColumnSpan(this.label2, 2);
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.label2.Name = "label2";
            this.label2.Text = "Copyright \u00A9 2026 Bentley Systems, Incorporated";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.label3.Name = "label3";
            this.label3.Text = "Copyright \u00A9 2026 Bentley Systems, Incorporated. All rights reserved.\r\n\r\nIncluding software, file formats, and audiovisual displays; may only be used pursuant to applicable software license agreement; contains confidential and proprietary information of Bentley Systems, Incorporated and/or third parties which is protected by copyright and trade secret law and may not be provided or otherwise made available without proper authorization.\r\n\r\nTRADEMARK NOTICE\r\nBentley and the \"B\" Bentley logo are registered or non-registered trademarks of Bentley Systems, Inc. or Bentley Software, Inc.";
            // 
            // buttonRowPanel
            // 
            this.buttonRowPanel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.buttonRowPanel.AutoSize = true;
            this.buttonRowPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.buttonRowPanel.Controls.Add(this.buttonOK);
            this.buttonRowPanel.Controls.Add(this.buttonLegal);
            this.buttonRowPanel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.buttonRowPanel.Name = "buttonRowPanel";
            // 
            // buttonOK
            // 
            this.buttonOK.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.Text = "Ok";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // buttonLegal
            // 
            this.buttonLegal.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.buttonLegal.Name = "buttonLegal";
            this.buttonLegal.Size = new System.Drawing.Size(154, 23);
            this.buttonLegal.Text = "Legal and Patent Notices";
            this.buttonLegal.UseVisualStyleBackColor = true;
            this.buttonLegal.Click += new System.EventHandler(this.ButtonLegal_Click);
            // 
            // rootLayoutPanel
            // 
            this.rootLayoutPanel.ColumnCount = 1;
            this.rootLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.rootLayoutPanel.Controls.Add(this.groupBox1, 0, 0);
            this.rootLayoutPanel.Controls.Add(this.label3, 0, 1);
            this.rootLayoutPanel.Controls.Add(this.buttonRowPanel, 0, 2);
            this.rootLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rootLayoutPanel.Name = "rootLayoutPanel";
            this.rootLayoutPanel.Padding = new System.Windows.Forms.Padding(12);
            this.rootLayoutPanel.RowCount = 3;
            this.rootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.rootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.rootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(560, 320);
            this.Controls.Add(this.rootLayoutPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About Bentley Configuration Explorer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AboutForm_FormClosing);
            this.Load += new System.EventHandler(this.About_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBoxLayoutPanel.ResumeLayout(false);
            this.groupBoxLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.infoLayoutPanel.ResumeLayout(false);
            this.infoLayoutPanel.PerformLayout();
            this.buttonRowPanel.ResumeLayout(false);
            this.rootLayoutPanel.ResumeLayout(false);
            this.rootLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

            }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel groupBoxLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel infoLayoutPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.FlowLayoutPanel buttonRowPanel;
        private System.Windows.Forms.Button buttonLegal;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelAppName;
        private System.Windows.Forms.TableLayoutPanel rootLayoutPanel;



        }
    }
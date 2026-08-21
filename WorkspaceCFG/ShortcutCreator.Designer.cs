using System;
using System.Diagnostics;
using System.Drawing;

namespace WorkspaceCFG
{
    public partial class ShortcutCreator : System.Windows.Forms.Form
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
            btnCancel = new System.Windows.Forms.Button();
            btnCancel.Click += new EventHandler(BtnCancel_Click);
            tbConfigFile = new System.Windows.Forms.TextBox();
            tbConfigFile.TextChanged += new EventHandler(TbConfigFile_TextChanged);
            cmb_APP1 = new System.Windows.Forms.ComboBox();
            cmb_APP1.SelectedIndexChanged += new EventHandler(Cmb_APP1_SelectedIndexChanged);
            Label3 = new System.Windows.Forms.Label();
            Label4 = new System.Windows.Forms.Label();
            btnBrowseCFG = new System.Windows.Forms.Button();
            btnBrowseCFG.Click += new EventHandler(BtnBrowseCFG_Click);
            Label9 = new System.Windows.Forms.Label();
            btnBrowseOutPut = new System.Windows.Forms.Button();
            btnBrowseOutPut.Click += new EventHandler(BtnBrowseOutPut_Click);
            Label10 = new System.Windows.Forms.Label();
            btnBrowseTargetApp = new System.Windows.Forms.Button();
            btnBrowseTargetApp.Click += new EventHandler(BtnBrowseTargetApp_Click);
            FolderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            OpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            Label11 = new System.Windows.Forms.Label();
            tbAddCmdArgs = new System.Windows.Forms.TextBox();
            tbAddCmdArgs.TextChanged += new EventHandler(TbAddCmdArgs_TextChanged);
            tbOutPutFolder = new System.Windows.Forms.TextBox();
            Label5 = new System.Windows.Forms.Label();
            tbShortcutName = new System.Windows.Forms.TextBox();
            Label8 = new System.Windows.Forms.Label();
            Label7 = new System.Windows.Forms.Label();
            tbTargetApp = new System.Windows.Forms.TextBox();
            tbTargetApp.TextChanged += new EventHandler(TbTargetApp_TextChanged);
            Label1 = new System.Windows.Forms.Label();
            btnCreate = new System.Windows.Forms.Button();
            btnCreate.Click += new EventHandler(BtnCreate_Click);
            Label2 = new System.Windows.Forms.Label();
            tbCommandPreview = new System.Windows.Forms.TextBox();
            Label6 = new System.Windows.Forms.Label();
            RootLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            TargetAppRowPanel = new System.Windows.Forms.TableLayoutPanel();
            ConfigFileRowPanel = new System.Windows.Forms.TableLayoutPanel();
            OutputRowPanel = new System.Windows.Forms.TableLayoutPanel();
            ButtonFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.AutoSize = true;
            btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            btnCancel.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCancel.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            btnCancel.MinimumSize = new Size(81, 31);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new System.Windows.Forms.Padding(8, 2, 8, 2);
            btnCancel.TabIndex = 0;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // tbConfigFile
            // 
            tbConfigFile.Dock = System.Windows.Forms.DockStyle.Fill;
            tbConfigFile.Font = new Font("Segoe UI", 8.0f);
            tbConfigFile.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            tbConfigFile.Name = "tbConfigFile";
            tbConfigFile.TabIndex = 7;
            tbConfigFile.Text = "-wc";
            // 
            // cmb_APP1
            // 
            cmb_APP1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            cmb_APP1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            cmb_APP1.DropDownWidth = 300;
            cmb_APP1.Font = new Font("Segoe UI", 8.0f);
            cmb_APP1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            cmb_APP1.MaxDropDownItems = 25;
            cmb_APP1.Name = "cmb_APP1";
            cmb_APP1.Size = new Size(320, 21);
            cmb_APP1.Sorted = true;
            cmb_APP1.TabIndex = 20;
            // 
            // Label3
            // 
            Label3.AutoSize = true;
            Label3.Font = new Font("Segoe UI", 8.0f);
            Label3.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            Label3.Name = "Label3";
            Label3.TabIndex = 21;
            Label3.Text = "Use Ex App as S Template";
            // 
            // Label4
            // 
            Label4.AutoSize = true;
            Label4.Font = new Font("Segoe UI", 8.0f);
            Label4.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            Label4.Name = "Label4";
            Label4.TabIndex = 23;
            Label4.Text = "App CFG File";
            // 
            // btnBrowseCFG
            // 
            btnBrowseCFG.Anchor = System.Windows.Forms.AnchorStyles.Right;
            btnBrowseCFG.Margin = new System.Windows.Forms.Padding(0);
            btnBrowseCFG.Name = "btnBrowseCFG";
            btnBrowseCFG.Size = new Size(35, 19);
            btnBrowseCFG.TabIndex = 33;
            btnBrowseCFG.Text = "...";
            btnBrowseCFG.UseVisualStyleBackColor = true;
            // 
            // Label9
            // 
            Label9.AutoSize = true;
            Label9.Font = new Font("Segoe UI", 8.0f);
            Label9.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            Label9.Name = "Label9";
            Label9.TabIndex = 38;
            Label9.Text = "Output";
            // 
            // btnBrowseOutPut
            // 
            btnBrowseOutPut.Anchor = System.Windows.Forms.AnchorStyles.Right;
            btnBrowseOutPut.Margin = new System.Windows.Forms.Padding(0);
            btnBrowseOutPut.Name = "btnBrowseOutPut";
            btnBrowseOutPut.Size = new Size(35, 19);
            btnBrowseOutPut.TabIndex = 39;
            btnBrowseOutPut.Text = "...";
            btnBrowseOutPut.UseVisualStyleBackColor = true;
            // 
            // Label10
            // 
            Label10.AutoSize = true;
            Label10.Font = new Font("Segoe UI", 8.0f);
            Label10.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            Label10.Name = "Label10";
            Label10.TabIndex = 41;
            Label10.Text = "Tgt App";
            // 
            // btnBrowseTargetApp
            // 
            btnBrowseTargetApp.Anchor = System.Windows.Forms.AnchorStyles.Right;
            btnBrowseTargetApp.Font = new Font("Segoe UI", 8.0f);
            btnBrowseTargetApp.Margin = new System.Windows.Forms.Padding(0);
            btnBrowseTargetApp.Name = "btnBrowseTargetApp";
            btnBrowseTargetApp.Size = new Size(35, 19);
            btnBrowseTargetApp.TabIndex = 42;
            btnBrowseTargetApp.Text = "...";
            btnBrowseTargetApp.UseVisualStyleBackColor = true;
            // 
            // OpenFileDialog1
            // 
            OpenFileDialog1.FileName = "OpenFileDialog1";
            // 
            // Label11
            // 
            Label11.AutoSize = true;
            Label11.Font = new Font("Segoe UI", 8.0f);
            Label11.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            Label11.Name = "Label11";
            Label11.TabIndex = 44;
            Label11.Text = "Add Cmd Ln Args";
            // 
            // tbAddCmdArgs
            // 
            tbAddCmdArgs.Dock = System.Windows.Forms.DockStyle.Top;
            tbAddCmdArgs.Font = new Font("Segoe UI", 8.0f);
            tbAddCmdArgs.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            tbAddCmdArgs.Name = "tbAddCmdArgs";
            tbAddCmdArgs.TabIndex = 43;
            // 
            // tbOutPutFolder
            // 
            tbOutPutFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            tbOutPutFolder.Font = new Font("Segoe UI", 8.0f);
            tbOutPutFolder.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            tbOutPutFolder.Name = "tbOutPutFolder";
            tbOutPutFolder.TabIndex = 37;
            tbOutPutFolder.Text = @"C:\";
            // 
            // Label5
            // 
            Label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            Label5.Dock = System.Windows.Forms.DockStyle.Top;
            Label5.Height = 2;
            Label5.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);
            Label5.Name = "Label5";
            Label5.TabIndex = 49;
            // 
            // tbShortcutName
            // 
            tbShortcutName.Dock = System.Windows.Forms.DockStyle.Top;
            tbShortcutName.Font = new Font("Segoe UI", 8.0f);
            tbShortcutName.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            tbShortcutName.Name = "tbShortcutName";
            tbShortcutName.TabIndex = 50;
            // 
            // Label8
            // 
            Label8.AutoSize = true;
            Label8.Dock = System.Windows.Forms.DockStyle.Top;
            Label8.Font = new Font("Segoe UI", 8.0f);
            Label8.Margin = new System.Windows.Forms.Padding(0);
            Label8.Name = "Label8";
            Label8.TabIndex = 51;
            Label8.Text = "S Name";
            // 
            // Label7
            // 
            Label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            Label7.Dock = System.Windows.Forms.DockStyle.Top;
            Label7.Height = 2;
            Label7.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);
            Label7.Name = "Label7";
            Label7.TabIndex = 52;
            // 
            // tbTargetApp
            // 
            tbTargetApp.Dock = System.Windows.Forms.DockStyle.Fill;
            tbTargetApp.Font = new Font("Segoe UI", 8.0f);
            tbTargetApp.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            tbTargetApp.Name = "tbTargetApp";
            tbTargetApp.TabIndex = 53;
            // 
            // Label1
            // 
            Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            Label1.Dock = System.Windows.Forms.DockStyle.Top;
            Label1.Height = 2;
            Label1.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);
            Label1.Name = "Label1";
            Label1.TabIndex = 54;
            // 
            // btnCreate
            // 
            btnCreate.AutoSize = true;
            btnCreate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            btnCreate.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCreate.Margin = new System.Windows.Forms.Padding(0);
            btnCreate.MinimumSize = new Size(81, 31);
            btnCreate.Name = "btnCreate";
            btnCreate.Padding = new System.Windows.Forms.Padding(8, 2, 8, 2);
            btnCreate.TabIndex = 0;
            btnCreate.Text = "Create";
            btnCreate.UseVisualStyleBackColor = true;
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.Dock = System.Windows.Forms.DockStyle.Top;
            Label2.Font = new Font("Segoe UI", 8.0f);
            Label2.Margin = new System.Windows.Forms.Padding(0);
            Label2.Name = "Label2";
            Label2.TabIndex = 44;
            Label2.Text = "Shortcut Command Line Preview:";
            // 
            // tbCommandPreview
            // 
            tbCommandPreview.Dock = System.Windows.Forms.DockStyle.Top;
            tbCommandPreview.Enabled = false;
            tbCommandPreview.Font = new Font("Segoe UI", 8.0f);
            tbCommandPreview.Height = 50;
            tbCommandPreview.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            tbCommandPreview.Multiline = true;
            tbCommandPreview.Name = "tbCommandPreview";
            tbCommandPreview.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            tbCommandPreview.TabIndex = 55;
            // 
            // Label6
            // 
            Label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            Label6.Dock = System.Windows.Forms.DockStyle.Top;
            Label6.Height = 2;
            Label6.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);
            Label6.Name = "Label6";
            Label6.TabIndex = 56;
            // 
            // RootLayoutPanel
            // 
            RootLayoutPanel.ColumnCount = 1;
            RootLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            RootLayoutPanel.Controls.Add(Label3, 0, 0);
            RootLayoutPanel.Controls.Add(cmb_APP1, 0, 1);
            RootLayoutPanel.Controls.Add(Label5, 0, 2);
            RootLayoutPanel.Controls.Add(Label8, 0, 3);
            RootLayoutPanel.Controls.Add(tbShortcutName, 0, 4);
            RootLayoutPanel.Controls.Add(Label7, 0, 5);
            RootLayoutPanel.Controls.Add(Label10, 0, 6);
            RootLayoutPanel.Controls.Add(TargetAppRowPanel, 0, 7);
            RootLayoutPanel.Controls.Add(Label4, 0, 8);
            RootLayoutPanel.Controls.Add(ConfigFileRowPanel, 0, 9);
            RootLayoutPanel.Controls.Add(Label11, 0, 10);
            RootLayoutPanel.Controls.Add(tbAddCmdArgs, 0, 11);
            RootLayoutPanel.Controls.Add(Label1, 0, 12);
            RootLayoutPanel.Controls.Add(Label2, 0, 13);
            RootLayoutPanel.Controls.Add(tbCommandPreview, 0, 14);
            RootLayoutPanel.Controls.Add(Label6, 0, 15);
            RootLayoutPanel.Controls.Add(Label9, 0, 16);
            RootLayoutPanel.Controls.Add(OutputRowPanel, 0, 17);
            RootLayoutPanel.Controls.Add(ButtonFlowPanel, 0, 18);
            RootLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            RootLayoutPanel.Name = "RootLayoutPanel";
            RootLayoutPanel.Padding = new System.Windows.Forms.Padding(19, 14, 19, 12);
            RootLayoutPanel.RowCount = 19;
            for (int i = 0; i < 19; i++)
                RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // TargetAppRowPanel
            // 
            TargetAppRowPanel.AutoSize = true;
            TargetAppRowPanel.ColumnCount = 2;
            TargetAppRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            TargetAppRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            TargetAppRowPanel.Controls.Add(tbTargetApp, 0, 0);
            TargetAppRowPanel.Controls.Add(btnBrowseTargetApp, 1, 0);
            TargetAppRowPanel.Dock = System.Windows.Forms.DockStyle.Top;
            TargetAppRowPanel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            TargetAppRowPanel.Name = "TargetAppRowPanel";
            TargetAppRowPanel.RowCount = 1;
            TargetAppRowPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // ConfigFileRowPanel
            // 
            ConfigFileRowPanel.AutoSize = true;
            ConfigFileRowPanel.ColumnCount = 2;
            ConfigFileRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ConfigFileRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            ConfigFileRowPanel.Controls.Add(tbConfigFile, 0, 0);
            ConfigFileRowPanel.Controls.Add(btnBrowseCFG, 1, 0);
            ConfigFileRowPanel.Dock = System.Windows.Forms.DockStyle.Top;
            ConfigFileRowPanel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            ConfigFileRowPanel.Name = "ConfigFileRowPanel";
            ConfigFileRowPanel.RowCount = 1;
            ConfigFileRowPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // OutputRowPanel
            // 
            OutputRowPanel.AutoSize = true;
            OutputRowPanel.ColumnCount = 2;
            OutputRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            OutputRowPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            OutputRowPanel.Controls.Add(tbOutPutFolder, 0, 0);
            OutputRowPanel.Controls.Add(btnBrowseOutPut, 1, 0);
            OutputRowPanel.Dock = System.Windows.Forms.DockStyle.Top;
            OutputRowPanel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            OutputRowPanel.Name = "OutputRowPanel";
            OutputRowPanel.RowCount = 1;
            OutputRowPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // ButtonFlowPanel
            // 
            ButtonFlowPanel.AutoSize = true;
            ButtonFlowPanel.Controls.Add(btnCreate);
            ButtonFlowPanel.Controls.Add(btnCancel);
            ButtonFlowPanel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            ButtonFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            ButtonFlowPanel.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
            ButtonFlowPanel.Name = "ButtonFlowPanel";
            ButtonFlowPanel.WrapContents = false;
            // 
            // ShortcutCreator
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(495, 530);
            Controls.Add(RootLayoutPanel);
            Font = new Font("Segoe UI", 8.0f);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            MaximumSize = new Size(515, 573);
            MinimumSize = new Size(515, 573);
            Name = "ShortcutCreator";
            ShowIcon = false;
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            FormClosing += new System.Windows.Forms.FormClosingEventHandler(IconCreator_FormClosing);
            Shown += new EventHandler(IconCreator_Shown);
            RootLayoutPanel.ResumeLayout(false);
            RootLayoutPanel.PerformLayout();
            TargetAppRowPanel.ResumeLayout(false);
            TargetAppRowPanel.PerformLayout();
            ConfigFileRowPanel.ResumeLayout(false);
            ConfigFileRowPanel.PerformLayout();
            OutputRowPanel.ResumeLayout(false);
            OutputRowPanel.PerformLayout();
            ButtonFlowPanel.ResumeLayout(false);
            ButtonFlowPanel.PerformLayout();
            ResumeLayout(false);

        }
        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.TextBox tbConfigFile;
        internal System.Windows.Forms.ComboBox cmb_APP1;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Button btnBrowseCFG;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.TextBox tbOutPutFolder;
        internal System.Windows.Forms.Button btnBrowseOutPut;
        internal System.Windows.Forms.Label Label10;
        internal System.Windows.Forms.Button btnBrowseTargetApp;
        internal System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog1;
        internal System.Windows.Forms.OpenFileDialog OpenFileDialog1;
        internal System.Windows.Forms.TextBox tbAddCmdArgs;
        internal System.Windows.Forms.Label Label11;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.TextBox tbShortcutName;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.TextBox tbTargetApp;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Button btnCreate;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox tbCommandPreview;
        internal System.Windows.Forms.Label Label6;
        private System.Windows.Forms.TableLayoutPanel RootLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel TargetAppRowPanel;
        private System.Windows.Forms.TableLayoutPanel ConfigFileRowPanel;
        private System.Windows.Forms.TableLayoutPanel OutputRowPanel;
        private System.Windows.Forms.FlowLayoutPanel ButtonFlowPanel;
    }
}
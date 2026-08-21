using System;
using System.Diagnostics;
using System.Drawing;
using WorkspaceCFG.My;

namespace WorkspaceCFG
{
    public partial class ConfigurationCompare : System.Windows.Forms.Form
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
            var MySettings1 = new MySettings();
            var MySettings2 = new MySettings();
            RootLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            SidesPanel = new System.Windows.Forms.TableLayoutPanel();
            Side1Panel = new System.Windows.Forms.TableLayoutPanel();
            RadioButtonConfiguration1 = new System.Windows.Forms.RadioButton();
            RadioButtonConfiguration1.CheckedChanged += new EventHandler(RadioButtonWorkspace1_CheckedChanged);
            ConfigurationSelection1 = new ConfigurationSelection();
            RadioButtonFile1 = new System.Windows.Forms.RadioButton();
            FileRowPanel1 = new System.Windows.Forms.TableLayoutPanel();
            TextBoxFile1 = new System.Windows.Forms.TextBox();
            ButtonBrowse1 = new System.Windows.Forms.Button();
            ButtonBrowse1.Click += new EventHandler(ButtonBrowse1_Click);
            Side2Panel = new System.Windows.Forms.TableLayoutPanel();
            RadioButtonConfiguration2 = new System.Windows.Forms.RadioButton();
            RadioButtonConfiguration2.CheckedChanged += new EventHandler(RadioButtonWorkspace2_CheckedChanged);
            ConfigurationSelection2 = new ConfigurationSelection();
            RadioButtonFile2 = new System.Windows.Forms.RadioButton();
            FileRowPanel2 = new System.Windows.Forms.TableLayoutPanel();
            TextBoxFile2 = new System.Windows.Forms.TextBox();
            ButtonBrowse2 = new System.Windows.Forms.Button();
            ButtonBrowse2.Click += new EventHandler(ButtonBrowse2_Click);
            ButtonRowPanel = new System.Windows.Forms.FlowLayoutPanel();
            ButtonOK = new System.Windows.Forms.Button();
            ButtonOK.Click += new EventHandler(ButtonOK_Click);
            ButtonCancel = new System.Windows.Forms.Button();
            ButtonCancel.Click += new EventHandler(ButtonCancel_Click);
            RootLayoutPanel.SuspendLayout();
            SidesPanel.SuspendLayout();
            Side1Panel.SuspendLayout();
            FileRowPanel1.SuspendLayout();
            Side2Panel.SuspendLayout();
            FileRowPanel2.SuspendLayout();
            ButtonRowPanel.SuspendLayout();
            SuspendLayout();
            // 
            // RootLayoutPanel
            // 
            RootLayoutPanel.ColumnCount = 1;
            RootLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            RootLayoutPanel.Controls.Add(SidesPanel, 0, 0);
            RootLayoutPanel.Controls.Add(ButtonRowPanel, 0, 1);
            RootLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            RootLayoutPanel.Name = "RootLayoutPanel";
            RootLayoutPanel.Padding = new System.Windows.Forms.Padding(12, 12, 12, 11);
            RootLayoutPanel.RowCount = 2;
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // SidesPanel
            // 
            SidesPanel.ColumnCount = 2;
            SidesPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            SidesPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            SidesPanel.Controls.Add(Side1Panel, 0, 0);
            SidesPanel.Controls.Add(Side2Panel, 1, 0);
            SidesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            SidesPanel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            SidesPanel.Name = "SidesPanel";
            SidesPanel.RowCount = 1;
            SidesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            // 
            // Side1Panel
            // 
            Side1Panel.ColumnCount = 1;
            Side1Panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            Side1Panel.Controls.Add(RadioButtonConfiguration1, 0, 0);
            Side1Panel.Controls.Add(ConfigurationSelection1, 0, 1);
            Side1Panel.Controls.Add(RadioButtonFile1, 0, 2);
            Side1Panel.Controls.Add(FileRowPanel1, 0, 3);
            Side1Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            Side1Panel.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            Side1Panel.Name = "Side1Panel";
            Side1Panel.RowCount = 4;
            Side1Panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            Side1Panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            Side1Panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            Side1Panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // RadioButtonConfiguration1
            // 
            RadioButtonConfiguration1.AutoSize = true;
            RadioButtonConfiguration1.Checked = true;
            RadioButtonConfiguration1.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            RadioButtonConfiguration1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            RadioButtonConfiguration1.Name = "RadioButtonWorkspace1";
            RadioButtonConfiguration1.TabIndex = 17;
            RadioButtonConfiguration1.TabStop = true;
            RadioButtonConfiguration1.Text = "Load From Current Configuration";
            RadioButtonConfiguration1.UseVisualStyleBackColor = true;
            // 
            // ConfigurationSelection1
            // 
            ConfigurationSelection1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ConfigurationSelection1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            ConfigurationSelection1.Name = "WorkspaceSelection1";
            ConfigurationSelection1.Ready = false;
            ConfigurationSelection1.TabIndex = 23;
            // 
            // RadioButtonFile1
            // 
            RadioButtonFile1.AutoSize = true;
            RadioButtonFile1.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            RadioButtonFile1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            RadioButtonFile1.Name = "RadioButtonFile1";
            RadioButtonFile1.TabIndex = 22;
            RadioButtonFile1.Text = "Load From Configuration File";
            RadioButtonFile1.UseVisualStyleBackColor = true;
            // 
            // FileRowPanel1
            // 
            FileRowPanel1.ColumnCount = 2;
            FileRowPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            FileRowPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            FileRowPanel1.Controls.Add(TextBoxFile1, 0, 0);
            FileRowPanel1.Controls.Add(ButtonBrowse1, 1, 0);
            FileRowPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            FileRowPanel1.Name = "FileRowPanel1";
            FileRowPanel1.RowCount = 1;
            FileRowPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // TextBoxFile1
            // 
            MySettings1.AutoloadLastWorkspace = true;
            MySettings1.AutoloadWorkspaceSelector = false;
            MySettings1.Chosen_LOCAL = false;
            MySettings1.Chosen_OOB = false;
            MySettings1.ChosenWorkset = "";
            MySettings1.ChosenWorkspace = "";
            MySettings1.ChosenApplication = "";
            MySettings1.ChosenApplicationPath = "";
            MySettings1.EH_Location = new Point(40, 40);
            MySettings1.EH_Size = new Size(833, 668);
            MySettings1.EH_StartPos = System.Windows.Forms.FormStartPosition.Manual;
            MySettings1.EH_WindowState = System.Windows.Forms.FormWindowState.Normal;
            MySettings1.FH_Location = new Point(40, 40);
            MySettings1.FH_Size = new Size(891, 551);
            MySettings1.FH_Split1_Dist = 161;
            MySettings1.FH_WindowState = System.Windows.Forms.FormWindowState.Normal;
            MySettings1.FV_Location = new Point(40, 40);
            MySettings1.FV_Size = new Size(834, 659);
            MySettings1.FV_StartPos = System.Windows.Forms.FormStartPosition.Manual;
            MySettings1.FV_WindowState = System.Windows.Forms.FormWindowState.Normal;
            MySettings1.GC_Location = new Point(40, 40);
            MySettings1.GC_StartPos = System.Windows.Forms.FormStartPosition.Manual;
            MySettings1.GroupWindows = true;
            MySettings1.Loc_Location = new Point(40, 40);
            MySettings1.Loc_Size = new Size(677, 295);
            MySettings1.Loc_WindowState = System.Windows.Forms.FormWindowState.Normal;
            MySettings1.MAIN_Location = new Point(40, 40);
            MySettings1.Main_StartPos = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            MySettings1.PS_Location = new Point(40, 40);
            MySettings1.PS_StartPos = System.Windows.Forms.FormStartPosition.CenterParent;
            MySettings1.ResolveFilePaths = false;
            MySettings1.SettingsKey = "";
            MySettings1.Shortcut_Args = "";
            MySettings1.Shortcut_Out = @"C:\";
            MySettings1.SRCH_Location = new Point(40, 40);
            MySettings1.SRCH_Size = new Size(775, 425);
            MySettings1.SRCH_WindowState = System.Windows.Forms.FormWindowState.Normal;
            MySettings1.ST_Location = new Point(40, 40);
            MySettings1.ST_StartPos = System.Windows.Forms.FormStartPosition.Manual;
            MySettings1.VC_Location = new Point(40, 40);
            MySettings1.VC_Size = new Size(897, 660);
            MySettings1.VC_Split1_Dist = 406;
            MySettings1.VC_Split2_Dist = 406;
            MySettings1.VC_Split3_Dist = 356;
            MySettings1.VC_StartPos = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            MySettings1.VC_WindowState = System.Windows.Forms.FormWindowState.Normal;
            MySettings1.VE_C1 = true;
            MySettings1.VE_C2 = true;
            MySettings1.VE_C3 = true;
            MySettings1.VE_C4 = false;
            MySettings1.VE_C5 = false;
            MySettings1.VE_C6 = true;
            MySettings1.VE_Location = new Point(40, 40);
            MySettings1.VE_MULTILINE = false;
            MySettings1.VE_Size = new Size(1023, 881);
            MySettings1.VE_Split1_Dist = 275;
            MySettings1.VE_StartPos = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            MySettings1.VE_WindowState = System.Windows.Forms.FormWindowState.Normal;
            MySettings1.VG_Location = new Point(40, 40);
            MySettings1.VG_Size = new Size(653, 447);
            MySettings1.VG_StartPos = System.Windows.Forms.FormStartPosition.Manual;
            MySettings1.VG_WindowState = System.Windows.Forms.FormWindowState.Normal;
            MySettings1.VL_Location = new Point(40, 40);
            MySettings1.VL_Size = new Size(935, 729);
            MySettings1.VL_StartPos = System.Windows.Forms.FormStartPosition.Manual;
            MySettings1.VL_WindowState = System.Windows.Forms.FormWindowState.Normal;
            MySettings1.VR_Location = new Point(40, 40);
            MySettings1.VR_Size = new Size(759, 763);
            MySettings1.VR_Split2_Dist = 185;
            MySettings1.VR_Split3_Dist = 390;
            MySettings1.VR_StartPos = System.Windows.Forms.FormStartPosition.Manual;
            MySettings1.VR_WindowState = System.Windows.Forms.FormWindowState.Normal;
            MySettings1.WRK1_ChosenFile = "";
            MySettings1.WRK1_ChosenWorkset = "untitled";
            MySettings1.WRK1_ChosenWorkspace = "untitled";
            MySettings1.WRK1_ChosenApplication = "";
            MySettings1.WRK1_OP1 = false;
            MySettings1.WRK1_OP2 = false;
            MySettings1.WRK2_ChosenFile = "";
            MySettings1.WRK2_ChosenWorkset = "untitled";
            MySettings1.WRK2_ChosenWorkspace = "untitled";
            MySettings1.WRK2_ChosenApplication = "";
            MySettings1.WRK2_OP1 = true;
            MySettings1.WRK2_OP2 = false;
            TextBoxFile1.DataBindings.Add(new System.Windows.Forms.Binding("Text", MySettings1, "WRK1_ChosenFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            TextBoxFile1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TextBoxFile1.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            TextBoxFile1.Name = "TextBoxFile1";
            TextBoxFile1.TabIndex = 21;
            TextBoxFile1.Text = MySettings1.WRK1_ChosenFile;
            // 
            // ButtonBrowse1
            // 
            ButtonBrowse1.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            ButtonBrowse1.Margin = new System.Windows.Forms.Padding(0);
            ButtonBrowse1.Name = "ButtonBrowse1";
            ButtonBrowse1.Size = new Size(66, 25);
            ButtonBrowse1.TabIndex = 20;
            ButtonBrowse1.Text = "Browse";
            ButtonBrowse1.UseVisualStyleBackColor = true;
            // 
            // Side2Panel
            // 
            Side2Panel.ColumnCount = 1;
            Side2Panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            Side2Panel.Controls.Add(RadioButtonConfiguration2, 0, 0);
            Side2Panel.Controls.Add(ConfigurationSelection2, 0, 1);
            Side2Panel.Controls.Add(RadioButtonFile2, 0, 2);
            Side2Panel.Controls.Add(FileRowPanel2, 0, 3);
            Side2Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            Side2Panel.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            Side2Panel.Name = "Side2Panel";
            Side2Panel.RowCount = 4;
            Side2Panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            Side2Panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            Side2Panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            Side2Panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // RadioButtonConfiguration2
            // 
            RadioButtonConfiguration2.AutoSize = true;
            RadioButtonConfiguration2.Checked = true;
            RadioButtonConfiguration2.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            RadioButtonConfiguration2.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            RadioButtonConfiguration2.Name = "RadioButtonWorkspace2";
            RadioButtonConfiguration2.TabIndex = 17;
            RadioButtonConfiguration2.TabStop = true;
            RadioButtonConfiguration2.Text = "Load From Current Workspace";
            RadioButtonConfiguration2.UseVisualStyleBackColor = true;
            // 
            // ConfigurationSelection2
            // 
            ConfigurationSelection2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ConfigurationSelection2.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            ConfigurationSelection2.Name = "WorkspaceSelection2";
            ConfigurationSelection2.Ready = false;
            ConfigurationSelection2.TabIndex = 23;
            // 
            // RadioButtonFile2
            // 
            RadioButtonFile2.AutoSize = true;
            RadioButtonFile2.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            RadioButtonFile2.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            RadioButtonFile2.Name = "RadioButtonFile2";
            RadioButtonFile2.TabIndex = 22;
            RadioButtonFile2.Text = "Load From Configuration File";
            RadioButtonFile2.UseVisualStyleBackColor = true;
            // 
            // FileRowPanel2
            // 
            FileRowPanel2.ColumnCount = 2;
            FileRowPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            FileRowPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            FileRowPanel2.Controls.Add(TextBoxFile2, 0, 0);
            FileRowPanel2.Controls.Add(ButtonBrowse2, 1, 0);
            FileRowPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            FileRowPanel2.Name = "FileRowPanel2";
            FileRowPanel2.RowCount = 1;
            FileRowPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // TextBoxFile2
            // 
            MySettings2.AutoloadLastWorkspace = true;
            MySettings2.AutoloadWorkspaceSelector = false;
            MySettings2.Chosen_LOCAL = false;
            MySettings2.Chosen_OOB = false;
            MySettings2.ChosenWorkset = "";
            MySettings2.ChosenWorkspace = "";
            MySettings2.ChosenApplication = "";
            MySettings2.ChosenApplicationPath = "";
            MySettings2.EH_Location = new Point(40, 40);
            MySettings2.EH_Size = new Size(833, 668);
            MySettings2.EH_StartPos = System.Windows.Forms.FormStartPosition.Manual;
            MySettings2.EH_WindowState = System.Windows.Forms.FormWindowState.Normal;
            MySettings2.FH_Location = new Point(40, 40);
            MySettings2.FH_Size = new Size(891, 551);
            MySettings2.FH_Split1_Dist = 161;
            MySettings2.FH_WindowState = System.Windows.Forms.FormWindowState.Normal;
            MySettings2.FV_Location = new Point(40, 40);
            MySettings2.FV_Size = new Size(834, 659);
            MySettings2.FV_StartPos = System.Windows.Forms.FormStartPosition.Manual;
            MySettings2.FV_WindowState = System.Windows.Forms.FormWindowState.Normal;
            MySettings2.GC_Location = new Point(40, 40);
            MySettings2.GC_StartPos = System.Windows.Forms.FormStartPosition.Manual;
            MySettings2.GroupWindows = true;
            MySettings2.Loc_Location = new Point(40, 40);
            MySettings2.Loc_Size = new Size(677, 295);
            MySettings2.Loc_WindowState = System.Windows.Forms.FormWindowState.Normal;
            MySettings2.MAIN_Location = new Point(40, 40);
            MySettings2.Main_StartPos = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            MySettings2.PS_Location = new Point(40, 40);
            MySettings2.PS_StartPos = System.Windows.Forms.FormStartPosition.CenterParent;
            MySettings2.ResolveFilePaths = false;
            MySettings2.SettingsKey = "";
            MySettings2.Shortcut_Args = "";
            MySettings2.Shortcut_Out = @"C:\";
            MySettings2.SRCH_Location = new Point(40, 40);
            MySettings2.SRCH_Size = new Size(775, 425);
            MySettings2.SRCH_WindowState = System.Windows.Forms.FormWindowState.Normal;
            MySettings2.ST_Location = new Point(40, 40);
            MySettings2.ST_StartPos = System.Windows.Forms.FormStartPosition.Manual;
            MySettings2.VC_Location = new Point(40, 40);
            MySettings2.VC_Size = new Size(897, 660);
            MySettings2.VC_Split1_Dist = 406;
            MySettings2.VC_Split2_Dist = 406;
            MySettings2.VC_Split3_Dist = 356;
            MySettings2.VC_StartPos = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            MySettings2.VC_WindowState = System.Windows.Forms.FormWindowState.Normal;
            MySettings2.VE_C1 = true;
            MySettings2.VE_C2 = true;
            MySettings2.VE_C3 = true;
            MySettings2.VE_C4 = false;
            MySettings2.VE_C5 = false;
            MySettings2.VE_C6 = true;
            MySettings2.VE_Location = new Point(40, 40);
            MySettings2.VE_MULTILINE = false;
            MySettings2.VE_Size = new Size(1023, 881);
            MySettings2.VE_Split1_Dist = 275;
            MySettings2.VE_StartPos = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            MySettings2.VE_WindowState = System.Windows.Forms.FormWindowState.Normal;
            MySettings2.VG_Location = new Point(40, 40);
            MySettings2.VG_Size = new Size(653, 447);
            MySettings2.VG_StartPos = System.Windows.Forms.FormStartPosition.Manual;
            MySettings2.VG_WindowState = System.Windows.Forms.FormWindowState.Normal;
            MySettings2.VL_Location = new Point(40, 40);
            MySettings2.VL_Size = new Size(935, 729);
            MySettings2.VL_StartPos = System.Windows.Forms.FormStartPosition.Manual;
            MySettings2.VL_WindowState = System.Windows.Forms.FormWindowState.Normal;
            MySettings2.VR_Location = new Point(40, 40);
            MySettings2.VR_Size = new Size(759, 763);
            MySettings2.VR_Split2_Dist = 185;
            MySettings2.VR_Split3_Dist = 390;
            MySettings2.VR_StartPos = System.Windows.Forms.FormStartPosition.Manual;
            MySettings2.VR_WindowState = System.Windows.Forms.FormWindowState.Normal;
            MySettings2.WRK1_ChosenFile = "";
            MySettings2.WRK1_ChosenWorkset = "untitled";
            MySettings2.WRK1_ChosenWorkspace = "untitled";
            MySettings2.WRK1_ChosenApplication = "";
            MySettings2.WRK1_OP1 = false;
            MySettings2.WRK1_OP2 = false;
            MySettings2.WRK2_ChosenFile = "";
            MySettings2.WRK2_ChosenWorkset = "untitled";
            MySettings2.WRK2_ChosenWorkspace = "untitled";
            MySettings2.WRK2_ChosenApplication = "";
            MySettings2.WRK2_OP1 = true;
            MySettings2.WRK2_OP2 = false;
            TextBoxFile2.DataBindings.Add(new System.Windows.Forms.Binding("Text", MySettings2, "WRK2_ChosenFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            TextBoxFile2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TextBoxFile2.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            TextBoxFile2.Name = "TextBoxFile2";
            TextBoxFile2.TabIndex = 21;
            TextBoxFile2.Text = MySettings2.WRK2_ChosenFile;
            // 
            // ButtonBrowse2
            // 
            ButtonBrowse2.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            ButtonBrowse2.Margin = new System.Windows.Forms.Padding(0);
            ButtonBrowse2.Name = "ButtonBrowse2";
            ButtonBrowse2.Size = new Size(64, 25);
            ButtonBrowse2.TabIndex = 20;
            ButtonBrowse2.Text = "Browse";
            ButtonBrowse2.UseVisualStyleBackColor = true;
            // 
            // ButtonRowPanel
            // 
            ButtonRowPanel.AutoSize = true;
            ButtonRowPanel.Controls.Add(ButtonCancel);
            ButtonRowPanel.Controls.Add(ButtonOK);
            ButtonRowPanel.Dock = System.Windows.Forms.DockStyle.Right;
            ButtonRowPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            ButtonRowPanel.Name = "ButtonRowPanel";
            // 
            // ButtonCancel
            // 
            ButtonCancel.Font = new Font("Segoe UI", 8.0f);
            ButtonCancel.Margin = new System.Windows.Forms.Padding(0);
            ButtonCancel.Name = "ButtonCancel";
            ButtonCancel.Size = new Size(66, 25);
            ButtonCancel.TabIndex = 24;
            ButtonCancel.Text = "Cancel";
            // 
            // ButtonOK
            // 
            ButtonOK.Font = new Font("Segoe UI", 8.0f);
            ButtonOK.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            ButtonOK.Name = "ButtonOK";
            ButtonOK.Size = new Size(58, 24);
            ButtonOK.TabIndex = 23;
            ButtonOK.Text = "OK";
            // 
            // ConfigurationCompare
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(838, 338);
            Controls.Add(RootLayoutPanel);
            Font = new Font("Segoe UI", 8.0f);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "WorkspaceCompare";
            ShowIcon = false;
            Text = "Select Workspaces to Compare";
            FormClosing += new System.Windows.Forms.FormClosingEventHandler(ConfigurationCompare_FormClosing);
            Shown += new EventHandler(ConfigurationCompare_Shown);
            Load += new EventHandler(ConfigurationCompare_Load);
            ButtonRowPanel.ResumeLayout(false);
            FileRowPanel2.ResumeLayout(false);
            FileRowPanel2.PerformLayout();
            Side2Panel.ResumeLayout(false);
            Side2Panel.PerformLayout();
            FileRowPanel1.ResumeLayout(false);
            FileRowPanel1.PerformLayout();
            Side1Panel.ResumeLayout(false);
            Side1Panel.PerformLayout();
            SidesPanel.ResumeLayout(false);
            RootLayoutPanel.ResumeLayout(false);
            RootLayoutPanel.PerformLayout();
            ResumeLayout(false);

        }
        internal System.Windows.Forms.TableLayoutPanel RootLayoutPanel;
        internal System.Windows.Forms.TableLayoutPanel SidesPanel;
        internal System.Windows.Forms.TableLayoutPanel Side1Panel;
        internal System.Windows.Forms.TableLayoutPanel Side2Panel;
        internal System.Windows.Forms.TableLayoutPanel FileRowPanel1;
        internal System.Windows.Forms.TableLayoutPanel FileRowPanel2;
        internal System.Windows.Forms.FlowLayoutPanel ButtonRowPanel;
        internal System.Windows.Forms.Button ButtonCancel;
        internal System.Windows.Forms.Button ButtonOK;
        internal System.Windows.Forms.RadioButton RadioButtonConfiguration1;
        internal System.Windows.Forms.RadioButton RadioButtonConfiguration2;
        internal System.Windows.Forms.Button ButtonBrowse1;
        internal System.Windows.Forms.Button ButtonBrowse2;
        internal System.Windows.Forms.TextBox TextBoxFile1;
        internal System.Windows.Forms.TextBox TextBoxFile2;
        internal System.Windows.Forms.RadioButton RadioButtonFile1;
        internal System.Windows.Forms.RadioButton RadioButtonFile2;
        internal ConfigurationSelection ConfigurationSelection1;
        internal ConfigurationSelection ConfigurationSelection2;
    }
}

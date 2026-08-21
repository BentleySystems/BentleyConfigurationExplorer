using System;
using System.Diagnostics;
using System.Drawing;
using WorkspaceCFG.My;

namespace WorkspaceCFG
{
    public partial class ConfigurationSelection : System.Windows.Forms.UserControl
    {

        // UserControl overrides dispose to clean up the component list.
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
            RootLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            Label3 = new System.Windows.Forms.Label();
            cmbApplication = new System.Windows.Forms.ComboBox();
            cmbApplication.SelectedIndexChanged += new EventHandler(CmbApplication_SelectedIndexChanged);
            Label1 = new System.Windows.Forms.Label();
            cmbWorkpace = new System.Windows.Forms.ComboBox();
            cmbWorkpace.SelectedIndexChanged += new EventHandler(CmbWorkspace_SelectedIndexChanged);
            Label2 = new System.Windows.Forms.Label();
            cmbWorkset = new System.Windows.Forms.ComboBox();
            cmbWorkset.SelectedIndexChanged += new EventHandler(CmbWorkset_SelectedIndexChanged);
            lblRole = new System.Windows.Forms.Label();
            cmbRole = new System.Windows.Forms.ComboBox();
            cmbRole.SelectedIndexChanged += new EventHandler(CmbRole_SelectedIndexChanged);
            RootLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // RootLayoutPanel
            // 
            RootLayoutPanel.AutoSize = true;
            RootLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            RootLayoutPanel.ColumnCount = 1;
            RootLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            RootLayoutPanel.Controls.Add(Label3, 0, 0);
            RootLayoutPanel.Controls.Add(cmbApplication, 0, 1);
            RootLayoutPanel.Controls.Add(Label1, 0, 2);
            RootLayoutPanel.Controls.Add(cmbWorkpace, 0, 3);
            RootLayoutPanel.Controls.Add(Label2, 0, 4);
            RootLayoutPanel.Controls.Add(cmbWorkset, 0, 5);
            RootLayoutPanel.Controls.Add(lblRole, 0, 6);
            RootLayoutPanel.Controls.Add(cmbRole, 0, 7);
            RootLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            RootLayoutPanel.Name = "RootLayoutPanel";
            RootLayoutPanel.RowCount = 8;
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // Label3
            // 
            Label3.AutoSize = true;
            Label3.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            Label3.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            Label3.Name = "Label3";
            Label3.Text = "Application:";
            // 
            // cmbApplication
            // 
            cmbApplication.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmbApplication.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            cmbApplication.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            // MySettings1.AutoloadLastWorkspace = True
            // MySettings1.AutoloadWorkspaceSelector = False
            MySettings1.Chosen_LOCAL = false;
            MySettings1.Chosen_OOB = false;
            MySettings1.ChosenApplication = "";
            MySettings1.ChosenApplicationPath = "";
            MySettings1.ChosenWorkset = "";
            MySettings1.ChosenWorkspace = "";
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
            MySettings1.WRK1_ChosenApplication = "";
            MySettings1.WRK1_ChosenFile = "";
            MySettings1.WRK1_ChosenWorkset = "untitled";
            MySettings1.WRK1_ChosenWorkspace = "untitled";
            MySettings1.WRK1_OP1 = false;
            MySettings1.WRK1_OP2 = false;
            MySettings1.WRK2_ChosenApplication = "";
            MySettings1.WRK2_ChosenFile = "";
            MySettings1.WRK2_ChosenWorkset = "untitled";
            MySettings1.WRK2_ChosenWorkspace = "untitled";
            MySettings1.WRK2_OP1 = true;
            MySettings1.WRK2_OP2 = false;
            cmbApplication.DataBindings.Add(new System.Windows.Forms.Binding("Text", MySettings1, "ChosenApplication", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            cmbApplication.DropDownWidth = 250;
            cmbApplication.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbApplication.ItemHeight = 21;
            cmbApplication.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            cmbApplication.MaxDropDownItems = 25;
            cmbApplication.Name = "cmbApplication";
            cmbApplication.Sorted = true;
            cmbApplication.TabIndex = 14;
            cmbApplication.Text = MySettings1.ChosenApplication;
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            Label1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            Label1.Name = "Label1";
            Label1.TabIndex = 17;
            Label1.Text = "Workspace:";
            // 
            // cmbWorkpace
            // 
            cmbWorkpace.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmbWorkpace.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            cmbWorkpace.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            cmbWorkpace.DropDownWidth = 250;
            cmbWorkpace.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbWorkpace.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            cmbWorkpace.MaxDropDownItems = 25;
            cmbWorkpace.Name = "cmbWorkpace";
            cmbWorkpace.Sorted = true;
            cmbWorkpace.TabIndex = 15;
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            Label2.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            Label2.Name = "Label2";
            Label2.TabIndex = 20;
            Label2.Text = "Workset:";
            // 
            // cmbWorkset
            // 
            cmbWorkset.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmbWorkset.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            cmbWorkset.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            cmbWorkset.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbWorkset.ItemHeight = 21;
            cmbWorkset.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            cmbWorkset.MaxDropDownItems = 25;
            cmbWorkset.Name = "cmbWorkset";
            cmbWorkset.TabIndex = 16;
            // 
            // lblRole
            // 
            lblRole.AutoSize = true;
            lblRole.Enabled = false;
            lblRole.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblRole.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            lblRole.Name = "lblRole";
            lblRole.Text = "Role:";
            // 
            // cmbRole
            // 
            cmbRole.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmbRole.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            cmbRole.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            cmbRole.Enabled = false;
            cmbRole.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbRole.ItemHeight = 21;
            cmbRole.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            cmbRole.MaxDropDownItems = 25;
            cmbRole.Name = "cmbRole";
            cmbRole.TabIndex = 22;
            // 
            // ConfigurationSelection
            // 
            AutoScaleDimensions = new SizeF(9.0f, 21.0f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            Controls.Add(RootLayoutPanel);
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "ConfigurationSelection";
            Size = new Size(404, 286);
            RootLayoutPanel.ResumeLayout(false);
            RootLayoutPanel.PerformLayout();
            ResumeLayout(false);

        }

        internal System.Windows.Forms.TableLayoutPanel RootLayoutPanel;
        internal System.Windows.Forms.CheckBox chkLocal;
        internal System.Windows.Forms.ComboBox cmbApplication;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.ComboBox cmbWorkset;
        internal System.Windows.Forms.ComboBox cmbWorkpace;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.ComboBox cmbRole;
        internal System.Windows.Forms.Label lblRole;
    }
}
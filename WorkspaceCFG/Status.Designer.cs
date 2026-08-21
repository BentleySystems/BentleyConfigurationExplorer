using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace WorkspaceCFG
{
    public partial class Status : System.Windows.Forms.Form
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
            StatusBox = new System.Windows.Forms.GroupBox();
            StatusGridPanel = new System.Windows.Forms.TableLayoutPanel();
            Label11 = new System.Windows.Forms.Label();
            _lblCurrentFile = new System.Windows.Forms.Label();
            Label2 = new System.Windows.Forms.Label();
            _lblDepth = new System.Windows.Forms.Label();
            Label6 = new System.Windows.Forms.Label();
            _lblLineCount = new System.Windows.Forms.Label();
            Link_Variables = new System.Windows.Forms.LinkLabel();
            Link_Variables.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(Link_Variables_LinkClicked);
            _lblVariables = new System.Windows.Forms.Label();
            Link_Files = new System.Windows.Forms.LinkLabel();
            Link_Files.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(Link_Files_LinkClicked);
            _lblFiles = new System.Windows.Forms.Label();
            Link_Events = new System.Windows.Forms.LinkLabel();
            Link_Events.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(Link_Events_LinkClicked);
            _lblEvents = new System.Windows.Forms.Label();
            Link_Warnings = new System.Windows.Forms.LinkLabel();
            Link_Warnings.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(Link_Warnings_LinkClicked);
            _lblWarnings = new System.Windows.Forms.Label();
            Link_Errors = new System.Windows.Forms.LinkLabel();
            Link_Errors.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(Link_Errors_LinkClicked);
            _lblErrors = new System.Windows.Forms.Label();
            ListBoxEcho = new System.Windows.Forms.ListBox();
            RootLayoutPanel.SuspendLayout();
            StatusBox.SuspendLayout();
            StatusGridPanel.SuspendLayout();
            SuspendLayout();
            // 
            // RootLayoutPanel
            // 
            RootLayoutPanel.ColumnCount = 2;
            RootLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 312F));
            RootLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            RootLayoutPanel.Controls.Add(StatusBox, 0, 0);
            RootLayoutPanel.Controls.Add(ListBoxEcho, 1, 0);
            RootLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            RootLayoutPanel.Name = "RootLayoutPanel";
            RootLayoutPanel.Padding = new System.Windows.Forms.Padding(8, 10, 8, 10);
            RootLayoutPanel.RowCount = 1;
            RootLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            // 
            // StatusBox
            // 
            StatusBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            StatusBox.AutoSize = true;
            StatusBox.BackColor = Color.Transparent;
            StatusBox.Controls.Add(StatusGridPanel);
            StatusBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            StatusBox.Font = new Font("Segoe UI", 8.0f, FontStyle.Bold, GraphicsUnit.Point, 0);
            StatusBox.ForeColor = SystemColors.ActiveBorder;
            StatusBox.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            StatusBox.Name = "StatusBox";
            StatusBox.TabIndex = 10;
            StatusBox.TabStop = false;
            // 
            // StatusGridPanel
            // 
            StatusGridPanel.AutoSize = true;
            StatusGridPanel.ColumnCount = 2;
            StatusGridPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            StatusGridPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            StatusGridPanel.Controls.Add(Label11, 0, 0);
            StatusGridPanel.Controls.Add(_lblCurrentFile, 1, 0);
            StatusGridPanel.Controls.Add(Label2, 0, 1);
            StatusGridPanel.Controls.Add(_lblDepth, 1, 1);
            StatusGridPanel.Controls.Add(Label6, 0, 2);
            StatusGridPanel.Controls.Add(_lblLineCount, 1, 2);
            StatusGridPanel.Controls.Add(Link_Variables, 0, 3);
            StatusGridPanel.Controls.Add(_lblVariables, 1, 3);
            StatusGridPanel.Controls.Add(Link_Files, 0, 4);
            StatusGridPanel.Controls.Add(_lblFiles, 1, 4);
            StatusGridPanel.Controls.Add(Link_Events, 0, 5);
            StatusGridPanel.Controls.Add(_lblEvents, 1, 5);
            StatusGridPanel.Controls.Add(Link_Warnings, 0, 6);
            StatusGridPanel.Controls.Add(_lblWarnings, 1, 6);
            StatusGridPanel.Controls.Add(Link_Errors, 0, 7);
            StatusGridPanel.Controls.Add(_lblErrors, 1, 7);
            StatusGridPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            StatusGridPanel.Name = "StatusGridPanel";
            StatusGridPanel.Padding = new System.Windows.Forms.Padding(9, 7, 9, 7);
            StatusGridPanel.RowCount = 8;
            StatusGridPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            StatusGridPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            StatusGridPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            StatusGridPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            StatusGridPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            StatusGridPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            StatusGridPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            StatusGridPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            // 
            // Label11
            // 
            Label11.AutoSize = true;
            Label11.Font = new Font("Segoe UI", 8.0f, FontStyle.Bold, GraphicsUnit.Point, 0);
            Label11.ForeColor = SystemColors.WindowText;
            Label11.Margin = new System.Windows.Forms.Padding(0, 3, 6, 3);
            Label11.Name = "Label11";
            Label11.Text = "Current File:";
            // 
            // lblCurrentFile
            // 
            _lblCurrentFile.AutoSize = true;
            _lblCurrentFile.ForeColor = SystemColors.WindowText;
            _lblCurrentFile.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            _lblCurrentFile.Name = "_lblCurrentFile";
            _lblCurrentFile.Text = "Current File:";
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.Font = new Font("Segoe UI", 8.0f, FontStyle.Bold, GraphicsUnit.Point, 0);
            Label2.ForeColor = SystemColors.WindowText;
            Label2.Margin = new System.Windows.Forms.Padding(0, 3, 6, 3);
            Label2.Name = "Label2";
            Label2.Text = "Depth:";
            // 
            // lblDepth
            // 
            _lblDepth.AutoSize = true;
            _lblDepth.ForeColor = SystemColors.WindowText;
            _lblDepth.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            _lblDepth.Name = "_lblDepth";
            _lblDepth.Text = "Depth";
            // 
            // Label6
            // 
            Label6.AutoSize = true;
            Label6.Font = new Font("Segoe UI", 8.0f, FontStyle.Bold, GraphicsUnit.Point, 0);
            Label6.ForeColor = SystemColors.WindowText;
            Label6.Margin = new System.Windows.Forms.Padding(0, 3, 6, 3);
            Label6.Name = "Label6";
            Label6.Text = "Lines:";
            // 
            // lblLineCount
            // 
            _lblLineCount.AutoSize = true;
            _lblLineCount.ForeColor = SystemColors.WindowText;
            _lblLineCount.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            _lblLineCount.Name = "_lblLineCount";
            _lblLineCount.Text = "Line Count";
            // 
            // Link_Variables
            // 
            Link_Variables.AutoSize = true;
            Link_Variables.Font = new Font("Segoe UI", 8.0f, FontStyle.Bold, GraphicsUnit.Point, 0);
            Link_Variables.ForeColor = SystemColors.ActiveBorder;
            Link_Variables.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            Link_Variables.LinkColor = Color.Navy;
            Link_Variables.Margin = new System.Windows.Forms.Padding(0, 3, 6, 3);
            Link_Variables.Name = "Link_Variables";
            Link_Variables.TabStop = true;
            Link_Variables.Text = "Variables:";
            // 
            // lblVariables
            // 
            _lblVariables.AutoSize = true;
            _lblVariables.ForeColor = SystemColors.WindowText;
            _lblVariables.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            _lblVariables.Name = "_lblVariables";
            _lblVariables.Text = "Variables:";
            // 
            // Link_Files
            // 
            Link_Files.AutoSize = true;
            Link_Files.Font = new Font("Segoe UI", 8.0f, FontStyle.Bold, GraphicsUnit.Point, 0);
            Link_Files.ForeColor = SystemColors.ActiveBorder;
            Link_Files.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            Link_Files.LinkColor = Color.Navy;
            Link_Files.Margin = new System.Windows.Forms.Padding(0, 3, 6, 3);
            Link_Files.Name = "Link_Files";
            Link_Files.TabStop = true;
            Link_Files.Text = "Files:";
            // 
            // lblFiles
            // 
            _lblFiles.AutoSize = true;
            _lblFiles.ForeColor = SystemColors.WindowText;
            _lblFiles.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            _lblFiles.Name = "_lblFiles";
            _lblFiles.Text = "Files:";
            // 
            // Link_Events
            // 
            Link_Events.AutoSize = true;
            Link_Events.Font = new Font("Segoe UI", 8.0f, FontStyle.Bold, GraphicsUnit.Point, 0);
            Link_Events.ForeColor = SystemColors.ActiveBorder;
            Link_Events.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            Link_Events.LinkColor = Color.Navy;
            Link_Events.Margin = new System.Windows.Forms.Padding(0, 3, 6, 3);
            Link_Events.Name = "Link_Events";
            Link_Events.TabStop = true;
            Link_Events.Text = "Events:";
            // 
            // lblEvents
            // 
            _lblEvents.AutoSize = true;
            _lblEvents.ForeColor = SystemColors.WindowText;
            _lblEvents.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            _lblEvents.Name = "_lblEvents";
            _lblEvents.Text = "Events:";
            // 
            // Link_Warnings
            // 
            Link_Warnings.AutoSize = true;
            Link_Warnings.Font = new Font("Segoe UI", 8.0f, FontStyle.Bold, GraphicsUnit.Point, 0);
            Link_Warnings.ForeColor = SystemColors.ActiveBorder;
            Link_Warnings.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            Link_Warnings.LinkColor = Color.Navy;
            Link_Warnings.Margin = new System.Windows.Forms.Padding(0, 3, 6, 3);
            Link_Warnings.Name = "Link_Warnings";
            Link_Warnings.TabStop = true;
            Link_Warnings.Text = "Warnings:";
            // 
            // lblWarnings
            // 
            _lblWarnings.AutoSize = true;
            _lblWarnings.ForeColor = SystemColors.WindowText;
            _lblWarnings.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            _lblWarnings.Name = "_lblWarnings";
            _lblWarnings.Text = "Warnings:";
            // 
            // Link_Errors
            // 
            Link_Errors.AutoSize = true;
            Link_Errors.Font = new Font("Segoe UI", 8.0f, FontStyle.Bold, GraphicsUnit.Point, 0);
            Link_Errors.ForeColor = SystemColors.ActiveBorder;
            Link_Errors.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            Link_Errors.LinkColor = Color.Navy;
            Link_Errors.Margin = new System.Windows.Forms.Padding(0, 3, 6, 3);
            Link_Errors.Name = "Link_Errors";
            Link_Errors.TabStop = true;
            Link_Errors.Text = "Errors:";
            // 
            // lblErrors
            // 
            _lblErrors.AutoSize = true;
            _lblErrors.ForeColor = SystemColors.WindowText;
            _lblErrors.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            _lblErrors.Name = "_lblErrors";
            _lblErrors.Text = "Errors:";
            // 
            // ListBoxEcho
            // 
            ListBoxEcho.BorderStyle = System.Windows.Forms.BorderStyle.None;
            ListBoxEcho.Dock = System.Windows.Forms.DockStyle.Fill;
            ListBoxEcho.Font = new Font("Segoe UI", 8.0f);
            ListBoxEcho.FormattingEnabled = true;
            ListBoxEcho.IntegralHeight = false;
            ListBoxEcho.Margin = new System.Windows.Forms.Padding(0);
            ListBoxEcho.Name = "ListBoxEcho";
            ListBoxEcho.ScrollAlwaysVisible = true;
            ListBoxEcho.Size = new Size(555, 187);
            ListBoxEcho.TabIndex = 13;
            // 
            // Status
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = SystemColors.ControlLightLight;
            ClientSize = new Size(842, 225);
            Controls.Add(RootLayoutPanel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Status";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Text = "Status";
            GotFocus += new EventHandler(Status_GotFocus);
            LostFocus += new EventHandler(Status_LostFocus);
            StatusGridPanel.ResumeLayout(false);
            StatusGridPanel.PerformLayout();
            StatusBox.ResumeLayout(false);
            StatusBox.PerformLayout();
            RootLayoutPanel.ResumeLayout(false);
            RootLayoutPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }
        internal System.Windows.Forms.TableLayoutPanel RootLayoutPanel;
        internal System.Windows.Forms.TableLayoutPanel StatusGridPanel;
        internal System.Windows.Forms.GroupBox StatusBox;
        private System.Windows.Forms.Label _lblLineCount;

        public virtual System.Windows.Forms.Label lblLineCount
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblLineCount;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _lblLineCount = value;
            }
        }
        internal System.Windows.Forms.Label Label6;
        private System.Windows.Forms.Label _lblDepth;

        public virtual System.Windows.Forms.Label lblDepth
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblDepth;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _lblDepth = value;
            }
        }
        internal System.Windows.Forms.Label Label11;
        internal System.Windows.Forms.Label Label2;
        private System.Windows.Forms.Label _lblVariables;

        public virtual System.Windows.Forms.Label lblVariables
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblVariables;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _lblVariables = value;
            }
        }
        internal System.Windows.Forms.LinkLabel Link_Variables;
        private System.Windows.Forms.Label _lblFiles;

        public virtual System.Windows.Forms.Label lblFiles
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblFiles;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _lblFiles = value;
            }
        }
        internal System.Windows.Forms.LinkLabel Link_Files;
        private System.Windows.Forms.Label _lblEvents;

        public virtual System.Windows.Forms.Label lblEvents
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblEvents;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _lblEvents = value;
            }
        }
        internal System.Windows.Forms.LinkLabel Link_Events;
        private System.Windows.Forms.Label _lblCurrentFile;

        public virtual System.Windows.Forms.Label lblCurrentFile
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblCurrentFile;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _lblCurrentFile = value;
            }
        }
        internal System.Windows.Forms.LinkLabel Link_Errors;
        private System.Windows.Forms.Label _lblWarnings;

        public virtual System.Windows.Forms.Label lblWarnings
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblWarnings;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _lblWarnings = value;
            }
        }
        internal System.Windows.Forms.LinkLabel Link_Warnings;
        private System.Windows.Forms.Label _lblErrors;

        public virtual System.Windows.Forms.Label lblErrors
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblErrors;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _lblErrors = value;
            }
        }
        internal System.Windows.Forms.ListBox ListBoxEcho;
    }
}
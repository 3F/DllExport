namespace net.r_eg.DllExport.Wizard.UI.Controls
{
    partial class ProjectItemControl
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
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.linkOurILAsm = new System.Windows.Forms.LinkLabel();
            this.chkOurILAsm = new System.Windows.Forms.CheckBox();
            this.linkExpLib = new System.Windows.Forms.LinkLabel();
            this.linkOrdinals = new System.Windows.Forms.LinkLabel();
            this.chkGenExpLib = new System.Windows.Forms.CheckBox();
            this.numOrdinal = new System.Windows.Forms.NumericUpDown();
            this.labelOrdinals = new System.Windows.Forms.Label();
            this.rbPlatformAnyCPU = new System.Windows.Forms.RadioButton();
            this.rbPlatformX64 = new System.Windows.Forms.RadioButton();
            this.groupCompiler = new System.Windows.Forms.GroupBox();
            this.rbPlatformX86 = new System.Windows.Forms.RadioButton();
            this.groupPlatform = new System.Windows.Forms.GroupBox();
            this.rbCecil = new System.Windows.Forms.RadioButton();
            this.rbDirect = new System.Windows.Forms.RadioButton();
            this.panelNScombo = new System.Windows.Forms.Panel();
            this.comboNS = new System.Windows.Forms.ComboBox();
            this.linkDDNS = new System.Windows.Forms.LinkLabel();
            this.groupNS = new System.Windows.Forms.GroupBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbProject = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.textBoxProjectGuid = new System.Windows.Forms.TextBox();
            this.chkInstalled = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelStatus = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.numOrdinal)).BeginInit();
            this.groupCompiler.SuspendLayout();
            this.groupPlatform.SuspendLayout();
            this.panelNScombo.SuspendLayout();
            this.groupNS.SuspendLayout();
            this.gbProject.SuspendLayout();
            this.SuspendLayout();
            // 
            // linkOurILAsm
            // 
            this.linkOurILAsm.AutoSize = true;
            this.linkOurILAsm.Location = new System.Drawing.Point(292, 61);
            this.linkOurILAsm.Name = "linkOurILAsm";
            this.linkOurILAsm.Size = new System.Drawing.Size(13, 13);
            this.linkOurILAsm.TabIndex = 7;
            this.linkOurILAsm.TabStop = true;
            this.linkOurILAsm.Text = "?";
            this.linkOurILAsm.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkOurILAsm_LinkClicked);
            // 
            // chkOurILAsm
            // 
            this.chkOurILAsm.AutoSize = true;
            this.chkOurILAsm.Location = new System.Drawing.Point(10, 61);
            this.chkOurILAsm.Name = "chkOurILAsm";
            this.chkOurILAsm.Size = new System.Drawing.Size(279, 17);
            this.chkOurILAsm.TabIndex = 6;
            this.chkOurILAsm.Text = "Use our IL Assembler. Try to fix 0x13 / 0x11 opcodes.";
            this.chkOurILAsm.UseVisualStyleBackColor = true;
            // 
            // linkExpLib
            // 
            this.linkExpLib.AutoSize = true;
            this.linkExpLib.Location = new System.Drawing.Point(248, 40);
            this.linkExpLib.Name = "linkExpLib";
            this.linkExpLib.Size = new System.Drawing.Size(13, 13);
            this.linkExpLib.TabIndex = 5;
            this.linkExpLib.TabStop = true;
            this.linkExpLib.Text = "?";
            this.linkExpLib.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkExpLib_LinkClicked);
            // 
            // linkOrdinals
            // 
            this.linkOrdinals.AutoSize = true;
            this.linkOrdinals.Location = new System.Drawing.Point(214, 16);
            this.linkOrdinals.Name = "linkOrdinals";
            this.linkOrdinals.Size = new System.Drawing.Size(13, 13);
            this.linkOrdinals.TabIndex = 4;
            this.linkOrdinals.TabStop = true;
            this.linkOrdinals.Text = "?";
            this.linkOrdinals.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkOrdinals_LinkClicked);
            // 
            // chkGenExpLib
            // 
            this.chkGenExpLib.AutoSize = true;
            this.chkGenExpLib.Location = new System.Drawing.Point(10, 41);
            this.chkGenExpLib.Name = "chkGenExpLib";
            this.chkGenExpLib.Size = new System.Drawing.Size(236, 17);
            this.chkGenExpLib.TabIndex = 3;
            this.chkGenExpLib.Text = "Generate .exp + .lib via MS Library Manager.";
            this.chkGenExpLib.UseVisualStyleBackColor = true;
            // 
            // numOrdinal
            // 
            this.numOrdinal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numOrdinal.Location = new System.Drawing.Point(10, 16);
            this.numOrdinal.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numOrdinal.Name = "numOrdinal";
            this.numOrdinal.Size = new System.Drawing.Size(84, 20);
            this.numOrdinal.TabIndex = 2;
            this.numOrdinal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numOrdinal_KeyDown);
            // 
            // labelOrdinals
            // 
            this.labelOrdinals.AutoSize = true;
            this.labelOrdinals.Location = new System.Drawing.Point(100, 19);
            this.labelOrdinals.Name = "labelOrdinals";
            this.labelOrdinals.Size = new System.Drawing.Size(110, 13);
            this.labelOrdinals.TabIndex = 1;
            this.labelOrdinals.Text = "The Base for ordinals.";
            // 
            // rbPlatformAnyCPU
            // 
            this.rbPlatformAnyCPU.AutoSize = true;
            this.rbPlatformAnyCPU.Checked = true;
            this.rbPlatformAnyCPU.Location = new System.Drawing.Point(21, 61);
            this.rbPlatformAnyCPU.Name = "rbPlatformAnyCPU";
            this.rbPlatformAnyCPU.Size = new System.Drawing.Size(71, 17);
            this.rbPlatformAnyCPU.TabIndex = 2;
            this.rbPlatformAnyCPU.TabStop = true;
            this.rbPlatformAnyCPU.Text = "x86 + x64";
            this.rbPlatformAnyCPU.UseVisualStyleBackColor = true;
            // 
            // rbPlatformX64
            // 
            this.rbPlatformX64.AutoSize = true;
            this.rbPlatformX64.Location = new System.Drawing.Point(21, 40);
            this.rbPlatformX64.Name = "rbPlatformX64";
            this.rbPlatformX64.Size = new System.Drawing.Size(42, 17);
            this.rbPlatformX64.TabIndex = 1;
            this.rbPlatformX64.Text = "x64";
            this.rbPlatformX64.UseVisualStyleBackColor = true;
            // 
            // groupCompiler
            // 
            this.groupCompiler.Controls.Add(this.linkOurILAsm);
            this.groupCompiler.Controls.Add(this.chkOurILAsm);
            this.groupCompiler.Controls.Add(this.linkExpLib);
            this.groupCompiler.Controls.Add(this.linkOrdinals);
            this.groupCompiler.Controls.Add(this.chkGenExpLib);
            this.groupCompiler.Controls.Add(this.numOrdinal);
            this.groupCompiler.Controls.Add(this.labelOrdinals);
            this.groupCompiler.Location = new System.Drawing.Point(130, 118);
            this.groupCompiler.Name = "groupCompiler";
            this.groupCompiler.Size = new System.Drawing.Size(313, 84);
            this.groupCompiler.TabIndex = 9;
            this.groupCompiler.TabStop = false;
            this.groupCompiler.Text = "Compiler settings";
            // 
            // rbPlatformX86
            // 
            this.rbPlatformX86.AutoSize = true;
            this.rbPlatformX86.Location = new System.Drawing.Point(21, 19);
            this.rbPlatformX86.Name = "rbPlatformX86";
            this.rbPlatformX86.Size = new System.Drawing.Size(42, 17);
            this.rbPlatformX86.TabIndex = 0;
            this.rbPlatformX86.Text = "x86";
            this.rbPlatformX86.UseVisualStyleBackColor = true;
            // 
            // groupPlatform
            // 
            this.groupPlatform.Controls.Add(this.rbPlatformAnyCPU);
            this.groupPlatform.Controls.Add(this.rbPlatformX64);
            this.groupPlatform.Controls.Add(this.rbPlatformX86);
            this.groupPlatform.Location = new System.Drawing.Point(5, 118);
            this.groupPlatform.Name = "groupPlatform";
            this.groupPlatform.Size = new System.Drawing.Size(119, 84);
            this.groupPlatform.TabIndex = 8;
            this.groupPlatform.TabStop = false;
            this.groupPlatform.Text = "Export for platform:";
            // 
            // rbCecil
            // 
            this.rbCecil.AutoSize = true;
            this.rbCecil.Checked = true;
            this.rbCecil.Location = new System.Drawing.Point(384, 41);
            this.rbCecil.Name = "rbCecil";
            this.rbCecil.Size = new System.Drawing.Size(48, 17);
            this.rbCecil.TabIndex = 4;
            this.rbCecil.TabStop = true;
            this.rbCecil.Text = "Cecil";
            this.rbCecil.UseVisualStyleBackColor = true;
            // 
            // rbDirect
            // 
            this.rbDirect.AutoSize = true;
            this.rbDirect.Location = new System.Drawing.Point(297, 41);
            this.rbDirect.Name = "rbDirect";
            this.rbDirect.Size = new System.Drawing.Size(85, 17);
            this.rbDirect.TabIndex = 5;
            this.rbDirect.Text = "Direct-Mod /";
            this.rbDirect.UseVisualStyleBackColor = true;
            // 
            // panelNScombo
            // 
            this.panelNScombo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(158)))), ((int)(((byte)(207)))));
            this.panelNScombo.Controls.Add(this.comboNS);
            this.panelNScombo.Location = new System.Drawing.Point(11, 16);
            this.panelNScombo.Name = "panelNScombo";
            this.panelNScombo.Padding = new System.Windows.Forms.Padding(1);
            this.panelNScombo.Size = new System.Drawing.Size(421, 23);
            this.panelNScombo.TabIndex = 3;
            // 
            // comboNS
            // 
            this.comboNS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboNS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboNS.FormattingEnabled = true;
            this.comboNS.Location = new System.Drawing.Point(1, 1);
            this.comboNS.Name = "comboNS";
            this.comboNS.Size = new System.Drawing.Size(419, 21);
            this.comboNS.TabIndex = 1;
            this.toolTip.SetToolTip(this.comboNS, "It will override prev. if exists");
            this.comboNS.TextUpdate += new System.EventHandler(this.comboNS_TextUpdate);
            // 
            // linkDDNS
            // 
            this.linkDDNS.AutoSize = true;
            this.linkDDNS.Location = new System.Drawing.Point(8, 43);
            this.linkDDNS.Name = "linkDDNS";
            this.linkDDNS.Size = new System.Drawing.Size(100, 13);
            this.linkDDNS.TabIndex = 2;
            this.linkDDNS.TabStop = true;
            this.linkDDNS.Text = "? More about ddNS";
            this.linkDDNS.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDDNS_LinkClicked);
            // 
            // groupNS
            // 
            this.groupNS.Controls.Add(this.rbCecil);
            this.groupNS.Controls.Add(this.rbDirect);
            this.groupNS.Controls.Add(this.panelNScombo);
            this.groupNS.Controls.Add(this.linkDDNS);
            this.groupNS.Location = new System.Drawing.Point(5, 49);
            this.groupNS.Name = "groupNS";
            this.groupNS.Size = new System.Drawing.Size(438, 63);
            this.groupNS.TabIndex = 7;
            this.groupNS.TabStop = false;
            this.groupNS.Text = "Namespace for DllExport";
            // 
            // gbProject
            // 
            this.gbProject.Controls.Add(this.btnBrowse);
            this.gbProject.Controls.Add(this.textBoxProjectGuid);
            this.gbProject.Controls.Add(this.chkInstalled);
            this.gbProject.Location = new System.Drawing.Point(6, 0);
            this.gbProject.Name = "gbProject";
            this.gbProject.Size = new System.Drawing.Size(437, 43);
            this.gbProject.TabIndex = 11;
            this.gbProject.TabStop = false;
            this.gbProject.Text = "Project: ";
            // 
            // btnBrowse
            // 
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBrowse.Location = new System.Drawing.Point(367, 13);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(63, 23);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // textBoxProjectGuid
            // 
            this.textBoxProjectGuid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxProjectGuid.Location = new System.Drawing.Point(126, 20);
            this.textBoxProjectGuid.Name = "textBoxProjectGuid";
            this.textBoxProjectGuid.ReadOnly = true;
            this.textBoxProjectGuid.Size = new System.Drawing.Size(268, 13);
            this.textBoxProjectGuid.TabIndex = 2;
            this.textBoxProjectGuid.Text = "{00000000-0000-0000-0000-000000000000}";
            // 
            // chkInstalled
            // 
            this.chkInstalled.AutoSize = true;
            this.chkInstalled.Location = new System.Drawing.Point(42, 19);
            this.chkInstalled.Name = "chkInstalled";
            this.chkInstalled.Size = new System.Drawing.Size(80, 17);
            this.chkInstalled.TabIndex = 0;
            this.chkInstalled.Text = "Installed   - ";
            this.chkInstalled.UseVisualStyleBackColor = true;
            this.chkInstalled.CheckedChanged += new System.EventHandler(this.chkInstalled_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Location = new System.Drawing.Point(6, 204);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(435, 4);
            this.panel1.TabIndex = 12;
            // 
            // panelStatus
            // 
            this.panelStatus.BackColor = System.Drawing.Color.DarkRed;
            this.panelStatus.Location = new System.Drawing.Point(0, 7);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(4, 193);
            this.panelStatus.TabIndex = 0;
            // 
            // ProjectItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelStatus);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gbProject);
            this.Controls.Add(this.groupCompiler);
            this.Controls.Add(this.groupPlatform);
            this.Controls.Add(this.groupNS);
            this.Name = "ProjectItemControl";
            this.Size = new System.Drawing.Size(444, 210);
            ((System.ComponentModel.ISupportInitialize)(this.numOrdinal)).EndInit();
            this.groupCompiler.ResumeLayout(false);
            this.groupCompiler.PerformLayout();
            this.groupPlatform.ResumeLayout(false);
            this.groupPlatform.PerformLayout();
            this.panelNScombo.ResumeLayout(false);
            this.groupNS.ResumeLayout(false);
            this.groupNS.PerformLayout();
            this.gbProject.ResumeLayout(false);
            this.gbProject.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.LinkLabel linkOurILAsm;
        private System.Windows.Forms.CheckBox chkOurILAsm;
        private System.Windows.Forms.LinkLabel linkExpLib;
        private System.Windows.Forms.LinkLabel linkOrdinals;
        private System.Windows.Forms.CheckBox chkGenExpLib;
        private System.Windows.Forms.NumericUpDown numOrdinal;
        private System.Windows.Forms.Label labelOrdinals;
        private System.Windows.Forms.RadioButton rbPlatformAnyCPU;
        private System.Windows.Forms.RadioButton rbPlatformX64;
        private System.Windows.Forms.GroupBox groupCompiler;
        private System.Windows.Forms.RadioButton rbPlatformX86;
        private System.Windows.Forms.GroupBox groupPlatform;
        private System.Windows.Forms.RadioButton rbCecil;
        private System.Windows.Forms.RadioButton rbDirect;
        private System.Windows.Forms.Panel panelNScombo;
        private System.Windows.Forms.ComboBox comboNS;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.LinkLabel linkDDNS;
        private System.Windows.Forms.GroupBox groupNS;
        private System.Windows.Forms.GroupBox gbProject;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.CheckBox chkInstalled;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBoxProjectGuid;
        private System.Windows.Forms.Panel panelStatus;
    }
}

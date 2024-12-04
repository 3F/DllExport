using net.r_eg.DllExport.Wizard.UI.Components;

namespace net.r_eg.DllExport.Wizard.UI.Controls
{
    partial class ProjectItemControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.linkRefreshObj = new System.Windows.Forms.LinkLabel();
            this.chkRefreshObj = new System.Windows.Forms.CheckBox();
            this.chkNaNPatching = new System.Windows.Forms.CheckBox();
            this.linkInfPatching = new System.Windows.Forms.LinkLabel();
            this.linkSysObjRebase = new System.Windows.Forms.LinkLabel();
            this.chkRebaseSysObj = new System.Windows.Forms.CheckBox();
            this.chkIntermediateFiles = new System.Windows.Forms.CheckBox();
            this.chkCustomILAsm = new System.Windows.Forms.CheckBox();
            this.chkInfPatching = new System.Windows.Forms.CheckBox();
            this.rbPlatformX86 = new System.Windows.Forms.RadioButton();
            this.groupPlatform = new System.Windows.Forms.GroupBox();
            this.rbPlatformAuto = new System.Windows.Forms.RadioButton();
            this.labelPlatform = new System.Windows.Forms.Label();
            this.rbCecil = new System.Windows.Forms.RadioButton();
            this.rbDirect = new System.Windows.Forms.RadioButton();
            this.panelNScombo = new System.Windows.Forms.Panel();
            this.comboNS = new System.Windows.Forms.ComboBox();
            this.labelBackgroundNS = new System.Windows.Forms.Label();
            this.groupNS = new System.Windows.Forms.GroupBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.numTimeout = new System.Windows.Forms.NumericUpDown();
            this.chkPECheckIl = new System.Windows.Forms.CheckBox();
            this.chkPECheck1to1 = new System.Windows.Forms.CheckBox();
            this.chkPECheck32 = new System.Windows.Forms.CheckBox();
            this.gbProject = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.textBoxIdent = new System.Windows.Forms.TextBox();
            this.textBoxProjectPath = new System.Windows.Forms.TextBox();
            this.chkInstalled = new System.Windows.Forms.CheckBox();
            this.menuForInstalled = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemLimitPKT = new System.Windows.Forms.ToolStripMenuItem();
            this.panelStatus = new System.Windows.Forms.Panel();
            this.groupTimeout = new System.Windows.Forms.GroupBox();
            this.linkPeIl = new System.Windows.Forms.LinkLabel();
            this.labelPeCheck = new System.Windows.Forms.Label();
            this.linkPe1to1 = new System.Windows.Forms.LinkLabel();
            this.labelTimeout = new System.Windows.Forms.Label();
            this.textBoxCustomILAsm = new net.r_eg.DllExport.Wizard.UI.Components.TextBoxExt();
            ((System.ComponentModel.ISupportInitialize)(this.numOrdinal)).BeginInit();
            this.groupCompiler.SuspendLayout();
            this.groupPlatform.SuspendLayout();
            this.panelNScombo.SuspendLayout();
            this.groupNS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).BeginInit();
            this.gbProject.SuspendLayout();
            this.menuForInstalled.SuspendLayout();
            this.groupTimeout.SuspendLayout();
            this.SuspendLayout();
            // 
            // linkOurILAsm
            // 
            this.linkOurILAsm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkOurILAsm.AutoSize = true;
            this.linkOurILAsm.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkOurILAsm.Location = new System.Drawing.Point(291, 66);
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
            this.chkOurILAsm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkOurILAsm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkOurILAsm.Location = new System.Drawing.Point(10, 70);
            this.chkOurILAsm.Name = "chkOurILAsm";
            this.chkOurILAsm.Size = new System.Drawing.Size(274, 17);
            this.chkOurILAsm.TabIndex = 6;
            this.chkOurILAsm.Text = "Use 3F\'s IL Assembler. +Fix for 0x13 / 0x11 opcodes.";
            this.chkOurILAsm.UseVisualStyleBackColor = true;
            this.chkOurILAsm.CheckedChanged += new System.EventHandler(this.chkOurILAsm_CheckedChanged);
            // 
            // linkExpLib
            // 
            this.linkExpLib.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkExpLib.AutoSize = true;
            this.linkExpLib.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkExpLib.Location = new System.Drawing.Point(251, 50);
            this.linkExpLib.Name = "linkExpLib";
            this.linkExpLib.Size = new System.Drawing.Size(13, 13);
            this.linkExpLib.TabIndex = 5;
            this.linkExpLib.TabStop = true;
            this.linkExpLib.Text = "?";
            this.linkExpLib.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkExpLib_LinkClicked);
            // 
            // linkOrdinals
            // 
            this.linkOrdinals.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkOrdinals.AutoSize = true;
            this.linkOrdinals.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkOrdinals.Location = new System.Drawing.Point(211, 9);
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
            this.chkGenExpLib.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkGenExpLib.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkGenExpLib.Location = new System.Drawing.Point(10, 50);
            this.chkGenExpLib.Name = "chkGenExpLib";
            this.chkGenExpLib.Size = new System.Drawing.Size(233, 17);
            this.chkGenExpLib.TabIndex = 3;
            this.chkGenExpLib.Text = "Generate .exp + .lib via MS Library Manager.";
            this.chkGenExpLib.UseVisualStyleBackColor = true;
            // 
            // numOrdinal
            // 
            this.numOrdinal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numOrdinal.Location = new System.Drawing.Point(10, 8);
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
            this.labelOrdinals.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOrdinals.Location = new System.Drawing.Point(99, 11);
            this.labelOrdinals.Name = "labelOrdinals";
            this.labelOrdinals.Size = new System.Drawing.Size(110, 13);
            this.labelOrdinals.TabIndex = 1;
            this.labelOrdinals.Text = "The Base for ordinals.";
            // 
            // rbPlatformAnyCPU
            // 
            this.rbPlatformAnyCPU.AutoSize = true;
            this.rbPlatformAnyCPU.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbPlatformAnyCPU.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbPlatformAnyCPU.Location = new System.Drawing.Point(66, 29);
            this.rbPlatformAnyCPU.Name = "rbPlatformAnyCPU";
            this.rbPlatformAnyCPU.Size = new System.Drawing.Size(13, 12);
            this.rbPlatformAnyCPU.TabIndex = 2;
            this.toolTip.SetToolTip(this.rbPlatformAnyCPU, "(86+64) - Export for both platforms: x86 + x64.");
            this.rbPlatformAnyCPU.UseVisualStyleBackColor = true;
            this.rbPlatformAnyCPU.CheckedChanged += new System.EventHandler(this.rbPlatformAnyCPU_CheckedChanged);
            // 
            // rbPlatformX64
            // 
            this.rbPlatformX64.AutoSize = true;
            this.rbPlatformX64.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbPlatformX64.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbPlatformX64.Location = new System.Drawing.Point(34, 29);
            this.rbPlatformX64.Name = "rbPlatformX64";
            this.rbPlatformX64.Size = new System.Drawing.Size(13, 12);
            this.rbPlatformX64.TabIndex = 1;
            this.toolTip.SetToolTip(this.rbPlatformX64, "x64 - Export for platform: x64.");
            this.rbPlatformX64.UseVisualStyleBackColor = true;
            // 
            // groupCompiler
            // 
            this.groupCompiler.Controls.Add(this.linkExpLib);
            this.groupCompiler.Controls.Add(this.linkRefreshObj);
            this.groupCompiler.Controls.Add(this.chkRefreshObj);
            this.groupCompiler.Controls.Add(this.chkNaNPatching);
            this.groupCompiler.Controls.Add(this.linkInfPatching);
            this.groupCompiler.Controls.Add(this.linkSysObjRebase);
            this.groupCompiler.Controls.Add(this.chkRebaseSysObj);
            this.groupCompiler.Controls.Add(this.textBoxCustomILAsm);
            this.groupCompiler.Controls.Add(this.chkIntermediateFiles);
            this.groupCompiler.Controls.Add(this.chkCustomILAsm);
            this.groupCompiler.Controls.Add(this.linkOurILAsm);
            this.groupCompiler.Controls.Add(this.chkOurILAsm);
            this.groupCompiler.Controls.Add(this.linkOrdinals);
            this.groupCompiler.Controls.Add(this.chkGenExpLib);
            this.groupCompiler.Controls.Add(this.numOrdinal);
            this.groupCompiler.Controls.Add(this.labelOrdinals);
            this.groupCompiler.Controls.Add(this.chkInfPatching);
            this.groupCompiler.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupCompiler.Location = new System.Drawing.Point(130, 82);
            this.groupCompiler.Name = "groupCompiler";
            this.groupCompiler.Size = new System.Drawing.Size(313, 170);
            this.groupCompiler.TabIndex = 9;
            this.groupCompiler.TabStop = false;
            // 
            // linkRefreshObj
            // 
            this.linkRefreshObj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkRefreshObj.AutoSize = true;
            this.linkRefreshObj.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkRefreshObj.Location = new System.Drawing.Point(275, 28);
            this.linkRefreshObj.Name = "linkRefreshObj";
            this.linkRefreshObj.Size = new System.Drawing.Size(13, 13);
            this.linkRefreshObj.TabIndex = 18;
            this.linkRefreshObj.TabStop = true;
            this.linkRefreshObj.Text = "?";
            this.linkRefreshObj.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkRefreshObj_LinkClicked);
            // 
            // chkRefreshObj
            // 
            this.chkRefreshObj.AutoSize = true;
            this.chkRefreshObj.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkRefreshObj.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRefreshObj.Location = new System.Drawing.Point(10, 30);
            this.chkRefreshObj.Name = "chkRefreshObj";
            this.chkRefreshObj.Size = new System.Drawing.Size(253, 17);
            this.chkRefreshObj.TabIndex = 17;
            this.chkRefreshObj.Text = "Refresh intermediate module (obj) using modified.";
            this.chkRefreshObj.UseVisualStyleBackColor = true;
            // 
            // chkNaNPatching
            // 
            this.chkNaNPatching.AutoSize = true;
            this.chkNaNPatching.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkNaNPatching.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkNaNPatching.Location = new System.Drawing.Point(244, 150);
            this.chkNaNPatching.Name = "chkNaNPatching";
            this.chkNaNPatching.Size = new System.Drawing.Size(64, 17);
            this.chkNaNPatching.TabIndex = 16;
            this.chkNaNPatching.Text = "-nan(ind)";
            this.toolTip.SetToolTip(this.chkNaNPatching, "ldc.r8; ldc.r4; .field;\r\n\r\n-nan(ind) to \r\n00 00 C0 FF\r\n00 00 00 00 00 00 F8 FF");
            this.chkNaNPatching.UseVisualStyleBackColor = true;
            // 
            // linkInfPatching
            // 
            this.linkInfPatching.AutoSize = true;
            this.linkInfPatching.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkInfPatching.Location = new System.Drawing.Point(221, 151);
            this.linkInfPatching.Name = "linkInfPatching";
            this.linkInfPatching.Size = new System.Drawing.Size(13, 13);
            this.linkInfPatching.TabIndex = 15;
            this.linkInfPatching.TabStop = true;
            this.linkInfPatching.Text = "?";
            this.linkInfPatching.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkInfPatching_LinkClicked);
            // 
            // linkSysObjRebase
            // 
            this.linkSysObjRebase.AutoSize = true;
            this.linkSysObjRebase.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkSysObjRebase.Location = new System.Drawing.Point(287, 128);
            this.linkSysObjRebase.Name = "linkSysObjRebase";
            this.linkSysObjRebase.Size = new System.Drawing.Size(13, 13);
            this.linkSysObjRebase.TabIndex = 13;
            this.linkSysObjRebase.TabStop = true;
            this.linkSysObjRebase.Text = "?";
            this.linkSysObjRebase.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkSysObjRebase_LinkClicked);
            // 
            // chkRebaseSysObj
            // 
            this.chkRebaseSysObj.AutoSize = true;
            this.chkRebaseSysObj.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkRebaseSysObj.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRebaseSysObj.Location = new System.Drawing.Point(10, 130);
            this.chkRebaseSysObj.Name = "chkRebaseSysObj";
            this.chkRebaseSysObj.Size = new System.Drawing.Size(263, 17);
            this.chkRebaseSysObj.TabIndex = 12;
            this.chkRebaseSysObj.Text = "Rebase System Object: System.Runtime > mscorlib";
            this.chkRebaseSysObj.UseVisualStyleBackColor = true;
            // 
            // chkIntermediateFiles
            // 
            this.chkIntermediateFiles.AutoSize = true;
            this.chkIntermediateFiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkIntermediateFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIntermediateFiles.Location = new System.Drawing.Point(10, 110);
            this.chkIntermediateFiles.Name = "chkIntermediateFiles";
            this.chkIntermediateFiles.Size = new System.Drawing.Size(251, 17);
            this.chkIntermediateFiles.TabIndex = 11;
            this.chkIntermediateFiles.Text = "Keep Intermediate Files (IL Code, Resources, ...)";
            this.chkIntermediateFiles.UseVisualStyleBackColor = true;
            // 
            // chkCustomILAsm
            // 
            this.chkCustomILAsm.AutoSize = true;
            this.chkCustomILAsm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkCustomILAsm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCustomILAsm.Location = new System.Drawing.Point(10, 90);
            this.chkCustomILAsm.Name = "chkCustomILAsm";
            this.chkCustomILAsm.Size = new System.Drawing.Size(93, 17);
            this.chkCustomILAsm.TabIndex = 8;
            this.chkCustomILAsm.Text = "Custom ILAsm:";
            this.chkCustomILAsm.UseVisualStyleBackColor = true;
            this.chkCustomILAsm.CheckedChanged += new System.EventHandler(this.chkCustomILAsm_CheckedChanged);
            // 
            // chkInfPatching
            // 
            this.chkInfPatching.AutoSize = true;
            this.chkInfPatching.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkInfPatching.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkInfPatching.Location = new System.Drawing.Point(10, 150);
            this.chkInfPatching.Name = "chkInfPatching";
            this.chkInfPatching.Size = new System.Drawing.Size(207, 17);
            this.chkInfPatching.TabIndex = 14;
            this.chkInfPatching.Text = "Single + Double Inf/-Inf token patching";
            this.toolTip.SetToolTip(this.chkInfPatching, "ldc.r8; ldc.r4; .field; \r\n\r\ninf/-inf to \r\n0x7F800000/0xFF800000\r\n0x7FF00000000000" +
        "00/0xFFF0000000000000");
            this.chkInfPatching.UseVisualStyleBackColor = true;
            // 
            // rbPlatformX86
            // 
            this.rbPlatformX86.AutoSize = true;
            this.rbPlatformX86.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbPlatformX86.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbPlatformX86.Location = new System.Drawing.Point(7, 29);
            this.rbPlatformX86.Name = "rbPlatformX86";
            this.rbPlatformX86.Size = new System.Drawing.Size(13, 12);
            this.rbPlatformX86.TabIndex = 0;
            this.toolTip.SetToolTip(this.rbPlatformX86, "x86 - Export for platform: x86.");
            this.rbPlatformX86.UseVisualStyleBackColor = true;
            // 
            // groupPlatform
            // 
            this.groupPlatform.Controls.Add(this.rbPlatformAuto);
            this.groupPlatform.Controls.Add(this.rbPlatformAnyCPU);
            this.groupPlatform.Controls.Add(this.rbPlatformX64);
            this.groupPlatform.Controls.Add(this.rbPlatformX86);
            this.groupPlatform.Controls.Add(this.labelPlatform);
            this.groupPlatform.Location = new System.Drawing.Point(5, 82);
            this.groupPlatform.Name = "groupPlatform";
            this.groupPlatform.Size = new System.Drawing.Size(122, 49);
            this.groupPlatform.TabIndex = 8;
            this.groupPlatform.TabStop = false;
            // 
            // rbPlatformAuto
            // 
            this.rbPlatformAuto.AutoSize = true;
            this.rbPlatformAuto.Checked = true;
            this.rbPlatformAuto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbPlatformAuto.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbPlatformAuto.Location = new System.Drawing.Point(99, 29);
            this.rbPlatformAuto.Name = "rbPlatformAuto";
            this.rbPlatformAuto.Size = new System.Drawing.Size(13, 12);
            this.rbPlatformAuto.TabIndex = 6;
            this.rbPlatformAuto.TabStop = true;
            this.toolTip.SetToolTip(this.rbPlatformAuto, "auto - Automatic configuring platform from user settings.");
            this.rbPlatformAuto.UseVisualStyleBackColor = true;
            // 
            // labelPlatform
            // 
            this.labelPlatform.AutoSize = true;
            this.labelPlatform.BackColor = System.Drawing.Color.Transparent;
            this.labelPlatform.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlatform.Location = new System.Drawing.Point(1, 11);
            this.labelPlatform.Name = "labelPlatform";
            this.labelPlatform.Size = new System.Drawing.Size(116, 12);
            this.labelPlatform.TabIndex = 3;
            this.labelPlatform.Text = " x86      x64     (86+64)    auto";
            this.toolTip.SetToolTip(this.labelPlatform, "Platform for export:");
            // 
            // rbCecil
            // 
            this.rbCecil.AutoSize = true;
            this.rbCecil.Checked = true;
            this.rbCecil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbCecil.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbCecil.Location = new System.Drawing.Point(384, 7);
            this.rbCecil.Name = "rbCecil";
            this.rbCecil.Size = new System.Drawing.Size(47, 17);
            this.rbCecil.TabIndex = 4;
            this.rbCecil.TabStop = true;
            this.rbCecil.Text = "Cecil";
            this.rbCecil.UseVisualStyleBackColor = true;
            // 
            // rbDirect
            // 
            this.rbDirect.AutoSize = true;
            this.rbDirect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbDirect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDirect.Location = new System.Drawing.Point(297, 7);
            this.rbDirect.Name = "rbDirect";
            this.rbDirect.Size = new System.Drawing.Size(84, 17);
            this.rbDirect.TabIndex = 5;
            this.rbDirect.Text = "Direct-Mod /";
            this.rbDirect.UseVisualStyleBackColor = true;
            // 
            // panelNScombo
            // 
            this.panelNScombo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(158)))), ((int)(((byte)(207)))));
            this.panelNScombo.Controls.Add(this.comboNS);
            this.panelNScombo.Location = new System.Drawing.Point(5, 24);
            this.panelNScombo.Name = "panelNScombo";
            this.panelNScombo.Padding = new System.Windows.Forms.Padding(1);
            this.panelNScombo.Size = new System.Drawing.Size(429, 23);
            this.panelNScombo.TabIndex = 3;
            // 
            // comboNS
            // 
            this.comboNS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboNS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboNS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboNS.FormattingEnabled = true;
            this.comboNS.Location = new System.Drawing.Point(1, 1);
            this.comboNS.Name = "comboNS";
            this.comboNS.Size = new System.Drawing.Size(427, 21);
            this.comboNS.TabIndex = 1;
            this.comboNS.TextUpdate += new System.EventHandler(this.comboNS_TextUpdate);
            // 
            // labelBackgroundNS
            // 
            this.labelBackgroundNS.AutoSize = true;
            this.labelBackgroundNS.BackColor = System.Drawing.SystemColors.Control;
            this.labelBackgroundNS.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBackgroundNS.ForeColor = System.Drawing.Color.DimGray;
            this.labelBackgroundNS.Location = new System.Drawing.Point(5, 9);
            this.labelBackgroundNS.Name = "labelBackgroundNS";
            this.labelBackgroundNS.Size = new System.Drawing.Size(125, 13);
            this.labelBackgroundNS.TabIndex = 6;
            this.labelBackgroundNS.Text = "Namespace for DllExport:";
            // 
            // groupNS
            // 
            this.groupNS.Controls.Add(this.panelNScombo);
            this.groupNS.Controls.Add(this.labelBackgroundNS);
            this.groupNS.Controls.Add(this.rbCecil);
            this.groupNS.Controls.Add(this.rbDirect);
            this.groupNS.Location = new System.Drawing.Point(5, 30);
            this.groupNS.Name = "groupNS";
            this.groupNS.Size = new System.Drawing.Size(438, 53);
            this.groupNS.TabIndex = 7;
            this.groupNS.TabStop = false;
            // 
            // numTimeout
            // 
            this.numTimeout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numTimeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numTimeout.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numTimeout.Location = new System.Drawing.Point(14, 101);
            this.numTimeout.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numTimeout.Name = "numTimeout";
            this.numTimeout.Size = new System.Drawing.Size(76, 20);
            this.numTimeout.TabIndex = 10;
            this.numTimeout.ThousandsSeparator = true;
            this.toolTip.SetToolTip(this.numTimeout, "Timeout in Milliseconds");
            this.numTimeout.Value = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            // 
            // chkPECheckIl
            // 
            this.chkPECheckIl.AutoSize = true;
            this.chkPECheckIl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkPECheckIl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPECheckIl.Location = new System.Drawing.Point(14, 43);
            this.chkPECheckIl.Name = "chkPECheckIl";
            this.chkPECheckIl.Size = new System.Drawing.Size(59, 17);
            this.chkPECheckIl.TabIndex = 13;
            this.chkPECheckIl.Text = "IL code";
            this.toolTip.SetToolTip(this.chkPECheckIl, "Will check existence of all planned exports (IL code) in actual PE32/PE32+ module" +
        ".");
            this.chkPECheckIl.UseVisualStyleBackColor = true;
            // 
            // chkPECheck1to1
            // 
            this.chkPECheck1to1.AutoSize = true;
            this.chkPECheck1to1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkPECheck1to1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPECheck1to1.Location = new System.Drawing.Point(14, 23);
            this.chkPECheck1to1.Name = "chkPECheck1to1";
            this.chkPECheck1to1.Size = new System.Drawing.Size(38, 17);
            this.chkPECheck1to1.TabIndex = 12;
            this.chkPECheck1to1.Text = "1:1";
            this.toolTip.SetToolTip(this.chkPECheck1to1, "Expect the exact number of exported functions in final PE32/PE32+ module.");
            this.chkPECheck1to1.UseVisualStyleBackColor = true;
            // 
            // chkPECheck32
            // 
            this.chkPECheck32.AutoSize = true;
            this.chkPECheck32.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkPECheck32.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPECheck32.Location = new System.Drawing.Point(14, 63);
            this.chkPECheck32.Name = "chkPECheck32";
            this.chkPECheck32.Size = new System.Drawing.Size(92, 17);
            this.chkPECheck32.TabIndex = 17;
            this.chkPECheck32.Text = "PE32 / PE32+";
            this.toolTip.SetToolTip(this.chkPECheck32, "Will check module type for PE32 or PE32+ regarding selected architecture");
            this.chkPECheck32.UseVisualStyleBackColor = true;
            // 
            // gbProject
            // 
            this.gbProject.Controls.Add(this.btnBrowse);
            this.gbProject.Controls.Add(this.textBoxIdent);
            this.gbProject.Controls.Add(this.textBoxProjectPath);
            this.gbProject.Controls.Add(this.chkInstalled);
            this.gbProject.Location = new System.Drawing.Point(5, -4);
            this.gbProject.Name = "gbProject";
            this.gbProject.Size = new System.Drawing.Size(438, 40);
            this.gbProject.TabIndex = 11;
            this.gbProject.TabStop = false;
            // 
            // btnBrowse
            // 
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse.Location = new System.Drawing.Point(380, 11);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(54, 23);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // textBoxIdent
            // 
            this.textBoxIdent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxIdent.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxIdent.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxIdent.Location = new System.Drawing.Point(73, 7);
            this.textBoxIdent.Name = "textBoxIdent";
            this.textBoxIdent.ReadOnly = true;
            this.textBoxIdent.Size = new System.Drawing.Size(304, 11);
            this.textBoxIdent.TabIndex = 2;
            this.textBoxIdent.Text = "{00000000-0000-0000-0000-000000000000}";
            // 
            // textBoxProjectPath
            // 
            this.textBoxProjectPath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxProjectPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxProjectPath.Location = new System.Drawing.Point(73, 21);
            this.textBoxProjectPath.Name = "textBoxProjectPath";
            this.textBoxProjectPath.ReadOnly = true;
            this.textBoxProjectPath.Size = new System.Drawing.Size(304, 13);
            this.textBoxProjectPath.TabIndex = 4;
            this.textBoxProjectPath.Text = "<Project Path>";
            // 
            // chkInstalled
            // 
            this.chkInstalled.AutoSize = true;
            this.chkInstalled.ContextMenuStrip = this.menuForInstalled;
            this.chkInstalled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkInstalled.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkInstalled.Location = new System.Drawing.Point(5, 14);
            this.chkInstalled.Name = "chkInstalled";
            this.chkInstalled.Size = new System.Drawing.Size(65, 17);
            this.chkInstalled.TabIndex = 0;
            this.chkInstalled.Text = "Installed:";
            this.chkInstalled.UseVisualStyleBackColor = true;
            this.chkInstalled.CheckedChanged += new System.EventHandler(this.chkInstalled_CheckedChanged);
            // 
            // menuForInstalled
            // 
            this.menuForInstalled.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemLimitPKT});
            this.menuForInstalled.Name = "menuForInstalled";
            this.menuForInstalled.Size = new System.Drawing.Size(279, 26);
            // 
            // menuItemLimitPKT
            // 
            this.menuItemLimitPKT.Checked = true;
            this.menuItemLimitPKT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemLimitPKT.Name = "menuItemLimitPKT";
            this.menuItemLimitPKT.Size = new System.Drawing.Size(278, 22);
            this.menuItemLimitPKT.Text = "Limitations if not used PublicKeyToken";
            this.menuItemLimitPKT.Click += new System.EventHandler(this.menuItemLimitPKT_Click);
            // 
            // panelStatus
            // 
            this.panelStatus.BackColor = System.Drawing.Color.DarkRed;
            this.panelStatus.Location = new System.Drawing.Point(0, 2);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(4, 249);
            this.panelStatus.TabIndex = 0;
            // 
            // groupTimeout
            // 
            this.groupTimeout.Controls.Add(this.linkPeIl);
            this.groupTimeout.Controls.Add(this.labelPeCheck);
            this.groupTimeout.Controls.Add(this.chkPECheck32);
            this.groupTimeout.Controls.Add(this.linkPe1to1);
            this.groupTimeout.Controls.Add(this.chkPECheckIl);
            this.groupTimeout.Controls.Add(this.chkPECheck1to1);
            this.groupTimeout.Controls.Add(this.labelTimeout);
            this.groupTimeout.Controls.Add(this.numTimeout);
            this.groupTimeout.Location = new System.Drawing.Point(5, 126);
            this.groupTimeout.Name = "groupTimeout";
            this.groupTimeout.Size = new System.Drawing.Size(122, 126);
            this.groupTimeout.TabIndex = 9;
            this.groupTimeout.TabStop = false;
            // 
            // linkPeIl
            // 
            this.linkPeIl.AutoSize = true;
            this.linkPeIl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkPeIl.Location = new System.Drawing.Point(74, 43);
            this.linkPeIl.Name = "linkPeIl";
            this.linkPeIl.Size = new System.Drawing.Size(13, 13);
            this.linkPeIl.TabIndex = 19;
            this.linkPeIl.TabStop = true;
            this.linkPeIl.Text = "?";
            this.linkPeIl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkPeIl_LinkClicked);
            // 
            // labelPeCheck
            // 
            this.labelPeCheck.AutoSize = true;
            this.labelPeCheck.BackColor = System.Drawing.Color.Transparent;
            this.labelPeCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPeCheck.Location = new System.Drawing.Point(2, 7);
            this.labelPeCheck.Name = "labelPeCheck";
            this.labelPeCheck.Size = new System.Drawing.Size(58, 13);
            this.labelPeCheck.TabIndex = 18;
            this.labelPeCheck.Text = "PE Check:";
            // 
            // linkPe1to1
            // 
            this.linkPe1to1.AutoSize = true;
            this.linkPe1to1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkPe1to1.Location = new System.Drawing.Point(53, 25);
            this.linkPe1to1.Name = "linkPe1to1";
            this.linkPe1to1.Size = new System.Drawing.Size(13, 13);
            this.linkPe1to1.TabIndex = 16;
            this.linkPe1to1.TabStop = true;
            this.linkPe1to1.Text = "?";
            this.linkPe1to1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkPe1to1_LinkClicked);
            // 
            // labelTimeout
            // 
            this.labelTimeout.AutoSize = true;
            this.labelTimeout.BackColor = System.Drawing.Color.Transparent;
            this.labelTimeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTimeout.Location = new System.Drawing.Point(2, 83);
            this.labelTimeout.Name = "labelTimeout";
            this.labelTimeout.Size = new System.Drawing.Size(105, 13);
            this.labelTimeout.TabIndex = 11;
            this.labelTimeout.Text = "Processing Limit (ms)";
            // 
            // textBoxCustomILAsm
            // 
            this.textBoxCustomILAsm.BackgroundCaption = null;
            this.textBoxCustomILAsm.BackgroundCaptionAlpha = 60;
            this.textBoxCustomILAsm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxCustomILAsm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCustomILAsm.ForeColor = System.Drawing.Color.DarkGray;
            this.textBoxCustomILAsm.Location = new System.Drawing.Point(103, 88);
            this.textBoxCustomILAsm.Name = "textBoxCustomILAsm";
            this.textBoxCustomILAsm.Size = new System.Drawing.Size(204, 20);
            this.textBoxCustomILAsm.TabIndex = 9;
            // 
            // ProjectItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.groupPlatform);
            this.Controls.Add(this.groupCompiler);
            this.Controls.Add(this.groupTimeout);
            this.Controls.Add(this.panelStatus);
            this.Controls.Add(this.gbProject);
            this.Controls.Add(this.groupNS);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Name = "ProjectItemControl";
            this.Size = new System.Drawing.Size(444, 252);
            ((System.ComponentModel.ISupportInitialize)(this.numOrdinal)).EndInit();
            this.groupCompiler.ResumeLayout(false);
            this.groupCompiler.PerformLayout();
            this.groupPlatform.ResumeLayout(false);
            this.groupPlatform.PerformLayout();
            this.panelNScombo.ResumeLayout(false);
            this.groupNS.ResumeLayout(false);
            this.groupNS.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).EndInit();
            this.gbProject.ResumeLayout(false);
            this.gbProject.PerformLayout();
            this.menuForInstalled.ResumeLayout(false);
            this.groupTimeout.ResumeLayout(false);
            this.groupTimeout.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupNS;
        private System.Windows.Forms.GroupBox gbProject;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.CheckBox chkInstalled;
        private System.Windows.Forms.TextBox textBoxIdent;
        private System.Windows.Forms.Panel panelStatus;
        private System.Windows.Forms.CheckBox chkIntermediateFiles;
        private System.Windows.Forms.NumericUpDown numTimeout;
        private TextBoxExt textBoxCustomILAsm;
        private System.Windows.Forms.CheckBox chkCustomILAsm;
        private System.Windows.Forms.GroupBox groupTimeout;
        private System.Windows.Forms.Label labelBackgroundNS;
        private System.Windows.Forms.TextBox textBoxProjectPath;
        private System.Windows.Forms.Label labelTimeout;
        private System.Windows.Forms.Label labelPlatform;
        private System.Windows.Forms.CheckBox chkPECheckIl;
        private System.Windows.Forms.CheckBox chkPECheck1to1;
        private System.Windows.Forms.ContextMenuStrip menuForInstalled;
        private System.Windows.Forms.ToolStripMenuItem menuItemLimitPKT;
        private System.Windows.Forms.RadioButton rbPlatformAuto;
        private System.Windows.Forms.LinkLabel linkSysObjRebase;
        private System.Windows.Forms.CheckBox chkRebaseSysObj;
        private System.Windows.Forms.CheckBox chkInfPatching;
        private System.Windows.Forms.LinkLabel linkInfPatching;
        private System.Windows.Forms.LinkLabel linkPe1to1;
        private System.Windows.Forms.CheckBox chkNaNPatching;
        private System.Windows.Forms.CheckBox chkPECheck32;
        private System.Windows.Forms.CheckBox chkRefreshObj;
        private System.Windows.Forms.LinkLabel linkRefreshObj;
        private System.Windows.Forms.Label labelPeCheck;
        private System.Windows.Forms.LinkLabel linkPeIl;
    }
}

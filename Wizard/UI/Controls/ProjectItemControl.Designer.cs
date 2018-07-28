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
            this.textBoxCustomILAsm = new System.Windows.Forms.TextBox();
            this.chkIntermediateFiles = new System.Windows.Forms.CheckBox();
            this.chkCustomILAsm = new System.Windows.Forms.CheckBox();
            this.rbPlatformX86 = new System.Windows.Forms.RadioButton();
            this.groupPlatform = new System.Windows.Forms.GroupBox();
            this.labelX86X64 = new System.Windows.Forms.Label();
            this.labelX64 = new System.Windows.Forms.Label();
            this.labelX86 = new System.Windows.Forms.Label();
            this.rbCecil = new System.Windows.Forms.RadioButton();
            this.rbDirect = new System.Windows.Forms.RadioButton();
            this.panelNScombo = new System.Windows.Forms.Panel();
            this.comboNS = new System.Windows.Forms.ComboBox();
            this.labelBackgroundNS = new System.Windows.Forms.Label();
            this.linkDDNS = new System.Windows.Forms.LinkLabel();
            this.groupNS = new System.Windows.Forms.GroupBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.numTimeout = new System.Windows.Forms.NumericUpDown();
            this.chkPECheckIl = new System.Windows.Forms.CheckBox();
            this.chkPECheck1to1 = new System.Windows.Forms.CheckBox();
            this.gbProject = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.textBoxIdent = new System.Windows.Forms.TextBox();
            this.textBoxProjectPath = new System.Windows.Forms.TextBox();
            this.chkInstalled = new System.Windows.Forms.CheckBox();
            this.menuForInstalled = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemLimitPKT = new System.Windows.Forms.ToolStripMenuItem();
            this.panelBottomLine = new System.Windows.Forms.Panel();
            this.panelStatus = new System.Windows.Forms.Panel();
            this.groupTimeout = new System.Windows.Forms.GroupBox();
            this.labelTimeout = new System.Windows.Forms.Label();
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
            this.linkOurILAsm.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.linkOurILAsm.Location = new System.Drawing.Point(284, 57);
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
            this.chkOurILAsm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkOurILAsm.Location = new System.Drawing.Point(10, 61);
            this.chkOurILAsm.Name = "chkOurILAsm";
            this.chkOurILAsm.Size = new System.Drawing.Size(279, 17);
            this.chkOurILAsm.TabIndex = 6;
            this.chkOurILAsm.Text = "Use our IL Assembler. Try to fix 0x13 / 0x11 opcodes.";
            this.chkOurILAsm.UseVisualStyleBackColor = true;
            this.chkOurILAsm.CheckedChanged += new System.EventHandler(this.chkOurILAsm_CheckedChanged);
            // 
            // linkExpLib
            // 
            this.linkExpLib.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkExpLib.AutoSize = true;
            this.linkExpLib.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkExpLib.Location = new System.Drawing.Point(241, 38);
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
            this.linkOrdinals.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.linkOrdinals.Location = new System.Drawing.Point(206, 14);
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
            this.chkGenExpLib.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.labelOrdinals.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOrdinals.Location = new System.Drawing.Point(99, 19);
            this.labelOrdinals.Name = "labelOrdinals";
            this.labelOrdinals.Size = new System.Drawing.Size(110, 13);
            this.labelOrdinals.TabIndex = 1;
            this.labelOrdinals.Text = "The Base for ordinals.";
            // 
            // rbPlatformAnyCPU
            // 
            this.rbPlatformAnyCPU.AutoSize = true;
            this.rbPlatformAnyCPU.Checked = true;
            this.rbPlatformAnyCPU.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbPlatformAnyCPU.Location = new System.Drawing.Point(81, 25);
            this.rbPlatformAnyCPU.Name = "rbPlatformAnyCPU";
            this.rbPlatformAnyCPU.Size = new System.Drawing.Size(14, 13);
            this.rbPlatformAnyCPU.TabIndex = 2;
            this.rbPlatformAnyCPU.TabStop = true;
            this.toolTip.SetToolTip(this.rbPlatformAnyCPU, "Export for both Platforms: x86 + x64");
            this.rbPlatformAnyCPU.UseVisualStyleBackColor = true;
            // 
            // rbPlatformX64
            // 
            this.rbPlatformX64.AutoSize = true;
            this.rbPlatformX64.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbPlatformX64.Location = new System.Drawing.Point(44, 25);
            this.rbPlatformX64.Name = "rbPlatformX64";
            this.rbPlatformX64.Size = new System.Drawing.Size(14, 13);
            this.rbPlatformX64.TabIndex = 1;
            this.toolTip.SetToolTip(this.rbPlatformX64, "Export for Platform: x64");
            this.rbPlatformX64.UseVisualStyleBackColor = true;
            // 
            // groupCompiler
            // 
            this.groupCompiler.Controls.Add(this.textBoxCustomILAsm);
            this.groupCompiler.Controls.Add(this.chkIntermediateFiles);
            this.groupCompiler.Controls.Add(this.chkCustomILAsm);
            this.groupCompiler.Controls.Add(this.linkOurILAsm);
            this.groupCompiler.Controls.Add(this.chkOurILAsm);
            this.groupCompiler.Controls.Add(this.linkExpLib);
            this.groupCompiler.Controls.Add(this.linkOrdinals);
            this.groupCompiler.Controls.Add(this.chkGenExpLib);
            this.groupCompiler.Controls.Add(this.numOrdinal);
            this.groupCompiler.Controls.Add(this.labelOrdinals);
            this.groupCompiler.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupCompiler.Location = new System.Drawing.Point(130, 82);
            this.groupCompiler.Name = "groupCompiler";
            this.groupCompiler.Size = new System.Drawing.Size(313, 120);
            this.groupCompiler.TabIndex = 9;
            this.groupCompiler.TabStop = false;
            this.groupCompiler.Text = "Compiler settings";
            // 
            // textBoxCustomILAsm
            // 
            this.textBoxCustomILAsm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxCustomILAsm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.textBoxCustomILAsm.ForeColor = System.Drawing.Color.DarkGray;
            this.textBoxCustomILAsm.Location = new System.Drawing.Point(103, 79);
            this.textBoxCustomILAsm.Name = "textBoxCustomILAsm";
            this.textBoxCustomILAsm.Size = new System.Drawing.Size(204, 20);
            this.textBoxCustomILAsm.TabIndex = 9;
            // 
            // chkIntermediateFiles
            // 
            this.chkIntermediateFiles.AutoSize = true;
            this.chkIntermediateFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIntermediateFiles.Location = new System.Drawing.Point(10, 101);
            this.chkIntermediateFiles.Name = "chkIntermediateFiles";
            this.chkIntermediateFiles.Size = new System.Drawing.Size(254, 17);
            this.chkIntermediateFiles.TabIndex = 11;
            this.chkIntermediateFiles.Text = "Keep Intermediate Files (IL Code, Resources, ...)";
            this.chkIntermediateFiles.UseVisualStyleBackColor = true;
            // 
            // chkCustomILAsm
            // 
            this.chkCustomILAsm.AutoSize = true;
            this.chkCustomILAsm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCustomILAsm.Location = new System.Drawing.Point(10, 81);
            this.chkCustomILAsm.Name = "chkCustomILAsm";
            this.chkCustomILAsm.Size = new System.Drawing.Size(96, 17);
            this.chkCustomILAsm.TabIndex = 8;
            this.chkCustomILAsm.Text = "Custom ILAsm:";
            this.chkCustomILAsm.UseVisualStyleBackColor = true;
            this.chkCustomILAsm.CheckedChanged += new System.EventHandler(this.chkCustomILAsm_CheckedChanged);
            // 
            // rbPlatformX86
            // 
            this.rbPlatformX86.AutoSize = true;
            this.rbPlatformX86.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbPlatformX86.Location = new System.Drawing.Point(9, 25);
            this.rbPlatformX86.Name = "rbPlatformX86";
            this.rbPlatformX86.Size = new System.Drawing.Size(14, 13);
            this.rbPlatformX86.TabIndex = 0;
            this.toolTip.SetToolTip(this.rbPlatformX86, "Export for Platform: x86");
            this.rbPlatformX86.UseVisualStyleBackColor = true;
            // 
            // groupPlatform
            // 
            this.groupPlatform.Controls.Add(this.rbPlatformAnyCPU);
            this.groupPlatform.Controls.Add(this.rbPlatformX64);
            this.groupPlatform.Controls.Add(this.rbPlatformX86);
            this.groupPlatform.Controls.Add(this.labelX86X64);
            this.groupPlatform.Controls.Add(this.labelX64);
            this.groupPlatform.Controls.Add(this.labelX86);
            this.groupPlatform.Location = new System.Drawing.Point(5, 82);
            this.groupPlatform.Name = "groupPlatform";
            this.groupPlatform.Size = new System.Drawing.Size(119, 43);
            this.groupPlatform.TabIndex = 8;
            this.groupPlatform.TabStop = false;
            // 
            // labelX86X64
            // 
            this.labelX86X64.AutoSize = true;
            this.labelX86X64.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX86X64.Location = new System.Drawing.Point(67, 9);
            this.labelX86X64.Name = "labelX86X64";
            this.labelX86X64.Size = new System.Drawing.Size(47, 13);
            this.labelX86X64.TabIndex = 5;
            this.labelX86X64.Text = "x86+x64";
            this.toolTip.SetToolTip(this.labelX86X64, "Export for both Platforms: x86 + x64");
            // 
            // labelX64
            // 
            this.labelX64.AutoSize = true;
            this.labelX64.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX64.Location = new System.Drawing.Point(37, 9);
            this.labelX64.Name = "labelX64";
            this.labelX64.Size = new System.Drawing.Size(24, 13);
            this.labelX64.TabIndex = 4;
            this.labelX64.Text = "x64";
            this.toolTip.SetToolTip(this.labelX64, "Export for Platform: x64");
            // 
            // labelX86
            // 
            this.labelX86.AutoSize = true;
            this.labelX86.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX86.Location = new System.Drawing.Point(3, 9);
            this.labelX86.Name = "labelX86";
            this.labelX86.Size = new System.Drawing.Size(24, 13);
            this.labelX86.TabIndex = 3;
            this.labelX86.Text = "x86";
            this.toolTip.SetToolTip(this.labelX86, "Export for Platform: x86");
            // 
            // rbCecil
            // 
            this.rbCecil.AutoSize = true;
            this.rbCecil.Checked = true;
            this.rbCecil.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbCecil.Location = new System.Drawing.Point(384, 7);
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
            this.rbDirect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDirect.Location = new System.Drawing.Point(297, 7);
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
            // linkDDNS
            // 
            this.linkDDNS.AutoSize = true;
            this.linkDDNS.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkDDNS.Location = new System.Drawing.Point(150, 9);
            this.linkDDNS.Name = "linkDDNS";
            this.linkDDNS.Size = new System.Drawing.Size(99, 13);
            this.linkDDNS.TabIndex = 2;
            this.linkDDNS.TabStop = true;
            this.linkDDNS.Text = "? More about ddNS";
            this.linkDDNS.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDDNS_LinkClicked);
            // 
            // groupNS
            // 
            this.groupNS.Controls.Add(this.panelNScombo);
            this.groupNS.Controls.Add(this.linkDDNS);
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
            this.numTimeout.Location = new System.Drawing.Point(5, 13);
            this.numTimeout.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numTimeout.Name = "numTimeout";
            this.numTimeout.Size = new System.Drawing.Size(64, 20);
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
            this.chkPECheckIl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPECheckIl.Location = new System.Drawing.Point(5, 61);
            this.chkPECheckIl.Name = "chkPECheckIl";
            this.chkPECheckIl.Size = new System.Drawing.Size(113, 17);
            this.chkPECheckIl.TabIndex = 13;
            this.chkPECheckIl.Text = "PE Check IL code";
            this.toolTip.SetToolTip(this.chkPECheckIl, "Will check existence of all planned exports (IL code) in actual PE32/PE32+ module" +
        ".");
            this.chkPECheckIl.UseVisualStyleBackColor = true;
            // 
            // chkPECheck1to1
            // 
            this.chkPECheck1to1.AutoSize = true;
            this.chkPECheck1to1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPECheck1to1.Location = new System.Drawing.Point(5, 40);
            this.chkPECheck1to1.Name = "chkPECheck1to1";
            this.chkPECheck1to1.Size = new System.Drawing.Size(92, 17);
            this.chkPECheck1to1.TabIndex = 12;
            this.chkPECheck1to1.Text = "PE Check 1:1";
            this.toolTip.SetToolTip(this.chkPECheck1to1, "Will check count of all planned exports from final PE32/PE32+ module.");
            this.chkPECheck1to1.UseVisualStyleBackColor = true;
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
            this.textBoxProjectPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
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
            this.chkInstalled.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkInstalled.Location = new System.Drawing.Point(5, 14);
            this.chkInstalled.Name = "chkInstalled";
            this.chkInstalled.Size = new System.Drawing.Size(68, 17);
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
            this.menuForInstalled.Size = new System.Drawing.Size(280, 48);
            // 
            // menuItemLimitPKT
            // 
            this.menuItemLimitPKT.Checked = true;
            this.menuItemLimitPKT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemLimitPKT.Name = "menuItemLimitPKT";
            this.menuItemLimitPKT.Size = new System.Drawing.Size(279, 22);
            this.menuItemLimitPKT.Text = "Limitations if not used PublicKeyToken";
            this.menuItemLimitPKT.Click += new System.EventHandler(this.menuItemLimitPKT_Click);
            // 
            // panelBottomLine
            // 
            this.panelBottomLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panelBottomLine.BackColor = System.Drawing.Color.Gray;
            this.panelBottomLine.Location = new System.Drawing.Point(6, 202);
            this.panelBottomLine.Margin = new System.Windows.Forms.Padding(0);
            this.panelBottomLine.Name = "panelBottomLine";
            this.panelBottomLine.Size = new System.Drawing.Size(438, 2);
            this.panelBottomLine.TabIndex = 12;
            // 
            // panelStatus
            // 
            this.panelStatus.BackColor = System.Drawing.Color.DarkRed;
            this.panelStatus.Location = new System.Drawing.Point(0, 2);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(4, 200);
            this.panelStatus.TabIndex = 0;
            // 
            // groupTimeout
            // 
            this.groupTimeout.Controls.Add(this.chkPECheckIl);
            this.groupTimeout.Controls.Add(this.chkPECheck1to1);
            this.groupTimeout.Controls.Add(this.labelTimeout);
            this.groupTimeout.Controls.Add(this.numTimeout);
            this.groupTimeout.Location = new System.Drawing.Point(5, 120);
            this.groupTimeout.Name = "groupTimeout";
            this.groupTimeout.Size = new System.Drawing.Size(119, 82);
            this.groupTimeout.TabIndex = 9;
            this.groupTimeout.TabStop = false;
            // 
            // labelTimeout
            // 
            this.labelTimeout.AutoSize = true;
            this.labelTimeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTimeout.Location = new System.Drawing.Point(71, 16);
            this.labelTimeout.Name = "labelTimeout";
            this.labelTimeout.Size = new System.Drawing.Size(45, 13);
            this.labelTimeout.TabIndex = 11;
            this.labelTimeout.Text = "Timeout";
            // 
            // ProjectItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelBottomLine);
            this.Controls.Add(this.groupPlatform);
            this.Controls.Add(this.groupCompiler);
            this.Controls.Add(this.groupTimeout);
            this.Controls.Add(this.panelStatus);
            this.Controls.Add(this.gbProject);
            this.Controls.Add(this.groupNS);
            this.Name = "ProjectItemControl";
            this.Size = new System.Drawing.Size(444, 204);
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
        private System.Windows.Forms.LinkLabel linkDDNS;
        private System.Windows.Forms.GroupBox groupNS;
        private System.Windows.Forms.GroupBox gbProject;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.CheckBox chkInstalled;
        private System.Windows.Forms.Panel panelBottomLine;
        private System.Windows.Forms.TextBox textBoxIdent;
        private System.Windows.Forms.Panel panelStatus;
        private System.Windows.Forms.CheckBox chkIntermediateFiles;
        private System.Windows.Forms.NumericUpDown numTimeout;
        private System.Windows.Forms.TextBox textBoxCustomILAsm;
        private System.Windows.Forms.CheckBox chkCustomILAsm;
        private System.Windows.Forms.GroupBox groupTimeout;
        private System.Windows.Forms.Label labelBackgroundNS;
        private System.Windows.Forms.TextBox textBoxProjectPath;
        private System.Windows.Forms.Label labelTimeout;
        private System.Windows.Forms.Label labelX86X64;
        private System.Windows.Forms.Label labelX64;
        private System.Windows.Forms.Label labelX86;
        private System.Windows.Forms.CheckBox chkPECheckIl;
        private System.Windows.Forms.CheckBox chkPECheck1to1;
        private System.Windows.Forms.ContextMenuStrip menuForInstalled;
        private System.Windows.Forms.ToolStripMenuItem menuItemLimitPKT;
    }
}

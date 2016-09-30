namespace net.r_eg.DllExport.Configurator
{
    partial class InstallationForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnConfigure = new System.Windows.Forms.Button();
            this.comboNS = new System.Windows.Forms.ComboBox();
            this.groupNS = new System.Windows.Forms.GroupBox();
            this.panelNScombo = new System.Windows.Forms.Panel();
            this.linkDDNS = new System.Windows.Forms.LinkLabel();
            this.groupPlatform = new System.Windows.Forms.GroupBox();
            this.rbPlatformX64 = new System.Windows.Forms.RadioButton();
            this.rbPlatformX86 = new System.Windows.Forms.RadioButton();
            this.groupCompiler = new System.Windows.Forms.GroupBox();
            this.numOrdinal = new System.Windows.Forms.NumericUpDown();
            this.labelOrdinals = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupNS.SuspendLayout();
            this.panelNScombo.SuspendLayout();
            this.groupPlatform.SuspendLayout();
            this.groupCompiler.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOrdinal)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConfigure
            // 
            this.btnConfigure.Location = new System.Drawing.Point(366, 162);
            this.btnConfigure.Name = "btnConfigure";
            this.btnConfigure.Size = new System.Drawing.Size(75, 23);
            this.btnConfigure.TabIndex = 0;
            this.btnConfigure.Text = "Configure";
            this.btnConfigure.UseVisualStyleBackColor = true;
            this.btnConfigure.Click += new System.EventHandler(this.btnConfigure_Click);
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
            // groupNS
            // 
            this.groupNS.Controls.Add(this.panelNScombo);
            this.groupNS.Controls.Add(this.linkDDNS);
            this.groupNS.Location = new System.Drawing.Point(3, 3);
            this.groupNS.Name = "groupNS";
            this.groupNS.Size = new System.Drawing.Size(438, 63);
            this.groupNS.TabIndex = 2;
            this.groupNS.TabStop = false;
            this.groupNS.Text = "Namespace for DllExport";
            // 
            // panelNScombo
            // 
            this.panelNScombo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(158)))), ((int)(((byte)(207)))));
            this.panelNScombo.Controls.Add(this.comboNS);
            this.panelNScombo.Location = new System.Drawing.Point(9, 16);
            this.panelNScombo.Name = "panelNScombo";
            this.panelNScombo.Padding = new System.Windows.Forms.Padding(1);
            this.panelNScombo.Size = new System.Drawing.Size(421, 23);
            this.panelNScombo.TabIndex = 3;
            // 
            // linkDDNS
            // 
            this.linkDDNS.AutoSize = true;
            this.linkDDNS.Location = new System.Drawing.Point(6, 43);
            this.linkDDNS.Name = "linkDDNS";
            this.linkDDNS.Size = new System.Drawing.Size(100, 13);
            this.linkDDNS.TabIndex = 2;
            this.linkDDNS.TabStop = true;
            this.linkDDNS.Text = "? More about ddNS";
            this.linkDDNS.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDDNS_LinkClicked);
            // 
            // groupPlatform
            // 
            this.groupPlatform.Controls.Add(this.rbPlatformX64);
            this.groupPlatform.Controls.Add(this.rbPlatformX86);
            this.groupPlatform.Enabled = false;
            this.groupPlatform.Location = new System.Drawing.Point(3, 72);
            this.groupPlatform.Name = "groupPlatform";
            this.groupPlatform.Size = new System.Drawing.Size(95, 84);
            this.groupPlatform.TabIndex = 3;
            this.groupPlatform.TabStop = false;
            this.groupPlatform.Text = "Platform target";
            // 
            // rbPlatformX64
            // 
            this.rbPlatformX64.AutoSize = true;
            this.rbPlatformX64.Location = new System.Drawing.Point(22, 51);
            this.rbPlatformX64.Name = "rbPlatformX64";
            this.rbPlatformX64.Size = new System.Drawing.Size(42, 17);
            this.rbPlatformX64.TabIndex = 1;
            this.rbPlatformX64.Text = "x64";
            this.rbPlatformX64.UseVisualStyleBackColor = true;
            // 
            // rbPlatformX86
            // 
            this.rbPlatformX86.AutoSize = true;
            this.rbPlatformX86.Checked = true;
            this.rbPlatformX86.Location = new System.Drawing.Point(22, 25);
            this.rbPlatformX86.Name = "rbPlatformX86";
            this.rbPlatformX86.Size = new System.Drawing.Size(42, 17);
            this.rbPlatformX86.TabIndex = 0;
            this.rbPlatformX86.TabStop = true;
            this.rbPlatformX86.Text = "x86";
            this.rbPlatformX86.UseVisualStyleBackColor = true;
            // 
            // groupCompiler
            // 
            this.groupCompiler.Controls.Add(this.numOrdinal);
            this.groupCompiler.Controls.Add(this.labelOrdinals);
            this.groupCompiler.Enabled = false;
            this.groupCompiler.Location = new System.Drawing.Point(104, 72);
            this.groupCompiler.Name = "groupCompiler";
            this.groupCompiler.Size = new System.Drawing.Size(337, 84);
            this.groupCompiler.TabIndex = 4;
            this.groupCompiler.TabStop = false;
            this.groupCompiler.Text = "Compiler settings";
            // 
            // numOrdinal
            // 
            this.numOrdinal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numOrdinal.Location = new System.Drawing.Point(6, 22);
            this.numOrdinal.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numOrdinal.Name = "numOrdinal";
            this.numOrdinal.Size = new System.Drawing.Size(76, 20);
            this.numOrdinal.TabIndex = 2;
            this.numOrdinal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numOrdinal_KeyDown);
            // 
            // labelOrdinals
            // 
            this.labelOrdinals.AutoSize = true;
            this.labelOrdinals.Location = new System.Drawing.Point(88, 25);
            this.labelOrdinals.Name = "labelOrdinals";
            this.labelOrdinals.Size = new System.Drawing.Size(161, 13);
            this.labelOrdinals.TabIndex = 1;
            this.labelOrdinals.Text = "Base for ordinals. `.export [{0}+] `";
            // 
            // InstallationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 190);
            this.Controls.Add(this.groupCompiler);
            this.Controls.Add(this.groupPlatform);
            this.Controls.Add(this.groupNS);
            this.Controls.Add(this.btnConfigure);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InstallationForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DllExport settings";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.InstallationForm_FormClosed);
            this.groupNS.ResumeLayout(false);
            this.groupNS.PerformLayout();
            this.panelNScombo.ResumeLayout(false);
            this.groupPlatform.ResumeLayout(false);
            this.groupPlatform.PerformLayout();
            this.groupCompiler.ResumeLayout(false);
            this.groupCompiler.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOrdinal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConfigure;
        private System.Windows.Forms.ComboBox comboNS;
        private System.Windows.Forms.GroupBox groupNS;
        private System.Windows.Forms.LinkLabel linkDDNS;
        private System.Windows.Forms.GroupBox groupPlatform;
        private System.Windows.Forms.GroupBox groupCompiler;
        private System.Windows.Forms.RadioButton rbPlatformX64;
        private System.Windows.Forms.RadioButton rbPlatformX86;
        private System.Windows.Forms.Label labelOrdinals;
        private System.Windows.Forms.NumericUpDown numOrdinal;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel panelNScombo;
    }
}
﻿namespace net.r_eg.DllExport.Wizard.UI.Controls
{
    partial class PostProcControl
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
            if(disposing && (components != null))
            {
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelMain = new System.Windows.Forms.Panel();
            this.linkAboutDependent = new System.Windows.Forms.LinkLabel();
            this.radioDependentProjects = new System.Windows.Forms.RadioButton();
            this.linkAboutPostProc = new System.Windows.Forms.LinkLabel();
            this.chkIntermediateFiles = new System.Windows.Forms.CheckBox();
            this.radioCustom = new System.Windows.Forms.RadioButton();
            this.radioPostProcDisabled = new System.Windows.Forms.RadioButton();
            this.chkX86X64 = new System.Windows.Forms.CheckBox();
            this.txtPostProc = new System.Windows.Forms.TextBox();
            this.listActivatedProperties = new System.Windows.Forms.ListBox();
            this.dgvProperties = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.linkAboutVsSBE = new System.Windows.Forms.LinkLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProperties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.linkAboutDependent);
            this.panelMain.Controls.Add(this.radioDependentProjects);
            this.panelMain.Controls.Add(this.linkAboutPostProc);
            this.panelMain.Controls.Add(this.chkIntermediateFiles);
            this.panelMain.Controls.Add(this.radioCustom);
            this.panelMain.Controls.Add(this.radioPostProcDisabled);
            this.panelMain.Controls.Add(this.chkX86X64);
            this.panelMain.Controls.Add(this.txtPostProc);
            this.panelMain.Controls.Add(this.listActivatedProperties);
            this.panelMain.Controls.Add(this.dgvProperties);
            this.panelMain.Controls.Add(this.linkAboutVsSBE);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(442, 230);
            this.panelMain.TabIndex = 32;
            // 
            // linkAboutDependent
            // 
            this.linkAboutDependent.AutoSize = true;
            this.linkAboutDependent.Location = new System.Drawing.Point(3, 26);
            this.linkAboutDependent.Name = "linkAboutDependent";
            this.linkAboutDependent.Size = new System.Drawing.Size(13, 13);
            this.linkAboutDependent.TabIndex = 42;
            this.linkAboutDependent.TabStop = true;
            this.linkAboutDependent.Text = "?";
            this.linkAboutDependent.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkAboutDependent_LinkClicked);
            // 
            // radioDependentProjects
            // 
            this.radioDependentProjects.AutoSize = true;
            this.radioDependentProjects.Checked = true;
            this.radioDependentProjects.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioDependentProjects.Location = new System.Drawing.Point(3, 3);
            this.radioDependentProjects.Name = "radioDependentProjects";
            this.radioDependentProjects.Size = new System.Drawing.Size(179, 17);
            this.radioDependentProjects.TabIndex = 41;
            this.radioDependentProjects.TabStop = true;
            this.radioDependentProjects.Text = "Post-Proc for dependent projects";
            this.radioDependentProjects.UseVisualStyleBackColor = true;
            this.radioDependentProjects.CheckedChanged += new System.EventHandler(this.radioDependentProjects_CheckedChanged);
            // 
            // linkAboutPostProc
            // 
            this.linkAboutPostProc.AutoSize = true;
            this.linkAboutPostProc.Location = new System.Drawing.Point(306, 51);
            this.linkAboutPostProc.Name = "linkAboutPostProc";
            this.linkAboutPostProc.Size = new System.Drawing.Size(13, 13);
            this.linkAboutPostProc.TabIndex = 40;
            this.linkAboutPostProc.TabStop = true;
            this.linkAboutPostProc.Text = "?";
            this.linkAboutPostProc.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkAboutPostProc_LinkClicked);
            // 
            // chkIntermediateFiles
            // 
            this.chkIntermediateFiles.AutoSize = true;
            this.chkIntermediateFiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkIntermediateFiles.Location = new System.Drawing.Point(19, 49);
            this.chkIntermediateFiles.Name = "chkIntermediateFiles";
            this.chkIntermediateFiles.Size = new System.Drawing.Size(140, 17);
            this.chkIntermediateFiles.TabIndex = 39;
            this.chkIntermediateFiles.Text = "Provide intermediate files";
            this.chkIntermediateFiles.UseVisualStyleBackColor = true;
            // 
            // radioCustom
            // 
            this.radioCustom.AutoSize = true;
            this.radioCustom.Enabled = false;
            this.radioCustom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioCustom.Location = new System.Drawing.Point(325, 49);
            this.radioCustom.Name = "radioCustom";
            this.radioCustom.Size = new System.Drawing.Size(111, 17);
            this.radioCustom.TabIndex = 38;
            this.radioCustom.Text = "Custom Post-Proc:";
            this.toolTip.SetToolTip(this.radioCustom, "Not implemented for this version");
            this.radioCustom.UseVisualStyleBackColor = true;
            // 
            // radioPostProcDisabled
            // 
            this.radioPostProcDisabled.AutoSize = true;
            this.radioPostProcDisabled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioPostProcDisabled.Location = new System.Drawing.Point(269, 3);
            this.radioPostProcDisabled.Name = "radioPostProcDisabled";
            this.radioPostProcDisabled.Size = new System.Drawing.Size(167, 17);
            this.radioPostProcDisabled.TabIndex = 37;
            this.radioPostProcDisabled.Text = "Disabled /Runtime alternative:";
            this.radioPostProcDisabled.UseVisualStyleBackColor = true;
            // 
            // chkX86X64
            // 
            this.chkX86X64.AutoSize = true;
            this.chkX86X64.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkX86X64.Location = new System.Drawing.Point(19, 26);
            this.chkX86X64.Name = "chkX86X64";
            this.chkX86X64.Size = new System.Drawing.Size(156, 17);
            this.chkX86X64.TabIndex = 36;
            this.chkX86X64.Text = "Provide x86+x64 assemblies";
            this.chkX86X64.UseVisualStyleBackColor = true;
            // 
            // txtPostProc
            // 
            this.txtPostProc.BackColor = System.Drawing.SystemColors.Control;
            this.txtPostProc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPostProc.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPostProc.Location = new System.Drawing.Point(0, 162);
            this.txtPostProc.Multiline = true;
            this.txtPostProc.Name = "txtPostProc";
            this.txtPostProc.ReadOnly = true;
            this.txtPostProc.Size = new System.Drawing.Size(439, 65);
            this.txtPostProc.TabIndex = 35;
            // 
            // listActivatedProperties
            // 
            this.listActivatedProperties.FormattingEnabled = true;
            this.listActivatedProperties.IntegralHeight = false;
            this.listActivatedProperties.Location = new System.Drawing.Point(0, 72);
            this.listActivatedProperties.Name = "listActivatedProperties";
            this.listActivatedProperties.Size = new System.Drawing.Size(133, 84);
            this.listActivatedProperties.TabIndex = 34;
            // 
            // dgvProperties
            // 
            this.dgvProperties.AllowUserToAddRows = false;
            this.dgvProperties.AllowUserToDeleteRows = false;
            this.dgvProperties.AllowUserToOrderColumns = true;
            this.dgvProperties.AllowUserToResizeRows = false;
            this.dgvProperties.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvProperties.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProperties.ColumnHeadersVisible = false;
            this.dgvProperties.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colValue});
            this.dgvProperties.Location = new System.Drawing.Point(135, 72);
            this.dgvProperties.MultiSelect = false;
            this.dgvProperties.Name = "dgvProperties";
            this.dgvProperties.ReadOnly = true;
            this.dgvProperties.RowHeadersVisible = false;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(36)))), ((int)(((byte)(47)))));
            this.dgvProperties.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvProperties.RowTemplate.Height = 17;
            this.dgvProperties.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProperties.Size = new System.Drawing.Size(304, 84);
            this.dgvProperties.StandardTab = true;
            this.dgvProperties.TabIndex = 33;
            // 
            // colName
            // 
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 150;
            // 
            // colValue
            // 
            this.colValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colValue.HeaderText = "Value";
            this.colValue.Name = "colValue";
            this.colValue.ReadOnly = true;
            // 
            // linkAboutVsSBE
            // 
            this.linkAboutVsSBE.AutoSize = true;
            this.linkAboutVsSBE.Location = new System.Drawing.Point(330, 27);
            this.linkAboutVsSBE.Name = "linkAboutVsSBE";
            this.linkAboutVsSBE.Size = new System.Drawing.Size(107, 13);
            this.linkAboutVsSBE.TabIndex = 32;
            this.linkAboutVsSBE.TabStop = true;
            this.linkAboutVsSBE.Text = "vsSolutionBuildEvent";
            this.linkAboutVsSBE.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkAboutVsSBE_LinkClicked);
            // 
            // PostProcControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Name = "PostProcControl";
            this.Size = new System.Drawing.Size(442, 230);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProperties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.LinkLabel linkAboutDependent;
        private System.Windows.Forms.RadioButton radioDependentProjects;
        private System.Windows.Forms.LinkLabel linkAboutPostProc;
        private System.Windows.Forms.CheckBox chkIntermediateFiles;
        private System.Windows.Forms.RadioButton radioCustom;
        private System.Windows.Forms.RadioButton radioPostProcDisabled;
        private System.Windows.Forms.CheckBox chkX86X64;
        private System.Windows.Forms.TextBox txtPostProc;
        private System.Windows.Forms.ListBox listActivatedProperties;
        private System.Windows.Forms.DataGridView dgvProperties;
        private System.Windows.Forms.LinkLabel linkAboutVsSBE;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
    }
}

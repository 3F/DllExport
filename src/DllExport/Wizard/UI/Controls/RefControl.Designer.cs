namespace net.r_eg.DllExport.Wizard.UI.Controls
{
    partial class RefControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.radioGetNuTool = new System.Windows.Forms.RadioButton();
            this.radioVS = new System.Windows.Forms.RadioButton();
            this.btnMemoryAndUnsafe = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.Panel();
            this.linkGetNuTool = new System.Windows.Forms.LinkLabel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.dgvList = new net.r_eg.DllExport.Wizard.UI.Components.DataGridViewExt();
            this.colPackage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTfmOrPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDelAsm = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panelMain.SuspendLayout();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.SuspendLayout();
            // 
            // radioGetNuTool
            // 
            this.radioGetNuTool.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioGetNuTool.AutoSize = true;
            this.radioGetNuTool.Enabled = false;
            this.radioGetNuTool.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.radioGetNuTool.Location = new System.Drawing.Point(384, 6);
            this.radioGetNuTool.Name = "radioGetNuTool";
            this.radioGetNuTool.Size = new System.Drawing.Size(13, 12);
            this.radioGetNuTool.TabIndex = 3;
            this.toolTip.SetToolTip(this.radioGetNuTool, "Not implemented in the current version");
            this.radioGetNuTool.UseVisualStyleBackColor = true;
            // 
            // radioVS
            // 
            this.radioVS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioVS.AutoSize = true;
            this.radioVS.Checked = true;
            this.radioVS.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.radioVS.Location = new System.Drawing.Point(276, 4);
            this.radioVS.Name = "radioVS";
            this.radioVS.Size = new System.Drawing.Size(107, 17);
            this.radioVS.TabIndex = 2;
            this.radioVS.TabStop = true;
            this.radioVS.Text = "VS 16+ NuGet   /";
            this.toolTip.SetToolTip(this.radioVS, "Requires `$(Pkg+` feature (VS 16.0+) or select GetNuTool etc.");
            this.radioVS.UseVisualStyleBackColor = true;
            // 
            // btnMemoryAndUnsafe
            // 
            this.btnMemoryAndUnsafe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMemoryAndUnsafe.Location = new System.Drawing.Point(3, 3);
            this.btnMemoryAndUnsafe.Name = "btnMemoryAndUnsafe";
            this.btnMemoryAndUnsafe.Size = new System.Drawing.Size(111, 23);
            this.btnMemoryAndUnsafe.TabIndex = 5;
            this.btnMemoryAndUnsafe.Text = "+Memory +Unsafe";
            this.toolTip.SetToolTip(this.btnMemoryAndUnsafe, "To support Span etc; add System.Memory and System.Runtime.CompilerServices.Unsafe" +
        "");
            this.btnMemoryAndUnsafe.UseVisualStyleBackColor = true;
            this.btnMemoryAndUnsafe.Click += new System.EventHandler(this.btnMemoryAndUnsafe_Click);
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.dgvList);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Margin = new System.Windows.Forms.Padding(0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(0, 29, 0, 0);
            this.panelMain.Size = new System.Drawing.Size(462, 176);
            this.panelMain.TabIndex = 32;
            // 
            // linkGetNuTool
            // 
            this.linkGetNuTool.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkGetNuTool.AutoSize = true;
            this.linkGetNuTool.Location = new System.Drawing.Point(400, 6);
            this.linkGetNuTool.Name = "linkGetNuTool";
            this.linkGetNuTool.Size = new System.Drawing.Size(59, 13);
            this.linkGetNuTool.TabIndex = 4;
            this.linkGetNuTool.TabStop = true;
            this.linkGetNuTool.Text = "GetNuTool";
            this.toolTip.SetToolTip(this.linkGetNuTool, "Not implemented in the current version");
            this.linkGetNuTool.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkGetNuTool_LinkClicked);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.btnMemoryAndUnsafe);
            this.panelTop.Controls.Add(this.radioGetNuTool);
            this.panelTop.Controls.Add(this.linkGetNuTool);
            this.panelTop.Controls.Add(this.radioVS);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(0, 0, 0, 29);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(462, 29);
            this.panelTop.TabIndex = 33;
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToResizeRows = false;
            this.dgvList.AlwaysSelected = false;
            this.dgvList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dgvList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HotTrack;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPackage,
            this.colVersion,
            this.colTfmOrPath,
            this.colDelAsm});
            this.dgvList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvList.DragDropSortable = true;
            this.dgvList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvList.Location = new System.Drawing.Point(0, 29);
            this.dgvList.Margin = new System.Windows.Forms.Padding(0);
            this.dgvList.MultiSelect = false;
            this.dgvList.Name = "dgvList";
            this.dgvList.NumberingForRowsHeader = true;
            this.dgvList.RowHeadersWidth = 32;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(36)))), ((int)(((byte)(47)))));
            this.dgvList.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvList.Size = new System.Drawing.Size(462, 147);
            this.dgvList.StandardTab = true;
            this.dgvList.TabIndex = 1;
            this.dgvList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellClick);
            // 
            // colPackage
            // 
            this.colPackage.HeaderText = "Package name";
            this.colPackage.MinimumWidth = 50;
            this.colPackage.Name = "colPackage";
            this.colPackage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colPackage.ToolTipText = "E.g. System.Memory";
            this.colPackage.Width = 140;
            // 
            // colVersion
            // 
            this.colVersion.HeaderText = "Version";
            this.colVersion.MinimumWidth = 20;
            this.colVersion.Name = "colVersion";
            this.colVersion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colVersion.ToolTipText = "E.g. 4.6.0";
            this.colVersion.Width = 60;
            // 
            // colTfmOrPath
            // 
            this.colTfmOrPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTfmOrPath.HeaderText = "TFM or relative Path to dll";
            this.colTfmOrPath.MinimumWidth = 60;
            this.colTfmOrPath.Name = "colTfmOrPath";
            this.colTfmOrPath.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colTfmOrPath.ToolTipText = "E.g. netstandard2.0 or lib\\netstandard2.0\\System.Memory.dll";
            // 
            // colDelAsm
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Maroon;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Maroon;
            this.colDelAsm.DefaultCellStyle = dataGridViewCellStyle2;
            this.colDelAsm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colDelAsm.HeaderText = "";
            this.colDelAsm.MinimumWidth = 18;
            this.colDelAsm.Name = "colDelAsm";
            this.colDelAsm.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colDelAsm.Text = "x";
            this.colDelAsm.UseColumnTextForButtonValue = true;
            this.colDelAsm.Width = 18;
            // 
            // RefControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Name = "RefControl";
            this.Size = new System.Drawing.Size(462, 176);
            this.panelMain.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel panelMain;
        private Components.DataGridViewExt dgvList;
        private System.Windows.Forms.RadioButton radioGetNuTool;
        private System.Windows.Forms.RadioButton radioVS;
        private System.Windows.Forms.LinkLabel linkGetNuTool;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnMemoryAndUnsafe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPackage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTfmOrPath;
        private System.Windows.Forms.DataGridViewButtonColumn colDelAsm;
    }
}

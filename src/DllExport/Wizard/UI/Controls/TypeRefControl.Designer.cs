namespace net.r_eg.DllExport.Wizard.UI.Controls
{
    partial class TypeRefControl
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
            this.chkInterpolation = new System.Windows.Forms.CheckBox();
            this.panelMain = new System.Windows.Forms.Panel();
            this.linkTypeRef = new System.Windows.Forms.LinkLabel();
            this.dgvList = new net.r_eg.DllExport.Wizard.UI.Components.DataGridViewExt();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFlagAny = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colScopeType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colScope = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDelTyperef = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelExtra = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkInterpolation
            // 
            this.chkInterpolation.AutoSize = true;
            this.chkInterpolation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkInterpolation.Location = new System.Drawing.Point(43, 3);
            this.chkInterpolation.Name = "chkInterpolation";
            this.chkInterpolation.Size = new System.Drawing.Size(147, 17);
            this.chkInterpolation.TabIndex = 0;
            this.chkInterpolation.Text = "$ interpolation for .NET 6+";
            this.toolTip.SetToolTip(this.chkInterpolation, "Through DefaultInterpolatedStringHandler(.NET 6.0+) stub; Or use other implementa" +
        "tion via .typeref ...");
            this.chkInterpolation.UseVisualStyleBackColor = true;
            this.chkInterpolation.CheckedChanged += new System.EventHandler(this.chkInterpolation_CheckedChanged);
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.linkTypeRef);
            this.panelMain.Controls.Add(this.dgvList);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Margin = new System.Windows.Forms.Padding(0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(0, 29, 0, 0);
            this.panelMain.Size = new System.Drawing.Size(462, 180);
            this.panelMain.TabIndex = 32;
            // 
            // linkTypeRef
            // 
            this.linkTypeRef.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkTypeRef.AutoSize = true;
            this.linkTypeRef.Location = new System.Drawing.Point(447, 30);
            this.linkTypeRef.Name = "linkTypeRef";
            this.linkTypeRef.Size = new System.Drawing.Size(13, 13);
            this.linkTypeRef.TabIndex = 3;
            this.linkTypeRef.TabStop = true;
            this.linkTypeRef.Text = "?";
            this.linkTypeRef.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkTypeRef_LinkClicked);
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
            this.colType,
            this.colFlagAny,
            this.colScopeType,
            this.colScope,
            this.colDelTyperef});
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
            this.dgvList.Size = new System.Drawing.Size(462, 151);
            this.dgvList.StandardTab = true;
            this.dgvList.TabIndex = 2;
            this.dgvList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellClick);
            this.dgvList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellValueChanged);
            this.dgvList.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvList_RowsAdded);
            // 
            // colType
            // 
            this.colType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colType.HeaderText = ".typeref";
            this.colType.MinimumWidth = 40;
            this.colType.Name = "colType";
            this.colType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colFlagAny
            // 
            this.colFlagAny.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colFlagAny.HeaderText = "any";
            this.colFlagAny.MinimumWidth = 22;
            this.colFlagAny.Name = "colFlagAny";
            this.colFlagAny.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colFlagAny.Width = 28;
            // 
            // colScopeType
            // 
            this.colScopeType.HeaderText = "scope";
            this.colScopeType.Items.AddRange(new object[] {
            "at",
            "assert",
            "deny"});
            this.colScopeType.MinimumWidth = 60;
            this.colScopeType.Name = "colScopeType";
            this.colScopeType.Width = 90;
            // 
            // colScope
            // 
            this.colScope.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colScope.HeaderText = "value";
            this.colScope.MinimumWidth = 80;
            this.colScope.Name = "colScope";
            this.colScope.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colDelTyperef
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Maroon;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Maroon;
            this.colDelTyperef.DefaultCellStyle = dataGridViewCellStyle2;
            this.colDelTyperef.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colDelTyperef.HeaderText = "";
            this.colDelTyperef.MinimumWidth = 18;
            this.colDelTyperef.Name = "colDelTyperef";
            this.colDelTyperef.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colDelTyperef.Text = "x";
            this.colDelTyperef.UseColumnTextForButtonValue = true;
            this.colDelTyperef.Width = 18;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.labelExtra);
            this.panelTop.Controls.Add(this.chkInterpolation);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(0, 0, 0, 29);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(462, 29);
            this.panelTop.TabIndex = 4;
            // 
            // labelExtra
            // 
            this.labelExtra.AutoSize = true;
            this.labelExtra.Location = new System.Drawing.Point(3, 5);
            this.labelExtra.Name = "labelExtra";
            this.labelExtra.Size = new System.Drawing.Size(34, 13);
            this.labelExtra.TabIndex = 1;
            this.labelExtra.Text = "Extra:";
            // 
            // TypeRefControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Name = "TypeRefControl";
            this.Size = new System.Drawing.Size(462, 180);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel panelMain;
        private Components.DataGridViewExt dgvList;
        private System.Windows.Forms.LinkLabel linkTypeRef;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colFlagAny;
        private System.Windows.Forms.DataGridViewComboBoxColumn colScopeType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScope;
        private System.Windows.Forms.DataGridViewButtonColumn colDelTyperef;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.CheckBox chkInterpolation;
        private System.Windows.Forms.Label labelExtra;
    }
}

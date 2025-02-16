namespace net.r_eg.DllExport.Wizard.UI.Controls
{
    partial class AsmControl
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
            this.panelMain = new System.Windows.Forms.Panel();
            this.dgvList = new net.r_eg.DllExport.Wizard.UI.Components.DataGridViewExt();
            this.colAssembly = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKeytoken = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAsmVer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDelAsm = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.dgvList);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(491, 99);
            this.panelMain.TabIndex = 32;
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
            this.colAssembly,
            this.colKeytoken,
            this.colAsmVer,
            this.colDelAsm});
            this.dgvList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvList.DragDropSortable = true;
            this.dgvList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvList.Location = new System.Drawing.Point(0, 0);
            this.dgvList.Margin = new System.Windows.Forms.Padding(0);
            this.dgvList.MultiSelect = false;
            this.dgvList.Name = "dgvList";
            this.dgvList.NumberingForRowsHeader = true;
            this.dgvList.RowHeadersWidth = 32;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(241)))), ((int)(((byte)(253)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(36)))), ((int)(((byte)(47)))));
            this.dgvList.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvList.Size = new System.Drawing.Size(491, 99);
            this.dgvList.StandardTab = true;
            this.dgvList.TabIndex = 1;
            this.dgvList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellClick);
            // 
            // colAssembly
            // 
            this.colAssembly.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colAssembly.HeaderText = ".assembly extern";
            this.colAssembly.MinimumWidth = 60;
            this.colAssembly.Name = "colAssembly";
            this.colAssembly.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colAssembly.ToolTipText = "E.g. System.Memory";
            // 
            // colKeytoken
            // 
            this.colKeytoken.HeaderText = ".publickeytoken";
            this.colKeytoken.MinimumWidth = 85;
            this.colKeytoken.Name = "colKeytoken";
            this.colKeytoken.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colKeytoken.ToolTipText = "E.g. CC 7B 13 FF CD 2D DD 51";
            this.colKeytoken.Width = 191;
            // 
            // colAsmVer
            // 
            this.colAsmVer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colAsmVer.HeaderText = "version";
            this.colAsmVer.MinimumWidth = 60;
            this.colAsmVer.Name = "colAsmVer";
            this.colAsmVer.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colAsmVer.ToolTipText = "major:minor:build:rev E.g. 4:0:2:0";
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
            // AsmControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.panelMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Name = "AsmControl";
            this.Size = new System.Drawing.Size(491, 99);
            this.panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel panelMain;
        private Components.DataGridViewExt dgvList;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAssembly;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKeytoken;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAsmVer;
        private System.Windows.Forms.DataGridViewButtonColumn colDelAsm;
    }
}

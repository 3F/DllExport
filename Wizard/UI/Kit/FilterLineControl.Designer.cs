namespace net.r_eg.DllExport.Wizard.UI.Kit
{
    partial class FilterLineControl
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
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnInfo = new System.Windows.Forms.Button();
            this.panelFBorder = new System.Windows.Forms.Panel();
            this.btnBug = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelFBorder.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.textBoxFilter.Location = new System.Drawing.Point(1, 1);
            this.textBoxFilter.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(390, 16);
            this.textBoxFilter.TabIndex = 0;
            this.toolTip.SetToolTip(this.textBoxFilter, "Filter by project path");
            this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnClose.Location = new System.Drawing.Point(443, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(26, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "x";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip.SetToolTip(this.btnClose, "Close panel");
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnInfo
            // 
            this.btnInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInfo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnInfo.Location = new System.Drawing.Point(398, 0);
            this.btnInfo.Margin = new System.Windows.Forms.Padding(0);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(18, 23);
            this.btnInfo.TabIndex = 6;
            this.btnInfo.Text = "!";
            this.toolTip.SetToolTip(this.btnInfo, "Information");
            this.btnInfo.UseVisualStyleBackColor = true;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // panelFBorder
            // 
            this.panelFBorder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFBorder.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.panelFBorder.Controls.Add(this.textBoxFilter);
            this.panelFBorder.Location = new System.Drawing.Point(2, 3);
            this.panelFBorder.Margin = new System.Windows.Forms.Padding(0);
            this.panelFBorder.Name = "panelFBorder";
            this.panelFBorder.Padding = new System.Windows.Forms.Padding(1);
            this.panelFBorder.Size = new System.Drawing.Size(392, 18);
            this.panelFBorder.TabIndex = 7;
            // 
            // btnBug
            // 
            this.btnBug.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBug.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBug.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnBug.Location = new System.Drawing.Point(418, 0);
            this.btnBug.Margin = new System.Windows.Forms.Padding(0);
            this.btnBug.Name = "btnBug";
            this.btnBug.Size = new System.Drawing.Size(18, 23);
            this.btnBug.TabIndex = 8;
            this.btnBug.Text = "#";
            this.toolTip.SetToolTip(this.btnBug, "Bugs ?");
            this.btnBug.UseVisualStyleBackColor = true;
            this.btnBug.Click += new System.EventHandler(this.btnBug_Click);
            // 
            // FilterLineControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnBug);
            this.Controls.Add(this.btnInfo);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.panelFBorder);
            this.Name = "FilterLineControl";
            this.Size = new System.Drawing.Size(469, 24);
            this.panelFBorder.ResumeLayout(false);
            this.panelFBorder.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnInfo;
        private System.Windows.Forms.Panel panelFBorder;
        private System.Windows.Forms.Button btnBug;
        private System.Windows.Forms.ToolTip toolTip;
    }
}

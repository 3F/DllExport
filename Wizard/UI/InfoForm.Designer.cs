namespace net.r_eg.DllExport.Wizard.UI
{
    partial class InfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoForm));
            this.linkWhy = new System.Windows.Forms.LinkLabel();
            this.linkLocalDxp = new System.Windows.Forms.LinkLabel();
            this.linkRemoteDxp = new System.Windows.Forms.LinkLabel();
            this.labelDllExportBatch = new System.Windows.Forms.Label();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.textBoxInfo = new System.Windows.Forms.TextBox();
            this.panelInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // linkWhy
            // 
            this.linkWhy.AutoSize = true;
            this.linkWhy.Location = new System.Drawing.Point(12, 140);
            this.linkWhy.Name = "linkWhy";
            this.linkWhy.Size = new System.Drawing.Size(62, 13);
            this.linkWhy.TabIndex = 1;
            this.linkWhy.TabStop = true;
            this.linkWhy.Text = "Why is so ?";
            this.linkWhy.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkWhy_LinkClicked);
            // 
            // linkLocalDxp
            // 
            this.linkLocalDxp.AutoSize = true;
            this.linkLocalDxp.Location = new System.Drawing.Point(41, 184);
            this.linkLocalDxp.Name = "linkLocalDxp";
            this.linkLocalDxp.Size = new System.Drawing.Size(157, 13);
            this.linkLocalDxp.TabIndex = 2;
            this.linkLocalDxp.TabStop = true;
            this.linkLocalDxp.Text = "Local version from this package";
            this.linkLocalDxp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLocalDxp_LinkClicked);
            // 
            // linkRemoteDxp
            // 
            this.linkRemoteDxp.AutoSize = true;
            this.linkRemoteDxp.Location = new System.Drawing.Point(41, 208);
            this.linkRemoteDxp.Name = "linkRemoteDxp";
            this.linkRemoteDxp.Size = new System.Drawing.Size(164, 13);
            this.linkRemoteDxp.TabIndex = 3;
            this.linkRemoteDxp.TabStop = true;
            this.linkRemoteDxp.Text = "Latest version from GitHub server";
            this.linkRemoteDxp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkRemoteDxp_LinkClicked);
            // 
            // labelDllExportBatch
            // 
            this.labelDllExportBatch.AutoSize = true;
            this.labelDllExportBatch.Location = new System.Drawing.Point(12, 162);
            this.labelDllExportBatch.Name = "labelDllExportBatch";
            this.labelDllExportBatch.Size = new System.Drawing.Size(175, 13);
            this.labelDllExportBatch.TabIndex = 4;
            this.labelDllExportBatch.Text = "DllExport.bat ( batch script ~20 Kb )";
            // 
            // panelInfo
            // 
            this.panelInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(195)))), ((int)(((byte)(101)))));
            this.panelInfo.Controls.Add(this.textBoxInfo);
            this.panelInfo.Location = new System.Drawing.Point(2, 2);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Padding = new System.Windows.Forms.Padding(1);
            this.panelInfo.Size = new System.Drawing.Size(449, 135);
            this.panelInfo.TabIndex = 5;
            // 
            // textBoxInfo
            // 
            this.textBoxInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(191)))));
            this.textBoxInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxInfo.Location = new System.Drawing.Point(1, 1);
            this.textBoxInfo.Multiline = true;
            this.textBoxInfo.Name = "textBoxInfo";
            this.textBoxInfo.ReadOnly = true;
            this.textBoxInfo.Size = new System.Drawing.Size(447, 133);
            this.textBoxInfo.TabIndex = 1;
            this.textBoxInfo.Text = resources.GetString("textBoxInfo.Text");
            // 
            // InfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 234);
            this.Controls.Add(this.panelInfo);
            this.Controls.Add(this.labelDllExportBatch);
            this.Controls.Add(this.linkRemoteDxp);
            this.Controls.Add(this.linkLocalDxp);
            this.Controls.Add(this.linkWhy);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InfoForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = ".NET DllExport :: Attention please !";
            this.TopMost = true;
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.LinkLabel linkWhy;
        private System.Windows.Forms.LinkLabel linkLocalDxp;
        private System.Windows.Forms.LinkLabel linkRemoteDxp;
        private System.Windows.Forms.Label labelDllExportBatch;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.TextBox textBoxInfo;
    }
}
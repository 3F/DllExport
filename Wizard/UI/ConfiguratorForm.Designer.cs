namespace net.r_eg.DllExport.Wizard.UI
{
    partial class ConfiguratorForm
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
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnApply = new System.Windows.Forms.Button();
            this.comboBoxSln = new System.Windows.Forms.ComboBox();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelTop.Controls.Add(this.btnApply);
            this.panelTop.Controls.Add(this.comboBoxSln);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(470, 28);
            this.panelTop.TabIndex = 0;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(388, 1);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            // 
            // comboBoxSln
            // 
            this.comboBoxSln.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSln.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSln.FormattingEnabled = true;
            this.comboBoxSln.Location = new System.Drawing.Point(3, 3);
            this.comboBoxSln.Name = "comboBoxSln";
            this.comboBoxSln.Size = new System.Drawing.Size(379, 21);
            this.comboBoxSln.TabIndex = 0;
            this.comboBoxSln.SelectedIndexChanged += new System.EventHandler(this.comboBoxSln_SelectedIndexChanged);
            // 
            // panelMain
            // 
            this.panelMain.AutoScroll = true;
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 28);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(470, 422);
            this.panelMain.TabIndex = 1;
            // 
            // ConfiguratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 450);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelTop);
            this.MaximizeBox = false;
            this.Name = "ConfiguratorForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ConfiguratorForm";
            this.panelTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.ComboBox comboBoxSln;
        private System.Windows.Forms.Panel panelMain;
    }
}
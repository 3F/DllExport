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
            this.components = new System.ComponentModel.Container();
            this.panelTop = new System.Windows.Forms.Panel();
            this.progressLine = new net.r_eg.DllExport.Wizard.UI.Controls.ProgressLineControl();
            this.comboBoxStorage = new System.Windows.Forms.ComboBox();
            this.btnExt = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.comboBoxSln = new System.Windows.Forms.ComboBox();
            this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
            this.projectItems = new net.r_eg.DllExport.Wizard.UI.Controls.ProjectItemsControl();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelTop.Controls.Add(this.progressLine);
            this.panelTop.Controls.Add(this.comboBoxStorage);
            this.panelTop.Controls.Add(this.btnExt);
            this.panelTop.Controls.Add(this.btnApply);
            this.panelTop.Controls.Add(this.comboBoxSln);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(469, 29);
            this.panelTop.TabIndex = 0;
            // 
            // progressLine
            // 
            this.progressLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressLine.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.progressLine.Location = new System.Drawing.Point(0, 21);
            this.progressLine.Name = "progressLine";
            this.progressLine.Size = new System.Drawing.Size(275, 5);
            this.progressLine.TabIndex = 8;
            // 
            // comboBoxStorage
            // 
            this.comboBoxStorage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxStorage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStorage.DropDownWidth = 190;
            this.comboBoxStorage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxStorage.FormattingEnabled = true;
            this.comboBoxStorage.Location = new System.Drawing.Point(297, 2);
            this.comboBoxStorage.Margin = new System.Windows.Forms.Padding(1);
            this.comboBoxStorage.Name = "comboBoxStorage";
            this.comboBoxStorage.Size = new System.Drawing.Size(78, 21);
            this.comboBoxStorage.TabIndex = 7;
            this.toolTipMain.SetToolTip(this.comboBoxStorage, "Storage");
            // 
            // btnExt
            // 
            this.btnExt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExt.Location = new System.Drawing.Point(439, 1);
            this.btnExt.Margin = new System.Windows.Forms.Padding(0);
            this.btnExt.Name = "btnExt";
            this.btnExt.Size = new System.Drawing.Size(26, 23);
            this.btnExt.TabIndex = 2;
            this.btnExt.Text = "+";
            this.btnExt.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTipMain.SetToolTip(this.btnExt, "Features");
            this.btnExt.UseVisualStyleBackColor = true;
            this.btnExt.Click += new System.EventHandler(this.btnExt_Click);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.Location = new System.Drawing.Point(377, 1);
            this.btnApply.Margin = new System.Windows.Forms.Padding(1);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(61, 23);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "Apply";
            this.toolTipMain.SetToolTip(this.btnApply, "Apply changes");
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // comboBoxSln
            // 
            this.comboBoxSln.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSln.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSln.DropDownWidth = 500;
            this.comboBoxSln.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.comboBoxSln.FormattingEnabled = true;
            this.comboBoxSln.Location = new System.Drawing.Point(1, 2);
            this.comboBoxSln.Margin = new System.Windows.Forms.Padding(1);
            this.comboBoxSln.Name = "comboBoxSln";
            this.comboBoxSln.Size = new System.Drawing.Size(294, 21);
            this.comboBoxSln.TabIndex = 0;
            this.toolTipMain.SetToolTip(this.comboBoxSln, "Solution File");
            this.comboBoxSln.SelectedIndexChanged += new System.EventHandler(this.comboBoxSln_SelectedIndexChanged);
            // 
            // projectItems
            // 
            this.projectItems.Browse = null;
            this.projectItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.projectItems.Location = new System.Drawing.Point(0, 29);
            this.projectItems.Name = "projectItems";
            this.projectItems.NamespaceValidate = null;
            this.projectItems.OpenUrl = null;
            this.projectItems.Size = new System.Drawing.Size(469, 421);
            this.projectItems.TabIndex = 1;
            this.projectItems.RenderedItemsSizeChanged += new System.EventHandler(this.projectItems_RenderedItemsSizeChanged);
            // 
            // ConfiguratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 450);
            this.Controls.Add(this.projectItems);
            this.Controls.Add(this.panelTop);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(495, 2000);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(470, 39);
            this.Name = "ConfiguratorForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DllExport";
            this.TopMost = true;
            this.panelTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.ComboBox comboBoxSln;
        private Controls.ProjectItemsControl projectItems;
        private System.Windows.Forms.Button btnExt;
        private System.Windows.Forms.ToolTip toolTipMain;
        private System.Windows.Forms.ComboBox comboBoxStorage;
        private Controls.ProgressLineControl progressLine;
    }
}
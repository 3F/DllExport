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
            fdialog?.Dispose();
            icons?.Dispose();

            ctsUpdater?.Cancel();

            if(tUpdater != null)
            {
                tUpdater.Wait();
                tUpdater.Dispose();
            }
            ctsUpdater?.Dispose();

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelTop = new System.Windows.Forms.Panel();
            this.progressLine = new net.r_eg.DllExport.Wizard.UI.Controls.ProgressLineControl();
            this.btnApply = new System.Windows.Forms.Button();
            this.comboBoxSln = new System.Windows.Forms.ComboBox();
            this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
            this.btnUpdListOfPkg = new System.Windows.Forms.Button();
            this.btnToOnline = new System.Windows.Forms.Button();
            this.comboBoxStorage = new System.Windows.Forms.ComboBox();
            this.splitCon = new System.Windows.Forms.SplitContainer();
            this.panelPrjs = new System.Windows.Forms.Panel();
            this.dgvFilter = new net.r_eg.vsSBE.UI.WForms.Components.DataGridViewExt();
            this.gcInstalled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gcType = new System.Windows.Forms.DataGridViewImageColumn();
            this.gcPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelFilter = new System.Windows.Forms.Panel();
            this.tabCtrl = new System.Windows.Forms.TabControl();
            this.tabCfgDxp = new System.Windows.Forms.TabPage();
            this.projectItems = new net.r_eg.DllExport.Wizard.UI.Controls.ProjectItemsControl();
            this.tabData = new System.Windows.Forms.TabPage();
            this.labelStorage = new System.Windows.Forms.Label();
            this.txtCfgData = new System.Windows.Forms.TextBox();
            this.tabUpdating = new System.Windows.Forms.TabPage();
            this.txtLogUpd = new System.Windows.Forms.TextBox();
            this.panelUpdVerTop = new System.Windows.Forms.Panel();
            this.cbPackages = new System.Windows.Forms.ComboBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.tabBuildInfo = new System.Windows.Forms.TabPage();
            this.labelSrcMit = new System.Windows.Forms.Label();
            this.linkIlasm = new System.Windows.Forms.LinkLabel();
            this.lnkSrc = new System.Windows.Forms.LinkLabel();
            this.lnk3F = new System.Windows.Forms.LinkLabel();
            this.labelSrc = new System.Windows.Forms.Label();
            this.txtBuildInfo = new System.Windows.Forms.TextBox();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitCon)).BeginInit();
            this.splitCon.Panel1.SuspendLayout();
            this.splitCon.Panel2.SuspendLayout();
            this.splitCon.SuspendLayout();
            this.panelPrjs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilter)).BeginInit();
            this.tabCtrl.SuspendLayout();
            this.tabCfgDxp.SuspendLayout();
            this.tabData.SuspendLayout();
            this.tabUpdating.SuspendLayout();
            this.panelUpdVerTop.SuspendLayout();
            this.tabBuildInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelTop.Controls.Add(this.progressLine);
            this.panelTop.Controls.Add(this.btnApply);
            this.panelTop.Controls.Add(this.comboBoxSln);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(446, 29);
            this.panelTop.TabIndex = 0;
            // 
            // progressLine
            // 
            this.progressLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressLine.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.progressLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.progressLine.Location = new System.Drawing.Point(0, 21);
            this.progressLine.Name = "progressLine";
            this.progressLine.Size = new System.Drawing.Size(275, 5);
            this.progressLine.TabIndex = 8;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.Location = new System.Drawing.Point(381, 1);
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
            this.comboBoxSln.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxSln.FormattingEnabled = true;
            this.comboBoxSln.Location = new System.Drawing.Point(1, 2);
            this.comboBoxSln.Margin = new System.Windows.Forms.Padding(1);
            this.comboBoxSln.Name = "comboBoxSln";
            this.comboBoxSln.Size = new System.Drawing.Size(378, 21);
            this.comboBoxSln.TabIndex = 0;
            this.toolTipMain.SetToolTip(this.comboBoxSln, "Solution File");
            this.comboBoxSln.SelectedIndexChanged += new System.EventHandler(this.comboBoxSln_SelectedIndexChanged);
            // 
            // btnUpdListOfPkg
            // 
            this.btnUpdListOfPkg.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnUpdListOfPkg.Location = new System.Drawing.Point(287, 4);
            this.btnUpdListOfPkg.Name = "btnUpdListOfPkg";
            this.btnUpdListOfPkg.Size = new System.Drawing.Size(38, 23);
            this.btnUpdListOfPkg.TabIndex = 2;
            this.btnUpdListOfPkg.Text = "( @ )";
            this.toolTipMain.SetToolTip(this.btnUpdListOfPkg, "<< Receive new list");
            this.btnUpdListOfPkg.UseVisualStyleBackColor = true;
            this.btnUpdListOfPkg.Click += new System.EventHandler(this.BtnUpdListOfPkg_Click);
            // 
            // btnToOnline
            // 
            this.btnToOnline.Location = new System.Drawing.Point(6, 73);
            this.btnToOnline.Name = "btnToOnline";
            this.btnToOnline.Size = new System.Drawing.Size(148, 23);
            this.btnToOnline.TabIndex = 15;
            this.btnToOnline.Text = "Convert to online version";
            this.toolTipMain.SetToolTip(this.btnToOnline, "It will try to convert to the normal online version");
            this.btnToOnline.UseVisualStyleBackColor = true;
            this.btnToOnline.Visible = false;
            this.btnToOnline.Click += new System.EventHandler(this.BtnToOnline_Click);
            // 
            // comboBoxStorage
            // 
            this.comboBoxStorage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxStorage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStorage.DropDownWidth = 190;
            this.comboBoxStorage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxStorage.FormattingEnabled = true;
            this.comboBoxStorage.Location = new System.Drawing.Point(251, 4);
            this.comboBoxStorage.Margin = new System.Windows.Forms.Padding(1);
            this.comboBoxStorage.Name = "comboBoxStorage";
            this.comboBoxStorage.Size = new System.Drawing.Size(170, 21);
            this.comboBoxStorage.TabIndex = 14;
            this.toolTipMain.SetToolTip(this.comboBoxStorage, "Storage");
            // 
            // splitCon
            // 
            this.splitCon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitCon.IsSplitterFixed = true;
            this.splitCon.Location = new System.Drawing.Point(0, 29);
            this.splitCon.Margin = new System.Windows.Forms.Padding(0);
            this.splitCon.Name = "splitCon";
            this.splitCon.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitCon.Panel1
            // 
            this.splitCon.Panel1.Controls.Add(this.panelPrjs);
            this.splitCon.Panel1.Controls.Add(this.panelFilter);
            // 
            // splitCon.Panel2
            // 
            this.splitCon.Panel2.Controls.Add(this.tabCtrl);
            this.splitCon.Size = new System.Drawing.Size(446, 341);
            this.splitCon.SplitterDistance = 78;
            this.splitCon.TabIndex = 2;
            // 
            // panelPrjs
            // 
            this.panelPrjs.Controls.Add(this.dgvFilter);
            this.panelPrjs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPrjs.Location = new System.Drawing.Point(0, 26);
            this.panelPrjs.Margin = new System.Windows.Forms.Padding(0);
            this.panelPrjs.Name = "panelPrjs";
            this.panelPrjs.Size = new System.Drawing.Size(446, 52);
            this.panelPrjs.TabIndex = 2;
            // 
            // dgvFilter
            // 
            this.dgvFilter.AllowUserToAddRows = false;
            this.dgvFilter.AllowUserToDeleteRows = false;
            this.dgvFilter.AllowUserToResizeRows = false;
            this.dgvFilter.AlwaysSelected = true;
            this.dgvFilter.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvFilter.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvFilter.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgvFilter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFilter.ColumnHeadersVisible = false;
            this.dgvFilter.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gcInstalled,
            this.gcType,
            this.gcPath});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(238)))), ((int)(((byte)(239)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilter.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFilter.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvFilter.Location = new System.Drawing.Point(0, 0);
            this.dgvFilter.Margin = new System.Windows.Forms.Padding(0);
            this.dgvFilter.MultiSelect = false;
            this.dgvFilter.Name = "dgvFilter";
            this.dgvFilter.NumberingForRowsHeader = false;
            this.dgvFilter.ReadOnly = true;
            this.dgvFilter.RowHeadersVisible = false;
            this.dgvFilter.RowHeadersWidth = 28;
            this.dgvFilter.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvFilter.RowTemplate.Height = 17;
            this.dgvFilter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvFilter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFilter.Size = new System.Drawing.Size(446, 52);
            this.dgvFilter.TabIndex = 0;
            this.dgvFilter.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFilter_RowEnter);
            this.dgvFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvFilter_KeyDown);
            // 
            // gcInstalled
            // 
            this.gcInstalled.HeaderText = "";
            this.gcInstalled.MinimumWidth = 2;
            this.gcInstalled.Name = "gcInstalled";
            this.gcInstalled.ReadOnly = true;
            this.gcInstalled.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.gcInstalled.Width = 6;
            // 
            // gcType
            // 
            this.gcType.HeaderText = "Type";
            this.gcType.MinimumWidth = 16;
            this.gcType.Name = "gcType";
            this.gcType.ReadOnly = true;
            this.gcType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.gcType.ToolTipText = "Project Type";
            this.gcType.Width = 32;
            // 
            // gcPath
            // 
            this.gcPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.gcPath.HeaderText = "Path";
            this.gcPath.MinimumWidth = 70;
            this.gcPath.Name = "gcPath";
            this.gcPath.ReadOnly = true;
            this.gcPath.ToolTipText = "Path to project file";
            // 
            // panelFilter
            // 
            this.panelFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilter.Location = new System.Drawing.Point(0, 0);
            this.panelFilter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.Size = new System.Drawing.Size(446, 26);
            this.panelFilter.TabIndex = 1;
            // 
            // tabCtrl
            // 
            this.tabCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCtrl.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabCtrl.Controls.Add(this.tabCfgDxp);
            this.tabCtrl.Controls.Add(this.tabData);
            this.tabCtrl.Controls.Add(this.tabUpdating);
            this.tabCtrl.Controls.Add(this.tabBuildInfo);
            this.tabCtrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabCtrl.Location = new System.Drawing.Point(-4, 0);
            this.tabCtrl.Margin = new System.Windows.Forms.Padding(0);
            this.tabCtrl.Name = "tabCtrl";
            this.tabCtrl.SelectedIndex = 0;
            this.tabCtrl.Size = new System.Drawing.Size(450, 259);
            this.tabCtrl.TabIndex = 0;
            // 
            // tabCfgDxp
            // 
            this.tabCfgDxp.Controls.Add(this.projectItems);
            this.tabCfgDxp.Location = new System.Drawing.Point(4, 25);
            this.tabCfgDxp.Name = "tabCfgDxp";
            this.tabCfgDxp.Padding = new System.Windows.Forms.Padding(3);
            this.tabCfgDxp.Size = new System.Drawing.Size(442, 230);
            this.tabCfgDxp.TabIndex = 0;
            this.tabCfgDxp.Text = "Options";
            this.tabCfgDxp.UseVisualStyleBackColor = true;
            // 
            // projectItems
            // 
            this.projectItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.projectItems.BackColor = System.Drawing.SystemColors.Control;
            this.projectItems.Browse = null;
            this.projectItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.projectItems.Location = new System.Drawing.Point(0, 0);
            this.projectItems.Margin = new System.Windows.Forms.Padding(0);
            this.projectItems.Name = "projectItems";
            this.projectItems.NamespaceValidate = null;
            this.projectItems.OpenUrl = null;
            this.projectItems.Size = new System.Drawing.Size(448, 247);
            this.projectItems.TabIndex = 2;
            // 
            // tabData
            // 
            this.tabData.Controls.Add(this.labelStorage);
            this.tabData.Controls.Add(this.comboBoxStorage);
            this.tabData.Controls.Add(this.txtCfgData);
            this.tabData.Location = new System.Drawing.Point(4, 25);
            this.tabData.Name = "tabData";
            this.tabData.Size = new System.Drawing.Size(442, 230);
            this.tabData.TabIndex = 3;
            this.tabData.Text = "Data";
            this.tabData.UseVisualStyleBackColor = true;
            // 
            // labelStorage
            // 
            this.labelStorage.AutoSize = true;
            this.labelStorage.Location = new System.Drawing.Point(180, 7);
            this.labelStorage.Name = "labelStorage";
            this.labelStorage.Size = new System.Drawing.Size(67, 13);
            this.labelStorage.TabIndex = 15;
            this.labelStorage.Text = "Use storage:";
            // 
            // txtCfgData
            // 
            this.txtCfgData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCfgData.BackColor = System.Drawing.SystemColors.Window;
            this.txtCfgData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCfgData.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCfgData.Location = new System.Drawing.Point(3, 29);
            this.txtCfgData.Multiline = true;
            this.txtCfgData.Name = "txtCfgData";
            this.txtCfgData.ReadOnly = true;
            this.txtCfgData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCfgData.Size = new System.Drawing.Size(436, 198);
            this.txtCfgData.TabIndex = 13;
            // 
            // tabUpdating
            // 
            this.tabUpdating.Controls.Add(this.btnToOnline);
            this.tabUpdating.Controls.Add(this.txtLogUpd);
            this.tabUpdating.Controls.Add(this.panelUpdVerTop);
            this.tabUpdating.Location = new System.Drawing.Point(4, 25);
            this.tabUpdating.Name = "tabUpdating";
            this.tabUpdating.Padding = new System.Windows.Forms.Padding(3);
            this.tabUpdating.Size = new System.Drawing.Size(442, 230);
            this.tabUpdating.TabIndex = 2;
            this.tabUpdating.Text = "Updater";
            this.tabUpdating.UseVisualStyleBackColor = true;
            // 
            // txtLogUpd
            // 
            this.txtLogUpd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLogUpd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(12)))));
            this.txtLogUpd.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLogUpd.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLogUpd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(203)))), ((int)(((byte)(203)))));
            this.txtLogUpd.Location = new System.Drawing.Point(0, 33);
            this.txtLogUpd.Multiline = true;
            this.txtLogUpd.Name = "txtLogUpd";
            this.txtLogUpd.ReadOnly = true;
            this.txtLogUpd.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLogUpd.Size = new System.Drawing.Size(446, 201);
            this.txtLogUpd.TabIndex = 13;
            this.txtLogUpd.WordWrap = false;
            // 
            // panelUpdVerTop
            // 
            this.panelUpdVerTop.Controls.Add(this.btnUpdListOfPkg);
            this.panelUpdVerTop.Controls.Add(this.cbPackages);
            this.panelUpdVerTop.Controls.Add(this.btnUpdate);
            this.panelUpdVerTop.Location = new System.Drawing.Point(39, 0);
            this.panelUpdVerTop.Name = "panelUpdVerTop";
            this.panelUpdVerTop.Size = new System.Drawing.Size(342, 34);
            this.panelUpdVerTop.TabIndex = 14;
            // 
            // cbPackages
            // 
            this.cbPackages.FormattingEnabled = true;
            this.cbPackages.Location = new System.Drawing.Point(121, 6);
            this.cbPackages.Name = "cbPackages";
            this.cbPackages.Size = new System.Drawing.Size(160, 21);
            this.cbPackages.TabIndex = 1;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(11, 4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(104, 23);
            this.btnUpdate.TabIndex = 0;
            this.btnUpdate.Text = "Update to";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // tabBuildInfo
            // 
            this.tabBuildInfo.BackColor = System.Drawing.SystemColors.Control;
            this.tabBuildInfo.Controls.Add(this.labelSrcMit);
            this.tabBuildInfo.Controls.Add(this.linkIlasm);
            this.tabBuildInfo.Controls.Add(this.lnkSrc);
            this.tabBuildInfo.Controls.Add(this.lnk3F);
            this.tabBuildInfo.Controls.Add(this.labelSrc);
            this.tabBuildInfo.Controls.Add(this.txtBuildInfo);
            this.tabBuildInfo.Location = new System.Drawing.Point(4, 25);
            this.tabBuildInfo.Name = "tabBuildInfo";
            this.tabBuildInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabBuildInfo.Size = new System.Drawing.Size(442, 230);
            this.tabBuildInfo.TabIndex = 1;
            this.tabBuildInfo.Text = "Build info";
            // 
            // labelSrcMit
            // 
            this.labelSrcMit.AutoSize = true;
            this.labelSrcMit.Location = new System.Drawing.Point(41, 23);
            this.labelSrcMit.Name = "labelSrcMit";
            this.labelSrcMit.Size = new System.Drawing.Size(38, 13);
            this.labelSrcMit.TabIndex = 21;
            this.labelSrcMit.Text = "( MIT )";
            // 
            // linkIlasm
            // 
            this.linkIlasm.AutoSize = true;
            this.linkIlasm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkIlasm.Location = new System.Drawing.Point(108, 23);
            this.linkIlasm.Name = "linkIlasm";
            this.linkIlasm.Size = new System.Drawing.Size(149, 13);
            this.linkIlasm.TabIndex = 20;
            this.linkIlasm.TabStop = true;
            this.linkIlasm.Text = "https://github.com/3F/coreclr";
            this.linkIlasm.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkIlasm_LinkClicked);
            // 
            // lnkSrc
            // 
            this.lnkSrc.AutoSize = true;
            this.lnkSrc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkSrc.Location = new System.Drawing.Point(108, 3);
            this.lnkSrc.Name = "lnkSrc";
            this.lnkSrc.Size = new System.Drawing.Size(159, 13);
            this.lnkSrc.TabIndex = 16;
            this.lnkSrc.TabStop = true;
            this.lnkSrc.Text = "https://github.com/3F/DllExport";
            this.lnkSrc.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSrc_LinkClicked);
            // 
            // lnk3F
            // 
            this.lnk3F.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnk3F.AutoSize = true;
            this.lnk3F.Location = new System.Drawing.Point(377, 23);
            this.lnk3F.Name = "lnk3F";
            this.lnk3F.Size = new System.Drawing.Size(57, 13);
            this.lnk3F.TabIndex = 14;
            this.lnk3F.TabStop = true;
            this.lnk3F.Text = "GitHub/3F";
            this.lnk3F.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnk3F_LinkClicked);
            // 
            // labelSrc
            // 
            this.labelSrc.AutoSize = true;
            this.labelSrc.Location = new System.Drawing.Point(21, 3);
            this.labelSrc.Name = "labelSrc";
            this.labelSrc.Size = new System.Drawing.Size(68, 13);
            this.labelSrc.TabIndex = 13;
            this.labelSrc.Text = "Open source";
            // 
            // txtBuildInfo
            // 
            this.txtBuildInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBuildInfo.BackColor = System.Drawing.SystemColors.Control;
            this.txtBuildInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBuildInfo.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBuildInfo.Location = new System.Drawing.Point(3, 41);
            this.txtBuildInfo.Multiline = true;
            this.txtBuildInfo.Name = "txtBuildInfo";
            this.txtBuildInfo.ReadOnly = true;
            this.txtBuildInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBuildInfo.Size = new System.Drawing.Size(436, 184);
            this.txtBuildInfo.TabIndex = 12;
            // 
            // ConfiguratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 370);
            this.Controls.Add(this.splitCon);
            this.Controls.Add(this.panelTop);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfiguratorForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DllExport";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ConfiguratorForm_Load);
            this.panelTop.ResumeLayout(false);
            this.splitCon.Panel1.ResumeLayout(false);
            this.splitCon.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitCon)).EndInit();
            this.splitCon.ResumeLayout(false);
            this.panelPrjs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilter)).EndInit();
            this.tabCtrl.ResumeLayout(false);
            this.tabCfgDxp.ResumeLayout(false);
            this.tabData.ResumeLayout(false);
            this.tabData.PerformLayout();
            this.tabUpdating.ResumeLayout(false);
            this.tabUpdating.PerformLayout();
            this.panelUpdVerTop.ResumeLayout(false);
            this.tabBuildInfo.ResumeLayout(false);
            this.tabBuildInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.ComboBox comboBoxSln;
        private System.Windows.Forms.ToolTip toolTipMain;
        private Controls.ProgressLineControl progressLine;
        private System.Windows.Forms.SplitContainer splitCon;
        private System.Windows.Forms.TabControl tabCtrl;
        private System.Windows.Forms.TabPage tabCfgDxp;
        private Controls.ProjectItemsControl projectItems;
        private System.Windows.Forms.TabPage tabBuildInfo;
        private vsSBE.UI.WForms.Components.DataGridViewExt dgvFilter;
        private System.Windows.Forms.Panel panelFilter;
        private System.Windows.Forms.Panel panelPrjs;
        private System.Windows.Forms.DataGridViewTextBoxColumn gcInstalled;
        private System.Windows.Forms.DataGridViewImageColumn gcType;
        private System.Windows.Forms.DataGridViewTextBoxColumn gcPath;
        private System.Windows.Forms.TextBox txtBuildInfo;
        private System.Windows.Forms.Label labelSrc;
        private System.Windows.Forms.LinkLabel lnk3F;
        private System.Windows.Forms.LinkLabel lnkSrc;
        private System.Windows.Forms.LinkLabel linkIlasm;
        private System.Windows.Forms.TabPage tabUpdating;
        private System.Windows.Forms.ComboBox cbPackages;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.TextBox txtLogUpd;
        private System.Windows.Forms.Panel panelUpdVerTop;
        private System.Windows.Forms.Button btnUpdListOfPkg;
        private System.Windows.Forms.Button btnToOnline;
        private System.Windows.Forms.TabPage tabData;
        private System.Windows.Forms.Label labelStorage;
        private System.Windows.Forms.ComboBox comboBoxStorage;
        private System.Windows.Forms.TextBox txtCfgData;
        private System.Windows.Forms.Label labelSrcMit;
    }
}
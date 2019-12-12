/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2019  Denis Kuzmin < entry.reg@gmail.com > GitHub/3F
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using net.r_eg.DllExport.Wizard.UI.Extensions;
using RGiesecke.DllExport;

namespace net.r_eg.DllExport.Wizard.UI.Controls
{
    internal sealed partial class ProjectItemControl: UserControl, IDisposable
    {
        public IProject Project
        {
            get;
            private set;
        }

        public bool Installed
        {
            get => chkInstalled.Checked;
            set => chkInstalled.Checked = value;
        }

        public string ProjectPath
        {
            get => textBoxProjectPath.Text;
            set {
                textBoxProjectPath.Text = value;
                toolTip.SetToolTip(textBoxProjectPath, textBoxProjectPath.Text);
            }
        }

        public string Identifier
        {
            get => textBoxIdent.Text;
            set => textBoxIdent.Text = value;
        }

        /// <summary>
        /// Function of the browse button.
        /// </summary>
        [Browsable(false)]
        public Action<string> Browse
        {
            get;
            set;
        }

        public ComboBox Namespaces
        {
            get => comboNS;
        }

        /// <summary>
        /// Function to validate namespace after update, or null if not used.
        /// </summary>
        [Browsable(false)]
        public Func<string, bool> NamespaceValidate
        {
            get;
            set;
        }

        public bool UseCecil
        {
            get => rbCecil.Checked;
            set
            {
                if(value) {
                    rbCecil.Checked = true;
                }
                else {
                    rbDirect.Checked = true;
                }
            }
        }

        public Platform Platform
        {
            get => GetPlatform();
            set => SetPlatform(value);
        }

        public CompilerCfg Compiler
        {
            get => new CompilerCfg()
            {
                ordinalsBase        = (int)numOrdinal.Value,
                genExpLib           = chkGenExpLib.Checked,
                ourILAsm            = chkOurILAsm.Checked,
                customILAsm         = chkCustomILAsm.Checked ? textBoxCustomILAsm.Text : null,
                rSysObj             = chkRebaseSysObj.Checked,
                intermediateFiles   = chkIntermediateFiles.Checked,
                timeout             = (int)numTimeout.Value,
                peCheck             = GetPeCheckType()
            };
            set
            {
                numOrdinal.Value        = value.ordinalsBase;
                chkGenExpLib.Checked    = value.genExpLib;
                chkOurILAsm.Checked     = value.ourILAsm;
                chkRebaseSysObj.Checked = value.rSysObj;
                numTimeout.Value        = value.timeout;

                if(String.IsNullOrWhiteSpace(value.customILAsm)) {
                    textBoxCustomILAsm.Text = CompilerCfg.PATH_CTM_ILASM;
                    chkCustomILAsm.Checked  = false;
                }
                else {
                    textBoxCustomILAsm.Text = value.customILAsm;
                    chkCustomILAsm.Checked  = true;
                }

                chkIntermediateFiles.Checked = value.intermediateFiles;
                SetPeCheckType(value.peCheck);
            }
        }

        /// <summary>
        /// Function to open url.
        /// </summary>
        [Browsable(false)]
        public Action<string> OpenUrl
        {
            get;
            set;
        }

        public int Order
        {
            get;
            set;
        }

        public void SetNamespace(string name, bool addIfNotExists = true)
        {
            name = name?.Trim() ?? String.Empty;

            if(addIfNotExists && !Namespaces.Items.Contains(name)) {
                Namespaces.Items.Add(name);
            }
            Namespaces.Text = name;
        }

        public bool LockIfError(string msg)
        {
            if(msg == null) {
                return false;
            }

            chkInstalled.Enabled    = false;
            textBoxIdent.Text       = "Project cannot be loaded:";
            textBoxProjectPath.Text = msg;
            return true;
        }

        public ProjectItemControl(IProject project)
        {
            Project = project ?? throw  new ArgumentNullException(nameof(project));

            InitializeComponent();

            textBoxIdent.BackColor = SystemColors.Control;
            textBoxIdent.ForeColor = Color.DimGray;

            InstalledStatus(false);
            UpdateRebaseChk();
        }

        private Platform GetPlatform()
        {
            if(rbPlatformX86.Checked) {
                return Platform.x86;
            }

            if(rbPlatformX64.Checked) {
                return Platform.x64;
            }

            if(rbPlatformAnyCPU.Checked) {
                return Platform.AnyCPU;
            }

            if(rbPlatformAuto.Checked) {
                return Platform.Auto;
            }

            return Platform.Default;
        }

        private void SetPlatform(Platform platform)
        {
            switch(platform)
            {
                case Platform.x86: {
                    rbPlatformX86.Checked = true;
                    return;
                }
                case Platform.x64: {
                    rbPlatformX64.Checked = true;
                    return;
                }
                case Platform.AnyCPU: {
                    rbPlatformAnyCPU.Checked = true;
                    return;
                }
                case Platform.Auto:
                case Platform.Default: {
                    rbPlatformAuto.Checked = true;
                    return;
                }
            }
        }

        private PeCheckType GetPeCheckType()
        {
            PeCheckType peCheck = PeCheckType.None;

            if(chkPECheck1to1.Checked) {
                peCheck |= PeCheckType.Pe1to1;
            }

            if(chkPECheckIl.Checked) {
                peCheck |= PeCheckType.PeIl;
            }

            return peCheck;
        }

        private void SetPeCheckType(PeCheckType type)
        {
            chkPECheck1to1.Checked  = (type & PeCheckType.Pe1to1) == PeCheckType.Pe1to1;
            chkPECheckIl.Checked    = (type & PeCheckType.PeIl) == PeCheckType.PeIl;
        }

        private void InstalledStatus(bool status)
        {
            if(status) {
                panelStatus.BackColor = Color.FromArgb(111, 145, 6);
            }
            else {
                panelStatus.BackColor = Color.FromArgb(168, 47, 17);
            }
        }

        private void UpdateRebaseChk() => chkRebaseSysObj.Enabled = chkOurILAsm.Checked || chkCustomILAsm.Checked;

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var path = Project.XProject?.ProjectItem.project.fullPath;
            if(!String.IsNullOrWhiteSpace(path)) {
                Browse?.Invoke(Path.GetDirectoryName(path));
            }
        }

        private void numOrdinal_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyValue >= '0' && e.KeyValue <= '9') {
                return;
            }

            switch(e.KeyCode)
            {
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                case Keys.Back:
                case Keys.Delete: {
                    return;
                }
            }

            e.SuppressKeyPress = true;
            e.Handled = true;
        }

        private void linkOrdinals_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => OpenUrl?.Invoke("https://github.com/3F/DllExport/issues/11#issuecomment-250907940");

        private void linkExpLib_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => OpenUrl?.Invoke("https://github.com/3F/DllExport/issues/9#issuecomment-246189220");

        private void linkOurILAsm_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => OpenUrl?.Invoke("https://github.com/3F/DllExport/issues/17");

        private void linkSysObjRebase_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => OpenUrl?.Invoke("https://github.com/3F/DllExport/issues/125#issuecomment-561245575");

        private void comboNS_TextUpdate(object sender, EventArgs e)
        {
            if(NamespaceValidate == null) {
                return;
            }

            if(!NamespaceValidate(comboNS.Text)) {
                panelNScombo.BackColor = Color.FromArgb(234, 0, 0);
            }
            else {
                panelNScombo.BackColor = Color.FromArgb(92, 158, 207);
            }
        }

        private void chkInstalled_CheckedChanged(object sender, EventArgs e)
        {
            InstalledStatus(chkInstalled.Checked);
        }

        private void chkCustomILAsm_CheckedChanged(object sender, EventArgs e)
        {
            if(chkCustomILAsm.Checked) {
                chkOurILAsm.Checked = false;
                textBoxCustomILAsm.ForeColor = SystemColors.WindowText;
            }
            else {
                textBoxCustomILAsm.ForeColor = Color.DarkGray;
            }

            UpdateRebaseChk();
        }

        private void chkOurILAsm_CheckedChanged(object sender, EventArgs e)
        {
            if(chkOurILAsm.Checked) {
                chkCustomILAsm.Checked = false;
            }

            UpdateRebaseChk();
        }

        private void menuItemLimitPKT_Click(object sender, EventArgs e)
        {
            menuItemLimitPKT.Checked    = Project.PublicKeyTokenLimit
                                        = !menuItemLimitPKT.Checked;

            chkInstalled.ForeColor = (Project.PublicKeyTokenLimit) ?
                                        SystemColors.ControlText : Color.FromArgb(43, 145, 175);
        }

        #region disposing

        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null)) {
                components.Dispose();
            }

            Controls.Dispose();

            base.Dispose(disposing);
        }

        #endregion
    }
}

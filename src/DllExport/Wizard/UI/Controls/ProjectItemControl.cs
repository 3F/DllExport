﻿/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using net.r_eg.DllExport.Wizard.UI.Extensions;

namespace net.r_eg.DllExport.Wizard.UI.Controls
{
    internal sealed partial class ProjectItemControl: UserControl, IDisposable
    {
        public IProject Project { get; private set; }

        public bool Installed
        {
            get => chkInstalled.Checked;
            set => chkInstalled.Checked = value;
        }

        public string ProjectPath
        {
            get => textBoxProjectPath.Text;
            set
            {
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
        public Action<string> Browse { get; set; }

        public ComboBox Namespaces => comboNS;

        /// <summary>
        /// Function to validate namespace after update, or null if not used.
        /// </summary>
        [Browsable(false)]
        public Func<string, bool> NamespaceValidate { get; set; }

        public bool UseCecil
        {
            get => rbCecil.Checked;
            set
            {
                if(value)
                {
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
            get => new()
            {
                imageBase           = txtImageBase.Text.Trim(),
                imageBaseStep       = txtImageBaseStep.Text.Trim(),
                ordinalsBase        = (int)numOrdinal.Value,
                genExpLib           = chkGenExpLib.Checked,
                ourILAsm            = chkOurILAsm.Checked,
                customILAsm         = chkCustomILAsm.Checked ? txtCustomILAsm.Text : null,
                rSysObj             = chkRebaseSysObj.Checked,
                intermediateFiles   = chkIntermediateFiles.Checked,
                timeout             = (int)numTimeout.Value,
                peCheck             = GetPeCheckType(),
                patches             = GetPatchesType(),
                refreshObj          = chkRefreshObj.Checked,
            };

            set
            {
                txtImageBase.Text       = value.imageBase;
                txtImageBaseStep.Text   = value.imageBaseStep;
                numOrdinal.Value        = value.ordinalsBase;
                chkGenExpLib.Checked    = value.genExpLib;
                chkOurILAsm.Checked     = value.ourILAsm;
                chkRebaseSysObj.Checked = value.rSysObj;
                numTimeout.Value        = value.timeout;

                if(string.IsNullOrWhiteSpace(value.customILAsm))
                {
                    txtCustomILAsm.Text = CompilerCfg.PATH_CTM_ILASM;
                    chkCustomILAsm.Checked  = false;
                }
                else
                {
                    txtCustomILAsm.Text = value.customILAsm;
                    chkCustomILAsm.Checked  = true;
                }

                chkIntermediateFiles.Checked = value.intermediateFiles;
                SetPeCheckType(value.peCheck);
                SetPatchesType(value.patches);
                chkRefreshObj.Checked = value.refreshObj;
            }
        }

        /// <summary>
        /// Function to open url.
        /// </summary>
        [Browsable(false)]
        public Action<string> OpenUrl { get; set; }

        public int Order { get; set; }

        public void SetNamespace(string name, bool addIfNotExists = true)
        {
            name = name?.Trim() ?? string.Empty;

            if(addIfNotExists && !Namespaces.Items.Contains(name)) {
                Namespaces.Items.Add(name);
            }
            Namespaces.Text = name;
        }

        public bool LockIfError(string msg)
        {
            if(msg == null) return false;

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

            if(chkPECheck1to1.Checked) peCheck |= PeCheckType.Pe1to1;
            if(chkPECheckIl.Checked) peCheck |= PeCheckType.PeIl;
            if(chkPECheck32.Checked) peCheck |= PeCheckType.Pe32orPlus;

            return peCheck;
        }

        private void SetPeCheckType(PeCheckType type)
        {
            chkPECheck1to1.Checked = (type & PeCheckType.Pe1to1) == PeCheckType.Pe1to1;
            chkPECheckIl.Checked = (type & PeCheckType.PeIl) == PeCheckType.PeIl;
            chkPECheck32.Checked = (type & PeCheckType.Pe32orPlus) == PeCheckType.Pe32orPlus;
        }

        private PatchesType GetPatchesType()
        {
            PatchesType patches = PatchesType.None;

            if(chkInfPatching.Checked) { patches |= PatchesType.InfToken; }
            if(chkNaNPatching.Checked) { patches |= PatchesType.NaNToken; }

            return patches;
        }

        private void SetPatchesType(PatchesType type)
        {
            chkInfPatching.Checked = (type & PatchesType.InfToken) == PatchesType.InfToken;
            chkNaNPatching.Checked = (type & PatchesType.NaNToken) == PatchesType.NaNToken;
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
            string path = Project.XProject?.ProjectItem.project.fullPath;
            if(!string.IsNullOrWhiteSpace(path))
            {
                Browse?.Invoke(Path.GetDirectoryName(path));
            }
        }

        private void numOrdinal_KeyDown(object sender, KeyEventArgs e) => e.AllowKeysOnly(x => x.IsDigitsOrControl());

        private void txtImageBase_KeyDown(object sender, KeyEventArgs e) => e.AllowKeysOnly(x => x.IsHexDigitsOrControl());

        private void txtImageBaseStep_KeyDown(object sender, KeyEventArgs e) => e.AllowKeysOnly(x => x.IsHexDigitsOrControl());

        private void linkOrdinals_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => OpenUrl?.Invoke("https://github.com/3F/DllExport/issues/11#issuecomment-250907940");

        private void linkExpLib_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => OpenUrl?.Invoke("https://github.com/3F/DllExport/issues/9#issuecomment-246189220");

        private void linkOurILAsm_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => OpenUrl?.Invoke("https://github.com/3F/DllExport/issues/17");

        private void linkSysObjRebase_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => OpenUrl?.Invoke("https://github.com/3F/DllExport/issues/125#issuecomment-561245575");

        private void LinkInfPatching_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => OpenUrl?.Invoke("https://github.com/3F/DllExport/issues/128");

        private void LinkPe1to1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => OpenUrl?.Invoke("https://github.com/3F/DllExport/issues/55");

        private void linkPeIl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => OpenUrl?.Invoke("https://github.com/3F/DllExport/issues/59");

        private void linkRefreshObj_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => OpenUrl?.Invoke("https://github.com/3F/DllExport/issues/206#issuecomment-2517253564");

        private void comboNS_TextUpdate(object sender, EventArgs e)
        {
            if(NamespaceValidate == null) return;

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
                txtCustomILAsm.ForeColor = SystemColors.WindowText;
            }
            else {
                txtCustomILAsm.ForeColor = Color.DarkGray;
            }

            UpdateRebaseChk();
        }

        private void chkOurILAsm_CheckedChanged(object sender, EventArgs e)
        {
            if(chkOurILAsm.Checked) chkCustomILAsm.Checked = false;

            UpdateRebaseChk();
        }

        private void rbPlatformAnyCPU_CheckedChanged(object sender, EventArgs e)
        {
            chkRefreshObj.Enabled = !rbPlatformAnyCPU.Checked;
            if(rbPlatformAnyCPU.Checked) chkRefreshObj.Checked = false;
        }

        private void menuItemLimitPKT_Click(object sender, EventArgs e)
        {
            menuItemLimitPKT.Checked = Project.PublicKeyTokenLimit
                                     = !menuItemLimitPKT.Checked;

            chkInstalled.ForeColor = Project.PublicKeyTokenLimit ?
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

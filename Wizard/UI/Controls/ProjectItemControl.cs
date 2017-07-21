/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2017  Denis Kuzmin < entry.reg@gmail.com > :: github.com/3F
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
using System.IO;
using System.Windows.Forms;

namespace net.r_eg.DllExport.Wizard.UI.Controls
{
    internal sealed partial class ProjectItemControl: UserControl
    {
        public bool Installed
        {
            get => chkInstalled.Checked;
            set => chkInstalled.Checked = value;
        }

        public string ProjectPath
        {
            get => gbProject.Text;
            set {
                gbProject.Text = value;
                toolTip.SetToolTip(gbProject, gbProject.Text);
            }
        }

        public string ProjectGuid
        {
            get => textBoxProjectGuid.Text;
            set => textBoxProjectGuid.Text = value;
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
            set => comboNS = value;
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
            set {
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
                ordinalsBase    = (int)numOrdinal.Value,
                genExpLib       = chkGenExpLib.Checked,
                ourILAsm        = chkOurILAsm.Checked
            };

            set {
                numOrdinal.Value        = value.ordinalsBase;
                chkGenExpLib.Checked    = value.genExpLib;
                chkOurILAsm.Checked     = value.ourILAsm;
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

        public ProjectItemControl()
        {
            InitializeComponent();
            InstalledStatus(false);
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
                case Platform.AnyCPU:
                case Platform.Default: {
                    rbPlatformAnyCPU.Checked = true;
                    return;
                }
            }
        }

        private void InstalledStatus(bool status)
        {
            if(status) {
                panelStatus.BackColor = System.Drawing.Color.FromArgb(111, 145, 6);
            }
            else {
                panelStatus.BackColor = System.Drawing.Color.FromArgb(168, 47, 17);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            Browse?.Invoke(Path.GetDirectoryName(ProjectPath));
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

        private void linkDDNS_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl?.Invoke("https://github.com/3F/DllExport/issues/2");
        }

        private void linkOrdinals_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl?.Invoke("https://github.com/3F/DllExport/issues/11#issuecomment-250907940");
        }

        private void linkExpLib_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl?.Invoke("https://github.com/3F/DllExport/issues/9#issuecomment-246189220");
        }

        private void linkOurILAsm_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl?.Invoke("https://github.com/3F/DllExport/issues/17");
        }

        private void comboNS_TextUpdate(object sender, EventArgs e)
        {
            if(NamespaceValidate == null) {
                return;
            }

            if(!NamespaceValidate(comboNS.Text)) {
                panelNScombo.BackColor = System.Drawing.Color.FromArgb(234, 0, 0);
            }
            else {
                panelNScombo.BackColor = System.Drawing.Color.FromArgb(92, 158, 207);
            }
        }

        private void chkInstalled_CheckedChanged(object sender, EventArgs e)
        {
            InstalledStatus(chkInstalled.Checked);
        }
    }
}

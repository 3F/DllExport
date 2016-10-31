/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016  Denis Kuzmin <entry.reg@gmail.com>
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
using System.Windows.Forms;
using net.r_eg.DllExport.Configurator.Dynamic;
using net.r_eg.DllExport.NSBin;

namespace net.r_eg.DllExport.Configurator.GUI
{
    using Platform = UserConfig.PlatformTarget;

    internal partial class InstallationForm: Form
    {
        const int FIRST_ORDINAL = 1;

        private UserConfig config;
        private IProject project;

        public InstallationForm(UserConfig cfg, IProject prj)
        {
            if(cfg == null) {
                throw new ArgumentNullException("UserConfig cannot be null.");
            }
            config  = cfg;
            project = prj;

            InitializeComponent();
            comboNS.MaxLength   = config.nsBuffer;
            numOrdinal.Value    = FIRST_ORDINAL;

#if CONFVER_EXISTS
            Text += " - v" + ConfVersion.S_INFO;
#endif

            foreach(var ns in cfg.defnamespaces) {
                comboNS.Items.Add(ns);
            }
            comboNS.SelectedIndex = 0;
        }

        private void saveConfig()
        {
            config.unamespace               = getValidNS(comboNS.Text);
            config.compiler.ordinalsBase    = (int)numOrdinal.Value;
            config.compiler.genExpLib       = chkGenExpLib.Checked;
            config.compiler.ourILAsm        = chkOurILAsm.Checked;
            config.useCecil                 = rbCecil.Checked;

            if(rbPlatformX86.Checked) {
                config.platform = Platform.x86;
            }
            else if(rbPlatformX64.Checked) {
                config.platform = Platform.x64;
            }
            else if(rbPlatformAnyCPU.Checked) {
                config.platform = Platform.AnyCPU;
            }
            else {
                config.platform = Platform.Default;
            }
        }

        private void apply(bool byDefault = false)
        {
            if(byDefault) {
                comboNS.Text = UserConfig.NS_DEFAULT_VALUE;
            }
            saveConfig();
        }

        private string getValidNS(string name)
        {
            if(String.IsNullOrWhiteSpace(name)) {
                return UserConfig.NS_DEFAULT_VALUE;
            }
            return name;
        }

        private void prefilter()
        {
            comboNS.Text = comboNS.Text.Replace(" ", "");
        }

        private void openUrl(string url)
        {
            System.Diagnostics.Process.Start(url);
        }

        private void btnConfigure_Click(object sender, EventArgs e)
        {
            prefilter();

            if(!DDNS.IsValidNS(comboNS.Text)) {
                MessageBox.Show("Your namespace is not correct.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            apply();

            FormClosed -= InstallationForm_FormClosed;
            Close();
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

        private void InstallationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            apply(true);
        }

        private void linkDDNS_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openUrl("https://github.com/3F/DllExport/issues/2");
        }

        private void comboNS_TextUpdate(object sender, EventArgs e)
        {
            if(!DDNS.IsValidNS(comboNS.Text)) {
                panelNScombo.BackColor = System.Drawing.Color.FromArgb(234, 0, 0);
            }
            else {
                panelNScombo.BackColor = System.Drawing.Color.FromArgb(92, 158, 207);
            }
        }

        private void numOrdinal_ValueChanged(object sender, EventArgs e)
        {
            //labelOrdinals.Text = String.Format(ResText.Settings_Compiler_Ordinals_Label, numOrdinal.Value);
        }

        private void InstallationForm_Load(object sender, EventArgs e)
        {
            labelActiveCfg.Text += $"' {project.getPropertyValue("Configuration")} | {project.getPropertyValue("Platform")} '";
        }

        private void linkOrdinals_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openUrl("https://github.com/3F/DllExport/issues/11#issuecomment-250907940");
        }

        private void linkExpLib_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openUrl("https://github.com/3F/DllExport/issues/9#issuecomment-246189220");
        }

        private void linkOurILAsm_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openUrl("https://github.com/3F/DllExport/issues/17");
        }
    }
}

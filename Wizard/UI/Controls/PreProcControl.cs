/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
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
using System.Drawing;
using System.Windows.Forms;
using net.r_eg.DllExport.Wizard.Extensions;
using static net.r_eg.DllExport.Wizard.PreProc;

namespace net.r_eg.DllExport.Wizard.UI.Controls
{
    internal partial class PreProcControl: UserControl
    {
        public CmdType Type => GetCmdType();

        public string Cmd => txtPreProc.Text;

        public void Render(PreProc instance)
        {
            if(instance == null) {
                throw new ArgumentNullException(nameof(instance));
            }

            SetCmdType(instance.Type);
            txtPreProc.Text = instance.Cmd;
        }

        public PreProc Export(PreProc obj) 
            => (obj ?? throw new ArgumentNullException(nameof(obj))).Configure(Type, Cmd);

        public PreProcControl() => InitializeComponent();

        private void SetCmdType(CmdType type)
        {
            if(type == CmdType.None)
            {
                radioPreProcDisabled.Checked = true; 
                return;
            }

            if((type & CmdType.ILMerge) == CmdType.ILMerge) { radioILMerge.Checked = true; }
            if((type & CmdType.Exec) == CmdType.Exec)       { radioRawExec.Checked = true; }

            chkMergeConari.Checked  = ((type & CmdType.Conari) == CmdType.Conari);
            chkIgnoreErrors.Checked = ((type & CmdType.IgnoreErr) == CmdType.IgnoreErr);
            chkGenDebugInfo.Checked = ((type & CmdType.DebugInfo) == CmdType.DebugInfo);
        }

        private CmdType GetCmdType()
        {
            CmdType ret = CmdType.None;

            if(radioILMerge.Checked) { ret = CmdType.ILMerge; }
            if(radioRawExec.Checked) { ret = CmdType.Exec; }

            if(chkMergeConari.Checked)  { ret |= CmdType.Conari; }
            if(chkIgnoreErrors.Checked) { ret |= CmdType.IgnoreErr; }
            if(chkGenDebugInfo.Checked) { ret |= CmdType.DebugInfo; }

            return ret;
        }

        private void SetUIMergeConari(bool disabled)
        {
            if(disabled) chkMergeConari.Checked = false;
        }

        private bool SetUIPreProcCmd(bool disabled)
        {
            txtPreProc.Enabled      = !disabled;
            txtPreProc.BackColor    = (disabled) ? SystemColors.Control : SystemColors.Window;

            chkIgnoreErrors.Checked = chkIgnoreErrors.Enabled
                                    = !disabled;
            return disabled;
        }

        private void ChkMergeConari_CheckedChanged(object sender, EventArgs e)
        {
            if(chkMergeConari.Checked) radioILMerge.Checked = true;
        }

        private void RadioPreProcDisabled_CheckedChanged(object sender, EventArgs e) => SetUIMergeConari(SetUIPreProcCmd(radioPreProcDisabled.Checked));
        private void RadioRawExec_CheckedChanged(object sender, EventArgs e) => SetUIMergeConari(radioRawExec.Checked);
        private void LinkAboutConari_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => "https://github.com/3F/DllExport/wiki/Quick-start#when-conari-can-help-you".OpenUrl();
        private void LinkPreProc_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => "https://github.com/3F/DllExport/issues/40".OpenUrl();
        private void RadioILMerge_CheckedChanged(object sender, EventArgs e) => chkGenDebugInfo.Checked = chkGenDebugInfo.Enabled = radioILMerge.Checked;
    }
}

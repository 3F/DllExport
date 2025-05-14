/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
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
            if(instance == null) throw new ArgumentNullException(nameof(instance));

            SetCmdType(instance.Type);
            txtPreProc.Text = instance.Cmd;
        }

        public PreProc Export(PreProc obj) 
            => (obj ?? throw new ArgumentNullException(nameof(obj))).Configure(Type, Cmd);

        public PreProcControl() => InitializeComponent();

        private void SetCmdType(CmdType type)
        {
            if(type == CmdType.None) { radioPreProcDisabled.Checked = true; return; }

            if((type & CmdType.ILMerge) == CmdType.ILMerge)     radioILMerge.Checked = true;
            if((type & CmdType.ILRepack) == CmdType.ILRepack)   radioILRepack.Checked = true;
            if((type & CmdType.Exec) == CmdType.Exec)           radioRawExec.Checked = true;

            chkMergeConari.Checked  = (type & CmdType.Conari) == CmdType.Conari;
            chkMergeRef.Checked     = (type & CmdType.MergeRefPkg) == CmdType.MergeRefPkg;
            chkIgnoreErrors.Checked = (type & CmdType.IgnoreErr) == CmdType.IgnoreErr;
            chkGenDebugInfo.Checked = (type & CmdType.DebugInfo) == CmdType.DebugInfo;
            chkLog.Checked          = (type & CmdType.Log) == CmdType.Log;
        }

        private CmdType GetCmdType()
        {
            CmdType ret = CmdType.None;

            if(radioILMerge.Checked)    ret = CmdType.ILMerge;
            if(radioILRepack.Checked)   ret = CmdType.ILRepack;
            if(radioRawExec.Checked)    ret = CmdType.Exec;

            if(chkMergeConari.Checked)  ret |= CmdType.Conari;
            if(chkMergeRef.Checked)     ret |= CmdType.MergeRefPkg;
            if(chkIgnoreErrors.Checked) ret |= CmdType.IgnoreErr;
            if(chkGenDebugInfo.Checked) ret |= CmdType.DebugInfo;
            if(chkLog.Checked)          ret |= CmdType.Log;

            return ret;
        }

        private void SetUIMergeDep(bool disabled)
        {
            if(disabled) chkMergeRef.Checked = chkMergeConari.Checked = false;
        }

        private bool SetUIPreProcCmd(bool disabled)
        {
            txtPreProc.Enabled      = !disabled;
            txtPreProc.BackColor    = disabled ? SystemColors.Control : SystemColors.Window;

            chkIgnoreErrors.Checked = chkIgnoreErrors.Enabled
                                    = !disabled;
            return disabled;
        }

        private void ChkMergeConari_CheckedChanged(object sender, EventArgs e)
        {
            if(!chkMergeConari.Checked || radioILMerge.Checked || radioILRepack.Checked) return;
            radioILMerge.Checked = true; // use ILMerge by default for Conari because ILMerge is about 2+ times faster than ILRepack -_-
        }

        private void ChkMergeRef_CheckedChanged(object sender, EventArgs e)
        {
            if(!chkMergeRef.Checked || radioILMerge.Checked || radioILRepack.Checked) return;
            radioILRepack.Checked = true; // use ILRepack by default in order to get a common modern support for all user [Ref] assemblies
        }

        private void RadioPreProcDisabled_CheckedChanged(object sender, EventArgs e) => SetUIMergeDep(SetUIPreProcCmd(radioPreProcDisabled.Checked));

        private void RadioRawExec_CheckedChanged(object sender, EventArgs e)
        {
            SetUIMergeDep(radioRawExec.Checked);
            txtPreProc.BackgroundCaption = txtPreProc.Enabled ? "... $(TargetName).dll ..." : string.Empty;
            txtPreProc.Refresh();
        }

        private void LinkAboutConari_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => "https://github.com/3F/DllExport/wiki/Quick-start#when-conari-can-help-you".OpenUrl();

        private void LinkPreProc_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => "https://github.com/3F/DllExport/issues/40".OpenUrl();

        private void radioILRepack_CheckedChanged(object sender, EventArgs e) => RadioILMerge_CheckedChanged(sender, e);

        private void RadioILMerge_CheckedChanged(object sender, EventArgs e)
        {
            chkLog.Enabled = chkGenDebugInfo.Checked = chkGenDebugInfo.Enabled = radioILMerge.Checked || radioILRepack.Checked;
            if(!radioILMerge.Checked || !radioILRepack.Checked) chkLog.Checked = false;

            txtPreProc.BackgroundCaption = chkLog.Enabled ? "Module1 Module2 /arg1 ..." : string.Empty;
            txtPreProc.Refresh();
        }
    }
}

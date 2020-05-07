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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using net.r_eg.DllExport.Wizard.Extensions;
using net.r_eg.MvsSln.Core;
using net.r_eg.MvsSln.Extensions;
using static net.r_eg.DllExport.Wizard.PostProc;

namespace net.r_eg.DllExport.Wizard.UI.Controls
{
    public partial class PostProcControl: UserControl
    {
        public CmdType Type => GetCmdType();

        public IEnumerable<string> ProcEnv => GetProcEnv();

        //TODO: implement logic
        //public string Cmd => txtPostProc.Text;
        public string Cmd => string.Empty;

        public void Render(PostProc instance)
        {
            if(instance == null) {
                throw new ArgumentNullException(nameof(instance));
            }

            SetCmdType(instance.Type);
            SetProcEnv(instance.ProcEnv);

            //TODO: implement logic
            //txtPostProc.Text = instance.Cmd;
            txtPostProc.Text = $"Some Post-Proc features are not yet available in GUI for v{DllExportVersion.S_INFO}. But you can already configure it with msbuild: https://github.com/3F/DllExport/pull/148 \r\n* Follow the news or open PR to https://github.com/3F/DllExport";
        }

        public PostProc Export(PostProc obj) => (obj ?? throw new ArgumentNullException(nameof(obj)))
                .Configure(ProcEnv, Type, Cmd);

        //TODO: select a specific project + add property to the list
        internal void LoadProperties(IXProject project)
        {
            dgvProperties.Rows.Clear();
            project?.GetProperties().ForEach(p => dgvProperties.Rows.Add(p.name, p.evaluatedValue));
        }

        public PostProcControl() => InitializeComponent();

        private void SetCmdType(CmdType type)
        {
            if(type == CmdType.None)
            {
                radioPostProcDisabled.Checked = true;
                return;
            }

            if((type & CmdType.Custom) == CmdType.Custom) { radioCustom.Checked = true; }
            if((type & CmdType.Predefined) == CmdType.Predefined) { radioDependentProjects.Checked = true; }

            chkX86X64.Checked               = ((type & CmdType.DependentX86X64) == CmdType.DependentX86X64);
            chkIntermediateFiles.Checked    = ((type & CmdType.DependentIntermediateFiles) == CmdType.DependentIntermediateFiles);
            chkSeqDep.Checked               = ((type & CmdType.SeqDependentForSys) == CmdType.SeqDependentForSys);
        }

        private CmdType GetCmdType()
        {
            CmdType ret = CmdType.None;

            if(radioCustom.Checked)             { ret = CmdType.Custom; }
            if(radioDependentProjects.Checked)  { ret = CmdType.Predefined; }

            if(chkX86X64.Checked)               { ret |= CmdType.DependentX86X64; }
            if(chkIntermediateFiles.Checked)    { ret |= CmdType.DependentIntermediateFiles; }
            if(chkSeqDep.Checked)               { ret |= CmdType.SeqDependentForSys; }

            return ret;
        }

        private void SetProcEnv(IEnumerable<string> env)
        {
            listActivatedProperties.Items.Clear();
            listActivatedProperties.Items.AddRange(env.Skip(RGiesecke.DllExport.MSBuild.PostProc.OFS_ENV_PROP).ToArray());
        }

        private IEnumerable<string> GetProcEnv()
        {
            foreach(var item in listActivatedProperties.Items) yield return $"{item}";
        }

        private void SetUIDependentProjects(bool activated)
        {
            chkX86X64.Enabled = chkIntermediateFiles.Enabled = chkSeqDep.Enabled = activated;

            if(!activated) chkX86X64.Checked = chkIntermediateFiles.Checked = false;
        }

        private void radioDependentProjects_CheckedChanged(object sender, EventArgs e) => SetUIDependentProjects(radioDependentProjects.Checked);

        private void linkAboutVsSBE_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => "https://github.com/3F/DllExport/issues/144#issuecomment-609494726".OpenUrl();
        private void linkAboutPostProc_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => "https://github.com/3F/DllExport/pull/148#issuecomment-622115091".OpenUrl();
        private void linkAboutDependent_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => "https://github.com/3F/DllExport/issues/144#issue-594663447".OpenUrl();
    }
}

/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2018  Denis Kuzmin < entry.reg@gmail.com > :: github.com/3F
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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using net.r_eg.DllExport.Wizard.Extensions;
using net.r_eg.DllExport.Wizard.UI.Extensions;
using net.r_eg.MvsSln.Log;

namespace net.r_eg.DllExport.Wizard.UI.Kit
{
    internal partial class FilterLineControl: UserControl, IExtCfg
    {
        protected IExecutor exec;
        protected IRender render;

        private DateTime prevPress;
        private bool block = false;

        /// <summary>
        /// To get new project list after applying filter.
        /// TODO: ProjectFilter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="projects"></param>
        /// <returns>Filtered project list.</returns>
        public IEnumerable<IProject> FilterProjects(ProjectFilter filter, IEnumerable<IProject> projects)
        {
            if(String.IsNullOrWhiteSpace(filter.path)) {
                return projects;
            }

            return projects?.Where(p => p.ProjectPath.IndexOf(filter.path, StringComparison.OrdinalIgnoreCase) != -1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        public void ApplyDelayedFilter(int timeout = 700)
        {
            prevPress = DateTime.Now;
            render.ShowProgressLine(true);

            Task.Factory.StartNew(() => 
            {
                if(block) {
                    return;
                }
                block = true;

                int actualdelay = timeout;

                double delta = 0;
                do
                {
                    Thread.Sleep(actualdelay);

                    delta = (DateTime.Now - prevPress).TotalMilliseconds;
                    actualdelay = timeout - (int)(ushort)delta;
                }
                while(actualdelay > 0);

                try
                {
                    this.UIAction(() => {
                        render.ShowProgressLine(false);
                        ApplyFilter();
                    });
                }
                catch(Exception ex) {
                    LSender.Send(this, $"Failed when applying filter: {ex.ToString()}");
                }
                
                block = false;
            });
        }

        /// <summary>
        /// To apply filter via used render.
        /// TODO: ProjectFilter
        /// </summary>
        public void ApplyFilter()
        {
            render.ApplyFilter(new ProjectFilter {
                path = textBoxFilter.Text.Trim()
            });
        }

        public FilterLineControl(IRender render, IExecutor exec)
        {
            this.render = render ?? throw new ArgumentNullException(nameof(render));
            this.exec   = exec ?? throw new ArgumentNullException(nameof(exec));

            InitializeComponent();
        }

        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null)) {
                components.Dispose();
            }

            Controls.Dispose();
            base.Dispose(disposing);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            ApplyDelayedFilter();
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();

#if !PUBLIC_RELEASE
            sb.Append($"The base: ");
#endif
            sb.Append($"{WizardVersion.S_NUM_REV} {WizardVersion.S_REL} [{WizardVersion.BRANCH_SHA1}]");
#if DEBUG
            sb.Append("[ Debug ] ");
#else
            sb.Append("[ Release ] ");
#endif
            sb.AppendLine();
            sb.AppendLine();

            sb.Append("https://github.com/3F/DllExport");
            sb.AppendLine();

            var info = Path.Combine(exec.Config.PkgPath, "build_info.txt");
            if(!File.Exists(info)) {
                sb.Append("Detailed information about build was not found. :(");
            }
            else {
                File.ReadAllLines(info).ForEach(s => 
                {
                    sb.Append(Regex.Replace(s, @":(\s\s*)(?!generated)", (Match m) => $": {m.Groups[1].Value.Replace(' ', '_')} "));
                    sb.AppendLine();
                });
            }

            MessageBox.Show(sb.ToString(), ".NET DllExport");
        }

        private void btnBug_Click(object sender, EventArgs e)
        {
            "https://github.com/3F/DllExport/issues".OpenUrl();
        }
    }
}

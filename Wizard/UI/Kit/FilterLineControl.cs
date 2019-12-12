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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        public string FilterText
        {
            get => textBoxFilter.Text;
            set => textBoxFilter.Text = value;
        }

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

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            ApplyDelayedFilter(550);
        }
    }
}

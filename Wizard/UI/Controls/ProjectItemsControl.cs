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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace net.r_eg.DllExport.Wizard.UI.Controls
{
    internal sealed partial class ProjectItemsControl: UserControl
    {
        private List<UProject> items = new List<UProject>();

        /// <summary>
        /// When the size of rendered items has been changed.
        /// </summary>
        public event EventHandler RenderedItemsSizeChanged = delegate(object sender, EventArgs e) { };

        private struct UProject
        {
            public ProjectItemControl control;
            public IProject project;
        }

        /// <summary>
        /// Access to data via IProject.
        /// </summary>
        public IEnumerable<IProject> Data
        {
            get => items.Select(i => ConfigureProject(i.project, i.control));
        }

        /// <summary>
        /// Count of added items.
        /// </summary>
        public int Count
        {
            get => items.Count;
        }

        public int MaxItemHeight
        {
            get => GetMaxItemsHeight(1);
        }

        public int MaxItemsHeight
        {
            get => GetMaxItemsHeight(Count);
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

        /// <summary>
        /// Function to validate namespace after update, or null if not used.
        /// </summary>
        [Browsable(false)]
        public Func<string, bool> NamespaceValidate
        {
            get;
            set;
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

        /// <summary>
        /// To add new item that's configured by IProject.
        /// </summary>
        /// <param name="project"></param>
        public void Add(IProject project)
        {
            var control = new ProjectItemControl(project) {
                Order = items.Count
            };
            control.Top = MaxItemsHeight;

            control.SizeChanged += ControlSizeChanged;
            ConfigureControl(control, project);

            panelMain.Controls.Add(control);
            items.Add(new UProject() {
                control = control,
                project = project
            });
        }

        /// <summary>
        /// Reset items.
        /// </summary>
        public void Reset()
        {
            items.Clear();
            panelMain.Controls.Clear();
        }

        public int GetMaxItemsHeight(int count)
        {
            return (Count < 1 || items.Count < 1) ? 
                        0 : items.OrderByDescending(i => i.control.Height)
                                    .Take(count)
                                    .Sum(i => i.control.Height);
        }

        public ProjectItemsControl()
        {
            InitializeComponent();
        }

        private void ConfigureControl(ProjectItemControl control, IProject project)
        {
            control.Installed       = project.Installed;
            control.ProjectPath     = project.ProjectPath;
            control.Identifier      = project.DxpIdent;
            control.UseCecil        = project.Config.UseCecil;
            control.Platform        = project.Config.Platform;
            control.Compiler        = project.Config.Compiler;

            control.Browse  = Browse;
            control.OpenUrl = OpenUrl;

            if(control.LockIfError(project.InternalError)) {
                return;
            }

            control.NamespaceValidate = NamespaceValidate;

            control.Namespaces.Items.Clear();
            control.Namespaces.Items.AddRange(project.Config.Namespaces.ToArray());
            control.Namespaces.SelectedIndex = 0;
            control.Namespaces.MaxLength = project.Config.NSBuffer;

            if(control.Installed) {
                control.SetNamespace(project.Config.Namespace, true);
            }
        }

        private IProject ConfigureProject(IProject project, ProjectItemControl control)
        {
            project.Config.Install      = control.Installed;
            project.Config.UseCecil     = control.UseCecil;
            project.Config.Platform     = control.Platform;
            project.Config.Compiler     = control.Compiler;
            project.Config.Namespace    = control.Namespaces.Text.Trim();

            return project;
        }

        private void ControlSizeChanged(object sender, EventArgs e)
        {
            if(sender is ProjectItemControl control)
            {
                int xprev = control.Top + control.Height;
                foreach(var item in items.Skip(control.Order + 1)) {
                    item.control.Top = xprev;
                    xprev += item.control.Height;
                }
            }
            RenderedItemsSizeChanged(this, EventArgs.Empty);
        }
    }
}

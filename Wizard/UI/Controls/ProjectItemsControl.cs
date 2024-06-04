/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using net.r_eg.DllExport.Wizard.UI.Extensions;
using net.r_eg.MvsSln.Core;
using net.r_eg.MvsSln.Extensions;

namespace net.r_eg.DllExport.Wizard.UI.Controls
{
    internal sealed partial class ProjectItemsControl: UserControl, IDisposable
    {
        private readonly List<UProject> items = new List<UProject>();

        private readonly Lazy<ProjectItemControl> empty;

        private sealed class UProject
        {
            public ProjectItemControl control;
            public IProject project;

            /// <summary>
            /// false when item should not be rendered (GUI).
            /// </summary>
            public bool rendered = true;
        }

        /// <summary>
        /// Access to data via IProject.
        /// Includes non-rendered items.
        /// </summary>
        public IEnumerable<IProject> Data
            => items.Select(i => ConfigureProject(i.project, i.control));

        /// <summary>
        /// Count of rendered items.
        /// </summary>
        public int CountRendered => RenderedItems.Count();

        /// <summary>
        /// Function of the browse button.
        /// </summary>
        [Browsable(false)]
        public Action<string> Browse { get; set; }

        /// <summary>
        /// Function to validate namespace after update, or null if not used.
        /// </summary>
        [Browsable(false)]
        public Func<string, bool> NamespaceValidate { get; set; }

        /// <summary>
        /// Function to open url.
        /// </summary>
        [Browsable(false)]
        public Action<string> OpenUrl { get; set; }

        private IEnumerable<UProject> RenderedItems => items.Where(i => i.rendered);

        /// <summary>
        /// Sets a single IProject for panel.
        /// </summary>
        /// <param name="project"></param>
        public void Set(IProject project)
        {
            Reset(false);
            if(project == null)
            {
                UseStub();
            }
            else
            {
                Add(project);
            }
        }

        /// <summary>
        /// Reset items.
        /// </summary>
        /// <param name="disposing">true value allows the real disposing for each control; false only avoids gui rendering.</param>
        public void Reset(bool disposing)
        {
            panelMain.Controls.Clear();

            if(!disposing) {
                items.ForEach(i => i.rendered = false);
                return;
            }

            items.ForEach(i => i.control.Dispose());
            items.Clear();
        }

        /// <summary>
        /// Returns an additional installed projects that are not presented in Data property.
        /// </summary>
        public IEnumerable<IProject> GetInactiveInstalled(IEnumerable<IProject> projects)
        {
            return projects?.Where(p => p.Installed && Data.All(d => d.DxpIdent != p.DxpIdent))
                            .ForEach(p => p.Config.Install = true); // since we're not using ConfigureProject()
        }

        public ProjectItemsControl()
        {
            InitializeComponent();

            empty = new Lazy<ProjectItemControl>(() => 
            {
                var item = new ProjectItemControl(new Project(new XProject()));
                ConfigureControl(item, item.Project);
                return item;
            });
        }

        /// <summary>
        /// To add IProject for panel.
        /// Will also add an item into collection if it's still not presented there.
        /// </summary>
        /// <param name="project"></param>
        private void Add(IProject project)
        {
            if(project == null) {
                throw new ArgumentNullException(nameof(project));
            }

            var prj = items.FirstOrDefault(i => i.control.Project.DxpIdent == project.DxpIdent);

            var control     = prj?.control ?? new ProjectItemControl(project);
            control.Order   = CountRendered;

            if(prj?.control == null)
            {
                ConfigureControl(control, project);

                items.Add(new UProject() {
                    control = control,
                    project = project
                });
            }
            else {
                prj.rendered = true;
            }

            panelMain.Controls.Add(control);
        }

        private void UseStub() => panelMain.Controls.Add(empty.Value);

        private void ConfigureControl(ProjectItemControl control, IProject project)
        {
            if(project?.ProjectPath == null || project.Config == null) 
            {
                control.Identifier = Project.DXP_INVALID;
                control.ProjectPath = "<<<>>>";
                return;
            }

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

            control.UIAction(x => 
                project.Config.Namespace = x.Namespaces.Text.Trim()
            );

            return project;
        }

        #region disposing

        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null)) {
                components.Dispose();
            }

            Reset(true);
            empty.Value?.Dispose();

            base.Dispose(disposing);
        }

        #endregion
    }
}

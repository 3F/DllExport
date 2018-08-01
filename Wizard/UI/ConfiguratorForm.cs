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
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using net.r_eg.DllExport.NSBin;
using net.r_eg.DllExport.Wizard.Extensions;
using net.r_eg.DllExport.Wizard.UI.Extensions;
using net.r_eg.MvsSln.Log;

namespace net.r_eg.DllExport.Wizard.UI
{
    internal sealed partial class ConfiguratorForm: Form, IRender
    {
        public const int MAX_VIEW_ITEMS = 2;

        private IExecutor exec;
        private Lazy<IExtCfg> extcfg;

        private CfgStorage storage;
        private FileDialog fdialog;
        private int prevSlnItemIndex = 0;
        private object sync = new object();

        /// <summary>
        /// To apply filter for rendered projects.
        /// </summary>
        /// <param name="filter"></param>
        public void ApplyFilter(ProjectFilter filter)
        {
            RenderProjects(exec.ActiveSlnFile, filter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enabled"></param>
        public void ShowProgressLine(bool enabled)
        {
            if(enabled) {
                progressLine.StartTrainEffect(panelTop.Width, 40, 50);
            }
            else {
                progressLine.StopAll();
            }
        }

        public ConfiguratorForm(IExecutor exec)
        {
            this.exec = exec ?? throw new ArgumentNullException(nameof(exec));

            extcfg = new Lazy<IExtCfg>(() => new Kit.FilterLineControl(this, exec));

            InitializeComponent();

            Text = ".NET DllExport";

#if PUBLIC_RELEASE
            Text += " - v" + WizardVersion.S_INFO;
#else
            Text += $" - Based on v{WizardVersion.S_NUM} {WizardVersion.S_REL} [{WizardVersion.BRANCH_SHA1}]";
#endif
#if DEBUG
            Text += " [ Debug ]";
#endif
            Text += " github.com/3F/DllExport";

            projectItems.Browse  =
            projectItems.OpenUrl = OpenUrl;

            projectItems.NamespaceValidate = (string ns) => {
                return DDNS.IsValidNS(ns?.Trim());
            };

            progressLine.StartTrainEffect(panelTop.Width);
            {
                RenderSlnFiles();
                comboBoxSln.SelectedIndex = 0;

                storage = new CfgStorage(exec, comboBoxStorage);
                storage.UpdateItem();
            }
            progressLine.StopAll();

            Load += (object sender, EventArgs e) => { TopMost = false; TopMost = true; };
        }

        private void OpenUrl(string url)
        {
            url.OpenUrl();
        }

        private void RenderSlnFiles()
        {
            comboBoxSln.Items.Clear();
            foreach(var sln in exec.SlnFiles) {
                comboBoxSln.Items.Add(sln);
            }
            comboBoxSln.Items.Add(">> Select .sln ... <<");
        }

        private string OpenFile()
        {
            if(fdialog == null) {
                fdialog = new OpenFileDialog() { Filter = "Solution File (*.sln)|*.sln" };
            }

            var dlgres  = DialogResult.None;
            var thread  = new Thread(() => dlgres = fdialog.ShowDialog());

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            if(dlgres != DialogResult.OK) {
                return null;
            };

            return fdialog.FileName;
        }

        private void RenderProjects(ComboBox box)
        {
            if(box.Items.Count < 1) {
                return;
            }

            if(box.SelectedIndex != box.Items.Count - 1) {
                RenderProjects(box.SelectedItem.ToString());
                return;
            }

            string file = OpenFile();
            if(file == null 
                || !file.TrimEnd().EndsWith(".sln", StringComparison.InvariantCultureIgnoreCase))
            {
                DoSilentAction(() => box.SelectedIndex = prevSlnItemIndex);
                return;
            }

            DoSilentAction(() =>
            {
                if(box.Items.Contains(file)) {
                    box.SelectedIndex = box.Items.IndexOf(file);
                }
                else {
                    box.Items.Insert(0, file);
                    box.SelectedIndex = 0;
                }
            });

            RenderProjects(file);
        }

        private void RenderProjects(string sln)
        {
            if(String.IsNullOrWhiteSpace(sln)) {
                return;
            }

            exec.ActiveSlnFile = sln;
            storage.UpdateItem();

            toolTipMain.SetToolTip(comboBoxSln, sln);

            GetProjects(sln).ForEach(prj => projectItems.Add(prj));
        }

        private void RenderProjects(string sln, ProjectFilter filter)
        {
            lock(sync)
            {
                projectItems.RenderedItemsSizeChanged -= projectItems_RenderedItemsSizeChanged;
                projectItems.Reset(false);

                projectItems.Pause();

                extcfg.Value.FilterProjects(filter, GetProjects(sln))
                                .ForEach(prj => projectItems.Add(prj));

                projectItems.Resume();
                projectItems.RenderedItemsSizeChanged += projectItems_RenderedItemsSizeChanged;
            }
        }

        private IEnumerable<IProject> GetProjects(string sln)
        {
            return exec.UniqueProjectsBy(sln)?
                            .OrderByDescending(p => p.Installed)
                            .OrderByDescending(p => p.InternalError == null);
        }

        private void DoSilentAction(Action act, ComboBox box, EventHandler handler)
        {
            lock(sync)
            {
                box.SelectedIndexChanged -= handler;
                act();
                box.SelectedIndexChanged += handler;
            }
        }

        private void DoSilentAction(Action act)
        {
            DoSilentAction(act, comboBoxSln, comboBoxSln_SelectedIndexChanged);
        }

        private void ResizeHeight()
        {
            int actual = Math.Max(
                projectItems.MaxItemHeight,
                Math.Min(projectItems.MaxItemsHeight, projectItems.GetMaxItemsHeight(MAX_VIEW_ITEMS))
            );

            ClientSize = new Size(ClientSize.Width, panelTop.Height + actual);
        }

        private void comboBoxSln_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(sender is ComboBox) {
                RenderProjects((ComboBox)sender);
            }
            prevSlnItemIndex = comboBoxSln.SelectedIndex;

            ResizeHeight();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            foreach(var prj in projectItems.Data)
            {
                if(!DDNS.IsValidNS(prj.Config.Namespace)) {
                    MessageBox.Show($"Fix incorrect namespace before continue:\n\n'{prj.Config.Namespace}'", "Incorrect data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if(exec.Config.CfgStorage == CfgStorageType.TargetsFile) {
                    exec.TargetsFile?.Configure(ActionType.Configure, prj);
                }
                prj.Configure(ActionType.Configure);
            }
            Close();
        }

        private void btnExt_Click(object sender, EventArgs e)
        {
            if(extcfg.IsValueCreated) {
                ((Kit.FilterLineControl)extcfg.Value).Show();
                return;
            }

            LSender.Send(this, $"Create {nameof(Kit.FilterLineControl)} panel");

            var panel = ((Kit.FilterLineControl)extcfg.Value);

            panel.Left      = 0;
            panel.Top       = 0;
            panel.Width     = panelTop.Width;
            panel.Height    = panelTop.Height/* - progressLine.Height*/;

            panelTop.Controls.Add(panel);
            panel.Dock = DockStyle.Fill;

            panel.BringToFront();
            progressLine.BringToFront();

            panel.Show();
        }

        private void projectItems_RenderedItemsSizeChanged(object sender, EventArgs e)
        {
            ResizeHeight();
        }
    }
}

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
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using net.r_eg.DllExport.NSBin;

namespace net.r_eg.DllExport.Wizard.UI
{
    internal sealed partial class ConfiguratorForm: Form
    {
        public const int MAX_VIEW_ITEMS = 2;

        private IExecutor exec;
        private FileDialog fdialog;
        private object sync = new object();

        public ConfiguratorForm(IExecutor exec)
        {
            this.exec = exec ?? throw new ArgumentNullException(nameof(exec));

            InitializeComponent();

            Text = ".NET DllExport";

#if PUBLIC_RELEASE
            Text += " - v" + WizardVersion.S_INFO;
#else
            Text += " - v" + WizardVersion.S_NUM;
#endif
#if DEBUG
            Text += " [ Debug ]";
#endif
            Text += " github.com/3F/DllExport";

            projectItems.Browse  =
            projectItems.OpenUrl = OpenUrl;

            projectItems.NamespaceValidate = (string str) => {
                return DDNS.IsValidNS(str);
            };

            RenderSlnFiles();
            comboBoxSln.SelectedIndex = 0;
        }

        private void OpenUrl(string url)
        {
            if(!String.IsNullOrWhiteSpace(url)) {
                System.Diagnostics.Process.Start(url);
            }
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
                fdialog = new OpenFileDialog();
            }

            if(fdialog.ShowDialog(this) != DialogResult.OK) {
                return null;
            }
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

            Task.Factory.StartNew(() =>
            {
                string file = OpenFile();
                BeginInvoke((MethodInvoker)delegate
                {
                    lock(sync)
                    {
                        comboBoxSln.SelectedIndexChanged -= comboBoxSln_SelectedIndexChanged;
                        if(box.Items.Contains(file)) {
                            box.SelectedIndex = box.Items.IndexOf(file);
                        }
                        else {
                            box.Items.Insert(0, file);
                            box.SelectedIndex = 0;
                        }
                        comboBoxSln.SelectedIndexChanged += comboBoxSln_SelectedIndexChanged;
                    }
                    RenderProjects(file);
                });
            });
        }

        private void RenderProjects(string sln)
        {
            if(String.IsNullOrWhiteSpace(sln)) {
                return;
            }
            projectItems.Reset();

            var projects = exec.UniqueProjectsBy(sln);
            if(projects != null)
            {
                foreach(var project in projects) {
                    projectItems.Add(project);
                }
            }

            ClientSize = new Size(
                ClientSize.Width, 
                panelTop.Height + Math.Max(
                                    projectItems.HeightOfItem, 
                                    Math.Min(projectItems.HeightOfItem * projectItems.Count, projectItems.HeightOfItem * MAX_VIEW_ITEMS)
                                  )
            );
        }

        private void comboBoxSln_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(sender is ComboBox) {
                RenderProjects((ComboBox)sender);
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            foreach(var prj in projectItems.Data) {
                prj.Configure(ActionType.Configure);
            }
            Close();
        }

        private void btnBug_Click(object sender, EventArgs e)
        {
            OpenUrl("https://github.com/3F/DllExport/issues");
        }
    }
}

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
using System.Windows.Forms;
using net.r_eg.DllExport.Wizard.UI.Controls;

namespace net.r_eg.DllExport.Wizard.UI
{
    internal sealed partial class ConfiguratorForm: Form
    {
        private List<UProject> items = new List<UProject>();
        private IExecutor exec;
        private FileDialog fdialog;

        private struct UProject
        {
            public ProjectItemControl control;
            public IProject project;
        }

        public ConfiguratorForm(IExecutor exec)
        {
            this.exec = exec ?? throw new ArgumentNullException(nameof(exec));

            InitializeComponent();

            RenderSlnFiles();
        }

        private void AddRc(IProject project)
        {
            var control = new ProjectItemControl();
            control.Top = control.Height * items.Count;

            panelMain.Controls.Add(control);
            items.Add(new UProject() { control = control, project = project });
        }

        private void ResetRc()
        {
            items.Clear();
            panelMain.Controls.Clear();
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

        private void RenderSlnFiles()
        {
            comboBoxSln.Items.Clear();
            foreach(var sln in exec.SlnFiles) {
                comboBoxSln.Items.Add(sln);
            }
            comboBoxSln.Items.Add(">> Select .sln ... <<");
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

            System.Threading.Tasks.Task.Factory.StartNew(() => {
                string file = OpenFile();
                BeginInvoke((MethodInvoker)delegate {
                    RenderProjects(file);
                });
            });
        }

        private void RenderProjects(string sln)
        {
            if(String.IsNullOrWhiteSpace(sln)) {
                return;
            }

            ResetRc();
            foreach(var project in exec.UniqueProjectsBy(sln)) {
                AddRc(project);
            }
        }

        private void comboBoxSln_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(sender is ComboBox) {
                RenderProjects((ComboBox)sender);
            }
        }
    }
}

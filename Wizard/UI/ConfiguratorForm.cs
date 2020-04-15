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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using net.r_eg.DllExport.NSBin;
using net.r_eg.DllExport.Wizard.Extensions;
using net.r_eg.DllExport.Wizard.UI.Extensions;
using net.r_eg.DllExport.Wizard.UI.Kit;
using net.r_eg.MvsSln.Log;

namespace net.r_eg.DllExport.Wizard.UI
{
    internal sealed partial class ConfiguratorForm: Form, IRender
    {
        private const string CL_TRUE  = " ";
        private const string CL_FALSE = "";

        private readonly IExecutor exec;
        private readonly Lazy<IExtCfg> extcfg;

        private CfgStorage storage;
        private FileDialog fdialog;
        private readonly Icons icons = new Icons();
        private readonly Caller caller;
        private readonly PackageInfo pkgVer;
        private readonly IConfFormater confFormater;
        private int prevSlnItemIndex = 0;
        private volatile bool _suspendCbSln;
        private readonly string updaterInitName;
        private CancellationTokenSource ctsUpdater;
        private Task tUpdater;
        private readonly object sync = new object();

        private string UpdToVersion => cbPackages.Text.Trim();
        private string CmdUpdate => $".\\{UserConfig.MGR_NAME} -action Upgrade -dxp-version ";

        /// <summary>
        /// To apply filter for rendered projects.
        /// </summary>
        /// <param name="filter"></param>
        public void ApplyFilter(ProjectFilter filter)
        {
            RenderProjects(exec.ActiveSlnFile, filter);
        }

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
            this.exec       = exec ?? throw new ArgumentNullException(nameof(exec));

            extcfg          = new Lazy<IExtCfg>(() => new FilterLineControl(this, exec));
            caller          = new Caller(exec.Config.SlnDir);
            pkgVer          = new PackageInfo(exec);
            confFormater    = new SimpleConfFormater(exec);

            InitializeComponent();

            updaterInitName = tabUpdating.Text;
            Text = GetVersionInfo();

            projectItems.Browse  =
            projectItems.OpenUrl = (string url) => url.OpenUrl();

            projectItems.NamespaceValidate = (string ns) => DDNS.IsValidNS(ns?.Trim());

            ShowFilterPanel();
            txtBuildInfo.Text = GetBuildInfo();

            RenderSlnFiles();
            comboBoxSln.SelectedIndex = 0;

            storage = new CfgStorage(exec, comboBoxStorage);
            storage.UpdateItem();

            projectItems.Set(null); // TODO: this only when no projects in solution and only when initial start
        }

        private void ConfiguratorForm_Load(object sender, EventArgs e)
        {
            TopMost = false; TopMost = true;

            if(!string.IsNullOrEmpty(pkgVer.Activated))
            {
                UpdateListOfPackages();
                txtLogUpd.SetData($"{CmdUpdate} ...");
            }
            else
            {
                panelUpdVerTop.Enabled = false;
                btnToOnline.Visible = true;
                txtLogUpd.SetData("You're using an offline version or such `-dxp-version actual`.");
            }
        }

        private void UpdateListOfPackages()
        {
            const int _ANI_DELAY = 550; //ms

            if(tUpdater != null && tUpdater.Status != TaskStatus.Running
                && !(tUpdater.IsCompleted || tUpdater.IsCanceled || tUpdater.IsFaulted)) { return; }

            ctsUpdater = ctsUpdater.CancelAndResetIfRunning(tUpdater, _ANI_DELAY * 2);

            tUpdater?.Dispose();
            cbPackages.Items.Clear();
            ((Control)tabUpdating).Enabled = false;

            tUpdater = Task.Factory
                .StartNew(() => pkgVer.GetFromGitHubAsync(ctsUpdater.Token), ctsUpdater.Token)
                .ContinueWith(t =>
                {
                    var rctask      = t.Result;
                    var releases    = rctask.Result.ToArray();

                    cbPackages.UIAction(x => x.Items.AddRange(releases));

                    int pos = cbPackages.FindString(pkgVer.Activated);
                    cbPackages.UIAction(x =>
                    {
                        if(pos == -1) {
                            x.Text = pkgVer.Activated;
                        }
                        else {
                            x.SelectedIndex = pos;
                        }
                    });
                    return releases;

                }, ctsUpdater.Token)
                .ContinueWith(t => 
                {
                    tabUpdating.UIAction(x => x.Enabled = true);
                    if(!pkgVer.IsNewStableVersionFrom(t.Result, out Version remote)) {
                        return;
                    }

                    tabUpdating.UIBlinkText
                    (
                        _ANI_DELAY,
                        $" Up to {remote}",
                        ctsUpdater.Token,
                        "^....",
                        ".^...",
                        "..^..",
                        "...^.",
                        "....^",
                        "..... "
                    );
                    tabUpdating.UIAction(x => x.Text = updaterInitName);

                }, ctsUpdater.Token);
        }

        private string GetVersionInfo(bool urlinfo = true)
        {
            var sb = new StringBuilder();

            sb.Append(".NET DllExport");

#if PUBLIC_RELEASE
            sb.Append(" " + WizardVersion.S_INFO);
#else
            sb.Append($" - Based on {WizardVersion.S_NUM}");
#endif
            if(WizardVersion.S_REL.Length > 0)
            {
                sb.Append($" [{WizardVersion.S_REL}]");
            }
#if DEBUG
            sb.Append(" [Debug]");
#endif
            if(urlinfo)
            {
                sb.Append(" //github.com/3F/DllExport");
            }

            return sb.ToString();
        }

        private void ShowFilterPanel()
        {
            if(extcfg.IsValueCreated) {
                ((FilterLineControl)extcfg.Value).Show();
                return;
            }

            LSender.Send(this, $"Create {nameof(FilterLineControl)} panel");

            var panel = (FilterLineControl)extcfg.Value;

            panel.Left      = 0;
            panel.Top       = 0;
            panel.Width     = panelFilter.Width;
            panel.Height    = panelFilter.Height;

            panelFilter.Controls.Add(panel);
            panel.Dock = DockStyle.Fill;

            panel.BringToFront();
            progressLine.BringToFront();

            panel.Show();
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
                EnableTabsWhenNoSln(false);
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
            if(string.IsNullOrWhiteSpace(sln)) {
                return;
            }

            exec.ActiveSlnFile = sln;
            storage.UpdateItem();

            toolTipMain.SetToolTip(comboBoxSln, sln);

            RenderListOfProjects(GetProjects(sln));
        }

        private void RenderProjects(string sln, ProjectFilter filter)
        {
            projectItems.Pause();
            dgvFilter.Pause();

            RenderListOfProjects(
                extcfg.Value.FilterProjects(filter, GetProjects(sln))
            );

            dgvFilter.Resume();
            projectItems.Resume();
        }

        private void RenderListOfProjects(IEnumerable<IProject> projects)
        {
            dgvFilter.Rows.Clear();
            projects.ForEach(prj => 
            {
                int n = dgvFilter.Rows.Add
                (
                    prj.Installed ? CL_TRUE : CL_FALSE,
                    icons.GetIcon(prj.XProject.ProjectItem.project.EpType),
                    prj.ProjectPath
                );

                dgvFilter.Rows[n].Cells[1].ToolTipText = prj.XProject.ProjectItem.project.EpType.ToString();
                dgvFilter.Rows[n].Cells[0].Style.BackColor = (prj.Installed)? Color.FromArgb(111, 145, 6) 
                                                                            : Color.FromArgb(168, 47, 17);
            });

            EnableTabsWhenNoSln(dgvFilter.Rows.Count > 0);
        }

        private IEnumerable<IProject> GetProjects(string sln)
        {
            return exec.UniqueProjectsBy(sln)?
                            .OrderByDescending(p => p.Installed)
                            .OrderByDescending(p => p.InternalError == null);
        }

        private void DoSilentAction(Action act)
        {
            lock(sync)
            {
                _suspendCbSln = true;
                act();
                _suspendCbSln = false;
            }
        }

        private void Execute(string cmd, Action success, Action<int> failed)
        {
            txtLogUpd.AppendData(">" + cmd);

            void std(object _, DataReceivedEventArgs _e)
            {
                if(!string.IsNullOrEmpty(_e.Data)) {
                    txtLogUpd.UIAction(x => x.AppendData(_e.Data));
                }
            }

            Task.Factory.StartNew(() => caller.Shell
            (
                cmd,
                Caller.WAIT_INF,
                (p) => 
                {
                    if(p.ExitCode != 0) 
                    {
                        failed(p.ExitCode);
                        return;
                    }

                    success();
                },
                std, std
            ));
        }

        private void EnableTabsWhenNoSln(bool status) => ((Control)tabCfgDxp).Enabled = status;

        private string GetBuildInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine(GetVersionInfo(false));

            var info = Path.Combine(exec.Config.PkgPath, "build_info.txt");
            if(!File.Exists(info))
            {
                sb.Append("Detailed information about build was not found. :(");
            }
            else
            {
                File.ReadAllLines(info).ForEach(s =>
                {
                    sb.Append(Regex.Replace(s, @":(\s\s*)(?!generated)", (Match m) => $": {m.Groups[1].Value.Replace(' ', '.')} "));
                    sb.AppendLine();
                });
            }

            return sb.ToString();
        }

        private void comboBoxSln_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(_suspendCbSln) { return; }

            ((FilterLineControl)extcfg.Value).FilterText = string.Empty;

            if(sender is ComboBox) 
            {
                progressLine.StartTrainEffect(panelTop.Width);
                RenderProjects((ComboBox)sender);
                progressLine.StopAll();
            }
            prevSlnItemIndex = comboBoxSln.SelectedIndex;
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

        private void lnkSrc_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => "https://github.com/3F/DllExport".OpenUrl();
        private void linkIlasm_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => "https://github.com/3F/coreclr".OpenUrl();
        private void lnk3F_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => "https://github.com/3F".OpenUrl();

        private void dgvFilter_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex == -1 || e.RowIndex >= dgvFilter.RowCount) {
                return;
            }

            string path     = dgvFilter.Rows[e.RowIndex].Cells[gcPath.Name].Value.ToString();
            IProject prj    = GetProjects(exec.ActiveSlnFile).FirstOrDefault(p => p.ProjectPath == path);

            projectItems.Pause();
            projectItems.Set(prj);
            projectItems.Resume();
            
            txtCfgData.Text = confFormater.Parse(prj);
        }

        private void dgvFilter_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.F2:
                case Keys.Enter:
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                    return;
                }
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(UpdToVersion)) {
                txtLogUpd.AppendData($"You need specify version for command.");
                return;
            }

            if(pkgVer.Activated == UpdToVersion) {
                txtLogUpd.AppendData($"You're already using {pkgVer.Activated}");
                return;
            }

            panelUpdVerTop.Enabled = false;
            txtLogUpd.SetData($"Updating to {UpdToVersion} is starting ...");

            Execute
            (
                CmdUpdate + UpdToVersion, 
                () => 
                {
                    caller.Shell($".\\{UserConfig.MGR_NAME} -action Configure");
                    Close();
                },
                (code) =>
                {
                    txtLogUpd.UIAction(x => x.AppendData("Failed Task."));
                    panelUpdVerTop.UIAction(x => x.Enabled = true);
                }
            );
        }

        private void BtnUpdListOfPkg_Click(object sender, EventArgs e) => UpdateListOfPackages();

        private void BtnToOnline_Click(object sender, EventArgs e)
        {
            const string _FAILED = "Failed Task. You only need to try manually.";

            btnToOnline.Visible = false;

            var src = Path.Combine(exec.Config.PkgPath, UserConfig.MGR_FILE);
            if(!File.Exists(src)) 
            {
                txtLogUpd.AppendData($"{UserConfig.MGR_FILE} was not found in `{exec.Config.PkgPath}`.");
                txtLogUpd.AppendData(_FAILED);
                return;
            }
            File.Copy(src, Path.Combine(exec.Config.SlnDir, UserConfig.MGR_FILE), true);

            Execute
            (
                $".\\{UserConfig.MGR_NAME} -action Update",
                () => 
                {
                    caller.Shell($".\\{UserConfig.MGR_NAME} -action Configure");
                    Close();
                },
                (code) =>
                {
                    txtLogUpd.UIAction(x => x.AppendData(_FAILED));
                }
            );
        }
    }
}

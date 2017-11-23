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
using System.IO;
using System.Linq;
using System.Text;
using net.r_eg.DllExport.NSBin;
using net.r_eg.DllExport.Wizard.Extensions;
using net.r_eg.MvsSln;
using net.r_eg.MvsSln.Core;
using net.r_eg.MvsSln.Log;

namespace net.r_eg.DllExport.Wizard
{
    public class Executor: IExecutor, IConfigInitializer, IDisposable
    {
        protected Dictionary<string, IEnvironment> solutions = new Dictionary<string, IEnvironment>();

        /// <summary>
        /// Cache for loaded projects.
        /// </summary>
        protected Dictionary<Guid, IProject> pcache = new Dictionary<Guid, IProject>();

        /// <summary>
        /// Access to wizard configuration.
        /// </summary>
        public IWizardConfig Config
        {
            get;
            protected set;
        }

        public ISender Log
        {
            get => LSender._;
        }

        /// <summary>
        /// ddNS feature core.
        /// </summary>
        public IDDNS DDNS
        {
            get;
            set;
        } = new DDNS(Encoding.UTF8);

        /// <summary>
        /// Latest selected .sln file.
        /// </summary>
        public virtual string ActiveSlnFile
        {
            get => _activeSlnFile;
            set
            {
                _activeSlnFile = value;
                Log.send(this, $"Selected .sln '{_activeSlnFile}'", Message.Level.Info);
                UpdateCfgStorageType(_activeSlnFile);
                _targetsFile = null;
            }
        }
        private string _activeSlnFile;

        /// <summary>
        /// List of available .sln files.
        /// </summary>
        public IEnumerable<string> SlnFiles
        {
            get => Directory.GetFiles(Config.SlnDir, "*.sln", SearchOption.TopDirectoryOnly)
                                .Select(s => Path.GetFullPath(s));
        }

        /// <summary>
        /// Access to used external .targets.
        /// </summary>
        public ITargetsFile TargetsFile
        {
            get
            {
                if(_targetsFile != null) {
                    return _targetsFile;
                }

                if(String.IsNullOrWhiteSpace(Config?.StoragePath) 
                    || String.IsNullOrWhiteSpace(ActiveSlnFile))
                {
                    return null;
                }

                string dir      = Path.GetDirectoryName(ActiveSlnFile);
                string xfile    = Path.Combine(dir, Config.StoragePath);

                _targetsFile = new TargetsFile(xfile, dir);
                return _targetsFile;
            }
        }
        protected ITargetsFile _targetsFile;

        /// <summary>
        /// List of all found projects with different configurations.
        /// </summary>
        /// <param name="sln">Full path to .sln</param>
        /// <returns></returns>
        public IEnumerable<IProject> ProjectsBy(string sln)
        {
            return GetEnv(sln)?.Projects?.Select(p => GetProject(p));
        }

        /// <summary>
        /// List of all found projects that's unique by guid.
        /// </summary>
        /// <param name="sln"></param>
        /// <returns></returns>
        public IEnumerable<IProject> UniqueProjectsBy(string sln)
        {
            return GetEnv(sln)?.UniqueByGuidProjects?.Select(p => GetProject(p));
        }

        /// <summary>
        /// To start process of the required configuration.
        /// </summary>
        public void Configure()
        {
            Log.send(this, $"Action: {Config.Type}; Storage: {Config.CfgStorage}", Message.Level.Info);

            if(Config.Type == ActionType.Configure)
            {
                UI.App.RunSTA(new UI.ConfiguratorForm(this));
                return;
            }

            if(Config.Type == ActionType.Restore || Config.Type == ActionType.Update)
            {
                var sln = String.IsNullOrWhiteSpace(Config.SlnFile) ? 
                                    SlnFiles?.FirstOrDefault() : Config.SlnFile;

                ActiveSlnFile = sln ?? throw new ArgumentException(
                    String.Format(
                        "We can't find any .sln file to continue processing. Use '{0}' property or check '{1}'",
                        nameof(IWizardConfig.SlnFile),
                        nameof(IWizardConfig.SlnDir)
                    )
                );

                Configure(sln);
            }

            if(Config.Type == ActionType.Info)
            {
                (new CfgBatWrapper(Config, Log)).TryPrepare();
                UI.App.RunSTA(new UI.InfoForm(this));
                return;
            }
        }

        /// <param name="cfg"></param>
        public Executor(IWizardConfig cfg)
        {
            Config = cfg ?? throw new ArgumentNullException(nameof(cfg));
        }

        protected void Configure(string sln)
        {
            UniqueProjectsBy(sln)?.ForEach(p => 
            {
                if(Config.CfgStorage == CfgStorageType.TargetsFile) {
                    TargetsFile?.Configure(Config.Type, p);
                }
                p.Configure(Config.Type);
            });
        }

        /// <summary>
        /// Updates CfgStorage type via current state from selected .sln.
        /// </summary>
        /// <param name="sln">Path to .sln</param>
        /// <returns></returns>
        protected bool UpdateCfgStorageType(string sln)
        {
            if(String.IsNullOrWhiteSpace(sln)) {
                return false;
            }

            if(UniqueProjectsBy(sln)?.Any(p => p.HasExternalStorage) == true) {
                Config.CfgStorage = CfgStorageType.TargetsFile;
                return true;
            }

            Config.CfgStorage = CfgStorageType.ProjectFiles;
            return true;
        }

        protected IProject GetProject(IXProject xp)
        {
            var pid = xp.GetPId();

            if(!pcache.ContainsKey(pid)) {
                pcache[pid] = new Project(xp, this);
            }

            return pcache[pid];
        }

        protected IEnvironment GetEnv(string file)
        {
            if(String.IsNullOrWhiteSpace(file)) {
                return null;
            }

            if(!solutions.ContainsKey(file))
            {
                var sln = new Sln(file, SlnItems.Projects 
                                            | SlnItems.SolutionConfPlatforms 
                                            | SlnItems.ProjectConfPlatforms);

                solutions[file] = new DxpIsolatedEnv(sln.Result);
                solutions[file].LoadMinimalProjects();
            }

            return solutions[file];
        }

        private void Free()
        {
            solutions?.ForEach(sln => sln.Value?.Dispose());

            if(_targetsFile != null) {
                _targetsFile.Dispose();
            }
        }

        #region IDisposable

        // To detect redundant calls
        private bool disposed = false;

        // To correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposed) {
                return;
            }
            disposed = true;

            Free();
        }

        #endregion
    }
}

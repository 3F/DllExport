/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
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
using net.r_eg.MvsSln.Extensions;
using net.r_eg.MvsSln.Log;

namespace net.r_eg.DllExport.Wizard
{
    public class Executor(IWizardConfig cfg): IExecutor, IConfigInitializer, IDisposable
    {
        private string _activeSlnFile;
        private ITargetsFile _targetsFile;

        protected Dictionary<string, IEnvironment> solutions = [];
        protected Dictionary<Guid, IProject> pcache = [];

        public IWizardConfig Config { get; protected set; } = cfg ?? throw new ArgumentNullException(nameof(cfg));

        public ISender Log => LSender._;

        public IDDNS DDNS { get; set; } = new DDNS(Encoding.UTF8);

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

        public IEnumerable<string> SlnFiles
            => Directory.GetFiles(Config.SlnDir, "*.sln", SearchOption.TopDirectoryOnly)
                        .Combine(Config.SlnFile, true)
                        .Select(Path.GetFullPath);

        public ITargetsFile TargetsFile
        {
            get
            {
                _targetsFile ??= new TargetsFile
                (
                    GetTStoragePath(out string _),
                    this
                );
                return _targetsFile;
            }
        }

        public ITargetsFile TargetsFileIfCfg
            => (Config?.CfgStorage == CfgStorageType.TargetsFile) ? TargetsFile : null;

        public IEnumerable<IProject> ProjectsBy(string sln)
        {
            return GetEnv(sln)?.Projects?.Select(GetProject);
        }

        public IEnumerable<IProject> UniqueProjectsBy(string sln)
        {
            return GetEnv(sln)?.UniqueByGuidProjects?.Select(GetProject);
        }

        public void Configure()
        {
            Log.send(this, $"Action: {Config.Type}; Storage: {Config.CfgStorage}", Message.Level.Info);

            switch(Config.Type)
            {
                case ActionType.Configure:
                {
                    UI.App.RunSTA(new UI.ConfiguratorForm(this));
                    break;
                }

                case ActionType.Recover:
                case ActionType.RecoverInit:
                {
                    ActivateSln();
                    Configure(TargetsFile.XProject, Config.Type);
                    break;
                }

                case ActionType.Restore:
                case ActionType.Update:
                case ActionType.Export:
                case ActionType.Unset:
                {
                    Configure(ActivateSln());
                    break;
                }

                case ActionType.Info:
                {
                    "https://github.com/3F/DllExport/wiki/Quick-start".OpenUrl();
                    break;
                }

                case ActionType.Default: return;

                default: throw new NotImplementedException();
            }
        }

        public void SaveTStorageOrDelete()
        {
            if(Config?.CfgStorage == CfgStorageType.TargetsFile)
            {
                TargetsFile.Save(true);
                return;
            }

            try
            {
                File.Delete(GetTStoragePath(out _));
            }
            catch(Exception ex)
            {
                Log.send(this, $"External storage cannot be deleted due to error: {ex.Message}", Message.Level.Debug);
            }
        }

        /// <summary>
        /// Activates sln by using config.
        /// </summary>
        protected string ActivateSln()
        {
            string sln = string.IsNullOrWhiteSpace(Config.SlnFile) ?
                                SlnFiles?.FirstOrDefault() : Config.SlnFile;

            ActiveSlnFile = sln ?? throw new ArgumentException
            (
                string.Format
                (
                    "Solution file (.sln) was not found. Try '{0}' property or check '{1}'",
                    nameof(IWizardConfig.SlnFile),
                    nameof(IWizardConfig.SlnDir)
                )
            );

            return sln;
        }

        protected void Configure(string sln)
        {
            var projects = UniqueProjectsBy(sln);
            if(projects == null) {
                Log.send(this, $"{nameof(UniqueProjectsBy)} returned null for '{sln}'", Message.Level.Debug);
                return;
            }

            foreach(var prj in projects)
            {
                if(Config.Type == ActionType.Export) {
                    TargetsFile.Export(prj);
                    continue;
                }

                if(Config.Type == ActionType.Unset) {
                    prj.Unset();
                    continue;
                }

                if(Config.CfgStorage == CfgStorageType.TargetsFile) {
                    TargetsFile?.Configure(Config.Type, prj);
                }
                prj.Configure(Config.Type);
            }

            if(Config.Type == ActionType.Export) {
                TargetsFile.Save(true);
            }
        }

        protected void Configure(IXProject xp, ActionType type)
        {
            if(type == ActionType.Recover)
            {
                Config.CfgStorage = CfgStorageType.TargetsFile;
            }

            foreach(var pgr in xp.Project.Xml.PropertyGroups)
            {
                string file = null;
                string id   = null;

                Log.send(this, $"Searching the recover properties from '{pgr.Condition}':'{pgr.Label}'");

                foreach(var prop in pgr.Properties)
                {
                    if(prop.Name == MSBuildProperties.DXP_PRJ_FILE) {
                        file = prop.Value;
                        Log.send(this, $"Found {MSBuildProperties.DXP_PRJ_FILE} = '{file}'", Message.Level.Debug);
                        continue;
                    }

                    if(prop.Name == MSBuildProperties.DXP_CFG_ID) {
                        id = prop.Value;
                        Log.send(this, $"Found {MSBuildProperties.DXP_CFG_ID} = '{id}'", Message.Level.Debug);
                    }
                }

                if(file != null && id != null)
                {
                    Log.send(this, $"Trying to recover '{file}' : '{id}'", Message.Level.Info);
                    UniqueProjectsBy(ActiveSlnFile)?
                            .Where(p => p.ProjectPath == file)
                            .ForEach(p => p.Recover(id, type, xp));
                }
            }
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

        protected string GetTStoragePath(out string dir)
        {
            if(string.IsNullOrWhiteSpace(Config?.StoragePath) 
                || string.IsNullOrWhiteSpace(Config?.RootPath))
            {
                throw new ArgumentNullException(nameof(Config));
            }

            dir = Path.GetDirectoryName(Config.RootPath);
            return Path.Combine(dir, Config.StoragePath);
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

        #region IDisposable

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!disposed)
            {
                solutions?.ForEach(sln =>
                    sln.Value?.Dispose()
                );
                _targetsFile?.Dispose();

                disposed = true;
            }
        }

        #endregion
    }
}

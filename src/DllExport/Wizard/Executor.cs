﻿/*!
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
                                .Combine(Config.SlnFile, true)
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

                _targetsFile = new TargetsFile
                (
                    GetTStoragePath(out string dir), 
                    dir, 
                    this
                );
                return _targetsFile;
            }
        }
        private ITargetsFile _targetsFile;

        /// <summary>
        /// Access to used external .targets 
        /// Only if CfgStorageType.TargetsFile or null.
        /// </summary>
        public ITargetsFile TargetsFileIfCfg => (Config?.CfgStorage == CfgStorageType.TargetsFile)? TargetsFile : null;

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

        /// <summary>
        /// Depending on the current CfgStorageType, 
        /// Either save or delete used TargetsFile.
        /// </summary>
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
                // optional behavior, we don't care
                Log.send(this, $"Storage file cannot be deleted due to error: {ex.Message}", Message.Level.Debug);
            }
        }

        public Executor(IWizardConfig cfg) => Config = cfg ?? throw new ArgumentNullException(nameof(cfg));

        /// <summary>
        /// Activates sln by using config.
        /// </summary>
        /// <returns></returns>
        protected string ActivateSln()
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
                            .ForEach(p => p.Recover(id));
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
                || string.IsNullOrWhiteSpace(ActiveSlnFile))
            {
                dir = null;
                return null;
            }

            dir = Path.GetDirectoryName(ActiveSlnFile);
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

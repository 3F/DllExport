//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
//$ Distributed under the MIT License (MIT)

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using net.r_eg.DllExport;
using net.r_eg.MvsSln;
using net.r_eg.MvsSln.Core;
using net.r_eg.MvsSln.Extensions;

namespace RGiesecke.DllExport.MSBuild
{
    internal sealed class PostProc: IDisposable
    {
        private const string ENTRY_POINT = "DllExportPostProc";

        private readonly IList<string> allocatedItems = new List<string>();
        private readonly Sln _sln;
        private readonly TaskLoggingHelper log;

        public ISlnResult Sln => _sln.Result;

        public IXProject Prj
        {
            get;
            private set;
        }

        public IEnumerable<string> CallbackProperties
        {
            get;
            private set;
        }

        private IDictionary<string, string> SysProperties => new Dictionary<string, string>()
        {
            { "DllExport", DllExportVersion.S_NUM_REV },
            { "DllExportSln", Sln.SolutionFile },
            { "DllExportPrj", Prj.ProjectFullPath },
        };

        public bool Process(IPostProcExecutor executor)
        {
            if(executor == null) {
                log.LogError(Resources._0_is_not_configured, nameof(IPostProcExecutor));
                return false;
            }

            if(Prj == null) {
                log.LogMessage(Resources._0_is_ignored_due_to_1, nameof(IPostProcExecutor), $"{nameof(IXProject)} == null");
                return true;
            }

            return executor.Execute(Prj, ENTRY_POINT, SysProperties);
        }

        /// <param name="raw">$(SolutionPath);$(MSBuildThisFileFullPath);...CallbackProperties...</param>
        /// <param name="log"></param>
        public PostProc(string raw, TaskLoggingHelper log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            var data = (raw ?? throw new ArgumentNullException(nameof(raw))).Split(';');

            if(data.Length < 2) {
                throw new ArgumentException(string.Format(Resources.Incorrect_format_of_0_, "sln;prj"));
            }

            string slnfile = data[0].Trim();
            string prjfile = data[1].Trim();

            _sln = new Sln(slnfile, SlnItems.ProjectDependenciesXml | SlnItems.LoadMinimalDefaultData);
            Prj  = Sln.Env.Projects.FirstOrDefault(xp => xp.ProjectFullPath == prjfile);

            CallbackProperties = data.Skip(2).Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p));

            AllocateItem("DllExportDirX64", @"$(TargetDir)x64\*.*");
            AllocateItem("DllExportDirX86", @"$(TargetDir)x86\*.*");
            AllocateItem("DllExportDirBefore", @"$(TargetDir)Before\*.*");
            AllocateItem("DllExportDirAfter", @"$(TargetDir)After\*.*");

            PopulateProperties(GetDependents(Prj), (p) => $"DllExportDependents{p}");
            PopulateProperties(GetDependencies(Prj), (p) => $"DllExportDependencies{p}");
        }

        /// <summary>
        /// Get projects that depend on the specified project.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        private IEnumerable<IXProject> GetDependents(IXProject project)
        {
            foreach(var dep in Sln.ProjectDependencies.Dependencies)
            {
                if(dep.Value.Any(g => g == project.ProjectGuid))
                {
                    var prj = GetProjectByGuid(Sln.Env.Projects, dep.Key);
                    if(prj != null) yield return prj;
                }
            }
        }

        /// <summary>
        /// Get dependencies for the specified project.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        private IEnumerable<IXProject> GetDependencies(IXProject project)
        {
            foreach(var pguid in Sln.ProjectDependencies.Dependencies[project.ProjectGuid])
            {
                var prj = GetProjectByGuid(Sln.Env.Projects, pguid);
                if(prj != null) yield return prj;
            }
        }

        private void PopulateProperties(IEnumerable<IXProject> projects, Func<string, string> formatter)
        {
            foreach(var dep in projects)
            {
                CallbackProperties.ForEach(p => AllocateItem(formatter(p), dep.GetProperty(p, false).evaluatedValue));
            }
        }

        private void AllocateItem(string type, string inc)
        {
            if(Prj.AddItem(type, inc))
            {
                allocatedItems.Add(type);
                log.LogMessage(MessageImportance.Low, Resources.Allocated_item_0_1, type, inc);
            }
        }

        private IXProject GetProjectByGuid(IEnumerable<IXProject> projects, string guid) 
            => projects?.FirstOrDefault(p => p.ProjectGuid.Guid() == guid.Guid());

        #region IDisposable

        private bool disposed = false;

        public void Dispose() => Dispose(true);

        void Dispose(bool _)
        {
            if(disposed) return;
            disposed = true;

            allocatedItems?.ForEach(t => Prj.GetItems(t).ToArray().ForEach(i => Prj.RemoveItem(i)));
            _sln.Dispose();
        }
        
        #endregion
    }
}

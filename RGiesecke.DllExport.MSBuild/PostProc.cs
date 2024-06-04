/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

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
        internal const string ENTRY_POINT = "DllExportPostProc";

        /// <summary>
        /// Offset to CallbackProperties.
        /// </summary>
        internal const int OFS_ENV_PROP = 2; // sln;prj;...

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

            if(!IsExistCallback(ENTRY_POINT)) {
                log.LogMessage(Resources._0_is_ignored_due_to_1, nameof(IPostProcExecutor), $"{ENTRY_POINT} == null");
                return true;
            }

            return executor.Execute(Prj, ENTRY_POINT, SysProperties);
        }

        internal static string GetNameForDependentsProperty(string name) => $"DllExportDependents{name}";

        internal static string GetNameForSeqDependentsProperty(string name) => $"DllExportSeqDependents{name}";

        internal static string GetNameForDependenciesProperty(string name) => $"DllExportDependencies{name}";

        /// <param name="raw">$(SolutionPath);$(MSBuildThisFileFullPath);...CallbackProperties...</param>
        /// <param name="log"></param>
        public PostProc(string raw, TaskLoggingHelper log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            var data = (raw ?? throw new ArgumentNullException(nameof(raw))).Split(';');

            if(data.Length < OFS_ENV_PROP) {
                throw new ArgumentException(string.Format(Resources.Incorrect_format_of_0_, "sln;prj"));
            }

            string slnfile = data[0].Trim();
            string prjfile = data[1].Trim();

            _sln = new Sln(slnfile, SlnItems.ProjectDependenciesXml | SlnItems.LoadMinimalDefaultData);
            Prj  = GetProject(prjfile);

            CallbackProperties = data.Skip(OFS_ENV_PROP).Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p));

            AllocateItem("DllExportDirX64", @"$(TargetDir)x64\*.*");
            AllocateItem("DllExportDirX86", @"$(TargetDir)x86\*.*");
            AllocateItem("DllExportDirBefore", @"$(TargetDir)Before\*.*");
            AllocateItem("DllExportDirAfter", @"$(TargetDir)After\*.*");

            PopulateProperties(GetDependents(Prj), p => GetNameForDependentsProperty(p));
            PopulateProperties(GetSeqDependents(Prj), p => GetNameForSeqDependentsProperty(p));
            PopulateProperties(GetDependencies(Prj), p => GetNameForDependenciesProperty(p));
        }

        private IXProject GetProject(string file)
        {
            IXProject ret = Sln.Env.Projects.FirstOrDefault(p => p.ProjectFullPath == file);
            if(ret != null)
            {
                return ret;
            }

            // We're working with external storage through Import section.
            foreach(var xp in Sln.Env.Projects)
            {
                if(xp.Project.Imports?.Any(p => p.ImportedProject?.FullPath == file) == true)
                {
                    return xp;
                }
            }

            // or not ...
            throw new NotSupportedException(string.Format(Resources.File_0_is_not_supported_for_1, file, nameof(PostProc)));
        }

        /// <summary>
        /// Get projects that depend on the specified project.
        /// 
        /// https://github.com/3F/DllExport/pull/148#issuecomment-624831606
        /// ProjectC -} ProjectA + ProjectB
        /// ProjectB -} ProjectA
        /// ...
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        private IEnumerable<IXProject> GetDependents(IXProject project)
        {
            foreach(var dep in Sln.ProjectDependencies.Dependencies)
            {
                if(dep.Value.Any(g => g.Guid() == project.ProjectGuid.Guid()))
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

        /// <summary>
        /// Get projects that depend on the specified project.
        /// Including sequential referencing through other projects.
        /// 
        /// https://github.com/3F/DllExport/pull/148#issuecomment-624831606
        /// ProjectC -} ProjectA + ProjectB
        /// ProjectB -} ProjectA
        /// ...
        /// +
        /// ... ProjectC -} ProjectB -} ProjectA
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        private IEnumerable<IXProject> GetSeqDependents(IXProject project)
        {
            bool GetSeqDependents(HashSet<string> dep)
            {
                foreach(var prj in dep)
                {
                    if(prj.Guid() == project.ProjectGuid.Guid()) return true;
                    if(GetSeqDependents(Sln.ProjectDependencies.Dependencies[prj])) return true;
                }
                return false;
            }

            foreach(var dep in Sln.ProjectDependencies.Dependencies)
            {
                if(GetSeqDependents(dep.Value))
                {
                    var prj = GetProjectByGuid(Sln.Env.Projects, dep.Key);
                    if(prj != null) yield return prj;
                }
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
            if(!IsExistItem(type, inc) && Prj.AddItem(type, inc))
            {
                allocatedItems.Add(type);
                log.LogMessage(MessageImportance.Low, Resources.Allocated_item_0_1, type, inc);
            }
        }

        private bool IsExistItem(string type, string inc) => Prj.GetItem(type, inc).parentProject != null;

        private bool IsExistCallback(string name) => Prj.Project.Targets.ContainsKey(name) == true;

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

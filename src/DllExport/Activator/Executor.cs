/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using net.r_eg.DllExport;
using net.r_eg.MvsSln.Core;

namespace net.r_eg.DllExport.Activator
{
    internal class Executor: IPostProcExecutor
    {
        private readonly TaskLoggingHelper log;

        /// <summary>
        /// Executes IXProject through MSBuild engine.
        /// </summary>
        /// <param name="xp"></param>
        /// <param name="entrypoint">Initial target.</param>
        /// <param name="properties">Additional properties that will be available when executing code.</param>
        /// <returns>True if request was a complete success.</returns>
        public bool Execute(IXProject xp, string entrypoint, IDictionary<string, string> properties = null)
        {
            if(xp == null) {
                throw new ArgumentNullException(nameof(xp));
            }

            UpdateGlobalProperties(xp, properties);

            var request = new BuildRequestData
            (
                xp.Project.CreateProjectInstance(),
                new string[] { entrypoint ?? throw new ArgumentNullException(nameof(entrypoint)) }, 
                new HostServices()
            );

#if !NET40

            using(BuildManager manager = new BuildManager(DllExportVersion.DXP)) {
                return Build(manager, request, false);
            }

#else

            // 4.0.30319
            return Build(new BuildManager(DllExportVersion.DXP), request, false);

#endif
        }

        public Executor(TaskLoggingHelper log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        protected bool Build(BuildManager manager, BuildRequestData request, bool silent)
        {
            BuildResult result = manager.Build
            (
                new BuildParameters()
                {
                    MaxNodeCount = 4,
                    Loggers = new List<ILogger>() 
                    {
                        new ExecutorLogger(log) {
                            Verbosity = silent ? LoggerVerbosity.Quiet : LoggerVerbosity.Normal
                        }
                    }
                },
                request
            );

            return result.OverallResult == BuildResultCode.Success;
        }

        private void UpdateGlobalProperties(IXProject xp, IDictionary<string, string> properties)
        {
            if(properties == null) {
                return;
            }

            foreach(var prop in properties) 
            {
                xp.Project.SetGlobalProperty(prop.Key, prop.Value);
                log.LogMessage(MessageImportance.Low, Resources._0_is_configured_as_1, prop.Key, prop.Value);
            }
        }
    }
}

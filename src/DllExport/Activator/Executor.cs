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
using net.r_eg.MvsSln.Core;

namespace net.r_eg.DllExport.Activator
{
    internal class Executor(TaskLoggingHelper log): IPostProcExecutor
    {
        private readonly TaskLoggingHelper log = log ?? throw new ArgumentNullException(nameof(log));

        public bool Execute(IXProject xp, string entrypoint, IDictionary<string, string> properties = null)
        {
            if(xp == null) throw new ArgumentNullException(nameof(xp));

            UpdateGlobalProperties(xp, properties);

            BuildRequestData request = new
            (
                xp.Project.CreateProjectInstance(),
                [entrypoint ?? throw new ArgumentNullException(nameof(entrypoint))], 
                new HostServices()
            );

            // NOTE: Legacy BuildManager (like for netfx 4.0.30319) does not require disposing
            // so we'll continue to use try/finally here to support both versions due to possible retargeting in future releases.
            BuildManager buildManager = new(DllExportVersion.DXP);
            try
            {
                bool status = Build(buildManager, request, silent: false);
                log.LogMessage(MessageImportance.Low, $"{nameof(Executor)} {nameof(BuildManager)} = {status}");
                return status;
            }
            finally
            {
                if(buildManager is IDisposable bm) bm.Dispose();
            }
        }

        protected bool Build(BuildManager manager, BuildRequestData request, bool silent)
        {
            BuildResult result = manager.Build
            (
                new BuildParameters()
                {
                    MaxNodeCount = 4,
                    Loggers =
                    [new ExecutorLogger(log)
                    {
                        Verbosity = silent ? LoggerVerbosity.Quiet : LoggerVerbosity.Normal
                    }]
                },
                request
            );

            return result.OverallResult == BuildResultCode.Success;
        }

        private void UpdateGlobalProperties(IXProject xp, IDictionary<string, string> properties)
        {
            if(properties == null) return;

            foreach(var prop in properties) 
            {
                xp.Project.SetGlobalProperty(prop.Key, prop.Value);
                log.LogMessage(MessageImportance.Low, Resources._0_is_configured_as_1, prop.Key, prop.Value);
            }
        }
    }
}

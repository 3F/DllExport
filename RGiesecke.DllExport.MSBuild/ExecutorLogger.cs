//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2021  Denis Kuzmin <x-3F@outlook.com> github/3F
//$ Distributed under the MIT License (MIT)

using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace RGiesecke.DllExport.MSBuild
{
    internal class ExecutorLogger: ILogger
    {
        private readonly TaskLoggingHelper log;

        public string Parameters { get; set; }

        public LoggerVerbosity Verbosity { get; set; }

        public void Shutdown()
        {

        }

        public void Initialize(IEventSource eventSource)
        {
            eventSource.WarningRaised   += (sender, e) => log.LogWarning($"[{e.LineNumber}]: {e.Code} - '{e.Message}'");
            eventSource.ErrorRaised     += (sender, e) => log.LogError($"[{e.LineNumber}]: {e.Code} - '{e.Message}'");

            if(Verbosity != LoggerVerbosity.Quiet) {
                eventSource.AnyEventRaised += (sender, e) => log.LogMessage(MessageImportance.Low, $"{nameof(ExecutorLogger)}: {e.Message}");
            }
        }

        public ExecutorLogger(TaskLoggingHelper log) => this.log = log ?? throw new ArgumentNullException(nameof(log));
    }
}

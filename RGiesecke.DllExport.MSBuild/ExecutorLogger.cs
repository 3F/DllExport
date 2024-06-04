/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

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

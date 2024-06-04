/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Diagnostics;

namespace net.r_eg.DllExport.Wizard
{
    internal class Caller
    {
        internal const int WAIT_INF = -1;
        internal const int WAIT_NOT = 0;

        protected string workingDir;

        public int Shell(string cmd, int wait = WAIT_NOT, Action<Process> after = null, DataReceivedEventHandler stdout = null, DataReceivedEventHandler stderr = null)
        {
            return Run("cmd", $"/C \"{cmd}\"", wait, after, stdout, stderr);
        }

        public int Run(string file, string args = "", int wait = WAIT_NOT, Action<Process> after = null, DataReceivedEventHandler stdout = null, DataReceivedEventHandler stderr = null)
        {
            Process p = Prepare(file, args, stdout != null || stderr != null);

            if(stdout != null) {
                p.OutputDataReceived += stdout;
            }

            if(stderr != null) {
                p.ErrorDataReceived += stderr;
            }

            p.Start();

            if(p.StartInfo.RedirectStandardOutput) {
                p.BeginOutputReadLine();
            }

            if(p.StartInfo.RedirectStandardError) {
                p.BeginErrorReadLine();
            }

            p.WaitForExit(wait);
            after?.Invoke(p);

            return p.HasExited ? p.ExitCode : -1;
        }

        public Caller(string slnDir)
        {
            workingDir = slnDir ?? throw new ArgumentNullException(nameof(slnDir));
        }

        protected Process Prepare(string file, string args, bool hidden)
        {
            var p = new Process();

            p.StartInfo.FileName            = file;
            p.StartInfo.Arguments           = args;
            p.StartInfo.WorkingDirectory    = workingDir;

            if(!hidden) {
                p.StartInfo.UseShellExecute = true;
                return p;
            }

            p.StartInfo.UseShellExecute         = false;
            p.StartInfo.WindowStyle             = ProcessWindowStyle.Hidden;
            p.StartInfo.CreateNoWindow          = true;
            p.StartInfo.RedirectStandardOutput  = true;
            p.StartInfo.RedirectStandardError   = true;
            return p;
        }
    }
}
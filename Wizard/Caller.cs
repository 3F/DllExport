/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
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
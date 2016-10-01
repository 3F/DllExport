/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016  Denis Kuzmin <entry.reg@gmail.com>
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
using System.Management.Automation;
using net.r_eg.Conari.Log;

namespace net.r_eg.DllExport.Configurator
{
    [Cmdlet(VerbsCommon.Set, "Configuration")]
    public class SetConfigurationCmdlet: Cmdlet, IScriptConfig
    {
        private object synch = new object();

        /// <summary>
        /// Full path to DllExport.dll
        /// </summary>
        [Parameter(Mandatory = true)]
        public string MetaLib
        {
            get;
            set;
        }

        /// <summary>
        /// Full path to package.
        /// </summary>
        [Parameter(Mandatory = true)]
        public string InstallPath
        {
            get;
            set;
        }

        /// <summary>
        /// Full path of package tools.
        /// </summary>
        [Parameter(Mandatory = true)]
        public string ToolsPath
        {
            get;
            set;
        }

        /// <summary>
        /// The instance of EnvDTE project.
        /// </summary>
        [Parameter(Mandatory = true)]
        public dynamic ProjectDTE
        {
            get;
            set;
        }

        /// <summary>
        /// The instance of Microsoft.Build.Evaluation.ProjectCollection.GlobalProjectCollection
        /// </summary>
        [Parameter(Mandatory = true)]
        public dynamic ProjectsMBE
        {
            get;
            set;
        }

        protected override void ProcessRecord()
        {
            IExecutor exec = new Executor(this);

            lock(synch) {
                exec.Log.Received -= onMsg;
                exec.Log.Received += onMsg;

                try {
                    exec.configure();
                }
                catch(Exception ex) {
                    LSender.Send(this, $"ERROR-GUI: {ex.Message}");
                }

                exec.Log.Received -= onMsg;
            }
        }

        private void onMsg(object sender, Message e)
        {
            WriteObject(e.content); //TODO:
        }
    }
}

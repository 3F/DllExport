/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2017  Denis Kuzmin < entry.reg@gmail.com > :: github.com/3F
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
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using net.r_eg.MvsSln.Log;

namespace net.r_eg.DllExport.Wizard
{
    public class DllExportCfgTask: Task, ITask, IWizardConfig
    {
        private object synch = new object();

        /// <summary>
        /// Path to directory with .sln files to be processed.
        /// </summary>
        [Required]
        public string SlnDir
        {
            get;
            set;
        }

        /// <summary>
        /// Root path of the DllExport package.
        /// </summary>
        [Required]
        public string PkgPath
        {
            get;
            set;
        }

        /// <summary>
        /// Raw type of operation via ActionType.
        /// </summary>
        [Required]
        public string Action
        {
            set
            {
                if(String.IsNullOrWhiteSpace(value)) {
                    Type = ActionType.Default;
                }
                value = value.Trim();

                Type = (ActionType)Enum.Parse(
                    typeof(ActionType), 
                    char.ToUpperInvariant(value[0]) + value.Substring(1).ToLowerInvariant()
                );
            }
        }

        /// <summary>
        /// The evaluated type of operation.
        /// </summary>
        public ActionType Type
        {
            get;
            set;
        }

        /// <summary>
        /// Executes the msbuild task.
        /// </summary>
        /// <returns>true if the task successfully executed.</returns>
        public override bool Execute()
        {
            if(String.IsNullOrWhiteSpace(SlnDir)) {
                throw new ArgumentNullException(nameof(SlnDir));
            }

            using(IExecutor exec = new Executor(this)) {
                return TryExecute(exec, exec.Configure);
            }
        }

        internal bool TryExecute(IExecutor exec, Action act)
        {
            lock(synch)
            {
                exec.Log.Received -= OnMsg;
                exec.Log.Received += OnMsg;

                try
                {
                    act();
                    return true;
                }
                catch(Exception ex)
                {
                    LSender.Send(this, $"ERROR-GUI: {ex.Message}");
                    LSender.Send(this, $"SlnDir: {SlnDir}");
                    LSender.Send(this, $"PkgPath: {PkgPath}");
                    LSender.Send(this, $"Action: {Type}");
#if DEBUG
                    LSender.Send(this, $"Stack trace: {ex.StackTrace}");
#endif
                }
                finally {
                    exec.Log.Received -= OnMsg;
                }

                return false;
            }
        }

        protected virtual void ConWrite(string message, Message.Level level)
        {
            if(level == Message.Level.Error || level == Message.Level.Fatal) {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if(level == Message.Level.Warn) {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void OnMsg(object sender, Message e)
        {
            ConWrite(e.content, e.type);
        }
    }
}

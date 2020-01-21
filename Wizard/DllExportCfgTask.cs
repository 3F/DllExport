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
using System.Globalization;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using net.r_eg.DllExport.Wizard.Extensions;
using net.r_eg.MvsSln.Log;

namespace net.r_eg.DllExport.Wizard
{
    public class DllExportCfgTask: Task, ITask, IWizardConfig, IDisposable
    {
        protected readonly string PTN_TIME = CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern + ".ffff";

        private UI.MsgForm uimsg;
        private readonly object sync = new object();

        /// <summary>
        /// Optional root path of user paths. 
        /// Affects on wSlnFile, wSlnDir, wPkgPath.
        /// </summary>
        public string RootPath
        {
            get => _rootPath;
            set => _rootPath = value.DirectoryPathFormat();
        }
        private string _rootPath;

        /// <summary>
        /// Path to directory with .sln files to be processed.
        /// </summary>
        [Required]
        public string SlnDir
        {
            get => _slnDir;
            set => _slnDir = value.DirectoryPathFormat(RootPath);
        }
        private string _slnDir;

        /// <summary>
        /// Optional predefined .sln file to process via the restore operations etc.
        /// </summary>
        public string SlnFile
        {
            get => _slnFile;
            set => _slnFile = value.FilePathFormat(RootPath);
        }
        private string _slnFile;

        /// <summary>
        /// Root path of the DllExport package.
        /// </summary>
        [Required]
        public string PkgPath
        {
            get => _pkgPath;
            set => _pkgPath = value.DirectoryPathFormat(RootPath);
        }
        private string _pkgPath;

        /// <summary>
        /// Relative path to meta library.
        /// </summary>
        [Required]
        public string MetaLib
        {
            get => _metaLib;
            set => _metaLib = value.FilePathFormat();
        }
        private string _metaLib;

        /// <summary>
        /// Relative path to meta core library.
        /// </summary>
        [Required]
        public string MetaCor
        {
            get => _metacor;
            set => _metacor = value.FilePathFormat();
        }
        private string _metacor;

        /// <summary>
        /// Path to .targets file of the DllExport.
        /// </summary>
        [Required]
        public string DxpTarget
        {
            get => _dxpTarget;
            set => _dxpTarget = value.FilePathFormat();
        }
        private string _dxpTarget;

        /// <summary>
        /// Arguments to manager.
        /// </summary>
        public string MgrArgs
        {
            get => _mgrArgs;
            // possible double quotes at least from 1.6.0 and 1.6.1
            set => _mgrArgs = value.OpenDoubleQuotes();
        }
        public string _mgrArgs;

        /// <summary>
        /// Version of the package that invokes target.
        /// </summary>
        public string PkgVer
        {
            get;
            set;
        }

        /// <summary>
        /// Path to external storage if used.
        /// </summary>
        public string StoragePath
        {
            get
            {
                if(_storagePath == null) {
                    _storagePath = TargetsFile.DEF_CFG_FILE;
                }
                return _storagePath;
            }
            set => _storagePath = value.FilePathFormat();
        }
        private string _storagePath;

        /// <summary>
        /// Raw storage type via CfgStorage. 
        /// </summary>
        public string Storage
        {
            set
            {
                if(String.IsNullOrWhiteSpace(value)) {
                    CfgStorage = CfgStorageType.Default;
                    return;
                }

                CfgStorage = (CfgStorageType)Enum.Parse(
                    typeof(CfgStorageType), 
                    value.Trim(), 
                    true
                );
            }
        }

        /// <summary>
        /// Where to store configuration data.
        /// </summary>
        public CfgStorageType CfgStorage
        {
            get;
            set;
        } = CfgStorageType.Default;

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
                    return;
                }

                Type = (ActionType)Enum.Parse(
                    typeof(ActionType),
                    value.Trim(),
                    //char.ToUpperInvariant(value[0]) + value.Substring(1).ToLowerInvariant()
                    false
                );
            }
        }

        /// <summary>
        /// The evaluated type of operation.
        /// </summary>
        public ActionType Type
        {
            get;
            protected set;
        }

        /// <summary>
        /// To show messages via GUI dlg for selected level (any positive number) and above.
        /// Levels: 0 - 5 (see Message.Level)
        /// '4' = means 4 (Error) + 5 (Fatal) levels.
        /// Any negative number disables this.
        /// It affects only for messages to GUI.
        /// </summary>
        public int MsgGuiLevel
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

            if(MsgGuiLevel >= 0)
            {
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    uimsg = new UI.MsgForm(MsgGuiLevel);
                    UI.App.RunSTA(uimsg);
                    uimsg = null;
                });
            }

            using(IExecutor exec = new Executor(this)) {
                return TryExecute(exec, exec.Configure);
            }
        }

        internal bool TryExecute(IExecutor exec, Action act)
        {
            lock(sync)
            {
                exec.Log.Received -= OnMsg;
                exec.Log.Received += OnMsg;

                LSender.Send(this, $"vW: {WizardVersion.S_INFO}", Message.Level.Info);
                PrintKeys(Message.Level.Debug);

                try
                {
                    act();
                    return true;
                }
                catch(Exception ex)
                {
                    LSender.Send(this, $"ERROR-Wizard: {ex.Message}", Message.Level.Fatal);
                    PrintKeys(Message.Level.Warn);
#if DEBUG
                    LSender.Send(this, $"Stack trace: {ex.StackTrace}", Message.Level.Error);
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
            if(level == Message.Level.Fatal) {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else if(level == Message.Level.Error) {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if(level == Message.Level.Warn) {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            if(IsLevelOrAbove(level, Message.Level.Error)) {
                Console.Error.WriteLine(message);
            }
            else {
                Console.WriteLine(message);
            }
            Console.ResetColor();
        }

        private void PrintKeys(Message.Level level)
        {
            LSender.Send(this, $"SlnDir: '{SlnDir}'", level);
            LSender.Send(this, $"SlnFile: '{SlnFile}'", level);
            LSender.Send(this, $"PkgPath: '{PkgPath}'", level);
            LSender.Send(this, $"MetaLib: '{MetaLib}'", level);
            LSender.Send(this, $"MetaCor: '{MetaCor}'", level);
            LSender.Send(this, $"MgrArgs: '{MgrArgs}'", level);
            LSender.Send(this, $"PkgVer:  '{PkgVer}'", level);
            LSender.Send(this, $"DxpTarget: '{DxpTarget}'", level);
            LSender.Send(this, $"RootPath: '{RootPath}'", level);
            LSender.Send(this, $"Storage: '{CfgStorage}'", level);
            LSender.Send(this, $"StoragePath: '{StoragePath}'", level);
            LSender.Send(this, $"Action: '{Type}'", level);
            LSender.Send(this, $"MsgGuiLevel: '{MsgGuiLevel}'", level);
        }

        private bool IsLevelOrAbove(Message.Level lvl1, Message.Level lvl2)
        {
            return ((int)lvl1) >= ((int)lvl2);
        }

        private void Free()
        {
            if(uimsg != null && !uimsg.IsDisposed) {
                uimsg.Dispose();
            }
        }

        private void OnMsg(object sender, Message e)
        {
            var msg = $"[{e.stamp.ToString(PTN_TIME)}] [{e.type}] {e.content}";

            uimsg?.AddMsg(msg, e.type);
            ConWrite(msg, e.type);
        }

        #region IDisposable
        private bool disposed = false;

        // To correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool _)
        {
            if(disposed) {
                return;
            }
            disposed = true;

            Free();
        }

        #endregion
    }
}

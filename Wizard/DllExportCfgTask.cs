/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Globalization;
using System.Reflection;
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
        private readonly string toolDir = Environment.CurrentDirectory;
        private readonly object sync = new object();

        #region ITask properties

        /// <inheritdoc cref="IWizardConfig.RootPath"/>
        public string RootPath { get; set; }

        /// <inheritdoc cref="IWizardConfig.SlnDir"/>
        public string SlnDir { get; set; }

        /// <inheritdoc cref="IWizardConfig.SlnFile"/>
        public string SlnFile { get; set; }

        /// <inheritdoc cref="IWizardConfig.PkgPath"/>
        public string PkgPath { get; set; }

        /// <inheritdoc cref="IWizardConfig.MetaLib"/>
        [Required]
        public string MetaLib { get; set; }

        /// <inheritdoc cref="IWizardConfig.MetaCor"/>
        [Required]
        public string MetaCor { get; set; }

        /// <inheritdoc cref="IWizardConfig.DxpTarget"/>
        [Required]
        public string DxpTarget { get; set; }

        /// <inheritdoc cref="IWizardConfig.MgrArgs"/>
        public string MgrArgs { get; set; }

        /// <inheritdoc cref="IWizardConfig.PkgVer"/>
        public string PkgVer { get; set; }

        /// <inheritdoc cref="IWizardConfig.Proxy"/>
        public string Proxy { get; set; }

        /// <inheritdoc cref="IWizardConfig.MsgGuiLevel"/>
        public int MsgGuiLevel { get; set; }

        /// <inheritdoc cref="IWizardConfig.StoragePath"/>
        public string StoragePath { get; set; }

        /// <summary>
        /// Updates an action type for <see cref="IWizardConfig.Type"/> using raw value.
        /// </summary>
        [Required]
        public string Action
        {
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    Type = ActionType.Default;
                    return;
                }

                Type = (ActionType)Enum.Parse
                (
                    typeof(ActionType),
                    value.Trim(),
                    false
                );
            }
        }

        /// <summary>
        /// Updates storage type for <see cref="IWizardConfig.CfgStorage"/> using raw value.
        /// </summary>
        public string Storage
        {
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    CfgStorage = CfgStorageType.Default;
                    return;
                }

                CfgStorage = (CfgStorageType)Enum.Parse
                (
                    typeof(CfgStorageType), 
                    value.Trim(), 
                    true
                );
            }
        }

        /// <summary>
        /// Updates type for <see cref="Options"/>.
        /// </summary>
        public long DxpOpt
        {
            set => Options = (DxpOptType)value;
        }

        #endregion

        /// <inheritdoc cref="IWizardConfig.Distributable"/>
        public bool Distributable => !string.IsNullOrWhiteSpace(PkgVer) && PkgVer[0] != '-';

        /// <inheritdoc cref="IWizardConfig.PackageType"/>
        public string PackageType => (string.IsNullOrWhiteSpace(PkgVer) || PkgVer.Equals("actual", StringComparison.OrdinalIgnoreCase)) ? "offline" : PkgVer;

        /// <inheritdoc cref="IWizardConfig.CfgStorage"/>
        public CfgStorageType CfgStorage { get; set; } = CfgStorageType.Default;

        /// <inheritdoc cref="IWizardConfig.Options"/>
        public DxpOptType Options { get; protected set; }

        /// <inheritdoc cref="IWizardConfig.Type"/>
        public ActionType Type { get; protected set; }

        /// <summary>
        /// Executes the msbuild task.
        /// </summary>
        /// <returns>true if the task successfully executed.</returns>
        public override bool Execute()
        {
            UpdateMSBuildValues();

            if(string.IsNullOrWhiteSpace(SlnDir)) {
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

                LSender.Send(this, $"vW: {DllExportVersion.S_INFO}", Message.Level.Info);
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

        private void UpdateMSBuildValues()
        {
            RootPath    = (RootPath ?? toolDir.FindUpDirUsingFile("*.sln")).DirectoryPathFormat();

            SlnDir      = SlnDir.DirectoryPathFormat(RootPath);
            SlnFile     = SlnFile.FilePathFormat(RootPath);

            PkgPath     = (PkgPath ?? toolDir.FindUpDirUsingFile("*.nuspec") ?? toolDir.UpDir())
                            .DirectoryPathFormat(RootPath);

            MetaLib     = MetaLib.FilePathFormat();
            MetaCor     = MetaCor.FilePathFormat();
            DxpTarget   = DxpTarget.FilePathFormat();
            PkgVer      = PkgVer?.Trim();
            StoragePath = StoragePath.FilePathFormat() ?? TargetsFile.DEF_CFG_FILE;

            // possible double quotes at least from 1.6.0 and 1.6.1
            MgrArgs = MgrArgs.OpenDoubleQuotes();
        }

        private void PrintKeys(Message.Level level)
        {
            LSender.Send(this, $"Instance: '{Assembly.GetEntryAssembly().Location}'", level);
            LSender.Send(this, $"Build version: '{DllExportVersion.S_INFO_P}'", level);
            LSender.Send(this, $"SlnDir: '{SlnDir}'", level);
            LSender.Send(this, $"SlnFile: '{SlnFile}'", level);
            LSender.Send(this, $"PkgPath: '{PkgPath}'", level);
            LSender.Send(this, $"MetaLib: '{MetaLib}'", level);
            LSender.Send(this, $"MetaCor: '{MetaCor}'", level);
            LSender.Send(this, $"MgrArgs: '{MgrArgs}'", level);
            LSender.Send(this, $"PkgVer:  '{PkgVer}'", level);
            LSender.Send(this, $"Proxy:   '{Proxy}'", level);
            LSender.Send(this, $"DxpTarget: '{DxpTarget}'", level);
            LSender.Send(this, $"RootPath: '{RootPath}'", level);
            LSender.Send(this, $"Options:  '{Options}'", level);
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

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool _)
        {
            if(!disposed)
            {
                Free();
                disposed = true;
            }
        }

        #endregion
    }
}

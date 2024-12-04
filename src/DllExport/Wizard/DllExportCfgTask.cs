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
using net.r_eg.MvsSln.Core.SlnHandlers;
using net.r_eg.MvsSln.Log;

namespace net.r_eg.DllExport.Wizard
{
    public class DllExportCfgTask: Task, ITask, IWizardConfig, IDisposable
    {
        protected readonly string PTN_TIME = CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern + ".ffff";

        private UI.MsgForm uimsg;
        private readonly string toolDir = Environment.CurrentDirectory;
        private readonly object sync = new();

        #region ITask properties

        public string RootPath { get; set; }

        public string SlnDir { get; set; }

        public string SlnFile { get; set; }

        public string PkgPath { get; set; }

        [Required]
        public string MetaLib { get; set; }

        [Required]
        public string MetaCor { get; set; }

        [Required]
        public string DxpTarget { get; set; }

        public string MgrArgs { get; set; }

        public string PkgVer { get; set; }

        public string Proxy { get; set; }

        public int MsgGuiLevel { get; set; }

        public string StoragePath { get; set; }

        /// <summary>
        /// Updates an action type for <see cref="IWizardConfig.Type"/> using raw value.
        /// </summary>
        [Required]
        public string Action
        {
            set => Type = ParseEnum(value, or: ActionType.Default);
        }

        /// <summary>
        /// Updates storage type for <see cref="IWizardConfig.CfgStorage"/> using raw value.
        /// </summary>
        public string Storage
        {
            set => CfgStorage = ParseEnum(value, or: CfgStorageType.Default);
        }

        /// <summary>
        /// Updates type for <see cref="Options"/>.
        /// </summary>
        public long DxpOpt
        {
            set => Options = (DxpOptType)value;
        }

        public string MsgLevel
        {
            set => MsgLevelLimit = ParseEnum(value, or: Message.Level.Trace);
        }

        #endregion

        public bool Distributable => !string.IsNullOrWhiteSpace(PkgVer) && PkgVer[0] != '-';

        public string PackageType => (string.IsNullOrWhiteSpace(PkgVer) || PkgVer.Equals("actual", StringComparison.OrdinalIgnoreCase)) ? "offline" : PkgVer;

        public CfgStorageType CfgStorage { get; set; } = CfgStorageType.Default;

        public DxpOptType Options { get; protected set; }

        public ActionType Type { get; protected set; }

        internal Message.Level MsgLevelLimit { get; set; }

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

                LSender.Send(this, $"vW: {DllExportVersion.S_INFO_FULL}", Message.Level.Info);
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
            if(!IsLevelOrAbove(level, MsgLevelLimit)) return;

            Console.ForegroundColor = level switch
            {
                Message.Level.Fatal => ConsoleColor.Cyan,
                Message.Level.Error => ConsoleColor.Red,
                Message.Level.Warn => ConsoleColor.Yellow,
                Message.Level.Debug => ConsoleColor.DarkGray,
                Message.Level.Trace => ConsoleColor.DarkGray,
                _ => ConsoleColor.Gray,
            };

            if(IsLevelOrAbove(level, Message.Level.Error))
            {
                Console.Error.WriteLine(message);
            }
            else
            {
                Console.WriteLine(message);
            }

            if(Console.ForegroundColor != ConsoleColor.Gray)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
            }
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
            LSender.Send(this, $"MsgLevel: '{MsgLevelLimit}'", level);
            LSender.Send(this, $"MsgGuiLevel: '{MsgGuiLevel}'", level);
        }

        private static T ParseEnum<T>(string input, T or = default, bool icase = true)
        {
            if(string.IsNullOrWhiteSpace(input)) return or;

            return (T)Enum.Parse
            (
                typeof(T),
                input.Trim(),
                ignoreCase: icase
            );
        }

        private static bool IsLevelOrAbove(Message.Level lvl1, Message.Level lvl2)
        {
            return ((int)lvl1) >= ((int)lvl2);
        }

        private static bool IgnoreSender(object sender, Message.Level level) => sender switch
        {
            MvsSln.Core.SynchSubscribers<ISlnHandler> => true,
            LProjectConfigurationPlatforms => IsLevelOrAbove(Message.Level.Debug, level),
            _ => false
        };

        private static Message.Level ChangeLevel(Message.Level level, object sender) => sender switch
        {
            LSolutionConfigurationPlatforms => Message.Level.Trace,
            LProjectConfigurationPlatforms => level == Message.Level.Info ? Message.Level.Trace : level,
            ISlnHandler => level == Message.Level.Debug ? Message.Level.Trace : level,
            _ => level
        };

        private void OnMsg(object sender, Message e)
        {
            if(IgnoreSender(sender, e.type)) return;
            Message.Level level = ChangeLevel(e.type, sender);

            string msg = $"[{e.stamp.ToString(PTN_TIME)}] [{level}] {e.content}";

            uimsg?.AddMsg(msg, level);
            ConWrite(msg, level);
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
                if(uimsg?.IsDisposed == false) uimsg.Dispose();

                disposed = true;
            }
        }

        #endregion
    }
}

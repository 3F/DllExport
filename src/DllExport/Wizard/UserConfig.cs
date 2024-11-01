﻿/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using System.Linq;
using net.r_eg.DllExport.NSBin;
using net.r_eg.DllExport.Wizard.Extensions;
using net.r_eg.MvsSln.Core;
using net.r_eg.MvsSln.Log;
using net.r_eg.DllExport;
using PostProcType = net.r_eg.DllExport.Wizard.PostProc.CmdType;
using PreProcType = net.r_eg.DllExport.Wizard.PreProc.CmdType;

namespace net.r_eg.DllExport.Wizard
{
    public class UserConfig: IUserConfig
    {
        /// <summary>
        /// https://github.com/3F/DllExport/issues/2#issue-164662993
        /// </summary>
        public const string NS_DEFAULT_VALUE = "System.Runtime.InteropServices";

        /// <summary>
        /// Default meta library.
        /// </summary>
        public const string METALIB_NAME = "DllExport.dll";

        //TODO: 
        internal const string MGR_NAME = "DllExport";
        internal const string MGR_FILE = MGR_NAME + ".bat";
        internal const string PKG_ID = "DllExport";

        /// <summary>
        /// Flag of installation.
        /// </summary>
        public bool Install
        {
            get;
            set;
        }

        /// <summary>
        /// A selected namespace for ddNS feature.
        /// </summary>
        public string Namespace
        {
            get;
            set;
        }

        /// <summary>
        /// Allowed buffer size for NS.
        /// </summary>
        public int NSBuffer
        {
            get;
            set;
        }

        /// <summary>
        /// To use Cecil instead of direct modifications.
        /// </summary>
        public bool UseCecil
        {
            get;
            set;
        }

        /// <summary>
        /// Predefined list of namespaces.
        /// </summary>
        public List<string> Namespaces
        {
            get;
            set;
        }

        /// <summary>
        /// Access to wizard configuration.
        /// </summary>
        public IWizardConfig Wizard
        {
            get;
            set;
        }

        /// <summary>
        /// Access to ddNS core.
        /// </summary>
        public IDDNS DDNS
        {
            get;
            set;
        }

        /// <summary>
        /// Specific logger.
        /// </summary>
        public ISender Log
        {
            get;
            set;
        }

        /// <summary>
        /// To use this target platform.
        /// </summary>
        public Platform Platform
        {
            get;
            set;
        }

        /// <summary>
        /// Settings for ILasm etc.
        /// </summary>
        public CompilerCfg Compiler
        {
            get;
            set;
        }

        /// <summary>
        /// Access to Pre-Processing.
        /// </summary>
        public PreProc PreProc
        {
            get;
            set;
        }

        /// <summary>
        /// Access to Post-Processing.
        /// </summary>
        public PostProc PostProc
        {
            get;
            set;
        }

        /// <summary>
        /// Adds to top new namespace into Namespaces property.
        /// </summary>
        /// <param name="ns"></param>
        /// <returns>true if added.</returns>
        public bool AddTopNamespace(string ns)
        {
            if(!String.IsNullOrWhiteSpace(ns) && !Namespaces.Contains(ns)) {
                Namespaces.Insert(0, ns);
                return true;
            }
            return false;
        }

        public UserConfig(IWizardConfig cfg)
        {
            Wizard = cfg ?? throw new ArgumentNullException(nameof(cfg));

            Namespaces = new List<string>() {
                NS_DEFAULT_VALUE,
                "net.r_eg.DllExport",
                "com.github._3F.DllExport",
                string.Empty, //https://github.com/3F/DllExport/issues/47
            };
        }

        public UserConfig(IWizardConfig cfg, IXProject xp)
            : this(cfg)
        {
            if(xp == null) {
                return;
            }

            Namespace   = GetValue(MSBuildProperties.DXP_NAMESPACE, xp);
            Platform    = GetPlatform(xp);
            UseCecil    = GetValue(MSBuildProperties.DXP_DDNS_CECIL, xp).ToBoolean();

            var rawTimeout = GetValue(MSBuildProperties.DXP_TIMEOUT, xp);

            Compiler = new CompilerCfg() {
                genExpLib           = GetValue(MSBuildProperties.DXP_GEN_EXP_LIB, xp).ToBoolean(),
                ordinalsBase        = GetValue(MSBuildProperties.DXP_ORDINALS_BASE, xp).ToInteger(),
                ourILAsm            = GetValue(MSBuildProperties.DXP_OUR_ILASM, xp).ToBoolean(),
                customILAsm         = GetUnevaluatedValue(MSBuildProperties.DXP_CUSTOM_ILASM, xp),
                rSysObj             = GetValue(MSBuildProperties.DXP_SYSOBJ_REBASE, xp).ToBoolean(),
                intermediateFiles   = GetValue(MSBuildProperties.DXP_INTERMEDIATE_FILES, xp).ToBoolean(),
                timeout             = String.IsNullOrWhiteSpace(rawTimeout) ? CompilerCfg.TIMEOUT_EXEC : rawTimeout.ToInteger(),
                peCheck             = (PeCheckType)GetValue(MSBuildProperties.DXP_PE_CHECK, xp).ToInteger(),
                patches             = (PatchesType)GetValue(MSBuildProperties.DXP_PATCHES, xp).ToLongInteger()
            };

            var preType = (PreProcType)GetValue(MSBuildProperties.DXP_PRE_PROC_TYPE, xp).ToLongInteger();
            PreProc = new PreProc().Configure(preType, GetPreProcCmd(preType, xp));

            var postType = (PostProcType)GetValue(MSBuildProperties.DXP_POST_PROC_TYPE, xp).ToLongInteger();
            PostProc = new PostProc().Configure
            (
                postType,
                GetPostProcEnv(postType, xp),
                GetPostProcCmd(postType, xp)
            );
        }

        public UserConfig(IConfigInitializer cfg)
            : this(cfg, null)
        {

        }

        public UserConfig(IConfigInitializer cfg, IXProject project)
            : this(cfg?.Config, project)
        {
            if(cfg?.DDNS != null) {
                NSBuffer    = cfg.DDNS.NSBuffer;
                DDNS        = cfg.DDNS;
            }
            Log = cfg.Log;
        }

        protected Platform GetPlatform(IXProject project)
        {
            if(GetUnevaluatedValue(MSBuildProperties.DXP_PLATFORM, project) == Platform.Auto.ToString()) {
                return Platform.Auto;
            }
            return GetPlatform(GetValue(MSBuildProperties.PRJ_PLATFORM, project));
        }

        protected Platform GetPlatform(string value)
        {
            if(String.IsNullOrWhiteSpace(value)) {
                return Platform.Default;
            }

            switch(value.Trim().ToLowerInvariant())
            {
                case "x86":
                case "x32":
                case "win32": {
                    return Platform.x86;
                }
                case "x64": {
                    return Platform.x64;
                }
                case "anycpu":
                case "any cpu": {
                    return Platform.AnyCPU;
                }
            }

            LSender.Send(this, $"Incorrect platform target: '{value}'. Use '{nameof(Platform.Default)}'", Message.Level.Warn);
            return Platform.Default;
        }

        protected string GetPreProcCmd(PreProcType type, IXProject xp)
        {
            if((type & PreProcType.ILMerge) == PreProcType.ILMerge)
            {
                return GetUnevaluatedValue(MSBuildProperties.DXP_ILMERGE, xp);
            }

            if((type & PreProcType.Exec) == PreProcType.Exec)
            {
                var tExec = xp?.Project.Xml?.Targets
                            .FirstOrDefault(t => t.Name == MSBuildTargets.DXP_PRE_PROC && t.Label == Project.METALIB_PK_TOKEN)?
                            .Tasks
                            .FirstOrDefault(t => t.Name == "Exec");

                if(tExec != null)
                {
                    return tExec.GetParameter("Command");
                }
            }

            return null;
        }

        protected string GetPostProcEnv(PostProcType type, IXProject xp)
        {
            if(type != PostProcType.None) {
                return GetUnevaluatedValue(MSBuildProperties.DXP_PROC_ENV, xp);
            }
            return null;
        }

        protected string GetPostProcCmd(PostProcType type, IXProject xp)
        {
            if(type == PostProcType.None) {
                return null;
            }

            //TODO: 
            return "...";
        }

        private string GetValue(string property, IXProject project)
        {
            return project?.GetPropertyValue(property);
        }

        private string GetUnevaluatedValue(string property, IXProject project)
        {
            return project?.GetUnevaluatedPropertyValue(property);
        }
    }
}

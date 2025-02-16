/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using net.r_eg.DllExport.ILAsm;
using net.r_eg.DllExport.NSBin;
using net.r_eg.DllExport.Wizard.Extensions;
using net.r_eg.MvsSln.Core;
using net.r_eg.MvsSln.Log;
using PostProcType = net.r_eg.DllExport.Wizard.PostProc.CmdType;
using PreProcType = net.r_eg.DllExport.Wizard.PreProc.CmdType;

namespace net.r_eg.DllExport.Wizard
{
    public class UserConfig(IWizardConfig cfg): IUserConfig
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

        public bool Install { get; set; }

        public string Namespace { get; set; }

        public int NSBuffer { get; set; }

        public bool UseCecil { get; set; }

        public List<string> Namespaces { get; set; } =
        [
            NS_DEFAULT_VALUE,
            "net.r_eg.DllExport",
            "com.github._3F.DllExport",
            string.Empty, //https://github.com/3F/DllExport/issues/47
        ];

        public IWizardConfig Wizard { get; set; } = cfg ?? throw new ArgumentNullException(nameof(cfg));

        public IDDNS DDNS { get; set; }

        public ISender Log { get; set; }

        public Platform Platform { get; set; }

        public CompilerCfg Compiler { get; set; }

        public PreProc PreProc { get; set; }

        public PostProc PostProc { get; set; }

        public List<ILAsm.AssemblyExternDirective> AssemblyExternDirectives { get; set; }

        public List<ILAsm.TypeRefDirective> TypeRefDirectives { get; set; }

        public TypeRefOptions TypeRefOptions { get; set; }

        public List<RefPackage> RefPackages { get; set; }

        public bool AddTopNamespace(string ns)
        {
            if(!string.IsNullOrWhiteSpace(ns) && !Namespaces.Contains(ns))
            {
                Namespaces.Insert(0, ns);
                return true;
            }
            return false;
        }

        public void UpdateDataFrom(IXProject xp)
        {
            if(xp == null) return;

            Namespace   = GetValue(MSBuildProperties.DXP_NAMESPACE, xp);
            Platform    = GetPlatform(xp);
            UseCecil    = GetValue(MSBuildProperties.DXP_DDNS_CECIL, xp).ToBoolean();

            string rawTimeout = GetValue(MSBuildProperties.DXP_TIMEOUT, xp);

            Compiler = new()
            {
                genExpLib           = GetValue(MSBuildProperties.DXP_GEN_EXP_LIB, xp).ToBoolean(),
                ordinalsBase        = GetValue(MSBuildProperties.DXP_ORDINALS_BASE, xp).ToInteger(),
                ourILAsm            = GetValue(MSBuildProperties.DXP_OUR_ILASM, xp).ToBoolean(),
                customILAsm         = GetUnevaluatedValue(MSBuildProperties.DXP_CUSTOM_ILASM, xp),
                rSysObj             = GetValue(MSBuildProperties.DXP_SYSOBJ_REBASE, xp).ToBoolean(),
                intermediateFiles   = GetValue(MSBuildProperties.DXP_INTERMEDIATE_FILES, xp).ToBoolean(),
                timeout             = string.IsNullOrWhiteSpace(rawTimeout) ? CompilerCfg.TIMEOUT_EXEC : rawTimeout.ToInteger(),
                peCheck             = (PeCheckType)GetValue(MSBuildProperties.DXP_PE_CHECK, xp).ToInteger(),
                patches             = (PatchesType)GetValue(MSBuildProperties.DXP_PATCHES, xp).ToLongInteger(),
                refreshObj          = GetValue(MSBuildProperties.DXP_REFRESH_OBJ, xp).ToBoolean(),
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

            AssemblyExternDirectives = new List<AssemblyExternDirective>
                (GetValue(MSBuildProperties.DXP_ILASM_EXTERN_ASM, xp).Deserialize<AssemblyExternDirective>());

            TypeRefDirectives = new List<TypeRefDirective>
                (GetValue(MSBuildProperties.DXP_ILASM_TYPEREF, xp).Deserialize<TypeRefDirective>());

            TypeRefOptions = (TypeRefOptions)GetValue(MSBuildProperties.DXP_TYPEREF_OPTIONS, xp).ToLongInteger();

            RefPackages = new List<RefPackage>
                (GetValue(MSBuildProperties.DXP_REF_PACKAGES, xp).Deserialize<RefPackage>());
        }

        public bool ValidateAssemblyExternDirectives(Func<string, bool> onFailed)
        {
            if(AssemblyExternDirectives == null || onFailed == null || AssemblyExternDirectives.Count < 1) return true;

            static string _At(int index) => $"at #{index + 1} position";
            for(int i = 0; i < AssemblyExternDirectives.Count; ++i)
            {
                AssemblyExternDirective d = AssemblyExternDirectives[i];

                if(string.IsNullOrWhiteSpace(d.Name))
                    return onFailed($".assembly extern 'name' is not defined {_At(i)}");

                if(d.Version == null || !Regex.IsMatch(d.Version, @"\d+:\d+:\d+:\d+"))
                    return onFailed($"Incorrect 'version' format {_At(i)}");

                if(d.Publickeytoken == null || !Regex.IsMatch(d.Publickeytoken, @"^[0-9A-F\s]+$", RegexOptions.IgnoreCase))
                    return onFailed($"Incorrect '.publickeytoken' format {_At(i)}");
            }
            return true;
        }

        public bool ValidateTypeRefDirectives(Func<string, bool> onFailed)
        {
            if(TypeRefDirectives == null || onFailed == null || TypeRefDirectives.Count < 1) return true;

            static string _At(int index) => $"at #{index + 1} position";
            for(int i = 0; i < TypeRefDirectives.Count; ++i)
            {
                TypeRefDirective d = TypeRefDirectives[i];

                if(string.IsNullOrWhiteSpace(d.ResolutionScope) && !d.Assert && !d.Deny)
                    return onFailed($"ResolutionScope is not defined {_At(i)}");
            }
            return true;
        }

        public bool ValidateRefPackages(Func<string, bool> onFailed)
        {
            if(RefPackages == null || onFailed == null || RefPackages.Count < 1) return true;

            static string _At(int index) => $"at #{index + 1} position";
            for(int i = 0; i < RefPackages.Count; ++i)
            {
                RefPackage r = RefPackages[i];

                if(r.Name == null || !Regex.IsMatch(r.Name, /*nuget core based rule:*/@"^\w+(?:[_.-]\w+)*$"))
                    return onFailed($"Incorrect package name {_At(i)}");

                if(string.IsNullOrWhiteSpace(r.TfmOrPath))
                    return onFailed($"Tfm or path is not defined {_At(i)}");
            }
            return true;
        }

        public UserConfig(IWizardConfig cfg, IXProject xp)
            : this(cfg)
        {
            UpdateDataFrom(xp);
        }

        public UserConfig(IConfigInitializer cfg)
            : this(cfg, null)
        {

        }

        public UserConfig(IConfigInitializer cfg, IXProject project)
            : this(cfg?.Config, project)
        {
            if(cfg?.DDNS != null)
            {
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
            if((type & (PreProcType.ILMerge | PreProcType.ILRepack)) != 0)
            {
                return GetUnevaluatedValue(MSBuildProperties.DXP_ILMERGE, xp);
            }

            if((type & PreProcType.Exec) == PreProcType.Exec)
            {
                var tExec = xp?.Project.Xml?.Targets
                    .FirstOrDefault
                    (t =>
                        t.Name == MSBuildTargets.DXP_PRE_PROC
                        &&
                        (
                            t.Label == PreProc.ID
                            || t.Label == Project.METALIB_PK_TOKEN // for 1.7.3 or less
                        )
                    )?
                    .Tasks.FirstOrDefault(t => t.Name == "Exec");

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

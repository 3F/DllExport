/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Build.Construction;
using net.r_eg.DllExport.ILAsm;
using net.r_eg.DllExport.NSBin;
using net.r_eg.DllExport.Wizard.Extensions;
using net.r_eg.DllExport.Wizard.Gears;
using net.r_eg.MvsSln.Core;
using net.r_eg.MvsSln.Extensions;
using net.r_eg.MvsSln.Log;
using net.r_eg.MvsSln.Projects;

namespace net.r_eg.DllExport.Wizard
{
    public class Project: IProject, IProjectSvc
    {
        public const string DXP_INVALID = "EEE00000-0000-0000-0000-000000000000";

        /// <summary>
        /// PublicKeyToken of the meta library.
        /// </summary>
        public const string METALIB_PK_TOKEN = "8337224c9ad9e356";

        /// <summary>
        /// The name of the default .target file.
        /// </summary>
        public const string DXP_TARGET = "net.r_eg.DllExport.targets";

        private const string WZ_ID = "Wz";

        private readonly IEnumerable<IProjectGear> gears;
        private readonly PackagesConfig pkgconf;
        private string _dxpIdent;

        public IXProject XProject { get; protected set; }

        public bool Installed
            => InternalError == null 
            && !string.IsNullOrWhiteSpace(GetProperty(MSBuildProperties.DXP_ID));

        public string InternalError => XProject?.GetProperty(DxpIsolatedEnv.ERR_MSG, true).evaluated;

        public string DxpIdent
        {
            get
            {
                if(_dxpIdent != null) return _dxpIdent;

                _dxpIdent = GetProperty(MSBuildProperties.DXP_ID);
                if(string.IsNullOrWhiteSpace(_dxpIdent))
                {
                    _dxpIdent = Guid.NewGuid().ToString().ToUpperInvariant();
                    Log.send(this, $"Generated new identifier: '{_dxpIdent}'", Message.Level.Trace);
                }
                return _dxpIdent;
            }
            protected set => _dxpIdent = value;
        }

        public string ProjectPath
        {
            get
            {
                if(XProject == null) return null;

                return XProject.ProjectItem.project.path ??
                            MakeBasePath(XProject.ProjectFullPath, false);
            }
        }

        public string ProjectNamespace => GetProperty(MSBuildProperties.PRJ_NAMESPACE);

        public bool HasExternalStorage => XProject?.GetImports(project: null, Guids.X_EXT_STORAGE).Any() == true;

        public IUserConfig Config { get; set; }

        public IDictionary<string, string> ConfigProperties { get; private set; } 
            = new Dictionary<string, string>();

        public bool PublicKeyTokenLimit { get; set; } = true;

        protected ISender Log => Config?.Log ?? LSender._;

        public virtual string MetaLib(bool evaluate, bool corlib = false)
        {
            string mdll = GetMetaDll(corlib);

            return Path.GetFullPath
            (
                Path.Combine
                (
                    Config.Wizard.PkgPath,
                    "gcache",
                    evaluate ? corlib ? "metacor" : "metalib" 
                                    : "$(DllExportMetaXBase)",

                    (evaluate && Config?.Namespace != null) ? 
                        Config.Namespace : $"$({MSBuildProperties.DXP_NAMESPACE})",

                    (evaluate && mdll != null) ? 
                        Path.GetFileName(mdll) : $"$({MSBuildProperties.DXP_METALIB_NAME})"
                )
            );
        }

        public void Recover(string id, ActionType type = ActionType.Recover, IXProject xp = null)
        {
            if(string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));

            if(xp != null && type == ActionType.RecoverInit)
            {
                xp.Project.SetGlobalProperty(MSBuildProperties.DXP_ID, id);
                xp.Reevaluate();
                Config.UpdateDataFrom(xp);
            }

            DxpIdent = id;
            ActionConfigure(force: true);
        }

        public void Unset()
        {
            Log.send(this, $"Attempt to unset data from '{DxpIdent}'", Message.Level.Info);
            Reset(hardReset: true);
            Save();
        }

        public virtual bool Configure(ActionType type)
        {
            switch(type) {
                case ActionType.Restore: {
                    ActionRestore();
                    break;
                }
                case ActionType.Update: {
                    ActionUpdate();
                    break;
                }
                case ActionType.Configure: {
                    ActionConfigure();
                    break;
                }
            }

            return true;
        }

        #region IProjectSvc

        ProjectTargetElement IProjectSvc.AddTarget(string name) => AddTarget(name);
        bool IProjectSvc.RemoveXmlTarget(string name) => XProject.RemoveXmlTarget(name);
        void IProjectSvc.SetProperty(string name, string value) => SetProperty(name, value);
        void IProjectSvc.SetProperty(string name, bool val) => SetProperty(name, val);
        void IProjectSvc.SetProperty(string name, int val) => SetProperty(name, val);
        void IProjectSvc.SetProperty(string name, long val) => SetProperty(name, val);
        IProjectSvc IProjectSvc.RemovePackageReferences(string id, Func<Item, bool> opt, bool wzstrict) => RemovePackageReferences(id, opt, wzstrict);
        IEnumerable<KeyValuePair<string, string>> IProjectSvc.GetMeta(bool privateAssets, bool hide, bool generatePath) => Meta.Get(privateAssets, hide, generatePath);

        #endregion

        public Project(IXProject xproject, IConfigInitializer init)
            : this(xproject)
        {
            Config = GetUserConfig(xproject, init);
            Config.AddTopNamespace(ProjectNamespace);
        }

        public Project(IXProject xproject)
        {
            XProject = xproject ?? throw new ArgumentNullException(nameof(xproject));

            gears =
            [
                new PreProcGear(this),
                new PostProcGear(this),
            ];

            pkgconf = new PackagesConfig
            (
                xproject.ProjectPath,
                PackagesConfigOptions.LoadOrNew
                    | PackagesConfigOptions.AutoCommit
                    | PackagesConfigOptions.SilentLoading
            );

            AllocateProperties
            (
                MSBuildProperties.DXP_ID,
                MSBuildProperties.DXP_METALIB_NAME,
                MSBuildProperties.DXP_NAMESPACE,
                MSBuildProperties.DXP_ORDINALS_BASE,
                MSBuildProperties.DXP_SKIP_ANYCPU,
                MSBuildProperties.DXP_DDNS_CECIL,
                MSBuildProperties.DXP_GEN_EXP_LIB,
                MSBuildProperties.DXP_SYSOBJ_REBASE,
                MSBuildProperties.DXP_OUR_ILASM,
                MSBuildProperties.DXP_CUSTOM_ILASM,
                MSBuildProperties.DXP_INTERMEDIATE_FILES,
                MSBuildProperties.DXP_TIMEOUT,
                MSBuildProperties.DXP_PE_CHECK,
                MSBuildProperties.DXP_PATCHES,
                MSBuildProperties.DXP_REFRESH_OBJ,
                MSBuildProperties.DXP_PLATFORM,
                MSBuildProperties.DXP_PRE_PROC_TYPE,
                MSBuildProperties.DXP_ILMERGE,
                MSBuildProperties.DXP_POST_PROC_TYPE,
                MSBuildProperties.DXP_PROC_ENV,
                MSBuildProperties.DXP_DIR,
                MSBuildProperties.DXP_ILASM_EXTERN_ASM,
                MSBuildProperties.DXP_ILASM_TYPEREF,
                MSBuildProperties.DXP_REF_PACKAGES,
                MSBuildProperties.DXP_TYPEREF_OPTIONS
            );
            AllocPlatformTargetIfNeeded(xproject);

            Log.send(this, $"{DxpIdent} - {ProjectPath}", Message.Level.Info);
        }

        protected void InstallGears() => gears.ForEach(g => g.Install());

        protected void UninstallGears(bool hardReset) => gears.ForEach(g => g.Uninstall(hardReset));

        protected void ActionRestore()
        {
            if(Installed)
            {
                CfgDDNS();
            }
        }

        protected void ActionUpdate()
        {
            Reset(hardReset: false);

            if(Installed)
            {
                CfgDDNS();
                ActionStorage(Config.Wizard.CfgStorage);
            }

            Save();
        }

        protected void ActionConfigure(bool force = false)
        {
            Reset(hardReset: true);

            if(Config.Install || force)
            {
                SetProperty(MSBuildProperties.DXP_ID, DxpIdent);

                if(Config.Wizard.CfgStorage != CfgStorageType.TargetsFile) {
                    CfgCommonData();
                }

                XProject.SetProperties(ConfigProperties);
                XProject.Reevaluate();

                ActionStorage(Config.Wizard.CfgStorage);
            }

            Save();
        }

        protected void ActionStorage(CfgStorageType type)
        {
            if(type == CfgStorageType.TargetsFile) {
                AddExternalStorage();
            }
            else {
                AddDllExportLib();
            }

            XProject.Reevaluate();
        }

        protected IUserConfig GetUserConfig(IXProject project, IConfigInitializer cfg)
        {
            if(Installed) {
                return new UserConfig(cfg, project);
            }

            return new UserConfig(cfg)
            {
                UseCecil    = true,
                Platform    = Platform.Auto,
                Compiler    = new CompilerCfg() 
                {
                    ordinalsBase    = 1,
                    timeout         = CompilerCfg.TIMEOUT_EXEC,
                    peCheck         = PeCheckType.PeIl | PeCheckType.Pe32orPlus,
                    patches         = PatchesType.None,
                },
                PreProc     = new PreProc().Configure(),
                PostProc    = new PostProc().Configure(null),
            };
        }

        /// <summary>
        /// Saves the project to the file system, if modified.
        /// </summary>
        protected void Save()
        {
            if(XProject?.ProjectFullPath != null && InternalError == null)
            {
                XProject.Save();
                Log.send(this, $"'{ProjectPath}' completed (Installed: {Config?.Install}) {Config?.Namespace} : {DxpIdent}", Message.Level.Info);
            }
        }

        protected void CfgDDNS()
        {
            CfgDDNS(corlib: false);
            CfgDDNS(corlib: true);
        }

        protected void CfgDDNS(bool corlib)
        {
            Config.DDNS.SetNamespace
            (
                CopyLib
                (
                    Path.Combine(Config.Wizard.PkgPath, GetMetaDll(corlib)), 
                    MetaLib(evaluate: true, corlib)
                ), 
                Config.Namespace, 
                Config.UseCecil,
                preparing: false
            );
        }

        protected void CfgNamespace()
        {
            CfgDDNS();

            SetProperty(MSBuildProperties.DXP_METALIB_NAME, UserConfig.METALIB_NAME);
            SetProperty(MSBuildProperties.DXP_NAMESPACE, Config.Namespace ?? string.Empty);
            SetProperty(MSBuildProperties.DXP_DDNS_CECIL, Config.UseCecil);
        }

        protected void CfgPlatform()
        {
            string platform, platformS = null;

            switch(Config.Platform)
            {
                case Platform.Auto:
                case Platform.Default:
                {
                    SetProperty(MSBuildProperties.DXP_SKIP_ANYCPU, false);
                    SetProperty(MSBuildProperties.DXP_PLATFORM, Platform.Auto.ToString());
                    Log.send(this, $"Export will try to use available platform from user settings.");
                    return;
                }
                case Platform.x64:
                case Platform.x86: {
                    platform = Config.Platform.ToString();
                    break;
                }
                case Platform.AnyCPU: {
                    platform = "AnyCPU";
                    SetProperty(MSBuildProperties.DXP_SKIP_ANYCPU, false);
                    platformS = "x86 + x64";
                    break;
                }
                default: {
                    throw new NotSupportedException($"Platform '{Config.Platform}' not yet supported.");
                }
            }

            XProject.RemoveProperties(MSBuildProperties.PRJ_PLATFORM);
            SetProperty(MSBuildProperties.PRJ_PLATFORM, platform);
            Log.send(this, $"Export has been configured for platform: {platformS ?? platform}");
        }

        protected void CfgCompiler()
        {
            SetProperty(MSBuildProperties.DXP_ORDINALS_BASE, Config.Compiler.ordinalsBase);
            Log.send(this, $"The Base for ordinals: {Config.Compiler.ordinalsBase}");

            SetProperty(MSBuildProperties.DXP_GEN_EXP_LIB, Config.Compiler.genExpLib);
            Log.send(this, $"Generate .exp + .lib via MS Library Manager: {Config.Compiler.genExpLib}");

            SetProperty(MSBuildProperties.DXP_OUR_ILASM, Config.Compiler.ourILAsm);
            Log.send(this, $"Use 3F's IL Assembler: {Config.Compiler.ourILAsm}");

            if(!string.IsNullOrEmpty(Config.Compiler.customILAsm))
            {
                SetProperty(MSBuildProperties.DXP_CUSTOM_ILASM, Config.Compiler.customILAsm);
                Log.send(this, $"Set path to custom ILAsm: {Config.Compiler.customILAsm}");
            }

            SetProperty(MSBuildProperties.DXP_SYSOBJ_REBASE, Config.Compiler.rSysObj);
            Log.send(this, $"Rebase System Object: {Config.Compiler.rSysObj}");

            SetProperty(MSBuildProperties.DXP_INTERMEDIATE_FILES, Config.Compiler.intermediateFiles);
            Log.send(this, $"Flag to keep intermediate Files (IL Code, Resources, ...): {Config.Compiler.intermediateFiles}");

            if(Config.Compiler.timeout >= 0) 
            {
                SetProperty(MSBuildProperties.DXP_TIMEOUT, Config.Compiler.timeout);
                Log.send(this, $"Timeout of execution in milliseconds: {Config.Compiler.timeout}");
            }

            SetProperty(MSBuildProperties.DXP_PE_CHECK, (int)Config.Compiler.peCheck);
            Log.send(this, $"Type of checking PE32/PE32+ module: {Config.Compiler.peCheck}");

            SetProperty(MSBuildProperties.DXP_PATCHES, (long)Config.Compiler.patches);
            Log.send(this, $"Applied Patches: {Config.Compiler.patches}");

            SetProperty(MSBuildProperties.DXP_REFRESH_OBJ, Config.Compiler.refreshObj);
            Log.send(this, $"Refresh intermediate module (obj) using modified (bin): {Config.Compiler.refreshObj}");

            string asmExterns = Config.AssemblyExternDirectives.Serialize();
            SetProperty(MSBuildProperties.DXP_ILASM_EXTERN_ASM, asmExterns);
            Log.send(this, $"Custom .assembly extern ... : {asmExterns}");

            string typerefs = Config.TypeRefDirectives.Serialize();
            SetProperty(MSBuildProperties.DXP_ILASM_TYPEREF, typerefs);
            Log.send(this, $"Custom .typeref ... : {typerefs}");

            SetProperty(MSBuildProperties.DXP_TYPEREF_OPTIONS, (long)Config.TypeRefOptions);
            Log.send(this, $"Options for .typeref: {Config.TypeRefOptions}");

            string refPackages = Config.RefPackages.Serialize();
            SetProperty(MSBuildProperties.DXP_REF_PACKAGES, refPackages);
            Log.send(this, $"Package references at the pre-processing stage ... : {refPackages}");
        }

        protected void CfgCommonData(string dxpDir = null)
        {
            CfgNamespace();
            CfgPlatform();
            CfgCompiler();
            InstallGears();

            SetProperty
            (
                MSBuildProperties.DXP_DIR,
                Path.Combine("$(MSBuildProjectDirectory)\\", dxpDir ?? MakeBaseProjectPath(Config.Wizard.RootPath))
            );
        }

        protected void Reset(bool hardReset)
        {
            RemoveDllExportLib();
            UninstallGears(hardReset);

            if(hardReset) {
                XProject.RemoveProperties(ConfigProperties.Keys.ToArray()); //TODO: id for our group
                ConfigProperties.Clear();
            }

            XProject.Reevaluate();
        }

        protected void AddExternalStorage()
        {
            if(string.IsNullOrWhiteSpace(Config?.Wizard?.StoragePath))
            {
                throw new ArgumentNullException(nameof(Config.Wizard.StoragePath));
            }

            AddImport
            (
                Path.Combine(Config.Wizard.RootPath, Config.Wizard.StoragePath),
                dxpDirBased: false,
                Guids.X_EXT_STORAGE
            );
        }

        protected void AddDllExportLib()
        {
            string dxpTarget = AddImport(Config.Wizard.DxpTarget, dxpDirBased: true, METALIB_PK_TOKEN);

            if(Config.Wizard.Distributable 
                && XProject.GetFirstPackageReference(UserConfig.PKG_ID).parentItem == null)
            {
                AddDllExportRef(Config.Wizard.PkgVer);
            }

            string gCacheDir = MakeBasePath(Path.Combine(Config.Wizard.PkgPath, "gcache"));

            var rst = AddRestoreDxp
            (
                MSBuildTargets.DXP_PKG_R, 
                $"'$({MSBuildTargets.DXP_MAIN_FLAG})'!='true' Or !Exists('{dxpTarget}') Or !Exists('{gCacheDir}')",
                UserConfig.MGR_FILE
            );

            AddRestoreFirstPhase(rst, dxpTarget);
            AddDynRestore
            (
                MSBuildTargets.DXP_R_DYN, 
                $"'$({MSBuildTargets.DXP_MAIN_FLAG})'!='true' And '$(DllExportRPkgDyn)'!='false'"
            );
        }

        // https://github.com/3F/DllExport/issues/152
        protected void AddDllExportRef(string version)
        {
            if(!pkgconf.IsNew)
            {
                pkgconf.AddOrUpdatePackage(UserConfig.PKG_ID, version, "net40");
                return;
            }

            XProject.AddPackageReference
            (
                UserConfig.PKG_ID,
                version,
                Meta.Get(hide: true /*VS2010 etc*/)
            );
        }

        protected ProjectTargetElement AddRestoreDxp(string name, string condition, string manager)
        {
            var target = AddTarget(name);
            target.BeforeTargets = "PrepareForBuild";

            if((Config.Wizard.Options & DxpOptType.NoMgr) == DxpOptType.NoMgr) return target;

            string ifManager = $"Exists('{GetDxpDirBased(manager)}')";
            const string dxpNoRestore = $"'$({MSBuildProperties.DXP_NO_RESTORE})'!='true' And ";

            var taskMsg = target.AddTask("Error");
            taskMsg.Condition = $"{dxpNoRestore}!{ifManager}";
            taskMsg.SetParameter("Text", $"{manager} was not found in {GetDxpDirBased()}; https://github.com/3F/DllExport");

            var taskExec = target.AddTask("Exec");
            taskExec.Condition = $"{dxpNoRestore}({condition}) And {ifManager}";

            string args;
            if(Config?.Wizard?.MgrArgs != null) 
            {
                args = Regex.Replace(Config.Wizard.MgrArgs, @"-action\s\w+", "", RegexOptions.IgnoreCase);
                args = args.Replace("%", "%%"); // part of issue 88, probably because of %(_data.FullPath) etc.
                args = Regex.Replace(args, @"-force(?:\s|$)", "", RegexOptions.IgnoreCase); // user commands only
            }
            else
            {
                args = string.Empty;
            }
            taskExec.SetParameter("Command", $".\\{manager} {args} -action Restore");
            taskExec.SetParameter("WorkingDirectory", GetDxpDirBased());

            return target;
        }

        // https://github.com/3F/DllExport/issues/159
        protected void AddRestoreFirstPhase(ProjectTargetElement target, string dxpTarget)
        {
            var t = target.AddTask("MSBuild");
            t.Condition = $"'$({MSBuildTargets.DXP_MAIN_FLAG})'!='true'";
            t.SetParameter("Projects", dxpTarget);
            t.SetParameter("Targets", "DllExportMetaXBaseTarget");
            t.SetParameter("Properties", "TargetFramework=$(TargetFramework)");
            t.AddOutputProperty("TargetOutputs", "DllExportMetaXBase");

            string lib = MakeBasePath(MetaLib(evaluate: false));
            Log.send(this, $"'{ProjectPath}' add meta library: '{lib}'");

            target.AddItemGroup().AddItem
            (
                "Reference",
                $"DllExport, PublicKeyToken={METALIB_PK_TOKEN}",
                new Dictionary<string, string>() 
                {
                    { "HintPath", lib },
                    { "Private", false.ToString() },
                    { "SpecificVersion", false.ToString() }
                }
            );
        }

        // https://github.com/3F/DllExport/issues/62#issuecomment-353785676
        protected void AddDynRestore(string name, string condition)
        {
            var target = AddTarget(name);
            target.BeforeTargets    = "PostBuildEvent";
            target.DependsOnTargets = "GetFrameworkPaths";
            target.Condition        = condition;

            var taskMsb = target.AddTask("MSBuild");

            taskMsb.SetParameter("BuildInParallel", "true");
            taskMsb.SetParameter("UseResultsCache", "true");
            taskMsb.SetParameter("Projects", "$(MSBuildProjectFullPath)");
            taskMsb.SetParameter("Properties", "Configuration=$(Configuration);DllExportRPkgDyn=true");
            taskMsb.SetParameter("Targets", "Build");
        }

        protected ProjectTargetElement AddTarget(string name)
        {
            Log.send(this, $"'{ProjectPath}' add '{name}' target");
            return XProject.Project.Xml.AddTarget(name);
        }

        protected void RemoveDllExportLib()
        {
            foreach(var refer in XProject.GetReferences().ToArray())
            {
                if(refer.isImported) {
                    continue;
                }

                if(PublicKeyTokenLimit && METALIB_PK_TOKEN.CmpPublicKeyTokenWith(refer.Assembly.PublicKeyToken)) {
                    Log.send(this, $"Remove old reference pk:'{METALIB_PK_TOKEN}'");
                    XProject.RemoveItem(refer); // immediately modifies collection from XProject.GetReferences
                    continue;
                }

                // all obsolete packages - https://github.com/3F/DllExport/issues/65
                if(!PublicKeyTokenLimit && refer.evaluated == "DllExport") {
                    Log.send(this, $"Remove old reference no-pk:'{refer.evaluated}'");
                    XProject.RemoveItem(refer);
                }
            }

            Log.send(this, $"Attempt to delete {WZ_ID} PackageReference records");
            RemovePackageReferences(UserConfig.PKG_ID, _=> Config?.Wizard.CfgStorage == CfgStorageType.TargetsFile);
            if(!pkgconf.IsNew) pkgconf.RemovePackage(UserConfig.PKG_ID);

            Log.send(this, $"Remove old Import elements:'{DXP_TARGET}'");
            while(XProject.RemoveImport(XProject.GetImport(DXP_TARGET, null))) { }

            Log.send(this, $"Attempt to delete old Import elements via pk:'{METALIB_PK_TOKEN}'");
            while(XProject.RemoveImport(XProject.GetImport(null, METALIB_PK_TOKEN))) { }

            Log.send(this, $"Attempt to delete old restore-target: '{MSBuildTargets.DXP_PKG_R}'");
            while(XProject.RemoveXmlTarget(MSBuildTargets.DXP_PKG_R)) { }

            Log.send(this, $"Attempt to delete dynamic `import` section: '{MSBuildTargets.DXP_R_DYN}'");
            while(XProject.RemoveXmlTarget(MSBuildTargets.DXP_R_DYN)) { }

            Log.send(this, $"Attempt to delete X_EXT_STORAGE Import elements: '{Guids.X_EXT_STORAGE}'");
            while(XProject.RemoveImport(XProject.GetImport(null, Guids.X_EXT_STORAGE))) { }

            if(string.IsNullOrWhiteSpace(Config?.Wizard.DxpTarget)) {
                Log.send(this, $"DxpTarget is null or empty", Message.Level.Debug);
                return;
            }

            var dxpTarget = Path.GetFileName(Config.Wizard.DxpTarget);
            if(DXP_TARGET.Equals(dxpTarget, StringComparison.InvariantCultureIgnoreCase))
            {
                Log.send(this, $"Find and remove old DllExport's .target '{dxpTarget}'");
                while(XProject.RemoveImport(XProject.GetImport(dxpTarget, null))) { }
            }
        }

        /// <summary>
        /// To add `import` section.
        /// </summary>
        /// <param name="file">Path to project or .targets file.</param>
        /// <param name="dxpDirBased">To base on <see cref="MSBuildProperties.DXP_DIR"/>.</param>
        /// <param name="id">Id via `Label` attr.</param>
        /// <returns>Will return final added path to project or .targets file.</returns>
        protected string AddImport(string file, bool dxpDirBased, string id)
        {
            if(string.IsNullOrWhiteSpace(file)) throw new ArgumentNullException(nameof(file));

            string targets = dxpDirBased ?
                    MakeBasePath(Path.Combine(Config.Wizard.PkgPath, file), prefix: true)
                    : MakeBaseProjectPath(Path.Combine(Config.Wizard.RootPath, file));

            Log.send(this, $"'{ProjectPath}' add '{targets}':{id}");

            // we'll add inside ImportGroup because of: https://github.com/3F/DllExport/issues/77
            XProject.AddImport
            (
                [new MvsSln.Projects.ImportElement
                ( 
                    project: targets,
                    // [MSBuild]::Escape for bug when path contains ';' symbol:
                    // -The "exists" function only accepts a scalar value, but its argument ... evaluates to ...\path;\... which is not a scalar value. yeah
                    condition: dxpDirBased ? $"Exists($([MSBuild]::Escape('{targets}')))" : null,
                    label: id
                )],
                condition: null,
                DllExportVersion.DXP
            );
            return targets;
        }

        protected string GetProperty(string name) => XProject?.GetPropertyValue(name);

        protected string MakeBaseProjectPath(string path, IProject parent = null)
        {
            if(path == null) return null;

            return (parent?.XProject.ProjectPath ?? XProject.ProjectPath)
                .MakeRelativePath(Path.GetFullPath(path));
        }

        protected string MakeBasePath(string path, bool prefix = true)
        {
            if(path == null) return null;

            string ret = Config.Wizard.RootPath?.MakeRelativePath(Path.GetFullPath(path));
            return prefix ? GetDxpDirBased(ret) : ret;
        }

        protected Project RemovePackageReferences(string id, Func<Item, bool> opt = null, bool wzstrict = true)
        {
            foreach(Item item in XProject.GetPackageReferences().Where(p => !p.isImported && (id == null || p.evaluated == id)).ToArray()) 
            {
                if(!wzstrict
                    || item.meta?.GetOrDefault(WZ_ID).evaluated == "1"
                    || opt?.Invoke(item) == true)
                {
                    Log.send(this, $"{nameof(RemovePackageReferences)} {item.evaluated} {item.Assembly.Info.Version}", Message.Level.Trace);
                    XProject.RemoveItem(item);
                }
            }
            return this;
        }

        private void AllocPlatformTargetIfNeeded(IXProject xp)
        {
            foreach(var pg in xp.Project.Xml.PropertyGroups)
            {
                var v = pg.Properties.FirstOrDefault(p => p.Name == MSBuildProperties.PRJ_PLATFORM)?.Value;
                if(!string.IsNullOrEmpty(v) && pg.Properties.FirstOrDefault(p => p.Name == MSBuildProperties.DXP_ID)?.Name != null)
                {
                    AllocateProperties(MSBuildProperties.PRJ_PLATFORM);
                    break;
                }
            }
        }

        private void AllocateProperties(params string[] names)
        {
            foreach(string name in names) ConfigProperties[name] = string.Empty;
        }

        private void SetProperty(string name, string value)
        {
            if(!string.IsNullOrWhiteSpace(name))
            {
                Log.send(this, $"'{ProjectPath}' Schedule: '{name}':'{value}' ", Message.Level.Debug);
                ConfigProperties[name] = value ?? string.Empty;
            }
        }

        private void SetProperty(string name, bool val) => SetProperty(name, val.ToString().ToLower());
        private void SetProperty(string name, int val) => SetProperty(name, val.ToString());
        private void SetProperty(string name, long val) => SetProperty(name, val.ToString());

        private string CopyLib(string src, string dest)
        {
            if(!File.Exists(src)) throw new FileNotFoundException();
            if(string.IsNullOrWhiteSpace(dest)) throw new DirectoryNotFoundException();

            var dir = Path.GetDirectoryName(dest);
            if(!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }

            File.Copy(src, dest, true);

            try {
                File.Copy(DDNS.GetMetaXml(src), DDNS.GetMetaXml(dest), true);
            }
            catch(Exception ex) {
                Log.send(this, $"Xml metadata is not found: {ex.Message}", Message.Level.Debug);
            }

            return dest;
        }

        private string GetMetaDll(bool corlib)
        {
            IWizardConfig cfg = Config?.Wizard;
            if(cfg == null) {
                return null;
            }

            return corlib ? cfg.MetaCor : cfg.MetaLib;
        }

        private static class Meta
        {
            private static readonly KeyValuePair<string, string> wz = new(WZ_ID, "1");
            private static readonly KeyValuePair<string, string> privateAssets = new("PrivateAssets", "all");
            private static readonly KeyValuePair<string, string> noVisible = new("Visible", "false");
            private static readonly KeyValuePair<string, string> generatePath = new("GeneratePathProperty", "true");

            public static IEnumerable<KeyValuePair<string, string>> Get
            (
                bool privateAssets = false, bool hide = false, bool generatePath = false
            )
            {
                if(privateAssets) yield return Meta.privateAssets;
                if(hide) yield return noVisible;
                if(generatePath) yield return Meta.generatePath;
                yield return wz;
            }
        }

        private static string GetDxpDirBased(string path = null) => $"$({MSBuildProperties.DXP_DIR}){path}";
    }
}

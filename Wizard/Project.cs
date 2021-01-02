/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2021  Denis Kuzmin <x-3F@outlook.com> github/3F
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Build.Construction;
using net.r_eg.DllExport.NSBin;
using net.r_eg.DllExport.Wizard.Extensions;
using net.r_eg.DllExport.Wizard.Gears;
using net.r_eg.MvsSln.Core;
using net.r_eg.MvsSln.Extensions;
using net.r_eg.MvsSln.Log;
using RGiesecke.DllExport;

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
        private readonly LegacyPackagesFile lpf;

        /// <inheritdoc cref="IProject.XProject"/>
        public IXProject XProject { get; protected set; }

        /// <inheritdoc cref="IProject.Installed"/>
        public bool Installed
            => InternalError == null 
            && !string.IsNullOrWhiteSpace(GetProperty(MSBuildProperties.DXP_ID));

        /// <inheritdoc cref="IProject.InternalError"/>
        public string InternalError => XProject?.GetProperty(DxpIsolatedEnv.ERR_MSG, true).evaluatedValue;

        /// <inheritdoc cref="IProject.DxpIdent"/>
        public string DxpIdent
        {
            get
            {
                if(_dxpIdent != null) {
                    return _dxpIdent;
                }

                _dxpIdent = GetProperty(MSBuildProperties.DXP_ID);
                if(string.IsNullOrWhiteSpace(_dxpIdent))
                {
                    _dxpIdent = Guid.NewGuid().ToString().ToUpperInvariant();
                    Log.send(this, $"Generated new identifier: '{_dxpIdent}'", Message.Level.Debug);
                }
                return _dxpIdent;
            }
            protected set => _dxpIdent = value;
        }
        private string _dxpIdent;

        /// <inheritdoc cref="IProject.ProjectPath"/>
        public string ProjectPath
        {
            get
            {
                if(XProject == null) {
                    return null;
                }

                return XProject.ProjectItem.project.path ??
                            MakeBasePath(XProject.ProjectFullPath, false);
            }
        }

        /// <inheritdoc cref="IProject.SlnDir"/>
        public virtual string SlnDir => XProject?.Sln?.SolutionDir ?? Config?.Wizard?.SlnDir;

        /// <inheritdoc cref="IProject.ProjectNamespace"/>
        public string ProjectNamespace => GetProperty(MSBuildProperties.PRJ_NAMESPACE);

        /// <inheritdoc cref="IProject.HasExternalStorage"/>
        public bool HasExternalStorage => XProject?.GetImports(null, Guids.X_EXT_STORAGE).Count() > 0;

        /// <inheritdoc cref="IProject.Config"/>
        public IUserConfig Config { get; set; }

        /// <inheritdoc cref="IProject.ConfigProperties"/>
        public IDictionary<string, string> ConfigProperties { get; private set; } 
            = new Dictionary<string, string>();

        /// <inheritdoc cref="IProject.PublicKeyTokenLimit"/>
        public bool PublicKeyTokenLimit { get; set; } = true;

        protected ISender Log => Config?.Log ?? LSender._;

        /// <inheritdoc cref="IProject.MetaLib"/>
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
                        Config.Namespace : "$(DllExportNamespace)",

                    (evaluate && mdll != null) ? 
                        Path.GetFileName(mdll) : "$(DllExportMetaLibName)"
                )
            );
        }

        /// <inheritdoc cref="IProject.Recover"/>
        public void Recover(string id)
        {
            if(string.IsNullOrWhiteSpace(id)) {
                throw new ArgumentNullException(nameof(id));
            }
            DxpIdent = id;

            Reset(true);
            ActionConfigure(true);
        }

        /// <inheritdoc cref="IProject.Unset"/>
        public void Unset()
        {
            Log.send(this, $"Trying to unset data from '{DxpIdent}'", Message.Level.Info);
            Reset(true);
            Save();
        }

        /// <inheritdoc cref="IProject.Configure"/>
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

            gears = new IProjectGear[] 
            { 
                new PreProcGear(this),
                new PostProcGear(this),
            };

            lpf = new LegacyPackagesFile(xproject.ProjectPath);

            AllocateProperties(
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
                MSBuildProperties.DXP_PLATFORM,
                MSBuildProperties.DXP_PRE_PROC_TYPE,
                MSBuildProperties.DXP_ILMERGE,
                MSBuildProperties.DXP_POST_PROC_TYPE,
                MSBuildProperties.DXP_PROC_ENV
            );

            Log.send(this, $"Identifier: {DxpIdent}", Message.Level.Info);
        }

        protected void InstallGears() => gears.ForEach(g => g.Install());
        protected void UninstallGears(bool hardReset) => gears.ForEach(g => g.Uninstall(hardReset));

        protected void ActionRestore()
        {
            if(Installed) {
                CfgDDNS();
            }
        }

        protected void ActionUpdate()
        {
            Reset(false);

            if(Installed)
            {
                CfgDDNS();
                ActionStorage(Config.Wizard.CfgStorage);
            }

            Save();
        }

        protected void ActionConfigure(bool force = false)
        {
            Reset(true);

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
                    peCheck         = PeCheckType.PeIl,
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
            if(XProject?.ProjectFullPath != null && InternalError == null) {
                XProject.Save();
            }
        }

        protected void CfgDDNS()
        {
            CfgDDNS(false);
            CfgDDNS(true);
        }

        protected void CfgDDNS(bool corlib)
        {
            Config.DDNS.setNamespace(
                CopyLib(
                    Path.Combine(Config.Wizard.PkgPath, GetMetaDll(corlib)), 
                    MetaLib(true, corlib)
                ), 
                Config.Namespace, 
                Config.UseCecil,
                false
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
            Log.send(this, $"Use our IL Assembler: {Config.Compiler.ourILAsm}");

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
        }

        protected void CfgCommonData()
        {
            CfgNamespace();
            CfgPlatform();
            CfgCompiler();
            InstallGears();
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
            if(string.IsNullOrWhiteSpace(Config?.Wizard?.StoragePath)) {
                throw new ArgumentException($"StoragePath is empty or null.", nameof(Config.Wizard.StoragePath));
            }

            string xfile = Config.Wizard.StoragePath;
            if(SlnDir != null) {
                xfile = Path.Combine(SlnDir, xfile);
            }

            AddImport(xfile, false, Guids.X_EXT_STORAGE);
        }

        protected void AddDllExportLib()
        {
            string dxpTarget = AddImport(Config.Wizard.DxpTarget, true, METALIB_PK_TOKEN);

            if(Config.Wizard.Distributable 
                && XProject.GetFirstPackageReference(UserConfig.PKG_ID).parentItem == null)
            {
                AddDllExportRef(Config.Wizard.PkgVer);
            }

            var rst = AddRestoreDxp(
                MSBuildTargets.DXP_PKG_R, 
                $"'$({MSBuildTargets.DXP_MAIN_FLAG})' != 'true' Or !Exists('{dxpTarget}')",
                UserConfig.MGR_FILE
            );

            AddRestoreFirstPhase(rst, dxpTarget);

            AddDynRestore(
                MSBuildTargets.DXP_R_DYN, 
                $"'$({MSBuildTargets.DXP_MAIN_FLAG})' != 'true' And '$(DllExportRPkgDyn)' != 'false'"
            );
        }

        // https://github.com/3F/DllExport/issues/152
        protected void AddDllExportRef(string version)
        {
            if(lpf.IsValidExists)
            {
                lpf.AddOrUpdatePackage(UserConfig.PKG_ID, version, "net40");
                return;
            }

            XProject.AddPackageReference
            (
                UserConfig.PKG_ID,
                version,
                new Dictionary<string, string>() { { "Visible", "false" }, { WZ_ID, "1" } } // VS2010 etc
            );
        }

        protected ProjectTargetElement AddRestoreDxp(string name, string condition, string manager)
        {
            var target = AddTarget(name);
            target.BeforeTargets = "PrepareForBuild";

            if((Config.Wizard.Options & DxpOptType.NoMgr) == DxpOptType.NoMgr) return target;

            var ifManager = $"Exists('$({MSBuildProperties.SLN_DIR}){manager}')";

            var taskMsg = target.AddTask("Error");
            taskMsg.Condition = $"!{ifManager}";
            taskMsg.SetParameter("Text", $"{manager} is not found. Path: '$({MSBuildProperties.SLN_DIR})' - https://github.com/3F/DllExport");

            var taskExec = target.AddTask("Exec");
            taskExec.Condition = $"({condition}) And {ifManager}";

            string args;
            if(Config?.Wizard?.MgrArgs != null) 
            {
                args = Regex.Replace(Config.Wizard.MgrArgs, @"-action\s\w+", "", RegexOptions.IgnoreCase);
                args = args.Replace("%", "%%"); // part of issue 88, probably because of %(_data.FullPath) etc.
                args = Regex.Replace(args, @"-force(?:\s|$)", "", RegexOptions.IgnoreCase); // user commands only
            }
            else {
                args = string.Empty;
            }
            taskExec.SetParameter("Command", $".\\{manager} {args} -action Restore");
            taskExec.SetParameter("WorkingDirectory", $"$({MSBuildProperties.SLN_DIR})");

            return target;
        }

        // https://github.com/3F/DllExport/issues/159
        protected void AddRestoreFirstPhase(ProjectTargetElement target, string dxpTarget)
        {
            var t = target.AddTask("MSBuild");
            t.Condition = $"'$({MSBuildTargets.DXP_MAIN_FLAG})' != 'true'";
            t.SetParameter("Projects", dxpTarget);
            t.SetParameter("Targets", "DllExportMetaXBaseTarget");
            t.SetParameter("Properties", "TargetFramework=$(TargetFramework)");
            t.AddOutputProperty("TargetOutputs", "DllExportMetaXBase");

            var lib = MetaLib(false);
            Log.send(this, $"Add meta library: '{lib}'", Message.Level.Info);

            target.AddItemGroup().AddItem
            (
                "Reference",
                $"DllExport, PublicKeyToken={METALIB_PK_TOKEN}",
                new Dictionary<string, string>() 
                {
                    { "HintPath", MakeBasePath(lib) },
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
            taskMsb.SetParameter("Properties", "DllExportRPkgDyn=true");
            taskMsb.SetParameter("Targets", "Build");
        }

        protected ProjectTargetElement AddTarget(string name)
        {
            Log.send(this, $"Add '{name}' target", Message.Level.Info);
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
                    Log.send(this, $"Remove old reference pk:'{METALIB_PK_TOKEN}'", Message.Level.Info);
                    XProject.RemoveItem(refer); // immediately modifies collection from XProject.GetReferences
                    continue;
                }

                // all obsolete packages - https://github.com/3F/DllExport/issues/65
                if(!PublicKeyTokenLimit && refer.evaluatedInclude == "DllExport") {
                    Log.send(this, $"Remove old reference no-pk:'{refer.evaluatedInclude}'", Message.Level.Info);
                    XProject.RemoveItem(refer);
                }
            }

            Log.send(this, $"Trying to remove {WZ_ID} PackageReference records", Message.Level.Info);
            foreach(var item in XProject.GetItems("PackageReference", UserConfig.PKG_ID).ToArray()) 
            {
                if(item.isImported) {
                    continue;
                }

                if(item.meta?.ContainsKey(WZ_ID) == true && item.meta[WZ_ID].evaluated == "1"
                    || Config?.Wizard.CfgStorage == CfgStorageType.TargetsFile) 
                {
                    XProject.RemoveItem(item);
                }
            }
            lpf.RemoveSavedPackage(UserConfig.PKG_ID);

            Log.send(this, $"Remove old Import elements:'{DXP_TARGET}'", Message.Level.Info);
            while(XProject.RemoveImport(XProject.GetImport(DXP_TARGET, null))) { }

            Log.send(this, $"Trying to remove old Import elements via pk:'{METALIB_PK_TOKEN}'", Message.Level.Info);
            while(XProject.RemoveImport(XProject.GetImport(null, METALIB_PK_TOKEN))) { }

            Log.send(this, $"Trying to remove old restore-target: '{MSBuildTargets.DXP_PKG_R}'", Message.Level.Info);
            while(XProject.RemoveXmlTarget(MSBuildTargets.DXP_PKG_R)) { }

            Log.send(this, $"Trying to remove dynamic `import` section: '{MSBuildTargets.DXP_R_DYN}'", Message.Level.Info);
            while(XProject.RemoveXmlTarget(MSBuildTargets.DXP_R_DYN)) { }

            Log.send(this, $"Trying to remove X_EXT_STORAGE Import elements: '{Guids.X_EXT_STORAGE}'", Message.Level.Info);
            while(XProject.RemoveImport(XProject.GetImport(null, Guids.X_EXT_STORAGE))) { }

            if(string.IsNullOrWhiteSpace(Config?.Wizard.DxpTarget)) {
                Log.send(this, $"DxpTarget is null or empty", Message.Level.Debug);
                return;
            }

            var dxpTarget = Path.GetFileName(Config.Wizard.DxpTarget);
            if(DXP_TARGET.Equals(dxpTarget, StringComparison.InvariantCultureIgnoreCase))
            {
                Log.send(this, $"Find and remove '{dxpTarget}' as an old .target file of the DllExport.", Message.Level.Info);
                while(XProject.RemoveImport(XProject.GetImport(dxpTarget, null))) { }
            }
        }

        /// <summary>
        /// To add `import` section.
        /// </summary>
        /// <param name="file">Path to project or .targets file.</param>
        /// <param name="checking">To check existence of target via 'Condition' attr.</param>
        /// <param name="id">Id via `Label` attr.</param>
        /// <returns>Will return final added path to project or .targets file.</returns>
        protected string AddImport(string file, bool checking, string id)
        {
            if(string.IsNullOrWhiteSpace(file)) {
                throw new ArgumentNullException(nameof(file));
            }

            var targets = MakeBasePath(
                Path.GetFullPath(
                    Path.Combine(Config.Wizard.PkgPath, file)
                )
            );

            Log.send(this, $"Add .targets: '{targets}':{id}", Message.Level.Info);

            // we'll add inside ImportGroup because of: https://github.com/3F/DllExport/issues/77
            XProject.AddImport(
                new [] {
                    new MvsSln.Projects.ImportElement() {
                        project     = targets,
                        // [MSBuild]::Escape for bug when path contains ';' symbol:
                        // -The "exists" function only accepts a scalar value, but its argument ... evaluates to ...\path;\... which is not a scalar value. yeah
                        condition   = $"Exists($([MSBuild]::Escape('{targets}')))",
                        label       = id
                    }
                }, 
                null,
                DllExportVersion.DXP
            );
            return targets;
        }

        protected string GetProperty(string name) => XProject?.GetPropertyValue(name);

        protected virtual string MakeBasePath(string path, bool prefix = true)
        {
            string ret = SlnDir?.MakeRelativePath(path);
            if(prefix) {
                return $"$({MSBuildProperties.SLN_DIR}){ret}";
            }
            return ret;
        }

        private void AllocateProperties(params string[] names)
        {
            foreach(var name in names) {
                ConfigProperties[name] = null;
            }
        }

        private void SetProperty(string name, string value)
        {
            if(!string.IsNullOrWhiteSpace(name)) {
                Log.send(this, $"'{ProjectPath}' Schedule an adding property: '{name}':'{value}' ", Message.Level.Debug);
                ConfigProperties[name] = value;
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
    }
}

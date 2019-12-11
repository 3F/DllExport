/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2019  Denis Kuzmin < entry.reg@gmail.com > GitHub/3F
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
using net.r_eg.DllExport.NSBin;
using net.r_eg.DllExport.Wizard.Extensions;
using net.r_eg.MvsSln.Core;
using net.r_eg.MvsSln.Extensions;
using net.r_eg.MvsSln.Log;
using RGiesecke.DllExport;

namespace net.r_eg.DllExport.Wizard
{
    public class Project: IProject
    {
        /// <summary>
        /// PublicKeyToken of the meta library.
        /// </summary>
        public const string METALIB_PK_TOKEN = "8337224c9ad9e356";

        /// <summary>
        /// The name of the default .target file.
        /// </summary>
        public const string DXP_TARGET = "net.r_eg.DllExport.targets";

        /// <summary>
        /// The name of target to restore package.
        /// </summary>
        protected const string DXP_TARGET_PKG_R = "DllExportRestorePkg";

        /// <summary>
        /// To support dynamic `import` section.
        /// https://github.com/3F/DllExport/issues/62
        /// </summary>
        protected const string DXP_TARGET_R_DYN = "DllExportRPkgDynamicImport";

        /// <summary>
        /// Access to found project.
        /// </summary>
        public IXProject XProject
        {
            get;
            protected set;
        }

        /// <summary>
        /// Installation checking.
        /// </summary>
        public bool Installed
        {
            get => InternalError == null && !String.IsNullOrWhiteSpace(GetProperty(MSBuildProperties.DXP_ID));
        }

        /// <summary>
        /// Message if an internal error occurred, otherwise null value.
        /// TODO: because of DxpIsolatedEnv. See details there.
        /// </summary>
        public string InternalError
        {
            get => XProject?.GetProperty(DxpIsolatedEnv.ERR_MSG, true).evaluatedValue;
        }

        /// <summary>
        /// Special identifier. Like `ProjectGuid` that is not available in SDK-based projects.
        /// https://github.com/3F/DllExport/issues/36#issuecomment-320794498
        /// </summary>
        public string DxpIdent
        {
            get
            {
                if(_dxpIdent != null) {
                    return _dxpIdent;
                }

                _dxpIdent = GetProperty(MSBuildProperties.DXP_ID);
                if(String.IsNullOrWhiteSpace(_dxpIdent))
                {
                    _dxpIdent = Guid.NewGuid().ToString().ToUpperInvariant();
                    Log.send(this, $"Generated new identifier: '{_dxpIdent}'", Message.Level.Debug);
                }
                return _dxpIdent;
            }
            protected set => _dxpIdent = value;
        }
        private string _dxpIdent;

        /// <summary>
        /// Relative path from location of sln file.
        /// </summary>
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

        /// <summary>
        /// Full path to root solution directory.
        /// </summary>
        public virtual string SlnDir
        {
            get => XProject?.Sln?.SolutionDir ?? Config?.Wizard?.SlnDir;
        }

        /// <summary>
        /// Get defined namespace for project.
        /// </summary>
        public string ProjectNamespace
        {
            get => GetProperty(MSBuildProperties.PRJ_NAMESPACE);
        }

        /// <summary>
        /// Checks usage of external storage for this project.
        /// </summary>
        public bool HasExternalStorage
        {
            get => XProject?.GetImports(null, Guids.X_EXT_STORAGE).Count() > 0;
        }

        /// <summary>
        /// Active configuration of user data.
        /// </summary>
        public IUserConfig Config
        {
            get;
            set;
        }

        /// <summary>
        /// List of used MSBuild properties.
        /// </summary>
        public IDictionary<string, string> ConfigProperties
        {
            get;
            private set;
        } = new Dictionary<string, string>();

        /// <summary>
        /// Limitation of actions if not used PublicKeyToken.
        /// https://github.com/3F/DllExport/issues/65
        /// </summary>
        public bool PublicKeyTokenLimit
        {
            get;
            set;
        } = true;

        protected ISender Log
        {
            get => Config?.Log;
        }

        /// <summary>
        /// Returns fullpath to meta library for current project.
        /// </summary>
        /// <param name="evaluate">Will return unevaluated value if false.</param>
        /// <param name="corlib">netfx-based or netcore-based meta lib.</param>
        /// <returns></returns>
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

        /// <summary>
        /// To recover references with project file.
        /// IWizardConfig.CfgStorage value can affect on type of this references.
        /// </summary>
        /// <param name="id">Known identifier of the references.</param>
        public void Recover(string id)
        {
            if(String.IsNullOrWhiteSpace(id)) {
                throw new ArgumentNullException(nameof(id));
            }
            DxpIdent = id;

            Reset(true);
            ActionConfigure(true);
        }

        /// <summary>
        /// To unset configured data from project if presented.
        /// </summary>
        public void Unset()
        {
            Log.send(this, $"Trying to unset data from '{DxpIdent}'", Message.Level.Info);
            Reset(true);
            Save();
        }

        /// <summary>
        /// To configure project via specific action.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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

        /// <param name="xproject"></param>
        /// <param name="init"></param>
        public Project(IXProject xproject, IConfigInitializer init)
            : this(xproject)
        {
            Config = GetUserConfig(xproject, init);

            Config.AddTopNamespace(ProjectNamespace);

            AllocateProperties(
                MSBuildProperties.DXP_ID,
                MSBuildProperties.DXP_METALIB_NAME,
                MSBuildProperties.DXP_NAMESPACE,
                MSBuildProperties.DXP_ORDINALS_BASE,
                MSBuildProperties.DXP_SKIP_ANYCPU,
                MSBuildProperties.DXP_DDNS_CECIL,
                MSBuildProperties.DXP_GEN_EXP_LIB,
                MSBuildProperties.DXP_OUR_ILASM,
                MSBuildProperties.DXP_CUSTOM_ILASM,
                MSBuildProperties.DXP_INTERMEDIATE_FILES,
                MSBuildProperties.DXP_TIMEOUT,
                MSBuildProperties.DXP_PE_CHECK,
                MSBuildProperties.DXP_PLATFORM
            );

            Log.send(this, $"Identifier: {DxpIdent}", Message.Level.Info);
        }

        /// <param name="xproject"></param>
        public Project(IXProject xproject)
        {
            XProject = xproject ?? throw new ArgumentNullException(nameof(xproject));
        }

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
                Compiler = new CompilerCfg() {
                    ordinalsBase    = 1,
                    timeout         = CompilerCfg.TIMEOUT_EXEC,
                    peCheck         = PeCheckType.PeIl
                },
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
            SetProperty(MSBuildProperties.DXP_NAMESPACE, Config.Namespace ?? String.Empty);
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

            RemoveProperties(MSBuildProperties.PRJ_PLATFORM);
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

            if(Config.Compiler.customILAsm != null) {
                SetProperty(MSBuildProperties.DXP_CUSTOM_ILASM, Config.Compiler.customILAsm);
                Log.send(this, $"Set path to custom ILAsm: {Config.Compiler.customILAsm}");
            }

            SetProperty(MSBuildProperties.DXP_INTERMEDIATE_FILES, Config.Compiler.intermediateFiles);
            Log.send(this, $"Flag to keep intermediate Files (IL Code, Resources, ...): {Config.Compiler.intermediateFiles}");

            if(Config.Compiler.timeout >= 0) {
                SetProperty(MSBuildProperties.DXP_TIMEOUT, Config.Compiler.timeout);
                Log.send(this, $"Timeout of execution in milliseconds: {Config.Compiler.timeout}");
            }

            SetProperty(MSBuildProperties.DXP_PE_CHECK, (int)Config.Compiler.peCheck);
            Log.send(this, $"Type of checking PE32/PE32+ module: {Config.Compiler.peCheck}");
        }

        protected void CfgCommonData()
        {
            CfgNamespace();
            CfgPlatform();
            CfgCompiler();
        }

        protected void Reset(bool properties)
        {
            RemoveDllExportLib();

            if(properties) {
                RemoveProperties(ConfigProperties.Keys.ToArray());
                ConfigProperties.Clear();
            }

            XProject.Reevaluate();
        }

        protected bool CmpPublicKeyTokens(string pkToken, string pkTokenAsm)
        {
            if(pkTokenAsm == null || String.IsNullOrWhiteSpace(pkToken)) {
                return false;
            }
            return pkToken.Equals(pkTokenAsm, StringComparison.InvariantCultureIgnoreCase);
        }

        protected void AddExternalStorage()
        {
            if(String.IsNullOrWhiteSpace(Config?.Wizard?.StoragePath)) {
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

            var lib = MetaLib(false);
            Log.send(this, $"Add meta library: '{lib}'", Message.Level.Info);
            //XProject.AddReference(lib, false);
            XProject.AddReference(
                $"DllExport, PublicKeyToken={METALIB_PK_TOKEN}",
                MakeBasePath(lib), 
                false,
                null,
                false
            );

            AddRestoreDxp(
                DXP_TARGET_PKG_R, 
                $"'$(DllExportModImported)' != 'true' Or !Exists('{dxpTarget}')",
                UserConfig.MGR_FILE
            );

            AddDynRestore(
                DXP_TARGET_R_DYN, 
                $"'$(DllExportModImported)' != 'true' And '$(DllExportRPkgDyn)' != 'false'"
            );
        }

        protected void AddRestoreDxp(string name, string condition, string manager)
        {
            var target = AddTarget(name);
            target.BeforeTargets = "PrepareForBuild";

            var ifManager = $"Exists('$(SolutionDir){manager}')";

            var taskMsg = target.AddTask("Error");
            taskMsg.Condition = $"!{ifManager}";
            taskMsg.SetParameter("Text", $"{manager} is not found. Path: '$(SolutionDir)' - https://github.com/3F/DllExport");

            var taskExec = target.AddTask("Exec");
            taskExec.Condition = $"({condition}) And {ifManager}";

            string args;
            if(Config?.Wizard?.MgrArgs != null) {
                args = Regex.Replace(Config.Wizard.MgrArgs, @"-action\s\w+", "", RegexOptions.IgnoreCase);
                args = args.Replace("%", "%%"); // part of issue 88, probably because of %(_data.FullPath) etc.
            }
            else {
                args = String.Empty;
            }
            taskExec.SetParameter("Command", $"{manager} {args} -action Restore");
            taskExec.SetParameter("WorkingDirectory", "$(SolutionDir)");
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

        protected Microsoft.Build.Construction.ProjectTargetElement AddTarget(string name)
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

                if(PublicKeyTokenLimit && CmpPublicKeyTokens(METALIB_PK_TOKEN, refer.Assembly.PublicKeyToken)) {
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

            Log.send(this, $"Remove old Import elements:'{DXP_TARGET}'", Message.Level.Info);
            while(XProject.RemoveImport(XProject.GetImport(DXP_TARGET, null))) { }

            Log.send(this, $"Trying to remove old Import elements via pk:'{METALIB_PK_TOKEN}'", Message.Level.Info);
            while(XProject.RemoveImport(XProject.GetImport(null, METALIB_PK_TOKEN))) { }

            Log.send(this, $"Trying to remove old restore-target: '{DXP_TARGET_PKG_R}'", Message.Level.Info);
            while(RemoveXmlTarget(DXP_TARGET_PKG_R)) { }

            Log.send(this, $"Trying to remove dynamic `import` section: '{DXP_TARGET_R_DYN}'", Message.Level.Info);
            while(RemoveXmlTarget(DXP_TARGET_R_DYN)) { }

            Log.send(this, $"Trying to remove X_EXT_STORAGE Import elements: '{Guids.X_EXT_STORAGE}'", Message.Level.Info);
            while(XProject.RemoveImport(XProject.GetImport(null, Guids.X_EXT_STORAGE))) { }

            if(String.IsNullOrWhiteSpace(Config.Wizard.DxpTarget)) {
                return;
            }

            var dxpTarget = Path.GetFileName(Config.Wizard.DxpTarget);
            if(DXP_TARGET.Equals(dxpTarget, StringComparison.InvariantCultureIgnoreCase))
            {
                Log.send(this, $"Find and remove '{dxpTarget}' as an old .target file of the DllExport.", Message.Level.Info);
                while(XProject.RemoveImport(XProject.GetImport(dxpTarget, null))) { }
            }
        }

        protected bool RemoveXmlTarget(string name)
        {
            if(String.IsNullOrWhiteSpace(name)) {
                return false;
            }

            var target = XProject.Project.Xml.Targets.Where(t => t.Name == name).FirstOrDefault();
            if(target != null) {
                XProject.Project.Xml.RemoveChild(target);
                return true;
            }
            return false;
        }

        protected void RemoveProperties(params string[] names)
        {
            foreach(string name in names)
            {
                if(!String.IsNullOrWhiteSpace(name)) {
                    // Log.send(this, $"'{ProjectPath}' Remove old properties: '{name}'", Message.Level.Trace);
                    while(XProject.RemoveProperty(name, true)) { }
                }
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
            if(String.IsNullOrWhiteSpace(file)) {
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
                ".NET DllExport"
            );
            return targets;
        }

        protected string GetProperty(string name)
        {
            return XProject?.GetPropertyValue(name);
        }

        protected virtual string MakeBasePath(string path, bool prefix = true)
        {
            string ret = SlnDir?.MakeRelativePath(path);
            if(prefix) {
                return $"$(SolutionDir){ret}";
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
            if(!String.IsNullOrWhiteSpace(name)) {
                Log.send(this, $"'{ProjectPath}' Schedule an adding property: '{name}':'{value}' ", Message.Level.Debug);
                ConfigProperties[name] = value;
            }
        }

        private void SetProperty(string name, bool val)
        {
            SetProperty(name, val.ToString().ToLower());
        }

        private void SetProperty(string name, int val)
        {
            SetProperty(name, val.ToString());
        }

        private string CopyLib(string src, string dest)
        {
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

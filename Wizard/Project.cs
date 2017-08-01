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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using net.r_eg.MvsSln.Core;
using net.r_eg.MvsSln.Extensions;
using net.r_eg.MvsSln.Log;

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
        /// Access to found project.
        /// </summary>
        public IXProject XProject
        {
            get;
            private set;
        }

        /// <summary>
        /// Installation checking.
        /// </summary>
        public bool Installed
        {
            get {
                string vNamespace = GetProperty(MSBuildProperties.DXP_NAMESPACE);
                return !String.IsNullOrWhiteSpace(vNamespace);
            }
        }

        /// <summary>
        /// Relative path from location of sln file.
        /// </summary>
        public string ProjectPath
        {
            get => XProject?.ProjectItem.project.path;
        }

        /// <summary>
        /// The Guid of current project.
        /// </summary>
        public string ProjectGuid
        {
            get => XProject?.ProjectGuid;
        }

        /// <summary>
        /// Get defined namespace for project.
        /// </summary>
        public string ProjectNamespace
        {
            get => GetProperty(MSBuildProperties.PRJ_NAMESPACE);
        }

        /// <summary>
        /// Returns fullpath to meta library for current project.
        /// </summary>
        public virtual string MetaLib
        {
            get => Path.GetFullPath
                   (
                        Path.Combine(
                            Config.Wizard.PkgPath,
                            "gcache",
                            ProjectGuid,
                            Path.GetFileName(Config.Wizard.MetaLib)
                        )
                   );
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

        protected ISender Log
        {
            get => Config?.Log;
        }

        /// <summary>
        /// To configure project via specific action.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool Configure(ActionType type)
        {
            if(type == ActionType.Restore)
            {
                if(Installed) {
                    CfgDDNS();
                }
                return true;
            }

            if(type == ActionType.Configure || type == ActionType.Update)
            {
                Reset();
                XProject.Reevaluate();

                if(Config.Install)
                {
                    CfgNamespace();
                    CfgPlatform();
                    CfgCompiler();

                    AddDllExportLib();

                    XProject.SetProperties(ConfigProperties);
                    XProject.Reevaluate();
                }

                Save();
                return true;
            }

            return false;
        }

        /// <param name="xproject"></param>
        /// <param name="init"></param>
        public Project(IXProject xproject, IConfigInitializer init)
        {
            XProject    = xproject ?? throw new ArgumentNullException(nameof(xproject));
            Config      = GetUserConfig(xproject, init);

            Config.AddTopNamespace(ProjectNamespace);

            AllocateProperties(
                MSBuildProperties.DXP_NAMESPACE,
                MSBuildProperties.DXP_ORDINALS_BASE,
                MSBuildProperties.DXP_SKIP_ANYCPU,
                MSBuildProperties.DXP_DDNS_CECIL,
                MSBuildProperties.DXP_GEN_EXP_LIB,
                MSBuildProperties.DXP_OUR_ILASM,
                MSBuildProperties.PRJ_PLATFORM
            );
        }

        protected IUserConfig GetUserConfig(IXProject project, IConfigInitializer cfg)
        {
            if(Installed) {
                return new UserConfig(cfg, project);
            }

            return new UserConfig(cfg)
            {
                UseCecil    = true,
                Platform    = Platform.x86x64,
                Compiler = new CompilerCfg() {
                    ordinalsBase = 1
                },
            };
        }

        /// <summary>
        /// Saves the project to the file system, if modified.
        /// </summary>
        protected void Save()
        {
            XProject?.Save();
        }

        protected void CfgDDNS()
        {
            Config.DDNS.setNamespace(
                CopyFile(
                    Path.Combine(Config.Wizard.PkgPath, Config.Wizard.MetaLib), 
                    MetaLib
                ), 
                Config.Namespace, 
                Config.UseCecil,
                false
            );
        }

        protected void CfgNamespace()
        {
            CfgDDNS();

            SetProperty(MSBuildProperties.DXP_NAMESPACE, Config.Namespace ?? String.Empty);
            SetProperty(MSBuildProperties.DXP_DDNS_CECIL, Config.UseCecil);
        }

        protected void CfgPlatform()
        {
            string platform, platformS;

            switch(Config.Platform)
            {
                case Platform.x64: {
                    platformS   =
                    platform    = 
                                "x64";
                    break;
                }
                case Platform.x86: {
                    platformS   =
                    platform    = 
                                "x86";
                    break;
                }
                case Platform.Default:
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

            SetProperty(MSBuildProperties.PRJ_PLATFORM, platform);
            Log.send(this, $"Export has been configured for platform: {platformS}");
        }

        protected void CfgCompiler()
        {
            SetProperty(MSBuildProperties.DXP_ORDINALS_BASE, Config.Compiler.ordinalsBase);
            Log.send(this, $"The Base for ordinals: {Config.Compiler.ordinalsBase}");

            SetProperty(MSBuildProperties.DXP_GEN_EXP_LIB, Config.Compiler.genExpLib);
            Log.send(this, $"Generate .exp + .lib via MS Library Manager: {Config.Compiler.genExpLib}");

            SetProperty(MSBuildProperties.DXP_OUR_ILASM, Config.Compiler.ourILAsm);
            Log.send(this, $"Use our IL Assembler: {Config.Compiler.ourILAsm}");
        }

        protected void Reset()
        {
            RemoveProperties(ConfigProperties.Keys.ToArray());
            ConfigProperties.Clear();

            RemoveDllExportLib();
        }

        protected bool CmpPublicKeyTokens(string pkToken, string pkTokenAsm)
        {
            if(pkTokenAsm == null || String.IsNullOrWhiteSpace(pkToken)) {
                return false;
            }
            return pkToken.Equals(pkTokenAsm, StringComparison.InvariantCultureIgnoreCase);
        }

        protected void AddDllExportLib()
        {
            if(String.IsNullOrWhiteSpace(Config.Wizard.DxpTarget)) {
                throw new ArgumentNullException(nameof(Config.Wizard.DxpTarget));
            }

            var dxpTarget = XProject.ProjectPath.MakeRelativePath(
                Path.GetFullPath(
                    Path.Combine(Config.Wizard.PkgPath, Config.Wizard.DxpTarget)
                )
            );

            Log.send(this, $"Add .target: '{dxpTarget}':{METALIB_PK_TOKEN}", Message.Level.Info);
            XProject.AddImport(dxpTarget, true, METALIB_PK_TOKEN);

            var lib = MetaLib;
            Log.send(this, $"Add meta library: '{lib}'", Message.Level.Info);
            XProject.AddReference(lib, false);
        }

        protected void RemoveDllExportLib()
        {
            foreach(var refer in XProject.GetReferences().ToArray())
            {
                if(CmpPublicKeyTokens(METALIB_PK_TOKEN, refer.Assembly.PublicKeyToken)) {
                    Log.send(this, $"Remove old reference pk:'{METALIB_PK_TOKEN}'", Message.Level.Debug);
                    XProject.RemoveItem(refer); // immediately modifies collection from XProject.GetReferences
                }
            }

            Log.send(this, $"Remove old Import elements:'{DXP_TARGET}'", Message.Level.Debug);
            while(XProject.RemoveImport(XProject.GetImport(DXP_TARGET, null))) { }

            Log.send(this, $"Try to remove old Import elements via pk:'{METALIB_PK_TOKEN}'", Message.Level.Debug);
            while(XProject.RemoveImport(XProject.GetImport(null, METALIB_PK_TOKEN))) { }

            if(String.IsNullOrWhiteSpace(Config.Wizard.DxpTarget)) {
                return;
            }

            var dxpTarget = Path.GetFileName(Config.Wizard.DxpTarget);
            if(DXP_TARGET.Equals(dxpTarget, StringComparison.InvariantCultureIgnoreCase))
            {
                Log.send(this, $"Find and remove '{dxpTarget}' as an old .target file of the DllExport.", Message.Level.Debug);
                while(XProject.RemoveImport(XProject.GetImport(dxpTarget, null))) { }
            }
        }

        protected void RemoveProperties(params string[] names)
        {
            foreach(string name in names)
            {
                if(!String.IsNullOrWhiteSpace(name)) {
                    Log.send(this, $"'{ProjectGuid}' Remove old properties: '{name}'", Message.Level.Trace);
                    while(XProject.RemoveProperty(name, true)) { }
                }
            }
        }

        protected string GetProperty(string name)
        {
            return XProject?.GetProperty(name).evaluatedValue;
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
                Log.send(this, $"'{ProjectGuid}' Schedule an adding property: '{name}':'{value}' ", Message.Level.Debug);
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

        private string CopyFile(string src, string dest, bool overwrite = true)
        {
            var dir = Path.GetDirectoryName(dest);
            if(!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }

            File.Copy(src, dest, overwrite);
            return dest;
        }
    }
}

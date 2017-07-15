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
using System.IO;
using net.r_eg.MvsSln.Core;
using net.r_eg.MvsSln.Log;

namespace net.r_eg.DllExport.Wizard
{
    public class Project: IProject
    {
        ///// <summary>
        ///// PublicKeyToken of meta library.
        ///// </summary>
        //private string MetaLibPublicKeyToken = "8337224C9AD9E356";

        ///// <summary>
        ///// File name without extension of meta library.
        ///// </summary>
        //private string MetaLibAsmFileName = "DllExport";

        /// <summary>
        /// Access to found project.
        /// </summary>
        public IXProject XProject
        {
            get;
            private set;
        }

        /// <summary>
        /// Checks existence of installed DllExport for current project.
        /// </summary>
        public bool Installed
        {
            get {
                // TODO: to check also metalib Key
                string vNamespace = XProject?.GetProperty(MSBuildProperties.DXP_NAMESPACE).evaluatedValue;
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
            get => XProject?.GetProperty(MSBuildProperties.PRJ_NAMESPACE).evaluatedValue;
        }

        /// <summary>
        /// Returns fullpath to meta library for current project.
        /// </summary>
        public virtual string MetaLibPath
        {
            get => Path.GetFullPath(Path.Combine(Config.Wizard.PkgPath, "gcache", ProjectGuid));
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
        public string[] ConfigProperties
        {
            get;
            private set;
        }

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
            //TODO:  install/remove
            //TODO: read properties for UI

            //switch(type) {
            //    case ActionType.Remove: {
            //        //RemoveProperties();
            //        return true;
            //    }
            //}

            return true;
        }

        /// <summary>
        /// Saves the project to the file system, if modified.
        /// </summary>
        public void Save()
        {
            XProject?.Save();
        }

        /// <param name="xproject"></param>
        public Project(IXProject xproject, IUserConfig cfg)
        {
            XProject    = xproject ?? throw new ArgumentNullException(nameof(xproject));
            Config      = cfg ?? throw new ArgumentNullException(nameof(cfg));

            var ns = ProjectNamespace;
            if(!Config.Namespaces.Contains(ns)) {
                Config.Namespaces.Insert(0, ns);
            }

            ConfigProperties = new[] {
                MSBuildProperties.DXP_NAMESPACE,
                MSBuildProperties.DXP_ORDINALS_BASE,
                MSBuildProperties.DXP_SKIP_ANYCPU,
                MSBuildProperties.DXP_DDNS_CECIL,
                MSBuildProperties.DXP_GEN_EXP_LIB,
                MSBuildProperties.DXP_OUR_ILASM
            };
        }

        //public void UpdatePlatform(string file, Platform platform)
        //{
        //    foreach(var p in GetEnv(file)?.Projects) {
        //        p.SetProperty("Platform", platform.)
        //    }
        //}

        protected void SetNamespace()
        {
            XProject?.SetProperty(MSBuildProperties.DXP_NAMESPACE, Config.Namespace ?? String.Empty);
        }

        protected void CfgNamespace()
        {
            SetProperty(MSBuildProperties.DXP_NAMESPACE, Config.Namespace ?? String.Empty);
            SetProperty(MSBuildProperties.DXP_DDNS_CECIL, Config.UseCecil);

            // binary modifications of metalib assembly
            Config.DDNS.setNamespace(MetaLibPath, Config.Namespace, Config.UseCecil);
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

            SetProperty("PlatformTarget", platform);

            Log.send(this, $"The Export has been configured for platform: {platformS}");
        }

        protected void CfgCompiler()
        {
            SetProperty("DllExportOrdinalsBase", Config.Compiler.ordinalsBase);
            Log.send(this, $"The Base for ordinals: {Config.Compiler.ordinalsBase}");

            SetProperty("DllExportGenExpLib", Config.Compiler.genExpLib);
            Log.send(this, $"Generate .exp + .lib via MS Library Manager: {Config.Compiler.genExpLib}");

            SetProperty("DllExportOurILAsm", Config.Compiler.ourILAsm);
            Log.send(this, $"Use our IL Assembler: {Config.Compiler.ourILAsm}");
        }

        /// <summary>
        /// Rollback the changes.
        /// </summary>
        protected void Reset()
        {
            RemoveProperties(ConfigProperties);
        }

        private void RemoveProperties(params string[] names)
        {
            foreach(string name in names)
            {
                if(!String.IsNullOrWhiteSpace(name)) {
                    XProject.RemoveProperty(name);
                }
            }
        }

        private void SetProperty(string name, string val)
        {
            if(String.IsNullOrWhiteSpace(name)) {
                return;
            }

            XProject.SetProperty(name, val);
            //project.saveViaDTE();
        }

        private void SetProperty(string name, bool val)
        {
            SetProperty(name, val.ToString().ToLower());
        }

        private void SetProperty(string name, int val)
        {
            SetProperty(name, val.ToString());
        }
    }
}

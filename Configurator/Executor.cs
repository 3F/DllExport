/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2018  Denis Kuzmin < entry.reg@gmail.com > :: github.com/3F
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
using System.Text;
using net.r_eg.Conari.Log;
using net.r_eg.DllExport.Configurator.Dynamic;
using net.r_eg.DllExport.Configurator.GUI;
using net.r_eg.DllExport.NSBin;

namespace net.r_eg.DllExport.Configurator
{
    using Platform = UserConfig.PlatformTarget;

    internal class Executor: IExecutor
    {
        protected UserConfig config;
        protected IProject project;
        protected IDDNS ddns;

        public ISender Log
        {
            get {
                return LSender._;
            }
        }

        public void configure()
        {
            if(!project.IsDefinedNamespace)
            {
                config.defnamespaces.Insert(0, project.getPropertyValue("RootNamespace"));

                var form = new InstallationForm(config, project);
                form.ShowDialog();

                project.defineNamespace(config.unamespace);
            }

            cfgNamespace();
            cfgPlatform();
            cfgCompiler();
        }

        /// <summary>
        /// Rollback the changes.
        /// </summary>
        public void reset()
        {
            //TODO: unified place for all used msbuild properties.
            removeMsbuildProperties(
                "DllExportNamespace",
                "DllExportOrdinalsBase",
                "DllExportSkipOnAnyCpu",
                "DllExportDDNSCecil",
                "DllExportGenExpLib",
                "DllExportOurILAsm"
            );
        }

        public Executor(IScriptConfig cfg)
        {
            ddns    = new DDNS(Encoding.UTF8);
            config  = new UserConfig(cfg) { nsBuffer = ddns.NSBuffer };
            project = new Project(config.script.ProjectDTE, config.script.ProjectsMBE);
        }

        protected void cfgNamespace()
        {
            project.updateNamespaceForAllProjects(
                config.unamespace, 
                config.MetaLibAsmFileName, 
                config.MetaLibPublicKeyToken
            );

            setProperty("DllExportDDNSCecil", config.useCecil);

            // binary modifications of metalib assembly
            ddns.setNamespace(config.script.MetaLib, config.unamespace, config.useCecil);
        }

        protected void cfgPlatform()
        {
            string platform, platformS;

            switch(config.platform)
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
                    project.setProperty("DllExportSkipOnAnyCpu", "false");
                    platformS = "x86 + x64";
                    break;
                }
                default: {
                    throw new NotSupportedException($"Platform '{config.platform}' not yet supported.");
                }
            }

            setProperty("PlatformTarget", platform);

            Log.send(this, $"The Export has been configured for platform: {platformS}");
        }

        protected void cfgCompiler()
        {
            setProperty("DllExportOrdinalsBase", config.compiler.ordinalsBase);
            Log.send(this, $"The Base for ordinals: {config.compiler.ordinalsBase}");

            setProperty("DllExportGenExpLib", config.compiler.genExpLib);
            Log.send(this, $"Generate .exp + .lib via MS Library Manager: {config.compiler.genExpLib}");

            setProperty("DllExportOurILAsm", config.compiler.ourILAsm);
            Log.send(this, $"Use our IL Assembler: {config.compiler.ourILAsm}");
        }

        protected void removeMsbuildProperties(params string[] names)
        {
            foreach(string p in names) {
                project.removeProperty(p);
            }
            project.saveViaDTE();
        }

        private void setProperty(string name, string val)
        {
            if(String.IsNullOrWhiteSpace(name)) {
                return;
            }

            project.setProperty(name, val);
            project.saveViaDTE();
        }

        private void setProperty(string name, bool val)
        {
            setProperty(name, val.ToString().ToLower());
        }

        private void setProperty(string name, int val)
        {
            setProperty(name, val.ToString());
        }
    }
}

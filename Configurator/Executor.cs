/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016  Denis Kuzmin <entry.reg@gmail.com>
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
            if(project.IsDefinedNamespace)
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

            // binary modifications of metalib assembly
            ddns.setNamespace(config.script.MetaLib, config.unamespace);
        }

        protected void cfgPlatform()
        {
            string platform;
            switch(config.platform) {
                case Platform.x64: {
                    platform = "x64";
                    break;
                }
                case Platform.x86: {
                    platform = "x86";
                    break;
                }
                case Platform.Default:
                case Platform.AnyCPU: {
                    platform = "AnyCPU";
                    project.setProperty("DllExportSkipOnAnyCpu", "false");
                    break;
                }
                default: {
                    throw new NotSupportedException($"Platform '{config.platform}' not yet supported.");
                }
            }

            project.setProperty("PlatformTarget", platform);
            project.saveViaDTE();
        }

        protected void cfgCompiler()
        {
            //TODO:
        }
    }
}

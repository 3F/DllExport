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
using System.Collections.Generic;
using net.r_eg.DllExport.NSBin;
using net.r_eg.DllExport.Wizard.Extensions;
using net.r_eg.MvsSln.Core;
using net.r_eg.MvsSln.Log;
using RGiesecke.DllExport;

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
                "RGiesecke.DllExport",
                "net.r_eg.DllExport"
            };
        }

        public UserConfig(IWizardConfig cfg, IXProject xp)
            : this(cfg)
        {
            if(xp == null) {
                return;
            }

            Namespace   = GetValue(MSBuildProperties.DXP_NAMESPACE, xp);
            Platform    = GetPlatform(GetValue(MSBuildProperties.PRJ_PLATFORM, xp));
            UseCecil    = GetValue(MSBuildProperties.DXP_DDNS_CECIL, xp).ToBoolean();

            var rawTimeout = GetValue(MSBuildProperties.DXP_TIMEOUT, xp);

            Compiler = new CompilerCfg() {
                genExpLib           = GetValue(MSBuildProperties.DXP_GEN_EXP_LIB, xp).ToBoolean(),
                ordinalsBase        = GetValue(MSBuildProperties.DXP_ORDINALS_BASE, xp).ToInteger(),
                ourILAsm            = GetValue(MSBuildProperties.DXP_OUR_ILASM, xp).ToBoolean(),
                customILAsm         = GetUnevaluatedValue(MSBuildProperties.DXP_CUSTOM_ILASM, xp),
                intermediateFiles   = GetValue(MSBuildProperties.DXP_INTERMEDIATE_FILES, xp).ToBoolean(),
                timeout             = String.IsNullOrWhiteSpace(rawTimeout) ? CompilerCfg.TIMEOUT_EXEC : rawTimeout.ToInteger(),
                peCheck             = (PeCheckType)GetValue(MSBuildProperties.DXP_PE_CHECK, xp).ToInteger()
            };
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

        protected Platform GetPlatform(string value)
        {
            if(String.IsNullOrWhiteSpace(value)) {
                return Platform.Default;
            }

            switch(value.Trim().ToLowerInvariant()) {
                case "x86": {
                    return Platform.x86;
                }
                case "x64": {
                    return Platform.x64;
                }
                case "anycpu": {
                    return Platform.AnyCPU;
                }
            }

            LSender.Send(this, $"Incorrect platform target: '{value}'. Use '{nameof(Platform.Default)}'", Message.Level.Warn);
            return Platform.Default;
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

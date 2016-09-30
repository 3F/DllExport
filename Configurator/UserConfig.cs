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
using System.Collections.Generic;

namespace net.r_eg.DllExport.Configurator
{
    public sealed class UserConfig
    {
        public const string NS_DEFAULT_VALUE = "System.Runtime.InteropServices";

        /// <summary>
        /// A selected namespace for dynamic definition features.
        /// </summary>
        public string unamespace;

        /// <summary>
        /// Allowed buffer size for NS.
        /// </summary>
        public int nsBuffer;

        /// <summary>
        /// Predefined list of namespaces.
        /// </summary>
        public List<string> defnamespaces = new List<string>() {
            NS_DEFAULT_VALUE,
            "RGiesecke.DllExport",
            "net.r_eg.DllExport"
        };

        /// <summary>
        /// Script configuration.
        /// </summary>
        public IScriptConfig script;

        /// <summary>
        /// Platform target for project.
        /// </summary>
        public PlatformTarget platform;

        /// <summary>
        /// Settings for ILasm etc.
        /// </summary>
        public CompilerCfg compiler;

        /// <summary>
        /// PublicKeyToken of meta library.
        /// </summary>
        public string MetaLibPublicKeyToken = "8337224C9AD9E356";

        /// <summary>
        /// File name without extension of meta library.
        /// </summary>
        public string MetaLibAsmFileName = "DllExport";

        public enum PlatformTarget
        {
            Default,
            x86,
            x64,
            AnyCPU
        }

        public struct CompilerCfg
        {
            /// <summary>
            /// Base for ordinals.
            /// </summary>
            public int ordinalsBase;
        }

        public UserConfig(IScriptConfig cfg)
        {
            if(cfg == null) {
                throw new ArgumentNullException("Script configuration cannot be null.");
            }
            script = cfg;
        }
    }
}

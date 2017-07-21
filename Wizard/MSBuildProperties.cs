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

namespace net.r_eg.DllExport.Wizard
{
    public struct MSBuildProperties
    {
        /// <summary>
        /// https://github.com/3F/DllExport/issues/2
        /// </summary>
        public const string DXP_NAMESPACE = "DllExportNamespace";

        /// <summary>
        /// https://github.com/3F/DllExport/issues/11#issuecomment-250907940
        /// </summary>
        public const string DXP_ORDINALS_BASE = "DllExportOrdinalsBase";

        /// <summary>
        /// x86 + x64
        /// </summary>
        public const string DXP_SKIP_ANYCPU = "DllExportSkipOnAnyCpu";

        /// <summary>
        /// Cecil or Direct-Mod.
        /// </summary>
        public const string DXP_DDNS_CECIL = "DllExportDDNSCecil";

        /// <summary>
        /// https://github.com/3F/DllExport/issues/9#issuecomment-246189220
        /// </summary>
        public const string DXP_GEN_EXP_LIB = "DllExportGenExpLib";

        /// <summary>
        /// https://github.com/3F/DllExport/issues/17
        /// </summary>
        public const string DXP_OUR_ILASM = "DllExportOurILAsm";

        /// <summary>
        /// Platform Target for binaries.
        /// </summary>
        public const string PRJ_PLATFORM = "PlatformTarget";

        /// <summary>
        /// Used namespace for project.
        /// </summary>
        public const string PRJ_NAMESPACE = "RootNamespace";
    }
}

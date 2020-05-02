/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
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
    internal struct MSBuildTargets
    {
        /// <summary>
        /// The name of target to restore package.
        /// </summary>
        internal const string DXP_PKG_R = "DllExportRestorePkg";

        /// <summary>
        /// Pre-Processing.
        /// https://github.com/3F/DllExport/pull/146
        /// </summary>
        internal const string DXP_PRE_PROC = "DllExportPreProc";

        /// <summary>
        /// Post-Processing.
        /// https://github.com/3F/DllExport/pull/148
        /// </summary>
        internal const string DXP_POST_PROC = RGiesecke.DllExport.MSBuild.PostProc.ENTRY_POINT;

        /// <summary>
        /// Post-Actions of the main Pre-Processing.
        /// </summary>
        internal const string DXP_PRE_PROC_AFTER = "DllExportPreProcAfter";

        /// <summary>
        /// To support dynamic `import` section.
        /// https://github.com/3F/DllExport/issues/62
        /// </summary>
        internal const string DXP_R_DYN = "DllExportRPkgDynamicImport";

        /// <summary>
        /// The entry point of the main task.
        /// </summary>
        internal const string DXP_MAIN = "DllExportMod";

        /// <summary>
        /// Flag when imported the {DXP_MAIN} target.
        /// </summary>
        internal const string DXP_MAIN_FLAG = "DllExportModImported";
    }
}

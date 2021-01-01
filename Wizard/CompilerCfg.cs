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

using RGiesecke.DllExport;

namespace net.r_eg.DllExport.Wizard
{
    public struct CompilerCfg
    {
        public const string PATH_CTM_ILASM = @"$(SolutionDir)bin\";
        public const int TIMEOUT_EXEC = 30000;

        /// <summary>
        /// Base for ordinals.
        /// </summary>
        public int ordinalsBase;

        /// <summary>
        /// Generate .exp + .lib via MS Library Manager.
        /// </summary>
        public bool genExpLib;

        /// <summary>
        /// To use our ILAsm / ILDasm if true.
        /// </summary>
        public bool ourILAsm;

        /// <summary>
        /// Path to custom ILAsm, or null if not used.
        /// </summary>
        public string customILAsm;

        /// <summary>
        /// Rebase System Object: `netstandard` } `System.Runtime` } `mscorlib`
        /// https://github.com/3F/DllExport/issues/125#issuecomment-561245575
        /// </summary>
        public bool rSysObj;

        /// <summary>
        /// Flag to keep intermediate Files (IL Code, Resources, ...).
        /// </summary>
        public bool intermediateFiles;

        /// <summary>
        /// Timeout of execution in milliseconds.
        /// </summary>
        public int timeout;

        /// <summary>
        /// Type of checking PE32/PE32+ module.
        /// </summary>
        public PeCheckType peCheck;

        /// <summary>
        /// Optional patches.
        /// </summary>
        public PatchesType patches;
    }
}

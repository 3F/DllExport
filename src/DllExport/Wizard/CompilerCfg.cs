/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

namespace net.r_eg.DllExport.Wizard
{
    public struct CompilerCfg
    {
        public const string PATH_CTM_ILASM = @"$(SolutionDir)bin\";
        public const int TIMEOUT_EXEC = 30_000;

        /// <summary>
        /// Custom ImageBase. Must be 0x10000 aligned.
        /// </summary>
        public string imageBase;

        /// <summary>
        /// ImageBase step. Must be 0x10000 aligned.
        /// </summary>
        public string imageBaseStep;

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

        /// <summary>
        /// Refresh intermediate module (obj) using modified (bin).
        /// </summary>
        public bool refreshObj;
    }
}

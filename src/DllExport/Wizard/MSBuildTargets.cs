/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
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
        /// [Ref] processing
        /// </summary>
        internal const string DXP_REF_PROC = "DllExportRefProc";

        /// <summary>
        /// Pre-Processing.
        /// https://github.com/3F/DllExport/pull/146
        /// </summary>
        internal const string DXP_PRE_PROC = "DllExportPreProc";

        /// <summary>
        /// Post-Processing.
        /// https://github.com/3F/DllExport/pull/148
        /// </summary>
        internal const string DXP_POST_PROC = Activator.PostProc.ENTRY_POINT;

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

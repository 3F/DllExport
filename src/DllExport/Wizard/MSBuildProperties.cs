/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

namespace net.r_eg.DllExport.Wizard
{
    public struct MSBuildProperties
    {
        public const string DXP_ID = "DllExportIdent";

        /// <summary>
        /// Base path to the manager and its packages
        /// </summary>
        public const string DXP_DIR = "DllExportDir";

        /// <summary>
        /// Meta library file name.
        /// </summary>
        public const string DXP_METALIB_NAME = "DllExportMetaLibName";

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
        /// Path to custom ILAsm.
        /// </summary>
        public const string DXP_CUSTOM_ILASM = "DllExportILAsmCustomPath";

        /// <summary>
        /// https://github.com/3F/DllExport/issues/125
        /// </summary>
        public const string DXP_SYSOBJ_REBASE = "DllExportSysObjRebase";

        /// <summary>
        /// Flag to keep intermediate Files (IL Code, Resources, ...).
        /// </summary>
        public const string DXP_INTERMEDIATE_FILES = "DllExportLeaveIntermediateFiles";

        /// <summary>
        /// Timeout of execution in milliseconds.
        /// </summary>
        public const string DXP_TIMEOUT = "DllExportTimeout";

        /// <summary>
        /// Type of checking PE32/PE32+ module.
        /// </summary>
        public const string DXP_PE_CHECK = "DllExportPeCheck";

        /// <summary>
        /// https://github.com/3F/DllExport/issues/206
        /// </summary>
        public const string DXP_REFRESH_OBJ = "DllExportRefreshObj";

        /// <summary>
        /// Optional patches.
        /// </summary>
        public const string DXP_PATCHES = "DllExportPatches";

        /// <summary>
        /// Platform Target for binaries.
        /// </summary>
        public const string PRJ_PLATFORM = "PlatformTarget";

        /// <summary>
        /// Used namespace for project.
        /// </summary>
        public const string PRJ_NAMESPACE = "RootNamespace";

        /// <summary>
        /// Relative path to project file where it was configured.
        /// </summary>
        public const string DXP_PRJ_FILE = "DllExportProjectFile";

        /// <summary>
        /// DXP_ID for new configure. Presented by 'Recover' action.
        /// </summary>
        public const string DXP_CFG_ID = "DllExportCfgId";

        /// <summary>
        /// Platform for DllExport tool.
        /// </summary>
        public const string DXP_PLATFORM = "DllExportPlatform";

        /// <summary>
        /// Used Pre-Processing type.
        /// https://github.com/3F/DllExport/pull/146
        /// </summary>
        public const string DXP_PRE_PROC_TYPE = "DllExportPreProcType";

        /// <summary>
        /// Used Post-Processing type.
        /// https://github.com/3F/DllExport/pull/148
        /// </summary>
        public const string DXP_POST_PROC_TYPE = "DllExportPostProcType";

        /// <summary>
        /// List of modules for ILMerge if used.
        /// </summary>
        public const string DXP_ILMERGE = "DllExportILMerge";

        /// <summary>
        /// Env cfg for Post-Proc feature.
        /// </summary>
        public const string DXP_PROC_ENV = "DllExportProcEnv";

        /// <summary>
        /// Meta library full path to file.
        /// </summary>
        public const string DXP_METALIB_FPATH = "DllExportMetaLibFullPath";

        public const string DXP_NO_RESTORE = "DllExportNoRestore";

        public const string PRJ_TARGET_DIR = "TargetDir";

        public const string PRJ_TARGET_F = "TargetFileName";

        public const string PRJ_DBG_TYPE = "DebugType";

        public const string PRJ_CP_LOCKFILE_ASM = "CopyLocalLockFileAssemblies";

        public const string SLN_DIR = "SolutionDir";

        public const string SLN_PATH = "SolutionPath";

        public const string MSB_THIS_FULL_FPATH = "MSBuildThisFileFullPath";

        public const string _PATH_TO_RSLV_TARGET_PACK = "_PathToResolvedTargetingPack";
    }
}

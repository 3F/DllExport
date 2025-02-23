/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.IO;
using static net.r_eg.Conari.Static.Members;

namespace net.r_eg.DllExport.UnitedTest._svc
{
    using PeMagic = Conari.PE.WinNT.Magic;

    internal sealed class Assets
    {
        const string X64 = "x64";
        const string X86 = "x86";

        const string ROOT = @"bin\assets\";

        const string PKG = @"DllExport\";

        const string PKG_TOOLS = PKG + @"tools\";

        internal const string PRJ_NETFX = "NetfxAsset";

        internal const string PRJ_NETCORE = "NetCoreAsset";

        internal const string PRJ_NET5_OR_LESS = "Net5OrLessAsset";

        internal static PeMagic MagicAtThis => Is64bit ? PeMagic.PE64 : PeMagic.PE32;

        /// <summary>
        /// Full path to PeViewer from generated package.
        /// </summary>
        internal static string PkgPeViewer => Path.Combine(PkgToolsDir, "PeViewer.exe");

        /// <summary>
        /// Full path to package tools\ directory.
        /// </summary>
        internal static string PkgToolsDir => GetFullPathTo(PKG_TOOLS);

        /// <summary>
        /// Path to bin at NetfxAsset project
        /// </summary>
        internal static string PrjNetfxBin => GetPathToPrj(PRJ_NETFX);

        /// <summary>
        /// x86 modified module at NetfxAsset project
        /// </summary>
        internal static string PrjNetfxX86 => GetDllPath(PRJ_NETFX, X86);

        /// <summary>
        /// x64 modified module at NetfxAsset project
        /// </summary>
        internal static string PrjNetfxX64 => GetDllPath(PRJ_NETFX, X64);

        /// <summary>
        /// original or modified (x86/x64) at NetfxAsset project
        /// </summary>
        internal static string PrjNetfx => GetPathToPrj(PRJ_NETFX);

        /// <summary>
        /// <see cref="PrjNetfxX64"/> or <see cref="PrjNetfxX86"/> depending on env at runtime
        /// </summary>
        internal static string PrjNetfxRuntime => Is64bit ? PrjNetfxX64 : PrjNetfxX86;

        internal static string BaseFullPath => Path.GetFullPath(Path.Combine
        (
            AppDomain.CurrentDomain.BaseDirectory,
            @"..\..\..\..", // common.props: bin\prj\$(MSBuildProjectName)\$(Configuration)
            ROOT
        ));

        internal static string GetDllPath(string project, string arch)
        {
            return Path.Combine(GetPathToPrj(project), arch, project + ".dll");
        }

        internal static string GetNetCoreDll(string tfm, string arch = null)
            => GetDllFor(PRJ_NETCORE, tfm, arch);

        internal static string GetNet5OrLessDll(string tfm, string arch = null)
            => GetDllFor(PRJ_NET5_OR_LESS, tfm, arch);

        internal static string GetDllFor(string project, string tfm, string arch = null)
        {
            return Path.Combine(GetPathToPrj(project), tfm, arch ?? (Is64bit ? X64 : X86), project + ".dll");
        }

        internal static string GetPathToPrj(string name) => GetPathTo("prj", name);

        internal static string GetPathToObj(string name) => GetPathTo("obj", name);

        internal static string GetFullPathTo(params string[] paths) => Path.Combine(BaseFullPath, Path.Combine(paths));

        internal static string GetPathTo(string baseName, string subname)
        {
            return GetFullPathTo
            (
                BaseFullPath,
#if DEBUG
                "Debug",
#else
                "Release",
#endif
                baseName, subname
            )
            + Path.DirectorySeparatorChar;
        }
    }
}

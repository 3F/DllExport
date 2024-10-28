/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.IO;
using static net.r_eg.Conari.Static.Members;

namespace net.r_eg.DllExport.UnitedTest
{
    using PeMagic = Conari.PE.WinNT.Magic;

    internal sealed class Assets
    {
        const string X64 = "x64";
        const string X86 = "x86";

        internal const string PRJ_NETFX = "NetfxAsset";

        internal const string ROOT = @"bin\assets\";

        internal const string PKG = @"bin\assets\DllExport\";

        internal static PeMagic MagicAtThis => Is64bit ? PeMagic.PE64 : PeMagic.PE32;

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

        internal static string GetDllPath(string project) => GetDllPath(project, string.Empty);

        internal static string GetDllPath(string project, string arch)
        {
            return Path.Combine(GetPathToPrj(project), arch, $"{project}.dll");
        }

        internal static string GetPathToPrj(string name) => GetPathTo("prj", name);

        internal static string GetPathToObj(string name) => GetPathTo("obj", name);

        internal static string GetPathTo(string baseName, string subname)
        {
            return Path.GetFullPath(Path.Combine
            (
                AppDomain.CurrentDomain.BaseDirectory,
                @"..\..\..\..", // common.props: bin\prj\$(MSBuildProjectName)\$(Configuration)

                ROOT,
#if DEBUG
                "Debug",
#else
                "Release",
#endif
                baseName, subname
            ))
            + Path.DirectorySeparatorChar;
        }
    }
}

/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

/* 
 *  Via Cecil or direct modification:
    https://github.com/3F/DllExport/issues/2#issuecomment-231593744

    000005B0                 00 C4 7B 01 00 00 00 2F 00 12 05       .Д{..../...
    000005C0  00 00 02 00 00 00 00 00 00 00 00 00 00 00 26 00  ..............&.
    000005D0  20 02 00 00 00 00 00 00 00 49 2E 77 61 6E 74 2E   ........I.want.
    000005E0  74 6F 2E 66 6C 79 00 00 00 00 00 00 00 00 00 00  to.fly..........
    000005F0  00 00 00 00 00 00 03 00 00 00 00 00 00 00 00 00  ................
    00000600  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
 */

using System;
using System.Runtime.InteropServices;


namespace
#if NS_RG
    net.r_eg.DllExport
#elif NS_3F
    net.r_eg.DllExport
#elif NS_RI
    System.Runtime.InteropServices
#else
    // byte-seq via chars: 
    // + Identifier        = [32]bytes
    // + size of buffer    = [ 4]bytes (range: 0000 - FFF9; reserved: FFFA - FFFF)
    // + buffer of n size
    D3F00FF1770DED978EC774BA389F2DC901F4 // 01F4 - allocated buffer size
    .B00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000 // 100
    .C00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
    .D00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
    .E00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
    .F00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
    //= 536
#endif

{
    /// <summary>
    /// To export this as __cdecl C-exported function. Named as current method where is used attribute.
    /// [.NET DllExport]
    /// 
    /// About our meta-information in user-code:
    /// https://github.com/3F/DllExport/issues/16
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class DllExportAttribute: Attribute
    {
        /// <summary>
        /// Specified calling convention.
        /// 
        /// __cdecl is the default convention in .NET DllExport like for other C/C++ programs (Microsoft Specific).
        /// __stdCall mostly used with winapi.
        /// 
        /// https://msdn.microsoft.com/en-us/library/zkwh89ks.aspx
        /// https://msdn.microsoft.com/en-us/library/56h2zst2.aspx
        /// https://github.com/3F/Conari also uses __cdecl by default
        /// </summary>
        public CallingConvention CallingConvention
        {
            get;
            set;
        } = CallingConvention.Cdecl;

        /// <summary>
        /// Optional name for C-exported function.
        /// </summary>
        public string ExportName
        {
            get;
            set;
        }

        /* Available signatures */

        /// <param name="function">Optional name for C-exported function.</param>
        /// <param name="convention">Specified calling convention. __cdecl is the default convention in .NET DllExport.</param>
        public DllExportAttribute(string function, CallingConvention convention) { }

        /// <param name="function">Optional name for C-exported function.</param>
        public DllExportAttribute(string function) { }

        /// <param name="convention">Specified calling convention. __cdecl is the default convention in .NET DllExport.</param>
        public DllExportAttribute(CallingConvention convention) { }

        /// <summary>
        /// To export this as __cdecl C-exported function. Named as current method where is used attribute.
        /// </summary>
        public DllExportAttribute() { }
    }
}

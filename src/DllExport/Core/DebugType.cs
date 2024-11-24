/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;

namespace net.r_eg.DllExport
{
    [Flags]
    public enum DebugType
    {
        Default = DebugOptimize,

        /// <summary>
        /// Disable JIT optimization, create PDB file, use sequence points from PDB.
        /// </summary>
        /// <remarks>/DEBUG</remarks>
        Debug = 0x01,

        /// <summary>
        /// Optimize long instructions to short.
        /// </summary>
        /// <remarks>/OPTIMIZE</remarks>
        Optimize = 0x02,

        /// <summary>
        /// Create the PDB file without enabling debug info tracking.
        /// </summary>
        /// <remarks>/PDB /OPTIMIZE</remarks>
        PdbOptimize = 0x04 | Optimize,

        /// <summary>
        /// Enable JIT optimization, create PDB file, use implicit sequence points.
        /// </summary>
        /// <remarks>/DEBUG=OPT /OPTIMIZE</remarks>
        DebugOptimize = 0x08 | Debug | Optimize,

        /// <summary>
        /// Disable JIT optimization, create PDB file, use implicit sequence points.
        /// </summary>
        /// <remarks>/DEBUG=IMPL</remarks>
        DebugImpl = 0x10 | Debug,

        #region Legacy

        True = Debug,

        False = Optimize,

        #endregion
    }
}

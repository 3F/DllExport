﻿/*!
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
    public enum PeCheckType: int
    {
        None,

        /// <summary>
        /// PE Check 1:1.
        /// Will check count of all planned exports from final PE32/PE32+ module.
        /// </summary>
        Pe1to1 = 0x01,

        /// <summary>
        /// PE Check IL code.
        /// Will check existence of all planned exports (IL code) in actual PE32/PE32+ module.
        /// </summary>
        PeIl = 0x02,

        /// <summary>
        /// PE Check PE32 / PE32+
        /// Will check module type for PE32 or PE32+ regarding selected architecture.
        /// </summary>
        Pe32orPlus = 0x04,
    }
}

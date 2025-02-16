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
    public enum TypeRefOptions: long
    {
        None,

        /// <summary>
        /// $-interpolation using predefined DllExport's stub implementation.
        /// </summary>
        DefaultInterpolatedStringHandler = 0x01,
    }
}

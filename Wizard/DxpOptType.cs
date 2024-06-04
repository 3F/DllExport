/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;

namespace net.r_eg.DllExport.Wizard
{
    [Flags]
    public enum DxpOptType
    {
        None,

        /// <summary>
        /// Do not use manager for automatic restore the remote package.
        /// </summary>
        NoMgr = 0x01,
    }
}
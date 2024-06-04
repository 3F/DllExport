/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;

namespace net.r_eg.DllExport.NSBin
{
    [Serializable]
    public struct MarkerData
    {
        /// <summary>
        /// Full namespace
        /// </summary>
        public string nsName;

        /// <summary>
        /// Position of current nsName
        /// </summary>
        public int nsPosition;

        /// <summary>
        /// Valid buffer size for namespace.
        /// </summary>
        public int nsBuffer;

        /// <summary>
        /// Cecil logic.
        /// </summary>
        public bool viaCecil;
    }
}

/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using net.r_eg.Conari.Log;

namespace net.r_eg.DllExport.NSBin
{
    public interface IDDNS
    {
        /// <summary>
        /// Available buffer for namespace.
        /// </summary>
        int NSBuffer { get; }

        /// <summary>
        /// Access to logger.
        /// </summary>
        ISender Log { get; }

        /// <summary>
        /// Define namespace.
        /// </summary>
        /// <param name="lib">Full path to prepared library.</param>
        /// <param name="name">New namespace.</param>
        /// <param name="useCecil">To use Cecil instead of direct modification.</param>
        /// <param name="preparing">Preparing library is obsolete variant for previous distribution with nuget.</param>
        void setNamespace(string lib, string name, bool useCecil, bool preparing = true);
    }
}

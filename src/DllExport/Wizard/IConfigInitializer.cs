/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using net.r_eg.DllExport.NSBin;
using net.r_eg.MvsSln.Log;

namespace net.r_eg.DllExport.Wizard
{
    public interface IConfigInitializer
    {
        /// <summary>
        /// Access to wizard configuration.
        /// </summary>
        IWizardConfig Config { get; }

        /// <summary>
        /// ddNS feature core.
        /// </summary>
        IDDNS DDNS { get; }

        /// <summary>
        /// Access to logger.
        /// </summary>
        ISender Log { get; }
    }
}

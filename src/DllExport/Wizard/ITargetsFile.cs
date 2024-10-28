/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;

namespace net.r_eg.DllExport.Wizard
{
    public interface ITargetsFile: IProject, IDisposable
    {
        /// <summary>
        /// To configure external .targets through parent project.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        bool Configure(ActionType type, IProject parent);

        /// <summary>
        /// To export data via parent project into .targets file.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        bool Export(IProject parent);

        /// <summary>
        /// Resets data for external .targets file.
        /// </summary>
        void Reset();

        /// <summary>
        /// Saves data to the file system.
        /// </summary>
        /// <param name="reevaluate">Try to reevaluate data of project before saving.</param>
        void Save(bool reevaluate);
    }
}

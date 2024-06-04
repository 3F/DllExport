/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System.Collections.Generic;

namespace net.r_eg.DllExport.Wizard.UI
{
    public interface IExtCfg
    {
        /// <summary>
        /// To get new project list after applying filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="projects"></param>
        /// <returns>Filtered project list.</returns>
        IEnumerable<IProject> FilterProjects(ProjectFilter filter, IEnumerable<IProject> projects);
    }
}

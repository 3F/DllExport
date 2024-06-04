/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

namespace net.r_eg.DllExport.Wizard.UI
{
    public interface IRender
    {
        /// <summary>
        /// To apply filter for rendered projects.
        /// </summary>
        /// <param name="filter"></param>
        void ApplyFilter(ProjectFilter filter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enabled"></param>
        void ShowProgressLine(bool enabled);
    }
}

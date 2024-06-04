/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

namespace net.r_eg.DllExport.Wizard.UI
{
    /// <summary>
    /// TODO:
    /// </summary>
    public struct ProjectFilter
    {
        public static readonly ProjectFilter Default = new ProjectFilter();

        //public IUserConfig config;

        /// <summary>
        /// Project relative path.
        /// </summary>
        public string path;
    }
}

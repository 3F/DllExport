/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System.Collections.Generic;
using net.r_eg.MvsSln.Core;

namespace RGiesecke.DllExport.MSBuild
{
    internal interface IPostProcExecutor
    {
        /// <summary>
        /// Executes IXProject through MSBuild engine.
        /// </summary>
        /// <param name="xp"></param>
        /// <param name="entrypoint">Initial target.</param>
        /// <param name="properties">Additional properties that will be available when executing code.</param>
        /// <returns>True if request was a complete success.</returns>
        bool Execute(IXProject xp, string entrypoint, IDictionary<string, string> properties = null);
    }
}

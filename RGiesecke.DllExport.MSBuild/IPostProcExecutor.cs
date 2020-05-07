//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
//$ Distributed under the MIT License (MIT)

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

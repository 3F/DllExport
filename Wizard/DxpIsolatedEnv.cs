/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using net.r_eg.MvsSln.Core;
using net.r_eg.MvsSln.Log;

namespace net.r_eg.DllExport.Wizard
{
    /// <summary>
    /// Isolates problems like this: https://github.com/3F/DllExport/issues/56
    /// TODO: MvsSln core
    /// 
    /// About possible incorrect Sdk-based project types:
    /// https://github.com/3F/DllExport/pull/123
    /// Planned to drop msbuild support in v3:
    /// https://github.com/3F/MvsSln/issues/23
    /// </summary>
    public class DxpIsolatedEnv: IsolatedEnv, IEnvironment
    {
        public const string ERR_MSG = "DXPInternalErrorMsg";

        public DxpIsolatedEnv(ISlnResult data)
            : base(data)
        {

        }

        protected override Microsoft.Build.Evaluation.Project Load(string path, IDictionary<string, string> properties)
        {
            try
            {
                return base.Load(path, properties);
            }
            catch(Exception ex)
            {
                LSender._.send(this, $"MBE. Found problem when new Project('{path}'): '{ex.Message}'", Message.Level.Debug);

                var prj = new Microsoft.Build.Evaluation.Project();
                prj.SetProperty(ERR_MSG, ex.Message);
                return prj;
            }
        }
    }
}

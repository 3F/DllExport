/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using Microsoft.Build.Construction;
using net.r_eg.MvsSln.Projects;

namespace net.r_eg.DllExport.Wizard
{
    internal interface IProjectSvc: IProject
    {
        ProjectTargetElement AddTarget(string name);

        bool RemoveXmlTarget(string name);

        void SetProperty(string name, string value);

        void SetProperty(string name, bool val);

        void SetProperty(string name, int val);

        void SetProperty(string name, long val);

        IProjectSvc RemovePackageReferences(string id, Func<Item, bool> opt = null, bool wzstrict = true);

        IProjectSvc RemovePackageReferences(Func<Item, bool> opt, bool wzstrict = true);

        IEnumerable<KeyValuePair<string, string>> GetMeta
        (
            bool privateAssets = false, bool hide = false, bool generatePath = false
        );
    }
}

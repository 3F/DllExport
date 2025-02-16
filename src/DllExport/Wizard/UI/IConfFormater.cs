/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;

namespace net.r_eg.DllExport.Wizard.UI
{
    internal interface IConfFormater
    {
        string ParseIfNeeded(IProject prj, Action onParse = null);

        string Parse(IProject project);
    }
}

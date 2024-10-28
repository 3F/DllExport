/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using Microsoft.Build.Utilities;

namespace net.r_eg.DllExport.Activator
{
    public interface IDllExportTask: IInputValues, IServiceProvider
    {
        TaskLoggingHelper Log
        {
            get;
        }

        bool? SkipOnAnyCpu
        {
            get;
            set;
        }

        string TargetFrameworkVersion
        {
            get;
            set;
        }

        string Platform
        {
            get;
            set;
        }
    }
}

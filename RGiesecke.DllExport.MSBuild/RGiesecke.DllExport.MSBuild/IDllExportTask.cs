// [Decompiled] Assembly: RGiesecke.DllExport.MSBuild, Version=1.2.7.38851, Culture=neutral, PublicKeyToken=8f52d83c1a22df51
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using Microsoft.Build.Utilities;

namespace RGiesecke.DllExport.MSBuild
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

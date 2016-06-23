// [Decompiled] Assembly: RGiesecke.DllExport.MSBuild, Version=1.2.1.28778, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using Microsoft.Build.Utilities;

namespace RGiesecke.DllExport.MSBuild
{
    public interface IDllExportTask: IInputValues
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

        string PlatformTarget
        {
            get;
            set;
        }
    }
}

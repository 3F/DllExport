// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.7.38850, Culture=neutral, PublicKeyToken=8f52d83c1a22df51
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System.Runtime.InteropServices;

namespace RGiesecke.DllExport
{
    public interface IExportInfo
    {
        CallingConvention CallingConvention
        {
            get;
            set;
        }

        string ExportName
        {
            get;
            set;
        }

        bool IsStatic
        {
            get;
        }

        bool IsGeneric
        {
            get;
        }

        void AssignFrom(IExportInfo info);
    }
}

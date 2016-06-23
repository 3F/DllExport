// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.6.36226, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
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

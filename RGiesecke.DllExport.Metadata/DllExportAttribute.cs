// [Decompiled] Assembly: RGiesecke.DllExport.Metadata, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8f52d83c1a22df51
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using System.Runtime.InteropServices;

namespace RGiesecke.DllExport
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [Serializable]
    public class DllExportAttribute: Attribute
    {
        public CallingConvention CallingConvention
        {
            get;
            set;
        }

        public string ExportName
        {
            get;
            set;
        }

        public DllExportAttribute()
        {
        }

        public DllExportAttribute(string exportName)
        : this(exportName, CallingConvention.StdCall)
        {
        }

        public DllExportAttribute(string exportName, CallingConvention callingConvention)
        {
            this.ExportName = exportName;
            this.CallingConvention = callingConvention;
        }
    }
}

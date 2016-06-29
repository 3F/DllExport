using System;

namespace System.Runtime.InteropServices
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
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

        public DllExportAttribute(string function, CallingConvention convention)
        {
            ExportName          = function;
            CallingConvention   = convention;
        }

        public DllExportAttribute(string function)
            : this(function, CallingConvention.StdCall)
        {

        }

        public DllExportAttribute(CallingConvention convention)
            : this(null, convention)
        {

        }

        public DllExportAttribute()
        {

        }
    }
}

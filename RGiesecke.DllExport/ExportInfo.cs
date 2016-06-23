// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.1.28776, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using System.Runtime.InteropServices;

namespace RGiesecke.DllExport
{
    [Serializable]
    public class ExportInfo: IExportInfo
    {
        private CallingConvention _CallingConvention;
        private string _ExportName;

        public virtual string ExportName
        {
            get {
                return this._ExportName;
            }

            set {
                this._ExportName = value;
            }
        }

        public CallingConvention CallingConvention
        {
            get {
                return this._CallingConvention;
            }

            set {
                this._CallingConvention = value;
            }
        }

        public void AssignFrom(IExportInfo info)
        {
            if(info == null)
            {
                return;
            }
            this.CallingConvention = info.CallingConvention != (CallingConvention)0 ? info.CallingConvention : CallingConvention.StdCall;
            this.ExportName = info.ExportName;
        }
    }
}

// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.2.23706, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System.Collections.Generic;
using System.Threading;

namespace RGiesecke.DllExport
{
    public class ExportedClass
    {
        private readonly List<ExportedMethod> _Methods = new List<ExportedMethod>();
        private readonly Dictionary<string, List<ExportedMethod>> _MethodsByName = new Dictionary<string, List<ExportedMethod>>();

        public string FullTypeName
        {
            get;
            set;
        }

        internal Dictionary<string, List<ExportedMethod>> MethodsByName
        {
            get {
                return this._MethodsByName;
            }
        }

        internal List<ExportedMethod> Methods
        {
            get {
                return this._Methods;
            }
        }

        internal void Refresh()
        {
            Monitor.Enter((object)this);
            try
            {
                this.MethodsByName.Clear();
                foreach(ExportedMethod method in this.Methods)
                {
                    List<ExportedMethod> exportedMethodList;
                    if(!this.MethodsByName.TryGetValue(method.Name, out exportedMethodList))
                    {
                        this.MethodsByName.Add(method.Name, exportedMethodList = new List<ExportedMethod>());
                    }
                    exportedMethodList.Add(method);
                }
            }
            finally
            {
                Monitor.Exit((object)this);
            }
        }
    }
}

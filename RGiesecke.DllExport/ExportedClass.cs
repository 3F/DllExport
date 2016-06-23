// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.6.36226, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System.Collections.Generic;

namespace RGiesecke.DllExport
{
    public class ExportedClass
    {
        private readonly List<ExportedMethod> _Methods = new List<ExportedMethod>();
        private readonly Dictionary<string, List<ExportedMethod>> _MethodsByName = new Dictionary<string, List<ExportedMethod>>();

        public string FullTypeName
        {
            get;
            private set;
        }

        public bool HasGenericContext
        {
            get;
            private set;
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

        public ExportedClass(string fullTypeName, bool hasGenericContext)
        {
            this.FullTypeName = fullTypeName;
            this.HasGenericContext = hasGenericContext;
        }

        internal void Refresh()
        {
            lock(this)
            {
                this.MethodsByName.Clear();
                foreach(ExportedMethod item_0 in this.Methods)
                {
                    List<ExportedMethod> local_1;
                    if(!this.MethodsByName.TryGetValue(item_0.Name, out local_1))
                    {
                        this.MethodsByName.Add(item_0.Name, local_1 = new List<ExportedMethod>());
                    }
                    local_1.Add(item_0);
                }
            }
        }
    }
}

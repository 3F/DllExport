// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.1.28776, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RGiesecke.DllExport
{
    public sealed class AssemblyExports
    {
        private readonly Dictionary<string, ExportedClass> _ClassesByName = new Dictionary<string, ExportedClass>();
        private readonly List<DuplicateExports> _DuplicateExportMethods = new List<DuplicateExports>();
        private readonly Dictionary<object, ExportedMethod> _DuplicateExportMethodsbyFullName = new Dictionary<object, ExportedMethod>();
        private readonly Dictionary<string, ExportedMethod> _MethodsByExportName = new Dictionary<string, ExportedMethod>();

        internal static readonly Dictionary<CallingConvention, string> ConventionTypeNames = new Dictionary<CallingConvention, string>() { { CallingConvention.Cdecl, typeof (CallConvCdecl).FullName }, { CallingConvention.FastCall, typeof (CallConvFastcall).FullName }, { CallingConvention.StdCall, typeof (CallConvStdcall).FullName }, { CallingConvention.ThisCall, typeof (CallConvThiscall).FullName }, { CallingConvention.Winapi, typeof (CallConvStdcall).FullName }
    };

        private readonly ReadOnlyCollection<DuplicateExports> _ReadOnlyDuplicateExportMethods;

        public ReadOnlyCollection<DuplicateExports> DuplicateExportMethods
        {
            get {
                return this._ReadOnlyDuplicateExportMethods;
            }
        }

        public IInputValues InputValues
        {
            get;
            set;
        }

        public string DllExportAttributeAssemblyName
        {
            get {
                if(this.InputValues == null)
                {
                    return Utilities.DllExportAttributeAssemblyName;
                }
                return this.InputValues.DllExportAttributeAssemblyName;
            }
        }

        public string DllExportAttributeFullName
        {
            get {
                if(this.InputValues == null)
                {
                    return Utilities.DllExportAttributeFullName;
                }
                return this.InputValues.DllExportAttributeFullName;
            }
        }

        internal Dictionary<string, ExportedMethod> MethodsByExportName
        {
            get {
                return this._MethodsByExportName;
            }
        }

        internal Dictionary<string, ExportedClass> ClassesByName
        {
            get {
                return this._ClassesByName;
            }
        }

        public int Count
        {
            get {
                return this.MethodsByExportName.Count;
            }
        }

        public AssemblyExports()
        {
            this._ReadOnlyDuplicateExportMethods = new ReadOnlyCollection<DuplicateExports>((IList<DuplicateExports>)this._DuplicateExportMethods);
        }

        internal void Refresh()
        {
            int num = 0;
            this.MethodsByExportName.Clear();
            this._DuplicateExportMethods.Clear();
            Dictionary<string, DuplicateExports> dictionary = new Dictionary<string, DuplicateExports>();
            foreach(ExportedClass exportedClass in this.ClassesByName.Values)
            {
                List<ExportedMethod> exportedMethodList = new List<ExportedMethod>(exportedClass.Methods.Count);
                foreach(ExportedMethod method in exportedClass.Methods)
                {
                    DuplicateExports duplicateExports;
                    if(!dictionary.TryGetValue(method.ExportName, out duplicateExports))
                    {
                        method.VTableOffset = num++;
                        this.MethodsByExportName.Add(method.MemberName, method);
                        dictionary.Add(method.ExportName, new DuplicateExports(method));
                    }
                    else
                    {
                        exportedMethodList.Add(method);
                        duplicateExports.Duplicates.Add(method);
                    }
                }
                ExportedClass exportClassCopy = exportedClass;
                exportedMethodList.ForEach((Action<ExportedMethod>)(m => exportClassCopy.Methods.Remove(m)));
                exportedClass.Refresh();
            }
            foreach(DuplicateExports duplicateExports in dictionary.Values)
            {
                if(duplicateExports.Duplicates.Count > 0)
                {
                    this._DuplicateExportMethods.Add(duplicateExports);
                    foreach(ExportedMethod duplicate in (IEnumerable<ExportedMethod>)duplicateExports.Duplicates)
                    {
                        this._DuplicateExportMethodsbyFullName.Add(AssemblyExports.GetKey(duplicate.ExportedClass.FullTypeName, duplicate.MemberName), duplicate);
                    }
                }
            }
            this._DuplicateExportMethods.Sort((Comparison<DuplicateExports>)((l, r) => string.CompareOrdinal(l.UsedExport.ExportName, r.UsedExport.ExportName)));
        }

        public ExportedMethod GetDuplicateExport(string fullTypeName, string memberName)
        {
            ExportedMethod exportedMethod;
            if(!this.TryGetDuplicateExport(fullTypeName, memberName, out exportedMethod))
            {
                return (ExportedMethod)null;
            }
            return exportedMethod;
        }

        public bool TryGetDuplicateExport(string fullTypeName, string memberName, out ExportedMethod exportedMethod)
        {
            return this._DuplicateExportMethodsbyFullName.TryGetValue(AssemblyExports.GetKey(fullTypeName, memberName), out exportedMethod);
        }

        private static object GetKey(string fullTypeName, string memberName)
        {
            return (object)new { fullTypeName = fullTypeName, memberName = memberName };
        }
    }
}

//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2018  Denis Kuzmin < entry.reg@gmail.com > :: github.com/3F
//$ Distributed under the MIT License (MIT)

namespace RGiesecke.DllExport
{
    public sealed class ExportedMethod: ExportInfo
    {
        private readonly ExportedClass _ExportedClass;

        public string Name
        {
            get {
                return this.MemberName;
            }
        }

        public ExportedClass ExportedClass
        {
            get {
                return this._ExportedClass;
            }
        }

        public string MemberName
        {
            get;
            set;
        }

        public int VTableOffset
        {
            get;
            set;
        }

        public override string ExportName
        {
            get {
                return base.ExportName ?? this.Name;
            }

            set {
                base.ExportName = value;
            }
        }

        public ExportedMethod(ExportedClass exportedClass)
        {
            this._ExportedClass = exportedClass;
        }
    }
}

/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

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

//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2019  Denis Kuzmin < entry.reg@gmail.com > GitHub/3F
//$ Distributed under the MIT License (MIT)

using System.Collections.Generic;

namespace RGiesecke.DllExport
{
    public sealed class DuplicateExports
    {
        private readonly List<ExportedMethod> _Duplicates = new List<ExportedMethod>();
        private readonly ExportedMethod _UsedExport;

        public ExportedMethod UsedExport
        {
            get {
                return this._UsedExport;
            }
        }

        public ICollection<ExportedMethod> Duplicates
        {
            get {
                return (ICollection<ExportedMethod>)this._Duplicates;
            }
        }

        internal DuplicateExports(ExportedMethod usedExport)
        {
            this._UsedExport = usedExport;
        }
    }
}

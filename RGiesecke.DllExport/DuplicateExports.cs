// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.1.28776, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

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

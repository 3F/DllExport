/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System.Collections.Generic;

namespace net.r_eg.DllExport
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

/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using net.r_eg.DllExport.ILAsm;

namespace net.r_eg.DllExport.Wizard
{
    public sealed class RefPackage: AsmDirectiveAbstract
    {
        public string Version { get; set; }

        /// <remarks>E.g. netstandard2.0 or lib\netstandard2.0\System.Memory.dll</remarks>
        public string TfmOrPath { get; set; }

        public bool HasPath => TfmOrPath?.LastIndexOf(".dll", StringComparison.OrdinalIgnoreCase) != -1;

        public override string Serialize() => Pack(Name, Version, TfmOrPath);

        public override AsmDirectiveAbstract Deserialize(string serialized)
        {
            object[] v = Unpack(serialized);

            Name = ToS(v[0]);
            Version = ToS(v[1]);
            TfmOrPath = ToS(v[2]);

            return ReturnObjects(v, max: 3);
        }

        public override IEnumerable<string> Format() => throw new NotSupportedException();
    }
}

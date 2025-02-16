/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System.Collections.Generic;

namespace net.r_eg.DllExport.ILAsm
{
    public sealed class AssemblyExternDirective: AsmDirectiveAbstract
    {
        /// <remarks>E.g. CC 7B 13 FF CD 2D DD 51 </remarks>
        public string Publickeytoken { get; set; }

        /// <summary>
        /// major:minor:build:rev
        /// </summary>
        /// <remarks>E.g. 4:0:2:0</remarks>
        public string Version { get; set; }

        public override string Serialize() => Pack(Name, Publickeytoken, Version);

        public override AsmDirectiveAbstract Deserialize(string serialized)
        {
            object[] v = Unpack(serialized);

            Name = ToS(v[0]);
            Publickeytoken = ToS(v[1]);
            Version = ToS(v[2]);

            return ReturnObjects(v, max: 3);
        }

        public override IEnumerable<string> Format()
        {
            yield return $".assembly extern '{Name}'";
            yield return "{";
            yield return $"  .publickeytoken = ({Publickeytoken} )";
            yield return "  .ver " + Version;
            yield return "}";
        }
    }
}

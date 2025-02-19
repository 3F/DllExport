/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using net.r_eg.Conari.Extension;

namespace net.r_eg.DllExport.Parsing.Actions
{
    internal sealed class ExternalAssemlyDeclaration(int inputLineIndex, string assemblyName, string aliasName)
    {
        public int InputLineIndex { get; private set; } = inputLineIndex;

        public string AssemblyName { get; private set; } = assemblyName;

        public string AliasName { get; private set; } = aliasName ?? assemblyName;

        public string Publickeytoken { get; set; }

        public static bool operator ==(ExternalAssemlyDeclaration a, ExternalAssemlyDeclaration b)
        {
            return a is null ? b is null : a.Equals(b);
        }

        public static bool operator !=(ExternalAssemlyDeclaration a, ExternalAssemlyDeclaration b) => !(a == b);

        public override bool Equals(object obj)
        {
            if(obj is null || obj is not ExternalAssemlyDeclaration b) return false;
            return AssemblyName == b.AssemblyName
                    && Publickeytoken == b.Publickeytoken
                    && AliasName == b.AliasName;
        }

        public override int GetHashCode() => 0.CalculateHashCode
        (
            AssemblyName,
            AliasName,
            Publickeytoken,
            InputLineIndex
        );

        public ExternalAssemlyDeclaration(string assemblyName, string publickeytoken, string alias = null)
            : this(inputLineIndex: -1, assemblyName, alias)
        {
            Publickeytoken = publickeytoken;
        }
    }
}

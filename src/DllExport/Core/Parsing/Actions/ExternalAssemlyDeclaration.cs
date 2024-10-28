/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

namespace net.r_eg.DllExport.Parsing.Actions
{
    public sealed class ExternalAssemlyDeclaration
    {
        public int InputLineIndex
        {
            get;
            private set;
        }

        public string AssemblyName
        {
            get;
            private set;
        }

        public string AliasName
        {
            get;
            private set;
        }

        public ExternalAssemlyDeclaration(int inputLineIndex, string assemblyName, string aliasName)
        {
            this.InputLineIndex = inputLineIndex;
            this.AssemblyName = assemblyName;
            this.AliasName = aliasName;
        }
    }
}

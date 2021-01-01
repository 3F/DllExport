//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2021  Denis Kuzmin <x-3F@outlook.com> github/3F
//$ Distributed under the MIT License (MIT)

namespace RGiesecke.DllExport.Parsing.Actions
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

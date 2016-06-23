// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.6.36226, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

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

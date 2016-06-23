// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.7.38850, Culture=neutral, PublicKeyToken=8f52d83c1a22df51
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

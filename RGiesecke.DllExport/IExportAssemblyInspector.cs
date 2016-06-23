// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.7.38850, Culture=neutral, PublicKeyToken=8f52d83c1a22df51
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System.IO;
using Mono.Cecil;

namespace RGiesecke.DllExport
{
    internal interface IExportAssemblyInspector
    {
        IInputValues InputValues
        {
            get;
        }

        AssemblyExports ExtractExports();

        AssemblyExports ExtractExports(AssemblyDefinition assemblyDefinition);

        AssemblyExports ExtractExports(string fileName);

        AssemblyExports ExtractExports(AssemblyDefinition assemblyDefinition, ExtractExportHandler exportFilter);

        AssemblyExports ExtractExports(string fileName, ExtractExportHandler exportFilter);

        AssemblyBinaryProperties GetAssemblyBinaryProperties(string assemblyFileName);

        AssemblyDefinition LoadAssembly(string fileName);

        bool SafeExtractExports(string fileName, Stream stream);
    }
}

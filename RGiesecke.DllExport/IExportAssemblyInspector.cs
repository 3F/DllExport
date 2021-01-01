//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2021  Denis Kuzmin <x-3F@outlook.com> github/3F
//$ Distributed under the MIT License (MIT)

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

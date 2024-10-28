/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System.IO;
using Mono.Cecil;

namespace net.r_eg.DllExport
{
    internal interface IExportAssemblyInspector
    {
        IInputValues InputValues { get; }

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

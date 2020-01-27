//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
//$ Distributed under the MIT License (MIT)

using Mono.Cecil;

namespace RGiesecke.DllExport
{
    /// <summary>
    /// Implements disabled assembly resolving.
    /// Related issue: https://github.com/3F/DllExport/issues/127
    /// 
    /// For issue127 we can also use DefaultAssemblyResolver.AddSearchDirectory(),
    /// But we don't actually need resolving of any ref assemblies. Only ref types in our meta attr.
    /// </summary>
    internal sealed class DisabledAssemblyResolver: DefaultAssemblyResolver
    {
        public override AssemblyDefinition Resolve(AssemblyNameReference aref)
        {
            // https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.callingconvention
            // Assemblies: System.Runtime.InteropServices.dll, mscorlib.dll, netstandard.dll

            if(aref.Name == "System.Runtime.InteropServices" || aref.Name == "mscorlib" || aref.Name == "netstandard")
            {
                return base.Resolve(aref);
            }

            return null; // It will be considered as `resolved = false` due to internal ResolutionException processing in cecil.
        }
    }
}

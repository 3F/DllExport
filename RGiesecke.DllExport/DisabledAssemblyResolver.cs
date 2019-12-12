//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2019  Denis Kuzmin < entry.reg@gmail.com > GitHub/3F
//$ Distributed under the MIT License (MIT)

using Mono.Cecil;

namespace RGiesecke.DllExport
{
    /// <summary>
    /// Implements disabled assembly resolving.
    /// Related issue: https://github.com/3F/DllExport/issues/127
    /// 
    /// For issue127 we can also use DefaultAssemblyResolver.AddSearchDirectory(),
    /// But we don't actually need any assembly resolving in our case at all.
    /// </summary>
    internal sealed class DisabledAssemblyResolver: DefaultAssemblyResolver
    {
        public override AssemblyDefinition Resolve(AssemblyNameReference name)
        {
            return null; // It will be considered as `resolved = false` due to internal ResolutionException processing in cecil.
        }
    }
}

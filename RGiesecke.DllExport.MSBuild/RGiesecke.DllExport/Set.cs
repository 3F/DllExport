// [Decompiled] Assembly: RGiesecke.DllExport.MSBuild, Version=1.2.7.38851, Culture=neutral, PublicKeyToken=8f52d83c1a22df51
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using Mono.Cecil;

namespace RGiesecke.DllExport
{
    internal static class Set
    {
        public static bool Contains(this ModuleAttributes input, ModuleAttributes set)
        {
            return (input & set) == input;
        }

        public static bool Contains(this TargetArchitecture input, TargetArchitecture set)
        {
            return (input & set) == input;
        }
    }
}

/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using Mono.Cecil;

namespace net.r_eg.DllExport.Extensions
{
    internal static class BinaryExtension
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

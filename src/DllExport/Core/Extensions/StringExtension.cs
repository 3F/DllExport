/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

namespace net.r_eg.DllExport.Extensions
{
    internal static class StringExtension
    {
        internal static bool IsTrue(this string str)
        {
            if(string.IsNullOrEmpty(str)) return false;

            return str switch
            {
                "1" or "true" or "True" or "TRUE" => true,
                "yes" or "Yes" or "YES" => true, // legacy
                _ => false,
            };
        }

        internal static string NullIfEmpty(this string input)
        {
            return string.IsNullOrEmpty(input) ? null : input;
        }
    }
}

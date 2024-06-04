/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

namespace RGiesecke.DllExport.Extensions
{
    internal static class StringExtension
    {
        internal static bool IsTrue(this string str)
        {
            if(string.IsNullOrEmpty(str))
            {
                return false;
            }

            switch(str)
            {
                case "1": case "true": case "True": case "TRUE": return true;
                /* legacy */ case "yes": case "Yes": case "YES": return true;
            }

            return false;
        }
    }
}

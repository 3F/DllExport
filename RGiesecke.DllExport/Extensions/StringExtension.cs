//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2021  Denis Kuzmin <x-3F@outlook.com> github/3F
//$ Distributed under the MIT License (MIT)

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

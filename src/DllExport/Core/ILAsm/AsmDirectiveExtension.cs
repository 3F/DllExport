/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace net.r_eg.DllExport.ILAsm
{
    internal static class AsmDirectiveExtension
    {
        public static string Serialize(this IEnumerable<AsmDirectiveAbstract> src)
        {
            StringBuilder sb = new();
            foreach(AsmDirectiveAbstract directive in src) sb.Append(directive.Serialize());
            return sb.ToString();
        }

        public static IEnumerable<T> Deserialize<T>(this string serialized)
            where T : AsmDirectiveAbstract, new()
        {
            if(string.IsNullOrWhiteSpace(serialized)) yield break;
            foreach(string raw in serialized.Split([';'], StringSplitOptions.RemoveEmptyEntries))
            {
                if(string.IsNullOrWhiteSpace(raw)) continue;

                T obj = new();
                obj.Deserialize(raw);
                yield return obj;
            }
        }
    }
}

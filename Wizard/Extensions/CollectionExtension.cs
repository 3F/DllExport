/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System.Collections.Generic;

namespace net.r_eg.DllExport.Wizard.Extensions
{
    internal static class CollectionExtension
    {
        /// <summary>
        /// To combine string items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="elem">An item to combine with general list if it's not null or empty.</param>
        /// <param name="before">Place it before if true, otherwise after an general list.</param>
        /// <returns></returns>
        public static IEnumerable<string> Combine(this IEnumerable<string> items, string elem, bool before = false)
        {
            if(before && !string.IsNullOrEmpty(elem)) {
                yield return elem;
            }

            foreach(var item in items) {
                yield return item;
            }

            if(!before && !string.IsNullOrEmpty(elem)) {
                yield return elem;
            }
        }
    }
}
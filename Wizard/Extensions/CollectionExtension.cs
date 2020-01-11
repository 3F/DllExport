/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/

using System;
using System.Collections.Generic;

namespace net.r_eg.DllExport.Wizard.Extensions
{
    public static class CollectionExtension
    {
        /// <summary>
        /// Foreach in Linq manner.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="act">The action that should be executed for each item.</param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> act)
        {
            if(items == null) {
                return;
            }

            foreach(var item in items) {
                act(item);
            }
        }

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
            if(before && !String.IsNullOrEmpty(elem)) {
                yield return elem;
            }

            foreach(var item in items) {
                yield return item;
            }

            if(!before && !String.IsNullOrEmpty(elem)) {
                yield return elem;
            }
        }
    }
}
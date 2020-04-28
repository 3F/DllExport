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
using net.r_eg.MvsSln.Core;
using net.r_eg.MvsSln.Extensions;

namespace net.r_eg.DllExport.Wizard.Extensions
{
    internal static class XProjectExtension
    {
        /// <summary>
        /// To get property value with global scope by default.
        /// </summary>
        /// <param name="xp"></param>
        /// <param name="name">The name of the property.</param>
        /// <param name="localScope">If true, will return default value for any special and imported properties type.</param>
        /// <returns>The evaluated property value, which is never null.</returns>
        public static string GetPropertyValue(this IXProject xp, string name, bool localScope = false)
        {
            return xp?.GetProperty(name, localScope).evaluatedValue;
        }

        /// <summary>
        /// To get unevaluated property value with global scope by default.
        /// </summary>
        /// <param name="xp"></param>
        /// <param name="name">The name of the property.</param>
        /// <param name="localScope">If true, will return default value for any special and imported properties type.</param>
        /// <returns>The unevaluated property value, which is never null.</returns>
        public static string GetUnevaluatedPropertyValue(this IXProject xp, string name, bool localScope = false)
        {
            return xp?.GetProperty(name, localScope).unevaluatedValue;
        }

        /// <summary>
        /// Get unique identifier for project (not instance).
        /// TODO: MvsSln should provide similar PId with v2.0.1+
        /// </summary>
        /// <param name="xp"></param>
        /// <returns></returns>
        public static Guid GetPId(this IXProject xp)
        {
            if(xp == null) {
                return Guid.Empty;
            }

            var pItem = xp.ProjectItem;
            return (
                pItem.project.pGuid
                    + pItem.projectConfig 
                    + pItem.solutionConfig
            )
            .Guid();
        }

        public static void AddPackageIfNotExists(this IXProject xp, string id, string version)
        {
            if(xp == null) {
                throw new ArgumentNullException(nameof(xp)); 
            }

            if(xp.GetFirstPackageReference(id ?? throw new ArgumentNullException(nameof(id))).parentItem == null) {
                xp.AddPackageReference(id, version);
            }
        }
    }
}
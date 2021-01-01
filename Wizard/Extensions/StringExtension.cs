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
using System.IO;

namespace net.r_eg.DllExport.Wizard.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// To get boolean value from string.
        /// </summary>
        /// <param name="value">Any compatible value.</param>
        /// <returns></returns>
        public static bool ToBoolean(this string value)
        {
            if(string.IsNullOrWhiteSpace(value)) {
                return false;
            }

            switch(value.Trim())
            {
                case "1":
                case "True":
                case "TRUE":
                case "true": {
                    return true;
                }
                case "0":
                case "False":
                case "FALSE":
                case "false": {
                    return false;
                }
            }
            throw new ArgumentException($"Incorrect boolean value - '{value}'");
        }

        /// <summary>
        /// To get integer value from string.
        /// </summary>
        /// <param name="value">Any compatible value.</param>
        /// <returns></returns>
        public static int ToInteger(this string value)
        {
            if(string.IsNullOrWhiteSpace(value)) {
                return 0;
            }

            return Int32.Parse(value);
        }

        /// <summary>
        /// To get long integer value from string.
        /// </summary>
        /// <param name="value">Any compatible value.</param>
        /// <returns></returns>
        public static long ToLongInteger(this string value)
        {
            if(string.IsNullOrWhiteSpace(value)) {
                return 0;
            }

            return Int64.Parse(value);
        }

        /// <summary>
        /// Open url through default application.
        /// </summary>
        /// <param name="url"></param>
        public static void OpenUrl(this string url)
        {
            if(!string.IsNullOrWhiteSpace(url)) {
                System.Diagnostics.Process.Start(url);
            }
        }

        internal static bool CmpPublicKeyTokenWith(this string pkToken, string pkTokenAsm)
        {
            if(pkTokenAsm == null || string.IsNullOrWhiteSpace(pkToken)) {
                return false;
            }
            return pkToken.Equals(pkTokenAsm, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// To open value from double quotes if they present.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string OpenDoubleQuotes(this string value)
        {
            if(string.IsNullOrWhiteSpace(value)) {
                return value;
            }

            var v = value.Trim();

            if(v[0] == '"' && v.Length > 1 && v[v.Length - 1] == '"') {
                return v.Substring(1, v.Length - 2);
            }

            return v;
        }

        internal static string FindUpDirUsingFile(this string path, string pattern)
        {
            if(string.IsNullOrWhiteSpace(pattern)) throw new ArgumentOutOfRangeException(nameof(pattern));

            var dir = new DirectoryInfo(path ?? throw new ArgumentNullException(nameof(path)));
            while(dir.Parent != null)
            {
                foreach(var _ in dir.EnumerateFiles(pattern)) return dir.FullName;
                dir = dir.Parent;
            }

            return null;
        }

        internal static string UpDir(this string path, int level = 1)
        {
            var dir = new DirectoryInfo(path ?? throw new ArgumentNullException(nameof(path)));
            
            while(dir.Parent != null && level-- > 0) dir = dir.Parent;
            return dir?.FullName;
        }

        /// <summary>
        /// Formatting of the path to directory.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string DirectoryPathFormat(this string path, string root = null)
        {
            if(string.IsNullOrWhiteSpace(path)) return root;

            return CombineRootPath(
                MvsSln.Extensions.StringExtension.DirectoryPathFormat(OpenDoubleQuotes(path)),
                root
            );
        }

        /// <summary>
        /// Formatting file path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FilePathFormat(this string path, string root = null)
        {
            return CombineRootPath(OpenDoubleQuotes(path)?.Trim(), root);
        }

        /// <summary>
        /// To combine relative path with root.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        private static string CombineRootPath(string path, string root)
        {
            if(string.IsNullOrWhiteSpace(path) || Path.IsPathRooted(path)) {
                return path;
            }

            if(string.IsNullOrWhiteSpace(root)) {
                return path;
            }

            return Path.Combine(root, path);
        }
    }
}
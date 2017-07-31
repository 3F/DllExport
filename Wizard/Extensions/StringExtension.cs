﻿/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2017  Denis Kuzmin < entry.reg@gmail.com > :: github.com/3F
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
using System.Security.Cryptography;
using System.Text;

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
            if(String.IsNullOrWhiteSpace(value)) {
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
            if(String.IsNullOrWhiteSpace(value)) {
                return 0;
            }

            return Int32.Parse(value);
        }

        /// <summary>
        /// Open url through default application.
        /// </summary>
        /// <param name="url"></param>
        public static void OpenUrl(this string url)
        {
            if(!String.IsNullOrWhiteSpace(url)) {
                System.Diagnostics.Process.Start(url);
            }
        }

        /// <summary>
        /// Calculate SHA-1 hash from file.
        /// </summary>
        /// <param name="file">Path to file.</param>
        /// <returns>SHA-1 Hash code.</returns>
        public static string SHA1HashFromFile(this string file)
        {
            using(var fs = File.OpenRead(file))
            {
                using(SHA1 sha1 = SHA1.Create()) {
                    return BytesToHexView(sha1.ComputeHash(fs));
                }
            }
        }

        /// <summary>
        /// To open value from double quotes.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string OpenDoubleQuotes(this string value)
        {
            if(String.IsNullOrWhiteSpace(value)) {
                return value;
            }

            // leave trailing and leading whitespaces inside double quotes
            return value.Trim().Trim(new[] { '"' });
        }

        /// <summary>
        /// Formatting of the path to directory.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string DirectoryPathFormat(this string path, string root = null)
        {
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
        public static string CombineRootPath(string path, string root)
        {
            if(String.IsNullOrWhiteSpace(path) || Path.IsPathRooted(path)) {
                return path;
            }

            if(String.IsNullOrWhiteSpace(root)) {
                return path;
            }

            return Path.Combine(root, path);
        }

        /// <summary>
        /// To format bytes data to hex view.
        /// </summary>
        /// <param name="data">Bytes data.</param>
        /// <returns>Hex view of bytes.</returns>
        private static string BytesToHexView(byte[] data)
        {
            var ret = new StringBuilder();
            foreach(byte b in data) {
                ret.Append(b.ToString("X2"));
            }
            return ret.ToString();
        }
    }
}
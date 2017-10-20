/*
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

using System.Collections.Generic;
using net.r_eg.MvsSln.Core;

namespace net.r_eg.DllExport.Wizard
{
    public interface IProject
    {
        /// <summary>
        /// Access to found project.
        /// </summary>
        IXProject XProject { get; }

        /// <summary>
        /// Installation checking.
        /// </summary>
        bool Installed { get; }

        /// <summary>
        /// Special identifier. Like `ProjectGuid` that is not available in SDK-based projects.
        /// https://github.com/3F/DllExport/issues/36#issuecomment-320794498
        /// </summary>
        string DxpIdent { get; }

        /// <summary>
        /// Relative path from location of sln file.
        /// </summary>
        string ProjectPath { get; }

        /// <summary>
        /// Get defined namespace for project.
        /// </summary>
        string ProjectNamespace { get; }

        /// <summary>
        /// Checks usage of external storage for this project.
        /// </summary>
        bool HasExternalStorage { get; }

        /// <summary>
        /// Active configuration of user data.
        /// </summary>
        IUserConfig Config { get; set; }

        /// <summary>
        /// List of used MSBuild properties.
        /// </summary>
        IDictionary<string, string> ConfigProperties { get; }

        /// <summary>
        /// Returns fullpath to meta library for current project.
        /// </summary>
        /// <param name="evaluate">Will return unevaluated value if false.</param>
        /// <returns></returns>
        string MetaLib(bool evaluate);

        /// <summary>
        /// To configure project via specific action.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool Configure(ActionType type);
    }
}

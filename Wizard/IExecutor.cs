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
using net.r_eg.DllExport.NSBin;
using net.r_eg.MvsSln.Log;

namespace net.r_eg.DllExport.Wizard
{
    public interface IExecutor: IDisposable
    {
        /// <summary>
        /// Access to wizard configuration.
        /// </summary>
        IWizardConfig Config { get; }

        ISender Log { get; }

        /// <summary>
        /// ddNS feature core.
        /// </summary>
        IDDNS DDNS { get; }

        /// <summary>
        /// Latest selected .sln file.
        /// </summary>
        string ActiveSlnFile { get; set; }

        /// <summary>
        /// List of available .sln files.
        /// </summary>
        IEnumerable<string> SlnFiles { get; }

        /// <summary>
        /// Access to used external .targets.
        /// </summary>
        ITargetsFile TargetsFile { get; }

        /// <summary>
        /// List of all found projects with different configurations.
        /// </summary>
        /// <param name="sln">Full path to .sln</param>
        /// <returns></returns>
        IEnumerable<IProject> ProjectsBy(string sln);

        /// <summary>
        /// List of all found projects that's unique by guid.
        /// </summary>
        /// <param name="sln"></param>
        /// <returns></returns>
        IEnumerable<IProject> UniqueProjectsBy(string sln);

        /// <summary>
        /// To start process of the required configuration.
        /// </summary>
        void Configure();
    }
}

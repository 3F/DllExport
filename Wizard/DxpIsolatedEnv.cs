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

using System;
using System.Collections.Generic;
using net.r_eg.MvsSln.Core;
using net.r_eg.MvsSln.Log;

namespace net.r_eg.DllExport.Wizard
{
    /// <summary>
    /// https://github.com/3F/DllExport/issues/56
    /// TODO: MvsSln core
    /// </summary>
    public class DxpIsolatedEnv: IsolatedEnv, IEnvironment
    {
        public const string ERR_MSG = "DXPInternalErrorMsg";

        public DxpIsolatedEnv(ISlnResult data)
            : base(data)
        {

        }

        protected override Microsoft.Build.Evaluation.Project Load(string path, IDictionary<string, string> properties)
        {
            try
            {
                return base.Load(path, properties);
            }
            catch(Exception ex)
            {
                LSender._.send(this, $"MBE. Found problem when new Project('{path}'): '{ex.Message}'", Message.Level.Debug);

                var prj = new Microsoft.Build.Evaluation.Project();
                prj.SetProperty(ERR_MSG, ex.Message);
                return prj;
            }
        }
    }
}

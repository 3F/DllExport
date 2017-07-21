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
using net.r_eg.DllExport.NSBin;
using net.r_eg.MvsSln.Log;

namespace net.r_eg.DllExport.Wizard
{
    public interface IUserConfig
    {
        /// <summary>
        /// Flag of installation.
        /// </summary>
        bool Install { get; set; }

        /// <summary>
        /// A selected namespace for ddNS feature.
        /// </summary>
        string Namespace { get; set; }

        /// <summary>
        /// Allowed buffer size for NS.
        /// </summary>
        int NSBuffer { get; set; }

        /// <summary>
        /// To use Cecil instead of direct modifications.
        /// </summary>
        bool UseCecil { get; set; }

        /// <summary>
        /// Predefined list of namespaces.
        /// </summary>
        List<string> Namespaces { get; set; }

        /// <summary>
        /// Access to wizard configuration.
        /// </summary>
        IWizardConfig Wizard { get; set; }

        /// <summary>
        /// Access to ddNS core.
        /// </summary>
        IDDNS DDNS { get; set; }

        /// <summary>
        /// Specific logger.
        /// </summary>
        ISender Log { get; set; }

        /// <summary>
        /// Platform target for project.
        /// </summary>
        Platform Platform { get; set; }

        /// <summary>
        /// Settings for ILasm etc.
        /// </summary>
        CompilerCfg Compiler { get; set; }

        /// <summary>
        /// Adds to top new namespace into Namespaces property.
        /// </summary>
        /// <param name="ns"></param>
        /// <returns>true if added.</returns>
        bool AddTopNamespace(string ns);
    }
}

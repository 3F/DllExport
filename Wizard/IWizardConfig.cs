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

namespace net.r_eg.DllExport.Wizard
{
    public interface IWizardConfig
    {
        /// <summary>
        /// Optional root path of user paths. 
        /// Affects on SlnFile, SlnDir, PkgPath.
        /// </summary>
        string RootPath { get; }

        /// <summary>
        /// Path to directory with .sln files to be processed.
        /// </summary>
        string SlnDir { get; }

        /// <summary>
        /// Optional predefined .sln file to process via the restore operations etc.
        /// </summary>
        string SlnFile { get; }

        /// <summary>
        /// Root path of the DllExport package.
        /// </summary>
        string PkgPath { get; }

        /// <summary>
        /// Relative path to meta library.
        /// </summary>
        string MetaLib { get; }

        /// <summary>
        /// Relative path to meta core library.
        /// </summary>
        string MetaCor { get; }

        /// <summary>
        /// Path to .targets file of the DllExport.
        /// </summary>
        string DxpTarget { get; }

        /// <summary>
        /// Arguments to manager.
        /// </summary>
        string MgrArgs { get; }

        /// <summary>
        /// Version of the package that invokes target.
        /// </summary>
        string PkgVer { get; }

        /// <summary>
        /// Indicates the status of the used package.
        /// </summary>
        bool Distributable { get; }

        /// <summary>
        /// The type of the current package, eg. offline, ...
        /// </summary>
        string PackageType { get; }

        /// <summary>
        /// Proxy configuration if presented in `-proxy` key.
        /// </summary>
        string Proxy { get; }

        /// <summary>
        /// Path to external storage if used.
        /// </summary>
        string StoragePath { get; }

        /// <summary>
        /// Where to store configuration data.
        /// </summary>
        CfgStorageType CfgStorage { get; set; }

        /// <summary>
        /// The evaluated type of operation.
        /// </summary>
        ActionType Type { get; }

        /// <summary>
        /// To show messages via GUI dlg for selected level (any positive number) and above.
        /// Levels: 0 - 5 (see Message.Level)
        /// '4' = means 4 (Error) + 5 (Fatal) levels.
        /// Any negative number disables this.
        /// It affects only for messages to GUI.
        /// </summary>
        int MsgGuiLevel { get; }
    }
}

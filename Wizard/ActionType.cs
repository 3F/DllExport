/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2021  Denis Kuzmin <x-3F@outlook.com> github/3F
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
    public enum ActionType: uint
    {
        Default,

        /// <summary>
        /// Process of configuration of available projects.
        /// Install and Remove operation will be defined by user at runtime.
        /// </summary>
        Configure,

        /// <summary>
        /// To update package reference for already configured projects.
        /// </summary>
        Update,

        /// <summary>
        /// To restore already configured environment.
        /// </summary>
        Restore,

        /// <summary>
        /// Information about obsolete nuget clients etc.
        /// </summary>
        Info,

        /// <summary>
        /// To re-configure projects via predefined/exported data.
        /// </summary>
        Recover,

        /// <summary>
        /// To export configured projects data.
        /// </summary>
        Export,

        /// <summary>
        /// To unset all data from specified projects.
        /// </summary>
        Unset,

        /// <summary>
        /// Aggregates an Update action with additions for upgrading.
        /// TODO: Currently is equal to the Update action.
        /// </summary>
        Upgrade = Update,

        /// <summary>
        /// To list projects and their statuses. As plain text data.
        /// TODO:
        /// </summary>
        //ListPlain
    }
}
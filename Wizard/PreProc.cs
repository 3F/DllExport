﻿/*
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
    public sealed class PreProc
    {
        /// <summary>
        /// Never null command.
        /// </summary>
        public string Cmd
        {
            get;
            private set;
        }

        public CmdType Type
        {
            get;
            private set;
        }

        [System.Flags]
        public enum CmdType: long
        {
            None        = 0x0,

            ILMerge     = 0x1,

            Conari      = 0x2,

            Exec        = 0x4,

            DebugInfo   = 0x8,

            IgnoreErr   = 0x10,
        }

        public PreProc Configure(CmdType type = CmdType.None, string cmd = null)
        {
            Type    = type;
            Cmd     = (type == CmdType.None || cmd == null) ? string.Empty : cmd;

            return this;
        }
    }
}

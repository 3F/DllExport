/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2019  Denis Kuzmin < entry.reg@gmail.com > GitHub/3F
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
using System.Collections;
using System.Collections.Generic;

namespace net.r_eg.DllExport.PeViewer
{
    internal sealed class ArgsHelper: IEnumerable<ArgsHelper>
    {
        private HashSet<int> accepted = new HashSet<int>();

        private string[] args;
        private volatile int idx;
        private object sync = new object();

        public IEnumerator<ArgsHelper> GetEnumerator()
        {
            return Iterate.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Iterate.GetEnumerator();
        }

        public IEnumerable<ArgsHelper> Iterate
        {
            get
            {
                lock(sync)
                {
                    for(idx = 0; idx < args.Length; ++idx) {
                        yield return this;
                    }
                }
            }
        }

        public bool IsEmpty
        {
            get => args.Length < 1;
        }

        /// <summary>
        /// Expected keys that was accepted for using.
        /// </summary>
        public IEnumerable<string> Used
        {
            get
            {
                foreach(var pos in accepted) {
                    yield return args[pos];
                }
            }
        }

        /// <summary>
        /// Unknown or still unacceptable keys from arguments.
        /// </summary>
        public IEnumerable<string> NonUsed
        {
            get
            {
                for(int i = 0; i < args.Length; ++i) {
                    if(!accepted.Contains(i)) {
                        yield return args[i];
                    }
                }
            }
        }

        public bool Is(string key)
        {
            return Is(key, false, out string nul);
        }

        public bool Is(string key, out string value)
        {
            return Is(key, true, out value);
        }

        public bool Is(string key, bool hasVal, out string value)
        {
            value = null;

            lock(sync)
            {
                if(String.IsNullOrWhiteSpace(key) || !Eq(args[idx], key)) {
                    return false;
                }

                accepted.Add(idx);

                if(hasVal && ++idx < args.Length) {
                    value = args[idx];
                    accepted.Add(idx);
                }
                return true;
            }
        }

        public ArgsHelper(string[] args)
        {
            this.args = args ?? throw new ArgumentNullException(nameof(args));
        }

        private bool Eq(string a, string b, StringComparison cmp = StringComparison.InvariantCultureIgnoreCase)
        {
            return a.Equals(b, cmp);
        }
    }
}

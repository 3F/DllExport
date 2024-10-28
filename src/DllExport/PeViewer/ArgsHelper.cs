/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
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

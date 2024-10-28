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
    /// <remarks>NOT thread safe</remarks>
    internal sealed class ArgsHelper(string[] args): IEnumerable<ArgsHelper>
    {
        private readonly HashSet<int> accepted = [];

        private readonly string[] args = args ?? throw new ArgumentNullException(nameof(args));
        private int idx;

        public IEnumerator<ArgsHelper> GetEnumerator() => Iterate.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Iterate.GetEnumerator();

        public IEnumerable<ArgsHelper> Iterate
        {
            get
            {
                for(idx = 0; idx < args.Length; ++idx) yield return this;
            }
        }

        public bool IsEmpty => args.Length < 1;

        /// <summary>
        /// Expected keys that was accepted for using.
        /// </summary>
        public IEnumerable<string> Used
        {
            get
            {
                foreach(int pos in accepted) yield return args[pos];
            }
        }

        /// <summary>
        /// Unknown or still unacceptable keys from arguments.
        /// </summary>
        public IEnumerable<string> NonUsed
        {
            get
            {
                for(int i = 0; i < args.Length; ++i)
                {
                    if(!accepted.Contains(i)) yield return args[i];
                }
            }
        }

        public bool Is(string key) => Is(key, false, out _);

        public bool Is(string key, out string value) => Is(key, true, out value);

        public bool Is(string key, bool hasVal, out string value)
        {
            value = null;

            if(string.IsNullOrWhiteSpace(key) || !Eq(args[idx], key))
            {
                return false;
            }

            accepted.Add(idx);

            if(hasVal && ++idx < args.Length)
            {
                value = args[idx];
                accepted.Add(idx);
            }
            return true;
        }

        private bool Eq(string a, string b, StringComparison cmp = StringComparison.InvariantCultureIgnoreCase)
        {
            return a.Equals(b, cmp);
        }
    }
}

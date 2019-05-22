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
using System.Collections.Generic;
using System.Linq;

namespace net.r_eg.DllExport.PeViewer
{
    internal sealed class ArgsMapper
    {
        private IDictionary<string, bool> commands;

        public ArgsHelper AHelper
        {
            get;
            private set;
        }

        public bool IsEmptyArgs
        {
            get => AHelper.IsEmpty;
        }

        public IEnumerable<string> CommandsPrintVersion
        {
            get
            {
                foreach(var cmd in commands)
                {
                    var ret = $"{cmd.Key}";
                    if(cmd.Value) {
                        ret += " {data}";
                    }
                    yield return ret;
                }
            }
        }

        /// <summary>
        /// Keys that was defined in map but was not found in arguments.
        /// </summary>
        public IEnumerable<string> NotFound
        {
            get => Map.Where(m => !m.Value.defined).Select(m => m.Key);
        }

        public Dictionary<string, ArgImp> Map
        {
            get;
            private set;
        } = new Dictionary<string, ArgImp>();

        internal struct ArgImp
        {
            public string name;
            public string value;
            public bool defined;
        }

        public ArgImp GetArg(string name)
        {
            if(String.IsNullOrWhiteSpace(name) || !Map.ContainsKey(name)) {
                return new ArgImp();
            }
            return Map[name];
        }

        /// <summary>
        /// Is defined in args collection ?
        /// </summary>
        /// <param name="argname"></param>
        /// <returns></returns>
        public bool Is(string argname)
        {
            return GetArg(argname).defined;
        }

        /// <summary>
        /// Is defined in args collection ?
        /// </summary>
        /// <param name="argname"></param>
        /// <param name="value">The data of this arg if present.</param>
        /// <returns></returns>
        public bool Is(string argname, out string value)
        {
            var ret = GetArg(argname);

            value = ret.value;
            return ret.defined;
        }

        public string GetValue(string argname)
        {
            return GetArg(argname).value;
        }

        public ArgsMapper(string[] args, IDictionary<string, bool> cmds)
        {
            commands    = cmds ?? throw new ArgumentNullException(nameof(cmds));
            AHelper     = new ArgsHelper(args);

            foreach(var cmd in commands)
            {
                var imp = new ArgImp {
                    name = cmd.Key
                };

                foreach(var arg in AHelper)
                {
                    if(!cmd.Value) {
                        imp.defined = arg.Is(imp.name);
                    }
                    else {
                        imp.defined = arg.Is(imp.name, out imp.value);
                    }

                    if(imp.defined) {
                        break;
                    }
                }

                if(Map.ContainsKey(imp.name)) {
                    throw new ArgumentException($"Duplicate arguments are not allowed: '{imp.name}'");
                }
                Map[imp.name] = imp;
            }
        }
    }
}

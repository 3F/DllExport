/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace net.r_eg.DllExport.PeViewer
{
    internal sealed class ArgsMapper
    {
        private readonly IDictionary<string, bool> commands;

        public ArgsHelper AHelper { get; private set; }

        public bool IsEmptyArgs => AHelper.IsEmpty;

        public IEnumerable<string> CommandsPrintVersion
        {
            get
            {
                foreach(var cmd in commands)
                {
                    string ret = $"{cmd.Key}";
                    if(cmd.Value) ret += " {...}";
                    yield return ret;
                }
            }
        }

        /// <summary>
        /// Keys that was defined in map but was not found in arguments.
        /// </summary>
        public IEnumerable<string> NotFound
            => Map.Where(m => !m.Value.defined).Select(m => m.Key);

        public Dictionary<string, ArgImp> Map { get; private set; } = [];

        internal struct ArgImp
        {
            public string name;
            public string value;
            public bool defined;
        }

        public ArgImp GetArg(string name)
        {
            if(string.IsNullOrWhiteSpace(name) || !Map.ContainsKey(name))
            {
                return default;
            }
            return Map[name];
        }

        /// <summary>
        /// Is defined in args collection ?
        /// </summary>
        /// <param name="argname"></param>
        /// <returns></returns>
        public bool Is(string argname) => GetArg(argname).defined;

        /// <summary>
        /// Is defined in args collection ?
        /// </summary>
        /// <param name="argname"></param>
        /// <param name="value">The data of this arg if present.</param>
        /// <returns></returns>
        public bool Is(string argname, out string value)
        {
            ArgImp ret = GetArg(argname);

            value = ret.value;
            return ret.defined;
        }

        public bool Find(out string value, params string[] argnames)
        {
            foreach(string arg in argnames)
            {
                ArgImp ret = GetArg(arg);
                if(ret.defined)
                {
                    value = ret.value;
                    return true;
                }
            }
            value = null;
            return false;
        }

        public string GetValue(string argname) => GetArg(argname).value;

        public ArgsMapper(string[] args, IDictionary<string, bool> cmds)
        {
            commands    = cmds ?? throw new ArgumentNullException(nameof(cmds));
            AHelper     = new ArgsHelper(args);

            foreach(var cmd in commands)
            {
                ArgImp imp = new() { name = cmd.Key };

                foreach(ArgsHelper arg in AHelper)
                {
                    if(!cmd.Value)
                    {
                        imp.defined = arg.Is(imp.name);
                    }
                    else
                    {
                        imp.defined = arg.Is(imp.name, out imp.value);
                    }

                    if(imp.defined) break;
                }

                if(Map.ContainsKey(imp.name))
                {
                    throw new ArgumentException($"Duplicate arguments are not allowed: '{imp.name}'");
                }
                Map[imp.name] = imp;
            }
        }
    }
}

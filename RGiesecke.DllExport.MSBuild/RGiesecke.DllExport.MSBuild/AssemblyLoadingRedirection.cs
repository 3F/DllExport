//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
//$ Distributed under the MIT License (MIT)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RGiesecke.DllExport.MSBuild
{
    internal static class AssemblyLoadingRedirection
    {
        public static readonly bool IsSetup;

        static AssemblyLoadingRedirection()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (ResolveEventHandler)((sender, args) => {
                AssemblyName assemblyName = new AssemblyName(args.Name);
                if(((IEnumerable<string>)new string[2]
            {
            "Mono.Cecil",
            "RGiesecke.DllExport"
            }).Contains<string>(assemblyName.Name))
                {
                    string str = Path.Combine(Path.GetDirectoryName(new Uri(typeof(AssemblyLoadingRedirection).Assembly.EscapedCodeBase).AbsolutePath), assemblyName.Name + ".dll");
                    if(File.Exists(str))
                    {
                        return Assembly.LoadFrom(str);
                    }
                }
                return (Assembly)null;
            });
            AssemblyLoadingRedirection.IsSetup = true;
        }

        public static void EnsureSetup()
        {
            if(!AssemblyLoadingRedirection.IsSetup)
            {
                throw new InvalidOperationException(string.Format(Resources.AssemblyRedirection_for_0_has_not_been_setup_, (object)typeof(AssemblyLoadingRedirection).FullName));
            }
        }
    }
}

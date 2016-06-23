// [Decompiled] Assembly: RGiesecke.DllExport.MSBuild, Version=1.2.1.28778, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

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
                if(((IEnumerable<string>)new string[2] { "Mono.Cecil", "RGiesecke.DllExport" }).Contains<string>(assemblyName.Name))
                {
                    string str = Path.Combine(Path.GetDirectoryName(new Uri(typeof(ExportTaskImplementation<>).Assembly.EscapedCodeBase).AbsolutePath), assemblyName.Name + ".dll");
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
                throw new InvalidOperationException(string.Format("{0} has not been setup.", (object)typeof(AssemblyLoadingRedirection).FullName));
            }
        }
    }
}

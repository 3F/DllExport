/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

#if FEATURE_ACTIVATOR_ISOLATED_TASK
using System;
using System.IO;
using System.Reflection;
#endif

namespace net.r_eg.DllExport.Activator
{
    internal static class AssemblyLoadingRedirection
    {
#if FEATURE_ACTIVATOR_ISOLATED_TASK
        public static readonly bool IsSetup;

        static AssemblyLoadingRedirection()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string req = Path.Combine
                (
                    Path.GetDirectoryName(typeof(AssemblyLoadingRedirection).Assembly.Location),
                    $"{new AssemblyName(args.Name).Name}.dll"
                );

                if(File.Exists(req)) return Assembly.LoadFrom(req);
                return null;
            };

            IsSetup = true;
        }
#endif

        public static void EnsureSetup()
        {
#if FEATURE_ACTIVATOR_ISOLATED_TASK
            if(!IsSetup)
            {
                throw new InvalidOperationException(string.Format(Resources.AssemblyRedirection_for_0_has_not_been_setup_, typeof(AssemblyLoadingRedirection).FullName));
            }
#endif
        }
    }
}

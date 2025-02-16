/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;

namespace net.r_eg.DllExport
{
    public static class Utilities
    {
        public static readonly string DllExportAttributeAssemblyName    = "DllExport";
        public static readonly string DllExportAttributeFullName        = "System.Runtime.InteropServices.DllExportAttribute";

        public static MethodInfo GetMethodInfo<TResult>(Expression<Func<TResult>> expression)
        {
            return ((MethodCallExpression)expression.Body).Method;
        }

        internal static IExportAssemblyInspector CreateAssemblyInspector(IInputValues input)
        {
            return new ExportAssemblyInspector(input);
        }

        public static int GetCoreFlagsForPlatform(CpuPlatform cpu)
        {
            return cpu != CpuPlatform.X86 ? 0 : 2;
        }

        public static CpuPlatform ToCpuPlatform(string platformTarget)
        {
            if(!string.IsNullOrEmpty(platformTarget))
            {
                switch(platformTarget.ToLowerInvariant())
                {
                    case "anycpu":
                    case "any cpu":
                    return CpuPlatform.AnyCpu;

                    case "x86":
                    case "x32":
                    case "win32":
                    return CpuPlatform.X86;

                    case "x64":
                    return CpuPlatform.X64;

                    case "ia64":
                    return CpuPlatform.Itanium;
                }
            }
            throw new ArgumentException(string.Format(Resources.Unknown_cpu_platform_0_, platformTarget), nameof(platformTarget));
        }

        public static T TryInitialize<T>(this T instance, Action<T> call) where T : IDisposable
        {
            try
            {
                call(instance);
                return instance;
            }
            catch
            {
                instance.Dispose();
                throw;
            }
        }

        internal sealed class TempDir: IDisposable
        {
            public string FullPath { get; }

            public TempDir()
            {
                string path = Path.GetTempPath();
                string name = Guid.NewGuid().ToString();

                FullPath = Path.Combine(path, $"dxp-{name}");
                Directory.CreateDirectory(FullPath);
            }

            #region IDisposable

            private bool disposed;

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool _)
            {
                if(!disposed)
                {
                    if(Directory.Exists(FullPath))
                        Directory.Delete(FullPath, true);

                    disposed = true;
                }
            }

            #endregion
        }
    }
}

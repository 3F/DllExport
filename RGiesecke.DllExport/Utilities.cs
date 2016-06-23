// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.3.29766, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using RGiesecke.DllExport.Properties;

namespace RGiesecke.DllExport
{
    public static class Utilities
    {
        public static readonly string DllExportAttributeAssemblyName = "RGiesecke.DllExport.Metadata";
        public static readonly string DllExportAttributeFullName = "RGiesecke.DllExport.DllExportAttribute";

        public static MethodInfo GetMethodInfo<TResult>(Expression<Func<TResult>> expression)
        {
            return ((MethodCallExpression)expression.Body).Method;
        }

        public static string GetSdkPath(Version frameworkVersion)
        {
            using(RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework", false))
            {
                if(registryKey == null)
                {
                    return (string)null;
                }
                string @string = registryKey.GetValue("sdkInstallRootv" + frameworkVersion.ToString(2), (object)"").NullSafeToString();
                if(!string.IsNullOrEmpty(@string))
                {
                    return @string;
                }
                return (string)null;
            }
        }

        public static string GetSdkPath()
        {
            return Utilities.GetSdkPath(new Version(RuntimeEnvironment.GetSystemVersion().NullSafeTrimStart('v')));
        }

        internal static IExportAssemblyInspector CreateAssemblyInspector(IInputValues inputValues)
        {
            return (IExportAssemblyInspector)new ExportAssemblyInspector(inputValues);
        }

        public static int GetCoreFlagsForPlatform(CpuPlatform cpu)
        {
            return cpu != CpuPlatform.X86 ? 0 : 2;
        }

        public static CpuPlatform ToCpuPlatform(string platformTarget)
        {
            if(!string.IsNullOrEmpty(platformTarget))
            {
                switch(platformTarget.NullSafeToLowerInvariant())
                {
                    case "anycpu":
                    return CpuPlatform.AnyCpu;

                    case "x86":
                    return CpuPlatform.X86;

                    case "x64":
                    return CpuPlatform.X64;

                    case "ia64":
                    return CpuPlatform.Itanium;
                }
            }
            throw new ArgumentException(string.Format(Resources.Unknown_cpu_platform_0_, (object)platformTarget), "platformTarget");
        }

        public static T TryInitialize<T>(this T instance, Action<T> call) where T : IDisposable
        {
            try
            {
                call(instance);
                return instance;
            }
            catch(Exception ex)
            {
                instance.Dispose();
                throw;
            }
        }

        public static ValueDisposable<string> CreateTempDirectory()
        {
            return new ValueDisposable<string>(Utilities.CreateTempDirectoryCore(), (Action<string>)(dir => Directory.Delete(dir, true)));
        }

        private static string CreateTempDirectoryCore()
        {
            string path1 = (string)null;
            try
            {
                string tempFileName = Path.GetTempFileName();
                if(!string.IsNullOrEmpty(tempFileName) && File.Exists(tempFileName))
                {
                    File.Delete(tempFileName);
                }
                string path2 = Path.Combine(Path.GetFullPath(Path.GetDirectoryName(tempFileName)), Path.GetFileNameWithoutExtension(tempFileName));
                Directory.CreateDirectory(path2);
                return path2;
            }
            catch
            {
                if(!string.IsNullOrEmpty(path1) && Directory.Exists(path1))
                {
                    Directory.Delete(path1, true);
                }
                throw;
            }
        }
    }
}

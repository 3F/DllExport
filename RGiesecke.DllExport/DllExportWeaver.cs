// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.7.38850, Culture=neutral, PublicKeyToken=8f52d83c1a22df51
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using RGiesecke.DllExport.Parsing;
using RGiesecke.DllExport.Properties;

namespace RGiesecke.DllExport
{
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public sealed class DllExportWeaver: HasServiceProvider
    {
        private int _Timeout = 45000;

        public int Timeout
        {
            get {
                return this._Timeout;
            }

            set {
                this._Timeout = value;
            }
        }

        internal AssemblyExports Exports
        {
            get;
            set;
        }

        public IInputValues InputValues
        {
            get;
            set;
        }

        public DllExportWeaver(IServiceProvider serviceProvider)
        : base(serviceProvider)
        {
        }

        public void Run()
        {
            if(this.Exports == null)
            {
                IExportAssemblyInspector assemblyInspector = Utilities.CreateAssemblyInspector(this.InputValues);
                using(this.GetNotifier().CreateContextName((object)this, Resources.ExtractExportsContextName))
                    this.Exports = assemblyInspector.ExtractExports();
                using(this.GetNotifier().CreateContextName((object)this, Resources.FindDuplicateExportMethodsContextName))
                {
                    foreach(DuplicateExports duplicateExportMethod in this.Exports.DuplicateExportMethods)
                    {
                        if(duplicateExportMethod.Duplicates.Count > 0)
                        {
                            StringBuilder stringBuilder = new StringBuilder(200).AppendFormat("{0}.{1}", (object)duplicateExportMethod.UsedExport.ExportedClass.NullSafeCall<ExportedClass, string>((Func<ExportedClass, string>)(ec => ec.FullTypeName)), (object)duplicateExportMethod.UsedExport.Name);
                            foreach(ExportedMethod duplicate in (IEnumerable<ExportedMethod>)duplicateExportMethod.Duplicates)
                            {
                                stringBuilder.AppendFormat(", {0}.{1}", (object)duplicate.ExportedClass.NullSafeCall<ExportedClass, string>((Func<ExportedClass, string>)(ec => ec.FullTypeName)), (object)duplicate.Name);
                            }
                        }
                    }
                }
            }
            if(this.Exports.Count == 0)
            {
                return;
            }
            using(this.GetNotifier().CreateContextName((object)this, Resources.CreateTempDirectoryContextName))
            {
                using(ValueDisposable<string> tempDirectory = Utilities.CreateTempDirectory())
                {
                    this.RunIlDasm(tempDirectory.Value);
                    bool flag = ((IEnumerable<string>)new string[2] { "true", "yes" }).Any<string>((Func<string, bool>)(t => t.Equals(this.InputValues.LeaveIntermediateFiles, StringComparison.InvariantCultureIgnoreCase)));
                    if(flag)
                    {
                        using(this.GetNotifier().CreateContextName((object)this, Resources.CopyBeforeContextName))
                            DllExportWeaver.CopyDirectory(tempDirectory.Value, Path.Combine(Path.GetDirectoryName(this.InputValues.OutputFileName), "Before"), true);
                    }
                    using(IlAsm ilAsm = this.PrepareIlAsm(tempDirectory.Value))
                        this.RunIlAsm(ilAsm);
                    if(!flag)
                    {
                        return;
                    }
                    using(this.GetNotifier().CreateContextName((object)this, Resources.CopyAfterContextName))
                        DllExportWeaver.CopyDirectory(tempDirectory.Value, Path.Combine(Path.GetDirectoryName(this.InputValues.OutputFileName), "After"), true);
                }
            }
        }

        private IDllExportNotifier GetNotifier()
        {
            return this.ServiceProvider.GetService<IDllExportNotifier>();
        }

        private static string GetCleanedDirectoryPath(string path)
        {
            if(path == null)
            {
                throw new ArgumentNullException("path");
            }
            DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetFullPath(path));
            if(directoryInfo.Parent == null)
            {
                return directoryInfo.FullName;
            }
            return Path.Combine(directoryInfo.Parent.FullName, directoryInfo.Name);
        }

        private static void CopyDirectory(string sourceDirectory, string destinationDirectory, bool overwrite = false)
        {
            sourceDirectory = DllExportWeaver.GetCleanedDirectoryPath(sourceDirectory);
            destinationDirectory = DllExportWeaver.GetCleanedDirectoryPath(destinationDirectory);
            if(Directory.Exists(destinationDirectory) && !overwrite)
            {
                throw new IOException(Resources.The_destination_directory_already_exists_);
            }
            if(!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }
            int startIndex = sourceDirectory.Length + 1;
            foreach(string directory in Directory.GetDirectories(sourceDirectory, "*", SearchOption.AllDirectories))
            {
                string path = Path.Combine(destinationDirectory, directory.Substring(startIndex));
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            foreach(string file in Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories))
            {
                string destFileName = Path.Combine(destinationDirectory, file.Substring(startIndex));
                File.Copy(file, destFileName, overwrite);
            }
        }

        private void RunIlAsm(IlAsm ilAsm)
        {
            using(this.GetNotifier().CreateContextName((object)this, "RunIlAsm"))
            {
                if(this.InputValues.Cpu == CpuPlatform.AnyCpu)
                {
                    string str = Path.GetDirectoryName(this.InputValues.OutputFileName) ?? "";
                    string fileName = Path.GetFileName(this.InputValues.OutputFileName);
                    if(!Directory.Exists(str))
                    {
                        throw new DirectoryNotFoundException(string.Format(Resources.Directory_0_does_not_exist, (object)str));
                    }
                    this.GetNotifier().Notify(1, DllExportLogginCodes.CreatingBinariesForEachPlatform, Resources.Platform_is_0_creating_binaries_for_each_CPU_platform_in_a_separate_subfolder, (object)this.InputValues.Cpu);
                    ilAsm.ReassembleFile(Path.Combine(Path.Combine(str, "x86"), fileName), ".x86", CpuPlatform.X86);
                    ilAsm.ReassembleFile(Path.Combine(Path.Combine(str, "x64"), fileName), ".x64", CpuPlatform.X64);
                }
                else
                {
                    ilAsm.ReassembleFile(this.InputValues.OutputFileName, "", this.InputValues.Cpu);
                }
            }
        }

        private IlAsm PrepareIlAsm(string tempDirectory)
        {
            using(this.GetNotifier().CreateContextName((object)this, "PrepareIlAsm"))
            {
                IlAsm instance = new IlAsm((IServiceProvider)this, this.InputValues);
                instance.Timeout = this.Timeout;
                return instance.TryInitialize<IlAsm>((Action<IlAsm>)(ilAsm => {
                    ilAsm.TempDirectory = tempDirectory;
                    ilAsm.Exports = this.Exports;
                }));
            }
        }

        private void RunIlDasm(string tempDirectory)
        {
            using(this.GetNotifier().CreateContextName((object)this, "RunIlDasm"))
            {
                IlDasm ilDasm1 = new IlDasm((IServiceProvider)this, this.InputValues);
                ilDasm1.Timeout = this.Timeout;
                using(IlDasm ilDasm2 = ilDasm1)
                {
                    ilDasm2.TempDirectory = tempDirectory;
                    ilDasm2.Run();
                }
            }
        }
    }
}

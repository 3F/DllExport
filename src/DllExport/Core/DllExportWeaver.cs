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
using System.Security.Permissions;
using System.Text;
using net.r_eg.DllExport.Extensions;
using net.r_eg.DllExport.Parsing;
using TempDir = net.r_eg.DllExport.Utilities.TempDir;

namespace net.r_eg.DllExport
{
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public sealed class DllExportWeaver(IServiceProvider serviceProvider): HasServiceProvider(serviceProvider)
    {
        public int Timeout { get; set; } = 45_000;

        internal AssemblyExports Exports { get; set; }

        public IInputValues InputValues { get; set; }

        public void Run()
        {
            if(Exports == null)
            {
                IExportAssemblyInspector assemblyInspector = Utilities.CreateAssemblyInspector(InputValues);

                using(GetNotifier().CreateContextName(this, Resources.ExtractExportsContextName))
                {
                    Exports = assemblyInspector.ExtractExports();
                }

                using(GetNotifier().CreateContextName(this, Resources.FindDuplicateExportMethodsContextName))
                {
                    foreach(DuplicateExports duplicateExportMethod in Exports.DuplicateExportMethods)
                    {
                        if(duplicateExportMethod.Duplicates.Count < 1) continue;

                        ExportedMethod em = duplicateExportMethod.UsedExport;

                        StringBuilder sb = new(200);
                        sb.AppendFormat("{0}.{1}", em.ExportedClass?.FullTypeName, em.Name);
                        foreach(ExportedMethod duplicate in (IEnumerable<ExportedMethod>)duplicateExportMethod.Duplicates)
                        {
                            sb.AppendFormat(", {0}.{1}", duplicate.ExportedClass?.FullTypeName, duplicate.Name);
                        }
                    }
                }
            }

            if(Exports.Count == 0) return;

            using(GetNotifier().CreateContextName(this, Resources.CreateTempDirectoryContextName))
            using(TempDir dir = new())
            {
                RunIlDasm(dir.FullPath);

                void _ilasm() => RunIlAsm(dir.FullPath);

                if(InputValues.LeaveIntermediateFiles.IsTrue())
                {
                    MakeWithIntermediateFiles(dir, _ilasm);
                    return;
                }

                _ilasm();
            }
        }

        private void MakeWithIntermediateFiles(TempDir dir, Action ilasm)
        {
            const string _PRE   = "Before";
            const string _POST  = "After";

            string fout = Path.GetDirectoryName(InputValues.OutputFileName);

            using(GetNotifier().CreateContextName(this, Resources.CopyBeforeContextName))
                CopyDirectory(dir.FullPath, Path.Combine(fout, _PRE), true);

            ilasm?.Invoke();

            using(GetNotifier().CreateContextName(this, Resources.CopyAfterContextName))
                CopyDirectory(dir.FullPath, Path.Combine(fout, _POST), true);
        }

        private IDllExportNotifier GetNotifier()
            => (IDllExportNotifier)ServiceProvider.GetService(typeof(IDllExportNotifier));

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

        private void RunIlAsm(string tempDir)
        {
            using(IlAsm ilAsm = PrepareIlAsm(tempDir ?? throw new ArgumentNullException(nameof(tempDir))))
            {
                RunIlAsm(ilAsm);
            }
        }

        private IlAsm RunIlAsm(IlAsm ilAsm)
        {
            using(GetNotifier().CreateContextName(this, nameof(RunIlAsm)))
            {
                if(InputValues.Cpu != CpuPlatform.AnyCpu) {
                    ReassembleFile(ilAsm, InputValues.OutputFileName, string.Empty, InputValues.Cpu);
                    return ilAsm;
                }

                string dir      = Path.GetDirectoryName(InputValues.OutputFileName) ?? string.Empty;
                string fileName = Path.GetFileName(InputValues.OutputFileName);

                if(!Directory.Exists(dir)) {
                    throw new DirectoryNotFoundException(String.Format(Resources.Directory_0_does_not_exist, dir));
                }

                ReassembleFile(ilAsm, Path.Combine(Path.Combine(dir, "x86"), fileName), ".x86", CpuPlatform.X86);
                ReassembleFile(ilAsm, Path.Combine(Path.Combine(dir, "x64"), fileName), ".x64", CpuPlatform.X64);
            }

            return ilAsm;
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

        private int ReassembleFile(IlAsm ilAsm, string outputFile, string ilSuffix, CpuPlatform cpu)
        {
            try
            {
                return ilAsm.ReassembleFile(outputFile, ilSuffix, cpu);
            }
            finally
            {
                Cleanup(ilAsm);
            }
        }

        private void Cleanup(IlAsm ilAsm)
        {
            foreach(var _class in ilAsm.Exports.ClassesByName.Values)
            {
                _class.ResetExportedMethods();
            }
        }
    }
}

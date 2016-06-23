// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.2.23706, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Text;
using RGiesecke.DllExport.Parsing;
using RGiesecke.DllExport.Properties;

namespace RGiesecke.DllExport
{
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public sealed class DllExportWeaver: IDllExportNotifier, IDisposable
    {
        private readonly DllExportNotifier _Notifier = new DllExportNotifier();
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

        public event EventHandler<DllExportNotificationEventArgs> Notification
        {
            add {
                this._Notifier.Notification += value;
            }
            remove {
                this._Notifier.Notification -= value;
            }
        }

        public DllExportWeaver()
        {
            this._Notifier.Context = (object)this;
        }

        public void Dispose()
        {
            this._Notifier.Dispose();
        }

        private void DllExportIDllExportNotifier(object sender, DllExportNotificationEventArgs e)
        {
            string str = e.Context != null ? e.Context.GetType().Name : (string)null;
            if(string.IsNullOrEmpty(str))
            {
                str = sender.GetType().Name;
            }
            e.Message = str + ": " + e.Message;
            this._Notifier.Notify(e);
        }

        public void Run()
        {
            if(this.Exports == null)
            {
                this.Exports = Utilities.CreateAssemblyInspector(this.InputValues).ExtractExports();
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
            if(this.Exports.Count == 0)
            {
                return;
            }
            using(ValueDisposable<string> tempDirectory = Utilities.CreateTempDirectory())
            {
                Console.WriteLine(tempDirectory.Value);
                this.RunIlDasm(tempDirectory.Value);
                using(IlAsm ilAsm = this.PrepareIlAsm(tempDirectory.Value))
                    this.RunIlAsm(ilAsm);
            }
        }

        private void RunIlAsm(IlAsm ilAsm)
        {
            if(this.InputValues.Cpu == CpuPlatform.AnyCpu)
            {
                string str = Path.GetDirectoryName(this.InputValues.OutputFileName) ?? "";
                string fileName = Path.GetFileName(this.InputValues.OutputFileName);
                if(!Directory.Exists(str))
                {
                    throw new DirectoryNotFoundException(string.Format(Resources.Directory_0_does_not_exist, (object)str));
                }
                this._Notifier.Notify(1, "EXP0009", Resources.Platform_is_0_creating_binaries_for_each_CPU_platform_in_a_separate_subfolder, (object)this.InputValues.Cpu);
                ilAsm.ReassembleFile(Path.Combine(Path.Combine(str, "x86"), fileName), ".x86", CpuPlatform.X86);
                ilAsm.ReassembleFile(Path.Combine(Path.Combine(str, "x64"), fileName), ".x64", CpuPlatform.X64);
            }
            else
            {
                ilAsm.ReassembleFile(this.InputValues.OutputFileName, "", this.InputValues.Cpu);
            }
        }

        private IlAsm PrepareIlAsm(string tempDirectory)
        {
            return new IlAsm() {
                Timeout = this.Timeout
            }.TryInitialize<IlAsm>((Action<IlAsm>)(ilAsm => {
                ilAsm.Notification += new EventHandler<DllExportNotificationEventArgs>(this.DllExportIDllExportNotifier);
                ilAsm.InputValues = this.InputValues;
                ilAsm.TempDirectory = tempDirectory;
                ilAsm.Exports = this.Exports;
            }));
        }

        private void RunIlDasm(string tempDirectory)
        {
            using(IlDasm ilDasm = new IlDasm() {
                Timeout = this.Timeout
            })
            {
                ilDasm.Notification += new EventHandler<DllExportNotificationEventArgs>(this.DllExportIDllExportNotifier);
                ilDasm.TempDirectory = tempDirectory;
                ilDasm.InputValues = this.InputValues;
                ilDasm.Run();
            }
        }
    }
}

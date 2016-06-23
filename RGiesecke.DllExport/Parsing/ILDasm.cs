// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.3.29766, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using System.Globalization;
using System.IO;
using System.Security.Permissions;

namespace RGiesecke.DllExport.Parsing
{
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public sealed class IlDasm: IDllExportNotifier, IDisposable
    {
        private readonly DllExportNotifier _Notifier;

        public int Timeout
        {
            get;
            set;
        }

        public IInputValues InputValues
        {
            get;
            set;
        }

        public string TempDirectory
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

        public IlDasm()
        {
            this._Notifier = new DllExportNotifier() {
                Context = (object)this
            };
        }

        public void Notify(int severity, string code, string message, params object[] values)
        {
            this._Notifier.Notify(severity, code, message, values);
        }

        public void Notify(int severity, string code, string fileName, SourceCodePosition? startPosition, SourceCodePosition? endPosition, string message, params object[] values)
        {
            this._Notifier.Notify(severity, code, fileName, startPosition, endPosition, message, values);
        }

        public void Dispose()
        {
            this._Notifier.Dispose();
        }

        public int Run()
        {
            return IlParser.RunIlTool(this.InputValues.SdkPath, "ildasm.exe", (string)null, (string)null, "ILDasmPath", string.Format((IFormatProvider)CultureInfo.InvariantCulture, "/quoteallnames /unicode /nobar{2}\"/out:{0}.il\" \"{1}\"", (object)Path.Combine(this.TempDirectory, this.InputValues.FileName), (object)this.InputValues.InputFileName, this.InputValues.EmitDebugSymbols ? (object)" /linenum " : (object)" "), DllExportLogginCodes.IlDasmLogging, DllExportLogginCodes.VerboseToolLogging, this._Notifier, this.Timeout);
        }
    }
}

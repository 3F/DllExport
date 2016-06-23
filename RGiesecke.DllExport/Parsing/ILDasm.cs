// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.2.23706, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
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

        public void Dispose()
        {
            this._Notifier.Dispose();
        }

        public int Run()
        {
            return IlParser.RunIlTool(this.InputValues.SdkPath, "ildasm.exe", (string)null, (string)null, "ILDasmPath", string.Format((IFormatProvider)CultureInfo.InvariantCulture, "/quoteallnames /unicode /nobar{2}\"/out:{0}.il\" \"{1}\"", (object)Path.Combine(this.TempDirectory, this.InputValues.FileName), (object)this.InputValues.InputFileName, this.InputValues.EmitDebugSymbols ? (object)" /linenum " : (object)" "), "EXP0007", "EXP0006", this._Notifier, this.Timeout);
        }
    }
}

// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.4.23262, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using System.Globalization;
using System.IO;
using System.Security.Permissions;

namespace RGiesecke.DllExport.Parsing
{
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public sealed class IlDasm: IlToolBase
    {
        public IlDasm(IInputValues inputValues)
        : base(inputValues)
        {
        }

        public int Run()
        {
            return IlParser.RunIlTool(this.InputValues.SdkPath, "ildasm.exe", (string)null, (string)null, "ILDasmPath", string.Format((IFormatProvider)CultureInfo.InvariantCulture, "/quoteallnames /unicode /nobar{2}\"/out:{0}.il\" \"{1}\"", (object)Path.Combine(this.TempDirectory, this.InputValues.FileName), (object)this.InputValues.InputFileName, this.InputValues.EmitDebugSymbols ? (object)" /linenum " : (object)" "), DllExportLogginCodes.IlDasmLogging, DllExportLogginCodes.VerboseToolLogging, this.Notifier, this.Timeout, (Func<string, bool>)null);
        }
    }
}

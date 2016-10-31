// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.7.38850, Culture=neutral, PublicKeyToken=8f52d83c1a22df51
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
        public IlDasm(IServiceProvider serviceProvider, IInputValues inputValues)
        : base(serviceProvider, inputValues)
        {
        }

        public int Run()
        {
            return IlParser.RunIlTool
            (
                String.IsNullOrWhiteSpace(InputValues.OurILAsmPath) ? InputValues.SdkPath : InputValues.OurILAsmPath,
                "ildasm.exe", 
                null, 
                null, 
                "ILDasmPath", 
                String.Format(
                    CultureInfo.InvariantCulture, 
                    "/quoteallnames /unicode /nobar{2}\"/out:{0}.il\" \"{1}\"", 
                    Path.Combine(TempDirectory, InputValues.FileName), 
                    InputValues.InputFileName, 
                    InputValues.EmitDebugSymbols ? " /linenum " : " "
                ), 
                DllExportLogginCodes.IlDasmLogging, 
                DllExportLogginCodes.VerboseToolLogging, 
                Notifier,
                Timeout, 
                null
             );
        }
    }
}

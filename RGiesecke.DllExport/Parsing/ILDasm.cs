//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
//$ Distributed under the MIT License (MIT)

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

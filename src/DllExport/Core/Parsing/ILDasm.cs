/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Globalization;
using System.IO;
using System.Security.Permissions;

namespace net.r_eg.DllExport.Parsing
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

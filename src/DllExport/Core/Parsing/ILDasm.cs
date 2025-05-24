/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Security.Permissions;

namespace net.r_eg.DllExport.Parsing
{
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public sealed class IlDasm(IServiceProvider provider, IInputValues inputValues): IlToolBase(provider, inputValues)
    {
        public int Run() => IlParser.RunIlTool
        (
            InputValues.IsILAsmDefault ? InputValues.SdkPath : InputValues.OurILAsmPath,
            "ildasm.exe",
            requiredPaths: null,
            workingDirectory: null,
            string.Format
            (
                CultureInfo.InvariantCulture, 
                "/quoteallnames /unicode /nobar{2}\"/out:{0}.il\" \"{1}\"", 
                Path.Combine(TempDirectory, InputValues.FileName), 
                InputValues.InputFileName,
                GetKeysToDebug(InputValues.EmitDebugSymbols)
            ), 
            DllExportLogginCodes.IlDasmLogging, 
            DllExportLogginCodes.VerboseToolLogging, 
            Notifier,
            Timeout
        );

        [Localizable(false)]
        private string GetKeysToDebug(DebugType type)
        {
            if(type.HasFlag(DebugType.Debug)) return " /linenum ";
            return " ";
        }
    }
}

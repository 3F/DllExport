/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using net.r_eg.DllExport.ILAsm;

namespace net.r_eg.DllExport.Parsing.Actions
{
    public sealed class NormalParserAction(IInputValues input): ParserStateAction
    {
        private readonly IInputValues inputValues = input;

        public override void Execute(ParserStateValues state, string trimmedLine)
        {
            if(trimmedLine.StartsWith(".corflags", StringComparison.Ordinal))
            {
                state.Result.Add(string.Format
                (
                    CultureInfo.InvariantCulture,
                    ".corflags 0x{0}",
                    Utilities.GetCoreFlagsForPlatform(state.Cpu).ToString("X8", CultureInfo.InvariantCulture)
                ));
                state.AddLine = false;
            }
            else if(trimmedLine.StartsWith(".imagebase ", StringComparison.Ordinal))
            {
                if(inputValues.ImageBase == -1) state.AddLine = true;
                else
                {
                    state.Result.Add(".imagebase 0x" + inputValues.ImageBase.ToString("X8"));
                    state.AddLine = false;
                }
            }
            else if(trimmedLine.StartsWith(".class extern forwarder ", StringComparison.Ordinal))
            {
                state.State = ParserState.ClassExternForwarder;
                state.AddLine = true;
                state.ClassDeclaration = trimmedLine;
            }
            else if(trimmedLine.StartsWith(".class extern ", StringComparison.Ordinal))
            {
                state.State = ParserState.ClassExtern;
                state.AddLine = true;
                state.ClassDeclaration = trimmedLine;
            }
            else if(trimmedLine.StartsWith(".class", StringComparison.Ordinal))
            {
                state.State = ParserState.ClassDeclaration;
                state.AddLine = true;
                state.ClassDeclaration = trimmedLine;
            }
            else if(trimmedLine.StartsWith(".assembly '"))
            {
                EmitCustomExternalAssemlies(state);
                //
                state.State = ParserState.AssemblyDeclaration;
                state.AddLine = true;
            }
            else if(IsExternalAssemblyReference(trimmedLine, out string assemblyName, out string aliasName))
            {
                state.RegisterExternalAssemlyAlias(assemblyName, aliasName);
                state.State = ParserState.AssemblyExtern;
            }
        }

        private void EmitCustomExternalAssemlies(ParserStateValues state)
        {
            if(inputValues?.AssemblyExternDirectives?.Count > 0)
            {
                foreach(AssemblyExternDirective decl in inputValues.AssemblyExternDirectives)
                {
                    state.Result.AddRange(decl.Format());
                }
            }
        }

        private bool IsExternalAssemblyReference(string trimmedLine, out string assemblyName, out string aliasName)
        {
            const string _ASM_EXT_D = ".assembly extern ";
            assemblyName = aliasName = null;

            if(trimmedLine.Length < _ASM_EXT_D.Length || !trimmedLine.StartsWith(_ASM_EXT_D, StringComparison.Ordinal))
            {
                return false;
            }

            List<string> identifiers = [];
            IlParsingUtils.ParseIlSnippet
            (
                trimmedLine.Substring(_ASM_EXT_D.Length),
                ParsingDirection.Forward,
                current =>
                {
                    if(!current.WithinString && current.CurrentChar == '\'' && current.LastIdentifier != null)
                    {
                        identifiers.Add(current.LastIdentifier);
                        if(identifiers.Count > 1) return false;
                    }
                    return true;
                },
                finalization: null
            );

            if(identifiers.Count < 1) return false;
            
            assemblyName = identifiers[0];
            aliasName = identifiers.Count > 1 ? identifiers[1] : identifiers[0];
            return true;
        }
    }
}

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

namespace RGiesecke.DllExport.Parsing.Actions
{
    [ParserStateAction(ParserState.Normal)]
    public sealed class NormalParserAction: IlParser.ParserStateAction
    {
        public override void Execute(ParserStateValues state, string trimmedLine)
        {
            if(trimmedLine.StartsWith(".corflags", StringComparison.Ordinal))
            {
                state.Result.Add(string.Format((IFormatProvider)CultureInfo.InvariantCulture, ".corflags 0x{0}", new object[1]
                {
                (object) Utilities.GetCoreFlagsForPlatform(state.Cpu).ToString("X8", (IFormatProvider) CultureInfo.InvariantCulture)
                }));
                state.AddLine = false;
            }
            else if(trimmedLine.StartsWith(".class", StringComparison.Ordinal))
            {
                state.State = ParserState.ClassDeclaration;
                state.AddLine = true;
                state.ClassDeclaration = trimmedLine;
            }
            else
            {
                string assemblyName;
                string aliasName;
                if(!this.IsExternalAssemblyReference(trimmedLine, out assemblyName, out aliasName))
                {
                    return;
                }
                state.RegisterMsCorelibAlias(assemblyName, aliasName);
            }
        }

        private bool IsExportAttributeAssemblyReference(string trimmedLine)
        {
            return trimmedLine.StartsWith(".assembly extern '" + this.DllExportAttributeAssemblyName + "'", StringComparison.Ordinal);
        }

        private bool IsExternalAssemblyReference(string trimmedLine, out string assemblyName, out string aliasName)
        {
            assemblyName = (string)null;
            aliasName = (string)null;
            if(trimmedLine.Length < ".assembly extern ".Length || !trimmedLine.StartsWith(".assembly extern ", StringComparison.Ordinal))
            {
                return false;
            }
            List<string> identifiers = new List<string>();
            IlParsingUtils.ParseIlSnippet(trimmedLine.Substring(".assembly extern ".Length), ParsingDirection.Forward, (Func<IlParsingUtils.IlSnippetLocation, bool>)(current => {
                if(!current.WithinString && (int)current.CurrentChar == 39 && current.LastIdentifier != null)
                {
                    identifiers.Add(current.LastIdentifier);
                    if(identifiers.Count > 1)
                    {
                        return false;
                    }
                }
                return true;
            }), (Action<IlParsingUtils.IlSnippetFinalizaton>)null);
            if(identifiers.Count == 0)
            {
                return false;
            }
            if(identifiers.Count > 0)
            {
                assemblyName = identifiers[0];
            }
            aliasName = identifiers.Count > 1 ? identifiers[1] : identifiers[0];
            return true;
        }
    }
}

/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;

namespace net.r_eg.DllExport.Parsing.Actions
{
    public sealed class MethodPropertiesParserAction: IlParser.ParserStateAction
    {
        public override void Execute(ParserStateValues state, string trimmedLine)
        {
            if(trimmedLine.StartsWith(".custom instance void ", StringComparison.Ordinal) && trimmedLine.Contains(this.Parser.DllExportAttributeIlAsmFullName))
            {
                state.AddLine = false;
                state.State = ParserState.DeleteExportAttribute;
                this.Notifier.Notify(-2, DllExportLogginCodes.RemovingDllExportAttribute, Resources.Removing_0_from_1_, (object)Utilities.DllExportAttributeFullName, (object)(state.ClassNames.Peek() + "." + state.Method.Name));
            }
            else
            {
                if(!trimmedLine.StartsWith(".maxstack", StringComparison.InvariantCulture)) {
                    return;
                }

                state.State = ParserState.Method;
                if(state.MethodPos == 0)
                {
                    return;
                }
                state.Result.Insert(state.MethodPos, state.Method.Declaration);
            }
        }
    }
}

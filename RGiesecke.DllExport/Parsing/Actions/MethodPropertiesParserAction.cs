//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2019  Denis Kuzmin < entry.reg@gmail.com > GitHub/3F
//$ Distributed under the MIT License (MIT)

using System;
using RGiesecke.DllExport.Properties;

namespace RGiesecke.DllExport.Parsing.Actions
{
    [ParserStateAction(ParserState.MethodProperties)]
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

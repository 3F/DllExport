//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
//$ Distributed under the MIT License (MIT)

using System;

namespace RGiesecke.DllExport.Parsing.Actions
{
    [ParserStateAction(ParserState.Method)]
    public sealed class MethodParserAction: IlParser.ParserStateAction
    {
        public override void Execute(ParserStateValues state, string trimmedLine)
        {
            if(state.Method.Pinvokeimpl != null && trimmedLine == "}") {
                state.State = ParserState.Class;
                return;
            }

            if(trimmedLine.StartsWith("} // end of method", StringComparison.Ordinal)) {
                state.State = ParserState.Class;
                return;
            }
        }
    }
}

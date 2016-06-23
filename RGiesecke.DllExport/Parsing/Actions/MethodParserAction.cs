// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.4.23262, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;

namespace RGiesecke.DllExport.Parsing.Actions
{
    [ParserStateAction(ParserState.Method)]
    public sealed class MethodParserAction: IlParser.ParserStateAction
    {
        public override void Execute(ParserStateValues state, string trimmedLine)
        {
            if(!trimmedLine.StartsWith("} // end of method", StringComparison.Ordinal))
            {
                return;
            }
            state.State = ParserState.Class;
        }
    }
}

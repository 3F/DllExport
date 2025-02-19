/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

namespace net.r_eg.DllExport.Parsing.Actions
{
    public sealed class ClassExternParserAction: IlParser.ParserStateAction
    {
        public override void Execute(ParserStateValues state, string trimmedLine)
        {
            if(trimmedLine.StartsWith("{"))
            {
                //.class extern forwarder '...'
                //{ <<<<<
                // .assembly extern '...'
                //}

                //.class extern '...'
                //{ <<<<<
                //  .class extern '...'
                //}
            }
            else if(trimmedLine.StartsWith("}"))
            {
                state.State = ParserState.Normal;
            }

            state.AddLine = true;
        }
    }
}

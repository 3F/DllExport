/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

namespace net.r_eg.DllExport.Parsing.Actions
{
    public sealed class AssemblyExternParserAction: ParserStateAction
    {
        public override void Execute(ParserStateValues state, string trimmedLine)
        {
            if(trimmedLine.StartsWith("{"))
            {
                //.assembly extern 'DllExport'
                //{ <<<<<<
                //  .publickeytoken = (83 37 22 4C 9A D9 E3 56 )                         // .7"L...V
                //  .ver 1:7:4:0
                //}
            }
            else if(trimmedLine.StartsWith(".publickeytoken"))
            {
                ProcessPublickeytoken(state, trimmedLine);
            }
            else if(trimmedLine.StartsWith("}"))
            {
                state.State = ParserState.Normal;
            }

            state.AddLine = true;
        }

        private void ProcessPublickeytoken(ParserStateValues state, string trimmedLine)
        {
            int left = trimmedLine.IndexOf("(") + 1;
            if(left > 1)
            {
                string pkt = trimmedLine.Substring(left, trimmedLine.IndexOf(")", left) - left).Trim();
                state.ExternalAssemlyDeclarations[state.ExternalAssemlyDeclarations.Count - 1].Publickeytoken = pkt;
            }
        }
    }
}

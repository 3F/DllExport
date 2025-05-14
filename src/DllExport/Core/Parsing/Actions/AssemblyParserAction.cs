/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using net.r_eg.DllExport.ILAsm;

namespace net.r_eg.DllExport.Parsing.Actions
{
    public sealed class AssemblyParserAction(IInputValues input): ParserStateAction
    {
        private readonly IInputValues inputValues = input;
        private bool emittedTyperefs;

        public override void Execute(ParserStateValues state, string trimmedLine)
        {
            if(!emittedTyperefs && state.State == ParserState.Assembly)
            {
                EmitTypeRefDirectives(state);
            }

            if(trimmedLine.StartsWith("{"))
            {
                state.State = ParserState.Assembly;
            }
            else if(trimmedLine.StartsWith("}"))
            {
                state.State = ParserState.Normal;
            }

            state.AddLine = true;
        }

        private void EmitTypeRefDirectives(ParserStateValues state)
        {
            if(inputValues?.TypeRefDirectives?.Count > 0)
            {
                foreach(TypeRefDirective decl in inputValues.TypeRefDirectives)
                {
                    state.Result.AddRange(decl.Format());
                }
                emittedTyperefs = true;
            }
        }
    }
}

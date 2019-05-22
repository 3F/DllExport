//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2019  Denis Kuzmin < entry.reg@gmail.com > GitHub/3F
//$ Distributed under the MIT License (MIT)

using System;

namespace RGiesecke.DllExport.Parsing.Actions
{
    [ParserStateAction(ParserState.Class)]
    public sealed class ClassParserAction: IlParser.ParserStateAction
    {
        public override void Execute(ParserStateValues state, string trimmedLine)
        {
            if(trimmedLine.StartsWith(".class", StringComparison.Ordinal))
            {
                state.State = ParserState.ClassDeclaration;
                state.AddLine = true;
                state.ClassDeclaration = trimmedLine;
            }
            else if(trimmedLine.StartsWith(".method", StringComparison.Ordinal))
            {
                ExportedClass exportedClass;
                if(state.ClassNames.Count == 0 || !this.Parser.Exports.ClassesByName.TryGetValue(state.ClassNames.Peek(), out exportedClass))
                {
                    return;
                }
                state.Method.Reset();
                state.Method.Declaration = trimmedLine;
                state.AddLine = false;
                state.State = ParserState.MethodDeclaration;
            }
            else
            {
                if(!trimmedLine.StartsWith("} // end of class", StringComparison.Ordinal))
                {
                    return;
                }
                state.ClassNames.Pop();
                state.State = state.ClassNames.Count > 0 ? ParserState.Class : ParserState.Normal;
            }
        }
    }
}

//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2021  Denis Kuzmin <x-3F@outlook.com> github/3F
//$ Distributed under the MIT License (MIT)

using System;
using System.Text;

namespace RGiesecke.DllExport.Parsing.Actions
{
    [ParserStateAction(ParserState.ClassDeclaration)]
    public sealed class ClassDeclarationParserAction: IlParser.ParserStateAction
    {
        public override void Execute(ParserStateValues state, string trimmedLine)
        {
            if(trimmedLine.StartsWith("{"))
            {
                state.State = ParserState.Class;
                string str = ClassDeclarationParserAction.GetClassName(state);
                if(state.ClassNames.Count > 0)
                {
                    str = state.ClassNames.Peek() + "/" + str;
                }
                state.ClassNames.Push(str);
            }
            else
            {
                state.ClassDeclaration = state.ClassDeclaration + " " + trimmedLine;
                state.AddLine = true;
            }
        }

        private static string GetClassName(ParserStateValues state)
        {
            bool hadClassName = false;
            StringBuilder classNameBuilder = new StringBuilder(state.ClassDeclaration.Length);
            IlParsingUtils.ParseIlSnippet(state.ClassDeclaration, ParsingDirection.Forward, (Func<IlParsingUtils.IlSnippetLocation, bool>)(s => {
                if(s.WithinString)
                {
                    hadClassName = true;
                    if((int)s.CurrentChar != 39)
                    {
                        classNameBuilder.Append(s.CurrentChar);
                    }
                }
                else if(hadClassName)
                {
                    if((int)s.CurrentChar == 46 || (int)s.CurrentChar == 47)
                    {
                        classNameBuilder.Append(s.CurrentChar);
                    }
                    else if((int)s.CurrentChar != 39)
                    {
                        return false;
                    }
                }
                return true;
            }), (Action<IlParsingUtils.IlSnippetFinalizaton>)null);
            return classNameBuilder.ToString();
        }
    }
}

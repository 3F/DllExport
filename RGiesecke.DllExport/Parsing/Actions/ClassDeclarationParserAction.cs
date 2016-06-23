// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.1.28776, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using System.Text.RegularExpressions;

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
                string str = "";
                Match match = state.MatchClass(state.ClassDeclaration);
                if(match.Groups.Count > 1)
                {
                    str = match.Groups[1].Value.NullSafeCall<string, string>((Func<string, string>)(v => v.Replace("'", "")));
                }
                if(state.ClassNames.Count > 0)
                {
                    str = state.ClassNames.Peek() + "+" + str;
                }
                state.ClassNames.Push(str);
                state.Result.Add(state.ClassDeclaration);
            }
            else
            {
                state.ClassDeclaration = state.ClassDeclaration + " " + trimmedLine;
                state.AddLine = false;
            }
        }
    }
}

//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2019  Denis Kuzmin < entry.reg@gmail.com > GitHub/3F
//$ Distributed under the MIT License (MIT)

using System;
using System.Text;
using System.Text.RegularExpressions;

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
            else if(trimmedLine.StartsWith(".field", StringComparison.Ordinal))
            {
                if(TreatField(state, ref trimmedLine))
                {
                    state.AddLine = false;
                    state.Result.Add(trimmedLine);
                }
                return;
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

        /// <param name="state"></param>
        /// <param name="raw">raw definition of the .field</param>
        /// <returns>true if processed</returns>
        private bool TreatField(ParserStateValues state, ref string raw)
        {
            if((Parser.InputValues.Patches & PatchesType.InfToken) == PatchesType.InfToken)
            {
                // .field public static literal float32 'Infinity' = float32(inf)
                // .field public static literal float32 'NegativeInfinity' = float32(-inf)
                // .field public static literal float64 'Infinity' = float64(inf)
                // .field public static literal float64 'NegativeInfinity' = float64(-inf)

                Match m = Regex.Match
                (
                    raw,
                    @"=\s*
                        float(?:(?'x64'64)|32)
                        \(
                            (?'sign'-?)
                            inf
                        \)
                    ", 
                    RegexOptions.IgnorePatternWhitespace
                );

                if(m.Success) 
                {
                    raw = new string(' ', 2) + raw.Substring(0, m.Index) + GetFloatDef(m);
                    return true;
                }
            }

            return false;
        }

        private static string GetFloatDef(Match fld)
        {
            var sb = new StringBuilder(4);
            sb.Append("= float");

            if(fld.Groups["x64"].Success)
            {
                sb.Append("64");
                sb.Append
                (
                    fld.Groups["sign"].Value.Length > 0 ? 
                        "(0xFFF0000000000000)" : "(0x7FF0000000000000)"
                );

                return sb.ToString();
            }

            sb.Append("32");
            sb.Append
            (
                fld.Groups["sign"].Value.Length > 0 ?
                    "(0xFF800000)" : "(0x7F800000)"
            );

            return sb.ToString();
        }
    }
}

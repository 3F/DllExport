//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2019  Denis Kuzmin < entry.reg@gmail.com > GitHub/3F
//$ Distributed under the MIT License (MIT)

using System;
using System.Text;
using System.Text.RegularExpressions;

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

            if(trimmedLine.StartsWith("IL_", StringComparison.Ordinal))
            {
                if(TreatIL(state, ref trimmedLine))
                {
                    state.AddLine = false;
                    state.Result.Add(trimmedLine);
                }
                return;
            }
        }

        private bool TreatIL(ParserStateValues state, ref string raw)
        {
            if((Parser.InputValues.Patches & PatchesType.InfToken) == PatchesType.InfToken)
            {
                // ldc.r8     inf
                // ldc.r8     -inf
                // ldc.r4     inf
                // ldc.r4     -inf

                Match m = Regex.Match
                (
                    raw,
                    @"
                        ldc.r(?:(?'x64'8)|4)
                        \s*
                        (?'sign'-?)
                        inf
                    ", 
                    RegexOptions.IgnorePatternWhitespace
                );

                if(m.Success)
                {
                    raw = new string(' ', 4) + raw.Substring(0, m.Index) + GetFloatDef(m);
                    return true;
                }
            }

            return false;
        }

        private static string GetFloatDef(Match fld)
        {
            var sb = new StringBuilder(4);
            sb.Append("ldc.r");

            if(fld.Groups["x64"].Success)
            {
                sb.Append("8     ");
                sb.Append
                (
                    fld.Groups["sign"].Value.Length > 0 ? 
                        "(00 00 00 00 00 00 F0 FF)" : "(00 00 00 00 00 00 F0 7F)"
                );

                return sb.ToString();
            }

            sb.Append("4     ");
            sb.Append
            (
                fld.Groups["sign"].Value.Length > 0 ? 
                    "(00 00 80 FF)" : "(00 00 80 7F)"
            );

            return sb.ToString();
        }
    }
}

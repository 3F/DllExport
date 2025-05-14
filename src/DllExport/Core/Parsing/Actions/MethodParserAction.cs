/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Text;
using System.Text.RegularExpressions;

namespace net.r_eg.DllExport.Parsing.Actions
{
    public sealed class MethodParserAction: ParserStateAction
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

        // https://github.com/3F/DllExport/issues/128
        // https://github.com/3F/DllExport/issues/158
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
                    raw = GetInst(raw.Substring(0, m.Index) + GetFloatInfDef(m));
                    return true;
                }
            }

            if((Parser.InputValues.Patches & PatchesType.NaNToken) == PatchesType.NaNToken)
            {
                // ldc.r8     -nan(ind)
                // ldc.r4     -nan(ind)

                Match m = Regex.Match
                (
                    raw,
                    @"
                        ldc.r(?:(?'x64'8)|4)
                        \s*
                        -nan\(ind\)
                    ", 
                    RegexOptions.IgnorePatternWhitespace
                );

                if(m.Success)
                {
                    raw = GetInst(raw.Substring(0, m.Index) + GetNaNDef(m));
                    return true;
                }
            }

            return false;
        }

        private static string GetFloatInfDef(Match fld)
        {
            var sb = new StringBuilder(capacity: 36);
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

        private static string GetNaNDef(Match fld)
        {
            var sb = new StringBuilder(capacity: 36);
            sb.Append("ldc.r");

            if(fld.Groups["x64"].Success)
            {
                sb.Append("8     ");
                sb.Append("(00 00 00 00 00 00 F8 FF)");
            }
            else
            {
                sb.Append("4     ");
                sb.Append("(00 00 C0 FF)");
            }

            return sb.ToString();
        }

        private static string GetInst(string l) => new string(' ', 4) + l;
    }
}

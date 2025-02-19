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
                if(state.ClassNames.Count == 0 || !Parser.Exports.ClassesByName.TryGetValue(state.ClassNames.Peek(), out _))
                {
                    state.State = ParserState.Method; // https://github.com/3F/DllExport/issues/174
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

        /// <summary>
        /// https://github.com/3F/DllExport/issues/128
        /// https://github.com/3F/DllExport/issues/158
        /// </summary>
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
                    raw = GetInst(raw.Substring(0, m.Index) + GetFloatInfDef(m));
                    return true;
                }
            }

            if((Parser.InputValues.Patches & PatchesType.NaNToken) == PatchesType.NaNToken)
            {
                // .field public static literal float32 'NaN' = float32(-nan(ind))
                // .field public static literal float64 'NaN' = float64(-nan(ind))

                Match m = Regex.Match
                (
                    raw,
                    @"=\s*
                        float(?:(?'x64'64)|32)
                        \(
                            -nan\(ind\)
                        \)
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
            var sb = new StringBuilder(capacity: 29);
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

        private static string GetNaNDef(Match fld)
        {
            var sb = new StringBuilder(capacity: 29);
            sb.Append("= float");

            if(fld.Groups["x64"].Success)
            {
                sb.Append("64");
                sb.Append("(0xFFF8000000000000)");
            }
            else
            {
                sb.Append("32");
                sb.Append("(0xFFC00000)");
            }

            return sb.ToString();
        }

        private static string GetInst(string l) => new string(' ', 2) + l;
    }
}

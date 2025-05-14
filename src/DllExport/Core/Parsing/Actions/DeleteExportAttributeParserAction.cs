/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace net.r_eg.DllExport.Parsing.Actions
{
    public sealed class DeleteExportAttributeParserAction: ParserStateAction
    {
        public override void Execute(ParserStateValues state, string trimmedLine)
        {
            if(!trimmedLine.StartsWith(".custom", StringComparison.InvariantCulture) // .custom instance void ['DllExport']'...'.'DllExportAttribute'::.ctor(string) = ( 01 00 06 50 72 69 6E 74 31 00 00 ) // ...Print1..
                && !trimmedLine.StartsWith(".maxstack", StringComparison.InvariantCulture))
            {
                state.AddLine = false;
                return;
            }
            state.State = ParserState.Method;

            ExportedClass exportedClass;
            if(!Exports.ClassesByName.TryGetValue(state.ClassNames.Peek(), out exportedClass)) {
                state.AddLine = false;
                return;
            }

            ExportedMethod exportMethod = GetExportedMethod(state, exportedClass);
            string declaration          = state.Method.Declaration;
            StringBuilder stringBuilder = new StringBuilder(250);

            stringBuilder.Append(".method ").Append(state.Method.Attributes?.Trim()).Append(" ");
            stringBuilder.Append(state.Method.Result?.Trim());

#if F_LEGACY_EMIT_MSCORLIB
            string convBase = "mscorlib";
#else
            string convBase = state.ExternalAssemlyDeclarations.FirstOrDefault()?.AliasName ?? "mscorlib";
#endif
            stringBuilder.Append($" modopt(['{convBase}']'")
                .Append(AssemblyExports.ConventionTypeNames[exportMethod.CallingConvention]).Append("') ");

            if(!String.IsNullOrEmpty(state.Method.ResultAttributes)) {
                stringBuilder.Append(" ").Append(state.Method.ResultAttributes);
            }

            stringBuilder.Append(" '").Append(state.Method.Name).Append("'").Append(state.Method.After?.Trim());
            bool flag = ValidateExportNameAndLogError(exportMethod, state);

            if(flag) {
                state.Method.Declaration = stringBuilder.ToString();
            }

            if(state.MethodPos != 0) {
                state.Result.Insert(state.MethodPos, state.Method.Declaration);
            }

            if(flag)
            {
                Notifier.Notify(-2, DllExportLogginCodes.OldDeclaration, "\t" + Resources.OldDeclaration_0_, declaration);
                Notifier.Notify(-2, DllExportLogginCodes.NewDeclaration, "\t" + Resources.NewDeclaration_0_, state.Method.Declaration);

                state.Result.Add(
                    String.Format(
                        CultureInfo.InvariantCulture, 
                        "    .export [{0}] as '{1}'",
                        exportMethod.VTableOffset,
                        exportMethod.ExportName
                    )
                );

                Notifier.Notify(-1, DllExportLogginCodes.AddingVtEntry, "\t" + Resources.AddingVtEntry_0_export_1_, exportMethod.VTableOffset, exportMethod.ExportName);
            }
        }

        private ExportedMethod GetExportedMethod(ParserStateValues state, ExportedClass exportedClass)
        {
            //TODO: see details in NextExportedMethod()
            return exportedClass.NextExportedMethod(state.Method.Name);
        }
    }
}

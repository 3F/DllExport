//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2021  Denis Kuzmin <x-3F@outlook.com> github/3F
//$ Distributed under the MIT License (MIT)

using System;
using System.Globalization;
using System.Text;
using RGiesecke.DllExport.Properties;

namespace RGiesecke.DllExport.Parsing.Actions
{
    [ParserStateAction(ParserState.DeleteExportAttribute)]
    public sealed class DeleteExportAttributeParserAction: IlParser.ParserStateAction
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

            stringBuilder.Append(".method ").Append(state.Method.Attributes.NullSafeTrim()).Append(" ");
            stringBuilder.Append(state.Method.Result.NullSafeTrim());
            stringBuilder.Append(" modopt(['mscorlib']'").Append(AssemblyExports.ConventionTypeNames[exportMethod.CallingConvention]).Append("') ");

            if(!String.IsNullOrEmpty(state.Method.ResultAttributes)) {
                stringBuilder.Append(" ").Append(state.Method.ResultAttributes);
            }

            stringBuilder.Append(" '").Append(state.Method.Name).Append("'").Append(state.Method.After.NullSafeTrim());
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

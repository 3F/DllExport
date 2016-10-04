// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.7.38850, Culture=neutral, PublicKeyToken=8f52d83c1a22df51
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

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

            ExportedMethod exportMethod = getExportedMethod(state, exportedClass);
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

        private ExportedMethod getExportedMethod(ParserStateValues state, ExportedClass exportedClass)
        {
            var exportedMethods = exportedClass.MethodsByName[state.Method.Name];

            // TODO: To get from zero index - it was in original code, as getting of first exported method.
            //       However, to solve problem https://github.com/3F/DllExport/issues/10 I also added the `ExportName` instead of `MemberName` in AssemblyExports.cs
            //       And I left it 'as is', but the good way to provide information about arguments, i.e.:
            //       ~ 'Print'(int32 'a') cil managed   -> .export [0] as 'Print1'
            //       ~ 'Print'(bool 'b') cil managed    -> .export [1] as 'Print2'
            try {
                return exportedMethods[0];
            }
            finally {
                exportedMethods.RemoveAt(0); // moving in order by adding ^
            }
        }
    }
}

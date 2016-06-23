// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.3.29766, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
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
            if(trimmedLine.StartsWith(".custom", StringComparison.Ordinal) || trimmedLine.StartsWith("// Code", StringComparison.Ordinal))
            {
                ExportedClass exportedClass;
                if(this.Exports.ClassesByName.TryGetValue(state.ClassNames.Peek(), out exportedClass))
                {
                    ExportedMethod exportMethod = exportedClass.MethodsByName[state.Method.Name][0];
                    string declaration = state.Method.Declaration;
                    StringBuilder stringBuilder = new StringBuilder(250);
                    stringBuilder.Append(".method ").Append(state.Method.Attributes.NullSafeTrim()).Append(" ");
                    stringBuilder.Append(state.Method.Result.NullSafeTrim());
                    stringBuilder.Append(" modopt([mscorlib]").Append(AssemblyExports.ConventionTypeNames[exportMethod.CallingConvention]).Append(") ");
                    if(!string.IsNullOrEmpty(state.Method.ResultAttributes))
                    {
                        stringBuilder.Append(" ").Append(state.Method.ResultAttributes);
                    }
                    stringBuilder.Append(" '").Append(state.Method.Name).Append("'").Append(state.Method.After.NullSafeTrim());
                    bool flag = this.ValidateExportNameAndLogError(exportMethod, state);
                    if(flag)
                    {
                        state.Method.Declaration = stringBuilder.ToString();
                    }
                    if(state.MethodPos != 0)
                    {
                        state.Result.Insert(state.MethodPos, state.Method.Declaration);
                    }
                    if(flag)
                    {
                        this.Notifier.Notify(-2, DllExportLogginCodes.OldDeclaration, "\t" + Resources.OldDeclaration_0_, (object)declaration);
                        this.Notifier.Notify(-2, DllExportLogginCodes.NewDeclaration, "\t" + Resources.NewDeclaration_0_, (object)state.Method.Declaration);
                        state.Result.Add(string.Format((IFormatProvider)CultureInfo.InvariantCulture, "    .export [{0}] as '{1}'", new object[2]
                        {
                        (object) exportMethod.VTableOffset,
                        (object) exportMethod.ExportName
                        }));
                        this.Notifier.Notify(-1, DllExportLogginCodes.AddingVtEntry, "\t" + Resources.AddingVtEntry_0_export_1_, (object)exportMethod.VTableOffset, (object)exportMethod.ExportName);
                    }
                }
                else
                {
                    state.AddLine = false;
                }
                state.State = ParserState.Method;
            }
            else
            {
                state.AddLine = false;
            }
        }
    }
}

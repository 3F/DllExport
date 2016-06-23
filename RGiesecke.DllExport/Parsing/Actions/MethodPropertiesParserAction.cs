// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.7.38850, Culture=neutral, PublicKeyToken=8f52d83c1a22df51
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using RGiesecke.DllExport.Properties;

namespace RGiesecke.DllExport.Parsing.Actions
{
    [ParserStateAction(ParserState.MethodProperties)]
    public sealed class MethodPropertiesParserAction: IlParser.ParserStateAction
    {
        public override void Execute(ParserStateValues state, string trimmedLine)
        {
            if(trimmedLine.StartsWith(".custom instance void ", StringComparison.Ordinal) && trimmedLine.Contains(this.Parser.DllExportAttributeIlAsmFullName))
            {
                state.AddLine = false;
                state.State = ParserState.DeleteExportAttribute;
                this.Notifier.Notify(-2, DllExportLogginCodes.RemovingDllExportAttribute, Resources.Removing_0_from_1_, (object)Utilities.DllExportAttributeFullName, (object)(state.ClassNames.Peek() + "." + state.Method.Name));
            }
            else
            {
                if(!trimmedLine.StartsWith("// Code", StringComparison.Ordinal))
                {
                    return;
                }
                state.State = ParserState.Method;
                if(state.MethodPos == 0)
                {
                    return;
                }
                state.Result.Insert(state.MethodPos, state.Method.Declaration);
            }
        }
    }
}

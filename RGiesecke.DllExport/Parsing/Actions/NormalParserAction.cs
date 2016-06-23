// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.1.28776, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using System.Globalization;

namespace RGiesecke.DllExport.Parsing.Actions
{
    [ParserStateAction(ParserState.Normal)]
    public sealed class NormalParserAction: IlParser.ParserStateAction
    {
        public override void Execute(ParserStateValues state, string trimmedLine)
        {
            if(trimmedLine.StartsWith(".corflags", StringComparison.Ordinal))
            {
                state.Result.Add(string.Format((IFormatProvider)CultureInfo.InvariantCulture, ".corflags 0x{0}", new object[1]
                {
                (object) Utilities.GetCoreFlagsForPlatform(state.Cpu).ToString("X8", (IFormatProvider) CultureInfo.InvariantCulture)
                }));
                state.AddLine = false;
            }
            else if(trimmedLine.StartsWith(".class", StringComparison.Ordinal))
            {
                state.State = ParserState.ClassDeclaration;
                state.AddLine = false;
                state.ClassDeclaration = trimmedLine;
            }
            else
            {
                if(!trimmedLine.StartsWith(".assembly extern " + this.DllExportAttributeAssemblyName, StringComparison.Ordinal))
                {
                    return;
                }
                state.AddLine = false;
                state.State = ParserState.DeleteExportDependency;
                this.Notifier.Notify(-1, "EXP0010", "Deleting " + this.DllExportAttributeFullName + " dependency.");
            }
        }
    }
}

// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.3.29766, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using System.Globalization;
using RGiesecke.DllExport.Properties;

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
                state.AddLine = true;
                state.ClassDeclaration = trimmedLine;
            }
            else
            {
                if(!this.IsExportAttributeAssemblyReference(trimmedLine))
                {
                    return;
                }
                state.AddLine = false;
                state.State = ParserState.DeleteExportDependency;
                this.Notifier.Notify(-1, DllExportLogginCodes.RemovingReferenceToDllExportAttributeAssembly, string.Format(Resources.Deleting_reference_to_0_, (object)this.DllExportAttributeAssemblyName));
            }
        }

        private bool IsExportAttributeAssemblyReference(string trimmedLine)
        {
            return trimmedLine.StartsWith(".assembly extern '" + this.DllExportAttributeAssemblyName + "'", StringComparison.Ordinal);
        }
    }
}

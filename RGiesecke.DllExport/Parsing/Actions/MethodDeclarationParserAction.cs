// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.1.28776, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using System.Text.RegularExpressions;

namespace RGiesecke.DllExport.Parsing.Actions
{
    [ParserStateAction(ParserState.MethodDeclaration)]
    public sealed class MethodDeclarationParserAction: IlParser.ParserStateAction
    {
        public override void Execute(ParserStateValues state, string trimmedLine)
        {
            if(trimmedLine.StartsWith("{", StringComparison.Ordinal))
            {
                ExportedClass exportedClass;
                if(this.GetIsExport(state, out exportedClass))
                {
                    this.Notifier.Notify(-1, "EXP0001", "Found method: " + exportedClass.FullTypeName + "." + state.Method.Declaration);
                    state.MethodPos = state.Result.Count;
                    state.State = ParserState.MethodProperties;
                }
                else
                {
                    state.Result.Add(state.Method.Declaration);
                    state.State = ParserState.Method;
                    state.MethodPos = 0;
                }
            }
            else
            {
                state.Method.Declaration = state.Method.Declaration + " " + trimmedLine;
                state.AddLine = false;
            }
        }

        private bool GetIsExport(ParserStateValues state, out ExportedClass exportedClass)
        {
            Match match = state.MatchMethod(state.Method.Declaration);
            if(match.Groups.Count > 3)
            {
                state.Method.After = match.Groups["afterMethod"].Value;
                state.Method.Name = match.Groups["methodName"].Value.NullSafeTrim('\'');
                state.Method.Attributes = match.Groups["methodAttributes"].Value.NullSafeTrim();
                state.Method.Result = match.Groups["result"].Value.NullSafeTrim();
                state.Method.ResultAttributes = match.Groups["resultAttributes"].Value.NullSafeTrim();
            }
            bool flag = this.Exports.ClassesByName.TryGetValue(state.ClassNames.Peek(), out exportedClass) && exportedClass.MethodsByName.ContainsKey(state.Method.Name);
            if(!flag)
            {
                ExportedMethod duplicateExport = this.Exports.GetDuplicateExport(exportedClass.FullTypeName, state.Method.Name);
                this.ValidateExportNameAndLogError(duplicateExport, state);
                if(duplicateExport != null)
                {
                    string fileName;
                    SourceCodePosition startPosition;
                    SourceCodePosition endPosition;
                    if(IlParser.ParserStateAction.TryGetLineNumbers(state, out fileName, out startPosition, out endPosition))
                    {
                        this.Notifier.Notify(1, "EXP00010", fileName, new SourceCodePosition?(startPosition), new SourceCodePosition?(endPosition), "Ambiguous export name '{0}' on '{1}'.'{2}'.", (object)duplicateExport.ExportName, (object)duplicateExport.ExportedClass.FullTypeName, (object)duplicateExport.MemberName);
                    }
                    else
                    {
                        this.Notifier.Notify(1, "EXP00010", "Ambiguous export name '{0}' on '{1}'.'{2}'.", (object)duplicateExport.ExportName, (object)duplicateExport.ExportedClass.FullTypeName, (object)duplicateExport.MemberName);
                    }
                }
            }
            return flag;
        }
    }
}

/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace net.r_eg.DllExport.Parsing.Actions
{
    public abstract class ParserStateAction: IParserStateAction
    {
        protected string DllExportAttributeAssemblyName => Parser.DllExportAttributeAssemblyName;

        protected string DllExportAttributeFullName => Parser.DllExportAttributeFullName;

        protected IDllExportNotifier Notifier => Parser.GetNotifier();

        protected AssemblyExports Exports => Parser.Exports;

        public long Milliseconds { get; set; }

        public IlParser Parser { get; set; }

        public abstract void Execute(ParserStateValues state, string trimmedLine);

        protected void Notify(int severity, string code, string message, params object[] values)
        {
            Notify(stateValues: null, severity, code, message, values);
        }

        protected void Notify(ParserStateValues stateValues, int severity, string code, string message, params object[] values)
        {
            SourceCodeRange range = stateValues?.GetRange();
            if(range != null)
            {
                Notifier.Notify(severity, code, range.FileName, new SourceCodePosition?(range.StartPosition), new SourceCodePosition?(range.EndPosition), message, values);
            }
            else
            {
                Notifier.Notify(severity, code, message, values);
            }
        }

        protected bool ValidateExportNameAndLogError(ExportedMethod exportMethod, ParserStateValues stateValues)
        {
            if(exportMethod == null) return false;

            if(exportMethod.ExportName != null
                && (exportMethod.ExportName.Contains("'") || Regex.IsMatch(exportMethod.ExportName, "\\P{IsBasicLatin}")))
            {
                Notify
                (
                    stateValues,
                    severity: 2,
                    DllExportLogginCodes.ExportNamesHaveToBeBasicLatin,
                    Resources.Export_name_0_on_1__2_is_Unicode_windows_export_names_have_to_be_basic_latin,
                    exportMethod.ExportName,
                    exportMethod.ExportedClass.FullTypeName,
                    exportMethod.MemberName
                );
                return false;
            }
            return true;
        }

        public static Dictionary<ParserState, IParserStateAction> GetActionsByState(IlParser parser)
        {
            AssemblyParserAction assemblyParser = new(parser.InputValues);
            ClassExternParserAction classExternParser = new();

            Dictionary<ParserState, IParserStateAction> dictionary = new()
            {
                { ParserState.ClassDeclaration, new ClassDeclarationParserAction() },
                { ParserState.Class, new ClassParserAction() },
                { ParserState.ClassExtern, classExternParser },
                { ParserState.ClassExternForwarder, classExternParser },
                { ParserState.DeleteExportAttribute, new DeleteExportAttributeParserAction() },
                { ParserState.MethodDeclaration, new MethodDeclarationParserAction() },
                { ParserState.Method, new MethodParserAction() },
                { ParserState.MethodProperties, new MethodPropertiesParserAction() },
                { ParserState.Normal, new NormalParserAction(parser.InputValues) },
                { ParserState.AssemblyDeclaration, assemblyParser },
                { ParserState.Assembly, assemblyParser },
                { ParserState.AssemblyExtern, new AssemblyExternParserAction() },
            };

            foreach(IParserStateAction parserStateAction in dictionary.Values)
            {
                parserStateAction.Parser = parser;
            }
            return dictionary;
        }
    }
}

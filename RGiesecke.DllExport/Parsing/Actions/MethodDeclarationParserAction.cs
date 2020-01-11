//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
//$ Distributed under the MIT License (MIT)

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using RGiesecke.DllExport.Properties;

namespace RGiesecke.DllExport.Parsing.Actions
{
    [ParserStateAction(ParserState.MethodDeclaration)]
    public sealed class MethodDeclarationParserAction: IlParser.ParserStateAction
    {
        private static readonly Regex _CilManagedRegex = new Regex("\\b(?:cil|managed)\\b", RegexOptions.Compiled);

        public override void Execute(ParserStateValues state, string trimmedLine)
        {
            if(trimmedLine.StartsWith("{", StringComparison.Ordinal))
            {
                ExportedClass exportedClass;
                if(this.GetIsExport(state, out exportedClass))
                {
                    this.Notify(-1, DllExportLogginCodes.MethodFound, string.Format(Resources.Found_method_0_1_, (object)exportedClass.FullTypeName, (object)state.Method.Declaration));
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

        private bool GetPartBeforeParameters(string line, out string methodName, out string afterMethodName, out string foundResult, out string foundResultModifier, out string foundMethodAttributes)
        {
            methodName = (string)null;
            foundResult = (string)null;
            foundResultModifier = (string)null;
            afterMethodName = (string)null;
            foundMethodAttributes = (string)null;
            line = line.TrimStart();
            if(!line.StartsWith(".method"))
            {
                return false;
            }
            line = line.Substring(".method".Length).TrimStart();
            StringBuilder afterMethodNameBuilder = new StringBuilder(line.Length);
            string result = (string)null;
            IlParsingUtils.ParseIlSnippet(line, ParsingDirection.Backward, (Func<IlParsingUtils.IlSnippetLocation, bool>)(s => {
                if(!s.WithinString && s.AtOuterBracket)
                {
                    if((int)s.CurrentChar != 41)
                    {
                        result = line.Substring(0, s.Index);
                        afterMethodNameBuilder.Insert(0, s.CurrentChar);
                        return false;
                    }
                    MethodDeclarationParserAction.RemoveCilManagedFromMethodSuffix(ref afterMethodNameBuilder);
                }
                afterMethodNameBuilder.Insert(0, s.CurrentChar);
                return true;
            }), (Action<IlParsingUtils.IlSnippetFinalizaton>)null);
            if(afterMethodNameBuilder.Length > 0)
            {
                afterMethodName = afterMethodNameBuilder.ToString();
            }
            if(result != null)
            {
                string attributesWithResult;
                methodName = MethodDeclarationParserAction.ExtractMethodName(result, out attributesWithResult);
                if(this.SplitAttributesAndResult(attributesWithResult, out foundResult, out foundMethodAttributes))
                {
                    if(foundResult != null && foundResult.Contains("("))
                    {
                        MethodDeclarationParserAction.ExtractResultModifier(ref foundResult, out foundResultModifier);
                    }
                    return true;
                }
            }
            return false;
        }

        private static void ExtractResultModifier(ref string foundResult, out string foundResultModifier)
        {
            int bracketEnd = -1;
            string localFoundResult = foundResult;
            string localfoundResultModifier = (string)null;
            IlParsingUtils.ParseIlSnippet(foundResult, ParsingDirection.Backward, (Func<IlParsingUtils.IlSnippetLocation, bool>)(s => {
                if(s.WithinString || !s.AtOuterBracket)
                {
                    return true;
                }
                if((int)s.CurrentChar == 41)
                {
                    bracketEnd = s.Index;
                    return true;
                }
                string str = s.InputText.Substring(0, s.Index);
                int num = str.LastIndexOf(' ');
                if(num > -1 && str.Substring(num + 1) == "marshal")
                {
                    localfoundResultModifier = s.InputText.Substring(num + 1, bracketEnd - num);
                    localFoundResult = s.InputText.Remove(num + 1, bracketEnd - num);
                }
                return true;
            }), (Action<IlParsingUtils.IlSnippetFinalizaton>)null);
            foundResult = localFoundResult;
            foundResultModifier = localfoundResultModifier;
        }

        private static bool RemoveCilManagedFromMethodSuffix(ref StringBuilder afterMethodNameBuilder)
        {
            bool cilManagedFound = false;
            string str = MethodDeclarationParserAction._CilManagedRegex.Replace(afterMethodNameBuilder.ToString(), (MatchEvaluator)(m => {
                cilManagedFound = true;
                return "";
            }));
            if(cilManagedFound)
            {
                afterMethodNameBuilder = new StringBuilder(str, afterMethodNameBuilder.Capacity);
            }
            return cilManagedFound;
        }

        private bool SplitAttributesAndResult(string attributesWithResult, out string foundResult, out string foundMethodAttributes)
        {
            if(string.IsNullOrEmpty(attributesWithResult))
            {
                foundResult = (string)null;
                foundMethodAttributes = (string)null;
                return false;
            }
            int startIndex = -1;
            int num = -1;
            for(int index = 0; index < attributesWithResult.Length; ++index)
            {
                char c = attributesWithResult[index];
                if((int)c == 39)
                {
                    num = index;
                    break;
                }
                if(char.IsWhiteSpace(c) || index == attributesWithResult.Length - 1)
                {
                    if(startIndex > -1)
                    {
                        if(!this.Parser.MethodAttributes.Contains(attributesWithResult.Substring(startIndex, index - startIndex).Trim()))
                        {
                            num = startIndex;
                            break;
                        }
                        startIndex = index + 1;
                    }
                }
                else if(startIndex < 0)
                {
                    startIndex = index;
                }
            }
            if(num > -1)
            {
                foundMethodAttributes = attributesWithResult.Substring(0, num).Trim();
                foundResult = attributesWithResult.Substring(num).Trim();
                return true;
            }
            foundResult = (string)null;
            foundMethodAttributes = (string)null;
            return false;
        }

        private static string ExtractMethodName(string result, out string attributesWithResult)
        {
            string localAttributesWithResult = (string)null;
            StringBuilder methodNameBuilder = new StringBuilder(result.Length);
            IlParsingUtils.ParseIlSnippet(result, ParsingDirection.Backward, (Func<IlParsingUtils.IlSnippetLocation, bool>)(s => {
                if((int)s.CurrentChar == 39)
                {
                    return true;
                }
                if(!s.WithinString && (int)s.CurrentChar != 46 && ((int)s.CurrentChar != 44 && (int)s.CurrentChar != 47) && ((int)s.CurrentChar != 60 && (int)s.CurrentChar != 62 && (int)s.CurrentChar != 33))
                {
                    return false;
                }
                methodNameBuilder.Insert(0, s.CurrentChar);
                return true;
            }), (Action<IlParsingUtils.IlSnippetFinalizaton>)(f => localAttributesWithResult = f.LastPosition > -1 ? result.Substring(0, f.LastPosition) : (string)null));
            string @string = methodNameBuilder.ToString();
            attributesWithResult = localAttributesWithResult;
            return @string;
        }

        private static StringBuilder OldExtractMethodName(string result, out string attributesWithResult)
        {
            StringBuilder stringBuilder = new StringBuilder(result.Length);
            bool flag = false;
            int length = -1;
            for(int index = result.Length - 1; index > -1; --index)
            {
                char ch = result[index];
                if((int)ch == 39)
                {
                    flag = !flag;
                }
                else if(flag || (int)ch == 46 || ((int)ch == 44 || (int)ch == 47) || ((int)ch == 60 || (int)ch == 62 || (int)ch == 33))
                {
                    stringBuilder.Insert(0, ch);
                }
                else
                {
                    break;
                }
                length = index;
            }
            attributesWithResult = length > -1 ? result.Substring(0, length) : (string)null;
            return stringBuilder;
        }

        private bool GetIsExport(ParserStateValues state, out ExportedClass exportedClass)
        {
            if(!this.ExtractMethodParts(state))
            {
                exportedClass = (ExportedClass)null;
                return false;
            }
            bool flag1 = this.Exports.ClassesByName.TryGetValue(state.ClassNames.Peek(), out exportedClass) && exportedClass != null;
            List<ExportedMethod> exportedMethodList1 = (List<ExportedMethod>)null;
            if(flag1 && exportedClass.HasGenericContext)
            {
                if(exportedClass.MethodsByName.TryGetValue(state.Method.Name, out exportedMethodList1))
                {
                    exportedMethodList1.ForEach((Action<ExportedMethod>)(method => this.Notify(2, DllExportLogginCodes.ExportInGenericType, Resources.The_type_1_cannot_export_the_method_2_as_0_because_it_is_generic_or_is_nested_within_a_generic_type, (object)method.ExportName, (object)method.ExportedClass.FullTypeName, (object)method.MemberName)));
                }
                return false;
            }
            bool flag2 = flag1 && exportedClass.MethodsByName.TryGetValue(state.Method.Name, out exportedMethodList1);
            if(flag1 && !flag2)
            {
                ExportedMethod duplicateExport = this.Exports.GetDuplicateExport(exportedClass.FullTypeName, state.Method.Name);
                this.ValidateExportNameAndLogError(duplicateExport, state);
                if(duplicateExport != null)
                {
                    this.Notify(state, 1, DllExportLogginCodes.AmbigiguousExportName, Resources.Ambiguous_export_name_0_on_1_2_, (object)duplicateExport.ExportName, (object)duplicateExport.ExportedClass.FullTypeName, (object)duplicateExport.MemberName);
                }
            }
            else
            {
                List<ExportedMethod> exportedMethodList2;
                if((exportedMethodList2 = exportedMethodList1 ?? exportedClass.NullSafeCall<ExportedClass, List<ExportedMethod>>((Func<ExportedClass, List<ExportedMethod>>)(i => i.Methods))) != null)
                {
                    foreach(ExportedMethod exportedMethod in exportedMethodList2)
                    {
                        if(!exportedMethod.IsStatic)
                        {
                            flag2 = false;
                            this.Notify(state, 2, DllExportLogginCodes.MethodIsNotStatic, Resources.The_method_1_2_is_not_static_export_name_0_, (object)exportedMethod.ExportName, (object)exportedMethod.ExportedClass.FullTypeName, (object)exportedMethod.MemberName);
                        }
                        if(exportedMethod.IsGeneric)
                        {
                            flag2 = false;
                            this.Notify(state, 2, DllExportLogginCodes.ExportOnGenericMethod, Resources.The_method_1_2_is_generic_export_name_0_Generic_methods_cannot_be_exported_, (object)exportedMethod.ExportName, (object)exportedMethod.ExportedClass.FullTypeName, (object)exportedMethod.MemberName);
                        }
                    }
                }
            }
            return flag2;
        }

        private string ExtractPinvokeimpl(ParserStateValues state)
        {
            string data = state?.Method?.Result;
            if(String.IsNullOrWhiteSpace(data)) {
                return null;
            }

            const string sign = "pinvokeimpl";

            int op = data.IndexOf(sign, StringComparison.InvariantCulture);
            if(op == -1) {
                return null;
            }

            int ed = data.IndexOf(')', op);
            if(ed == -1) {
                throw new MissingMethodException($"Failed {sign} signature.");
            }

            return data.Substring(op, ed - op + 1);
        }

        private bool ExtractMethodParts(ParserStateValues state)
        {
            if(!GetPartBeforeParameters
                (
                    state.Method.Declaration, 
                    out string name, 
                    out string afterName, 
                    out string result, 
                    out string resultAttrs, 
                    out string attributes
                ))
            {
                return false;
            }

            state.Method.Name               = name;
            state.Method.After              = afterName;
            state.Method.Attributes         = attributes;
            state.Method.Result             = result;
            state.Method.ResultAttributes   = resultAttrs;
            state.Method.Pinvokeimpl        = ExtractPinvokeimpl(state);
            return true;
        }
    }
}

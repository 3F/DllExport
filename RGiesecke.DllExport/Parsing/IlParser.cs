// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.7.38850, Culture=neutral, PublicKeyToken=8f52d83c1a22df51
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RGiesecke.DllExport.Parsing.Actions;
using RGiesecke.DllExport.Properties;

namespace RGiesecke.DllExport.Parsing
{
    public sealed class IlParser: HasServiceProvider
    {
        [Localizable(false)]
        private static readonly string[] _DefaultMethodAttributes = new string[20]
        {
        "static",
        "public",
        "private",
        "family",
        "final",
        "specialname",
        "virtual",
        "abstract",
        "assembly",
        "famandassem",
        "famorassem",
        "privatescope",
        "hidebysig",
        "newslot",
        "strict",
        "rtspecialname",
        "flags",
        "unmanagedexp",
        "reqsecobj",
        "pinvokeimpl"
        };

        private HashSet<string> _MethodAttributes;

        public string DllExportAttributeAssemblyName
        {
            get {
                return this.Exports.DllExportAttributeAssemblyName;
            }
        }

        public string DllExportAttributeFullName
        {
            get {
                return this.Exports.DllExportAttributeFullName;
            }
        }

        public string DllExportAttributeIlAsmFullName
        {
            get {
                string str = this.Exports.DllExportAttributeFullName;
                if(!string.IsNullOrEmpty(str))
                {
                    int length = str.LastIndexOf('.');
                    if(length > -1)
                    {
                        str = "'" + str.Substring(0, length) + "'.'" + str.Substring(length + 1) + "'";
                    }
                    else
                    {
                        str = "'" + str + "'";
                    }
                }
                return str;
            }
        }

        public IInputValues InputValues
        {
            get;
            set;
        }

        public AssemblyExports Exports
        {
            get;
            set;
        }

        public string TempDirectory
        {
            get;
            set;
        }

        public bool ProfileActions
        {
            get;
            set;
        }

        public HashSet<string> MethodAttributes
        {
            get {
                lock(this)
                    return this._MethodAttributes ?? (this._MethodAttributes = this.GetMethodAttributes());
            }
        }

        public IlParser(IServiceProvider serviceProvider)
        : base(serviceProvider)
        {
        }

        public IEnumerable<string> GetLines(CpuPlatform cpu)
        {
            using(this.GetNotifier().CreateContextName((object)this, Resources.ParseILContextName))
            {
                Dictionary<ParserState, IParserStateAction> actionsByState = IlParser.ParserStateAction.GetActionsByState(this);
                List<string> stringList1 = new List<string>(1000000);
                ParserStateValues state = new ParserStateValues(cpu, (IList<string>)stringList1) {
                    State = ParserState.Normal
                };
                Stopwatch stopwatch1 = Stopwatch.StartNew();

                var dest = Path.Combine(TempDirectory, InputValues.FileName + ".il");
                using(var sreader = new StreamReader(new FileStream(dest, FileMode.Open), Encoding.Unicode))
                {
                    while(!sreader.EndOfStream) {
                        stringList1.Add(sreader.ReadLine());
                    }
                }

                Action<IParserStateAction, string> action1 = (Action<IParserStateAction, string>)((action, trimmedLine) => {
                    string name = action.GetType().Name;
                    using(this.GetNotifier().CreateContextName((object)action, name))
                        action.Execute(state, trimmedLine);
                });
                if(this.ProfileActions)
                {
                    Action<IParserStateAction, string> executeActionCore = action1;
                    action1 = (Action<IParserStateAction, string>)((action, trimmedLine) => {
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        executeActionCore(action, trimmedLine);
                        stopwatch.Stop();
                        action.Milliseconds += stopwatch.ElapsedMilliseconds;
                    });
                }
                Dictionary<string, int> usedScopeNames = new Dictionary<string, int>();
                for(int index = 0; index < stringList1.Count; ++index)
                {
                    state.InputPosition = index;
                    string str1 = stringList1[index];
                    IlParsingUtils.ParseIlSnippet(str1, ParsingDirection.Forward, (Func<IlParsingUtils.IlSnippetLocation, bool>)(current => {
                        if(!current.WithinString && (int)current.CurrentChar == 93 && (current.LastIdentifier != null && !usedScopeNames.ContainsKey(current.LastIdentifier)))
                        {
                            usedScopeNames.Add(current.LastIdentifier, usedScopeNames.Count);
                        }
                        return true;
                    }), (Action<IlParsingUtils.IlSnippetFinalizaton>)null);
                    string str2 = str1.NullSafeTrim();
                    state.AddLine = true;
                    IParserStateAction parserStateAction;
                    if(!actionsByState.TryGetValue(state.State, out parserStateAction))
                    {
                        this.GetNotifier().Notify(2, DllExportLogginCodes.NoParserActionError, Resources.No_action_for_parser_state_0_, (object)state.State);
                    }
                    else
                    {
                        action1(parserStateAction, str2);
                    }
                    if(state.AddLine)
                    {
                        state.Result.Add(str1);
                    }
                }
                List<string> stringList2 = state.Result;
                if(state.ExternalAssemlyDeclarations.Count > 0)
                {
                    stringList2 = new List<string>(state.Result.Count);
                    stringList2.AddRange((IEnumerable<string>)state.Result);
                    List<ExternalAssemlyDeclaration> assemlyDeclarationList = new List<ExternalAssemlyDeclaration>(state.ExternalAssemlyDeclarations.Count);
                    Dictionary<string, int> foundAliases = new Dictionary<string, int>();
                    foreach(string inputText in stringList2)
                    {
                        if(inputText.Length >= 3 && inputText.Contains("["))
                            IlParsingUtils.ParseIlSnippet(inputText, ParsingDirection.Forward, (Func<IlParsingUtils.IlSnippetLocation, bool>)(current => {
                                if(current.WithinScope && !current.WithinString && (current.LastIdentifier != null && !foundAliases.ContainsKey(current.LastIdentifier)))
                                {
                                    foundAliases.Add(current.LastIdentifier, foundAliases.Count);
                                }
                                return true;
                            }), (Action<IlParsingUtils.IlSnippetFinalizaton>)null);
                    }
                    foreach(ExternalAssemlyDeclaration assemlyDeclaration in (IEnumerable<ExternalAssemlyDeclaration>)state.ExternalAssemlyDeclarations)
                    {
                        if(!foundAliases.ContainsKey(assemlyDeclaration.AliasName))
                        {
                            assemlyDeclarationList.Add(assemlyDeclaration);
                        }
                    }
                    if(assemlyDeclarationList.Count > 0)
                    {
                        assemlyDeclarationList.Reverse();
                        foreach(ExternalAssemlyDeclaration assemlyDeclaration in assemlyDeclarationList)
                        {
                            int num1 = 0;
                            int num2 = -1;
                            for(int inputLineIndex = assemlyDeclaration.InputLineIndex; inputLineIndex < stringList2.Count; ++inputLineIndex)
                            {
                                string str = stringList2[inputLineIndex].TrimStart();
                                if(str == "{")
                                {
                                    ++num1;
                                }
                                else if(str == "}")
                                {
                                    if(num1 == 1)
                                    {
                                        num2 = inputLineIndex;
                                        break;
                                    }
                                    --num1;
                                }
                            }
                            if(num2 > -1)
                            {
                                this.GetNotifier().Notify(-2, DllExportLogginCodes.RemovingReferenceToDllExportAttributeAssembly, string.Format(Resources.Deleting_reference_to_0_, (object)assemlyDeclaration.AssemblyName, assemlyDeclaration.AliasName != assemlyDeclaration.AssemblyName ? (object)string.Format(Resources.AssemblyAlias, (object)assemlyDeclaration.AliasName) : (object)""));
                                stringList2.RemoveRange(assemlyDeclaration.InputLineIndex, num2 - assemlyDeclaration.InputLineIndex + 1);
                            }
                        }
                    }
                }
                stopwatch1.Stop();
                this.GetNotifier().Notify(-2, "EXPPERF02", Resources.Parsing_0_lines_of_IL_took_1_ms_, (object)stringList1.Count, (object)stopwatch1.ElapsedMilliseconds);
                if(this.ProfileActions)
                {
                    foreach(KeyValuePair<ParserState, IParserStateAction> keyValuePair in actionsByState)
                    {
                        this.GetNotifier().Notify(-1, "EXPPERF03", Resources.Parsing_action_0_took_1_ms, (object)keyValuePair.Key, (object)keyValuePair.Value.Milliseconds);
                    }
                }
                return (IEnumerable<string>)stringList2;
            }
        }

        internal IDllExportNotifier GetNotifier()
        {
            return this.ServiceProvider.GetService<IDllExportNotifier>();
        }

        private HashSet<string> GetMethodAttributes()
        {
            string str = (this.InputValues.MethodAttributes ?? "").Trim();
            object obj;
            if(!string.IsNullOrEmpty(str))
                obj = (object)((IEnumerable<string>)str.Split(new char[6]
            {
            ' ',
            ',',
            ';',
            '\t',
            '\n',
            '\r'
            }, StringSplitOptions.RemoveEmptyEntries)).Distinct<string>();
            else
            {
                obj = (object)IlParser._DefaultMethodAttributes;
            }
            return new HashSet<string>((IEnumerable<string>)obj);
        }

        private static string GetExePath(string toolFileName, string installPath, string settingsName)
        {
            string path = "";
            if(!string.IsNullOrEmpty(installPath))
            {
                path = Path.Combine(Path.GetFullPath(installPath), toolFileName);
                if(!File.Exists(path))
                {
                    path = Path.Combine(Path.Combine(Path.GetFullPath(installPath), "Bin"), toolFileName);
                }
            }

            if(string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                path = toolFileName;
            }
            return path;
        }

        private static IEnumerable<string> EnumerateStreamLines(StreamReader streamReader)
        {
            while(!streamReader.EndOfStream)
            {
                yield return streamReader.ReadLine();
            }
        }

        internal static int RunIlTool(string installPath, string toolFileName, string requiredPaths, string workingDirectory, string settingsName, string arguments, string toolLoggingCode, string verboseLoggingCode, IDllExportNotifier notifier, int timeout, Func<string, bool> suppressErrorOutputLine = null)
        {
            using(notifier.CreateContextName((object)null, toolFileName))
            {
                if(suppressErrorOutputLine != null)
                {
                    Func<string, bool> suppressErrorOutputLineCore = suppressErrorOutputLine;
                    suppressErrorOutputLine = (Func<string, bool>)(line => {
                        if(line != null)
                        {
                            return suppressErrorOutputLineCore(line);
                        }
                        return false;
                    });
                }
                else
                {
                    suppressErrorOutputLine = (Func<string, bool>)(l => false);
                }
                string fileName = Path.GetFileName(toolFileName);
                string exePath = IlParser.GetExePath(fileName, installPath, settingsName);
                string withoutExtension = Path.GetFileNameWithoutExtension(fileName);
                int num;
                using(Process process = new Process())
                {
                    notifier.Notify(-2, toolLoggingCode, Resources.calling_0_with_1_, (object)exePath, (object)arguments);
                    ProcessStartInfo processStartInfo = new ProcessStartInfo(exePath, arguments) {
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };
                    if(!string.IsNullOrEmpty(workingDirectory))
                    {
                        processStartInfo.WorkingDirectory = Path.GetFullPath(workingDirectory);
                    }
                    if(!string.IsNullOrEmpty(requiredPaths))
                    {
                        processStartInfo.EnvironmentVariables["PATH"] = requiredPaths.Trim(';') + ";" + (processStartInfo.EnvironmentVariables.ContainsKey("PATH") ? processStartInfo.EnvironmentVariables["PATH"] : (string)null);
                    }
                    process.StartInfo = processStartInfo;
                    process.Start();
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    StringBuilder stringBuilder1 = new StringBuilder();
                    StringBuilder stringBuilder2 = new StringBuilder();
                    Action<IEnumerable<string>, StringBuilder> appendLines = (Action<IEnumerable<string>, StringBuilder>)((lines, sb) => lines.Aggregate<string, StringBuilder>(sb, (Func<StringBuilder, string, StringBuilder>)((r, line) => r.AppendLine(line))));
                    Action<StreamReader, StringBuilder> action1 = (Action<StreamReader, StringBuilder>)((sr, sb) => appendLines(IlParser.EnumerateStreamLines(sr), sb));
                    Action<StreamReader, StringBuilder> action2 = (Action<StreamReader, StringBuilder>)((sr, sb) => appendLines(IlParser.EnumerateStreamLines(sr).Where<string>((Func<string, bool>)(line => !suppressErrorOutputLine(line))), sb));
                    while(stopwatch.ElapsedMilliseconds < (long)timeout && !process.HasExited)
                    {
                        action1(process.StandardOutput, stringBuilder1);
                        action2(process.StandardError, stringBuilder2);
                    }
                    bool hasExited = process.HasExited;
                    action1(process.StandardOutput, stringBuilder1);
                    action2(process.StandardError, stringBuilder2);
                    if(hasExited)
                    {
                        notifier.Notify(-2, toolLoggingCode, Resources.R_0_1_returned_gracefully, (object)withoutExtension, (object)exePath);
                        int exitCode = process.ExitCode;
                        if(exitCode != 0 || stringBuilder2.Length > 0)
                        {
                            throw new InvalidOperationException((stringBuilder2.Length > 0 ? (object)stringBuilder2 : (object)stringBuilder1).ToString());
                        }
                        if(stringBuilder2.Length > 0)
                        {
                            notifier.Notify(-3, verboseLoggingCode, stringBuilder2.ToString());
                        }
                        if(stringBuilder1.Length > 0)
                        {
                            notifier.Notify(-3, verboseLoggingCode, stringBuilder1.ToString());
                        }
                        num = exitCode;
                    }
                    else
                    {
                        bool flag = false;
                        Exception exception = (Exception)null;
                        try
                        {
                            process.Kill();
                            flag = true;
                        }
                        catch(Exception ex)
                        {
                            exception = ex;
                        }
                        if(flag)
                            throw new InvalidOperationException(string.Format((IFormatProvider)CultureInfo.InvariantCulture, Resources.R_0_did_not_return_after_1_ms, new object[2]
                        {
                        (object) withoutExtension,
                        (object) timeout
                        }));
                        throw new InvalidOperationException(string.Format((IFormatProvider)CultureInfo.InvariantCulture, Resources.R_0_did_not_return_after_1_ms_and_it_could_not_be_stopped, (object)withoutExtension, (object)timeout, (object)exception.Message));
                    }
                }
                return num;
            }
        }

        public abstract class ParserStateAction: IParserStateAction
        {
            protected string DllExportAttributeAssemblyName
            {
                get {
                    return this.Parser.DllExportAttributeAssemblyName;
                }
            }

            protected string DllExportAttributeFullName
            {
                get {
                    return this.Parser.DllExportAttributeFullName;
                }
            }

            protected IDllExportNotifier Notifier
            {
                get {
                    return this.Parser.GetNotifier();
                }
            }

            protected AssemblyExports Exports
            {
                get {
                    return this.Parser.Exports;
                }
            }

            public long Milliseconds
            {
                get;
                set;
            }

            public IlParser Parser
            {
                get;
                set;
            }

            public abstract void Execute(ParserStateValues state, string trimmedLine);

            protected void Notify(int severity, string code, string message, params object[] values)
            {
                this.Notify((ParserStateValues)null, severity, code, message, values);
            }

            protected void Notify(ParserStateValues stateValues, int severity, string code, string message, params object[] values)
            {
                SourceCodeRange range;
                if(stateValues != null && (range = stateValues.GetRange()) != null)
                {
                    this.Notifier.Notify(severity, code, range.FileName, new SourceCodePosition?(range.StartPosition), new SourceCodePosition?(range.EndPosition), message, values);
                }
                else
                {
                    this.Notifier.Notify(severity, code, message, values);
                }
            }

            protected bool ValidateExportNameAndLogError(ExportedMethod exportMethod, ParserStateValues stateValues)
            {
                bool flag;
                if(exportMethod == null)
                {
                    flag = false;
                }
                else if(exportMethod.ExportName != null && (exportMethod.ExportName.Contains("'") || Regex.IsMatch(exportMethod.ExportName, "\\P{IsBasicLatin}")))
                {
                    this.Notify(stateValues, 3, DllExportLogginCodes.ExportNamesHaveToBeBasicLatin, Resources.Export_name_0_on_1__2_is_Unicode_windows_export_names_have_to_be_basic_latin, (object)exportMethod.ExportName, (object)exportMethod.ExportedClass.FullTypeName, (object)exportMethod.MemberName);
                    flag = false;
                }
                else
                {
                    flag = true;
                }
                return flag;
            }

            public static Dictionary<ParserState, IParserStateAction> GetActionsByState(IlParser parser)
            {
                Dictionary<ParserState, IParserStateAction> dictionary = new Dictionary<ParserState, IParserStateAction>()
                {
                {
                    ParserState.ClassDeclaration,
                    (IParserStateAction) new ClassDeclarationParserAction()
                },
                {
                    ParserState.Class,
                    (IParserStateAction) new ClassParserAction()
                },
                {
                    ParserState.DeleteExportAttribute,
                    (IParserStateAction) new DeleteExportAttributeParserAction()
                },
                {
                    ParserState.MethodDeclaration,
                    (IParserStateAction) new MethodDeclarationParserAction()
                },
                {
                    ParserState.Method,
                    (IParserStateAction) new MethodParserAction()
                },
                {
                    ParserState.MethodProperties,
                    (IParserStateAction) new MethodPropertiesParserAction()
                },
                {
                    ParserState.Normal,
                    (IParserStateAction) new NormalParserAction()
                }
            };
                foreach(IParserStateAction parserStateAction in dictionary.Values)
                {
                    parserStateAction.Parser = parser;
                }
                return dictionary;
            }
        }
    }
}

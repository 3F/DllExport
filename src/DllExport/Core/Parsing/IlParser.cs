/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using net.r_eg.DllExport.ILAsm;
using net.r_eg.DllExport.Parsing.Actions;

namespace net.r_eg.DllExport.Parsing
{
    public sealed class IlParser(IServiceProvider serviceProvider): HasServiceProvider(serviceProvider)
    {
        [Localizable(false)]
        private static readonly string[] _DefaultMethodAttributes =
        [
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
        ];

        private HashSet<string> _MethodAttributes;

        public string DllExportAttributeAssemblyName => Exports.DllExportAttributeAssemblyName;

        public string DllExportAttributeFullName => Exports.DllExportAttributeFullName;

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

        public IInputValues InputValues { get; set; }

        public AssemblyExports Exports { get; set; }

        public string TempDirectory { get; set; }

        public bool ProfileActions { get; set; }

        public HashSet<string> MethodAttributes
        {
            get {
                lock(this)
                    return this._MethodAttributes ?? (this._MethodAttributes = this.GetMethodAttributes());
            }
        }

        public IEnumerable<string> GetLines(CpuPlatform cpu)
        {
            using IDisposable _ = GetNotifier().CreateContextName(context: this, Resources.ParseILContextName);
            
            Dictionary<ParserState, IParserStateAction> actionsByState = ParserStateAction.GetActionsByState(this);
            List<string> lines = new(1_000_000); // TODO: 

            ParserStateValues state = new(cpu, lines) { State = ParserState.Normal };
            Stopwatch stopwatch = Stopwatch.StartNew();

            string dst = Path.Combine(TempDirectory, InputValues.FileName + ".il");
            using(StreamReader sreader = new(new FileStream(dst, FileMode.Open), Encoding.Unicode))
            {
                while(!sreader.EndOfStream) lines.Add(sreader.ReadLine());
            }

            void _ExecAction(IParserStateAction action, string trimmed)
            {
                using(GetNotifier().CreateContextName(action, action.GetType().Name))
                    action.Execute(state, trimmed);
            }
            Action<IParserStateAction, string> _Execute = _ExecAction;

            if(ProfileActions)
            {
                _Execute = (action, trimmed) =>
                {
                    Stopwatch swCore = Stopwatch.StartNew();
                    _ExecAction(action, trimmed);
                    swCore.Stop();
                    action.Milliseconds += swCore.ElapsedMilliseconds;
                };
            }

            Dictionary<string, int> usedScopeNames = [];
            for(int idx = 0; idx < lines.Count; ++idx)
            {
                state.InputPosition = idx;
                string line = lines[idx];

                IlParsingUtils.ParseIlSnippet
                (
                    line,
                    ParsingDirection.Forward,
                    current =>
                    {
                        if(!current.WithinString && current.CurrentChar == ']'
                            && current.LastIdentifier != null && !usedScopeNames.ContainsKey(current.LastIdentifier))
                        {
                            usedScopeNames.Add(current.LastIdentifier, usedScopeNames.Count);
                        }
                        return true;
                    },
                    finalization: null
                );

                state.AddLine = true;

                if(!actionsByState.TryGetValue(state.State, out IParserStateAction parserStateAction))
                {
                    GetNotifier().Notify(2, DllExportLogginCodes.NoParserActionError, Resources.No_action_for_parser_state_0_, state.State);
                }
                else
                {
                    _Execute(parserStateAction, line?.Trim());
                }

                if(state.AddLine) state.Result.Add(line);
            }

            CleanExternalAssemlyDeclarations(state);

#if F_LEGACY_EMIT_MSCORLIB
            EmitMsCorlib(state);
#endif

            stopwatch.Stop();
            GetNotifier().Notify(-2, "EXPPERF02", Resources.Parsing_0_lines_of_IL_took_1_ms_, lines.Count, stopwatch.ElapsedMilliseconds);
            if(ProfileActions)
            {
                foreach(KeyValuePair<ParserState, IParserStateAction> keyValuePair in actionsByState)
                    GetNotifier().Notify(-1, "EXPPERF03", Resources.Parsing_action_0_took_1_ms, keyValuePair.Key, keyValuePair.Value.Milliseconds);
            }

            return state.Result;
        }

        internal IDllExportNotifier GetNotifier()
            => (IDllExportNotifier)ServiceProvider.GetService(typeof(IDllExportNotifier));

        private void CleanExternalAssemlyDeclarations(ParserStateValues state)
        {
            if(state.ExternalAssemlyDeclarations.Count < 1) return;

            List<ExternalAssemlyDeclaration> declarations = new(state.ExternalAssemlyDeclarations.Count);
            Dictionary<string, int> aliases = [];

            foreach(string line in state.Result)
            {
                if(line.Length >= 3 && line.Contains("["))
                    IlParsingUtils.ParseIlSnippet
                    (
                        line,
                        ParsingDirection.Forward,
                        current =>
                        {
                            if(current.WithinScope && !current.WithinString
                                && current.LastIdentifier != null && !aliases.ContainsKey(current.LastIdentifier))
                            {
                                aliases.Add(current.LastIdentifier, aliases.Count);
                            }
                            return true;
                        },
                        finalization: null
                    );
            }

            foreach(ExternalAssemlyDeclaration decl in state.ExternalAssemlyDeclarations)
            {
                if(!aliases.ContainsKey(decl.AliasName)) declarations.Add(decl);
            }

            if(declarations.Count < 1) return;

            declarations.Reverse();
            foreach(ExternalAssemlyDeclaration decl in declarations)
            {
                int bLeft = 0, bRight = -1;
                for(int idx = decl.InputLineIndex; idx < state.Result.Count; ++idx)
                {
                    string line = state.Result[idx].TrimStart();
                    if(line == "{")
                    {
                        ++bLeft;
                    }
                    else if(line == "}")
                    {
                        if(bLeft == 1) { bRight = idx; break; }
                        --bLeft;
                    }
                }

                if(bRight > -1)
                {
                    GetNotifier().Notify
                    (
                        -2,
                        DllExportLogginCodes.RemovingReferenceToDllExportAttributeAssembly,
                        string.Format
                        (
                            Resources.Deleting_reference_to_0_,
                            decl.AssemblyName,
                            decl.AliasName != decl.AssemblyName
                                ? string.Format(Resources.AssemblyAlias, decl.AliasName)
                                : string.Empty
                        )
                    );
                    state.Result.RemoveRange(decl.InputLineIndex, bRight - decl.InputLineIndex + 1);
                }
            }
        }

#if F_LEGACY_EMIT_MSCORLIB
        /// <summary>
        /// Read my note in https://github.com/3F/DllExport/issues/90
        /// 
        /// .assembly extern 'netstandard'
        /// ...               ^^^^^^^^^^^
        /// 
        /// .class public auto ansi beforefieldinit ...
        ///     extends[mscorlib] System.Object
        ///             ^^^^^^^^
        ///     ...
        ///     call instance void [mscorlib]System.Object::.ctor()
        ///                         ^^^^^^^^
        /// </summary>
        /// <param name="il"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private bool EmitMsCorlib(ParserStateValues state)
        {
            if(state.ExternalAssemlyDeclarations.Count < 1) return false;

            const string _EASM = "mscorlib";

            if(state.ExternalAssemlyDeclarations.Any(x => x.AssemblyName == _EASM))
            {
                return false;
            }

            state.Result.InsertRange
            (
                state.ExternalAssemlyDeclarations[0].InputLineIndex, 
                [
                    $".assembly extern '{_EASM}'",
                    "{",
                    "  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 ) ",
                    "  .ver 4:0:0:0",
                    "}"
                ]
            );

            return true;
        }
#endif // F_LEGACY_EMIT_MSCORLIB

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
                AssemblyParserAction assemblyParser = new(parser.InputValues);
                Dictionary<ParserState, IParserStateAction> dictionary = new()
                {
                    { ParserState.ClassDeclaration, new ClassDeclarationParserAction() },
                    { ParserState.Class, new ClassParserAction() },
                    { ParserState.DeleteExportAttribute, new DeleteExportAttributeParserAction() },
                    { ParserState.MethodDeclaration, new MethodDeclarationParserAction() },
                    { ParserState.Method, new MethodParserAction() },
                    { ParserState.MethodProperties, new MethodPropertiesParserAction() },
                    { ParserState.Normal, new NormalParserAction(parser.InputValues) },
                    { ParserState.AssemblyDeclaration, assemblyParser },
                    { ParserState.Assembly, assemblyParser },
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

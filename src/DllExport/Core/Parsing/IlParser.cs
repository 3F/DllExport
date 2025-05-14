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
using net.r_eg.Conari.Extension;
using net.r_eg.DllExport.Extensions;
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

        private static readonly ExternalAssemlyDeclaration _AsmExternDllExport = new("DllExport", "83 37 22 4C 9A D9 E3 56");

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

        internal static int RunIlTool
        (
            string installPath,
            string toolFileName,
            string requiredPaths,
            string workingDirectory,
            string arguments,
            string toolLoggingCode,
            string verboseLoggingCode,
            IDllExportNotifier notifier,
            int timeout,
            Func<string, bool> suppressErrorOutputLine = null
        )
        {
            using IDisposable context = notifier.CreateContextName(context: null, toolFileName);

            string fileName = Path.GetFileName(toolFileName);
            string exePath = GetExePath(fileName, installPath);
            string withoutExtension = Path.GetFileNameWithoutExtension(fileName);

            notifier.NotifyLow(toolLoggingCode, Resources.calling_0_with_1_, exePath, arguments);
            ProcessStartInfo processStartInfo = new(exePath, arguments)
            {
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
                processStartInfo.EnvironmentVariables["PATH"]
                    = requiredPaths.Trim(';') + ";" + processStartInfo.EnvironmentVariables.GetOrNull("PATH");
            }

            using Process process = new();
            process.StartInfo = processStartInfo;
            process.Start();

            StringBuilder sbStdout = new(), sbStderr = new();

            Stopwatch stopwatch = Stopwatch.StartNew();
            do
            {
                EnumerateStreamLines(process.StandardOutput).ForEach(l =>
                {
                    if(Environment.GetEnvironmentVariable("DllExportSupressFindWarnInLine") == null // TODO: option for .targets
                        && FindWarnInLine(l))
                    {
                        notifier.NotifyWarn(toolLoggingCode, l);
                    }
                    sbStdout.AppendLine(l);
                });

                EnumerateStreamLines(process.StandardError)
                    .Where(line => line != null && suppressErrorOutputLine?.Invoke(line) != true)
                    .ForEach(l => sbStderr.AppendLine(l));
            }
            while(!process.HasExited && stopwatch.ElapsedMilliseconds < timeout);

            if(process.HasExited)
            {
                notifier.NotifyLow(toolLoggingCode, Resources.R_0_1_returned_gracefully, withoutExtension, exePath);

                if(process.ExitCode != 0 || sbStderr.Length > 0)
                {
                    sbStderr.AppendLine(nameof(sbStdout)).Append(sbStdout.ToString());
                    throw new InvalidOperationException(sbStderr.ToString());
                }

                if(sbStdout.Length > 0)
                {
                    notifier.NotifyLow(verboseLoggingCode, sbStdout.ToString());
                }

                return process.ExitCode;
            }

            string exmsg;
            try
            {
                process.Kill();
                exmsg = string.Format(CultureInfo.InvariantCulture, Resources.R_0_did_not_return_after_1_ms, withoutExtension, timeout);
            }
            catch(Exception ex)
            {
                exmsg = string.Format(CultureInfo.InvariantCulture, Resources.R_0_did_not_return_after_1_ms_and_it_could_not_be_stopped, withoutExtension, timeout, ex.Message);
            }
            throw new InvalidOperationException(exmsg);
        }

        internal IDllExportNotifier GetNotifier()
            => (IDllExportNotifier)ServiceProvider.GetService(typeof(IDllExportNotifier));

        private void CleanExternalAssemlyDeclarations(ParserStateValues state)
        {
            if(state.ExternalAssemlyDeclarations.Count < 1) return;
            List<ExternalAssemlyDeclaration> unused = new(state.ExternalAssemlyDeclarations.Count);

#if F_ORIGIN_DEL_UNUSED_ASM_EXTERN // F-320

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
                if(!aliases.ContainsKey(decl.AliasName)) unused.Add(decl);
            }

            if(unused.Count < 1) return;
            unused.Reverse();

#else
            foreach(var ead in state.ExternalAssemlyDeclarations.Reverse<ExternalAssemlyDeclaration>())
            {
                if(ead == _AsmExternDllExport) unused.Add(ead); // possibly several
                //...
            }
#endif
            foreach(ExternalAssemlyDeclaration decl in unused)
            {
                if(decl.InputLineIndex < 0) continue;

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

        private static string GetExePath(string toolFileName, string installPath)
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

        private static bool FindWarnInLine(string line)
        {
            if(string.IsNullOrEmpty(line)) return false;

            const string _warn = "WARNING";
            const string _w1 = _warn + ":";
            const string _w2 = _warn + " ";

            return line.TrimStart().Substring(0, _w1.Length).ToUpper() is _w1 or _w2;
        }
    }
}

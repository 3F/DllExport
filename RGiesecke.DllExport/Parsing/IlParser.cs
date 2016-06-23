// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.2.23706, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using RGiesecke.DllExport.Parsing.Actions;
using RGiesecke.DllExport.Properties;

namespace RGiesecke.DllExport.Parsing
{
    public sealed class IlParser: IDllExportNotifier, IDisposable
    {
        private readonly DllExportNotifier _Notifier = new DllExportNotifier();

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

        public event EventHandler<DllExportNotificationEventArgs> Notification
        {
            add {
                this._Notifier.Notification += value;
            }
            remove {
                this._Notifier.Notification -= value;
            }
        }

        public void Dispose()
        {
            this._Notifier.Dispose();
        }

        public IEnumerable<string> GetLines(CpuPlatform cpu)
        {
            Dictionary<ParserState, IParserStateAction> actionsByState = IlParser.ParserStateAction.GetActionsByState(this);
            List<string> stringList = new List<string>(1000000);
            ParserStateValues state = new ParserStateValues(cpu, IlParser.GetClassDeclareRegex(RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace), IlParser.GetMethodDeclareRegex(RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace), (IList<string>)stringList) {
                State = ParserState.Normal
            };
            Stopwatch stopwatch1 = Stopwatch.StartNew();
            using(FileStream fileStream = new FileStream(Path.Combine(this.TempDirectory, this.InputValues.FileName + ".il"), FileMode.Open))
            {
                using(StreamReader streamReader = new StreamReader((Stream)fileStream, Encoding.Unicode))
                {
                    while(!streamReader.EndOfStream)
                    {
                        stringList.Add(streamReader.ReadLine());
                    }
                }
            }
            Action<IParserStateAction, string> action1 = (Action<IParserStateAction, string>)((action, trimmedLine) => action.Execute(state, trimmedLine));
            if(this.ProfileActions)
            {
                Action<IParserStateAction, string> executeActionCore = action1;
                action1 = (Action<IParserStateAction, string>)((action, trimmedLine) => {
                    Stopwatch stopwatch2 = Stopwatch.StartNew();
                    executeActionCore(action, trimmedLine);
                    stopwatch2.Stop();
                    action.Milliseconds += stopwatch2.ElapsedMilliseconds;
                });
            }
            for(int index = 0; index < stringList.Count; ++index)
            {
                state.InputPosition = index;
                string input = stringList[index];
                string str = input.NullSafeTrim();
                state.AddLine = true;
                IParserStateAction parserStateAction;
                if(!actionsByState.TryGetValue(state.State, out parserStateAction))
                {
                    this._Notifier.Notify(2, "EXP0007", Resources.No_action_for_parser_state_0_, (object)state.State);
                }
                else
                {
                    action1(parserStateAction, str);
                }
                if(state.AddLine)
                {
                    state.Result.Add(input);
                }
            }
            stopwatch1.Stop();
            this._Notifier.Notify(-1, "EXPPERF02", Resources.Parsing_0_lines_of_IL_took_1_ms_, (object)stringList.Count, (object)stopwatch1.ElapsedMilliseconds);
            if(this.ProfileActions)
            {
                foreach(KeyValuePair<ParserState, IParserStateAction> keyValuePair in actionsByState)
                {
                    this._Notifier.Notify(-1, "EXPPERF03", Resources.Parsing_action_0_took_1_ms, (object)keyValuePair.Key, (object)keyValuePair.Value.Milliseconds);
                }
            }
            return (IEnumerable<string>)state.Result;
        }

        private static Regex GetClassDeclareRegex(RegexOptions regexOptions)
        {
            return new Regex(Regexes.TypeDeclaration, regexOptions);
        }

        private static Regex GetMethodDeclareRegex(RegexOptions regexOptions)
        {
            return new Regex(Regexes.MethodDeclaration.Replace("{methodAttributes}", string.Join(")|(?:", new string[19] { "static", "public", "private", "family", "final", "specialname", "virtual", "abstract", "assembly", "famandassem", "famorassem", "privatescope", "hidebysig", "newslot", "strict", "rtspecialname", "unmanagedexp", "reqsecobj", "pinvokeimpl" })), regexOptions);
        }

        internal static string GetExePath(string toolFileName, string installPath, string settingsName)
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
            else if(!string.IsNullOrEmpty((Settings.Default[settingsName] as string).NullSafeTrim()))
            {
                path = Settings.Default.ILDasmPath;
            }
            if(string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                path = toolFileName;
            }
            return path;
        }

        internal static int RunIlTool(string installPath, string toolFileName, string requiredPaths, string workingDirectory, string settingsName, string arguments, string toolLoggingCode, string verboseLoggingCode, DllExportNotifier notifier, int timeout)
        {
            string fileName = Path.GetFileName(toolFileName);
            string exePath = IlParser.GetExePath(fileName, installPath, settingsName);
            string withoutExtension = Path.GetFileNameWithoutExtension(fileName);
            using(Process process = new Process())
            {
                notifier.Notify(0, toolLoggingCode, Resources.calling_0_with_1_, (object)exePath, (object)arguments);
                ProcessStartInfo processStartInfo = new ProcessStartInfo(exePath, arguments) {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
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
                StringBuilder stringBuilder = new StringBuilder();
                while(stopwatch.ElapsedMilliseconds < (long)timeout && !process.HasExited)
                {
                    while(!process.StandardOutput.EndOfStream)
                    {
                        string str = process.StandardOutput.ReadLine();
                        stringBuilder.AppendLine(str);
                    }
                }
                bool hasExited = process.HasExited;
                while(!process.StandardOutput.EndOfStream)
                {
                    string str = process.StandardOutput.ReadLine();
                    stringBuilder.AppendLine(str);
                }
                if(hasExited)
                {
                    notifier.Notify(0, toolLoggingCode, Resources.R_0_1_returned_gracefully, (object)withoutExtension, (object)exePath);
                    int exitCode = process.ExitCode;
                    if(exitCode != 0)
                    {
                        throw new InvalidOperationException(stringBuilder.ToString());
                    }
                    notifier.Notify(-2, verboseLoggingCode, stringBuilder.ToString());
                    return exitCode;
                }
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
                    throw new InvalidOperationException(string.Format((IFormatProvider)CultureInfo.InvariantCulture, Resources.R_0_did_not_return_after_1_ms, new object[2] { (object)withoutExtension, (object)timeout }));
                throw new InvalidOperationException(string.Format((IFormatProvider)CultureInfo.InvariantCulture, Resources.R_0_did_not_return_after_1_ms_and_it_could_not_be_stopped, (object)withoutExtension, (object)timeout, (object)exception.Message));
            }
        }

        public abstract class ParserStateAction: IParserStateAction
        {
            private static readonly Regex LineNumberRegex = new Regex(Regexes.LineNumbers);

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

            protected DllExportNotifier Notifier
            {
                get {
                    return this.Parser._Notifier;
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

            protected static bool TryGetLineNumbers(ParserStateValues state, out string fileName, out SourceCodePosition startPosition, out SourceCodePosition endPosition)
            {
                Regex regex = IlParser.ParserStateAction.LineNumberRegex;
                fileName = (string)null;
                startPosition = new SourceCodePosition();
                endPosition = new SourceCodePosition();
                for(int inputPosition = state.InputPosition; inputPosition < state.InputLines.Count; ++inputPosition)
                {
                    string input1 = state.InputLines[inputPosition];
                    if(input1.NullSafeCall<string, bool>((Func<string, bool>)(l => l.Trim().StartsWith(".line", StringComparison.Ordinal))))
                    {
                        Match match1 = regex.Match(input1);
                        if(match1.Success)
                        {
                            Match matchCopy = match1;
                            Func<int, int> func = (Func<int, int>)(g => {
                                if(!matchCopy.Groups[g].Success)
                                {
                                    return 0;
                                }
                                return int.Parse(matchCopy.Groups[g].Value, (IFormatProvider)CultureInfo.InvariantCulture);
                            });
                            startPosition = new SourceCodePosition(func(1), func(3));
                            endPosition = new SourceCodePosition(func(2), func(4));
                            fileName = match1.Groups[5].Success ? match1.Groups[5].Value : (string)null;
                            while(inputPosition >= 0 && string.IsNullOrEmpty(fileName))
                            {
                                --inputPosition;
                                string input2 = state.InputLines[inputPosition];
                                Match match2 = regex.Match(input2);
                                fileName = match2.Groups[5].Success ? match2.Groups[5].Value : (string)null;
                            }
                            return !string.IsNullOrEmpty(fileName);
                        }
                    }
                }
                return false;
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
                    string fileName;
                    SourceCodePosition startPosition;
                    SourceCodePosition endPosition;
                    if(IlParser.ParserStateAction.TryGetLineNumbers(stateValues, out fileName, out startPosition, out endPosition))
                    {
                        this.Notifier.Notify(3, "EXP00011", fileName, new SourceCodePosition?(startPosition), new SourceCodePosition?(endPosition), Resources.Export_name_0_on_1__2_is_Unicode_windows_export_names_have_to_be_basic_latin, (object)exportMethod.ExportName, (object)exportMethod.ExportedClass.FullTypeName, (object)exportMethod.MemberName);
                    }
                    else
                    {
                        this.Notifier.Notify(3, "EXP00011", Resources.Export_name_0_on_1__2_is_Unicode_windows_export_names_have_to_be_basic_latin, (object)exportMethod.ExportName, (object)exportMethod.ExportedClass.FullTypeName, (object)exportMethod.MemberName);
                    }
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
                Dictionary<ParserState, IParserStateAction> dictionary = new Dictionary<ParserState, IParserStateAction>() { { ParserState.ClassDeclaration, (IParserStateAction) new ClassDeclarationParserAction() }, { ParserState.Class, (IParserStateAction) new ClassParserAction() }, { ParserState.DeleteExportAttribute, (IParserStateAction) new DeleteExportAttributeParserAction() }, { ParserState.DeleteExportDependency, (IParserStateAction) new DeleteExportDependencyParserAction() }, { ParserState.MethodDeclaration, (IParserStateAction) new MethodDeclarationParserAction() }, { ParserState.Method, (IParserStateAction) new MethodParserAction() }, { ParserState.MethodProperties, (IParserStateAction) new MethodPropertiesParserAction() }, { ParserState.Normal, (IParserStateAction) new NormalParserAction() }
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

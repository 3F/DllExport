/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Win32;
using net.r_eg.DllExport.Extensions;
using net.r_eg.DllExport.ILAsm;
using net.r_eg.MvsSln;

namespace net.r_eg.DllExport.Activator
{
    using TCallbackTool = Func<TfmIdentifier, Version, string, string>;

    public class ExportTaskImplementation<TTask>: IInputValues, IServiceContainer, IServiceProvider where TTask : IDllExportTask, ITask
    {
        private const string ToolLocationHelperTypeName = "Microsoft.Build.Utilities.ToolLocationHelper";
        private const string UndefinedPropertyValue = PropertyNames.UNDEFINED;

        private static readonly Version _VersionUsingToolLocationHelper = new(4, 5);
        private static readonly IDictionary<string, TCallbackTool> _GetFrameworkToolPathByMethodName = new Dictionary<string, TCallbackTool>();
        private static readonly MethodInfo WrapGetToolPathCallMethodInfo = Utilities.GetMethodInfo(() => WrapGetToolPathCall<int>(default)).GetGenericMethodDefinition();
        private static readonly Regex MsbuildTaskLibRegex = new
        (
            "(?<=^|\\\\)Microsoft\\.Build\\.Utilities\\.v(\\d+(?:\\.(\\d+))?)(?:\\.dll)?$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant
        );
        
        private readonly IServiceContainer _ServiceProvider = new ServiceContainer();
        private readonly Dictionary<object, string> _LoggedMessages = [];
        private readonly TTask _ActualTask;
        private readonly IInputValues _Values;

        private int errorCount;

        public string MethodAttributes
        {
            get => _Values.MethodAttributes;
            set => _Values.MethodAttributes = value;
        }

        public CpuPlatform Cpu
        {
            get => _Values.Cpu;
            set => _Values.Cpu = value;
        }

        public string TargetFrameworkVersion { get; set; }

        public string TargetFrameworkIdentifier { get; set; }

        public bool? SkipOnAnyCpu { get; set; }

        public DebugType EmitDebugSymbols
        {
            get => _Values.EmitDebugSymbols;
            set => _Values.EmitDebugSymbols = value;
        }

        public string LeaveIntermediateFiles
        {
            get => _Values.LeaveIntermediateFiles;
            set => _Values.LeaveIntermediateFiles = value;
        }

        public string FileName
        {
            get => _Values.FileName;
            set => _Values.FileName = value;
        }

        [Required]
        public string FrameworkPath
        {
            get => _Values.FrameworkPath;
            set => _Values.FrameworkPath = value;
        }

        public string VsDevCmd
        {
            get => _Values.VsDevCmd;
            set => _Values.VsDevCmd = value;
        }

        public string VcVarsAll
        {
            get => _Values.VcVarsAll;
            set => _Values.VcVarsAll = value;
        }

        public string LibToolPath
        {
            get => _Values.LibToolPath;
            set => _Values.LibToolPath = value;
        }

        public string LibToolDllPath
        {
            get => _Values.LibToolDllPath;
            set => _Values.LibToolDllPath = value;
        }

        [Required]
        public string InputFileName
        {
            get => _Values.InputFileName;
            set => _Values.InputFileName = value;
        }

        public string KeyContainer
        {
            get => _Values.KeyContainer;
            set => _Values.KeyContainer = value;
        }

        public string KeyFile
        {
            get => _Values.KeyFile;
            set => _Values.KeyFile = value;
        }

        public string OutputFileName
        {
            get => _Values.OutputFileName;
            set => _Values.OutputFileName = value;
        }

        public string RootDirectory
        {
            get => _Values.RootDirectory;
            set => _Values.RootDirectory = value;
        }

        public string SdkPath
        {
            get => _Values.SdkPath;
            set => _Values.SdkPath = value;
        }

        public long ImageBase
        {
            get => _Values.ImageBase;
            set => _Values.ImageBase = value;
        }

        public int OrdinalsBase
        {
            get => _Values.OrdinalsBase;
            set => _Values.OrdinalsBase = value;
        }

        public bool GenExpLib
        {
            get => _Values.GenExpLib;
            set => _Values.GenExpLib = value;
        }

        public string OurILAsmPath
        {
            get => _Values.OurILAsmPath;
            set => _Values.OurILAsmPath = value;
        }

        public bool IsILAsmDefault => _Values.IsILAsmDefault;

        public bool SysObjRebase
        {
            get => _Values.SysObjRebase;
            set => _Values.SysObjRebase = value;
        }

        public string ProcEnv
        {
            get => _Values.ProcEnv;
            set => _Values.ProcEnv = value;
        }

        public string MetaLib
        {
            get => _Values.MetaLib;
            set => _Values.MetaLib = value;
        }

        public PatchesType Patches
        {
            get => _Values.Patches;
            set => _Values.Patches = value;
        }

        public PeCheckType PeCheck
        {
            get => _Values.PeCheck;
            set => _Values.PeCheck = value;
        }

        public List<AssemblyExternDirective> AssemblyExternDirectives
        {
            get => _Values.AssemblyExternDirectives;
            set => _Values.AssemblyExternDirectives = value;
        }

        public List<TypeRefDirective> TypeRefDirectives
        {
            get => _Values.TypeRefDirectives;
            set => _Values.TypeRefDirectives = value;
        }

        public string DllExportAttributeFullName
        {
            get => _Values.DllExportAttributeFullName;
            set => _Values.DllExportAttributeFullName = value;
        }

        public string DllExportAttributeAssemblyName
        {
            get => _Values.DllExportAttributeAssemblyName;
            set => _Values.DllExportAttributeAssemblyName = value;
        }

        public string Platform { get; set; }

        public string CpuType { get; set; }

        public string ProjectDirectory
        {
            get => _Values.RootDirectory;
            set => _Values.RootDirectory = value;
        }

        public string AssemblyKeyContainerName
        {
            get => _Values.KeyContainer;
            set => _Values.KeyContainer = value;
        }

        public int Timeout { get; set; } = 45_000;

        private TCallbackTool GetFrameworkToolPath => GetGetToolPath("GetPathToDotNetFrameworkFile");

        private TCallbackTool GetSdkToolPath => GetGetToolPath("GetPathToDotNetFrameworkSdkFile");

        static ExportTaskImplementation() => AssemblyLoadingRedirection.EnsureSetup();

        public ExportTaskImplementation(TTask actualTask)
        {
            _ActualTask = actualTask;

            AddService
            (
                typeof(IDllExportNotifier), 
                (sp, t) => new DllExportNotifierWithTask(_ActualTask)
            );

            _Values = new InputValuesCore(this);
            GetNotifier().Notification += OnDllWrapperNotification;
        }

        public object GetService(Type serviceType) => _ServiceProvider.GetService(serviceType);

        public void AddService(Type serviceType, object serviceInstance)
        {
            _ServiceProvider.AddService(serviceType, serviceInstance);
        }

        public void AddService(Type serviceType, object serviceInstance, bool promote)
        {
            _ServiceProvider.AddService(serviceType, serviceInstance, promote);
        }

        public void AddService(Type serviceType, ServiceCreatorCallback callback)
        {
            _ServiceProvider.AddService(serviceType, callback);
        }

        public void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
        {
            _ServiceProvider.AddService(serviceType, callback, promote);
        }

        public void RemoveService(Type serviceType)
        {
            _ServiceProvider.RemoveService(serviceType);
        }

        public void RemoveService(Type serviceType, bool promote)
        {
            _ServiceProvider.RemoveService(serviceType, promote);
        }

        public IDllExportNotifier GetNotifier()
        {
            return (IDllExportNotifier)GetService(typeof(IDllExportNotifier));
        }

        public void Notify(int severity, string code, string message, params object[] values)
        {
            GetNotifier().Notify(severity, code, message, values);
        }

        public void Notify(int severity, string code, string fileName, SourceCodePosition? startPosition, SourceCodePosition? endPosition, string message, params object[] values)
        {
            GetNotifier().Notify(severity, code, fileName, startPosition, endPosition, message, values);
        }

        [CLSCompliant(false)]
        public AssemblyBinaryProperties InferAssemblyBinaryProperties() => _Values.InferAssemblyBinaryProperties();

        public void InferOutputFile() => _Values.InferOutputFile();

        public bool Execute()
        {
            errorCount = 0;
            if(ValidateInputValues())
            {
                this._Values.Cpu = Utilities.ToCpuPlatform(this.Platform);
                try
                {
                    bool? skipOnAnyCpu = this.SkipOnAnyCpu;
                    if((skipOnAnyCpu.HasValue ? (skipOnAnyCpu.GetValueOrDefault() ? 1 : 0) : 1) != 0 && this._Values.Cpu == CpuPlatform.AnyCpu)
                    {
                        this._ActualTask.Log.LogMessage(Resources.Skipped_Method_Exports);
                        return true;
                    }
                    AssemblyBinaryProperties binaryProperties = this._Values.InferAssemblyBinaryProperties();
                    if(string.IsNullOrEmpty(this.KeyFile) && !string.IsNullOrEmpty(binaryProperties.KeyFileName))
                    {
                        this.KeyFile = binaryProperties.KeyFileName;
                    }
                    if(string.IsNullOrEmpty(this.KeyContainer) && !string.IsNullOrEmpty(binaryProperties.KeyContainer))
                    {
                        this.KeyContainer = binaryProperties.KeyContainer;
                    }
                    this._Values.InferOutputFile();
                    this.ValidateKeyFiles(binaryProperties.IsSigned);

                    using(var dllExportWeaver = new DllExportWeaver(this) { Timeout = Timeout })
                    {
                        dllExportWeaver.InputValues = _ActualTask;
                        dllExportWeaver.Run();
                    }
                    return ExecutePostProc(errorCount == 0, _ActualTask.Log);
                }
                catch(Exception ex)
                {
                    _ActualTask.Log.LogErrorFromException(ex);
                    _ActualTask.Log.LogMessage(ex.StackTrace);
                    ProblemSolver(ex);
                }
                this._LoggedMessages.Clear();
            }
            return false;
        }

        // https://github.com/3F/DllExport/pull/148
        private bool ExecutePostProc(bool ret, TaskLoggingHelper log)
        {
            if(!ret || string.IsNullOrEmpty(ProcEnv))
            {
                log.LogMessage(Resources._0_is_ignored_due_to_1, nameof(PostProc), $"!{ret} || {nameof(ProcEnv)} == null");
                return ret;
            }

            using(var postproc = new PostProc(ProcEnv, log))
            {
                return postproc.Process(new Executor(log));
            }
        }

        private void OnDllWrapperNotification(object sender, DllExportNotificationEventArgs e)
        {
            MessageImportance messageImportance = ExportTaskImplementation<TTask>.GetMessageImportance(e.Severity);
            string file = string.IsNullOrEmpty(e.FileName) ? this._ActualTask.BuildEngine.ProjectFileOfTaskNode : e.FileName;
            SourceCodePosition sourceCodePosition1 = e.StartPosition ?? new SourceCodePosition(0, 0);
            SourceCodePosition sourceCodePosition2 = e.EndPosition ?? new SourceCodePosition(0, 0);
            string message = e.Message;
            if(e.Severity > 0 && e.Context != (NotificationContext)null && e.Context.Name != null)
            {
                message = e.Context.Name + ": " + message;
            }
            var data = new {
                startPos = sourceCodePosition1,
                endPos = sourceCodePosition2,
                fileName = file,
                Severity = e.Severity,
                Code = e.Code,
                Message = message
            };
            if(this._LoggedMessages.ContainsKey((object)data))
            {
                return;
            }
            this._LoggedMessages.Add((object)data, message);
            if(e.Severity == 1)
            {
                this._ActualTask.Log.LogWarning("Export", e.Code, (string)null, file, sourceCodePosition1.Line, sourceCodePosition1.Character, sourceCodePosition2.Line, sourceCodePosition2.Character, message);
            }
            else if(e.Severity > 1)
            {
                ++errorCount;
                this._ActualTask.Log.LogError("Export", e.Code, (string)null, file, sourceCodePosition1.Line, sourceCodePosition1.Character, sourceCodePosition2.Line, sourceCodePosition2.Character, message);
            }
            else
            {
                this._ActualTask.Log.LogMessage(messageImportance, message, new object[0]);
            }
        }

        private static MessageImportance GetMessageImportance(int severity)
        {
            MessageImportance messageImportance = MessageImportance.Normal;
            if(severity < -1)
            {
                messageImportance = MessageImportance.Low;
            }
            else if(severity == 0)
            {
                messageImportance = MessageImportance.High;
            }
            return messageImportance;
        }

        private bool ValidateInputValues()
        {
            VsDevCmd    = ValidateCmdScript(VsDevCmd);
            VcVarsAll   = ValidateCmdScript(VcVarsAll);

            ValidateLibToolPath();
            bool flag = ValidateFrameworkPath() & ValidateSdkPath();

            // TODO: redundant cfg
            if(!string.IsNullOrEmpty(CpuType) && 
                (string.IsNullOrEmpty(Platform) || string.Equals(Platform, "anycpu", StringComparison.OrdinalIgnoreCase)))
            {
                Platform = CpuType;
            }

            return flag & ValidateFileName();
        }

        private void ValidateKeyFiles(bool isSigned)
        {
            if(isSigned && string.IsNullOrEmpty(this.KeyContainer) && string.IsNullOrEmpty(this.KeyFile))
            {
                this._ActualTask.Log.LogWarning(Resources.Output_assembly_was_signed_however_neither_keyfile_nor_keycontainer_could_be_inferred__Reading_those_values_from_assembly_attributes_is_not__yet__supported__they_have_to_be_defined_inside_the_MSBuild_project_file);
            }
            if(string.IsNullOrEmpty(this._Values.KeyContainer) || string.IsNullOrEmpty(this._Values.KeyFile))
            {
                return;
            }
            this._ActualTask.Log.LogError(Resources.Both_key_values_KeyContainer_0_and_KeyFile_0_are_present_only_one_can_be_specified, (object)this._Values.KeyContainer, (object)this._Values.KeyFile);
        }

        private TCallbackTool GetGetToolPath(string methodName)
        {
            lock(ExportTaskImplementation<TTask>._GetFrameworkToolPathByMethodName)
            {
                TCallbackTool local_0;
                if(!ExportTaskImplementation<TTask>._GetFrameworkToolPathByMethodName.TryGetValue(methodName, out local_0))
                {
                    ExportTaskImplementation<TTask>._GetFrameworkToolPathByMethodName.Add(methodName, local_0 = this.GetGetToolPathInternal(methodName));
                }
                return local_0;
            }
        }

        private static Func<string, int, string> WrapGetToolPathCall<TTargetDotNetFrameworkVersion>(MethodInfo methodInfo)
        {
            Func<string, TTargetDotNetFrameworkVersion, string> actualCall = (Func<string, TTargetDotNetFrameworkVersion, string>)Delegate.CreateDelegate(typeof(Func<string, TTargetDotNetFrameworkVersion, string>), methodInfo);
            return (Func<string, int, string>)((fileName, versionValue) => {
                TTargetDotNetFrameworkVersion frameworkVersion = (TTargetDotNetFrameworkVersion)Enum.ToObject(typeof(TTargetDotNetFrameworkVersion), versionValue);
                return actualCall(fileName, frameworkVersion);
            });
        }

        private TCallbackTool GetGetToolPathInternal(string methodName)
        {
            Type type = this.GetToolLocationHelperTypeFromRegsitry();
            if(type == null)
            {
                type = Type.GetType("Microsoft.Build.Utilities.ToolLocationHelper") ?? Type.GetType("Microsoft.Build.Utilities.ToolLocationHelper, Microsoft.Build.Utilities.v4.0, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
                if(type == null)
                {
                    Assembly assembly;
                    try
                    {
                        assembly = Assembly.Load("Microsoft.Build.Utilities.v4.0, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
                    }
                    catch
                    {
                        assembly = null;
                    }

                    if(assembly != null)
                    {
                        type = assembly.GetType("Microsoft.Build.Utilities.ToolLocationHelper");
                    }
                }
            }

            if(type == null) {
                return null;
            }

            Type targetDotNetFrameworkVersionType = type.Assembly.GetType("Microsoft.Build.Utilities.TargetDotNetFrameworkVersion");
            MethodInfo method = type.GetMethod(
                methodName, 
                BindingFlags.Static | BindingFlags.Public, 
                null, 
                new Type[2]
                {
                    typeof(string),
                    targetDotNetFrameworkVersionType
                }, 
                null
            );

            if(method == null) {
                return null;
            }

            var getToolPathCore = (Func<string, int, string>)WrapGetToolPathCallMethodInfo
                                                            .MakeGenericMethod(targetDotNetFrameworkVersionType)
                                                            .Invoke(null, new object[] { method });

            string getToolPath(string n, int v)
            {
                try
                {
                    return getToolPathCore(n, v);
                }
                catch
                {
                    return null;
                }
            }

            int getNum(string version)
            {
                return (int)Enum.Parse(targetDotNetFrameworkVersionType, version);
            }

            const string _V_LATEST = "Latest";

            return (ident, version, toolName) => 
            {
                int num;

                // TargetDotNetFrameworkVersion Enumeration: https://msdn.microsoft.com/en-us/library/ms126273.aspx
                try
                {
                    num = getNum
                    (
                        ident == TfmIdentifier.NETFramework
                        ? $"{nameof(Version)}{version.Major}{version.Minor}"
                        : nameof(Version) + _V_LATEST // latest released for current env
                    );
                }
                catch(ArgumentException)
                {
                    if(ident != TfmIdentifier.NETFramework) throw;
                    num = getNum(nameof(Version) + _V_LATEST);
                }

                string path = getToolPath(toolName, num);

                for(; path == null; path = getToolPath(toolName, num))
                {
                    ++num;
                    if(!Enum.IsDefined(targetDotNetFrameworkVersionType, num))
                    {
                        return null;
                    }
                }

                for(; path != null && !File.Exists(path); path = getToolPath(toolName, num))
                {
                    --num;
                    if(!Enum.IsDefined(targetDotNetFrameworkVersionType, num))
                    {
                        return null;
                    }
                }

                if(path != null && !File.Exists(path))
                {
                    return null;
                }
                return path;
            };
        }

        protected Type GetToolLocationHelperTypeFromRegsitry()
        {
            RegistryKey toolVersionsKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\MSBuild\\ToolsVersions", false);
            try
            {
                if(toolVersionsKey == null)
                {
                    return (Type)null;
                }
                Regex nameRegex = new Regex("^v?(\\d+(?:\\.\\d+))$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
                HashSet<string> utilFileNames = new HashSet<string>((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase)
                {
                    "Microsoft.Build.Utilities.Core",
                    "Microsoft.Build.Utilities"
                };
                Func<string, Type> getToolhelperType = (Func<string, Type>)(n => {
                    using(RegistryKey registryKey = toolVersionsKey.OpenSubKey(n, false))
                    {
                        if(registryKey == null)
                        {
                            return (Type)null;
                        }
                        string path = (string)registryKey.GetValue("MSBuildToolsPath");
                        if(path == null)
                        {
                            return (Type)null;
                        }
                        string[] files = Directory.GetFiles(path, "*.dll");
                        if(files.LongLength == 0L)
                        {
                            return (Type)null;
                        }
                        string assemblyFile = ((IEnumerable<string>)files).Select(dllName => new {
                            dllName = dllName,
                            fileName = Path.GetFileNameWithoutExtension(dllName)
                        }).Select(param0 => new {
                            __cc__h__TransparentIdentifierc = param0,
                            isUtil = utilFileNames.Contains(param0.fileName)
                        }).Select(param0 => new {
                            __cc__h__TransparentIdentifierd = param0,
                            utilMatch = ExportTaskImplementation<TTask>.MsbuildTaskLibRegex.Match(param0.__cc__h__TransparentIdentifierc.fileName)
                        }).Select(param0 => new {
                            __cc__h__TransparentIdentifiere = param0,
                            utilVersion = param0.utilMatch.Success ? new Version(param0.utilMatch.Groups[1].Value) : (Version)null
                        }).Where(param0 => {
                            if(!param0.__cc__h__TransparentIdentifiere.__cc__h__TransparentIdentifierd.isUtil)
                            {
                                return param0.utilVersion != (Version)null;
                            }
                            return true;
                        }).OrderBy(param0 => !(param0.utilVersion != (Version)null) ? 2 : 1).ThenBy(param0 => !param0.__cc__h__TransparentIdentifiere.__cc__h__TransparentIdentifierd.isUtil ? 2 : 1).ThenByDescending(param0 => param0.utilVersion).Select(param0 => new {
                            __cc__h__TransparentIdentifierf = param0,
                            asm = Assembly.ReflectionOnlyLoadFrom(param0.__cc__h__TransparentIdentifiere.__cc__h__TransparentIdentifierd.__cc__h__TransparentIdentifierc.dllName)
                        }).Select(param0 => new {
                            __cc__h__TransparentIdentifier10 = param0,
                            t = param0.asm.GetType("Microsoft.Build.Utilities.ToolLocationHelper")
                        }).Where(param0 => param0.t != null).Select(param0 => param0.__cc__h__TransparentIdentifier10.__cc__h__TransparentIdentifierf.__cc__h__TransparentIdentifiere.__cc__h__TransparentIdentifierd.__cc__h__TransparentIdentifierc.dllName).FirstOrDefault<string>();
                        if(assemblyFile == null)
                        {
                            return (Type)null;
                        }
                        return Assembly.LoadFrom(assemblyFile).GetType("Microsoft.Build.Utilities.ToolLocationHelper");
                    }
                });
                return ((IEnumerable<string>)toolVersionsKey.GetSubKeyNames()).Select(n => new {
                    n = n,
                    m = nameRegex.Match(n)
                }).Where(param0 => param0.m.Success).Select(param0 => new {
                    __cc__h__TransparentIdentifier12 = param0,
                    version = new Version(param0.m.Groups[1].Value)
                }).OrderByDescending(param0 => param0.version).Select(param0 => new {
                    __cc__h__TransparentIdentifier13 = param0,
                    t = getToolhelperType(param0.__cc__h__TransparentIdentifier12.n)
                }).Where(param0 => param0.t != null).Select(param0 => param0.t).FirstOrDefault<Type>();
            }
            finally
            {
                if(toolVersionsKey != null)
                {
                    ((IDisposable)toolVersionsKey).Dispose();
                }
            }
        }

        // FIXME: two different places (see ILAsm.RunCore) for the same thing ! be careful
        private bool ValidateFrameworkPath()
        {
            string foundPath;
            if(!IsILAsmDefault)
            {
                // to help find a .res -> obj COFF converter https://github.com/3F/coreclr/issues/2
                ValidateToolPath("cvtres.exe", FrameworkPath, GetFrameworkToolPath, out foundPath);
            }
            else if(!ValidateToolPath("ilasm.exe", FrameworkPath, GetFrameworkToolPath, out foundPath))
            {
                return false;
            }

            FrameworkPath = foundPath;
            return true;
        }

        private bool CopyIfNotExists(string file, string dir, string paths, TCallbackTool toolp)
        {
            string dest = Path.Combine(dir, file);
            if(File.Exists(dest)) {
                return true;
            }

            if(ValidateToolPath(file, paths, toolp, out string found)) {
                File.Copy(Path.Combine(found, file), dest);
                return true;
            }

            return false;
        }

        private bool ValidateToolPath(string toolFileName, string currentValue, TCallbackTool getToolPath, out string foundPath)
        {
            // via user values, e.g.: $(TargetFrameworkSDKToolsDirectory);...
            if(PropertyHasValue(currentValue) && TrySearchToolPath(currentValue, toolFileName, out foundPath))
            {
                return true;
            }

            // via ToolLocationHelper
            if(PropertyHasValue(TargetFrameworkVersion))
            {
                string toolDirectory = currentValue;
                if(TryToGetToolDirForFxVersion(toolFileName, getToolPath, ref toolDirectory))
                {
                    foundPath = toolDirectory;
                    return true;
                }
            }

            foundPath = null;
            return false;
        }

        private bool TryToGetToolDirForFxVersion(string toolFileName, TCallbackTool getToolPath, ref string toolDirectory)
        {
            Version tfmVersion = GetTfmVersion(TargetFrameworkVersion);
            TfmIdentifier tfmIdent = GetTfmIdentifier(TargetFrameworkIdentifier);

            if(tfmIdent == TfmIdentifier.NETFramework && tfmVersion.Major < 1)
            {
                return false;
            }

            if(getToolPath != null)
            {
                string path = getToolPath(tfmIdent, tfmVersion, toolFileName);
                if(path != null && File.Exists(path))
                {
                    toolDirectory = Path.GetDirectoryName(path);
                    return true;
                }
                _ActualTask.Log.LogError(string.Format(Resources.ToolLocationHelperTypeName_could_not_find_1, "Microsoft.Build.Utilities.ToolLocationHelper", toolFileName));
                return false;
            }

            // TODO
            if(!(tfmVersion >= _VersionUsingToolLocationHelper))
            {
                return false;
            }

            _ActualTask.Log.LogError(string.Format(Resources.Cannot_get_a_reference_to_ToolLocationHelper, "Microsoft.Build.Utilities.ToolLocationHelper"));
            return false;
        }

        private Version GetTfmVersion(string frameworkVersion)
        {
            if(!PropertyHasValue(frameworkVersion)) return null;
            return new Version(frameworkVersion.TrimStart('v', 'V'));
        }

        private TfmIdentifier GetTfmIdentifier(string raw) => raw?.Trim(' ', '.').ToLowerInvariant() switch
        {
            "netcoreapp" => TfmIdentifier.NETCoreApp,
            "netstandard" => TfmIdentifier.NETStandard,
            null or "" or "netframework" => TfmIdentifier.NETFramework,
            _ => TfmIdentifier.NETFramework, //TODO
        };

        // FIXME: two different places (see IlDasm.Run) for the same thing ! be careful
        private bool ValidateSdkPath()
        {
            if(!IsILAsmDefault) return true;

            if(!ValidateToolPath("ildasm.exe", SdkPath, GetSdkToolPath, out string foundPath))
            {
                return false;
            }

            SdkPath = foundPath;
            return true;
        }

        private static bool PropertyHasValue(string propertyValue)
        {
            if(String.IsNullOrWhiteSpace(propertyValue)) {
                return false;
            }
            //return !propertyValue.Contains("*Undefined*"); // *Undefined*\path1;C:\path2 ...
            //TODO: for something like this - Path.GetFullPath("*Undefined*\\..\\path") it will return absolute path to current folder + level up instead of exception.
            return !propertyValue.Trim().Equals("*Undefined*", StringComparison.InvariantCulture);
        }

        private string ValidateCmdScript(string data)
        {
            if(!PropertyHasValue(data)) {
                return null;
            }

            if(TrySearchToolPath(data, String.Empty, out string ret)) {
                return ret;
            }
            _ActualTask.Log.LogMessage(MessageImportance.Normal, Resources.Cannot_find_0_, data);
            return null;
        }

        private bool ValidateLibToolPath()
        {
            string str1 = (string)null;
            if(ExportTaskImplementation<TTask>.PropertyHasValue(this.LibToolPath))
            {
                if(ExportTaskImplementation<TTask>.TrySearchToolPath(this.LibToolPath, "lib.exe", out str1))
                {
                    this.LibToolPath = str1;
                }
                else
                {
                    this._ActualTask.Log.LogMessage(MessageImportance.Low, Resources.Cannot_find_lib_exe_in_0_, (object)this.LibToolPath);
                    this.LibToolPath = (string)null;
                    return false;
                }
            }
            else
            {
                LibToolPath = null;
                string dir = Environment.GetEnvironmentVariable("DevEnvDir").NullIfEmpty() ?? GetVsPath();
                if(PropertyHasValue(dir))
                {
                    if(TrySearchToolPath(dir, "lib.exe", out str1) || TrySearchToolPath(Path.Combine(dir, "VC"), "lib.exe", out str1) || TrySearchToolPath(Path.Combine(Path.Combine(dir, "VC"), "bin"), "lib.exe", out str1))
                    {
                        this.LibToolPath = str1;
                    }
                    else
                    {
                        this._ActualTask.Log.LogMessage(MessageImportance.Low, Resources.Cannot_find_lib_exe_in_0_, (object)this.LibToolPath);
                        this.LibToolPath = (string)null;
                        return true;
                    }
                }
            }
            if(!ExportTaskImplementation<TTask>.PropertyHasValue(str1))
            {
                str1 = (string)null;
            }
            if(!ExportTaskImplementation<TTask>.PropertyHasValue(this.LibToolDllPath))
            {
                if(!ExportTaskImplementation<TTask>.PropertyHasValue(str1))
                {
                    this.LibToolDllPath = (string)null;
                }
                else
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(str1);
                    while(directoryInfo != null && !Directory.Exists(Path.Combine(Path.Combine(directoryInfo.FullName, "Common7"), "IDE")))
                    {
                        directoryInfo = directoryInfo.Parent;
                    }
                    if(directoryInfo != null)
                    {
                        string path = Path.Combine(Path.Combine(directoryInfo.FullName, "Common7"), "IDE");
                        if(Directory.Exists(path))
                        {
                            this.LibToolDllPath = path;
                        }
                    }
                }
            }
            return true;
        }

        private static string GetVsPath()
        {
            string vsver = Environment.GetEnvironmentVariable("VisualStudioVersion");
            if(string.IsNullOrEmpty(vsver)) return null;

            Version v = new(vsver);
            string path = Environment.GetEnvironmentVariable(string.Format("VS{0}{1}COMNTOOLS", v.Major, v.Minor));
            if(string.IsNullOrEmpty(path)) return null;

            DirectoryInfo directoryInfo = new(path);
            if(directoryInfo.Name.Equals("tools", StringComparison.InvariantCultureIgnoreCase) && directoryInfo.Exists)
            {
                DirectoryInfo parent = directoryInfo.Parent;
                if(parent != null && parent.Parent != null && parent.Name.Equals("common7", StringComparison.InvariantCultureIgnoreCase))
                {
                    return parent.Parent.FullName;
                }
            }
            return null;
        }

        public static bool TrySearchToolPath(string toolPath, string toolFilename, out string value)
        {
            value = (string)null;
            while(toolPath.Contains("\\\\"))
            {
                toolPath = toolPath.Replace("\\\\", "\\");
            }
            string str = toolPath;
            char[] separator = new char[1] { ';' };
            int num = 1;
            foreach(string path in str.Split(separator, (StringSplitOptions)num))
            {
                if(!string.IsNullOrEmpty(path))
                {
                    string path1 = path;
                    if(File.Exists(Path.Combine(path1, toolFilename)))
                    {
                        value = path1;
                        return true;
                    }
                    string fullPath = Path.GetFullPath(path);
                    if(File.Exists(Path.Combine(fullPath, toolFilename)))
                    {
                        value = fullPath;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ValidateFileName()
        {
            bool flag = false;
            if(string.IsNullOrEmpty(this._Values.InputFileName))
            {
                this._ActualTask.Log.LogWarning(Resources.Input_file_is_empty__cannot_create_unmanaged_exports);
            }
            else if(!File.Exists(this._Values.InputFileName))
            {
                this._ActualTask.Log.LogWarning(Resources.Input_file_0_does_not_exist__cannot_create_unmanaged_exports, (object)this._Values.InputFileName);
            }
            else
            {
                if(!string.Equals(Path.GetExtension(this._Values.InputFileName).TrimStart('.'), "dll", StringComparison.OrdinalIgnoreCase))
                {
                    this._ActualTask.Log.LogMessage(Resources.Input_file_0_is_not_a_DLL_hint, (object)this._Values.InputFileName);
                }
                flag = true;
            }
            return flag;
        }

        private void ProblemSolver(Exception ex)
        {
            var found = new ProblemSolver(ex).FMsg;
            if(found != null) {
                _ActualTask.Log.LogError(found);
            }
        }

        private sealed class DllExportNotifierWithTask(TTask actualTask): DllExportNotifier
        {
            public TTask ActualTask { get; private set; } = actualTask;
        }
    }
}

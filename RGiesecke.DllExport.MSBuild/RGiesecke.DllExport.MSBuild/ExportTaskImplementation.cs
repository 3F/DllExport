//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2018  Denis Kuzmin < entry.reg@gmail.com > :: github.com/3F
//$ Distributed under the MIT License (MIT)

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Win32;
using RGiesecke.DllExport.MSBuild.Properties;

namespace RGiesecke.DllExport.MSBuild
{
    using TCallbackTool = Func<Version, string, string>;

    public class ExportTaskImplementation<TTask>: IInputValues, IServiceContainer, IServiceProvider where TTask : IDllExportTask, ITask
    {
        private static readonly Version _VersionUsingToolLocationHelper = new Version(4, 5);
        private static readonly IDictionary<string, TCallbackTool> _GetFrameworkToolPathByMethodName = (IDictionary<string, TCallbackTool>)new Dictionary<string, TCallbackTool>();
        private static readonly MethodInfo WrapGetToolPathCallMethodInfo = Utilities.GetMethodInfo<Func<string, int, string>>((Expression<Func<Func<string, int, string>>>)(() => ExportTaskImplementation<TTask>.WrapGetToolPathCall<int>(default(MethodInfo)))).GetGenericMethodDefinition();
        private static readonly Regex MsbuildTaskLibRegex = new Regex("(?<=^|\\\\)Microsoft\\.Build\\.Utilities\\.v(\\d+(?:\\.(\\d+))?)(?:\\.dll)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private readonly IServiceContainer _ServiceProvider = (IServiceContainer)new ServiceContainer();
        private readonly Dictionary<object, string> _LoggedMessages = new Dictionary<object, string>();
        private int _Timeout = 45000;
        private const string ToolLocationHelperTypeName = "Microsoft.Build.Utilities.ToolLocationHelper";
        private const string UndefinedPropertyValue = "*Undefined*";
        private readonly TTask _ActualTask;
        private readonly IInputValues _Values;
        private int _ErrorCount;

        public string MethodAttributes
        {
            get {
                return this._Values.MethodAttributes;
            }

            set {
                this._Values.MethodAttributes = value;
            }
        }

        public CpuPlatform Cpu
        {
            get {
                return this._Values.Cpu;
            }

            set {
                this._Values.Cpu = value;
            }
        }

        public string TargetFrameworkVersion
        {
            get;
            set;
        }

        public bool? SkipOnAnyCpu
        {
            get;
            set;
        }

        public bool EmitDebugSymbols
        {
            get {
                return this._Values.EmitDebugSymbols;
            }

            set {
                this._Values.EmitDebugSymbols = value;
            }
        }

        public string LeaveIntermediateFiles
        {
            get {
                return this._Values.LeaveIntermediateFiles;
            }

            set {
                this._Values.LeaveIntermediateFiles = value;
            }
        }

        public string FileName
        {
            get {
                return this._Values.FileName;
            }

            set {
                this._Values.FileName = value;
            }
        }

        [Required]
        public string FrameworkPath
        {
            get {
                return this._Values.FrameworkPath;
            }

            set {
                this._Values.FrameworkPath = value;
            }
        }

        public string VsDevCmd
        {
            get {
                return _Values.VsDevCmd;
            }

            set {
                _Values.VsDevCmd = value;
            }
        }

        public string VcVarsAll
        {
            get {
                return _Values.VcVarsAll;
            }

            set {
                _Values.VcVarsAll = value;
            }
        }

        public string LibToolPath
        {
            get {
                return this._Values.LibToolPath;
            }

            set {
                this._Values.LibToolPath = value;
            }
        }

        public string LibToolDllPath
        {
            get {
                return this._Values.LibToolDllPath;
            }

            set {
                this._Values.LibToolDllPath = value;
            }
        }

        [Required]
        public string InputFileName
        {
            get {
                return this._Values.InputFileName;
            }

            set {
                this._Values.InputFileName = value;
            }
        }

        public string KeyContainer
        {
            get {
                return this._Values.KeyContainer;
            }

            set {
                this._Values.KeyContainer = value;
            }
        }

        public string KeyFile
        {
            get {
                return this._Values.KeyFile;
            }

            set {
                this._Values.KeyFile = value;
            }
        }

        public string OutputFileName
        {
            get {
                return this._Values.OutputFileName;
            }

            set {
                this._Values.OutputFileName = value;
            }
        }

        public string RootDirectory
        {
            get {
                return this._Values.RootDirectory;
            }

            set {
                this._Values.RootDirectory = value;
            }
        }

        public string SdkPath
        {
            get {
                return this._Values.SdkPath;
            }

            set {
                this._Values.SdkPath = value;
            }
        }

        public int OrdinalsBase
        {
            get {
                return _Values.OrdinalsBase;
            }

            set {
                _Values.OrdinalsBase = value;
            }
        }

        public bool GenExpLib
        {
            get {
                return _Values.GenExpLib;
            }

            set {
                _Values.GenExpLib = value;
            }
        }

        public string OurILAsmPath
        {
            get {
                return _Values.OurILAsmPath;
            }

            set {
                _Values.OurILAsmPath = value;
            }
        }

        public string MetaLib
        {
            get {
                return _Values.MetaLib;
            }

            set {
                _Values.MetaLib = value;
            }
        }

        public PeCheckType PeCheck
        {
            get => _Values.PeCheck;
            set => _Values.PeCheck = value;
        }

        public string DllExportAttributeFullName
        {
            get {
                return this._Values.DllExportAttributeFullName;
            }

            set {
                this._Values.DllExportAttributeFullName = value;
            }
        }

        public string DllExportAttributeAssemblyName
        {
            get {
                return this._Values.DllExportAttributeAssemblyName;
            }

            set {
                this._Values.DllExportAttributeAssemblyName = value;
            }
        }

        public string Platform
        {
            get;
            set;
        }

        public string CpuType
        {
            get;
            set;
        }

        public string ProjectDirectory
        {
            get {
                return this._Values.RootDirectory;
            }

            set {
                this._Values.RootDirectory = value;
            }
        }

        public string AssemblyKeyContainerName
        {
            get {
                return this._Values.KeyContainer;
            }

            set {
                this._Values.KeyContainer = value;
            }
        }

        public int Timeout
        {
            get {
                return this._Timeout;
            }

            set {
                this._Timeout = value;
            }
        }

        private TCallbackTool GetFrameworkToolPath
        {
            get {
                return this.GetGetToolPath("GetPathToDotNetFrameworkFile");
            }
        }

        private TCallbackTool GetSdkToolPath
        {
            get {
                return this.GetGetToolPath("GetPathToDotNetFrameworkSdkFile");
            }
        }

        static ExportTaskImplementation()
        {
            AssemblyLoadingRedirection.EnsureSetup();
        }

        public ExportTaskImplementation(TTask actualTask)
        {
            this._ActualTask = actualTask;
            this.AddServiceFactory<ExportTaskImplementation<TTask>, IDllExportNotifier>((Func<IServiceProvider, IDllExportNotifier>)(sp => (IDllExportNotifier)new ExportTaskImplementation<TTask>.DllExportNotifierWithTask(this._ActualTask)));
            this._Values = (IInputValues)new InputValuesCore((IServiceProvider)this);
            this.GetNotifier().Notification += new EventHandler<DllExportNotificationEventArgs>(this.OnDllWrapperNotification);
        }

        public object GetService(Type serviceType)
        {
            return _ServiceProvider.GetService(serviceType);
        }

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
            return DllExportServiceProviderExtensions.GetService<IDllExportNotifier>(this);
        }

        public void Notify(int severity, string code, string message, params object[] values)
        {
            this.GetNotifier().Notify(severity, code, message, values);
        }

        public void Notify(int severity, string code, string fileName, SourceCodePosition? startPosition, SourceCodePosition? endPosition, string message, params object[] values)
        {
            this.GetNotifier().Notify(severity, code, fileName, startPosition, endPosition, message, values);
        }

        [CLSCompliant(false)]
        public AssemblyBinaryProperties InferAssemblyBinaryProperties()
        {
            return this._Values.InferAssemblyBinaryProperties();
        }

        public void InferOutputFile()
        {
            this._Values.InferOutputFile();
        }

        public bool Execute()
        {
            this._ErrorCount = 0;
            if(this.ValidateInputValues())
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
                    using(DllExportWeaver dllExportWeaver = new DllExportWeaver((IServiceProvider)this) {
                        Timeout = this.Timeout
                    })
                    {
                        dllExportWeaver.InputValues = (IInputValues)this._ActualTask;
                        dllExportWeaver.Run();
                    }
                    return this._ErrorCount == 0;
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
                ++this._ErrorCount;
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

            Func<string, int, string> getToolPath = ((n, v) => 
            {
                try
                {
                    return getToolPathCore(n, v);
                }
                catch
                {
                    return null;
                }
            });

            Func<string, int> getNum = delegate(string version) {
                return (int)Enum.Parse(targetDotNetFrameworkVersionType, version);
            };

            return ((version, toolName) => 
            {
                int num;

                // TargetDotNetFrameworkVersion Enumeration: https://msdn.microsoft.com/en-us/library/ms126273.aspx
                try {
                    num = getNum($"Version{version.Major}{version.Minor}");
                }
                catch(ArgumentException) {
                    num = getNum("VersionLatest"); // try with latest released version
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
            });
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
            if(!String.IsNullOrWhiteSpace(OurILAsmPath)) {
                // null value is also valid because we can try with env.PATH later for our new ILAsm - https://github.com/3F/coreclr/issues/2
                ValidateToolPath("cvtres.exe", FrameworkPath, GetFrameworkToolPath, out foundPath);
                //return CopyIfNotExists("cvtres.exe", OurILAsmPath, FrameworkPath, GetFrameworkToolPath);
            }
            else if(!ValidateToolPath("ilasm.exe", FrameworkPath, GetFrameworkToolPath, out foundPath)) {
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
            Version frameworkVersion = GetFrameworkVersion();
            if(frameworkVersion.Major < 1) {
                return false;
            }

            if(getToolPath != null)
            {
                string path = getToolPath(frameworkVersion, toolFileName);
                if(path != null && File.Exists(path))
                {
                    toolDirectory = Path.GetDirectoryName(path);
                    return true;
                }
                this._ActualTask.Log.LogError(string.Format(Resources.ToolLocationHelperTypeName_could_not_find_1, (object)"Microsoft.Build.Utilities.ToolLocationHelper", (object)toolFileName));
                return false;
            }
            if(!(frameworkVersion >= ExportTaskImplementation<TTask>._VersionUsingToolLocationHelper))
            {
                return false;
            }
            this._ActualTask.Log.LogError(string.Format(Resources.Cannot_get_a_reference_to_ToolLocationHelper, (object)"Microsoft.Build.Utilities.ToolLocationHelper"));
            return false;
        }

        private Version GetFrameworkVersion()
        {
            string frameworkVersion = this.TargetFrameworkVersion;
            if(!ExportTaskImplementation<TTask>.PropertyHasValue(frameworkVersion))
            {
                return (Version)null;
            }
            return new Version(frameworkVersion.TrimStart('v', 'V'));
        }

        // FIXME: two different places (see IlDasm.Run) for the same thing ! be careful
        private bool ValidateSdkPath()
        {
            if(!String.IsNullOrWhiteSpace(OurILAsmPath)) {
                return true;
            }

            if(!ValidateToolPath("ildasm.exe", SdkPath, GetSdkToolPath, out string foundPath)) {
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
                this.LibToolPath = (string)null;
                string str2 = Null.NullIfEmpty(Environment.GetEnvironmentVariable("DevEnvDir")) ?? ExportTaskImplementation<TTask>.GetVsPath();
                if(ExportTaskImplementation<TTask>.PropertyHasValue(str2))
                {
                    if(ExportTaskImplementation<TTask>.TrySearchToolPath(str2, "lib.exe", out str1) || ExportTaskImplementation<TTask>.TrySearchToolPath(Path.Combine(str2, "VC"), "lib.exe", out str1) || ExportTaskImplementation<TTask>.TrySearchToolPath(Path.Combine(Path.Combine(str2, "VC"), "bin"), "lib.exe", out str1))
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
            string version1 = Null.NullIfEmpty(Environment.GetEnvironmentVariable("VisualStudioVersion"));
            if(version1 == null)
            {
                return (string)null;
            }
            Version version2 = new Version(version1);
            string path = Null.NullIfEmpty(Environment.GetEnvironmentVariable(string.Format("VS{0}{1}COMNTOOLS", (object)version2.Major, (object)version2.Minor)));
            if(path == null)
            {
                return (string)null;
            }
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if(directoryInfo.Name.Equals("tools", StringComparison.InvariantCultureIgnoreCase) && directoryInfo.Exists)
            {
                DirectoryInfo parent = directoryInfo.Parent;
                if(parent != null && parent.Parent != null && parent.Name.Equals("common7", StringComparison.InvariantCultureIgnoreCase))
                {
                    return parent.Parent.FullName;
                }
            }
            return (string)null;
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

        private sealed class DllExportNotifierWithTask: DllExportNotifier
        {
            public TTask ActualTask
            {
                get;
                private set;
            }

            public DllExportNotifierWithTask(TTask actualTask)
            {
                this.ActualTask = actualTask;
            }
        }
    }
}

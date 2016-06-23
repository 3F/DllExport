// [Decompiled] Assembly: RGiesecke.DllExport.MSBuild, Version=1.2.6.36228, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Build.Framework;
using RGiesecke.DllExport.MSBuild.Properties;

namespace RGiesecke.DllExport.MSBuild
{
    public class ExportTaskImplementation<TTask>: IInputValues, IServiceContainer, IServiceProvider where TTask : IDllExportTask, ITask
    {
        private static readonly Version _VersionUsingToolLocationHelper = new Version(4, 5);
        private static readonly IDictionary<string, Func<Version, string, string>> _GetFrameworkToolPathByMethodName = (IDictionary<string, Func<Version, string, string>>)new Dictionary<string, Func<Version, string, string>>();
        private static readonly MethodInfo WrapGetToolPathCallMethodInfo = Utilities.GetMethodInfo<Func<string, int, string>>((Expression<Func<Func<string, int, string>>>)(() => ExportTaskImplementation<TTask>.WrapGetToolPathCall<int>(default(MethodInfo)))).GetGenericMethodDefinition();
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

        public string PlatformTarget
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

        private static Func<Version, string, string> GetFrameworkToolPath
        {
            get {
                return ExportTaskImplementation<TTask>.GetGetToolPath("GetPathToDotNetFrameworkFile");
            }
        }

        private static Func<Version, string, string> GetSdkToolPath
        {
            get {
                return ExportTaskImplementation<TTask>.GetGetToolPath("GetPathToDotNetFrameworkSdkFile");
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

        object IServiceProvider.GetService(Type serviceType)
        {
            return this._ServiceProvider.GetService(serviceType);
        }

        void IServiceContainer.AddService(Type serviceType, object serviceInstance)
        {
            this._ServiceProvider.AddService(serviceType, serviceInstance);
        }

        void IServiceContainer.AddService(Type serviceType, object serviceInstance, bool promote)
        {
            this._ServiceProvider.AddService(serviceType, serviceInstance, promote);
        }

        void IServiceContainer.AddService(Type serviceType, ServiceCreatorCallback callback)
        {
            this._ServiceProvider.AddService(serviceType, callback);
        }

        void IServiceContainer.AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
        {
            this._ServiceProvider.AddService(serviceType, callback, promote);
        }

        void IServiceContainer.RemoveService(Type serviceType)
        {
            this._ServiceProvider.RemoveService(serviceType);
        }

        void IServiceContainer.RemoveService(Type serviceType, bool promote)
        {
            this._ServiceProvider.RemoveService(serviceType, promote);
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
                    this._ActualTask.Log.LogErrorFromException(ex);
                    this._ActualTask.Log.LogMessage(ex.StackTrace);
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
            var data = new { startPos = sourceCodePosition1, endPos = sourceCodePosition2, fileName = file, Severity = e.Severity, Code = e.Code, Message = message };
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
            bool flag = this.ValidateLibToolPath() & this.ValidateFrameworkPath() & this.ValidateSdkPath();
            if(!string.IsNullOrEmpty(this.CpuType) && (string.IsNullOrEmpty(this.Platform) || string.Equals(this.Platform, "anycpu", StringComparison.OrdinalIgnoreCase)))
            {
                this.Platform = this.CpuType;
            }
            if(!string.IsNullOrEmpty(this.PlatformTarget) && (string.IsNullOrEmpty(this.Platform) || string.Equals(this.Platform, "anycpu", StringComparison.OrdinalIgnoreCase)))
            {
                this.Platform = this.PlatformTarget;
            }
            return flag & this.ValidateFileName();
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

        private static Func<Version, string, string> GetGetToolPath(string methodName)
        {
            lock(ExportTaskImplementation<TTask>._GetFrameworkToolPathByMethodName)
            {
                Func<Version, string, string> local_0;
                if(!ExportTaskImplementation<TTask>._GetFrameworkToolPathByMethodName.TryGetValue(methodName, out local_0))
                {
                    ExportTaskImplementation<TTask>._GetFrameworkToolPathByMethodName.Add(methodName, local_0 = ExportTaskImplementation<TTask>.GetGetToolPathInternal(methodName));
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

        private static Func<Version, string, string> GetGetToolPathInternal(string methodName)
        {
            Type type = Type.GetType("Microsoft.Build.Utilities.ToolLocationHelper") ?? Type.GetType("Microsoft.Build.Utilities.ToolLocationHelper, Microsoft.Build.Utilities.v4.0, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
            if(type == null)
            {
                Assembly assembly;
                try
                {
                    assembly = Assembly.Load("Microsoft.Build.Utilities.v4.0, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
                }
                catch(FileNotFoundException ex)
                {
                    assembly = (Assembly)null;
                }
                if(assembly != null)
                {
                    type = assembly.GetType("Microsoft.Build.Utilities.ToolLocationHelper");
                }
            }
            if(type == null)
            {
                return (Func<Version, string, string>)null;
            }
            Type targetDotNetFrameworkVersionType = type.Assembly.GetType("Microsoft.Build.Utilities.TargetDotNetFrameworkVersion");
            MethodInfo method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public, (Binder)null, new Type[2] { typeof(string), targetDotNetFrameworkVersionType }, (ParameterModifier[])null);
            if(method == null)
            {
                return (Func<Version, string, string>)null;
            }
            Func<string, int, string> getToolPathCore = (Func<string, int, string>)ExportTaskImplementation<TTask>.WrapGetToolPathCallMethodInfo.MakeGenericMethod(targetDotNetFrameworkVersionType).Invoke((object)null, new object[1] { (object)method });
            Func<string, int, string> getToolPath = (Func<string, int, string>)((n, v) => {
                try
                {
                    return getToolPathCore(n, v);
                }
                catch(ArgumentException ex)
                {
                    return (string)null;
                }
            });
            return (Func<Version, string, string>)((version, toolName) => {
                int num = (int)Enum.Parse(targetDotNetFrameworkVersionType, "Version" + (object)version.Major + (object)version.Minor);
                string path;
                for(path = getToolPath(toolName, num); path == null; path = getToolPath(toolName, num))
                {
                    ++num;
                    if(!Enum.IsDefined(targetDotNetFrameworkVersionType, (object)num))
                    {
                        return (string)null;
                    }
                }
                for(; path != null && !File.Exists(path); path = getToolPath(toolName, num))
                {
                    --num;
                    if(!Enum.IsDefined(targetDotNetFrameworkVersionType, (object)num))
                    {
                        return (string)null;
                    }
                }
                if(path != null && !File.Exists(path))
                {
                    return (string)null;
                }
                return path;
            });
        }

        private bool ValidateFrameworkPath()
        {
            string foundPath;
            if(!this.ValidateToolPath("ilasm.exe", this.FrameworkPath, ExportTaskImplementation<TTask>.GetFrameworkToolPath, out foundPath))
            {
                return false;
            }
            this.FrameworkPath = foundPath;
            return true;
        }

        private bool ValidateToolPath(string toolFileName, string currentValue, Func<Version, string, string> getToolPath, out string foundPath)
        {
            if(ExportTaskImplementation<TTask>.PropertyHasValue(this.TargetFrameworkVersion))
            {
                string toolDirectory = currentValue;
                if(this.TryToGetToolDirForFxVersion(toolFileName, getToolPath, ref toolDirectory))
                {
                    foundPath = toolDirectory;
                    return true;
                }
            }
            if(ExportTaskImplementation<TTask>.PropertyHasValue(currentValue) && ExportTaskImplementation<TTask>.TrySearchToolPath(currentValue, toolFileName, out foundPath))
            {
                return true;
            }
            foundPath = (string)null;
            return false;
        }

        private bool TryToGetToolDirForFxVersion(string toolFileName, Func<Version, string, string> getToolPath, ref string toolDirectory)
        {
            Version frameworkVersion = this.GetFrameworkVersion();
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

        private bool ValidateSdkPath()
        {
            string foundPath;
            if(!this.ValidateToolPath("ildasm.exe", this.SdkPath, ExportTaskImplementation<TTask>.GetSdkToolPath, out foundPath))
            {
                return false;
            }
            this.SdkPath = foundPath;
            return true;
        }

        private static bool PropertyHasValue(string propertyValue)
        {
            if(!string.IsNullOrEmpty(propertyValue))
            {
                return !propertyValue.Contains("*Undefined*");
            }
            return false;
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
                    this._ActualTask.Log.LogMessage(MessageImportance.Normal, Resources.Cannot_find_lib_exe_in_0_, (object)this.LibToolPath);
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

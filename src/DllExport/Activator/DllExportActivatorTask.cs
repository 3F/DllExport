/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using System.Resources;
using System.Security.Permissions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using net.r_eg.DllExport.Extensions;
using net.r_eg.DllExport.ILAsm;

namespace net.r_eg.DllExport.Activator
{
#if FEATURE_ACTIVATOR_ISOLATED_TASK
    [LoadInSeparateAppDomain]
#endif
    [PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
    public class DllExportActivatorTask:
#if FEATURE_ACTIVATOR_ISOLATED_TASK
        AppDomainIsolatedTask,
#else
        Task,
#endif
        IDllExportTask, IInputValues, IInputRawValues, IServiceProvider, ITask
    {
        private readonly ExportTaskImplementation<DllExportActivatorTask> exportTask;

        public string MethodAttributes
        {
            get => exportTask.MethodAttributes;
            set => exportTask.MethodAttributes = value;
        }

        bool? IDllExportTask.SkipOnAnyCpu
        {
            get => exportTask.SkipOnAnyCpu;
            set => exportTask.SkipOnAnyCpu = value;
        }

        public string SkipOnAnyCpu
        {
            set => exportTask.SkipOnAnyCpu = value.ParseNullableBool();
        }

        public string TargetFrameworkVersion
        {
            get => exportTask.TargetFrameworkVersion;
            set => exportTask.TargetFrameworkVersion = value;
        }

        public string TargetFrameworkIdentifier
        {
            get => exportTask.TargetFrameworkIdentifier;
            set => exportTask.TargetFrameworkIdentifier = value;
        }

        public string Platform
        {
            get => exportTask.Platform;
            set => exportTask.Platform = value;
        }

        public string CpuType
        {
            get => exportTask.CpuType;
            set => exportTask.CpuType = value;
        }

        public string ProjectDirectory
        {
            get => exportTask.ProjectDirectory;
            set => exportTask.ProjectDirectory = value;
        }

        public string AssemblyKeyContainerName
        {
            get => exportTask.AssemblyKeyContainerName;
            set => exportTask.AssemblyKeyContainerName = value;
        }

        public int Timeout
        {
            get => exportTask.Timeout;
            set => exportTask.Timeout = value;
        }

        public CpuPlatform Cpu
        {
            get => exportTask.Cpu;
            set => exportTask.Cpu = value;
        }

        DebugType IInputValues.EmitDebugSymbols
        {
            get => exportTask.EmitDebugSymbols;
            set => exportTask.EmitDebugSymbols = value;
        }

        public string EmitDebugSymbols
        {
            set => exportTask.EmitDebugSymbols = ParseEmitDebugSymbols(value);
        }

        public string LeaveIntermediateFiles
        {
            get => exportTask.LeaveIntermediateFiles;
            set => exportTask.LeaveIntermediateFiles = value;
        }

        public string FileName
        {
            get => exportTask.FileName;
            set => exportTask.FileName = value;
        }

        [Required]
        public string FrameworkPath
        {
            get => exportTask.FrameworkPath;
            set => exportTask.FrameworkPath = value;
        }

        public string VsDevCmd
        {
            get => exportTask.VsDevCmd;
            set => exportTask.VsDevCmd = value;
        }

        public string VcVarsAll
        {
            get => exportTask.VcVarsAll;
            set => exportTask.VcVarsAll = value;
        }

        public string LibToolPath
        {
            get => exportTask.LibToolPath;
            set => exportTask.LibToolPath = value;
        }

        public string LibToolDllPath
        {
            get => exportTask.LibToolDllPath;
            set => exportTask.LibToolDllPath = value;
        }

        [Required]
        public string InputFileName
        {
            get => exportTask.InputFileName;
            set => exportTask.InputFileName = value;
        }

        public string KeyContainer
        {
            get => exportTask.KeyContainer;
            set => exportTask.KeyContainer = value;
        }

        public string KeyFile
        {
            get => exportTask.KeyFile;
            set => exportTask.KeyFile = value;
        }

        public string OutputFileName
        {
            get => exportTask.OutputFileName;
            set => exportTask.OutputFileName = value;
        }

        public string RootDirectory
        {
            get => exportTask.RootDirectory;
            set => exportTask.RootDirectory = value;
        }

        public string SdkPath
        {
            get => exportTask.SdkPath;
            set => exportTask.SdkPath = value;
        }

        public int OrdinalsBase
        {
            get => exportTask.OrdinalsBase;
            set => exportTask.OrdinalsBase = value;
        }

        public bool GenExpLib
        {
            get => exportTask.GenExpLib;
            set => exportTask.GenExpLib = value;
        }

        public string OurILAsmPath
        {
            get => exportTask.OurILAsmPath;
            set => exportTask.OurILAsmPath = value;
        }

        public bool SysObjRebase
        {
            get => exportTask.SysObjRebase;
            set => exportTask.SysObjRebase = value;
        }

        public string ProcEnv
        {
            get => exportTask.ProcEnv;
            set => exportTask.ProcEnv = value;
        }

        public string MetaLib
        {
            get => exportTask.MetaLib;
            set => exportTask.MetaLib = value;
        }

        public PatchesType Patches
        {
            get => exportTask.Patches;
            set => exportTask.Patches = value;
        }

        public long PatchesRaw
        {
            set => Patches = (PatchesType)value;
        }

        public PeCheckType PeCheck
        {
            get => exportTask.PeCheck;
            set => exportTask.PeCheck = value;
        }

        public int PeCheckRaw
        {
            set => PeCheck = (PeCheckType)value;
        }

        public List<AssemblyExternDirective> AssemblyExternDirectives
        {
            get => exportTask.AssemblyExternDirectives;
            set => exportTask.AssemblyExternDirectives = value;
        }

        public string AssemblyExternDirectivesRaw
        {
            set => AssemblyExternDirectives = new(value.Deserialize<AssemblyExternDirective>());
        }

        public List<TypeRefDirective> TypeRefDirectives
        {
            get => exportTask.TypeRefDirectives;
            set => exportTask.TypeRefDirectives = value;
        }

        public string TypeRefDirectivesRaw
        {
            set => TypeRefDirectives = new(value.Deserialize<TypeRefDirective>());
        }

        public string DllExportAttributeFullName
        {
            get => exportTask.DllExportAttributeFullName;
            set => exportTask.DllExportAttributeFullName = value;
        }

        public string DllExportAttributeAssemblyName
        {
            get => exportTask.DllExportAttributeAssemblyName;
            set => exportTask.DllExportAttributeAssemblyName = value;
        }

        static DllExportActivatorTask() => AssemblyLoadingRedirection.EnsureSetup();

        public DllExportActivatorTask() => exportTask = new(this);

        public DllExportActivatorTask(ResourceManager taskResources)
            : base(taskResources)
        {
            exportTask = new(this);
        }

        public DllExportActivatorTask(ResourceManager taskResources, string helpKeywordPrefix)
            : base(taskResources, helpKeywordPrefix)
        {
            exportTask = new(this);
        }

        public IDllExportNotifier GetNotifier() => exportTask.GetNotifier();

        public void Notify(int severity, string code, string message, params object[] values)
        {
            exportTask.Notify(severity, code, message, values);
        }

        public void Notify(int severity, string code, string fileName, SourceCodePosition? startPosition, SourceCodePosition? endPosition, string message, params object[] values)
        {
            exportTask.Notify(severity, code, fileName, startPosition, endPosition, message, values);
        }

        [CLSCompliant(false)]
        public AssemblyBinaryProperties InferAssemblyBinaryProperties() => exportTask.InferAssemblyBinaryProperties();

        public void InferOutputFile() => exportTask.InferOutputFile();

        public override bool Execute() => exportTask.Execute();

        public object GetService(Type serviceType) => exportTask.GetService(serviceType);

        private DebugType ParseEmitDebugSymbols(string input)
        {
            if(string.IsNullOrWhiteSpace(input)) return DebugType.Default;

            return (DebugType)Enum.Parse
            (
                typeof(DebugType),
                input.Trim(),
                ignoreCase: true
            );
        }
    }
}

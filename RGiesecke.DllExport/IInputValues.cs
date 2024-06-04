/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

namespace RGiesecke.DllExport
{
    public interface IInputValues
    {
        CpuPlatform Cpu
        {
            get;
            set;
        }

        bool EmitDebugSymbols
        {
            get;
            set;
        }

        string LeaveIntermediateFiles
        {
            get;
            set;
        }

        string FileName
        {
            get;
            set;
        }

        string FrameworkPath
        {
            get;
            set;
        }

        string InputFileName
        {
            get;
            set;
        }

        string KeyContainer
        {
            get;
            set;
        }

        string KeyFile
        {
            get;
            set;
        }

        string OutputFileName
        {
            get;
            set;
        }

        string RootDirectory
        {
            get;
            set;
        }

        string SdkPath
        {
            get;
            set;
        }

        int OrdinalsBase
        {
            get;
            set;
        }

        bool GenExpLib
        {
            get;
            set;
        }

        string OurILAsmPath
        {
            get;
            set;
        }

        bool SysObjRebase
        {
            get;
            set;
        }

        string ProcEnv
        {
            get;
            set;
        }

        string MetaLib
        {
            get;
            set;
        }

        PatchesType Patches
        {
            get;
            set;
        }

        PeCheckType PeCheck
        {
            get;
            set;
        }

        string MethodAttributes
        {
            get;
            set;
        }

        string VsDevCmd
        {
            get;
            set;
        }

        string VcVarsAll
        {
            get;
            set;
        }

        string LibToolPath
        {
            get;
            set;
        }

        string DllExportAttributeFullName
        {
            get;
            set;
        }

        string DllExportAttributeAssemblyName
        {
            get;
            set;
        }

        string LibToolDllPath
        {
            get;
            set;
        }

        AssemblyBinaryProperties InferAssemblyBinaryProperties();

        void InferOutputFile();
    }
}

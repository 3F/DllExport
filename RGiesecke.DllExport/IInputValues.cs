//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2018  Denis Kuzmin < entry.reg@gmail.com > :: github.com/3F
//$ Distributed under the MIT License (MIT)

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

        string MetaLib
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

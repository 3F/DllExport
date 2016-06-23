// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.1.28776, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

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

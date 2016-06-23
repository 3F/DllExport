// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.1.28776, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System.IO;
using System.Threading;

namespace RGiesecke.DllExport
{
    public class InputValuesCore: IInputValues
    {
        private string _DllExportAttributeAssemblyName = Utilities.DllExportAttributeAssemblyName;
        private string _DllExportAttributeFullName = Utilities.DllExportAttributeFullName;
        private string _Filename;

        public CpuPlatform Cpu
        {
            get;
            set;
        }

        public bool EmitDebugSymbols
        {
            get;
            set;
        }

        public string FrameworkPath
        {
            get;
            set;
        }

        public string InputFileName
        {
            get;
            set;
        }

        public string KeyContainer
        {
            get;
            set;
        }

        public string KeyFile
        {
            get;
            set;
        }

        public string OutputFileName
        {
            get;
            set;
        }

        public string RootDirectory
        {
            get;
            set;
        }

        public string SdkPath
        {
            get;
            set;
        }

        public string LibToolPath
        {
            get;
            set;
        }

        public string LibToolDllPath
        {
            get;
            set;
        }

        public string DllExportAttributeFullName
        {
            get {
                return this._DllExportAttributeFullName;
            }

            set {
                this._DllExportAttributeFullName = value;
            }
        }

        public string DllExportAttributeAssemblyName
        {
            get {
                return this._DllExportAttributeAssemblyName;
            }

            set {
                this._DllExportAttributeAssemblyName = value;
            }
        }

        public string FileName
        {
            get {
                Monitor.Enter((object)this);
                try
                {
                    if(string.IsNullOrEmpty(this._Filename))
                    {
                        this._Filename = Path.GetFileNameWithoutExtension(this.InputFileName);
                    }
                }
                finally
                {
                    Monitor.Exit((object)this);
                }
                return this._Filename;
            }

            set {
                Monitor.Enter((object)this);
                try
                {
                    this._Filename = value;
                }
                finally
                {
                    Monitor.Exit((object)this);
                }
            }
        }

        public AssemblyBinaryProperties InferAssemblyBinaryProperties()
        {
            AssemblyBinaryProperties binaryProperties = Utilities.CreateAssemblyInspector((IInputValues)this).GetAssemblyBinaryProperties(this.InputFileName);
            if(this.Cpu == CpuPlatform.None && binaryProperties.BinaryWasScanned)
            {
                this.Cpu = binaryProperties.CpuPlatform;
            }
            return binaryProperties;
        }

        public void InferOutputFile()
        {
            if(!string.IsNullOrEmpty(this.OutputFileName))
            {
                return;
            }
            this.OutputFileName = this.InputFileName;
        }
    }
}

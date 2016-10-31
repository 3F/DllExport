// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.7.38850, Culture=neutral, PublicKeyToken=8f52d83c1a22df51
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using System.IO;
using System.Threading;
using RGiesecke.DllExport.Parsing;

namespace RGiesecke.DllExport
{
    public class InputValuesCore: HasServiceProvider, IInputValues
    {
        private string _DllExportAttributeAssemblyName = Utilities.DllExportAttributeAssemblyName;
        private string _DllExportAttributeFullName = Utilities.DllExportAttributeFullName;
        private string _Filename;

        public CpuPlatform Cpu
        {
            get;
            set;
        }

        public string LeaveIntermediateFiles
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

        public int OrdinalsBase
        {
            get;
            set;
        }

        public bool GenExpLib
        {
            get;
            set;
        }

        public string OurILAsmPath
        {
            get;
            set;
        }

        public string MetaLib
        {
            get;
            set;
        }

        public string MethodAttributes
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

        public InputValuesCore(IServiceProvider serviceProvider)
        : base(serviceProvider)
        {
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

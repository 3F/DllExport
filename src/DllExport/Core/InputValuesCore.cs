﻿/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.IO;
using System.Threading;
using net.r_eg.DllExport.Parsing;

namespace net.r_eg.DllExport
{
    public class InputValuesCore(IServiceProvider provider): HasServiceProvider(provider), IInputValues
    {
        private string _DllExportAttributeAssemblyName = Utilities.DllExportAttributeAssemblyName;
        private string _DllExportAttributeFullName = Utilities.DllExportAttributeFullName;
        private string _Filename;

        public CpuPlatform Cpu { get; set; }

        public string LeaveIntermediateFiles { get; set; }

        public bool EmitDebugSymbols { get; set; }

        public string FrameworkPath { get; set; }

        public string InputFileName { get; set; }

        public string KeyContainer { get; set; }

        public string KeyFile { get; set; }

        public string OutputFileName { get; set; }

        public string RootDirectory { get; set; }

        public string SdkPath { get; set; }

        public int OrdinalsBase { get; set; }

        public bool GenExpLib { get; set; }

        public string OurILAsmPath { get; set; }

        public bool SysObjRebase { get; set; }

        public string ProcEnv { get; set; }

        public string MetaLib { get; set; }

        public PatchesType Patches { get; set; }

        public PeCheckType PeCheck { get; set; }

        public string MethodAttributes { get; set; }

        public string VsDevCmd { get; set; }

        public string VcVarsAll { get; set; }

        public string LibToolPath { get; set; }

        public string LibToolDllPath { get; set; }

        public string DllExportAttributeFullName
        {
            get => _DllExportAttributeFullName;
            set => _DllExportAttributeFullName = value;
        }

        public string DllExportAttributeAssemblyName
        {
            get => _DllExportAttributeAssemblyName;
            set => _DllExportAttributeAssemblyName = value;
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

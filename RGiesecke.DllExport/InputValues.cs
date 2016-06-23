// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.1.28776, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using System.Collections.Generic;
using System.IO;
using RGiesecke.DllExport.Properties;

namespace RGiesecke.DllExport
{
    public sealed class InputValues: InputValuesCore, IDllExportNotifier, IDisposable
    {
        private readonly DllExportNotifier _Notifier = new DllExportNotifier();

        public object Context
        {
            get {
                return this._Notifier.Context;
            }

            set {
                this._Notifier.Context = value;
            }
        }

        public event EventHandler<DllExportNotificationEventArgs> Notification
        {
            add {
                this._Notifier.Notification += value;
            }
            remove {
                this._Notifier.Notification -= value;
            }
        }

        public void Dispose()
        {
            this._Notifier.Dispose();
        }

        private static bool StartsWith(string input, string left)
        {
            if(!input.StartsWith("/" + left, StringComparison.OrdinalIgnoreCase))
            {
                return input.StartsWith("-" + left, StringComparison.OrdinalIgnoreCase);
            }
            return true;
        }

        private static bool Equals(string input, string left)
        {
            if(!string.Equals(input, "/" + left, StringComparison.OrdinalIgnoreCase))
            {
                return string.Equals(input, "-" + left, StringComparison.OrdinalIgnoreCase);
            }
            return true;
        }

        public bool ValidateInputValues(params string[] args)
        {
            if(((ICollection<string>)args).NullSafeCount<string>() < 1)
            {
                throw new ArgumentException(Resources.No_paramaters_provided__at_least_the_input_assembly_name_is_needed);
            }
            this.InputFileName = Path.GetFullPath(args[0]);
            if(string.IsNullOrEmpty(this.InputFileName))
            {
                throw new ArgumentException(Resources.Input_file_required);
            }
            if(!File.Exists(this.InputFileName))
            {
                throw new ArgumentException(Resources.Input_assembly_does_not_exist);
            }
            if(!string.Equals(Path.GetExtension(this.InputFileName), ".dll"))
            {
                throw new ArgumentException(Resources.Input_file_must_be_a_DLL);
            }
            for(int index = 1; index < args.Length; ++index)
            {
                string str = args[index];
                if(!string.IsNullOrEmpty(str))
                {
                    if(InputValues.StartsWith(str, "out"))
                    {
                        this.OutputFileName = InputValues.GetArgumentValueWithoutDuplicates(str, "out", this.OutputFileName);
                    }
                    else if(InputValues.StartsWith(str, "frameworkdir"))
                    {
                        this.FrameworkPath = InputValues.GetArgumentValueWithoutDuplicates(str, "frameworkdir", this.FrameworkPath);
                    }
                    else if(InputValues.StartsWith(str, "attributename"))
                    {
                        this.DllExportAttributeFullName = InputValues.GetArgumentValueWithoutDuplicates(str, "attributename", this.DllExportAttributeFullName);
                    }
                    else if(InputValues.StartsWith(str, "dllexportattributefullname"))
                    {
                        this.DllExportAttributeFullName = InputValues.GetArgumentValueWithoutDuplicates(str, "dllexportattributefullname", this.DllExportAttributeFullName);
                    }
                    else if(InputValues.StartsWith(str, "sdkdir"))
                    {
                        this.SdkPath = InputValues.GetArgumentValueWithoutDuplicates(str, "sdkdir", this.SdkPath);
                    }
                    else if(InputValues.Equals(str, "Debug"))
                    {
                        this.EmitDebugSymbols = true;
                    }
                    else if(InputValues.Equals(str, "x86"))
                    {
                        this.Cpu = InputValues.GetCpuValueWithoutDuplicates(this.Cpu, CpuPlatform.X86);
                    }
                    else if(InputValues.Equals(str, "anycpu"))
                    {
                        this.Cpu = InputValues.GetCpuValueWithoutDuplicates(this.Cpu, CpuPlatform.AnyCpu);
                    }
                    else if(InputValues.Equals(str, "x64"))
                    {
                        this.Cpu = InputValues.GetCpuValueWithoutDuplicates(this.Cpu, CpuPlatform.X64);
                    }
                    else if(InputValues.Equals(str, "Itanium"))
                    {
                        this.Cpu = InputValues.GetCpuValueWithoutDuplicates(this.Cpu, CpuPlatform.Itanium);
                    }
                }
            }
            return true;
        }

        private static string GetArgumentValue(string argumentElement, string argumentName)
        {
            int colonIndex = InputValues.GetColonIndex(argumentElement, argumentName);
            return argumentElement.Substring(colonIndex + 1).NullSafeTrim();
        }

        private static string GetArgumentValueWithoutDuplicates(string argumentElement, string argumentName, string oldValue)
        {
            if(!string.IsNullOrEmpty(oldValue))
            {
                throw new ArgumentException(string.Format(Resources.Duplicate_0_specified_, (object)argumentName));
            }
            return InputValues.GetArgumentValue(argumentElement, argumentName);
        }

        private static CpuPlatform GetCpuValueWithoutDuplicates(CpuPlatform oldValue, CpuPlatform newValue)
        {
            if(oldValue != CpuPlatform.None)
            {
                throw new ArgumentException(string.Format(Resources.Duplicate_0_specified_, (object)"CPU platform"));
            }
            return newValue;
        }

        private static int GetColonIndex(string argumentElement, string argumentName)
        {
            int length = argumentName.Length;
            int num = argumentElement.IndexOf(':');
            if(num < 0)
            {
                num = length;
            }
            return num;
        }
    }
}

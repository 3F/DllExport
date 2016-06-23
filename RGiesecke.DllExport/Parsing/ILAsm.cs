// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.2.23706, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Text;
using RGiesecke.DllExport.Properties;

namespace RGiesecke.DllExport.Parsing
{
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public sealed class IlAsm: IDllExportNotifier, IDisposable
    {
        private readonly DllExportNotifier _Notifier = new DllExportNotifier();

        public int Timeout
        {
            get;
            set;
        }

        public AssemblyExports Exports
        {
            get;
            set;
        }

        public IInputValues InputValues
        {
            get;
            set;
        }

        public string TempDirectory
        {
            get;
            set;
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

        public IlAsm()
        {
            this._Notifier.Context = (object)this;
        }

        public void Dispose()
        {
            this._Notifier.Dispose();
        }

        public int ReassembleFile(string outputFile, string ilSuffix, CpuPlatform cpu)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(this.TempDirectory);
            try
            {
                string directoryName = Path.GetDirectoryName(outputFile);
                if(directoryName != null && !Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                using(IlParser ilParser = new IlParser())
                {
                    ilParser.Notification += (EventHandler<DllExportNotificationEventArgs>)((s, e) => this._Notifier.Notify(e));
                    ilParser.Exports = this.Exports;
                    ilParser.InputValues = this.InputValues;
                    ilParser.TempDirectory = this.TempDirectory;
                    List<string> stringList = new List<string>(ilParser.GetLines(cpu));
                    if(stringList.Count > 0)
                    {
                        string input = stringList[stringList.Count - 1];
                        if(!input.NullSafeCall<string, bool>((Func<string, bool>)(l => {
                            if(!l.EndsWith("\\r"))
                            {
                                return l.EndsWith("\\n");
                            }
                            return true;
                        })))
                            stringList[stringList.Count - 1] = input + Environment.NewLine;
                    }
                    using(FileStream fileStream = new FileStream(Path.Combine(this.TempDirectory, this.InputValues.FileName + ilSuffix + ".il"), FileMode.Create))
                    {
                        using(StreamWriter streamWriter = new StreamWriter((Stream)fileStream, Encoding.Unicode))
                        {
                            foreach(string str in stringList)
                            {
                                streamWriter.WriteLine(str);
                            }
                        }
                    }
                }
                return this.Run(outputFile, ilSuffix, cpu);
            }
            finally
            {
                Directory.SetCurrentDirectory(currentDirectory);
            }
        }

        private int Run(string outputFile, string ilSuffix, CpuPlatform cpu)
        {
            StringBuilder stringBuilder = new StringBuilder(100);
            foreach(string file in Directory.GetFiles(this.TempDirectory, "*.res"))
            {
                if(string.Equals(Path.GetExtension(file).NullSafeTrimStart('.'), "res", StringComparison.OrdinalIgnoreCase))
                {
                    stringBuilder.Append(" \"/resource=").Append(file).Append("\" ");
                }
            }
            string ressourceParam = stringBuilder.ToString();
            if(string.IsNullOrEmpty(ressourceParam))
            {
                ressourceParam = " ";
            }
            string str1 = "";
            if(string.Equals(this.InputValues.InputFileName, outputFile, StringComparison.OrdinalIgnoreCase))
            {
                string str2 = this.InputValues.InputFileName + ".bak";
                int num = 1;
                do
                {
                    str1 = str2 + (object)num;
                    ++num;
                }
                while(File.Exists(str1));
                File.Move(this.InputValues.InputFileName, str1);
            }
            try
            {
                return this.RunCore(cpu, outputFile, ressourceParam, ilSuffix);
            }
            finally
            {
                if(!string.IsNullOrEmpty(str1) && File.Exists(str1))
                {
                    File.Delete(str1);
                }
            }
        }

        private int RunCore(CpuPlatform cpu, string fileName, string ressourceParam, string ilSuffix)
        {
            string str = (string)null;
            if(!string.IsNullOrEmpty(this.InputValues.KeyFile))
            {
                str = Path.GetFullPath(this.InputValues.KeyFile);
            }
            if(!string.IsNullOrEmpty(str) && !File.Exists(str))
            {
                if(!string.IsNullOrEmpty(this.InputValues.RootDirectory) && Directory.Exists(this.InputValues.RootDirectory))
                {
                    str = Path.Combine(this.InputValues.RootDirectory, this.InputValues.KeyFile);
                }
                if(!File.Exists(str))
                {
                    throw new FileNotFoundException(string.Format(Resources.Provided_key_file_0_cannot_be_found, (object)str));
                }
            }
            string fullPath = Path.GetFullPath(Path.GetDirectoryName(fileName));
            int num = IlParser.RunIlTool(this.InputValues.FrameworkPath, "ILAsm.exe", (string)null, (string)null, "ILAsmPath", this.GetCommandLineArguments(cpu, fileName, ressourceParam, ilSuffix, str), "EXP0005", "EXP0006", this._Notifier, this.Timeout);
            if(num == 0 && !string.IsNullOrEmpty(this.InputValues.LibToolPath))
            {
                string libraryFileNameRoot = IlAsm.GetLibraryFileNameRoot(fileName);
                string defFile = this.CreateDefFile(cpu, fullPath, libraryFileNameRoot);
                try
                {
                    num = this.RunLibTool(cpu, fullPath, defFile);
                }
                finally
                {
                    if(File.Exists(defFile))
                    {
                        File.Delete(defFile);
                    }
                }
            }
            return num;
        }

        private int RunLibTool(CpuPlatform cpu, string directory, string defFileName)
        {
            string path = Path.Combine(directory, Path.GetFileNameWithoutExtension(this.InputValues.OutputFileName)) + ".lib";
            try
            {
                return IlParser.RunIlTool(this.InputValues.LibToolPath, "Lib.exe", string.IsNullOrEmpty(this.InputValues.LibToolDllPath) || !Directory.Exists(this.InputValues.LibToolDllPath) ? (string)null : this.InputValues.LibToolDllPath, (string)null, "LibToolPath", string.Format("\"/def:{0}\" /machine:{1} \"/out:{2}\"", (object)defFileName, (object)cpu, (object)path), "EXP0011", "EXP0012", this._Notifier, this.Timeout);
            }
            catch(Exception ex)
            {
                if(File.Exists(path))
                {
                    File.Delete(path);
                }
                throw;
            }
        }

        private string CreateDefFile(CpuPlatform cpu, string directory, string libraryName)
        {
            string path = Path.Combine(directory, libraryName + "." + (object)cpu + ".def");
            try
            {
                using(FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    using(StreamWriter streamWriter = new StreamWriter((Stream)fileStream, Encoding.UTF8))
                    {
                        streamWriter.WriteLine("LIBRARY {0}.dll", (object)libraryName);
                        streamWriter.WriteLine();
                        streamWriter.WriteLine("EXPORTS");
                        foreach(ExportedClass exportedClass in this.Exports.ClassesByName.Values)
                        {
                            foreach(ExportedMethod method in exportedClass.Methods)
                            {
                                streamWriter.WriteLine(method.ExportName);
                            }
                        }
                    }
                }
                return path;
            }
            catch(Exception ex)
            {
                if(File.Exists(path))
                {
                    File.Delete(path);
                }
                throw;
            }
        }

        private static string GetLibraryFileNameRoot(string fileName)
        {
            fileName = !string.Equals(Path.GetExtension(fileName).TrimStart('.'), "dll", StringComparison.InvariantCultureIgnoreCase) ? Path.GetFileName(fileName) : Path.GetFileNameWithoutExtension(fileName);
            return fileName;
        }

        private string GetCommandLineArguments(CpuPlatform cpu, string fileName, string ressourceParam, string ilSuffix, string keyFile)
        {
            return string.Format((IFormatProvider)CultureInfo.InvariantCulture, "/nologo \"/out:{0}\" \"{1}.il\" /DLL{2} {3} {4} {5}", (object)fileName, (object)(Path.Combine(this.TempDirectory, Path.GetFileNameWithoutExtension(this.InputValues.InputFileName)) + ilSuffix), (object)ressourceParam, this.InputValues.EmitDebugSymbols ? (object)"/debug" : (object)"/optimize", cpu == CpuPlatform.X86 ? (object)"" : (object)(" /PE64 " + (cpu == CpuPlatform.Itanium ? " /ITANIUM" : " /X64")), string.IsNullOrEmpty(keyFile) ? (!string.IsNullOrEmpty(this.InputValues.KeyContainer) ? (object)("\"/Key=@" + this.InputValues.KeyContainer + "\"") : (object)(string)null) : (object)("\"/Key=" + keyFile + (object)'"'));
        }
    }
}

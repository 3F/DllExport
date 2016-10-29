// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.7.38850, Culture=neutral, PublicKeyToken=8f52d83c1a22df51
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using RGiesecke.DllExport.Properties;

namespace RGiesecke.DllExport.Parsing
{
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public sealed class IlAsm: IlToolBase
    {
        private static readonly Regex _NormalizeIlErrorLineRegex = new Regex("(?:\\n|\\s|\\t|\\r|\\-|\\:|\\,)+", RegexOptions.Compiled);

        public AssemblyExports Exports
        {
            get;
            set;
        }

        public IlAsm(IServiceProvider serviceProvider, IInputValues inputValues)
        : base(serviceProvider, inputValues)
        {
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
                using(IlParser ilParser = new IlParser(this.ServiceProvider))
                {
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

                    Stream stream = null;
                    try {
                        stream = new FileStream(Path.Combine(TempDirectory, InputValues.FileName + ilSuffix + ".il"), FileMode.Create);
                        using(StreamWriter swriter = new StreamWriter(stream, Encoding.Unicode))
                        {
                            stream = null; // avoid CA2202
                            swriter.WriteLine(String.Join(Environment.NewLine, stringList));
                        }
                    }
                    finally {
                        if(stream != null) {
                            stream.Dispose();
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
            int num1 = IlParser.RunIlTool(this.InputValues.FrameworkPath, "ILAsm.exe", (string)null, (string)null, "ILAsmPath", this.GetCommandLineArguments(cpu, fileName, ressourceParam, ilSuffix, str), DllExportLogginCodes.IlAsmLogging, DllExportLogginCodes.VerboseToolLogging, this.Notifier, this.Timeout, (Func<string, bool>)(line => {
                int num2 = line.IndexOf(": ");
                if(num2 > 0)
                {
                    line = line.Substring(num2 + 1);
                }
                return IlAsm._NormalizeIlErrorLineRegex.Replace(line, "").ToLowerInvariant().StartsWith("warningnonvirtualnonabstractinstancemethodininterfacesettosuch");
            }));
            if(num1 == 0)
            {
                this.RunLibTool(cpu, fileName, fullPath);
            }
            return num1;
        }

        private int RunLibTool(CpuPlatform cpu, string fileName, string directory)
        {
            if(!InputValues.GenExpLib || String.IsNullOrWhiteSpace(InputValues.LibToolPath)) {
                return 0;
            }

            string libraryFileNameRoot  = IlAsm.GetLibraryFileNameRoot(fileName);
            string defFile              = CreateDefFile(cpu, directory, libraryFileNameRoot);

            try
            {
                return RunLibToolCore(cpu, directory, defFile);
            }
            catch(Exception ex)
            {
                Notifier.Notify(1, DllExportLogginCodes.LibToolLooging, Resources.An_error_occurred_while_calling_0_1_, "lib.exe", ex.Message);
                return -1;
            }
            finally
            {
                if(File.Exists(defFile)) {
                    File.Delete(defFile);
                }
            }
        }

        [Localizable(false)]
        private int RunLibToolCore(CpuPlatform cpu, string directory, string defFileName)
        {
            string path = Path.Combine(directory, Path.GetFileNameWithoutExtension(this.InputValues.OutputFileName)) + ".lib";
            try
            {
                return IlParser.RunIlTool(this.InputValues.LibToolPath, "Lib.exe", string.IsNullOrEmpty(this.InputValues.LibToolDllPath) || !Directory.Exists(this.InputValues.LibToolDllPath) ? (string)null : this.InputValues.LibToolDllPath, (string)null, "LibToolPath", string.Format("\"/def:{0}\" /machine:{1} \"/out:{2}\"", (object)defFileName, (object)cpu, (object)path), DllExportLogginCodes.LibToolLooging, DllExportLogginCodes.LibToolVerboseLooging, this.Notifier, this.Timeout, (Func<string, bool>)null);
            }
            catch
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
                Stream stream = null;
                try {
                    stream = new FileStream(path, FileMode.Create);
                    using(StreamWriter swriter = new StreamWriter(stream, Encoding.UTF8))
                    {
                        stream = null; // avoid CA2202

                        var data = new List<string>()
                        {
                            $"LIBRARY {libraryName}.dll",
                            "",
                            "EXPORTS"
                        };

                        data.AddRange(Exports.ClassesByName.Values
                                                            .SelectMany(c =>
                                                                c.Methods.Select(m => m.ExportName)));

                        swriter.WriteLine(String.Join(Environment.NewLine, data));
                    }
                }
                finally {
                    if(stream != null) {
                        stream.Dispose();
                    }
                }

                return path;
            }
            catch
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

        [Localizable(false)]
        private string GetCommandLineArguments(CpuPlatform cpu, string fileName, string ressourceParam, string ilSuffix, string keyFile)
        {
            if(String.IsNullOrEmpty(keyFile)) {
                keyFile = String.IsNullOrEmpty(InputValues.KeyContainer) ? null : "\"/Key=@" + InputValues.KeyContainer + "\"";
            }
            else {
                keyFile = "\"/Key=" + keyFile + '"';
            }

            return String.Format(
                CultureInfo.InvariantCulture, 
                "/nologo \"/out:{0}\" \"{1}.il\" {2} {3} {4} {5} {6}", 
                fileName, 
                Path.Combine(TempDirectory, Path.GetFileNameWithoutExtension(InputValues.InputFileName)) + ilSuffix,
                "/" + Path.GetExtension(fileName).Trim(new char[] { '.', '"' }).ToUpperInvariant(),
                ressourceParam, 
                InputValues.EmitDebugSymbols ? "/debug" : "/optimize", 
                cpu == CpuPlatform.X86 ? "" : (" /PE64 " + (cpu == CpuPlatform.Itanium ? " /ITANIUM" : " /X64")),
                keyFile
             );
        }
    }
}

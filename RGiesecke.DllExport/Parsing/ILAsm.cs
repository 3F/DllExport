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

            int ret = IlParser.RunIlTool
            (
                String.IsNullOrWhiteSpace(InputValues.OurILAsmPath) ? InputValues.FrameworkPath : InputValues.OurILAsmPath,
                "ilasm.exe", 
                null, 
                null, 
                "ILAsmPath", 
                GetCommandLineArguments(cpu, fileName, ressourceParam, ilSuffix, str), 
                DllExportLogginCodes.IlAsmLogging, 
                DllExportLogginCodes.VerboseToolLogging, 
                Notifier, 
                Timeout, 
                line =>
                {
                    int col = line.IndexOf(": ");
                    if(col > 0) {
                        line = line.Substring(col + 1);
                    }

                    return IlAsm
                            ._NormalizeIlErrorLineRegex
                            .Replace(line, "")
                            .ToLowerInvariant()
                            .StartsWith("warningnonvirtualnonabstractinstancemethodininterfacesettosuch");
                }
            );

            if(ret == 0) {
                RunLibTool(cpu, fileName, Path.GetFullPath(Path.GetDirectoryName(fileName)));
            }
            return ret;
        }

        private int RunLibTool(CpuPlatform cpu, string fileName, string directory)
        {
            if(!InputValues.GenExpLib) {
                return 0;
            }

            string libFileRoot  = IlAsm.GetLibraryFileNameRoot(fileName);
            string defFile      = CreateDefFile(cpu, directory, libFileRoot);
            string path         = Path.Combine(directory, Path.GetFileNameWithoutExtension(InputValues.OutputFileName)) + ".lib";
            string cfg          = $"\"/def:{defFile}\" /machine:{cpu} \"/out:{path}\"";

            try
            {
                if(!String.IsNullOrWhiteSpace(InputValues.VsDevCmd))
                {
                    int code = RunLibToolCore(
                        "cmd", 
                        $"/C \"\"{InputValues.VsDevCmd}\" -no_logo -arch={(cpu == CpuPlatform.X64 ? "amd64" : "x86")} && lib.exe {cfg}\""
                    );

                    Notifier.Notify(-1, DllExportLogginCodes.LibToolLooging, $"lib tool via VsDevCmd: {code}");
                    if(code == 0) {
                        return code;
                    }
                }

                if(!String.IsNullOrWhiteSpace(InputValues.LibToolPath))
                {
                    string reqPath = (String.IsNullOrEmpty(InputValues.LibToolDllPath) || !Directory.Exists(InputValues.LibToolDllPath)) ? null : InputValues.LibToolDllPath;
                    int code = RunLibToolCore("Lib.exe", cfg, InputValues.LibToolPath, reqPath);

                    Notifier.Notify(-1, DllExportLogginCodes.LibToolLooging, $"lib tool via LibToolPath: {code}");
                    if(code == 0) {
                        return code;
                    }
                }

                if(!String.IsNullOrWhiteSpace(InputValues.VcVarsAll))
                {
                    int code = RunLibToolCore(
                        "cmd",
                        $"/C \"\"{InputValues.VcVarsAll}\" {(cpu == CpuPlatform.X64 ? "x64" : "x86")} && lib.exe {cfg}\""
                    );

                    Notifier.Notify(-1, DllExportLogginCodes.LibToolLooging, $"lib tool via VcVarsAll: {code}");
                    if(code == 0) {
                        return code;
                    }
                }

                int ret = RunLibToolCore("lib.exe", cfg, String.Empty, InputValues.LibToolDllPath);
                Notifier.Notify(0, DllExportLogginCodes.LibToolLooging, $"lib tool via LibToolDllPath: {ret}");
                if(ret != -1) {
                    return ret;
                }

                throw new Exception();
            }
            catch(Exception ex)
            {
                if(File.Exists(path)) {
                    File.Delete(path);
                }
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
        private int RunLibToolCore(string tool, string args, string path = "", string reqPath = null)
        {
            try
            {
                return IlParser.RunIlTool
                (
                    path,
                    tool,
                    reqPath, 
                    null, 
                    "LibToolPath", 
                    args, 
                    DllExportLogginCodes.LibToolLooging, 
                    DllExportLogginCodes.LibToolVerboseLooging, 
                    Notifier, 
                    Timeout, 
                    null
                );
            }
            catch(Exception ex) {
                Notifier.Notify(-1, DllExportLogginCodes.LibToolLooging, Resources.An_error_occurred_while_calling_0_1_, $" {tool} {args} ", ex.Message);
                return -1;
            }
        }

        private string CreateDefFile(CpuPlatform cpu, string directory, string libraryName)
        {
            string path = Path.Combine(directory, libraryName + "." + cpu + ".def");
            try
            {
                using(var swriter = new StreamWriter(path, false, Encoding.UTF8))
                {
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

            string cvtres;
            if(!String.IsNullOrWhiteSpace(InputValues.OurILAsmPath)) {
                // https://github.com/3F/coreclr/issues/2
                // Only our new ILAsm 4.5.1+ may detect cvtres.exe automatically if the path is not presented at all. However, we can also provide CVR key
                cvtres = String.IsNullOrWhiteSpace(InputValues.FrameworkPath) ? "" : $"/CVRES=\"{InputValues.FrameworkPath}/\"";
            }
            else {
                cvtres = String.Empty; // original coreclr \ ILAsm does not support /CVRES
            }

            return String.Format(
                CultureInfo.InvariantCulture, 
                "/nologo \"/out:{0}\" \"{1}.il\" {2} {3} {4} {5} {6} {7}", 
                fileName, 
                Path.Combine(TempDirectory, Path.GetFileNameWithoutExtension(InputValues.InputFileName)) + ilSuffix,
                "/" + Path.GetExtension(fileName).Trim(new char[] { '.', '"' }).ToUpperInvariant(),
                ressourceParam, 
                InputValues.EmitDebugSymbols ? "/debug" : "/optimize", 
                cpu == CpuPlatform.X86 ? "" : (" /PE64 " + (cpu == CpuPlatform.Itanium ? " /ITANIUM" : " /X64")),
                keyFile,
                cvtres
             );
        }
    }
}

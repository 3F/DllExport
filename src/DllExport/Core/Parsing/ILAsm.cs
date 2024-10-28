/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;

namespace net.r_eg.DllExport.Parsing
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
            Directory.SetCurrentDirectory(TempDirectory);

            try
            {
                string directoryName = Path.GetDirectoryName(outputFile);
                if(directoryName != null && !Directory.Exists(directoryName)) {
                    Directory.CreateDirectory(directoryName);
                }

                using(IlParser ilParser = new IlParser(ServiceProvider)) {
                    ReassembleFile(ilParser, ilSuffix, cpu);
                }

                return Run(outputFile, ilSuffix, cpu);
            }
            finally
            {
                Directory.SetCurrentDirectory(currentDirectory);
            }
        }

        private void ReassembleFile(IlParser ilParser, string ilSuffix, CpuPlatform cpu)
        {
            ilParser.Exports        = Exports;
            ilParser.InputValues    = InputValues;
            ilParser.TempDirectory  = TempDirectory;

            var stringList = new List<string>(ilParser.GetLines(cpu));
            if(stringList.Count > 0)
            {
                string input = stringList[stringList.Count - 1];

                if(!input.EndsWith("\\r") && !input.EndsWith("\\n"))
                {
                    stringList[stringList.Count - 1] = input + Environment.NewLine;
                }
            }

            var dest = Path.Combine(TempDirectory, InputValues.FileName + ilSuffix + ".il");

            using(var swriter = new StreamWriter(dest, false, Encoding.Unicode)) {
                swriter.WriteLine(String.Join(Environment.NewLine, stringList));
            }
        }

        private int Run(string outputFile, string ilSuffix, CpuPlatform cpu)
        {
            StringBuilder stringBuilder = new StringBuilder(100);
            foreach(string file in Directory.GetFiles(this.TempDirectory, "*.res"))
            {
                // TODO: ".  res" o_O
                if(string.Equals(Path.GetExtension(file)?.Trim('.'), "res", StringComparison.OrdinalIgnoreCase))
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

            // https://github.com/3F/coreclr/blob/05afa4f81fdf671429b54467c64d65cde6b5fadc/src/debug/ildbsymlib/symwrite.cpp#L308
            // Due to possible incorrect ISymUnmanagedWriter when exists initial pdb data for non-modified asm.
            // \- Part of https://github.com/3F/DllExport/issues/90
            File.Delete(Path.ChangeExtension(InputValues.InputFileName, ".pdb"));

            try {
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

            if(ret != 0) {
                return ret;
            }

            ret = CheckPE(cpu, fileName);
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
                Notifier.Notify(-1, DllExportLogginCodes.LibToolLooging, $"VsDevCmd: {InputValues.VsDevCmd}");
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

                Notifier.Notify(-1, DllExportLogginCodes.LibToolLooging, $"LibToolPath: {InputValues.LibToolPath}");
                if(!String.IsNullOrWhiteSpace(InputValues.LibToolPath))
                {
                    string reqPath = (String.IsNullOrEmpty(InputValues.LibToolDllPath) || !Directory.Exists(InputValues.LibToolDllPath)) ? null : InputValues.LibToolDllPath;
                    int code = RunLibToolCore("Lib.exe", cfg, InputValues.LibToolPath, reqPath);

                    Notifier.Notify(-1, DllExportLogginCodes.LibToolLooging, $"lib tool via LibToolPath: {code}");
                    if(code == 0) {
                        return code;
                    }
                }

                Notifier.Notify(-1, DllExportLogginCodes.LibToolLooging, $"VcVarsAll: {InputValues.VcVarsAll}");
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
                Notifier.Notify(0, DllExportLogginCodes.LibToolLooging, $"lib tool via LibToolDllPath '{InputValues.LibToolDllPath}': {ret}");
                if(ret != -1) {
                    return ret;
                }

                throw new FileNotFoundException("The library manager still cannot be found or something went wrong.");
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

        /// <param name="cpu"></param>
        /// <param name="file">Modified PE-file.</param>
        private int CheckPE(CpuPlatform cpu, string pefile)
        {
            int ret = 0;

            if(InputValues.PeCheck == PeCheckType.None) {
                return ret;
            }

            using(var pe = new net.r_eg.Conari.PE.PEFile(pefile))
            {
                var ilMethods = Exports.ClassesByName.Values
                                                    .SelectMany(c => c.Methods.Select(m => m.ExportName))
                                                    .ToArray();

                var peMethods = pe.ExportedProcNames.ToArray();

                if((InputValues.PeCheck & PeCheckType.Pe1to1) == PeCheckType.Pe1to1)
                {
                    Notifier.Notify(-2, DllExportLogginCodes.PeCheck1to1, $"{nameof(PeCheckType.Pe1to1)} is activated.");
                    if(!CheckPE1to1(ilMethods, peMethods, pefile)) {
                        ret = -1;
                    }
                }

                if((InputValues.PeCheck & PeCheckType.PeIl) == PeCheckType.PeIl)
                {
                    Notifier.Notify(-2, DllExportLogginCodes.PeCheckIl, $"{nameof(PeCheckType.PeIl)} is activated.");
                    if(!CheckPEIl(ilMethods, peMethods, pefile)) {
                        ret = -1;
                    }
                }
            }

            return ret;
        }

        private bool CheckPE1to1(string[] ilMethods, string[] peMethods, string pefile)
        {
            if(ilMethods.Length == peMethods.Length) {
                return true;
            }

            Notifier.Notify(
                2, 
                DllExportLogginCodes.PeCheck1to1, 
                $"The number ({peMethods.Length}) of exports from PE32/PE32+ module '{pefile}' is not equal to number ({ilMethods.Length}) from IL code."
            );
            return false;
        }

        private bool CheckPEIl(string[] ilMethods, string[] peMethods, string pefile)
        {
            var notFound = ilMethods.Except(peMethods).ToArray();

            if(notFound.Length < 1) {
                return true;
            }

            Notifier.Notify(
                2, 
                DllExportLogginCodes.PeCheckIl, 
                $"Something went wrong. We can't find '{notFound.Length}' exports from PE32/PE32+ module '{pefile}': {String.Join(" ; ", notFound)}"
            );
            return false;
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

            return string.Format
            (
                CultureInfo.InvariantCulture, 
                "/nologo \"/out:{0}\" \"{1}.il\" {2} {3} {4} {5} {6} {7}", 
                fileName, 
                Path.Combine(TempDirectory, Path.GetFileNameWithoutExtension(InputValues.InputFileName)) + ilSuffix,
                "/" + Path.GetExtension(fileName).Trim(new char[] { '.', '"' }).ToUpperInvariant(),
                ressourceParam, 
                InputValues.EmitDebugSymbols ? "/debug" : "/optimize", 
                cpu == CpuPlatform.X86 ? "" : (" /PE64 " + (cpu == CpuPlatform.Itanium ? " /ITANIUM" : " /X64")),
                keyFile,
                GetKeysToCustomILAsm()
             );
        }

        /// <returns>
        /// Keys to modified coreclr \ ILAsm 
        /// </returns>
        private string GetKeysToCustomILAsm()
        {
            if(string.IsNullOrWhiteSpace(InputValues.OurILAsmPath))
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            // https://github.com/3F/coreclr/issues/2
            // Our custom ILAsm 4.5.1+ may automatically detect cvtres if the path is not presented at all
            if(!string.IsNullOrWhiteSpace(InputValues.FrameworkPath)) 
            {
                sb.Append($" /CVRES=\"{InputValues.FrameworkPath}/\"");
            }

            if(InputValues.SysObjRebase)
            {
                sb.Append(" /REBASE");
            }

            return sb.ToString();
        }
    }
}

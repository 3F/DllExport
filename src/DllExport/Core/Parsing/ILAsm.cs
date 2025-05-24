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
            StringBuilder resource = new(100);
            foreach(string rfile in Directory.GetFiles(TempDirectory, "*.res"))
            {
                // TODO: ".  res" o_O
                if(string.Equals(Path.GetExtension(rfile)?.Trim('.'), "res", StringComparison.OrdinalIgnoreCase))
                {
                    resource.Append($" \"/resource={rfile}\" ");
                }
            }

            return PrepareOutput(outputFile, InputValues.InputFileName, f =>
            {
                return FixPdb(f, () => RunCore(cpu, outputFile, resource.ToString(), ilSuffix));
            });
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
                InputValues.IsILAsmDefault ? InputValues.FrameworkPath : InputValues.OurILAsmPath,
                "ilasm.exe", 
                requiredPaths: null,
                workingDirectory: null,
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

                    return _NormalizeIlErrorLineRegex
                            .Replace(line, string.Empty)
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
                    workingDirectory: null,
                    args, 
                    DllExportLogginCodes.LibToolLooging, 
                    DllExportLogginCodes.LibToolVerboseLooging, 
                    Notifier, 
                    Timeout
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
            if(InputValues.PeCheck == PeCheckType.None) return ret;

            using Conari.PE.PEFile pe = new(pefile);

            if((InputValues.PeCheck & (PeCheckType.Pe1to1 | PeCheckType.PeIl)) != 0)
            {
                string[] ilMethods = Exports.ClassesByName.Values
                                    .SelectMany(c => c.Methods.Select(m => m.ExportName))
                                    .ToArray();

                string[] peMethods = pe.DExport.NumberOfFunctions > 0 ? pe.ExportedProcNames.ToArray() : [];

                if((InputValues.PeCheck & PeCheckType.Pe1to1) == PeCheckType.Pe1to1)
                {
                    Notifier.NotifyLow(DllExportLogginCodes.PeCheck1to1, Resources._0_is_activated, nameof(PeCheckType.Pe1to1));
                    if(!CheckPe1to1(ilMethods, peMethods, pe)) ret = -1;
                }

                if((InputValues.PeCheck & PeCheckType.PeIl) == PeCheckType.PeIl)
                {
                    Notifier.NotifyLow(DllExportLogginCodes.PeCheckIl, Resources._0_is_activated, nameof(PeCheckType.PeIl));
                    if(!CheckPeIl(ilMethods, peMethods, pe)) ret = -1;
                }
            }

            if((InputValues.PeCheck & PeCheckType.Pe32orPlus) == PeCheckType.Pe32orPlus)
            {
                Notifier.NotifyLow(DllExportLogginCodes.PeCheck32orPlus, Resources._0_is_activated, nameof(PeCheckType.Pe32orPlus));
                if(!CheckPe32orPlus(cpu, pe)) ret = -1;
            }

            return ret;
        }

        private bool CheckPe1to1(string[] ilMethods, string[] peMethods, Conari.PE.PEFile pe)
        {
            if(ilMethods.Length == peMethods.Length) return true;

            Notifier.NotifyError
            (
                DllExportLogginCodes.PeCheck1to1, 
                $"The number ({peMethods.Length}) of exports from PE32/PE32+ module '{pe.FileName}' is not equal to number ({ilMethods.Length}) from IL code."
            );
            return false;
        }

        private bool CheckPeIl(string[] ilMethods, string[] peMethods, Conari.PE.PEFile pe)
        {
            IEnumerable<string> notFound = ilMethods.Except(peMethods);

            if(!notFound.Any()) return true;

            Notifier.NotifyError
            (
                DllExportLogginCodes.PeCheckIl, 
                $"Lost '{notFound.Count()}' entries from {pe.Magic} module '{pe.FileName}': {string.Join(" ; ", notFound.ToArray())}"
            );
            return false;
        }

        private bool CheckPe32orPlus(CpuPlatform cpu, Conari.PE.PEFile pe)
        {
            if((pe.Magic == Conari.PE.WinNT.Magic.PE64 && cpu == CpuPlatform.X64)
                || (pe.Magic == Conari.PE.WinNT.Magic.PE32 && cpu == CpuPlatform.X86))
            {
                return true;
            }

            Notifier.NotifyError
            (
                DllExportLogginCodes.PeCheck32orPlus, 
                $"Module '{pe.FileName}' is '{pe.Magic}' while configuration is '{cpu}'"
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
                "/" + Path.GetExtension(fileName).Trim(['.', '"']).ToUpperInvariant(),
                ressourceParam,
                GetKeysToDebug(InputValues.EmitDebugSymbols), 
                cpu == CpuPlatform.X86 ? "" : (" /PE64 " + (cpu == CpuPlatform.Itanium ? " /ITANIUM" : " /X64")),
                keyFile,
                GetKeysToCustomILAsm()
             );
        }

        /// <returns>
        /// Keys to modified coreclr \ ILAsm 
        /// </returns>
        [Localizable(false)]
        private string GetKeysToCustomILAsm()
        {
            if(InputValues.IsILAsmDefault) return string.Empty;

            StringBuilder sb = new();

#if F_ILASM_CVRES_USE_FX // 3F's ILAsm 9.3+ fully automates the process of choosing the suitable converter

            // https://github.com/3F/coreclr/issues/2
            if(!string.IsNullOrWhiteSpace(InputValues.FrameworkPath)) 
            {
                sb.Append($" /CVRES=\"{InputValues.FrameworkPath}/\"");
            }
#endif

            if(InputValues.SysObjRebase)
            {
                sb.Append(" /REBASE");
            }

            return sb.ToString();
        }

        [Localizable(false)]
        private string GetKeysToDebug(DebugType type)
        {
            return type switch
            {
                DebugType.Debug => "/DEBUG",
                DebugType.Optimize => "/OPTIMIZE",
                DebugType.PdbOptimize => "/PDB /OPTIMIZE",
                DebugType.DebugOptimize => "/DEBUG=OPT",
                DebugType.DebugImpl => "/DEBUG=IMPL",
                _ => throw new NotImplementedException()
            };
        }

        private T FixPdb<T>(string inputModule, Func<T> action) where T : struct
        {
            if(InputValues.IsILAsmDefault)
            {
                string pdb = Path.ChangeExtension(inputModule, ".pdb");
                // The original (netfx-based) assembler cannot handle the BSJB format if it exists for some reason; "error : Failed to define a document writer" /F-378
                // The error can be avoided either by converting to MSF ( https://github.com/3F/coreclr/issues/3#issuecomment-2845660889 ) or delete to regenerate it from scratch;
                // but regenerated MSF can be still problematic for user's DebugType=portable, thus warn about it
                if(IsPdbBSJB(pdb)) Notifier.WarnAndRun
                (
                    "DllExportSupressWarnBSJB",
                    DllExportLogginCodes.InvalidPdb,
                    "Possibly invalid PDB. Check the DebugType or use a different assembler",
                    () => File.Delete(pdb)
                );
            }
            return action?.Invoke() ?? default;
        }

        private bool IsPdbBSJB(string pdb)
        {
            if(!File.Exists(pdb)) return false;

            using FileStream fs = new(pdb, FileMode.Open, FileAccess.Read);
            using BinaryReader br = new(fs);

            return br.ReadInt32() == 0x424A5342;
        }

        private T PrepareOutput<T>(string fullpath, string src, Func<string, T> action) where T: struct
        {
            string bak = null;
            try
            {
                if(string.Equals(src, fullpath, StringComparison.OrdinalIgnoreCase))
                {
                    int num = 1;
                    do
                    {
                        bak = $"{src}.bak{num}";
                        ++num;
                    }
                    while(File.Exists(bak));
                    File.Move(src, bak);
                }

                return action?.Invoke(src) ?? default;
            }
            finally
            {
                if(bak != null && File.Exists(bak)) File.Delete(bak);
            }
        }
    }
}

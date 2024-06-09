/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Reflection;
using System.Text;

namespace net.r_eg.DllExport.Wizard.UI
{
    internal sealed class SimpleConfFormater: IConfFormater
    {
        private readonly IExecutor exec;

        public string Parse(IProject prj)
        {
            if(prj == null) {
                return string.Empty;
            }

            var sb = new StringBuilder(45);
            sb.AppendLine("```"); // for default reports on github issue tracker because some users don't know
            sb.AppendLine($"Installed: {prj.Installed}; {DllExportVersion.S_INFO_P}; invoked: {prj.Config.Wizard.PkgVer}");
            sb.AppendLine($"Project type: {prj.XProject.ProjectItem.project.EpType}");
            sb.AppendLine($"Storage: {prj.Config.Wizard.CfgStorage}");
            sb.AppendLine($"Compiler.Platform: {prj.Config.Platform}");
            sb.AppendLine($"Compiler.ordinalsBase: {prj.Config.Compiler.ordinalsBase}");
            sb.AppendLine($"Compiler.rSysObj: {prj.Config.Compiler.rSysObj}");
            sb.AppendLine($"Compiler.ourILAsm: {prj.Config.Compiler.ourILAsm}");
            sb.AppendLine($"Compiler.customILAsm: {prj.Config.Compiler.customILAsm}");
            sb.AppendLine($"Compiler.genExpLib: {prj.Config.Compiler.genExpLib}");
            sb.AppendLine($"Compiler.peCheck: {prj.Config.Compiler.peCheck}");
            sb.AppendLine($"Compiler.patches: {prj.Config.Compiler.patches}");
            sb.AppendLine($"PreProc.Type: {prj.Config.PreProc.Type}");
            sb.AppendLine($"PreProc.Cmd: {prj.Config.PreProc.Cmd}");
            sb.AppendLine($"PostProc.Type: {prj.Config.PostProc.Type}");
            sb.AppendLine($"PostProc.ProcEnv: {prj.Config.PostProc.GetProcEnvAsString()}");
            sb.AppendLine($"PostProc.Cmd: {prj.Config.PostProc.Cmd}");
            sb.AppendLine($"SignAssembly: {prj.XProject.GetProperty("SignAssembly").unevaluated}");
            sb.AppendLine($"Identifier: {prj.DxpIdent}");
            sb.AppendLine($"Instance: {Assembly.GetEntryAssembly().Location}");
            sb.AppendLine($"Project path: {prj.XProject.ProjectFullPath}");
            sb.AppendLine($"Action: {prj.Config.Wizard.Type}");
            sb.AppendLine($"PlatformTarget: {prj.XProject.GetProperty("PlatformTarget").unevaluated}");
            sb.AppendLine($"TargetFramework: {prj.XProject.GetProperty("TargetFramework").unevaluated}");
            sb.AppendLine($"TargetFrameworks: {prj.XProject.GetProperty("TargetFrameworks").unevaluated}");
            sb.AppendLine($"TargetFrameworkVersion: {prj.XProject.GetProperty("TargetFrameworkVersion").unevaluated}");
            sb.AppendLine($"RootNamespace: {prj.XProject.GetProperty("RootNamespace").unevaluated}");
            sb.AppendLine($"AssemblyName: {prj.XProject.GetProperty("AssemblyName").unevaluated}");
            sb.AppendLine($"MgrArgs: {prj.Config.Wizard.MgrArgs}");
            sb.AppendLine($"MetaLib: {prj.Config.Wizard.MetaLib}");
            sb.AppendLine($"MetaCor: {prj.Config.Wizard.MetaCor}");
            sb.AppendLine($"Proxy: {prj.Config.Wizard.Proxy}");
            sb.AppendLine($"StoragePath: {prj.Config.Wizard.StoragePath}");
            sb.AppendLine($"ddNS: {prj.Config.Namespace}");
            sb.AppendLine($"ddNS max buffer: {prj.Config.NSBuffer}");
            sb.AppendLine($"UseCecil: {prj.Config.UseCecil}");
            sb.AppendLine($"intermediateFiles: {prj.Config.Compiler.intermediateFiles}");
            sb.AppendLine($"timeout: {prj.Config.Compiler.timeout}");
            sb.AppendLine($"Options: {prj.Config.Wizard.Options}");
            sb.AppendLine($"RootPath: {prj.Config.Wizard.RootPath}");
            sb.AppendLine($"PkgPath: {prj.Config.Wizard.PkgPath}");
            sb.AppendLine($"SlnFile: {prj.Config.Wizard.SlnFile}");
            sb.AppendLine($"SlnDir: {prj.Config.Wizard.SlnDir}");
            sb.AppendLine($"DxpTarget: {prj.Config.Wizard.DxpTarget}");
            sb.AppendLine($"MsgGuiLevel: {prj.Config.Wizard.MsgGuiLevel}");
            sb.AppendLine($"LockIfError: {prj.InternalError}");
            sb.AppendLine("```"); //<

            return sb.ToString();
        }

        public SimpleConfFormater(IExecutor exec)
        {
            this.exec = exec ?? throw new ArgumentNullException(nameof(exec));
        }
    }
}

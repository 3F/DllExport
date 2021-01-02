/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2021  Denis Kuzmin <x-3F@outlook.com> github/3F
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
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
            sb.AppendLine($"SignAssembly: {prj.XProject.GetProperty("SignAssembly").unevaluatedValue}");
            sb.AppendLine($"Identifier: {prj.DxpIdent}");
            sb.AppendLine($"Instance: {Assembly.GetEntryAssembly().Location}");
            sb.AppendLine($"Project path: {prj.XProject.ProjectFullPath}");
            sb.AppendLine($"Action: {prj.Config.Wizard.Type}");
            sb.AppendLine($"PlatformTarget: {prj.XProject.GetProperty("PlatformTarget").unevaluatedValue}");
            sb.AppendLine($"TargetFramework: {prj.XProject.GetProperty("TargetFramework").unevaluatedValue}");
            sb.AppendLine($"TargetFrameworks: {prj.XProject.GetProperty("TargetFrameworks").unevaluatedValue}");
            sb.AppendLine($"TargetFrameworkVersion: {prj.XProject.GetProperty("TargetFrameworkVersion").unevaluatedValue}");
            sb.AppendLine($"RootNamespace: {prj.XProject.GetProperty("RootNamespace").unevaluatedValue}");
            sb.AppendLine($"AssemblyName: {prj.XProject.GetProperty("AssemblyName").unevaluatedValue}");
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

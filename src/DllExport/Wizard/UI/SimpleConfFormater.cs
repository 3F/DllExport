/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using net.r_eg.DllExport.ILAsm;
using net.r_eg.MvsSln.Extensions;

namespace net.r_eg.DllExport.Wizard.UI
{
    internal class SimpleConfFormater(IExecutor exec): IConfFormater
    {
        protected readonly IExecutor exec = exec ?? throw new ArgumentNullException(nameof(exec));

        protected Dictionary<string, string> cache = [];

        public string ParseIfNeeded(IProject prj, Action onParse = null)
        {
            if(prj == null) return string.Empty;

            string id = prj.DxpIdent;
            if(!cache.ContainsKey(id))
            {
                onParse?.Invoke();
                cache[id] = Parse(prj);
            }
            return cache[id];
        }

        public string Parse(IProject prj)
        {
            if(prj == null) return string.Empty;

            return GetFrame(sb =>
            {
                sb.AppendLine($"Installed: {prj.Installed}; {DllExportVersion.S_INFO_FULL}; invoked: {prj.Config.Wizard.PkgVer}");
                sb.AppendLine($"Project type: {prj.XProject.ProjectItem.project.EpType}");
                sb.AppendLine($"Storage: {prj.Config.Wizard.CfgStorage}");
                sb.AppendLine($"Compiler.Platform: {prj.Config.Platform}");
                sb.AppendLine($"Compiler.imageBase: {prj.Config.Compiler.imageBase}");
                sb.AppendLine($"Compiler.imageBaseStep: {prj.Config.Compiler.imageBaseStep}");
                sb.AppendLine($"Compiler.ordinalsBase: {prj.Config.Compiler.ordinalsBase}");
                sb.AppendLine($"Compiler.rSysObj: {prj.Config.Compiler.rSysObj}");
                sb.AppendLine($"Compiler.ourILAsm: {prj.Config.Compiler.ourILAsm}");
                sb.AppendLine($"Compiler.customILAsm: {prj.Config.Compiler.customILAsm}");
                sb.AppendLine($"Compiler.genExpLib: {prj.Config.Compiler.genExpLib}");
                sb.AppendLine($"Compiler.peCheck: {prj.Config.Compiler.peCheck}");
                sb.AppendLine($"Compiler.patches: {prj.Config.Compiler.patches}");
                sb.AppendLine($"Compiler.refreshObj: {prj.Config.Compiler.refreshObj}");
                sb.AppendLine($"PreProc.Type: {prj.Config.PreProc.Type}");
                sb.AppendLine($"PreProc.Cmd: {prj.Config.PreProc.Cmd}");
                sb.AppendLine($"PostProc.Type: {prj.Config.PostProc.Type}");
                sb.AppendLine($"PostProc.ProcEnv: {prj.Config.PostProc.GetProcEnvAsString()}");
                sb.AppendLine($"PostProc.Cmd: {prj.Config.PostProc.Cmd}");
                sb.AppendLine($"AssemblyExternDirectives: {prj.Config.AssemblyExternDirectives.Serialize()}");
                sb.AppendLine($"TypeRefDirectives: {prj.Config.TypeRefDirectives.Serialize()}");
                sb.AppendLine($"TypeRefOptions: {prj.Config.TypeRefOptions}");
                sb.AppendLine($"RefPackages: {prj.Config.RefPackages.Serialize()}");
                Append
                (
                    exec.ActiveEnv.Sln.ProjectItemsConfigs
                        .Where(p =>
                            (p.projectConfig.IncludeInBuild || p.projectConfig.IncludeInDeploy)
                                && p.project == prj.XProject.ProjectItem.project
                        )
                        .Select(p => p.projectConfig.Configuration).Distinct(),
                    [
                            "SignAssembly",
                            "PlatformTarget",
                            "TargetFramework",
                            "TargetFrameworks",
                            "TargetFrameworkVersion",
                            "TargetFrameworkIdentifier",
                            "RootNamespace",
                            "AssemblyName",
                            "DebugType",
                            "Optimize",
                            "DebugSymbols"
                    ],
                    prj, sb
                );
                sb.AppendLine($"Identifier: {prj.DxpIdent}");
                sb.AppendLine($"Instance: {Assembly.GetEntryAssembly().Location}");
                sb.AppendLine($"Project path: {prj.XProject.ProjectFullPath}");
                sb.AppendLine($"Action: {prj.Config.Wizard.Type}");
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
                sb.AppendLine($"MsgLevel: {prj.Config.Wizard.MsgLevelLimit}");
                sb.AppendLine($"LockIfError: {prj.InternalError}");
            });
        }

        protected virtual string GetFrame(Action<StringBuilder> builder, int capacity = 4096)
        {
            StringBuilder sb = new(capacity);

            sb.AppendLine("```"); // for default reports on github issue tracker because some users don't know
            builder?.Invoke(sb);
            sb.AppendLine("```");

            return sb.ToString();
        }

        private static void Use(string configuration, IProject prj, Action<IProject> action)
        {
            string origin = prj.XProject.Project.GlobalProperties.GetOrDefault(MvsSln.PropertyNames.CONFIG);
            bool mod = false;

            if(configuration == null && origin != null)
            {
                prj.XProject.Project.RemoveGlobalProperty(MvsSln.PropertyNames.CONFIG);
                mod = true;
            }
            else if(configuration != origin)
            {
                prj.XProject.Project.SetGlobalProperty(MvsSln.PropertyNames.CONFIG, configuration);
                mod = true;
            }

            if(mod) prj.XProject.Reevaluate();
            action?.Invoke(prj);

            if(!mod) return;

            if(origin == null)
            {
                prj.XProject.Project.RemoveGlobalProperty(MvsSln.PropertyNames.CONFIG);
            }
            else
            {
                prj.XProject.Project.SetGlobalProperty(MvsSln.PropertyNames.CONFIG, origin);
            }
            prj.XProject.Reevaluate();
        }

        private static void Append(IEnumerable<string> configurations, string[] properties, IProject prj, StringBuilder sb)
        {
            bool added = false;
            foreach(string conf in configurations)
            {
                Use(conf, prj, _ =>
                {
                    Append(properties, prj, sb);
                });
                added = true;
            }

            if(!added) Append(properties, prj, sb);
        }

        private static void Append(string[] properties, IProject prj, StringBuilder sb)
        {
            string config = prj.XProject.Project.GlobalProperties.GetOrDefault(MvsSln.PropertyNames.CONFIG);

            foreach(string p in properties) sb.AppendLine($"{p}({config}): {GetRaw(p, prj)}");
        }

        private static string GetRaw(string property, IProject prj) => prj.XProject.GetProperty(property).unevaluated;
    }
}

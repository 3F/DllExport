/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.Construction;
using net.r_eg.DllExport.Wizard.Extensions;
using net.r_eg.MvsSln.Extensions;
using static net.r_eg.DllExport.Wizard.PreProc;

namespace net.r_eg.DllExport.Wizard.Gears
{
    internal sealed class PreProcGear(IProjectSvc prj): ProjectGearAbstract(prj)
    {
        private const string ILMERGE_TMP = ".ilm0";

        private readonly Dictionary<CmdType, _ToolOrLib> mTool = new()
        {
            { CmdType.ILMerge, new("ilmerge", new("3.0.41"), "$(ILMergeConsolePath)") },
            { CmdType.ILRepack, new("ILRepack", new("2.0.43"), "$(ILRepack)") },
            { CmdType.Conari, new("Conari", new("1.5.0")) },
        };

        public override void Install()
        {
            CfgPreProc(Config.PreProc.Type);

            // Since CmdType can remove or add any feature at the same time,
            XProject.RemoveEmptyPropertyGroups(); // we'll also need to release its possible empty container
        }

        public override void Uninstall(bool hardReset)
        {
            RemovePreProcTarget(hardReset);
            XProject.RemovePropertyGroups(p => p.Label == ID);

            foreach(_ToolOrLib tool in mTool.Values)
            {
                prj.RemovePackageReferences(tool.name);
            }
            prj.RemovePackageReferences(i => i.evaluated != UserConfig.PKG_ID);
        }

        private void CfgPreProc(CmdType type)
        {
            prj.SetProperty(MSBuildProperties.DXP_PRE_PROC_TYPE, (long)type);
            Log.send(this, $"Pre-Processing type: {type}");

            if(type == CmdType.None) return;

            _FxCorArgBuilder sb = new(capacity: 100);

            if((type & CmdType.AnyMerging) != 0)
                sb.AppendBoth(Path.Combine(ILMERGE_TMP, "$(TargetName).dll")); //F-315, keep it first

            if(type.HasFlag(CmdType.Conari))
            {
                _ToolOrLib lib = GetLib(CmdType.Conari);
                Log.send(this, $"Merge {lib.name}: {lib.version}");
                XProject.AddPackageIfNotExists(lib.name, $"{lib.version}", prj.GetMeta(privateAssets: true));
                sb.AppendBoth(lib.module);

#if F_CONARI_ADD_SYS_DLL
                sb.AppendCor("Microsoft.CSharp.dll System.Reflection.Emit.dll");
                sb.AppendCor("System.Reflection.Emit.ILGeneration.dll System.Reflection.Emit.Lightweight.dll");
#endif
            }

            if(type.HasFlag(CmdType.MergeRefPkg))
            {
                foreach(RefPackage rp in Config.RefPackages)
                    sb.AppendBoth(rp.Name + ".dll");
            }

            if((type & CmdType.AnyMerging) != 0)
            {
                _ToolOrLib tool = GetMergeTool(type);
                prj.SetProperty(MSBuildProperties.DXP_ILMERGE, Config.PreProc.Cmd);
                Log.send(this, $"Merge modules via {tool.name} {tool.version}: {Config.PreProc.Cmd}");

                XProject.AddPackageIfNotExists(tool.name, $"{tool.version}", prj.GetMeta());
                sb.AppendBoth(Config.PreProc.Cmd);
            }

            if(type.HasFlag(CmdType.Exec))
            {
                Log.send(this, $"Pre-Processing command: {Config.PreProc.Cmd}");
                sb.AppendBoth(Config.PreProc.Cmd);
            }

            AddPreProcTarget
            (
                FormatPreProcCmd(type, sb.Fx),
                FormatPreProcCmd(type, sb.Cor),
                type
            );
        }

        private void AddPreProcTarget(string fxCmd, string corCmd, CmdType type)
        {
            var target = prj.AddTarget(MSBuildTargets.DXP_PRE_PROC);

            target.BeforeTargets = MSBuildTargets.DXP_MAIN;
            target.Label = ID;

            bool ignoreErr = (type & CmdType.IgnoreErr) == CmdType.IgnoreErr;

            AddCopyTo(target, $"$({MSBuildProperties.DXP_METALIB_FPATH})", $"$({MSBuildProperties.PRJ_TARGET_DIR})", ignoreErr);

            void _AddCmd()
            {
                if(fxCmd != corCmd)
                {
                    AddExecTask(target, fxCmd, "'$(IsNetCoreBased)'!='true'", ignoreErr);
                    AddExecTask(target, corCmd, "'$(IsNetCoreBased)'=='true'", ignoreErr);
                }
                else
                {
                    AddExecTask(target, fxCmd, condition: null, ignoreErr);
                }
            }

            if((type & CmdType.AnyMerging) != 0)
            {
                AddILMergeWrapper(target, ignoreErr, _ => _AddCmd());
            }
            else
            {
                _AddCmd();
            }

            StringBuilder fDel = new(capacity: 120);
            fDel.Append($"$({MSBuildProperties.PRJ_TARGET_DIR})$({MSBuildProperties.DXP_METALIB_NAME})");
            if(type.HasFlag(CmdType.Conari))
            {
                fDel.Append($";$({MSBuildProperties.PRJ_TARGET_DIR}){GetLib(CmdType.Conari).module}");
            }
            if(type.HasFlag(CmdType.MergeRefPkg))
            {
                AppendPathsToRefPackages(fDel, Config.Platform);
            }
            target.AddTask("Delete", continueOnError: true, t => t.SetParameter("Files", fDel.ToString()));
        }

        private void AddILMergeWrapper(ProjectTargetElement target, bool continueOnError, Action<ProjectTargetElement> act)
        {
            AddMoveAs(target, "$(TargetPath)", $"$(TargetDir){ILMERGE_TMP}\\$(TargetName).dll", continueOnError);
            AddMoveAs(target, "$(TargetDir)$(TargetName).pdb", $"$(TargetDir){ILMERGE_TMP}\\$(TargetName).pdb", continueOnError);

            act?.Invoke(target);
            target.AddTask("RemoveDir", continueOnError, t => t.SetParameter("Directories", "$(TargetDir)" + ILMERGE_TMP));
        }

        private void RemovePreProcTarget(bool hardReset)
        {
            Log.send(this, $"Attempt to delete pre-proc-targets: `{MSBuildTargets.DXP_PRE_PROC}`, `{MSBuildTargets.DXP_PRE_PROC_AFTER}`");
            while(prj.RemoveXmlTarget(MSBuildTargets.DXP_PRE_PROC)) { }

            #region unused since 1.8
            while(prj.RemoveXmlTarget(MSBuildTargets.DXP_PRE_PROC_AFTER)) { }
            #endregion

            if(hardReset)
            {
                while(RemoveCopyLocalLockFileAssemblies()) { }
                while(RemoveOverridedDebugType()) { }
            }
        }

        private string FormatPreProcCmd(CmdType type, StringBuilder sb)
        {
            string cmd = sb?.ToString().TrimEnd();

            if((type & CmdType.AnyMerging) != 0)
            {
                _ToolOrLib tool = GetMergeTool(type);
                StringBuilder ilm = new(100);
                ilm.Append($"{tool.module} ");
                ilm.Append(cmd);
                ilm.Append(" /out:$(TargetFileName)");
                if((type & CmdType.Lib) == CmdType.Lib) ilm.Append(" $(DllExportPreProcSysLibs)");
                if((type & CmdType.DebugInfo) == 0) ilm.Append(" /ndebug");
                if((type & CmdType.Log) == CmdType.Log) ilm.Append($" /log:$(TargetFileName).{tool.name}.log");
                return ilm.ToString();
            }
            return cmd;
        }

        private void AppendPathsToRefPackages(StringBuilder src, Platform platfrom)
        {
            string root = $";$({MSBuildProperties.PRJ_TARGET_DIR})";
            foreach(RefPackage rp in Config.RefPackages)
            {
                string name = rp.Name + ".dll";
                if(platfrom is not Platform.x64 and not Platform.x86)
                {
                    src.Append(root).Append(Path.Combine("x64", name));
                    src.Append(root).Append(Path.Combine("x86", name));
                }
                src.Append(root + name);
            }
        }

        #region obsolete since 1.8
        private bool RemoveCopyLocalLockFileAssemblies() => RemoveLabeledProperty(MSBuildProperties.PRJ_CP_LOCKFILE_ASM);

        private bool RemoveOverridedDebugType() => RemoveLabeledProperty(MSBuildProperties.PRJ_DBG_TYPE);
        #endregion

        private bool RemoveLabeledProperty(string name)
        {
            // access to properties without evaluating the condition attribute
            ProjectPropertyElement _Get() => XProject.Project.Xml.Properties
                                            .FirstOrDefault(p => 
                                                p.Name == name 
                                                && (p.Label == ID || p.Label == Project.METALIB_PK_TOKEN)
                                             ); // METALIB_PK_TOKEN was for 1.7.3 or less

            var pp = _Get();
            if(pp?.Parent == null) {
                return false;
            }

            if(pp.Parent.Children.Count <= 1 && pp.Parent.Parent != null)
            {
                //TODO: but for sdk-style it still can leave an empty <PropertyGroup /> due to msbuild bug
                pp.Parent.Parent.RemoveChild(pp.Parent);
            }
            else
            {
                pp.Parent.RemoveChild(pp);
            }

            return _Get() != null;
        }

        private _ToolOrLib GetMergeTool(CmdType type) => mTool.GetOrDefault(type & CmdType.AnyMerging);

        private _ToolOrLib GetLib(CmdType type) => mTool.GetOrDefault(type);

        private readonly struct _ToolOrLib(string name, Version v, string module = null)
        {
            public readonly string name = name;
            public readonly Version version = v;
            public readonly string module = module ?? name + ".dll";
        }

        private sealed class _FxCorArgBuilder(int capacity)
        {
            public StringBuilder Fx { get; } = new StringBuilder(capacity);
            public StringBuilder Cor { get; } = new StringBuilder(capacity);

            public void AppendBoth(string value) { AppendFx(value); AppendCor(value); }

            public StringBuilder AppendFx(string value) => Fx.Append(GetArg(value));

            public StringBuilder AppendCor(string value) => Cor.Append(GetArg(value));

            private string GetArg(string value) => string.IsNullOrWhiteSpace(value) ? string.Empty : value + " ";
        }
    }
}

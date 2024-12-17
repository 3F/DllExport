/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Linq;
using System.Text;
using Microsoft.Build.Construction;
using net.r_eg.DllExport.Wizard.Extensions;
using net.r_eg.MvsSln.Core;
using net.r_eg.MvsSln.Log;
using static net.r_eg.DllExport.Wizard.PreProc;

namespace net.r_eg.DllExport.Wizard.Gears
{
    internal sealed class PreProcGear(IProjectSvc prj): IProjectGear
    {
        private const string ILMERGE_TMP = ".ilm0";

        private readonly Version incConari = new("1.5.0");
        private readonly Version incILMerge = new("3.0.41");

        private readonly IProjectSvc prj = prj ?? throw new ArgumentNullException(nameof(prj));

        private string Id => $"{Project.METALIB_PK_TOKEN}:PreProc";

        private IUserConfig Config => prj.Config;
        private IXProject XProject => prj.XProject;
        private ISender Log => Config.Log;

        public void Install()
        {
            CfgPreProc(Config.PreProc.Type);

            // Since CmdType can remove or add any feature at the same time,
            XProject.RemoveEmptyPropertyGroups(); // we'll also need to release its possible empty container
        }

        public void Uninstall(bool hardReset)
        {
            RemovePreProcTarget(hardReset);
            XProject.RemovePropertyGroups(p => p.Label == Id);

            prj.RemovePackageReferences("Conari")
                .RemovePackageReferences("ilmerge");
        }

        private void CfgPreProc(CmdType type)
        {
            prj.SetProperty(MSBuildProperties.DXP_PRE_PROC_TYPE, (long)type);
            Log.send(this, $"Pre-Processing type: {type}");

            if(type == CmdType.None) return;

            _FxCorArgBuilder sb = new(capacity: 100);

            if((type & CmdType.Conari) == CmdType.Conari)
            {
                Log.send(this, $"Merge Conari: {incConari}");
                XProject.AddPackageIfNotExists("Conari", $"{incConari}", prj.GetMeta(privateAssets: true));
                sb.AppendBoth("Conari.dll");

#if F_CONARI_ADD_SYS_DLL
                sb.AppendCor("Microsoft.CSharp.dll System.Reflection.Emit.dll");
                sb.AppendCor("System.Reflection.Emit.ILGeneration.dll System.Reflection.Emit.Lightweight.dll");
#endif
                sb.AppendCor("/lib:\"$(_PathToResolvedTargetingPack)\"");
            }

            if((type & CmdType.ILMerge) == CmdType.ILMerge)
            {
                prj.SetProperty(MSBuildProperties.DXP_ILMERGE, Config.PreProc.Cmd);
                Log.send(this, $"Merge modules via ILMerge {incILMerge}: {Config.PreProc.Cmd}");

                XProject.AddPackageIfNotExists("ilmerge", $"{incILMerge}", prj.GetMeta());
                sb.AppendBoth(Config.PreProc.Cmd);
            }

            if((type & CmdType.Exec) == CmdType.Exec)
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
            target.Label = Id;

            target.AddPropertyGroup().SetProperty
            (
                MSBuildProperties._PATH_TO_RSLV_TARGET_PACK,
                "@(ResolvedTargetingPack->'%(Path)\\ref\\%(TargetFramework)')"
            )
            .Condition = "'%(RuntimeFrameworkName)'=='Microsoft.NETCore.App'";

            bool ignoreErr = (type & CmdType.IgnoreErr) == CmdType.IgnoreErr;

            target.AddTask("Copy", ignoreErr, t =>
            {
                t.SetParameter("SourceFiles", $"$({MSBuildProperties.DXP_METALIB_FPATH})");
                t.SetParameter("DestinationFolder", $"$({MSBuildProperties.PRJ_TARGET_DIR})");
                t.SetParameter("SkipUnchangedFiles", "true");
                t.SetParameter("OverwriteReadOnlyFiles", "true");
            });

            AddILMergeWrapper(target, ignoreErr, _=>
            {
                if(corCmd != fxCmd)
                {
                    AddExecTask(target, fxCmd, "'$(IsNetCoreBased)'!='true'", ignoreErr);
                    AddExecTask(target, corCmd, "'$(IsNetCoreBased)'=='true'", ignoreErr);
                }
                else
                {
                    AddExecTask(target, fxCmd, null, ignoreErr);
                }
            });

            target.AddTask("Delete", continueOnError: true,
                t => t.SetParameter("Files", $"$({MSBuildProperties.PRJ_TARGET_DIR})$({MSBuildProperties.DXP_METALIB_NAME})")
            );
        }

        private void AddILMergeWrapper(ProjectTargetElement target, bool continueOnError, Action<ProjectTargetElement> act)
        {
            AddMoveTask(target, "$(TargetPath)", $"$(TargetDir){ILMERGE_TMP}\\$(TargetName).dll", continueOnError);
            AddMoveTask(target, "$(TargetDir)$(TargetName).pdb", $"$(TargetDir){ILMERGE_TMP}\\$(TargetName).pdb", continueOnError);

            act?.Invoke(target);
            target.AddTask("RemoveDir", continueOnError, t => t.SetParameter("Directories", "$(TargetDir)" + ILMERGE_TMP));
        }

        private void AddMoveTask(ProjectTargetElement target, string src, string dst, bool continueOnError)
        {
            target.AddTask("Move", continueOnError, t =>
            {
                t.SetParameter("SourceFiles", src);
                t.SetParameter("DestinationFiles", dst);
                t.SetParameter("OverwriteReadOnlyFiles", "true");
            });
        }

        private ProjectTaskElement AddExecTask(ProjectTargetElement target, string cmd, string condition, bool continueOnError)
        {
            return target.AddTask("Exec", condition, continueOnError, t =>
            {
                t.SetParameter("Command", cmd ?? throw new ArgumentNullException(nameof(cmd)));
                t.SetParameter("WorkingDirectory", $"$({MSBuildProperties.PRJ_TARGET_DIR})");
            });
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

            if((type & CmdType.ILMerge) == CmdType.ILMerge)
            {
                return $"$(ILMergeConsolePath) {cmd} {ILMERGE_TMP}\\$(TargetName).dll /out:$(TargetFileName)" 
                        + (((type & CmdType.DebugInfo) == 0) ? " /ndebug" : string.Empty);
            }
            return cmd;
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
                                                && (p.Label == Id || p.Label == Project.METALIB_PK_TOKEN)
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

        private sealed class _FxCorArgBuilder(int capacity)
        {
            public StringBuilder Fx { get; } = new StringBuilder(capacity);
            public StringBuilder Cor { get; } = new StringBuilder(capacity);

            public void AppendBoth(string value) { AppendFx(value); AppendCor(value); }

            public StringBuilder AppendFx(string value) => Fx.Append(GetArg(value));

            public StringBuilder AppendCor(string value) => Cor.Append(GetArg(value));

            private string GetArg(string value) => value + " ";
        }
    }
}

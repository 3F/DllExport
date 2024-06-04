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
    internal sealed class PreProcGear: IProjectGear
    {
        private const string ILMERGE_TMP = ".ilm0";

        private readonly Version incConari = new Version("1.4.0");
        private readonly Version incILMerge = new Version("3.0.29");

        private readonly IProjectSvc prj;
        private ProjectPropertyGroupElement pgroup;

        private string Id => $"{Project.METALIB_PK_TOKEN}:PreProc";

        private IUserConfig Config => prj.Config;
        private IXProject XProject => prj.XProject;
        private ISender Log => Config.Log;

        public void Install()
        {
            pgroup = XProject.GetOrAddPropertyGroup(Id);
            CfgPreProc(Config.PreProc.Type);

            // Since CmdType can remove or add any feature at the same time,
            XProject.RemoveEmptyPropertyGroups(); // we'll also need to release its possible empty container
        }

        public void Uninstall(bool hardReset)
        {
            RemovePreProcTarget(hardReset);
            XProject.RemovePropertyGroups(p => p.Label == Id);
        }

        public PreProcGear(IProjectSvc prj)
        {
            this.prj = prj ?? throw new ArgumentNullException(nameof(prj));
        }

        private void CfgPreProc(CmdType type)
        {
            prj.SetProperty(MSBuildProperties.DXP_PRE_PROC_TYPE, (long)type);
            Log.send(this, $"Pre-Processing type: {type}");

            if(type == CmdType.None) {
                return;
            }

            var sb = new _MixStringBuilder(5);

            if((type & CmdType.Conari) == CmdType.Conari)
            {
                Log.send(this, $"Integrate Conari: {incConari}");
                XProject.AddPackageIfNotExists("Conari", $"{incConari}");
                sb.AppendBoth("Conari.dll ");

                // .NET Core
                sb.AppendCor("Microsoft.CSharp.dll System.Reflection.Emit.dll ");
                sb.AppendCor("System.Reflection.Emit.ILGeneration.dll System.Reflection.Emit.Lightweight.dll ");
                OverrideCopyLocalLockFileAssemblies();
            }

            if((type & CmdType.ILMerge) == CmdType.ILMerge)
            {
                prj.SetProperty(MSBuildProperties.DXP_ILMERGE, Config.PreProc.Cmd);
                Log.send(this, $"Merge modules via ILMerge {incILMerge}: {Config.PreProc.Cmd}");

                XProject.AddPackageIfNotExists("ilmerge", $"{incILMerge}");
                sb.AppendBoth($"{Config.PreProc.Cmd} ");
            }

            if((type & CmdType.Exec) == CmdType.Exec)
            {
                Log.send(this, $"Pre-Processing command: {Config.PreProc.Cmd}");
                sb.AppendBoth($"{Config.PreProc.Cmd} ");
            }

            AddPreProcTarget(
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

            var tCopy = target.AddTask("Copy");
            tCopy.SetParameter("SourceFiles", $"$({MSBuildProperties.DXP_METALIB_FPATH})");
            tCopy.SetParameter("DestinationFolder", $"$({MSBuildProperties.PRJ_TARGET_DIR})");
            tCopy.SetParameter("SkipUnchangedFiles", "true");
            tCopy.SetParameter("OverwriteReadOnlyFiles", "true");

            bool ignoreErr = (type & CmdType.IgnoreErr) == CmdType.IgnoreErr;
            ProjectTaskElement tExec;

            if(corCmd != fxCmd)
            {
                tExec = AddExecTask(target, fxCmd, "'$(IsNetCoreBased)'!='true'", ignoreErr);
                        AddExecTask(target, corCmd, "'$(IsNetCoreBased)'=='true'", ignoreErr);
            }
            else
            {
                tExec = AddExecTask(target, fxCmd, null, ignoreErr);
            }

            if((type & CmdType.DebugInfo) == CmdType.DebugInfo)
            {
                OverrideDebugType();
                AddPreProcAfterTarget(target, tExec);
            }

            var tDelete = target.AddTask("Delete");
            tDelete.SetParameter("Files", $"$({MSBuildProperties.PRJ_TARGET_DIR})$({MSBuildProperties.DXP_METALIB_NAME})");
            tDelete.ContinueOnError = "true";
        }

        private void AddPreProcAfterTarget(ProjectTargetElement ppTarget, ProjectTaskElement tExec)
        {
            var tMove = ppTarget.AddTask("Move");
            tMove.SetParameter("SourceFiles", $"$({MSBuildProperties.PRJ_TARGET_DIR})$({MSBuildProperties.PRJ_TARGET_F}){ILMERGE_TMP}.dll");
            tMove.SetParameter("DestinationFiles", $"$({MSBuildProperties.PRJ_TARGET_DIR})$({MSBuildProperties.PRJ_TARGET_F})");
            tMove.SetParameter("OverwriteReadOnlyFiles", "true");
            tMove.ContinueOnError = tExec.ContinueOnError;

            var target = prj.AddTarget(MSBuildTargets.DXP_PRE_PROC_AFTER);
            target.AfterTargets = MSBuildTargets.DXP_MAIN;
            target.Label = Id;

            var tDelete = target.AddTask("Delete");
            tDelete.SetParameter("Files", $"$({MSBuildProperties.PRJ_TARGET_DIR})$({MSBuildProperties.PRJ_TARGET_F}){ILMERGE_TMP}.pdb");
            tDelete.ContinueOnError = "true";
        }

        private ProjectTaskElement AddExecTask(ProjectTargetElement target, string cmd, string condition, bool continueOnError)
        {
            var tExec = target.AddTask("Exec");
            tExec.Condition = condition;
            tExec.SetParameter("Command", cmd ?? throw new ArgumentNullException(nameof(cmd)));
            tExec.SetParameter("WorkingDirectory", $"$({MSBuildProperties.PRJ_TARGET_DIR})");
            tExec.ContinueOnError = continueOnError.ToString().ToLower();

            return tExec;
        }

        private void RemovePreProcTarget(bool hardReset)
        {
            Log.send(this, $"Trying to remove pre-proc-targets: `{MSBuildTargets.DXP_PRE_PROC}`, `{MSBuildTargets.DXP_PRE_PROC_AFTER}`");
            while(prj.RemoveXmlTarget(MSBuildTargets.DXP_PRE_PROC)) { }
            while(prj.RemoveXmlTarget(MSBuildTargets.DXP_PRE_PROC_AFTER)) { }

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
                return $"$(ILMergeConsolePath) {cmd} $(TargetFileName) /out:$(TargetFileName)" 
                        + (((type & CmdType.DebugInfo) == 0) ? " /ndebug" : $"{ILMERGE_TMP}.dll");
            }
            return cmd;
        }

        private void OverrideCopyLocalLockFileAssemblies()
        {
            var prop = pgroup.SetProperty(MSBuildProperties.PRJ_CP_LOCKFILE_ASM, "true");
            prop.Condition = "$(TargetFramework.StartsWith('netc')) Or $(TargetFramework.StartsWith('nets'))";
            prop.Label = Id;
        }

        private bool RemoveCopyLocalLockFileAssemblies() => RemoveLabeledProperty(MSBuildProperties.PRJ_CP_LOCKFILE_ASM);

        private void OverrideDebugType()
        {
            var prop = pgroup.SetProperty(MSBuildProperties.PRJ_DBG_TYPE, "pdbonly");
            prop.Condition = "'$(DebugType)'!='full' And '$(DebugType)'!='pdbonly'";
            prop.Label = Id;
        }

        private bool RemoveOverridedDebugType() => RemoveLabeledProperty(MSBuildProperties.PRJ_DBG_TYPE);

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

        private sealed class _MixStringBuilder
        {
            public StringBuilder Fx { get; private set; }
            public StringBuilder Cor { get; private set; }

            public void AppendBoth(string value)
            {
                AppendFx(value);
                AppendCor(value);
            }

            public StringBuilder AppendFx(string value) => Fx.Append(value);
            public StringBuilder AppendCor(string value) => Cor.Append(value);

            public _MixStringBuilder(int capacity)
            {
                Fx  = new StringBuilder(capacity);
                Cor = new StringBuilder(capacity);
            }
        }
    }
}

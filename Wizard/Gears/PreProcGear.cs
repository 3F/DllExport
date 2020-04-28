/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
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
        private readonly ProjectPropertyGroupElement pgroup;

        private IUserConfig Config => prj.Config;
        private IXProject XProject => prj.XProject;
        private ISender Log => Config.Log;

        public void Install() => CfgPreProc();

        public void Uninstall(bool hardReset) => RemovePreProcTarget(hardReset);

        public PreProcGear(IProjectSvc prj)
        {
            this.prj    = prj ?? throw new ArgumentNullException(nameof(prj));
            pgroup      = XProject.Project.Xml.AddPropertyGroup();
        }

        private void CfgPreProc()
        {
            CmdType type = Config.PreProc.Type;

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
            target.Label = Project.METALIB_PK_TOKEN;

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
            target.Label = Project.METALIB_PK_TOKEN;

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
            Log.send(this, $"Trying to remove pre-proc-targets: `{MSBuildTargets.DXP_PRE_PROC}`, `{MSBuildTargets.DXP_PRE_PROC_AFTER}`", Message.Level.Info);
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
            prop.Label = Project.METALIB_PK_TOKEN;
        }

        private bool RemoveCopyLocalLockFileAssemblies() => RemoveLabeledProperty(MSBuildProperties.PRJ_CP_LOCKFILE_ASM);

        private void OverrideDebugType()
        {
            var prop = pgroup.SetProperty(MSBuildProperties.PRJ_DBG_TYPE, "pdbonly");
            prop.Condition = "'$(DebugType)'!='full'";
            prop.Label = Project.METALIB_PK_TOKEN;
        }

        private bool RemoveOverridedDebugType() => RemoveLabeledProperty(MSBuildProperties.PRJ_DBG_TYPE);

        private bool RemoveLabeledProperty(string name)
        {
            // access to properties without evaluating the condition attribute
            ProjectPropertyElement _Get() => XProject.Project.Xml.Properties
                                            .FirstOrDefault(p => p.Name == name && p.Label == Project.METALIB_PK_TOKEN);

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

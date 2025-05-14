/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using Microsoft.Build.Construction;
using net.r_eg.MvsSln.Log;
using static net.r_eg.DllExport.Wizard.PostProc;
using MSBuildPostProc = net.r_eg.DllExport.Activator.PostProc;

namespace net.r_eg.DllExport.Wizard.Gears
{
    internal sealed class PostProcGear(IProjectSvc prj): ProjectGearAbstract(prj)
    {
        public override void Install()
        {
            CfgPostProc(Config.PostProc.Type);
        }

        public override void Uninstall(bool hardReset)
        {
            RemoveDerivativeTargets();
            RemovePostProcTarget();
        }

        private void CfgPostProc(CmdType type)
        {
            prj.SetProperty(MSBuildProperties.DXP_POST_PROC_TYPE, (long)type);
            Log.send(this, $"Post-Processing type: {type}");

            if(type == CmdType.None) {
                return;
            }

            prj.SetProperty(MSBuildProperties.DXP_PROC_ENV, Config.PostProc.GetProcEnvAsString());
            Log.send(this, $"Proc-Env: {Config.PostProc.ProcEnv}");

            var target = prj.AddTarget(MSBuildTargets.DXP_POST_PROC);
            target.Label = ID;

            if((type & CmdType.Custom) == CmdType.Custom)
            {
                //TODO: 
                Log.send(this, $"Post-Processing Custom type is not fully implemented: {type}", Message.Level.Warn);
                return;
            }

            if((type & (CmdType.DependentX86X64 | CmdType.DependentIntermediateFiles)) != 0)
            {
                CfgDepPostProc(AddRecursiveDependentsFor(MSBuildProperties.PRJ_TARGET_DIR, type), type);
            }
        }

        private void CfgDepPostProc(ProjectTargetElement dep, CmdType type)
        {
            if((type & CmdType.DependentX86X64) == CmdType.DependentX86X64)
            {
                AddTaskForX86X64(dep, type, 86);
                AddTaskForX86X64(dep, type, 64);
            }

            if((type & CmdType.DependentIntermediateFiles) == CmdType.DependentIntermediateFiles)
            {
                AddTasksIntermediateFiles(dep, type, "Before");
                AddTasksIntermediateFiles(dep, type, "After");
            }
        }

        /// <summary>
        /// https://github.com/3F/DllExport/pull/148#issuecomment-624021746
        /// </summary>
        private ProjectTargetElement AddRecursiveDependentsFor(string id, CmdType type)
        {
            var target = AllocateDerivativeTarget("For" + id);

            target.AfterTargets = MSBuildTargets.DXP_POST_PROC;
            target.Label        = ID;
            target.Outputs      = $"%({GetDependentsTargetDir(type)}.Identity)";

            return target;
        }

        private void AddTaskForX86X64(ProjectTargetElement target, CmdType type, int arch)
        {
            NewTasksCopyDependentsTargetDir(target, type, $"@(DllExportDirX{arch})", $"x{arch}\\");
        }

        private void AddTasksIntermediateFiles(ProjectTargetElement target, CmdType type, string dir)
        {
            NewTasksCopyDependentsTargetDir(target, type, $"@(DllExportDir{dir})", $"{dir}\\");
        }

        private void NewTasksCopyDependentsTargetDir(ProjectTargetElement target, CmdType type, string src, string dst)
        {
            string dependentsTargetDir = GetDependentsTargetDir(type);

            AddCopyTo(target, src, $@"%({dependentsTargetDir}.Identity){dst}", ignoreErr: false)
                .Condition = $"'%({dependentsTargetDir}.Identity)'!=''";
        }

        private ProjectTargetElement AllocateDerivativeTarget(string name) => prj.AddTarget(GetDerivativeTargetName(name));

        private string GetDependentsTargetDir(CmdType type)
        {
            if((type & CmdType.SeqDependentForSys) == CmdType.SeqDependentForSys) {
                return MSBuildPostProc.GetNameForSeqDependentsProperty(MSBuildProperties.PRJ_TARGET_DIR);
            }
            return MSBuildPostProc.GetNameForDependentsProperty(MSBuildProperties.PRJ_TARGET_DIR);
        }

        private void RemovePostProcTarget()
        {
            Log.send(this, $"Attempt to delete post-proc-target `{MSBuildTargets.DXP_POST_PROC}`");
            while(prj.RemoveXmlTarget(MSBuildTargets.DXP_POST_PROC)) { }
        }

        private void RemoveDerivativeTargets()
        {
            foreach(var target in prj.XProject.Project.Xml.Targets)
            {
                if((target.Label != ID && target.Label != Project.METALIB_PK_TOKEN) // METALIB_PK_TOKEN was for 1.7.3 or less
                    || target.Name == MSBuildTargets.DXP_POST_PROC
                    || !target.Name.StartsWith(GetDerivativeTargetName(null)))
                {
                    continue;
                }

                Log.send(this, $"Remove derivative post-proc target `{target.Name}`", Message.Level.Trace);
                while(prj.RemoveXmlTarget(target.Name)) { }
            }
        }

        private string GetDerivativeTargetName(string name) => $"{MSBuildTargets.DXP_POST_PROC}{name}";
    }
}

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
using Microsoft.Build.Construction;
using net.r_eg.MvsSln.Log;
using static net.r_eg.DllExport.Wizard.PostProc;

namespace net.r_eg.DllExport.Wizard.Gears
{
    internal sealed class PostProcGear: IProjectGear
    {
        private readonly IProjectSvc prj;

        private IUserConfig Config => prj.Config;
        private ISender Log => Config.Log;

        public void Install() => CfgPostProc();

        public void Uninstall(bool hardReset) => RemovePostProcTarget(hardReset);

        public PostProcGear(IProjectSvc prj)
        {
            this.prj = prj ?? throw new ArgumentNullException(nameof(prj));
        }

        private void CfgPostProc()
        {
            CmdType type = Config.PostProc.Type;

            prj.SetProperty(MSBuildProperties.DXP_POST_PROC_TYPE, (long)type);
            Log.send(this, $"Post-Processing type: {type}");

            if(type == CmdType.None) {
                return;
            }

            prj.SetProperty(MSBuildProperties.DXP_PROC_ENV, Config.PostProc.GetProcEnvAsString());
            Log.send(this, $"Proc-Env: {Config.PostProc.ProcEnv}");

            var target = prj.AddTarget(MSBuildTargets.DXP_POST_PROC);
            target.Label = Project.METALIB_PK_TOKEN;

            if((type & CmdType.Custom) == CmdType.Custom)
            {
                //TODO: 
                Log.send(this, $"Post-Processing Custom type is not fully implemented: {type}", Message.Level.Warn);
                return;
            }
            
            if((type & CmdType.DependentX86X64) == CmdType.DependentX86X64)
            {
                AddTaskForX86X64(target, 86);
                AddTaskForX86X64(target, 64);
            }

            if((type & CmdType.DependentIntermediateFiles) == CmdType.DependentIntermediateFiles)
            {
                AddTasksIntermediateFiles(target, "Before");
                AddTasksIntermediateFiles(target, "After");
            }
        }

        private void AddTaskForX86X64(ProjectTargetElement target, int arch)
        {
            var tCopy = target.AddTask("Copy");
            tCopy.SetParameter("SourceFiles", $"@(DllExportDirX{arch})");
            tCopy.SetParameter("DestinationFolder", $@"@(DllExportDependentsTargetDir->'%(Identity)x{arch}\')");
            tCopy.SetParameter("OverwriteReadOnlyFiles", "true");
        }

        private void AddTasksIntermediateFiles(ProjectTargetElement target, string dir)
        {
            var tCopy = target.AddTask("Copy");
            tCopy.SetParameter("SourceFiles", $"@(DllExportDir{dir})");
            tCopy.SetParameter("DestinationFolder", $@"@(DllExportDependentsTargetDir->'%(Identity){dir}\')");
            tCopy.SetParameter("OverwriteReadOnlyFiles", "true");
        }

        private void RemovePostProcTarget(bool _)
        {
            Log.send(this, $"Trying to remove post-proc-target `{MSBuildTargets.DXP_POST_PROC}`");
            while(prj.RemoveXmlTarget(MSBuildTargets.DXP_POST_PROC)) { }
        }
    }
}

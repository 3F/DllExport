/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using Microsoft.Build.Construction;
using net.r_eg.DllExport.Wizard.Extensions;
using net.r_eg.MvsSln.Core;
using net.r_eg.MvsSln.Log;

namespace net.r_eg.DllExport.Wizard.Gears
{
    internal abstract class ProjectGearAbstract(IProjectSvc prj)
    {
        protected readonly IProjectSvc prj = prj ?? throw new ArgumentNullException(nameof(prj));

        protected IUserConfig Config => prj.Config;

        protected IXProject XProject => prj.XProject;

        protected ISender Log => Config.Log;

        public abstract void Install();

        public abstract void Uninstall(bool hardReset);

        protected ProjectTaskElement AddExecTask(ProjectTargetElement target, string cmd, string condition, bool continueOnError)
        {
            return target.AddTask("Exec", condition, continueOnError, t =>
            {
                t.SetParameter("Command", cmd ?? throw new ArgumentNullException(nameof(cmd)));
                t.SetParameter("WorkingDirectory", $"$({MSBuildProperties.PRJ_TARGET_DIR})");
            });
        }

        protected ProjectTaskElement AddCopyTo(ProjectTargetElement target, string src, string dstFolder, bool ignoreErr)
            => AddCopyOrMoveTask(copy: true, target, src, dstFolder, ignoreErr);

        protected ProjectTaskElement AddMoveAs(ProjectTargetElement target, string src, string dstFiles, bool ignoreErr)
             => AddCopyOrMoveTask(copy: false, target, src, dstFiles, ignoreErr);

        protected ProjectTaskElement AddCopyOrMoveTask(bool copy, ProjectTargetElement target, string src, string dst, bool ignoreErr)
        {
            return target.AddTask(copy ? "Copy" : "Move", ignoreErr, t =>
            {
                t.SetParameter("SourceFiles", src);
                if(copy)
                {
                    t.SetParameter("DestinationFolder", dst);
                    t.SetParameter("SkipUnchangedFiles", "true");
                }
                else t.SetParameter("DestinationFiles", dst);
                t.SetParameter("OverwriteReadOnlyFiles", "true");
            });
        }
    }
}

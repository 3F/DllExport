/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using net.r_eg.DllExport.NSBin;
using net.r_eg.MvsSln.Log;

namespace net.r_eg.DllExport.Wizard
{
    public interface IExecutor: IDisposable
    {
        /// <summary>
        /// Access to wizard configuration.
        /// </summary>
        IWizardConfig Config { get; }

        ISender Log { get; }

        /// <summary>
        /// ddNS feature core.
        /// </summary>
        IDDNS DDNS { get; }

        /// <summary>
        /// Latest selected .sln file.
        /// </summary>
        string ActiveSlnFile { get; set; }

        /// <summary>
        /// List of available .sln files.
        /// </summary>
        IEnumerable<string> SlnFiles { get; }

        /// <summary>
        /// Access to used external .targets.
        /// </summary>
        ITargetsFile TargetsFile { get; }

        /// <summary>
        /// Access to used external .targets 
        /// Only if CfgStorageType.TargetsFile or null.
        /// </summary>
        ITargetsFile TargetsFileIfCfg { get; }

        /// <summary>
        /// List of all found projects with different configurations.
        /// </summary>
        /// <param name="sln">Full path to .sln</param>
        /// <returns></returns>
        IEnumerable<IProject> ProjectsBy(string sln);

        /// <summary>
        /// List of all found projects that's unique by guid.
        /// </summary>
        /// <param name="sln"></param>
        /// <returns></returns>
        IEnumerable<IProject> UniqueProjectsBy(string sln);

        /// <summary>
        /// To start process of the required configuration.
        /// </summary>
        void Configure();

        /// <summary>
        /// Depending on the current CfgStorageType, 
        /// Either save or delete used TargetsFile.
        /// </summary>
        void SaveTStorageOrDelete();
    }
}

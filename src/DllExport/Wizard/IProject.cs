/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System.Collections.Generic;
using net.r_eg.MvsSln.Core;

namespace net.r_eg.DllExport.Wizard
{
    public interface IProject
    {
        /// <summary>
        /// Access to found project.
        /// </summary>
        IXProject XProject { get; }

        /// <summary>
        /// Installation checking.
        /// </summary>
        bool Installed { get; }

        /// <summary>
        /// Message if an internal error occurred, otherwise null value.
        /// TODO: because of DxpIsolatedEnv. See details there.
        /// </summary>
        string InternalError { get; }

        /// <summary>
        /// Special identifier. Like `ProjectGuid` that is not available in SDK-based projects.
        /// https://github.com/3F/DllExport/issues/36#issuecomment-320794498
        /// </summary>
        string DxpIdent { get; }

        /// <summary>
        /// Relative path from location of sln file.
        /// </summary>
        string ProjectPath { get; }

        /// <summary>
        /// Get defined namespace for project.
        /// </summary>
        string ProjectNamespace { get; }

        /// <summary>
        /// Checks usage of external storage for this project.
        /// </summary>
        bool HasExternalStorage { get; }

        /// <summary>
        /// Active configuration of user data.
        /// </summary>
        IUserConfig Config { get; set; }

        /// <summary>
        /// List of used MSBuild properties.
        /// </summary>
        IDictionary<string, string> ConfigProperties { get; }

        /// <summary>
        /// Limitation of actions if not used PublicKeyToken.
        /// https://github.com/3F/DllExport/issues/65
        /// </summary>
        bool PublicKeyTokenLimit { get; set; }

        /// <summary>
        /// Returns fullpath to meta library for current project.
        /// </summary>
        /// <param name="evaluate">Will return unevaluated value if false.</param>
        /// <param name="corlib">netfx-based or netcore-based meta lib.</param>
        /// <returns></returns>
        string MetaLib(bool evaluate, bool corlib = false);

        /// <summary>
        /// To recover references in user project files.
        /// </summary>
        /// <remarks>Note: the final type of references depends on <see cref="IWizardConfig.CfgStorage"/></remarks>
        /// <param name="id">Known identifier of the references.</param>
        /// <param name="type">The type of action that initiated the recovery process.</param>
        /// <param name="xp">For non-<see cref="CfgStorageType.ProjectFiles"/> storage provides an additional way to deal with actual data to recover from it.</param>
        void Recover(string id, ActionType type = ActionType.Recover, IXProject xp = null);

        /// <summary>
        /// To unset configured data from project if presented.
        /// </summary>
        void Unset();

        /// <summary>
        /// To configure project via specific action.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool Configure(ActionType type);
    }
}

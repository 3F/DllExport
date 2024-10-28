/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

namespace net.r_eg.DllExport.Wizard
{
    public enum ActionType: uint
    {
        Default,

        /// <summary>
        /// Process of configuration of available projects.
        /// Install and Remove operation will be defined by user at runtime.
        /// </summary>
        Configure,

        /// <summary>
        /// To update package reference for already configured projects.
        /// </summary>
        Update,

        /// <summary>
        /// To restore already configured environment.
        /// </summary>
        Restore,

        /// <summary>
        /// Information about obsolete nuget clients etc.
        /// </summary>
        Info,

        /// <summary>
        /// To re-configure projects using predefined/exported data.
        /// </summary>
        Recover,

        /// <summary>
        /// Recover to initial setup using predefined/exported data.
        /// </summary>
        RecoverInit,

        /// <summary>
        /// To export configured projects data.
        /// </summary>
        Export,

        /// <summary>
        /// To unset all data from specified projects.
        /// </summary>
        Unset,

        /// <summary>
        /// Aggregates an Update action with additions for upgrading.
        /// TODO: Currently is equal to the Update action.
        /// </summary>
        Upgrade = Update,

        /// <summary>
        /// To list projects and their statuses. As plain text data.
        /// TODO:
        /// </summary>
        //ListPlain
    }
}
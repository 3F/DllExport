/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

namespace net.r_eg.DllExport.Wizard
{
    public interface IWizardConfig
    {
        /// <summary>
        /// Optional root path of user paths. 
        /// Affects on SlnFile, SlnDir, PkgPath.
        /// </summary>
        string RootPath { get; }

        /// <summary>
        /// Path to directory with .sln files to be processed.
        /// </summary>
        string SlnDir { get; }

        /// <summary>
        /// Optional predefined .sln file to process via the restore operations etc.
        /// </summary>
        string SlnFile { get; }

        /// <summary>
        /// Root path of the DllExport package.
        /// </summary>
        string PkgPath { get; }

        /// <summary>
        /// Relative path to meta library.
        /// </summary>
        string MetaLib { get; }

        /// <summary>
        /// Relative path to meta core library.
        /// </summary>
        string MetaCor { get; }

        /// <summary>
        /// Path to .targets file of the DllExport.
        /// </summary>
        string DxpTarget { get; }

        /// <summary>
        /// Arguments to manager.
        /// </summary>
        string MgrArgs { get; }

        /// <summary>
        /// Version of the package that invokes target.
        /// </summary>
        string PkgVer { get; }

        /// <summary>
        /// Indicates the status of the used package.
        /// </summary>
        bool Distributable { get; }

        /// <summary>
        /// The type of the current package, eg. offline, ...
        /// </summary>
        string PackageType { get; }

        /// <summary>
        /// Proxy configuration if presented in `-proxy` key.
        /// </summary>
        string Proxy { get; }

        /// <summary>
        /// Options through <see cref="DxpOptType"/>.
        /// </summary>
        DxpOptType Options { get; }

        /// <summary>
        /// Path to external storage if used.
        /// </summary>
        string StoragePath { get; }

        /// <summary>
        /// Where to store configuration data.
        /// </summary>
        CfgStorageType CfgStorage { get; set; }

        /// <summary>
        /// The evaluated type of operation.
        /// </summary>
        ActionType Type { get; }

        /// <summary>
        /// To show messages via GUI dlg for selected level (any positive number) and above.
        /// Levels: 0 - 5 (see Message.Level)
        /// '4' = means 4 (Error) + 5 (Fatal) levels.
        /// Any negative number disables this.
        /// It affects only for messages to GUI.
        /// </summary>
        int MsgGuiLevel { get; }
    }
}

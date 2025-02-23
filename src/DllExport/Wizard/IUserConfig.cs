/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using net.r_eg.DllExport.NSBin;
using net.r_eg.MvsSln.Core;
using net.r_eg.MvsSln.Log;

namespace net.r_eg.DllExport.Wizard
{
    public interface IUserConfig
    {
        /// <summary>
        /// Flag of installation.
        /// </summary>
        bool Install { get; set; }

        /// <summary>
        /// A selected namespace for ddNS feature.
        /// </summary>
        string Namespace { get; set; }

        /// <summary>
        /// Allowed buffer size for NS.
        /// </summary>
        int NSBuffer { get; set; }

        /// <summary>
        /// To use Cecil instead of direct modifications.
        /// </summary>
        bool UseCecil { get; set; }

        /// <summary>
        /// Predefined list of namespaces.
        /// </summary>
        List<string> Namespaces { get; set; }

        /// <summary>
        /// Access to wizard configuration.
        /// </summary>
        IWizardConfig Wizard { get; set; }

        /// <summary>
        /// Access to ddNS core.
        /// </summary>
        IDDNS DDNS { get; set; }

        /// <summary>
        /// Specific logger.
        /// </summary>
        ISender Log { get; set; }

        /// <summary>
        /// Platform target for project.
        /// </summary>
        Platform Platform { get; set; }

        /// <summary>
        /// Settings for ILasm etc.
        /// </summary>
        CompilerCfg Compiler { get; set; }

        /// <summary>
        /// Access to Pre-Processing.
        /// </summary>
        PreProc PreProc { get; set; }

        /// <summary>
        /// Access to Post-Processing.
        /// </summary>
        PostProc PostProc { get; set; }

        /// <summary>
        /// Custom .assembly extern ... configurations
        /// </summary>
        List<ILAsm.AssemblyExternDirective> AssemblyExternDirectives { get; set; }

        /// <summary>
        /// Custom .typeref ... configurations
        /// </summary>
        List<ILAsm.TypeRefDirective> TypeRefDirectives { get; set; }

        /// <summary>
        /// Options for .typeref like $-interpolation using predefined stub implementation etc.
        /// </summary>
        TypeRefOptions TypeRefOptions { get; set; }

        /// <summary>
        /// List of package references to provide the specified assemblies along with the module being modified
        /// at the pre-processing stage.
        /// </summary>
        List<RefPackage> RefPackages { get; set; }

        /// <summary>
        /// Adds to top new namespace into Namespaces property.
        /// </summary>
        /// <param name="ns"></param>
        /// <returns>true if added.</returns>
        bool AddTopNamespace(string ns);

        /// <summary>
        /// Updates <see cref="IUserConfig"/> data using <see cref="IXProject"/>.
        /// </summary>
        /// <param name="xp"></param>
        void UpdateDataFrom(IXProject xp);

        /// <summary>
        /// Validate <see cref="AssemblyExternDirectives"/> directives.
        /// </summary>
        /// <param name="onFailed">Execute action if the directive is invalid.</param>
        /// <returns>true if all directives are valid.</returns>
        bool ValidateAssemblyExternDirectives(Func<string, bool> onFailed);

        /// <summary>
        /// Validate <see cref="TypeRefDirectives"/> directives.
        /// </summary>
        /// <param name="onFailed">Execute action if the directive is invalid.</param>
        /// <returns>true if all directives are valid.</returns>
        bool ValidateTypeRefDirectives(Func<string, bool> onFailed);

        /// <summary>
        /// Validate <see cref="RefPackages"/> records.
        /// </summary>
        /// <param name="onFailed">Execute action if record is invalid.</param>
        /// <returns>true if all records are valid.</returns>
        bool ValidateRefPackages(Func<string, bool> onFailed);

        /// <summary>
        /// Validate <see cref="CompilerCfg.imageBase"/> and <see cref="CompilerCfg.imageBaseStep"/>.
        /// </summary>
        /// <param name="onFailed">Execute action if value is invalid.</param>
        /// <returns>true if both <see cref="CompilerCfg.imageBase"/> and <see cref="CompilerCfg.imageBaseStep"/> meets all the requirements.</returns>
        bool ValidateImageBase(Func<string, bool> onFailed);
    }
}

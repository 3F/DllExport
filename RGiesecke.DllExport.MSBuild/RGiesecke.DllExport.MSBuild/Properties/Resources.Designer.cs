//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2018  Denis Kuzmin < entry.reg@gmail.com > :: github.com/3F
//$ Distributed under the MIT License (MIT)

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace RGiesecke.DllExport.MSBuild.Properties
{
[DebuggerNonUserCode]
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
public class Resources
{
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static ResourceManager ResourceManager
    {
        get
        {
            if (object.ReferenceEquals((object) RGiesecke.DllExport.MSBuild.Properties.Resources.resourceMan, (object) null)) {
                RGiesecke.DllExport.MSBuild.Properties.Resources.resourceMan = new ResourceManager("RGiesecke.DllExport.MSBuild.Properties.Resources", typeof (RGiesecke.DllExport.MSBuild.Properties.Resources).Assembly);
            }
            return RGiesecke.DllExport.MSBuild.Properties.Resources.resourceMan;
        }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static CultureInfo Culture
    {
        get
        {
            return RGiesecke.DllExport.MSBuild.Properties.Resources.resourceCulture;
        }
        set
        {
            RGiesecke.DllExport.MSBuild.Properties.Resources.resourceCulture = value;
        }
    }

    public static string AssemblyRedirection_for_0_has_not_been_setup_
    {
        get
        {
            return RGiesecke.DllExport.MSBuild.Properties.Resources.ResourceManager.GetString("AssemblyRedirection_for_0_has_not_been_setup_", RGiesecke.DllExport.MSBuild.Properties.Resources.resourceCulture);
        }
    }

    public static string Both_key_values_KeyContainer_0_and_KeyFile_0_are_present_only_one_can_be_specified
    {
        get
        {
            return RGiesecke.DllExport.MSBuild.Properties.Resources.ResourceManager.GetString("Both_key_values_KeyContainer_0_and_KeyFile_0_are_present_only_one_can_be_specified", RGiesecke.DllExport.MSBuild.Properties.Resources.resourceCulture);
        }
    }

    public static string Cannot_find_ilasm_exe_in_0_
    {
        get
        {
            return RGiesecke.DllExport.MSBuild.Properties.Resources.ResourceManager.GetString("Cannot_find_ilasm_exe_in_0_", RGiesecke.DllExport.MSBuild.Properties.Resources.resourceCulture);
        }
    }

    public static string Cannot_find_ilasm_exe_without_a_FrameworkPath
    {
        get
        {
            return RGiesecke.DllExport.MSBuild.Properties.Resources.ResourceManager.GetString("Cannot_find_ilasm_exe_without_a_FrameworkPath", RGiesecke.DllExport.MSBuild.Properties.Resources.resourceCulture);
        }
    }

    public static string Cannot_find_lib_exe_in_0_
    {
        get
        {
            return RGiesecke.DllExport.MSBuild.Properties.Resources.ResourceManager.GetString("Cannot_find_lib_exe_in_0_", RGiesecke.DllExport.MSBuild.Properties.Resources.resourceCulture);
        }
    }

    public static string Cannot_get_a_reference_to_ToolLocationHelper
    {
        get
        {
            return RGiesecke.DllExport.MSBuild.Properties.Resources.ResourceManager.GetString("Cannot_get_a_reference_to_ToolLocationHelper", RGiesecke.DllExport.MSBuild.Properties.Resources.resourceCulture);
        }
    }

    public static string Input_file_0_does_not_exist__cannot_create_unmanaged_exports
    {
        get
        {
            return RGiesecke.DllExport.MSBuild.Properties.Resources.ResourceManager.GetString("Input_file_0_does_not_exist__cannot_create_unmanaged_exports", RGiesecke.DllExport.MSBuild.Properties.Resources.resourceCulture);
        }
    }

    public static string Input_file_0_is_not_a_DLL_hint
    {
        get
        {
            return RGiesecke.DllExport.MSBuild.Properties.Resources.ResourceManager.GetString("Input_file_0_is_not_a_DLL_hint", RGiesecke.DllExport.MSBuild.Properties.Resources.resourceCulture);
        }
    }

    public static string Input_file_is_empty__cannot_create_unmanaged_exports
    {
        get
        {
            return RGiesecke.DllExport.MSBuild.Properties.Resources.ResourceManager.GetString("Input_file_is_empty__cannot_create_unmanaged_exports", RGiesecke.DllExport.MSBuild.Properties.Resources.resourceCulture);
        }
    }

    public static string Output_assembly_was_signed_however_neither_keyfile_nor_keycontainer_could_be_inferred__Reading_those_values_from_assembly_attributes_is_not__yet__supported__they_have_to_be_defined_inside_the_MSBuild_project_file
    {
        get
        {
            return RGiesecke.DllExport.MSBuild.Properties.Resources.ResourceManager.GetString("Output_assembly_was_signed_however_neither_keyfile_nor_keycontainer_could_be_inferred__Reading_those_values_from_assembly_attributes_is_not__yet__supported__they_have_to_be_defined_inside_the_MSBuild_project_file", RGiesecke.DllExport.MSBuild.Properties.Resources.resourceCulture);
        }
    }

    public static string SdkPath_is_empty_continuing_with_0_
    {
        get
        {
            return RGiesecke.DllExport.MSBuild.Properties.Resources.ResourceManager.GetString("SdkPath_is_empty_continuing_with_0_", RGiesecke.DllExport.MSBuild.Properties.Resources.resourceCulture);
        }
    }

    public static string Skipped_Method_Exports
    {
        get
        {
            return RGiesecke.DllExport.MSBuild.Properties.Resources.ResourceManager.GetString("Skipped_Method_Exports", RGiesecke.DllExport.MSBuild.Properties.Resources.resourceCulture);
        }
    }

    public static string ToolLocationHelperTypeName_could_not_find_1
    {
        get
        {
            return RGiesecke.DllExport.MSBuild.Properties.Resources.ResourceManager.GetString("ToolLocationHelperTypeName_could_not_find_1", RGiesecke.DllExport.MSBuild.Properties.Resources.resourceCulture);
        }
    }

    public static string Cannot_find_0_
    {
        get
        {
            return RGiesecke.DllExport.MSBuild.Properties.Resources.ResourceManager.GetString("Cannot_find_0_", RGiesecke.DllExport.MSBuild.Properties.Resources.resourceCulture);
        }
    }

    internal Resources()
    {
    }
}
}

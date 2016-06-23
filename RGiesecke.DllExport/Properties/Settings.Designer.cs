// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.1.28776, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RGiesecke.DllExport.Properties
{
[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
public sealed class Settings : ApplicationSettingsBase
{
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default
    {
        get
        {
            return Settings.defaultInstance;
        }
    }

    [ApplicationScopedSetting]
    [DefaultSettingValue("")]
    [DebuggerNonUserCode]
    public string ILDasmPath
    {
        get
        {
            return (string) this["ILDasmPath"];
        }
    }

    [DefaultSettingValue("")]
    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    public string ILAsmPath
    {
        get
        {
            return (string) this["ILAsmPath"];
        }
    }
}
}

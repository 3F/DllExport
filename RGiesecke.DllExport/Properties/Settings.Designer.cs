// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.6.36226, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RGiesecke.DllExport.Properties
{
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
[CompilerGenerated]
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

    [DebuggerNonUserCode]
    [DefaultSettingValue("")]
    [ApplicationScopedSetting]
    public string ILDasmPath
    {
        get
        {
            return (string) this["ILDasmPath"];
        }
    }

    [DebuggerNonUserCode]
    [DefaultSettingValue("")]
    [ApplicationScopedSetting]
    public string ILAsmPath
    {
        get
        {
            return (string) this["ILAsmPath"];
        }
    }
}
}

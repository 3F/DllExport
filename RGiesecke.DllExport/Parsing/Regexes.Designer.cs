// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.2.23706, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace RGiesecke.DllExport.Parsing
{
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
public class Regexes
{
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static ResourceManager ResourceManager
    {
        get
        {
            if (object.ReferenceEquals((object) Regexes.resourceMan, (object) null)) {
                Regexes.resourceMan = new ResourceManager("RGiesecke.DllExport.Parsing.Regexes", typeof (Regexes).Assembly);
            }
            return Regexes.resourceMan;
        }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static CultureInfo Culture
    {
        get
        {
            return Regexes.resourceCulture;
        }
        set
        {
            Regexes.resourceCulture = value;
        }
    }

    public static string LineNumbers
    {
        get
        {
            return Regexes.ResourceManager.GetString("LineNumbers", Regexes.resourceCulture);
        }
    }

    public static string MethodDeclaration
    {
        get
        {
            return Regexes.ResourceManager.GetString("MethodDeclaration", Regexes.resourceCulture);
        }
    }

    public static string TypeDeclaration
    {
        get
        {
            return Regexes.ResourceManager.GetString("TypeDeclaration", Regexes.resourceCulture);
        }
    }

    internal Regexes()
    {
    }
}
}

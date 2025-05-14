/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System.IO;
using net.r_eg.DllExport.ILAsm;
using net.r_eg.DllExport.Wizard.Extensions;
using net.r_eg.MvsSln.Log;

namespace net.r_eg.DllExport.Wizard.Gears
{
    internal sealed class RefPackagesGear(IProjectSvc prj): ProjectGearAbstract(prj)
    {
        internal const string ID = Project.METALIB_PK_TOKEN + ":Ref";

        public override void Install()
        {
            string refPackages = Config.RefPackages.Serialize();
            prj.SetProperty(MSBuildProperties.DXP_REF_PACKAGES, refPackages);
            Log.send(this, $"Package references at the pre-processing stage ... : {refPackages}");

            if(Config.RefPackages.Count < 1) return;

            foreach(RefPackage rp in Config.RefPackages)
            {
                Log.send(this, $"[Ref]: {rp.Name} {rp.Version}");
                // NOTE: PrivateAssets cannot guarantee delivery of assemblies because some packages contains `_._` stubs
                XProject.AddPackageIfNotExists(rp.Name, rp.Version, prj.GetMeta(generatePath: true));
            }

            var target = prj.AddTarget(MSBuildTargets.DXP_REF_PROC);

            target.BeforeTargets = $"{MSBuildTargets.DXP_PRE_PROC};{MSBuildTargets.DXP_MAIN}";
            target.Label = ID;

            foreach(RefPackage rp in Config.RefPackages)
            {
                string src = $"$(Pkg{rp.Name.Trim().Replace('.', '_')})";
                src = !rp.HasPath
                    ? src = Path.Combine(src, "lib", rp.TfmOrPath, rp.Name + ".dll")
                    : Path.Combine(src, rp.TfmOrPath ?? rp.Name + ".dll");

                Log.send(this, $"[Ref] Add Copy Task for {rp.Name}:  {src}", Message.Level.Trace);
                AddCopyTo(target, src, "%(RefPkgsDestinationFolders.Identity)", ignoreErr: false);
            }
        }

        public override void Uninstall(bool hardReset)
        {
            while(prj.RemoveXmlTarget(MSBuildTargets.DXP_REF_PROC)) { }

            XProject.RemovePropertyGroups(p => p.Label == ID);
            prj.RemovePackageReferences(i => i.evaluated != UserConfig.PKG_ID);
        }
    }
}

/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace net.r_eg.DllExport.Wizard
{
    internal class LegacyPackagesFile
    {
        internal const string FNAME = "packages.config";
        internal const string ROOT  = "packages";
        internal const string ELEM  = "package";

        protected readonly string file;
        protected readonly XDocument xml;

        internal bool IsValidExists { get; }
        internal Exception ExceptionWhenInit { get; }

        internal bool AddOrUpdatePackage(string id, string version, string targetFramework)
        {
            var pkg = GetPackage(id);
            if(pkg == null)
            {
                return AddPackage(id, version, targetFramework);
            }

            pkg.SetAttributeValue(nameof(version), version);
            pkg.SetAttributeValue(nameof(targetFramework), targetFramework);
            xml.Save(file);
            return true;
        }

        internal bool RemoveSavedPackage(string id) => IsValidExists && RemovePackage(id);

        internal bool RemovePackage(string id)
        {
            var pkg = GetPackage(id);

            if(pkg == null) {
                return false;
            }

            pkg.Remove();
            xml.Save(file);
            return true;
        }

        internal LegacyPackagesFile(string path)
        {
            file = Path.Combine(path ?? throw new ArgumentNullException(nameof(path)), FNAME);

            if(!File.Exists(file))
            {
                xml = GetNewInstance();
                return;
            }

            try
            {
                xml = XDocument.Load(file);
                IsValidExists = true;
            }
            catch(Exception ex)
            {
                ExceptionWhenInit = ex;
                xml = GetNewInstance();
            }
        }

        protected XElement GetPackage(string id)
        {
            return xml.Element(ROOT).Elements().FirstOrDefault(p =>
                p.Attribute(nameof(id)).Value == (id ?? throw new ArgumentNullException(nameof(id)))
            );
        }

        protected bool AddPackage(string id, string version, string targetFramework)
        {
            xml.Element(ROOT).Add(new XElement
            (
                ELEM, 
                new XAttribute(nameof(id), id), 
                new XAttribute(nameof(version), version), 
                new XAttribute(nameof(targetFramework), targetFramework))
            );

            xml.Save(file);
            return true;
        }

        private XDocument GetNewInstance() => new XDocument(new XElement(ROOT));
    }
}

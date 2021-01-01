/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2021  Denis Kuzmin <x-3F@outlook.com> github/3F
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
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

/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System.IO;
using System.Text;
using net.r_eg.Conari.Log;

namespace net.r_eg.DllExport.NSBin
{
    public sealed class DDNS: IDDNS
    {
        private Encoding encoding;

        /// <summary>
        /// Available buffer for namespace.
        /// </summary>
        public int NSBuffer
        {
            get {
                return Rmod.NS_BUF_MAX;
            }
        }

        /// <summary>
        /// Access to logger.
        /// </summary>
        public ISender Log
        {
            get {
                return LSender._;
            }
        }

        /// <summary>
        /// Check that name of namespace is correct for using.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsValidNS(string name)
        {
            return Rmod.IsValidNS(name);
        }

        /// <summary>
        /// Returns path to XML metadata of the library.
        /// </summary>
        /// <param name="lib">Path to library.</param>
        /// <returns></returns>
        public static string GetMetaXml(string lib)
        {
            return Path.ChangeExtension(lib, ".xml");
        }

        /// <summary>
        /// Define namespace.
        /// </summary>
        /// <param name="lib">Full path to prepared library.</param>
        /// <param name="name">New namespace.</param>
        /// <param name="useCecil">Use Cecil instead of direct modification.</param>
        /// <param name="preparing">Preparing library is obsolete variant for previous distribution with nuget.</param>
        public void SetNamespace(string lib, string name, bool useCecil, bool preparing = true)
        {
            using(var def = new Rmod(lib, encoding)) {
                def.SetNamespace(name, useCecil, preparing);
            }
        }

        public DDNS(Encoding encoding)
        {
            this.encoding = encoding;
        }

        public DDNS()
            : this(Encoding.UTF8)
        {

        }
    }
}

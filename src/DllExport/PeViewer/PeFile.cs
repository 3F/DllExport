/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System.IO;
using net.r_eg.Conari.PE;

namespace net.r_eg.DllExport.PeViewer
{
    /// <summary>
    /// TODO: it's a first draft for `-list` command only.
    /// </summary>
    internal class PeFile
    {
        public static string[] GetExportNames(string module)
        {
            if(!File.Exists(module)) {
                throw new FileNotFoundException($"File '{module}' was not found.");
            }

            using(var pe = new PEFile(module)) {
                return pe.ExportedProcNamesArray;
            }
        }
    }
}

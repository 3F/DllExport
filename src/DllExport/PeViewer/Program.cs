/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using net.r_eg.Conari.PE;

namespace net.r_eg.DllExport.PeViewer
{
    internal class Program
    {
        internal static readonly Dictionary<string, bool> Switches = new()
        {
            { "-i", true },
            { "-pemodule", true },
            { "-list", false },
            { "-list-addr", false },
            { "-hex", false },
            { "-list-ord", false },
            { "-list-all", false },
            { "-magic", false },
            { "-num-functions", false },
            { "-base", false },
            { "-version", false },
            { "-h", false },
            { "-help", false },
        };

        static void Main(string[] args) => new Program(args);

        Program(string[] args)
        {
            ArgsMapper map = new(args, Switches);
            try
            {
                CheckUnknown(map);
                Cmd(map);
            }
            catch(Exception ex)
            {
                Msg(ex.Message, true);
                Msg();
                CmdHelp(map);
            }
        }

        private void Cmd(ArgsMapper map)
        {
            if(map.IsEmptyArgs || map.Is("-version"))
            {
#if PUBLIC_RELEASE
                Msg(DllExportVersion.S_INFO_P, newline: false);
#else
                Msg(DllExportVersion.S_NUM, newline: false);
#endif

#if DEBUG
                Msg(" Debug");
#endif
                Msg();
                Msg($"Powered by Conari {Conari.ConariVersion.S_NUM_REV}+{Conari.ConariVersion.BRANCH_SHA1}");
                Msg("https://github.com/3F/Conari"); return;
            }

            if(map.Is("-h") || map.Is("-help"))
            {
                CmdHelp(map); return;
            }

            map.Find(out string file, "-pemodule", "-i");

            bool hex = map.Is("-hex");

            if(map.Is("-list-all"))
            {
                CmdEx(file, x =>
                    PrintAll
                    (
                        x.Export.NameOrdinals.Select(o => o + x.DExport.Base),
                        x.Export.Names,
                        x.Export.Addresses,
                        hex
                    )
                );
                return;
            }

            if(map.Is("-list"))
            {
                CmdEx(file, x => Print(x.Export.Names)); return;
            }

            if(map.Is("-list-addr"))
            {
                CmdEx(file, x => Print(x.Export.Addresses, hex)); return;
            }

            if(map.Is("-list-ord"))
            {
                CmdEx(file, x => Print(x.Export.NameOrdinals, hex)); return;
            }

            if(map.Is("-magic"))
            {
                CmdPe(file, x => PrintStr((ushort)x.Magic, hex)); return;
            }

            if(map.Is("-num-functions"))
            {
                CmdPe(file, x => PrintStr(x.DExport.NumberOfFunctions, hex)); return;
            }

            if(map.Is("-base"))
            {
                CmdEx(file, x => PrintStr(x.DExport.Base, hex)); return;
            }

            throw new NotSupportedException
            (
                $"Invalid or Incomplete command: {string.Join(" ; ", map.AHelper.Used)} "
            );
        }

        private void CmdHelp(ArgsMapper map)
        {
            Msg("Available keys: ");
            foreach(string cmd in map.CommandsPrintVersion) Msg($"  {cmd}");
        }

        private void CmdEx(string pemodule, Action<PEFile> act) => CmdPe(pemodule, act, check: true);

        private void CmdPe(string pemodule, Action<PEFile> act, bool check = false)
        {
            if(pemodule == null)
            {
                throw new Exception("Missing key to load module.");
            }

            if(!File.Exists(pemodule))
            {
                throw new FileNotFoundException($"Module '{pemodule}' is not found.");
            }

            using PEFile pe = new(pemodule);
            if(!check || pe.DExport.NumberOfFunctions > 0)
            {
                act?.Invoke(pe);
            }
        }

        private void Print(IEnumerable list, bool hex = false)
        {
            foreach(object data in list) PrintStr(data, hex);
        }

        private void PrintStr(object data, bool hex = false)
        {
            Msg(hex ? $"0x{data:x8}" : data.ToString());
        }

        private void PrintAll(IEnumerable ords, IEnumerable names, IEnumerable addrs, bool hex = false)
        {
            IEnumerator eOrds = ords.GetEnumerator();
            IEnumerator eNames = names.GetEnumerator();
            IEnumerator eAddrs = addrs.GetEnumerator();

            while(eOrds.MoveNext() && eNames.MoveNext() && eAddrs.MoveNext())
            {
                Msg($"{eOrds.Current} {eNames.Current} {(hex ? $"0x{eAddrs.Current:x8}" : eAddrs.Current)}");
            }
        }

        private void CheckUnknown(ArgsMapper map)
        {
            string[] nonUsed = map?.AHelper?.NonUsed?.ToArray();

            if(nonUsed.Length > 0)
            {
                foreach(string arg in nonUsed) Msg($" ?-> {arg}", true);
                throw new ArgumentException("Unknown or Incorrect arguments.");
            }
        }

        private void Msg(string data = "", bool stderr = false, bool newline = true)
        {
            if(newline) data += Environment.NewLine;

            if(!stderr)
            {
                Console.Write(data);
                return;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.Write(data);
            Console.ResetColor();
        }
    }
}

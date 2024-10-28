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
            { "-pemodule", true },
            { "-list", false },
            { "-list-addr", false },
            { "-hex", false },
            { "-list-ord", false },
            { "-magic", false },
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

            map.Is("-pemodule", out string file);

            if(map.Is("-list"))
            {
                CmdPe(file, x => Print(x.Export.Names)); return;
            }

            if(map.Is("-list-addr"))
            {
                CmdPe(file, x => Print(x.Export.Addresses, map.Is("-hex"))); return;
            }

            if(map.Is("-list-ord"))
            {
                CmdPe(file, x => Print(x.Export.NameOrdinals, map.Is("-hex"))); return;
            }

            if(map.Is("-magic"))
            {
                CmdPe(file, x => PrintStr((ushort)x.Magic, map.Is("-hex"))); return;
            }

            throw new NotSupportedException
            (
                $"Unsupported keys: {string.Join(" ; ", map.AHelper.Used)} "
            );
        }

        private void CmdHelp(ArgsMapper map)
        {
            Msg("Available keys: ");
            foreach(string cmd in map.CommandsPrintVersion) Msg($"  {cmd}");
        }

        private void CmdPe(string pemodule, Action<PEFile> act)
        {
            if(pemodule == null)
            {
                throw new ArgumentNullException($"-{nameof(pemodule)}");
            }

            if(!File.Exists(pemodule))
            {
                throw new FileNotFoundException($"Module '{pemodule}' is not found.");
            }

            using PEFile pe = new(pemodule);
            act?.Invoke(pe);
        }

        private void Print(IEnumerable list, bool hex = false)
        {
            foreach(object data in list) PrintStr(data, hex);
        }

        private void PrintStr(object data, bool hex = false)
        {
            Msg(hex ? $"0x{data:x8}" : data.ToString());
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

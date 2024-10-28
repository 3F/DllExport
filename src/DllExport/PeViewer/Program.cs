/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using net.r_eg.Conari.PE;

namespace net.r_eg.DllExport.PeViewer
{
    class Program
    {
        static void Main(string[] args)
        {
            ArgsMapper map = new(args, new Dictionary<string, bool>()
            {
                { "-pemodule", true },
                { "-list", false },
                { "-version", false },
                { "-h", false },
                { "-help", false },
            });

            try
            {
                CheckUnknown(map);
                Cmd(map);
            }
            catch(Exception ex)
            {
                Msg(ex.Message, true);
                Msg("");
                CmdHelp(map);
            }
        }

        private static void Cmd(ArgsMapper map)
        {
            if(map.IsEmptyArgs || map.Is("-version"))
            {
#if PUBLIC_RELEASE
                Msg(DllExportVersion.S_INFO_P, newline: false);
#else
                Msg(DllExportVersion.S_NUM, newline: false);
#endif

#if DEBUG
                Msg(" [Debug]");
#endif
                Msg("");
                Msg($"Powered by Conari {Conari.ConariVersion.S_NUM_REV}+{Conari.ConariVersion.BRANCH_SHA1}");
                Msg("https://github.com/3F/Conari");
                return;
            }

            if(map.Is("-h") || map.Is("-help"))
            {
                CmdHelp(map);
                return;
            }

            map.Is("-pemodule", out string aPeFile);

            if(map.Is("-list"))
            {
                CmdList(aPeFile);
                return;
            }

            throw new NotSupportedException
            (
                $"Unsupported keys: {string.Join(" ; ", map.AHelper.Used)} "
            );
        }

        private static void CmdHelp(ArgsMapper map)
        {
            Msg("Active commands: ");
            foreach(string cmd in map.CommandsPrintVersion) Msg($"  {cmd}");
        }

        private static void CmdList(string pemodule)
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
            foreach(string name in pe.ExportedProcNames) Msg(name);
        }

        private static void CheckUnknown(ArgsMapper map)
        {
            string[] nonUsed = map?.AHelper?.NonUsed?.ToArray();

            if(nonUsed == null || nonUsed.Length < 1) return;

            foreach(string arg in nonUsed)
            {
                Msg($" ?-> `{arg}`", true);
            }

            throw new ArgumentException("Unknown or Incorrect arguments. Use `-help` to list available commands.");
        }

        private static void Msg(string data, bool stderr = false, bool newline = true)
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

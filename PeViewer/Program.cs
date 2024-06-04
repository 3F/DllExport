/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace net.r_eg.DllExport.PeViewer
{
    /// <summary>
    /// TODO: it's a first draft for `-list` command only.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var map = new ArgsMapper(args, new Dictionary<string, bool>()
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
            catch(Exception ex) {
                Msg(ex.Message, true);
            }
        }

        private static void Cmd(ArgsMapper map)
        {
            if(map.IsEmptyArgs || map.Is("-version")) {
                Msg(Assembly.GetExecutingAssembly().GetName().Version.ToString());
                Msg("");
                Msg("Under the Conari license.");
                Msg($"Conari: v{Conari.ConariVersion.S_NUM_REV} [{Conari.ConariVersion.BRANCH_SHA1}]");
                Msg("Src: github.com/3F ");
                Msg("     x-3F@outlook.com ");
                return;
            }

            if(map.Is("-h") || map.Is("-help"))
            {
                Msg("Active commands: ");
                foreach(var cmd in map.CommandsPrintVersion) {
                    Msg($"  {cmd}");
                }
                return;
            }

            map.Is("-pemodule", out string aPeFile);

            if(map.Is("-list")) {
                CmdList(aPeFile);
                return;
            }

            throw new NotSupportedException(
                $"Something went wrong. We can't handle this commands correctly: {String.Join(" ; ", map.AHelper.Used)} "
            );
        }

        private static void CmdList(string pemodule)
        {
            if(pemodule == null) {
                throw new ArgumentNullException($"-{nameof(pemodule)}");
            }

            foreach(var name in PeFile.GetExportNames(pemodule)) {
                Msg(name);
            }
        }

        private static void CheckUnknown(ArgsMapper map)
        {
            var nonUsed = map?.AHelper?.NonUsed?.ToArray();

            if(nonUsed == null || nonUsed.Length < 1) {
                return;
            }

            foreach(var arg in nonUsed) {
                Msg($" ?-> `{arg}`", true);
            }

            throw new ArgumentException("Unknown or Incorrect arguments. Use `-help` to list available commands.");
        }

        private static void Msg(string data, bool stderr = false)
        {
            if(!stderr) {
                Console.WriteLine(data);
                return;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(data);
            Console.ResetColor();
        }
    }
}

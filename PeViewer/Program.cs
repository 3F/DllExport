/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
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

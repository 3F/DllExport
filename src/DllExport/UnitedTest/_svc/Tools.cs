/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Diagnostics;
using System.IO;

namespace net.r_eg.DllExport.UnitedTest._svc
{
    internal sealed class Tools
    {
        public static string RunPeViewer(string args) => RunPeViewer(args, out _);

        public static string RunPeViewer(string args, out string stderr)
            => Run(Assets.PkgPeViewer, args, Assets.PkgToolsDir, out stderr);

        public static string Run(string file, string args, string dir, out string stderr)
        {
            if(!File.Exists(file)) throw new FileNotFoundException();

            Process p = new();
            p.StartInfo.FileName = file ?? throw new ArgumentNullException(nameof(file));

            p.StartInfo.Arguments = args ?? string.Empty;
            p.StartInfo.UseShellExecute = false;

            if(!string.IsNullOrEmpty(dir)) p.StartInfo.WorkingDirectory = dir;

            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;

            p.Start();
            p.WaitForExit(30_000);

            stderr = p.StandardError.ReadToEnd();
            return p.StandardOutput.ReadToEnd();
        }
    }
}

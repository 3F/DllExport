/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Threading;
using System.Windows.Forms;
using net.r_eg.MvsSln.Log;

namespace net.r_eg.DllExport.Wizard.UI
{
    internal static class App
    {
        private static object sync = new object();

        /// <summary>
        /// Execute form with single-threaded apartment model.
        /// </summary>
        /// <param name="frm"></param>
        public static void RunSTA(Form frm)
        {
            var thread = new Thread(() => Run(frm));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            if(!frm.IsDisposed) {
                frm.Dispose();
            }
        }

        private static void Run(Form frm)
        {
            Application.EnableVisualStyles();
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.Automatic);

            lock(sync) {
                Application.ThreadException -= OnThreadException;
                Application.ThreadException += OnThreadException;
            }

            try {
                Application.Run(frm);
            }
            catch(Exception ex) {
                UnknownFail(ex, false);
            }
        }

        private static void UnknownFail(Exception ex, bool threadEx)
        {
            LSender.Send<Executor>(
                $"{ex.Message}{(threadEx ? "[TH]" : "[M]")}\n---\n{ex.ToString()}", 
                MvsSln.Log.Message.Level.Fatal
            );
        }

        private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            UnknownFail(e.Exception, true);
        }
    }
}

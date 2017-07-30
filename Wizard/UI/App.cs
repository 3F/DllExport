/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2017  Denis Kuzmin < entry.reg@gmail.com > :: github.com/3F
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

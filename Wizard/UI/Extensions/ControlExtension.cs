/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2021  Denis Kuzmin <x-3F@outlook.com> github/3F
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
using net.r_eg.Conari.Accessors.WinAPI;

namespace net.r_eg.DllExport.Wizard.UI.Extensions
{
    public static class ControlExtension
    {
        internal const uint WM_SETREDRAW = 0x000B;

        public static void SuspendDraw(this Control ctrl)
        {
            using(dynamic l = new User32()) {
                l.SendMessageW(ctrl.Handle, WM_SETREDRAW, 0, 0);
            }
        }

        public static void ResumeDraw(this Control ctrl)
        {
            using(dynamic l = new User32()) {
                l.SendMessageW(ctrl.Handle, WM_SETREDRAW, 1, 0);
            }
            ctrl.Refresh();
        }

        public static void Pause(this Control ctrl)
        {
            ctrl.SuspendDraw();
            ctrl.SuspendLayout();
        }

        public static void Resume(this Control ctrl)
        {
            ctrl.ResumeLayout();
            ctrl.ResumeDraw();
        }

        public static void Dispose(this Control.ControlCollection controls)
        {
            object[] ctrls = new object[controls.Count];
            controls.CopyTo(ctrls, 0);

            foreach(var ctrl in ctrls)
            {
                if(ctrl is IDisposable elem) {
                    elem.Dispose();
                }
            }
        }

        /// <summary>
        /// Executes an Action through BeginInvoke if it's required.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="method"></param>
        public static void UIAction(this Control control, Action method)
            => UIAction(control, (x) => method());

        /// <summary>
        /// Executes an Action through BeginInvoke if it's required.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="method"></param>
        public static void UIAction(this Control control, Action<Control> method)
            => UIAction<Control>(control, (x) => method(x));

        /// <summary>
        /// Executes an Action through BeginInvoke if it's required.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctrl"></param>
        /// <param name="method"></param>
        public static void UIAction<T>(this T ctrl, Action<T> method) where T: Control
        {
            if(ctrl.InvokeRequired) {
                ctrl.BeginInvoke((MethodInvoker)delegate { method(ctrl); });
            }
            else {
                method(ctrl);
            }
        }

        internal static void SetData(this TextBox control, string text, bool newline = true)
        {
            if(newline) {
                text += Environment.NewLine;
            }

            control.SelectionStart = 0;
            control.Text = text;
        }

        internal static void AppendData(this TextBox control, string text, bool newline = true)
        {
            if(newline) {
                text += Environment.NewLine;
            }
            control.AppendText(text);

            control.SelectionStart = control.Text.Length;
            control.ScrollToCaret();
        }

        internal static void UIBlinkText(this Control ctrl, int delay, string text, CancellationToken ct, params string[] effects)
        {
            while(!ct.IsCancellationRequested)
            {
                foreach(var ef in effects)
                {
                    ctrl.UIAction(x => x.Text = ef + text);
                    Thread.Sleep(delay);

                    if(ct.IsCancellationRequested) { return; }
                }
            }
        }

        internal static void ForegroundAction(this Form ctrl, Action<Form> act = null)
        {
            ctrl.TopMost = false;
            act?.Invoke(ctrl);
            ctrl.TopMost = true;
        }
    }
}

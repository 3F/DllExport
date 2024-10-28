/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
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
                ctrl.Invoke(() => method(ctrl));
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

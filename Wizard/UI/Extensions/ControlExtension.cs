/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2018  Denis Kuzmin < entry.reg@gmail.com > :: github.com/3F
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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace net.r_eg.DllExport.Wizard.UI.Extensions
{
    public static class ControlExtension
    {
        internal static class NativeMethods
        {
            public const uint WM_SETREDRAW = 0x000B;

            [DllImport("user32.dll")]
            public static extern IntPtr SendMessage(IntPtr hWnd, uint wMsg, IntPtr wParam, IntPtr lParam);
        }

        public static void SuspendDraw(this Control ctrl)
        {
            NativeMethods.SendMessage(ctrl.Handle, NativeMethods.WM_SETREDRAW, new IntPtr(0), new IntPtr(0));
        }

        public static void ResumeDraw(this Control ctrl)
        {
            NativeMethods.SendMessage(ctrl.Handle, NativeMethods.WM_SETREDRAW, new IntPtr(1), new IntPtr(0));
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
        {
            if(control.InvokeRequired) {
                control.BeginInvoke((MethodInvoker)delegate { method(); });
            }
            else {
                method();
            }
        }
    }
}

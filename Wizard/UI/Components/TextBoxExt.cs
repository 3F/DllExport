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

using System.Drawing;
using System.Windows.Forms;

namespace net.r_eg.DllExport.Wizard.UI.Components
{
    internal class TextBoxExt: TextBox
    {
        private const int WM_PAINT = 0x000F;

        public string BackgroundCaption { get; set; }

        public int BackgroundCaptionAlpha { get; set; } = 60;

        protected void DrawString(string str, int alpha)
        {
            Graphics g = CreateGraphics();

            g.DrawString(str, Font, new SolidBrush(Color.FromArgb(alpha, ForeColor)), ClientRectangle, new StringFormat
            {
                Alignment = StringAlignment.Near
            });
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if(m.Msg == WM_PAINT)
            {
                if(!string.IsNullOrEmpty(BackgroundCaption) && string.IsNullOrEmpty(Text)) 
                {
                    DrawString(BackgroundCaption, BackgroundCaptionAlpha);
                }
            }
        }
    }
}

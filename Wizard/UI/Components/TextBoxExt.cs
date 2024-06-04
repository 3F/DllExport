/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
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
            using(Graphics g = CreateGraphics())
            {
                g.DrawString(str, Font, new SolidBrush(Color.FromArgb(alpha, ForeColor)), ClientRectangle, new StringFormat
                {
                    Alignment = StringAlignment.Near
                });
            }
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

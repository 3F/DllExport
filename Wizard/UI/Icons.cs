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
using System.Collections.Concurrent;
using System.Drawing;
using net.r_eg.MvsSln.Core;
using net.r_eg.MvsSln.Extensions;

namespace net.r_eg.DllExport.Wizard.UI
{
    internal sealed class Icons: IDisposable
    {
        private readonly Font txtFont = new Font("Microsoft Sans Serif", 6.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        private readonly SolidBrush txtBrush = new SolidBrush(Color.FromArgb(255, Color.Black));

        private readonly ConcurrentDictionary<ProjectType, Bitmap> raw = new ConcurrentDictionary<ProjectType, Bitmap>();

        public Bitmap GetIcon(ProjectType type)
        {
            switch(type)
            {
                case ProjectType.Cs:
                case ProjectType.CsSdk: return IconResources.CS_ProjectSENode_16x;

                case ProjectType.Vc: return IconResources.CPP_ProjectSENode_16x_;

                case ProjectType.Vb:
                case ProjectType.VbSdk: return IconResources.VB_ProjectSENode_16x;

                case ProjectType.Fs:
                case ProjectType.FsSdk: return IconResources.FS_ProjectSENode_16x;
            }

            return GetTextIcon(type);
        }

        public Bitmap GetTextIcon(ProjectType type)
        {
            if(!raw.ContainsKey(type)) 
            {
                raw[type] = DrawString(
                    (type == ProjectType.Unknown) ? "" : type.ToString(), 
                    new Bitmap(32, 16)
                );
            }
            return raw[type];
        }

        private Bitmap DrawString(string str, Bitmap bmp)
        {
            using(var g = Graphics.FromImage(bmp ?? throw new ArgumentNullException(nameof(bmp))))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                g.DrawString(str, txtFont, txtBrush, 0, 0);
            }
            return bmp;
        }

        #region IDisposable

        private bool disposed = false;

        void Dispose(bool _)
        {
            if(!disposed)
            {
                raw?.ForEach(b => b.Value?.Dispose());

                txtFont?.Dispose();
                txtBrush?.Dispose();

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}

/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
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

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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

        #endregion
    }
}

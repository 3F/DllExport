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
using System.Windows.Forms;
using net.r_eg.DllExport.Wizard.Extensions;

namespace net.r_eg.DllExport.Wizard.UI
{
    internal partial class InfoForm: Form
    {
        private IExecutor exec;

        public InfoForm(IExecutor exec)
        {
            this.exec = exec;
            InitializeComponent();

            Load += (object sender, EventArgs e) => { TopMost = false; TopMost = true; };
        }

        private void linkLocalDxp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            exec.Config.SlnDir.OpenUrl();
        }

        private void linkRemoteDxp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            "https://3F.github.io/DllExport/releases/latest/manager/".OpenUrl();
        }

        private void picVideo_Click(object sender, System.EventArgs e)
        {
            "https://www.youtube.com/watch?v=9bYgywZ9pPE".OpenUrl();
        }

        private void linkManagerWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            "https://github.com/3F/DllExport/wiki/DllExport-Manager".OpenUrl();
        }
    }
}

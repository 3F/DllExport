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
using System.Text;
using System.Windows.Forms;
using net.r_eg.DllExport.Wizard.UI.Extensions;

namespace net.r_eg.DllExport.Wizard.UI
{
    internal partial class MsgForm: Form
    {
        public int MsgLevel
        {
            get;
            set;
        }

        public void AddMsg(string msg, MvsSln.Log.Message.Level level)
        {
            if(!IsValidMsg(level)) {
                return;
            }

            if(!String.IsNullOrWhiteSpace(msg))
            {
                this.UIAction(() => {
                    listBoxLog.Items.Add(msg);
                });
            }
        }

        public MsgForm(int msgLevel)
        {
            MsgLevel = msgLevel;
            InitializeComponent();

            Load += (object sender, EventArgs e) => { TopMost = false; TopMost = true; };
        }

        private bool IsValidMsg(MvsSln.Log.Message.Level level)
        {
            if(MsgLevel < 0) {
                return false;
            }
            return ((int)level) >= MsgLevel;
        }

        private void menuSelectAll_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < listBoxLog.Items.Count; ++i) {
                listBoxLog.SetSelected(i, true);
            }
        }

        private void menuCopySel_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();

            foreach(string item in listBoxLog.SelectedItems) {
                sb.Append(item + Environment.NewLine);
            }

            if(sb.Length > 0) {
                Clipboard.SetText(sb.ToString());
            }
        }

        private void menuClear_Click(object sender, EventArgs e)
        {
            listBoxLog.Items.Clear();
        }

        private void listBoxLog_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Modifiers == Keys.Control && e.KeyCode == Keys.C) {
                menuCopySel_Click(sender, e);
            }
            else if(e.Modifiers == Keys.Control && e.KeyCode == Keys.A) {
                menuSelectAll_Click(sender, e);
            }
        }
    }
}

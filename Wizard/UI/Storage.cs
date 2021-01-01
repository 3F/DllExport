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
using System.Collections.Generic;
using System.Windows.Forms;

namespace net.r_eg.DllExport.Wizard.UI
{
    internal struct CfgStorage
    {
        private IExecutor exec;
        private List<Elems> storage;
        private ComboBox box;

        private struct Elems
        {
            public CfgStorageType type;
            public string name;

            public Elems(CfgStorageType type, string name)
            {
                this.type = type;
                this.name = name ?? throw new ArgumentNullException(nameof(name));
            }
        }

        public CfgStorageType GetItemType(int index)
        {
            if(index < 0 || index >= storage.Count) {
                return CfgStorageType.Default;
            }
            return storage[index].type;
        }

        public void UpdateItem()
        {
            if(exec?.Config != null) {
                SetItem(exec.Config.CfgStorage);
            }
        }

        public void SetItem(CfgStorageType type)
        {
            for(int i = 0; i < storage.Count; ++i) {
                if(storage[i].type == type) {
                    box.SelectedIndex = i;
                    return;
                }
            }
        }

        public CfgStorage(IExecutor exec, ComboBox box)
        {
            this.exec   = exec ?? throw new ArgumentNullException(nameof(exec));
            this.box    = box ?? throw new ArgumentNullException(nameof(box));

            storage = new List<Elems>() {
                new Elems(CfgStorageType.ProjectFiles, "Project files (.csproj, ...)"),
                new Elems(CfgStorageType.TargetsFile, TargetsFile.DEF_CFG_FILE)
            };

            Populate();
            box.SelectedIndexChanged += OnSelectedIndexChanged;
        }

        private void Populate()
        {
            box.Items.Clear();
            foreach(var elem in storage) {
                box.Items.Add(elem.name);
            }
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if(exec.Config != null) {
                exec.Config.CfgStorage = GetItemType(box.SelectedIndex);
            }
        }
    }
}

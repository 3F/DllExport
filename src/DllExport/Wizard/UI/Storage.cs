/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
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

/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System.Collections.Generic;
using System.Windows.Forms;
using net.r_eg.DllExport.Wizard.Extensions;
using net.r_eg.DllExport.Wizard.UI.Extensions;

namespace net.r_eg.DllExport.Wizard.UI.Controls
{
    public partial class RefControl: UserControl
    {
        public RefControl() => InitializeComponent();

        internal void Export(List<RefPackage> records)
        {
            if(records == null) return;
            records.Clear();
            records.Capacity = dgvList.Rows.Count;

            foreach(DataGridViewRow row in dgvList.Rows)
            {
                if(row.IsNewRow) continue;

                records.Add(new RefPackage()
                {
                    Name = GetTrimmed(row.Cells[colPackage.Name].Value),
                    Version = GetTrimmed(row.Cells[colVersion.Name].Value),
                    TfmOrPath = GetTrimmed(row.Cells[colTfmOrPath.Name].Value),
                });
            }
        }

        internal void Render(List<RefPackage> records)
        {
            dgvList.Suspend(() =>
            {
                dgvList.Rows.Clear();
                if(records == null) return;

                foreach(RefPackage r in records)
                {
                    dgvList.Rows.Add(r.Name, r.Version, r.TfmOrPath);
                }
            });
        }

        private bool AddIfNotExist(string package, string version, string tfm)
        {
            foreach(DataGridViewRow row in dgvList.Rows)
            {
                if(row.IsNewRow) continue;
                if((string)row.Cells[colPackage.Name].Value == package) return false;
            }

            dgvList.Rows.Add(package, version, tfm);
            return true;
        }

        private static string GetTrimmed(object value) => ((string)value)?.Trim();

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e) => dgvList.RemoveRow(e, colDelAsm);

        private void btnMemoryAndUnsafe_Click(object sender, System.EventArgs e)
        {
            AddIfNotExist("System.Memory", "4.6.0", "netstandard2.0");
            AddIfNotExist("System.Runtime.CompilerServices.Unsafe", "6.1.0", "netstandard2.0");
        }

        private void linkGetNuTool_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => "https://github.com/3F/GetNuTool".OpenUrl();
    }
}

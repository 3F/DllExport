/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System.Collections.Generic;
using System.Windows.Forms;
using net.r_eg.DllExport.Wizard.UI.Extensions;

namespace net.r_eg.DllExport.Wizard.UI.Controls
{
    public partial class AsmControl: UserControl
    {
        public AsmControl() => InitializeComponent();

        internal void Export(List<ILAsm.AssemblyExternDirective> directives)
        {
            if(directives == null) return;
            directives.Clear();
            directives.Capacity = dgvList.Rows.Count;

            foreach(DataGridViewRow row in dgvList.Rows)
            {
                if(row.IsNewRow) continue;

                directives.Add(new ILAsm.AssemblyExternDirective()
                {
                    Name = (string)row.Cells[colAssembly.Name].Value,
                    Publickeytoken = (string)row.Cells[colKeytoken.Name].Value,
                    Version = ((string)row.Cells[colAsmVer.Name].Value)?.Trim(),
                });
            }
        }

        internal void Render(List<ILAsm.AssemblyExternDirective> directives)
        {
            dgvList.Suspend(() =>
            {
                dgvList.Rows.Clear();
                if(directives == null) return;

                foreach(ILAsm.AssemblyExternDirective d in directives)
                {
                    dgvList.Rows.Add(d.Name, d.Publickeytoken, d.Version);
                }
            });
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e) => dgvList.RemoveRow(e, colDelAsm);
    }
}

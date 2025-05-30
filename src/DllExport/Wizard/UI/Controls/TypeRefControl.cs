﻿/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using net.r_eg.DllExport.Wizard.Extensions;
using net.r_eg.DllExport.Wizard.UI.Extensions;

namespace net.r_eg.DllExport.Wizard.UI.Controls
{
    public partial class TypeRefControl: UserControl
    {
        private const string KW_DENY = "deny";
        private const string KW_ASSERT = "assert";
        private const string KW_AT = "at";

        private const string INTERPOLATED_STRING_HANDLER = "System.Runtime.CompilerServices.DefaultInterpolatedStringHandler";

        public TypeRefControl() => InitializeComponent();

        internal void Export(IUserConfig config)
        {
            config.TypeRefOptions = GetTypeRefOptions();

            List<ILAsm.TypeRefDirective> directives = config.TypeRefDirectives;
            if(directives == null) return;

            directives.Clear();
            directives.Capacity = dgvList.Rows.Count;

            foreach(DataGridViewRow row in dgvList.Rows)
            {
                if(row.IsNewRow) continue;

                string scopeType = (string)row.Cells[colScopeType.Name].Value;
                if(string.IsNullOrEmpty(scopeType)) continue;

                directives.Add(new ILAsm.TypeRefDirective()
                {
                    Name = (string)row.Cells[colType.Name].Value,
                    ResolutionScope = (string)row.Cells[colScope.Name].Value,
                    Any = Convert.ToBoolean(row.Cells[colFlagAny.Name].Value),
                    Deny = scopeType == KW_DENY,
                    Assert = scopeType == KW_ASSERT,
                });
            }
        }

        internal void Render(IUserConfig config)
        {
            SetTypeRefOptions(config.TypeRefOptions);

            List<ILAsm.TypeRefDirective> directives = config.TypeRefDirectives;
            dgvList.Suspend(() =>
            {
                dgvList.Rows.Clear();
                if(directives == null) return;

                foreach(ILAsm.TypeRefDirective d in directives)
                    AddRow(d.Name, d.Any, d.Deny, d.Assert, d.ResolutionScope);
            });
        }

        private TypeRefOptions GetTypeRefOptions()
        {
            TypeRefOptions opt = TypeRefOptions.None;

            if(chkInterpolation.Checked) opt |= TypeRefOptions.DefaultInterpolatedStringHandler;

            return opt;
        }

        private void SetTypeRefOptions(TypeRefOptions opt)
        {
            chkInterpolation.Checked = (opt & TypeRefOptions.DefaultInterpolatedStringHandler) == TypeRefOptions.DefaultInterpolatedStringHandler;
        }

        private void AddRow(string name, bool any, bool deny, bool assert, string scope = null)
        {
            int idx = dgvList.Rows.Add(name, any, GetScopeType(deny, assert), scope);
            DisableColumns(dgvList.Rows[idx]);
        }

        private int FindTypeInRows(string name)
        {
            int index = -1;
            foreach(DataGridViewRow row in dgvList.Rows)
            {
                if(row.IsNewRow) continue;
                if((string)row.Cells[colType.Name].Value == name) return row.Index;
            }
            return index;
        }

        private string GetScopeType(bool deny, bool assert)
        {
            if(deny) return KW_DENY;
            else if(assert) return KW_ASSERT;
            return KW_AT;
        }

        private void DisableColumns(DataGridViewRow row)
        {
            string scopeType = (string)row.Cells[colScopeType.Name].Value;

            DataGridViewCell scope = row.Cells[colScope.Name];
            if(scopeType != KW_AT)
            {
                scope.ReadOnly = true;
                scope.Style.BackColor = System.Drawing.SystemColors.Control;
                scope.Value = string.Empty;
            }
            else
            {
                scope.ReadOnly = false;
                scope.Style.BackColor = System.Drawing.SystemColors.Window;
            }
        }

        private void linkTypeRef_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => "https://github.com/3F/coreclr/#typeref-custom-definitions".OpenUrl();

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e) => dgvList.RemoveRow(e, colDelTyperef);

        private void dgvList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex == -1 || e.ColumnIndex != dgvList.Columns.IndexOf(colScopeType)) return;
            DisableColumns(dgvList.Rows[e.RowIndex]);
        }

        private void dgvList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if(e.RowIndex == -1) return;
            DataGridViewCell scope = dgvList.Rows[e.RowIndex].Cells[colScopeType.Name];
            if(string.IsNullOrEmpty((string)scope.Value)) scope.Value = KW_AT;
        }

        private void chkInterpolation_CheckedChanged(object sender, EventArgs e)
        {
            if(chkInterpolation.Checked && FindTypeInRows(INTERPOLATED_STRING_HANDLER) == -1)
            {
                AddRow(INTERPOLATED_STRING_HANDLER, any: false, deny: false, assert: true);
            }
        }
    }
}

/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

// Based on version from https://github.com/3F/vsSolutionBuildEvent
// 3b82a028fe955947ab05b1396375ca84696afff5

using System;
using System.Drawing;
using System.Windows.Forms;
using net.r_eg.DllExport.Wizard.UI.Extensions;

namespace net.r_eg.DllExport.Wizard.UI.Components
{
    internal class DataGridViewExt: DataGridView
    {
        public event EventHandler<MovingRowArgs> DragDropSortedRow = (sender, e) => { };

        public sealed class MovingRowArgs: EventArgs { public int from, to; }

        /// <summary>
        /// Custom column: for work with numeric formats with standard TextBoxCell 
        /// </summary>
        internal class NumericColumn: DataGridViewColumn
        {
            public bool Decimal { get; set; }
            public bool Negative { get; set; }

            public NumericColumn()
                : base(new DataGridViewTextBoxCell())
            {

            }
        }

        /// <summary>
        /// Shows total count of rows and current position for each row
        /// </summary>
        public bool NumberingForRowsHeader { get; set; }

        /// <summary>
        /// Allows sorting with Drag 'n' Drop
        /// </summary>
        public bool DragDropSortable
        {
            get => dragDropSortable;
            set
            {
                dragDropSortable = value;
                setupSortable(value);
            }
        }
        protected bool dragDropSortable = false;

        /// <summary>
        /// Always one row selected
        /// </summary>
        public bool AlwaysSelected { get; set; }

        /// <summary>
        /// Support AlwaysSelected
        /// </summary>
        protected int lastSelectedRowIndex = 0;

        /// <summary>
        /// Support drag 'n' drop for sortable rows
        /// </summary>
        protected MovingRowArgs ddSort = new();

        protected readonly SolidBrush sbBlack = new(Color.Black);

        private readonly StringFormat rowNumFormat;

        private readonly object _eLock = new();
        private bool disposed;

        public DataGridViewExt()
            : this(rowHeight: 17)
        {

        }

        public DataGridViewExt(int rowHeight) // optional arguments in .ctor are not supported in generator due to incorrect activator use
        {
            RowTemplate.Height = this.GetValueUsingDpi(rowHeight);

            rowNumFormat = new()
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
            };

            CellPainting += onNumberingCellPainting;
            SelectionChanged += onAlwaysSelected;
            EditingControlShowing += (sender, e) =>
            {
                if(e.Control == null) return;
                e.Control.KeyPress += onControlKeyPress;
                e.Control.PreviewKeyDown += onControlPreviewKeyDown;
            };
        }

        protected void onControlKeyPress(object sender, KeyPressEventArgs e)
        {
            if(sender == null || sender.GetType() != typeof(DataGridViewTextBoxEditingControl))
            {
                return;
            }
            DataGridView dgv = ((DataGridViewTextBoxEditingControl)sender).EditingControlDataGridView;

            if(dgv.CurrentCell.OwningColumn.GetType() != typeof(NumericColumn))
            {
                return;
            }
            //(NumericColumn)dgv.CurrentCell.OwningColumn;

            if(!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        protected void onSortableDragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        protected void onSortableDragDrop(object sender, DragEventArgs e)
        {
            Point point = PointToClient(new Point(e.X, e.Y));
            ddSort.to = HitTest(point.X, point.Y).RowIndex;

            if(e.Effect != DragDropEffects.Move || ddSort.to == -1 || Rows.Count < 1 || Rows[ddSort.to].IsNewRow)
            {
                return;
            }
            e.Effect = DragDropEffects.None;

            Rows.RemoveAt(ddSort.from);
            Rows.Insert(ddSort.to, (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow)));
            ClearSelection();
            Rows[ddSort.to].Selected = true;

            DragDropSortedRow(this, ddSort);
        }

        protected void onSortableMouseMove(object sender, MouseEventArgs e)
        {
            if((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                if(ddSort.from == -1 || Rows.Count < 1 || Rows[ddSort.from].IsNewRow) return;
                DoDragDrop(Rows[ddSort.from], DragDropEffects.Move);
            }
        }

        protected void onSortableMouseDown(object sender, MouseEventArgs e)
        {
            ddSort.from = HitTest(e.X, e.Y).RowIndex;
        }

        protected void setupSortable(bool enabled)
        {
            if(enabled)
            {
                AllowDrop = true;
                enableSortEvents();
                return;
            }
            disableSortEvents();
        }

        protected void enableSortEvents()
        {
            lock(_eLock)
            {
                disableSortEvents();
                DragOver   += onSortableDragOver;
                DragDrop   += onSortableDragDrop;
                MouseMove  += onSortableMouseMove;
                MouseDown  += onSortableMouseDown;
            }
        }

        protected void disableSortEvents()
        {
            lock(_eLock)
            {
                DragOver   -= onSortableDragOver;
                DragDrop   -= onSortableDragDrop;
                MouseMove  -= onSortableMouseMove;
                MouseDown  -= onSortableMouseDown;
            }
        }

        protected virtual void numberingRowsHeader(DataGridViewCellPaintingEventArgs e)
        {
            if(e.ColumnIndex != -1) return;
            if(e.RowIndex == Rows.Count - 1) return;

            e.PaintBackground(e.CellBounds, false);
            e.Graphics.DrawString
            (
                e.RowIndex >= 0 ? $"{e.RowIndex + 1}"
                    : (Rows.Count > 1 ? $"{Rows.Count - 1}" : ""),

                e.CellStyle.Font,
                sbBlack,
                e.CellBounds,
                rowNumFormat
            );
            e.Handled = true;
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if(keyData == Keys.Enter)
            {
                EndEdit();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        protected override void Dispose(bool disposing)
        {
            if(!disposed)
            {
                sbBlack.Dispose();
                rowNumFormat.Dispose();
                disposed = true;
            }
            base.Dispose(disposing);
        }

        private void onNumberingCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if(NumberingForRowsHeader)
            {
                numberingRowsHeader(e);
            }
        }

        private void onAlwaysSelected(object sender, EventArgs e)
        {
            if(!AlwaysSelected || Rows.Count < 1)
            {
                return;
            }

            if(SelectedRows.Count < 1)
            {
                lastSelectedRowIndex = Math.Max(0, Math.Min(lastSelectedRowIndex, Rows.Count - 1));
                Rows[lastSelectedRowIndex].Selected = true;
                return;
            }
            lastSelectedRowIndex = SelectedRows[0].Index;
        }

        /// <summary>
        /// A trick with left/right keys in EditMode of text columns.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onControlPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if(sender == null || sender.GetType() != typeof(DataGridViewTextBoxEditingControl))
            {
                return;
            }
            var box = (DataGridViewTextBoxEditingControl)sender;
            int pos = box.SelectionStart;

            if(box.Text.Length < 1)
            {
                return;
            }

            if(pos == 0 && e.KeyData == Keys.Left)
            {
                BeginEdit(false);
                box.SelectionStart = Math.Min(1, box.Text.Length); // will decrease with std handler
                return;
            }

            if(pos == box.Text.Length && e.KeyData == Keys.Right)
            {
                BeginEdit(false);
                box.SelectionStart = box.Text.Length - 1; // also will with std handler later
                return;
            }
        }
    }
}

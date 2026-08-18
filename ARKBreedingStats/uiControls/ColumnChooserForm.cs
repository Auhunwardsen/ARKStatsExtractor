using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ARKBreedingStats.uiControls
{
    /// <summary>
    /// Generic dialog to show/hide and reorder the columns of a ListView (Details view).
    /// Visibility is implemented the same way the rest of the app already does it: a hidden column has Width 0.
    /// Changes are applied live to the passed ListView so the user sees a preview; Cancel reverts to the state at opening.
    /// </summary>
    internal sealed class ColumnChooserForm : Form
    {
        private readonly ListView _listView;
        private readonly CheckedListBox _checkedListBox;
        private readonly Dictionary<int, int> _lastNonZeroWidth = new Dictionary<int, int>();
        private readonly int[] _originalWidths;
        private readonly int[] _originalDisplayIndices;
        private readonly Action _onApplied;
        private readonly Func<ColumnHeader, string> _labelSelector;
        private readonly TextBox _filterBox;
        private readonly Button _btnMoveUp;
        private readonly Button _btnMoveDown;
        private bool _updatingCheckStates;

        /// <summary>
        /// Default width used when a column that has no known previous width (i.e. it was already hidden when the dialog was opened) gets checked visible again.
        /// </summary>
        private const int FallbackColumnWidth = 60;

        /// <summary>
        /// </summary>
        /// <param name="labelSelector">Optional. Returns the label to display for a column; return null or an empty string to fall back to the column's own header text.</param>
        public ColumnChooserForm(ListView listView, string title, Action onApplied, Func<ColumnHeader, string> labelSelector = null)
        {
            _listView = listView ?? throw new ArgumentNullException(nameof(listView));
            _onApplied = onApplied;
            _labelSelector = labelSelector;

            _originalWidths = _listView.Columns.Cast<ColumnHeader>().Select(c => c.Width).ToArray();
            _originalDisplayIndices = _listView.Columns.Cast<ColumnHeader>().Select(c => c.DisplayIndex).ToArray();

            foreach (ColumnHeader c in _listView.Columns)
            {
                if (c.Width > 0) _lastNonZeroWidth[c.Index] = c.Width;
            }

            Text = title;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimizeBox = false;
            MaximizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(300, 450);
            MinimumSize = new Size(260, 300);

            var lbHint = new Label
            {
                Text = "Check the columns to display. Select an entry and use the buttons to reorder. Type below to filter, e.g. to find only the mutation-level columns.",
                Dock = DockStyle.Top,
                AutoSize = false,
                Height = 56,
                Padding = new Padding(8, 8, 8, 0)
            };

            _filterBox = new TextBox
            {
                Dock = DockStyle.Top,
                Margin = new Padding(8, 0, 8, 4),
                PlaceholderText = "Filter, e.g. \"mutation\"..."
            };
            var filterPanel = new Panel { Dock = DockStyle.Top, Height = 28, Padding = new Padding(8, 0, 8, 4) };
            _filterBox.Dock = DockStyle.Fill;
            filterPanel.Controls.Add(_filterBox);
            _filterBox.TextChanged += (s, e) => PopulateList();

            _checkedListBox = new CheckedListBox
            {
                Dock = DockStyle.Fill,
                CheckOnClick = true,
                IntegralHeight = false
            };
            _checkedListBox.ItemCheck += CheckedListBox_ItemCheck;

            var btnOk = new Button { Text = "OK", DialogResult = DialogResult.OK, AutoSize = true };
            var btnCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, AutoSize = true };
            _btnMoveUp = new Button { Text = "Move up", AutoSize = true };
            _btnMoveDown = new Button { Text = "Move down", AutoSize = true };
            _btnMoveUp.Click += (s, e) => MoveSelected(-1);
            _btnMoveDown.Click += (s, e) => MoveSelected(1);

            // added right-to-left, so visually the reading order ends up OK, Cancel on the first row, Move buttons on the second
            var row1 = new FlowLayoutPanel { FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Margin = Padding.Empty };
            row1.Controls.Add(btnOk);
            row1.Controls.Add(btnCancel);
            var row2 = new FlowLayoutPanel { FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Margin = Padding.Empty };
            row2.Controls.Add(_btnMoveDown);
            row2.Controls.Add(_btnMoveUp);

            var buttonStack = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Padding = new Padding(8)
            };
            buttonStack.Controls.Add(row1);
            buttonStack.Controls.Add(row2);

            AcceptButton = btnOk;
            CancelButton = btnCancel;

            Controls.Add(_checkedListBox);
            Controls.Add(buttonStack);
            Controls.Add(filterPanel);
            Controls.Add(lbHint);

            PopulateList();
            _btnMoveUp.Enabled = false;
            _btnMoveDown.Enabled = false;
            _checkedListBox.SelectedIndexChanged += (s, e) =>
            {
                var hasFilter = !string.IsNullOrWhiteSpace(_filterBox.Text);
                _btnMoveUp.Enabled = !hasFilter && _checkedListBox.SelectedIndex > 0;
                _btnMoveDown.Enabled = !hasFilter && _checkedListBox.SelectedIndex >= 0 && _checkedListBox.SelectedIndex < _checkedListBox.Items.Count - 1;
            };

            FormClosing += (s, e) =>
            {
                if (DialogResult == DialogResult.OK)
                {
                    _onApplied?.Invoke();
                }
                else
                {
                    RevertToOriginal();
                }
            };
        }

        private void PopulateList()
        {
            _updatingCheckStates = true;
            _checkedListBox.Items.Clear();
            var filter = _filterBox.Text?.Trim();
            var ordered = _listView.Columns.Cast<ColumnHeader>().OrderBy(c => c.DisplayIndex);
            foreach (var col in ordered)
            {
                var label = _labelSelector?.Invoke(col);
                if (string.IsNullOrWhiteSpace(label))
                    label = string.IsNullOrWhiteSpace(col.Text) ? $"(column {col.Index + 1})" : col.Text;
                if (!string.IsNullOrEmpty(filter) && label.IndexOf(filter, StringComparison.OrdinalIgnoreCase) < 0)
                    continue;
                _checkedListBox.Items.Add(new ColumnEntry(col, label), col.Width > 0);
            }
            _updatingCheckStates = false;
        }

        private void CheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_updatingCheckStates) return;
            if (!(_checkedListBox.Items[e.Index] is ColumnEntry entry)) return;

            var col = entry.Column;
            if (e.NewValue == CheckState.Checked)
            {
                col.Width = _lastNonZeroWidth.TryGetValue(col.Index, out var w) && w > 0 ? w : FallbackColumnWidth;
            }
            else
            {
                if (col.Width > 0) _lastNonZeroWidth[col.Index] = col.Width;
                col.Width = 0;
            }
        }

        private void MoveSelected(int direction)
        {
            var index = _checkedListBox.SelectedIndex;
            if (index < 0) return;
            var newIndex = index + direction;
            if (newIndex < 0 || newIndex >= _checkedListBox.Items.Count) return;

            var entryA = (ColumnEntry)_checkedListBox.Items[index];
            var entryB = (ColumnEntry)_checkedListBox.Items[newIndex];

            // swap DisplayIndex of the two affected columns
            (entryA.Column.DisplayIndex, entryB.Column.DisplayIndex) = (entryB.Column.DisplayIndex, entryA.Column.DisplayIndex);

            var wasChecked = _checkedListBox.GetItemChecked(index);
            var otherChecked = _checkedListBox.GetItemChecked(newIndex);

            _updatingCheckStates = true;
            _checkedListBox.Items[index] = entryB;
            _checkedListBox.Items[newIndex] = entryA;
            _checkedListBox.SetItemChecked(index, otherChecked);
            _checkedListBox.SetItemChecked(newIndex, wasChecked);
            _updatingCheckStates = false;

            _checkedListBox.SelectedIndex = newIndex;
        }

        private void RevertToOriginal()
        {
            for (int c = 0; c < _originalWidths.Length && c < _listView.Columns.Count; c++)
            {
                _listView.Columns[c].Width = _originalWidths[c];
            }
            // set indices in increasing order so they don't push each other around while applying
            var ordered = _originalDisplayIndices.Select((di, c) => (columnIndex: c, displayIndex: di))
                .Where(x => x.columnIndex < _listView.Columns.Count)
                .OrderBy(x => x.displayIndex);
            foreach (var x in ordered)
                _listView.Columns[x.columnIndex].DisplayIndex = x.displayIndex;
        }

        private sealed class ColumnEntry
        {
            public ColumnHeader Column { get; }
            private readonly string _label;

            public ColumnEntry(ColumnHeader column, string label)
            {
                Column = column;
                _label = label;
            }

            public override string ToString() => _label;
        }
    }
}

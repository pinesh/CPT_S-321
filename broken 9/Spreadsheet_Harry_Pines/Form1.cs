// <copyright file="Form1.cs" company="Harry Pines - 11578059 - Cpts 321">
// Copyright (c) Harry Pines - 11578059 - Cpts 321. All rights reserved.
// </copyright>

namespace Spreadsheet_Harry_Pines
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.Xml;
    using CptS321;

    /// <summary>
    /// The main entry point to the Form for the application.
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();
            this.VarSpreadsheet = new Spreadsheet(rowI: 50, colI: 26);
            this.VarSpreadsheet.PropertyChanged += this.NotifyCellChangedHandler;
            this.undoToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Text = @"Redo";
            this.undoToolStripMenuItem.Text = @"Undo";

            // VISUAL TABLE STUFF ---------------------------------------
            for (var i = 65; i <= 90; i++)
            {
                var c = ((char)i).ToString();
                this.gridView.Columns.Add(columnName: c, headerText: c);
            }

            this.gridView.Rows.Add(count: 50);
            foreach (DataGridViewRow gridRow in this.gridView.Rows)
            {
                gridRow.HeaderCell.Value = string.Format(format: "{0}", arg0: gridRow.Index + 1);
            }
        }

        /// <summary>
        /// Gets the dataGridView for test.
        /// </summary>
        public DataGridView DataGrid => this.gridView;

        /// <summary>
        /// Gets the spreadsheet for test.
        /// </summary>
        public Spreadsheet VarSpreadsheet { get; }

        /// <summary>
        /// The event that drives the UI layer to update a cell.
        /// </summary>
        /// <param name="source"> The source  object, in this case an instance of a HelperCell.</param>
        /// <param name="p"> The property event change p.</param>
        private void NotifyCellChangedHandler(object source, PropertyChangedEventArgs p)
        {
            switch (p.PropertyName)
            {
                case "Text":
                    this.gridView.Rows[((HelperCell)source).RowIndex].Cells[((HelperCell)source).ColumnIndex].Value = this
                        .VarSpreadsheet.GridCells[((HelperCell)source).RowIndex, ((HelperCell)source).ColumnIndex].Value;
                    break;
                case "Colour":
                    this.gridView[((HelperCell)source).ColumnIndex, ((HelperCell)source).RowIndex].Style.BackColor =
                        System.Drawing.Color.FromArgb((int)this.VarSpreadsheet
                            .GridCells[((HelperCell)source).RowIndex, ((HelperCell)source).ColumnIndex].BgColour);
                    break;
            }

            this.RefreshUndoRedo();
        }

        /// <summary>
        /// Demo code event from early homework.
        /// </summary>
        /// <param name="sender">the DataGridView.</param>
        /// <param name="e">The DataGridViewEvent.</param>
        private void Button1_Click(object sender, EventArgs e)
        {
            var rnd = new Random();
            for (var i = 0; i < 50; i++)
            {
                this.VarSpreadsheet.UpdateCell(row: rnd.Next(minValue: 1, maxValue: 49), col: rnd.Next(minValue: 1, maxValue: 26), value: "Hello World!");
            }

            for (var i = 1; i <= 50; i++)
            {
                var tempString = "This is cell B" + i;
                this.VarSpreadsheet.UpdateCell(row: i - 1, col: 1, value: tempString);
            }

            for (var i = 1; i <= 50; i++)
            {
                var tempString = "=B" + i;
                this.VarSpreadsheet.UpdateCell(row: i - 1, col: 0, value: tempString);
            }
        }

        /// <summary>
        /// gridView Event Start.
        /// </summary>
        /// <param name="sender">the DataGridView.</param>
        /// <param name="e">The DataGridViewEvent.</param>
        private void GridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            this.gridView.Rows[index: e.RowIndex].Cells[index: e.ColumnIndex].Value =
                this.VarSpreadsheet.GetCellFromPos(e.RowIndex, e.ColumnIndex).Text;
        }

        /// <summary>
        /// gridView Event End.
        /// </summary>
        /// <param name="sender">the DataGridView.</param>
        /// <param name="e">The DataGridViewEvent.</param>
        private void GridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var a = ((DataGridView)sender)[e.ColumnIndex, e.RowIndex].Value;

            var newStr = a == null ? string.Empty : a.ToString();

            if (!newStr.Equals(this.VarSpreadsheet.GetCellFromPos(e.RowIndex, e.ColumnIndex).Text))
            {
                this.VarSpreadsheet.UpdateCell(e.RowIndex, e.ColumnIndex, newStr);
            }
            else
            {
                this.gridView.Rows[index: e.RowIndex].Cells[index: e.ColumnIndex].Value =
                    this.VarSpreadsheet.GetCellFromPos(e.RowIndex, e.ColumnIndex).Value;
            }
        }

        /// <summary>
        /// Updates the buttons on state change.
        /// </summary>
        private void RefreshUndoRedo()
        {
            if (this.VarSpreadsheet.UndoRedo.CanUndo())
            {
                this.undoToolStripMenuItem.Enabled = true;
                this.undoToolStripMenuItem.Text = @"Undo " + this.VarSpreadsheet.UndoRedo.UndoPeak();
            }
            else
            {
                this.undoToolStripMenuItem.Enabled = false;
                this.undoToolStripMenuItem.Text = @"Undo";
            }

            if (this.VarSpreadsheet.UndoRedo.CanRedo())
            {
                this.redoToolStripMenuItem.Enabled = true;
                this.redoToolStripMenuItem.Text = @"Redo " + this.VarSpreadsheet.UndoRedo.RedoPeak();
            }
            else
            {
                this.redoToolStripMenuItem.Enabled = false;
                this.redoToolStripMenuItem.Text = @"Redo";
            }
        }

        /// <summary>
        /// Undo button click.
        /// </summary>
        /// <param name="sender">Menuitem object.</param>
        /// <param name="e">Event arg.</param>
        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.VarSpreadsheet.UndoRedo.Undo();
        }

        /// <summary>
        /// Redo button click.
        /// </summary>
        /// <param name="sender">Menuitem object.</param>
        /// <param name="e">Event arg.</param>
        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.VarSpreadsheet.UndoRedo.Redo();
        }

        /// <summary>
        /// Opens the file dialog to save an xml.
        /// </summary>
        /// <param name="sender">the DataGridView.</param>
        /// <param name="e">The DataGridViewEvent.</param>
        private void SaveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Assign properties.
            var saveFileDialog1 = new SaveFileDialog
            {
                Filter = "xml files (*.xml)|*.xml",
                FilterIndex = 2,
                RestoreDirectory = true,
            };

            // If the user saved a file.
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
             this.VarSpreadsheet.WriteXml(saveFileDialog1.FileName);
            }
        }

        /// <summary>
        /// Opens the file dialog to load in an xml.
        /// </summary>
        /// <param name="sender">the DataGridView.</param>
        /// <param name="e">The DataGridViewEvent.</param>
        private void OpenFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Assign properties.
            var openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse xml Files",
                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "xml",
                Filter = "xml files (*.xml)|*.xml",
                FilterIndex = 2,
                RestoreDirectory = true,
                ReadOnlyChecked = true,
                ShowReadOnly = true,
            };

            // Open the file.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.VarSpreadsheet.ReadInXml(openFileDialog1.FileName);
            }

            this.RefreshUndoRedo();
        }

        /// <summary>
        /// Button event to change the background colour of a selected group of cells.
        /// </summary>
        /// <param name="sender">The gridView.</param>
        /// <param name="e">The button event.</param>
        private void ChangeBackgroundColourToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            var myDialog = new ColorDialog();

            if (myDialog.ShowDialog() == DialogResult.OK)
            {
                var newList = new List<(int, int)>();
                var selectedCells = this.gridView.SelectedCells;
                for (var i = 0; i < selectedCells.Count; i++)
                {
                    newList.Add((selectedCells[i].RowIndex, selectedCells[i].ColumnIndex));
                }

                this.VarSpreadsheet.UpdateCellsColour(newList, (uint)myDialog.Color.ToArgb());
                this.gridView.ClearSelection();
            }
        }
    }
}

// <copyright file="Spreadsheet.cs" company="Harry Pines - 11578059 - Cpts 321">
// Copyright (c) Harry Pines - 11578059 - Cpts 321. All rights reserved.
// </copyright>

namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Xml;
    using SpreadsheetEngine;


    /// <summary>
    /// The spreadsheet class; Creates an instance of cells and handles they assigning and events between the abstract cell class and the UI layer.
    /// </summary>
    public class Spreadsheet : Cell
    {
        private readonly HashSet<(int, int)> modCells;

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// </summary>
        /// <param name="rowI"> row index.</param>
        /// <param name="colI"> col index.</param>
        public Spreadsheet(int rowI, int colI)
            : base(rowI: rowI, colI: colI)
        {
            this.modCells = new HashSet<(int, int)>();
            this.GridCells = new HelperCell[rowI, colI];
            this.UndoRedo = new UndoRedoCollection();
            for (var x = 0; x < this.GridCells.GetLength(dimension: 0); x += 1)
            {
                for (var y = 0; y < this.GridCells.GetLength(dimension: 1); y += 1)
                {
                    var newCell = new HelperCell(rowI: x, colI: y);

                    // this.NotifyCellPropertyChanged += newCell.NotifyPropertyChanged;
                    this.GridCells[x, y] = newCell;

                    // newCell.PropertyChanged += NotifyCellPropertyChanged;
                    this.GridCells[x, y].PropertyChanged += this.NotifyCellPropertyChanged;
                }
            }
        }

        /// <summary>
        /// Cell property changed event handler.
        /// </summary>
        public new event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets The 2D Array of cells.
        /// </summary>
        public HelperCell[,] GridCells { get; }

        /// <summary>
        /// Gets for undoRedoStack.
        /// </summary>
        public UndoRedoCollection UndoRedo { get; }

        /// <summary>
        /// This will later act as the factory for new spreadsheets.
        /// </summary>
        /// <param name="rowI">takes the row dimension.</param>
        /// <param name="colI">takes the col dimension.</param>
        /// <returns> A new spreadsheet.</returns>
        public static Spreadsheet CreateSpreadsheet(int rowI, int colI)
        {
            return new Spreadsheet(rowI: rowI, colI: colI);
        }

        /// <summary>
        /// This sets the new text from the GUI.
        /// </summary>
        /// <param name="row">Target row. </param>
        /// <param name="col">Target col. </param>
        /// <param name="value">new textValue. </param>
        public void UpdateCell(int row, int col, string value)
        {
            var oldText = this.GridCells[row, col].Text;
            this.modCells.Add((row, col));
            var action = new UndoRedoAction
            {
                Do = () => { this.GridCells[row, col].Text = value; },
                Undo = () => { this.GridCells[row, col].Text = oldText; },
                Title = "Text Change",
            };
            this.UndoRedo.AddItem(new List<UndoRedoAction> { action });
        }

        /// <summary>
        /// Sets the new colour from the GUI.
        /// </summary>
        /// <param name="xyList">The list of selected cell coordinates.</param>
        /// <param name="newColour">The new colour.</param>
        public void UpdateCellsColour(List<(int, int)> xyList, uint newColour)
        {
            var actionList = new List<UndoRedoAction>();
            foreach (var (row, col) in xyList)
            {
                this.modCells.Add((row, col));
                var oldBg = this.GridCells[row, col].BgColour;
                var action = new UndoRedoAction
                {
                    Do = () => this.GridCells[row, col].BgColour = (uint)newColour,
                    Undo = () => this.GridCells[row, col].BgColour = oldBg,
                    Title = "Colour Change",
                };

                actionList.Add(action);
            }

            this.UndoRedo.AddItem(actionList);
        }

        /// <summary>
        /// Converts a mixed col name to number (example: ABA). This doesn't yet handle lower case interpretations or non char letters.
        /// </summary>
        /// <param name="colText">The name of the col.</param>
        /// <returns>The col number as a double. </returns>
        public double ConvertColToInt(string colText)
        {
            if (colText.Length == 1)
            {
                return colText[index: 0] - 65;
            }

            return (Math.Pow(x: 26, y: colText.Length - 1) * (colText[index: 0] - 65)) +
                   this.ConvertColToInt(colText: colText.Substring(startIndex: 1));
        }

        /// <summary>
        /// Gets a cell based on name.
        /// </summary>
        /// <param name="cellName">The string cell-name.</param>
        /// <returns>The cell.</returns>
        public ref HelperCell GetCell(string cellName)
        {
            for (var i = 0; i < cellName.Length; i++)
            {
                if (char.IsDigit(cellName[i]))
                {
                    try
                    {
                        if (!int.TryParse(cellName.Substring(i), out var row))
                        {
                            throw new Exception();
                        }

                        return ref this.GridCells[row - 1, (int)this.ConvertColToInt(cellName.Substring(0, i))];
                    }
                    catch (Exception)
                    {
                        // If we got an exception, our conversion was out of bounds.
                        throw new IndexOutOfRangeException();
                    }
                }
            }

            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Gets a cell based on position.
        /// </summary>
        /// <param name="row">The int row.</param>
        /// <param name="col">The int col.</param>
        /// <returns>The cell.</returns>
        public HelperCell GetCellFromPos(int row, int col)
        {
            try
            {
                return this.GridCells[row, col];
            }
            catch (Exception)
            {
                throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Finds a node labeled cell, then calls child function to pull the parameters.
        /// </summary>
        /// <param name="path">The xml reader path.</param>
        public void ReadInXml(string path)
        {
            using (var reader = XmlReader.Create(path))
            {
                this.ClearCells();
                this.modCells.Clear();
                reader.MoveToContent();
                if (reader.IsEmptyElement)
                {
                    reader.Read();
                    return;
                }

                reader.Read();
                while (!reader.EOF)
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.Name == "cell")
                        {
                            this.ReadFromXml(reader);
                        }
                        else
                        {
                            reader.Skip();
                        }
                    }
                    else
                    {
                        reader.Read();
                        break;
                    }
                }

                this.UndoRedo.Clear();
            }
        }

        /// <summary>
        /// Writes the current spreadsheet to a file path created in the saveDialog.
        /// </summary>
        /// <param name="path">The filePath.</param>
        public void WriteXml(string path)
        {
            using (var writer = XmlWriter.Create(path, null))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                var cellList = this.modCells.ToList();
                foreach (var (row, col) in cellList)
                {
                    var currentCell = this.GridCells[row, col];
                    writer.WriteStartElement("cell");
                    writer.WriteAttributeString("name", currentCell.StringName);

                    writer.WriteElementString("bgcolor", currentCell.BgColour.ToString("X"));

                    writer.WriteElementString("text", currentCell.Text);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        /// <summary>
        /// Xml reader child.
        /// </summary>
        /// <param name="reader">The xml reader stream.</param>
        public void ReadFromXml(XmlReader reader)
        {
            reader.MoveToContent();

            // Read node attributes
            var cellName = reader.GetAttribute("name");
            if (cellName != null)
            {
                var targetCell = this.GetCell(cellName);
                if (reader.IsEmptyElement)
                {
                    reader.Read();
                    return;
                }

                reader.Read();
                while (!reader.EOF)
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "bgcolor":
                                var uintValue = Convert.ToUInt32(reader.ReadElementContentAsString(), 16);
                                this.UpdateCellsColour(new List<(int, int)> { (targetCell.RowIndex, targetCell.ColumnIndex) }, uintValue);
                                break;

                            case "text":
                                var text = reader.ReadElementContentAsString();
                                this.UpdateCell(targetCell.RowIndex, targetCell.ColumnIndex, text);
                                break;

                            default:
                                reader.Skip();
                                break;
                        }
                    }
                    else
                    {
                        reader.Read();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// The redone event driven handler which formulates the expression and determines what to set in order to notify the GUI.
        /// </summary>
        /// <param name="sender">Ideally an instance of HelperCell.</param>
        /// <param name="e">The property changed event argument.</param>
        private void NotifyCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var senderCell = (HelperCell)sender;
            if (sender != null)
            {
                if (e.PropertyName == "Text")
                {
                    // Only evaluate the cell if first char is '=', else just copy Text over.
                    if (senderCell.Text.StartsWith("="))
                    {
                        if (senderCell.Text.Substring(1).StartsWith("\"") && senderCell.Text.EndsWith("\""))
                        {
                            senderCell.SetEval(senderCell.Text.Substring(2, senderCell.Text.Length - 3));
                            this.PropertyChanged?.Invoke(senderCell, new PropertyChangedEventArgs("Text"));
                            return;
                        }

                        try
                        {
                            if (senderCell.UsesList.Count != 0)
                            {
                                foreach (var usedCell in senderCell.UsesList.Where(usedCell => usedCell != null))
                                {
                                    this.GetCell(usedCell).PropertyChanged -= senderCell.CellPropertyChanged;
                                }

                                senderCell.UsesList.Clear();
                            }

                            var newTree = new ExpressionTree(senderCell.Text.Substring(1));
                            var variables = newTree.GetVariableNames();
                            foreach (var varName in variables)
                            {
                                try
                                {
                                    var cell = this.GetCell(varName);
                                    this.SearchChildren(senderCell, cell);
                                    if (double.TryParse(cell.Value, out var cellValue))
                                    {
                                        newTree.SetVariable(varName, cellValue);
                                        senderCell.UsesList.Add(varName);
                                    }
                                    else
                                    {
                                        newTree.SetVariable(varName, 0);
                                    }

                                    cell.PropertyChanged += senderCell.CellPropertyChanged;
                                }

                                // Handle out of bound or circular expressions.
                                catch (IndexOutOfRangeException)
                                {
                                    newTree.SetVariable(varName, 0);
                                }
                                catch (Exception)
                                {
                                    senderCell.SetEval("Error - Circular Ref");
                                    this.PropertyChanged?.Invoke(senderCell, new PropertyChangedEventArgs("Text"));
                                    return;
                                }
                            }

                            ((HelperCell)sender).SetEval(newTree.Evaluate().ToString());
                        }
                        catch (Exception)
                        {
                            // This exception would be caused by expression tree failing.
                            senderCell.SetEval("Error - Invalid Expression");
                            this.PropertyChanged?.Invoke(senderCell, new PropertyChangedEventArgs("Text"));
                            return;
                        }
                    }

                    this.PropertyChanged?.Invoke(senderCell, new PropertyChangedEventArgs("Text"));
                }

                // COLOUR CODE.
                else if (e.PropertyName == "Colour")
                {
                    this.PropertyChanged?.Invoke(senderCell, new PropertyChangedEventArgs("Colour"));
                }
            }
            else
            {
                throw new Exception("Invalid source");
            }
        }

        /// <summary>
        /// This function searches the list of cells used in order to determine if a circular reference has occured.
        /// </summary>
        /// <param name="targetCell">The cell we are currently in.</param>
        /// <param name="memberCell">The cell we wish to investigate.</param>
        /// <returns>An int if passed and an exception if failed.</returns>
        private int SearchChildren(HelperCell targetCell, HelperCell memberCell)
        {
            if (memberCell == targetCell)
            {
                throw new Exception();
            }

            return memberCell.UsesList.Count != 0
                ? memberCell.UsesList.Select(member => 0 + this.SearchChildren(targetCell, this.GetCell(member)))
                    .FirstOrDefault()
                : 0;
        }

        /// <summary>
        /// Wipes the spreadsheet and undo stack.
        /// </summary>
        private void ClearCells()
        {
            // Reset everything in this spreadsheet to default.
            foreach (var cell in this.GridCells)
            {
                if (cell.BgColour != 0xFFFFFFFF)
                {
                    cell.BgColour = 0xFFFFFFFF;
                }

                if (cell.Text != string.Empty)
                {
                    cell.Text = string.Empty;
                }
            }

            // Now clear the undo and redo stacks.
            this.UndoRedo.Clear();
        }
    }
}
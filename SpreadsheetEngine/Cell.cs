// <copyright file="Cell.cs" company="Harry Pines - 11578059 - Cpts 321">
// Copyright (c) Harry Pines - 11578059 - Cpts 321. All rights reserved.
// </copyright>

namespace CptS321
{
    using System.ComponentModel;

    /// <summary>
    /// instance of abstract class Cell.
    /// </summary>
    public abstract class Cell : INotifyPropertyChanged
    {
        /// <summary>
        /// Stores the evaluated text if starts with "=".
        /// </summary>
        internal string Evaluated = string.Empty;

        /// <summary>
        /// Stores the text.
        /// </summary>
        private string text = string.Empty;

        /// <summary>
        /// Stores the cell colour, default.
        /// </summary>
        private uint bgColour = 0xFFFFFFFF;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="rowI"> The new row index.</param>
        /// <param name="colI"> The new col index.</param>
        protected Cell(int rowI, int colI)
        {
            this.RowIndex = rowI;
            this.ColumnIndex = colI;
            this.StringName = this.ConstructCol(colI + 1) + (rowI + 1);
        }

        /// <summary>
        /// primary event for change of property value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets row index property.
        /// </summary>
        public int RowIndex { get; }

        /// <summary>
        /// Gets col index property.
        /// </summary>
        public int ColumnIndex { get; }

        /// <summary>
        /// Gets The string cell name.
        /// </summary>
        public string StringName { get; }

        /// <summary>
        /// Gets or Sets text property.
        /// </summary>
        public string Text
        {
            get => this.text;
            set
            {
                if (value != this.text)
                {
                    this.text = value;
                    this.NotifyOnPropertyChanged("Text");
                }
            }
        }

        /// <summary>
        /// Gets or sets value property.
        /// </summary>
        public string Value
        {
            get
            {
                if (this.Text.Length == 0)
                {
                    return string.Empty;
                }

                return this.Text.StartsWith(value: "=") ? this.Evaluated : this.Text;
            }

            set
            {
                if (value != this.Evaluated)
                {
                    this.Evaluated = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the cell colour property (ARGB).
        /// </summary>
        public uint BgColour
        {
            get => this.bgColour;

            set
            {
                if (value != this.bgColour)
                {
                    this.bgColour = value;
                    this.NotifyOnPropertyChanged("Colour");
                }
            }
        }

        /// <summary>
        /// Base event handler overload for outside subscription.
        /// </summary>
        /// <param name="sender">The HelperCell object.</param>
        /// <param name="e">The event argument.</param>
        public void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e.PropertyName));
        }

        /// <summary>
        /// microsoft reference base event handler.
        /// </summary>
        /// <param name="propertyName"> Default property name.</param>
        protected void NotifyOnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(sender: this, e: new PropertyChangedEventArgs(propertyName: propertyName));
        }

        /// <summary>
        /// Converts an int to its associated col.
        /// </summary>
        /// <param name="col">The column integer.</param>
        /// <returns>The string letter that represents col.</returns>
        private string ConstructCol(int col)
        {
            if (col < 1)
            {
                return string.Empty;
            }

            return this.ConstructCol((int)((col - 1) / 26)) + (char)(((col - 1) % 26) + 65);
        }
    }
}
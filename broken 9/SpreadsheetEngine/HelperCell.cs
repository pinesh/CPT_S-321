// <copyright file="HelperCell.cs" company="Harry Pines - 11578059 - Cpts 321">
// Copyright (c) Harry Pines - 11578059 - Cpts 321. All rights reserved.
// </copyright>

namespace CptS321
{
    using System.Collections.Generic;

    /// <summary>
    /// This class handles the abstract Cell class the way evaluated is assigned may be incorrect.
    /// </summary>
    public class HelperCell : Cell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HelperCell"/> class.
        /// </summary>
        /// <param name="rowI"> row number. </param>
        /// <param name="colI"> col number. </param>
        public HelperCell(int rowI, int colI)
            : base(rowI: rowI, colI: colI)
        {
            this.UsesList = new List<string>();
        }

        /// <summary>
        /// Gets or Sets, This list keeps track of cells it uses.
        /// </summary>
        public List<string> UsesList { get; set; }

        /// <summary>
        /// This sets the evaluated value.
        /// </summary>
        /// <param name="newEval">The new string.</param>
        public void SetEval(string newEval)
        {
            this.Evaluated = newEval;
        }
    }
}
// <copyright file="UndoRedoAction.cs" company="Harry Pines - 11578059 - Cpts 321">
// Copyright (c) Harry Pines - 11578059 - Cpts 321. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;

    /// <summary>
    /// Stores the command actions and the title of the action.
    /// </summary>
    public class UndoRedoAction
    {
        /// <summary>
        /// Gets or Sets the undo action.
        /// </summary>
        public Action Undo { get; set; }

        /// <summary>
        /// Gets or Sets the redo action.
        /// </summary>
        public Action Do { get; set; }

        /// <summary>
        /// Gets or Sets the action title.
        /// </summary>
        public string Title { get; set; }
    }
}
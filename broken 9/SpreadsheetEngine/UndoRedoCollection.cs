// <copyright file="UndoRedoCollection.cs" company="Harry Pines - 11578059 - Cpts 321">
// Copyright (c) Harry Pines - 11578059 - Cpts 321. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System.Collections.Generic;

    /// <summary>
    /// This collection handles all undo and redo operations.
    /// </summary>
    public class UndoRedoCollection
    {
        private readonly Stack<List<UndoRedoAction>> undoStack;
        private readonly Stack<List<UndoRedoAction>> redoStack;

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoRedoCollection"/> class.
        /// Basic constructor.
        /// </summary>
        public UndoRedoCollection()
        {
            this.undoStack = new Stack<List<UndoRedoAction>>();
            this.redoStack = new Stack<List<UndoRedoAction>>();
        }

        /// <summary>
        /// Wipes both stacks.
        /// </summary>
        public void Clear()
        {
            this.undoStack.Clear();
            this.redoStack.Clear();
        }

        /// <summary>
        /// Adds an item to the undo and clears the redo stack.
        /// </summary>
        /// <param name="actions">The list of actions.</param>
        public void AddItem(List<UndoRedoAction> actions)
        {
            foreach (var action in actions)
            {
                action.Do();
            }

            this.undoStack.Push(actions);
            this.redoStack.Clear();
        }

        /// <summary>
        /// Performs the action(s) at the top of the undo stack.
        /// </summary>
        public void Undo()
        {
            if (this.CanUndo())
            {
                var newActions = this.undoStack.Pop();
                foreach (var action in newActions)
                {
                    action.Undo();
                }

                this.redoStack.Push(newActions);
            }
        }

        /// <summary>
        /// Performs the action(s) at the top of the redo stack.
        /// </summary>
        public void Redo()
        {
            if (this.CanRedo())
            {
                var newActions = this.redoStack.Pop();
                foreach (var action in newActions)
                {
                    action.Do();
                }

                this.undoStack.Push(newActions);
            }
        }

        /// <summary>
        /// Bool check to see if undo stack > 0.
        /// </summary>
        /// <returns>A bool.</returns>
        public bool CanUndo()
        {
            return this.undoStack.Count > 0;
        }

        /// <summary>
        /// Bool check to see if redo stack > 0.
        /// </summary>
        /// <returns>A bool.</returns>
        public bool CanRedo()
        {
            return this.redoStack.Count > 0;
        }

        /// <summary>
        /// Informs the caller if there is an undo message.
        /// </summary>
        /// <returns>The undo message or null.</returns>
        public string UndoPeak()
        {
            return this.CanUndo() ? this.undoStack.Peek()[0].Title : null;
        }

        /// <summary>
        /// Informs the caller if there is an redo message.
        /// </summary>
        /// <returns>The redo message or null.</returns>
        public string RedoPeak()
        {
            return this.CanRedo() ? this.redoStack.Peek()[0].Title : null;
        }
    }
}
// <copyright file="VarNode.cs" company="Harry Pines - 11578059 - Cpts 321">
// Copyright (c) Harry Pines - 11578059 - Cpts 321. All rights reserved.
// </copyright>

namespace CptS321
{
    /// <summary>
    /// This class handles nodes that store variables.
    /// </summary>
    public class VarNode : Node
    {
        /// <summary>
        /// The multiplier state.
        /// </summary>
        private readonly int multiplier;

        /// <summary>
        /// The variable name of this node.
        /// </summary>
        private readonly string variableName;

        /// <summary>
        /// Initializes a new instance of the <see cref="VarNode"/> class.
        /// </summary>
        /// <param name="x"> The string variable name.</param>
        /// <param name="y"> The sign of the variable.</param>
        public VarNode(string x, int y)
        {
            this.variableName = x;
            this.multiplier = y;
        }

        /// <summary>
        /// Public getter for the multiplier.
        /// </summary>
        /// <returns>The multiplier.</returns>
        public int GetMultiplier()
        {
            return this.multiplier;
        }

        /// <summary>
        /// Public getter for name.
        /// </summary>
        /// <returns>The variable name.</returns>
        public string GetName()
        {
            return this.variableName;
        }
    }
}
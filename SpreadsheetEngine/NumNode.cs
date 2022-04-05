// <copyright file="NumNode.cs" company="Harry Pines - 11578059 - Cpts 321">
// Copyright (c) Harry Pines - 11578059 - Cpts 321. All rights reserved.
// </copyright>

namespace CptS321
{
    /// <summary>
    /// Class that handles nodes of numbers.
    /// </summary>
    public class NumNode : Node
    {
        private readonly double num;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumNode"/> class.
        /// </summary>
        /// <param name="f">The double value carried by this Node.</param>
        public NumNode(double f)
        {
            this.num = f;
        }

        /// <summary>
        /// Public getter for the value _num.
        /// </summary>
        /// <returns>A double.</returns>
        public double Number()
        {
            return this.num;
        }
    }
}
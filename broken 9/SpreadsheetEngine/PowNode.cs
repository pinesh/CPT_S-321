// <copyright file="PowNode.cs" company="Harry Pines - 11578059 - Cpts 321">
// Copyright (c) Harry Pines - 11578059 - Cpts 321. All rights reserved.
// </copyright>

namespace CptS321
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This child handles the exponential operator.
    /// </summary>
    public class PowNode : OpNode
    {
        /// <summary>
        /// Gets Operator property.
        /// </summary>
        public static char Operator => '^';

        /// <summary>
        /// Gets op.
        /// </summary>
        public override char Op => Operator;

        /// <summary>
        /// This method overload takes the exponential of two nodes.
        /// </summary>
        /// <param name="varDict">The variable Dictionary from the Expression Tree.</param>
        /// <returns>The evaluated double. </returns>
        public override double DoOperator(Dictionary<string, double> varDict)
        {
            return Math.Pow(x: Evaluate(node: this.Left, varDict: varDict), y: Evaluate(node: this.Right, varDict: varDict));
        }

        /// <summary>
        /// Normalize with power rules.
        /// </summary>
        /// <param name="input">The input expression.</param>
        /// <returns>The normalized string.</returns>
        public override string Normalize(string input)
        {
            return input.Replace(this.Op.ToString(), this.ForTranValue(Operator));
        }

        /// <summary>
        /// Returns precedence.
        /// </summary>
        /// <returns>The bracket number.</returns>
        internal override int Precedence()
        {
            return 1;
        }
    }
}
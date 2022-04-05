// <copyright file="SubNode.cs" company="Harry Pines - 11578059 - Cpts 321">
// Copyright (c) Harry Pines - 11578059 - Cpts 321. All rights reserved.
// </copyright>

namespace CptS321
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// This child class handles subtraction.
    /// </summary>
    public class SubNode : OpNode
    {
        /// <summary>
        /// Gets Operator property.
        /// </summary>
        public static char Operator => '-';

        /// <summary>
        /// Gets op.
        /// </summary>
        public override char Op => Operator;

        /// <summary>
        /// This method overload subtracts two nodes.
        /// </summary>
        /// <param name="varDict">The variable Dictionary from the Expression Tree.</param>
        /// <returns>The evaluated double. </returns>
        public override double DoOperator(Dictionary<string, double> varDict)
        {
            return Evaluate(node: this.Left, varDict: varDict) - Evaluate(node: this.Right, varDict: varDict);
        }

        /// <summary>
        /// Normalize with subtraction rules.
        /// </summary>
        /// <param name="input">The input expression.</param>
        /// <returns>The normalized string.</returns>
        public override string Normalize(string input)
        {
            var sb = new StringBuilder();
            sb.Append(input.First());
            var temp = input.Substring(1);
            sb.Append(temp.Replace(this.Op.ToString(), this.ForTranValue(Operator)));
            return sb.ToString();
        }

        /// <summary>
        /// Returns precedence.
        /// </summary>
        /// <returns>The bracket number.</returns>
        internal override int Precedence()
        {
            return 3;
        }
    }
}
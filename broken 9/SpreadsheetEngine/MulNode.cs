// <copyright file="MulNode.cs" company="Harry Pines - 11578059 - Cpts 321">
// Copyright (c) Harry Pines - 11578059 - Cpts 321. All rights reserved.
// </copyright>

namespace CptS321
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// This child node handles multiplication.
    /// </summary>
    public class MulNode : OpNode
    {
        /// <summary>
        /// Gets Operator property.
        /// </summary>
        public static char Operator => '*';

        /// <summary>
        /// Gets op.
        /// </summary>
        public override char Op => Operator;

        /// <summary>
        /// This method overload multiplies two nodes.
        /// </summary>
        /// <param name="varDict">The variable Dictionary from the Expression Tree.</param>
        /// <returns>The evaluated double. </returns>
        public override double DoOperator(Dictionary<string, double> varDict)
        {
            return Evaluate(node: this.Left, varDict: varDict) * Evaluate(node: this.Right, varDict: varDict);
        }

        /// <summary>
        /// Normalizes the input string to fit for precedence and multiplication rules.
        /// </summary>
        /// <param name="input"> The input string.</param>
        /// <returns>The modified input string.</returns>
        public override string Normalize(string input)
        {
            var sb = new StringBuilder();
            var temp = new StringBuilder();
            temp.Append(")").Append(Operator).Append("(");

            var subChar = new SubNode().Op;
            input = input.Replace(this.Op.ToString(), this.ForTranValue(Operator));
            input = input.Replace(")(", temp.ToString());   // )( = )*( rule
            for (var i = 0; i < input.Length - 1; i++)
            {
                // -( = -1*( rule
                if (input[i] == subChar && input[i + 1] == '(')
                {
                    sb.Append(input.Substring(0, i)).Append("-1" + Operator);
                    input = input.Substring(i + 1);
                    i = 0;
                }

                // x( = x*( rule
                else if ((char.IsDigit(input[i]) && input[i + 1] == '(') || (char.IsDigit(input[i]) && char.IsLetter(input[i + 1])))
                {
                    sb.Append(input.Substring(0, i + 1)).Append(Operator);
                    input = input.Substring(i + 1);
                    i = 0;
                }

                // )x = )*x rule
                else if (input[i] == ')' && char.IsLetterOrDigit(input[i + 1]))
                {
                    sb.Append(input.Substring(0, i + 1)).Append(Operator);
                    input = input.Substring(i + 1);
                    i = 0;
                }
            }

            sb.Append(input);
            return sb.ToString();
        }

        /// <summary>
        /// Returns precedence.
        /// </summary>
        /// <returns>The bracket number.</returns>
        internal override int Precedence()
        {
            return 2;
        }
    }
}
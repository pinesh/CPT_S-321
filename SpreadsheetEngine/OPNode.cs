// <copyright file="OPNode.cs" company="Harry Pines - 11578059 - Cpts 321">
// Copyright (c) Harry Pines - 11578059 - Cpts 321. All rights reserved.
// </copyright>

namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using static System.String;

    /// <summary>
    /// This special node acts as the parent for all operations and handles the dictionary of current symbols.
    /// </summary>
    public class OpNode : Node
    {
        /// <summary>
        /// Gets or Sets the operation char.
        /// </summary>
        public virtual char Op { get; set; }

        /// <summary>
        /// Gets or Sets the left node.
        /// </summary>
        internal Node Left { get; set; }

        /// <summary>
        /// Gets or Sets the right node.
        /// </summary>
        internal Node Right { get; set; }

        /// <summary>
        /// Evaluates a node.
        /// </summary>
        /// <param name="node"> The current working node.</param>
        /// <param name="varDict">The variable dictionary.</param>
        /// <returns>The evaluated operated node.</returns>
        public static double Evaluate(Node node, Dictionary<string, double> varDict)
        {
            switch (node)
            {
                // try to evaluate the node as a constant
                // the "as" operator is evaluated to null
                // as opposed to throwing an exception
                case NumNode constantNode:
                    return constantNode.Number();

                // as a variable
                case VarNode variableNode:
                    try
                    {
                        if (variableNode.GetMultiplier() == -1)
                        {
                            return varDict[key: variableNode.GetName()] * -1;
                        }
                    }
                    catch (KeyNotFoundException)
                    {
                        return 0;
                    }

                    return varDict[key: variableNode.GetName()];

                // it is an operator node if we came here
                case OpNode operatorNode:
                    return operatorNode.DoOperator(varDict: varDict);

                default:
                    throw new NotSupportedException(message: "This node was null");
            }
        }

        /// <summary>
        /// Virtual edition that determines operator.
        /// </summary>
        /// <param name="varDict">The input dictionary.</param>
        /// <returns>0.</returns>
        public virtual double DoOperator(Dictionary<string, double> varDict)
        {
            return 0;
        }

        /// <summary>
        /// Virtual normalize function.
        /// </summary>
        /// <param name="input">The input expression.</param>
        /// <returns>The normalized out.</returns>
        public virtual string Normalize(string input)
        {
            return Empty;
        }

        /// <summary>
        /// Getter for FoxTron brackets.
        /// </summary>
        /// <param name="op">The operator symbol.</param>
        /// <returns>The FoxTron behaviour. </returns>
        internal string ForTranValue(char op)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < this.Precedence(); i++)
            {
                sb.Append(")");
            }

            sb.Append(op);

            for (var i = 0; i < this.Precedence(); i++)
            {
                sb.Append("(");
            }

            return sb.ToString();
        }

        /// <summary>
        /// The precedence of an operator.
        /// </summary>
        /// <returns>The integer precedence.</returns>
        internal virtual int Precedence()
        {
            return 0;
        }
    }
}
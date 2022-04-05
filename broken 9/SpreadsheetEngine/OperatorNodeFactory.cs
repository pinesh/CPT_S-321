// <copyright file="OperatorNodeFactory.cs" company="Harry Pines - 11578059 - Cpts 321">
// Copyright (c) Harry Pines - 11578059 - Cpts 321. All rights reserved.
// </copyright>

namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Operator node factory class.
    /// </summary>
    public class OperatorNodeFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorNodeFactory"/> class.
        /// </summary>
        public OperatorNodeFactory()
        {
            TraverseAvailableOperators((op, type) => Operators.Add(op, type));
        }

        private delegate void OnOperator(char op, Type type);

        /// <summary>
        /// Gets Operator dictionary.
        /// </summary>
        public static Dictionary<char, Type> Operators { get; } = new Dictionary<char, Type>();

        /// <summary>
        /// Factory method to initialise a new node, also modifies the input string to ensure correct behaviour.
        /// </summary>
        /// <param name="op">A reference to the working string of who the first char should be an operator, else failure.</param>
        /// <returns>The corresponding operation child node.</returns>
        public static OpNode CreateNewNode(ref string op)
        {
            if (Operators.ContainsKey(op[0]))
            {
                var nodeObject = System.Activator.CreateInstance(Operators[op[0]]);
                op = op.Substring(1);
                if (nodeObject is OpNode node)
                {
                    var type = node.GetType();
                    var method = type.GetMethod("Normalize");
                    var parametersArray = new object[] { op };
                    if (method != null)
                    {
                        op = (string)method.Invoke(nodeObject, parametersArray);
                    }

                    return node;
                }

                throw new Exception("Not an operator");
            }

            return null;
        }

        /// <summary>
        /// Factory method to initialise a new node, also modifies the input string to ensure correct behaviour.
        /// </summary>
        /// <param name="op">A reference to the working string of who the first char should be an operator, else failure.</param>
        /// <returns>The corresponding operation child node.</returns>
        public static OpNode CreateNewNode(char op)
        {
            if (!Operators.ContainsKey(op))
            {
                return null;
            }

            var nodeObject = System.Activator.CreateInstance(Operators[op]);
            if (nodeObject is OpNode node)
            {
                return node;
            }

            throw new Exception("Not an operator");
        }

        /// <summary>
        /// Removes duplicates of + or - in order to pass nonspecific or legal repeating operators.
        /// </summary>
        /// <param name="input">Input expression.</param>
        /// <returns>the modified input expression.</returns>
        public string RemoveInlineDuplicates(string input)
        {
            var sb = new StringBuilder();
            var workingStringBuilder = new StringBuilder();
            var addSym = new AddNode().Op;
            var subSym = new SubNode().Op;
            for (var i = 0; i < input.Length; i++)
            {
                if (input[i] == addSym || input[i] == subSym)
                {
                    if (i != 0)
                    {
                        sb.Append(input.Substring(0, i));
                        input = input.Substring(i);
                    }

                    i = 0;
                    while (input[i] == addSym || input[i] == subSym)
                    {
                        workingStringBuilder.Append(input[i]);
                        i++;
                    }

                    input = input.Substring(i);
                    i = 0;
                    sb.Append(this.ReduceString(workingStringBuilder));
                    workingStringBuilder.Clear();
                }
            }

            sb.Append(input);
            return sb[0] == addSym ? sb.ToString().Substring(1) : sb.ToString();
        }

        /// <summary>
        /// Determines if there is a case of repeating plus or minus signs following a number, determines its ultimate type.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The corresponding end operation.</returns>
        internal char ReduceString(StringBuilder input)
        {
            var addSym = new AddNode().Op;
            var subSym = new SubNode().Op;
            var negCount = 0;
            for (var i = 0; i < input.Length; i++)
            {
                if (input[i] == subSym)
                {
                    negCount++;
                }
            }

            if (negCount == 0)
            {
                return addSym;
            }
            else if (negCount % 2 == 0)
            {
                return addSym;
            }
            else
            {
                return subSym;
            }
        }

        /// <summary>
        /// Professor provided code to iterate through assemblies and construct a dictionary of found operators.
        /// </summary>
        /// <param name="onOperator">An instance of OnOperator.</param>
        private static void TraverseAvailableOperators(OnOperator onOperator)
        {
            // get the type declaration of OperatorNode
            var operatorNodeType = typeof(OpNode); // Iterate over all loaded assemblies:
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                // Get all types that inherit from our OperatorNode class using LINQ
                var operatorTypes = assembly.GetTypes().Where(type => type.IsSubclassOf(operatorNodeType));
                foreach (var type in operatorTypes)
                {
                    // for each subclass, retrieve the Operator property
                    var operatorField = type.GetProperty("Operator");
                    if (operatorField != null)
                    {
                        // Get the character of the Operator //THE PROBLEM IS ON THIS LINE
                        var value = operatorField.GetValue(type);

                        if (value is char operatorSymbol)
                        {
                            // And invoke the function passed as parameter
                            // // with the operator symbol and the operator class
                            onOperator(operatorSymbol, type);
                        }
                    }
                }
            }
        }
    }
}
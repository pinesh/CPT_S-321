// <copyright file="ExpressionTree.cs" company="Harry Pines - 11578059 - Cpts 321">
// Copyright (c) Harry Pines - 11578059 - Cpts 321. All rights reserved.
// </copyright>

namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// This class is the main entry point to evaluating expression functionality.
    /// </summary>
    public class ExpressionTree
    {
        private static readonly OperatorNodeFactory Factory = new OperatorNodeFactory();
        private readonly Node expressionRoot;
        private readonly Dictionary<string, double> varDict;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree" /> class.
        /// </summary>
        /// <param name="expression"> The input expression as a string.</param>
        public ExpressionTree(string expression)
        {
            try
            {
                // prepare the expression by handling bracket checks and whitespace removal
                this.varDict = new Dictionary<string, double>();
                var countLeftP = expression.Count(predicate: x => x == '(');
                var countRightP = expression.Count(predicate: x => x == ')');
                expression = RemoveWhitespace(input: expression);
                try
                {
                    // check if valid parenthesis
                    if (countLeftP.Equals(obj: countRightP))
                    {
                        if (expression != null)
                        {
                            if (OperatorNodeFactory.Operators.TryGetValue(key: expression[index: 0], value: out _))
                            {
                                if (expression[0] == SubNode.Operator || expression[0] == AddNode.Operator)
                                {
                                }
                                else
                                {
                                    throw new ArgumentException();
                                }
                            }

                            expression = Factory.RemoveInlineDuplicates(expression);
                            expression = DoubleParenthesis(expression: expression);
                            expression = Normalize(expression);
                            this.expressionRoot = this.ConstructExpression(expression: expression);
                        }
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }
                catch (ArgumentException)
                {
                    throw new ArgumentException("Error: An Invalid expression has been entered."); // THIS IS USED FOR THE DEMO
                }
            }
            catch
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Returns all variable keys.
        /// </summary>
        /// <returns>The variable keys.</returns>
        public List<string> GetVariableNames()
        {
            return this.varDict.Keys.Where(key => char.IsLetter(key[0]) && char.IsDigit(key[key.Length - 1])).ToList();
        }

        /// <summary>
        /// Sets the specified variable within the ExpressionTree variables dictionary.
        /// </summary>
        /// <param name="variableName">The string name of our variable.</param>
        /// <param name="variableValue">The double value of our variable.</param>
        public void SetVariable(string variableName, double variableValue)
        {
            if (variableName == null)
            {
                throw new ArgumentNullException(paramName: nameof(variableName));
            }

            variableName = RemoveWhitespace(input: variableName);
            if (this.varDict.TryGetValue(key: variableName, value: out _))
            {
                this.varDict[key: variableName] = variableValue;
            }
            else
            {
                this.varDict.Add(key: variableName, value: variableValue);
            }
        }

        /// <summary>
        /// Entry point for outside classes to evaluate their expressions.
        /// </summary>
        /// <returns> The evaluated function.</returns>
        public double Evaluate()
        {
            return OpNode.Evaluate(node: this.expressionRoot, varDict: this.varDict);
        }

        /// <summary>
        /// Determines if the string contains any operators.
        /// </summary>
        /// <param name="testString">The input string.</param>
        /// <returns>True or False depending on the presence of defined operators.</returns>
        public bool ContainsOp(string testString)
        {
            return testString.Any(t => OperatorNodeFactory.Operators.TryGetValue(key: t, value: out _));
        }

        /// <summary>
        /// Professor example code method to remove the parenthesis from a string.
        /// </summary>
        /// <param name="s">The input string.</param>
        /// <returns>the output string without the outermost containing layer of parenthesis.</returns>
        public string RemoveParenthesis(string s)
        {
            // Check for extra parentheses and get rid of them, e.g. (((((2+3)-(4+5)))))
            if (s[index: 0] == '(')
            {
                var parenthesisCounter = 1;
                for (var characterIndex = 1; characterIndex < s.Length; characterIndex++)
                {
                    // if open parenthesis increment a counter
                    if (s[index: characterIndex] == '(')
                    {
                        parenthesisCounter++;
                    }

                    // if closed parenthesis decrement the counter
                    else if (s[index: characterIndex] == ')')
                    {
                        parenthesisCounter--;

                        // if the counter is 0 check where we are
                        if (parenthesisCounter == 0)
                        {
                            if (characterIndex != s.Length - 1)
                            {
                                // if we are not at the end, then get out (there are no extra parentheses)
                                break;
                            }

                            // Else get rid of the outer most parentheses and start over
                            return this.RemoveParenthesis(s: s.Substring(startIndex: 1, length: s.Length - 2));
                        }
                    }
                }
            }

            return s;
        }

        /// <summary>
        /// Creates a new node based on the operator.
        /// </summary>
        /// <param name="expression">The input expression.</param>
        /// <param name="index">The index of the operator.</param>
        /// <returns>The new Node.</returns>
        internal OpNode CreateNode(string expression, int index)
        {
            var newNode = OperatorNodeFactory.CreateNewNode(expression[index: index - 1]);
            newNode.Left = this.ConstructExpression(expression: expression.Substring(startIndex: 0, length: index - 1));
            newNode.Right = this.ConstructExpression(expression: expression.Substring(startIndex: index));
            return newNode;
        }

        /// <summary>
        /// Using the rules em-placed in the operators, this alters the expression to handle precedence and implied case.
        /// </summary>
        /// <param name="expression">The input string.</param>
        /// <returns>The normalized string.</returns>
        private static string Normalize(string expression)
        {
            var sbF = new StringBuilder();
            sbF.Append("(((");
            var sb = new StringBuilder();
            foreach (var keyType in OperatorNodeFactory.Operators)
            {
                sb.Append(keyType.Key).Append(expression);
                expression = sb.ToString();
                OperatorNodeFactory.CreateNewNode(ref expression);
                sb.Clear();
            }

            sbF.Append(expression).Append(")))");
            return sbF.ToString().Replace("()", "0");
        }

        /// <summary>
        /// Removes all whitespace from an input expression.
        /// </summary>
        /// <param name="input">The input expression.</param>
        /// <returns>The input expression without whitespace characters.</returns>
        private static string RemoveWhitespace(string input)
        {
            return new string(value: input.ToCharArray().Where(predicate: c => !char.IsWhiteSpace(c: c)).ToArray());
        }

        /// <summary>
        /// Handles strings that already have parenthesis in order to preserve the order of operations.
        /// </summary>
        /// <param name="expression">The input expression.</param>
        /// <returns>The new modified string.</returns>
        private static string DoubleParenthesis(string expression)
        {
            var newExpression = new StringBuilder();
            foreach (var t in expression)
            {
                switch (t)
                {
                    case '(':
                        newExpression.Append(value: "((((");
                        break;

                    case ')':
                        newExpression.Append(value: "))))");
                        break;

                    default:
                        newExpression.Append(value: t);
                        break;
                }
            }

            return newExpression.ToString();
        }

        /// <summary>
        /// Lots of funky stuff going in to make sure that if an expression could be passed after it is changed to make sure it "could".
        /// </summary>
        /// <param name="expression">The expression passed to the class.</param>
        /// <exception cref="ArgumentException">If this error returns then the expression wasn't parseable.</exception>
        /// <returns>Returns the root node and subsequent nodes recursively. </returns>
        private Node ConstructExpression(string expression)
        {
            var plusSymbol = new AddNode().Op;
            var minusSymbol = new SubNode().Op;
            if (string.IsNullOrEmpty(value: expression))
            {
                return null;
            }

            expression = this.RemoveParenthesis(expression);
            if (string.IsNullOrEmpty(value: expression))
            {
                var newNumNode = new NumNode(0);
                return newNumNode;
            }

            // check if we have a number
            if (double.TryParse(s: expression, result: out var f))
            {
                var newNumNode = new NumNode(f: f);
                return newNumNode;
            }

            // make sure that the first or last element isn't a defined operator.
            if (OperatorNodeFactory.Operators.TryGetValue(key: expression[index: 0], value: out _))
            {
                // Makes an exception for + and - which can be the first character of an entity.
                if (expression[index: 0] == plusSymbol)
                {
                    expression = expression.Substring(startIndex: 1);

                    // If we had a plus at the start, we could have had a number or a variable so we just run through again without it.
                    return this.ConstructExpression(expression: expression);
                }

                if (expression[index: 0] == minusSymbol)
                {
                    var tempAmend = new StringBuilder();
                    tempAmend.Append("(-1)*").Append(expression.Substring(1));
                    expression = tempAmend.ToString();
                }
                else
                {
                    throw new ArgumentException();
                }
            }

            // Make sure the last char isn't an operator (that would be bad).
            if (OperatorNodeFactory.Operators.TryGetValue(key: expression.Last(), value: out _))
            {
                throw new ArgumentException();
            }

            // if not then we check if there any ops in order to decide if any more work needs to be done.

            // If the rest of the expression contains an operator then we don't  have a variable
            if (this.ContainsOp(testString: expression.Substring(startIndex: 1)) == false)
            {
                VarNode newVarNode;

                // If we had ANY symbols at all in what we believe to be a variable then we fail as a variable can only be A-Z/a-z+0-9.
                if (expression.Substring(1).Any(char.IsSymbol) == true)
                {
                    throw new ArgumentException();
                }

                if (expression[index: 0] == minusSymbol)
                {
                    newVarNode = new VarNode(x: expression.Substring(startIndex: 1), y: -1);
                }
                else if (expression[index: 0] == plusSymbol)
                {
                    newVarNode = new VarNode(x: expression, y: 1);
                }
                else
                {
                    // If the first character was a letter then we have a valid variable.
                    if (char.IsLetter(expression[0]))
                    {
                        newVarNode = new VarNode(x: expression, y: 1);
                    }

                    // If we didn't then it means we had an unhandled operator in front and thus fail.
                    else
                    {
                        throw new ArgumentException();
                    }
                }

                // If we haven't seen this variable before then we add it to our dictionary and give it a generic 0 value.
                if (!this.varDict.TryGetValue(key: newVarNode.GetName(), value: out _))
                {
                    this.varDict.Add(key: newVarNode.GetName(), value: 0);
                }

                return newVarNode;
            }

            var index = expression.Length - 1;

            // Derived version of professor provided parenthesis function.
            if (expression[index] == ')')
            {
                var parenthesisCounter = 1;
                for (var characterIndex = index - 1; characterIndex >= 0; characterIndex--)
                {
                    switch (expression[index: characterIndex])
                    {
                        // if open parenthesis increment a counter
                        // if closed parenthesis decrement the counter
                        case ')':
                            parenthesisCounter++;
                            break;

                        case '(':
                            {
                                parenthesisCounter--;

                                // if the counter is 0 check where we are, because we have already removed the outer parenthesis it can only mean we hit the end of a bracket set.
                                if (parenthesisCounter == 0)
                                {
                                    // if the next character isn't an operator then we have an invalid expression
                                    if (OperatorNodeFactory.Operators.TryGetValue(key: expression[index: characterIndex - 1], value: out _))
                                    {
                                        return this.CreateNode(expression, characterIndex);
                                    }
                                }

                                break;
                            }
                    }
                }
            }
            else
            {
                for (var iterate = expression.Length - 1; iterate >= 0; iterate--)
                {
                    if (OperatorNodeFactory.Operators.TryGetValue(key: expression[index: iterate - 1], value: out _))
                    {
                        return this.CreateNode(expression, iterate);
                    }
                }
            }

            return null;
        }
    }
}
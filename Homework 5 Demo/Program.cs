// <copyright file="Program.cs" company="Harry Pines - 11578059 - Cpts 321">
// Copyright (c) Harry Pines - 11578059 - Cpts 321. All rights reserved.
// </copyright>

namespace Homework_5_Demo
{
    using System;
    using CptS321;
    using static System.Console;

    /// <summary>
    /// Test Program.
    /// </summary>
    internal class Program
    {
        private static void Main()
        {
            var exit = false;
            const string expressionStart = "A1+B1+C1";
            var expression = expressionStart;
            var tree = new ExpressionTree(expression: expressionStart);
            while (exit == false)
            {
                WriteLine(value: "Menu (current expression: " + expression + ")");
                WriteLine(value: ">1: Enter a new expression");
                WriteLine(value: ">2: Set a variable");
                WriteLine(value: ">3: Evaluate tree");
                WriteLine(value: ">4: Quit\n");
                var input = ReadLine();
                int.TryParse(s: input, result: out var x);
                switch (x)
                {
                    case 1:
                        WriteLine(value: "Enter new expression: ");
                        expression = ReadLine();
                        try
                        {
                            tree = new ExpressionTree(expression: expression);
                        }
                        catch (ArgumentException)
                        {
                            tree = new ExpressionTree(expression: expressionStart);
                            expression = expressionStart;
                            WriteLine(value: "INVALID EXPRESSION");
                        }

                        break;
                    case 2:
                        WriteLine(value: "Enter a variable name: ");
                        var varName = ReadLine();
                        WriteLine(value: "Enter a variable value: ");
                        var varInput = ReadLine();
                        if (double.TryParse(s: varInput, result: out var test))
                        {
                            tree.SetVariable(variableName: varName, variableValue: test);
                        }
                        else
                        {
                            WriteLine(value: "Not a valid variable value!");
                        }

                        break;
                    case 3:
                        WriteLine(value: tree.Evaluate());
                        break;
                    case 4:
                        exit = true;
                        break;
                }
            }
        }
    }
}

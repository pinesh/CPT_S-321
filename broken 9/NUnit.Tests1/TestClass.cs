// <copyright file="TestClass.cs" company="Harry Pines - 11578059 - Cpts 321">
// Copyright (c) Harry Pines - 11578059 - Cpts 321. All rights reserved.
// </copyright>

namespace NUnit.Tests1
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using CptS321;
    using NUnit.Framework;
    using Spreadsheet_Harry_Pines;
    

    /// <summary>
    /// TEST CLASS.
    /// </summary>
    [TestFixture]
    public class TestClass
    {
        /// <summary>
        /// Test loading an xml with different unused attributes.
        /// </summary>
        [Test]
        public void TestSpreadsheetUnusedAttributeLoad()
        {
            var path = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            path = path.Substring(6);
            Form1 newForm = new Form1();
            newForm.VarSpreadsheet.ReadInXml(path + "\\unusedLoad.xml");
            Assert.That(newForm.DataGrid.Rows[0].Cells[1].Value, Is.EqualTo("6"), "ERROR: INCORRECT READ");
        }
<<<<<<< HEAD

=======
        
>>>>>>> 19b00c5c3a7ca69ce62179f46aa9e03336f13d54
        /// <summary>
        /// Test loading in an xml.
        /// </summary>
        [Test]
        public void TestSpreadsheetLoad()
        {
            var path = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            path = path.Substring(6);
            Form1 newForm = new Form1();
            newForm.VarSpreadsheet.ReadInXml(path + "\\testLoad.xml");
            Assert.That(newForm.DataGrid.Rows[0].Cells[0].Value, Is.EqualTo("6"), "ERROR: INCORRECT READ");
        }

        /// <summary>
        /// Test saving an xml.
        /// </summary>
        [Test]
        public void TestSpreadsheetSave()
        {
            Form1 newForm = new Form1();
            var path = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            path = path.Substring(6);

            newForm.VarSpreadsheet.UpdateCell(0, 0, "10");
            newForm.VarSpreadsheet.UpdateCell(0, 1, "=A1");
            newForm.VarSpreadsheet.UpdateCellsColour(
                new List<(int, int)> { (0, 0) },
                (uint)System.Drawing.Color.AliceBlue.ToArgb());
            newForm.VarSpreadsheet.WriteXml(path + "\\testSave.xml");
            var fileName = path + "\\testSave.xml";
            FileAssert.Exists(@fileName);
        }

        /// <summary>
        /// Test writing saving and then loading a new file.
        /// </summary>
        [Test]
        public void TestSpreadsheetWriteSaveAndLoadNew()
        {
            var path = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            path = path.Substring(6);

            Form1 newForm = new Form1();
            newForm.VarSpreadsheet.UpdateCell(0, 0, "10");
            newForm.VarSpreadsheet.UpdateCell(0, 1, "=A1");
            newForm.VarSpreadsheet.UpdateCellsColour(new List<(int, int)> {(0, 0)},
                (uint)System.Drawing.Color.AliceBlue.ToArgb());
            newForm.VarSpreadsheet.WriteXml(path + "\\testSave.xml");
            newForm.VarSpreadsheet.ReadInXml(path + "\\testLoad.xml");
            Assert.That(newForm.DataGrid.Rows[0].Cells[0].Value, Is.EqualTo("6"), "ERROR: INCORRECT READ");

        }

        /// <summary>
        /// Test writing saving and then loading a new file and then loading the original.
        /// </summary>
        [Test]
        public void TestSpreadsheetWriteSaveAndLoadNewAndLoadOld()
        {
            var path = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            path = path.Substring(6);

            Form1 newForm = new Form1();
            newForm.VarSpreadsheet.UpdateCell(0, 0, "10");
            newForm.VarSpreadsheet.UpdateCell(0, 1, "=A1");
            newForm.VarSpreadsheet.UpdateCellsColour(new List<(int, int)> { (0, 0) },
                (uint)System.Drawing.Color.AliceBlue.ToArgb());
            newForm.VarSpreadsheet.WriteXml(path + "\\testSave.xml");
            newForm.VarSpreadsheet.ReadInXml(path + "\\testLoad.xml");
            newForm.VarSpreadsheet.ReadInXml(path + "\\testSave.xml");
            Assert.That(newForm.DataGrid.Rows[0].Cells[0].Value, Is.EqualTo("10"), "ERROR: INCORRECT READ");

        }

        /// <summary>
        /// Test undo after write.
        /// </summary>
        [Test]
        public void TestSpreadsheetWrite1Undo1()
        {
            Form1 newForm = new Form1();
            newForm.VarSpreadsheet.UpdateCell(0, 0, "Test String");
            newForm.VarSpreadsheet.UpdateCell(0, 0, "New string");
            newForm.VarSpreadsheet.UndoRedo.Undo();
            Assert.That(newForm.DataGrid.Rows[0].Cells[0].Value, Is.EqualTo("Test String"), "ERROR: INCORRECT READ");
        }

        /// <summary>
        /// Test undo after write and then redo.
        /// </summary>
        [Test]
        public void TestSpreadsheetWrite1Undo1Redo1()
        {
            Form1 newForm = new Form1();
            newForm.VarSpreadsheet.UpdateCell(0, 0, "Test String");
            newForm.VarSpreadsheet.UpdateCell(0, 0, "New string");
            newForm.VarSpreadsheet.UndoRedo.Undo();
            newForm.VarSpreadsheet.UndoRedo.Redo();
            Assert.That(newForm.DataGrid.Rows[0].Cells[0].Value, Is.EqualTo("New string"), "ERROR: INCORRECT READ");
        }

        /// <summary>
        /// Test undo after write and then redo.
        /// </summary>
        [Test]
        public void TestSpreadsheetWrite1Undo1Redo1Reference()
        {
            Form1 newForm = new Form1();
            newForm.VarSpreadsheet.UpdateCell(0, 0, "=10");
            newForm.VarSpreadsheet.UpdateCell(0, 1, "=A1");
            newForm.VarSpreadsheet.UpdateCell(0, 0, "=11");
            newForm.VarSpreadsheet.UndoRedo.Undo();
            Assert.That(newForm.DataGrid.Rows[0].Cells[1].Value, Is.EqualTo("10"), "ERROR: INCORRECT READ");
        }

        /// <summary>
        /// Test single cell colour change. NOTE TO GRADER, THIS TEST SHOULD WORK IF YOU USE THE MOST RECENT VERSION OF .NET FRAMEWORK. IF USING LESS THAN 4.7 DO NOT RUN.
        /// </summary>
        [Test]
        public void TestSpreadsheetCellColourChange1()
        {
            Form1 newForm = new Form1();
            var tuple = new List<(int, int)> {(0, 0)};
            newForm.VarSpreadsheet.UpdateCellsColour(tuple, (uint)System.Drawing.Color.AliceBlue.ToArgb());
            Assert.That(newForm.DataGrid.Rows[0].Cells[0].Style.BackColor.ToArgb(),
                Is.EqualTo(System.Drawing.Color.AliceBlue.ToArgb()), "ERROR: INCORRECT colour");
        }


        /// <summary>
        /// Test single cell colour change with an undo. NOTE TO GRADER, THIS TEST SHOULD WORK IF YOU USE THE MOST RECENT VERSION OF .NET FRAMEWORK. IF USING LESS THAN 4.7 DO NOT RUN
        /// </summary>
        [Test]
        public void TestSpreadsheetCellColourChange2()
        {
            Form1 newForm = new Form1();
            var tuple = new List<(int, int)> {(0, 0)};
            newForm.VarSpreadsheet.UpdateCellsColour(tuple, (uint)System.Drawing.Color.AliceBlue.ToArgb());
            newForm.VarSpreadsheet.UndoRedo.Undo();
            Assert.That(newForm.DataGrid.Rows[0].Cells[0].Style.BackColor.ToArgb(),
                Is.EqualTo(System.Drawing.Color.White.ToArgb()), "ERROR: INCORRECT colour");
        }

        // Test cell usage.

        /// <summary>
        /// Test write.
        /// </summary>
        [Test]
        public void TestSpreadsheetWrite()
        {
            Form1 newForm = new Form1();
            newForm.VarSpreadsheet.UpdateCell(0, 0, "Test String");
            Assert.That(newForm.DataGrid.Rows[0].Cells[0].Value, Is.EqualTo("Test String"), "ERROR: INCORRECT READ");
        }

        /// <summary>
        /// Test write.
        /// </summary>
        [Test]
        public void TestSpreadsheetEqualsWriteString()
        {
            Form1 newForm = new Form1();
            newForm.VarSpreadsheet.UpdateCell(0, 0, "=\"Test String\"");
            Assert.That(newForm.DataGrid.Rows[0].Cells[0].Value, Is.EqualTo("Test String"), "ERROR: INCORRECT READ");
        }

        /// <summary>
        /// Test empty read.
        /// </summary>
        [Test]
        public void TestSpreadsheetReadEmpty()
        {
            Form1 newForm = new Form1();

            Assert.That(newForm.DataGrid.Rows[0].Cells[1].Value, Is.EqualTo(null), "ERROR: Expected Null String");
        }

        /// <summary>
        /// Test formula write.
        /// </summary>
        [Test]
        public void TestSpreadsheetFormulaWrite()
        {
            Form1 newForm = new Form1();
            newForm.VarSpreadsheet.UpdateCell(0, 0, "10");
            newForm.VarSpreadsheet.UpdateCell(0, 1, "=A1");
            Assert.That(newForm.DataGrid.Rows[0].Cells[1].Value, Is.EqualTo("10"), "ERROR: Expected 10");
        }

        /// <summary>
        /// Test formula write on empty.
        /// </summary>
        [Test]
        public void TestSpreadsheetFormulaWriteEmptyError()
        {
            Form1 newForm = new Form1();
            newForm.VarSpreadsheet.UpdateCell(0, 1, "=A1");
            Assert.That(newForm.DataGrid.Rows[0].Cells[1].Value, Is.EqualTo("0"), "ERROR: Expected 0 String");
        }

        /// <summary>
        /// Test formula write and reference.
        /// </summary>
        [Test]
        public void TestSpreadsheetFormulaWriteFormula()
        {
            Form1 newForm = new Form1();
            newForm.VarSpreadsheet.UpdateCell(0, 0, "=10*3");
            newForm.VarSpreadsheet.UpdateCell(0, 1, "=A1");
            Assert.That(newForm.DataGrid.Rows[0].Cells[1].Value, Is.EqualTo("30"), "ERROR: Expected 30");
        }

        /// <summary>
        /// Test formula write and reference and write and update.
        /// </summary>
        [Test]
        public void TestSpreadsheetFormulaWriteFormulaChangeFormula()
        {
            Form1 newForm = new Form1();
            newForm.VarSpreadsheet.UpdateCell(0, 0, "=10*3");
            newForm.VarSpreadsheet.UpdateCell(0, 1, "=A1");
            newForm.VarSpreadsheet.UpdateCell(0, 0, "=10*4");
            Assert.That(newForm.DataGrid.Rows[0].Cells[1].Value, Is.EqualTo("40"), "ERROR: Expected 40");
        }

        /// <summary>
        /// Test formula bad write and reference and write and update.
        /// </summary>
        [Test]
        public void TestSpreadsheetFormulaWriteFailFormulaChangeFormula()
        {
            Form1 newForm = new Form1();
            newForm.VarSpreadsheet.UpdateCell(0, 1, "=A1");
            newForm.VarSpreadsheet.UpdateCell(0, 0, "=10*4");
            Assert.That(newForm.DataGrid.Rows[0].Cells[1].Value, Is.EqualTo("40"), "ERROR: Expected 40");
        }

        /// <summary>
        /// Test formula circle reference.
        /// </summary>
        [Test]
        public void TestSpreadsheetFormulaWriteCircleFormula()
        {
            Form1 newForm = new Form1();
            newForm.VarSpreadsheet.UpdateCell(0, 1, "=B1");
            Assert.That(newForm.DataGrid.Rows[0].Cells[1].Value, Is.EqualTo("Error - Circular Ref"), "ERROR: Expected Error - Circular Ref");
        }

        /// <summary>
        /// Test formula invalid reference.
        /// </summary>
        [Test]
        public void TestSpreadsheetFormulaWriteInvalidFormula()
        {
            Form1 newForm = new Form1();
            newForm.VarSpreadsheet.UpdateCell(0, 1, "=(1*3");
            Assert.That(newForm.DataGrid.Rows[0].Cells[1].Value, Is.EqualTo("Error - Invalid Expression"), "ERROR: Expected Error - Invalid Expression");
        }

        /// <summary>
        /// Test formula invalid reference which is then corrected.
        /// </summary>
        [Test]
        public void TestSpreadsheetFormulaWriteInvalidFormulaCorrection()
        {
            Form1 newForm = new Form1();
            newForm.VarSpreadsheet.UpdateCell(0, 1, "=(1*3");
            newForm.VarSpreadsheet.UpdateCell(0, 1, "=(1*3)");
            Assert.That(newForm.DataGrid.Rows[0].Cells[1].Value, Is.EqualTo("3"), "ERROR: Expected 3");
        }

        // ------------------------------------------------------------------------------Expression tree tests

        /// <summary>
        /// Test assumption of mixed cap string variable without assigning value.
        /// </summary>
        [Test]
        public void TestSpreadsheetExpressionEmptyVar()
        {
            ExpressionTree newTree = new ExpressionTree("Testexpression");
            Assert.That(newTree.Evaluate(), Is.EqualTo(0), "Error Variable not 0");
        }

        /// <summary>
        /// Test assumption of mixed cap string as variable with assigned value.
        /// </summary>
        [Test]
        public void TestExpressionVar()
        {
            ExpressionTree newTree = new ExpressionTree("Testexpression");
            newTree.SetVariable("Testexpression", 1);
            Assert.That(newTree.Evaluate(), Is.EqualTo(1), "Error Variable not q");
        }

        /// <summary>
        /// Test handling of double negatives.
        /// </summary>
        [Test]
        public void TestExpressionDoubleNegResolve()
        {
            ExpressionTree newTree = new ExpressionTree("--10");
            Assert.That(newTree.Evaluate(), Is.EqualTo(10), "Error result not 10");
        }

        /// <summary>
        /// Test handling of double+ negatives addition.
        /// </summary>
        [Test]
        public void TestExpressionDoubleNegAdd()
        {
            ExpressionTree newTree = new ExpressionTree("9--10");
            Assert.That(newTree.Evaluate(), Is.EqualTo(19), "Error result not 19");
        }

        /// <summary>
        /// test order of operations with numbers.
        /// </summary>
        [Test]
        public void TestExpressionOrderOfOp()
        {
            ExpressionTree newTree = new ExpressionTree("9+1*2");
            Assert.That(newTree.Evaluate(), Is.EqualTo(11), "Error result not 11");
        }

        /// <summary>
        /// Test order of operations with numbers and variables.
        /// </summary>
        [Test]
        public void TestExpressionOrderOfOpWithNumAndVar()
        {
            ExpressionTree newTree = new ExpressionTree("9+1*A1");
            newTree.SetVariable("A1", 2);
            Assert.That(newTree.Evaluate(), Is.EqualTo(11), "Error result not 11");
        }

        /// <summary>
        /// Test propper order of operations with variables.
        /// </summary>
        [Test]
        public void TestExpressionOrderOfOpWithVar()
        {
            ExpressionTree newTree = new ExpressionTree("A1+A2*A3");
            newTree.SetVariable("A1", 9);
            newTree.SetVariable("A2", 1);
            newTree.SetVariable("A3", 2);
            Assert.That(newTree.Evaluate(), Is.EqualTo(11), "Error result not 11");
        }

        /// <summary>
        /// Test a double negative as a multiplier.
        /// </summary>
        [Test]
        public void TestExpressionDoubleNegBrackets()
        {
            ExpressionTree newTree = new ExpressionTree("-(-10)");
            Assert.That(newTree.Evaluate(), Is.EqualTo(10), "Error result not 10");
        }

        /// <summary>
        /// Test the demo function from homework 6.
        /// </summary>
        [Test]
        public void TestExpressionHomework5Demo()
        {
            ExpressionTree newTree = new ExpressionTree("A1+A2+A3");
            Assert.That(newTree.Evaluate(), Is.EqualTo(0), "Error result not 0");
        }

        /// <summary>
        /// test negative square rules without brackets.
        /// </summary>
        [Test]
        public void TestExpressionNegativeSquare()
        {
            ExpressionTree newTree = new ExpressionTree("-2^2");
            Assert.That(newTree.Evaluate(), Is.EqualTo(4), "Error result not 4");
        }

        /// <summary>
        /// Test number to bracket multiplication.
        /// </summary>
        [Test]
        public void TestExpressionImpliedMultiplicationNum()
        {
            ExpressionTree newTree = new ExpressionTree("2(2(2))");
            Assert.That(newTree.Evaluate(), Is.EqualTo(8), "Error result not 8");
        }

        /// <summary>
        /// test bracket to number multiplication.
        /// </summary>
        [Test]
        public void TestExpressionImpliedMultiplicationReverse()
        {
            ExpressionTree newTree = new ExpressionTree("(2)2");
            Assert.That(newTree.Evaluate(), Is.EqualTo(4), "Error result not 4");
        }

        /// <summary>
        /// Test bracket to bracket multiplication.
        /// </summary>
        [Test]
        public void TestExpressionImpliedMultiplicationNum2()
        {
            ExpressionTree newTree = new ExpressionTree("(2)(2)");
            Assert.That(newTree.Evaluate(), Is.EqualTo(4), "Error result not 4");
        }

        /// <summary>
        /// Test implied number variable multiplication.
        /// </summary>
        [Test]
        public void TestExpressionImpliedMultiplicationVar()
        {
            ExpressionTree newTree = new ExpressionTree("2A1");
            newTree.SetVariable("A1", 9);
            Assert.That(newTree.Evaluate(), Is.EqualTo(18), "Error result not 18");
        }

        /// <summary>
        /// Test division is working.
        /// </summary>
        [Test]
        public void TestExpressionDivision()
        {
            ExpressionTree newTree = new ExpressionTree("2/2");
            Assert.That(newTree.Evaluate(), Is.EqualTo(1), "Error result not 1");
        }

        /// <summary>
        /// Test an empty expression.
        /// </summary>
        [Test]
        public void TestExpressionEmpty()
        {
            var ex = Assert.Throws<System.ArgumentException>(() => new ExpressionTree(string.Empty));
            Assert.That(ex.Message, Is.EqualTo("Value does not fall within the expected range."));
        }

        /// <summary>
        ///  Test an expression with an invalid operator behind.
        /// </summary>
        [Test]
        public void TestExpressionInvalidOperatorRear()
        {
            var ex = Assert.Throws<System.ArgumentException>(() => new ExpressionTree("1+"));
            Assert.That(ex.Message, Is.EqualTo("Value does not fall within the expected range."));
        }

        /// <summary>
        /// Test an expression with an invalid operator in front.
        /// </summary>
        [Test]
        public void TestExpressionInvalidOperatorFront()
        {
            var ex = Assert.Throws<System.ArgumentException>(() => new ExpressionTree("*1"));
            Assert.That(ex.Message, Is.EqualTo("Value does not fall within the expected range."));
        }
    }
}

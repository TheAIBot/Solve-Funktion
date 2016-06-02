using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solve_Funktion;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class EquationTests
    {
        [TestMethod]
        public void TestEquation1()
        {
            EvolutionInfo EInfo = TestTools.GetEvolutionInfo("x = {3, 4, 5, 6}", "4, 6, 8, 10");
            Equation e1 = TestTools.MakeEquation(EInfo);

            // (x) = (x) * x
            Operator o1 = e1.AddOperator(false, new Parentheses(), 0, 2, false, new Multiply(), e1);

            // - 1 = (x - 1) * x
            Operator o2 = o1.AddOperator(false, new Subtract(), 0, 1, true, null, o1);



            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((x - 1) * x)", new double[] { 6, 12, 20, 30 });
            CheckEquationCopyAfterResult(e1, EInfo);

            o1.ResultOnRightSide = true;
            o1.OperatorChanged();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = (x * (x - 1))", new double[] { 6, 12, 20, 30 });
            CheckEquationCopyAfterResult(e1, EInfo);

            o2.ResultOnRightSide = true;
            o2.OperatorChanged();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = (x * (1 - x))", new double[] { -6, -12, -20, -30 });
            CheckEquationCopyAfterResult(e1, EInfo);

            o1.ResultOnRightSide = false;
            o2.ResultOnRightSide = true;
            o1.OperatorChanged();
            o2.OperatorChanged();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((1 - x) * x)", new double[] { -6, -12, -20, -30 });
            CheckEquationCopyAfterResult(e1, EInfo);



            o2.RandomNumber = -5;
            o2.OperatorChanged();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((-5 - x) * x)", new double[] { -24, -36, -50, -66 });
            CheckEquationCopyAfterResult(e1, EInfo);

            o2.RandomNumber = 3;
            o2.OperatorChanged();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((3 - x) * x)", new double[] { 0, -4, -10, -18 });
            CheckEquationCopyAfterResult(e1, EInfo);



            o2.MFunction = new Plus();
            o2.OperatorChanged();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((3 + x) * x)", new double[] { 18, 28, 40, 54 });
            CheckEquationCopyAfterResult(e1, EInfo);

            o2.MFunction = new Multiply();
            o2.OperatorChanged();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((3 * x) * x)", new double[] { 27, 48, 75, 108 });
            CheckEquationCopyAfterResult(e1, EInfo);

            o2.MFunction = new Divide();
            o2.OperatorChanged();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((3 / x) * x)", new double[] { 3, 3, 3, 3 });
            CheckEquationCopyAfterResult(e1, EInfo);



            o2.MFunction = new Subtract();
            o1.ExtraMathFunction = new Plus();
            o1.OperatorChanged();
            o2.OperatorChanged();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((3 - x) + x)", new double[] { 3, 3, 3, 3 });
            CheckEquationCopyAfterResult(e1, EInfo);

            o1.ExtraMathFunction = new Subtract();
            o1.OperatorChanged();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((3 - x) - x)", new double[] { -3, -5, -7, -9 });
            CheckEquationCopyAfterResult(e1, EInfo);

            o1.ExtraMathFunction = new Divide();
            o1.OperatorChanged();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((3 - x) / x)", new double[] { 0, -0.25, -0.4, -0.5 });
            CheckEquationCopyAfterResult(e1, EInfo);



            o1.ResultOnRightSide = true;
            o1.ExtraMathFunction = new Plus();
            o1.OperatorChanged();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = (x + (3 - x))", new double[] { 3, 3, 3, 3 });
            CheckEquationCopyAfterResult(e1, EInfo);

            o1.ExtraMathFunction = new Subtract();
            o1.OperatorChanged();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = (x - (3 - x))", new double[] { 3, 5, 7, 9 });
            CheckEquationCopyAfterResult(e1, EInfo);

            o1.ExtraMathFunction = new Divide();
            o1.OperatorChanged();
            CheckShowResult(e1.CreateFunction(), "f(x) = (x / (3 - x))");
            e1.CalcTotalOffSet();
            Assert.IsFalse(Tools.IsANumber(e1.OffSet), "expected NaN as offset, but got: " + e1.OffSet);




            // - 1 = (x / (3 - x)) - 1
            Operator o3 = e1.AddOperator(false, new Subtract(), 0, 1, true, null, e1);

            // - 1 = 1 - (x / (3 - x)) - 1)
            Operator o4 = e1.AddOperator(true, new Subtract(), 0, 1, true, null, e1);

            // x = (1 - ((x / (3 - x)) - 1)) * x
            Operator o5 = e1.AddOperator(false, new Parentheses(), 0, 2, false, new Multiply(), e1);

            // x = x * (x * ((1 - ((x / (3 - x)) - 1)) * x))
            Operator o6 = e1.AddOperator(true, new Parentheses(), 0, 2, false, new Multiply(), e1);




            o1.OperatorChanged();
            CheckShowResult(e1.CreateFunction(), "f(x) = ((x * (1 - ((x / (3 - x)) - 1))) * x)");
            e1.CalcTotalOffSet();
            Assert.IsFalse(Tools.IsANumber(e1.OffSet), "expected NaN as offset, but got: " + e1.OffSet);

            o1.ExtraMathFunction = new Subtract();
            o1.OperatorChanged();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((x * (1 - ((x - (3 - x)) - 1))) * x)", new double[] { -9, -48, -125, -252 });
            CheckEquationCopyAfterResult(e1, EInfo);




            // * x = (x * x) * (x * ((1 - ((x / (3 - x)) - 1)) * x))
            Operator o7 = o6.AddOperator(false, new Multiply(), 0, 1, false, null, o6);




            o6.OperatorChanged();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((x * (1 - ((x - (3 - x)) - 1))) * (x * x))", new double[] { -27, -192, -625, -1512 });
            CheckEquationCopyAfterResult(e1, EInfo);
        }

        [TestMethod]
        public void TestEquation2()
        {
            Equation e1 = TestTools.MakeEquation("x = {3, 4, 5, 6}", "4, 6, 8, 10");


            // (x) = (x) * x
            Operator o1 = e1.AddOperator(false, new Parentheses(), 0, 2, false, new Multiply(), e1);

            // - 1 = (x - 1) * x
            Operator o2 = o1.AddOperator(false, new Subtract(), 0, 1, true, null, o1);

            // - 1 = (x / (3 - x)) - 1
            Operator o3 = e1.AddOperator(false, new Subtract(), 0, 1, true, null, e1);

            // - 1 = 1 - (x / (3 - x)) - 1)
            Operator o4 = e1.AddOperator(true, new Subtract(), 0, 1, true, null, e1);

            // x = (1 - ((x / (3 - x)) - 1)) * x
            Operator o5 = e1.AddOperator(false, new Parentheses(), 0, 2, false, new Multiply(), e1);

            // x = x * (x * ((1 - ((x / (3 - x)) - 1)) * x))
            Operator o6 = e1.AddOperator(true, new Parentheses(), 0, 2, false, new Multiply(), e1);

            // * x = (x * x) * (x * ((1 - ((x / (3 - x)) - 1)) * x))
            Operator o7 = o6.AddOperator(false, new Multiply(), 0, 1, false, null, o6);


            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((x * (1 - (((x - 1) * x) - 1))) * (x * x))", new double[] { -108, -640, -2250, -6048 });
        }

        private void CheckEquationShowResult(Equation e, string equation, string expected, double[] expectedResults)
        {
            CheckShowResult(equation, expected);
            int index = 0;
            e.CalcTotalOffSet();
            Assert.IsTrue(e.GetFunctionResults().All(x => Double.Parse(x) == expectedResults[index++]),
                          "Tested " + equation + " and got " + String.Join(", ", e.GetFunctionResults()) + " != " + String.Join(", ", expectedResults.Select(x => x.ToString("N2"))));
            e.CalcTotalOffSet();
            index = 0;
            Assert.IsTrue(e.GetFunctionResults().All(x => Double.Parse(x) == expectedResults[index++]),
                          "Tested2 " + equation + " and got " + String.Join(", ", e.GetFunctionResults()) + " != " + String.Join(", ", expectedResults.Select(x => x.ToString("N2"))));
        }

        private void CheckEquationCopyAfterResult(Equation Original, EvolutionInfo EInfo)
        {
            Equation Copy = TestTools.MakeEquation(EInfo);
            Original.MakeClone(Copy);
            CheckEquationShowResult(Copy, Copy.CreateFunction(), Original.CreateFunction(), Original.Results);
        }

        private void CheckShowResult(string equation, string expected)
        {
            Assert.IsTrue(equation == expected, equation + " != " + expected);
        }
    }
}

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
        public void TestEquation()
        {
            Equation e1 = TestTools.MakeEquation("x = {3, 4, 5, 6}", "4, 6, 8, 10");

            // x = x * x
            Operator o1 = e1.OPStorage.Pop();
            o1.ContainedList = e1.EquationParts;
            e1.AllOperators.Add(o1);
            e1.EquationParts.Add(o1);
            o1.ResultOnRightSide = false;
            o1.MFunction = new Parentheses();
            o1.UseRandomNumber = false;
            o1.randomNumber = TestTools.CreateVector(2, 2);
            o1.parameterIndex = 0;
            o1.ExtraMathFunction = new Multiply();

            // - 1 = (x - 1) * 2
            Operator o2 = e1.OPStorage.Pop();
            o2.ContainedList = o1.Operators;
            e1.AllOperators.Add(o2);
            o1.Operators.Add(o2);
            o2.ResultOnRightSide = false;
            o2.MFunction = new Subtract();
            o2.UseRandomNumber = true;
            o2.randomNumber = TestTools.CreateVector(1, 1);
            o2.parameterIndex = 0;

            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((x - 1) * x)", new double[] { 6, 12, 20, 30 });

            o1.ResultOnRightSide = true;
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = (x * (x - 1))", new double[] { 6, 12, 20, 30 });

            o2.ResultOnRightSide = true;
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = (x * (1 - x))", new double[] { -6, -12, -20, -30 });

            o1.ResultOnRightSide = false;
            o2.ResultOnRightSide = true;
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((1 - x) * x)", new double[] { -6, -12, -20, -30 });
        }

        private void CheckEquationShowResult(Equation e, string equation, string expected, double[] expectedResults)
        {
            Assert.IsTrue(equation == expected, equation + " != " + expected);
            int index = 0;
            e.CalcTotalOffSet();
            Assert.IsTrue(e.GetFunctionResults().All(x => Double.Parse(x) == expectedResults[index++]),
                          "Tested " + equation + " and got " + String.Join(", ", e.GetFunctionResults()) + " != " + String.Join(", ", expectedResults.Select(x => x.ToString("N2"))));
        }
    }
}

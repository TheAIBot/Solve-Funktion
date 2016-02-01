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
            Equation e1 = TestTools.MakeEquation("x = {3, 4, 5, 6}", "4, 6, 8, 10");

            // (x) = (x) * x
            Operator o1 = e1.OPStorage.Pop();
            o1.ContainedList = e1.EquationParts;
            e1.AllOperators.Add(o1);
            e1.EquationParts.Add(o1);
            o1.ResultOnRightSide = false;
            o1.MFunction = new Parentheses();
            o1.UseRandomNumber = false;
            o1.randomNumber = TestTools.CreateVectorRepeat(2);
            o1.parameterIndex = 0;
            o1.ExtraMathFunction = new Multiply();

            // - 1 = (x - 1) * x
            Operator o2 = e1.OPStorage.Pop();
            o2.ContainedList = o1.Operators;
            e1.AllOperators.Add(o2);
            o1.Operators.Add(o2);
            o2.ResultOnRightSide = false;
            o2.MFunction = new Subtract();
            o2.UseRandomNumber = true;
            o2.randomNumber = TestTools.CreateVectorRepeat(1);
            o2.parameterIndex = 0;

            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((x - 1) * x)", new double[] { 6, 12, 20, 30 });

            o1.ResultOnRightSide = true;
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = (x * (x - 1))", new double[] { 6, 12, 20, 30 });

            o2.ResultOnRightSide = true;
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = (x * (1 - x))", new double[] { -6, -12, -20, -30 });

            o1.ResultOnRightSide = false;
            o2.ResultOnRightSide = true;
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((1 - x) * x)", new double[] { -6, -12, -20, -30 });



            o2.randomNumber = TestTools.CreateVectorRepeat(-5);
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((-5 - x) * x)", new double[] { -24, -36, -50, -66 });
            
            o2.randomNumber = TestTools.CreateVectorRepeat(3);
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((3 - x) * x)", new double[] { 0, -4, -10, -18 });



            o2.MFunction = new Plus();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((3 + x) * x)", new double[] { 18, 28, 40, 54 });

            o2.MFunction = new Multiply();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((3 * x) * x)", new double[] { 27, 48, 75, 108 });

            o2.MFunction = new Divide();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((3 / x) * x)", new double[] { 3, 3, 3, 3 });



            o2.MFunction = new Subtract();
            o1.ExtraMathFunction = new Plus();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((3 - x) + x)", new double[] { 3, 3, 3, 3 });

            o1.ExtraMathFunction = new Subtract();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((3 - x) - x)", new double[] { -3, -5, -7, -9 });

            o1.ExtraMathFunction = new Divide();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((3 - x) / x)", new double[] { 0, -0.25, -0.4, -0.5 });



            o1.ResultOnRightSide = true;
            o1.ExtraMathFunction = new Plus();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = (x + (3 - x))", new double[] { 3, 3, 3, 3 });

            o1.ExtraMathFunction = new Subtract();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = (x - (3 - x))", new double[] { 3, 5, 7, 9 });

            o1.ExtraMathFunction = new Divide();
            CheckShowResult(e1.CreateFunction(), "f(x) = (x / (3 - x))");
            e1.CalcTotalOffSet();
            Assert.IsFalse(Tools.IsANumber(e1.OffSet), "expected NaN as offset, but got: " + e1.OffSet);




            // - 1 = (x / (3 - x)) - 1
            Operator o3 = e1.OPStorage.Pop();
            o3.ContainedList = e1.EquationParts;
            e1.AllOperators.Add(o3);
            e1.EquationParts.Add(o3);
            o3.ResultOnRightSide = false;
            o3.MFunction = new Subtract();
            o3.UseRandomNumber = true;
            o3.randomNumber = TestTools.CreateVectorRepeat(1);
            o3.parameterIndex = 0;

            // - 1 = 1 - (x / (3 - x)) - 1)
            Operator o4 = e1.OPStorage.Pop();
            o4.ContainedList = e1.EquationParts;
            e1.AllOperators.Add(o4);
            e1.EquationParts.Add(o4);
            o4.ResultOnRightSide = true;
            o4.MFunction = new Subtract();
            o4.UseRandomNumber = true;
            o4.randomNumber = TestTools.CreateVectorRepeat(1);
            o4.parameterIndex = 0;

            // x = (1 - ((x / (3 - x)) - 1)) * x
            Operator o5 = e1.OPStorage.Pop();
            o5.ContainedList = e1.EquationParts;
            e1.AllOperators.Add(o5);
            e1.EquationParts.Add(o5);
            o5.ResultOnRightSide = false;
            o5.MFunction = new Parentheses();
            o5.UseRandomNumber = false;
            o5.randomNumber = TestTools.CreateVectorRepeat(2);
            o5.parameterIndex = 0;
            o5.ExtraMathFunction = new Multiply();

            // x = x * (x * ((1 - ((x / (3 - x)) - 1)) * x))
            Operator o6 = e1.OPStorage.Pop();
            o6.ContainedList = e1.EquationParts;
            e1.AllOperators.Add(o6);
            e1.EquationParts.Add(o6);
            o6.ResultOnRightSide = true;
            o6.MFunction = new Parentheses();
            o6.UseRandomNumber = false;
            o6.randomNumber = TestTools.CreateVectorRepeat(2);
            o6.parameterIndex = 0;
            o6.ExtraMathFunction = new Multiply();



            CheckShowResult(e1.CreateFunction(), "f(x) = ((x * (1 - ((x / (3 - x)) - 1))) * x)");
            e1.CalcTotalOffSet();
            Assert.IsFalse(Tools.IsANumber(e1.OffSet), "expected NaN as offset, but got: " + e1.OffSet);

            o1.ExtraMathFunction = new Subtract();
            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((x * (1 - ((x - (3 - x)) - 1))) * x)", new double[] { -9, -48, -125, -252 });




            // * x = (x * x) * (x * ((1 - ((x / (3 - x)) - 1)) * x))
            Operator o7 = e1.OPStorage.Pop();
            o7.ContainedList = o6.Operators;
            e1.AllOperators.Add(o7);
            o6.Operators.Add(o7);
            o7.ResultOnRightSide = false;
            o7.MFunction = new Multiply();
            o7.UseRandomNumber = false;
            o7.randomNumber = TestTools.CreateVectorRepeat(1);
            o7.parameterIndex = 0;



            CheckEquationShowResult(e1, e1.CreateFunction(), "f(x) = ((x * (1 - ((x - (3 - x)) - 1))) * (x * x))", new double[] { -27, -192, -625, -1512 });
        }

        [TestMethod]
        public void TestEquation2()
        {

        }

        private void CheckEquationShowResult(Equation e, string equation, string expected, double[] expectedResults)
        {
            CheckShowResult(equation, expected);
            int index = 0;
            e.CalcTotalOffSet();
            Assert.IsTrue(e.GetFunctionResults().All(x => Double.Parse(x) == expectedResults[index++]),
                          "Tested " + equation + " and got " + String.Join(", ", e.GetFunctionResults()) + " != " + String.Join(", ", expectedResults.Select(x => x.ToString("N2"))));
        }

        private void CheckShowResult(string equation, string expected)
        {
            Assert.IsTrue(equation == expected, equation + " != " + expected);
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using EquationCreator;

namespace Tests
{
    [TestClass]
    public class MathLogicTests
    {
        [TestMethod]
        public void NOTTest()
        {
            TestLogicOperator(new float[] { 1, 0 }, new float[] { 0, 0 }, new float[] { 0, 1 }, (a, b) => MathLogic.NOT(a), "not");
        }

        [TestMethod]
        public void ANDTest()
        {
            TestLogicOperator(new float[] { 1, 1, 0, 0 }, new float[] { 1, 0, 1, 0 }, new float[] { 1, 0, 0, 0 }, (a, b) => MathLogic.AND(a, b), "and");
        }

        [TestMethod]
        public void NANDTest()
        {
            TestLogicOperator(new float[] { 1, 1, 0, 0 }, new float[] { 1, 0, 1, 0 }, new float[] { 0, 1, 1, 1 }, (a, b) => MathLogic.NAND(a, b), "nand");
        }

        [TestMethod]
        public void ORTest()
        {
            TestLogicOperator(new float[] { 1, 1, 0, 0 }, new float[] { 1, 0, 1, 0 }, new float[] { 1, 1, 1, 0 }, (a, b) => MathLogic.OR(a, b), "or");
        }

        [TestMethod]
        public void NORTest()
        {
            TestLogicOperator(new float[] { 1, 1, 0, 0 }, new float[] { 1, 0, 1, 0 }, new float[] { 0, 0, 0, 1 }, (a, b) => MathLogic.NOR(a, b), "nor");
        }

        [TestMethod]
        public void XORTest()
        {
            TestLogicOperator(new float[] { 1, 1, 0, 0 }, new float[] { 1, 0, 1, 0 }, new float[] { 0, 1, 1, 0 }, (a, b) => MathLogic.XOR(a, b), "xor");
        }

        [TestMethod]
        public void XNORTest()
        {
            TestLogicOperator(new float[] { 1, 1, 0, 0 }, new float[] { 1, 0, 1, 0 }, new float[] { 1, 0, 0, 1 }, (a, b) => MathLogic.XNOR(a, b), "xnor");
        }

        public void TestLogicOperator(float[] a, float[] b, float[] expected, Func<float, float, float> fn, string operatorType)
        {
            for (int i = 0; i < a.Length; i++)
            {
                Assert.IsTrue(fn.Invoke(a[i], b[i]) == expected[i], operatorType + ": " + fn.Invoke(a[i], b[i]) + " != " + expected[i]);
            }
        }
    }
}

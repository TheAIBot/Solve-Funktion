using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using Solve_Funktion;

namespace Tests
{
    [TestClass]
    public class VectorLogicTests
    {
        [TestMethod]
        public void NOTTest()
        {
        }

        [TestMethod]
        public void ANDTest()
        {
        }

        [TestMethod]
        public void NANDTest()
        {
        }

        [TestMethod]
        public void ORTest()
        {
        }

        [TestMethod]
        public void NORTest()
        {
        }

        [TestMethod]
        public void XORTest()
        {
        }

        [TestMethod]
        public void XNORTest()
        {
        }

        public void TestLogicOperator(double[] result, double[] expected, string operatorType, int toCheck)
        {
            for (int i = 0; i < toCheck; i++)
            {
                Assert.IsTrue(result[i] == expected[i], operatorType + ": " + result + " != " + expected);
            }
        }
    }
}

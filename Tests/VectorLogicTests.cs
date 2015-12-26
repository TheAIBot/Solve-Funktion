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
            TestLogicOperator(ShittyVectorLogic.NOT(TestTools.CreateVector(1, 1)),
                              TestTools.CreateVector(0, 0), "NOT", 2);
            TestLogicOperator(ShittyVectorLogic.NOT(TestTools.CreateVector(1, 0)),
                              TestTools.CreateVector(0, 1), "NOT", 2);
            TestLogicOperator(ShittyVectorLogic.NOT(TestTools.CreateVector(0, 1)),
                              TestTools.CreateVector(1, 0), "NOT", 2);
            TestLogicOperator(ShittyVectorLogic.NOT(TestTools.CreateVector(0, 0)),
                              TestTools.CreateVector(1, 1), "NOT", 2);
        }

        [TestMethod]
        public void ANDTest()
        {
            TestLogicOperator(ShittyVectorLogic.AND(TestTools.CreateVector(1, 0),
                                                    TestTools.CreateVector(1, 0)), 
                              TestTools.CreateVector(1, 0), "AND", 2);
            TestLogicOperator(ShittyVectorLogic.AND(TestTools.CreateVector(1, 0),
                                                    TestTools.CreateVector(0, 1)),
                              TestTools.CreateVector(0, 0), "AND", 2);
        }

        [TestMethod]
        public void NANDTest()
        {
            TestLogicOperator(ShittyVectorLogic.NAND(TestTools.CreateVector(1, 0),
                                                    TestTools.CreateVector(1, 0)),
                              TestTools.CreateVector(0, 1), "NAND", 2);
            TestLogicOperator(ShittyVectorLogic.NAND(TestTools.CreateVector(1, 0),
                                                    TestTools.CreateVector(0, 1)),
                              TestTools.CreateVector(1, 1), "NAND", 2);
        }

        [TestMethod]
        public void ORTest()
        {
            TestLogicOperator(ShittyVectorLogic.OR(TestTools.CreateVector(1, 0),
                                                    TestTools.CreateVector(1, 0)),
                              TestTools.CreateVector(1, 0), "OR", 2);
            TestLogicOperator(ShittyVectorLogic.OR(TestTools.CreateVector(1, 0),
                                                    TestTools.CreateVector(0, 1)),
                              TestTools.CreateVector(1, 1), "OR", 2);
        }

        [TestMethod]
        public void NORTest()
        {
            TestLogicOperator(ShittyVectorLogic.NOR(TestTools.CreateVector(1, 0),
                                                    TestTools.CreateVector(1, 0)),
                              TestTools.CreateVector(0, 1), "NOR", 2);
            TestLogicOperator(ShittyVectorLogic.NOR(TestTools.CreateVector(1, 0),
                                                    TestTools.CreateVector(0, 1)),
                              TestTools.CreateVector(0, 0), "NOR", 2);
        }

        [TestMethod]
        public void XORTest()
        {
            TestLogicOperator(ShittyVectorLogic.XOR(TestTools.CreateVector(1, 0),
                                                    TestTools.CreateVector(1, 0)),
                              TestTools.CreateVector(0, 0), "XOR", 2);
            TestLogicOperator(ShittyVectorLogic.XOR(TestTools.CreateVector(1, 0),
                                                    TestTools.CreateVector(0, 1)),
                              TestTools.CreateVector(1, 1), "XOR", 2);
        }

        [TestMethod]
        public void XNORTest()
        {
            TestLogicOperator(ShittyVectorLogic.XNOR(TestTools.CreateVector(1, 0),
                                                    TestTools.CreateVector(1, 0)),
                              TestTools.CreateVector(1, 1), "XNOR", 2);
            TestLogicOperator(ShittyVectorLogic.XNOR(TestTools.CreateVector(1, 0),
                                                    TestTools.CreateVector(0, 1)),
                              TestTools.CreateVector(0, 0), "XNOR", 2);
        }

        public void TestLogicOperator(Vector<double> result, Vector<double> expected, string operatorType, int toCheck)
        {
            for (int i = 0; i < toCheck; i++)
            {
                Assert.IsTrue(result[i] == expected[i], operatorType + ": " + result + " != " + expected);
            }
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using Solve_Funktion;

namespace Tests
{
    [TestClass]
    public class VectorMathTests
    {
        const int DefaultLoopLimit = 100000; // 100.000
        Random rDom = new Random(12322);

        [TestMethod]
        public void TestModulus()
        {
            for (int i = 0; i < DefaultLoopLimit; i++)
            {
                Vector<double> num = TestTools.CreateVector(rDom.Next(-50, 51), rDom.Next(-50, 51));
                Vector<double> mod = TestTools.CreateVector(rDom.Next(-50, 51), rDom.Next(-50, 51));
                Vector<double> expectedResult = ShittyVectorMath.Modulus(num, mod);
                Vector<double> result = ShittyVectorMath.BetterModulus(num, mod);
                Assert.IsTrue(TestTools.IsVectorsEqual(result, expectedResult), num.ToString() + " % " + mod.ToString() + " != " + expectedResult.ToString() + "but gave " + result.ToString());
            }
        }

        [TestMethod]
        public void TestFloor()
        {
            for (int i = 0; i < DefaultLoopLimit; i++)
            {
                Vector<double> num = TestTools.CreateVector(TestTools.GetRandomDouble(-50, 51), TestTools.GetRandomDouble(-50, 51));
                Vector<double> expectedResult = ShittyVectorMath.Floor(num);
                Vector<double> result = ShittyVectorMath.BetterFloor(num);
                Assert.IsTrue(TestTools.IsVectorsEqual(result, expectedResult), "Floor(" + num.ToString() + ") " + " != " + expectedResult.ToString() + "but gave " + result.ToString());
            }
        }
    }
}

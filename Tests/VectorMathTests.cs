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
                Vector<double> num = CreateVector(rDom.Next(-50, 51), rDom.Next(-50, 51));
                Vector<double> mod = CreateVector(rDom.Next(-50, 51), rDom.Next(-50, 51));
                Vector<double> expectedResult = ShittyVectorMath.Modulus(num, mod);
                Vector<double> result = ShittyVectorMath.BetterModulus(num, mod);
                Assert.IsTrue(IsVectorsEqual(result, expectedResult), num.ToString() + " % " + mod.ToString() + " != " + expectedResult.ToString() + "but gave " + result.ToString());
            }
        }

        [TestMethod]
        public void TestFloor()
        {
            for (int i = 0; i < DefaultLoopLimit; i++)
            {
                Vector<double> num = CreateVector(GetRandomDouble(-50, 51), GetRandomDouble(-50, 51));
                Vector<double> expectedResult = ShittyVectorMath.Floor(num);
                Vector<double> result = ShittyVectorMath.BetterFloor(num);
                Assert.IsTrue(IsVectorsEqual(result, expectedResult), "Floor(" + num.ToString() + ") " + " != " + expectedResult.ToString() + "but gave " + result.ToString());
            }
        }

        private Vector<double> CreateVector(params double[] data)
        {
            if (data.Length < Vector<double>.Count)
            {
                double[] newData = new double[Vector<double>.Count];
                Array.Copy(data, newData, data.Length);
                data = newData;
            }
            return new Vector<double>(data);
        }

        private bool IsVectorsEqual(Vector<double> one, Vector<double> two)
        {
            for (int i = 0; i < Vector<double>.Count; i++)
            {
                if (one[i] != two[i])
                {
                    return false;
                }
            }
            return true;
        }

        private double GetRandomDouble(int min, int max)
        {
            return (double)rDom.Next(min, max) + rDom.NextDouble();
        }

    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solve_Funktion;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Windows;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Tests
{
    [TestClass]
    public class Test
    {
        const int DefaultLoopLimit = 100000; // 100.000

        [TestMethod]
        public void MassSingleOPCopy()
        {
            for (int i = 0; i < DefaultLoopLimit; i++)
            {
                SingleOPCopyTest();
            }
        }
        public void SingleOPCopyTest()
        {
            Equation Cand = TestTools.MakeRandomEquation();
            Operator Original = Cand.EquationParts.First();
            Operator Copy = TestTools.MakeSingleOperator();
            Copy.Eq.Cleanup();
            Original.GetCopy(Copy, Cand,Cand.EquationParts, Cand);

            RecursiveCompareOperators(Original, Copy);
        }

        [TestMethod]
        public void MassCopyEquationInfoTest()
        {
            for (int i = 0; i < DefaultLoopLimit; i++)
            {
                CopyEquationInfoTest();
            }
        }
        public void CopyEquationInfoTest()
        {
            Equation Cand = TestTools.MakeRandomEquation();
            Equation Derp = TestTools.MakeRandomEquation();
            Derp.Cleanup();
            Equation Copy = Cand.MakeClone(Derp);

            EquationsAreSame(Cand, Copy);
        }

        [TestMethod]
        public void MassEquationStoreAndCleanupTest()
        {
            for (int i = 0; i < DefaultLoopLimit; i++)
            {
                EquationStoreAndCleanupTest();
            }
        }
        public void EquationStoreAndCleanupTest()
        {
            Equation Cand = TestTools.MakeRandomEquation();
            int StartOPCount = Cand.EInfo.MaxSize;
            Cand.Cleanup();

            Assert.AreEqual(Cand.EquationParts.Count, 0, "EquationParts is not empty");
            Assert.AreEqual(Cand.AllOperators.Count, 0, "AllOperators is not empty");
            Assert.AreEqual(Cand.OPStorage.Count, StartOPCount, "Missing OP in storage");
            Assert.AreEqual(Cand.SortedOperators.Sum(x => x.Count), 0, "SortedOperators is not empty");
        }

        [TestMethod]
        public void MassOPStoreAndCleaupTest()
        {
            for (int i = 0; i < DefaultLoopLimit; i++)
            {
                OPStoreAndCleanupTest();
            }
        }
        public void OPStoreAndCleanupTest()
        {
            Equation Cand = TestTools.MakeRandomEquation();
            int OPStorageCount = Cand.OPStorage.Count;
            int AllOperatorsCount = Cand.AllOperators.Count;
            int SortedopreatorsCount = Cand.SortedOperators.Sum(x => x.Count);
            int OperatorsLeftCount = Cand.OperatorsLeft;
            Operator Oper = Cand.EquationParts.First();
            int OPCount = Oper.MFunction.GetOperatorCount(Oper);
            int ContainedListCount = Cand.EquationParts.Count;
            Oper.StoreAndCleanup();

            Assert.AreEqual(OPStorageCount + OPCount, Cand.OPStorage.Count, "Not all operators was put back into storage");
            Assert.AreEqual(AllOperatorsCount - OPCount, Cand.AllOperators.Count, "Not all operators was removed from AllOperators");
            Assert.AreEqual(SortedopreatorsCount - OPCount, Cand.SortedOperators.Sum(x => x.Count), "Not all operators was removed from SortedOperators");
            Assert.AreEqual(OperatorsLeftCount + OPCount, Cand.OperatorsLeft, "OperatorsLeft doesn't match the expected result");
            Assert.AreEqual(ContainedListCount - 1, Cand.EquationParts.Count, "Operator was not removed from ContainedList");
        }

        [TestMethod]
        public void MassCheckEquationInfo_OperatorsLeft()
        {
            for (int i = 0; i < DefaultLoopLimit; i++)
            {
                CheckEquationInfo_OperatorsLeft();
            }
        }
        public void CheckEquationInfo_OperatorsLeft()
        {
            SynchronizedRandom.CreateRandom();
            Equation Cand = TestTools.MakeEquation();
            Assert.AreEqual(Cand.OperatorsLeft, Cand.EInfo.MaxSize, "OperatorsLeft return wrong value");
            if (Cand.OperatorsLeft > 0)
            {
                Operator ToAdd = new Operator(Cand);
                ToAdd.MakeRandom(Cand.EquationParts, Cand);
                int DerpCount = ToAdd.GetOperatorCount();
                Assert.AreEqual(Cand.OperatorsLeft, Cand.EInfo.MaxSize - DerpCount, "OperatorsLeft return wrong value");
            }
        }

        [TestMethod]
        public void MassCheckEquationMaking()
        {
            for (int i = 0; i < DefaultLoopLimit; i++)
            {
                CheckEquationMaking();
            }
        }
        public void CheckEquationMaking()
        {
            Equation Cand = TestTools.MakeRandomEquation();
            Cand.CalcTotalOffSet();

            VerifyEquation(Cand);
        }

        [TestMethod]
        public void MassTestEvolveCand()
        {
            for (int i = 0; i < DefaultLoopLimit; i++)
            {
                TestEvolveCand();
            }
        }
        public void TestEvolveCand()
        {
            Equation Cand = TestTools.MakeRandomEquation();
            EvolveCand.EvolveCandidate(Cand.EInfo, Cand);

            VerifyEquation(Cand);
        }

        public void VerifyEquation(Equation Cand)
        {
            Assert.IsTrue(Cand.EquationParts.Count >= 0 && Cand.EquationParts.Count <= Cand.EInfo.MaxSize,
              "Equationparts hold incorrect amount of operators" + Environment.NewLine +
              "Holds:" + Cand.EquationParts.Count.ToString() + Environment.NewLine +
              "Expected 0 < " + Cand.EInfo.MaxSize.ToString());
            Assert.IsTrue(Cand != null, "Equation Is null");
            Assert.IsTrue(Cand.AllOperators.Count == Cand.EInfo.MaxSize - Cand.OperatorsLeft);
        }

        public void EquationsAreSame(Equation Original, Equation Copy)
        {
            Assert.AreEqual(Original.AllOperators.Count, Copy.AllOperators.Count, "AllOperators Count is not the same");
            Assert.AreEqual(Original.SortedOperators.Count, Copy.SortedOperators.Count, "SortedOperators Count is not the same");

            for (int i = 0; i < Original.AllOperators.Count; i++)
            {
                RecursiveCompareOperators(Original.AllOperators[i], Copy.AllOperators[i]);
            }
            if (!Original.SortedOperators.All(x =>  Copy.SortedOperators.Any(z => z.Count == x.Count)))
            {
                Assert.Fail("SortedOperators is not the same");
            }
        }

        public void RecursiveCompareOperators(Operator Original, Operator Copy)
        {
            SimpleOPCopyCheck(Original, Copy);
            for (int i = 0; i < Original.Operators.Count; i++)
            {
                RecursiveCompareOperators(Original.Operators[i], Copy.Operators[i]);
            }
        }

        public void SimpleOPCopyCheck(Operator Original, Operator Copy)
        {
            Assert.AreEqual(Original.RandomNumber, Copy.RandomNumber, "Number is not the same");
            Assert.AreEqual(Original.ParameterIndex, Copy.ParameterIndex, "parameter index is not the same");
            Assert.AreEqual(Original.MFunction, Copy.MFunction, "Operator is not the same");
            Assert.AreEqual(Original.ResultOnRightSide, Copy.ResultOnRightSide, "Side is not the same");
            Assert.AreEqual(Original.UseRandomNumber, Copy.UseRandomNumber, "UseNumber is not the same");
            Assert.AreEqual(Original.Operators.Count, Copy.Operators.Count, "Operators Count is not the same");
            Assert.AreEqual(Original.MaxCalculated, Copy.MaxCalculated, "Max calculated number is not the same");
            int Index = 0;
            Assert.IsTrue(Original.OperatorResults.ToList().All(x => x == Copy.OperatorResults[Index++]));
            //can't check for correct holder. Nee a way to check that the holder is correct
        }



    }
}

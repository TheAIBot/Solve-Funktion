using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solve_Funktion;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Windows;

namespace Tests
{
    [TestClass]
    public class Test
    {
        const int DefaultLoopLimit = 100000; // 100.000

        [TestMethod]
        public void MassSingleOPCopy()
        {
            PrepareTests();
            for (int i = 0; i < DefaultLoopLimit; i++)
            {
                SingleOPCopyTest();
            }
        }
        public void SingleOPCopyTest()
        {
            Equation Cand = MakeRandomEquation(Info.MaxSize * 2);
            Operator Original = Cand.EquationParts.First();
            Operator Copy = new Operator();
            Original.GetCopy(Copy, Cand,Cand.EquationParts);

            RecursiveCompareOperators(Original, Copy);
        }

        [TestMethod]
        public void MassCopyEquationInfoTest()
        {
            PrepareTests();
            for (int i = 0; i < DefaultLoopLimit; i++)
            {
                CopyEquationInfoTest();
            }
        }
        public void CopyEquationInfoTest()
        {
            Equation Cand = MakeRandomEquation();
            Equation Derp = MakeRandomEquation();
            Derp.Cleanup();
            Equation Copy = Cand.MakeClone(Derp);

            EquationsAreSame(Cand, Copy);
        }

        [TestMethod]
        public void MassEquationStoreAndCleanupTest()
        {
            PrepareTests();
            for (int i = 0; i < DefaultLoopLimit; i++)
            {
                EquationStoreAndCleanupTest();
            }
        }
        public void EquationStoreAndCleanupTest()
        {
            Equation Cand = MakeRandomEquation();
            int StartOPCount = Info.MaxSize;
            Cand.Cleanup();

            Assert.AreEqual(Cand.EquationParts.Count, 0, "EquationParts is not empty");
            Assert.AreEqual(Cand.AllOperators.Count, 0, "AllOperators is not empty");
            Assert.AreEqual(Cand.OPStorage.Count, StartOPCount, "Missing OP in storage");
            Assert.AreEqual(Cand.SortedOperators.Sum(x => x.Count), 0, "SortedOperators is not empty");
        }

        [TestMethod]
        public void MassOPStoreAndCleaupTest()
        {
            PrepareTests();
            for (int i = 0; i < DefaultLoopLimit; i++)
            {
                OPStoreAndCleanupTest();
            }
        }
        public void OPStoreAndCleanupTest()
        {
            Equation Cand = MakeRandomEquation();
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
            PrepareTests();
            for (int i = 0; i < DefaultLoopLimit; i++)
            {
                CheckEquationInfo_OperatorsLeft();
            }
        }
        public void CheckEquationInfo_OperatorsLeft()
        {
            SynchronizedRandom.CreateRandom();
            Stack<Operator> TestStorage = new Stack<Operator>();
            for (int i = 0; i < Info.MaxSize; i++)
            {
                TestStorage.Push(new Operator());
            }
            Equation Cand = new Equation(Info.MaxSize);
            Assert.AreEqual(Cand.OperatorsLeft, Info.MaxSize, "OperatorsLeft return wrong value");
            if (Cand.OperatorsLeft > 0)
            {
                Operator ToAdd = new Operator();
                ToAdd.MakeRandom(Cand, Cand.EquationParts);
                int DerpCount = ToAdd.GetOperatorCount();
                Assert.AreEqual(Cand.OperatorsLeft, Info.MaxSize - DerpCount, "OperatorsLeft return wrong value");
            }
        }

        [TestMethod]
        public void MassCheckEquationMaking()
        {
            PrepareTests();
            for (int i = 0; i < DefaultLoopLimit; i++)
            {
                CheckEquationMaking();
            }
        }
        public void CheckEquationMaking()
        {
            Equation Cand = MakeRandomEquation();
            Cand.CalcTotalOffSet();

            VerifyEquation(Cand);
        }

        [TestMethod]
        public void MassTestEvolveCand()
        {
            PrepareTests();
            for (int i = 0; i < DefaultLoopLimit; i++)
            {
                TestEvolveCand();
            }
        }
        public void TestEvolveCand()
        {
            Equation Cand = MakeRandomEquation();
            EvolveCand.EvolveCandidate(Cand);

            VerifyEquation(Cand);
        }

        public void PrepareTests()
        {
            const string SeqX = "1,2,3,4, 5, 6, 7, 8, 9,10";
            double[] SeqRX = SeqX.Split(',').Select(x => Convert.ToDouble(x, CultureInfo.InvariantCulture.NumberFormat)).ToArray();
            const string SeqY = "2,3,5,7,11,13,17,19,23,29";
            double[] SeqRY = SeqY.Split(',').Select(x => Convert.ToDouble(x, CultureInfo.InvariantCulture.NumberFormat)).ToArray();
            Info.Seq = new Point[SeqRX.Length];
            for (int i = 0; i < SeqRX.Length; i++)
            {
                Info.Seq[i] = new Point(SeqRX[i], SeqRY[i]);
            }
        }

        public void VerifyEquation(Equation Cand)
        {
            Assert.IsTrue(Cand.EquationParts.Count >= 0 && Cand.EquationParts.Count <= Info.MaxSize,
              "Equationparts hold incorrect amount of operators" + Environment.NewLine +
              "Holds:" + Cand.EquationParts.Count.ToString() + Environment.NewLine +
              "Expected 0 < " + Info.MaxSize.ToString());
            Assert.IsTrue(Cand != null, "Equation Is null");
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
            Assert.AreEqual(Original.Number, Copy.Number, "Number is not the same");
            Assert.AreEqual(Original.MFunction, Copy.MFunction, "Operator is not the same");
            Assert.AreEqual(Original.ResultOnRightSide, Copy.ResultOnRightSide, "Side is not the same");
            Assert.AreEqual(Original.UseNumber, Copy.UseNumber, "UseNumber is not the same");
            Assert.AreEqual(Original.Operators.Count, Copy.Operators.Count, "Operators Count is not the same");
        }

        public Equation MakeRandomEquation()
        {
            Equation Cand = new Equation(Info.MaxSize);
            SynchronizedRandom.CreateRandom();
            Cand.MakeRandom();
            return Cand;
        }
        public Equation MakeRandomEquation(int OperatorCount)
        {
            Stack<Operator> OperatorStorage = new Stack<Operator>();
            for (int i = 0; i < OperatorCount; i++)
            {
                OperatorStorage.Push(new Operator());
            }
            Equation Cand = new Equation(OperatorStorage);
            SynchronizedRandom.CreateRandom();
            Cand.MakeRandom();
            return Cand;
        }
    }
}

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
            Equation Cand = MakeRandomEquation();
            Operator Original = Cand.EquationParts.First();
            Operator Copy = MakeSingleOperator();
            Original.GetCopy(Copy, Cand,Cand.EquationParts);

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
            Equation Cand = MakeRandomEquation();
            Equation Derp = MakeRandomEquation();
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
            Equation Cand = MakeRandomEquation();
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
            for (int i = 0; i < DefaultLoopLimit; i++)
            {
                CheckEquationInfo_OperatorsLeft();
            }
        }
        public void CheckEquationInfo_OperatorsLeft()
        {
            SynchronizedRandom.CreateRandom();
            Equation Cand = MakeEquation();
            Assert.AreEqual(Cand.OperatorsLeft, Cand.EInfo.MaxSize, "OperatorsLeft return wrong value");
            if (Cand.OperatorsLeft > 0)
            {
                Operator ToAdd = new Operator(Cand);
                ToAdd.MakeRandom(Cand.EquationParts);
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
            Equation Cand = MakeRandomEquation();
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
            Equation Cand = MakeRandomEquation();
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
            Assert.AreEqual(Original.randomNumber, Copy.randomNumber, "Number is not the same");
            Assert.AreEqual(Original.parameterIndex, Copy.parameterIndex, "parameter index is not the same");
            Assert.AreEqual(Original.MFunction, Copy.MFunction, "Operator is not the same");
            Assert.AreEqual(Original.ResultOnRightSide, Copy.ResultOnRightSide, "Side is not the same");
            Assert.AreEqual(Original.UseRandomNumber, Copy.UseRandomNumber, "UseNumber is not the same");
            Assert.AreEqual(Original.Operators.Count, Copy.Operators.Count, "Operators Count is not the same");
        }

        public Equation MakeRandomEquation()
        {
            Equation Cand = new Equation(GetEvolutionInfo());
            SynchronizedRandom.CreateRandom();
            Cand.MakeRandom();
            return Cand;
        }

        public Equation MakeEquation()
        {
            Equation Cand = new Equation(GetEvolutionInfo());
            return Cand;
        }

        public Operator MakeSingleOperator()
        {
            return MakeEquation().OPStorage.Pop();
        }

        private EvolutionInfo GetEvolutionInfo()
        {
            const string SequenceX = " 1,  2, 3,  4, 5, 6,7,  8,  9, 10";
            const string SequenceY = "74,143,34,243,23,52,9,253,224,231";

            VectorPoint[] Seq = GetSequence(SequenceX,
                                            SequenceY);

            MathFunction[] Operators = new MathFunction[]
            {
                new Plus(),
                new Subtract(),
                new Multiply(),
                new Divide(),

                //new PowerOf(),
                new Root(),
                //new Exponent(),
                //new NaturalLog(),
                //new Log(),

                //new Modulos(),
                //new Floor(),
                //new Ceil(),
                //new Round(),

                //new Sin(),
                //new Cos(),
                //new Tan(),
                //new ASin(),
                //new ACos(),
                //new ATan(),

                new Parentheses(),
                new Absolute(),

                new AND(),
                new NAND(),
                new OR(),
                new NOR(),
                new XOR(),
                new XNOR(),
                new NOT()
            };

            return new EvolutionInfo(
                Seq,    // Sequence
                40,    // MaxSize
                5,     // MaxChange
                30000,  // CandidatesPerGen
                102,    // NumberRangeMax
                0,      // NumberRangeMin
                1,      // SpeciesAmount
                100,    // MaxStuckGens
                0.8,    // EvolvedCandidatesPerGen
                0,      // RandomCandidatesPerGen
                0.2,    // SmartCandidatesPerGen
                Operators // Operatres that can be used in an equation
                );
        }

        private VectorPoint[] GetSequence(string SequenceX, string SequenceY)
        {
            string[] lines = Regex.Split(SequenceX, "} *,");
            string[] refined = lines.Select(x => Regex.Replace(x, "[ =}]", String.Empty)).ToArray();
            string[][] namesAndValues = refined.Select(x => x.Split('{')).ToArray();
            string[] names = namesAndValues.Select(x => x[0]).ToArray();
            double[][] SeqRX = namesAndValues.Select(x => x[1].Split(',').Select(z => Convert.ToDouble(z, CultureInfo.InvariantCulture.NumberFormat)).ToArray()).ToArray();
            double[] SeqRY = SequenceY.Split(',').Select(x => Convert.ToDouble(x, CultureInfo.InvariantCulture.NumberFormat)).ToArray();
            return GetSequence(names, SeqRX, SeqRY);
        }
        private VectorPoint[] GetSequence(string[] names, double[][] parameters, double[] functionValues)
        {
            VectorPoint[] Seq = new VectorPoint[(int)Math.Ceiling((double)parameters[0].Length / (double)Constants.VECTOR_LENGTH)];
            int[] counts = new int[parameters.Length];
            Vector<double>[] functionValuesVector = getParameterValues(functionValues, out counts);
            Vector<double>[][] splittedParameters = new Vector<double>[parameters.Length][];
            for (int i = 0; i < parameters.Length; i++)
            {
                int[] count;
                splittedParameters[i] = getParameterValues(parameters[i], out count);
            }
            for (int i = 0; i < splittedParameters.Length; i++)
            {
                Vector<double>[] parameterValues = new Vector<double>[splittedParameters[0].Length];
                for (int j = 0; j < splittedParameters[0].Length; j++)
                {
                    parameterValues[j] = splittedParameters[i][j];
                }
                Seq[i] = new VectorPoint(parameterValues, names, functionValuesVector[i], counts[i]);
            }
            return Seq;
        }

        private Vector<double>[] getParameterValues(double[] parameter, out int[] vectorSize)
        {
            Vector<double>[] parameterValues = new Vector<double>[(int)Math.Ceiling((double)parameter.Length / (double)Constants.VECTOR_LENGTH)];
            int index = 0;
            vectorSize = new int[parameterValues.Length];
            for (int i = 0; i < parameter.Length; i += Constants.VECTOR_LENGTH)
            {
                int sizeLeft = parameter.Length - i;
                Vector<double> partialParameterVector;
                if (sizeLeft >= Constants.VECTOR_LENGTH)
                {
                    partialParameterVector = new Vector<double>(parameter, i);
                    vectorSize[i] = Constants.VECTOR_LENGTH;
                }
                else
                {
                    vectorSize[i] = sizeLeft;
                    double[] rXData = new double[Constants.VECTOR_LENGTH];

                    Array.Copy(parameter, i, rXData, 0, sizeLeft);

                    int missingNumbers = Constants.VECTOR_LENGTH - sizeLeft;
                    for (int y = 1; y < missingNumbers + 1; y++)
                    {
                        rXData[y] = rXData[0];
                    }
                    partialParameterVector = new Vector<double>(rXData);
                }
                parameterValues[index] = partialParameterVector;
                index++;
            }
            return parameterValues;
        }
    }
}

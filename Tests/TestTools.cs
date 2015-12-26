using Solve_Funktion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tests
{
    public static class TestTools
    {
        private static readonly Random rDom = new Random(12322);

        public static Equation MakeRandomEquation()
        {
            Equation Cand = new Equation(GetEvolutionInfo());
            SynchronizedRandom.CreateRandom();
            Cand.MakeRandom();
            return Cand;
        }

        public static Equation MakeEquation()
        {
            Equation Cand = new Equation(GetEvolutionInfo());
            return Cand;
        }

        public static Equation MakeEquation(string parameters, string result)
        {
            return new Equation(GetEvolutionInfo(parameters, result));
        }

        public static Operator MakeSingleOperator()
        {
            return MakeEquation().OPStorage.Pop();
        }

        public static EvolutionInfo GetEvolutionInfo()
        {
            const string SequenceX = "x = { 1,  2, 3,  4, 5, 6,7,  8,  9, 10}";
            const string SequenceY = "     74,143,34,243,23,52,9,253,224,231";

            return GetEvolutionInfo(SequenceX, SequenceY);
        }

        public static EvolutionInfo GetEvolutionInfo(string parameters, string result)
        {
            VectorPoint[] Seq = GetSequence(parameters,
                                            result);

            MathFunction[] Operators = new MathFunction[]
            {
                new Plus(),
                new Subtract(),
                new Multiply(),
                new Divide(),

                new PowerOf(),
                new Root(),
                new Exponent(),
                new NaturalLog(),
                new Log(),

                new Modulos(),
                new Floor(),
                new Ceil(),
                new Round(),

                new Sin(),
                new Cos(),
                new Tan(),
                new ASin(),
                new ACos(),
                new ATan(),

                new Parentheses(),
                new Absolute(),

                //new AND(),
                //new NAND(),
                //new OR(),
                //new NOR(),
                //new XOR(),
                //new XNOR(),
                //new NOT()
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

        public static VectorPoint[] GetSequence(string SequenceX, string SequenceY)
        {
            string[] lines = Regex.Split(SequenceX, "} *,");
            string[] refined = lines.Select(x => Regex.Replace(x, "[ =}]", String.Empty)).ToArray();
            string[][] namesAndValues = refined.Select(x => x.Split('{')).ToArray();
            string[] names = namesAndValues.Select(x => x[0]).ToArray();
            double[][] SeqRX = namesAndValues.Select(x => x[1].Split(',').Select(z => Convert.ToDouble(z, CultureInfo.InvariantCulture.NumberFormat)).ToArray()).ToArray();
            double[] SeqRY = SequenceY.Split(',').Select(x => Convert.ToDouble(x, CultureInfo.InvariantCulture.NumberFormat)).ToArray();
            return GetSequence(names, SeqRX, SeqRY);
        }
        public static VectorPoint[] GetSequence(string[] names, double[][] parameters, double[] functionValues)
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
            for (int i = 0; i < splittedParameters[0].Length; i++)
            {
                Vector<double>[] parameterValues = new Vector<double>[splittedParameters.Length];
                for (int j = 0; j < splittedParameters.Length; j++)
                {
                    parameterValues[j] = splittedParameters[j][i];
                }
                Seq[i] = new VectorPoint(parameterValues, names, functionValuesVector[i], counts[i]);
            }
            return Seq;
        }

        private static Vector<double>[] getParameterValues(double[] parameter, out int[] vectorSize)
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
                    vectorSize[index] = Constants.VECTOR_LENGTH;
                }
                else
                {
                    vectorSize[index] = sizeLeft;
                    double[] rXData = new double[Constants.VECTOR_LENGTH];

                    Array.Copy(parameter, i, rXData, 0, sizeLeft);

                    int missingNumbers = Constants.VECTOR_LENGTH - sizeLeft;
                    for (int y = sizeLeft; y < missingNumbers + sizeLeft; y++)
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

        public static Vector<double> CreateVector(params double[] data)
        {
            if (data.Length < Constants.VECTOR_LENGTH)
            {
                double[] newData = new double[Constants.VECTOR_LENGTH];
                Array.Copy(data, newData, data.Length);
                data = newData;
            }
            return new Vector<double>(data);
        }

        public static Vector<double> CreateVectorRepeat(double number)
        {
            double[] result = new double[Constants.VECTOR_LENGTH];
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                result[i] = number;
            }
            return new Vector<double>(result);
        }

        public static bool IsVectorsEqual(Vector<double> one, Vector<double> two)
        {
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                if (one[i] != two[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static double GetRandomDouble(int min, int max)
        {
            return (double)rDom.Next(min, max) + rDom.NextDouble();
        }
    }
}

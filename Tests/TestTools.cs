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
        private static readonly SynchronizedRandom Randomizer = new SynchronizedRandom();

        public static Equation MakeRandomEquation()
        {
            Equation Cand = new Equation(GetEvolutionInfo(), Randomizer);
            Cand.MakeRandom();
            return Cand;
        }

        public static Equation MakeEquation()
        {
            Equation Cand = new Equation(GetEvolutionInfo(), Randomizer);
            return Cand;
        }

        public static Equation MakeEquation(string parameters, string result)
        {
            return new Equation(GetEvolutionInfo(parameters, result), Randomizer);
        }

        public static Equation MakeEquation(EvolutionInfo EInfo)
        {
            return new Equation(EInfo, Randomizer);
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
            CoordInfo Seq = GetSequence(parameters, result);

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

        public static CoordInfo GetSequence(string SequenceX, string SequenceY)
        {
            string[] lines = Regex.Split(SequenceX, "} *,");
            string[] refined = lines.Select(x => Regex.Replace(x, "[ =}]", String.Empty)).ToArray();
            string[][] namesAndValues = refined.Select(x => x.Split('{')).ToArray();
            string[] names = namesAndValues.Select(x => x[0]).ToArray();
            double[][] SeqRX = namesAndValues.Select(x => x[1].Split(',').Select(z => Convert.ToDouble(z, CultureInfo.InvariantCulture.NumberFormat)).ToArray()).ToArray();
            double[] SeqRY = SequenceY.Split(',').Select(x => Convert.ToDouble(x, CultureInfo.InvariantCulture.NumberFormat)).ToArray();
            return new CoordInfo(SeqRY, SeqRX, names);
        }

        public static double[] CreateArray(params double[] parameters)
        {
            return parameters;
        }

        public static double GetRandomDouble(int min, int max)
        {
            return (double)rDom.Next(min, max) + rDom.NextDouble();
        }
    }
}

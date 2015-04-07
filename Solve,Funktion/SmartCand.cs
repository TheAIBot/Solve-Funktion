using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Solve_Funktion
{
    public static class SmartCand
    {
        public static List<Equation> SmartifyCandidates(List<Equation> Copys, Equation BCand, Equation OCand)
        {
            //this doesn't work at all due to the function CanSmartChangeNumbers always returning -1
            List<Equation> SmartCandidates = new List<Equation>();
            List<int> Indexes = CanSmartChangeNumbers(BCand, OCand);
            if (Indexes.Count > 0)
            {
                SmartCandidates = SmartChangeNumbers(Copys, BCand, OCand, Indexes);
            }
            else
            {
                SmartCandidates = StupidChangeNumbers(Copys, BCand);
            }
            return SmartCandidates;
        }

        private static List<int> CanSmartChangeNumbers(Equation BCand, Equation OCand)
        {
            List<int> SmartChangeOperatorIndexes = new List<int>();
            if (BCand.AllOperators.Count != OCand.AllOperators.Count)
                return SmartChangeOperatorIndexes;

            for (int i = 0; i < BCand.AllOperators.Count; i++)
            {
                Operator BCandOper = BCand.AllOperators[i];
                Operator OCandOper = OCand.AllOperators[i];
                if (BCandOper.ResultOnRightSide      == OCandOper.ResultOnRightSide      &&
                    BCandOper.MFunction == OCandOper.MFunction &&
                    BCandOper.UseNumber == OCandOper.UseNumber &&
                    BCandOper.Number    != OCandOper.Number    &&
                    BCandOper.UseNumber)
                {
                    SmartChangeOperatorIndexes.Add(i);
                }
            }
            return SmartChangeOperatorIndexes;
        }

        private static List<Equation> SmartChangeNumbers(List<Equation> Copys, Equation BCand, Equation OCand, List<int> Indexes)
        {
            foreach (Equation Eq in Copys)
            {
                foreach (int Index in Indexes)
                {
                    Operator BCandOper = BCand.AllOperators[Index];
                    Operator OCandOper = OCand.AllOperators[Index];
                    if (BCandOper.Number > OCandOper.Number)
                    {
                        Eq.AllOperators[Index].Number = SynchronizedRandom.NextDouble(Info.NumberRangeMin, (int)BCandOper.Number + 1);
                    }
                    else
                    {
                        Eq.AllOperators[Index].Number = SynchronizedRandom.NextDouble((int)BCandOper.Number, Info.NumberRangeMax);
                    }
                    Eq.CalcOffSet();
                }
            }
            return Copys;
        }

        private static List<Equation> StupidChangeNumbers(List<Equation> Copys, Equation BCand)
        {
            foreach (Equation Cand in Copys)
            {
                int AmountToChange = SynchronizedRandom.Next(1, Info.MaxChange);
                for (int i = 0; i < AmountToChange; i++)
                {
                    int Index = SynchronizedRandom.Next(0, BCand.EquationParts.Count);
                    Cand.EquationParts[Index].Number = SynchronizedRandom.NextDouble(Info.NumberRangeMin, Info.NumberRangeMax);
                }
                Cand.CalcOffSet();
            }
            return Copys;
        }


    }
}

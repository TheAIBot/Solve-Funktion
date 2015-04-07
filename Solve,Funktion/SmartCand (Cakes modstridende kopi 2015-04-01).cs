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
                Operator BCandOper = BCand.AllOperators.ElementAt(i);
                Operator OCandOper = OCand.AllOperators.ElementAt(i);
                if (BCandOper.Side == OCandOper.Side &&
                    BCandOper.MFunction == OCandOper.MFunction &&
                    BCandOper.UseNumber == OCandOper.UseNumber &&
                    BCandOper.Number != OCandOper.Number
                      && BCandOper.UseNumber)
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
                    Operator BCandOper = BCand.AllOperators.ElementAt(Index);
                    Operator OCandOper = OCand.AllOperators.ElementAt(Index);
                    if (BCandOper.Number > OCandOper.Number)
                    {
                        Eq.AllOperators.ElementAt(Index).Number = SynchronizedRandom.Next(-Info.NumberRange, (int)BCandOper.Number + 1);
                    }
                    else
                    {
                        Eq.AllOperators.ElementAt(Index).Number = SynchronizedRandom.Next((int)BCandOper.Number + 1, Info.NumberRange);
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
                int AmountToChange = SynchronizedRandom.Next(1, BCand.EquationParts.Count);
                for (int i = 0; i < AmountToChange; i++)
                {
                    int Index = SynchronizedRandom.Next(0, BCand.EquationParts.Count - 1);
                    Cand.EquationParts.ElementAt(Index).Number = SynchronizedRandom.Next(-Info.NumberRange, Info.NumberRange);
                }
                Cand.CalcOffSet();
            }
            return Copys;
        }
    }
}

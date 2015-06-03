using System.Collections.Generic;

namespace Solve_Funktion
{
    public static class SmartCand
    {
        public static void SmartifyCandidates(Equation[] Copys, Equation BCand, Equation OCand, int StartIndex, int Amount)
        {
            List<int> Indexes = CanSmartChangeNumbers(BCand, OCand);
            if (Indexes != null)
            {
                SmartChangeNumbers(Copys, BCand, OCand, Indexes, StartIndex, Amount);
            }
            else
            {
                StupidChangeNumbers(Copys, StartIndex, Amount);
            }
        }
        public static void SmartifyCandidate(Equation ToSmartify, Equation BCand, Equation OCand)
        {
            List<int> Indexes = CanSmartChangeNumbers(BCand, OCand);
            if (Indexes != null)
            {
                SmartChangeNumber(ToSmartify, BCand, OCand, Indexes);
            }
            else
            {
                StupidChangeNumber(ToSmartify);
            }
        }
        public static void SmartifyCandidate(Equation ToSmartify, Equation BCand, Equation OCand, List<int> Indexes)
        {
            if (Indexes != null)
            {
                SmartChangeNumber(ToSmartify, BCand, OCand, Indexes);
            }
            else
            {
                StupidChangeNumber(ToSmartify);
            }
        }

        public static List<int> CanSmartChangeNumbers(Equation BCand, Equation OCand)
        {
            List<int> SmartChangeOperatorIndexes = new List<int>();
            if (BCand.AllOperators.Count != OCand.AllOperators.Count)
                return null;

            for (int i = 0; i < BCand.AllOperators.Count; i++)
            {
                Operator BCandOper = BCand.AllOperators[i];
                Operator OCandOper = OCand.AllOperators[i];
                if (BCandOper.ResultOnRightSide == OCandOper.ResultOnRightSide &&
                    BCandOper.MFunction == OCandOper.MFunction &&
                    BCandOper.UseNumber == OCandOper.UseNumber &&
                    BCandOper.Number != OCandOper.Number &&
                    BCandOper.UseNumber)
                {
                    SmartChangeOperatorIndexes.Add(i);
                }
            }
            return SmartChangeOperatorIndexes;
        }

        private static void SmartChangeNumbers(Equation[] Copys, Equation BCand, Equation OCand, List<int> Indexes, int StartIndex, int Amount)
        {
            for (int i = StartIndex; i < StartIndex + Amount; i++)
            {
                SmartChangeNumber(Copys[i], BCand, OCand, Indexes);
            }
        }
        private static void SmartChangeNumber(Equation Eq, Equation BCand, Equation OCand, List<int> Indexes)
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
                Eq.CalcTotalOffSet();
            }
        }

        private static void StupidChangeNumbers(Equation[] Copys, int StartIndex, int Amount)
        {
            for (int i = StartIndex; i < StartIndex + Amount; i++)
            {
                StupidChangeNumber(Copys[i]);
            }
        }
        private static void StupidChangeNumber(Equation Eq)
        {
            if (Eq.AllOperators.Count == 0)
            {
                return;
            }
            int AmountToChange = SynchronizedRandom.Next(1, Info.MaxChange);
            for (int i = 0; i < AmountToChange; i++)
            {
                int Index = SynchronizedRandom.Next(0, Eq.EquationParts.Count);
                Eq.EquationParts[Index].Number = SynchronizedRandom.NextDouble(Info.NumberRangeMin, Info.NumberRangeMax);
            }
            Eq.CalcTotalOffSet();
        }
    }
}
﻿using System.Collections.Generic;

namespace Solve_Funktion
{
    public static class SmartCand
    {
        public static void SmartifyCandidates(EvolutionInfo EInfo, Equation[] Copys, Equation BCand, Equation OCand, int StartIndex, int Amount)
        {
            int[] Indexes = CanSmartChangeNumbers(BCand, OCand);
            if (Indexes != null)
            {
                SmartChangeNumbers(EInfo, Copys, BCand, OCand, Indexes, StartIndex, Amount);
            }
            else
            {
                StupidChangeNumbers(EInfo, Copys, StartIndex, Amount);
            }
        }
        public static void SmartifyCandidate(EvolutionInfo EInfo, Equation ToSmartify, Equation BCand, Equation OCand)
        {
            int[] Indexes = CanSmartChangeNumbers(BCand, OCand);
            if (Indexes != null)
            {
                SmartChangeNumber(EInfo, ToSmartify, BCand, OCand, Indexes);
            }
            else
            {
                StupidChangeNumber(EInfo, ToSmartify);
            }
        }
        public static void SmartifyCandidate(EvolutionInfo EInfo, Equation ToSmartify, Equation BCand, Equation OCand, int[] Indexes)
        {
            if (Indexes != null)
            {
                SmartChangeNumber(EInfo, ToSmartify, BCand, OCand, Indexes);
            }
            else
            {
                StupidChangeNumber(EInfo, ToSmartify);
            }
        }

        public static int[] CanSmartChangeNumbers(Equation BCand, Equation OCand)
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
                    BCandOper.UseRandomNumber == OCandOper.UseRandomNumber &&
                    BCandOper.randomNumber != OCandOper.randomNumber &&
                    BCandOper.UseRandomNumber)
                {
                    SmartChangeOperatorIndexes.Add(i);
                }
            }
            return SmartChangeOperatorIndexes.ToArray();
        }

        private static void SmartChangeNumbers(EvolutionInfo EInfo, Equation[] Copys, Equation BCand, Equation OCand, int[] Indexes, int StartIndex, int Amount)
        {
            for (int i = StartIndex; i < StartIndex + Amount; i++)
            {
                SmartChangeNumber(EInfo, Copys[i], BCand, OCand, Indexes);
                Copys[i].CalcTotalOffSet();
            }
        }
        private static void SmartChangeNumber(EvolutionInfo EInfo, Equation Eq, Equation BCand, Equation OCand, int[] Indexes)
        {
            foreach (int Index in Indexes)
            {
#if DEBUG
                if (Index >= BCand.AllOperators.Count ||
                    Index < 0 ||
                    Index >= OCand.AllOperators.Count ||
                    Index < 0)
                {
                    System.Diagnostics.Debugger.Break();
                }
#endif
                Operator BCandOper = BCand.AllOperators[Index];
                Operator OCandOper = OCand.AllOperators[Index];
                if (BCandOper.randomNumber[0] > OCandOper.randomNumber[0])
                {
                    Eq.AllOperators[Index].randomNumber = SynchronizedRandom.NextVector(EInfo.NumberRangeMin, (int)BCandOper.randomNumber[0] + 1);
                }
                else
                {
                    Eq.AllOperators[Index].randomNumber = SynchronizedRandom.NextVector((int)BCandOper.randomNumber[0], EInfo.NumberRangeMax);
                }
            }
        }

        private static void StupidChangeNumbers(EvolutionInfo EInfo, Equation[] Copys, int StartIndex, int Amount)
        {
            for (int i = StartIndex; i < StartIndex + Amount; i++)
            {
                StupidChangeNumber(EInfo, Copys[i]);
                Copys[i].CalcTotalOffSet();
            }
        }
        private static void StupidChangeNumber(EvolutionInfo EInfo, Equation Eq)
        {
            if (Eq.AllOperators.Count == 0)
            {
                return;
            }
            int AmountToChange = SynchronizedRandom.Next(1, EInfo.MaxChange);
            for (int i = 0; i < AmountToChange; i++)
            {
                int Index = SynchronizedRandom.Next(0, Eq.EquationParts.Count);
                Eq.EquationParts[Index].randomNumber = SynchronizedRandom.NextVector(EInfo.NumberRangeMin, EInfo.NumberRangeMax);
            }
        }
    }
}
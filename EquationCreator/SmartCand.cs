﻿using System;
using System.Collections.Generic;

namespace EquationCreator
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
            SmartifyCandidate(EInfo, ToSmartify, BCand, OCand, Indexes);
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
            if (BCand.NumberOfAllOperators != OCand.NumberOfAllOperators)
                return null;

            for (int i = 0; i < BCand.AllOperators.Length; i++)
            {
                Operator BCandOper = BCand.AllOperators[i];
                Operator OCandOper = OCand.AllOperators[i];
                if (BCandOper != null &&
                    OCandOper != null &&
                    BCandOper.ResultOnRightSide == OCandOper.ResultOnRightSide &&
                    BCandOper.MFunction == OCandOper.MFunction &&
                    BCandOper.UseRandomNumber == OCandOper.UseRandomNumber &&
                    BCandOper.RandomNumber != OCandOper.RandomNumber &&
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
                if (Index >= BCand.AllOperators.Length ||
                    Index < 0 ||
                    Index >= OCand.AllOperators.Length ||
                    Index < 0)
                {
                    System.Diagnostics.Debugger.Break();
                }
#endif
                Operator BCandOper = BCand.AllOperators[Index];
                Operator OCandOper = OCand.AllOperators[Index];
                if (BCandOper.RandomNumber > OCandOper.RandomNumber)
                {
                    Eq.AllOperators[Index].RandomNumber = Eq.Randomizer.Next(EInfo.NumberRangeMin, (int)BCandOper.RandomNumber + 1);
                }
                else
                {
                    Eq.AllOperators[Index].RandomNumber = Eq.Randomizer.Next((int)BCandOper.RandomNumber, EInfo.NumberRangeMax);
                }
                Eq.AllOperators[Index].OperatorChanged();
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
            if (Eq.NumberOfAllOperators == 0)
            {
                return;
            }
            int AmountToChange = Eq.Randomizer.Next(1, (int)Math.Max(2, EInfo.MaxChange * Eq.NumberOfAllOperators));
            for (int i = 0; i < AmountToChange; i++)
            {
                int Index = Eq.GetRandomOperatorIndexFromAllOperators();
                Eq.AllOperators[Index].RandomNumber = Eq.Randomizer.Next(EInfo.NumberRangeMin, EInfo.NumberRangeMax);
                Eq.AllOperators[Index].OperatorChanged();
            }
        }
    }
}
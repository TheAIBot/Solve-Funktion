using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;

namespace Solve_Funktion
{
    public static class EvolveCand
    {
        public static void EvolveCandidates(EvolutionInfo EInfo, Equation[] Copys, int StartIndex, int Amount)
        {
            for (int i = StartIndex; i < StartIndex + Amount; i++)
            {
                EvolveCandidate(EInfo, Copys[i]);
                Copys[i].CalcTotalOffSet();
            }
        }

        public static void EvolveCandidate(EvolutionInfo EInfo, Equation Cand)
        {
            int AmountToChange = SynchronizedRandom.Next(1, EInfo.MaxChange);
            while (AmountToChange > 0)
            {
                int ToDo = SynchronizedRandom.Next(0, 3);
                switch (ToDo)
                {
                    case 0:
                        AmountToChange -= InsertOPS(Cand);
                        break;
                    case 1:
                        AmountToChange -= RemoveOPS(Cand, EInfo.MaxChange);
                        break;
                    case 2:
                        AmountToChange -= ChangeOPS(Cand, EInfo.MaxChange);
                        break;
                }
#if DEBUG
                if (Cand.OperatorsLeft < 0)
                {
                    System.Diagnostics.Debugger.Break();
                }
#endif
            }
        }

        private static int ChangeOPS(Equation Cand, int MaxChange)
        {
            if (Cand.OperatorsLeft < MaxChange)
            {
                return Cand.ChangeRandomOperator(MaxChange);
            }
            return 0;
        }

        private static int RemoveOPS(Equation Cand, int MaxChange)
        {
            if (Cand.OperatorsLeft < MaxChange)
            {
                return Cand.RemoveRandomOperator(MaxChange);
            }
            return 0;
        }

        private static int InsertOPS(Equation Cand)
        {
            if (Cand.OperatorsLeft > 0)
            {
                int WhereToAdd = SynchronizedRandom.Next(0, Cand.SortedOperators.Count);
                List<Operator> LLOper = Cand.SortedOperators[WhereToAdd];
                int WhereToAddOP = SynchronizedRandom.Next(0, LLOper.Count);
                //there has to be an operator in the list because that's the only way to get the holder of the list
                if (Cand.SortedOperators[WhereToAdd].Count > 0)
                {
                    Operator Oper = Cand.SortedOperators[WhereToAdd][0];
                    OperatorHolder Holder = Oper.Holder;
                    Operator ToAdd = Cand.OPStorage.Pop();
                    ToAdd.MakeRandom(LLOper, Holder, WhereToAddOP);
                    Oper.OperatorChanged();
                    return ToAdd.GetOperatorCount();
                }
            }
            return 0;
        }
    }
}
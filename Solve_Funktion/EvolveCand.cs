using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using System.Numerics;

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
            for (int i = 0; i < AmountToChange; i++)
            {
                int ToDo = SynchronizedRandom.Next(0, 3);
                switch (ToDo)
                {
                    case 0:
                        InsertOPS(Cand);
                        break;
                    case 1:
                        RemoveOPS(Cand, EInfo.MaxChange);
                        break;
                    case 2:
                        ChangeOPS(Cand, EInfo.MaxChange);
                        break;
                }
#if DEBUG
                if (Cand.OperatorsLeft < 0)
                {
                    System.Diagnostics.Debugger.Break();
                }
# endif
            }
        }

        private static void ChangeOPS(Equation Cand, int MaxChange)
        {
            if (Cand.OperatorsLeft < MaxChange)
            {
                Cand.ChangeRandomOperator();
            }
        }

        private static void RemoveOPS(Equation Cand, int MaxChange)
        {
            if (Cand.OperatorsLeft < MaxChange)
            {
                Cand.RemoveRandomOperator();
            }
        }

        private static void InsertOPS(Equation Cand)
        {
            if (Cand.OperatorsLeft > 0)
            {
                int WhereToAdd = SynchronizedRandom.Next(0, Cand.SortedOperators.Count);
                List<Operator> LLOper = Cand.SortedOperators[WhereToAdd];
                int WhereToAddOP = SynchronizedRandom.Next(0, LLOper.Count);
                Operator ToAdd = Cand.OPStorage.Pop();
                ToAdd.MakeRandom(LLOper, WhereToAddOP);
            }
        }
    }
}
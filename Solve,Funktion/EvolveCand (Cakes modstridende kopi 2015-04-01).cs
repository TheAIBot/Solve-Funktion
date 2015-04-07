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
        public static List<Equation> EvolveCandidates(List<Equation> Copys)
        {
            foreach (Equation Cand in Copys)
            {
                EvolveCandidate(Cand);
            }
            return Copys;
        }

        public static void EvolveCandidate(Equation Cand)
        {
            int AmountToChange = SynchronizedRandom.Next(1, Info.MaxChange);
            for (int i = 0; i < AmountToChange; i++)
            {
                int ToDo = SynchronizedRandom.Next(0, 3);
                switch (ToDo)
                {
                    case 0:
                        InsertOPS(Cand);
                        break;
                    case 1:
                        RemoveOPS(Cand);
                        break;
                    case 2:
                        ChangeOPS(Cand);
                        break;
                }
#if DEBUG
                if (Cand.OperatorsLeft < 0)
                {
                    System.Diagnostics.Debugger.Break();
                }
# endif
            }
            Cand.CalcOffSet();
        }

        private static void ChangeOPS(Equation Cand)
        {
            if (Cand.OperatorsLeft < Info.MaxSize)
            {
                Cand.ChangeRandomOperator();
            }
        }

        private static void RemoveOPS(Equation Cand)
        {
            if (Cand.OperatorsLeft < Info.MaxSize)
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
                ToAdd.MakeRandom(Cand, LLOper);
                //Cand.SortedOperators[WhereToAdd].Insert(WhereToAddOP, ToAdd);
            }
        }
    }
}

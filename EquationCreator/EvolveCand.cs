using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;

namespace EquationCreator
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
            int AmountToChange = Cand.Randomizer.Next(1, (int)Math.Max(2, EInfo.MaxChange * Cand.NumberOfAllOperators));
            while (AmountToChange > 0 && Cand.OperatorsLeft < Cand.EInfo.MaxSize)
            {
                int ToDo = Cand.Randomizer.Next(0, 3);
                switch (ToDo)
                {
                    case 0:
                        AmountToChange -= InsertOPS(Cand);
                        break;
                    case 1:
                        AmountToChange -= RemoveOPS(Cand, AmountToChange);
                        break;
                    case 2:
                        AmountToChange -= ChangeOPS(Cand, AmountToChange);
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
            if (Cand.OperatorsLeft < Cand.EInfo.MaxSize)
            {
                return Cand.ChangeRandomOperator(MaxChange);
            }
            return 0;
        }

        private static int RemoveOPS(Equation Cand, int MaxChange)
        {
            if (Cand.OperatorsLeft < Cand.EInfo.MaxSize && Cand.NumberOfAllOperators > 1)
            {
                return Cand.RemoveRandomOperator(MaxChange);
            }
            return 0;
        }

        private static int InsertOPS(Equation Cand)
        {
            if (Cand.OperatorsLeft > 0 && Cand.OperatorsLeft < Cand.EInfo.MaxSize)
            {
                return Cand.InsertOperator(Cand);
            }
            return 0;
        }
    }
}
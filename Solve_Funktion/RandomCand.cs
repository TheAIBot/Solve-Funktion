using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Diagnostics;

namespace Solve_Funktion
{
    public static class RandomCand
    {
        public static void MakeRandomEquations(Equation[] Candidates, int StartIndex, int Amount)
        {
            for (int i = StartIndex; i < StartIndex + Amount; i++)
            {
                MakeRandomEquation(Candidates[i]);
            }
        }

        public static void MakeRandomEquation(Equation Cand)
        {
            Cand.MakeRandom();
            Cand.CalcTotalOffSet();
        }
    }
}
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

        public static List<Equation> RandomCandidates(double Amount, ref Stack<Equation> CandidateStorage)
        {
            List<Equation> AllCand = new List<Equation>((int)Amount);
            for (int i = 0; i < Amount; i++)
            {
                Equation Cand = CandidateStorage.Pop();
                Cand.MakeRandom();
                Cand.CalcOffSet();
                AllCand.Add(Cand);
            }
            return AllCand;
        }
    }
}

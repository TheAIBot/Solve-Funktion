using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solve_Funktion
{
    class EdgeEvolution : BatchSpecieEvolutionMethod
    {
        protected override void ChoseBestCandidate(Equation NBCand, ref Equation OCand, ref int StuckCounter)
        {
            {
                ResetSingle(OCand);
                ResetSingle(BestCandidate);
                OCand = BestCandidate.MakeClone(OCand);
                BestCandidate = NBCand.MakeClone(BestCandidate);
            }
        }

        protected override void GetValidCandidateIndexes(Equation[] Cands, int[] Indexes)
        {
            int CurrentIndex = 0;
            for (int i = 0; i < Cands.Length; i++)
            {
                if (Tools.IsANumber(Cands[i].OffSet) && !Tools.IsEquationsTheSame(BestCandidate, Cands[i]) && Cands[i].EquationParts.Count > 0)
                {
                    Indexes[CurrentIndex] = i;
                    CurrentIndex++;
                }
            }
            if (CurrentIndex + 1 < Cands.Length)
            {
                Indexes[CurrentIndex] = -1;
                return;
            }
        }
    }
}

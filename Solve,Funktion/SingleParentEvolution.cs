using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;

namespace Solve_Funktion
{
    public class SingleParentEvolution : BatchSpecieEvolutionMethod
    {
        protected override void ChoseBestCandidate(Equation NBCand, ref Equation OCand, ref int StuckCounter)
        {
            if (BestCandidate.OffSet == NBCand.OffSet && NBCand.OperatorsLeft <= BestCandidate.OperatorsLeft
                || BestCandidate.OffSet < NBCand.OffSet)
            {
                StuckCounter++;
            }
            else
            {
                StuckCounter = 0;
                ResetSingle(OCand);
                ResetSingle(BestCandidate);
                OCand = BestCandidate.MakeClone(OCand);
                BestCandidate = NBCand.MakeClone(BestCandidate);
            }
        }
    }
}
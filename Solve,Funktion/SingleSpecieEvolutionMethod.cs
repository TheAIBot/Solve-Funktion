﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solve_Funktion
{
    public class SingleSpecieEvolutionMethod : Genome
    {
        public override Genome EvolveSolution()
        {
            StartFinding();
            BestCandidate = new Equation(Info.MaxSize);
            do
            {
                ResetSingle(BestCandidate);
                RandomCand.MakeRandomEquation(BestCandidate);
            } while (!Tools.IsANumber(BestCandidate.OffSet));
            Equation EvolvedEquation = new Equation(Info.MaxSize) { OffSet = Double.NaN };
            Equation OldEquation = new Equation(Info.MaxSize) { OffSet = Double.NaN };
            int StuckCounter = 0;
            bool BestCandEvolved = false;
            do
            {
                BestCandEvolved = GetNextGen(EvolvedEquation, OldEquation);

                StuckCounter = SetStuckCounter(StuckCounter, BestCandEvolved);

                UpdateInfo();
            } while (BestCandidate.OffSet != 0 && StuckCounter < Info.MaxStuckGens);
            //} while (BestCandidate.OffSet != 0 && StuckCounter < Info.MaxStuckGens || BestCandidate.OperatorsLeft != 0);
            return this;
        }

        protected virtual int SetStuckCounter(int StuckCounter, bool BestCandEvolved)
        {
            return (!BestCandEvolved) ? StuckCounter++ : StuckCounter = 0;
        }

        protected virtual bool GetNextGen(Equation EvolvedEquation, Equation OldEquation)
        {
            bool BestCandEvolved = false;
            Equation BestEvolvedEquation = BestCandidate.MakeClone(new Equation(Info.MaxSize));
            for (double i = 0; i < Info.CandidatesPerGen * Info.EvolvedCandidatesPerGen; i++)
            {
                BestCandidate.MakeClone(EvolvedEquation);
                EvolveCand.EvolveCandidate(EvolvedEquation);
                bool EvolvedToBetter = ChangeIfBetter(EvolvedEquation, OldEquation, BestEvolvedEquation);
                BestCandEvolved = (EvolvedToBetter) ? true : BestCandEvolved;
                ResetSingle(EvolvedEquation);
            }
            List<int> Indexes = SmartCand.CanSmartChangeNumbers(BestCandidate, OldEquation);
            for (double i = 0; i < Info.CandidatesPerGen * Info.SmartCandidatesPerGen; i++)
            {
                BestCandidate.MakeClone(EvolvedEquation);
                SmartCand.SmartifyCandidate(EvolvedEquation, BestCandidate, OldEquation, Indexes);
                bool EvolvedToBetter = ChangeIfBetter(EvolvedEquation, OldEquation, BestEvolvedEquation);
                BestCandEvolved = (EvolvedToBetter) ? true : BestCandEvolved;
                ResetSingle(EvolvedEquation);
            }
            for (double i = 0; i < Info.CandidatesPerGen * Info.RandomCandidatesPerGen; i++)
            {
                RandomCand.MakeRandomEquation(EvolvedEquation);
                bool EvolvedToBetter = ChangeIfBetter(EvolvedEquation, OldEquation, BestEvolvedEquation);
                BestCandEvolved = (EvolvedToBetter) ? true : BestCandEvolved;
                ResetSingle(EvolvedEquation);
            }
            if (BestCandEvolved)
            {
                ResetSingle(BestCandidate);
                BestEvolvedEquation.MakeClone(BestCandidate);
            }
            return BestCandEvolved;
        }

        protected virtual bool ChangeIfBetter(Equation Eq, Equation OldEquation, Equation BestEvolvedCand)
        {
            if (Tools.IsANumber(Eq.OffSet))
            {
                if (Eq.OffSet < BestEvolvedCand.OffSet || Eq.OffSet == BestEvolvedCand.OffSet && Eq.OperatorsLeft > BestEvolvedCand.OperatorsLeft)
                //if (Eq.OffSet < BestEvolvedCand.OffSet)
                //if (Eq.OffSet < BestEvolvedCand.OffSet || Eq.OffSet == BestEvolvedCand.OffSet && Eq.OperatorsLeft < BestEvolvedCand.OperatorsLeft)
                {
                    ResetSingle(OldEquation);
                    BestEvolvedCand.MakeClone(OldEquation);

                    ResetSingle(BestEvolvedCand);
                    Eq.MakeClone(BestEvolvedCand);
                    return true;
                }
            }
            return false;
        }
    }
}
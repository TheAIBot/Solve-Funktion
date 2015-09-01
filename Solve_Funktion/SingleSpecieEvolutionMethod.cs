using System;
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
            BestCandidate = new Equation(EInfo);
            do
            {
                ResetSingle(BestCandidate);
                RandomCand.MakeRandomEquation(BestCandidate);
                BestCandidate.CalcTotalOffSet();
            } while (!Tools.IsANumber(BestCandidate.OffSet));
            Equation EvolvedEquation = new Equation(EInfo) { OffSet = Double.NaN };
            Equation OldEquation = new Equation(EInfo) { OffSet = Double.NaN };
            bool BestCandEvolved = false;
            _toCalc = EInfo.GoalLength;
            while (_toCalc <= EInfo.GoalLength)
            {
                int StuckCounter = 0;
                do
                {
                    BestCandEvolved = GetNextGen(EvolvedEquation, OldEquation, _toCalc);
                    StuckCounter = SetStuckCounter(StuckCounter, BestCandEvolved);
                    UpdateInfo();
                    //} while (StuckCounter <= EInfo.MaxStuckGens && BestCandidate.OffSet != 0);
                } while (StuckCounter <= EInfo.MaxStuckGens);
                _toCalc++;
                //BestCandidate.OffSet = double.MaxValue;
            }
            return this;
        }

        protected virtual int SetStuckCounter(int StuckCounter, bool BestCandEvolved)
        {
            return (!BestCandEvolved) ? ++StuckCounter : StuckCounter = 0;
        }

        protected virtual bool GetNextGen(Equation EvolvedEquation, Equation OldEquation, int toCalc)
        {
            bool BestCandEvolved = false;
            Equation BestEvolvedEquation = BestCandidate.MakeClone(new Equation(EInfo));
            for (double i = 0; i < EInfo.CandidatesPerGen * EInfo.EvolvedCandidatesPerGen; i++)
            {
                BestCandidate.MakeClone(EvolvedEquation);
                EvolveCand.EvolveCandidate(EInfo, EvolvedEquation);
                EvolvedEquation.CalcPartialOffSet(toCalc);
                bool EvolvedToBetter = ChangeIfBetter(EvolvedEquation, OldEquation, BestEvolvedEquation);
                BestCandEvolved = (EvolvedToBetter) ? true : BestCandEvolved;
                ResetSingle(EvolvedEquation);
            }
            List<int> Indexes = SmartCand.CanSmartChangeNumbers(BestCandidate, OldEquation);
            for (double i = 0; i < EInfo.CandidatesPerGen * EInfo.SmartCandidatesPerGen; i++)
            {
                BestCandidate.MakeClone(EvolvedEquation);
                SmartCand.SmartifyCandidate(EInfo, EvolvedEquation, BestCandidate, OldEquation, Indexes);
                EvolvedEquation.CalcPartialOffSet(toCalc);
                bool EvolvedToBetter = ChangeIfBetter(EvolvedEquation, OldEquation, BestEvolvedEquation);
                BestCandEvolved = (EvolvedToBetter) ? true : BestCandEvolved;
                ResetSingle(EvolvedEquation);
            }
            for (double i = 0; i < EInfo.CandidatesPerGen * EInfo.RandomCandidatesPerGen; i++)
            {
                RandomCand.MakeRandomEquation(EvolvedEquation);
                EvolvedEquation.CalcPartialOffSet(toCalc);
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
                if (Eq.OffSet < BestEvolvedCand.OffSet &&
                    Eq._toCalc >= BestEvolvedCand._toCalc ||
                    Eq.OffSet == BestEvolvedCand.OffSet &&
                    Eq.AllOperators.Count < BestEvolvedCand.AllOperators.Count &&
                    Eq._toCalc >= BestEvolvedCand._toCalc)
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

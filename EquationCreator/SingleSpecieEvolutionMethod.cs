using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EquationCreator
{
    public class SingleSpecieEvolutionMethod : SingleGenome
    {

        public override Genome EvolveSpecie()
        {
            StartFinding();
            BestCandidate = new Equation(EInfo, Randomizer);
            do
            {
                ResetSingle(BestCandidate);
                RandomCand.MakeRandomEquation(BestCandidate);
                BestCandidate.CalcTotalOffSet();
            } while (!Tools.IsANumber(BestCandidate.OffSet));
            Equation EvolvedEquation = new Equation(EInfo, Randomizer) { OffSet = float.NaN };
            Equation OldEquation = new Equation(EInfo, Randomizer) { OffSet = float.NaN };
            bool BestCandEvolved = false;
            _toCalc = EInfo.coordInfo.expectedResults.Length;
            while (_toCalc <= EInfo.coordInfo.expectedResults.Length)
            {
                int StuckCounter = 0;
                do
                {
                    BestCandEvolved = GetNextGen(EvolvedEquation, OldEquation, _toCalc);
                    StuckCounter = SetStuckCounter(StuckCounter, BestCandEvolved);
                    UpdateInfo();
                    SpecEnviroment.CheckBestCandidate(this.SpecInfo.GetCopy());
                    //} while (StuckCounter <= EInfo.MaxStuckGens && BestCandidate.OffSet != 0);
                } while (StuckCounter <= EInfo.MaxStuckGens);
                break;
                //_toCalc++;
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
            Equation BestEvolvedEquation = BestCandidate.MakeClone(new Equation(EInfo, Randomizer));
            Debug.WriteLine(BestCandidate.CreateFunction());
            //int simplestCount = 0;
            for (double i = 0; i < EInfo.CandidatesPerGen * EInfo.EvolvedCandidatesPerGen; i++)
            {
                BestCandidate.MakeClone(EvolvedEquation);
                EvolveCand.EvolveCandidate(EInfo, EvolvedEquation);
                EvolvedEquation.CalcPartialOffSet(toCalc);
                //if (EvolvedEquation.CreateFunction() == "f(x) = x")
                //{
                //    simplestCount++;
                //}
                //Debug.WriteLine(EvolvedEquation.CreateFunction());
                //Thread.Sleep(500);
                bool EvolvedToBetter = ChangeIfBetter(EvolvedEquation, OldEquation, BestEvolvedEquation);
                BestCandEvolved = (EvolvedToBetter) ? true : BestCandEvolved;
                ResetSingle(EvolvedEquation);
            }
            //Debug.WriteLine(simplestCount);
            //Thread.Sleep(500);
            int[] Indexes = SmartCand.CanSmartChangeNumbers(BestCandidate, OldEquation);
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
                ResetSingle(OldEquation);
                BestEvolvedEquation.MakeClone(OldEquation);

                ResetSingle(BestCandidate);
                BestEvolvedEquation.MakeClone(BestCandidate);
                BestCandidate.CompressEquation();
            }
            return BestCandEvolved;
        }

        protected bool ChangeIfBetter(Equation Eq, Equation OldEquation, Equation BestEvolvedCand)
        {
            if (Tools.IsANumber(Eq.OffSet))
            {
                if (Eq.OffSet < BestEvolvedCand.OffSet &&
                    Eq._toCalc >= BestEvolvedCand._toCalc ||
                    Eq.OffSet == BestEvolvedCand.OffSet &&
                    Eq.NumberOfAllOperators < BestEvolvedCand.NumberOfAllOperators &&
                    Eq._toCalc >= BestEvolvedCand._toCalc)
                //if (Eq.OffSet < BestEvolvedCand.OffSet)
                //if (Eq.OffSet < BestEvolvedCand.OffSet || Eq.OffSet == BestEvolvedCand.OffSet && Eq.OperatorsLeft < BestEvolvedCand.OperatorsLeft)
                {
                    ResetSingle(BestEvolvedCand);
                    BestEvolvedCand = Eq.MakeClone(BestEvolvedCand);
                    return true;
                }
            }
            return false;
        }

    }
}

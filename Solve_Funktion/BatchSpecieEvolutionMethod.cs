using System;
using System.Linq;

namespace Solve_Funktion
{
    public abstract class BatchSpecieEvolutionMethod : Genome
    {
        protected Equation[] Equations;

        public override void Startup(SpecieEnviromentBase Env, GeneralInfo ginfo, EvolutionInfo einfo)
        {
            base.Startup(Env, ginfo, einfo);
            CreateStorage();
        }

        public override Genome EvolveSolution()
        {
            StartFinding();
            bool FirstGen = true;
            int StuckCounter = 0;
            BestCandidate = new Equation(EInfo) { OffSet = Double.NaN };
            Equation OldCandidate = new Equation(EInfo) { OffSet = Double.NaN };
            int[] ValidCandidateIndexes = Enumerable.Range(0, (int)EInfo.CandidatesPerGen).ToArray();

            do
            {
                // the BestCandidate and OldCandidate has not been created yet
                // so the first generation has to be created specificly to create those two
                if (FirstGen)
                    GetFirstGen();
                else
                    GetNextGen(Equations, BestCandidate, OldCandidate);

                GetValidCandidateIndexes(Equations, ValidCandidateIndexes);
                if (ValidCandidateIndexes[0] != -1)
                {
                    Equation NewGenBestCandidate = GetClosest(Equations, ValidCandidateIndexes);
                    if (FirstGen)
                        FirstGetBestAndOldCand(NewGenBestCandidate, ref OldCandidate);
                    else
                        ChoseBestCandidate(NewGenBestCandidate, ref OldCandidate, ref StuckCounter);

                    FirstGen = false;
                }
                else
                {
                    StuckCounter++;
                }
                UpdateInfo();
                ResetEquations(Equations);
            } while (BestCandidate.OffSet != 0 && StuckCounter < EInfo.MaxStuckGens);
            if (BestCandidate.OffSet == 0)
            {
                Simplify(OldCandidate);
            }
            ResetSingle(OldCandidate);
            ResetSingle(BestCandidate);
            return this;
        }

        protected virtual void CreateStorage()
        {
            Equations = new Equation[(int)EInfo.CandidatesPerGen];
            for (int i = 0; i < (int)EInfo.CandidatesPerGen; i++)
            {
                Equations[i] = new Equation(EInfo);
            }
        }

        protected virtual void GetFirstGen()
        {
            RandomCand.MakeRandomEquations(Equations, 0, Equations.Length);
        }

        protected virtual void GetNextGen(Equation[] Cands, Equation BestCand, Equation OldCand)
        {
            int EvolveStart = 0;
            int EvolveCount = (int)(EInfo.CandidatesPerGen * EInfo.EvolvedCandidatesPerGen);
            MakeCopys(BestCand, Cands, EvolveStart, EvolveCount);
            EvolveCand.EvolveCandidates(EInfo, Equations, EvolveStart, EvolveCount);

            int SmartStart = EvolveStart + EvolveCount;
            int SmartCount = (int)(EInfo.CandidatesPerGen * EInfo.SmartCandidatesPerGen);
            MakeCopys(BestCand, Cands, SmartStart, SmartCount);
            SmartCand.SmartifyCandidates(EInfo, Equations, BestCand, OldCand, SmartStart, SmartCount);

            int RandomStart = SmartStart + SmartCount;
            int RandomCount = (int)(EInfo.CandidatesPerGen * EInfo.RandomCandidatesPerGen);
            RandomCand.MakeRandomEquations(Equations, RandomStart, RandomCount);
        }

        protected virtual void GetValidCandidateIndexes(Equation[] Cands, int[] Indexes)
        {
            int CurrentIndex = 0;
            for (int i = 0; i < Cands.Length; i++)
            {
                if (Tools.IsANumber(Cands[i].OffSet) && Cands[i].EquationParts.Count > 0)
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

        /// <summary>
        /// Finds the equation with the least offset and the least operators used
        /// </summary>
        /// <param name="Cands"></param>
        /// <param name="Indexes"></param>
        /// <returns></returns>
        protected virtual Equation GetClosest(Equation[] Cands, int[] Indexes)
        {
            Equation BestOffspring = Cands[Indexes[0]];
            for (int i = 1; i < Indexes.Length && Indexes[i] != -1; i++)
            {
                if (Cands[Indexes[i]].OffSet < BestOffspring.OffSet || Cands[Indexes[i]].OffSet == BestOffspring.OffSet && Cands[Indexes[i]].OperatorsLeft > BestOffspring.OperatorsLeft)
                {
                    BestOffspring = Cands[Indexes[i]];
                }
            }
            return BestOffspring;
        }

        protected virtual void FirstGetBestAndOldCand(Equation NBCand, ref Equation OCand)
        {
            OCand = NBCand.MakeClone(OCand); // why does this not make an error?
            BestCandidate = NBCand.MakeClone(BestCandidate);
            ResetSingle(NBCand);
        }

        protected abstract void ChoseBestCandidate(Equation NBCand, ref Equation OCand, ref int StuckCounter);

        protected virtual void Simplify(Equation OldCandidate)
        {
            int StuckCounter = 0;
            int[] ValidCandidateIndexes = Enumerable.Range(0, (int)EInfo.CandidatesPerGen).ToArray();

            do
            {
                GetNextGen(Equations, BestCandidate, OldCandidate);
                GetValidCandidateIndexes(Equations, ValidCandidateIndexes);
                if (ValidCandidateIndexes[0] != -1)
                {
                    Equation NewGenBestCandidate = GetClosest(Equations, ValidCandidateIndexes);
                    ChoseBestCandidate(NewGenBestCandidate, ref OldCandidate, ref StuckCounter);
                    UpdateInfo();
                }
                ResetEquations(Equations);
            } while (StuckCounter != EInfo.MaxStuckGens);
        }

        protected void MakeCopys(Equation BestCand, Equation[] Cands, int Start, int CopyAmount)
        {
            for (int i = Start; i < Start + CopyAmount; i++)
            {
                BestCand.MakeClone(Cands[i]);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solve_Funktion
{
    class EdgeEvolution : Genome
    {
        public EdgeEvolution(MainWindow mwin)
        {
            MWin = mwin;
        }

        public override void CreateStorages()
        {
            for (int i = 0; i < Info.CandidatesPerGen + 2; i++)
            {
                EquationStorage.Push(new Equation(ref OPStorage));
            }
            for (int i = 0; i < (Info.CandidatesPerGen + 2) * Info.MaxSize; i++)
            {
                OPStorage.Push(new Operator());
            }
        }

        public override Genome EvolveSolution()
        {
            StartFinding();
            bool FirstGen = true;
            int StuckCounter = 0;
            BestCandidate = new Equation(ref OPStorage) { OffSet = double.NaN };
            Equation OldCandidate = new Equation(ref OPStorage) { OffSet = double.NaN };
            List<Equation> NewGeneration = new List<Equation>();

            do 
            {
                // the BestCandidate and OldCandidate has not been created yet
                // so the first generation has to be created specificly to create those two
                NewGeneration = (FirstGen) ? GetFirstGen() :
                                             GetNextGen(BestCandidate, OldCandidate);
                NewGeneration = RemoveInvalidCandidates(NewGeneration);
                if (NewGeneration.Count > 0)
                {
                    Equation NewGenBestCandidate = GetClosest(NewGeneration);
                    StoreCandidatesAndOperators(NewGeneration, NewGenBestCandidate);
                    BestCandidate = (FirstGen) ? FirstGetBestAndOldCand(NewGenBestCandidate, ref OldCandidate) :
                                                 ChoseBestCandidate(BestCandidate, NewGenBestCandidate, ref OldCandidate, ref StuckCounter);
                    FirstGen = false;
                    UpdateInfo();
                }
                else
                {
                    StuckCounter++;
                    StoreCandidatesAndOperators(NewGeneration, null);
                }
            } while (BestCandidate.OffSet != 0 && StuckCounter < Info.MaxStuckGens);
            if (BestCandidate.OffSet == 0)
            {
                Simplify(OldCandidate);
            }
            StoreSingle(OldCandidate);
            StoreSingle(BestCandidate);
            return this;
        }

        private void Simplify(Equation OldCandidate)
        {
            int StuckCounter = 0; 
            List<Equation> NewGeneration = new List<Equation>();

            do
            {
                NewGeneration = GetNextGen(BestCandidate, OldCandidate);
                NewGeneration = RemoveInvalidCandidates(NewGeneration);
                if (NewGeneration.Count > 0)
                {
                    Equation NewGenBestCandidate = GetClosest(NewGeneration);
                    StoreCandidatesAndOperators(NewGeneration, NewGenBestCandidate);
                    BestCandidate = ChoseBestCandidate(BestCandidate, NewGenBestCandidate, ref OldCandidate, ref StuckCounter);
                    UpdateInfo();
                }
            } while (StuckCounter != Info.MaxStuckGens);
        }

        private List<Equation> GetFirstGen()
        {
            return RandomCand.RandomCandidates(Info.CandidatesPerGen, ref EquationStorage);
        }

        private Equation GetClosest(List<Equation> NextGen)
        {
            return NextGen.OrderBy(x => x.OffSet)
                  .ThenByDescending(x => x.OperatorsLeft)
                  .First();
        }

        public override List<Equation> RemoveInvalidCandidates(List<Equation> Cands)
        {
            var InvalidCandidates = new List<Equation>();
            var ValidCandidates = new List<Equation>();
            foreach (Equation Cand in Cands)
            {
                if (Tools.IsANumber(Cand.OffSet) && !Tools.IsEquationsTheSame(BestCandidate, Cand))
                {
                    ValidCandidates.Add(Cand);
                }
                else
                {
                    InvalidCandidates.Add(Cand);
                }
            }
            StoreCandidatesAndOperators(InvalidCandidates, null);
            return ValidCandidates;
        }

        private Equation FirstGetBestAndOldCand(Equation NBCand, ref Equation OCand)
        {
            OCand = NBCand.GetClone(EquationStorage.Pop());
            BestCandidate = NBCand.GetClone(EquationStorage.Pop());
            StoreSingle(NBCand);
            return BestCandidate;
        }

        private Equation ChoseBestCandidate(Equation BCand, Equation NBCand, ref Equation OCand, ref int StuckCounter)
        {
            StoreSingle(OCand);
            OCand = BCand;
            return NBCand;
        }
    }
}

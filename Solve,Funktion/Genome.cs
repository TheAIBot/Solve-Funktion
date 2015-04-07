using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Solve_Funktion
{
    public abstract class Genome
    {
        public MainWindow MWin;
        public Equation BestCandidate;
        public SpeciesInfo SpecInfo;
        public SpecieInfoControl SpecInfoControl;
        public GeneralInformationControl GInfoControl;
        public Stack<Equation> EquationStorage;
        public Stack<Operator> OPStorage;
        private string ShouldBeNumbers = String.Empty;
        private bool IsUpdatedInfoSet = false;

        public virtual void CreateStorages()
        {
            EquationStorage = new Stack<Equation>((int)(Info.CandidatesPerGen + 2));
            OPStorage = new Stack<Operator>(((int)(Info.CandidatesPerGen + 2) * Info.MaxSize));
            for (int i = 0; i < Info.CandidatesPerGen + 2; i++)
            {
                EquationStorage.Push(new Equation(ref OPStorage));
            }
            for (int i = 0; i < (Info.CandidatesPerGen + 2) * Info.MaxSize; i++)
            {
                OPStorage.Push(new Operator());
            }
        }

        public abstract Genome EvolveSolution();

        public virtual List<Equation> RemoveInvalidCandidates(List<Equation> Cands)
        {
            var InvalidCandidates = new List<Equation>((int)Info.CandidatesPerGen);
            var ValidCandidates = new List<Equation>((int)Info.CandidatesPerGen);
            foreach (Equation Cand in Cands)
            {
                if (Tools.IsANumber(Cand.OffSet) && Cand.EquationParts.Count > 0)
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

        public virtual void StartFinding()
        {
            SpecInfo = new SpeciesInfo();
            SpecInfoControl.InsertInfo(SpecInfo);
            SynchronizedRandom.CreateRandom();
            GInfoControl.GInfo.TotalSpecies++; // threading problem here but i am too lazy to fix it
        }

        public virtual void UpdateInfo()
        {
            InitializeUpdateInfo();
            SpecInfo.FunctionText = BestCandidate.CreateFunction();
            SpecInfo.Offset = BestCandidate.OffSet;
            SpecInfo.ResultText = String.Join(", ", BestCandidate.GetFunctionResults());
            SpecInfo.Attempts += Info.CandidatesPerGen;
            SpecInfo.Generation++;
            GInfoControl.GInfo.TotalAttempts += (long)Info.CandidatesPerGen;
            MWin.UpdateBestInfo();
        }

        private void InitializeUpdateInfo()
        {
            if (!IsUpdatedInfoSet)
            {
                List<string> SeqText = (from x in Info.Seq
                                        select x.Y.ToString(Info.SRounding)).ToList();
                ShouldBeNumbers = String.Join(", ", SeqText);
                SpecInfo.SequenceText = ShouldBeNumbers;
                IsUpdatedInfoSet = true;
            }
        }

        public virtual List<Equation> GetNextGen(Equation BestCand, Equation OldCand)
        {
            List<Equation> NextGen = new List<Equation>((int)Info.CandidatesPerGen);

            List<Equation> EvolvedCopys = CreateCopys(BestCand, Info.CandidatesPerGen * Info.EvolvedCandidatesPerGen);
            List<Equation> SmartCopys = CreateCopys(BestCand, Info.CandidatesPerGen * Info.SmartCandidatesPerGen);

            NextGen.AddRange(EvolveCand.EvolveCandidates(EvolvedCopys));
            NextGen.AddRange(SmartCand.SmartifyCandidates(SmartCopys, BestCand, OldCand));
            NextGen.AddRange(RandomCand.RandomCandidates(Info.CandidatesPerGen * Info.RandomCandidatesPerGen, ref EquationStorage));
            return NextGen;
        }

        private List<Equation> CreateCopys(Equation BestCand, double CopysToCreate)
        {
            List<Equation> Cands = new List<Equation>((int)CopysToCreate);
            for (int i = 0; i < CopysToCreate; i++)
            {
                Cands.Add(BestCand.GetClone(EquationStorage.Pop()));
            }
            return Cands;
        }

        public void StoreCandidatesAndOperators(List<Equation> NextGen, Equation NewGenBestCand)
        {
            if (NewGenBestCand != null)
            {
                NextGen.Remove(NewGenBestCand);
            }
            foreach (Equation Cand in NextGen)
            {
                StoreSingle(Cand);
            }
        }

        public void StoreSingle(Equation Cand)
        {
            Cand.StoreAndCleanup();
            EquationStorage.Push(Cand);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Solve_Funktion
{
    public abstract class Genome
    {
        public Equation BestCandidate;
        public SpeciesInfo SpecInfo;
        public GeneralInfo GInfo;
        public SpecieEnviromentBase SpecEnviroment;
        protected string ShouldBeNumbers = String.Empty;
        public object UIUpdateLocker = new object();
        public event SpecieCreatedEventHandler OnSpecieCreated;

        public virtual void Startup(SpecieEnviromentBase Env, GeneralInfo ginfo)
        {
            SpecEnviroment = Env;
            GInfo = ginfo;
        }

        public abstract Genome EvolveSolution();

        public virtual void StartFinding()
        {
            SpecInfo = new SpeciesInfo();
            SynchronizedRandom.CreateRandom();
            GInfo.IncrementTotalSpecies();
            InitializeUpdateInfo();
            if (OnSpecieCreated != null)
            {
                OnSpecieCreated(new SpecieCreatedEventArgs() { SpecInfo = SpecInfo });
            }
        }

        public virtual void UpdateInfo()
        {
            lock (UIUpdateLocker)
            {
                SpecInfo.FunctionText = BestCandidate.CreateFunction();
                SpecInfo.Offset = BestCandidate.OffSet;
                SpecInfo.ResultText = String.Join(", ", BestCandidate.GetFunctionResults());
                SpecInfo.Attempts += Info.CandidatesPerGen;
                SpecInfo.Generation++;
                SpecInfo.OperatorCount = BestCandidate.AllOperators.Count;
                GInfo.AddTotalAttempts((long)Info.CandidatesPerGen);
                SpecEnviroment.CheckBestCandidate();
            }
        }

        protected void InitializeUpdateInfo()
        {
                IEnumerable<string> SeqText = (from x in Info.Seq
                                               select x.Y.ToString(Info.SRounding));
                ShouldBeNumbers = String.Join(", ", SeqText);
                SpecInfo.SequenceText = ShouldBeNumbers;
        }

        public void ResetEquations(Equation[] NextGen)
        {
            foreach (Equation Cand in NextGen)
            {
                ResetSingle(Cand);
            }
        }

        public void ResetSingle(Equation Cand)
        {
            Cand.Cleanup();
        }
    }
}
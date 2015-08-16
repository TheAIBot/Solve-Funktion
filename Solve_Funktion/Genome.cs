using System;
using System.Collections.Generic;
using System.Linq;

namespace Solve_Funktion
{
    public abstract class Genome : IDisposable
    {
        public Equation BestCandidate;
        public SpeciesInfo SpecInfo;
        public GeneralInfo GInfo;
        public EvolutionInfo EInfo;
        public SpecieEnviromentBase SpecEnviroment;
        public event SpecieCreatedEventHandler OnSpecieCreated;

        public virtual void Startup(SpecieEnviromentBase Env, GeneralInfo ginfo, EvolutionInfo einfo)
        {
            SpecEnviroment = Env;
            GInfo = ginfo;
            EInfo = einfo;
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
                OnSpecieCreated(new SpecieCreatedEventArgs() 
                { 
                    SpecInfo = SpecInfo
                });
            }
        }

        public virtual void UpdateInfo()
        {
            SpecInfo.FunctionText = BestCandidate.CreateFunction();
            SpecInfo.Offset = BestCandidate.OffSet;
            SpecInfo.ResultText = String.Join(", ", BestCandidate.GetFunctionResults());
            SpecInfo.Attempts += EInfo.CandidatesPerGen;
            SpecInfo.Generation++;
            SpecInfo.OperatorCount = BestCandidate.AllOperators.Count;
            GInfo.AddTotalAttempts((long)EInfo.CandidatesPerGen);
            SpecEnviroment.CheckBestCandidate();
        }

        protected void InitializeUpdateInfo()
        {
            IEnumerable<string> SeqText = (from x in EInfo.Goal
                                               select x.Y.ToString(Info.SRounding));
                SpecInfo.SequenceText = String.Join(", ", SeqText);
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

        public void Dispose()
        {
            if (SpecInfo != null)
            {
                SpecInfo.Dispose();
            }
        }
    }
}
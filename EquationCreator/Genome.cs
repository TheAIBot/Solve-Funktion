using System;
using System.Collections.Generic;
using System.Linq;

namespace EquationCreator
{
    public abstract class Genome
    {
        /*
        this class contains the basic setup to use an evolutionary approach to finding the best equation
        Solve, fuktion has multiple evolutionary approaches to finding the best equation and they all share this class as their base class
        This abstract class is not supposed to define what approach to use but instead allow multiple ays of doing it by allowing other classes to build upon this one
        */
        public Equation BestCandidate; // should contain the equation that fits the given points best
        public SpeciesInfo SpecInfo = new SpeciesInfo(); //contains information about the BestCandidate that can be shown in a GUI
        public GeneralInfo GInfo; // contains information about all the different species running at the same time
        public EvolutionInfo EInfo; // contains information about the parameters this evolutionary approach should use
        public SpecieEnviromentBase SpecEnviroment; // this is the enviroment this specie is living in
        public int _toCalc = 3; // amount of points that should be used to calculate the offset of an equation. min should be 3
        public event SpecieCreatedEventHandler OnSpecieCreated; // event is called when a specie is created
        public readonly SynchronizedRandom Randomizer = new SynchronizedRandom();

        public void StartSetup(SpecieEnviromentBase SE, GeneralInfo G, EvolutionInfo E)
        {
            SpecEnviroment = SE;
            GInfo = G;
            EInfo = E;
        }

        /// <summary>
        /// evolves the best equation to find better equations
        /// </summary>
        /// <returns> itself</returns>
        public abstract Genome EvolveSolution();

        /// <summary>
        /// is called before solution is evolved to setup the specie
        /// </summary>
        protected virtual void StartFinding()
        {
            lock (SpecInfo)
            {
                SpecInfo = new SpeciesInfo();
            }
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

        /// <summary>
        /// updates SpecInfo with the newest information about BestCandidate
        /// </summary>
        protected virtual void UpdateInfo()
        {
            SpecInfo.FunctionText = BestCandidate.CreateFunction();
            SpecInfo.Offset = BestCandidate.OffSet;
            SpecInfo.ResultText = String.Join(", ", BestCandidate.GetFunctionResults());
            SpecInfo.Attempts += EInfo.CandidatesPerGen;
            SpecInfo.Generation++;
            SpecInfo.OperatorCount = BestCandidate.NumberOfAllOperators;
            GInfo.AddTotalAttempts((long)EInfo.CandidatesPerGen);
            SpecEnviroment.CheckBestCandidate();
        }

        /// <summary>
        /// information only needed to be set once in SpecInfo when the evolution begins
        /// </summary>
        protected void InitializeUpdateInfo()
        {
            SpecInfo.SequenceText = String.Join(", ", EInfo.coordInfo.expectedResults.Select(x => x.ToString("N2")));
        }

        /// <summary>
        /// resets equations making them ready to create a new ones
        /// </summary>
        /// <param name="NextGen">list of equations to reset</param>
        protected void ResetEquations(Equation[] NextGen)
        {
            foreach (Equation Cand in NextGen)
            {
                ResetSingle(Cand);
            }
        }

        /// <summary>
        /// resets a singl equation making it ready to create a new one
        /// </summary>
        /// <param name="Cand"></param>
        protected void ResetSingle(Equation Cand)
        {
            Cand.Cleanup();
        }
    }
}
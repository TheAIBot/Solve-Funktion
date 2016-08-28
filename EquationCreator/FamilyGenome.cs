using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationCreator
{
    public abstract class FamilyGenome : Genome
    {
        public SpecieEnviromentBase<FamilyGenome> SpecEnviroment; // this is the enviroment this specie is living in
        public Equation child;
        public void StartSetup(SpecieEnviromentBase<FamilyGenome> SE, GeneralInfo G, EvolutionInfo E)
        {
            SpecEnviroment = SE;
            GInfo = G;
            EInfo = E;
            child = new Equation(EInfo, Randomizer);
            BestCandidate = new Equation(EInfo, Randomizer);
        }

        protected virtual void StartFinding()
        {
            GInfo.IncrementTotalSpecies();
            NewSpecieEvent();
        }

        public abstract Family EvolveFamily(Family family);
    }
}

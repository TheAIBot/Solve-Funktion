using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationCreator
{
    public abstract class SingleGenome : Genome
    {
        public SpecieEnviromentBase<SingleGenome> SpecEnviroment; // this is the enviroment this specie is living in

        public void StartSetup(SpecieEnviromentBase<SingleGenome> SE, GeneralInfo G, EvolutionInfo E)
        {
            SpecEnviroment = SE;
            GInfo = G;
            EInfo = E;
        }

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
            NewSpecieEvent();
        }

        public abstract Genome EvolveSpecie();
    }
}

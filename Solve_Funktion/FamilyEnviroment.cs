using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solve_Funktion
{
    public class FamilyEnviroment<T> : SpecieEnviromentBase where T : Genome, new()
    {
        public override GeneralInfo SetupEviroment(EvolutionInfo einfo)
        {
            if (einfo.SpeciesAmount % 2 != 0)
            {
                throw new Exception("Number is not divicible by 2");
            }
            EInfo = einfo;
            Species = new Genome[EInfo.SpeciesAmount];
            GInfo = new GeneralInfo();
            for (int i = 0; i < EInfo.SpeciesAmount; i++)
            {
                Species[i] = new T();
                Species[i].StartSetup(this, GInfo, EInfo);
                DoSubscribeEvent(Species[i]);
            }
            return GInfo;
        }

        public override void SimulateEnviroment()
        {
            while (true)
            {
                Parallel.For(0, Species.Length, (i) => Species[i].EvolveSolution());
            }
        }

        private void MixSpecies(Genome Mother, Genome Father)
        {
            Equation EBrother;
            Equation ESister;
            do
            {
                EBrother = Tools.DeepCopy<Equation>(Father.BestCandidate);
                ESister = Tools.DeepCopy<Equation>(Mother.BestCandidate);

            } while (Tools.IsANumber(Mother.BestCandidate.OffSet) && 
                     Tools.IsANumber(Father.BestCandidate.OffSet));
        }

        private void SuperMix(Equation E1, Equation E2)
        {
            for (int i = 0; i < E1.Holders.Count; i++)
            {
                //if (SynchronizedRandom.RandomBool())
                //{
                    
                //}
            }
        }
    }
}
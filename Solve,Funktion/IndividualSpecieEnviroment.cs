using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solve_Funktion
{
    public class IndividualSpecieEnviroment<T> : SpecieEnviromentBase/*<T>*/where T : Genome, new()
    {
        public override GeneralInfo SetupEviroment(int SpecieCount)
        {
            Species = new Genome[SpecieCount];
            GInfo = new GeneralInfo();
            for (int i = 0; i < SpecieCount; i++)
            {
                Species[i] = new T();
                Species[i].Startup(this, GInfo);
                DoSubscribeEvent(Species[i]);
            }
            return GInfo;
        }

        public override void SimulateEnviroment()
        {
            Parallel.For(0, Species.Length, (i, LoopState) =>
            {
                Genome FinishedSpecie;
                do
                {
                    FinishedSpecie = Species[i].EvolveSolution();
                } while (FinishedSpecie.BestCandidate.OffSet != 0);
                LoopState.Stop();
            });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Solve_Funktion
{
    public class IndividualSpecieEnviroment<T> : SpecieEnviromentBase where T : Genome, new()
    {
        public override GeneralInfo SetupEviroment(EvolutionInfo einfo)
        {
            EInfo = einfo;
            Species = new Genome[EInfo.SpeciesAmount];
            GInfo = new GeneralInfo();
            for (int i = 0; i < EInfo.SpeciesAmount; i++)
            {
                Species[i] = new T();
                Species[i].Startup(this, GInfo, einfo);
                DoSubscribeEvent(Species[i]);
            }
            return GInfo;
        }

        public override void SimulateEnviroment()
        {
            Parallel.For(0, Species.Length, (i, LoopState) =>
            {
                TryAgain:
                try
                {
                    Genome FinishedSpecie;
                    do
                    {
                        FinishedSpecie = Species[i].EvolveSolution();
                    } while (FinishedSpecie.BestCandidate.OffSet != 0);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace);
                    goto TryAgain;
                }
            });
        }
    }
}
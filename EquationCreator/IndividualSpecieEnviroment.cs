using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EquationCreator
{
    public class IndividualSpecieEnviroment<T> : SpecieEnviromentBase<SingleGenome> where T : SingleGenome, new()
    {
        public override GeneralInfo SetupEviroment(EvolutionInfo einfo)
        {
            EInfo = einfo;
            Species = new SingleGenome[EInfo.SpeciesAmount];
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
            Exception error = null;
            Parallel.For(0, Species.Length, (i, LoopState) =>
            {
                try
                {
                    Genome FinishedSpecie;
                    do
                    {
                        FinishedSpecie = Species[i].EvolveSpecie();
                        CheckBestCandidate(FinishedSpecie.SpecInfo.GetCopy());
                    } while (FinishedSpecie.BestCandidate.OffSet != 0);
                }
                catch (Exception e)
                {
                    error = e;
                    LoopState.Break();
                }
            });
            if (error != null)
            {
                ExceptionDispatchInfo.Capture(error).Throw();
            }
        }
    }
}
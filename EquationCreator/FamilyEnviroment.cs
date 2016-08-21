using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.ExceptionServices;

namespace EquationCreator
{
    public class FamilyEnviroment<T> : SpecieEnviromentBase<FamilyGenome> where T : FamilyGenome, new()
    {
        public override GeneralInfo SetupEviroment(EvolutionInfo einfo)
        {
            EInfo = einfo;
            Species = new FamilyGenome[EInfo.SpeciesAmount];
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
            const int NUMBER_OF_PARENTS = 2; // need to be changeable
            Equation[] parents = new Equation[NUMBER_OF_PARENTS];
            for (int i = 0; i < parents.Length; i++)
            {
                parents[i] = new Equation(EInfo, new SynchronizedRandom());
            }
            while (true)
            {
                for (int i = 0; i < Species.Length; i++)
                {
                    RandomCand.MakeValidRandomEquation(Species[i].child);
                }
                for (int i = 0; i < parents.Length; i++)
                {
                    parents[i] = Species[i].child.MakeClone(parents[i]);
                }
                bool takeBest = true;
                while (true)
                {
                    Exception error = null;
                    Parallel.For(0, Species.Length, (i, LoopState) =>
                    {
                        try
                        {
                            Genome FinishedSpecie = Species[i].EvolveFamily(parents);
                            CheckBestCandidate(FinishedSpecie.SpecInfo.GetCopy());
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
                    Equation[] bestEquations = Species.Select(x => x.BestCandidate).OrderBy(x => x.OffSet).ToArray();
                    if (takeBest)
                    {
                        for (int i = 0; i < parents.Length; i++)
                        {
                            parents[i].Cleanup();
                            parents[i] = bestEquations[i].MakeClone(parents[i]);
                            parents[i].Compress(parents[i]);
                        }
                        takeBest = false;
                    }
                    else
                    {
                        for (int i = parents.Length - 1; i >= 0; i--)
                        {
                            parents[i].Cleanup();
                            parents[i] = bestEquations[i].MakeClone(parents[i]);
                            parents[i].Compress(parents[i]);
                        }
                        takeBest = true;
                    }
                    
                    //parents[parents.Length - 1].Cleanup();
                    //RandomCand.MakeValidRandomEquation(parents[parents.Length - 1]);
                    //parents[parents.Length - 1] = bestEquations[parents.Length - 1].MakeClone(parents[parents.Length - 1]);
                    //parents[parents.Length - 1].Compress(parents[parents.Length - 1]);
                }
            }
        }
    }
}
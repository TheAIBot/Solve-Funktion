using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.ExceptionServices;
using System.Collections.Concurrent;

namespace EquationCreator
{
    public class FamilyEnviroment<T> : SpecieEnviromentBase<FamilyGenome> where T : FamilyGenome, new()
    {
        public const int PARENT_COUNT = 5;



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
            ConcurrentQueue<Family> families = new ConcurrentQueue<Family>();
            ConcurrentQueue<Equation> parents = new ConcurrentQueue<Equation>();
            for (int i = 0; i < EInfo.SpeciesAmount; i++)
            {
                Family newFamily = new Family(EInfo, PARENT_COUNT);
                for (int y = 0; y < PARENT_COUNT; y++)
                {
                    parents.Enqueue(newFamily.parents[y]);
                    newFamily.parents[y] = null;
                }
                families.Enqueue(newFamily);
            }
            while (true)
            {
                ConcurrentQueue<Family> processedFamilies = new ConcurrentQueue<Family>();

                foreach (Family family in families)
                {
                    for (int i = 0; i < PARENT_COUNT; i++)
                    {
                        Equation parent = null;
                        if (!parents.TryDequeue(out parent))
                        {
                            throw new NullReferenceException("parent was null");
                        }
                        family.parents[i] = parent;
                    }
                }
                Exception error = null;
                Parallel.For(0, EInfo.SpeciesAmount, (z, loopState) =>
                {
                    try
                    {
                        Family family = null;
                        if (!families.TryDequeue(out family))
                        {
                            throw new NullReferenceException("family was null");
                        }

                        Species[z].EvolveFamily(family);
                        Species[z].BestCandidate.Cleanup();
                        if (Tools.IsANumber(family.children[family.children.Length - 1].OffSet))
                        {
                            family.children[family.children.Length - 1].MakeClone(Species[z].BestCandidate);
                        }
                        family.MakeChildrenParents();
                        processedFamilies.Enqueue(family);
                        for (int i = 0; i < PARENT_COUNT; i++)
                        {
                            if (family.parents[i].NumberOfAllOperators == 0)
                            {

                            }
                            parents.Enqueue(family.parents[i]);
                            family.parents[i] = null;
                        }
                        CheckBestCandidate(Species[z].SpecInfo.GetCopy());
                    }
                    catch (Exception e)
                    {
                        error = e;
                        loopState.Break();
                    }
                });
                if (error != null)
                {
                    ExceptionDispatchInfo.Capture(error).Throw();
                }
                foreach (Family family in processedFamilies)
                {
                    families.Enqueue(family);
                }
                List<Equation> sortedParents = parents.ToList().OrderBy(x => x.OffSet).ToList();
                //const double SURVIVAL_RATE = 0.01;
                //for (int i = (int)((double)EInfo.SpeciesAmount * (1 - SURVIVAL_RATE)); i < EInfo.SpeciesAmount; i++)
                //{
                //    sortedParents[i].Cleanup();
                //    RandomCand.MakeValidRandomEquation(sortedParents[i]);
                //}
                parents = new ConcurrentQueue<Equation>();
                sortedParents.ForEach(x => parents.Enqueue(x));
            }
        }
    }
}
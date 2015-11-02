using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Solve_Funktion
{
    /*
    enviroments are used to control interaction between different species
    this not only allows species to evolve by them selfs but also allows
    them to use each other to evolve if that is what the evolutionary approach aims to do
    */
    public abstract class SpecieEnviromentBase//<T> where T : Genome, new()
    {
        public event BestEquationEventHandler OnBestEquationChanged;
        public event SubscribeEventEventHandler OnSubscribeToSpecies;
        protected EvolutionInfo EInfo;
        protected SpeciesInfo BestEquationInfo;
        protected GeneralInfo GInfo;
        protected Genome[] Species;

        /// <summary>
        /// prepares the enviroment with its evolution parameters
        /// </summary>
        /// <param name="EInfo">evolution parameters</param>
        /// <returns>shared info about species</returns>
        public abstract GeneralInfo SetupEviroment(EvolutionInfo EInfo);

        public abstract void SimulateEnviroment();

        public void DoSubscribeEvent(Genome Spec)
        {
            if (OnSubscribeToSpecies != null)
            {
                OnSubscribeToSpecies(new SubscribeEventEventArgs() { Specie = Spec });
            }
        }

        public void CheckBestCandidate()
        {
            if (OnBestEquationChanged != null)
            {
                Genome[] SpecInfos = Species.Where(x => x.BestCandidate != null && Tools.IsANumber(x.BestCandidate.OffSet))
                                        .OrderByDescending(x => x._toCalc)
                                        .ThenBy(x => x.BestCandidate.OffSet)
                                        .ThenByDescending(x => x.BestCandidate.OperatorsLeft).ToArray();
                if (SpecInfos.Length > 0)
                {
                    if (BestEquationInfo == null ||
                        BestEquationInfo.Offset > SpecInfos[0].SpecInfo.Offset &&
                        BestEquationInfo.toCalc <= SpecInfos[0].SpecInfo.toCalc || 
                        BestEquationInfo.Offset == SpecInfos[0].SpecInfo.Offset && 
                        BestEquationInfo.OperatorCount > SpecInfos[0].SpecInfo.OperatorCount &&
                        BestEquationInfo.toCalc <= SpecInfos[0].SpecInfo.toCalc)
                    {
                        SpeciesInfo SpecInfo = SpecInfos[0].SpecInfo;
                        OnBestEquationChanged(new BestEquationEventArgs
                        {
                            BestEquationInfo = SpecInfo
                        });
                    }
                }
            }
        }
    }

    public delegate void BestEquationEventHandler(BestEquationEventArgs e);
    public delegate void SpecieCreatedEventHandler(SpecieCreatedEventArgs e);
    public delegate void SubscribeEventEventHandler(SubscribeEventEventArgs e);

    public class BestEquationEventArgs : EventArgs
    {
        public SpeciesInfo BestEquationInfo;
    }
    public class SpecieCreatedEventArgs : EventArgs
    {
        public SpeciesInfo SpecInfo;
    }
    public class SubscribeEventEventArgs : EventArgs
    {
        public GeneralInfo GInfo;
        public Genome Specie;
    }

    public sealed class VectorPoint
    {
        public Vector<double>[] Parameters;
        public string[] ParameterNames;
        public Vector<double> Result;
        public int Count;

        public VectorPoint(Vector<double>[] parameters, string[] parameterNames, Vector<double> result, int count)
        {
            Parameters = parameters;
            ParameterNames = parameterNames;
            Result = result;
            Count = count;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationCreator
{
    /*
    enviroments are used to control interaction between different species
    this not only allows species to evolve by them selfs but also allows
    them to use each other to evolve if that is what the evolutionary approach aims to do
    */
    public abstract class SpecieEnviromentBase<T>
    {
        public event BestEquationEventHandler OnBestEquationChanged;
        public event SubscribeEventEventHandler OnSubscribeToSpecies;
        protected EvolutionInfo EInfo;
        protected SpeciesInfo BestEquationInfo;
        protected GeneralInfo GInfo;
        public T[] Species;

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

        private object checkEquationLocker = new object();

        public void CheckBestCandidate(SpeciesInfo specInfo)
        {
            OnBestEquationChanged?.Invoke(new BestEquationEventArgs
            {
                BestEquationInfo = specInfo
            });
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

    
}
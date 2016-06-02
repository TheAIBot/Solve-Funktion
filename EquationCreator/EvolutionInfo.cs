using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EquationCreator
{
    public class EvolutionInfo
    {
        public readonly CoordInfo coordInfo;
        public readonly int MaxSize;
        public readonly double MaxChange;
        public readonly double CandidatesPerGen;
        public readonly int NumberRangeMax;
        public readonly int NumberRangeMin;
        public readonly int SpeciesAmount;
        public readonly int MaxStuckGens;
        public readonly double EvolvedCandidatesPerGen;
        public readonly double RandomCandidatesPerGen;
        public readonly double SmartCandidatesPerGen;
        public readonly MathFunction[] Operators;
        public readonly Connector[] Connectors;

        public EvolutionInfo(CoordInfo coordInfoo, 
                             int maxsize,
                             double maxchange, 
                             double candidatespergen, 
                             int numberrangemax, 
                             int numberrangemin,
                             int speciesamount, 
                             int maxstuckgens, 
                             double evolvedcandidatespergen, 
                             double randomcandidatespergen,
                             double smartcandidatespergen, 
                             MathFunction[] operators)
        {
            coordInfo = coordInfoo;
            MaxSize = maxsize;
            MaxChange = maxchange;
            CandidatesPerGen = candidatespergen;
            NumberRangeMax = numberrangemax;
            NumberRangeMin = numberrangemin;
            SpeciesAmount = speciesamount;
            MaxStuckGens = maxstuckgens;
            EvolvedCandidatesPerGen = evolvedcandidatespergen;
            RandomCandidatesPerGen = randomcandidatespergen;
            SmartCandidatesPerGen = smartcandidatespergen;
            Operators = operators;
            Connectors = operators.OfType<Connector>().ToArray();
        }
    }
}

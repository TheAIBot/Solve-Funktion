using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Solve_Funktion
{
    public class EvolutionInfo
    {
        public readonly VectorPoint[] Goal;
        public readonly int GoalLength;
        public readonly int MaxSize;
        public readonly int MaxChange;
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

        public EvolutionInfo(VectorPoint[] goal, 
                             int maxsize, 
                             int maxchange, 
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
            Goal = goal;
            GoalLength = goal.Sum(x => x.Count);
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

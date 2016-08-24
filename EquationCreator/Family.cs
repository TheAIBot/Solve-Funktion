using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationCreator
{
    internal class Family
    {
        public readonly Equation[] parents;
        public readonly Equation[] children;

        public Family(EvolutionInfo eInfo, int parentCount)
        {
            parents = new Equation[parentCount];
            children = new Equation[parentCount];

            SynchronizedRandom random = new SynchronizedRandom();
            
            for (int i = 0; i < parents.Length; i++)
            {
                parents[i] = new Equation(eInfo, random);
            }
        }

        internal void CheckNewChild(Equation child)
        {
            throw new NotImplementedException();
        }

        internal void MakeChildrenParents()
        {
            throw new NotImplementedException();
        }
    }
}
